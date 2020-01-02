using System;
using UnityEngine;

namespace Flowmap
{
	public static class Primitives
	{
		public static global::UnityEngine.Mesh PlaneMesh
		{
			get
			{
				if (!global::Flowmap.Primitives.planeMesh)
				{
					global::Flowmap.Primitives.planeMesh = new global::UnityEngine.Mesh();
					global::Flowmap.Primitives.planeMesh.name = "Plane";
					global::Flowmap.Primitives.planeMesh.vertices = new global::UnityEngine.Vector3[]
					{
						new global::UnityEngine.Vector3(-0.5f, 0f, -0.5f),
						new global::UnityEngine.Vector3(0.5f, 0f, -0.5f),
						new global::UnityEngine.Vector3(-0.5f, 0f, 0.5f),
						new global::UnityEngine.Vector3(0.5f, 0f, 0.5f)
					};
					global::Flowmap.Primitives.planeMesh.uv = new global::UnityEngine.Vector2[]
					{
						new global::UnityEngine.Vector2(0f, 0f),
						new global::UnityEngine.Vector2(1f, 0f),
						new global::UnityEngine.Vector2(0f, 1f),
						new global::UnityEngine.Vector2(1f, 1f)
					};
					global::Flowmap.Primitives.planeMesh.normals = new global::UnityEngine.Vector3[]
					{
						global::UnityEngine.Vector3.up,
						global::UnityEngine.Vector3.up,
						global::UnityEngine.Vector3.up,
						global::UnityEngine.Vector3.up
					};
					global::Flowmap.Primitives.planeMesh.triangles = new int[]
					{
						2,
						1,
						0,
						3,
						1,
						2
					};
					global::Flowmap.Primitives.planeMesh.tangents = new global::UnityEngine.Vector4[]
					{
						new global::UnityEngine.Vector4(1f, 0f, 0f, 1f),
						new global::UnityEngine.Vector4(1f, 0f, 0f, 1f),
						new global::UnityEngine.Vector4(1f, 0f, 0f, 1f),
						new global::UnityEngine.Vector4(1f, 0f, 0f, 1f)
					};
					global::Flowmap.Primitives.planeMesh.hideFlags = global::UnityEngine.HideFlags.HideAndDontSave;
				}
				return global::Flowmap.Primitives.planeMesh;
			}
		}

		private static global::UnityEngine.Mesh planeMesh;
	}
}
