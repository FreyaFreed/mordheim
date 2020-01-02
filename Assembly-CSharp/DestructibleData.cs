using System;
using Mono.Data.Sqlite;

public class DestructibleData : global::DataCore
{
	public global::DestructibleId Id { get; private set; }

	public string Name { get; private set; }

	public int Wounds { get; private set; }

	public global::ZoneAoeId ZoneAoeId { get; private set; }

	public double ZoneAoeRadius { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = (global::DestructibleId)reader.GetInt32(0);
		this.Name = reader.GetString(1);
		this.Wounds = reader.GetInt32(2);
		this.ZoneAoeId = (global::ZoneAoeId)reader.GetInt32(3);
		this.ZoneAoeRadius = reader.GetDouble(4);
	}
}
