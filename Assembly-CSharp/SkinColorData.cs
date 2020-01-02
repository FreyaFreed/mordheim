using System;
using Mono.Data.Sqlite;

public class SkinColorData : global::DataCore
{
	public global::SkinColorId Id { get; private set; }

	public string Name { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = (global::SkinColorId)reader.GetInt32(0);
		this.Name = reader.GetString(1);
	}
}
