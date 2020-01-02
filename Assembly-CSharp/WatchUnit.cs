using System;
using System.Collections.Generic;
using UnityEngine;

public class WatchUnit : global::ICheapState
{
	public WatchUnit(global::MissionManager mission)
	{
		this.missionMngr = mission;
	}

	void global::ICheapState.Destroy()
	{
		this.missionMngr = null;
	}

	void global::ICheapState.Enter(int iFrom)
	{
		this.units = this.missionMngr.GetMyAliveUnits();
		if (this.units.Count == 0)
		{
			this.units = this.missionMngr.GetAllMyUnits();
		}
		this.curUnit = 0;
		this.curUnitCtrl = this.units[this.curUnit];
		this.curUnitCtrl = global::PandoraSingleton<global::MissionManager>.Instance.GetLastPlayedAliveUnit(this.units[this.curUnit].unit.warbandIdx);
		for (int i = 0; i < this.units.Count; i++)
		{
			if (this.curUnitCtrl == this.units[i])
			{
				this.curUnit = i;
				break;
			}
		}
		if (this.curUnitCtrl == null)
		{
			this.curUnitCtrl = this.units[this.curUnit];
		}
		this.watchCam = this.missionMngr.CamManager.GetCamOfType<global::WatchCamera>(global::CameraManager.CameraType.WATCH);
		if (global::PandoraSingleton<global::MissionManager>.Instance.lastWarbandIdx == -1 || global::PandoraSingleton<global::MissionManager>.Instance.lastWarbandIdx == global::PandoraSingleton<global::MissionManager>.Instance.GetMyWarbandCtrlr().idx)
		{
			if (this.missionMngr.GetCurrentUnit().IsImprintVisible())
			{
				this.missionMngr.CamManager.SwitchToCam(global::CameraManager.CameraType.WATCH, null, true, true, this.curUnitCtrl.unit.Data.UnitSizeId == global::UnitSizeId.LARGE, false);
			}
			else
			{
				this.missionMngr.CamManager.SwitchToCam(global::CameraManager.CameraType.CHARACTER, this.curUnitCtrl.transform, true, true, true, this.curUnitCtrl.unit.Data.UnitSizeId == global::UnitSizeId.LARGE);
			}
		}
		this.seeTime = 1f;
	}

	void global::ICheapState.Exit(int iTo)
	{
	}

