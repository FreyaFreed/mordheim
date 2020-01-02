using System;
using Mono.Data.Sqlite;

public class UnitCostData : global::DataCore
{
	public int Id { get; private set; }

	public global::UnitTypeId UnitTypeId { get; private set; }

	public int Rank { get; private set; }

	public int DecisiveVictory { get; private set; }

	public int PartialVictory { get; private set; }

	public int Defeat { get; private set; }

	public int Treatment { get; private set; }

	public int Hiring { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = reader.GetInt32(0);
		this.UnitTypeId = (global::UnitTypeId)reader.GetInt32(1);
		this.Rank = reader.GetInt32(2);
		this.DecisiveVictory = reader.GetInt32(3);
		this.PartialVictory = reader.GetInt32(4);
		this.Defeat = reader.GetInt32(5);
		this.Treatment = reader.GetInt32(6);
		this.Hiring = reader.GetInt32(7);
	}
}
