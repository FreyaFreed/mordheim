using System;
using System.Collections.Generic;
using UnityEngine;

public class Progressor
{
	public void UpdateUnitStats(global::MissionEndUnitSave endUnit, global::Unit unit)
	{
		foreach (global::System.Collections.Generic.KeyValuePair<int, int> keyValuePair in endUnit.unitSave.stats.stats)
		{
			this.SetUnitStat(unit, (global::AttributeId)keyValuePair.Key, keyValuePair.Value, true);
		}
	}

	public void EndGameUnitProgress(global::MissionEndUnitSave endUnit, global::Unit unit)
	{
		for (int i = endUnit.items.Count - 1; i >= 6; i--)
		{
			if (endUnit.items[i].TypeData.Id == global::ItemTypeId.QUEST_ITEM)
			{
				endUnit.items[i] = new global::Item(global::ItemId.NONE, global::ItemQualityId.NORMAL);
			}
		}
		global::MissionEndDataSave endMission = global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.GetWarbandSave().endMission;
		if (endMission.VictoryType == global::VictoryTypeId.LOSS && endUnit.status == global::UnitStateId.OUT_OF_ACTION)
		{
			this.CalculateCostofLosing(endUnit);
		}
		for (int j = 0; j < unit.Items.Count; j++)
		{
			endUnit.lostItems.Add(new global::Item(global::ItemId.NONE, global::ItemQualityId.NORMAL));
		}
		for (int k = unit.Items.Count - 1; k >= 0; k--)
		{
			bool flag = false;
			int num = 0;
			while (num < endUnit.items.Count && !flag)
			{
				if (endUnit.items[num].IsSame(unit.Items[k]))
				{
					flag = true;
					endUnit.items.RemoveAt(num);
				}
				num++;
			}
			if (!flag)
			{
				global::System.Collections.Generic.List<global::Item> list = unit.EquipItem((global::UnitSlotId)k, new global::Item(global::ItemId.NONE, global::ItemQualityId.NORMAL), true);
				for (int l = 1; l < list.Count; l++)
				{
					global::PandoraSingleton<global::HideoutManager>.Instance.WarbandChest.AddItem(list[l].Save, false);
				}
			}
		}
		if (endMission.missionSave.VictoryTypeId != 2 || endMission.VictoryType != global::VictoryTypeId.LOSS)
		{
			for (int m = 0; m < endUnit.items.Count; m++)
			{
				endMission.wagonItems.AddItem(endUnit.items[m].Save, false);
			}
		}
		for (int n = 0; n < endUnit.enchantments.Count; n++)
		{
			unit.AddEnchantment(endUnit.enchantments[n].enchantId, unit, false, true, global::AllegianceId.NONE);
		}
		unit.SetStatus(endUnit.status);
		endUnit.dead = !this.CalculateInjuries(global::PandoraSingleton<global::HideoutManager>.Instance.Date.CurrentDate, unit, endUnit);
		global::PandoraDebug.LogDebug("Unit Dead After Injuries = " + endUnit.dead, "uncategorised", null);
		if (endUnit.injuries.Count > 0)
		{
			this.AddInjuryTime(unit, endUnit.injuries[endUnit.injuries.Count - 1].Id);
		}
		for (int num2 = 0; num2 < endUnit.enchantments.Count; num2++)
		{
			unit.RemoveEnchantments(endUnit.enchantments[num2].enchantId);
		}
		unit.SetStatus(global::UnitStateId.NONE);
		if (endUnit.dead)
		{
			for (int num3 = 0; num3 < unit.Items.Count; num3++)
			{
				if (unit.Items[num3].Id != global::ItemId.NONE)
				{
					global::PandoraSingleton<global::HideoutManager>.Instance.WarbandChest.AddItem(unit.Items[num3].Save, false);
				}
			}
			unit.Items.Clear();
		}
		if (!endUnit.dead)
		{
			this.CalculateXP(endUnit, unit);
			for (int num4 = 0; num4 < endUnit.mutations.Count; num4++)
			{
				unit.Logger.AddHistory(global::PandoraSingleton<global::HideoutManager>.Instance.Date.CurrentDate, global::EventLogger.LogEvent.MUTATION, (int)endUnit.mutations[num4].Data.Id);
			}
			if (endUnit.mutations.Count > 0)
			{
				global::PandoraSingleton<global::Hephaestus>.Instance.IncrementStat(global::Hephaestus.StatId.MUTATIONS, 1);
			}
		}
		if (!endUnit.dead)
		{
			this.UpdateUpkeep(endUnit, unit, global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.GetWarbandSave().endMission.VictoryType);
		}
		this.UpdateUnitStats(endUnit, unit);
		unit.UpdateAttributes();
	}

	public void CalculateCostofLosing(global::MissionEndUnitSave endUnit)
	{
		if (global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.GetWarbandSave().endMission.VictoryType != global::VictoryTypeId.LOSS)
		{
			return;
		}
		global::System.Collections.Generic.List<global::CostOfLosingData> datas = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::CostOfLosingData>();
		global::Tyche localTyche = global::PandoraSingleton<global::GameManager>.Instance.LocalTyche;
		global::CostOfLosingData randomRatio = global::CostOfLosingData.GetRandomRatio(datas, localTyche, null);
		endUnit.costOfLosingId = randomRatio.Id;
		for (int i = endUnit.items.Count - 1; i >= 6; i--)
		{
			if (endUnit.items[i].Id != global::ItemId.NONE)
			{
				endUnit.items[i] = new global::Item(global::ItemId.NONE, global::ItemQualityId.NORMAL);
			}
		}
		if (randomRatio.MainWeapons)
		{
			if (endUnit.items[2].Id != global::ItemId.NONE)
			{
				endUnit.items[2] = new global::Item(global::ItemId.NONE, global::ItemQualityId.NORMAL);
			}
			if (endUnit.items[3].Id != global::ItemId.NONE)
			{
				endUnit.items[3] = new global::Item(global::ItemId.NONE, global::ItemQualityId.NORMAL);
			}
		}
		if (randomRatio.SecondaryWeapons)
		{
			if (endUnit.items[4].Id != global::ItemId.NONE)
			{
				endUnit.items[4] = new global::Item(global::ItemId.NONE, global::ItemQualityId.NORMAL);
			}
			if (endUnit.items[5].Id != global::ItemId.NONE)
			{
				endUnit.items[5] = new global::Item(global::ItemId.NONE, global::ItemQualityId.NORMAL);
			}
		}
		if (randomRatio.Armor && endUnit.items[1].Id != global::ItemId.NONE)
		{
			endUnit.items[1] = new global::Item(global::ItemId.NONE, global::ItemQualityId.NORMAL);
		}
		if (randomRatio.Helmet && endUnit.items[0].Id != global::ItemId.NONE)
		{
			endUnit.items[0] = new global::Item(global::ItemId.NONE, global::ItemQualityId.NORMAL);
		}
		if (randomRatio.OpenWound)
		{
			global::EndUnitEnchantment item = default(global::EndUnitEnchantment);
			item.enchantId = global::EnchantmentId.OPEN_WOUND;
			endUnit.enchantments.Add(item);
		}
	}

