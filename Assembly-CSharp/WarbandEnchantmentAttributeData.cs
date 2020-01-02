﻿using System;
using Mono.Data.Sqlite;

public class WarbandEnchantmentAttributeData : global::DataCore
{
	public int Id { get; private set; }

	public global::WarbandEnchantmentId WarbandEnchantmentId { get; private set; }

	public global::WarbandAttributeId WarbandAttributeId { get; private set; }

	public int Modifier { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = reader.GetInt32(0);
		this.WarbandEnchantmentId = (global::WarbandEnchantmentId)reader.GetInt32(1);
		this.WarbandAttributeId = (global::WarbandAttributeId)reader.GetInt32(2);
		this.Modifier = reader.GetInt32(3);
	}
}
