using System;
using UnityEngine;

public class ManticoreFight : global::UnityEngine.MonoBehaviour, global::ICustomMissionSetup
{
	void global::ICustomMissionSetup.Execute()
	{
		for (int i = 0; i < global::PandoraSingleton<global::MissionManager>.Instance.WarbandCtrlrs.Count; i++)
		{
			for (int j = 0; j < global::PandoraSingleton<global::MissionManager>.Instance.WarbandCtrlrs[i].unitCtrlrs.Count; j++)
			{
				global::UnitController unitController = global::PandoraSingleton<global::MissionManager>.Instance.WarbandCtrlrs[i].unitCtrlrs[j];
				if (unitController.unit.Id == global::UnitId.MANTICORE && unitController.AICtrlr != null)
				{
					unitController.AICtrlr.targetDecisionPoint = this.spawnPoint;
					return;
				}
			}
		}
	}

	public global::FlyPoint spawnPoint;
}
