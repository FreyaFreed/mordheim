using System;
using Mono.Data.Sqlite;

public class CampaignMissionMessageData : global::DataCore
{
	public global::CampaignMissionMessageId Id { get; private set; }

	public string Label { get; private set; }

	public global::CampaignMissionId CampaignMissionId { get; private set; }

	public int UnitTurn { get; private set; }

	public int Position { get; private set; }

	public string Name { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = (global::CampaignMissionMessageId)reader.GetInt32(0);
		this.Label = reader.GetString(1);
		this.CampaignMissionId = (global::CampaignMissionId)reader.GetInt32(2);
		this.UnitTurn = reader.GetInt32(3);
		this.Position = reader.GetInt32(4);
		this.Name = reader.GetString(5);
	}
}
