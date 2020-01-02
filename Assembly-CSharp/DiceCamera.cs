using System;
using UnityEngine;

public class DiceCamera : global::CameraBase
{
	private void Update()
	{
	}

	public override void GetNextPositionAngle(ref global::UnityEngine.Vector3 position, ref global::UnityEngine.Quaternion angle)
	{
		position = this.GetPosition();
		angle = this.GetAngle();
	}

	private global::UnityEngine.Vector3 GetPosition()
	{
		return base.transform.position;
	}

	private global::UnityEngine.Quaternion GetAngle()
	{
		return base.transform.rotation;
	}

	public override void SetTarget(global::UnityEngine.Transform target)
	{
		this.targetPosition = target;
	}

	public override global::UnityEngine.Transform GetTarget()
	{
		return this.targetPosition;
	}

	private global::UnityEngine.Transform targetPosition;
}
