using System;
using Mono.Data.Sqlite;

public class CampaignWarbandWhitelistData : global::DataCore
{
	public int Id { get; private set; }

	public global::CampaignWarbandId CampaignWarbandId { get; private set; }

	public global::CampaignWarbandId CampaignWarbandIdWhitelisted { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = reader.GetInt32(0);
		this.CampaignWarbandId = (global::CampaignWarbandId)reader.GetInt32(1);
		this.CampaignWarbandIdWhitelisted = (global::CampaignWarbandId)reader.GetInt32(2);
	}
}
