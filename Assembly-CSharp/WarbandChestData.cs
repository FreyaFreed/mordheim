using System;
using Mono.Data.Sqlite;

public class WarbandChestData : global::DataCore
{
	public int Id { get; private set; }

	public global::WarbandId WarbandId { get; private set; }

	public global::ItemId ItemId { get; private set; }

	public int Count { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = reader.GetInt32(0);
		this.WarbandId = (global::WarbandId)reader.GetInt32(1);
		this.ItemId = (global::ItemId)reader.GetInt32(2);
		this.Count = reader.GetInt32(3);
	}
}
