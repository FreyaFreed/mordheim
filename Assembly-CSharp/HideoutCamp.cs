using System;
using UnityEngine;
using UnityEngine.Events;

public class HideoutCamp : global::ICheapState
{
	public HideoutCamp(global::HideoutManager ctrl, global::HideoutCamAnchor anchor)
	{
		this.camAnchor = anchor;
		this.mngr = ctrl;
		this.greenHighlightColor = global::Constant.GetColor(global::ConstantId.COLOR_GREEN) / 2f;
		global::UnityEngine.Color red = global::UnityEngine.Color.red;
		red.a /= 4f;
		this.redHighlightColor = red;
	}

	void global::ICheapState.Destroy()
	{
	}

	void global::ICheapState.Enter(int iFrom)
	{
		global::PandoraSingleton<global::HideoutManager>.Instance.CamManager.ClearLookAtFocus();
		global::PandoraSingleton<global::HideoutManager>.Instance.CamManager.CancelTransition();
		this.mngr.CamManager.dummyCam.transform.position = this.camAnchor.transform.position;
		this.mngr.CamManager.dummyCam.transform.rotation = this.camAnchor.transform.rotation;
		global::PandoraSingleton<global::HideoutManager>.Instance.CamManager.SetDOFTarget(this.camAnchor.dofTarget, 0f);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.ActivateLeftTabModules(false, new global::ModuleId[0]);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.ActivateRightTabModules(false, new global::ModuleId[]
		{
			global::ModuleId.TREASURY
		});
		global::PandoraSingleton<global::HideoutTabManager>.Instance.ActivateCenterTabModules(new global::ModuleId[]
		{
			global::ModuleId.CAMP_SECTIONS,
			global::ModuleId.WARBAND_TABS,
			global::ModuleId.TITLE,
			global::ModuleId.NOTIFICATION
		});
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button1.gameObject.SetActive(true);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button1.SetAction("cancel", "menu_options", 0, false, global::PandoraSingleton<global::HideoutTabManager>.Instance.icnOptions, null);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button1.OnAction(new global::UnityEngine.Events.UnityAction(this.ShowOptions), false, true);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button2.gameObject.SetActive(false);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button3.gameObject.SetActive(false);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button4.gameObject.SetActive(false);
		this.warbandTabs = global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleCenter<global::WarbandTabsModule>(global::ModuleId.WARBAND_TABS);
		this.warbandTabs.Setup(global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleCenter<global::TitleModule>(global::ModuleId.TITLE));
		global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleRight<global::TreasuryModule>(global::ModuleId.TREASURY).Refresh(global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.GetWarbandSave());
		this.campModule = global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleCenter<global::CampSectionsModule>(global::ModuleId.CAMP_SECTIONS);
		this.nodeGroup = global::PandoraSingleton<global::HideoutManager>.Instance.campNodeGroup;
		this.leader = global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.GetLeader();
		if (this.leader != null)
		{
			this.nodeGroup.nodes[2].SetContent(this.leader, null);
			this.leader.SwitchWeapons(global::UnitSlotId.SET1_MAINHAND);
		}
		else
		{
			this.nodeGroup.nodes[2].SetContent(global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.idol);
		}
		global::UnityEngine.Cloth cloth = global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.banner.GetComponentsInChildren<global::UnityEngine.Cloth>(true)[0];
		cloth.enabled = false;
		this.nodeGroup.nodes[3].SetContent(global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.banner);
		cloth.enabled = true;
		this.nodeGroup.nodes[5].SetContent(global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.wagon);
		this.nodeGroup.nodes[0].SetContent(global::PandoraSingleton<global::HideoutManager>.Instance.GetShipmentNodeContent());
		this.nodeGroup.nodes[1].SetContent(global::PandoraSingleton<global::HideoutManager>.Instance.GetShopNodeContent());
		this.nodeGroup.nodes[6].SetContent(global::PandoraSingleton<global::HideoutManager>.Instance.GetNextDayNodeContent());
		global::UnitMenuController dramatis = global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.GetDramatis();
		this.nodeGroup.nodes[4].SetContent(dramatis, null);
		dramatis.SwitchWeapons(global::UnitSlotId.SET1_MAINHAND);
		this.RefreshButtons();
		this.CheckTrophies();
		this.once = true;
	}

	void global::ICheapState.Exit(int iTo)
	{
		this.nodeGroup.Deactivate();
	}

	void global::ICheapState.Update()
	{
	}