	public bool CalculateInjuries(int currentDate, global::Unit unit, global::MissionEndUnitSave endUnit)
	{
		endUnit.injuries.Clear();
		bool flag = true;
		global::System.Collections.Generic.List<global::Item> list = new global::System.Collections.Generic.List<global::Item>();
		if (unit.Status != global::UnitStateId.OUT_OF_ACTION)
		{
			if (unit.HasEnchantment(global::EnchantmentId.OPEN_WOUND))
			{
				global::InjuryData injuryData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::InjuryData>(31);
				flag = unit.AddInjury(injuryData, currentDate, list, false, -1);
				endUnit.injuries.Add(injuryData);
			}
		}
		else
		{
			global::System.Collections.Generic.List<global::InjuryId> list2 = new global::System.Collections.Generic.List<global::InjuryId>();
			int num = 1;
			while (num > 0 && flag)
			{
				global::InjuryData injuryData2 = this.RollInjury(list2, unit);
				if (injuryData2.Id == global::InjuryId.MULTIPLE_INJURIES)
				{
					global::PandoraSingleton<global::Hephaestus>.Instance.UnlockAchievement(global::Hephaestus.TrophyId.MULTIPLE_INJURIES);
					list2.AddRange(global::Progressor.INJURY_EXCLUDES);
					num = 3;
				}
				else
				{
					endUnit.injuries.Add(injuryData2);
					if (injuryData2.Id != global::InjuryId.FULL_RECOVERY)
					{
						flag = unit.AddInjury(injuryData2, currentDate, list, false, -1);
						if (flag)
						{
							global::PandoraSingleton<global::Hephaestus>.Instance.IncrementStat(global::Hephaestus.StatId.MY_TOTAL_INJURIES, 1);
						}
					}
					num--;
					list2.Add(injuryData2.Id);
				}
			}
		}
		global::PandoraSingleton<global::HideoutManager>.Instance.WarbandChest.AddItems(list);
		global::PandoraDebug.LogDebug(string.Concat(new object[]
		{
			"CalculateInjuries return = ",
			flag,
			" for ",
			unit.Name
		}), "uncategorised", null);
		return flag;
	}

	public global::InjuryData RollInjury(global::System.Collections.Generic.List<global::InjuryId> excludes, global::Unit unit)
	{
		global::System.Collections.Generic.Dictionary<global::InjuryId, int> injuryModifiers = unit.GetInjuryModifiers();
		global::System.Collections.Generic.List<global::InjuryData> possibleInjuries = unit.GetPossibleInjuries(excludes, unit, injuryModifiers);
		return global::InjuryData.GetRandomRatio(possibleInjuries, global::PandoraSingleton<global::GameManager>.Instance.LocalTyche, injuryModifiers);
	}

