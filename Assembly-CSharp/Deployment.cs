using System;
using System.Collections;
using System.Collections.Generic;
using Prometheus;
using UnityEngine;
using UnityEngine.Events;

public class Deployment : global::IMyrtilus, global::ICheapState
{
	public Deployment(global::MissionManager mission)
	{
		this.missionMngr = mission;
		this.RegisterToHermes();
	}

	void global::ICheapState.Destroy()
	{
		this.RemoveFromHermes();
	}

	void global::ICheapState.Enter(int iFrom)
	{
		global::PandoraDebug.LogDebug("Deployment Enter", "FLOW", null);
		this.synCount = 0;
		this.done = false;
		if (global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.isCampaign || global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.isTuto || global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.autoDeploy)
		{
			for (int i = 0; i < this.missionMngr.WarbandCtrlrs.Count; i++)
			{
				global::System.Collections.Generic.List<global::UnitController> unitCtrlrs = this.missionMngr.WarbandCtrlrs[i].unitCtrlrs;
				for (int j = 0; j < unitCtrlrs.Count; j++)
				{
					unitCtrlrs[j].Imprint.alwaysVisible = this.missionMngr.WarbandCtrlrs[i].IsPlayed();
					unitCtrlrs[j].Imprint.needsRefresh = true;
				}
			}
			this.StartRound();
			global::PandoraSingleton<global::MissionManager>.Instance.resendLadder = true;
			return;
		}
		global::PandoraSingleton<global::MissionManager>.Instance.isDeploying = true;
		this.lastCamTarget = this.missionMngr.CamManager.Target;
		global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice(global::Notices.MISSION_DEPLOY);
		this.deployIndex = -1;
		this.impressives = true;
		this.missionMngr.SetTurnTimer((float)global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.deployTimer, new global::UnityEngine.Events.UnityAction(this.OnTimerDone));
		this.fxs = new global::System.Collections.Generic.List<global::UnityEngine.GameObject>[this.missionMngr.WarbandCtrlrs.Count];
		for (int k = 0; k < this.missionMngr.WarbandCtrlrs.Count; k++)
		{
			this.fxs[k] = new global::System.Collections.Generic.List<global::UnityEngine.GameObject>();
			global::System.Collections.Generic.List<global::UnitController> unitCtrlrs2 = this.missionMngr.WarbandCtrlrs[k].unitCtrlrs;
			for (int l = 0; l < unitCtrlrs2.Count; l++)
			{
				unitCtrlrs2[l].Imprint.alwaysVisible = unitCtrlrs2[l].IsPlayed();
				unitCtrlrs2[l].Imprint.alwaysHide = !unitCtrlrs2[l].IsPlayed();
				if (unitCtrlrs2[l].IsPlayed())
				{
					unitCtrlrs2[l].Imprint.needsRefresh = true;
				}
				else
				{
					unitCtrlrs2[l].Imprint.Hide();
				}
				unitCtrlrs2[l].Hide(!unitCtrlrs2[l].IsPlayed(), true, null);
			}
			this.availableNodes = global::PandoraSingleton<global::MissionStartData>.Instance.spawnNodes[k];
			for (int m = 0; m < this.availableNodes.Count; m++)
			{
				if (this.missionMngr.WarbandCtrlrs[k].IsPlayed())
				{
					this.availableNodes[m].ShowImprint(true);
					global::UnityEngine.GameObject gameObject = global::UnityEngine.Object.Instantiate<global::UnityEngine.GameObject>(global::PandoraSingleton<global::MissionManager>.Instance.deployBeaconPrefab);
					gameObject.transform.SetParent(this.availableNodes[m].transform);
					gameObject.transform.localPosition = global::UnityEngine.Vector3.zero;
					gameObject.transform.localRotation = global::UnityEngine.Quaternion.identity;
					this.fxs[k].Add(gameObject);
				}
			}
		}
		global::PandoraSingleton<global::MissionManager>.Instance.resendLadder = true;
		this.NextUnitDeploy();
	}

