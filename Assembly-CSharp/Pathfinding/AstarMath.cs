using System;
using UnityEngine;

namespace Pathfinding
{
	public static class AstarMath
	{
		[global::System.Obsolete("Use VectorMath.ClosestPointOnLine instead")]
		public static global::UnityEngine.Vector3 NearestPoint(global::UnityEngine.Vector3 lineStart, global::UnityEngine.Vector3 lineEnd, global::UnityEngine.Vector3 point)
		{
			return global::Pathfinding.VectorMath.ClosestPointOnLine(lineStart, lineEnd, point);
		}

		[global::System.Obsolete("Use VectorMath.ClosestPointOnLineFactor instead")]
		public static float NearestPointFactor(global::UnityEngine.Vector3 lineStart, global::UnityEngine.Vector3 lineEnd, global::UnityEngine.Vector3 point)
		{
			return global::Pathfinding.VectorMath.ClosestPointOnLineFactor(lineStart, lineEnd, point);
		}

		[global::System.Obsolete("Use VectorMath.ClosestPointOnLineFactor instead")]
		public static float NearestPointFactor(global::Pathfinding.Int3 lineStart, global::Pathfinding.Int3 lineEnd, global::Pathfinding.Int3 point)
		{
			return global::Pathfinding.VectorMath.ClosestPointOnLineFactor(lineStart, lineEnd, point);
		}

		[global::System.Obsolete("Use VectorMath.ClosestPointOnLineFactor instead")]
		public static float NearestPointFactor(global::Pathfinding.Int2 lineStart, global::Pathfinding.Int2 lineEnd, global::Pathfinding.Int2 point)
		{
			return global::Pathfinding.VectorMath.ClosestPointOnLineFactor(lineStart, lineEnd, point);
		}

		[global::System.Obsolete("Use VectorMath.ClosestPointOnSegment instead")]
		public static global::UnityEngine.Vector3 NearestPointStrict(global::UnityEngine.Vector3 lineStart, global::UnityEngine.Vector3 lineEnd, global::UnityEngine.Vector3 point)
		{
			return global::Pathfinding.VectorMath.ClosestPointOnSegment(lineStart, lineEnd, point);
		}

		[global::System.Obsolete("Use VectorMath.ClosestPointOnSegmentXZ instead")]
		public static global::UnityEngine.Vector3 NearestPointStrictXZ(global::UnityEngine.Vector3 lineStart, global::UnityEngine.Vector3 lineEnd, global::UnityEngine.Vector3 point)
		{
			return global::Pathfinding.VectorMath.ClosestPointOnSegmentXZ(lineStart, lineEnd, point);
		}

		[global::System.Obsolete("Use VectorMath.SqrDistancePointSegmentApproximate instead")]
		public static float DistancePointSegment(int x, int z, int px, int pz, int qx, int qz)
		{
			return global::Pathfinding.VectorMath.SqrDistancePointSegmentApproximate(x, z, px, pz, qx, qz);
		}

		[global::System.Obsolete("Use VectorMath.SqrDistancePointSegmentApproximate instead")]
		public static float DistancePointSegment(global::Pathfinding.Int3 a, global::Pathfinding.Int3 b, global::Pathfinding.Int3 p)
		{
			return global::Pathfinding.VectorMath.SqrDistancePointSegmentApproximate(a, b, p);
		}

		[global::System.Obsolete("Use VectorMath.SqrDistancePointSegment instead")]
		public static float DistancePointSegmentStrict(global::UnityEngine.Vector3 a, global::UnityEngine.Vector3 b, global::UnityEngine.Vector3 p)
		{
			return global::Pathfinding.VectorMath.SqrDistancePointSegment(a, b, p);
		}

		[global::System.Obsolete("Use AstarSplines.CubicBezier instead")]
		public static global::UnityEngine.Vector3 CubicBezier(global::UnityEngine.Vector3 p0, global::UnityEngine.Vector3 p1, global::UnityEngine.Vector3 p2, global::UnityEngine.Vector3 p3, float t)
		{
			return global::Pathfinding.AstarSplines.CubicBezier(p0, p1, p2, p3, t);
		}

