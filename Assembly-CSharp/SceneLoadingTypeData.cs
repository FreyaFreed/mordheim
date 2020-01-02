using System;
using Mono.Data.Sqlite;

public class SceneLoadingTypeData : global::DataCore
{
	public global::SceneLoadingTypeId Id { get; private set; }

	public string Name { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = (global::SceneLoadingTypeId)reader.GetInt32(0);
		this.Name = reader.GetString(1);
	}
}
