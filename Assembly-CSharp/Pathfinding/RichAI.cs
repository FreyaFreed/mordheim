using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding.RVO;
using UnityEngine;

namespace Pathfinding
{
	[global::UnityEngine.HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_rich_a_i.php")]
	[global::UnityEngine.RequireComponent(typeof(global::Seeker))]
	[global::UnityEngine.AddComponentMenu("Pathfinding/AI/RichAI (3D, for navmesh)")]
	public class RichAI : global::UnityEngine.MonoBehaviour
	{
		public global::UnityEngine.Vector3 Velocity
		{
			get
			{
				return this.velocity;
			}
		}

		private void Awake()
		{
			this.seeker = base.GetComponent<global::Seeker>();
			this.controller = base.GetComponent<global::UnityEngine.CharacterController>();
			this.rvoController = base.GetComponent<global::Pathfinding.RVO.RVOController>();
			this.tr = base.transform;
		}

		protected virtual void Start()
		{
			this.startHasRun = true;
			this.OnEnable();
		}

		protected virtual void OnEnable()
		{
			this.lastRepath = -9999f;
			this.waitingForPathCalc = false;
			this.canSearchPath = true;
			if (this.startHasRun)
			{
				global::Seeker seeker = this.seeker;
				seeker.pathCallback = (global::Pathfinding.OnPathDelegate)global::System.Delegate.Combine(seeker.pathCallback, new global::Pathfinding.OnPathDelegate(this.OnPathComplete));
				base.StartCoroutine(this.SearchPaths());
			}
		}

		public void OnDisable()
		{
			if (this.seeker != null && !this.seeker.IsDone())
			{
				this.seeker.GetCurrentPath().Error();
			}
			global::Seeker seeker = this.seeker;
			seeker.pathCallback = (global::Pathfinding.OnPathDelegate)global::System.Delegate.Remove(seeker.pathCallback, new global::Pathfinding.OnPathDelegate(this.OnPathComplete));
		}

		public virtual void UpdatePath()
		{
			this.canSearchPath = true;
			this.waitingForPathCalc = false;
			global::Pathfinding.Path currentPath = this.seeker.GetCurrentPath();
			if (currentPath != null && !this.seeker.IsDone())
			{
				currentPath.Error();
				currentPath.Claim(this);
				currentPath.Release(this, false);
			}
			this.waitingForPathCalc = true;
			this.lastRepath = global::UnityEngine.Time.time;
			this.seeker.StartPath(this.tr.position, this.target.position);
		}

		private global::System.Collections.IEnumerator SearchPaths()
		{
			for (;;)
			{
				while (!this.repeatedlySearchPaths || this.waitingForPathCalc || !this.canSearchPath || global::UnityEngine.Time.time - this.lastRepath < this.repathRate)
				{
					yield return null;
				}
				this.UpdatePath();
				yield return null;
			}
			yield break;
		}

		private void OnPathComplete(global::Pathfinding.Path p)
		{
			this.waitingForPathCalc = false;
			p.Claim(this);
			if (p.error)
			{
				p.Release(this, false);
				return;
			}
			if (this.traversingSpecialPath)
			{
				this.delayUpdatePath = true;
			}
			else
			{
				if (this.rp == null)
				{
					this.rp = new global::Pathfinding.RichPath();
				}
				this.rp.Initialize(this.seeker, p, true, this.funnelSimplification);
			}
			p.Release(this, false);
		}

		public bool TraversingSpecial
		{
			get
			{
				return this.traversingSpecialPath;
			}
		}

		public global::UnityEngine.Vector3 TargetPoint
		{
			get
			{
				return this.lastTargetPoint;
			}
		}

		public bool ApproachingPartEndpoint
		{
			get
			{
				return this.lastCorner;
			}
		}

		public bool ApproachingPathEndpoint
		{
			get
			{
				return this.rp != null && this.ApproachingPartEndpoint && !this.rp.PartsLeft();
			}
		}

		public float DistanceToNextWaypoint
		{
			get
			{
				return this.distanceToWaypoint;
			}
		}

		private void NextPart()
		{
			this.rp.NextPart();
			this.lastCorner = false;
			if (!this.rp.PartsLeft())
			{
				this.OnTargetReached();
			}
		}

		protected virtual void OnTargetReached()
		{
		}

		protected virtual global::UnityEngine.Vector3 UpdateTarget(global::Pathfinding.RichFunnel fn)
		{
			this.nextCorners.Clear();
			global::UnityEngine.Vector3 vector = this.tr.position;
			bool flag;
			vector = fn.Update(vector, this.nextCorners, 2, out this.lastCorner, out flag);
			if (flag && !this.waitingForPathCalc)
			{
				this.UpdatePath();
			}
			return vector;
		}

		private static global::UnityEngine.Vector2 To2D(global::UnityEngine.Vector3 v)
		{
			return new global::UnityEngine.Vector2(v.x, v.z);
		}

		protected virtual void Update()
		{
			global::Pathfinding.RichAI.deltaTime = global::UnityEngine.Mathf.Min(global::UnityEngine.Time.smoothDeltaTime * 2f, global::UnityEngine.Time.deltaTime);
			if (this.rp != null)
			{
				global::Pathfinding.RichPathPart currentPart = this.rp.GetCurrentPart();
				global::Pathfinding.RichFunnel richFunnel = currentPart as global::Pathfinding.RichFunnel;
				if (richFunnel != null)
				{
					global::UnityEngine.Vector3 vector = this.UpdateTarget(richFunnel);
					if (global::UnityEngine.Time.frameCount % 5 == 0 && this.wallForce > 0f && this.wallDist > 0f)
					{
						this.wallBuffer.Clear();
						richFunnel.FindWalls(this.wallBuffer, this.wallDist);
					}
					int num = 0;
					global::UnityEngine.Vector3 vector2 = this.nextCorners[num];
					global::UnityEngine.Vector3 vector3 = vector2 - vector;
					vector3.y = 0f;
					bool flag = global::UnityEngine.Vector3.Dot(vector3, this.currentTargetDirection) < 0f;
					if (flag && this.nextCorners.Count - num > 1)
					{
						num++;
						vector2 = this.nextCorners[num];
					}
					if (vector2 != this.lastTargetPoint)
					{
						this.currentTargetDirection = vector2 - vector;
						this.currentTargetDirection.y = 0f;
						this.currentTargetDirection.Normalize();
						this.lastTargetPoint = vector2;
					}
					vector3 = vector2 - vector;
					vector3.y = 0f;
					global::UnityEngine.Vector3 vector4 = global::Pathfinding.VectorMath.Normalize(vector3, out this.distanceToWaypoint);
					bool flag2 = this.lastCorner && this.nextCorners.Count - num == 1;
					if (flag2 && this.distanceToWaypoint < 0.01f * this.maxSpeed)
					{
						this.velocity = (vector2 - vector) * 100f;
					}
					else
					{
						global::UnityEngine.Vector3 a = this.CalculateWallForce(vector, vector4);
						global::UnityEngine.Vector2 vector5;
						if (flag2)
						{
							vector5 = this.CalculateAccelerationToReachPoint(global::Pathfinding.RichAI.To2D(vector2 - vector), global::UnityEngine.Vector2.zero, global::Pathfinding.RichAI.To2D(this.velocity));
							a *= global::System.Math.Min(this.distanceToWaypoint / 0.5f, 1f);
							if (this.distanceToWaypoint < this.endReachedDistance)
							{
								this.NextPart();
							}
						}
						else
						{
							global::UnityEngine.Vector3 a2 = (num >= this.nextCorners.Count - 1) ? ((vector2 - vector) * 2f + vector) : this.nextCorners[num + 1];
							global::UnityEngine.Vector3 v = (a2 - vector2).normalized * this.maxSpeed;
							vector5 = this.CalculateAccelerationToReachPoint(global::Pathfinding.RichAI.To2D(vector2 - vector), global::Pathfinding.RichAI.To2D(v), global::Pathfinding.RichAI.To2D(this.velocity));
						}
						this.velocity += (new global::UnityEngine.Vector3(vector5.x, 0f, vector5.y) + a * this.wallForce) * global::Pathfinding.RichAI.deltaTime;
					}
					global::Pathfinding.TriangleMeshNode currentNode = richFunnel.CurrentNode;
					global::UnityEngine.Vector3 b;
					if (currentNode != null)
					{
						b = currentNode.ClosestPointOnNode(vector);
					}
					else
					{
						b = vector;
					}
					float magnitude = (richFunnel.exactEnd - b).magnitude;
					float num2 = this.maxSpeed;
					num2 *= global::UnityEngine.Mathf.Sqrt(global::UnityEngine.Mathf.Min(1f, magnitude / (this.maxSpeed * this.slowdownTime)));
					if (this.slowWhenNotFacingTarget)
					{
						float num3 = global::UnityEngine.Mathf.Max((global::UnityEngine.Vector3.Dot(vector4, this.tr.forward) + 0.5f) / 1.5f, 0.2f);
						num2 *= num3;
						float num4 = global::Pathfinding.VectorMath.MagnitudeXZ(this.velocity);
						float y = this.velocity.y;
						this.velocity.y = 0f;
						num4 = global::UnityEngine.Mathf.Min(num4, num2);
						this.velocity = global::UnityEngine.Vector3.Lerp(this.velocity.normalized * num4, this.tr.forward * num4, global::UnityEngine.Mathf.Clamp((!flag2) ? 1f : (this.distanceToWaypoint * 2f), 0f, 0.5f));
						this.velocity.y = y;
					}
					else
					{
						this.velocity = global::Pathfinding.VectorMath.ClampMagnitudeXZ(this.velocity, num2);
					}
					this.velocity += global::Pathfinding.RichAI.deltaTime * this.gravity;
					if (this.rvoController != null && this.rvoController.enabled)
					{
						global::UnityEngine.Vector3 pos = vector + global::Pathfinding.VectorMath.ClampMagnitudeXZ(this.velocity, magnitude);
						this.rvoController.SetTarget(pos, global::Pathfinding.VectorMath.MagnitudeXZ(this.velocity), this.maxSpeed);
					}
					global::UnityEngine.Vector3 vector6;
					if (this.rvoController != null && this.rvoController.enabled)
					{
						vector6 = this.rvoController.CalculateMovementDelta(vector, global::Pathfinding.RichAI.deltaTime);
						vector6.y = this.velocity.y * global::Pathfinding.RichAI.deltaTime;
					}
					else
					{
						vector6 = this.velocity * global::Pathfinding.RichAI.deltaTime;
					}
					if (flag2)
					{
						global::UnityEngine.Vector3 trotdir = global::UnityEngine.Vector3.Lerp(vector6.normalized, this.currentTargetDirection, global::System.Math.Max(1f - this.distanceToWaypoint * 2f, 0f));
						this.RotateTowards(trotdir);
					}
					else
					{
						this.RotateTowards(vector6);
					}
					if (this.controller != null && this.controller.enabled)
					{
						this.tr.position = vector;
						this.controller.Move(vector6);
						vector = this.tr.position;
					}
					else
					{
						float y2 = vector.y;
						vector += vector6;
						vector = this.RaycastPosition(vector, y2);
					}
					global::UnityEngine.Vector3 vector7 = richFunnel.ClampToNavmesh(vector);
					if (vector != vector7)
					{
						global::UnityEngine.Vector3 vector8 = vector7 - vector;
						this.velocity -= vector8 * global::UnityEngine.Vector3.Dot(vector8, this.velocity) / vector8.sqrMagnitude;
						if (this.rvoController != null && this.rvoController.enabled)
						{
							this.rvoController.SetCollisionNormal(vector8);
						}
					}
					this.tr.position = vector7;
				}
				else if (this.rvoController != null && this.rvoController.enabled)
				{
					this.rvoController.Move(global::UnityEngine.Vector3.zero);
				}
				if (currentPart is global::Pathfinding.RichSpecial && !this.traversingSpecialPath)
				{
					base.StartCoroutine(this.TraverseSpecial(currentPart as global::Pathfinding.RichSpecial));
				}
			}
			else if (this.rvoController != null && this.rvoController.enabled)
			{
				this.rvoController.Move(global::UnityEngine.Vector3.zero);
			}
			else if (!(this.controller != null) || !this.controller.enabled)
			{
				this.tr.position = this.RaycastPosition(this.tr.position, this.tr.position.y);
			}
		}

		private global::UnityEngine.Vector2 CalculateAccelerationToReachPoint(global::UnityEngine.Vector2 deltaPosition, global::UnityEngine.Vector2 targetVelocity, global::UnityEngine.Vector2 currentVelocity)
		{
			if (targetVelocity == global::UnityEngine.Vector2.zero)
			{
				float num = 0.05f;
				float num2 = 10f;
				while (num2 - num > 0.01f)
				{
					float num3 = (num2 + num) * 0.5f;
					global::UnityEngine.Vector2 a = (6f * deltaPosition - 4f * num3 * currentVelocity) / (num3 * num3);
					global::UnityEngine.Vector2 a2 = 6f * (num3 * currentVelocity - 2f * deltaPosition) / (num3 * num3 * num3);
					if (a.sqrMagnitude > this.acceleration * this.acceleration || (a + a2 * num3).sqrMagnitude > this.acceleration * this.acceleration)
					{
						num = num3;
					}
					else
					{
						num2 = num3;
					}
				}
				return (6f * deltaPosition - 4f * num2 * currentVelocity) / (num2 * num2);
			}
			float magnitude = deltaPosition.magnitude;
			float magnitude2 = currentVelocity.magnitude;
			float num4;
			global::UnityEngine.Vector2 a3 = global::Pathfinding.VectorMath.Normalize(targetVelocity, out num4);
			return (deltaPosition - a3 * global::System.Math.Min(0.5f * magnitude * num4 / (magnitude2 + num4), this.maxSpeed * 2f)).normalized * this.acceleration;
		}

		private global::UnityEngine.Vector3 CalculateWallForce(global::UnityEngine.Vector3 position, global::UnityEngine.Vector3 directionToTarget)
		{
			if (this.wallForce > 0f && this.wallDist > 0f)
			{
				float num = 0f;
				float num2 = 0f;
				for (int i = 0; i < this.wallBuffer.Count; i += 2)
				{
					global::UnityEngine.Vector3 a = global::Pathfinding.VectorMath.ClosestPointOnSegment(this.wallBuffer[i], this.wallBuffer[i + 1], this.tr.position);
					float sqrMagnitude = (a - position).sqrMagnitude;
					if (sqrMagnitude <= this.wallDist * this.wallDist)
					{
						global::UnityEngine.Vector3 normalized = (this.wallBuffer[i + 1] - this.wallBuffer[i]).normalized;
						float num3 = global::UnityEngine.Vector3.Dot(directionToTarget, normalized) * (1f - global::System.Math.Max(0f, 2f * (sqrMagnitude / (this.wallDist * this.wallDist)) - 1f));
						if (num3 > 0f)
						{
							num2 = global::System.Math.Max(num2, num3);
						}
						else
						{
							num = global::System.Math.Max(num, -num3);
						}
					}
				}
				global::UnityEngine.Vector3 a2 = global::UnityEngine.Vector3.Cross(global::UnityEngine.Vector3.up, directionToTarget);
				return a2 * (num2 - num);
			}
			return global::UnityEngine.Vector3.zero;
		}

		private global::UnityEngine.Vector3 RaycastPosition(global::UnityEngine.Vector3 position, float lasty)
		{
			if (this.raycastingForGroundPlacement)
			{
				float num = global::UnityEngine.Mathf.Max(this.centerOffset, lasty - position.y + this.centerOffset);
				global::UnityEngine.RaycastHit raycastHit;
				if (global::UnityEngine.Physics.Raycast(position + global::UnityEngine.Vector3.up * num, global::UnityEngine.Vector3.down, out raycastHit, num, this.groundMask) && raycastHit.distance < num)
				{
					position = raycastHit.point;
					this.velocity.y = 0f;
				}
			}
			return position;
		}

		private bool RotateTowards(global::UnityEngine.Vector3 trotdir)
		{
			trotdir.y = 0f;
			if (trotdir != global::UnityEngine.Vector3.zero)
			{
				global::UnityEngine.Quaternion rotation = this.tr.rotation;
				global::UnityEngine.Vector3 eulerAngles = global::UnityEngine.Quaternion.LookRotation(trotdir).eulerAngles;
				global::UnityEngine.Vector3 eulerAngles2 = rotation.eulerAngles;
				eulerAngles2.y = global::UnityEngine.Mathf.MoveTowardsAngle(eulerAngles2.y, eulerAngles.y, this.rotationSpeed * global::Pathfinding.RichAI.deltaTime);
				this.tr.rotation = global::UnityEngine.Quaternion.Euler(eulerAngles2);
				return global::UnityEngine.Mathf.Abs(eulerAngles2.y - eulerAngles.y) < 5f;
			}
			return false;
		}

		public void OnDrawGizmos()
		{
			if (this.drawGizmos)
			{
				if (this.raycastingForGroundPlacement)
				{
					global::UnityEngine.Gizmos.color = global::Pathfinding.RichAI.GizmoColorRaycast;
					global::UnityEngine.Gizmos.DrawLine(base.transform.position, base.transform.position + global::UnityEngine.Vector3.up * this.centerOffset);
					global::UnityEngine.Gizmos.DrawLine(base.transform.position + global::UnityEngine.Vector3.left * 0.1f, base.transform.position + global::UnityEngine.Vector3.right * 0.1f);
					global::UnityEngine.Gizmos.DrawLine(base.transform.position + global::UnityEngine.Vector3.back * 0.1f, base.transform.position + global::UnityEngine.Vector3.forward * 0.1f);
				}
				if (this.tr != null && this.nextCorners != null)
				{
					global::UnityEngine.Gizmos.color = global::Pathfinding.RichAI.GizmoColorPath;
					global::UnityEngine.Vector3 from = this.tr.position;
					for (int i = 0; i < this.nextCorners.Count; i++)
					{
						global::UnityEngine.Gizmos.DrawLine(from, this.nextCorners[i]);
						from = this.nextCorners[i];
					}
				}
			}
		}

		private global::System.Collections.IEnumerator TraverseSpecial(global::Pathfinding.RichSpecial rs)
		{
			this.traversingSpecialPath = true;
			this.velocity = global::UnityEngine.Vector3.zero;
			global::Pathfinding.AnimationLink al = rs.nodeLink as global::Pathfinding.AnimationLink;
			if (al == null)
			{
				global::UnityEngine.Debug.LogError("Unhandled RichSpecial");
				yield break;
			}
			while (!this.RotateTowards(rs.first.forward))
			{
				yield return null;
			}
			this.tr.parent.position = this.tr.position;
			this.tr.parent.rotation = this.tr.rotation;
			this.tr.localPosition = global::UnityEngine.Vector3.zero;
			this.tr.localRotation = global::UnityEngine.Quaternion.identity;
			if (rs.reverse && al.reverseAnim)
			{
				this.anim[al.clip].speed = -al.animSpeed;
				this.anim[al.clip].normalizedTime = 1f;
				this.anim.Play(al.clip);
				this.anim.Sample();
			}
			else
			{
				this.anim[al.clip].speed = al.animSpeed;
				this.anim.Rewind(al.clip);
				this.anim.Play(al.clip);
			}
			this.tr.parent.position -= this.tr.position - this.tr.parent.position;
			yield return new global::UnityEngine.WaitForSeconds(global::UnityEngine.Mathf.Abs(this.anim[al.clip].length / al.animSpeed));
			this.traversingSpecialPath = false;
			this.NextPart();
			if (this.delayUpdatePath)
			{
				this.delayUpdatePath = false;
				this.UpdatePath();
			}
			yield break;
		}

		public global::UnityEngine.Transform target;

		public bool drawGizmos = true;

		public bool repeatedlySearchPaths;

		public float repathRate = 0.5f;

		public float maxSpeed = 1f;

		public float acceleration = 5f;

		public float slowdownTime = 0.5f;

		public float rotationSpeed = 360f;

		public float endReachedDistance = 0.01f;

		public float wallForce = 3f;

		public float wallDist = 1f;

		public global::UnityEngine.Vector3 gravity = new global::UnityEngine.Vector3(0f, -9.82f, 0f);

		public bool raycastingForGroundPlacement = true;

		public global::UnityEngine.LayerMask groundMask = -1;

		public float centerOffset = 1f;

		public global::Pathfinding.RichFunnel.FunnelSimplification funnelSimplification;

		public global::UnityEngine.Animation anim;

		public bool preciseSlowdown = true;

		public bool slowWhenNotFacingTarget = true;

		private global::UnityEngine.Vector3 velocity;

		protected global::Pathfinding.RichPath rp;

		protected global::Seeker seeker;

		protected global::UnityEngine.Transform tr;

		private global::UnityEngine.CharacterController controller;

		private global::Pathfinding.RVO.RVOController rvoController;

		private global::UnityEngine.Vector3 lastTargetPoint;

		private global::UnityEngine.Vector3 currentTargetDirection;

		protected bool waitingForPathCalc;

		protected bool canSearchPath;

		protected bool delayUpdatePath;

		protected bool traversingSpecialPath;

		protected bool lastCorner;

		private float distanceToWaypoint = 999f;

		protected global::System.Collections.Generic.List<global::UnityEngine.Vector3> nextCorners = new global::System.Collections.Generic.List<global::UnityEngine.Vector3>();

		protected global::System.Collections.Generic.List<global::UnityEngine.Vector3> wallBuffer = new global::System.Collections.Generic.List<global::UnityEngine.Vector3>();

		private bool startHasRun;

		protected float lastRepath = -9999f;

		private static float deltaTime;

		public static readonly global::UnityEngine.Color GizmoColorRaycast = new global::UnityEngine.Color(0.4627451f, 0.807843149f, 0.4392157f);

		public static readonly global::UnityEngine.Color GizmoColorPath = new global::UnityEngine.Color(0.03137255f, 0.305882365f, 0.7607843f);
	}
}
