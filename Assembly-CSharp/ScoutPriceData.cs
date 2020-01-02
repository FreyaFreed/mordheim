using System;
using Mono.Data.Sqlite;

public class ScoutPriceData : global::DataCore
{
	public int WarbandRank { get; private set; }

	public int Idx { get; private set; }

	public int Price { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.WarbandRank = reader.GetInt32(0);
		this.Idx = reader.GetInt32(1);
		this.Price = reader.GetInt32(2);
	}
}
