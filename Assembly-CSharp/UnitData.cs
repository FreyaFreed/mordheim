using System;
using Mono.Data.Sqlite;

public class UnitData : global::DataCore
{
	public global::UnitId Id { get; private set; }

	public string Name { get; private set; }

	public string Asset { get; private set; }

	public string AltAsset { get; private set; }

	public bool Base { get; private set; }

	public bool Released { get; private set; }

	public global::RaceId RaceId { get; private set; }

	public global::WarbandId WarbandId { get; private set; }

	public int MaxCount { get; private set; }

	public global::UnitTypeId UnitTypeId { get; private set; }

	public global::UnitSizeId UnitSizeId { get; private set; }

	public global::MovementId MovementId { get; private set; }

	public global::UnitWoundId UnitWoundId { get; private set; }

	public global::UnitBaseId UnitBaseId { get; private set; }

	public global::ItemId ItemIdTrophy { get; private set; }

	public global::AllegianceId AllegianceId { get; private set; }

	public global::UnitId UnitIdDeathSpawn { get; private set; }

	public int DeathSpawnCount { get; private set; }

	public global::ZoneAoeId ZoneAoeIdDeathSpawn { get; private set; }

	public string SkinColor { get; private set; }

	public string FirstName { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = (global::UnitId)reader.GetInt32(0);
		this.Name = reader.GetString(1);
		this.Asset = reader.GetString(2);
		this.AltAsset = reader.GetString(3);
		this.Base = (reader.GetInt32(4) != 0);
		this.Released = (reader.GetInt32(5) != 0);
		this.RaceId = (global::RaceId)reader.GetInt32(6);
		this.WarbandId = (global::WarbandId)reader.GetInt32(7);
		this.MaxCount = reader.GetInt32(8);
		this.UnitTypeId = (global::UnitTypeId)reader.GetInt32(9);
		this.UnitSizeId = (global::UnitSizeId)reader.GetInt32(10);
		this.MovementId = (global::MovementId)reader.GetInt32(11);
		this.UnitWoundId = (global::UnitWoundId)reader.GetInt32(12);
		this.UnitBaseId = (global::UnitBaseId)reader.GetInt32(13);
		this.ItemIdTrophy = (global::ItemId)reader.GetInt32(14);
		this.AllegianceId = (global::AllegianceId)reader.GetInt32(15);
		this.UnitIdDeathSpawn = (global::UnitId)reader.GetInt32(16);
		this.DeathSpawnCount = reader.GetInt32(17);
		this.ZoneAoeIdDeathSpawn = (global::ZoneAoeId)reader.GetInt32(18);
		this.SkinColor = reader.GetString(19);
		this.FirstName = reader.GetString(20);
	}
}
