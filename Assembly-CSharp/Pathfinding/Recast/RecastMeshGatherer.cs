using System;
using System.Collections.Generic;
using Pathfinding.Util;
using Pathfinding.Voxels;
using UnityEngine;

namespace Pathfinding.Recast
{
	internal class RecastMeshGatherer
	{
		public RecastMeshGatherer(global::UnityEngine.Bounds bounds, int terrainSampleSize, global::UnityEngine.LayerMask mask, global::System.Collections.Generic.List<string> tagMask, float colliderRasterizeDetail)
		{
			terrainSampleSize = global::System.Math.Max(terrainSampleSize, 1);
			this.bounds = bounds;
			this.terrainSampleSize = terrainSampleSize;
			this.mask = mask;
			this.tagMask = (tagMask ?? new global::System.Collections.Generic.List<string>());
			this.colliderRasterizeDetail = colliderRasterizeDetail;
		}

		private static global::System.Collections.Generic.List<global::UnityEngine.MeshFilter> FilterMeshes(global::UnityEngine.MeshFilter[] meshFilters, global::System.Collections.Generic.List<string> tagMask, global::UnityEngine.LayerMask layerMask)
		{
			global::System.Collections.Generic.List<global::UnityEngine.MeshFilter> list = new global::System.Collections.Generic.List<global::UnityEngine.MeshFilter>(meshFilters.Length / 3);
			foreach (global::UnityEngine.MeshFilter meshFilter in meshFilters)
			{
				global::UnityEngine.Renderer component = meshFilter.GetComponent<global::UnityEngine.Renderer>();
				if (component != null && meshFilter.sharedMesh != null && component.enabled && ((1 << meshFilter.gameObject.layer & layerMask) != 0 || tagMask.Contains(meshFilter.tag)) && meshFilter.GetComponent<global::Pathfinding.RecastMeshObj>() == null)
				{
					list.Add(meshFilter);
				}
			}
			return list;
		}

		public void CollectSceneMeshes(global::System.Collections.Generic.List<global::Pathfinding.Voxels.RasterizationMesh> meshes)
		{
			if (this.tagMask.Count > 0 || this.mask != 0)
			{
				global::UnityEngine.MeshFilter[] meshFilters = global::UnityEngine.Object.FindObjectsOfType<global::UnityEngine.MeshFilter>();
				global::System.Collections.Generic.List<global::UnityEngine.MeshFilter> list = global::Pathfinding.Recast.RecastMeshGatherer.FilterMeshes(meshFilters, this.tagMask, this.mask);
				global::System.Collections.Generic.Dictionary<global::UnityEngine.Mesh, global::UnityEngine.Vector3[]> dictionary = new global::System.Collections.Generic.Dictionary<global::UnityEngine.Mesh, global::UnityEngine.Vector3[]>();
				global::System.Collections.Generic.Dictionary<global::UnityEngine.Mesh, int[]> dictionary2 = new global::System.Collections.Generic.Dictionary<global::UnityEngine.Mesh, int[]>();
				bool flag = false;
				for (int i = 0; i < list.Count; i++)
				{
					global::UnityEngine.MeshFilter meshFilter = list[i];
					global::UnityEngine.Renderer component = meshFilter.GetComponent<global::UnityEngine.Renderer>();
					if (component.isPartOfStaticBatch)
					{
						flag = true;
					}
					else if (component.bounds.Intersects(this.bounds))
					{
						global::UnityEngine.Mesh sharedMesh = meshFilter.sharedMesh;
						global::Pathfinding.Voxels.RasterizationMesh rasterizationMesh = new global::Pathfinding.Voxels.RasterizationMesh();
						rasterizationMesh.matrix = component.localToWorldMatrix;
						rasterizationMesh.original = meshFilter;
						if (dictionary.ContainsKey(sharedMesh))
						{
							rasterizationMesh.vertices = dictionary[sharedMesh];
							rasterizationMesh.triangles = dictionary2[sharedMesh];
						}
						else
						{
							rasterizationMesh.vertices = sharedMesh.vertices;
							rasterizationMesh.triangles = sharedMesh.triangles;
							dictionary[sharedMesh] = rasterizationMesh.vertices;
							dictionary2[sharedMesh] = rasterizationMesh.triangles;
						}
						rasterizationMesh.bounds = component.bounds;
						meshes.Add(rasterizationMesh);
					}
					if (flag)
					{
						global::UnityEngine.Debug.LogWarning("Some meshes were statically batched. These meshes can not be used for navmesh calculation due to technical constraints.\nDuring runtime scripts cannot access the data of meshes which have been statically batched.\nOne way to solve this problem is to use cached startup (Save & Load tab in the inspector) to only calculate the graph when the game is not playing.");
					}
				}
			}
		}

