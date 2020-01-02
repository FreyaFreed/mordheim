using System;
using Mono.Data.Sqlite;

public class UnitJoinAttributeData : global::DataCore
{
	public global::UnitId UnitId { get; private set; }

	public global::AttributeId AttributeId { get; private set; }

	public int BaseValue { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.UnitId = (global::UnitId)reader.GetInt32(0);
		this.AttributeId = (global::AttributeId)reader.GetInt32(1);
		this.BaseValue = reader.GetInt32(2);
	}
}
