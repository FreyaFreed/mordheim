using System;
using System.Collections.Generic;

public class Injury
{
	public Injury(global::InjuryId id, global::Unit unit)
	{
		this.Data = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::InjuryData>((int)id);
		this.AttributeModifiers = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::InjuryJoinAttributeData>("fk_injury_id", ((int)id).ToConstantString());
		global::System.Collections.Generic.List<global::InjuryJoinEnchantmentData> list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::InjuryJoinEnchantmentData>("fk_injury_id", ((int)id).ToConstantString());
		this.Enchantments = new global::System.Collections.Generic.List<global::Enchantment>();
		for (int i = 0; i < list.Count; i++)
		{
			this.Enchantments.Add(new global::Enchantment(list[i].EnchantmentId, unit, unit, true, false, global::AllegianceId.NONE, true));
		}
		this.LabelName = string.Format("injury_name_{0}", this.Data.Name);
		this.LocName = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(this.LabelName);
		this.LabelDesc = string.Format("injury_desc_{0}", this.Data.Name);
		this.LocDesc = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(this.LabelDesc);
	}

	public global::InjuryData Data { get; private set; }

	public global::System.Collections.Generic.List<global::InjuryJoinAttributeData> AttributeModifiers { get; private set; }

	public global::System.Collections.Generic.List<global::Enchantment> Enchantments { get; private set; }

	public string LocName { get; private set; }

	public string LocDesc { get; private set; }

	public string LabelName { get; private set; }

	public string LabelDesc { get; private set; }

	private const string LOC_TITLE = "injury_name_{0}";

	private const string LOC_DESC = "injury_desc_{0}";
}
