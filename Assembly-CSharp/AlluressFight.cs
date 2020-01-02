using System;
using System.Collections.Generic;
using UnityEngine;

public class AlluressFight : global::UnityEngine.MonoBehaviour, global::ICustomMissionSetup
{
	void global::ICustomMissionSetup.Execute()
	{
		for (int i = 0; i < global::PandoraSingleton<global::MissionManager>.Instance.WarbandCtrlrs.Count; i++)
		{
			for (int j = 0; j < global::PandoraSingleton<global::MissionManager>.Instance.WarbandCtrlrs[i].unitCtrlrs.Count; j++)
			{
				global::UnitController unitController = global::PandoraSingleton<global::MissionManager>.Instance.WarbandCtrlrs[i].unitCtrlrs[j];
				if (unitController.unit.CampaignData != null)
				{
					if (unitController.unit.CampaignData.Id == this.firstAlluress)
					{
						unitController.unlockSearchPointOnDeath = true;
						unitController.linkedSearchPoints = this.searchPoints;
					}
					if (unitController.unit.CampaignData.Id == this.secondAlluress)
					{
						unitController.reviveUntilSearchEmpty = true;
						unitController.linkedSearchPoints = this.searchPoints;
						unitController.forcedSpawnPoints = this.spawnPoints;
					}
				}
			}
		}
	}

	public global::CampaignUnitId firstAlluress;

	public global::CampaignUnitId secondAlluress;

	public global::System.Collections.Generic.List<global::SearchPoint> searchPoints;

	public global::System.Collections.Generic.List<global::DecisionPoint> spawnPoints;
}
