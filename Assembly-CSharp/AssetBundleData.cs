using System;
using Mono.Data.Sqlite;

public class AssetBundleData : global::DataCore
{
	public global::AssetBundleId Id { get; private set; }

	public string Name { get; private set; }

	public string Mask { get; private set; }

	public string Format { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = (global::AssetBundleId)reader.GetInt32(0);
		this.Name = reader.GetString(1);
		this.Mask = reader.GetString(2);
		this.Format = reader.GetString(3);
	}
}
