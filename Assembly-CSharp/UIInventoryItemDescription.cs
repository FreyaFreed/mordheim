using System;
using UnityEngine;
using UnityEngine.UI;

public class UIInventoryItemDescription : global::UnityEngine.MonoBehaviour
{
	public void Set(global::Item item, global::UnitSlotId? unitSlotId = null)
	{
		if (item.Id == global::ItemId.NONE)
		{
			base.gameObject.SetActive(false);
		}
		else
		{
			base.gameObject.SetActive(true);
			if (item.DamageMin != 0 && item.DamageMax != 0)
			{
				this.damageGroup.Set(string.Format("{0}-{1}", item.DamageMin, item.DamageMax));
			}
			else
			{
				this.damageGroup.gameObject.SetActive(false);
			}
			if (item.SpeedData.Id != global::ItemSpeedId.NONE)
			{
				this.speedGroup.Set(item.SpeedData.Speed.ToString("+0;-0;0"));
			}
			else
			{
				this.speedGroup.gameObject.SetActive(false);
			}
			if (item.TypeData.Id != global::ItemTypeId.NONE)
			{
				if (item.UnitSlots != null && item.UnitSlots.Count > 0)
				{
					switch (item.UnitSlots[0].UnitSlotId)
					{
					case global::UnitSlotId.HELMET:
						this.typeGroup.icon.overrideSprite = global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadResource<global::UnityEngine.Sprite>("item_slot/helmet", true);
						break;
					case global::UnitSlotId.ARMOR:
						this.typeGroup.icon.overrideSprite = global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadResource<global::UnityEngine.Sprite>("item_slot/armor", true);
						break;
					case global::UnitSlotId.SET1_MAINHAND:
					case global::UnitSlotId.SET1_OFFHAND:
					case global::UnitSlotId.SET2_MAINHAND:
					case global::UnitSlotId.SET2_OFFHAND:
						if (item.IsTwoHanded)
						{
							this.typeGroup.icon.overrideSprite = global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadResource<global::UnityEngine.Sprite>("item_slot_two_handed", true);
						}
						else
						{
							this.typeGroup.icon.overrideSprite = global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadResource<global::UnityEngine.Sprite>("item_slot_one_handed", true);
						}
						break;
					}
					this.typeGroup.Set(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("item_type_" + item.TypeData.Name));
				}
				else
				{
					this.typeGroup.gameObject.SetActive(false);
				}
			}
			else
			{
				this.typeGroup.gameObject.SetActive(false);
			}
			if (item.RangeMax != 0)
			{
				this.rangeGroup.Set(item.RangeMax.ToConstantString());
			}
			else
			{
				this.rangeGroup.gameObject.SetActive(false);
			}
			if (item.ArmorAbsorption != 0)
			{
				this.armorGroup.Set(item.ArmorAbsorption.ToConstantString());
			}
			else
			{
				this.armorGroup.gameObject.SetActive(false);
			}
			if (this.ratingGroup != null)
			{
				int rating = item.GetRating();
				if (rating != 0)
				{
					this.ratingGroup.Set(rating.ToConstantString());
				}
				else
				{
					this.ratingGroup.gameObject.SetActive(false);
				}
			}
			string localizedDescription = item.GetLocalizedDescription(unitSlotId);
			if (string.IsNullOrEmpty(localizedDescription))
			{
				this.description.gameObject.SetActive(false);
			}
			else
			{
				this.description.gameObject.SetActive(true);
				this.description.text = localizedDescription;
			}
			if (this.cantEquipReason != null)
			{
				this.cantEquipReason.gameObject.SetActive(false);
			}
			if (this.runeBlock != null)
			{
				this.runeBlock.Set(item);
			}
		}
	}

	public global::UIInventoryItemAttribute damageGroup;

	public global::UIInventoryItemAttribute speedGroup;

	public global::UIInventoryItemAttribute typeGroup;

	public global::UIInventoryItemAttribute rangeGroup;

	public global::UIInventoryItemAttribute armorGroup;

	public global::UIInventoryItemAttribute ratingGroup;

	public global::UnityEngine.UI.Text description;

	public global::UIInventoryRune runeBlock;

	public global::UnityEngine.UI.Text cantEquipReason;
}
