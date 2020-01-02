using System;
using Mono.Data.Sqlite;

public class CampaignWarbandData : global::DataCore
{
	public global::CampaignWarbandId Id { get; private set; }

	public string Name { get; private set; }

	public global::WarbandId WarbandId { get; private set; }

	public global::ColorPresetId ColorPresetId { get; private set; }

	public int Rank { get; private set; }

	public bool NoWagon { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = (global::CampaignWarbandId)reader.GetInt32(0);
		this.Name = reader.GetString(1);
		this.WarbandId = (global::WarbandId)reader.GetInt32(2);
		this.ColorPresetId = (global::ColorPresetId)reader.GetInt32(3);
		this.Rank = reader.GetInt32(4);
		this.NoWagon = (reader.GetInt32(5) != 0);
	}
}
