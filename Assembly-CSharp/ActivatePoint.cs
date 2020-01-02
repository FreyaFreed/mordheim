using System;
using Pathfinding;
using UnityEngine;

public class ActivatePoint : global::InteractivePoint
{
	public override void Init(uint id)
	{
		base.Init(id);
		this.animations = base.GetComponentsInChildren<global::UnityEngine.Animation>(true);
		for (int i = 0; i < this.animations.Length; i++)
		{
			this.animations[i].Stop();
		}
		this.cutter = base.GetComponentInChildren<global::Pathfinding.NavmeshCut>();
		this.occupation = base.GetComponentInChildren<global::OccupationChecker>();
		if (this.alternateVisual != null)
		{
			this.alternateDissolver = this.alternateVisual.AddComponent<global::Dissolver>();
			this.alternateDissolver.dissolveSpeed = this.apparitionDelay;
			this.alternateDissolver.Hide(true, true, null);
		}
		this.RefreshAnims(true);
	}

	public virtual void Activate(global::UnitController unitCtrlr, bool force = false)
	{
		this.activated = !this.activated;
		global::PandoraSingleton<global::MissionManager>.Instance.MissionEndData.UpdateActivated(this.guid, this.activated);
		if (!this.reverse)
		{
			if (this.visual != null && this.alternateVisual != null)
			{
				this.visualDissolver.Hide(true, false, null);
				this.alternateDissolver.Hide(false, false, null);
			}
			if (this.destroy)
			{
				base.DestroyVisual(this.destroyOnlyTriggers, false);
			}
			else
			{
				this.SetTriggerVisual();
			}
		}
		base.SpawnFxs(this.activated);
		if (this.cutter != null)
		{
			this.cutter.ForceUpdate();
		}
		this.RefreshAnims(force);
	}

	public bool IsAnimationPlaying()
	{
		if (this.animations != null)
		{
			for (int i = 0; i < this.animations.Length; i++)
			{
				if (this.animations[i] != null && this.animations[i].isPlaying)
				{
					return true;
				}
			}
		}
		return false;
	}

	public override void SetTriggerVisual()
	{
		base.SetTriggerVisual(!this.activated);
	}

	public void RefreshAnims(bool force = false)
	{
		for (int i = 0; i < this.animations.Length; i++)
		{
			global::UnityEngine.Animation animation = this.animations[i];
			if (animation && animation.clip != null)
			{
				animation.Stop();
				animation.wrapMode = global::UnityEngine.WrapMode.Default;
				animation.cullingType = global::UnityEngine.AnimationCullingType.AlwaysAnimate;
				float num = (float)((!this.activated) ? 1 : 0);
				num = ((!this.mirrorAnim) ? num : ((num + 1f) % 2f));
				num = ((!force) ? num : ((num + 1f) % 2f));
				float num2 = (float)((!this.activated) ? -1 : 1);
				num2 *= (float)((!this.mirrorAnim) ? 1 : -1);
				animation[animation.clip.name].normalizedTime = num;
				animation[animation.clip.name].speed = num2;
				animation.Play(animation.clip.name);
			}
		}
	}

	protected override bool LinkValid(global::UnitController unitCtrlr, bool reverseCondition)
	{
		return this.reverse || this.activated;
	}

	protected override bool CanInteract(global::UnitController unitCtrlr)
	{
		return (this.occupation == null || this.occupation.Occupation == 0) && (this.reverse || !this.activated) && base.CanInteract(unitCtrlr) && !unitCtrlr.unit.BothArmsMutated() && unitCtrlr.unit.Data.UnitSizeId != global::UnitSizeId.LARGE && !this.IsAnimationPlaying();
	}

	public global::UnityEngine.GameObject alternateVisual;

	private global::Dissolver alternateDissolver;

	public bool reverse;

	public bool activated;

	public bool mirrorAnim;

	public bool destroy;

	public bool destroyOnlyTriggers;

	public bool consumeRequestedItem;

	private global::OccupationChecker occupation;

	private global::UnityEngine.Animation[] animations;

	private global::Pathfinding.NavmeshCut cutter;
}
