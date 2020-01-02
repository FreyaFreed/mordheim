using System;
using System.Collections.Generic;
using Pathfinding.Serialization;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding
{
	[global::Pathfinding.Serialization.JsonOptIn]
	public class PointGraph : global::Pathfinding.NavGraph, global::Pathfinding.IUpdatableGraph
	{
		public int nodeCount { get; private set; }

		public override int CountNodes()
		{
			return this.nodeCount;
		}

		public override void GetNodes(global::Pathfinding.GraphNodeDelegateCancelable del)
		{
			if (this.nodes == null)
			{
				return;
			}
			int nodeCount = this.nodeCount;
			int num = 0;
			while (num < nodeCount && del(this.nodes[num]))
			{
				num++;
			}
		}

		public override global::Pathfinding.NNInfoInternal GetNearest(global::UnityEngine.Vector3 position, global::Pathfinding.NNConstraint constraint, global::Pathfinding.GraphNode hint)
		{
			return this.GetNearestForce(position, null);
		}

		public override global::Pathfinding.NNInfoInternal GetNearestForce(global::UnityEngine.Vector3 position, global::Pathfinding.NNConstraint constraint)
		{
			if (this.nodes == null)
			{
				return default(global::Pathfinding.NNInfoInternal);
			}
			if (this.optimizeForSparseGraph)
			{
				return new global::Pathfinding.NNInfoInternal(this.lookupTree.GetNearest((global::Pathfinding.Int3)position, constraint));
			}
			float num = (constraint != null && !constraint.constrainDistance) ? float.PositiveInfinity : global::AstarPath.active.maxNearestNodeDistanceSqr;
			global::Pathfinding.NNInfoInternal result = new global::Pathfinding.NNInfoInternal(null);
			float num2 = float.PositiveInfinity;
			float num3 = float.PositiveInfinity;
			for (int i = 0; i < this.nodeCount; i++)
			{
				global::Pathfinding.PointNode pointNode = this.nodes[i];
				float sqrMagnitude = (position - (global::UnityEngine.Vector3)pointNode.position).sqrMagnitude;
				if (sqrMagnitude < num2)
				{
					num2 = sqrMagnitude;
					result.node = pointNode;
				}
				if (sqrMagnitude < num3 && sqrMagnitude < num && (constraint == null || constraint.Suitable(pointNode)))
				{
					num3 = sqrMagnitude;
					result.constrainedNode = pointNode;
				}
			}
			result.UpdateInfo();
			return result;
		}

		public global::Pathfinding.PointNode AddNode(global::Pathfinding.Int3 position)
		{
			return this.AddNode<global::Pathfinding.PointNode>(new global::Pathfinding.PointNode(this.active), position);
		}

		public T AddNode<T>(T node, global::Pathfinding.Int3 position) where T : global::Pathfinding.PointNode
		{
			if (this.nodes == null || this.nodeCount == this.nodes.Length)
			{
				global::Pathfinding.PointNode[] array = new global::Pathfinding.PointNode[(this.nodes == null) ? 4 : global::System.Math.Max(this.nodes.Length + 4, this.nodes.Length * 2)];
				for (int i = 0; i < this.nodeCount; i++)
				{
					array[i] = this.nodes[i];
				}
				this.nodes = array;
			}
			node.SetPosition(position);
			node.GraphIndex = this.graphIndex;
			node.Walkable = true;
			this.nodes[this.nodeCount] = node;
			this.nodeCount++;
			this.AddToLookup(node);
			return node;
		}

		protected static int CountChildren(global::UnityEngine.Transform tr)
		{
			int num = 0;
			foreach (object obj in tr)
			{
				global::UnityEngine.Transform tr2 = (global::UnityEngine.Transform)obj;
				num++;
				num += global::Pathfinding.PointGraph.CountChildren(tr2);
			}
			return num;
		}

		protected void AddChildren(ref int c, global::UnityEngine.Transform tr)
		{
			foreach (object obj in tr)
			{
				global::UnityEngine.Transform transform = (global::UnityEngine.Transform)obj;
				this.nodes[c].SetPosition((global::Pathfinding.Int3)transform.position);
				this.nodes[c].Walkable = true;
				this.nodes[c].gameObject = transform.gameObject;
				c++;
				this.AddChildren(ref c, transform);
			}
		}

		public void RebuildNodeLookup()
		{
			if (!this.optimizeForSparseGraph || this.nodes == null)
			{
				this.lookupTree = new global::Pathfinding.PointKDTree();
			}
			else
			{
				this.lookupTree.Rebuild(this.nodes, 0, this.nodeCount);
			}
		}

		private void AddToLookup(global::Pathfinding.PointNode node)
		{
			this.lookupTree.Add(node);
		}

		public override global::System.Collections.Generic.IEnumerable<global::Pathfinding.Progress> ScanInternal()
		{
			yield return new global::Pathfinding.Progress(0f, "Searching for GameObjects");
			if (this.root == null)
			{
				global::UnityEngine.GameObject[] gos = (this.searchTag == null) ? null : global::UnityEngine.GameObject.FindGameObjectsWithTag(this.searchTag);
				if (gos == null)
				{
					this.nodes = new global::Pathfinding.PointNode[0];
					this.nodeCount = 0;
					yield break;
				}
				yield return new global::Pathfinding.Progress(0.1f, "Creating nodes");
				this.nodes = new global::Pathfinding.PointNode[gos.Length];
				this.nodeCount = this.nodes.Length;
				for (int i = 0; i < this.nodes.Length; i++)
				{
					this.nodes[i] = new global::Pathfinding.PointNode(this.active);
				}
				for (int j = 0; j < gos.Length; j++)
				{
					this.nodes[j].SetPosition((global::Pathfinding.Int3)gos[j].transform.position);
					this.nodes[j].Walkable = true;
					this.nodes[j].gameObject = gos[j].gameObject;
				}
			}
			else if (!this.recursive)
			{
				this.nodes = new global::Pathfinding.PointNode[this.root.childCount];
				this.nodeCount = this.nodes.Length;
				for (int k = 0; k < this.nodes.Length; k++)
				{
					this.nodes[k] = new global::Pathfinding.PointNode(this.active);
				}
				int c = 0;
				foreach (object obj in this.root)
				{
					global::UnityEngine.Transform child = (global::UnityEngine.Transform)obj;
					this.nodes[c].SetPosition((global::Pathfinding.Int3)child.position);
					this.nodes[c].Walkable = true;
					this.nodes[c].gameObject = child.gameObject;
					c++;
				}
			}
			else
			{
				this.nodes = new global::Pathfinding.PointNode[global::Pathfinding.PointGraph.CountChildren(this.root)];
				this.nodeCount = this.nodes.Length;
				for (int l = 0; l < this.nodes.Length; l++)
				{
					this.nodes[l] = new global::Pathfinding.PointNode(this.active);
				}
				int startID = 0;
				this.AddChildren(ref startID, this.root);
			}
			if (this.optimizeForSparseGraph)
			{
				yield return new global::Pathfinding.Progress(0.15f, "Building node lookup");
				this.RebuildNodeLookup();
			}
			if (this.maxDistance >= 0f)
			{
				global::System.Collections.Generic.List<global::Pathfinding.PointNode> connections = new global::System.Collections.Generic.List<global::Pathfinding.PointNode>();
				global::System.Collections.Generic.List<uint> costs = new global::System.Collections.Generic.List<uint>();
				global::System.Collections.Generic.List<global::Pathfinding.GraphNode> candidateConnections = new global::System.Collections.Generic.List<global::Pathfinding.GraphNode>();
				long maxPossibleSqrRange;
				if (this.maxDistance == 0f && (this.limits.x == 0f || this.limits.y == 0f || this.limits.z == 0f))
				{
					maxPossibleSqrRange = long.MaxValue;
				}
				else
				{
					maxPossibleSqrRange = (long)(global::UnityEngine.Mathf.Max(this.limits.x, global::UnityEngine.Mathf.Max(this.limits.y, global::UnityEngine.Mathf.Max(this.limits.z, this.maxDistance))) * 1000f) + 1L;
					maxPossibleSqrRange *= maxPossibleSqrRange;
				}
				for (int m = 0; m < this.nodes.Length; m++)
				{
					if (m % 512 == 0)
					{
						yield return new global::Pathfinding.Progress(global::UnityEngine.Mathf.Lerp(0.15f, 1f, (float)m / (float)this.nodes.Length), "Connecting nodes");
					}
					connections.Clear();
					costs.Clear();
					global::Pathfinding.PointNode node = this.nodes[m];
					if (this.optimizeForSparseGraph)
					{
						candidateConnections.Clear();
						this.lookupTree.GetInRange(node.position, maxPossibleSqrRange, candidateConnections);
						global::System.Console.WriteLine(m + " " + candidateConnections.Count);
						for (int n = 0; n < candidateConnections.Count; n++)
						{
							global::Pathfinding.PointNode other = candidateConnections[n] as global::Pathfinding.PointNode;
							float dist;
							if (other != node && this.IsValidConnection(node, other, out dist))
							{
								connections.Add(other);
								costs.Add((uint)global::UnityEngine.Mathf.RoundToInt(dist * 1000f));
							}
						}
					}
					else
					{
						for (int j2 = 0; j2 < this.nodes.Length; j2++)
						{
							if (m != j2)
							{
								global::Pathfinding.PointNode other2 = this.nodes[j2];
								float dist2;
								if (this.IsValidConnection(node, other2, out dist2))
								{
									connections.Add(other2);
									costs.Add((uint)global::UnityEngine.Mathf.RoundToInt(dist2 * 1000f));
								}
							}
						}
					}
					node.connections = connections.ToArray();
					node.connectionCosts = costs.ToArray();
				}
			}
			yield break;
		}

		public virtual bool IsValidConnection(global::Pathfinding.GraphNode a, global::Pathfinding.GraphNode b, out float dist)
		{
			dist = 0f;
			if (!a.Walkable || !b.Walkable)
			{
				return false;
			}
			global::UnityEngine.Vector3 vector = (global::UnityEngine.Vector3)(b.position - a.position);
			if ((!global::UnityEngine.Mathf.Approximately(this.limits.x, 0f) && global::UnityEngine.Mathf.Abs(vector.x) > this.limits.x) || (!global::UnityEngine.Mathf.Approximately(this.limits.y, 0f) && global::UnityEngine.Mathf.Abs(vector.y) > this.limits.y) || (!global::UnityEngine.Mathf.Approximately(this.limits.z, 0f) && global::UnityEngine.Mathf.Abs(vector.z) > this.limits.z))
			{
				return false;
			}
			dist = vector.magnitude;
			if (this.maxDistance != 0f && dist >= this.maxDistance)
			{
				return false;
			}
			if (!this.raycast)
			{
				return true;
			}
			global::UnityEngine.Ray ray = new global::UnityEngine.Ray((global::UnityEngine.Vector3)a.position, vector);
			global::UnityEngine.Ray ray2 = new global::UnityEngine.Ray((global::UnityEngine.Vector3)b.position, -vector);
			if (this.use2DPhysics)
			{
				if (this.thickRaycast)
				{
					return !global::UnityEngine.Physics2D.CircleCast(ray.origin, this.thickRaycastRadius, ray.direction, dist, this.mask) && !global::UnityEngine.Physics2D.CircleCast(ray2.origin, this.thickRaycastRadius, ray2.direction, dist, this.mask);
				}
				return !global::UnityEngine.Physics2D.Linecast((global::UnityEngine.Vector3)a.position, (global::UnityEngine.Vector3)b.position, this.mask) && !global::UnityEngine.Physics2D.Linecast((global::UnityEngine.Vector3)b.position, (global::UnityEngine.Vector3)a.position, this.mask);
			}
			else
			{
				if (this.thickRaycast)
				{
					return !global::UnityEngine.Physics.SphereCast(ray, this.thickRaycastRadius, dist, this.mask) && !global::UnityEngine.Physics.SphereCast(ray2, this.thickRaycastRadius, dist, this.mask);
				}
				return !global::UnityEngine.Physics.Linecast((global::UnityEngine.Vector3)a.position, (global::UnityEngine.Vector3)b.position, this.mask) && !global::UnityEngine.Physics.Linecast((global::UnityEngine.Vector3)b.position, (global::UnityEngine.Vector3)a.position, this.mask);
			}
		}

		public global::Pathfinding.GraphUpdateThreading CanUpdateAsync(global::Pathfinding.GraphUpdateObject o)
		{
			return global::Pathfinding.GraphUpdateThreading.UnityThread;
		}

		public void UpdateAreaInit(global::Pathfinding.GraphUpdateObject o)
		{
		}

		public void UpdateAreaPost(global::Pathfinding.GraphUpdateObject o)
		{
		}

		public void UpdateArea(global::Pathfinding.GraphUpdateObject guo)
		{
			if (this.nodes == null)
			{
				return;
			}
			for (int i = 0; i < this.nodeCount; i++)
			{
				if (guo.bounds.Contains((global::UnityEngine.Vector3)this.nodes[i].position))
				{
					guo.WillUpdateNode(this.nodes[i]);
					guo.Apply(this.nodes[i]);
				}
			}
			if (guo.updatePhysics)
			{
				global::UnityEngine.Bounds bounds = guo.bounds;
				if (this.thickRaycast)
				{
					bounds.Expand(this.thickRaycastRadius * 2f);
				}
				global::System.Collections.Generic.List<global::Pathfinding.GraphNode> list = global::Pathfinding.Util.ListPool<global::Pathfinding.GraphNode>.Claim();
				global::System.Collections.Generic.List<uint> list2 = global::Pathfinding.Util.ListPool<uint>.Claim();
				for (int j = 0; j < this.nodeCount; j++)
				{
					global::Pathfinding.PointNode pointNode = this.nodes[j];
					global::UnityEngine.Vector3 a = (global::UnityEngine.Vector3)pointNode.position;
					global::System.Collections.Generic.List<global::Pathfinding.GraphNode> list3 = null;
					global::System.Collections.Generic.List<uint> list4 = null;
					for (int k = 0; k < this.nodeCount; k++)
					{
						if (k != j)
						{
							global::UnityEngine.Vector3 b = (global::UnityEngine.Vector3)this.nodes[k].position;
							if (global::Pathfinding.VectorMath.SegmentIntersectsBounds(bounds, a, b))
							{
								global::Pathfinding.PointNode pointNode2 = this.nodes[k];
								bool flag = pointNode.ContainsConnection(pointNode2);
								float num;
								bool flag2 = this.IsValidConnection(pointNode, pointNode2, out num);
								if (!flag && flag2)
								{
									if (list3 == null)
									{
										list.Clear();
										list2.Clear();
										list3 = list;
										list4 = list2;
										list3.AddRange(pointNode.connections);
										list4.AddRange(pointNode.connectionCosts);
									}
									uint item = (uint)global::UnityEngine.Mathf.RoundToInt(num * 1000f);
									list3.Add(pointNode2);
									list4.Add(item);
								}
								else if (flag && !flag2)
								{
									if (list3 == null)
									{
										list.Clear();
										list2.Clear();
										list3 = list;
										list4 = list2;
										list3.AddRange(pointNode.connections);
										list4.AddRange(pointNode.connectionCosts);
									}
									int num2 = list3.IndexOf(pointNode2);
									if (num2 != -1)
									{
										list3.RemoveAt(num2);
										list4.RemoveAt(num2);
									}
								}
							}
						}
					}
					if (list3 != null)
					{
						pointNode.connections = list3.ToArray();
						pointNode.connectionCosts = list4.ToArray();
					}
				}
				global::Pathfinding.Util.ListPool<global::Pathfinding.GraphNode>.Release(list);
				global::Pathfinding.Util.ListPool<uint>.Release(list2);
			}
		}

		public override void PostDeserialization()
		{
			this.RebuildNodeLookup();
		}

		public override void RelocateNodes(global::UnityEngine.Matrix4x4 oldMatrix, global::UnityEngine.Matrix4x4 newMatrix)
		{
			base.RelocateNodes(oldMatrix, newMatrix);
			this.RebuildNodeLookup();
		}

		public override void DeserializeSettingsCompatibility(global::Pathfinding.Serialization.GraphSerializationContext ctx)
		{
			base.DeserializeSettingsCompatibility(ctx);
			this.root = (ctx.DeserializeUnityObject() as global::UnityEngine.Transform);
			this.searchTag = ctx.reader.ReadString();
			this.maxDistance = ctx.reader.ReadSingle();
			this.limits = ctx.DeserializeVector3();
			this.raycast = ctx.reader.ReadBoolean();
			this.use2DPhysics = ctx.reader.ReadBoolean();
			this.thickRaycast = ctx.reader.ReadBoolean();
			this.thickRaycastRadius = ctx.reader.ReadSingle();
			this.recursive = ctx.reader.ReadBoolean();
			ctx.reader.ReadBoolean();
			this.mask = ctx.reader.ReadInt32();
			this.optimizeForSparseGraph = ctx.reader.ReadBoolean();
			ctx.reader.ReadBoolean();
		}

		public override void SerializeExtraInfo(global::Pathfinding.Serialization.GraphSerializationContext ctx)
		{
			if (this.nodes == null)
			{
				ctx.writer.Write(-1);
			}
			ctx.writer.Write(this.nodeCount);
			for (int i = 0; i < this.nodeCount; i++)
			{
				if (this.nodes[i] == null)
				{
					ctx.writer.Write(-1);
				}
				else
				{
					ctx.writer.Write(0);
					this.nodes[i].SerializeNode(ctx);
				}
			}
		}

		public override void DeserializeExtraInfo(global::Pathfinding.Serialization.GraphSerializationContext ctx)
		{
			int num = ctx.reader.ReadInt32();
			if (num == -1)
			{
				this.nodes = null;
				return;
			}
			this.nodes = new global::Pathfinding.PointNode[num];
			this.nodeCount = num;
			for (int i = 0; i < this.nodes.Length; i++)
			{
				if (ctx.reader.ReadInt32() != -1)
				{
					this.nodes[i] = new global::Pathfinding.PointNode(this.active);
					this.nodes[i].DeserializeNode(ctx);
				}
			}
		}

		[global::Pathfinding.Serialization.JsonMember]
		public global::UnityEngine.Transform root;

		[global::Pathfinding.Serialization.JsonMember]
		public string searchTag;

		[global::Pathfinding.Serialization.JsonMember]
		public float maxDistance;

		[global::Pathfinding.Serialization.JsonMember]
		public global::UnityEngine.Vector3 limits;

		[global::Pathfinding.Serialization.JsonMember]
		public bool raycast = true;

		[global::Pathfinding.Serialization.JsonMember]
		public bool use2DPhysics;

		[global::Pathfinding.Serialization.JsonMember]
		public bool thickRaycast;

		[global::Pathfinding.Serialization.JsonMember]
		public float thickRaycastRadius = 1f;

		[global::Pathfinding.Serialization.JsonMember]
		public bool recursive = true;

		[global::Pathfinding.Serialization.JsonMember]
		public global::UnityEngine.LayerMask mask;

		[global::Pathfinding.Serialization.JsonMember]
		public bool optimizeForSparseGraph;

		private global::Pathfinding.PointKDTree lookupTree = new global::Pathfinding.PointKDTree();

		public global::Pathfinding.PointNode[] nodes;
	}
}
