using System;
using UnityEngine;

namespace Pathfinding
{
	[global::UnityEngine.HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_debug_utility.php")]
	public class DebugUtility : global::UnityEngine.MonoBehaviour
	{
		public void Awake()
		{
			global::Pathfinding.DebugUtility.active = this;
		}

		public static void DrawCubes(global::UnityEngine.Vector3[] topVerts, global::UnityEngine.Vector3[] bottomVerts, global::UnityEngine.Color[] vertexColors, float width)
		{
			if (global::Pathfinding.DebugUtility.active == null)
			{
				global::Pathfinding.DebugUtility.active = (global::UnityEngine.Object.FindObjectOfType(typeof(global::Pathfinding.DebugUtility)) as global::Pathfinding.DebugUtility);
			}
			if (global::Pathfinding.DebugUtility.active == null)
			{
				throw new global::System.NullReferenceException();
			}
			if (topVerts.Length != bottomVerts.Length || topVerts.Length != vertexColors.Length)
			{
				global::UnityEngine.Debug.LogError("Array Lengths are not the same");
				return;
			}
			if (topVerts.Length > 2708)
			{
				global::UnityEngine.Vector3[] array = new global::UnityEngine.Vector3[topVerts.Length - 2708];
				global::UnityEngine.Vector3[] array2 = new global::UnityEngine.Vector3[topVerts.Length - 2708];
				global::UnityEngine.Color[] array3 = new global::UnityEngine.Color[topVerts.Length - 2708];
				for (int i = 2708; i < topVerts.Length; i++)
				{
					array[i - 2708] = topVerts[i];
					array2[i - 2708] = bottomVerts[i];
					array3[i - 2708] = vertexColors[i];
				}
				global::UnityEngine.Vector3[] array4 = new global::UnityEngine.Vector3[2708];
				global::UnityEngine.Vector3[] array5 = new global::UnityEngine.Vector3[2708];
				global::UnityEngine.Color[] array6 = new global::UnityEngine.Color[2708];
				for (int j = 0; j < 2708; j++)
				{
					array4[j] = topVerts[j];
					array5[j] = bottomVerts[j];
					array6[j] = vertexColors[j];
				}
				global::Pathfinding.DebugUtility.DrawCubes(array, array2, array3, width);
				topVerts = array4;
				bottomVerts = array5;
				vertexColors = array6;
			}
			width /= 2f;
			global::UnityEngine.Vector3[] array7 = new global::UnityEngine.Vector3[topVerts.Length * 4 * 6];
			int[] array8 = new int[topVerts.Length * 6 * 6];
			global::UnityEngine.Color[] array9 = new global::UnityEngine.Color[topVerts.Length * 4 * 6];
			for (int k = 0; k < topVerts.Length; k++)
			{
				global::UnityEngine.Vector3 a = topVerts[k] + new global::UnityEngine.Vector3(0f, global::Pathfinding.DebugUtility.active.offset, 0f);
				global::UnityEngine.Vector3 a2 = bottomVerts[k] - new global::UnityEngine.Vector3(0f, global::Pathfinding.DebugUtility.active.offset, 0f);
				global::UnityEngine.Vector3 vector = a + new global::UnityEngine.Vector3(-width, 0f, -width);
				global::UnityEngine.Vector3 vector2 = a + new global::UnityEngine.Vector3(width, 0f, -width);
				global::UnityEngine.Vector3 vector3 = a + new global::UnityEngine.Vector3(width, 0f, width);
				global::UnityEngine.Vector3 vector4 = a + new global::UnityEngine.Vector3(-width, 0f, width);
				global::UnityEngine.Vector3 vector5 = a2 + new global::UnityEngine.Vector3(-width, 0f, -width);
				global::UnityEngine.Vector3 vector6 = a2 + new global::UnityEngine.Vector3(width, 0f, -width);
				global::UnityEngine.Vector3 vector7 = a2 + new global::UnityEngine.Vector3(width, 0f, width);
				global::UnityEngine.Vector3 vector8 = a2 + new global::UnityEngine.Vector3(-width, 0f, width);
				int num = k * 4 * 6;
				int num2 = k * 6 * 6;
				global::UnityEngine.Color color = vertexColors[k];
				for (int l = num; l < num + 24; l++)
				{
					array9[l] = color;
				}
				array7[num] = vector;
				array7[num + 1] = vector4;
				array7[num + 2] = vector3;
				array7[num + 3] = vector2;
				num += 4;
				array7[num + 3] = vector5;
				array7[num + 2] = vector8;
				array7[num + 1] = vector7;
				array7[num] = vector6;
				num += 4;
				array7[num] = vector6;
				array7[num + 1] = vector2;
				array7[num + 2] = vector3;
				array7[num + 3] = vector7;
				num += 4;
				array7[num + 3] = vector5;
				array7[num + 2] = vector;
				array7[num + 1] = vector4;
				array7[num] = vector8;
				num += 4;
				array7[num + 3] = vector7;
				array7[num + 2] = vector8;
				array7[num + 1] = vector4;
				array7[num] = vector3;
				num += 4;
				array7[num] = vector6;
				array7[num + 1] = vector5;
				array7[num + 2] = vector;
				array7[num + 3] = vector2;
				for (int m = 0; m < 6; m++)
				{
					int num3 = num + m * 4;
					int num4 = num2 + m * 6;
					array8[num4] = num3;
					array8[num4 + 1] = num3 + 1;
					array8[num4 + 2] = num3 + 2;
					array8[num4 + 3] = num3;
					array8[num4 + 4] = num3 + 2;
					array8[num4 + 5] = num3 + 3;
				}
			}
			global::UnityEngine.Mesh mesh = new global::UnityEngine.Mesh();
			mesh.vertices = array7;
			mesh.triangles = array8;
			mesh.colors = array9;
			mesh.name = "VoxelMesh";
			mesh.RecalculateNormals();
			mesh.RecalculateBounds();
			if (global::Pathfinding.DebugUtility.active.optimizeMeshes)
			{
				mesh.Optimize();
			}
			global::UnityEngine.GameObject gameObject = new global::UnityEngine.GameObject("DebugMesh");
			global::UnityEngine.MeshRenderer meshRenderer = gameObject.AddComponent(typeof(global::UnityEngine.MeshRenderer)) as global::UnityEngine.MeshRenderer;
			meshRenderer.material = global::Pathfinding.DebugUtility.active.defaultMaterial;
			(gameObject.AddComponent(typeof(global::UnityEngine.MeshFilter)) as global::UnityEngine.MeshFilter).mesh = mesh;
		}

		public static void DrawQuads(global::UnityEngine.Vector3[] verts, float width)
		{
			if (verts.Length >= 16250)
			{
				global::UnityEngine.Vector3[] array = new global::UnityEngine.Vector3[verts.Length - 16250];
				for (int i = 16250; i < verts.Length; i++)
				{
					array[i - 16250] = verts[i];
				}
				global::UnityEngine.Vector3[] array2 = new global::UnityEngine.Vector3[16250];
				for (int j = 0; j < 16250; j++)
				{
					array2[j] = verts[j];
				}
				global::Pathfinding.DebugUtility.DrawQuads(array, width);
				verts = array2;
			}
			width /= 2f;
			global::UnityEngine.Vector3[] array3 = new global::UnityEngine.Vector3[verts.Length * 4];
			int[] array4 = new int[verts.Length * 6];
			for (int k = 0; k < verts.Length; k++)
			{
				global::UnityEngine.Vector3 a = verts[k];
				int num = k * 4;
				array3[num] = a + new global::UnityEngine.Vector3(-width, 0f, -width);
				array3[num + 1] = a + new global::UnityEngine.Vector3(-width, 0f, width);
				array3[num + 2] = a + new global::UnityEngine.Vector3(width, 0f, width);
				array3[num + 3] = a + new global::UnityEngine.Vector3(width, 0f, -width);
				int num2 = k * 6;
				array4[num2] = num;
				array4[num2 + 1] = num + 1;
				array4[num2 + 2] = num + 2;
				array4[num2 + 3] = num;
				array4[num2 + 4] = num + 2;
				array4[num2 + 5] = num + 3;
			}
			global::UnityEngine.Mesh mesh = new global::UnityEngine.Mesh();
			mesh.vertices = array3;
			mesh.triangles = array4;
			mesh.RecalculateNormals();
			mesh.RecalculateBounds();
			global::UnityEngine.GameObject gameObject = new global::UnityEngine.GameObject("DebugMesh");
			global::UnityEngine.MeshRenderer meshRenderer = gameObject.AddComponent(typeof(global::UnityEngine.MeshRenderer)) as global::UnityEngine.MeshRenderer;
			meshRenderer.material = global::Pathfinding.DebugUtility.active.defaultMaterial;
			(gameObject.AddComponent(typeof(global::UnityEngine.MeshFilter)) as global::UnityEngine.MeshFilter).mesh = mesh;
		}

		public global::UnityEngine.Material defaultMaterial;

		public static global::Pathfinding.DebugUtility active;

		public float offset = 0.2f;

		public bool optimizeMeshes;
	}
}
