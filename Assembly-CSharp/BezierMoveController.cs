using System;
using UnityEngine;
using UnityEngine.Events;

public class BezierMoveController : global::MoveController
{
	private void FixedUpdate()
	{
		this.Move();
	}

	private void Move()
	{
		if (this.timer > 0f)
		{
			if (this.currentTime > this.timer)
			{
				this.currentTime = this.timer;
			}
			float num = this.currentTime / this.timer;
			global::UnityEngine.Vector3 vector = (1f - num) * ((1f - num) * this.start + num * this.summit) + num * ((1f - num) * this.summit + num * this.end);
			global::UnityEngine.Vector3 position = base.transform.position;
			base.transform.position = vector;
			global::UnityEngine.Vector3 forward = position - vector;
			forward = new global::UnityEngine.Vector3(-forward.x, forward.y, -forward.z);
			base.transform.rotation = global::UnityEngine.Quaternion.LookRotation(forward);
			if (global::UnityEngine.Mathf.Approximately(this.currentTime, this.timer))
			{
				this.StopMoving();
			}
			else
			{
				this.currentTime += global::UnityEngine.Time.fixedDeltaTime;
			}
		}
	}

	public void StartMoving(global::UnityEngine.Vector3 startPosition, global::UnityEngine.Vector3 endPosition, float height, float speed, global::UnityEngine.Events.UnityAction onDestination)
	{
		base.enabled = true;
		this.currentSpeed = speed * 10f;
		base.transform.position = startPosition;
		if (global::UnityEngine.Vector3.SqrMagnitude(startPosition - endPosition) < 4f)
		{
			height = 0f;
		}
		this.start = startPosition;
		this.end = endPosition;
		this.summit = startPosition + (endPosition - startPosition) / 2f + global::UnityEngine.Vector3.up * height;
		this.timer = global::UnityEngine.Vector3.Distance(startPosition, endPosition) / this.currentSpeed;
		this.currentTime = 0f;
		this.onDestination = onDestination;
		this.Move();
	}

	public override void StopMoving()
	{
		base.StopMoving();
		this.timer = 0f;
		if (this.onDestination != null)
		{
			this.onDestination();
		}
	}

	private global::UnityEngine.Vector3 start = global::UnityEngine.Vector3.zero;

	private global::UnityEngine.Vector3 end = global::UnityEngine.Vector3.zero;

	private global::UnityEngine.Vector3 summit = global::UnityEngine.Vector3.zero;

	private float timer;

	private float currentTime;

	private global::UnityEngine.Events.UnityAction onDestination;
}
