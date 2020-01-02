using System;
using Mono.Data.Sqlite;

public class UnitJoinSkillData : global::DataCore
{
	public global::UnitId UnitId { get; private set; }

	public global::SkillId SkillId { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.UnitId = (global::UnitId)reader.GetInt32(0);
		this.SkillId = (global::SkillId)reader.GetInt32(1);
	}
}
