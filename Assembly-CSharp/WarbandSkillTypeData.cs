using System;
using Mono.Data.Sqlite;

public class WarbandSkillTypeData : global::DataCore
{
	public global::WarbandSkillTypeId Id { get; private set; }

	public string Name { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = (global::WarbandSkillTypeId)reader.GetInt32(0);
		this.Name = reader.GetString(1);
	}
}
