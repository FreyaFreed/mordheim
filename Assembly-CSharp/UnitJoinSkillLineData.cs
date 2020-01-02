using System;
using Mono.Data.Sqlite;

public class UnitJoinSkillLineData : global::DataCore
{
	public global::UnitId UnitId { get; private set; }

	public global::SkillLineId SkillLineId { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.UnitId = (global::UnitId)reader.GetInt32(0);
		this.SkillLineId = (global::SkillLineId)reader.GetInt32(1);
	}
}
