using System;
using System.Collections.Generic;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding
{
	public static class Polygon
	{
		[global::System.Obsolete("Use VectorMath.SignedTriangleAreaTimes2XZ instead")]
		public static long TriangleArea2(global::Pathfinding.Int3 a, global::Pathfinding.Int3 b, global::Pathfinding.Int3 c)
		{
			return global::Pathfinding.VectorMath.SignedTriangleAreaTimes2XZ(a, b, c);
		}

		[global::System.Obsolete("Use VectorMath.SignedTriangleAreaTimes2XZ instead")]
		public static float TriangleArea2(global::UnityEngine.Vector3 a, global::UnityEngine.Vector3 b, global::UnityEngine.Vector3 c)
		{
			return global::Pathfinding.VectorMath.SignedTriangleAreaTimes2XZ(a, b, c);
		}

		[global::System.Obsolete("Use TriangleArea2 instead to avoid confusion regarding the factor 2")]
		public static long TriangleArea(global::Pathfinding.Int3 a, global::Pathfinding.Int3 b, global::Pathfinding.Int3 c)
		{
			return global::Pathfinding.Polygon.TriangleArea2(a, b, c);
		}

		[global::System.Obsolete("Use TriangleArea2 instead to avoid confusion regarding the factor 2")]
		public static float TriangleArea(global::UnityEngine.Vector3 a, global::UnityEngine.Vector3 b, global::UnityEngine.Vector3 c)
		{
			return global::Pathfinding.Polygon.TriangleArea2(a, b, c);
		}

		[global::System.Obsolete("Use ContainsPointXZ instead")]
		public static bool ContainsPoint(global::UnityEngine.Vector3 a, global::UnityEngine.Vector3 b, global::UnityEngine.Vector3 c, global::UnityEngine.Vector3 p)
		{
			return global::Pathfinding.Polygon.ContainsPointXZ(a, b, c, p);
		}

		public static bool ContainsPointXZ(global::UnityEngine.Vector3 a, global::UnityEngine.Vector3 b, global::UnityEngine.Vector3 c, global::UnityEngine.Vector3 p)
		{
			return global::Pathfinding.VectorMath.IsClockwiseMarginXZ(a, b, p) && global::Pathfinding.VectorMath.IsClockwiseMarginXZ(b, c, p) && global::Pathfinding.VectorMath.IsClockwiseMarginXZ(c, a, p);
		}

		[global::System.Obsolete("Use ContainsPointXZ instead")]
		public static bool ContainsPoint(global::Pathfinding.Int3 a, global::Pathfinding.Int3 b, global::Pathfinding.Int3 c, global::Pathfinding.Int3 p)
		{
			return global::Pathfinding.Polygon.ContainsPointXZ(a, b, c, p);
		}

		public static bool ContainsPointXZ(global::Pathfinding.Int3 a, global::Pathfinding.Int3 b, global::Pathfinding.Int3 c, global::Pathfinding.Int3 p)
		{
			return global::Pathfinding.VectorMath.IsClockwiseOrColinearXZ(a, b, p) && global::Pathfinding.VectorMath.IsClockwiseOrColinearXZ(b, c, p) && global::Pathfinding.VectorMath.IsClockwiseOrColinearXZ(c, a, p);
		}

		public static bool ContainsPoint(global::Pathfinding.Int2 a, global::Pathfinding.Int2 b, global::Pathfinding.Int2 c, global::Pathfinding.Int2 p)
		{
			return global::Pathfinding.VectorMath.IsClockwiseOrColinear(a, b, p) && global::Pathfinding.VectorMath.IsClockwiseOrColinear(b, c, p) && global::Pathfinding.VectorMath.IsClockwiseOrColinear(c, a, p);
		}

		[global::System.Obsolete("Use ContainsPointXZ instead")]
		public static bool ContainsPoint(global::UnityEngine.Vector3[] polyPoints, global::UnityEngine.Vector3 p)
		{
			return global::Pathfinding.Polygon.ContainsPointXZ(polyPoints, p);
		}

		public static bool ContainsPoint(global::UnityEngine.Vector2[] polyPoints, global::UnityEngine.Vector2 p)
		{
			int num = polyPoints.Length - 1;
			bool flag = false;
			int i = 0;
			while (i < polyPoints.Length)
			{
				if (((polyPoints[i].y <= p.y && p.y < polyPoints[num].y) || (polyPoints[num].y <= p.y && p.y < polyPoints[i].y)) && p.x < (polyPoints[num].x - polyPoints[i].x) * (p.y - polyPoints[i].y) / (polyPoints[num].y - polyPoints[i].y) + polyPoints[i].x)
				{
					flag = !flag;
				}
				num = i++;
			}
			return flag;
		}

		public static bool ContainsPointXZ(global::UnityEngine.Vector3[] polyPoints, global::UnityEngine.Vector3 p)
		{
			int num = polyPoints.Length - 1;
			bool flag = false;
			int i = 0;
			while (i < polyPoints.Length)
			{
				if (((polyPoints[i].z <= p.z && p.z < polyPoints[num].z) || (polyPoints[num].z <= p.z && p.z < polyPoints[i].z)) && p.x < (polyPoints[num].x - polyPoints[i].x) * (p.z - polyPoints[i].z) / (polyPoints[num].z - polyPoints[i].z) + polyPoints[i].x)
				{
					flag = !flag;
				}
				num = i++;
			}
			return flag;
		}

		[global::System.Obsolete("Use VectorMath.RightXZ instead. Note that it now uses a left handed coordinate system (same as Unity)")]
		public static bool LeftNotColinear(global::UnityEngine.Vector3 a, global::UnityEngine.Vector3 b, global::UnityEngine.Vector3 p)
		{
			return global::Pathfinding.VectorMath.RightXZ(a, b, p);
		}

		[global::System.Obsolete("Use VectorMath.RightOrColinearXZ instead. Note that it now uses a left handed coordinate system (same as Unity)")]
		public static bool Left(global::UnityEngine.Vector3 a, global::UnityEngine.Vector3 b, global::UnityEngine.Vector3 p)
		{
			return global::Pathfinding.VectorMath.RightOrColinearXZ(a, b, p);
		}

		[global::System.Obsolete("Use VectorMath.RightOrColinear instead. Note that it now uses a left handed coordinate system (same as Unity)")]
		public static bool Left(global::UnityEngine.Vector2 a, global::UnityEngine.Vector2 b, global::UnityEngine.Vector2 p)
		{
			return global::Pathfinding.VectorMath.RightOrColinear(a, b, p);
		}

		[global::System.Obsolete("Use VectorMath.RightOrColinearXZ instead. Note that it now uses a left handed coordinate system (same as Unity)")]
		public static bool Left(global::Pathfinding.Int3 a, global::Pathfinding.Int3 b, global::Pathfinding.Int3 p)
		{
			return global::Pathfinding.VectorMath.RightOrColinearXZ(a, b, p);
		}

		[global::System.Obsolete("Use VectorMath.RightXZ instead. Note that it now uses a left handed coordinate system (same as Unity)")]
		public static bool LeftNotColinear(global::Pathfinding.Int3 a, global::Pathfinding.Int3 b, global::Pathfinding.Int3 p)
		{
			return global::Pathfinding.VectorMath.RightXZ(a, b, p);
		}

		[global::System.Obsolete("Use VectorMath.RightOrColinear instead. Note that it now uses a left handed coordinate system (same as Unity)")]
		public static bool Left(global::Pathfinding.Int2 a, global::Pathfinding.Int2 b, global::Pathfinding.Int2 p)
		{
			return global::Pathfinding.VectorMath.RightOrColinear(a, b, p);
		}

		[global::System.Obsolete("Use VectorMath.IsClockwiseMarginXZ instead")]
		public static bool IsClockwiseMargin(global::UnityEngine.Vector3 a, global::UnityEngine.Vector3 b, global::UnityEngine.Vector3 c)
		{
			return global::Pathfinding.VectorMath.IsClockwiseMarginXZ(a, b, c);
		}

		[global::System.Obsolete("Use VectorMath.IsClockwiseXZ instead")]
		public static bool IsClockwise(global::UnityEngine.Vector3 a, global::UnityEngine.Vector3 b, global::UnityEngine.Vector3 c)
		{
			return global::Pathfinding.VectorMath.IsClockwiseXZ(a, b, c);
		}

		[global::System.Obsolete("Use VectorMath.IsClockwiseXZ instead")]
		public static bool IsClockwise(global::Pathfinding.Int3 a, global::Pathfinding.Int3 b, global::Pathfinding.Int3 c)
		{
			return global::Pathfinding.VectorMath.IsClockwiseXZ(a, b, c);
		}

		[global::System.Obsolete("Use VectorMath.IsClockwiseOrColinearXZ instead")]
		public static bool IsClockwiseMargin(global::Pathfinding.Int3 a, global::Pathfinding.Int3 b, global::Pathfinding.Int3 c)
		{
			return global::Pathfinding.VectorMath.IsClockwiseOrColinearXZ(a, b, c);
		}

		[global::System.Obsolete("Use VectorMath.IsClockwiseOrColinear instead")]
		public static bool IsClockwiseMargin(global::Pathfinding.Int2 a, global::Pathfinding.Int2 b, global::Pathfinding.Int2 c)
		{
			return global::Pathfinding.VectorMath.IsClockwiseOrColinear(a, b, c);
		}

		[global::System.Obsolete("Use VectorMath.IsColinearXZ instead")]
		public static bool IsColinear(global::Pathfinding.Int3 a, global::Pathfinding.Int3 b, global::Pathfinding.Int3 c)
		{
			return global::Pathfinding.VectorMath.IsColinearXZ(a, b, c);
		}

		[global::System.Obsolete("Use VectorMath.IsColinearAlmostXZ instead")]
		public static bool IsColinearAlmost(global::Pathfinding.Int3 a, global::Pathfinding.Int3 b, global::Pathfinding.Int3 c)
		{
			return global::Pathfinding.VectorMath.IsColinearAlmostXZ(a, b, c);
		}

		[global::System.Obsolete("Use VectorMath.IsColinearXZ instead")]
		public static bool IsColinear(global::UnityEngine.Vector3 a, global::UnityEngine.Vector3 b, global::UnityEngine.Vector3 c)
		{
			return global::Pathfinding.VectorMath.IsColinearXZ(a, b, c);
		}

		[global::System.Obsolete("Marked for removal since it is not used by any part of the A* Pathfinding Project")]
		public static bool IntersectsUnclamped(global::UnityEngine.Vector3 a, global::UnityEngine.Vector3 b, global::UnityEngine.Vector3 a2, global::UnityEngine.Vector3 b2)
		{
			return global::Pathfinding.VectorMath.RightOrColinearXZ(a, b, a2) != global::Pathfinding.VectorMath.RightOrColinearXZ(a, b, b2);
		}

		[global::System.Obsolete("Use VectorMath.SegmentsIntersect instead")]
		public static bool Intersects(global::Pathfinding.Int2 start1, global::Pathfinding.Int2 end1, global::Pathfinding.Int2 start2, global::Pathfinding.Int2 end2)
		{
			return global::Pathfinding.VectorMath.SegmentsIntersect(start1, end1, start2, end2);
		}

		[global::System.Obsolete("Use VectorMath.SegmentsIntersectXZ instead")]
		public static bool Intersects(global::Pathfinding.Int3 start1, global::Pathfinding.Int3 end1, global::Pathfinding.Int3 start2, global::Pathfinding.Int3 end2)
		{
			return global::Pathfinding.VectorMath.SegmentsIntersectXZ(start1, end1, start2, end2);
		}

		[global::System.Obsolete("Use VectorMath.SegmentsIntersectXZ instead")]
		public static bool Intersects(global::UnityEngine.Vector3 start1, global::UnityEngine.Vector3 end1, global::UnityEngine.Vector3 start2, global::UnityEngine.Vector3 end2)
		{
			return global::Pathfinding.VectorMath.SegmentsIntersectXZ(start1, end1, start2, end2);
		}

		[global::System.Obsolete("Use VectorMath.LineDirIntersectionPointXZ instead")]
		public static global::UnityEngine.Vector3 IntersectionPointOptimized(global::UnityEngine.Vector3 start1, global::UnityEngine.Vector3 dir1, global::UnityEngine.Vector3 start2, global::UnityEngine.Vector3 dir2)
		{
			return global::Pathfinding.VectorMath.LineDirIntersectionPointXZ(start1, dir1, start2, dir2);
		}

		[global::System.Obsolete("Use VectorMath.LineDirIntersectionPointXZ instead")]
		public static global::UnityEngine.Vector3 IntersectionPointOptimized(global::UnityEngine.Vector3 start1, global::UnityEngine.Vector3 dir1, global::UnityEngine.Vector3 start2, global::UnityEngine.Vector3 dir2, out bool intersects)
		{
			return global::Pathfinding.VectorMath.LineDirIntersectionPointXZ(start1, dir1, start2, dir2, out intersects);
		}

		[global::System.Obsolete("Use VectorMath.RaySegmentIntersectXZ instead")]
		public static bool IntersectionFactorRaySegment(global::Pathfinding.Int3 start1, global::Pathfinding.Int3 end1, global::Pathfinding.Int3 start2, global::Pathfinding.Int3 end2)
		{
			return global::Pathfinding.VectorMath.RaySegmentIntersectXZ(start1, end1, start2, end2);
		}

		[global::System.Obsolete("Use VectorMath.LineIntersectionFactorXZ instead")]
		public static bool IntersectionFactor(global::Pathfinding.Int3 start1, global::Pathfinding.Int3 end1, global::Pathfinding.Int3 start2, global::Pathfinding.Int3 end2, out float factor1, out float factor2)
		{
			return global::Pathfinding.VectorMath.LineIntersectionFactorXZ(start1, end1, start2, end2, out factor1, out factor2);
		}

		[global::System.Obsolete("Use VectorMath.LineIntersectionFactorXZ instead")]
		public static bool IntersectionFactor(global::UnityEngine.Vector3 start1, global::UnityEngine.Vector3 end1, global::UnityEngine.Vector3 start2, global::UnityEngine.Vector3 end2, out float factor1, out float factor2)
		{
			return global::Pathfinding.VectorMath.LineIntersectionFactorXZ(start1, end1, start2, end2, out factor1, out factor2);
		}

		[global::System.Obsolete("Use VectorMath.LineRayIntersectionFactorXZ instead")]
		public static float IntersectionFactorRay(global::Pathfinding.Int3 start1, global::Pathfinding.Int3 end1, global::Pathfinding.Int3 start2, global::Pathfinding.Int3 end2)
		{
			return global::Pathfinding.VectorMath.LineRayIntersectionFactorXZ(start1, end1, start2, end2);
		}

		[global::System.Obsolete("Use VectorMath.LineIntersectionFactorXZ instead")]
		public static float IntersectionFactor(global::UnityEngine.Vector3 start1, global::UnityEngine.Vector3 end1, global::UnityEngine.Vector3 start2, global::UnityEngine.Vector3 end2)
		{
			return global::Pathfinding.VectorMath.LineIntersectionFactorXZ(start1, end1, start2, end2);
		}

		[global::System.Obsolete("Use VectorMath.LineIntersectionPointXZ instead")]
		public static global::UnityEngine.Vector3 IntersectionPoint(global::UnityEngine.Vector3 start1, global::UnityEngine.Vector3 end1, global::UnityEngine.Vector3 start2, global::UnityEngine.Vector3 end2)
		{
			return global::Pathfinding.VectorMath.LineIntersectionPointXZ(start1, end1, start2, end2);
		}

		[global::System.Obsolete("Use VectorMath.LineIntersectionPointXZ instead")]
		public static global::UnityEngine.Vector3 IntersectionPoint(global::UnityEngine.Vector3 start1, global::UnityEngine.Vector3 end1, global::UnityEngine.Vector3 start2, global::UnityEngine.Vector3 end2, out bool intersects)
		{
			return global::Pathfinding.VectorMath.LineIntersectionPointXZ(start1, end1, start2, end2, out intersects);
		}

		[global::System.Obsolete("Use VectorMath.LineIntersectionPoint instead")]
		public static global::UnityEngine.Vector2 IntersectionPoint(global::UnityEngine.Vector2 start1, global::UnityEngine.Vector2 end1, global::UnityEngine.Vector2 start2, global::UnityEngine.Vector2 end2)
		{
			return global::Pathfinding.VectorMath.LineIntersectionPoint(start1, end1, start2, end2);
		}

		[global::System.Obsolete("Use VectorMath.LineIntersectionPoint instead")]
		public static global::UnityEngine.Vector2 IntersectionPoint(global::UnityEngine.Vector2 start1, global::UnityEngine.Vector2 end1, global::UnityEngine.Vector2 start2, global::UnityEngine.Vector2 end2, out bool intersects)
		{
			return global::Pathfinding.VectorMath.LineIntersectionPoint(start1, end1, start2, end2, out intersects);
		}

		[global::System.Obsolete("Use VectorMath.SegmentIntersectionPointXZ instead")]
		public static global::UnityEngine.Vector3 SegmentIntersectionPoint(global::UnityEngine.Vector3 start1, global::UnityEngine.Vector3 end1, global::UnityEngine.Vector3 start2, global::UnityEngine.Vector3 end2, out bool intersects)
		{
			return global::Pathfinding.VectorMath.SegmentIntersectionPointXZ(start1, end1, start2, end2, out intersects);
		}

		[global::System.Obsolete("Use ConvexHullXZ instead")]
		public static global::UnityEngine.Vector3[] ConvexHull(global::UnityEngine.Vector3[] points)
		{
			return global::Pathfinding.Polygon.ConvexHullXZ(points);
		}

		public static global::UnityEngine.Vector3[] ConvexHullXZ(global::UnityEngine.Vector3[] points)
		{
			if (points.Length == 0)
			{
				return new global::UnityEngine.Vector3[0];
			}
			global::System.Collections.Generic.List<global::UnityEngine.Vector3> list = global::Pathfinding.Util.ListPool<global::UnityEngine.Vector3>.Claim();
			int num = 0;
			for (int i = 1; i < points.Length; i++)
			{
				if (points[i].x < points[num].x)
				{
					num = i;
				}
			}
			int num2 = num;
			int num3 = 0;
			for (;;)
			{
				list.Add(points[num]);
				int num4 = 0;
				for (int j = 0; j < points.Length; j++)
				{
					if (num4 == num || !global::Pathfinding.VectorMath.RightOrColinearXZ(points[num], points[num4], points[j]))
					{
						num4 = j;
					}
				}
				num = num4;
				num3++;
				if (num3 > 10000)
				{
					break;
				}
				if (num == num2)
				{
					goto IL_E3;
				}
			}
			global::UnityEngine.Debug.LogWarning("Infinite Loop in Convex Hull Calculation");
			IL_E3:
			global::UnityEngine.Vector3[] result = list.ToArray();
			global::Pathfinding.Util.ListPool<global::UnityEngine.Vector3>.Release(list);
			return result;
		}

		[global::System.Obsolete("Use VectorMath.SegmentIntersectsBounds instead")]
		public static bool LineIntersectsBounds(global::UnityEngine.Bounds bounds, global::UnityEngine.Vector3 a, global::UnityEngine.Vector3 b)
		{
			return global::Pathfinding.VectorMath.SegmentIntersectsBounds(bounds, a, b);
		}

		public static global::UnityEngine.Vector3[] Subdivide(global::UnityEngine.Vector3[] path, int subdivisions)
		{
			subdivisions = ((subdivisions >= 0) ? subdivisions : 0);
			if (subdivisions == 0)
			{
				return path;
			}
			global::UnityEngine.Vector3[] array = new global::UnityEngine.Vector3[(path.Length - 1) * (int)global::UnityEngine.Mathf.Pow(2f, (float)subdivisions) + 1];
			int num = 0;
			for (int i = 0; i < path.Length - 1; i++)
			{
				float num2 = 1f / global::UnityEngine.Mathf.Pow(2f, (float)subdivisions);
				for (float num3 = 0f; num3 < 1f; num3 += num2)
				{
					array[num] = global::UnityEngine.Vector3.Lerp(path[i], path[i + 1], global::UnityEngine.Mathf.SmoothStep(0f, 1f, num3));
					num++;
				}
			}
			array[num] = path[path.Length - 1];
			return array;
		}

		[global::System.Obsolete("Scheduled for removal since it is not used by any part of the A* Pathfinding Project")]
		public static global::UnityEngine.Vector3 ClosestPointOnTriangle(global::UnityEngine.Vector3[] triangle, global::UnityEngine.Vector3 point)
		{
			return global::Pathfinding.Polygon.ClosestPointOnTriangle(triangle[0], triangle[1], triangle[2], point);
		}

		public static global::UnityEngine.Vector2 ClosestPointOnTriangle(global::UnityEngine.Vector2 a, global::UnityEngine.Vector2 b, global::UnityEngine.Vector2 c, global::UnityEngine.Vector2 p)
		{
			global::UnityEngine.Vector2 vector = b - a;
			global::UnityEngine.Vector2 vector2 = c - a;
			global::UnityEngine.Vector2 rhs = p - a;
			float num = global::UnityEngine.Vector2.Dot(vector, rhs);
			float num2 = global::UnityEngine.Vector2.Dot(vector2, rhs);
			if (num <= 0f && num2 <= 0f)
			{
				return a;
			}
			global::UnityEngine.Vector2 rhs2 = p - b;
			float num3 = global::UnityEngine.Vector2.Dot(vector, rhs2);
			float num4 = global::UnityEngine.Vector2.Dot(vector2, rhs2);
			if (num3 >= 0f && num4 <= num3)
			{
				return b;
			}
			if (num >= 0f && num3 <= 0f)
			{
				float num5 = num * num4 - num3 * num2;
				if (num5 <= 0f)
				{
					float d = num / (num - num3);
					return a + vector * d;
				}
			}
			global::UnityEngine.Vector2 rhs3 = p - c;
			float num6 = global::UnityEngine.Vector2.Dot(vector, rhs3);
			float num7 = global::UnityEngine.Vector2.Dot(vector2, rhs3);
			if (num7 >= 0f && num6 <= num7)
			{
				return c;
			}
			if (num2 >= 0f && num7 <= 0f)
			{
				float num8 = num6 * num2 - num * num7;
				if (num8 <= 0f)
				{
					float d2 = num2 / (num2 - num7);
					return a + vector2 * d2;
				}
			}
			if (num4 - num3 >= 0f && num6 - num7 >= 0f)
			{
				float num9 = num3 * num7 - num6 * num4;
				if (num9 <= 0f)
				{
					float d3 = (num4 - num3) / (num4 - num3 + (num6 - num7));
					return b + (c - b) * d3;
				}
			}
			return p;
		}

		public static global::UnityEngine.Vector3 ClosestPointOnTriangle(global::UnityEngine.Vector3 a, global::UnityEngine.Vector3 b, global::UnityEngine.Vector3 c, global::UnityEngine.Vector3 p)
		{
			global::UnityEngine.Vector3 vector = b - a;
			global::UnityEngine.Vector3 vector2 = c - a;
			global::UnityEngine.Vector3 rhs = p - a;
			float num = global::UnityEngine.Vector3.Dot(vector, rhs);
			float num2 = global::UnityEngine.Vector3.Dot(vector2, rhs);
			if (num <= 0f && num2 <= 0f)
			{
				return a;
			}
			global::UnityEngine.Vector3 rhs2 = p - b;
			float num3 = global::UnityEngine.Vector3.Dot(vector, rhs2);
			float num4 = global::UnityEngine.Vector3.Dot(vector2, rhs2);
			if (num3 >= 0f && num4 <= num3)
			{
				return b;
			}
			float num5 = num * num4 - num3 * num2;
			if (num >= 0f && num3 <= 0f && num5 <= 0f)
			{
				float d = num / (num - num3);
				return a + vector * d;
			}
			global::UnityEngine.Vector3 rhs3 = p - c;
			float num6 = global::UnityEngine.Vector3.Dot(vector, rhs3);
			float num7 = global::UnityEngine.Vector3.Dot(vector2, rhs3);
			if (num7 >= 0f && num6 <= num7)
			{
				return c;
			}
			float num8 = num6 * num2 - num * num7;
			if (num2 >= 0f && num7 <= 0f && num8 <= 0f)
			{
				float d2 = num2 / (num2 - num7);
				return a + vector2 * d2;
			}
			float num9 = num3 * num7 - num6 * num4;
			if (num4 - num3 >= 0f && num6 - num7 >= 0f && num9 <= 0f)
			{
				float d3 = (num4 - num3) / (num4 - num3 + (num6 - num7));
				return b + (c - b) * d3;
			}
			float num10 = 1f / (num9 + num8 + num5);
			float d4 = num8 * num10;
			float d5 = num5 * num10;
			return a + vector * d4 + vector2 * d5;
		}

		[global::System.Obsolete("Use VectorMath.SqrDistanceSegmentSegment instead")]
		public static float DistanceSegmentSegment3D(global::UnityEngine.Vector3 s1, global::UnityEngine.Vector3 e1, global::UnityEngine.Vector3 s2, global::UnityEngine.Vector3 e2)
		{
			return global::Pathfinding.VectorMath.SqrDistanceSegmentSegment(s1, e1, s2, e2);
		}
	}
}
