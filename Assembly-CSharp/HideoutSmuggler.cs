using System;
using UnityEngine.UI;

public class HideoutSmuggler : global::ICheapState
{
	public HideoutSmuggler(global::HideoutManager mng, global::HideoutCamAnchor anchor)
	{
		this.camAnchor = anchor;
	}

	void global::ICheapState.Destroy()
	{
	}

	void global::ICheapState.Enter(int iFrom)
	{
		global::PandoraSingleton<global::HideoutManager>.Instance.CamManager.ClearLookAtFocus();
		global::PandoraSingleton<global::HideoutManager>.Instance.CamManager.CancelTransition();
		global::PandoraSingleton<global::HideoutManager>.Instance.CamManager.dummyCam.transform.position = this.camAnchor.transform.position;
		global::PandoraSingleton<global::HideoutManager>.Instance.CamManager.dummyCam.transform.rotation = this.camAnchor.transform.rotation;
		global::PandoraSingleton<global::HideoutManager>.Instance.CamManager.SetDOFTarget(this.camAnchor.dofTarget, 0f);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.ActivateLeftTabModules(false, new global::ModuleId[]
		{
			global::ModuleId.SMUGGLER
		});
		global::PandoraSingleton<global::HideoutTabManager>.Instance.ActivateRightTabModules(false, new global::ModuleId[]
		{
			global::ModuleId.SMUGGLER,
			global::ModuleId.TREASURY
		});
		global::PandoraSingleton<global::HideoutTabManager>.Instance.ActivateCenterTabModules(new global::ModuleId[]
		{
			global::ModuleId.SMUGGLER,
			global::ModuleId.WARBAND_TABS,
			global::ModuleId.TITLE,
			global::ModuleId.NOTIFICATION
		});
		this.warbandTabs = global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleCenter<global::WarbandTabsModule>(global::ModuleId.WARBAND_TABS);
		this.warbandTabs.Setup(global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleCenter<global::TitleModule>(global::ModuleId.TITLE));
		this.warbandTabs.SetCurrentTab(global::HideoutManager.State.SHIPMENT);
		this.warbandTabs.Refresh();
		this.treasury = global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleRight<global::TreasuryModule>(global::ModuleId.TREASURY);
		this.treasury.Refresh(global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.GetWarbandSave());
		this.factionShipment = global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleCenter<global::SmugglerFactionShipmentModule>(global::ModuleId.SMUGGLER);
		global::UnityEngine.UI.Toggle[] componentsInChildren = this.factionShipment.GetComponentsInChildren<global::UnityEngine.UI.Toggle>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].onValueChanged.AddListener(delegate(bool on)
			{
				if (on)
				{
					this.SetCenterPanelButtons();
				}
			});
		}
		this.factionBonus = global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleRight<global::SmugglerFactionBonusModule>(global::ModuleId.SMUGGLER);
		this.factionsOverview = global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleLeft<global::SmugglerFactionOverviewModule>(global::ModuleId.SMUGGLER);
		this.factionsOverview.Setup(new global::System.Action<global::FactionMenuController>(this.OnFactionHighlighted), new global::System.Action<global::FactionMenuController>(this.OnFactionConfirmed));
		this.SetFocusOnLeftPanel();
		this.ValidateFactionReputations();
		this.once = true;
	}

	void global::ICheapState.Exit(int iTo)
	{
	}

	void global::ICheapState.Update()
	{
	}

	void global::ICheapState.FixedUpdate()
	{
		if (this.once)
		{
			this.once = false;
			global::PandoraSingleton<global::HideoutManager>.Instance.ShowHideoutTuto(global::HideoutManager.HideoutTutoType.SMUGGLER);
		}
	}

	private void ValidateFactionReputations()
	{
		for (int i = 0; i < global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.factionCtrlrs.Count; i++)
		{
			global::FactionMenuController factionMenuController = global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.factionCtrlrs[i];
			int rank = factionMenuController.Faction.Rank;
			int num = factionMenuController.AddReputation(0);
			if (rank != num)
			{
				global::PandoraSingleton<global::HideoutManager>.Instance.Progressor.IncrementFactionRank(factionMenuController, num);
			}
		}
	}

	private void OnFactionHighlighted(global::FactionMenuController faction)
	{
		this.factionShipment.OnLostFocus();
		this.factionShipment.Setup(faction, new global::System.Action(this.OnShipmentSent));
		this.factionBonus.Setup(faction);
	}

	private void OnFactionConfirmed(global::FactionMenuController faction)
	{
		this.factionsOverview.OnLostFocus();
		this.SetFocusOnCenterPanel();
	}

	private void OnShipmentSent()
	{
		this.factionsOverview.Refresh();
		this.SetFocusOnLeftPanel();
		this.treasury.Refresh(global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.GetWarbandSave());
	}

	private void SetLeftPanelButtons()
	{
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button1.gameObject.SetActive(true);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button1.SetAction("cancel", "go_to_camp", 0, false, global::PandoraSingleton<global::HideoutTabManager>.Instance.icnBack, null);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button1.OnAction(delegate
		{
			global::PandoraSingleton<global::HideoutManager>.Instance.StateMachine.ChangeState(0);
		}, false, true);
		if (global::PandoraSingleton<global::PandoraInput>.Instance.lastInputMode == global::PandoraInput.InputMode.JOYSTICK)
		{
			global::PandoraSingleton<global::HideoutTabManager>.Instance.button2.SetAction("action", "menu_confirm", 0, false, null, null);
			global::PandoraSingleton<global::HideoutTabManager>.Instance.button2.OnAction(null, false, true);
		}
		else
		{
			global::PandoraSingleton<global::HideoutTabManager>.Instance.button2.gameObject.SetActive(false);
		}
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button3.gameObject.SetActive(false);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button4.gameObject.SetActive(false);
	}

	private void SetCenterPanelButtons()
	{
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button1.gameObject.SetActive(true);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button1.SetAction("cancel", "menu_back", 0, false, global::PandoraSingleton<global::HideoutTabManager>.Instance.icnBack, null);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button1.OnAction(delegate
		{
			this.SetFocusOnLeftPanel();
		}, false, true);
		if (global::PandoraSingleton<global::PandoraInput>.Instance.lastInputMode == global::PandoraInput.InputMode.JOYSTICK)
		{
			global::PandoraSingleton<global::HideoutTabManager>.Instance.button2.SetAction("action", "menu_confirm", 0, false, null, null);
			global::PandoraSingleton<global::HideoutTabManager>.Instance.button2.OnAction(null, false, true);
		}
		else
		{
			global::PandoraSingleton<global::HideoutTabManager>.Instance.button2.gameObject.SetActive(false);
		}
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button3.gameObject.SetActive(false);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button4.gameObject.SetActive(false);
	}

	private void SetFocusOnCenterPanel()
	{
		this.SetCenterPanelButtons();
		this.factionShipment.SetFocus();
	}

	private void SetFocusOnLeftPanel()
	{
		this.SetLeftPanelButtons();
		this.factionsOverview.SetFocus();
		this.factionShipment.OnLostFocus();
	}

	private global::HideoutCamAnchor camAnchor;

	private global::TreasuryModule treasury;

	private global::SmugglerFactionOverviewModule factionsOverview;

	private global::SmugglerFactionShipmentModule factionShipment;

	private global::SmugglerFactionBonusModule factionBonus;

	private global::WarbandTabsModule warbandTabs;

	private bool once = true;
}
