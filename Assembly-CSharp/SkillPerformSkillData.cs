using System;
using Mono.Data.Sqlite;

public class SkillPerformSkillData : global::DataCore
{
	public int Id { get; private set; }

	public global::SkillId SkillId { get; private set; }

	public global::SkillId SkillIdPerformed { get; private set; }

	public global::AttributeId AttributeIdRoll { get; private set; }

	public bool Success { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = reader.GetInt32(0);
		this.SkillId = (global::SkillId)reader.GetInt32(1);
		this.SkillIdPerformed = (global::SkillId)reader.GetInt32(2);
		this.AttributeIdRoll = (global::AttributeId)reader.GetInt32(3);
		this.Success = (reader.GetInt32(4) != 0);
	}
}
