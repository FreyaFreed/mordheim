using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
	[global::UnityEngine.AddComponentMenu("Pathfinding/Link3")]
	[global::UnityEngine.HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_node_link3.php")]
	public class NodeLink3 : global::Pathfinding.GraphModifier
	{
		public static global::Pathfinding.NodeLink3 GetNodeLink(global::Pathfinding.GraphNode node)
		{
			global::Pathfinding.NodeLink3 result;
			global::Pathfinding.NodeLink3.reference.TryGetValue(node, out result);
			return result;
		}

		public global::UnityEngine.Transform StartTransform
		{
			get
			{
				return base.transform;
			}
		}

		public global::UnityEngine.Transform EndTransform
		{
			get
			{
				return this.end;
			}
		}

		public global::Pathfinding.GraphNode StartNode
		{
			get
			{
				return this.startNode;
			}
		}

		public global::Pathfinding.GraphNode EndNode
		{
			get
			{
				return this.endNode;
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
			if (global::AstarPath.active.astarData.pointGraph == null)
			{
				global::AstarPath.active.astarData.AddGraph(new global::Pathfinding.PointGraph());
			}
			this.startNode = global::AstarPath.active.astarData.pointGraph.AddNode<global::Pathfinding.NodeLink3Node>(new global::Pathfinding.NodeLink3Node(global::AstarPath.active), (global::Pathfinding.Int3)this.StartTransform.position);
			this.startNode.link = this;
			this.endNode = global::AstarPath.active.astarData.pointGraph.AddNode<global::Pathfinding.NodeLink3Node>(new global::Pathfinding.NodeLink3Node(global::AstarPath.active), (global::Pathfinding.Int3)this.EndTransform.position);
			this.endNode.link = this;
			this.connectedNode1 = null;
			this.connectedNode2 = null;
			if (this.startNode == null || this.endNode == null)
			{
				this.startNode = null;
				this.endNode = null;
				return;
			}
			this.postScanCalled = true;
			global::Pathfinding.NodeLink3.reference[this.startNode] = this;
			global::Pathfinding.NodeLink3.reference[this.endNode] = this;
			this.Apply(true);
		}

		public override void OnGraphsPostUpdate()
		{
			if (!global::AstarPath.active.isScanning)
			{
				if (this.connectedNode1 != null && this.connectedNode1.Destroyed)
				{
					this.connectedNode1 = null;
				}
				if (this.connectedNode2 != null && this.connectedNode2.Destroyed)
				{
					this.connectedNode2 = null;
				}
				if (!this.postScanCalled)
				{
					this.OnPostScan();
				}
				else
				{
					this.Apply(false);
				}
			}
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			if (global::UnityEngine.Application.isPlaying && global::AstarPath.active != null && global::AstarPath.active.astarData != null && global::AstarPath.active.astarData.pointGraph != null)
			{
				this.OnGraphsPostUpdate();
			}
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			this.postScanCalled = false;
			if (this.startNode != null)
			{
				global::Pathfinding.NodeLink3.reference.Remove(this.startNode);
			}
			if (this.endNode != null)
			{
				global::Pathfinding.NodeLink3.reference.Remove(this.endNode);
			}
			if (this.startNode != null && this.endNode != null)
			{
				this.startNode.RemoveConnection(this.endNode);
				this.endNode.RemoveConnection(this.startNode);
				if (this.connectedNode1 != null && this.connectedNode2 != null)
				{
					this.startNode.RemoveConnection(this.connectedNode1);
					this.connectedNode1.RemoveConnection(this.startNode);
					this.endNode.RemoveConnection(this.connectedNode2);
					this.connectedNode2.RemoveConnection(this.endNode);
				}
			}
		}

		private void RemoveConnections(global::Pathfinding.GraphNode node)
		{
			node.ClearConnections(true);
		}

		[global::UnityEngine.ContextMenu("Recalculate neighbours")]
		private void ContextApplyForce()
		{
			if (global::UnityEngine.Application.isPlaying)
			{
				this.Apply(true);
				if (global::AstarPath.active != null)
				{
					global::AstarPath.active.FloodFill();
				}
			}
		}

		public void Apply(bool forceNewCheck)
		{
			global::Pathfinding.NNConstraint none = global::Pathfinding.NNConstraint.None;
			none.distanceXZ = true;
			int graphIndex = (int)this.startNode.GraphIndex;
			none.graphMask = ~(1 << graphIndex);
			bool flag = true;
			global::Pathfinding.NNInfo nearest = global::AstarPath.active.GetNearest(this.StartTransform.position, none);
			flag &= (nearest.node == this.connectedNode1 && nearest.node != null);
			this.connectedNode1 = (nearest.node as global::Pathfinding.MeshNode);
			this.clamped1 = nearest.position;
			if (this.connectedNode1 != null)
			{
				global::UnityEngine.Debug.DrawRay((global::UnityEngine.Vector3)this.connectedNode1.position, global::UnityEngine.Vector3.up * 5f, global::UnityEngine.Color.red);
			}
			global::Pathfinding.NNInfo nearest2 = global::AstarPath.active.GetNearest(this.EndTransform.position, none);
			flag &= (nearest2.node == this.connectedNode2 && nearest2.node != null);
			this.connectedNode2 = (nearest2.node as global::Pathfinding.MeshNode);
			this.clamped2 = nearest2.position;
			if (this.connectedNode2 != null)
			{
				global::UnityEngine.Debug.DrawRay((global::UnityEngine.Vector3)this.connectedNode2.position, global::UnityEngine.Vector3.up * 5f, global::UnityEngine.Color.cyan);
			}
			if (this.connectedNode2 == null || this.connectedNode1 == null)
			{
				return;
			}
			this.startNode.SetPosition((global::Pathfinding.Int3)this.StartTransform.position);
			this.endNode.SetPosition((global::Pathfinding.Int3)this.EndTransform.position);
			if (flag && !forceNewCheck)
			{
				return;
			}
			this.RemoveConnections(this.startNode);
			this.RemoveConnections(this.endNode);
			uint cost = (uint)global::UnityEngine.Mathf.RoundToInt((float)((global::Pathfinding.Int3)(this.StartTransform.position - this.EndTransform.position)).costMagnitude * this.costFactor);
			this.startNode.AddConnection(this.endNode, cost);
			this.endNode.AddConnection(this.startNode, cost);
			global::Pathfinding.Int3 rhs = this.connectedNode2.position - this.connectedNode1.position;
			for (int i = 0; i < this.connectedNode1.GetVertexCount(); i++)
			{
				global::Pathfinding.Int3 vertex = this.connectedNode1.GetVertex(i);
				global::Pathfinding.Int3 vertex2 = this.connectedNode1.GetVertex((i + 1) % this.connectedNode1.GetVertexCount());
				if (global::Pathfinding.Int3.DotLong((vertex2 - vertex).Normal2D(), rhs) <= 0L)
				{
					for (int j = 0; j < this.connectedNode2.GetVertexCount(); j++)
					{
						global::Pathfinding.Int3 vertex3 = this.connectedNode2.GetVertex(j);
						global::Pathfinding.Int3 vertex4 = this.connectedNode2.GetVertex((j + 1) % this.connectedNode2.GetVertexCount());
						if (global::Pathfinding.Int3.DotLong((vertex4 - vertex3).Normal2D(), rhs) >= 0L)
						{
							if ((double)global::Pathfinding.Int3.Angle(vertex4 - vertex3, vertex2 - vertex) > 2.9670598109563189)
							{
								float num = 0f;
								float num2 = 1f;
								num2 = global::System.Math.Min(num2, global::Pathfinding.VectorMath.ClosestPointOnLineFactor(vertex, vertex2, vertex3));
								num = global::System.Math.Max(num, global::Pathfinding.VectorMath.ClosestPointOnLineFactor(vertex, vertex2, vertex4));
								if (num2 >= num)
								{
									global::UnityEngine.Vector3 vector = (global::UnityEngine.Vector3)(vertex2 - vertex) * num + (global::UnityEngine.Vector3)vertex;
									global::UnityEngine.Vector3 vector2 = (global::UnityEngine.Vector3)(vertex2 - vertex) * num2 + (global::UnityEngine.Vector3)vertex;
									this.startNode.portalA = vector;
									this.startNode.portalB = vector2;
									this.endNode.portalA = vector2;
									this.endNode.portalB = vector;
									this.connectedNode1.AddConnection(this.startNode, (uint)global::UnityEngine.Mathf.RoundToInt((float)((global::Pathfinding.Int3)(this.clamped1 - this.StartTransform.position)).costMagnitude * this.costFactor));
									this.connectedNode2.AddConnection(this.endNode, (uint)global::UnityEngine.Mathf.RoundToInt((float)((global::Pathfinding.Int3)(this.clamped2 - this.EndTransform.position)).costMagnitude * this.costFactor));
									this.startNode.AddConnection(this.connectedNode1, (uint)global::UnityEngine.Mathf.RoundToInt((float)((global::Pathfinding.Int3)(this.clamped1 - this.StartTransform.position)).costMagnitude * this.costFactor));
									this.endNode.AddConnection(this.connectedNode2, (uint)global::UnityEngine.Mathf.RoundToInt((float)((global::Pathfinding.Int3)(this.clamped2 - this.EndTransform.position)).costMagnitude * this.costFactor));
									return;
								}
								global::UnityEngine.Debug.LogError(string.Concat(new object[]
								{
									"Wait wut!? ",
									num,
									" ",
									num2,
									" ",
									vertex,
									" ",
									vertex2,
									" ",
									vertex3,
									" ",
									vertex4,
									"\nTODO, fix this error"
								}));
							}
						}
					}
				}
			}
		}

		private void DrawCircle(global::UnityEngine.Vector3 o, float r, int detail, global::UnityEngine.Color col)
		{
			global::UnityEngine.Vector3 from = new global::UnityEngine.Vector3(global::UnityEngine.Mathf.Cos(0f) * r, 0f, global::UnityEngine.Mathf.Sin(0f) * r) + o;
			global::UnityEngine.Gizmos.color = col;
			for (int i = 0; i <= detail; i++)
			{
				float f = (float)i * 3.14159274f * 2f / (float)detail;
				global::UnityEngine.Vector3 vector = new global::UnityEngine.Vector3(global::UnityEngine.Mathf.Cos(f) * r, 0f, global::UnityEngine.Mathf.Sin(f) * r) + o;
				global::UnityEngine.Gizmos.DrawLine(from, vector);
				from = vector;
			}
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

		public virtual void OnDrawGizmosSelected()
		{
			this.OnDrawGizmos(true);
		}

		public void OnDrawGizmos()
		{
			this.OnDrawGizmos(false);
		}

		public void OnDrawGizmos(bool selected)
		{
			global::UnityEngine.Color color = (!selected) ? global::Pathfinding.NodeLink3.GizmosColor : global::Pathfinding.NodeLink3.GizmosColorSelected;
			if (this.StartTransform != null)
			{
				this.DrawCircle(this.StartTransform.position, 0.4f, 10, color);
			}
			if (this.EndTransform != null)
			{
				this.DrawCircle(this.EndTransform.position, 0.4f, 10, color);
			}
			if (this.StartTransform != null && this.EndTransform != null)
			{
				global::UnityEngine.Gizmos.color = color;
				this.DrawGizmoBezier(this.StartTransform.position, this.EndTransform.position);
				if (selected)
				{
					global::UnityEngine.Vector3 normalized = global::UnityEngine.Vector3.Cross(global::UnityEngine.Vector3.up, this.EndTransform.position - this.StartTransform.position).normalized;
					this.DrawGizmoBezier(this.StartTransform.position + normalized * 0.1f, this.EndTransform.position + normalized * 0.1f);
					this.DrawGizmoBezier(this.StartTransform.position - normalized * 0.1f, this.EndTransform.position - normalized * 0.1f);
				}
			}
		}

		protected static global::System.Collections.Generic.Dictionary<global::Pathfinding.GraphNode, global::Pathfinding.NodeLink3> reference = new global::System.Collections.Generic.Dictionary<global::Pathfinding.GraphNode, global::Pathfinding.NodeLink3>();

		public global::UnityEngine.Transform end;

		public float costFactor = 1f;

		public bool oneWay;

		private global::Pathfinding.NodeLink3Node startNode;

		private global::Pathfinding.NodeLink3Node endNode;

		private global::Pathfinding.MeshNode connectedNode1;

		private global::Pathfinding.MeshNode connectedNode2;

		private global::UnityEngine.Vector3 clamped1;

		private global::UnityEngine.Vector3 clamped2;

		private bool postScanCalled;

		private static readonly global::UnityEngine.Color GizmosColor = new global::UnityEngine.Color(0.807843149f, 0.533333361f, 0.1882353f, 0.5f);

		private static readonly global::UnityEngine.Color GizmosColorSelected = new global::UnityEngine.Color(0.921568632f, 0.482352942f, 0.1254902f, 1f);
	}
}
