using System;

public class TurnMessage : global::ICheapState
{
	public TurnMessage(global::UnitController ctrlr)
	{
		this.unitCtrlr = ctrlr;
	}

	void global::ICheapState.Destroy()
	{
	}

	void global::ICheapState.Enter(int iFrom)
	{
		this.displayingMessage = global::PandoraSingleton<global::MissionManager>.Instance.MsgManager.DisplayNewTurn(null);
	}

	void global::ICheapState.Exit(int iTo)
	{
		global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<bool, string, bool>(global::Notices.GAME_TUTO_MESSAGE, false, string.Empty, false);
		if (global::PandoraSingleton<global::MissionManager>.Instance.CurrentLadderIdx >= 0)
		{
			global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice(global::Notices.MISSION_PLAYER_CHANGE);
		}
	}

	void global::ICheapState.Update()
	{
		if (!this.displayingMessage)
		{
			this.unitCtrlr.nextState = global::UnitController.State.UPDATE_EFFECTS;
		}
		else if (global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp("action", 1))
		{
			this.displayingMessage = global::PandoraSingleton<global::MissionManager>.Instance.MsgManager.DisplayNextPos(null);
		}
	}

	void global::ICheapState.FixedUpdate()
	{
	}

	private global::UnitController unitCtrlr;

	private bool displayingMessage;
}
