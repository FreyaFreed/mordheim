using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using Pathfinding.RVO;
using UnityEngine;

[global::UnityEngine.HelpURL("http://arongranberg.com/astar/docs/class_a_i_path.php")]
[global::UnityEngine.AddComponentMenu("Pathfinding/AI/AIPath (3D)")]
[global::UnityEngine.RequireComponent(typeof(global::Seeker))]
public class AIPath : global::UnityEngine.MonoBehaviour
{
	public bool TargetReached
	{
		get
		{
			return this.targetReached;
		}
	}

	protected virtual void Awake()
	{
		this.seeker = base.GetComponent<global::Seeker>();
		this.tr = base.transform;
		this.controller = base.GetComponent<global::UnityEngine.CharacterController>();
		this.rvoController = base.GetComponent<global::Pathfinding.RVO.RVOController>();
		this.rigid = base.GetComponent<global::UnityEngine.Rigidbody>();
	}

	protected virtual void Start()
	{
		this.startHasRun = true;
		this.OnEnable();
	}

	protected virtual void OnEnable()
	{
		this.lastRepath = -9999f;
		this.canSearchAgain = true;
		this.lastFoundWaypointPosition = this.GetFeetPosition();
		if (this.startHasRun)
		{
			global::Seeker seeker = this.seeker;
			seeker.pathCallback = (global::Pathfinding.OnPathDelegate)global::System.Delegate.Combine(seeker.pathCallback, new global::Pathfinding.OnPathDelegate(this.OnPathComplete));
			base.StartCoroutine(this.RepeatTrySearchPath());
		}
	}

	public void OnDisable()
	{
		if (this.seeker != null && !this.seeker.IsDone())
		{
			this.seeker.GetCurrentPath().Error();
		}
		if (this.path != null)
		{
			this.path.Release(this, false);
		}
		this.path = null;
		global::Seeker seeker = this.seeker;
		seeker.pathCallback = (global::Pathfinding.OnPathDelegate)global::System.Delegate.Remove(seeker.pathCallback, new global::Pathfinding.OnPathDelegate(this.OnPathComplete));
	}

	protected global::System.Collections.IEnumerator RepeatTrySearchPath()
	{
		for (;;)
		{
			float v = this.TrySearchPath();
			yield return new global::UnityEngine.WaitForSeconds(v);
		}
		yield break;
	}

	public float TrySearchPath()
	{
		if (global::UnityEngine.Time.time - this.lastRepath >= this.repathRate && this.canSearchAgain && this.canSearch && this.target != null)
		{
			this.SearchPath();
			return this.repathRate;
		}
		float num = this.repathRate - (global::UnityEngine.Time.time - this.lastRepath);
		return (num >= 0f) ? num : 0f;
	}

	public virtual void SearchPath()
	{
		if (this.target == null)
		{
			throw new global::System.InvalidOperationException("Target is null");
		}
		this.lastRepath = global::UnityEngine.Time.time;
		global::UnityEngine.Vector3 position = this.target.position;
		this.canSearchAgain = false;
		this.seeker.StartPath(this.GetFeetPosition(), position);
	}

	public virtual void OnTargetReached()
	{
	}

	public virtual void OnPathComplete(global::Pathfinding.Path _p)
	{
		global::Pathfinding.ABPath abpath = _p as global::Pathfinding.ABPath;
		if (abpath == null)
		{
			throw new global::System.Exception("This function only handles ABPaths, do not use special path types");
		}
		this.canSearchAgain = true;
		abpath.Claim(this);
		if (abpath.error)
		{
			abpath.Release(this, false);
			return;
		}
		if (this.path != null)
		{
			this.path.Release(this, false);
		}
		this.path = abpath;
		this.currentWaypointIndex = 0;
		this.targetReached = false;
		if (this.closestOnPathCheck)
		{
			global::UnityEngine.Vector3 vector = (global::UnityEngine.Time.time - this.lastFoundWaypointTime >= 0.3f) ? abpath.originalStartPoint : this.lastFoundWaypointPosition;
			global::UnityEngine.Vector3 feetPosition = this.GetFeetPosition();
			global::UnityEngine.Vector3 vector2 = feetPosition - vector;
			float magnitude = vector2.magnitude;
			vector2 /= magnitude;
			int num = (int)(magnitude / this.pickNextWaypointDist);
			for (int i = 0; i <= num; i++)
			{
				this.CalculateVelocity(vector);
				vector += vector2;
			}
		}
	}

	public virtual global::UnityEngine.Vector3 GetFeetPosition()
	{
		if (this.rvoController != null)
		{
			return this.tr.position - global::UnityEngine.Vector3.up * this.rvoController.height * 0.5f;
		}
		if (this.controller != null)
		{
			return this.tr.position - global::UnityEngine.Vector3.up * this.controller.height * 0.5f;
		}
		return this.tr.position;
	}

	public virtual void Update()
	{
		if (!this.canMove)
		{
			return;
		}
		global::UnityEngine.Vector3 vector = this.CalculateVelocity(this.GetFeetPosition());
		this.RotateTowards(this.targetDirection);
		if (this.rvoController != null)
		{
			this.rvoController.Move(vector);
			vector = this.rvoController.velocity;
		}
		if (this.controller != null)
		{
			this.controller.SimpleMove(vector);
		}
		else if (this.rigid != null)
		{
			this.rigid.AddForce(vector);
		}
		else
		{
			this.tr.Translate(vector * global::UnityEngine.Time.deltaTime, global::UnityEngine.Space.World);
		}
	}

