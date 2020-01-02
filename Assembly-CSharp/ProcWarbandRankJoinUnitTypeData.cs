using System;
using Mono.Data.Sqlite;

public class ProcWarbandRankJoinUnitTypeData : global::DataCore
{
	public global::ProcWarbandRankId ProcWarbandRankId { get; private set; }

	public global::UnitTypeId UnitTypeId { get; private set; }

	public int MinCount { get; private set; }

	public int MaxCount { get; private set; }

	public int MinRank { get; private set; }

	public int MaxRank { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.ProcWarbandRankId = (global::ProcWarbandRankId)reader.GetInt32(0);
		this.UnitTypeId = (global::UnitTypeId)reader.GetInt32(1);
		this.MinCount = reader.GetInt32(2);
		this.MaxCount = reader.GetInt32(3);
		this.MinRank = reader.GetInt32(4);
		this.MaxRank = reader.GetInt32(5);
	}
}
