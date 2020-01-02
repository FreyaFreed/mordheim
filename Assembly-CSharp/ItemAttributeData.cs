using System;
using Mono.Data.Sqlite;

public class ItemAttributeData : global::DataCore
{
	public int Id { get; private set; }

	public global::ItemId ItemId { get; private set; }

	public global::ItemQualityId ItemQualityId { get; private set; }

	public global::MutationId MutationId { get; private set; }

	public global::AttributeId AttributeId { get; private set; }

	public global::UnitActionId UnitActionIdTrigger { get; private set; }

	public global::SkillId SkillIdTrigger { get; private set; }

	public int Modifier { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = reader.GetInt32(0);
		this.ItemId = (global::ItemId)reader.GetInt32(1);
		this.ItemQualityId = (global::ItemQualityId)reader.GetInt32(2);
		this.MutationId = (global::MutationId)reader.GetInt32(3);
		this.AttributeId = (global::AttributeId)reader.GetInt32(4);
		this.UnitActionIdTrigger = (global::UnitActionId)reader.GetInt32(5);
		this.SkillIdTrigger = (global::SkillId)reader.GetInt32(6);
		this.Modifier = reader.GetInt32(7);
	}
}