	private void CalculateXP(global::MissionEndUnitSave endUnit, global::Unit unit)
	{
		if (unit.IsMaxRank())
		{
			endUnit.isMaxRank = true;
			return;
		}
		global::MissionEndDataSave endMission = global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.GetWarbandSave().endMission;
		int num = 0;
		endUnit.XPs.Clear();
		endUnit.XPs.Add(new global::System.Collections.Generic.KeyValuePair<int, string>(global::Constant.GetInt(global::ConstantId.UNIT_XP_SURVIVED), "end_game_survive"));
		num += global::Constant.GetInt(global::ConstantId.UNIT_XP_SURVIVED);
		if (endMission.primaryObjectiveCompleted)
		{
			endUnit.XPs.Add(new global::System.Collections.Generic.KeyValuePair<int, string>(global::Constant.GetInt(global::ConstantId.UNIT_XP_OBJECTIVE), "end_game_objective"));
			num += global::Constant.GetInt(global::ConstantId.UNIT_XP_OBJECTIVE);
			if (endMission.won)
			{
				num += global::Constant.GetInt(global::ConstantId.UNIT_XP_WINNING);
				endUnit.XPs.Add(new global::System.Collections.Generic.KeyValuePair<int, string>(global::Constant.GetInt(global::ConstantId.UNIT_XP_WINNING), "mission_victory_decisive"));
			}
		}
		if (endMission.playerMVUIdx == endUnit.unitSave.warbandSlotIndex)
		{
			num += global::Constant.GetInt(global::ConstantId.UNIT_XP_MVU);
			endUnit.XPs.Add(new global::System.Collections.Generic.KeyValuePair<int, string>(global::Constant.GetInt(global::ConstantId.UNIT_XP_MVU), "menu_mvu"));
		}
		int underdogBonus = this.GetUnderdogBonus(endMission);
		if (underdogBonus > 0)
		{
			num += underdogBonus;
			endUnit.XPs.Add(new global::System.Collections.Generic.KeyValuePair<int, string>(underdogBonus, "end_game_underdog"));
		}
		int num2 = 0;
		unit.UnitSave.stats.stats.TryGetValue(141, out num2);
		int num3 = endUnit.GetAttribute(global::AttributeId.TOTAL_KILL) - num2;
		if (num3 > 0)
		{
			num += num3 * global::Constant.GetInt(global::ConstantId.UNIT_XP_OUT_OF_ACTION);
			endUnit.XPs.Add(new global::System.Collections.Generic.KeyValuePair<int, string>(num3 * global::Constant.GetInt(global::ConstantId.UNIT_XP_OUT_OF_ACTION), "end_game_ooa"));
		}
		num2 = 0;
		unit.UnitSave.stats.stats.TryGetValue(151, out num2);
		num3 = endUnit.GetAttribute(global::AttributeId.TOTAL_KILL_ROAMING) - num2;
		if (num3 > 0)
		{
			int num4 = num3 * global::Constant.GetInt(global::ConstantId.UNIT_XP_ROAMING_OUT_OF_ACTION);
			num += num4;
			endUnit.XPs.Add(new global::System.Collections.Generic.KeyValuePair<int, string>(num4, "end_game_ooa_roaming"));
		}
		for (int i = 0; i < endUnit.injuries.Count; i++)
		{
			if (endUnit.injuries[i].Id == global::InjuryId.NEAR_DEATH)
			{
				num += global::Constant.GetInt(global::ConstantId.UNIT_XP_NEAR_DEATH);
				endUnit.XPs.Add(new global::System.Collections.Generic.KeyValuePair<int, string>(global::Constant.GetInt(global::ConstantId.UNIT_XP_NEAR_DEATH), "injury_name_near_death"));
			}
			if (endUnit.injuries[i].Id == global::InjuryId.AMNESIA)
			{
				num += global::Constant.GetInt(global::ConstantId.UNIT_XP_AMNESIA);
				endUnit.XPs.Add(new global::System.Collections.Generic.KeyValuePair<int, string>(global::Constant.GetInt(global::ConstantId.UNIT_XP_AMNESIA), "injury_name_amnesia"));
			}
		}
		global::System.Collections.Generic.List<global::Item> list = new global::System.Collections.Generic.List<global::Item>();
		unit.AddXp(num, endUnit.advancements, endUnit.mutations, list, global::PandoraSingleton<global::HideoutManager>.Instance.Date.CurrentDate, -1);
		global::PandoraSingleton<global::HideoutManager>.Instance.WarbandChest.AddItems(list);
		if (endUnit.advancements.Count > 0)
		{
			global::PandoraSingleton<global::GameManager>.Instance.Profile.CheckAchievement(unit, global::AttributeId.RANK, unit.GetAttribute(global::AttributeId.RANK));
			global::UnitRankData unitRankData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::UnitRankData>((int)unit.UnitSave.rankId);
			if (unitRankData.Rank == global::Constant.GetInt(global::ConstantId.MAX_UNIT_RANK))
			{
				bool flag = true;
				for (int j = 0; j < unit.Injuries.Count; j++)
				{
					global::InjuryId id = unit.Injuries[j].Data.Id;
					if (id != global::InjuryId.LIGHT_WOUND && id != global::InjuryId.FULL_RECOVERY && id != global::InjuryId.NEAR_DEATH && id != global::InjuryId.AMNESIA)
					{
						flag = false;
						break;
					}
				}
				switch (unit.GetUnitTypeId())
				{
				case global::UnitTypeId.LEADER:
					global::PandoraSingleton<global::Hephaestus>.Instance.IncrementStat(global::Hephaestus.StatId.LEADER_RANK_10, 1);
					if (flag)
					{
						global::PandoraSingleton<global::Hephaestus>.Instance.UnlockAchievement(global::Hephaestus.TrophyId.LEADER_NO_INJURY);
					}
					break;
				case global::UnitTypeId.HENCHMEN:
					global::PandoraSingleton<global::Hephaestus>.Instance.IncrementStat(global::Hephaestus.StatId.HENCHMEN_RANK_10, 1);
					if (flag)
					{
						global::PandoraSingleton<global::Hephaestus>.Instance.UnlockAchievement(global::Hephaestus.TrophyId.HENCHMEN_NO_INJURY);
					}
					break;
				case global::UnitTypeId.IMPRESSIVE:
					global::PandoraSingleton<global::Hephaestus>.Instance.IncrementStat(global::Hephaestus.StatId.IMPRESSIVE_RANK_10, 1);
					if (flag)
					{
						global::PandoraSingleton<global::Hephaestus>.Instance.UnlockAchievement(global::Hephaestus.TrophyId.IMPRESSIVE_NO_INJURY);
					}
					break;
				case global::UnitTypeId.HERO_1:
				case global::UnitTypeId.HERO_2:
				case global::UnitTypeId.HERO_3:
					global::PandoraSingleton<global::Hephaestus>.Instance.IncrementStat(global::Hephaestus.StatId.HERO_RANK_10, 1);
					if (flag)
					{
						global::PandoraSingleton<global::Hephaestus>.Instance.UnlockAchievement(global::Hephaestus.TrophyId.HERO_NO_INJURY);
					}
					break;
				}
			}
		}
	}

	private int GetUnderdogBonus(global::MissionEndDataSave endMission)
	{
		int result = 0;
		if (endMission.ratingId > global::ProcMissionRatingId.NONE)
		{
			global::ProcMissionRatingData procMissionRatingData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::ProcMissionRatingData>((int)endMission.ratingId);
			result = procMissionRatingData.UnderdogXp;
		}
		return result;
	}

	private void UpdateUpkeep(global::MissionEndUnitSave endUnit, global::Unit unit, global::VictoryTypeId victory)
	{
		global::UnitCostData unitCost = unit.GetUnitCost();
		switch (victory)
		{
		case global::VictoryTypeId.LOSS:
			unit.AddToUpkeepOwned(unitCost.Defeat);
			goto IL_67;
		case global::VictoryTypeId.BATTLEGROUND:
		case global::VictoryTypeId.OBJECTIVE:
			unit.AddToUpkeepOwned(unitCost.PartialVictory);
			goto IL_67;
		case global::VictoryTypeId.DECISIVE:
			break;
		default:
			if (victory != global::VictoryTypeId.CAMPAIGN)
			{
				goto IL_67;
			}
			break;
		}
		unit.AddToUpkeepOwned(unitCost.DecisiveVictory);
		IL_67:
		if (victory != global::VictoryTypeId.LOSS)
		{
			if (unit.GetUnitTypeId() == global::UnitTypeId.LEADER)
			{
				unit.AddToUpkeepOwned(global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.GetAttribute(global::WarbandAttributeId.WON_UPKEEP_LEADER_MODIFIER));
			}
			else if (unit.GetUnitTypeId() == global::UnitTypeId.IMPRESSIVE)
			{
				unit.AddToUpkeepOwned(global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.GetAttribute(global::WarbandAttributeId.WON_UPKEEP_IMPRESSIVE_MODIFIER));
			}
		}
		if (unit.UnitSave.injuredTime == 0)
		{
			int date = global::PandoraSingleton<global::HideoutManager>.Instance.Date.CurrentDate + global::Constant.GetInt(global::ConstantId.UPKEEP_DAYS_WITHOUT_PAY);
			unit.Logger.AddHistory(date, global::EventLogger.LogEvent.LEFT, 0);
		}
	}

