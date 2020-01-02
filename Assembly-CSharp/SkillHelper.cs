using System;
using System.Collections.Generic;
using UnityEngine;

public static class SkillHelper
{
	public static bool IsMastery(global::SkillData skillData)
	{
		return skillData.SkillQualityId == global::SkillQualityId.MASTER_QUALITY;
	}

	public static global::SkillData GetSkillMastery(global::SkillData skillData)
	{
		if (skillData.SkillQualityId == global::SkillQualityId.NORMAL_QUALITY)
		{
			global::System.Collections.Generic.List<global::SkillData> list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::SkillData>("fk_skill_id_prerequiste", skillData.Id.ToIntString<global::SkillId>());
			global::SkillData result = null;
			if (list.Count > 0)
			{
				result = list[0];
			}
			return result;
		}
		return null;
	}

	public static global::System.Collections.Generic.List<global::SkillLearnBonusData> GetSkillLearnBonus(global::SkillId skillId)
	{
		return global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::SkillLearnBonusData>("fk_skill_id", ((int)skillId).ToConstantString());
	}

	public static bool HasMastery(global::SkillData skillData)
	{
		return global::SkillHelper.GetSkillMastery(skillData) != null;
	}

	public static string GetLocalizedMasteryDescription(global::SkillId skillId)
	{
		return global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("skill_desc_" + skillId.ToLowerString() + "_hideout");
	}

	public static string GetLocalizedRange(global::SkillData skillData)
	{
		return global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("skill_range", new string[]
		{
			skillData.Range.ToConstantString()
		});
	}

	public static string GetLocalizedCasting(global::SkillData skillData)
	{
		global::System.Collections.Generic.List<global::SkillAttributeData> list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::SkillAttributeData>(new string[]
		{
			"fk_skill_id",
			"fk_attribute_id"
		}, new string[]
		{
			skillData.Id.ToIntString<global::SkillId>(),
			global::AttributeId.SPELLCASTING_ROLL.ToIntString<global::AttributeId>()
		});
		if (list.Count > 0)
		{
			return list[0].Modifier.ToString("+#;-#") + "%";
		}
		return "0%";
	}

	public static string GetLocalizedDuration(global::SkillData skillData)
	{
		int num = 0;
		if (skillData.ZoneAoeId != global::ZoneAoeId.NONE)
		{
			global::ZoneAoeData zoneAoeData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::ZoneAoeData>((int)skillData.ZoneAoeId);
			if (zoneAoeData != null)
			{
				num = zoneAoeData.Duration;
			}
		}
		else if (skillData.DestructibleId != global::DestructibleId.NONE)
		{
			global::DestructibleData destructibleData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::DestructibleData>((int)skillData.DestructibleId);
			global::ZoneAoeData zoneAoeData2 = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::ZoneAoeData>((int)destructibleData.ZoneAoeId);
			if (zoneAoeData2 != null)
			{
				num = zoneAoeData2.Duration;
			}
		}
		else
		{
			global::System.Collections.Generic.List<global::SkillEnchantmentData> list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::SkillEnchantmentData>("fk_skill_id", skillData.Id.ToIntString<global::SkillId>());
			for (int i = 0; i < list.Count; i++)
			{
				global::EnchantmentData enchantmentData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::EnchantmentData>((int)list[i].EnchantmentId);
				if (enchantmentData != null)
				{
					num = global::UnityEngine.Mathf.Max(num, enchantmentData.Duration);
				}
			}
		}
		return global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("skill_duration", new string[]
		{
			num.ToConstantString()
		});
	}