		public void CollectRecastMeshObjs(global::System.Collections.Generic.List<global::Pathfinding.Voxels.RasterizationMesh> buffer)
		{
			global::System.Collections.Generic.List<global::Pathfinding.RecastMeshObj> list = global::Pathfinding.Util.ListPool<global::Pathfinding.RecastMeshObj>.Claim();
			global::Pathfinding.RecastMeshObj.GetAllInBounds(list, this.bounds);
			global::System.Collections.Generic.Dictionary<global::UnityEngine.Mesh, global::UnityEngine.Vector3[]> dictionary = new global::System.Collections.Generic.Dictionary<global::UnityEngine.Mesh, global::UnityEngine.Vector3[]>();
			global::System.Collections.Generic.Dictionary<global::UnityEngine.Mesh, int[]> dictionary2 = new global::System.Collections.Generic.Dictionary<global::UnityEngine.Mesh, int[]>();
			for (int i = 0; i < list.Count; i++)
			{
				global::UnityEngine.MeshFilter meshFilter = list[i].GetMeshFilter();
				global::UnityEngine.Renderer renderer = (!(meshFilter != null)) ? null : meshFilter.GetComponent<global::UnityEngine.Renderer>();
				if (meshFilter != null && renderer != null)
				{
					global::UnityEngine.Mesh sharedMesh = meshFilter.sharedMesh;
					global::Pathfinding.Voxels.RasterizationMesh rasterizationMesh = new global::Pathfinding.Voxels.RasterizationMesh();
					rasterizationMesh.matrix = renderer.localToWorldMatrix;
					rasterizationMesh.original = meshFilter;
					rasterizationMesh.area = list[i].area;
					if (dictionary.ContainsKey(sharedMesh))
					{
						rasterizationMesh.vertices = dictionary[sharedMesh];
						rasterizationMesh.triangles = dictionary2[sharedMesh];
					}
					else
					{
						rasterizationMesh.vertices = sharedMesh.vertices;
						rasterizationMesh.triangles = sharedMesh.triangles;
						dictionary[sharedMesh] = rasterizationMesh.vertices;
						dictionary2[sharedMesh] = rasterizationMesh.triangles;
					}
					rasterizationMesh.bounds = renderer.bounds;
					buffer.Add(rasterizationMesh);
				}
				else
				{
					global::UnityEngine.Collider collider = list[i].GetCollider();
					if (collider == null)
					{
						global::UnityEngine.Debug.LogError("RecastMeshObject (" + list[i].gameObject.name + ") didn't have a collider or MeshFilter+Renderer attached", list[i].gameObject);
					}
					else
					{
						global::Pathfinding.Voxels.RasterizationMesh rasterizationMesh2 = this.RasterizeCollider(collider);
						if (rasterizationMesh2 != null)
						{
							rasterizationMesh2.area = list[i].area;
							buffer.Add(rasterizationMesh2);
						}
					}
				}
			}
			this.capsuleCache.Clear();
			global::Pathfinding.Util.ListPool<global::Pathfinding.RecastMeshObj>.Release(list);
		}

		public void CollectTerrainMeshes(bool rasterizeTrees, float desiredChunkSize, global::System.Collections.Generic.List<global::Pathfinding.Voxels.RasterizationMesh> result)
		{
			global::UnityEngine.Terrain[] array = global::UnityEngine.Object.FindObjectsOfType(typeof(global::UnityEngine.Terrain)) as global::UnityEngine.Terrain[];
			if (array.Length > 0)
			{
				for (int i = 0; i < array.Length; i++)
				{
					if (!(array[i].terrainData == null))
					{
						this.GenerateTerrainChunks(array[i], this.bounds, desiredChunkSize, result);
						if (rasterizeTrees)
						{
							this.CollectTreeMeshes(array[i], result);
						}
					}
				}
			}
		}

