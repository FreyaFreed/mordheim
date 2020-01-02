using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MainMenuController : global::UIStateMachineMonoBehaviour
{
	private void Awake()
	{
		global::PandoraSingleton<global::UIStateMachineMonoBehaviour>.instance = base.GetComponent<global::MainMenuController>();
		global::PandoraDebug.LogInfo("Welcome to Mordheim V 1.4.4.4", "INIT", this);
		global::DG.Tweening.DOTween.Init(new bool?(false), new bool?(true), null);
		global::PandoraSingleton<global::GameManager>.Instance.campaign = -1;
		global::PandoraSingleton<global::GameManager>.Instance.currentSave = null;
		this.camManager = global::UnityEngine.Camera.main.GetComponent<global::CameraManager>();
		global::CameraSetter component = base.GetComponent<global::CameraSetter>();
		component.SetCameraInfo(this.camManager.GetComponent<global::UnityEngine.Camera>());
		global::PandoraSingleton<global::NoticeManager>.Instance.RegisterListener(global::Notices.GAME_LOADED, new global::DelReceiveNotice(this.OnCampaignLoad));
		global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadAsync("loading");
	}

	private void OnDestroy()
	{
	}

	private void Start()
	{
		base.StartCoroutine(this.WaitForHephaestus());
	}

	private global::System.Collections.IEnumerator WaitForHephaestus()
	{
		while (!global::PandoraSingleton<global::Hephaestus>.Instance.IsInitialized())
		{
			yield return null;
		}
		global::PandoraSingleton<global::Hephaestus>.Instance.RefreshSaveData(new global::Hephaestus.OnSaveDataRefreshed(this.WaitForCopyright));
		yield break;
	}

	private global::System.Collections.IEnumerator WaitForCopyrightAsync()
	{
		global::PandoraSingleton<global::TransitionManager>.Instance.SetGameLoadingDone(false);
		while (global::PandoraSingleton<global::GameManager>.Instance.inCopyright || global::PandoraSingleton<global::TransitionManager>.Instance.IsLoading())
		{
			yield return null;
		}
		global::PandoraSingleton<global::Hermes>.Instance.StopConnections(true);
		global::PandoraSingleton<global::Pan>.Instance.PlayMusic();
		global::PandoraSingleton<global::PandoraInput>.Instance.SetCurrentState(global::PandoraInput.States.MENU, true);
		base.ChangeState(0);
		this.camManager.SetDOFTarget(this.dofTarget, 0f);
		global::PandoraSingleton<global::Hephaestus>.Instance.SetRichPresence(global::Hephaestus.RichPresenceId.MAIN_MENU, true);
		yield break;
	}

	private void WaitForCopyright()
	{
		base.StartCoroutine(this.WaitForCopyrightAsync());
	}

	protected override void Update()
	{
		bool inCopyright = global::PandoraSingleton<global::GameManager>.Instance.inCopyright;
		bool flag = global::PandoraSingleton<global::Hephaestus>.Instance.IsJoiningInvite();
		bool flag2 = global::PandoraSingleton<global::Hephaestus>.Instance.IsPlayTogether();
		if (!inCopyright && base.CurrentState != null && !global::PandoraSingleton<global::TransitionManager>.Instance.IsLoading() && base.CurrentState.StateId != 6 && (base.NextState == null || base.NextState.StateId != 6) && (flag || flag2))
		{
			if (global::PandoraSingleton<global::GameManager>.Instance.Save.HasCampaigns())
			{
				if (this.ContinuePopup != null && this.ContinuePopup.IsVisible)
				{
					this.ContinuePopup.Hide();
				}
				this.ChangeState(global::MainMenuController.State.LOAD_CAMPAIGN);
			}
			else if (flag)
			{
				global::PandoraSingleton<global::Hephaestus>.Instance.ResetInvite();
				global::PandoraSingleton<global::GameManager>.Instance.ShowSystemPopup(global::GameManager.SystemPopupId.INVITE, "invite_no_warband_title", "invite_no_warband_desc", null, false);
			}
			else if (flag2)
			{
				global::PandoraSingleton<global::Hephaestus>.Instance.ResetPlayTogether(true);
				global::PandoraSingleton<global::GameManager>.Instance.ShowSystemPopup(global::GameManager.SystemPopupId.INVITE, "play_together_no_warband_title", "play_together_no_warband_desc", null, false);
			}
		}
		base.Update();
		if (global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyDown("hide_gui", -1))
		{
			this.uiContainer.enabled = !this.uiContainer.enabled;
		}
	}

	private void OnCampaignLoad()
	{
		if (global::PandoraSingleton<global::Hephaestus>.Instance.IsJoiningInvite())
		{
			global::Warband warband = new global::Warband(global::PandoraSingleton<global::GameManager>.Instance.currentSave);
			if (warband.ValidateWarbandForInvite(false))
			{
				global::SceneLauncher.Instance.LaunchScene(global::SceneLoadingId.LAUNCH_HIDEOUT, false, true);
			}
		}
		else if (global::PandoraSingleton<global::GameManager>.Instance.currentSave.inMission)
		{
			global::MissionEndDataSave missionEndDataSave = global::PandoraSingleton<global::GameManager>.Instance.currentSave.endMission;
			if (missionEndDataSave.lastVersion < 11)
			{
				global::PandoraSingleton<global::GameManager>.Instance.currentSave.inMission = false;
				missionEndDataSave = new global::MissionEndDataSave();
				global::SceneLauncher.Instance.LaunchScene(global::SceneLoadingId.LAUNCH_HIDEOUT, false, true);
			}
			else if (global::PandoraSingleton<global::GameManager>.Instance.currentSave.endMission.myrtilusLadder.Count == 0)
			{
				global::PandoraSingleton<global::GameManager>.Instance.currentSave.inMission = false;
				global::PandoraSingleton<global::GameManager>.Instance.currentSave.endMission = null;
				global::PandoraSingleton<global::GameManager>.Instance.Save.SaveCampaign(global::PandoraSingleton<global::GameManager>.Instance.currentSave, global::PandoraSingleton<global::GameManager>.Instance.campaign);
				this.ConfirmPopup.Show("mission_progress_reset_title", "mission_progress_reset_desc", delegate(bool confirm)
				{
					global::SceneLauncher.Instance.LaunchScene(global::SceneLoadingId.LAUNCH_HIDEOUT, false, true);
				}, false, true);
			}
			else if (missionEndDataSave.isVsAI && !missionEndDataSave.isSkirmish && !missionEndDataSave.missionFinished)
			{
				this.ContinuePopup.Show("popup_load_fallback_title", "popup_load_fallback_desc", new global::System.Action<bool>(this.ResumeOrAbandonMission));
				this.ContinuePopup.abandonButton.SetAction(string.Empty, (!missionEndDataSave.routable) ? "menu_quit_mission_title" : "menu_quit_mission_voluntary_rout_title", 0, false, null, null);
				this.ContinuePopup.cancelButton.SetSelected(false);
			}
			else
			{
				this.AbandonMission();
			}
		}
		else
		{
			global::SceneLauncher.Instance.LaunchScene(global::SceneLoadingId.LAUNCH_HIDEOUT, false, true);
		}
	}

	private void ResumeOrAbandonMission(bool resume)
	{
		global::PandoraSingleton<global::NoticeManager>.Instance.RemoveListener(global::Notices.GAME_LOADED, new global::DelReceiveNotice(this.OnCampaignLoad));
		if (resume)
		{
			this.ResumeMission();
		}
		else
		{
			this.AbandonMission();
		}
	}

	private void ResumeMission()
	{
		global::MissionEndDataSave endMission = global::PandoraSingleton<global::GameManager>.Instance.currentSave.endMission;
		if (!global::PandoraSingleton<global::MissionStartData>.Exists())
		{
			global::UnityEngine.GameObject gameObject = new global::UnityEngine.GameObject("mission_start_data");
			gameObject.AddComponent<global::MissionStartData>();
		}
		else
		{
			global::PandoraSingleton<global::MissionStartData>.Instance.Clear();
		}
		global::PandoraSingleton<global::MissionStartData>.Instance.ReloadMission(endMission, global::PandoraSingleton<global::GameManager>.Instance.currentSave);
		if (global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.isCampaign)
		{
			global::SceneLauncher.Instance.LaunchScene(global::SceneLoadingId.LAUNCH_MISSION_CAMPAIGN, false, true);
		}
		else
		{
			global::SceneLauncher.Instance.LaunchScene(global::SceneLoadingId.LAUNCH_MISSION, false, true);
		}
	}

	private void AbandonMission()
	{
		global::MissionEndDataSave endMission = global::PandoraSingleton<global::GameManager>.Instance.currentSave.endMission;
		if (endMission.VictoryType == global::VictoryTypeId.LOSS && !endMission.routable)
		{
			endMission.crushed = true;
			for (int i = 0; i < endMission.units.Count; i++)
			{
				endMission.units[i].status = global::UnitStateId.OUT_OF_ACTION;
			}
		}
		if (!endMission.isVsAI)
		{
			global::PandoraSingleton<global::Hephaestus>.Instance.UnlockAchievement(global::Hephaestus.TrophyId.ALTF4);
		}
		global::SceneLauncher.Instance.LaunchScene(global::SceneLoadingId.LAUNCH_END_GAME, false, true);
	}

	public void ChangeState(global::MainMenuController.State state)
	{
		base.ChangeState((int)state);
	}

	public const string VERSION = "1.4.4.4";

	public global::ConfirmationPopupView ConfirmPopup;

	public global::ContinuePopupView ContinuePopup;

	public global::System.Collections.Generic.List<global::ButtonMapView> buttons = new global::System.Collections.Generic.List<global::ButtonMapView>();

	public global::CanvasGroupDisabler uiContainer;

	public global::CameraManager camManager;

	public global::UnityEngine.Transform dofTarget;

	public global::UnityEngine.GameObject environment;

	[global::System.Serializable]
	public enum State
	{
		MAIN_MENU,
		TUTORIALS,
		STORE,
		OPTIONS,
		CREDITS,
		NEW_CAMPAIGN,
		LOAD_CAMPAIGN,
		NB_STATE
	}

	public enum InputLayer
	{
		BASE,
		POP_UP
	}
}
