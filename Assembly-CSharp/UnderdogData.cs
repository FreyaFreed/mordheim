using System;
using Mono.Data.Sqlite;

public class UnderdogData : global::DataCore
{
	public int Diff { get; private set; }

	public int XpBonus { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Diff = reader.GetInt32(0);
		this.XpBonus = reader.GetInt32(1);
	}
}
