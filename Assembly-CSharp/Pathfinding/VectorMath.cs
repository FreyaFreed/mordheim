using System;
using UnityEngine;

namespace Pathfinding
{
	public static class VectorMath
	{
		public static global::UnityEngine.Vector3 ClosestPointOnLine(global::UnityEngine.Vector3 lineStart, global::UnityEngine.Vector3 lineEnd, global::UnityEngine.Vector3 point)
		{
			global::UnityEngine.Vector3 vector = global::UnityEngine.Vector3.Normalize(lineEnd - lineStart);
			float d = global::UnityEngine.Vector3.Dot(point - lineStart, vector);
			return lineStart + d * vector;
		}

		public static float ClosestPointOnLineFactor(global::UnityEngine.Vector3 lineStart, global::UnityEngine.Vector3 lineEnd, global::UnityEngine.Vector3 point)
		{
			global::UnityEngine.Vector3 rhs = lineEnd - lineStart;
			float sqrMagnitude = rhs.sqrMagnitude;
			if ((double)sqrMagnitude <= 1E-06)
			{
				return 0f;
			}
			return global::UnityEngine.Vector3.Dot(point - lineStart, rhs) / sqrMagnitude;
		}

		public static float ClosestPointOnLineFactor(global::Pathfinding.Int3 lineStart, global::Pathfinding.Int3 lineEnd, global::Pathfinding.Int3 point)
		{
			global::Pathfinding.Int3 rhs = lineEnd - lineStart;
			float sqrMagnitude = rhs.sqrMagnitude;
			float num = (float)global::Pathfinding.Int3.Dot(point - lineStart, rhs);
			if (sqrMagnitude != 0f)
			{
				num /= sqrMagnitude;
			}
			return num;
		}

		public static float ClosestPointOnLineFactor(global::Pathfinding.Int2 lineStart, global::Pathfinding.Int2 lineEnd, global::Pathfinding.Int2 point)
		{
			global::Pathfinding.Int2 b = lineEnd - lineStart;
			double num = (double)b.sqrMagnitudeLong;
			double num2 = (double)global::Pathfinding.Int2.DotLong(point - lineStart, b);
			if (num != 0.0)
			{
				num2 /= num;
			}
			return (float)num2;
		}

		public static global::UnityEngine.Vector3 ClosestPointOnSegment(global::UnityEngine.Vector3 lineStart, global::UnityEngine.Vector3 lineEnd, global::UnityEngine.Vector3 point)
		{
			global::UnityEngine.Vector3 vector = lineEnd - lineStart;
			float sqrMagnitude = vector.sqrMagnitude;
			if ((double)sqrMagnitude <= 1E-06)
			{
				return lineStart;
			}
			float value = global::UnityEngine.Vector3.Dot(point - lineStart, vector) / sqrMagnitude;
			return lineStart + global::UnityEngine.Mathf.Clamp01(value) * vector;
		}

		public static global::UnityEngine.Vector3 ClosestPointOnSegmentXZ(global::UnityEngine.Vector3 lineStart, global::UnityEngine.Vector3 lineEnd, global::UnityEngine.Vector3 point)
		{
			lineStart.y = point.y;
			lineEnd.y = point.y;
			global::UnityEngine.Vector3 vector = lineEnd - lineStart;
			global::UnityEngine.Vector3 a = vector;
			a.y = 0f;
			float magnitude = a.magnitude;
			global::UnityEngine.Vector3 vector2 = (magnitude <= float.Epsilon) ? global::UnityEngine.Vector3.zero : (a / magnitude);
			float value = global::UnityEngine.Vector3.Dot(point - lineStart, vector2);
			return lineStart + global::UnityEngine.Mathf.Clamp(value, 0f, a.magnitude) * vector2;
		}

