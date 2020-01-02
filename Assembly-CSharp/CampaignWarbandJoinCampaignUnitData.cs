using System;
using Mono.Data.Sqlite;

public class CampaignWarbandJoinCampaignUnitData : global::DataCore
{
	public global::CampaignWarbandId CampaignWarbandId { get; private set; }

	public global::CampaignUnitId CampaignUnitId { get; private set; }

	public string DeployNode { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.CampaignWarbandId = (global::CampaignWarbandId)reader.GetInt32(0);
		this.CampaignUnitId = (global::CampaignUnitId)reader.GetInt32(1);
		this.DeployNode = reader.GetString(2);
	}
}
