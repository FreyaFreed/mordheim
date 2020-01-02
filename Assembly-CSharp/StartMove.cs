using System;
using System.Collections;

public class StartMove : global::ICheapState
{
	public StartMove(global::UnitController ctrlr)
	{
		this.unitCtrlr = ctrlr;
	}

	void global::ICheapState.Destroy()
	{
	}

	void global::ICheapState.Enter(int iFrom)
	{
		this.fixedUpdateCount = 0;
		this.unitCtrlr.startPosition = this.unitCtrlr.transform.position;
		this.unitCtrlr.startRotation = this.unitCtrlr.transform.rotation;
		this.unitCtrlr.SetKinemantic(false);
		this.unitCtrlr.attackerCtrlr = null;
		this.unitCtrlr.defenderCtrlr = null;
		this.unitCtrlr.friendlyEntered.Clear();
		this.unitCtrlr.interactivePoint = null;
		this.unitCtrlr.activeActionDest = null;
		this.unitCtrlr.prevInteractiveTarget = null;
		this.unitCtrlr.nextInteractiveTarget = null;
		this.unitCtrlr.wyrdstoneRollModifier = 0;
		this.unitCtrlr.attackResultId = global::AttackResultId.NONE;
		this.unitCtrlr.lastActionWounds = 0;
		this.unitCtrlr.actionOutcomeLabel = string.Empty;
		this.unitCtrlr.flyingLabel = string.Empty;
		if (global::PandoraSingleton<global::Hermes>.Instance.IsHost() && global::PandoraSingleton<global::MissionManager>.Instance.interruptingUnit == this.unitCtrlr)
		{
			global::PandoraSingleton<global::MissionManager>.Instance.interruptingUnit = null;
		}
		this.unitCtrlr.unit.UpdateValidNextActionEnchantments();
		if (this.unitCtrlr.LastActivatedAction != null)
		{
			this.unitCtrlr.TriggerEnchantments(global::EnchantmentTriggerId.ON_POST_ACTION, this.unitCtrlr.LastActivatedAction.SkillId, this.unitCtrlr.LastActivatedAction.ActionId);
			this.unitCtrlr.LastActivatedAction = null;
		}
		this.unitCtrlr.SetCurrentAction(global::SkillId.NONE);
		this.unitCtrlr.unit.ResetTempPoints();
		global::PandoraDebug.LogDebug(string.Concat(new object[]
		{
			"StartMove Enter - its turn ",
			this.unitCtrlr == global::PandoraSingleton<global::MissionManager>.Instance.GetCurrentUnit(),
			" - status : ",
			this.unitCtrlr.unit.Status,
			" - wasEngaged = ",
			this.unitCtrlr.wasEngaged,
			" IsEngaged Now = ",
			this.unitCtrlr.Engaged
		}), "FLOW", this.unitCtrlr);
		global::PandoraSingleton<global::MissionManager>.Instance.ClearBeacons();
		this.unitCtrlr.currentActionData.Reset();
		global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice(global::Notices.RETROACTION_ACTION_CLEAR);
		this.unitCtrlr.StopCoroutine(this.GotoNextState());
		this.unitCtrlr.StartCoroutine(this.GotoNextState());
	}

	void global::ICheapState.Exit(int iTo)
	{
		global::PandoraSingleton<global::MissionManager>.Instance.MissionEndData.UpdateUnit(this.unitCtrlr);
		if (global::PandoraSingleton<global::GameManager>.Instance.currentSave != null && !global::PandoraSingleton<global::MissionManager>.Instance.MissionEndData.missionSave.isTuto)
		{
			global::PandoraSingleton<global::GameManager>.Instance.Save.SaveCampaign(global::PandoraSingleton<global::GameManager>.Instance.currentSave, global::PandoraSingleton<global::GameManager>.Instance.campaign);
		}
	}

	void global::ICheapState.Update()
	{
	}

	void global::ICheapState.FixedUpdate()
	{
		this.fixedUpdateCount++;
	}

