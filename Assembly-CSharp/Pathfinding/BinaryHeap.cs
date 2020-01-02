using System;

namespace Pathfinding
{
	public class BinaryHeap
	{
		public BinaryHeap(int capacity)
		{
			capacity = global::Pathfinding.BinaryHeap.RoundUpToNextMultipleMod1(capacity);
			this.heap = new global::Pathfinding.BinaryHeap.Tuple[capacity];
			this.numberOfItems = 0;
		}

		public bool isEmpty
		{
			get
			{
				return this.numberOfItems <= 0;
			}
		}

		private static int RoundUpToNextMultipleMod1(int v)
		{
			return v + (4 - (v - 1) % 4) % 4;
		}

		public void Clear()
		{
			this.numberOfItems = 0;
		}

		internal global::Pathfinding.PathNode GetNode(int i)
		{
			return this.heap[i].node;
		}

		internal void SetF(int i, uint f)
		{
			this.heap[i].F = f;
		}

		private void Expand()
		{
			int num = global::System.Math.Max(this.heap.Length + 4, (int)global::System.Math.Round((double)((float)this.heap.Length * this.growthFactor)));
			num = global::Pathfinding.BinaryHeap.RoundUpToNextMultipleMod1(num);
			if (num > 262144)
			{
				throw new global::System.Exception("Binary Heap Size really large (2^18). A heap size this large is probably the cause of pathfinding running in an infinite loop. \nRemove this check (in BinaryHeap.cs) if you are sure that it is not caused by a bug");
			}
			global::Pathfinding.BinaryHeap.Tuple[] array = new global::Pathfinding.BinaryHeap.Tuple[num];
			for (int i = 0; i < this.heap.Length; i++)
			{
				array[i] = this.heap[i];
			}
			this.heap = array;
		}

		public void Add(global::Pathfinding.PathNode node)
		{
			if (node == null)
			{
				throw new global::System.ArgumentNullException("node");
			}
			if (this.numberOfItems == this.heap.Length)
			{
				this.Expand();
			}
			int num = this.numberOfItems;
			uint f = node.F;
			uint g = node.G;
			while (num != 0)
			{
				int num2 = (num - 1) / 4;
				if (f >= this.heap[num2].F && (f != this.heap[num2].F || g <= this.heap[num2].node.G))
				{
					break;
				}
				this.heap[num] = this.heap[num2];
				num = num2;
			}
			this.heap[num] = new global::Pathfinding.BinaryHeap.Tuple(f, node);
			this.numberOfItems++;
		}

		public global::Pathfinding.PathNode Remove()
		{
			this.numberOfItems--;
			global::Pathfinding.PathNode node = this.heap[0].node;
			global::Pathfinding.BinaryHeap.Tuple tuple = this.heap[this.numberOfItems];
			uint g = tuple.node.G;
			int num = 0;
			for (;;)
			{
				int num2 = num;
				uint num3 = tuple.F;
				int num4 = num2 * 4 + 1;
				if (num4 <= this.numberOfItems)
				{
					uint f = this.heap[num4].F;
					uint f2 = this.heap[num4 + 1].F;
					uint f3 = this.heap[num4 + 2].F;
					uint f4 = this.heap[num4 + 3].F;
					if (num4 < this.numberOfItems && (f < num3 || (f == num3 && this.heap[num4].node.G < g)))
					{
						num3 = f;
						num = num4;
					}
					if (num4 + 1 < this.numberOfItems && (f2 < num3 || (f2 == num3 && this.heap[num4 + 1].node.G < ((num != num2) ? this.heap[num].node.G : g))))
					{
						num3 = f2;
						num = num4 + 1;
					}
					if (num4 + 2 < this.numberOfItems && (f3 < num3 || (f3 == num3 && this.heap[num4 + 2].node.G < ((num != num2) ? this.heap[num].node.G : g))))
					{
						num3 = f3;
						num = num4 + 2;
					}
					if (num4 + 3 < this.numberOfItems && (f4 < num3 || (f4 == num3 && this.heap[num4 + 3].node.G < ((num != num2) ? this.heap[num].node.G : g))))
					{
						num = num4 + 3;
					}
				}
				if (num2 == num)
				{
					break;
				}
				this.heap[num2] = this.heap[num];
			}
			this.heap[num] = tuple;
			return node;
		}

		private void Validate()
		{
			for (int i = 1; i < this.numberOfItems; i++)
			{
				int num = (i - 1) / 4;
				if (this.heap[num].F > this.heap[i].F)
				{
					throw new global::System.Exception(string.Concat(new object[]
					{
						"Invalid state at ",
						i,
						":",
						num,
						" ( ",
						this.heap[num].F,
						" > ",
						this.heap[i].F,
						" ) "
					}));
				}
			}
		}

		public void Rebuild()
		{
			for (int i = 2; i < this.numberOfItems; i++)
			{
				int num = i;
				global::Pathfinding.BinaryHeap.Tuple tuple = this.heap[i];
				uint f = tuple.F;
				while (num != 1)
				{
					int num2 = num / 4;
					if (f >= this.heap[num2].F)
					{
						break;
					}
					this.heap[num] = this.heap[num2];
					this.heap[num2] = tuple;
					num = num2;
				}
			}
		}

		private const int D = 4;

		private const bool SortGScores = true;

		public int numberOfItems;

		public float growthFactor = 2f;

		private global::Pathfinding.BinaryHeap.Tuple[] heap;

		private struct Tuple
		{
			public Tuple(uint f, global::Pathfinding.PathNode node)
			{
				this.F = f;
				this.node = node;
			}

			public uint F;

			public global::Pathfinding.PathNode node;
		}
	}
}
