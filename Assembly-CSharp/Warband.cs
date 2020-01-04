using System;
using System.Collections.Generic;
using UnityEngine;

public class Warband
{
	public Warband(global::WarbandSave ws)
	{
		this.warbandSave = ws;
		this.ValidateOutsiders();
		this.Logger = new global::EventLogger(ws.stats.history);
		this.MarketEventModifiers = new global::System.Collections.Generic.Dictionary<global::MarketEventId, int>(global::MarketEventIdComparer.Instance);
		this.MissionRatingModifiers = new global::System.Collections.Generic.Dictionary<global::ProcMissionRatingId, int>(global::ProcMissionRatingIdComparer.Instance);
		this.AddtionnalMarketItems = new global::System.Collections.Generic.List<global::ItemId>();
		this.unclaimedRecipe = new global::System.Collections.Generic.List<global::ItemId>();
		this.WyrdstoneDensityModifiers = new global::System.Collections.Generic.Dictionary<int, int>[5];
		this.SearchDensityModifiers = new global::System.Collections.Generic.Dictionary<int, int>[5];
		for (int i = 0; i < 5; i++)
		{
			this.SearchDensityModifiers[i] = new global::System.Collections.Generic.Dictionary<int, int>();
			this.WyrdstoneDensityModifiers[i] = new global::System.Collections.Generic.Dictionary<int, int>();
		}
		this.attributeDataList = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::WarbandAttributeData>();
		this.Id = (global::WarbandId)this.warbandSave.id;
		this.WarbandData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::WarbandData>((int)this.Id);
		this.attributes = new int[63];
		global::System.Array.Copy(this.warbandSave.stats.stats, this.attributes, 63);
		this.Units = new global::System.Collections.Generic.List<global::Unit>();
		for (int j = 0; j < this.warbandSave.units.Count; j++)
		{
			global::Unit item = new global::Unit(this.warbandSave.units[j]);
			this.Units.Add(item);
		}
		this.Outsiders = new global::System.Collections.Generic.List<global::Unit>();
		for (int k = 0; k < this.warbandSave.outsiders.Count; k++)
		{
			this.warbandSave.outsiders[k].attributes.Clear();
			this.warbandSave.outsiders[k].activeSkills.Clear();
			this.warbandSave.outsiders[k].passiveSkills.Clear();
			this.warbandSave.outsiders[k].spells.Clear();
			global::Unit item2 = new global::Unit(this.warbandSave.outsiders[k]);
			this.Outsiders.Add(item2);
		}
		this.HireableUnitIds = new global::System.Collections.Generic.List<global::UnitId>();
		this.HireableOutsiderUnitIds = new global::System.Collections.Generic.List<global::UnitData>();
		this.HireableUnitTypeRank = new int[10];
		this.skills = new global::System.Collections.Generic.List<global::WarbandSkill>();
		for (int l = 0; l < this.warbandSave.skills.Count; l++)
		{
			this.AddSkill(this.warbandSave.skills[l], false, false);
		}
		this.Factions = new global::System.Collections.Generic.List<global::Faction>();
		for (int m = 0; m < this.warbandSave.factions.Count; m++)
		{
			this.Factions.Add(new global::Faction(this.warbandSave, this.warbandSave.factions[m]));
			for (int n = 0; n < this.Factions[m].Rewards.Count; n++)
			{
				if (this.Factions[m].HasRank((int)this.Factions[m].Rewards[n].FactionRankId))
				{
					this.AddSkill(this.Factions[m].Rewards[n].WarbandSkillId, false, true);
				}
			}
		}
		this.RefreshPlayerSkills(false);
		this.UpdateAttributes();
	}

	public global::System.Collections.Generic.List<global::UnitId> HireableUnitIds { get; private set; }

	public global::System.Collections.Generic.List<global::UnitData> HireableOutsiderUnitIds { get; private set; }

	public global::System.Collections.Generic.List<global::ItemId> AddtionnalMarketItems { get; private set; }

	public int[] HireableUnitTypeRank { get; private set; }

	public global::System.Collections.Generic.List<global::WarbandSkill> skills { get; private set; }

	public global::System.Collections.Generic.List<global::WarbandEnchantment> enchantments { get; private set; }

	public global::WarbandId Id { get; private set; }

	public global::WarbandData WarbandData { get; private set; }

	public global::System.Collections.Generic.List<global::Unit> Units { get; private set; }

	public global::System.Collections.Generic.List<global::Unit> Outsiders { get; private set; }

	public global::System.Collections.Generic.List<global::Faction> Factions { get; private set; }

	public global::System.Collections.Generic.Dictionary<int, int>[] WyrdstoneDensityModifiers { get; private set; }

	public global::System.Collections.Generic.Dictionary<int, int>[] SearchDensityModifiers { get; private set; }

	public global::System.Collections.Generic.Dictionary<global::MarketEventId, int> MarketEventModifiers { get; private set; }

	public global::System.Collections.Generic.Dictionary<global::ProcMissionRatingId, int> MissionRatingModifiers { get; set; }

	public global::EventLogger Logger { get; private set; }

	public int Rank
	{
		get
		{
			return this.GetAttribute(global::WarbandAttributeId.RANK);
		}
		set
		{
			this.warbandSave.rank = value;
			this.SetAttribute(global::WarbandAttributeId.RANK, value);
		}
	}

	public int Xp
	{
		get
		{
			return this.warbandSave.xp;
		}
		set
		{
			this.warbandSave.xp = value;
		}
	}

	public bool ValidateWarbandForInvite(bool inMission)
	{
		global::WarbandSave warbandSave = this.GetWarbandSave();
		if ((!inMission && warbandSave.inMission) || global::PandoraSingleton<global::Hephaestus>.Instance.joiningLobby == null)
		{
			return false;
		}
		string text;
		if (global::PandoraSingleton<global::Hephaestus>.Instance.joiningLobby.isExhibition)
		{
			return global::PandoraUtils.IsBetween(this.GetSkirmishRating(), global::PandoraSingleton<global::Hephaestus>.Instance.joiningLobby.ratingMin, global::PandoraSingleton<global::Hephaestus>.Instance.joiningLobby.ratingMax) && this.IsSkirmishAvailable(out text);
		}
		return global::PandoraUtils.IsBetween(this.GetRating(), global::PandoraSingleton<global::Hephaestus>.Instance.joiningLobby.ratingMin, global::PandoraSingleton<global::Hephaestus>.Instance.joiningLobby.ratingMax) && this.IsContestAvailable(out text);
	}

	private void ValidateOutsiders()
	{
		if (this.warbandSave == null || this.warbandSave.outsiders == null)
		{
			return;
		}
		for (int i = this.warbandSave.outsiders.Count - 1; i >= 0; i--)
		{
			global::WarbandId warbandId = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::UnitData>(this.warbandSave.outsiders[i].stats.id).WarbandId;
			if (!global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::WarbandData>((int)warbandId).Basic)
			{
				this.warbandSave.outsiders.RemoveAt(i);
			}
		}
	}

