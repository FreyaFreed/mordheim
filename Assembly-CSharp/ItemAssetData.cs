using System;
using Mono.Data.Sqlite;

public class ItemAssetData : global::DataCore
{
	public int Id { get; private set; }

	public global::ItemId ItemId { get; private set; }

	public global::RaceId RaceId { get; private set; }

	public global::WarbandId WarbandId { get; private set; }

	public global::UnitId UnitId { get; private set; }

	public string Asset { get; private set; }

	public bool NoTrail { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = reader.GetInt32(0);
		this.ItemId = (global::ItemId)reader.GetInt32(1);
		this.RaceId = (global::RaceId)reader.GetInt32(2);
		this.WarbandId = (global::WarbandId)reader.GetInt32(3);
		this.UnitId = (global::UnitId)reader.GetInt32(4);
		this.Asset = reader.GetString(5);
		this.NoTrail = (reader.GetInt32(6) != 0);
	}
}
