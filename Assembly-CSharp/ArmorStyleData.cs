using System;
using Mono.Data.Sqlite;

public class ArmorStyleData : global::DataCore
{
	public global::ArmorStyleId Id { get; private set; }

	public string Name { get; private set; }

	public global::ItemTypeId ItemTypeIdArmor { get; private set; }

	public global::ItemTypeId ItemTypeIdHelmet { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = (global::ArmorStyleId)reader.GetInt32(0);
		this.Name = reader.GetString(1);
		this.ItemTypeIdArmor = (global::ItemTypeId)reader.GetInt32(2);
		this.ItemTypeIdHelmet = (global::ItemTypeId)reader.GetInt32(3);
	}
}
