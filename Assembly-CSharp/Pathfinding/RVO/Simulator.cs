using System;
using System.Collections.Generic;
using System.Threading;
using Pathfinding.RVO.Sampled;
using UnityEngine;

namespace Pathfinding.RVO
{
	public class Simulator
	{
		public Simulator(int workers, bool doubleBuffering)
		{
			this.workers = new global::Pathfinding.RVO.Simulator.Worker[workers];
			this.doubleBuffering = doubleBuffering;
			this.DesiredDeltaTime = 1f;
			this.Quadtree = new global::Pathfinding.RVO.RVOQuadtree();
			for (int i = 0; i < workers; i++)
			{
				this.workers[i] = new global::Pathfinding.RVO.Simulator.Worker(this);
			}
			this.agents = new global::System.Collections.Generic.List<global::Pathfinding.RVO.Sampled.Agent>();
			this.obstacles = new global::System.Collections.Generic.List<global::Pathfinding.RVO.ObstacleVertex>();
		}

		public global::Pathfinding.RVO.RVOQuadtree Quadtree { get; private set; }

		public float DeltaTime
		{
			get
			{
				return this.deltaTime;
			}
		}

		public bool Multithreading
		{
			get
			{
				return this.workers != null && this.workers.Length > 0;
			}
		}

		public float DesiredDeltaTime
		{
			get
			{
				return this.desiredDeltaTime;
			}
			set
			{
				this.desiredDeltaTime = global::System.Math.Max(value, 0f);
			}
		}

		public global::System.Collections.Generic.List<global::Pathfinding.RVO.Sampled.Agent> GetAgents()
		{
			return this.agents;
		}

		public global::System.Collections.Generic.List<global::Pathfinding.RVO.ObstacleVertex> GetObstacles()
		{
			return this.obstacles;
		}

		public void ClearAgents()
		{
			this.BlockUntilSimulationStepIsDone();
			for (int i = 0; i < this.agents.Count; i++)
			{
				this.agents[i].simulator = null;
			}
			this.agents.Clear();
		}

		public void OnDestroy()
		{
			if (this.workers != null)
			{
				for (int i = 0; i < this.workers.Length; i++)
				{
					this.workers[i].Terminate();
				}
			}
		}

		~Simulator()
		{
			this.OnDestroy();
		}

		public global::Pathfinding.RVO.IAgent AddAgent(global::Pathfinding.RVO.IAgent agent)
		{
			if (agent == null)
			{
				throw new global::System.ArgumentNullException("Agent must not be null");
			}
			global::Pathfinding.RVO.Sampled.Agent agent2 = agent as global::Pathfinding.RVO.Sampled.Agent;
			if (agent2 == null)
			{
				throw new global::System.ArgumentException("The agent must be of type Agent. Agent was of type " + agent.GetType());
			}
			if (agent2.simulator != null && agent2.simulator == this)
			{
				throw new global::System.ArgumentException("The agent is already in the simulation");
			}
			if (agent2.simulator != null)
			{
				throw new global::System.ArgumentException("The agent is already added to another simulation");
			}
			agent2.simulator = this;
			this.BlockUntilSimulationStepIsDone();
			this.agents.Add(agent2);
			return agent;
		}

		[global::System.Obsolete("Use AddAgent(Vector2,float) instead")]
		public global::Pathfinding.RVO.IAgent AddAgent(global::UnityEngine.Vector3 position)
		{
			return this.AddAgent(new global::UnityEngine.Vector2(position.x, position.z), position.y);
		}

		public global::Pathfinding.RVO.IAgent AddAgent(global::UnityEngine.Vector2 position, float elevationCoordinate)
		{
			return this.AddAgent(new global::Pathfinding.RVO.Sampled.Agent(position, elevationCoordinate));
		}

