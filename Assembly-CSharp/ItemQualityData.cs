using System;
using Mono.Data.Sqlite;

public class ItemQualityData : global::DataCore
{
	public global::ItemQualityId Id { get; private set; }

	public string Name { get; private set; }

	public int EnchantSlots { get; private set; }

	public int PriceBuyModifier { get; private set; }

	public int PriceSoldModifier { get; private set; }

	public global::RuneMarkQualityId RuneMarkQualityIdMax { get; private set; }

	public string Color { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = (global::ItemQualityId)reader.GetInt32(0);
		this.Name = reader.GetString(1);
		this.EnchantSlots = reader.GetInt32(2);
		this.PriceBuyModifier = reader.GetInt32(3);
		this.PriceSoldModifier = reader.GetInt32(4);
		this.RuneMarkQualityIdMax = (global::RuneMarkQualityId)reader.GetInt32(5);
		this.Color = reader.GetString(6);
	}
}
