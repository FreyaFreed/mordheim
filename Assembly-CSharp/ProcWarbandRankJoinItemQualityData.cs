using System;
using Mono.Data.Sqlite;

public class ProcWarbandRankJoinItemQualityData : global::DataCore
{
	public global::ProcWarbandRankId ProcWarbandRankId { get; private set; }

	public global::ItemQualityId ItemQualityId { get; private set; }

	public int MinCount { get; private set; }

	public int MaxCount { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.ProcWarbandRankId = (global::ProcWarbandRankId)reader.GetInt32(0);
		this.ItemQualityId = (global::ItemQualityId)reader.GetInt32(1);
		this.MinCount = reader.GetInt32(2);
		this.MaxCount = reader.GetInt32(3);
	}
}