		public void RemoveAgent(global::Pathfinding.RVO.IAgent agent)
		{
			if (agent == null)
			{
				throw new global::System.ArgumentNullException("Agent must not be null");
			}
			global::Pathfinding.RVO.Sampled.Agent agent2 = agent as global::Pathfinding.RVO.Sampled.Agent;
			if (agent2 == null)
			{
				throw new global::System.ArgumentException("The agent must be of type Agent. Agent was of type " + agent.GetType());
			}
			if (agent2.simulator != this)
			{
				throw new global::System.ArgumentException("The agent is not added to this simulation");
			}
			this.BlockUntilSimulationStepIsDone();
			agent2.simulator = null;
			if (!this.agents.Remove(agent2))
			{
				throw new global::System.ArgumentException("Critical Bug! This should not happen. Please report this.");
			}
		}

		public global::Pathfinding.RVO.ObstacleVertex AddObstacle(global::Pathfinding.RVO.ObstacleVertex v)
		{
			if (v == null)
			{
				throw new global::System.ArgumentNullException("Obstacle must not be null");
			}
			this.BlockUntilSimulationStepIsDone();
			this.obstacles.Add(v);
			this.UpdateObstacles();
			return v;
		}

		public global::Pathfinding.RVO.ObstacleVertex AddObstacle(global::UnityEngine.Vector3[] vertices, float height, bool cycle = true)
		{
			return this.AddObstacle(vertices, height, global::UnityEngine.Matrix4x4.identity, global::Pathfinding.RVO.RVOLayer.DefaultObstacle, cycle);
		}

		public global::Pathfinding.RVO.ObstacleVertex AddObstacle(global::UnityEngine.Vector3[] vertices, float height, global::UnityEngine.Matrix4x4 matrix, global::Pathfinding.RVO.RVOLayer layer = global::Pathfinding.RVO.RVOLayer.DefaultObstacle, bool cycle = true)
		{
			if (vertices == null)
			{
				throw new global::System.ArgumentNullException("Vertices must not be null");
			}
			if (vertices.Length < 2)
			{
				throw new global::System.ArgumentException("Less than 2 vertices in an obstacle");
			}
			global::Pathfinding.RVO.ObstacleVertex obstacleVertex = null;
			global::Pathfinding.RVO.ObstacleVertex obstacleVertex2 = null;
			this.BlockUntilSimulationStepIsDone();
			for (int i = 0; i < vertices.Length; i++)
			{
				global::Pathfinding.RVO.ObstacleVertex obstacleVertex3 = new global::Pathfinding.RVO.ObstacleVertex
				{
					prev = obstacleVertex2,
					layer = layer,
					height = height
				};
				if (obstacleVertex == null)
				{
					obstacleVertex = obstacleVertex3;
				}
				else
				{
					obstacleVertex2.next = obstacleVertex3;
				}
				obstacleVertex2 = obstacleVertex3;
			}
			if (cycle)
			{
				obstacleVertex2.next = obstacleVertex;
				obstacleVertex.prev = obstacleVertex2;
			}
			this.UpdateObstacle(obstacleVertex, vertices, matrix);
			this.obstacles.Add(obstacleVertex);
			return obstacleVertex;
		}

		public global::Pathfinding.RVO.ObstacleVertex AddObstacle(global::UnityEngine.Vector3 a, global::UnityEngine.Vector3 b, float height)
		{
			global::Pathfinding.RVO.ObstacleVertex obstacleVertex = new global::Pathfinding.RVO.ObstacleVertex();
			global::Pathfinding.RVO.ObstacleVertex obstacleVertex2 = new global::Pathfinding.RVO.ObstacleVertex();
			obstacleVertex.layer = global::Pathfinding.RVO.RVOLayer.DefaultObstacle;
			obstacleVertex2.layer = global::Pathfinding.RVO.RVOLayer.DefaultObstacle;
			obstacleVertex.prev = obstacleVertex2;
			obstacleVertex2.prev = obstacleVertex;
			obstacleVertex.next = obstacleVertex2;
			obstacleVertex2.next = obstacleVertex;
			obstacleVertex.position = a;
			obstacleVertex2.position = b;
			obstacleVertex.height = height;
			obstacleVertex2.height = height;
			obstacleVertex2.ignore = true;
			global::Pathfinding.RVO.ObstacleVertex obstacleVertex3 = obstacleVertex;
			global::UnityEngine.Vector2 vector = new global::UnityEngine.Vector2(b.x - a.x, b.z - a.z);
			obstacleVertex3.dir = vector.normalized;
			obstacleVertex2.dir = -obstacleVertex.dir;
			this.BlockUntilSimulationStepIsDone();
			this.obstacles.Add(obstacleVertex);
			this.UpdateObstacles();
			return obstacleVertex;
		}

