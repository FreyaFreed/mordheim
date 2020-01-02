using System;
using UnityEngine;

namespace Pathfinding.Voxels
{
	[global::System.Obsolete("Use RasterizationMesh instead")]
	public class ExtraMesh : global::Pathfinding.Voxels.RasterizationMesh
	{
		public ExtraMesh(global::UnityEngine.Vector3[] vertices, int[] triangles, global::UnityEngine.Bounds bounds) : base(vertices, triangles, bounds)
		{
		}

		public ExtraMesh(global::UnityEngine.Vector3[] vertices, int[] triangles, global::UnityEngine.Bounds bounds, global::UnityEngine.Matrix4x4 matrix) : base(vertices, triangles, bounds, matrix)
		{
		}
	}
}
