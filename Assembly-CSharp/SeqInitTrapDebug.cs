using System;
using System.Collections.Generic;
using UnityEngine;
using WellFired;

[global::WellFired.USequencerFriendlyName("InitTrapDebug")]
[global::WellFired.USequencerEvent("Mordheim/InitTrapDebug")]
public class SeqInitTrapDebug : global::WellFired.USEventBase
{
	public override void FireEvent()
	{
		if (global::UnityEngine.Application.loadedLevelName != "sequence")
		{
			return;
		}
		global::UnitController focusedUnit = global::PandoraSingleton<global::MissionManager>.Instance.focusedUnit;
		if (this.trap != null)
		{
			global::UnityEngine.Object.Destroy(this.trap.gameObject);
		}
		if (this.trapNode != null)
		{
			this.trapNode.gameObject.SetActive(false);
			global::TrapTypeData trapTypeData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::TrapTypeData>((int)this.trapNode.typeId);
			global::System.Collections.Generic.List<global::TrapTypeJoinTrapData> list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::TrapTypeJoinTrapData>("fk_trap_type_id", trapTypeData.Id.ToIntString<global::TrapTypeId>());
			int index = global::PandoraSingleton<global::MissionManager>.Instance.NetworkTyche.Rand(0, list.Count);
			global::TrapId trapId = list[index].TrapId;
			string name = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::TrapData>((int)trapId).Name;
			global::UnityEngine.GameObject original = null;
			global::UnityEngine.GameObject gameObject = global::UnityEngine.Object.Instantiate<global::UnityEngine.GameObject>(original);
			gameObject.transform.position = this.trapNode.transform.position;
			gameObject.transform.rotation = this.trapNode.transform.rotation;
			this.trap = gameObject.GetComponent<global::Trap>();
			this.trap.Init(trapTypeData, 0U);
			this.trap.trigger.SetActive(false);
			focusedUnit.activeTrigger = this.trap;
		}
		focusedUnit.attackResultId = this.resultId;
		focusedUnit.unit.SetStatus(this.state);
		focusedUnit.currentActionLabel = this.actionLabel;
		focusedUnit.flyingLabel = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(this.atkFlyingLabel);
	}

	public override void ProcessEvent(float runningTime)
	{
	}

	public override void EndEvent()
	{
		base.EndEvent();
	}

	public global::TrapNode trapNode;

	public global::AttackResultId resultId;

	public global::UnitStateId state;

	public string actionLabel;

	public string atkFlyingLabel;

	private global::Trap trap;
}
