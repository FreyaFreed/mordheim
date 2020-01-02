using System;
using UnityEngine;

[global::UnityEngine.AddComponentMenu("Dynamic Bone/Dynamic Bone Collider")]
public class DynamicBoneCollider : global::UnityEngine.MonoBehaviour
{
	private void OnValidate()
	{
		this.m_Radius = global::UnityEngine.Mathf.Max(this.m_Radius, 0f);
		this.m_Height = global::UnityEngine.Mathf.Max(this.m_Height, 0f);
	}

	public void Collide(ref global::UnityEngine.Vector3 particlePosition, float particleRadius)
	{
		float num = this.m_Radius * global::UnityEngine.Mathf.Abs(base.transform.lossyScale.x);
		float num2 = this.m_Height * 0.5f - num;
		if (num2 <= 0f)
		{
			if (this.m_Bound == global::DynamicBoneCollider.Bound.Outside)
			{
				global::DynamicBoneCollider.OutsideSphere(ref particlePosition, particleRadius, base.transform.TransformPoint(this.m_Center), num);
			}
			else
			{
				global::DynamicBoneCollider.InsideSphere(ref particlePosition, particleRadius, base.transform.TransformPoint(this.m_Center), num);
			}
		}
		else
		{
			global::UnityEngine.Vector3 center = this.m_Center;
			global::UnityEngine.Vector3 center2 = this.m_Center;
			switch (this.m_Direction)
			{
			case global::DynamicBoneCollider.Direction.X:
				center.x -= num2;
				center2.x += num2;
				break;
			case global::DynamicBoneCollider.Direction.Y:
				center.y -= num2;
				center2.y += num2;
				break;
			case global::DynamicBoneCollider.Direction.Z:
				center.z -= num2;
				center2.z += num2;
				break;
			}
			if (this.m_Bound == global::DynamicBoneCollider.Bound.Outside)
			{
				global::DynamicBoneCollider.OutsideCapsule(ref particlePosition, particleRadius, base.transform.TransformPoint(center), base.transform.TransformPoint(center2), num);
			}
			else
			{
				global::DynamicBoneCollider.InsideCapsule(ref particlePosition, particleRadius, base.transform.TransformPoint(center), base.transform.TransformPoint(center2), num);
			}
		}
	}

	private static void OutsideSphere(ref global::UnityEngine.Vector3 particlePosition, float particleRadius, global::UnityEngine.Vector3 sphereCenter, float sphereRadius)
	{
		float num = sphereRadius + particleRadius;
		float num2 = num * num;
		global::UnityEngine.Vector3 a = particlePosition - sphereCenter;
		float sqrMagnitude = a.sqrMagnitude;
		if (sqrMagnitude > 0f && sqrMagnitude < num2)
		{
			float num3 = global::UnityEngine.Mathf.Sqrt(sqrMagnitude);
			particlePosition = sphereCenter + a * (num / num3);
		}
	}

	private static void InsideSphere(ref global::UnityEngine.Vector3 particlePosition, float particleRadius, global::UnityEngine.Vector3 sphereCenter, float sphereRadius)
	{
		float num = sphereRadius + particleRadius;
		float num2 = num * num;
		global::UnityEngine.Vector3 a = particlePosition - sphereCenter;
		float sqrMagnitude = a.sqrMagnitude;
		if (sqrMagnitude > num2)
		{
			float num3 = global::UnityEngine.Mathf.Sqrt(sqrMagnitude);
			particlePosition = sphereCenter + a * (num / num3);
		}
	}

	private static void OutsideCapsule(ref global::UnityEngine.Vector3 particlePosition, float particleRadius, global::UnityEngine.Vector3 capsuleP0, global::UnityEngine.Vector3 capsuleP1, float capsuleRadius)
	{
		float num = capsuleRadius + particleRadius;
		float num2 = num * num;
		global::UnityEngine.Vector3 vector = capsuleP1 - capsuleP0;
		global::UnityEngine.Vector3 vector2 = particlePosition - capsuleP0;
		float num3 = global::UnityEngine.Vector3.Dot(vector2, vector);
		if (num3 <= 0f)
		{
			float sqrMagnitude = vector2.sqrMagnitude;
			if (sqrMagnitude > 0f && sqrMagnitude < num2)
			{
				float num4 = global::UnityEngine.Mathf.Sqrt(sqrMagnitude);
				particlePosition = capsuleP0 + vector2 * (num / num4);
			}
		}
		else
		{
			float sqrMagnitude2 = vector.sqrMagnitude;
			if (num3 >= sqrMagnitude2)
			{
				vector2 = particlePosition - capsuleP1;
				float sqrMagnitude3 = vector2.sqrMagnitude;
				if (sqrMagnitude3 > 0f && sqrMagnitude3 < num2)
				{
					float num5 = global::UnityEngine.Mathf.Sqrt(sqrMagnitude3);
					particlePosition = capsuleP1 + vector2 * (num / num5);
				}
			}
			else if (sqrMagnitude2 > 0f)
			{
				num3 /= sqrMagnitude2;
				vector2 -= vector * num3;
				float sqrMagnitude4 = vector2.sqrMagnitude;
				if (sqrMagnitude4 > 0f && sqrMagnitude4 < num2)
				{
					float num6 = global::UnityEngine.Mathf.Sqrt(sqrMagnitude4);
					particlePosition += vector2 * ((num - num6) / num6);
				}
			}
		}
	}

