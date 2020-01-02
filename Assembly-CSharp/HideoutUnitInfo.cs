using System;
using UnityEngine.Events;
using UnityEngine.UI;

public class HideoutUnitInfo : global::BaseHideoutUnitState
{
	public HideoutUnitInfo(global::HideoutManager mng, global::HideoutCamAnchor anchor) : base(anchor, global::HideoutManager.State.UNIT_INFO)
	{
	}

	public override void Enter(int iFrom)
	{
		global::PandoraSingleton<global::HideoutTabManager>.Instance.ActivateRightTabModules(true, new global::ModuleId[]
		{
			global::ModuleId.TREASURY,
			global::ModuleId.UNIT_DESC
		});
		if (global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.unitCtrlrs.Count > 1)
		{
			global::PandoraSingleton<global::HideoutTabManager>.Instance.ActivateCenterTabModules(new global::ModuleId[]
			{
				global::ModuleId.CHARACTER_AREA,
				global::ModuleId.UNIT_TABS,
				global::ModuleId.TITLE,
				global::ModuleId.NEXT_UNIT,
				global::ModuleId.DESC,
				global::ModuleId.UNIT_DESC,
				global::ModuleId.NOTIFICATION
			});
			global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleCenter<global::NextUnitModule>(global::ModuleId.NEXT_UNIT).Setup();
		}
		else
		{
			global::PandoraSingleton<global::HideoutTabManager>.Instance.ActivateCenterTabModules(new global::ModuleId[]
			{
				global::ModuleId.CHARACTER_AREA,
				global::ModuleId.UNIT_TABS,
				global::ModuleId.TITLE,
				global::ModuleId.DESC,
				global::ModuleId.UNIT_DESC,
				global::ModuleId.NOTIFICATION
			});
		}
		base.Enter(iFrom);
		this.unitDescModule = global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleRight<global::UnitDescriptionModule>(global::ModuleId.UNIT_DESC);
		this.statusDescModule = global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleCenter<global::StatusDescModule>(global::ModuleId.UNIT_DESC);
		if (global::PandoraSingleton<global::HideoutManager>.Instance.IsTrainingOutsider)
		{
			global::PandoraSingleton<global::HideoutTabManager>.Instance.ActivateCenterTabModule(false, new global::ModuleId[]
			{
				global::ModuleId.UNIT_DESC
			});
		}
		else
		{
			this.statusDescModule.onPayUpkeep = new global::UnityEngine.Events.UnityAction(this.PayUpkeep);
			this.statusDescModule.onPayTreatment = new global::UnityEngine.Events.UnityAction(this.PayTreatment);
			this.statusDescModule.onFireUnit = new global::UnityEngine.Events.UnityAction(this.FireUnit);
			this.statusDescDisabler = this.statusDescModule.GetComponent<global::CanvasGroupDisabler>();
			this.statusDescDisabler.enabled = true;
		}
		this.descModule = global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleCenter<global::DescriptionModule>(global::ModuleId.DESC);
		this.descModule.gameObject.SetActive(false);
		this.treasuryModule = global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleRight<global::TreasuryModule>(global::ModuleId.TREASURY);
		this.treasuryModule.Refresh(global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.GetWarbandSave());
		this.characterCamModule = global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleCenter<global::CharacterCameraAreaModule>(global::ModuleId.CHARACTER_AREA);
		this.characterCamModule.Init(this.camAnchor.transform.position);
		this.SelectUnit(global::PandoraSingleton<global::HideoutManager>.Instance.currentUnit);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.DeactivateAllButtons();
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button1.SetAction("cancel", "go_to_warband", 0, false, global::PandoraSingleton<global::HideoutTabManager>.Instance.icnBack, null);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button1.OnAction(new global::UnityEngine.Events.UnityAction(base.ReturnToWarband), false, true);
		this.once = true;
	}

	public override void FixedUpdate()
	{
		if (this.once)
		{
			this.once = false;
			global::PandoraSingleton<global::HideoutManager>.Instance.ShowHideoutTuto(global::HideoutManager.HideoutTutoType.UNIT);
		}
	}

