using System;
using Mono.Data.Sqlite;

public class UnitBaseData : global::DataCore
{
	public global::UnitBaseId Id { get; private set; }

	public string Name { get; private set; }

	public global::UnitRigId UnitRigId { get; private set; }

	public string CamBase { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = (global::UnitBaseId)reader.GetInt32(0);
		this.Name = reader.GetString(1);
		this.UnitRigId = (global::UnitRigId)reader.GetInt32(2);
		this.CamBase = reader.GetString(3);
	}
}
