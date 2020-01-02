using System;
using System.Collections;
using UnityEngine;

public class ActivateStance : global::ICheapState
{
	public ActivateStance(global::UnitController ctrlr)
	{
		this.unitCtrlr = ctrlr;
	}

	void global::ICheapState.Destroy()
	{
	}

	void global::ICheapState.Enter(int iFrom)
	{
		this.unitCtrlr.SetFixed(true);
		this.unitCtrlr.currentActionData.SetAction(this.unitCtrlr.CurrentAction);
		global::PandoraSingleton<global::MissionManager>.Instance.PlaySequence("skill", this.unitCtrlr, new global::DelSequenceDone(this.OnSeqDone));
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

	private global::System.Collections.IEnumerator ShowOutcome()
	{
		yield return new global::UnityEngine.WaitForSeconds(0.5f);
		global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<global::UnitController>(global::Notices.RETROACTION_SHOW_OUTCOME, this.unitCtrlr);
		yield break;
	}

	private void OnSeqDone()
	{
		this.unitCtrlr.SkillRPC(339);
	}

	private global::UnitController unitCtrlr;
}
