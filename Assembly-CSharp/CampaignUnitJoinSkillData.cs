using System;
using Mono.Data.Sqlite;

public class CampaignUnitJoinSkillData : global::DataCore
{
	public global::CampaignUnitId CampaignUnitId { get; private set; }

	public global::SkillId SkillId { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.CampaignUnitId = (global::CampaignUnitId)reader.GetInt32(0);
		this.SkillId = (global::SkillId)reader.GetInt32(1);
	}
}
