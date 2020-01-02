using System;
using Mono.Data.Sqlite;

public class RuneMarkData : global::DataCore
{
	public global::RuneMarkId Id { get; private set; }

	public string Name { get; private set; }

	public bool Rune { get; private set; }

	public bool Mark { get; private set; }

	public int Cost { get; private set; }

	public bool Released { get; private set; }

	public bool Lootable { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = (global::RuneMarkId)reader.GetInt32(0);
		this.Name = reader.GetString(1);
		this.Rune = (reader.GetInt32(2) != 0);
		this.Mark = (reader.GetInt32(3) != 0);
		this.Cost = reader.GetInt32(4);
		this.Released = (reader.GetInt32(5) != 0);
		this.Lootable = (reader.GetInt32(6) != 0);
	}
}
