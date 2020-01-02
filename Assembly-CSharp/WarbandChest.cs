using System;
using System.Collections.Generic;

public class WarbandChest : global::Chest
{
	public WarbandChest()
	{
		this.warband = null;
	}

	public WarbandChest(global::Warband wb) : base(wb.GetWarbandSave().items)
	{
		this.warband = wb;
		global::ItemSave itemSave = base.PopItem(new global::ItemSave(global::ItemId.WYRDSTONE_FRAGMENT, global::ItemQualityId.NONE, global::RuneMarkId.NONE, global::RuneMarkQualityId.NONE, global::AllegianceId.NONE, 1), 1);
		if (itemSave != null)
		{
			itemSave.qualityId = 1;
			base.AddItem(itemSave, false);
		}
		itemSave = base.PopItem(new global::ItemSave(global::ItemId.WYRDSTONE_SHARD, global::ItemQualityId.NONE, global::RuneMarkId.NONE, global::RuneMarkQualityId.NONE, global::AllegianceId.NONE, 1), 1);
		if (itemSave != null)
		{
			itemSave.qualityId = 1;
			base.AddItem(itemSave, false);
		}
		itemSave = base.PopItem(new global::ItemSave(global::ItemId.WYRDSTONE_CLUSTER, global::ItemQualityId.NONE, global::RuneMarkId.NONE, global::RuneMarkQualityId.NONE, global::AllegianceId.NONE, 1), 1);
		if (itemSave != null)
		{
			itemSave.qualityId = 1;
			base.AddItem(itemSave, false);
		}
	}

	public override bool AddItem(global::ItemId itemId, global::ItemQualityId quality = global::ItemQualityId.NORMAL, global::RuneMarkId runeMark = global::RuneMarkId.NONE, global::RuneMarkQualityId runeMarkQuality = global::RuneMarkQualityId.NONE, global::AllegianceId allegiance = global::AllegianceId.NONE, int count = 1, bool sold = false)
	{
		bool flag = base.AddItem(itemId, quality, runeMark, runeMarkQuality, allegiance, count, false);
		if (flag)
		{
			switch (itemId)
			{
			case global::ItemId.WYRDSTONE_FRAGMENT:
				this.warband.AddToAttribute(global::WarbandAttributeId.FRAGMENTS_GATHERED, count);
				break;
			default:
				if (itemId != global::ItemId.WYRDSTONE_SHARD)
				{
					if (itemId == global::ItemId.WYRDSTONE_CLUSTER)
					{
						this.warband.AddToAttribute(global::WarbandAttributeId.CLUSTERS_GATHERED, count);
					}
				}
				else
				{
					this.warband.AddToAttribute(global::WarbandAttributeId.SHARDS_GATHERED, count);
				}
				break;
			case global::ItemId.GOLD:
				this.warband.AddToAttribute(global::WarbandAttributeId.TOTAL_GOLD, count);
				if (this.sendStats)
				{
					global::PandoraSingleton<global::Hephaestus>.Instance.IncrementStat(global::Hephaestus.StatId.GOLD_EARNED, count);
				}
				break;
			}
			global::ItemData itemData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::ItemData>((int)itemId);
			if (itemData.ItemTypeId == global::ItemTypeId.RECIPE_ENCHANTMENT_NORMAL || itemData.ItemTypeId == global::ItemTypeId.RECIPE_ENCHANTMENT_MASTERY)
			{
				this.warband.AddToAttribute(global::WarbandAttributeId.RECIPE_FOUND, 1);
				global::PandoraSingleton<global::Hephaestus>.Instance.IncrementStat(global::Hephaestus.StatId.UNLOCKED_RECIPES, 1);
			}
		}
		return flag;
	}

	public int GetGold()
	{
		for (int i = 0; i < this.items.Count; i++)
		{
			if (this.items[i].id == 133)
			{
				return this.items[i].amount;
			}
		}
		return 0;
	}

	public void RemoveGold(int amount)
	{
		for (int i = 0; i < this.items.Count; i++)
		{
			if (this.items[i].id == 133)
			{
				this.items[i].amount -= amount;
				return;
			}
		}
	}

	public void AddGold(int amount)
	{
		this.warband.AddToAttribute(global::WarbandAttributeId.TOTAL_GOLD, amount);
		for (int i = 0; i < this.items.Count; i++)
		{
			if (this.items[i].id == 133)
			{
				this.items[i].amount += amount;
				return;
			}
		}
	}

	public global::System.Collections.Generic.List<global::ItemSave> GetSellableItems()
	{
		global::System.Collections.Generic.List<global::ItemSave> list = new global::System.Collections.Generic.List<global::ItemSave>();
		for (int i = 0; i < this.items.Count; i++)
		{
			global::ItemData itemData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::ItemData>(this.items[i].id);
			if (itemData.Sellable)
			{
				list.Add(this.items[i]);
			}
		}
		return list;
	}

	public global::Warband warband;

	public bool sendStats = true;
}
