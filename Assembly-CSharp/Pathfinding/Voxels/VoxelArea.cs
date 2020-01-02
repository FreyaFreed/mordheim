using System;
using UnityEngine;

namespace Pathfinding.Voxels
{
	public class VoxelArea
	{
		public VoxelArea(int width, int depth)
		{
			this.width = width;
			this.depth = depth;
			int num = width * depth;
			this.compactCells = new global::Pathfinding.Voxels.CompactVoxelCell[num];
			this.linkedSpans = new global::Pathfinding.Voxels.LinkedVoxelSpan[(int)((float)num * 8f) + 15 & -16];
			this.ResetLinkedVoxelSpans();
			int[] array = new int[4];
			array[0] = -1;
			array[2] = 1;
			this.DirectionX = array;
			this.DirectionZ = new int[]
			{
				0,
				width,
				0,
				-width
			};
			this.VectorDirection = new global::UnityEngine.Vector3[]
			{
				global::UnityEngine.Vector3.left,
				global::UnityEngine.Vector3.forward,
				global::UnityEngine.Vector3.right,
				global::UnityEngine.Vector3.back
			};
		}

		public void Reset()
		{
			this.ResetLinkedVoxelSpans();
			for (int i = 0; i < this.compactCells.Length; i++)
			{
				this.compactCells[i].count = 0U;
				this.compactCells[i].index = 0U;
			}
		}

		private void ResetLinkedVoxelSpans()
		{
			int num = this.linkedSpans.Length;
			this.linkedSpanCount = this.width * this.depth;
			global::Pathfinding.Voxels.LinkedVoxelSpan linkedVoxelSpan = new global::Pathfinding.Voxels.LinkedVoxelSpan(uint.MaxValue, uint.MaxValue, -1, -1);
			for (int i = 0; i < num; i++)
			{
				this.linkedSpans[i] = linkedVoxelSpan;
				i++;
				this.linkedSpans[i] = linkedVoxelSpan;
				i++;
				this.linkedSpans[i] = linkedVoxelSpan;
				i++;
				this.linkedSpans[i] = linkedVoxelSpan;
				i++;
				this.linkedSpans[i] = linkedVoxelSpan;
				i++;
				this.linkedSpans[i] = linkedVoxelSpan;
				i++;
				this.linkedSpans[i] = linkedVoxelSpan;
				i++;
				this.linkedSpans[i] = linkedVoxelSpan;
				i++;
				this.linkedSpans[i] = linkedVoxelSpan;
				i++;
				this.linkedSpans[i] = linkedVoxelSpan;
				i++;
				this.linkedSpans[i] = linkedVoxelSpan;
				i++;
				this.linkedSpans[i] = linkedVoxelSpan;
				i++;
				this.linkedSpans[i] = linkedVoxelSpan;
				i++;
				this.linkedSpans[i] = linkedVoxelSpan;
				i++;
				this.linkedSpans[i] = linkedVoxelSpan;
				i++;
				this.linkedSpans[i] = linkedVoxelSpan;
			}
			this.removedStackCount = 0;
		}

		public int GetSpanCountAll()
		{
			int num = 0;
			int num2 = this.width * this.depth;
			for (int i = 0; i < num2; i++)
			{
				int num3 = i;
				while (num3 != -1 && this.linkedSpans[num3].bottom != 4294967295U)
				{
					num++;
					num3 = this.linkedSpans[num3].next;
				}
			}
			return num;
		}

		public int GetSpanCount()
		{
			int num = 0;
			int num2 = this.width * this.depth;
			for (int i = 0; i < num2; i++)
			{
				int num3 = i;
				while (num3 != -1 && this.linkedSpans[num3].bottom != 4294967295U)
				{
					if (this.linkedSpans[num3].area != 0)
					{
						num++;
					}
					num3 = this.linkedSpans[num3].next;
				}
			}
			return num;
		}

		private void PushToSpanRemovedStack(int index)
		{
			if (this.removedStackCount == this.removedStack.Length)
			{
				int[] dst = new int[this.removedStackCount * 4];
				global::System.Buffer.BlockCopy(this.removedStack, 0, dst, 0, this.removedStackCount * 4);
				this.removedStack = dst;
			}
			this.removedStack[this.removedStackCount] = index;
			this.removedStackCount++;
		}

