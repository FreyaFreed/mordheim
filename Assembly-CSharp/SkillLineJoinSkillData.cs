using System;
using Mono.Data.Sqlite;

public class SkillLineJoinSkillData : global::DataCore
{
	public global::SkillLineId SkillLineId { get; private set; }

	public global::SkillId SkillId { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.SkillLineId = (global::SkillLineId)reader.GetInt32(0);
		this.SkillId = (global::SkillId)reader.GetInt32(1);
	}
}
