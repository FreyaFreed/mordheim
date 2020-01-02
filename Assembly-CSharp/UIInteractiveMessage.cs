using System;
using UnityEngine.UI;

public class UIInteractiveMessage : global::CanvasGroupDisabler
{
	private void Awake()
	{
		base.gameObject.SetActive(false);
		global::PandoraSingleton<global::NoticeManager>.Instance.RegisterListener(global::Notices.INTERACTION_POINTS_CHANGED, new global::DelReceiveNotice(this.UpdateInteractivePoints));
		global::PandoraSingleton<global::NoticeManager>.Instance.RegisterListener(global::Notices.LADDER_UNIT_CHANGED, new global::DelReceiveNotice(this.UpdateInteractivePoints));
	}

	private void UpdateInteractivePoints()
	{
		global::UnitController currentUnit = global::PandoraSingleton<global::MissionManager>.Instance.GetCurrentUnit();
		if (currentUnit != null && currentUnit.IsPlayed())
		{
			bool flag = currentUnit.unit.Data.UnitSizeId == global::UnitSizeId.LARGE;
			string text = string.Empty;
			for (int i = 0; i < currentUnit.interactivePoints.Count; i++)
			{
				if (!currentUnit.interactivePoints[i].HasRequiredItem(currentUnit))
				{
					text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("na_interactive_required_item", new string[]
					{
						global::Item.GetLocalizedName(currentUnit.interactivePoints[i].requestedItemId)
					});
					break;
				}
				if (currentUnit.interactivePoints[i] is global::ActionZone)
				{
					global::ActionZone actionZone = (global::ActionZone)currentUnit.interactivePoints[i];
					for (int j = 0; j < actionZone.destinations.Count; j++)
					{
						if (flag && !actionZone.destinations[j].destination.supportLargeUnit)
						{
							text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("na_interactive_large");
							break;
						}
						if (!actionZone.destinations[j].destination.PointsChecker.IsAvailable())
						{
							text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("na_interactive_full");
							break;
						}
					}
					if (!string.IsNullOrEmpty(text))
					{
						break;
					}
				}
				else if (currentUnit.interactivePoints[i] is global::SearchPoint)
				{
					global::SearchPoint searchPoint = (global::SearchPoint)currentUnit.interactivePoints[i];
					if (searchPoint.slots.Count > 0 && ((searchPoint.slots[0].restrictedItemId != global::ItemId.NONE && !currentUnit.unit.HasItem(searchPoint.slots[0].restrictedItemId, global::ItemQualityId.NONE)) || (searchPoint.slots[0].restrictedItemTypeId != global::ItemTypeId.NONE && !currentUnit.unit.HasItem(searchPoint.slots[0].restrictedItemTypeId))) && currentUnit.unit.IsInventoryFull())
					{
						text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("na_interactive_inventory_full");
					}
				}
			}
			if (currentUnit.currentTeleporter != null && !currentUnit.currentTeleporter.IsActive())
			{
				text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("na_teleport_full");
			}
			if (!string.IsNullOrEmpty(text))
			{
				this.Show(text);
			}
			else
			{
				this.Hide();
			}
		}
		else
		{
			this.Hide();
		}
	}

	public void Show(string message)
	{
		base.gameObject.SetActive(true);
		this.text.text = message;
	}

	public void Hide()
	{
		base.gameObject.SetActive(false);
	}

	public global::UnityEngine.UI.Text text;
}
