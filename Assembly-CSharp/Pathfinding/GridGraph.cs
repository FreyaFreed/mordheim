using System;
using System.Collections.Generic;
using Pathfinding.Serialization;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding
{
	[global::Pathfinding.Serialization.JsonOptIn]
	public class GridGraph : global::Pathfinding.NavGraph, global::Pathfinding.IUpdatableGraph, global::Pathfinding.IRaycastableGraph
	{
		public GridGraph()
		{
			this.unclampedSize = new global::UnityEngine.Vector2(10f, 10f);
			this.nodeSize = 1f;
			this.collision = new global::Pathfinding.GraphCollision();
		}

		public override void OnDestroy()
		{
			base.OnDestroy();
			this.RemoveGridGraphFromStatic();
		}

		private void RemoveGridGraphFromStatic()
		{
			global::Pathfinding.GridNode.SetGridGraph(global::AstarPath.active.astarData.GetGraphIndex(this), null);
		}

		public virtual bool uniformWidthDepthGrid
		{
			get
			{
				return true;
			}
		}

		public override int CountNodes()
		{
			return this.nodes.Length;
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

		public bool useRaycastNormal
		{
			get
			{
				return global::System.Math.Abs(90f - this.maxSlope) > float.Epsilon;
			}
		}

		public global::UnityEngine.Vector2 size { get; protected set; }

		public global::UnityEngine.Matrix4x4 boundsMatrix { get; protected set; }

		public void RelocateNodes(global::UnityEngine.Vector3 center, global::UnityEngine.Quaternion rotation, float nodeSize, float aspectRatio = 1f, float isometricAngle = 0f)
		{
			global::UnityEngine.Matrix4x4 matrix = this.matrix;
			this.center = center;
			this.rotation = rotation.eulerAngles;
			this.nodeSize = nodeSize;
			this.aspectRatio = aspectRatio;
			this.isometricAngle = isometricAngle;
			this.UpdateSizeFromWidthDepth();
			this.RelocateNodes(matrix, this.matrix);
		}

		public global::Pathfinding.Int3 GraphPointToWorld(int x, int z, float height)
		{
			return (global::Pathfinding.Int3)this.matrix.MultiplyPoint3x4(new global::UnityEngine.Vector3((float)x + 0.5f, height, (float)z + 0.5f));
		}

		public int Width
		{
			get
			{
				return this.width;
			}
			set
			{
				this.width = value;
			}
		}

		public int Depth
		{
			get
			{
				return this.depth;
			}
			set
			{
				this.depth = value;
			}
		}

		public uint GetConnectionCost(int dir)
		{
			return this.neighbourCosts[dir];
		}

		public global::Pathfinding.GridNode GetNodeConnection(global::Pathfinding.GridNode node, int dir)
		{
			if (!node.GetConnectionInternal(dir))
			{
				return null;
			}
			if (!node.EdgeNode)
			{
				return this.nodes[node.NodeInGridIndex + this.neighbourOffsets[dir]];
			}
			int nodeInGridIndex = node.NodeInGridIndex;
			int num = nodeInGridIndex / this.Width;
			int x = nodeInGridIndex - num * this.Width;
			return this.GetNodeConnection(nodeInGridIndex, x, num, dir);
		}

		public bool HasNodeConnection(global::Pathfinding.GridNode node, int dir)
		{
			if (!node.GetConnectionInternal(dir))
			{
				return false;
			}
			if (!node.EdgeNode)
			{
				return true;
			}
			int nodeInGridIndex = node.NodeInGridIndex;
			int num = nodeInGridIndex / this.Width;
			int x = nodeInGridIndex - num * this.Width;
			return this.HasNodeConnection(nodeInGridIndex, x, num, dir);
		}

		public void SetNodeConnection(global::Pathfinding.GridNode node, int dir, bool value)
		{
			int nodeInGridIndex = node.NodeInGridIndex;
			int num = nodeInGridIndex / this.Width;
			int x = nodeInGridIndex - num * this.Width;
			this.SetNodeConnection(nodeInGridIndex, x, num, dir, value);
		}

		private global::Pathfinding.GridNode GetNodeConnection(int index, int x, int z, int dir)
		{
			if (!this.nodes[index].GetConnectionInternal(dir))
			{
				return null;
			}
			int num = x + this.neighbourXOffsets[dir];
			if (num < 0 || num >= this.Width)
			{
				return null;
			}
			int num2 = z + this.neighbourZOffsets[dir];
			if (num2 < 0 || num2 >= this.Depth)
			{
				return null;
			}
			int num3 = index + this.neighbourOffsets[dir];
			return this.nodes[num3];
		}

		public void SetNodeConnection(int index, int x, int z, int dir, bool value)
		{
			this.nodes[index].SetConnectionInternal(dir, value);
		}

		public bool HasNodeConnection(int index, int x, int z, int dir)
		{
			if (!this.nodes[index].GetConnectionInternal(dir))
			{
				return false;
			}
			int num = x + this.neighbourXOffsets[dir];
			if (num < 0 || num >= this.Width)
			{
				return false;
			}
			int num2 = z + this.neighbourZOffsets[dir];
			return num2 >= 0 && num2 < this.Depth;
		}

		public void UpdateSizeFromWidthDepth()
		{
			this.unclampedSize = new global::UnityEngine.Vector2((float)this.width, (float)this.depth) * this.nodeSize;
			this.GenerateMatrix();
		}

		public void GenerateMatrix()
		{
			global::UnityEngine.Vector2 size = this.unclampedSize;
			size.x *= global::UnityEngine.Mathf.Sign(size.x);
			size.y *= global::UnityEngine.Mathf.Sign(size.y);
			this.nodeSize = global::UnityEngine.Mathf.Clamp(this.nodeSize, size.x / 1024f, float.PositiveInfinity);
			this.nodeSize = global::UnityEngine.Mathf.Clamp(this.nodeSize, size.y / 1024f, float.PositiveInfinity);
			size.x = ((size.x >= this.nodeSize) ? size.x : this.nodeSize);
			size.y = ((size.y >= this.nodeSize) ? size.y : this.nodeSize);
			this.size = size;
			global::UnityEngine.Matrix4x4 rhs = global::UnityEngine.Matrix4x4.TRS(global::UnityEngine.Vector3.zero, global::UnityEngine.Quaternion.Euler(0f, 45f, 0f), global::UnityEngine.Vector3.one);
			rhs = global::UnityEngine.Matrix4x4.Scale(new global::UnityEngine.Vector3(global::UnityEngine.Mathf.Cos(0.0174532924f * this.isometricAngle), 1f, 1f)) * rhs;
			rhs = global::UnityEngine.Matrix4x4.TRS(global::UnityEngine.Vector3.zero, global::UnityEngine.Quaternion.Euler(0f, -45f, 0f), global::UnityEngine.Vector3.one) * rhs;
			this.boundsMatrix = global::UnityEngine.Matrix4x4.TRS(this.center, global::UnityEngine.Quaternion.Euler(this.rotation), new global::UnityEngine.Vector3(this.aspectRatio, 1f, 1f)) * rhs;
			this.width = global::UnityEngine.Mathf.FloorToInt(this.size.x / this.nodeSize);
			this.depth = global::UnityEngine.Mathf.FloorToInt(this.size.y / this.nodeSize);
			if (global::UnityEngine.Mathf.Approximately(this.size.x / this.nodeSize, (float)global::UnityEngine.Mathf.CeilToInt(this.size.x / this.nodeSize)))
			{
				this.width = global::UnityEngine.Mathf.CeilToInt(this.size.x / this.nodeSize);
			}
			if (global::UnityEngine.Mathf.Approximately(this.size.y / this.nodeSize, (float)global::UnityEngine.Mathf.CeilToInt(this.size.y / this.nodeSize)))
			{
				this.depth = global::UnityEngine.Mathf.CeilToInt(this.size.y / this.nodeSize);
			}
			global::UnityEngine.Matrix4x4 matrix = global::UnityEngine.Matrix4x4.TRS(this.boundsMatrix.MultiplyPoint3x4(-new global::UnityEngine.Vector3(this.size.x, 0f, this.size.y) * 0.5f), global::UnityEngine.Quaternion.Euler(this.rotation), new global::UnityEngine.Vector3(this.nodeSize * this.aspectRatio, 1f, this.nodeSize)) * rhs;
			base.SetMatrix(matrix);
		}

		public override global::Pathfinding.NNInfoInternal GetNearest(global::UnityEngine.Vector3 position, global::Pathfinding.NNConstraint constraint, global::Pathfinding.GraphNode hint)
		{
			if (this.nodes == null || this.depth * this.width != this.nodes.Length)
			{
				return default(global::Pathfinding.NNInfoInternal);
			}
			position = this.inverseMatrix.MultiplyPoint3x4(position);
			float num = position.x - 0.5f;
			float num2 = position.z - 0.5f;
			int num3 = global::UnityEngine.Mathf.Clamp(global::UnityEngine.Mathf.RoundToInt(num), 0, this.width - 1);
			int num4 = global::UnityEngine.Mathf.Clamp(global::UnityEngine.Mathf.RoundToInt(num2), 0, this.depth - 1);
			global::Pathfinding.NNInfoInternal result = new global::Pathfinding.NNInfoInternal(this.nodes[num4 * this.width + num3]);
			float y = this.inverseMatrix.MultiplyPoint3x4((global::UnityEngine.Vector3)this.nodes[num4 * this.width + num3].position).y;
			result.clampedPosition = this.matrix.MultiplyPoint3x4(new global::UnityEngine.Vector3(global::UnityEngine.Mathf.Clamp(num, (float)num3 - 0.5f, (float)num3 + 0.5f) + 0.5f, y, global::UnityEngine.Mathf.Clamp(num2, (float)num4 - 0.5f, (float)num4 + 0.5f) + 0.5f));
			return result;
		}

		public override global::Pathfinding.NNInfoInternal GetNearestForce(global::UnityEngine.Vector3 position, global::Pathfinding.NNConstraint constraint)
		{
			if (this.nodes == null || this.depth * this.width != this.nodes.Length)
			{
				return default(global::Pathfinding.NNInfoInternal);
			}
			global::UnityEngine.Vector3 b = position;
			position = this.inverseMatrix.MultiplyPoint3x4(position);
			float num = position.x - 0.5f;
			float num2 = position.z - 0.5f;
			int num3 = global::UnityEngine.Mathf.Clamp(global::UnityEngine.Mathf.RoundToInt(num), 0, this.width - 1);
			int num4 = global::UnityEngine.Mathf.Clamp(global::UnityEngine.Mathf.RoundToInt(num2), 0, this.depth - 1);
			global::Pathfinding.GridNode gridNode = this.nodes[num3 + num4 * this.width];
			global::Pathfinding.GridNode gridNode2 = null;
			float num5 = float.PositiveInfinity;
			int num6 = 2;
			global::UnityEngine.Vector3 clampedPosition = global::UnityEngine.Vector3.zero;
			global::Pathfinding.NNInfoInternal result = new global::Pathfinding.NNInfoInternal(null);
			if (constraint.Suitable(gridNode))
			{
				gridNode2 = gridNode;
				num5 = ((global::UnityEngine.Vector3)gridNode2.position - b).sqrMagnitude;
				float y = this.inverseMatrix.MultiplyPoint3x4((global::UnityEngine.Vector3)gridNode.position).y;
				clampedPosition = this.matrix.MultiplyPoint3x4(new global::UnityEngine.Vector3(global::UnityEngine.Mathf.Clamp(num, (float)num3 - 0.5f, (float)num3 + 0.5f) + 0.5f, y, global::UnityEngine.Mathf.Clamp(num2, (float)num4 - 0.5f, (float)num4 + 0.5f) + 0.5f));
			}
			if (gridNode2 != null)
			{
				result.node = gridNode2;
				result.clampedPosition = clampedPosition;
				if (num6 == 0)
				{
					return result;
				}
				num6--;
			}
			float num7 = (!constraint.constrainDistance) ? float.PositiveInfinity : global::AstarPath.active.maxNearestNodeDistance;
			float num8 = num7 * num7;
			int num9 = 1;
			while (this.nodeSize * (float)num9 <= num7)
			{
				bool flag = false;
				int i = num4 + num9;
				int num10 = i * this.width;
				int j;
				for (j = num3 - num9; j <= num3 + num9; j++)
				{
					if (j >= 0 && i >= 0 && j < this.width && i < this.depth)
					{
						flag = true;
						if (constraint.Suitable(this.nodes[j + num10]))
						{
							float sqrMagnitude = ((global::UnityEngine.Vector3)this.nodes[j + num10].position - b).sqrMagnitude;
							if (sqrMagnitude < num5 && sqrMagnitude < num8)
							{
								num5 = sqrMagnitude;
								gridNode2 = this.nodes[j + num10];
								clampedPosition = this.matrix.MultiplyPoint3x4(new global::UnityEngine.Vector3(global::UnityEngine.Mathf.Clamp(num, (float)j - 0.5f, (float)j + 0.5f) + 0.5f, this.inverseMatrix.MultiplyPoint3x4((global::UnityEngine.Vector3)gridNode2.position).y, global::UnityEngine.Mathf.Clamp(num2, (float)i - 0.5f, (float)i + 0.5f) + 0.5f));
							}
						}
					}
				}
				i = num4 - num9;
				num10 = i * this.width;
				for (j = num3 - num9; j <= num3 + num9; j++)
				{
					if (j >= 0 && i >= 0 && j < this.width && i < this.depth)
					{
						flag = true;
						if (constraint.Suitable(this.nodes[j + num10]))
						{
							float sqrMagnitude2 = ((global::UnityEngine.Vector3)this.nodes[j + num10].position - b).sqrMagnitude;
							if (sqrMagnitude2 < num5 && sqrMagnitude2 < num8)
							{
								num5 = sqrMagnitude2;
								gridNode2 = this.nodes[j + num10];
								clampedPosition = this.matrix.MultiplyPoint3x4(new global::UnityEngine.Vector3(global::UnityEngine.Mathf.Clamp(num, (float)j - 0.5f, (float)j + 0.5f) + 0.5f, this.inverseMatrix.MultiplyPoint3x4((global::UnityEngine.Vector3)gridNode2.position).y, global::UnityEngine.Mathf.Clamp(num2, (float)i - 0.5f, (float)i + 0.5f) + 0.5f));
							}
						}
					}
				}
				j = num3 - num9;
				for (i = num4 - num9 + 1; i <= num4 + num9 - 1; i++)
				{
					if (j >= 0 && i >= 0 && j < this.width && i < this.depth)
					{
						flag = true;
						if (constraint.Suitable(this.nodes[j + i * this.width]))
						{
							float sqrMagnitude3 = ((global::UnityEngine.Vector3)this.nodes[j + i * this.width].position - b).sqrMagnitude;
							if (sqrMagnitude3 < num5 && sqrMagnitude3 < num8)
							{
								num5 = sqrMagnitude3;
								gridNode2 = this.nodes[j + i * this.width];
								clampedPosition = this.matrix.MultiplyPoint3x4(new global::UnityEngine.Vector3(global::UnityEngine.Mathf.Clamp(num, (float)j - 0.5f, (float)j + 0.5f) + 0.5f, this.inverseMatrix.MultiplyPoint3x4((global::UnityEngine.Vector3)gridNode2.position).y, global::UnityEngine.Mathf.Clamp(num2, (float)i - 0.5f, (float)i + 0.5f) + 0.5f));
							}
						}
					}
				}
				j = num3 + num9;
				for (i = num4 - num9 + 1; i <= num4 + num9 - 1; i++)
				{
					if (j >= 0 && i >= 0 && j < this.width && i < this.depth)
					{
						flag = true;
						if (constraint.Suitable(this.nodes[j + i * this.width]))
						{
							float sqrMagnitude4 = ((global::UnityEngine.Vector3)this.nodes[j + i * this.width].position - b).sqrMagnitude;
							if (sqrMagnitude4 < num5 && sqrMagnitude4 < num8)
							{
								num5 = sqrMagnitude4;
								gridNode2 = this.nodes[j + i * this.width];
								clampedPosition = this.matrix.MultiplyPoint3x4(new global::UnityEngine.Vector3(global::UnityEngine.Mathf.Clamp(num, (float)j - 0.5f, (float)j + 0.5f) + 0.5f, this.inverseMatrix.MultiplyPoint3x4((global::UnityEngine.Vector3)gridNode2.position).y, global::UnityEngine.Mathf.Clamp(num2, (float)i - 0.5f, (float)i + 0.5f) + 0.5f));
							}
						}
					}
				}
				if (gridNode2 != null)
				{
					if (num6 == 0)
					{
						result.node = gridNode2;
						result.clampedPosition = clampedPosition;
						return result;
					}
					num6--;
				}
				if (!flag)
				{
					result.node = gridNode2;
					result.clampedPosition = clampedPosition;
					return result;
				}
				num9++;
			}
			result.node = gridNode2;
			result.clampedPosition = clampedPosition;
			return result;
		}

		public virtual void SetUpOffsetsAndCosts()
		{
			this.neighbourOffsets[0] = -this.width;
			this.neighbourOffsets[1] = 1;
			this.neighbourOffsets[2] = this.width;
			this.neighbourOffsets[3] = -1;
			this.neighbourOffsets[4] = -this.width + 1;
			this.neighbourOffsets[5] = this.width + 1;
			this.neighbourOffsets[6] = this.width - 1;
			this.neighbourOffsets[7] = -this.width - 1;
			uint num = (uint)global::UnityEngine.Mathf.RoundToInt(this.nodeSize * 1000f);
			uint num2 = (uint)((!this.uniformEdgeCosts) ? global::UnityEngine.Mathf.RoundToInt(this.nodeSize * global::UnityEngine.Mathf.Sqrt(2f) * 1000f) : ((int)num));
			this.neighbourCosts[0] = num;
			this.neighbourCosts[1] = num;
			this.neighbourCosts[2] = num;
			this.neighbourCosts[3] = num;
			this.neighbourCosts[4] = num2;
			this.neighbourCosts[5] = num2;
			this.neighbourCosts[6] = num2;
			this.neighbourCosts[7] = num2;
			this.neighbourXOffsets[0] = 0;
			this.neighbourXOffsets[1] = 1;
			this.neighbourXOffsets[2] = 0;
			this.neighbourXOffsets[3] = -1;
			this.neighbourXOffsets[4] = 1;
			this.neighbourXOffsets[5] = 1;
			this.neighbourXOffsets[6] = -1;
			this.neighbourXOffsets[7] = -1;
			this.neighbourZOffsets[0] = -1;
			this.neighbourZOffsets[1] = 0;
			this.neighbourZOffsets[2] = 1;
			this.neighbourZOffsets[3] = 0;
			this.neighbourZOffsets[4] = -1;
			this.neighbourZOffsets[5] = 1;
			this.neighbourZOffsets[6] = 1;
			this.neighbourZOffsets[7] = -1;
		}

		public override global::System.Collections.Generic.IEnumerable<global::Pathfinding.Progress> ScanInternal()
		{
			global::AstarPath.OnPostScan = (global::Pathfinding.OnScanDelegate)global::System.Delegate.Combine(global::AstarPath.OnPostScan, new global::Pathfinding.OnScanDelegate(this.OnPostScan));
			if (this.nodeSize <= 0f)
			{
				yield break;
			}
			this.GenerateMatrix();
			if (this.width > 1024 || this.depth > 1024)
			{
				global::UnityEngine.Debug.LogError("One of the grid's sides is longer than 1024 nodes");
				yield break;
			}
			if (this.useJumpPointSearch)
			{
				global::UnityEngine.Debug.LogError("Trying to use Jump Point Search, but support for it is not enabled. Please enable it in the inspector (Grid Graph settings).");
			}
			this.SetUpOffsetsAndCosts();
			int graphIndex = global::AstarPath.active.astarData.GetGraphIndex(this);
			global::Pathfinding.GridNode.SetGridGraph(graphIndex, this);
			yield return new global::Pathfinding.Progress(0.05f, "Creating nodes");
			this.nodes = new global::Pathfinding.GridNode[this.width * this.depth];
			for (int i = 0; i < this.nodes.Length; i++)
			{
				this.nodes[i] = new global::Pathfinding.GridNode(this.active);
				this.nodes[i].GraphIndex = (uint)graphIndex;
			}
			if (this.collision == null)
			{
				this.collision = new global::Pathfinding.GraphCollision();
			}
			this.collision.Initialize(this.matrix, this.nodeSize);
			this.textureData.Initialize();
			int progressCounter = 0;
			for (int z = 0; z < this.depth; z++)
			{
				if (progressCounter >= 1000)
				{
					progressCounter = 0;
					yield return new global::Pathfinding.Progress(global::UnityEngine.Mathf.Lerp(0.1f, 0.7f, (float)z / (float)this.depth), "Calculating positions");
				}
				progressCounter += this.width;
				for (int x = 0; x < this.width; x++)
				{
					global::Pathfinding.GridNode node = this.nodes[z * this.width + x];
					node.NodeInGridIndex = z * this.width + x;
					this.UpdateNodePositionCollision(node, x, z, true);
					this.textureData.Apply(node, x, z);
				}
			}
			for (int z2 = 0; z2 < this.depth; z2++)
			{
				if (progressCounter >= 1000)
				{
					progressCounter = 0;
					yield return new global::Pathfinding.Progress(global::UnityEngine.Mathf.Lerp(0.1f, 0.7f, (float)z2 / (float)this.depth), "Calculating connections");
				}
				for (int x2 = 0; x2 < this.width; x2++)
				{
					global::Pathfinding.GridNode node2 = this.nodes[z2 * this.width + x2];
					this.CalculateConnections(x2, z2, node2);
				}
			}
			yield return new global::Pathfinding.Progress(0.95f, "Calculating erosion");
			this.ErodeWalkableArea();
			yield break;
		}

		public virtual void UpdateNodePositionCollision(global::Pathfinding.GridNode node, int x, int z, bool resetPenalty = true)
		{
			node.position = this.GraphPointToWorld(x, z, 0f);
			global::UnityEngine.RaycastHit raycastHit;
			bool flag;
			global::UnityEngine.Vector3 ob = this.collision.CheckHeight((global::UnityEngine.Vector3)node.position, out raycastHit, out flag);
			node.position = (global::Pathfinding.Int3)ob;
			if (resetPenalty)
			{
				node.Penalty = this.initialPenalty;
				if (this.penaltyPosition)
				{
					node.Penalty += (uint)global::UnityEngine.Mathf.RoundToInt(((float)node.position.y - this.penaltyPositionOffset) * this.penaltyPositionFactor);
				}
			}
			if (flag && this.useRaycastNormal && this.collision.heightCheck && raycastHit.normal != global::UnityEngine.Vector3.zero)
			{
				float num = global::UnityEngine.Vector3.Dot(raycastHit.normal.normalized, this.collision.up);
				if (this.penaltyAngle && resetPenalty)
				{
					node.Penalty += (uint)global::UnityEngine.Mathf.RoundToInt((1f - global::UnityEngine.Mathf.Pow(num, this.penaltyAnglePower)) * this.penaltyAngleFactor);
				}
				float num2 = global::UnityEngine.Mathf.Cos(this.maxSlope * 0.0174532924f);
				if (num < num2)
				{
					flag = false;
				}
			}
			node.Walkable = (flag && this.collision.Check((global::UnityEngine.Vector3)node.position));
			node.WalkableErosion = node.Walkable;
		}

		public virtual void ErodeWalkableArea()
		{
			this.ErodeWalkableArea(0, 0, this.Width, this.Depth);
		}

		private bool ErosionAnyFalseConnections(global::Pathfinding.GridNode node)
		{
			if (this.neighbours == global::Pathfinding.NumNeighbours.Six)
			{
				for (int i = 0; i < 6; i++)
				{
					if (!this.HasNodeConnection(node, global::Pathfinding.GridGraph.hexagonNeighbourIndices[i]))
					{
						return true;
					}
				}
			}
			else
			{
				for (int j = 0; j < 4; j++)
				{
					if (!this.HasNodeConnection(node, j))
					{
						return true;
					}
				}
			}
			return false;
		}

		public virtual void ErodeWalkableArea(int xmin, int zmin, int xmax, int zmax)
		{
			xmin = global::UnityEngine.Mathf.Clamp(xmin, 0, this.Width);
			xmax = global::UnityEngine.Mathf.Clamp(xmax, 0, this.Width);
			zmin = global::UnityEngine.Mathf.Clamp(zmin, 0, this.Depth);
			zmax = global::UnityEngine.Mathf.Clamp(zmax, 0, this.Depth);
			if (!this.erosionUseTags)
			{
				for (int i = 0; i < this.erodeIterations; i++)
				{
					for (int j = zmin; j < zmax; j++)
					{
						for (int k = xmin; k < xmax; k++)
						{
							global::Pathfinding.GridNode gridNode = this.nodes[j * this.Width + k];
							if (gridNode.Walkable && this.ErosionAnyFalseConnections(gridNode))
							{
								gridNode.Walkable = false;
							}
						}
					}
					for (int l = zmin; l < zmax; l++)
					{
						for (int m = xmin; m < xmax; m++)
						{
							global::Pathfinding.GridNode node = this.nodes[l * this.Width + m];
							this.CalculateConnections(m, l, node);
						}
					}
				}
			}
			else
			{
				if (this.erodeIterations + this.erosionFirstTag > 31)
				{
					global::UnityEngine.Debug.LogError(string.Concat(new object[]
					{
						"Too few tags available for ",
						this.erodeIterations,
						" erode iterations and starting with tag ",
						this.erosionFirstTag,
						" (erodeIterations+erosionFirstTag > 31)"
					}));
					return;
				}
				if (this.erosionFirstTag <= 0)
				{
					global::UnityEngine.Debug.LogError("First erosion tag must be greater or equal to 1");
					return;
				}
				for (int n = 0; n < this.erodeIterations; n++)
				{
					for (int num = zmin; num < zmax; num++)
					{
						for (int num2 = xmin; num2 < xmax; num2++)
						{
							global::Pathfinding.GridNode gridNode2 = this.nodes[num * this.width + num2];
							if (gridNode2.Walkable && (ulong)gridNode2.Tag >= (ulong)((long)this.erosionFirstTag) && (ulong)gridNode2.Tag < (ulong)((long)(this.erosionFirstTag + n)))
							{
								if (this.neighbours == global::Pathfinding.NumNeighbours.Six)
								{
									for (int num3 = 0; num3 < 6; num3++)
									{
										global::Pathfinding.GridNode nodeConnection = this.GetNodeConnection(gridNode2, global::Pathfinding.GridGraph.hexagonNeighbourIndices[num3]);
										if (nodeConnection != null)
										{
											uint tag = nodeConnection.Tag;
											if ((ulong)tag > (ulong)((long)(this.erosionFirstTag + n)) || (ulong)tag < (ulong)((long)this.erosionFirstTag))
											{
												nodeConnection.Tag = (uint)(this.erosionFirstTag + n);
											}
										}
									}
								}
								else
								{
									for (int num4 = 0; num4 < 4; num4++)
									{
										global::Pathfinding.GridNode nodeConnection2 = this.GetNodeConnection(gridNode2, num4);
										if (nodeConnection2 != null)
										{
											uint tag2 = nodeConnection2.Tag;
											if ((ulong)tag2 > (ulong)((long)(this.erosionFirstTag + n)) || (ulong)tag2 < (ulong)((long)this.erosionFirstTag))
											{
												nodeConnection2.Tag = (uint)(this.erosionFirstTag + n);
											}
										}
									}
								}
							}
							else if (gridNode2.Walkable && n == 0 && this.ErosionAnyFalseConnections(gridNode2))
							{
								gridNode2.Tag = (uint)(this.erosionFirstTag + n);
							}
						}
					}
				}
			}
		}

		public virtual bool IsValidConnection(global::Pathfinding.GridNode n1, global::Pathfinding.GridNode n2)
		{
			return n1.Walkable && n2.Walkable && (this.maxClimb <= 0f || (float)global::System.Math.Abs(n1.position[this.maxClimbAxis] - n2.position[this.maxClimbAxis]) <= this.maxClimb * 1000f);
		}

		public static void CalculateConnections(global::Pathfinding.GridNode node)
		{
			global::Pathfinding.GridGraph gridGraph = global::Pathfinding.AstarData.GetGraph(node) as global::Pathfinding.GridGraph;
			if (gridGraph != null)
			{
				int nodeInGridIndex = node.NodeInGridIndex;
				int x = nodeInGridIndex % gridGraph.width;
				int z = nodeInGridIndex / gridGraph.width;
				gridGraph.CalculateConnections(x, z, node);
			}
		}

		[global::System.Obsolete("CalculateConnections no longer takes a node array, it just uses the one on the graph")]
		public virtual void CalculateConnections(global::Pathfinding.GridNode[] nodes, int x, int z, global::Pathfinding.GridNode node)
		{
			this.CalculateConnections(x, z, node);
		}

		public virtual void CalculateConnections(int x, int z, global::Pathfinding.GridNode node)
		{
			if (!node.Walkable)
			{
				node.ResetConnectionsInternal();
				return;
			}
			int nodeInGridIndex = node.NodeInGridIndex;
			if (this.neighbours == global::Pathfinding.NumNeighbours.Four || this.neighbours == global::Pathfinding.NumNeighbours.Eight)
			{
				int num = 0;
				for (int i = 0; i < 4; i++)
				{
					int num2 = x + this.neighbourXOffsets[i];
					int num3 = z + this.neighbourZOffsets[i];
					if (num2 >= 0 & num3 >= 0 & num2 < this.width & num3 < this.depth)
					{
						global::Pathfinding.GridNode n = this.nodes[nodeInGridIndex + this.neighbourOffsets[i]];
						if (this.IsValidConnection(node, n))
						{
							num |= 1 << i;
						}
					}
				}
				int num4 = 0;
				if (this.neighbours == global::Pathfinding.NumNeighbours.Eight)
				{
					if (this.cutCorners)
					{
						for (int j = 0; j < 4; j++)
						{
							if (((num >> j | num >> j + 1 | num >> j + 1 - 4) & 1) != 0)
							{
								int num5 = j + 4;
								int num6 = x + this.neighbourXOffsets[num5];
								int num7 = z + this.neighbourZOffsets[num5];
								if (num6 >= 0 & num7 >= 0 & num6 < this.width & num7 < this.depth)
								{
									global::Pathfinding.GridNode n2 = this.nodes[nodeInGridIndex + this.neighbourOffsets[num5]];
									if (this.IsValidConnection(node, n2))
									{
										num4 |= 1 << num5;
									}
								}
							}
						}
					}
					else
					{
						for (int k = 0; k < 4; k++)
						{
							if ((num >> k & 1) != 0 && ((num >> k + 1 | num >> k + 1 - 4) & 1) != 0)
							{
								global::Pathfinding.GridNode n3 = this.nodes[nodeInGridIndex + this.neighbourOffsets[k + 4]];
								if (this.IsValidConnection(node, n3))
								{
									num4 |= 1 << k + 4;
								}
							}
						}
					}
				}
				node.SetAllConnectionInternal(num | num4);
			}
			else
			{
				node.ResetConnectionsInternal();
				for (int l = 0; l < global::Pathfinding.GridGraph.hexagonNeighbourIndices.Length; l++)
				{
					int num8 = global::Pathfinding.GridGraph.hexagonNeighbourIndices[l];
					int num9 = x + this.neighbourXOffsets[num8];
					int num10 = z + this.neighbourZOffsets[num8];
					if (num9 >= 0 & num10 >= 0 & num9 < this.width & num10 < this.depth)
					{
						global::Pathfinding.GridNode n4 = this.nodes[nodeInGridIndex + this.neighbourOffsets[num8]];
						node.SetConnectionInternal(num8, this.IsValidConnection(node, n4));
					}
				}
			}
		}

		public void OnPostScan(global::AstarPath script)
		{
			global::AstarPath.OnPostScan = (global::Pathfinding.OnScanDelegate)global::System.Delegate.Remove(global::AstarPath.OnPostScan, new global::Pathfinding.OnScanDelegate(this.OnPostScan));
			if (!this.autoLinkGrids || this.autoLinkDistLimit <= 0f)
			{
				return;
			}
			throw new global::System.NotSupportedException();
		}

		public override void OnDrawGizmos(bool drawNodes)
		{
			global::UnityEngine.Gizmos.matrix = this.boundsMatrix;
			global::UnityEngine.Gizmos.color = global::UnityEngine.Color.white;
			global::UnityEngine.Gizmos.DrawWireCube(global::UnityEngine.Vector3.zero, new global::UnityEngine.Vector3(this.size.x, 0f, this.size.y));
			global::UnityEngine.Gizmos.matrix = global::UnityEngine.Matrix4x4.identity;
			if (!drawNodes || this.nodes == null || this.nodes.Length != this.width * this.depth)
			{
				return;
			}
			global::Pathfinding.PathHandler debugPathData = global::AstarPath.active.debugPathData;
			bool flag = global::AstarPath.active.showSearchTree && debugPathData != null;
			for (int i = 0; i < this.depth; i++)
			{
				for (int j = 0; j < this.width; j++)
				{
					global::Pathfinding.GridNode gridNode = this.nodes[i * this.width + j];
					if (gridNode.Walkable)
					{
						global::UnityEngine.Gizmos.color = this.NodeColor(gridNode, debugPathData);
						global::UnityEngine.Vector3 from = (global::UnityEngine.Vector3)gridNode.position;
						if (flag)
						{
							if (global::Pathfinding.NavGraph.InSearchTree(gridNode, global::AstarPath.active.debugPath))
							{
								global::Pathfinding.PathNode pathNode = debugPathData.GetPathNode(gridNode);
								if (pathNode != null && pathNode.parent != null)
								{
									global::UnityEngine.Gizmos.DrawLine(from, (global::UnityEngine.Vector3)pathNode.parent.node.position);
								}
							}
						}
						else
						{
							for (int k = 0; k < 8; k++)
							{
								if (gridNode.GetConnectionInternal(k))
								{
									global::Pathfinding.GridNode gridNode2 = this.nodes[gridNode.NodeInGridIndex + this.neighbourOffsets[k]];
									global::UnityEngine.Gizmos.DrawLine(from, (global::UnityEngine.Vector3)gridNode2.position);
								}
							}
							if (gridNode.connections != null)
							{
								for (int l = 0; l < gridNode.connections.Length; l++)
								{
									global::Pathfinding.GraphNode graphNode = gridNode.connections[l];
									global::UnityEngine.Gizmos.DrawLine(from, (global::UnityEngine.Vector3)graphNode.position);
								}
							}
						}
					}
				}
			}
		}

		protected static void GetBoundsMinMax(global::UnityEngine.Bounds b, global::UnityEngine.Matrix4x4 matrix, out global::UnityEngine.Vector3 min, out global::UnityEngine.Vector3 max)
		{
			global::UnityEngine.Vector3[] array = new global::UnityEngine.Vector3[]
			{
				matrix.MultiplyPoint3x4(b.center + new global::UnityEngine.Vector3(b.extents.x, b.extents.y, b.extents.z)),
				matrix.MultiplyPoint3x4(b.center + new global::UnityEngine.Vector3(b.extents.x, b.extents.y, -b.extents.z)),
				matrix.MultiplyPoint3x4(b.center + new global::UnityEngine.Vector3(b.extents.x, -b.extents.y, b.extents.z)),
				matrix.MultiplyPoint3x4(b.center + new global::UnityEngine.Vector3(b.extents.x, -b.extents.y, -b.extents.z)),
				matrix.MultiplyPoint3x4(b.center + new global::UnityEngine.Vector3(-b.extents.x, b.extents.y, b.extents.z)),
				matrix.MultiplyPoint3x4(b.center + new global::UnityEngine.Vector3(-b.extents.x, b.extents.y, -b.extents.z)),
				matrix.MultiplyPoint3x4(b.center + new global::UnityEngine.Vector3(-b.extents.x, -b.extents.y, b.extents.z)),
				matrix.MultiplyPoint3x4(b.center + new global::UnityEngine.Vector3(-b.extents.x, -b.extents.y, -b.extents.z))
			};
			min = array[0];
			max = array[0];
			for (int i = 1; i < 8; i++)
			{
				min = global::UnityEngine.Vector3.Min(min, array[i]);
				max = global::UnityEngine.Vector3.Max(max, array[i]);
			}
		}

		public global::System.Collections.Generic.List<global::Pathfinding.GraphNode> GetNodesInArea(global::UnityEngine.Bounds b)
		{
			return this.GetNodesInArea(b, null);
		}

		public global::System.Collections.Generic.List<global::Pathfinding.GraphNode> GetNodesInArea(global::Pathfinding.GraphUpdateShape shape)
		{
			return this.GetNodesInArea(shape.GetBounds(), shape);
		}

		private global::System.Collections.Generic.List<global::Pathfinding.GraphNode> GetNodesInArea(global::UnityEngine.Bounds b, global::Pathfinding.GraphUpdateShape shape)
		{
			if (this.nodes == null || this.width * this.depth != this.nodes.Length)
			{
				return null;
			}
			global::System.Collections.Generic.List<global::Pathfinding.GraphNode> list = global::Pathfinding.Util.ListPool<global::Pathfinding.GraphNode>.Claim();
			global::UnityEngine.Vector3 vector;
			global::UnityEngine.Vector3 vector2;
			global::Pathfinding.GridGraph.GetBoundsMinMax(b, this.inverseMatrix, out vector, out vector2);
			int xmin = global::UnityEngine.Mathf.RoundToInt(vector.x - 0.5f);
			int xmax = global::UnityEngine.Mathf.RoundToInt(vector2.x - 0.5f);
			int ymin = global::UnityEngine.Mathf.RoundToInt(vector.z - 0.5f);
			int ymax = global::UnityEngine.Mathf.RoundToInt(vector2.z - 0.5f);
			global::Pathfinding.IntRect a = new global::Pathfinding.IntRect(xmin, ymin, xmax, ymax);
			global::Pathfinding.IntRect b2 = new global::Pathfinding.IntRect(0, 0, this.width - 1, this.depth - 1);
			global::Pathfinding.IntRect intRect = global::Pathfinding.IntRect.Intersection(a, b2);
			for (int i = intRect.xmin; i <= intRect.xmax; i++)
			{
				for (int j = intRect.ymin; j <= intRect.ymax; j++)
				{
					int num = j * this.width + i;
					global::Pathfinding.GraphNode graphNode = this.nodes[num];
					if (b.Contains((global::UnityEngine.Vector3)graphNode.position) && (shape == null || shape.Contains((global::UnityEngine.Vector3)graphNode.position)))
					{
						list.Add(graphNode);
					}
				}
			}
			return list;
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
			if (this.nodes == null || this.nodes.Length != this.width * this.depth)
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
			int num = (!o.updateErosion) ? 0 : this.erodeIterations;
			bool flag = o.updatePhysics || o.modifyWalkability;
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
			for (int i = intRect4.xmin; i <= intRect4.xmax; i++)
			{
				for (int j = intRect4.ymin; j <= intRect4.ymax; j++)
				{
					o.WillUpdateNode(this.nodes[j * this.width + i]);
				}
			}
			if (o.updatePhysics && !o.modifyWalkability)
			{
				this.collision.Initialize(this.matrix, this.nodeSize);
				intRect4 = global::Pathfinding.IntRect.Intersection(intRect3, b);
				for (int k = intRect4.xmin; k <= intRect4.xmax; k++)
				{
					for (int l = intRect4.ymin; l <= intRect4.ymax; l++)
					{
						int num2 = l * this.width + k;
						global::Pathfinding.GridNode node = this.nodes[num2];
						this.UpdateNodePositionCollision(node, k, l, o.resetPenaltyOnPhysics);
					}
				}
			}
			intRect4 = global::Pathfinding.IntRect.Intersection(intRect, b);
			for (int m = intRect4.xmin; m <= intRect4.xmax; m++)
			{
				for (int n = intRect4.ymin; n <= intRect4.ymax; n++)
				{
					int num3 = n * this.width + m;
					global::Pathfinding.GridNode gridNode = this.nodes[num3];
					if (flag)
					{
						gridNode.Walkable = gridNode.WalkableErosion;
						if (o.bounds.Contains((global::UnityEngine.Vector3)gridNode.position))
						{
							o.Apply(gridNode);
						}
						gridNode.WalkableErosion = gridNode.Walkable;
					}
					else if (o.bounds.Contains((global::UnityEngine.Vector3)gridNode.position))
					{
						o.Apply(gridNode);
					}
				}
			}
			if (flag && num == 0)
			{
				intRect4 = global::Pathfinding.IntRect.Intersection(intRect2, b);
				for (int num4 = intRect4.xmin; num4 <= intRect4.xmax; num4++)
				{
					for (int num5 = intRect4.ymin; num5 <= intRect4.ymax; num5++)
					{
						int num6 = num5 * this.width + num4;
						global::Pathfinding.GridNode node2 = this.nodes[num6];
						this.CalculateConnections(num4, num5, node2);
					}
				}
			}
			else if (flag && num > 0)
			{
				global::Pathfinding.IntRect a4 = global::Pathfinding.IntRect.Union(intRect, intRect3).Expand(num);
				global::Pathfinding.IntRect a5 = a4.Expand(num);
				a4 = global::Pathfinding.IntRect.Intersection(a4, b);
				a5 = global::Pathfinding.IntRect.Intersection(a5, b);
				for (int num7 = a5.xmin; num7 <= a5.xmax; num7++)
				{
					for (int num8 = a5.ymin; num8 <= a5.ymax; num8++)
					{
						int num9 = num8 * this.width + num7;
						global::Pathfinding.GridNode gridNode2 = this.nodes[num9];
						bool walkable = gridNode2.Walkable;
						gridNode2.Walkable = gridNode2.WalkableErosion;
						if (!a4.Contains(num7, num8))
						{
							gridNode2.TmpWalkable = walkable;
						}
					}
				}
				for (int num10 = a5.xmin; num10 <= a5.xmax; num10++)
				{
					for (int num11 = a5.ymin; num11 <= a5.ymax; num11++)
					{
						int num12 = num11 * this.width + num10;
						global::Pathfinding.GridNode node3 = this.nodes[num12];
						this.CalculateConnections(num10, num11, node3);
					}
				}
				this.ErodeWalkableArea(a5.xmin, a5.ymin, a5.xmax + 1, a5.ymax + 1);
				for (int num13 = a5.xmin; num13 <= a5.xmax; num13++)
				{
					for (int num14 = a5.ymin; num14 <= a5.ymax; num14++)
					{
						if (!a4.Contains(num13, num14))
						{
							int num15 = num14 * this.width + num13;
							global::Pathfinding.GridNode gridNode3 = this.nodes[num15];
							gridNode3.Walkable = gridNode3.TmpWalkable;
						}
					}
				}
				for (int num16 = a5.xmin; num16 <= a5.xmax; num16++)
				{
					for (int num17 = a5.ymin; num17 <= a5.ymax; num17++)
					{
						int num18 = num17 * this.width + num16;
						global::Pathfinding.GridNode node4 = this.nodes[num18];
						this.CalculateConnections(num16, num17, node4);
					}
				}
			}
		}

		public bool Linecast(global::UnityEngine.Vector3 _a, global::UnityEngine.Vector3 _b)
		{
			global::Pathfinding.GraphHitInfo graphHitInfo;
			return this.Linecast(_a, _b, null, out graphHitInfo);
		}

		public bool Linecast(global::UnityEngine.Vector3 _a, global::UnityEngine.Vector3 _b, global::Pathfinding.GraphNode hint)
		{
			global::Pathfinding.GraphHitInfo graphHitInfo;
			return this.Linecast(_a, _b, hint, out graphHitInfo);
		}

		public bool Linecast(global::UnityEngine.Vector3 _a, global::UnityEngine.Vector3 _b, global::Pathfinding.GraphNode hint, out global::Pathfinding.GraphHitInfo hit)
		{
			return this.Linecast(_a, _b, hint, out hit, null);
		}

		protected static float CrossMagnitude(global::UnityEngine.Vector2 a, global::UnityEngine.Vector2 b)
		{
			return a.x * b.y - b.x * a.y;
		}

		protected virtual global::Pathfinding.GridNodeBase GetNeighbourAlongDirection(global::Pathfinding.GridNodeBase node, int direction)
		{
			global::Pathfinding.GridNode gridNode = node as global::Pathfinding.GridNode;
			if (gridNode.GetConnectionInternal(direction))
			{
				return this.nodes[gridNode.NodeInGridIndex + this.neighbourOffsets[direction]];
			}
			return null;
		}

		protected bool ClipLineSegmentToBounds(global::UnityEngine.Vector3 a, global::UnityEngine.Vector3 b, out global::UnityEngine.Vector3 outA, out global::UnityEngine.Vector3 outB)
		{
			if (a.x < 0f || a.z < 0f || a.x > (float)this.width || a.z > (float)this.depth || b.x < 0f || b.z < 0f || b.x > (float)this.width || b.z > (float)this.depth)
			{
				global::UnityEngine.Vector3 vector = new global::UnityEngine.Vector3(0f, 0f, 0f);
				global::UnityEngine.Vector3 vector2 = new global::UnityEngine.Vector3(0f, 0f, (float)this.depth);
				global::UnityEngine.Vector3 vector3 = new global::UnityEngine.Vector3((float)this.width, 0f, (float)this.depth);
				global::UnityEngine.Vector3 vector4 = new global::UnityEngine.Vector3((float)this.width, 0f, 0f);
				int num = 0;
				bool flag;
				global::UnityEngine.Vector3 vector5 = global::Pathfinding.VectorMath.SegmentIntersectionPointXZ(a, b, vector, vector2, out flag);
				if (flag)
				{
					num++;
					if (!global::Pathfinding.VectorMath.RightOrColinearXZ(vector, vector2, a))
					{
						a = vector5;
					}
					else
					{
						b = vector5;
					}
				}
				vector5 = global::Pathfinding.VectorMath.SegmentIntersectionPointXZ(a, b, vector2, vector3, out flag);
				if (flag)
				{
					num++;
					if (!global::Pathfinding.VectorMath.RightOrColinearXZ(vector2, vector3, a))
					{
						a = vector5;
					}
					else
					{
						b = vector5;
					}
				}
				vector5 = global::Pathfinding.VectorMath.SegmentIntersectionPointXZ(a, b, vector3, vector4, out flag);
				if (flag)
				{
					num++;
					if (!global::Pathfinding.VectorMath.RightOrColinearXZ(vector3, vector4, a))
					{
						a = vector5;
					}
					else
					{
						b = vector5;
					}
				}
				vector5 = global::Pathfinding.VectorMath.SegmentIntersectionPointXZ(a, b, vector4, vector, out flag);
				if (flag)
				{
					num++;
					if (!global::Pathfinding.VectorMath.RightOrColinearXZ(vector4, vector, a))
					{
						a = vector5;
					}
					else
					{
						b = vector5;
					}
				}
				if (num == 0)
				{
					outA = global::UnityEngine.Vector3.zero;
					outB = global::UnityEngine.Vector3.zero;
					return false;
				}
			}
			outA = a;
			outB = b;
			return true;
		}

		public bool Linecast(global::UnityEngine.Vector3 _a, global::UnityEngine.Vector3 _b, global::Pathfinding.GraphNode hint, out global::Pathfinding.GraphHitInfo hit, global::System.Collections.Generic.List<global::Pathfinding.GraphNode> trace)
		{
			hit = default(global::Pathfinding.GraphHitInfo);
			hit.origin = _a;
			global::UnityEngine.Vector3 vector = this.inverseMatrix.MultiplyPoint3x4(_a);
			global::UnityEngine.Vector3 vector2 = this.inverseMatrix.MultiplyPoint3x4(_b);
			if (!this.ClipLineSegmentToBounds(vector, vector2, out vector, out vector2))
			{
				return false;
			}
			global::Pathfinding.GridNodeBase gridNodeBase = base.GetNearest(this.matrix.MultiplyPoint3x4(vector), global::Pathfinding.NNConstraint.None).node as global::Pathfinding.GridNodeBase;
			global::Pathfinding.GridNodeBase gridNodeBase2 = base.GetNearest(this.matrix.MultiplyPoint3x4(vector2), global::Pathfinding.NNConstraint.None).node as global::Pathfinding.GridNodeBase;
			if (!gridNodeBase.Walkable)
			{
				hit.node = gridNodeBase;
				hit.point = this.matrix.MultiplyPoint3x4(vector);
				hit.tangentOrigin = hit.point;
				return true;
			}
			global::UnityEngine.Vector2 vector3 = new global::UnityEngine.Vector2(vector.x, vector.z);
			global::UnityEngine.Vector2 vector4 = new global::UnityEngine.Vector2(vector2.x, vector2.z);
			vector3 -= global::UnityEngine.Vector2.one * 0.5f;
			vector4 -= global::UnityEngine.Vector2.one * 0.5f;
			if (gridNodeBase == null || gridNodeBase2 == null)
			{
				hit.node = null;
				hit.point = _a;
				return true;
			}
			global::UnityEngine.Vector2 a = vector4 - vector3;
			global::Pathfinding.Int2 @int = new global::Pathfinding.Int2((int)global::UnityEngine.Mathf.Sign(a.x), (int)global::UnityEngine.Mathf.Sign(a.y));
			float num = global::Pathfinding.GridGraph.CrossMagnitude(a, new global::UnityEngine.Vector2((float)@int.x, (float)@int.y)) * 0.5f;
			int num2;
			int num3;
			if (a.y >= 0f)
			{
				if (a.x >= 0f)
				{
					num2 = 1;
					num3 = 2;
				}
				else
				{
					num2 = 2;
					num3 = 3;
				}
			}
			else if (a.x < 0f)
			{
				num2 = 3;
				num3 = 0;
			}
			else
			{
				num2 = 0;
				num3 = 1;
			}
			global::Pathfinding.GridNodeBase gridNodeBase3 = gridNodeBase;
			while (gridNodeBase3.NodeInGridIndex != gridNodeBase2.NodeInGridIndex)
			{
				if (trace != null)
				{
					trace.Add(gridNodeBase3);
				}
				global::UnityEngine.Vector2 a2 = new global::UnityEngine.Vector2((float)(gridNodeBase3.NodeInGridIndex % this.width), (float)(gridNodeBase3.NodeInGridIndex / this.width));
				float num4 = global::Pathfinding.GridGraph.CrossMagnitude(a, a2 - vector3);
				float num5 = num4 + num;
				int num6 = (num5 >= 0f) ? num2 : num3;
				global::Pathfinding.GridNodeBase neighbourAlongDirection = this.GetNeighbourAlongDirection(gridNodeBase3, num6);
				if (neighbourAlongDirection == null)
				{
					global::UnityEngine.Vector2 vector5 = a2 + new global::UnityEngine.Vector2((float)this.neighbourXOffsets[num6], (float)this.neighbourZOffsets[num6]) * 0.5f;
					global::UnityEngine.Vector2 b;
					if (this.neighbourXOffsets[num6] == 0)
					{
						b = new global::UnityEngine.Vector2(1f, 0f);
					}
					else
					{
						b = new global::UnityEngine.Vector2(0f, 1f);
					}
					global::UnityEngine.Vector2 vector6 = global::Pathfinding.VectorMath.LineIntersectionPoint(vector5, vector5 + b, vector3, vector4);
					global::UnityEngine.Vector3 vector7 = this.inverseMatrix.MultiplyPoint3x4((global::UnityEngine.Vector3)gridNodeBase3.position);
					global::UnityEngine.Vector3 v = new global::UnityEngine.Vector3(vector6.x + 0.5f, vector7.y, vector6.y + 0.5f);
					global::UnityEngine.Vector3 v2 = new global::UnityEngine.Vector3(vector5.x + 0.5f, vector7.y, vector5.y + 0.5f);
					hit.point = this.matrix.MultiplyPoint3x4(v);
					hit.tangentOrigin = this.matrix.MultiplyPoint3x4(v2);
					hit.tangent = this.matrix.MultiplyVector(new global::UnityEngine.Vector3(b.x, 0f, b.y));
					hit.node = gridNodeBase3;
					return true;
				}
				gridNodeBase3 = neighbourAlongDirection;
			}
			if (trace != null)
			{
				trace.Add(gridNodeBase3);
			}
			if (gridNodeBase3 == gridNodeBase2)
			{
				return false;
			}
			hit.point = (global::UnityEngine.Vector3)gridNodeBase3.position;
			hit.tangentOrigin = hit.point;
			return true;
		}

		public bool SnappedLinecast(global::UnityEngine.Vector3 a, global::UnityEngine.Vector3 b, global::Pathfinding.GraphNode hint, out global::Pathfinding.GraphHitInfo hit)
		{
			return this.Linecast((global::UnityEngine.Vector3)base.GetNearest(a, global::Pathfinding.NNConstraint.None).node.position, (global::UnityEngine.Vector3)base.GetNearest(b, global::Pathfinding.NNConstraint.None).node.position, hint, out hit);
		}

		public bool CheckConnection(global::Pathfinding.GridNode node, int dir)
		{
			if (this.neighbours == global::Pathfinding.NumNeighbours.Eight || this.neighbours == global::Pathfinding.NumNeighbours.Six || dir < 4)
			{
				return this.HasNodeConnection(node, dir);
			}
			int num = dir - 4 - 1 & 3;
			int num2 = dir - 4 + 1 & 3;
			if (!this.HasNodeConnection(node, num) || !this.HasNodeConnection(node, num2))
			{
				return false;
			}
			global::Pathfinding.GridNode gridNode = this.nodes[node.NodeInGridIndex + this.neighbourOffsets[num]];
			global::Pathfinding.GridNode gridNode2 = this.nodes[node.NodeInGridIndex + this.neighbourOffsets[num2]];
			return gridNode.Walkable && gridNode2.Walkable && this.HasNodeConnection(gridNode2, num) && this.HasNodeConnection(gridNode, num2);
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
				this.nodes[i].SerializeNode(ctx);
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
			this.nodes = new global::Pathfinding.GridNode[num];
			for (int i = 0; i < this.nodes.Length; i++)
			{
				this.nodes[i] = new global::Pathfinding.GridNode(this.active);
				this.nodes[i].DeserializeNode(ctx);
			}
		}

		public override void DeserializeSettingsCompatibility(global::Pathfinding.Serialization.GraphSerializationContext ctx)
		{
			base.DeserializeSettingsCompatibility(ctx);
			this.aspectRatio = ctx.reader.ReadSingle();
			this.rotation = ctx.DeserializeVector3();
			this.center = ctx.DeserializeVector3();
			this.unclampedSize = ctx.DeserializeVector3();
			this.nodeSize = ctx.reader.ReadSingle();
			this.collision.DeserializeSettingsCompatibility(ctx);
			this.maxClimb = ctx.reader.ReadSingle();
			this.maxClimbAxis = ctx.reader.ReadInt32();
			this.maxSlope = ctx.reader.ReadSingle();
			this.erodeIterations = ctx.reader.ReadInt32();
			this.erosionUseTags = ctx.reader.ReadBoolean();
			this.erosionFirstTag = ctx.reader.ReadInt32();
			this.autoLinkGrids = ctx.reader.ReadBoolean();
			this.neighbours = (global::Pathfinding.NumNeighbours)ctx.reader.ReadInt32();
			this.cutCorners = ctx.reader.ReadBoolean();
			this.penaltyPosition = ctx.reader.ReadBoolean();
			this.penaltyPositionFactor = ctx.reader.ReadSingle();
			this.penaltyAngle = ctx.reader.ReadBoolean();
			this.penaltyAngleFactor = ctx.reader.ReadSingle();
			this.penaltyAnglePower = ctx.reader.ReadSingle();
			this.isometricAngle = ctx.reader.ReadSingle();
			this.uniformEdgeCosts = ctx.reader.ReadBoolean();
			this.useJumpPointSearch = ctx.reader.ReadBoolean();
		}

		public override void PostDeserialization()
		{
			this.GenerateMatrix();
			this.SetUpOffsetsAndCosts();
			if (this.nodes == null || this.nodes.Length == 0)
			{
				return;
			}
			if (this.width * this.depth != this.nodes.Length)
			{
				global::UnityEngine.Debug.LogError("Node data did not match with bounds data. Probably a change to the bounds/width/depth data was made after scanning the graph just prior to saving it. Nodes will be discarded");
				this.nodes = new global::Pathfinding.GridNode[0];
				return;
			}
			global::Pathfinding.GridNode.SetGridGraph(global::AstarPath.active.astarData.GetGraphIndex(this), this);
			for (int i = 0; i < this.depth; i++)
			{
				for (int j = 0; j < this.width; j++)
				{
					global::Pathfinding.GridNode gridNode = this.nodes[i * this.width + j];
					if (gridNode == null)
					{
						global::UnityEngine.Debug.LogError("Deserialization Error : Couldn't cast the node to the appropriate type - GridGenerator");
						return;
					}
					gridNode.NodeInGridIndex = i * this.width + j;
				}
			}
		}

		public const int getNearestForceOverlap = 2;

		public int width;

		public int depth;

		[global::Pathfinding.Serialization.JsonMember]
		public float aspectRatio = 1f;

		[global::Pathfinding.Serialization.JsonMember]
		public float isometricAngle;

		[global::Pathfinding.Serialization.JsonMember]
		public bool uniformEdgeCosts;

		[global::Pathfinding.Serialization.JsonMember]
		public global::UnityEngine.Vector3 rotation;

		[global::Pathfinding.Serialization.JsonMember]
		public global::UnityEngine.Vector3 center;

		[global::Pathfinding.Serialization.JsonMember]
		public global::UnityEngine.Vector2 unclampedSize;

		[global::Pathfinding.Serialization.JsonMember]
		public float nodeSize = 1f;

		[global::Pathfinding.Serialization.JsonMember]
		public global::Pathfinding.GraphCollision collision;

		[global::Pathfinding.Serialization.JsonMember]
		public float maxClimb = 0.4f;

		[global::Pathfinding.Serialization.JsonMember]
		public int maxClimbAxis = 1;

		[global::Pathfinding.Serialization.JsonMember]
		public float maxSlope = 90f;

		[global::Pathfinding.Serialization.JsonMember]
		public int erodeIterations;

		[global::Pathfinding.Serialization.JsonMember]
		public bool erosionUseTags;

		[global::Pathfinding.Serialization.JsonMember]
		public int erosionFirstTag = 1;

		[global::Pathfinding.Serialization.JsonMember]
		public bool autoLinkGrids;

		[global::Pathfinding.Serialization.JsonMember]
		public float autoLinkDistLimit = 10f;

		[global::Pathfinding.Serialization.JsonMember]
		public global::Pathfinding.NumNeighbours neighbours = global::Pathfinding.NumNeighbours.Eight;

		[global::Pathfinding.Serialization.JsonMember]
		public bool cutCorners = true;

		[global::Pathfinding.Serialization.JsonMember]
		public float penaltyPositionOffset;

		[global::Pathfinding.Serialization.JsonMember]
		public bool penaltyPosition;

		[global::Pathfinding.Serialization.JsonMember]
		public float penaltyPositionFactor = 1f;

		[global::Pathfinding.Serialization.JsonMember]
		public bool penaltyAngle;

		[global::Pathfinding.Serialization.JsonMember]
		public float penaltyAngleFactor = 100f;

		[global::Pathfinding.Serialization.JsonMember]
		public float penaltyAnglePower = 1f;

		[global::Pathfinding.Serialization.JsonMember]
		public bool useJumpPointSearch;

		[global::Pathfinding.Serialization.JsonMember]
		public global::Pathfinding.GridGraph.TextureData textureData = new global::Pathfinding.GridGraph.TextureData();

		[global::System.NonSerialized]
		public readonly int[] neighbourOffsets = new int[8];

		[global::System.NonSerialized]
		public readonly uint[] neighbourCosts = new uint[8];

		[global::System.NonSerialized]
		public readonly int[] neighbourXOffsets = new int[8];

		[global::System.NonSerialized]
		public readonly int[] neighbourZOffsets = new int[8];

		internal static readonly int[] hexagonNeighbourIndices = new int[]
		{
			0,
			1,
			2,
			3,
			5,
			7
		};

		public global::Pathfinding.GridNode[] nodes;

		public class TextureData
		{
			public void Initialize()
			{
				if (this.enabled && this.source != null)
				{
					for (int i = 0; i < this.channels.Length; i++)
					{
						if (this.channels[i] != global::Pathfinding.GridGraph.TextureData.ChannelUse.None)
						{
							try
							{
								this.data = this.source.GetPixels32();
							}
							catch (global::UnityEngine.UnityException ex)
							{
								global::UnityEngine.Debug.LogWarning(ex.ToString());
								this.data = null;
							}
							break;
						}
					}
				}
			}

			public void Apply(global::Pathfinding.GridNode node, int x, int z)
			{
				if (this.enabled && this.data != null && x < this.source.width && z < this.source.height)
				{
					global::UnityEngine.Color32 color = this.data[z * this.source.width + x];
					if (this.channels[0] != global::Pathfinding.GridGraph.TextureData.ChannelUse.None)
					{
						this.ApplyChannel(node, x, z, (int)color.r, this.channels[0], this.factors[0]);
					}
					if (this.channels[1] != global::Pathfinding.GridGraph.TextureData.ChannelUse.None)
					{
						this.ApplyChannel(node, x, z, (int)color.g, this.channels[1], this.factors[1]);
					}
					if (this.channels[2] != global::Pathfinding.GridGraph.TextureData.ChannelUse.None)
					{
						this.ApplyChannel(node, x, z, (int)color.b, this.channels[2], this.factors[2]);
					}
				}
			}

			private void ApplyChannel(global::Pathfinding.GridNode node, int x, int z, int value, global::Pathfinding.GridGraph.TextureData.ChannelUse channelUse, float factor)
			{
				switch (channelUse)
				{
				case global::Pathfinding.GridGraph.TextureData.ChannelUse.Penalty:
					node.Penalty += (uint)global::UnityEngine.Mathf.RoundToInt((float)value * factor);
					break;
				case global::Pathfinding.GridGraph.TextureData.ChannelUse.Position:
					node.position = global::Pathfinding.GridNode.GetGridGraph(node.GraphIndex).GraphPointToWorld(x, z, (float)value);
					break;
				case global::Pathfinding.GridGraph.TextureData.ChannelUse.WalkablePenalty:
					if (value == 0)
					{
						node.Walkable = false;
					}
					else
					{
						node.Penalty += (uint)global::UnityEngine.Mathf.RoundToInt((float)(value - 1) * factor);
					}
					break;
				}
			}

			public bool enabled;

			public global::UnityEngine.Texture2D source;

			public float[] factors = new float[3];

			public global::Pathfinding.GridGraph.TextureData.ChannelUse[] channels = new global::Pathfinding.GridGraph.TextureData.ChannelUse[3];

			private global::UnityEngine.Color32[] data;

			public enum ChannelUse
			{
				None,
				Penalty,
				Position,
				WalkablePenalty
			}
		}
	}
}
