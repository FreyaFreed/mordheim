using System;
using System.Collections.Generic;

public class Market
{
	public Market(global::Warband wb)
	{
		this.warband = wb;
		this.SetCurrentEvent((global::MarketEventId)wb.GetWarbandSave().marketEventId);
		this.shop = new global::Chest(wb.GetWarbandSave().marketItems);
		this.addedItems = new global::Chest(wb.GetWarbandSave().addedMarketItems);
	}

	public global::MarketEventId CurrentEventId { get; private set; }

	public global::System.Collections.Generic.List<global::ItemSave> GetItems()
	{
		this.AssertMarketValidity();
		return this.shop.GetItems();
	}

	public global::System.Collections.Generic.List<global::ItemSave> GetItems(global::Unit unit, global::UnitSlotId slotId)
	{
		this.AssertMarketValidity();
		return this.shop.GetItems(unit, slotId);
	}

	public global::System.Collections.Generic.List<global::ItemSave> GetAddedItems()
	{
		this.AssertMarketValidity();
		return this.addedItems.GetItems();
	}

	public bool HasItem(global::Item tempItem)
	{
		return this.shop.HasItem(tempItem);
	}

	public bool HasItem(global::ItemId itemId, global::ItemQualityId qualityId)
	{
		return this.shop.HasItem(itemId, qualityId, global::RuneMarkId.NONE, global::RuneMarkQualityId.NONE);
	}

	public global::ItemSave PopItem(global::ItemSave save, int qty = 1)
	{
		return this.shop.PopItem(save, qty);
	}

	public void AddSoldItem(global::ItemSave save)
	{
		if (this.shop.GetItems().Count < global::Constant.GetInt(global::ConstantId.MAX_SHOP_ITEM) || this.shop.HasItem(save))
		{
			this.shop.AddItem(save, true);
		}
	}

	private void AssertMarketValidity()
	{
	}

	private void SetCurrentEvent(global::MarketEventId eventId)
	{
		this.CurrentEventId = eventId;
		this.warband.GetWarbandSave().marketEventId = (int)this.CurrentEventId;
		this.warband.UpdateAttributes();
	}

