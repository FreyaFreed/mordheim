using System;
using Mono.Data.Sqlite;

public class EnchantmentInjuryModifierData : global::DataCore
{
	public int Id { get; private set; }

	public global::EnchantmentId EnchantmentId { get; private set; }

	public global::InjuryId InjuryId { get; private set; }

	public int RatioModifier { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = reader.GetInt32(0);
		this.EnchantmentId = (global::EnchantmentId)reader.GetInt32(1);
		this.InjuryId = (global::InjuryId)reader.GetInt32(2);
		this.RatioModifier = reader.GetInt32(3);
	}
}
