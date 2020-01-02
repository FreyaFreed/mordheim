using System;
using Mono.Data.Sqlite;

public class SearchZoneData : global::DataCore
{
	public global::SearchZoneId Id { get; private set; }

	public string Name { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = (global::SearchZoneId)reader.GetInt32(0);
		this.Name = reader.GetString(1);
	}
}
