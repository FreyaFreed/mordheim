using System;
using Mono.Data.Sqlite;

public class CampaignMissionJoinCampaignWarbandData : global::DataCore
{
	public global::CampaignMissionId CampaignMissionId { get; private set; }

	public global::CampaignWarbandId CampaignWarbandId { get; private set; }

	public string DeployZone { get; private set; }

	public int Team { get; private set; }

	public global::PlayerTypeId PlayerTypeId { get; private set; }

	public bool CanRout { get; private set; }

	public bool Objective { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.CampaignMissionId = (global::CampaignMissionId)reader.GetInt32(0);
		this.CampaignWarbandId = (global::CampaignWarbandId)reader.GetInt32(1);
		this.DeployZone = reader.GetString(2);
		this.Team = reader.GetInt32(3);
		this.PlayerTypeId = (global::PlayerTypeId)reader.GetInt32(4);
		this.CanRout = (reader.GetInt32(5) != 0);
		this.Objective = (reader.GetInt32(6) != 0);
	}
}
