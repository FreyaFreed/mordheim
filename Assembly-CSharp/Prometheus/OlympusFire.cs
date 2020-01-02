using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Prometheus
{
	public class OlympusFire : global::UnityEngine.MonoBehaviour
	{
		public void Spawn(global::UnitMenuController startCtrlr, global::UnitMenuController endCtrlr, global::UnityEngine.Vector3 startPos, global::UnityEngine.Vector3 endPos, global::UnityEngine.Events.UnityAction endAction)
		{
			this.unitCtrlr = startCtrlr;
			this.Init((!(startCtrlr != null) || !(startCtrlr.audioSource != null)) ? null : startCtrlr.audioSource);
			this.startBone.active = true;
			if (this.startBone.active && startCtrlr != null)
			{
				this.currentBoneFx = this.GetBone(startCtrlr.unit.BaseData.UnitRigId);
				global::UnityEngine.Transform transform = (this.currentBoneFx.bone == global::BoneId.NONE) ? startCtrlr.transform : startCtrlr.BonesTr[this.currentBoneFx.bone];
				if ((this.currentBoneFx.bone == global::BoneId.RIG_WEAPONR && this.unitCtrlr.Equipments[(int)this.unitCtrlr.unit.ActiveWeaponSlot] != null) || (this.currentBoneFx.bone == global::BoneId.RIG_WEAPONL && this.unitCtrlr.Equipments[(int)(this.unitCtrlr.unit.ActiveWeaponSlot + 1)] != null))
				{
					transform = ((this.currentBoneFx.bone != global::BoneId.RIG_WEAPONR) ? this.unitCtrlr.Equipments[(int)(this.unitCtrlr.unit.ActiveWeaponSlot + 1)].transform : this.unitCtrlr.Equipments[(int)this.unitCtrlr.unit.ActiveWeaponSlot].transform);
				}
				if (this.attached)
				{
					base.transform.SetParent(transform);
					base.transform.localPosition = this.currentBoneFx.offset;
					if (this.currentBoneFx.rotationWorldSpace)
					{
						base.transform.rotation = global::UnityEngine.Quaternion.Euler(this.currentBoneFx.rotation);
					}
					else
					{
						base.transform.localRotation = global::UnityEngine.Quaternion.Euler(this.currentBoneFx.rotation);
					}
				}
				else
				{
					base.transform.SetParent(null);
					base.transform.position = transform.position + startCtrlr.transform.rotation * this.currentBoneFx.offset;
					base.transform.rotation = startCtrlr.transform.rotation * global::UnityEngine.Quaternion.Euler(this.currentBoneFx.rotation);
				}
			}
			else
			{
				base.transform.SetParent(null);
				base.transform.position = startPos;
				base.transform.rotation = global::UnityEngine.Quaternion.identity;
			}
			if (this.endBone.active || endPos != global::UnityEngine.Vector3.zero)
			{
				this.endMoveAction = endAction;
				this.fxStartPos = base.transform.position;
				global::UnityEngine.Vector3 vector = (!this.endBone.active || !(endCtrlr != null)) ? endPos : (endCtrlr.BonesTr[this.endBone.bone].position + startCtrlr.transform.rotation * this.endBone.offset);
				this.SqrDistToTarget = global::UnityEngine.Vector3.SqrMagnitude(this.fxStartPos - vector);
				if (this.bezierMove)
				{
					this.moveCtrlr = base.gameObject.AddComponent<global::BezierMoveController>();
					((global::BezierMoveController)this.moveCtrlr).StartMoving(startPos, endPos, 3f, this.speed, endAction);
				}
				else
				{
					this.moveCtrlr = base.gameObject.AddComponent<global::MoveController>();
					this.moveCtrlr.StartMoving(vector - this.fxStartPos, this.speed);
				}
			}
			if (this.unitCtrlr is global::UnitController && ((global::UnitController)this.unitCtrlr).Imprint.State != global::MapImprintStateId.VISIBLE)
			{
				global::UnityEngine.Renderer[] componentsInChildren = base.GetComponentsInChildren<global::UnityEngine.Renderer>(true);
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					componentsInChildren[i].enabled = false;
				}
			}
		}

		public void Spawn(global::UnityEngine.Transform anchor, bool attached = true)
		{
			this.Init(null);
			if (attached)
			{
				base.transform.SetParent(anchor);
				base.transform.localPosition = global::UnityEngine.Vector3.zero;
				base.transform.localRotation = global::UnityEngine.Quaternion.identity;
			}
			else
			{
				base.transform.position = anchor.position;
				base.transform.rotation = anchor.rotation;
			}
		}

		public void Spawn(global::UnityEngine.Vector3 pos)
		{
			this.Init(null);
			base.transform.position = pos;
		}

		private void Init(global::UnityEngine.AudioSource source = null)
		{
			global::UnityEngine.ParticleSystem componentInChildren = base.GetComponentInChildren<global::UnityEngine.ParticleSystem>();
			this.duration = ((!(componentInChildren != null) || componentInChildren.duration <= 0f || componentInChildren.loop) ? 0f : componentInChildren.duration);
			this.timer = this.duration;
			this.particleExtraTimer = 0f;
			this.lightTimer = this.lightTime;
			this.lightFadeTimer = 0f;
			this.preventSound = (this.unitCtrlr != null && this.unitCtrlr is global::UnitController && ((global::UnitController)this.unitCtrlr).Imprint.State != global::MapImprintStateId.VISIBLE);
			if (this.delay == 0f)
			{
				this.PlaySound(this.soundStartName, source);
			}
		}

		public void Reactivate()
		{
			this.PlaySound(this.soundStartName, (!(this.unitCtrlr != null) || !(this.unitCtrlr.audioSource != null)) ? null : this.unitCtrlr.audioSource);
		}

		public void Stop()
		{
			if (this.audioSource != null)
			{
				this.audioSource.Stop();
			}
			if (this.particleExtraTime > 0f)
			{
				this.particleExtraTimer = this.particleExtraTime;
			}
			if (this.lightTime == 0f && this.lightExtraTime > 0f)
			{
				this.SetLightFadeTimer();
			}
			this.DestroyFx(false);
		}

		public void DestroyFx(bool force = false)
		{
			if ((this.duration != 0f || force) && this.particleExtraTimer <= 0f && this.lightTimer <= 0f && this.lightFadeTimer <= 0f)
			{
				global::PandoraDebug.LogInfo("Destroying Fx : " + base.name, "PROMETHEUS", null);
				global::UnityEngine.Object.Destroy(base.gameObject);
			}
		}

		private void Update()
		{
			if (this.destroyMe)
			{
				this.DestroyFx(true);
				return;
			}
			if (this.currentBoneFx != null && this.currentBoneFx.lockRotation)
			{
				if (this.currentBoneFx.rotationWorldSpace)
				{
					base.transform.rotation = global::UnityEngine.Quaternion.Euler(this.currentBoneFx.rotation);
				}
				else
				{
					base.transform.localRotation = global::UnityEngine.Quaternion.Euler(this.currentBoneFx.rotation);
				}
			}
			if (this.timer > 0f)
			{
				this.timer -= global::UnityEngine.Time.deltaTime;
				if (this.timer < 0f)
				{
					this.Stop();
				}
			}
			if (this.particleExtraTimer > 0f)
			{
				this.particleExtraTimer -= global::UnityEngine.Time.deltaTime;
				if (this.particleExtraTimer < 0f)
				{
					this.DestroyFx(false);
				}
			}
			if (this.lightTimer > 0f)
			{
				this.lightTimer -= global::UnityEngine.Time.deltaTime;
				if (this.lightTimer < 0f)
				{
					this.SetLightFadeTimer();
				}
			}
			if (this.lightFadeTimer > 0f)
			{
				if (this.light != null)
				{
					this.light.intensity = this.lightIntensity * (this.lightFadeTimer / this.lightExtraTime);
				}
				this.lightFadeTimer -= global::UnityEngine.Time.deltaTime;
				if (this.lightFadeTimer < 0f)
				{
					this.DestroyFx(false);
				}
			}
		}

		private void FixedUpdate()
		{
			if (this.moveCtrlr != null && !this.bezierMove && global::UnityEngine.Vector3.SqrMagnitude(this.moveCtrlr.transform.position - this.fxStartPos) > this.SqrDistToTarget)
			{
				this.Stop();
				if (this.endMoveAction != null)
				{
					this.endMoveAction();
				}
				global::UnityEngine.Object.Destroy(this.moveCtrlr);
				this.moveCtrlr = null;
			}
		}

		private void PlaySound(string soundName, global::UnityEngine.AudioSource source)
		{
			if (this.preventSound)
			{
				return;
			}
			if (string.IsNullOrEmpty(soundName))
			{
				return;
			}
			if (this.useUnitAudioSource && !this.soundLoop && source != null)
			{
				this.audioSource = source;
			}
			if (this.audioSource == null)
			{
				global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadResourceAsync<global::UnityEngine.GameObject>("prefabs/sound_base", delegate(global::UnityEngine.Object soundBasePrefab)
				{
					if (this == null)
					{
						return;
					}
					global::UnityEngine.GameObject gameObject = global::UnityEngine.Object.Instantiate<global::UnityEngine.GameObject>((global::UnityEngine.GameObject)soundBasePrefab);
					gameObject.transform.SetParent(this.transform);
					gameObject.transform.localPosition = global::UnityEngine.Vector3.zero;
					gameObject.transform.localRotation = global::UnityEngine.Quaternion.identity;
					this.audioSource = gameObject.GetComponent<global::UnityEngine.AudioSource>();
					this.PlaySound(soundName);
				}, false);
			}
			else
			{
				this.PlaySound(soundName);
			}
		}

		private void PlaySound(string soundName)
		{
			global::PandoraSingleton<global::Pan>.Instance.GetSound(soundName, true, new global::System.Action<global::UnityEngine.AudioClip>(this.PlaySound));
		}

		private void PlaySound(global::UnityEngine.AudioClip sound)
		{
			if (!this.soundLoop)
			{
				this.audioSource.PlayOneShot(sound);
			}
			else
			{
				this.audioSource.clip = sound;
				this.audioSource.loop = true;
				this.audioSource.Play();
			}
		}

		private void SetLightFadeTimer()
		{
			this.lightFadeTimer = ((!(this.light != null)) ? 0f : this.lightExtraTime);
			this.lightIntensity = ((!(this.light != null)) ? 0f : this.light.intensity);
		}

		public global::Prometheus.BoneFx GetBone(global::UnitRigId id)
		{
			for (int i = 0; i < this.rigSpecialCases.Count; i++)
			{
				if (this.rigSpecialCases[i].rig == id)
				{
					return this.rigSpecialCases[i].bone;
				}
			}
			return this.startBone;
		}

		public global::UnityEngine.GameObject AttachToUnit(global::UnityEngine.GameObject root)
		{
			global::UnitMenuController component = root.GetComponent<global::UnitMenuController>();
			base.transform.SetParent(component.BonesTr[this.startBone.bone]);
			base.transform.localPosition = global::UnityEngine.Vector3.zero;
			base.transform.localRotation = global::UnityEngine.Quaternion.identity;
			return base.gameObject;
		}

		public float delay;

		[global::UnityEngine.Serialization.FormerlySerializedAs("defaultBone")]
		public global::Prometheus.BoneFx startBone;

		public bool attached;

		public global::System.Collections.Generic.List<global::Prometheus.RigFx> rigSpecialCases;

		public global::Prometheus.BoneFx endBone;

		public float particleExtraTime;

		public string soundStartName;

		public bool soundLoop;

		public bool useUnitAudioSource;

		public string allyFxName;

		public string enemyFxName;

		public string mediumFxName;

		public string largeFxName;

		public bool weaponBased;

		public global::UnityEngine.Light light;

		public float lightTime;

		public float lightExtraTime;

		public float speed;

		[global::UnityEngine.HideInInspector]
		public bool destroyMe;

		public bool bezierMove;

		private float duration;

		private float timer;

		private float particleExtraTimer;

		private float lightTimer;

		private float lightFadeTimer;

		private float lightIntensity;

		private global::UnityEngine.AudioSource audioSource;

		private global::Prometheus.BoneFx currentBoneFx;

		private global::MoveController moveCtrlr;

		private global::UnityEngine.Events.UnityAction endMoveAction;

		private global::UnityEngine.Vector3 fxStartPos;

		private float SqrDistToTarget;

		private global::UnitMenuController unitCtrlr;

		private bool preventSound;
	}
}
