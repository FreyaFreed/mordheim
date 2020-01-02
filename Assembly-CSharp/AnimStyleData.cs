using System;
using Mono.Data.Sqlite;

public class AnimStyleData : global::DataCore
{
	public global::AnimStyleId Id { get; private set; }

	public string Name { get; private set; }

	public string Size { get; private set; }

	public string Layer { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = (global::AnimStyleId)reader.GetInt32(0);
		this.Name = reader.GetString(1);
		this.Size = reader.GetString(2);
		this.Layer = reader.GetString(3);
	}
}
