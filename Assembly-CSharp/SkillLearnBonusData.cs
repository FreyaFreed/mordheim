using System;
using Mono.Data.Sqlite;

public class SkillLearnBonusData : global::DataCore
{
	public int Id { get; private set; }

	public global::SkillId SkillId { get; private set; }

	public int Physical { get; private set; }

	public int Mental { get; private set; }

	public int Martial { get; private set; }

	public int Skill { get; private set; }

	public int Spell { get; private set; }

	public global::UnitTypeId UnitTypeId { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = reader.GetInt32(0);
		this.SkillId = (global::SkillId)reader.GetInt32(1);
		this.Physical = reader.GetInt32(2);
		this.Mental = reader.GetInt32(3);
		this.Martial = reader.GetInt32(4);
		this.Skill = reader.GetInt32(5);
		this.Spell = reader.GetInt32(6);
		this.UnitTypeId = (global::UnitTypeId)reader.GetInt32(7);
	}
}
