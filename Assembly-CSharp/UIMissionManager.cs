using System;
using System.Collections.Generic;
using UnityEngine;

public class UIMissionManager : global::PandoraSingleton<global::UIMissionManager>
{
	public global::UIMissionManager.UnitInfoState ShowingMoreInfoUnit { get; private set; }

	public bool ShowingMoreInfoUnitAction { get; private set; }

	public bool ShowingMoreInfoMission { get; private set; }

	public bool ShowingOverview { get; set; }

	[global::UnityEngine.HideInInspector]
	public global::UnitController CurrentUnitController
	{
		get
		{
			return this.currentUnitController;
		}
		set
		{
			this.currentUnitController = value;
			if (this.currentUnitController != null)
			{
				for (int i = 0; i < this.currentUnitUI.Count; i++)
				{
					this.currentUnitUI[i].UnitChanged(this.currentUnitController, null, null);
				}
			}
		}
	}

	[global::UnityEngine.HideInInspector]
	public global::UnitController CurrentUnitTargetController
	{
		get
		{
			return this.currentUnitTargetController;
		}
		set
		{
			this.currentUnitTargetController = value;
			if (this.currentUnitTargetController != null)
			{
				this.currentUnitTargetDestructible = null;
				for (int i = 0; i < this.currentUnitTargetUI.Count; i++)
				{
					this.currentUnitTargetUI[i].UnitChanged(this.currentUnitTargetController, this.currentUnitController, null);
				}
			}
		}
	}

	[global::UnityEngine.HideInInspector]
	public global::Destructible CurrentUnitTargetDestructible
	{
		get
		{
			return this.currentUnitTargetDestructible;
		}
		set
		{
			this.currentUnitTargetDestructible = value;
			if (this.currentUnitTargetDestructible != null)
			{
				this.currentUnitTargetController = null;
				for (int i = 0; i < this.currentUnitTargetUI.Count; i++)
				{
					this.currentUnitTargetUI[i].UnitChanged(this.currentUnitTargetController, this.currentUnitController, this.currentUnitTargetDestructible);
				}
			}
		}
	}

	private void Awake()
	{
		this.optionsMenu = global::UnityEngine.Object.Instantiate<global::UnityEngine.GameObject>(this.optionsMenu);
		this.optionsMan = this.optionsMenu.GetComponentsInChildren<global::OptionsManager>(true)[0];
		this.optionsMan.onCloseOptionsMenu = new global::System.Action(this.HideOptions);
		this.optionsMan.onQuitGame = new global::System.Action<bool>(this.OnQuitGame);
		this.optionsMan.onSaveQuitGame = new global::System.Action(this.OnSaveAndQuit);
		this.optionsMan.SetBackButtonLoc("menu_back_to_game");
		int num = 0;
		for (int i = 0; i < global::PandoraSingleton<global::MissionStartData>.Instance.FightingWarbands.Count; i++)
		{
			if (global::PandoraSingleton<global::MissionStartData>.Instance.FightingWarbands[i].PlayerTypeId == global::PlayerTypeId.PLAYER)
			{
				num++;
			}
		}
		if (global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.isTuto)
		{
			this.optionsMan.SetQuitButtonLoc("menu_quit_tutorial", string.Empty);
			this.optionsMan.HideAltQuitOption();
			this.optionsMan.HideSaveAndQuitOption();
		}
		else if (!global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.isCampaign && (global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.isSkirmish || num > 1))
		{
			this.optionsMan.DisableSaveAndQuitOption();
		}
		this.quitGamePopup.transform.SetParent(this.optionsMenu.transform);
		this.quitGamePopup.transform.localPosition = global::UnityEngine.Vector3.zero;
		this.quitGamePopup.transform.localScale = global::UnityEngine.Vector3.one;
		this.canvasDisabler = base.GetComponent<global::CanvasGroupDisabler>();
		this.audioSource = base.GetComponent<global::UnityEngine.AudioSource>();
		this.StateMachine = new global::CheapStateMachine(7);
		this.StateMachine.AddState(new global::UIMissionMoving(this), 0);
		this.StateMachine.AddState(new global::UIMissionTarget(this), 1);
		this.StateMachine.AddState(new global::UIMissionSequence(this), 2);
		this.StateMachine.AddState(new global::UIMissionSearch(this), 4);
		this.StateMachine.AddState(new global::UIMissionEndGame(this), 5);
		this.StateMachine.AddState(new global::UIMissionDeploy(this), 6);
	}

