using System;
using Mono.Data.Sqlite;

public class UnitJoinEnchantmentData : global::DataCore
{
	public global::UnitId UnitId { get; private set; }

	public global::EnchantmentId EnchantmentId { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.UnitId = (global::UnitId)reader.GetInt32(0);
		this.EnchantmentId = (global::EnchantmentId)reader.GetInt32(1);
	}
}
