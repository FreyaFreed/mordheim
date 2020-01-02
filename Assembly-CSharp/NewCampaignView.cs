using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class NewCampaignView : global::UIStateMonoBehaviour<global::MainMenuController>
{
	public override int StateId
	{
		get
		{
			return 5;
		}
	}

	protected override void Start()
	{
		if (this.initialized)
		{
			return;
		}
		this.initialized = true;
		base.Start();
		this.leaderUnits = new global::System.Collections.Generic.List<global::MainMenuUnit>();
		for (int i = 0; i < this.characterNodes.nodes.Count; i++)
		{
			this.leaderUnits.Add(this.characterNodes.nodes[i].content.GetComponent<global::MainMenuUnit>());
			this.leaderUnits[i].Hide(true);
		}
		for (int j = 0; j < this.flagNodes.nodes.Count; j++)
		{
			this.flagNodes.nodes[j].content.GetComponent<global::Dissolver>().Hide(true, true, null);
		}
		this.characterNodes.Deactivate();
		this.flagNodes.Deactivate();
		this.unitMenuNodeIndex = -1;
	}

	public override void StateEnter()
	{
		if (!this.initialized)
		{
			this.Start();
		}
		base.StateMachine.camManager.dummyCam.transform.position = this.camPos.transform.position;
		base.StateMachine.camManager.dummyCam.transform.rotation = this.camPos.transform.rotation;
		base.StateMachine.camManager.Transition(2f, true);
		this.darkSideBar.SetActive(false);
		this.needActivateNode = true;
		for (int i = 0; i < this.leaderUnits.Count; i++)
		{
			this.leaderUnits[i].Hide(false);
			if (this.flagNodes.nodes[i].GetContent())
			{
				this.flagNodes.nodes[i].GetContent().GetComponent<global::Dissolver>().Hide(true, true, null);
			}
		}
		base.Show(true);
		this.butExit.SetAction("cancel", "menu_back", 0, false, this.backIcon, null);
		this.butExit.OnAction(new global::UnityEngine.Events.UnityAction(this.OnInputCancel), false, true);
		this.butConfirm.SetAction("action", "menu_confirm", 0, false, null, null);
		this.OnInputTypeChanged();
		global::PandoraSingleton<global::NoticeManager>.Instance.RegisterListener(global::Notices.INPUT_TYPE_CHANGED, new global::DelReceiveNotice(this.OnInputTypeChanged));
	}

	public override void StateUpdate()
	{
		base.StateUpdate();
		if (this.needActivateNode)
		{
			this.characterNodes.Activate(new global::MenuNodeDelegateNode(this.NodeSelected), new global::MenuNodeDelegateNode(this.NodeUnSeleteced), new global::MenuNodeDelegateNode(this.NodeConfirmed), global::PandoraInput.InputLayer.NORMAL, false);
			this.characterNodes.SelectNode(this.characterNodes.nodes[0]);
			this.needActivateNode = false;
		}
	}

	public override void StateExit()
	{
		this.darkSideBar.SetActive(true);
		for (int i = 0; i < this.leaderUnits.Count; i++)
		{
			this.leaderUnits[i].Hide(true);
			if (this.flagNodes.nodes[i].GetContent() != null)
			{
				this.flagNodes.nodes[i].GetContent().GetComponent<global::Dissolver>().Hide(true, false, null);
			}
		}
		this.characterNodes.Deactivate();
		this.flagNodes.Deactivate();
		base.Show(false);
		global::PandoraSingleton<global::NoticeManager>.Instance.RemoveListener(global::Notices.INPUT_TYPE_CHANGED, new global::DelReceiveNotice(this.OnInputTypeChanged));
	}

	public void SetDescription(global::WarbandId warbandId)
	{
		this.raceTitle.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("com_wb_type_" + warbandId.ToString().ToLowerInvariant());
		this.raceDescription.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("com_wb_desc_" + warbandId.ToString().ToLowerInvariant());
	}

	private void NodeSelected(global::MenuNode node, int idx)
	{
		this.unitMenuNodeIndex = idx;
		if (node.GetContent() != null)
		{
			global::MainMenuUnit mainMenuUnit = this.leaderUnits[idx];
			this.SetDescription(mainMenuUnit.warbandId);
			mainMenuUnit.LaunchAction(global::UnitActionId.MISC, true, global::UnitStateId.NONE, 1);
		}
		for (int i = 0; i < this.flagNodes.nodes.Count; i++)
		{
			if (this.flagNodes.nodes[i].GetContent())
			{
				this.flagNodes.nodes[i].GetContent().GetComponent<global::Dissolver>().Hide(i != idx, false, null);
			}
		}
	}

	private void NodeUnSeleteced(global::MenuNode node, int idx)
	{
	}

	private void NodeConfirmed(global::MenuNode node, int idx)
	{
		global::MainMenuUnit mainMenuUnit = this.leaderUnits[this.unitMenuNodeIndex];
		global::WarbandId warbandId = mainMenuUnit.warbandId;
		if ((warbandId == global::WarbandId.WITCH_HUNTERS && !global::PandoraSingleton<global::Hephaestus>.Instance.OwnsDLC(global::Hephaestus.DlcId.WITCH_HUNTERS)) || (warbandId == global::WarbandId.UNDEAD && !global::PandoraSingleton<global::Hephaestus>.Instance.OwnsDLC(global::Hephaestus.DlcId.UNDEAD)))
		{
			this.ShowDlcPopup(warbandId);
		}
		else
		{
			this.CheckWarbandLevel(warbandId);
		}
	}

	private void OnCheckNetwork(bool result, string reason, global::WarbandId wId)
	{
		if (result)
		{
			global::NewCampaignView.OpenStore(wId);
		}
		else
		{
			global::PandoraSingleton<global::GameManager>.Instance.ShowSystemPopup(global::GameManager.SystemPopupId.CONNECTION_VALIDATION, "console_offline_error_title", global::PandoraSingleton<global::Hephaestus>.Instance.GetOfflineReason(), null, null, false);
		}
	}

	private void ShowDlcPopup(global::WarbandId wId)
	{
		base.StateMachine.ConfirmPopup.ShowLocalized(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("com_wb_title_dlc"), global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("com_wb_desc_dlc", new string[]
		{
			global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("com_wb_type_" + wId.ToString().ToLowerInvariant())
		}), delegate(bool confirm)
		{
			this.OnDLCBuyPopup(confirm, wId);
		}, false, false);
	}

	public void OnDLCBuyPopup(bool confirm, global::WarbandId wId)
	{
		if (confirm)
		{
			global::NewCampaignView.OpenStore(wId);
		}
	}

	private static void OpenStore(global::WarbandId wId)
	{
		if (wId != global::WarbandId.WITCH_HUNTERS)
		{
			if (wId == global::WarbandId.UNDEAD)
			{
				global::PandoraSingleton<global::Hephaestus>.Instance.OpenStore(global::Hephaestus.DlcId.UNDEAD);
			}
		}
		else
		{
			global::PandoraSingleton<global::Hephaestus>.Instance.OpenStore(global::Hephaestus.DlcId.WITCH_HUNTERS);
		}
	}

	public override void OnInputCancel()
	{
		base.StateMachine.GoToPrev();
	}

	private void CheckWarbandLevel(global::WarbandId id)
	{
		if (global::PandoraSingleton<global::GameManager>.Instance.Profile.Rank < 5)
		{
			this.CreateNewCampaign(id, 0);
		}
		else
		{
			this.warbandRankPopup.Show(delegate(bool confirm, int rank)
			{
				if (confirm)
				{
					this.CreateNewCampaign(id, rank);
				}
			}, false);
		}
	}

	private void CreateNewCampaign(global::WarbandId id, int rank = 0)
	{
		global::PandoraSingleton<global::GameManager>.Instance.campaign = global::PandoraSingleton<global::GameManager>.Instance.Save.GetEmptyCampaignSlot();
		global::PandoraSingleton<global::NoticeManager>.Instance.RegisterListener(global::Notices.GAME_SAVED, new global::DelReceiveNotice(this.OnCampaignSaved));
		global::PandoraSingleton<global::GameManager>.Instance.Save.NewCampaign(global::PandoraSingleton<global::GameManager>.Instance.campaign, id, rank);
	}

	private void OnCampaignSaved()
	{
		global::PandoraSingleton<global::NoticeManager>.Instance.RemoveListener(global::Notices.GAME_SAVED, new global::DelReceiveNotice(this.OnCampaignSaved));
		global::SceneLauncher.Instance.LaunchScene(global::SceneLoadingId.NEW_CAMPAIGN, false, false);
	}

	private void OnInputTypeChanged()
	{
		this.butConfirm.gameObject.SetActive(global::PandoraSingleton<global::PandoraInput>.Instance.lastInputMode == global::PandoraInput.InputMode.JOYSTICK);
	}

	public global::WarbandRankPopupView warbandRankPopup;

	public global::MenuNodeGroup characterNodes;

	public global::MenuNodeGroup flagNodes;

	public global::UnityEngine.Camera mainCamera;

	public global::UnityEngine.UI.Text raceTitle;

	public global::UnityEngine.UI.Text raceDescription;

	public global::ButtonGroup butConfirm;

	public global::ButtonGroup butExit;

	public global::UnityEngine.GameObject darkSideBar;

	public global::UnityEngine.Sprite backIcon;

	public global::UnityEngine.GameObject camPos;

	private int unitMenuNodeIndex;

	private bool needActivateNode;

	private global::System.Collections.Generic.List<global::MainMenuUnit> leaderUnits;

	private bool initialized;
}
