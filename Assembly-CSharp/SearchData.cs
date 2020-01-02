using System;
using Mono.Data.Sqlite;

public class SearchData : global::DataCore
{
	public global::SearchId Id { get; private set; }

	public string Name { get; private set; }

	public bool Outdoor { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = (global::SearchId)reader.GetInt32(0);
		this.Name = reader.GetString(1);
		this.Outdoor = (reader.GetInt32(2) != 0);
	}
}
