using System;
using System.Collections.Generic;
using UnityEngine;

public class HideoutWarband : global::WarbandManagementBaseState
{
	public HideoutWarband(global::HideoutManager mng, global::HideoutCamAnchor anchor) : base(mng, global::ModuleId.WARBAND_MANAGEMENT)
	{
		this.camAnchor = anchor;
		this.once = true;
	}

	public override void Enter(int iFrom)
	{
		global::PandoraSingleton<global::HideoutManager>.Instance.CamManager.ClearLookAtFocus();
		global::PandoraSingleton<global::HideoutManager>.Instance.CamManager.CancelTransition();
		this.hideoutMngr.CamManager.dummyCam.transform.position = this.camAnchor.transform.position;
		this.hideoutMngr.CamManager.dummyCam.transform.rotation = this.camAnchor.transform.rotation;
		global::PandoraSingleton<global::HideoutManager>.Instance.CamManager.SetDOFTarget(this.camAnchor.dofTarget, 0f);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.ActivateLeftTabModules(true, new global::ModuleId[]
		{
			global::ModuleId.WARBAND_SHEET
		});
		global::PandoraSingleton<global::HideoutTabManager>.Instance.ActivateCenterTabModules(new global::ModuleId[]
		{
			global::ModuleId.WARBAND_TABS,
			global::ModuleId.TITLE,
			global::ModuleId.NOTIFICATION,
			global::ModuleId.WARBAND_MANAGEMENT
		});
		this.warbandSheet = global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleLeft<global::WarbandSheetModule>(global::ModuleId.WARBAND_SHEET);
		this.warbandSheet.Set(global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband);
		this.warbandTabs = global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleCenter<global::WarbandTabsModule>(global::ModuleId.WARBAND_TABS);
		this.warbandTabs.Setup(global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleCenter<global::TitleModule>(global::ModuleId.TITLE));
		this.warbandTabs.SetCurrentTab(global::HideoutManager.State.WARBAND);
		this.warbandTabs.Refresh();
		this.unitOverviewModule = global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleLeft<global::WarbandOverviewUnitsModule>(global::ModuleId.WARBAND_OVERVIEW_UNITS);
		this.hideoutMngr.warbandNodeWagon.SetContent(global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.wagon);
		this.nodeGroup = global::PandoraSingleton<global::HideoutManager>.Instance.warbandNodeGroup;
		this.nodeGroup.Deactivate();
		this.PlaceUnits();
		base.Enter(iFrom);
		this.treasuryModule = global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleRight<global::TreasuryModule>(global::ModuleId.TREASURY);
	}

	public override void Exit(int iTo)
	{
		global::PandoraSingleton<global::HideoutTabManager>.Instance.DeactivateAllButtons();
		this.ClearUnits();
	}

	public override void FixedUpdate()
	{
		if (this.once)
		{
			this.once = false;
			global::PandoraSingleton<global::HideoutManager>.Instance.ShowHideoutTuto(global::HideoutManager.HideoutTutoType.MANAGEMENT);
		}
	}

	private void PlaceUnits()
	{
		for (int i = 0; i < this.nodeGroup.nodes.Count; i++)
		{
			this.nodeGroup.nodes[i].RemoveContent();
			this.nodeGroup.nodes[i].Hide();
		}
		global::System.Collections.Generic.List<global::UnitMenuController> unitCtrlrs = global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.unitCtrlrs;
		for (int j = 0; j < unitCtrlrs.Count; j++)
		{
			global::UnitMenuController unitMenuController = unitCtrlrs[j];
			int warbandSlotIndex = unitMenuController.unit.UnitSave.warbandSlotIndex;
			if (warbandSlotIndex >= 0 && warbandSlotIndex < 20)
			{
				if (this.nodeGroup.nodes[warbandSlotIndex].IsOccupied())
				{
					global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.FindSuitableSlot(unitMenuController.unit, false);
				}
				if (warbandSlotIndex == 5 && unitMenuController.unit.IsImpressive)
				{
					this.nodeGroup.nodes[1].SetContent(unitMenuController, null);
				}
				else
				{
					this.nodeGroup.nodes[warbandSlotIndex].SetContent(unitMenuController, (!unitMenuController.unit.IsImpressive) ? null : this.nodeGroup.nodes[warbandSlotIndex + 1]);
				}
				unitMenuController.SwitchWeapons(global::UnitSlotId.SET1_MAINHAND);
				unitMenuController.animator.Play(global::AnimatorIds.idle, -1, (float)global::PandoraSingleton<global::GameManager>.Instance.LocalTyche.Rand(0.0, 1.0));
			}
		}
	}

