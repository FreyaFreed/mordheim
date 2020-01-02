using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Teleport : global::ICheapState
{
	public Teleport(global::UnitController ctrlr)
	{
		this.unitCtrlr = ctrlr;
	}

	void global::ICheapState.Destroy()
	{
	}

	void global::ICheapState.Enter(int iFrom)
	{
		this.teleport = (global::Teleporter)this.unitCtrlr.activeTrigger;
		this.unitCtrlr.currentTeleporter = null;
		this.unitCtrlr.SetAnimSpeed(0f);
		this.unitCtrlr.SetFixed(true);
		this.unitCtrlr.SetKinemantic(true);
		if (this.unitCtrlr == global::PandoraSingleton<global::MissionManager>.Instance.GetCurrentUnit())
		{
			this.unitCtrlr.ValidMove();
		}
		this.unitCtrlr.currentActionData.SetAction(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("skill_name_teleport"), global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadResource<global::UnityEngine.Sprite>("action/teleport", true));
		this.teleport.Trigger(this.unitCtrlr);
		this.unitCtrlr.Hide(true, false, null);
		this.unitCtrlr.StartCoroutine(this.WaitForDissolved(1f));
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

	private global::System.Collections.IEnumerator WaitForDissolved(float time)
	{
		yield return new global::UnityEngine.WaitForSeconds(time);
		this.teleport.ActionOnUnit(this.unitCtrlr);
		if (this.unitCtrlr.IsPlayed())
		{
			this.unitCtrlr.Hide(false, false, new global::UnityEngine.Events.UnityAction(this.OnSeqDone));
		}
		else
		{
			global::PandoraSingleton<global::MissionManager>.Instance.RefreshFoWTargetMoving(this.unitCtrlr);
			if (this.unitCtrlr.Imprint.State == global::MapImprintStateId.VISIBLE)
			{
				this.unitCtrlr.Hide(false, false, null);
				this.unitCtrlr.StartCoroutine(this.WaitForSeqDone(1f));
			}
			else
			{
				this.OnSeqDone();
			}
		}
		yield break;
	}

	private global::System.Collections.IEnumerator WaitForSeqDone(float time)
	{
		yield return new global::UnityEngine.WaitForSeconds(time);
		this.OnSeqDone();
		yield break;
	}

	public void OnSeqDone()
	{
		this.unitCtrlr.activeTrigger = null;
		this.unitCtrlr.StateMachine.ChangeState(10);
	}

	private global::UnitController unitCtrlr;

	private global::Teleporter teleport;
}