		private void GenerateTerrainChunks(global::UnityEngine.Terrain terrain, global::UnityEngine.Bounds bounds, float desiredChunkSize, global::System.Collections.Generic.List<global::Pathfinding.Voxels.RasterizationMesh> result)
		{
			global::UnityEngine.TerrainData terrainData = terrain.terrainData;
			if (terrainData == null)
			{
				throw new global::System.ArgumentException("Terrain contains no terrain data");
			}
			global::UnityEngine.Vector3 position = terrain.GetPosition();
			global::UnityEngine.Vector3 center = position + terrainData.size * 0.5f;
			global::UnityEngine.Bounds bounds2 = new global::UnityEngine.Bounds(center, terrainData.size);
			if (!bounds2.Intersects(bounds))
			{
				return;
			}
			int heightmapWidth = terrainData.heightmapWidth;
			int heightmapHeight = terrainData.heightmapHeight;
			float[,] heights = terrainData.GetHeights(0, 0, heightmapWidth, heightmapHeight);
			global::UnityEngine.Vector3 heightmapScale = terrainData.heightmapScale;
			heightmapScale.y = terrainData.size.y;
			int num = global::UnityEngine.Mathf.CeilToInt(global::UnityEngine.Mathf.Max(desiredChunkSize / (heightmapScale.x * (float)this.terrainSampleSize), 12f)) * this.terrainSampleSize;
			int num2 = global::UnityEngine.Mathf.CeilToInt(global::UnityEngine.Mathf.Max(desiredChunkSize / (heightmapScale.z * (float)this.terrainSampleSize), 12f)) * this.terrainSampleSize;
			for (int i = 0; i < heightmapHeight; i += num2)
			{
				for (int j = 0; j < heightmapWidth; j += num)
				{
					int width = global::UnityEngine.Mathf.Min(num, heightmapWidth - j);
					int depth = global::UnityEngine.Mathf.Min(num2, heightmapHeight - i);
					global::Pathfinding.Voxels.RasterizationMesh item = this.GenerateHeightmapChunk(heights, heightmapScale, position, j, i, width, depth, this.terrainSampleSize);
					result.Add(item);
				}
			}
		}

		private static int CeilDivision(int lhs, int rhs)
		{
			return (lhs + rhs - 1) / rhs;
		}

		private global::Pathfinding.Voxels.RasterizationMesh GenerateHeightmapChunk(float[,] heights, global::UnityEngine.Vector3 sampleSize, global::UnityEngine.Vector3 offset, int x0, int z0, int width, int depth, int stride)
		{
			int num = global::Pathfinding.Recast.RecastMeshGatherer.CeilDivision(width, this.terrainSampleSize) + 1;
			int num2 = global::Pathfinding.Recast.RecastMeshGatherer.CeilDivision(depth, this.terrainSampleSize) + 1;
			int length = heights.GetLength(0);
			int length2 = heights.GetLength(1);
			global::UnityEngine.Vector3[] array = new global::UnityEngine.Vector3[num * num2];
			for (int i = 0; i < num2; i++)
			{
				for (int j = 0; j < num; j++)
				{
					int num3 = global::System.Math.Min(x0 + j * stride, length - 1);
					int num4 = global::System.Math.Min(z0 + i * stride, length2 - 1);
					array[i * num + j] = new global::UnityEngine.Vector3((float)num4 * sampleSize.x, heights[num3, num4] * sampleSize.y, (float)num3 * sampleSize.z) + offset;
				}
			}
			int[] array2 = new int[(num - 1) * (num2 - 1) * 2 * 3];
			int num5 = 0;
			for (int k = 0; k < num2 - 1; k++)
			{
				for (int l = 0; l < num - 1; l++)
				{
					array2[num5] = k * num + l;
					array2[num5 + 1] = k * num + l + 1;
					array2[num5 + 2] = (k + 1) * num + l + 1;
					num5 += 3;
					array2[num5] = k * num + l;
					array2[num5 + 1] = (k + 1) * num + l + 1;
					array2[num5 + 2] = (k + 1) * num + l;
					num5 += 3;
				}
			}
			global::Pathfinding.Voxels.RasterizationMesh rasterizationMesh = new global::Pathfinding.Voxels.RasterizationMesh(array, array2, default(global::UnityEngine.Bounds));
			rasterizationMesh.RecalculateBounds();
			return rasterizationMesh;
		}

