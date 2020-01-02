using System;
using Mono.Data.Sqlite;

public class InjuryJoinAttributeData : global::DataCore
{
	public global::InjuryId InjuryId { get; private set; }

	public global::AttributeId AttributeId { get; private set; }

	public int Modifier { get; private set; }

	public int Limit { get; private set; }

	public bool Retire { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.InjuryId = (global::InjuryId)reader.GetInt32(0);
		this.AttributeId = (global::AttributeId)reader.GetInt32(1);
		this.Modifier = reader.GetInt32(2);
		this.Limit = reader.GetInt32(3);
		this.Retire = (reader.GetInt32(4) != 0);
	}
}
