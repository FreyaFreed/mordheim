using System;
using System.Collections.Generic;

public class WarbandManagementBaseState : global::ICheapState
{
	protected WarbandManagementBaseState(global::HideoutManager mng, global::ModuleId warbandManagementModuleId)
	{
		this.warbandManagementModuleId = warbandManagementModuleId;
		this.hideoutMngr = mng;
	}

	void global::ICheapState.Destroy()
	{
	}

	void global::ICheapState.Update()
	{
	}

	public virtual void Enter(int iFrom)
	{
		this.currentUnit = null;
		this.currentUnitSlotIndex = -1;
		this.warbandManagementModule = global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleCenter<global::WarbandManagementModule>(this.warbandManagementModuleId);
		this.warbandManagementModule.Set(this.hideoutMngr.WarbandCtrlr.Warband, new global::System.Action<int, global::Unit, bool>(this.NodeSelected), new global::System.Action<int, global::Unit, bool>(this.NodeConfirmed), new global::System.Action(this.ShowSwap), new global::System.Action(this.OnHiredSwordsSelected), new global::System.Action(this.OnHiredSwordsConfirmed));
		global::HideoutTabManager instance = global::PandoraSingleton<global::HideoutTabManager>.Instance;
		bool displayBg = false;
		global::ModuleId[] modules;
		(modules = new global::ModuleId[1])[0] = global::ModuleId.TREASURY;
		instance.ActivateRightTabModules(displayBg, modules);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleRight<global::TreasuryModule>(global::ModuleId.TREASURY).Refresh(global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.GetWarbandSave());
		this.SetupDefaultButtons();
		this.warbandManagementModule.leaderSlots[0].slot.SetSelected(true);
	}

	public virtual void Exit(int iTo)
	{
	}

	private void NodeSelected(int slotIndex, global::Unit unit, bool isImpressive)
	{
		this.currentUnitIsImpressive = isImpressive;
		this.currentUnitSlotIndex = slotIndex;
		this.currentUnit = unit;
		if (slotIndex == 0)
		{
			this.OnIdolSelected();
		}
		else if (this.currentUnit != null)
		{
			this.OnUnitSelected(this.currentUnit);
		}
		else
		{
			this.OnEmptyNodeSelected(slotIndex, isImpressive);
		}
	}

	private void NodeConfirmed(int slotIndex, global::Unit unit, bool isImpressive)
	{
		this.currentUnitIsImpressive = isImpressive;
		this.currentUnitSlotIndex = slotIndex;
		this.currentUnit = unit;
		if (slotIndex == 0)
		{
			this.OnIdolConfirmed();
		}
		else if (this.currentUnit != null)
		{
			this.OnUnitConfirmed(this.currentUnit);
		}
		else
		{
			this.OnEmptyNodeConfirmed(isImpressive);
		}
	}

	protected virtual void OnEmptyNodeConfirmed(bool isImpressive)
	{
	}

	protected virtual void OnHiredSwordsConfirmed()
	{
	}

	protected virtual void OnHiredSwordsSelected()
	{
	}

	protected virtual void OnIdolSelected()
	{
	}

	protected virtual void OnIdolConfirmed()
	{
	}

	protected virtual void OnUnitSelected(global::Unit unit)
	{
		if (unit != null)
		{
			global::PandoraSingleton<global::HideoutTabManager>.Instance.ActivateLeftTabModules(true, new global::ModuleId[]
			{
				global::ModuleId.UNIT_SHEET,
				global::ModuleId.UNIT_QUICK_STATS
			});
			global::UnitQuickStatsModule moduleLeft = global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleLeft<global::UnitQuickStatsModule>(global::ModuleId.UNIT_QUICK_STATS);
			moduleLeft.RefreshStats(unit, null);
			moduleLeft.SetInteractable(false);
			global::UnitSheetModule moduleLeft2 = global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleLeft<global::UnitSheetModule>(global::ModuleId.UNIT_SHEET);
			moduleLeft2.SetInteractable(false);
			moduleLeft2.Refresh(null, unit, null, null, null);
			this.SetupDefaultButtons();
		}
	}

	protected virtual void OnUnitConfirmed(global::Unit unit)
	{
		this.ShowSwap();
	}

	protected virtual void OnEmptyNodeSelected(int slotIndex, bool isImpressive)
	{
	}

	public virtual void ShowSwap()
	{
		global::PandoraSingleton<global::HideoutTabManager>.Instance.ActivateCenterTabModules(new global::ModuleId[]
		{
			global::ModuleId.SWAP
		});
		global::PandoraSingleton<global::HideoutTabManager>.Instance.ActivateLeftTabModules(false, new global::ModuleId[0]);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.ActivateRightTabModules(false, new global::ModuleId[0]);
		global::WarbandSwapModule moduleCenter = global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleCenter<global::WarbandSwapModule>(global::ModuleId.SWAP);
		moduleCenter.Set(this.hideoutMngr.WarbandCtrlr.Warband, new global::System.Action<bool>(this.HideSwap), false, false, false, null, false, 0, 9999);
	}

	protected virtual void HideSwap(bool confirm)
	{
		global::PandoraSingleton<global::HideoutTabManager>.Instance.ActivateCenterTabModules(new global::ModuleId[]
		{
			this.warbandManagementModuleId,
			global::ModuleId.NOTIFICATION
		});
		global::PandoraSingleton<global::HideoutTabManager>.Instance.ActivateRightTabModules(false, new global::ModuleId[]
		{
			global::ModuleId.TREASURY
		});
		this.warbandManagementModule.Set(this.hideoutMngr.WarbandCtrlr.Warband, new global::System.Action<int, global::Unit, bool>(this.NodeSelected), new global::System.Action<int, global::Unit, bool>(this.NodeConfirmed), new global::System.Action(this.ShowSwap), new global::System.Action(this.OnHiredSwordsSelected), new global::System.Action(this.OnHiredSwordsConfirmed));
		this.SetupDefaultButtons();
	}

	protected virtual void SetupDefaultButtons()
	{
	}

	protected void SelectCurrentUnit()
	{
		this.OnUnitConfirmed(this.currentUnit);
	}

	public virtual void FixedUpdate()
	{
	}

	private global::System.Collections.Generic.List<int> cannotSwapNodes = new global::System.Collections.Generic.List<int>();

	protected global::HideoutManager hideoutMngr;

	protected global::WarbandManagementModule warbandManagementModule;

	private global::ModuleId warbandManagementModuleId;

	protected global::Unit currentUnit;

	protected int currentUnitSlotIndex;

	protected bool currentUnitIsImpressive;
}
