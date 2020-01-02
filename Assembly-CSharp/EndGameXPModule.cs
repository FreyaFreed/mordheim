using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGameXPModule : global::UIModule
{
	public void Set(global::MissionEndUnitSave endUnit, global::Unit unit)
	{
		this.xpList.gameObject.SetActive(true);
		this.advList.gameObject.SetActive(true);
		this.xpList.ClearList();
		this.advList.ClearList();
		if (endUnit.XPs.Count > 0)
		{
			global::PandoraDebug.LogDebug(string.Concat(new object[]
			{
				"Unit Name = ",
				endUnit.unitSave.stats.Name,
				"XP Count = ",
				endUnit.XPs.Count
			}), "uncategorised", null);
			if (unit.IsMaxRank())
			{
				this.xpList.Setup("hideout_max_rank", this.xpItem);
				this.showQueue.Enqueue(this.xpList.gameObject);
			}
			else
			{
				this.xpList.Setup("menu_experience", this.xpItem);
				this.showQueue.Enqueue(this.xpList.gameObject);
				for (int i = 0; i < endUnit.XPs.Count; i++)
				{
					this.AddXp(string.Empty + endUnit.XPs[i].Key, endUnit.XPs[i].Value);
				}
			}
		}
		bool flag = false;
		if (endUnit.advancements.Count > 0)
		{
			global::PandoraDebug.LogDebug(string.Concat(new object[]
			{
				"Unit Name = ",
				endUnit.unitSave.stats.Name,
				"ADV Count = ",
				endUnit.advancements.Count
			}), "uncategorised", null);
			this.advList.Setup("menu_advancement", this.advItem);
			this.showQueue.Enqueue(this.advList.gameObject);
			for (int j = 0; j < endUnit.advancements.Count; j++)
			{
				if (endUnit.advancements[j].Martial > 0)
				{
					this.AddAdvancement("unit_adv_martial", new string[]
					{
						endUnit.advancements[j].Martial.ToString()
					});
				}
				if (endUnit.advancements[j].Mental > 0)
				{
					this.AddAdvancement("unit_adv_mental", new string[]
					{
						endUnit.advancements[j].Mental.ToString()
					});
				}
				if (endUnit.advancements[j].Physical > 0)
				{
					this.AddAdvancement("unit_adv_physical", new string[]
					{
						endUnit.advancements[j].Physical.ToString()
					});
				}
				if (endUnit.advancements[j].Mutation)
				{
					if (!flag)
					{
						global::PandoraSingleton<global::Pan>.Instance.Narrate("mutation" + global::PandoraSingleton<global::GameManager>.Instance.LocalTyche.Rand(1, 6));
						flag = true;
					}
					this.AddAdvancement("unit_adv_mutation", new string[]
					{
						"1"
					});
				}
				if (endUnit.advancements[j].Spell > 0)
				{
					this.AddAdvancement("unit_adv_spell", new string[]
					{
						endUnit.advancements[j].Spell.ToString()
					});
				}
				if (endUnit.advancements[j].Skill > 0)
				{
					this.AddAdvancement("unit_adv_skill", new string[]
					{
						endUnit.advancements[j].Skill.ToString()
					});
				}
				if (endUnit.advancements[j].Offense > 0)
				{
					this.AddAdvancement("unit_adv_off", new string[]
					{
						endUnit.advancements[j].Offense.ToString()
					});
				}
				if (endUnit.advancements[j].Strategy > 0)
				{
					this.AddAdvancement("unit_adv_strat", new string[]
					{
						endUnit.advancements[j].Strategy.ToString()
					});
				}
			}
		}
		this.xpList.gameObject.SetActive(false);
		this.advList.gameObject.SetActive(false);
		base.StartShow(0.5f);
	}

	public void Set(global::Warband warband)
	{
		global::VictoryTypeId victoryType = warband.GetWarbandSave().endMission.VictoryType;
		this.xpList.gameObject.SetActive(true);
		this.advList.gameObject.SetActive(true);
		this.xpList.ClearList();
		this.advList.ClearList();
		if (warband.rankGained || warband.Rank < global::Constant.GetInt(global::ConstantId.MAX_UNIT_RANK))
		{
			this.xpList.Setup("menu_warband_experience", this.xpItem);
			this.showQueue.Enqueue(this.xpList.gameObject);
			this.AddXp(global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::VictoryTypeData>((int)victoryType).WarbandExperience.ToString(), "mission_victory_" + victoryType.ToLowerString());
		}
		if (warband.rankGained)
		{
			global::PandoraSingleton<global::Pan>.Instance.Narrate("new_warband_rank");
			warband.Logger.AddHistory(warband.GetWarbandSave().currentDate, global::EventLogger.LogEvent.RANK_ACHIEVED, warband.Rank);
			this.advList.Setup("menu_advancement", this.advItem);
			this.showQueue.Enqueue(this.advList.gameObject);
			int id = warband.Rank - 1;
			global::WarbandRankSlotData warbandRankSlotData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::WarbandRankSlotData>("fk_warband_rank_id", id.ToString())[0];
			global::WarbandRankSlotData warbandRankSlotData2 = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::WarbandRankSlotData>("fk_warband_rank_id", warband.Rank.ToString())[0];
			int num = warbandRankSlotData2.Henchman - warbandRankSlotData.Henchman;
			if (num > 0)
			{
				string stringById = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("unit_type_" + global::UnitTypeId.HENCHMEN.ToString());
				this.AddAdvancement("warband_adv_slot_gained", new string[]
				{
					num.ToString(),
					stringById
				});
			}
			num = warbandRankSlotData2.Hero - warbandRankSlotData.Hero;
			if (num > 0)
			{
				string stringById2 = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("unit_type_" + global::UnitTypeId.HERO_1.ToString());
				this.AddAdvancement("warband_adv_slot_gained", new string[]
				{
					num.ToString(),
					stringById2
				});
			}
			num = warbandRankSlotData2.Impressive - warbandRankSlotData.Impressive;
			if (num > 0)
			{
				if (warbandRankSlotData.Impressive > 0)
				{
					string stringById3 = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("unit_type_" + global::UnitTypeId.IMPRESSIVE.ToString());
					this.AddAdvancement("warband_adv_slot_gained", new string[]
					{
						"1",
						stringById3
					});
				}
				else
				{
					string stringById4 = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("unit_type_" + global::UnitTypeId.HERO_1.ToString());
					this.AddAdvancement("warband_adv_slot_gained", new string[]
					{
						num.ToString(),
						stringById4
					});
				}
			}
			num = warbandRankSlotData2.Leader - warbandRankSlotData.Leader;
			if (num > 0)
			{
				string stringById5 = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("unit_type_" + global::UnitTypeId.LEADER.ToString());
				this.AddAdvancement("warband_adv_slot_gained", new string[]
				{
					num.ToString(),
					stringById5
				});
			}
			num = warbandRankSlotData2.Reserve - warbandRankSlotData.Reserve;
			if (num > 0)
			{
				string stringById6 = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_menu_warband_reserve");
				this.AddAdvancement("warband_adv_slot_gained", new string[]
				{
					num.ToString(),
					stringById6
				});
			}
			global::System.Collections.Generic.List<global::WarbandRankJoinUnitTypeData> list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::WarbandRankJoinUnitTypeData>("fk_warband_rank_id", warband.Rank.ToString());
			foreach (global::WarbandRankJoinUnitTypeData warbandRankJoinUnitTypeData in list)
			{
				if (warbandRankJoinUnitTypeData.UnitTypeId == global::UnitTypeId.IMPRESSIVE)
				{
					this.AddAdvancement("warband_adv_impressive_unit_unlock", new string[0]);
				}
				else
				{
					this.AddAdvancement("warband_adv_hero_unit_unlock", new string[0]);
				}
				global::PandoraSingleton<global::Pan>.Instance.Narrate("new_warriors");
			}
			global::WarbandRankData warbandRankData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::WarbandRankData>(id);
			global::WarbandRankData warbandRankData2 = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::WarbandRankData>(warband.Rank);
			if (warbandRankData2.CartSize - warbandRankData.CartSize > 0)
			{
				this.AddAdvancement("warband_adv_cart", new string[]
				{
					(warbandRankData2.CartSize - warbandRankData.CartSize).ToString()
				});
			}
			this.AddAdvancement("hideout_warband_adv_idol_" + warband.Rank, new string[0]);
		}
		this.xpList.gameObject.SetActive(false);
		this.advList.gameObject.SetActive(false);
		base.StartShow(0.5f);
	}

	private void AddXp(string value, string title)
	{
		global::UnityEngine.GameObject gameObject = this.xpList.AddToList();
		global::UIDescription component = gameObject.GetComponent<global::UIDescription>();
		gameObject.SetActive(false);
		component.title.text = value;
		component.desc.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(title);
		this.showQueue.Enqueue(gameObject);
	}

	private void AddAdvancement(string locKey, params string[] parms)
	{
		global::UnityEngine.GameObject gameObject = this.advList.AddToList();
		global::UnityEngine.UI.Text componentInChildren = gameObject.GetComponentInChildren<global::UnityEngine.UI.Text>();
		componentInChildren.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(locKey, parms);
		this.showQueue.Enqueue(gameObject);
		gameObject.SetActive(false);
	}

	public global::ListGroup xpList;

	public global::ListGroup advList;

	public global::UnityEngine.GameObject xpItem;

	public global::UnityEngine.GameObject advItem;
}
