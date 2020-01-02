using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Pathfinding.Recast;
using Pathfinding.Serialization;
using Pathfinding.Util;
using Pathfinding.Voxels;
using UnityEngine;

namespace Pathfinding
{
	[global::Pathfinding.Serialization.JsonOptIn]
	[global::System.Serializable]
	public class RecastGraph : global::Pathfinding.NavGraph, global::Pathfinding.IUpdatableGraph, global::Pathfinding.IRaycastableGraph, global::Pathfinding.INavmesh, global::Pathfinding.INavmeshHolder
	{
		public event global::System.Action<global::Pathfinding.RecastGraph.NavmeshTile[]> OnRecalculatedTiles;

		global::Pathfinding.GraphUpdateThreading global::Pathfinding.IUpdatableGraph.CanUpdateAsync(global::Pathfinding.GraphUpdateObject o)
		{
			return (!o.updatePhysics) ? global::Pathfinding.GraphUpdateThreading.SeparateThread : ((global::Pathfinding.GraphUpdateThreading)7);
		}

		void global::Pathfinding.IUpdatableGraph.UpdateAreaInit(global::Pathfinding.GraphUpdateObject o)
		{
			if (!o.updatePhysics)
			{
				return;
			}
			if (!this.dynamic)
			{
				throw new global::System.Exception("Recast graph must be marked as dynamic to enable graph updates");
			}
			global::Pathfinding.RelevantGraphSurface.UpdateAllPositions();
			global::Pathfinding.IntRect touchingTiles = this.GetTouchingTiles(o.bounds);
			global::UnityEngine.Bounds tileBounds = this.GetTileBounds(touchingTiles);
			tileBounds.Expand(new global::UnityEngine.Vector3(1f, 0f, 1f) * this.TileBorderSizeInWorldUnits * 2f);
			global::System.Collections.Generic.List<global::Pathfinding.Voxels.RasterizationMesh> inputMeshes = this.CollectMeshes(tileBounds);
			if (this.globalVox == null)
			{
				this.globalVox = new global::Pathfinding.Voxels.Voxelize(this.CellHeight, this.cellSize, this.walkableClimb, this.walkableHeight, this.maxSlope);
				this.globalVox.maxEdgeLength = this.maxEdgeLength;
			}
			this.globalVox.inputMeshes = inputMeshes;
		}

		void global::Pathfinding.IUpdatableGraph.UpdateArea(global::Pathfinding.GraphUpdateObject guo)
		{
			global::Pathfinding.IntRect touchingTiles = this.GetTouchingTiles(guo.bounds);
			if (!guo.updatePhysics)
			{
				for (int i = touchingTiles.ymin; i <= touchingTiles.ymax; i++)
				{
					for (int j = touchingTiles.xmin; j <= touchingTiles.xmax; j++)
					{
						global::Pathfinding.RecastGraph.NavmeshTile graph = this.tiles[i * this.tileXCount + j];
						global::Pathfinding.NavMeshGraph.UpdateArea(guo, graph);
					}
				}
				return;
			}
			if (!this.dynamic)
			{
				throw new global::System.Exception("Recast graph must be marked as dynamic to enable graph updates with updatePhysics = true");
			}
			global::Pathfinding.Voxels.Voxelize voxelize = this.globalVox;
			if (voxelize == null)
			{
				throw new global::System.InvalidOperationException("No Voxelizer object. UpdateAreaInit should have been called before this function.");
			}
			for (int k = touchingTiles.xmin; k <= touchingTiles.xmax; k++)
			{
				for (int l = touchingTiles.ymin; l <= touchingTiles.ymax; l++)
				{
					this.stagingTiles.Add(this.BuildTileMesh(voxelize, k, l, 0));
				}
			}
			uint graphIndex = (uint)global::AstarPath.active.astarData.GetGraphIndex(this);
			for (int m = 0; m < this.stagingTiles.Count; m++)
			{
				global::Pathfinding.RecastGraph.NavmeshTile navmeshTile = this.stagingTiles[m];
				global::Pathfinding.GraphNode[] nodes = navmeshTile.nodes;
				for (int n = 0; n < nodes.Length; n++)
				{
					nodes[n].GraphIndex = graphIndex;
				}
			}
		}

		void global::Pathfinding.IUpdatableGraph.UpdateAreaPost(global::Pathfinding.GraphUpdateObject guo)
		{
			for (int i = 0; i < this.stagingTiles.Count; i++)
			{
				global::Pathfinding.RecastGraph.NavmeshTile navmeshTile = this.stagingTiles[i];
				int num = navmeshTile.x + navmeshTile.z * this.tileXCount;
				global::Pathfinding.RecastGraph.NavmeshTile navmeshTile2 = this.tiles[num];
				for (int j = 0; j < navmeshTile2.nodes.Length; j++)
				{
					navmeshTile2.nodes[j].Destroy();
				}
				this.tiles[num] = navmeshTile;
			}
			for (int k = 0; k < this.stagingTiles.Count; k++)
			{
				global::Pathfinding.RecastGraph.NavmeshTile tile = this.stagingTiles[k];
				this.ConnectTileWithNeighbours(tile, false);
			}
			if (this.OnRecalculatedTiles != null)
			{
				this.OnRecalculatedTiles(this.stagingTiles.ToArray());
			}
			this.stagingTiles.Clear();
		}

		public global::UnityEngine.Bounds forcedBounds
		{
			get
			{
				return new global::UnityEngine.Bounds(this.forcedBoundsCenter, this.forcedBoundsSize);
			}
		}

		public global::Pathfinding.RecastGraph.NavmeshTile GetTile(int x, int z)
		{
			return this.tiles[x + z * this.tileXCount];
		}

		public global::Pathfinding.Int3 GetVertex(int index)
		{
			int num = index >> 12 & 524287;
			return this.tiles[num].GetVertex(index);
		}

		public int GetTileIndex(int index)
		{
			return index >> 12 & 524287;
		}

		public int GetVertexArrayIndex(int index)
		{
			return index & 4095;
		}

		public void GetTileCoordinates(int tileIndex, out int x, out int z)
		{
			z = tileIndex / this.tileXCount;
			x = tileIndex - z * this.tileXCount;
		}

		public global::Pathfinding.RecastGraph.NavmeshTile[] GetTiles()
		{
			return this.tiles;
		}

		public global::UnityEngine.Bounds GetTileBounds(global::Pathfinding.IntRect rect)
		{
			return this.GetTileBounds(rect.xmin, rect.ymin, rect.Width, rect.Height);
		}

		public global::UnityEngine.Bounds GetTileBounds(int x, int z, int width = 1, int depth = 1)
		{
			global::UnityEngine.Bounds result = default(global::UnityEngine.Bounds);
			result.SetMinMax(new global::UnityEngine.Vector3((float)(x * this.tileSizeX) * this.cellSize, 0f, (float)(z * this.tileSizeZ) * this.cellSize) + this.forcedBounds.min, new global::UnityEngine.Vector3((float)((x + width) * this.tileSizeX) * this.cellSize, this.forcedBounds.size.y, (float)((z + depth) * this.tileSizeZ) * this.cellSize) + this.forcedBounds.min);
			return result;
		}

		public global::Pathfinding.Int2 GetTileCoordinates(global::UnityEngine.Vector3 p)
		{
			p -= this.forcedBounds.min;
			p.x /= this.cellSize * (float)this.tileSizeX;
			p.z /= this.cellSize * (float)this.tileSizeZ;
			return new global::Pathfinding.Int2((int)p.x, (int)p.z);
		}

		public override void OnDestroy()
		{
			base.OnDestroy();
			global::Pathfinding.TriangleMeshNode.SetNavmeshHolder(this.active.astarData.GetGraphIndex(this), null);
		}

		public override void RelocateNodes(global::UnityEngine.Matrix4x4 oldMatrix, global::UnityEngine.Matrix4x4 newMatrix)
		{
			if (this.tiles != null)
			{
				global::UnityEngine.Matrix4x4 inverse = oldMatrix.inverse;
				global::UnityEngine.Matrix4x4 matrix4x = newMatrix * inverse;
				if (this.tiles.Length > 1)
				{
					throw new global::System.Exception("RelocateNodes cannot be used on tiled recast graphs");
				}
				for (int i = 0; i < this.tiles.Length; i++)
				{
					global::Pathfinding.RecastGraph.NavmeshTile navmeshTile = this.tiles[i];
					if (navmeshTile != null)
					{
						global::Pathfinding.Int3[] verts = navmeshTile.verts;
						for (int j = 0; j < verts.Length; j++)
						{
							verts[j] = (global::Pathfinding.Int3)matrix4x.MultiplyPoint((global::UnityEngine.Vector3)verts[j]);
						}
						for (int k = 0; k < navmeshTile.nodes.Length; k++)
						{
							global::Pathfinding.TriangleMeshNode triangleMeshNode = navmeshTile.nodes[k];
							triangleMeshNode.UpdatePositionFromVertices();
						}
						navmeshTile.bbTree.RebuildFrom(navmeshTile.nodes);
					}
				}
			}
			base.SetMatrix(newMatrix);
		}

		private static global::Pathfinding.RecastGraph.NavmeshTile NewEmptyTile(int x, int z)
		{
			return new global::Pathfinding.RecastGraph.NavmeshTile
			{
				x = x,
				z = z,
				w = 1,
				d = 1,
				verts = new global::Pathfinding.Int3[0],
				tris = new int[0],
				nodes = new global::Pathfinding.TriangleMeshNode[0],
				bbTree = global::Pathfinding.Util.ObjectPool<global::Pathfinding.BBTree>.Claim()
			};
		}

