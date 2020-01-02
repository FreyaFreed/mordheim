using System;
using Mono.Data.Sqlite;

public class AiProfileData : global::DataCore
{
	public global::AiProfileId Id { get; private set; }

	public string Name { get; private set; }

	public global::AiUnitId AiUnitIdBase { get; private set; }

	public global::AiUnitId AiUnitIdAlternate { get; private set; }

	public global::AiUnitId AiUnitIdSearch { get; private set; }

	public global::AiUnitId AiUnitIdSkillSpellTarget { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = (global::AiProfileId)reader.GetInt32(0);
		this.Name = reader.GetString(1);
		this.AiUnitIdBase = (global::AiUnitId)reader.GetInt32(2);
		this.AiUnitIdAlternate = (global::AiUnitId)reader.GetInt32(3);
		this.AiUnitIdSearch = (global::AiUnitId)reader.GetInt32(4);
		this.AiUnitIdSkillSpellTarget = (global::AiUnitId)reader.GetInt32(5);
	}
}
