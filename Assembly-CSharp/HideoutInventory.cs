using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HideoutInventory : global::BaseHideoutUnitState
{
	public HideoutInventory(global::HideoutManager mng, global::HideoutCamAnchor anchor) : base(anchor, global::HideoutManager.State.INVENTORY)
	{
	}

	public override void Enter(int iFrom)
	{
		this.warband = global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband;
		global::PandoraSingleton<global::HideoutTabManager>.Instance.ActivateRightTabModules(true, new global::ModuleId[]
		{
			global::ModuleId.TREASURY,
			global::ModuleId.INVENTORY
		});
		if (global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.unitCtrlrs.Count > 1)
		{
			global::PandoraSingleton<global::HideoutTabManager>.Instance.ActivateCenterTabModules(new global::ModuleId[]
			{
				global::ModuleId.WHEEL,
				global::ModuleId.ITEM_DESC,
				global::ModuleId.DESC,
				global::ModuleId.NEXT_UNIT,
				global::ModuleId.TITLE,
				global::ModuleId.UNIT_TABS,
				global::ModuleId.CHARACTER_AREA,
				global::ModuleId.NOTIFICATION
			});
			global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleCenter<global::NextUnitModule>(global::ModuleId.NEXT_UNIT).Setup();
		}
		else
		{
			global::PandoraSingleton<global::HideoutTabManager>.Instance.ActivateCenterTabModules(new global::ModuleId[]
			{
				global::ModuleId.WHEEL,
				global::ModuleId.ITEM_DESC,
				global::ModuleId.DESC,
				global::ModuleId.TITLE,
				global::ModuleId.UNIT_TABS,
				global::ModuleId.CHARACTER_AREA,
				global::ModuleId.NOTIFICATION
			});
		}
		base.Enter(iFrom);
		this.treasuryMod = global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleRight<global::TreasuryModule>(global::ModuleId.TREASURY);
		this.treasuryMod.Refresh(global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.GetWarbandSave());
		this.inventoryModule = global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleRight<global::InventoryModule>(global::ModuleId.INVENTORY);
		this.inventoryModule.Init(new global::UnityEngine.Events.UnityAction<global::InventoryModuleTab>(this.OnInventoryTabChanged));
		this.wheelModule = global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleCenter<global::WheelModule>(global::ModuleId.WHEEL);
		this.wheelModule.Activate(base.ModuleCentertOnLeft(), new global::UnityEngine.Events.UnityAction<global::UnitSlotId>(this.OnWheelSlotSelected), new global::UnityEngine.Events.UnityAction<int>(this.OnWheelMutationSlotSelected), new global::UnityEngine.Events.UnityAction<global::UnitSlotId>(this.OnWheelSlotConfirmed), new global::UnityEngine.Events.UnityAction<int>(this.OnWheelMutationSlotConfirmed));
		this.itemDescModule = global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleCenter<global::ItemDescModule>(global::ModuleId.ITEM_DESC);
		this.descModule = global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleCenter<global::DescriptionModule>(global::ModuleId.DESC);
		this.descModule.gameObject.SetActive(false);
		this.characterCamModule = global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleCenter<global::CharacterCameraAreaModule>(global::ModuleId.CHARACTER_AREA);
		this.characterCamModule.Init(this.camAnchor.transform.position);
		this.currentRune = null;
		this.tempItem = null;
		this.SelectUnit(global::PandoraSingleton<global::HideoutManager>.Instance.currentUnit);
		this.inventoryModule.SetTab(global::InventoryModuleTab.INVENTORY, false);
		this.inventoryModule.Clear(true);
		this.wheelModule.itemSlots[2].SetSelected(true);
		this.SetButtonsForWheelSelection();
	}

	public override void Exit(int iTo)
	{
		base.Exit(iTo);
		this.inventoryModule.Clear(true);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleCenter<global::WheelModule>(global::ModuleId.WHEEL).Deactivate();
	}

	private void OnUnitChanged()
	{
		this.SelectUnit(global::PandoraSingleton<global::HideoutManager>.Instance.currentUnit);
	}

	public override void SelectUnit(global::UnitMenuController ctrlr)
	{
		base.SelectUnit(ctrlr);
		this.wheelSelectedSlotId = global::UnitSlotId.SET1_MAINHAND;
		this.wheelModule.itemSlots[2].SetSelected(true);
		this.RefreshStatsAndSlots();
		this.OnWheelSlotConfirmed(this.wheelSelectedSlotId);
	}

	public override global::UnityEngine.UI.Selectable ModuleLeftOnRight()
	{
		return this.wheelModule.itemSlots[0].slot.toggle;
	}

	private void OnWheelSlotSelected(global::UnitSlotId slotId)
	{
		this.SetSlotDescription(slotId);
	}

	private void OnWheelSlotConfirmed(global::UnitSlotId slotId)
	{
		global::PandoraDebug.LogDebug("Wheel slot selected " + slotId, "uncategorised", null);
		if ((slotId == global::UnitSlotId.SET1_OFFHAND || slotId == global::UnitSlotId.SET2_OFFHAND) && (global::PandoraSingleton<global::HideoutManager>.Instance.currentUnit.unit.Items[slotId - global::UnitSlotId.ARMOR].IsPaired || global::PandoraSingleton<global::HideoutManager>.Instance.currentUnit.unit.Items[slotId - global::UnitSlotId.ARMOR].IsTwoHanded))
		{
			this.inventoryModule.Clear(true);
			this.wheelSelectedSlotId = global::UnitSlotId.NB_SLOTS;
		}
		else
		{
			this.wheelSelectedSlotId = slotId;
			this.inventoryModule.SetTab(global::InventoryModuleTab.INVENTORY, false);
			this.SetActiveSlot(true);
		}
		if (global::UnityEngine.Input.mousePresent)
		{
			global::PandoraSingleton<global::HideoutTabManager>.Instance.button1.SetAction("cancel", "go_to_warband", 0, false, global::PandoraSingleton<global::HideoutTabManager>.Instance.icnBack, null);
			global::PandoraSingleton<global::HideoutTabManager>.Instance.button1.OnAction(new global::UnityEngine.Events.UnityAction(base.ReturnToWarband), true, true);
		}
		else
		{
			global::PandoraSingleton<global::HideoutTabManager>.Instance.button1.gameObject.SetActive(false);
		}
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button2.SetAction("cancel", "menu_return_select_slot", 0, false, null, null);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button2.OnAction(new global::UnityEngine.Events.UnityAction(this.ReturnToWheelSlotSelection), false, true);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button3.SetAction("action", "menu_equip", 0, false, null, null);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button3.OnAction(null, false, true);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button4.gameObject.SetActive(false);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button5.gameObject.SetActive(false);
	}

	private void OnWheelMutationSlotSelected(int mutationIdx)
	{
		if (mutationIdx == -1)
		{
			return;
		}
		global::PandoraDebug.LogInfo("Wheel mutation slot confirmed (unitsMutationIdx : " + mutationIdx + ")", "uncategorised", null);
		this.inventoryModule.Clear(true);
		this.wheelSelectedSlotId = global::UnitSlotId.NB_SLOTS;
		this.descModule.gameObject.SetActive(false);
		this.itemDescModule.gameObject.SetActive(true);
		this.itemDescModule.SetMutation(global::PandoraSingleton<global::HideoutManager>.Instance.currentUnit.unit.Mutations[mutationIdx]);
	}

	private void OnWheelMutationSlotConfirmed(int mutationIdx)
	{
		this.OnWheelMutationSlotSelected(mutationIdx);
		if (mutationIdx != -1)
		{
			this.SetButtonsForWheelSelection();
		}
	}

	private void ReturnToWheelSlotSelection()
	{
		this.wheelModule.Unlock();
		this.itemDescModule.HideDesc(1);
		this.inventoryModule.Clear(true);
		if (this.wheelSelectedSlotId != global::UnitSlotId.NONE && this.wheelSelectedSlotId < global::UnitSlotId.NB_SLOTS)
		{
			this.wheelModule.itemSlots[(int)this.wheelSelectedSlotId].SetSelected(true);
		}
		else
		{
			this.wheelModule.itemSlots[2].SetSelected(true);
		}
		this.SetButtonsForWheelSelection();
	}

	private void OnInventoryTabChanged(global::InventoryModuleTab tab)
	{
		this.SetActiveSlot(true);
	}

	private void OnInventorySlotConfirmed(global::Item item)
	{
		if (this.wheelSelectedSlotId == global::UnitSlotId.NB_SLOTS)
		{
			return;
		}
		global::System.Collections.Generic.List<global::ItemConsumableData> list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::ItemConsumableData>(new string[]
		{
			"fk_item_id",
			"fk_item_quality_id"
		}, new string[]
		{
			((int)item.Id).ToString(),
			((int)item.QualityData.Id).ToString()
		});
		this.tempItem = item;
		if (this.inventoryModule.currentTab == global::InventoryModuleTab.SHOP)
		{
			if (this.warband.GetItemBuyPrice(this.tempItem) > global::PandoraSingleton<global::HideoutManager>.Instance.WarbandChest.GetGold())
			{
				global::PandoraSingleton<global::HideoutManager>.Instance.messagePopup.ShowLocalized(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_no_gold"), global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_no_gold_item"), null, false, false);
				global::PandoraSingleton<global::HideoutManager>.Instance.messagePopup.HideCancelButton();
				return;
			}
			global::PandoraSingleton<global::HideoutManager>.Instance.messagePopup.ShowLocalized(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_buy_confirm_title"), global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_buy_confirm_desc", new string[]
			{
				this.tempItem.LocalizedName,
				this.warband.GetItemBuyPrice(this.tempItem).ToString()
			}), new global::System.Action<bool>(this.OnInventoryItemPopupConfirmed), false, false);
		}
		else if (list != null && list.Count > 0 && list[0].OutOfCombat)
		{
			global::PandoraSingleton<global::HideoutManager>.Instance.messagePopup.ShowLocalized(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_consumable_confirm_title"), global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_consumable_confirm_desc", new string[]
			{
				this.tempItem.LocalizedName
			}), new global::System.Action<bool>(this.OnInventoryItemPopupConfirmed), false, false);
		}
		else
		{
			this.OnInventoryItemPopupConfirmed(true);
		}
	}

	private void OnInventoryItemPopupConfirmed(bool confirm)
	{
		if (confirm)
		{
			global::ItemSave itemSave;
			if (this.tempItem.Id == global::ItemId.NONE)
			{
				itemSave = new global::ItemSave(global::ItemId.NONE, global::ItemQualityId.NORMAL, global::RuneMarkId.NONE, global::RuneMarkQualityId.NONE, global::AllegianceId.NONE, 1);
			}
			else if (this.inventoryModule.currentTab == global::InventoryModuleTab.SHOP)
			{
				itemSave = global::PandoraSingleton<global::HideoutManager>.Instance.Market.PopItem(this.tempItem.Save, 1);
				int itemBuyPrice = this.warband.GetItemBuyPrice(this.tempItem);
				this.Pay(itemBuyPrice);
				this.warband.AddToAttribute(global::WarbandAttributeId.BUY_AMOUNT, itemBuyPrice);
			}
			else
			{
				itemSave = global::PandoraSingleton<global::HideoutManager>.Instance.WarbandChest.PopItem(this.tempItem.Save, 1);
			}
			global::System.Collections.Generic.List<global::ItemSave> list = global::PandoraSingleton<global::HideoutManager>.Instance.currentUnit.EquipItem(this.wheelSelectedSlotId, itemSave);
			global::PandoraSingleton<global::HideoutManager>.Instance.currentUnit.unit.CheckItemsAchievments();
			global::System.Collections.Generic.List<global::Item> list2 = new global::System.Collections.Generic.List<global::Item>();
			global::PandoraSingleton<global::HideoutManager>.Instance.currentUnit.unit.UpdateAttributesAndCheckBackPack(list2);
			global::PandoraSingleton<global::HideoutManager>.Instance.WarbandChest.AddItems(list2);
			global::PandoraSingleton<global::GameManager>.Instance.Profile.CheckEquipAchievement(this.currentUnit.unit, this.wheelSelectedSlotId);
			for (int i = 0; i < list.Count; i++)
			{
				global::PandoraSingleton<global::HideoutManager>.Instance.WarbandChest.AddItem(list[i], false);
			}
			this.SaveChanges();
			global::PandoraSingleton<global::HideoutManager>.Instance.currentUnit.unit.UpdateAttributes();
			this.RefreshStatsAndSlots();
			this.ReturnToWheelSlotSelection();
		}
		this.tempItem = null;
	}

	private void OnInventorySlotSelected(global::Item item)
	{
		this.descModule.gameObject.SetActive(false);
		this.itemDescModule.gameObject.SetActive(true);
		if (item.Id == global::ItemId.NONE)
		{
			this.itemDescModule.HideDesc(1);
			if (this.currentUnit.unit.Items[(int)this.wheelSelectedSlotId].RuneMark != null)
			{
				this.itemDescModule.SetRune(this.currentUnit.unit.Items[(int)this.wheelSelectedSlotId].RuneMark);
			}
		}
		else
		{
			this.itemDescModule.SetItem(item, this.wheelSelectedSlotId, 1);
		}
	}

	private void OnInventorySlotConfirmed(global::RuneMark rune)
	{
		if (this.wheelSelectedSlotId == global::UnitSlotId.NB_SLOTS)
		{
			return;
		}
		if (this.warband.GetRuneMarkBuyPrice(rune) > global::PandoraSingleton<global::HideoutManager>.Instance.WarbandChest.GetGold())
		{
			global::PandoraSingleton<global::HideoutManager>.Instance.messagePopup.ShowLocalized(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_no_gold"), global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_no_gold_rune", new string[]
			{
				rune.FullLocName
			}), null, false, false);
			global::PandoraSingleton<global::HideoutManager>.Instance.messagePopup.HideCancelButton();
			return;
		}
		this.currentRune = rune;
		global::UnitMenuController currentUnit = global::PandoraSingleton<global::HideoutManager>.Instance.currentUnit;
		global::PandoraSingleton<global::HideoutManager>.Instance.messagePopup.ShowLocalized(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_rune_confirm_title"), global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_rune_confirm_desc", new string[]
		{
			rune.FullLocName,
			currentUnit.unit.Items[(int)this.wheelSelectedSlotId].LocalizedName
		}), new global::System.Action<bool>(this.OnInventoryRunePopupConfirmed), false, false);
	}

	private void OnInventoryRunePopupConfirmed(bool isConfirm)
	{
		if (isConfirm)
		{
			global::UnitMenuController currentUnit = global::PandoraSingleton<global::HideoutManager>.Instance.currentUnit;
			this.Pay(this.warband.GetRuneMarkBuyPrice(this.currentRune));
			currentUnit.unit.Items[(int)this.wheelSelectedSlotId].AddRuneMark(this.currentRune.Data.Id, this.currentRune.QualityData.Id, currentUnit.unit.AllegianceId);
			currentUnit.unit.CheckItemsAchievments();
			global::System.Collections.Generic.List<global::Item> list = new global::System.Collections.Generic.List<global::Item>();
			global::PandoraSingleton<global::HideoutManager>.Instance.currentUnit.unit.UpdateAttributesAndCheckBackPack(list);
			global::PandoraSingleton<global::HideoutManager>.Instance.WarbandChest.AddItems(list);
			this.SaveChanges();
			global::PandoraSingleton<global::GameManager>.Instance.Profile.CheckEquipAchievement(this.currentUnit.unit, this.wheelSelectedSlotId);
			this.RefreshStatsAndSlots();
			this.ReturnToWheelSlotSelection();
		}
		this.currentRune = null;
	}

	private void OnInventorySlotSelected(global::RuneMark rune)
	{
		this.descModule.gameObject.SetActive(false);
		this.itemDescModule.gameObject.SetActive(true);
		this.itemDescModule.SetItem(global::PandoraSingleton<global::HideoutManager>.Instance.currentUnit.unit.Items[(int)this.wheelSelectedSlotId], this.wheelSelectedSlotId, 0);
		if (rune == null)
		{
			this.itemDescModule.HideDesc(1);
			if (this.currentUnit.unit.Items[(int)this.wheelSelectedSlotId].RuneMark != null)
			{
				this.itemDescModule.SetRune(this.currentUnit.unit.Items[(int)this.wheelSelectedSlotId].RuneMark);
			}
		}
		else
		{
			this.itemDescModule.SetRune(rune);
		}
	}

	private void SetButtonsForWheelSelection()
	{
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button1.SetAction("cancel", "go_to_warband", 0, false, global::PandoraSingleton<global::HideoutTabManager>.Instance.icnBack, null);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button1.OnAction(new global::UnityEngine.Events.UnityAction(base.ReturnToWarband), false, true);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button2.SetAction("action", "menu_select_slot", 0, false, null, null);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button2.OnAction(null, false, true);
		base.SetupApplyButton(global::PandoraSingleton<global::HideoutTabManager>.Instance.button3);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button4.gameObject.SetActive(false);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button5.gameObject.SetActive(false);
	}

	private void RefreshStatsAndSlots()
	{
		base.RefreshUnitAttributes();
		this.SetActiveSlot(false);
		this.wheelModule.RefreshSlots(this.currentUnit);
	}

	private void SetSlotDescription(global::UnitSlotId slotId)
	{
		this.descModule.gameObject.SetActive(false);
		this.itemDescModule.gameObject.SetActive(true);
		if (this.currentUnit != null && slotId < (global::UnitSlotId)this.currentUnit.unit.Items.Count && this.currentUnit.unit.Items[(int)slotId].Id != global::ItemId.NONE)
		{
			this.itemDescModule.SetItem(this.currentUnit.unit.Items[(int)slotId], slotId, 0);
		}
	}

	private void SetActiveSlot(bool setList)
	{
		if (this.wheelSelectedSlotId == global::UnitSlotId.NB_SLOTS || this.wheelSelectedSlotId >= (global::UnitSlotId)this.currentUnit.unit.Items.Count)
		{
			this.wheelSelectedSlotId = global::UnitSlotId.SET1_MAINHAND;
		}
		this.descModule.gameObject.SetActive(false);
		this.itemDescModule.gameObject.SetActive(true);
		this.itemDescModule.HideDesc(1);
		bool flag = this.wheelSelectedSlotId == global::UnitSlotId.SET2_MAINHAND || this.wheelSelectedSlotId == global::UnitSlotId.SET2_OFFHAND;
		global::UnitSlotId nextWeaponSlot = (!flag) ? global::UnitSlotId.SET1_MAINHAND : global::UnitSlotId.SET2_MAINHAND;
		this.currentUnit.SwitchWeapons(nextWeaponSlot);
		base.RefreshUnitAttributes();
		global::UnitSlotId slotId = this.wheelSelectedSlotId;
		if (this.currentUnit.unit.Items[(int)this.wheelSelectedSlotId].Id != global::ItemId.NONE)
		{
			this.itemDescModule.SetItem(this.currentUnit.unit.Items[(int)this.wheelSelectedSlotId], this.wheelSelectedSlotId, 0);
		}
		else
		{
			this.itemDescModule.HideDesc(0);
		}
		global::UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
		if (setList)
		{
			switch (this.inventoryModule.currentTab)
			{
			case global::InventoryModuleTab.INVENTORY:
			{
				global::System.Collections.Generic.List<global::ItemSave> items = global::PandoraSingleton<global::HideoutManager>.Instance.WarbandChest.GetItems(this.currentUnit.unit, slotId);
				string reason;
				if (items.Count > 0)
				{
					reason = string.Empty;
				}
				else
				{
					reason = "na_item_inventory";
				}
				this.SetInventoryModuleItems(items, slotId, reason);
				this.wheelModule.Lock();
				break;
			}
			case global::InventoryModuleTab.SHOP:
			{
				global::System.Collections.Generic.List<global::ItemSave> items2 = global::PandoraSingleton<global::HideoutManager>.Instance.Market.GetItems(this.currentUnit.unit, slotId);
				string reason2;
				if (items2.Count > 0)
				{
					reason2 = string.Empty;
				}
				else
				{
					reason2 = "na_item_store";
				}
				this.SetInventoryModuleItems(items2, slotId, reason2);
				this.wheelModule.Lock();
				break;
			}
			case global::InventoryModuleTab.ENCHANTS:
			{
				string reason3;
				global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.GetAvailableRuneMarks(this.wheelSelectedSlotId, this.currentUnit.unit.Items[(int)this.wheelSelectedSlotId], out reason3, ref this.availableRuneMarks, ref this.notAvailableRuneMarks);
				this.inventoryModule.SetList(this.availableRuneMarks, this.notAvailableRuneMarks, new global::UnityEngine.Events.UnityAction<global::RuneMark>(this.OnInventorySlotConfirmed), new global::UnityEngine.Events.UnityAction<global::RuneMark>(this.OnInventorySlotSelected), reason3);
				this.wheelModule.Lock();
				break;
			}
			}
		}
	}

	private void SetInventoryModuleItems(global::System.Collections.Generic.List<global::ItemSave> itemSaves, global::UnitSlotId slotId, string reason)
	{
		global::System.Collections.Generic.List<global::Item> list = new global::System.Collections.Generic.List<global::Item>();
		for (int i = 0; i < itemSaves.Count; i++)
		{
			global::Item item = new global::Item(itemSaves[i]);
			item.SetModifiers(this.currentUnit.unit.GetMutationId(slotId));
			list.Add(item);
		}
		this.inventoryModule.SetList(list, new global::UnityEngine.Events.UnityAction<global::Item>(this.OnInventorySlotConfirmed), new global::UnityEngine.Events.UnityAction<global::Item>(this.OnInventorySlotSelected), this.wheelSelectedSlotId, reason, this.currentUnit.unit.Items[(int)slotId].IsLockSlot);
	}

	protected override void ShowDescription(string title, string desc)
	{
		base.ShowDescription(title, desc);
		this.itemDescModule.gameObject.SetActive(false);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button1.SetAction("cancel", "go_to_warband", 0, false, global::PandoraSingleton<global::HideoutTabManager>.Instance.icnBack, null);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button1.OnAction(new global::UnityEngine.Events.UnityAction(base.ReturnToWarband), false, true);
		base.SetupAttributeButtons(global::PandoraSingleton<global::HideoutTabManager>.Instance.button2, global::PandoraSingleton<global::HideoutTabManager>.Instance.button3, global::PandoraSingleton<global::HideoutTabManager>.Instance.button4);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button5.gameObject.SetActive(false);
	}

	public override void OnApplyChanges()
	{
		base.OnApplyChanges();
		this.ReturnToWheelSlotSelection();
		this.wheelModule.RefreshSlots(this.currentUnit);
	}

	protected override void OnAttributeChanged()
	{
		base.OnAttributeChanged();
		this.inventoryModule.Clear(true);
	}

	private void Pay(int amount)
	{
		global::PandoraSingleton<global::HideoutManager>.Instance.WarbandChest.RemoveGold(amount);
		global::PandoraSingleton<global::Hephaestus>.Instance.IncrementStat(global::Hephaestus.StatId.SHOP_GOLD, amount);
		this.treasuryMod.Refresh(global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.GetWarbandSave());
	}

	private void SaveChanges()
	{
		global::PandoraSingleton<global::HideoutManager>.Instance.SaveChanges();
		global::PandoraSingleton<global::HideoutManager>.Instance.currentUnit.unit.UpdateAttributes();
		base.RefreshUnitAttributes();
		this.ReturnToWheelSlotSelection();
	}

	public override bool CanIncreaseAttributes()
	{
		return true;
	}

	private global::InventoryModule inventoryModule;

	private global::WheelModule wheelModule;

	private global::ItemDescModule itemDescModule;

	private global::UnitSlotId wheelSelectedSlotId;

	private global::RuneMark currentRune;

	private global::Item tempItem;

	private global::TreasuryModule treasuryMod;

	private global::Warband warband;

	private global::System.Collections.Generic.List<global::RuneMark> availableRuneMarks = new global::System.Collections.Generic.List<global::RuneMark>();

	private global::System.Collections.Generic.List<global::RuneMark> notAvailableRuneMarks = new global::System.Collections.Generic.List<global::RuneMark>();
}