		public static float MapTo(float startMin, float startMax, float value)
		{
			if (startMax != startMin)
			{
				value -= startMin;
				value /= startMax - startMin;
				value = global::UnityEngine.Mathf.Clamp01(value);
			}
			else
			{
				value = 0f;
			}
			return value;
		}

		public static float MapTo(float startMin, float startMax, float targetMin, float targetMax, float value)
		{
			value -= startMin;
			value /= startMax - startMin;
			value = global::UnityEngine.Mathf.Clamp01(value);
			value *= targetMax - targetMin;
			value += targetMin;
			return value;
		}

		public static string FormatBytesBinary(int bytes)
		{
			double num = (bytes < 0) ? -1.0 : 1.0;
			bytes = ((bytes < 0) ? (-bytes) : bytes);
			if (bytes < 1024)
			{
				return (double)bytes * num + " bytes";
			}
			if (bytes < 1048576)
			{
				return ((double)bytes / 1024.0 * num).ToString("0.0") + " kb";
			}
			if (bytes < 1073741824)
			{
				return ((double)bytes / 1048576.0 * num).ToString("0.0") + " mb";
			}
			return ((double)bytes / 1073741824.0 * num).ToString("0.0") + " gb";
		}

		private static int Bit(int a, int b)
		{
			return a >> b & 1;
		}

		public static global::UnityEngine.Color IntToColor(int i, float a)
		{
			int num = global::Pathfinding.AstarMath.Bit(i, 1) + global::Pathfinding.AstarMath.Bit(i, 3) * 2 + 1;
			int num2 = global::Pathfinding.AstarMath.Bit(i, 2) + global::Pathfinding.AstarMath.Bit(i, 4) * 2 + 1;
			int num3 = global::Pathfinding.AstarMath.Bit(i, 0) + global::Pathfinding.AstarMath.Bit(i, 5) * 2 + 1;
			return new global::UnityEngine.Color((float)num * 0.25f, (float)num2 * 0.25f, (float)num3 * 0.25f, a);
		}

		public static global::UnityEngine.Color HSVToRGB(float h, float s, float v)
		{
			float num = 0f;
			float num2 = 0f;
			float num3 = 0f;
			float num4 = s * v;
			float num5 = h / 60f;
			float num6 = num4 * (1f - global::System.Math.Abs(num5 % 2f - 1f));
			if (num5 < 1f)
			{
				num = num4;
				num2 = num6;
			}
			else if (num5 < 2f)
			{
				num = num6;
				num2 = num4;
			}
			else if (num5 < 3f)
			{
				num2 = num4;
				num3 = num6;
			}
			else if (num5 < 4f)
			{
				num2 = num6;
				num3 = num4;
			}
			else if (num5 < 5f)
			{
				num = num6;
				num3 = num4;
			}
			else if (num5 < 6f)
			{
				num = num4;
				num3 = num6;
			}
			float num7 = v - num4;
			num += num7;
			num2 += num7;
			num3 += num7;
			return new global::UnityEngine.Color(num, num2, num3);
		}

		[global::System.Obsolete("Use VectorMath.SqrDistanceXZ instead")]
		public static float SqrMagnitudeXZ(global::UnityEngine.Vector3 a, global::UnityEngine.Vector3 b)
		{
			return global::Pathfinding.VectorMath.SqrDistanceXZ(a, b);
		}

		[global::System.Obsolete("Obsolete", true)]
		public static float DistancePointSegment2(int x, int z, int px, int pz, int qx, int qz)
		{
			throw new global::System.NotImplementedException("Obsolete");
		}

		[global::System.Obsolete("Obsolete", true)]
		public static float DistancePointSegment2(global::UnityEngine.Vector3 a, global::UnityEngine.Vector3 b, global::UnityEngine.Vector3 p)
		{
			throw new global::System.NotImplementedException("Obsolete");
		}

		[global::System.Obsolete("Use Int3.GetHashCode instead", true)]
		public static int ComputeVertexHash(int x, int y, int z)
		{
			throw new global::System.NotImplementedException("Obsolete");
		}

		[global::System.Obsolete("Obsolete", true)]
		public static float Hermite(float start, float end, float value)
		{
			throw new global::System.NotImplementedException("Obsolete");
		}