		private void CollectTreeMeshes(global::UnityEngine.Terrain terrain, global::System.Collections.Generic.List<global::Pathfinding.Voxels.RasterizationMesh> result)
		{
			global::UnityEngine.TerrainData terrainData = terrain.terrainData;
			for (int i = 0; i < terrainData.treeInstances.Length; i++)
			{
				global::UnityEngine.TreeInstance treeInstance = terrainData.treeInstances[i];
				global::UnityEngine.TreePrototype treePrototype = terrainData.treePrototypes[treeInstance.prototypeIndex];
				if (!(treePrototype.prefab == null))
				{
					global::UnityEngine.Collider component = treePrototype.prefab.GetComponent<global::UnityEngine.Collider>();
					global::UnityEngine.Vector3 pos = terrain.transform.position + global::UnityEngine.Vector3.Scale(treeInstance.position, terrainData.size);
					if (component == null)
					{
						global::UnityEngine.Bounds bounds = new global::UnityEngine.Bounds(terrain.transform.position + global::UnityEngine.Vector3.Scale(treeInstance.position, terrainData.size), new global::UnityEngine.Vector3(treeInstance.widthScale, treeInstance.heightScale, treeInstance.widthScale));
						global::UnityEngine.Matrix4x4 matrix = global::UnityEngine.Matrix4x4.TRS(pos, global::UnityEngine.Quaternion.identity, new global::UnityEngine.Vector3(treeInstance.widthScale, treeInstance.heightScale, treeInstance.widthScale) * 0.5f);
						global::Pathfinding.Voxels.RasterizationMesh item = new global::Pathfinding.Voxels.RasterizationMesh(global::Pathfinding.Recast.RecastMeshGatherer.BoxColliderVerts, global::Pathfinding.Recast.RecastMeshGatherer.BoxColliderTris, bounds, matrix);
						result.Add(item);
					}
					else
					{
						global::UnityEngine.Vector3 s = new global::UnityEngine.Vector3(treeInstance.widthScale, treeInstance.heightScale, treeInstance.widthScale);
						global::Pathfinding.Voxels.RasterizationMesh rasterizationMesh = this.RasterizeCollider(component, global::UnityEngine.Matrix4x4.TRS(pos, global::UnityEngine.Quaternion.identity, s));
						if (rasterizationMesh != null)
						{
							rasterizationMesh.RecalculateBounds();
							result.Add(rasterizationMesh);
						}
					}
				}
			}
		}

		public void CollectColliderMeshes(global::System.Collections.Generic.List<global::Pathfinding.Voxels.RasterizationMesh> result)
		{
			global::UnityEngine.Collider[] array = global::UnityEngine.Object.FindObjectsOfType<global::UnityEngine.Collider>();
			if (this.tagMask.Count > 0 || this.mask != 0)
			{
				foreach (global::UnityEngine.Collider collider in array)
				{
					if (((this.mask >> collider.gameObject.layer & 1) != 0 || this.tagMask.Contains(collider.tag)) && collider.enabled && !collider.isTrigger && collider.bounds.Intersects(this.bounds) && collider.GetComponent<global::Pathfinding.RecastMeshObj>() == null)
					{
						global::Pathfinding.Voxels.RasterizationMesh rasterizationMesh = this.RasterizeCollider(collider);
						if (rasterizationMesh != null)
						{
							result.Add(rasterizationMesh);
						}
					}
				}
			}
			this.capsuleCache.Clear();
		}

		private global::Pathfinding.Voxels.RasterizationMesh RasterizeCollider(global::UnityEngine.Collider col)
		{
			return this.RasterizeCollider(col, col.transform.localToWorldMatrix);
		}

