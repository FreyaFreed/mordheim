using System;

public class Delay : global::ICheapState
{
	public Delay(global::UnitController ctrlr)
	{
		this.unitCtrlr = ctrlr;
	}

	void global::ICheapState.Destroy()
	{
		this.unitCtrlr = null;
	}

	void global::ICheapState.Enter(int iFrom)
	{
		this.unitCtrlr.currentActionData.SetAction(this.unitCtrlr.CurrentAction);
		this.unitCtrlr.SetFixed(true);
		this.unitCtrlr.SetGraphWalkability(false);
		global::PandoraSingleton<global::MissionManager>.Instance.PlaySequence("skill", this.unitCtrlr, new global::DelSequenceDone(this.OnSeqDone));
		this.unitCtrlr.lastTimer = global::PandoraSingleton<global::MissionManager>.Instance.TurnTimer.Timer;
	}

	void global::ICheapState.Exit(int iTo)
	{
		global::PandoraSingleton<global::MissionManager>.Instance.MissionEndData.UpdateUnit(this.unitCtrlr);
	}

	void global::ICheapState.Update()
	{
	}

	void global::ICheapState.FixedUpdate()
	{
	}

	private void OnSeqDone()
	{
		this.unitCtrlr.StateMachine.ChangeState(9);
		global::PandoraSingleton<global::MissionManager>.Instance.SendUnitBack(global::Constant.GetInt(global::ConstantId.DELAY_POSITIONS));
	}

	private global::UnitController unitCtrlr;
}
