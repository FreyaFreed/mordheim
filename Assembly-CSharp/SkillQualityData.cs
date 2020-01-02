using System;
using Mono.Data.Sqlite;

public class SkillQualityData : global::DataCore
{
	public global::SkillQualityId Id { get; private set; }

	public string Name { get; private set; }

	public int Rating { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = (global::SkillQualityId)reader.GetInt32(0);
		this.Name = reader.GetString(1);
		this.Rating = reader.GetInt32(2);
	}
}
