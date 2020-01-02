using System;
using UnityEngine;
using UnityEngine.UI;

public class UIUnitAlternateWeaponGroup : global::UnityEngine.MonoBehaviour
{
	public void Set(global::Item item)
	{
		if (item.Id != global::ItemId.NONE)
		{
			this.icon.enabled = true;
			this.icon.sprite = item.GetIcon();
			this.nameText.enabled = true;
			this.nameText.text = item.LocalizedName;
			if (this.damage != null)
			{
				this.damage.enabled = true;
				if (item.DamageMin == 0 && item.DamageMax == 0)
				{
					this.damage.text = string.Empty;
				}
				else
				{
					this.damage.text = global::PandoraUtils.StringBuilder.Append(item.DamageMin.ToConstantString()).Append('-').Append(item.DamageMax.ToConstantString()).ToString();
				}
			}
		}
		else
		{
			this.icon.enabled = false;
			this.nameText.enabled = false;
			if (this.damage != null)
			{
				this.damage.enabled = false;
			}
		}
	}

	public global::UnityEngine.UI.Image icon;

	public global::UnityEngine.UI.Text nameText;

	public global::UnityEngine.UI.Text damage;
}
