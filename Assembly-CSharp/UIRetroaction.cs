using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class UIRetroaction : global::UnityEngine.MonoBehaviour
{
	public global::UIRetroactionAction RetroactionAction
	{
		get
		{
			return (!global::PandoraSingleton<global::MissionManager>.Instance.focusedUnit.IsPlayed()) ? this.enemyUnitAction : this.playerUnitAction;
		}
	}

	public int Dir
	{
		get
		{
			return (!global::PandoraSingleton<global::MissionManager>.Instance.focusedUnit.IsPlayed()) ? -1 : 1;
		}
	}

	private void Awake()
	{
		for (int i = 0; i < this.playerTargets.Count; i++)
		{
			global::RetroactionInfo retroactionInfo = new global::RetroactionInfo();
			retroactionInfo.playerUnitAction = this.playerTargets[i];
			retroactionInfo.enemyUnitAction = this.enemyTargets[i];
			this.targetsInfo.Add(retroactionInfo);
		}
		global::PandoraSingleton<global::NoticeManager>.Instance.RegisterListener(global::Notices.SEQUENCE_STARTED, new global::DelReceiveNotice(this.StartSequence));
		global::PandoraSingleton<global::NoticeManager>.Instance.RegisterListener(global::Notices.SEQUENCE_ENDED, new global::DelReceiveNotice(this.EndSequence));
		global::PandoraSingleton<global::NoticeManager>.Instance.RegisterListener(global::Notices.RETROACTION_ACTION, new global::DelReceiveNotice(this.SetAction));
		global::PandoraSingleton<global::NoticeManager>.Instance.RegisterListener(global::Notices.RETROACTION_ACTION_CLEAR, new global::DelReceiveNotice(this.EndSequence));
		global::PandoraSingleton<global::NoticeManager>.Instance.RegisterListener(global::Notices.RETROACTION_ACTION_OUTCOME, new global::DelReceiveNotice(this.SetActionOutcome));
		global::PandoraSingleton<global::NoticeManager>.Instance.RegisterListener(global::Notices.RETROACTION_SHOW_OUTCOME, new global::DelReceiveNotice(this.ShowActionOutcome));
		global::PandoraSingleton<global::NoticeManager>.Instance.RegisterListener(global::Notices.RETROACTION_TARGET_STATUS, new global::DelReceiveNotice(this.ShowTargetStatus));
		global::PandoraSingleton<global::NoticeManager>.Instance.RegisterListener(global::Notices.RETROACTION_TARGET_DAMAGE, new global::DelReceiveNotice(this.SetTargetDamage));
		global::PandoraSingleton<global::NoticeManager>.Instance.RegisterListener(global::Notices.RETROACTION_TARGET_OUTCOME, new global::DelReceiveNotice(this.SetTargetOutcome));
		global::PandoraSingleton<global::NoticeManager>.Instance.RegisterListener(global::Notices.RETROACTION_TARGET_ENCHANTMENT, new global::DelReceiveNotice(this.SetTargetEnchantment));
	}

	private void ShowTargetStatus()
	{
		object target = global::PandoraSingleton<global::NoticeManager>.Instance.Parameters[0];
		string status = (string)global::PandoraSingleton<global::NoticeManager>.Instance.Parameters[1];
		global::RetroactionInfo target2 = this.GetTarget(target);
		if (target2 != null)
		{
			target2.ShowStatus(status);
		}
	}

	private void Start()
	{
		this.EndSequence();
	}

	private void ShowActionOutcome()
	{
		this.ShowActionOutcome(0f);
	}

	private void ShowActionOutcome(float delay)
	{
		object target = global::PandoraSingleton<global::NoticeManager>.Instance.Parameters[0];
		global::RetroactionInfo target2 = this.GetTarget(target);
		if (target2 != null && this.actionSet)
		{
			target2.ShowOutcome();
			target2.ShowEnchant();
			target2.RetroactionTarget.check.Show();
			this.DoMove(target2.RetroactionTarget.check.offset, 600f * (float)this.Dir, 0f);
			for (int i = 0; i < target2.RetroactionTarget.enchants.Count; i++)
			{
				if (!string.IsNullOrEmpty(target2.RetroactionTarget.enchants[i].resultName.text))
				{
					target2.RetroactionTarget.enchants[i].Show();
					this.DoMove(target2.RetroactionTarget.enchants[i].offset, -600f * (float)this.Dir, delay + 0.35f + (float)i * 0.1f);
				}
			}
		}
	}

	public void StartSequence()
	{
	}

	public void EndSequence()
	{
		this.clear = false;
		this.playerUnitAction.gameObject.SetActive(false);
		this.enemyUnitAction.gameObject.SetActive(false);
		for (int i = 0; i < this.targetsInfo.Count; i++)
		{
			this.targetsInfo[i].Reset();
		}
		this.actionSet = false;
	}

	private void SetAction()
	{
		global::UnitController unitCtrlr = (global::UnitController)global::PandoraSingleton<global::NoticeManager>.Instance.Parameters[0];
		if (!this.IsTargetValid(unitCtrlr))
		{
			return;
		}
		this.actionSet = true;
		global::UIRetroactionAction retroactionAction = this.RetroactionAction;
		retroactionAction.gameObject.SetActive(true);
		retroactionAction.Set(unitCtrlr);
		this.DoMove(retroactionAction.offset, -800f * (float)this.Dir, 0f);
		for (int i = 0; i < this.targetsInfo.Count; i++)
		{
			if (this.targetsInfo[i].unitCtrlr != null || this.targetsInfo[i].destructible != null)
			{
				this.targetsInfo[i].ShowTarget();
				this.DoMove(this.targetsInfo[i].RetroactionTarget.offset, -600f * (float)this.Dir, 0.5f + 0.2f * (float)i);
			}
		}
		this.ShowActionOutcome(0.5f);
	}

	private void DoMove(global::UnityEngine.RectTransform rectTransform, float posX, float delay)
	{
		global::UnityEngine.Vector2 anchoredPosition = rectTransform.anchoredPosition;
		anchoredPosition.x = posX;
		rectTransform.anchoredPosition = anchoredPosition;
		global::DG.Tweening.DOTween.To(() => rectTransform.anchoredPosition, delegate(global::UnityEngine.Vector2 x)
		{
			rectTransform.anchoredPosition = x;
		}, global::UnityEngine.Vector2.zero, 0.15f).SetOptions(global::DG.Tweening.AxisConstraint.X, true).SetTarget(rectTransform).SetDelay(delay);
	}

	private void SetActionOutcome()
	{
		global::UnitController unitCtrlr = (global::UnitController)global::PandoraSingleton<global::NoticeManager>.Instance.Parameters[0];
		if (!this.IsTargetValid(unitCtrlr))
		{
			return;
		}
		this.RetroactionAction.result.Set(unitCtrlr);
	}

	private void SetTargetOutcome()
	{
		object target = global::PandoraSingleton<global::NoticeManager>.Instance.Parameters[0];
		string actionEffect = (string)global::PandoraSingleton<global::NoticeManager>.Instance.Parameters[1];
		bool success = (bool)global::PandoraSingleton<global::NoticeManager>.Instance.Parameters[2];
		string damageEffect = (string)global::PandoraSingleton<global::NoticeManager>.Instance.Parameters[3];
		global::RetroactionInfo retroactionInfo = this.TryAddTarget(target);
		if (retroactionInfo != null)
		{
			retroactionInfo.AddOutcome(actionEffect, success, damageEffect);
		}
	}

	private void SetTargetDamage()
	{
		object target = global::PandoraSingleton<global::NoticeManager>.Instance.Parameters[0];
		int damage = (int)global::PandoraSingleton<global::NoticeManager>.Instance.Parameters[1];
		bool critical = (bool)global::PandoraSingleton<global::NoticeManager>.Instance.Parameters[2];
		global::RetroactionInfo retroactionInfo = this.TryAddTarget(target);
		if (retroactionInfo == null)
		{
			retroactionInfo = this.GetTarget(target);
		}
		if (retroactionInfo != null)
		{
			retroactionInfo.AddDamage(damage, critical);
		}
	}

	private void SetTargetEnchantment()
	{
		if (global::PandoraSingleton<global::NoticeManager>.Instance.Parameters[0] is global::Unit)
		{
			global::Unit unit = (global::Unit)global::PandoraSingleton<global::NoticeManager>.Instance.Parameters[0];
			string enchantmentId = (string)global::PandoraSingleton<global::NoticeManager>.Instance.Parameters[1];
			global::EffectTypeId effectTypeId = (global::EffectTypeId)((int)global::PandoraSingleton<global::NoticeManager>.Instance.Parameters[2]);
			global::RetroactionInfo target = this.GetTarget(unit);
			if (target != null)
			{
				target.AddEnchant(enchantmentId, effectTypeId, string.Empty);
			}
		}
		else if (global::PandoraSingleton<global::NoticeManager>.Instance.Parameters[0] is global::UnitController)
		{
			global::UnitController unitCtrlr = (global::UnitController)global::PandoraSingleton<global::NoticeManager>.Instance.Parameters[0];
			string enchantmentId2 = (string)global::PandoraSingleton<global::NoticeManager>.Instance.Parameters[1];
			global::EffectTypeId effectTypeId2 = (global::EffectTypeId)((int)global::PandoraSingleton<global::NoticeManager>.Instance.Parameters[2]);
			string effect = (string)global::PandoraSingleton<global::NoticeManager>.Instance.Parameters[3];
			global::RetroactionInfo retroactionInfo = this.TryAddTarget(unitCtrlr);
			if (retroactionInfo != null)
			{
				retroactionInfo.AddEnchant(enchantmentId2, effectTypeId2, effect);
			}
		}
	}

	private global::RetroactionInfo TryAddTarget(object target)
	{
		if (!this.clear)
		{
			this.EndSequence();
			this.clear = true;
		}
		global::UnitController unitController = target as global::UnitController;
		if (unitController != null)
		{
			return this.TryAddTarget(unitController);
		}
		global::Destructible destructible = target as global::Destructible;
		if (destructible != null)
		{
			return this.TryAddTarget(destructible);
		}
		return null;
	}

	private global::RetroactionInfo TryAddTarget(global::UnitController unitCtrlr)
	{
		if (!this.IsTargetValid(unitCtrlr))
		{
			return null;
		}
		global::RetroactionInfo retroactionInfo = this.GetTarget(unitCtrlr);
		if (retroactionInfo == null)
		{
			int index = this.targetsInfo.FindIndex((global::RetroactionInfo x) => x.unitCtrlr == null && x.destructible == null);
			retroactionInfo = this.targetsInfo[index];
			retroactionInfo.SetTarget(unitCtrlr);
		}
		return retroactionInfo;
	}

	private global::RetroactionInfo TryAddTarget(global::Destructible dest)
	{
		if (!this.IsTargetValid(dest))
		{
			return null;
		}
		global::RetroactionInfo retroactionInfo = this.GetTarget(dest);
		if (retroactionInfo == null)
		{
			int index = this.targetsInfo.FindIndex((global::RetroactionInfo x) => x.unitCtrlr == null && x.destructible == null);
			retroactionInfo = this.targetsInfo[index];
			retroactionInfo.SetTarget(dest);
		}
		return retroactionInfo;
	}

	public bool IsTargetValid(global::UnitController unitCtrlr)
	{
		return unitCtrlr.Imprint.State == global::MapImprintStateId.VISIBLE;
	}

	public bool IsTargetValid(global::Destructible dest)
	{
		return dest.Imprint.State == global::MapImprintStateId.VISIBLE;
	}

	private global::RetroactionInfo GetTarget(object target)
	{
		global::UnitController unitController = target as global::UnitController;
		if (unitController != null)
		{
			return this.GetTarget(unitController);
		}
		global::Unit unit = target as global::Unit;
		if (unit != null)
		{
			return this.GetTarget(unit);
		}
		global::Destructible destructible = target as global::Destructible;
		if (destructible != null)
		{
			return this.GetTarget(destructible);
		}
		return null;
	}

	private global::RetroactionInfo GetTarget(global::UnitController unitCtrlr)
	{
		return this.targetsInfo.Find((global::RetroactionInfo x) => x.unitCtrlr == unitCtrlr);
	}

	private global::RetroactionInfo GetTarget(global::Unit unit)
	{
		return this.targetsInfo.Find((global::RetroactionInfo x) => x.unitCtrlr != null && x.unitCtrlr.unit == unit);
	}

	private global::RetroactionInfo GetTarget(global::Destructible dest)
	{
		return this.targetsInfo.Find((global::RetroactionInfo x) => x.destructible == dest);
	}

	public global::UIRetroactionAction playerUnitAction;

	public global::UIRetroactionAction enemyUnitAction;

	public global::System.Collections.Generic.List<global::UIRetroactionTarget> playerTargets;

	public global::System.Collections.Generic.List<global::UIRetroactionTarget> enemyTargets;

	private readonly global::System.Collections.Generic.List<global::RetroactionInfo> targetsInfo = new global::System.Collections.Generic.List<global::RetroactionInfo>();

	private bool actionSet;

	private bool clear;
}
