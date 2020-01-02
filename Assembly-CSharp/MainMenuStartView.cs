using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MainMenuStartView : global::UIStateMonoBehaviour<global::MainMenuController>
{
	public override void Awake()
	{
		base.Awake();
		this.btnContinue.onAction.AddListener(new global::UnityEngine.Events.UnityAction(this.OnContinueCampaign));
		this.btnLoadGame.onAction.AddListener(delegate()
		{
			base.StateMachine.ChangeState(global::MainMenuController.State.LOAD_CAMPAIGN);
		});
		this.btnNewGame.onAction.AddListener(new global::UnityEngine.Events.UnityAction(this.OnCreateCampaign));
		this.btnTutorials.onAction.AddListener(delegate()
		{
			base.StateMachine.ChangeState(global::MainMenuController.State.TUTORIALS);
		});
		this.btnOptions.onAction.AddListener(delegate()
		{
			base.StateMachine.ChangeState(global::MainMenuController.State.OPTIONS);
		});
		this.btnCredits.onAction.AddListener(delegate()
		{
			base.StateMachine.ChangeState(global::MainMenuController.State.CREDITS);
		});
		this.btnExit.onAction.AddListener(new global::UnityEngine.Events.UnityAction(this.OnInputCancel));
		this.versionLabel.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(this.versionLabel.text, new string[]
		{
			"1.4.4.4"
		});
		this.welcomeDesc.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("menu_welcome_desc");
	}

	public override void StateEnter()
	{
		base.Show(true);
		base.StateMachine.camManager.dummyCam.transform.position = this.camPos.transform.position;
		base.StateMachine.camManager.dummyCam.transform.rotation = this.camPos.transform.rotation;
		base.StateMachine.camManager.Transition(2f, true);
		this.once = true;
		this.btnContinue.gameObject.SetActive(false);
		this.btnLoadGame.gameObject.SetActive(false);
		this.btnNewGame.SetSelected(false);
		base.StartCoroutine(this.CheckContinue());
		this.playerInfo.SetActive(false);
	}

	private global::System.Collections.IEnumerator CheckContinue()
	{
		while (global::PandoraSingleton<global::GameManager>.Instance.Profile == null || !global::PandoraSingleton<global::GameManager>.Instance.profileInitialized)
		{
			yield return null;
		}
		bool showContinue = global::PandoraSingleton<global::GameManager>.Instance.Profile.LastPlayedCampaign != -1 && global::PandoraSingleton<global::GameManager>.Instance.Save.CampaignExist(global::PandoraSingleton<global::GameManager>.Instance.Profile.LastPlayedCampaign);
		bool showLoad = global::PandoraSingleton<global::GameManager>.Instance.Save.HasCampaigns();
		this.btnContinue.gameObject.SetActive(showContinue);
		this.btnLoadGame.gameObject.SetActive(showLoad);
		while (global::PandoraSingleton<global::GameManager>.Instance.Popup.isShow)
		{
			yield return null;
		}
		if (showContinue)
		{
			this.btnContinue.SetSelected(true);
			this.btnNewGame.toggle.isOn = false;
		}
		else if (showLoad)
		{
			this.btnLoadGame.SetSelected(true);
			this.btnNewGame.toggle.isOn = false;
		}
		yield break;
	}

	private void OnPlayerPictureLoaded(global::UnityEngine.Sprite sprite)
	{
		this.playerPic.sprite = sprite;
	}

	private void Setup()
	{
		this.butCommunity.SetAction("hub", "info_news", 0, false, null, null);
		this.butCommunity.OnAction(new global::UnityEngine.Events.UnityAction(this.OpenCommunity), false, true);
		this.butConfirm.gameObject.SetActive(false);
		this.butExit.gameObject.SetActive(false);
	}

	private void OpenCommunity()
	{
		global::PandoraSingleton<global::Hephaestus>.Instance.OpenCommunity();
	}

	public override void StateUpdate()
	{
		base.StateUpdate();
		if (this.once)
		{
			this.Setup();
			this.once = false;
		}
	}

	public override void OnInputCancel()
	{
		base.StateMachine.ConfirmPopup.Show("menu_warning", "menu_exit_game_confirm", new global::System.Action<bool>(this.OnPopup), false, false);
	}

	private void OnPopup(bool confirm)
	{
		if (confirm)
		{
			global::UnityEngine.Application.Quit();
		}
	}

	private void OnCreateCampaign()
	{
		if (!global::PandoraSingleton<global::GameManager>.Instance.Save.HasCampaigns() && !global::PandoraSingleton<global::GameManager>.Instance.Profile.HasCompletedTutorials())
		{
			base.StateMachine.ConfirmPopup.Show("menu_new_campaign_tuto_title", "menu_new_campaign_tuto_desc", new global::System.Action<bool>(this.OnCreateNoTuto), false, false);
		}
		else if (global::PandoraSingleton<global::GameManager>.Instance.Save.EmptyCampaignSaveExists())
		{
			base.StateMachine.ChangeState(global::MainMenuController.State.NEW_CAMPAIGN);
		}
		else
		{
			base.StateMachine.ConfirmPopup.Show("menu_warning", "menu_out_of_campaign_slots", null, false, false);
			base.StateMachine.ConfirmPopup.HideCancelButton();
		}
	}

	private void OnCreateNoTuto(bool confirm)
	{
		if (confirm)
		{
			base.StateMachine.ChangeState(global::MainMenuController.State.NEW_CAMPAIGN);
		}
	}

	private void OnContinueCampaign()
	{
		if (!global::PandoraSingleton<global::TransitionManager>.Instance.IsDone())
		{
			return;
		}
		int lastPlayedCampaign = global::PandoraSingleton<global::GameManager>.Instance.Profile.LastPlayedCampaign;
		if (lastPlayedCampaign != -1 && global::PandoraSingleton<global::GameManager>.Instance.Save.CampaignExist(lastPlayedCampaign))
		{
			global::PandoraSingleton<global::GameManager>.Instance.campaign = lastPlayedCampaign;
			global::PandoraSingleton<global::GameManager>.Instance.Save.LoadCampaign(lastPlayedCampaign);
		}
		else
		{
			base.StateMachine.ChangeState(global::MainMenuController.State.NEW_CAMPAIGN);
		}
	}

	public override void StateExit()
	{
		this.butConfirm.gameObject.SetActive(false);
		base.Show(false);
		base.StopCoroutine(this.CheckContinue());
	}

	public override int StateId
	{
		get
		{
			return 0;
		}
	}

	private const string welcomeStringId = "menu_welcome_desc";

	public global::ToggleEffects btnContinue;

	public global::ToggleEffects btnLoadGame;

	public global::ToggleEffects btnNewGame;

	public global::ToggleEffects btnTutorials;

	public global::ToggleEffects btnOptions;

	public global::ToggleEffects btnCredits;

	public global::ToggleEffects btnExit;

	public global::UnityEngine.UI.Text welcomeDesc;

	public global::UnityEngine.UI.Text versionLabel;

	public global::UnityEngine.GameObject grpCommunity;

	public global::ButtonGroup butCommunity;

	public global::ButtonGroup butConfirm;

	public global::ButtonGroup butExit;

	public global::UnityEngine.GameObject camPos;

	public global::UnityEngine.GameObject playerInfo;

	public global::UnityEngine.UI.Image playerPic;

	public global::UnityEngine.UI.Text playerName;

	public global::ButtonGroup butSwitchAccount;

	private bool once = true;
}
