using System;
using Mono.Data.Sqlite;

public class WyrdstoneDensityProgressionData : global::DataCore
{
	public global::WyrdstoneDensityId WyrdstoneDensityId { get; private set; }

	public int WarbandRank { get; private set; }

	public int Fragment { get; private set; }

	public int Shard { get; private set; }

	public int Cluster { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.WyrdstoneDensityId = (global::WyrdstoneDensityId)reader.GetInt32(0);
		this.WarbandRank = reader.GetInt32(1);
		this.Fragment = reader.GetInt32(2);
		this.Shard = reader.GetInt32(3);
		this.Cluster = reader.GetInt32(4);
	}
}
