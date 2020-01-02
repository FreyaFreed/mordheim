using System;
using Mono.Data.Sqlite;

public class MutationAttributeData : global::DataCore
{
	public int Id { get; private set; }

	public global::MutationId MutationId { get; private set; }

	public global::AttributeId AttributeId { get; private set; }

	public global::UnitActionId UnitActionIdTrigger { get; private set; }

	public global::SkillId SkillIdTrigger { get; private set; }

	public int Modifier { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = reader.GetInt32(0);
		this.MutationId = (global::MutationId)reader.GetInt32(1);
		this.AttributeId = (global::AttributeId)reader.GetInt32(2);
		this.UnitActionIdTrigger = (global::UnitActionId)reader.GetInt32(3);
		this.SkillIdTrigger = (global::SkillId)reader.GetInt32(4);
		this.Modifier = reader.GetInt32(5);
	}
}