	void global::ICheapState.Exit(int iTo)
	{
		if (global::PandoraSingleton<global::MissionStartData>.Instance.spawnNodes != null)
		{
			for (int i = 0; i < global::PandoraSingleton<global::MissionStartData>.Instance.spawnNodes.Length; i++)
			{
				global::System.Collections.Generic.List<global::SpawnNode> list = global::PandoraSingleton<global::MissionStartData>.Instance.spawnNodes[i];
				for (int j = 0; j < list.Count; j++)
				{
					global::SpawnNode spawnNode = list[j];
					if (spawnNode != null)
					{
						global::UnityEngine.Object.DestroyImmediate(spawnNode.gameObject);
					}
				}
			}
			global::PandoraSingleton<global::MissionStartData>.Instance.spawnNodes = null;
		}
		if (global::PandoraSingleton<global::MissionStartData>.Instance.spawnZones != null)
		{
			for (int k = 0; k < global::PandoraSingleton<global::MissionStartData>.Instance.spawnZones.Count; k++)
			{
				global::SpawnZone spawnZone = global::PandoraSingleton<global::MissionStartData>.Instance.spawnZones[k];
				if (spawnZone != null)
				{
					global::UnityEngine.Object.DestroyImmediate(spawnZone.gameObject);
				}
			}
			global::PandoraSingleton<global::MissionStartData>.Instance.spawnZones = null;
		}
		this.availableNodes = null;
		this.currentFxs = null;
		this.fxs = null;
		this.missionMngr.CreateMissionEndData();
		this.missionMngr.SetDepoyLadderIndex(-1);
		this.missionMngr.SetTurnTimer((float)global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.turnTimer, null);
		this.missionMngr.CamManager.SetZoomLevel(1U);
		this.missionMngr.InitFoW();
		if (this.pointerFx != null)
		{
			this.pointerFx.SetActive(false);
			global::UnityEngine.Object.Destroy(this.pointerFx);
			this.pointerFx = null;
		}
	}

	void global::ICheapState.Update()
	{
		if (this.done)
		{
			return;
		}
		if (global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp("overview", 0) && this.missionMngr.CamManager.GetCurrentCamType() != global::CameraManager.CameraType.OVERVIEW)
		{
			this.missionMngr.CamManager.SwitchToCam(global::CameraManager.CameraType.OVERVIEW, this.curUnitCtrl.transform, true, true, true, false);
		}
		else if ((global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp("overview", 6) || global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp("esc_cancel", 6)) && this.missionMngr.CamManager.GetCurrentCamType() == global::CameraManager.CameraType.OVERVIEW)
		{
			if (this.curUnitCtrl.IsPlayed())
			{
				this.missionMngr.CamManager.SwitchToCam(global::CameraManager.CameraType.DEPLOY, this.curUnitCtrl.transform, true, true, this.curUnitCtrl.unit.Data.UnitSizeId == global::UnitSizeId.LARGE, false);
			}
			else
			{
				this.missionMngr.CamManager.SwitchToCam(global::CameraManager.CameraType.DEPLOY, this.lastCamTarget, true, true, true, false);
			}
		}
		else if (this.curUnitCtrl != null && this.missionMngr.CamManager.GetCurrentCamType() != global::CameraManager.CameraType.OVERVIEW && this.curUnitCtrl.IsPlayed())
		{
			this.lastCamTarget = this.curUnitCtrl.transform;
			if (global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp("h", 0))
			{
				this.Send(true, global::Hermes.SendTarget.ALL, this.uid, 2U, new object[0]);
			}
			else if (global::PandoraSingleton<global::PandoraInput>.Instance.GetNegKeyUp("h", 0))
			{
				this.Send(true, global::Hermes.SendTarget.ALL, this.uid, 3U, new object[0]);
			}
			else if (global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp("action", 0))
			{
				this.Send(true, global::Hermes.SendTarget.ALL, this.uid, 1U, new object[0]);
			}
		}
	}