		public static float SqrDistancePointSegmentApproximate(int x, int z, int px, int pz, int qx, int qz)
		{
			float num = (float)(qx - px);
			float num2 = (float)(qz - pz);
			float num3 = (float)(x - px);
			float num4 = (float)(z - pz);
			float num5 = num * num + num2 * num2;
			float num6 = num * num3 + num2 * num4;
			if (num5 > 0f)
			{
				num6 /= num5;
			}
			if (num6 < 0f)
			{
				num6 = 0f;
			}
			else if (num6 > 1f)
			{
				num6 = 1f;
			}
			num3 = (float)px + num6 * num - (float)x;
			num4 = (float)pz + num6 * num2 - (float)z;
			return num3 * num3 + num4 * num4;
		}

		public static float SqrDistancePointSegmentApproximate(global::Pathfinding.Int3 a, global::Pathfinding.Int3 b, global::Pathfinding.Int3 p)
		{
			float num = (float)(b.x - a.x);
			float num2 = (float)(b.z - a.z);
			float num3 = (float)(p.x - a.x);
			float num4 = (float)(p.z - a.z);
			float num5 = num * num + num2 * num2;
			float num6 = num * num3 + num2 * num4;
			if (num5 > 0f)
			{
				num6 /= num5;
			}
			if (num6 < 0f)
			{
				num6 = 0f;
			}
			else if (num6 > 1f)
			{
				num6 = 1f;
			}
			num3 = (float)a.x + num6 * num - (float)p.x;
			num4 = (float)a.z + num6 * num2 - (float)p.z;
			return num3 * num3 + num4 * num4;
		}

		public static float SqrDistancePointSegment(global::UnityEngine.Vector3 a, global::UnityEngine.Vector3 b, global::UnityEngine.Vector3 p)
		{
			global::UnityEngine.Vector3 a2 = global::Pathfinding.VectorMath.ClosestPointOnSegment(a, b, p);
			return (a2 - p).sqrMagnitude;
		}

		public static float SqrDistanceSegmentSegment(global::UnityEngine.Vector3 s1, global::UnityEngine.Vector3 e1, global::UnityEngine.Vector3 s2, global::UnityEngine.Vector3 e2)
		{
			global::UnityEngine.Vector3 vector = e1 - s1;
			global::UnityEngine.Vector3 vector2 = e2 - s2;
			global::UnityEngine.Vector3 vector3 = s1 - s2;
			float num = global::UnityEngine.Vector3.Dot(vector, vector);
			float num2 = global::UnityEngine.Vector3.Dot(vector, vector2);
			float num3 = global::UnityEngine.Vector3.Dot(vector2, vector2);
			float num4 = global::UnityEngine.Vector3.Dot(vector, vector3);
			float num5 = global::UnityEngine.Vector3.Dot(vector2, vector3);
			float num6 = num * num3 - num2 * num2;
			float num7 = num6;
			float num8 = num6;
			float num9;
			float num10;
			if (num6 < 1E-06f)
			{
				num9 = 0f;
				num7 = 1f;
				num10 = num5;
				num8 = num3;
			}
			else
			{
				num9 = num2 * num5 - num3 * num4;
				num10 = num * num5 - num2 * num4;
				if (num9 < 0f)
				{
					num9 = 0f;
					num10 = num5;
					num8 = num3;
				}
				else if (num9 > num7)
				{
					num9 = num7;
					num10 = num5 + num2;
					num8 = num3;
				}
			}
			if (num10 < 0f)
			{
				num10 = 0f;
				if (-num4 < 0f)
				{
					num9 = 0f;
				}
				else if (-num4 > num)
				{
					num9 = num7;
				}
				else
				{
					num9 = -num4;
					num7 = num;
				}
			}
			else if (num10 > num8)
			{
				num10 = num8;
				if (-num4 + num2 < 0f)
				{
					num9 = 0f;
				}
				else if (-num4 + num2 > num)
				{
					num9 = num7;
				}
				else
				{
					num9 = -num4 + num2;
					num7 = num;
				}
			}
			float d = (global::System.Math.Abs(num9) >= 1E-06f) ? (num9 / num7) : 0f;
			float d2 = (global::System.Math.Abs(num10) >= 1E-06f) ? (num10 / num8) : 0f;
			return (vector3 + d * vector - d2 * vector2).sqrMagnitude;
		}

