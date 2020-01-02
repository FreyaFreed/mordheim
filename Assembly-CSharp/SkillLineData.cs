using System;
using Mono.Data.Sqlite;

public class SkillLineData : global::DataCore
{
	public global::SkillLineId Id { get; private set; }

	public string Name { get; private set; }

	public global::SkillLineId SkillLineIdDisplayed { get; private set; }

	public global::WarbandAttributeId WarbandAttributeIdPriceModifier { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = (global::SkillLineId)reader.GetInt32(0);
		this.Name = reader.GetString(1);
		this.SkillLineIdDisplayed = (global::SkillLineId)reader.GetInt32(2);
		this.WarbandAttributeIdPriceModifier = (global::WarbandAttributeId)reader.GetInt32(3);
	}
}
