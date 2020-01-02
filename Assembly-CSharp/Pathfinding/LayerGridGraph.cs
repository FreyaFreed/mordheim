using System;
using System.Collections.Generic;
using Pathfinding.Serialization;
using UnityEngine;

namespace Pathfinding
{
	public class LayerGridGraph : global::Pathfinding.GridGraph, global::Pathfinding.IUpdatableGraph
	{
		public override void OnDestroy()
		{
			base.OnDestroy();
			this.RemoveGridGraphFromStatic();
		}

		private void RemoveGridGraphFromStatic()
		{
			global::Pathfinding.LevelGridNode.SetGridGraph(this.active.astarData.GetGraphIndex(this), null);
		}

		public override bool uniformWidthDepthGrid
		{
			get
			{
				return false;
			}
		}

		public override int CountNodes()
		{
			if (this.nodes == null)
			{
				return 0;
			}
			int num = 0;
			for (int i = 0; i < this.nodes.Length; i++)
			{
				if (this.nodes[i] != null)
				{
					num++;
				}
			}
			return num;
		}

		public override void GetNodes(global::Pathfinding.GraphNodeDelegateCancelable del)
		{
			if (this.nodes == null)
			{
				return;
			}
			for (int i = 0; i < this.nodes.Length; i++)
			{
				if (this.nodes[i] != null && !del(this.nodes[i]))
				{
					break;
				}
			}
		}

