using System;
using System.Collections;
using Pathfinding;
using UnityEngine;

[global::UnityEngine.RequireComponent(typeof(global::Seeker))]
[global::UnityEngine.AddComponentMenu("Pathfinding/AI/AIFollow (deprecated)")]
[global::UnityEngine.HelpURL("http://arongranberg.com/astar/docs/class_a_i_follow.php")]
[global::UnityEngine.RequireComponent(typeof(global::UnityEngine.CharacterController))]
public class AIFollow : global::UnityEngine.MonoBehaviour
{
	public void Start()
	{
		this.seeker = base.GetComponent<global::Seeker>();
		this.controller = base.GetComponent<global::UnityEngine.CharacterController>();
		this.tr = base.transform;
		this.Repath();
	}

	public void Reset()
	{
		this.path = null;
	}

	public void OnPathComplete(global::Pathfinding.Path p)
	{
		base.StartCoroutine(this.WaitToRepath());
		if (p.error)
		{
			return;
		}
		this.path = p.vectorPath.ToArray();
		float num = float.PositiveInfinity;
		int num2 = 0;
		for (int i = 0; i < this.path.Length - 1; i++)
		{
			float num3 = global::Pathfinding.VectorMath.SqrDistancePointSegment(this.path[i], this.path[i + 1], this.tr.position);
			if (num3 < num)
			{
				num2 = 0;
				num = num3;
				this.pathIndex = i + 1;
			}
			else if (num2 > 6)
			{
				break;
			}
		}
	}

	public global::System.Collections.IEnumerator WaitToRepath()
	{
		float timeLeft = this.repathRate - (global::UnityEngine.Time.time - this.lastPathSearch);
		yield return new global::UnityEngine.WaitForSeconds(timeLeft);
		this.Repath();
		yield break;
	}

	public void Stop()
	{
		this.canMove = false;
		this.canSearch = false;
	}

	public void Resume()
	{
		this.canMove = true;
		this.canSearch = true;
	}

	public virtual void Repath()
	{
		this.lastPathSearch = global::UnityEngine.Time.time;
		if (this.seeker == null || this.target == null || !this.canSearch || !this.seeker.IsDone())
		{
			base.StartCoroutine(this.WaitToRepath());
			return;
		}
		global::Pathfinding.Path p = global::Pathfinding.ABPath.Construct(base.transform.position, this.target.position, null);
		this.seeker.StartPath(p, new global::Pathfinding.OnPathDelegate(this.OnPathComplete), -1);
	}

	public void PathToTarget(global::UnityEngine.Vector3 targetPoint)
	{
		this.lastPathSearch = global::UnityEngine.Time.time;
		if (this.seeker == null)
		{
			return;
		}
		this.seeker.StartPath(base.transform.position, targetPoint, new global::Pathfinding.OnPathDelegate(this.OnPathComplete));
	}

	public virtual void ReachedEndOfPath()
	{
	}

	public void Update()
	{
		if (this.path == null || this.pathIndex >= this.path.Length || this.pathIndex < 0 || !this.canMove)
		{
			return;
		}
		global::UnityEngine.Vector3 a = this.path[this.pathIndex];
		a.y = this.tr.position.y;
		while ((a - this.tr.position).sqrMagnitude < this.pickNextWaypointDistance * this.pickNextWaypointDistance)
		{
			this.pathIndex++;
			if (this.pathIndex >= this.path.Length)
			{
				if ((a - this.tr.position).sqrMagnitude < this.pickNextWaypointDistance * this.targetReached * (this.pickNextWaypointDistance * this.targetReached))
				{
					this.ReachedEndOfPath();
					return;
				}
				this.pathIndex--;
				break;
			}
			else
			{
				a = this.path[this.pathIndex];
				a.y = this.tr.position.y;
			}
		}
		global::UnityEngine.Vector3 forward = a - this.tr.position;
		this.tr.rotation = global::UnityEngine.Quaternion.Slerp(this.tr.rotation, global::UnityEngine.Quaternion.LookRotation(forward), this.rotationSpeed * global::UnityEngine.Time.deltaTime);
		this.tr.eulerAngles = new global::UnityEngine.Vector3(0f, this.tr.eulerAngles.y, 0f);
		global::UnityEngine.Vector3 a2 = base.transform.forward;
		a2 *= this.speed;
		a2 *= global::UnityEngine.Mathf.Clamp01(global::UnityEngine.Vector3.Dot(forward.normalized, this.tr.forward));
		if (this.controller != null)
		{
			this.controller.SimpleMove(a2);
		}
		else
		{
			base.transform.Translate(a2 * global::UnityEngine.Time.deltaTime, global::UnityEngine.Space.World);
		}
	}

	public void OnDrawGizmos()
	{
		if (!this.drawGizmos || this.path == null || this.pathIndex >= this.path.Length || this.pathIndex < 0)
		{
			return;
		}
		global::UnityEngine.Vector3 vector = this.path[this.pathIndex];
		vector.y = this.tr.position.y;
		global::UnityEngine.Debug.DrawLine(base.transform.position, vector, global::UnityEngine.Color.blue);
		float num = this.pickNextWaypointDistance;
		if (this.pathIndex == this.path.Length - 1)
		{
			num *= this.targetReached;
		}
		global::UnityEngine.Vector3 start = vector + num * new global::UnityEngine.Vector3(1f, 0f, 0f);
		float num2 = 0f;
		while ((double)num2 < 6.2831853071795862)
		{
			global::UnityEngine.Vector3 vector2 = vector + new global::UnityEngine.Vector3((float)global::System.Math.Cos((double)num2) * num, 0f, (float)global::System.Math.Sin((double)num2) * num);
			global::UnityEngine.Debug.DrawLine(start, vector2, global::UnityEngine.Color.yellow);
			start = vector2;
			num2 += 0.1f;
		}
		global::UnityEngine.Debug.DrawLine(start, vector + num * new global::UnityEngine.Vector3(1f, 0f, 0f), global::UnityEngine.Color.yellow);
	}

	public global::UnityEngine.Transform target;

	public float repathRate = 0.1f;

	public float pickNextWaypointDistance = 1f;

	public float targetReached = 0.2f;

	public float speed = 5f;

	public float rotationSpeed = 1f;

	public bool drawGizmos;

	public bool canSearch = true;

	public bool canMove = true;

	protected global::Seeker seeker;

	protected global::UnityEngine.CharacterController controller;

	protected global::UnityEngine.Transform tr;

	protected float lastPathSearch = -9999f;

	protected int pathIndex;

	protected global::UnityEngine.Vector3[] path;
}
