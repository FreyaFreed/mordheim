using System;
using System.Collections.Generic;
using UnityEngine.Events;


public class CounterChoice : ICheapState
{
	
	public CounterChoice(UnitController ctrlr)
	{
		this.unitCtrlr = ctrlr;
	}

	
	void ICheapState.Destroy()
	{
	}

	
	void ICheapState.Enter(int iFrom)
	{
		this.unitCtrlr.SetFixed(true);
		this.unitCtrlr.defenderCtrlr = this.unitCtrlr.attackerCtrlr;
		this.unitCtrlr.SetCurrentAction(SkillId.BASE_COUNTER_ATTACK);
		this.timer = null;
		this.CounterOnce = false;
		if (this.unitCtrlr.IsPlayed() && PandoraSingleton<MissionStartData>.Instance.CurrentMission.missionSave.turnTimer != 0)
		{
			this.timer = new TurnTimer((float)Constant.GetInt(ConstantId.COUNTER_TIMER), new UnityAction(this.OnTimerDone));
			this.timer.Reset(-1f);
			this.timer.Resume();
		}
		PandoraSingleton<NoticeManager>.Instance.SendNotice<UnitController>(Notices.CURRENT_UNIT_CHANGED, this.unitCtrlr);
		PandoraSingleton<NoticeManager>.Instance.SendNotice<UnitController>(Notices.CURRENT_UNIT_TARGET_CHANGED, this.unitCtrlr.defenderCtrlr);
		PandoraSingleton<NoticeManager>.Instance.SendNotice<UnitController, ActionStatus, List<ActionStatus>>(Notices.CURRENT_UNIT_ACTION_CHANGED, this.unitCtrlr, this.unitCtrlr.CurrentAction, null);
		if (this.unitCtrlr.IsPlayed())
		{
			PandoraSingleton<NoticeManager>.Instance.SendNotice(Notices.UNIT_START_SINGLE_TARGETING);
		}
	}

	
	void ICheapState.Exit(int iTo)
	{
		if (this.timer != null)
		{
			this.timer.Pause();
		}
	}

	
	void ICheapState.Update()
	{
		if (this.timer != null)
		{
			this.timer.Update();
		}
		if (PandoraSingleton<MissionManager>.Instance.IsCurrentPlayer() && (this.unitCtrlr.AICtrlr != null || (this.unitCtrlr.unit.CounterForced > 0 && !this.CounterOnce)))
		{
			PandoraSingleton<NoticeManager>.Instance.SendNotice(Notices.GAME_ACTION_CONFIRM);
			PandoraSingleton<NoticeManager>.Instance.SendNotice(Notices.CLOSE_COMBAT_COUNTER_ATTACK_VALID);
			this.unitCtrlr.SendSkillSingleTarget(SkillId.BASE_COUNTER_ATTACK, this.unitCtrlr.attackerCtrlr);
			this.CounterOnce = true;
			return;
		}
		if (this.unitCtrlr.IsPlayed())
		{
			if (PandoraSingleton<PandoraInput>.Instance.GetKeyUp("action", 0) || (this.unitCtrlr.unit.CounterForced > 0 && !this.CounterOnce))
			{
				PandoraSingleton<NoticeManager>.Instance.SendNotice(Notices.GAME_ACTION_CONFIRM);
				PandoraSingleton<NoticeManager>.Instance.SendNotice(Notices.CLOSE_COMBAT_COUNTER_ATTACK_VALID);
				this.unitCtrlr.SendSkillSingleTarget(SkillId.BASE_COUNTER_ATTACK, this.unitCtrlr.attackerCtrlr);
				this.CounterOnce = true;
				return;
			}
			if (PandoraSingleton<PandoraInput>.Instance.GetKeyUp("cancel", 0) || PandoraSingleton<PandoraInput>.Instance.GetKeyUp("esc_cancel", 0))
			{
				this.unitCtrlr.SendActionDone();
			}
		}
	}


	void ICheapState.FixedUpdate()
	{
	}

	
	private void OnTimerDone()
	{
		if (this.unitCtrlr.IsPlayed())
		{
			this.unitCtrlr.SendActionDone();
		}
	}

	
	private UnitController unitCtrlr;

	
	private TurnTimer timer;


	private bool CounterOnce;
