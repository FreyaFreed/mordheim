using System;
using Mono.Data.Sqlite;

public class UnitTypeJoinSkillLineData : global::DataCore
{
	public global::UnitTypeId UnitTypeId { get; private set; }

	public global::SkillLineId SkillLineId { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.UnitTypeId = (global::UnitTypeId)reader.GetInt32(0);
		this.SkillLineId = (global::SkillLineId)reader.GetInt32(1);
	}
}
