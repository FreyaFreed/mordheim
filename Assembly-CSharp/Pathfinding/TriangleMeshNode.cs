using System;
using System.Collections.Generic;
using Pathfinding.Serialization;
using UnityEngine;

namespace Pathfinding
{
	public class TriangleMeshNode : global::Pathfinding.MeshNode
	{
		public TriangleMeshNode(global::AstarPath astar) : base(astar)
		{
		}

		public static global::Pathfinding.INavmeshHolder GetNavmeshHolder(uint graphIndex)
		{
			return global::Pathfinding.TriangleMeshNode._navmeshHolders[(int)graphIndex];
		}

		public static void SetNavmeshHolder(int graphIndex, global::Pathfinding.INavmeshHolder graph)
		{
			if (global::Pathfinding.TriangleMeshNode._navmeshHolders.Length <= graphIndex)
			{
				object obj = global::Pathfinding.TriangleMeshNode.lockObject;
				lock (obj)
				{
					if (global::Pathfinding.TriangleMeshNode._navmeshHolders.Length <= graphIndex)
					{
						global::Pathfinding.INavmeshHolder[] array = new global::Pathfinding.INavmeshHolder[graphIndex + 1];
						for (int i = 0; i < global::Pathfinding.TriangleMeshNode._navmeshHolders.Length; i++)
						{
							array[i] = global::Pathfinding.TriangleMeshNode._navmeshHolders[i];
						}
						global::Pathfinding.TriangleMeshNode._navmeshHolders = array;
					}
					global::Pathfinding.TriangleMeshNode._navmeshHolders[graphIndex] = graph;
				}
			}
			else
			{
				object obj2 = global::Pathfinding.TriangleMeshNode.lockObject;
				lock (obj2)
				{
					global::Pathfinding.TriangleMeshNode._navmeshHolders[graphIndex] = graph;
				}
			}
		}

		public void UpdatePositionFromVertices()
		{
			global::Pathfinding.INavmeshHolder navmeshHolder = global::Pathfinding.TriangleMeshNode.GetNavmeshHolder(base.GraphIndex);
			this.position = (navmeshHolder.GetVertex(this.v0) + navmeshHolder.GetVertex(this.v1) + navmeshHolder.GetVertex(this.v2)) * 0.333333f;
		}

		public int GetVertexIndex(int i)
		{
			return (i != 0) ? ((i != 1) ? this.v2 : this.v1) : this.v0;
		}

		public int GetVertexArrayIndex(int i)
		{
			return global::Pathfinding.TriangleMeshNode.GetNavmeshHolder(base.GraphIndex).GetVertexArrayIndex((i != 0) ? ((i != 1) ? this.v2 : this.v1) : this.v0);
		}

		public override global::Pathfinding.Int3 GetVertex(int i)
		{
			return global::Pathfinding.TriangleMeshNode.GetNavmeshHolder(base.GraphIndex).GetVertex(this.GetVertexIndex(i));
		}

		public override int GetVertexCount()
		{
			return 3;
		}

		public override global::UnityEngine.Vector3 ClosestPointOnNode(global::UnityEngine.Vector3 p)
		{
			global::Pathfinding.INavmeshHolder navmeshHolder = global::Pathfinding.TriangleMeshNode.GetNavmeshHolder(base.GraphIndex);
			return global::Pathfinding.Polygon.ClosestPointOnTriangle((global::UnityEngine.Vector3)navmeshHolder.GetVertex(this.v0), (global::UnityEngine.Vector3)navmeshHolder.GetVertex(this.v1), (global::UnityEngine.Vector3)navmeshHolder.GetVertex(this.v2), p);
		}

		public override global::UnityEngine.Vector3 ClosestPointOnNodeXZ(global::UnityEngine.Vector3 p)
		{
			global::Pathfinding.INavmeshHolder navmeshHolder = global::Pathfinding.TriangleMeshNode.GetNavmeshHolder(base.GraphIndex);
			global::Pathfinding.Int3 vertex = navmeshHolder.GetVertex(this.v0);
			global::Pathfinding.Int3 vertex2 = navmeshHolder.GetVertex(this.v1);
			global::Pathfinding.Int3 vertex3 = navmeshHolder.GetVertex(this.v2);
			global::UnityEngine.Vector2 vector = global::Pathfinding.Polygon.ClosestPointOnTriangle(new global::UnityEngine.Vector2((float)vertex.x * 0.001f, (float)vertex.z * 0.001f), new global::UnityEngine.Vector2((float)vertex2.x * 0.001f, (float)vertex2.z * 0.001f), new global::UnityEngine.Vector2((float)vertex3.x * 0.001f, (float)vertex3.z * 0.001f), new global::UnityEngine.Vector2(p.x, p.z));
			return new global::UnityEngine.Vector3(vector.x, p.y, vector.y);
		}

