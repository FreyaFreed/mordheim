using System;
using Mono.Data.Sqlite;

public class ProcWarbandRankData : global::DataCore
{
	public global::ProcWarbandRankId Id { get; private set; }

	public string Name { get; private set; }

	public int Rating { get; private set; }

	public int WarbandRank { get; private set; }

	public global::WarbandRankId WarbandRankId { get; private set; }

	public int MinUnit { get; private set; }

	public int MaxUnit { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = (global::ProcWarbandRankId)reader.GetInt32(0);
		this.Name = reader.GetString(1);
		this.Rating = reader.GetInt32(2);
		this.WarbandRank = reader.GetInt32(3);
		this.WarbandRankId = (global::WarbandRankId)reader.GetInt32(4);
		this.MinUnit = reader.GetInt32(5);
		this.MaxUnit = reader.GetInt32(6);
	}
}
