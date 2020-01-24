using System;
using System.Collections.Generic;
using UnityEngine.Events;

public class CounterChoice : global::ICheapState
{
	public CounterChoice(global::UnitController ctrlr)
	{
		this.unitCtrlr = ctrlr;
	}

	void global::ICheapState.Destroy()
	{
	}

	void global::ICheapState.Enter(int iFrom)
	{
		this.unitCtrlr.SetFixed(true);
		this.unitCtrlr.defenderCtrlr = this.unitCtrlr.attackerCtrlr;
		this.unitCtrlr.SetCurrentAction(global::SkillId.BASE_COUNTER_ATTACK);
		this.timer = null;
		this.CounterOnce = false;
		if (this.unitCtrlr.IsPlayed() && global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.turnTimer != 0)
		{
			this.timer = new global::TurnTimer((float)global::Constant.GetInt(global::ConstantId.COUNTER_TIMER), new global::UnityEngine.Events.UnityAction(this.OnTimerDone));
			this.timer.Reset(-1f);
			this.timer.Resume();
		}
		global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<global::UnitController>(global::Notices.CURRENT_UNIT_CHANGED, this.unitCtrlr);
		global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<global::UnitController>(global::Notices.CURRENT_UNIT_TARGET_CHANGED, this.unitCtrlr.defenderCtrlr);
		global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<global::UnitController, global::ActionStatus, global::System.Collections.Generic.List<global::ActionStatus>>(global::Notices.CURRENT_UNIT_ACTION_CHANGED, this.unitCtrlr, this.unitCtrlr.CurrentAction, null);
		if (this.unitCtrlr.IsPlayed())
		{
			global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice(global::Notices.UNIT_START_SINGLE_TARGETING);
		}
	}

	void global::ICheapState.Exit(int iTo)
	{
		if (this.timer != null)
		{
			this.timer.Pause();
		}
	}

	void global::ICheapState.Update()
	{
		if (this.timer != null)
		{
			this.timer.Update();
		}
		if (global::PandoraSingleton<global::MissionManager>.Instance.IsCurrentPlayer() && (this.unitCtrlr.AICtrlr != null || (this.unitCtrlr.unit.CounterForced > 0 && !this.CounterOnce)))
		{
			global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice(global::Notices.GAME_ACTION_CONFIRM);
			global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice(global::Notices.CLOSE_COMBAT_COUNTER_ATTACK_VALID);
			this.unitCtrlr.SendSkillSingleTarget(global::SkillId.BASE_COUNTER_ATTACK, this.unitCtrlr.attackerCtrlr);
			this.CounterOnce = true;
			return;
		}
		if (this.unitCtrlr.IsPlayed())
		{
			if (global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp("action", 0) || (this.unitCtrlr.unit.CounterForced > 0 && !this.CounterOnce))
			{
				global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice(global::Notices.GAME_ACTION_CONFIRM);
				global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice(global::Notices.CLOSE_COMBAT_COUNTER_ATTACK_VALID);
				this.unitCtrlr.SendSkillSingleTarget(global::SkillId.BASE_COUNTER_ATTACK, this.unitCtrlr.attackerCtrlr);
				this.CounterOnce = true;
				return;
			}
			if (global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp("cancel", 0) || global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp("esc_cancel", 0))
			{
				this.unitCtrlr.SendActionDone();
			}
		}
	}

	void global::ICheapState.FixedUpdate()
	{
	}

	private void OnTimerDone()
	{
		if (this.unitCtrlr.IsPlayed())
		{
			this.unitCtrlr.SendActionDone();
		}
	}

	private global::UnitController unitCtrlr;

	private global::TurnTimer timer;

	private bool CounterOnce;
