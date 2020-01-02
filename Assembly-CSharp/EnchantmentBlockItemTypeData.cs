using System;
using Mono.Data.Sqlite;

public class EnchantmentBlockItemTypeData : global::DataCore
{
	public int Id { get; private set; }

	public global::EnchantmentId EnchantmentId { get; private set; }

	public global::ItemTypeId ItemTypeId { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = reader.GetInt32(0);
		this.EnchantmentId = (global::EnchantmentId)reader.GetInt32(1);
		this.ItemTypeId = (global::ItemTypeId)reader.GetInt32(2);
	}
}
