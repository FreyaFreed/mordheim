using System;
using Mono.Data.Sqlite;

public class ShrineData : global::DataCore
{
	public global::ShrineId Id { get; private set; }

	public string Name { get; private set; }

	public global::AllegianceId AllegianceId { get; private set; }

	public global::WarbandId WarbandId { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = (global::ShrineId)reader.GetInt32(0);
		this.Name = reader.GetString(1);
		this.AllegianceId = (global::AllegianceId)reader.GetInt32(2);
		this.WarbandId = (global::WarbandId)reader.GetInt32(3);
	}
}
