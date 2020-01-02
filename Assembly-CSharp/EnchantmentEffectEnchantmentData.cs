using System;
using System.Collections.Generic;
using Mono.Data.Sqlite;
using UnityEngine;

public class EnchantmentEffectEnchantmentData : global::DataCore
{
	public int Id { get; private set; }

	public global::EnchantmentId EnchantmentId { get; private set; }

	public global::EnchantmentId EnchantmentIdEffect { get; private set; }

	public global::EnchantmentTriggerId EnchantmentTriggerId { get; private set; }

	public global::AttributeId AttributeIdRoll { get; private set; }

	public global::UnitActionId UnitActionIdTrigger { get; private set; }

	public global::SkillId SkillIdTrigger { get; private set; }

	public int Ratio { get; private set; }

	public bool Self { get; private set; }

	public bool TargetSelf { get; private set; }

	public bool TargetAlly { get; private set; }

	public bool TargetEnemy { get; private set; }

	public override void Populate(global::Mono.Data.Sqlite.SqliteDataReader reader)
	{
		this.Id = reader.GetInt32(0);
		this.EnchantmentId = (global::EnchantmentId)reader.GetInt32(1);
		this.EnchantmentIdEffect = (global::EnchantmentId)reader.GetInt32(2);
		this.EnchantmentTriggerId = (global::EnchantmentTriggerId)reader.GetInt32(3);
		this.AttributeIdRoll = (global::AttributeId)reader.GetInt32(4);
		this.UnitActionIdTrigger = (global::UnitActionId)reader.GetInt32(5);
		this.SkillIdTrigger = (global::SkillId)reader.GetInt32(6);
		this.Ratio = reader.GetInt32(7);
		this.Self = (reader.GetInt32(8) != 0);
		this.TargetSelf = (reader.GetInt32(9) != 0);
		this.TargetAlly = (reader.GetInt32(10) != 0);
		this.TargetEnemy = (reader.GetInt32(11) != 0);
	}

	public static global::EnchantmentEffectEnchantmentData GetRandomRatio(global::System.Collections.Generic.List<global::EnchantmentEffectEnchantmentData> datas, global::Tyche tyche, global::System.Collections.Generic.Dictionary<int, int> modifiers = null)
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
