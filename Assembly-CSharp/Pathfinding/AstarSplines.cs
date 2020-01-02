﻿using System;
using UnityEngine;

namespace Pathfinding
{
	internal static class AstarSplines
	{
		public static global::UnityEngine.Vector3 CatmullRom(global::UnityEngine.Vector3 previous, global::UnityEngine.Vector3 start, global::UnityEngine.Vector3 end, global::UnityEngine.Vector3 next, float elapsedTime)
		{
			float num = elapsedTime * elapsedTime;
			float num2 = num * elapsedTime;
			return previous * (-0.5f * num2 + num - 0.5f * elapsedTime) + start * (1.5f * num2 + -2.5f * num + 1f) + end * (-1.5f * num2 + 2f * num + 0.5f * elapsedTime) + next * (0.5f * num2 - 0.5f * num);
		}

		[global::System.Obsolete("Use CatmullRom")]
		public static global::UnityEngine.Vector3 CatmullRomOLD(global::UnityEngine.Vector3 previous, global::UnityEngine.Vector3 start, global::UnityEngine.Vector3 end, global::UnityEngine.Vector3 next, float elapsedTime)
		{
			return global::Pathfinding.AstarSplines.CatmullRom(previous, start, end, next, elapsedTime);
		}

		public static global::UnityEngine.Vector3 CubicBezier(global::UnityEngine.Vector3 p0, global::UnityEngine.Vector3 p1, global::UnityEngine.Vector3 p2, global::UnityEngine.Vector3 p3, float t)
		{
			t = global::UnityEngine.Mathf.Clamp01(t);
			float num = 1f - t;
			return num * num * num * p0 + 3f * num * num * t * p1 + 3f * num * t * t * p2 + t * t * t * p3;
		}

		public static global::UnityEngine.Vector3 CubicBezierDerivative(global::UnityEngine.Vector3 p0, global::UnityEngine.Vector3 p1, global::UnityEngine.Vector3 p2, global::UnityEngine.Vector3 p3, float t)
		{
			t = global::UnityEngine.Mathf.Clamp01(t);
			float num = 1f - t;
			return 3f * num * num * (p1 - p0) + 6f * num * t * (p2 - p1) + 3f * t * t * (p3 - p2);
		}

		public static global::UnityEngine.Vector3 CubicBezierSecondDerivative(global::UnityEngine.Vector3 p0, global::UnityEngine.Vector3 p1, global::UnityEngine.Vector3 p2, global::UnityEngine.Vector3 p3, float t)
		{
			t = global::UnityEngine.Mathf.Clamp01(t);
			float num = 1f - t;
			return 6f * num * (p2 - 2f * p1 + p0) + 6f * t * (p3 - 2f * p2 + p1);
		}
	}
}
