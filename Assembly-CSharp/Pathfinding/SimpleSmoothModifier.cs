using System;
using System.Collections.Generic;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding
{
	[global::UnityEngine.RequireComponent(typeof(global::Seeker))]
	[global::UnityEngine.AddComponentMenu("Pathfinding/Modifiers/Simple Smooth")]
	[global::UnityEngine.HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_simple_smooth_modifier.php")]
	[global::System.Serializable]
	public class SimpleSmoothModifier : global::Pathfinding.MonoModifier
	{
		public override int Order
		{
			get
			{
				return 50;
			}
		}

		public override void Apply(global::Pathfinding.Path p)
		{
			if (p.vectorPath == null)
			{
				global::UnityEngine.Debug.LogWarning("Can't process NULL path (has another modifier logged an error?)");
				return;
			}
			global::System.Collections.Generic.List<global::UnityEngine.Vector3> list = null;
			switch (this.smoothType)
			{
			case global::Pathfinding.SimpleSmoothModifier.SmoothType.Simple:
				list = this.SmoothSimple(p.vectorPath);
				break;
			case global::Pathfinding.SimpleSmoothModifier.SmoothType.Bezier:
				list = this.SmoothBezier(p.vectorPath);
				break;
			case global::Pathfinding.SimpleSmoothModifier.SmoothType.OffsetSimple:
				list = this.SmoothOffsetSimple(p.vectorPath);
				break;
			case global::Pathfinding.SimpleSmoothModifier.SmoothType.CurvedNonuniform:
				list = this.CurvedNonuniform(p.vectorPath);
				break;
			}
			if (list != p.vectorPath)
			{
				global::Pathfinding.Util.ListPool<global::UnityEngine.Vector3>.Release(p.vectorPath);
				p.vectorPath = list;
			}
		}

		public global::System.Collections.Generic.List<global::UnityEngine.Vector3> CurvedNonuniform(global::System.Collections.Generic.List<global::UnityEngine.Vector3> path)
		{
			if (this.maxSegmentLength <= 0f)
			{
				global::UnityEngine.Debug.LogWarning("Max Segment Length is <= 0 which would cause DivByZero-exception or other nasty errors (avoid this)");
				return path;
			}
			int num = 0;
			for (int i = 0; i < path.Count - 1; i++)
			{
				float magnitude = (path[i] - path[i + 1]).magnitude;
				for (float num2 = 0f; num2 <= magnitude; num2 += this.maxSegmentLength)
				{
					num++;
				}
			}
			global::System.Collections.Generic.List<global::UnityEngine.Vector3> list = global::Pathfinding.Util.ListPool<global::UnityEngine.Vector3>.Claim(num);
			global::UnityEngine.Vector3 vector = (path[1] - path[0]).normalized;
			for (int j = 0; j < path.Count - 1; j++)
			{
				float magnitude2 = (path[j] - path[j + 1]).magnitude;
				global::UnityEngine.Vector3 a = vector;
				global::UnityEngine.Vector3 vector2 = (j >= path.Count - 2) ? (path[j + 1] - path[j]).normalized : ((path[j + 2] - path[j + 1]).normalized - (path[j] - path[j + 1]).normalized).normalized;
				global::UnityEngine.Vector3 tan = a * magnitude2 * this.factor;
				global::UnityEngine.Vector3 tan2 = vector2 * magnitude2 * this.factor;
				global::UnityEngine.Vector3 a2 = path[j];
				global::UnityEngine.Vector3 b = path[j + 1];
				float num3 = 1f / magnitude2;
				for (float num4 = 0f; num4 <= magnitude2; num4 += this.maxSegmentLength)
				{
					float t = num4 * num3;
					list.Add(global::Pathfinding.SimpleSmoothModifier.GetPointOnCubic(a2, b, tan, tan2, t));
				}
				vector = vector2;
			}
			list[list.Count - 1] = path[path.Count - 1];
			return list;
		}

		public static global::UnityEngine.Vector3 GetPointOnCubic(global::UnityEngine.Vector3 a, global::UnityEngine.Vector3 b, global::UnityEngine.Vector3 tan1, global::UnityEngine.Vector3 tan2, float t)
		{
			float num = t * t;
			float num2 = num * t;
			float d = 2f * num2 - 3f * num + 1f;
			float d2 = -2f * num2 + 3f * num;
			float d3 = num2 - 2f * num + t;
			float d4 = num2 - num;
			return d * a + d2 * b + d3 * tan1 + d4 * tan2;
		}

		public global::System.Collections.Generic.List<global::UnityEngine.Vector3> SmoothOffsetSimple(global::System.Collections.Generic.List<global::UnityEngine.Vector3> path)
		{
			if (path.Count <= 2 || this.iterations <= 0)
			{
				return path;
			}
			if (this.iterations > 12)
			{
				global::UnityEngine.Debug.LogWarning("A very high iteration count was passed, won't let this one through");
				return path;
			}
			int num = (path.Count - 2) * (int)global::UnityEngine.Mathf.Pow(2f, (float)this.iterations) + 2;
			global::System.Collections.Generic.List<global::UnityEngine.Vector3> list = global::Pathfinding.Util.ListPool<global::UnityEngine.Vector3>.Claim(num);
			global::System.Collections.Generic.List<global::UnityEngine.Vector3> list2 = global::Pathfinding.Util.ListPool<global::UnityEngine.Vector3>.Claim(num);
			for (int i = 0; i < num; i++)
			{
				list.Add(global::UnityEngine.Vector3.zero);
				list2.Add(global::UnityEngine.Vector3.zero);
			}
			for (int j = 0; j < path.Count; j++)
			{
				list[j] = path[j];
			}
			for (int k = 0; k < this.iterations; k++)
			{
				int num2 = (path.Count - 2) * (int)global::UnityEngine.Mathf.Pow(2f, (float)k) + 2;
				global::System.Collections.Generic.List<global::UnityEngine.Vector3> list3 = list;
				list = list2;
				list2 = list3;
				for (int l = 0; l < num2 - 1; l++)
				{
					global::UnityEngine.Vector3 vector = list2[l];
					global::UnityEngine.Vector3 vector2 = list2[l + 1];
					global::UnityEngine.Vector3 normalized = global::UnityEngine.Vector3.Cross(vector2 - vector, global::UnityEngine.Vector3.up).normalized;
					bool flag = false;
					bool flag2 = false;
					bool flag3 = false;
					bool flag4 = false;
					if (l != 0 && !global::Pathfinding.VectorMath.IsColinearXZ(vector, vector2, list2[l - 1]))
					{
						flag3 = true;
						flag = global::Pathfinding.VectorMath.RightOrColinearXZ(vector, vector2, list2[l - 1]);
					}
					if (l < num2 - 1 && !global::Pathfinding.VectorMath.IsColinearXZ(vector, vector2, list2[l + 2]))
					{
						flag4 = true;
						flag2 = global::Pathfinding.VectorMath.RightOrColinearXZ(vector, vector2, list2[l + 2]);
					}
					if (flag3)
					{
						list[l * 2] = vector + ((!flag) ? (-normalized * this.offset * 1f) : (normalized * this.offset * 1f));
					}
					else
					{
						list[l * 2] = vector;
					}
					if (flag4)
					{
						list[l * 2 + 1] = vector2 + ((!flag2) ? (-normalized * this.offset * 1f) : (normalized * this.offset * 1f));
					}
					else
					{
						list[l * 2 + 1] = vector2;
					}
				}
				list[(path.Count - 2) * (int)global::UnityEngine.Mathf.Pow(2f, (float)(k + 1)) + 2 - 1] = list2[num2 - 1];
			}
			global::Pathfinding.Util.ListPool<global::UnityEngine.Vector3>.Release(list2);
			return list;
		}

		public global::System.Collections.Generic.List<global::UnityEngine.Vector3> SmoothSimple(global::System.Collections.Generic.List<global::UnityEngine.Vector3> path)
		{
			if (path.Count < 2)
			{
				return path;
			}
			global::System.Collections.Generic.List<global::UnityEngine.Vector3> list;
			if (this.uniformLength)
			{
				this.maxSegmentLength = global::UnityEngine.Mathf.Max(this.maxSegmentLength, 0.005f);
				float num = 0f;
				for (int i = 0; i < path.Count - 1; i++)
				{
					num += global::UnityEngine.Vector3.Distance(path[i], path[i + 1]);
				}
				int num2 = global::UnityEngine.Mathf.FloorToInt(num / this.maxSegmentLength);
				list = global::Pathfinding.Util.ListPool<global::UnityEngine.Vector3>.Claim(num2 + 2);
				float num3 = 0f;
				for (int j = 0; j < path.Count - 1; j++)
				{
					global::UnityEngine.Vector3 a = path[j];
					global::UnityEngine.Vector3 b = path[j + 1];
					float num4 = global::UnityEngine.Vector3.Distance(a, b);
					while (num3 < num4)
					{
						list.Add(global::UnityEngine.Vector3.Lerp(a, b, num3 / num4));
						num3 += this.maxSegmentLength;
					}
					num3 -= num4;
				}
			}
			else
			{
				this.subdivisions = global::UnityEngine.Mathf.Max(this.subdivisions, 0);
				if (this.subdivisions > 10)
				{
					global::UnityEngine.Debug.LogWarning("Very large number of subdivisions. Cowardly refusing to subdivide every segment into more than " + (1 << this.subdivisions) + " subsegments");
					this.subdivisions = 10;
				}
				int num5 = 1 << this.subdivisions;
				list = global::Pathfinding.Util.ListPool<global::UnityEngine.Vector3>.Claim((path.Count - 1) * num5 + 1);
				for (int k = 0; k < path.Count - 1; k++)
				{
					for (int l = 0; l < num5; l++)
					{
						list.Add(global::UnityEngine.Vector3.Lerp(path[k], path[k + 1], (float)l / (float)num5));
					}
				}
			}
			list.Add(path[path.Count - 1]);
			if (this.strength > 0f)
			{
				for (int m = 0; m < this.iterations; m++)
				{
					global::UnityEngine.Vector3 a2 = list[0];
					for (int n = 1; n < list.Count - 1; n++)
					{
						global::UnityEngine.Vector3 vector = list[n];
						list[n] = global::UnityEngine.Vector3.Lerp(vector, (a2 + list[n + 1]) / 2f, this.strength);
						a2 = vector;
					}
				}
			}
			return list;
		}

		public global::System.Collections.Generic.List<global::UnityEngine.Vector3> SmoothBezier(global::System.Collections.Generic.List<global::UnityEngine.Vector3> path)
		{
			if (this.subdivisions < 0)
			{
				this.subdivisions = 0;
			}
			int num = 1 << this.subdivisions;
			global::System.Collections.Generic.List<global::UnityEngine.Vector3> list = global::Pathfinding.Util.ListPool<global::UnityEngine.Vector3>.Claim();
			for (int i = 0; i < path.Count - 1; i++)
			{
				global::UnityEngine.Vector3 vector;
				if (i == 0)
				{
					vector = path[i + 1] - path[i];
				}
				else
				{
					vector = path[i + 1] - path[i - 1];
				}
				global::UnityEngine.Vector3 vector2;
				if (i == path.Count - 2)
				{
					vector2 = path[i] - path[i + 1];
				}
				else
				{
					vector2 = path[i] - path[i + 2];
				}
				vector *= this.bezierTangentLength;
				vector2 *= this.bezierTangentLength;
				global::UnityEngine.Vector3 vector3 = path[i];
				global::UnityEngine.Vector3 p = vector3 + vector;
				global::UnityEngine.Vector3 vector4 = path[i + 1];
				global::UnityEngine.Vector3 p2 = vector4 + vector2;
				for (int j = 0; j < num; j++)
				{
					list.Add(global::Pathfinding.AstarSplines.CubicBezier(vector3, p, p2, vector4, (float)j / (float)num));
				}
			}
			list.Add(path[path.Count - 1]);
			return list;
		}

		public global::Pathfinding.SimpleSmoothModifier.SmoothType smoothType;

		[global::UnityEngine.Tooltip("The number of times to subdivide (divide in half) the path segments. [0...inf] (recommended [1...10])")]
		public int subdivisions = 2;

		[global::UnityEngine.Tooltip("Number of times to apply smoothing")]
		public int iterations = 2;

		[global::UnityEngine.Tooltip("Determines how much smoothing to apply in each smooth iteration. 0.5 usually produces the nicest looking curves")]
		public float strength = 0.5f;

		[global::UnityEngine.Tooltip("Toggle to divide all lines in equal length segments")]
		public bool uniformLength = true;

		[global::UnityEngine.Tooltip("The length of each segment in the smoothed path. A high value yields rough paths and low value yields very smooth paths, but is slower")]
		public float maxSegmentLength = 2f;

		[global::UnityEngine.Tooltip("Length factor of the bezier curves' tangents")]
		public float bezierTangentLength = 0.4f;

		[global::UnityEngine.Tooltip("Offset to apply in each smoothing iteration when using Offset Simple")]
		public float offset = 0.2f;

		[global::UnityEngine.Tooltip("How much to smooth the path. A higher value will give a smoother path, but might take the character far off the optimal path.")]
		public float factor = 0.1f;

		public enum SmoothType
		{
			Simple,
			Bezier,
			OffsetSimple,
			CurvedNonuniform
		}
	}
}
