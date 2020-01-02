using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LatestArrivalModule : global::UIModule
{
	public void Set(global::MarketEventId eventId, global::System.Collections.Generic.List<global::ItemSave> itemsSave, global::UnityEngine.Events.UnityAction<global::Item> onItemConfirmed, global::UnityEngine.Events.UnityAction<global::Item> onItemSelected)
	{
		this.eventName.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("market_" + eventId.ToLowerString() + "_name");
		this.description.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("market_" + eventId.ToLowerString() + "_desc");
		global::Date date = global::PandoraSingleton<global::HideoutManager>.Instance.Date;
		int nextDay = date.GetNextDay(global::WeekDayId.MARKTAG);
		this.refreshMessage.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("market_refresh", new string[]
		{
			(nextDay - date.CurrentDate).ToString()
		});
		global::System.Collections.Generic.List<global::Item> list = new global::System.Collections.Generic.List<global::Item>();
		for (int i = 0; i < itemsSave.Count; i++)
		{
			list.Add(new global::Item(itemsSave[i]));
		}
		list.Sort(new global::CompareItem());
		this.itemRuneList.SetList(list, onItemConfirmed, onItemSelected, false, false, false, true, false, null);
	}

	private void Update()
	{
		float axis = global::PandoraSingleton<global::PandoraInput>.Instance.GetAxis("cam_y", 0);
		if (!global::UnityEngine.Mathf.Approximately(axis, 0f))
		{
			this.itemRuneList.scrollGroup.ForceScroll(axis < 0f, false);
		}
	}

	public global::UnityEngine.UI.Text eventName;

	public global::UnityEngine.UI.Text description;

	public global::UnityEngine.UI.Text emptyListMessage;

	public global::UnityEngine.UI.Text refreshMessage;

	public global::UIInventoryItemRuneList itemRuneList;
}
