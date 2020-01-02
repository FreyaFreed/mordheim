using System;
using Mono.Data.Sqlite;

public class EnchantmentCurseModifierData : global::DataCore
{
	public int Id { get; private set; }

	public global::EnchantmentId EnchantmentId { get; private set; }

	public global::SpellCurseId SpellCurseId { get; private set; }

	public int RatioModifier { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = reader.GetInt32(0);
		this.EnchantmentId = (global::EnchantmentId)reader.GetInt32(1);
		this.SpellCurseId = (global::SpellCurseId)reader.GetInt32(2);
		this.RatioModifier = reader.GetInt32(3);
	}
}
