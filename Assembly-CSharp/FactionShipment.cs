using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public abstract class FactionShipment : global::UnityEngine.MonoBehaviour
{
	public global::FactionMenuController FactionCtrlr { get; set; }

	public int TotalFragmentWeight { get; set; }

	public int TotalShardWeight { get; set; }

	public int TotalClusterWeight { get; set; }

	public int FragmentCount
	{
		get
		{
			return this.fragDel.CurSel;
		}
	}

	public int ShardCount
	{
		get
		{
			return this.shardDel.CurSel;
		}
	}

	public int ClusterCount
	{
		get
		{
			return this.clusterDel.CurSel;
		}
	}

	public int TotalGold { get; set; }

	public int TotalWeight { get; set; }

	public int TotalReputation { get; set; }

	public virtual void SetFaction(global::FactionMenuController faction, global::System.Action<global::FactionShipment> send)
	{
		this.FactionCtrlr = faction;
		this.sendCallback = send;
		this.TotalGold = 0;
		this.TotalWeight = 0;
		this.TotalReputation = 0;
		this.sendBtn.SetAction("send_shipment", "hideout_send", 0, false, null, null);
		this.sendBtn.OnAction(new global::UnityEngine.Events.UnityAction(this.OnSend), false, true);
		this.sendBtn.SetDisabled(true);
		global::WarbandChest warbandChest = global::PandoraSingleton<global::HideoutManager>.Instance.WarbandChest;
		int num = global::UnityEngine.Mathf.Min(999, warbandChest.GetItem(global::ItemId.WYRDSTONE_FRAGMENT, global::ItemQualityId.NORMAL).amount);
		this.fragDel.onValueChanged = new global::SelectorGroup.OnValueChanged(this.OnDeliveryChange);
		this.fragDel.selections.Clear();
		for (int i = 0; i < num + 1; i++)
		{
			this.fragDel.selections.Add(i.ToString("D3"));
		}
		this.fragDel.SetCurrentSel(0);
		num = global::UnityEngine.Mathf.Min(999, warbandChest.GetItem(global::ItemId.WYRDSTONE_SHARD, global::ItemQualityId.NORMAL).amount);
		this.shardDel.onValueChanged = new global::SelectorGroup.OnValueChanged(this.OnDeliveryChange);
		this.shardDel.selections.Clear();
		for (int j = 0; j < num + 1; j++)
		{
			this.shardDel.selections.Add(j.ToString("D3"));
		}
		this.shardDel.SetCurrentSel(0);
		num = global::UnityEngine.Mathf.Min(999, warbandChest.GetItem(global::ItemId.WYRDSTONE_CLUSTER, global::ItemQualityId.NORMAL).amount);
		this.clusterDel.onValueChanged = new global::SelectorGroup.OnValueChanged(this.OnDeliveryChange);
		this.clusterDel.selections.Clear();
		for (int k = 0; k < num + 1; k++)
		{
			this.clusterDel.selections.Add(k.ToString("D3"));
		}
		this.clusterDel.SetCurrentSel(0);
		global::System.Collections.Generic.List<global::Tuple<int, global::EventLogger.LogEvent, int>> list = global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.Logger.FindEventsAfter(faction.Faction.GetFactionDeliveryEvent(), global::PandoraSingleton<global::HideoutManager>.Instance.Date.CurrentDate + 1);
		int num2 = 0;
		global::System.Collections.Generic.List<global::Tuple<int, global::EventLogger.LogEvent, int>> list2 = null;
		if (faction.Faction.Primary)
		{
			list2 = global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.Logger.FindEventsAfter(global::EventLogger.LogEvent.SHIPMENT_DELIVERY, global::PandoraSingleton<global::HideoutManager>.Instance.Date.CurrentDate + 1);
			num2 = list2.Count;
		}
		this.deliveryCount.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("delivery_count", new string[]
		{
			(list.Count + num2).ToString()
		});
		if (list.Count + num2 > 0)
		{
			this.deliveryDate.gameObject.SetActive(true);
			int num3 = int.MaxValue;
			for (int l = 0; l < list.Count; l++)
			{
				if (list[l].Item1 < num3)
				{
					num3 = list[l].Item1;
				}
			}
			for (int m = 0; m < num2; m++)
			{
				if (list2[m].Item1 < num3)
				{
					num3 = list2[m].Item1;
				}
			}
			int num4 = num3 - global::PandoraSingleton<global::HideoutManager>.Instance.Date.CurrentDate;
			this.deliveryDate.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("smuggler_next_delivery", new string[]
			{
				num4.ToString()
			});
		}
		else
		{
			this.deliveryDate.gameObject.SetActive(false);
		}
	}

	protected virtual void OnSend()
	{
		this.sendCallback(this);
	}

	protected virtual void OnDeliveryChange(int id, int index)
	{
		this.RefreshTotal();
	}

	protected virtual void RefreshTotal()
	{
		this.TotalFragmentWeight = global::UnityEngine.Mathf.FloorToInt((float)(this.FragmentCount * this.FactionCtrlr.FragmentPrice) * global::Constant.GetFloat(global::ConstantId.WYRDSTONE_WEIGHT));
		this.TotalShardWeight = global::UnityEngine.Mathf.FloorToInt((float)(this.ShardCount * this.FactionCtrlr.ShardPrice) * global::Constant.GetFloat(global::ConstantId.WYRDSTONE_WEIGHT));
		this.TotalClusterWeight = global::UnityEngine.Mathf.FloorToInt((float)(this.ClusterCount * this.FactionCtrlr.ClusterPrice) * global::Constant.GetFloat(global::ConstantId.WYRDSTONE_WEIGHT));
		this.TotalWeight = this.TotalFragmentWeight + this.TotalShardWeight + this.TotalClusterWeight;
		this.sendBtn.SetDisabled(this.TotalWeight == 0);
	}

	public void ShowArrows(bool show)
	{
		this.fragDel.SetButtonsVisible(show);
		this.shardDel.SetButtonsVisible(show);
		this.clusterDel.SetButtonsVisible(show);
	}

	protected global::System.Action<global::FactionShipment> sendCallback;

	public global::ButtonGroup sendBtn;

	public global::SelectorGroup fragDel;

	public global::SelectorGroup shardDel;

	public global::SelectorGroup clusterDel;

	public global::ToggleEffects fragGroup;

	public global::ToggleEffects shardGroup;

	public global::ToggleEffects clusterGroup;

	public global::UnityEngine.UI.ToggleGroup toggleGroup;

	public global::UnityEngine.UI.Text deliveryCount;

	public global::UnityEngine.UI.Text deliveryDate;
}
