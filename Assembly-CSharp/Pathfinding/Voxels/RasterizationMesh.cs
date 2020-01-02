using System;
using UnityEngine;

namespace Pathfinding.Voxels
{
	public class RasterizationMesh
	{
		public RasterizationMesh()
		{
		}

		public RasterizationMesh(global::UnityEngine.Vector3[] vertices, int[] triangles, global::UnityEngine.Bounds bounds)
		{
			this.matrix = global::UnityEngine.Matrix4x4.identity;
			this.vertices = vertices;
			this.triangles = triangles;
			this.bounds = bounds;
			this.original = null;
			this.area = 0;
		}

		public RasterizationMesh(global::UnityEngine.Vector3[] vertices, int[] triangles, global::UnityEngine.Bounds bounds, global::UnityEngine.Matrix4x4 matrix)
		{
			this.matrix = matrix;
			this.vertices = vertices;
			this.triangles = triangles;
			this.bounds = bounds;
			this.original = null;
			this.area = 0;
		}

		public void RecalculateBounds()
		{
			global::UnityEngine.Bounds bounds = new global::UnityEngine.Bounds(this.matrix.MultiplyPoint3x4(this.vertices[0]), global::UnityEngine.Vector3.zero);
			for (int i = 1; i < this.vertices.Length; i++)
			{
				bounds.Encapsulate(this.matrix.MultiplyPoint3x4(this.vertices[i]));
			}
			this.bounds = bounds;
		}

		public global::UnityEngine.MeshFilter original;

		public int area;

		public global::UnityEngine.Vector3[] vertices;

		public int[] triangles;

		public global::UnityEngine.Bounds bounds;

		public global::UnityEngine.Matrix4x4 matrix;
	}
}