		public override bool ContainsPoint(global::Pathfinding.Int3 p)
		{
			global::Pathfinding.INavmeshHolder navmeshHolder = global::Pathfinding.TriangleMeshNode.GetNavmeshHolder(base.GraphIndex);
			global::Pathfinding.Int3 vertex = navmeshHolder.GetVertex(this.v0);
			global::Pathfinding.Int3 vertex2 = navmeshHolder.GetVertex(this.v1);
			global::Pathfinding.Int3 vertex3 = navmeshHolder.GetVertex(this.v2);
			return (long)(vertex2.x - vertex.x) * (long)(p.z - vertex.z) - (long)(p.x - vertex.x) * (long)(vertex2.z - vertex.z) <= 0L && (long)(vertex3.x - vertex2.x) * (long)(p.z - vertex2.z) - (long)(p.x - vertex2.x) * (long)(vertex3.z - vertex2.z) <= 0L && (long)(vertex.x - vertex3.x) * (long)(p.z - vertex3.z) - (long)(p.x - vertex3.x) * (long)(vertex.z - vertex3.z) <= 0L && global::UnityEngine.Mathf.Abs(p.y - this.position.y) <= 2000;
		}

		public override void UpdateRecursiveG(global::Pathfinding.Path path, global::Pathfinding.PathNode pathNode, global::Pathfinding.PathHandler handler)
		{
			base.UpdateG(path, pathNode);
			handler.heap.Add(pathNode);
			if (this.connections == null)
			{
				return;
			}
			for (int i = 0; i < this.connections.Length; i++)
			{
				global::Pathfinding.GraphNode graphNode = this.connections[i];
				global::Pathfinding.PathNode pathNode2 = handler.GetPathNode(graphNode);
				if (pathNode2.parent == pathNode && pathNode2.pathID == handler.PathID)
				{
					graphNode.UpdateRecursiveG(path, pathNode2, handler);
				}
			}
		}

		public override void Open(global::Pathfinding.Path path, global::Pathfinding.PathNode pathNode, global::Pathfinding.PathHandler handler)
		{
			if (this.connections == null)
			{
				return;
			}
			bool flag = pathNode.flag2;
			for (int i = this.connections.Length - 1; i >= 0; i--)
			{
				global::Pathfinding.GraphNode graphNode = this.connections[i];
				if (path.CanTraverse(graphNode))
				{
					global::Pathfinding.PathNode pathNode2 = handler.GetPathNode(graphNode);
					if (pathNode2 != pathNode.parent)
					{
						uint num = this.connectionCosts[i];
						if (flag || pathNode2.flag2)
						{
							num = path.GetConnectionSpecialCost(this, graphNode, num);
						}
						if (pathNode2.pathID != handler.PathID)
						{
							pathNode2.node = graphNode;
							pathNode2.parent = pathNode;
							pathNode2.pathID = handler.PathID;
							pathNode2.cost = num;
							pathNode2.H = path.CalculateHScore(graphNode);
							graphNode.UpdateG(path, pathNode2);
							handler.heap.Add(pathNode2);
						}
						else if (pathNode.G + num + path.GetTraversalCost(graphNode) < pathNode2.G)
						{
							pathNode2.cost = num;
							pathNode2.parent = pathNode;
							graphNode.UpdateRecursiveG(path, pathNode2, handler);
						}
						else if (pathNode2.G + num + path.GetTraversalCost(this) < pathNode.G && graphNode.ContainsConnection(this))
						{
							pathNode.parent = pathNode2;
							pathNode.cost = num;
							this.UpdateRecursiveG(path, pathNode, handler);
						}
					}
				}
			}
		}

