using System;
using Mono.Data.Sqlite;

public class WarbandRankSlotData : global::DataCore
{
	public int Id { get; private set; }

	public global::WarbandRankId WarbandRankId { get; private set; }

	public int Leader { get; private set; }

	public int Impressive { get; private set; }

	public int Hero { get; private set; }

	public int Henchman { get; private set; }

	public int Reserve { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = reader.GetInt32(0);
		this.WarbandRankId = (global::WarbandRankId)reader.GetInt32(1);
		this.Leader = reader.GetInt32(2);
		this.Impressive = reader.GetInt32(3);
		this.Hero = reader.GetInt32(4);
		this.Henchman = reader.GetInt32(5);
		this.Reserve = reader.GetInt32(6);
	}
}