		public new void UpdateArea(global::Pathfinding.GraphUpdateObject o)
		{
			if (this.nodes == null || this.nodes.Length != this.width * this.depth * this.layerCount)
			{
				global::UnityEngine.Debug.LogWarning("The Grid Graph is not scanned, cannot update area ");
				return;
			}
			global::UnityEngine.Bounds bounds = o.bounds;
			global::UnityEngine.Vector3 a;
			global::UnityEngine.Vector3 a2;
			global::Pathfinding.GridGraph.GetBoundsMinMax(bounds, this.inverseMatrix, out a, out a2);
			int xmin = global::UnityEngine.Mathf.RoundToInt(a.x - 0.5f);
			int xmax = global::UnityEngine.Mathf.RoundToInt(a2.x - 0.5f);
			int ymin = global::UnityEngine.Mathf.RoundToInt(a.z - 0.5f);
			int ymax = global::UnityEngine.Mathf.RoundToInt(a2.z - 0.5f);
			global::Pathfinding.IntRect intRect = new global::Pathfinding.IntRect(xmin, ymin, xmax, ymax);
			global::Pathfinding.IntRect intRect2 = intRect;
			global::Pathfinding.IntRect b = new global::Pathfinding.IntRect(0, 0, this.width - 1, this.depth - 1);
			global::Pathfinding.IntRect intRect3 = intRect;
			bool flag = o.updatePhysics || o.modifyWalkability;
			bool flag2 = o is global::Pathfinding.LayerGridGraphUpdate && ((global::Pathfinding.LayerGridGraphUpdate)o).recalculateNodes;
			bool preserveExistingNodes = !(o is global::Pathfinding.LayerGridGraphUpdate) || ((global::Pathfinding.LayerGridGraphUpdate)o).preserveExistingNodes;
			int num = (!o.updateErosion) ? 0 : this.erodeIterations;
			if (o.trackChangedNodes && flag2)
			{
				global::UnityEngine.Debug.LogError("Cannot track changed nodes when creating or deleting nodes.\nWill not update LayerGridGraph");
				return;
			}
			if (o.updatePhysics && !o.modifyWalkability && this.collision.collisionCheck)
			{
				global::UnityEngine.Vector3 a3 = new global::UnityEngine.Vector3(this.collision.diameter, 0f, this.collision.diameter) * 0.5f;
				a -= a3 * 1.02f;
				a2 += a3 * 1.02f;
				intRect3 = new global::Pathfinding.IntRect(global::UnityEngine.Mathf.RoundToInt(a.x - 0.5f), global::UnityEngine.Mathf.RoundToInt(a.z - 0.5f), global::UnityEngine.Mathf.RoundToInt(a2.x - 0.5f), global::UnityEngine.Mathf.RoundToInt(a2.z - 0.5f));
				intRect2 = global::Pathfinding.IntRect.Union(intRect3, intRect2);
			}
			if (flag || num > 0)
			{
				intRect2 = intRect2.Expand(num + 1);
			}
			global::Pathfinding.IntRect intRect4 = global::Pathfinding.IntRect.Intersection(intRect2, b);
			if (!flag2)
			{
				for (int i = intRect4.xmin; i <= intRect4.xmax; i++)
				{
					for (int j = intRect4.ymin; j <= intRect4.ymax; j++)
					{
						for (int k = 0; k < this.layerCount; k++)
						{
							o.WillUpdateNode(this.nodes[k * this.width * this.depth + j * this.width + i]);
						}
					}
				}
			}
			if (o.updatePhysics && !o.modifyWalkability)
			{
				this.collision.Initialize(this.matrix, this.nodeSize);
				intRect4 = global::Pathfinding.IntRect.Intersection(intRect3, b);
				bool flag3 = false;
				for (int l = intRect4.xmin; l <= intRect4.xmax; l++)
				{
					for (int m = intRect4.ymin; m <= intRect4.ymax; m++)
					{
						flag3 |= this.RecalculateCell(l, m, preserveExistingNodes);
					}
				}
				for (int n = intRect4.xmin; n <= intRect4.xmax; n++)
				{
					for (int num2 = intRect4.ymin; num2 <= intRect4.ymax; num2++)
					{
						for (int num3 = 0; num3 < this.layerCount; num3++)
						{
							int num4 = num3 * this.width * this.depth + num2 * this.width + n;
							global::Pathfinding.LevelGridNode levelGridNode = this.nodes[num4];
							if (levelGridNode != null)
							{
								this.CalculateConnections(this.nodes, levelGridNode, n, num2, num3);
							}
						}
					}
				}
			}
			intRect4 = global::Pathfinding.IntRect.Intersection(intRect, b);
			for (int num5 = intRect4.xmin; num5 <= intRect4.xmax; num5++)
			{
				for (int num6 = intRect4.ymin; num6 <= intRect4.ymax; num6++)
				{
					for (int num7 = 0; num7 < this.layerCount; num7++)
					{
						int num8 = num7 * this.width * this.depth + num6 * this.width + num5;
						global::Pathfinding.LevelGridNode levelGridNode2 = this.nodes[num8];
						if (levelGridNode2 != null)
						{
							if (flag)
							{
								levelGridNode2.Walkable = levelGridNode2.WalkableErosion;
								if (o.bounds.Contains((global::UnityEngine.Vector3)levelGridNode2.position))
								{
									o.Apply(levelGridNode2);
								}
								levelGridNode2.WalkableErosion = levelGridNode2.Walkable;
							}
							else if (o.bounds.Contains((global::UnityEngine.Vector3)levelGridNode2.position))
							{
								o.Apply(levelGridNode2);
							}
						}
					}
				}
			}
			if (flag && num == 0)
			{
				intRect4 = global::Pathfinding.IntRect.Intersection(intRect2, b);
				for (int num9 = intRect4.xmin; num9 <= intRect4.xmax; num9++)
				{
					for (int num10 = intRect4.ymin; num10 <= intRect4.ymax; num10++)
					{
						for (int num11 = 0; num11 < this.layerCount; num11++)
						{
							int num12 = num11 * this.width * this.depth + num10 * this.width + num9;
							global::Pathfinding.LevelGridNode levelGridNode3 = this.nodes[num12];
							if (levelGridNode3 != null)
							{
								this.CalculateConnections(this.nodes, levelGridNode3, num9, num10, num11);
							}
						}
					}
				}
			}
			else if (flag && num > 0)
			{
				global::Pathfinding.IntRect a4 = global::Pathfinding.IntRect.Union(intRect, intRect3).Expand(num);
				global::Pathfinding.IntRect a5 = a4.Expand(num);
				a4 = global::Pathfinding.IntRect.Intersection(a4, b);
				a5 = global::Pathfinding.IntRect.Intersection(a5, b);
				for (int num13 = a5.xmin; num13 <= a5.xmax; num13++)
				{
					for (int num14 = a5.ymin; num14 <= a5.ymax; num14++)
					{
						for (int num15 = 0; num15 < this.layerCount; num15++)
						{
							int num16 = num15 * this.width * this.depth + num14 * this.width + num13;
							global::Pathfinding.LevelGridNode levelGridNode4 = this.nodes[num16];
							if (levelGridNode4 != null)
							{
								bool walkable = levelGridNode4.Walkable;
								levelGridNode4.Walkable = levelGridNode4.WalkableErosion;
								if (!a4.Contains(num13, num14))
								{
									levelGridNode4.TmpWalkable = walkable;
								}
							}
						}
					}
				}
				for (int num17 = a5.xmin; num17 <= a5.xmax; num17++)
				{
					for (int num18 = a5.ymin; num18 <= a5.ymax; num18++)
					{
						for (int num19 = 0; num19 < this.layerCount; num19++)
						{
							int num20 = num19 * this.width * this.depth + num18 * this.width + num17;
							global::Pathfinding.LevelGridNode levelGridNode5 = this.nodes[num20];
							if (levelGridNode5 != null)
							{
								this.CalculateConnections(this.nodes, levelGridNode5, num17, num18, num19);
							}
						}
					}
				}
				this.ErodeWalkableArea(a5.xmin, a5.ymin, a5.xmax + 1, a5.ymax + 1);
				for (int num21 = a5.xmin; num21 <= a5.xmax; num21++)
				{
					for (int num22 = a5.ymin; num22 <= a5.ymax; num22++)
					{
						if (!a4.Contains(num21, num22))
						{
							for (int num23 = 0; num23 < this.layerCount; num23++)
							{
								int num24 = num23 * this.width * this.depth + num22 * this.width + num21;
								global::Pathfinding.LevelGridNode levelGridNode6 = this.nodes[num24];
								if (levelGridNode6 != null)
								{
									levelGridNode6.Walkable = levelGridNode6.TmpWalkable;
								}
							}
						}
					}
				}
				for (int num25 = a5.xmin; num25 <= a5.xmax; num25++)
				{
					for (int num26 = a5.ymin; num26 <= a5.ymax; num26++)
					{
						for (int num27 = 0; num27 < this.layerCount; num27++)
						{
							int num28 = num27 * this.width * this.depth + num26 * this.width + num25;
							global::Pathfinding.LevelGridNode levelGridNode7 = this.nodes[num28];
							if (levelGridNode7 != null)
							{
								this.CalculateConnections(this.nodes, levelGridNode7, num25, num26, num27);
							}
						}
					}
				}
			}
		}

