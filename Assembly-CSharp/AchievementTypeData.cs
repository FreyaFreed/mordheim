using System;
using Mono.Data.Sqlite;

public class AchievementTypeData : global::DataCore
{
	public global::AchievementTypeId Id { get; private set; }

	public string Name { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = (global::AchievementTypeId)reader.GetInt32(0);
		this.Name = reader.GetString(1);
	}
}