	public void AddInjuryTime(global::Unit unit, global::InjuryId id)
	{
		int num = global::UnityEngine.Mathf.Max(1, global::Constant.GetInt(global::ConstantId.INJURY_ROLL_INTERVAL) - 1);
		if (unit.UnitSave.injuredTime > 0)
		{
			global::Tuple<int, global::EventLogger.LogEvent, int> tuple = unit.Logger.FindEventAfter(global::EventLogger.LogEvent.NO_TREATMENT, global::PandoraSingleton<global::HideoutManager>.Instance.Date.CurrentDate + 1);
			if (tuple != null)
			{
				unit.Logger.RemoveHistory(tuple);
			}
			unit.Logger.AddHistory(global::PandoraSingleton<global::HideoutManager>.Instance.Date.CurrentDate + num, global::EventLogger.LogEvent.NO_TREATMENT, (int)id);
			tuple = unit.Logger.FindEventAfter(global::EventLogger.LogEvent.RECOVERY, global::PandoraSingleton<global::HideoutManager>.Instance.Date.CurrentDate + 1);
			if (tuple != null)
			{
				unit.Logger.RemoveHistory(tuple);
			}
			tuple = unit.Logger.FindEventAfter(global::EventLogger.LogEvent.LEFT, global::PandoraSingleton<global::HideoutManager>.Instance.Date.CurrentDate + 1);
			if (tuple != null)
			{
				unit.Logger.RemoveHistory(tuple);
			}
		}
	}

	public void EndGameWarbandProgress(global::MissionEndDataSave endData, global::Warband warband)
	{
		warband.rankGained = false;
		if (warband.Rank < global::Constant.GetInt(global::ConstantId.MAX_UNIT_RANK))
		{
			warband.Xp += global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::VictoryTypeData>((int)endData.VictoryType).WarbandExperience;
			global::WarbandRankData warbandRankData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::WarbandRankData>(warband.Rank + 1);
			while (warband.Xp >= warbandRankData.Exp && warband.Rank < warband.GetMaxRank())
			{
				if (warband.rankGained)
				{
					warband.Xp = warbandRankData.Exp - 1;
					break;
				}
				warband.Rank++;
				warband.rankGained = true;
				warband.Xp -= warbandRankData.Exp;
				warbandRankData = warband.GetWarbandRankData();
			}
		}
		this.UpdateWarbandStats(warband);
		this.ProcessTrophies(warband);
		warband.UpdateAttributes();
		global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.GenerateHireableUnits();
	}

	public void UpdateWarbandStats(global::Warband warband)
	{
		global::MissionEndDataSave endMission = warband.GetWarbandSave().endMission;
		bool flag = true;
		for (int i = 0; i < warband.GetWarbandSave().endMission.units.Count; i++)
		{
			if (endMission.units[i].status == global::UnitStateId.OUT_OF_ACTION)
			{
				flag = false;
				break;
			}
		}
		if (!endMission.isVsAI)
		{
			this.IncrementWarbandStat(global::WarbandAttributeId.SKIRMISH_ATTEMPTED, 1);
			if (endMission.VictoryType != global::VictoryTypeId.LOSS)
			{
				this.IncrementWarbandStat(global::WarbandAttributeId.SKIRMISH_WIN, 1);
			}
			if (endMission.VictoryType == global::VictoryTypeId.DECISIVE)
			{
				this.IncrementWarbandStat(global::WarbandAttributeId.SKIRMISH_DECISIVE_VICTORY, 1);
			}
			else if (endMission.VictoryType == global::VictoryTypeId.BATTLEGROUND)
			{
				this.IncrementWarbandStat(global::WarbandAttributeId.SKIRMISH_BATTLEGROUND_VICTORY, 1);
			}
			else if (endMission.VictoryType == global::VictoryTypeId.OBJECTIVE)
			{
				this.IncrementWarbandStat(global::WarbandAttributeId.SKIRMISH_OBJECTIVE_VICTORY, 1);
			}
			else
			{
				this.IncrementWarbandStat(global::WarbandAttributeId.SKIRMISH_LOST, 1);
			}
		}
		else
		{
			warband.AddToAttribute(global::WarbandAttributeId.CAMPAIGN_MISSION_ATTEMPTED, 1);
			if (endMission.won || endMission.primaryObjectiveCompleted)
			{
				this.IncrementWarbandStat(global::WarbandAttributeId.CAMPAIGN_MISSION_WIN, 1);
				int attribute = warband.GetAttribute(global::WarbandAttributeId.CAMPAIGN_MISSION_WIN);
				if (attribute % 10 == 0)
				{
					warband.Logger.AddHistory(warband.GetWarbandSave().currentDate, global::EventLogger.LogEvent.MEMORABLE_CAMPAIGN_VICTORY, attribute);
				}
				warband.GetWarbandSave().winningStreak++;
				int num = warband.GetWarbandSave().winningStreak - warband.GetAttribute(global::WarbandAttributeId.CAMPAIGN_MISSION_WIN_STREAK);
				if (num > 0)
				{
					this.IncrementWarbandStat(global::WarbandAttributeId.CAMPAIGN_MISSION_WIN_STREAK, num);
					if (warband.GetWarbandSave().winningStreak % 5 == 0)
					{
						warband.Logger.AddHistory(warband.GetWarbandSave().currentDate, global::EventLogger.LogEvent.VICTORY_STREAK, warband.GetWarbandSave().winningStreak);
					}
				}
				if (endMission.VictoryType == global::VictoryTypeId.CAMPAIGN)
				{
					global::System.Collections.Generic.List<global::CampaignMissionData> list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::CampaignMissionData>(new string[]
					{
						"fk_warband_id",
						"idx"
					}, new string[]
					{
						((int)warband.WarbandData.Id).ToString(),
						warband.GetWarbandSave().curCampaignIdx.ToString()
					});
					global::PandoraSingleton<global::GameManager>.Instance.Profile.CheckAchievement(list[0].Id);
					global::PandoraSingleton<global::GameManager>.Instance.Profile.IsAchievementUnlocked(global::AchievementId.STORY_SKAVEN_1);
					if (warband.GetWarbandSave().curCampaignIdx <= global::Constant.GetInt(global::ConstantId.CAMPAIGN_LAST_MISSION))
					{
						warband.Logger.AddHistory(global::PandoraSingleton<global::HideoutManager>.Instance.Date.CurrentDate + global::Constant.GetInt(global::ConstantId.MISSION_REWARD_DAYS), global::EventLogger.LogEvent.MISSION_REWARDS, warband.GetWarbandSave().curCampaignIdx);
						global::PandoraSingleton<global::GameManager>.Instance.Profile.UpdateGameProgress(warband.Id, warband.GetWarbandSave().curCampaignIdx);
						global::PandoraSingleton<global::Hephaestus>.Instance.UpdateGameProgress();
						warband.GetWarbandSave().curCampaignIdx++;
						if (list.Count > 0)
						{
							int date = global::PandoraSingleton<global::HideoutManager>.Instance.Date.CurrentDate + list[0].Days;
							warband.Logger.AddHistory(date, global::EventLogger.LogEvent.NEW_MISSION, warband.GetWarbandSave().curCampaignIdx);
						}
					}
				}
				if (endMission.crushed)
				{
					this.IncrementWarbandStat(global::WarbandAttributeId.CAMPAIGN_MISSION_CRUSHED_VICTORY, 1);
				}
				if (flag)
				{
					this.IncrementWarbandStat(global::WarbandAttributeId.CAMPAIGN_MISSION_TOTAL_VICTORY, 1);
				}
			}
			else
			{
				this.IncrementWarbandStat(global::WarbandAttributeId.CAMPAIGN_MISSION_LOST, 1);
				warband.GetWarbandSave().winningStreak = 0;
			}
		}
	}

