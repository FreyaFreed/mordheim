using System;
using UnityEngine;
using UnityEngine.UI;

public class UIInventoryItem : global::UnityEngine.MonoBehaviour
{
	public void Set(global::Item item, bool shop = false, bool buy = false, global::ItemId restrictedItemId = global::ItemId.NONE, bool flagSold = false)
	{
		this.item = item;
		this.icon.sprite = item.GetIcon();
		this.title.text = item.LocalizedName;
		if (item.Id == global::ItemId.NONE && restrictedItemId != global::ItemId.NONE)
		{
			global::UnityEngine.UI.Text text = this.title;
			text.text = text.text + "\n(" + global::Item.GetLocalizedName(restrictedItemId) + ")";
		}
		if (this.sold != null)
		{
			this.sold.gameObject.SetActive(item.Id != global::ItemId.NONE && flagSold && !item.IsSold());
		}
		global::ItemQualityData itemQualityData = item.QualityData;
		if (item.IsRecipe)
		{
			if (item.TypeData.Id == global::ItemTypeId.RECIPE_ENCHANTMENT_NORMAL)
			{
				itemQualityData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::ItemQualityData>(2);
			}
			else if (item.TypeData.Id == global::ItemTypeId.RECIPE_ENCHANTMENT_MASTERY)
			{
				itemQualityData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::ItemQualityData>(3);
			}
		}
		global::UnityEngine.Color color = global::PandoraUtils.StringToColor(itemQualityData.Color);
		this.title.color = color;
		this.icon.color = color;
		if (this.enchantIcon != null)
		{
			global::UnityEngine.Sprite runeIcon = item.GetRuneIcon();
			this.enchantIcon.gameObject.SetActive(item.RuneMark != null || runeIcon != null);
			if (runeIcon != null)
			{
				this.enchantIcon.sprite = runeIcon;
			}
		}
		if (this.quantity != null)
		{
			this.quantity.gameObject.SetActive(!flagSold);
		}
		this.UpdateQuantity();
		if (this.sellSection != null)
		{
			this.sellSection.SetActive(shop);
		}
		if (shop && this.cost != null)
		{
			int num = (!buy) ? global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.GetItemSellPrice(item) : global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.GetItemBuyPrice(item);
			this.cost.text = num.ToConstantString();
			if (buy && global::PandoraSingleton<global::HideoutManager>.Instance.WarbandChest.GetGold() < num)
			{
				this.cost.color = global::UnityEngine.Color.red;
			}
		}
		if (this.rating != null)
		{
			this.rating.transform.parent.gameObject.SetActive(!flagSold);
			this.rating.text = item.GetRating().ToConstantString();
		}
	}

	public bool UpdateQuantity()
	{
		if (this.quantity != null)
		{
			this.quantity.text = ((this.item.Id != global::ItemId.GOLD && this.item.Save.amount != 0) ? string.Format("x{0}", this.item.Save.amount) : string.Empty);
		}
		return this.item.Save.amount > 0;
	}

	private const string quant = "x{0}";

	public global::UnityEngine.UI.Image icon;

	public global::UnityEngine.UI.Image enchantIcon;

	public global::UnityEngine.UI.Text title;

	public global::UnityEngine.UI.Text quantity;

	public global::UnityEngine.GameObject sellSection;

	public global::UnityEngine.UI.Text cost;

	public global::UnityEngine.UI.Text rating;

	public global::UnityEngine.GameObject sold;

	public global::Item item;
}