		public override global::System.Collections.Generic.IEnumerable<global::Pathfinding.Progress> ScanInternal()
		{
			this.ScanInternal(delegate(global::Pathfinding.Progress p)
			{
			});
			yield break;
		}

		public void ScanInternal(global::Pathfinding.OnScanStatus status)
		{
			if (this.nodeSize <= 0f)
			{
				return;
			}
			base.GenerateMatrix();
			if (this.width > 1024 || this.depth > 1024)
			{
				global::UnityEngine.Debug.LogError("One of the grid's sides is longer than 1024 nodes");
				return;
			}
			this.lastScannedWidth = this.width;
			this.lastScannedDepth = this.depth;
			this.SetUpOffsetsAndCosts();
			global::Pathfinding.LevelGridNode.SetGridGraph(this.active.astarData.GetGraphIndex(this), this);
			this.maxClimb = global::UnityEngine.Mathf.Clamp(this.maxClimb, 0f, this.characterHeight);
			global::Pathfinding.LinkedLevelCell[] array = new global::Pathfinding.LinkedLevelCell[this.width * this.depth];
			this.collision = (this.collision ?? new global::Pathfinding.GraphCollision());
			this.collision.Initialize(this.matrix, this.nodeSize);
			for (int i = 0; i < this.depth; i++)
			{
				for (int j = 0; j < this.width; j++)
				{
					array[i * this.width + j] = new global::Pathfinding.LinkedLevelCell();
					global::Pathfinding.LinkedLevelCell linkedLevelCell = array[i * this.width + j];
					global::UnityEngine.Vector3 position = this.matrix.MultiplyPoint3x4(new global::UnityEngine.Vector3((float)j + 0.5f, 0f, (float)i + 0.5f));
					global::UnityEngine.RaycastHit[] array2 = this.collision.CheckHeightAll(position);
					for (int k = 0; k < array2.Length / 2; k++)
					{
						global::UnityEngine.RaycastHit raycastHit = array2[k];
						array2[k] = array2[array2.Length - 1 - k];
						array2[array2.Length - 1 - k] = raycastHit;
					}
					if (array2.Length > 0)
					{
						global::Pathfinding.LinkedLevelNode linkedLevelNode = null;
						for (int l = 0; l < array2.Length; l++)
						{
							global::Pathfinding.LinkedLevelNode linkedLevelNode2 = new global::Pathfinding.LinkedLevelNode();
							linkedLevelNode2.position = array2[l].point;
							if (linkedLevelNode != null && linkedLevelNode2.position.y - linkedLevelNode.position.y <= this.mergeSpanRange)
							{
								linkedLevelNode.position = linkedLevelNode2.position;
								linkedLevelNode.hit = array2[l];
								linkedLevelNode.walkable = this.collision.Check(linkedLevelNode2.position);
							}
							else
							{
								linkedLevelNode2.walkable = this.collision.Check(linkedLevelNode2.position);
								linkedLevelNode2.hit = array2[l];
								linkedLevelNode2.height = float.PositiveInfinity;
								if (linkedLevelCell.first == null)
								{
									linkedLevelCell.first = linkedLevelNode2;
									linkedLevelNode = linkedLevelNode2;
								}
								else
								{
									linkedLevelNode.next = linkedLevelNode2;
									linkedLevelNode.height = linkedLevelNode2.position.y - linkedLevelNode.position.y;
									linkedLevelNode = linkedLevelNode.next;
								}
							}
						}
					}
					else
					{
						linkedLevelCell.first = new global::Pathfinding.LinkedLevelNode
						{
							position = position,
							height = float.PositiveInfinity,
							walkable = !this.collision.unwalkableWhenNoGround
						};
					}
				}
			}
			int num = 0;
			this.layerCount = 0;
			for (int m = 0; m < this.depth; m++)
			{
				for (int n = 0; n < this.width; n++)
				{
					global::Pathfinding.LinkedLevelCell linkedLevelCell2 = array[m * this.width + n];
					global::Pathfinding.LinkedLevelNode linkedLevelNode3 = linkedLevelCell2.first;
					int num2 = 0;
					do
					{
						num2++;
						num++;
						linkedLevelNode3 = linkedLevelNode3.next;
					}
					while (linkedLevelNode3 != null);
					this.layerCount = ((num2 <= this.layerCount) ? this.layerCount : num2);
				}
			}
			if (this.layerCount > 255)
			{
				global::UnityEngine.Debug.LogError("Too many layers, a maximum of LevelGridNode.MaxLayerCount are allowed (found " + this.layerCount + ")");
				return;
			}
			this.nodes = new global::Pathfinding.LevelGridNode[this.width * this.depth * this.layerCount];
			for (int num3 = 0; num3 < this.nodes.Length; num3++)
			{
				this.nodes[num3] = new global::Pathfinding.LevelGridNode(this.active);
				this.nodes[num3].Penalty = this.initialPenalty;
			}
			int num4 = 0;
			float num5 = global::UnityEngine.Mathf.Cos(this.maxSlope * 0.0174532924f);
			for (int num6 = 0; num6 < this.depth; num6++)
			{
				for (int num7 = 0; num7 < this.width; num7++)
				{
					global::Pathfinding.LinkedLevelCell linkedLevelCell3 = array[num6 * this.width + num7];
					global::Pathfinding.LinkedLevelNode linkedLevelNode4 = linkedLevelCell3.first;
					linkedLevelCell3.index = num4;
					int num8 = 0;
					int num9 = 0;
					do
					{
						global::Pathfinding.LevelGridNode levelGridNode = this.nodes[num6 * this.width + num7 + this.width * this.depth * num9];
						levelGridNode.SetPosition((global::Pathfinding.Int3)linkedLevelNode4.position);
						levelGridNode.Walkable = linkedLevelNode4.walkable;
						if (linkedLevelNode4.hit.normal != global::UnityEngine.Vector3.zero && (this.penaltyAngle || num5 < 1f))
						{
							float num10 = global::UnityEngine.Vector3.Dot(linkedLevelNode4.hit.normal.normalized, this.collision.up);
							if (this.penaltyAngle)
							{
								levelGridNode.Penalty += (uint)global::UnityEngine.Mathf.RoundToInt((1f - num10) * this.penaltyAngleFactor);
							}
							if (num10 < num5)
							{
								levelGridNode.Walkable = false;
							}
						}
						levelGridNode.NodeInGridIndex = num6 * this.width + num7;
						if (linkedLevelNode4.height < this.characterHeight)
						{
							levelGridNode.Walkable = false;
						}
						levelGridNode.WalkableErosion = levelGridNode.Walkable;
						num4++;
						num8++;
						linkedLevelNode4 = linkedLevelNode4.next;
						num9++;
					}
					while (linkedLevelNode4 != null);
					while (num9 < this.layerCount)
					{
						this.nodes[num6 * this.width + num7 + this.width * this.depth * num9] = null;
						num9++;
					}
					linkedLevelCell3.count = num8;
				}
			}
			for (int num11 = 0; num11 < this.depth; num11++)
			{
				for (int num12 = 0; num12 < this.width; num12++)
				{
					for (int num13 = 0; num13 < this.layerCount; num13++)
					{
						global::Pathfinding.GraphNode node = this.nodes[num11 * this.width + num12 + this.width * this.depth * num13];
						this.CalculateConnections(this.nodes, node, num12, num11, num13);
					}
				}
			}
			uint graphIndex = (uint)this.active.astarData.GetGraphIndex(this);
			for (int num14 = 0; num14 < this.nodes.Length; num14++)
			{
				global::Pathfinding.LevelGridNode levelGridNode2 = this.nodes[num14];
				if (levelGridNode2 != null)
				{
					this.UpdatePenalty(levelGridNode2);
					levelGridNode2.GraphIndex = graphIndex;
					if (!levelGridNode2.HasAnyGridConnections())
					{
						levelGridNode2.Walkable = false;
						levelGridNode2.WalkableErosion = levelGridNode2.Walkable;
					}
				}
			}
			this.ErodeWalkableArea();
		}

