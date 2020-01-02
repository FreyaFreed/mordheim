using System;
using UnityEngine;
using UnityEngine.UI;

public class UIWheelDescription : global::UnityEngine.MonoBehaviour
{
	public void SetCurrentAction(global::WheelAction action)
	{
		this.categoryText.enabled = true;
		this.backgroundImage.gameObject.SetActive(true);
		this.backgroundImageSmall.gameObject.SetActive(false);
		switch (action.category)
		{
		case global::UIWheelController.Category.BASE_ACTION:
		case global::UIWheelController.Category.ACTIVE_SKILL:
		case global::UIWheelController.Category.PASSIVE_SKILL:
		case global::UIWheelController.Category.STANCES:
			this.spellDesc.gameObject.SetActive(false);
			this.descriptionText.gameObject.SetActive(false);
			this.itemDesc.gameObject.SetActive(false);
			this.itemTitle.gameObject.SetActive(false);
			this.skillDesc.Set(action.action.skillData, null);
			break;
		case global::UIWheelController.Category.SPELLS:
			this.skillDesc.gameObject.SetActive(false);
			this.descriptionText.gameObject.SetActive(false);
			this.itemDesc.gameObject.SetActive(false);
			this.itemTitle.gameObject.SetActive(false);
			this.spellDesc.Set(action.action.skillData, null);
			break;
		case global::UIWheelController.Category.INVENTORY:
			this.mastery.enabled = false;
			this.skillDesc.gameObject.SetActive(false);
			this.descriptionText.gameObject.SetActive(false);
			this.itemDesc.Set(action.item, null);
			this.itemTitle.Set(action.item, false, false, global::ItemId.NONE, false);
			this.spellDesc.gameObject.SetActive(false);
			break;
		}
		if (action.action != null)
		{
			this.nonAvailableText.text = action.action.LocalizedNotAvailableReason;
		}
		else
		{
			this.nonAvailableText.text = string.Empty;
		}
		if (action.Available)
		{
			this.titleBackgroundImage.sprite = this.titleBgAvailable;
			this.nonAvailableText.gameObject.SetActive(false);
		}
		else
		{
			this.nonAvailableText.gameObject.SetActive(true);
			this.titleBackgroundImage.sprite = this.titleBgNonAvailable;
		}
	}

	public void SetCurrentCategory(global::UIWheelController.Category category)
	{
		this.nonAvailableText.gameObject.SetActive(false);
		this.icon.sprite = this.wheelController.innerWheelIcons[(int)category].icon.sprite;
		this.titleText.color = global::UnityEngine.Color.white;
		this.titleBackgroundImage.sprite = this.titleBgAvailable;
		this.categoryText.enabled = false;
		this.mastery.enabled = false;
		this.backgroundImage.gameObject.SetActive(false);
		this.backgroundImageSmall.gameObject.SetActive(true);
		if (category == global::UIWheelController.Category.INVENTORY)
		{
			global::UnityEngine.UI.Text text = this.categoryText;
			string stringById = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("skill_category_title_" + category.ToLowerString(), new string[]
			{
				this.wheelController.CurrentUnitController.unit.GetNumUsedItemSlot().ToConstantString(),
				this.wheelController.CurrentUnitController.unit.BackpackCapacity.ToConstantString()
			});
			this.titleText.text = stringById;
			text.text = stringById;
		}
		else
		{
			global::UnityEngine.UI.Text text2 = this.categoryText;
			string stringById = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("skill_category_title_" + category.ToLowerString());
			this.titleText.text = stringById;
			text2.text = stringById;
		}
		this.descriptionText.gameObject.SetActive(true);
		this.skillDesc.gameObject.SetActive(false);
		this.spellDesc.gameObject.SetActive(false);
		this.itemDesc.gameObject.SetActive(false);
		this.descriptionText.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("skill_category_desc_" + category.ToLowerString());
	}

	public global::UIWheelController wheelController;

	public global::UnityEngine.UI.Image backgroundImage;

	public global::UnityEngine.UI.Image backgroundImageSmall;

	public global::UnityEngine.UI.Image titleBackgroundImage;

	public global::UnityEngine.UI.Image icon;

	public global::UnityEngine.UI.Text titleText;

	public global::UnityEngine.UI.Image mastery;

	public global::UnityEngine.UI.Text categoryText;

	public global::UnityEngine.UI.Text descriptionText;

	public global::UnityEngine.UI.Text nonAvailableText;

	public global::UnityEngine.Sprite titleBgAvailable;

	public global::UnityEngine.Sprite titleBgNonAvailable;

	public global::SkillDescModule skillDesc;

	public global::SpellDescModule spellDesc;

	public global::UIInventoryItemDescription itemDesc;

	public global::UIInventoryItem itemTitle;
}
