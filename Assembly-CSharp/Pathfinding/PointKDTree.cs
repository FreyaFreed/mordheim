using System;
using System.Collections.Generic;

namespace Pathfinding
{
	public class PointKDTree
	{
		public PointKDTree()
		{
			this.tree[1] = new global::Pathfinding.PointKDTree.Node
			{
				data = this.GetOrCreateList()
			};
		}

		public void Add(global::Pathfinding.GraphNode node)
		{
			this.numNodes++;
			this.Add(node, 1, 0);
		}

		public void Rebuild(global::Pathfinding.GraphNode[] nodes, int start, int end)
		{
			if (start < 0 || end < start || end > nodes.Length)
			{
				throw new global::System.ArgumentException();
			}
			for (int i = 0; i < this.tree.Length; i++)
			{
				if (this.tree[i].data != null)
				{
					this.tree[i].data.Clear();
					this.listCache.Push(this.tree[i].data);
					this.tree[i].data = null;
				}
			}
			this.numNodes = end - start;
			this.Build(1, new global::System.Collections.Generic.List<global::Pathfinding.GraphNode>(nodes), start, end);
		}

		private global::System.Collections.Generic.List<global::Pathfinding.GraphNode> GetOrCreateList()
		{
			return (this.listCache.Count <= 0) ? new global::System.Collections.Generic.List<global::Pathfinding.GraphNode>(global::Pathfinding.PointKDTree.LeafSize * 2 + 1) : this.listCache.Pop();
		}

		private int Size(int index)
		{
			return (this.tree[index].data == null) ? (this.Size(2 * index) + this.Size(2 * index + 1)) : this.tree[index].data.Count;
		}

		private void CollectAndClear(int index, global::System.Collections.Generic.List<global::Pathfinding.GraphNode> buffer)
		{
			global::System.Collections.Generic.List<global::Pathfinding.GraphNode> data = this.tree[index].data;
			if (data != null)
			{
				this.tree[index] = default(global::Pathfinding.PointKDTree.Node);
				for (int i = 0; i < data.Count; i++)
				{
					buffer.Add(data[i]);
				}
				data.Clear();
				this.listCache.Push(data);
			}
			else
			{
				this.CollectAndClear(index * 2, buffer);
				this.CollectAndClear(index * 2 + 1, buffer);
			}
		}

		private static int MaxAllowedSize(int numNodes, int depth)
		{
			return global::System.Math.Min(5 * numNodes / 2 >> depth, 3 * numNodes / 4);
		}

		private void Rebalance(int index)
		{
			this.CollectAndClear(index, this.largeList);
			this.Build(index, this.largeList, 0, this.largeList.Count);
			this.largeList.Clear();
		}

		private void EnsureSize(int index)
		{
			if (index >= this.tree.Length)
			{
				global::Pathfinding.PointKDTree.Node[] array = new global::Pathfinding.PointKDTree.Node[global::System.Math.Max(index + 1, this.tree.Length * 2)];
				this.tree.CopyTo(array, 0);
				this.tree = array;
			}
		}

		private void Build(int index, global::System.Collections.Generic.List<global::Pathfinding.GraphNode> nodes, int start, int end)
		{
			this.EnsureSize(index);
			if (end - start <= global::Pathfinding.PointKDTree.LeafSize)
			{
				this.tree[index].data = this.GetOrCreateList();
				for (int i = start; i < end; i++)
				{
					this.tree[index].data.Add(nodes[i]);
				}
			}
			else
			{
				global::Pathfinding.Int3 position;
				global::Pathfinding.Int3 lhs = position = nodes[start].position;
				for (int j = start; j < end; j++)
				{
					global::Pathfinding.Int3 position2 = nodes[j].position;
					position = new global::Pathfinding.Int3(global::System.Math.Min(position.x, position2.x), global::System.Math.Min(position.y, position2.y), global::System.Math.Min(position.z, position2.z));
					lhs = new global::Pathfinding.Int3(global::System.Math.Max(lhs.x, position2.x), global::System.Math.Max(lhs.y, position2.y), global::System.Math.Max(lhs.z, position2.z));
				}
				global::Pathfinding.Int3 @int = lhs - position;
				int num = (@int.x <= @int.y) ? ((@int.y <= @int.z) ? 2 : 1) : ((@int.x <= @int.z) ? 2 : 0);
				nodes.Sort(start, end - start, global::Pathfinding.PointKDTree.comparers[num]);
				int num2 = (start + end) / 2;
				this.tree[index].split = (nodes[num2 - 1].position[num] + nodes[num2].position[num] + 1) / 2;
				this.tree[index].splitAxis = (byte)num;
				this.Build(index * 2, nodes, start, num2);
				this.Build(index * 2 + 1, nodes, num2, end);
			}
		}

		private void Add(global::Pathfinding.GraphNode point, int index, int depth = 0)
		{
			while (this.tree[index].data == null)
			{
				index = 2 * index + ((point.position[(int)this.tree[index].splitAxis] >= this.tree[index].split) ? 1 : 0);
				depth++;
			}
			this.tree[index].data.Add(point);
			if (this.tree[index].data.Count > global::Pathfinding.PointKDTree.LeafSize * 2)
			{
				int num = 0;
				while (depth - num > 0 && this.Size(index >> num) > global::Pathfinding.PointKDTree.MaxAllowedSize(this.numNodes, depth - num))
				{
					num++;
				}
				this.Rebalance(index >> num);
			}
		}

