using System;
using System.Collections.Generic;

public class Faction
{
	public Faction(global::WarbandSave warSave, global::FactionSave save)
	{
		this.Data = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::FactionData>((int)save.factionId);
		this.WarSave = warSave;
		this.Save = save;
		this.Primary = this.Data.Primary;
		this.RanksData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::FactionRankData>();
		this.Rewards = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::FactionRankWarbandSkillData>("fk_faction_id", ((int)this.Data.Id).ToString());
	}

	public global::FactionData Data { get; private set; }

	public global::FactionSave Save { get; private set; }

	public global::WarbandSave WarSave { get; private set; }

	public bool Primary { get; set; }

	public int Rank
	{
		get
		{
			return this.Save.rank;
		}
	}

	public global::System.Collections.Generic.List<global::FactionRankData> RanksData { get; private set; }

	public global::System.Collections.Generic.List<global::FactionRankWarbandSkillData> Rewards { get; set; }

	public int Reputation
	{
		get
		{
			return this.Save.reputation;
		}
	}

	public string LocalizedConsequences { get; private set; }

	public global::WarbandSkillId GetRewardWarbandSkillId(global::FactionRankId rankId)
	{
		for (int i = 0; i < this.Rewards.Count; i++)
		{
			if (this.Rewards[i].FactionRankId == rankId)
			{
				return this.Rewards[i].WarbandSkillId;
			}
		}
		return global::WarbandSkillId.NONE;
	}

	public bool HasRank(int rank)
	{
		for (int i = 0; i < this.Save.shipments.Count; i++)
		{
			for (int j = 0; j < 5; j++)
			{
				int num = this.Save.shipments[i].rank >> 4 * j & 15;
				if (num > 0 && num == rank)
				{
					return false;
				}
			}
		}
		return this.Rank >= rank;
	}

	public global::EventLogger.LogEvent GetFactionDeliveryEvent()
	{
		global::EventLogger.LogEvent result = global::EventLogger.LogEvent.NONE;
		switch (this.Save.factionIndex)
		{
		case 0:
			result = global::EventLogger.LogEvent.FACTION0_DELIVERY;
			break;
		case 1:
			result = global::EventLogger.LogEvent.FACTION1_DELIVERY;
			break;
		case 2:
			result = global::EventLogger.LogEvent.FACTION2_DELIVERY;
			break;
		}
		return result;
	}

	public bool SaveDelivery(int weight, int gold, int rank, int date, out int id)
	{
		global::ShipmentSave shipmentSave;
		for (int i = 0; i < this.Save.shipments.Count; i++)
		{
			if (this.Save.shipments[i].sendDate == date)
			{
				shipmentSave = this.Save.shipments[i];
				shipmentSave.weight += weight;
				shipmentSave.gold += gold;
				if (rank > 0)
				{
					for (int j = 0; j < 5; j++)
					{
						if (shipmentSave.rank >> 4 * j == 0)
						{
							shipmentSave.rank |= rank << 4 * j;
							break;
						}
					}
				}
				id = shipmentSave.guid;
				this.Save.shipments[i] = shipmentSave;
				return false;
			}
		}
		shipmentSave.weight = weight;
		shipmentSave.gold = gold;
		shipmentSave.rank = rank;
		shipmentSave.sendDate = date;
		shipmentSave.guid = global::System.Guid.NewGuid().GetHashCode();
		this.Save.shipments.Add(shipmentSave);
		id = shipmentSave.guid;
		return true;
	}

	public global::ShipmentSave GetDelivery(int guid)
	{
		for (int i = 0; i < this.Save.shipments.Count; i++)
		{
			if (this.Save.shipments[i].guid == guid)
			{
				return this.Save.shipments[i];
			}
		}
		return default(global::ShipmentSave);
	}

	public void ClearDelivery(int guid)
	{
		for (int i = this.Save.shipments.Count - 1; i >= 0; i--)
		{
			if (this.Save.shipments[i].guid == guid)
			{
				this.Save.shipments.RemoveAt(i);
				return;
			}
		}
	}
}
