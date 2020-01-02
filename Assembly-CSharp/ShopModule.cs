using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ShopModule : global::UIModule
{
	public void Init(global::UnityEngine.Events.UnityAction<global::ShopModuleTab> tabSelected)
	{
		base.Init();
		this.tabSelected = tabSelected;
		this.btnPreviousFilter.SetAction("subfilter", null, 0, false, null, null);
		this.btnPreviousFilter.OnAction(new global::UnityEngine.Events.UnityAction(this.NextFilter), false, true);
		for (int i = 0; i < this.tabs.Count; i++)
		{
			this.tabs[i].onAction.RemoveAllListeners();
			global::ShopModuleTab tab = (global::ShopModuleTab)i;
			this.tabs[i].onAction.AddListener(delegate()
			{
				this.SetTab(tab);
			});
		}
		for (int j = 0; j < this.itemTypesIcons.Count; j++)
		{
			int filterIdx = j;
			this.itemTypesIcons[j].image.onAction.RemoveAllListeners();
			this.itemTypesIcons[j].image.onAction.AddListener(delegate()
			{
				this.selectedFilter = filterIdx;
				tabSelected(this.currentTab);
				this.filterTitle.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_item_type_" + this.itemTypesIcons[this.selectedFilter].itemType.ToLowerString());
			});
		}
	}

	public void SetTab(global::ShopModuleTab idx)
	{
		this.tabs[(int)idx].toggle.isOn = true;
		this.currentTab = idx;
		this.itemRuneList.Clear();
		if (this.tabSelected != null)
		{
			this.tabSelected(idx);
		}
		this.filterTitle.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_item_type_" + this.itemTypesIcons[this.selectedFilter].itemType.ToLowerString());
	}

	public void SetList(global::System.Collections.Generic.List<global::Item> items, global::UnityEngine.Events.UnityAction<global::Item> slotConfirmed, global::UnityEngine.Events.UnityAction<global::Item> slotSelected, bool buy)
	{
		float realtimeSinceStartup = global::UnityEngine.Time.realtimeSinceStartup;
		this.ApplyFilter(items);
		global::UnityEngine.Debug.Log("SHOP " + (global::UnityEngine.Time.realtimeSinceStartup - realtimeSinceStartup));
		realtimeSinceStartup = global::UnityEngine.Time.realtimeSinceStartup;
		if (items.Count <= 0)
		{
			global::ShopModuleTab shopModuleTab = this.currentTab;
			if (shopModuleTab != global::ShopModuleTab.BUY)
			{
				if (shopModuleTab != global::ShopModuleTab.SELL)
				{
					this.emptyMessage.text = string.Empty;
				}
				else
				{
					this.emptyMessage.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("na_item_inventory");
				}
			}
			else
			{
				this.emptyMessage.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("market_sold_out");
			}
			this.emptyMessage.gameObject.SetActive(true);
		}
		else
		{
			this.emptyMessage.gameObject.SetActive(false);
		}
		global::UnityEngine.Debug.Log("SHOP " + (global::UnityEngine.Time.realtimeSinceStartup - realtimeSinceStartup));
		realtimeSinceStartup = global::UnityEngine.Time.realtimeSinceStartup;
		this.itemRuneList.SetList(items, slotConfirmed, slotSelected, false, true, buy, false, true, null);
		global::UnityEngine.Debug.Log("SHOP " + (global::UnityEngine.Time.realtimeSinceStartup - realtimeSinceStartup));
	}

	private void PrevFilter()
	{
		if (global::PandoraSingleton<global::PandoraInput>.Instance.lastInputMode == global::PandoraInput.InputMode.JOYSTICK)
		{
			return;
		}
		if (this.selectedFilter == 0)
		{
			this.selectedFilter = this.itemTypesIcons.Count - 1;
		}
		else
		{
			this.selectedFilter--;
		}
		this.itemTypesIcons[this.selectedFilter].image.SetOn();
		this.itemTypesIcons[this.selectedFilter].image.onAction.Invoke();
		this.filterTitle.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_item_type_" + this.itemTypesIcons[this.selectedFilter].itemType.ToLowerString());
	}

	private void NextFilter()
	{
		if (this.selectedFilter == this.itemTypesIcons.Count - 1)
		{
			this.selectedFilter = 0;
		}
		else
		{
			this.selectedFilter++;
		}
		this.itemTypesIcons[this.selectedFilter].image.SetOn();
		this.itemTypesIcons[this.selectedFilter].image.onAction.Invoke();
		this.filterTitle.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_item_type_" + this.itemTypesIcons[this.selectedFilter].itemType.ToLowerString());
	}

	private void ApplyFilter(global::System.Collections.Generic.List<global::Item> items)
	{
		if (this.itemTypesIcons[this.selectedFilter].itemType != global::ItemTypeId.NONE)
		{
			for (int i = items.Count - 1; i >= 0; i--)
			{
				if (items[i].TypeData.Id != this.itemTypesIcons[this.selectedFilter].itemType)
				{
					global::ItemTypeId itemType = this.itemTypesIcons[this.selectedFilter].itemType;
					switch (itemType)
					{
					case global::ItemTypeId.RECIPE_ENCHANTMENT_NORMAL:
					case global::ItemTypeId.RECIPE_ENCHANTMENT_MASTERY:
					case global::ItemTypeId.CONSUMABLE_MISC:
					case global::ItemTypeId.CONSUMABLE_OUT_COMBAT:
						break;
					default:
						if (itemType == global::ItemTypeId.MELEE_1H)
						{
							if (items[i].TypeData.Id != global::ItemTypeId.SHIELD)
							{
								items.RemoveAt(i);
							}
							goto IL_153;
						}
						if (itemType != global::ItemTypeId.CONSUMABLE_POTIONS)
						{
							items.RemoveAt(i);
							goto IL_153;
						}
						break;
					}
					if (items[i].TypeData.Id != global::ItemTypeId.CONSUMABLE_POTIONS && items[i].TypeData.Id != global::ItemTypeId.CONSUMABLE_OUT_COMBAT && items[i].TypeData.Id != global::ItemTypeId.CONSUMABLE_MISC && items[i].TypeData.Id != global::ItemTypeId.RECIPE_ENCHANTMENT_NORMAL && items[i].TypeData.Id != global::ItemTypeId.RECIPE_ENCHANTMENT_MASTERY)
					{
						items.RemoveAt(i);
					}
				}
				IL_153:;
			}
		}
	}

	private void Update()
	{
		if (global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp("h", 0))
		{
			int tab = (int)((this.currentTab + 1) % global::ShopModuleTab.MAX_VALUE);
			this.SetTab((global::ShopModuleTab)tab);
		}
		else if (global::PandoraSingleton<global::PandoraInput>.Instance.GetNegKeyUp("h", 0))
		{
			int num = this.currentTab - global::ShopModuleTab.SELL;
			num = ((num >= 0) ? num : 1);
			this.SetTab((global::ShopModuleTab)num);
		}
	}

	public void RemoveItem(global::Item item)
	{
		this.itemRuneList.RemoveItem(item);
	}

	public global::System.Collections.Generic.List<global::ToggleEffects> tabs;

	public global::UIInventoryItemRuneList itemRuneList;

	public global::ShopModuleTab currentTab;

	public global::UnityEngine.UI.Text emptyMessage;

	public global::System.Collections.Generic.List<global::ItemTypeTab> itemTypesIcons;

	public global::UnityEngine.UI.Text filterTitle;

	public global::ButtonGroup btnPreviousFilter;

	public global::ButtonGroup btnNextFilter;

	private int selectedFilter;

	private global::UnityEngine.Events.UnityAction<global::ShopModuleTab> tabSelected;
}