	private void Start()
	{
		((global::UnityEngine.RectTransform)base.transform).localScale = global::UnityEngine.Vector3.one;
		this.optionsMenu.SetActive(false);
		global::PandoraSingleton<global::NoticeManager>.Instance.RegisterListener(global::Notices.UNIT_START_MOVE, delegate
		{
			this.ChangeState(global::UIMissionManager.State.MOVING);
		});
		global::PandoraSingleton<global::NoticeManager>.Instance.RegisterListener(global::Notices.MISSION_INVENTORY, delegate
		{
			this.ChangeState(global::UIMissionManager.State.SEARCH);
		});
		global::PandoraSingleton<global::NoticeManager>.Instance.RegisterListener(global::Notices.UNIT_START_SINGLE_TARGETING, delegate
		{
			this.ChangeState(global::UIMissionManager.State.SINGLE_TARGETING);
		});
		global::PandoraSingleton<global::NoticeManager>.Instance.RegisterListener(global::Notices.MISSION_END, delegate
		{
			this.ChangeState(global::UIMissionManager.State.ENDGAME);
		});
		global::PandoraSingleton<global::NoticeManager>.Instance.RegisterListener(global::Notices.MISSION_DEPLOY, delegate
		{
			this.ChangeState(global::UIMissionManager.State.DEPLOY);
		});
		global::PandoraSingleton<global::NoticeManager>.Instance.RegisterListener(global::Notices.CURRENT_UNIT_CHANGED, new global::DelReceiveNotice(this.OnCurrentUnitChanged));
		global::PandoraSingleton<global::NoticeManager>.Instance.RegisterListener(global::Notices.CURRENT_UNIT_TARGET_CHANGED, new global::DelReceiveNotice(this.OnCurrentUnitTargetChanged));
		global::PandoraSingleton<global::NoticeManager>.Instance.RegisterListener(global::Notices.CURRENT_UNIT_TARGET_DESTUCTIBLE_CHANGED, new global::DelReceiveNotice(this.OnCurrentTargetDestructibleChanged));
		global::PandoraSingleton<global::NoticeManager>.Instance.RegisterListener(global::Notices.SEQUENCE_STARTED, new global::DelReceiveNotice(this.OnSequenceStarted));
		this.ladder.OnDisable();
		this.wheel.OnDisable();
		this.turnMessage.OnDisable();
		this.morale.OnDisable();
		this.leftSequenceMessage.OnDisable();
		this.rightSequenceMessage.OnDisable();
		this.objectives.OnDisable();
		this.endGameReport.gameObject.SetActive(false);
		this.unitCombatStats.OnDisable();
		this.unitAction.OnDisable();
		this.unitAlternateWeapon.OnDisable();
		this.unitEnchantments.OnDisable();
		this.unitEnchantmentsDebuffs.OnDisable();
		this.unitStats.OnDisable();
		this.targetCombatStats.OnDisable();
		this.targetAlternateWeapon.OnDisable();
		this.targetEnchantments.OnDisable();
		this.targetEnchantmentsDebuffs.OnDisable();
		this.targetStats.OnDisable();
		this.inventory.OnDisable();
		this.chatLog.Setup();
		this.chatLog.OnDisable();
		this.overview.OnDisable();
		this.propsInfo.OnDisable();
		this.deployControls.OnDisable();
		this.ShowingMoreInfoMission = true;
		this.OnSetOptionFullUI(false);
	}

