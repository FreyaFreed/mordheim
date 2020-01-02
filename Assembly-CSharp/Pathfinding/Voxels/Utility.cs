using System;
using System.Collections.Generic;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding.Voxels
{
	public class Utility
	{
		public static float Min(float a, float b, float c)
		{
			a = ((a >= b) ? b : a);
			return (a >= c) ? c : a;
		}

		public static float Max(float a, float b, float c)
		{
			a = ((a <= b) ? b : a);
			return (a <= c) ? c : a;
		}

		public static int Max(int a, int b, int c, int d)
		{
			a = ((a <= b) ? b : a);
			a = ((a <= c) ? c : a);
			return (a <= d) ? d : a;
		}

		public static int Min(int a, int b, int c, int d)
		{
			a = ((a >= b) ? b : a);
			a = ((a >= c) ? c : a);
			return (a >= d) ? d : a;
		}

		public static float Max(float a, float b, float c, float d)
		{
			a = ((a <= b) ? b : a);
			a = ((a <= c) ? c : a);
			return (a <= d) ? d : a;
		}

		public static float Min(float a, float b, float c, float d)
		{
			a = ((a >= b) ? b : a);
			a = ((a >= c) ? c : a);
			return (a >= d) ? d : a;
		}

		public static void CopyVector(float[] a, int i, global::UnityEngine.Vector3 v)
		{
			a[i] = v.x;
			a[i + 1] = v.y;
			a[i + 2] = v.z;
		}

		public static void Swap(ref int a, ref int b)
		{
			int num = a;
			a = b;
			b = num;
		}

		public static global::Pathfinding.Int3[] RemoveDuplicateVertices(global::Pathfinding.Int3[] vertices, int[] triangles)
		{
			global::System.Collections.Generic.Dictionary<global::Pathfinding.Int3, int> dictionary = global::Pathfinding.Util.ObjectPoolSimple<global::System.Collections.Generic.Dictionary<global::Pathfinding.Int3, int>>.Claim();
			dictionary.Clear();
			int[] array = new int[vertices.Length];
			int num = 0;
			for (int i = 0; i < vertices.Length; i++)
			{
				if (!dictionary.ContainsKey(vertices[i]))
				{
					dictionary.Add(vertices[i], num);
					array[i] = num;
					vertices[num] = vertices[i];
					num++;
				}
				else
				{
					array[i] = dictionary[vertices[i]];
				}
			}
			dictionary.Clear();
			global::Pathfinding.Util.ObjectPoolSimple<global::System.Collections.Generic.Dictionary<global::Pathfinding.Int3, int>>.Release(ref dictionary);
			for (int j = 0; j < triangles.Length; j++)
			{
				triangles[j] = array[triangles[j]];
			}
			global::Pathfinding.Int3[] array2 = new global::Pathfinding.Int3[num];
			for (int k = 0; k < num; k++)
			{
				array2[k] = vertices[k];
			}
			return array2;
		}
	}
}
