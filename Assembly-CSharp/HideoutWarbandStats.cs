using System;
using UnityEngine;
using UnityEngine.Events;

public class HideoutWarbandStats : global::ICheapState
{
	public HideoutWarbandStats(global::HideoutManager mng, global::HideoutCamAnchor anchor)
	{
		this.camAnchor = anchor;
	}

	public void Destroy()
	{
	}

	public void Enter(int iFrom)
	{
		this.warband = global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband;
		global::PandoraSingleton<global::HideoutManager>.Instance.CamManager.dummyCam.transform.position = this.camAnchor.transform.position;
		global::PandoraSingleton<global::HideoutManager>.Instance.CamManager.dummyCam.transform.rotation = this.camAnchor.transform.rotation;
		global::PandoraSingleton<global::HideoutManager>.Instance.CamManager.SetDOFTarget(this.camAnchor.dofTarget, 0f);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.ActivateCenterTabModules(new global::ModuleId[]
		{
			global::ModuleId.WARBAND_TABS,
			global::ModuleId.TITLE,
			global::ModuleId.NOTIFICATION,
			global::ModuleId.UNIT_DESC
		});
		global::PandoraSingleton<global::HideoutTabManager>.Instance.ActivateLeftTabModules(true, new global::ModuleId[]
		{
			global::ModuleId.WARBAND_SHEET,
			global::ModuleId.WARBAND_OVERVIEW_UNITS,
			global::ModuleId.WARBAND_NEXT_RANK_PREVIEW
		});
		global::PandoraSingleton<global::HideoutTabManager>.Instance.ActivateRightTabModules(true, new global::ModuleId[]
		{
			global::ModuleId.TREASURY,
			global::ModuleId.WARBAND_OVERVIEW
		});
		this.warbandOverviewModule = global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleRight<global::WarbandOverviewModule>(global::ModuleId.WARBAND_OVERVIEW);
		this.warbandOverviewModule.Set(global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband);
		this.warbandOverviewModule.ShowPanel(0);
		this.unitOverviewModule = global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleLeft<global::WarbandOverviewUnitsModule>(global::ModuleId.WARBAND_OVERVIEW_UNITS);
		this.warbandSheet = global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleLeft<global::WarbandSheetModule>(global::ModuleId.WARBAND_SHEET);
		this.treasuryModule = global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleRight<global::TreasuryModule>(global::ModuleId.TREASURY);
		this.statusDescModule = global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleCenter<global::StatusDescModule>(global::ModuleId.UNIT_DESC);
		this.statusDescModule.onPayUpkeep = new global::UnityEngine.Events.UnityAction(this.PayAllUpkeep);
		this.statusDescModule.onPayTreatment = new global::UnityEngine.Events.UnityAction(this.PayAllTreatment);
		this.statusDescModule.onFireUnit = new global::UnityEngine.Events.UnityAction(this.DisbandWarband);
		this.statusDescModule.Refresh(this.warband);
		this.statusDescModule.SetNav(null, this.warbandOverviewModule.tabs[0].toggle);
		this.statusDescModule.SetFocus();
		global::UnityEngine.Cloth cloth = global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.banner.GetComponentsInChildren<global::UnityEngine.Cloth>(true)[0];
		cloth.enabled = false;
		global::PandoraSingleton<global::HideoutManager>.Instance.warbandNodeFlag.SetContent(global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.banner);
		cloth.enabled = true;
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button1.SetAction("cancel", "go_to_warband", 0, false, global::PandoraSingleton<global::HideoutTabManager>.Instance.icnBack, null);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button1.OnAction(new global::UnityEngine.Events.UnityAction(this.ReturnToWarband), false, true);
		if (global::PandoraSingleton<global::Hephaestus>.Instance.IsPrivilegeRestricted(global::Hephaestus.RestrictionId.UGC))
		{
			global::PandoraSingleton<global::HideoutTabManager>.Instance.button2.gameObject.SetActive(false);
		}
		else
		{
			global::PandoraSingleton<global::HideoutTabManager>.Instance.button2.SetAction("rename_warband", "hideout_rename_warband", 0, false, null, null);
			global::PandoraSingleton<global::HideoutTabManager>.Instance.button2.OnAction(new global::UnityEngine.Events.UnityAction(this.RenameWarband), false, true);
		}
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button3.gameObject.SetActive(false);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button4.gameObject.SetActive(false);
	}

