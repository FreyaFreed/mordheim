using System;
using Mono.Data.Sqlite;

public class BoneData : global::DataCore
{
	public global::BoneId Id { get; private set; }

	public string Name { get; private set; }

	public global::BoneId BoneIdMirror { get; private set; }

	public bool IsRange { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = (global::BoneId)reader.GetInt32(0);
		this.Name = reader.GetString(1);
		this.BoneIdMirror = (global::BoneId)reader.GetInt32(2);
		this.IsRange = (reader.GetInt32(3) != 0);
	}
}