		private global::Pathfinding.Voxels.RasterizationMesh RasterizeCollider(global::UnityEngine.Collider col, global::UnityEngine.Matrix4x4 localToWorldMatrix)
		{
			global::Pathfinding.Voxels.RasterizationMesh result = null;
			if (col is global::UnityEngine.BoxCollider)
			{
				result = this.RasterizeBoxCollider(col as global::UnityEngine.BoxCollider, localToWorldMatrix);
			}
			else if (col is global::UnityEngine.SphereCollider || col is global::UnityEngine.CapsuleCollider)
			{
				global::UnityEngine.SphereCollider sphereCollider = col as global::UnityEngine.SphereCollider;
				global::UnityEngine.CapsuleCollider capsuleCollider = col as global::UnityEngine.CapsuleCollider;
				float num = (!(sphereCollider != null)) ? capsuleCollider.radius : sphereCollider.radius;
				float height = (!(sphereCollider != null)) ? (capsuleCollider.height * 0.5f / num - 1f) : 0f;
				global::UnityEngine.Matrix4x4 matrix4x = global::UnityEngine.Matrix4x4.TRS((!(sphereCollider != null)) ? capsuleCollider.center : sphereCollider.center, global::UnityEngine.Quaternion.identity, global::UnityEngine.Vector3.one * num);
				matrix4x = localToWorldMatrix * matrix4x;
				result = this.RasterizeCapsuleCollider(num, height, col.bounds, matrix4x);
			}
			else if (col is global::UnityEngine.MeshCollider)
			{
				global::UnityEngine.MeshCollider meshCollider = col as global::UnityEngine.MeshCollider;
				if (meshCollider.sharedMesh != null)
				{
					result = new global::Pathfinding.Voxels.RasterizationMesh(meshCollider.sharedMesh.vertices, meshCollider.sharedMesh.triangles, meshCollider.bounds, localToWorldMatrix);
				}
			}
			return result;
		}

		private global::Pathfinding.Voxels.RasterizationMesh RasterizeBoxCollider(global::UnityEngine.BoxCollider collider, global::UnityEngine.Matrix4x4 localToWorldMatrix)
		{
			global::UnityEngine.Matrix4x4 matrix4x = global::UnityEngine.Matrix4x4.TRS(collider.center, global::UnityEngine.Quaternion.identity, collider.size * 0.5f);
			matrix4x = localToWorldMatrix * matrix4x;
			return new global::Pathfinding.Voxels.RasterizationMesh(global::Pathfinding.Recast.RecastMeshGatherer.BoxColliderVerts, global::Pathfinding.Recast.RecastMeshGatherer.BoxColliderTris, collider.bounds, matrix4x);
		}