	void global::ICheapState.FixedUpdate()
	{
		if (this.done)
		{
			return;
		}
		if (this.curUnitCtrl != null)
		{
			this.curUnitCtrl.UpdateTargetsData();
			if (this.curUnitCtrl.IsPlayed())
			{
				global::PandoraSingleton<global::MissionManager>.Instance.RefreshFoWOwnMoving(this.curUnitCtrl);
			}
			else
			{
				global::PandoraSingleton<global::MissionManager>.Instance.RefreshFoWTargetMoving(this.curUnitCtrl);
			}
		}
	}

	private void StartRound()
	{
		this.done = true;
		this.missionMngr.StartCoroutine(this.FinalizeDeployment());
	}

	private global::System.Collections.IEnumerator FinalizeDeployment()
	{
		for (int i = 0; i < this.missionMngr.WarbandCtrlrs.Count; i++)
		{
			global::System.Collections.Generic.List<global::UnitController> units = this.missionMngr.WarbandCtrlrs[i].unitCtrlrs;
			for (int j = 0; j < units.Count; j++)
			{
				units[j].Deployed(false);
			}
		}
		for (int k = 0; k < this.missionMngr.WarbandCtrlrs.Count; k++)
		{
			global::System.Collections.Generic.List<global::UnitController> units = this.missionMngr.WarbandCtrlrs[k].unitCtrlrs;
			for (int l = 0; l < units.Count; l++)
			{
				units[l].UpdateTargetsData();
				units[l].CheckEngaged(false);
				yield return null;
			}
		}
		this.SendDeploymentFinishedRPC();
		yield break;
	}

	private void SendDeploymentFinishedRPC()
	{
		for (int i = 0; i < global::PandoraSingleton<global::MissionManager>.Instance.WarbandCtrlrs.Count; i++)
		{
			if (global::PandoraSingleton<global::MissionManager>.Instance.WarbandCtrlrs[i].playerIdx == global::PandoraSingleton<global::Hermes>.Instance.PlayerIndex)
			{
				this.Send(true, global::Hermes.SendTarget.ALL, this.uid, 4U, new object[0]);
			}
		}
	}

	private void DeploymentFinishedRPC()
	{
		this.synCount++;
		if (this.synCount != global::PandoraSingleton<global::MissionManager>.Instance.WarbandCtrlrs.Count)
		{
			return;
		}
		for (int i = 0; i < this.missionMngr.WarbandCtrlrs.Count; i++)
		{
			global::System.Collections.Generic.List<global::UnitController> unitCtrlrs = this.missionMngr.WarbandCtrlrs[i].unitCtrlrs;
			for (int j = 0; j < unitCtrlrs.Count; j++)
			{
				unitCtrlrs[j].TriggerEngagedEnchantments();
				unitCtrlrs[j].TriggerAlliesEnchantments();
			}
		}
		global::PandoraSingleton<global::MissionManager>.Instance.isDeploying = false;
		if (global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.isCampaign || global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.isTuto || global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.autoDeploy)
		{
			this.missionMngr.StateMachine.ChangeState(3);
			global::PandoraSingleton<global::TransitionManager>.Instance.SetGameLoadingDone(false);
		}
		else
		{
			this.missionMngr.StateMachine.ChangeState(2);
		}
	}

	public void OnTimerDone()
	{
		if (this.curUnitCtrl.IsPlayed())
		{
			this.Send(true, global::Hermes.SendTarget.ALL, this.uid, 1U, new object[0]);
		}
	}

	public void DeployHere()
	{
		this.availableNodes[this.spawnNodeIndex].claimed = true;
		if (this.pointerFx != null)
		{
			this.pointerFx.SetActive(false);
			global::UnityEngine.Object.Destroy(this.pointerFx);
			this.pointerFx = null;
		}
		this.NextUnitDeploy();
	}

