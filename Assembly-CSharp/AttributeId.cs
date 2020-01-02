﻿using System;

public enum AttributeId
{
	NONE,
	MOVEMENT,
	WEAPON_SKILL,
	BALLISTIC_SKILL,
	STRENGTH,
	TOUGHNESS,
	WOUND,
	AGILITY,
	LEADERSHIP = 9,
	INTELLIGENCE,
	OFFENSE_POINTS,
	STRATEGY_POINTS,
	ALERTNESS = 14,
	ACCURACY,
	CLIMB_ROLL_3,
	LEAP_ROLL,
	JUMP_DOWN_ROLL_3,
	POISON_RESIST_ROLL = 21,
	ALL_ALONE_ROLL = 23,
	FEAR_ROLL,
	TERROR_ROLL = 26,
	WARBAND_ROUT_ROLL,
	TZEENTCHS_CURSE_ROLL,
	SPELLCASTING_ROLL = 30,
	PERCEPTION_ROLL,
	COMBAT_MELEE_HIT_ROLL = 33,
	COMBAT_RANGE_HIT_ROLL = 35,
	CRITICAL_MELEE_ATTEMPT_ROLL,
	CRITICAL_RANGE_ATTEMPT_ROLL,
	CRITICAL_RESULT_ROLL,
	STUPIDITY_ROLL,
	PARRYING_ROLL,
	DODGE_ROLL,
	ARMOR_ABSORPTION,
	DAMAGE_BONUS,
	DAMAGE_MIN,
	DAMAGE_MAX,
	DAMAGE_CRITICAL_BONUS,
	BYPASS_ARMOR = 48,
	DAMAGE_HOLY,
	DAMAGE_UNHOLY,
	CRIT_RESISTANCE,
	INJURY_ROLL,
	MAGIC_RESISTANCE,
	LOCKPICKING_ROLL,
	WYRDSTONE_RESIST_ROLL,
	DAMAGE_BONUS_MELEE_PERC = 61,
	DAMAGE_BONUS_RANGE_PERC,
	DAMAGE_BONUS_DIV_MAG_PERC,
	DAMAGE_BONUS_ARC_MAG_PERC,
	DIVINE_WRATH_ROLL,
	MORAL = 67,
	CHARGE_MOVEMENT = 69,
	AMBUSH_MOVEMENT,
	DAMAGE_CRITICAL_BONUS_PERC,
	DODGE_DEFENDER_MODIFIER,
	PARRY_DEFENDER_MODIFIER,
	RANGE_RESISTANCE,
	MELEE_RESISTANCE,
	INITIATIVE,
	VIEW_DISTANCE = 79,
	DAMAGE_BONUS_MELEE,
	DAMAGE_BONUS_RANGE,
	DAMAGE_BONUS_SPELL,
	RANGE_BONUS_SPELL,
	DAMAGE_BONUS_HOLY_PERC,
	DAMAGE_BONUS_UNHOLY_PERC,
	ATTACK_PER_ACTION = 87,
	PARRY_LEFT,
	DODGE_LEFT,
	OVERWATCH_LEFT,
	AMBUSH_LEFT,
	DAMAGE_BONUS_CHARGE_PERC = 94,
	AGILITY_MAX,
	STRENGTH_MAX,
	TOUGHNESS_MAX,
	LEADERSHIP_MAX,
	INTELLIGENCE_MAX,
	ALERTNESS_MAX,
	WEAPON_SKILL_MAX,
	BALLISTIC_SKILL_MAX,
	ACCURACY_MAX,
	POISON_RESIST_DEFENDER_MODIFIER,
	CLIMB_ROLL_6 = 109,
	CLIMB_ROLL_9,
	JUMP_DOWN_ROLL_6,
	JUMP_DOWN_ROLL_9,
	CURRENT_WOUND,
	LEADERSHIP_ROLL,
	TRAP_RESIST_ROLL,
	COUNTER_DISABLED,
	COUNTER_FORCED,
	MAGIC_RESIST_DEFENDER_MODIFIER = 121,
	CURRENT_STRATEGY_POINTS,
	CURRENT_OFFENSE_POINTS,
	WOUND_PERC,
	DODGE_BYPASS,
	PARRY_BYPASS,
	STUN_RESIST_ROLL,
	DIVINE_SPELLCASTING_ROLL,
	ARCANE_SPELLCASTING_ROLL,
	ARMOR_ABSORPTION_PERC,
	BYPASS_ARMOR_PERC,
	RANGE_HIT_ROLL_BONUS_HIGHER,
	RANGE_HIT_ROLL_BONUS_LOWER,
	RANGE_HIT_ROLL_BONUS_ENGAGED,
	RANGE_HIT_ROLL_BONUS_STUNNED,
	RANGE_RESISTANCE_DEFENDER_MODIFIER,
	GLOBAL_RANGE_DAMAGE_PERC,
	GLOBAL_MELEE_DAMAGE_PERC,
	TOTAL_WIN,
	TOTAL_LOST,
	TOTAL_KILL,
	TOTAL_OOA,
	TOTAL_PRAY,
	TOTAL_DAMAGE,
	TOTAL_SEARCH,
	TOTAL_MVU,
	RANK,
	MORAL_IMPACT,
	CURRENT_MVU,
	AI_VIEW_DISTANCE,
	TOTAL_KILL_ROAMING,
	MAX_VALUE
}
