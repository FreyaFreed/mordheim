using System;
using Mono.Data.Sqlite;

public class PlayerRankData : global::DataCore
{
	public global::PlayerRankId Id { get; private set; }

	public string Name { get; private set; }

	public int NewGameGold { get; private set; }

	public int XpNeeded { get; private set; }

	public global::WarbandSkillId WarbandSkillId { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = (global::PlayerRankId)reader.GetInt32(0);
		this.Name = reader.GetString(1);
		this.NewGameGold = reader.GetInt32(2);
		this.XpNeeded = reader.GetInt32(3);
		this.WarbandSkillId = (global::WarbandSkillId)reader.GetInt32(4);
	}
}
