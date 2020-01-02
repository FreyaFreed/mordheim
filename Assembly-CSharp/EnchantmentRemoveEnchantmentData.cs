using System;
using Mono.Data.Sqlite;

public class EnchantmentRemoveEnchantmentData : global::DataCore
{
	public int Id { get; private set; }

	public global::EnchantmentId EnchantmentId { get; private set; }

	public global::EnchantmentId EnchantmentIdRemove { get; private set; }

	public int Count { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = reader.GetInt32(0);
		this.EnchantmentId = (global::EnchantmentId)reader.GetInt32(1);
		this.EnchantmentIdRemove = (global::EnchantmentId)reader.GetInt32(2);
		this.Count = reader.GetInt32(3);
	}
}
