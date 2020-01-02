using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionModule : global::UIModule
{
	public void Setup(global::MissionModule.OnScoutButton scoutCb)
	{
		this.priceDatas = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::ScoutPriceData>("warband_rank", global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.Rank.ToString());
		this.scoutButton.SetAction("send_scout", "mission_send_scout", 0, false, null, null);
		this.scoutButton.OnAction(delegate
		{
			scoutCb();
		}, false, true);
		this.scoutButton.gameObject.SetActive(true);
		this.RefreshScoutButton();
		global::EventLogger logger = global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.Logger;
		int num = global::PandoraSingleton<global::HideoutManager>.Instance.Date.CurrentDate;
		global::System.Collections.Generic.List<global::Tuple<int, global::EventLogger.LogEvent, int>> eventsFromDay = logger.GetEventsFromDay(num);
		this.calEvents.Setup(string.Empty, this.item);
		for (int i = 0; i < eventsFromDay.Count; i++)
		{
			if (eventsFromDay[i].Item1 > num && eventsFromDay[i].Item2 != global::EventLogger.LogEvent.NEW_MISSION && eventsFromDay[i].Item2 != global::EventLogger.LogEvent.SHIPMENT_REQUEST && eventsFromDay[i].Item2 != global::EventLogger.LogEvent.CONTACT_ITEM)
			{
				num = eventsFromDay[i].Item1;
				global::UnityEngine.GameObject gameObject = this.calEvents.AddToList();
				global::UIMissionEvent component = gameObject.GetComponent<global::UIMissionEvent>();
				component.Setup(eventsFromDay, i, num);
			}
		}
	}

	public void RefreshScoutButton()
	{
		global::WarbandSave warbandSave = global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.GetWarbandSave();
		int num = this.priceDatas.Count - warbandSave.scoutsSent;
		if (warbandSave.scoutsSent < 0)
		{
			num = 0;
		}
		this.availableScouts.text = num.ToString();
		if (num == 0)
		{
			this.scoutButton.SetInteractable(false);
			this.scoutButton.SetDisabled(true);
			this.scoutCost.text = "0";
			this.availableScouts.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_scouts_available_none");
			return;
		}
		int num2 = global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.GetScoutCost(this.priceDatas[warbandSave.scoutsSent]);
		this.scoutCost.text = num2.ToString();
		if (num2 > global::PandoraSingleton<global::HideoutManager>.Instance.WarbandChest.GetGold())
		{
			this.scoutCost.color = global::UnityEngine.Color.red;
			this.scoutButton.SetDisabled(true);
		}
		else
		{
			this.scoutButton.SetDisabled(false);
		}
	}

	public global::ListGroup calEvents;

	public global::UnityEngine.GameObject item;

	public global::UnityEngine.UI.Text availableScouts;

	public global::UnityEngine.UI.Text scoutCost;

	public global::ButtonGroup scoutButton;

	private global::System.Collections.Generic.List<global::ScoutPriceData> priceDatas;

	public delegate void OnScoutButton();
}
