using System;
using Mono.Data.Sqlite;

public class WarbandSkillUnitTypeRankData : global::DataCore
{
	public int Id { get; private set; }

	public global::WarbandSkillId WarbandSkillId { get; private set; }

	public global::UnitTypeId UnitTypeId { get; private set; }

	public int Rank { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = reader.GetInt32(0);
		this.WarbandSkillId = (global::WarbandSkillId)reader.GetInt32(1);
		this.UnitTypeId = (global::UnitTypeId)reader.GetInt32(2);
		this.Rank = reader.GetInt32(3);
	}
}
