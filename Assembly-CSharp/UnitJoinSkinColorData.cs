using System;
using Mono.Data.Sqlite;

public class UnitJoinSkinColorData : global::DataCore
{
	public global::UnitId UnitId { get; private set; }

	public global::SkinColorId SkinColorId { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.UnitId = (global::UnitId)reader.GetInt32(0);
		this.SkinColorId = (global::SkinColorId)reader.GetInt32(1);
	}
}