		public bool RecalculateCell(int x, int z, bool preserveExistingNodes)
		{
			global::Pathfinding.LinkedLevelCell linkedLevelCell = new global::Pathfinding.LinkedLevelCell();
			global::UnityEngine.Vector3 position = this.matrix.MultiplyPoint3x4(new global::UnityEngine.Vector3((float)x + 0.5f, 0f, (float)z + 0.5f));
			global::UnityEngine.RaycastHit[] array = this.collision.CheckHeightAll(position);
			for (int i = 0; i < array.Length / 2; i++)
			{
				global::UnityEngine.RaycastHit raycastHit = array[i];
				array[i] = array[array.Length - 1 - i];
				array[array.Length - 1 - i] = raycastHit;
			}
			bool result = false;
			if (array.Length > 0)
			{
				global::Pathfinding.LinkedLevelNode linkedLevelNode = null;
				for (int j = 0; j < array.Length; j++)
				{
					global::Pathfinding.LinkedLevelNode linkedLevelNode2 = new global::Pathfinding.LinkedLevelNode();
					linkedLevelNode2.position = array[j].point;
					if (linkedLevelNode != null && linkedLevelNode2.position.y - linkedLevelNode.position.y <= this.mergeSpanRange)
					{
						linkedLevelNode.position = linkedLevelNode2.position;
						linkedLevelNode.hit = array[j];
						linkedLevelNode.walkable = this.collision.Check(linkedLevelNode2.position);
					}
					else
					{
						linkedLevelNode2.walkable = this.collision.Check(linkedLevelNode2.position);
						linkedLevelNode2.hit = array[j];
						linkedLevelNode2.height = float.PositiveInfinity;
						if (linkedLevelCell.first == null)
						{
							linkedLevelCell.first = linkedLevelNode2;
							linkedLevelNode = linkedLevelNode2;
						}
						else
						{
							linkedLevelNode.next = linkedLevelNode2;
							linkedLevelNode.height = linkedLevelNode2.position.y - linkedLevelNode.position.y;
							linkedLevelNode = linkedLevelNode.next;
						}
					}
				}
			}
			else
			{
				linkedLevelCell.first = new global::Pathfinding.LinkedLevelNode
				{
					position = position,
					height = float.PositiveInfinity,
					walkable = !this.collision.unwalkableWhenNoGround
				};
			}
			uint graphIndex = (uint)this.active.astarData.GetGraphIndex(this);
			global::Pathfinding.LinkedLevelNode linkedLevelNode3 = linkedLevelCell.first;
			int num = 0;
			int k = 0;
			for (;;)
			{
				if (k >= this.layerCount)
				{
					if (k + 1 > 255)
					{
						break;
					}
					this.AddLayers(1);
					result = true;
				}
				global::Pathfinding.LevelGridNode levelGridNode = this.nodes[z * this.width + x + this.width * this.depth * k];
				if (levelGridNode == null || !preserveExistingNodes)
				{
					this.nodes[z * this.width + x + this.width * this.depth * k] = new global::Pathfinding.LevelGridNode(this.active);
					levelGridNode = this.nodes[z * this.width + x + this.width * this.depth * k];
					levelGridNode.Penalty = this.initialPenalty;
					levelGridNode.GraphIndex = graphIndex;
					result = true;
				}
				levelGridNode.SetPosition((global::Pathfinding.Int3)linkedLevelNode3.position);
				levelGridNode.Walkable = linkedLevelNode3.walkable;
				levelGridNode.WalkableErosion = levelGridNode.Walkable;
				if (linkedLevelNode3.hit.normal != global::UnityEngine.Vector3.zero)
				{
					float num2 = global::UnityEngine.Vector3.Dot(linkedLevelNode3.hit.normal.normalized, this.collision.up);
					if (this.penaltyAngle)
					{
						levelGridNode.Penalty += (uint)global::UnityEngine.Mathf.RoundToInt((1f - num2) * this.penaltyAngleFactor);
					}
					float num3 = global::UnityEngine.Mathf.Cos(this.maxSlope * 0.0174532924f);
					if (num2 < num3)
					{
						levelGridNode.Walkable = false;
					}
				}
				levelGridNode.NodeInGridIndex = z * this.width + x;
				if (linkedLevelNode3.height < this.characterHeight)
				{
					levelGridNode.Walkable = false;
				}
				num++;
				linkedLevelNode3 = linkedLevelNode3.next;
				k++;
				if (linkedLevelNode3 == null)
				{
					goto Block_14;
				}
			}
			global::UnityEngine.Debug.LogError("Too many layers, a maximum of LevelGridNode.MaxLayerCount are allowed (required " + (k + 1) + ")");
			return result;
			Block_14:
			while (k < this.layerCount)
			{
				this.nodes[z * this.width + x + this.width * this.depth * k] = null;
				k++;
			}
			linkedLevelCell.count = num;
			return result;
		}

