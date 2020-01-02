using System;
using Mono.Data.Sqlite;

public class WyrdstoneShipmentData : global::DataCore
{
	public global::WyrdstoneShipmentId Id { get; private set; }

	public string Name { get; private set; }

	public global::WarbandRankId WarbandRankId { get; private set; }

	public int MinWeight { get; private set; }

	public int MaxWeight { get; private set; }

	public int MinDays { get; private set; }

	public int MaxDays { get; private set; }

	public int NextMinDays { get; private set; }

	public int NextMaxDays { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = (global::WyrdstoneShipmentId)reader.GetInt32(0);
		this.Name = reader.GetString(1);
		this.WarbandRankId = (global::WarbandRankId)reader.GetInt32(2);
		this.MinWeight = reader.GetInt32(3);
		this.MaxWeight = reader.GetInt32(4);
		this.MinDays = reader.GetInt32(5);
		this.MaxDays = reader.GetInt32(6);
		this.NextMinDays = reader.GetInt32(7);
		this.NextMaxDays = reader.GetInt32(8);
	}
}