	public void OnSetOptionFullUI(bool resetUI)
	{
		if (global::PandoraSingleton<global::GameManager>.Instance.Options.displayFullUI)
		{
			this.ShowingMoreInfoUnit = global::UIMissionManager.UnitInfoState.BUFFS;
			this.ShowingMoreInfoUnitAction = true;
			this.chatLog.ShowLog();
		}
		else
		{
			this.ShowingMoreInfoUnit = global::UIMissionManager.UnitInfoState.NONE;
			this.ShowingMoreInfoUnitAction = false;
			this.chatLog.HideLog();
		}
		if (resetUI)
		{
			global::PandoraSingleton<global::NoticeManager>.instance.SendNotice<bool>(global::Notices.GAME_MORE_INFO_UNIT_ACTION_TOGGLE, this.ShowingMoreInfoUnitAction);
			this.ShowUnitExtraStats();
			this.ShowTargetExtraStats();
			this.ShowObjectives();
		}
	}

	public void OnDestroy()
	{
		this.StateMachine.Destroy();
	}

	private void ChangeState(global::UIMissionManager.State state)
	{
		int activeStateId = this.StateMachine.GetActiveStateId();
		if (activeStateId == 5)
		{
			global::PandoraDebug.LogWarning("Trying to enter state " + state.ToString() + " while in state endgame", "uncategorised", null);
		}
		else
		{
			this.StateMachine.ChangeState((int)state);
		}
	}

	private void OnSequenceStarted()
	{
		this.ChangeState(global::UIMissionManager.State.SEQUENCE);
	}

	private void OnCurrentUnitChanged()
	{
		this.CurrentUnitController = (global::UnitController)global::PandoraSingleton<global::NoticeManager>.Instance.Parameters[0];
		if (this.CurrentUnitController == global::PandoraSingleton<global::MissionManager>.Instance.GetCurrentUnit())
		{
			global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<global::UnitController>(global::Notices.LADDER_UNIT_CHANGED, this.CurrentUnitController);
		}
	}

	private void OnCurrentUnitTargetChanged()
	{
		this.CurrentUnitTargetController = (global::UnitController)global::PandoraSingleton<global::NoticeManager>.Instance.Parameters[0];
	}

	private void OnCurrentTargetDestructibleChanged()
	{
		this.CurrentUnitTargetDestructible = (global::Destructible)global::PandoraSingleton<global::NoticeManager>.Instance.Parameters[0];
	}

	public bool IsNoticeCurrentUnitController()
	{
		global::UnitController y = (global::UnitController)global::PandoraSingleton<global::NoticeManager>.Instance.Parameters[0];
		return this.CurrentUnitController == y;
	}