		public void AddLayers(int count)
		{
			int num = this.layerCount + count;
			if (num > 255)
			{
				global::UnityEngine.Debug.LogError("Too many layers, a maximum of LevelGridNode.MaxLayerCount are allowed (required " + num + ")");
				return;
			}
			global::Pathfinding.LevelGridNode[] array = this.nodes;
			this.nodes = new global::Pathfinding.LevelGridNode[this.width * this.depth * num];
			for (int i = 0; i < array.Length; i++)
			{
				this.nodes[i] = array[i];
			}
			this.layerCount = num;
		}

		public virtual void UpdatePenalty(global::Pathfinding.LevelGridNode node)
		{
			node.Penalty = 0U;
			node.Penalty = this.initialPenalty;
			if (this.penaltyPosition)
			{
				node.Penalty += (uint)global::UnityEngine.Mathf.RoundToInt(((float)node.position.y - this.penaltyPositionOffset) * this.penaltyPositionFactor);
			}
		}

		public override void ErodeWalkableArea(int xmin, int zmin, int xmax, int zmax)
		{
			xmin = global::UnityEngine.Mathf.Clamp(xmin, 0, base.Width);
			xmax = global::UnityEngine.Mathf.Clamp(xmax, 0, base.Width);
			zmin = global::UnityEngine.Mathf.Clamp(zmin, 0, base.Depth);
			zmax = global::UnityEngine.Mathf.Clamp(zmax, 0, base.Depth);
			if (this.erosionUseTags)
			{
				global::UnityEngine.Debug.LogError("Erosion Uses Tags is not supported for LayerGridGraphs yet");
			}
			for (int i = 0; i < this.erodeIterations; i++)
			{
				for (int j = 0; j < this.layerCount; j++)
				{
					for (int k = zmin; k < zmax; k++)
					{
						for (int l = xmin; l < xmax; l++)
						{
							global::Pathfinding.LevelGridNode levelGridNode = this.nodes[k * this.width + l + this.width * this.depth * j];
							if (levelGridNode != null)
							{
								if (levelGridNode.Walkable)
								{
									bool flag = false;
									for (int m = 0; m < 4; m++)
									{
										if (!levelGridNode.GetConnection(m))
										{
											flag = true;
											break;
										}
									}
									if (flag)
									{
										levelGridNode.Walkable = false;
									}
								}
							}
						}
					}
				}
				for (int n = 0; n < this.layerCount; n++)
				{
					for (int num = zmin; num < zmax; num++)
					{
						for (int num2 = xmin; num2 < xmax; num2++)
						{
							global::Pathfinding.LevelGridNode levelGridNode2 = this.nodes[num * this.width + num2 + this.width * this.depth * n];
							if (levelGridNode2 != null)
							{
								this.CalculateConnections(this.nodes, levelGridNode2, num2, num, n);
							}
						}
					}
				}
			}
		}

