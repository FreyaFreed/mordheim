using System;
using Mono.Data.Sqlite;

public class AssetBundleFolderData : global::DataCore
{
	public int Id { get; private set; }

	public global::AssetBundleId AssetBundleId { get; private set; }

	public string Folder { get; private set; }

	public global::AssetExtensionId AssetExtensionId { get; private set; }

	public bool IncludeSubFolders { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = reader.GetInt32(0);
		this.AssetBundleId = (global::AssetBundleId)reader.GetInt32(1);
		this.Folder = reader.GetString(2);
		this.AssetExtensionId = (global::AssetExtensionId)reader.GetInt32(3);
		this.IncludeSubFolders = (reader.GetInt32(4) != 0);
	}
}
