using System;
using System.Collections.Generic;
using UnityEngine;

public class WarbandSkill
{
	public WarbandSkill(global::WarbandSkillId id)
	{
		this.Data = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::WarbandSkillData>((int)id);
		this.Enchantments = new global::System.Collections.Generic.List<global::WarbandEnchantment>();
		global::System.Collections.Generic.List<global::WarbandSkillWarbandEnchantmentData> list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::WarbandSkillWarbandEnchantmentData>("fk_warband_skill_id", ((int)this.Id).ToString());
		this.HireableUnits = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::WarbandSkillUnitData>("fk_warband_skill_id", ((int)this.Id).ToString());
		this.UnitTypeRankDatas = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::WarbandSkillUnitTypeRankData>("fk_warband_skill_id", ((int)this.Id).ToString());
		this.ContactsData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::WarbandSkillWarbandContactData>("fk_warband_skill_id", ((int)this.Id).ToString());
		this.MarketItems = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::WarbandSkillMarketItemData>("fk_warband_skill_id", ((int)this.Id).ToString());
		for (int i = 0; i < list.Count; i++)
		{
			this.Enchantments.Add(new global::WarbandEnchantment(list[i].WarbandEnchantmentId));
		}
	}

	public global::WarbandSkillId Id
	{
		get
		{
			return this.Data.Id;
		}
	}

	public global::WarbandSkillTypeId TypeId
	{
		get
		{
			return this.Data.WarbandSkillTypeId;
		}
	}

	public bool IsMastery
	{
		get
		{
			return this.Data.SkillQualityId == global::SkillQualityId.MASTER_QUALITY;
		}
	}

	public global::WarbandSkillData Data { get; private set; }

	public global::System.Collections.Generic.List<global::WarbandEnchantment> Enchantments { get; private set; }

	public global::System.Collections.Generic.List<global::WarbandSkillUnitData> HireableUnits { get; private set; }

	public global::System.Collections.Generic.List<global::WarbandSkillUnitTypeRankData> UnitTypeRankDatas { get; private set; }

	public global::System.Collections.Generic.List<global::WarbandSkillWarbandContactData> ContactsData { get; private set; }

	public global::System.Collections.Generic.List<global::WarbandSkillMarketItemData> MarketItems { get; private set; }

	public bool CanBuy
	{
		get
		{
			return this.Data.Points <= global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.GetPlayerSkillsAvailablePoints();
		}
	}

	public string LocalizedName
	{
		get
		{
			if (this.locName == null)
			{
				this.locName = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("warband_skill_title_" + this.Data.Name);
			}
			return this.locName;
		}
	}

	public string LocalizedDesc
	{
		get
		{
			if (this.locDesc == null)
			{
				this.locDesc = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("warband_skill_desc_" + this.Data.Name);
			}
			return this.locDesc;
		}
	}

	public global::WarbandSkillData GetSkillMastery()
	{
		if (this.Data.SkillQualityId == global::SkillQualityId.NORMAL_QUALITY)
		{
			global::System.Collections.Generic.List<global::WarbandSkillData> list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::WarbandSkillData>("fk_warband_skill_id_prerequisite", this.Data.Id.ToIntString<global::WarbandSkillId>());
			if (list.Count > 0)
			{
				return list[0];
			}
		}
		return null;
	}

	public global::UnityEngine.Sprite GetIcon()
	{
		if (this.Data.WarbandSkillIdPrerequisite != global::WarbandSkillId.NONE)
		{
			return global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadResource<global::UnityEngine.Sprite>("warband_skill/" + this.Data.WarbandSkillIdPrerequisite.ToLowerString(), true);
		}
		return global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadResource<global::UnityEngine.Sprite>("warband_skill/" + this.Data.Name, true);
	}

	public static global::UnityEngine.Sprite GetIcon(global::WarbandSkill skill)
	{
		if (skill != null)
		{
			return skill.GetIcon();
		}
		return global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadResource<global::UnityEngine.Sprite>("item/add", true);
	}

	public static string LocName(global::WarbandSkill skill)
	{
		if (skill != null)
		{
			return skill.LocalizedName;
		}
		return global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("warband_skill_slot_available_title");
	}

	public static string LocDesc(global::WarbandSkill skill)
	{
		if (skill != null)
		{
			return skill.LocalizedDesc;
		}
		return global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("warband_skill_slot_available_desc");
	}

	private string locName;

	private string locDesc;
}
