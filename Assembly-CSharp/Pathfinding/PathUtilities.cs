using System;
using System.Collections.Generic;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding
{
	public static class PathUtilities
	{
		public static bool IsPathPossible(global::Pathfinding.GraphNode n1, global::Pathfinding.GraphNode n2)
		{
			return n1.Walkable && n2.Walkable && n1.Area == n2.Area;
		}

		public static bool IsPathPossible(global::System.Collections.Generic.List<global::Pathfinding.GraphNode> nodes)
		{
			if (nodes.Count == 0)
			{
				return true;
			}
			uint area = nodes[0].Area;
			for (int i = 0; i < nodes.Count; i++)
			{
				if (!nodes[i].Walkable || nodes[i].Area != area)
				{
					return false;
				}
			}
			return true;
		}

		public static bool IsPathPossible(global::System.Collections.Generic.List<global::Pathfinding.GraphNode> nodes, int tagMask)
		{
			if (nodes.Count == 0)
			{
				return true;
			}
			if ((tagMask >> (int)nodes[0].Tag & 1) == 0)
			{
				return false;
			}
			if (!global::Pathfinding.PathUtilities.IsPathPossible(nodes))
			{
				return false;
			}
			global::System.Collections.Generic.List<global::Pathfinding.GraphNode> reachableNodes = global::Pathfinding.PathUtilities.GetReachableNodes(nodes[0], tagMask);
			bool result = true;
			for (int i = 1; i < nodes.Count; i++)
			{
				if (!reachableNodes.Contains(nodes[i]))
				{
					result = false;
					break;
				}
			}
			global::Pathfinding.Util.ListPool<global::Pathfinding.GraphNode>.Release(reachableNodes);
			return result;
		}

		public static global::System.Collections.Generic.List<global::Pathfinding.GraphNode> GetReachableNodes(global::Pathfinding.GraphNode seed, int tagMask = -1)
		{
			global::System.Collections.Generic.Stack<global::Pathfinding.GraphNode> stack = global::Pathfinding.Util.StackPool<global::Pathfinding.GraphNode>.Claim();
			global::System.Collections.Generic.List<global::Pathfinding.GraphNode> list = global::Pathfinding.Util.ListPool<global::Pathfinding.GraphNode>.Claim();
			global::System.Collections.Generic.HashSet<global::Pathfinding.GraphNode> map = new global::System.Collections.Generic.HashSet<global::Pathfinding.GraphNode>();
			global::Pathfinding.GraphNodeDelegate graphNodeDelegate;
			if (tagMask == -1)
			{
				graphNodeDelegate = delegate(global::Pathfinding.GraphNode node)
				{
					if (node.Walkable && map.Add(node))
					{
						list.Add(node);
						stack.Push(node);
					}
				};
			}
			else
			{
				graphNodeDelegate = delegate(global::Pathfinding.GraphNode node)
				{
					if (node.Walkable && (tagMask >> (int)node.Tag & 1) != 0 && map.Add(node))
					{
						list.Add(node);
						stack.Push(node);
					}
				};
			}
			graphNodeDelegate(seed);
			while (stack.Count > 0)
			{
				stack.Pop().GetConnections(graphNodeDelegate);
			}
			global::Pathfinding.Util.StackPool<global::Pathfinding.GraphNode>.Release(stack);
			return list;
		}

		public static global::System.Collections.Generic.List<global::Pathfinding.GraphNode> BFS(global::Pathfinding.GraphNode seed, int depth, int tagMask = -1)
		{
			global::Pathfinding.PathUtilities.BFSQueue = (global::Pathfinding.PathUtilities.BFSQueue ?? new global::System.Collections.Generic.Queue<global::Pathfinding.GraphNode>());
			global::System.Collections.Generic.Queue<global::Pathfinding.GraphNode> que = global::Pathfinding.PathUtilities.BFSQueue;
			global::Pathfinding.PathUtilities.BFSMap = (global::Pathfinding.PathUtilities.BFSMap ?? new global::System.Collections.Generic.Dictionary<global::Pathfinding.GraphNode, int>());
			global::System.Collections.Generic.Dictionary<global::Pathfinding.GraphNode, int> map = global::Pathfinding.PathUtilities.BFSMap;
			que.Clear();
			map.Clear();
			global::System.Collections.Generic.List<global::Pathfinding.GraphNode> result = global::Pathfinding.Util.ListPool<global::Pathfinding.GraphNode>.Claim();
			int currentDist = -1;
			global::Pathfinding.GraphNodeDelegate graphNodeDelegate;
			if (tagMask == -1)
			{
				graphNodeDelegate = delegate(global::Pathfinding.GraphNode node)
				{
					if (node.Walkable && !map.ContainsKey(node))
					{
						map.Add(node, currentDist + 1);
						result.Add(node);
						que.Enqueue(node);
					}
				};
			}
			else
			{
				graphNodeDelegate = delegate(global::Pathfinding.GraphNode node)
				{
					if (node.Walkable && (tagMask >> (int)node.Tag & 1) != 0 && !map.ContainsKey(node))
					{
						map.Add(node, currentDist + 1);
						result.Add(node);
						que.Enqueue(node);
					}
				};
			}
			graphNodeDelegate(seed);
			while (que.Count > 0)
			{
				global::Pathfinding.GraphNode graphNode = que.Dequeue();
				currentDist = map[graphNode];
				if (currentDist >= depth)
				{
					break;
				}
				graphNode.GetConnections(graphNodeDelegate);
			}
			que.Clear();
			map.Clear();
			return result;
		}

		public static global::System.Collections.Generic.List<global::UnityEngine.Vector3> GetSpiralPoints(int count, float clearance)
		{
			global::System.Collections.Generic.List<global::UnityEngine.Vector3> list = global::Pathfinding.Util.ListPool<global::UnityEngine.Vector3>.Claim(count);
			float num = clearance / 6.28318548f;
			float num2 = 0f;
			list.Add(global::Pathfinding.PathUtilities.InvoluteOfCircle(num, num2));
			for (int i = 0; i < count; i++)
			{
				global::UnityEngine.Vector3 b = list[list.Count - 1];
				float num3 = -num2 / 2f + global::UnityEngine.Mathf.Sqrt(num2 * num2 / 4f + 2f * clearance / num);
				float num4 = num2 + num3;
				float num5 = num2 + 2f * num3;
				while (num5 - num4 > 0.01f)
				{
					float num6 = (num4 + num5) / 2f;
					global::UnityEngine.Vector3 a = global::Pathfinding.PathUtilities.InvoluteOfCircle(num, num6);
					if ((a - b).sqrMagnitude < clearance * clearance)
					{
						num4 = num6;
					}
					else
					{
						num5 = num6;
					}
				}
				list.Add(global::Pathfinding.PathUtilities.InvoluteOfCircle(num, num5));
				num2 = num5;
			}
			return list;
		}

		private static global::UnityEngine.Vector3 InvoluteOfCircle(float a, float t)
		{
			return new global::UnityEngine.Vector3(a * (global::UnityEngine.Mathf.Cos(t) + t * global::UnityEngine.Mathf.Sin(t)), 0f, a * (global::UnityEngine.Mathf.Sin(t) - t * global::UnityEngine.Mathf.Cos(t)));
		}

		public static void GetPointsAroundPointWorld(global::UnityEngine.Vector3 p, global::Pathfinding.IRaycastableGraph g, global::System.Collections.Generic.List<global::UnityEngine.Vector3> previousPoints, float radius, float clearanceRadius)
		{
			if (previousPoints.Count == 0)
			{
				return;
			}
			global::UnityEngine.Vector3 vector = global::UnityEngine.Vector3.zero;
			for (int i = 0; i < previousPoints.Count; i++)
			{
				vector += previousPoints[i];
			}
			vector /= (float)previousPoints.Count;
			for (int j = 0; j < previousPoints.Count; j++)
			{
				int index2;
				int index = index2 = j;
				global::UnityEngine.Vector3 a = previousPoints[index2];
				previousPoints[index] = a - vector;
			}
			global::Pathfinding.PathUtilities.GetPointsAroundPoint(p, g, previousPoints, radius, clearanceRadius);
		}

		public static void GetPointsAroundPoint(global::UnityEngine.Vector3 p, global::Pathfinding.IRaycastableGraph g, global::System.Collections.Generic.List<global::UnityEngine.Vector3> previousPoints, float radius, float clearanceRadius)
		{
			if (g == null)
			{
				throw new global::System.ArgumentNullException("g");
			}
			global::Pathfinding.NavGraph navGraph = g as global::Pathfinding.NavGraph;
			if (navGraph == null)
			{
				throw new global::System.ArgumentException("g is not a NavGraph");
			}
			global::Pathfinding.NNInfoInternal nearestForce = navGraph.GetNearestForce(p, global::Pathfinding.NNConstraint.Default);
			p = nearestForce.clampedPosition;
			if (nearestForce.node == null)
			{
				return;
			}
			radius = global::UnityEngine.Mathf.Max(radius, 1.4142f * clearanceRadius * global::UnityEngine.Mathf.Sqrt((float)previousPoints.Count));
			clearanceRadius *= clearanceRadius;
			for (int i = 0; i < previousPoints.Count; i++)
			{
				global::UnityEngine.Vector3 vector = previousPoints[i];
				float magnitude = vector.magnitude;
				if (magnitude > 0f)
				{
					vector /= magnitude;
				}
				float num = radius;
				vector *= num;
				bool flag = false;
				int num2 = 0;
				do
				{
					global::UnityEngine.Vector3 vector2 = p + vector;
					global::Pathfinding.GraphHitInfo graphHitInfo;
					if (g.Linecast(p, vector2, nearestForce.node, out graphHitInfo))
					{
						vector2 = graphHitInfo.point;
					}
					for (float num3 = 0.1f; num3 <= 1f; num3 += 0.05f)
					{
						global::UnityEngine.Vector3 vector3 = (vector2 - p) * num3 + p;
						flag = true;
						for (int j = 0; j < i; j++)
						{
							if ((previousPoints[j] - vector3).sqrMagnitude < clearanceRadius)
							{
								flag = false;
								break;
							}
						}
						if (flag)
						{
							previousPoints[i] = vector3;
							break;
						}
					}
					if (!flag)
					{
						if (num2 > 8)
						{
							flag = true;
						}
						else
						{
							clearanceRadius *= 0.9f;
							vector = global::UnityEngine.Random.onUnitSphere * global::UnityEngine.Mathf.Lerp(num, radius, (float)(num2 / 5));
							vector.y = 0f;
							num2++;
						}
					}
				}
				while (!flag);
			}
		}

		public static global::System.Collections.Generic.List<global::UnityEngine.Vector3> GetPointsOnNodes(global::System.Collections.Generic.List<global::Pathfinding.GraphNode> nodes, int count, float clearanceRadius = 0f)
		{
			if (nodes == null)
			{
				throw new global::System.ArgumentNullException("nodes");
			}
			if (nodes.Count == 0)
			{
				throw new global::System.ArgumentException("no nodes passed");
			}
			global::System.Collections.Generic.List<global::UnityEngine.Vector3> list = global::Pathfinding.Util.ListPool<global::UnityEngine.Vector3>.Claim(count);
			clearanceRadius *= clearanceRadius;
			if (clearanceRadius > 0f || nodes[0] is global::Pathfinding.TriangleMeshNode || nodes[0] is global::Pathfinding.GridNode)
			{
				global::System.Collections.Generic.List<float> list2 = global::Pathfinding.Util.ListPool<float>.Claim(nodes.Count);
				float num = 0f;
				for (int i = 0; i < nodes.Count; i++)
				{
					float num2 = nodes[i].SurfaceArea();
					num2 += 0.001f;
					num += num2;
					list2.Add(num);
				}
				for (int j = 0; j < count; j++)
				{
					int num3 = 0;
					int num4 = 10;
					bool flag = false;
					while (!flag)
					{
						flag = true;
						if (num3 >= num4)
						{
							clearanceRadius *= 0.809999943f;
							num4 += 10;
							if (num4 > 100)
							{
								clearanceRadius = 0f;
							}
						}
						float item = global::UnityEngine.Random.value * num;
						int num5 = list2.BinarySearch(item);
						if (num5 < 0)
						{
							num5 = ~num5;
						}
						if (num5 >= nodes.Count)
						{
							flag = false;
						}
						else
						{
							global::Pathfinding.GraphNode graphNode = nodes[num5];
							global::UnityEngine.Vector3 vector = graphNode.RandomPointOnSurface();
							if (clearanceRadius > 0f)
							{
								for (int k = 0; k < list.Count; k++)
								{
									if ((list[k] - vector).sqrMagnitude < clearanceRadius)
									{
										flag = false;
										break;
									}
								}
							}
							if (flag)
							{
								list.Add(vector);
								break;
							}
							num3++;
						}
					}
				}
				global::Pathfinding.Util.ListPool<float>.Release(list2);
			}
			else
			{
				for (int l = 0; l < count; l++)
				{
					list.Add(nodes[global::UnityEngine.Random.Range(0, nodes.Count)].RandomPointOnSurface());
				}
			}
			return list;
		}

		private static global::System.Collections.Generic.Queue<global::Pathfinding.GraphNode> BFSQueue;

		private static global::System.Collections.Generic.Dictionary<global::Pathfinding.GraphNode, int> BFSMap;
	}
}