		public static float SqrDistanceXZ(global::UnityEngine.Vector3 a, global::UnityEngine.Vector3 b)
		{
			global::UnityEngine.Vector3 vector = a - b;
			return vector.x * vector.x + vector.z * vector.z;
		}

		public static long SignedTriangleAreaTimes2XZ(global::Pathfinding.Int3 a, global::Pathfinding.Int3 b, global::Pathfinding.Int3 c)
		{
			return (long)(b.x - a.x) * (long)(c.z - a.z) - (long)(c.x - a.x) * (long)(b.z - a.z);
		}

		public static float SignedTriangleAreaTimes2XZ(global::UnityEngine.Vector3 a, global::UnityEngine.Vector3 b, global::UnityEngine.Vector3 c)
		{
			return (b.x - a.x) * (c.z - a.z) - (c.x - a.x) * (b.z - a.z);
		}

		public static bool RightXZ(global::UnityEngine.Vector3 a, global::UnityEngine.Vector3 b, global::UnityEngine.Vector3 p)
		{
			return (b.x - a.x) * (p.z - a.z) - (p.x - a.x) * (b.z - a.z) < -1.401298E-45f;
		}

		public static bool RightXZ(global::Pathfinding.Int3 a, global::Pathfinding.Int3 b, global::Pathfinding.Int3 p)
		{
			return (long)(b.x - a.x) * (long)(p.z - a.z) - (long)(p.x - a.x) * (long)(b.z - a.z) < 0L;
		}

		public static bool RightOrColinear(global::UnityEngine.Vector2 a, global::UnityEngine.Vector2 b, global::UnityEngine.Vector2 p)
		{
			return (b.x - a.x) * (p.y - a.y) - (p.x - a.x) * (b.y - a.y) <= 0f;
		}

		public static bool RightOrColinear(global::Pathfinding.Int2 a, global::Pathfinding.Int2 b, global::Pathfinding.Int2 p)
		{
			return (long)(b.x - a.x) * (long)(p.y - a.y) - (long)(p.x - a.x) * (long)(b.y - a.y) <= 0L;
		}

		public static bool RightOrColinearXZ(global::UnityEngine.Vector3 a, global::UnityEngine.Vector3 b, global::UnityEngine.Vector3 p)
		{
			return (b.x - a.x) * (p.z - a.z) - (p.x - a.x) * (b.z - a.z) <= 0f;
		}

		public static bool RightOrColinearXZ(global::Pathfinding.Int3 a, global::Pathfinding.Int3 b, global::Pathfinding.Int3 p)
		{
			return (long)(b.x - a.x) * (long)(p.z - a.z) - (long)(p.x - a.x) * (long)(b.z - a.z) <= 0L;
		}

		public static bool IsClockwiseMarginXZ(global::UnityEngine.Vector3 a, global::UnityEngine.Vector3 b, global::UnityEngine.Vector3 c)
		{
			return (b.x - a.x) * (c.z - a.z) - (c.x - a.x) * (b.z - a.z) <= float.Epsilon;
		}

		public static bool IsClockwiseXZ(global::UnityEngine.Vector3 a, global::UnityEngine.Vector3 b, global::UnityEngine.Vector3 c)
		{
			return (b.x - a.x) * (c.z - a.z) - (c.x - a.x) * (b.z - a.z) < 0f;
		}

		public static bool IsClockwiseXZ(global::Pathfinding.Int3 a, global::Pathfinding.Int3 b, global::Pathfinding.Int3 c)
		{
			return global::Pathfinding.VectorMath.RightXZ(a, b, c);
		}

		public static bool IsClockwiseOrColinearXZ(global::Pathfinding.Int3 a, global::Pathfinding.Int3 b, global::Pathfinding.Int3 c)
		{
			return global::Pathfinding.VectorMath.RightOrColinearXZ(a, b, c);
		}

		public static bool IsClockwiseOrColinear(global::Pathfinding.Int2 a, global::Pathfinding.Int2 b, global::Pathfinding.Int2 c)
		{
			return global::Pathfinding.VectorMath.RightOrColinear(a, b, c);
		}