	private static void InsideCapsule(ref global::UnityEngine.Vector3 particlePosition, float particleRadius, global::UnityEngine.Vector3 capsuleP0, global::UnityEngine.Vector3 capsuleP1, float capsuleRadius)
	{
		float num = capsuleRadius + particleRadius;
		float num2 = num * num;
		global::UnityEngine.Vector3 vector = capsuleP1 - capsuleP0;
		global::UnityEngine.Vector3 vector2 = particlePosition - capsuleP0;
		float num3 = global::UnityEngine.Vector3.Dot(vector2, vector);
		if (num3 <= 0f)
		{
			float sqrMagnitude = vector2.sqrMagnitude;
			if (sqrMagnitude > num2)
			{
				float num4 = global::UnityEngine.Mathf.Sqrt(sqrMagnitude);
				particlePosition = capsuleP0 + vector2 * (num / num4);
			}
		}
		else
		{
			float sqrMagnitude2 = vector.sqrMagnitude;
			if (num3 >= sqrMagnitude2)
			{
				vector2 = particlePosition - capsuleP1;
				float sqrMagnitude3 = vector2.sqrMagnitude;
				if (sqrMagnitude3 > num2)
				{
					float num5 = global::UnityEngine.Mathf.Sqrt(sqrMagnitude3);
					particlePosition = capsuleP1 + vector2 * (num / num5);
				}
			}
			else if (sqrMagnitude2 > 0f)
			{
				num3 /= sqrMagnitude2;
				vector2 -= vector * num3;
				float sqrMagnitude4 = vector2.sqrMagnitude;
				if (sqrMagnitude4 > num2)
				{
					float num6 = global::UnityEngine.Mathf.Sqrt(sqrMagnitude4);
					particlePosition += vector2 * ((num - num6) / num6);
				}
			}
		}
	}

	private void OnDrawGizmosSelected()
	{
		if (!base.enabled)
		{
			return;
		}
		if (this.m_Bound == global::DynamicBoneCollider.Bound.Outside)
		{
			global::UnityEngine.Gizmos.color = global::UnityEngine.Color.yellow;
		}
		else
		{
			global::UnityEngine.Gizmos.color = global::UnityEngine.Color.magenta;
		}
		float num = this.m_Radius * global::UnityEngine.Mathf.Abs(base.transform.lossyScale.x);
		float num2 = this.m_Height * 0.5f - num;
		if (num2 <= 0f)
		{
			global::UnityEngine.Gizmos.DrawWireSphere(base.transform.TransformPoint(this.m_Center), num);
		}
		else
		{
			global::UnityEngine.Vector3 center = this.m_Center;
			global::UnityEngine.Vector3 center2 = this.m_Center;
			switch (this.m_Direction)
			{
			case global::DynamicBoneCollider.Direction.X:
				center.x -= num2;
				center2.x += num2;
				break;
			case global::DynamicBoneCollider.Direction.Y:
				center.y -= num2;
				center2.y += num2;
				break;
			case global::DynamicBoneCollider.Direction.Z:
				center.z -= num2;
				center2.z += num2;
				break;
			}
			global::UnityEngine.Gizmos.DrawWireSphere(base.transform.TransformPoint(center), num);
			global::UnityEngine.Gizmos.DrawWireSphere(base.transform.TransformPoint(center2), num);
		}
	}

	public global::UnityEngine.Vector3 m_Center = global::UnityEngine.Vector3.zero;

	public float m_Radius = 0.5f;

	public float m_Height;

	public global::DynamicBoneCollider.Direction m_Direction;

	public global::DynamicBoneCollider.Bound m_Bound;

	public enum Direction
	{
		X,
		Y,
		Z
	}

	public enum Bound
	{
		Outside,
		Inside
	}
}