	public void UpdateAttributes()
	{
		for (int i = 0; i < this.attributeDataList.Count; i++)
		{
			if (!this.attributeDataList[i].Persistent)
			{
				this.SetAttribute(this.attributeDataList[i].Id, this.attributeDataList[i].BaseValue);
			}
		}
		this.SetAttribute(global::WarbandAttributeId.RANK, this.warbandSave.rank);
		for (int j = 0; j < this.HireableUnitTypeRank.Length; j++)
		{
			this.HireableUnitTypeRank[j] = 1;
		}
		int num = 5;
		for (int k = 0; k < num; k++)
		{
			this.SearchDensityModifiers[k].Clear();
			this.WyrdstoneDensityModifiers[k].Clear();
		}
		this.MissionRatingModifiers.Clear();
		this.MarketEventModifiers.Clear();
		this.HireableUnitIds.Clear();
		this.HireableOutsiderUnitIds.Clear();
		this.AddtionnalMarketItems.Clear();
		if (this.warbandSave.currentDate == global::Constant.GetInt(global::ConstantId.CAL_DAY_START))
		{
			for (global::ProcMissionRatingId procMissionRatingId = global::ProcMissionRatingId.NONE; procMissionRatingId < global::ProcMissionRatingId.MAX_VALUE; procMissionRatingId++)
			{
				if (procMissionRatingId != global::ProcMissionRatingId.NORMAL)
				{
					this.MissionRatingModifiers.Add(procMissionRatingId, -100);
				}
			}
		}
		global::System.Collections.Generic.List<global::UnitData> list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::UnitData>("fk_warband_id", ((int)this.Id).ToString(), "base", "1");
		for (int l = 0; l < list.Count; l++)
		{
			if (this.IsUnitTypeUnlocked(list[l].UnitTypeId))
			{
				this.HireableUnitIds.Add(list[l].Id);
				this.HireableOutsiderUnitIds.Add(list[l]);
			}
		}
		for (int m = 0; m < this.skills.Count; m++)
		{
			this.ApplySkill(this.skills[m]);
		}
		this.ApplySkill(new global::WarbandSkill(global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::MarketEventData>(this.warbandSave.marketEventId).WarbandSkillId));
		this.UpdateFactionAttributes();
	}

	public bool HasUnclaimedRecipe(global::ItemId randomRecipe)
	{
		this.unclaimedRecipe.Clear();
		global::ItemData itemData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::ItemData>((int)randomRecipe);
		global::System.Collections.Generic.List<global::ItemData> list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::ItemData>(new string[]
		{
			"fk_item_type_id",
			"lootable"
		}, new string[]
		{
			itemData.ItemTypeId.ToIntString<global::ItemTypeId>(),
			"1"
		});
		for (int i = 0; i < list.Count; i++)
		{
			if (!global::PandoraSingleton<global::HideoutManager>.Instance.WarbandChest.HasItem(list[i].Id, global::ItemQualityId.NORMAL, global::RuneMarkId.NONE, global::RuneMarkQualityId.NONE) && !global::PandoraSingleton<global::HideoutManager>.Instance.Market.HasItem(list[i].Id, global::ItemQualityId.NORMAL))
			{
				this.unclaimedRecipe.Add(list[i].Id);
			}
		}
		return this.unclaimedRecipe.Count > 0;
	}

	public global::ItemSave GetUnclaimedRecipe(global::ItemId randomRecipe, bool giveGoldOnAllClaimed = true, global::ItemId excludedItemId = global::ItemId.NONE)
	{
		this.unclaimedRecipe.Clear();
		global::ItemData itemData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::ItemData>((int)randomRecipe);
		global::System.Collections.Generic.List<global::ItemData> list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::ItemData>(new string[]
		{
			"fk_item_type_id",
			"lootable"
		}, new string[]
		{
			itemData.ItemTypeId.ToIntString<global::ItemTypeId>(),
			"1"
		});
		for (int i = 0; i < list.Count; i++)
		{
			if (!global::PandoraSingleton<global::HideoutManager>.Instance.WarbandChest.HasItem(list[i].Id, global::ItemQualityId.NORMAL, global::RuneMarkId.NONE, global::RuneMarkQualityId.NONE) && !global::PandoraSingleton<global::HideoutManager>.Instance.Market.HasItem(list[i].Id, global::ItemQualityId.NORMAL) && list[i].Id != excludedItemId)
			{
				this.unclaimedRecipe.Add(list[i].Id);
			}
		}
		if (this.unclaimedRecipe.Count > 0)
		{
			return new global::ItemSave(this.unclaimedRecipe[global::PandoraSingleton<global::GameManager>.Instance.LocalTyche.Rand(0, this.unclaimedRecipe.Count)], global::ItemQualityId.NORMAL, global::RuneMarkId.NONE, global::RuneMarkQualityId.NONE, global::AllegianceId.NONE, 1);
		}
		if (giveGoldOnAllClaimed)
		{
			return new global::ItemSave(global::ItemId.GOLD, global::ItemQualityId.NORMAL, global::RuneMarkId.NONE, global::RuneMarkQualityId.NONE, global::AllegianceId.NONE, 1)
			{
				amount = itemData.PriceSold
			};
		}
		return null;
	}

	private void UpdateFactionAttributes()
	{
		this.AddToAttribute(global::WarbandAttributeId.REQUEST_TIME_MODIFIER, this.warbandSave.nextShipmentExtraDays);
		for (int i = 0; i < this.Factions.Count; i++)
		{
			if (this.Factions[i].Primary && this.warbandSave.lastShipmentFailed)
			{
				global::FactionConsequenceData factionConsequenceData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::FactionConsequenceData>(new string[]
				{
					"fk_faction_id",
					"late_shipment_count"
				}, new string[]
				{
					((int)this.Factions[i].Data.Id).ToString(),
					this.warbandSave.lateShipmentCount.ToString()
				})[0];
				this.AddToAttribute(global::WarbandAttributeId.REQUEST_PRICE_GLOBAL_PERC, factionConsequenceData.NextShipmentGoldRewardModifierPerc);
				this.AddToAttribute(global::WarbandAttributeId.REQUEST_WEIGHT_PERC, factionConsequenceData.NextShipmentRequestModifierPerc);
				break;
			}
		}
	}

	public void CheckRespecPoints()
	{
		if (this.warbandSave.availaibleRespec == -1)
		{
			this.warbandSave.availaibleRespec = 1;
			int @int = global::Constant.GetInt(global::ConstantId.CAL_DAYS_PER_YEAR);
			int num = (int)((float)this.warbandSave.currentDate / (float)@int);
			this.warbandSave.availaibleRespec += num;
			this.Logger.AddHistory(@int * (num + 1), global::EventLogger.LogEvent.RESPEC_POINT, 0);
		}
	}

	public void ResetPlayerSkills()
	{
		this.warbandSave.availaibleRespec = global::UnityEngine.Mathf.Max(0, this.warbandSave.availaibleRespec - 1);
		for (int i = this.skills.Count - 1; i >= 0; i--)
		{
			if (this.skills[i].TypeId == global::WarbandSkillTypeId.PLAYER_SKILL)
			{
				this.warbandSave.skills.Remove(this.skills[i].Id);
				this.skills.RemoveAt(i);
			}
		}
	}

	public global::System.Collections.Generic.List<global::WarbandSkill> GetPlayerSkills()
	{
		global::System.Collections.Generic.List<global::WarbandSkill> list = new global::System.Collections.Generic.List<global::WarbandSkill>();
		for (int i = 0; i < this.skills.Count; i++)
		{
			if (this.skills[i].TypeId == global::WarbandSkillTypeId.PLAYER_SKILL)
			{
				list.Add(this.skills[i]);
			}
		}
		return list;
	}

	public int GetPlayerSkillsAvailablePoints()
	{
		int num = this.GetAttribute(global::WarbandAttributeId.PLAYER_SKILL_POINTS);
		for (int i = 0; i < this.skills.Count; i++)
		{
			if (this.skills[i].TypeId == global::WarbandSkillTypeId.PLAYER_SKILL)
			{
				num -= this.skills[i].Data.Points;
				if (this.skills[i].Data.WarbandSkillIdPrerequisite != global::WarbandSkillId.NONE)
				{
					global::WarbandSkillData warbandSkillData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::WarbandSkillData>((int)this.skills[i].Data.WarbandSkillIdPrerequisite);
					num -= warbandSkillData.Points;
				}
			}
		}
		return num;
	}

