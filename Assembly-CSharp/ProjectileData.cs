using System;
using Mono.Data.Sqlite;

public class ProjectileData : global::DataCore
{
	public global::ProjectileId Id { get; private set; }

	public string Name { get; private set; }

	public string Sound { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = (global::ProjectileId)reader.GetInt32(0);
		this.Name = reader.GetString(1);
		this.Sound = reader.GetString(2);
	}
}
