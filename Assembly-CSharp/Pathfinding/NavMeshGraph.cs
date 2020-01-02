using System;
using System.Collections.Generic;
using Pathfinding.Serialization;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding
{
	[global::Pathfinding.Serialization.JsonOptIn]
	[global::System.Serializable]
	public class NavMeshGraph : global::Pathfinding.NavGraph, global::Pathfinding.IUpdatableGraph, global::Pathfinding.IRaycastableGraph, global::Pathfinding.INavmesh, global::Pathfinding.INavmeshHolder
	{
		public global::Pathfinding.TriangleMeshNode[] TriNodes
		{
			get
			{
				return this.nodes;
			}
		}

		public override void GetNodes(global::Pathfinding.GraphNodeDelegateCancelable del)
		{
			if (this.nodes == null)
			{
				return;
			}
			int num = 0;
			while (num < this.nodes.Length && del(this.nodes[num]))
			{
				num++;
			}
		}

		public override void OnDestroy()
		{
			base.OnDestroy();
			global::Pathfinding.TriangleMeshNode.SetNavmeshHolder(this.active.astarData.GetGraphIndex(this), null);
		}

		public global::Pathfinding.Int3 GetVertex(int index)
		{
			return this.vertices[index];
		}

		public int GetVertexArrayIndex(int index)
		{
			return index;
		}

		public void GetTileCoordinates(int tileIndex, out int x, out int z)
		{
			x = (z = 0);
		}

		public global::Pathfinding.BBTree bbTree
		{
			get
			{
				return this._bbTree;
			}
			set
			{
				this._bbTree = value;
			}
		}

		public global::Pathfinding.Int3[] vertices
		{
			get
			{
				return this._vertices;
			}
			set
			{
				this._vertices = value;
			}
		}

		public void GenerateMatrix()
		{
			base.SetMatrix(global::UnityEngine.Matrix4x4.TRS(this.offset, global::UnityEngine.Quaternion.Euler(this.rotation), new global::UnityEngine.Vector3(this.scale, this.scale, this.scale)));
		}

		public override void RelocateNodes(global::UnityEngine.Matrix4x4 oldMatrix, global::UnityEngine.Matrix4x4 newMatrix)
		{
			if (this.vertices == null || this.vertices.Length == 0 || this.originalVertices == null || this.originalVertices.Length != this.vertices.Length)
			{
				return;
			}
			for (int i = 0; i < this._vertices.Length; i++)
			{
				this._vertices[i] = (global::Pathfinding.Int3)newMatrix.MultiplyPoint3x4(this.originalVertices[i]);
			}
			for (int j = 0; j < this.nodes.Length; j++)
			{
				global::Pathfinding.TriangleMeshNode triangleMeshNode = this.nodes[j];
				triangleMeshNode.UpdatePositionFromVertices();
				if (triangleMeshNode.connections != null)
				{
					for (int k = 0; k < triangleMeshNode.connections.Length; k++)
					{
						triangleMeshNode.connectionCosts[k] = (uint)(triangleMeshNode.position - triangleMeshNode.connections[k].position).costMagnitude;
					}
				}
			}
			base.SetMatrix(newMatrix);
			global::Pathfinding.NavMeshGraph.RebuildBBTree(this);
		}

		public static global::Pathfinding.NNInfoInternal GetNearest(global::Pathfinding.NavMeshGraph graph, global::Pathfinding.GraphNode[] nodes, global::UnityEngine.Vector3 position, global::Pathfinding.NNConstraint constraint, bool accurateNearestNode)
		{
			if (nodes == null || nodes.Length == 0)
			{
				global::UnityEngine.Debug.LogError("NavGraph hasn't been generated yet or does not contain any nodes");
				return default(global::Pathfinding.NNInfoInternal);
			}
			if (constraint == null)
			{
				constraint = global::Pathfinding.NNConstraint.None;
			}
			global::Pathfinding.Int3[] vertices = graph.vertices;
			if (graph.bbTree == null)
			{
				return global::Pathfinding.NavMeshGraph.GetNearestForce(graph, graph, position, constraint, accurateNearestNode);
			}
			float num = (graph.bbTree.Size.width + graph.bbTree.Size.height) * 0.5f * 0.02f;
			global::Pathfinding.NNInfoInternal result = graph.bbTree.QueryCircle(position, num, constraint);
			if (result.node == null)
			{
				for (int i = 1; i <= 8; i++)
				{
					result = graph.bbTree.QueryCircle(position, (float)(i * i) * num, constraint);
					if (result.node != null || (float)((i - 1) * (i - 1)) * num > global::AstarPath.active.maxNearestNodeDistance * 2f)
					{
						break;
					}
				}
			}
			if (result.node != null)
			{
				result.clampedPosition = global::Pathfinding.NavMeshGraph.ClosestPointOnNode(result.node as global::Pathfinding.TriangleMeshNode, vertices, position);
			}
			if (result.constrainedNode != null)
			{
				if (constraint.constrainDistance && ((global::UnityEngine.Vector3)result.constrainedNode.position - position).sqrMagnitude > global::AstarPath.active.maxNearestNodeDistanceSqr)
				{
					result.constrainedNode = null;
				}
				else
				{
					result.constClampedPosition = global::Pathfinding.NavMeshGraph.ClosestPointOnNode(result.constrainedNode as global::Pathfinding.TriangleMeshNode, vertices, position);
				}
			}
			return result;
		}

		public override global::Pathfinding.NNInfoInternal GetNearest(global::UnityEngine.Vector3 position, global::Pathfinding.NNConstraint constraint, global::Pathfinding.GraphNode hint)
		{
			return global::Pathfinding.NavMeshGraph.GetNearest(this, this.nodes, position, constraint, this.accurateNearestNode);
		}

		public override global::Pathfinding.NNInfoInternal GetNearestForce(global::UnityEngine.Vector3 position, global::Pathfinding.NNConstraint constraint)
		{
			return global::Pathfinding.NavMeshGraph.GetNearestForce(this, this, position, constraint, this.accurateNearestNode);
		}

		public static global::Pathfinding.NNInfoInternal GetNearestForce(global::Pathfinding.NavGraph graph, global::Pathfinding.INavmeshHolder navmesh, global::UnityEngine.Vector3 position, global::Pathfinding.NNConstraint constraint, bool accurateNearestNode)
		{
			global::Pathfinding.NNInfoInternal nearestForceBoth = global::Pathfinding.NavMeshGraph.GetNearestForceBoth(graph, navmesh, position, constraint, accurateNearestNode);
			nearestForceBoth.node = nearestForceBoth.constrainedNode;
			nearestForceBoth.clampedPosition = nearestForceBoth.constClampedPosition;
			return nearestForceBoth;
		}

		public static global::Pathfinding.NNInfoInternal GetNearestForceBoth(global::Pathfinding.NavGraph graph, global::Pathfinding.INavmeshHolder navmesh, global::UnityEngine.Vector3 position, global::Pathfinding.NNConstraint constraint, bool accurateNearestNode)
		{
			global::Pathfinding.Int3 pos = (global::Pathfinding.Int3)position;
			float minDist = -1f;
			global::Pathfinding.GraphNode minNode = null;
			float minConstDist = -1f;
			global::Pathfinding.GraphNode minConstNode = null;
			float maxDistSqr = (!constraint.constrainDistance) ? float.PositiveInfinity : global::AstarPath.active.maxNearestNodeDistanceSqr;
			global::Pathfinding.GraphNodeDelegateCancelable del = delegate(global::Pathfinding.GraphNode _node)
			{
				global::Pathfinding.TriangleMeshNode triangleMeshNode3 = _node as global::Pathfinding.TriangleMeshNode;
				if (accurateNearestNode)
				{
					global::UnityEngine.Vector3 b = triangleMeshNode3.ClosestPointOnNode(position);
					float sqrMagnitude = ((global::UnityEngine.Vector3)pos - b).sqrMagnitude;
					if (minNode == null || sqrMagnitude < minDist)
					{
						minDist = sqrMagnitude;
						minNode = triangleMeshNode3;
					}
					if (sqrMagnitude < maxDistSqr && constraint.Suitable(triangleMeshNode3) && (minConstNode == null || sqrMagnitude < minConstDist))
					{
						minConstDist = sqrMagnitude;
						minConstNode = triangleMeshNode3;
					}
				}
				else if (!triangleMeshNode3.ContainsPoint((global::Pathfinding.Int3)position))
				{
					float sqrMagnitude2 = (triangleMeshNode3.position - pos).sqrMagnitude;
					if (minNode == null || sqrMagnitude2 < minDist)
					{
						minDist = sqrMagnitude2;
						minNode = triangleMeshNode3;
					}
					if (sqrMagnitude2 < maxDistSqr && constraint.Suitable(triangleMeshNode3) && (minConstNode == null || sqrMagnitude2 < minConstDist))
					{
						minConstDist = sqrMagnitude2;
						minConstNode = triangleMeshNode3;
					}
				}
				else
				{
					int num = global::System.Math.Abs(triangleMeshNode3.position.y - pos.y);
					if (minNode == null || (float)num < minDist)
					{
						minDist = (float)num;
						minNode = triangleMeshNode3;
					}
					if ((float)num < maxDistSqr && constraint.Suitable(triangleMeshNode3) && (minConstNode == null || (float)num < minConstDist))
					{
						minConstDist = (float)num;
						minConstNode = triangleMeshNode3;
					}
				}
				return true;
			};
			graph.GetNodes(del);
			global::Pathfinding.NNInfoInternal result = new global::Pathfinding.NNInfoInternal(minNode);
			if (result.node != null)
			{
				global::Pathfinding.TriangleMeshNode triangleMeshNode = result.node as global::Pathfinding.TriangleMeshNode;
				global::UnityEngine.Vector3 clampedPosition = triangleMeshNode.ClosestPointOnNode(position);
				result.clampedPosition = clampedPosition;
			}
			result.constrainedNode = minConstNode;
			if (result.constrainedNode != null)
			{
				global::Pathfinding.TriangleMeshNode triangleMeshNode2 = result.constrainedNode as global::Pathfinding.TriangleMeshNode;
				global::UnityEngine.Vector3 constClampedPosition = triangleMeshNode2.ClosestPointOnNode(position);
				result.constClampedPosition = constClampedPosition;
			}
			return result;
		}

		public bool Linecast(global::UnityEngine.Vector3 origin, global::UnityEngine.Vector3 end)
		{
			return this.Linecast(origin, end, base.GetNearest(origin, global::Pathfinding.NNConstraint.None).node);
		}

		public bool Linecast(global::UnityEngine.Vector3 origin, global::UnityEngine.Vector3 end, global::Pathfinding.GraphNode hint, out global::Pathfinding.GraphHitInfo hit)
		{
			return global::Pathfinding.NavMeshGraph.Linecast(this, origin, end, hint, out hit, null);
		}

		public bool Linecast(global::UnityEngine.Vector3 origin, global::UnityEngine.Vector3 end, global::Pathfinding.GraphNode hint)
		{
			global::Pathfinding.GraphHitInfo graphHitInfo;
			return global::Pathfinding.NavMeshGraph.Linecast(this, origin, end, hint, out graphHitInfo, null);
		}

		public bool Linecast(global::UnityEngine.Vector3 origin, global::UnityEngine.Vector3 end, global::Pathfinding.GraphNode hint, out global::Pathfinding.GraphHitInfo hit, global::System.Collections.Generic.List<global::Pathfinding.GraphNode> trace)
		{
			return global::Pathfinding.NavMeshGraph.Linecast(this, origin, end, hint, out hit, trace);
		}

		public static bool Linecast(global::Pathfinding.INavmesh graph, global::UnityEngine.Vector3 tmp_origin, global::UnityEngine.Vector3 tmp_end, global::Pathfinding.GraphNode hint, out global::Pathfinding.GraphHitInfo hit)
		{
			return global::Pathfinding.NavMeshGraph.Linecast(graph, tmp_origin, tmp_end, hint, out hit, null);
		}

		public static bool Linecast(global::Pathfinding.INavmesh graph, global::UnityEngine.Vector3 tmp_origin, global::UnityEngine.Vector3 tmp_end, global::Pathfinding.GraphNode hint, out global::Pathfinding.GraphHitInfo hit, global::System.Collections.Generic.List<global::Pathfinding.GraphNode> trace)
		{
			global::Pathfinding.Int3 @int = (global::Pathfinding.Int3)tmp_end;
			global::Pathfinding.Int3 int2 = (global::Pathfinding.Int3)tmp_origin;
			hit = default(global::Pathfinding.GraphHitInfo);
			if (float.IsNaN(tmp_origin.x + tmp_origin.y + tmp_origin.z))
			{
				throw new global::System.ArgumentException("origin is NaN");
			}
			if (float.IsNaN(tmp_end.x + tmp_end.y + tmp_end.z))
			{
				throw new global::System.ArgumentException("end is NaN");
			}
			global::Pathfinding.TriangleMeshNode triangleMeshNode = hint as global::Pathfinding.TriangleMeshNode;
			if (triangleMeshNode == null)
			{
				triangleMeshNode = ((graph as global::Pathfinding.NavGraph).GetNearest(tmp_origin, global::Pathfinding.NNConstraint.None).node as global::Pathfinding.TriangleMeshNode);
				if (triangleMeshNode == null)
				{
					global::UnityEngine.Debug.LogError("Could not find a valid node to start from");
					hit.point = tmp_origin;
					return true;
				}
			}
			if (int2 == @int)
			{
				hit.node = triangleMeshNode;
				return false;
			}
			int2 = (global::Pathfinding.Int3)triangleMeshNode.ClosestPointOnNode((global::UnityEngine.Vector3)int2);
			hit.origin = (global::UnityEngine.Vector3)int2;
			if (!triangleMeshNode.Walkable)
			{
				hit.point = (global::UnityEngine.Vector3)int2;
				hit.tangentOrigin = (global::UnityEngine.Vector3)int2;
				return true;
			}
			global::System.Collections.Generic.List<global::UnityEngine.Vector3> list = global::Pathfinding.Util.ListPool<global::UnityEngine.Vector3>.Claim();
			global::System.Collections.Generic.List<global::UnityEngine.Vector3> list2 = global::Pathfinding.Util.ListPool<global::UnityEngine.Vector3>.Claim();
			int num = 0;
			for (;;)
			{
				num++;
				if (num > 2000)
				{
					break;
				}
				global::Pathfinding.TriangleMeshNode triangleMeshNode2 = null;
				if (trace != null)
				{
					trace.Add(triangleMeshNode);
				}
				if (triangleMeshNode.ContainsPoint(@int))
				{
					goto Block_9;
				}
				for (int i = 0; i < triangleMeshNode.connections.Length; i++)
				{
					if (triangleMeshNode.connections[i].GraphIndex == triangleMeshNode.GraphIndex)
					{
						list.Clear();
						list2.Clear();
						if (triangleMeshNode.GetPortal(triangleMeshNode.connections[i], list, list2, false))
						{
							global::UnityEngine.Vector3 vector = list[0];
							global::UnityEngine.Vector3 vector2 = list2[0];
							if (global::Pathfinding.VectorMath.RightXZ(vector, vector2, hit.origin) || !global::Pathfinding.VectorMath.RightXZ(vector, vector2, tmp_end))
							{
								float num2;
								float num3;
								if (global::Pathfinding.VectorMath.LineIntersectionFactorXZ(vector, vector2, hit.origin, tmp_end, out num2, out num3))
								{
									if (num3 >= 0f)
									{
										if (num2 >= 0f && num2 <= 1f)
										{
											triangleMeshNode2 = (triangleMeshNode.connections[i] as global::Pathfinding.TriangleMeshNode);
											break;
										}
									}
								}
							}
						}
					}
				}
				if (triangleMeshNode2 == null)
				{
					goto Block_18;
				}
				triangleMeshNode = triangleMeshNode2;
			}
			global::UnityEngine.Debug.LogError("Linecast was stuck in infinite loop. Breaking.");
			global::Pathfinding.Util.ListPool<global::UnityEngine.Vector3>.Release(list);
			global::Pathfinding.Util.ListPool<global::UnityEngine.Vector3>.Release(list2);
			return true;
			Block_9:
			global::Pathfinding.Util.ListPool<global::UnityEngine.Vector3>.Release(list);
			global::Pathfinding.Util.ListPool<global::UnityEngine.Vector3>.Release(list2);
			return false;
			Block_18:
			int vertexCount = triangleMeshNode.GetVertexCount();
			for (int j = 0; j < vertexCount; j++)
			{
				global::UnityEngine.Vector3 vector3 = (global::UnityEngine.Vector3)triangleMeshNode.GetVertex(j);
				global::UnityEngine.Vector3 vector4 = (global::UnityEngine.Vector3)triangleMeshNode.GetVertex((j + 1) % vertexCount);
				if (global::Pathfinding.VectorMath.RightXZ(vector3, vector4, hit.origin) || !global::Pathfinding.VectorMath.RightXZ(vector3, vector4, tmp_end))
				{
					float num4;
					float num5;
					if (global::Pathfinding.VectorMath.LineIntersectionFactorXZ(vector3, vector4, hit.origin, tmp_end, out num4, out num5))
					{
						if (num5 >= 0f)
						{
							if (num4 >= 0f && num4 <= 1f)
							{
								global::UnityEngine.Vector3 point = vector3 + (vector4 - vector3) * num4;
								hit.point = point;
								hit.node = triangleMeshNode;
								hit.tangent = vector4 - vector3;
								hit.tangentOrigin = vector3;
								global::Pathfinding.Util.ListPool<global::UnityEngine.Vector3>.Release(list);
								global::Pathfinding.Util.ListPool<global::UnityEngine.Vector3>.Release(list2);
								return true;
							}
						}
					}
				}
			}
			global::UnityEngine.Debug.LogWarning("Linecast failing because point not inside node, and line does not hit any edges of it");
			global::Pathfinding.Util.ListPool<global::UnityEngine.Vector3>.Release(list);
			global::Pathfinding.Util.ListPool<global::UnityEngine.Vector3>.Release(list2);
			return false;
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

		public void UpdateArea(global::Pathfinding.GraphUpdateObject o)
		{
			global::Pathfinding.NavMeshGraph.UpdateArea(o, this);
		}

		public static void UpdateArea(global::Pathfinding.GraphUpdateObject o, global::Pathfinding.INavmesh graph)
		{
			global::UnityEngine.Bounds bounds = o.bounds;
			global::UnityEngine.Rect r = global::UnityEngine.Rect.MinMaxRect(bounds.min.x, bounds.min.z, bounds.max.x, bounds.max.z);
			global::Pathfinding.IntRect r2 = new global::Pathfinding.IntRect(global::UnityEngine.Mathf.FloorToInt(bounds.min.x * 1000f), global::UnityEngine.Mathf.FloorToInt(bounds.min.z * 1000f), global::UnityEngine.Mathf.FloorToInt(bounds.max.x * 1000f), global::UnityEngine.Mathf.FloorToInt(bounds.max.z * 1000f));
			global::Pathfinding.Int3 a = new global::Pathfinding.Int3(r2.xmin, 0, r2.ymin);
			global::Pathfinding.Int3 b = new global::Pathfinding.Int3(r2.xmin, 0, r2.ymax);
			global::Pathfinding.Int3 c = new global::Pathfinding.Int3(r2.xmax, 0, r2.ymin);
			global::Pathfinding.Int3 d = new global::Pathfinding.Int3(r2.xmax, 0, r2.ymax);
			int ymin = ((global::Pathfinding.Int3)bounds.min).y;
			int ymax = ((global::Pathfinding.Int3)bounds.max).y;
			graph.GetNodes(delegate(global::Pathfinding.GraphNode _node)
			{
				global::Pathfinding.TriangleMeshNode triangleMeshNode = _node as global::Pathfinding.TriangleMeshNode;
				bool flag = false;
				int num = 0;
				int num2 = 0;
				int num3 = 0;
				int num4 = 0;
				for (int i = 0; i < 3; i++)
				{
					global::Pathfinding.Int3 vertex = triangleMeshNode.GetVertex(i);
					global::UnityEngine.Vector3 vector = (global::UnityEngine.Vector3)vertex;
					if (r2.Contains(vertex.x, vertex.z))
					{
						flag = true;
						break;
					}
					if (vector.x < r.xMin)
					{
						num++;
					}
					if (vector.x > r.xMax)
					{
						num2++;
					}
					if (vector.z < r.yMin)
					{
						num3++;
					}
					if (vector.z > r.yMax)
					{
						num4++;
					}
				}
				if (!flag && (num == 3 || num2 == 3 || num3 == 3 || num4 == 3))
				{
					return true;
				}
				for (int j = 0; j < 3; j++)
				{
					int i2 = (j <= 1) ? (j + 1) : 0;
					global::Pathfinding.Int3 vertex2 = triangleMeshNode.GetVertex(j);
					global::Pathfinding.Int3 vertex3 = triangleMeshNode.GetVertex(i2);
					if (global::Pathfinding.VectorMath.SegmentsIntersectXZ(a, b, vertex2, vertex3))
					{
						flag = true;
						break;
					}
					if (global::Pathfinding.VectorMath.SegmentsIntersectXZ(a, c, vertex2, vertex3))
					{
						flag = true;
						break;
					}
					if (global::Pathfinding.VectorMath.SegmentsIntersectXZ(c, d, vertex2, vertex3))
					{
						flag = true;
						break;
					}
					if (global::Pathfinding.VectorMath.SegmentsIntersectXZ(d, b, vertex2, vertex3))
					{
						flag = true;
						break;
					}
				}
				if (flag || triangleMeshNode.ContainsPoint(a) || triangleMeshNode.ContainsPoint(b) || triangleMeshNode.ContainsPoint(c) || triangleMeshNode.ContainsPoint(d))
				{
					flag = true;
				}
				if (!flag)
				{
					return true;
				}
				int num5 = 0;
				int num6 = 0;
				for (int k = 0; k < 3; k++)
				{
					global::Pathfinding.Int3 vertex4 = triangleMeshNode.GetVertex(k);
					if (vertex4.y < ymin)
					{
						num6++;
					}
					if (vertex4.y > ymax)
					{
						num5++;
					}
				}
				if (num6 == 3 || num5 == 3)
				{
					return true;
				}
				o.WillUpdateNode(triangleMeshNode);
				o.Apply(triangleMeshNode);
				return true;
			});
		}

		private static global::UnityEngine.Vector3 ClosestPointOnNode(global::Pathfinding.TriangleMeshNode node, global::Pathfinding.Int3[] vertices, global::UnityEngine.Vector3 pos)
		{
			return global::Pathfinding.Polygon.ClosestPointOnTriangle((global::UnityEngine.Vector3)vertices[node.v0], (global::UnityEngine.Vector3)vertices[node.v1], (global::UnityEngine.Vector3)vertices[node.v2], pos);
		}

		[global::System.Obsolete("Use TriangleMeshNode.ContainsPoint instead")]
		public bool ContainsPoint(global::Pathfinding.TriangleMeshNode node, global::UnityEngine.Vector3 pos)
		{
			return global::Pathfinding.VectorMath.IsClockwiseXZ((global::UnityEngine.Vector3)this.vertices[node.v0], (global::UnityEngine.Vector3)this.vertices[node.v1], pos) && global::Pathfinding.VectorMath.IsClockwiseXZ((global::UnityEngine.Vector3)this.vertices[node.v1], (global::UnityEngine.Vector3)this.vertices[node.v2], pos) && global::Pathfinding.VectorMath.IsClockwiseXZ((global::UnityEngine.Vector3)this.vertices[node.v2], (global::UnityEngine.Vector3)this.vertices[node.v0], pos);
		}

		[global::System.Obsolete("Use TriangleMeshNode.ContainsPoint instead")]
		public static bool ContainsPoint(global::Pathfinding.TriangleMeshNode node, global::UnityEngine.Vector3 pos, global::Pathfinding.Int3[] vertices)
		{
			if (!global::Pathfinding.VectorMath.IsClockwiseMarginXZ((global::UnityEngine.Vector3)vertices[node.v0], (global::UnityEngine.Vector3)vertices[node.v1], (global::UnityEngine.Vector3)vertices[node.v2]))
			{
				global::UnityEngine.Debug.LogError("Noes!");
			}
			return global::Pathfinding.VectorMath.IsClockwiseMarginXZ((global::UnityEngine.Vector3)vertices[node.v0], (global::UnityEngine.Vector3)vertices[node.v1], pos) && global::Pathfinding.VectorMath.IsClockwiseMarginXZ((global::UnityEngine.Vector3)vertices[node.v1], (global::UnityEngine.Vector3)vertices[node.v2], pos) && global::Pathfinding.VectorMath.IsClockwiseMarginXZ((global::UnityEngine.Vector3)vertices[node.v2], (global::UnityEngine.Vector3)vertices[node.v0], pos);
		}

		public void ScanInternal(string objMeshPath)
		{
			global::UnityEngine.Mesh x = global::Pathfinding.ObjImporter.ImportFile(objMeshPath);
			if (x == null)
			{
				global::UnityEngine.Debug.LogError("Couldn't read .obj file at '" + objMeshPath + "'");
				return;
			}
			this.sourceMesh = x;
			global::System.Collections.Generic.IEnumerator<global::Pathfinding.Progress> enumerator = this.ScanInternal().GetEnumerator();
			while (enumerator.MoveNext())
			{
			}
		}

		public override global::System.Collections.Generic.IEnumerable<global::Pathfinding.Progress> ScanInternal()
		{
			this.ScanInternal(delegate(global::Pathfinding.Progress p)
			{
			});
			yield break;
		}

		public void ScanInternal(global::Pathfinding.OnScanStatus statusCallback)
		{
			if (this.sourceMesh == null)
			{
				return;
			}
			this.GenerateMatrix();
			global::UnityEngine.Vector3[] vertices = this.sourceMesh.vertices;
			this.triangles = this.sourceMesh.triangles;
			global::Pathfinding.TriangleMeshNode.SetNavmeshHolder(this.active.astarData.GetGraphIndex(this), this);
			this.GenerateNodes(vertices, this.triangles, out this.originalVertices, out this._vertices);
		}

		private void GenerateNodes(global::UnityEngine.Vector3[] vectorVertices, int[] triangles, out global::UnityEngine.Vector3[] originalVertices, out global::Pathfinding.Int3[] vertices)
		{
			if (vectorVertices.Length == 0 || triangles.Length == 0)
			{
				originalVertices = vectorVertices;
				vertices = new global::Pathfinding.Int3[0];
				this.nodes = new global::Pathfinding.TriangleMeshNode[0];
				return;
			}
			vertices = new global::Pathfinding.Int3[vectorVertices.Length];
			int num = 0;
			for (int i = 0; i < vertices.Length; i++)
			{
				vertices[i] = (global::Pathfinding.Int3)this.matrix.MultiplyPoint3x4(vectorVertices[i]);
			}
			global::System.Collections.Generic.Dictionary<global::Pathfinding.Int3, int> dictionary = new global::System.Collections.Generic.Dictionary<global::Pathfinding.Int3, int>();
			int[] array = new int[vertices.Length];
			for (int j = 0; j < vertices.Length; j++)
			{
				if (!dictionary.ContainsKey(vertices[j]))
				{
					array[num] = j;
					dictionary.Add(vertices[j], num);
					num++;
				}
			}
			for (int k = 0; k < triangles.Length; k++)
			{
				global::Pathfinding.Int3 key = vertices[triangles[k]];
				triangles[k] = dictionary[key];
			}
			global::Pathfinding.Int3[] array2 = vertices;
			vertices = new global::Pathfinding.Int3[num];
			originalVertices = new global::UnityEngine.Vector3[num];
			for (int l = 0; l < num; l++)
			{
				vertices[l] = array2[array[l]];
				originalVertices[l] = vectorVertices[array[l]];
			}
			this.nodes = new global::Pathfinding.TriangleMeshNode[triangles.Length / 3];
			int graphIndex = this.active.astarData.GetGraphIndex(this);
			for (int m = 0; m < this.nodes.Length; m++)
			{
				this.nodes[m] = new global::Pathfinding.TriangleMeshNode(this.active);
				global::Pathfinding.TriangleMeshNode triangleMeshNode = this.nodes[m];
				triangleMeshNode.GraphIndex = (uint)graphIndex;
				triangleMeshNode.Penalty = this.initialPenalty;
				triangleMeshNode.Walkable = true;
				triangleMeshNode.v0 = triangles[m * 3];
				triangleMeshNode.v1 = triangles[m * 3 + 1];
				triangleMeshNode.v2 = triangles[m * 3 + 2];
				if (!global::Pathfinding.VectorMath.IsClockwiseXZ(vertices[triangleMeshNode.v0], vertices[triangleMeshNode.v1], vertices[triangleMeshNode.v2]))
				{
					int v = triangleMeshNode.v0;
					triangleMeshNode.v0 = triangleMeshNode.v2;
					triangleMeshNode.v2 = v;
				}
				if (global::Pathfinding.VectorMath.IsColinearXZ(vertices[triangleMeshNode.v0], vertices[triangleMeshNode.v1], vertices[triangleMeshNode.v2]))
				{
					global::UnityEngine.Debug.DrawLine((global::UnityEngine.Vector3)vertices[triangleMeshNode.v0], (global::UnityEngine.Vector3)vertices[triangleMeshNode.v1], global::UnityEngine.Color.red);
					global::UnityEngine.Debug.DrawLine((global::UnityEngine.Vector3)vertices[triangleMeshNode.v1], (global::UnityEngine.Vector3)vertices[triangleMeshNode.v2], global::UnityEngine.Color.red);
					global::UnityEngine.Debug.DrawLine((global::UnityEngine.Vector3)vertices[triangleMeshNode.v2], (global::UnityEngine.Vector3)vertices[triangleMeshNode.v0], global::UnityEngine.Color.red);
				}
				triangleMeshNode.UpdatePositionFromVertices();
			}
			global::System.Collections.Generic.Dictionary<global::Pathfinding.Int2, global::Pathfinding.TriangleMeshNode> dictionary2 = new global::System.Collections.Generic.Dictionary<global::Pathfinding.Int2, global::Pathfinding.TriangleMeshNode>();
			int n = 0;
			int num2 = 0;
			while (n < triangles.Length)
			{
				dictionary2[new global::Pathfinding.Int2(triangles[n], triangles[n + 1])] = this.nodes[num2];
				dictionary2[new global::Pathfinding.Int2(triangles[n + 1], triangles[n + 2])] = this.nodes[num2];
				dictionary2[new global::Pathfinding.Int2(triangles[n + 2], triangles[n])] = this.nodes[num2];
				num2++;
				n += 3;
			}
			global::System.Collections.Generic.List<global::Pathfinding.MeshNode> list = new global::System.Collections.Generic.List<global::Pathfinding.MeshNode>();
			global::System.Collections.Generic.List<uint> list2 = new global::System.Collections.Generic.List<uint>();
			int num3 = 0;
			int num4 = 0;
			while (num3 < triangles.Length)
			{
				list.Clear();
				list2.Clear();
				global::Pathfinding.TriangleMeshNode triangleMeshNode2 = this.nodes[num4];
				for (int num5 = 0; num5 < 3; num5++)
				{
					global::Pathfinding.TriangleMeshNode triangleMeshNode3;
					if (dictionary2.TryGetValue(new global::Pathfinding.Int2(triangles[num3 + (num5 + 1) % 3], triangles[num3 + num5]), out triangleMeshNode3))
					{
						list.Add(triangleMeshNode3);
						list2.Add((uint)(triangleMeshNode2.position - triangleMeshNode3.position).costMagnitude);
					}
				}
				triangleMeshNode2.connections = list.ToArray();
				triangleMeshNode2.connectionCosts = list2.ToArray();
				num4++;
				num3 += 3;
			}
			global::Pathfinding.NavMeshGraph.RebuildBBTree(this);
		}

		public static void RebuildBBTree(global::Pathfinding.NavMeshGraph graph)
		{
			global::Pathfinding.BBTree bbtree = graph.bbTree;
			bbtree = (bbtree ?? new global::Pathfinding.BBTree());
			bbtree.RebuildFrom(graph.nodes);
			graph.bbTree = bbtree;
		}

		public void PostProcess()
		{
		}

		public override void OnDrawGizmos(bool drawNodes)
		{
			if (!drawNodes)
			{
				return;
			}
			global::UnityEngine.Matrix4x4 matrix = this.matrix;
			this.GenerateMatrix();
			if (this.nodes == null)
			{
			}
			if (this.nodes == null)
			{
				return;
			}
			if (matrix != this.matrix)
			{
				this.RelocateNodes(matrix, this.matrix);
			}
			global::Pathfinding.PathHandler debugPathData = global::AstarPath.active.debugPathData;
			for (int i = 0; i < this.nodes.Length; i++)
			{
				global::Pathfinding.TriangleMeshNode triangleMeshNode = this.nodes[i];
				global::UnityEngine.Gizmos.color = this.NodeColor(triangleMeshNode, global::AstarPath.active.debugPathData);
				if (triangleMeshNode.Walkable)
				{
					if (global::AstarPath.active.showSearchTree && debugPathData != null && debugPathData.GetPathNode(triangleMeshNode).parent != null)
					{
						global::UnityEngine.Gizmos.DrawLine((global::UnityEngine.Vector3)triangleMeshNode.position, (global::UnityEngine.Vector3)debugPathData.GetPathNode(triangleMeshNode).parent.node.position);
					}
					else
					{
						for (int j = 0; j < triangleMeshNode.connections.Length; j++)
						{
							global::UnityEngine.Gizmos.DrawLine((global::UnityEngine.Vector3)triangleMeshNode.position, global::UnityEngine.Vector3.Lerp((global::UnityEngine.Vector3)triangleMeshNode.position, (global::UnityEngine.Vector3)triangleMeshNode.connections[j].position, 0.45f));
						}
					}
					global::UnityEngine.Gizmos.color = global::Pathfinding.AstarColor.MeshEdgeColor;
				}
				else
				{
					global::UnityEngine.Gizmos.color = global::Pathfinding.AstarColor.UnwalkableNode;
				}
				global::UnityEngine.Gizmos.DrawLine((global::UnityEngine.Vector3)this.vertices[triangleMeshNode.v0], (global::UnityEngine.Vector3)this.vertices[triangleMeshNode.v1]);
				global::UnityEngine.Gizmos.DrawLine((global::UnityEngine.Vector3)this.vertices[triangleMeshNode.v1], (global::UnityEngine.Vector3)this.vertices[triangleMeshNode.v2]);
				global::UnityEngine.Gizmos.DrawLine((global::UnityEngine.Vector3)this.vertices[triangleMeshNode.v2], (global::UnityEngine.Vector3)this.vertices[triangleMeshNode.v0]);
			}
		}

		public override void DeserializeExtraInfo(global::Pathfinding.Serialization.GraphSerializationContext ctx)
		{
			uint graphIndex = ctx.graphIndex;
			global::Pathfinding.TriangleMeshNode.SetNavmeshHolder((int)graphIndex, this);
			int num = ctx.reader.ReadInt32();
			int num2 = ctx.reader.ReadInt32();
			if (num == -1)
			{
				this.nodes = new global::Pathfinding.TriangleMeshNode[0];
				this._vertices = new global::Pathfinding.Int3[0];
				this.originalVertices = new global::UnityEngine.Vector3[0];
				return;
			}
			this.nodes = new global::Pathfinding.TriangleMeshNode[num];
			this._vertices = new global::Pathfinding.Int3[num2];
			this.originalVertices = new global::UnityEngine.Vector3[num2];
			for (int i = 0; i < num2; i++)
			{
				this._vertices[i] = ctx.DeserializeInt3();
				this.originalVertices[i] = ctx.DeserializeVector3();
			}
			this.bbTree = new global::Pathfinding.BBTree();
			for (int j = 0; j < num; j++)
			{
				this.nodes[j] = new global::Pathfinding.TriangleMeshNode(this.active);
				global::Pathfinding.TriangleMeshNode triangleMeshNode = this.nodes[j];
				triangleMeshNode.DeserializeNode(ctx);
				triangleMeshNode.UpdatePositionFromVertices();
			}
			this.bbTree.RebuildFrom(this.nodes);
		}

		public override void SerializeExtraInfo(global::Pathfinding.Serialization.GraphSerializationContext ctx)
		{
			if (this.nodes == null || this.originalVertices == null || this._vertices == null || this.originalVertices.Length != this._vertices.Length)
			{
				ctx.writer.Write(-1);
				ctx.writer.Write(-1);
				return;
			}
			ctx.writer.Write(this.nodes.Length);
			ctx.writer.Write(this._vertices.Length);
			for (int i = 0; i < this._vertices.Length; i++)
			{
				ctx.SerializeInt3(this._vertices[i]);
				ctx.SerializeVector3(this.originalVertices[i]);
			}
			for (int j = 0; j < this.nodes.Length; j++)
			{
				this.nodes[j].SerializeNode(ctx);
			}
		}

		public override void DeserializeSettingsCompatibility(global::Pathfinding.Serialization.GraphSerializationContext ctx)
		{
			base.DeserializeSettingsCompatibility(ctx);
			this.sourceMesh = (ctx.DeserializeUnityObject() as global::UnityEngine.Mesh);
			this.offset = ctx.DeserializeVector3();
			this.rotation = ctx.DeserializeVector3();
			this.scale = ctx.reader.ReadSingle();
			this.accurateNearestNode = ctx.reader.ReadBoolean();
		}

		[global::Pathfinding.Serialization.JsonMember]
		public global::UnityEngine.Mesh sourceMesh;

		[global::Pathfinding.Serialization.JsonMember]
		public global::UnityEngine.Vector3 offset;

		[global::Pathfinding.Serialization.JsonMember]
		public global::UnityEngine.Vector3 rotation;

		[global::Pathfinding.Serialization.JsonMember]
		public float scale = 1f;

		[global::Pathfinding.Serialization.JsonMember]
		public bool accurateNearestNode = true;

		public global::Pathfinding.TriangleMeshNode[] nodes;

		private global::Pathfinding.BBTree _bbTree;

		[global::System.NonSerialized]
		private global::Pathfinding.Int3[] _vertices;

		[global::System.NonSerialized]
		private global::UnityEngine.Vector3[] originalVertices;

		[global::System.NonSerialized]
		public int[] triangles;
	}
}