	public void RefreshPlayerSkills(bool isNew = false)
	{
		int attribute = this.GetAttribute(global::WarbandAttributeId.OUTSIDERS_COUNT);
		for (int i = 0; i <= global::PandoraSingleton<global::GameManager>.Instance.Profile.Rank; i++)
		{
			global::PlayerRankData playerRankData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::PlayerRankData>(i);
			if (playerRankData != null)
			{
				this.AddSkill(playerRankData.WarbandSkillId, isNew, true);
			}
		}
		if (isNew && attribute == 0 && this.GetAttribute(global::WarbandAttributeId.OUTSIDERS_COUNT) > 0)
		{
			this.AddOutsiderRotationEvent();
		}
	}

	public void AddOutsiderRotationEvent()
	{
		global::System.Collections.Generic.List<global::WeekDayData> list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::WeekDayData>();
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i].RefreshOutsiders)
			{
				int nextDay = global::PandoraSingleton<global::HideoutManager>.Instance.Date.GetNextDay(list[i].Id);
				if (!this.Logger.HasEventAtDay(global::EventLogger.LogEvent.OUTSIDER_ROTATION, nextDay))
				{
					this.Logger.AddHistory(nextDay, global::EventLogger.LogEvent.OUTSIDER_ROTATION, 0);
				}
			}
		}
	}

	public bool HasSkill(global::WarbandSkillId skillId, bool includeMastery)
	{
		for (int i = 0; i < this.skills.Count; i++)
		{
			if (this.skills[i].Id == skillId || (includeMastery && this.skills[i].Data.WarbandSkillIdPrerequisite == skillId))
			{
				return true;
			}
		}
		return false;
	}

	public void LearnSkill(global::WarbandSkill skill)
	{
		this.warbandSave.skills.Add(skill.Id);
		this.AddSkill(skill.Id, true, true);
	}

	public void AddSkill(global::WarbandSkillId skillId, bool isNew = false, bool updateAttributes = true)
	{
		for (int i = 0; i < this.skills.Count; i++)
		{
			if (this.skills[i].Id == skillId)
			{
				return;
			}
		}
		bool flag = true;
		global::WarbandSkill warbandSkill = new global::WarbandSkill(skillId);
		if (warbandSkill.IsMastery)
		{
			for (int j = this.skills.Count - 1; j >= 0; j--)
			{
				if (this.skills[j].Id == warbandSkill.Data.WarbandSkillIdPrerequisite)
				{
					this.skills[j] = warbandSkill;
					flag = false;
					break;
				}
			}
		}
		else
		{
			for (int k = 0; k < this.skills.Count; k++)
			{
				if (this.skills[k].Data.WarbandSkillIdPrerequisite == warbandSkill.Id)
				{
					return;
				}
			}
		}
		if (flag)
		{
			this.skills.Add(warbandSkill);
		}
		if (updateAttributes)
		{
			this.UpdateAttributes();
		}
		if (isNew)
		{
			global::System.Collections.Generic.List<global::WarbandSkillItemData> list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::WarbandSkillItemData>("fk_warband_skill_id", warbandSkill.Id.ToIntString<global::WarbandSkillId>());
			for (int l = 0; l < list.Count; l++)
			{
				global::PandoraSingleton<global::HideoutManager>.Instance.WarbandChest.AddItem(list[l].ItemId, list[l].ItemQualityId, list[l].RuneMarkId, list[l].RuneMarkQualityId, global::AllegianceId.NONE, list[l].Quantity, false);
			}
			global::System.Collections.Generic.List<global::WarbandSkillFreeOutsiderData> list2 = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::WarbandSkillFreeOutsiderData>("fk_warband_skill_id", warbandSkill.Id.ToIntString<global::WarbandSkillId>());
			for (int m = 0; m < list2.Count; m++)
			{
				int num = list2[m].Rank;
				global::ItemQualityId itemQualityId = global::ItemQualityId.NONE;
				if (num == -1)
				{
					for (int n = 0; n < this.Units.Count; n++)
					{
						num = global::UnityEngine.Mathf.Max(num, this.Units[n].Rank);
					}
					num = global::UnityEngine.Mathf.Clamp(num, 0, 7);
					itemQualityId = ((num < 6) ? global::ItemQualityId.GOOD : global::ItemQualityId.BEST);
					itemQualityId = ((num > 2) ? itemQualityId : global::ItemQualityId.NORMAL);
				}
				global::Unit unit = global::Unit.GenerateUnit(list2[m].UnitId, num);
				unit.UnitSave.isFreeOutsider = true;
				unit.UnitSave.isOutsider = true;
				global::System.Collections.Generic.List<global::WarbandSkillFreeOutsiderItemData> list3 = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::WarbandSkillFreeOutsiderItemData>("fk_warband_skill_free_outsider_id", list2[m].Id.ToIntString<global::WarbandSkillFreeOutsiderId>());
				for (int num2 = 0; num2 < list3.Count; num2++)
				{
					unit.EquipItem(list3[num2].UnitSlotId, new global::ItemSave(list3[num2].ItemId, (itemQualityId != global::ItemQualityId.NONE) ? itemQualityId : list3[num2].ItemQualityId, list3[num2].RuneMarkId, list3[num2].RuneMarkQualityId, global::AllegianceId.NONE, 1));
				}
				this.warbandSave.outsiders.Add(unit.UnitSave);
				this.Outsiders.Add(unit);
			}
		}
	}

	private void ApplySkill(global::WarbandSkill skill)
	{
		this.ApplyEnchantments(skill.Enchantments);
		for (int i = 0; i < skill.HireableUnits.Count; i++)
		{
			global::WarbandSkillUnitData warbandSkillUnitData = skill.HireableUnits[i];
			if (warbandSkillUnitData.UnitId != global::UnitId.NONE)
			{
				if (warbandSkillUnitData.BaseUnit && !this.HireableUnitIds.Contains(warbandSkillUnitData.UnitId, global::UnitIdComparer.Instance))
				{
					this.HireableUnitIds.Add(warbandSkillUnitData.UnitId);
				}
				if (!this.HireableOutsiderUnitIds.Exists((global::UnitData x) => x.Id == warbandSkillUnitData.UnitId))
				{
					this.HireableOutsiderUnitIds.Add(global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::UnitData>((int)warbandSkillUnitData.UnitId));
				}
			}
		}
		for (int j = 0; j < skill.UnitTypeRankDatas.Count; j++)
		{
			this.HireableUnitTypeRank[(int)skill.UnitTypeRankDatas[j].UnitTypeId] = global::UnityEngine.Mathf.Max(skill.UnitTypeRankDatas[j].Rank, this.HireableUnitTypeRank[(int)skill.UnitTypeRankDatas[j].UnitTypeId]);
		}
		for (int k = 0; k < skill.MarketItems.Count; k++)
		{
			if (!this.AddtionnalMarketItems.Contains(skill.MarketItems[k].ItemId, global::ItemIdComparer.Instance))
			{
				this.AddtionnalMarketItems.Add(skill.MarketItems[k].ItemId);
			}
		}
	}

	public void GenerateContactItems()
	{
		global::System.Collections.Generic.List<global::WarbandContactItemQualityData> datas = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::WarbandContactItemQualityData>("fk_warband_rank_id", this.Rank.ToString());
		global::System.Collections.Generic.List<global::ItemTypeId> list = new global::System.Collections.Generic.List<global::ItemTypeId>();
		for (int i = 0; i < this.skills.Count; i++)
		{
			if (this.skills[i].ContactsData.Count > 0)
			{
				global::WarbandSkillWarbandContactData randomRatio = global::WarbandSkillWarbandContactData.GetRandomRatio(this.skills[i].ContactsData, global::PandoraSingleton<global::GameManager>.Instance.LocalTyche, null);
				if (randomRatio.WarbandContactId != global::WarbandContactId.NONE)
				{
					list.Clear();
					global::WarbandContactData warbandContactData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::WarbandContactData>((int)randomRatio.WarbandContactId);
					global::WarbandContactItemQualityData randomRatio2 = global::WarbandContactItemQualityData.GetRandomRatio(datas, global::PandoraSingleton<global::GameManager>.Instance.LocalTyche, null);
					global::ItemQualityId itemQualityId = randomRatio2.ItemQualityId;
					list.Add(warbandContactData.ItemTypeId);
					global::Item item = global::Item.GetRandomLootableItem(global::PandoraSingleton<global::GameManager>.Instance.LocalTyche, list, itemQualityId, global::RuneMarkQualityId.NONE, this.WarbandData.AllegianceId, null, null, null);
					if (item.Id == global::ItemId.RECIPE_RANDOM_NORMAL || item.Id == global::ItemId.RECIPE_RANDOM_MASTERY)
					{
						item = new global::Item(this.GetUnclaimedRecipe(item.Id, true, global::ItemId.NONE));
					}
					if (item.Id != global::ItemId.NONE)
					{
						global::PandoraSingleton<global::HideoutManager>.Instance.WarbandChest.AddItem(item.Save, false);
						this.Logger.AddHistory(global::PandoraSingleton<global::HideoutManager>.Instance.Date.CurrentDate, global::EventLogger.LogEvent.CONTACT_ITEM, this.EncodeContactItemData(warbandContactData.Id, item.Id, (int)((item.Id != global::ItemId.GOLD) ? item.QualityData.Id : ((global::ItemQualityId)item.Amount))));
					}
				}
			}
		}
	}

	public int EncodeContactItemData(global::WarbandContactId contactId, global::ItemId itemId, int itemQualityId)
	{
		int num = (int)(contactId | (global::WarbandContactId)((int)itemId << 8));
		return num | itemQualityId << 24;
	}

	public void DecodeContactItemData(int value, out global::WarbandContactId contactId, out global::ItemId itemId, out int itemQualityId)
	{
		contactId = (global::WarbandContactId)(value & 255);
		itemId = (global::ItemId)(value >> 8 & 4095);
		itemQualityId = (value >> 24 & 255);
	}

	private void ApplyEnchantments(global::System.Collections.Generic.List<global::WarbandEnchantment> enchants)
	{
		for (int i = 0; i < enchants.Count; i++)
		{
			this.ApplyEnchantmentAttributes(enchants[i].Attributes);
			this.ApplyEnchantmentModifiers(enchants[i]);
		}
	}

	private void ApplyEnchantmentAttributes(global::System.Collections.Generic.List<global::WarbandEnchantmentAttributeData> attributeModifiers)
	{
		for (int i = 0; i < attributeModifiers.Count; i++)
		{
			this.AddToAttribute(attributeModifiers[i].WarbandAttributeId, attributeModifiers[i].Modifier);
		}
	}

	private void ApplyEnchantmentModifiers(global::WarbandEnchantment enchantment)
	{
		for (int i = 0; i < enchantment.SearchDensityModifiers.Count; i++)
		{
			global::WarbandEnchantmentSearchDensityModifierData warbandEnchantmentSearchDensityModifierData = enchantment.SearchDensityModifiers[i];
			int procMissionRatingId = (int)warbandEnchantmentSearchDensityModifierData.ProcMissionRatingId;
			int searchDensityId = (int)warbandEnchantmentSearchDensityModifierData.SearchDensityId;
			if (this.SearchDensityModifiers[procMissionRatingId].ContainsKey(searchDensityId))
			{
				global::System.Collections.Generic.Dictionary<int, int> dictionary2;
				global::System.Collections.Generic.Dictionary<int, int> dictionary = dictionary2 = this.SearchDensityModifiers[procMissionRatingId];
				int num;
				int key = num = searchDensityId;
				num = dictionary2[num];
				dictionary[key] = num + warbandEnchantmentSearchDensityModifierData.Modifier;
			}
			else
			{
				this.SearchDensityModifiers[procMissionRatingId].Add(searchDensityId, warbandEnchantmentSearchDensityModifierData.Modifier);
			}
		}
		for (int j = 0; j < enchantment.WyrdStoneDensityModifiers.Count; j++)
		{
			global::WarbandEnchantmentWyrdstoneDensityModifierData warbandEnchantmentWyrdstoneDensityModifierData = enchantment.WyrdStoneDensityModifiers[j];
			int procMissionRatingId2 = (int)warbandEnchantmentWyrdstoneDensityModifierData.ProcMissionRatingId;
			int wyrdstoneDensityId = (int)warbandEnchantmentWyrdstoneDensityModifierData.WyrdstoneDensityId;
			if (this.WyrdstoneDensityModifiers[procMissionRatingId2].ContainsKey(wyrdstoneDensityId))
			{
				global::System.Collections.Generic.Dictionary<int, int> dictionary4;
				global::System.Collections.Generic.Dictionary<int, int> dictionary3 = dictionary4 = this.WyrdstoneDensityModifiers[procMissionRatingId2];
				int num;
				int key2 = num = wyrdstoneDensityId;
				num = dictionary4[num];
				dictionary3[key2] = num + warbandEnchantmentWyrdstoneDensityModifierData.Modifier;
			}
			else
			{
				this.WyrdstoneDensityModifiers[procMissionRatingId2].Add(wyrdstoneDensityId, warbandEnchantmentWyrdstoneDensityModifierData.Modifier);
			}
		}
		for (int k = 0; k < enchantment.MarketEventModifiers.Count; k++)
		{
			this.MarketEventModifiers.Add(enchantment.MarketEventModifiers[k].MarketEventId, enchantment.MarketEventModifiers[k].Modifier);
		}
	}

	public int GetAttribute(global::WarbandAttributeId attributeId)
	{
		return this.attributes[(int)attributeId];
	}

	public float GetPercAttribute(global::WarbandAttributeId attributeId)
	{
		return (float)this.attributes[(int)attributeId] / 100f;
	}

	public void SetAttribute(global::WarbandAttributeId attributeId, int value)
	{
		this.attributes[(int)attributeId] = value;
		this.CheckStat(attributeId);
	}

	public void AddToAttribute(global::WarbandAttributeId attributeId, int increment)
	{
		this.attributes[(int)attributeId] += increment;
		global::WarbandAttributeData warbandAttributeData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::WarbandAttributeData>((int)attributeId);
		if (warbandAttributeData.CheckAchievement)
		{
			int num = this.attributes[(int)attributeId];
			this.warbandSave.stats.stats[(int)attributeId] = num;
			global::PandoraSingleton<global::GameManager>.Instance.Profile.CheckAchievement(this, attributeId, num);
			global::PandoraSingleton<global::GameManager>.Instance.Profile.AddToStat(attributeId, increment);
		}
	}

	public void CheckStat(global::WarbandAttributeId attributeId)
	{
	}

	public global::WarbandSave GetWarbandSave()
	{
		return this.warbandSave;
	}

	public bool IsUnitTypeUnlocked(global::UnitTypeId unitType)
	{
		global::System.Collections.Generic.List<global::WarbandRankJoinUnitTypeData> list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::WarbandRankJoinUnitTypeData>("fk_unit_type_id", unitType.ToIntString<global::UnitTypeId>());
		return list.Count == 1 && list[0].WarbandRankId <= (global::WarbandRankId)this.Rank;
	}

	public int GetActiveUnitIdCount(global::UnitId unitId, global::System.Collections.Generic.List<int> excludeSlots = null)
	{
		int num = 0;
		for (int i = 0; i < this.Units.Count; i++)
		{
			if (this.IsActiveWarbandSlot(this.Units[i].UnitSave.warbandSlotIndex) && this.Units[i].Id == unitId && (excludeSlots == null || !excludeSlots.Contains(this.Units[i].UnitSave.warbandSlotIndex)))
			{
				num++;
			}
		}
		return num;
	}

	public int GetRating()
	{
		int num = 0;
		for (int i = 0; i < this.Units.Count; i++)
		{
			if (this.Units[i].Active)
			{
				int rating = this.Units[i].GetRating();
				num += rating;
			}
		}
		return num;
	}

	public int GetSkirmishRating()
	{
		int num = 0;
		for (int i = 0; i < this.Units.Count; i++)
		{
			if (this.IsActiveWarbandSlot(this.Units[i].UnitSave.warbandSlotIndex))
			{
				int rating = this.Units[i].GetRating();
				num += rating;
			}
		}
		return num;
	}

	public int GetCartSize()
	{
		return this.GetWarbandRankData().CartSize;
	}

	public global::WarbandRankData GetWarbandRankData()
	{
		return global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::WarbandRankData>(this.Rank);
	}

	public global::WarbandRankData GetNextWarbandRankData()
	{
		return global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::WarbandRankData>(this.Rank + 1);
	}

	public int GetMaxRank()
	{
		if (this.maxRank == -1)
		{
			global::System.Collections.Generic.List<global::WarbandRankData> list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::WarbandRankData>();
			for (int i = 0; i < list.Count; i++)
			{
				if (this.maxRank < list[i].Rank)
				{
					this.maxRank = list[i].Rank;
				}
			}
		}
		return this.maxRank;
	}

	public void RefreshOutsiders()
	{
		global::System.Collections.Generic.List<global::UnitTypeId> list = new global::System.Collections.Generic.List<global::UnitTypeId>(global::Warband.OutsidersTypeIds);
		int num = 0;
		global::Tyche localTyche = global::PandoraSingleton<global::GameManager>.Instance.LocalTyche;
		for (int i = this.warbandSave.outsiders.Count - 1; i >= 0; i--)
		{
			if (!this.warbandSave.outsiders[i].isFreeOutsider)
			{
				if ((float)localTyche.Rand(0, 100) < global::Constant.GetFloat(global::ConstantId.HIRE_UNIT_REMOVE_OUTSIDER_RATIO) * 100f)
				{
					this.warbandSave.outsiders.RemoveAt(i);
					this.Outsiders.RemoveAt(i);
				}
				else
				{
					num++;
					for (int j = 0; j < list.Count; j++)
					{
						global::UnitTypeId unitTypeId = this.Outsiders[i].Data.UnitTypeId;
						if (list[j] == unitTypeId || (list[j] == global::UnitTypeId.HERO_1 && (unitTypeId == global::UnitTypeId.HERO_2 || unitTypeId == global::UnitTypeId.HERO_3)))
						{
							list.RemoveAt(j);
							break;
						}
					}
				}
			}
		}
		bool flag = false;
		global::System.Collections.Generic.List<global::UnitData> list2 = new global::System.Collections.Generic.List<global::UnitData>();
		for (int k = num; k < this.GetAttribute(global::WarbandAttributeId.OUTSIDERS_COUNT); k++)
		{
			if ((float)localTyche.Rand(0, 100) < global::Constant.GetFloat(global::ConstantId.HIRE_UNIT_ADD_OUTSIDER_RATIO) * 100f)
			{
				global::UnitTypeId unitTypeId2 = list[0];
				list.RemoveAt(0);
				if (unitTypeId2 == global::UnitTypeId.IMPRESSIVE && !this.IsUnitTypeUnlocked(global::UnitTypeId.IMPRESSIVE))
				{
					unitTypeId2 = global::UnitTypeId.HERO_1;
				}
				for (int l = 0; l < this.HireableOutsiderUnitIds.Count; l++)
				{
					global::UnitData unitData = this.HireableOutsiderUnitIds[l];
					global::UnitTypeId unitTypeId3 = unitData.UnitTypeId;
					if (unitTypeId3 == unitTypeId2 || (unitTypeId2 == global::UnitTypeId.HERO_1 && (unitTypeId3 == global::UnitTypeId.HERO_2 || unitTypeId3 == global::UnitTypeId.HERO_3)))
					{
						list2.Add(unitData);
					}
				}
				global::UnitData unitData2 = list2[localTyche.Rand(0, list2.Count)];
				list2.Clear();
				int unitRank = localTyche.Rand(1, this.HireableUnitTypeRank[(int)unitData2.UnitTypeId] + 1);
				global::Unit unit = global::Unit.GenerateHireUnit(unitData2.Id, this.Rank, unitRank);
				unit.UnitSave.isOutsider = true;
				this.Outsiders.Add(unit);
				this.warbandSave.outsiders.Add(unit.UnitSave);
				flag = true;
			}
		}
		if (flag)
		{
			global::PandoraSingleton<global::Pan>.Instance.Narrate("new_hiredsword");
		}
	}

	public void HireOutsider(global::Unit unit)
	{
		bool flag = this.warbandSave.outsiders.Remove(unit.UnitSave);
		this.Outsiders.Remove(unit);
		this.HireUnit(unit);
	}

	public void HireUnit(global::Unit unit)
	{
		this.warbandSave.units.Add(unit.UnitSave);
		this.Units.Add(unit);
		unit.Logger.AddHistory(this.warbandSave.currentDate, global::EventLogger.LogEvent.HIRE, 0);
		this.IncrementHireStat(unit);
	}

	public bool IsSlotAvailable(int slotIdx)
	{
		for (int i = 0; i < this.Units.Count; i++)
		{
			if (this.Units[i].UnitSave.warbandSlotIndex == slotIdx)
			{
				return false;
			}
		}
		return true;
	}

	private void IncrementHireStat(global::Unit unit)
	{
		switch (unit.GetUnitTypeId())
		{
		case global::UnitTypeId.LEADER:
			this.AddToAttribute(global::WarbandAttributeId.HIRE_LEADER, 1);
			break;
		case global::UnitTypeId.HENCHMEN:
			this.AddToAttribute(global::WarbandAttributeId.HIRE_HENCHMEN, 1);
			break;
		case global::UnitTypeId.IMPRESSIVE:
			this.AddToAttribute(global::WarbandAttributeId.HIRE_IMPRESSIVE, 1);
			break;
		case global::UnitTypeId.HERO_1:
		case global::UnitTypeId.HERO_2:
		case global::UnitTypeId.HERO_3:
			this.AddToAttribute(global::WarbandAttributeId.HIRE_HERO, 1);
			break;
		}
		this.AddToAttribute(global::WarbandAttributeId.TOTAL_HIRE_UNIT, 1);
		if (unit.UnitSave.isOutsider)
		{
			this.AddToAttribute(global::WarbandAttributeId.TOTAL_HIRE_OUTSIDER, 1);
		}
	}

	public global::System.Collections.Generic.List<global::Item> Disband(global::Unit unit)
	{
		this.warbandSave.units.Remove(unit.UnitSave);
		this.warbandSave.oldUnits.Add(unit.UnitSave.stats);
		this.Units.Remove(unit);
		return unit.UnequipAllItems();
	}

	public int GetTotalUpkeepOwned()
	{
		int num = 0;
		for (int i = 0; i < this.Units.Count; i++)
		{
			num += this.Units[i].GetUpkeepOwned();
		}
		return num;
	}

	public void PayAllUpkeepOwned()
	{
		for (int i = 0; i < this.Units.Count; i++)
		{
			this.Units[i].PayUpkeepOwned();
		}
	}

	public int GetTotalTreatmentOwned()
	{
		int num = 0;
		for (int i = 0; i < this.Units.Count; i++)
		{
			num += this.GetUnitTreatmentCost(this.Units[i]);
		}
		return num;
	}

	public void PayAllTreatmentOwned()
	{
		for (int i = 0; i < this.Units.Count; i++)
		{
			if (!this.Units[i].UnitSave.injuryPaid)
			{
				this.Units[i].TreatmentPaid();
			}
		}
	}

	public string GetBannerName()
	{
		return global::Warband.GetBannerName(this.Id);
	}

	public static string GetBannerName(global::WarbandId warbandId)
	{
		global::WarbandData warbandData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::WarbandData>((int)warbandId);
		string text = warbandData.Wagon;
		text = text.Substring(5);
		return "banner" + text;
	}

	public string GetIdolName()
	{
		global::WarbandData warbandData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::WarbandData>((int)this.Id);
		string text = warbandData.Wagon;
		if (!string.IsNullOrEmpty(text))
		{
			text = text.Substring(5);
			return "idol" + text;
		}
		return string.Empty;
	}

	public string GetMapName()
	{
		global::WarbandData warbandData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::WarbandData>((int)this.Id);
		string text = warbandData.Wagon;
		text = text.Substring(5);
		return "map_table" + text;
	}

	public string GetMapPawnName()
	{
		global::WarbandData warbandData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::WarbandData>((int)this.Id);
		string text = warbandData.Wagon;
		text = text.Substring(5);
		return "map_pawn" + text;
	}

	public static global::UnityEngine.Sprite GetIcon(global::WarbandId warbandId)
	{
		global::UnityEngine.Sprite sprite = global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadResource<global::UnityEngine.Sprite>(string.Format("warband/{0}_large", warbandId.ToLowerString()), false);
		if (sprite == null)
		{
			global::PandoraDebug.LogWarning("Cannot load warband icon : warband/" + warbandId.ToLowerString() + "_large", "uncategorised", null);
		}
		return sprite;
	}

	public static global::UnityEngine.Sprite GetFlagIcon(global::WarbandId warbandId, bool defeated = false)
	{
		if (defeated)
		{
			return global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadResource<global::UnityEngine.Sprite>("flags/flag_" + warbandId.ToLowerString() + "_defeated", false);
		}
		return global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadResource<global::UnityEngine.Sprite>("flags/flag_" + warbandId.ToLowerString(), false);
	}

	public static string GetLocalizedName(global::WarbandId warbandId)
	{
		return global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("com_wb_type_" + warbandId.ToLowerString());
	}

	public global::WarbandRankSlotData GetWarbandSlots()
	{
		return global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::WarbandRankSlotData>("fk_warband_rank_id", this.Rank.ToString())[0];
	}

	public int GetNbMaxReserveSlot()
	{
		global::WarbandRankSlotData warbandSlots = this.GetWarbandSlots();
		return warbandSlots.Reserve;
	}

	public int GetNbReserveUnits()
	{
		int num = 0;
		for (int i = 0; i < this.Units.Count; i++)
		{
			if (this.Units[i].UnitSave.warbandSlotIndex >= 12)
			{
				if (this.Units[i].IsImpressive)
				{
					num += 2;
				}
				else
				{
					num++;
				}
			}
		}
		return num;
	}

	public int GetNbMaxActiveSlots()
	{
		global::WarbandRankSlotData warbandSlots = this.GetWarbandSlots();
		return warbandSlots.Leader + warbandSlots.Hero + warbandSlots.Impressive + warbandSlots.Henchman;
	}

	public int GetNbActiveUnits(bool impressiveCountFor2 = true)
	{
		int num = 0;
		for (int i = 0; i < this.Units.Count; i++)
		{
			if (this.Units[i].Active)
			{
				if (impressiveCountFor2 && this.Units[i].IsImpressive)
				{
					num += 2;
				}
				else
				{
					num++;
				}
			}
		}
		return num;
	}

	public int GetNbInactiveUnits(bool impressiveCountFor2 = true)
	{
		int num = 0;
		for (int i = 0; i < this.Units.Count; i++)
		{
			if (this.Units[i].GetActiveStatus() != global::UnitActiveStatusId.AVAILABLE)
			{
				if (impressiveCountFor2 && this.Units[i].IsImpressive)
				{
					num += 2;
				}
				else
				{
					num++;
				}
			}
		}
		return num;
	}

	public bool CanHireMoreUnit(bool isImpressive)
	{
		bool flag = this.GetWarbandSave().lateShipmentCount >= global::Constant.GetInt(global::ConstantId.MAX_SHIPMENT_FAIL);
		if (flag)
		{
			return false;
		}
		int num = 0;
		for (int i = 0; i < this.Units.Count; i++)
		{
			if (this.Units[i].IsImpressive)
			{
				num += 2;
			}
			else
			{
				num++;
			}
		}
		int num2 = this.GetNbMaxActiveSlots() + this.GetNbMaxReserveSlot();
		return (!isImpressive) ? (num + 1 <= num2) : (num + 2 <= num2);
	}

	public int GetNbWarbandSlot(global::UnitTypeId warbandSlotId)
	{
		global::WarbandRankSlotData warbandSlots = this.GetWarbandSlots();
		int result = 0;
		switch (warbandSlotId)
		{
		case global::UnitTypeId.LEADER:
			result = warbandSlots.Leader;
			break;
		case global::UnitTypeId.HENCHMEN:
			result = warbandSlots.Henchman;
			break;
		case global::UnitTypeId.IMPRESSIVE:
			result = warbandSlots.Impressive;
			break;
		case global::UnitTypeId.HERO_1:
		case global::UnitTypeId.HERO_2:
		case global::UnitTypeId.HERO_3:
			result = warbandSlots.Hero;
			break;
		}
		return result;
	}

	public bool CanPlaceUnitAt(global::Unit unit, int toIndex)
	{
		global::UnitTypeId unitTypeId = unit.GetUnitTypeId();
		bool result = false;
		if (toIndex < 20)
		{
			switch (unitTypeId)
			{
			case global::UnitTypeId.LEADER:
				result = (toIndex == 2 || toIndex >= 12);
				break;
			case global::UnitTypeId.HENCHMEN:
				result = (toIndex >= 3);
				break;
			case global::UnitTypeId.IMPRESSIVE:
				if (toIndex >= 5 && toIndex < 7 && (toIndex - 5) % 2 == 0)
				{
					result = true;
				}
				else if (toIndex >= 12 && (toIndex - 12) % 2 == 0)
				{
					result = true;
				}
				break;
			case global::UnitTypeId.HERO_1:
			case global::UnitTypeId.HERO_2:
			case global::UnitTypeId.HERO_3:
				result = ((toIndex >= 3 && toIndex < 7) || toIndex >= 12);
				break;
			}
		}
		else
		{
			result = true;
		}
		return result;
	}

	public bool IsUnitCountExceeded(global::Unit unit, int excludeSlot = -1)
	{
		this.tempList[0] = excludeSlot;
		return this.GetActiveUnitIdCount(unit.Id, this.tempList) >= unit.Data.MaxCount;
	}

	public bool IsActiveWarbandSlot(int warbandSlotIndex)
	{
		return warbandSlotIndex >= 2 && warbandSlotIndex < 12;
	}

	public void FindSuitableSlot(global::Unit unit, bool checkCurrent)
	{
		if (checkCurrent && unit.UnitSave.warbandSlotIndex >= 0 && unit.UnitSave.warbandSlotIndex < 20)
		{
			global::Unit unitAtWarbandSlot = global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.GetUnitAtWarbandSlot(unit.UnitSave.warbandSlotIndex, false);
			if (unitAtWarbandSlot == null || unitAtWarbandSlot == unit)
			{
				return;
			}
			if (unit.IsImpressive && global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.GetUnitAtWarbandSlot(unit.UnitSave.warbandSlotIndex + 1, false) == null)
			{
				return;
			}
		}
		global::WarbandRankSlotData warbandSlots = this.GetWarbandSlots();
		int emptyWarbandSlotIndex = this.GetEmptyWarbandSlotIndex(global::WarbandSlotTypeId.RESERVE, warbandSlots.Reserve, unit.IsImpressive);
		if (emptyWarbandSlotIndex == -1)
		{
			switch (unit.GetUnitTypeId())
			{
			case global::UnitTypeId.LEADER:
				emptyWarbandSlotIndex = this.GetEmptyWarbandSlotIndex(global::WarbandSlotTypeId.LEADER, warbandSlots.Leader, unit.IsImpressive);
				break;
			case global::UnitTypeId.HENCHMEN:
				emptyWarbandSlotIndex = this.GetEmptyWarbandSlotIndex(global::WarbandSlotTypeId.HENCHMEN, warbandSlots.Henchman, unit.IsImpressive);
				if (emptyWarbandSlotIndex == -1)
				{
					emptyWarbandSlotIndex = this.GetEmptyWarbandSlotIndex(global::WarbandSlotTypeId.HERO, warbandSlots.Hero, unit.IsImpressive);
				}
				if (emptyWarbandSlotIndex == -1)
				{
					emptyWarbandSlotIndex = this.GetEmptyWarbandSlotIndex(global::WarbandSlotTypeId.HERO_IMPRESSIVE, warbandSlots.Impressive, unit.IsImpressive);
				}
				break;
			case global::UnitTypeId.IMPRESSIVE:
				emptyWarbandSlotIndex = this.GetEmptyWarbandSlotIndex(global::WarbandSlotTypeId.HERO_IMPRESSIVE, warbandSlots.Impressive, unit.IsImpressive);
				break;
			case global::UnitTypeId.HERO_1:
			case global::UnitTypeId.HERO_2:
			case global::UnitTypeId.HERO_3:
				emptyWarbandSlotIndex = this.GetEmptyWarbandSlotIndex(global::WarbandSlotTypeId.HERO, warbandSlots.Hero, unit.IsImpressive);
				if (emptyWarbandSlotIndex == -1)
				{
					emptyWarbandSlotIndex = this.GetEmptyWarbandSlotIndex(global::WarbandSlotTypeId.HERO_IMPRESSIVE, warbandSlots.Impressive, unit.IsImpressive);
				}
				break;
			}
		}
		if (emptyWarbandSlotIndex != -1)
		{
			unit.UnitSave.warbandSlotIndex = emptyWarbandSlotIndex;
		}
	}

	private int GetEmptyWarbandSlotIndex(global::WarbandSlotTypeId warbandSlotTypeId, int slotCount, bool isImpressive)
	{
		for (int i = (int)warbandSlotTypeId; i < (int)(warbandSlotTypeId + slotCount); i++)
		{
			global::Unit unitAtWarbandSlot = this.GetUnitAtWarbandSlot(i, false);
			if (unitAtWarbandSlot == null)
			{
				if (!isImpressive)
				{
					return i;
				}
				if ((i - (int)warbandSlotTypeId) % 2 == 0 && this.GetUnitAtWarbandSlot(i + 1, false) == null)
				{
					return i;
				}
			}
			else if (unitAtWarbandSlot.IsImpressive)
			{
				i++;
			}
		}
		return -1;
	}

	public global::Unit GetUnitAtWarbandSlot(int index, bool includeUnavailable = false)
	{
		for (int i = 0; i < this.Units.Count; i++)
		{
			if (this.Units[i].UnitSave.warbandSlotIndex == index && (includeUnavailable || this.Units[i].GetActiveStatus() == global::UnitActiveStatusId.AVAILABLE))
			{
				return this.Units[i];
			}
		}
		return null;
	}

	public global::System.Collections.Generic.List<global::ItemId> GetMarketAdditionalItems(global::ItemCategoryId itemCategoryId)
	{
		this.addtionnalMarketItems.Clear();
		global::DataFactory instance = global::PandoraSingleton<global::DataFactory>.Instance;
		string field = "fk_item_category_id";
		int num = (int)itemCategoryId;
		global::System.Collections.Generic.List<global::ItemTypeData> list = instance.InitData<global::ItemTypeData>(field, num.ToString());
		for (int i = 0; i < this.AddtionnalMarketItems.Count; i++)
		{
			global::ItemData itemData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::ItemData>((int)this.AddtionnalMarketItems[i]);
			for (int j = 0; j < list.Count; j++)
			{
				if (list[j].Id == itemData.ItemTypeId)
				{
					this.addtionnalMarketItems.Add(itemData.Id);
					break;
				}
			}
		}
		return this.addtionnalMarketItems;
	}

	public global::System.Collections.Generic.List<global::ItemId> GetAllowedItemIds()
	{
		this.allowedItemIds.Clear();
		for (int i = 0; i < this.HireableUnitIds.Count; i++)
		{
			global::System.Collections.Generic.List<global::ItemUnitData> list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::ItemUnitData>("fk_unit_id", ((int)this.HireableUnitIds[i]).ToString());
			for (int j = 0; j < list.Count; j++)
			{
				if (list[j].ItemId != global::ItemId.NONE && this.allowedItemIds.IndexOf(list[j].ItemId, global::ItemIdComparer.Instance) == -1)
				{
					this.allowedItemIds.Add(list[j].ItemId);
				}
			}
		}
		return this.allowedItemIds;
	}

	public global::WarbandData GetNextNotFacedWarband(global::Tyche tyche)
	{
		global::System.Collections.Generic.List<global::WarbandData> list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::WarbandData>("basic", "1");
		global::System.Collections.Generic.List<global::WarbandData> list2 = new global::System.Collections.Generic.List<global::WarbandData>(list);
		if (this.warbandSave.warbandFaced == 0)
		{
			for (int i = 0; i < list2.Count; i++)
			{
				this.warbandSave.warbandFaced |= 1 << i;
			}
		}
		else
		{
			for (int j = list2.Count - 1; j >= 0; j--)
			{
				if ((this.warbandSave.warbandFaced >> j & 1) == 0)
				{
					list2.RemoveAt(j);
				}
			}
		}
		int index = tyche.Rand(0, list2.Count);
		int num = list.IndexOf(list2[index]);
		this.warbandSave.warbandFaced &= ~(1 << num);
		return list[num];
	}

	public global::System.Collections.Generic.List<global::Unit> GetUnavailableUnits()
	{
		global::System.Collections.Generic.List<global::Unit> list = new global::System.Collections.Generic.List<global::Unit>();
		for (int i = 0; i < this.Units.Count; i++)
		{
			if (this.Units[i].GetActiveStatus() != global::UnitActiveStatusId.AVAILABLE || this.Units[i].UnitSave.warbandSlotIndex >= 20 || this.Units[i].UnitSave.warbandSlotIndex == -1)
			{
				list.Add(this.Units[i]);
			}
		}
		return list;
	}

	public int GetScoutCost(global::ScoutPriceData scout)
	{
		return global::UnityEngine.Mathf.Max(0, (int)((float)(scout.Price * this.GetAttribute(global::WarbandAttributeId.SCOUT_COST_PERC)) / 100f));
	}

	public int GetUnitHireCost(global::Unit unit)
	{
		return global::UnityEngine.Mathf.Max(0, (int)((float)(unit.GetHireCost() * this.GetAttribute(global::WarbandAttributeId.HIRE_COST_PERC)) / 100f));
	}

	public int GetUnitTreatmentCost(global::Unit unit)
	{
		return global::UnityEngine.Mathf.Max(0, unit.GetTreatmentCost() + this.GetAttribute(global::WarbandAttributeId.UPKEEP_WOUNDED));
	}

	public int GetItemSellPrice(global::Item item)
	{
		return global::UnityEngine.Mathf.Clamp((int)((float)(item.PriceSold * this.GetAttribute(global::WarbandAttributeId.SELL_PRICE_PERC)) / 100f), 0, this.GetItemBuyPrice(item));
	}

	public int GetItemBuyPrice(global::Item item)
	{
		return global::UnityEngine.Mathf.Max(0, (int)((float)(item.PriceBuy * this.GetAttribute(global::WarbandAttributeId.BUY_COST_PERC)) / 100f));
	}

	public int GetRuneMarkBuyPrice(global::RuneMark runeMark)
	{
		return global::UnityEngine.Mathf.Max(0, (int)((float)(runeMark.Cost * this.GetAttribute(global::WarbandAttributeId.BUY_COST_PERC)) / 100f));
	}

	public int GetSkillLearnPrice(global::SkillData skillData, global::Unit unit)
	{
		if (global::PandoraSingleton<global::HideoutManager>.Instance.IsTrainingOutsider)
		{
			return 0;
		}
		global::SkillLineData baseSkillLine = global::SkillHelper.GetBaseSkillLine(skillData, unit.Id);
		if (baseSkillLine.WarbandAttributeIdPriceModifier != global::WarbandAttributeId.NONE)
		{
			return (int)((float)(skillData.Cost * (this.GetAttribute(global::WarbandAttributeId.SKILL_SPELL_LEARN_COST_PERC) + this.GetAttribute(baseSkillLine.WarbandAttributeIdPriceModifier))) / 100f);
		}
		return global::UnityEngine.Mathf.Max(0, (int)((float)(skillData.Cost * this.GetAttribute(global::WarbandAttributeId.SKILL_SPELL_LEARN_COST_PERC)) / 100f));
	}

	public void SetHideoutTutoShown(global::HideoutManager.HideoutTutoType type)
	{
		this.warbandSave.hideoutTutos |= (uint)type;
	}

	public bool HasShownHideoutTuto(global::HideoutManager.HideoutTutoType type)
	{
		return (this.warbandSave.hideoutTutos & (uint)type) != 0U;
	}

	public global::System.Collections.Generic.List<global::UnitSave> GetExhibitionUnits()
	{
		return new global::System.Collections.Generic.List<global::UnitSave>();
	}

	public bool IsSkirmishAvailable(out string reason)
	{
		reason = string.Empty;
		if (this.warbandSave.currentDate == global::Constant.GetInt(global::ConstantId.CAL_DAY_START) && this.Units.Count == 1 && this.warbandSave.scoutsSent < 0)
		{
			reason = "na_hideout_new_game";
			return false;
		}
		if (!this.HasLeader(false))
		{
			reason = "na_hideout_leader";
			return false;
		}
		if (this.GetUnitsCount(false) < global::Constant.GetInt(global::ConstantId.MIN_MISSION_UNITS))
		{
			reason = "na_hideout_min_unit";
			return false;
		}
		return true;
	}

	public bool IsContestAvailable(out string reason)
	{
		reason = string.Empty;
		if (this.warbandSave.lateShipmentCount >= global::Constant.GetInt(global::ConstantId.MAX_SHIPMENT_FAIL))
		{
			reason = "na_hideout_late_shipment_count";
			return false;
		}
		if (this.warbandSave.scoutsSent < 0)
		{
			reason = "na_hideout_post_mission";
			return false;
		}
		if (!this.HasLeader(true))
		{
			reason = "na_hideout_active_leader";
			return false;
		}
		if (this.GetUnitsCount(true) < global::Constant.GetInt(global::ConstantId.MIN_MISSION_UNITS))
		{
			reason = "na_hideout_min_active_unit";
			return false;
		}
		return true;
	}

	public bool HasLeader(bool needToBeActive)
	{
		for (int i = 0; i < this.Units.Count; i++)
		{
			if (this.Units[i].GetUnitTypeId() == global::UnitTypeId.LEADER && (!needToBeActive || this.Units[i].GetActiveStatus() == global::UnitActiveStatusId.AVAILABLE) && this.Units[i].UnitSave.warbandSlotIndex < 12)
			{
				return true;
			}
		}
		return false;
	}

	public int GetUnitsCount(bool needToBeActive)
	{
		int num = 0;
		for (int i = 0; i < this.Units.Count; i++)
		{
			if ((!needToBeActive || this.Units[i].GetActiveStatus() == global::UnitActiveStatusId.AVAILABLE) && this.Units[i].UnitSave.warbandSlotIndex < 12)
			{
				num++;
			}
		}
		return num;
	}

	public void UnequipAllUnits()
	{
		foreach (global::Unit unit in this.Units)
		{
			global::System.Collections.Generic.List<global::Item> items = unit.UnequipAllItems();
			global::PandoraSingleton<global::HideoutManager>.Instance.WarbandChest.AddItems(items);
		}
	}

	private static readonly global::UnitTypeId[] OutsidersTypeIds = new global::UnitTypeId[]
	{
		global::UnitTypeId.HENCHMEN,
		global::UnitTypeId.HERO_1,
		global::UnitTypeId.LEADER,
		global::UnitTypeId.IMPRESSIVE,
		global::UnitTypeId.HENCHMEN,
		global::UnitTypeId.HERO_1,
		global::UnitTypeId.HERO_1,
		global::UnitTypeId.LEADER,
		global::UnitTypeId.HENCHMEN,
		global::UnitTypeId.HERO_1
	};

	private readonly global::System.Collections.Generic.List<global::WarbandAttributeData> attributeDataList;

	private readonly int[] attributes;

	public bool rankGained;

	private readonly global::WarbandSave warbandSave;

	private int maxRank = -1;

	private global::System.Collections.Generic.List<global::ItemId> allowedItemIds = new global::System.Collections.Generic.List<global::ItemId>();

	private global::System.Collections.Generic.List<global::ItemId> addtionnalMarketItems = new global::System.Collections.Generic.List<global::ItemId>();

	private global::System.Collections.Generic.List<global::ItemId> unclaimedRecipe = new global::System.Collections.Generic.List<global::ItemId>();

	private readonly global::System.Collections.Generic.List<int> tempList = new global::System.Collections.Generic.List<int>(1)
	{
		-1
	};
}