	protected float XZSqrMagnitude(global::UnityEngine.Vector3 a, global::UnityEngine.Vector3 b)
	{
		float num = b.x - a.x;
		float num2 = b.z - a.z;
		return num * num + num2 * num2;
	}

	private static global::UnityEngine.Vector2 To2D(global::UnityEngine.Vector3 p)
	{
		return new global::UnityEngine.Vector2(p.x, p.z);
	}

	protected global::UnityEngine.Vector3 CalculateVelocity(global::UnityEngine.Vector3 currentPosition)
	{
		if (this.path == null || this.path.vectorPath == null || this.path.vectorPath.Count == 0)
		{
			return global::UnityEngine.Vector3.zero;
		}
		global::System.Collections.Generic.List<global::UnityEngine.Vector3> vectorPath = this.path.vectorPath;
		if (vectorPath.Count == 1)
		{
			vectorPath.Insert(0, currentPosition);
		}
		if (this.currentWaypointIndex >= vectorPath.Count)
		{
			this.currentWaypointIndex = vectorPath.Count - 1;
		}
		if (this.currentWaypointIndex <= 1)
		{
			this.currentWaypointIndex = 1;
		}
		while (this.currentWaypointIndex < vectorPath.Count - 1)
		{
			float num = global::Pathfinding.VectorMath.SqrDistanceXZ(vectorPath[this.currentWaypointIndex], currentPosition);
			if (num >= this.pickNextWaypointDist * this.pickNextWaypointDist)
			{
				break;
			}
			this.lastFoundWaypointPosition = currentPosition;
			this.lastFoundWaypointTime = global::UnityEngine.Time.time;
			this.currentWaypointIndex++;
		}
		global::UnityEngine.Vector3 vector = vectorPath[this.currentWaypointIndex - 1];
		global::UnityEngine.Vector3 vector2 = vectorPath[this.currentWaypointIndex];
		float num2 = global::Pathfinding.VectorMath.LineCircleIntersectionFactor(global::AIPath.To2D(currentPosition), global::AIPath.To2D(vector), global::AIPath.To2D(vector2), this.pickNextWaypointDist);
		num2 = global::UnityEngine.Mathf.Clamp01(num2);
		global::UnityEngine.Vector3 a = global::UnityEngine.Vector3.Lerp(vector, vector2, num2);
		global::UnityEngine.Vector3 vector3 = a - currentPosition;
		vector3.y = 0f;
		float magnitude = vector3.magnitude;
		float num3 = (this.slowdownDistance <= 0f) ? 1f : global::UnityEngine.Mathf.Clamp01(magnitude / this.slowdownDistance);
		this.targetDirection = vector3;
		this.targetPoint = a;
		if (this.currentWaypointIndex == vectorPath.Count - 1 && magnitude <= this.endReachedDistance)
		{
			if (!this.targetReached)
			{
				this.targetReached = true;
				this.OnTargetReached();
			}
			return global::UnityEngine.Vector3.zero;
		}
		global::UnityEngine.Vector3 forward = this.tr.forward;
		float a2 = global::UnityEngine.Vector3.Dot(vector3.normalized, forward);
		float num4 = this.speed * global::UnityEngine.Mathf.Max(a2, this.minMoveScale) * num3;
		if (global::UnityEngine.Time.deltaTime > 0f)
		{
			num4 = global::UnityEngine.Mathf.Clamp(num4, 0f, magnitude / (global::UnityEngine.Time.deltaTime * 2f));
		}
		return forward * num4;
	}

	protected virtual void RotateTowards(global::UnityEngine.Vector3 dir)
	{
		if (dir == global::UnityEngine.Vector3.zero)
		{
			return;
		}
		global::UnityEngine.Quaternion quaternion = this.tr.rotation;
		global::UnityEngine.Quaternion b = global::UnityEngine.Quaternion.LookRotation(dir);
		global::UnityEngine.Vector3 eulerAngles = global::UnityEngine.Quaternion.Slerp(quaternion, b, this.turningSpeed * global::UnityEngine.Time.deltaTime).eulerAngles;
		eulerAngles.z = 0f;
		eulerAngles.x = 0f;
		quaternion = global::UnityEngine.Quaternion.Euler(eulerAngles);
		this.tr.rotation = quaternion;
	}

	public float repathRate = 0.5f;

	public global::UnityEngine.Transform target;

	public bool canSearch = true;

	public bool canMove = true;

	public float speed = 3f;

	public float turningSpeed = 5f;

	public float slowdownDistance = 0.6f;

	public float pickNextWaypointDist = 2f;

	public float endReachedDistance = 0.2f;

	public bool closestOnPathCheck = true;

	protected float minMoveScale = 0.05f;

	protected global::Seeker seeker;

	protected global::UnityEngine.Transform tr;

	protected float lastRepath = -9999f;

	protected global::Pathfinding.Path path;

	protected global::UnityEngine.CharacterController controller;

	protected global::Pathfinding.RVO.RVOController rvoController;

	protected global::UnityEngine.Rigidbody rigid;

	protected int currentWaypointIndex;

	protected bool targetReached;

	protected bool canSearchAgain = true;

	protected global::UnityEngine.Vector3 lastFoundWaypointPosition;

	protected float lastFoundWaypointTime = -9999f;

	private bool startHasRun;

	protected global::UnityEngine.Vector3 targetPoint;

	protected global::UnityEngine.Vector3 targetDirection;
}
