using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Rewired;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionsManager : global::UnityEngine.MonoBehaviour
{
	private void Awake()
	{
		this.controlsRemapPopup = global::UnityEngine.Object.Instantiate<global::RemapButtonPopupView>(this.controlsRemapPopup);
		global::UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(this.controlsRemapPopup.gameObject, base.gameObject.scene);
		this.confirmPopup = global::UnityEngine.Object.Instantiate<global::ConfirmationPopupView>(this.confirmPopup);
		global::UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(this.confirmPopup.gameObject, base.gameObject.scene);
		this.audioSrc = base.GetComponent<global::UnityEngine.AudioSource>();
		this.overDescriptionText = this.overDescription.GetComponentsInChildren<global::UnityEngine.UI.Text>(true)[0];
		global::PandoraSingleton<global::NoticeManager>.Instance.RegisterListener(global::Notices.CONTROLLER_CONNECTED, new global::DelReceiveNotice(this.FillControlsPanel));
		global::PandoraSingleton<global::NoticeManager>.Instance.RegisterListener(global::Notices.CONTROLLER_DISCONNECTED, new global::DelReceiveNotice(this.FillControlsPanel));
		this.availableLangs = global::PandoraSingleton<global::Hephaestus>.Instance.GetAvailableLanguages();
		global::PandoraSingleton<global::Pan>.Instance.GetSound("snd_interface_general_click", true, delegate(global::UnityEngine.AudioClip clip)
		{
			this.ambientVolumeSample = clip;
		});
	}

	private void Init()
	{
		if (this.initialized)
		{
			return;
		}
		this.initialized = true;
		this.InitButtons();
		this.InitAudio();
		this.InitControls();
		this.InitGameplay();
		this.InitGraphics();
		this.InitMappings();
		this.butSaveAndQuit.onAction.AddListener(delegate()
		{
			this.onSaveQuitGame();
		});
		this.butSaveAndQuit.onSelect.AddListener(new global::UnityEngine.Events.UnityAction(this.ShowSaveAndQuitDesc));
		this.butSaveAndQuit.onUnselect.AddListener(new global::UnityEngine.Events.UnityAction(this.HideOverDesc));
		this.butSaveAndQuit.onPointerEnter.AddListener(new global::UnityEngine.Events.UnityAction(this.ShowSaveAndQuitDesc));
		this.butSaveAndQuit.onPointerExit.AddListener(new global::UnityEngine.Events.UnityAction(this.HideOverDesc));
		this.butSaveAndQuit.toggle.onValueChanged.AddListener(delegate(bool isOn)
		{
			this.SetMappingsPanelButtonsVisible(!isOn);
		});
		this.butQuit.GetComponentsInChildren<global::UnityEngine.UI.Text>(true)[0].text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(this.quitButtonLoc);
		this.butQuit.onAction.AddListener(delegate()
		{
			this.onQuitGame(true);
		});
		this.butQuit.onSelect.AddListener(new global::UnityEngine.Events.UnityAction(this.ShowQuitDesc));
		this.butQuit.onUnselect.AddListener(new global::UnityEngine.Events.UnityAction(this.HideOverDesc));
		this.butQuit.onPointerEnter.AddListener(new global::UnityEngine.Events.UnityAction(this.ShowQuitDesc));
		this.butQuit.onPointerExit.AddListener(new global::UnityEngine.Events.UnityAction(this.HideOverDesc));
		this.butQuit.toggle.onValueChanged.AddListener(delegate(bool isOn)
		{
			this.SetMappingsPanelButtonsVisible(!isOn);
		});
		this.butQuitAlt.GetComponentsInChildren<global::UnityEngine.UI.Text>(true)[0].text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(this.quitAltButtonLoc);
		this.butQuitAlt.onAction.AddListener(delegate()
		{
			this.onQuitGame(false);
		});
		this.butQuitAlt.onSelect.AddListener(new global::UnityEngine.Events.UnityAction(this.ShowAltQuitDesc));
		this.butQuitAlt.onUnselect.AddListener(new global::UnityEngine.Events.UnityAction(this.HideOverDesc));
		this.butQuitAlt.onPointerEnter.AddListener(new global::UnityEngine.Events.UnityAction(this.ShowAltQuitDesc));
		this.butQuitAlt.onPointerExit.AddListener(new global::UnityEngine.Events.UnityAction(this.HideOverDesc));
		this.butQuitAlt.toggle.onValueChanged.AddListener(delegate(bool isOn)
		{
			this.SetMappingsPanelButtonsVisible(!isOn);
		});
		this.butHelp.gameObject.SetActive(false);
		this.butOpponentProfile.gameObject.SetActive(false);
		this.SetMappingsPanelButtonsVisible(false);
	}

	private void ShowQuitDesc()
	{
		if (!string.IsNullOrEmpty(this.quitButtonOverDescLoc))
		{
			if (this.butQuit.actionDisabled)
			{
				this.overDescriptionText.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(this.quitButtonOverDisabledDescLoc);
			}
			else
			{
				this.overDescriptionText.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(this.quitButtonOverDescLoc);
			}
			this.TweenOverDesc(1f);
		}
	}

	private void ShowAltQuitDesc()
	{
		if (!string.IsNullOrEmpty(this.quitAltButtonOverDescLoc))
		{
			if (this.butQuitAlt.actionDisabled)
			{
				this.overDescriptionText.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(this.quitAltButtonOverDisabledDescLoc);
			}
			else
			{
				this.overDescriptionText.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(this.quitAltButtonOverDescLoc);
			}
			this.TweenOverDesc(1f);
		}
	}

	private void ShowSaveAndQuitDesc()
	{
		if (this.butSaveAndQuit.actionDisabled)
		{
			this.overDescriptionText.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(this.saveAndQuitDisabledDescLoc);
			this.TweenOverDesc(1f);
		}
	}

	private void HideOverDesc()
	{
		this.TweenOverDesc(0f);
	}

	private void TweenOverDesc(float start)
	{
		global::DG.Tweening.DOTween.To(delegate()
		{
			if (this.overDescription != null)
			{
				return this.overDescription.alpha;
			}
			return 0f;
		}, delegate(float alpha)
		{
			if (this.overDescription != null)
			{
				this.overDescription.alpha = alpha;
			}
		}, start, 0.3f).SetTarget(this);
	}

	private void OnDestroy()
	{
		if (global::PandoraSingleton<global::NoticeManager>.Instance != null)
		{
			global::PandoraSingleton<global::NoticeManager>.Instance.RemoveListener(global::Notices.CONTROLLER_CONNECTED, new global::DelReceiveNotice(this.FillControlsPanel));
			global::PandoraSingleton<global::NoticeManager>.Instance.RemoveListener(global::Notices.CONTROLLER_DISCONNECTED, new global::DelReceiveNotice(this.FillControlsPanel));
		}
	}

	private void OnEnable()
	{
		this.panelOpen = false;
	}

	private void OnDisable()
	{
		this.audioOptions.toggle.isOn = false;
		this.graphics.toggle.isOn = false;
		this.controlsOptions.toggle.isOn = false;
		this.gameplayOptions.toggle.isOn = false;
		this.mappings.toggle.isOn = false;
		this.butQuit.toggle.isOn = false;
		this.butQuitAlt.toggle.isOn = false;
		this.butQuit.enabled = true;
		this.butQuitAlt.enabled = true;
	}

	private void Update()
	{
		if (this.countdownToRestore)
		{
			int num = global::UnityEngine.Mathf.CeilToInt(this.restoreTime - global::UnityEngine.Time.time);
			if (num <= 0)
			{
				this.countdownToRestore = false;
				this.confirmPopup.Hide();
				this.RevertChanges();
			}
			else if (this.lastTimeDisplayed != num)
			{
				this.lastTimeDisplayed = num;
				this.confirmPopup.text.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("popup_apply_changes_revert", new string[]
				{
					num.ToString()
				});
			}
		}
		if (global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp("cancel", 6))
		{
			if (this.panelOpen)
			{
				this.OnBackToOptions();
			}
			else
			{
				this.OnBack();
			}
		}
	}

	public void OnShow()
	{
		this.Init();
		this.audioOptions.toggle.SetSelected(true);
		this.audioOptions.toggle.isOn = true;
		this.InitButtons();
		this.mappingChanged = false;
		this.FillControlsPanel();
		this.InitAudioData();
		this.InitControlsData();
		this.InitGameplayData();
		this.InitGraphicsData();
		this.InitMappingsData();
		this.overDescription.alpha = 0f;
		if (this.camMan == null && global::UnityEngine.Camera.main != null)
		{
			this.camMan = global::UnityEngine.Camera.main.GetComponent<global::CameraManager>();
		}
		if (this.camMan != null)
		{
			this.camMan.ActivateOverlay(true, 0f);
		}
		this.mappingsUnselected = true;
	}

	public void OnHide()
	{
		if (this.camMan == null && global::UnityEngine.Camera.main != null)
		{
			this.camMan = global::UnityEngine.Camera.main.GetComponent<global::CameraManager>();
		}
		if (this.camMan != null)
		{
			this.camMan.ActivateOverlay(false, 0f);
		}
		if (this.confirmPopup.IsVisible)
		{
			this.confirmPopup.Hide();
		}
		this.controlsRemapPopup.Hide();
		this.CancelMappingChanges();
		this.controlsList.ClearList();
		this.controlsList.DestroyItems();
		global::PandoraSingleton<global::GameManager>.Instance.SetVolumeOptions();
		global::PandoraSingleton<global::GameManager>.Instance.SetMappingOptions();
	}

	private void SetMappingsPanelButtonsVisible(bool visible)
	{
		if (this.butRestore != null)
		{
			this.butRestore.gameObject.SetActive(visible);
		}
	}

	private void InitButtons()
	{
		this.butExit.SetAction("cancel", "menu_back", 6, false, this.icnBack, null);
		this.butExit.OnAction(new global::UnityEngine.Events.UnityAction(this.OnBack), true, true);
		this.butExit.gameObject.SetActive(true);
		this.butBack.gameObject.SetActive(false);
		this.butApply.SetAction("apply_changes", "menu_apply_changes", 6, false, null, null);
		this.butApply.OnAction(new global::UnityEngine.Events.UnityAction(this.OnApplyChanges), false, true);
		this.butApply.gameObject.SetActive(true);
		this.butRestore.SetAction("restore_default_mapping", "menu_restore_default_mappings", 6, false, null, null);
		this.butRestore.OnAction(new global::UnityEngine.Events.UnityAction(this.ResetMappings), false, true);
		this.butRestore.gameObject.SetActive(false);
	}

	public void HideQuitOption()
	{
		global::UnityEngine.UI.Navigation navigation = this.butQuit.toggle.navigation;
		global::UnityEngine.UI.Navigation navigation2 = navigation.selectOnUp.GetComponent<global::UnityEngine.UI.Toggle>().navigation;
		global::UnityEngine.UI.Navigation navigation3 = navigation.selectOnDown.GetComponent<global::UnityEngine.UI.Toggle>().navigation;
		navigation2.selectOnDown = navigation.selectOnDown;
		navigation3.selectOnUp = navigation.selectOnUp;
		navigation.selectOnUp.GetComponent<global::UnityEngine.UI.Toggle>().navigation = navigation2;
		navigation.selectOnDown.GetComponent<global::UnityEngine.UI.Toggle>().navigation = navigation3;
		this.butQuit.gameObject.SetActive(false);
		this.HideSaveAndQuitOption();
		this.HideAltQuitOption();
	}

	public void HideAltQuitOption()
	{
		global::UnityEngine.UI.Navigation navigation = this.butQuitAlt.toggle.navigation;
		global::UnityEngine.UI.Navigation navigation2 = navigation.selectOnUp.GetComponent<global::UnityEngine.UI.Toggle>().navigation;
		navigation2.selectOnDown = navigation.selectOnDown;
		navigation.selectOnUp.GetComponent<global::UnityEngine.UI.Toggle>().navigation = navigation2;
		this.butQuitAlt.gameObject.SetActive(false);
	}

	public void HideSaveAndQuitOption()
	{
		global::UnityEngine.UI.Navigation navigation = this.butSaveAndQuit.toggle.navigation;
		global::UnityEngine.UI.Navigation navigation2 = navigation.selectOnUp.GetComponent<global::UnityEngine.UI.Toggle>().navigation;
		navigation2.selectOnDown = navigation.selectOnDown;
		navigation.selectOnUp.GetComponent<global::UnityEngine.UI.Toggle>().navigation = navigation2;
		global::UnityEngine.UI.Navigation navigation3 = navigation.selectOnDown.GetComponent<global::UnityEngine.UI.Toggle>().navigation;
		navigation3.selectOnUp = navigation.selectOnUp;
		navigation.selectOnDown.GetComponent<global::UnityEngine.UI.Toggle>().navigation = navigation3;
		this.butSaveAndQuit.gameObject.SetActive(false);
	}

	public void SetBackButtonLoc(string loc)
	{
		this.backButtonLoc = loc;
	}

	public void SetQuitButtonLoc(string loc, string desc = "")
	{
		this.quitButtonLoc = loc;
		this.quitButtonOverDescLoc = desc;
	}

	public void SetQuitAltButtonLoc(string loc, string desc = "")
	{
		this.quitAltButtonLoc = loc;
		this.quitAltButtonOverDescLoc = desc;
	}

	public void DisableQuitOption()
	{
		this.butQuit.DisableAction();
		this.butQuit.toggle.interactable = false;
		this.butQuitAlt.EnableAction();
		this.butQuitAlt.toggle.interactable = true;
	}

	public void DisableAltQuitOption()
	{
		this.butQuit.EnableAction();
		this.butQuit.toggle.interactable = true;
		this.butQuitAlt.DisableAction();
		this.butQuitAlt.toggle.interactable = false;
	}

	public void DisableSaveAndQuitOption()
	{
		this.butSaveAndQuit.DisableAction();
		this.butSaveAndQuit.toggle.interactable = false;
	}

	public void RemoveFromNav(global::ToggleEffects itm)
	{
		global::UnityEngine.UI.Navigation navigation = itm.toggle.navigation;
		if (navigation.selectOnUp != null)
		{
			global::UnityEngine.UI.Toggle component = navigation.selectOnUp.GetComponent<global::UnityEngine.UI.Toggle>();
			global::UnityEngine.UI.Navigation navigation2 = component.navigation;
			navigation2.selectOnDown = navigation.selectOnDown;
			component.navigation = navigation2;
		}
		if (navigation.selectOnDown != null)
		{
			global::UnityEngine.UI.Toggle component2 = navigation.selectOnDown.GetComponent<global::UnityEngine.UI.Toggle>();
			global::UnityEngine.UI.Navigation navigation3 = component2.navigation;
			navigation3.selectOnUp = navigation.selectOnUp;
			component2.navigation = navigation3;
		}
	}

	public void AddBackToNav(global::ToggleEffects itm)
	{
		global::UnityEngine.UI.Navigation navigation = itm.toggle.navigation;
		if (navigation.selectOnUp != null)
		{
			global::UnityEngine.UI.Toggle component = navigation.selectOnUp.GetComponent<global::UnityEngine.UI.Toggle>();
			global::UnityEngine.UI.Navigation navigation2 = component.navigation;
			navigation2.selectOnDown = itm.toggle;
			component.navigation = navigation2;
		}
		if (navigation.selectOnDown != null)
		{
			global::UnityEngine.UI.Toggle component2 = navigation.selectOnDown.GetComponent<global::UnityEngine.UI.Toggle>();
			global::UnityEngine.UI.Navigation navigation3 = component2.navigation;
			navigation3.selectOnUp = itm.toggle;
			component2.navigation = navigation3;
		}
	}

	private void ToggleOffLastSelection()
	{
		bool activeSelf = this.audioPanel.gameObject.activeSelf;
		this.audioPanel.gameObject.SetActive(true);
		this.audioPanel.SetAllTogglesOff();
		this.audioPanel.gameObject.SetActive(activeSelf);
		activeSelf = this.graphicsPanel.gameObject.activeSelf;
		this.graphicsPanel.gameObject.SetActive(true);
		this.graphicsPanel.SetAllTogglesOff();
		this.graphicsPanel.gameObject.SetActive(activeSelf);
		activeSelf = this.controlsPanel.gameObject.activeSelf;
		this.controlsPanel.gameObject.SetActive(true);
		this.controlsPanel.SetAllTogglesOff();
		this.controlsPanel.gameObject.SetActive(activeSelf);
		activeSelf = this.gameplayPanel.gameObject.activeSelf;
		this.gameplayPanel.gameObject.SetActive(true);
		this.gameplayPanel.SetAllTogglesOff();
		this.gameplayPanel.gameObject.SetActive(activeSelf);
		activeSelf = this.mappingPanel.gameObject.activeSelf;
		this.mappingPanel.gameObject.SetActive(true);
		this.mappingPanel.SetAllTogglesOff();
		this.mappingPanel.gameObject.SetActive(activeSelf);
	}

	private void OnPanelOpen()
	{
		this.ToggleOffLastSelection();
		this.panelOpen = true;
	}

	public void OnBack()
	{
		if (this.CheckNeedApply())
		{
			this.confirmPopup.Show("popup_apply_exit_title", "popup_apply_exit_desc", delegate(bool confirm)
			{
				if (confirm)
				{
					this.DoApplyChanges(this.onCloseOptionsMenu);
				}
				else
				{
					this.CancelMappingChanges();
					this.onCloseOptionsMenu();
				}
			}, false, false);
		}
		else
		{
			this.onCloseOptionsMenu();
		}
	}

	public void OnBackToOptions()
	{
		this.panelOpen = false;
		global::UnityEngine.GameObject currentSelectedGameObject = global::UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
		if (this.audioOptions.toggle.isOn)
		{
			this.audioPanel.SetAllTogglesOff();
			this.audioOptions.toggle.Select();
		}
		else if (this.graphics.toggle.isOn)
		{
			this.graphicsPanel.SetAllTogglesOff();
			this.graphics.toggle.Select();
		}
		else if (this.controlsOptions.toggle.isOn)
		{
			this.controlsPanel.SetAllTogglesOff();
			this.controlsOptions.toggle.Select();
		}
		else if (this.gameplayOptions.toggle.isOn)
		{
			this.gameplayPanel.SetAllTogglesOff();
			this.gameplayOptions.toggle.Select();
		}
		else if (this.mappings.toggle.isOn)
		{
			this.mappingPanel.SetAllTogglesOff();
			this.mappings.toggle.Select();
		}
	}

	private bool CheckNeedApply()
	{
		return this.CheckAudioNeedApply() || this.CheckControlsNeedApply() || this.CheckGameplayNeedApply() || this.CheckMappingNeedApply() || this.CheckGraphicsNeedApply();
	}

	private void OnApplyChanges()
	{
		if (this.CheckNeedApply())
		{
			this.DoApplyChanges(null);
		}
	}

	private void DoApplyChanges(global::System.Action onDone = null)
	{
		bool noRestartNeeded = true;
		noRestartNeeded &= this.ApplyAudioChanges();
		noRestartNeeded &= this.ApplyGraphicsChanges();
		noRestartNeeded &= this.ApplyMappingChanges();
		noRestartNeeded &= this.ApplyControlsChanges();
		noRestartNeeded &= this.ApplyGameplayChanges();
		if (this.needRevertOptionsConfirm)
		{
			this.countdownToRestore = true;
			this.restoreTime = global::UnityEngine.Time.time + 15f;
			this.lastTimeDisplayed = global::UnityEngine.Mathf.CeilToInt(15f);
			this.confirmPopup.ShowLocalized(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("popup_apply_changes_title"), global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("popup_apply_changes_revert", new string[]
			{
				15f.ToString()
			}), delegate(bool confirm)
			{
				this.countdownToRestore = false;
				if (confirm)
				{
					this.SaveChanges(noRestartNeeded, onDone);
				}
				else
				{
					this.RevertChanges();
				}
			}, false, false);
		}
		else
		{
			this.SaveChanges(noRestartNeeded, onDone);
		}
	}

	private void SaveChanges(bool noRestartNeeded = false, global::System.Action onDone = null)
	{
		global::PandoraSingleton<global::GameManager>.Instance.WriteOptions();
		if (!noRestartNeeded)
		{
			this.confirmPopup.Show("menu_title_changes_after_resart", "menu_desc_changes_after_resart", delegate(bool confirm)
			{
				this.FillControlsPanel();
				if (onDone != null)
				{
					onDone();
				}
			}, false, false);
			this.confirmPopup.HideCancelButton();
		}
		else
		{
			this.FillControlsPanel();
			if (onDone != null)
			{
				onDone();
			}
		}
	}

	private void RevertChanges()
	{
		global::PandoraSingleton<global::GameManager>.Instance.ReadOptions();
	}

	private void InitAudio()
	{
		this.audioOptions.onAction.AddListener(new global::UnityEngine.Events.UnityAction(this.OnAudioPanelOpen));
		this.audioOptions.onSelect.AddListener(new global::UnityEngine.Events.UnityAction(this.OnAudioPanelDisplayed));
		this.InitAudioData();
	}

	private void InitAudioData()
	{
		this.musicVolume.SetValue(global::PandoraSingleton<global::GameManager>.Instance.Options.musicVolume * 100f + 0.5f);
		this.musicVolume.id = 2;
		this.musicVolume.onValueChanged = new global::SliderGroup.OnValueChanged(this.OnVolumeValueChanged);
		this.effectsVolume.SetValue(global::PandoraSingleton<global::GameManager>.Instance.Options.fxVolume * 100f + 0.5f);
		this.effectsVolume.id = 1;
		this.effectsVolume.onValueChanged = new global::SliderGroup.OnValueChanged(this.OnVolumeValueChanged);
		this.masterVolume.SetValue(global::PandoraSingleton<global::GameManager>.Instance.Options.masterVolume * 100f + 0.5f);
		this.masterVolume.id = 0;
		this.masterVolume.onValueChanged = new global::SliderGroup.OnValueChanged(this.OnVolumeValueChanged);
		this.voiceVolume.SetValue(global::PandoraSingleton<global::GameManager>.Instance.Options.voiceVolume * 100f + 0.5f);
		this.voiceVolume.id = 4;
		this.voiceVolume.onValueChanged = new global::SliderGroup.OnValueChanged(this.OnVolumeValueChanged);
		this.ambientVolume.SetValue(global::PandoraSingleton<global::GameManager>.Instance.Options.ambientVolume * 100f + 0.5f);
		this.ambientVolume.id = 3;
		this.ambientVolume.onValueChanged = new global::SliderGroup.OnValueChanged(this.OnVolumeValueChanged);
	}

	private void OnAudioPanelDisplayed()
	{
		this.ToggleOffLastSelection();
		this.SetMappingsPanelButtonsVisible(false);
	}

	private void OnAudioPanelOpen()
	{
		this.OnPanelOpen();
		this.masterVolume.GetComponentsInChildren<global::UnityEngine.UI.Slider>(true)[0].SetSelected(true);
		this.OnAudioPanelDisplayed();
	}

	private bool CheckAudioNeedApply()
	{
		if (!global::UnityEngine.Mathf.Approximately(global::PandoraSingleton<global::GameManager>.Instance.Options.masterVolume, (float)this.masterVolume.GetValue() / 100f))
		{
			global::PandoraDebug.LogDebug(string.Concat(new object[]
			{
				"Options require Apply action, masterVolume changed from ",
				global::PandoraSingleton<global::GameManager>.Instance.Options.masterVolume,
				" to ",
				(float)this.masterVolume.GetValue() / 100f
			}), "OPTIONS", this);
			return true;
		}
		if (!global::UnityEngine.Mathf.Approximately(global::PandoraSingleton<global::GameManager>.Instance.Options.fxVolume, (float)this.effectsVolume.GetValue() / 100f))
		{
			global::PandoraDebug.LogDebug(string.Concat(new object[]
			{
				"Options require Apply action, fxVolume changed from ",
				global::PandoraSingleton<global::GameManager>.Instance.Options.fxVolume,
				" to ",
				(float)this.effectsVolume.GetValue() / 100f
			}), "OPTIONS", this);
			return true;
		}
		if (!global::UnityEngine.Mathf.Approximately(global::PandoraSingleton<global::GameManager>.Instance.Options.musicVolume, (float)this.musicVolume.GetValue() / 100f))
		{
			global::PandoraDebug.LogDebug(string.Concat(new object[]
			{
				"Options require Apply action, fxVolume changed from ",
				global::PandoraSingleton<global::GameManager>.Instance.Options.musicVolume,
				" to ",
				(float)this.musicVolume.GetValue() / 100f
			}), "OPTIONS", this);
			return true;
		}
		if (!global::UnityEngine.Mathf.Approximately(global::PandoraSingleton<global::GameManager>.Instance.Options.voiceVolume, (float)this.voiceVolume.GetValue() / 100f))
		{
			global::PandoraDebug.LogDebug(string.Concat(new object[]
			{
				"Options require Apply action, fxVolume changed from ",
				global::PandoraSingleton<global::GameManager>.Instance.Options.voiceVolume,
				" to ",
				(float)this.voiceVolume.GetValue() / 100f
			}), "OPTIONS", this);
			return true;
		}
		if (!global::UnityEngine.Mathf.Approximately(global::PandoraSingleton<global::GameManager>.Instance.Options.ambientVolume, (float)this.ambientVolume.GetValue() / 100f))
		{
			global::PandoraDebug.LogDebug(string.Concat(new object[]
			{
				"Options require Apply action, ambient Volume changed from ",
				global::PandoraSingleton<global::GameManager>.Instance.Options.ambientVolume,
				" to ",
				(float)this.ambientVolume.GetValue() / 100f
			}), "OPTIONS", this);
			return true;
		}
		return false;
	}

	private bool ApplyAudioChanges()
	{
		bool flag = false;
		if (!global::UnityEngine.Mathf.Approximately(global::PandoraSingleton<global::GameManager>.Instance.Options.masterVolume, (float)this.masterVolume.GetValue() / 100f))
		{
			flag = true;
			global::PandoraSingleton<global::GameManager>.Instance.Options.masterVolume = (float)this.masterVolume.GetValue() / 100f;
		}
		if (!global::UnityEngine.Mathf.Approximately(global::PandoraSingleton<global::GameManager>.Instance.Options.fxVolume, (float)this.effectsVolume.GetValue() / 100f))
		{
			flag = true;
			global::PandoraSingleton<global::GameManager>.Instance.Options.fxVolume = (float)this.effectsVolume.GetValue() / 100f;
		}
		if (!global::UnityEngine.Mathf.Approximately(global::PandoraSingleton<global::GameManager>.Instance.Options.musicVolume, (float)this.musicVolume.GetValue() / 100f))
		{
			flag = true;
			global::PandoraSingleton<global::GameManager>.Instance.Options.musicVolume = (float)this.musicVolume.GetValue() / 100f;
		}
		if (!global::UnityEngine.Mathf.Approximately(global::PandoraSingleton<global::GameManager>.Instance.Options.voiceVolume, (float)this.voiceVolume.GetValue() / 100f))
		{
			flag = true;
			global::PandoraSingleton<global::GameManager>.Instance.Options.voiceVolume = (float)this.voiceVolume.GetValue() / 100f;
		}
		if (!global::UnityEngine.Mathf.Approximately(global::PandoraSingleton<global::GameManager>.Instance.Options.ambientVolume, (float)this.ambientVolume.GetValue() / 100f))
		{
			flag = true;
			global::PandoraSingleton<global::GameManager>.Instance.Options.ambientVolume = (float)this.ambientVolume.GetValue() / 100f;
		}
		if (flag)
		{
			global::PandoraSingleton<global::GameManager>.Instance.SetVolumeOptions();
		}
		return true;
	}

	private void OnVolumeValueChanged(int id, float value)
	{
		value /= 100f;
		global::PandoraSingleton<global::Pan>.Instance.SetVolume((global::Pan.Type)id, value);
		if (id == 0 || id == 1 || id == 4)
		{
			this.audioSrc.PlayOneShot(this.ambientVolumeSample, value);
		}
		else if (id == 3 && this.ambientVolumeSample != null)
		{
			this.audioSrc.PlayOneShot(this.ambientVolumeSample, value);
		}
	}

	private void InitGraphics()
	{
		this.graphics.onAction.AddListener(new global::UnityEngine.Events.UnityAction(this.OnGraphicsPanelOpen));
		this.graphics.onSelect.AddListener(new global::UnityEngine.Events.UnityAction(this.OnGraphicsPanelDisplayed));
		this.InitGraphicsData();
	}

	private void InitGraphicsData()
	{
		this.fullscreen.isOn = global::PandoraSingleton<global::GameManager>.Instance.Options.fullScreen;
		this.resolutions.selections.Clear();
		int currentSel = global::UnityEngine.Screen.resolutions.Length - 1;
		for (int i = 0; i < global::UnityEngine.Screen.resolutions.Length; i++)
		{
			if (global::UnityEngine.Screen.resolutions[i].width == global::PandoraSingleton<global::GameManager>.Instance.Options.resolution.width && global::UnityEngine.Screen.resolutions[i].height == global::PandoraSingleton<global::GameManager>.Instance.Options.resolution.height)
			{
				currentSel = i;
			}
			this.resolutions.selections.Add(string.Format("{0}x{1}", global::UnityEngine.Screen.resolutions[i].width, global::UnityEngine.Screen.resolutions[i].height));
		}
		this.resolutions.SetCurrentSel(currentSel);
		this.vsync.isOn = global::PandoraSingleton<global::GameManager>.Instance.Options.vsync;
		this.shadowsQuality.selections.Clear();
		this.shadowsQuality.selections.Add(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("menu_off"));
		this.shadowsQuality.selections.Add(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("opt_low"));
		this.shadowsQuality.selections.Add(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("opt_medium"));
		this.shadowsQuality.selections.Add(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("opt_high"));
		this.shadowsQuality.selections.Add(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("opt_very_high"));
		this.shadowsQuality.SetCurrentSel(global::PandoraSingleton<global::GameManager>.Instance.Options.shadowsQuality);
		this.shadowCascades.selections.Clear();
		this.shadowCascades.selections.Add("0");
		this.shadowCascades.selections.Add("2");
		this.shadowCascades.selections.Add("4");
		this.shadowCascades.SetCurrentSel(global::PandoraSingleton<global::GameManager>.Instance.Options.shadowCascades);
		this.textureQuality.selections.Clear();
		this.textureQuality.selections.Add(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("opt_low"));
		this.textureQuality.selections.Add(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("opt_medium"));
		this.textureQuality.selections.Add(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("opt_high"));
		this.textureQuality.selections.Add(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("opt_very_high"));
		this.textureQuality.SetCurrentSel(global::PandoraSingleton<global::GameManager>.Instance.Options.textureQuality);
		this.dof.isOn = global::PandoraSingleton<global::GameManager>.Instance.Options.graphicsDof;
		this.ssao.isOn = global::PandoraSingleton<global::GameManager>.Instance.Options.graphicsSsao;
		this.smaa.selections.Clear();
		this.smaa.selections.Add(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("menu_off"));
		this.smaa.selections.Add(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("opt_low"));
		this.smaa.selections.Add(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("opt_medium"));
		this.smaa.selections.Add(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("opt_high"));
		this.smaa.selections.Add(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("opt_very_high"));
		this.smaa.SetCurrentSel(global::PandoraSingleton<global::GameManager>.Instance.Options.graphicsSmaa);
		this.brightness.SetValue(global::PandoraSingleton<global::GameManager>.Instance.Options.graphicsBrightness * 100f);
		this.guiScale.SetValue(global::PandoraSingleton<global::GameManager>.Instance.Options.graphicsGuiScale * 100f);
	}

	private void OnGraphicsPanelDisplayed()
	{
		this.SetMappingsPanelButtonsVisible(false);
	}

	private void OnGraphicsPanelOpen()
	{
		this.OnPanelOpen();
		this.fullscreen.SetSelected(true);
		this.OnGraphicsPanelDisplayed();
	}

	private bool CheckGraphicsNeedApply()
	{
		if (global::PandoraSingleton<global::GameManager>.Instance.Options.fullScreen != this.fullscreen.isOn)
		{
			global::PandoraDebug.LogDebug(string.Concat(new object[]
			{
				"Options require Apply action, fullScreen changed from ",
				global::PandoraSingleton<global::GameManager>.Instance.Options.fullScreen,
				" to ",
				this.fullscreen.isOn
			}), "OPTIONS", this);
			return true;
		}
		if (global::PandoraSingleton<global::GameManager>.Instance.Options.resolution.width != global::UnityEngine.Screen.resolutions[this.resolutions.CurSel].width || global::PandoraSingleton<global::GameManager>.Instance.Options.resolution.height != global::UnityEngine.Screen.resolutions[this.resolutions.CurSel].height)
		{
			global::PandoraDebug.LogDebug(string.Concat(new object[]
			{
				"Options require Apply action, resolution changed from ",
				global::PandoraSingleton<global::GameManager>.Instance.Options.resolution,
				" to ",
				global::UnityEngine.Screen.resolutions[this.resolutions.CurSel]
			}), "OPTIONS", this);
			return true;
		}
		if (global::PandoraSingleton<global::GameManager>.Instance.Options.vsync != this.vsync.isOn)
		{
			global::PandoraDebug.LogDebug(string.Concat(new object[]
			{
				"Options require Apply action, vsync changed from ",
				global::PandoraSingleton<global::GameManager>.Instance.Options.vsync,
				" to ",
				this.vsync.isOn
			}), "OPTIONS", this);
			return true;
		}
		if (global::PandoraSingleton<global::GameManager>.Instance.Options.textureQuality != this.textureQuality.CurSel)
		{
			global::PandoraDebug.LogDebug(string.Concat(new object[]
			{
				"Options require Apply action, textureQuality changed from ",
				global::PandoraSingleton<global::GameManager>.Instance.Options.textureQuality,
				" to ",
				this.textureQuality.CurSel
			}), "OPTIONS", this);
			return true;
		}
		if (global::PandoraSingleton<global::GameManager>.Instance.Options.shadowsQuality != this.shadowsQuality.CurSel)
		{
			global::PandoraDebug.LogDebug(string.Concat(new object[]
			{
				"Options require Apply action, shadow quality changed from ",
				global::PandoraSingleton<global::GameManager>.Instance.Options.shadowsQuality,
				" to ",
				this.shadowsQuality.CurSel
			}), "OPTIONS", this);
			return true;
		}
		if (global::PandoraSingleton<global::GameManager>.Instance.Options.shadowCascades != this.shadowCascades.CurSel)
		{
			global::PandoraDebug.LogDebug(string.Concat(new object[]
			{
				"Options require Apply action, shadow cascades changed from ",
				global::PandoraSingleton<global::GameManager>.Instance.Options.shadowCascades,
				" to ",
				this.shadowCascades.CurSel
			}), "OPTIONS", this);
			return true;
		}
		if (global::PandoraSingleton<global::GameManager>.Instance.Options.graphicsDof != this.dof.isOn)
		{
			global::PandoraDebug.LogDebug(string.Concat(new object[]
			{
				"Options require Apply action, DoF changed from ",
				global::PandoraSingleton<global::GameManager>.Instance.Options.graphicsDof,
				" to ",
				this.dof.isOn
			}), "OPTIONS", this);
			return true;
		}
		if (global::PandoraSingleton<global::GameManager>.Instance.Options.graphicsSsao != this.ssao.isOn)
		{
			global::PandoraDebug.LogDebug(string.Concat(new object[]
			{
				"Options require Apply action, SSAO changed from ",
				global::PandoraSingleton<global::GameManager>.Instance.Options.graphicsSsao,
				" to ",
				this.ssao.isOn
			}), "OPTIONS", this);
			return true;
		}
		if (global::PandoraSingleton<global::GameManager>.Instance.Options.graphicsSmaa != this.smaa.CurSel)
		{
			global::PandoraDebug.LogDebug(string.Concat(new object[]
			{
				"Options require Apply action, SMAA changed from ",
				global::PandoraSingleton<global::GameManager>.Instance.Options.graphicsSmaa,
				" to ",
				this.smaa.CurSel
			}), "OPTIONS", this);
			return true;
		}
		if (!global::UnityEngine.Mathf.Approximately(global::PandoraSingleton<global::GameManager>.Instance.Options.graphicsBrightness, (float)this.brightness.GetValue() / 100f))
		{
			global::PandoraDebug.LogDebug(string.Concat(new object[]
			{
				"Options require Apply action, Brightness changed from ",
				global::PandoraSingleton<global::GameManager>.Instance.Options.graphicsBrightness,
				" to ",
				(float)this.brightness.GetValue() / 100f
			}), "OPTIONS", this);
			return true;
		}
		if (!global::UnityEngine.Mathf.Approximately(global::PandoraSingleton<global::GameManager>.Instance.Options.graphicsGuiScale, (float)this.guiScale.GetValue() / 100f))
		{
			global::PandoraDebug.LogDebug(string.Concat(new object[]
			{
				"Options require Apply action, Gui Scale changed from ",
				global::PandoraSingleton<global::GameManager>.Instance.Options.graphicsGuiScale,
				" to ",
				(float)this.guiScale.GetValue() / 100f
			}), "OPTIONS", this);
			return true;
		}
		return false;
	}

	private bool ApplyGraphicsChanges()
	{
		bool flag = false;
		bool result = true;
		this.needRevertOptionsConfirm = false;
		if (global::PandoraSingleton<global::GameManager>.Instance.Options.fullScreen != this.fullscreen.isOn)
		{
			flag = true;
			global::PandoraSingleton<global::GameManager>.Instance.Options.fullScreen = this.fullscreen.isOn;
		}
		if (global::PandoraSingleton<global::GameManager>.Instance.Options.resolution.width != global::UnityEngine.Screen.resolutions[this.resolutions.CurSel].width || global::PandoraSingleton<global::GameManager>.Instance.Options.resolution.height != global::UnityEngine.Screen.resolutions[this.resolutions.CurSel].height)
		{
			flag = true;
			global::PandoraSingleton<global::GameManager>.Instance.Options.resolution = global::UnityEngine.Screen.resolutions[this.resolutions.CurSel];
		}
		if (global::PandoraSingleton<global::GameManager>.Instance.Options.vsync != this.vsync.isOn)
		{
			flag = true;
			global::PandoraSingleton<global::GameManager>.Instance.Options.vsync = this.vsync.isOn;
		}
		if (global::PandoraSingleton<global::GameManager>.Instance.Options.textureQuality != this.textureQuality.CurSel)
		{
			flag = true;
			result = false;
			global::PandoraSingleton<global::GameManager>.Instance.Options.textureQuality = this.textureQuality.CurSel;
		}
		if (global::PandoraSingleton<global::GameManager>.Instance.Options.shadowsQuality != this.shadowsQuality.CurSel)
		{
			flag = true;
			result = false;
			global::PandoraSingleton<global::GameManager>.Instance.Options.shadowsQuality = this.shadowsQuality.CurSel;
		}
		if (global::PandoraSingleton<global::GameManager>.Instance.Options.shadowCascades != this.shadowCascades.CurSel)
		{
			flag = true;
			global::PandoraSingleton<global::GameManager>.Instance.Options.shadowCascades = this.shadowCascades.CurSel;
		}
		if (global::PandoraSingleton<global::GameManager>.Instance.Options.graphicsDof != this.dof.isOn)
		{
			flag = true;
			global::PandoraSingleton<global::GameManager>.Instance.Options.graphicsDof = this.dof.isOn;
		}
		if (global::PandoraSingleton<global::GameManager>.Instance.Options.graphicsSsao != this.ssao.isOn)
		{
			flag = true;
			global::PandoraSingleton<global::GameManager>.Instance.Options.graphicsSsao = this.ssao.isOn;
		}
		if (global::PandoraSingleton<global::GameManager>.Instance.Options.graphicsSmaa != this.smaa.CurSel)
		{
			flag = true;
			global::PandoraSingleton<global::GameManager>.Instance.Options.graphicsSmaa = this.smaa.CurSel;
		}
		if (!global::UnityEngine.Mathf.Approximately(global::PandoraSingleton<global::GameManager>.Instance.Options.graphicsBrightness, (float)this.brightness.GetValue() / 100f))
		{
			flag = true;
			global::PandoraSingleton<global::GameManager>.Instance.Options.graphicsBrightness = (float)this.brightness.GetValue() / 100f;
		}
		if (!global::UnityEngine.Mathf.Approximately(global::PandoraSingleton<global::GameManager>.Instance.Options.graphicsGuiScale, (float)this.guiScale.GetValue() / 100f))
		{
			flag = true;
			global::PandoraSingleton<global::GameManager>.Instance.Options.graphicsGuiScale = (float)this.guiScale.GetValue() / 100f;
		}
		if (flag)
		{
			global::PandoraSingleton<global::GameManager>.Instance.SetGraphicOptions();
			this.needRevertOptionsConfirm = true;
		}
		return result;
	}

	private void InitMappings()
	{
		this.mappings.onAction.AddListener(new global::UnityEngine.Events.UnityAction(this.OnMappingPanelOpen));
		this.mappings.onSelect.AddListener(new global::UnityEngine.Events.UnityAction(this.OnMappingPanelDisplayed));
		this.mappings.toggle.onValueChanged.AddListener(new global::UnityEngine.Events.UnityAction<bool>(this.ClearMappings));
		this.InitMappingsData();
	}

	private void InitMappingsData()
	{
		this.FillControlsPanel();
		this.mappingChanged = false;
	}

	private void OnMappingPanelDisplayed()
	{
		global::UnityEngine.Debug.Log("OnMappingPanelDisplayed " + this.mappingsUnselected);
		if (this.mappingsUnselected)
		{
			this.SetMappingsPanelButtonsVisible(true);
			this.FillControlsPanel();
			base.StartCoroutine(this.RealignOnNextFrame());
		}
		else
		{
			this.controlsList.ResetSelection();
		}
	}

	private global::System.Collections.IEnumerator RealignOnNextFrame()
	{
		yield return null;
		this.controlsList.RealignList(true, 0, true);
		yield break;
	}

	private void OnMappingPanelOpen()
	{
		global::UnityEngine.Debug.Log("OnMappingPanelOpen " + this.mappingsUnselected);
		if (this.mappingsUnselected)
		{
			this.SetMappingsPanelButtonsVisible(true);
			this.FillControlsPanel();
		}
		this.OnPanelOpen();
		this.controlsList.GetComponentsInChildren<global::ToggleEffects>(true)[0].SetOn();
	}

	private void ClearMappings(bool value)
	{
		global::UnityEngine.Debug.Log("ClearMappings " + this.mappingsUnselected);
		if (!value)
		{
			this.controlsList.ClearList();
			this.controlsList.DestroyItems();
			this.mappingsUnselected = true;
		}
	}

	public void FillControlsPanel()
	{
		if (!base.gameObject.activeInHierarchy || !this.mappings.toggle.isOn)
		{
			return;
		}
		this.mappingsUnselected = false;
		global::UnityEngine.Debug.Log("Fill list");
		this.controlsList.ClearList();
		this.controlsList.Setup(this.controlEntry, false);
		global::System.Collections.Generic.IEnumerable<global::Rewired.InputAction> enumerable = global::Rewired.ReInput.mapping.UserAssignableActionsInCategory("game_input");
		foreach (global::Rewired.InputAction inputAction in enumerable)
		{
			if (inputAction.type == global::Rewired.InputActionType.Button)
			{
				if (inputAction.name == "more_info_unit")
				{
					string stringById = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(inputAction.descriptiveName);
					this.CreateControlMappingEntry(stringById, inputAction.id, true, "game_input", this.controlsList, true);
					int num = 0;
					foreach (global::Rewired.InputAction inputAction2 in enumerable)
					{
						if (inputAction2.name == "unit_info_buffs" || inputAction2.name == "unit_info_resists" || inputAction2.name == "unit_info_stats")
						{
							stringById = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(inputAction2.descriptiveName);
							this.CreateControlMappingEntry(stringById, inputAction2.id, true, "game_input", this.controlsList, false);
							if (++num >= 3)
							{
								break;
							}
						}
					}
				}
				else if (inputAction.name != "unit_info_buffs" && inputAction.name != "unit_info_resists" && inputAction.name != "unit_info_stats")
				{
					string stringById2 = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(inputAction.descriptiveName);
					this.CreateControlMappingEntry(stringById2, inputAction.id, true, "game_input", this.controlsList, true);
				}
			}
			else if (inputAction.name == "v" || inputAction.name == "cam_y")
			{
				string stringById3 = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(inputAction.positiveDescriptiveName);
				this.CreateControlMappingEntry(stringById3, inputAction.id, true, "game_input", this.controlsList, true);
				stringById3 = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(inputAction.negativeDescriptiveName);
				this.CreateControlMappingEntry(stringById3, inputAction.id, false, "game_input", this.controlsList, true);
			}
			else
			{
				string stringById4 = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(inputAction.negativeDescriptiveName);
				this.CreateControlMappingEntry(stringById4, inputAction.id, false, "game_input", this.controlsList, true);
				stringById4 = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(inputAction.positiveDescriptiveName);
				this.CreateControlMappingEntry(stringById4, inputAction.id, true, "game_input", this.controlsList, true);
			}
		}
		this.SetupControlsListNavig(this.controlsList);
		if (base.gameObject.activeInHierarchy)
		{
			base.StartCoroutine(this.OnListFilled());
		}
	}

	private void SetupControlsListNavig(global::ScrollGroup controlsList)
	{
		global::UIControlMappingItem[] componentsInChildren = controlsList.GetComponentsInChildren<global::UIControlMappingItem>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			global::UIControlMappingItem up;
			if (i == 0)
			{
				up = componentsInChildren[componentsInChildren.Length - 1];
			}
			else
			{
				up = componentsInChildren[i - 1];
			}
			global::UIControlMappingItem down;
			if (i == componentsInChildren.Length - 1)
			{
				down = componentsInChildren[0];
			}
			else
			{
				down = componentsInChildren[i + 1];
			}
			componentsInChildren[i].SetNav(up, down);
		}
	}

	private global::System.Collections.IEnumerator OnListFilled()
	{
		yield return null;
		if (base.gameObject.activeInHierarchy)
		{
			int timer = global::UnityEngine.Mathf.CeilToInt(this.restoreTime - global::UnityEngine.Time.time);
			if (this.confirmPopup.IsVisible && timer <= 0)
			{
				this.confirmPopup.Hide();
			}
			if (global::UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject == null)
			{
				this.controlsList.GetComponentsInChildren<global::ToggleEffects>(true)[0].SetOn();
			}
		}
		yield break;
	}

	private void CreateControlMappingEntry(string actionName, int actionId, bool isPositiveInput, string inputCategory, global::ScrollGroup controlsList, bool allowGamepadMapping = true)
	{
		global::UIControlMappingItem uicontrolMappingItem = controlsList.AddToList(null, null).GetComponentsInChildren<global::UIControlMappingItem>(true)[0];
		global::UnityEngine.UI.Toggle[] componentsInChildren = uicontrolMappingItem.GetComponentsInChildren<global::UnityEngine.UI.Toggle>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].group = this.mappingPanel;
		}
		global::UIGroupEffects[] componentsInChildren2 = uicontrolMappingItem.GetComponentsInChildren<global::UIGroupEffects>(true);
		for (int j = 0; j < componentsInChildren2.Length; j++)
		{
			componentsInChildren2[j].highlight = controlsList.GetComponentsInChildren<global::HightlightAnimate>(true)[0];
		}
		uicontrolMappingItem.actionLabel.text = actionName;
		uicontrolMappingItem.actionId = actionId;
		uicontrolMappingItem.isPositiveInput = isPositiveInput;
		uicontrolMappingItem.inputCategory = inputCategory;
		uicontrolMappingItem.OnMappingButton = new global::System.Action<global::UIControlMappingItem, int, global::Rewired.ActionElementMap>(this.OnButtonRemap);
		if (global::PandoraSingleton<global::PandoraInput>.Instance.player.controllers.Keyboard != null)
		{
			global::Rewired.ActionElementMap[] buttonMapsWithAction = global::PandoraSingleton<global::PandoraInput>.Instance.player.controllers.maps.GetFirstMapInCategory(global::Rewired.ControllerType.Keyboard, 0, uicontrolMappingItem.inputCategory).GetButtonMapsWithAction(actionId);
			if (buttonMapsWithAction != null)
			{
				for (int k = 0; k < buttonMapsWithAction.Length; k++)
				{
					if (buttonMapsWithAction[k].axisContribution == global::Rewired.Pole.Positive == isPositiveInput)
					{
						uicontrolMappingItem.SetMapping(buttonMapsWithAction[k], 0, true);
					}
				}
			}
		}
		if (global::PandoraSingleton<global::PandoraInput>.Instance.player.controllers.Mouse != null)
		{
			global::Rewired.ActionElementMap[] elementMapsWithAction = global::PandoraSingleton<global::PandoraInput>.Instance.player.controllers.maps.GetFirstMapInCategory(global::Rewired.ControllerType.Mouse, 0, uicontrolMappingItem.inputCategory).GetElementMapsWithAction(actionId);
			if (elementMapsWithAction != null)
			{
				for (int l = 0; l < elementMapsWithAction.Length; l++)
				{
					if (elementMapsWithAction[l].axisType != global::Rewired.AxisType.None || elementMapsWithAction[l].axisContribution == global::Rewired.Pole.Positive == isPositiveInput)
					{
						if (elementMapsWithAction[l].axisType != global::Rewired.AxisType.None)
						{
							uicontrolMappingItem.SetMapping(elementMapsWithAction[l], 0, false);
						}
						else
						{
							uicontrolMappingItem.SetMapping(elementMapsWithAction[l], 0, true);
						}
					}
				}
			}
		}
		if (global::PandoraSingleton<global::PandoraInput>.Instance.player.controllers.Joysticks.Count > 0)
		{
			if (!allowGamepadMapping)
			{
				uicontrolMappingItem.SetMapping(null, 1, false);
			}
			else
			{
				global::Rewired.ActionElementMap[] elementMapsWithAction2 = global::PandoraSingleton<global::PandoraInput>.Instance.player.controllers.maps.GetFirstMapInCategory(global::Rewired.ControllerType.Joystick, 0, uicontrolMappingItem.inputCategory).GetElementMapsWithAction(actionId);
				if (elementMapsWithAction2 != null)
				{
					for (int m = 0; m < elementMapsWithAction2.Length; m++)
					{
						if ((elementMapsWithAction2[m].axisRange == global::Rewired.AxisRange.Full && elementMapsWithAction2[m].axisType != global::Rewired.AxisType.None) || elementMapsWithAction2[m].axisContribution == global::Rewired.Pole.Positive == isPositiveInput)
						{
							string text = elementMapsWithAction2[m].elementIdentifierName.ToLowerInvariant().Replace(" ", "_");
							if (text.Equals("left_stick_x") || text.Equals("left_stick_y") || text.Equals("right_stick_x") || text.Equals("right_stick_y"))
							{
								uicontrolMappingItem.SetMapping(elementMapsWithAction2[m], 1, false);
							}
							else
							{
								uicontrolMappingItem.SetMapping(elementMapsWithAction2[m], 1, true);
							}
							break;
						}
					}
				}
			}
		}
	}

	public void OnButtonRemap(global::UIControlMappingItem mappingEntry, int buttonIndex, global::Rewired.ActionElementMap actionMap)
	{
		this.remappedEntry = mappingEntry;
		this.remappedButtonIndex = buttonIndex;
		this.remappedAction = actionMap;
		if (buttonIndex == 0 || global::PandoraSingleton<global::PandoraInput>.Instance.player.controllers.Joysticks.Count > 0)
		{
			this.controlsRemapPopup.Show(new global::System.Action<global::Rewired.Pole, global::Rewired.ControllerPollingInfo>(this.OnMapKeyPressed), (buttonIndex != 0) ? global::Rewired.ControllerType.Joystick : global::Rewired.ControllerType.Keyboard, mappingEntry.actionLabel.text);
		}
		else
		{
			string stringById = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("menu_no_controller_desc");
			this.confirmPopup.ShowLocalized(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("menu_no_controller_title"), stringById, null, false, false);
			this.confirmPopup.HideCancelButton();
		}
	}

	public void OnMapKeyPressed(global::Rewired.Pole pole, global::Rewired.ControllerPollingInfo pollInfo)
	{
		int keyIdentifier = -1;
		global::Rewired.ControllerType controllerType = pollInfo.controllerType;
		switch (controllerType)
		{
		case global::Rewired.ControllerType.Keyboard:
			keyIdentifier = (int)pollInfo.keyboardKey;
			goto IL_5B;
		case global::Rewired.ControllerType.Mouse:
		case global::Rewired.ControllerType.Joystick:
			break;
		default:
			if (controllerType != global::Rewired.ControllerType.Custom)
			{
				global::PandoraDebug.LogWarning("Unknown controller type", "INPUT_MAPPING", this);
				goto IL_5B;
			}
			break;
		}
		keyIdentifier = pollInfo.elementIdentifierId;
		IL_5B:
		this.newMapInput = pollInfo;
		this.remappedEntry.mappingButtons[this.remappedButtonIndex].SetSelected(true);
		this.conflictingAction = global::PandoraSingleton<global::PandoraInput>.Instance.GetFirstConflictingActionMap(this.remappedEntry.actionId, this.remappedEntry.inputCategory, this.newMapInput.controllerType, keyIdentifier);
		if (this.conflictingAction != null && (this.conflictingAction.actionId != this.remappedEntry.actionId || this.conflictingAction.axisContribution == global::Rewired.Pole.Positive != this.remappedEntry.isPositiveInput))
		{
			global::Rewired.InputAction action = global::Rewired.ReInput.mapping.GetAction(this.conflictingAction.actionId);
			string key;
			if (action.type == global::Rewired.InputActionType.Button)
			{
				key = action.descriptiveName;
			}
			else if (this.conflictingAction.axisContribution == global::Rewired.Pole.Positive)
			{
				key = action.positiveDescriptiveName;
			}
			else
			{
				key = action.negativeDescriptiveName;
			}
			this.newMapKeyPole = pole;
			string stringById = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("menu_already_mapped", new string[]
			{
				global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(key)
			});
			this.confirmPopup.ShowLocalized(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("menu_already_mapped_title"), stringById, new global::System.Action<bool>(this.OnSwapMappingPopupResult), false, false);
		}
		else
		{
			switch (this.newMapInput.controllerType)
			{
			case global::Rewired.ControllerType.Keyboard:
				this.MapNewKeyboardKey(this.newMapInput.keyboardKey);
				break;
			case global::Rewired.ControllerType.Mouse:
				this.MapNewMouseButton(this.newMapInput.elementIdentifierId);
				break;
			case global::Rewired.ControllerType.Joystick:
				this.MapNewJoystickKey(this.newMapInput);
				break;
			}
		}
	}

	public void OnSwapMappingPopupResult(bool confirm)
	{
		if (confirm)
		{
			if (this.remappedButtonIndex == 0)
			{
				global::Rewired.ActionElementMap mapping = null;
				if (this.remappedAction == null)
				{
					global::PandoraSingleton<global::PandoraInput>.Instance.player.controllers.maps.GetFirstMapInCategory(this.newMapInput.controllerType, 0, this.remappedEntry.inputCategory).DeleteElementMap(this.conflictingAction.id);
					mapping = null;
				}
				else
				{
					global::Rewired.ControllerType controllerType;
					if (this.remappedAction.keyCode == global::UnityEngine.KeyCode.None)
					{
						controllerType = global::Rewired.ControllerType.Mouse;
					}
					else
					{
						controllerType = global::Rewired.ControllerType.Keyboard;
					}
					if (controllerType == global::Rewired.ControllerType.Keyboard)
					{
						if (this.newMapInput.controllerType == global::Rewired.ControllerType.Keyboard)
						{
							global::Rewired.ElementAssignment elementAssignment = new global::Rewired.ElementAssignment(this.remappedAction.keyCode, global::Rewired.ModifierKeyFlags.None, this.conflictingAction.actionId, this.conflictingAction.axisContribution, this.conflictingAction.id);
							global::PandoraSingleton<global::PandoraInput>.Instance.player.controllers.maps.GetFirstMapInCategory(this.newMapInput.controllerType, 0, this.remappedEntry.inputCategory).ReplaceElementMap(elementAssignment);
						}
						else
						{
							global::PandoraSingleton<global::PandoraInput>.Instance.player.controllers.maps.GetFirstMapInCategory(this.newMapInput.controllerType, 0, this.remappedEntry.inputCategory).DeleteElementMap(this.conflictingAction.id);
							global::Rewired.ElementAssignment elementAssignment = new global::Rewired.ElementAssignment(this.remappedAction.keyCode, global::Rewired.ModifierKeyFlags.None, this.conflictingAction.actionId, this.conflictingAction.axisContribution);
							global::PandoraSingleton<global::PandoraInput>.Instance.player.controllers.maps.GetFirstMapInCategory(controllerType, 0, this.remappedEntry.inputCategory).CreateElementMap(elementAssignment);
						}
					}
					else if (this.newMapInput.controllerType == global::Rewired.ControllerType.Mouse)
					{
						global::Rewired.ElementAssignment elementAssignment = new global::Rewired.ElementAssignment(this.remappedAction.elementIdentifierId, this.conflictingAction.actionId, this.conflictingAction.axisContribution, this.conflictingAction.id);
						global::PandoraSingleton<global::PandoraInput>.Instance.player.controllers.maps.GetFirstMapInCategory(this.newMapInput.controllerType, 0, this.remappedEntry.inputCategory).ReplaceElementMap(elementAssignment);
					}
					else
					{
						global::PandoraSingleton<global::PandoraInput>.Instance.player.controllers.maps.GetFirstMapInCategory(this.newMapInput.controllerType, 0, this.remappedEntry.inputCategory).DeleteElementMap(this.conflictingAction.id);
						global::Rewired.ElementAssignment elementAssignment = new global::Rewired.ElementAssignment(this.remappedAction.elementIdentifierId, this.conflictingAction.actionId, this.conflictingAction.axisContribution);
						global::PandoraSingleton<global::PandoraInput>.Instance.player.controllers.maps.GetFirstMapInCategory(controllerType, 0, this.remappedEntry.inputCategory).CreateElementMap(elementAssignment);
					}
					foreach (global::Rewired.ActionElementMap actionElementMap in global::PandoraSingleton<global::PandoraInput>.Instance.player.controllers.maps.GetFirstMapInCategory(controllerType, 0, this.remappedEntry.inputCategory).GetElementMapsWithAction(this.conflictingAction.actionId))
					{
						if (actionElementMap.keyCode == this.remappedAction.keyCode)
						{
							mapping = actionElementMap;
							break;
						}
					}
				}
				for (int j = 0; j < this.controlsList.items.Count; j++)
				{
					global::UIControlMappingItem component = this.controlsList.items[j].GetComponent<global::UIControlMappingItem>();
					if (component.actionId == this.conflictingAction.actionId && component.isPositiveInput == (this.conflictingAction.axisContribution == global::Rewired.Pole.Positive))
					{
						component.SetMapping(mapping, this.remappedButtonIndex, true);
						break;
					}
				}
				if (this.newMapInput.controllerType == global::Rewired.ControllerType.Keyboard)
				{
					this.MapNewKeyboardKey(this.newMapInput.keyboardKey);
				}
				else
				{
					this.MapNewMouseButton(this.newMapInput.elementIdentifierId);
				}
			}
			else
			{
				global::Rewired.ActionElementMap mapping2 = null;
				if (this.remappedAction == null)
				{
					global::PandoraSingleton<global::PandoraInput>.Instance.player.controllers.maps.GetFirstMapInCategory(global::Rewired.ControllerType.Joystick, 0, this.remappedEntry.inputCategory).DeleteElementMap(this.conflictingAction.id);
					mapping2 = null;
				}
				else
				{
					global::Rewired.ElementAssignment elementAssignment2 = new global::Rewired.ElementAssignment(global::Rewired.ControllerType.Joystick, this.remappedAction.elementType, this.remappedAction.elementIdentifierId, this.remappedAction.axisRange, this.remappedAction.keyCode, this.remappedAction.modifierKeyFlags, this.conflictingAction.actionId, this.conflictingAction.axisContribution, this.conflictingAction.invert, this.conflictingAction.id);
					global::Rewired.ControllerMap firstMapInCategory = global::PandoraSingleton<global::PandoraInput>.Instance.player.controllers.maps.GetFirstMapInCategory(global::Rewired.ControllerType.Joystick, 0, this.remappedEntry.inputCategory);
					bool flag = firstMapInCategory.ReplaceElementMap(elementAssignment2);
					foreach (global::Rewired.ActionElementMap actionElementMap2 in firstMapInCategory.GetElementMapsWithAction(this.conflictingAction.actionId))
					{
						if (actionElementMap2.elementIdentifierId == this.remappedAction.elementIdentifierId)
						{
							mapping2 = actionElementMap2;
							break;
						}
					}
				}
				for (int l = 0; l < this.controlsList.items.Count; l++)
				{
					global::UIControlMappingItem component2 = this.controlsList.items[l].GetComponent<global::UIControlMappingItem>();
					if (component2.actionId == this.conflictingAction.actionId && component2.isPositiveInput == (this.conflictingAction.axisContribution == global::Rewired.Pole.Positive))
					{
						component2.SetMapping(mapping2, this.remappedButtonIndex, true);
						break;
					}
				}
				this.MapNewJoystickKey(this.newMapInput);
			}
		}
		this.remappedEntry.mappingButtons[this.remappedButtonIndex].SetSelected(true);
	}

	private void MapNewJoystickKey(global::Rewired.ControllerPollingInfo pollInfo)
	{
		if (this.remappedAction != null)
		{
			global::Rewired.ElementAssignment elementAssignment = new global::Rewired.ElementAssignment(pollInfo.controllerType, pollInfo.elementType, pollInfo.elementIdentifierId, (pollInfo.elementType != global::Rewired.ControllerElementType.Button) ? ((pollInfo.axisPole != global::Rewired.Pole.Negative) ? global::Rewired.AxisRange.Positive : global::Rewired.AxisRange.Negative) : global::Rewired.AxisRange.Full, pollInfo.keyboardKey, global::Rewired.ModifierKeyFlags.None, this.remappedEntry.actionId, (!this.remappedEntry.isPositiveInput) ? global::Rewired.Pole.Negative : global::Rewired.Pole.Positive, this.remappedAction.invert, this.remappedAction.id);
			global::PandoraSingleton<global::PandoraInput>.Instance.player.controllers.maps.GetFirstMapInCategory(global::Rewired.ControllerType.Joystick, 0, this.remappedEntry.inputCategory).ReplaceElementMap(elementAssignment);
		}
		else
		{
			global::Rewired.ElementAssignment elementAssignment2 = new global::Rewired.ElementAssignment(pollInfo.controllerType, pollInfo.elementType, pollInfo.elementIdentifierId, (pollInfo.elementType != global::Rewired.ControllerElementType.Button) ? ((pollInfo.axisPole != global::Rewired.Pole.Negative) ? global::Rewired.AxisRange.Positive : global::Rewired.AxisRange.Negative) : global::Rewired.AxisRange.Full, pollInfo.keyboardKey, global::Rewired.ModifierKeyFlags.None, this.remappedEntry.actionId, (!this.remappedEntry.isPositiveInput) ? global::Rewired.Pole.Negative : global::Rewired.Pole.Positive, false);
			global::PandoraSingleton<global::PandoraInput>.Instance.player.controllers.maps.GetFirstMapInCategory(global::Rewired.ControllerType.Joystick, 0, this.remappedEntry.inputCategory).CreateElementMap(elementAssignment2);
		}
		global::Rewired.ActionElementMap mapping = null;
		foreach (global::Rewired.ActionElementMap actionElementMap in global::PandoraSingleton<global::PandoraInput>.Instance.player.controllers.maps.GetFirstMapInCategory(global::Rewired.ControllerType.Joystick, 0, this.remappedEntry.inputCategory).GetElementMapsWithAction(this.remappedEntry.actionId))
		{
			if (actionElementMap.elementIdentifierId == pollInfo.elementIdentifierId)
			{
				mapping = actionElementMap;
				break;
			}
		}
		this.remappedEntry.SetMapping(mapping, this.remappedButtonIndex, true);
		this.mappingChanged = true;
	}

	private void MapNewMouseButton(int buttonId)
	{
		if (this.remappedAction != null)
		{
			if (this.remappedAction.keyCode == global::UnityEngine.KeyCode.None)
			{
				global::Rewired.ElementAssignment elementAssignment = new global::Rewired.ElementAssignment(buttonId, this.remappedEntry.actionId, (!this.remappedEntry.isPositiveInput) ? global::Rewired.Pole.Negative : global::Rewired.Pole.Positive, this.remappedAction.id);
				global::PandoraSingleton<global::PandoraInput>.Instance.player.controllers.maps.GetFirstMapInCategory(global::Rewired.ControllerType.Mouse, 0, this.remappedEntry.inputCategory).ReplaceOrCreateElementMap(elementAssignment);
			}
			else
			{
				global::PandoraSingleton<global::PandoraInput>.Instance.player.controllers.maps.GetFirstMapInCategory(global::Rewired.ControllerType.Keyboard, 0, this.remappedEntry.inputCategory).DeleteElementMap(this.remappedAction.id);
				global::Rewired.ElementAssignment elementAssignment2 = new global::Rewired.ElementAssignment(buttonId, this.remappedEntry.actionId, (!this.remappedEntry.isPositiveInput) ? global::Rewired.Pole.Negative : global::Rewired.Pole.Positive);
				global::PandoraSingleton<global::PandoraInput>.Instance.player.controllers.maps.GetFirstMapInCategory(global::Rewired.ControllerType.Mouse, 0, this.remappedEntry.inputCategory).CreateElementMap(elementAssignment2);
			}
		}
		else
		{
			global::Rewired.ElementAssignment elementAssignment3 = new global::Rewired.ElementAssignment(buttonId, this.remappedEntry.actionId, (!this.remappedEntry.isPositiveInput) ? global::Rewired.Pole.Negative : global::Rewired.Pole.Positive);
			global::PandoraSingleton<global::PandoraInput>.Instance.player.controllers.maps.GetFirstMapInCategory(global::Rewired.ControllerType.Mouse, 0, this.remappedEntry.inputCategory).CreateElementMap(elementAssignment3);
		}
		global::Rewired.ActionElementMap mapping = null;
		foreach (global::Rewired.ActionElementMap actionElementMap in global::PandoraSingleton<global::PandoraInput>.Instance.player.controllers.maps.GetFirstMapInCategory(global::Rewired.ControllerType.Mouse, 0, this.remappedEntry.inputCategory).GetElementMapsWithAction(this.remappedEntry.actionId))
		{
			if (actionElementMap.elementIdentifierId == buttonId)
			{
				mapping = actionElementMap;
				break;
			}
		}
		this.remappedEntry.SetMapping(mapping, this.remappedButtonIndex, true);
		this.mappingChanged = true;
	}

	private void MapNewKeyboardKey(global::UnityEngine.KeyCode key)
	{
		if (this.remappedAction != null)
		{
			if (this.remappedAction.keyCode != global::UnityEngine.KeyCode.None)
			{
				global::Rewired.ElementAssignment elementAssignment = new global::Rewired.ElementAssignment(key, global::Rewired.ModifierKeyFlags.None, this.remappedEntry.actionId, (!this.remappedEntry.isPositiveInput) ? global::Rewired.Pole.Negative : global::Rewired.Pole.Positive, this.remappedAction.id);
				global::PandoraSingleton<global::PandoraInput>.Instance.player.controllers.maps.GetFirstMapInCategory(global::Rewired.ControllerType.Keyboard, 0, this.remappedEntry.inputCategory).ReplaceOrCreateElementMap(elementAssignment);
			}
			else
			{
				global::PandoraSingleton<global::PandoraInput>.Instance.player.controllers.maps.GetFirstMapInCategory(global::Rewired.ControllerType.Mouse, 0, this.remappedEntry.inputCategory).DeleteElementMap(this.remappedAction.id);
				global::PandoraSingleton<global::PandoraInput>.Instance.MapKeyboardKey(this.remappedEntry.inputCategory, key, this.remappedEntry.actionId, this.remappedEntry.isPositiveInput);
			}
		}
		else
		{
			global::PandoraSingleton<global::PandoraInput>.Instance.MapKeyboardKey(this.remappedEntry.inputCategory, key, this.remappedEntry.actionId, this.remappedEntry.isPositiveInput);
		}
		global::Rewired.ActionElementMap mapping = null;
		foreach (global::Rewired.ActionElementMap actionElementMap in global::PandoraSingleton<global::PandoraInput>.Instance.player.controllers.maps.GetFirstMapInCategory(global::Rewired.ControllerType.Keyboard, 0, this.remappedEntry.inputCategory).GetElementMapsWithAction(this.remappedEntry.actionId))
		{
			if (actionElementMap.keyCode == key)
			{
				mapping = actionElementMap;
				break;
			}
		}
		this.remappedEntry.SetMapping(mapping, this.remappedButtonIndex, true);
		this.mappingChanged = true;
	}

	public void ResetMappings()
	{
		this.lastSelection = global::UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
		this.confirmPopup.Show("menu_restore_default_mappings", "menu_confirm_restore_mappings", new global::System.Action<bool>(this.OnConfirmResetMappings), false, false);
	}

	public void OnConfirmResetMappings(bool confirmed)
	{
		if (confirmed)
		{
			global::PandoraSingleton<global::PandoraInput>.Instance.RestoreDefaultMappings();
			this.FillControlsPanel();
			this.mappings.SetSelected(true);
			this.panelOpen = false;
			this.mappingChanged = true;
		}
		else if (this.lastSelection != null)
		{
			this.lastSelection.SetSelected(true);
		}
	}

	private void UntoggleSelected()
	{
		if (global::UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject != null)
		{
			global::UnityEngine.UI.Toggle component = global::UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<global::UnityEngine.UI.Toggle>();
			if (component != null)
			{
				component.isOn = false;
			}
		}
	}

	private void SaveJoystickMappingData()
	{
		global::Rewired.JoystickMapSaveData[] mapSaveData = global::PandoraSingleton<global::PandoraInput>.Instance.player.controllers.maps.GetMapSaveData<global::Rewired.JoystickMapSaveData>(0, true);
		global::PandoraSingleton<global::GameManager>.Instance.Options.joystickMappingData = new global::System.Collections.Generic.List<string>();
		if (mapSaveData != null)
		{
			for (int i = 0; i < mapSaveData.Length; i++)
			{
				global::PandoraSingleton<global::GameManager>.Instance.Options.joystickMappingData.Add(mapSaveData[i].map.ToXmlString());
			}
		}
	}

	private void SaveKeyboardMappingData()
	{
		global::Rewired.KeyboardMapSaveData[] mapSaveData = global::PandoraSingleton<global::PandoraInput>.Instance.player.controllers.maps.GetMapSaveData<global::Rewired.KeyboardMapSaveData>(0, true);
		global::PandoraSingleton<global::GameManager>.Instance.Options.keyboardMappingData = new global::System.Collections.Generic.List<string>();
		if (mapSaveData != null)
		{
			for (int i = 0; i < mapSaveData.Length; i++)
			{
				global::PandoraSingleton<global::GameManager>.Instance.Options.keyboardMappingData.Add(mapSaveData[i].map.ToXmlString());
			}
		}
	}

	private void SaveMouseMappingData()
	{
		global::Rewired.MouseMapSaveData[] mapSaveData = global::PandoraSingleton<global::PandoraInput>.Instance.player.controllers.maps.GetMapSaveData<global::Rewired.MouseMapSaveData>(0, true);
		global::PandoraSingleton<global::GameManager>.Instance.Options.mouseMappingData = new global::System.Collections.Generic.List<string>();
		if (mapSaveData != null)
		{
			for (int i = 0; i < mapSaveData.Length; i++)
			{
				global::PandoraSingleton<global::GameManager>.Instance.Options.mouseMappingData.Add(mapSaveData[i].map.ToXmlString());
			}
		}
	}

	private bool ApplyMappingChanges()
	{
		this.SaveJoystickMappingData();
		this.SaveKeyboardMappingData();
		this.SaveMouseMappingData();
		this.mappingChanged = false;
		return true;
	}

	private void CancelMappingChanges()
	{
		if (this.mappingChanged)
		{
			global::PandoraSingleton<global::GameManager>.Instance.SetMappingOptions();
			this.mappingChanged = false;
		}
	}

	private bool CheckMappingNeedApply()
	{
		if (this.mappingChanged)
		{
			global::PandoraDebug.LogDebug("Options require Apply action, mappings changed ", "OPTIONS", this);
		}
		return this.mappingChanged;
	}

	private void InitControls()
	{
		this.controlsOptions.onAction.AddListener(new global::UnityEngine.Events.UnityAction(this.OnControlsPanelOpen));
		this.controlsOptions.onSelect.AddListener(new global::UnityEngine.Events.UnityAction(this.OnControlsPanelDisplayed));
		this.InitControlsData();
	}

	private void InitControlsData()
	{
		this.invertCameraHorizontalEnabled.isOn = global::PandoraSingleton<global::GameManager>.Instance.Options.cameraXInverted;
		this.invertCameraVerticalEnabled.isOn = global::PandoraSingleton<global::GameManager>.Instance.Options.cameraYInverted;
		this.joystickSensitivitySlider.SetValue(global::PandoraSingleton<global::GameManager>.Instance.Options.joystickSensitivity * 100f);
		this.leftHandedControllerEnabled.isOn = global::PandoraSingleton<global::GameManager>.Instance.Options.leftHandedController;
		this.leftHandedMouseEnabled.isOn = global::PandoraSingleton<global::GameManager>.Instance.Options.leftHandedMouse;
		this.mouseSensitivitySlider.SetValue(global::PandoraSingleton<global::GameManager>.Instance.Options.mouseSensitivity * 100f);
		this.gamepadEnabled.isOn = global::PandoraSingleton<global::GameManager>.Instance.Options.gamepadEnabled;
	}

	private void OnControlsPanelDisplayed()
	{
		this.SetMappingsPanelButtonsVisible(false);
	}

	private void OnControlsPanelOpen()
	{
		this.OnPanelOpen();
		this.gamepadEnabled.SetSelected(true);
		this.OnControlsPanelDisplayed();
	}

	private bool CheckControlsNeedApply()
	{
		if (global::PandoraSingleton<global::GameManager>.Instance.Options.gamepadEnabled != this.gamepadEnabled.isOn)
		{
			global::PandoraDebug.LogDebug(string.Concat(new object[]
			{
				"Options require Apply action, gamepadEnabled changed from ",
				global::PandoraSingleton<global::GameManager>.Instance.Options.gamepadEnabled,
				" to ",
				this.gamepadEnabled.isOn
			}), "OPTIONS", this);
			return true;
		}
		if (global::PandoraSingleton<global::GameManager>.Instance.Options.cameraXInverted != this.invertCameraHorizontalEnabled.isOn)
		{
			global::PandoraDebug.LogDebug(string.Concat(new object[]
			{
				"Options require Apply action, cameraXInvert changed from ",
				global::PandoraSingleton<global::GameManager>.Instance.Options.cameraXInverted,
				" to ",
				this.invertCameraHorizontalEnabled.isOn
			}), "OPTIONS", this);
			return true;
		}
		if (global::PandoraSingleton<global::GameManager>.Instance.Options.cameraYInverted != this.invertCameraVerticalEnabled.isOn)
		{
			global::PandoraDebug.LogDebug(string.Concat(new object[]
			{
				"Options require Apply action, cameraYInvert changed from ",
				global::PandoraSingleton<global::GameManager>.Instance.Options.cameraYInverted,
				" to ",
				this.invertCameraVerticalEnabled.isOn
			}), "OPTIONS", this);
			return true;
		}
		if (global::PandoraSingleton<global::GameManager>.Instance.Options.leftHandedMouse != this.leftHandedMouseEnabled.isOn)
		{
			global::PandoraDebug.LogDebug(string.Concat(new object[]
			{
				"Options require Apply action, left handed mouse changed from ",
				global::PandoraSingleton<global::GameManager>.Instance.Options.leftHandedMouse,
				" to ",
				this.leftHandedMouseEnabled.isOn
			}), "OPTIONS", this);
			return true;
		}
		if (!global::UnityEngine.Mathf.Approximately(global::PandoraSingleton<global::GameManager>.Instance.Options.mouseSensitivity, (float)this.mouseSensitivitySlider.GetValue() / 100f))
		{
			global::PandoraDebug.LogDebug(string.Concat(new object[]
			{
				"Options require Apply action, mouseSensitivity changed from ",
				global::PandoraSingleton<global::GameManager>.Instance.Options.mouseSensitivity,
				" to ",
				(float)this.mouseSensitivitySlider.GetValue() / 100f
			}), "OPTIONS", this);
			return true;
		}
		if (global::PandoraSingleton<global::GameManager>.Instance.Options.leftHandedController != this.leftHandedControllerEnabled.isOn)
		{
			global::PandoraDebug.LogDebug(string.Concat(new object[]
			{
				"Options require Apply action, left handed controller changed from ",
				global::PandoraSingleton<global::GameManager>.Instance.Options.leftHandedController,
				" to ",
				this.leftHandedControllerEnabled.isOn
			}), "OPTIONS", this);
			return true;
		}
		if (!global::UnityEngine.Mathf.Approximately(global::PandoraSingleton<global::GameManager>.Instance.Options.joystickSensitivity, (float)this.joystickSensitivitySlider.GetValue() / 100f))
		{
			global::PandoraDebug.LogDebug(string.Concat(new object[]
			{
				"Options require Apply action, joystickSensitibity changed from ",
				global::PandoraSingleton<global::GameManager>.Instance.Options.joystickSensitivity,
				" to ",
				(float)this.joystickSensitivitySlider.GetValue() / 100f
			}), "OPTIONS", this);
			return true;
		}
		return false;
	}

	private bool ApplyControlsChanges()
	{
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		if (global::PandoraSingleton<global::GameManager>.Instance.Options.gamepadEnabled != this.gamepadEnabled.isOn)
		{
			flag = true;
			global::PandoraSingleton<global::GameManager>.Instance.Options.gamepadEnabled = this.gamepadEnabled.isOn;
		}
		if (global::PandoraSingleton<global::GameManager>.Instance.Options.cameraXInverted != this.invertCameraHorizontalEnabled.isOn)
		{
			flag = true;
			global::PandoraSingleton<global::GameManager>.Instance.Options.cameraXInverted = this.invertCameraHorizontalEnabled.isOn;
			flag2 = true;
			flag3 = true;
		}
		if (global::PandoraSingleton<global::GameManager>.Instance.Options.cameraYInverted != this.invertCameraVerticalEnabled.isOn)
		{
			flag = true;
			global::PandoraSingleton<global::GameManager>.Instance.Options.cameraYInverted = this.invertCameraVerticalEnabled.isOn;
			flag2 = true;
			flag3 = true;
		}
		if (global::PandoraSingleton<global::GameManager>.Instance.Options.leftHandedMouse != this.leftHandedMouseEnabled.isOn)
		{
			flag = true;
			global::PandoraSingleton<global::GameManager>.Instance.Options.leftHandedMouse = this.leftHandedMouseEnabled.isOn;
			flag2 = true;
		}
		if (!global::UnityEngine.Mathf.Approximately(global::PandoraSingleton<global::GameManager>.Instance.Options.mouseSensitivity, (float)this.mouseSensitivitySlider.GetValue() / 100f))
		{
			flag = true;
			global::PandoraSingleton<global::GameManager>.Instance.Options.mouseSensitivity = (float)this.mouseSensitivitySlider.GetValue() / 100f;
		}
		if (global::PandoraSingleton<global::GameManager>.Instance.Options.leftHandedController != this.leftHandedControllerEnabled.isOn)
		{
			flag = true;
			global::PandoraSingleton<global::GameManager>.Instance.Options.leftHandedController = this.leftHandedControllerEnabled.isOn;
			flag3 = true;
		}
		if (!global::UnityEngine.Mathf.Approximately(global::PandoraSingleton<global::GameManager>.Instance.Options.joystickSensitivity, (float)this.joystickSensitivitySlider.GetValue() / 100f))
		{
			flag = true;
			global::PandoraSingleton<global::GameManager>.Instance.Options.joystickSensitivity = (float)this.joystickSensitivitySlider.GetValue() / 100f;
		}
		if (flag)
		{
			global::PandoraSingleton<global::GameManager>.Instance.SetControlsOptions();
			if (flag2)
			{
				this.SaveMouseMappingData();
			}
			if (flag3)
			{
				this.SaveJoystickMappingData();
			}
		}
		return true;
	}

	private void InitGameplay()
	{
		this.gameplayOptions.onAction.AddListener(new global::UnityEngine.Events.UnityAction(this.OnGameplayPanelOpen));
		this.gameplayOptions.onSelect.AddListener(new global::UnityEngine.Events.UnityAction(this.OnGameplayPanelDisplayed));
		this.InitGameplayData();
	}

	private void InitGameplayData()
	{
		this.languageSelect.selections.Clear();
		int currentSel = 0;
		for (int i = 0; i < this.availableLangs.Count; i++)
		{
			this.languageSelect.selections.Add(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("lang_" + this.availableLangs[i].ToString()));
			if (this.availableLangs[i] == (global::SupportedLanguage)global::PandoraSingleton<global::GameManager>.Instance.Options.language)
			{
				currentSel = i;
			}
		}
		this.languageSelect.SetCurrentSel(currentSel);
		this.tacticalViewHelpersEnabled.isOn = global::PandoraSingleton<global::GameManager>.Instance.Options.tacticalViewHelpersEnabled;
		this.wagonBeaconsEnabled.isOn = global::PandoraSingleton<global::GameManager>.Instance.Options.wagonBeaconsEnabled;
		this.autoExitTacticalEnabled.isOn = global::PandoraSingleton<global::GameManager>.Instance.Options.autoExitTacticalEnabled;
		this.displayFullUI.isOn = global::PandoraSingleton<global::GameManager>.Instance.Options.displayFullUI;
		this.fastForward.isOn = global::PandoraSingleton<global::GameManager>.Instance.Options.fastForwarded;
		this.skipTuto.isOn = global::PandoraSingleton<global::GameManager>.Instance.Options.skipTuto;
	}

	private void OnGameplayPanelDisplayed()
	{
		this.SetMappingsPanelButtonsVisible(false);
	}

	private void OnGameplayPanelOpen()
	{
		this.OnPanelOpen();
		this.languageSelect.selection.SetSelected(true);
		this.OnGameplayPanelDisplayed();
	}

	private bool CheckGameplayNeedApply()
	{
		if (global::PandoraSingleton<global::GameManager>.Instance.Options.language != this.languageSelect.CurSel)
		{
			global::PandoraDebug.LogDebug(string.Concat(new object[]
			{
				"Options require Apply action, language changed from ",
				global::PandoraSingleton<global::GameManager>.Instance.Options.language,
				" to ",
				this.gamepadEnabled.isOn
			}), "OPTIONS", this);
			return true;
		}
		if (global::PandoraSingleton<global::GameManager>.Instance.Options.tacticalViewHelpersEnabled != this.tacticalViewHelpersEnabled.isOn)
		{
			global::PandoraDebug.LogDebug(string.Concat(new object[]
			{
				"Options require Apply action, tacticalviewHelpers changed from ",
				global::PandoraSingleton<global::GameManager>.Instance.Options.tacticalViewHelpersEnabled,
				" to ",
				this.tacticalViewHelpersEnabled.isOn
			}), "OPTIONS", this);
			return true;
		}
		if (global::PandoraSingleton<global::GameManager>.Instance.Options.wagonBeaconsEnabled != this.wagonBeaconsEnabled.isOn)
		{
			global::PandoraDebug.LogDebug(string.Concat(new object[]
			{
				"Options require Apply action, wagonBeacons changed from ",
				global::PandoraSingleton<global::GameManager>.Instance.Options.wagonBeaconsEnabled,
				" to ",
				this.wagonBeaconsEnabled.isOn
			}), "OPTIONS", this);
			return true;
		}
		if (global::PandoraSingleton<global::GameManager>.Instance.Options.autoExitTacticalEnabled != this.autoExitTacticalEnabled.isOn)
		{
			global::PandoraDebug.LogDebug(string.Concat(new object[]
			{
				"Options require Apply action, autoExitTactical changed from ",
				global::PandoraSingleton<global::GameManager>.Instance.Options.autoExitTacticalEnabled,
				" to ",
				this.autoExitTacticalEnabled.isOn
			}), "OPTIONS", this);
			return true;
		}
		if (global::PandoraSingleton<global::GameManager>.Instance.Options.fastForwarded != this.fastForward.isOn)
		{
			global::PandoraDebug.LogDebug(string.Concat(new object[]
			{
				"Options require Apply action, fastForwarded changed from ",
				global::PandoraSingleton<global::GameManager>.Instance.Options.fastForwarded,
				" to ",
				this.fastForward.isOn
			}), "OPTIONS", this);
			return true;
		}
		if (global::PandoraSingleton<global::GameManager>.Instance.Options.skipTuto != this.skipTuto.isOn)
		{
			global::PandoraDebug.LogDebug(string.Concat(new object[]
			{
				"Options require Apply action, skipTuto changed from ",
				global::PandoraSingleton<global::GameManager>.Instance.Options.skipTuto,
				" to ",
				this.skipTuto.isOn
			}), "OPTIONS", this);
			return true;
		}
		if (global::PandoraSingleton<global::GameManager>.Instance.Options.displayFullUI != this.displayFullUI.isOn)
		{
			global::PandoraDebug.LogDebug(string.Concat(new object[]
			{
				"Options require Apply action, displayFullUI changed from ",
				global::PandoraSingleton<global::GameManager>.Instance.Options.displayFullUI,
				" to ",
				this.displayFullUI.isOn
			}), "OPTIONS", this);
			return true;
		}
		return false;
	}

	private bool ApplyGameplayChanges()
	{
		bool flag = false;
		bool result = true;
		if (global::PandoraSingleton<global::GameManager>.Instance.Options.language != this.languageSelect.CurSel)
		{
			flag = true;
			result = false;
			global::PandoraSingleton<global::GameManager>.Instance.Options.language = (int)this.availableLangs[this.languageSelect.CurSel];
		}
		if (global::PandoraSingleton<global::GameManager>.Instance.Options.tacticalViewHelpersEnabled != this.tacticalViewHelpersEnabled.isOn)
		{
			flag = true;
			global::PandoraSingleton<global::GameManager>.Instance.Options.tacticalViewHelpersEnabled = this.tacticalViewHelpersEnabled.isOn;
		}
		if (global::PandoraSingleton<global::GameManager>.Instance.Options.wagonBeaconsEnabled != this.wagonBeaconsEnabled.isOn)
		{
			flag = true;
			global::PandoraSingleton<global::GameManager>.Instance.Options.wagonBeaconsEnabled = this.wagonBeaconsEnabled.isOn;
		}
		if (global::PandoraSingleton<global::GameManager>.Instance.Options.autoExitTacticalEnabled != this.autoExitTacticalEnabled.isOn)
		{
			flag = true;
			global::PandoraSingleton<global::GameManager>.Instance.Options.autoExitTacticalEnabled = this.autoExitTacticalEnabled.isOn;
		}
		if (global::PandoraSingleton<global::GameManager>.Instance.Options.displayFullUI != this.displayFullUI.isOn)
		{
			flag = true;
			global::PandoraSingleton<global::GameManager>.Instance.Options.displayFullUI = this.displayFullUI.isOn;
			if (global::PandoraSingleton<global::UIMissionManager>.Exists())
			{
				global::PandoraSingleton<global::UIMissionManager>.Instance.OnSetOptionFullUI(true);
			}
		}
		if (global::PandoraSingleton<global::GameManager>.Instance.Options.fastForwarded != this.fastForward.isOn)
		{
			flag = true;
			global::PandoraSingleton<global::GameManager>.Instance.Options.fastForwarded = this.fastForward.isOn;
		}
		if (global::PandoraSingleton<global::GameManager>.Instance.Options.skipTuto != this.skipTuto.isOn)
		{
			flag = true;
			global::PandoraSingleton<global::GameManager>.Instance.Options.skipTuto = this.skipTuto.isOn;
		}
		if (flag)
		{
			global::PandoraSingleton<global::GameManager>.Instance.SetGameplayOptions();
		}
		return result;
	}

	private const float RESTORE_DELAY = 15f;

	private global::CameraManager camMan;

	[global::UnityEngine.Header("UI items")]
	public global::ConfirmationPopupView confirmPopup;

	public global::ButtonGroup butExit;

	public global::ButtonGroup butBack;

	public global::ButtonGroup butRestore;

	public global::ButtonGroup butApply;

	public global::System.Action onSaveQuitGame;

	public global::System.Action onCloseOptionsMenu;

	public global::System.Action<bool> onQuitGame;

	public global::UnityEngine.Sprite icnBack;

	public global::UnityEngine.CanvasGroup overDescription;

	private global::UnityEngine.UI.Text overDescriptionText;

	private string backButtonLoc = "menu_back_main_menu";

	private string quitButtonLoc = "menu_quit_mission";

	private string quitAltButtonLoc = "menu_voluntary_rout";

	private string quitButtonOverDescLoc = "menu_abandon_preview_desc";

	private string quitAltButtonOverDescLoc = "menu_voluntary_rout_preview_desc";

	private string quitButtonOverDisabledDescLoc = "menu_no_abandon_desc";

	private string quitAltButtonOverDisabledDescLoc = "menu_no_voluntary_rout_desc";

	private string saveAndQuitDisabledDescLoc = "menu_no_save_and_quit_desc";

	private bool panelOpen;

	[global::UnityEngine.Header("Panel toggles")]
	public global::ToggleEffects audioOptions;

	public global::ToggleEffects controlsOptions;

	public global::ToggleEffects gameplayOptions;

	public global::ToggleEffects graphics;

	public global::ToggleEffects mappings;

	public global::ToggleEffects butSaveAndQuit;

	public global::ToggleEffects butQuit;

	public global::ToggleEffects butQuitAlt;

	public global::ToggleEffects butHelp;

	public global::ToggleEffects butOpponentProfile;

	[global::UnityEngine.Header("Audio")]
	public global::UnityEngine.UI.ToggleGroup audioPanel;

	public global::SliderGroup masterVolume;

	public global::SliderGroup effectsVolume;

	public global::SliderGroup musicVolume;

	public global::SliderGroup voiceVolume;

	public global::SliderGroup ambientVolume;

	private global::UnityEngine.AudioSource audioSrc;

	private global::UnityEngine.AudioClip ambientVolumeSample;

	[global::UnityEngine.Header("Graphics")]
	public global::UnityEngine.UI.ToggleGroup graphicsPanel;

	public global::UnityEngine.UI.Toggle fullscreen;

	public global::SelectorGroup resolutions;

	public global::UnityEngine.UI.Toggle vsync;

	public global::SelectorGroup textureQuality;

	public global::SelectorGroup shadowsQuality;

	public global::SelectorGroup shadowCascades;

	public global::UnityEngine.UI.Toggle dof;

	public global::UnityEngine.UI.Toggle bloom;

	public global::UnityEngine.UI.Toggle ssao;

	public global::SelectorGroup smaa;

	public global::SliderGroup brightness;

	public global::SliderGroup guiScale;

	[global::UnityEngine.Header("Controls")]
	public global::UnityEngine.UI.ToggleGroup controlsPanel;

	public global::UnityEngine.UI.Toggle gamepadEnabled;

	public global::UnityEngine.UI.Toggle invertCameraHorizontalEnabled;

	public global::UnityEngine.UI.Toggle invertCameraVerticalEnabled;

	public global::UnityEngine.UI.Toggle leftHandedMouseEnabled;

	public global::SliderGroup mouseSensitivitySlider;

	public global::UnityEngine.UI.Toggle leftHandedControllerEnabled;

	public global::SliderGroup joystickSensitivitySlider;

	[global::UnityEngine.Header("Gameplay")]
	public global::UnityEngine.UI.ToggleGroup gameplayPanel;

	public global::SelectorGroup languageSelect;

	public global::UnityEngine.UI.Toggle tacticalViewHelpersEnabled;

	public global::UnityEngine.UI.Toggle wagonBeaconsEnabled;

	public global::UnityEngine.UI.Toggle autoExitTacticalEnabled;

	public global::UnityEngine.UI.Toggle displayFullUI;

	public global::UnityEngine.UI.Toggle fastForward;

	public global::UnityEngine.UI.Toggle skipTuto;

	[global::UnityEngine.Header("Mapping")]
	public global::UnityEngine.UI.ToggleGroup mappingPanel;

	public global::UnityEngine.GameObject controlEntry;

	public global::ScrollGroup controlsList;

	public global::RemapButtonPopupView controlsRemapPopup;

	private bool mappingsUnselected;

	private bool mappingChanged;

	private bool needRevertOptionsConfirm;

	private bool countdownToRestore;

	private float restoreTime;

	private int lastTimeDisplayed;

	private global::UIControlMappingItem remappedEntry;

	private int remappedButtonIndex;

	private global::Rewired.ActionElementMap remappedAction;

	private global::UnityEngine.GameObject lastSelection;

	private global::Rewired.ActionElementMap conflictingAction;

	private global::Rewired.Pole newMapKeyPole;

	private global::Rewired.ControllerPollingInfo newMapInput;

	private global::System.Collections.Generic.List<global::SupportedLanguage> availableLangs = new global::System.Collections.Generic.List<global::SupportedLanguage>();

	private bool initialized;
}
