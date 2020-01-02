using System;
using Mono.Data.Sqlite;

public class CampaignMissionSpawnData : global::DataCore
{
	public global::CampaignMissionSpawnId Id { get; private set; }

	public string Name { get; private set; }

	public global::CampaignMissionId CampaignMissionId { get; private set; }

	public int Team { get; private set; }

	public int MinUnit { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = (global::CampaignMissionSpawnId)reader.GetInt32(0);
		this.Name = reader.GetString(1);
		this.CampaignMissionId = (global::CampaignMissionId)reader.GetInt32(2);
		this.Team = reader.GetInt32(3);
		this.MinUnit = reader.GetInt32(4);
	}
}
