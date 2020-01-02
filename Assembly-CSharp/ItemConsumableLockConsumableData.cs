using System;
using Mono.Data.Sqlite;

public class ItemConsumableLockConsumableData : global::DataCore
{
	public int Id { get; private set; }

	public global::SkillId SkillId { get; private set; }

	public global::SkillId SkillIdLocked { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = reader.GetInt32(0);
		this.SkillId = (global::SkillId)reader.GetInt32(1);
		this.SkillIdLocked = (global::SkillId)reader.GetInt32(2);
	}
}
