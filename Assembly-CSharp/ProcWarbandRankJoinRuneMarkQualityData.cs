using System;
using Mono.Data.Sqlite;

public class ProcWarbandRankJoinRuneMarkQualityData : global::DataCore
{
	public global::ProcWarbandRankId ProcWarbandRankId { get; private set; }

	public global::RuneMarkQualityId RuneMarkQualityId { get; private set; }

	public int MinCount { get; private set; }

	public int MaxCount { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.ProcWarbandRankId = (global::ProcWarbandRankId)reader.GetInt32(0);
		this.RuneMarkQualityId = (global::RuneMarkQualityId)reader.GetInt32(1);
		this.MinCount = reader.GetInt32(2);
		this.MaxCount = reader.GetInt32(3);
	}
}