	public void ClearUnits()
	{
		for (int i = 0; i < this.nodeGroup.nodes.Count; i++)
		{
			this.nodeGroup.nodes[i].RemoveContent();
		}
	}

	protected override void OnEmptyNodeConfirmed(bool isImpressive)
	{
		base.OnEmptyNodeConfirmed(isImpressive);
		if (this.hideoutMngr.WarbandCtrlr.Warband.CanHireMoreUnit(isImpressive))
		{
			this.HireUnit();
		}
	}

	protected override void OnIdolSelected()
	{
		base.OnIdolSelected();
		global::PandoraSingleton<global::HideoutTabManager>.Instance.ActivateLeftTabModules(true, new global::ModuleId[]
		{
			global::ModuleId.WARBAND_SHEET,
			global::ModuleId.WARBAND_OVERVIEW_UNITS,
			global::ModuleId.WARBAND_NEXT_RANK_PREVIEW
		});
		this.warbandSheet.Set(global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband);
		this.unitOverviewModule.Set(global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleLeft<global::WarbandRankBonusPreviewModule>(global::ModuleId.WARBAND_NEXT_RANK_PREVIEW).Set(global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button1.gameObject.SetActive(true);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button1.SetAction("cancel", "go_to_camp", 0, false, global::PandoraSingleton<global::HideoutTabManager>.Instance.icnBack, null);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button1.OnAction(delegate
		{
			global::PandoraSingleton<global::HideoutManager>.Instance.StateMachine.ChangeState(0);
		}, false, true);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button2.gameObject.SetActive(false);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button3.gameObject.SetActive(false);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button4.gameObject.SetActive(false);
	}

	protected override void OnIdolConfirmed()
	{
		base.OnIdolConfirmed();
		global::PandoraSingleton<global::HideoutManager>.Instance.StateMachine.ChangeState(18);
	}

	protected override void OnUnitSelected(global::Unit unit)
	{
		base.OnUnitSelected(unit);
		if (unit != null)
		{
			global::UnitMenuController unitMenuController = this.hideoutMngr.GetUnitMenuController(unit);
			this.hideoutMngr.CamManager.SetDOFTarget(unitMenuController.gameObject.transform, 0f);
		}
		global::UnityEngine.GameObject gameObject = null;
		if (unit != null)
		{
			global::UnitMenuController unitMenuController2 = global::PandoraSingleton<global::HideoutManager>.Instance.GetUnitMenuController(unit);
			if (unitMenuController2 != null)
			{
				gameObject = unitMenuController2.gameObject;
			}
		}
		if (gameObject != null)
		{
			for (int i = 0; i < this.nodeGroup.nodes.Count; i++)
			{
				if (this.nodeGroup.nodes[i].IsContent(gameObject))
				{
					this.nodeGroup.SelectNode(this.nodeGroup.nodes[i]);
					this.nodeGroup.nodes[i].highlightable.ReinitMaterials();
					break;
				}
			}
		}
		else
		{
			this.nodeGroup.UnSelectCurrentNode();
		}
	}

	protected override void OnEmptyNodeSelected(int slotIndex, bool isImpressive)
	{
		this.nodeGroup.UnSelectCurrentNode();
		global::PandoraSingleton<global::HideoutTabManager>.Instance.ActivateLeftTabModules(true, new global::ModuleId[]
		{
			global::ModuleId.WARBAND_SHEET,
			global::ModuleId.AVAILABLE_UNITS
		});
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button1.gameObject.SetActive(true);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button1.SetAction("cancel", "go_to_camp", 0, false, global::PandoraSingleton<global::HideoutTabManager>.Instance.icnBack, null);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button1.OnAction(delegate
		{
			global::PandoraSingleton<global::HideoutManager>.Instance.StateMachine.ChangeState(0);
		}, false, true);
		if (global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.CanHireMoreUnit(isImpressive))
		{
			global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleLeft<global::AvailableUnitsModule>(global::ModuleId.AVAILABLE_UNITS).Set(global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.GetHireableUnits(slotIndex, isImpressive), true);
		}
		else
		{
			global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleLeft<global::AvailableUnitsModule>(global::ModuleId.AVAILABLE_UNITS).Set(global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.GetHireableUnits(slotIndex, isImpressive), false);
		}
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button2.gameObject.SetActive(false);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button3.gameObject.SetActive(false);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button4.gameObject.SetActive(false);
	}

	protected override void OnHiredSwordsConfirmed()
	{
		base.OnHiredSwordsConfirmed();
		if (global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.GetHiredSwordUnits().Count > 0)
		{
			this.hideoutMngr.currentWarbandSlotIdx = -1;
			this.hideoutMngr.StateMachine.ChangeState(14);
		}
	}

	protected override void OnHiredSwordsSelected()
	{
		global::PandoraSingleton<global::HideoutTabManager>.Instance.ActivateLeftTabModules(true, new global::ModuleId[]
		{
			global::ModuleId.WARBAND_SHEET,
			global::ModuleId.AVAILABLE_UNITS
		});
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button1.gameObject.SetActive(true);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button1.SetAction("cancel", "go_to_camp", 0, false, global::PandoraSingleton<global::HideoutTabManager>.Instance.icnBack, null);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button1.OnAction(delegate
		{
			global::PandoraSingleton<global::HideoutManager>.Instance.StateMachine.ChangeState(0);
		}, false, true);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleLeft<global::AvailableUnitsModule>(global::ModuleId.AVAILABLE_UNITS).Set(global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.GetHiredSwordUnits(), true);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button2.gameObject.SetActive(false);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button3.gameObject.SetActive(false);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button4.gameObject.SetActive(false);
	}

	protected override void SetupDefaultButtons()
	{
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button1.gameObject.SetActive(true);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button1.SetAction("cancel", "go_to_camp", 0, false, global::PandoraSingleton<global::HideoutTabManager>.Instance.icnBack, null);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button1.OnAction(delegate
		{
			global::PandoraSingleton<global::HideoutManager>.Instance.StateMachine.ChangeState(0);
		}, false, true);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button2.gameObject.SetActive(false);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button3.gameObject.SetActive(false);
	}

	private void HireUnit()
	{
		this.hideoutMngr.currentWarbandSlotIdx = this.currentUnitSlotIndex;
		this.hideoutMngr.currentWarbandSlotHireImpressive = this.currentUnitIsImpressive;
		this.hideoutMngr.StateMachine.ChangeState(14);
	}

	protected override void HideSwap(bool confirm)
	{
		base.HideSwap(confirm);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.ActivateLeftTabModules(true, new global::ModuleId[]
		{
			global::ModuleId.WARBAND_SHEET
		});
		global::PandoraSingleton<global::HideoutTabManager>.Instance.ActivateCenterTabModules(new global::ModuleId[]
		{
			global::ModuleId.WARBAND_TABS,
			global::ModuleId.TITLE,
			global::ModuleId.NOTIFICATION,
			global::ModuleId.WARBAND_MANAGEMENT
		});
		this.PlaceUnits();
	}

	protected override void OnUnitConfirmed(global::Unit unit)
	{
		this.hideoutMngr.currentUnit = global::PandoraSingleton<global::HideoutManager>.Instance.GetUnitMenuController(unit);
		this.hideoutMngr.StateMachine.ChangeState(15);
	}

	private global::MenuNodeGroup nodeGroup;

	private global::HideoutCamAnchor camAnchor;

	private global::WarbandTabsModule warbandTabs;

	private global::TreasuryModule treasuryModule;

	private global::WarbandOverviewUnitsModule unitOverviewModule;

	private global::WarbandSheetModule warbandSheet;

	private bool once = true;
}