	public void NextUnitDeploy()
	{
		if (!this.impressives)
		{
			for (;;)
			{
				this.deployIndex++;
				if (this.deployIndex >= this.missionMngr.InitiativeLadder.Count)
				{
					break;
				}
				this.curUnitCtrl = this.missionMngr.InitiativeLadder[this.deployIndex];
				if (!this.curUnitCtrl.unit.IsImpressive)
				{
					goto IL_E0;
				}
			}
			this.StartRound();
			return;
		}
		for (;;)
		{
			this.deployIndex++;
			if (this.deployIndex >= this.missionMngr.InitiativeLadder.Count)
			{
				break;
			}
			this.curUnitCtrl = this.missionMngr.InitiativeLadder[this.deployIndex];
			if (this.curUnitCtrl.unit.IsImpressive)
			{
				goto Block_2;
			}
		}
		this.deployIndex = -1;
		this.impressives = false;
		this.NextUnitDeploy();
		return;
		Block_2:
		IL_E0:
		this.curUnitCtrl = this.missionMngr.InitiativeLadder[this.deployIndex];
		this.missionMngr.SetDepoyLadderIndex(this.deployIndex);
		global::PandoraSingleton<global::UIMissionManager>.Instance.CurrentUnitController = this.curUnitCtrl;
		global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<global::UnitController>(global::Notices.CURRENT_UNIT_CHANGED, this.curUnitCtrl);
		global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<global::UnitController>(global::Notices.DEPLOY_UNIT, this.curUnitCtrl);
		this.availableNodes = global::PandoraSingleton<global::MissionStartData>.Instance.spawnNodes[this.curUnitCtrl.GetWarband().idx];
		this.currentFxs = this.fxs[this.curUnitCtrl.GetWarband().idx];
		this.missionMngr.TurnTimer.Reset(-1f);
		this.missionMngr.TurnTimer.Resume();
		this.spawnNodeIndex = -1;
		this.FindNextAvailableNode(1, this.curUnitCtrl.unit.IsImpressive);
		this.curUnitCtrl.Imprint.alwaysVisible = this.curUnitCtrl.IsPlayed();
		this.curUnitCtrl.Imprint.alwaysHide = false;
		this.curUnitCtrl.Imprint.needsRefresh = true;
		if (this.curUnitCtrl.IsPlayed())
		{
			if (this.curUnitCtrl.GetWarband().deploymentId == global::DeploymentId.SCATTERED)
			{
				this.DeployAutoUnit();
			}
			else
			{
				this.missionMngr.CamManager.SwitchToCam(global::CameraManager.CameraType.DEPLOY, this.curUnitCtrl.transform, true, true, this.curUnitCtrl.unit.Data.UnitSizeId == global::UnitSizeId.LARGE, false);
				global::PandoraSingleton<global::Prometheus.Prometheus>.Instance.SpawnFx("fx_arrow_perso_location_01", this.curUnitCtrl, null, delegate(global::UnityEngine.GameObject fx)
				{
					this.pointerFx = fx;
				}, default(global::UnityEngine.Vector3), default(global::UnityEngine.Vector3), null);
			}
		}
		else if (this.curUnitCtrl.GetWarband().playerTypeId == global::PlayerTypeId.PLAYER && this.curUnitCtrl.GetWarband().deploymentId == global::DeploymentId.SCATTERED)
		{
			this.DeployAutoUnit();
		}
		else if (this.curUnitCtrl.AICtrlr != null)
		{
			this.DeployAutoUnit();
		}
		else if (this.missionMngr.CamManager.GetCurrentCamType() != global::CameraManager.CameraType.OVERVIEW)
		{
			this.missionMngr.CamManager.SwitchToCam(global::CameraManager.CameraType.DEPLOY, this.lastCamTarget, true, true, true, false);
		}
	}

	public void DeployAutoUnit()
	{
		bool isImpressive = this.curUnitCtrl.unit.IsImpressive;
		bool flag = this.curUnitCtrl.unit.CampaignData == null && this.curUnitCtrl.unit.Data.UnitTypeId == global::UnitTypeId.MONSTER;
		global::System.Collections.Generic.List<int> list = new global::System.Collections.Generic.List<int>();
		for (int i = 0; i < this.availableNodes.Count; i++)
		{
			if (flag)
			{
				if (this.availableNodes[i].IsOfType(global::SpawnNodeId.ROAMING) && !this.availableNodes[i].claimed)
				{
					list.Add(i);
				}
			}
			else if (isImpressive)
			{
				if (this.availableNodes[i].IsOfType(global::SpawnNodeId.IMPRESSIVE) && !this.availableNodes[i].claimed)
				{
					list.Add(i);
				}
			}
			else if (!this.availableNodes[i].claimed)
			{
				list.Add(i);
			}
		}
		int index = this.missionMngr.NetworkTyche.Rand(0, list.Count);
		this.spawnNodeIndex = list[index];
		if (this.currentFxs.Count > 0)
		{
			this.currentFxs[this.spawnNodeIndex].SetActive(false);
		}
		this.curUnitCtrl.transform.rotation = this.availableNodes[this.spawnNodeIndex].transform.rotation;
		this.curUnitCtrl.SetFixed(this.availableNodes[this.spawnNodeIndex].transform.position, true);
		this.DeployHere();
		list.Clear();
	}

