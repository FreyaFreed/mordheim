using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class UIUnitEnchantmentsController : global::UIUnitControllerChanged
{
	protected virtual void Awake()
	{
		global::PandoraSingleton<global::NoticeManager>.Instance.RegisterListener(global::Notices.UNIT_ENCHANTMENTS_CHANGED, new global::DelReceiveNotice(this.OnEnchantmentsChanged));
		this.enchantmentComparer = new global::EnchantmentComparer();
	}

	private void OnEnchantmentsChanged()
	{
		global::Unit unit = global::PandoraSingleton<global::NoticeManager>.Instance.Parameters[0] as global::Unit;
		if (base.CurrentUnitController != null && base.CurrentUnitController.unit == unit)
		{
			base.UpdateUnit = true;
		}
	}

	private void EnchantmentsChanged()
	{
		this.enchantments.Clear();
		if (base.CurrentUnitController != null)
		{
			this.enchantments.AddRange(base.CurrentUnitController.unit.Enchantments);
			for (int i = 0; i < base.CurrentUnitController.unit.ActiveItems.Count; i++)
			{
				this.enchantments.AddRange(base.CurrentUnitController.unit.ActiveItems[i].Enchantments);
			}
			for (int j = 0; j < base.CurrentUnitController.unit.Injuries.Count; j++)
			{
				this.enchantments.AddRange(base.CurrentUnitController.unit.Injuries[j].Enchantments);
			}
			for (int k = 0; k < base.CurrentUnitController.unit.Mutations.Count; k++)
			{
				this.enchantments.AddRange(base.CurrentUnitController.unit.Mutations[k].Enchantments);
			}
			this.enchantments.Sort(this.enchantmentComparer);
			for (int l = 0; l < this.enchantments.Count; l++)
			{
				if (base.CurrentUnitController.CanShowEnchantment(this.enchantments[l], (!this.showBuff) ? global::EffectTypeId.DEBUFF : global::EffectTypeId.BUFF))
				{
					this.content.Add(this.enchantments[l]);
				}
			}
			this.unitClass.text = base.CurrentUnitController.unit.LocalizedType;
			this.content.OnAddEnd();
		}
	}

	protected override void OnUnitChanged()
	{
		this.EnchantmentsChanged();
	}

	public global::UIUnitEnchantmentsContent content;

	private global::System.Collections.Generic.List<global::Enchantment> enchantments = new global::System.Collections.Generic.List<global::Enchantment>();

	public global::UnityEngine.UI.Text unitClass;

	private global::EnchantmentComparer enchantmentComparer;

	public bool showBuff;
}
