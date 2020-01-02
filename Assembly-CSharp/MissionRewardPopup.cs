using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionRewardPopup : global::ConfirmationPopupView
{
	public void Show(global::System.Action<bool> callback, global::System.Collections.Generic.List<global::WarbandSkillItemData> itemRewards, global::System.Collections.Generic.List<global::WarbandSkillFreeOutsiderData> freeOutsiders)
	{
		base.Show("mission_camp_reward_title", "mission_camp_reward_" + global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.Id.ToLowerString(), callback, false, false);
		global::UnityEngine.Sprite sprite = global::Warband.GetIcon(global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.Id);
		this.icon.sprite = sprite;
		this.rewardItems.Setup(string.Empty, this.itemReward);
		for (int i = 0; i < itemRewards.Count; i++)
		{
			global::Item item = new global::Item(itemRewards[i].ItemId, itemRewards[i].ItemQualityId);
			item.Save.amount = itemRewards[i].Quantity;
			global::UnityEngine.GameObject gameObject = this.rewardItems.AddToList();
			gameObject.GetComponent<global::UIInventoryItem>().Set(item, false, false, global::ItemId.NONE, false);
		}
		this.rewardUnits.Setup(string.Empty, this.unitReward);
		for (int j = 0; j < freeOutsiders.Count; j++)
		{
			global::UnityEngine.GameObject gameObject2 = this.rewardUnits.AddToList();
			gameObject2.GetComponent<global::HireUnitDescription>().Set(freeOutsiders[j].UnitId, freeOutsiders[j].Rank);
		}
	}

	public global::UnityEngine.UI.Image icon;

	public global::ListGroup rewardItems;

	public global::ListGroup rewardUnits;

	public global::UnityEngine.GameObject itemReward;

	public global::UnityEngine.GameObject unitReward;
}