		public global::Pathfinding.GraphNode GetNearest(global::Pathfinding.Int3 point, global::Pathfinding.NNConstraint constraint)
		{
			global::Pathfinding.GraphNode result = null;
			long maxValue = long.MaxValue;
			this.GetNearestInternal(1, point, constraint, ref result, ref maxValue);
			return result;
		}

		private void GetNearestInternal(int index, global::Pathfinding.Int3 point, global::Pathfinding.NNConstraint constraint, ref global::Pathfinding.GraphNode best, ref long bestSqrDist)
		{
			global::System.Collections.Generic.List<global::Pathfinding.GraphNode> data = this.tree[index].data;
			if (data != null)
			{
				for (int i = data.Count - 1; i >= 0; i--)
				{
					long sqrMagnitudeLong = (data[i].position - point).sqrMagnitudeLong;
					if (sqrMagnitudeLong < bestSqrDist && (constraint == null || constraint.Suitable(data[i])))
					{
						bestSqrDist = sqrMagnitudeLong;
						best = data[i];
					}
				}
			}
			else
			{
				long num = (long)(point[(int)this.tree[index].splitAxis] - this.tree[index].split);
				int num2 = 2 * index + ((num >= 0L) ? 1 : 0);
				this.GetNearestInternal(num2, point, constraint, ref best, ref bestSqrDist);
				if (num * num < bestSqrDist)
				{
					this.GetNearestInternal(num2 ^ 1, point, constraint, ref best, ref bestSqrDist);
				}
			}
		}

		public void GetInRange(global::Pathfinding.Int3 point, long sqrRadius, global::System.Collections.Generic.List<global::Pathfinding.GraphNode> buffer)
		{
			this.GetInRangeInternal(1, point, sqrRadius, buffer);
		}

		private void GetInRangeInternal(int index, global::Pathfinding.Int3 point, long sqrRadius, global::System.Collections.Generic.List<global::Pathfinding.GraphNode> buffer)
		{
			global::System.Collections.Generic.List<global::Pathfinding.GraphNode> data = this.tree[index].data;
			if (data != null)
			{
				for (int i = data.Count - 1; i >= 0; i--)
				{
					long sqrMagnitudeLong = (data[i].position - point).sqrMagnitudeLong;
					if (sqrMagnitudeLong < sqrRadius)
					{
						buffer.Add(data[i]);
					}
				}
			}
			else
			{
				long num = (long)(point[(int)this.tree[index].splitAxis] - this.tree[index].split);
				int num2 = 2 * index + ((num >= 0L) ? 1 : 0);
				this.GetInRangeInternal(num2, point, sqrRadius, buffer);
				if (num * num < sqrRadius)
				{
					this.GetInRangeInternal(num2 ^ 1, point, sqrRadius, buffer);
				}
			}
		}

		public static int LeafSize = 10;

		private global::Pathfinding.PointKDTree.Node[] tree = new global::Pathfinding.PointKDTree.Node[16];

		private int numNodes;

		private readonly global::System.Collections.Generic.List<global::Pathfinding.GraphNode> largeList = new global::System.Collections.Generic.List<global::Pathfinding.GraphNode>();

		private readonly global::System.Collections.Generic.Stack<global::System.Collections.Generic.List<global::Pathfinding.GraphNode>> listCache = new global::System.Collections.Generic.Stack<global::System.Collections.Generic.List<global::Pathfinding.GraphNode>>();

		private static readonly global::System.Collections.Generic.IComparer<global::Pathfinding.GraphNode>[] comparers = new global::System.Collections.Generic.IComparer<global::Pathfinding.GraphNode>[]
		{
			new global::Pathfinding.PointKDTree.CompareX(),
			new global::Pathfinding.PointKDTree.CompareY(),
			new global::Pathfinding.PointKDTree.CompareZ()
		};

		private struct Node
		{
			public global::System.Collections.Generic.List<global::Pathfinding.GraphNode> data;

			public int split;

			public byte splitAxis;
		}

		private class CompareX : global::System.Collections.Generic.IComparer<global::Pathfinding.GraphNode>
		{
			public int Compare(global::Pathfinding.GraphNode lhs, global::Pathfinding.GraphNode rhs)
			{
				return lhs.position.x.CompareTo(rhs.position.x);
			}
		}

		private class CompareY : global::System.Collections.Generic.IComparer<global::Pathfinding.GraphNode>
		{
			public int Compare(global::Pathfinding.GraphNode lhs, global::Pathfinding.GraphNode rhs)
			{
				return lhs.position.y.CompareTo(rhs.position.y);
			}
		}

		private class CompareZ : global::System.Collections.Generic.IComparer<global::Pathfinding.GraphNode>
		{
			public int Compare(global::Pathfinding.GraphNode lhs, global::Pathfinding.GraphNode rhs)
			{
				return lhs.position.z.CompareTo(rhs.position.z);
			}
		}
	}
}