		public static bool IsColinearXZ(global::Pathfinding.Int3 a, global::Pathfinding.Int3 b, global::Pathfinding.Int3 c)
		{
			return (long)(b.x - a.x) * (long)(c.z - a.z) - (long)(c.x - a.x) * (long)(b.z - a.z) == 0L;
		}

		public static bool IsColinearXZ(global::UnityEngine.Vector3 a, global::UnityEngine.Vector3 b, global::UnityEngine.Vector3 c)
		{
			float num = (b.x - a.x) * (c.z - a.z) - (c.x - a.x) * (b.z - a.z);
			return num <= 1E-07f && num >= -1E-07f;
		}

		public static bool IsColinearAlmostXZ(global::Pathfinding.Int3 a, global::Pathfinding.Int3 b, global::Pathfinding.Int3 c)
		{
			long num = (long)(b.x - a.x) * (long)(c.z - a.z) - (long)(c.x - a.x) * (long)(b.z - a.z);
			return num > -1L && num < 1L;
		}

		public static bool SegmentsIntersect(global::Pathfinding.Int2 start1, global::Pathfinding.Int2 end1, global::Pathfinding.Int2 start2, global::Pathfinding.Int2 end2)
		{
			return global::Pathfinding.VectorMath.RightOrColinear(start1, end1, start2) != global::Pathfinding.VectorMath.RightOrColinear(start1, end1, end2) && global::Pathfinding.VectorMath.RightOrColinear(start2, end2, start1) != global::Pathfinding.VectorMath.RightOrColinear(start2, end2, end1);
		}

		public static bool SegmentsIntersectXZ(global::Pathfinding.Int3 start1, global::Pathfinding.Int3 end1, global::Pathfinding.Int3 start2, global::Pathfinding.Int3 end2)
		{
			return global::Pathfinding.VectorMath.RightOrColinearXZ(start1, end1, start2) != global::Pathfinding.VectorMath.RightOrColinearXZ(start1, end1, end2) && global::Pathfinding.VectorMath.RightOrColinearXZ(start2, end2, start1) != global::Pathfinding.VectorMath.RightOrColinearXZ(start2, end2, end1);
		}

		public static bool SegmentsIntersectXZ(global::UnityEngine.Vector3 start1, global::UnityEngine.Vector3 end1, global::UnityEngine.Vector3 start2, global::UnityEngine.Vector3 end2)
		{
			global::UnityEngine.Vector3 vector = end1 - start1;
			global::UnityEngine.Vector3 vector2 = end2 - start2;
			float num = vector2.z * vector.x - vector2.x * vector.z;
			if (num == 0f)
			{
				return false;
			}
			float num2 = vector2.x * (start1.z - start2.z) - vector2.z * (start1.x - start2.x);
			float num3 = vector.x * (start1.z - start2.z) - vector.z * (start1.x - start2.x);
			float num4 = num2 / num;
			float num5 = num3 / num;
			return num4 >= 0f && num4 <= 1f && num5 >= 0f && num5 <= 1f;
		}

		public static global::UnityEngine.Vector3 LineDirIntersectionPointXZ(global::UnityEngine.Vector3 start1, global::UnityEngine.Vector3 dir1, global::UnityEngine.Vector3 start2, global::UnityEngine.Vector3 dir2)
		{
			float num = dir2.z * dir1.x - dir2.x * dir1.z;
			if (num == 0f)
			{
				return start1;
			}
			float num2 = dir2.x * (start1.z - start2.z) - dir2.z * (start1.x - start2.x);
			float d = num2 / num;
			return start1 + dir1 * d;
		}

		public static global::UnityEngine.Vector3 LineDirIntersectionPointXZ(global::UnityEngine.Vector3 start1, global::UnityEngine.Vector3 dir1, global::UnityEngine.Vector3 start2, global::UnityEngine.Vector3 dir2, out bool intersects)
		{
			float num = dir2.z * dir1.x - dir2.x * dir1.z;
			if (num == 0f)
			{
				intersects = false;
				return start1;
			}
			float num2 = dir2.x * (start1.z - start2.z) - dir2.z * (start1.x - start2.x);
			float d = num2 / num;
			intersects = true;
			return start1 + dir1 * d;
		}

