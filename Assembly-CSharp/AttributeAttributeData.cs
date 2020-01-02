using System;
using Mono.Data.Sqlite;

public class AttributeAttributeData : global::DataCore
{
	public int Id { get; private set; }

	public global::AttributeId AttributeId { get; private set; }

	public global::AttributeId AttributeIdBase { get; private set; }

	public int Modifier { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = reader.GetInt32(0);
		this.AttributeId = (global::AttributeId)reader.GetInt32(1);
		this.AttributeIdBase = (global::AttributeId)reader.GetInt32(2);
		this.Modifier = reader.GetInt32(3);
	}
}
