using System;
using System.Collections.Generic;
using Mono.Data.Sqlite;
using UnityEngine;

public class ProcMissionRatingData : global::DataCore
{
	public global::ProcMissionRatingId Id { get; private set; }

	public string Name { get; private set; }

	public int Ratio { get; private set; }

	public int MinValue { get; private set; }

	public int MaxValue { get; private set; }

	public int ProcMinValue { get; private set; }

	public int ProcMaxValue { get; private set; }

	public int RewardSearchPerc { get; private set; }

	public int RewardWyrdstonePerc { get; private set; }

	public global::EnchantmentId EnchantmentId { get; private set; }

	public int UnderdogXp { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = (global::ProcMissionRatingId)reader.GetInt32(0);
		this.Name = reader.GetString(1);
		this.Ratio = reader.GetInt32(2);
		this.MinValue = reader.GetInt32(3);
		this.MaxValue = reader.GetInt32(4);
		this.ProcMinValue = reader.GetInt32(5);
		this.ProcMaxValue = reader.GetInt32(6);
		this.RewardSearchPerc = reader.GetInt32(7);
		this.RewardWyrdstonePerc = reader.GetInt32(8);
		this.EnchantmentId = (global::EnchantmentId)reader.GetInt32(9);
		this.UnderdogXp = reader.GetInt32(10);
	}

	public static global::ProcMissionRatingData GetRandomRatio(global::System.Collections.Generic.List<global::ProcMissionRatingData> datas, global::Tyche tyche, global::System.Collections.Generic.Dictionary<global::ProcMissionRatingId, int> modifiers = null)
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
