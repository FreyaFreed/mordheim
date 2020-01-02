using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using UnityEngine;

public class PandoraUtils
{
	public static global::System.Text.StringBuilder StringBuilder
	{
		get
		{
			global::PandoraUtils.stringBuilder.Remove(0, global::PandoraUtils.stringBuilder.Length);
			return global::PandoraUtils.stringBuilder;
		}
	}

	public static string UnderToCamel(string underscore, bool Upper = true)
	{
		global::System.Text.StringBuilder stringBuilder = global::PandoraUtils.StringBuilder;
		for (int i = 0; i <= underscore.Length - 1; i++)
		{
			if (underscore[i] != '_')
			{
				if ((i == 0 && Upper) || (i != 0 && underscore[i - 1] == '_'))
				{
					stringBuilder.Append(char.ToUpper(underscore[i]));
				}
				else
				{
					stringBuilder.Append(char.ToLower(underscore[i]));
				}
			}
		}
		return stringBuilder.ToString();
	}

	public static string CamelToUnder(string camel)
	{
		global::System.Text.StringBuilder stringBuilder = global::PandoraUtils.StringBuilder;
		for (int i = 0; i <= camel.Length - 1; i++)
		{
			if (char.IsUpper(camel[i]))
			{
				if (i != 0)
				{
					stringBuilder.Append('_');
				}
				stringBuilder.Append(char.ToLowerInvariant(camel[i]));
			}
			else
			{
				stringBuilder.Append(camel[i]);
			}
		}
		return stringBuilder.ToString();
	}

	public static int GetCharIdxPos(char c, int idx, string str)
	{
		int num = 0;
		for (int i = 0; i < str.Length; i++)
		{
			if (str[i] == c)
			{
				num++;
				if (num == idx)
				{
					return i;
				}
			}
		}
		return -1;
	}

	public static float ManhattanDistance(global::UnityEngine.Vector3 src, global::UnityEngine.Vector3 dest)
	{
		return global::UnityEngine.Mathf.Abs(src.x - dest.x) + global::UnityEngine.Mathf.Abs(src.y - dest.y) + global::UnityEngine.Mathf.Abs(src.z - dest.z);
	}

	public static float FlatSqrDistance(global::UnityEngine.Vector3 src, global::UnityEngine.Vector3 dest)
	{
		return global::UnityEngine.Vector2.SqrMagnitude(new global::UnityEngine.Vector2(src.x, src.z) - new global::UnityEngine.Vector2(dest.x, dest.z));
	}

