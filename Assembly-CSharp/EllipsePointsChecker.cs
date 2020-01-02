using System;
using UnityEngine;

public class EllipsePointsChecker : global::PointsChecker
{
	public EllipsePointsChecker(global::UnityEngine.Transform transform, bool hasOffset, float radius1, float radius2) : base(transform, hasOffset)
	{
		this.radiusA = radius1;
		this.radiusB = radius2;
	}

	public override bool GetPoint(global::UnityEngine.Vector3 startPoint, float angle, float dist, out global::UnityEngine.Vector3 pos)
	{
		float f = angle * 0.0174532924f;
		float num = this.radiusA * this.radiusB / global::UnityEngine.Mathf.Sqrt(global::UnityEngine.Mathf.Pow(this.radiusB, 2f) + global::UnityEngine.Mathf.Pow(this.radiusA, 2f) * global::UnityEngine.Mathf.Pow(global::UnityEngine.Mathf.Tan(f), 2f));
		num *= (((0f > angle || angle >= 90f) && (270f >= angle || angle > 360f)) ? 1f : -1f);
		float z = global::UnityEngine.Mathf.Tan(f) * num;
		global::UnityEngine.Vector3 vector = new global::UnityEngine.Vector3(num, 0f, z);
		vector = this.zoneTransform.rotation * -vector;
		dist = vector.magnitude;
		vector /= dist;
		pos = global::UnityEngine.Vector3.zero;
		if (!global::PandoraUtils.SendCapsule(startPoint, vector, 0.6f, 1.5f, dist, 0.5f))
		{
			pos = startPoint + vector * dist;
			return true;
		}
		return false;
	}

	private float radiusA;

	private float radiusB;
}
