using System;
using System.Collections.Generic;

public class StartGame : global::ICheapState
{
	public StartGame(global::MissionManager mission)
	{
		this.missionMngr = mission;
	}

	void global::ICheapState.Destroy()
	{
	}

	void global::ICheapState.Enter(int iFrom)
	{
		for (int i = 0; i < this.missionMngr.WarbandCtrlrs.Count; i++)
		{
			global::System.Collections.Generic.List<global::UnitController> unitCtrlrs = this.missionMngr.WarbandCtrlrs[i].unitCtrlrs;
			for (int j = 0; j < unitCtrlrs.Count; j++)
			{
				if (unitCtrlrs[j].unit.Status != global::UnitStateId.OUT_OF_ACTION)
				{
					unitCtrlrs[j].StartGameInitialization();
				}
			}
		}
		this.missionMngr.TurnOffActionZones();
		if (!global::PandoraSingleton<global::MissionStartData>.Instance.isReload)
		{
			this.missionMngr.ResetLadderIdx(false);
		}
		this.missionMngr.EndLoading();
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
