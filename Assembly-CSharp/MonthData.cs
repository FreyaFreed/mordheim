using System;
using Mono.Data.Sqlite;

public class MonthData : global::DataCore
{
	public global::MonthId Id { get; private set; }

	public string Name { get; private set; }

	public int NumDays { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = (global::MonthId)reader.GetInt32(0);
		this.Name = reader.GetString(1);
		this.NumDays = reader.GetInt32(2);
	}
}
