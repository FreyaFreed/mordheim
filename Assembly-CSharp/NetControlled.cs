using System;
using UnityEngine;

public class NetControlled : global::ICheapState
{
	public NetControlled(global::UnitController ctrlr)
	{
		this.unitCtrlr = ctrlr;
	}

	void global::ICheapState.Destroy()
	{
	}

	void global::ICheapState.Enter(int iFrom)
	{
		global::PandoraDebug.LogInfo("NetControlled Enter ", "UNIT_FLOW", this.unitCtrlr);
		this.unitCtrlr.SetFixed(false);
		this.unitCtrlr.GetComponent<global::UnityEngine.Rigidbody>().isKinematic = true;
		global::PandoraSingleton<global::MissionManager>.Instance.TurnOffActionZones();
	}

	void global::ICheapState.Exit(int iTo)
	{
		this.unitCtrlr.lastActionWounds = 0;
		global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice(global::Notices.RETROACTION_ACTION_CLEAR);
	}

	void global::ICheapState.Update()
	{
		this.unitCtrlr.UpdateTargetsData();
		global::PandoraSingleton<global::MissionManager>.Instance.RefreshFoWTargetMoving(this.unitCtrlr);
	}

	void global::ICheapState.FixedUpdate()
	{
	}

	private global::UnitController unitCtrlr;
}
