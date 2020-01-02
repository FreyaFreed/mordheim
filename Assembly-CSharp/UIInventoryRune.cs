using System;
using UnityEngine;
using UnityEngine.UI;

public class UIInventoryRune : global::UnityEngine.MonoBehaviour
{
	public void Set(global::RuneMark rune)
	{
		this.icon.sprite = rune.GetIcon();
		if (this.title != null)
		{
			this.title.text = rune.FullLocName;
		}
		if (this.quality != null)
		{
			this.quality.text = rune.LocQuality;
		}
		if (this.redSplatter != null)
		{
			this.redSplatter.enabled = !string.IsNullOrEmpty(rune.Reason);
		}
		if (this.cannotEnchant != null)
		{
			this.cannotEnchant.enabled = !string.IsNullOrEmpty(rune.Reason);
		}
		if (this.notFound != null)
		{
			if (string.IsNullOrEmpty(rune.Reason))
			{
				this.notFound.enabled = false;
			}
			else
			{
				this.notFound.enabled = true;
				this.notFound.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(rune.Reason);
			}
		}
		if (this.cost != null)
		{
			this.cost.enabled = true;
			this.cost.text = global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.GetRuneMarkBuyPrice(rune).ToConstantString();
			if (global::PandoraSingleton<global::HideoutManager>.Instance.WarbandChest.GetGold() < global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.GetRuneMarkBuyPrice(rune))
			{
				this.cost.color = global::UnityEngine.Color.red;
			}
		}
		if (this.rating != null)
		{
			this.rating.text = rune.QualityItemTypeData.Rating.ToConstantString();
		}
		if (this.desc != null)
		{
			this.desc.text = rune.LocDesc;
		}
	}

	public void Set(global::Item item)
	{
		global::UnityEngine.Sprite runeIcon = item.GetRuneIcon();
		if (runeIcon == null)
		{
			base.gameObject.SetActive(false);
		}
		else
		{
			base.gameObject.SetActive(true);
			this.icon.sprite = runeIcon;
			if (this.title != null)
			{
				this.title.text = item.GetRuneNameDesc();
			}
		}
	}

	public global::UnityEngine.UI.Image icon;

	public global::UnityEngine.UI.Image redSplatter;

	public global::UnityEngine.UI.Image cannotEnchant;

	public global::UnityEngine.UI.Text title;

	public global::UnityEngine.UI.Text quality;

	public global::UnityEngine.UI.Text cost;

	public global::UnityEngine.UI.Text desc;

	public global::UnityEngine.UI.Text rating;

	public global::UnityEngine.UI.Text notFound;
}
