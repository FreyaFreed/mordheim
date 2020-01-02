using System;
using Mono.Data.Sqlite;

public class TableFilterData : global::DataCore
{
	public global::TableFilterId Id { get; private set; }

	public string Name { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = (global::TableFilterId)reader.GetInt32(0);
		this.Name = reader.GetString(1);
	}
}
