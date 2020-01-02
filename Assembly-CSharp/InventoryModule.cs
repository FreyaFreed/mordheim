using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InventoryModule : global::UIModule
{
	public void Init(global::UnityEngine.Events.UnityAction<global::InventoryModuleTab> tabSelected)
	{
		base.Init();
		this.tabSelected = tabSelected;
		this.btnPreviousFilter.SetAction("subfilter", null, 0, false, null, null);
		this.btnPreviousFilter.OnAction(new global::UnityEngine.Events.UnityAction(this.NextFilter), false, true);
		this.btnPreviousFilter.SetDisabled(true);
		for (int i = 0; i < this.tabs.Count; i++)
		{
			this.tabs[i].onAction.RemoveAllListeners();
			global::InventoryModuleTab tab = (global::InventoryModuleTab)i;
			this.tabs[i].onAction.AddListener(delegate()
			{
				this.SetTab(tab, true);
			});
		}
		for (int j = 0; j < this.itemTypesTabs.Count; j++)
		{
			int filterIdx = j;
			this.itemTypesTabs[j].image.onAction.AddListener(delegate()
			{
				this.SelectFilter(filterIdx);
			});
		}
	}

	public void SelectFilter(int filterIdx)
	{
		this.filterTitle.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_item_type_" + this.itemTypesTabs[filterIdx].itemType.ToLowerString());
		this.itemTypesTabs[filterIdx].image.SetOn();
		this.selectedFilter = filterIdx;
		this.tabSelected(this.currentTab);
	}

	private void NextFilter()
	{
		int num = this.selectedFilter++;
		if (this.selectedFilter == this.itemTypesTabs.Count)
		{
			this.selectedFilter = 0;
		}
		while (num != this.selectedFilter)
		{
			if (this.itemTypesTabs[this.selectedFilter].image.enabled)
			{
				break;
			}
			if (++this.selectedFilter == this.itemTypesTabs.Count)
			{
				this.selectedFilter = 0;
			}
		}
		this.itemTypesTabs[this.selectedFilter].image.onAction.Invoke();
		this.filterTitle.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_item_type_" + this.itemTypesTabs[this.selectedFilter].itemType.ToLowerString());
	}

	public void SetTab(global::InventoryModuleTab idx, bool sendCallback = true)
	{
		this.tabs[(int)idx].toggle.isOn = true;
		this.currentTab = idx;
		this.Clear(false);
		if (this.isFocused && sendCallback)
		{
			this.tabSelected(idx);
		}
		this.filterTitle.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_item_type_" + this.itemTypesTabs[this.selectedFilter].itemType.ToLowerString());
	}

	public void Clear(bool clearFocus = true)
	{
		if (clearFocus)
		{
			this.isFocused = false;
			this.btnPreviousFilter.SetDisabled(true);
		}
		this.itemRuneList.Clear();
	}

	private void SetAvailableFilters(global::UnitSlotId slot)
	{
		switch (slot)
		{
		case global::UnitSlotId.HELMET:
			for (int i = 0; i < this.itemTypesTabs.Count; i++)
			{
				global::ItemTypeId itemType = this.itemTypesTabs[i].itemType;
				this.itemTypesTabs[i].image.enabled = (itemType == global::ItemTypeId.NONE || itemType == global::ItemTypeId.HELMET);
			}
			break;
		case global::UnitSlotId.ARMOR:
			for (int j = 0; j < this.itemTypesTabs.Count; j++)
			{
				global::ItemTypeId itemType2 = this.itemTypesTabs[j].itemType;
				this.itemTypesTabs[j].image.enabled = (itemType2 == global::ItemTypeId.NONE || itemType2 == global::ItemTypeId.CLOTH_ARMOR || itemType2 == global::ItemTypeId.LIGHT_ARMOR || itemType2 == global::ItemTypeId.HEAVY_ARMOR);
			}
			break;
		case global::UnitSlotId.SET1_MAINHAND:
		case global::UnitSlotId.SET2_MAINHAND:
			for (int k = 0; k < this.itemTypesTabs.Count; k++)
			{
				global::ItemTypeId itemType3 = this.itemTypesTabs[k].itemType;
				this.itemTypesTabs[k].image.enabled = (itemType3 == global::ItemTypeId.NONE || itemType3 == global::ItemTypeId.MELEE_1H || itemType3 == global::ItemTypeId.MELEE_2H || itemType3 == global::ItemTypeId.RANGE || itemType3 == global::ItemTypeId.RANGE_FIREARM);
			}
			break;
		case global::UnitSlotId.SET1_OFFHAND:
		case global::UnitSlotId.SET2_OFFHAND:
			for (int l = 0; l < this.itemTypesTabs.Count; l++)
			{
				global::ItemTypeId itemType4 = this.itemTypesTabs[l].itemType;
				this.itemTypesTabs[l].image.enabled = (itemType4 == global::ItemTypeId.NONE || itemType4 == global::ItemTypeId.MELEE_1H || itemType4 == global::ItemTypeId.SHIELD);
			}
			break;
		case global::UnitSlotId.ITEM_1:
		case global::UnitSlotId.ITEM_2:
		case global::UnitSlotId.ITEM_3:
		case global::UnitSlotId.ITEM_4:
		case global::UnitSlotId.ITEM_5:
		case global::UnitSlotId.ITEM_6:
		case global::UnitSlotId.ITEM_7:
			for (int m = 0; m < this.itemTypesTabs.Count; m++)
			{
				global::ItemTypeId itemType5 = this.itemTypesTabs[m].itemType;
				this.itemTypesTabs[m].image.enabled = (itemType5 == global::ItemTypeId.NONE);
			}
			break;
		default:
			for (int n = 0; n < this.itemTypesTabs.Count; n++)
			{
				this.itemTypesTabs[n].image.enabled = true;
			}
			break;
		}
		if (!this.itemTypesTabs[this.selectedFilter].image.enabled)
		{
			this.SelectFilter(0);
		}
	}

	public void SetList(global::System.Collections.Generic.List<global::Item> items, global::UnityEngine.Events.UnityAction<global::Item> slotConfirmed, global::UnityEngine.Events.UnityAction<global::Item> slotSelected, global::UnitSlotId currentSlot, string reason = "", bool slotLocked = false)
	{
		this.isFocused = true;
		this.btnPreviousFilter.SetDisabled(false);
		this.filtersSection.SetActive(true);
		this.SetAvailableFilters(currentSlot);
		if (this.itemTypesTabs[this.selectedFilter].itemType != global::ItemTypeId.NONE)
		{
			for (int i = items.Count - 1; i >= 0; i--)
			{
				if (items[i].TypeData.Id != this.itemTypesTabs[this.selectedFilter].itemType)
				{
					items.RemoveAt(i);
				}
			}
		}
		bool addEmpty = currentSlot != global::UnitSlotId.SET1_MAINHAND && currentSlot != global::UnitSlotId.ARMOR && this.currentTab != global::InventoryModuleTab.SHOP && !slotLocked;
		this.itemRuneList.SetList(items, slotConfirmed, slotSelected, addEmpty, this.currentTab == global::InventoryModuleTab.SHOP, true, false, true, reason);
		this.itemRuneList.SetFocus();
	}

	public void SetList(global::System.Collections.Generic.List<global::RuneMark> runeList, global::System.Collections.Generic.List<global::RuneMark> notAvailableRuneList, global::UnityEngine.Events.UnityAction<global::RuneMark> slotConfirmed, global::UnityEngine.Events.UnityAction<global::RuneMark> slotSelected, string reason = null)
	{
		this.filtersSection.SetActive(false);
		this.isFocused = true;
		this.btnPreviousFilter.SetDisabled(false);
		this.itemRuneList.SetList(runeList, notAvailableRuneList, slotConfirmed, slotSelected, reason);
		this.itemRuneList.SetFocus();
	}

	private void Update()
	{
		if (this.isFocused)
		{
			if (global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp("h", 0))
			{
				int idx = (int)((this.currentTab + 1) % global::InventoryModuleTab.MAX_VALUE);
				this.SetTab((global::InventoryModuleTab)idx, true);
			}
			else if (global::PandoraSingleton<global::PandoraInput>.Instance.GetNegKeyUp("h", 0))
			{
				int num = this.currentTab - global::InventoryModuleTab.SHOP;
				num = ((num >= 0) ? num : 2);
				this.SetTab((global::InventoryModuleTab)num, true);
			}
		}
	}

	public global::System.Collections.Generic.List<global::ToggleEffects> tabs;

	public global::UIInventoryItemRuneList itemRuneList;

	public global::InventoryModuleTab currentTab;

	public global::UnityEngine.GameObject filtersSection;

	public global::System.Collections.Generic.List<global::ItemTypeTab> itemTypesTabs;

	public global::UnityEngine.UI.Text filterTitle;

	public global::ButtonGroup btnPreviousFilter;

	public global::ButtonGroup btnNextFilter;

	private bool isFocused;

	private int selectedFilter;

	private global::UnityEngine.Events.UnityAction<global::InventoryModuleTab> tabSelected;
}
