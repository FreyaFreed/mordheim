using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TutorialsMenuState : global::UIStateMonoBehaviour<global::MainMenuController>
{
	public override void StateEnter()
	{
		if (!this.isInit)
		{
			this.isInit = true;
			base.GetComponentsInChildren<global::GameModeTutorialView>(true, this.tutorialsView);
			foreach (global::GameModeTutorialView gameModeTutorialView in this.tutorialsView)
			{
				gameModeTutorialView.Load();
				gameModeTutorialView.button.onAction.AddListener(new global::UnityEngine.Events.UnityAction(this.CheckTutoStart));
			}
			this.imageModule.Setup();
			this.hideoutTutoPanel.GetComponentsInChildren<global::ToggleEffects>(true, this.hideoutTutorials);
			for (int i = 0; i < this.hideoutTutorials.Count; i++)
			{
				int index = i;
				this.hideoutTutorials[i].onAction.AddListener(delegate()
				{
					this.ShowTutoImage(index);
				});
			}
			this.largeTutoImagePanel.gameObject.SetActive(false);
		}
		this.RefreshTutorialDoneToggle();
		this.darkSideBar.SetActive(false);
		base.StateMachine.camManager.dummyCam.transform.position = this.camPos.transform.position;
		base.StateMachine.camManager.dummyCam.transform.rotation = this.camPos.transform.rotation;
		base.StateMachine.camManager.Transition(2f, true);
		base.Show(true);
		this.tutorialsView[0].GetComponentsInChildren<global::ToggleEffects>(true)[0].toggle.isOn = true;
		this.cancelButton.SetAction("cancel", "menu_back", 0, false, this.icnBack, null);
		this.cancelButton.OnAction(new global::UnityEngine.Events.UnityAction(this.OnInputCancel), true, true);
		this.actionButton.SetAction("action", "menu_confirm", 0, false, null, null);
		this.actionButton.OnAction(null, false, true);
		this.OnInputTypeChanged();
		global::PandoraSingleton<global::NoticeManager>.Instance.RegisterListener(global::Notices.INPUT_TYPE_CHANGED, new global::DelReceiveNotice(this.OnInputTypeChanged));
	}

	private void RefreshTutorialDoneToggle()
	{
		for (int i = 0; i < this.tutorialsCompletionToggle.Count; i++)
		{
			if (this.tutorialsCompletionToggle[i] != null)
			{
				if (i < global::PandoraSingleton<global::GameManager>.Instance.Profile.TutorialCompletion.Length - 1)
				{
					this.tutorialsCompletionToggle[i].isOn = global::PandoraSingleton<global::GameManager>.Instance.Profile.TutorialCompletion[i];
				}
				else
				{
					this.tutorialsCompletionToggle[i].isOn = false;
				}
			}
		}
	}

	private void ShowTutoImage(int index)
	{
		if (!this.showingImage)
		{
			this.showingImage = true;
			this.actionButton.gameObject.SetActive(false);
			this.lastSelectedHideoutTuto = index;
			this.imageModule.Set(index, new global::System.Action(this.OnInputCancel));
			this.largeTutoImagePanel.gameObject.SetActive(true);
		}
	}

	private void HideTutoImage()
	{
		if (this.showingImage)
		{
			this.showingImage = false;
			this.actionButton.gameObject.SetActive(true);
			this.RefreshTutorialDoneToggle();
			this.largeTutoImagePanel.gameObject.SetActive(false);
			this.hideoutTutorials[this.lastSelectedHideoutTuto].SetSelected(true);
		}
	}

	public override void OnInputCancel()
	{
		if (!this.showingImage)
		{
			this.OnQuit();
		}
		else
		{
			this.HideTutoImage();
		}
	}

	public void OnQuit()
	{
		base.StateMachine.ChangeState(global::MainMenuController.State.MAIN_MENU);
	}

	public override void StateExit()
	{
		this.darkSideBar.SetActive(true);
		this.cancelButton.gameObject.SetActive(false);
		this.actionButton.gameObject.SetActive(false);
		base.Show(false);
		global::PandoraSingleton<global::NoticeManager>.Instance.RemoveListener(global::Notices.INPUT_TYPE_CHANGED, new global::DelReceiveNotice(this.OnInputTypeChanged));
	}

	private void CheckTutoStart()
	{
		global::GameModeTutorialView component = global::UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<global::GameModeTutorialView>();
		this.currentTutoIdx = component.tutorialIdx + 1;
		base.StateMachine.ConfirmPopup.Show(string.Format("tutorial_0{0}_title", this.currentTutoIdx), string.Format("tutorial_0{0}_menu", this.currentTutoIdx), new global::System.Action<bool>(this.StartTutorial), false, false);
	}

	private void StartTutorial(bool confirmed)
	{
		if (!confirmed)
		{
			this.currentTutoIdx = -1;
			return;
		}
		if (!global::PandoraSingleton<global::MissionStartData>.Exists())
		{
			global::UnityEngine.GameObject gameObject = new global::UnityEngine.GameObject("mission_start_data");
			gameObject.AddComponent<global::MissionStartData>();
		}
		global::PandoraSingleton<global::MissionStartData>.Instance.ResetSeed();
		this.canvasGroup.interactable = false;
		global::UnityEngine.EventSystems.EventSystem.current.currentInputModule.DeactivateModule();
		global::PandoraSingleton<global::PandoraInput>.Instance.SetActive(false);
		base.StartCoroutine(global::PandoraSingleton<global::MissionStartData>.Instance.SetMissionFull(global::Mission.GenerateCampaignMission(global::WarbandId.NONE, this.currentTutoIdx), null, delegate
		{
			this.currentTutoIdx = -1;
			global::SceneLauncher.Instance.LaunchScene(global::SceneLoadingId.LAUNCH_TUTO, false, false);
		}));
	}

	public override int StateId
	{
		get
		{
			return 1;
		}
	}

	private void OnInputTypeChanged()
	{
		this.actionButton.gameObject.SetActive(global::PandoraSingleton<global::PandoraInput>.Instance.lastInputMode == global::PandoraInput.InputMode.JOYSTICK);
	}

	private const string TUTO_TITLE = "tutorial_0{0}_title";

	private const string TUTO_DESC = "tutorial_0{0}_menu";

	private bool isInit;

	public global::System.Collections.Generic.List<global::UnityEngine.UI.Toggle> tutorialsCompletionToggle = new global::System.Collections.Generic.List<global::UnityEngine.UI.Toggle>();

	private global::System.Collections.Generic.List<global::GameModeTutorialView> tutorialsView = new global::System.Collections.Generic.List<global::GameModeTutorialView>();

	private global::System.Collections.Generic.List<global::ToggleEffects> hideoutTutorials = new global::System.Collections.Generic.List<global::ToggleEffects>();

	public global::ButtonGroup actionButton;

	public global::ButtonGroup cancelButton;

	public global::UnityEngine.Sprite icnBack;

	public global::UnityEngine.GameObject camPos;

	public global::UnityEngine.GameObject darkSideBar;

	public global::UnityEngine.GameObject hideoutTutoPanel;

	public global::UnityEngine.GameObject largeTutoImagePanel;

	private bool showingImage;

	public global::NextTutoImageModule imageModule;

	private int lastSelectedHideoutTuto;

	private int currentTutoIdx = -1;
}
