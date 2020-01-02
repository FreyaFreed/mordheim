using System;
using Mono.Data.Sqlite;

public class MissionMapLayoutData : global::DataCore
{
	public global::MissionMapLayoutId Id { get; private set; }

	public string Name { get; private set; }

	public global::MissionMapId MissionMapId { get; private set; }

	public string CloudsName { get; private set; }

	public string LightsName { get; private set; }

	public string FxName { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = (global::MissionMapLayoutId)reader.GetInt32(0);
		this.Name = reader.GetString(1);
		this.MissionMapId = (global::MissionMapId)reader.GetInt32(2);
		this.CloudsName = reader.GetString(3);
		this.LightsName = reader.GetString(4);
		this.FxName = reader.GetString(5);
	}
}
