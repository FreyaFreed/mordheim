﻿using System;
using System.Collections.Generic;
using Mono.Data.Sqlite;
using UnityEngine;

public class SearchDensityLootData : global::DataCore
{
	public int Id { get; private set; }

	public global::SearchDensityId SearchDensityId { get; private set; }

	public global::SearchRewardId SearchRewardId { get; private set; }

	public int WarbandRank { get; private set; }

	public int ItemMin { get; private set; }

	public int ItemMax { get; private set; }

	public int Ratio { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = reader.GetInt32(0);
		this.SearchDensityId = (global::SearchDensityId)reader.GetInt32(1);
		this.SearchRewardId = (global::SearchRewardId)reader.GetInt32(2);
		this.WarbandRank = reader.GetInt32(3);
		this.ItemMin = reader.GetInt32(4);
		this.ItemMax = reader.GetInt32(5);
		this.Ratio = reader.GetInt32(6);
	}

	public static global::SearchDensityLootData GetRandomRatio(global::System.Collections.Generic.List<global::SearchDensityLootData> datas, global::Tyche tyche, global::System.Collections.Generic.Dictionary<int, int> modifiers = null)
	{
		int num = 0;
		global::System.Collections.Generic.List<int> list = new global::System.Collections.Generic.List<int>();
		for (int i = 0; i < datas.Count; i++)
		{
			int num2 = datas[i].Ratio;
			if (modifiers != null && modifiers.ContainsKey(datas[i].Id))
			{
				num2 = global::UnityEngine.Mathf.Clamp(num2 + modifiers[datas[i].Id], 0, int.MaxValue);
			}
			num += num2;
			list.Add(num);
		}
		int num3 = tyche.Rand(0, num);
		for (int j = 0; j < list.Count; j++)
		{
			if (num3 < list[j])
			{
				return datas[j];
			}
		}
		return null;
	}
}
