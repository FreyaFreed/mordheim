using System;
using Mono.Data.Sqlite;

public class ConstantData : global::DataCore
{
	public global::ConstantId Id { get; private set; }

	public string Name { get; private set; }

	public string Value { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = (global::ConstantId)reader.GetInt32(0);
		this.Name = reader.GetString(1);
		this.Value = reader.GetString(2);
	}
}
