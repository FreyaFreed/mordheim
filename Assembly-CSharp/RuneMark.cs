using System;
using System.Collections.Generic;
using UnityEngine;

public class RuneMark
{
	public RuneMark(global::RuneMarkId id, global::RuneMarkQualityId qualityId, global::AllegianceId allegianceId, global::ItemTypeId itemTypeId, string reason = null)
	{
		this.Reason = reason;
		this.AllegianceId = allegianceId;
		this.Data = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::RuneMarkData>((int)id);
		this.QualityData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::RuneMarkQualityData>((int)qualityId);
		if (itemTypeId == global::ItemTypeId.NONE)
		{
			itemTypeId = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::ItemQualityJoinItemTypeData>("fk_item_type_id", this.QualityData.Id.ToIntString<global::RuneMarkQualityId>())[0].ItemTypeId;
		}
		global::System.Collections.Generic.List<global::RuneMarkQualityJoinItemTypeData> list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::RuneMarkQualityJoinItemTypeData>(new string[]
		{
			"fk_rune_mark_quality_id",
			"fk_item_type_id"
		}, new string[]
		{
			((int)qualityId).ToConstantString(),
			((int)itemTypeId).ToConstantString()
		});
		this.QualityItemTypeData = list[0];
		this.AttributeModifiers = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::RuneMarkAttributeData>(new string[]
		{
			"fk_rune_mark_id",
			"fk_rune_mark_quality_id",
			"fk_item_type_id"
		}, new string[]
		{
			((int)this.Data.Id).ToConstantString(),
			((int)this.QualityData.Id).ToConstantString(),
			((int)itemTypeId).ToConstantString()
		});
		global::System.Collections.Generic.List<global::RuneMarkEnchantmentData> list2 = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::RuneMarkEnchantmentData>(new string[]
		{
			"fk_rune_mark_id",
			"fk_rune_mark_quality_id",
			"fk_item_type_id"
		}, new string[]
		{
			((int)this.Data.Id).ToConstantString(),
			((int)this.QualityData.Id).ToConstantString(),
			((int)itemTypeId).ToConstantString()
		});
		this.Enchantments = new global::System.Collections.Generic.List<global::Enchantment>();
		for (int i = 0; i < list2.Count; i++)
		{
			this.Enchantments.Add(new global::Enchantment(list2[i].EnchantmentId, null, null, true, true, this.AllegianceId, true));
		}
		this.LabelName = "item_enchant_name_" + this.Data.Name + ((this.QualityData.Id != global::RuneMarkQualityId.MASTER) ? string.Empty : "_mstr");
		this.FullLabel = "#" + ((this.AllegianceId != global::AllegianceId.ORDER) ? "item_enchant_type_mark" : "item_enchant_type_rune") + " #" + this.LabelName;
	}

	public string Name
	{
		get
		{
			return this.Data.Name;
		}
	}

	public string FullLabel { get; set; }

	public string FullLocName
	{
		get
		{
			if (string.IsNullOrEmpty(this.fullLocName))
			{
				this.fullLocName = this.Color + global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById((this.AllegianceId != global::AllegianceId.ORDER) ? "item_enchant_type_mark_param" : "item_enchant_type_rune_param", new string[]
				{
					this.SuffixLocName
				}) + "</color>";
			}
			return this.fullLocName;
		}
	}

	public string LocDesc
	{
		get
		{
			if (string.IsNullOrEmpty(this.locDesc))
			{
				this.locDesc = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("item_enchant_desc_" + this.Data.Name + ((this.QualityData.Id != global::RuneMarkQualityId.MASTER) ? string.Empty : "_mstr"));
			}
			return this.locDesc;
		}
	}

	public string LocShort
	{
		get
		{
			if (string.IsNullOrEmpty(this.locShort))
			{
				this.locShort = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("item_enchant_short_" + this.Data.Name + ((this.QualityData.Id != global::RuneMarkQualityId.MASTER) ? string.Empty : "_mstr"));
			}
			return this.locShort;
		}
	}

	public string LocQuality
	{
		get
		{
			if (string.IsNullOrEmpty(this.locQuality))
			{
				this.locQuality = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("rune_quality_" + this.QualityData.Name);
			}
			return this.locQuality;
		}
	}

	public string SuffixLocName
	{
		get
		{
			if (string.IsNullOrEmpty(this.suffixLocName))
			{
				this.suffixLocName = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(this.LabelName);
			}
			return this.suffixLocName;
		}
	}

	public string Reason { get; set; }

	public string Color
	{
		get
		{
			if (string.IsNullOrEmpty(this.color))
			{
				this.color = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById((this.QualityData.Id != global::RuneMarkQualityId.REGULAR) ? "color_item_purple" : "color_item_blue");
			}
			return this.color;
		}
	}

	public string LabelName { get; set; }

	public global::RuneMarkData Data { get; private set; }

	public global::RuneMarkQualityData QualityData { get; private set; }

	public global::RuneMarkQualityJoinItemTypeData QualityItemTypeData { get; private set; }

	public global::System.Collections.Generic.List<global::RuneMarkAttributeData> AttributeModifiers { get; private set; }

	public global::System.Collections.Generic.List<global::Enchantment> Enchantments { get; private set; }

	public global::AllegianceId AllegianceId { get; private set; }

	public int Cost
	{
		get
		{
			return this.Data.Cost * this.QualityItemTypeData.CostModifier;
		}
	}

	public global::UnityEngine.Sprite GetIcon()
	{
		return global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadResource<global::UnityEngine.Sprite>("runemark/" + this.QualityData.Name, true);
	}

	private const string RUNE_NAME_PARAM = "item_enchant_type_rune_param";

	private const string MARK_NAME_PARAM = "item_enchant_type_mark_param";

	private const string RUNE_NAME = "item_enchant_type_rune";

	private const string MARK_NAME = "item_enchant_type_mark";

	private string fullLocName;

	private string locDesc;

	private string locShort;

	private string locQuality;

	private string suffixLocName;

	private string color;
}
