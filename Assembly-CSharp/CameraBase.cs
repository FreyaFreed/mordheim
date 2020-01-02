using System;
using UnityEngine;

public abstract class CameraBase : global::UnityEngine.MonoBehaviour
{
	public virtual void GetNextPositionAngle(ref global::UnityEngine.Vector3 position, ref global::UnityEngine.Quaternion angle)
	{
		position = base.transform.position;
		angle = base.transform.rotation;
	}

	public virtual void SetTarget(global::UnityEngine.Transform target)
	{
		this.target = target;
	}

	public virtual global::UnityEngine.Transform GetTarget()
	{
		return this.target;
	}

	public global::UnityEngine.Vector3 OffsetPosition(global::UnityEngine.Transform trans, global::UnityEngine.Vector3 offset)
	{
		global::UnityEngine.Vector3 a = trans.position;
		a += trans.forward * offset.z;
		a += trans.up * offset.y;
		return a + trans.right * offset.x;
	}

	public global::UnityEngine.Vector3 OrientOffset(global::UnityEngine.Transform trans, global::UnityEngine.Vector3 offset)
	{
		global::UnityEngine.Vector3 a = global::UnityEngine.Vector3.zero;
		a += trans.forward * offset.z;
		a += trans.up * offset.y;
		return a + trans.right * offset.x;
	}

	protected global::UnityEngine.Transform target;
}