	private void Update()
	{
		this.StateMachine.Update();
		if (this.StateMachine.GetActiveStateId() != 2 && this.StateMachine.GetActiveStateId() != 6)
		{
			if (global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp("menu", 0))
			{
				if (global::PandoraSingleton<global::PandoraInput>.Instance.lastInputMode == global::PandoraInput.InputMode.JOYSTICK || this.CurrentUnitController == null || !this.CurrentUnitController.IsPlayed() || (!this.CurrentUnitController.IsCurrentState(global::UnitController.State.AOE_TARGETING) && !this.CurrentUnitController.IsCurrentState(global::UnitController.State.SINGLE_TARGETING) && !this.CurrentUnitController.IsCurrentState(global::UnitController.State.LINE_TARGETING) && !this.CurrentUnitController.IsCurrentState(global::UnitController.State.COUNTER_CHOICE) && !this.CurrentUnitController.IsCurrentState(global::UnitController.State.INTERACTIVE_TARGET) && this.StateMachine.GetActiveStateId() == 0 && (this.CurrentUnitController.CurrentAction == null || !this.CurrentUnitController.CurrentAction.waitForConfirmation)))
				{
					this.ShowOptions();
				}
			}
			else if (global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp("wheel", 0) && this.CurrentUnitController != null && !this.CurrentUnitController.CurrentAction.waitForConfirmation && (this.CurrentUnitController.IsCurrentState(global::UnitController.State.MOVE) || this.CurrentUnitController.IsCurrentState(global::UnitController.State.ENGAGED)))
			{
				this.wheel.Show();
			}
		}
		if (global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp("hide_gui", -1))
		{
			this.ToggleUI();
		}
		if (global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp("show_chat", 0) || global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp("show_chat", 3))
		{
			this.ShowChat();
		}
		if (global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp("show_action_log", 0) || global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp("show_action_log", 7))
		{
			this.ShowActionLog();
		}
		if (global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp("more_info_unit", 0) || global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp("more_info_unit", 6))
		{
			if (++this.ShowingMoreInfoUnit >= global::UIMissionManager.UnitInfoState.MAX)
			{
				this.ShowingMoreInfoUnit = global::UIMissionManager.UnitInfoState.NONE;
			}
			if (this.StateMachine.GetActiveStateId() != 2)
			{
				this.ShowUnitExtraStats();
				this.ShowTargetExtraStats();
			}
		}
		if (global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp("unit_info_buffs", 0) || global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp("unit_info_buffs", 6))
		{
			if (this.ShowingMoreInfoUnit == global::UIMissionManager.UnitInfoState.BUFFS)
			{
				this.ShowingMoreInfoUnit = global::UIMissionManager.UnitInfoState.DEBUFFS;
			}
			else if (this.ShowingMoreInfoUnit == global::UIMissionManager.UnitInfoState.DEBUFFS)
			{
				this.ShowingMoreInfoUnit = global::UIMissionManager.UnitInfoState.NONE;
			}
			else
			{
				this.ShowingMoreInfoUnit = global::UIMissionManager.UnitInfoState.BUFFS;
			}
			if (this.StateMachine.GetActiveStateId() != 2)
			{
				this.ShowUnitExtraStats();
				this.ShowTargetExtraStats();
			}
		}
		if (global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp("unit_info_stats", 0) || global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp("unit_info_stats", 6))
		{
			if (this.ShowingMoreInfoUnit == global::UIMissionManager.UnitInfoState.STATS)
			{
				this.ShowingMoreInfoUnit = global::UIMissionManager.UnitInfoState.NONE;
			}
			else
			{
				this.ShowingMoreInfoUnit = global::UIMissionManager.UnitInfoState.STATS;
			}
			if (this.StateMachine.GetActiveStateId() != 2)
			{
				this.ShowUnitExtraStats();
				this.ShowTargetExtraStats();
			}
		}
		if (global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp("unit_info_resists", 0) || global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp("unit_info_resists", 6))
		{
			if (this.ShowingMoreInfoUnit == global::UIMissionManager.UnitInfoState.WEAPON_RESIST)
			{
				this.ShowingMoreInfoUnit = global::UIMissionManager.UnitInfoState.NONE;
			}
			else
			{
				this.ShowingMoreInfoUnit = global::UIMissionManager.UnitInfoState.WEAPON_RESIST;
			}
			if (this.StateMachine.GetActiveStateId() != 2)
			{
				this.ShowUnitExtraStats();
				this.ShowTargetExtraStats();
			}
		}
		if (global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp("more_info_unit_action", 0))
		{
			this.ShowingMoreInfoUnitAction = !this.ShowingMoreInfoUnitAction;
			global::PandoraSingleton<global::NoticeManager>.instance.SendNotice<bool>(global::Notices.GAME_MORE_INFO_UNIT_ACTION_TOGGLE, this.ShowingMoreInfoUnitAction);
		}
		else if (global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp("more_info_mission", 0))
		{
			this.ShowingMoreInfoMission = !this.ShowingMoreInfoMission;
			this.ShowObjectives();
		}
	}

	private void ToggleUI()
	{
		if (this.optionsMenu.activeSelf)
		{
			this.uiVisible = !this.uiVisible;
		}
		else
		{
			this.canvasDisabler.enabled = !this.canvasDisabler.enabled;
			global::PandoraDebug.LogInfo("Gui is now " + this.canvasDisabler.enabled, "Commands", this);
		}
	}

