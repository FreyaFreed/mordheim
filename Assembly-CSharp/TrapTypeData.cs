using System;
using Mono.Data.Sqlite;

public class TrapTypeData : global::DataCore
{
	public global::TrapTypeId Id { get; private set; }

	public string Name { get; private set; }

	public int Perc { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = (global::TrapTypeId)reader.GetInt32(0);
		this.Name = reader.GetString(1);
		this.Perc = reader.GetInt32(2);
	}
}
