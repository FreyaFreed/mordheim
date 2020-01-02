using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
	[global::System.Serializable]
	public class StartEndModifier : global::Pathfinding.PathModifier
	{
		public override int Order
		{
			get
			{
				return 0;
			}
		}

		public override void Apply(global::Pathfinding.Path _p)
		{
			global::Pathfinding.ABPath abpath = _p as global::Pathfinding.ABPath;
			if (abpath == null || abpath.vectorPath.Count == 0)
			{
				return;
			}
			if (abpath.vectorPath.Count == 1 && !this.addPoints)
			{
				abpath.vectorPath.Add(abpath.vectorPath[0]);
			}
			bool flag;
			global::UnityEngine.Vector3 vector = this.Snap(abpath, this.exactStartPoint, true, out flag);
			bool flag2;
			global::UnityEngine.Vector3 vector2 = this.Snap(abpath, this.exactEndPoint, false, out flag2);
			if ((flag || this.addPoints) && this.exactStartPoint != global::Pathfinding.StartEndModifier.Exactness.SnapToNode)
			{
				abpath.vectorPath.Insert(0, vector);
			}
			else
			{
				abpath.vectorPath[0] = vector;
			}
			if ((flag2 || this.addPoints) && this.exactEndPoint != global::Pathfinding.StartEndModifier.Exactness.SnapToNode)
			{
				abpath.vectorPath.Add(vector2);
			}
			else
			{
				abpath.vectorPath[abpath.vectorPath.Count - 1] = vector2;
			}
		}

		private global::UnityEngine.Vector3 Snap(global::Pathfinding.ABPath path, global::Pathfinding.StartEndModifier.Exactness mode, bool start, out bool forceAddPoint)
		{
			int num = (!start) ? (path.path.Count - 1) : 0;
			global::Pathfinding.GraphNode graphNode = path.path[num];
			global::UnityEngine.Vector3 vector = (global::UnityEngine.Vector3)graphNode.position;
			forceAddPoint = false;
			switch (mode)
			{
			case global::Pathfinding.StartEndModifier.Exactness.SnapToNode:
				return vector;
			case global::Pathfinding.StartEndModifier.Exactness.Original:
			case global::Pathfinding.StartEndModifier.Exactness.Interpolate:
			case global::Pathfinding.StartEndModifier.Exactness.NodeConnection:
			{
				global::UnityEngine.Vector3 vector2;
				if (start)
				{
					vector2 = ((this.adjustStartPoint == null) ? path.originalStartPoint : this.adjustStartPoint());
				}
				else
				{
					vector2 = path.originalEndPoint;
				}
				switch (mode)
				{
				case global::Pathfinding.StartEndModifier.Exactness.Original:
					return this.GetClampedPoint(vector, vector2, graphNode);
				case global::Pathfinding.StartEndModifier.Exactness.Interpolate:
				{
					global::UnityEngine.Vector3 clampedPoint = this.GetClampedPoint(vector, vector2, graphNode);
					global::Pathfinding.GraphNode graphNode2 = path.path[global::UnityEngine.Mathf.Clamp(num + ((!start) ? -1 : 1), 0, path.path.Count - 1)];
					return global::Pathfinding.VectorMath.ClosestPointOnSegment(vector, (global::UnityEngine.Vector3)graphNode2.position, clampedPoint);
				}
				case global::Pathfinding.StartEndModifier.Exactness.NodeConnection:
				{
					this.connectionBuffer = (this.connectionBuffer ?? new global::System.Collections.Generic.List<global::Pathfinding.GraphNode>());
					global::Pathfinding.GraphNodeDelegate graphNodeDelegate;
					if ((graphNodeDelegate = this.connectionBufferAddDelegate) == null)
					{
						graphNodeDelegate = new global::Pathfinding.GraphNodeDelegate(this.connectionBuffer.Add);
					}
					this.connectionBufferAddDelegate = graphNodeDelegate;
					global::Pathfinding.GraphNode graphNode2 = path.path[global::UnityEngine.Mathf.Clamp(num + ((!start) ? -1 : 1), 0, path.path.Count - 1)];
					graphNode.GetConnections(this.connectionBufferAddDelegate);
					global::UnityEngine.Vector3 result = vector;
					float num2 = float.PositiveInfinity;
					for (int i = this.connectionBuffer.Count - 1; i >= 0; i--)
					{
						global::Pathfinding.GraphNode graphNode3 = this.connectionBuffer[i];
						global::UnityEngine.Vector3 vector3 = global::Pathfinding.VectorMath.ClosestPointOnSegment(vector, (global::UnityEngine.Vector3)graphNode3.position, vector2);
						float sqrMagnitude = (vector3 - vector2).sqrMagnitude;
						if (sqrMagnitude < num2)
						{
							result = vector3;
							num2 = sqrMagnitude;
							forceAddPoint = (graphNode3 != graphNode2);
						}
					}
					this.connectionBuffer.Clear();
					return result;
				}
				}
				throw new global::System.ArgumentException("Cannot reach this point, but the compiler is not smart enough to realize that.");
			}
			case global::Pathfinding.StartEndModifier.Exactness.ClosestOnNode:
				return this.GetClampedPoint(vector, (!start) ? path.endPoint : path.startPoint, graphNode);
			default:
				throw new global::System.ArgumentException("Invalid mode");
			}
		}

		public global::UnityEngine.Vector3 GetClampedPoint(global::UnityEngine.Vector3 from, global::UnityEngine.Vector3 to, global::Pathfinding.GraphNode hint)
		{
			global::UnityEngine.Vector3 vector = to;
			global::UnityEngine.RaycastHit raycastHit;
			if (this.useRaycasting && global::UnityEngine.Physics.Linecast(from, to, out raycastHit, this.mask))
			{
				vector = raycastHit.point;
			}
			if (this.useGraphRaycasting && hint != null)
			{
				global::Pathfinding.IRaycastableGraph raycastableGraph = global::Pathfinding.AstarData.GetGraph(hint) as global::Pathfinding.IRaycastableGraph;
				global::Pathfinding.GraphHitInfo graphHitInfo;
				if (raycastableGraph != null && raycastableGraph.Linecast(from, vector, hint, out graphHitInfo))
				{
					vector = graphHitInfo.point;
				}
			}
			return vector;
		}

		public bool addPoints;

		public global::Pathfinding.StartEndModifier.Exactness exactStartPoint = global::Pathfinding.StartEndModifier.Exactness.ClosestOnNode;

		public global::Pathfinding.StartEndModifier.Exactness exactEndPoint = global::Pathfinding.StartEndModifier.Exactness.ClosestOnNode;

		public global::System.Func<global::UnityEngine.Vector3> adjustStartPoint;

		public bool useRaycasting;

		public global::UnityEngine.LayerMask mask = -1;

		public bool useGraphRaycasting;

		private global::System.Collections.Generic.List<global::Pathfinding.GraphNode> connectionBuffer;

		private global::Pathfinding.GraphNodeDelegate connectionBufferAddDelegate;

		public enum Exactness
		{
			SnapToNode,
			Original,
			Interpolate,
			ClosestOnNode,
			NodeConnection
		}
	}
}
