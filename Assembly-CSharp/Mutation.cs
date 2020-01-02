using System;
using System.Collections.Generic;
using UnityEngine;

public class Mutation
{
	public Mutation(global::MutationId id, global::Unit unit)
	{
		this.Data = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::MutationData>((int)id);
		this.GroupData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::MutationGroupData>((int)this.Data.MutationGroupId);
		this.AttributeModifiers = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::MutationAttributeData>("fk_mutation_id", ((int)id).ToConstantString());
		this.RelatedBodyParts = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::MutationGroupBodyPartData>("fk_mutation_group_id", ((int)this.GroupData.Id).ToConstantString());
		global::System.Collections.Generic.List<global::MutationEnchantmentData> list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::MutationEnchantmentData>("fk_mutation_id", ((int)id).ToConstantString());
		this.Enchantments = new global::System.Collections.Generic.List<global::Enchantment>();
		for (int i = 0; i < list.Count; i++)
		{
			this.Enchantments.Add(new global::Enchantment(list[i].EnchantmentId, unit, unit, true, false, global::AllegianceId.NONE, true));
		}
		this.owner = unit;
		this.LabelName = string.Format("mutation_name_{0}", this.Data.Name);
		this.LocName = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(this.LabelName);
	}

	public global::MutationData Data { get; private set; }

	public global::MutationGroupData GroupData { get; private set; }

	public global::System.Collections.Generic.List<global::MutationAttributeData> AttributeModifiers { get; private set; }

	public global::System.Collections.Generic.List<global::Enchantment> Enchantments { get; private set; }

	public global::System.Collections.Generic.List<global::MutationGroupBodyPartData> RelatedBodyParts { get; private set; }

	public string LocName { get; private set; }

	public string LabelName { get; private set; }

	public string LocDesc
	{
		get
		{
			if (this.GroupData.UnitSlotId == global::UnitSlotId.SET1_MAINHAND || this.GroupData.UnitSlotId == global::UnitSlotId.SET1_OFFHAND)
			{
				global::Item item = this.owner.Items[(int)this.GroupData.UnitSlotId];
				global::ItemQualityId itemQualityId = (item != null) ? item.QualityData.Id : global::ItemQualityId.NORMAL;
				return global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(string.Format("mutation_desc_{0}_{1}", this.Data.Name, itemQualityId));
			}
			return global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(string.Format("mutation_desc_{0}", this.Data.Name));
		}
	}

	public global::UnityEngine.Sprite GetIcon()
	{
		return global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadResource<global::UnityEngine.Sprite>("mutation/" + this.Data.Name, true);
	}

	public bool HasBodyPart(global::BodyPartId partId)
	{
		for (int i = 0; i < this.RelatedBodyParts.Count; i++)
		{
			if (this.RelatedBodyParts[i].BodyPartId == partId)
			{
				return true;
			}
		}
		return false;
	}

	private const string LOC_TITLE = "mutation_name_{0}";

	private const string LOC_DESC = "mutation_desc_{0}";

	private const string LOC_DESC_ITEM = "mutation_desc_{0}_{1}";

	private global::Unit owner;
}
