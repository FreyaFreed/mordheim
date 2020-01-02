using System;
using System.Collections.Generic;
using UnityEngine;
using WellFired;

public class SequenceHelper : global::PandoraSingleton<global::SequenceHelper>
{
	private void Start()
	{
		global::UnityEngine.GameObject gameObject = global::UnityEngine.GameObject.Find("units");
		this.unitCtrlrs = new global::System.Collections.Generic.List<global::UnitController>();
		this.unitCtrlrs.AddRange(gameObject.GetComponentsInChildren<global::UnitController>());
		global::PandoraSingleton<global::NoticeManager>.Instance.RegisterListener(global::Notices.BASE_NOTICE, new global::DelReceiveNotice(this.BaseNotice));
		this.Refresh();
		global::UnityEngine.Object.Destroy(global::PandoraSingleton<global::MissionManager>.Instance.CamManager.GetComponent<global::UnityEngine.Animator>());
	}

	private void Update()
	{
		if (!global::PandoraSingleton<global::MissionManager>.Instance.CamManager.GetComponent<global::UnityEngine.Camera>().enabled)
		{
			return;
		}
		if (global::UnityEngine.Input.GetKeyDown(global::UnityEngine.KeyCode.Backspace))
		{
			global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice(global::Notices.SEQUENCE_ENDED);
		}
		if (this.refresh)
		{
			this.Refresh();
		}
	}

	public global::ActionZoneSeqHelper GetActionZoneHelper(global::UnitActionId id)
	{
		foreach (global::ActionZoneSeqHelper actionZoneSeqHelper in this.zoneHelpers)
		{
			if (actionZoneSeqHelper.actionId == id)
			{
				return actionZoneSeqHelper;
			}
		}
		return this.defaultZoneHelper;
	}

	private void BaseNotice()
	{
		string str = (string)global::PandoraSingleton<global::NoticeManager>.Instance.Parameters[0];
		bool flag = (bool)global::PandoraSingleton<global::NoticeManager>.Instance.Parameters[1];
		global::UnityEngine.Debug.Log("Notice Received : " + str + ((!flag) ? " allied " : " enemy "));
	}

	private void InitWarbands()
	{
		if (global::PandoraSingleton<global::MissionManager>.Instance.WarbandCtrlrs.Count == 0)
		{
			global::MissionWarbandSave warband = new global::MissionWarbandSave(global::WarbandId.HUMAN_MERCENARIES, global::CampaignWarbandId.NONE, "war1", string.Empty, "cpu", 0, global::PandoraSingleton<global::Hermes>.Instance.PlayerIndex, 0, global::PlayerTypeId.PLAYER, null);
			global::WarbandController warbandController = new global::WarbandController(warband, global::DeploymentId.WAGON, 0, 0, global::PrimaryObjectiveTypeId.NONE, 0, 0);
			warbandController.playerIdx = global::PandoraSingleton<global::Hermes>.Instance.PlayerIndex;
			global::PandoraSingleton<global::MissionManager>.Instance.WarbandCtrlrs.Add(warbandController);
			warbandController = new global::WarbandController(warband, global::DeploymentId.WAGON, 1, 1, global::PrimaryObjectiveTypeId.NONE, 0, 0);
			warbandController.playerIdx++;
			global::PandoraSingleton<global::MissionManager>.Instance.WarbandCtrlrs.Add(warbandController);
		}
		global::PandoraSingleton<global::MissionManager>.Instance.WarbandCtrlrs[0].unitCtrlrs.Clear();
		global::PandoraSingleton<global::MissionManager>.Instance.WarbandCtrlrs[1].unitCtrlrs.Clear();
	}

	private void Refresh()
	{
		this.refresh = false;
		this.InitWarbands();
		foreach (global::UnitController unitController in this.unitCtrlrs)
		{
			if (unitController == this.currentUnit)
			{
				unitController.gameObject.SetActive(true);
				unitController.transform.position = global::UnityEngine.Vector3.zero;
				unitController.transform.rotation = global::UnityEngine.Quaternion.identity;
				global::PandoraSingleton<global::MissionManager>.Instance.InitiativeLadder.Clear();
				global::PandoraSingleton<global::MissionManager>.Instance.InitiativeLadder.Add(unitController);
				global::PandoraSingleton<global::MissionManager>.Instance.WarbandCtrlrs[0].unitCtrlrs.Add(unitController);
				global::PandoraSingleton<global::MissionManager>.Instance.ForceFocusedUnit(unitController);
			}
		}
		foreach (global::UnitController unitController2 in this.unitCtrlrs)
		{
			if (unitController2 == this.defender)
			{
				unitController2.gameObject.SetActive(true);
				unitController2.transform.position = global::UnityEngine.Vector3.forward * 5f;
				unitController2.transform.rotation = global::UnityEngine.Quaternion.Euler(global::UnityEngine.Vector3.up * -180f);
				global::PandoraSingleton<global::MissionManager>.Instance.GetCurrentUnit().defenderCtrlr = unitController2;
				unitController2.attackerCtrlr = global::PandoraSingleton<global::MissionManager>.Instance.GetCurrentUnit();
				global::PandoraSingleton<global::MissionManager>.Instance.WarbandCtrlrs[1].unitCtrlrs.Add(unitController2);
			}
		}
		foreach (global::UnitController unitController3 in this.unitCtrlrs)
		{
			if (unitController3 != this.currentUnit && unitController3 != this.defender)
			{
				unitController3.gameObject.SetActive(false);
			}
		}
	}

	public global::UnitController currentUnit;

	public global::UnitController defender;

	public bool refresh = true;

	public global::System.Collections.Generic.List<global::ActionZoneSeqHelper> zoneHelpers;

	private global::System.Collections.Generic.List<global::UnitController> unitCtrlrs;

	public global::ActionZoneSeqHelper defaultZoneHelper;

	public bool gui;

	private global::System.Collections.Generic.List<global::WellFired.USSequencer> sequences;

	private global::UnityEngine.Vector2 scroller = default(global::UnityEngine.Vector2);
}
