﻿using System;
using Mono.Data.Sqlite;

public class WarbandEnchantmentWyrdstoneDensityModifierData : global::DataCore
{
	public int Id { get; private set; }

	public global::WarbandEnchantmentId WarbandEnchantmentId { get; private set; }

	public global::ProcMissionRatingId ProcMissionRatingId { get; private set; }

	public global::WyrdstoneDensityId WyrdstoneDensityId { get; private set; }

	public int Modifier { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = reader.GetInt32(0);
		this.WarbandEnchantmentId = (global::WarbandEnchantmentId)reader.GetInt32(1);
		this.ProcMissionRatingId = (global::ProcMissionRatingId)reader.GetInt32(2);
		this.WyrdstoneDensityId = (global::WyrdstoneDensityId)reader.GetInt32(3);
		this.Modifier = reader.GetInt32(4);
	}
}