	private void ProcessTrophies(global::Warband warband)
	{
		int num = 0;
		if (global::PandoraSingleton<global::GameManager>.Instance.Profile.IsAchievementUnlocked(global::AchievementId.STORY_SKAVEN_1))
		{
			num++;
			global::PandoraSingleton<global::Hephaestus>.Instance.UnlockAchievement(global::Hephaestus.TrophyId.STORY_SKAVEN_1);
		}
		if (global::PandoraSingleton<global::GameManager>.Instance.Profile.IsAchievementUnlocked(global::AchievementId.STORY_MERC_1))
		{
			num++;
			global::PandoraSingleton<global::Hephaestus>.Instance.UnlockAchievement(global::Hephaestus.TrophyId.STORY_MERC_1);
		}
		if (global::PandoraSingleton<global::GameManager>.Instance.Profile.IsAchievementUnlocked(global::AchievementId.STORY_POSSESSED_1))
		{
			num++;
			global::PandoraSingleton<global::Hephaestus>.Instance.UnlockAchievement(global::Hephaestus.TrophyId.STORY_POSSESSED_1);
		}
		if (global::PandoraSingleton<global::GameManager>.Instance.Profile.IsAchievementUnlocked(global::AchievementId.STORY_SISTERS_1))
		{
			num++;
			global::PandoraSingleton<global::Hephaestus>.Instance.UnlockAchievement(global::Hephaestus.TrophyId.STORY_SISTERS_1);
		}
		if (global::PandoraSingleton<global::GameManager>.Instance.Profile.IsAchievementUnlocked(global::AchievementId.STORY_WITCH_HUNTERS_1))
		{
			global::PandoraSingleton<global::Hephaestus>.Instance.UnlockAchievement(global::Hephaestus.TrophyId.STORY_WITCH_HUNTERS_1);
		}
		if (global::PandoraSingleton<global::GameManager>.Instance.Profile.IsAchievementUnlocked(global::AchievementId.STORY_UNDEAD_1))
		{
			global::PandoraSingleton<global::Hephaestus>.Instance.UnlockAchievement(global::Hephaestus.TrophyId.STORY_UNDEAD_1);
		}
		if (num == 4)
		{
			global::PandoraSingleton<global::Hephaestus>.Instance.UnlockAchievement(global::Hephaestus.TrophyId.STORY_ALL_1);
		}
		num = 0;
		if (global::PandoraSingleton<global::GameManager>.Instance.Profile.IsAchievementUnlocked(global::AchievementId.STORY_SKAVEN_2))
		{
			num++;
			global::PandoraSingleton<global::Hephaestus>.Instance.UnlockAchievement(global::Hephaestus.TrophyId.STORY_SKAVEN_2);
		}
		if (global::PandoraSingleton<global::GameManager>.Instance.Profile.IsAchievementUnlocked(global::AchievementId.STORY_MERC_2))
		{
			num++;
			global::PandoraSingleton<global::Hephaestus>.Instance.UnlockAchievement(global::Hephaestus.TrophyId.STORY_MERC_2);
		}
		if (global::PandoraSingleton<global::GameManager>.Instance.Profile.IsAchievementUnlocked(global::AchievementId.STORY_POSSESSED_2))
		{
			num++;
			global::PandoraSingleton<global::Hephaestus>.Instance.UnlockAchievement(global::Hephaestus.TrophyId.STORY_POSSESSED_2);
		}
		if (global::PandoraSingleton<global::GameManager>.Instance.Profile.IsAchievementUnlocked(global::AchievementId.STORY_SISTERS_2))
		{
			num++;
			global::PandoraSingleton<global::Hephaestus>.Instance.UnlockAchievement(global::Hephaestus.TrophyId.STORY_SISTERS_2);
		}
		if (global::PandoraSingleton<global::GameManager>.Instance.Profile.IsAchievementUnlocked(global::AchievementId.STORY_WITCH_HUNTERS_2))
		{
			global::PandoraSingleton<global::Hephaestus>.Instance.UnlockAchievement(global::Hephaestus.TrophyId.STORY_WITCH_HUNTERS_2);
		}
		if (global::PandoraSingleton<global::GameManager>.Instance.Profile.IsAchievementUnlocked(global::AchievementId.STORY_UNDEAD_2))
		{
			global::PandoraSingleton<global::Hephaestus>.Instance.UnlockAchievement(global::Hephaestus.TrophyId.STORY_UNDEAD_2);
		}
		if (num == 4)
		{
			global::PandoraSingleton<global::Hephaestus>.Instance.UnlockAchievement(global::Hephaestus.TrophyId.STORY_ALL_2);
		}
		if (warband.rankGained && warband.Rank == global::Constant.GetInt(global::ConstantId.MAX_UNIT_RANK))
		{
			global::WarbandId id = warband.Id;
			switch (id)
			{
			case global::WarbandId.HUMAN_MERCENARIES:
				global::PandoraSingleton<global::Hephaestus>.Instance.UnlockAchievement(global::Hephaestus.TrophyId.MERC_RANK_10);
				break;
			case global::WarbandId.SKAVENS:
				global::PandoraSingleton<global::Hephaestus>.Instance.UnlockAchievement(global::Hephaestus.TrophyId.SKAVEN_RANK_10);
				break;
			case global::WarbandId.SISTERS_OF_SIGMAR:
				global::PandoraSingleton<global::Hephaestus>.Instance.UnlockAchievement(global::Hephaestus.TrophyId.SISTERS_RANK_10);
				break;
			default:
				if (id != global::WarbandId.WITCH_HUNTERS)
				{
					if (id == global::WarbandId.UNDEAD)
					{
						global::PandoraSingleton<global::Hephaestus>.Instance.UnlockAchievement(global::Hephaestus.TrophyId.UNDEAD_10);
					}
				}
				else
				{
					global::PandoraSingleton<global::Hephaestus>.Instance.UnlockAchievement(global::Hephaestus.TrophyId.WITCH_HUNTERS_10);
				}
				break;
			case global::WarbandId.POSSESSED:
				global::PandoraSingleton<global::Hephaestus>.Instance.UnlockAchievement(global::Hephaestus.TrophyId.POSSESSED_RANK_10);
				break;
			}
			if (global::PandoraSingleton<global::Hephaestus>.Instance.IsAchievementUnlocked(global::Hephaestus.TrophyId.SKAVEN_RANK_10) && global::PandoraSingleton<global::Hephaestus>.Instance.IsAchievementUnlocked(global::Hephaestus.TrophyId.MERC_RANK_10) && global::PandoraSingleton<global::Hephaestus>.Instance.IsAchievementUnlocked(global::Hephaestus.TrophyId.SISTERS_RANK_10) && global::PandoraSingleton<global::Hephaestus>.Instance.IsAchievementUnlocked(global::Hephaestus.TrophyId.POSSESSED_RANK_10))
			{
				global::PandoraSingleton<global::Hephaestus>.Instance.UnlockAchievement(global::Hephaestus.TrophyId.ALL_RANK_10);
			}
		}
	}

