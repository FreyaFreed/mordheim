using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HideoutShop : global::ICheapState
{
	public HideoutShop(global::HideoutManager mng, global::HideoutCamAnchor anchor)
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
		this.warband = global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband;
		global::PandoraSingleton<global::HideoutTabManager>.Instance.ActivateLeftTabModules(true, new global::ModuleId[]
		{
			global::ModuleId.LATEST_ARRIVAL
		});
		global::PandoraSingleton<global::HideoutTabManager>.Instance.ActivateRightTabModules(true, new global::ModuleId[]
		{
			global::ModuleId.TREASURY,
			global::ModuleId.SHOP
		});
		global::PandoraSingleton<global::HideoutTabManager>.Instance.ActivateCenterTabModules(new global::ModuleId[]
		{
			global::ModuleId.ITEM_DESC,
			global::ModuleId.WARBAND_TABS,
			global::ModuleId.TITLE,
			global::ModuleId.NOTIFICATION
		});
		this.warbandTabs = global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleCenter<global::WarbandTabsModule>(global::ModuleId.WARBAND_TABS);
		this.warbandTabs.Setup(global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleCenter<global::TitleModule>(global::ModuleId.TITLE));
		this.warbandTabs.SetCurrentTab(global::HideoutManager.State.SHOP);
		this.warbandTabs.Refresh();
		this.latestMod = global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleLeft<global::LatestArrivalModule>(global::ModuleId.LATEST_ARRIVAL);
		global::System.Collections.Generic.List<global::ItemSave> addedItems = global::PandoraSingleton<global::HideoutManager>.Instance.Market.GetAddedItems();
		this.latestMod.Set(global::PandoraSingleton<global::HideoutManager>.Instance.Market.CurrentEventId, addedItems, null, null);
		this.treasuryMod = global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleRight<global::TreasuryModule>(global::ModuleId.TREASURY);
		this.descModule = global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleCenter<global::ItemDescModule>(global::ModuleId.ITEM_DESC);
		this.descModule.HideAll();
		this.shopModule = global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleRight<global::ShopModule>(global::ModuleId.SHOP);
		this.shopModule.Init(new global::UnityEngine.Events.UnityAction<global::ShopModuleTab>(this.OnTabSelected));
		this.shopModule.SetTab(global::ShopModuleTab.BUY);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button1.SetAction("cancel", "go_to_camp", 0, false, global::PandoraSingleton<global::HideoutTabManager>.Instance.icnBack, null);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button1.OnAction(new global::UnityEngine.Events.UnityAction(this.ReturnToCamp), false, true);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button2.gameObject.SetActive(false);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button3.gameObject.SetActive(false);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button4.gameObject.SetActive(false);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button5.gameObject.SetActive(false);
		global::UnityEngine.GameObject shopNodeContent = global::PandoraSingleton<global::HideoutManager>.Instance.GetShopNodeContent();
		global::PandoraSingleton<global::HideoutManager>.Instance.shopNode.SetContent(shopNodeContent);
		shopNodeContent.transform.localPosition = new global::UnityEngine.Vector3(0.84f, 0f, 0f);
		shopNodeContent.transform.localRotation = global::UnityEngine.Quaternion.Euler(new global::UnityEngine.Vector3(0f, 340f, 0f));
		this.once = true;
	}

	void global::ICheapState.Exit(int iTo)
	{
		this.shopModule.itemRuneList.Clear();
	}

	void global::ICheapState.Update()
	{
	}

	void global::ICheapState.FixedUpdate()
	{
		if (this.once)
		{
			this.once = false;
			global::PandoraSingleton<global::HideoutManager>.Instance.ShowHideoutTuto(global::HideoutManager.HideoutTutoType.SHOP);
		}
	}

	private void ReturnToCamp()
	{
		global::PandoraSingleton<global::HideoutManager>.Instance.StateMachine.ChangeState(0);
	}

	private void BuySell()
	{
		if (this.tempItem != null)
		{
			this.OnSlotConfirmed(this.tempItem);
		}
	}

	private void OnTabSelected(global::ShopModuleTab tab)
	{
		global::System.Collections.Generic.List<global::ItemSave> list = null;
		if (tab != global::ShopModuleTab.BUY)
		{
			if (tab == global::ShopModuleTab.SELL)
			{
				list = global::PandoraSingleton<global::HideoutManager>.Instance.WarbandChest.GetSellableItems();
			}
		}
		else
		{
			list = global::PandoraSingleton<global::HideoutManager>.Instance.Market.GetItems();
		}
		global::System.Collections.Generic.List<global::Item> list2 = new global::System.Collections.Generic.List<global::Item>();
		for (int i = 0; i < list.Count; i++)
		{
			global::Item item = new global::Item(list[i]);
			list2.Add(item);
		}
		list2.Sort(new global::CompareItem());
		this.shopModule.SetList(list2, new global::UnityEngine.Events.UnityAction<global::Item>(this.OnSlotConfirmed), new global::UnityEngine.Events.UnityAction<global::Item>(this.OnSlotSelected), tab == global::ShopModuleTab.BUY);
		if (list2.Count <= 0)
		{
			this.descModule.HideAll();
		}
		else if (global::PandoraSingleton<global::PandoraInput>.Instance.CurrentInputLayer == 0)
		{
			this.shopModule.StartCoroutine(this.SelectFirstItemOnNextFrame());
		}
	}

	private global::System.Collections.IEnumerator SelectFirstItemOnNextFrame()
	{
		yield return null;
		this.shopModule.itemRuneList.scrollGroup.items[0].SetSelected(false);
		yield break;
	}

	private void OnSlotSelected(global::Item item)
	{
		this.tempItem = item;
		this.descModule.SetItem(item, global::UnitSlotId.NONE, 0);
	}

	private void OnSlotConfirmed(global::Item item)
	{
		if (global::PandoraSingleton<global::PandoraInput>.Instance.CurrentInputLayer != 0)
		{
			return;
		}
		this.tempItem = item;
		global::ShopModuleTab currentTab = this.shopModule.currentTab;
		if (currentTab != global::ShopModuleTab.BUY)
		{
			if (currentTab == global::ShopModuleTab.SELL)
			{
				if (!global::PandoraSingleton<global::HideoutManager>.Instance.WarbandChest.HasItem(this.tempItem))
				{
					this.ItemNotAvailalble();
				}
				else
				{
					int itemSellPrice = this.warband.GetItemSellPrice(this.tempItem);
					global::PandoraSingleton<global::HideoutManager>.Instance.shopConfirmPopup.Show(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_sell_confirm_title"), global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_sell_confirm_desc", new string[]
					{
						this.tempItem.LocalizedName,
						itemSellPrice.ToString()
					}), this.tempItem.Amount, itemSellPrice, false, new global::System.Action<bool, int>(this.OnSellItemPopupConfirmed));
				}
			}
		}
		else if (!global::PandoraSingleton<global::HideoutManager>.Instance.Market.HasItem(this.tempItem))
		{
			this.ItemNotAvailalble();
		}
		else if (this.warband.GetItemBuyPrice(this.tempItem) > global::PandoraSingleton<global::HideoutManager>.Instance.WarbandChest.GetGold())
		{
			global::PandoraSingleton<global::HideoutManager>.Instance.messagePopup.ShowLocalized(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_no_gold"), global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_no_gold_item"), null, false, false);
			global::PandoraSingleton<global::HideoutManager>.Instance.messagePopup.HideCancelButton();
		}
		else
		{
			int itemBuyPrice = this.warband.GetItemBuyPrice(this.tempItem);
			global::PandoraSingleton<global::HideoutManager>.Instance.shopConfirmPopup.Show(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_buy_confirm_title"), global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_buy_confirm_desc", new string[]
			{
				this.tempItem.LocalizedName,
				itemBuyPrice.ToString()
			}), this.tempItem.Amount, itemBuyPrice, true, new global::System.Action<bool, int>(this.OnBuyItemPopupConfirmed));
			global::PandoraSingleton<global::HideoutManager>.Instance.shopConfirmPopup.confirmButton.SetSelected(false);
			global::PandoraSingleton<global::HideoutManager>.Instance.shopConfirmPopup.confirmButton.effects.toggle.isOn = true;
		}
	}

	private void OnBuyItemPopupConfirmed(bool confirm, int qty)
	{
		if (confirm && this.tempItem != null)
		{
			int itemBuyPrice = this.warband.GetItemBuyPrice(this.tempItem);
			if (itemBuyPrice * qty <= global::PandoraSingleton<global::HideoutManager>.Instance.WarbandChest.GetGold())
			{
				global::ItemSave save = global::PandoraSingleton<global::HideoutManager>.Instance.Market.PopItem(this.tempItem.Save, qty);
				global::PandoraSingleton<global::HideoutManager>.Instance.WarbandChest.RemoveGold(qty * itemBuyPrice);
				global::PandoraSingleton<global::Hephaestus>.Instance.IncrementStat(global::Hephaestus.StatId.SHOP_GOLD, qty * itemBuyPrice);
				global::PandoraSingleton<global::HideoutManager>.Instance.WarbandChest.AddItem(save, false);
				this.warband.AddToAttribute(global::WarbandAttributeId.BUY_AMOUNT, qty * itemBuyPrice);
				this.SaveRefresh();
			}
		}
		this.tempItem = null;
	}

	private void OnSellItemPopupConfirmed(bool confirm, int qty)
	{
		if (confirm && this.tempItem != null)
		{
			global::ItemSave save = global::PandoraSingleton<global::HideoutManager>.Instance.WarbandChest.PopItem(this.tempItem.Save, qty);
			int itemSellPrice = this.warband.GetItemSellPrice(this.tempItem);
			global::PandoraSingleton<global::HideoutManager>.Instance.WarbandChest.AddGold(qty * itemSellPrice);
			global::PandoraSingleton<global::HideoutManager>.Instance.Market.AddSoldItem(save);
			this.warband.AddToAttribute(global::WarbandAttributeId.SELL_AMOUNT, qty * itemSellPrice);
			this.SaveRefresh();
		}
		this.tempItem = null;
	}

	private void SaveRefresh()
	{
		global::PandoraSingleton<global::HideoutManager>.Instance.SaveChanges();
		this.latestMod.Set(global::PandoraSingleton<global::HideoutManager>.Instance.Market.CurrentEventId, global::PandoraSingleton<global::HideoutManager>.Instance.Market.GetAddedItems(), null, null);
		this.shopModule.RemoveItem(this.tempItem);
		this.treasuryMod.Refresh(global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.GetWarbandSave());
	}

	private void ItemNotAvailalble()
	{
		global::PandoraSingleton<global::HideoutManager>.Instance.messagePopup.ShowLocalized(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("item_unavailable"), global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("item_desc_unavailable", new string[]
		{
			this.tempItem.LocalizedName
		}), null, false, false);
		global::PandoraSingleton<global::HideoutManager>.Instance.messagePopup.HideCancelButton();
	}

	private global::HideoutCamAnchor camAnchor;

	private global::ShopModule shopModule;

	private global::ItemDescModule descModule;

	private global::Item tempItem;

	private global::LatestArrivalModule latestMod;

	private global::TreasuryModule treasuryMod;

	private global::Warband warband;

	private global::WarbandTabsModule warbandTabs;

	private bool once = true;
}
