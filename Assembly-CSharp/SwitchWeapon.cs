using System;

public class SwitchWeapon : global::ICheapState
{
	public SwitchWeapon(global::UnitController ctrlr)
	{
		this.unitCtrlr = ctrlr;
	}

	void global::ICheapState.Destroy()
	{
	}

	void global::ICheapState.Enter(int iFrom)
	{
		this.unitCtrlr.ValidMove();
		this.unitCtrlr.currentActionData.SetAction(this.unitCtrlr.CurrentAction);
		if (this.unitCtrlr.AICtrlr != null)
		{
			this.unitCtrlr.AICtrlr.switchCount++;
		}
		global::PandoraSingleton<global::MissionManager>.Instance.PlaySequence("switch_weapons", this.unitCtrlr, new global::DelSequenceDone(this.OnSeqDone));
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

	private void OnSeqDone()
	{
		global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<global::UnitController>(global::Notices.UNIT_WEAPON_CHANGED, this.unitCtrlr);
		this.unitCtrlr.StateMachine.ChangeState(10);
	}

	private global::UnitController unitCtrlr;
}