	private void ShowChat()
	{
		if (global::PandoraSingleton<global::Hephaestus>.Instance.IsOnline())
		{
			this.chatLog.ShowChat(true);
		}
	}

	private void ShowActionLog()
	{
		this.chatLog.ToggleLogDisplay();
	}

	private void FixedUpdate()
	{
		this.StateMachine.FixedUpdate();
	}

	public void ShowObjectives()
	{
		this.ShowingMoreInfoMission |= (global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.isCampaign && !global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.isTuto);
		if (this.ShowingMoreInfoMission)
		{
			this.objectives.OnEnable();
		}
		else
		{
			this.objectives.OnDisable();
		}
	}

	public void ShowUnitExtraStats()
	{
		if (this.CurrentUnitController != null && this.CurrentUnitController.IsPlayed())
		{
			switch (this.ShowingMoreInfoUnit)
			{
			case global::UIMissionManager.UnitInfoState.NONE:
				this.unitAlternateWeapon.Hide();
				this.unitStats.Hide();
				this.unitEnchantments.Hide();
				this.unitEnchantmentsDebuffs.Hide();
				break;
			case global::UIMissionManager.UnitInfoState.BUFFS:
				this.unitAlternateWeapon.Hide();
				this.unitStats.Hide();
				this.unitEnchantments.Show();
				this.unitEnchantmentsDebuffs.Hide();
				break;
			case global::UIMissionManager.UnitInfoState.DEBUFFS:
				this.unitAlternateWeapon.Hide();
				this.unitStats.Hide();
				this.unitEnchantments.Hide();
				this.unitEnchantmentsDebuffs.Show();
				break;
			case global::UIMissionManager.UnitInfoState.STATS:
				this.unitAlternateWeapon.Hide();
				this.unitEnchantments.Hide();
				this.unitEnchantmentsDebuffs.Hide();
				this.unitStats.Show();
				break;
			case global::UIMissionManager.UnitInfoState.WEAPON_RESIST:
				this.unitStats.Hide();
				this.unitEnchantments.Hide();
				this.unitEnchantmentsDebuffs.Hide();
				this.unitAlternateWeapon.Show();
				break;
			default:
				global::PandoraDebug.LogWarning("Unsupported UnitInfoState, this should not happen, showing no unit infos.", "UI", this);
				break;
			}
		}
		else
		{
			this.unitStats.Hide();
			this.unitEnchantments.Hide();
			this.unitEnchantmentsDebuffs.Hide();
			this.unitAlternateWeapon.Hide();
		}
	}

	public void ShowTargetExtraStats()
	{
		if ((this.ShowingOverview && this.CurrentUnitTargetController != null) || (this.CurrentUnitController != null && this.CurrentUnitTargetController != null && this.CurrentUnitController.IsPlayed() && this.StateMachine.GetActiveStateId() == 1))
		{
			switch (this.ShowingMoreInfoUnit)
			{
			case global::UIMissionManager.UnitInfoState.NONE:
				this.targetAlternateWeapon.Hide();
				this.targetStats.Hide();
				this.targetEnchantments.Hide();
				this.targetEnchantmentsDebuffs.Hide();
				break;
			case global::UIMissionManager.UnitInfoState.BUFFS:
				this.targetAlternateWeapon.Hide();
				this.targetStats.Hide();
				this.targetEnchantments.Show();
				this.targetEnchantmentsDebuffs.Hide();
				break;
			case global::UIMissionManager.UnitInfoState.DEBUFFS:
				this.targetAlternateWeapon.Hide();
				this.targetStats.Hide();
				this.targetEnchantments.Hide();
				this.targetEnchantmentsDebuffs.Show();
				break;
			case global::UIMissionManager.UnitInfoState.STATS:
				this.targetAlternateWeapon.Hide();
				this.targetStats.Show();
				this.targetEnchantments.Hide();
				this.targetEnchantmentsDebuffs.Hide();
				break;
			case global::UIMissionManager.UnitInfoState.WEAPON_RESIST:
				this.targetAlternateWeapon.Show();
				this.targetStats.Hide();
				this.targetEnchantments.Hide();
				this.targetEnchantmentsDebuffs.Hide();
				break;
			default:
				global::PandoraDebug.LogWarning("Unsupported UnitInfoState, this should not happen, showing no unit infos.", "UI", this);
				break;
			}
		}
		else
		{
			this.targetAlternateWeapon.Hide();
			this.targetStats.Hide();
			this.targetEnchantments.Hide();
		}
	}

