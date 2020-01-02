using System;

namespace Pathfinding.Voxels
{
	public struct VoxelCell
	{
		public void AddSpan(uint bottom, uint top, int area, int voxelWalkableClimb)
		{
			global::Pathfinding.Voxels.VoxelSpan voxelSpan = new global::Pathfinding.Voxels.VoxelSpan(bottom, top, area);
			if (this.firstSpan == null)
			{
				this.firstSpan = voxelSpan;
				return;
			}
			global::Pathfinding.Voxels.VoxelSpan voxelSpan2 = null;
			global::Pathfinding.Voxels.VoxelSpan voxelSpan3 = this.firstSpan;
			while (voxelSpan3 != null)
			{
				if (voxelSpan3.bottom > voxelSpan.top)
				{
					break;
				}
				if (voxelSpan3.top < voxelSpan.bottom)
				{
					voxelSpan2 = voxelSpan3;
					voxelSpan3 = voxelSpan3.next;
				}
				else
				{
					if (voxelSpan3.bottom < bottom)
					{
						voxelSpan.bottom = voxelSpan3.bottom;
					}
					if (voxelSpan3.top > top)
					{
						voxelSpan.top = voxelSpan3.top;
					}
					if (global::System.Math.Abs((int)(voxelSpan.top - voxelSpan3.top)) <= voxelWalkableClimb)
					{
						voxelSpan.area = global::System.Math.Max(voxelSpan.area, voxelSpan3.area);
					}
					global::Pathfinding.Voxels.VoxelSpan next = voxelSpan3.next;
					if (voxelSpan2 != null)
					{
						voxelSpan2.next = next;
					}
					else
					{
						this.firstSpan = next;
					}
					voxelSpan3 = next;
				}
			}
			if (voxelSpan2 != null)
			{
				voxelSpan.next = voxelSpan2.next;
				voxelSpan2.next = voxelSpan;
			}
			else
			{
				voxelSpan.next = this.firstSpan;
				this.firstSpan = voxelSpan;
			}
		}

		public global::Pathfinding.Voxels.VoxelSpan firstSpan;
	}
}
