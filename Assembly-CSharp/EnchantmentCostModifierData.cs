using System;
using Mono.Data.Sqlite;

public class EnchantmentCostModifierData : global::DataCore
{
	public int Id { get; private set; }

	public global::EnchantmentId EnchantmentId { get; private set; }

	public global::UnitActionId UnitActionId { get; private set; }

	public global::SkillId SkillId { get; private set; }

	public global::SpellTypeId SpellTypeId { get; private set; }

	public int StrategyPoints { get; private set; }

	public int OffensePoints { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = reader.GetInt32(0);
		this.EnchantmentId = (global::EnchantmentId)reader.GetInt32(1);
		this.UnitActionId = (global::UnitActionId)reader.GetInt32(2);
		this.SkillId = (global::SkillId)reader.GetInt32(3);
		this.SpellTypeId = (global::SpellTypeId)reader.GetInt32(4);
		this.StrategyPoints = reader.GetInt32(5);
		this.OffensePoints = reader.GetInt32(6);
	}
}
