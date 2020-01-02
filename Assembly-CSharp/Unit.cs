using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Unit
{
	public Unit(global::UnitSave us)
	{
		this.UnitSave = us;
		this.Init();
	}

	public string Name
	{
		get
		{
			return this.UnitSave.stats.Name;
		}
	}

	public global::UnitId Id
	{
		get
		{
			return this.Data.Id;
		}
	}

	public global::RaceId RaceId
	{
		get
		{
			return this.Data.RaceId;
		}
	}

	public global::WarbandId WarbandId
	{
		get
		{
			return this.Data.WarbandId;
		}
	}

	public global::AllegianceId AllegianceId
	{
		get
		{
			return (this.WarData.AllegianceId == global::AllegianceId.NONE) ? this.Data.AllegianceId : this.WarData.AllegianceId;
		}
	}

	public global::UnitData Data { get; private set; }

	public global::UnitBaseData BaseData { get; private set; }

	public global::UnitSave UnitSave { get; private set; }

	public global::EventLogger Logger { get; private set; }

	public global::CampaignUnitData CampaignData { get; private set; }

	public bool NoLootBag
	{
		get
		{
			return this.CampaignData != null && this.CampaignData.NoLootBag;
		}
	}

	public global::WarbandData WarData { get; private set; }

	public bool Active
	{
		get
		{
			return this.GetActiveStatus() == global::UnitActiveStatusId.AVAILABLE && this.UnitSave.warbandSlotIndex < 12;
		}
	}

	public global::UnitStateId Status { get; set; }

	public global::UnitStateId PreviousStatus { get; set; }

	public string LocalizedType { get; set; }

	public string LocalizedName { get; set; }

	public string LocalizedDescription { get; set; }

	public bool IsLeader
	{
		get
		{
			return this.GetUnitTypeId() == global::UnitTypeId.LEADER;
		}
	}

	public bool IsActiveLeader
	{
		get
		{
			return this.IsLeader && this.UnitSave.warbandSlotIndex < 12;
		}
	}

	public bool IsImpressive
	{
		get
		{
			return this.GetUnitTypeId() == global::UnitTypeId.IMPRESSIVE;
		}
	}

	public bool IsMonster
	{
		get
		{
			return this.GetUnitTypeId() == global::UnitTypeId.MONSTER || this.Id == global::UnitId.CHAOS_OGRE;
		}
	}

	public bool IsSpellcaster
	{
		get
		{
			return this.UnitSave.spells.Count > 0;
		}
	}

	public int CurrentWound
	{
		get
		{
			return this.GetAttribute(global::AttributeId.CURRENT_WOUND);
		}
		set
		{
			this.SetAttribute(global::AttributeId.CURRENT_WOUND, global::UnityEngine.Mathf.Clamp(value, 0, this.GetAttribute(global::AttributeId.WOUND)));
		}
	}

	public global::UnitSlotId ActiveWeaponSlot
	{
		get
		{
			return this.activeWeaponSlot;
		}
		set
		{
			this.activeWeaponSlot = value;
			this.UpdateAttributes();
		}
	}

	public global::UnitSlotId InactiveWeaponSlot
	{
		get
		{
			return (this.ActiveWeaponSlot != global::UnitSlotId.SET1_MAINHAND) ? global::UnitSlotId.SET1_MAINHAND : global::UnitSlotId.SET2_MAINHAND;
		}
	}

	public global::System.Collections.Generic.List<global::Enchantment> Enchantments { get; private set; }

	public global::System.Collections.Generic.List<global::Item> Items
	{
		get
		{
			int num = 6 + this.BackpackCapacity;
			if (num != this.items.Count)
			{
				for (int i = this.items.Count; i < num; i++)
				{
					this.items.Add(new global::Item(global::ItemId.NONE, global::ItemQualityId.NORMAL));
				}
				for (int j = this.items.Count - 1; j >= num; j--)
				{
					this.items.RemoveAt(j);
				}
			}
			return this.items;
		}
		private set
		{
			this.items = value;
		}
	}

	public global::System.Collections.Generic.List<global::Item> ActiveItems { get; private set; }

	public global::System.Collections.Generic.List<global::Mutation> Mutations { get; private set; }

	public global::System.Collections.Generic.List<global::Injury> Injuries { get; private set; }

	public global::System.Collections.Generic.List<global::SkillData> ActiveSkills { get; private set; }

	public global::System.Collections.Generic.List<global::SkillData> PassiveSkills { get; private set; }

	public global::System.Collections.Generic.List<global::SkillData> ConsumableSkills { get; private set; }

	public global::System.Collections.Generic.List<global::SkillData> Spells { get; private set; }

	public global::SkillData ActiveSkill { get; private set; }

	public bool UnitAlwaysVisible { get; set; }

	public int UnspentPhysical
	{
		get
		{
			int num = 0;
			for (int i = 0; i < global::Unit.PhysicalAttributeIds.Length; i++)
			{
				num += this.GetTempAttribute(global::Unit.PhysicalAttributeIds[i]);
			}
			return this.totalPhysicalPoints - this.spentPhysicalPoints - num;
		}
	}

	public int UnspentMental
	{
		get
		{
			int num = 0;
			for (int i = 0; i < global::Unit.MentalAttributeIds.Length; i++)
			{
				num += this.GetTempAttribute(global::Unit.MentalAttributeIds[i]);
			}
			return this.totalMentalPoints - this.spentMentalPoints - num;
		}
	}

	public int UnspentMartial
	{
		get
		{
			int num = 0;
			for (int i = 0; i < global::Unit.MartialAttributeIds.Length; i++)
			{
				num += this.GetTempAttribute(global::Unit.MartialAttributeIds[i]);
			}
			return this.totalMartialPoints - this.spentMartialPoints - num;
		}
	}

	public int UnspentSkill
	{
		get
		{
			return this.totalSkillPoints - this.spentSkillPoints;
		}
	}

	public int UnspentSpell
	{
		get
		{
			return this.totalSpellPoints - this.spentSpellPoints;
		}
	}

	public int BackpackCapacity
	{
		get
		{
			return (this.cachedBackpackCapacity == -1) ? this.realBackPackCapacity : this.cachedBackpackCapacity;
		}
	}

	public bool HasEnchantmentsChanged { get; private set; }

	public int Movement
	{
		get
		{
			return this.GetAttribute(global::AttributeId.MOVEMENT);
		}
	}

	public int WeaponSkill
	{
		get
		{
			return this.GetAttribute(global::AttributeId.WEAPON_SKILL);
		}
	}

	public int BallisticSkill
	{
		get
		{
			return this.GetAttribute(global::AttributeId.BALLISTIC_SKILL);
		}
	}

	public int Strength
	{
		get
		{
			return this.GetAttribute(global::AttributeId.STRENGTH);
		}
	}

	public int Toughness
	{
		get
		{
			return this.GetAttribute(global::AttributeId.TOUGHNESS);
		}
	}

	public int Wound
	{
		get
		{
			return this.GetAttribute(global::AttributeId.WOUND);
		}
	}

	public int Agility
	{
		get
		{
			return this.GetAttribute(global::AttributeId.AGILITY);
		}
	}

	public int Leadership
	{
		get
		{
			return this.GetAttribute(global::AttributeId.LEADERSHIP);
		}
	}

	public int Moral
	{
		get
		{
			return this.GetAttribute(global::AttributeId.MORAL);
		}
	}

	public int MoralImpact
	{
		get
		{
			return global::UnityEngine.Mathf.Max(0, this.GetAttribute(global::AttributeId.MORAL_IMPACT));
		}
	}

	public int Alertness
	{
		get
		{
			return this.GetAttribute(global::AttributeId.ALERTNESS);
		}
	}

	public int Accuracy
	{
		get
		{
			return this.GetAttribute(global::AttributeId.ACCURACY);
		}
	}

	public int Intelligence
	{
		get
		{
			return this.GetAttribute(global::AttributeId.INTELLIGENCE);
		}
	}

	public int Initiative
	{
		get
		{
			return (this.Status == global::UnitStateId.OUT_OF_ACTION) ? 0 : this.GetAttribute(global::AttributeId.INITIATIVE);
		}
	}

	public int CurrentStrategyPoints
	{
		get
		{
			return this.GetAttribute(global::AttributeId.CURRENT_STRATEGY_POINTS);
		}
	}

	public int StrategyPoints
	{
		get
		{
			return this.GetAttribute(global::AttributeId.STRATEGY_POINTS);
		}
	}

	public int CurrentOffensePoints
	{
		get
		{
			return this.GetAttribute(global::AttributeId.CURRENT_OFFENSE_POINTS);
		}
	}

	public int OffensePoints
	{
		get
		{
			return this.GetAttribute(global::AttributeId.OFFENSE_POINTS);
		}
	}

	public int WoundPerc
	{
		get
		{
			return this.GetAttribute(global::AttributeId.WOUND_PERC);
		}
	}

	public int ArmorAbsorption
	{
		get
		{
			return this.GetAttribute(global::AttributeId.ARMOR_ABSORPTION);
		}
	}

	public int BypassArmor
	{
		get
		{
			return this.GetAttribute(global::AttributeId.BYPASS_ARMOR);
		}
	}

	public int ArmorAbsorptionPerc
	{
		get
		{
			return this.GetAttribute(global::AttributeId.ARMOR_ABSORPTION_PERC);
		}
	}

	public int ByPassArmorPerc
	{
		get
		{
			return this.GetAttribute(global::AttributeId.BYPASS_ARMOR_PERC);
		}
	}

	public int DamageBonus
	{
		get
		{
			return this.GetAttribute(global::AttributeId.DAMAGE_BONUS);
		}
	}

	public int DamageBonusMelee
	{
		get
		{
			return this.GetAttribute(global::AttributeId.DAMAGE_BONUS_MELEE);
		}
	}

	public int DamageBonusRange
	{
		get
		{
			return this.GetAttribute(global::AttributeId.DAMAGE_BONUS_RANGE);
		}
	}

	public int DamageBonusSpell
	{
		get
		{
			return this.GetAttribute(global::AttributeId.DAMAGE_BONUS_SPELL);
		}
	}

	public int DamageBonusMeleePerc
	{
		get
		{
			return this.GetAttribute(global::AttributeId.DAMAGE_BONUS_MELEE_PERC);
		}
	}

	public int DamageBonusRangePerc
	{
		get
		{
			return this.GetAttribute(global::AttributeId.DAMAGE_BONUS_RANGE_PERC);
		}
	}

	public int DamageBonusDivMagPerc
	{
		get
		{
			return this.GetAttribute(global::AttributeId.DAMAGE_BONUS_DIV_MAG_PERC);
		}
	}

	public int DamageBonusArcMagPerc
	{
		get
		{
			return this.GetAttribute(global::AttributeId.DAMAGE_BONUS_ARC_MAG_PERC);
		}
	}

	public int DamageBonusChargePerc
	{
		get
		{
			return this.GetAttribute(global::AttributeId.DAMAGE_BONUS_CHARGE_PERC);
		}
	}

	public int DamageMin
	{
		get
		{
			return this.GetAttribute(global::AttributeId.DAMAGE_MIN);
		}
	}

	public int DamageMax
	{
		get
		{
			return this.GetAttribute(global::AttributeId.DAMAGE_MAX);
		}
	}

	public int DamageCriticalBonus
	{
		get
		{
			return this.GetAttribute(global::AttributeId.DAMAGE_CRITICAL_BONUS);
		}
	}

	public int DamageCriticalBonusPerc
	{
		get
		{
			return this.GetAttribute(global::AttributeId.DAMAGE_CRITICAL_BONUS_PERC);
		}
	}

	public int DamageHoly
	{
		get
		{
			return this.GetAttribute(global::AttributeId.DAMAGE_HOLY);
		}
	}

	public int DamageUnholy
	{
		get
		{
			return this.GetAttribute(global::AttributeId.DAMAGE_UNHOLY);
		}
	}

	public int DamageBonusHolyPerc
	{
		get
		{
			return this.GetAttribute(global::AttributeId.DAMAGE_BONUS_HOLY_PERC);
		}
	}

	public int DamageBonusUnholyPerc
	{
		get
		{
			return this.GetAttribute(global::AttributeId.DAMAGE_BONUS_UNHOLY_PERC);
		}
	}

	public int GlobalRangeDamagePerc
	{
		get
		{
			return this.GetAttribute(global::AttributeId.GLOBAL_RANGE_DAMAGE_PERC);
		}
	}

	public int GlobalMeleeDamagePerc
	{
		get
		{
			return this.GetAttribute(global::AttributeId.GLOBAL_MELEE_DAMAGE_PERC);
		}
	}

	public int CritResistance
	{
		get
		{
			return this.GetAttribute(global::AttributeId.CRIT_RESISTANCE);
		}
	}

	public int MagicResistance
	{
		get
		{
			return this.GetAttribute(global::AttributeId.MAGIC_RESISTANCE);
		}
	}

	public int MagicResistDefenderModifier
	{
		get
		{
			return this.GetAttribute(global::AttributeId.MAGIC_RESIST_DEFENDER_MODIFIER);
		}
	}

	public int DodgeDefenderModifier
	{
		get
		{
			return this.GetAttribute(global::AttributeId.DODGE_DEFENDER_MODIFIER);
		}
	}

	public int ParryDefenderModifier
	{
		get
		{
			return this.GetAttribute(global::AttributeId.PARRY_DEFENDER_MODIFIER);
		}
	}

	public int MeleeResistance
	{
		get
		{
			return this.GetAttribute(global::AttributeId.MELEE_RESISTANCE);
		}
	}

	public int RangeResistance
	{
		get
		{
			return this.GetAttribute(global::AttributeId.RANGE_RESISTANCE);
		}
	}

	public int RangeResistanceDefenderModifier
	{
		get
		{
			return this.GetAttribute(global::AttributeId.RANGE_RESISTANCE_DEFENDER_MODIFIER);
		}
	}

	public int PoisonResistDefenderModifier
	{
		get
		{
			return this.GetAttribute(global::AttributeId.POISON_RESIST_DEFENDER_MODIFIER);
		}
	}

	public int Xp
	{
		get
		{
			return this.UnitSave.xp;
		}
	}

	public int Rank
	{
		get
		{
			return this.GetAttribute(global::AttributeId.RANK);
		}
	}

	public int ViewDistance
	{
		get
		{
			return this.GetAttribute((!this.isAI) ? global::AttributeId.VIEW_DISTANCE : global::AttributeId.AI_VIEW_DISTANCE);
		}
	}

	public int RangeBonusSpell
	{
		get
		{
			return this.GetAttribute(global::AttributeId.RANGE_BONUS_SPELL);
		}
	}

	public int ParryLeft
	{
		get
		{
			return this.GetAttribute(global::AttributeId.PARRY_LEFT);
		}
	}

	public int DodgeLeft
	{
		get
		{
			return this.GetAttribute(global::AttributeId.DODGE_LEFT);
		}
	}

	public int AmbushLeft
	{
		get
		{
			return this.GetAttribute(global::AttributeId.AMBUSH_LEFT);
		}
	}

	public int OverwatchLeft
	{
		get
		{
			return this.GetAttribute(global::AttributeId.OVERWATCH_LEFT);
		}
	}

	public int AttackPerAction
	{
		get
		{
			return this.GetAttribute(global::AttributeId.ATTACK_PER_ACTION);
		}
	}

	public int MaxAgility
	{
		get
		{
			return this.GetAttribute(global::AttributeId.AGILITY_MAX);
		}
	}

	public int MaxStrength
	{
		get
		{
			return this.GetAttribute(global::AttributeId.STRENGTH_MAX);
		}
	}

	public int MaxToughness
	{
		get
		{
			return this.GetAttribute(global::AttributeId.TOUGHNESS_MAX);
		}
	}

	public int MaxLeadership
	{
		get
		{
			return this.GetAttribute(global::AttributeId.LEADERSHIP_MAX);
		}
	}

	public int MaxIntelligence
	{
		get
		{
			return this.GetAttribute(global::AttributeId.INTELLIGENCE_MAX);
		}
	}

	public int MaxAlertness
	{
		get
		{
			return this.GetAttribute(global::AttributeId.ALERTNESS_MAX);
		}
	}

	public int MaxWeaponSkill
	{
		get
		{
			return this.GetAttribute(global::AttributeId.WEAPON_SKILL_MAX);
		}
	}

	public int MaxBallisticSkill
	{
		get
		{
			return this.GetAttribute(global::AttributeId.BALLISTIC_SKILL_MAX);
		}
	}

	public int MaxAccuracy
	{
		get
		{
			return this.GetAttribute(global::AttributeId.ACCURACY_MAX);
		}
	}

	public int CounterDisabled
	{
		get
		{
			return this.GetAttribute(global::AttributeId.COUNTER_DISABLED);
		}
	}

	public int CounterForced
	{
		get
		{
			return this.GetAttribute(global::AttributeId.COUNTER_FORCED);
		}
	}

	public int ChargeMovement
	{
		get
		{
			return this.GetAttribute(global::AttributeId.CHARGE_MOVEMENT);
		}
	}

	public int AmbushMovement
	{
		get
		{
			return this.GetAttribute(global::AttributeId.AMBUSH_MOVEMENT);
		}
	}

	public int DodgeBypass
	{
		get
		{
			return this.GetAttribute(global::AttributeId.DODGE_BYPASS);
		}
	}

	public int ParryBypass
	{
		get
		{
			return this.GetAttribute(global::AttributeId.PARRY_BYPASS);
		}
	}

	public int ClimbRoll3
	{
		get
		{
			return this.GetAttribute(global::AttributeId.CLIMB_ROLL_3);
		}
	}

	public int ClimbRoll6
	{
		get
		{
			return this.GetAttribute(global::AttributeId.CLIMB_ROLL_6);
		}
	}

	public int ClimbRoll9
	{
		get
		{
			return this.GetAttribute(global::AttributeId.CLIMB_ROLL_9);
		}
	}

	public int LeapRoll
	{
		get
		{
			return this.GetAttribute(global::AttributeId.LEAP_ROLL);
		}
	}

	public int JumpDownRoll3
	{
		get
		{
			return this.GetAttribute(global::AttributeId.JUMP_DOWN_ROLL_3);
		}
	}

	public int JumpDownRoll6
	{
		get
		{
			return this.GetAttribute(global::AttributeId.JUMP_DOWN_ROLL_6);
		}
	}

	public int JumpDownRoll9
	{
		get
		{
			return this.GetAttribute(global::AttributeId.JUMP_DOWN_ROLL_9);
		}
	}

	public int LockpickingRoll
	{
		get
		{
			return this.GetAttribute(global::AttributeId.LOCKPICKING_ROLL);
		}
	}

	public int PoisonResistRoll
	{
		get
		{
			return this.GetAttribute(global::AttributeId.POISON_RESIST_ROLL);
		}
	}

	public int TrapResistRoll
	{
		get
		{
			return this.GetAttribute(global::AttributeId.TRAP_RESIST_ROLL);
		}
	}

	public int LeadershipRoll
	{
		get
		{
			return this.GetAttribute(global::AttributeId.LEADERSHIP_ROLL);
		}
	}

	public int AllAloneRoll
	{
		get
		{
			return this.GetAttribute(global::AttributeId.ALL_ALONE_ROLL);
		}
	}

	public int FearRoll
	{
		get
		{
			return this.GetAttribute(global::AttributeId.FEAR_ROLL);
		}
	}

	public int TerrorRoll
	{
		get
		{
			return this.GetAttribute(global::AttributeId.TERROR_ROLL);
		}
	}

	public int WarbandRoutRoll
	{
		get
		{
			return this.GetAttribute(global::AttributeId.WARBAND_ROUT_ROLL);
		}
	}

	public int SpellcastingRoll
	{
		get
		{
			return this.GetAttribute(global::AttributeId.SPELLCASTING_ROLL);
		}
	}

	public int DivineSpellcastingRoll
	{
		get
		{
			return this.GetAttribute(global::AttributeId.DIVINE_SPELLCASTING_ROLL);
		}
	}

	public int ArcaneSpellcastingRoll
	{
		get
		{
			return this.GetAttribute(global::AttributeId.ARCANE_SPELLCASTING_ROLL);
		}
	}

	public int TzeentchsCurseRoll
	{
		get
		{
			return this.GetAttribute(global::AttributeId.TZEENTCHS_CURSE_ROLL);
		}
	}

	public int DivineWrathRoll
	{
		get
		{
			return this.GetAttribute(global::AttributeId.DIVINE_WRATH_ROLL);
		}
	}

	public int PerceptionRoll
	{
		get
		{
			return this.GetAttribute(global::AttributeId.PERCEPTION_ROLL);
		}
	}

	public int CombatMeleeHitRoll
	{
		get
		{
			return this.GetAttribute(global::AttributeId.COMBAT_MELEE_HIT_ROLL);
		}
	}

	public int CombatRangeHitRoll
	{
		get
		{
			return this.GetAttribute(global::AttributeId.COMBAT_RANGE_HIT_ROLL);
		}
	}

	public int CriticalMeleeAttemptRoll
	{
		get
		{
			return this.GetAttribute(global::AttributeId.CRITICAL_MELEE_ATTEMPT_ROLL);
		}
	}

	public int CriticalRangeAttemptRoll
	{
		get
		{
			return this.GetAttribute(global::AttributeId.CRITICAL_RANGE_ATTEMPT_ROLL);
		}
	}

	public int CriticalResultRoll
	{
		get
		{
			return this.GetAttribute(global::AttributeId.CRITICAL_RESULT_ROLL);
		}
	}

	public int StupidityRoll
	{
		get
		{
			return this.GetAttribute(global::AttributeId.STUPIDITY_ROLL);
		}
	}

	public int ParryingRoll
	{
		get
		{
			return this.GetAttribute(global::AttributeId.PARRYING_ROLL);
		}
	}

	public int DodgeRoll
	{
		get
		{
			return this.GetAttribute(global::AttributeId.DODGE_ROLL);
		}
	}

	public int InjuryRoll
	{
		get
		{
			return this.GetAttribute(global::AttributeId.INJURY_ROLL);
		}
	}

	public int WyrdstoneResistRoll
	{
		get
		{
			return this.GetAttribute(global::AttributeId.WYRDSTONE_RESIST_ROLL);
		}
	}

	public int StunResistRoll
	{
		get
		{
			return this.GetAttribute(global::AttributeId.STUN_RESIST_ROLL);
		}
	}

	public static global::Unit GenerateUnit(global::UnitId unitId, int rank)
	{
		float realtimeSinceStartup = global::UnityEngine.Time.realtimeSinceStartup;
		global::Unit unit = new global::Unit(new global::UnitSave(unitId));
		global::PandoraDebug.LogDebug("iNIT uNIT" + (global::UnityEngine.Time.realtimeSinceStartup - realtimeSinceStartup) + "s", "LOADING", null);
		if (rank > 0)
		{
			global::System.Collections.Generic.List<global::UnitJoinUnitRankData> advancements = new global::System.Collections.Generic.List<global::UnitJoinUnitRankData>();
			global::System.Collections.Generic.List<global::Mutation> newMutations = new global::System.Collections.Generic.List<global::Mutation>();
			global::System.Collections.Generic.List<global::Item> previousItems = new global::System.Collections.Generic.List<global::Item>();
			unit.AddXp(99999999, advancements, newMutations, previousItems, 0, rank);
			unit.UnitSave.xp = 0;
			unit.UnitSave.items.Clear();
			for (int i = 0; i < 13; i++)
			{
				unit.UnitSave.items.Add(null);
			}
			unit.InitItems();
		}
		global::PandoraDebug.LogDebug("iNIT uNIT gENERATE uNIT fINISHED" + (global::UnityEngine.Time.realtimeSinceStartup - realtimeSinceStartup) + "s", "LOADING", null);
		return unit;
	}

	public static global::Unit GenerateHireUnit(global::UnitId unitId, int warbandRank, int unitRank)
	{
		global::Unit unit = global::Unit.GenerateUnit(unitId, unitRank);
		global::System.Collections.Generic.List<global::Item> removedItems = new global::System.Collections.Generic.List<global::Item>();
		global::System.Collections.Generic.List<global::InjuryId> list = new global::System.Collections.Generic.List<global::InjuryId>(global::Unit.HIRE_UNIT_INJURY_EXCLUDES);
		global::HireUnitInjuryData randomRatio = global::HireUnitInjuryData.GetRandomRatio(global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::HireUnitInjuryData>("unit_rank", unit.Rank.ToConstantString()), global::PandoraSingleton<global::GameManager>.Instance.LocalTyche, null);
		for (int i = 0; i < randomRatio.Count; i++)
		{
			global::InjuryData injuryData = global::PandoraSingleton<global::HideoutManager>.Instance.Progressor.RollInjury(list, unit);
			if (injuryData == null)
			{
				break;
			}
			unit.AddInjury(injuryData, global::PandoraSingleton<global::HideoutManager>.Instance.Date.CurrentDate, removedItems, true, -1);
			list.Add(injuryData.Id);
		}
		global::System.Collections.Generic.List<global::HireUnitItemData> list2 = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::HireUnitItemData>("unit_rank", unit.Rank.ToConstantString());
		global::System.Collections.Generic.List<global::HireUnitItemQualityData> datas = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::HireUnitItemQualityData>("fk_warband_rank_id", warbandRank.ToConstantString());
		global::HireUnitItemRunemarkData hireUnitItemRunemarkData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::HireUnitItemRunemarkData>("fk_warband_rank_id", warbandRank.ToConstantString())[0];
		int num = 0;
		global::System.Collections.Generic.List<global::Item> list3 = new global::System.Collections.Generic.List<global::Item>();
		global::CombatStyleId excludedCombatStyleId = global::CombatStyleId.NONE;
		bool flag = false;
		bool flag2 = false;
		for (int j = 0; j < list2.Count; j++)
		{
			if (global::PandoraSingleton<global::GameManager>.Instance.LocalTyche.Rand(0, 100) < list2[j].Ratio)
			{
				switch (list2[j].UnitSlotId)
				{
				case global::UnitSlotId.HELMET:
					flag = true;
					break;
				case global::UnitSlotId.ARMOR:
					flag2 = true;
					break;
				case global::UnitSlotId.SET1_MAINHAND:
				case global::UnitSlotId.SET2_MAINHAND:
					excludedCombatStyleId = global::UnitFactory.AddCombatStyleSet(global::PandoraSingleton<global::GameManager>.Instance.LocalTyche, ref num, unit, list2[j].UnitSlotId, excludedCombatStyleId, global::HireUnitItemQualityData.GetRandomRatio(datas, global::PandoraSingleton<global::GameManager>.Instance.LocalTyche, null).ItemQualityId, global::PandoraSingleton<global::GameManager>.Instance.LocalTyche.Rand(0, 100) < hireUnitItemRunemarkData.Ratio, list3);
					break;
				case global::UnitSlotId.ITEM_1:
				case global::UnitSlotId.ITEM_2:
				case global::UnitSlotId.ITEM_3:
				case global::UnitSlotId.ITEM_4:
				case global::UnitSlotId.ITEM_5:
				case global::UnitSlotId.ITEM_6:
				case global::UnitSlotId.ITEM_7:
					if (!unit.BothArmsMutated() && list2[j].UnitSlotId < (global::UnitSlotId)unit.Items.Count)
					{
						global::Item procItem = global::UnitFactory.GetProcItem(global::PandoraSingleton<global::GameManager>.Instance.LocalTyche, ref num, unit, global::UnitSlotId.ITEM_1, global::ItemTypeId.CONSUMABLE_POTIONS, global::HireUnitItemQualityData.GetRandomRatio(datas, global::PandoraSingleton<global::GameManager>.Instance.LocalTyche, null).ItemQualityId, false, false);
						list3.Add(procItem);
						unit.EquipItem(list2[j].UnitSlotId, procItem, true);
					}
					break;
				}
			}
		}
		if (flag2 || flag)
		{
			global::UnitFactory.AddArmorStyleSet(global::PandoraSingleton<global::GameManager>.Instance.LocalTyche, ref num, unit, global::HireUnitItemQualityData.GetRandomRatio(datas, global::PandoraSingleton<global::GameManager>.Instance.LocalTyche, null).ItemQualityId, flag2, flag, flag2 && global::PandoraSingleton<global::GameManager>.Instance.LocalTyche.Rand(0, 100) < hireUnitItemRunemarkData.Ratio, flag && global::PandoraSingleton<global::GameManager>.Instance.LocalTyche.Rand(0, 100) < hireUnitItemRunemarkData.Ratio, list3);
		}
		unit.ResetBodyPart();
		return unit;
	}

	private void Init()
	{
		this.Enchantments = new global::System.Collections.Generic.List<global::Enchantment>();
		this.enchantTypeImmunities = new global::System.Collections.Generic.List<global::EnchantmentTypeId>();
		this.enchantTypeToBeRemoved = new global::System.Collections.Generic.List<global::EnchantmentTypeId>();
		this.enchantToBeRemoved = new global::System.Collections.Generic.List<global::EnchantmentId>();
		this.ActiveSkills = new global::System.Collections.Generic.List<global::SkillData>();
		this.PassiveSkills = new global::System.Collections.Generic.List<global::SkillData>();
		this.ConsumableSkills = new global::System.Collections.Generic.List<global::SkillData>();
		this.Spells = new global::System.Collections.Generic.List<global::SkillData>();
		this.Mutations = new global::System.Collections.Generic.List<global::Mutation>();
		this.Injuries = new global::System.Collections.Generic.List<global::Injury>();
		this.actionCostModifiers = new global::System.Collections.Generic.Dictionary<global::UnitActionId, global::CostModifier>(global::UnitActionIdComparer.Instance);
		this.skillCostModifiers = new global::System.Collections.Generic.Dictionary<global::SkillId, global::CostModifier>(global::SkillIdComparer.Instance);
		this.damagePercModifiers = new global::System.Collections.Generic.Dictionary<global::SkillId, int>(global::SkillIdComparer.Instance);
		this.spellTypeModifiers = new global::System.Collections.Generic.Dictionary<global::SpellTypeId, global::CostModifier>(global::SpellTypeIdComparer.Instance);
		this.blockedActions = new global::System.Collections.Generic.Dictionary<global::UnitActionId, global::EnchantmentId>(global::UnitActionIdComparer.Instance);
		this.blockedSkills = new global::System.Collections.Generic.Dictionary<global::SkillId, global::EnchantmentId>(global::SkillIdComparer.Instance);
		this.blockedBones = new global::System.Collections.Generic.Dictionary<global::BoneId, global::EnchantmentId>(global::BoneIdComparer.Instance);
		this.blockedItemTypes = new global::System.Collections.Generic.Dictionary<global::ItemTypeId, global::EnchantmentId>(global::ItemTypeIdComparer.Instance);
		this.attributes = new int[152];
		this.availableBodyParts = new global::System.Collections.Generic.List<global::BodyPartData>();
		this.bodyParts = new global::System.Collections.Generic.Dictionary<global::BodyPartId, global::BodyPart>(global::BodyPartIdComparer.Instance);
		this.tempAttributes = new int[152];
		this.maxAttributes = new global::System.Collections.Generic.Dictionary<int, global::AttributeId>();
		this.ActiveItems = new global::System.Collections.Generic.List<global::Item>();
		this.Logger = new global::EventLogger(this.UnitSave.stats.history);
		this.Data = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::UnitData>(this.UnitSave.stats.id);
		this.BaseData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::UnitBaseData>((int)this.Data.UnitBaseId);
		this.WarData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::WarbandData>((int)this.Data.WarbandId);
		global::UnitRankData unitRankData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::UnitRankData>((int)this.UnitSave.rankId);
		this.SetAttribute(global::AttributeId.RANK, unitRankData.Rank);
		this.SetMoralImpact();
		this.attributeDataList = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::AttributeData>();
		if (global::Unit.attributeDataById == null)
		{
			global::Unit.attributeDataById = new global::AttributeData[152];
			for (int i = 0; i < this.attributeDataList.Count; i++)
			{
				global::AttributeData attributeData = this.attributeDataList[i];
				global::Unit.attributeDataById[(int)attributeData.Id] = attributeData;
			}
			global::System.Collections.Generic.List<global::AttributeAttributeData> list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::AttributeAttributeData>();
			global::Unit.attributeAttributeDataById = new global::System.Collections.Generic.Dictionary<global::AttributeId, global::System.Collections.Generic.List<global::AttributeAttributeData>>(global::AttributeIdComparer.Instance);
			for (int j = 0; j < list.Count; j++)
			{
				global::AttributeAttributeData attributeAttributeData = list[j];
				global::System.Collections.Generic.List<global::AttributeAttributeData> list2;
				if (!global::Unit.attributeAttributeDataById.TryGetValue(attributeAttributeData.AttributeId, out list2))
				{
					list2 = new global::System.Collections.Generic.List<global::AttributeAttributeData>();
					global::Unit.attributeAttributeDataById[attributeAttributeData.AttributeId] = list2;
				}
				list2.Add(attributeAttributeData);
			}
		}
		this.baseAttributesData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::UnitJoinAttributeData>("fk_unit_id", ((int)this.Id).ToConstantString());
		this.ranksData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::UnitJoinUnitRankData>("fk_unit_id", ((int)this.Id).ToConstantString());
		this.baseAttributesDataMax = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::UnitTypeAttributeData>("fk_unit_type_id", ((int)this.Data.UnitTypeId).ToConstantString());
		this.RefreshDescription();
		this.CalculateProgressionPoints();
		if (this.UnitSave.campaignId != 0)
		{
			this.CampaignData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::CampaignUnitData>(this.UnitSave.campaignId);
		}
		if (string.IsNullOrEmpty(this.UnitSave.stats.name))
		{
			this.UnitSave.stats.name = this.GetRandomName(this.Data.WarbandId, this.Data.Id);
		}
		if (string.IsNullOrEmpty(this.UnitSave.skinColor))
		{
			this.UnitSave.skinColor = this.Data.SkinColor;
		}
		global::System.Collections.Generic.List<global::UnitJoinEnchantmentData> list3 = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::UnitJoinEnchantmentData>("fk_unit_id", this.Data.Id.ToIntString<global::UnitId>());
		for (int k = 0; k < list3.Count; k++)
		{
			this.Enchantments.Add(new global::Enchantment(list3[k].EnchantmentId, null, this, true, true, global::AllegianceId.NONE, true));
		}
		for (int l = 0; l < this.ranksData.Count; l++)
		{
			if (this.ranksData[l].EnchantmentId != global::EnchantmentId.NONE && this.ranksData[l].UnitRankId <= unitRankData.Id)
			{
				this.Enchantments.Add(new global::Enchantment(this.ranksData[l].EnchantmentId, null, this, true, true, global::AllegianceId.NONE, true));
			}
		}
		global::System.Collections.Generic.List<global::UnitJoinSkillData> list4 = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::UnitJoinSkillData>("fk_unit_id", this.Data.Id.ToIntString<global::UnitId>());
		for (int m = 0; m < list4.Count; m++)
		{
			this.AddSkill(list4[m].SkillId, false);
		}
		if (this.CampaignData != null)
		{
			global::System.Collections.Generic.List<global::CampaignUnitJoinEnchantmentData> list5 = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::CampaignUnitJoinEnchantmentData>("fk_campaign_unit_id", this.CampaignData.Id.ToIntString<global::CampaignUnitId>());
			for (int n = 0; n < list5.Count; n++)
			{
				this.Enchantments.Add(new global::Enchantment(list5[n].EnchantmentId, null, this, true, true, global::AllegianceId.NONE, true));
			}
		}
		for (int num = 0; num < this.UnitSave.injuries.Count; num++)
		{
			this.Injuries.Add(new global::Injury(this.UnitSave.injuries[num], this));
		}
		for (int num2 = 0; num2 < this.UnitSave.mutations.Count; num2++)
		{
			this.Mutations.Add(new global::Mutation(this.UnitSave.mutations[num2], this));
		}
		for (int num3 = 0; num3 < this.UnitSave.activeSkills.Count; num3++)
		{
			this.AddSkill(this.UnitSave.activeSkills[num3], false);
		}
		for (int num4 = 0; num4 < this.UnitSave.passiveSkills.Count; num4++)
		{
			this.AddSkill(this.UnitSave.passiveSkills[num4], false);
		}
		for (int num5 = 0; num5 < this.UnitSave.consumableSkills.Count; num5++)
		{
			this.AddSkill(this.UnitSave.consumableSkills[num5], false);
		}
		for (int num6 = 0; num6 < this.UnitSave.spells.Count; num6++)
		{
			this.AddSkill(this.UnitSave.spells[num6], false);
		}
		for (int num7 = 0; num7 < this.ActiveSkills.Count; num7++)
		{
			this.RemovePrequesiteSkill(this.ActiveSkills[num7]);
		}
		for (int num8 = 0; num8 < this.PassiveSkills.Count; num8++)
		{
			this.RemovePrequesiteSkill(this.PassiveSkills[num8]);
		}
		for (int num9 = 0; num9 < this.Spells.Count; num9++)
		{
			this.RemovePrequesiteSkill(this.Spells[num9]);
		}
		this.attributeModifiers = new global::System.Collections.Generic.Dictionary<global::AttributeId, global::System.Collections.Generic.List<global::AttributeMod>>(global::AttributeIdComparer.Instance);
		global::System.Collections.Generic.List<global::AttributeData> list6 = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::AttributeData>();
		for (int num10 = 0; num10 < list6.Count; num10++)
		{
			int id = (int)list6[num10].Id;
			this.attributeModifiers[list6[num10].Id] = new global::System.Collections.Generic.List<global::AttributeMod>();
			if (list6[num10].Save)
			{
				if (!this.UnitSave.stats.stats.ContainsKey(id))
				{
					this.UnitSave.stats.stats[id] = 0;
				}
				this.SetAttribute(list6[num10].Id, this.UnitSave.stats.stats[id]);
			}
			if (list6[num10].AttributeIdMax != global::AttributeId.NONE)
			{
				this.maxAttributes.Add(id, list6[num10].AttributeIdMax);
			}
		}
		if (this.CampaignData != null)
		{
			this.campaignModifiers = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::CampaignUnitJoinAttributeData>("fk_campaign_unit_id", ((int)this.CampaignData.Id).ToConstantString());
		}
		this.Items = new global::System.Collections.Generic.List<global::Item>();
		this.UpdateAttributes();
		foreach (global::BodyPart bodyPart in this.bodyParts.Values)
		{
			bodyPart.DestroyRelatedGO();
		}
		global::System.Collections.Generic.List<global::BodyPartData> list7 = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::BodyPartData>();
		global::System.Collections.Generic.List<global::BodyPartUnitExcludedData> list8 = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::BodyPartUnitExcludedData>("fk_unit_id", this.Id.ToIntString<global::UnitId>());
		int value;
		if (this.UnitSave.campaignId != 0)
		{
			global::CampaignWarbandJoinCampaignUnitData campaignWarbandJoinCampaignUnitData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::CampaignWarbandJoinCampaignUnitData>("fk_campaign_unit_id", this.UnitSave.campaignId.ToConstantString())[0];
			global::CampaignWarbandData campaignWarbandData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::CampaignWarbandData>((int)campaignWarbandJoinCampaignUnitData.CampaignWarbandId);
			value = (int)((int)campaignWarbandData.ColorPresetId << 8);
		}
		else
		{
			value = (int)((int)this.WarData.ColorPresetId << 8);
		}
		for (int num11 = 0; num11 < list7.Count; num11++)
		{
			global::BodyPartData bodyPartData = list7[num11];
			if (bodyPartData.Id != global::BodyPartId.NONE)
			{
				bool flag = string.IsNullOrEmpty(this.WarData.Asset);
				int num12 = 0;
				while (num12 < list8.Count && !flag)
				{
					if (list8[num12].BodyPartId == bodyPartData.Id)
					{
						flag = true;
					}
					num12++;
				}
				if (!flag)
				{
					this.availableBodyParts.Add(bodyPartData);
					if (!this.UnitSave.customParts.ContainsKey(bodyPartData.Id))
					{
						global::System.Collections.Generic.KeyValuePair<int, int> value2 = new global::System.Collections.Generic.KeyValuePair<int, int>(-1, value);
						this.UnitSave.customParts[bodyPartData.Id] = value2;
					}
				}
			}
		}
		this.InitItems();
		this.deathTrophy = new global::Item(this.Data.ItemIdTrophy, global::ItemQualityId.NORMAL);
		this.deathTrophy.owner = this;
		this.HasEnchantmentsChanged = false;
	}

	private void RefreshDescription()
	{
		this.LocalizedName = global::PandoraSingleton<global::LocalizationManager>.Instance.BuildStringAndLocalize("unit_name_", this.Id.ToString(), null, null);
		this.LocalizedType = string.Format("{0} / {1}", global::PandoraSingleton<global::LocalizationManager>.Instance.BuildStringAndLocalize("unit_type_", this.GetUnitTypeId().ToString(), null, null), this.LocalizedName);
		this.LocalizedDescription = global::PandoraSingleton<global::LocalizationManager>.Instance.BuildStringAndLocalize("unit_desc_", this.Id.ToString(), null, null);
	}

	private void InitItems()
	{
		this.SetItems();
		this.ResetBodyPart();
		this.ActiveWeaponSlot = global::UnitSlotId.SET1_MAINHAND;
		this.SetAttribute(global::AttributeId.CURRENT_WOUND, this.Wound);
		this.Status = global::UnitStateId.NONE;
		this.PreviousStatus = global::UnitStateId.NONE;
		this.SetAttribute(global::AttributeId.CURRENT_STRATEGY_POINTS, 0);
		this.SetAttribute(global::AttributeId.CURRENT_OFFENSE_POINTS, 0);
		this.tempStrategyPoints = 0;
		this.tempOffensePoints = 0;
	}

	private void CalculateProgressionPoints()
	{
		this.totalPhysicalPoints = 0;
		this.totalMartialPoints = 0;
		this.totalMentalPoints = 0;
		this.totalSkillPoints = 0;
		this.totalSpellPoints = 0;
		this.spentPhysicalPoints = 0;
		this.spentMartialPoints = 0;
		this.spentMentalPoints = 0;
		this.spentSkillPoints = 0;
		this.spentSpellPoints = 0;
		int num = 0;
		while (num < this.ranksData.Count && this.ranksData[num].UnitRankId <= this.UnitSave.rankId)
		{
			this.totalPhysicalPoints += this.ranksData[num].Physical;
			this.totalMartialPoints += this.ranksData[num].Martial;
			this.totalMentalPoints += this.ranksData[num].Mental;
			this.totalSkillPoints += this.ranksData[num].Skill;
			this.totalSpellPoints += this.ranksData[num].Spell;
			num++;
		}
		for (int i = 0; i < this.UnitSave.passiveSkills.Count; i++)
		{
			this.AddSkillLearnBonusPoints(this.UnitSave.passiveSkills[i]);
			this.spentSkillPoints += global::SkillHelper.GetSkill(this.UnitSave.passiveSkills[i]).Points;
		}
		for (int j = 0; j < this.UnitSave.activeSkills.Count; j++)
		{
			this.spentSkillPoints += global::SkillHelper.GetSkill(this.UnitSave.activeSkills[j]).Points;
		}
		for (int k = 0; k < this.UnitSave.spells.Count; k++)
		{
			this.spentSpellPoints += global::SkillHelper.GetSkill(this.UnitSave.spells[k]).Points;
		}
		for (int l = 0; l < this.UnitSave.consumableSkills.Count; l++)
		{
			this.AddSkillLearnBonusPoints(this.UnitSave.consumableSkills[l]);
		}
		for (int m = 0; m < global::Unit.PhysicalAttributeIds.Length; m++)
		{
			this.spentPhysicalPoints += this.GetSaveAttribute(global::Unit.PhysicalAttributeIds[m]);
		}
		for (int n = 0; n < global::Unit.MentalAttributeIds.Length; n++)
		{
			this.spentMentalPoints += this.GetSaveAttribute(global::Unit.MentalAttributeIds[n]);
		}
		for (int num2 = 0; num2 < global::Unit.MartialAttributeIds.Length; num2++)
		{
			this.spentMartialPoints += this.GetSaveAttribute(global::Unit.MartialAttributeIds[num2]);
		}
		if (this.UnitSave.skillInTrainingId != global::SkillId.NONE)
		{
			global::SkillData skill = global::SkillHelper.GetSkill(this.UnitSave.skillInTrainingId);
			if (skill.SkillTypeId == global::SkillTypeId.SPELL_ACTION)
			{
				this.spentSpellPoints += skill.Points;
			}
			else
			{
				this.spentSkillPoints += skill.Points;
			}
		}
	}

	private void AddSkillLearnBonusPoints(global::SkillId skillId)
	{
		global::System.Collections.Generic.List<global::SkillLearnBonusData> skillLearnBonus = global::SkillHelper.GetSkillLearnBonus(skillId);
		if (skillLearnBonus.Count > 0)
		{
			for (int i = 0; i < skillLearnBonus.Count; i++)
			{
				this.totalPhysicalPoints += skillLearnBonus[i].Physical;
				this.totalMentalPoints += skillLearnBonus[i].Mental;
				this.totalMartialPoints += skillLearnBonus[i].Martial;
				this.totalSkillPoints += skillLearnBonus[i].Skill;
				this.totalSpellPoints += skillLearnBonus[i].Spell;
			}
		}
	}

	private string GetRandomName(global::WarbandId warbandId, global::UnitId unitId)
	{
		global::System.Text.StringBuilder stringBuilder = new global::System.Text.StringBuilder();
		string text = ((int)warbandId).ToConstantString();
		string text2 = ((int)unitId).ToConstantString();
		string[] fields = new string[]
		{
			"fk_warband_id",
			"fk_unit_id",
			"surname"
		};
		string[] array = new string[]
		{
			text,
			text2,
			"0"
		};
		global::System.Collections.Generic.List<global::UnitNameData> list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::UnitNameData>(fields, array);
		if (list.Count > 0)
		{
			int index = global::UnityEngine.Random.Range(0, list.Count - 1);
			stringBuilder.Append(list[index].TheName);
		}
		else
		{
			array[1] = "0";
			list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::UnitNameData>(fields, array);
			if (list.Count > 0)
			{
				int index2 = global::UnityEngine.Random.Range(0, list.Count - 1);
				stringBuilder.Append(list[index2].TheName);
			}
		}
		array[1] = text2;
		array[2] = "1";
		list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::UnitNameData>(fields, array);
		if (list.Count > 0)
		{
			int index3 = global::UnityEngine.Random.Range(0, list.Count - 1);
			stringBuilder.Append(" ");
			stringBuilder.Append(list[index3].TheName);
		}
		else
		{
			array[1] = "0";
			list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::UnitNameData>(fields, array);
			if (list.Count > 0)
			{
				int index4 = global::UnityEngine.Random.Range(0, list.Count - 1);
				stringBuilder.Append(" ");
				stringBuilder.Append(list[index4].TheName);
			}
		}
		return (stringBuilder.Length <= 0) ? "Edgar" : stringBuilder.ToString();
	}

	public global::UnityEngine.Sprite GetIcon()
	{
		global::UnityEngine.Sprite sprite = global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadResource<global::UnityEngine.Sprite>("unit/" + this.Id.ToLowerString(), true);
		if (sprite == null)
		{
			global::PandoraDebug.LogWarning("Could not find icon at unit/" + this.Id.ToLowerString(), "UI", null);
		}
		return sprite;
	}

	public global::UnityEngine.Sprite GetUnitTypeIcon()
	{
		switch (this.GetUnitTypeId())
		{
		case global::UnitTypeId.LEADER:
			return global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadResource<global::UnityEngine.Sprite>("icn_leader", true);
		case global::UnitTypeId.IMPRESSIVE:
			return global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadResource<global::UnityEngine.Sprite>("icn_impressive", true);
		case global::UnitTypeId.HERO_1:
		case global::UnitTypeId.HERO_2:
		case global::UnitTypeId.HERO_3:
			return global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadResource<global::UnityEngine.Sprite>("icn_heroes", true);
		}
		return null;
	}

	public global::UnityEngine.Sprite GetActiveStatusIcon()
	{
		switch (this.GetActiveStatus())
		{
		case global::UnitActiveStatusId.AVAILABLE:
			return null;
		case global::UnitActiveStatusId.INJURED:
			return global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadResource<global::UnityEngine.Sprite>("active_status/injured", true);
		case global::UnitActiveStatusId.UPKEEP_NOT_PAID:
		case global::UnitActiveStatusId.INJURED_AND_UPKEEP_NOT_PAID:
			return global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadResource<global::UnityEngine.Sprite>("active_status/unpaid", true);
		case global::UnitActiveStatusId.IN_TRAINING:
			return global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadResource<global::UnityEngine.Sprite>("active_status/training", true);
		case global::UnitActiveStatusId.TREATMENT_NOT_PAID:
			return global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadResource<global::UnityEngine.Sprite>("active_status/treatment", true);
		default:
			return null;
		}
	}

	public global::UnityEngine.Color GetActiveStatusIconColor()
	{
		switch (this.GetActiveStatus())
		{
		case global::UnitActiveStatusId.AVAILABLE:
			return global::UnityEngine.Color.white;
		case global::UnitActiveStatusId.INJURED:
		case global::UnitActiveStatusId.TREATMENT_NOT_PAID:
			return global::Constant.GetColor(global::ConstantId.COLOR_RED);
		case global::UnitActiveStatusId.UPKEEP_NOT_PAID:
		case global::UnitActiveStatusId.INJURED_AND_UPKEEP_NOT_PAID:
			return global::Constant.GetColor(global::ConstantId.COLOR_GOLD);
		case global::UnitActiveStatusId.IN_TRAINING:
			return global::Constant.GetColor(global::ConstantId.COLOR_GREEN);
		default:
			return global::UnityEngine.Color.white;
		}
	}

	public int GetActiveStatusUnits()
	{
		switch (this.GetActiveStatus())
		{
		case global::UnitActiveStatusId.AVAILABLE:
			return 0;
		case global::UnitActiveStatusId.INJURED:
			return this.UnitSave.injuredTime;
		case global::UnitActiveStatusId.UPKEEP_NOT_PAID:
		case global::UnitActiveStatusId.INJURED_AND_UPKEEP_NOT_PAID:
			return this.GetUpkeepOwned();
		case global::UnitActiveStatusId.IN_TRAINING:
			return this.UnitSave.trainingTime;
		case global::UnitActiveStatusId.TREATMENT_NOT_PAID:
		{
			global::Tuple<int, global::EventLogger.LogEvent, int> tuple = this.Logger.FindLastEvent(global::EventLogger.LogEvent.NO_TREATMENT);
			return tuple.Item1 - global::PandoraSingleton<global::HideoutManager>.Instance.Date.CurrentDate;
		}
		default:
			return 0;
		}
	}

	public global::UnityEngine.Sprite GetSecondActiveStatusIcon()
	{
		switch (this.GetActiveStatus())
		{
		case global::UnitActiveStatusId.AVAILABLE:
		case global::UnitActiveStatusId.INJURED:
		case global::UnitActiveStatusId.UPKEEP_NOT_PAID:
		case global::UnitActiveStatusId.IN_TRAINING:
			return null;
		case global::UnitActiveStatusId.TREATMENT_NOT_PAID:
			return global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadResource<global::UnityEngine.Sprite>("active_status/unpaid", true);
		case global::UnitActiveStatusId.INJURED_AND_UPKEEP_NOT_PAID:
			return global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadResource<global::UnityEngine.Sprite>("active_status/injured", true);
		default:
			return null;
		}
	}

	public global::UnityEngine.Color GetSecondActiveStatusIconColor()
	{
		switch (this.GetActiveStatus())
		{
		case global::UnitActiveStatusId.AVAILABLE:
		case global::UnitActiveStatusId.INJURED:
		case global::UnitActiveStatusId.UPKEEP_NOT_PAID:
		case global::UnitActiveStatusId.IN_TRAINING:
			return global::UnityEngine.Color.white;
		case global::UnitActiveStatusId.TREATMENT_NOT_PAID:
			return global::Constant.GetColor(global::ConstantId.COLOR_GOLD);
		case global::UnitActiveStatusId.INJURED_AND_UPKEEP_NOT_PAID:
			return global::Constant.GetColor(global::ConstantId.COLOR_RED);
		default:
			return global::UnityEngine.Color.white;
		}
	}

	public int GetSecondActiveStatusUnits()
	{
		switch (this.GetActiveStatus())
		{
		case global::UnitActiveStatusId.AVAILABLE:
		case global::UnitActiveStatusId.INJURED:
		case global::UnitActiveStatusId.UPKEEP_NOT_PAID:
		case global::UnitActiveStatusId.IN_TRAINING:
			return 0;
		case global::UnitActiveStatusId.TREATMENT_NOT_PAID:
			return global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.GetUnitTreatmentCost(this);
		case global::UnitActiveStatusId.INJURED_AND_UPKEEP_NOT_PAID:
			return this.UnitSave.injuredTime;
		default:
			return 0;
		}
	}

	public void Resurect()
	{
		this.CurrentWound = this.Wound;
		this.Status = global::UnitStateId.NONE;
	}

	public int AddToAttribute(global::AttributeId attributeId, int add)
	{
		int num = this.attributes[(int)attributeId] + add;
		this.attributes[(int)attributeId] = num;
		if (this.UnitSave.stats.stats.ContainsKey((int)attributeId))
		{
			global::System.Collections.Generic.Dictionary<int, int> stats;
			global::System.Collections.Generic.Dictionary<int, int> dictionary = stats = this.UnitSave.stats.stats;
			int num2 = stats[(int)attributeId];
			dictionary[(int)attributeId] = num2 + add;
		}
		return num;
	}

	public global::AttributeData GetAttributeData(global::AttributeId attributeId)
	{
		return global::Unit.attributeDataById[(int)attributeId];
	}

	public int GetAttribute(global::AttributeId attributeId)
	{
		return this.attributes[(int)attributeId];
	}

	public global::AttributeId GetAttributeModifierId(global::AttributeId attributeId)
	{
		global::AttributeData attributeData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::AttributeData>((int)attributeId);
		return attributeData.AttributeIdModifier;
	}

	public int GetAttributeModifier(global::AttributeId attributeId)
	{
		global::AttributeId attributeModifierId = this.GetAttributeModifierId(attributeId);
		return (attributeModifierId == global::AttributeId.NONE) ? 0 : this.GetAttribute(attributeModifierId);
	}

	public global::AttributeId GetMaxAttribute(global::AttributeId attributeId)
	{
		global::AttributeId result;
		if (this.maxAttributes.TryGetValue((int)attributeId, out result))
		{
			return result;
		}
		return global::AttributeId.NONE;
	}

	public void SetAttribute(global::AttributeId attributeId, int value)
	{
		this.attributes[(int)attributeId] = value;
		if (this.UnitSave.stats.stats.ContainsKey((int)attributeId))
		{
			this.UnitSave.stats.stats[(int)attributeId] = value;
		}
	}

	public int GetTempAttribute(global::AttributeId attributeId)
	{
		return this.tempAttributes[(int)attributeId];
	}

	public void SetTempAttribute(global::AttributeId attributeId, int value)
	{
		this.tempAttributes[(int)attributeId] = value;
	}

	public bool HasModifierType(global::AttributeId attributeId, global::AttributeMod.Type modifierType)
	{
		global::System.Collections.Generic.List<global::AttributeMod> orNull = this.attributeModifiers.GetOrNull(attributeId);
		if (orNull != null)
		{
			for (int i = 0; i < orNull.Count; i++)
			{
				if (orNull[i].type == modifierType)
				{
					return true;
				}
			}
		}
		return false;
	}

	public int AddToTempAttribute(global::AttributeId attributeId, int add)
	{
		int num = this.tempAttributes[(int)attributeId] + add;
		this.tempAttributes[(int)attributeId] = num;
		return num;
	}

	public bool HasPendingChanges()
	{
		for (int i = 0; i < this.tempAttributes.Length; i++)
		{
			if (this.tempAttributes[i] > 0)
			{
				return true;
			}
		}
		return false;
	}

	public void ApplyChanges(bool update = true)
	{
		for (int i = 0; i < this.tempAttributes.Length; i++)
		{
			if (this.tempAttributes[i] > 0)
			{
				global::AttributeId key = (global::AttributeId)i;
				int num = this.tempAttributes[i];
				int num2 = 0;
				if (this.UnitSave.attributes.TryGetValue(key, out num2))
				{
					this.UnitSave.attributes[key] = num2 + num;
				}
				else
				{
					this.UnitSave.attributes[key] = num;
				}
				this.tempAttributes[i] = 0;
			}
		}
		this.CalculateProgressionPoints();
		if (update)
		{
			this.UpdateAttributes();
		}
	}

	public void ResetChanges()
	{
		for (int i = 0; i < this.tempAttributes.Length; i++)
		{
			this.tempAttributes[i] = 0;
		}
		this.UpdateAttributes();
	}

	private void AddAttributeModifier(global::AttributeMod.Type modifierType, global::AttributeId attributeId, string reason, int modifier, bool isPercent = false)
	{
		this.attributeModifiers[attributeId].Add(new global::AttributeMod(modifierType, reason, modifier, null, false, false));
	}

	public void UpdateAttributesAndCheckBackPack(global::System.Collections.Generic.List<global::Item> removedItems)
	{
		int backpackCapacity = this.BackpackCapacity;
		this.UpdateAttributes();
		if (backpackCapacity > this.BackpackCapacity)
		{
			for (int i = 6 + this.BackpackCapacity; i < 6 + backpackCapacity; i++)
			{
				if (this.items[i].Id != global::ItemId.NONE)
				{
					removedItems.Add(this.items[i]);
					this.items[i] = new global::Item(global::ItemId.NONE, global::ItemQualityId.NORMAL);
				}
				this.UnitSave.items[i] = null;
			}
		}
	}

	public void UpdateAttributes()
	{
		foreach (global::System.Collections.Generic.List<global::AttributeMod> list in this.attributeModifiers.Values)
		{
			list.Clear();
		}
		global::MovementData movementData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::MovementData>((int)this.Data.MovementId);
		global::UnitWoundData unitWoundData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::UnitWoundData>((int)this.Data.UnitWoundId);
		global::UnitTypeData unitTypeData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::UnitTypeData>((int)this.GetUnitTypeId());
		global::UnitRankData unitRankData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::UnitRankData>((int)this.UnitSave.rankId);
		global::UnitRankData unitRankData2 = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::UnitRankData>("rank", unitRankData.Rank.ToConstantString(), "advancement", "0")[0];
		for (int i = 0; i < this.attributeDataList.Count; i++)
		{
			global::AttributeData attributeData = this.attributeDataList[i];
			if (attributeData.Id != global::AttributeId.NONE && !attributeData.Persistent)
			{
				if (attributeData.BaseRoll != 0)
				{
					if (attributeData.IsBaseRoll)
					{
						this.AddAttributeModifier(global::AttributeMod.Type.BASE, attributeData.Id, global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("attribute_base_roll"), attributeData.BaseRoll, true);
					}
					else
					{
						this.AddAttributeModifier(global::AttributeMod.Type.BASE, attributeData.Id, global::PandoraSingleton<global::LocalizationManager>.Instance.BuildStringAndLocalize("attribute_", attributeData.Name, null, null), attributeData.BaseRoll, true);
					}
				}
				this.SetAttribute(attributeData.Id, attributeData.BaseRoll);
			}
		}
		this.SetAttribute(global::AttributeId.MORAL_IMPACT, this.baseMoralImpact);
		this.UnitAlwaysVisible = false;
		this.newState = this.Status;
		this.AddToAttribute(global::AttributeId.MOVEMENT, movementData.Distance);
		this.AddToAttribute(global::AttributeId.STRATEGY_POINTS, unitTypeData.StartSp);
		this.AddToAttribute(global::AttributeId.OFFENSE_POINTS, unitTypeData.StartOp);
		this.AddToAttribute(global::AttributeId.WOUND, unitWoundData.BaseWound + unitRankData2.Wound);
		this.AddToAttribute(global::AttributeId.INITIATIVE, unitTypeData.InitiativeBonus);
		this.SetAttribute(global::AttributeId.DAMAGE_MIN, this.Items[(int)this.ActiveWeaponSlot].DamageMin);
		this.SetAttribute(global::AttributeId.DAMAGE_MAX, this.Items[(int)this.ActiveWeaponSlot].DamageMax);
		for (int j = 0; j < this.baseAttributesData.Count; j++)
		{
			global::UnitJoinAttributeData unitJoinAttributeData = this.baseAttributesData[j];
			this.AddToAttribute(unitJoinAttributeData.AttributeId, unitJoinAttributeData.BaseValue);
		}
		foreach (global::System.Collections.Generic.KeyValuePair<global::AttributeId, int> keyValuePair in this.UnitSave.attributes)
		{
			this.AddToAttribute(keyValuePair.Key, keyValuePair.Value);
		}
		int num = 0;
		while (num < this.ranksData.Count && this.ranksData[num].UnitRankId <= this.UnitSave.rankId)
		{
			this.AddToAttribute(global::AttributeId.WOUND, this.ranksData[num].Wound);
			this.AddToAttribute(global::AttributeId.STRATEGY_POINTS, this.ranksData[num].Strategy);
			this.AddToAttribute(global::AttributeId.OFFENSE_POINTS, this.ranksData[num].Offense);
			num++;
		}
		if (this.CampaignData != null)
		{
			for (int k = 0; k < this.campaignModifiers.Count; k++)
			{
				global::CampaignUnitJoinAttributeData campaignUnitJoinAttributeData = this.campaignModifiers[k];
				this.AddToAttribute(campaignUnitJoinAttributeData.AttributeId, campaignUnitJoinAttributeData.Value);
			}
		}
		for (int l = 0; l < this.tempAttributes.Length; l++)
		{
			global::AttributeId attributeId = (global::AttributeId)l;
			if (this.GetTempAttribute(attributeId) > 0)
			{
				this.AddToAttribute((global::AttributeId)l, this.tempAttributes[l]);
				this.AddAttributeModifier(global::AttributeMod.Type.TEMP, (global::AttributeId)l, "temp", this.tempAttributes[l], false);
			}
		}
		this.AddToAttribute(global::AttributeId.ARMOR_ABSORPTION, this.Items[0].ArmorAbsorption + this.Items[1].ArmorAbsorption);
		if (this.Items[0].ArmorAbsorption > 0)
		{
			this.AddAttributeModifier(global::AttributeMod.Type.ITEM, global::AttributeId.ARMOR_ABSORPTION, global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(this.Items[0].LabelName), this.Items[0].ArmorAbsorption, false);
		}
		if (this.Items[1].ArmorAbsorption > 0)
		{
			this.AddAttributeModifier(global::AttributeMod.Type.ITEM, global::AttributeId.ARMOR_ABSORPTION, global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(this.Items[1].LabelName), this.Items[1].ArmorAbsorption, false);
		}
		this.actionCostModifiers.Clear();
		this.skillCostModifiers.Clear();
		this.spellTypeModifiers.Clear();
		this.damagePercModifiers.Clear();
		this.blockedActions.Clear();
		this.blockedSkills.Clear();
		this.blockedBones.Clear();
		this.blockedItemTypes.Clear();
		if (this.Items[(int)(this.ActiveWeaponSlot + 1)].Id != global::ItemId.NONE && this.Items[(int)(this.ActiveWeaponSlot + 1)].DamageMin > 0)
		{
			this.AddToAttribute(global::AttributeId.DAMAGE_MIN, this.Items[(int)(this.ActiveWeaponSlot + 1)].DamageMin);
			this.AddToAttribute(global::AttributeId.DAMAGE_MAX, this.Items[(int)(this.ActiveWeaponSlot + 1)].DamageMax);
		}
		this.RefreshActiveItems();
		for (int m = 0; m < this.PassiveSkills.Count; m++)
		{
			this.ApplySkill(this.PassiveSkills[m]);
		}
		for (int n = 0; n < this.ConsumableSkills.Count; n++)
		{
			this.ApplySkill(this.ConsumableSkills[n]);
		}
		if (this.ActiveSkill != null)
		{
			this.ApplySkill(this.ActiveSkill);
		}
		this.RefreshImmunities();
		for (int num2 = this.Enchantments.Count - 1; num2 >= 0; num2--)
		{
			bool flag = false;
			if (!this.Enchantments[num2].Data.Indestructible)
			{
				int num3 = this.enchantTypeImmunities.IndexOf(this.Enchantments[num2].Data.EnchantmentTypeId, global::EnchantmentTypeIdComparer.Instance);
				int num4 = this.enchantTypeToBeRemoved.IndexOf(this.Enchantments[num2].Data.EnchantmentTypeId, global::EnchantmentTypeIdComparer.Instance);
				int num5 = this.enchantToBeRemoved.IndexOf(this.Enchantments[num2].Id, global::EnchantmentIdComparer.Instance);
				if ((this.Enchantments[num2].Data.RequireUnitState && this.Enchantments[num2].Data.UnitStateIdRequired != this.Status) || num3 != -1 || num4 != -1 || num5 != -1)
				{
					flag = true;
					this.RemoveEnchantment(num2);
					if (num4 != -1)
					{
						this.enchantTypeToBeRemoved.RemoveAt(num4);
					}
					else if (num5 != -1)
					{
						this.enchantToBeRemoved.RemoveAt(num5);
					}
				}
			}
			if (!flag)
			{
				this.ApplyEnchantment(this.Enchantments[num2]);
			}
		}
		for (int num6 = 0; num6 < this.ActiveItems.Count; num6++)
		{
			this.ApplyItem(this.ActiveItems[num6]);
			if (this.ActiveItems[num6].SpeedData != null)
			{
				this.AddToAttribute(global::AttributeId.INITIATIVE, this.ActiveItems[num6].SpeedData.Speed);
			}
		}
		for (int num7 = 0; num7 < this.Injuries.Count; num7++)
		{
			this.ApplyInjury(this.Injuries[num7]);
		}
		for (int num8 = 0; num8 < this.Mutations.Count; num8++)
		{
			this.ApplyMutation(this.Mutations[num8]);
		}
		this.RemoveAppliedEnchantments();
		for (int num9 = 0; num9 < this.baseAttributesDataMax.Count; num9++)
		{
			global::UnitTypeAttributeData unitTypeAttributeData = this.baseAttributesDataMax[num9];
			int num10 = this.GetAttribute(unitTypeAttributeData.AttributeId);
			num10 = global::UnityEngine.Mathf.Min(num10, unitTypeAttributeData.Max);
			this.SetAttribute(unitTypeAttributeData.AttributeId, num10);
		}
		for (int num11 = 0; num11 < this.attributeDataList.Count; num11++)
		{
			global::AttributeData attributeData2 = this.attributeDataList[num11];
			if (attributeData2.Id != global::AttributeId.NONE)
			{
				int num12 = this.GetAttribute(attributeData2.Id) + this.BaseStatIncrease(attributeData2.Id, attributeData2.IsPercent);
				if (attributeData2.AttributeIdMax != global::AttributeId.NONE)
				{
					num12 = global::UnityEngine.Mathf.Clamp(num12, 1, this.GetAttribute(attributeData2.AttributeIdMax));
				}
				if (attributeData2.Id == global::AttributeId.CRIT_RESISTANCE || attributeData2.Id == global::AttributeId.MORAL_IMPACT)
				{
					num12 = global::UnityEngine.Mathf.Max(0, num12);
				}
				if (attributeData2.Id == global::AttributeId.ARMOR_ABSORPTION_PERC)
				{
					num12 = global::UnityEngine.Mathf.Min(num12, 95);
				}
				this.SetAttribute(attributeData2.Id, num12);
			}
		}
		this.SetAttribute(global::AttributeId.WOUND, (int)((float)this.Wound * (1f + (float)this.WoundPerc / 100f)));
		this.attributeModifiers[global::AttributeId.WOUND].AddRange(this.attributeModifiers[global::AttributeId.WOUND_PERC]);
		this.CurrentWound = global::UnityEngine.Mathf.Clamp(this.CurrentWound, 0, this.Wound);
		this.SetAttribute(global::AttributeId.STRATEGY_POINTS, global::UnityEngine.Mathf.Clamp(this.GetAttribute(global::AttributeId.STRATEGY_POINTS), 0, unitTypeData.MaxSp));
		this.SetAttribute(global::AttributeId.OFFENSE_POINTS, global::UnityEngine.Mathf.Clamp(this.GetAttribute(global::AttributeId.OFFENSE_POINTS), 0, unitTypeData.MaxOp));
		this.SetAttribute(global::AttributeId.CURRENT_STRATEGY_POINTS, global::UnityEngine.Mathf.Clamp(this.CurrentStrategyPoints, 0, global::UnityEngine.Mathf.Max(this.StrategyPoints, 0)));
		this.SetAttribute(global::AttributeId.CURRENT_OFFENSE_POINTS, global::UnityEngine.Mathf.Clamp(this.CurrentOffensePoints, 0, global::UnityEngine.Mathf.Max(this.OffensePoints, 0)));
		if (this.newState != this.Status)
		{
			this.SetStatus(this.newState);
			this.newState = global::UnitStateId.NONE;
		}
		if (this.needFxRefresh)
		{
			this.UpdateEnchantmentsFx();
		}
		this.realBackPackCapacity = global::Constant.GetInt(global::ConstantId.UNIT_BACKPACK_SLOT) + ((this.Strength < global::Constant.GetInt(global::ConstantId.UNIT_BACKPACK_SLOT_STR_INCREASE_1)) ? 0 : 1) + ((this.Strength < global::Constant.GetInt(global::ConstantId.UNIT_BACKPACK_SLOT_STR_INCREASE_2)) ? 0 : 1) + ((this.Strength < global::Constant.GetInt(global::ConstantId.UNIT_BACKPACK_SLOT_STR_INCREASE_3)) ? 0 : 1) + ((this.Strength < global::Constant.GetInt(global::ConstantId.UNIT_BACKPACK_SLOT_STR_INCREASE_4)) ? 0 : 1) + ((this.Strength < global::Constant.GetInt(global::ConstantId.UNIT_BACKPACK_SLOT_STR_INCREASE_5)) ? 0 : 1);
		global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<global::Unit>(global::Notices.UNIT_ATTRIBUTES_CHANGED, this);
	}

	private void RefreshImmunities()
	{
		this.enchantTypeImmunities.Clear();
		this.enchantTypeToBeRemoved.Clear();
		this.enchantToBeRemoved.Clear();
		this.GetEnchantmentsImmunitiesRemovers(this.Enchantments);
		for (int i = 0; i < this.ActiveItems.Count; i++)
		{
			this.GetEnchantmentsImmunitiesRemovers(this.ActiveItems[i].Enchantments);
			if (this.ActiveItems[i].RuneMark != null)
			{
				this.GetEnchantmentsImmunitiesRemovers(this.ActiveItems[i].RuneMark.Enchantments);
			}
		}
		for (int j = 0; j < this.Injuries.Count; j++)
		{
			this.GetEnchantmentsImmunitiesRemovers(this.Injuries[j].Enchantments);
		}
		for (int k = 0; k < this.Mutations.Count; k++)
		{
			this.GetEnchantmentsImmunitiesRemovers(this.Mutations[k].Enchantments);
		}
	}

	public bool HasEnchantmentImmunity(global::Enchantment enchantment)
	{
		return this.HasEnchantmentImmunity(enchantment.Data.EnchantmentTypeId, enchantment.Id);
	}

	public bool HasEnchantmentImmunity(global::EnchantmentTypeId typeId, global::EnchantmentId id)
	{
		return this.enchantTypeImmunities.Contains(typeId, global::EnchantmentTypeIdComparer.Instance) || this.enchantTypeToBeRemoved.Contains(typeId, global::EnchantmentTypeIdComparer.Instance) || this.enchantToBeRemoved.Contains(id, global::EnchantmentIdComparer.Instance);
	}

	private int BaseStatIncrease(global::AttributeId attrId, bool isPercent)
	{
		int num = 0;
		global::System.Collections.Generic.List<global::AttributeAttributeData> list;
		if (global::Unit.attributeAttributeDataById.TryGetValue(attrId, out list))
		{
			for (int i = 0; i < list.Count; i++)
			{
				global::AttributeAttributeData attributeAttributeData = list[i];
				if (attributeAttributeData != null)
				{
					int num2 = this.GetAttribute(attributeAttributeData.AttributeIdBase) * attributeAttributeData.Modifier;
					num += num2;
					this.AddAttributeModifier(global::AttributeMod.Type.ATTRIBUTE, attrId, global::PandoraSingleton<global::LocalizationManager>.Instance.BuildStringAndLocalize("attribute_name_", this.GetAttributeData(attributeAttributeData.AttributeIdBase).Name, null, null), num2, isPercent);
					int tempAttribute = this.GetTempAttribute(attributeAttributeData.AttributeIdBase);
					if (tempAttribute > 0)
					{
						this.AddAttributeModifier(global::AttributeMod.Type.TEMP, attrId, "temp", tempAttribute, isPercent);
					}
				}
			}
		}
		return num;
	}

	private void AddUnitCostModifier(global::UnitActionId id, int sp, int op)
	{
		if (!this.actionCostModifiers.ContainsKey(id))
		{
			this.actionCostModifiers[id] = new global::CostModifier();
		}
		this.actionCostModifiers[id].strat += sp;
		this.actionCostModifiers[id].off += op;
	}

	private void AddSkillCostModifier(global::SkillId id, int sp, int op)
	{
		if (!this.skillCostModifiers.ContainsKey(id))
		{
			this.skillCostModifiers[id] = new global::CostModifier();
		}
		this.skillCostModifiers[id].strat += sp;
		this.skillCostModifiers[id].off += op;
	}

	private void AddSpellTypeCostMofier(global::SpellTypeId id, int sp, int op)
	{
		if (!this.spellTypeModifiers.ContainsKey(id))
		{
			this.spellTypeModifiers[id] = new global::CostModifier();
		}
		this.spellTypeModifiers[id].strat += sp;
		this.spellTypeModifiers[id].off += op;
	}

	private void AddDamagePercModifier(global::SkillId id, int damage)
	{
		if (!this.damagePercModifiers.ContainsKey(id))
		{
			this.damagePercModifiers[id] = 0;
		}
		global::System.Collections.Generic.Dictionary<global::SkillId, int> dictionary2;
		global::System.Collections.Generic.Dictionary<global::SkillId, int> dictionary = dictionary2 = this.damagePercModifiers;
		int num = dictionary2[id];
		dictionary[id] = num + damage;
	}

	private void GetEnchantmentsImmunitiesRemovers(global::System.Collections.Generic.List<global::Enchantment> enchantments)
	{
		for (int i = 0; i < enchantments.Count; i++)
		{
			global::Enchantment enchantment = enchantments[i];
			for (int j = 0; j < enchantment.Immunities.Count; j++)
			{
				global::EnchantmentRemoveEnchantmentTypeData enchantmentRemoveEnchantmentTypeData = enchantment.Immunities[j];
				if (enchantmentRemoveEnchantmentTypeData.EnchantmentTypeId != global::EnchantmentTypeId.NONE)
				{
					if (enchantmentRemoveEnchantmentTypeData.Count == 0)
					{
						this.enchantTypeImmunities.Add(enchantmentRemoveEnchantmentTypeData.EnchantmentTypeId);
					}
					else
					{
						for (int k = 0; k < enchantmentRemoveEnchantmentTypeData.Count; k++)
						{
							this.enchantTypeToBeRemoved.Add(enchantmentRemoveEnchantmentTypeData.EnchantmentTypeId);
						}
					}
				}
			}
			for (int l = 0; l < enchantment.Remover.Count; l++)
			{
				global::EnchantmentRemoveEnchantmentData enchantmentRemoveEnchantmentData = enchantment.Remover[l];
				if (enchantmentRemoveEnchantmentData.EnchantmentIdRemove != global::EnchantmentId.NONE)
				{
					for (int m = 0; m < enchantmentRemoveEnchantmentData.Count; m++)
					{
						this.enchantToBeRemoved.Add(enchantmentRemoveEnchantmentData.EnchantmentIdRemove);
					}
				}
			}
		}
	}

	private void SetMoralImpact()
	{
		global::UnitRankData unitRankData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::UnitRankData>((int)this.UnitSave.rankId);
		global::UnitTypeData unitTypeData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::UnitTypeData>((int)this.GetUnitTypeId());
		this.baseMoralImpact = unitTypeData.MoralImpact + unitRankData.Rank;
	}

	private void ApplySkill(global::SkillData skillData)
	{
		string id = ((int)skillData.Id).ToConstantString();
		global::System.Collections.Generic.List<global::SkillItemData> list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::SkillItemData>("fk_skill_id", ((int)skillData.Id).ToConstantString());
		if (list.Count > 0)
		{
			bool flag = false;
			int num = 0;
			while (num < list.Count && !flag)
			{
				global::SkillItemData skillItemData = list[num];
				flag = this.HasItemActive(skillItemData.ItemId);
				if (flag && skillItemData.MutationId != global::MutationId.NONE)
				{
					flag = this.HasMutation(skillItemData.MutationId);
				}
				num++;
			}
			if (!flag)
			{
				return;
			}
		}
		global::System.Collections.Generic.List<global::SkillAttributeData> list2 = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::SkillAttributeData>("fk_skill_id", id);
		for (int i = 0; i < list2.Count; i++)
		{
			global::SkillAttributeData skillAttributeData = list2[i];
			if ((skillAttributeData.UnitActionIdTrigger == global::UnitActionId.NONE && skillAttributeData.SkillIdTrigger == global::SkillId.NONE) || (this.ActiveSkill != null && this.ActiveSkill.UnitActionId != global::UnitActionId.NONE && this.ActiveSkill.UnitActionId == skillAttributeData.UnitActionIdTrigger) || (this.ActiveSkill != null && this.ActiveSkill.Id != global::SkillId.NONE && this.ActiveSkill.Id == skillAttributeData.SkillIdTrigger))
			{
				this.AddToAttribute(skillAttributeData.AttributeId, skillAttributeData.Modifier);
				this.AddAttributeModifier(global::AttributeMod.Type.SKILL, skillAttributeData.AttributeId, global::PandoraSingleton<global::LocalizationManager>.Instance.BuildStringAndLocalize("skill_name_", skillAttributeData.SkillId.ToSkillIdString(), null, null), skillAttributeData.Modifier, false);
			}
		}
		global::System.Collections.Generic.List<global::SkillCostModifierData> list3 = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::SkillCostModifierData>("fk_skill_id", id);
		for (int j = 0; j < list3.Count; j++)
		{
			global::SkillCostModifierData skillCostModifierData = list3[j];
			if (skillCostModifierData.UnitActionId != global::UnitActionId.NONE)
			{
				this.AddUnitCostModifier(skillCostModifierData.UnitActionId, skillCostModifierData.StrategyPoints, skillCostModifierData.OffensePoints);
			}
			else if (skillCostModifierData.SkillId != global::SkillId.NONE)
			{
				this.AddSkillCostModifier(skillCostModifierData.SkillIdTarget, skillCostModifierData.StrategyPoints, skillCostModifierData.OffensePoints);
			}
		}
	}

	public bool HasPassiveSkill(global::SkillId skillId)
	{
		for (int i = 0; i < this.PassiveSkills.Count; i++)
		{
			global::SkillData skillData = this.PassiveSkills[i];
			if (skillData.Id == skillId || skillData.SkillIdPrerequiste == skillId)
			{
				return true;
			}
		}
		return false;
	}

	private int RemoveSkillFromList(global::System.Collections.Generic.List<global::SkillData> skillsList, global::SkillId skillId)
	{
		for (int i = 0; i < skillsList.Count; i++)
		{
			if (skillsList[i].Id == skillId)
			{
				this.RemoveEnchantments(skillId, this);
				skillsList.RemoveAt(i);
				return i;
			}
		}
		return -1;
	}

	private void AddSkill(global::SkillId skillId, bool updateAttributes = true)
	{
		global::SkillData skillData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::SkillData>((int)skillId);
		int num = this.RemovePrequesiteSkill(skillData);
		if (skillData.SkillTypeId == global::SkillTypeId.CONSUMABLE_ACTION)
		{
			if (num == -1)
			{
				num = this.ConsumableSkills.Count;
			}
			this.ConsumableSkills.Insert(num, skillData);
			this.AddSkillEnchantment(skillData);
			this.CalculateProgressionPoints();
		}
		else if (skillData.Passive)
		{
			if (num == -1)
			{
				num = this.PassiveSkills.Count;
			}
			this.PassiveSkills.Insert(num, skillData);
			this.AddSkillEnchantment(skillData);
		}
		else if (skillData.SpellTypeId != global::SpellTypeId.NONE)
		{
			if (num == -1)
			{
				num = this.Spells.Count;
			}
			this.Spells.Insert(num, skillData);
		}
		else
		{
			if (num == -1)
			{
				num = this.ActiveSkills.Count;
			}
			this.ActiveSkills.Insert(num, skillData);
		}
		if (updateAttributes)
		{
			this.UpdateAttributes();
		}
	}

	private int RemovePrequesiteSkill(global::SkillData skillData)
	{
		int result = -1;
		if (skillData.SkillTypeId != global::SkillTypeId.CONSUMABLE_ACTION && global::SkillHelper.IsMastery(skillData) && skillData.SkillIdPrerequiste != global::SkillId.NONE)
		{
			if (skillData.Passive)
			{
				result = this.RemoveSkillFromList(this.PassiveSkills, skillData.SkillIdPrerequiste);
			}
			else if (skillData.SkillTypeId == global::SkillTypeId.SPELL_ACTION)
			{
				result = this.RemoveSkillFromList(this.Spells, skillData.SkillIdPrerequiste);
			}
			else
			{
				result = this.RemoveSkillFromList(this.ActiveSkills, skillData.SkillIdPrerequiste);
			}
		}
		return result;
	}

	private void AddSkillEnchantment(global::SkillData skillData)
	{
		global::System.Collections.Generic.List<global::SkillEnchantmentData> list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::SkillEnchantmentData>("fk_skill_id", ((int)skillData.Id).ToConstantString());
		for (int i = 0; i < list.Count; i++)
		{
			global::SkillEnchantmentData skillEnchantmentData = list[i];
			if (skillEnchantmentData.EnchantmentTriggerId == global::EnchantmentTriggerId.NONE && skillEnchantmentData.Self)
			{
				this.AddEnchantment(skillEnchantmentData.EnchantmentId, this, skillData.Passive, false, global::AllegianceId.NONE);
			}
		}
	}

	public void SetActiveSkill(global::SkillData data)
	{
		this.ActiveSkill = data;
		this.UpdateAttributes();
	}

	public bool NeedsRoll(global::AttributeId attributeId)
	{
		if (this.HasEnchantmentRollEffect(this.Enchantments, attributeId))
		{
			return true;
		}
		for (int i = 0; i < this.ActiveItems.Count; i++)
		{
			if (this.HasEnchantmentRollEffect(this.ActiveItems[i].Enchantments, attributeId))
			{
				return true;
			}
			if (this.ActiveItems[i].RuneMark != null && this.HasEnchantmentRollEffect(this.ActiveItems[i].RuneMark.Enchantments, attributeId))
			{
				return true;
			}
		}
		for (int j = 0; j < this.Mutations.Count; j++)
		{
			if (this.HasEnchantmentRollEffect(this.Mutations[j].Enchantments, attributeId))
			{
				return true;
			}
		}
		for (int k = 0; k < this.Injuries.Count; k++)
		{
			if (this.HasEnchantmentRollEffect(this.Injuries[k].Enchantments, attributeId))
			{
				return true;
			}
		}
		return false;
	}

	public bool Roll(global::Tyche tyche, global::AttributeId attributeId, bool reverse = false, bool apply = true)
	{
		return this.Roll(tyche, this.GetAttribute(attributeId), attributeId, reverse, apply, 0);
	}

	public bool Roll(global::Tyche tyche, int target, global::AttributeId attributeId, bool reverse = false, bool apply = true, int mod = 0)
	{
		target = global::UnityEngine.Mathf.Clamp(target, 0, 100);
		int num = tyche.Rand(0, 100);
		num = global::UnityEngine.Mathf.Max(num - mod, 0);
		bool flag = num < target;
		bool flag2 = (!reverse) ? flag : (!flag);
		global::UnitController currentUnit = global::PandoraSingleton<global::MissionManager>.Instance.GetCurrentUnit();
		if (currentUnit != null)
		{
			global::Unit unit = currentUnit.unit;
			if (this == unit)
			{
				global::PandoraSingleton<global::MissionManager>.Instance.CombatLogger.AddLog(this, (!flag2) ? global::CombatLogger.LogMessage.ROLL_FAIL : global::CombatLogger.LogMessage.ROLL_SUCCESS, new string[]
				{
					global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("attribute_name_" + attributeId.ToAttributeIdString()),
					(num + 1).ToConstantString(),
					target.ToConstantString()
				});
			}
			else
			{
				global::PandoraSingleton<global::MissionManager>.Instance.CombatLogger.AddLog(this, (!flag2) ? global::CombatLogger.LogMessage.UNIT_ROLL_FAIL : global::CombatLogger.LogMessage.UNIT_ROLL_SUCCESS, new string[]
				{
					global::PandoraSingleton<global::MissionManager>.Instance.GetUnitController(this, false).GetLogName(),
					global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("attribute_name_" + attributeId.ToAttributeIdString()),
					(num + 1).ToConstantString(),
					target.ToConstantString()
				});
			}
		}
		if (apply)
		{
			this.ApplyRollResult(tyche, num, flag, attributeId);
		}
		return flag;
	}

	public void ApplyRollResult(global::Tyche tyche, int rand, bool success, global::AttributeId attrId)
	{
		bool flag = false;
		flag |= this.SetEnchantRollResult(tyche, this.Enchantments, rand, success, attrId);
		for (int i = 0; i < this.ActiveItems.Count; i++)
		{
			flag |= this.SetEnchantRollResult(tyche, this.ActiveItems[i].Enchantments, rand, success, attrId);
			if (this.ActiveItems[i].RuneMark != null)
			{
				flag |= this.SetEnchantRollResult(tyche, this.ActiveItems[i].RuneMark.Enchantments, rand, success, attrId);
			}
		}
		for (int j = 0; j < this.Injuries.Count; j++)
		{
			flag |= this.SetEnchantRollResult(tyche, this.Injuries[j].Enchantments, rand, success, attrId);
		}
		for (int k = 0; k < this.Mutations.Count; k++)
		{
			flag |= this.SetEnchantRollResult(tyche, this.Mutations[k].Enchantments, rand, success, attrId);
		}
		if (flag)
		{
			this.UpdateAttributes();
		}
		if (flag)
		{
			this.UpdateAttributes();
		}
	}

	private bool SetEnchantRollResult(global::Tyche tyche, global::System.Collections.Generic.List<global::Enchantment> enchantments, int rand, bool success, global::AttributeId attrId)
	{
		bool result = false;
		for (int i = enchantments.Count - 1; i >= 0; i--)
		{
			for (int j = 0; j < enchantments[i].Effects.Count; j++)
			{
				global::EnchantmentEffectEnchantmentData enchantmentEffectEnchantmentData = enchantments[i].Effects[j];
				if (enchantmentEffectEnchantmentData.AttributeIdRoll == attrId && ((success && enchantmentEffectEnchantmentData.EnchantmentTriggerId == global::EnchantmentTriggerId.ON_ROLL_SUCCESS) || (!success && enchantmentEffectEnchantmentData.EnchantmentTriggerId == global::EnchantmentTriggerId.ON_ROLL_FAIL)))
				{
					bool flag = true;
					if (enchantmentEffectEnchantmentData.Ratio != 0)
					{
						flag = (tyche.Rand(0, 100) < enchantmentEffectEnchantmentData.Ratio);
					}
					if (flag)
					{
						global::Enchantment enchantment = this.AddEnchantment(enchantmentEffectEnchantmentData.EnchantmentIdEffect, (enchantments[i].Provider == null) ? this : enchantments[i].Provider, false, false, enchantments[i].AllegianceId);
						if (enchantment != null)
						{
							if (!enchantment.Data.NoDisplay)
							{
								global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<global::Unit, string, global::EffectTypeId>(global::Notices.RETROACTION_TARGET_ENCHANTMENT, this, enchantment.LocalizedName, enchantment.Data.EffectTypeId);
							}
							result = true;
						}
					}
				}
			}
		}
		return result;
	}

	public void ResetEnchantsChanged()
	{
		this.HasEnchantmentsChanged = false;
	}

	private void ApplyEnchantment(global::Enchantment enchant)
	{
		if (this.enchantTypeImmunities.IndexOf(enchant.Data.EnchantmentTypeId, global::EnchantmentTypeIdComparer.Instance) != -1 || (enchant.Data.RequireUnitState && this.Status != enchant.Data.UnitStateIdRequired))
		{
			return;
		}
		for (int i = 0; i < enchant.AttributeModifiers.Count; i++)
		{
			global::EnchantmentJoinAttributeData enchantmentJoinAttributeData = enchant.AttributeModifiers[i];
			this.AddToAttribute(enchantmentJoinAttributeData.AttributeId, enchantmentJoinAttributeData.Modifier);
			if (enchantmentJoinAttributeData.AttributeId == global::AttributeId.CURRENT_WOUND && enchantmentJoinAttributeData.Modifier != 0 && global::PandoraSingleton<global::MissionManager>.Exists())
			{
				global::PandoraSingleton<global::MissionManager>.Instance.GetUnitController(this, false).ComputeDirectWound(-enchantmentJoinAttributeData.Modifier, true, global::PandoraSingleton<global::MissionManager>.Instance.GetUnitController(enchant.Provider, false), true);
			}
			if (!enchant.Data.NoDisplay)
			{
				global::AttributeMod.Type modifierType = global::AttributeMod.Type.ENCHANTMENT;
				global::EffectTypeId effectTypeId = enchant.Data.EffectTypeId;
				if (effectTypeId != global::EffectTypeId.BUFF)
				{
					if (effectTypeId == global::EffectTypeId.DEBUFF)
					{
						modifierType = global::AttributeMod.Type.DEBUFF;
					}
				}
				else
				{
					modifierType = global::AttributeMod.Type.BUFF;
				}
				this.AddAttributeModifier(modifierType, enchantmentJoinAttributeData.AttributeId, global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(enchant.LabelName), enchantmentJoinAttributeData.Modifier, false);
			}
		}
		for (int j = 0; j < enchant.CostModifiers.Count; j++)
		{
			global::EnchantmentCostModifierData enchantmentCostModifierData = enchant.CostModifiers[j];
			if (enchantmentCostModifierData.UnitActionId != global::UnitActionId.NONE)
			{
				this.AddUnitCostModifier(enchantmentCostModifierData.UnitActionId, enchantmentCostModifierData.StrategyPoints, enchantmentCostModifierData.OffensePoints);
			}
			else if (enchantmentCostModifierData.SkillId != global::SkillId.NONE)
			{
				this.AddSkillCostModifier(enchantmentCostModifierData.SkillId, enchantmentCostModifierData.StrategyPoints, enchantmentCostModifierData.OffensePoints);
			}
			else if (enchantmentCostModifierData.SpellTypeId != global::SpellTypeId.NONE)
			{
				this.AddSpellTypeCostMofier(enchantmentCostModifierData.SpellTypeId, enchantmentCostModifierData.StrategyPoints, enchantmentCostModifierData.OffensePoints);
			}
		}
		for (int k = 0; k < enchant.DamageModifiers.Count; k++)
		{
			global::EnchantmentDamageModifierData enchantmentDamageModifierData = enchant.DamageModifiers[k];
			this.AddDamagePercModifier(enchantmentDamageModifierData.SkillId, enchantmentDamageModifierData.DamagePercModifier);
		}
		for (int l = 0; l < enchant.ActionBlockers.Count; l++)
		{
			global::EnchantmentBlockUnitActionData enchantmentBlockUnitActionData = enchant.ActionBlockers[l];
			if (enchantmentBlockUnitActionData.UnitActionId != global::UnitActionId.NONE && !this.blockedActions.ContainsKey(enchantmentBlockUnitActionData.UnitActionId))
			{
				this.blockedActions.Add(enchantmentBlockUnitActionData.UnitActionId, enchant.Id);
			}
			if (enchantmentBlockUnitActionData.SkillId != global::SkillId.NONE && !this.blockedSkills.ContainsKey(enchantmentBlockUnitActionData.SkillId))
			{
				this.blockedSkills.Add(enchantmentBlockUnitActionData.SkillId, enchant.Id);
			}
		}
		for (int m = 0; m < enchant.BoneBlockers.Count; m++)
		{
			global::EnchantmentBlockBoneData enchantmentBlockBoneData = enchant.BoneBlockers[m];
			if (!this.blockedBones.ContainsKey(enchantmentBlockBoneData.BoneId))
			{
				this.blockedBones.Add(enchantmentBlockBoneData.BoneId, enchantmentBlockBoneData.EnchantmentId);
			}
		}
		for (int n = 0; n < enchant.ItemTypeBlockers.Count; n++)
		{
			global::EnchantmentBlockItemTypeData enchantmentBlockItemTypeData = enchant.ItemTypeBlockers[n];
			if (!this.blockedItemTypes.ContainsKey(enchantmentBlockItemTypeData.ItemTypeId))
			{
				this.blockedItemTypes.Add(enchantmentBlockItemTypeData.ItemTypeId, enchantmentBlockItemTypeData.EnchantmentId);
			}
		}
		if (enchant.Data.ChangeUnitState && enchant.Data.UnitStateIdRequired == this.Status)
		{
			this.newState = enchant.Data.UnitStateIdNext;
			this.stunningUnit = enchant.Provider;
		}
		if (enchant.Data.MakeUnitVisible)
		{
			this.UnitAlwaysVisible = true;
		}
	}

	public bool HasEnchantment(global::EnchantmentId enchantmentId)
	{
		global::EnchantmentData enchantmentData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::EnchantmentData>((int)enchantmentId);
		if (this.enchantTypeImmunities.IndexOf(enchantmentData.EnchantmentTypeId, global::EnchantmentTypeIdComparer.Instance) != -1)
		{
			return false;
		}
		bool flag = this.Items[(int)this.ActiveWeaponSlot].HasEnchantment(enchantmentId) || this.Items[(int)(this.ActiveWeaponSlot + 1)].HasEnchantment(enchantmentId);
		int num = 0;
		while (num < this.Enchantments.Count && !flag)
		{
			if (this.Enchantments[num].Id == enchantmentId)
			{
				flag = true;
			}
			num++;
		}
		return flag;
	}

	public bool HasEnchantment(global::EnchantmentTypeId typeId)
	{
		if (this.enchantTypeImmunities.IndexOf(typeId, global::EnchantmentTypeIdComparer.Instance) != -1)
		{
			return false;
		}
		for (int i = 0; i < this.Enchantments.Count; i++)
		{
			if (this.Enchantments[i].Data.EnchantmentTypeId == typeId)
			{
				return true;
			}
		}
		for (int j = 0; j < this.ActiveItems.Count; j++)
		{
			for (int k = 0; k < this.ActiveItems[j].Enchantments.Count; k++)
			{
				if (this.ActiveItems[j].Enchantments[k].Data.EnchantmentTypeId == typeId)
				{
					return true;
				}
			}
		}
		for (int l = 0; l < this.Injuries.Count; l++)
		{
			for (int m = 0; m < this.Injuries[l].Enchantments.Count; m++)
			{
				if (this.Injuries[l].Enchantments[m].Data.EnchantmentTypeId == typeId)
				{
					return true;
				}
			}
		}
		for (int n = 0; n < this.Mutations.Count; n++)
		{
			for (int num = 0; num < this.Mutations[n].Enchantments.Count; num++)
			{
				if (this.Mutations[n].Enchantments[num].Data.EnchantmentTypeId == typeId)
				{
					return true;
				}
			}
		}
		return false;
	}

	public bool HasEnchantmentRollEffect(global::System.Collections.Generic.List<global::Enchantment> enchantments, global::AttributeId attributeId)
	{
		for (int i = 0; i < enchantments.Count; i++)
		{
			for (int j = 0; j < enchantments[i].Effects.Count; j++)
			{
				if (enchantments[i].Effects[j].AttributeIdRoll == attributeId)
				{
					return true;
				}
			}
		}
		return false;
	}

	public global::Enchantment AddEnchantment(global::EnchantmentId id, global::Unit provider, bool original, bool updateAttributes = true, global::AllegianceId allegianceId = global::AllegianceId.NONE)
	{
		this.RefreshImmunities();
		global::EnchantmentData enchantmentData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::EnchantmentData>((int)id);
		if (enchantmentData.RequireUnitState && this.Status != enchantmentData.UnitStateIdRequired)
		{
			return null;
		}
		if (this.HasEnchantmentImmunity(enchantmentData.EnchantmentTypeId, enchantmentData.Id))
		{
			return null;
		}
		int num = -1;
		for (int i = 0; i < this.Enchantments.Count; i++)
		{
			if (this.Enchantments[i].Id == id)
			{
				num = i;
			}
		}
		if (!enchantmentData.Stackable && num != -1)
		{
			this.RemoveEnchantment(num);
		}
		global::Enchantment enchantment = new global::Enchantment(id, this, provider, original, false, allegianceId, num == -1);
		this.Enchantments.Add(enchantment);
		if (global::PandoraSingleton<global::MissionManager>.Exists())
		{
			global::UnitController unitController = global::PandoraSingleton<global::MissionManager>.Instance.GetUnitController(this, false);
			global::PandoraSingleton<global::MissionManager>.Instance.MissionEndData.UpdateUnit(unitController);
		}
		if (updateAttributes)
		{
			this.UpdateAttributes();
		}
		this.HasEnchantmentsChanged = true;
		global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<global::Unit>(global::Notices.UNIT_ENCHANTMENTS_CHANGED, this);
		return enchantment;
	}

	public void ConsumeEnchantments(global::EnchantmentConsumeId consumeId)
	{
		bool flag = false;
		global::System.Collections.Generic.List<global::EnchantmentId> list = new global::System.Collections.Generic.List<global::EnchantmentId>();
		for (int i = this.Enchantments.Count - 1; i >= 0; i--)
		{
			if (this.Enchantments[i].Data.EnchantmentConsumeId == consumeId && list.IndexOf(this.Enchantments[i].Id, global::EnchantmentIdComparer.Instance) == -1)
			{
				list.Add(this.Enchantments[i].Id);
				this.RemoveEnchantment(i);
				flag = true;
			}
		}
		if (flag)
		{
			this.UpdateAttributes();
		}
		this.HasEnchantmentsChanged = true;
		global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<global::Unit>(global::Notices.UNIT_ENCHANTMENTS_CHANGED, this);
	}

	public void DestroyEnchantments(global::EnchantmentTriggerId triggerId, bool updateAttributes = false)
	{
		bool flag = false;
		for (int i = 0; i < this.Enchantments.Count; i++)
		{
			if (this.Enchantments[i].Data.EnchantmentTriggerIdDestroy == triggerId)
			{
				flag = true;
				this.RemoveEnchantment(i);
			}
		}
		if (flag && updateAttributes)
		{
			this.UpdateAttributes();
		}
		this.HasEnchantmentsChanged = true;
		global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<global::Unit>(global::Notices.UNIT_ENCHANTMENTS_CHANGED, this);
	}

	public void RemoveEnchantments(global::SkillId skillId, global::Unit unit)
	{
		global::System.Collections.Generic.List<global::SkillEnchantmentData> list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::SkillEnchantmentData>("fk_skill_id", ((int)skillId).ToConstantString());
		for (int i = 0; i < list.Count; i++)
		{
			this.RemoveEnchantment(list[i].EnchantmentId, unit);
		}
	}

	public void RemoveEnchantments(global::EnchantmentTypeId typeId)
	{
		for (int i = this.Enchantments.Count - 1; i >= 0; i--)
		{
			if (this.Enchantments[i].Data.EnchantmentTypeId == typeId)
			{
				this.RemoveEnchantment(i);
			}
		}
	}

	public void RemoveEnchantments(global::EnchantmentId id)
	{
		for (int i = this.Enchantments.Count - 1; i >= 0; i--)
		{
			if (this.Enchantments[i].Id == id)
			{
				this.RemoveEnchantment(i);
			}
		}
	}

	public void RemoveEnchantment(global::EnchantmentId id, global::Unit unit)
	{
		for (int i = this.Enchantments.Count - 1; i >= 0; i--)
		{
			if (this.Enchantments[i].Id == id && (unit == null || this.Enchantments[i].Provider == unit))
			{
				this.RemoveEnchantment(i);
				break;
			}
		}
	}

	public void RemoveEnchantment(int i)
	{
		this.Enchantments[i].RemoveFx();
		this.Enchantments.RemoveAt(i);
		this.needFxRefresh = true;
		if (global::PandoraSingleton<global::MissionManager>.Exists())
		{
			global::UnitController unitController = global::PandoraSingleton<global::MissionManager>.Instance.GetUnitController(this, false);
			global::PandoraSingleton<global::MissionManager>.Instance.MissionEndData.UpdateUnit(unitController);
		}
		this.HasEnchantmentsChanged = true;
		global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<global::Unit>(global::Notices.UNIT_ENCHANTMENTS_CHANGED, this);
	}

	public void RemoveAppliedEnchantments()
	{
		for (int i = this.Enchantments.Count - 1; i >= 0; i--)
		{
			if (this.Enchantments[i].Data.DestroyOnApply)
			{
				this.RemoveEnchantment(i);
			}
		}
	}

	public int GetEnchantmentDamage(global::Tyche tyche, global::EnchantmentDmgTriggerId dmgTriggerId)
	{
		global::System.Collections.Generic.List<global::Tuple<global::Enchantment, int>> enchantmentDamages = this.GetEnchantmentDamages(tyche, dmgTriggerId);
		int num = 0;
		for (int i = 0; i < enchantmentDamages.Count; i++)
		{
			num += enchantmentDamages[i].Item2;
		}
		return num;
	}

	public global::System.Collections.Generic.List<global::Tuple<global::Enchantment, int>> GetEnchantmentDamages(global::Tyche tyche, global::EnchantmentDmgTriggerId dmgTriggerId)
	{
		global::System.Collections.Generic.List<global::Tuple<global::Enchantment, int>> list = new global::System.Collections.Generic.List<global::Tuple<global::Enchantment, int>>();
		for (int i = 0; i < this.Enchantments.Count; i++)
		{
			if (this.Enchantments[i].Data.EnchantmentDmgTriggerId == dmgTriggerId && (dmgTriggerId != global::EnchantmentDmgTriggerId.ON_APPLY || !this.Enchantments[i].damageApplied) && (this.Enchantments[i].Data.Indestructible || (this.enchantTypeImmunities.IndexOf(this.Enchantments[i].Data.EnchantmentTypeId, global::EnchantmentTypeIdComparer.Instance) == -1 && this.enchantTypeToBeRemoved.IndexOf(this.Enchantments[i].Data.EnchantmentTypeId, global::EnchantmentTypeIdComparer.Instance) == -1 && this.enchantToBeRemoved.IndexOf(this.Enchantments[i].Id, global::EnchantmentIdComparer.Instance) == -1)))
			{
				bool flag = false;
				this.Enchantments[i].damageApplied = true;
				if (this.Enchantments[i].Data.AttributeIdDmgResistRoll != global::AttributeId.NONE)
				{
					int num = this.GetAttribute(this.Enchantments[i].Data.AttributeIdDmgResistRoll);
					if (this.Enchantments[i].Provider != null)
					{
						num += this.Enchantments[i].Provider.GetAttributeModifier(this.Enchantments[i].Data.AttributeIdDmgResistRoll);
					}
					flag = this.Roll(global::PandoraSingleton<global::MissionManager>.Instance.NetworkTyche, num, this.Enchantments[i].Data.AttributeIdDmgResistRoll, false, false, 0);
				}
				int item = 0;
				if (!flag)
				{
					item = tyche.Rand(this.Enchantments[i].Data.DamageMin, this.Enchantments[i].Data.DamageMax);
				}
				list.Add(new global::Tuple<global::Enchantment, int>(this.Enchantments[i], item));
			}
		}
		return list;
	}

	public void UpdateEnchantmentsDuration(global::Unit currentUnit, bool updateAttributes = true)
	{
		bool flag = false;
		for (int i = this.Enchantments.Count - 1; i >= 0; i--)
		{
			if (this.Enchantments[i].UpdateDuration(currentUnit))
			{
				this.RemoveEnchantment(i);
				flag = true;
			}
		}
		for (int j = 0; j < this.Items.Count; j++)
		{
			flag |= this.Items[j].UpdateEnchantmentsDuration(currentUnit);
		}
		if (flag && updateAttributes)
		{
			this.UpdateAttributes();
		}
	}

	public void UpdateEnchantments()
	{
		bool flag = false;
		for (int i = this.Enchantments.Count - 1; i >= 0; i--)
		{
			if (this.Enchantments[i].UpdateStatus(this.Status))
			{
				this.RemoveEnchantment(i);
				flag = true;
			}
		}
		for (int j = 0; j < this.Items.Count; j++)
		{
			flag |= this.Items[j].UpdateEnchantments(this.Status);
		}
		if (flag)
		{
			this.UpdateAttributes();
		}
	}

	public void UpdateValidNextActionEnchantments()
	{
		for (int i = this.Enchantments.Count - 1; i >= 0; i--)
		{
			if (this.Enchantments[i].UpdateValidNextAction())
			{
				this.RemoveEnchantment(i);
			}
		}
	}

	public void UpdateEnchantmentsFx()
	{
		this.needFxRefresh = false;
		global::System.Collections.Generic.List<global::EnchantmentId> list = new global::System.Collections.Generic.List<global::EnchantmentId>();
		for (int i = 0; i < this.Enchantments.Count; i++)
		{
			if (this.Enchantments[i].HasFx && this.Enchantments[i].fxSpawned)
			{
				list.Add(this.Enchantments[i].Id);
			}
		}
		for (int j = 0; j < this.Enchantments.Count; j++)
		{
			if (this.Enchantments[j].HasFx && !this.Enchantments[j].fxSpawned && list.IndexOf(this.Enchantments[j].Id, global::EnchantmentIdComparer.Instance) == -1)
			{
				list.Add(this.Enchantments[j].Id);
				this.Enchantments[j].SpawnFx(this);
			}
		}
	}

	public void CleanEnchantments()
	{
		for (int i = this.Enchantments.Count - 1; i >= 0; i--)
		{
			if (!this.Enchantments[i].Innate && !this.Enchantments[i].Data.KeepOnDeath)
			{
				this.RemoveEnchantment(i);
			}
		}
	}

	public void ApplyTurnStartEnchantments()
	{
		for (int i = this.Enchantments.Count - 1; i >= 0; i--)
		{
			global::EnchantmentId enchantmentIdOnTurnStart = this.Enchantments[i].EnchantmentIdOnTurnStart;
			if (enchantmentIdOnTurnStart != global::EnchantmentId.NONE)
			{
				this.AddEnchantment(enchantmentIdOnTurnStart, this, false, true, global::AllegianceId.NONE);
			}
		}
	}

	private void ApplyItem(global::Item item)
	{
		for (int i = 0; i < item.AttributeModifiers.Count; i++)
		{
			global::ItemAttributeData itemAttributeData = item.AttributeModifiers[i];
			if ((itemAttributeData.UnitActionIdTrigger == global::UnitActionId.NONE && itemAttributeData.SkillIdTrigger == global::SkillId.NONE) || (this.ActiveSkill != null && this.ActiveSkill.UnitActionId != global::UnitActionId.NONE && this.ActiveSkill.UnitActionId == itemAttributeData.UnitActionIdTrigger) || (this.ActiveSkill != null && this.ActiveSkill.Id != global::SkillId.NONE && this.ActiveSkill.Id == itemAttributeData.SkillIdTrigger))
			{
				this.AddToAttribute(itemAttributeData.AttributeId, itemAttributeData.Modifier);
				this.AddAttributeModifier(global::AttributeMod.Type.ITEM, itemAttributeData.AttributeId, global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(item.LabelName), itemAttributeData.Modifier, false);
			}
		}
		for (int j = 0; j < item.Enchantments.Count; j++)
		{
			this.ApplyEnchantment(item.Enchantments[j]);
		}
		if (item.RuneMark != null)
		{
			this.ApplyRuneMark(item.RuneMark);
		}
	}

	public global::ItemId GetItemId(global::UnitSlotId slot)
	{
		return this.Items[(int)slot].Id;
	}

	public void SetItems()
	{
		if ((!global::PandoraSingleton<global::MissionStartData>.Exists() || !global::PandoraSingleton<global::MissionStartData>.Instance.isReload) && this.UnitSave.campaignId != 0)
		{
			this.SetCampaignItems();
		}
		else
		{
			bool flag = true;
			for (int i = 0; i < this.UnitSave.items.Count; i++)
			{
				if (this.UnitSave.items[i] != null && this.UnitSave.items[i].id != 0)
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				this.InitBaseEquipment();
			}
			this.SetSaveItems();
		}
	}

	private void InitBaseEquipment()
	{
		for (int i = 0; i < 13; i++)
		{
			global::System.Collections.Generic.List<global::UnitBaseItemData> list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::UnitBaseItemData>(new string[]
			{
				"fk_unit_id",
				"fk_unit_slot_id"
			}, new string[]
			{
				((int)this.Id).ToConstantString(),
				i.ToConstantString()
			});
			if (list != null && list.Count > 0)
			{
				global::UnitBaseItemData unitBaseItemData = list[0];
				global::MutationId mutationId = this.GetMutationId((global::UnitSlotId)i);
				this.UnitSave.items[(int)unitBaseItemData.UnitSlotId] = new global::ItemSave((mutationId != global::MutationId.NONE) ? unitBaseItemData.ItemIdMutation : unitBaseItemData.ItemId, global::ItemQualityId.NORMAL, global::RuneMarkId.NONE, global::RuneMarkQualityId.NONE, global::AllegianceId.NONE, 1);
			}
		}
	}

	private void SetSaveItems()
	{
		for (int i = 0; i < this.UnitSave.items.Count; i++)
		{
			if (i < this.Items.Count && this.UnitSave.items[i] != null)
			{
				this.Items[i] = new global::Item(this.UnitSave.items[i]);
			}
		}
		if (this.CampaignData != null)
		{
			return;
		}
		for (int j = 0; j < 6; j++)
		{
			if (this.UnitSave.items[j] == null)
			{
				this.EquipItem((global::UnitSlotId)j, global::ItemId.NONE);
			}
			else
			{
				this.EquipItem((global::UnitSlotId)j, this.UnitSave.items[j]);
			}
		}
	}

	private void SetCampaignItems()
	{
		global::System.Collections.Generic.List<global::CampaignUnitJoinItemData> list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::CampaignUnitJoinItemData>("fk_campaign_unit_id", ((int)this.CampaignData.Id).ToConstantString());
		for (int i = 0; i < list.Count; i++)
		{
			global::CampaignUnitJoinItemData campaignUnitJoinItemData = list[i];
			this.Items[(int)campaignUnitJoinItemData.UnitSlotId] = new global::Item(campaignUnitJoinItemData.ItemId, campaignUnitJoinItemData.ItemQualityId);
			this.Items[(int)campaignUnitJoinItemData.UnitSlotId].owner = this;
			if (campaignUnitJoinItemData.RuneMarkId != global::RuneMarkId.NONE)
			{
				this.Items[(int)campaignUnitJoinItemData.UnitSlotId].AddRuneMark(campaignUnitJoinItemData.RuneMarkId, campaignUnitJoinItemData.RuneMarkQualityId, this.AllegianceId);
			}
			this.UnitSave.items[(int)campaignUnitJoinItemData.UnitSlotId] = this.Items[(int)campaignUnitJoinItemData.UnitSlotId].Save;
		}
	}

	public bool GetEmptyItemSlot(out global::UnitSlotId slotId, global::Item item)
	{
		slotId = global::UnitSlotId.NB_SLOTS;
		int i = 6;
		while (i < this.Items.Count)
		{
			if (this.GetItemId((global::UnitSlotId)i) == global::ItemId.NONE || (item.IsStackable && this.GetItemId((global::UnitSlotId)i) == item.Id))
			{
				slotId = (global::UnitSlotId)i;
				return true;
			}
			i++;
			slotId++;
		}
		slotId = global::UnitSlotId.NB_SLOTS;
		return false;
	}

	public void CacheBackpackSize()
	{
		this.cachedBackpackCapacity = this.realBackPackCapacity;
	}

	public int GetNumUsedItemSlot()
	{
		return this.BackpackCapacity - this.GetNumEmptyItemSlot();
	}

	public int GetNumEmptyItemSlot()
	{
		int num = 0;
		for (int i = 6; i < this.Items.Count; i++)
		{
			if (this.Items[i].Id == global::ItemId.NONE)
			{
				num++;
			}
		}
		return num;
	}

	private void ReequipWeapons(global::System.Collections.Generic.List<global::Item> removedItems)
	{
		for (int i = 2; i <= 5; i++)
		{
			removedItems.AddRange(this.EquipItem((global::UnitSlotId)i, this.Items[i], true));
		}
	}

	public global::System.Collections.Generic.List<global::Item> EquipItem(global::UnitSlotId slot, global::ItemId itemId)
	{
		return this.EquipItem(slot, new global::ItemSave(itemId, global::ItemQualityId.NORMAL, global::RuneMarkId.NONE, global::RuneMarkQualityId.NONE, global::AllegianceId.NONE, 1));
	}

	public global::System.Collections.Generic.List<global::Item> EquipItem(global::UnitSlotId slot, global::ItemSave itemSave)
	{
		return this.EquipItem(slot, new global::Item(itemSave), true);
	}

	public global::System.Collections.Generic.List<global::Item> EquipItem(global::UnitSlotId slot, global::Item item, bool sortItems = true)
	{
		global::System.Collections.Generic.List<global::Item> result = new global::System.Collections.Generic.List<global::Item>();
		global::MutationId mutationId = this.GetMutationId(slot);
		item = this.SetItem(slot, item, mutationId, ref result);
		if (item.Id != global::ItemId.NONE && slot < global::UnitSlotId.ITEM_1)
		{
			bool flag = false;
			if (slot == global::UnitSlotId.SET1_MAINHAND || slot == global::UnitSlotId.SET2_MAINHAND)
			{
				for (int i = 0; i < item.Enchantments.Count; i++)
				{
					if (item.Enchantments[i].Id == global::EnchantmentId.ITEM_UNWIELDY)
					{
						flag = true;
						break;
					}
				}
			}
			bool flag2 = (slot == global::UnitSlotId.SET1_MAINHAND || slot == global::UnitSlotId.SET2_MAINHAND) && this.Items[(int)(slot + 1)].Id == global::ItemId.NONE;
			if (item.IsPaired || flag2 || item.IsTwoHanded || (flag && this.Items[(int)(slot + 1)] != null && this.Items[(int)(slot + 1)].Id != global::ItemId.NONE && this.Items[(int)(slot + 1)].TypeData.Id != global::ItemTypeId.SHIELD))
			{
				this.SetItem(slot + 1, new global::Item(global::ItemId.NONE, global::ItemQualityId.NORMAL), mutationId, ref result);
			}
			if (flag && this.GetMutationId(slot + 1) != global::MutationId.NONE)
			{
				this.SetItem(slot, new global::Item(global::ItemId.NONE, global::ItemQualityId.NORMAL), mutationId, ref result);
			}
			if (item.IsLockSlot && (slot == global::UnitSlotId.SET1_MAINHAND || slot == global::UnitSlotId.SET1_OFFHAND))
			{
				global::System.Collections.Generic.List<global::Item> list = new global::System.Collections.Generic.List<global::Item>(0);
				global::ItemSave itemSave = new global::ItemSave(global::ItemId.NONE, global::ItemQualityId.NORMAL, global::RuneMarkId.NONE, global::RuneMarkQualityId.NONE, global::AllegianceId.NONE, 1);
				global::Thoth.Copy(item.Save, itemSave);
				this.SetItem(slot + 2, new global::Item(itemSave), mutationId, ref list);
			}
		}
		else if (item.Id == global::ItemId.NONE && slot == global::UnitSlotId.SET2_MAINHAND)
		{
			this.SetItem(global::UnitSlotId.SET2_OFFHAND, new global::Item(global::ItemId.NONE, global::ItemQualityId.NORMAL), this.GetMutationId(global::UnitSlotId.SET2_OFFHAND), ref result);
		}
		if (sortItems && slot >= global::UnitSlotId.ITEM_1)
		{
			global::Item.SortEmptyItems(this.items, 6);
			for (int j = 6; j < this.items.Count; j++)
			{
				this.UnitSave.items[j] = this.items[j].Save;
			}
		}
		return result;
	}

	public void CheckItemsAchievments()
	{
		bool flag = true;
		bool flag2 = true;
		global::ItemQualityId itemQualityId = global::ItemQualityId.NONE;
		global::RuneMarkQualityId runeMarkQualityId = global::RuneMarkQualityId.NONE;
		for (int i = 0; i < 6; i++)
		{
			if ((i != 3 && i != 5) || (!this.Items[i - 1].IsPaired && !this.Items[i - 1].IsTwoHanded))
			{
				if (this.Items[i].Id == global::ItemId.NONE || (itemQualityId != global::ItemQualityId.NONE && this.Items[i].QualityData.Id != itemQualityId) || this.Items[i].Backup)
				{
					flag = false;
				}
				else if (flag)
				{
					itemQualityId = this.Items[i].QualityData.Id;
				}
				if (this.Items[i].Id == global::ItemId.NONE || this.Items[i].RuneMark == null || (runeMarkQualityId != global::RuneMarkQualityId.NONE && this.Items[i].RuneMark.QualityData.Id != runeMarkQualityId))
				{
					flag2 = false;
				}
				else if (flag2)
				{
					runeMarkQualityId = this.Items[i].RuneMark.QualityData.Id;
				}
			}
		}
		if (flag)
		{
			global::Hephaestus.TrophyId achievement = global::Hephaestus.TrophyId.NORMAL_EQUIP;
			global::ItemQualityId itemQualityId2 = itemQualityId;
			if (itemQualityId2 != global::ItemQualityId.GOOD)
			{
				if (itemQualityId2 == global::ItemQualityId.BEST)
				{
					achievement = global::Hephaestus.TrophyId.BEST_EQUIP;
				}
			}
			else
			{
				achievement = global::Hephaestus.TrophyId.GOOD_EQUIP;
			}
			global::PandoraSingleton<global::Hephaestus>.Instance.UnlockAchievement(achievement);
		}
		if (flag2)
		{
			global::PandoraSingleton<global::Hephaestus>.Instance.UnlockAchievement((runeMarkQualityId != global::RuneMarkQualityId.REGULAR) ? global::Hephaestus.TrophyId.ENCHANT_EQUIP_2 : global::Hephaestus.TrophyId.ENCHANT_EQUIP_1);
		}
	}

	private global::Item SetItem(global::UnitSlotId slot, global::Item item, global::MutationId mutationId, ref global::System.Collections.Generic.List<global::Item> previousItems)
	{
		if (this.Items[(int)slot].IsUndroppable)
		{
			previousItems.Add(item);
			return new global::Item(global::ItemId.NONE, global::ItemQualityId.NORMAL);
		}
		global::UnitSlotData unitSlotData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::UnitSlotData>((int)slot);
		bool flag = this.IsBoneBlocked(unitSlotData.BoneId);
		if (slot < global::UnitSlotId.ITEM_1)
		{
			if (flag || this.IsItemTypeBlocked(item.TypeData.Id))
			{
				if (item.Id != global::ItemId.NONE)
				{
					previousItems.Add(item);
				}
				item = new global::Item(global::ItemId.NONE, global::ItemQualityId.NORMAL);
			}
			global::System.Collections.Generic.List<global::ItemUnitData> list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::ItemUnitData>(new string[]
			{
				"fk_unit_id",
				"mutation"
			}, new string[]
			{
				((int)this.Id).ToConstantString(),
				(mutationId == global::MutationId.NONE) ? "0" : "1"
			});
			if (list.Find((global::ItemUnitData x) => x.ItemId == item.Id) == null)
			{
				item = new global::Item(global::ItemId.NONE, global::ItemQualityId.NORMAL);
			}
			bool flag2 = false;
			if (item.IsPaired || item.IsTwoHanded)
			{
				global::UnitSlotData unitSlotData2 = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::UnitSlotData>((int)slot);
				flag2 = this.blockedBones.ContainsKey(unitSlotData2.BoneId);
				if (item.Id != global::ItemId.NONE && (slot == global::UnitSlotId.SET1_MAINHAND || slot == global::UnitSlotId.SET2_MAINHAND) && (this.GetMutationId(slot + 1) != global::MutationId.NONE || this.GetInjury(slot + 1) != global::InjuryId.NONE))
				{
					previousItems.Add(item);
					item = new global::Item(global::ItemId.NONE, global::ItemQualityId.NORMAL);
				}
			}
			if (item.Id == global::ItemId.NONE && !flag && ((slot != global::UnitSlotId.SET1_OFFHAND && slot != global::UnitSlotId.SET2_OFFHAND) || ((slot == global::UnitSlotId.SET1_OFFHAND || slot == global::UnitSlotId.SET2_OFFHAND) && !this.Items[slot - global::UnitSlotId.ARMOR].IsPaired && !this.Items[slot - global::UnitSlotId.ARMOR].IsTwoHanded && this.Items[slot - global::UnitSlotId.ARMOR].Id != global::ItemId.NONE) || flag2))
			{
				global::System.Collections.Generic.List<global::UnitDefaultItemData> list2 = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::UnitDefaultItemData>(new string[]
				{
					"fk_unit_id",
					"fk_unit_slot_id"
				}, new string[]
				{
					((int)this.Id).ToConstantString(),
					((int)slot).ToConstantString()
				});
				if (list2.Count != 0)
				{
					item = new global::Item((mutationId == global::MutationId.NONE) ? list2[0].ItemId : list2[0].ItemIdMutation, global::ItemQualityId.NORMAL);
				}
			}
		}
		if (item.ConsumableData != null && item.ConsumableData.OutOfCombat && !global::PandoraSingleton<global::MissionManager>.Exists())
		{
			this.UnitSave.consumableSkills.Add(item.ConsumableData.SkillId);
			this.AddSkill(item.ConsumableData.SkillId, false);
			return item;
		}
		global::Item item2 = this.Items[(int)slot];
		if (!item.IsTrophy)
		{
			global::ItemId id = item.Id;
			if (id != global::ItemId.WYRDSTONE_SHARD && id != global::ItemId.WYRDSTONE_CLUSTER && id != global::ItemId.WYRDSTONE_FRAGMENT)
			{
				item.owner = this;
			}
			else if (item.owner == null || item.owner.Status == global::UnitStateId.OUT_OF_ACTION || item.owner.warbandIdx != this.warbandIdx)
			{
				item.owner = this;
			}
		}
		item.SetModifiers(mutationId);
		if (item2 != null && item2.IsStackable && item2.Id == item.Id)
		{
			item.Save.amount += item2.Save.amount;
		}
		else if (item2 != null && item2.Id != global::ItemId.NONE && !item2.Backup)
		{
			previousItems.Add(item2);
		}
		this.UnitSave.items[(int)slot] = item.Save;
		this.Items[(int)slot] = item;
		this.SetBodyPartItemLock(item2, true);
		this.SetBodyPartsSlot(slot);
		return item;
	}

	public global::System.Collections.Generic.List<global::Item> UnequipAllItems()
	{
		global::System.Collections.Generic.List<global::Item> list = new global::System.Collections.Generic.List<global::Item>();
		for (int i = 0; i < this.Items.Count; i++)
		{
			global::UnitSlotId unitSlotId = (global::UnitSlotId)i;
			global::Item item = this.Items[i];
			if (item != null && ((unitSlotId == global::UnitSlotId.SET1_OFFHAND && !this.Items[i - 1].IsPaired) || unitSlotId != global::UnitSlotId.SET1_OFFHAND) && ((unitSlotId == global::UnitSlotId.SET2_MAINHAND && !this.Items[i - 2].IsLockSlot) || unitSlotId != global::UnitSlotId.SET2_MAINHAND) && ((unitSlotId == global::UnitSlotId.SET2_MAINHAND && this.GetMutationId(unitSlotId) == global::MutationId.NONE) || unitSlotId != global::UnitSlotId.SET2_MAINHAND) && ((unitSlotId == global::UnitSlotId.SET2_OFFHAND && this.GetMutationId(unitSlotId) == global::MutationId.NONE) || unitSlotId != global::UnitSlotId.SET2_OFFHAND) && ((unitSlotId == global::UnitSlotId.SET2_OFFHAND && !this.Items[i - 2].IsLockSlot) || unitSlotId != global::UnitSlotId.SET2_OFFHAND) && ((unitSlotId == global::UnitSlotId.SET2_OFFHAND && !this.Items[i - 1].IsPaired) || unitSlotId != global::UnitSlotId.SET2_OFFHAND))
			{
				list.Add(item);
			}
		}
		for (int j = 0; j < 13; j++)
		{
			this.UnitSave.items[j] = null;
		}
		this.SetItems();
		return list;
	}

	public bool HasItem(global::Item item)
	{
		for (int i = 0; i < this.Items.Count; i++)
		{
			if (this.Items[i].IsSame(item))
			{
				return true;
			}
		}
		return false;
	}

	public bool HasItem(global::ItemId id, global::ItemQualityId qualityId = global::ItemQualityId.NONE)
	{
		for (int i = 0; i < this.Items.Count; i++)
		{
			if (this.Items[i].Id == id && (qualityId == global::ItemQualityId.NONE || this.Items[i].QualityData.Id == qualityId))
			{
				return true;
			}
		}
		return false;
	}

	public bool HasItem(global::ItemTypeId typeId)
	{
		for (int i = 0; i < this.Items.Count; i++)
		{
			if (this.Items[i].TypeData.Id == typeId)
			{
				return true;
			}
		}
		return false;
	}

	public void RefreshActiveItems()
	{
		this.ActiveItems.Clear();
		for (int i = 0; i < 6; i++)
		{
			global::Item item = this.Items[i];
			if (item.Id != global::ItemId.NONE && i != (int)this.InactiveWeaponSlot && i != (int)(this.InactiveWeaponSlot + 1) && ((item.IsPaired && i != 3 && i != 5) || !item.IsPaired))
			{
				this.ActiveItems.Add(item);
			}
		}
	}

	public bool HasItemActive(global::ItemId id)
	{
		for (int i = 0; i < this.ActiveItems.Count; i++)
		{
			if (this.ActiveItems[i].Id == id)
			{
				return true;
			}
		}
		return false;
	}

	public bool IsInventoryFull()
	{
		bool result = true;
		for (int i = 6; i < this.Items.Count; i++)
		{
			if (this.Items[i].Id == global::ItemId.NONE)
			{
				result = false;
				break;
			}
		}
		return result;
	}

	public bool IsInventoryEmpty()
	{
		bool result = true;
		for (int i = 6; i < this.Items.Count; i++)
		{
			if (this.Items[i].Id != global::ItemId.NONE)
			{
				result = false;
				break;
			}
		}
		return result;
	}

	public void ConsumeItem(global::ItemId itemId)
	{
		for (int i = 0; i < this.Items.Count; i++)
		{
			if (this.Items[i].Id == itemId)
			{
				this.EquipItem((global::UnitSlotId)i, global::ItemId.NONE);
				return;
			}
		}
	}

	public void RemoveItem(global::Item item)
	{
		for (int i = 0; i < this.Items.Count; i++)
		{
			if (this.Items[i].Id == item.Id && this.Items[i].QualityData.Id == item.QualityData.Id && ((this.Items[i].RuneMark == null && item.RuneMark == null) || (this.Items[i].RuneMark != null && item.RuneMark != null && this.Items[i].RuneMark.Data.Id == item.RuneMark.Data.Id && this.Items[i].RuneMark.QualityData.Id == item.RuneMark.QualityData.Id)))
			{
				this.EquipItem((global::UnitSlotId)i, global::ItemId.NONE);
				return;
			}
		}
	}

	public bool WeaponSlotsLocked()
	{
		return (this.Items[2].IsLockSlot && this.Items[3].IsLockSlot) || (this.Items[2].IsLockSlot && this.Items[2].IsTwoHanded);
	}

	public bool BothArmsMutated()
	{
		return this.GetMutationId(global::UnitSlotId.SET1_MAINHAND) != global::MutationId.NONE && this.GetMutationId(global::UnitSlotId.SET1_OFFHAND) != global::MutationId.NONE;
	}

	public bool HasMutatedArm()
	{
		return this.GetMutationId(global::UnitSlotId.SET1_MAINHAND) != global::MutationId.NONE || this.GetMutationId(global::UnitSlotId.SET1_OFFHAND) != global::MutationId.NONE;
	}

	public bool CanSwitchWeapon()
	{
		return this.Items[(int)this.InactiveWeaponSlot].Id != global::ItemId.NONE && !this.WeaponSlotsLocked();
	}

	public global::AttributeModList GetSpellDamageModifier(global::SkillId skillId, global::Unit target, global::SpellTypeId spellType, bool bypassArmor = false)
	{
		this.weaponDamageModifiers.Clear();
		string stringById = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("attribute_name_damage");
		if (this.DamageBonusSpell != 0)
		{
			this.weaponDamageModifiers.AddRange(this.attributeModifiers.GetOrNull(global::AttributeId.DAMAGE_BONUS_SPELL), stringById, false, false, false);
		}
		if (spellType == global::SpellTypeId.ARCANE)
		{
			if (this.DamageBonusArcMagPerc != 0)
			{
				this.weaponDamageModifiers.AddRange(this.attributeModifiers.GetOrNull(global::AttributeId.DAMAGE_BONUS_ARC_MAG_PERC), stringById, true, false, false);
			}
		}
		else if (spellType == global::SpellTypeId.DIVINE && this.DamageBonusDivMagPerc != 0)
		{
			this.weaponDamageModifiers.AddRange(this.attributeModifiers.GetOrNull(global::AttributeId.DAMAGE_BONUS_DIV_MAG_PERC), stringById, true, false, false);
		}
		if (target != null && target.damagePercModifiers.ContainsKey(skillId))
		{
			this.weaponDamageModifiers.Add(new global::AttributeMod(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("attribute_name_damage_bonus_spell_perc", new string[]
			{
				target.damagePercModifiers[skillId].ToString()
			})), null, false, false, false);
		}
		if (bypassArmor)
		{
			this.weaponDamageModifiers.Add(new global::AttributeMod(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("attribute_ignore_armor")), null, false, false, false);
		}
		else
		{
			if (target != null && target.ArmorAbsorption != 0)
			{
				this.weaponDamageModifiers.AddRange(target.attributeModifiers.GetOrNull(global::AttributeId.ARMOR_ABSORPTION), global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("attribute_name_armor_absorption"), false, true, true);
			}
			if (target != null && target.ArmorAbsorptionPerc != 0)
			{
				this.weaponDamageModifiers.AddRange(target.attributeModifiers.GetOrNull(global::AttributeId.ARMOR_ABSORPTION_PERC), global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("attribute_name_armor_absorption_perc"), true, true, true);
			}
		}
		return this.weaponDamageModifiers;
	}

	public int ApplySpellDamageModifier(global::SkillId skillId, int damage, global::Unit target, global::SpellTypeId spellType, bool byPassArmor)
	{
		int num = 0;
		float num2 = 100f;
		num += this.DamageBonusSpell;
		if (spellType == global::SpellTypeId.ARCANE)
		{
			num2 += (float)this.DamageBonusArcMagPerc;
		}
		else if (spellType == global::SpellTypeId.DIVINE)
		{
			num2 += (float)this.DamageBonusDivMagPerc;
		}
		if (target != null && target.damagePercModifiers.ContainsKey(skillId))
		{
			num2 += (float)target.damagePercModifiers[skillId];
		}
		int num3 = global::PandoraUtils.Round((float)(damage + num) * (num2 / 100f));
		if (target != null && !byPassArmor)
		{
			float num4 = (float)global::UnityEngine.Mathf.Max(target.ArmorAbsorptionPerc, 0);
			num3 -= global::UnityEngine.Mathf.Max(target.ArmorAbsorption, 0);
			num3 -= global::PandoraUtils.Round((float)num3 * num4 / 100f);
		}
		return (num3 <= 0) ? 0 : num3;
	}

	public global::AttributeModList GetWeaponDamageModifier(global::Unit target, bool bypassArmor = false, bool isCharging = false)
	{
		this.weaponDamageModifiers.Clear();
		bool flag = this.HasRange();
		this.weaponDamageModifiers.Add(new global::AttributeMod(global::AttributeMod.Type.BASE, global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("attribute_base_weapon_damage"), this.DamageMin, this.DamageMax), null, false, false, false);
		string stringById = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("attribute_name_damage");
		if (this.DamageBonus != 0)
		{
			this.weaponDamageModifiers.AddRange(this.attributeModifiers.GetOrNull(global::AttributeId.DAMAGE_BONUS), stringById, false, false, false);
		}
		string stringById2 = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("attribute_name_global_damage");
		if (flag)
		{
			if (this.DamageBonusRange != 0)
			{
				this.weaponDamageModifiers.AddRange(this.attributeModifiers.GetOrNull(global::AttributeId.DAMAGE_BONUS_RANGE), stringById, false, false, false);
			}
			if (this.DamageBonusRangePerc != 0)
			{
				this.weaponDamageModifiers.AddRange(this.attributeModifiers.GetOrNull(global::AttributeId.DAMAGE_BONUS_RANGE_PERC), stringById, true, false, false);
			}
			if (this.GlobalRangeDamagePerc != 0)
			{
				this.weaponDamageModifiers.AddRange(this.attributeModifiers.GetOrNull(global::AttributeId.GLOBAL_RANGE_DAMAGE_PERC), stringById2, true, false, false);
			}
		}
		else
		{
			if (this.DamageBonusMelee != 0)
			{
				this.weaponDamageModifiers.AddRange(this.attributeModifiers.GetOrNull(global::AttributeId.DAMAGE_BONUS_MELEE), stringById, false, false, false);
			}
			if (this.DamageBonusMeleePerc != 0)
			{
				this.weaponDamageModifiers.AddRange(this.attributeModifiers.GetOrNull(global::AttributeId.DAMAGE_BONUS_MELEE_PERC), stringById, true, false, false);
			}
			if (isCharging && this.DamageBonusChargePerc != 0)
			{
				this.weaponDamageModifiers.AddRange(this.attributeModifiers.GetOrNull(global::AttributeId.DAMAGE_BONUS_CHARGE_PERC), stringById, true, false, false);
			}
			if (this.GlobalMeleeDamagePerc != 0)
			{
				this.weaponDamageModifiers.AddRange(this.attributeModifiers.GetOrNull(global::AttributeId.GLOBAL_MELEE_DAMAGE_PERC), stringById2, true, false, false);
			}
		}
		if (target != null)
		{
			if (target.AllegianceId == global::AllegianceId.DESTRUCTION)
			{
				if (this.DamageHoly != 0)
				{
					this.weaponDamageModifiers.Add(new global::AttributeMod(global::AttributeMod.Type.NONE, global::AttributeId.DAMAGE_HOLY.ToLowerString(), this.DamageHoly, stringById, false, false), null, false, false, false);
				}
				if (this.DamageBonusHolyPerc != 0)
				{
					this.weaponDamageModifiers.Add(new global::AttributeMod(global::AttributeMod.Type.NONE, global::AttributeId.DAMAGE_BONUS_HOLY_PERC.ToLowerString(), this.DamageBonusHolyPerc, stringById, false, false), null, false, false, false);
				}
			}
			else
			{
				if (this.DamageUnholy != 0)
				{
					this.weaponDamageModifiers.Add(new global::AttributeMod(global::AttributeMod.Type.NONE, global::AttributeId.DAMAGE_UNHOLY.ToLowerString(), this.DamageUnholy, stringById, false, false), null, false, false, false);
				}
				if (this.DamageBonusUnholyPerc != 0)
				{
					this.weaponDamageModifiers.Add(new global::AttributeMod(global::AttributeMod.Type.NONE, global::AttributeId.DAMAGE_BONUS_UNHOLY_PERC.ToLowerString(), this.DamageBonusUnholyPerc, stringById, false, false), null, false, false, false);
				}
			}
		}
		if (bypassArmor)
		{
			this.weaponDamageModifiers.Add(new global::AttributeMod(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("attribute_ignore_armor")), null, false, false, false);
		}
		else
		{
			if (this.BypassArmor != 0)
			{
				this.weaponDamageModifiers.AddRange(this.attributeModifiers.GetOrNull(global::AttributeId.BYPASS_ARMOR), global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("attribute_name_bypass_armor"), false, false, false);
			}
			if (this.ByPassArmorPerc != 0)
			{
				this.weaponDamageModifiers.AddRange(this.attributeModifiers.GetOrNull(global::AttributeId.BYPASS_ARMOR_PERC), global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("attribute_name_bypass_armor_perc"), true, false, false);
			}
		}
		if (target != null && !bypassArmor)
		{
			if (target.ArmorAbsorption != 0)
			{
				this.weaponDamageModifiers.AddRange(target.attributeModifiers.GetOrNull(global::AttributeId.ARMOR_ABSORPTION), global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("attribute_name_armor_absorption"), false, true, true);
			}
			if (target.ArmorAbsorptionPerc != 0)
			{
				this.weaponDamageModifiers.AddRange(target.attributeModifiers.GetOrNull(global::AttributeId.ARMOR_ABSORPTION_PERC), global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("attribute_name_armor_absorption_perc"), true, true, true);
			}
		}
		return this.weaponDamageModifiers;
	}

	public int GetWeaponDamageMin(global::Unit target, bool critical = false, bool byPassArmor = false, bool charging = false)
	{
		return this.ApplyWeaponDamageModifier(this.DamageMin, target, critical, byPassArmor, charging);
	}

	public int GetWeaponDamageMax(global::Unit target, bool critical = false, bool byPassArmor = false, bool charging = false)
	{
		return this.ApplyWeaponDamageModifier(this.DamageMax, target, critical, byPassArmor, charging);
	}

	private int ApplyWeaponDamageModifier(int damage, global::Unit target, bool critical, bool byPassArmor, bool charging)
	{
		int num = 0;
		float num2 = 100f;
		bool flag = this.HasRange();
		num += this.DamageBonus;
		num += ((!flag) ? this.DamageBonusMelee : this.DamageBonusRange);
		num += ((!critical) ? 0 : this.DamageCriticalBonus);
		num2 += (float)((!flag) ? this.DamageBonusMeleePerc : this.DamageBonusRangePerc);
		num2 += (float)((!charging) ? 0 : this.DamageBonusChargePerc);
		num2 += (float)((!critical) ? 0 : this.DamageCriticalBonusPerc);
		if (target != null)
		{
			num += ((target.AllegianceId != global::AllegianceId.DESTRUCTION) ? this.DamageUnholy : this.DamageHoly);
			num2 += (float)((target.AllegianceId != global::AllegianceId.DESTRUCTION) ? this.DamageBonusUnholyPerc : this.DamageBonusHolyPerc);
		}
		int num3 = global::PandoraUtils.Round((float)(damage + num) * (num2 / 100f));
		float num4 = 100f + (float)((!flag) ? this.GlobalMeleeDamagePerc : this.GlobalRangeDamagePerc);
		num3 = global::PandoraUtils.Round((float)num3 * (num4 / 100f));
		if (target != null && !critical && !byPassArmor)
		{
			float num5 = (float)global::UnityEngine.Mathf.Max(target.ArmorAbsorptionPerc - this.ByPassArmorPerc, 0);
			num3 -= global::UnityEngine.Mathf.Max(target.ArmorAbsorption - this.BypassArmor, 0);
			num3 -= global::PandoraUtils.Round((float)num3 * num5 / 100f);
		}
		return (num3 <= 0) ? 0 : num3;
	}

	public bool HasRange()
	{
		return this.Items[(int)this.ActiveWeaponSlot] != null && this.Items[(int)this.ActiveWeaponSlot].TypeData != null && this.Items[(int)this.ActiveWeaponSlot].TypeData.IsRange;
	}

	private void ApplyRuneMark(global::RuneMark runeMark)
	{
		for (int i = 0; i < runeMark.AttributeModifiers.Count; i++)
		{
			global::RuneMarkAttributeData runeMarkAttributeData = runeMark.AttributeModifiers[i];
			if ((runeMarkAttributeData.UnitActionId == global::UnitActionId.NONE && runeMarkAttributeData.SkillId == global::SkillId.NONE) || (this.ActiveSkill != null && this.ActiveSkill.UnitActionId != global::UnitActionId.NONE && this.ActiveSkill.UnitActionId == runeMarkAttributeData.UnitActionId) || (this.ActiveSkill != null && this.ActiveSkill.Id != global::SkillId.NONE && this.ActiveSkill.Id == runeMarkAttributeData.SkillId))
			{
				this.AddToAttribute(runeMarkAttributeData.AttributeId, runeMarkAttributeData.Modifier);
				this.AddAttributeModifier(global::AttributeMod.Type.ITEM, runeMarkAttributeData.AttributeId, global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(runeMark.FullLabel), runeMarkAttributeData.Modifier, false);
			}
		}
		for (int j = 0; j < runeMark.Enchantments.Count; j++)
		{
			this.ApplyEnchantment(runeMark.Enchantments[j]);
		}
	}

	public void SetStatus(global::UnitStateId newStatus)
	{
		if (global::PandoraSingleton<global::MissionManager>.Exists())
		{
			if (newStatus == global::UnitStateId.STUNNED)
			{
				bool flag = this.Roll(global::PandoraSingleton<global::MissionManager>.Instance.NetworkTyche, this.StunResistRoll, global::AttributeId.STUN_RESIST_ROLL, false, true, 0);
				if (flag)
				{
					return;
				}
			}
			if (newStatus != this.Status)
			{
				if (newStatus == global::UnitStateId.OUT_OF_ACTION)
				{
					global::PandoraSingleton<global::MissionManager>.Instance.CombatLogger.AddLog(this, global::CombatLogger.LogMessage.UNIT_OUT_OF_ACTION, new string[]
					{
						global::PandoraSingleton<global::MissionManager>.Instance.GetUnitController(this, false).GetLogName(),
						this.GetAttribute(global::AttributeId.MORAL_IMPACT).ToConstantString()
					});
					this.AddToAttribute(global::AttributeId.TOTAL_OOA, 1);
				}
				else
				{
					global::PandoraSingleton<global::MissionManager>.Instance.CombatLogger.AddLog(this, global::CombatLogger.LogMessage.UNIT_STATUS, new string[]
					{
						global::PandoraSingleton<global::MissionManager>.Instance.GetUnitController(this, false).GetLogName(),
						global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("unit_state_" + newStatus)
					});
					if (newStatus == global::UnitStateId.STUNNED)
					{
						this.AddEnchantment(global::EnchantmentId.FREE_STANCE_REMOVAL, this, false, true, global::AllegianceId.NONE);
					}
					if (this.stunningUnit != null && newStatus == global::UnitStateId.STUNNED)
					{
						global::UnitController unitController = global::PandoraSingleton<global::MissionManager>.Instance.GetUnitController(this, false);
						global::UnitController unitController2 = global::PandoraSingleton<global::MissionManager>.Instance.GetUnitController(this.stunningUnit, false);
						if (unitController != null && unitController2 != null && unitController2.IsPlayed() && !unitController.IsPlayed())
						{
							global::PandoraSingleton<global::Hephaestus>.Instance.IncrementStat(global::Hephaestus.StatId.STUN_ENEMIES, 1);
						}
					}
				}
			}
		}
		this.Status = newStatus;
		this.stunningUnit = null;
	}

	public bool IsAvailable()
	{
		return this.Status == global::UnitStateId.NONE;
	}

	public bool IsSkillBlocked(global::SkillData data, out global::EnchantmentId enchantmentId)
	{
		enchantmentId = global::EnchantmentId.NONE;
		return this.blockedActions.TryGetValue(data.UnitActionId, out enchantmentId) || this.blockedSkills.TryGetValue(data.Id, out enchantmentId);
	}

	public bool IsUnitActionBlocked(global::UnitActionId actionId)
	{
		return this.blockedActions.ContainsKey(actionId);
	}

	public bool IsBoneBlocked(global::BoneId boneId)
	{
		global::EnchantmentId enchantmentId;
		return this.IsBoneBlocked(boneId, out enchantmentId);
	}

	public bool IsBoneBlocked(global::BoneId bone, out global::EnchantmentId enchantmentId)
	{
		enchantmentId = global::EnchantmentId.NONE;
		return this.blockedBones.TryGetValue(bone, out enchantmentId);
	}

	public bool IsItemTypeBlocked(global::ItemTypeId typeId)
	{
		global::EnchantmentId enchantmentId;
		return this.IsItemTypeBlocked(typeId, out enchantmentId);
	}

	public bool IsItemTypeBlocked(global::ItemTypeId typeId, out global::EnchantmentId enchantmentId)
	{
		enchantmentId = global::EnchantmentId.NONE;
		return this.blockedItemTypes.TryGetValue(typeId, out enchantmentId);
	}

	public void ResetPoints()
	{
		this.SetAttribute(global::AttributeId.CURRENT_OFFENSE_POINTS, this.OffensePoints);
		this.SetAttribute(global::AttributeId.CURRENT_STRATEGY_POINTS, this.StrategyPoints);
		this.ResetTempPoints();
	}

	public bool HasEnoughPoints(int stratPoints, int offPoints)
	{
		return global::UnityEngine.Mathf.Max(0, stratPoints + this.tempStrategyPoints) <= this.CurrentStrategyPoints && global::UnityEngine.Mathf.Max(0, offPoints + this.tempOffensePoints) <= this.CurrentOffensePoints;
	}

	public void ResetTempPoints()
	{
		this.tempOffensePoints = 0;
		this.tempStrategyPoints = 0;
	}

	public int GetStratCostModifier(global::SkillId skillId, global::UnitActionId actionId, global::SpellTypeId spellTypeId)
	{
		int num = 0;
		if (this.skillCostModifiers.ContainsKey(skillId))
		{
			num += this.skillCostModifiers[skillId].strat;
		}
		if (this.actionCostModifiers.ContainsKey(actionId))
		{
			num += this.actionCostModifiers[actionId].strat;
		}
		if (this.spellTypeModifiers.ContainsKey(spellTypeId))
		{
			num += this.spellTypeModifiers[spellTypeId].strat;
		}
		return num;
	}

	public int GetOffCostModifier(global::SkillId skillId, global::UnitActionId actionId, global::SpellTypeId spellTypeId)
	{
		int num = 0;
		if (this.skillCostModifiers.ContainsKey(skillId))
		{
			num += this.skillCostModifiers[skillId].off;
		}
		if (this.actionCostModifiers.ContainsKey(actionId))
		{
			num += this.actionCostModifiers[actionId].off;
		}
		if (this.spellTypeModifiers.ContainsKey(spellTypeId))
		{
			num += this.spellTypeModifiers[spellTypeId].off;
		}
		return num;
	}

	public void RemovePoints(int stratPoints, int offPoints)
	{
		this.SetAttribute(global::AttributeId.CURRENT_OFFENSE_POINTS, global::UnityEngine.Mathf.Min(this.CurrentOffensePoints - global::UnityEngine.Mathf.Max(offPoints, 0), this.OffensePoints));
		this.SetAttribute(global::AttributeId.CURRENT_STRATEGY_POINTS, global::UnityEngine.Mathf.Min(this.CurrentStrategyPoints - global::UnityEngine.Mathf.Max(stratPoints, 0), this.StrategyPoints));
		this.ResetTempPoints();
		global::PandoraDebug.LogInfo("Strat Points : " + this.CurrentStrategyPoints, "CHARACTER", null);
		global::PandoraDebug.LogInfo("Off Points : " + this.CurrentOffensePoints, "CHARACTER", null);
		global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice(global::Notices.UPDATE_POINTS);
		if (this.CurrentOffensePoints < 0 || this.CurrentStrategyPoints < 0)
		{
			global::PandoraDebug.LogWarning("Removing points when not enough points available", "uncategorised", null);
		}
	}

	public void RemoveTempPoints()
	{
		this.AddToAttribute(global::AttributeId.CURRENT_OFFENSE_POINTS, -this.tempOffensePoints);
		this.SetAttribute(global::AttributeId.CURRENT_OFFENSE_POINTS, (this.CurrentOffensePoints < 0) ? 0 : this.CurrentOffensePoints);
		this.AddToAttribute(global::AttributeId.CURRENT_STRATEGY_POINTS, -this.tempStrategyPoints);
		this.SetAttribute(global::AttributeId.CURRENT_STRATEGY_POINTS, (this.CurrentStrategyPoints < 0) ? 0 : this.CurrentStrategyPoints);
		this.ResetTempPoints();
	}

	public int GetUnitTypeRating()
	{
		return global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::UnitTypeData>((int)this.GetUnitTypeId()).Rating;
	}

	public int GetRankRating()
	{
		return global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::UnitRankData>((int)this.UnitSave.rankId).Rating;
	}

	public int GetStatsRating()
	{
		int num = 0;
		for (int i = 0; i < this.attributeDataList.Count; i++)
		{
			global::AttributeData attributeData = this.attributeDataList[i];
			if (attributeData.Rating != 0)
			{
				int baseAttribute = this.GetBaseAttribute(attributeData.Id);
				num += baseAttribute * attributeData.Rating;
			}
		}
		return num;
	}

	public int GetSkillsRating()
	{
		int num = 0;
		for (int i = 0; i < this.ActiveSkills.Count; i++)
		{
			num += global::SkillHelper.GetRating(this.ActiveSkills[i]);
		}
		for (int j = 0; j < this.PassiveSkills.Count; j++)
		{
			num += global::SkillHelper.GetRating(this.PassiveSkills[j]);
		}
		for (int k = 0; k < this.Spells.Count; k++)
		{
			num += global::SkillHelper.GetRating(this.Spells[k]);
		}
		return num;
	}

	public int GetInjuriesRating()
	{
		int num = 0;
		for (int i = 0; i < this.Injuries.Count; i++)
		{
			num += this.Injuries[i].Data.Rating;
		}
		return num;
	}

	public int GetMutationsRating()
	{
		int num = 0;
		for (int i = 0; i < this.Mutations.Count; i++)
		{
			num += this.Mutations[i].Data.Rating;
		}
		return num;
	}

	public int GetEquipmentRating()
	{
		int num = 0;
		for (int i = 0; i < this.Items.Count; i++)
		{
			num += this.Items[i].GetRating();
		}
		return num;
	}

	public int GetRating()
	{
		int num = 0;
		num += this.GetUnitTypeRating();
		num += this.GetRankRating();
		num += this.GetStatsRating();
		num += this.GetSkillsRating();
		num += this.GetInjuriesRating();
		num += this.GetMutationsRating();
		return num + this.GetEquipmentRating();
	}

	public bool IsHero()
	{
		global::UnitTypeId unitTypeId = this.GetUnitTypeId();
		return unitTypeId == global::UnitTypeId.LEADER || unitTypeId == global::UnitTypeId.HERO_1 || unitTypeId == global::UnitTypeId.HERO_2 || unitTypeId == global::UnitTypeId.HERO_3;
	}

	public global::UnitTypeId GetUnitTypeId()
	{
		return global::Unit.GetUnitTypeId(this.UnitSave, this.Data.UnitTypeId);
	}

	public static global::UnitTypeId GetUnitTypeId(global::UnitSave save, global::UnitTypeId baseUnitTypeId)
	{
		return (save.overrideTypeId == global::UnitTypeId.NONE) ? baseUnitTypeId : save.overrideTypeId;
	}

	public int GetHireCost()
	{
		if (this.GetUnitTypeId() == global::UnitTypeId.DRAMATIS || this.GetUnitTypeId() == global::UnitTypeId.MONSTER || this.UnitSave.isFreeOutsider)
		{
			return 0;
		}
		int num = this.GetUnitCost().Hiring;
		if (this.UnitSave.isOutsider)
		{
			num += this.UnspentSkill * global::Constant.GetInt(global::ConstantId.HIRE_UNIT_COST_PER_SKILL_POINT);
			num += this.UnspentSpell * global::Constant.GetInt(global::ConstantId.HIRE_UNIT_COST_PER_SKILL_POINT);
			num += this.Injuries.Count * global::Constant.GetInt(global::ConstantId.HIRE_UNIT_COST_PER_INJURY);
			for (int i = 0; i < this.items.Count; i++)
			{
				num += this.items[i].PriceSold;
			}
		}
		return num;
	}

	public global::UnitCostData GetUnitCost()
	{
		global::System.Collections.Generic.List<global::UnitCostData> list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::UnitCostData>(new string[]
		{
			"fk_unit_type_id",
			"rank"
		}, new string[]
		{
			((int)this.GetUnitTypeId()).ToConstantString(),
			this.Rank.ToConstantString()
		});
		return list[0];
	}

	public int NeedLevelUp()
	{
		return this.UnspentPhysical + this.UnspentMartial + this.UnspentMental + this.UnspentSkill + this.UnspentSpell;
	}

	public int GetBaseAttribute(global::AttributeId attributeId)
	{
		int num = 0;
		global::AttributeData attributeData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::AttributeData>((int)attributeId);
		for (int i = 0; i < this.baseAttributesData.Count; i++)
		{
			if (this.baseAttributesData[i].AttributeId == attributeId)
			{
				num += this.baseAttributesData[i].BaseValue;
				break;
			}
		}
		for (int j = 0; j < this.PassiveSkills.Count; j++)
		{
			global::System.Collections.Generic.List<global::SkillAttributeData> list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::SkillAttributeData>("fk_skill_id", ((int)this.PassiveSkills[j].Id).ToConstantString());
			for (int k = 0; k < list.Count; k++)
			{
				global::SkillAttributeData skillAttributeData = list[k];
				if (skillAttributeData.AttributeId == attributeId && skillAttributeData.UnitActionIdTrigger == global::UnitActionId.NONE && skillAttributeData.SkillIdTrigger == global::SkillId.NONE)
				{
					num += skillAttributeData.Modifier;
				}
			}
		}
		for (int l = 0; l < this.Injuries.Count; l++)
		{
			for (int m = 0; m < this.Injuries[l].AttributeModifiers.Count; m++)
			{
				global::InjuryJoinAttributeData injuryJoinAttributeData = this.Injuries[l].AttributeModifiers[m];
				if (injuryJoinAttributeData.AttributeId == attributeId)
				{
					num += injuryJoinAttributeData.Modifier;
				}
			}
		}
		for (int n = 0; n < this.ConsumableSkills.Count; n++)
		{
			global::System.Collections.Generic.List<global::SkillAttributeData> list2 = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::SkillAttributeData>("fk_skill_id", ((int)this.ConsumableSkills[n].Id).ToConstantString());
			for (int num2 = 0; num2 < list2.Count; num2++)
			{
				global::SkillAttributeData skillAttributeData2 = list2[num2];
				if (skillAttributeData2.AttributeId == attributeId && skillAttributeData2.UnitActionIdTrigger == global::UnitActionId.NONE && skillAttributeData2.SkillIdTrigger == global::SkillId.NONE)
				{
					num += skillAttributeData2.Modifier;
				}
			}
		}
		num += this.GetSaveAttribute(attributeId);
		if (attributeData.AttributeIdMax != global::AttributeId.NONE)
		{
			num = global::UnityEngine.Mathf.Clamp(num, 1, this.GetBaseAttribute(attributeData.AttributeIdMax));
		}
		for (int num3 = 0; num3 < this.baseAttributesDataMax.Count; num3++)
		{
			global::UnitTypeAttributeData unitTypeAttributeData = this.baseAttributesDataMax[num3];
			if (attributeId == unitTypeAttributeData.AttributeId)
			{
				num = global::UnityEngine.Mathf.Min(num, unitTypeAttributeData.Max);
			}
		}
		return num;
	}

	public int GetSaveAttribute(global::AttributeId attributeId)
	{
		int result = 0;
		this.UnitSave.attributes.TryGetValue(attributeId, out result);
		return result;
	}

	public bool CanLowerAttribute(global::AttributeId attributeId)
	{
		return this.GetTempAttribute(attributeId) > 0;
	}

	private int GetUnspentPoints(global::AttributeId attributeId)
	{
		for (int i = 0; i < global::Unit.PhysicalAttributeIds.Length; i++)
		{
			if (global::Unit.PhysicalAttributeIds[i] == attributeId)
			{
				return this.UnspentPhysical;
			}
		}
		for (int j = 0; j < global::Unit.MentalAttributeIds.Length; j++)
		{
			if (global::Unit.MentalAttributeIds[j] == attributeId)
			{
				return this.UnspentMental;
			}
		}
		for (int k = 0; k < global::Unit.MartialAttributeIds.Length; k++)
		{
			if (global::Unit.MartialAttributeIds[k] == attributeId)
			{
				return this.UnspentMartial;
			}
		}
		return 0;
	}

	public bool CanRaiseAttribute(global::AttributeId attributeId)
	{
		int num = this.GetBaseAttribute(attributeId) + this.GetTempAttribute(attributeId);
		global::AttributeId attributeId2;
		if (!this.maxAttributes.TryGetValue((int)attributeId, out attributeId2))
		{
			return false;
		}
		int baseAttribute = this.GetBaseAttribute(attributeId2);
		return num < baseAttribute && this.GetUnspentPoints(attributeId) > 0;
	}

	public bool CanRaiseAttributeFast(global::AttributeId attributeId, int[] baseAttributes, int[] maxAttributes, int unspent)
	{
		int num = 20;
		int num2 = 20;
		switch (attributeId)
		{
		case global::AttributeId.WEAPON_SKILL:
			num = baseAttributes[6];
			num2 = maxAttributes[6];
			break;
		case global::AttributeId.BALLISTIC_SKILL:
			num = baseAttributes[7];
			num2 = maxAttributes[7];
			break;
		case global::AttributeId.STRENGTH:
			num = baseAttributes[0];
			num2 = maxAttributes[0];
			break;
		case global::AttributeId.TOUGHNESS:
			num = baseAttributes[1];
			num2 = maxAttributes[1];
			break;
		case global::AttributeId.AGILITY:
			num = baseAttributes[2];
			num2 = maxAttributes[2];
			break;
		case global::AttributeId.LEADERSHIP:
			num = baseAttributes[3];
			num2 = maxAttributes[3];
			break;
		case global::AttributeId.INTELLIGENCE:
			num = baseAttributes[4];
			num2 = maxAttributes[4];
			break;
		case global::AttributeId.ALERTNESS:
			num = baseAttributes[5];
			num2 = maxAttributes[5];
			break;
		case global::AttributeId.ACCURACY:
			num = baseAttributes[8];
			num2 = maxAttributes[8];
			break;
		}
		int num3 = num + this.GetTempAttribute(attributeId);
		return num3 < num2 && unspent > 0;
	}

	public void RaiseAttribute(global::AttributeId attributeId, bool updateAttributes = true)
	{
		this.AddToTempAttribute(attributeId, 1);
		if (updateAttributes)
		{
			this.UpdateAttributes();
		}
	}

	public void LowerAttribute(global::AttributeId attributeId)
	{
		this.AddToTempAttribute(attributeId, -1);
		this.UpdateAttributes();
	}

	public bool HasSkillOrSpell(global::SkillId skillId)
	{
		for (int i = 0; i < this.PassiveSkills.Count; i++)
		{
			if (this.PassiveSkills[i].Id == skillId)
			{
				return true;
			}
		}
		for (int i = 0; i < this.ConsumableSkills.Count; i++)
		{
			if (this.ConsumableSkills[i].Id == skillId)
			{
				return true;
			}
		}
		for (int i = 0; i < this.ActiveSkills.Count; i++)
		{
			if (this.ActiveSkills[i].Id == skillId)
			{
				return true;
			}
		}
		for (int i = 0; i < this.Spells.Count; i++)
		{
			if (this.Spells[i].Id == skillId)
			{
				return true;
			}
		}
		return false;
	}

	public void EndLearnSkill(bool updateAttributes = true)
	{
		global::SkillData skillData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::SkillData>((int)this.UnitSave.skillInTrainingId);
		this.UnitSave.skillInTrainingId = global::SkillId.NONE;
		this.UnitSave.trainingTime = 0;
		bool flag = skillData.SpellTypeId != global::SpellTypeId.NONE;
		if (flag)
		{
			this.UnitSave.spells.Add(skillData.Id);
		}
		else if (skillData.Passive)
		{
			this.UnitSave.passiveSkills.Add(skillData.Id);
		}
		else
		{
			this.UnitSave.activeSkills.Add(skillData.Id);
		}
		this.AddSkill(skillData.Id, true);
		this.AddSkillLearnBonus(skillData.Id);
		this.CalculateProgressionPoints();
		if (updateAttributes)
		{
			this.UpdateAttributes();
		}
	}

	private void AddSkillLearnBonus(global::SkillId skillId)
	{
		global::System.Collections.Generic.List<global::SkillLearnBonusData> skillLearnBonus = global::SkillHelper.GetSkillLearnBonus(skillId);
		if (skillLearnBonus.Count > 0)
		{
			for (int i = 0; i < skillLearnBonus.Count; i++)
			{
				if (skillLearnBonus[i].UnitTypeId != global::UnitTypeId.NONE)
				{
					this.UnitSave.overrideTypeId = skillLearnBonus[i].UnitTypeId;
					this.RefreshDescription();
					this.SetMoralImpact();
				}
			}
		}
	}

	public void StartLearningSkill(global::SkillData skillData, int currentDate, bool log = true)
	{
		this.UnitSave.skillInTrainingId = skillData.Id;
		this.UnitSave.trainingTime = skillData.Time;
		this.CalculateProgressionPoints();
		if (log)
		{
			this.Logger.AddHistory(currentDate + skillData.Time, global::EventLogger.LogEvent.SKILL, (int)skillData.Id);
		}
	}

	public bool HasEnoughPointsForSkill(global::SkillData skillData)
	{
		if (skillData.SkillTypeId == global::SkillTypeId.SPELL_ACTION)
		{
			return skillData.Points <= this.UnspentSpell;
		}
		return skillData.Points <= this.UnspentSkill;
	}

	public bool CanLearnSkill(global::SkillData skillData, out string reason)
	{
		reason = null;
		if (this.GetActiveStatus() != global::UnitActiveStatusId.AVAILABLE)
		{
			reason = "na_skill_" + this.GetActiveStatus().ToLowerString();
		}
		if (skillData.SkillTypeId == global::SkillTypeId.SPELL_ACTION)
		{
			if (skillData.Points > this.UnspentSpell)
			{
				reason = "na_skill_not_enough_spell_points";
			}
		}
		else if (skillData.Points > this.UnspentSkill)
		{
			reason = "na_skill_not_enough_skill_points";
		}
		if (!global::SkillHelper.IsMastery(skillData))
		{
			if (skillData.SkillTypeId == global::SkillTypeId.SPELL_ACTION)
			{
				if (this.Spells.Count == 5)
				{
					reason = "na_skill_spell_slot_full";
				}
			}
			else
			{
				if (skillData.Passive && this.PassiveSkills.Count == 5)
				{
					reason = "na_skill_passive_slot_full";
				}
				if (!skillData.Passive && this.ActiveSkills.Count == 5)
				{
					reason = "na_skill_active_slot_full";
				}
			}
		}
		if (global::SkillHelper.IsMastery(skillData) && this.GetUnitTypeId() == global::UnitTypeId.HENCHMEN)
		{
			reason = "na_skill_henchmen";
		}
		if (skillData.AttributeIdStat != global::AttributeId.NONE && this.GetBaseAttribute(skillData.AttributeIdStat) < skillData.StatValue)
		{
			reason = "na_skill_attribute";
		}
		return reason == null;
	}

	public bool CanLearnSkillFast(global::SkillData skillData, int[] baseAttributes)
	{
		if (this.GetUnitTypeId() == global::UnitTypeId.HENCHMEN && global::SkillHelper.IsMastery(skillData))
		{
			return false;
		}
		int num = 20;
		switch (skillData.AttributeIdStat)
		{
		case global::AttributeId.WEAPON_SKILL:
			num = baseAttributes[6];
			break;
		case global::AttributeId.BALLISTIC_SKILL:
			num = baseAttributes[7];
			break;
		case global::AttributeId.STRENGTH:
			num = baseAttributes[0];
			break;
		case global::AttributeId.TOUGHNESS:
			num = baseAttributes[1];
			break;
		case global::AttributeId.AGILITY:
			num = baseAttributes[2];
			break;
		case global::AttributeId.LEADERSHIP:
			num = baseAttributes[3];
			break;
		case global::AttributeId.INTELLIGENCE:
			num = baseAttributes[4];
			break;
		case global::AttributeId.ALERTNESS:
			num = baseAttributes[5];
			break;
		case global::AttributeId.ACCURACY:
			num = baseAttributes[8];
			break;
		}
		return num >= skillData.StatValue;
	}

	public bool IsMaxRank()
	{
		global::UnitRankData unitRankData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::UnitRankData>((int)this.UnitSave.rankId);
		return unitRankData.Rank >= global::Constant.GetInt(global::ConstantId.MAX_UNIT_RANK);
	}

	public void AddXp(int xp, global::System.Collections.Generic.List<global::UnitJoinUnitRankData> advancements, global::System.Collections.Generic.List<global::Mutation> newMutations, global::System.Collections.Generic.List<global::Item> previousItems, int day, int maxRank = -1)
	{
		global::UnitRankData unitRankData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::UnitRankData>((int)this.UnitSave.rankId);
		bool flag = maxRank == -1;
		if (this.IsMaxRank())
		{
			return;
		}
		global::System.Collections.Generic.List<global::UnitRankProgressionData> list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::UnitRankProgressionData>("fk_unit_type_id", ((int)this.GetUnitTypeId()).ToConstantString());
		int num = 0;
		for (int i = 0; i < list.Count; i++)
		{
			global::UnitRankProgressionData unitRankProgressionData = list[i];
			if (unitRankProgressionData.Rank == unitRankData.Rank)
			{
				num = unitRankProgressionData.Xp;
				break;
			}
		}
		int num2 = 0;
		for (int j = 0; j < this.ranksData.Count; j++)
		{
			if (this.ranksData[j].UnitRankId == this.UnitSave.rankId)
			{
				num2 = j;
				break;
			}
		}
		this.UnitSave.xp += xp;
		this.UnitSave.xp = global::UnityEngine.Mathf.Max(this.UnitSave.xp, 0);
		while (this.UnitSave.xp >= num && unitRankData.Rank < global::Constant.GetInt(global::ConstantId.MAX_UNIT_RANK) && (maxRank == -1 || unitRankData.Rank < maxRank))
		{
			this.UnitSave.xp -= num;
			num2++;
			global::UnitJoinUnitRankData unitJoinUnitRankData = this.ranksData[num2];
			this.UnitSave.rankId = unitJoinUnitRankData.UnitRankId;
			if (unitJoinUnitRankData.EnchantmentId != global::EnchantmentId.NONE)
			{
				this.Enchantments.Add(new global::Enchantment(unitJoinUnitRankData.EnchantmentId, null, this, true, true, global::AllegianceId.NONE, true));
			}
			advancements.Add(unitJoinUnitRankData);
			if (unitJoinUnitRankData.Mutation)
			{
				global::Mutation item = this.AddRandomMutation(previousItems);
				newMutations.Add(item);
			}
			global::UnitRankData unitRankData2 = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::UnitRankData>((int)this.UnitSave.rankId);
			if (unitRankData2.Rank != unitRankData.Rank)
			{
				if (flag)
				{
					this.Logger.AddHistory(day, global::EventLogger.LogEvent.RANK_ACHIEVED, unitRankData2.Rank);
				}
				unitRankData = unitRankData2;
				this.SetAttribute(global::AttributeId.RANK, unitRankData.Rank);
				global::PandoraDebug.LogDebug("Rank is now = " + unitRankData.Rank, "uncategorised", null);
				foreach (global::UnitRankProgressionData unitRankProgressionData2 in list)
				{
					if (unitRankProgressionData2.Rank == unitRankData.Rank)
					{
						num = unitRankProgressionData2.Xp;
						break;
					}
				}
			}
		}
		this.ResetBodyPart();
		this.CalculateProgressionPoints();
		this.UpdateAttributes();
		this.SetMoralImpact();
	}

	public int GetUpkeepOwned()
	{
		return this.UnitSave.upkeepOwned;
	}

	public int GetUpkeepMissedDays()
	{
		return this.UnitSave.upkeepMissedDays;
	}

	public int AddToUpkeepOwned(int money)
	{
		if (this.UnitSave.upkeepOwned > 0 && money > 0)
		{
			this.UnitSave.upkeepMissedDays++;
		}
		this.UnitSave.upkeepOwned += money;
		return this.GetUpkeepMissedDays();
	}

	public void PayUpkeepOwned()
	{
		this.UnitSave.upkeepOwned = 0;
		this.UnitSave.upkeepMissedDays = 0;
		this.Logger.RemoveLastHistory(global::EventLogger.LogEvent.LEFT);
	}

	public global::UnitActiveStatusId GetActiveStatus()
	{
		if (!this.UnitSave.injuryPaid && this.UnitSave.injuredTime > 0)
		{
			return global::UnitActiveStatusId.TREATMENT_NOT_PAID;
		}
		if (this.UnitSave.injuredTime > 0)
		{
			if (this.UnitSave.upkeepOwned > 0)
			{
				return global::UnitActiveStatusId.INJURED_AND_UPKEEP_NOT_PAID;
			}
			return global::UnitActiveStatusId.INJURED;
		}
		else
		{
			if (this.UnitSave.upkeepOwned > 0)
			{
				return global::UnitActiveStatusId.UPKEEP_NOT_PAID;
			}
			if (this.UnitSave.trainingTime > 0)
			{
				return global::UnitActiveStatusId.IN_TRAINING;
			}
			return global::UnitActiveStatusId.AVAILABLE;
		}
	}

	public void ResetBodyPart()
	{
		foreach (global::BodyPart bodyPart in this.bodyParts.Values)
		{
			bodyPart.DestroyRelatedGO();
		}
		this.bodyParts.Clear();
		for (int i = 0; i < this.availableBodyParts.Count; i++)
		{
			global::System.Collections.Generic.KeyValuePair<int, int> keyValuePair;
			this.UnitSave.customParts.TryGetValue(this.availableBodyParts[i].Id, out keyValuePair);
			this.bodyParts[this.availableBodyParts[i].Id] = new global::BodyPart(this.availableBodyParts[i], this.Id, this.WarData.Asset, this.Data.Asset, this.Data.AltAsset, this.UnitSave.skinColor, keyValuePair.Value, keyValuePair.Key);
		}
		for (int j = 0; j < this.Items.Count; j++)
		{
			this.SetBodyPartsSlot((global::UnitSlotId)j);
		}
		for (int k = 0; k < this.Mutations.Count; k++)
		{
			for (int l = 0; l < this.Mutations[k].RelatedBodyParts.Count; l++)
			{
				global::BodyPartId bodyPartId = this.Mutations[k].RelatedBodyParts[l].BodyPartId;
				if (this.bodyParts.ContainsKey(bodyPartId))
				{
					this.bodyParts[bodyPartId].SetMutation(this.Mutations[k].Data.Id);
					if (this.Mutations[k].GroupData.EmptyLinkedBodyPart)
					{
						this.EmptyLinkedBodyParts(bodyPartId, true);
					}
				}
			}
		}
		for (int m = 0; m < this.Injuries.Count; m++)
		{
			global::InjuryData data = this.Injuries[m].Data;
			if (data.BodyPartId != global::BodyPartId.NONE)
			{
				this.bodyParts[data.BodyPartId].SetInjury(data.Id);
				this.EmptyLinkedBodyParts(data.BodyPartId, false);
			}
		}
	}

	private void EmptyLinkedBodyParts(global::BodyPartId partId, bool mutation)
	{
		global::System.Collections.Generic.List<global::BodyPartUpdateData> list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::BodyPartUpdateData>("fk_body_part_id", partId.ToIntString<global::BodyPartId>());
		for (int i = 0; i < list.Count; i++)
		{
			if (this.bodyParts.ContainsKey(list[i].BodyPartIdUpdated))
			{
				if (!mutation || list[i].BodyPartIdUpdated != global::BodyPartId.GEAR_ARML)
				{
					this.bodyParts[list[i].BodyPartIdUpdated].SetEmpty(true);
				}
			}
		}
	}

	public void SetBodyPartsSlot(global::UnitSlotId slotId)
	{
		global::System.Collections.Generic.List<global::BodyPartData> list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::BodyPartData>("fk_unit_slot_id", slotId.ToIntString<global::UnitSlotId>());
		if (list.Count > 0)
		{
			for (int i = 0; i < list.Count; i++)
			{
				if (this.bodyParts.ContainsKey(list[i].Id))
				{
					this.bodyParts[list[i].Id].SetRelatedItem(this.Items[(int)slotId]);
				}
			}
		}
		this.SetBodyPartItemLock(this.Items[(int)slotId], false);
	}

	private void SetBodyPartItemLock(global::Item item, bool reverse = false)
	{
		if (item == null)
		{
			return;
		}
		global::System.Collections.Generic.List<global::ItemJoinBodyPartData> list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::ItemJoinBodyPartData>("fk_item_id", item.Id.ToIntString<global::ItemId>());
		for (int i = 0; i < list.Count; i++)
		{
			if (this.bodyParts.ContainsKey(list[i].BodyPartId))
			{
				this.bodyParts[list[i].BodyPartId].SetLocked((!reverse) ? list[i].Lock : (!list[i].Lock));
				if (!list[i].Lock)
				{
					global::BodyPartData bodyPartData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::BodyPartData>((int)list[i].BodyPartId);
					if (bodyPartData.UnitSlotId != global::UnitSlotId.NONE)
					{
						this.bodyParts[list[i].BodyPartId].SetRelatedItem(this.Items[(int)bodyPartData.UnitSlotId]);
					}
				}
			}
		}
	}

	public global::System.Collections.Generic.Dictionary<global::SpellCurseId, int> GetCurseModifiers()
	{
		global::System.Collections.Generic.Dictionary<global::SpellCurseId, int> dictionary = new global::System.Collections.Generic.Dictionary<global::SpellCurseId, int>();
		this.RefreshActiveItems();
		this.GetEnchantmentsCurseModifiers(dictionary, this.Enchantments);
		for (int i = 0; i < this.ActiveItems.Count; i++)
		{
			this.GetEnchantmentsCurseModifiers(dictionary, this.ActiveItems[i].Enchantments);
			if (this.ActiveItems[i].RuneMark != null)
			{
				this.GetEnchantmentsCurseModifiers(dictionary, this.ActiveItems[i].RuneMark.Enchantments);
			}
		}
		for (int j = 0; j < this.Injuries.Count; j++)
		{
			this.GetEnchantmentsCurseModifiers(dictionary, this.Injuries[j].Enchantments);
		}
		for (int k = 0; k < this.Mutations.Count; k++)
		{
			this.GetEnchantmentsCurseModifiers(dictionary, this.Mutations[k].Enchantments);
		}
		return dictionary;
	}

	private void GetEnchantmentsCurseModifiers(global::System.Collections.Generic.Dictionary<global::SpellCurseId, int> modifiers, global::System.Collections.Generic.List<global::Enchantment> enchantments)
	{
		for (int i = 0; i < enchantments.Count; i++)
		{
			for (int j = 0; j < enchantments[i].CurseModifiers.Count; j++)
			{
				global::EnchantmentCurseModifierData enchantmentCurseModifierData = enchantments[i].CurseModifiers[j];
				if (modifiers.ContainsKey(enchantmentCurseModifierData.SpellCurseId))
				{
					global::SpellCurseId spellCurseId;
					global::SpellCurseId key = spellCurseId = enchantmentCurseModifierData.SpellCurseId;
					int num = modifiers[spellCurseId];
					modifiers[key] = num + enchantmentCurseModifierData.RatioModifier;
				}
				else
				{
					modifiers[enchantmentCurseModifierData.SpellCurseId] = enchantmentCurseModifierData.RatioModifier;
				}
			}
		}
	}

	public global::System.Collections.Generic.Dictionary<global::InjuryId, int> GetInjuryModifiers()
	{
		this.injuryRollModifiers.Clear();
		this.GetEnchantmentsInjuryModifiers(this.injuryRollModifiers, this.Enchantments);
		this.RefreshActiveItems();
		for (int i = 0; i < this.ActiveItems.Count; i++)
		{
			this.GetEnchantmentsInjuryModifiers(this.injuryRollModifiers, this.ActiveItems[i].Enchantments);
			if (this.ActiveItems[i].RuneMark != null)
			{
				this.GetEnchantmentsInjuryModifiers(this.injuryRollModifiers, this.ActiveItems[i].RuneMark.Enchantments);
			}
		}
		for (int j = 0; j < this.Injuries.Count; j++)
		{
			this.GetEnchantmentsInjuryModifiers(this.injuryRollModifiers, this.Injuries[j].Enchantments);
		}
		for (int k = 0; k < this.Mutations.Count; k++)
		{
			this.GetEnchantmentsInjuryModifiers(this.injuryRollModifiers, this.Mutations[k].Enchantments);
		}
		return this.injuryRollModifiers;
	}

	private void GetEnchantmentsInjuryModifiers(global::System.Collections.Generic.Dictionary<global::InjuryId, int> injuryRollModifiers, global::System.Collections.Generic.List<global::Enchantment> enchantments)
	{
		for (int i = 0; i < enchantments.Count; i++)
		{
			for (int j = 0; j < enchantments[i].InjuryModifiers.Count; j++)
			{
				global::EnchantmentInjuryModifierData enchantmentInjuryModifierData = enchantments[i].InjuryModifiers[j];
				if (injuryRollModifiers.ContainsKey(enchantmentInjuryModifierData.InjuryId))
				{
					global::InjuryId injuryId;
					global::InjuryId key = injuryId = enchantmentInjuryModifierData.InjuryId;
					int num = injuryRollModifiers[injuryId];
					injuryRollModifiers[key] = num + enchantmentInjuryModifierData.RatioModifier;
				}
				else
				{
					injuryRollModifiers[enchantmentInjuryModifierData.InjuryId] = enchantmentInjuryModifierData.RatioModifier;
				}
			}
		}
	}

	private void ApplyInjury(global::Injury injury)
	{
		for (int i = 0; i < injury.AttributeModifiers.Count; i++)
		{
			global::InjuryJoinAttributeData injuryJoinAttributeData = injury.AttributeModifiers[i];
			this.AddToAttribute(injuryJoinAttributeData.AttributeId, injuryJoinAttributeData.Modifier);
			this.AddAttributeModifier(global::AttributeMod.Type.INJURY, injuryJoinAttributeData.AttributeId, injury.LocName, injuryJoinAttributeData.Modifier, false);
		}
		for (int j = 0; j < injury.Enchantments.Count; j++)
		{
			this.ApplyEnchantment(injury.Enchantments[j]);
		}
	}

	public bool HasInjury(global::InjuryId injuryId)
	{
		return this.UnitSave.injuries.IndexOf(injuryId, global::InjuryIdComparer.Instance) != -1;
	}

	public global::InjuryId GetInjury(global::UnitSlotId slotId)
	{
		if (slotId == global::UnitSlotId.SET2_MAINHAND || slotId == global::UnitSlotId.SET2_OFFHAND)
		{
			slotId -= 2;
		}
		for (int i = 0; i < this.Injuries.Count; i++)
		{
			if (this.Injuries[i].Data.UnitSlotId == slotId)
			{
				return this.Injuries[i].Data.Id;
			}
		}
		return global::InjuryId.NONE;
	}

	public bool IsInjuryRepeatLimitExceeded(global::InjuryData injuryData, bool post)
	{
		int num = 0;
		for (int i = 0; i < this.UnitSave.injuries.Count; i++)
		{
			if (this.UnitSave.injuries[i] == injuryData.Id)
			{
				num++;
			}
		}
		if (injuryData.RepeatLimit == -1)
		{
			return false;
		}
		if (injuryData.RepeatLimit == 1)
		{
			return !post && num >= injuryData.RepeatLimit;
		}
		if (injuryData.RepeatLimit == 2)
		{
			return num >= injuryData.RepeatLimit;
		}
		return injuryData.RepeatLimit != -1 && num > injuryData.RepeatLimit;
	}

	public bool IsInjuryAttributeLimitExceeded(global::InjuryData injuryData, bool checkRetire)
	{
		global::System.Collections.Generic.List<global::InjuryJoinAttributeData> list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::InjuryJoinAttributeData>("fk_injury_id", ((int)injuryData.Id).ToConstantString());
		for (int i = 0; i < list.Count; i++)
		{
			global::InjuryJoinAttributeData injuryJoinAttributeData = list[i];
			if (injuryJoinAttributeData.Limit != -1)
			{
				int attribute = this.GetAttribute(injuryJoinAttributeData.AttributeId);
				if (attribute <= injuryJoinAttributeData.Limit && (!checkRetire || injuryJoinAttributeData.Retire))
				{
					return true;
				}
			}
		}
		return false;
	}

	public global::System.Collections.Generic.List<global::InjuryData> GetPossibleInjuries(global::System.Collections.Generic.List<global::InjuryId> toExcludes, global::Unit unit, global::System.Collections.Generic.Dictionary<global::InjuryId, int> injuryModifiers)
	{
		global::System.Collections.Generic.List<global::InjuryData> list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::InjuryData>("released", "1");
		global::System.Collections.Generic.List<global::InjuryData> list2 = new global::System.Collections.Generic.List<global::InjuryData>(list.Count);
		for (int i = 0; i < list.Count; i++)
		{
			if (this.CanRollInjury(list[i], toExcludes, injuryModifiers))
			{
				list2.Add(list[i]);
			}
		}
		return list2;
	}

	public bool CanRollInjury(global::InjuryData injury, global::System.Collections.Generic.List<global::InjuryId> toExcludes, global::System.Collections.Generic.Dictionary<global::InjuryId, int> injuryModifiers)
	{
		int num = 0;
		injuryModifiers.TryGetValue(injury.Id, out num);
		return toExcludes.IndexOf(injury.Id, global::InjuryIdComparer.Instance) == -1 && this.CanStackInjury(injury) && injury.Ratio + num > 0;
	}

	private bool CanStackInjury(global::InjuryData injuryData)
	{
		if (injuryData.Id == global::InjuryId.AMNESIA && this.Rank == global::Constant.GetInt(global::ConstantId.MAX_UNIT_RANK))
		{
			return false;
		}
		if (injuryData.Id == global::InjuryId.DEAD || !this.HasInjury(injuryData.Id))
		{
			return true;
		}
		bool flag = true;
		return flag & (!this.IsInjuryRepeatLimitExceeded(injuryData, false) && !this.IsInjuryAttributeLimitExceeded(injuryData, false));
	}

	public void ClearInjuries()
	{
		this.Injuries.Clear();
		this.UnitSave.injuries.Clear();
		this.UnitSave.lastInjuryDate = 0;
		this.UnitSave.injuredTime = 0;
		this.UnitSave.injuryPaid = false;
	}

	public bool AddInjury(global::InjuryData injury, int day, global::System.Collections.Generic.List<global::Item> removedItems, bool isHireUnit = false, int overrideInjuryTime = -1)
	{
		this.UnitSave.injuries.Add(injury.Id);
		global::Injury item = new global::Injury(injury.Id, this);
		this.Injuries.Add(item);
		this.Logger.AddHistory(day, global::EventLogger.LogEvent.INJURY, (int)injury.Id);
		if (this.IsInjuryAttributeLimitExceeded(injury, true) || this.IsInjuryRepeatLimitExceeded(injury, true))
		{
			global::PandoraDebug.LogDebug("Injury Added... Unit Dead or Retired", "uncategorised", null);
			return false;
		}
		for (int i = this.Mutations.Count - 1; i >= 0; i--)
		{
			if (this.Mutations[i].HasBodyPart(injury.BodyPartId))
			{
				for (int j = this.UnitSave.mutations.Count - 1; j >= 0; j--)
				{
					if (this.UnitSave.mutations[j] == this.Mutations[i].Data.Id)
					{
						this.UnitSave.mutations.RemoveAt(j);
					}
				}
				this.Mutations.RemoveAt(i);
			}
		}
		this.UpdateAttributesAndCheckBackPack(removedItems);
		this.RefreshActiveItems();
		if (injury.UnitSlotId != global::UnitSlotId.NONE)
		{
			this.ReequipWeapons(removedItems);
		}
		if (overrideInjuryTime == -1)
		{
			overrideInjuryTime = injury.Duration;
		}
		if (!isHireUnit && overrideInjuryTime > 0)
		{
			this.UnitSave.lastInjuryDate = day;
			this.UnitSave.injuryPaid = false;
			if (overrideInjuryTime > this.UnitSave.injuredTime)
			{
				this.UnitSave.injuredTime = overrideInjuryTime;
			}
		}
		return true;
	}

	public int GetTreatmentCost()
	{
		int result = 0;
		if (!this.UnitSave.injuryPaid)
		{
			global::UnitCostData unitCost = this.GetUnitCost();
			result = this.UnitSave.injuredTime * unitCost.Treatment;
		}
		return result;
	}

	public void TreatmentPaid()
	{
		this.UnitSave.injuryPaid = true;
		this.Logger.RemoveLastHistory(global::EventLogger.LogEvent.NO_TREATMENT);
		this.Logger.AddHistory(this.UnitSave.lastInjuryDate + this.UnitSave.injuredTime, global::EventLogger.LogEvent.RECOVERY, (int)this.UnitSave.injuries[this.UnitSave.injuries.Count - 1]);
		if (this.GetUpkeepOwned() > 0)
		{
			int date = global::PandoraSingleton<global::HideoutManager>.Instance.Date.CurrentDate + global::Constant.GetInt(global::ConstantId.UPKEEP_DAYS_WITHOUT_PAY);
			this.Logger.AddHistory(date, global::EventLogger.LogEvent.LEFT, 0);
		}
	}

	public void UpdateInjury()
	{
		if (this.UnitSave.injuredTime > 0 && this.UnitSave.injuryPaid)
		{
			this.UnitSave.injuredTime = global::UnityEngine.Mathf.Max(this.UnitSave.injuredTime - 1, 0);
		}
	}

	public void UpdateSkillTraining()
	{
		if (this.UnitSave.skillInTrainingId != global::SkillId.NONE)
		{
			this.UnitSave.trainingTime--;
			if (this.UnitSave.trainingTime <= 0)
			{
				this.EndLearnSkill(true);
			}
		}
	}

	private void ApplyMutation(global::Mutation mutation)
	{
		for (int i = 0; i < mutation.AttributeModifiers.Count; i++)
		{
			global::MutationAttributeData mutationAttributeData = mutation.AttributeModifiers[i];
			if ((mutationAttributeData.UnitActionIdTrigger == global::UnitActionId.NONE && mutationAttributeData.SkillIdTrigger == global::SkillId.NONE) || (this.ActiveSkill != null && this.ActiveSkill.UnitActionId != global::UnitActionId.NONE && this.ActiveSkill.UnitActionId == mutationAttributeData.UnitActionIdTrigger) || (this.ActiveSkill != null && this.ActiveSkill.Id != global::SkillId.NONE && this.ActiveSkill.Id == mutationAttributeData.SkillIdTrigger))
			{
				this.AddToAttribute(mutationAttributeData.AttributeId, mutationAttributeData.Modifier);
				this.AddAttributeModifier(global::AttributeMod.Type.MUTATION, mutationAttributeData.AttributeId, mutation.LocName, mutationAttributeData.Modifier, false);
			}
		}
		for (int j = 0; j < mutation.Enchantments.Count; j++)
		{
			this.ApplyEnchantment(mutation.Enchantments[j]);
		}
	}

	public global::MutationId GetMutationId(global::UnitSlotId slotId)
	{
		if (slotId == global::UnitSlotId.SET2_MAINHAND || slotId == global::UnitSlotId.SET2_OFFHAND)
		{
			slotId -= 2;
		}
		for (int i = 0; i < this.Mutations.Count; i++)
		{
			if (this.Mutations[i].GroupData.UnitSlotId == slotId)
			{
				return this.Mutations[i].Data.Id;
			}
		}
		return global::MutationId.NONE;
	}

	public global::Mutation GetMutation(global::UnitSlotId slotId)
	{
		if (slotId == global::UnitSlotId.SET2_MAINHAND || slotId == global::UnitSlotId.SET2_OFFHAND)
		{
			slotId -= 2;
		}
		for (int i = 0; i < this.Mutations.Count; i++)
		{
			if (this.Mutations[i].GroupData.UnitSlotId == slotId)
			{
				return this.Mutations[i];
			}
		}
		return null;
	}

	public bool HasMutation(global::MutationId id)
	{
		for (int i = 0; i < this.Mutations.Count; i++)
		{
			if (this.Mutations[i].Data.Id == id)
			{
				return true;
			}
		}
		return false;
	}

	public void ClearMutations()
	{
		this.Mutations.Clear();
		this.UnitSave.mutations.Clear();
	}

	public global::Mutation AddRandomMutation(global::System.Collections.Generic.List<global::Item> previousItems)
	{
		global::System.Collections.Generic.List<global::UnitJoinMutationData> list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::UnitJoinMutationData>("fk_unit_id", ((int)this.Id).ToConstantString());
		global::System.Collections.Generic.List<global::MutationData> list2 = new global::System.Collections.Generic.List<global::MutationData>();
		for (int i = 0; i < list.Count; i++)
		{
			global::MutationData mutationData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::MutationData>((int)list[i].MutationId);
			bool flag = true;
			int num = 0;
			while (num < this.Mutations.Count && flag)
			{
				if (mutationData.MutationGroupId == this.Mutations[num].GroupData.Id)
				{
					flag = false;
				}
				num++;
			}
			if (flag)
			{
				list2.Add(mutationData);
			}
		}
		global::MutationData randomRatio = global::MutationData.GetRandomRatio(list2, global::PandoraSingleton<global::GameManager>.Instance.LocalTyche, null);
		return this.AddMutation(randomRatio.Id, previousItems);
	}

	public global::Mutation AddMutation(global::MutationId mutationId, global::System.Collections.Generic.List<global::Item> removeItems)
	{
		this.UnitSave.mutations.Add(mutationId);
		global::Mutation mutation = new global::Mutation(mutationId, this);
		this.Mutations.Add(mutation);
		for (int i = this.Injuries.Count - 1; i >= 0; i--)
		{
			if (mutation.HasBodyPart(this.Injuries[i].Data.BodyPartId))
			{
				this.UnitSave.injuries.RemoveAt(i);
				this.Injuries.RemoveAt(i);
			}
		}
		global::PandoraDebug.LogInfo(string.Concat(new object[]
		{
			"Mutation ",
			mutation.Data.Id,
			" added to ",
			this.Name
		}), "uncategorised", null);
		if (mutation.GroupData.UnitSlotId != global::UnitSlotId.NONE)
		{
			this.ReequipWeapons(removeItems);
		}
		if (this.BothArmsMutated())
		{
			for (int j = 6; j < this.Items.Count; j++)
			{
				removeItems.AddRange(this.EquipItem((global::UnitSlotId)j, global::ItemId.NONE));
			}
		}
		this.UpdateAttributesAndCheckBackPack(removeItems);
		return mutation;
	}

	public int GetCRC()
	{
		int num = 0;
		num += this.attributes[113];
		num += this.attributes[6];
		num += this.attributes[123];
		num += this.attributes[11];
		num += this.attributes[12];
		num += this.attributes[144];
		num += this.attributes[141];
		num += this.attributes[140];
		num += this.attributes[146];
		num += this.attributes[142];
		num += this.attributes[143];
		num += this.attributes[145];
		num += this.attributes[139];
		for (int i = 0; i < this.Enchantments.Count; i++)
		{
			num = (int)(num + this.Enchantments[i].Id);
		}
		return num;
	}

	public void ResetUnitSkills()
	{
		for (int i = this.ActiveSkills.Count - 1; i >= 0; i--)
		{
			global::SkillData skillData = this.ActiveSkills[i];
			if (skillData.SkillIdPrerequiste != global::SkillId.NONE || skillData.SkillQualityId != global::SkillQualityId.MASTER_QUALITY)
			{
				this.UnitSave.activeSkills.Remove(skillData.Id);
				this.ActiveSkills.RemoveAt(i);
			}
		}
		for (int j = this.PassiveSkills.Count - 1; j >= 0; j--)
		{
			global::SkillData skillData2 = this.PassiveSkills[j];
			if (skillData2.SkillIdPrerequiste != global::SkillId.NONE || skillData2.SkillQualityId != global::SkillQualityId.MASTER_QUALITY)
			{
				this.UnitSave.passiveSkills.Remove(skillData2.Id);
				this.PassiveSkills.RemoveAt(j);
			}
		}
		this.spentSkillPoints = 0;
	}

	public static readonly global::AttributeId[] PhysicalAttributeIds = new global::AttributeId[]
	{
		global::AttributeId.STRENGTH,
		global::AttributeId.TOUGHNESS,
		global::AttributeId.AGILITY
	};

	public static readonly global::AttributeId[] MentalAttributeIds = new global::AttributeId[]
	{
		global::AttributeId.LEADERSHIP,
		global::AttributeId.INTELLIGENCE,
		global::AttributeId.ALERTNESS
	};

	public static readonly global::AttributeId[] MartialAttributeIds = new global::AttributeId[]
	{
		global::AttributeId.WEAPON_SKILL,
		global::AttributeId.BALLISTIC_SKILL,
		global::AttributeId.ACCURACY
	};

	public static readonly global::InjuryId[] HIRE_UNIT_INJURY_EXCLUDES = new global::InjuryId[]
	{
		global::InjuryId.DEAD,
		global::InjuryId.MULTIPLE_INJURIES,
		global::InjuryId.NEAR_DEATH,
		global::InjuryId.FULL_RECOVERY,
		global::InjuryId.AMNESIA
	};

	public int warbandIdx;

	public int warbandPos;

	public bool isAI;

	private global::UnitSlotId activeWeaponSlot;

	public global::AnimStyleId currentAnimStyleId;

	private global::System.Collections.Generic.List<global::Item> items;

	private global::System.Collections.Generic.List<global::BodyPartData> availableBodyParts;

	public global::System.Collections.Generic.Dictionary<global::BodyPartId, global::BodyPart> bodyParts;

	public global::System.Collections.Generic.Dictionary<global::AttributeId, global::System.Collections.Generic.List<global::AttributeMod>> attributeModifiers;

	public global::Item deathTrophy;

	public int tempStrategyPoints;

	public int tempOffensePoints;

	private int[] attributes;

	private int[] tempAttributes;

	private static global::AttributeData[] attributeDataById;

	private static global::System.Collections.Generic.Dictionary<global::AttributeId, global::System.Collections.Generic.List<global::AttributeAttributeData>> attributeAttributeDataById;

	private global::System.Collections.Generic.Dictionary<int, global::AttributeId> maxAttributes;

	private global::System.Collections.Generic.Dictionary<global::UnitActionId, global::CostModifier> actionCostModifiers;

	private global::System.Collections.Generic.Dictionary<global::SkillId, global::CostModifier> skillCostModifiers;

	private global::System.Collections.Generic.Dictionary<global::SkillId, int> damagePercModifiers;

	private global::System.Collections.Generic.Dictionary<global::SpellTypeId, global::CostModifier> spellTypeModifiers;

	private global::System.Collections.Generic.Dictionary<global::UnitActionId, global::EnchantmentId> blockedActions;

	private global::System.Collections.Generic.Dictionary<global::SkillId, global::EnchantmentId> blockedSkills;

	private global::System.Collections.Generic.Dictionary<global::BoneId, global::EnchantmentId> blockedBones;

	private global::System.Collections.Generic.Dictionary<global::ItemTypeId, global::EnchantmentId> blockedItemTypes;

	private global::UnitStateId newState;

	private global::System.Collections.Generic.List<global::AttributeData> attributeDataList;

	private global::System.Collections.Generic.List<global::EnchantmentTypeId> enchantTypeImmunities;

	private global::System.Collections.Generic.List<global::EnchantmentTypeId> enchantTypeToBeRemoved;

	private global::System.Collections.Generic.List<global::EnchantmentId> enchantToBeRemoved;

	private global::System.Collections.Generic.List<global::UnitJoinAttributeData> baseAttributesData;

	private global::System.Collections.Generic.List<global::CampaignUnitJoinAttributeData> campaignModifiers;

	private global::System.Collections.Generic.List<global::UnitTypeAttributeData> baseAttributesDataMax;

	private global::System.Collections.Generic.List<global::UnitJoinUnitRankData> ranksData;

	private int totalPhysicalPoints;

	private int spentPhysicalPoints;

	private int totalMentalPoints;

	private int spentMentalPoints;

	private int totalMartialPoints;

	private int spentMartialPoints;

	private int totalSkillPoints;

	private int spentSkillPoints;

	private int totalSpellPoints;

	private int spentSpellPoints;

	private global::AttributeModList weaponDamageModifiers = new global::AttributeModList();

	private bool needFxRefresh;

	private int realBackPackCapacity;

	private int cachedBackpackCapacity = -1;

	private global::Unit stunningUnit;

	private int baseMoralImpact;

	private readonly global::System.Collections.Generic.Dictionary<global::InjuryId, int> injuryRollModifiers = new global::System.Collections.Generic.Dictionary<global::InjuryId, int>(global::InjuryIdComparer.Instance);
}