	public void HideUnitStats()
	{
		this.CurrentUnitController = null;
		this.CurrentUnitTargetController = null;
		this.unitCombatStats.OnDisable();
		this.targetCombatStats.OnDisable();
		this.ShowUnitExtraStats();
		this.ShowTargetExtraStats();
	}

	public void SetPropsInfo(global::UnityEngine.Sprite icon, string title)
	{
		this.propsInfo.OnEnable();
		this.propsInfo.Set(icon, title);
	}

	public void HidePropsInfo()
	{
		this.propsInfo.OnDisable();
	}

	public void ShowOptions()
	{
		if (!this.optionsMenu.activeSelf)
		{
			this.uiVisible = this.canvasDisabler.enabled;
			this.canvasDisabler.enabled = false;
			global::PandoraSingleton<global::PandoraInput>.Instance.SetCurrentState(global::PandoraInput.States.MENU, true);
			global::PandoraSingleton<global::PandoraInput>.Instance.PushInputLayer(global::PandoraInput.InputLayer.MENU);
			this.optionsMenu.SetActive(true);
			if (((global::UnityEngine.RectTransform)this.optionsMenu.transform).localScale == global::UnityEngine.Vector3.zero)
			{
				((global::UnityEngine.RectTransform)this.optionsMenu.transform).localScale = global::UnityEngine.Vector3.one;
			}
			if (!global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.isTuto)
			{
				if (global::PandoraSingleton<global::MissionManager>.Instance.GetMyWarbandCtrlr().MoralRatio > global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.routThreshold)
				{
					this.optionsMan.DisableAltQuitOption();
				}
				else
				{
					this.optionsMan.DisableQuitOption();
				}
			}
			this.optionsMan.OnShow();
			this.optionsMan.butExit.RefreshImage();
		}
	}

	public void HideOptions()
	{
		if (this.optionsMenu.activeSelf)
		{
			this.canvasDisabler.enabled = this.uiVisible;
			global::PandoraSingleton<global::PandoraInput>.Instance.SetCurrentState(global::PandoraInput.States.MISSION, false);
			global::PandoraSingleton<global::PandoraInput>.Instance.PopInputLayer(global::PandoraInput.InputLayer.MENU);
			this.optionsMenu.SetActive(false);
			this.optionsMan.OnHide();
			this.quitGamePopup.Hide();
			global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice(global::Notices.OPTIONS_CLOSED);
		}
	}

	public void OnSaveAndQuit()
	{
		this.quitGamePopup.ShowLocalized(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("menu_save_and_quit_title"), global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("menu_save_and_quit_desc"), new global::System.Action<bool>(this.SaveAndQuit), false, false);
	}

	private void SaveAndQuit(bool confirm)
	{
		if (confirm)
		{
			global::PandoraSingleton<global::MissionManager>.Instance.SaveAndQuit();
		}
		else
		{
			this.optionsMan.butSaveAndQuit.SetSelected(true);
		}
	}

