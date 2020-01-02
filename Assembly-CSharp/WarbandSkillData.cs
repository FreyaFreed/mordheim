using System;
using Mono.Data.Sqlite;

public class WarbandSkillData : global::DataCore
{
	public global::WarbandSkillId Id { get; private set; }

	public string Name { get; private set; }

	public global::WarbandSkillTypeId WarbandSkillTypeId { get; private set; }

	public global::SkillQualityId SkillQualityId { get; private set; }

	public global::WarbandSkillId WarbandSkillIdPrerequisite { get; private set; }

	public int Points { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = (global::WarbandSkillId)reader.GetInt32(0);
		this.Name = reader.GetString(1);
		this.WarbandSkillTypeId = (global::WarbandSkillTypeId)reader.GetInt32(2);
		this.SkillQualityId = (global::SkillQualityId)reader.GetInt32(3);
		this.WarbandSkillIdPrerequisite = (global::WarbandSkillId)reader.GetInt32(4);
		this.Points = reader.GetInt32(5);
	}
}
