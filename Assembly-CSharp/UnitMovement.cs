using System;

public class UnitMovement : global::ICheapState
{
	public UnitMovement(global::MissionManager mission)
	{
		this.missionMngr = mission;
	}

	void global::ICheapState.Destroy()
	{
		this.missionMngr = null;
	}

	void global::ICheapState.Enter(int iFrom)
	{
		global::PandoraDebug.LogInfo("MissionManager UnitMovement", "SUBFLOW", null);
	}

	void global::ICheapState.Exit(int iTo)
	{
	}

	void global::ICheapState.Update()
	{
		this.missionMngr.CheckUnitTurnFinished();
	}

	void global::ICheapState.FixedUpdate()
	{
	}

	private global::MissionManager missionMngr;
}
