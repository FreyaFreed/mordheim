using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

[global::UnityEngine.AddComponentMenu("Pathfinding/AI/AISimpleLerp (2D,3D generic)")]
[global::UnityEngine.HelpURL("http://arongranberg.com/astar/docs/class_a_i_lerp.php")]
[global::UnityEngine.RequireComponent(typeof(global::Seeker))]
public class AILerp : global::UnityEngine.MonoBehaviour
{
	public bool targetReached { get; private set; }

	protected virtual void Awake()
	{
		this.tr = base.transform;
		this.seeker = base.GetComponent<global::Seeker>();
		this.seeker.startEndModifier.adjustStartPoint = (() => this.tr.position);
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
		this.ForceSearchPath();
	}

	public virtual void ForceSearchPath()
	{
		if (this.target == null)
		{
			throw new global::System.InvalidOperationException("Target is null");
		}
		this.lastRepath = global::UnityEngine.Time.time;
		global::UnityEngine.Vector3 position = this.target.position;
		global::UnityEngine.Vector3 start = this.GetFeetPosition();
		if (this.path != null && this.path.vectorPath.Count > 1)
		{
			start = this.path.vectorPath[this.currentWaypointIndex];
		}
		this.canSearchAgain = false;
		this.seeker.StartPath(start, position);
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
		if (this.interpolatePathSwitches)
		{
			this.ConfigurePathSwitchInterpolation();
		}
		if (this.path != null)
		{
			this.path.Release(this, false);
		}
		this.path = abpath;
		if (this.path.vectorPath != null && this.path.vectorPath.Count == 1)
		{
			this.path.vectorPath.Insert(0, this.GetFeetPosition());
		}
		this.targetReached = false;
		this.ConfigureNewPath();
	}

	protected virtual void ConfigurePathSwitchInterpolation()
	{
		bool flag = this.path != null && this.path.vectorPath != null && this.path.vectorPath.Count > 1;
		bool flag2 = false;
		if (flag)
		{
			flag2 = (this.currentWaypointIndex == this.path.vectorPath.Count - 1 && this.distanceAlongSegment >= (this.path.vectorPath[this.path.vectorPath.Count - 1] - this.path.vectorPath[this.path.vectorPath.Count - 2]).magnitude);
		}
		if (flag && !flag2)
		{
			global::System.Collections.Generic.List<global::UnityEngine.Vector3> vectorPath = this.path.vectorPath;
			this.currentWaypointIndex = global::UnityEngine.Mathf.Clamp(this.currentWaypointIndex, 1, vectorPath.Count - 1);
			global::UnityEngine.Vector3 vector = vectorPath[this.currentWaypointIndex] - vectorPath[this.currentWaypointIndex - 1];
			float magnitude = vector.magnitude;
			float num = magnitude * global::UnityEngine.Mathf.Clamp01(1f - this.distanceAlongSegment);
			for (int i = this.currentWaypointIndex; i < vectorPath.Count - 1; i++)
			{
				num += (vectorPath[i + 1] - vectorPath[i]).magnitude;
			}
			this.previousMovementOrigin = this.GetFeetPosition();
			this.previousMovementDirection = vector.normalized * num;
			this.previousMovementStartTime = global::UnityEngine.Time.time;
		}
		else
		{
			this.previousMovementOrigin = global::UnityEngine.Vector3.zero;
			this.previousMovementDirection = global::UnityEngine.Vector3.zero;
			this.previousMovementStartTime = -9999f;
		}
	}

	public virtual global::UnityEngine.Vector3 GetFeetPosition()
	{
		return this.tr.position;
	}

	protected virtual void ConfigureNewPath()
	{
		global::System.Collections.Generic.List<global::UnityEngine.Vector3> vectorPath = this.path.vectorPath;
		global::UnityEngine.Vector3 feetPosition = this.GetFeetPosition();
		float num = 0f;
		float num2 = float.PositiveInfinity;
		global::UnityEngine.Vector3 vector = global::UnityEngine.Vector3.zero;
		int num3 = 1;
		for (int i = 0; i < vectorPath.Count - 1; i++)
		{
			float num4 = global::Pathfinding.VectorMath.ClosestPointOnLineFactor(vectorPath[i], vectorPath[i + 1], feetPosition);
			num4 = global::UnityEngine.Mathf.Clamp01(num4);
			global::UnityEngine.Vector3 b = global::UnityEngine.Vector3.Lerp(vectorPath[i], vectorPath[i + 1], num4);
			float sqrMagnitude = (feetPosition - b).sqrMagnitude;
			if (sqrMagnitude < num2)
			{
				num2 = sqrMagnitude;
				vector = vectorPath[i + 1] - vectorPath[i];
				num = num4 * vector.magnitude;
				num3 = i + 1;
			}
		}
		this.currentWaypointIndex = num3;
		this.distanceAlongSegment = num;
		if (this.interpolatePathSwitches && this.switchPathInterpolationSpeed > 0.01f)
		{
			float num5 = global::UnityEngine.Mathf.Max(-global::UnityEngine.Vector3.Dot(this.previousMovementDirection.normalized, vector.normalized), 0f);
			this.distanceAlongSegment -= this.speed * num5 * (1f / this.switchPathInterpolationSpeed);
		}
	}

