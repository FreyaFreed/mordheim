using System;
using Mono.Data.Sqlite;

public class WyrdstoneTypeData : global::DataCore
{
	public global::WyrdstoneTypeId Id { get; private set; }

	public string Name { get; private set; }

	public global::ItemId ItemId { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = (global::WyrdstoneTypeId)reader.GetInt32(0);
		this.Name = reader.GetString(1);
		this.ItemId = (global::ItemId)reader.GetInt32(2);
	}
}
