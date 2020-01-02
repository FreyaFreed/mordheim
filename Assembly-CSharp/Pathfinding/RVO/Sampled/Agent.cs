using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding.RVO.Sampled
{
	public class Agent : global::Pathfinding.RVO.IAgent
	{
		public Agent(global::UnityEngine.Vector2 pos, float elevationCoordinate)
		{
			this.NeighbourDist = 15f;
			this.AgentTimeHorizon = 2f;
			this.ObstacleTimeHorizon = 2f;
			this.Height = 5f;
			this.Radius = 5f;
			this.MaxNeighbours = 10;
			this.Locked = false;
			this.Position = pos;
			this.ElevationCoordinate = elevationCoordinate;
			this.Layer = global::Pathfinding.RVO.RVOLayer.DefaultAgent;
			this.CollidesWith = (global::Pathfinding.RVO.RVOLayer)(-1);
			this.Priority = 0.5f;
			this.CalculatedTargetPoint = pos;
			this.CalculatedSpeed = 0f;
			this.SetTarget(pos, 0f, 0f);
		}

		public global::UnityEngine.Vector2 Position { get; set; }

		public float ElevationCoordinate { get; set; }

		public global::UnityEngine.Vector2 CalculatedTargetPoint { get; private set; }

		public float CalculatedSpeed { get; private set; }

		public bool Locked { get; set; }

		public float Radius { get; set; }

		public float Height { get; set; }

		public float NeighbourDist { get; set; }

		public float AgentTimeHorizon { get; set; }

		public float ObstacleTimeHorizon { get; set; }

		public int MaxNeighbours { get; set; }

		public int NeighbourCount { get; private set; }

		public global::Pathfinding.RVO.RVOLayer Layer { get; set; }

		public global::Pathfinding.RVO.RVOLayer CollidesWith { get; set; }

		public bool DebugDraw
		{
			get
			{
				return this.debugDraw;
			}
			set
			{
				this.debugDraw = (value && this.simulator != null && !this.simulator.Multithreading);
			}
		}

		public global::Pathfinding.RVO.MovementMode MovementMode { get; set; }

		public float Priority { get; set; }

		public global::System.Action PreCalculationCallback { private get; set; }

		public void SetTarget(global::UnityEngine.Vector2 targetPoint, float desiredSpeed, float maxSpeed)
		{
			maxSpeed = global::System.Math.Max(maxSpeed, 0f);
			desiredSpeed = global::System.Math.Min(global::System.Math.Max(desiredSpeed, 0f), maxSpeed);
			this.nextTargetPoint = targetPoint;
			this.nextDesiredSpeed = desiredSpeed;
			this.nextMaxSpeed = maxSpeed;
		}

		public void SetCollisionNormal(global::UnityEngine.Vector2 normal)
		{
			this.collisionNormal = normal;
		}

		public void ForceSetVelocity(global::UnityEngine.Vector2 velocity)
		{
			this.CalculatedTargetPoint = this.position + velocity * 1000f;
			this.CalculatedSpeed = velocity.magnitude;
			this.manuallyControlled = true;
		}

		public global::System.Collections.Generic.List<global::Pathfinding.RVO.ObstacleVertex> NeighbourObstacles
		{
			get
			{
				return null;
			}
		}

		public void BufferSwitch()
		{
			this.radius = this.Radius;
			this.height = this.Height;
			this.maxSpeed = this.nextMaxSpeed;
			this.desiredSpeed = this.nextDesiredSpeed;
			this.neighbourDist = this.NeighbourDist;
			this.agentTimeHorizon = this.AgentTimeHorizon;
			this.obstacleTimeHorizon = this.ObstacleTimeHorizon;
			this.maxNeighbours = this.MaxNeighbours;
			this.locked = this.Locked;
			this.position = this.Position;
			this.elevationCoordinate = this.ElevationCoordinate;
			this.collidesWith = this.CollidesWith;
			this.layer = this.Layer;
			this.desiredTargetPointInVelocitySpace = this.nextTargetPoint - this.position;
			this.currentVelocity = (this.CalculatedTargetPoint - this.position).normalized * this.CalculatedSpeed;
			if (this.collisionNormal != global::UnityEngine.Vector2.zero)
			{
				this.collisionNormal.Normalize();
				float num = global::UnityEngine.Vector2.Dot(this.currentVelocity, this.collisionNormal);
				if (num < 0f)
				{
					this.currentVelocity -= this.collisionNormal * num;
				}
				this.collisionNormal = global::UnityEngine.Vector2.zero;
			}
		}

		public void PreCalculation()
		{
			if (this.PreCalculationCallback != null)
			{
				this.PreCalculationCallback();
			}
		}

		public void PostCalculation()
		{
			if (!this.manuallyControlled)
			{
				this.CalculatedTargetPoint = this.calculatedTargetPoint;
				this.CalculatedSpeed = this.calculatedSpeed;
			}
			global::System.Collections.Generic.List<global::Pathfinding.RVO.ObstacleVertex> list = this.obstaclesBuffered;
			this.obstaclesBuffered = this.obstacles;
			this.obstacles = list;
			this.manuallyControlled = false;
		}

		public void CalculateNeighbours()
		{
			this.neighbours.Clear();
			this.neighbourDists.Clear();
			if (this.locked)
			{
				return;
			}
			if (this.MaxNeighbours > 0)
			{
				this.simulator.Quadtree.Query(this.position, this.neighbourDist, this);
			}
			this.NeighbourCount = this.neighbours.Count;
		}

		private static float Sqr(float x)
		{
			return x * x;
		}

		internal float InsertAgentNeighbour(global::Pathfinding.RVO.Sampled.Agent agent, float rangeSq)
		{
			if (this == agent || (agent.layer & this.collidesWith) == (global::Pathfinding.RVO.RVOLayer)0)
			{
				return rangeSq;
			}
			float sqrMagnitude = (agent.position - this.position).sqrMagnitude;
			if (sqrMagnitude < rangeSq)
			{
				if (this.neighbours.Count < this.maxNeighbours)
				{
					this.neighbours.Add(null);
					this.neighbourDists.Add(float.PositiveInfinity);
				}
				int num = this.neighbours.Count - 1;
				if (sqrMagnitude < this.neighbourDists[num])
				{
					while (num != 0 && sqrMagnitude < this.neighbourDists[num - 1])
					{
						this.neighbours[num] = this.neighbours[num - 1];
						this.neighbourDists[num] = this.neighbourDists[num - 1];
						num--;
					}
					this.neighbours[num] = agent;
					this.neighbourDists[num] = sqrMagnitude;
				}
				if (this.neighbours.Count == this.maxNeighbours)
				{
					rangeSq = this.neighbourDists[this.neighbourDists.Count - 1];
				}
			}
			return rangeSq;
		}

		public void InsertObstacleNeighbour(global::Pathfinding.RVO.ObstacleVertex ob1, float rangeSq)
		{
			global::Pathfinding.RVO.ObstacleVertex obstacleVertex = ob1.next;
			float num = global::Pathfinding.VectorMath.SqrDistancePointSegment(ob1.position, obstacleVertex.position, this.Position);
			if (num < rangeSq)
			{
				this.obstacles.Add(ob1);
				this.obstacleDists.Add(num);
				int num2 = this.obstacles.Count - 1;
				while (num2 != 0 && num < this.obstacleDists[num2 - 1])
				{
					this.obstacles[num2] = this.obstacles[num2 - 1];
					this.obstacleDists[num2] = this.obstacleDists[num2 - 1];
					num2--;
				}
				this.obstacles[num2] = ob1;
				this.obstacleDists[num2] = num;
			}
		}

		private static global::UnityEngine.Vector3 To3D(global::UnityEngine.Vector2 p)
		{
			return new global::UnityEngine.Vector3(p.x, 0f, p.y);
		}

		private static global::UnityEngine.Vector2 To2D(global::UnityEngine.Vector3 p)
		{
			return new global::UnityEngine.Vector2(p.x, p.z);
		}

		private static void DrawCircle(global::UnityEngine.Vector2 _p, float radius, global::UnityEngine.Color col)
		{
			global::Pathfinding.RVO.Sampled.Agent.DrawCircle(_p, radius, 0f, 6.28318548f, col);
		}

		private static void DrawCircle(global::UnityEngine.Vector2 _p, float radius, float a0, float a1, global::UnityEngine.Color col)
		{
			global::UnityEngine.Vector3 a2 = global::Pathfinding.RVO.Sampled.Agent.To3D(_p);
			while (a0 > a1)
			{
				a0 -= 6.28318548f;
			}
			global::UnityEngine.Vector3 b = new global::UnityEngine.Vector3(global::UnityEngine.Mathf.Cos(a0) * radius, 0f, global::UnityEngine.Mathf.Sin(a0) * radius);
			int num = 0;
			while ((float)num <= 40f)
			{
				global::UnityEngine.Vector3 vector = new global::UnityEngine.Vector3(global::UnityEngine.Mathf.Cos(global::UnityEngine.Mathf.Lerp(a0, a1, (float)num / 40f)) * radius, 0f, global::UnityEngine.Mathf.Sin(global::UnityEngine.Mathf.Lerp(a0, a1, (float)num / 40f)) * radius);
				global::UnityEngine.Debug.DrawLine(a2 + b, a2 + vector, col);
				b = vector;
				num++;
			}
		}

		private static void DrawVO(global::UnityEngine.Vector2 circleCenter, float radius, global::UnityEngine.Vector2 origin)
		{
			float num = global::UnityEngine.Mathf.Atan2((origin - circleCenter).y, (origin - circleCenter).x);
			float num2 = radius / (origin - circleCenter).magnitude;
			float num3 = (num2 > 1f) ? 0f : global::UnityEngine.Mathf.Abs(global::UnityEngine.Mathf.Acos(num2));
			global::Pathfinding.RVO.Sampled.Agent.DrawCircle(circleCenter, radius, num - num3, num + num3, global::UnityEngine.Color.black);
			global::UnityEngine.Vector2 vector = new global::UnityEngine.Vector2(global::UnityEngine.Mathf.Cos(num - num3), global::UnityEngine.Mathf.Sin(num - num3)) * radius;
			global::UnityEngine.Vector2 vector2 = new global::UnityEngine.Vector2(global::UnityEngine.Mathf.Cos(num + num3), global::UnityEngine.Mathf.Sin(num + num3)) * radius;
			global::UnityEngine.Vector2 p = -new global::UnityEngine.Vector2(-vector.y, vector.x);
			global::UnityEngine.Vector2 p2 = new global::UnityEngine.Vector2(-vector2.y, vector2.x);
			vector += circleCenter;
			vector2 += circleCenter;
			global::UnityEngine.Debug.DrawRay(global::Pathfinding.RVO.Sampled.Agent.To3D(vector), global::Pathfinding.RVO.Sampled.Agent.To3D(p).normalized * 100f, global::UnityEngine.Color.black);
			global::UnityEngine.Debug.DrawRay(global::Pathfinding.RVO.Sampled.Agent.To3D(vector2), global::Pathfinding.RVO.Sampled.Agent.To3D(p2).normalized * 100f, global::UnityEngine.Color.black);
		}

		private static void DrawCross(global::UnityEngine.Vector2 p, float size = 1f)
		{
			global::Pathfinding.RVO.Sampled.Agent.DrawCross(p, global::UnityEngine.Color.white, size);
		}

		private static void DrawCross(global::UnityEngine.Vector2 p, global::UnityEngine.Color col, float size = 1f)
		{
			size *= 0.5f;
			global::UnityEngine.Debug.DrawLine(new global::UnityEngine.Vector3(p.x, 0f, p.y) - global::UnityEngine.Vector3.right * size, new global::UnityEngine.Vector3(p.x, 0f, p.y) + global::UnityEngine.Vector3.right * size, col);
			global::UnityEngine.Debug.DrawLine(new global::UnityEngine.Vector3(p.x, 0f, p.y) - global::UnityEngine.Vector3.forward * size, new global::UnityEngine.Vector3(p.x, 0f, p.y) + global::UnityEngine.Vector3.forward * size, col);
		}

		internal void CalculateVelocity(global::Pathfinding.RVO.Simulator.WorkerContext context)
		{
			if (this.manuallyControlled)
			{
				return;
			}
			if (this.locked)
			{
				this.calculatedSpeed = 0f;
				this.calculatedTargetPoint = this.position;
				return;
			}
			this.desiredVelocity = this.desiredTargetPointInVelocitySpace.normalized * this.desiredSpeed;
			global::Pathfinding.RVO.Sampled.Agent.VOBuffer vos = context.vos;
			vos.Clear();
			this.GenerateObstacleVOs(vos);
			this.GenerateNeighbourAgentVOs(vos);
			if (!global::Pathfinding.RVO.Sampled.Agent.BiasDesiredVelocity(vos, ref this.desiredVelocity, ref this.desiredTargetPointInVelocitySpace, this.simulator.symmetryBreakingBias))
			{
				this.calculatedTargetPoint = this.desiredTargetPointInVelocitySpace + this.position;
				this.calculatedSpeed = this.desiredSpeed;
				if (this.DebugDraw)
				{
					global::Pathfinding.RVO.Sampled.Agent.DrawCross(this.calculatedTargetPoint, 1f);
				}
				return;
			}
			global::UnityEngine.Vector2 vector = global::UnityEngine.Vector2.zero;
			vector = this.GradientDescent(vos, this.currentVelocity, this.desiredVelocity);
			if (this.DebugDraw)
			{
				global::Pathfinding.RVO.Sampled.Agent.DrawCross(vector + this.position, 1f);
			}
			this.calculatedTargetPoint = this.position + vector;
			this.calculatedSpeed = global::UnityEngine.Mathf.Min(vector.magnitude, this.maxSpeed);
		}

		private static global::UnityEngine.Color Rainbow(float v)
		{
			global::UnityEngine.Color result = new global::UnityEngine.Color(v, 0f, 0f);
			if (result.r > 1f)
			{
				result.g = result.r - 1f;
				result.r = 1f;
			}
			if (result.g > 1f)
			{
				result.b = result.g - 1f;
				result.g = 1f;
			}
			return result;
		}

		private void GenerateObstacleVOs(global::Pathfinding.RVO.Sampled.Agent.VOBuffer vos)
		{
			for (int i = 0; i < this.simulator.obstacles.Count; i++)
			{
				global::Pathfinding.RVO.ObstacleVertex obstacleVertex = this.simulator.obstacles[i];
				global::Pathfinding.RVO.ObstacleVertex obstacleVertex2 = obstacleVertex;
				do
				{
					if (obstacleVertex2.ignore || (obstacleVertex2.layer & this.collidesWith) == (global::Pathfinding.RVO.RVOLayer)0)
					{
						obstacleVertex2 = obstacleVertex2.next;
					}
					else
					{
						global::UnityEngine.Vector2 vector = global::Pathfinding.RVO.Sampled.Agent.To2D(obstacleVertex2.position);
						global::UnityEngine.Vector2 vector2 = global::Pathfinding.RVO.Sampled.Agent.To2D(obstacleVertex2.next.position);
						float num = global::Pathfinding.RVO.Sampled.Agent.VO.SignedDistanceFromLine(vector, obstacleVertex2.dir, this.position);
						if (num >= -0.01f && num < this.neighbourDist)
						{
							float t = global::UnityEngine.Vector2.Dot(this.position - vector, vector2 - vector) / (vector2 - vector).sqrMagnitude;
							float num2 = global::UnityEngine.Mathf.Lerp(obstacleVertex2.position.y, obstacleVertex2.next.position.y, t);
							float sqrMagnitude = (global::UnityEngine.Vector2.Lerp(vector, vector2, t) - this.position).sqrMagnitude;
							if (sqrMagnitude < this.neighbourDist * this.neighbourDist && this.elevationCoordinate <= num2 + obstacleVertex2.height && this.elevationCoordinate + this.height >= num2)
							{
								vos.Add(global::Pathfinding.RVO.Sampled.Agent.VO.SegmentObstacle(vector2 - this.position, vector - this.position, global::UnityEngine.Vector2.zero, this.radius * 0.01f, 1f / this.ObstacleTimeHorizon, 1f / this.simulator.DeltaTime));
							}
						}
						obstacleVertex2 = obstacleVertex2.next;
					}
				}
				while (obstacleVertex2 != obstacleVertex && obstacleVertex2 != null && obstacleVertex2.next != null);
			}
		}

		private void GenerateNeighbourAgentVOs(global::Pathfinding.RVO.Sampled.Agent.VOBuffer vos)
		{
			float num = 1f / this.agentTimeHorizon;
			global::UnityEngine.Vector2 a = this.currentVelocity;
			for (int i = 0; i < this.neighbours.Count; i++)
			{
				global::Pathfinding.RVO.Sampled.Agent agent = this.neighbours[i];
				if (agent != this)
				{
					float num2 = global::System.Math.Min(this.elevationCoordinate + this.height, agent.elevationCoordinate + agent.height);
					float num3 = global::System.Math.Max(this.elevationCoordinate, agent.elevationCoordinate);
					if (num2 - num3 >= 0f)
					{
						global::UnityEngine.Vector2 b = agent.currentVelocity;
						float num4 = this.radius + agent.radius;
						global::UnityEngine.Vector2 vector = agent.position - this.position;
						float t;
						if (agent.locked || agent.manuallyControlled)
						{
							t = 1f;
						}
						else if (agent.Priority > 1E-05f || this.Priority > 1E-05f)
						{
							t = agent.Priority / (this.Priority + agent.Priority);
						}
						else
						{
							t = 0.5f;
						}
						global::UnityEngine.Vector2 vector2 = global::UnityEngine.Vector2.Lerp(a, b, t);
						vos.Add(new global::Pathfinding.RVO.Sampled.Agent.VO(vector, vector2, num4, num, 1f / this.simulator.DeltaTime));
						if (this.DebugDraw)
						{
							global::Pathfinding.RVO.Sampled.Agent.DrawVO(this.position + vector * num + vector2, num4 * num, this.position + vector2);
						}
					}
				}
			}
		}

		private global::UnityEngine.Vector2 GradientDescent(global::Pathfinding.RVO.Sampled.Agent.VOBuffer vos, global::UnityEngine.Vector2 sampleAround1, global::UnityEngine.Vector2 sampleAround2)
		{
			float num;
			global::UnityEngine.Vector2 vector = this.Trace(vos, sampleAround1, out num);
			if (this.DebugDraw)
			{
				global::Pathfinding.RVO.Sampled.Agent.DrawCross(vector + this.position, global::UnityEngine.Color.yellow, 0.5f);
			}
			float num2;
			global::UnityEngine.Vector2 vector2 = this.Trace(vos, sampleAround2, out num2);
			if (this.DebugDraw)
			{
				global::Pathfinding.RVO.Sampled.Agent.DrawCross(vector2 + this.position, global::UnityEngine.Color.magenta, 0.5f);
			}
			return (num >= num2) ? vector2 : vector;
		}

		private static bool BiasDesiredVelocity(global::Pathfinding.RVO.Sampled.Agent.VOBuffer vos, ref global::UnityEngine.Vector2 desiredVelocity, ref global::UnityEngine.Vector2 targetPointInVelocitySpace, float maxBiasRadians)
		{
			float magnitude = desiredVelocity.magnitude;
			float num = 0f;
			for (int i = 0; i < vos.length; i++)
			{
				float b;
				vos.buffer[i].Gradient(desiredVelocity, out b);
				num = global::UnityEngine.Mathf.Max(num, b);
			}
			bool result = num > 0f;
			if (magnitude < 0.001f)
			{
				return result;
			}
			float d = global::UnityEngine.Mathf.Min(maxBiasRadians, num / magnitude);
			desiredVelocity += new global::UnityEngine.Vector2(desiredVelocity.y, -desiredVelocity.x) * d;
			targetPointInVelocitySpace += new global::UnityEngine.Vector2(targetPointInVelocitySpace.y, -targetPointInVelocitySpace.x) * d;
			return result;
		}

		private global::UnityEngine.Vector2 EvaluateGradient(global::Pathfinding.RVO.Sampled.Agent.VOBuffer vos, global::UnityEngine.Vector2 p, out float value)
		{
			global::UnityEngine.Vector2 vector = global::UnityEngine.Vector2.zero;
			value = 0f;
			for (int i = 0; i < vos.length; i++)
			{
				float num;
				global::UnityEngine.Vector2 vector2 = vos.buffer[i].ScaledGradient(p, out num);
				if (num > value)
				{
					value = num;
					vector = vector2;
				}
			}
			global::UnityEngine.Vector2 a = this.desiredVelocity - p;
			float magnitude = a.magnitude;
			if (magnitude > 0.0001f)
			{
				vector += a * (0.1f / magnitude);
				value += magnitude * 0.1f;
			}
			float sqrMagnitude = p.sqrMagnitude;
			if (sqrMagnitude > this.desiredSpeed * this.desiredSpeed)
			{
				float num2 = global::UnityEngine.Mathf.Sqrt(sqrMagnitude);
				if (num2 > this.maxSpeed)
				{
					value += 3f * (num2 - this.maxSpeed);
					vector -= 3f * (p / num2);
				}
				float num3 = 0.2f;
				value += num3 * (num2 - this.desiredSpeed);
				vector -= num3 * (p / num2);
			}
			return vector;
		}

		private global::UnityEngine.Vector2 Trace(global::Pathfinding.RVO.Sampled.Agent.VOBuffer vos, global::UnityEngine.Vector2 p, out float score)
		{
			float num = global::UnityEngine.Mathf.Max(this.radius, 0.2f * this.desiredSpeed);
			float num2 = float.PositiveInfinity;
			global::UnityEngine.Vector2 result = p;
			for (int i = 0; i < 50; i++)
			{
				float num3 = 1f - (float)i / 50f;
				num3 = global::Pathfinding.RVO.Sampled.Agent.Sqr(num3) * num;
				float num4;
				global::UnityEngine.Vector2 vector = this.EvaluateGradient(vos, p, out num4);
				if (num4 < num2)
				{
					num2 = num4;
					result = p;
				}
				vector.Normalize();
				vector *= num3;
				global::UnityEngine.Vector2 a = p;
				p += vector;
				if (this.DebugDraw)
				{
					global::UnityEngine.Debug.DrawLine(global::Pathfinding.RVO.Sampled.Agent.To3D(a + this.position), global::Pathfinding.RVO.Sampled.Agent.To3D(p + this.position), global::Pathfinding.RVO.Sampled.Agent.Rainbow((float)i * 0.1f) * new global::UnityEngine.Color(1f, 1f, 1f, 1f));
				}
			}
			score = num2;
			return result;
		}

		private const float DesiredVelocityWeight = 0.1f;

		private const float WallWeight = 5f;

		internal float radius;

		internal float height;

		internal float desiredSpeed;

		internal float maxSpeed;

		internal float neighbourDist;

		internal float agentTimeHorizon;

		internal float obstacleTimeHorizon;

		internal bool locked;

		private global::Pathfinding.RVO.RVOLayer layer;

		private global::Pathfinding.RVO.RVOLayer collidesWith;

		private int maxNeighbours;

		internal global::UnityEngine.Vector2 position;

		private float elevationCoordinate;

		private global::UnityEngine.Vector2 currentVelocity;

		private global::UnityEngine.Vector2 desiredTargetPointInVelocitySpace;

		private global::UnityEngine.Vector2 desiredVelocity;

		private global::UnityEngine.Vector2 nextTargetPoint;

		private float nextDesiredSpeed;

		private float nextMaxSpeed;

		private global::UnityEngine.Vector2 collisionNormal;

		private bool manuallyControlled;

		private bool debugDraw;

		internal global::Pathfinding.RVO.Sampled.Agent next;

		private float calculatedSpeed;

		private global::UnityEngine.Vector2 calculatedTargetPoint;

		internal global::Pathfinding.RVO.Simulator simulator;

		private global::System.Collections.Generic.List<global::Pathfinding.RVO.Sampled.Agent> neighbours = new global::System.Collections.Generic.List<global::Pathfinding.RVO.Sampled.Agent>();

		private global::System.Collections.Generic.List<float> neighbourDists = new global::System.Collections.Generic.List<float>();

		private global::System.Collections.Generic.List<global::Pathfinding.RVO.ObstacleVertex> obstaclesBuffered = new global::System.Collections.Generic.List<global::Pathfinding.RVO.ObstacleVertex>();

		private global::System.Collections.Generic.List<global::Pathfinding.RVO.ObstacleVertex> obstacles = new global::System.Collections.Generic.List<global::Pathfinding.RVO.ObstacleVertex>();

		private global::System.Collections.Generic.List<float> obstacleDists = new global::System.Collections.Generic.List<float>();

		internal struct VO
		{
			public VO(global::UnityEngine.Vector2 center, global::UnityEngine.Vector2 offset, float radius, float inverseDt, float inverseDeltaTime)
			{
				this.weightFactor = 1f;
				this.weightBonus = 0f;
				this.circleCenter = center * inverseDt + offset;
				this.weightFactor = 4f * global::UnityEngine.Mathf.Exp(-global::Pathfinding.RVO.Sampled.Agent.Sqr(center.sqrMagnitude / (radius * radius))) + 1f;
				if (center.magnitude < radius)
				{
					this.colliding = true;
					this.line1 = center.normalized * (center.magnitude - radius - 0.001f) * 0.3f * inverseDeltaTime;
					global::UnityEngine.Vector2 vector = new global::UnityEngine.Vector2(this.line1.y, -this.line1.x);
					this.dir1 = vector.normalized;
					this.line1 += offset;
					this.cutoffDir = global::UnityEngine.Vector2.zero;
					this.cutoffLine = global::UnityEngine.Vector2.zero;
					this.dir2 = global::UnityEngine.Vector2.zero;
					this.line2 = global::UnityEngine.Vector2.zero;
					this.radius = 0f;
				}
				else
				{
					this.colliding = false;
					center *= inverseDt;
					radius *= inverseDt;
					global::UnityEngine.Vector2 b = center + offset;
					float d = center.magnitude - radius + 0.001f;
					this.cutoffLine = center.normalized * d;
					global::UnityEngine.Vector2 vector2 = new global::UnityEngine.Vector2(-this.cutoffLine.y, this.cutoffLine.x);
					this.cutoffDir = vector2.normalized;
					this.cutoffLine += offset;
					float num = global::UnityEngine.Mathf.Atan2(-center.y, -center.x);
					float num2 = global::UnityEngine.Mathf.Abs(global::UnityEngine.Mathf.Acos(radius / center.magnitude));
					this.radius = radius;
					this.line1 = new global::UnityEngine.Vector2(global::UnityEngine.Mathf.Cos(num + num2), global::UnityEngine.Mathf.Sin(num + num2));
					this.dir1 = new global::UnityEngine.Vector2(this.line1.y, -this.line1.x);
					this.line2 = new global::UnityEngine.Vector2(global::UnityEngine.Mathf.Cos(num - num2), global::UnityEngine.Mathf.Sin(num - num2));
					this.dir2 = new global::UnityEngine.Vector2(this.line2.y, -this.line2.x);
					this.line1 = this.line1 * radius + b;
					this.line2 = this.line2 * radius + b;
				}
				this.segmentStart = global::UnityEngine.Vector2.zero;
				this.segmentEnd = global::UnityEngine.Vector2.zero;
				this.segment = false;
			}

			private static global::UnityEngine.Vector2 ComplexMultiply(global::UnityEngine.Vector2 a, global::UnityEngine.Vector2 b)
			{
				return new global::UnityEngine.Vector2(a.x * b.x - a.y * b.y, a.x * b.y + a.y * b.x);
			}

			public static global::Pathfinding.RVO.Sampled.Agent.VO SegmentObstacle(global::UnityEngine.Vector2 segmentStart, global::UnityEngine.Vector2 segmentEnd, global::UnityEngine.Vector2 offset, float radius, float inverseDt, float inverseDeltaTime)
			{
				global::Pathfinding.RVO.Sampled.Agent.VO result = default(global::Pathfinding.RVO.Sampled.Agent.VO);
				result.weightFactor = 1f;
				result.weightBonus = global::UnityEngine.Mathf.Max(radius, 1f) * 40f;
				global::UnityEngine.Vector3 vector = global::Pathfinding.VectorMath.ClosestPointOnSegment(segmentStart, segmentEnd, global::UnityEngine.Vector2.zero);
				if (vector.magnitude <= radius)
				{
					result.colliding = true;
					result.line1 = vector.normalized * (vector.magnitude - radius) * 0.3f * inverseDeltaTime;
					global::UnityEngine.Vector2 vector2 = new global::UnityEngine.Vector2(result.line1.y, -result.line1.x);
					result.dir1 = vector2.normalized;
					result.line1 += offset;
					result.cutoffDir = global::UnityEngine.Vector2.zero;
					result.cutoffLine = global::UnityEngine.Vector2.zero;
					result.dir2 = global::UnityEngine.Vector2.zero;
					result.line2 = global::UnityEngine.Vector2.zero;
					result.radius = 0f;
					result.segmentStart = global::UnityEngine.Vector2.zero;
					result.segmentEnd = global::UnityEngine.Vector2.zero;
					result.segment = false;
				}
				else
				{
					result.colliding = false;
					segmentStart *= inverseDt;
					segmentEnd *= inverseDt;
					radius *= inverseDt;
					global::UnityEngine.Vector2 normalized = (segmentEnd - segmentStart).normalized;
					result.cutoffDir = normalized;
					result.cutoffLine = segmentStart + new global::UnityEngine.Vector2(-normalized.y, normalized.x) * radius;
					result.cutoffLine += offset;
					float sqrMagnitude = segmentStart.sqrMagnitude;
					global::UnityEngine.Vector2 a = -global::Pathfinding.RVO.Sampled.Agent.VO.ComplexMultiply(segmentStart, new global::UnityEngine.Vector2(radius, global::UnityEngine.Mathf.Sqrt(global::UnityEngine.Mathf.Max(0f, sqrMagnitude - radius * radius)))) / sqrMagnitude;
					float sqrMagnitude2 = segmentEnd.sqrMagnitude;
					global::UnityEngine.Vector2 a2 = -global::Pathfinding.RVO.Sampled.Agent.VO.ComplexMultiply(segmentEnd, new global::UnityEngine.Vector2(radius, -global::UnityEngine.Mathf.Sqrt(global::UnityEngine.Mathf.Max(0f, sqrMagnitude2 - radius * radius)))) / sqrMagnitude2;
					result.line1 = segmentStart + a * radius + offset;
					result.line2 = segmentEnd + a2 * radius + offset;
					result.dir1 = new global::UnityEngine.Vector2(a.y, -a.x);
					result.dir2 = new global::UnityEngine.Vector2(a2.y, -a2.x);
					result.segmentStart = segmentStart;
					result.segmentEnd = segmentEnd;
					result.radius = radius;
					result.segment = true;
				}
				return result;
			}

			public static float SignedDistanceFromLine(global::UnityEngine.Vector2 a, global::UnityEngine.Vector2 dir, global::UnityEngine.Vector2 p)
			{
				return (p.x - a.x) * dir.y - dir.x * (p.y - a.y);
			}

			public global::UnityEngine.Vector2 ScaledGradient(global::UnityEngine.Vector2 p, out float weight)
			{
				global::UnityEngine.Vector2 vector = this.Gradient(p, out weight);
				if (weight > 0f)
				{
					vector *= 2f * this.weightFactor;
					weight *= 2f * this.weightFactor;
					weight += 1f + this.weightBonus;
				}
				return vector;
			}

			public global::UnityEngine.Vector2 Gradient(global::UnityEngine.Vector2 p, out float weight)
			{
				if (this.colliding)
				{
					float num = global::Pathfinding.RVO.Sampled.Agent.VO.SignedDistanceFromLine(this.line1, this.dir1, p);
					if (num >= 0f)
					{
						weight = num;
						return new global::UnityEngine.Vector2(-this.dir1.y, this.dir1.x);
					}
					weight = 0f;
					return new global::UnityEngine.Vector2(0f, 0f);
				}
				else
				{
					float num2 = global::Pathfinding.RVO.Sampled.Agent.VO.SignedDistanceFromLine(this.cutoffLine, this.cutoffDir, p);
					if (num2 <= 0f)
					{
						weight = 0f;
						return global::UnityEngine.Vector2.zero;
					}
					float num3 = global::Pathfinding.RVO.Sampled.Agent.VO.SignedDistanceFromLine(this.line1, this.dir1, p);
					float num4 = global::Pathfinding.RVO.Sampled.Agent.VO.SignedDistanceFromLine(this.line2, this.dir2, p);
					if (num3 < 0f || num4 < 0f)
					{
						weight = 0f;
						return global::UnityEngine.Vector2.zero;
					}
					global::UnityEngine.Vector2 result;
					if (global::UnityEngine.Vector2.Dot(p - this.line1, this.dir1) > 0f && global::UnityEngine.Vector2.Dot(p - this.line2, this.dir2) < 0f)
					{
						if (!this.segment)
						{
							global::UnityEngine.Vector2 v = p - this.circleCenter;
							float num5;
							result = global::Pathfinding.VectorMath.Normalize(v, out num5);
							weight = this.radius - num5;
							return result;
						}
						if (num2 < this.radius)
						{
							global::UnityEngine.Vector2 b = global::Pathfinding.VectorMath.ClosestPointOnSegment(this.segmentStart, this.segmentEnd, p);
							global::UnityEngine.Vector2 v2 = p - b;
							float num6;
							result = global::Pathfinding.VectorMath.Normalize(v2, out num6);
							weight = this.radius - num6;
							return result;
						}
					}
					if (this.segment && num2 < num3 && num2 < num4)
					{
						weight = num2;
						result = new global::UnityEngine.Vector2(-this.cutoffDir.y, this.cutoffDir.x);
						return result;
					}
					if (num3 < num4)
					{
						weight = num3;
						result = new global::UnityEngine.Vector2(-this.dir1.y, this.dir1.x);
					}
					else
					{
						weight = num4;
						result = new global::UnityEngine.Vector2(-this.dir2.y, this.dir2.x);
					}
					return result;
				}
			}

			private global::UnityEngine.Vector2 line1;

			private global::UnityEngine.Vector2 line2;

			private global::UnityEngine.Vector2 dir1;

			private global::UnityEngine.Vector2 dir2;

			private global::UnityEngine.Vector2 cutoffLine;

			private global::UnityEngine.Vector2 cutoffDir;

			private global::UnityEngine.Vector2 circleCenter;

			private bool colliding;

			private float radius;

			private float weightFactor;

			private float weightBonus;

			private global::UnityEngine.Vector2 segmentStart;

			private global::UnityEngine.Vector2 segmentEnd;

			private bool segment;
		}

		internal class VOBuffer
		{
			public VOBuffer(int n)
			{
				this.buffer = new global::Pathfinding.RVO.Sampled.Agent.VO[n];
				this.length = 0;
			}

			public void Clear()
			{
				this.length = 0;
			}

			public void Add(global::Pathfinding.RVO.Sampled.Agent.VO vo)
			{
				if (this.length >= this.buffer.Length)
				{
					global::Pathfinding.RVO.Sampled.Agent.VO[] array = new global::Pathfinding.RVO.Sampled.Agent.VO[this.buffer.Length * 2];
					this.buffer.CopyTo(array, 0);
					this.buffer = array;
				}
				this.buffer[this.length++] = vo;
			}

			public global::Pathfinding.RVO.Sampled.Agent.VO[] buffer;

			public int length;
		}
	}
}
