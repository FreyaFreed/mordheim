using System;
using System.Collections.Generic;
using Pathfinding.Serialization;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding
{
	public abstract class NavGraph
	{
		public virtual int CountNodes()
		{
			int count = 0;
			global::Pathfinding.GraphNodeDelegateCancelable del = delegate(global::Pathfinding.GraphNode node)
			{
				count++;
				return true;
			};
			this.GetNodes(del);
			return count;
		}

		public abstract void GetNodes(global::Pathfinding.GraphNodeDelegateCancelable del);

		public void SetMatrix(global::UnityEngine.Matrix4x4 m)
		{
			this.matrix = m;
			this.inverseMatrix = m.inverse;
		}

		public virtual void RelocateNodes(global::UnityEngine.Matrix4x4 oldMatrix, global::UnityEngine.Matrix4x4 newMatrix)
		{
			global::UnityEngine.Matrix4x4 inverse = oldMatrix.inverse;
			global::UnityEngine.Matrix4x4 m = newMatrix * inverse;
			this.GetNodes(delegate(global::Pathfinding.GraphNode node)
			{
				node.position = (global::Pathfinding.Int3)m.MultiplyPoint((global::UnityEngine.Vector3)node.position);
				return true;
			});
			this.SetMatrix(newMatrix);
		}

		public global::Pathfinding.NNInfoInternal GetNearest(global::UnityEngine.Vector3 position)
		{
			return this.GetNearest(position, global::Pathfinding.NNConstraint.None);
		}

		public global::Pathfinding.NNInfoInternal GetNearest(global::UnityEngine.Vector3 position, global::Pathfinding.NNConstraint constraint)
		{
			return this.GetNearest(position, constraint, null);
		}

		public virtual global::Pathfinding.NNInfoInternal GetNearest(global::UnityEngine.Vector3 position, global::Pathfinding.NNConstraint constraint, global::Pathfinding.GraphNode hint)
		{
			float maxDistSqr = (!constraint.constrainDistance) ? float.PositiveInfinity : global::AstarPath.active.maxNearestNodeDistanceSqr;
			float minDist = float.PositiveInfinity;
			global::Pathfinding.GraphNode minNode = null;
			float minConstDist = float.PositiveInfinity;
			global::Pathfinding.GraphNode minConstNode = null;
			this.GetNodes(delegate(global::Pathfinding.GraphNode node)
			{
				float sqrMagnitude = (position - (global::UnityEngine.Vector3)node.position).sqrMagnitude;
				if (sqrMagnitude < minDist)
				{
					minDist = sqrMagnitude;
					minNode = node;
				}
				if (sqrMagnitude < minConstDist && sqrMagnitude < maxDistSqr && constraint.Suitable(node))
				{
					minConstDist = sqrMagnitude;
					minConstNode = node;
				}
				return true;
			});
			global::Pathfinding.NNInfoInternal result = new global::Pathfinding.NNInfoInternal(minNode);
			result.constrainedNode = minConstNode;
			if (minConstNode != null)
			{
				result.constClampedPosition = (global::UnityEngine.Vector3)minConstNode.position;
			}
			else if (minNode != null)
			{
				result.constrainedNode = minNode;
				result.constClampedPosition = (global::UnityEngine.Vector3)minNode.position;
			}
			return result;
		}

		public virtual global::Pathfinding.NNInfoInternal GetNearestForce(global::UnityEngine.Vector3 position, global::Pathfinding.NNConstraint constraint)
		{
			return this.GetNearest(position, constraint);
		}

		public virtual void Awake()
		{
		}

		public virtual void OnDestroy()
		{
			this.GetNodes(delegate(global::Pathfinding.GraphNode node)
			{
				node.Destroy();
				return true;
			});
		}

		public void ScanGraph()
		{
			if (global::AstarPath.OnPreScan != null)
			{
				global::AstarPath.OnPreScan(global::AstarPath.active);
			}
			if (global::AstarPath.OnGraphPreScan != null)
			{
				global::AstarPath.OnGraphPreScan(this);
			}
			global::System.Collections.Generic.IEnumerator<global::Pathfinding.Progress> enumerator = this.ScanInternal().GetEnumerator();
			while (enumerator.MoveNext())
			{
			}
			if (global::AstarPath.OnGraphPostScan != null)
			{
				global::AstarPath.OnGraphPostScan(this);
			}
			if (global::AstarPath.OnPostScan != null)
			{
				global::AstarPath.OnPostScan(global::AstarPath.active);
			}
		}

		[global::System.Obsolete("Please use AstarPath.active.Scan or if you really want this.ScanInternal which has the same functionality as this method had")]
		public void Scan()
		{
			throw new global::System.Exception("This method is deprecated. Please use AstarPath.active.Scan or if you really want this.ScanInternal which has the same functionality as this method had.");
		}

		public abstract global::System.Collections.Generic.IEnumerable<global::Pathfinding.Progress> ScanInternal();

		public virtual global::UnityEngine.Color NodeColor(global::Pathfinding.GraphNode node, global::Pathfinding.PathHandler data)
		{
			global::Pathfinding.GraphDebugMode debugMode = global::AstarPath.active.debugMode;
			global::UnityEngine.Color result;
			switch (debugMode)
			{
			case global::Pathfinding.GraphDebugMode.Areas:
				result = global::Pathfinding.AstarColor.GetAreaColor(node.Area);
				goto IL_11F;
			case global::Pathfinding.GraphDebugMode.Penalty:
				result = global::UnityEngine.Color.Lerp(global::Pathfinding.AstarColor.ConnectionLowLerp, global::Pathfinding.AstarColor.ConnectionHighLerp, (node.Penalty - global::AstarPath.active.debugFloor) / (global::AstarPath.active.debugRoof - global::AstarPath.active.debugFloor));
				goto IL_11F;
			case global::Pathfinding.GraphDebugMode.Connections:
				result = global::Pathfinding.AstarColor.NodeConnection;
				goto IL_11F;
			case global::Pathfinding.GraphDebugMode.Tags:
				result = global::Pathfinding.AstarColor.GetAreaColor(node.Tag);
				goto IL_11F;
			}
			if (data == null)
			{
				return global::Pathfinding.AstarColor.NodeConnection;
			}
			global::Pathfinding.PathNode pathNode = data.GetPathNode(node);
			float num;
			if (debugMode == global::Pathfinding.GraphDebugMode.G)
			{
				num = pathNode.G;
			}
			else if (debugMode == global::Pathfinding.GraphDebugMode.H)
			{
				num = pathNode.H;
			}
			else
			{
				num = pathNode.F;
			}
			result = global::UnityEngine.Color.Lerp(global::Pathfinding.AstarColor.ConnectionLowLerp, global::Pathfinding.AstarColor.ConnectionHighLerp, (num - global::AstarPath.active.debugFloor) / (global::AstarPath.active.debugRoof - global::AstarPath.active.debugFloor));
			IL_11F:
			result.a *= 0.5f;
			return result;
		}

		public virtual void SerializeExtraInfo(global::Pathfinding.Serialization.GraphSerializationContext ctx)
		{
		}

		public virtual void DeserializeExtraInfo(global::Pathfinding.Serialization.GraphSerializationContext ctx)
		{
		}

		public virtual void PostDeserialization()
		{
		}

		public virtual void DeserializeSettingsCompatibility(global::Pathfinding.Serialization.GraphSerializationContext ctx)
		{
			this.guid = new global::Pathfinding.Util.Guid(ctx.reader.ReadBytes(16));
			this.initialPenalty = ctx.reader.ReadUInt32();
			this.open = ctx.reader.ReadBoolean();
			this.name = ctx.reader.ReadString();
			this.drawGizmos = ctx.reader.ReadBoolean();
			this.infoScreenOpen = ctx.reader.ReadBoolean();
			for (int i = 0; i < 4; i++)
			{
				global::UnityEngine.Vector4 zero = global::UnityEngine.Vector4.zero;
				for (int j = 0; j < 4; j++)
				{
					zero[j] = ctx.reader.ReadSingle();
				}
				this.matrix.SetRow(i, zero);
			}
		}

		public static bool InSearchTree(global::Pathfinding.GraphNode node, global::Pathfinding.Path path)
		{
			if (path == null || path.pathHandler == null)
			{
				return true;
			}
			global::Pathfinding.PathNode pathNode = path.pathHandler.GetPathNode(node);
			return pathNode.pathID == path.pathID;
		}

		public virtual void OnDrawGizmos(bool drawNodes)
		{
			if (!drawNodes)
			{
				return;
			}
			global::Pathfinding.PathHandler data = global::AstarPath.active.debugPathData;
			global::Pathfinding.GraphNode node = null;
			global::Pathfinding.GraphNodeDelegate drawConnection = delegate(global::Pathfinding.GraphNode otherNode)
			{
				global::UnityEngine.Gizmos.DrawLine((global::UnityEngine.Vector3)node.position, (global::UnityEngine.Vector3)otherNode.position);
			};
			this.GetNodes(delegate(global::Pathfinding.GraphNode _node)
			{
				node = _node;
				global::UnityEngine.Gizmos.color = this.NodeColor(node, global::AstarPath.active.debugPathData);
				if (global::AstarPath.active.showSearchTree && !global::Pathfinding.NavGraph.InSearchTree(node, global::AstarPath.active.debugPath))
				{
					return true;
				}
				global::Pathfinding.PathNode pathNode = (data == null) ? null : data.GetPathNode(node);
				if (global::AstarPath.active.showSearchTree && pathNode != null && pathNode.parent != null)
				{
					global::UnityEngine.Gizmos.DrawLine((global::UnityEngine.Vector3)node.position, (global::UnityEngine.Vector3)pathNode.parent.node.position);
				}
				else
				{
					node.GetConnections(drawConnection);
				}
				return true;
			});
		}

		internal virtual void UnloadGizmoMeshes()
		{
		}

		public global::AstarPath active;

		[global::Pathfinding.Serialization.JsonMember]
		public global::Pathfinding.Util.Guid guid;

		[global::Pathfinding.Serialization.JsonMember]
		public uint initialPenalty;

		[global::Pathfinding.Serialization.JsonMember]
		public bool open;

		public uint graphIndex;

		[global::Pathfinding.Serialization.JsonMember]
		public string name;

		[global::Pathfinding.Serialization.JsonMember]
		public bool drawGizmos = true;

		[global::Pathfinding.Serialization.JsonMember]
		public bool infoScreenOpen;

		public global::UnityEngine.Matrix4x4 matrix = global::UnityEngine.Matrix4x4.identity;

		public global::UnityEngine.Matrix4x4 inverseMatrix = global::UnityEngine.Matrix4x4.identity;
	}
}
