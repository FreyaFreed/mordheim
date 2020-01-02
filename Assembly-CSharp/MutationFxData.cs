using System;
using Mono.Data.Sqlite;

public class MutationFxData : global::DataCore
{
	public int Id { get; private set; }

	public global::MutationId MutationId { get; private set; }

	public global::UnitId UnitId { get; private set; }

	public string Asset { get; private set; }

	public string Trail { get; private set; }

	public global::BoneId BoneIdTrail { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = reader.GetInt32(0);
		this.MutationId = (global::MutationId)reader.GetInt32(1);
		this.UnitId = (global::UnitId)reader.GetInt32(2);
		this.Asset = reader.GetString(3);
		this.Trail = reader.GetString(4);
		this.BoneIdTrail = (global::BoneId)reader.GetInt32(5);
	}
}
