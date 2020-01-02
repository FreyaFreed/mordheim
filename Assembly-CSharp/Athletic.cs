using System;
using UnityEngine;

public class Athletic : global::ICheapState
{
	public Athletic(global::UnitController ctrlr)
	{
		this.unitCtrlr = ctrlr;
	}

	void global::ICheapState.Destroy()
	{
	}

	void global::ICheapState.Enter(int iFrom)
	{
		this.prepareState = (global::PrepareAthletic)this.unitCtrlr.StateMachine.GetState(46);
		this.unitCtrlr.SetFixed(false);
		this.unitCtrlr.SetKinemantic(true);
		this.targetPos = this.unitCtrlr.activeActionDest.destination.transform.position + this.unitCtrlr.activeActionDest.destination.transform.forward * -(this.unitCtrlr.CapsuleRadius / 2f);
		if (!this.prepareState.success)
		{
			int damage = global::PandoraSingleton<global::MissionManager>.Instance.NetworkTyche.Rand(this.unitCtrlr.CurrentAction.GetMinDamage(false), this.unitCtrlr.CurrentAction.GetMaxDamage(false));
			this.unitCtrlr.ComputeDirectWound(damage, this.unitCtrlr.CurrentAction.skillData.BypassArmor, this.unitCtrlr, false);
		}
		else
		{
			global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<global::UnitController, string, bool, string>(global::Notices.RETROACTION_TARGET_OUTCOME, this.unitCtrlr, string.Empty, this.prepareState.success, global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("retro_outcome_success"));
		}
		this.unitCtrlr.RemoveAthletics();
		this.unitCtrlr.currentActionData.SetAction(this.unitCtrlr.CurrentAction);
		this.unitCtrlr.currentActionData.SetActionOutcome(this.prepareState.success);
		string sequence = string.Format("{0}_{1}m{2}{3}", new object[]
		{
			this.prepareState.actionId.ToLowerString(),
			this.prepareState.height,
			this.prepareState.success ? string.Empty : "_fail",
			(this.prepareState.success || this.unitCtrlr.unit.Status != global::UnitStateId.OUT_OF_ACTION) ? string.Empty : "_ooa"
		});
		bool value = !this.unitCtrlr.unit.HasMutatedArm();
		this.unitCtrlr.animator.SetBool(global::AnimatorIds.sheathe, value);
		global::PandoraSingleton<global::MissionManager>.Instance.PlaySequence(sequence, this.unitCtrlr, new global::DelSequenceDone(this.SequenceDone));
		global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<global::UnitController>(global::Notices.RETROACTION_SHOW_OUTCOME, this.unitCtrlr);
		if (global::PandoraSingleton<global::GameManager>.Instance.IsFastForwarded && this.unitCtrlr.unit.isAI)
		{
			global::UnityEngine.Time.timeScale = 1.5f;
		}
	}

	void global::ICheapState.Exit(int iTo)
	{
		global::PandoraSingleton<global::GameManager>.Instance.ResetTimeScale();
	}

	void global::ICheapState.Update()
	{
	}

	void global::ICheapState.FixedUpdate()
	{
	}

	private void SequenceDone()
	{
		if (this.unitCtrlr.unit.Status == global::UnitStateId.OUT_OF_ACTION)
		{
			this.unitCtrlr.KillUnit();
			this.unitCtrlr.SkillRPC(339);
		}
		else
		{
			this.unitCtrlr.transform.position = this.targetPos;
			this.unitCtrlr.SetFixed(true);
			this.unitCtrlr.CheckEngaged(true);
			if (global::PandoraSingleton<global::MissionManager>.Instance.IsCurrentPlayer())
			{
				for (int i = 0; i < global::PandoraSingleton<global::MissionManager>.Instance.zoneAoes.Count; i++)
				{
					global::PandoraSingleton<global::MissionManager>.Instance.zoneAoes[i].CheckEnterOrExitUnit(this.unitCtrlr, true);
				}
				this.unitCtrlr.SendAthleticFinished(this.prepareState.success, this.prepareState.actionId);
			}
			else
			{
				this.unitCtrlr.StateMachine.ChangeState(9);
			}
		}
	}

	private global::UnitController unitCtrlr;

	private global::UnityEngine.Vector3 targetPos;

	private global::PrepareAthletic prepareState;
}
