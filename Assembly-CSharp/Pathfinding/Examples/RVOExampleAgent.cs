using System;
using System.Collections.Generic;
using Pathfinding.RVO;
using UnityEngine;

namespace Pathfinding.Examples
{
	[global::UnityEngine.HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_examples_1_1_r_v_o_example_agent.php")]
	public class RVOExampleAgent : global::UnityEngine.MonoBehaviour
	{
		public void Awake()
		{
			this.seeker = base.GetComponent<global::Seeker>();
		}

		public void Start()
		{
			this.SetTarget(-base.transform.position);
			this.controller = base.GetComponent<global::Pathfinding.RVO.RVOController>();
		}

		public void SetTarget(global::UnityEngine.Vector3 target)
		{
			this.target = target;
			this.RecalculatePath();
		}

		public void SetColor(global::UnityEngine.Color col)
		{
			if (this.rends == null)
			{
				this.rends = base.GetComponentsInChildren<global::UnityEngine.MeshRenderer>();
			}
			foreach (global::UnityEngine.MeshRenderer meshRenderer in this.rends)
			{
				global::UnityEngine.Color color = meshRenderer.material.GetColor("_TintColor");
				global::UnityEngine.AnimationCurve curve = global::UnityEngine.AnimationCurve.Linear(0f, color.r, 1f, col.r);
				global::UnityEngine.AnimationCurve curve2 = global::UnityEngine.AnimationCurve.Linear(0f, color.g, 1f, col.g);
				global::UnityEngine.AnimationCurve curve3 = global::UnityEngine.AnimationCurve.Linear(0f, color.b, 1f, col.b);
				global::UnityEngine.AnimationClip animationClip = new global::UnityEngine.AnimationClip();
				animationClip.legacy = true;
				animationClip.SetCurve(string.Empty, typeof(global::UnityEngine.Material), "_TintColor.r", curve);
				animationClip.SetCurve(string.Empty, typeof(global::UnityEngine.Material), "_TintColor.g", curve2);
				animationClip.SetCurve(string.Empty, typeof(global::UnityEngine.Material), "_TintColor.b", curve3);
				global::UnityEngine.Animation animation = meshRenderer.gameObject.GetComponent<global::UnityEngine.Animation>();
				if (animation == null)
				{
					animation = meshRenderer.gameObject.AddComponent<global::UnityEngine.Animation>();
				}
				animationClip.wrapMode = global::UnityEngine.WrapMode.Once;
				animation.AddClip(animationClip, "ColorAnim");
				animation.Play("ColorAnim");
			}
		}

		public void RecalculatePath()
		{
			this.canSearchAgain = false;
			this.nextRepath = global::UnityEngine.Time.time + this.repathRate * (global::UnityEngine.Random.value + 0.5f);
			this.seeker.StartPath(base.transform.position, this.target, new global::Pathfinding.OnPathDelegate(this.OnPathComplete));
		}

		public void OnPathComplete(global::Pathfinding.Path _p)
		{
			global::Pathfinding.ABPath abpath = _p as global::Pathfinding.ABPath;
			this.canSearchAgain = true;
			if (this.path != null)
			{
				this.path.Release(this, false);
			}
			this.path = abpath;
			abpath.Claim(this);
			if (abpath.error)
			{
				this.wp = 0;
				this.vectorPath = null;
				return;
			}
			global::UnityEngine.Vector3 originalStartPoint = abpath.originalStartPoint;
			global::UnityEngine.Vector3 position = base.transform.position;
			originalStartPoint.y = position.y;
			float magnitude = (position - originalStartPoint).magnitude;
			this.wp = 0;
			this.vectorPath = abpath.vectorPath;
			for (float num = 0f; num <= magnitude; num += this.moveNextDist * 0.6f)
			{
				this.wp--;
				global::UnityEngine.Vector3 a = originalStartPoint + (position - originalStartPoint) * num;
				global::UnityEngine.Vector3 b;
				do
				{
					this.wp++;
					b = this.vectorPath[this.wp];
					b.y = a.y;
				}
				while ((a - b).sqrMagnitude < this.moveNextDist * this.moveNextDist && this.wp != this.vectorPath.Count - 1);
			}
		}

		public void Update()
		{
			if (global::UnityEngine.Time.time >= this.nextRepath && this.canSearchAgain)
			{
				this.RecalculatePath();
			}
			global::UnityEngine.Vector3 vector = base.transform.position;
			if (this.vectorPath != null && this.vectorPath.Count != 0)
			{
				global::UnityEngine.Vector3 vector2 = this.vectorPath[this.wp];
				vector2.y = vector.y;
				while ((vector - vector2).sqrMagnitude < this.moveNextDist * this.moveNextDist && this.wp != this.vectorPath.Count - 1)
				{
					this.wp++;
					vector2 = this.vectorPath[this.wp];
					vector2.y = vector.y;
				}
				if (this.wp == this.vectorPath.Count - 1)
				{
					float speed = global::UnityEngine.Mathf.Clamp01((vector2 - vector).magnitude / this.slowdownDistance) * this.maxSpeed;
					this.controller.SetTarget(vector2, speed, this.maxSpeed);
				}
				else
				{
					global::UnityEngine.Vector3 vector3 = vector2 - vector;
					this.controller.Move(vector3.normalized * this.maxSpeed);
				}
			}
			else
			{
				this.controller.SetTarget(vector, this.maxSpeed, this.maxSpeed);
			}
			global::UnityEngine.Vector3 vector4 = this.controller.CalculateMovementDelta(global::UnityEngine.Time.deltaTime);
			vector += vector4;
			if (global::UnityEngine.Time.deltaTime > 0f && vector4.magnitude / global::UnityEngine.Time.deltaTime > 0.01f)
			{
				global::UnityEngine.Quaternion rotation = base.transform.rotation;
				global::UnityEngine.Quaternion b = global::UnityEngine.Quaternion.LookRotation(vector4);
				base.transform.rotation = global::UnityEngine.Quaternion.Slerp(rotation, b, global::UnityEngine.Time.deltaTime * 5f);
			}
			global::UnityEngine.RaycastHit raycastHit;
			if (global::UnityEngine.Physics.Raycast(vector + global::UnityEngine.Vector3.up, global::UnityEngine.Vector3.down, out raycastHit, 2f, this.groundMask))
			{
				vector.y = raycastHit.point.y;
			}
			base.transform.position = vector;
		}

		public float repathRate = 1f;

		private float nextRepath;

		private global::UnityEngine.Vector3 target;

		private bool canSearchAgain = true;

		private global::Pathfinding.RVO.RVOController controller;

		public float maxSpeed = 10f;

		private global::Pathfinding.Path path;

		private global::System.Collections.Generic.List<global::UnityEngine.Vector3> vectorPath;

		private int wp;

		public float moveNextDist = 1f;

		public float slowdownDistance = 1f;

		public global::UnityEngine.LayerMask groundMask;

		private global::Seeker seeker;

		private global::UnityEngine.MeshRenderer[] rends;
	}
}
