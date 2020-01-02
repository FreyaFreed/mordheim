using System;
using UnityEngine;

public class MoveController : global::UnityEngine.MonoBehaviour
{
	private void Awake()
	{
		base.enabled = false;
	}

	private void FixedUpdate()
	{
		global::UnityEngine.Vector3 position = base.transform.position + this.direction * this.currentSpeed * global::UnityEngine.Time.fixedDeltaTime;
		base.transform.position = position;
	}

	public void StartMoving(global::UnityEngine.Vector3 dir, float speed)
	{
		base.enabled = true;
		this.direction = dir;
		this.currentSpeed = speed;
	}

	public virtual void StopMoving()
	{
		base.enabled = false;
		this.currentSpeed = 0f;
	}

	protected float currentSpeed;

	private global::UnityEngine.Vector3 direction;
}
