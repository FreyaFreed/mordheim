using System;
using Mono.Data.Sqlite;

public class InjuryJoinEnchantmentData : global::DataCore
{
	public global::InjuryId InjuryId { get; private set; }

	public global::EnchantmentId EnchantmentId { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.InjuryId = (global::InjuryId)reader.GetInt32(0);
		this.EnchantmentId = (global::EnchantmentId)reader.GetInt32(1);
	}
}