		[global::System.Obsolete("Obsolete", true)]
		public static float MapToRange(float targetMin, float targetMax, float value)
		{
			throw new global::System.NotImplementedException("Obsolete");
		}

		[global::System.Obsolete("Obsolete", true)]
		public static string FormatBytes(int bytes)
		{
			throw new global::System.NotImplementedException("Obsolete");
		}

		[global::System.Obsolete("Obsolete", true)]
		public static float MagnitudeXZ(global::UnityEngine.Vector3 a, global::UnityEngine.Vector3 b)
		{
			throw new global::System.NotImplementedException("Obsolete");
		}

		[global::System.Obsolete("Obsolete", true)]
		public static int Repeat(int i, int n)
		{
			throw new global::System.NotImplementedException("Obsolete");
		}

		[global::System.Obsolete("Use Mathf.Abs instead", true)]
		public static float Abs(float a)
		{
			throw new global::System.NotImplementedException("Obsolete");
		}

		[global::System.Obsolete("Use Mathf.Abs instead", true)]
		public static int Abs(int a)
		{
			throw new global::System.NotImplementedException("Obsolete");
		}

		[global::System.Obsolete("Use Mathf.Min instead", true)]
		public static float Min(float a, float b)
		{
			throw new global::System.NotImplementedException("Obsolete");
		}

		[global::System.Obsolete("Use Mathf.Min instead", true)]
		public static int Min(int a, int b)
		{
			throw new global::System.NotImplementedException("Obsolete");
		}

		[global::System.Obsolete("Use Mathf.Min instead", true)]
		public static uint Min(uint a, uint b)
		{
			throw new global::System.NotImplementedException("Obsolete");
		}

		[global::System.Obsolete("Use Mathf.Max instead", true)]
		public static float Max(float a, float b)
		{
			throw new global::System.NotImplementedException("Obsolete");
		}

		[global::System.Obsolete("Use Mathf.Max instead", true)]
		public static int Max(int a, int b)
		{
			throw new global::System.NotImplementedException("Obsolete");
		}

		[global::System.Obsolete("Use Mathf.Max instead", true)]
		public static uint Max(uint a, uint b)
		{
			throw new global::System.NotImplementedException("Obsolete");
		}

		[global::System.Obsolete("Use Mathf.Max instead", true)]
		public static ushort Max(ushort a, ushort b)
		{
			throw new global::System.NotImplementedException("Obsolete");
		}

		[global::System.Obsolete("Use Mathf.Sign instead", true)]
		public static float Sign(float a)
		{
			throw new global::System.NotImplementedException("Obsolete");
		}

		[global::System.Obsolete("Use Mathf.Sign instead", true)]
		public static int Sign(int a)
		{
			throw new global::System.NotImplementedException("Obsolete");
		}

		[global::System.Obsolete("Use Mathf.Clamp instead", true)]
		public static float Clamp(float a, float b, float c)
		{
			throw new global::System.NotImplementedException("Obsolete");
		}

		[global::System.Obsolete("Use Mathf.Clamp instead", true)]
		public static int Clamp(int a, int b, int c)
		{
			throw new global::System.NotImplementedException("Obsolete");
		}

		[global::System.Obsolete("Use Mathf.Clamp01 instead", true)]
		public static float Clamp01(float a)
		{
			throw new global::System.NotImplementedException("Obsolete");
		}

		[global::System.Obsolete("Use Mathf.Clamp01 instead", true)]
		public static int Clamp01(int a)
		{
			throw new global::System.NotImplementedException("Obsolete");
		}

		[global::System.Obsolete("Use Mathf.Lerp instead", true)]
		public static float Lerp(float a, float b, float t)
		{
			throw new global::System.NotImplementedException("Obsolete");
		}

		[global::System.Obsolete("Use Mathf.RoundToInt instead", true)]
		public static int RoundToInt(float v)
		{
			throw new global::System.NotImplementedException("Obsolete");
		}

		[global::System.Obsolete("Use Mathf.RoundToInt instead", true)]
		public static int RoundToInt(double v)
		{
			throw new global::System.NotImplementedException("Obsolete");
		}
	}
}
