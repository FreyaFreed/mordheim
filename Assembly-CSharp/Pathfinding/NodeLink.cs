using System;
using UnityEngine;

namespace Pathfinding
{
	[global::UnityEngine.HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_node_link.php")]
	[global::UnityEngine.AddComponentMenu("Pathfinding/Link")]
	public class NodeLink : global::Pathfinding.GraphModifier
	{
		public global::UnityEngine.Transform Start
		{
			get
			{
				return base.transform;
			}
		}

		public global::UnityEngine.Transform End
		{
			get
			{
				return this.end;
			}
		}

		public override void OnPostScan()
		{
			if (global::AstarPath.active.isScanning)
			{
				this.InternalOnPostScan();
			}
			else
			{
				global::AstarPath.active.AddWorkItem(new global::Pathfinding.AstarWorkItem(delegate(bool force)
				{
					this.InternalOnPostScan();
					return true;
				}));
			}
		}

		public void InternalOnPostScan()
		{
			this.Apply();
		}

		public override void OnGraphsPostUpdate()
		{
			if (!global::AstarPath.active.isScanning)
			{
				global::AstarPath.active.AddWorkItem(new global::Pathfinding.AstarWorkItem(delegate(bool force)
				{
					this.InternalOnPostScan();
					return true;
				}));
			}
		}

		public virtual void Apply()
		{
			if (this.Start == null || this.End == null || global::AstarPath.active == null)
			{
				return;
			}
			global::Pathfinding.GraphNode node = global::AstarPath.active.GetNearest(this.Start.position).node;
			global::Pathfinding.GraphNode node2 = global::AstarPath.active.GetNearest(this.End.position).node;
			if (node == null || node2 == null)
			{
				return;
			}
			if (this.deleteConnection)
			{
				node.RemoveConnection(node2);
				if (!this.oneWay)
				{
					node2.RemoveConnection(node);
				}
			}
			else
			{
				uint cost = (uint)global::System.Math.Round((double)((float)(node.position - node2.position).costMagnitude * this.costFactor));
				node.AddConnection(node2, cost);
				if (!this.oneWay)
				{
					node2.AddConnection(node, cost);
				}
			}
		}

		public void OnDrawGizmos()
		{
			if (this.Start == null || this.End == null)
			{
				return;
			}
			global::UnityEngine.Vector3 position = this.Start.position;
			global::UnityEngine.Vector3 position2 = this.End.position;
			global::UnityEngine.Gizmos.color = ((!this.deleteConnection) ? global::UnityEngine.Color.green : global::UnityEngine.Color.red);
			this.DrawGizmoBezier(position, position2);
		}

		private void DrawGizmoBezier(global::UnityEngine.Vector3 p1, global::UnityEngine.Vector3 p2)
		{
			global::UnityEngine.Vector3 vector = p2 - p1;
			if (vector == global::UnityEngine.Vector3.zero)
			{
				return;
			}
			global::UnityEngine.Vector3 rhs = global::UnityEngine.Vector3.Cross(global::UnityEngine.Vector3.up, vector);
			global::UnityEngine.Vector3 vector2 = global::UnityEngine.Vector3.Cross(vector, rhs).normalized;
			vector2 *= vector.magnitude * 0.1f;
			global::UnityEngine.Vector3 p3 = p1 + vector2;
			global::UnityEngine.Vector3 p4 = p2 + vector2;
			global::UnityEngine.Vector3 from = p1;
			for (int i = 1; i <= 20; i++)
			{
				float t = (float)i / 20f;
				global::UnityEngine.Vector3 vector3 = global::Pathfinding.AstarSplines.CubicBezier(p1, p3, p4, p2, t);
				global::UnityEngine.Gizmos.DrawLine(from, vector3);
				from = vector3;
			}
		}

		public global::UnityEngine.Transform end;

		public float costFactor = 1f;

		public bool oneWay;

		public bool deleteConnection;
	}
}