	void global::ICheapState.FixedUpdate()
	{
		if (global::PandoraSingleton<global::HideoutManager>.Instance.transitionDone && this.once && global::PandoraSingleton<global::PandoraInput>.Instance.CurrentInputLayer == 0)
		{
			this.once = false;
			this.nodeGroup.Activate(new global::MenuNodeDelegateNode(this.NodeSelecteded), null, new global::MenuNodeDelegateNode(this.NodeConfirmed), global::PandoraInput.InputLayer.NORMAL, false);
			this.nodeGroup.SelectNode(this.nodeGroup.nodes[0]);
			global::PandoraSingleton<global::HideoutManager>.Instance.ShowHideoutTuto(global::HideoutManager.HideoutTutoType.CAMP);
			if (global::PandoraSingleton<global::HideoutManager>.Instance.Date.CurrentDate == global::Constant.GetInt(global::ConstantId.CAL_DAY_START))
			{
				if (global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Units.Count != 1 || global::PandoraSingleton<global::HideoutManager>.Instance.IsPostMission())
				{
					if (!global::PandoraSingleton<global::HideoutManager>.Instance.IsPostMission())
					{
						global::PandoraSingleton<global::HideoutManager>.Instance.ShowHideoutTuto(global::HideoutManager.HideoutTutoType.CAMP_2);
					}
					else if (global::PandoraSingleton<global::HideoutManager>.Instance.IsPostMission())
					{
						global::PandoraSingleton<global::HideoutManager>.Instance.ShowHideoutTuto(global::HideoutManager.HideoutTutoType.CAMP_3);
					}
				}
			}
		}
	}

	public void RefreshButtons()
	{
		this.warbandTabs.SetCurrentTab(global::HideoutManager.State.CAMP);
		this.warbandTabs.Refresh();
		for (int i = 0; i < this.warbandTabs.icons.Count; i++)
		{
			global::TabIcon tabIcon = this.warbandTabs.icons[i];
			if (tabIcon.nodeSlot != global::HideoutCamp.NodeSlot.CAMP)
			{
				if (tabIcon.available)
				{
					this.nodeGroup.nodes[(int)tabIcon.nodeSlot].SetHighlightColor(this.greenHighlightColor);
				}
				else
				{
					this.nodeGroup.nodes[(int)tabIcon.nodeSlot].SetHighlightColor(this.redHighlightColor);
				}
			}
		}
		this.campModule.Setup(new global::UnityEngine.Events.UnityAction<int>(this.IconSelected), null, new global::UnityEngine.Events.UnityAction<int>(this.IconConfirmed));
	}

	private void CheckTrophies()
	{
		int amount = global::PandoraSingleton<global::HideoutManager>.Instance.WarbandChest.GetItem(global::ItemId.WYRDSTONE_FRAGMENT, global::ItemQualityId.NORMAL).amount;
		int amount2 = global::PandoraSingleton<global::HideoutManager>.Instance.WarbandChest.GetItem(global::ItemId.WYRDSTONE_SHARD, global::ItemQualityId.NORMAL).amount;
		int amount3 = global::PandoraSingleton<global::HideoutManager>.Instance.WarbandChest.GetItem(global::ItemId.WYRDSTONE_CLUSTER, global::ItemQualityId.NORMAL).amount;
		if (amount >= 100 && amount2 >= 100 && amount3 >= 100)
		{
			global::PandoraSingleton<global::Hephaestus>.Instance.UnlockAchievement(global::Hephaestus.TrophyId.WYRDSTONES);
		}
		global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.CheckUnitStatus();
	}

	private void NodeSelecteded(global::MenuNode node, int idx)
	{
		this.campModule.Refresh(idx, true);
	}

	private void NodeConfirmed(global::MenuNode node, int idx)
	{
		global::TabIcon tabIcon = this.warbandTabs.GetTabIcon((global::HideoutCamp.NodeSlot)idx);
		if (!tabIcon.available)
		{
			this.nodeGroup.SelectNode(node);
			return;
		}
		switch (idx)
		{
		case 0:
			global::PandoraSingleton<global::HideoutManager>.Instance.StateMachine.ChangeState(9);
			break;
		case 1:
			global::PandoraSingleton<global::HideoutManager>.Instance.StateMachine.ChangeState(10);
			break;
		case 2:
			global::PandoraSingleton<global::HideoutManager>.Instance.StateMachine.ChangeState(1);
			break;
		case 3:
			global::PandoraSingleton<global::HideoutManager>.Instance.StateMachine.ChangeState(3);
			break;
		case 4:
			global::PandoraSingleton<global::HideoutManager>.Instance.StateMachine.ChangeState(11);
			break;
		case 5:
			global::PandoraSingleton<global::HideoutManager>.Instance.StateMachine.ChangeState(12);
			break;
		case 6:
			global::PandoraSingleton<global::HideoutManager>.Instance.OnNextDay();
			break;
		}
	}

	private void IconSelected(int idx)
	{
		this.nodeGroup.SelectNode(this.nodeGroup.nodes[idx]);
	}

	private void IconConfirmed(int idx)
	{
		if (global::PandoraSingleton<global::PandoraInput>.Instance.CurrentInputLayer == 0)
		{
			this.NodeConfirmed(null, idx);
		}
	}

	private void ShowOptions()
	{
		global::PandoraSingleton<global::HideoutManager>.Instance.StateMachine.ChangeState(17);
	}

	private global::HideoutManager mngr;

	private global::HideoutCamAnchor camAnchor;

	private global::MenuNodeGroup nodeGroup;

	private global::CampSectionsModule campModule;

	private global::UnitMenuController leader;

	private global::WarbandTabsModule warbandTabs;

	private global::UnityEngine.Color greenHighlightColor;

	private global::UnityEngine.Color redHighlightColor;

	private bool once = true;

	public enum NodeSlot
	{
		CAMP = -1,
		SHIPMENT,
		SHOP,
		LEADER,
		BANNER,
		DRAMATIS,
		WAGON,
		NEXT_DAY
	}
}
