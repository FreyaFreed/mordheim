using System;
using Mono.Data.Sqlite;

public class HolidayJoinMonthData : global::DataCore
{
	public global::HolidayId HolidayId { get; private set; }

	public global::MonthId MonthId { get; private set; }

	public int Day { get; private set; }

	public bool Intercalary { get; private set; }

	public global::MoonId MoonId { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.HolidayId = (global::HolidayId)reader.GetInt32(0);
		this.MonthId = (global::MonthId)reader.GetInt32(1);
		this.Day = reader.GetInt32(2);
		this.Intercalary = (reader.GetInt32(3) != 0);
		this.MoonId = (global::MoonId)reader.GetInt32(4);
	}
}
