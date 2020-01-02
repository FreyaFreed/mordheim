﻿using System;
using System.Collections.Generic;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding.Voxels
{
	public class Voxelize
	{
		public Voxelize(float ch, float cs, float wc, float wh, float ms)
		{
			this.cellSize = cs;
			this.cellHeight = ch;
			this.maxSlope = ms;
			this.cellScale = new global::UnityEngine.Vector3(this.cellSize, this.cellHeight, this.cellSize);
			this.voxelWalkableHeight = (uint)(wh / this.cellHeight);
			this.voxelWalkableClimb = global::UnityEngine.Mathf.RoundToInt(wc / this.cellHeight);
		}

		public void BuildContours(float maxError, int maxEdgeLength, global::Pathfinding.Voxels.VoxelContourSet cset, int buildFlags)
		{
			int num = this.voxelArea.width;
			int num2 = this.voxelArea.depth;
			int num3 = num * num2;
			int capacity = global::UnityEngine.Mathf.Max(8, 8);
			global::System.Collections.Generic.List<global::Pathfinding.Voxels.VoxelContour> list = new global::System.Collections.Generic.List<global::Pathfinding.Voxels.VoxelContour>(capacity);
			ushort[] array = this.voxelArea.tmpUShortArr;
			if (array.Length < this.voxelArea.compactSpanCount)
			{
				array = (this.voxelArea.tmpUShortArr = new ushort[this.voxelArea.compactSpanCount]);
			}
			for (int i = 0; i < num3; i += this.voxelArea.width)
			{
				for (int j = 0; j < this.voxelArea.width; j++)
				{
					global::Pathfinding.Voxels.CompactVoxelCell compactVoxelCell = this.voxelArea.compactCells[j + i];
					int k = (int)compactVoxelCell.index;
					int num4 = (int)(compactVoxelCell.index + compactVoxelCell.count);
					while (k < num4)
					{
						ushort num5 = 0;
						global::Pathfinding.Voxels.CompactVoxelSpan compactVoxelSpan = this.voxelArea.compactSpans[k];
						if (compactVoxelSpan.reg == 0 || (compactVoxelSpan.reg & 32768) == 32768)
						{
							array[k] = 0;
						}
						else
						{
							for (int l = 0; l < 4; l++)
							{
								int num6 = 0;
								if ((long)compactVoxelSpan.GetConnection(l) != 63L)
								{
									int num7 = j + this.voxelArea.DirectionX[l];
									int num8 = i + this.voxelArea.DirectionZ[l];
									int num9 = (int)(this.voxelArea.compactCells[num7 + num8].index + (uint)compactVoxelSpan.GetConnection(l));
									num6 = this.voxelArea.compactSpans[num9].reg;
								}
								if (num6 == compactVoxelSpan.reg)
								{
									num5 |= (ushort)(1 << l);
								}
							}
							array[k] = (num5 ^ 15);
						}
						k++;
					}
				}
			}
			global::System.Collections.Generic.List<int> list2 = global::Pathfinding.Util.ListPool<int>.Claim(256);
			global::System.Collections.Generic.List<int> list3 = global::Pathfinding.Util.ListPool<int>.Claim(64);
			for (int m = 0; m < num3; m += this.voxelArea.width)
			{
				for (int n = 0; n < this.voxelArea.width; n++)
				{
					global::Pathfinding.Voxels.CompactVoxelCell compactVoxelCell2 = this.voxelArea.compactCells[n + m];
					int num10 = (int)compactVoxelCell2.index;
					int num11 = (int)(compactVoxelCell2.index + compactVoxelCell2.count);
					while (num10 < num11)
					{
						if (array[num10] == 0 || array[num10] == 15)
						{
							array[num10] = 0;
						}
						else
						{
							int reg = this.voxelArea.compactSpans[num10].reg;
							if (reg != 0 && (reg & 32768) != 32768)
							{
								int area = this.voxelArea.areaTypes[num10];
								list2.Clear();
								list3.Clear();
								this.WalkContour(n, m, num10, array, list2);
								this.SimplifyContour(list2, list3, maxError, maxEdgeLength, buildFlags);
								this.RemoveDegenerateSegments(list3);
								global::Pathfinding.Voxels.VoxelContour item = default(global::Pathfinding.Voxels.VoxelContour);
								item.verts = global::Pathfinding.Voxels.Voxelize.ClaimIntArr(list3.Count, false);
								for (int num12 = 0; num12 < list3.Count; num12++)
								{
									item.verts[num12] = list3[num12];
								}
								item.nverts = list3.Count / 4;
								item.reg = reg;
								item.area = area;
								list.Add(item);
							}
						}
						num10++;
					}
				}
			}
			global::Pathfinding.Util.ListPool<int>.Release(list2);
			global::Pathfinding.Util.ListPool<int>.Release(list3);
			for (int num13 = 0; num13 < list.Count; num13++)
			{
				global::Pathfinding.Voxels.VoxelContour value = list[num13];
				if (this.CalcAreaOfPolygon2D(value.verts, value.nverts) < 0)
				{
					int num14 = -1;
					for (int num15 = 0; num15 < list.Count; num15++)
					{
						if (num13 != num15)
						{
							if (list[num15].nverts > 0 && list[num15].reg == value.reg && this.CalcAreaOfPolygon2D(list[num15].verts, list[num15].nverts) > 0)
							{
								num14 = num15;
								break;
							}
						}
					}
					if (num14 == -1)
					{
						global::UnityEngine.Debug.LogError("rcBuildContours: Could not find merge target for bad contour " + num13 + ".");
					}
					else
					{
						global::Pathfinding.Voxels.VoxelContour value2 = list[num14];
						int num16 = 0;
						int num17 = 0;
						this.GetClosestIndices(value2.verts, value2.nverts, value.verts, value.nverts, ref num16, ref num17);
						if (num16 == -1 || num17 == -1)
						{
							global::UnityEngine.Debug.LogWarning(string.Concat(new object[]
							{
								"rcBuildContours: Failed to find merge points for ",
								num13,
								" and ",
								num14,
								"."
							}));
						}
						else if (!global::Pathfinding.Voxels.Voxelize.MergeContours(ref value2, ref value, num16, num17))
						{
							global::UnityEngine.Debug.LogWarning(string.Concat(new object[]
							{
								"rcBuildContours: Failed to merge contours ",
								num13,
								" and ",
								num14,
								"."
							}));
						}
						else
						{
							list[num14] = value2;
							list[num13] = value;
						}
					}
				}
			}
			cset.conts = list;
		}

		private void GetClosestIndices(int[] vertsa, int nvertsa, int[] vertsb, int nvertsb, ref int ia, ref int ib)
		{
			int num = 268435455;
			ia = -1;
			ib = -1;
			for (int i = 0; i < nvertsa; i++)
			{
				int num2 = (i + 1) % nvertsa;
				int num3 = (i + nvertsa - 1) % nvertsa;
				int num4 = i * 4;
				int b = num2 * 4;
				int a = num3 * 4;
				for (int j = 0; j < nvertsb; j++)
				{
					int num5 = j * 4;
					if (global::Pathfinding.Voxels.Voxelize.Ileft(a, num4, num5, vertsa, vertsa, vertsb) && global::Pathfinding.Voxels.Voxelize.Ileft(num4, b, num5, vertsa, vertsa, vertsb))
					{
						int num6 = vertsb[num5] - vertsa[num4];
						int num7 = vertsb[num5 + 2] / this.voxelArea.width - vertsa[num4 + 2] / this.voxelArea.width;
						int num8 = num6 * num6 + num7 * num7;
						if (num8 < num)
						{
							ia = i;
							ib = j;
							num = num8;
						}
					}
				}
			}
		}

		private static void ReleaseIntArr(int[] arr)
		{
			if (arr != null)
			{
				global::Pathfinding.Voxels.Voxelize.intArrCache.Add(arr);
			}
		}

		private static int[] ClaimIntArr(int minCapacity, bool zero)
		{
			for (int i = 0; i < global::Pathfinding.Voxels.Voxelize.intArrCache.Count; i++)
			{
				if (global::Pathfinding.Voxels.Voxelize.intArrCache[i].Length >= minCapacity)
				{
					int[] array = global::Pathfinding.Voxels.Voxelize.intArrCache[i];
					global::Pathfinding.Voxels.Voxelize.intArrCache.RemoveAt(i);
					if (zero)
					{
						global::Pathfinding.Util.Memory.MemSet<int>(array, 0, 4);
					}
					return array;
				}
			}
			return new int[minCapacity];
		}

		private static void ReleaseContours(global::Pathfinding.Voxels.VoxelContourSet cset)
		{
			for (int i = 0; i < cset.conts.Count; i++)
			{
				global::Pathfinding.Voxels.VoxelContour voxelContour = cset.conts[i];
				global::Pathfinding.Voxels.Voxelize.ReleaseIntArr(voxelContour.verts);
				global::Pathfinding.Voxels.Voxelize.ReleaseIntArr(voxelContour.rverts);
			}
			cset.conts = null;
		}

		public static bool MergeContours(ref global::Pathfinding.Voxels.VoxelContour ca, ref global::Pathfinding.Voxels.VoxelContour cb, int ia, int ib)
		{
			int num = ca.nverts + cb.nverts + 2;
			int[] array = global::Pathfinding.Voxels.Voxelize.ClaimIntArr(num * 4, false);
			int num2 = 0;
			for (int i = 0; i <= ca.nverts; i++)
			{
				int num3 = num2 * 4;
				int num4 = (ia + i) % ca.nverts * 4;
				array[num3] = ca.verts[num4];
				array[num3 + 1] = ca.verts[num4 + 1];
				array[num3 + 2] = ca.verts[num4 + 2];
				array[num3 + 3] = ca.verts[num4 + 3];
				num2++;
			}
			for (int j = 0; j <= cb.nverts; j++)
			{
				int num5 = num2 * 4;
				int num6 = (ib + j) % cb.nverts * 4;
				array[num5] = cb.verts[num6];
				array[num5 + 1] = cb.verts[num6 + 1];
				array[num5 + 2] = cb.verts[num6 + 2];
				array[num5 + 3] = cb.verts[num6 + 3];
				num2++;
			}
			global::Pathfinding.Voxels.Voxelize.ReleaseIntArr(ca.verts);
			global::Pathfinding.Voxels.Voxelize.ReleaseIntArr(cb.verts);
			ca.verts = array;
			ca.nverts = num2;
			cb.verts = global::Pathfinding.Voxels.Voxelize.emptyArr;
			cb.nverts = 0;
			return true;
		}

		public void SimplifyContour(global::System.Collections.Generic.List<int> verts, global::System.Collections.Generic.List<int> simplified, float maxError, int maxEdgeLenght, int buildFlags)
		{
			bool flag = false;
			for (int i = 0; i < verts.Count; i += 4)
			{
				if ((verts[i + 3] & 65535) != 0)
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				int j = 0;
				int num = verts.Count / 4;
				while (j < num)
				{
					int num2 = (j + 1) % num;
					bool flag2 = (verts[j * 4 + 3] & 65535) != (verts[num2 * 4 + 3] & 65535);
					bool flag3 = (verts[j * 4 + 3] & 131072) != (verts[num2 * 4 + 3] & 131072);
					if (flag2 || flag3)
					{
						simplified.Add(verts[j * 4]);
						simplified.Add(verts[j * 4 + 1]);
						simplified.Add(verts[j * 4 + 2]);
						simplified.Add(j);
					}
					j++;
				}
			}
			if (simplified.Count == 0)
			{
				int num3 = verts[0];
				int item = verts[1];
				int num4 = verts[2];
				int item2 = 0;
				int num5 = verts[0];
				int item3 = verts[1];
				int num6 = verts[2];
				int item4 = 0;
				for (int k = 0; k < verts.Count; k += 4)
				{
					int num7 = verts[k];
					int num8 = verts[k + 1];
					int num9 = verts[k + 2];
					if (num7 < num3 || (num7 == num3 && num9 < num4))
					{
						num3 = num7;
						item = num8;
						num4 = num9;
						item2 = k / 4;
					}
					if (num7 > num5 || (num7 == num5 && num9 > num6))
					{
						num5 = num7;
						item3 = num8;
						num6 = num9;
						item4 = k / 4;
					}
				}
				simplified.Add(num3);
				simplified.Add(item);
				simplified.Add(num4);
				simplified.Add(item2);
				simplified.Add(num5);
				simplified.Add(item3);
				simplified.Add(num6);
				simplified.Add(item4);
			}
			int num10 = verts.Count / 4;
			maxError *= maxError;
			int l = 0;
			while (l < simplified.Count / 4)
			{
				int num11 = (l + 1) % (simplified.Count / 4);
				int num12 = simplified[l * 4];
				int num13 = simplified[l * 4 + 2];
				int num14 = simplified[l * 4 + 3];
				int num15 = simplified[num11 * 4];
				int num16 = simplified[num11 * 4 + 2];
				int num17 = simplified[num11 * 4 + 3];
				float num18 = 0f;
				int num19 = -1;
				int num20;
				int num21;
				int num22;
				if (num15 > num12 || (num15 == num12 && num16 > num13))
				{
					num20 = 1;
					num21 = (num14 + num20) % num10;
					num22 = num17;
				}
				else
				{
					num20 = num10 - 1;
					num21 = (num17 + num20) % num10;
					num22 = num14;
					global::Pathfinding.Voxels.Utility.Swap(ref num12, ref num15);
					global::Pathfinding.Voxels.Utility.Swap(ref num13, ref num16);
				}
				if ((verts[num21 * 4 + 3] & 65535) == 0 || (verts[num21 * 4 + 3] & 131072) == 131072)
				{
					while (num21 != num22)
					{
						float num23 = global::Pathfinding.VectorMath.SqrDistancePointSegmentApproximate(verts[num21 * 4], verts[num21 * 4 + 2] / this.voxelArea.width, num12, num13 / this.voxelArea.width, num15, num16 / this.voxelArea.width);
						if (num23 > num18)
						{
							num18 = num23;
							num19 = num21;
						}
						num21 = (num21 + num20) % num10;
					}
				}
				if (num19 != -1 && num18 > maxError)
				{
					simplified.Add(0);
					simplified.Add(0);
					simplified.Add(0);
					simplified.Add(0);
					int num24 = simplified.Count / 4;
					for (int m = num24 - 1; m > l; m--)
					{
						simplified[m * 4] = simplified[(m - 1) * 4];
						simplified[m * 4 + 1] = simplified[(m - 1) * 4 + 1];
						simplified[m * 4 + 2] = simplified[(m - 1) * 4 + 2];
						simplified[m * 4 + 3] = simplified[(m - 1) * 4 + 3];
					}
					simplified[(l + 1) * 4] = verts[num19 * 4];
					simplified[(l + 1) * 4 + 1] = verts[num19 * 4 + 1];
					simplified[(l + 1) * 4 + 2] = verts[num19 * 4 + 2];
					simplified[(l + 1) * 4 + 3] = num19;
				}
				else
				{
					l++;
				}
			}
			float num25 = this.maxEdgeLength / this.cellSize;
			if (num25 > 0f && (buildFlags & 3) != 0)
			{
				int n = 0;
				while (n < simplified.Count / 4)
				{
					if (simplified.Count / 4 > 200)
					{
						break;
					}
					int num26 = (n + 1) % (simplified.Count / 4);
					int num27 = simplified[n * 4];
					int num28 = simplified[n * 4 + 2];
					int num29 = simplified[n * 4 + 3];
					int num30 = simplified[num26 * 4];
					int num31 = simplified[num26 * 4 + 2];
					int num32 = simplified[num26 * 4 + 3];
					int num33 = -1;
					int num34 = (num29 + 1) % num10;
					bool flag4 = false;
					if ((buildFlags & 1) == 1 && (verts[num34 * 4 + 3] & 65535) == 0)
					{
						flag4 = true;
					}
					if ((buildFlags & 2) == 1 && (verts[num34 * 4 + 3] & 131072) == 1)
					{
						flag4 = true;
					}
					if (flag4)
					{
						int num35 = num30 - num27;
						int num36 = num31 / this.voxelArea.width - num28 / this.voxelArea.width;
						if ((float)(num35 * num35 + num36 * num36) > num25 * num25)
						{
							int num37 = (num32 >= num29) ? (num32 - num29) : (num32 + num10 - num29);
							if (num37 > 1)
							{
								if (num30 > num27 || (num30 == num27 && num31 > num28))
								{
									num33 = (num29 + num37 / 2) % num10;
								}
								else
								{
									num33 = (num29 + (num37 + 1) / 2) % num10;
								}
							}
						}
					}
					if (num33 != -1)
					{
						simplified.AddRange(new int[4]);
						int num38 = simplified.Count / 4;
						for (int num39 = num38 - 1; num39 > n; num39--)
						{
							simplified[num39 * 4] = simplified[(num39 - 1) * 4];
							simplified[num39 * 4 + 1] = simplified[(num39 - 1) * 4 + 1];
							simplified[num39 * 4 + 2] = simplified[(num39 - 1) * 4 + 2];
							simplified[num39 * 4 + 3] = simplified[(num39 - 1) * 4 + 3];
						}
						simplified[(n + 1) * 4] = verts[num33 * 4];
						simplified[(n + 1) * 4 + 1] = verts[num33 * 4 + 1];
						simplified[(n + 1) * 4 + 2] = verts[num33 * 4 + 2];
						simplified[(n + 1) * 4 + 3] = num33;
					}
					else
					{
						n++;
					}
				}
			}
			for (int num40 = 0; num40 < simplified.Count / 4; num40++)
			{
				int num41 = (simplified[num40 * 4 + 3] + 1) % num10;
				int num42 = simplified[num40 * 4 + 3];
				simplified[num40 * 4 + 3] = ((verts[num41 * 4 + 3] & 65535) | (verts[num42 * 4 + 3] & 65536));
			}
		}

		public void WalkContour(int x, int z, int i, ushort[] flags, global::System.Collections.Generic.List<int> verts)
		{
			int num = 0;
			while ((flags[i] & (ushort)(1 << num)) == 0)
			{
				num++;
			}
			int num2 = num;
			int num3 = i;
			int num4 = this.voxelArea.areaTypes[i];
			int num5 = 0;
			while (num5++ < 40000)
			{
				if ((flags[i] & (ushort)(1 << num)) != 0)
				{
					bool flag = false;
					bool flag2 = false;
					int num6 = x;
					int cornerHeight = this.GetCornerHeight(x, z, i, num, ref flag);
					int num7 = z;
					switch (num)
					{
					case 0:
						num7 += this.voxelArea.width;
						break;
					case 1:
						num6++;
						num7 += this.voxelArea.width;
						break;
					case 2:
						num6++;
						break;
					}
					int num8 = 0;
					global::Pathfinding.Voxels.CompactVoxelSpan compactVoxelSpan = this.voxelArea.compactSpans[i];
					if ((long)compactVoxelSpan.GetConnection(num) != 63L)
					{
						int num9 = x + this.voxelArea.DirectionX[num];
						int num10 = z + this.voxelArea.DirectionZ[num];
						int num11 = (int)(this.voxelArea.compactCells[num9 + num10].index + (uint)compactVoxelSpan.GetConnection(num));
						num8 = this.voxelArea.compactSpans[num11].reg;
						if (num4 != this.voxelArea.areaTypes[num11])
						{
							flag2 = true;
						}
					}
					if (flag)
					{
						num8 |= 65536;
					}
					if (flag2)
					{
						num8 |= 131072;
					}
					verts.Add(num6);
					verts.Add(cornerHeight);
					verts.Add(num7);
					verts.Add(num8);
					flags[i] = (ushort)((int)flags[i] & ~(1 << num));
					num = (num + 1 & 3);
				}
				else
				{
					int num12 = -1;
					int num13 = x + this.voxelArea.DirectionX[num];
					int num14 = z + this.voxelArea.DirectionZ[num];
					global::Pathfinding.Voxels.CompactVoxelSpan compactVoxelSpan2 = this.voxelArea.compactSpans[i];
					if ((long)compactVoxelSpan2.GetConnection(num) != 63L)
					{
						global::Pathfinding.Voxels.CompactVoxelCell compactVoxelCell = this.voxelArea.compactCells[num13 + num14];
						num12 = (int)(compactVoxelCell.index + (uint)compactVoxelSpan2.GetConnection(num));
					}
					if (num12 == -1)
					{
						global::UnityEngine.Debug.LogWarning("Degenerate triangles might have been generated.\nUsually this is not a problem, but if you have a static level, try to modify the graph settings slightly to avoid this edge case.");
						return;
					}
					x = num13;
					z = num14;
					i = num12;
					num = (num + 3 & 3);
				}
				if (num3 == i && num2 == num)
				{
					break;
				}
			}
		}

		public int GetCornerHeight(int x, int z, int i, int dir, ref bool isBorderVertex)
		{
			global::Pathfinding.Voxels.CompactVoxelSpan compactVoxelSpan = this.voxelArea.compactSpans[i];
			int num = (int)compactVoxelSpan.y;
			int num2 = dir + 1 & 3;
			uint[] array = new uint[4];
			array[0] = (uint)(this.voxelArea.compactSpans[i].reg | this.voxelArea.areaTypes[i] << 16);
			if ((long)compactVoxelSpan.GetConnection(dir) != 63L)
			{
				int num3 = x + this.voxelArea.DirectionX[dir];
				int num4 = z + this.voxelArea.DirectionZ[dir];
				int num5 = (int)(this.voxelArea.compactCells[num3 + num4].index + (uint)compactVoxelSpan.GetConnection(dir));
				global::Pathfinding.Voxels.CompactVoxelSpan compactVoxelSpan2 = this.voxelArea.compactSpans[num5];
				num = global::System.Math.Max(num, (int)compactVoxelSpan2.y);
				array[1] = (uint)(compactVoxelSpan2.reg | this.voxelArea.areaTypes[num5] << 16);
				if ((long)compactVoxelSpan2.GetConnection(num2) != 63L)
				{
					int num6 = num3 + this.voxelArea.DirectionX[num2];
					int num7 = num4 + this.voxelArea.DirectionZ[num2];
					int num8 = (int)(this.voxelArea.compactCells[num6 + num7].index + (uint)compactVoxelSpan2.GetConnection(num2));
					global::Pathfinding.Voxels.CompactVoxelSpan compactVoxelSpan3 = this.voxelArea.compactSpans[num8];
					num = global::System.Math.Max(num, (int)compactVoxelSpan3.y);
					array[2] = (uint)(compactVoxelSpan3.reg | this.voxelArea.areaTypes[num8] << 16);
				}
			}
			if ((long)compactVoxelSpan.GetConnection(num2) != 63L)
			{
				int num9 = x + this.voxelArea.DirectionX[num2];
				int num10 = z + this.voxelArea.DirectionZ[num2];
				int num11 = (int)(this.voxelArea.compactCells[num9 + num10].index + (uint)compactVoxelSpan.GetConnection(num2));
				global::Pathfinding.Voxels.CompactVoxelSpan compactVoxelSpan4 = this.voxelArea.compactSpans[num11];
				num = global::System.Math.Max(num, (int)compactVoxelSpan4.y);
				array[3] = (uint)(compactVoxelSpan4.reg | this.voxelArea.areaTypes[num11] << 16);
				if ((long)compactVoxelSpan4.GetConnection(dir) != 63L)
				{
					int num12 = num9 + this.voxelArea.DirectionX[dir];
					int num13 = num10 + this.voxelArea.DirectionZ[dir];
					int num14 = (int)(this.voxelArea.compactCells[num12 + num13].index + (uint)compactVoxelSpan4.GetConnection(dir));
					global::Pathfinding.Voxels.CompactVoxelSpan compactVoxelSpan5 = this.voxelArea.compactSpans[num14];
					num = global::System.Math.Max(num, (int)compactVoxelSpan5.y);
					array[2] = (uint)(compactVoxelSpan5.reg | this.voxelArea.areaTypes[num14] << 16);
				}
			}
			for (int j = 0; j < 4; j++)
			{
				int num15 = j;
				int num16 = j + 1 & 3;
				int num17 = j + 2 & 3;
				int num18 = j + 3 & 3;
				bool flag = (array[num15] & array[num16] & 32768U) != 0U && array[num15] == array[num16];
				bool flag2 = ((array[num17] | array[num18]) & 32768U) == 0U;
				bool flag3 = array[num17] >> 16 == array[num18] >> 16;
				bool flag4 = array[num15] != 0U && array[num16] != 0U && array[num17] != 0U && array[num18] != 0U;
				if (flag && flag2 && flag3 && flag4)
				{
					isBorderVertex = true;
					break;
				}
			}
			return num;
		}

		public void RemoveDegenerateSegments(global::System.Collections.Generic.List<int> simplified)
		{
			for (int i = 0; i < simplified.Count / 4; i++)
			{
				int num = i + 1;
				if (num >= simplified.Count / 4)
				{
					num = 0;
				}
				if (simplified[i * 4] == simplified[num * 4] && simplified[i * 4 + 2] == simplified[num * 4 + 2])
				{
					simplified.RemoveRange(i, 4);
				}
			}
		}

		public int CalcAreaOfPolygon2D(int[] verts, int nverts)
		{
			int num = 0;
			int i = 0;
			int num2 = nverts - 1;
			while (i < nverts)
			{
				int num3 = i * 4;
				int num4 = num2 * 4;
				num += verts[num3] * (verts[num4 + 2] / this.voxelArea.width) - verts[num4] * (verts[num3 + 2] / this.voxelArea.width);
				num2 = i++;
			}
			return (num + 1) / 2;
		}

		public static bool Ileft(int a, int b, int c, int[] va, int[] vb, int[] vc)
		{
			return (vb[b] - va[a]) * (vc[c + 2] - va[a + 2]) - (vc[c] - va[a]) * (vb[b + 2] - va[a + 2]) <= 0;
		}

		public static bool Diagonal(int i, int j, int n, int[] verts, int[] indices)
		{
			return global::Pathfinding.Voxels.Voxelize.InCone(i, j, n, verts, indices) && global::Pathfinding.Voxels.Voxelize.Diagonalie(i, j, n, verts, indices);
		}

		public static bool InCone(int i, int j, int n, int[] verts, int[] indices)
		{
			int num = (indices[i] & 268435455) * 4;
			int num2 = (indices[j] & 268435455) * 4;
			int c = (indices[global::Pathfinding.Voxels.Voxelize.Next(i, n)] & 268435455) * 4;
			int num3 = (indices[global::Pathfinding.Voxels.Voxelize.Prev(i, n)] & 268435455) * 4;
			if (global::Pathfinding.Voxels.Voxelize.LeftOn(num3, num, c, verts))
			{
				return global::Pathfinding.Voxels.Voxelize.Left(num, num2, num3, verts) && global::Pathfinding.Voxels.Voxelize.Left(num2, num, c, verts);
			}
			return !global::Pathfinding.Voxels.Voxelize.LeftOn(num, num2, c, verts) || !global::Pathfinding.Voxels.Voxelize.LeftOn(num2, num, num3, verts);
		}

		public static bool Left(int a, int b, int c, int[] verts)
		{
			return global::Pathfinding.Voxels.Voxelize.Area2(a, b, c, verts) < 0;
		}

		public static bool LeftOn(int a, int b, int c, int[] verts)
		{
			return global::Pathfinding.Voxels.Voxelize.Area2(a, b, c, verts) <= 0;
		}

		public static bool Collinear(int a, int b, int c, int[] verts)
		{
			return global::Pathfinding.Voxels.Voxelize.Area2(a, b, c, verts) == 0;
		}

		public static int Area2(int a, int b, int c, int[] verts)
		{
			return (verts[b] - verts[a]) * (verts[c + 2] - verts[a + 2]) - (verts[c] - verts[a]) * (verts[b + 2] - verts[a + 2]);
		}

		private static bool Diagonalie(int i, int j, int n, int[] verts, int[] indices)
		{
			int a = (indices[i] & 268435455) * 4;
			int num = (indices[j] & 268435455) * 4;
			for (int k = 0; k < n; k++)
			{
				int num2 = global::Pathfinding.Voxels.Voxelize.Next(k, n);
				if (k != i && num2 != i && k != j && num2 != j)
				{
					int num3 = (indices[k] & 268435455) * 4;
					int num4 = (indices[num2] & 268435455) * 4;
					if (!global::Pathfinding.Voxels.Voxelize.Vequal(a, num3, verts) && !global::Pathfinding.Voxels.Voxelize.Vequal(num, num3, verts) && !global::Pathfinding.Voxels.Voxelize.Vequal(a, num4, verts) && !global::Pathfinding.Voxels.Voxelize.Vequal(num, num4, verts))
					{
						if (global::Pathfinding.Voxels.Voxelize.Intersect(a, num, num3, num4, verts))
						{
							return false;
						}
					}
				}
			}
			return true;
		}

		public static bool Xorb(bool x, bool y)
		{
			return !x ^ !y;
		}

		public static bool IntersectProp(int a, int b, int c, int d, int[] verts)
		{
			return !global::Pathfinding.Voxels.Voxelize.Collinear(a, b, c, verts) && !global::Pathfinding.Voxels.Voxelize.Collinear(a, b, d, verts) && !global::Pathfinding.Voxels.Voxelize.Collinear(c, d, a, verts) && !global::Pathfinding.Voxels.Voxelize.Collinear(c, d, b, verts) && global::Pathfinding.Voxels.Voxelize.Xorb(global::Pathfinding.Voxels.Voxelize.Left(a, b, c, verts), global::Pathfinding.Voxels.Voxelize.Left(a, b, d, verts)) && global::Pathfinding.Voxels.Voxelize.Xorb(global::Pathfinding.Voxels.Voxelize.Left(c, d, a, verts), global::Pathfinding.Voxels.Voxelize.Left(c, d, b, verts));
		}

		private static bool Between(int a, int b, int c, int[] verts)
		{
			if (!global::Pathfinding.Voxels.Voxelize.Collinear(a, b, c, verts))
			{
				return false;
			}
			if (verts[a] != verts[b])
			{
				return (verts[a] <= verts[c] && verts[c] <= verts[b]) || (verts[a] >= verts[c] && verts[c] >= verts[b]);
			}
			return (verts[a + 2] <= verts[c + 2] && verts[c + 2] <= verts[b + 2]) || (verts[a + 2] >= verts[c + 2] && verts[c + 2] >= verts[b + 2]);
		}

		private static bool Intersect(int a, int b, int c, int d, int[] verts)
		{
			return global::Pathfinding.Voxels.Voxelize.IntersectProp(a, b, c, d, verts) || (global::Pathfinding.Voxels.Voxelize.Between(a, b, c, verts) || global::Pathfinding.Voxels.Voxelize.Between(a, b, d, verts) || global::Pathfinding.Voxels.Voxelize.Between(c, d, a, verts) || global::Pathfinding.Voxels.Voxelize.Between(c, d, b, verts));
		}

		private static bool Vequal(int a, int b, int[] verts)
		{
			return verts[a] == verts[b] && verts[a + 2] == verts[b + 2];
		}

		public static int Prev(int i, int n)
		{
			return (i - 1 < 0) ? (n - 1) : (i - 1);
		}

		public static int Next(int i, int n)
		{
			return (i + 1 >= n) ? 0 : (i + 1);
		}

		public void BuildPolyMesh(global::Pathfinding.Voxels.VoxelContourSet cset, int nvp, out global::Pathfinding.Voxels.VoxelMesh mesh)
		{
			nvp = 3;
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			for (int i = 0; i < cset.conts.Count; i++)
			{
				if (cset.conts[i].nverts >= 3)
				{
					num += cset.conts[i].nverts;
					num2 += cset.conts[i].nverts - 2;
					num3 = global::System.Math.Max(num3, cset.conts[i].nverts);
				}
			}
			if (num >= 65534)
			{
				global::UnityEngine.Debug.LogWarning("To many vertices for unity to render - Unity might screw up rendering, but hopefully the navmesh will work ok");
			}
			global::Pathfinding.Int3[] array = new global::Pathfinding.Int3[num];
			int[] array2 = new int[num2 * nvp];
			int[] array3 = new int[num2];
			global::Pathfinding.Util.Memory.MemSet<int>(array2, 255, 4);
			int[] array4 = new int[num3];
			int[] array5 = new int[num3 * 3];
			int num4 = 0;
			int num5 = 0;
			int num6 = 0;
			for (int j = 0; j < cset.conts.Count; j++)
			{
				global::Pathfinding.Voxels.VoxelContour voxelContour = cset.conts[j];
				if (voxelContour.nverts >= 3)
				{
					for (int k = 0; k < voxelContour.nverts; k++)
					{
						array4[k] = k;
						voxelContour.verts[k * 4 + 2] /= this.voxelArea.width;
					}
					int num7 = this.Triangulate(voxelContour.nverts, voxelContour.verts, ref array4, ref array5);
					int num8 = num4;
					for (int l = 0; l < num7 * 3; l++)
					{
						array2[num5] = array5[l] + num8;
						num5++;
					}
					for (int m = 0; m < num7; m++)
					{
						array3[num6] = voxelContour.area;
						num6++;
					}
					for (int n = 0; n < voxelContour.nverts; n++)
					{
						array[num4] = new global::Pathfinding.Int3(voxelContour.verts[n * 4], voxelContour.verts[n * 4 + 1], voxelContour.verts[n * 4 + 2]);
						num4++;
					}
				}
			}
			mesh = default(global::Pathfinding.Voxels.VoxelMesh);
			global::Pathfinding.Int3[] array6 = new global::Pathfinding.Int3[num4];
			for (int num9 = 0; num9 < num4; num9++)
			{
				array6[num9] = array[num9];
			}
			int[] array7 = new int[num5];
			int[] array8 = new int[num6];
			global::System.Buffer.BlockCopy(array2, 0, array7, 0, num5 * 4);
			global::System.Buffer.BlockCopy(array3, 0, array8, 0, num6 * 4);
			mesh.verts = array6;
			mesh.tris = array7;
			mesh.areas = array8;
		}

		private int Triangulate(int n, int[] verts, ref int[] indices, ref int[] tris)
		{
			int num = 0;
			int[] array = tris;
			int num2 = 0;
			for (int i = 0; i < n; i++)
			{
				int num3 = global::Pathfinding.Voxels.Voxelize.Next(i, n);
				int j = global::Pathfinding.Voxels.Voxelize.Next(num3, n);
				if (global::Pathfinding.Voxels.Voxelize.Diagonal(i, j, n, verts, indices))
				{
					indices[num3] |= 1073741824;
				}
			}
			while (n > 3)
			{
				int num4 = -1;
				int num5 = -1;
				for (int k = 0; k < n; k++)
				{
					int num6 = global::Pathfinding.Voxels.Voxelize.Next(k, n);
					if ((indices[num6] & 1073741824) != 0)
					{
						int num7 = (indices[k] & 268435455) * 4;
						int num8 = (indices[global::Pathfinding.Voxels.Voxelize.Next(num6, n)] & 268435455) * 4;
						int num9 = verts[num8] - verts[num7];
						int num10 = verts[num8 + 2] - verts[num7 + 2];
						int num11 = num9 * num9 + num10 * num10;
						if (num4 < 0 || num11 < num4)
						{
							num4 = num11;
							num5 = k;
						}
					}
				}
				if (num5 == -1)
				{
					global::UnityEngine.Debug.LogWarning("Degenerate triangles might have been generated.\nUsually this is not a problem, but if you have a static level, try to modify the graph settings slightly to avoid this edge case.");
					return -num;
				}
				int num12 = num5;
				int num13 = global::Pathfinding.Voxels.Voxelize.Next(num12, n);
				int num14 = global::Pathfinding.Voxels.Voxelize.Next(num13, n);
				array[num2] = (indices[num12] & 268435455);
				num2++;
				array[num2] = (indices[num13] & 268435455);
				num2++;
				array[num2] = (indices[num14] & 268435455);
				num2++;
				num++;
				n--;
				for (int l = num13; l < n; l++)
				{
					indices[l] = indices[l + 1];
				}
				if (num13 >= n)
				{
					num13 = 0;
				}
				num12 = global::Pathfinding.Voxels.Voxelize.Prev(num13, n);
				if (global::Pathfinding.Voxels.Voxelize.Diagonal(global::Pathfinding.Voxels.Voxelize.Prev(num12, n), num13, n, verts, indices))
				{
					indices[num12] |= 1073741824;
				}
				else
				{
					indices[num12] &= 268435455;
				}
				if (global::Pathfinding.Voxels.Voxelize.Diagonal(num12, global::Pathfinding.Voxels.Voxelize.Next(num13, n), n, verts, indices))
				{
					indices[num13] |= 1073741824;
				}
				else
				{
					indices[num13] &= 268435455;
				}
			}
			array[num2] = (indices[0] & 268435455);
			num2++;
			array[num2] = (indices[1] & 268435455);
			num2++;
			array[num2] = (indices[2] & 268435455);
			num2++;
			return num + 1;
		}

		public global::UnityEngine.Vector3 CompactSpanToVector(int x, int z, int i)
		{
			return this.voxelOffset + new global::UnityEngine.Vector3(((float)x + 0.5f) * this.cellSize, (float)this.voxelArea.compactSpans[i].y * this.cellHeight, ((float)z + 0.5f) * this.cellSize);
		}

		public void VectorToIndex(global::UnityEngine.Vector3 p, out int x, out int z)
		{
			p -= this.voxelOffset;
			x = global::UnityEngine.Mathf.RoundToInt(p.x / this.cellSize - 0.5f);
			z = global::UnityEngine.Mathf.RoundToInt(p.z / this.cellSize - 0.5f);
		}

		public void Init()
		{
			if (this.voxelArea == null || this.voxelArea.width != this.width || this.voxelArea.depth != this.depth)
			{
				this.voxelArea = new global::Pathfinding.Voxels.VoxelArea(this.width, this.depth);
			}
			else
			{
				this.voxelArea.Reset();
			}
		}

		public void VoxelizeInput()
		{
			global::UnityEngine.Vector3 min = this.forcedBounds.min;
			this.voxelOffset = min;
			int num = (int)(this.forcedBounds.size.y / this.cellHeight);
			float num2 = 1f / this.cellSize;
			float num3 = 1f / this.cellHeight;
			float num4 = global::UnityEngine.Mathf.Cos(global::UnityEngine.Mathf.Atan(global::UnityEngine.Mathf.Tan(this.maxSlope * 0.0174532924f) * (num3 * this.cellSize)));
			float[] array = new float[9];
			float[] array2 = new float[21];
			float[] array3 = new float[21];
			float[] array4 = new float[21];
			float[] array5 = new float[21];
			if (this.inputMeshes == null)
			{
				throw new global::System.NullReferenceException("inputMeshes not set");
			}
			int num5 = 0;
			for (int i = 0; i < this.inputMeshes.Count; i++)
			{
				num5 = global::System.Math.Max(this.inputMeshes[i].vertices.Length, num5);
			}
			global::UnityEngine.Vector3[] array6 = new global::UnityEngine.Vector3[num5];
			global::UnityEngine.Matrix4x4 lhs = global::UnityEngine.Matrix4x4.TRS(-new global::UnityEngine.Vector3(0.5f, 0f, 0.5f), global::UnityEngine.Quaternion.identity, global::UnityEngine.Vector3.one) * global::UnityEngine.Matrix4x4.Scale(new global::UnityEngine.Vector3(num2, num3, num2)) * global::UnityEngine.Matrix4x4.TRS(-min, global::UnityEngine.Quaternion.identity, global::UnityEngine.Vector3.one);
			for (int j = 0; j < this.inputMeshes.Count; j++)
			{
				global::Pathfinding.Voxels.RasterizationMesh rasterizationMesh = this.inputMeshes[j];
				global::UnityEngine.Matrix4x4 matrix4x = rasterizationMesh.matrix;
				matrix4x = lhs * matrix4x;
				bool flag = global::Pathfinding.VectorMath.ReversesFaceOrientations(matrix4x);
				global::UnityEngine.Vector3[] vertices = rasterizationMesh.vertices;
				int[] triangles = rasterizationMesh.triangles;
				int num6 = triangles.Length;
				for (int k = 0; k < vertices.Length; k++)
				{
					array6[k] = matrix4x.MultiplyPoint3x4(vertices[k]);
				}
				int area = rasterizationMesh.area;
				for (int l = 0; l < num6; l += 3)
				{
					global::UnityEngine.Vector3 vector = array6[triangles[l]];
					global::UnityEngine.Vector3 vector2 = array6[triangles[l + 1]];
					global::UnityEngine.Vector3 vector3 = array6[triangles[l + 2]];
					if (flag)
					{
						global::UnityEngine.Vector3 vector4 = vector;
						vector = vector3;
						vector3 = vector4;
					}
					int num7 = (int)global::Pathfinding.Voxels.Utility.Min(vector.x, vector2.x, vector3.x);
					int num8 = (int)global::Pathfinding.Voxels.Utility.Min(vector.z, vector2.z, vector3.z);
					int num9 = (int)global::System.Math.Ceiling((double)global::Pathfinding.Voxels.Utility.Max(vector.x, vector2.x, vector3.x));
					int num10 = (int)global::System.Math.Ceiling((double)global::Pathfinding.Voxels.Utility.Max(vector.z, vector2.z, vector3.z));
					num7 = global::UnityEngine.Mathf.Clamp(num7, 0, this.voxelArea.width - 1);
					num9 = global::UnityEngine.Mathf.Clamp(num9, 0, this.voxelArea.width - 1);
					num8 = global::UnityEngine.Mathf.Clamp(num8, 0, this.voxelArea.depth - 1);
					num10 = global::UnityEngine.Mathf.Clamp(num10, 0, this.voxelArea.depth - 1);
					if (num7 < this.voxelArea.width && num8 < this.voxelArea.depth && num9 > 0 && num10 > 0)
					{
						float num11 = global::UnityEngine.Vector3.Dot(global::UnityEngine.Vector3.Cross(vector2 - vector, vector3 - vector).normalized, global::UnityEngine.Vector3.up);
						int area2;
						if (num11 < num4)
						{
							area2 = 0;
						}
						else
						{
							area2 = 1 + area;
						}
						global::Pathfinding.Voxels.Utility.CopyVector(array, 0, vector);
						global::Pathfinding.Voxels.Utility.CopyVector(array, 3, vector2);
						global::Pathfinding.Voxels.Utility.CopyVector(array, 6, vector3);
						for (int m = num7; m <= num9; m++)
						{
							int num12 = this.clipper.ClipPolygon(array, 3, array2, 1f, (float)(-(float)m) + 0.5f, 0);
							if (num12 >= 3)
							{
								num12 = this.clipper.ClipPolygon(array2, num12, array3, -1f, (float)m + 0.5f, 0);
								if (num12 >= 3)
								{
									float num13 = array3[2];
									float num14 = array3[2];
									for (int n = 1; n < num12; n++)
									{
										float val = array3[n * 3 + 2];
										num13 = global::System.Math.Min(num13, val);
										num14 = global::System.Math.Max(num14, val);
									}
									int num15 = global::UnityEngine.Mathf.Clamp((int)global::System.Math.Round((double)num13), 0, this.voxelArea.depth - 1);
									int num16 = global::UnityEngine.Mathf.Clamp((int)global::System.Math.Round((double)num14), 0, this.voxelArea.depth - 1);
									for (int num17 = num15; num17 <= num16; num17++)
									{
										int num18 = this.clipper.ClipPolygon(array3, num12, array4, 1f, (float)(-(float)num17) + 0.5f, 2);
										if (num18 >= 3)
										{
											num18 = this.clipper.ClipPolygonY(array4, num18, array5, -1f, (float)num17 + 0.5f, 2);
											if (num18 >= 3)
											{
												float num19 = array5[1];
												float num20 = array5[1];
												for (int num21 = 1; num21 < num18; num21++)
												{
													float val2 = array5[num21 * 3 + 1];
													num19 = global::System.Math.Min(num19, val2);
													num20 = global::System.Math.Max(num20, val2);
												}
												int num22 = (int)global::System.Math.Ceiling((double)num20);
												if (num22 >= 0 && num19 <= (float)num)
												{
													int num23 = global::System.Math.Max(0, (int)num19);
													num22 = global::System.Math.Max(num23 + 1, num22);
													this.voxelArea.AddLinkedSpan(num17 * this.voxelArea.width + m, (uint)num23, (uint)num22, area2, this.voxelWalkableClimb);
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
		}

		public void DebugDrawSpans()
		{
			int num = this.voxelArea.width * this.voxelArea.depth;
			global::UnityEngine.Vector3 min = this.forcedBounds.min;
			global::Pathfinding.Voxels.LinkedVoxelSpan[] linkedSpans = this.voxelArea.linkedSpans;
			int i = 0;
			int num2 = 0;
			while (i < num)
			{
				for (int j = 0; j < this.voxelArea.width; j++)
				{
					int num3 = i + j;
					while (num3 != -1 && linkedSpans[num3].bottom != 4294967295U)
					{
						uint top = linkedSpans[num3].top;
						uint num4 = (linkedSpans[num3].next == -1) ? 65536U : linkedSpans[linkedSpans[num3].next].bottom;
						if (top > num4)
						{
							global::UnityEngine.Debug.Log(top + " " + num4);
							global::UnityEngine.Debug.DrawLine(new global::UnityEngine.Vector3((float)j * this.cellSize, top * this.cellHeight, (float)num2 * this.cellSize) + min, new global::UnityEngine.Vector3((float)j * this.cellSize, num4 * this.cellHeight, (float)num2 * this.cellSize) + min, global::UnityEngine.Color.yellow, 1f);
						}
						if (num4 - top < this.voxelWalkableHeight)
						{
						}
						num3 = linkedSpans[num3].next;
					}
				}
				i += this.voxelArea.width;
				num2++;
			}
		}

		public void DebugDrawCompactSpans()
		{
			int num = this.voxelArea.compactSpans.Length;
			global::UnityEngine.Vector3[] array = new global::UnityEngine.Vector3[num];
			global::UnityEngine.Vector3[] array2 = new global::UnityEngine.Vector3[num];
			global::UnityEngine.Color[] array3 = new global::UnityEngine.Color[num];
			int num2 = 0;
			int num3 = this.voxelArea.width * this.voxelArea.depth;
			global::UnityEngine.Vector3 min = this.forcedBounds.min;
			int i = 0;
			int num4 = 0;
			while (i < num3)
			{
				for (int j = 0; j < this.voxelArea.width; j++)
				{
					global::UnityEngine.Vector3 vector = new global::UnityEngine.Vector3((float)j, 0f, (float)num4) * this.cellSize + min;
					global::Pathfinding.Voxels.CompactVoxelCell compactVoxelCell = this.voxelArea.compactCells[j + i];
					int k = (int)compactVoxelCell.index;
					int num5 = (int)(compactVoxelCell.index + compactVoxelCell.count);
					while (k < num5)
					{
						global::Pathfinding.Voxels.CompactVoxelSpan compactVoxelSpan = this.voxelArea.compactSpans[k];
						vector.y = ((float)compactVoxelSpan.y + 0.1f) * this.cellHeight + min.y;
						array[num2] = vector;
						vector.y = (float)compactVoxelSpan.y * this.cellHeight + min.y;
						array2[num2] = vector;
						global::UnityEngine.Color color = global::UnityEngine.Color.black;
						switch (compactVoxelSpan.reg)
						{
						case 0:
							color = global::UnityEngine.Color.red;
							break;
						case 1:
							color = global::UnityEngine.Color.green;
							break;
						case 2:
							color = global::UnityEngine.Color.yellow;
							break;
						case 3:
							color = global::UnityEngine.Color.magenta;
							break;
						}
						array3[num2] = color;
						num2++;
						k++;
					}
				}
				i += this.voxelArea.width;
				num4++;
			}
			global::Pathfinding.DebugUtility.DrawCubes(array, array2, array3, this.cellSize);
		}

		public void BuildCompactField()
		{
			int spanCount = this.voxelArea.GetSpanCount();
			this.voxelArea.compactSpanCount = spanCount;
			if (this.voxelArea.compactSpans == null || this.voxelArea.compactSpans.Length < spanCount)
			{
				this.voxelArea.compactSpans = new global::Pathfinding.Voxels.CompactVoxelSpan[spanCount];
				this.voxelArea.areaTypes = new int[spanCount];
			}
			uint num = 0U;
			int num2 = this.voxelArea.width;
			int num3 = this.voxelArea.depth;
			int num4 = num2 * num3;
			if (this.voxelWalkableHeight >= 65535U)
			{
				global::UnityEngine.Debug.LogWarning("Too high walkable height to guarantee correctness. Increase voxel height or lower walkable height.");
			}
			global::Pathfinding.Voxels.LinkedVoxelSpan[] linkedSpans = this.voxelArea.linkedSpans;
			int i = 0;
			int num5 = 0;
			while (i < num4)
			{
				for (int j = 0; j < num2; j++)
				{
					int num6 = j + i;
					if (linkedSpans[num6].bottom == 4294967295U)
					{
						this.voxelArea.compactCells[j + i] = new global::Pathfinding.Voxels.CompactVoxelCell(0U, 0U);
					}
					else
					{
						uint i2 = num;
						uint num7 = 0U;
						while (num6 != -1)
						{
							if (linkedSpans[num6].area != 0)
							{
								int top = (int)linkedSpans[num6].top;
								int next = linkedSpans[num6].next;
								int num8 = (int)((next == -1) ? 65536U : linkedSpans[next].bottom);
								this.voxelArea.compactSpans[(int)((global::System.UIntPtr)num)] = new global::Pathfinding.Voxels.CompactVoxelSpan((ushort)((top <= 65535) ? top : 65535), (uint)((num8 - top <= 65535) ? (num8 - top) : 65535));
								this.voxelArea.areaTypes[(int)((global::System.UIntPtr)num)] = linkedSpans[num6].area;
								num += 1U;
								num7 += 1U;
							}
							num6 = linkedSpans[num6].next;
						}
						this.voxelArea.compactCells[j + i] = new global::Pathfinding.Voxels.CompactVoxelCell(i2, num7);
					}
				}
				i += num2;
				num5++;
			}
		}

		public void BuildVoxelConnections()
		{
			int num = this.voxelArea.width * this.voxelArea.depth;
			global::Pathfinding.Voxels.CompactVoxelSpan[] compactSpans = this.voxelArea.compactSpans;
			global::Pathfinding.Voxels.CompactVoxelCell[] compactCells = this.voxelArea.compactCells;
			int i = 0;
			int num2 = 0;
			while (i < num)
			{
				for (int j = 0; j < this.voxelArea.width; j++)
				{
					global::Pathfinding.Voxels.CompactVoxelCell compactVoxelCell = compactCells[j + i];
					int k = (int)compactVoxelCell.index;
					int num3 = (int)(compactVoxelCell.index + compactVoxelCell.count);
					while (k < num3)
					{
						global::Pathfinding.Voxels.CompactVoxelSpan compactVoxelSpan = compactSpans[k];
						compactSpans[k].con = uint.MaxValue;
						for (int l = 0; l < 4; l++)
						{
							int num4 = j + this.voxelArea.DirectionX[l];
							int num5 = i + this.voxelArea.DirectionZ[l];
							if (num4 >= 0 && num5 >= 0 && num5 < num && num4 < this.voxelArea.width)
							{
								global::Pathfinding.Voxels.CompactVoxelCell compactVoxelCell2 = compactCells[num4 + num5];
								int m = (int)compactVoxelCell2.index;
								int num6 = (int)(compactVoxelCell2.index + compactVoxelCell2.count);
								while (m < num6)
								{
									global::Pathfinding.Voxels.CompactVoxelSpan compactVoxelSpan2 = compactSpans[m];
									int num7 = (int)global::System.Math.Max(compactVoxelSpan.y, compactVoxelSpan2.y);
									int num8 = global::System.Math.Min((int)((uint)compactVoxelSpan.y + compactVoxelSpan.h), (int)((uint)compactVoxelSpan2.y + compactVoxelSpan2.h));
									if ((long)(num8 - num7) >= (long)((ulong)this.voxelWalkableHeight) && global::System.Math.Abs((int)(compactVoxelSpan2.y - compactVoxelSpan.y)) <= this.voxelWalkableClimb)
									{
										uint num9 = (uint)(m - (int)compactVoxelCell2.index);
										if (num9 <= 65535U)
										{
											compactSpans[k].SetConnection(l, num9);
											break;
										}
										global::UnityEngine.Debug.LogError("Too many layers");
									}
									m++;
								}
							}
						}
						k++;
					}
				}
				i += this.voxelArea.width;
				num2++;
			}
		}

		private void DrawLine(int a, int b, int[] indices, int[] verts, global::UnityEngine.Color col)
		{
			int num = (indices[a] & 268435455) * 4;
			int num2 = (indices[b] & 268435455) * 4;
			global::UnityEngine.Debug.DrawLine(this.VoxelToWorld(verts[num], verts[num + 1], verts[num + 2]), this.VoxelToWorld(verts[num2], verts[num2 + 1], verts[num2 + 2]), col);
		}

		public global::UnityEngine.Vector3 VoxelToWorld(int x, int y, int z)
		{
			return global::UnityEngine.Vector3.Scale(new global::UnityEngine.Vector3((float)x, (float)y, (float)z), this.cellScale) + this.voxelOffset;
		}

		public global::Pathfinding.Int3 VoxelToWorldInt3(global::Pathfinding.Int3 voxelPosition)
		{
			global::Pathfinding.Int3 lhs = voxelPosition * 1000;
			lhs = new global::Pathfinding.Int3(global::UnityEngine.Mathf.RoundToInt((float)lhs.x * this.cellScale.x), global::UnityEngine.Mathf.RoundToInt((float)lhs.y * this.cellScale.y), global::UnityEngine.Mathf.RoundToInt((float)lhs.z * this.cellScale.z));
			return lhs + (global::Pathfinding.Int3)this.voxelOffset;
		}

		private global::UnityEngine.Vector3 ConvertPosWithoutOffset(int x, int y, int z)
		{
			return global::UnityEngine.Vector3.Scale(new global::UnityEngine.Vector3((float)x, (float)y, (float)z / (float)this.voxelArea.width), this.cellScale) + this.voxelOffset;
		}

		private global::UnityEngine.Vector3 ConvertPosition(int x, int z, int i)
		{
			global::Pathfinding.Voxels.CompactVoxelSpan compactVoxelSpan = this.voxelArea.compactSpans[i];
			return new global::UnityEngine.Vector3((float)x * this.cellSize, (float)compactVoxelSpan.y * this.cellHeight, (float)z / (float)this.voxelArea.width * this.cellSize) + this.voxelOffset;
		}

		public void ErodeWalkableArea(int radius)
		{
			ushort[] array = this.voxelArea.tmpUShortArr;
			if (array == null || array.Length < this.voxelArea.compactSpanCount)
			{
				array = (this.voxelArea.tmpUShortArr = new ushort[this.voxelArea.compactSpanCount]);
			}
			global::Pathfinding.Util.Memory.MemSet<ushort>(array, ushort.MaxValue, 2);
			this.CalculateDistanceField(array);
			for (int i = 0; i < array.Length; i++)
			{
				if ((int)array[i] < radius * 2)
				{
					this.voxelArea.areaTypes[i] = 0;
				}
			}
		}

		public void BuildDistanceField()
		{
			ushort[] array = this.voxelArea.tmpUShortArr;
			if (array == null || array.Length < this.voxelArea.compactSpanCount)
			{
				array = (this.voxelArea.tmpUShortArr = new ushort[this.voxelArea.compactSpanCount]);
			}
			global::Pathfinding.Util.Memory.MemSet<ushort>(array, ushort.MaxValue, 2);
			this.voxelArea.maxDistance = this.CalculateDistanceField(array);
			ushort[] array2 = this.voxelArea.dist;
			if (array2 == null || array2.Length < this.voxelArea.compactSpanCount)
			{
				array2 = new ushort[this.voxelArea.compactSpanCount];
			}
			array2 = this.BoxBlur(array, array2);
			this.voxelArea.dist = array2;
		}

		[global::System.Obsolete("This function is not complete and should not be used")]
		public void ErodeVoxels(int radius)
		{
			if (radius > 255)
			{
				global::UnityEngine.Debug.LogError("Max Erode Radius is 255");
				radius = 255;
			}
			int num = this.voxelArea.width * this.voxelArea.depth;
			int[] array = new int[this.voxelArea.compactSpanCount];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = 255;
			}
			for (int j = 0; j < num; j += this.voxelArea.width)
			{
				for (int k = 0; k < this.voxelArea.width; k++)
				{
					global::Pathfinding.Voxels.CompactVoxelCell compactVoxelCell = this.voxelArea.compactCells[k + j];
					int l = (int)compactVoxelCell.index;
					int num2 = (int)(compactVoxelCell.index + compactVoxelCell.count);
					while (l < num2)
					{
						if (this.voxelArea.areaTypes[l] != 0)
						{
							global::Pathfinding.Voxels.CompactVoxelSpan compactVoxelSpan = this.voxelArea.compactSpans[l];
							int num3 = 0;
							for (int m = 0; m < 4; m++)
							{
								if ((long)compactVoxelSpan.GetConnection(m) != 63L)
								{
									num3++;
								}
							}
							if (num3 != 4)
							{
								array[l] = 0;
							}
						}
						l++;
					}
				}
			}
		}

		public void FilterLowHeightSpans(uint voxelWalkableHeight, float cs, float ch, global::UnityEngine.Vector3 min)
		{
			int num = this.voxelArea.width * this.voxelArea.depth;
			global::Pathfinding.Voxels.LinkedVoxelSpan[] linkedSpans = this.voxelArea.linkedSpans;
			int i = 0;
			int num2 = 0;
			while (i < num)
			{
				for (int j = 0; j < this.voxelArea.width; j++)
				{
					int num3 = i + j;
					while (num3 != -1 && linkedSpans[num3].bottom != 4294967295U)
					{
						uint top = linkedSpans[num3].top;
						uint num4 = (linkedSpans[num3].next == -1) ? 65536U : linkedSpans[linkedSpans[num3].next].bottom;
						if (num4 - top < voxelWalkableHeight)
						{
							linkedSpans[num3].area = 0;
						}
						num3 = linkedSpans[num3].next;
					}
				}
				i += this.voxelArea.width;
				num2++;
			}
		}

		public void FilterLedges(uint voxelWalkableHeight, int voxelWalkableClimb, float cs, float ch, global::UnityEngine.Vector3 min)
		{
			int num = this.voxelArea.width * this.voxelArea.depth;
			global::Pathfinding.Voxels.LinkedVoxelSpan[] linkedSpans = this.voxelArea.linkedSpans;
			int[] directionX = this.voxelArea.DirectionX;
			int[] directionZ = this.voxelArea.DirectionZ;
			int num2 = this.voxelArea.width;
			int i = 0;
			int num3 = 0;
			while (i < num)
			{
				for (int j = 0; j < num2; j++)
				{
					if (linkedSpans[j + i].bottom != 4294967295U)
					{
						for (int num4 = j + i; num4 != -1; num4 = linkedSpans[num4].next)
						{
							if (linkedSpans[num4].area != 0)
							{
								int top = (int)linkedSpans[num4].top;
								int val = (int)((linkedSpans[num4].next == -1) ? 65536U : linkedSpans[linkedSpans[num4].next].bottom);
								int num5 = 65536;
								int num6 = (int)linkedSpans[num4].top;
								int num7 = num6;
								for (int k = 0; k < 4; k++)
								{
									int num8 = j + directionX[k];
									int num9 = i + directionZ[k];
									if (num8 < 0 || num9 < 0 || num9 >= num || num8 >= num2)
									{
										linkedSpans[num4].area = 0;
										break;
									}
									int num10 = num8 + num9;
									int num11 = -voxelWalkableClimb;
									int val2 = (int)((linkedSpans[num10].bottom == uint.MaxValue) ? 65536U : linkedSpans[num10].bottom);
									if ((long)(global::System.Math.Min(val, val2) - global::System.Math.Max(top, num11)) > (long)((ulong)voxelWalkableHeight))
									{
										num5 = global::System.Math.Min(num5, num11 - top);
									}
									if (linkedSpans[num10].bottom != 4294967295U)
									{
										for (int num12 = num10; num12 != -1; num12 = linkedSpans[num12].next)
										{
											num11 = (int)linkedSpans[num12].top;
											val2 = (int)((linkedSpans[num12].next == -1) ? 65536U : linkedSpans[linkedSpans[num12].next].bottom);
											if ((long)(global::System.Math.Min(val, val2) - global::System.Math.Max(top, num11)) > (long)((ulong)voxelWalkableHeight))
											{
												num5 = global::System.Math.Min(num5, num11 - top);
												if (global::System.Math.Abs(num11 - top) <= voxelWalkableClimb)
												{
													if (num11 < num6)
													{
														num6 = num11;
													}
													if (num11 > num7)
													{
														num7 = num11;
													}
												}
											}
										}
									}
								}
								if (num5 < -voxelWalkableClimb || num7 - num6 > voxelWalkableClimb)
								{
									linkedSpans[num4].area = 0;
								}
							}
						}
					}
				}
				i += num2;
				num3++;
			}
		}

		public ushort[] ExpandRegions(int maxIterations, uint level, ushort[] srcReg, ushort[] srcDist, ushort[] dstReg, ushort[] dstDist, global::System.Collections.Generic.List<int> stack)
		{
			int num = this.voxelArea.width;
			int num2 = this.voxelArea.depth;
			int num3 = num * num2;
			stack.Clear();
			int i = 0;
			int num4 = 0;
			while (i < num3)
			{
				for (int j = 0; j < this.voxelArea.width; j++)
				{
					global::Pathfinding.Voxels.CompactVoxelCell compactVoxelCell = this.voxelArea.compactCells[i + j];
					int k = (int)compactVoxelCell.index;
					int num5 = (int)(compactVoxelCell.index + compactVoxelCell.count);
					while (k < num5)
					{
						if ((uint)this.voxelArea.dist[k] >= level && srcReg[k] == 0 && this.voxelArea.areaTypes[k] != 0)
						{
							stack.Add(j);
							stack.Add(i);
							stack.Add(k);
						}
						k++;
					}
				}
				i += num;
				num4++;
			}
			int num6 = 0;
			int count = stack.Count;
			if (count > 0)
			{
				for (;;)
				{
					int num7 = 0;
					global::System.Buffer.BlockCopy(srcReg, 0, dstReg, 0, srcReg.Length * 2);
					global::System.Buffer.BlockCopy(srcDist, 0, dstDist, 0, dstDist.Length * 2);
					for (int l = 0; l < count; l += 3)
					{
						if (l >= count)
						{
							break;
						}
						int num8 = stack[l];
						int num9 = stack[l + 1];
						int num10 = stack[l + 2];
						if (num10 < 0)
						{
							num7++;
						}
						else
						{
							ushort num11 = srcReg[num10];
							ushort num12 = ushort.MaxValue;
							global::Pathfinding.Voxels.CompactVoxelSpan compactVoxelSpan = this.voxelArea.compactSpans[num10];
							int num13 = this.voxelArea.areaTypes[num10];
							for (int m = 0; m < 4; m++)
							{
								if ((long)compactVoxelSpan.GetConnection(m) != 63L)
								{
									int num14 = num8 + this.voxelArea.DirectionX[m];
									int num15 = num9 + this.voxelArea.DirectionZ[m];
									int num16 = (int)(this.voxelArea.compactCells[num14 + num15].index + (uint)compactVoxelSpan.GetConnection(m));
									if (num13 == this.voxelArea.areaTypes[num16])
									{
										if (srcReg[num16] > 0 && (srcReg[num16] & 32768) == 0 && srcDist[num16] + 2 < num12)
										{
											num11 = srcReg[num16];
											num12 = srcDist[num16] + 2;
										}
									}
								}
							}
							if (num11 != 0)
							{
								stack[l + 2] = -1;
								dstReg[num10] = num11;
								dstDist[num10] = num12;
							}
							else
							{
								num7++;
							}
						}
					}
					ushort[] array = srcReg;
					srcReg = dstReg;
					dstReg = array;
					array = srcDist;
					srcDist = dstDist;
					dstDist = array;
					if (num7 * 3 >= count)
					{
						break;
					}
					if (level > 0U)
					{
						num6++;
						if (num6 >= maxIterations)
						{
							break;
						}
					}
				}
			}
			return srcReg;
		}

		public bool FloodRegion(int x, int z, int i, uint level, ushort r, ushort[] srcReg, ushort[] srcDist, global::System.Collections.Generic.List<int> stack)
		{
			int num = this.voxelArea.areaTypes[i];
			stack.Clear();
			stack.Add(x);
			stack.Add(z);
			stack.Add(i);
			srcReg[i] = r;
			srcDist[i] = 0;
			int num2 = (int)((level < 2U) ? 0U : (level - 2U));
			int num3 = 0;
			while (stack.Count > 0)
			{
				int num4 = stack[stack.Count - 1];
				stack.RemoveAt(stack.Count - 1);
				int num5 = stack[stack.Count - 1];
				stack.RemoveAt(stack.Count - 1);
				int num6 = stack[stack.Count - 1];
				stack.RemoveAt(stack.Count - 1);
				global::Pathfinding.Voxels.CompactVoxelSpan compactVoxelSpan = this.voxelArea.compactSpans[num4];
				ushort num7 = 0;
				for (int j = 0; j < 4; j++)
				{
					if ((long)compactVoxelSpan.GetConnection(j) != 63L)
					{
						int num8 = num6 + this.voxelArea.DirectionX[j];
						int num9 = num5 + this.voxelArea.DirectionZ[j];
						int num10 = (int)(this.voxelArea.compactCells[num8 + num9].index + (uint)compactVoxelSpan.GetConnection(j));
						if (this.voxelArea.areaTypes[num10] == num)
						{
							ushort num11 = srcReg[num10];
							if ((num11 & 32768) != 32768)
							{
								if (num11 != 0 && num11 != r)
								{
									num7 = num11;
									break;
								}
								global::Pathfinding.Voxels.CompactVoxelSpan compactVoxelSpan2 = this.voxelArea.compactSpans[num10];
								int num12 = j + 1 & 3;
								if ((long)compactVoxelSpan2.GetConnection(num12) != 63L)
								{
									int num13 = num8 + this.voxelArea.DirectionX[num12];
									int num14 = num9 + this.voxelArea.DirectionZ[num12];
									int num15 = (int)(this.voxelArea.compactCells[num13 + num14].index + (uint)compactVoxelSpan2.GetConnection(num12));
									if (this.voxelArea.areaTypes[num15] == num)
									{
										ushort num16 = srcReg[num15];
										if (num16 != 0 && num16 != r)
										{
											num7 = num16;
											break;
										}
									}
								}
							}
						}
					}
				}
				if (num7 != 0)
				{
					srcReg[num4] = 0;
				}
				else
				{
					num3++;
					for (int k = 0; k < 4; k++)
					{
						if ((long)compactVoxelSpan.GetConnection(k) != 63L)
						{
							int num17 = num6 + this.voxelArea.DirectionX[k];
							int num18 = num5 + this.voxelArea.DirectionZ[k];
							int num19 = (int)(this.voxelArea.compactCells[num17 + num18].index + (uint)compactVoxelSpan.GetConnection(k));
							if (this.voxelArea.areaTypes[num19] == num)
							{
								if ((int)this.voxelArea.dist[num19] >= num2 && srcReg[num19] == 0)
								{
									srcReg[num19] = r;
									srcDist[num19] = 0;
									stack.Add(num17);
									stack.Add(num18);
									stack.Add(num19);
								}
							}
						}
					}
				}
			}
			return num3 > 0;
		}

		public void MarkRectWithRegion(int minx, int maxx, int minz, int maxz, ushort region, ushort[] srcReg)
		{
			int num = maxz * this.voxelArea.width;
			for (int i = minz * this.voxelArea.width; i < num; i += this.voxelArea.width)
			{
				for (int j = minx; j < maxx; j++)
				{
					global::Pathfinding.Voxels.CompactVoxelCell compactVoxelCell = this.voxelArea.compactCells[i + j];
					int k = (int)compactVoxelCell.index;
					int num2 = (int)(compactVoxelCell.index + compactVoxelCell.count);
					while (k < num2)
					{
						if (this.voxelArea.areaTypes[k] != 0)
						{
							srcReg[k] = region;
						}
						k++;
					}
				}
			}
		}

		public ushort CalculateDistanceField(ushort[] src)
		{
			int num = this.voxelArea.width * this.voxelArea.depth;
			for (int i = 0; i < num; i += this.voxelArea.width)
			{
				for (int j = 0; j < this.voxelArea.width; j++)
				{
					global::Pathfinding.Voxels.CompactVoxelCell compactVoxelCell = this.voxelArea.compactCells[j + i];
					int k = (int)compactVoxelCell.index;
					int num2 = (int)(compactVoxelCell.index + compactVoxelCell.count);
					while (k < num2)
					{
						global::Pathfinding.Voxels.CompactVoxelSpan compactVoxelSpan = this.voxelArea.compactSpans[k];
						int num3 = 0;
						for (int l = 0; l < 4; l++)
						{
							if ((long)compactVoxelSpan.GetConnection(l) == 63L)
							{
								break;
							}
							num3++;
						}
						if (num3 != 4)
						{
							src[k] = 0;
						}
						k++;
					}
				}
			}
			for (int m = 0; m < num; m += this.voxelArea.width)
			{
				for (int n = 0; n < this.voxelArea.width; n++)
				{
					global::Pathfinding.Voxels.CompactVoxelCell compactVoxelCell2 = this.voxelArea.compactCells[n + m];
					int num4 = (int)compactVoxelCell2.index;
					int num5 = (int)(compactVoxelCell2.index + compactVoxelCell2.count);
					while (num4 < num5)
					{
						global::Pathfinding.Voxels.CompactVoxelSpan compactVoxelSpan2 = this.voxelArea.compactSpans[num4];
						if ((long)compactVoxelSpan2.GetConnection(0) != 63L)
						{
							int num6 = n + this.voxelArea.DirectionX[0];
							int num7 = m + this.voxelArea.DirectionZ[0];
							int num8 = (int)((ulong)this.voxelArea.compactCells[num6 + num7].index + (ulong)((long)compactVoxelSpan2.GetConnection(0)));
							if (src[num8] + 2 < src[num4])
							{
								src[num4] = src[num8] + 2;
							}
							global::Pathfinding.Voxels.CompactVoxelSpan compactVoxelSpan3 = this.voxelArea.compactSpans[num8];
							if ((long)compactVoxelSpan3.GetConnection(3) != 63L)
							{
								int num9 = num6 + this.voxelArea.DirectionX[3];
								int num10 = num7 + this.voxelArea.DirectionZ[3];
								int num11 = (int)((ulong)this.voxelArea.compactCells[num9 + num10].index + (ulong)((long)compactVoxelSpan3.GetConnection(3)));
								if (src[num11] + 3 < src[num4])
								{
									src[num4] = src[num11] + 3;
								}
							}
						}
						if ((long)compactVoxelSpan2.GetConnection(3) != 63L)
						{
							int num12 = n + this.voxelArea.DirectionX[3];
							int num13 = m + this.voxelArea.DirectionZ[3];
							int num14 = (int)((ulong)this.voxelArea.compactCells[num12 + num13].index + (ulong)((long)compactVoxelSpan2.GetConnection(3)));
							if (src[num14] + 2 < src[num4])
							{
								src[num4] = src[num14] + 2;
							}
							global::Pathfinding.Voxels.CompactVoxelSpan compactVoxelSpan4 = this.voxelArea.compactSpans[num14];
							if ((long)compactVoxelSpan4.GetConnection(2) != 63L)
							{
								int num15 = num12 + this.voxelArea.DirectionX[2];
								int num16 = num13 + this.voxelArea.DirectionZ[2];
								int num17 = (int)((ulong)this.voxelArea.compactCells[num15 + num16].index + (ulong)((long)compactVoxelSpan4.GetConnection(2)));
								if (src[num17] + 3 < src[num4])
								{
									src[num4] = src[num17] + 3;
								}
							}
						}
						num4++;
					}
				}
			}
			for (int num18 = num - this.voxelArea.width; num18 >= 0; num18 -= this.voxelArea.width)
			{
				for (int num19 = this.voxelArea.width - 1; num19 >= 0; num19--)
				{
					global::Pathfinding.Voxels.CompactVoxelCell compactVoxelCell3 = this.voxelArea.compactCells[num19 + num18];
					int num20 = (int)compactVoxelCell3.index;
					int num21 = (int)(compactVoxelCell3.index + compactVoxelCell3.count);
					while (num20 < num21)
					{
						global::Pathfinding.Voxels.CompactVoxelSpan compactVoxelSpan5 = this.voxelArea.compactSpans[num20];
						if ((long)compactVoxelSpan5.GetConnection(2) != 63L)
						{
							int num22 = num19 + this.voxelArea.DirectionX[2];
							int num23 = num18 + this.voxelArea.DirectionZ[2];
							int num24 = (int)((ulong)this.voxelArea.compactCells[num22 + num23].index + (ulong)((long)compactVoxelSpan5.GetConnection(2)));
							if (src[num24] + 2 < src[num20])
							{
								src[num20] = src[num24] + 2;
							}
							global::Pathfinding.Voxels.CompactVoxelSpan compactVoxelSpan6 = this.voxelArea.compactSpans[num24];
							if ((long)compactVoxelSpan6.GetConnection(1) != 63L)
							{
								int num25 = num22 + this.voxelArea.DirectionX[1];
								int num26 = num23 + this.voxelArea.DirectionZ[1];
								int num27 = (int)((ulong)this.voxelArea.compactCells[num25 + num26].index + (ulong)((long)compactVoxelSpan6.GetConnection(1)));
								if (src[num27] + 3 < src[num20])
								{
									src[num20] = src[num27] + 3;
								}
							}
						}
						if ((long)compactVoxelSpan5.GetConnection(1) != 63L)
						{
							int num28 = num19 + this.voxelArea.DirectionX[1];
							int num29 = num18 + this.voxelArea.DirectionZ[1];
							int num30 = (int)((ulong)this.voxelArea.compactCells[num28 + num29].index + (ulong)((long)compactVoxelSpan5.GetConnection(1)));
							if (src[num30] + 2 < src[num20])
							{
								src[num20] = src[num30] + 2;
							}
							global::Pathfinding.Voxels.CompactVoxelSpan compactVoxelSpan7 = this.voxelArea.compactSpans[num30];
							if ((long)compactVoxelSpan7.GetConnection(0) != 63L)
							{
								int num31 = num28 + this.voxelArea.DirectionX[0];
								int num32 = num29 + this.voxelArea.DirectionZ[0];
								int num33 = (int)((ulong)this.voxelArea.compactCells[num31 + num32].index + (ulong)((long)compactVoxelSpan7.GetConnection(0)));
								if (src[num33] + 3 < src[num20])
								{
									src[num20] = src[num33] + 3;
								}
							}
						}
						num20++;
					}
				}
			}
			ushort num34 = 0;
			for (int num35 = 0; num35 < this.voxelArea.compactSpanCount; num35++)
			{
				num34 = global::System.Math.Max(src[num35], num34);
			}
			return num34;
		}

		public ushort[] BoxBlur(ushort[] src, ushort[] dst)
		{
			ushort num = 20;
			int num2 = this.voxelArea.width * this.voxelArea.depth;
			for (int i = num2 - this.voxelArea.width; i >= 0; i -= this.voxelArea.width)
			{
				for (int j = this.voxelArea.width - 1; j >= 0; j--)
				{
					global::Pathfinding.Voxels.CompactVoxelCell compactVoxelCell = this.voxelArea.compactCells[j + i];
					int k = (int)compactVoxelCell.index;
					int num3 = (int)(compactVoxelCell.index + compactVoxelCell.count);
					while (k < num3)
					{
						global::Pathfinding.Voxels.CompactVoxelSpan compactVoxelSpan = this.voxelArea.compactSpans[k];
						ushort num4 = src[k];
						if (num4 < num)
						{
							dst[k] = num4;
						}
						else
						{
							int num5 = (int)num4;
							for (int l = 0; l < 4; l++)
							{
								if ((long)compactVoxelSpan.GetConnection(l) != 63L)
								{
									int num6 = j + this.voxelArea.DirectionX[l];
									int num7 = i + this.voxelArea.DirectionZ[l];
									int num8 = (int)((ulong)this.voxelArea.compactCells[num6 + num7].index + (ulong)((long)compactVoxelSpan.GetConnection(l)));
									num5 += (int)src[num8];
									global::Pathfinding.Voxels.CompactVoxelSpan compactVoxelSpan2 = this.voxelArea.compactSpans[num8];
									int num9 = l + 1 & 3;
									if ((long)compactVoxelSpan2.GetConnection(num9) != 63L)
									{
										int num10 = num6 + this.voxelArea.DirectionX[num9];
										int num11 = num7 + this.voxelArea.DirectionZ[num9];
										int num12 = (int)((ulong)this.voxelArea.compactCells[num10 + num11].index + (ulong)((long)compactVoxelSpan2.GetConnection(num9)));
										num5 += (int)src[num12];
									}
									else
									{
										num5 += (int)num4;
									}
								}
								else
								{
									num5 += (int)(num4 * 2);
								}
							}
							dst[k] = (ushort)((float)(num5 + 5) / 9f);
						}
						k++;
					}
				}
			}
			return dst;
		}

		private void FloodOnes(global::System.Collections.Generic.List<global::Pathfinding.Int3> st1, ushort[] regs, uint level, ushort reg)
		{
			for (int i = 0; i < st1.Count; i++)
			{
				int x = st1[i].x;
				int y = st1[i].y;
				int z = st1[i].z;
				regs[y] = reg;
				global::Pathfinding.Voxels.CompactVoxelSpan compactVoxelSpan = this.voxelArea.compactSpans[y];
				int num = this.voxelArea.areaTypes[y];
				for (int j = 0; j < 4; j++)
				{
					if ((long)compactVoxelSpan.GetConnection(j) != 63L)
					{
						int num2 = x + this.voxelArea.DirectionX[j];
						int num3 = z + this.voxelArea.DirectionZ[j];
						int num4 = (int)(this.voxelArea.compactCells[num2 + num3].index + (uint)compactVoxelSpan.GetConnection(j));
						if (num == this.voxelArea.areaTypes[num4])
						{
							if (regs[num4] == 1)
							{
								regs[num4] = reg;
								st1.Add(new global::Pathfinding.Int3(num2, num4, num3));
							}
						}
					}
				}
			}
		}

		public void BuildRegions()
		{
			int num = this.voxelArea.width;
			int num2 = this.voxelArea.depth;
			int num3 = num * num2;
			int compactSpanCount = this.voxelArea.compactSpanCount;
			int num4 = 8;
			global::System.Collections.Generic.List<int> list = global::Pathfinding.Util.ListPool<int>.Claim(1024);
			ushort[] array = new ushort[compactSpanCount];
			ushort[] array2 = new ushort[compactSpanCount];
			ushort[] array3 = new ushort[compactSpanCount];
			ushort[] array4 = new ushort[compactSpanCount];
			ushort num5 = 2;
			this.MarkRectWithRegion(0, this.borderSize, 0, num2, num5 | 32768, array);
			num5 += 1;
			this.MarkRectWithRegion(num - this.borderSize, num, 0, num2, num5 | 32768, array);
			num5 += 1;
			this.MarkRectWithRegion(0, num, 0, this.borderSize, num5 | 32768, array);
			num5 += 1;
			this.MarkRectWithRegion(0, num, num2 - this.borderSize, num2, num5 | 32768, array);
			num5 += 1;
			uint num6 = (uint)(this.voxelArea.maxDistance + 1) & 4294967294U;
			int num7 = 0;
			while (num6 > 0U)
			{
				num6 = ((num6 < 2U) ? 0U : (num6 - 2U));
				if (this.ExpandRegions(num4, num6, array, array2, array3, array4, list) != array)
				{
					ushort[] array5 = array;
					array = array3;
					array3 = array5;
					array5 = array2;
					array2 = array4;
					array4 = array5;
				}
				int i = 0;
				int num8 = 0;
				while (i < num3)
				{
					for (int j = 0; j < this.voxelArea.width; j++)
					{
						global::Pathfinding.Voxels.CompactVoxelCell compactVoxelCell = this.voxelArea.compactCells[i + j];
						int k = (int)compactVoxelCell.index;
						int num9 = (int)(compactVoxelCell.index + compactVoxelCell.count);
						while (k < num9)
						{
							if ((uint)this.voxelArea.dist[k] >= num6 && array[k] == 0 && this.voxelArea.areaTypes[k] != 0)
							{
								if (this.FloodRegion(j, i, k, num6, num5, array, array2, list))
								{
									num5 += 1;
								}
							}
							k++;
						}
					}
					i += num;
					num8++;
				}
				num7++;
			}
			if (this.ExpandRegions(num4 * 8, 0U, array, array2, array3, array4, list) != array)
			{
				array = array3;
			}
			this.voxelArea.maxRegions = (int)num5;
			this.FilterSmallRegions(array, this.minRegionSize, this.voxelArea.maxRegions);
			for (int l = 0; l < this.voxelArea.compactSpanCount; l++)
			{
				this.voxelArea.compactSpans[l].reg = (int)array[l];
			}
			global::Pathfinding.Util.ListPool<int>.Release(list);
		}

		private static int union_find_find(int[] arr, int x)
		{
			if (arr[x] < 0)
			{
				return x;
			}
			return arr[x] = global::Pathfinding.Voxels.Voxelize.union_find_find(arr, arr[x]);
		}

		private static void union_find_union(int[] arr, int a, int b)
		{
			a = global::Pathfinding.Voxels.Voxelize.union_find_find(arr, a);
			b = global::Pathfinding.Voxels.Voxelize.union_find_find(arr, b);
			if (a == b)
			{
				return;
			}
			if (arr[a] > arr[b])
			{
				int num = a;
				a = b;
				b = num;
			}
			arr[a] += arr[b];
			arr[b] = a;
		}

		public void FilterSmallRegions(ushort[] reg, int minRegionSize, int maxRegions)
		{
			global::Pathfinding.RelevantGraphSurface relevantGraphSurface = global::Pathfinding.RelevantGraphSurface.Root;
			bool flag = !object.ReferenceEquals(relevantGraphSurface, null) && this.relevantGraphSurfaceMode != global::Pathfinding.RecastGraph.RelevantGraphSurfaceMode.DoNotRequire;
			if (!flag && minRegionSize <= 0)
			{
				return;
			}
			int[] array = new int[maxRegions];
			ushort[] array2 = this.voxelArea.tmpUShortArr;
			if (array2 == null || array2.Length < maxRegions)
			{
				array2 = (this.voxelArea.tmpUShortArr = new ushort[maxRegions]);
			}
			global::Pathfinding.Util.Memory.MemSet<int>(array, -1, 4);
			global::Pathfinding.Util.Memory.MemSet<ushort>(array2, 0, maxRegions, 2);
			int num = array.Length;
			int num2 = this.voxelArea.width * this.voxelArea.depth;
			int num3 = 2 | ((this.relevantGraphSurfaceMode != global::Pathfinding.RecastGraph.RelevantGraphSurfaceMode.OnlyForCompletelyInsideTile) ? 0 : 1);
			if (flag)
			{
				while (!object.ReferenceEquals(relevantGraphSurface, null))
				{
					int num4;
					int num5;
					this.VectorToIndex(relevantGraphSurface.Position, out num4, out num5);
					if (num4 < 0 || num5 < 0 || num4 >= this.voxelArea.width || num5 >= this.voxelArea.depth)
					{
						relevantGraphSurface = relevantGraphSurface.Next;
					}
					else
					{
						int num6 = (int)((relevantGraphSurface.Position.y - this.voxelOffset.y) / this.cellHeight);
						int num7 = (int)(relevantGraphSurface.maxRange / this.cellHeight);
						global::Pathfinding.Voxels.CompactVoxelCell compactVoxelCell = this.voxelArea.compactCells[num4 + num5 * this.voxelArea.width];
						int num8 = (int)compactVoxelCell.index;
						while ((long)num8 < (long)((ulong)(compactVoxelCell.index + compactVoxelCell.count)))
						{
							global::Pathfinding.Voxels.CompactVoxelSpan compactVoxelSpan = this.voxelArea.compactSpans[num8];
							if (global::System.Math.Abs((int)compactVoxelSpan.y - num6) <= num7 && reg[num8] != 0)
							{
								ushort[] array3 = array2;
								int num9 = global::Pathfinding.Voxels.Voxelize.union_find_find(array, (int)reg[num8] & -32769);
								array3[num9] |= 2;
							}
							num8++;
						}
						relevantGraphSurface = relevantGraphSurface.Next;
					}
				}
			}
			int i = 0;
			int num10 = 0;
			while (i < num2)
			{
				for (int j = 0; j < this.voxelArea.width; j++)
				{
					global::Pathfinding.Voxels.CompactVoxelCell compactVoxelCell2 = this.voxelArea.compactCells[j + i];
					int num11 = (int)compactVoxelCell2.index;
					while ((long)num11 < (long)((ulong)(compactVoxelCell2.index + compactVoxelCell2.count)))
					{
						global::Pathfinding.Voxels.CompactVoxelSpan compactVoxelSpan2 = this.voxelArea.compactSpans[num11];
						int num12 = (int)reg[num11];
						if ((num12 & -32769) != 0)
						{
							if (num12 >= num)
							{
								ushort[] array4 = array2;
								int num13 = global::Pathfinding.Voxels.Voxelize.union_find_find(array, num12 & -32769);
								array4[num13] |= 1;
							}
							else
							{
								int num14 = global::Pathfinding.Voxels.Voxelize.union_find_find(array, num12);
								array[num14]--;
								for (int k = 0; k < 4; k++)
								{
									if ((long)compactVoxelSpan2.GetConnection(k) != 63L)
									{
										int num15 = j + this.voxelArea.DirectionX[k];
										int num16 = i + this.voxelArea.DirectionZ[k];
										int num17 = (int)(this.voxelArea.compactCells[num15 + num16].index + (uint)compactVoxelSpan2.GetConnection(k));
										int num18 = (int)reg[num17];
										if (num12 != num18 && (num18 & -32769) != 0)
										{
											if ((num18 & 32768) != 0)
											{
												ushort[] array5 = array2;
												int num19 = num14;
												array5[num19] |= 1;
											}
											else
											{
												global::Pathfinding.Voxels.Voxelize.union_find_union(array, num14, num18);
											}
										}
									}
								}
							}
						}
						num11++;
					}
				}
				i += this.voxelArea.width;
				num10++;
			}
			for (int l = 0; l < array.Length; l++)
			{
				ushort[] array6 = array2;
				int num20 = global::Pathfinding.Voxels.Voxelize.union_find_find(array, l);
				array6[num20] |= array2[l];
			}
			for (int m = 0; m < array.Length; m++)
			{
				int num21 = global::Pathfinding.Voxels.Voxelize.union_find_find(array, m);
				if ((array2[num21] & 1) != 0)
				{
					array[num21] = -minRegionSize - 2;
				}
				if (flag && ((int)array2[num21] & num3) == 0)
				{
					array[num21] = -1;
				}
			}
			for (int n = 0; n < this.voxelArea.compactSpanCount; n++)
			{
				int num22 = (int)reg[n];
				if (num22 < num)
				{
					if (array[global::Pathfinding.Voxels.Voxelize.union_find_find(array, num22)] >= -minRegionSize - 1)
					{
						reg[n] = 0;
					}
				}
			}
		}

		public const uint NotConnected = 63U;

		private const int MaxLayers = 65535;

		private const int MaxRegions = 500;

		private const int UnwalkableArea = 0;

		private const ushort BorderReg = 32768;

		private const int RC_BORDER_VERTEX = 65536;

		private const int RC_AREA_BORDER = 131072;

		private const int VERTEX_BUCKET_COUNT = 4096;

		public const int RC_CONTOUR_TESS_WALL_EDGES = 1;

		public const int RC_CONTOUR_TESS_AREA_EDGES = 2;

		private const int ContourRegMask = 65535;

		private static global::System.Collections.Generic.List<int[]> intArrCache = new global::System.Collections.Generic.List<int[]>();

		private static readonly int[] emptyArr = new int[0];

		public global::System.Collections.Generic.List<global::Pathfinding.Voxels.RasterizationMesh> inputMeshes;

		public readonly int voxelWalkableClimb;

		public readonly uint voxelWalkableHeight;

		public readonly float cellSize = 0.2f;

		public readonly float cellHeight = 0.1f;

		public int minRegionSize = 100;

		public int borderSize;

		public float maxEdgeLength = 20f;

		public float maxSlope = 30f;

		public global::Pathfinding.RecastGraph.RelevantGraphSurfaceMode relevantGraphSurfaceMode;

		public global::UnityEngine.Bounds forcedBounds;

		public global::Pathfinding.Voxels.VoxelArea voxelArea;

		public global::Pathfinding.Voxels.VoxelContourSet countourSet;

		private global::Pathfinding.Voxels.VoxelPolygonClipper clipper;

		public int width;

		public int depth;

		private global::UnityEngine.Vector3 voxelOffset;

		private readonly global::UnityEngine.Vector3 cellScale;
	}
}
