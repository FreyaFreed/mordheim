using System;
using Mono.Data.Sqlite;

public class TrapEffectJoinEnchantmentData : global::DataCore
{
	public global::TrapEffectId TrapEffectId { get; private set; }

	public global::EnchantmentId EnchantmentId { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.TrapEffectId = (global::TrapEffectId)reader.GetInt32(0);
		this.EnchantmentId = (global::EnchantmentId)reader.GetInt32(1);
	}
}
