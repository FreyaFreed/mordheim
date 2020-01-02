using System;
using Mono.Data.Sqlite;

public class TrapEffectData : global::DataCore
{
	public global::TrapEffectId Id { get; private set; }

	public string Name { get; private set; }

	public string Fx { get; private set; }

	public global::ZoneAoeId ZoneAoeId { get; private set; }

	public double Radius { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = (global::TrapEffectId)reader.GetInt32(0);
		this.Name = reader.GetString(1);
		this.Fx = reader.GetString(2);
		this.ZoneAoeId = (global::ZoneAoeId)reader.GetInt32(3);
		this.Radius = reader.GetDouble(4);
	}
}
