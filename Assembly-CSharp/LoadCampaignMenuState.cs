using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LoadCampaignMenuState : global::UIStateMonoBehaviour<global::MainMenuController>
{
	public override void Awake()
	{
		base.Awake();
		this.loadedBanners = new global::System.Collections.Generic.Dictionary<global::WarbandId, global::UnityEngine.GameObject>();
		if (this.deleteButton != null)
		{
			this.deleteButton.gameObject.SetActive(false);
		}
	}

	public override void StateEnter()
	{
		base.Show(true);
		base.StateMachine.camManager.dummyCam.transform.position = this.camPos.transform.position;
		base.StateMachine.camManager.dummyCam.transform.rotation = this.camPos.transform.rotation;
		base.StateMachine.camManager.Transition(2f, true);
		this.cancelButton.SetAction("cancel", "menu_back", 0, false, this.icnBack, null);
		this.cancelButton.OnAction(new global::UnityEngine.Events.UnityAction(this.OnQuit), false, true);
		this.actionButton.SetAction("action", "menu_confirm", 0, false, null, null);
		this.deleteButton.SetAction("delete_campaign", "main_delete_campaign", 0, false, null, null);
		this.deleteButton.OnAction(new global::UnityEngine.Events.UnityAction(this.DeleteCampaign), false, true);
		this.OnInputTypeChanged();
		this.FillCampaignsList();
		this.title.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById((!global::PandoraSingleton<global::Hephaestus>.Instance.IsJoiningInvite()) ? "menu_load_game" : "invite_select_warband_title");
		global::PandoraSingleton<global::NoticeManager>.Instance.RegisterListener(global::Notices.INPUT_TYPE_CHANGED, new global::DelReceiveNotice(this.OnInputTypeChanged));
	}

	public void FillCampaignsList()
	{
		this.allSaveLoaded = false;
		this.campaignsList.ClearList();
		this.campaignsList.Setup(this.campaignEntry, false);
		global::System.Collections.Generic.List<int> campaignSlots = global::PandoraSingleton<global::GameManager>.Instance.Save.GetCampaignSlots();
		this.actionButton.gameObject.SetActive(campaignSlots.Count > 0);
		if (global::PandoraSingleton<global::Hephaestus>.Instance.IsJoiningInvite() || global::PandoraSingleton<global::Hephaestus>.Instance.IsPlayTogether())
		{
			this.deleteButton.gameObject.SetActive(false);
		}
		else
		{
			this.deleteButton.gameObject.SetActive(campaignSlots.Count > 0);
		}
		this.campaignCount.text = campaignSlots.Count + "/" + global::PandoraSingleton<global::GameManager>.Instance.Save.MaxSaveGames;
		base.StartCoroutine(this.StartLoadCampaigns(campaignSlots));
	}

	private global::System.Collections.IEnumerator StartLoadCampaigns(global::System.Collections.Generic.List<int> slots)
	{
		int nbValidWarband = 0;
		for (int i = 0; i < slots.Count; i++)
		{
			global::CampaignFlagView newEntry = this.campaignsList.AddToList(null, null).GetComponent<global::CampaignFlagView>();
			if (newEntry.campaignOver.activeSelf)
			{
				nbValidWarband++;
			}
			newEntry.Load(slots[i], new global::UnityEngine.Events.UnityAction<int, int>(this.OnSelectCampaign), new global::UnityEngine.Events.UnityAction<int, global::WarbandSave>(this.OnConfirmCampaign), i);
			while (!newEntry.loaded)
			{
				yield return null;
			}
		}
		if (nbValidWarband == 0)
		{
			if (global::PandoraSingleton<global::Hephaestus>.Instance.IsJoiningInvite())
			{
				global::PandoraSingleton<global::Hephaestus>.Instance.ResetInvite();
				global::PandoraSingleton<global::GameManager>.Instance.ShowSystemPopup(global::GameManager.SystemPopupId.INVITE, "invite_no_valid_warband_title", "invite_no_valid_warband_desc", null, false);
			}
			else if (global::PandoraSingleton<global::Hephaestus>.Instance.IsPlayTogether())
			{
				global::PandoraSingleton<global::Hephaestus>.Instance.ResetPlayTogether(true);
				global::PandoraSingleton<global::GameManager>.Instance.ShowSystemPopup(global::GameManager.SystemPopupId.INVITE, "play_together_no_valid_warband_title", "play_together_no_valid_warband_desc", null, false);
			}
		}
		else if (global::PandoraSingleton<global::Hephaestus>.Instance.IsJoiningInvite())
		{
			global::PandoraSingleton<global::GameManager>.Instance.ShowSystemPopup(global::GameManager.SystemPopupId.INVITE, "invite_hideout_quit_title", (!global::PandoraSingleton<global::Hephaestus>.Instance.GetJoiningLobby().isExhibition) ? "invite_message_contest" : "invite_message_exhibition", new global::System.Action<bool>(this.OnPopupClose), false);
		}
		yield return null;
		yield return null;
		this.allSaveLoaded = true;
		yield break;
	}

	private void OnPopupClose(bool obj)
	{
		global::ToggleEffects component = this.campaignsList.items[0].GetComponent<global::ToggleEffects>();
		component.SetSelected(true);
		base.StartCoroutine(this.SelectCampaignEntry());
	}

	private global::System.Collections.IEnumerator SelectCampaignEntry()
	{
		yield return null;
		yield return null;
		yield return null;
		global::ToggleEffects effects = this.campaignsList.items[0].GetComponent<global::ToggleEffects>();
		effects.SetSelected(true);
		yield break;
	}

	public void OnSelectCampaign(int campaignIdx, int warbandId)
	{
		global::PandoraSingleton<global::GameManager>.Instance.campaign = campaignIdx;
		if (warbandId == 0)
		{
			return;
		}
		if (!this.loadedBanners.ContainsKey((global::WarbandId)warbandId))
		{
			global::WarbandData warbandData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::WarbandData>(warbandId);
			string text = warbandData.Wagon;
			text = text.Substring(5);
			text = "banner" + text;
			global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadAssetAsync<global::UnityEngine.GameObject>("Assets/prefabs/environments/props/banners/", global::AssetBundleId.PROPS, text + ".prefab", delegate(global::UnityEngine.Object banPrefab)
			{
				this.loadedBanners[(global::WarbandId)warbandId] = (global::UnityEngine.GameObject)global::UnityEngine.Object.Instantiate(banPrefab);
				global::Dissolver dissolver = this.loadedBanners[(global::WarbandId)warbandId].AddComponent<global::Dissolver>();
				dissolver.Hide(true, true, null);
			});
		}
		this.flag.gameObject.SetActive(true);
		global::UnityEngine.GameObject content = this.flag.GetContent();
		if (content != null)
		{
			global::WarbandId wbId = (global::WarbandId)warbandId;
			content.GetComponent<global::Dissolver>().Hide(true, false, delegate
			{
				this.SetupBanner(wbId);
			});
		}
		else
		{
			this.SetupBanner((global::WarbandId)warbandId);
		}
	}

	private void SetupBanner(global::WarbandId wbId)
	{
		if (this.loadedBanners.ContainsKey(wbId))
		{
			global::UnityEngine.GameObject gameObject = this.loadedBanners[wbId];
			gameObject.SetActive(true);
			global::UnityEngine.Cloth cloth = gameObject.GetComponentsInChildren<global::UnityEngine.Cloth>(true)[0];
			cloth.enabled = false;
			this.flag.SetContent(gameObject);
			cloth.enabled = true;
			gameObject.GetComponent<global::Dissolver>().Hide(true, true, null);
			gameObject.GetComponent<global::Dissolver>().Hide(false, false, null);
		}
	}

	public void OnConfirmCampaign(int campaignIdx, global::WarbandSave warbandSave)
	{
		if (!this.allSaveLoaded || global::PandoraSingleton<global::PandoraInput>.Instance.CurrentInputLayer == 1 || global::PandoraSingleton<global::TransitionManager>.Instance.IsLoading())
		{
			return;
		}
		global::PandoraDebug.LogInfo("Confirmed Campaign", "uncategorised", null);
		global::PandoraSingleton<global::GameManager>.Instance.campaign = campaignIdx;
		global::PandoraSingleton<global::GameManager>.Instance.currentSave = warbandSave;
		if (global::PandoraSingleton<global::GameManager>.Instance.Save.CampaignExist(global::PandoraSingleton<global::GameManager>.Instance.campaign))
		{
			global::PandoraSingleton<global::GameManager>.Instance.Save.LoadCampaign(global::PandoraSingleton<global::GameManager>.Instance.campaign);
		}
	}

	public void DeleteCampaign()
	{
		if (!this.allSaveLoaded || global::PandoraSingleton<global::PandoraInput>.Instance.CurrentInputLayer == 1)
		{
			return;
		}
		if (global::PandoraSingleton<global::GameManager>.Instance.Save.CampaignExist(global::PandoraSingleton<global::GameManager>.Instance.campaign))
		{
			global::System.Collections.Generic.List<int> campaignSlots = global::PandoraSingleton<global::GameManager>.Instance.Save.GetCampaignSlots();
			for (int i = 0; i < campaignSlots.Count; i++)
			{
				if (campaignSlots[i] == global::PandoraSingleton<global::GameManager>.Instance.campaign)
				{
					base.StateMachine.ConfirmPopup.ShowLocalized(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("main_delete_campaign"), global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("main_delete_campaign_confirm", new string[]
					{
						this.campaignsList.items[i].GetComponent<global::CampaignFlagView>().textTitle.text
					}), new global::System.Action<bool>(this.OnDeletePopup), false, false);
					break;
				}
			}
		}
	}

	private void OnDeletePopup(bool isConfirm)
	{
		if (isConfirm)
		{
			global::PandoraSingleton<global::NoticeManager>.Instance.RegisterListener(global::Notices.GAME_DELETED, new global::DelReceiveNotice(this.OnDeleteSave));
			global::PandoraSingleton<global::GameManager>.Instance.Save.DeleteCampaign(global::PandoraSingleton<global::GameManager>.Instance.campaign);
			global::PandoraSingleton<global::GameManager>.Instance.campaign = -1;
			this.FillCampaignsList();
			if (this.flag.IsOccupied())
			{
				this.flag.GetContent().GetComponent<global::Dissolver>().Hide(true, false, null);
			}
		}
	}

	private void OnDeleteSave()
	{
		global::PandoraSingleton<global::NoticeManager>.Instance.RemoveListener(global::Notices.GAME_DELETED, new global::DelReceiveNotice(this.OnDeleteSave));
	}

	public override void OnInputCancel()
	{
		this.OnQuit();
	}

	public void OnQuit()
	{
		if (global::PandoraSingleton<global::Hephaestus>.Instance.IsJoiningInvite())
		{
			base.StateMachine.ConfirmPopup.Show("invite_select_warband_quit_title", "invite_select_warband_quit_desc", new global::System.Action<bool>(this.OnQuitConfirm), false, false);
		}
		else if (global::PandoraSingleton<global::Hephaestus>.Instance.IsPlayTogether())
		{
			base.StateMachine.ConfirmPopup.Show("play_together_select_warband_quit_title", "play_together_select_warband_quit_desc", new global::System.Action<bool>(this.OnQuitConfirm), false, false);
		}
		else
		{
			base.StateMachine.ChangeState(global::MainMenuController.State.MAIN_MENU);
		}
	}

	private void OnQuitConfirm(bool confirm)
	{
		if (confirm)
		{
			global::PandoraSingleton<global::Hephaestus>.Instance.ResetInvite();
			global::PandoraSingleton<global::Hephaestus>.Instance.ResetPlayTogether(true);
			base.StateMachine.ChangeState(global::MainMenuController.State.MAIN_MENU);
		}
	}

	public override void StateExit()
	{
		this.cancelButton.gameObject.SetActive(false);
		this.actionButton.gameObject.SetActive(false);
		this.deleteButton.gameObject.SetActive(false);
		base.Show(false);
		if (this.flag.IsOccupied())
		{
			global::Dissolver component = this.flag.GetContent().GetComponent<global::Dissolver>();
			component.Hide(true, false, null);
		}
		global::PandoraSingleton<global::NoticeManager>.Instance.RemoveListener(global::Notices.INPUT_TYPE_CHANGED, new global::DelReceiveNotice(this.OnInputTypeChanged));
	}

	public override int StateId
	{
		get
		{
			return 6;
		}
	}

	private void OnInputTypeChanged()
	{
		this.actionButton.gameObject.SetActive(global::PandoraSingleton<global::PandoraInput>.Instance.lastInputMode == global::PandoraInput.InputMode.JOYSTICK);
	}

	public global::UnityEngine.UI.Text title;

	public global::UnityEngine.GameObject campaignEntry;

	public global::ScrollGroup campaignsList;

	public global::UnityEngine.UI.Text campaignCount;

	public global::MenuNode flag;

	public global::ButtonGroup actionButton;

	public global::ButtonGroup cancelButton;

	public global::ButtonGroup deleteButton;

	public global::UnityEngine.Sprite icnBack;

	private global::System.Collections.Generic.Dictionary<global::WarbandId, global::UnityEngine.GameObject> loadedBanners;

	public global::UnityEngine.GameObject camPos;

	private bool allSaveLoaded;

	private bool isInviteDuringLoadingCampaign;
}
