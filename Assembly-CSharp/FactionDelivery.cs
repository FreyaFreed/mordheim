using System;
using UnityEngine;
using UnityEngine.UI;

public class FactionDelivery : global::FactionShipment
{
	public override void SetFaction(global::FactionMenuController faction, global::System.Action<global::FactionShipment> send)
	{
		base.SetFaction(faction, send);
		this.fragmentRep.text = "000";
		this.shardRep.text = "000";
		this.clusterRep.text = "000";
		this.fragmentPrice.text = "000";
		this.shardPrice.text = "000";
		this.clusterPrice.text = "000";
		this.totalRep.text = "0000";
		this.totalGold.text = "0000";
		this.fragmentRep.color = this.normalColor;
		this.shardRep.color = this.normalColor;
		this.clusterRep.color = this.normalColor;
		this.totalRep.color = this.normalColor;
	}

	protected override void RefreshTotal()
	{
		base.RefreshTotal();
		int num = base.TotalFragmentWeight;
		int num2 = base.TotalShardWeight;
		int num3 = base.TotalClusterWeight;
		int num4 = base.FactionCtrlr.FragmentPrice;
		int num5 = base.FactionCtrlr.ShardPrice;
		int num6 = base.FactionCtrlr.ClusterPrice;
		if (base.FactionCtrlr.PriceModifier != 0)
		{
			num4 += base.FactionCtrlr.FragmentPrice / base.FactionCtrlr.PriceModifier;
			num5 += base.FactionCtrlr.ShardPrice / base.FactionCtrlr.PriceModifier;
			num6 += base.FactionCtrlr.ClusterPrice / base.FactionCtrlr.PriceModifier;
		}
		int num7 = base.FragmentCount * num4;
		int num8 = base.ShardCount * num5;
		int num9 = base.ClusterCount * num6;
		this.fragmentPrice.text = num7.ToString("D3");
		this.shardPrice.text = num8.ToString("D3");
		this.clusterPrice.text = num9.ToString("D3");
		base.TotalGold = num7 + num8 + num9;
		this.totalGold.text = base.TotalGold.ToString("D4");
		base.TotalReputation = num + num2 + num3;
		if (base.TotalReputation > base.FactionCtrlr.MaxReputationGain)
		{
			base.TotalReputation = base.FactionCtrlr.MaxReputationGain;
			if (num > base.FactionCtrlr.MaxReputationGain)
			{
				num = base.FactionCtrlr.MaxReputationGain;
			}
			if (num + num2 > base.FactionCtrlr.MaxReputationGain)
			{
				num2 = global::UnityEngine.Mathf.Clamp(num2, 0, base.FactionCtrlr.MaxReputationGain - num);
			}
			if (num + num2 + num3 > base.FactionCtrlr.MaxReputationGain)
			{
				num3 = global::UnityEngine.Mathf.Clamp(num3, 0, base.FactionCtrlr.MaxReputationGain - (num + num2));
			}
			this.fragmentRep.color = this.cappedColor;
			this.shardRep.color = this.cappedColor;
			this.clusterRep.color = this.cappedColor;
			this.totalRep.color = this.cappedColor;
		}
		else
		{
			this.fragmentRep.color = this.normalColor;
			this.shardRep.color = this.normalColor;
			this.clusterRep.color = this.normalColor;
			this.totalRep.color = this.normalColor;
		}
		this.fragmentRep.text = num.ToString("D3");
		this.shardRep.text = num2.ToString("D3");
		this.clusterRep.text = num3.ToString("D3");
		this.totalRep.text = base.TotalReputation.ToString("D4");
	}

	public global::UnityEngine.UI.Text totalRep;

	public global::UnityEngine.UI.Text totalGold;

	public global::UnityEngine.UI.Text fragmentRep;

	public global::UnityEngine.UI.Text shardRep;

	public global::UnityEngine.UI.Text clusterRep;

	public global::UnityEngine.UI.Text fragmentPrice;

	public global::UnityEngine.UI.Text shardPrice;

	public global::UnityEngine.UI.Text clusterPrice;

	public global::UnityEngine.Color normalColor = global::UnityEngine.Color.white;

	public global::UnityEngine.Color cappedColor = global::UnityEngine.Color.red;
}