	public void RefreshMarket(global::MarketEventId eventId = global::MarketEventId.NONE, bool announceRefresh = true)
	{
		global::Tyche localTyche = global::PandoraSingleton<global::GameManager>.Instance.LocalTyche;
		global::System.Collections.Generic.List<global::MarketEventData> list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::MarketEventData>().ToDynList<global::MarketEventData>();
		if (eventId == global::MarketEventId.NONE)
		{
			if (!this.warband.HasUnclaimedRecipe(global::ItemId.RECIPE_RANDOM_NORMAL) && !this.warband.HasUnclaimedRecipe(global::ItemId.RECIPE_RANDOM_MASTERY))
			{
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].Id == global::MarketEventId.EXOTIC_PIECE)
					{
						list.RemoveAt(i);
						break;
					}
				}
			}
			global::MarketEventData randomRatio = global::MarketEventData.GetRandomRatio(list, localTyche, this.warband.MarketEventModifiers);
			eventId = randomRatio.Id;
		}
		this.SetCurrentEvent(eventId);
		global::System.Collections.Generic.List<global::ItemId> allowedItemIds = this.warband.GetAllowedItemIds();
		this.addedItems.Clear();
		this.shop.RemoveSoldItems();
		global::System.Collections.Generic.List<global::ItemSave> items = this.shop.GetItems();
		int @int = global::Constant.GetInt(global::ConstantId.MAX_SHOP_ITEM);
		if (items.Count > @int)
		{
			global::System.Collections.Generic.List<global::ItemSave> list2 = new global::System.Collections.Generic.List<global::ItemSave>();
			global::System.Collections.Generic.List<global::ItemSave> list3 = new global::System.Collections.Generic.List<global::ItemSave>();
			global::System.Collections.Generic.List<global::ItemSave> list4 = new global::System.Collections.Generic.List<global::ItemSave>();
			for (int j = 0; j < items.Count; j++)
			{
				switch (items[j].qualityId)
				{
				case 1:
					list2.Add(items[j]);
					break;
				case 2:
					list3.Add(items[j]);
					break;
				case 3:
					list4.Add(items[j]);
					break;
				}
			}
			global::System.Collections.Generic.List<global::ItemSave> list5 = null;
			while (items.Count > @int)
			{
				if (list2.Count > 0)
				{
					list5 = list2;
				}
				else if (list3.Count > 0)
				{
					list5 = list3;
				}
				else if (list4.Count > 0)
				{
					list5 = list4;
				}
				int index = localTyche.Rand(0, list5.Count);
				items.Remove(list5[index]);
				list5.RemoveAt(index);
			}
		}
		if (this.CurrentEventId != global::MarketEventId.NO_ROTATION)
		{
			for (int k = items.Count - 1; k >= 0; k--)
			{
				if ((float)localTyche.Rand(0, 100) >= global::Constant.GetFloat(global::ConstantId.MARKET_REMOVE_ITEM_RATIO) * 100f || items[k].amount <= 0)
				{
					items.RemoveAt(k);
				}
			}
			global::System.Collections.Generic.List<global::MarketRefillData> list6 = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::MarketRefillData>("fk_warband_rank_id", this.warband.Rank.ToString());
			for (int l = 0; l < list6.Count; l++)
			{
				global::System.Collections.Generic.List<global::ItemId> marketAdditionalItems = this.warband.GetMarketAdditionalItems(list6[l].ItemCategoryId);
				if (list6[l].ItemCategoryId != global::ItemCategoryId.CONSUMABLE_OUT_COMBAT || marketAdditionalItems.Count != 0)
				{
					int num = localTyche.Rand(list6[l].QuantityMin, list6[l].QuantityMax + 1);
					global::System.Collections.Generic.List<global::MarketRefillQualityData> datas = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::MarketRefillQualityData>(new string[]
					{
						"fk_warband_rank_id",
						"fk_item_category_id"
					}, new string[]
					{
						this.warband.Rank.ToString(),
						((int)list6[l].ItemCategoryId).ToString()
					});
					for (int m = 0; m < num; m++)
					{
						global::MarketRefillQualityData randomRatio2 = global::MarketRefillQualityData.GetRandomRatio(datas, global::PandoraSingleton<global::GameManager>.Instance.LocalTyche, null);
						if (randomRatio2 != null)
						{
							global::ItemSave save = global::Item.GetRandomLootableItem(global::PandoraSingleton<global::GameManager>.Instance.LocalTyche, list6[l].ItemCategoryId, randomRatio2.ItemQualityId, randomRatio2.RuneMarkQualityId, this.warband.WarbandData.AllegianceId, allowedItemIds, marketAdditionalItems, null).Save;
							this.shop.AddItem(save, false);
							this.addedItems.AddItem(save, false);
						}
						else
						{
							global::PandoraDebug.LogWarning(string.Concat(new object[]
							{
								"No quality data found for this rank ",
								this.warband.Rank.ToString(),
								" and item category ",
								list6[l].ItemCategoryId
							}), "MARKET", null);
						}
					}
				}
			}
			global::MarketEventId currentEventId = this.CurrentEventId;
			if (currentEventId == global::MarketEventId.MASTER_PIECE || currentEventId == global::MarketEventId.EXOTIC_PIECE)
			{
				global::System.Collections.Generic.List<global::MarketPieceData> list7 = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::MarketPieceData>(new string[]
				{
					"fk_market_event_id",
					"fk_warband_rank_id"
				}, new string[]
				{
					((int)this.CurrentEventId).ToString(),
					this.warband.Rank.ToString()
				}).ToDynList<global::MarketPieceData>();
				if (!this.warband.HasUnclaimedRecipe(global::ItemId.RECIPE_RANDOM_NORMAL))
				{
					for (int n = 0; n < list7.Count; n++)
					{
						if (list7[n].ItemCategoryId == global::ItemCategoryId.RECIPE_ENCHANTMENT_NORMAL)
						{
							list7.RemoveAt(n);
							break;
						}
					}
				}
				if (!this.warband.HasUnclaimedRecipe(global::ItemId.RECIPE_RANDOM_MASTERY))
				{
					for (int num2 = 0; num2 < list7.Count; num2++)
					{
						if (list7[num2].ItemCategoryId == global::ItemCategoryId.RECIPE_ENCHANTMENT_MASTERY)
						{
							list7.RemoveAt(num2);
							break;
						}
					}
				}
				if (list7.Count > 0)
				{
					global::MarketPieceData randomRatio3 = global::MarketPieceData.GetRandomRatio(list7, global::PandoraSingleton<global::GameManager>.Instance.LocalTyche, null);
					global::ItemId excludedItemId = global::ItemId.NONE;
					for (int num3 = 0; num3 < 2; num3++)
					{
						global::ItemSave itemSave;
						if (randomRatio3.ItemCategoryId == global::ItemCategoryId.RECIPE_ENCHANTMENT_NORMAL)
						{
							itemSave = this.warband.GetUnclaimedRecipe(global::ItemId.RECIPE_RANDOM_NORMAL, false, excludedItemId);
						}
						else if (randomRatio3.ItemCategoryId == global::ItemCategoryId.RECIPE_ENCHANTMENT_MASTERY)
						{
							itemSave = this.warband.GetUnclaimedRecipe(global::ItemId.RECIPE_RANDOM_MASTERY, false, excludedItemId);
						}
						else
						{
							itemSave = global::Item.GetRandomLootableItem(global::PandoraSingleton<global::GameManager>.Instance.LocalTyche, randomRatio3.ItemCategoryId, randomRatio3.ItemQualityId, randomRatio3.RuneMarkQualityId, this.warband.WarbandData.AllegianceId, allowedItemIds, null, null).Save;
						}
						if (itemSave != null)
						{
							excludedItemId = (global::ItemId)itemSave.id;
							this.shop.AddItem(itemSave, false);
							this.addedItems.AddItem(itemSave, false);
						}
					}
				}
			}
		}
		if (global::PandoraSingleton<global::HideoutManager>.Exists())
		{
			global::PandoraSingleton<global::HideoutManager>.Instance.SaveChanges();
		}
	}

	public bool IsRefreshMarket(global::WeekDayId weekDayId)
	{
		return global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::WeekDayData>((int)weekDayId).RefreshMarket;
	}

	public void RemoveRecipeIOwn(global::WarbandChest warbandChest)
	{
		global::System.Collections.Generic.List<global::ItemSave> items = this.shop.GetItems();
		global::System.Collections.Generic.List<global::RuneMarkRecipeData> list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::RuneMarkRecipeData>();
		for (int i = items.Count - 1; i >= 0; i--)
		{
			global::ItemSave itemSave = items[i];
			global::ItemId itemId = (global::ItemId)itemSave.id;
			if (list.Exists((global::RuneMarkRecipeData x) => x.ItemId == itemId) && warbandChest.HasItem(itemId, global::ItemQualityId.NORMAL, global::RuneMarkId.NONE, global::RuneMarkQualityId.NONE))
			{
				items.RemoveAt(i);
				this.addedItems.RemoveItem(itemId, global::ItemQualityId.NORMAL, global::RuneMarkId.NONE, global::RuneMarkQualityId.NONE, global::AllegianceId.NONE, 1);
			}
		}
	}

	private global::Chest shop;

	private global::Chest addedItems;

	private global::Warband warband;
}
