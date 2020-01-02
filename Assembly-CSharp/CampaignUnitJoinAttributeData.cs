using System;
using Mono.Data.Sqlite;

public class CampaignUnitJoinAttributeData : global::DataCore
{
	public global::CampaignUnitId CampaignUnitId { get; private set; }

	public global::AttributeId AttributeId { get; private set; }

	public int Value { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.CampaignUnitId = (global::CampaignUnitId)reader.GetInt32(0);
		this.AttributeId = (global::AttributeId)reader.GetInt32(1);
		this.Value = reader.GetInt32(2);
	}
}
