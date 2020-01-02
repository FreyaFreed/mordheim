using System;
using UnityEngine;
using UnityEngine.UI;

public class FactionOverviewCard : global::UnityEngine.MonoBehaviour
{
	public global::FactionMenuController FactionCtrlr { get; set; }

	public void SetFaction(global::FactionMenuController faction)
	{
		this.FactionCtrlr = faction;
		this.factionName.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(this.FactionCtrlr.Faction.Data.Desc + "_name");
		if (this.FactionCtrlr.Faction.Primary)
		{
			this.factionIcon.sprite = global::Warband.GetIcon(global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.Id);
			this.factionType.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_faction_primary");
			if (this.FactionCtrlr.HasShipment)
			{
				this.rewardsSection.SetActive(false);
				this.weightReqSection.SetActive(true);
				this.daysLeft.text = (this.FactionCtrlr.ShipmentDate - global::PandoraSingleton<global::HideoutManager>.Instance.Date.CurrentDate).ToString();
				this.weightReqText.text = this.FactionCtrlr.ShipmentWeight.ToString();
			}
			else
			{
				this.rewardsSection.SetActive(true);
				this.weightReqSection.SetActive(false);
			}
			this.daysLeft.gameObject.SetActive(this.FactionCtrlr.HasShipment);
		}
		else
		{
			global::UnityEngine.Sprite sprite = global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadResource<global::UnityEngine.Sprite>("warband/" + this.FactionCtrlr.Faction.Data.Name, true);
			if (sprite != null)
			{
				global::PandoraDebug.LogWarning("Cannot load faction icon : warband/" + this.FactionCtrlr.Faction.Data.Name, "uncategorised", null);
			}
			this.factionIcon.sprite = sprite;
			this.factionType.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_faction_secondary");
			this.rewardsSection.SetActive(true);
			this.weightReqSection.SetActive(false);
		}
		this.progressBar.enabled = true;
		this.reputation.text = this.FactionCtrlr.Faction.Save.rank.ToString();
		if (this.FactionCtrlr.NextRankReputation == -1)
		{
			this.progression.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_max_rank");
			this.progressBar.normalizedValue = 1f;
			this.reputationAdjustment.gameObject.SetActive(false);
		}
		else
		{
			this.progression.text = string.Format("{0}/{1}", this.FactionCtrlr.Faction.Reputation, this.FactionCtrlr.NextRankReputation);
			this.progressBar.normalizedValue = (float)this.FactionCtrlr.Faction.Reputation / (float)this.FactionCtrlr.NextRankReputation;
			bool flag = this.FactionCtrlr.NextRankReputationModifier != 0;
			if (flag)
			{
				this.reputationAdjustment.text = string.Format("({0}%)", global::UnityEngine.Mathf.FloorToInt(100f / (float)this.FactionCtrlr.NextRankReputationModifier).ToString("+#;-#"));
			}
			this.reputationAdjustment.gameObject.SetActive(flag);
		}
		this.progressBar.enabled = false;
		int num = this.FactionCtrlr.FragmentPrice;
		int num2 = this.FactionCtrlr.ShardPrice;
		int num3 = this.FactionCtrlr.ClusterPrice;
		bool flag2 = this.FactionCtrlr.PriceModifier != 0;
		if (flag2)
		{
			string text = string.Format("({0}%)", global::UnityEngine.Mathf.FloorToInt(100f / (float)this.FactionCtrlr.PriceModifier).ToString("+#;-#"));
			this.fragPriceAdjust.text = text;
			this.shardPriceAdjust.text = text;
			this.clusterPriceAdjust.text = text;
			num += this.FactionCtrlr.FragmentPrice / this.FactionCtrlr.PriceModifier;
			num2 += this.FactionCtrlr.ShardPrice / this.FactionCtrlr.PriceModifier;
			num3 += this.FactionCtrlr.ClusterPrice / this.FactionCtrlr.PriceModifier;
		}
		this.fragPrice.text = num.ToString();
		this.shardPrice.text = num2.ToString();
		this.clusterPrice.text = num3.ToString();
		this.fragPriceAdjust.gameObject.SetActive(flag2);
		this.shardPriceAdjust.gameObject.SetActive(flag2);
		this.clusterPriceAdjust.gameObject.SetActive(flag2);
		this.fragRep.text = ((float)this.FactionCtrlr.FragmentPrice * global::Constant.GetFloat(global::ConstantId.WYRDSTONE_WEIGHT)).ToString();
		this.shardRep.text = ((float)this.FactionCtrlr.ShardPrice * global::Constant.GetFloat(global::ConstantId.WYRDSTONE_WEIGHT)).ToString();
		this.clusterRep.text = ((float)this.FactionCtrlr.ClusterPrice * global::Constant.GetFloat(global::ConstantId.WYRDSTONE_WEIGHT)).ToString();
	}

	private const string MODIFIER_FORMAT = "({0}%)";

	public global::UnityEngine.UI.Text factionType;

	public global::UnityEngine.UI.Text factionName;

	public global::UnityEngine.UI.Text daysLeft;

	public global::UnityEngine.UI.Text weightReqText;

	public global::UnityEngine.UI.Image factionIcon;

	public global::UnityEngine.UI.Text reputation;

	public global::UnityEngine.UI.Text progression;

	public global::UnityEngine.UI.Text reputationAdjustment;

	public global::UnityEngine.UI.Slider progressBar;

	public global::UnityEngine.GameObject rewardsSection;

	public global::UnityEngine.GameObject weightReqSection;

	public global::UnityEngine.UI.Text fragPrice;

	public global::UnityEngine.UI.Text fragPriceAdjust;

	public global::UnityEngine.UI.Text fragRep;

	public global::UnityEngine.UI.Text shardPrice;

	public global::UnityEngine.UI.Text shardPriceAdjust;

	public global::UnityEngine.UI.Text shardRep;

	public global::UnityEngine.UI.Text clusterPrice;

	public global::UnityEngine.UI.Text clusterPriceAdjust;

	public global::UnityEngine.UI.Text clusterRep;
}