		public int SharedEdge(global::Pathfinding.GraphNode other)
		{
			int result;
			int num;
			this.GetPortal(other, null, null, false, out result, out num);
			return result;
		}

		public override bool GetPortal(global::Pathfinding.GraphNode _other, global::System.Collections.Generic.List<global::UnityEngine.Vector3> left, global::System.Collections.Generic.List<global::UnityEngine.Vector3> right, bool backwards)
		{
			int num;
			int num2;
			return this.GetPortal(_other, left, right, backwards, out num, out num2);
		}

		public bool GetPortal(global::Pathfinding.GraphNode _other, global::System.Collections.Generic.List<global::UnityEngine.Vector3> left, global::System.Collections.Generic.List<global::UnityEngine.Vector3> right, bool backwards, out int aIndex, out int bIndex)
		{
			aIndex = -1;
			bIndex = -1;
			if (_other.GraphIndex != base.GraphIndex)
			{
				return false;
			}
			global::Pathfinding.TriangleMeshNode triangleMeshNode = _other as global::Pathfinding.TriangleMeshNode;
			int num = this.GetVertexIndex(0) >> 12 & 524287;
			int num2 = triangleMeshNode.GetVertexIndex(0) >> 12 & 524287;
			if (num != num2 && global::Pathfinding.TriangleMeshNode.GetNavmeshHolder(base.GraphIndex) is global::Pathfinding.RecastGraph)
			{
				for (int i = 0; i < this.connections.Length; i++)
				{
					if (this.connections[i].GraphIndex != base.GraphIndex)
					{
						global::Pathfinding.NodeLink3Node nodeLink3Node = this.connections[i] as global::Pathfinding.NodeLink3Node;
						if (nodeLink3Node != null && nodeLink3Node.GetOther(this) == triangleMeshNode && left != null)
						{
							nodeLink3Node.GetPortal(triangleMeshNode, left, right, false);
							return true;
						}
					}
				}
				global::Pathfinding.INavmeshHolder navmeshHolder = global::Pathfinding.TriangleMeshNode.GetNavmeshHolder(base.GraphIndex);
				int num3;
				int num4;
				navmeshHolder.GetTileCoordinates(num, out num3, out num4);
				int num5;
				int num6;
				navmeshHolder.GetTileCoordinates(num2, out num5, out num6);
				int num7;
				if (global::System.Math.Abs(num3 - num5) == 1)
				{
					num7 = 0;
				}
				else
				{
					if (global::System.Math.Abs(num4 - num6) != 1)
					{
						throw new global::System.Exception(string.Concat(new object[]
						{
							"Tiles not adjacent (",
							num3,
							", ",
							num4,
							") (",
							num5,
							", ",
							num6,
							")"
						}));
					}
					num7 = 2;
				}
				int vertexCount = this.GetVertexCount();
				int vertexCount2 = triangleMeshNode.GetVertexCount();
				int num8 = -1;
				int num9 = -1;
				for (int j = 0; j < vertexCount; j++)
				{
					int num10 = this.GetVertex(j)[num7];
					for (int k = 0; k < vertexCount2; k++)
					{
						if (num10 == triangleMeshNode.GetVertex((k + 1) % vertexCount2)[num7] && this.GetVertex((j + 1) % vertexCount)[num7] == triangleMeshNode.GetVertex(k)[num7])
						{
							num8 = j;
							num9 = k;
							j = vertexCount;
							break;
						}
					}
				}
				aIndex = num8;
				bIndex = num9;
				if (num8 != -1)
				{
					global::Pathfinding.Int3 vertex = this.GetVertex(num8);
					global::Pathfinding.Int3 vertex2 = this.GetVertex((num8 + 1) % vertexCount);
					int i2 = (num7 != 2) ? 2 : 0;
					int num11 = global::System.Math.Min(vertex[i2], vertex2[i2]);
					int num12 = global::System.Math.Max(vertex[i2], vertex2[i2]);
					num11 = global::System.Math.Max(num11, global::System.Math.Min(triangleMeshNode.GetVertex(num9)[i2], triangleMeshNode.GetVertex((num9 + 1) % vertexCount2)[i2]));
					num12 = global::System.Math.Min(num12, global::System.Math.Max(triangleMeshNode.GetVertex(num9)[i2], triangleMeshNode.GetVertex((num9 + 1) % vertexCount2)[i2]));
					if (vertex[i2] < vertex2[i2])
					{
						vertex[i2] = num11;
						vertex2[i2] = num12;
					}
					else
					{
						vertex[i2] = num12;
						vertex2[i2] = num11;
					}
					if (left != null)
					{
						left.Add((global::UnityEngine.Vector3)vertex);
						right.Add((global::UnityEngine.Vector3)vertex2);
					}
					return true;
				}
			}
			else if (!backwards)
			{
				int num13 = -1;
				int num14 = -1;
				int vertexCount3 = this.GetVertexCount();
				int vertexCount4 = triangleMeshNode.GetVertexCount();
				for (int l = 0; l < vertexCount3; l++)
				{
					int vertexIndex = this.GetVertexIndex(l);
					for (int m = 0; m < vertexCount4; m++)
					{
						if (vertexIndex == triangleMeshNode.GetVertexIndex((m + 1) % vertexCount4) && this.GetVertexIndex((l + 1) % vertexCount3) == triangleMeshNode.GetVertexIndex(m))
						{
							num13 = l;
							num14 = m;
							l = vertexCount3;
							break;
						}
					}
				}
				aIndex = num13;
				bIndex = num14;
				if (num13 == -1)
				{
					for (int n = 0; n < this.connections.Length; n++)
					{
						if (this.connections[n].GraphIndex != base.GraphIndex)
						{
							global::Pathfinding.NodeLink3Node nodeLink3Node2 = this.connections[n] as global::Pathfinding.NodeLink3Node;
							if (nodeLink3Node2 != null && nodeLink3Node2.GetOther(this) == triangleMeshNode && left != null)
							{
								nodeLink3Node2.GetPortal(triangleMeshNode, left, right, false);
								return true;
							}
						}
					}
					return false;
				}
				if (left != null)
				{
					left.Add((global::UnityEngine.Vector3)this.GetVertex(num13));
					right.Add((global::UnityEngine.Vector3)this.GetVertex((num13 + 1) % vertexCount3));
				}
			}
			return true;
		}