		public void UpdateObstacle(global::Pathfinding.RVO.ObstacleVertex obstacle, global::UnityEngine.Vector3[] vertices, global::UnityEngine.Matrix4x4 matrix)
		{
			if (vertices == null)
			{
				throw new global::System.ArgumentNullException("Vertices must not be null");
			}
			if (obstacle == null)
			{
				throw new global::System.ArgumentNullException("Obstacle must not be null");
			}
			if (vertices.Length < 2)
			{
				throw new global::System.ArgumentException("Less than 2 vertices in an obstacle");
			}
			bool flag = matrix == global::UnityEngine.Matrix4x4.identity;
			this.BlockUntilSimulationStepIsDone();
			int i = 0;
			global::Pathfinding.RVO.ObstacleVertex obstacleVertex = obstacle;
			while (i < vertices.Length)
			{
				obstacleVertex.position = ((!flag) ? matrix.MultiplyPoint3x4(vertices[i]) : vertices[i]);
				obstacleVertex = obstacleVertex.next;
				i++;
				if (obstacleVertex == obstacle || obstacleVertex == null)
				{
					obstacleVertex = obstacle;
					do
					{
						if (obstacleVertex.next == null)
						{
							obstacleVertex.dir = global::UnityEngine.Vector2.zero;
						}
						else
						{
							global::UnityEngine.Vector3 vector = obstacleVertex.next.position - obstacleVertex.position;
							global::Pathfinding.RVO.ObstacleVertex obstacleVertex2 = obstacleVertex;
							global::UnityEngine.Vector2 vector2 = new global::UnityEngine.Vector2(vector.x, vector.z);
							obstacleVertex2.dir = vector2.normalized;
						}
						obstacleVertex = obstacleVertex.next;
					}
					while (obstacleVertex != obstacle && obstacleVertex != null);
					this.ScheduleCleanObstacles();
					this.UpdateObstacles();
					return;
				}
			}
			global::UnityEngine.Debug.DrawLine(obstacleVertex.prev.position, obstacleVertex.position, global::UnityEngine.Color.red);
			throw new global::System.ArgumentException("Obstacle has more vertices than supplied for updating (" + vertices.Length + " supplied)");
		}

		private void ScheduleCleanObstacles()
		{
			this.doCleanObstacles = true;
		}

		private void CleanObstacles()
		{
		}

		public void RemoveObstacle(global::Pathfinding.RVO.ObstacleVertex v)
		{
			if (v == null)
			{
				throw new global::System.ArgumentNullException("Vertex must not be null");
			}
			this.BlockUntilSimulationStepIsDone();
			this.obstacles.Remove(v);
			this.UpdateObstacles();
		}

		public void UpdateObstacles()
		{
			this.doUpdateObstacles = true;
		}