	public static void DrawFacingGizmoCube(global::UnityEngine.Transform transform, float height, float width, float length, float offsetX = 0f, float offsetZ = 0f, bool drawTriangle = true)
	{
		global::UnityEngine.Gizmos.DrawLine(transform.position + transform.forward * offsetZ + transform.right * offsetX + transform.forward * width + transform.right * length, transform.position + transform.forward * offsetZ + transform.right * offsetX + transform.forward * width + transform.right * length * -1f);
		global::UnityEngine.Gizmos.DrawLine(transform.position + transform.forward * offsetZ + transform.right * offsetX + transform.forward * width + transform.right * length, transform.position + transform.forward * offsetZ + transform.right * offsetX + transform.forward * width * -1f + transform.right * length);
		global::UnityEngine.Gizmos.DrawLine(transform.position + transform.forward * offsetZ + transform.right * offsetX + transform.forward * width + transform.right * length * -1f, transform.position + transform.forward * offsetZ + transform.right * offsetX + transform.forward * width * -1f + transform.right * length * -1f);
		global::UnityEngine.Gizmos.DrawLine(transform.position + transform.forward * offsetZ + transform.right * offsetX + transform.forward * width * -1f + transform.right * length * -1f, transform.position + transform.forward * offsetZ + transform.right * offsetX + transform.forward * width * -1f + transform.right * length);
		global::UnityEngine.Gizmos.DrawLine(transform.position + transform.forward * offsetZ + transform.right * offsetX + transform.forward * width + transform.right * length + transform.up * height, transform.position + transform.forward * offsetZ + transform.right * offsetX + transform.forward * width + transform.right * length * -1f + transform.up * height);
		global::UnityEngine.Gizmos.DrawLine(transform.position + transform.forward * offsetZ + transform.right * offsetX + transform.forward * width + transform.right * length + transform.up * height, transform.position + transform.forward * offsetZ + transform.right * offsetX + transform.forward * width * -1f + transform.right * length + transform.up * height);
		global::UnityEngine.Gizmos.DrawLine(transform.position + transform.forward * offsetZ + transform.right * offsetX + transform.forward * width + transform.right * length * -1f + transform.up * height, transform.position + transform.forward * offsetZ + transform.right * offsetX + transform.forward * width * -1f + transform.right * length * -1f + transform.up * height);
		global::UnityEngine.Gizmos.DrawLine(transform.position + transform.forward * offsetZ + transform.right * offsetX + transform.forward * width * -1f + transform.right * length * -1f + transform.up * height, transform.position + transform.forward * offsetZ + transform.right * offsetX + transform.forward * width * -1f + transform.right * length + transform.up * height);
		global::UnityEngine.Gizmos.DrawLine(transform.position + transform.forward * offsetZ + transform.right * offsetX + transform.forward * width + transform.right * length, transform.position + transform.forward * offsetZ + transform.right * offsetX + transform.forward * width + transform.right * length + transform.up * height);
		global::UnityEngine.Gizmos.DrawLine(transform.position + transform.forward * offsetZ + transform.right * offsetX + transform.forward * width + transform.right * length * -1f, transform.position + transform.forward * offsetZ + transform.right * offsetX + transform.forward * width + transform.right * length * -1f + transform.up * height);
		global::UnityEngine.Gizmos.DrawLine(transform.position + transform.forward * offsetZ + transform.right * offsetX + transform.forward * width * -1f + transform.right * length, transform.position + transform.forward * offsetZ + transform.right * offsetX + transform.forward * width * -1f + transform.right * length + transform.up * height);
		global::UnityEngine.Gizmos.DrawLine(transform.position + transform.forward * offsetZ + transform.right * offsetX + transform.forward * width * -1f + transform.right * length * -1f, transform.position + transform.forward * offsetZ + transform.right * offsetX + transform.forward * width * -1f + transform.right * length * -1f + transform.up * height);
		if (drawTriangle)
		{
			global::UnityEngine.Gizmos.DrawLine(transform.position + transform.forward * offsetZ + transform.right * offsetX + transform.forward * width, transform.position + transform.forward * offsetZ + transform.right * offsetX + transform.right * length * -1f);
			global::UnityEngine.Gizmos.DrawLine(transform.position + transform.forward * offsetZ + transform.right * offsetX + transform.forward * width, transform.position + transform.forward * offsetZ + transform.right * offsetX + transform.right * length);
			global::UnityEngine.Gizmos.DrawLine(transform.position + transform.forward * offsetZ + transform.right * offsetX + transform.right * length * -1f, transform.position + transform.forward * offsetZ + transform.right * offsetX + transform.right * length);
		}
	}

	public static int Round(float val)
	{
		return (int)val + ((val - (float)((int)val) < 0.5f) ? 0 : 1);
	}

	public static bool IsBetween(float val, float min, float max)
	{
		if (min > max)
		{
			return val >= max && val <= min;
		}
		return val >= min && val <= max;
	}

	public static bool IsBetween(int val, int min, int max)
	{
		if (min > max)
		{
			return val >= max && val <= min;
		}
		return val >= min && val <= max;
	}

