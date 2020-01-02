using System;
using Mono.Data.Sqlite;

public class UnitActionRefreshData : global::DataCore
{
	public global::UnitActionRefreshId Id { get; private set; }

	public string Name { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = (global::UnitActionRefreshId)reader.GetInt32(0);
		this.Name = reader.GetString(1);
	}
}
