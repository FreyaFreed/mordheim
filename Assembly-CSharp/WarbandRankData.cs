using System;
using Mono.Data.Sqlite;

public class WarbandRankData : global::DataCore
{
	public global::WarbandRankId Id { get; private set; }

	public string Name { get; private set; }

	public int Rank { get; private set; }

	public int Exp { get; private set; }

	public int CartSize { get; private set; }

	public int Moral { get; private set; }

	public int Rating { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = (global::WarbandRankId)reader.GetInt32(0);
		this.Name = reader.GetString(1);
		this.Rank = reader.GetInt32(2);
		this.Exp = reader.GetInt32(3);
		this.CartSize = reader.GetInt32(4);
		this.Moral = reader.GetInt32(5);
		this.Rating = reader.GetInt32(6);
	}
}