	public static bool RectCast(global::UnityEngine.Vector3 startPos, global::UnityEngine.Vector3 dir, float dist, float height, float width, int layerMask, global::System.Collections.Generic.List<global::UnityEngine.Collider> traversableColliders, out global::UnityEngine.RaycastHit raycastHit, bool useSphere = true)
	{
		global::PandoraUtils.castData.startPos = startPos;
		global::PandoraUtils.castData.dir = dir.normalized;
		global::PandoraUtils.castData.dist = dist;
		global::PandoraUtils.castData.layerMask = layerMask;
		global::PandoraUtils.castData.traversableColliders = traversableColliders;
		global::UnityEngine.Vector3 normalized = global::UnityEngine.Vector3.Cross(global::UnityEngine.Vector3.up, new global::UnityEngine.Vector3(dir.x, 0f, dir.z)).normalized;
		raycastHit = default(global::UnityEngine.RaycastHit);
		if (global::PandoraUtils.castData.dist <= 0f)
		{
			return true;
		}
		bool flag = true;
		flag &= !global::UnityEngine.Physics.Raycast(startPos, normalized, width / 2f, layerMask);
		if (flag)
		{
			flag &= !global::UnityEngine.Physics.Raycast(startPos, normalized * -1f, width / 2f, layerMask);
		}
		if (flag && useSphere)
		{
			global::PandoraUtils.castData.radius = global::UnityEngine.Mathf.Min(width / 2f, height / 2f);
			global::PandoraUtils.castData.startPos = startPos;
			flag &= global::PandoraUtils.LaunchRectSphere(global::PandoraUtils.castData, out raycastHit);
		}
		if (flag)
		{
			global::PandoraUtils.castData.startPos = startPos;
			flag &= global::PandoraUtils.LaunchRectRay(global::PandoraUtils.castData, out raycastHit);
		}
		if (flag)
		{
			global::PandoraUtils.castData.startPos = startPos + global::UnityEngine.Vector3.up * height / 2f;
			flag &= global::PandoraUtils.LaunchRectRay(global::PandoraUtils.castData, out raycastHit);
		}
		if (flag)
		{
			global::PandoraUtils.castData.startPos = startPos + global::UnityEngine.Vector3.up * height / 2f * -1f;
			flag &= global::PandoraUtils.LaunchRectRay(global::PandoraUtils.castData, out raycastHit);
		}
		global::PandoraUtils.castData.dist -= width / 2f;
		if (global::PandoraUtils.castData.dist <= 0f)
		{
			return true;
		}
		if (flag)
		{
			global::PandoraUtils.castData.startPos = startPos + normalized * width / 2f;
			flag &= global::PandoraUtils.LaunchRectRay(global::PandoraUtils.castData, out raycastHit);
		}
		if (flag)
		{
			global::PandoraUtils.castData.startPos = startPos + normalized * (width / 2f * -1f);
			flag &= global::PandoraUtils.LaunchRectRay(global::PandoraUtils.castData, out raycastHit);
		}
		if (flag)
		{
			global::PandoraUtils.castData.startPos = startPos + global::UnityEngine.Vector3.up * height / 2f + normalized * width / 2f;
			flag &= global::PandoraUtils.LaunchRectRay(global::PandoraUtils.castData, out raycastHit);
		}
		if (flag)
		{
			global::PandoraUtils.castData.startPos = startPos + global::UnityEngine.Vector3.up * height / 2f + normalized * width / 2f * -1f;
			flag &= global::PandoraUtils.LaunchRectRay(global::PandoraUtils.castData, out raycastHit);
		}
		if (flag)
		{
			global::PandoraUtils.castData.startPos = startPos + global::UnityEngine.Vector3.up * height / 2f * -1f + normalized * width / 2f;
			flag &= global::PandoraUtils.LaunchRectRay(global::PandoraUtils.castData, out raycastHit);
		}
		if (flag)
		{
			global::PandoraUtils.castData.startPos = startPos + global::UnityEngine.Vector3.up * height / 2f * -1f + normalized * width / 2f * -1f;
			flag &= global::PandoraUtils.LaunchRectRay(global::PandoraUtils.castData, out raycastHit);
		}
		return flag;
	}

	private static bool LaunchRectRay(global::PandoraUtils.RectCastData castData, out global::UnityEngine.RaycastHit raycastHit)
	{
		global::UnityEngine.Physics.Raycast(castData.startPos, castData.dir, out raycastHit, castData.dist, castData.layerMask);
		return raycastHit.collider == null || (castData.traversableColliders != null && castData.traversableColliders.IndexOf(raycastHit.collider) != -1);
	}

	private static bool LaunchRectSphere(global::PandoraUtils.RectCastData castData, out global::UnityEngine.RaycastHit raycastHit)
	{
		global::UnityEngine.Physics.SphereCast(castData.startPos, castData.radius, castData.dir, out raycastHit, castData.dist, castData.layerMask);
		return raycastHit.collider == null || (castData.traversableColliders != null && castData.traversableColliders.IndexOf(raycastHit.collider) != -1);
	}

	public static void InsertDistinct<T>(ref global::System.Collections.Generic.List<T> list, T obj)
	{
		if (list.IndexOf(obj) == -1)
		{
			list.Add(obj);
		}
	}

	public static global::System.Collections.Generic.List<T> GetPercList<T>(ref global::System.Collections.Generic.List<T> list, float perc)
	{
		global::PandoraUtils.Shuffle<T>(list);
		perc = global::UnityEngine.Mathf.Clamp01(perc);
		int count = global::UnityEngine.Mathf.CeilToInt(perc * (float)list.Count);
		return list.GetRange(0, count);
	}

