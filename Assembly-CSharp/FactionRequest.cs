using System;
using UnityEngine;
using UnityEngine.UI;

public class FactionRequest : global::FactionShipment
{
	public override void SetFaction(global::FactionMenuController faction, global::System.Action<global::FactionShipment> send)
	{
		base.SetFaction(faction, send);
		this.weightRequirement.text = base.FactionCtrlr.ShipmentWeight.ToString("D4");
		this.goldReward.text = base.FactionCtrlr.ShipmentGoldReward.ToString("D4");
		this.totalWeight.text = "0000";
		int num = base.FactionCtrlr.ShipmentDate - global::PandoraSingleton<global::HideoutManager>.Instance.Date.CurrentDate;
		this.daysLeft.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_days_left", new string[]
		{
			num.ToString()
		});
		if (global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.GetWarbandSave().lastShipmentFailed)
		{
			this.consequences.enabled = true;
			this.consequences.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("faction_consequence_penalty_" + global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.GetWarbandSave().lateShipmentCount);
		}
		else
		{
			this.consequences.enabled = false;
		}
		this.RefreshTotal();
	}

	protected override void OnSend()
	{
		if (base.TotalWeight >= base.FactionCtrlr.ShipmentWeight)
		{
			base.OnSend();
		}
	}

	protected override void RefreshTotal()
	{
		base.RefreshTotal();
		this.fragmentWeight.text = base.TotalFragmentWeight.ToString("D3");
		this.shardWeight.text = base.TotalShardWeight.ToString("D3");
		this.clusterWeight.text = base.TotalClusterWeight.ToString("D3");
		int num = base.TotalWeight - base.FactionCtrlr.ShipmentWeight;
		this.sendBtn.SetDisabled(num < 0);
		this.notFullShipment.gameObject.SetActive(num < 0);
		num = ((num >= 0) ? num : 0);
		this.totalWeight.text = base.TotalWeight.ToString("D4");
		int deliveryPrice = base.FactionCtrlr.GetDeliveryPrice(num);
		base.TotalGold = base.FactionCtrlr.ShipmentGoldReward + deliveryPrice;
		this.overweightGold.text = deliveryPrice.ToString("D4");
		base.TotalReputation = base.TotalWeight;
		if (base.TotalReputation > base.FactionCtrlr.MaxReputationGain)
		{
			base.TotalReputation = base.FactionCtrlr.MaxReputationGain;
			this.overweightReputation.color = this.cappedColor;
		}
		else
		{
			this.overweightReputation.color = this.normalColor;
		}
		this.overweightReputation.text = base.TotalReputation.ToString("D4");
	}

	public global::UnityEngine.UI.Text weightRequirement;

	public global::UnityEngine.UI.Text goldReward;

	public global::UnityEngine.UI.Text daysLeft;

	public global::UnityEngine.UI.Text fragmentWeight;

	public global::UnityEngine.UI.Text shardWeight;

	public global::UnityEngine.UI.Text clusterWeight;

	public global::UnityEngine.UI.Text totalWeight;

	public global::UnityEngine.UI.Text overweightGold;

	public global::UnityEngine.UI.Text overweightReputation;

	public global::UnityEngine.UI.Text consequences;

	public global::UnityEngine.UI.Text notFullShipment;

	public global::UnityEngine.Color normalColor = global::UnityEngine.Color.white;

	public global::UnityEngine.Color cappedColor = global::UnityEngine.Color.red;
}
