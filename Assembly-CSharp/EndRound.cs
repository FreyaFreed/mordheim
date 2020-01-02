using System;

public class EndRound : global::ICheapState
{
	public EndRound(global::MissionManager mission)
	{
		this.missionMngr = mission;
	}

	void global::ICheapState.Destroy()
	{
	}

	void global::ICheapState.Enter(int iFrom)
	{
		global::PandoraDebug.LogInfo("EndTurnState = " + this.missionMngr.currentTurn, "FLOW", null);
		this.missionMngr.MoveCircle.Hide();
		this.missionMngr.ClearZoneAoes();
		this.missionMngr.CamManager.SwitchToCam(global::CameraManager.CameraType.FIXED, null, false, false, true, false);
	}

	void global::ICheapState.Exit(int iTo)
	{
	}

	void global::ICheapState.Update()
	{
		this.missionMngr.currentTurn++;
		this.missionMngr.StateMachine.ChangeState(2);
	}

	void global::ICheapState.FixedUpdate()
	{
	}

	private global::MissionManager missionMngr;
}