		public void AddLinkedSpan(int index, uint bottom, uint top, int area, int voxelWalkableClimb)
		{
			if (this.linkedSpans[index].bottom == 4294967295U)
			{
				this.linkedSpans[index] = new global::Pathfinding.Voxels.LinkedVoxelSpan(bottom, top, area);
				return;
			}
			int num = -1;
			int num2 = index;
			while (index != -1)
			{
				if (this.linkedSpans[index].bottom > top)
				{
					break;
				}
				if (this.linkedSpans[index].top < bottom)
				{
					num = index;
					index = this.linkedSpans[index].next;
				}
				else
				{
					bottom = global::System.Math.Min(this.linkedSpans[index].bottom, bottom);
					top = global::System.Math.Max(this.linkedSpans[index].top, top);
					if (global::System.Math.Abs((int)(top - this.linkedSpans[index].top)) <= voxelWalkableClimb)
					{
						area = global::System.Math.Max(area, this.linkedSpans[index].area);
					}
					int next = this.linkedSpans[index].next;
					if (num != -1)
					{
						this.linkedSpans[num].next = next;
						this.PushToSpanRemovedStack(index);
						index = next;
					}
					else
					{
						if (next == -1)
						{
							this.linkedSpans[num2] = new global::Pathfinding.Voxels.LinkedVoxelSpan(bottom, top, area);
							return;
						}
						this.linkedSpans[num2] = this.linkedSpans[next];
						this.PushToSpanRemovedStack(next);
					}
				}
			}
			if (this.linkedSpanCount >= this.linkedSpans.Length)
			{
				global::Pathfinding.Voxels.LinkedVoxelSpan[] array = this.linkedSpans;
				int num3 = this.linkedSpanCount;
				int num4 = this.removedStackCount;
				this.linkedSpans = new global::Pathfinding.Voxels.LinkedVoxelSpan[this.linkedSpans.Length * 2];
				this.ResetLinkedVoxelSpans();
				this.linkedSpanCount = num3;
				this.removedStackCount = num4;
				for (int i = 0; i < this.linkedSpanCount; i++)
				{
					this.linkedSpans[i] = array[i];
				}
				global::UnityEngine.Debug.Log("Layer estimate too low, doubling size of buffer.\nThis message is harmless.");
			}
			int num5;
			if (this.removedStackCount > 0)
			{
				this.removedStackCount--;
				num5 = this.removedStack[this.removedStackCount];
			}
			else
			{
				num5 = this.linkedSpanCount;
				this.linkedSpanCount++;
			}
			if (num != -1)
			{
				this.linkedSpans[num5] = new global::Pathfinding.Voxels.LinkedVoxelSpan(bottom, top, area, this.linkedSpans[num].next);
				this.linkedSpans[num].next = num5;
			}
			else
			{
				this.linkedSpans[num5] = this.linkedSpans[num2];
				this.linkedSpans[num2] = new global::Pathfinding.Voxels.LinkedVoxelSpan(bottom, top, area, num5);
			}
		}

		public const uint MaxHeight = 65536U;

		public const int MaxHeightInt = 65536;

		public const uint InvalidSpanValue = 4294967295U;

		public const float AvgSpanLayerCountEstimate = 8f;

		public readonly int width;

		public readonly int depth;

		public global::Pathfinding.Voxels.CompactVoxelSpan[] compactSpans;

		public global::Pathfinding.Voxels.CompactVoxelCell[] compactCells;

		public int compactSpanCount;

		public ushort[] tmpUShortArr;

		public int[] areaTypes;

		public ushort[] dist;

		public ushort maxDistance;

		public int maxRegions;

		public int[] DirectionX;

		public int[] DirectionZ;

		public global::UnityEngine.Vector3[] VectorDirection;

		private int linkedSpanCount;

		public global::Pathfinding.Voxels.LinkedVoxelSpan[] linkedSpans;

		private int[] removedStack = new int[128];

		private int removedStackCount;
	}
}
