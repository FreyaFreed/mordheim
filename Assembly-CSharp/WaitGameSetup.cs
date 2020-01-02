using System;
using UnityEngine;

public class WaitGameSetup : global::ICheapState
{
	public WaitGameSetup(global::MissionManager mission)
	{
		this.missionMngr = mission;
	}

	void global::ICheapState.Destroy()
	{
	}

	void global::ICheapState.Enter(int iFrom)
	{
		global::UnityEngine.Transform camTarget = null;
		global::UnitController unitController = null;
		if (global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.isCampaign || global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.isTuto || global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.autoDeploy)
		{
			for (int i = 0; i < this.missionMngr.InitiativeLadder.Count; i++)
			{
				if (this.missionMngr.InitiativeLadder[i].IsPlayed())
				{
					unitController = this.missionMngr.InitiativeLadder[i];
					camTarget = unitController.transform;
					break;
				}
			}
		}
		else
		{
			camTarget = global::PandoraSingleton<global::MissionStartData>.Instance.spawnNodes[this.missionMngr.GetMyWarbandCtrlr().idx][0].transform;
		}
		this.missionMngr.CamManager.SwitchToCam(global::CameraManager.CameraType.CHARACTER, camTarget, false, true, true, unitController && unitController.unit.Data.UnitSizeId == global::UnitSizeId.LARGE);
		this.missionMngr.CamManager.GetComponent<global::UnityEngine.Camera>().enabled = true;
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

	private global::MissionManager missionMngr;
}
