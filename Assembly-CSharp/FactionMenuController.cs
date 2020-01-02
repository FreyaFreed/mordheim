using System;
using UnityEngine;

public class FactionMenuController
{
	public FactionMenuController(global::Faction faction, global::WarbandSave warSave)
	{
		this.Faction = faction;
		this.WarSave = warSave;
		this.FragmentPrice = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::ItemData>(130).PriceSold;
		this.ShardPrice = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::ItemData>(208).PriceSold;
		this.ClusterPrice = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::ItemData>(209).PriceSold;
	}

	public global::Faction Faction { get; private set; }

	public global::WarbandSave WarSave { get; private set; }

	public bool HasShipment
	{
		get
		{
			return this.Shipment != null;
		}
	}

	public global::Tuple<int, global::EventLogger.LogEvent, int> Shipment { get; private set; }

	public int ShipmentDate { get; private set; }

	public int ShipmentWeight { get; private set; }

	public int ShipmentGoldReward { get; set; }

	public int NextRankReputation { get; private set; }

	public int NextRankReputationModifier { get; private set; }

	public int MaxReputationGain { get; private set; }

	public int FragmentPrice { get; set; }

	public int ShardPrice { get; set; }

	public int ClusterPrice { get; set; }

	public int PriceModifier { get; set; }

	public void Refresh()
	{
		this.RefreshBonus();
		this.RefreshReputation();
		this.RefreshShipment();
	}

	public void RefreshReputation()
	{
		if (this.Faction.Rank + 1 >= this.Faction.RanksData.Count)
		{
			this.NextRankReputation = -1;
			this.MaxReputationGain = 0;
		}
		else
		{
			int num = this.Faction.Save.rank + 1;
			this.NextRankReputation = this.Faction.RanksData[num].Reputation;
			if (this.NextRankReputationModifier != 0)
			{
				this.NextRankReputation += this.NextRankReputation / this.NextRankReputationModifier;
			}
			this.MaxReputationGain = this.NextRankReputation - this.Faction.Save.reputation;
			if (num + 1 < this.Faction.RanksData.Count)
			{
				int num2 = 0;
				for (int i = 0; i < global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.Factions.Count; i++)
				{
					global::Faction faction = global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.Factions[i];
					if (faction == this.Faction)
					{
						num2 += faction.Data.RepBonusPercPerRank * num;
					}
					else
					{
						num2 += faction.Data.RepBonusPercPerOtherFactionRank * faction.Rank;
					}
				}
				int num3 = 0;
				if (num2 != 0)
				{
					num3 = 100 / num2;
				}
				int num4 = this.Faction.RanksData[num + 1].Reputation;
				if (num3 != 0)
				{
					num4 += num4 / num3;
				}
				this.MaxReputationGain += num4 - 1;
			}
		}
	}

	public void RefreshBonus()
	{
		int num = 100;
		int num2 = 0;
		for (int i = 0; i < global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.Factions.Count; i++)
		{
			global::Faction faction = global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.Factions[i];
			if (faction == this.Faction)
			{
				num += faction.Data.WyrdstonePriceBonusPercPerRank * this.Faction.Rank;
				num2 += faction.Data.RepBonusPercPerRank * this.Faction.Rank;
			}
			else
			{
				num += faction.Data.WyrdstonePriceBonusPercPerOtherFactionRank * faction.Rank;
				num2 += faction.Data.RepBonusPercPerOtherFactionRank * faction.Rank;
			}
		}
		num = global::UnityEngine.Mathf.Max(this.Faction.Data.MinWydstonePriceModifier, num);
		this.NextRankReputationModifier = 0;
		if (num2 != 0)
		{
			this.NextRankReputationModifier = 100 / num2;
		}
		this.PriceModifier = 0;
		if (num != 100)
		{
			this.PriceModifier = 100 / (num - 100);
		}
	}

	public int AddReputation(int rep)
	{
		int num = 0;
		if (this.NextRankReputation != -1)
		{
			this.Faction.Save.reputation += rep;
			while (this.NextRankReputation != -1 && this.Faction.Save.reputation >= this.NextRankReputation)
			{
				if (num != 0)
				{
					this.Faction.Save.reputation = this.NextRankReputation - 1;
					break;
				}
				this.Faction.Save.reputation -= this.NextRankReputation;
				num = ++this.Faction.Save.rank;
				this.Refresh();
				global::PandoraSingleton<global::Pan>.Instance.Narrate("new_reputation" + global::PandoraSingleton<global::GameManager>.Instance.LocalTyche.Rand(1, 3));
			}
		}
		return num;
	}

