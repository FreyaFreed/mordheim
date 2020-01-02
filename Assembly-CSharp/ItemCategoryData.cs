using System;
using Mono.Data.Sqlite;

public class ItemCategoryData : global::DataCore
{
	public global::ItemCategoryId Id { get; private set; }

	public string Name { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = (global::ItemCategoryId)reader.GetInt32(0);
		this.Name = reader.GetString(1);
	}
}
