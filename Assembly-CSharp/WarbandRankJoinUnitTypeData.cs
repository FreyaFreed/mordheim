using System;
using Mono.Data.Sqlite;

public class WarbandRankJoinUnitTypeData : global::DataCore
{
	public global::WarbandRankId WarbandRankId { get; private set; }

	public global::UnitTypeId UnitTypeId { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.WarbandRankId = (global::WarbandRankId)reader.GetInt32(0);
		this.UnitTypeId = (global::UnitTypeId)reader.GetInt32(1);
	}
}