		public static bool RaySegmentIntersectXZ(global::Pathfinding.Int3 start1, global::Pathfinding.Int3 end1, global::Pathfinding.Int3 start2, global::Pathfinding.Int3 end2)
		{
			global::Pathfinding.Int3 @int = end1 - start1;
			global::Pathfinding.Int3 int2 = end2 - start2;
			long num = (long)(int2.z * @int.x - int2.x * @int.z);
			if (num == 0L)
			{
				return false;
			}
			long num2 = (long)(int2.x * (start1.z - start2.z) - int2.z * (start1.x - start2.x));
			long num3 = (long)(@int.x * (start1.z - start2.z) - @int.z * (start1.x - start2.x));
			return (num2 < 0L ^ num < 0L) && (num3 < 0L ^ num < 0L) && (num < 0L || num3 <= num) && (num >= 0L || num3 > num);
		}

		public static bool LineIntersectionFactorXZ(global::Pathfinding.Int3 start1, global::Pathfinding.Int3 end1, global::Pathfinding.Int3 start2, global::Pathfinding.Int3 end2, out float factor1, out float factor2)
		{
			global::Pathfinding.Int3 @int = end1 - start1;
			global::Pathfinding.Int3 int2 = end2 - start2;
			long num = (long)(int2.z * @int.x - int2.x * @int.z);
			if (num == 0L)
			{
				factor1 = 0f;
				factor2 = 0f;
				return false;
			}
			long num2 = (long)(int2.x * (start1.z - start2.z) - int2.z * (start1.x - start2.x));
			long num3 = (long)(@int.x * (start1.z - start2.z) - @int.z * (start1.x - start2.x));
			factor1 = (float)num2 / (float)num;
			factor2 = (float)num3 / (float)num;
			return true;
		}

		public static bool LineIntersectionFactorXZ(global::UnityEngine.Vector3 start1, global::UnityEngine.Vector3 end1, global::UnityEngine.Vector3 start2, global::UnityEngine.Vector3 end2, out float factor1, out float factor2)
		{
			global::UnityEngine.Vector3 vector = end1 - start1;
			global::UnityEngine.Vector3 vector2 = end2 - start2;
			float num = vector2.z * vector.x - vector2.x * vector.z;
			if (num <= 1E-05f && num >= -1E-05f)
			{
				factor1 = 0f;
				factor2 = 0f;
				return false;
			}
			float num2 = vector2.x * (start1.z - start2.z) - vector2.z * (start1.x - start2.x);
			float num3 = vector.x * (start1.z - start2.z) - vector.z * (start1.x - start2.x);
			float num4 = num2 / num;
			float num5 = num3 / num;
			factor1 = num4;
			factor2 = num5;
			return true;
		}

		public static float LineRayIntersectionFactorXZ(global::Pathfinding.Int3 start1, global::Pathfinding.Int3 end1, global::Pathfinding.Int3 start2, global::Pathfinding.Int3 end2)
		{
			global::Pathfinding.Int3 @int = end1 - start1;
			global::Pathfinding.Int3 int2 = end2 - start2;
			int num = int2.z * @int.x - int2.x * @int.z;
			if (num == 0)
			{
				return float.NaN;
			}
			int num2 = int2.x * (start1.z - start2.z) - int2.z * (start1.x - start2.x);
			int num3 = @int.x * (start1.z - start2.z) - @int.z * (start1.x - start2.x);
			if ((float)num3 / (float)num < 0f)
			{
				return float.NaN;
			}
			return (float)num2 / (float)num;
		}

		public static float LineIntersectionFactorXZ(global::UnityEngine.Vector3 start1, global::UnityEngine.Vector3 end1, global::UnityEngine.Vector3 start2, global::UnityEngine.Vector3 end2)
		{
			global::UnityEngine.Vector3 vector = end1 - start1;
			global::UnityEngine.Vector3 vector2 = end2 - start2;
			float num = vector2.z * vector.x - vector2.x * vector.z;
			if (num == 0f)
			{
				return -1f;
			}
			float num2 = vector2.x * (start1.z - start2.z) - vector2.z * (start1.x - start2.x);
			return num2 / num;
		}