	public void FindNextAvailableNode(int dir, bool impressive)
	{
		int num = this.spawnNodeIndex;
		bool flag = false;
		while (!flag)
		{
			this.spawnNodeIndex += dir;
			if (this.spawnNodeIndex >= this.availableNodes.Count)
			{
				this.spawnNodeIndex = 0;
			}
			else if (this.spawnNodeIndex < 0)
			{
				this.spawnNodeIndex = this.availableNodes.Count - 1;
			}
			if (num == this.spawnNodeIndex)
			{
				flag = true;
			}
			if (!this.availableNodes[this.spawnNodeIndex].claimed)
			{
				if (this.availableNodes[this.spawnNodeIndex].IsOfType(global::SpawnNodeId.IMPRESSIVE) && impressive)
				{
					break;
				}
				if (!impressive)
				{
					break;
				}
			}
		}
		if (this.currentFxs.Count > 0)
		{
			if (num != -1)
			{
				if (this.currentFxs[num] != null)
				{
					this.currentFxs[num].SetActive(true);
				}
			}
			this.currentFxs[this.spawnNodeIndex].SetActive(false);
		}
		this.curUnitCtrl.transform.rotation = this.availableNodes[this.spawnNodeIndex].transform.rotation;
		this.curUnitCtrl.SetFixed(this.availableNodes[this.spawnNodeIndex].transform.position, true);
		if (this.curUnitCtrl.IsPlayed())
		{
			global::PandoraSingleton<global::MissionManager>.Instance.CamManager.Transition(2f, true);
		}
	}

	public uint uid { get; set; }

	public uint owner { get; set; }

	public void RegisterToHermes()
	{
		global::PandoraSingleton<global::Hermes>.Instance.RegisterMyrtilus(this, true);
	}

	public void RemoveFromHermes()
	{
		global::PandoraSingleton<global::Hermes>.Instance.RemoveMyrtilus(this);
	}

	public void Send(bool reliable, global::Hermes.SendTarget target, uint id, uint command, params object[] parms)
	{
		global::PandoraSingleton<global::Hermes>.Instance.Send(reliable, target, id, command, parms);
	}

	public void Receive(ulong from, uint command, object[] parms)
	{
		switch (command)
		{
		case 1U:
			this.DeployHere();
			break;
		case 2U:
			this.FindNextAvailableNode(1, this.curUnitCtrl.unit.IsImpressive);
			break;
		case 3U:
			this.FindNextAvailableNode(-1, this.curUnitCtrl.unit.IsImpressive);
			break;
		case 4U:
			this.DeploymentFinishedRPC();
			break;
		}
	}

	private global::MissionManager missionMngr;

	private global::UnitController curUnitCtrl;

	private int spawnNodeIndex = -1;

	private global::System.Collections.Generic.List<global::SpawnNode> availableNodes;

	private global::System.Collections.Generic.List<global::UnityEngine.GameObject> currentFxs;

	private global::System.Collections.Generic.List<global::UnityEngine.GameObject>[] fxs;

	private int deployIndex;

	private bool impressives;

	private global::UnityEngine.Transform lastCamTarget;

	private global::UnityEngine.GameObject pointerFx;

	private bool done;

	private int synCount;

	private enum CommandList
	{
		NONE,
		DEPLOY_HERE,
		FORWARD,
		BACKWARD,
		DEPLOY_FINISHED,
		COUNT
	}
}
