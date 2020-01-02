using System;
using Mono.Data.Sqlite;

public class UnitNameData : global::DataCore
{
	public int Id { get; private set; }

	public string TheName { get; private set; }

	public bool Surname { get; private set; }

	public global::WarbandId WarbandId { get; private set; }

	public global::UnitId UnitId { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = reader.GetInt32(0);
		this.TheName = reader.GetString(1);
		this.Surname = (reader.GetInt32(2) != 0);
		this.WarbandId = (global::WarbandId)reader.GetInt32(3);
		this.UnitId = (global::UnitId)reader.GetInt32(4);
	}
}
