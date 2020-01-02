using System;
using Mono.Data.Sqlite;

public class UnitWoundData : global::DataCore
{
	public global::UnitWoundId Id { get; private set; }

	public string Name { get; private set; }

	public int BaseWound { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = (global::UnitWoundId)reader.GetInt32(0);
		this.Name = reader.GetString(1);
		this.BaseWound = reader.GetInt32(2);
	}
}
