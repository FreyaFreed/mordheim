﻿using System;
using Mono.Data.Sqlite;

public class CampaignMissionItemData : global::DataCore
{
	public int Id { get; private set; }

	public global::CampaignMissionId CampaignMissionId { get; private set; }

	public global::CampaignWarbandId CampaignWarbandId { get; private set; }

	public global::ItemId ItemId { get; private set; }

	public global::ItemQualityId ItemQualityId { get; private set; }

	public global::RuneMarkId RuneMarkId { get; private set; }

	public global::RuneMarkQualityId RuneMarkQualityId { get; private set; }

	public global::AllegianceId AllegianceId { get; private set; }

	public int Amount { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = reader.GetInt32(0);
		this.CampaignMissionId = (global::CampaignMissionId)reader.GetInt32(1);
		this.CampaignWarbandId = (global::CampaignWarbandId)reader.GetInt32(2);
		this.ItemId = (global::ItemId)reader.GetInt32(3);
		this.ItemQualityId = (global::ItemQualityId)reader.GetInt32(4);
		this.RuneMarkId = (global::RuneMarkId)reader.GetInt32(5);
		this.RuneMarkQualityId = (global::RuneMarkQualityId)reader.GetInt32(6);
		this.AllegianceId = (global::AllegianceId)reader.GetInt32(7);
		this.Amount = reader.GetInt32(8);
	}
}
