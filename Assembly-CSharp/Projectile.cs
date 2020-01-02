using System;
using System.Collections.Generic;
using Prometheus;
using UnityEngine;

[global::UnityEngine.RequireComponent(typeof(global::MoveController))]
public class Projectile : global::UnityEngine.MonoBehaviour
{
	private void Awake()
	{
		this.moveCtrlr = base.GetComponent<global::MoveController>();
		this.trails = new global::System.Collections.Generic.List<global::UnityEngine.TrailRenderer>(base.GetComponentsInChildren<global::UnityEngine.TrailRenderer>());
		this.ActivateTrails(false);
		this.audioSource = base.GetComponent<global::UnityEngine.AudioSource>();
	}

	private void FixedUpdate()
	{
		if (!this.launched)
		{
			return;
		}
		float num = global::UnityEngine.Vector3.SqrMagnitude(base.transform.position - this.startPos);
		base.transform.LookAt(base.transform.position + (base.transform.position - this.previousPos));
		this.previousPos = base.transform.position;
		if (!this.missSoundPlayed && num > (this.maxDistance - 2f) * (this.maxDistance - 2f))
		{
			if (this.noHitCollision)
			{
				this.missSoundPlayed = true;
				if (this.missSound != null && this.audioSource != null)
				{
					this.audioSource.clip = this.missSound;
					this.audioSource.Play();
				}
			}
			if (this.attackResult == global::AttackResultId.MISS)
			{
				this.DisplayFlyingText();
			}
		}
		if (num >= this.maxDistance * this.maxDistance)
		{
			if (this.noHitCollision)
			{
				global::UnityEngine.Object.Destroy(base.gameObject);
				return;
			}
			if (this.attackResult >= global::AttackResultId.HIT_NO_WOUND)
			{
				if (this.target is global::UnitController)
				{
					global::UnitController unitController = (global::UnitController)this.target;
					base.transform.SetParent(this.bodyPart);
					base.transform.localPosition = global::UnityEngine.Vector3.zero;
					if (this.randPenetration)
					{
						base.transform.position += base.transform.forward * (float)global::PandoraSingleton<global::GameManager>.Instance.LocalTyche.Rand(0.10000000149011612, 0.5);
					}
					global::UnityEngine.Quaternion identity = global::UnityEngine.Quaternion.identity;
					identity.SetLookRotation(base.transform.rotation.eulerAngles, base.transform.forward * -1f);
					unitController.SetHitData(this.bodyPart, identity);
					if (!this.isSecondary)
					{
						global::UnityEngine.Vector3 forward = base.transform.forward;
						forward.y = 0f;
						global::UnityEngine.Vector3 forward2 = this.target.transform.forward;
						forward2.y = 0f;
						int num2 = (unitController.unit.Status != global::UnitStateId.OUT_OF_ACTION) ? 1 : 0;
						num2 += ((global::UnityEngine.Vector3.Angle(forward, forward2) <= 90f) ? 1 : 0);
						unitController.PlayDefState(this.attackResult, num2, unitController.unit.Status);
						if (this.attackerCtrlr.CurrentAction.fxData != null && !string.IsNullOrEmpty(this.attackerCtrlr.CurrentAction.fxData.ImpactFx))
						{
							global::PandoraSingleton<global::Prometheus.Prometheus>.Instance.SpawnFx(this.attackerCtrlr.CurrentAction.fxData.ImpactFx, unitController, null, null, default(global::UnityEngine.Vector3), default(global::UnityEngine.Vector3), null);
						}
					}
				}
				else if (this.target is global::Destructible)
				{
					((global::Destructible)this.target).projectiles.Add(this);
					if (!this.isSecondary)
					{
						((global::Destructible)this.target).Hit(this.attackerCtrlr);
					}
				}
			}
			else
			{
				if (!string.IsNullOrEmpty(this.fxHitObstacle))
				{
					global::PandoraSingleton<global::Prometheus.Prometheus>.Instance.SpawnFx(this.fxHitObstacle, base.transform.position);
				}
				if (this.hitFailSound != null && this.audioSource != null)
				{
					this.audioSource.clip = this.hitFailSound;
					this.audioSource.Play();
				}
				this.destroyOnArrival = true;
			}
			this.moveCtrlr.StopMoving();
			this.ActivateTrails(false);
			base.enabled = false;
			if (this.destroyOnArrival)
			{
				global::UnityEngine.Object.Destroy(base.gameObject);
			}
		}
	}

	private void DisplayFlyingText()
	{
		global::UnitController defenderCtrlr = (global::UnitController)this.target;
		global::PandoraSingleton<global::FlyingTextManager>.Instance.GetFlyingText(global::FlyingTextId.ACTION, delegate(global::FlyingText fl)
		{
			((global::FlyingLabel)fl).Play(defenderCtrlr.BonesTr[global::BoneId.RIG_HEAD].position, false, defenderCtrlr.flyingLabel, new string[0]);
		});
	}

	public void Launch(global::UnityEngine.Vector3 pos, global::UnitController attacker, global::UnityEngine.MonoBehaviour target, bool noCollision, global::UnityEngine.Transform part, bool secondary)
	{
		this.launched = true;
		this.missSoundPlayed = false;
		this.attackerCtrlr = attacker;
		this.target = target;
		if (target is global::UnitController)
		{
			this.attackResult = ((global::UnitController)target).attackResultId;
		}
		else if (target is global::Destructible)
		{
			this.attackResult = global::AttackResultId.HIT;
		}
		this.noHitCollision = noCollision;
		this.bodyPart = part;
		this.isSecondary = secondary;
		base.transform.SetParent(null);
		this.startPos = base.transform.position;
		this.previousPos = base.transform.position;
		if (noCollision)
		{
			pos += global::UnityEngine.Vector3.Normalize(pos - this.startPos) * 2f;
		}
		this.targetPos = pos;
		this.maxDistance = global::UnityEngine.Vector3.Distance(this.targetPos, this.startPos);
		base.transform.LookAt(this.targetPos);
		this.moveCtrlr.StartMoving(base.transform.forward, this.speed);
		this.ActivateTrails(true);
	}

	private void ActivateTrails(bool active)
	{
		if (this.trails != null && this.trails.Count > 0)
		{
			for (int i = 0; i < this.trails.Count; i++)
			{
				global::UnityEngine.TrailRenderer trailRenderer = this.trails[i];
				trailRenderer.gameObject.SetActive(active);
			}
		}
	}

	public float speed = 10f;

	public global::UnityEngine.AudioClip hitFailSound;

	public global::UnityEngine.AudioClip missSound;

	public bool destroyOnArrival;

	public string fxHitObstacle;

	public bool randPenetration;

	private bool launched;

	private global::UnityEngine.Vector3 startPos;

	private global::UnityEngine.Vector3 previousPos;

	private float maxDistance;

	private global::UnityEngine.Vector3 targetPos;

	private global::AttackResultId attackResult;

	private global::UnitController attackerCtrlr;

	private global::UnityEngine.MonoBehaviour target;

	private bool missSoundPlayed;

	private bool noHitCollision;

	private bool isSecondary;

	private global::UnityEngine.Transform bodyPart;

	private global::MoveController moveCtrlr;

	private global::System.Collections.Generic.List<global::UnityEngine.TrailRenderer> trails;

	private global::UnityEngine.AudioSource audioSource;

	private enum CamState
	{
		NONE,
		LOOKTARGET,
		ANIMATE
	}
}
