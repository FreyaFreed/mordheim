using System;
using System.Collections.Generic;

public class TurnStart : global::ICheapState
{
	public TurnStart(global::UnitController ctrlr)
	{
		this.unitCtrlr = ctrlr;
	}

	void global::ICheapState.Destroy()
	{
	}

	void global::ICheapState.Enter(int iFrom)
	{
		this.unitCtrlr.SetFixed(true);
		this.unitCtrlr.ResetAtkUsed();
		this.unitCtrlr.defenderCtrlr = null;
		this.unitCtrlr.Fleeing = false;
		this.unitCtrlr.TurnStarted = true;
		if (this.unitCtrlr.AICtrlr != null)
		{
			this.unitCtrlr.AICtrlr.TurnStartCleanUp();
		}
		this.unitCtrlr.defenderCtrlr = null;
		this.unitCtrlr.LastActivatedAction = null;
		this.circlesSet = false;
		global::PandoraSingleton<global::MissionManager>.Instance.SetCombatCircles(this.unitCtrlr, delegate
		{
			this.circlesSet = true;
		});
	}

	void global::ICheapState.Exit(int iTo)
	{
	}

	void global::ICheapState.Update()
	{
		if (!global::PandoraSingleton<global::MissionManager>.Instance.IsNavmeshUpdating && this.circlesSet)
		{
			this.unitCtrlr.UpdateTargetsData();
			if (!this.unitCtrlr.HasEnemyInSight())
			{
				this.unitCtrlr.unit.DestroyEnchantments(global::EnchantmentTriggerId.ON_NO_ENEMY_IN_SIGHT, false);
			}
			global::PandoraSingleton<global::MissionManager>.Instance.UpdateZoneAoeDurations(this.unitCtrlr);
			global::System.Collections.Generic.List<global::UnitController> allUnits = global::PandoraSingleton<global::MissionManager>.Instance.GetAllUnits();
			for (int i = 0; i < allUnits.Count; i++)
			{
				allUnits[i].unit.UpdateEnchantmentsDuration(this.unitCtrlr.unit, true);
			}
			this.unitCtrlr.unit.UpdateAttributes();
			this.unitCtrlr.unit.ResetPoints();
			global::PandoraSingleton<global::MissionManager>.Instance.TurnTimer.Reset(-1f);
			if (global::PandoraSingleton<global::Hermes>.Instance.IsHost())
			{
				global::PandoraSingleton<global::MissionManager>.Instance.interruptingUnit = null;
			}
			if (this.unitCtrlr.unit.Status != global::UnitStateId.OUT_OF_ACTION)
			{
				if (this.unitCtrlr.IsMine())
				{
					global::PandoraSingleton<global::MissionManager>.Instance.CamManager.SwitchToCam(global::CameraManager.CameraType.CHARACTER, this.unitCtrlr.transform, false, false, true, this.unitCtrlr.unit.Data.UnitSizeId == global::UnitSizeId.LARGE);
					if (global::PandoraSingleton<global::Hermes>.Instance.IsConnected())
					{
						this.unitCtrlr.StartSync();
					}
				}
				if (this.unitCtrlr.IsPlayed())
				{
					this.unitCtrlr.Imprint.SetCurrent(true);
				}
			}
			this.unitCtrlr.nextState = ((this.unitCtrlr.unit.Status == global::UnitStateId.OUT_OF_ACTION) ? global::UnitController.State.TURN_FINISHED : global::UnitController.State.TURN_MESSAGE);
		}
	}

	void global::ICheapState.FixedUpdate()
	{
	}

	private global::UnitController unitCtrlr;

	private bool circlesSet;
}
