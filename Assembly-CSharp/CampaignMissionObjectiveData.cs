using System;
using Mono.Data.Sqlite;

public class CampaignMissionObjectiveData : global::DataCore
{
	public int Id { get; private set; }

	public global::CampaignMissionId CampaignMissionId { get; private set; }

	public global::PrimaryObjectiveId PrimaryObjectiveId { get; private set; }

	public int SortWeight { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = reader.GetInt32(0);
		this.CampaignMissionId = (global::CampaignMissionId)reader.GetInt32(1);
		this.PrimaryObjectiveId = (global::PrimaryObjectiveId)reader.GetInt32(2);
		this.SortWeight = reader.GetInt32(3);
	}
}
