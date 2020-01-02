using System;

public class Overview : global::ICheapState
{
	public Overview(global::UnitController unit)
	{
		this.unitCtrlr = unit;
	}

	void global::ICheapState.Destroy()
	{
		this.unitCtrlr = null;
	}

	void global::ICheapState.Enter(int iFrom)
	{
		global::PandoraDebug.LogInfo("Overview Enter", "SUBFLOW", null);
		global::PandoraSingleton<global::MissionManager>.Instance.CamManager.SwitchToCam(global::CameraManager.CameraType.OVERVIEW, this.unitCtrlr.transform, true, false, true, false);
		global::PandoraSingleton<global::MissionManager>.Instance.HideCombatCircles();
	}

	void global::ICheapState.Exit(int iTo)
	{
	}

	void global::ICheapState.Update()
	{
		if (global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp("overview", -1) || global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp("cancel", -1) || global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp("esc_cancel", -1))
		{
			global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<global::UnitController>(global::Notices.CURRENT_UNIT_CHANGED, this.unitCtrlr);
			this.unitCtrlr.StateMachine.ChangeState((!this.unitCtrlr.Engaged) ? 11 : 12);
		}
	}

	void global::ICheapState.FixedUpdate()
	{
	}

	private global::UnitController unitCtrlr;
}