		private void BuildQuadtree()
		{
			this.Quadtree.Clear();
			if (this.agents.Count > 0)
			{
				global::UnityEngine.Rect bounds = global::UnityEngine.Rect.MinMaxRect(this.agents[0].position.x, this.agents[0].position.y, this.agents[0].position.x, this.agents[0].position.y);
				for (int i = 1; i < this.agents.Count; i++)
				{
					global::UnityEngine.Vector2 position = this.agents[i].position;
					bounds = global::UnityEngine.Rect.MinMaxRect(global::UnityEngine.Mathf.Min(bounds.xMin, position.x), global::UnityEngine.Mathf.Min(bounds.yMin, position.y), global::UnityEngine.Mathf.Max(bounds.xMax, position.x), global::UnityEngine.Mathf.Max(bounds.yMax, position.y));
				}
				this.Quadtree.SetBounds(bounds);
				for (int j = 0; j < this.agents.Count; j++)
				{
					this.Quadtree.Insert(this.agents[j]);
				}
			}
		}

		private void BlockUntilSimulationStepIsDone()
		{
			if (this.Multithreading && this.doubleBuffering)
			{
				for (int i = 0; i < this.workers.Length; i++)
				{
					this.workers[i].WaitOne();
				}
			}
		}

		private void PreCalculation()
		{
			for (int i = 0; i < this.agents.Count; i++)
			{
				this.agents[i].PreCalculation();
				if (this.agents[i].MovementMode != this.agents[0].MovementMode)
				{
					global::UnityEngine.Debug.LogError(string.Concat(new object[]
					{
						"RVO movement modes are inconsistent. Some are set to ",
						this.agents[i].MovementMode,
						" and some to ",
						this.agents[0].MovementMode,
						". Local avoidance will not work properly while this is the case."
					}));
					return;
				}
			}
		}

		private void CleanAndUpdateObstaclesIfNecessary()
		{
			if (this.doCleanObstacles)
			{
				this.CleanObstacles();
				this.doCleanObstacles = false;
				this.doUpdateObstacles = true;
			}
			if (this.doUpdateObstacles)
			{
				this.doUpdateObstacles = false;
			}
		}

		public void Update()
		{
			if (this.lastStep < 0f)
			{
				this.lastStep = global::UnityEngine.Time.time;
				this.deltaTime = this.DesiredDeltaTime;
			}
			if (global::UnityEngine.Time.time - this.lastStep >= this.DesiredDeltaTime)
			{
				this.deltaTime = global::UnityEngine.Time.time - this.lastStep;
				this.lastStep = global::UnityEngine.Time.time;
				this.deltaTime = global::System.Math.Max(this.deltaTime, 0.0005f);
				if (this.Multithreading)
				{
					if (this.doubleBuffering)
					{
						for (int i = 0; i < this.workers.Length; i++)
						{
							this.workers[i].WaitOne();
						}
						for (int j = 0; j < this.agents.Count; j++)
						{
							this.agents[j].PostCalculation();
						}
					}
					this.PreCalculation();
					this.CleanAndUpdateObstaclesIfNecessary();
					this.BuildQuadtree();
					for (int k = 0; k < this.workers.Length; k++)
					{
						this.workers[k].start = k * this.agents.Count / this.workers.Length;
						this.workers[k].end = (k + 1) * this.agents.Count / this.workers.Length;
					}
					for (int l = 0; l < this.workers.Length; l++)
					{
						this.workers[l].Execute(1);
					}
					for (int m = 0; m < this.workers.Length; m++)
					{
						this.workers[m].WaitOne();
					}
					for (int n = 0; n < this.workers.Length; n++)
					{
						this.workers[n].Execute(0);
					}
					if (!this.doubleBuffering)
					{
						for (int num = 0; num < this.workers.Length; num++)
						{
							this.workers[num].WaitOne();
						}
						for (int num2 = 0; num2 < this.agents.Count; num2++)
						{
							this.agents[num2].PostCalculation();
						}
					}
				}
				else
				{
					this.PreCalculation();
					this.CleanAndUpdateObstaclesIfNecessary();
					this.BuildQuadtree();
					for (int num3 = 0; num3 < this.agents.Count; num3++)
					{
						this.agents[num3].BufferSwitch();
					}
					for (int num4 = 0; num4 < this.agents.Count; num4++)
					{
						this.agents[num4].CalculateNeighbours();
						this.agents[num4].CalculateVelocity(this.coroutineWorkerContext);
					}
					for (int num5 = 0; num5 < this.agents.Count; num5++)
					{
						this.agents[num5].PostCalculation();
					}
				}
			}
		}