	private global::System.Collections.IEnumerator GotoNextState()
	{
		while (global::PandoraSingleton<global::MissionManager>.Instance.IsNavmeshUpdating)
		{
			yield return null;
		}
		while (this.fixedUpdateCount <= 1)
		{
			yield return null;
		}
		this.unitCtrlr.transform.position = this.unitCtrlr.startPosition;
		this.unitCtrlr.SetFixed(true);
		this.unitCtrlr.SetCombatCircle(global::PandoraSingleton<global::MissionManager>.Instance.GetCurrentUnit(), false);
		while (global::PandoraSingleton<global::MissionManager>.Instance.IsNavmeshUpdating)
		{
			yield return null;
		}
		this.unitCtrlr.wasEngaged = this.unitCtrlr.Engaged;
		this.unitCtrlr.CheckEngaged(true);
		if (this.unitCtrlr == global::PandoraSingleton<global::MissionManager>.Instance.GetCurrentUnit())
		{
			this.unitCtrlr.UpdateTargetsData();
			global::PandoraSingleton<global::MissionManager>.Instance.TurnTimer.Resume();
			if (this.unitCtrlr.IsPlayed())
			{
				global::PandoraSingleton<global::MissionManager>.Instance.ShowCombatCircles(this.unitCtrlr);
				global::PandoraSingleton<global::MissionManager>.Instance.RefreshFoWOwnMoving(this.unitCtrlr);
			}
			else
			{
				global::PandoraSingleton<global::MissionManager>.Instance.HideCombatCircles();
				global::PandoraSingleton<global::MissionManager>.Instance.RefreshFoWTargetMoving(this.unitCtrlr);
				global::PandoraSingleton<global::MissionManager>.Instance.CamManager.SwitchToCam(global::CameraManager.CameraType.WATCH, global::PandoraSingleton<global::MissionManager>.Instance.CamManager.Target, true, false, true, false);
			}
			this.unitCtrlr.UpdateActionStatus(false, global::UnitActionRefreshId.NONE);
			this.actionAvailables = (this.unitCtrlr.availableActionStatus.Count > 0);
			global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<global::UnitController>(global::Notices.CURRENT_UNIT_CHANGED, this.unitCtrlr);
			global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice(global::Notices.UNIT_START_MOVE);
		}
		if (global::PandoraSingleton<global::MissionManager>.Instance.gameFinished || global::PandoraSingleton<global::MissionManager>.Instance.CheckEndGame())
		{
			this.unitCtrlr.StateMachine.ChangeState(9);
		}
		else if (this.unitCtrlr.IsMine())
		{
			if (this.unitCtrlr == global::PandoraSingleton<global::MissionManager>.Instance.GetCurrentUnit())
			{
				if (this.unitCtrlr.unit.IsAvailable() && this.actionAvailables)
				{
					if (this.unitCtrlr.AICtrlr == null)
					{
						global::Engaged engaged = (global::Engaged)this.unitCtrlr.StateMachine.GetState(12);
						engaged.actionIndex = 0;
						global::Moving move = (global::Moving)this.unitCtrlr.StateMachine.GetState(11);
						move.actionIndex = 0;
						this.unitCtrlr.StateMachine.ChangeState(11);
					}
					else
					{
						this.unitCtrlr.ClampToNavMesh();
						this.unitCtrlr.StateMachine.ChangeState(42);
						global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<bool>(global::Notices.MISSION_SHOW_ENEMY, false);
					}
				}
				else
				{
					this.unitCtrlr.SendSkill(global::SkillId.BASE_END_TURN);
				}
			}
			else
			{
				this.unitCtrlr.StateMachine.ChangeState(9);
			}
		}
		else if (this.unitCtrlr == global::PandoraSingleton<global::MissionManager>.Instance.GetCurrentUnit())
		{
			this.unitCtrlr.StateMachine.ChangeState(43);
			global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<bool>(global::Notices.MISSION_SHOW_ENEMY, false);
		}
		else
		{
			this.unitCtrlr.StateMachine.ChangeState(9);
		}
		if (this.unitCtrlr != global::PandoraSingleton<global::MissionManager>.Instance.GetCurrentUnit())
		{
			this.unitCtrlr.ActionDone();
		}
		yield break;
	}

	private global::UnitController unitCtrlr;

	private bool actionAvailables;

	private int fixedUpdateCount;
}