		public override float SurfaceArea()
		{
			global::Pathfinding.INavmeshHolder navmeshHolder = global::Pathfinding.TriangleMeshNode.GetNavmeshHolder(base.GraphIndex);
			return (float)global::System.Math.Abs(global::Pathfinding.VectorMath.SignedTriangleAreaTimes2XZ(navmeshHolder.GetVertex(this.v0), navmeshHolder.GetVertex(this.v1), navmeshHolder.GetVertex(this.v2))) * 0.5f;
		}

		public override global::UnityEngine.Vector3 RandomPointOnSurface()
		{
			float value;
			float value2;
			do
			{
				value = global::UnityEngine.Random.value;
				value2 = global::UnityEngine.Random.value;
			}
			while (value + value2 > 1f);
			global::Pathfinding.INavmeshHolder navmeshHolder = global::Pathfinding.TriangleMeshNode.GetNavmeshHolder(base.GraphIndex);
			return (global::UnityEngine.Vector3)(navmeshHolder.GetVertex(this.v1) - navmeshHolder.GetVertex(this.v0)) * value + (global::UnityEngine.Vector3)(navmeshHolder.GetVertex(this.v2) - navmeshHolder.GetVertex(this.v0)) * value2 + (global::UnityEngine.Vector3)navmeshHolder.GetVertex(this.v0);
		}

		public override void SerializeNode(global::Pathfinding.Serialization.GraphSerializationContext ctx)
		{
			base.SerializeNode(ctx);
			ctx.writer.Write(this.v0);
			ctx.writer.Write(this.v1);
			ctx.writer.Write(this.v2);
		}

		public override void DeserializeNode(global::Pathfinding.Serialization.GraphSerializationContext ctx)
		{
			base.DeserializeNode(ctx);
			this.v0 = ctx.reader.ReadInt32();
			this.v1 = ctx.reader.ReadInt32();
			this.v2 = ctx.reader.ReadInt32();
		}

		public int v0;

		public int v1;

		public int v2;

		protected static global::Pathfinding.INavmeshHolder[] _navmeshHolders = new global::Pathfinding.INavmeshHolder[0];

		protected static readonly object lockObject = new object();
	}
}
