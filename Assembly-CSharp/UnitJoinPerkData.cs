using System;
using Mono.Data.Sqlite;

public class UnitJoinPerkData : global::DataCore
{
	public global::UnitId UnitId { get; private set; }

	public global::PerkId PerkId { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.UnitId = (global::UnitId)reader.GetInt32(0);
		this.PerkId = (global::PerkId)reader.GetInt32(1);
	}
}
