using System;
using Mono.Data.Sqlite;

public class WarbandData : global::DataCore
{
	public global::WarbandId Id { get; private set; }

	public string Name { get; private set; }

	public string Asset { get; private set; }

	public bool Basic { get; private set; }

	public int MinUnitCount { get; private set; }

	public int MaxUnitCount { get; private set; }

	public global::UnitId RequiredUnitId { get; private set; }

	public global::AllegianceId AllegianceId { get; private set; }

	public string Wagon { get; private set; }

	public string Chest { get; private set; }

	public global::ItemId ItemIdIdol { get; private set; }

	public global::UnitId UnitIdDramatis { get; private set; }

	public global::ColorPresetId ColorPresetId { get; private set; }

	public string DefaultName { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = (global::WarbandId)reader.GetInt32(0);
		this.Name = reader.GetString(1);
		this.Asset = reader.GetString(2);
		this.Basic = (reader.GetInt32(3) != 0);
		this.MinUnitCount = reader.GetInt32(4);
		this.MaxUnitCount = reader.GetInt32(5);
		this.RequiredUnitId = (global::UnitId)reader.GetInt32(6);
		this.AllegianceId = (global::AllegianceId)reader.GetInt32(7);
		this.Wagon = reader.GetString(8);
		this.Chest = reader.GetString(9);
		this.ItemIdIdol = (global::ItemId)reader.GetInt32(10);
		this.UnitIdDramatis = (global::UnitId)reader.GetInt32(11);
		this.ColorPresetId = (global::ColorPresetId)reader.GetInt32(12);
		this.DefaultName = reader.GetString(13);
	}
}
