using System;
using UnityEngine;

namespace Pathfinding.RVO
{
	[global::UnityEngine.AddComponentMenu("Pathfinding/Local Avoidance/RVO Controller")]
	[global::UnityEngine.HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_r_v_o_1_1_r_v_o_controller.php")]
	public class RVOController : global::UnityEngine.MonoBehaviour
	{
		public global::Pathfinding.RVO.IAgent rvoAgent { get; private set; }

		public global::Pathfinding.RVO.Simulator simulator { get; private set; }

		public global::UnityEngine.Vector3 position
		{
			get
			{
				return this.To3D(this.rvoAgent.Position, this.rvoAgent.ElevationCoordinate);
			}
		}

		public global::UnityEngine.Vector3 velocity
		{
			get
			{
				if (global::UnityEngine.Time.deltaTime > 1E-05f)
				{
					return this.CalculateMovementDelta(global::UnityEngine.Time.deltaTime) / global::UnityEngine.Time.deltaTime;
				}
				return global::UnityEngine.Vector3.zero;
			}
		}

		public global::UnityEngine.Vector3 CalculateMovementDelta(float deltaTime)
		{
			return this.To3D(global::UnityEngine.Vector2.ClampMagnitude(this.rvoAgent.CalculatedTargetPoint - this.To2D(this.tr.position), this.rvoAgent.CalculatedSpeed * deltaTime), 0f);
		}

		public global::UnityEngine.Vector3 CalculateMovementDelta(global::UnityEngine.Vector3 position, float deltaTime)
		{
			return this.To3D(global::UnityEngine.Vector2.ClampMagnitude(this.rvoAgent.CalculatedTargetPoint - this.To2D(position), this.rvoAgent.CalculatedSpeed * deltaTime), 0f);
		}

		public void SetCollisionNormal(global::UnityEngine.Vector3 normal)
		{
			this.rvoAgent.SetCollisionNormal(this.To2D(normal));
		}

		public void ForceSetVelocity(global::UnityEngine.Vector3 velocity)
		{
			this.rvoAgent.ForceSetVelocity(this.To2D(velocity));
		}

		private global::UnityEngine.Vector2 To2D(global::UnityEngine.Vector3 p)
		{
			float num;
			return this.To2D(p, out num);
		}

		private global::UnityEngine.Vector2 To2D(global::UnityEngine.Vector3 p, out float elevation)
		{
			if (this.movementMode == global::Pathfinding.RVO.MovementMode.XY)
			{
				elevation = p.z;
				return new global::UnityEngine.Vector2(p.x, p.y);
			}
			elevation = p.y;
			return new global::UnityEngine.Vector2(p.x, p.z);
		}

		private global::UnityEngine.Vector3 To3D(global::UnityEngine.Vector2 p, float elevationCoordinate)
		{
			if (this.movementMode == global::Pathfinding.RVO.MovementMode.XY)
			{
				return new global::UnityEngine.Vector3(p.x, p.y, elevationCoordinate);
			}
			return new global::UnityEngine.Vector3(p.x, elevationCoordinate, p.y);
		}

		public void OnDisable()
		{
			if (this.simulator == null)
			{
				return;
			}
			this.simulator.RemoveAgent(this.rvoAgent);
		}

		public void Awake()
		{
			this.tr = base.transform;
			if (global::Pathfinding.RVO.RVOController.cachedSimulator == null)
			{
				global::Pathfinding.RVO.RVOController.cachedSimulator = global::UnityEngine.Object.FindObjectOfType<global::Pathfinding.RVO.RVOSimulator>();
			}
			if (global::Pathfinding.RVO.RVOController.cachedSimulator == null)
			{
				global::UnityEngine.Debug.LogError("No RVOSimulator component found in the scene. Please add one.");
			}
			else
			{
				this.simulator = global::Pathfinding.RVO.RVOController.cachedSimulator.GetSimulator();
			}
		}

		public void OnEnable()
		{
			if (this.simulator == null)
			{
				return;
			}
			if (this.rvoAgent != null)
			{
				this.simulator.AddAgent(this.rvoAgent);
			}
			else
			{
				float elevationCoordinate;
				global::UnityEngine.Vector2 position = this.To2D(base.transform.position, out elevationCoordinate);
				this.rvoAgent = this.simulator.AddAgent(position, elevationCoordinate);
				this.rvoAgent.PreCalculationCallback = new global::System.Action(this.UpdateAgentProperties);
			}
			this.UpdateAgentProperties();
		}

		protected void UpdateAgentProperties()
		{
			this.rvoAgent.Radius = global::UnityEngine.Mathf.Max(0.001f, this.radius);
			this.rvoAgent.AgentTimeHorizon = this.agentTimeHorizon;
			this.rvoAgent.ObstacleTimeHorizon = this.obstacleTimeHorizon;
			this.rvoAgent.Locked = this.locked;
			this.rvoAgent.MaxNeighbours = this.maxNeighbours;
			this.rvoAgent.DebugDraw = this.debug;
			this.rvoAgent.NeighbourDist = this.neighbourDist;
			this.rvoAgent.Layer = this.layer;
			this.rvoAgent.CollidesWith = this.collidesWith;
			this.rvoAgent.MovementMode = this.movementMode;
			this.rvoAgent.Priority = this.priority;
			float num;
			this.rvoAgent.Position = this.To2D(base.transform.position, out num);
			if (this.movementMode == global::Pathfinding.RVO.MovementMode.XZ)
			{
				this.rvoAgent.Height = this.height;
				this.rvoAgent.ElevationCoordinate = num + this.center - 0.5f * this.height;
			}
			else
			{
				this.rvoAgent.Height = 1f;
				this.rvoAgent.ElevationCoordinate = 0f;
			}
		}

		public void SetTarget(global::UnityEngine.Vector3 pos, float speed, float maxSpeed)
		{
			this.rvoAgent.SetTarget(this.To2D(pos), speed, maxSpeed);
			if (this.lockWhenNotMoving)
			{
				this.locked = (speed < 0.001f);
			}
		}

		public void Move(global::UnityEngine.Vector3 vel)
		{
			global::UnityEngine.Vector2 b = this.To2D(vel);
			float magnitude = b.magnitude;
			this.rvoAgent.SetTarget(this.To2D(this.tr.position) + b, magnitude, magnitude);
			if (this.lockWhenNotMoving)
			{
				this.locked = (magnitude < 0.001f);
			}
		}

		public void Teleport(global::UnityEngine.Vector3 pos)
		{
			this.tr.position = pos;
		}

		public void Update()
		{
		}

		private static void DrawCircle(global::UnityEngine.Vector3 p, float radius, float a0, float a1)
		{
			while (a0 > a1)
			{
				a0 -= 6.28318548f;
			}
			global::UnityEngine.Vector3 b = new global::UnityEngine.Vector3(global::UnityEngine.Mathf.Cos(a0) * radius, 0f, global::UnityEngine.Mathf.Sin(a0) * radius);
			int num = 0;
			while ((float)num <= 40f)
			{
				global::UnityEngine.Vector3 vector = new global::UnityEngine.Vector3(global::UnityEngine.Mathf.Cos(global::UnityEngine.Mathf.Lerp(a0, a1, (float)num / 40f)) * radius, 0f, global::UnityEngine.Mathf.Sin(global::UnityEngine.Mathf.Lerp(a0, a1, (float)num / 40f)) * radius);
				global::UnityEngine.Gizmos.DrawLine(p + b, p + vector);
				b = vector;
				num++;
			}
		}

		private static void DrawCylinder(global::UnityEngine.Vector3 p, global::UnityEngine.Vector3 up, float height, float radius)
		{
			global::UnityEngine.Vector3 normalized = global::UnityEngine.Vector3.Cross(up, global::UnityEngine.Vector3.one).normalized;
			global::UnityEngine.Gizmos.matrix = global::UnityEngine.Matrix4x4.TRS(p, global::UnityEngine.Quaternion.LookRotation(normalized, up), new global::UnityEngine.Vector3(radius, height, radius));
			global::Pathfinding.RVO.RVOController.DrawCircle(new global::UnityEngine.Vector2(0f, 0f), 1f, 0f, 6.28318548f);
			if (height > 0f)
			{
				global::Pathfinding.RVO.RVOController.DrawCircle(new global::UnityEngine.Vector2(0f, 1f), 1f, 0f, 6.28318548f);
				global::UnityEngine.Gizmos.DrawLine(new global::UnityEngine.Vector3(1f, 0f, 0f), new global::UnityEngine.Vector3(1f, 1f, 0f));
				global::UnityEngine.Gizmos.DrawLine(new global::UnityEngine.Vector3(-1f, 0f, 0f), new global::UnityEngine.Vector3(-1f, 1f, 0f));
				global::UnityEngine.Gizmos.DrawLine(new global::UnityEngine.Vector3(0f, 0f, 1f), new global::UnityEngine.Vector3(0f, 1f, 1f));
				global::UnityEngine.Gizmos.DrawLine(new global::UnityEngine.Vector3(0f, 0f, -1f), new global::UnityEngine.Vector3(0f, 1f, -1f));
			}
		}

		private void OnDrawGizmos()
		{
			if (this.locked)
			{
				global::UnityEngine.Gizmos.color = global::Pathfinding.RVO.RVOController.GizmoColor * 0.5f;
			}
			else
			{
				global::UnityEngine.Gizmos.color = global::Pathfinding.RVO.RVOController.GizmoColor;
			}
			if (this.movementMode == global::Pathfinding.RVO.MovementMode.XY)
			{
				global::Pathfinding.RVO.RVOController.DrawCylinder(base.transform.position, global::UnityEngine.Vector3.forward, 0f, this.radius);
			}
			else
			{
				global::Pathfinding.RVO.RVOController.DrawCylinder(base.transform.position + this.To3D(global::UnityEngine.Vector2.zero, this.center - this.height * 0.5f), this.To3D(global::UnityEngine.Vector2.zero, 1f), this.height, this.radius);
			}
		}

		private void OnDrawGizmosSelected()
		{
		}

		public global::Pathfinding.RVO.MovementMode movementMode;

		[global::UnityEngine.Tooltip("Radius of the agent")]
		public float radius = 5f;

		[global::UnityEngine.Tooltip("Height of the agent. In world units")]
		public float height = 1f;

		[global::UnityEngine.Tooltip("A locked unit cannot move. Other units will still avoid it. But avoidance quality is not the best")]
		public bool locked;

		[global::UnityEngine.Tooltip("Automatically set #locked to true when desired velocity is approximately zero")]
		public bool lockWhenNotMoving = true;

		[global::UnityEngine.Tooltip("How far in the time to look for collisions with other agents")]
		public float agentTimeHorizon = 2f;

		public float obstacleTimeHorizon = 2f;

		[global::UnityEngine.Tooltip("Maximum distance to other agents to take them into account for collisions.\nDecreasing this value can lead to better performance, increasing it can lead to better quality of the simulation")]
		public float neighbourDist = 10f;

		[global::UnityEngine.Tooltip("Max number of other agents to take into account.\nA smaller value can reduce CPU load, a higher value can lead to better local avoidance quality.")]
		public int maxNeighbours = 10;

		public global::Pathfinding.RVO.RVOLayer layer = global::Pathfinding.RVO.RVOLayer.DefaultAgent;

		[global::Pathfinding.AstarEnumFlag]
		public global::Pathfinding.RVO.RVOLayer collidesWith = (global::Pathfinding.RVO.RVOLayer)(-1);

		[global::UnityEngine.HideInInspector]
		public float wallAvoidForce = 1f;

		[global::UnityEngine.HideInInspector]
		public float wallAvoidFalloff = 1f;

		[global::UnityEngine.Tooltip("How strongly other agents will avoid this agent")]
		[global::UnityEngine.Range(0f, 1f)]
		public float priority = 0.5f;

		[global::UnityEngine.Tooltip("Center of the agent relative to the pivot point of this game object")]
		public float center;

		private global::UnityEngine.Transform tr;

		public bool debug;

		private static global::Pathfinding.RVO.RVOSimulator cachedSimulator;

		private static readonly global::UnityEngine.Color GizmoColor = new global::UnityEngine.Color(0.9411765f, 0.8352941f, 0.117647059f);
	}
}
