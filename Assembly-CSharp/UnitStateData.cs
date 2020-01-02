using System;
using Mono.Data.Sqlite;

public class UnitStateData : global::DataCore
{
	public global::UnitStateId Id { get; private set; }

	public string Name { get; private set; }

	public string Fx { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = (global::UnitStateId)reader.GetInt32(0);
		this.Name = reader.GetString(1);
		this.Fx = reader.GetString(2);
	}
}