		private global::Pathfinding.Voxels.RasterizationMesh RasterizeCapsuleCollider(float radius, float height, global::UnityEngine.Bounds bounds, global::UnityEngine.Matrix4x4 localToWorldMatrix)
		{
			int num = global::UnityEngine.Mathf.Max(4, global::UnityEngine.Mathf.RoundToInt(this.colliderRasterizeDetail * global::UnityEngine.Mathf.Sqrt(localToWorldMatrix.MultiplyVector(global::UnityEngine.Vector3.one).magnitude)));
			if (num > 100)
			{
				global::UnityEngine.Debug.LogWarning("Very large detail for some collider meshes. Consider decreasing Collider Rasterize Detail (RecastGraph)");
			}
			int num2 = num;
			global::Pathfinding.Recast.RecastMeshGatherer.CapsuleCache capsuleCache = null;
			for (int i = 0; i < this.capsuleCache.Count; i++)
			{
				global::Pathfinding.Recast.RecastMeshGatherer.CapsuleCache capsuleCache2 = this.capsuleCache[i];
				if (capsuleCache2.rows == num && global::UnityEngine.Mathf.Approximately(capsuleCache2.height, height))
				{
					capsuleCache = capsuleCache2;
				}
			}
			global::UnityEngine.Vector3[] array;
			if (capsuleCache == null)
			{
				array = new global::UnityEngine.Vector3[num * num2 + 2];
				global::System.Collections.Generic.List<int> list = new global::System.Collections.Generic.List<int>();
				array[array.Length - 1] = global::UnityEngine.Vector3.up;
				for (int j = 0; j < num; j++)
				{
					for (int k = 0; k < num2; k++)
					{
						array[k + j * num2] = new global::UnityEngine.Vector3(global::UnityEngine.Mathf.Cos((float)k * 3.14159274f * 2f / (float)num2) * global::UnityEngine.Mathf.Sin((float)j * 3.14159274f / (float)(num - 1)), global::UnityEngine.Mathf.Cos((float)j * 3.14159274f / (float)(num - 1)) + ((j >= num / 2) ? (-height) : height), global::UnityEngine.Mathf.Sin((float)k * 3.14159274f * 2f / (float)num2) * global::UnityEngine.Mathf.Sin((float)j * 3.14159274f / (float)(num - 1)));
					}
				}
				array[array.Length - 2] = global::UnityEngine.Vector3.down;
				int l = 0;
				int num3 = num2 - 1;
				while (l < num2)
				{
					list.Add(array.Length - 1);
					list.Add(0 * num2 + num3);
					list.Add(0 * num2 + l);
					num3 = l++;
				}
				for (int m = 1; m < num; m++)
				{
					int n = 0;
					int num4 = num2 - 1;
					while (n < num2)
					{
						list.Add(m * num2 + n);
						list.Add(m * num2 + num4);
						list.Add((m - 1) * num2 + n);
						list.Add((m - 1) * num2 + num4);
						list.Add((m - 1) * num2 + n);
						list.Add(m * num2 + num4);
						num4 = n++;
					}
				}
				int num5 = 0;
				int num6 = num2 - 1;
				while (num5 < num2)
				{
					list.Add(array.Length - 2);
					list.Add((num - 1) * num2 + num6);
					list.Add((num - 1) * num2 + num5);
					num6 = num5++;
				}
				capsuleCache = new global::Pathfinding.Recast.RecastMeshGatherer.CapsuleCache();
				capsuleCache.rows = num;
				capsuleCache.height = height;
				capsuleCache.verts = array;
				capsuleCache.tris = list.ToArray();
				this.capsuleCache.Add(capsuleCache);
			}
			array = capsuleCache.verts;
			int[] tris = capsuleCache.tris;
			return new global::Pathfinding.Voxels.RasterizationMesh(array, tris, bounds, localToWorldMatrix);
		}

		private readonly int terrainSampleSize;

		private readonly global::UnityEngine.LayerMask mask;

		private readonly global::System.Collections.Generic.List<string> tagMask;

		private readonly float colliderRasterizeDetail;

		private readonly global::UnityEngine.Bounds bounds;

		private static readonly int[] BoxColliderTris = new int[]
		{
			0,
			1,
			2,
			0,
			2,
			3,
			6,
			5,
			4,
			7,
			6,
			4,
			0,
			5,
			1,
			0,
			4,
			5,
			1,
			6,
			2,
			1,
			5,
			6,
			2,
			7,
			3,
			2,
			6,
			7,
			3,
			4,
			0,
			3,
			7,
			4
		};

		private static readonly global::UnityEngine.Vector3[] BoxColliderVerts = new global::UnityEngine.Vector3[]
		{
			new global::UnityEngine.Vector3(-1f, -1f, -1f),
			new global::UnityEngine.Vector3(1f, -1f, -1f),
			new global::UnityEngine.Vector3(1f, -1f, 1f),
			new global::UnityEngine.Vector3(-1f, -1f, 1f),
			new global::UnityEngine.Vector3(-1f, 1f, -1f),
			new global::UnityEngine.Vector3(1f, 1f, -1f),
			new global::UnityEngine.Vector3(1f, 1f, 1f),
			new global::UnityEngine.Vector3(-1f, 1f, 1f)
		};

		private global::System.Collections.Generic.List<global::Pathfinding.Recast.RecastMeshGatherer.CapsuleCache> capsuleCache = new global::System.Collections.Generic.List<global::Pathfinding.Recast.RecastMeshGatherer.CapsuleCache>();

		private class CapsuleCache
		{
			public int rows;

			public float height;

			public global::UnityEngine.Vector3[] verts;

			public int[] tris;
		}
	}
}