	void global::ICheapState.Update()
	{
		if (this.missionMngr.CheckUnitTurnFinished())
		{
			return;
		}
		if (this.missionMngr.CamManager.GetCurrentCamType() != global::CameraManager.CameraType.WATCH && this.missionMngr.CamManager.GetCurrentCamType() != global::CameraManager.CameraType.ANIMATED && this.missionMngr.CamManager.GetCurrentCamType() != global::CameraManager.CameraType.CONSTRAINED && this.missionMngr.CamManager.GetCurrentCamType() != global::CameraManager.CameraType.SEMI_CONSTRAINED && this.missionMngr.CamManager.GetCurrentCamType() != global::CameraManager.CameraType.MELEE_ATTACK)
		{
			if (global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp("overview", 0) && this.missionMngr.CamManager.GetCurrentCamType() != global::CameraManager.CameraType.OVERVIEW)
			{
				this.missionMngr.CamManager.SwitchToCam(global::CameraManager.CameraType.OVERVIEW, this.curUnitCtrl.transform, true, true, true, false);
			}
			else if ((global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp("overview", 6) || global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp("esc_cancel", 6)) && this.missionMngr.CamManager.GetCurrentCamType() == global::CameraManager.CameraType.OVERVIEW)
			{
				this.missionMngr.CamManager.SwitchToCam(global::CameraManager.CameraType.CHARACTER, this.curUnitCtrl.transform, true, true, true, this.curUnitCtrl.unit.Data.UnitSizeId == global::UnitSizeId.LARGE);
			}
			else if (this.units.Count > 1 && this.missionMngr.CamManager.GetCurrentCamType() != global::CameraManager.CameraType.OVERVIEW)
			{
				if (global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp("h", 0))
				{
					if (++this.curUnit >= this.units.Count)
					{
						this.curUnit = 0;
					}
					this.curUnitCtrl = this.units[this.curUnit];
					this.missionMngr.CamManager.SwitchToCam(global::CameraManager.CameraType.CHARACTER, this.curUnitCtrl.transform, true, true, true, this.curUnitCtrl.unit.Data.UnitSizeId == global::UnitSizeId.LARGE);
				}
				else if (global::PandoraSingleton<global::PandoraInput>.Instance.GetNegKeyUp("h", 0))
				{
					if (--this.curUnit < 0)
					{
						this.curUnit = this.units.Count - 1;
					}
					this.curUnitCtrl = this.units[this.curUnit];
					this.missionMngr.CamManager.SwitchToCam(global::CameraManager.CameraType.CHARACTER, this.curUnitCtrl.transform, true, true, true, this.curUnitCtrl.unit.Data.UnitSizeId == global::UnitSizeId.LARGE);
				}
			}
		}
		if (this.missionMngr.GetCurrentUnit().IsImprintVisible())
		{
			if (this.seeTime < -0.1f)
			{
				this.seeTime = 0f;
			}
			this.seeTime += global::UnityEngine.Time.deltaTime;
		}
		else if (this.seeTime > 0.1f)
		{
			this.seeTime = 0f;
		}
		else
		{
			this.seeTime -= global::UnityEngine.Time.deltaTime;
		}
		if (this.seeTime > 0.5f && ((this.missionMngr.CamManager.GetCurrentCamType() != global::CameraManager.CameraType.ANIMATED && this.missionMngr.CamManager.GetCurrentCamType() != global::CameraManager.CameraType.CONSTRAINED && this.missionMngr.CamManager.GetCurrentCamType() != global::CameraManager.CameraType.SEMI_CONSTRAINED && this.missionMngr.CamManager.GetCurrentCamType() != global::CameraManager.CameraType.MELEE_ATTACK) || !global::PandoraSingleton<global::SequenceManager>.Instance.isPlaying))
		{
			if (((this.missionMngr.CamManager.GetCurrentCamType() == global::CameraManager.CameraType.OVERVIEW && global::PandoraSingleton<global::GameManager>.Instance.AutoExitTacticalEnabled) || this.missionMngr.CamManager.GetCurrentCamType() != global::CameraManager.CameraType.OVERVIEW) && this.missionMngr.CamManager.GetCurrentCamType() != global::CameraManager.CameraType.WATCH)
			{
				this.missionMngr.CamManager.SwitchToCam(global::CameraManager.CameraType.WATCH, null, true, true, true, false);
			}
		}
		else if (this.seeTime < -1f && this.missionMngr.CamManager.GetCurrentCamType() != global::CameraManager.CameraType.OVERVIEW && this.missionMngr.CamManager.GetCurrentCamType() != global::CameraManager.CameraType.CHARACTER)
		{
			global::UnitController lastWatcher = this.watchCam.lastWatcher;
			for (int i = 0; i < this.units.Count; i++)
			{
				if (lastWatcher == this.units[i])
				{
					this.curUnit = i;
					this.curUnitCtrl = this.units[i];
					break;
				}
			}
			this.missionMngr.CamManager.SwitchToCam(global::CameraManager.CameraType.CHARACTER, this.curUnitCtrl.transform, true, true, true, this.curUnitCtrl.unit.Data.UnitSizeId == global::UnitSizeId.LARGE);
		}
	}

	void global::ICheapState.FixedUpdate()
	{
	}

	private const float SEE_TIME = 0.5f;

	private const float NO_SEE_TIME = -1f;

	private global::MissionManager missionMngr;

	private global::System.Collections.Generic.List<global::UnitController> units;

	private global::UnitController curUnitCtrl;

	private int curUnit;

	private float seeTime;

	private global::WatchCamera watchCam;
}
