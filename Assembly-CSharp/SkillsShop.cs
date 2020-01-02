using System;
using System.Collections.Generic;

public class SkillsShop
{
	public void GetSkills(global::Unit unit, global::System.Collections.Generic.List<global::SkillLineId> skillLines, bool active, ref global::System.Collections.Generic.List<global::SkillData> canLearnSkills, ref global::System.Collections.Generic.List<global::SkillData> cannotLearnSkills)
	{
		canLearnSkills.Clear();
		cannotLearnSkills.Clear();
		for (int i = 0; i < skillLines.Count; i++)
		{
			global::System.Collections.Generic.List<global::SkillLineJoinSkillData> list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::SkillLineJoinSkillData>("fk_skill_line_id", skillLines[i].ToIntString<global::SkillLineId>());
			for (int j = 0; j < list.Count; j++)
			{
				global::SkillData skillData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::SkillData>((int)list[j].SkillId);
				if (skillData.Passive != active && skillData.Released && skillData.SkillIdPrerequiste == global::SkillId.NONE && this.CanShowSkill(unit, skillData))
				{
					if (this.CanLearnSkill(skillData))
					{
						canLearnSkills.Add(skillData);
					}
					else
					{
						cannotLearnSkills.Add(skillData);
					}
				}
			}
		}
		canLearnSkills.Sort(this.skillsComparer);
		cannotLearnSkills.Sort(this.skillsComparer);
	}

	public bool UnitHasSkillLine(global::Unit unit, global::SkillLineId skillLineId)
	{
		global::System.Collections.Generic.List<global::UnitJoinSkillLineData> list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::UnitJoinSkillLineData>("fk_unit_id", unit.Id.ToIntString<global::UnitId>());
		foreach (global::UnitJoinSkillLineData unitJoinSkillLineData in list)
		{
			global::SkillLineData skillLineData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::SkillLineData>((int)unitJoinSkillLineData.SkillLineId);
			if (skillLineData.SkillLineIdDisplayed == skillLineId)
			{
				return true;
			}
		}
		return false;
	}

	public bool CanLearnSkill(global::SkillData skillData)
	{
		string text;
		return this.CanLearnSkill(skillData, out text);
	}

	public bool IsSkillChangeType(global::SkillId skillId)
	{
		global::System.Collections.Generic.List<global::SkillLearnBonusData> list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::SkillLearnBonusData>("fk_skill_id", skillId.ToIntString<global::SkillId>());
		return list.Count == 1 && list[0].UnitTypeId != global::UnitTypeId.NONE;
	}

	public bool CanLearnSkill(global::SkillData skillData, out string reason)
	{
		if (global::PandoraSingleton<global::HideoutManager>.Instance.currentUnit.unit.CanLearnSkill(skillData, out reason))
		{
			if (this.IsSkillChangeType(skillData.Id) && global::PandoraSingleton<global::HideoutManager>.Instance.currentUnit.unit.UnitSave.warbandSlotIndex < 12)
			{
				reason = "na_skill_warband_slot";
				return false;
			}
			if (global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.GetSkillLearnPrice(skillData, global::PandoraSingleton<global::HideoutManager>.Instance.currentUnit.unit) > global::PandoraSingleton<global::HideoutManager>.Instance.WarbandChest.GetGold())
			{
				reason = "na_skill_not_enough_money";
				return false;
			}
		}
		return reason == null;
	}

	public bool CanShowSkill(global::Unit unit, global::SkillData skillData)
	{
		if (unit.HasSkillOrSpell(skillData.Id))
		{
			return false;
		}
		if (!global::SkillHelper.IsMastery(skillData))
		{
			global::SkillData skillMastery = global::SkillHelper.GetSkillMastery(skillData);
			if (skillMastery != null && unit.HasSkillOrSpell(skillMastery.Id))
			{
				return false;
			}
		}
		return skillData.SkillIdPrerequiste == global::SkillId.NONE || unit.HasSkillOrSpell(skillData.SkillIdPrerequiste);
	}

	public global::System.Collections.Generic.Dictionary<global::SkillLineId, global::System.Collections.Generic.List<global::SkillLineId>> GetUnitSkillLines(global::Unit unit)
	{
		global::System.Collections.Generic.Dictionary<global::SkillLineId, global::System.Collections.Generic.List<global::SkillLineId>> dictionary = new global::System.Collections.Generic.Dictionary<global::SkillLineId, global::System.Collections.Generic.List<global::SkillLineId>>();
		global::System.Collections.Generic.List<global::UnitJoinSkillLineData> list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::UnitJoinSkillLineData>("fk_unit_id", unit.Id.ToIntString<global::UnitId>());
		foreach (global::UnitJoinSkillLineData unitJoinSkillLineData in list)
		{
			global::SkillsShop.AddSkillLineToDictionary(unitJoinSkillLineData.SkillLineId, dictionary);
		}
		global::System.Collections.Generic.List<global::UnitTypeJoinSkillLineData> list2 = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::UnitTypeJoinSkillLineData>("fk_unit_type_id", unit.GetUnitTypeId().ToIntString<global::UnitTypeId>());
		foreach (global::UnitTypeJoinSkillLineData unitTypeJoinSkillLineData in list2)
		{
			global::SkillsShop.AddSkillLineToDictionary(unitTypeJoinSkillLineData.SkillLineId, dictionary);
		}
		return dictionary;
	}

	private static void AddSkillLineToDictionary(global::SkillLineId skillLineId, global::System.Collections.Generic.Dictionary<global::SkillLineId, global::System.Collections.Generic.List<global::SkillLineId>> dict)
	{
		global::SkillLineData skillLineData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::SkillLineData>((int)skillLineId);
		global::System.Collections.Generic.List<global::SkillLineId> list;
		if (!dict.TryGetValue(skillLineData.SkillLineIdDisplayed, out list))
		{
			list = new global::System.Collections.Generic.List<global::SkillLineId>();
			dict.Add(skillLineData.SkillLineIdDisplayed, list);
		}
		list.Add(skillLineData.Id);
	}

	public global::System.Collections.Generic.List<global::SkillLineId> GetUnitSpellsSkillLines(global::Unit unit)
	{
		global::System.Collections.Generic.List<global::SkillLineId> list = new global::System.Collections.Generic.List<global::SkillLineId>();
		global::System.Collections.Generic.List<global::UnitJoinSkillLineData> list2 = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::UnitJoinSkillLineData>("fk_unit_id", unit.Id.ToIntString<global::UnitId>());
		foreach (global::UnitJoinSkillLineData unitJoinSkillLineData in list2)
		{
			global::SkillLineData skillLineData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::SkillLineData>((int)unitJoinSkillLineData.SkillLineId);
			if (skillLineData.SkillLineIdDisplayed == global::SkillLineId.SPELL)
			{
				list.Add(skillLineData.Id);
			}
		}
		return list;
	}

	private readonly global::BuySkillsComparer skillsComparer = new global::BuySkillsComparer();
}
