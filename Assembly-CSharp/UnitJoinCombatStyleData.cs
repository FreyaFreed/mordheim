using System;
using System.Collections.Generic;
using Mono.Data.Sqlite;
using UnityEngine;

public class UnitJoinCombatStyleData : global::DataCore
{
	public global::UnitId UnitId { get; private set; }

	public global::CombatStyleId CombatStyleId { get; private set; }

	public int Ratio { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.UnitId = (global::UnitId)reader.GetInt32(0);
		this.CombatStyleId = (global::CombatStyleId)reader.GetInt32(1);
		this.Ratio = reader.GetInt32(2);
	}

	public static global::UnitJoinCombatStyleData GetRandomRatio(global::System.Collections.Generic.List<global::UnitJoinCombatStyleData> datas, global::Tyche tyche, global::System.Collections.Generic.Dictionary<global::UnitId, int> modifiers = null)
	{
		int num = 0;
		global::System.Collections.Generic.List<int> list = new global::System.Collections.Generic.List<int>();
		for (int i = 0; i < datas.Count; i++)
		{
			int num2 = datas[i].Ratio;
			if (modifiers != null && modifiers.ContainsKey(datas[i].UnitId))
			{
				num2 = global::UnityEngine.Mathf.Clamp(num2 + modifiers[datas[i].UnitId], 0, int.MaxValue);
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