	public static void Shuffle<T>(global::System.Collections.Generic.List<T> list)
	{
		int i = list.Count;
		while (i > 1)
		{
			int index = global::PandoraSingleton<global::GameManager>.Instance.LocalTyche.Rand(0, i--);
			T value = list[index];
			list[index] = list[i];
			list[i] = value;
		}
	}

	public static global::UnityEngine.Vector2 GetPointInsideMeshEdges(global::System.Collections.Generic.List<global::Tuple<global::UnityEngine.Vector2, global::UnityEngine.Vector2>> edges, global::UnityEngine.Vector2 center, global::UnityEngine.Vector2 src, global::UnityEngine.Vector2 target)
	{
		global::UnityEngine.Vector2 vector;
		global::PandoraUtils.IsPointInsideEdges(edges, src, target, out vector);
		global::UnityEngine.Vector2 vector2;
		global::PandoraUtils.IsPointInsideEdges(edges, (center - target) * 1000f, target, out vector2);
		return (global::UnityEngine.Vector2.SqrMagnitude(target - vector) >= global::UnityEngine.Vector2.SqrMagnitude(target - vector2)) ? vector2 : vector;
	}

	public static bool IsPointInsideEdges(global::System.Collections.Generic.List<global::Tuple<global::UnityEngine.Vector2, global::UnityEngine.Vector2>> edges, global::UnityEngine.Vector2 point, global::UnityEngine.Vector2 checkDestPoint, float minEdgeDistance = -1f)
	{
		int num = 0;
		global::PandoraUtils.tempPoints.Clear();
		for (int i = 0; i < edges.Count; i++)
		{
			global::UnityEngine.Vector2 vector;
			if (global::PandoraUtils.LineIntersect(edges[i].Item1, edges[i].Item2, point, checkDestPoint, out vector))
			{
				if (minEdgeDistance >= 0f && (vector - point).sqrMagnitude < minEdgeDistance * minEdgeDistance)
				{
					global::PandoraDebug.LogDebug("Discarding edge collision due to proximity.", "ENGAGE", null);
				}
				else
				{
					bool flag = true;
					for (int j = 0; j < global::PandoraUtils.tempPoints.Count; j++)
					{
						if (global::UnityEngine.Mathf.Approximately(global::PandoraUtils.tempPoints[j].x, vector.x) && global::UnityEngine.Mathf.Approximately(global::PandoraUtils.tempPoints[j].y, vector.y))
						{
							flag = false;
							break;
						}
					}
					if (flag)
					{
						global::PandoraUtils.tempPoints.Add(vector);
						num++;
					}
				}
			}
		}
		return num % 2 == 1;
	}

	public static bool IsPointInsideEdges(global::System.Collections.Generic.List<global::Tuple<global::UnityEngine.Vector2, global::UnityEngine.Vector2>> edges, global::UnityEngine.Vector2 src, global::UnityEngine.Vector2 target, out global::UnityEngine.Vector2 point)
	{
		global::UnityEngine.Vector2 vector = global::UnityEngine.Vector2.zero;
		float num = float.MaxValue;
		int num2 = 0;
		for (int i = 0; i < edges.Count; i++)
		{
			global::UnityEngine.Vector2 vector2;
			if (global::PandoraUtils.LineIntersect(edges[i].Item1, edges[i].Item2, src, target, out vector2))
			{
				num2++;
				float num3 = global::UnityEngine.Vector2.SqrMagnitude(vector2 - target);
				if (num3 < num)
				{
					vector = vector2;
					num = num3;
				}
			}
		}
		if (num2 % 2 == 1)
		{
			point = target;
			return true;
		}
		point = vector;
		return false;
	}

	private static bool LineIntersect(global::UnityEngine.Vector2 p0, global::UnityEngine.Vector2 p1, global::UnityEngine.Vector2 p2, global::UnityEngine.Vector2 p3, out global::UnityEngine.Vector2 intersect)
	{
		global::UnityEngine.Vector2 vector = new global::UnityEngine.Vector2(p1.x - p0.x, p1.y - p0.y);
		global::UnityEngine.Vector2 vector2 = new global::UnityEngine.Vector2(p3.x - p2.x, p3.y - p2.y);
		float num = (-vector.y * (p0.x - p2.x) + vector.x * (p0.y - p2.y)) / (-vector2.x * vector.y + vector.x * vector2.y);
		float num2 = (vector2.x * (p0.y - p2.y) - vector2.y * (p0.x - p2.x)) / (-vector2.x * vector.y + vector.x * vector2.y);
		if (num >= 0f && num <= 1f && num2 >= 0f && num2 <= 1f)
		{
			intersect = new global::UnityEngine.Vector2(p0.x + num2 * vector.x, p0.y + num2 * vector.y);
			return true;
		}
		intersect = global::UnityEngine.Vector2.zero;
		return false;
	}

