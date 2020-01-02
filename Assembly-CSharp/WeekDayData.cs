using System;
using Mono.Data.Sqlite;

public class WeekDayData : global::DataCore
{
	public global::WeekDayId Id { get; private set; }

	public string Name { get; private set; }

	public bool RefreshMarket { get; private set; }

	public bool RefreshOutsiders { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = (global::WeekDayId)reader.GetInt32(0);
		this.Name = reader.GetString(1);
		this.RefreshMarket = (reader.GetInt32(2) != 0);
		this.RefreshOutsiders = (reader.GetInt32(3) != 0);
	}
}
