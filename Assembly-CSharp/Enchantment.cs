using System;
using System.Collections.Generic;
using Prometheus;
using UnityEngine;

public class Enchantment
{
	public Enchantment(global::EnchantmentId id, global::Unit target, global::Unit provider, bool orig, bool innate, global::AllegianceId runeAllegianceId = global::AllegianceId.NONE, bool spawnFx = true)
	{
		this.Data = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::EnchantmentData>((int)id);
		this.Provider = provider;
		this.Duration = this.Data.Duration;
		this.Innate = innate;
		this.unitTarget = target;
		this.AllegianceId = runeAllegianceId;
		this.original = orig;
		this.guid = 0U;
		if (global::PandoraSingleton<global::MissionManager>.Exists())
		{
			this.guid = global::PandoraSingleton<global::MissionManager>.Instance.GetNextRTGUID();
		}
		string id2 = ((int)this.Id).ToConstantString();
		this.AttributeModifiers = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::EnchantmentJoinAttributeData>("fk_enchantment_id", id2);
		this.CostModifiers = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::EnchantmentCostModifierData>("fk_enchantment_id", id2);
		this.DamageModifiers = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::EnchantmentDamageModifierData>("fk_enchantment_id", id2);
		this.ActionBlockers = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::EnchantmentBlockUnitActionData>("fk_enchantment_id", id2);
		this.BoneBlockers = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::EnchantmentBlockBoneData>("fk_enchantment_id", id2);
		this.ItemTypeBlockers = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::EnchantmentBlockItemTypeData>("fk_enchantment_id", id2);
		this.Effects = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::EnchantmentEffectEnchantmentData>("fk_enchantment_id", id2);
		this.Immunities = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::EnchantmentRemoveEnchantmentTypeData>("fk_enchantment_id", id2);
		this.Remover = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::EnchantmentRemoveEnchantmentData>("fk_enchantment_id", id2);
		this.InjuryModifiers = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::EnchantmentInjuryModifierData>("fk_enchantment_id", id2);
		this.CurseModifiers = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::EnchantmentCurseModifierData>("fk_enchantment_id", id2);
		this.fxsData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::EnchantmentFxData>("fk_enchantment_id", id2);
		this.fxs = new global::System.Collections.Generic.List<global::UnityEngine.GameObject>();
		if (this.HasFx && spawnFx)
		{
			this.SpawnFx(null);
		}
		this.LabelName = "enchant_title_";
		string str = string.Empty;
		if (runeAllegianceId != global::AllegianceId.NONE)
		{
			str = ((runeAllegianceId != global::AllegianceId.ORDER) ? "mark_" : "rune_") + this.Data.Name;
		}
		else
		{
			str = this.Data.Name;
		}
		this.LabelName += str;
		if (!global::PandoraSingleton<global::LocalizationManager>.Instance.HasStringId(this.LabelName))
		{
			this.LabelName = "skill_name_";
			this.LabelName += str;
		}
	}

	public global::EnchantmentId Id
	{
		get
		{
			return this.Data.Id;
		}
	}

	public global::EnchantmentData Data { get; private set; }

	public int Duration { get; set; }

	public global::Unit Provider { get; private set; }

	public bool Innate { get; private set; }

	public global::EnchantmentId EnchantmentIdOnTurnStart
	{
		get
		{
			return this.Data.EnchantmentIdOnTurnStart;
		}
	}

	public global::System.Collections.Generic.List<global::EnchantmentJoinAttributeData> AttributeModifiers { get; private set; }

	public global::System.Collections.Generic.List<global::EnchantmentInjuryModifierData> InjuryModifiers { get; private set; }

	public global::System.Collections.Generic.List<global::EnchantmentCostModifierData> CostModifiers { get; private set; }

	public global::System.Collections.Generic.List<global::EnchantmentCurseModifierData> CurseModifiers { get; private set; }

	public global::System.Collections.Generic.List<global::EnchantmentDamageModifierData> DamageModifiers { get; private set; }

	public global::System.Collections.Generic.List<global::EnchantmentBlockUnitActionData> ActionBlockers { get; private set; }

	public global::System.Collections.Generic.List<global::EnchantmentBlockBoneData> BoneBlockers { get; private set; }

	public global::System.Collections.Generic.List<global::EnchantmentBlockItemTypeData> ItemTypeBlockers { get; private set; }

	public global::System.Collections.Generic.List<global::EnchantmentEffectEnchantmentData> Effects { get; private set; }

	public global::System.Collections.Generic.List<global::EnchantmentRemoveEnchantmentTypeData> Immunities { get; private set; }

	public global::System.Collections.Generic.List<global::EnchantmentRemoveEnchantmentData> Remover { get; private set; }

	public string LabelName { get; private set; }

	public string LocalizedName
	{
		get
		{
			return global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(this.LabelName);
		}
	}

	public string LocalizedDescription
	{
		get
		{
			return global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("enchant_desc_" + this.Data.Name);
		}
	}

	public bool HasFx
	{
		get
		{
			return this.fxsData != null && this.fxsData.Count > 0;
		}
	}

	public global::AllegianceId AllegianceId { get; private set; }

	public bool UpdateDuration(global::Unit currentUnit)
	{
		if (this.Duration > 0 && this.Provider == currentUnit && --this.Duration == 0)
		{
			this.RemoveFx();
			return true;
		}
		return false;
	}

	public bool UpdateStatus(global::UnitStateId unitStateId)
	{
		if (this.Data.RequireUnitState && unitStateId != this.Data.UnitStateIdRequired)
		{
			this.RemoveFx();
			return true;
		}
		return false;
	}

	public bool UpdateValidNextAction()
	{
		if (!this.Data.ValidNextAction)
		{
			return false;
		}
		this.actionCount++;
		return this.actionCount > 1;
	}

	public void RemoveFx()
	{
		if (this.fxs != null)
		{
			for (int i = 0; i < this.fxs.Count; i++)
			{
				if (this.fxs[i] != null)
				{
					global::UnityEngine.Object.Destroy(this.fxs[i]);
				}
			}
			this.fxs = null;
		}
	}

	public bool HasEffectAttribute(global::AttributeId attrId)
	{
		int num = 0;
		for (int i = 0; i < this.Effects.Count; i++)
		{
			if (this.Effects[i].AttributeIdRoll == attrId)
			{
				num++;
			}
		}
		return num > 0;
	}

	public void SpawnFx(global::Unit target = null)
	{
		if (this.unitTarget == null && target != null)
		{
			this.unitTarget = target;
		}
		if (this.fxSpawned)
		{
			return;
		}
		if (this.fxsData != null)
		{
			for (int i = 0; i < this.fxsData.Count; i++)
			{
				global::PandoraSingleton<global::Prometheus.Prometheus>.Instance.SpawnFx(this.fxsData[i].Fx, this.unitTarget, delegate(global::UnityEngine.GameObject fx)
				{
					if (fx != null)
					{
						this.fxSpawned = true;
						if (this.fxs != null)
						{
							this.fxs.Add(fx);
						}
						else
						{
							global::UnityEngine.Object.Destroy(fx);
						}
					}
				});
			}
		}
	}

	private int actionCount;

	public bool damageApplied;

	public bool fxSpawned;

	private global::System.Collections.Generic.List<global::UnityEngine.GameObject> fxs;

	private global::System.Collections.Generic.List<global::EnchantmentFxData> fxsData;

	private global::Unit unitTarget;

	public bool original;

	public uint guid;
}
