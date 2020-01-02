using System;
using UnityEngine.UI;

public class TreasuryModule : global::UIModule
{
	public void Refresh(global::WarbandSave save)
	{
		global::Tuple<int, global::EventLogger.LogEvent, int> tuple = global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.Logger.FindLastEvent(global::EventLogger.LogEvent.SHIPMENT_LATE);
		this.date.text = global::PandoraSingleton<global::HideoutManager>.Instance.Date.ToLocalizedString();
		this.holiday.text = global::PandoraSingleton<global::HideoutManager>.Instance.Date.ToLocalizedHoliday();
		int num = global::PandoraSingleton<global::HideoutManager>.Instance.WarbandChest.GetItem(global::ItemId.WYRDSTONE_FRAGMENT, global::ItemQualityId.NORMAL).amount;
		int num2 = global::PandoraSingleton<global::HideoutManager>.Instance.WarbandChest.GetItem(global::ItemId.WYRDSTONE_SHARD, global::ItemQualityId.NORMAL).amount;
		int num3 = global::PandoraSingleton<global::HideoutManager>.Instance.WarbandChest.GetItem(global::ItemId.WYRDSTONE_CLUSTER, global::ItemQualityId.NORMAL).amount;
		this.fragments.text = num.ToString();
		this.shards.text = num2.ToString();
		this.clusters.text = num3.ToString();
		float @float = global::Constant.GetFloat(global::ConstantId.WYRDSTONE_WEIGHT);
		num *= global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::ItemData>(130).PriceSold;
		num2 *= global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::ItemData>(208).PriceSold;
		num3 *= global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::ItemData>(209).PriceSold;
		this.totalWeight.text = ((float)(num + num2 + num3) * @float).ToString();
		this.gold.text = global::PandoraSingleton<global::HideoutManager>.Instance.WarbandChest.GetItem(global::ItemId.GOLD, global::ItemQualityId.NORMAL).amount.ToString();
		if (tuple != null && tuple.Item1 > global::PandoraSingleton<global::HideoutManager>.Instance.Date.CurrentDate)
		{
			this.shipment.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_days_left", new string[]
			{
				(tuple.Item1 - global::PandoraSingleton<global::HideoutManager>.Instance.Date.CurrentDate).ToString()
			});
		}
		else if (tuple != null && tuple.Item1 == global::PandoraSingleton<global::HideoutManager>.Instance.Date.CurrentDate)
		{
			this.shipment.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_no_days_left");
		}
		else
		{
			this.shipment.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_no_shipment");
		}
	}

	public global::UnityEngine.UI.Text holiday;

	public global::UnityEngine.UI.Text date;

	public global::UnityEngine.UI.Text fragments;

	public global::UnityEngine.UI.Text shards;

	public global::UnityEngine.UI.Text clusters;

	public global::UnityEngine.UI.Text totalWeight;

	public global::UnityEngine.UI.Text gold;

	public global::UnityEngine.UI.Text shipment;
}
