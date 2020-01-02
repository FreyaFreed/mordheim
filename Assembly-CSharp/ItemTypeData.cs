using System;
using Mono.Data.Sqlite;

public class ItemTypeData : global::DataCore
{
	public global::ItemTypeId Id { get; private set; }

	public string Name { get; private set; }

	public global::ItemCategoryId ItemCategoryId { get; private set; }

	public bool IsTwoHanded { get; private set; }

	public bool IsRange { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = (global::ItemTypeId)reader.GetInt32(0);
		this.Name = reader.GetString(1);
		this.ItemCategoryId = (global::ItemCategoryId)reader.GetInt32(2);
		this.IsTwoHanded = (reader.GetInt32(3) != 0);
		this.IsRange = (reader.GetInt32(4) != 0);
	}
}