	private void Refresh()
	{
		this.statusDescModule.Refresh(this.currentUnit.unit);
		if (global::PandoraSingleton<global::HideoutManager>.Instance.IsTrainingOutsider)
		{
			this.unitDescModule.SetNav(this.statsModule.GetComponentInChildren<global::UIStatIncrease>().gameObject.GetComponent<global::UnityEngine.UI.Toggle>());
			this.statsModule.SetNav(this.statsModule.GetComponentInChildren<global::UIStatIncrease>().gameObject.GetComponent<global::UnityEngine.UI.Toggle>());
			base.RefreshUnitAttributes();
			this.unitDescModule.tabs[0].SetSelected(true);
		}
		else
		{
			this.unitDescModule.SetNav(this.statusDescModule.GetActiveButton());
			this.statsModule.SetNav(this.statusDescModule.GetActiveButton());
			this.statusDescModule.SetNav(this.statsModule.GetComponentInChildren<global::UIStatIncrease>().gameObject.GetComponent<global::UnityEngine.UI.Toggle>(), this.unitDescModule.tabs[0].toggle);
			this.tabMod.Refresh();
			base.RefreshUnitAttributes();
			this.statusDescModule.SetFocus();
		}
	}

	public override void SelectUnit(global::UnitMenuController ctrlr)
	{
		base.SelectUnit(ctrlr);
		this.unitDescModule.Refresh(ctrlr.unit, false);
		this.Refresh();
	}

	public override bool CanIncreaseAttributes()
	{
		return true;
	}

	private void PayTreatment()
	{
		int unitTreatmentCost = global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.GetUnitTreatmentCost(this.currentUnit.unit);
		if (global::PandoraSingleton<global::HideoutManager>.Instance.WarbandChest.GetGold() >= unitTreatmentCost)
		{
			global::PandoraSingleton<global::HideoutManager>.Instance.messagePopup.ShowLocalized(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("popup_pay_unit_treatment_title"), global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("popup_pay_unit_treatment_desc", new string[]
			{
				unitTreatmentCost.ToString(),
				this.currentUnit.unit.Name,
				global::PandoraSingleton<global::HideoutManager>.Instance.WarbandChest.GetGold().ToString()
			}), new global::System.Action<bool>(this.OnPayTreatmentPopup), false, false);
		}
		else
		{
			global::PandoraSingleton<global::HideoutManager>.Instance.messagePopup.Show("hideout_no_gold", "hideout_no_gold_upkeep", null, false, false);
			global::PandoraSingleton<global::HideoutManager>.Instance.messagePopup.HideCancelButton();
		}
	}

	private void OnPayTreatmentPopup(bool confirm)
	{
		if (confirm)
		{
			global::PandoraSingleton<global::HideoutManager>.Instance.WarbandChest.RemoveGold(global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.GetUnitTreatmentCost(this.currentUnit.unit));
			this.treasuryModule.Refresh(global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.GetWarbandSave());
			this.currentUnit.unit.TreatmentPaid();
			global::PandoraSingleton<global::HideoutManager>.Instance.SaveChanges();
			this.Refresh();
		}
	}

	private void PayUpkeep()
	{
		int upkeepOwned = this.currentUnit.unit.GetUpkeepOwned();
		if (global::PandoraSingleton<global::HideoutManager>.Instance.WarbandChest.GetGold() >= upkeepOwned)
		{
			global::PandoraSingleton<global::HideoutManager>.Instance.messagePopup.ShowLocalized(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("popup_pay_unit_upkeep_title"), global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("popup_pay_unit_upkeep_desc", new string[]
			{
				upkeepOwned.ToString(),
				this.currentUnit.unit.Name,
				global::PandoraSingleton<global::HideoutManager>.Instance.WarbandChest.GetGold().ToString()
			}), new global::System.Action<bool>(this.OnPayUpkeepPopup), false, false);
		}
		else
		{
			global::PandoraSingleton<global::HideoutManager>.Instance.messagePopup.Show("hideout_no_gold", "hideout_no_gold_upkeep", null, false, false);
			global::PandoraSingleton<global::HideoutManager>.Instance.messagePopup.HideCancelButton();
		}
	}

