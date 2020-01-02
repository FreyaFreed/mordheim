using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMissionEvent : global::UnityEngine.MonoBehaviour
{
	public void Setup(global::System.Collections.Generic.List<global::Tuple<int, global::EventLogger.LogEvent, int>> log, int index, int day)
	{
		global::Date date = new global::Date(day);
		this.itemList.SetupLocalized(date.ToLocalizedString(), this.eventItem);
		while (index < log.Count && log[index].Item1 == day)
		{
			switch (log[index].Item2)
			{
			case global::EventLogger.LogEvent.SHIPMENT_DELIVERY:
			case global::EventLogger.LogEvent.FACTION0_DELIVERY:
			case global::EventLogger.LogEvent.FACTION1_DELIVERY:
			case global::EventLogger.LogEvent.FACTION2_DELIVERY:
				this.AddItemToList(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_delivery_received_future"), null, true);
				break;
			case global::EventLogger.LogEvent.SHIPMENT_LATE:
				this.AddItemToList(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_next_shipment"), log[index].Item3.ToConstantString(), false);
				break;
			case global::EventLogger.LogEvent.MARKET_ROTATION:
				this.AddItemToList(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_refresh_market_future"), null, true);
				break;
			case global::EventLogger.LogEvent.OUTSIDER_ROTATION:
				this.AddItemToList(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_refresh_outsider_future"), null, true);
				break;
			case global::EventLogger.LogEvent.RESPEC_POINT:
				this.AddItemToList(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_new_respec_point"), null, true);
				break;
			}
			IL_11F:
			index++;
			continue;
			goto IL_11F;
		}
	}

	private void AddItemToList(string title, string desc = null, bool hideImage = true)
	{
		global::UnityEngine.GameObject gameObject = this.itemList.AddToList();
		global::UIDescription component = gameObject.GetComponent<global::UIDescription>();
		component.SetLocalized(title, desc);
		if (hideImage)
		{
			gameObject.GetComponentsInChildren<global::UnityEngine.UI.Image>()[0].gameObject.SetActive(false);
		}
	}

	public global::ListGroup itemList;

	public global::UnityEngine.GameObject eventItem;
}
