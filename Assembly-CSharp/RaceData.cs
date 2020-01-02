using System;
using Mono.Data.Sqlite;

public class RaceData : global::DataCore
{
	public global::RaceId Id { get; private set; }

	public string Name { get; private set; }

	public global::ItemId ItemIdTrophy { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = (global::RaceId)reader.GetInt32(0);
		this.Name = reader.GetString(1);
		this.ItemIdTrophy = (global::ItemId)reader.GetInt32(2);
	}
}