	private void OnPayUpkeepPopup(bool confirm)
	{
		if (confirm)
		{
			global::PandoraSingleton<global::HideoutManager>.Instance.WarbandChest.RemoveGold(this.currentUnit.unit.GetUpkeepOwned());
			this.treasuryModule.Refresh(global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.GetWarbandSave());
			this.currentUnit.unit.PayUpkeepOwned();
			global::PandoraSingleton<global::HideoutManager>.Instance.SaveChanges();
			this.Refresh();
		}
	}

	private void FireUnit()
	{
		string key = "popup_fire_unit_desc";
		if (this.currentUnit.unit.IsLeader)
		{
			bool flag = false;
			for (int i = 0; i < global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.Units.Count; i++)
			{
				if (global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.Units[i].IsLeader && global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.Units[i] != this.currentUnit.unit)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				key = "popup_fire_unit_last_leader_desc";
			}
		}
		global::PandoraSingleton<global::HideoutManager>.Instance.messagePopup.ShowLocalized(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("popup_fire_unit_title"), global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(key, new string[]
		{
			this.currentUnit.unit.Name
		}), new global::System.Action<bool>(this.OnFireUnitPopup), false, false);
		global::PandoraSingleton<global::HideoutManager>.Instance.messagePopup.cancelButton.effects.toggle.isOn = true;
	}

	private void OnFireUnitPopup(bool confirm)
	{
		if (confirm)
		{
			for (int i = 0; i < this.currentUnit.unit.Injuries.Count; i++)
			{
				if (this.currentUnit.unit.Injuries[i].Data.Id == global::InjuryId.SEVERED_LEG || this.currentUnit.unit.Injuries[i].Data.Id == global::InjuryId.SEVERED_ARM)
				{
					global::PandoraSingleton<global::Hephaestus>.Instance.UnlockAchievement(global::Hephaestus.TrophyId.INJURED_FIRE);
				}
			}
			global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Disband(this.currentUnit.unit, global::EventLogger.LogEvent.FIRE, 0);
			global::PandoraSingleton<global::HideoutManager>.Instance.SaveChanges();
			base.ReturnToWarband();
		}
	}

	public override global::UnityEngine.UI.Selectable ModuleLeftOnRight()
	{
		if (global::PandoraSingleton<global::HideoutManager>.Instance.IsTrainingOutsider)
		{
			return this.unitDescModule.tabs[0].toggle;
		}
		if (this.statusDescModule.btnAction.isActiveAndEnabled)
		{
			return this.statusDescModule.btnAction.effects.toggle;
		}
		if (this.statusDescModule.btnFire.isActiveAndEnabled)
		{
			return this.statusDescModule.btnFire.effects.toggle;
		}
		return this.unitDescModule.tabs[0].toggle;
	}

	protected override void ShowDescription(string title, string desc)
	{
		base.ShowDescription(title, desc);
		this.statusDescDisabler.enabled = false;
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button1.SetAction("cancel", "go_to_warband", 0, false, global::PandoraSingleton<global::HideoutTabManager>.Instance.icnBack, null);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button1.OnAction(new global::UnityEngine.Events.UnityAction(base.ReturnToWarband), false, true);
		base.SetupAttributeButtons(global::PandoraSingleton<global::HideoutTabManager>.Instance.button2, global::PandoraSingleton<global::HideoutTabManager>.Instance.button3, global::PandoraSingleton<global::HideoutTabManager>.Instance.button4);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button5.gameObject.SetActive(false);
	}

	protected override void HideStatsDesc()
	{
		base.HideStatsDesc();
		this.statusDescDisabler.enabled = true;
		if (global::PandoraSingleton<global::HideoutManager>.Instance.StateMachine.GetActiveStateId() == 15)
		{
			global::PandoraSingleton<global::HideoutTabManager>.Instance.DeactivateAllButtons();
			global::PandoraSingleton<global::HideoutTabManager>.Instance.button1.SetAction("cancel", "go_to_warband", 0, false, global::PandoraSingleton<global::HideoutTabManager>.Instance.icnBack, null);
			global::PandoraSingleton<global::HideoutTabManager>.Instance.button1.OnAction(new global::UnityEngine.Events.UnityAction(base.ReturnToWarband), false, true);
		}
	}

	private global::UnitDescriptionModule unitDescModule;

	private global::StatusDescModule statusDescModule;

	private global::TreasuryModule treasuryModule;

	private global::CanvasGroupDisabler statusDescDisabler;

	private bool once = true;
}
