using System;
using UnityEngine;

namespace Pathfinding.Examples
{
	[global::UnityEngine.RequireComponent(typeof(global::Seeker))]
	[global::UnityEngine.HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_examples_1_1_mine_bot_a_i.php")]
	public class MineBotAI : global::AIPath
	{
		public new void Start()
		{
			this.anim["forward"].layer = 10;
			this.anim.Play("awake");
			this.anim.Play("forward");
			this.anim["awake"].wrapMode = global::UnityEngine.WrapMode.Once;
			this.anim["awake"].speed = 0f;
			this.anim["awake"].normalizedTime = 1f;
			base.Start();
		}

		public override void OnTargetReached()
		{
			if (this.endOfPathEffect != null && global::UnityEngine.Vector3.Distance(this.tr.position, this.lastTarget) > 1f)
			{
				global::UnityEngine.Object.Instantiate(this.endOfPathEffect, this.tr.position, this.tr.rotation);
				this.lastTarget = this.tr.position;
			}
		}

		public override global::UnityEngine.Vector3 GetFeetPosition()
		{
			return this.tr.position;
		}

		protected new void Update()
		{
			global::UnityEngine.Vector3 direction;
			if (this.canMove)
			{
				global::UnityEngine.Vector3 vector = base.CalculateVelocity(this.GetFeetPosition());
				this.RotateTowards(this.targetDirection);
				vector.y = 0f;
				if (vector.sqrMagnitude <= this.sleepVelocity * this.sleepVelocity)
				{
					vector = global::UnityEngine.Vector3.zero;
				}
				if (this.rvoController != null)
				{
					this.rvoController.Move(vector);
					direction = this.rvoController.velocity;
				}
				else if (this.controller != null)
				{
					this.controller.SimpleMove(vector);
					direction = this.controller.velocity;
				}
				else
				{
					global::UnityEngine.Debug.LogWarning("No NavmeshController or CharacterController attached to GameObject");
					direction = global::UnityEngine.Vector3.zero;
				}
			}
			else
			{
				direction = global::UnityEngine.Vector3.zero;
			}
			global::UnityEngine.Vector3 vector2 = this.tr.InverseTransformDirection(direction);
			vector2.y = 0f;
			if (direction.sqrMagnitude <= this.sleepVelocity * this.sleepVelocity)
			{
				this.anim.Blend("forward", 0f, 0.2f);
			}
			else
			{
				this.anim.Blend("forward", 1f, 0.2f);
				global::UnityEngine.AnimationState animationState = this.anim["forward"];
				float z = vector2.z;
				animationState.speed = z * this.animationSpeed;
			}
		}

		public global::UnityEngine.Animation anim;

		public float sleepVelocity = 0.4f;

		public float animationSpeed = 0.2f;

		public global::UnityEngine.GameObject endOfPathEffect;

		protected global::UnityEngine.Vector3 lastTarget;
	}
}
