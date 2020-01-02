using System;
using System.Collections.Generic;

public class WarbandEnchantment
{
	public WarbandEnchantment(global::WarbandEnchantmentId warbandEnchantmentId)
	{
		this.Data = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::WarbandEnchantmentData>((int)warbandEnchantmentId);
		this.Attributes = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::WarbandEnchantmentAttributeData>("fk_warband_enchantment_id", warbandEnchantmentId.ToIntString<global::WarbandEnchantmentId>());
		this.WyrdStoneDensityModifiers = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::WarbandEnchantmentWyrdstoneDensityModifierData>("fk_warband_enchantment_id", warbandEnchantmentId.ToIntString<global::WarbandEnchantmentId>());
		this.SearchDensityModifiers = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::WarbandEnchantmentSearchDensityModifierData>("fk_warband_enchantment_id", warbandEnchantmentId.ToIntString<global::WarbandEnchantmentId>());
		this.MarketEventModifiers = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::WarbandEnchantmentMarketModifierData>("fk_warband_enchantment_id", warbandEnchantmentId.ToIntString<global::WarbandEnchantmentId>());
	}

	public global::WarbandEnchantmentId Id
	{
		get
		{
			return this.Data.Id;
		}
	}

	public global::WarbandEnchantmentData Data { get; private set; }

	public global::System.Collections.Generic.List<global::WarbandEnchantmentAttributeData> Attributes { get; private set; }

	public global::System.Collections.Generic.List<global::WarbandEnchantmentWyrdstoneDensityModifierData> WyrdStoneDensityModifiers { get; private set; }

	public global::System.Collections.Generic.List<global::WarbandEnchantmentSearchDensityModifierData> SearchDensityModifiers { get; private set; }

	public global::System.Collections.Generic.List<global::WarbandEnchantmentMarketModifierData> MarketEventModifiers { get; private set; }

	public string LocalizedName
	{
		get
		{
			return global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(this.LabelName);
		}
	}

	public string LabelName { get; private set; }

	public string LocalizedDescription
	{
		get
		{
			return global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("enchant_desc_" + this.Data.Name);
		}
	}
}