	public static bool SendCapsule(global::UnityEngine.Vector3 pos, global::UnityEngine.Vector3 dir, float minHeight, float maxHeight, float maxDist, float radius, out global::UnityEngine.RaycastHit rayHitData)
	{
		return global::UnityEngine.Physics.CapsuleCast(pos + global::UnityEngine.Vector3.up * minHeight, pos + global::UnityEngine.Vector3.up * maxHeight, radius, dir, out rayHitData, maxDist, global::LayerMaskManager.decisionMask);
	}

	public static bool SendCapsule(global::UnityEngine.Vector3 pos, global::UnityEngine.Vector3 dir, float minHeight, float maxHeight, float maxDist, float radius)
	{
		return global::UnityEngine.Physics.CapsuleCast(pos + global::UnityEngine.Vector3.up * minHeight, pos + global::UnityEngine.Vector3.up * maxHeight, radius, dir, maxDist, global::LayerMaskManager.decisionMask);
	}

	public static float SqrDistPointLineDist(global::UnityEngine.Vector3 v1, global::UnityEngine.Vector3 v2, global::UnityEngine.Vector3 v0, bool isSegment)
	{
		float num = global::UnityEngine.Vector3.SqrMagnitude(v1 - v2);
		if (num == 0f)
		{
			return global::UnityEngine.Vector3.SqrMagnitude(v1 - v0);
		}
		float num2 = global::UnityEngine.Vector3.Dot(v0 - v1, v2 - v1) / num;
		if (num2 < 0f)
		{
			return global::UnityEngine.Vector3.SqrMagnitude(v0 - v1);
		}
		if (num2 > 1f)
		{
			return global::UnityEngine.Vector3.SqrMagnitude(v0 - v2);
		}
		global::UnityEngine.Vector3 a = v1 + num2 * (v2 - v1);
		return global::UnityEngine.Vector3.SqrMagnitude(a - v0);
	}

	public static bool Approximately(global::UnityEngine.Vector3 v1, global::UnityEngine.Vector3 v2)
	{
		return global::UnityEngine.Mathf.Approximately(v1.x, v2.x) && global::UnityEngine.Mathf.Approximately(v1.y, v2.y) && global::UnityEngine.Mathf.Approximately(v1.z, v2.z);
	}

	public static global::UnityEngine.Color StringToColor(string colorStr)
	{
		string[] array = colorStr.Split(new char[]
		{
			','
		});
		if (array.Length == 3)
		{
			return new global::UnityEngine.Color(global::PandoraUtils.StringToColorChannel(array[0]), global::PandoraUtils.StringToColorChannel(array[1]), global::PandoraUtils.StringToColorChannel(array[2]));
		}
		if (array.Length == 4)
		{
			return new global::UnityEngine.Color(global::PandoraUtils.StringToColorChannel(array[0]), global::PandoraUtils.StringToColorChannel(array[1]), global::PandoraUtils.StringToColorChannel(array[2]), global::PandoraUtils.StringToColorChannel(array[3]));
		}
		return default(global::UnityEngine.Color);
	}

	private static float StringToColorChannel(string colorChannel)
	{
		return float.Parse(colorChannel, global::System.Globalization.NumberFormatInfo.InvariantInfo) / 255f;
	}

	public static void RemoveBySwap<T>(global::System.Collections.Generic.List<T> list, int index)
	{
		list[index] = list[list.Count - 1];
		list.RemoveAt(list.Count - 1);
	}

	private const char under = '_';

	public static global::UnityEngine.RaycastHit[] hits = new global::UnityEngine.RaycastHit[1];

	private static readonly global::System.Text.StringBuilder stringBuilder = new global::System.Text.StringBuilder(1024);

	private static global::PandoraUtils.RectCastData castData = new global::PandoraUtils.RectCastData();

	private static global::System.Collections.Generic.List<global::UnityEngine.Vector2> tempPoints = new global::System.Collections.Generic.List<global::UnityEngine.Vector2>();

	private class RectCastData
	{
		public global::UnityEngine.Vector3 startPos;

		public global::UnityEngine.Vector3 dir;

		public float dist;

		public float radius;

		public int layerMask;

		public global::System.Collections.Generic.List<global::UnityEngine.Collider> traversableColliders;
	}
}