		public void CalculateConnections(global::Pathfinding.GraphNode[] nodes, global::Pathfinding.GraphNode node, int x, int z, int layerIndex)
		{
			if (node == null)
			{
				return;
			}
			global::Pathfinding.LevelGridNode levelGridNode = (global::Pathfinding.LevelGridNode)node;
			levelGridNode.ResetAllGridConnections();
			if (!node.Walkable)
			{
				return;
			}
			float num;
			if (layerIndex == this.layerCount - 1 || nodes[levelGridNode.NodeInGridIndex + this.width * this.depth * (layerIndex + 1)] == null)
			{
				num = float.PositiveInfinity;
			}
			else
			{
				num = (float)global::System.Math.Abs(levelGridNode.position.y - nodes[levelGridNode.NodeInGridIndex + this.width * this.depth * (layerIndex + 1)].position.y) * 0.001f;
			}
			for (int i = 0; i < 4; i++)
			{
				int num2 = x + this.neighbourXOffsets[i];
				int num3 = z + this.neighbourZOffsets[i];
				if (num2 >= 0 && num3 >= 0 && num2 < this.width && num3 < this.depth)
				{
					int num4 = num3 * this.width + num2;
					int value = 255;
					for (int j = 0; j < this.layerCount; j++)
					{
						global::Pathfinding.GraphNode graphNode = nodes[num4 + this.width * this.depth * j];
						if (graphNode != null && graphNode.Walkable)
						{
							float num5;
							if (j == this.layerCount - 1 || nodes[num4 + this.width * this.depth * (j + 1)] == null)
							{
								num5 = float.PositiveInfinity;
							}
							else
							{
								num5 = (float)global::System.Math.Abs(graphNode.position.y - nodes[num4 + this.width * this.depth * (j + 1)].position.y) * 0.001f;
							}
							float num6 = global::UnityEngine.Mathf.Max((float)graphNode.position.y * 0.001f, (float)levelGridNode.position.y * 0.001f);
							float num7 = global::UnityEngine.Mathf.Min((float)graphNode.position.y * 0.001f + num5, (float)levelGridNode.position.y * 0.001f + num);
							float num8 = num7 - num6;
							if (num8 >= this.characterHeight && (float)global::UnityEngine.Mathf.Abs(graphNode.position.y - levelGridNode.position.y) * 0.001f <= this.maxClimb)
							{
								value = j;
							}
						}
					}
					levelGridNode.SetConnectionValue(i, value);
				}
			}
		}

		public override global::Pathfinding.NNInfoInternal GetNearest(global::UnityEngine.Vector3 position, global::Pathfinding.NNConstraint constraint, global::Pathfinding.GraphNode hint)
		{
			if (this.nodes == null || this.depth * this.width * this.layerCount != this.nodes.Length)
			{
				return default(global::Pathfinding.NNInfoInternal);
			}
			global::UnityEngine.Vector3 vector = this.inverseMatrix.MultiplyPoint3x4(position);
			int x = global::UnityEngine.Mathf.Clamp(global::UnityEngine.Mathf.RoundToInt(vector.x - 0.5f), 0, this.width - 1);
			int z = global::UnityEngine.Mathf.Clamp(global::UnityEngine.Mathf.RoundToInt(vector.z - 0.5f), 0, this.depth - 1);
			global::Pathfinding.LevelGridNode nearestNode = this.GetNearestNode(position, x, z, null);
			return new global::Pathfinding.NNInfoInternal(nearestNode);
		}

		private global::Pathfinding.LevelGridNode GetNearestNode(global::UnityEngine.Vector3 position, int x, int z, global::Pathfinding.NNConstraint constraint)
		{
			int num = this.width * z + x;
			float num2 = float.PositiveInfinity;
			global::Pathfinding.LevelGridNode result = null;
			for (int i = 0; i < this.layerCount; i++)
			{
				global::Pathfinding.LevelGridNode levelGridNode = this.nodes[num + this.width * this.depth * i];
				if (levelGridNode != null)
				{
					float sqrMagnitude = ((global::UnityEngine.Vector3)levelGridNode.position - position).sqrMagnitude;
					if (sqrMagnitude < num2 && (constraint == null || constraint.Suitable(levelGridNode)))
					{
						num2 = sqrMagnitude;
						result = levelGridNode;
					}
				}
			}
			return result;
		}

