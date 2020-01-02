using System;
using Mono.Data.Sqlite;

public class ItemSpeedData : global::DataCore
{
	public global::ItemSpeedId Id { get; private set; }

	public string Name { get; private set; }

	public int Speed { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = (global::ItemSpeedId)reader.GetInt32(0);
		this.Name = reader.GetString(1);
		this.Speed = reader.GetInt32(2);
	}
}