		public static global::UnityEngine.Vector3 LineIntersectionPointXZ(global::UnityEngine.Vector3 start1, global::UnityEngine.Vector3 end1, global::UnityEngine.Vector3 start2, global::UnityEngine.Vector3 end2)
		{
			bool flag;
			return global::Pathfinding.VectorMath.LineIntersectionPointXZ(start1, end1, start2, end2, out flag);
		}

		public static global::UnityEngine.Vector3 LineIntersectionPointXZ(global::UnityEngine.Vector3 start1, global::UnityEngine.Vector3 end1, global::UnityEngine.Vector3 start2, global::UnityEngine.Vector3 end2, out bool intersects)
		{
			global::UnityEngine.Vector3 a = end1 - start1;
			global::UnityEngine.Vector3 vector = end2 - start2;
			float num = vector.z * a.x - vector.x * a.z;
			if (num == 0f)
			{
				intersects = false;
				return start1;
			}
			float num2 = vector.x * (start1.z - start2.z) - vector.z * (start1.x - start2.x);
			float d = num2 / num;
			intersects = true;
			return start1 + a * d;
		}

		public static global::UnityEngine.Vector2 LineIntersectionPoint(global::UnityEngine.Vector2 start1, global::UnityEngine.Vector2 end1, global::UnityEngine.Vector2 start2, global::UnityEngine.Vector2 end2)
		{
			bool flag;
			return global::Pathfinding.VectorMath.LineIntersectionPoint(start1, end1, start2, end2, out flag);
		}

		public static global::UnityEngine.Vector2 LineIntersectionPoint(global::UnityEngine.Vector2 start1, global::UnityEngine.Vector2 end1, global::UnityEngine.Vector2 start2, global::UnityEngine.Vector2 end2, out bool intersects)
		{
			global::UnityEngine.Vector2 a = end1 - start1;
			global::UnityEngine.Vector2 vector = end2 - start2;
			float num = vector.y * a.x - vector.x * a.y;
			if (num == 0f)
			{
				intersects = false;
				return start1;
			}
			float num2 = vector.x * (start1.y - start2.y) - vector.y * (start1.x - start2.x);
			float d = num2 / num;
			intersects = true;
			return start1 + a * d;
		}

		public static global::UnityEngine.Vector3 SegmentIntersectionPointXZ(global::UnityEngine.Vector3 start1, global::UnityEngine.Vector3 end1, global::UnityEngine.Vector3 start2, global::UnityEngine.Vector3 end2, out bool intersects)
		{
			global::UnityEngine.Vector3 a = end1 - start1;
			global::UnityEngine.Vector3 vector = end2 - start2;
			float num = vector.z * a.x - vector.x * a.z;
			if (num == 0f)
			{
				intersects = false;
				return start1;
			}
			float num2 = vector.x * (start1.z - start2.z) - vector.z * (start1.x - start2.x);
			float num3 = a.x * (start1.z - start2.z) - a.z * (start1.x - start2.x);
			float num4 = num2 / num;
			float num5 = num3 / num;
			if (num4 < 0f || num4 > 1f || num5 < 0f || num5 > 1f)
			{
				intersects = false;
				return start1;
			}
			intersects = true;
			return start1 + a * num4;
		}

