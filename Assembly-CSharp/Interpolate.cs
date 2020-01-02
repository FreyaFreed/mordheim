using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interpolate
{
	private static global::UnityEngine.Vector3 Identity(global::UnityEngine.Vector3 v)
	{
		return v;
	}

	private static global::UnityEngine.Vector3 TransformDotPosition(global::UnityEngine.Transform t)
	{
		return t.position;
	}

	private static global::System.Collections.Generic.IEnumerable<float> NewTimer(float duration)
	{
		float elapsedTime = 0f;
		while (elapsedTime < duration)
		{
			yield return elapsedTime;
			elapsedTime += global::UnityEngine.Time.deltaTime;
			if (elapsedTime >= duration)
			{
				yield return elapsedTime;
			}
		}
		yield break;
	}

	private static global::System.Collections.Generic.IEnumerable<float> NewCounter(int start, int end, int step)
	{
		for (int i = start; i <= end; i += step)
		{
			yield return (float)i;
		}
		yield break;
	}

	public static global::System.Collections.IEnumerator NewEase(global::Interpolate.Function ease, global::UnityEngine.Vector3 start, global::UnityEngine.Vector3 end, float duration)
	{
		global::System.Collections.Generic.IEnumerable<float> driver = global::Interpolate.NewTimer(duration);
		return global::Interpolate.NewEase(ease, start, end, duration, driver);
	}

	public static global::System.Collections.IEnumerator NewEase(global::Interpolate.Function ease, global::UnityEngine.Vector3 start, global::UnityEngine.Vector3 end, int slices)
	{
		global::System.Collections.Generic.IEnumerable<float> driver = global::Interpolate.NewCounter(0, slices + 1, 1);
		return global::Interpolate.NewEase(ease, start, end, (float)(slices + 1), driver);
	}

	private static global::System.Collections.IEnumerator NewEase(global::Interpolate.Function ease, global::UnityEngine.Vector3 start, global::UnityEngine.Vector3 end, float total, global::System.Collections.Generic.IEnumerable<float> driver)
	{
		global::UnityEngine.Vector3 distance = end - start;
		foreach (float num in driver)
		{
			float i = num;
			yield return global::Interpolate.Ease(ease, start, distance, i, total);
		}
		yield break;
	}

	private static global::UnityEngine.Vector3 Ease(global::Interpolate.Function ease, global::UnityEngine.Vector3 start, global::UnityEngine.Vector3 distance, float elapsedTime, float duration)
	{
		start.x = ease(start.x, distance.x, elapsedTime, duration);
		start.y = ease(start.y, distance.y, elapsedTime, duration);
		start.z = ease(start.z, distance.z, elapsedTime, duration);
		return start;
	}

	public static global::Interpolate.Function Ease(global::Interpolate.EaseType type)
	{
		global::Interpolate.Function result = null;
		switch (type)
		{
		case global::Interpolate.EaseType.Linear:
			result = new global::Interpolate.Function(global::Interpolate.Linear);
			break;
		case global::Interpolate.EaseType.EaseInQuad:
			result = new global::Interpolate.Function(global::Interpolate.EaseInQuad);
			break;
		case global::Interpolate.EaseType.EaseOutQuad:
			result = new global::Interpolate.Function(global::Interpolate.EaseOutQuad);
			break;
		case global::Interpolate.EaseType.EaseInOutQuad:
			result = new global::Interpolate.Function(global::Interpolate.EaseInOutQuad);
			break;
		case global::Interpolate.EaseType.EaseInCubic:
			result = new global::Interpolate.Function(global::Interpolate.EaseInCubic);
			break;
		case global::Interpolate.EaseType.EaseOutCubic:
			result = new global::Interpolate.Function(global::Interpolate.EaseOutCubic);
			break;
		case global::Interpolate.EaseType.EaseInOutCubic:
			result = new global::Interpolate.Function(global::Interpolate.EaseInOutCubic);
			break;
		case global::Interpolate.EaseType.EaseInQuart:
			result = new global::Interpolate.Function(global::Interpolate.EaseInQuart);
			break;
		case global::Interpolate.EaseType.EaseOutQuart:
			result = new global::Interpolate.Function(global::Interpolate.EaseOutQuart);
			break;
		case global::Interpolate.EaseType.EaseInOutQuart:
			result = new global::Interpolate.Function(global::Interpolate.EaseInOutQuart);
			break;
		case global::Interpolate.EaseType.EaseInQuint:
			result = new global::Interpolate.Function(global::Interpolate.EaseInQuint);
			break;
		case global::Interpolate.EaseType.EaseOutQuint:
			result = new global::Interpolate.Function(global::Interpolate.EaseOutQuint);
			break;
		case global::Interpolate.EaseType.EaseInOutQuint:
			result = new global::Interpolate.Function(global::Interpolate.EaseInOutQuint);
			break;
		case global::Interpolate.EaseType.EaseInSine:
			result = new global::Interpolate.Function(global::Interpolate.EaseInSine);
			break;
		case global::Interpolate.EaseType.EaseOutSine:
			result = new global::Interpolate.Function(global::Interpolate.EaseOutSine);
			break;
		case global::Interpolate.EaseType.EaseInOutSine:
			result = new global::Interpolate.Function(global::Interpolate.EaseInOutSine);
			break;
		case global::Interpolate.EaseType.EaseInExpo:
			result = new global::Interpolate.Function(global::Interpolate.EaseInExpo);
			break;
		case global::Interpolate.EaseType.EaseOutExpo:
			result = new global::Interpolate.Function(global::Interpolate.EaseOutExpo);
			break;
		case global::Interpolate.EaseType.EaseInOutExpo:
			result = new global::Interpolate.Function(global::Interpolate.EaseInOutExpo);
			break;
		case global::Interpolate.EaseType.EaseInCirc:
			result = new global::Interpolate.Function(global::Interpolate.EaseInCirc);
			break;
		case global::Interpolate.EaseType.EaseOutCirc:
			result = new global::Interpolate.Function(global::Interpolate.EaseOutCirc);
			break;
		case global::Interpolate.EaseType.EaseInOutCirc:
			result = new global::Interpolate.Function(global::Interpolate.EaseInOutCirc);
			break;
		}
		return result;
	}

	public static global::System.Collections.Generic.IEnumerable<global::UnityEngine.Vector3> NewBezier(global::Interpolate.Function ease, global::UnityEngine.Transform[] nodes, float duration)
	{
		global::System.Collections.Generic.IEnumerable<float> steps = global::Interpolate.NewTimer(duration);
		return global::Interpolate.NewBezier<global::UnityEngine.Transform>(ease, nodes, new global::Interpolate.ToVector3<global::UnityEngine.Transform>(global::Interpolate.TransformDotPosition), duration, steps);
	}

	public static global::System.Collections.Generic.IEnumerable<global::UnityEngine.Vector3> NewBezier(global::Interpolate.Function ease, global::UnityEngine.Transform[] nodes, int slices)
	{
		global::System.Collections.Generic.IEnumerable<float> steps = global::Interpolate.NewCounter(0, slices + 1, 1);
		return global::Interpolate.NewBezier<global::UnityEngine.Transform>(ease, nodes, new global::Interpolate.ToVector3<global::UnityEngine.Transform>(global::Interpolate.TransformDotPosition), (float)(slices + 1), steps);
	}

	public static global::System.Collections.Generic.IEnumerable<global::UnityEngine.Vector3> NewBezier(global::Interpolate.Function ease, global::UnityEngine.Vector3[] points, float duration)
	{
		global::System.Collections.Generic.IEnumerable<float> steps = global::Interpolate.NewTimer(duration);
		return global::Interpolate.NewBezier<global::UnityEngine.Vector3>(ease, points, new global::Interpolate.ToVector3<global::UnityEngine.Vector3>(global::Interpolate.Identity), duration, steps);
	}

	public static global::System.Collections.Generic.IEnumerable<global::UnityEngine.Vector3> NewBezier(global::Interpolate.Function ease, global::UnityEngine.Vector3[] points, int slices)
	{
		global::System.Collections.Generic.IEnumerable<float> steps = global::Interpolate.NewCounter(0, slices + 1, 1);
		return global::Interpolate.NewBezier<global::UnityEngine.Vector3>(ease, points, new global::Interpolate.ToVector3<global::UnityEngine.Vector3>(global::Interpolate.Identity), (float)(slices + 1), steps);
	}

	private static global::System.Collections.Generic.IEnumerable<global::UnityEngine.Vector3> NewBezier<T>(global::Interpolate.Function ease, global::System.Collections.IList nodes, global::Interpolate.ToVector3<T> toVector3, float maxStep, global::System.Collections.Generic.IEnumerable<float> steps)
	{
		if (nodes.Count >= 2)
		{
			global::UnityEngine.Vector3[] points = new global::UnityEngine.Vector3[nodes.Count];
			foreach (float num in steps)
			{
				float step = num;
				for (int i = 0; i < nodes.Count; i++)
				{
					points[i] = toVector3((T)((object)nodes[i]));
				}
				yield return global::Interpolate.Bezier(ease, points, step, maxStep);
			}
		}
		yield break;
	}

	private static global::UnityEngine.Vector3 Bezier(global::Interpolate.Function ease, global::UnityEngine.Vector3[] points, float elapsedTime, float duration)
	{
		for (int i = points.Length - 1; i > 0; i--)
		{
			for (int j = 0; j < i; j++)
			{
				points[j].x = ease(points[j].x, points[j + 1].x - points[j].x, elapsedTime, duration);
				points[j].y = ease(points[j].y, points[j + 1].y - points[j].y, elapsedTime, duration);
				points[j].z = ease(points[j].z, points[j + 1].z - points[j].z, elapsedTime, duration);
			}
		}
		return points[0];
	}

	public static global::System.Collections.Generic.IEnumerable<global::UnityEngine.Vector3> NewCatmullRom(global::UnityEngine.Transform[] nodes, int slices, bool loop)
	{
		return global::Interpolate.NewCatmullRom<global::UnityEngine.Transform>(nodes, new global::Interpolate.ToVector3<global::UnityEngine.Transform>(global::Interpolate.TransformDotPosition), slices, loop);
	}

	public static global::System.Collections.Generic.IEnumerable<global::UnityEngine.Vector3> NewCatmullRom(global::UnityEngine.Vector3[] points, int slices, bool loop)
	{
		return global::Interpolate.NewCatmullRom<global::UnityEngine.Vector3>(points, new global::Interpolate.ToVector3<global::UnityEngine.Vector3>(global::Interpolate.Identity), slices, loop);
	}

	private static global::System.Collections.Generic.IEnumerable<global::UnityEngine.Vector3> NewCatmullRom<T>(global::System.Collections.IList nodes, global::Interpolate.ToVector3<T> toVector3, int slices, bool loop)
	{
		if (nodes.Count >= 2)
		{
			yield return toVector3((T)((object)nodes[0]));
			int last = nodes.Count - 1;
			int current = 0;
			while (loop || current < last)
			{
				if (loop && current > last)
				{
					current = 0;
				}
				int previous = (current != 0) ? (current - 1) : ((!loop) ? current : last);
				int start = current;
				int end = (current != last) ? (current + 1) : ((!loop) ? current : 0);
				int next = (end != last) ? (end + 1) : ((!loop) ? end : 0);
				int stepCount = slices + 1;
				for (int step = 1; step <= stepCount; step++)
				{
					yield return global::Interpolate.CatmullRom(toVector3((T)((object)nodes[previous])), toVector3((T)((object)nodes[start])), toVector3((T)((object)nodes[end])), toVector3((T)((object)nodes[next])), (float)step, (float)stepCount);
				}
				current++;
			}
		}
		yield break;
	}

	private static global::UnityEngine.Vector3 CatmullRom(global::UnityEngine.Vector3 previous, global::UnityEngine.Vector3 start, global::UnityEngine.Vector3 end, global::UnityEngine.Vector3 next, float elapsedTime, float duration)
	{
		float num = elapsedTime / duration;
		float num2 = num * num;
		float num3 = num2 * num;
		return previous * (-0.5f * num3 + num2 - 0.5f * num) + start * (1.5f * num3 + -2.5f * num2 + 1f) + end * (-1.5f * num3 + 2f * num2 + 0.5f * num) + next * (0.5f * num3 - 0.5f * num2);
	}

	private static float Linear(float start, float distance, float elapsedTime, float duration)
	{
		if (elapsedTime > duration)
		{
			elapsedTime = duration;
		}
		return distance * (elapsedTime / duration) + start;
	}

	private static float EaseInQuad(float start, float distance, float elapsedTime, float duration)
	{
		elapsedTime = ((elapsedTime <= duration) ? (elapsedTime / duration) : 1f);
		return distance * elapsedTime * elapsedTime + start;
	}

	private static float EaseOutQuad(float start, float distance, float elapsedTime, float duration)
	{
		elapsedTime = ((elapsedTime <= duration) ? (elapsedTime / duration) : 1f);
		return -distance * elapsedTime * (elapsedTime - 2f) + start;
	}

	private static float EaseInOutQuad(float start, float distance, float elapsedTime, float duration)
	{
		elapsedTime = ((elapsedTime <= duration) ? (elapsedTime / (duration / 2f)) : 2f);
		if (elapsedTime < 1f)
		{
			return distance / 2f * elapsedTime * elapsedTime + start;
		}
		elapsedTime -= 1f;
		return -distance / 2f * (elapsedTime * (elapsedTime - 2f) - 1f) + start;
	}

	private static float EaseInCubic(float start, float distance, float elapsedTime, float duration)
	{
		elapsedTime = ((elapsedTime <= duration) ? (elapsedTime / duration) : 1f);
		return distance * elapsedTime * elapsedTime * elapsedTime + start;
	}

	private static float EaseOutCubic(float start, float distance, float elapsedTime, float duration)
	{
		elapsedTime = ((elapsedTime <= duration) ? (elapsedTime / duration) : 1f);
		elapsedTime -= 1f;
		return distance * (elapsedTime * elapsedTime * elapsedTime + 1f) + start;
	}

	private static float EaseInOutCubic(float start, float distance, float elapsedTime, float duration)
	{
		elapsedTime = ((elapsedTime <= duration) ? (elapsedTime / (duration / 2f)) : 2f);
		if (elapsedTime < 1f)
		{
			return distance / 2f * elapsedTime * elapsedTime * elapsedTime + start;
		}
		elapsedTime -= 2f;
		return distance / 2f * (elapsedTime * elapsedTime * elapsedTime + 2f) + start;
	}

	private static float EaseInQuart(float start, float distance, float elapsedTime, float duration)
	{
		elapsedTime = ((elapsedTime <= duration) ? (elapsedTime / duration) : 1f);
		return distance * elapsedTime * elapsedTime * elapsedTime * elapsedTime + start;
	}

	private static float EaseOutQuart(float start, float distance, float elapsedTime, float duration)
	{
		elapsedTime = ((elapsedTime <= duration) ? (elapsedTime / duration) : 1f);
		elapsedTime -= 1f;
		return -distance * (elapsedTime * elapsedTime * elapsedTime * elapsedTime - 1f) + start;
	}

	private static float EaseInOutQuart(float start, float distance, float elapsedTime, float duration)
	{
		elapsedTime = ((elapsedTime <= duration) ? (elapsedTime / (duration / 2f)) : 2f);
		if (elapsedTime < 1f)
		{
			return distance / 2f * elapsedTime * elapsedTime * elapsedTime * elapsedTime + start;
		}
		elapsedTime -= 2f;
		return -distance / 2f * (elapsedTime * elapsedTime * elapsedTime * elapsedTime - 2f) + start;
	}

	private static float EaseInQuint(float start, float distance, float elapsedTime, float duration)
	{
		elapsedTime = ((elapsedTime <= duration) ? (elapsedTime / duration) : 1f);
		return distance * elapsedTime * elapsedTime * elapsedTime * elapsedTime * elapsedTime + start;
	}

	private static float EaseOutQuint(float start, float distance, float elapsedTime, float duration)
	{
		elapsedTime = ((elapsedTime <= duration) ? (elapsedTime / duration) : 1f);
		elapsedTime -= 1f;
		return distance * (elapsedTime * elapsedTime * elapsedTime * elapsedTime * elapsedTime + 1f) + start;
	}

	private static float EaseInOutQuint(float start, float distance, float elapsedTime, float duration)
	{
		elapsedTime = ((elapsedTime <= duration) ? (elapsedTime / (duration / 2f)) : 2f);
		if (elapsedTime < 1f)
		{
			return distance / 2f * elapsedTime * elapsedTime * elapsedTime * elapsedTime * elapsedTime + start;
		}
		elapsedTime -= 2f;
		return distance / 2f * (elapsedTime * elapsedTime * elapsedTime * elapsedTime * elapsedTime + 2f) + start;
	}

	private static float EaseInSine(float start, float distance, float elapsedTime, float duration)
	{
		if (elapsedTime > duration)
		{
			elapsedTime = duration;
		}
		return -distance * global::UnityEngine.Mathf.Cos(elapsedTime / duration * 1.57079637f) + distance + start;
	}

	private static float EaseOutSine(float start, float distance, float elapsedTime, float duration)
	{
		if (elapsedTime > duration)
		{
			elapsedTime = duration;
		}
		return distance * global::UnityEngine.Mathf.Sin(elapsedTime / duration * 1.57079637f) + start;
	}

	private static float EaseInOutSine(float start, float distance, float elapsedTime, float duration)
	{
		if (elapsedTime > duration)
		{
			elapsedTime = duration;
		}
		return -distance / 2f * (global::UnityEngine.Mathf.Cos(3.14159274f * elapsedTime / duration) - 1f) + start;
	}

	private static float EaseInExpo(float start, float distance, float elapsedTime, float duration)
	{
		if (elapsedTime > duration)
		{
			elapsedTime = duration;
		}
		return distance * global::UnityEngine.Mathf.Pow(2f, 10f * (elapsedTime / duration - 1f)) + start;
	}

	private static float EaseOutExpo(float start, float distance, float elapsedTime, float duration)
	{
		if (elapsedTime > duration)
		{
			elapsedTime = duration;
		}
		return distance * (-global::UnityEngine.Mathf.Pow(2f, -10f * elapsedTime / duration) + 1f) + start;
	}

	private static float EaseInOutExpo(float start, float distance, float elapsedTime, float duration)
	{
		elapsedTime = ((elapsedTime <= duration) ? (elapsedTime / (duration / 2f)) : 2f);
		if (elapsedTime < 1f)
		{
			return distance / 2f * global::UnityEngine.Mathf.Pow(2f, 10f * (elapsedTime - 1f)) + start;
		}
		elapsedTime -= 1f;
		return distance / 2f * (-global::UnityEngine.Mathf.Pow(2f, -10f * elapsedTime) + 2f) + start;
	}

	private static float EaseInCirc(float start, float distance, float elapsedTime, float duration)
	{
		elapsedTime = ((elapsedTime <= duration) ? (elapsedTime / duration) : 1f);
		return -distance * (global::UnityEngine.Mathf.Sqrt(1f - elapsedTime * elapsedTime) - 1f) + start;
	}

	private static float EaseOutCirc(float start, float distance, float elapsedTime, float duration)
	{
		elapsedTime = ((elapsedTime <= duration) ? (elapsedTime / duration) : 1f);
		elapsedTime -= 1f;
		return distance * global::UnityEngine.Mathf.Sqrt(1f - elapsedTime * elapsedTime) + start;
	}

	private static float EaseInOutCirc(float start, float distance, float elapsedTime, float duration)
	{
		elapsedTime = ((elapsedTime <= duration) ? (elapsedTime / (duration / 2f)) : 2f);
		if (elapsedTime < 1f)
		{
			return -distance / 2f * (global::UnityEngine.Mathf.Sqrt(1f - elapsedTime * elapsedTime) - 1f) + start;
		}
		elapsedTime -= 2f;
		return distance / 2f * (global::UnityEngine.Mathf.Sqrt(1f - elapsedTime * elapsedTime) + 1f) + start;
	}

	public enum EaseType
	{
		Linear,
		EaseInQuad,
		EaseOutQuad,
		EaseInOutQuad,
		EaseInCubic,
		EaseOutCubic,
		EaseInOutCubic,
		EaseInQuart,
		EaseOutQuart,
		EaseInOutQuart,
		EaseInQuint,
		EaseOutQuint,
		EaseInOutQuint,
		EaseInSine,
		EaseOutSine,
		EaseInOutSine,
		EaseInExpo,
		EaseOutExpo,
		EaseInOutExpo,
		EaseInCirc,
		EaseOutCirc,
		EaseInOutCirc
	}

	public delegate global::UnityEngine.Vector3 ToVector3<T>(T v);

	public delegate float Function(float a, float b, float c, float d);
}