		public override void GetNodes(global::Pathfinding.GraphNodeDelegateCancelable del)
		{
			if (this.tiles == null)
			{
				return;
			}
			for (int i = 0; i < this.tiles.Length; i++)
			{
				if (this.tiles[i] != null && this.tiles[i].x + this.tiles[i].z * this.tileXCount == i)
				{
					global::Pathfinding.TriangleMeshNode[] nodes = this.tiles[i].nodes;
					if (nodes != null)
					{
						int num = 0;
						while (num < nodes.Length && del(nodes[num]))
						{
							num++;
						}
					}
				}
			}
		}

		[global::System.Obsolete("Use node.ClosestPointOnNode instead")]
		public global::UnityEngine.Vector3 ClosestPointOnNode(global::Pathfinding.TriangleMeshNode node, global::UnityEngine.Vector3 pos)
		{
			return global::Pathfinding.Polygon.ClosestPointOnTriangle((global::UnityEngine.Vector3)this.GetVertex(node.v0), (global::UnityEngine.Vector3)this.GetVertex(node.v1), (global::UnityEngine.Vector3)this.GetVertex(node.v2), pos);
		}

		[global::System.Obsolete("Use node.ContainsPoint instead")]
		public bool ContainsPoint(global::Pathfinding.TriangleMeshNode node, global::UnityEngine.Vector3 pos)
		{
			return global::Pathfinding.VectorMath.IsClockwiseXZ((global::UnityEngine.Vector3)this.GetVertex(node.v0), (global::UnityEngine.Vector3)this.GetVertex(node.v1), pos) && global::Pathfinding.VectorMath.IsClockwiseXZ((global::UnityEngine.Vector3)this.GetVertex(node.v1), (global::UnityEngine.Vector3)this.GetVertex(node.v2), pos) && global::Pathfinding.VectorMath.IsClockwiseXZ((global::UnityEngine.Vector3)this.GetVertex(node.v2), (global::UnityEngine.Vector3)this.GetVertex(node.v0), pos);
		}

		public void SnapForceBoundsToScene()
		{
			global::System.Collections.Generic.List<global::Pathfinding.Voxels.RasterizationMesh> list = this.CollectMeshes(new global::UnityEngine.Bounds(global::UnityEngine.Vector3.zero, new global::UnityEngine.Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity)));
			if (list.Count == 0)
			{
				return;
			}
			global::UnityEngine.Bounds bounds = list[0].bounds;
			for (int i = 1; i < list.Count; i++)
			{
				bounds.Encapsulate(list[i].bounds);
			}
			this.forcedBoundsCenter = bounds.center;
			this.forcedBoundsSize = bounds.size;
		}

		public global::Pathfinding.IntRect GetTouchingTiles(global::UnityEngine.Bounds b)
		{
			b.center -= this.forcedBounds.min;
			global::Pathfinding.IntRect intRect = new global::Pathfinding.IntRect(global::UnityEngine.Mathf.FloorToInt(b.min.x / ((float)this.tileSizeX * this.cellSize)), global::UnityEngine.Mathf.FloorToInt(b.min.z / ((float)this.tileSizeZ * this.cellSize)), global::UnityEngine.Mathf.FloorToInt(b.max.x / ((float)this.tileSizeX * this.cellSize)), global::UnityEngine.Mathf.FloorToInt(b.max.z / ((float)this.tileSizeZ * this.cellSize)));
			intRect = global::Pathfinding.IntRect.Intersection(intRect, new global::Pathfinding.IntRect(0, 0, this.tileXCount - 1, this.tileZCount - 1));
			return intRect;
		}

		public global::Pathfinding.IntRect GetTouchingTilesRound(global::UnityEngine.Bounds b)
		{
			b.center -= this.forcedBounds.min;
			global::Pathfinding.IntRect intRect = new global::Pathfinding.IntRect(global::UnityEngine.Mathf.RoundToInt(b.min.x / ((float)this.tileSizeX * this.cellSize)), global::UnityEngine.Mathf.RoundToInt(b.min.z / ((float)this.tileSizeZ * this.cellSize)), global::UnityEngine.Mathf.RoundToInt(b.max.x / ((float)this.tileSizeX * this.cellSize)) - 1, global::UnityEngine.Mathf.RoundToInt(b.max.z / ((float)this.tileSizeZ * this.cellSize)) - 1);
			intRect = global::Pathfinding.IntRect.Intersection(intRect, new global::Pathfinding.IntRect(0, 0, this.tileXCount - 1, this.tileZCount - 1));
			return intRect;
		}

		private void ConnectTileWithNeighbours(global::Pathfinding.RecastGraph.NavmeshTile tile, bool onlyUnflagged = false)
		{
			if (tile.w != 1 || tile.d != 1)
			{
				throw new global::System.ArgumentException("Tile widths or depths other than 1 are not supported. The fields exist mainly for possible future expansions.");
			}
			for (int i = -1; i <= 1; i++)
			{
				int num = tile.z + i;
				if (num >= 0 && num < this.tileZCount)
				{
					for (int j = -1; j <= 1; j++)
					{
						int num2 = tile.x + j;
						if (num2 >= 0 && num2 < this.tileXCount)
						{
							if (j == 0 != (i == 0))
							{
								global::Pathfinding.RecastGraph.NavmeshTile navmeshTile = this.tiles[num2 + num * this.tileXCount];
								if (!onlyUnflagged || !navmeshTile.flag)
								{
									this.ConnectTiles(navmeshTile, tile);
								}
							}
						}
					}
				}
			}
		}

		private void RemoveConnectionsFromTile(global::Pathfinding.RecastGraph.NavmeshTile tile)
		{
			if (tile.x > 0)
			{
				int num = tile.x - 1;
				for (int i = tile.z; i < tile.z + tile.d; i++)
				{
					this.RemoveConnectionsFromTo(this.tiles[num + i * this.tileXCount], tile);
				}
			}
			if (tile.x + tile.w < this.tileXCount)
			{
				int num2 = tile.x + tile.w;
				for (int j = tile.z; j < tile.z + tile.d; j++)
				{
					this.RemoveConnectionsFromTo(this.tiles[num2 + j * this.tileXCount], tile);
				}
			}
			if (tile.z > 0)
			{
				int num3 = tile.z - 1;
				for (int k = tile.x; k < tile.x + tile.w; k++)
				{
					this.RemoveConnectionsFromTo(this.tiles[k + num3 * this.tileXCount], tile);
				}
			}
			if (tile.z + tile.d < this.tileZCount)
			{
				int num4 = tile.z + tile.d;
				for (int l = tile.x; l < tile.x + tile.w; l++)
				{
					this.RemoveConnectionsFromTo(this.tiles[l + num4 * this.tileXCount], tile);
				}
			}
		}

		private void RemoveConnectionsFromTo(global::Pathfinding.RecastGraph.NavmeshTile a, global::Pathfinding.RecastGraph.NavmeshTile b)
		{
			if (a == null || b == null)
			{
				return;
			}
			if (a == b)
			{
				return;
			}
			int num = b.x + b.z * this.tileXCount;
			for (int i = 0; i < a.nodes.Length; i++)
			{
				global::Pathfinding.TriangleMeshNode triangleMeshNode = a.nodes[i];
				if (triangleMeshNode.connections != null)
				{
					for (int j = 0; j < triangleMeshNode.connections.Length; j++)
					{
						global::Pathfinding.TriangleMeshNode triangleMeshNode2 = triangleMeshNode.connections[j] as global::Pathfinding.TriangleMeshNode;
						if (triangleMeshNode2 != null)
						{
							int num2 = triangleMeshNode2.GetVertexIndex(0);
							num2 = (num2 >> 12 & 524287);
							if (num2 == num)
							{
								triangleMeshNode.RemoveConnection(triangleMeshNode.connections[j]);
								j--;
							}
						}
					}
				}
			}
		}

		public override global::Pathfinding.NNInfoInternal GetNearest(global::UnityEngine.Vector3 position, global::Pathfinding.NNConstraint constraint, global::Pathfinding.GraphNode hint)
		{
			return this.GetNearestForce(position, null);
		}

