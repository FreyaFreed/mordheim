using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CampSectionsModule : global::UIModule
{
	public override void Init()
	{
		base.Init();
		this.playerProgressionIcon.sprite = global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.GetDramatis().unit.GetIcon();
		this.warbandTabs = global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleCenter<global::WarbandTabsModule>(global::ModuleId.WARBAND_TABS);
		this.warbandTabs.onIconEnter = new global::System.Action<global::TabIcon>(this.OnTabEnter);
	}

	private void OnTabEnter(global::TabIcon tabIcon)
	{
		if (base.isActiveAndEnabled && !this.selecting && tabIcon.nodeSlot != global::HideoutCamp.NodeSlot.CAMP && this.icons[(int)tabIcon.nodeSlot].toggle.interactable)
		{
			this.icons[(int)tabIcon.nodeSlot].SetOn();
		}
	}

	public void Setup(global::UnityEngine.Events.UnityAction<int> onSelect, global::UnityEngine.Events.UnityAction<int> onUnselect, global::UnityEngine.Events.UnityAction<int> onConfirm)
	{
		this.selecting = false;
		this.onConfirmCallback = onConfirm;
		this.onUnselectCallback = onUnselect;
		this.onSelectCallback = onSelect;
		this.description.gameObject.SetActive(false);
		for (int i = 0; i < this.warbandTabs.icons.Count; i++)
		{
			global::TabIcon tabIcon = this.warbandTabs.icons[i];
			if (tabIcon.nodeSlot != global::HideoutCamp.NodeSlot.CAMP)
			{
				int idx = (int)tabIcon.nodeSlot;
				global::ToggleEffects toggleEffects = this.icons[idx];
				toggleEffects.onSelect.RemoveAllListeners();
				toggleEffects.onSelect.AddListener(delegate()
				{
					this.OnSelect(idx);
				});
				toggleEffects.onUnselect.RemoveAllListeners();
				toggleEffects.onUnselect.AddListener(delegate()
				{
					this.OnUnselect(idx);
				});
				if (tabIcon.available)
				{
					toggleEffects.onAction.RemoveAllListeners();
					toggleEffects.onAction.AddListener(delegate()
					{
						this.OnConfirm(idx);
					});
					toggleEffects.toColor[0].color = global::UnityEngine.Color.white;
					toggleEffects.toggle.image.overrideSprite = null;
				}
				else
				{
					toggleEffects.toggle.image.overrideSprite = this.lockIcon;
					toggleEffects.toColor[0].color = global::UnityEngine.Color.red;
					toggleEffects.onAction.RemoveAllListeners();
				}
			}
		}
		global::Warband warband = global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband;
		bool active = false;
		global::System.Collections.Generic.List<global::UnitMenuController> unitCtrlrs = global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.unitCtrlrs;
		for (int j = 0; j < unitCtrlrs.Count; j++)
		{
			if (unitCtrlrs[j].unit.UnspentSkill > 0)
			{
				active = true;
			}
		}
		this.unspentSkillPoints.gameObject.SetActive(active);
		this.unspentVeteranPoints.gameObject.SetActive(warband.GetPlayerSkillsAvailablePoints() > 0);
		if (warband.GetTotalTreatmentOwned() > 0)
		{
			this.unpaidUnits.SetActive(true);
			this.unpaidUnitsCost.text = warband.GetTotalTreatmentOwned().ToString();
		}
		else if (warband.GetTotalUpkeepOwned() > 0)
		{
			this.unpaidUnits.SetActive(true);
			this.unpaidUnitsCost.text = warband.GetTotalUpkeepOwned().ToString();
		}
		else
		{
			this.unpaidUnits.SetActive(false);
		}
		global::Tuple<int, global::EventLogger.LogEvent, int> tuple = warband.Logger.FindLastEvent(global::EventLogger.LogEvent.SHIPMENT_LATE);
		if (tuple != null && tuple.Item1 > global::PandoraSingleton<global::HideoutManager>.Instance.Date.CurrentDate)
		{
			this.deliveryDue.SetActive(true);
			this.deliveryDueText.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_days_left", new string[]
			{
				(tuple.Item1 - global::PandoraSingleton<global::HideoutManager>.Instance.Date.CurrentDate).ToString()
			});
		}
		else if (tuple != null && tuple.Item1 == global::PandoraSingleton<global::HideoutManager>.Instance.Date.CurrentDate)
		{
			this.deliveryDue.SetActive(true);
			this.deliveryDueText.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_no_days_left");
		}
		else
		{
			this.deliveryDue.SetActive(false);
		}
	}

	public void Refresh(int selectedIdx, bool selected = true)
	{
		this.icons[selectedIdx].toggle.isOn = selected;
	}

	public void OnSelect(int idx)
	{
		if (this.onSelectCallback != null)
		{
			this.onSelectCallback(idx);
		}
		global::TabIcon tabIcon = this.warbandTabs.GetTabIcon((global::HideoutCamp.NodeSlot)idx);
		if (!tabIcon.available)
		{
			this.description.gameObject.SetActive(true);
			this.description.Set(tabIcon.titleText, tabIcon.reason + "_camp");
		}
		else
		{
			this.description.gameObject.SetActive(false);
		}
		this.selecting = true;
		this.warbandTabs.OnTabIconEnter(tabIcon);
		this.selecting = false;
	}

	public void OnUnselect(int idx)
	{
		if (this.onUnselectCallback != null)
		{
			this.onUnselectCallback(idx);
		}
		this.warbandTabs.OnTabIconExit(this.warbandTabs.GetTabIcon((global::HideoutCamp.NodeSlot)idx));
	}

	public void OnConfirm(int idx)
	{
		if (this.onConfirmCallback != null)
		{
			this.onConfirmCallback(idx);
		}
	}

	private bool selecting;

	public global::UnityEngine.Sprite lockIcon;

	public global::UnityEngine.Sprite redSplatter;

	public global::System.Collections.Generic.List<global::ToggleEffects> icons;

	public global::UnityEngine.UI.Image playerProgressionIcon;

	public global::UIDescription description;

	public global::UnityEngine.GameObject unspentSkillPoints;

	public global::UnityEngine.GameObject unspentVeteranPoints;

	public global::UnityEngine.GameObject unpaidUnits;

	public global::UnityEngine.UI.Text unpaidUnitsCost;

	public global::UnityEngine.GameObject deliveryDue;

	public global::UnityEngine.UI.Text deliveryDueText;

	private global::WarbandTabsModule warbandTabs;

	private global::UnityEngine.Events.UnityAction<int> onSelectCallback;

	private global::UnityEngine.Events.UnityAction<int> onUnselectCallback;

	private global::UnityEngine.Events.UnityAction<int> onConfirmCallback;
}
