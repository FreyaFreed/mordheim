﻿using System;
using Mono.Data.Sqlite;

public class ItemSlotEnchantmentData : global::DataCore
{
	public int Id { get; private set; }

	public global::ItemId ItemId { get; private set; }

	public global::UnitSlotId UnitSlotId { get; private set; }

	public global::EnchantmentId EnchantmentId { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = reader.GetInt32(0);
		this.ItemId = (global::ItemId)reader.GetInt32(1);
		this.UnitSlotId = (global::UnitSlotId)reader.GetInt32(2);
		this.EnchantmentId = (global::EnchantmentId)reader.GetInt32(3);
	}
}