	public void Exit(int iTo)
	{
	}

	public void Update()
	{
	}

	public void FixedUpdate()
	{
	}

	private void ReturnToWarband()
	{
		global::PandoraSingleton<global::HideoutManager>.Instance.StateMachine.ChangeState(1);
	}

	private void RenameWarband()
	{
		if (global::PandoraSingleton<global::PandoraInput>.Instance.lastInputMode == global::PandoraInput.InputMode.JOYSTICK)
		{
			if (!global::PandoraSingleton<global::Hephaestus>.Instance.ShowVirtualKeyboard(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_rename_warband"), this.warband.GetWarbandSave().Name, (uint)global::Constant.GetInt(global::ConstantId.MAX_WARBAND_NAME_LENGTH), false, new global::Hephaestus.OnVirtualKeyboardCallback(this.OnRenameWarbandDialog), true))
			{
				global::PandoraSingleton<global::HideoutManager>.Instance.textInputPopup.ShowLocalized(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_rename_warband"), global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_rename_warband_desc"), new global::System.Action<bool, string>(this.OnRenameWarbandDialog), false, this.warband.GetWarbandSave().Name, global::Constant.GetInt(global::ConstantId.MAX_WARBAND_NAME_LENGTH));
			}
		}
		else
		{
			global::PandoraSingleton<global::HideoutManager>.Instance.textInputPopup.ShowLocalized(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_rename_warband"), global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_rename_warband_desc"), new global::System.Action<bool, string>(this.OnRenameWarbandDialog), false, this.warband.GetWarbandSave().Name, global::Constant.GetInt(global::ConstantId.MAX_WARBAND_NAME_LENGTH));
		}
	}

	private void OnRenameWarbandDialog(bool confirm, string newName)
	{
		if (confirm && !string.IsNullOrEmpty(newName))
		{
			this.warband.GetWarbandSave().overrideName = newName;
			this.warbandSheet.Set(this.warband);
			global::PandoraSingleton<global::HideoutManager>.Instance.SaveChanges();
			global::PandoraSingleton<global::Hephaestus>.Instance.UnlockAchievement(global::Hephaestus.TrophyId.RENAME);
		}
	}

