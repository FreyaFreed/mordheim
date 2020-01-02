using System;
using System.Collections.Generic;
using Pathfinding.RVO;
using UnityEngine;

namespace Pathfinding.Examples
{
	[global::UnityEngine.HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_examples_1_1_lightweight_r_v_o.php")]
	[global::UnityEngine.RequireComponent(typeof(global::UnityEngine.MeshFilter))]
	public class LightweightRVO : global::UnityEngine.MonoBehaviour
	{
		public void Start()
		{
			this.mesh = new global::UnityEngine.Mesh();
			global::Pathfinding.RVO.RVOSimulator rvosimulator = global::UnityEngine.Object.FindObjectOfType(typeof(global::Pathfinding.RVO.RVOSimulator)) as global::Pathfinding.RVO.RVOSimulator;
			if (rvosimulator == null)
			{
				global::UnityEngine.Debug.LogError("No RVOSimulator could be found in the scene. Please add a RVOSimulator component to any GameObject");
				return;
			}
			this.sim = rvosimulator.GetSimulator();
			base.GetComponent<global::UnityEngine.MeshFilter>().mesh = this.mesh;
			this.CreateAgents(this.agentCount);
		}

		public void OnGUI()
		{
			if (global::UnityEngine.GUILayout.Button("2", new global::UnityEngine.GUILayoutOption[0]))
			{
				this.CreateAgents(2);
			}
			if (global::UnityEngine.GUILayout.Button("10", new global::UnityEngine.GUILayoutOption[0]))
			{
				this.CreateAgents(10);
			}
			if (global::UnityEngine.GUILayout.Button("100", new global::UnityEngine.GUILayoutOption[0]))
			{
				this.CreateAgents(100);
			}
			if (global::UnityEngine.GUILayout.Button("500", new global::UnityEngine.GUILayoutOption[0]))
			{
				this.CreateAgents(500);
			}
			if (global::UnityEngine.GUILayout.Button("1000", new global::UnityEngine.GUILayoutOption[0]))
			{
				this.CreateAgents(1000);
			}
			if (global::UnityEngine.GUILayout.Button("5000", new global::UnityEngine.GUILayoutOption[0]))
			{
				this.CreateAgents(5000);
			}
			global::UnityEngine.GUILayout.Space(5f);
			if (global::UnityEngine.GUILayout.Button("Random Streams", new global::UnityEngine.GUILayoutOption[0]))
			{
				this.type = global::Pathfinding.Examples.LightweightRVO.RVOExampleType.RandomStreams;
				this.CreateAgents((this.agents == null) ? 100 : this.agents.Count);
			}
			if (global::UnityEngine.GUILayout.Button("Line", new global::UnityEngine.GUILayoutOption[0]))
			{
				this.type = global::Pathfinding.Examples.LightweightRVO.RVOExampleType.Line;
				this.CreateAgents((this.agents == null) ? 10 : global::UnityEngine.Mathf.Min(this.agents.Count, 100));
			}
			if (global::UnityEngine.GUILayout.Button("Circle", new global::UnityEngine.GUILayoutOption[0]))
			{
				this.type = global::Pathfinding.Examples.LightweightRVO.RVOExampleType.Circle;
				this.CreateAgents((this.agents == null) ? 100 : this.agents.Count);
			}
			if (global::UnityEngine.GUILayout.Button("Point", new global::UnityEngine.GUILayoutOption[0]))
			{
				this.type = global::Pathfinding.Examples.LightweightRVO.RVOExampleType.Point;
				this.CreateAgents((this.agents == null) ? 100 : this.agents.Count);
			}
			if (global::UnityEngine.GUILayout.Button("Crossing", new global::UnityEngine.GUILayoutOption[0]))
			{
				this.type = global::Pathfinding.Examples.LightweightRVO.RVOExampleType.Crossing;
				this.CreateAgents((this.agents == null) ? 100 : this.agents.Count);
			}
		}

		private float uniformDistance(float radius)
		{
			float num = global::UnityEngine.Random.value + global::UnityEngine.Random.value;
			if (num > 1f)
			{
				return radius * (2f - num);
			}
			return radius * num;
		}

		public void CreateAgents(int num)
		{
			this.agentCount = num;
			this.agents = new global::System.Collections.Generic.List<global::Pathfinding.RVO.IAgent>(this.agentCount);
			this.goals = new global::System.Collections.Generic.List<global::UnityEngine.Vector3>(this.agentCount);
			this.colors = new global::System.Collections.Generic.List<global::UnityEngine.Color>(this.agentCount);
			this.sim.ClearAgents();
			if (this.type == global::Pathfinding.Examples.LightweightRVO.RVOExampleType.Circle)
			{
				float d = global::UnityEngine.Mathf.Sqrt((float)this.agentCount * this.radius * this.radius * 4f / 3.14159274f) * this.exampleScale * 0.05f;
				for (int i = 0; i < this.agentCount; i++)
				{
					global::UnityEngine.Vector3 a = new global::UnityEngine.Vector3(global::UnityEngine.Mathf.Cos((float)i * 3.14159274f * 2f / (float)this.agentCount), 0f, global::UnityEngine.Mathf.Sin((float)i * 3.14159274f * 2f / (float)this.agentCount)) * d * (1f + global::UnityEngine.Random.value * 0.01f);
					global::Pathfinding.RVO.IAgent item = this.sim.AddAgent(new global::UnityEngine.Vector2(a.x, a.z), a.y);
					this.agents.Add(item);
					this.goals.Add(-a);
					this.colors.Add(global::Pathfinding.AstarMath.HSVToRGB((float)i * 360f / (float)this.agentCount, 0.8f, 0.6f));
				}
			}
			else if (this.type == global::Pathfinding.Examples.LightweightRVO.RVOExampleType.Line)
			{
				for (int j = 0; j < this.agentCount; j++)
				{
					global::UnityEngine.Vector3 vector = new global::UnityEngine.Vector3((float)((j % 2 != 0) ? -1 : 1) * this.exampleScale, 0f, (float)(j / 2) * this.radius * 2.5f);
					global::Pathfinding.RVO.IAgent item2 = this.sim.AddAgent(new global::UnityEngine.Vector2(vector.x, vector.z), vector.y);
					this.agents.Add(item2);
					this.goals.Add(new global::UnityEngine.Vector3(-vector.x, vector.y, vector.z));
					this.colors.Add((j % 2 != 0) ? global::UnityEngine.Color.blue : global::UnityEngine.Color.red);
				}
			}
			else if (this.type == global::Pathfinding.Examples.LightweightRVO.RVOExampleType.Point)
			{
				for (int k = 0; k < this.agentCount; k++)
				{
					global::UnityEngine.Vector3 vector2 = new global::UnityEngine.Vector3(global::UnityEngine.Mathf.Cos((float)k * 3.14159274f * 2f / (float)this.agentCount), 0f, global::UnityEngine.Mathf.Sin((float)k * 3.14159274f * 2f / (float)this.agentCount)) * this.exampleScale;
					global::Pathfinding.RVO.IAgent item3 = this.sim.AddAgent(new global::UnityEngine.Vector2(vector2.x, vector2.z), vector2.y);
					this.agents.Add(item3);
					this.goals.Add(new global::UnityEngine.Vector3(0f, vector2.y, 0f));
					this.colors.Add(global::Pathfinding.AstarMath.HSVToRGB((float)k * 360f / (float)this.agentCount, 0.8f, 0.6f));
				}
			}
			else if (this.type == global::Pathfinding.Examples.LightweightRVO.RVOExampleType.RandomStreams)
			{
				float num2 = global::UnityEngine.Mathf.Sqrt((float)this.agentCount * this.radius * this.radius * 4f / 3.14159274f) * this.exampleScale * 0.05f;
				for (int l = 0; l < this.agentCount; l++)
				{
					float f = global::UnityEngine.Random.value * 3.14159274f * 2f;
					float num3 = global::UnityEngine.Random.value * 3.14159274f * 2f;
					global::UnityEngine.Vector3 vector3 = new global::UnityEngine.Vector3(global::UnityEngine.Mathf.Cos(f), 0f, global::UnityEngine.Mathf.Sin(f)) * this.uniformDistance(num2);
					global::Pathfinding.RVO.IAgent item4 = this.sim.AddAgent(new global::UnityEngine.Vector2(vector3.x, vector3.z), vector3.y);
					this.agents.Add(item4);
					this.goals.Add(new global::UnityEngine.Vector3(global::UnityEngine.Mathf.Cos(num3), 0f, global::UnityEngine.Mathf.Sin(num3)) * this.uniformDistance(num2));
					this.colors.Add(global::Pathfinding.AstarMath.HSVToRGB(num3 * 57.29578f, 0.8f, 0.6f));
				}
			}
			else if (this.type == global::Pathfinding.Examples.LightweightRVO.RVOExampleType.Crossing)
			{
				float num4 = this.exampleScale * this.radius * 0.5f;
				int num5 = (int)global::UnityEngine.Mathf.Sqrt((float)this.agentCount / 25f);
				num5 = global::UnityEngine.Mathf.Max(num5, 2);
				for (int m = 0; m < this.agentCount; m++)
				{
					float num6 = (float)(m % num5) / (float)num5 * 3.14159274f * 2f;
					float d2 = num4 * ((float)(m / (num5 * 10) + 1) + 0.3f * global::UnityEngine.Random.value);
					global::UnityEngine.Vector3 vector4 = new global::UnityEngine.Vector3(global::UnityEngine.Mathf.Cos(num6), 0f, global::UnityEngine.Mathf.Sin(num6)) * d2;
					global::Pathfinding.RVO.IAgent agent = this.sim.AddAgent(new global::UnityEngine.Vector2(vector4.x, vector4.z), vector4.y);
					agent.Priority = ((m % num5 != 0) ? 0.01f : 1f);
					this.agents.Add(agent);
					this.goals.Add(-vector4.normalized * num4 * 3f);
					this.colors.Add(global::Pathfinding.AstarMath.HSVToRGB(num6 * 57.29578f, 0.8f, 0.6f));
				}
			}
			for (int n = 0; n < this.agents.Count; n++)
			{
				global::Pathfinding.RVO.IAgent agent2 = this.agents[n];
				agent2.Radius = this.radius;
				agent2.AgentTimeHorizon = this.agentTimeHorizon;
				agent2.ObstacleTimeHorizon = this.obstacleTimeHorizon;
				agent2.MaxNeighbours = this.maxNeighbours;
				agent2.NeighbourDist = this.neighbourDist;
				agent2.DebugDraw = (n == 0 && this.debug);
			}
			this.verts = new global::UnityEngine.Vector3[4 * this.agents.Count];
			this.uv = new global::UnityEngine.Vector2[this.verts.Length];
			this.tris = new int[this.agents.Count * 2 * 3];
			this.meshColors = new global::UnityEngine.Color[this.verts.Length];
		}

		public void Update()
		{
			if (this.agents == null || this.mesh == null)
			{
				return;
			}
			if (this.agents.Count != this.goals.Count)
			{
				global::UnityEngine.Debug.LogError("Agent count does not match goal count");
				return;
			}
			if (this.interpolatedVelocities == null || this.interpolatedVelocities.Length < this.agents.Count)
			{
				global::UnityEngine.Vector2[] array = new global::UnityEngine.Vector2[this.agents.Count];
				global::UnityEngine.Vector2[] array2 = new global::UnityEngine.Vector2[this.agents.Count];
				if (this.interpolatedVelocities != null)
				{
					for (int i = 0; i < this.interpolatedVelocities.Length; i++)
					{
						array[i] = this.interpolatedVelocities[i];
					}
				}
				if (this.interpolatedRotations != null)
				{
					for (int j = 0; j < this.interpolatedRotations.Length; j++)
					{
						array2[j] = this.interpolatedRotations[j];
					}
				}
				this.interpolatedVelocities = array;
				this.interpolatedRotations = array2;
			}
			for (int k = 0; k < this.agents.Count; k++)
			{
				global::Pathfinding.RVO.IAgent agent = this.agents[k];
				global::UnityEngine.Vector2 vector = agent.Position;
				global::UnityEngine.Vector2 b = global::UnityEngine.Vector2.ClampMagnitude(agent.CalculatedTargetPoint - vector, agent.CalculatedSpeed * global::UnityEngine.Time.deltaTime);
				vector += b;
				agent.Position = vector;
				agent.ElevationCoordinate = 0f;
				global::UnityEngine.Vector2 vector2 = new global::UnityEngine.Vector2(this.goals[k].x, this.goals[k].z);
				float magnitude = (vector2 - vector).magnitude;
				agent.SetTarget(vector2, global::UnityEngine.Mathf.Min(magnitude, this.maxSpeed), this.maxSpeed * 1.1f);
				this.interpolatedVelocities[k] += b;
				if (this.interpolatedVelocities[k].magnitude > this.maxSpeed * 0.1f)
				{
					this.interpolatedVelocities[k] = global::UnityEngine.Vector2.ClampMagnitude(this.interpolatedVelocities[k], this.maxSpeed * 0.1f);
					this.interpolatedRotations[k] = global::UnityEngine.Vector2.Lerp(this.interpolatedRotations[k], this.interpolatedVelocities[k], agent.CalculatedSpeed * global::UnityEngine.Time.deltaTime * 4f);
				}
				global::UnityEngine.Vector3 vector3 = new global::UnityEngine.Vector3(this.interpolatedRotations[k].x, 0f, this.interpolatedRotations[k].y);
				global::UnityEngine.Vector3 vector4 = vector3.normalized * agent.Radius;
				if (vector4 == global::UnityEngine.Vector3.zero)
				{
					vector4 = new global::UnityEngine.Vector3(0f, 0f, agent.Radius);
				}
				global::UnityEngine.Vector3 b2 = global::UnityEngine.Vector3.Cross(global::UnityEngine.Vector3.up, vector4);
				global::UnityEngine.Vector3 a = new global::UnityEngine.Vector3(agent.Position.x, agent.ElevationCoordinate, agent.Position.y) + this.renderingOffset;
				int num = 4 * k;
				int num2 = 6 * k;
				this.verts[num] = a + vector4 - b2;
				this.verts[num + 1] = a + vector4 + b2;
				this.verts[num + 2] = a - vector4 + b2;
				this.verts[num + 3] = a - vector4 - b2;
				this.uv[num] = new global::UnityEngine.Vector2(0f, 1f);
				this.uv[num + 1] = new global::UnityEngine.Vector2(1f, 1f);
				this.uv[num + 2] = new global::UnityEngine.Vector2(1f, 0f);
				this.uv[num + 3] = new global::UnityEngine.Vector2(0f, 0f);
				this.meshColors[num] = this.colors[k];
				this.meshColors[num + 1] = this.colors[k];
				this.meshColors[num + 2] = this.colors[k];
				this.meshColors[num + 3] = this.colors[k];
				this.tris[num2] = num;
				this.tris[num2 + 1] = num + 1;
				this.tris[num2 + 2] = num + 2;
				this.tris[num2 + 3] = num;
				this.tris[num2 + 4] = num + 2;
				this.tris[num2 + 5] = num + 3;
			}
			this.mesh.Clear();
			this.mesh.vertices = this.verts;
			this.mesh.uv = this.uv;
			this.mesh.colors = this.meshColors;
			this.mesh.triangles = this.tris;
			this.mesh.RecalculateNormals();
		}

		public int agentCount = 100;

		public float exampleScale = 100f;

		public global::Pathfinding.Examples.LightweightRVO.RVOExampleType type;

		public float radius = 3f;

		public float maxSpeed = 2f;

		public float agentTimeHorizon = 10f;

		[global::UnityEngine.HideInInspector]
		public float obstacleTimeHorizon = 10f;

		public int maxNeighbours = 10;

		public float neighbourDist = 15f;

		public global::UnityEngine.Vector3 renderingOffset = global::UnityEngine.Vector3.up * 0.1f;

		public bool debug;

		private global::UnityEngine.Mesh mesh;

		private global::Pathfinding.RVO.Simulator sim;

		private global::System.Collections.Generic.List<global::Pathfinding.RVO.IAgent> agents;

		private global::System.Collections.Generic.List<global::UnityEngine.Vector3> goals;

		private global::System.Collections.Generic.List<global::UnityEngine.Color> colors;

		private global::UnityEngine.Vector3[] verts;

		private global::UnityEngine.Vector2[] uv;

		private int[] tris;

		private global::UnityEngine.Color[] meshColors;

		private global::UnityEngine.Vector2[] interpolatedVelocities;

		private global::UnityEngine.Vector2[] interpolatedRotations;

		public enum RVOExampleType
		{
			Circle,
			Line,
			Point,
			RandomStreams,
			Crossing
		}
	}
}
