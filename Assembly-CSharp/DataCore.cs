using System;
using Mono.Data.Sqlite;

public abstract class DataCore
{
	public abstract void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader);
}
