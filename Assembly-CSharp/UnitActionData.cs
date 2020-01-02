using System;
using Mono.Data.Sqlite;

public class UnitActionData : global::DataCore
{
	public global::UnitActionId Id { get; private set; }

	public string Name { get; private set; }

	public bool Confirmation { get; private set; }

	public bool Interactive { get; private set; }

	public global::UnitActionRefreshId UnitActionRefreshId { get; private set; }

	public int SortWeight { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = (global::UnitActionId)reader.GetInt32(0);
		this.Name = reader.GetString(1);
		this.Confirmation = (reader.GetInt32(2) != 0);
		this.Interactive = (reader.GetInt32(3) != 0);
		this.UnitActionRefreshId = (global::UnitActionRefreshId)reader.GetInt32(4);
		this.SortWeight = reader.GetInt32(5);
	}
}