	private void DisbandWarband()
	{
		global::PandoraSingleton<global::HideoutManager>.Instance.messagePopup.ShowLocalized(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_disband"), global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_disband_desc"), new global::System.Action<bool>(this.OnDisbandWarbandDialog), false, false);
		global::PandoraSingleton<global::HideoutManager>.Instance.messagePopup.cancelButton.effects.toggle.isOn = true;
	}

	private void OnDisbandWarbandDialog(bool confirm)
	{
		if (confirm)
		{
			int num = this.warband.GetTotalUpkeepOwned() + this.warband.GetTotalTreatmentOwned();
			if (global::PandoraSingleton<global::HideoutManager>.Instance.WarbandChest.GetGold() >= num)
			{
				global::PandoraSingleton<global::HideoutManager>.Instance.WarbandChest.RemoveGold(num);
				this.treasuryModule.Refresh(this.warband.GetWarbandSave());
				this.warband.PayAllUpkeepOwned();
				this.warband.PayAllTreatmentOwned();
				global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.DisbandWarband();
				global::PandoraSingleton<global::HideoutManager>.Instance.SaveChanges();
				this.unitOverviewModule.Set(this.warband);
			}
			else
			{
				global::PandoraSingleton<global::HideoutManager>.Instance.messagePopup.ShowLocalized(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_no_gold"), global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_no_gold_upkeep"), null, false, false);
				global::PandoraSingleton<global::HideoutManager>.Instance.messagePopup.HideCancelButton();
			}
		}
	}

	private void PayAllUpkeep()
	{
		int totalUpkeepOwned = global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.GetTotalUpkeepOwned();
		global::PandoraSingleton<global::HideoutManager>.Instance.messagePopup.ShowLocalized(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_btn_pay_all_upkeep"), global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_pay_all_upkeep_desc", new string[]
		{
			totalUpkeepOwned.ToString(),
			global::PandoraSingleton<global::HideoutManager>.Instance.WarbandChest.GetGold().ToString()
		}), new global::System.Action<bool>(this.OnPayUpkeepDialog), false, false);
	}

	private void OnPayUpkeepDialog(bool confirm)
	{
		if (confirm)
		{
			int totalUpkeepOwned = global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.GetTotalUpkeepOwned();
			if (global::PandoraSingleton<global::HideoutManager>.Instance.WarbandChest.GetGold() >= totalUpkeepOwned)
			{
				global::PandoraSingleton<global::HideoutManager>.Instance.WarbandChest.RemoveGold(totalUpkeepOwned);
				global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.PayAllUpkeepOwned();
				global::PandoraSingleton<global::HideoutManager>.Instance.SaveChanges();
				this.treasuryModule.Refresh(global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.GetWarbandSave());
				this.unitOverviewModule.Set(global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband);
				this.warbandSheet.Set(global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband);
				this.statusDescModule.Refresh(global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband);
				this.statusDescModule.SetNav(null, this.warbandOverviewModule.tabs[0].toggle);
				this.statusDescModule.SetFocus();
			}
			else
			{
				global::PandoraSingleton<global::HideoutManager>.Instance.messagePopup.ShowLocalized(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_no_gold"), global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_no_gold_upkeep"), null, false, false);
				global::PandoraSingleton<global::HideoutManager>.Instance.messagePopup.HideCancelButton();
			}
		}
	}

	private void PayAllTreatment()
	{
		int totalTreatmentOwned = global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.GetTotalTreatmentOwned();
		global::PandoraSingleton<global::HideoutManager>.Instance.messagePopup.ShowLocalized(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_pay_all_treatment"), global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_pay_all_treatment_desc", new string[]
		{
			totalTreatmentOwned.ToString(),
			global::PandoraSingleton<global::HideoutManager>.Instance.WarbandChest.GetGold().ToString()
		}), new global::System.Action<bool>(this.OnPayTreatmentDialog), false, false);
	}

	private void OnPayTreatmentDialog(bool confirm)
	{
		if (confirm)
		{
			int totalTreatmentOwned = global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.GetTotalTreatmentOwned();
			if (global::PandoraSingleton<global::HideoutManager>.Instance.WarbandChest.GetGold() >= totalTreatmentOwned)
			{
				global::PandoraSingleton<global::HideoutManager>.Instance.WarbandChest.RemoveGold(totalTreatmentOwned);
				global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.PayAllTreatmentOwned();
				global::PandoraSingleton<global::HideoutManager>.Instance.SaveChanges();
				this.treasuryModule.Refresh(global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.GetWarbandSave());
				this.unitOverviewModule.Set(global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband);
				this.warbandSheet.Set(global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband);
				this.statusDescModule.Refresh(global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband);
				this.statusDescModule.SetNav(null, this.warbandOverviewModule.tabs[0].toggle);
				this.statusDescModule.SetFocus();
			}
			else
			{
				global::PandoraSingleton<global::HideoutManager>.Instance.messagePopup.ShowLocalized(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_no_gold"), global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_no_gold_upkeep"), null, false, false);
				global::PandoraSingleton<global::HideoutManager>.Instance.messagePopup.HideCancelButton();
			}
		}
	}

	private global::HideoutCamAnchor camAnchor;

	private global::Warband warband;

	private global::WarbandSheetModule warbandSheet;

	private global::WarbandOverviewUnitsModule unitOverviewModule;

	private global::TreasuryModule treasuryModule;

	private global::StatusDescModule statusDescModule;

	private global::WarbandOverviewModule warbandOverviewModule;
}