		public override global::Pathfinding.NNInfoInternal GetNearestForce(global::UnityEngine.Vector3 position, global::Pathfinding.NNConstraint constraint)
		{
			if (this.nodes == null || this.depth * this.width * this.layerCount != this.nodes.Length || this.layerCount == 0)
			{
				return default(global::Pathfinding.NNInfoInternal);
			}
			global::UnityEngine.Vector3 vector = position;
			position = this.inverseMatrix.MultiplyPoint3x4(position);
			int num = global::UnityEngine.Mathf.Clamp(global::UnityEngine.Mathf.RoundToInt(position.x - 0.5f), 0, this.width - 1);
			int num2 = global::UnityEngine.Mathf.Clamp(global::UnityEngine.Mathf.RoundToInt(position.z - 0.5f), 0, this.depth - 1);
			float num3 = float.PositiveInfinity;
			int num4 = 2;
			global::Pathfinding.LevelGridNode levelGridNode = this.GetNearestNode(vector, num, num2, constraint);
			if (levelGridNode != null)
			{
				num3 = ((global::UnityEngine.Vector3)levelGridNode.position - vector).sqrMagnitude;
			}
			if (levelGridNode != null)
			{
				if (num4 == 0)
				{
					return new global::Pathfinding.NNInfoInternal(levelGridNode);
				}
				num4--;
			}
			float num5 = (!constraint.constrainDistance) ? float.PositiveInfinity : global::AstarPath.active.maxNearestNodeDistance;
			float num6 = num5 * num5;
			int num7 = 1;
			for (;;)
			{
				int i = num2 + num7;
				if (this.nodeSize * (float)num7 > num5)
				{
					break;
				}
				int j;
				for (j = num - num7; j <= num + num7; j++)
				{
					if (j >= 0 && i >= 0 && j < this.width && i < this.depth)
					{
						global::Pathfinding.LevelGridNode nearestNode = this.GetNearestNode(vector, j, i, constraint);
						if (nearestNode != null)
						{
							float sqrMagnitude = ((global::UnityEngine.Vector3)nearestNode.position - vector).sqrMagnitude;
							if (sqrMagnitude < num3 && sqrMagnitude < num6)
							{
								num3 = sqrMagnitude;
								levelGridNode = nearestNode;
							}
						}
					}
				}
				i = num2 - num7;
				for (j = num - num7; j <= num + num7; j++)
				{
					if (j >= 0 && i >= 0 && j < this.width && i < this.depth)
					{
						global::Pathfinding.LevelGridNode nearestNode2 = this.GetNearestNode(vector, j, i, constraint);
						if (nearestNode2 != null)
						{
							float sqrMagnitude2 = ((global::UnityEngine.Vector3)nearestNode2.position - vector).sqrMagnitude;
							if (sqrMagnitude2 < num3 && sqrMagnitude2 < num6)
							{
								num3 = sqrMagnitude2;
								levelGridNode = nearestNode2;
							}
						}
					}
				}
				j = num - num7;
				for (i = num2 - num7 + 1; i <= num2 + num7 - 1; i++)
				{
					if (j >= 0 && i >= 0 && j < this.width && i < this.depth)
					{
						global::Pathfinding.LevelGridNode nearestNode3 = this.GetNearestNode(vector, j, i, constraint);
						if (nearestNode3 != null)
						{
							float sqrMagnitude3 = ((global::UnityEngine.Vector3)nearestNode3.position - vector).sqrMagnitude;
							if (sqrMagnitude3 < num3 && sqrMagnitude3 < num6)
							{
								num3 = sqrMagnitude3;
								levelGridNode = nearestNode3;
							}
						}
					}
				}
				j = num + num7;
				for (i = num2 - num7 + 1; i <= num2 + num7 - 1; i++)
				{
					if (j >= 0 && i >= 0 && j < this.width && i < this.depth)
					{
						global::Pathfinding.LevelGridNode nearestNode4 = this.GetNearestNode(vector, j, i, constraint);
						if (nearestNode4 != null)
						{
							float sqrMagnitude4 = ((global::UnityEngine.Vector3)nearestNode4.position - vector).sqrMagnitude;
							if (sqrMagnitude4 < num3 && sqrMagnitude4 < num6)
							{
								num3 = sqrMagnitude4;
								levelGridNode = nearestNode4;
							}
						}
					}
				}
				if (levelGridNode != null)
				{
					if (num4 == 0)
					{
						goto Block_37;
					}
					num4--;
				}
				num7++;
			}
			return new global::Pathfinding.NNInfoInternal(levelGridNode);
			Block_37:
			return new global::Pathfinding.NNInfoInternal(levelGridNode);
		}

