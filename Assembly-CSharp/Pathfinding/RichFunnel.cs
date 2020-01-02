using System;
using System.Collections.Generic;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding
{
	public class RichFunnel : global::Pathfinding.RichPathPart
	{
		public RichFunnel()
		{
			this.left = global::Pathfinding.Util.ListPool<global::UnityEngine.Vector3>.Claim();
			this.right = global::Pathfinding.Util.ListPool<global::UnityEngine.Vector3>.Claim();
			this.nodes = new global::System.Collections.Generic.List<global::Pathfinding.TriangleMeshNode>();
			this.graph = null;
		}

		public global::Pathfinding.RichFunnel Initialize(global::Pathfinding.RichPath path, global::Pathfinding.NavGraph graph)
		{
			if (graph == null)
			{
				throw new global::System.ArgumentNullException("graph");
			}
			if (this.graph != null)
			{
				throw new global::System.InvalidOperationException("Trying to initialize an already initialized object. " + graph);
			}
			this.graph = graph;
			this.path = path;
			return this;
		}

		public override void OnEnterPool()
		{
			this.left.Clear();
			this.right.Clear();
			this.nodes.Clear();
			this.graph = null;
			this.currentNode = 0;
			this.checkForDestroyedNodesCounter = 0;
		}

		public global::Pathfinding.TriangleMeshNode CurrentNode
		{
			get
			{
				global::Pathfinding.TriangleMeshNode triangleMeshNode = this.nodes[this.currentNode];
				if (!triangleMeshNode.Destroyed)
				{
					return triangleMeshNode;
				}
				return null;
			}
		}

		public void BuildFunnelCorridor(global::System.Collections.Generic.List<global::Pathfinding.GraphNode> nodes, int start, int end)
		{
			this.exactStart = (nodes[start] as global::Pathfinding.MeshNode).ClosestPointOnNode(this.exactStart);
			this.exactEnd = (nodes[end] as global::Pathfinding.MeshNode).ClosestPointOnNode(this.exactEnd);
			this.left.Clear();
			this.right.Clear();
			this.left.Add(this.exactStart);
			this.right.Add(this.exactStart);
			this.nodes.Clear();
			global::Pathfinding.IRaycastableGraph raycastableGraph = this.graph as global::Pathfinding.IRaycastableGraph;
			if (raycastableGraph != null && this.funnelSimplificationMode != global::Pathfinding.RichFunnel.FunnelSimplification.None)
			{
				global::System.Collections.Generic.List<global::Pathfinding.GraphNode> list = global::Pathfinding.Util.ListPool<global::Pathfinding.GraphNode>.Claim(end - start);
				switch (this.funnelSimplificationMode)
				{
				case global::Pathfinding.RichFunnel.FunnelSimplification.Iterative:
					this.SimplifyPath(raycastableGraph, nodes, start, end, list, this.exactStart, this.exactEnd);
					break;
				case global::Pathfinding.RichFunnel.FunnelSimplification.RecursiveBinary:
					global::Pathfinding.RichFunnel.SimplifyPath2(raycastableGraph, nodes, start, end, list, this.exactStart, this.exactEnd);
					break;
				case global::Pathfinding.RichFunnel.FunnelSimplification.RecursiveTrinary:
					global::Pathfinding.RichFunnel.SimplifyPath3(raycastableGraph, nodes, start, end, list, this.exactStart, this.exactEnd, 0);
					break;
				}
				if (this.nodes.Capacity < list.Count)
				{
					this.nodes.Capacity = list.Count;
				}
				for (int i = 0; i < list.Count; i++)
				{
					global::Pathfinding.TriangleMeshNode triangleMeshNode = list[i] as global::Pathfinding.TriangleMeshNode;
					if (triangleMeshNode != null)
					{
						this.nodes.Add(triangleMeshNode);
					}
				}
				global::Pathfinding.Util.ListPool<global::Pathfinding.GraphNode>.Release(list);
			}
			else
			{
				if (this.nodes.Capacity < end - start)
				{
					this.nodes.Capacity = end - start;
				}
				for (int j = start; j <= end; j++)
				{
					global::Pathfinding.TriangleMeshNode triangleMeshNode2 = nodes[j] as global::Pathfinding.TriangleMeshNode;
					if (triangleMeshNode2 != null)
					{
						this.nodes.Add(triangleMeshNode2);
					}
				}
			}
			for (int k = 0; k < this.nodes.Count - 1; k++)
			{
				this.nodes[k].GetPortal(this.nodes[k + 1], this.left, this.right, false);
			}
			this.left.Add(this.exactEnd);
			this.right.Add(this.exactEnd);
		}

		public static void SimplifyPath3(global::Pathfinding.IRaycastableGraph rcg, global::System.Collections.Generic.List<global::Pathfinding.GraphNode> nodes, int start, int end, global::System.Collections.Generic.List<global::Pathfinding.GraphNode> result, global::UnityEngine.Vector3 startPoint, global::UnityEngine.Vector3 endPoint, int depth = 0)
		{
			if (start == end)
			{
				result.Add(nodes[start]);
				return;
			}
			if (start + 1 == end)
			{
				result.Add(nodes[start]);
				result.Add(nodes[end]);
				return;
			}
			int count = result.Count;
			global::Pathfinding.GraphHitInfo graphHitInfo;
			if (rcg.Linecast(startPoint, endPoint, nodes[start], out graphHitInfo, result) || result[result.Count - 1] != nodes[end])
			{
				result.RemoveRange(count, result.Count - count);
				int num = 0;
				float num2 = 0f;
				for (int i = start + 1; i < end - 1; i++)
				{
					float num3 = global::Pathfinding.VectorMath.SqrDistancePointSegment(startPoint, endPoint, (global::UnityEngine.Vector3)nodes[i].position);
					if (num3 > num2)
					{
						num = i;
						num2 = num3;
					}
				}
				int num4 = (num + start) / 2;
				int num5 = (num + end) / 2;
				if (num4 == num5)
				{
					global::Pathfinding.RichFunnel.SimplifyPath3(rcg, nodes, start, num4, result, startPoint, (global::UnityEngine.Vector3)nodes[num4].position, 0);
					result.RemoveAt(result.Count - 1);
					global::Pathfinding.RichFunnel.SimplifyPath3(rcg, nodes, num4, end, result, (global::UnityEngine.Vector3)nodes[num4].position, endPoint, depth + 1);
				}
				else
				{
					global::Pathfinding.RichFunnel.SimplifyPath3(rcg, nodes, start, num4, result, startPoint, (global::UnityEngine.Vector3)nodes[num4].position, depth + 1);
					result.RemoveAt(result.Count - 1);
					global::Pathfinding.RichFunnel.SimplifyPath3(rcg, nodes, num4, num5, result, (global::UnityEngine.Vector3)nodes[num4].position, (global::UnityEngine.Vector3)nodes[num5].position, depth + 1);
					result.RemoveAt(result.Count - 1);
					global::Pathfinding.RichFunnel.SimplifyPath3(rcg, nodes, num5, end, result, (global::UnityEngine.Vector3)nodes[num5].position, endPoint, depth + 1);
				}
			}
		}

		public static void SimplifyPath2(global::Pathfinding.IRaycastableGraph rcg, global::System.Collections.Generic.List<global::Pathfinding.GraphNode> nodes, int start, int end, global::System.Collections.Generic.List<global::Pathfinding.GraphNode> result, global::UnityEngine.Vector3 startPoint, global::UnityEngine.Vector3 endPoint)
		{
			int count = result.Count;
			if (end <= start + 1)
			{
				result.Add(nodes[start]);
				result.Add(nodes[end]);
				return;
			}
			global::Pathfinding.GraphHitInfo graphHitInfo;
			if (rcg.Linecast(startPoint, endPoint, nodes[start], out graphHitInfo, result) || result[result.Count - 1] != nodes[end])
			{
				result.RemoveRange(count, result.Count - count);
				int num = -1;
				float num2 = float.PositiveInfinity;
				for (int i = start + 1; i < end; i++)
				{
					float num3 = global::Pathfinding.VectorMath.SqrDistancePointSegment(startPoint, endPoint, (global::UnityEngine.Vector3)nodes[i].position);
					if (num == -1 || num3 < num2)
					{
						num = i;
						num2 = num3;
					}
				}
				global::Pathfinding.RichFunnel.SimplifyPath2(rcg, nodes, start, num, result, startPoint, (global::UnityEngine.Vector3)nodes[num].position);
				result.RemoveAt(result.Count - 1);
				global::Pathfinding.RichFunnel.SimplifyPath2(rcg, nodes, num, end, result, (global::UnityEngine.Vector3)nodes[num].position, endPoint);
			}
		}

		public void SimplifyPath(global::Pathfinding.IRaycastableGraph graph, global::System.Collections.Generic.List<global::Pathfinding.GraphNode> nodes, int start, int end, global::System.Collections.Generic.List<global::Pathfinding.GraphNode> result, global::UnityEngine.Vector3 startPoint, global::UnityEngine.Vector3 endPoint)
		{
			if (graph == null)
			{
				throw new global::System.ArgumentNullException("graph");
			}
			if (start > end)
			{
				throw new global::System.ArgumentException("start >= end");
			}
			int num = start;
			int num2 = 0;
			while (num2++ <= 1000)
			{
				if (start == end)
				{
					result.Add(nodes[end]);
					return;
				}
				int count = result.Count;
				int i = end + 1;
				int num3 = start + 1;
				bool flag = false;
				while (i > num3 + 1)
				{
					int num4 = (i + num3) / 2;
					global::UnityEngine.Vector3 start2 = (start != num) ? ((global::UnityEngine.Vector3)nodes[start].position) : startPoint;
					global::UnityEngine.Vector3 end2 = (num4 != end) ? ((global::UnityEngine.Vector3)nodes[num4].position) : endPoint;
					global::Pathfinding.GraphHitInfo graphHitInfo;
					if (graph.Linecast(start2, end2, nodes[start], out graphHitInfo))
					{
						i = num4;
					}
					else
					{
						flag = true;
						num3 = num4;
					}
				}
				if (!flag)
				{
					result.Add(nodes[start]);
					start = num3;
				}
				else
				{
					global::UnityEngine.Vector3 start3 = (start != num) ? ((global::UnityEngine.Vector3)nodes[start].position) : startPoint;
					global::UnityEngine.Vector3 end3 = (num3 != end) ? ((global::UnityEngine.Vector3)nodes[num3].position) : endPoint;
					global::Pathfinding.GraphHitInfo graphHitInfo2;
					graph.Linecast(start3, end3, nodes[start], out graphHitInfo2, result);
					long num5 = 0L;
					long num6 = 0L;
					for (int j = start; j <= num3; j++)
					{
						num5 += (long)((ulong)nodes[j].Penalty + (ulong)((long)((!(this.path.seeker != null)) ? 0 : this.path.seeker.tagPenalties[(int)((global::System.UIntPtr)nodes[j].Tag)])));
					}
					for (int k = count; k < result.Count; k++)
					{
						num6 += (long)((ulong)result[k].Penalty + (ulong)((long)((!(this.path.seeker != null)) ? 0 : this.path.seeker.tagPenalties[(int)((global::System.UIntPtr)result[k].Tag)])));
					}
					if ((double)num5 * 1.4 * (double)(num3 - start + 1) < (double)(num6 * (long)(result.Count - count)) || result[result.Count - 1] != nodes[num3])
					{
						result.RemoveRange(count, result.Count - count);
						result.Add(nodes[start]);
						start++;
					}
					else
					{
						result.RemoveAt(result.Count - 1);
						start = num3;
					}
				}
			}
			global::UnityEngine.Debug.LogError("!!!");
		}

		public void UpdateFunnelCorridor(int splitIndex, global::Pathfinding.TriangleMeshNode prefix)
		{
			if (splitIndex > 0)
			{
				this.nodes.RemoveRange(0, splitIndex - 1);
				this.nodes[0] = prefix;
			}
			else
			{
				this.nodes.Insert(0, prefix);
			}
			this.left.Clear();
			this.right.Clear();
			this.left.Add(this.exactStart);
			this.right.Add(this.exactStart);
			for (int i = 0; i < this.nodes.Count - 1; i++)
			{
				this.nodes[i].GetPortal(this.nodes[i + 1], this.left, this.right, false);
			}
			this.left.Add(this.exactEnd);
			this.right.Add(this.exactEnd);
		}

		private bool CheckForDestroyedNodes()
		{
			int i = 0;
			int count = this.nodes.Count;
			while (i < count)
			{
				if (this.nodes[i].Destroyed)
				{
					return true;
				}
				i++;
			}
			return false;
		}

		public global::UnityEngine.Vector3 ClampToNavmesh(global::UnityEngine.Vector3 position)
		{
			this.ClampToNavmeshInternal(ref position);
			return position;
		}

		public global::UnityEngine.Vector3 Update(global::UnityEngine.Vector3 position, global::System.Collections.Generic.List<global::UnityEngine.Vector3> buffer, int numCorners, out bool lastCorner, out bool requiresRepath)
		{
			lastCorner = false;
			requiresRepath = false;
			if (this.checkForDestroyedNodesCounter >= 10)
			{
				this.checkForDestroyedNodesCounter = 0;
				requiresRepath |= this.CheckForDestroyedNodes();
			}
			else
			{
				this.checkForDestroyedNodesCounter++;
			}
			bool flag = this.ClampToNavmeshInternal(ref position);
			if (flag)
			{
				requiresRepath = true;
				lastCorner = false;
				buffer.Add(position);
				return position;
			}
			this.currentPosition = position;
			if (!this.FindNextCorners(position, this.currentNode, buffer, numCorners, out lastCorner))
			{
				global::UnityEngine.Debug.LogError("Failed to find next corners in the path");
				buffer.Add(position);
				return position;
			}
			return position;
		}

		private bool ClampToNavmeshInternal(ref global::UnityEngine.Vector3 position)
		{
			if (this.nodes[this.currentNode].Destroyed)
			{
				return true;
			}
			global::Pathfinding.Int3 p = (global::Pathfinding.Int3)position;
			if (this.nodes[this.currentNode].ContainsPoint(p))
			{
				return false;
			}
			int i = this.currentNode + 1;
			int num = global::System.Math.Min(this.currentNode + 3, this.nodes.Count);
			while (i < num)
			{
				if (this.nodes[i].Destroyed)
				{
					return true;
				}
				if (this.nodes[i].ContainsPoint(p))
				{
					this.currentNode = i;
					return false;
				}
				i++;
			}
			int j = this.currentNode - 1;
			int num2 = global::System.Math.Max(this.currentNode - 3, 0);
			while (j > num2)
			{
				if (this.nodes[j].Destroyed)
				{
					return true;
				}
				if (this.nodes[j].ContainsPoint(p))
				{
					this.currentNode = j;
					return false;
				}
				j--;
			}
			int index = 0;
			int closestIsNeighbourOf = 0;
			float closestDist = float.PositiveInfinity;
			bool closestIsInPath = false;
			global::Pathfinding.TriangleMeshNode closestNode = null;
			int containingIndex = this.nodes.Count - 1;
			this.checkForDestroyedNodesCounter = 0;
			int k = 0;
			int count = this.nodes.Count;
			while (k < count)
			{
				if (this.nodes[k].Destroyed)
				{
					return true;
				}
				global::UnityEngine.Vector3 a = this.nodes[k].ClosestPointOnNode(position);
				float sqrMagnitude = (a - position).sqrMagnitude;
				if (sqrMagnitude < closestDist)
				{
					closestDist = sqrMagnitude;
					index = k;
					closestNode = this.nodes[k];
					closestIsInPath = true;
				}
				k++;
			}
			global::UnityEngine.Vector3 posCopy = position;
			global::Pathfinding.GraphNodeDelegate del = delegate(global::Pathfinding.GraphNode node)
			{
				if ((containingIndex <= 0 || node != this.nodes[containingIndex - 1]) && (containingIndex >= this.nodes.Count - 1 || node != this.nodes[containingIndex + 1]))
				{
					global::Pathfinding.TriangleMeshNode triangleMeshNode = node as global::Pathfinding.TriangleMeshNode;
					if (triangleMeshNode != null)
					{
						global::UnityEngine.Vector3 a2 = triangleMeshNode.ClosestPointOnNode(posCopy);
						float sqrMagnitude2 = (a2 - posCopy).sqrMagnitude;
						if (sqrMagnitude2 < closestDist)
						{
							closestDist = sqrMagnitude2;
							closestIsNeighbourOf = containingIndex;
							closestNode = triangleMeshNode;
							closestIsInPath = false;
						}
					}
				}
			};
			while (containingIndex >= 0)
			{
				this.nodes[containingIndex].GetConnections(del);
				containingIndex--;
			}
			if (closestIsInPath)
			{
				this.currentNode = index;
				position = this.nodes[index].ClosestPointOnNodeXZ(position);
			}
			else
			{
				position = closestNode.ClosestPointOnNodeXZ(position);
				this.exactStart = position;
				this.UpdateFunnelCorridor(closestIsNeighbourOf, closestNode);
				this.currentNode = 0;
			}
			return false;
		}

		public void FindWalls(global::System.Collections.Generic.List<global::UnityEngine.Vector3> wallBuffer, float range)
		{
			this.FindWalls(this.currentNode, wallBuffer, this.currentPosition, range);
		}

		private void FindWalls(int nodeIndex, global::System.Collections.Generic.List<global::UnityEngine.Vector3> wallBuffer, global::UnityEngine.Vector3 position, float range)
		{
			if (range <= 0f)
			{
				return;
			}
			bool flag = false;
			bool flag2 = false;
			range *= range;
			position.y = 0f;
			int num = 0;
			while (!flag || !flag2)
			{
				if (num >= 0 || !flag)
				{
					if (num <= 0 || !flag2)
					{
						if (num < 0 && nodeIndex + num < 0)
						{
							flag = true;
						}
						else if (num > 0 && nodeIndex + num >= this.nodes.Count)
						{
							flag2 = true;
						}
						else
						{
							global::Pathfinding.TriangleMeshNode triangleMeshNode = (nodeIndex + num - 1 >= 0) ? this.nodes[nodeIndex + num - 1] : null;
							global::Pathfinding.TriangleMeshNode triangleMeshNode2 = this.nodes[nodeIndex + num];
							global::Pathfinding.TriangleMeshNode triangleMeshNode3 = (nodeIndex + num + 1 < this.nodes.Count) ? this.nodes[nodeIndex + num + 1] : null;
							if (triangleMeshNode2.Destroyed)
							{
								break;
							}
							if ((triangleMeshNode2.ClosestPointOnNodeXZ(position) - position).sqrMagnitude > range)
							{
								if (num < 0)
								{
									flag = true;
								}
								else
								{
									flag2 = true;
								}
							}
							else
							{
								for (int i = 0; i < 3; i++)
								{
									this.triBuffer[i] = 0;
								}
								for (int j = 0; j < triangleMeshNode2.connections.Length; j++)
								{
									global::Pathfinding.TriangleMeshNode triangleMeshNode4 = triangleMeshNode2.connections[j] as global::Pathfinding.TriangleMeshNode;
									if (triangleMeshNode4 != null)
									{
										int num2 = -1;
										for (int k = 0; k < 3; k++)
										{
											for (int l = 0; l < 3; l++)
											{
												if (triangleMeshNode2.GetVertex(k) == triangleMeshNode4.GetVertex((l + 1) % 3) && triangleMeshNode2.GetVertex((k + 1) % 3) == triangleMeshNode4.GetVertex(l))
												{
													num2 = k;
													k = 3;
													break;
												}
											}
										}
										if (num2 != -1)
										{
											this.triBuffer[num2] = ((triangleMeshNode4 != triangleMeshNode && triangleMeshNode4 != triangleMeshNode3) ? 1 : 2);
										}
									}
								}
								for (int m = 0; m < 3; m++)
								{
									if (this.triBuffer[m] == 0)
									{
										wallBuffer.Add((global::UnityEngine.Vector3)triangleMeshNode2.GetVertex(m));
										wallBuffer.Add((global::UnityEngine.Vector3)triangleMeshNode2.GetVertex((m + 1) % 3));
									}
								}
							}
						}
					}
				}
				num = ((num >= 0) ? (-num - 1) : (-num));
			}
		}

		public bool FindNextCorners(global::UnityEngine.Vector3 origin, int startIndex, global::System.Collections.Generic.List<global::UnityEngine.Vector3> funnelPath, int numCorners, out bool lastCorner)
		{
			lastCorner = false;
			if (this.left == null)
			{
				throw new global::System.Exception("left list is null");
			}
			if (this.right == null)
			{
				throw new global::System.Exception("right list is null");
			}
			if (funnelPath == null)
			{
				throw new global::System.ArgumentNullException("funnelPath");
			}
			if (this.left.Count != this.right.Count)
			{
				throw new global::System.ArgumentException("left and right lists must have equal length");
			}
			int count = this.left.Count;
			if (count == 0)
			{
				throw new global::System.ArgumentException("no diagonals");
			}
			if (count - startIndex < 3)
			{
				funnelPath.Add(this.left[count - 1]);
				lastCorner = true;
				return true;
			}
			while (this.left[startIndex + 1] == this.left[startIndex + 2] && this.right[startIndex + 1] == this.right[startIndex + 2])
			{
				startIndex++;
				if (count - startIndex <= 3)
				{
					return false;
				}
			}
			global::UnityEngine.Vector3 vector = this.left[startIndex + 2];
			if (vector == this.left[startIndex + 1])
			{
				vector = this.right[startIndex + 2];
			}
			while (global::Pathfinding.VectorMath.IsColinearXZ(origin, this.left[startIndex + 1], this.right[startIndex + 1]) || global::Pathfinding.VectorMath.RightOrColinearXZ(this.left[startIndex + 1], this.right[startIndex + 1], vector) == global::Pathfinding.VectorMath.RightOrColinearXZ(this.left[startIndex + 1], this.right[startIndex + 1], origin))
			{
				startIndex++;
				if (count - startIndex < 3)
				{
					funnelPath.Add(this.left[count - 1]);
					lastCorner = true;
					return true;
				}
				vector = this.left[startIndex + 2];
				if (vector == this.left[startIndex + 1])
				{
					vector = this.right[startIndex + 2];
				}
			}
			global::UnityEngine.Vector3 vector2 = origin;
			global::UnityEngine.Vector3 vector3 = this.left[startIndex + 1];
			global::UnityEngine.Vector3 vector4 = this.right[startIndex + 1];
			int num = startIndex + 1;
			int num2 = startIndex + 1;
			int i = startIndex + 2;
			while (i < count)
			{
				if (funnelPath.Count >= numCorners)
				{
					return true;
				}
				if (funnelPath.Count > 2000)
				{
					global::UnityEngine.Debug.LogWarning("Avoiding infinite loop. Remove this check if you have this long paths.");
					break;
				}
				global::UnityEngine.Vector3 vector5 = this.left[i];
				global::UnityEngine.Vector3 vector6 = this.right[i];
				if (global::Pathfinding.VectorMath.SignedTriangleAreaTimes2XZ(vector2, vector4, vector6) < 0f)
				{
					goto IL_2FB;
				}
				if (vector2 == vector4 || global::Pathfinding.VectorMath.SignedTriangleAreaTimes2XZ(vector2, vector3, vector6) <= 0f)
				{
					vector4 = vector6;
					num = i;
					goto IL_2FB;
				}
				funnelPath.Add(vector3);
				vector2 = vector3;
				int num3 = num2;
				vector3 = vector2;
				vector4 = vector2;
				num2 = num3;
				num = num3;
				i = num3;
				IL_35F:
				i++;
				continue;
				IL_2FB:
				if (global::Pathfinding.VectorMath.SignedTriangleAreaTimes2XZ(vector2, vector3, vector5) > 0f)
				{
					goto IL_35F;
				}
				if (vector2 == vector3 || global::Pathfinding.VectorMath.SignedTriangleAreaTimes2XZ(vector2, vector4, vector5) >= 0f)
				{
					vector3 = vector5;
					num2 = i;
					goto IL_35F;
				}
				funnelPath.Add(vector4);
				vector2 = vector4;
				num3 = num;
				vector3 = vector2;
				vector4 = vector2;
				num2 = num3;
				num = num3;
				i = num3;
				goto IL_35F;
			}
			lastCorner = true;
			funnelPath.Add(this.left[count - 1]);
			return true;
		}

		private readonly global::System.Collections.Generic.List<global::UnityEngine.Vector3> left;

		private readonly global::System.Collections.Generic.List<global::UnityEngine.Vector3> right;

		private global::System.Collections.Generic.List<global::Pathfinding.TriangleMeshNode> nodes;

		public global::UnityEngine.Vector3 exactStart;

		public global::UnityEngine.Vector3 exactEnd;

		private global::Pathfinding.NavGraph graph;

		private int currentNode;

		private global::UnityEngine.Vector3 currentPosition;

		private int checkForDestroyedNodesCounter;

		private global::Pathfinding.RichPath path;

		private int[] triBuffer = new int[3];

		public global::Pathfinding.RichFunnel.FunnelSimplification funnelSimplificationMode = global::Pathfinding.RichFunnel.FunnelSimplification.Iterative;

		public enum FunnelSimplification
		{
			None,
			Iterative,
			RecursiveBinary,
			RecursiveTrinary
		}
	}
}
