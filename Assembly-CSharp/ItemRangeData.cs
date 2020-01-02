using System;
using Mono.Data.Sqlite;

public class ItemRangeData : global::DataCore
{
	public global::ItemRangeId Id { get; private set; }

	public string Name { get; private set; }

	public int MinRange { get; private set; }

	public int Range { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = (global::ItemRangeId)reader.GetInt32(0);
		this.Name = reader.GetString(1);
		this.MinRange = reader.GetInt32(2);
		this.Range = reader.GetInt32(3);
	}
}
