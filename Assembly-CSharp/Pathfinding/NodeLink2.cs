using System;
using System.Collections.Generic;
using Pathfinding.Serialization;
using UnityEngine;

namespace Pathfinding
{
	[global::UnityEngine.AddComponentMenu("Pathfinding/Link2")]
	[global::UnityEngine.HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_node_link2.php")]
	public class NodeLink2 : global::Pathfinding.GraphModifier
	{
		public static global::Pathfinding.NodeLink2 GetNodeLink(global::Pathfinding.GraphNode node)
		{
			global::Pathfinding.NodeLink2 result;
			global::Pathfinding.NodeLink2.reference.TryGetValue(node, out result);
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

		public global::Pathfinding.PointNode startNode { get; private set; }

		public global::Pathfinding.PointNode endNode { get; private set; }

		[global::System.Obsolete("Use startNode instead (lowercase s)")]
		public global::Pathfinding.GraphNode StartNode
		{
			get
			{
				return this.startNode;
			}
		}

		[global::System.Obsolete("Use endNode instead (lowercase e)")]
		public global::Pathfinding.GraphNode EndNode
		{
			get
			{
				return this.endNode;
			}
		}

		public override void OnPostScan()
		{
			this.InternalOnPostScan();
		}

		public void InternalOnPostScan()
		{
			if (this.EndTransform == null || this.StartTransform == null)
			{
				return;
			}
			if (global::AstarPath.active.astarData.pointGraph == null)
			{
				global::Pathfinding.PointGraph pointGraph = global::AstarPath.active.astarData.AddGraph(typeof(global::Pathfinding.PointGraph)) as global::Pathfinding.PointGraph;
				pointGraph.name = "PointGraph (used for node links)";
			}
			if (this.startNode != null)
			{
				global::Pathfinding.NodeLink2.reference.Remove(this.startNode);
			}
			if (this.endNode != null)
			{
				global::Pathfinding.NodeLink2.reference.Remove(this.endNode);
			}
			this.startNode = global::AstarPath.active.astarData.pointGraph.AddNode((global::Pathfinding.Int3)this.StartTransform.position);
			this.endNode = global::AstarPath.active.astarData.pointGraph.AddNode((global::Pathfinding.Int3)this.EndTransform.position);
			this.connectedNode1 = null;
			this.connectedNode2 = null;
			if (this.startNode == null || this.endNode == null)
			{
				this.startNode = null;
				this.endNode = null;
				return;
			}
			this.postScanCalled = true;
			global::Pathfinding.NodeLink2.reference[this.startNode] = this;
			global::Pathfinding.NodeLink2.reference[this.endNode] = this;
			this.Apply(true);
		}

		public override void OnGraphsPostUpdate()
		{
			if (global::AstarPath.active.isScanning)
			{
				return;
			}
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

		protected override void OnEnable()
		{
			base.OnEnable();
			if (global::UnityEngine.Application.isPlaying && global::AstarPath.active != null && global::AstarPath.active.astarData != null && global::AstarPath.active.astarData.pointGraph != null && !global::AstarPath.active.isScanning)
			{
				global::AstarPath.RegisterSafeUpdate(new global::System.Action(this.OnGraphsPostUpdate));
			}
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			this.postScanCalled = false;
			if (this.startNode != null)
			{
				global::Pathfinding.NodeLink2.reference.Remove(this.startNode);
			}
			if (this.endNode != null)
			{
				global::Pathfinding.NodeLink2.reference.Remove(this.endNode);
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
			int graphIndex = (int)this.startNode.GraphIndex;
			none.graphMask = ~(1 << graphIndex);
			this.startNode.SetPosition((global::Pathfinding.Int3)this.StartTransform.position);
			this.endNode.SetPosition((global::Pathfinding.Int3)this.EndTransform.position);
			this.RemoveConnections(this.startNode);
			this.RemoveConnections(this.endNode);
			uint cost = (uint)global::UnityEngine.Mathf.RoundToInt((float)((global::Pathfinding.Int3)(this.StartTransform.position - this.EndTransform.position)).costMagnitude * this.costFactor);
			this.startNode.AddConnection(this.endNode, cost);
			this.endNode.AddConnection(this.startNode, cost);
			if (this.connectedNode1 == null || forceNewCheck)
			{
				global::Pathfinding.NNInfo nearest = global::AstarPath.active.GetNearest(this.StartTransform.position, none);
				this.connectedNode1 = nearest.node;
				this.clamped1 = nearest.position;
			}
			if (this.connectedNode2 == null || forceNewCheck)
			{
				global::Pathfinding.NNInfo nearest2 = global::AstarPath.active.GetNearest(this.EndTransform.position, none);
				this.connectedNode2 = nearest2.node;
				this.clamped2 = nearest2.position;
			}
			if (this.connectedNode2 == null || this.connectedNode1 == null)
			{
				return;
			}
			this.connectedNode1.AddConnection(this.startNode, (uint)global::UnityEngine.Mathf.RoundToInt((float)((global::Pathfinding.Int3)(this.clamped1 - this.StartTransform.position)).costMagnitude * this.costFactor));
			if (!this.oneWay)
			{
				this.connectedNode2.AddConnection(this.endNode, (uint)global::UnityEngine.Mathf.RoundToInt((float)((global::Pathfinding.Int3)(this.clamped2 - this.EndTransform.position)).costMagnitude * this.costFactor));
			}
			if (!this.oneWay)
			{
				this.startNode.AddConnection(this.connectedNode1, (uint)global::UnityEngine.Mathf.RoundToInt((float)((global::Pathfinding.Int3)(this.clamped1 - this.StartTransform.position)).costMagnitude * this.costFactor));
			}
			this.endNode.AddConnection(this.connectedNode2, (uint)global::UnityEngine.Mathf.RoundToInt((float)((global::Pathfinding.Int3)(this.clamped2 - this.EndTransform.position)).costMagnitude * this.costFactor));
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
			global::UnityEngine.Color color = (!selected) ? global::Pathfinding.NodeLink2.GizmosColor : global::Pathfinding.NodeLink2.GizmosColorSelected;
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

		internal static void SerializeReferences(global::Pathfinding.Serialization.GraphSerializationContext ctx)
		{
			global::System.Collections.Generic.List<global::Pathfinding.NodeLink2> modifiersOfType = global::Pathfinding.GraphModifier.GetModifiersOfType<global::Pathfinding.NodeLink2>();
			ctx.writer.Write(modifiersOfType.Count);
			foreach (global::Pathfinding.NodeLink2 nodeLink in modifiersOfType)
			{
				ctx.writer.Write(nodeLink.uniqueID);
				ctx.SerializeNodeReference(nodeLink.startNode);
				ctx.SerializeNodeReference(nodeLink.endNode);
				ctx.SerializeNodeReference(nodeLink.connectedNode1);
				ctx.SerializeNodeReference(nodeLink.connectedNode2);
				ctx.SerializeVector3(nodeLink.clamped1);
				ctx.SerializeVector3(nodeLink.clamped2);
				ctx.writer.Write(nodeLink.postScanCalled);
			}
		}

		internal static void DeserializeReferences(global::Pathfinding.Serialization.GraphSerializationContext ctx)
		{
			int num = ctx.reader.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				ulong key = ctx.reader.ReadUInt64();
				global::Pathfinding.GraphNode graphNode = ctx.DeserializeNodeReference();
				global::Pathfinding.GraphNode graphNode2 = ctx.DeserializeNodeReference();
				global::Pathfinding.GraphNode graphNode3 = ctx.DeserializeNodeReference();
				global::Pathfinding.GraphNode graphNode4 = ctx.DeserializeNodeReference();
				global::UnityEngine.Vector3 vector = ctx.DeserializeVector3();
				global::UnityEngine.Vector3 vector2 = ctx.DeserializeVector3();
				bool flag = ctx.reader.ReadBoolean();
				global::Pathfinding.GraphModifier graphModifier;
				if (!global::Pathfinding.GraphModifier.usedIDs.TryGetValue(key, out graphModifier))
				{
					throw new global::System.Exception("Tried to deserialize a NodeLink2 reference, but the link could not be found in the scene.\nIf a NodeLink2 is included in serialized graph data, the same NodeLink2 component must be present in the scene when loading the graph data.");
				}
				global::Pathfinding.NodeLink2 nodeLink = graphModifier as global::Pathfinding.NodeLink2;
				if (!(nodeLink != null))
				{
					throw new global::System.Exception("Tried to deserialize a NodeLink2 reference, but the link was not of the correct type or it has been destroyed.\nIf a NodeLink2 is included in serialized graph data, the same NodeLink2 component must be present in the scene when loading the graph data.");
				}
				global::Pathfinding.NodeLink2.reference[graphNode] = nodeLink;
				global::Pathfinding.NodeLink2.reference[graphNode2] = nodeLink;
				if (nodeLink.startNode != null)
				{
					global::Pathfinding.NodeLink2.reference.Remove(nodeLink.startNode);
				}
				if (nodeLink.endNode != null)
				{
					global::Pathfinding.NodeLink2.reference.Remove(nodeLink.endNode);
				}
				nodeLink.startNode = (graphNode as global::Pathfinding.PointNode);
				nodeLink.endNode = (graphNode2 as global::Pathfinding.PointNode);
				nodeLink.connectedNode1 = graphNode3;
				nodeLink.connectedNode2 = graphNode4;
				nodeLink.postScanCalled = flag;
				nodeLink.clamped1 = vector;
				nodeLink.clamped2 = vector2;
			}
		}

		protected static global::System.Collections.Generic.Dictionary<global::Pathfinding.GraphNode, global::Pathfinding.NodeLink2> reference = new global::System.Collections.Generic.Dictionary<global::Pathfinding.GraphNode, global::Pathfinding.NodeLink2>();

		public global::UnityEngine.Transform end;

		public float costFactor = 1f;

		public bool oneWay;

		private global::Pathfinding.GraphNode connectedNode1;

		private global::Pathfinding.GraphNode connectedNode2;

		private global::UnityEngine.Vector3 clamped1;

		private global::UnityEngine.Vector3 clamped2;

		private bool postScanCalled;

		private static readonly global::UnityEngine.Color GizmosColor = new global::UnityEngine.Color(0.807843149f, 0.533333361f, 0.1882353f, 0.5f);

		private static readonly global::UnityEngine.Color GizmosColorSelected = new global::UnityEngine.Color(0.921568632f, 0.482352942f, 0.1254902f, 1f);
	}
}
