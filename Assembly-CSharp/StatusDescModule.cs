using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class StatusDescModule : global::UIModule
{
	public void Refresh(global::Unit unit)
	{
		int @int = global::Constant.GetInt(global::ConstantId.UPKEEP_DAYS_WITHOUT_PAY);
		int num = 0;
		global::Tuple<int, global::EventLogger.LogEvent, int> tuple = unit.Logger.FindLastEvent(global::EventLogger.LogEvent.NO_TREATMENT);
		if (tuple != null && tuple.Item1 > global::PandoraSingleton<global::HideoutManager>.Instance.Date.CurrentDate)
		{
			num = tuple.Item1 - global::PandoraSingleton<global::HideoutManager>.Instance.Date.CurrentDate;
		}
		this.btnFire.SetAction(string.Empty, "hideout_btn_fire_unit", 0, false, null, null);
		this.btnFire.OnAction(this.onFireUnit, false, true);
		switch (unit.GetActiveStatus())
		{
		case global::UnitActiveStatusId.AVAILABLE:
			this.statusImage.sprite = this.availableIcon;
			this.statusTitle.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("unit_status_name_available");
			this.statusText.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("unit_status_desc_available");
			this.btnAction.SetAction(string.Empty, "hideout_pay", 0, false, null, null);
			this.btnAction.SetDisabled(true);
			this.costImage.gameObject.SetActive(false);
			this.statusCost.text = string.Empty;
			this.btnFire.SetDisabled(false);
			break;
		case global::UnitActiveStatusId.INJURED:
			this.statusImage.sprite = this.injuredIcon;
			this.statusTitle.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("unit_status_name_injured");
			this.statusText.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("unit_status_desc_injured", new string[]
			{
				unit.UnitSave.injuredTime.ToString()
			});
			this.btnAction.SetAction(string.Empty, "hideout_pay", 0, false, null, null);
			this.btnAction.SetDisabled(true);
			this.costImage.gameObject.SetActive(false);
			this.statusCost.text = string.Empty;
			this.btnFire.SetDisabled(false);
			break;
		case global::UnitActiveStatusId.UPKEEP_NOT_PAID:
			this.statusImage.sprite = this.upkeepIcon;
			this.statusTitle.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("unit_status_name_upkeep_not_paid");
			this.statusText.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("unit_status_desc_upkeep_not_paid", new string[]
			{
				unit.GetUpkeepOwned().ToString(),
				(@int - unit.GetUpkeepMissedDays()).ToString()
			});
			this.btnAction.SetDisabled(false);
			this.costImage.gameObject.SetActive(true);
			this.statusCost.text = unit.GetUpkeepOwned().ToString();
			this.btnAction.SetAction(string.Empty, "hideout_pay", 0, false, null, null);
			this.btnAction.OnAction(this.onPayUpkeep, false, true);
			this.btnAction.effects.enabled = true;
			this.btnFire.SetDisabled(true);
			break;
		case global::UnitActiveStatusId.IN_TRAINING:
			this.statusImage.sprite = this.trainingIcon;
			this.statusTitle.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("unit_status_name_in_training");
			this.statusText.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("unit_status_desc_in_training", new string[]
			{
				unit.UnitSave.trainingTime.ToString()
			});
			this.btnAction.SetAction(string.Empty, "hideout_pay", 0, false, null, null);
			this.btnAction.SetDisabled(true);
			this.costImage.gameObject.SetActive(false);
			this.btnFire.SetDisabled(true);
			this.statusCost.text = string.Empty;
			break;
		case global::UnitActiveStatusId.TREATMENT_NOT_PAID:
			this.statusImage.sprite = this.treatmentIcon;
			this.statusTitle.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("unit_status_name_treatment_not_paid");
			this.statusText.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("unit_status_desc_treatment_not_paid", new string[]
			{
				global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.GetUnitTreatmentCost(unit).ToString(),
				num.ToString()
			});
			this.btnAction.SetDisabled(false);
			this.costImage.gameObject.SetActive(true);
			this.statusCost.text = global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.GetUnitTreatmentCost(unit).ToString();
			this.btnAction.SetAction(string.Empty, "hideout_pay_treatment", 0, false, null, null);
			this.btnAction.OnAction(this.onPayTreatment, false, true);
			this.btnAction.effects.enabled = true;
			this.btnFire.SetDisabled(true);
			break;
		case global::UnitActiveStatusId.INJURED_AND_UPKEEP_NOT_PAID:
			this.statusImage.sprite = this.upkeepAndTreatmentIcon;
			this.statusTitle.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("unit_status_name_injured_and_upkeep_not_paid");
			this.statusText.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("unit_status_desc_injured_and_upkeep_not_paid", new string[]
			{
				unit.UnitSave.injuredTime.ToString(),
				unit.GetUpkeepOwned().ToString(),
				(@int - unit.GetUpkeepMissedDays()).ToString()
			});
			this.btnAction.SetDisabled(false);
			this.costImage.gameObject.SetActive(true);
			this.statusCost.text = unit.GetUpkeepOwned().ToString();
			this.btnAction.SetAction(string.Empty, "hideout_pay", 0, false, null, null);
			this.btnAction.OnAction(this.onPayUpkeep, false, true);
			this.btnAction.effects.enabled = true;
			this.btnFire.SetDisabled(true);
			break;
		}
		if (global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.GetWarbandSave().lateShipmentCount >= global::Constant.GetInt(global::ConstantId.MAX_SHIPMENT_FAIL))
		{
			this.btnAction.gameObject.SetActive(false);
			this.btnFire.gameObject.SetActive(false);
		}
	}

	public void Refresh(global::Warband wb)
	{
		this.btnFire.SetAction(string.Empty, "hideout_disband", 0, false, null, null);
		this.btnFire.OnAction(this.onFireUnit, false, true);
		int totalUpkeepOwned = wb.GetTotalUpkeepOwned();
		int totalTreatmentOwned = wb.GetTotalTreatmentOwned();
		if (totalTreatmentOwned > 0)
		{
			this.statusImage.sprite = this.treatmentIcon;
			this.statusTitle.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("warband_status_name_treatment_required");
			this.statusText.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("warband_status_desc_treatment_required", new string[]
			{
				totalTreatmentOwned.ToString()
			});
			this.btnFire.SetDisabled(true);
			this.btnAction.SetDisabled(false);
			this.costImage.gameObject.SetActive(true);
			this.statusCost.text = totalTreatmentOwned.ToString();
			this.btnAction.SetAction(string.Empty, "hideout_pay_treatment", 0, false, null, null);
			this.btnAction.OnAction(this.onPayTreatment, false, true);
			this.btnAction.effects.enabled = true;
		}
		else if (totalUpkeepOwned > 0)
		{
			this.statusImage.sprite = this.upkeepIcon;
			this.statusTitle.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("warband_status_name_upkeep_not_paid");
			this.statusText.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("warband_status_desc_upkeep_not_paid", new string[]
			{
				totalUpkeepOwned.ToString()
			});
			this.btnFire.SetDisabled(true);
			this.btnAction.SetDisabled(false);
			this.costImage.gameObject.SetActive(true);
			this.statusCost.text = totalUpkeepOwned.ToString();
			this.btnAction.SetAction(string.Empty, "hideout_pay", 0, false, null, null);
			this.btnAction.OnAction(this.onPayUpkeep, false, true);
			this.btnAction.effects.enabled = true;
		}
		else
		{
			this.statusImage.sprite = this.availableIcon;
			this.statusTitle.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("warband_status_name_normal");
			this.statusText.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("warband_status_desc_normal");
			this.btnAction.SetAction(string.Empty, "hideout_pay", 0, false, null, null);
			this.btnAction.SetDisabled(true);
			this.costImage.gameObject.SetActive(false);
			this.statusCost.text = string.Empty;
			this.btnFire.SetDisabled(false);
		}
		if (global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.GetWarbandSave().lateShipmentCount >= global::Constant.GetInt(global::ConstantId.MAX_SHIPMENT_FAIL))
		{
			this.btnAction.gameObject.SetActive(false);
			this.btnFire.gameObject.SetActive(false);
		}
	}

	public void SetFocus()
	{
		if (this.btnAction.IsInteractable())
		{
			this.btnAction.SetSelected(true);
		}
		else
		{
			this.btnFire.SetSelected(true);
		}
	}

	public global::UnityEngine.UI.Selectable GetActiveButton()
	{
		if (this.btnAction.IsInteractable())
		{
			return this.btnAction.effects.toggle;
		}
		return this.btnFire.effects.toggle;
	}

	private void OnDisable()
	{
		this.btnAction.effects.toggle.isOn = false;
		this.btnFire.effects.toggle.isOn = false;
	}

	private void Update()
	{
	}

	public void SetNav(global::UnityEngine.UI.Selectable left, global::UnityEngine.UI.Selectable right)
	{
		if (this.btnFire.IsInteractable())
		{
			global::UnityEngine.UI.Navigation navigation = this.btnFire.effects.toggle.navigation;
			navigation.mode = global::UnityEngine.UI.Navigation.Mode.Explicit;
			navigation.selectOnLeft = left;
			navigation.selectOnRight = right;
			navigation.selectOnUp = null;
			navigation.selectOnDown = ((!this.btnAction.IsInteractable()) ? null : this.btnAction.effects.toggle);
			this.btnFire.effects.toggle.navigation = navigation;
		}
		if (this.btnAction.IsInteractable())
		{
			global::UnityEngine.UI.Navigation navigation2 = this.btnAction.effects.toggle.navigation;
			navigation2.mode = global::UnityEngine.UI.Navigation.Mode.Explicit;
			navigation2.selectOnLeft = left;
			navigation2.selectOnRight = right;
			navigation2.selectOnDown = null;
			navigation2.selectOnUp = ((!this.btnFire.IsInteractable()) ? null : this.btnFire.effects.toggle);
			this.btnAction.effects.toggle.navigation = navigation2;
		}
	}

	public global::UnityEngine.UI.Image statusImage;

	public global::UnityEngine.UI.Text statusTitle;

	public global::UnityEngine.UI.Text statusText;

	public global::UnityEngine.UI.Text statusCost;

	public global::UnityEngine.UI.Image costImage;

	public global::ButtonGroup btnAction;

	public global::ButtonGroup btnFire;

	public global::UnityEngine.Sprite upkeepAndTreatmentIcon;

	public global::UnityEngine.Sprite upkeepIcon;

	public global::UnityEngine.Sprite treatmentIcon;

	public global::UnityEngine.Sprite injuredIcon;

	public global::UnityEngine.Sprite trainingIcon;

	public global::UnityEngine.Sprite availableIcon;

	public global::UnityEngine.Events.UnityAction onPayUpkeep;

	public global::UnityEngine.Events.UnityAction onPayTreatment;

	public global::UnityEngine.Events.UnityAction onFireUnit;
}
