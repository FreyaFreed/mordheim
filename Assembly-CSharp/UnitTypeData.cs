using System;
using Mono.Data.Sqlite;

public class UnitTypeData : global::DataCore
{
	public global::UnitTypeId Id { get; private set; }

	public string Name { get; private set; }

	public int StartSp { get; private set; }

	public int MaxSp { get; private set; }

	public int StartOp { get; private set; }

	public int MaxOp { get; private set; }

	public int InitiativeBonus { get; private set; }

	public int Rating { get; private set; }

	public int MoralImpact { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = (global::UnitTypeId)reader.GetInt32(0);
		this.Name = reader.GetString(1);
		this.StartSp = reader.GetInt32(2);
		this.MaxSp = reader.GetInt32(3);
		this.StartOp = reader.GetInt32(4);
		this.MaxOp = reader.GetInt32(5);
		this.InitiativeBonus = reader.GetInt32(6);
		this.Rating = reader.GetInt32(7);
		this.MoralImpact = reader.GetInt32(8);
	}
}