		public override global::Pathfinding.NNInfoInternal GetNearestForce(global::UnityEngine.Vector3 position, global::Pathfinding.NNConstraint constraint)
		{
			if (this.tiles == null)
			{
				return default(global::Pathfinding.NNInfoInternal);
			}
			global::UnityEngine.Vector3 vector = position - this.forcedBounds.min;
			int num = global::UnityEngine.Mathf.FloorToInt(vector.x / (this.cellSize * (float)this.tileSizeX));
			int num2 = global::UnityEngine.Mathf.FloorToInt(vector.z / (this.cellSize * (float)this.tileSizeZ));
			num = global::UnityEngine.Mathf.Clamp(num, 0, this.tileXCount - 1);
			num2 = global::UnityEngine.Mathf.Clamp(num2, 0, this.tileZCount - 1);
			int num3 = global::System.Math.Max(this.tileXCount, this.tileZCount);
			global::Pathfinding.NNInfoInternal nninfoInternal = default(global::Pathfinding.NNInfoInternal);
			float positiveInfinity = float.PositiveInfinity;
			bool flag = this.nearestSearchOnlyXZ || (constraint != null && constraint.distanceXZ);
			for (int i = 0; i < num3; i++)
			{
				if (!flag && positiveInfinity < (float)(i - 1) * this.cellSize * (float)global::System.Math.Max(this.tileSizeX, this.tileSizeZ))
				{
					break;
				}
				int num4 = global::System.Math.Min(i + num2 + 1, this.tileZCount);
				for (int j = global::System.Math.Max(-i + num2, 0); j < num4; j++)
				{
					int num5 = global::System.Math.Abs(i - global::System.Math.Abs(j - num2));
					if (-num5 + num >= 0)
					{
						int num6 = -num5 + num;
						global::Pathfinding.RecastGraph.NavmeshTile navmeshTile = this.tiles[num6 + j * this.tileXCount];
						if (navmeshTile != null)
						{
							if (flag)
							{
								nninfoInternal = navmeshTile.bbTree.QueryClosestXZ(position, constraint, ref positiveInfinity, nninfoInternal);
								if (positiveInfinity < float.PositiveInfinity)
								{
									break;
								}
							}
							else
							{
								nninfoInternal = navmeshTile.bbTree.QueryClosest(position, constraint, ref positiveInfinity, nninfoInternal);
							}
						}
					}
					if (num5 != 0 && num5 + num < this.tileXCount)
					{
						int num7 = num5 + num;
						global::Pathfinding.RecastGraph.NavmeshTile navmeshTile2 = this.tiles[num7 + j * this.tileXCount];
						if (navmeshTile2 != null)
						{
							if (flag)
							{
								nninfoInternal = navmeshTile2.bbTree.QueryClosestXZ(position, constraint, ref positiveInfinity, nninfoInternal);
								if (positiveInfinity < float.PositiveInfinity)
								{
									break;
								}
							}
							else
							{
								nninfoInternal = navmeshTile2.bbTree.QueryClosest(position, constraint, ref positiveInfinity, nninfoInternal);
							}
						}
					}
				}
			}
			nninfoInternal.node = nninfoInternal.constrainedNode;
			nninfoInternal.constrainedNode = null;
			nninfoInternal.clampedPosition = nninfoInternal.constClampedPosition;
			return nninfoInternal;
		}

		public global::Pathfinding.GraphNode PointOnNavmesh(global::UnityEngine.Vector3 position, global::Pathfinding.NNConstraint constraint)
		{
			if (this.tiles == null)
			{
				return null;
			}
			global::UnityEngine.Vector3 vector = position - this.forcedBounds.min;
			int num = global::UnityEngine.Mathf.FloorToInt(vector.x / (this.cellSize * (float)this.tileSizeX));
			int num2 = global::UnityEngine.Mathf.FloorToInt(vector.z / (this.cellSize * (float)this.tileSizeZ));
			if (num < 0 || num2 < 0 || num >= this.tileXCount || num2 >= this.tileZCount)
			{
				return null;
			}
			global::Pathfinding.RecastGraph.NavmeshTile navmeshTile = this.tiles[num + num2 * this.tileXCount];
			if (navmeshTile != null)
			{
				return navmeshTile.bbTree.QueryInside(position, constraint);
			}
			return null;
		}

		public override global::System.Collections.Generic.IEnumerable<global::Pathfinding.Progress> ScanInternal()
		{
			global::Pathfinding.TriangleMeshNode.SetNavmeshHolder(global::AstarPath.active.astarData.GetGraphIndex(this), this);
			foreach (global::Pathfinding.Progress p in this.ScanAllTiles())
			{
				yield return p;
			}
			yield break;
		}

		private void InitializeTileInfo()
		{
			int num = global::UnityEngine.Mathf.Max((int)(this.forcedBounds.size.x / this.cellSize + 0.5f), 1);
			int num2 = global::UnityEngine.Mathf.Max((int)(this.forcedBounds.size.z / this.cellSize + 0.5f), 1);
			if (!this.useTiles)
			{
				this.tileSizeX = num;
				this.tileSizeZ = num2;
			}
			else
			{
				this.tileSizeX = this.editorTileSize;
				this.tileSizeZ = this.editorTileSize;
			}
			this.tileXCount = (num + this.tileSizeX - 1) / this.tileSizeX;
			this.tileZCount = (num2 + this.tileSizeZ - 1) / this.tileSizeZ;
			if (this.tileXCount * this.tileZCount > 524288)
			{
				throw new global::System.Exception(string.Concat(new object[]
				{
					"Too many tiles (",
					this.tileXCount * this.tileZCount,
					") maximum is ",
					524288,
					"\nTry disabling ASTAR_RECAST_LARGER_TILES under the 'Optimizations' tab in the A* inspector."
				}));
			}
			this.tiles = new global::Pathfinding.RecastGraph.NavmeshTile[this.tileXCount * this.tileZCount];
		}

		private void BuildTiles(global::System.Collections.Generic.Queue<global::Pathfinding.Int2> tileQueue, global::System.Collections.Generic.List<global::Pathfinding.Voxels.RasterizationMesh>[] meshBuckets, global::System.Threading.ManualResetEvent doneEvent, int threadIndex)
		{
			try
			{
				global::Pathfinding.Voxels.Voxelize voxelize = new global::Pathfinding.Voxels.Voxelize(this.CellHeight, this.cellSize, this.walkableClimb, this.walkableHeight, this.maxSlope);
				voxelize.maxEdgeLength = this.maxEdgeLength;
				for (;;)
				{
					global::Pathfinding.Int2 @int;
					lock (tileQueue)
					{
						if (tileQueue.Count == 0)
						{
							break;
						}
						@int = tileQueue.Dequeue();
					}
					voxelize.inputMeshes = meshBuckets[@int.x + @int.y * this.tileXCount];
					this.tiles[@int.x + @int.y * this.tileXCount] = this.BuildTileMesh(voxelize, @int.x, @int.y, threadIndex);
				}
			}
			catch (global::System.Exception exception)
			{
				global::UnityEngine.Debug.LogException(exception);
			}
			finally
			{
				if (doneEvent != null)
				{
					doneEvent.Set();
				}
			}
		}

		private void ConnectTiles(global::System.Collections.Generic.Queue<global::Pathfinding.Int2> tileQueue, global::System.Threading.ManualResetEvent doneEvent)
		{
			try
			{
				for (;;)
				{
					global::Pathfinding.Int2 @int;
					lock (tileQueue)
					{
						if (tileQueue.Count == 0)
						{
							break;
						}
						@int = tileQueue.Dequeue();
					}
					if (@int.x < this.tileXCount - 1)
					{
						this.ConnectTiles(this.tiles[@int.x + @int.y * this.tileXCount], this.tiles[@int.x + 1 + @int.y * this.tileXCount]);
					}
					if (@int.y < this.tileZCount - 1)
					{
						this.ConnectTiles(this.tiles[@int.x + @int.y * this.tileXCount], this.tiles[@int.x + (@int.y + 1) * this.tileXCount]);
					}
				}
			}
			catch (global::System.Exception exception)
			{
				global::UnityEngine.Debug.LogException(exception);
			}
			finally
			{
				if (doneEvent != null)
				{
					doneEvent.Set();
				}
			}
		}

