using System;
using System.Collections.Generic;
using Mono.Data.Sqlite;
using UnityEngine;

public class CostOfLosingData : global::DataCore
{
	public int Id { get; private set; }

	public int Ratio { get; private set; }

	public bool MainWeapons { get; private set; }

	public bool SecondaryWeapons { get; private set; }

	public bool Armor { get; private set; }

	public bool Helmet { get; private set; }

	public bool OpenWound { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = reader.GetInt32(0);
		this.Ratio = reader.GetInt32(1);
		this.MainWeapons = (reader.GetInt32(2) != 0);
		this.SecondaryWeapons = (reader.GetInt32(3) != 0);
		this.Armor = (reader.GetInt32(4) != 0);
		this.Helmet = (reader.GetInt32(5) != 0);
		this.OpenWound = (reader.GetInt32(6) != 0);
	}

	public static global::CostOfLosingData GetRandomRatio(global::System.Collections.Generic.List<global::CostOfLosingData> datas, global::Tyche tyche, global::System.Collections.Generic.Dictionary<int, int> modifiers = null)
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
