using System;
using Mono.Data.Sqlite;

public class UnitTypeAttributeData : global::DataCore
{
	public int Id { get; private set; }

	public global::UnitTypeId UnitTypeId { get; private set; }

	public global::AttributeId AttributeId { get; private set; }

	public int Max { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = reader.GetInt32(0);
		this.UnitTypeId = (global::UnitTypeId)reader.GetInt32(1);
		this.AttributeId = (global::AttributeId)reader.GetInt32(2);
		this.Max = reader.GetInt32(3);
	}
}