		private global::System.Collections.Generic.List<global::Pathfinding.Voxels.RasterizationMesh>[] PutMeshesIntoTileBuckets(global::System.Collections.Generic.List<global::Pathfinding.Voxels.RasterizationMesh> meshes)
		{
			global::System.Collections.Generic.List<global::Pathfinding.Voxels.RasterizationMesh>[] array = new global::System.Collections.Generic.List<global::Pathfinding.Voxels.RasterizationMesh>[this.tiles.Length];
			global::UnityEngine.Vector3 amount = new global::UnityEngine.Vector3(1f, 0f, 1f) * this.TileBorderSizeInWorldUnits * 2f;
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = new global::System.Collections.Generic.List<global::Pathfinding.Voxels.RasterizationMesh>();
			}
			for (int j = 0; j < meshes.Count; j++)
			{
				global::Pathfinding.Voxels.RasterizationMesh rasterizationMesh = meshes[j];
				global::UnityEngine.Bounds bounds = rasterizationMesh.bounds;
				bounds.Expand(amount);
				global::Pathfinding.IntRect touchingTiles = this.GetTouchingTiles(bounds);
				for (int k = touchingTiles.ymin; k <= touchingTiles.ymax; k++)
				{
					for (int l = touchingTiles.xmin; l <= touchingTiles.xmax; l++)
					{
						array[l + k * this.tileXCount].Add(rasterizationMesh);
					}
				}
			}
			return array;
		}

		protected global::System.Collections.Generic.IEnumerable<global::Pathfinding.Progress> ScanAllTiles()
		{
			this.InitializeTileInfo();
			if (this.scanEmptyGraph)
			{
				this.FillWithEmptyTiles();
				yield break;
			}
			yield return new global::Pathfinding.Progress(0f, "Finding Meshes");
			global::System.Collections.Generic.List<global::Pathfinding.Voxels.RasterizationMesh> meshes = this.CollectMeshes(this.forcedBounds);
			this.walkableClimb = global::UnityEngine.Mathf.Min(this.walkableClimb, this.walkableHeight);
			global::System.Collections.Generic.List<global::Pathfinding.Voxels.RasterizationMesh>[] buckets = this.PutMeshesIntoTileBuckets(meshes);
			global::System.Collections.Generic.Queue<global::Pathfinding.Int2> tileQueue = new global::System.Collections.Generic.Queue<global::Pathfinding.Int2>();
			for (int z = 0; z < this.tileZCount; z++)
			{
				for (int x = 0; x < this.tileXCount; x++)
				{
					tileQueue.Enqueue(new global::Pathfinding.Int2(x, z));
				}
			}
			int threadCount = global::UnityEngine.Mathf.Min(tileQueue.Count, global::UnityEngine.Mathf.Max(1, global::AstarPath.CalculateThreadCount(global::Pathfinding.ThreadCount.AutomaticHighLoad)));
			global::System.Threading.ManualResetEvent[] waitEvents = new global::System.Threading.ManualResetEvent[threadCount];
			for (int i = 0; i < waitEvents.Length; i++)
			{
				waitEvents[i] = new global::System.Threading.ManualResetEvent(false);
				global::System.Threading.ThreadPool.QueueUserWorkItem(delegate(object state)
				{
					this.BuildTiles(tileQueue, buckets, waitEvents[(int)state], (int)state);
				}, i);
			}
			int timeoutMillis = (!global::UnityEngine.Application.isPlaying) ? 200 : 1;
			while (!global::System.Threading.WaitHandle.WaitAll(waitEvents, timeoutMillis))
			{
				global::System.Collections.Generic.Queue<global::Pathfinding.Int2> obj = tileQueue;
				int count;
				lock (obj)
				{
					count = tileQueue.Count;
				}
				yield return new global::Pathfinding.Progress(global::UnityEngine.Mathf.Lerp(0.1f, 0.9f, (float)(this.tiles.Length - count + 1) / (float)this.tiles.Length), string.Concat(new object[]
				{
					"Generating Tile ",
					this.tiles.Length - count + 1,
					"/",
					this.tiles.Length
				}));
			}
			yield return new global::Pathfinding.Progress(0.9f, "Assigning Graph Indices");
			uint graphIndex = (uint)global::AstarPath.active.astarData.GetGraphIndex(this);
			this.GetNodes(delegate(global::Pathfinding.GraphNode node)
			{
				node.GraphIndex = graphIndex;
				return true;
			});
			for (int coordinateSum = 0; coordinateSum <= 1; coordinateSum++)
			{
				for (int j = 0; j < this.tiles.Length; j++)
				{
					if ((this.tiles[j].x + this.tiles[j].z) % 2 == coordinateSum)
					{
						tileQueue.Enqueue(new global::Pathfinding.Int2(this.tiles[j].x, this.tiles[j].z));
					}
				}
				for (int k = 0; k < waitEvents.Length; k++)
				{
					waitEvents[k].Reset();
					global::System.Threading.ThreadPool.QueueUserWorkItem(delegate(object state)
					{
						this.ConnectTiles(tileQueue, state as global::System.Threading.ManualResetEvent);
					}, waitEvents[k]);
				}
				while (!global::System.Threading.WaitHandle.WaitAll(waitEvents, timeoutMillis))
				{
					global::System.Collections.Generic.Queue<global::Pathfinding.Int2> obj2 = tileQueue;
					int count2;
					lock (obj2)
					{
						count2 = tileQueue.Count;
					}
					yield return new global::Pathfinding.Progress(global::UnityEngine.Mathf.Lerp(0.9f, 1f, (float)(this.tiles.Length - count2 + 1) / (float)this.tiles.Length), string.Concat(new object[]
					{
						"Connecting Tile ",
						this.tiles.Length - count2 + 1,
						"/",
						this.tiles.Length,
						" (Phase ",
						coordinateSum + 1,
						")"
					}));
				}
			}
			if (this.OnRecalculatedTiles != null)
			{
				this.OnRecalculatedTiles(this.tiles.Clone() as global::Pathfinding.RecastGraph.NavmeshTile[]);
			}
			yield break;
		}

		private global::System.Collections.Generic.List<global::Pathfinding.Voxels.RasterizationMesh> CollectMeshes(global::UnityEngine.Bounds bounds)
		{
			global::System.Collections.Generic.List<global::Pathfinding.Voxels.RasterizationMesh> list = new global::System.Collections.Generic.List<global::Pathfinding.Voxels.RasterizationMesh>();
			global::Pathfinding.Recast.RecastMeshGatherer recastMeshGatherer = new global::Pathfinding.Recast.RecastMeshGatherer(bounds, this.terrainSampleSize, this.mask, this.tagMask, this.colliderRasterizeDetail);
			if (this.rasterizeMeshes)
			{
				recastMeshGatherer.CollectSceneMeshes(list);
			}
			recastMeshGatherer.CollectRecastMeshObjs(list);
			if (this.rasterizeTerrain)
			{
				float desiredChunkSize = this.cellSize * (float)global::System.Math.Max(this.tileSizeX, this.tileSizeZ);
				recastMeshGatherer.CollectTerrainMeshes(this.rasterizeTrees, desiredChunkSize, list);
			}
			if (this.rasterizeColliders)
			{
				recastMeshGatherer.CollectColliderMeshes(list);
			}
			if (list.Count == 0)
			{
				global::UnityEngine.Debug.LogWarning("No MeshFilters were found contained in the layers specified by the 'mask' variables");
			}
			return list;
		}

		private void FillWithEmptyTiles()
		{
			for (int i = 0; i < this.tileZCount; i++)
			{
				for (int j = 0; j < this.tileXCount; j++)
				{
					this.tiles[i * this.tileXCount + j] = global::Pathfinding.RecastGraph.NewEmptyTile(j, i);
				}
			}
		}

		private float CellHeight
		{
			get
			{
				return global::UnityEngine.Mathf.Max(this.forcedBounds.size.y / 64000f, 0.001f);
			}
		}

		private int CharacterRadiusInVoxels
		{
			get
			{
				return global::UnityEngine.Mathf.CeilToInt(this.characterRadius / this.cellSize - 0.1f);
			}
		}

		private int TileBorderSizeInVoxels
		{
			get
			{
				return this.CharacterRadiusInVoxels + 3;
			}
		}

		private float TileBorderSizeInWorldUnits
		{
			get
			{
				return (float)this.TileBorderSizeInVoxels * this.cellSize;
			}
		}

		private global::UnityEngine.Bounds CalculateTileBoundsWithBorder(int x, int z)
		{
			float num = (float)this.tileSizeX * this.cellSize;
			float num2 = (float)this.tileSizeZ * this.cellSize;
			global::UnityEngine.Vector3 min = this.forcedBounds.min;
			global::UnityEngine.Vector3 max = this.forcedBounds.max;
			global::UnityEngine.Bounds result = default(global::UnityEngine.Bounds);
			result.SetMinMax(new global::UnityEngine.Vector3((float)x * num, 0f, (float)z * num2) + min, new global::UnityEngine.Vector3((float)(x + 1) * num + min.x, max.y, (float)(z + 1) * num2 + min.z));
			result.Expand(new global::UnityEngine.Vector3(1f, 0f, 1f) * this.TileBorderSizeInWorldUnits * 2f);
			return result;
		}

		protected global::Pathfinding.RecastGraph.NavmeshTile BuildTileMesh(global::Pathfinding.Voxels.Voxelize vox, int x, int z, int threadIndex = 0)
		{
			vox.borderSize = this.TileBorderSizeInVoxels;
			vox.forcedBounds = this.CalculateTileBoundsWithBorder(x, z);
			vox.width = this.tileSizeX + vox.borderSize * 2;
			vox.depth = this.tileSizeZ + vox.borderSize * 2;
			if (!this.useTiles && this.relevantGraphSurfaceMode == global::Pathfinding.RecastGraph.RelevantGraphSurfaceMode.OnlyForCompletelyInsideTile)
			{
				vox.relevantGraphSurfaceMode = global::Pathfinding.RecastGraph.RelevantGraphSurfaceMode.RequireForAll;
			}
			else
			{
				vox.relevantGraphSurfaceMode = this.relevantGraphSurfaceMode;
			}
			vox.minRegionSize = global::UnityEngine.Mathf.RoundToInt(this.minRegionSize / (this.cellSize * this.cellSize));
			vox.Init();
			vox.VoxelizeInput();
			vox.FilterLedges(vox.voxelWalkableHeight, vox.voxelWalkableClimb, vox.cellSize, vox.cellHeight, vox.forcedBounds.min);
			vox.FilterLowHeightSpans(vox.voxelWalkableHeight, vox.cellSize, vox.cellHeight, vox.forcedBounds.min);
			vox.BuildCompactField();
			vox.BuildVoxelConnections();
			vox.ErodeWalkableArea(this.CharacterRadiusInVoxels);
			vox.BuildDistanceField();
			vox.BuildRegions();
			global::Pathfinding.Voxels.VoxelContourSet cset = new global::Pathfinding.Voxels.VoxelContourSet();
			vox.BuildContours(this.contourMaxError, 1, cset, 1);
			global::Pathfinding.Voxels.VoxelMesh mesh;
			vox.BuildPolyMesh(cset, 3, out mesh);
			for (int i = 0; i < mesh.verts.Length; i++)
			{
				mesh.verts[i] = vox.VoxelToWorldInt3(mesh.verts[i]);
			}
			return this.CreateTile(vox, mesh, x, z, threadIndex);
		}

		private global::Pathfinding.RecastGraph.NavmeshTile CreateTile(global::Pathfinding.Voxels.Voxelize vox, global::Pathfinding.Voxels.VoxelMesh mesh, int x, int z, int threadIndex = 0)
		{
			if (mesh.tris == null)
			{
				throw new global::System.ArgumentNullException("mesh.tris");
			}
			if (mesh.verts == null)
			{
				throw new global::System.ArgumentNullException("mesh.verts");
			}
			global::Pathfinding.RecastGraph.NavmeshTile navmeshTile = new global::Pathfinding.RecastGraph.NavmeshTile
			{
				x = x,
				z = z,
				w = 1,
				d = 1,
				tris = mesh.tris,
				verts = mesh.verts,
				bbTree = new global::Pathfinding.BBTree()
			};
			if (navmeshTile.tris.Length % 3 != 0)
			{
				throw new global::System.ArgumentException("Indices array's length must be a multiple of 3 (mesh.tris)");
			}
			if (navmeshTile.verts.Length >= 4095)
			{
				if (this.tileXCount * this.tileZCount == 1)
				{
					throw new global::System.ArgumentException("Too many vertices per tile (more than " + 4095 + ").\n<b>Try enabling tiling in the recast graph settings.</b>\n");
				}
				throw new global::System.ArgumentException("Too many vertices per tile (more than " + 4095 + ").\n<b>Try reducing tile size or enabling ASTAR_RECAST_LARGER_TILES under the 'Optimizations' tab in the A* Inspector</b>");
			}
			else
			{
				navmeshTile.verts = global::Pathfinding.Voxels.Utility.RemoveDuplicateVertices(navmeshTile.verts, navmeshTile.tris);
				global::Pathfinding.TriangleMeshNode[] array = new global::Pathfinding.TriangleMeshNode[navmeshTile.tris.Length / 3];
				navmeshTile.nodes = array;
				uint num = (uint)(global::AstarPath.active.astarData.graphs.Length + threadIndex);
				if (num > 255U)
				{
					throw new global::System.Exception("Graph limit reached. Multithreaded recast calculations cannot be done because a few scratch graph indices are required.");
				}
				int num2 = x + z * this.tileXCount;
				num2 <<= 12;
				global::Pathfinding.TriangleMeshNode.SetNavmeshHolder((int)num, navmeshTile);
				global::AstarPath active = global::AstarPath.active;
				lock (active)
				{
					for (int i = 0; i < array.Length; i++)
					{
						global::Pathfinding.TriangleMeshNode triangleMeshNode = new global::Pathfinding.TriangleMeshNode(this.active);
						array[i] = triangleMeshNode;
						triangleMeshNode.GraphIndex = num;
						triangleMeshNode.v0 = (navmeshTile.tris[i * 3] | num2);
						triangleMeshNode.v1 = (navmeshTile.tris[i * 3 + 1] | num2);
						triangleMeshNode.v2 = (navmeshTile.tris[i * 3 + 2] | num2);
						if (!global::Pathfinding.VectorMath.IsClockwiseXZ(triangleMeshNode.GetVertex(0), triangleMeshNode.GetVertex(1), triangleMeshNode.GetVertex(2)))
						{
							int v = triangleMeshNode.v0;
							triangleMeshNode.v0 = triangleMeshNode.v2;
							triangleMeshNode.v2 = v;
						}
						triangleMeshNode.Walkable = true;
						triangleMeshNode.Penalty = this.initialPenalty;
						triangleMeshNode.UpdatePositionFromVertices();
					}
				}
				navmeshTile.bbTree.RebuildFrom(array);
				this.CreateNodeConnections(navmeshTile.nodes);
				global::Pathfinding.TriangleMeshNode.SetNavmeshHolder((int)num, null);
				return navmeshTile;
			}
		}

		private void CreateNodeConnections(global::Pathfinding.TriangleMeshNode[] nodes)
		{
			global::System.Collections.Generic.List<global::Pathfinding.MeshNode> list = global::Pathfinding.Util.ListPool<global::Pathfinding.MeshNode>.Claim();
			global::System.Collections.Generic.List<uint> list2 = global::Pathfinding.Util.ListPool<uint>.Claim();
			global::System.Collections.Generic.Dictionary<global::Pathfinding.Int2, int> dictionary = global::Pathfinding.Util.ObjectPoolSimple<global::System.Collections.Generic.Dictionary<global::Pathfinding.Int2, int>>.Claim();
			dictionary.Clear();
			for (int i = 0; i < nodes.Length; i++)
			{
				global::Pathfinding.TriangleMeshNode triangleMeshNode = nodes[i];
				int vertexCount = triangleMeshNode.GetVertexCount();
				for (int j = 0; j < vertexCount; j++)
				{
					global::Pathfinding.Int2 key = new global::Pathfinding.Int2(triangleMeshNode.GetVertexIndex(j), triangleMeshNode.GetVertexIndex((j + 1) % vertexCount));
					if (!dictionary.ContainsKey(key))
					{
						dictionary.Add(key, i);
					}
				}
			}
			foreach (global::Pathfinding.TriangleMeshNode triangleMeshNode2 in nodes)
			{
				list.Clear();
				list2.Clear();
				int vertexCount2 = triangleMeshNode2.GetVertexCount();
				for (int l = 0; l < vertexCount2; l++)
				{
					int vertexIndex = triangleMeshNode2.GetVertexIndex(l);
					int vertexIndex2 = triangleMeshNode2.GetVertexIndex((l + 1) % vertexCount2);
					int num;
					if (dictionary.TryGetValue(new global::Pathfinding.Int2(vertexIndex2, vertexIndex), out num))
					{
						global::Pathfinding.TriangleMeshNode triangleMeshNode3 = nodes[num];
						int vertexCount3 = triangleMeshNode3.GetVertexCount();
						for (int m = 0; m < vertexCount3; m++)
						{
							if (triangleMeshNode3.GetVertexIndex(m) == vertexIndex2 && triangleMeshNode3.GetVertexIndex((m + 1) % vertexCount3) == vertexIndex)
							{
								uint costMagnitude = (uint)(triangleMeshNode2.position - triangleMeshNode3.position).costMagnitude;
								list.Add(triangleMeshNode3);
								list2.Add(costMagnitude);
								break;
							}
						}
					}
				}
				triangleMeshNode2.connections = list.ToArray();
				triangleMeshNode2.connectionCosts = list2.ToArray();
			}
			dictionary.Clear();
			global::Pathfinding.Util.ObjectPoolSimple<global::System.Collections.Generic.Dictionary<global::Pathfinding.Int2, int>>.Release(ref dictionary);
			global::Pathfinding.Util.ListPool<global::Pathfinding.MeshNode>.Release(list);
			global::Pathfinding.Util.ListPool<uint>.Release(list2);
		}

		private void ConnectTiles(global::Pathfinding.RecastGraph.NavmeshTile tile1, global::Pathfinding.RecastGraph.NavmeshTile tile2)
		{
			if (tile1 == null || tile2 == null)
			{
				return;
			}
			if (tile1.nodes == null)
			{
				throw new global::System.ArgumentException("tile1 does not contain any nodes");
			}
			if (tile2.nodes == null)
			{
				throw new global::System.ArgumentException("tile2 does not contain any nodes");
			}
			int num = global::UnityEngine.Mathf.Clamp(tile2.x, tile1.x, tile1.x + tile1.w - 1);
			int num2 = global::UnityEngine.Mathf.Clamp(tile1.x, tile2.x, tile2.x + tile2.w - 1);
			int num3 = global::UnityEngine.Mathf.Clamp(tile2.z, tile1.z, tile1.z + tile1.d - 1);
			int num4 = global::UnityEngine.Mathf.Clamp(tile1.z, tile2.z, tile2.z + tile2.d - 1);
			int num5;
			int i;
			int num6;
			int num7;
			float num8;
			if (num == num2)
			{
				num5 = 2;
				i = 0;
				num6 = num3;
				num7 = num4;
				num8 = (float)this.tileSizeZ * this.cellSize;
			}
			else
			{
				if (num3 != num4)
				{
					throw new global::System.ArgumentException("Tiles are not adjacent (neither x or z coordinates match)");
				}
				num5 = 0;
				i = 2;
				num6 = num;
				num7 = num2;
				num8 = (float)this.tileSizeX * this.cellSize;
			}
			if (global::System.Math.Abs(num6 - num7) != 1)
			{
				global::UnityEngine.Debug.Log(string.Concat(new object[]
				{
					tile1.x,
					" ",
					tile1.z,
					" ",
					tile1.w,
					" ",
					tile1.d,
					"\n",
					tile2.x,
					" ",
					tile2.z,
					" ",
					tile2.w,
					" ",
					tile2.d,
					"\n",
					num,
					" ",
					num3,
					" ",
					num2,
					" ",
					num4
				}));
				throw new global::System.ArgumentException(string.Concat(new object[]
				{
					"Tiles are not adjacent (tile coordinates must differ by exactly 1. Got '",
					num6,
					"' and '",
					num7,
					"')"
				}));
			}
			int num9 = (int)global::System.Math.Round((double)(((float)global::System.Math.Max(num6, num7) * num8 + this.forcedBounds.min[num5]) * 1000f));
			global::Pathfinding.TriangleMeshNode[] nodes = tile1.nodes;
			global::Pathfinding.TriangleMeshNode[] nodes2 = tile2.nodes;
			foreach (global::Pathfinding.TriangleMeshNode triangleMeshNode in nodes)
			{
				int vertexCount = triangleMeshNode.GetVertexCount();
				for (int k = 0; k < vertexCount; k++)
				{
					global::Pathfinding.Int3 vertex = triangleMeshNode.GetVertex(k);
					global::Pathfinding.Int3 vertex2 = triangleMeshNode.GetVertex((k + 1) % vertexCount);
					if (global::System.Math.Abs(vertex[num5] - num9) < 2 && global::System.Math.Abs(vertex2[num5] - num9) < 2)
					{
						int num10 = global::System.Math.Min(vertex[i], vertex2[i]);
						int num11 = global::System.Math.Max(vertex[i], vertex2[i]);
						if (num10 != num11)
						{
							foreach (global::Pathfinding.TriangleMeshNode triangleMeshNode2 in nodes2)
							{
								int vertexCount2 = triangleMeshNode2.GetVertexCount();
								for (int m = 0; m < vertexCount2; m++)
								{
									global::Pathfinding.Int3 vertex3 = triangleMeshNode2.GetVertex(m);
									global::Pathfinding.Int3 vertex4 = triangleMeshNode2.GetVertex((m + 1) % vertexCount);
									if (global::System.Math.Abs(vertex3[num5] - num9) < 2 && global::System.Math.Abs(vertex4[num5] - num9) < 2)
									{
										int num12 = global::System.Math.Min(vertex3[i], vertex4[i]);
										int num13 = global::System.Math.Max(vertex3[i], vertex4[i]);
										if (num12 != num13)
										{
											if (num11 > num12 && num10 < num13 && ((vertex == vertex3 && vertex2 == vertex4) || (vertex == vertex4 && vertex2 == vertex3) || global::Pathfinding.VectorMath.SqrDistanceSegmentSegment((global::UnityEngine.Vector3)vertex, (global::UnityEngine.Vector3)vertex2, (global::UnityEngine.Vector3)vertex3, (global::UnityEngine.Vector3)vertex4) < this.walkableClimb * this.walkableClimb))
											{
												uint costMagnitude = (uint)(triangleMeshNode.position - triangleMeshNode2.position).costMagnitude;
												triangleMeshNode.AddConnection(triangleMeshNode2, costMagnitude);
												triangleMeshNode2.AddConnection(triangleMeshNode, costMagnitude);
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

		public void StartBatchTileUpdate()
		{
			if (this.batchTileUpdate)
			{
				throw new global::System.InvalidOperationException("Calling StartBatchLoad when batching is already enabled");
			}
			this.batchTileUpdate = true;
		}

		public void EndBatchTileUpdate()
		{
			if (!this.batchTileUpdate)
			{
				throw new global::System.InvalidOperationException("Calling EndBatchLoad when batching not enabled");
			}
			this.batchTileUpdate = false;
			int num = this.tileXCount;
			int num2 = this.tileZCount;
			for (int i = 0; i < num2; i++)
			{
				for (int j = 0; j < num; j++)
				{
					this.tiles[j + i * this.tileXCount].flag = false;
				}
			}
			for (int k = 0; k < this.batchUpdatedTiles.Count; k++)
			{
				this.tiles[this.batchUpdatedTiles[k]].flag = true;
			}
			for (int l = 0; l < num2; l++)
			{
				for (int m = 0; m < num; m++)
				{
					if (m < num - 1 && (this.tiles[m + l * this.tileXCount].flag || this.tiles[m + 1 + l * this.tileXCount].flag) && this.tiles[m + l * this.tileXCount] != this.tiles[m + 1 + l * this.tileXCount])
					{
						this.ConnectTiles(this.tiles[m + l * this.tileXCount], this.tiles[m + 1 + l * this.tileXCount]);
					}
					if (l < num2 - 1 && (this.tiles[m + l * this.tileXCount].flag || this.tiles[m + (l + 1) * this.tileXCount].flag) && this.tiles[m + l * this.tileXCount] != this.tiles[m + (l + 1) * this.tileXCount])
					{
						this.ConnectTiles(this.tiles[m + l * this.tileXCount], this.tiles[m + (l + 1) * this.tileXCount]);
					}
				}
			}
			this.batchUpdatedTiles.Clear();
		}

		private void ClearTiles(int x, int z, int w, int d)
		{
			for (int i = z; i < z + d; i++)
			{
				for (int j = x; j < x + w; j++)
				{
					global::Pathfinding.RecastGraph.NavmeshTile navmeshTile = this.tiles[j + i * this.tileXCount];
					if (navmeshTile != null)
					{
						this.RemoveConnectionsFromTile(navmeshTile);
						for (int k = 0; k < navmeshTile.nodes.Length; k++)
						{
							navmeshTile.nodes[k].Destroy();
						}
						for (int l = navmeshTile.z; l < navmeshTile.z + navmeshTile.d; l++)
						{
							for (int m = navmeshTile.x; m < navmeshTile.x + navmeshTile.w; m++)
							{
								global::Pathfinding.RecastGraph.NavmeshTile navmeshTile2 = this.tiles[m + l * this.tileXCount];
								if (navmeshTile2 == null || navmeshTile2 != navmeshTile)
								{
									throw new global::System.Exception("This should not happen");
								}
								if (l < z || l >= z + d || m < x || m >= x + w)
								{
									this.tiles[m + l * this.tileXCount] = global::Pathfinding.RecastGraph.NewEmptyTile(m, l);
									if (this.batchTileUpdate)
									{
										this.batchUpdatedTiles.Add(m + l * this.tileXCount);
									}
								}
								else
								{
									this.tiles[m + l * this.tileXCount] = null;
								}
							}
						}
						global::Pathfinding.Util.ObjectPool<global::Pathfinding.BBTree>.Release(ref navmeshTile.bbTree);
					}
				}
			}
		}

		public void ReplaceTile(int x, int z, global::Pathfinding.Int3[] verts, int[] tris, bool worldSpace)
		{
			this.ReplaceTile(x, z, 1, 1, verts, tris, worldSpace);
		}

		public void ReplaceTile(int x, int z, int w, int d, global::Pathfinding.Int3[] verts, int[] tris, bool worldSpace)
		{
			if (x + w > this.tileXCount || z + d > this.tileZCount || x < 0 || z < 0)
			{
				throw new global::System.ArgumentException(string.Concat(new object[]
				{
					"Tile is placed at an out of bounds position or extends out of the graph bounds (",
					x,
					", ",
					z,
					" [",
					w,
					", ",
					d,
					"] ",
					this.tileXCount,
					" ",
					this.tileZCount,
					")"
				}));
			}
			if (w < 1 || d < 1)
			{
				throw new global::System.ArgumentException(string.Concat(new object[]
				{
					"width and depth must be greater or equal to 1. Was ",
					w,
					", ",
					d
				}));
			}
			this.ClearTiles(x, z, w, d);
			global::Pathfinding.RecastGraph.NavmeshTile navmeshTile = new global::Pathfinding.RecastGraph.NavmeshTile
			{
				x = x,
				z = z,
				w = w,
				d = d,
				tris = tris,
				verts = verts,
				bbTree = global::Pathfinding.Util.ObjectPool<global::Pathfinding.BBTree>.Claim()
			};
			if (navmeshTile.tris.Length % 3 != 0)
			{
				throw new global::System.ArgumentException("Triangle array's length must be a multiple of 3 (tris)");
			}
			if (navmeshTile.verts.Length > 65535)
			{
				throw new global::System.ArgumentException("Too many vertices per tile (more than 65535)");
			}
			if (!worldSpace)
			{
				if (!global::UnityEngine.Mathf.Approximately((float)(x * this.tileSizeX) * this.cellSize * 1000f, (float)global::System.Math.Round((double)((float)(x * this.tileSizeX) * this.cellSize * 1000f))))
				{
					global::UnityEngine.Debug.LogWarning("Possible numerical imprecision. Consider adjusting tileSize and/or cellSize");
				}
				if (!global::UnityEngine.Mathf.Approximately((float)(z * this.tileSizeZ) * this.cellSize * 1000f, (float)global::System.Math.Round((double)((float)(z * this.tileSizeZ) * this.cellSize * 1000f))))
				{
					global::UnityEngine.Debug.LogWarning("Possible numerical imprecision. Consider adjusting tileSize and/or cellSize");
				}
				global::Pathfinding.Int3 rhs = (global::Pathfinding.Int3)(new global::UnityEngine.Vector3((float)(x * this.tileSizeX) * this.cellSize, 0f, (float)(z * this.tileSizeZ) * this.cellSize) + this.forcedBounds.min);
				for (int i = 0; i < verts.Length; i++)
				{
					verts[i] += rhs;
				}
			}
			global::Pathfinding.TriangleMeshNode[] array = new global::Pathfinding.TriangleMeshNode[navmeshTile.tris.Length / 3];
			navmeshTile.nodes = array;
			int graphIndex = global::AstarPath.active.astarData.graphs.Length;
			global::Pathfinding.TriangleMeshNode.SetNavmeshHolder(graphIndex, navmeshTile);
			int num = x + z * this.tileXCount;
			num <<= 12;
			if (navmeshTile.verts.Length > 4095)
			{
				global::UnityEngine.Debug.LogError(string.Concat(new object[]
				{
					"Too many vertices in the tile (",
					navmeshTile.verts.Length,
					" > ",
					4095,
					")\nYou can enable ASTAR_RECAST_LARGER_TILES under the 'Optimizations' tab in the A* Inspector to raise this limit."
				}));
				this.tiles[num] = global::Pathfinding.RecastGraph.NewEmptyTile(x, z);
				return;
			}
			for (int j = 0; j < array.Length; j++)
			{
				global::Pathfinding.TriangleMeshNode triangleMeshNode = new global::Pathfinding.TriangleMeshNode(this.active);
				array[j] = triangleMeshNode;
				triangleMeshNode.GraphIndex = (uint)graphIndex;
				triangleMeshNode.v0 = (navmeshTile.tris[j * 3] | num);
				triangleMeshNode.v1 = (navmeshTile.tris[j * 3 + 1] | num);
				triangleMeshNode.v2 = (navmeshTile.tris[j * 3 + 2] | num);
				if (!global::Pathfinding.VectorMath.IsClockwiseXZ(triangleMeshNode.GetVertex(0), triangleMeshNode.GetVertex(1), triangleMeshNode.GetVertex(2)))
				{
					int v = triangleMeshNode.v0;
					triangleMeshNode.v0 = triangleMeshNode.v2;
					triangleMeshNode.v2 = v;
				}
				triangleMeshNode.Walkable = true;
				triangleMeshNode.Penalty = this.initialPenalty;
				triangleMeshNode.UpdatePositionFromVertices();
			}
			navmeshTile.bbTree.RebuildFrom(array);
			this.CreateNodeConnections(navmeshTile.nodes);
			for (int k = z; k < z + d; k++)
			{
				for (int l = x; l < x + w; l++)
				{
					this.tiles[l + k * this.tileXCount] = navmeshTile;
				}
			}
			if (this.batchTileUpdate)
			{
				this.batchUpdatedTiles.Add(x + z * this.tileXCount);
			}
			else
			{
				this.ConnectTileWithNeighbours(navmeshTile, false);
			}
			global::Pathfinding.TriangleMeshNode.SetNavmeshHolder(graphIndex, null);
			graphIndex = global::AstarPath.active.astarData.GetGraphIndex(this);
			for (int m = 0; m < array.Length; m++)
			{
				array[m].GraphIndex = (uint)graphIndex;
			}
		}

		public bool Linecast(global::UnityEngine.Vector3 origin, global::UnityEngine.Vector3 end)
		{
			return this.Linecast(origin, end, base.GetNearest(origin, global::Pathfinding.NNConstraint.None).node);
		}

		public bool Linecast(global::UnityEngine.Vector3 origin, global::UnityEngine.Vector3 end, global::Pathfinding.GraphNode hint, out global::Pathfinding.GraphHitInfo hit)
		{
			return global::Pathfinding.NavMeshGraph.Linecast(this, origin, end, hint, out hit, null);
		}

		public bool Linecast(global::UnityEngine.Vector3 origin, global::UnityEngine.Vector3 end, global::Pathfinding.GraphNode hint)
		{
			global::Pathfinding.GraphHitInfo graphHitInfo;
			return global::Pathfinding.NavMeshGraph.Linecast(this, origin, end, hint, out graphHitInfo, null);
		}

		public bool Linecast(global::UnityEngine.Vector3 tmp_origin, global::UnityEngine.Vector3 tmp_end, global::Pathfinding.GraphNode hint, out global::Pathfinding.GraphHitInfo hit, global::System.Collections.Generic.List<global::Pathfinding.GraphNode> trace)
		{
			return global::Pathfinding.NavMeshGraph.Linecast(this, tmp_origin, tmp_end, hint, out hit, trace);
		}

		public override void OnDrawGizmos(bool drawNodes)
		{
			if (!drawNodes)
			{
				return;
			}
			global::UnityEngine.Gizmos.color = global::UnityEngine.Color.white;
			global::UnityEngine.Gizmos.DrawWireCube(this.forcedBounds.center, this.forcedBounds.size);
			global::Pathfinding.PathHandler debugData = global::AstarPath.active.debugPathData;
			global::Pathfinding.GraphNodeDelegateCancelable del = delegate(global::Pathfinding.GraphNode _node)
			{
				global::Pathfinding.TriangleMeshNode triangleMeshNode = _node as global::Pathfinding.TriangleMeshNode;
				if (global::AstarPath.active.showSearchTree && debugData != null)
				{
					bool flag = global::Pathfinding.NavGraph.InSearchTree(triangleMeshNode, global::AstarPath.active.debugPath);
					if (flag && this.showNodeConnections)
					{
						global::Pathfinding.PathNode pathNode = debugData.GetPathNode(triangleMeshNode);
						if (pathNode.parent != null)
						{
							global::UnityEngine.Gizmos.color = this.NodeColor(triangleMeshNode, debugData);
							global::UnityEngine.Gizmos.DrawLine((global::UnityEngine.Vector3)triangleMeshNode.position, (global::UnityEngine.Vector3)debugData.GetPathNode(triangleMeshNode).parent.node.position);
						}
					}
					if (this.showMeshOutline)
					{
						global::UnityEngine.Gizmos.color = ((!triangleMeshNode.Walkable) ? global::Pathfinding.AstarColor.UnwalkableNode : this.NodeColor(triangleMeshNode, debugData));
						if (!flag)
						{
							global::UnityEngine.Gizmos.color *= new global::UnityEngine.Color(1f, 1f, 1f, 0.1f);
						}
						global::UnityEngine.Gizmos.DrawLine((global::UnityEngine.Vector3)triangleMeshNode.GetVertex(0), (global::UnityEngine.Vector3)triangleMeshNode.GetVertex(1));
						global::UnityEngine.Gizmos.DrawLine((global::UnityEngine.Vector3)triangleMeshNode.GetVertex(1), (global::UnityEngine.Vector3)triangleMeshNode.GetVertex(2));
						global::UnityEngine.Gizmos.DrawLine((global::UnityEngine.Vector3)triangleMeshNode.GetVertex(2), (global::UnityEngine.Vector3)triangleMeshNode.GetVertex(0));
					}
				}
				else
				{
					if (this.showNodeConnections)
					{
						global::UnityEngine.Gizmos.color = this.NodeColor(triangleMeshNode, null);
						for (int i = 0; i < triangleMeshNode.connections.Length; i++)
						{
							global::UnityEngine.Gizmos.DrawLine((global::UnityEngine.Vector3)triangleMeshNode.position, global::UnityEngine.Vector3.Lerp((global::UnityEngine.Vector3)triangleMeshNode.connections[i].position, (global::UnityEngine.Vector3)triangleMeshNode.position, 0.4f));
						}
					}
					if (this.showMeshOutline)
					{
						global::UnityEngine.Gizmos.color = ((!triangleMeshNode.Walkable) ? global::Pathfinding.AstarColor.UnwalkableNode : this.NodeColor(triangleMeshNode, debugData));
						global::UnityEngine.Gizmos.DrawLine((global::UnityEngine.Vector3)triangleMeshNode.GetVertex(0), (global::UnityEngine.Vector3)triangleMeshNode.GetVertex(1));
						global::UnityEngine.Gizmos.DrawLine((global::UnityEngine.Vector3)triangleMeshNode.GetVertex(1), (global::UnityEngine.Vector3)triangleMeshNode.GetVertex(2));
						global::UnityEngine.Gizmos.DrawLine((global::UnityEngine.Vector3)triangleMeshNode.GetVertex(2), (global::UnityEngine.Vector3)triangleMeshNode.GetVertex(0));
					}
				}
				return true;
			};
			this.GetNodes(del);
		}

		public override void DeserializeSettingsCompatibility(global::Pathfinding.Serialization.GraphSerializationContext ctx)
		{
			base.DeserializeSettingsCompatibility(ctx);
			this.characterRadius = ctx.reader.ReadSingle();
			this.contourMaxError = ctx.reader.ReadSingle();
			this.cellSize = ctx.reader.ReadSingle();
			ctx.reader.ReadSingle();
			this.walkableHeight = ctx.reader.ReadSingle();
			this.maxSlope = ctx.reader.ReadSingle();
			this.maxEdgeLength = ctx.reader.ReadSingle();
			this.editorTileSize = ctx.reader.ReadInt32();
			this.tileSizeX = ctx.reader.ReadInt32();
			this.nearestSearchOnlyXZ = ctx.reader.ReadBoolean();
			this.useTiles = ctx.reader.ReadBoolean();
			this.relevantGraphSurfaceMode = (global::Pathfinding.RecastGraph.RelevantGraphSurfaceMode)ctx.reader.ReadInt32();
			this.rasterizeColliders = ctx.reader.ReadBoolean();
			this.rasterizeMeshes = ctx.reader.ReadBoolean();
			this.rasterizeTerrain = ctx.reader.ReadBoolean();
			this.rasterizeTrees = ctx.reader.ReadBoolean();
			this.colliderRasterizeDetail = ctx.reader.ReadSingle();
			this.forcedBoundsCenter = ctx.DeserializeVector3();
			this.forcedBoundsSize = ctx.DeserializeVector3();
			this.mask = ctx.reader.ReadInt32();
			int num = ctx.reader.ReadInt32();
			this.tagMask = new global::System.Collections.Generic.List<string>(num);
			for (int i = 0; i < num; i++)
			{
				this.tagMask.Add(ctx.reader.ReadString());
			}
			this.showMeshOutline = ctx.reader.ReadBoolean();
			this.showNodeConnections = ctx.reader.ReadBoolean();
			this.terrainSampleSize = ctx.reader.ReadInt32();
			this.walkableClimb = ctx.DeserializeFloat(this.walkableClimb);
			this.minRegionSize = ctx.DeserializeFloat(this.minRegionSize);
			this.tileSizeZ = ctx.DeserializeInt(this.tileSizeX);
			this.showMeshSurface = ctx.reader.ReadBoolean();
		}

		public override void SerializeExtraInfo(global::Pathfinding.Serialization.GraphSerializationContext ctx)
		{
			global::System.IO.BinaryWriter writer = ctx.writer;
			if (this.tiles == null)
			{
				writer.Write(-1);
				return;
			}
			writer.Write(this.tileXCount);
			writer.Write(this.tileZCount);
			for (int i = 0; i < this.tileZCount; i++)
			{
				for (int j = 0; j < this.tileXCount; j++)
				{
					global::Pathfinding.RecastGraph.NavmeshTile navmeshTile = this.tiles[j + i * this.tileXCount];
					if (navmeshTile == null)
					{
						throw new global::System.Exception("NULL Tile");
					}
					writer.Write(navmeshTile.x);
					writer.Write(navmeshTile.z);
					if (navmeshTile.x == j && navmeshTile.z == i)
					{
						writer.Write(navmeshTile.w);
						writer.Write(navmeshTile.d);
						writer.Write(navmeshTile.tris.Length);
						for (int k = 0; k < navmeshTile.tris.Length; k++)
						{
							writer.Write(navmeshTile.tris[k]);
						}
						writer.Write(navmeshTile.verts.Length);
						for (int l = 0; l < navmeshTile.verts.Length; l++)
						{
							ctx.SerializeInt3(navmeshTile.verts[l]);
						}
						writer.Write(navmeshTile.nodes.Length);
						for (int m = 0; m < navmeshTile.nodes.Length; m++)
						{
							navmeshTile.nodes[m].SerializeNode(ctx);
						}
					}
				}
			}
		}

		public override void DeserializeExtraInfo(global::Pathfinding.Serialization.GraphSerializationContext ctx)
		{
			global::System.IO.BinaryReader reader = ctx.reader;
			this.tileXCount = reader.ReadInt32();
			if (this.tileXCount < 0)
			{
				return;
			}
			this.tileZCount = reader.ReadInt32();
			this.tiles = new global::Pathfinding.RecastGraph.NavmeshTile[this.tileXCount * this.tileZCount];
			global::Pathfinding.TriangleMeshNode.SetNavmeshHolder((int)ctx.graphIndex, this);
			for (int i = 0; i < this.tileZCount; i++)
			{
				for (int j = 0; j < this.tileXCount; j++)
				{
					int num = j + i * this.tileXCount;
					int num2 = reader.ReadInt32();
					if (num2 < 0)
					{
						throw new global::System.Exception("Invalid tile coordinates (x < 0)");
					}
					int num3 = reader.ReadInt32();
					if (num3 < 0)
					{
						throw new global::System.Exception("Invalid tile coordinates (z < 0)");
					}
					if (num2 != j || num3 != i)
					{
						this.tiles[num] = this.tiles[num3 * this.tileXCount + num2];
					}
					else
					{
						global::Pathfinding.RecastGraph.NavmeshTile navmeshTile = new global::Pathfinding.RecastGraph.NavmeshTile();
						navmeshTile.x = num2;
						navmeshTile.z = num3;
						navmeshTile.w = reader.ReadInt32();
						navmeshTile.d = reader.ReadInt32();
						navmeshTile.bbTree = global::Pathfinding.Util.ObjectPool<global::Pathfinding.BBTree>.Claim();
						this.tiles[num] = navmeshTile;
						int num4 = reader.ReadInt32();
						if (num4 % 3 != 0)
						{
							throw new global::System.Exception("Corrupt data. Triangle indices count must be divisable by 3. Got " + num4);
						}
						navmeshTile.tris = new int[num4];
						for (int k = 0; k < navmeshTile.tris.Length; k++)
						{
							navmeshTile.tris[k] = reader.ReadInt32();
						}
						navmeshTile.verts = new global::Pathfinding.Int3[reader.ReadInt32()];
						for (int l = 0; l < navmeshTile.verts.Length; l++)
						{
							navmeshTile.verts[l] = ctx.DeserializeInt3();
						}
						int num5 = reader.ReadInt32();
						navmeshTile.nodes = new global::Pathfinding.TriangleMeshNode[num5];
						num <<= 12;
						for (int m = 0; m < navmeshTile.nodes.Length; m++)
						{
							global::Pathfinding.TriangleMeshNode triangleMeshNode = new global::Pathfinding.TriangleMeshNode(this.active);
							navmeshTile.nodes[m] = triangleMeshNode;
							triangleMeshNode.DeserializeNode(ctx);
							triangleMeshNode.v0 = (navmeshTile.tris[m * 3] | num);
							triangleMeshNode.v1 = (navmeshTile.tris[m * 3 + 1] | num);
							triangleMeshNode.v2 = (navmeshTile.tris[m * 3 + 2] | num);
							triangleMeshNode.UpdatePositionFromVertices();
						}
						navmeshTile.bbTree.RebuildFrom(navmeshTile.nodes);
					}
				}
			}
		}

		public const int VertexIndexMask = 4095;

		public const int TileIndexMask = 524287;

		public const int TileIndexOffset = 12;

		public const int BorderVertexMask = 1;

		public const int BorderVertexOffset = 31;

		public bool dynamic = true;

		[global::Pathfinding.Serialization.JsonMember]
		public float characterRadius = 1.5f;

		[global::Pathfinding.Serialization.JsonMember]
		public float contourMaxError = 2f;

		[global::Pathfinding.Serialization.JsonMember]
		public float cellSize = 0.5f;

		[global::Pathfinding.Serialization.JsonMember]
		public float walkableHeight = 2f;

		[global::Pathfinding.Serialization.JsonMember]
		public float walkableClimb = 0.5f;

		[global::Pathfinding.Serialization.JsonMember]
		public float maxSlope = 30f;

		[global::Pathfinding.Serialization.JsonMember]
		public float maxEdgeLength = 20f;

		[global::Pathfinding.Serialization.JsonMember]
		public float minRegionSize = 3f;

		[global::Pathfinding.Serialization.JsonMember]
		public int editorTileSize = 128;

		[global::Pathfinding.Serialization.JsonMember]
		public int tileSizeX = 128;

		[global::Pathfinding.Serialization.JsonMember]
		public int tileSizeZ = 128;

		[global::Pathfinding.Serialization.JsonMember]
		public bool nearestSearchOnlyXZ;

		[global::Pathfinding.Serialization.JsonMember]
		public bool useTiles;

		public bool scanEmptyGraph;

		[global::Pathfinding.Serialization.JsonMember]
		public global::Pathfinding.RecastGraph.RelevantGraphSurfaceMode relevantGraphSurfaceMode;

		[global::Pathfinding.Serialization.JsonMember]
		public bool rasterizeColliders;

		[global::Pathfinding.Serialization.JsonMember]
		public bool rasterizeMeshes = true;

		[global::Pathfinding.Serialization.JsonMember]
		public bool rasterizeTerrain = true;

		[global::Pathfinding.Serialization.JsonMember]
		public bool rasterizeTrees = true;

		[global::Pathfinding.Serialization.JsonMember]
		public float colliderRasterizeDetail = 10f;

		[global::Pathfinding.Serialization.JsonMember]
		public global::UnityEngine.Vector3 forcedBoundsCenter;

		[global::Pathfinding.Serialization.JsonMember]
		public global::UnityEngine.Vector3 forcedBoundsSize = new global::UnityEngine.Vector3(100f, 40f, 100f);

		[global::Pathfinding.Serialization.JsonMember]
		public global::UnityEngine.LayerMask mask = -1;

		[global::Pathfinding.Serialization.JsonMember]
		public global::System.Collections.Generic.List<string> tagMask = new global::System.Collections.Generic.List<string>();

		[global::Pathfinding.Serialization.JsonMember]
		public bool showMeshOutline = true;

		[global::Pathfinding.Serialization.JsonMember]
		public bool showNodeConnections;

		[global::Pathfinding.Serialization.JsonMember]
		public bool showMeshSurface;

		[global::Pathfinding.Serialization.JsonMember]
		public int terrainSampleSize = 3;

		private global::Pathfinding.Voxels.Voxelize globalVox;

		public int tileXCount;

		public int tileZCount;

		private global::Pathfinding.RecastGraph.NavmeshTile[] tiles;

		private bool batchTileUpdate;

		private global::System.Collections.Generic.List<int> batchUpdatedTiles = new global::System.Collections.Generic.List<int>();

		private global::System.Collections.Generic.List<global::Pathfinding.RecastGraph.NavmeshTile> stagingTiles = new global::System.Collections.Generic.List<global::Pathfinding.RecastGraph.NavmeshTile>();

		public enum RelevantGraphSurfaceMode
		{
			DoNotRequire,
			OnlyForCompletelyInsideTile,
			RequireForAll
		}

		public class NavmeshTile : global::Pathfinding.INavmesh, global::Pathfinding.INavmeshHolder
		{
			public void GetTileCoordinates(int tileIndex, out int x, out int z)
			{
				x = this.x;
				z = this.z;
			}

			public int GetVertexArrayIndex(int index)
			{
				return index & 4095;
			}

			public global::Pathfinding.Int3 GetVertex(int index)
			{
				int num = index & 4095;
				return this.verts[num];
			}

			public void GetNodes(global::Pathfinding.GraphNodeDelegateCancelable del)
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

			public int[] tris;

			public global::Pathfinding.Int3[] verts;

			public int x;

			public int z;

			public int w;

			public int d;

			public global::Pathfinding.TriangleMeshNode[] nodes;

			public global::Pathfinding.BBTree bbTree;

			public bool flag;
		}
	}
}
