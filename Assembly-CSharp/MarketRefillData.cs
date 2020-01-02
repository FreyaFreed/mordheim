using System;
using Mono.Data.Sqlite;

public class MarketRefillData : global::DataCore
{
	public int Id { get; private set; }

	public global::WarbandRankId WarbandRankId { get; private set; }

	public global::ItemCategoryId ItemCategoryId { get; private set; }

	public int QuantityMin { get; private set; }

	public int QuantityMax { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = reader.GetInt32(0);
		this.WarbandRankId = (global::WarbandRankId)reader.GetInt32(1);
		this.ItemCategoryId = (global::ItemCategoryId)reader.GetInt32(2);
		this.QuantityMin = reader.GetInt32(3);
		this.QuantityMax = reader.GetInt32(4);
	}
}