	private void IncrementWarbandStat(global::WarbandAttributeId attributeId, int increment)
	{
		if (increment != 0)
		{
			global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.AddToAttribute(attributeId, increment);
		}
	}

	private void SetUnitStat(global::Unit unit, global::AttributeId attributeId, int value, bool checkWarbandStat = true)
	{
		if (value != 0 && unit != null)
		{
			int attribute = unit.GetAttribute(attributeId);
			unit.SetAttribute(attributeId, value);
			if (attributeId == global::AttributeId.TOTAL_KILL && attribute != value && attribute / 10 != value / 10)
			{
				unit.Logger.AddHistory(global::PandoraSingleton<global::HideoutManager>.Instance.Date.CurrentDate, global::EventLogger.LogEvent.MEMORABLE_KILL, value / 10 * 10);
			}
			global::PandoraSingleton<global::GameManager>.Instance.Profile.CheckAchievement(unit, attributeId, unit.GetAttribute(attributeId));
			if (checkWarbandStat)
			{
				global::DataFactory instance = global::PandoraSingleton<global::DataFactory>.Instance;
				string field = "fk_attribute_id";
				int num = (int)attributeId;
				global::System.Collections.Generic.List<global::WarbandAttributeData> list = instance.InitData<global::WarbandAttributeData>(field, num.ToString());
				if (list.Count > 0)
				{
					for (int i = 0; i < list.Count; i++)
					{
						this.IncrementWarbandStat(list[i].Id, value - attribute);
					}
				}
			}
		}
	}

	public void NextDayUnitProgress(global::Unit unit)
	{
		global::UnitActiveStatusId activeStatus = unit.GetActiveStatus();
		this.CheckForInjury(unit);
		unit.UpdateSkillTraining();
		this.CheckUpkeep(unit);
		if (unit.GetActiveStatus() == global::UnitActiveStatusId.AVAILABLE && activeStatus != global::UnitActiveStatusId.AVAILABLE)
		{
			global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.FindSuitableSlot(unit, true);
		}
	}

	public void NextDayWarbandProgress()
	{
		global::WarbandMenuController warbandCtrlr = global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr;
		global::WarbandSave warbandSave = warbandCtrlr.Warband.GetWarbandSave();
		warbandSave.scoutsSent = 0;
		warbandSave.missions.Clear();
		global::PandoraSingleton<global::HideoutManager>.Instance.GenerateMissions(true);
		warbandCtrlr.Warband.GenerateContactItems();
		global::Date date = global::PandoraSingleton<global::HideoutManager>.Instance.Date;
		global::System.Collections.Generic.List<global::Tuple<int, global::EventLogger.LogEvent, int>> eventsAtDay = warbandCtrlr.Warband.Logger.GetEventsAtDay(date.CurrentDate);
		int i = 0;
		while (i < eventsAtDay.Count)
		{
			switch (eventsAtDay[i].Item2)
			{
			case global::EventLogger.LogEvent.SHIPMENT_REQUEST:
			{
				int num = eventsAtDay[i].Item3;
				int date2;
				if (num == -1)
				{
					num = global::Constant.GetInt(global::ConstantId.FIRST_SHIPMENT_WEIGHT);
					date2 = eventsAtDay[i].Item1 + global::Constant.GetInt(global::ConstantId.FIRST_SHIPMENT_DAYS) + warbandCtrlr.Warband.GetAttribute(global::WarbandAttributeId.REQUEST_TIME_MODIFIER);
				}
				else
				{
					num = eventsAtDay[i].Item3;
					global::WyrdstoneShipmentData wyrdstoneShipmentData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::WyrdstoneShipmentData>("fk_warband_rank_id", warbandSave.rank.ToString())[0];
					date2 = eventsAtDay[i].Item1 + global::PandoraSingleton<global::GameManager>.Instance.LocalTyche.Rand(wyrdstoneShipmentData.MinDays, wyrdstoneShipmentData.MaxDays + 1) + warbandCtrlr.Warband.GetAttribute(global::WarbandAttributeId.REQUEST_TIME_MODIFIER);
				}
				warbandCtrlr.Warband.Logger.AddHistory(date2, global::EventLogger.LogEvent.SHIPMENT_LATE, num);
				break;
			}
			case global::EventLogger.LogEvent.SHIPMENT_DELIVERY:
				this.DoDelivery(global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.PrimaryFactionController, eventsAtDay[i].Item3, true);
				break;
			case global::EventLogger.LogEvent.FACTION0_DELIVERY:
			case global::EventLogger.LogEvent.FACTION1_DELIVERY:
			case global::EventLogger.LogEvent.FACTION2_DELIVERY:
			{
				global::FactionMenuController faction = null;
				for (int j = 0; j < warbandCtrlr.factionCtrlrs.Count; j++)
				{
					if (warbandCtrlr.factionCtrlrs[j].Faction.GetFactionDeliveryEvent() == eventsAtDay[i].Item2)
					{
						faction = warbandCtrlr.factionCtrlrs[j];
					}
				}
				this.DoDelivery(faction, eventsAtDay[i].Item3, false);
				break;
			}
			case global::EventLogger.LogEvent.MARKET_ROTATION:
				global::PandoraSingleton<global::HideoutManager>.Instance.Market.RefreshMarket(global::MarketEventId.NONE, true);
				warbandCtrlr.Warband.Logger.AddHistory(date.GetNextDay(date.WeekDay), global::EventLogger.LogEvent.MARKET_ROTATION, 0);
				break;
			case global::EventLogger.LogEvent.OUTSIDER_ROTATION:
				global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.RefreshOutsiders();
				global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.GenerateHireableUnits();
				warbandCtrlr.Warband.Logger.AddHistory(date.GetNextDay(date.WeekDay), global::EventLogger.LogEvent.OUTSIDER_ROTATION, 0);
				break;
			case global::EventLogger.LogEvent.RESPEC_POINT:
				warbandSave.availaibleRespec++;
				warbandCtrlr.Warband.Logger.AddHistory(date.CurrentDate + global::Constant.GetInt(global::ConstantId.CAL_DAYS_PER_YEAR), global::EventLogger.LogEvent.RESPEC_POINT, 0);
				break;
			}
			IL_2FA:
			i++;
			continue;
			goto IL_2FA;
		}
		global::System.Collections.Generic.List<global::Tuple<int, global::EventLogger.LogEvent, int>> list = warbandCtrlr.Warband.Logger.FindEventsAfter(global::EventLogger.LogEvent.MISSION_REWARDS, global::Constant.GetInt(global::ConstantId.CAL_DAY_START));
		for (int k = 1; k < warbandSave.curCampaignIdx; k++)
		{
			bool flag = false;
			for (int l = 0; l < list.Count; l++)
			{
				if (list[l].Item3 == k)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				warbandCtrlr.Warband.Logger.AddHistory(date.CurrentDate, global::EventLogger.LogEvent.MISSION_REWARDS, k);
			}
		}
		warbandCtrlr.Warband.UpdateAttributes();
	}

