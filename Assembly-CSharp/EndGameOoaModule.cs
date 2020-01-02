using System;
using System.Collections.Generic;
using UnityEngine;

public class EndGameOoaModule : global::UIModule
{
	public void Set(global::MissionEndUnitSave endUnit, global::Unit unit)
	{
		this.mainWeapons.ClearList();
		this.secWeapons.ClearList();
		this.equipment.ClearList();
		this.consumables.ClearList();
		this.costOfLosing.SetActive(true);
		this.titleDesc.Set("end_game_items_lost", "end_game_items_lost_desc");
		this.mainWeapons.gameObject.SetActive(false);
		this.secWeapons.gameObject.SetActive(false);
		this.equipment.gameObject.SetActive(false);
		this.consumables.gameObject.SetActive(false);
		this.injuryItem.gameObject.SetActive(false);
		global::PandoraDebug.LogDebug("Cost Of Losing Id = " + endUnit.costOfLosingId, "uncategorised", null);
		bool flag = false;
		if (endUnit.costOfLosingId > 1)
		{
			this.col = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::CostOfLosingData>(endUnit.costOfLosingId);
			if (endUnit.lostItems[2].Id != global::ItemId.NONE)
			{
				flag = true;
				this.mainWeapons.Setup("combat_main_weapon_set", this.item);
				global::UnityEngine.GameObject gameObject = this.mainWeapons.AddToList();
				global::UIInventoryItem component = gameObject.GetComponent<global::UIInventoryItem>();
				component.Set(endUnit.lostItems[2], false, false, global::ItemId.NONE, false);
				if (endUnit.lostItems[3].Id != global::ItemId.NONE)
				{
					gameObject = this.mainWeapons.AddToList();
					component = gameObject.GetComponent<global::UIInventoryItem>();
					component.Set(unit.Items[3], false, false, global::ItemId.NONE, false);
				}
				this.showQueue.Enqueue(this.mainWeapons.gameObject);
			}
			if (endUnit.lostItems[4].Id != global::ItemId.NONE)
			{
				flag = true;
				this.secWeapons.Setup("combat_alternate_weapon_set", this.item);
				global::UnityEngine.GameObject gameObject2 = this.secWeapons.AddToList();
				global::UIInventoryItem component2 = gameObject2.GetComponent<global::UIInventoryItem>();
				component2.Set(endUnit.lostItems[4], false, false, global::ItemId.NONE, false);
				if (endUnit.lostItems[5].Id != global::ItemId.NONE)
				{
					gameObject2 = this.secWeapons.AddToList();
					component2 = gameObject2.GetComponent<global::UIInventoryItem>();
					component2.Set(unit.Items[5], false, false, global::ItemId.NONE, false);
				}
				this.showQueue.Enqueue(this.secWeapons.gameObject);
			}
			if (endUnit.lostItems[1].Id != global::ItemId.NONE || endUnit.lostItems[0].Id != global::ItemId.NONE)
			{
				flag = true;
				this.equipment.Setup("hideout_menu_unit_equipment", this.item);
				if (endUnit.lostItems[0].Id != global::ItemId.NONE)
				{
					global::UnityEngine.GameObject gameObject3 = this.equipment.AddToList();
					global::UIInventoryItem component3 = gameObject3.GetComponent<global::UIInventoryItem>();
					component3.Set(endUnit.lostItems[0], false, false, global::ItemId.NONE, false);
				}
				if (endUnit.lostItems[1].Id != global::ItemId.NONE)
				{
					global::UnityEngine.GameObject gameObject4 = this.equipment.AddToList();
					global::UIInventoryItem component4 = gameObject4.GetComponent<global::UIInventoryItem>();
					component4.Set(endUnit.lostItems[1], false, false, global::ItemId.NONE, false);
				}
				this.showQueue.Enqueue(this.equipment.gameObject);
			}
			this.consumables.Setup("menu_backpack", this.item);
			for (int i = 6; i < unit.Items.Count; i++)
			{
				if (i < endUnit.lostItems.Count && endUnit.lostItems[i].Id != global::ItemId.NONE)
				{
					global::UnityEngine.GameObject gameObject5 = this.consumables.AddToList();
					global::UIInventoryItem component5 = gameObject5.GetComponent<global::UIInventoryItem>();
					component5.Set(unit.Items[i], false, false, global::ItemId.NONE, false);
				}
			}
			if (this.consumables.items.Count > 0)
			{
				flag = true;
				this.showQueue.Enqueue(this.consumables.gameObject);
			}
			if (this.col.OpenWound)
			{
				flag = true;
				this.injuryItem.Set("enchant_title_open_wound", "enchant_desc_open_wound");
				this.injuryItem.gameObject.SetActive(false);
				this.showQueue.Enqueue(this.injuryItem.gameObject);
			}
		}
		if (!flag)
		{
			this.injuryItem.Set("end_game_safe_return_title", "end_game_safe_return_desc");
			this.showQueue.Enqueue(this.injuryItem.gameObject);
		}
		base.StartShow(0.5f);
	}

	public void Set(global::Chest chest)
	{
		this.costOfLosing.SetActive(true);
		this.chestItems.gameObject.SetActive(true);
		this.mainWeapons.gameObject.SetActive(false);
		this.secWeapons.gameObject.SetActive(false);
		this.equipment.gameObject.SetActive(false);
		this.consumables.gameObject.SetActive(false);
		this.injuryItem.gameObject.SetActive(false);
		this.mainWeapons.ClearList();
		this.secWeapons.ClearList();
		this.equipment.ClearList();
		this.consumables.ClearList();
		this.chestItems.ClearList();
		this.chestItems.Setup(string.Empty, this.item);
		this.titleDesc.Set("warband_looted_items", "end_game_looted_items_desc");
		this.showQueue.Enqueue(this.chestItems.gameObject);
		this.chestItems.gameObject.SetActive(false);
		global::System.Collections.Generic.List<global::ItemSave> items = chest.GetItems();
		for (int i = 0; i < items.Count; i++)
		{
			global::UnityEngine.GameObject gameObject = this.chestItems.AddToList();
			global::UIInventoryItem component = gameObject.GetComponent<global::UIInventoryItem>();
			component.Set(new global::Item(items[i]), false, false, global::ItemId.NONE, false);
			this.showQueue.Enqueue(gameObject);
			gameObject.SetActive(false);
		}
		this.chestItems.gameObject.SetActive(false);
		base.StartShow(0.5f);
	}

	public global::UnityEngine.GameObject costOfLosing;

	public global::UIDescription titleDesc;

	public global::ListGroup mainWeapons;

	public global::ListGroup secWeapons;

	public global::ListGroup equipment;

	public global::ListGroup consumables;

	public global::ListGroup chestItems;

	public global::UIDescription injuryItem;

	public global::UnityEngine.GameObject item;

	public global::CostOfLosingData col;
}
