using System;
using UnityEngine;

public class Recovery : global::ICheapState
{
	public Recovery(global::UnitController ctrlr)
	{
		this.unitCtrlr = ctrlr;
	}

	void global::ICheapState.Destroy()
	{
	}

	void global::ICheapState.Enter(int iFrom)
	{
		this.unitCtrlr.SetFixed(true);
		if (this.unitCtrlr.unit.Status == global::UnitStateId.NONE)
		{
			this.OnSeqDone();
			return;
		}
		this.unitCtrlr.unit.PreviousStatus = this.unitCtrlr.unit.Status;
		this.unitCtrlr.unit.SetStatus(global::UnitStateId.NONE);
		this.unitCtrlr.TriggerEnchantments(global::EnchantmentTriggerId.ON_GET_UP, global::SkillId.NONE, global::UnitActionId.NONE);
		this.unitCtrlr.SetCurrentAction(global::SkillId.BASE_GETUP);
		this.unitCtrlr.CurrentAction.Activate();
		this.unitCtrlr.currentActionData.SetAction(this.unitCtrlr.CurrentAction.LocalizedName, global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadResource<global::UnityEngine.Sprite>("action/stun_recovery", true));
		global::PandoraSingleton<global::MissionManager>.Instance.PlaySequence("recovery", this.unitCtrlr, new global::DelSequenceDone(this.OnSeqDone));
	}

	void global::ICheapState.Exit(int iTo)
	{
	}

	void global::ICheapState.Update()
	{
	}

	void global::ICheapState.FixedUpdate()
	{
	}

	private void OnSeqDone()
	{
		this.unitCtrlr.SetStatusFX();
		this.unitCtrlr.WaitForAction(global::UnitController.State.STUPIDITY);
		this.unitCtrlr.ReapplyOnEngage();
		this.unitCtrlr.CheckAllAlone();
	}

	private global::UnitController unitCtrlr;
}