	public void OnQuitGame(bool altQuit)
	{
		if (global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.isTuto)
		{
			this.quitGamePopup.ShowLocalized(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("menu_quit_mission_title"), global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("menu_quit_mission_desc_no_penalty"), new global::System.Action<bool>(this.QuitGame), false, false);
		}
		else if (altQuit)
		{
			if (global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.isSkirmish)
			{
				this.quitGamePopup.ShowLocalized(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("menu_quit_mission_title"), global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("menu_quit_mission_exhibition_desc"), new global::System.Action<bool>(this.QuitGame), false, false);
			}
			else
			{
				this.quitGamePopup.ShowLocalized(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("menu_quit_mission_title"), global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("menu_quit_mission_desc"), new global::System.Action<bool>(this.QuitGame), false, false);
			}
		}
		else if (global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.isSkirmish)
		{
			this.quitGamePopup.ShowLocalized(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("menu_quit_mission_voluntary_rout_title"), global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("menu_quit_mission_desc_no_penalty"), new global::System.Action<bool>(this.QuitGame), false, false);
		}
		else
		{
			this.quitGamePopup.ShowLocalized(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("menu_quit_mission_voluntary_rout_title"), global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("menu_quit_mission_voluntary_rout_desc"), new global::System.Action<bool>(this.QuitGame), false, false);
		}
	}

	private void QuitGame(bool confirm)
	{
		if (confirm)
		{
			this.HideOptions();
			global::PandoraSingleton<global::MissionManager>.Instance.NetworkMngr.SendForfeitMission(global::PandoraSingleton<global::MissionManager>.Instance.GetMyWarbandCtrlr().idx);
		}
		else
		{
			this.optionsMan.butQuit.SetSelected(true);
		}
	}

	public global::QuitGamePopup quitGamePopup;

	public global::ConfirmationPopupView messagePopup;

	public global::UnityEngine.GameObject optionsMenu;

	private global::OptionsManager optionsMan;

	public global::UIMissionLadderController ladder;

	public global::UIMissionMoraleController morale;

	public global::UISequenceMessage leftSequenceMessage;

	public global::UISequenceMessage rightSequenceMessage;

	public global::UIInteractiveMessage interactiveMessage;

	public global::UITurnMessage turnMessage;

	public global::UIObjectivesController objectives;

	public global::UIUnitCombatStatsController unitCombatStats;

	public global::UIUnitCurrentActionController unitAction;

	public global::UISlideInElement unitAlternateWeapon;

	public global::UISlideInElement unitStats;

	public global::UISlideInElement unitEnchantments;

	public global::UISlideInElement unitEnchantmentsDebuffs;

	public global::UIUnitCombatStatsController targetCombatStats;

	public global::UISlideInElement targetAlternateWeapon;

	public global::UISlideInElement targetStats;

	public global::UISlideInElement targetEnchantments;

	public global::UISlideInElement targetEnchantmentsDebuffs;

	public global::System.Collections.Generic.List<global::UIUnitControllerChanged> currentUnitUI;

	public global::System.Collections.Generic.List<global::UIUnitControllerChanged> currentUnitTargetUI;

	public global::System.Collections.Generic.List<global::UISlideInElement> extraStats;

	public global::UIWheelController wheel;

	public global::UIInventoryController inventory;

	public global::UIEndGameReport endGameReport;

	public global::UIChatLog chatLog;

	public global::UIOverviewController overview;

	public global::UIPropsInfoController propsInfo;

	public global::UIDeployControls deployControls;

	public bool showExtraStats;

	private global::UnitController currentUnitController;

	private global::UnitController currentUnitTargetController;

	private global::Destructible currentUnitTargetDestructible;

	[global::UnityEngine.HideInInspector]
	public global::CheapStateMachine StateMachine;

	[global::UnityEngine.HideInInspector]
	public global::UnityEngine.AudioSource audioSource;

	private global::CanvasGroupDisabler canvasDisabler;

	private bool uiVisible;

	public enum State
	{
		MOVING,
		SINGLE_TARGETING,
		SEQUENCE,
		COUNTER,
		SEARCH,
		ENDGAME,
		DEPLOY,
		MAX
	}

	public enum UnitInfoState
	{
		NONE,
		BUFFS,
		DEBUFFS,
		STATS,
		WEAPON_RESIST,
		MAX
	}
}