		private bool doubleBuffering = true;

		private float desiredDeltaTime = 0.05f;

		private global::Pathfinding.RVO.Simulator.Worker[] workers;

		private global::System.Collections.Generic.List<global::Pathfinding.RVO.Sampled.Agent> agents;

		public global::System.Collections.Generic.List<global::Pathfinding.RVO.ObstacleVertex> obstacles;

		private float deltaTime;

		private float lastStep = -99999f;

		private bool doUpdateObstacles;

		private bool doCleanObstacles;

		public float symmetryBreakingBias = 0.1f;

		private global::Pathfinding.RVO.Simulator.WorkerContext coroutineWorkerContext = new global::Pathfinding.RVO.Simulator.WorkerContext();

		internal class WorkerContext
		{
			public const int KeepCount = 3;

			public global::Pathfinding.RVO.Sampled.Agent.VOBuffer vos = new global::Pathfinding.RVO.Sampled.Agent.VOBuffer(16);

			public global::UnityEngine.Vector2[] bestPos = new global::UnityEngine.Vector2[3];

			public float[] bestSizes = new float[3];

			public float[] bestScores = new float[4];

			public global::UnityEngine.Vector2[] samplePos = new global::UnityEngine.Vector2[50];

			public float[] sampleSize = new float[50];
		}

		private class Worker
		{
			public Worker(global::Pathfinding.RVO.Simulator sim)
			{
				this.simulator = sim;
				new global::System.Threading.Thread(new global::System.Threading.ThreadStart(this.Run))
				{
					IsBackground = true,
					Name = "RVO Simulator Thread"
				}.Start();
			}

			public void Execute(int task)
			{
				this.task = task;
				this.waitFlag.Reset();
				this.runFlag.Set();
			}

			public void WaitOne()
			{
				this.waitFlag.WaitOne();
			}

			public void Terminate()
			{
				this.terminate = true;
			}

			public void Run()
			{
				this.runFlag.WaitOne();
				while (!this.terminate)
				{
					try
					{
						global::System.Collections.Generic.List<global::Pathfinding.RVO.Sampled.Agent> agents = this.simulator.GetAgents();
						if (this.task == 0)
						{
							for (int i = this.start; i < this.end; i++)
							{
								agents[i].CalculateNeighbours();
								agents[i].CalculateVelocity(this.context);
							}
						}
						else if (this.task == 1)
						{
							for (int j = this.start; j < this.end; j++)
							{
								agents[j].BufferSwitch();
							}
						}
						else
						{
							if (this.task != 2)
							{
								global::UnityEngine.Debug.LogError("Invalid Task Number: " + this.task);
								throw new global::System.Exception("Invalid Task Number: " + this.task);
							}
							this.simulator.BuildQuadtree();
						}
					}
					catch (global::System.Exception message)
					{
						global::UnityEngine.Debug.LogError(message);
					}
					this.waitFlag.Set();
					this.runFlag.WaitOne();
				}
			}

			public int start;

			public int end;

			private readonly global::System.Threading.AutoResetEvent runFlag = new global::System.Threading.AutoResetEvent(false);

			private readonly global::System.Threading.ManualResetEvent waitFlag = new global::System.Threading.ManualResetEvent(true);

			private readonly global::Pathfinding.RVO.Simulator simulator;

			private int task;

			private bool terminate;

			private global::Pathfinding.RVO.Simulator.WorkerContext context = new global::Pathfinding.RVO.Simulator.WorkerContext();
		}
	}
}
