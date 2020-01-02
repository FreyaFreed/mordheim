using System;
using Mono.Data.Sqlite;

public class RuneMarkQualityJoinItemTypeData : global::DataCore
{
	public global::RuneMarkQualityId RuneMarkQualityId { get; private set; }

	public global::ItemTypeId ItemTypeId { get; private set; }

	public int Rating { get; private set; }

	public int CostModifier { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.RuneMarkQualityId = (global::RuneMarkQualityId)reader.GetInt32(0);
		this.ItemTypeId = (global::ItemTypeId)reader.GetInt32(1);
		this.Rating = reader.GetInt32(2);
		this.CostModifier = reader.GetInt32(3);
	}
}