	protected virtual void Update()
	{
		if (this.canMove)
		{
			global::UnityEngine.Vector3 vector;
			global::UnityEngine.Vector3 position = this.CalculateNextPosition(out vector);
			if (this.enableRotation && vector != global::UnityEngine.Vector3.zero)
			{
				if (this.rotationIn2D)
				{
					float b = global::UnityEngine.Mathf.Atan2(vector.x, -vector.y) * 57.29578f + 180f;
					global::UnityEngine.Vector3 eulerAngles = this.tr.eulerAngles;
					eulerAngles.z = global::UnityEngine.Mathf.LerpAngle(eulerAngles.z, b, global::UnityEngine.Time.deltaTime * this.rotationSpeed);
					this.tr.eulerAngles = eulerAngles;
				}
				else
				{
					global::UnityEngine.Quaternion rotation = this.tr.rotation;
					global::UnityEngine.Quaternion b2 = global::UnityEngine.Quaternion.LookRotation(vector);
					this.tr.rotation = global::UnityEngine.Quaternion.Slerp(rotation, b2, global::UnityEngine.Time.deltaTime * this.rotationSpeed);
				}
			}
			this.tr.position = position;
		}
	}

	protected virtual global::UnityEngine.Vector3 CalculateNextPosition(out global::UnityEngine.Vector3 direction)
	{
		if (this.path == null || this.path.vectorPath == null || this.path.vectorPath.Count == 0)
		{
			direction = global::UnityEngine.Vector3.zero;
			return this.tr.position;
		}
		global::System.Collections.Generic.List<global::UnityEngine.Vector3> vectorPath = this.path.vectorPath;
		this.currentWaypointIndex = global::UnityEngine.Mathf.Clamp(this.currentWaypointIndex, 1, vectorPath.Count - 1);
		global::UnityEngine.Vector3 vector = vectorPath[this.currentWaypointIndex] - vectorPath[this.currentWaypointIndex - 1];
		float num = vector.magnitude;
		this.distanceAlongSegment += global::UnityEngine.Time.deltaTime * this.speed;
		if (this.distanceAlongSegment >= num && this.currentWaypointIndex < vectorPath.Count - 1)
		{
			float num2 = this.distanceAlongSegment - num;
			global::UnityEngine.Vector3 vector2;
			float magnitude;
			for (;;)
			{
				this.currentWaypointIndex++;
				vector2 = vectorPath[this.currentWaypointIndex] - vectorPath[this.currentWaypointIndex - 1];
				magnitude = vector2.magnitude;
				if (num2 <= magnitude || this.currentWaypointIndex == vectorPath.Count - 1)
				{
					break;
				}
				num2 -= magnitude;
			}
			vector = vector2;
			num = magnitude;
			this.distanceAlongSegment = num2;
		}
		if (this.distanceAlongSegment >= num && this.currentWaypointIndex == vectorPath.Count - 1)
		{
			if (!this.targetReached)
			{
				this.OnTargetReached();
			}
			this.targetReached = true;
		}
		global::UnityEngine.Vector3 vector3 = vector * global::UnityEngine.Mathf.Clamp01((num <= 0f) ? 1f : (this.distanceAlongSegment / num)) + vectorPath[this.currentWaypointIndex - 1];
		direction = vector;
		if (this.interpolatePathSwitches)
		{
			global::UnityEngine.Vector3 a = this.previousMovementOrigin + global::UnityEngine.Vector3.ClampMagnitude(this.previousMovementDirection, this.speed * (global::UnityEngine.Time.time - this.previousMovementStartTime));
			return global::UnityEngine.Vector3.Lerp(a, vector3, this.switchPathInterpolationSpeed * (global::UnityEngine.Time.time - this.previousMovementStartTime));
		}
		return vector3;
	}

	public float repathRate = 0.5f;

	public global::UnityEngine.Transform target;

	public bool canSearch = true;

	public bool canMove = true;

	public float speed = 3f;

	public bool enableRotation = true;

	public bool rotationIn2D;

	public float rotationSpeed = 10f;

	public bool interpolatePathSwitches = true;

	public float switchPathInterpolationSpeed = 5f;

	protected global::Seeker seeker;

	protected global::UnityEngine.Transform tr;

	protected float lastRepath = -9999f;

	protected global::Pathfinding.ABPath path;

	protected int currentWaypointIndex;

	protected float distanceAlongSegment;

	protected bool canSearchAgain = true;

	protected global::UnityEngine.Vector3 previousMovementOrigin;

	protected global::UnityEngine.Vector3 previousMovementDirection;

	protected float previousMovementStartTime = -9999f;

	private bool startHasRun;
}