		protected override global::Pathfinding.GridNodeBase GetNeighbourAlongDirection(global::Pathfinding.GridNodeBase node, int direction)
		{
			global::Pathfinding.LevelGridNode levelGridNode = node as global::Pathfinding.LevelGridNode;
			if (levelGridNode.GetConnection(direction))
			{
				return this.nodes[levelGridNode.NodeInGridIndex + this.neighbourOffsets[direction] + this.width * this.depth * levelGridNode.GetConnectionValue(direction)];
			}
			return null;
		}

		public static bool CheckConnection(global::Pathfinding.LevelGridNode node, int dir)
		{
			return node.GetConnection(dir);
		}

		public override void OnDrawGizmos(bool drawNodes)
		{
			if (!drawNodes)
			{
				return;
			}
			base.OnDrawGizmos(false);
			if (this.nodes == null)
			{
				return;
			}
			global::Pathfinding.PathHandler debugPathData = global::AstarPath.active.debugPathData;
			for (int i = 0; i < this.nodes.Length; i++)
			{
				global::Pathfinding.LevelGridNode levelGridNode = this.nodes[i];
				if (levelGridNode != null && levelGridNode.Walkable)
				{
					global::UnityEngine.Gizmos.color = this.NodeColor(levelGridNode, global::AstarPath.active.debugPathData);
					if (global::AstarPath.active.showSearchTree && global::AstarPath.active.debugPathData != null)
					{
						if (global::Pathfinding.NavGraph.InSearchTree(levelGridNode, global::AstarPath.active.debugPath))
						{
							global::Pathfinding.PathNode pathNode = debugPathData.GetPathNode(levelGridNode);
							if (pathNode != null && pathNode.parent != null)
							{
								global::UnityEngine.Gizmos.DrawLine((global::UnityEngine.Vector3)levelGridNode.position, (global::UnityEngine.Vector3)pathNode.parent.node.position);
							}
						}
					}
					else
					{
						for (int j = 0; j < 4; j++)
						{
							int connectionValue = levelGridNode.GetConnectionValue(j);
							if (connectionValue != 255)
							{
								int num = levelGridNode.NodeInGridIndex + this.neighbourOffsets[j] + this.width * this.depth * connectionValue;
								if (num >= 0 && num < this.nodes.Length)
								{
									global::Pathfinding.GraphNode graphNode = this.nodes[num];
									if (graphNode != null)
									{
										global::UnityEngine.Gizmos.DrawLine((global::UnityEngine.Vector3)levelGridNode.position, (global::UnityEngine.Vector3)graphNode.position);
									}
								}
							}
						}
					}
				}
			}
		}

		public override void SerializeExtraInfo(global::Pathfinding.Serialization.GraphSerializationContext ctx)
		{
			if (this.nodes == null)
			{
				ctx.writer.Write(-1);
				return;
			}
			ctx.writer.Write(this.nodes.Length);
			for (int i = 0; i < this.nodes.Length; i++)
			{
				if (this.nodes[i] == null)
				{
					ctx.writer.Write(-1);
				}
				else
				{
					ctx.writer.Write(0);
					this.nodes[i].SerializeNode(ctx);
				}
			}
		}

		public override void DeserializeExtraInfo(global::Pathfinding.Serialization.GraphSerializationContext ctx)
		{
			int num = ctx.reader.ReadInt32();
			if (num == -1)
			{
				this.nodes = null;
				return;
			}
			this.nodes = new global::Pathfinding.LevelGridNode[num];
			for (int i = 0; i < this.nodes.Length; i++)
			{
				if (ctx.reader.ReadInt32() != -1)
				{
					this.nodes[i] = new global::Pathfinding.LevelGridNode(this.active);
					this.nodes[i].DeserializeNode(ctx);
				}
				else
				{
					this.nodes[i] = null;
				}
			}
		}

		public override void PostDeserialization()
		{
			base.GenerateMatrix();
			this.lastScannedWidth = this.width;
			this.lastScannedDepth = this.depth;
			this.SetUpOffsetsAndCosts();
			if (this.nodes == null || this.nodes.Length == 0)
			{
				return;
			}
			global::Pathfinding.LevelGridNode.SetGridGraph(global::AstarPath.active.astarData.GetGraphIndex(this), this);
			for (int i = 0; i < this.depth; i++)
			{
				for (int j = 0; j < this.width; j++)
				{
					for (int k = 0; k < this.layerCount; k++)
					{
						global::Pathfinding.LevelGridNode levelGridNode = this.nodes[i * this.width + j + this.width * this.depth * k];
						if (levelGridNode != null)
						{
							levelGridNode.NodeInGridIndex = i * this.width + j;
						}
					}
				}
			}
		}

		[global::Pathfinding.Serialization.JsonMember]
		public int layerCount;

		[global::Pathfinding.Serialization.JsonMember]
		public float mergeSpanRange = 0.5f;

		[global::Pathfinding.Serialization.JsonMember]
		public float characterHeight = 0.4f;

		internal int lastScannedWidth;

		internal int lastScannedDepth;

		public new global::Pathfinding.LevelGridNode[] nodes;
	}
}