	public void DoDelivery(global::FactionMenuController faction, int id, bool isShipment)
	{
		if (isShipment)
		{
			global::PandoraSingleton<global::HideoutManager>.Instance.WarbandChest.AddGold(id);
			global::PandoraSingleton<global::Hephaestus>.Instance.IncrementStat(global::Hephaestus.StatId.WYRDSTONE_SELL, id);
		}
		else
		{
			global::ShipmentSave delivery = faction.Faction.GetDelivery(id);
			global::PandoraSingleton<global::HideoutManager>.Instance.WarbandChest.AddGold(delivery.gold);
			global::PandoraSingleton<global::Hephaestus>.Instance.IncrementStat(global::Hephaestus.StatId.WYRDSTONE_SELL, delivery.gold);
			for (int i = 0; i < 5; i++)
			{
				int rankId = delivery.rank >> 4 * i & 15;
				this.IncrementFactionRank(faction, rankId);
			}
		}
		global::PandoraSingleton<global::Hephaestus>.Instance.IncrementStat(global::Hephaestus.StatId.SHIPMENTS, 1);
	}

	public void IncrementFactionRank(global::FactionMenuController faction, int rankId)
	{
		if (rankId > 0)
		{
			global::WarbandSkillId rewardWarbandSkillId = faction.Faction.GetRewardWarbandSkillId((global::FactionRankId)rankId);
			global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.AddSkill(rewardWarbandSkillId, true, true);
			global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.GenerateHireableUnits();
		}
	}

	public global::Unit CheckLateShipment(global::WarbandSave warSave, global::WarbandMenuController ctrlr, global::FactionMenuController primaryFactionCtrlr)
	{
		global::Tuple<int, global::EventLogger.LogEvent, int> tuple = ctrlr.Warband.Logger.FindLastEvent(global::EventLogger.LogEvent.SHIPMENT_LATE);
		global::Unit unit = null;
		if (tuple != null && tuple.Item1 + 1 == global::PandoraSingleton<global::HideoutManager>.Instance.Date.CurrentDate)
		{
			warSave.lateShipmentCount++;
			warSave.lastShipmentFailed = true;
			warSave.nextShipmentExtraDays = 0;
			global::FactionConsequenceData factionConsequenceData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::FactionConsequenceData>(new string[]
			{
				"fk_faction_id",
				"late_shipment_count"
			}, new string[]
			{
				((int)primaryFactionCtrlr.Faction.Data.Id).ToString(),
				warSave.lateShipmentCount.ToString()
			})[0];
			if (factionConsequenceData.FactionConsequenceTargetId != global::FactionConsequenceTargetId.NONE)
			{
				global::InjuryData injuryData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::InjuryData>((int)factionConsequenceData.InjuryId);
				global::FactionConsequenceTargetId factionConsequenceTargetId = factionConsequenceData.FactionConsequenceTargetId;
				if (factionConsequenceTargetId != global::FactionConsequenceTargetId.LOWEST_RANKED_HERO)
				{
					if (factionConsequenceTargetId == global::FactionConsequenceTargetId.HIGHEST_RANKED_HERO)
					{
						int num = 0;
						while (num < global::Progressor.HIGHEST_RANK_UNIT_TYPES.Length && unit == null)
						{
							unit = ctrlr.GetHighestRankUnit(global::Progressor.HIGHEST_RANK_UNIT_TYPES[num], injuryData);
							num++;
						}
					}
				}
				else
				{
					int num2 = 0;
					while (num2 < global::Progressor.LOWEST_RANK_UNIT_TYPES.Length && unit == null)
					{
						unit = ctrlr.GetLowestRankUnit(global::Progressor.LOWEST_RANK_UNIT_TYPES[num2], injuryData);
						num2++;
					}
				}
				if (unit != null)
				{
					global::System.Collections.Generic.List<global::Item> list = new global::System.Collections.Generic.List<global::Item>();
					bool flag = unit.AddInjury(injuryData, global::PandoraSingleton<global::HideoutManager>.Instance.Date.CurrentDate, list, false, factionConsequenceData.TreatmentTime);
					global::PandoraSingleton<global::HideoutManager>.Instance.WarbandChest.AddItems(list);
					if (factionConsequenceData.TreatmentTime > 0)
					{
						unit.UnitSave.injuredTime = factionConsequenceData.TreatmentTime;
					}
					this.AddInjuryTime(unit, injuryData.Id);
					if (!flag)
					{
						if (warSave.lateShipmentCount > 2)
						{
							global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Disband(unit, global::EventLogger.LogEvent.DEATH, (int)injuryData.Id);
						}
						else
						{
							global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Disband(unit, global::EventLogger.LogEvent.RETIREMENT, (int)injuryData.Id);
						}
					}
				}
			}
			global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.UpdateAttributes();
			primaryFactionCtrlr.CreateNewShipmentRequest(global::PandoraSingleton<global::HideoutManager>.Instance.Date.CurrentDate, global::PandoraSingleton<global::HideoutManager>.Instance.Date.CurrentDate);
			if (global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.GetWarbandSave().lateShipmentCount >= global::Constant.GetInt(global::ConstantId.MAX_SHIPMENT_FAIL))
			{
				global::PandoraSingleton<global::Hephaestus>.Instance.UnlockAchievement(global::Hephaestus.TrophyId.GAME_OVER);
			}
		}
		return unit;
	}