		public static bool SegmentIntersectsBounds(global::UnityEngine.Bounds bounds, global::UnityEngine.Vector3 a, global::UnityEngine.Vector3 b)
		{
			a -= bounds.center;
			b -= bounds.center;
			global::UnityEngine.Vector3 b2 = (a + b) * 0.5f;
			global::UnityEngine.Vector3 vector = a - b2;
			global::UnityEngine.Vector3 vector2 = new global::UnityEngine.Vector3(global::System.Math.Abs(vector.x), global::System.Math.Abs(vector.y), global::System.Math.Abs(vector.z));
			global::UnityEngine.Vector3 extents = bounds.extents;
			return global::System.Math.Abs(b2.x) <= extents.x + vector2.x && global::System.Math.Abs(b2.y) <= extents.y + vector2.y && global::System.Math.Abs(b2.z) <= extents.z + vector2.z && global::System.Math.Abs(b2.y * vector.z - b2.z * vector.y) <= extents.y * vector2.z + extents.z * vector2.y && global::System.Math.Abs(b2.x * vector.z - b2.z * vector.x) <= extents.x * vector2.z + extents.z * vector2.x && global::System.Math.Abs(b2.x * vector.y - b2.y * vector.x) <= extents.x * vector2.y + extents.y * vector2.x;
		}

		public static float LineCircleIntersectionFactor(global::UnityEngine.Vector3 circleCenter, global::UnityEngine.Vector3 linePoint1, global::UnityEngine.Vector3 linePoint2, float radius)
		{
			float num;
			global::UnityEngine.Vector3 rhs = global::Pathfinding.VectorMath.Normalize(linePoint2 - linePoint1, out num);
			global::UnityEngine.Vector3 lhs = linePoint1 - circleCenter;
			float num2 = global::UnityEngine.Vector3.Dot(lhs, rhs);
			float num3 = num2 * num2 - (lhs.sqrMagnitude - radius * radius);
			if (num3 < 0f)
			{
				num3 = 0f;
			}
			float num4 = -num2 + global::UnityEngine.Mathf.Sqrt(num3);
			return (num <= 1E-05f) ? 0f : (num4 / num);
		}

		public static bool ReversesFaceOrientations(global::UnityEngine.Matrix4x4 matrix)
		{
			global::UnityEngine.Vector3 lhs = matrix.MultiplyVector(new global::UnityEngine.Vector3(1f, 0f, 0f));
			global::UnityEngine.Vector3 rhs = matrix.MultiplyVector(new global::UnityEngine.Vector3(0f, 1f, 0f));
			global::UnityEngine.Vector3 rhs2 = matrix.MultiplyVector(new global::UnityEngine.Vector3(0f, 0f, 1f));
			float num = global::UnityEngine.Vector3.Dot(global::UnityEngine.Vector3.Cross(lhs, rhs), rhs2);
			return num < 0f;
		}

		public static bool ReversesFaceOrientationsXZ(global::UnityEngine.Matrix4x4 matrix)
		{
			global::UnityEngine.Vector3 vector = matrix.MultiplyVector(new global::UnityEngine.Vector3(1f, 0f, 0f));
			global::UnityEngine.Vector3 vector2 = matrix.MultiplyVector(new global::UnityEngine.Vector3(0f, 0f, 1f));
			float num = vector.x * vector2.z - vector2.x * vector.z;
			return num < 0f;
		}

		public static global::UnityEngine.Vector3 Normalize(global::UnityEngine.Vector3 v, out float magnitude)
		{
			magnitude = v.magnitude;
			if (magnitude > 1E-05f)
			{
				return v / magnitude;
			}
			return global::UnityEngine.Vector3.zero;
		}

		public static global::UnityEngine.Vector2 Normalize(global::UnityEngine.Vector2 v, out float magnitude)
		{
			magnitude = v.magnitude;
			if (magnitude > 1E-05f)
			{
				return v / magnitude;
			}
			return global::UnityEngine.Vector2.zero;
		}

		public static global::UnityEngine.Vector3 ClampMagnitudeXZ(global::UnityEngine.Vector3 v, float maxMagnitude)
		{
			float num = v.x * v.x + v.z * v.z;
			if (num > maxMagnitude * maxMagnitude && maxMagnitude > 0f)
			{
				float num2 = maxMagnitude / global::UnityEngine.Mathf.Sqrt(num);
				v.x *= num2;
				v.z *= num2;
			}
			return v;
		}

		public static float MagnitudeXZ(global::UnityEngine.Vector3 v)
		{
			return global::UnityEngine.Mathf.Sqrt(v.x * v.x + v.z * v.z);
		}
	}
}
