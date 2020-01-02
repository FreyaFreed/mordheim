using System;
using Mono.Data.Sqlite;

public class ItemUnitData : global::DataCore
{
	public int Id { get; private set; }

	public global::ItemId ItemId { get; private set; }

	public global::UnitId UnitId { get; private set; }

	public bool Mutation { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = reader.GetInt32(0);
		this.ItemId = (global::ItemId)reader.GetInt32(1);
		this.UnitId = (global::UnitId)reader.GetInt32(2);
		this.Mutation = (reader.GetInt32(3) != 0);
	}
}