	private void CheckForInjury(global::Unit unit)
	{
		global::Tuple<int, global::EventLogger.LogEvent, int> tuple = unit.Logger.FindLastEvent(global::EventLogger.LogEvent.NO_TREATMENT);
		global::Tuple<int, global::EventLogger.LogEvent, int> tuple2 = unit.Logger.FindLastEvent(global::EventLogger.LogEvent.RECOVERY);
		global::Tuple<int, global::EventLogger.LogEvent, int> tuple3 = null;
		if (tuple == null && tuple2 != null)
		{
			tuple3 = tuple2;
		}
		else if (tuple != null && tuple2 == null)
		{
			tuple3 = tuple;
		}
		else if (tuple != null && tuple2 != null)
		{
			if (tuple.Item1 > tuple2.Item1)
			{
				tuple3 = tuple;
			}
			else
			{
				tuple3 = tuple2;
			}
		}
		if (tuple3 != null)
		{
			if (tuple3.Item2 == global::EventLogger.LogEvent.NO_TREATMENT)
			{
				if (tuple3.Item1 == global::PandoraSingleton<global::HideoutManager>.Instance.Date.CurrentDate)
				{
					int @int = global::Constant.GetInt(global::ConstantId.INJURY_ROLL_INTERVAL);
					int num = tuple3.Item1 - unit.UnitSave.lastInjuryDate - 2;
					int num2 = global::Constant.GetInt(global::ConstantId.INJURY_ROLL_TARGET) + num / @int * global::Constant.GetInt(global::ConstantId.INJURY_DETERIORATION_PENALTY);
					if (num2 <= 1)
					{
						global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Disband(unit, global::EventLogger.LogEvent.DEATH, tuple3.Item3);
						global::PandoraSingleton<global::Hephaestus>.Instance.UnlockAchievement(global::Hephaestus.TrophyId.TREATMENT_NOT_PAID);
					}
					else
					{
						int num3 = global::PandoraSingleton<global::GameManager>.Instance.LocalTyche.Rand(1, 11);
						if (num3 >= num2)
						{
							global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Disband(unit, global::EventLogger.LogEvent.DEATH, tuple3.Item3);
							global::PandoraSingleton<global::Hephaestus>.Instance.UnlockAchievement(global::Hephaestus.TrophyId.TREATMENT_NOT_PAID);
							global::PandoraDebug.LogInfo(string.Concat(new object[]
							{
								"unit dies from injury ",
								num3,
								" >= ",
								num2
							}), "MENUS", null);
						}
						else
						{
							unit.Logger.AddHistory(global::PandoraSingleton<global::HideoutManager>.Instance.Date.CurrentDate + @int, global::EventLogger.LogEvent.NO_TREATMENT, tuple3.Item3);
						}
					}
				}
			}
			else if (tuple3.Item2 == global::EventLogger.LogEvent.RECOVERY)
			{
				unit.UpdateInjury();
			}
		}
	}

	private void CheckUpkeep(global::Unit unit)
	{
		if (unit.UnitSave.injuredTime > 0 && !unit.UnitSave.injuryPaid)
		{
			return;
		}
		int upkeepOwned = unit.GetUpkeepOwned();
		if (upkeepOwned != 0)
		{
			int num = (int)((float)upkeepOwned * global::Constant.GetFloat(global::ConstantId.UPKEEP_MISSED_DAY_PCT));
			int num2 = unit.AddToUpkeepOwned((num <= 0) ? 1 : num);
			if (num2 >= global::Constant.GetInt(global::ConstantId.UPKEEP_DAYS_WITHOUT_PAY))
			{
				unit.Logger.RemoveLastHistory(global::EventLogger.LogEvent.LEFT);
				global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Disband(unit, global::EventLogger.LogEvent.LEFT, 0);
				global::PandoraSingleton<global::Hephaestus>.Instance.UnlockAchievement(global::Hephaestus.TrophyId.UPKEEP_NOT_PAID);
			}
		}
	}

	private const int BASE_INJURY_COUNT = 1;

	private const int MULTIPLE_INJURY_COUNT = 3;

	private static readonly global::InjuryId[] INJURY_EXCLUDES = new global::InjuryId[]
	{
		global::InjuryId.DEAD,
		global::InjuryId.MULTIPLE_INJURIES,
		global::InjuryId.FULL_RECOVERY
	};

	private static readonly global::UnitTypeId[] LOWEST_RANK_UNIT_TYPES = new global::UnitTypeId[]
	{
		global::UnitTypeId.HERO_3,
		global::UnitTypeId.HERO_2,
		global::UnitTypeId.HERO_1,
		global::UnitTypeId.HENCHMEN,
		global::UnitTypeId.LEADER
	};

	private static readonly global::UnitTypeId[] HIGHEST_RANK_UNIT_TYPES = new global::UnitTypeId[]
	{
		global::UnitTypeId.HERO_1,
		global::UnitTypeId.HERO_2,
		global::UnitTypeId.HERO_3,
		global::UnitTypeId.HENCHMEN,
		global::UnitTypeId.LEADER
	};
}
