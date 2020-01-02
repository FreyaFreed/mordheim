using System;
using Mono.Data.Sqlite;

public class CampaignUnitJoinItemData : global::DataCore
{
	public global::CampaignUnitId CampaignUnitId { get; private set; }

	public global::UnitSlotId UnitSlotId { get; private set; }

	public global::ItemId ItemId { get; private set; }

	public global::ItemQualityId ItemQualityId { get; private set; }

	public global::RuneMarkId RuneMarkId { get; private set; }

	public global::RuneMarkQualityId RuneMarkQualityId { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.CampaignUnitId = (global::CampaignUnitId)reader.GetInt32(0);
		this.UnitSlotId = (global::UnitSlotId)reader.GetInt32(1);
		this.ItemId = (global::ItemId)reader.GetInt32(2);
		this.ItemQualityId = (global::ItemQualityId)reader.GetInt32(3);
		this.RuneMarkId = (global::RuneMarkId)reader.GetInt32(4);
		this.RuneMarkQualityId = (global::RuneMarkQualityId)reader.GetInt32(5);
	}
}
