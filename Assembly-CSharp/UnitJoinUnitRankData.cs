using System;
using Mono.Data.Sqlite;

public class UnitJoinUnitRankData : global::DataCore
{
	public global::UnitId UnitId { get; private set; }

	public global::UnitRankId UnitRankId { get; private set; }

	public int Physical { get; private set; }

	public int Mental { get; private set; }

	public int Martial { get; private set; }

	public int Skill { get; private set; }

	public int Spell { get; private set; }

	public int Strategy { get; private set; }

	public int Offense { get; private set; }

	public bool Mutation { get; private set; }

	public int Wound { get; private set; }

	public global::EnchantmentId EnchantmentId { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.UnitId = (global::UnitId)reader.GetInt32(0);
		this.UnitRankId = (global::UnitRankId)reader.GetInt32(1);
		this.Physical = reader.GetInt32(2);
		this.Mental = reader.GetInt32(3);
		this.Martial = reader.GetInt32(4);
		this.Skill = reader.GetInt32(5);
		this.Spell = reader.GetInt32(6);
		this.Strategy = reader.GetInt32(7);
		this.Offense = reader.GetInt32(8);
		this.Mutation = (reader.GetInt32(9) != 0);
		this.Wound = reader.GetInt32(10);
		this.EnchantmentId = (global::EnchantmentId)reader.GetInt32(11);
	}
}
