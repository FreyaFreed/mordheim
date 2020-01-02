using System;
using Mono.Data.Sqlite;

public class ZoneAoeData : global::DataCore
{
	public global::ZoneAoeId Id { get; private set; }

	public string Name { get; private set; }

	public int Duration { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = (global::ZoneAoeId)reader.GetInt32(0);
		this.Name = reader.GetString(1);
		this.Duration = reader.GetInt32(2);
	}
}
