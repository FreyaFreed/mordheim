﻿using System;

namespace Pathfinding.Voxels
{
	public struct CompactVoxelSpan
	{
		public CompactVoxelSpan(ushort bottom, uint height)
		{
			this.con = 24U;
			this.y = bottom;
			this.h = height;
			this.reg = 0;
		}

		public void SetConnection(int dir, uint value)
		{
			int num = dir * 6;
			this.con = (uint)(((ulong)this.con & (ulong)(~(63L << (num & 31)))) | (ulong)((ulong)(value & 63U) << num));
		}

		public int GetConnection(int dir)
		{
			return (int)this.con >> dir * 6 & 63;
		}

		public ushort y;

		public uint con;

		public uint h;

		public int reg;
	}
}
