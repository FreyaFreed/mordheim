using System;
using Mono.Data.Sqlite;

public class RuneMarkRecipeData : global::DataCore
{
	public int Id { get; private set; }

	public global::ItemId ItemId { get; private set; }

	public global::RuneMarkId RuneMarkId { get; private set; }

	public global::RuneMarkQualityId RuneMarkQualityId { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = reader.GetInt32(0);
		this.ItemId = (global::ItemId)reader.GetInt32(1);
		this.RuneMarkId = (global::RuneMarkId)reader.GetInt32(2);
		this.RuneMarkQualityId = (global::RuneMarkQualityId)reader.GetInt32(3);
	}
}
