using System;
using Mono.Data.Sqlite;

public class DistrictData : global::DataCore
{
	public global::DistrictId Id { get; private set; }

	public string Name { get; private set; }

	public int Slots { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = (global::DistrictId)reader.GetInt32(0);
		this.Name = reader.GetString(1);
		this.Slots = reader.GetInt32(2);
	}
}
