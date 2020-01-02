using System;
using Mono.Data.Sqlite;

public class MissionMapData : global::DataCore
{
	public global::MissionMapId Id { get; private set; }

	public string Name { get; private set; }

	public global::DistrictId DistrictId { get; private set; }

	public int Idx { get; private set; }

	public int LoadingImageCount { get; private set; }

	public bool HasRecastHelper { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = (global::MissionMapId)reader.GetInt32(0);
		this.Name = reader.GetString(1);
		this.DistrictId = (global::DistrictId)reader.GetInt32(2);
		this.Idx = reader.GetInt32(3);
		this.LoadingImageCount = reader.GetInt32(4);
		this.HasRecastHelper = (reader.GetInt32(5) != 0);
	}
}
