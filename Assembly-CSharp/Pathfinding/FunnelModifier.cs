﻿using System;
using System.Collections.Generic;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding
{
	[global::UnityEngine.HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_funnel_modifier.php")]
	[global::UnityEngine.AddComponentMenu("Pathfinding/Modifiers/Funnel")]
	[global::System.Serializable]
	public class FunnelModifier : global::Pathfinding.MonoModifier
	{
		public override int Order
		{
			get
			{
				return 10;
			}
		}

		public override void Apply(global::Pathfinding.Path p)
		{
			global::System.Collections.Generic.List<global::Pathfinding.GraphNode> path = p.path;
			global::System.Collections.Generic.List<global::UnityEngine.Vector3> vectorPath = p.vectorPath;
			if (path == null || path.Count == 0 || vectorPath == null || vectorPath.Count != path.Count)
			{
				return;
			}
			global::System.Collections.Generic.List<global::UnityEngine.Vector3> list = global::Pathfinding.Util.ListPool<global::UnityEngine.Vector3>.Claim();
			global::System.Collections.Generic.List<global::UnityEngine.Vector3> list2 = global::Pathfinding.Util.ListPool<global::UnityEngine.Vector3>.Claim(path.Count + 1);
			global::System.Collections.Generic.List<global::UnityEngine.Vector3> list3 = global::Pathfinding.Util.ListPool<global::UnityEngine.Vector3>.Claim(path.Count + 1);
			list2.Add(vectorPath[0]);
			list3.Add(vectorPath[0]);
			for (int i = 0; i < path.Count - 1; i++)
			{
				if (!path[i].GetPortal(path[i + 1], list2, list3, false))
				{
					list2.Add((global::UnityEngine.Vector3)path[i].position);
					list3.Add((global::UnityEngine.Vector3)path[i].position);
					list2.Add((global::UnityEngine.Vector3)path[i + 1].position);
					list3.Add((global::UnityEngine.Vector3)path[i + 1].position);
				}
			}
			list2.Add(vectorPath[vectorPath.Count - 1]);
			list3.Add(vectorPath[vectorPath.Count - 1]);
			if (!global::Pathfinding.FunnelModifier.RunFunnel(list2, list3, list))
			{
				list.Add(vectorPath[0]);
				list.Add(vectorPath[vectorPath.Count - 1]);
			}
			global::Pathfinding.Util.ListPool<global::UnityEngine.Vector3>.Release(p.vectorPath);
			p.vectorPath = list;
			global::Pathfinding.Util.ListPool<global::UnityEngine.Vector3>.Release(list2);
			global::Pathfinding.Util.ListPool<global::UnityEngine.Vector3>.Release(list3);
		}

		public static bool RunFunnel(global::System.Collections.Generic.List<global::UnityEngine.Vector3> left, global::System.Collections.Generic.List<global::UnityEngine.Vector3> right, global::System.Collections.Generic.List<global::UnityEngine.Vector3> funnelPath)
		{
			if (left == null)
			{
				throw new global::System.ArgumentNullException("left");
			}
			if (right == null)
			{
				throw new global::System.ArgumentNullException("right");
			}
			if (funnelPath == null)
			{
				throw new global::System.ArgumentNullException("funnelPath");
			}
			if (left.Count != right.Count)
			{
				throw new global::System.ArgumentException("left and right lists must have equal length");
			}
			if (left.Count < 3)
			{
				return false;
			}
			while (left[1] == left[2] && right[1] == right[2])
			{
				left.RemoveAt(1);
				right.RemoveAt(1);
				if (left.Count <= 3)
				{
					return false;
				}
			}
			global::UnityEngine.Vector3 vector = left[2];
			if (vector == left[1])
			{
				vector = right[2];
			}
			while (global::Pathfinding.VectorMath.IsColinearXZ(left[0], left[1], right[1]) || global::Pathfinding.VectorMath.RightOrColinearXZ(left[1], right[1], vector) == global::Pathfinding.VectorMath.RightOrColinearXZ(left[1], right[1], left[0]))
			{
				left.RemoveAt(1);
				right.RemoveAt(1);
				if (left.Count <= 3)
				{
					return false;
				}
				vector = left[2];
				if (vector == left[1])
				{
					vector = right[2];
				}
			}
			if (!global::Pathfinding.VectorMath.IsClockwiseXZ(left[0], left[1], right[1]) && !global::Pathfinding.VectorMath.IsColinearXZ(left[0], left[1], right[1]))
			{
				global::System.Collections.Generic.List<global::UnityEngine.Vector3> list = left;
				left = right;
				right = list;
			}
			funnelPath.Add(left[0]);
			global::UnityEngine.Vector3 vector2 = left[0];
			global::UnityEngine.Vector3 vector3 = left[1];
			global::UnityEngine.Vector3 vector4 = right[1];
			int num = 1;
			int num2 = 1;
			int i = 2;
			while (i < left.Count)
			{
				if (funnelPath.Count > 2000)
				{
					global::UnityEngine.Debug.LogWarning("Avoiding infinite loop. Remove this check if you have this long paths.");
					break;
				}
				global::UnityEngine.Vector3 vector5 = left[i];
				global::UnityEngine.Vector3 vector6 = right[i];
				if (global::Pathfinding.VectorMath.SignedTriangleAreaTimes2XZ(vector2, vector4, vector6) < 0f)
				{
					goto IL_279;
				}
				if (vector2 == vector4 || global::Pathfinding.VectorMath.SignedTriangleAreaTimes2XZ(vector2, vector3, vector6) <= 0f)
				{
					vector4 = vector6;
					num = i;
					goto IL_279;
				}
				funnelPath.Add(vector3);
				vector2 = vector3;
				int num3 = num2;
				vector3 = vector2;
				vector4 = vector2;
				num2 = num3;
				num = num3;
				i = num3;
				IL_2DD:
				i++;
				continue;
				IL_279:
				if (global::Pathfinding.VectorMath.SignedTriangleAreaTimes2XZ(vector2, vector3, vector5) > 0f)
				{
					goto IL_2DD;
				}
				if (vector2 == vector3 || global::Pathfinding.VectorMath.SignedTriangleAreaTimes2XZ(vector2, vector4, vector5) >= 0f)
				{
					vector3 = vector5;
					num2 = i;
					goto IL_2DD;
				}
				funnelPath.Add(vector4);
				vector2 = vector4;
				num3 = num;
				vector3 = vector2;
				vector4 = vector2;
				num2 = num3;
				num = num3;
				i = num3;
				goto IL_2DD;
			}
			funnelPath.Add(left[left.Count - 1]);
			return true;
		}
	}
}
