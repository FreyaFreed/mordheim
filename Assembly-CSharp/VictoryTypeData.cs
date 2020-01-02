using System;
using Mono.Data.Sqlite;

public class VictoryTypeData : global::DataCore
{
	public global::VictoryTypeId Id { get; private set; }

	public string Name { get; private set; }

	public int WarbandExperience { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = (global::VictoryTypeId)reader.GetInt32(0);
		this.Name = reader.GetString(1);
		this.WarbandExperience = reader.GetInt32(2);
	}
}