	public void RefreshShipment()
	{
		if (this.Faction.Primary)
		{
			global::Tuple<int, global::EventLogger.LogEvent, int> tuple = global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.Logger.FindLastEvent(global::EventLogger.LogEvent.SHIPMENT_REQUEST);
			global::Tuple<int, global::EventLogger.LogEvent, int> tuple2 = global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.Logger.FindEventAfter(global::EventLogger.LogEvent.SHIPMENT_LATE, tuple.Item1);
			if (tuple.Item1 <= global::PandoraSingleton<global::HideoutManager>.Instance.Date.CurrentDate && tuple2 != null && global::PandoraSingleton<global::HideoutManager>.Instance.Date.CurrentDate <= tuple2.Item1)
			{
				this.SetShipment(tuple2);
			}
			else
			{
				this.SetShipment(null);
			}
		}
		else
		{
			this.SetShipment(null);
		}
	}

	private void SetShipment(global::Tuple<int, global::EventLogger.LogEvent, int> latestShipment)
	{
		this.Shipment = latestShipment;
		if (latestShipment != null)
		{
			this.ShipmentDate = latestShipment.Item1;
			this.ShipmentWeight = latestShipment.Item3;
			this.ShipmentGoldReward = this.GetShipmentDeliveryPrice(this.ShipmentWeight, this.ShipmentWeight);
		}
	}

	public int GetDeliveryReputation(int shipmentWeight)
	{
		return shipmentWeight;
	}

	public int GetDeliveryPrice(int shipmentWeight)
	{
		int num = global::UnityEngine.Mathf.FloorToInt((float)shipmentWeight / global::Constant.GetFloat(global::ConstantId.WYRDSTONE_WEIGHT));
		if (this.PriceModifier != 0)
		{
			num += num / this.PriceModifier;
		}
		return num;
	}

	public int GetShipmentDeliveryReputation(int shipmentWeight, int requiredWeight)
	{
		return global::UnityEngine.Mathf.FloorToInt((float)global::UnityEngine.Mathf.Clamp(shipmentWeight - requiredWeight, 0, int.MaxValue) / global::Constant.GetFloat(global::ConstantId.WYRDSTONE_WEIGHT));
	}

	public int GetShipmentDeliveryPrice(int shipmentWeight, int requiredWeight)
	{
		int shipmentWeight2 = global::UnityEngine.Mathf.Clamp(shipmentWeight - requiredWeight, 0, int.MaxValue);
		int deliveryPrice = this.GetDeliveryPrice(shipmentWeight2);
		global::Warband warband = global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband;
		return global::UnityEngine.Mathf.FloorToInt((float)requiredWeight / global::Constant.GetFloat(global::ConstantId.WYRDSTONE_WEIGHT) * warband.GetPercAttribute(global::WarbandAttributeId.REQUEST_PRICE_PERC) * (1f + warband.GetPercAttribute(global::WarbandAttributeId.REQUEST_PRICE_GLOBAL_PERC))) + deliveryPrice;
	}

	public void CreateNewShipmentRequest(int lastShipmentDueDate, int lastShipmentDeliveryDate)
	{
		global::WarbandMenuController warbandCtrlr = global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr;
		global::WyrdstoneShipmentData wyrdstoneShipmentData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::WyrdstoneShipmentData>("fk_warband_rank_id", this.WarSave.rank.ToString())[0];
		int data = global::UnityEngine.Mathf.FloorToInt((float)global::PandoraSingleton<global::GameManager>.Instance.LocalTyche.Rand(wyrdstoneShipmentData.MinWeight, wyrdstoneShipmentData.MaxWeight + 1) * global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.GetPercAttribute(global::WarbandAttributeId.REQUEST_WEIGHT_PERC));
		int num = lastShipmentDueDate + global::PandoraSingleton<global::GameManager>.Instance.LocalTyche.Rand(wyrdstoneShipmentData.NextMinDays, wyrdstoneShipmentData.NextMaxDays + 1);
		this.WarSave.nextShipmentExtraDays = 0;
		if (num < lastShipmentDeliveryDate)
		{
			num = lastShipmentDeliveryDate;
		}
		warbandCtrlr.Warband.Logger.AddHistory(num, global::EventLogger.LogEvent.SHIPMENT_REQUEST, data);
	}

	public string GetConsequenceLabel()
	{
		return string.Concat(new object[]
		{
			"faction_consequence_",
			this.Faction.Data.Id,
			"_",
			this.WarSave.lateShipmentCount
		});
	}
}
