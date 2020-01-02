using System;
using Mono.Data.Sqlite;

public class RuneMarkAttributeData : global::DataCore
{
	public int Id { get; private set; }

	public global::RuneMarkId RuneMarkId { get; private set; }

	public global::RuneMarkQualityId RuneMarkQualityId { get; private set; }

	public global::ItemTypeId ItemTypeId { get; private set; }

	public global::AttributeId AttributeId { get; private set; }

	public global::UnitActionId UnitActionId { get; private set; }

	public global::SkillId SkillId { get; private set; }

	public int Modifier { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = reader.GetInt32(0);
		this.RuneMarkId = (global::RuneMarkId)reader.GetInt32(1);
		this.RuneMarkQualityId = (global::RuneMarkQualityId)reader.GetInt32(2);
		this.ItemTypeId = (global::ItemTypeId)reader.GetInt32(3);
		this.AttributeId = (global::AttributeId)reader.GetInt32(4);
		this.UnitActionId = (global::UnitActionId)reader.GetInt32(5);
		this.SkillId = (global::SkillId)reader.GetInt32(6);
		this.Modifier = reader.GetInt32(7);
	}
}
