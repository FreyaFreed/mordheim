using System;
using Mono.Data.Sqlite;

public class MissionMapGameplayData : global::DataCore
{
	public global::MissionMapGameplayId Id { get; private set; }

	public string Name { get; private set; }

	public global::MissionMapId MissionMapId { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = (global::MissionMapGameplayId)reader.GetInt32(0);
		this.Name = reader.GetString(1);
		this.MissionMapId = (global::MissionMapId)reader.GetInt32(2);
	}
}
