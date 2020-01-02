using System;
using Mono.Data.Sqlite;

public class WarbandContactData : global::DataCore
{
	public global::WarbandContactId Id { get; private set; }

	public string Name { get; private set; }

	public global::ItemTypeId ItemTypeId { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = (global::WarbandContactId)reader.GetInt32(0);
		this.Name = reader.GetString(1);
		this.ItemTypeId = (global::ItemTypeId)reader.GetInt32(2);
	}
}
