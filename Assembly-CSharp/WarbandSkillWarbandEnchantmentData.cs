using System;
using Mono.Data.Sqlite;

public class WarbandSkillWarbandEnchantmentData : global::DataCore
{
	public int Id { get; private set; }

	public global::WarbandSkillId WarbandSkillId { get; private set; }

	public global::WarbandEnchantmentId WarbandEnchantmentId { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = reader.GetInt32(0);
		this.WarbandSkillId = (global::WarbandSkillId)reader.GetInt32(1);
		this.WarbandEnchantmentId = (global::WarbandEnchantmentId)reader.GetInt32(2);
	}
}
