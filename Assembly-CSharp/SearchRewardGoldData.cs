using System;
using Mono.Data.Sqlite;

public class SearchRewardGoldData : global::DataCore
{
	public int Id { get; private set; }

	public global::SearchRewardId SearchRewardId { get; private set; }

	public int WarbandRank { get; private set; }

	public int Min { get; private set; }

	public int Max { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = reader.GetInt32(0);
		this.SearchRewardId = (global::SearchRewardId)reader.GetInt32(1);
		this.WarbandRank = reader.GetInt32(2);
		this.Min = reader.GetInt32(3);
		this.Max = reader.GetInt32(4);
	}
}
