using System;
using Mono.Data.Sqlite;

public class RuneMarkJoinItemTypeData : global::DataCore
{
	public global::RuneMarkId RuneMarkId { get; private set; }

	public global::ItemTypeId ItemTypeId { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.RuneMarkId = (global::RuneMarkId)reader.GetInt32(0);
		this.ItemTypeId = (global::ItemTypeId)reader.GetInt32(1);
	}
}
