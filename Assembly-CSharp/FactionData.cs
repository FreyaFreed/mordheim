using System;
using Mono.Data.Sqlite;

public class FactionData : global::DataCore
{
	public global::FactionId Id { get; private set; }

	public string Name { get; private set; }

	public string Desc { get; private set; }

	public global::AllegianceId AllegianceId { get; private set; }

	public global::WarbandId WarbandId { get; private set; }

	public bool Primary { get; private set; }

	public int WyrdstonePriceBonusPercPerOtherFactionRank { get; private set; }

	public int WyrdstonePriceBonusPercPerRank { get; private set; }

	public int MinWydstonePriceModifier { get; private set; }

	public int RepBonusPercPerOtherFactionRank { get; private set; }

	public int RepBonusPercPerRank { get; private set; }

	public int MinDeliveryDays { get; private set; }

	public int MaxDeliveryDays { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = (global::FactionId)reader.GetInt32(0);
		this.Name = reader.GetString(1);
		this.Desc = reader.GetString(2);
		this.AllegianceId = (global::AllegianceId)reader.GetInt32(3);
		this.WarbandId = (global::WarbandId)reader.GetInt32(4);
		this.Primary = (reader.GetInt32(5) != 0);
		this.WyrdstonePriceBonusPercPerOtherFactionRank = reader.GetInt32(6);
		this.WyrdstonePriceBonusPercPerRank = reader.GetInt32(7);
		this.MinWydstonePriceModifier = reader.GetInt32(8);
		this.RepBonusPercPerOtherFactionRank = reader.GetInt32(9);
		this.RepBonusPercPerRank = reader.GetInt32(10);
		this.MinDeliveryDays = reader.GetInt32(11);
		this.MaxDeliveryDays = reader.GetInt32(12);
	}
}
