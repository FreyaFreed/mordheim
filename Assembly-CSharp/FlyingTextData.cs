﻿using System;
using Mono.Data.Sqlite;

public class FlyingTextData : global::DataCore
{
	public global::FlyingTextId Id { get; private set; }

	public string Name { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = (global::FlyingTextId)reader.GetInt32(0);
		this.Name = reader.GetString(1);
	}
}