	public static string GetLocalizedCurse(global::SkillData skillData)
	{
		global::System.Collections.Generic.List<global::SkillAttributeData> list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::SkillAttributeData>(new string[]
		{
			"fk_skill_id",
			"fk_attribute_id"
		}, new string[]
		{
			skillData.Id.ToIntString<global::SkillId>(),
			(skillData.SpellTypeId != global::SpellTypeId.ARCANE) ? global::AttributeId.DIVINE_WRATH_ROLL.ToIntString<global::AttributeId>() : global::AttributeId.TZEENTCHS_CURSE_ROLL.ToIntString<global::AttributeId>()
		});
		if (list.Count > 0)
		{
			return list[0].Modifier.ToString("+#;-#") + "%";
		}
		return "0%";
	}

	public static string GetLocalizedDescription(global::SkillId skillId)
	{
		return global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("skill_desc_" + skillId.ToLowerString());
	}

	public static string GetLocalizedName(global::SkillId skillId)
	{
		return global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("skill_name_" + skillId.ToLowerString());
	}

	public static string GetLocalizedName(global::SkillData skillData)
	{
		return global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("skill_name_" + skillData.Name);
	}

	public static string GetLocalizedRequirement(global::SkillData skillData)
	{
		if (skillData.AttributeIdStat == global::AttributeId.NONE)
		{
			return string.Empty;
		}
		return global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("skill_requirement", new string[]
		{
			skillData.StatValue.ToConstantString(),
			"#attribute_name_" + skillData.AttributeIdStat.ToLowerString()
		});
	}

	public static string GetLocalizedTrainingTime(global::SkillData skillData)
	{
		return global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("skill_training_time", new string[]
		{
			skillData.Time.ToConstantString()
		});
	}

	public static global::UnityEngine.Sprite GetIcon(global::SkillData skillData)
	{
		global::UnityEngine.Sprite icon = global::SkillHelper.GetIcon(skillData.Name);
		if (icon == null)
		{
			icon = global::SkillHelper.GetIcon(skillData.UnitActionId.ToLowerString());
		}
		return icon;
	}

	public static global::SkillData GetSkill(global::SkillId skillId)
	{
		return global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::SkillData>((int)skillId);
	}

	private static global::UnityEngine.Sprite GetIcon(string name)
	{
		global::UnityEngine.Sprite sprite = global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadResource<global::UnityEngine.Sprite>("action/" + name.ToLowerString(), true);
		if (sprite == null)
		{
			sprite = global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadResource<global::UnityEngine.Sprite>("action/" + name.ToLowerInvariant().Replace("base_", string.Empty).Replace("_mstr", string.Empty), true);
		}
		return sprite;
	}

	public static global::SkillLineId GetSkillLineId(global::SkillId skillId, global::UnitId unitId)
	{
		global::System.Collections.Generic.List<global::SkillLineJoinSkillData> list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::SkillLineJoinSkillData>("fk_skill_id", skillId.ToIntString<global::SkillId>());
		if (list.Count == 1)
		{
			return list[0].SkillLineId;
		}
		if (list.Count > 1)
		{
			global::System.Collections.Generic.List<global::UnitJoinSkillLineData> list2 = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::UnitJoinSkillLineData>("fk_unit_id", unitId.ToIntString<global::UnitId>());
			for (int i = 0; i < list.Count; i++)
			{
				for (int j = 0; j < list2.Count; j++)
				{
					if (list[i].SkillLineId == list2[j].SkillLineId)
					{
						return list[i].SkillLineId;
					}
				}
			}
		}
		return global::SkillLineId.NONE;
	}

	public static global::SkillLineData GetBaseSkillLine(global::SkillData skillData, global::UnitId unitId)
	{
		global::SkillLineId skillLineId = global::SkillHelper.GetSkillLineId(skillData.Id, unitId);
		global::SkillLineData skillLineData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::SkillLineData>((int)skillLineId);
		return global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::SkillLineData>((int)skillLineData.SkillLineIdDisplayed);
	}

	public static int GetRating(global::SkillData skillData)
	{
		global::SkillQualityData skillQualityData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::SkillQualityData>((int)skillData.SkillQualityId);
		return skillQualityData.Rating;
	}
}
