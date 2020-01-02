using System;
using Mono.Data.Sqlite;

public class FactionRankWarbandSkillData : global::DataCore
{
	public int Id { get; private set; }

	public global::FactionId FactionId { get; private set; }

	public global::FactionRankId FactionRankId { get; private set; }

	public global::WarbandSkillId WarbandSkillId { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = reader.GetInt32(0);
		this.FactionId = (global::FactionId)reader.GetInt32(1);
		this.FactionRankId = (global::FactionRankId)reader.GetInt32(2);
		this.WarbandSkillId = (global::WarbandSkillId)reader.GetInt32(3);
	}
}
