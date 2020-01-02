using System;
using UnityEngine;
using UnityEngine.UI;

public class UIUnitEnchantmentGroup : global::UnityEngine.MonoBehaviour
{
	public void Set(global::Enchantment enchantment)
	{
		this.isBuff = (enchantment.Data.EffectTypeId == global::EffectTypeId.BUFF);
		if (this.isBuff)
		{
			this.nameBuff.gameObject.SetActive(true);
			this.iconBuff.gameObject.SetActive(true);
			this.nameDebuff.gameObject.SetActive(false);
			this.iconDebuff.gameObject.SetActive(false);
			this.nameBuff.text = enchantment.LocalizedName;
		}
		else
		{
			this.nameBuff.gameObject.SetActive(false);
			this.iconBuff.gameObject.SetActive(false);
			this.nameDebuff.gameObject.SetActive(true);
			this.iconDebuff.gameObject.SetActive(true);
			this.nameDebuff.text = enchantment.LocalizedName;
		}
		this.duration.text = enchantment.Duration.ToConstantString();
		this.description.text = enchantment.LocalizedDescription;
	}

	public bool isBuff;

	public global::UnityEngine.UI.Image iconBuff;

	public global::UnityEngine.UI.Image iconDebuff;

	public global::UnityEngine.UI.Text nameBuff;

	public global::UnityEngine.UI.Text nameDebuff;

	public global::UnityEngine.UI.Text description;

	public global::UnityEngine.UI.Text duration;
}
