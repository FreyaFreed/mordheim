using System;
using Mono.Data.Sqlite;

public class EnchantmentBlockUnitActionData : global::DataCore
{
	public int Id { get; private set; }

	public global::EnchantmentId EnchantmentId { get; private set; }

	public global::UnitActionId UnitActionId { get; private set; }

	public global::SkillId SkillId { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = reader.GetInt32(0);
		this.EnchantmentId = (global::EnchantmentId)reader.GetInt32(1);
		this.UnitActionId = (global::UnitActionId)reader.GetInt32(2);
		this.SkillId = (global::SkillId)reader.GetInt32(3);
	}
}
