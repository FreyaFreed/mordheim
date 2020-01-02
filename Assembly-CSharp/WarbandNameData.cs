using System;
using Mono.Data.Sqlite;

public class WarbandNameData : global::DataCore
{
	public global::WarbandNameId Id { get; private set; }

	public string Name { get; private set; }

	public global::WarbandId WarbandId { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = (global::WarbandNameId)reader.GetInt32(0);
		this.Name = reader.GetString(1);
		this.WarbandId = (global::WarbandId)reader.GetInt32(2);
	}
}
