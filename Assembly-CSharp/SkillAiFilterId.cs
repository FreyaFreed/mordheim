﻿using System;

public enum SkillAiFilterId
{
	NONE,
	KNEE_SHOT_VALID_INITIATIVE = 2,
	NERVE_SHOT_VALID_PARRYING_ROLL,
	NERVE_SHOT_MSTR_VALID_PARRYING_ROLL,
	NERVE_SHOT_MSTR_VALID_DODGE_ROLL,
	NERVE_SHOT_MSTR_VALID_PARRY_LEFT,
	NERVE_SHOT_MSTR_VALID_DODGE_LEFT,
	NERVE_SHOT_VALID_PARRY_LEFT,
	HEAD_SHOT_VALID_STUN_RESIST_ROLL,
	HEAD_SHOT_EXCLUDED_STUN_RESIST_ROLL,
	KNEE_SHOT_MSTR_VALID_INITIATIVE,
	HEAD_SHOT_MSTR_VALID_STUN_RESIST_ROLL,
	HEAD_SHOT_MSTR_EXCLUDED_STUN_RESIST_ROLL,
	KNEE_SHOT_EXCLUDED_INITIATIVE,
	KNEE_SHOT_MSTR_EXCLUDED_INITIATIVE,
	PUNCTURE_VALID_MELEE_RESISTANCE = 26,
	PUNCTURE_EXCLUDED_MELEE_RESISTANCE,
	PUNCTURE_MSTR_VALID_MELEE_RESISTANCE,
	PUNCTURE_MSTR_EXCLUDED_MELEE_RESISTANCE,
	STAGGERING_BLOW_VALID_INITIATIVE = 36,
	STAGGERING_BLOW_EXCLUDED_INITIATIVE,
	STAGGERING_BLOW_MSTR_VALID_INITIATIVE,
	STAGGERING_BLOW_MSTR_EXCLUDED_INITIATIVE,
	PRECISE_STRIKE_VALID_DODGE_LEFT = 43,
	PRECISE_STRIKE_VALID_PARRY_LEFT,
	PRECISE_STRIKE_EXCLUDED_DODGE_LEFT,
	PRECISE_STRIKE_EXCLUDED_PARRY_LEFT,
	PRECISE_STRIKE_MSTR_VALID_DODGE_LEFT,
	PRECISE_STRIKE_MSTR_VALID_PARRY_LEFT,
	PRECISE_STRIKE_MSTR_EXCLUDED_DODGE_LEFT,
	PRECISE_STRIKE_MSTR_EXCLUDED_PARRY_LEFT,
	PARADE_VALID_CURRENT_OFFENSE_POINTS = 53,
	PARADE_EXCLUDED_CURRENT_OFFENSE_POINTS,
	PARADE_MSTR_VALID_CURRENT_OFFENSE_POINTS,
	PARADE_MSTR_EXCLUDED_CURRENT_OFFENSE_POINTS,
	EXECUTION_VALID_ARMOR_ABSORPTION_PERC,
	EXECUTION_EXCLUDED_ARMOR_ABSORPTION_PERC,
	EXECUTION_MSTR_VALID_ARMOR_ABSORPTION_PERC,
	EXECUTION_MSTR_EXCLUDED_ARMOR_ABSORPTION_PERC,
	CARELESS_STRIKE_EXCLUDED_ENGAGED,
	CARELESS_STRIKE_MSTR_EXCLUDED_ENGAGED,
	PINNING_SHOT_VALID_NOT_ENGAGED,
	PINNING_SHOT_MSTR_VALID_NOT_ENGAGED,
	CRIPPLING_SHOT_VALID_MIN_ROLL,
	CRIPPLING_SHOT_MSTR_VALID_MIN_ROLL,
	JAW_STRIKE_VALID_HAS_SPELL,
	JAW_STRIKE_MSTR_VALID_HAS_SPELL,
	SNIPER_SHOT_VALID_COMBAT_RANGE_HIT_ROLL = 73,
	SNIPER_SHOT_MSTR_VALID_COMBAT_RANGE_HIT_ROLL = 75,
	SQUIRES_CURSE_VALID_HAS_ALT_SET = 77,
	HAMSTRING_VALID_MIN_ROLL,
	HAMSTRING_MSTR_VALID_MIN_ROLL,
	RIGHTEOUS_FURY_VALID_MIN_ROLL,
	RIGHTEOUS_FURY_MSTR_VALID_MIN_ROLL = 82,
	OVERPOWER_VALID_STUN_RESIST_ROLL = 84,
	OVERPOWER_EXCLUDED_STUN_RESIST_ROLL,
	OVERPOWER_MSTR_VALID_STUN_RESIST_ROLL = 88,
	OVERPOWER_MSTR_EXCLUDED_STUN_RESIST_ROLL,
	MIGHTY_CHARGE_VALID_HIT_ROLL = 92,
	MIGHTY_CHARGE_MSTR_VALID_HIT_ROLL,
	BASE_STANCE_PARRY_EXCLUDED_PARRY_ROLL = 95,
	ENTRENCHED_VALID_NO_ENEMY = 97,
	ENTRENCHED_MSTR_VALID_NO_ENEMY,
	WEB_OF_STEEL_EXCLUDED_PARRY_ROLL,
	WEB_OF_STEEL_MSTR_EXCLUDED_PARRY_ROLL,
	DEFENSIVE_STANCE_VALID_ENGAGED = 103,
	DEFENSIVE_STANCE_MSTR_VALID_ENGAGED,
	DEFT_STANCE_VALID_HAS_BEEN_SHOT,
	DEFT_STANCE_MSTR_VALID_HAS_BEEN_SHOT,
	SAFE_STANCE_VALID_ENGAGED,
	SAFE_STANCE_MSTR_VALID_ENGAGED,
	READY_STANCE_VALID_NO_ENEMY,
	READY_STANCE_MSTR_VALID_NO_ENEMY,
	TOUCH_OF_PALSY_VALID_RANGED,
	TOUCH_OF_PALSY_MSTR_VALID_RANGED,
	GUARD_STANCE_VALID_ENGAGED,
	GUARD_STANCE_MSTR_VALID_ENGAGED,
	BALANCE_VALID_HAS_BEEN_SHOT,
	BALANCE_VALID_MSTR_HAS_BEEN_SHOT,
	HAND_SHOT_VALID_MIN_ROLL,
	HAND_SHOT_MSTR_VALID_MIN_ROLL,
	STRONG_BLOW_VALID_COMBAT_MELEE_HIT_ROLL = 121,
	STRONG_BLOW_MSTR_VALID_COMBAT_MELEE_HIT_ROLL,
	PERFECT_STRIKE_MSTR_VALID_MIN_ROLL,
	PERFECT_STRIKE_VALID_MIN_ROLL,
	BUGMANS_ALE_NORMAL_VALID_ENGAGED,
	BUGMANS_ALE_GOOD_VALID_ENGAGED,
	BUGMANS_ALE_BEST_VALID_ENGAGED,
	ELVEN_WINE_NORMAL_VALID_ENGAGED,
	ELVEN_WINE_GOOD_VALID_ENGAGED,
	ELVEN_WINE_BEST_VALID_ENGAGED,
	MANDRAKE_ROOT_NORMAL_VALID_ENGAGED,
	MANDRAKE_ROOT_GOOD_VALID_ENGAGED,
	MANDRAKE_ROOT_BEST_VALID_ENGAGED,
	MAD_CAP_MUSHROOM_NORMAL_VALID_ENGAGED = 135,
	MAD_CAP_MUSHROOM_GOOD_VALID_ENGAGED,
	MAD_CAP_MUSHROOM_BEST_VALID_ENGAGED,
	HEALING_DRAUGHT_NORMAL_VALID_WOUND,
	HEALING_DRAUGHT_NORMAL_EXCLUDED_WOUND,
	HEALING_DRAUGHT_GOOD_VALID_WOUND,
	HEALING_DRAUGHT_GOOD_EXCLUDED_WOUND,
	HEALING_DRAUGHT_BEST_VALID_WOUND,
	HEALING_DRAUGHT_BEST_EXCLUDED_WOUND,
	POULTICE_NORMAL_EXCLUDED_ALL,
	POULTICE_GOOD_EXCLUDED_ALL,
	POULTICE_BEST_EXCLUDED_ALL,
	DRAUGHT_CONCENTRATION_NORMAL_VALID_SPELLCASTER,
	DRAUGHT_CONCENTRATION_GOOD_VALID_SPELLCASTER,
	DRAUGHT_CONCENTRATION_BEST_VALID_SPELLCASTER,
	DRAUGHT_CLARITY_NORMAL_EXCLUDED_NO_ENEMY,
	DRAUGHT_CLARITY_GOOD_EXCLUDED_NO_ENEMY,
	DRAUGHT_CLARITY_BEST_EXCLUDED_NO_ENEMY,
	LUCKY_CHARM_NORMAL_EXCLUDED_ALL,
	LUCKY_CHARM_GOOD_EXCLUDED_ALL,
	LUCKY_CHARM_BEST_EXCLUDED_ALL,
	TALISMAN_NORMAL_EXCLUDED_NO_ENEMY,
	TALISMAN_GOOD_EXCLUDED_NO_ENEMY,
	TALISMAN_BEST_EXCLUDED_NO_ENEMY,
	IGNORE_PAIN_VALID_ENGAGED,
	IGNORE_PAIN_EXCLUDED_WOUNDS,
	IGNORE_PAIN_MSTR_VALID_ENGAGED,
	IGNORE_PAIN_MSTR_EXCLUDED_WOUNDS,
	FRENZY_VALID_ENGAGED,
	FRENZY_EXCLUDED_SPELLCASTER,
	FRENZY_MSTR_VALID_ENGAGED,
	FRENZY_MSTR_EXCLUDED_SPELLCASTER,
	CAUTERIZE_EXCLUDED_ENGAGED,
	CAUTERIZE_MSTR_EXCLUDED_ENGAGED,
	ORDER_VALID_SELF_NOT_ENGAGED,
	ORDER_VALID_ENGAGED,
	ORDER_MSTR_VALID_SELF_NOT_ENGAGED,
	ORDER_MSTR_VALID_ENGAGED,
	COURAGE_VALID_ENGAGED,
	COURAGE_MSTR_VALID_ENGAGED,
	WAR_CRY_VALID_ENEMY_IN_SIGHT,
	WAR_CRY_MSTR_VALID_ENGAGED,
	THREATEN_VALID_ENGAGED,
	THREATEN_MSTR_VALID_ENGAGED,
	HOLD_GROUND_VALID_ENGAGED,
	HOLD_GROUND_MSTR_VALID_ENGAGED,
	INTIMIDATE_VALID_ENGAGED,
	INTIMIDATE_MSTR_VALID_ENGAGED,
	INSULT_VALID_MELEE_RESISTANCE,
	INSULT_MSTR_VALID_MELEE_RESISTANCE,
	INSULT_EXCLUDED_MELEE_RESISTANCE,
	INSULT_MSTR_EXCLUDED_MELEE_RESISTANCE,
	BATTLE_PLAN_VALID_OP,
	BATTLE_PLAN_MSTR_VALID_OP,
	BATTLE_PLAN_EXCLUDED_NO_OP,
	BATTLE_PLAN_EXCLUDED_MAX_OP,
	BATTLE_PLAN_MSTR_EXCLUDED_NO_OP,
	BATTLE_PLAN_MSTR_EXCLUDED_MAX_OP,
	COMBAT_SAVVY_VALID_ENGAGED,
	COMBAT_SAVVY_MSTR_VALID_ENGAGED,
	STIMULUS_VALID_INITIATIVE,
	STIMULUS_MSTR_VALID_INITIATIVE,
	STIMULUS_EXCLUDED_INITIATIVE,
	STIMULUS_MSTR_EXCLUDED_INITIATIVE,
	SCOUTS_ADVICE_VALID_HAS_BEEN_SHOT,
	SCOUTS_ADVICE_MSTR_VALID_HAS_BEEN_SHOT,
	SCOUTS_ADVICE_EXCLUDED_RANGED_RESISTANCE,
	SCOUTS_ADVICE_MSTR_EXCLUDED_RANGED_RESISTANCE,
	BLACK_HUNGER_VALID_ENGAGED,
	BLACK_HUNGER_EXCLUDED_WOUNDS,
	BLACK_HUNGER_MSTR_VALID_ENGAGED,
	BLACK_HUNGER_MSTR_EXCLUDED_WOUNDS,
	FOR_SIGMAR_VALID_NOT_ENGAGED,
	FOR_SIGMAR_EXCLUDED_ENGAGED,
	SIGN_OF_SIGMAR_VALID_ENGAGED,
	SIGN_OF_SIGMAR_MSTR_VALID_ENGAGED,
	ATTRACTING_LURE_VALID_ENGAGED,
	ATTRACTING_LURE_MSTR_VALID_ENGAGED,
	HYPNOTIC_ALLURE_VALID_NOT_ENGAGED,
	HYPNOTIC_ALLURE_EXCLUDED_ENGAGED,
	BASE_STANCE_PARRY_VALID_ENGAGED,
	BASE_STANCE_DODGE_VALID_ENGAGED,
	WEB_OF_STEEL_VALID_ENGAGED,
	WEB_OF_STEEL_MSTR_VALID_ENGAGED,
	SIDESTEP_VALID_ENGAGED,
	SIDESTEP_MSTR_VALID_ENGAGED,
	BLOOD_OFFERING_VALID_MIN_ROLL = 224,
	BLOOD_OFFERING_MSTR_VALID_MIN_ROLL,
	SWIFT_CHARGE_VALID_HIT_ROLL,
	SWIFT_CHARGE_MSTR_VALID_HIT_ROLL,
	FRENZY_EXCLUDED_RANGED,
	FRENZY_MSTR_EXCLUDED_RANGED,
	ADRENALINE_RUSH_VALID_OP,
	ADRENALINE_RUSH_EXCLUDED_WOUNDS,
	ADRENALINE_RUSH_MSTR_VALID_OP,
	ADRENALINE_RUSH_MSTR_EXCLUDED_WOUNDS,
	EXHAUSTION_VALID_SP,
	EXHAUSTION_EXCLUDED_WOUNDS,
	EXHAUSTION_MSTR_VALID_SP,
	EXHAUSTION_MSTR_EXCLUDED_WOUNDS,
	INTENSITY_VALID_NO_ENEMY,
	INTENSITY_VALID_RANGED,
	INTENSITY_MSTR_VALID_NO_ENEMY,
	INTENSITY_MSTR_VALID_RANGED,
	INTENSITY_EXCLUDED_WOUNDS,
	INTENSITY_MSTR_EXCLUDED_WOUNDS,
	RETREAT_VALID_ALL_ALONE,
	RETREAT_VALID_LOW_HEALTH,
	RETREAT_MSTR_VALID_ALL_ALONE,
	RETREAT_MSTR_VALID_LOW_HEALTH,
	COORDINATION_VALID_ALLIES,
	COORDINATION_MSTR_VALID_ALLIES,
	RALLYING_CRY_VALID_ALL_ALONE,
	RALLYING_CRY_MSTR_VALID_ALL_ALONE,
	TAUNT_VALID_LOW_HEALTH,
	TAUNT_VALID_RANGED,
	TAUNT_MSTR_VALID_LOW_HEALTH,
	TAUNT_MSTR_VALID_RANGED,
	THREATEN_EXCLUDED_LOW_HEALTH,
	THREATEN_MSTR_EXCLUDED_LOW_HEALTH,
	GUIDANCE_VALID_STUPIDITY,
	GUIDANCE_VALID_PARANOIA,
	GUIDANCE_VALID_MEGALOMANIA,
	GUIDANCE_VALID_HEROIC_IDIOCY,
	GUIDANCE_VALID_HALF_CRAZY,
	GUIDANCE_MSTR_VALID_STUPIDITY,
	GUIDANCE_MSTR_VALID_PARANOIA,
	GUIDANCE_MSTR_VALID_MEGALOMANIA,
	GUIDANCE_MSTR_VALID_HEROIC_IDIOCY,
	GUIDANCE_MSTR_VALID_HALF_CRAZY,
	MEDITATION_VALID_SP,
	MEDITATION_EXCLUDED_MAX_SP,
	MEDITATION_MSTR_VALID_SP,
	MEDITATION_MSTR_EXCLUDED_MAX_SP,
	INTROSPECTION_VALID_OP,
	INTROSPECTION_EXCLUDED_MAX_OP,
	INTROSPECTION_MSTR_VALID_OP,
	INTROSPECTION_MSTR_EXCLUDED_MAX_OP,
	EXPLOIT_POSITIONING_VALID_RANGED,
	EXPLOIT_POSITIONING_EXCLUDED_SISTERS,
	EXPLOIT_POSITIONING_MSTR_VALID_RANGED,
	EXPLOIT_POSITIONING_MSTR_EXCLUDED_SISTERS,
	CARELESS_STRIKE_EXCLUDED_ONCE = 282,
	CARELESS_STRIKE_MSTR_EXCLUDED_ONCE,
	SLEEPING_POISON_EXCLUDED_ONCE,
	SLEEPING_POISON_MSTR_EXCLUDED_ONCE,
	NUMBING_POISON_EXCLUDED_ONCE,
	NUMBING_POISON_MSTR_EXCLUDED_ONCE,
	TACTICIAN_VALID_LOW_HEALTH,
	TACTICIAN_VALID_MSTR_LOW_HEALTH,
	PRAYER_OF_SWIFTNESS_VALID_NO_PARRY,
	PRAYER_OF_SWIFTNESS_VALID_DODGE,
	PRAYER_OF_SWIFTNESS_MSTR_VALID_NO_PARRY,
	PRAYER_OF_SWIFTNESS_MSTR_VALID_DODGE,
	STENCH_OF_NURGLE_EXCLUDED_ONCE,
	ANTI_TOXIN_NORMAL_VALID_POISON,
	ANTI_TOXIN_GOOD_VALID_POISON,
	ANTI_TOXIN_BEST_VALID_POISON,
	GRETAS_BOON_NORMAL_EXCLUDED_ALL,
	GRETAS_BOON_GOOD_EXCLUDED_ALL,
	GRETAS_BOON_BEST_EXCLUDED_ALL,
	ANTIDOTE_NORMAL_VALID_POISON,
	ANTIDOTE_GOOD_VALID_POISON,
	ANTIDOTE_BEST_VALID_POISON,
	AETHYRIC_ELIXIR_NORMAL_VALID_MAGIC,
	AETHYRIC_ELIXIR_GOOD_VALID_MAGIC,
	AETHYRIC_ELIXIR_BEST_VALID_MAGIC,
	DRAUGHT_FOCUS_NORMAL_VALID_STUPIDITY,
	DRAUGHT_FOCUS_GOOD_VALID_STUPIDITY,
	DRAUGHT_FOCUS_BEST_VALID_STUPIDITY,
	DRAUGHT_LUCIDITY_NORMAL_VALID_HALF_CRAZY,
	DRAUGHT_LUCIDITY_GOOD_VALID_HALF_CRAZY,
	DRAUGHT_LUCIDITY_BEST_VALID_HALF_CRAZY,
	DRAUGHT_CALM_NORMAL_VALID_PARANOIA,
	DRAUGHT_CALM_GOOD_VALID_PARANOIA,
	DRAUGHT_CALM_BEST_VALID_PARANOIA,
	SMELLING_SALTS_NORMAL_VALID_STUNNED,
	SMELLING_SALTS_GOOD_VALID_STUNNED,
	SMELLING_SALTS_BEST_VALID_STUNNED,
	DREAD_OF_ARAMAR_VALID_MIN_ROLL,
	DREAD_OF_ARAMAR_MSTR_VALID_MIN_ROLL,
	ARMOR_OF_LEAD_VALID_MIN_ROLL,
	ARMOR_OF_LEAD_MSTR_VALID_MIN_ROLL,
	ARMOR_OF_LEAD_EXCLUDED_ENGAGED,
	ARMOR_OF_LEAD_MSTR_EXCLUDED_ENGAGED,
	BLINDING_LIGHT_VALID_MIN_ROLL,
	BLINDING_LIGHT_MSTR_VALID_MIN_ROLL,
	CURSE_OF_RUST_VALID_ARMOR,
	CURSE_OF_RUST_EXCLUDED_ARMOR,
	CURSE_OF_RUST_MSTR_VALID_ARMOR,
	CURSE_OF_RUST_MSTR_EXCLUDED_ARMOR,
	ENSHROUDING_MIST_VALID_MIN_ROLL,
	ENSHROUDING_MIST_MSTR_VALID_MIN_ROLL,
	ENSHROUDING_MIST_EXCLUDED_NOT_ENGAGED,
	ENSHROUDING_MIST_MSTR_EXCLUDED_NOT_ENGAGED,
	MUSK_OF_COURAGE_VALID_ALL_ALONE,
	MUSK_OF_COURAGE_MSTR_VALID_ALL_ALONE,
	WITHER_VALID_MIN_ROLL,
	WITHER_MSTR_VALID_MIN_ROLL,
	SORCERERS_CURSE_VALID_ARMOR,
	SORCERERS_CURSE_MSTR_VALID_ARMOR,
	SORCERERS_CURSE_MSTR_EXCLUDED_ARMOR,
	SORCERERS_CURSE_EXCLUDED_ARMOR,
	GAZE_HORNED_RAT_MSTR_VALID_MIN_ROLL,
	GAZE_HORNED_RAT_VALID_MIN_ROLL,
	BLESS_WITH_FILTH_VALID_ENGAGED,
	BLESS_WITH_FILTH_VALID_RANGED,
	BLESS_WITH_FILTH_MSTR_VALID_ENGAGED,
	BLESS_WITH_FILTH_MSTR_VALID_RANGED,
	POISONOUS_MIST_VALID_MIN_ROLL,
	POISONOUS_MIST_MSTR_VALID_MIN_ROLL,
	BLESSING_OF_SPEED_VALID_NO_ENEMY,
	BLESSING_OF_SPEED_MSTR_VALID_NO_ENEMY,
	BLESSING_OF_SPEED_EXCLUDED_ENGAGED,
	BLESSING_OF_SPEED_MSTR_EXCLUDED_ENGAGED,
	HEARTS_OF_GRIFFON_VALID_ALL_ALONE,
	HEARTS_OF_GRIFFON_MSTR_VALID_ALL_ALONE,
	HAMMER_OF_SIGMAR_VALID_ENGAGED,
	HAMMER_OF_SIGMAR_MSTR_VALID_ENGAGED,
	SINFUL_SPEECH_VALID_SPELLCASTER,
	SINFUL_SPEECH_MSTR_VALID_SPELLCASTER,
	SIGMARS_SECOND_WIND_VALID_STUNNED,
	SIGMARS_SECOND_WIND_VALID_ENGAGED,
	SIGMARS_SECOND_WIND_MSTR_VALID_STUNNED,
	SIGMARS_SECOND_WIND_MSTR_VALID_ENGAGED,
	DIVINE_REVELATION_VALID_HAS_BEEN_SHOT,
	DIVINE_REVELATION_VALID_LOW_HEALTH,
	DIVINE_REVELATION_MSTR_VALID_HAS_BEEN_SHOT,
	DIVINE_REVELATION_MSTR_VALID_LOW_HEALTH,
	HEALING_CIRCLE_VALID_LOW_HEALTH,
	HEALING_CIRCLE_MSTR_VALID_LOW_HEALTH,
	VEIL_OF_CORRUPTION_VALID_NEVER,
	VEIL_OF_CORRUPTION_MSTR_VALID_NEVER,
	VEIL_OF_CORRUPTION_EXCLUDED_NO_ENEMY,
	VEIL_OF_CORRUPTION_MSTR_EXCLUDED_NO_ENEMY,
	BLADE_OF_DESTRUCTION_VALID_MIN_ROLL,
	BLADE_OF_DESTRUCTION_MSTR_VALID_MIN_ROLL,
	BLADE_OF_DESTRUCTION_EXCLUDED_NO_ENEMY,
	BLADE_OF_DESTRUCTION_MSTR_EXCLUDED_NO_ENEMY,
	CURSE_OF_CHAOS_VALID_MIN_ROLL,
	CURSE_OF_CHAOS_MSTR_VALID_MIN_ROLL,
	ACID_BREATH_VALID_MIN_ROLL,
	ACID_BREATH_MSTR_VALID_MIN_ROLL,
	VISION_OF_TORMENT_VALID_ENGAGED,
	VISION_OF_TORMENT_MSTR_VALID_ENGAGED,
	BOON_OF_CHAOS_VALID_MIN_ROLL,
	BOON_OF_CHAOS_MSTR_VALID_MIN_ROLL,
	BOON_OF_CHAOS_EXCLUDED_NO_ENEMY,
	BOON_OF_CHAOS_MSTR_EXCLUDED_NO_ENEMY,
	CHAINS_OF_CHAOS_VALID_MIN_ROLL,
	CHAINS_OF_CHAOS_MSTR_VALID_MIN_ROLL,
	MARTIAL_INFLUENCE_VALID_ENGAGED = 395,
	FIGHT_ME_VALID_LOW_HEALTH,
	FIGHT_ME_VALID_RANGED,
	STIFLING_BLOW_VALID_MELEE_CRIT,
	STIFLING_BLOW_VALID_RANGED_CRIT,
	STIFLING_BLOW_EXCLUDED_MELEE_CRIT,
	STIFLING_BLOW_EXCLUDED_RANGED_CRIT,
	SCURRY_VALID_ENGAGED,
	SHREWD_DISTRACTION_VALID_DODGE_STANCE,
	SHREWD_DISTRACTION_VALID_PARRY_STANCE,
	CORROSIVE_ACID_EXCLUDED_NO_ENEMY,
	MASS_SUPPLICATION_VALID_SPELLCASTER,
	PENITENCE_VALID_OP,
	PENITENCE_EXCLUDED_OP,
	RENEWAL_VALID_LOW_HEALTH,
	RENEWAL_EXCLUDED_WOUNDS,
	DARK_GIFT_VALID_ENGAGED,
	DARK_GIFT_VALID_RANGED,
	FORSAKE_VALID_SPELLCASTER,
	KNEE_SHOT_EXCLUDED_ONCE_TARGET,
	KNEE_SHOT_MSTR_EXCLUDED_ONCE_TARGET,
	HAND_SHOT_EXCLUDED_ONCE_TARGET,
	HAND_SHOT_MSTR_EXCLUDED_ONCE_TARGET,
	NERVE_SHOT_EXCLUDED_ONCE_TARGET,
	NERVE_SHOT_MSTR_EXCLUDED_ONCE_TARGET,
	PINNING_SHOT_EXCLUDED_ONCE_TARGET,
	PINNING_SHOT_MSTR_EXCLUDED_ONCE_TARGET,
	CRIPPLING_SHOT_EXCLUDED_ONCE_TARGET,
	CRIPPLING_SHOT_MSTR_EXCLUDED_ONCE_TARGET,
	HEAD_SHOT_EXCLUDED_ONCE_TARGET,
	HEAD_SHOT_MSTR_EXCLUDED_ONCE_TARGET,
	SQUIRES_CURSE_EXCLUDED_NO_ALT_SET,
	SQUIRES_CURSE_EXCLUDED_ONCE_TARGET,
	RIGHTEOUS_FURY_EXCLUDED_ONCE_TARGET,
	RIGHTEOUS_FURY_MSTR_EXCLUDED_ONCE_TARGET,
	TOUCH_OF_PALSY_VALID_LOW_HEALTH,
	TOUCH_OF_PALSY_MSTR_VALID_LOW_HEALTH,
	TOUCH_OF_PALSY_EXCLUDED_ONCE_TARGET,
	TOUCH_OF_PALSY_MSTR_EXCLUDED_ONCE_TARGET,
	BLOOD_OFFERING_EXCLUDED_ONCE,
	BLOOD_OFFERING_MSTR_EXCLUDED_ONCE,
	OVERPOWER_EXCLUDED_ONCE_TARGET,
	OVERPOWER_MSTR_EXCLUDED_ONCE_TARGET,
	PUNCTURE_EXCLUDED_ONCE_TARGET,
	PUNCTURE_MSTR_EXCLUDED_ONCE_TARGET,
	STAGGERING_BLOW_EXCLUDED_ONCE_TARGET,
	STAGGERING_BLOW_MSTR_EXCLUDED_ONCE_TARGET,
	JAW_STRIKE_EXCLUDED_ONCE_TARGET,
	JAW_STRIKE_MSTR_EXCLUDED_ONCE_TARGET,
	JAW_STRIKE_EXCLUDED_NO_SPELLS,
	JAW_STRIKE_MSTR_EXCLUDED_NO_SPELLS,
	HAMSTRING_EXCLUDED_ONCE_TARGET,
	HAMSTRING_MSTR_EXCLUDED_ONCE_TARGET,
	EXECUTION_MSTR_EXCLUDED_ONCE_TARGET,
	EXECUTION_EXCLUDED_ONCE_TARGET,
	BALANCE_EXCLUDED_ONCE,
	BALANCE_MSTR_EXCLUDED_ONCE,
	IGNORE_PAIN_EXCLUDED_ONCE,
	IGNORE_PAIN_MSTR_EXCLUDED_ONCE,
	FRENZY_EXCLUDED_ONCE,
	FRENZY_MSTR_EXCLUDED_ONCE,
	COURAGE_EXCLUDED_ONCE,
	COURAGE_MSTR_EXCLUDED_ONCE,
	ORDER_EXCLUDED_ONCE,
	ORDER_MSTR_EXCLUDED_ONCE,
	WAR_CRY_EXCLUDED_ONCE,
	WAR_CRY_MSTR_EXCLUDED_ONCE,
	RETREAT_EXCLUDED_ONCE,
	RETREAT_MSTR_EXCLUDED_ONCE,
	COORDINATION_EXCLUDED_ONCE,
	COORDINATION_MSTR_EXCLUDED_ONCE,
	RALLYING_CRY_EXCLUDED_ONCE,
	RALLYING_CRY_MSTR_EXCLUDED_ONCE,
	TAUNT_EXCLUDED_ONCE_TARGET,
	TAUNT_MSTR_EXCLUDED_ONCE_TARGET,
	THREATEN_EXCLUDED_ONCE_TARGET,
	THREATEN_MSTR_EXCLUDED_ONCE_TARGET,
	HOLD_GROUND_EXCLUDED_ONCE,
	HOLD_GROUND_VALID_EXCLUDED_ONCE,
	INTIMIDATE_EXCLUDED_ONCE_TARGET,
	INTIMIDATE_MSTR_EXCLUDED_ONCE_TARGET,
	INSULT_EXCLUDED_ONCE_TARGET,
	INSULT_MSTR_EXCLUDED_ONCE_TARGET,
	BATTLE_PLAN_EXCLUDED_ONCE,
	BATTLE_PLAN_MSTR_EXCLUDED_ONCE,
	EXPLOIT_POSITIONING_EXCLUDED_ONCE_TARGET,
	EXPLOIT_POSITIONING_MSTR_EXCLUDED_ONCE_TARGET,
	COMBAT_SAVVY_EXCLUDED_ONCE_TARGET,
	COMBAT_SAVVY_MSTR_EXCLUDED_ONCE_TARGET,
	STUDY_EXCLUDED_ONCE,
	STUDY_MSTR_EXCLUDED_ONCE,
	STIMULUS_EXCLUDED_ONCE = 487,
	STIMULUS_MSTR_EXCLUDED_ONCE,
	INTENSITY_EXCLUDED_IN_SIGHT,
	INTENSITY_MSTR_EXCLUDED_IN_SIGHT,
	ADRENALINE_RUSH_EXCLUDED_MAX_OP,
	ADRENALINE_RUSH_MSTR_EXCLUDED_MAX_OP,
	EXHAUSTION_EXCLUDED_MAX_SP,
	EXHAUSTION_MSTR_EXCLUDED_MAX_SP,
	SCOUTS_ADVICE_EXCLUDED_ONCE,
	SCOUTS_ADVICE_MSTR_EXCLUDED_ONCE,
	FOR_SIGMAR_EXCLUDED_ONCE_TARGET = 498,
	BLACK_HUNGER_EXCLUDED_NO_ENEMY,
	BLACK_HUNGER_EXCLUDED_ONCE,
	BLACK_HUNGER_MSTR_EXCLUDED_ONCE,
	BLACK_HUNGER_MSTR_EXCLUDED_NO_ENEMY,
	TACTICIAN_EXCLUDED_ONCE,
	TACTICIAN_MSTR_EXCLUDED_ONCE,
	SIGN_OF_SIGMAR_EXCLUDED_ONCE,
	SIGN_OF_SIGMAR_MSTR_EXCLUDED_ONCE,
	ATTRACTING_LURE_EXCLUDED_ONCE_TARGET,
	ATTRACTING_LURE_MSTR_EXCLUDED_ONCE_TARGET,
	PRAYER_OF_SWIFTNESS_EXCLUDED_PARRY,
	PRAYER_OF_SWIFTNESS_MSTR_EXCLUDED_PARRY,
	PRAYER_OF_SWIFTNESS_EXCLUDED_ONCE,
	PRAYER_OF_SWIFTNESS_MSTR_EXCLUDED_ONCE,
	DREAD_OF_ARAMAR_EXCLUDED_ONCE,
	DREAD_OF_ARAMAR_MSTR_EXCLUDED_ONCE,
	ARMOR_OF_LEAD_EXCLUDED_ONCE,
	ARMOR_OF_LEAD_MSTR_EXCLUDED_ONCE,
	BLINDING_LIGHT_EXCLUDED_ONCE,
	BLINDING_LIGHT_MSTR_EXCLUDED_ONCE,
	CURSE_OF_RUST_EXCLUDED_ONCE_TARGET,
	CURSE_OF_RUST_MSTR_EXCLUDED_ONCE_TARGET,
	ENSHROUDING_MIST_EXCLUDED_ONCE,
	ENSHROUDING_MIST_MSTR_EXCLUDED_ONCE,
	MUSK_OF_COURAGE_EXCLUDED_ONCE,
	MUSK_OF_COURAGE_MSTR_EXCLUDED_ONCE,
	WITHER_EXCLUDED_ONCE_TARGET,
	WITHER_MSTR_EXCLUDED_ONCE_TARGET,
	SORCERERS_CURSE_EXCLUDED_ONCE_TARGET,
	SORCERERS_CURSE_MSTR_EXCLUDED_ONCE_TARGET,
	GAZE_HORNED_RAT_EXCLUDED_ONCE,
	GAZE_HORNED_RAT_MSTR_EXCLUDED_ONCE,
	BLESS_WITH_FILTH_EXCLUDED_ONCE_TARGET,
	BLESS_WITH_FILTH_MSTR_EXCLUDED_ONCE_TARGET,
	POISONOUS_MIST_EXCLUDED_ONCE,
	POISONOUS_MIST_MSTR_EXCLUDED_ONCE,
	BLESSING_OF_SPEED_EXCLUDED_ONCE,
	BLESSING_OF_SPEED_MSTR_EXCLUDED_ONCE,
	HEARTS_OF_GRIFFON_EXCLUDED_ONCE,
	HEARTS_OF_GRIFFON_MSTR_EXCLUDED_ONCE,
	HAMMER_OF_SIGMAR_EXCLUDED_ONCE,
	HAMMER_OF_SIGMAR_MSTR_EXCLUDED_ONCE,
	SINFUL_SPEECH_EXCLUDED_NO_SPELL,
	SINFUL_SPEECH_MSTR_EXCLUDED_NO_SPELL,
	SINFUL_SPEECH_EXCLUDED_ONCE_TARGET,
	SINFUL_SPEECH_MSTR_EXCLUDED_ONCE_TARGET,
	SIGMARS_SECOND_WIND_EXCLUDED_ONCE,
	SIGMARS_SECOND_WIND_MSTR_EXCLUDED_ONCE,
	DIVINE_REVELATION_EXCLUDED_NO_ENEMY,
	DIVINE_REVELATION_MSTR_EXCLUDED_NO_ENEMY,
	DIVINE_REVELATION_EXCLUDED_ONCE_TARGET,
	DIVINE_REVELATION_MSTR_EXCLUDED_ONCE_TARGET,
	VEIL_OF_CORRUPTION_EXCLUDED_ONCE,
	VEIL_OF_CORRUPTION_MSTR_EXCLUDED_ONCE,
	BLADE_OF_DESTRUCTION_EXCLUDED_ONCE_TARGET,
	BLADE_OF_DESTRUCTION_MSTR_EXCLUDED_ONCE_TARGET,
	CURSE_OF_CHAOS_EXCLUDED_ONCE,
	CURSE_OF_CHAOS_MSTR_EXCLUDED_ONCE,
	ACID_BREATH_EXCLUDED_ONCE,
	ACID_BREATH_MSTR_EXCLUDED_ONCE,
	VISION_OF_TORMENT_EXCLUDED_ONCE,
	VISION_OF_TORMENT_MSTR_EXCLUDED_ONCE,
	BOON_OF_CHAOS_EXCLUDED_ONCE_TARGET,
	BOON_OF_CHAOS_MSTR_EXCLUDED_ONCE_TARGET,
	CHAINS_OF_CHAOS_EXCLUDED_ONCE_TARGET,
	CHAINS_OF_CHAOS_MSTR_EXCLUDED_ONCE_TARGET,
	MARTIAL_INFLUENCE_EXCLUDED_ONCE,
	FIGHT_ME_EXCLUDED_ONCE,
	STIFLING_BLOW_EXCLUDED_ONCE_TARGET,
	HYPNOTIC_ALLURE_EXCLUDED_ONCE,
	SCURRY_EXCLUDED_ONCE,
	SHREWD_DISTRACTION_EXCLUDED_ONCE_TARGET,
	CORROSIVE_ACID_EXCLUDED_ONCE,
	MASS_SUPPLICATION_EXCLUDED_ONCE,
	PENITENCE_EXCLUDED_ONCE_TARGET,
	DARK_GIFT_EXCLUDED_ONCE,
	FORSAKE_EXCLUDED_ONCE_TARGET,
	FORSAKE_EXCLUDED_NO_SPELLS,
	ANTIDOTE_NORMAL_EXCLUDED_NO_POISON,
	ANTIDOTE_GOOD_EXCLUDED_NO_POISON,
	ANTIDOTE_BEST_EXCLUDED_NO_POISON,
	AETHYRIC_ELIXIR_NORMAL_EXCLUDED_NO_MAGIC,
	AETHYRIC_ELIXIR_GOOD_EXCLUDED_NO_MAGIC,
	AETHYRIC_ELIXIR_BEST_EXCLUDED_NO_MAGIC,
	DRAUGHT_FOCUS_NORMAL_EXCLUDED_NO_STUPIDITY,
	DRAUGHT_FOCUS_GOOD_EXCLUDED_NO_STUPIDITY,
	DRAUGHT_FOCUS_BEST_EXCLUDED_NO_STUPIDITY,
	DRAUGHT_LUCIDITY_NORMAL_EXCLUDED_NO_HALF_CRAZY,
	DRAUGHT_LUCIDITY_GOOD_EXCLUDED_NO_HALF_CRAZY,
	DRAUGHT_LUCIDITY_BEST_EXCLUDED_NO_HALF_CRAZY,
	DRAUGHT_CALM_NORMAL_EXCLUDED_NO_PARANOIA,
	DRAUGHT_CALM_GOOD_EXCLUDED_NO_PARANOIA,
	DRAUGHT_CALM_BEST_EXCLUDED_NO_PARANOIA,
	DRAUGHT_CONCENTRATION_NORMAL_EXCLUDED_NO_SPELLS,
	DRAUGHT_CONCENTRATION_GOOD_EXCLUDED_NO_SPELLS,
	DRAUGHT_CONCENTRATION_BEST_EXCLUDED_NO_SPELLS,
	SMELLING_SALTS_NORMAL_EXCLUDED_NOT_STUNNED,
	SMELLING_SALTS_GOOD_EXCLUDED_NOT_STUNNED,
	SMELLING_SALTS_BEST_EXCLUDED_NOT_STUNNED,
	FRENZY_EXCLUDED_END_TURN,
	FRENZY_MSTR_EXCLUDED_END_TURN,
	WAR_CRY_EXCLUDED_END_TURN,
	WAR_CRY_MSTR_EXCLUDED_END_TURN,
	INSULT_EXCLUDED_END_TURN,
	INSULT_MSTR_EXCLUDED_END_TURN,
	EXPLOIT_POSITIONING_EXCLUDED_END_TURN,
	EXPLOIT_POSITIONING_MSTR_EXCLUDED_END_TURN,
	CARELESS_STRIKE_EXCLUDED_END_TURN,
	CARELESS_STRIKE_MSTR_EXCLUDED_END_TURN,
	CONCENTRATION_EXCLUDED_END_TURN,
	BLACK_HUNGER_EXCLUDED_END_TURN,
	BLACK_HUNGER_MSTR_EXCLUDED_END_TURN,
	SLEEPING_POISON_EXCLUDED_END_TURN,
	SLEEPING_POISON_MSTR_EXCLUDED_END_TURN,
	NUMBING_POISON_EXCLUDED_END_TURN,
	NUMBING_POISON_MSTR_EXCLUDED_END_TURN,
	MAD_CAP_MUSHROOM_NORMAL_EXCLUDED_END_TURN,
	MAD_CAP_MUSHROOM_GOOD_EXCLUDED_END_TURN,
	MAD_CAP_MUSHROOM_BEST_EXCLUDED_END_TURN,
	DRAUGHT_CONCENTRATION_NORMAL_EXCLUDED_END_TURN,
	DRAUGHT_CONCENTRATION_GOOD_EXCLUDED_END_TURN,
	DRAUGHT_CONCENTRATION_BEST_EXCLUDED_END_TURN,
	MARTIAL_INFLUENCE_EXCLUDED_END_TURN,
	SHREWD_DISTRACTION_EXCLUDED_END_TURN,
	CORROSIVE_ACID_EXCLUDED_END_TURN,
	MASS_SUPPLICATION_EXCLUDED_END_TURN,
	PENITENCE_EXCLUDED_END_TURN,
	DARK_GIFT_EXCLUDED_END_TURN,
	TAUNT_EXCLUDED_HEROIC_IDIOCY,
	TAUNT_MSTR_EXCLUDED_HEROIC_IDIOCY,
	RETREAT_EXCLUDED_HEROIC_IDIOCY,
	RETREAT_MSTR_EXCLUDED_HEROIC_IDIOCY,
	INTIMIDATE_EXCLUDED_HEROIC_IDIOCY,
	INTIMIDATE_MSTR_EXCLUDED_HEROIC_IDIOCY,
	MANTICORE_TAIL_WHIP_EXCLUDED_ENGAGED,
	MANTICORE_TAIL_WHIP_EXCLUDED_ONCE,
	MANTICORE_ACID_SPIT_EXCLUDED_ONCE_TARGET = 636,
	TALISMAN_NORMAL_EXCLUDED_ONCE_TARGET,
	TALISMAN_GOOD_EXCLUDED_ONCE_TARGET,
	TALISMAN_BEST_EXCLUDED_ONCE_TARGET,
	IMPACT_SHOT_EXCLUDED_ARMOR,
	IMPACT_SHOT_MSTR_EXCLUDED_ARMOR,
	IMPACT_SHOT_VALID_ROLL,
	IMPACT_SHOT_MSTR_VALID_ROLL,
	RIPOSTE_STANCE_EXCLUDED_PARRY_ROLL,
	RIPOSTE_STANCE_MSTR_EXCLUDED_PARRY_ROLL,
	RIPOSTE_STANCE_VALID_ENGAGED,
	RIPOSTE_STANCE_MSTR_VALID_ENGAGED,
	INFUSED_GLOBE_EXCLUDED_WOUNDS,
	INFUSED_GLOBE_MSTR_EXCLUDED_WOUNDS,
	STRANGLING_GLOBE_EXCLUDED_ONCE,
	STRANGLING_GLOBE_MSTR_EXCLUDED_ONCE,
	POISON_GLOBE_EXCLUDED_ONCE,
	VENT_EXCLUDED_ONCE,
	VENT_MSTR_EXCLUDED_ONCE,
	WARP_FUMES_EXCLUDED_ONCE,
	WARP_FUMES_MSTR_EXCLUDED_ONCE,
	WARP_FUMES_EXCLUDED_RANGED,
	WARP_FUMES_MSTR_EXCLUDED_RANGED,
	LIQUID_COURAGE_EXCLUDED_ONCE,
	LIQUID_COURAGE_MSTR_EXCLUDED_ONCE,
	CAPTAINS_SPEECH_EXCLUDED_ONCE,
	CAPTAINS_SPEECH_MSTR_EXCLUDED_ONCE,
	CAPTAINS_SPEECH_EXCLUDED_NO_ENEMY,
	CAPTAINS_SPEECH_MSTR_EXCLUDED_NO_ENEMY,
	CAPTAINS_ORDER_EXCLUDED_ONCE_TARGET,
	CAPTAINS_ORDER_MSTR_EXCLUDED_ONCE_TARGET,
	CAPTAINS_ORDER_EXCLUDED_NO_ENEMY,
	CAPTAINS_ORDER_MSTR_EXCLUDED_NO_ENEMY,
	BURNING_BLOOD_EXCLUDED_WOUNDS,
	BURNING_BLOOD_MSTR_EXCLUDED_WOUNDS,
	BURNING_BLOOD_VALID_MIN_ROLL,
	BURNING_BLOOD_MSTR_VALID_MIN_ROLL,
	SPITEFUL_MANIFESTATION_EXCLUDED_ONCE,
	SPITEFUL_MANIFESTATION_MSTR_EXCLUDED_ONCE,
	SPITEFUL_MANIFESTATION_EXCLUDED_ARMOR,
	SPITEFUL_MANIFESTATION_MSTR_EXCLUDED_ARMOR,
	SPITEFUL_MANIFESTATION_VALID_MIN_ROLL,
	SPITEFUL_MANIFESTATION_MSTR_VALID_MIN_ROLL,
	BOON_OF_RUIN_EXCLUDED_ONCE_TARGET,
	BOON_OF_RUIN_MSTR_EXCLUDED_ONCE_TARGET,
	BOON_OF_RUIN_EXCLUDED_NO_ENEMY,
	BOON_OF_RUIN_MSTR_EXCLUDED_NO_ENEMY,
	BOON_OF_RUIN_EXCLUDED_RANGED,
	BOON_OF_RUIN_MSTR_EXCLUDED_RANGED,
	BOON_OF_RUIN_VALID_ENGAGED,
	BOON_OF_RUIN_MSTR_VALID_ENGAGED,
	BLESSING_OF_ULRIC_EXCLUDED_ONCE_TARGET,
	BLESSING_OF_ULRIC_MSTR_EXCLUDED_ONCE_TARGET,
	BLESSING_OF_ULRIC_EXCLUDED_NO_ENEMY,
	BLESSING_OF_ULRIC_MSTR_EXCLUDED_NO_ENEMY,
	BLESSING_OF_ULRIC_VALID_MIN_ROLL,
	BLESSING_OF_ULRIC_MSTR_VALID_MIN_ROLL,
	FROSTS_BITE_EXCLUDED_ONCE_TARGET,
	FROSTS_BITE_MSTR_EXCLUDED_ONCE_TARGET,
	FROSTS_BITE_VALID_MIN_ROLL,
	FROSTS_BITE_MSTR_VALID_MIN_ROLL,
	ICE_STORM_EXCLUDED_ONCE,
	ICE_STORM_MSTR_EXCLUDED_ONCE,
	ICE_STORM_VALID_MIN_ROLL,
	ICE_STORM_MSTR_VALID_MIN_ROLL,
	SNOW_KINGS_DECREE_EXCLUDED_ONCE_TARGET,
	SNOW_KINGS_DECREE_MSTR_EXCLUDED_ONCE_TARGET,
	SNOW_KINGS_DECREE_VALID_NO_SPELLS,
	SNOW_KINGS_DECREE_MSTR_VALID_NO_SPELLS,
	WINTERS_CHILL_EXCLUDED_ONCE_TARGET,
	WINTERS_CHILL_MSTR_EXCLUDED_ONCE_TARGET,
	WINTERS_CHILL_EXCLUDED_NO_ENEMY,
	WINTERS_CHILL_MSTR_EXCLUDED_NO_ENEMY,
	WINTERS_CHILL_VALID_ENGAGED,
	WINTERS_CHILL_MSTR_VALID_ENGAGED,
	HOWL_OF_THE_WOLF_EXCLUDED_ONCE,
	HOWL_OF_THE_WOLF_MSTR_EXCLUDED_ONCE,
	HOWL_OF_THE_WOLF_EXCLUDED_NO_ENEMY,
	HOWL_OF_THE_WOLF_MSTR_EXCLUDED_NO_ENEMY,
	HOWL_OF_THE_WOLF_VALID_MIN_ROLL,
	HOWL_OF_THE_WOLF_MSTR_VALID_MIN_ROLL,
	WILD_PACK_EXCLUDED_ONCE,
	WILD_PACK_MSTR_EXCLUDED_ONCE,
	WILD_PACK_EXCLUDED_NO_ENEMY,
	WILD_PACK_MSTR_EXCLUDED_NO_ENEMY,
	WILD_PACK_VALID_MIN_ROLL,
	WILD_PACK_MSTR_VALID_MIN_ROLL,
	ULRICS_GIFT_EXCLUDED_ONCE_TARGET,
	ULRICS_GIFT_MSTR_EXCLUDED_ONCE_TARGET,
	ULRICS_GIFT_EXCLUDED_RANGED,
	ULRICS_GIFT_MSTR_EXCLUDED_RANGED,
	ULRICS_GIFT_VALID_ENGAGED,
	ULRICS_GIFT_MSTR_VALID_ENGAGED,
	CRUSH_THE_WEAK_EXCLUDED_ONCE_TARGET,
	CRUSH_THE_WEAK_MSTR_EXCLUDED_ONCE_TARGET,
	CRUSH_THE_WEAK_EXCLUDED_CRIT_RES,
	CRUSH_THE_WEAK_MSTR_EXCLUDED_CRIT_RES,
	WARP_FUMES_EXCLUDED_END_TURN,
	WARP_FUMES_MSTR_EXCLUDED_END_TURN,
	ALPHA_HOWL_EXCLUDED_ONCE,
	ALPHA_HOWL_MSTR_EXCLUDED_ONCE,
	ALPHA_HOWL_EXCLUDED_NO_ENEMY,
	ALPHA_HOWL_MSTR_EXCLUDED_NO_ENEMY,
	RITUAL_OF_DEFIANCE_EXCLUDED_ONCE,
	RITUAL_OF_DEFIANCE_MSTR_EXCLUDED_ONCE,
	RITUAL_OF_DEFIANCE_EXCLUDED_NO_ENEMY,
	RITUAL_OF_DEFIANCE_MSTR_EXCLUDED_NO_ENEMY,
	RITUAL_OF_DEFIANCE_EXCLUDED_PRE_FIGHT,
	RITUAL_OF_DEFIANCE_MSTR_EXCLUDED_PRE_FIGHT,
	RITUAL_OF_SUFFERING_EXCLUDED_ONCE,
	RITUAL_OF_SUFFERING_MSTR_EXCLUDED_ONCE,
	RITUAL_OF_SUFFERING_EXCLUDED_NO_ENEMY,
	RITUAL_OF_SUFFERING_MSTR_EXCLUDED_NO_ENEMY,
	RITUAL_OF_SUFFERING_EXCLUDED_PRE_FIGHT,
	RITUAL_OF_SUFFERING_MSTR_EXCLUDED_PRE_FIGHT,
	RITUAL_OF_SCORN_EXCLUDED_ONCE,
	RITUAL_OF_SCORN_MSTR_EXCLUDED_ONCE,
	RITUAL_OF_SCORN_EXCLUDED_NO_ENEMY,
	RITUAL_OF_SCORN_MSTR_EXCLUDED_NO_ENEMY,
	RITUAL_OF_SCORN_EXCLUDED_PRE_FIGHT,
	RITUAL_OF_SCORN_MSTR_EXCLUDED_PRE_FIGHT,
	INTROSPECTION_EXCLUDED_PRE_FIGHT,
	INTROSPECTION_MSTR_EXCLUDED_PRE_FIGHT,
	TOUCH_OF_DECAY_EXCLUDED_ONCE,
	TOUCH_OF_RANCOR_EXCLUDED_ONCE_TARGET,
	TOUCH_OF_DESIRE_EXCLUDED_ENGAGED,
	TOUCH_OF_DESIRE_EXCLUDED_ONCE,
	TOUCH_OF_RANCOR_VALID_ENGAGED,
	WOE_OF_THE_WICKED_EXCLUDED_ONCE,
	GUILTY_VERDICT_VALID_WOUNDS,
	EXPOSE_SIN_VALID_ENGAGED,
	SHIELD_OF_FAITH_EXCLUDED_ONCE,
	SHIELD_OF_FAITH_MSTR_EXCLUDED_ONCE,
	SHIELD_OF_FAITH_VALID_MIN_ROLL,
	SHIELD_OF_FAITH_MSTR_VALID_MIN_ROLL,
	ARMOUR_OF_RIGHTEOUSNESS_EXCLUDED_ONCE_TARGET,
	ARMOUR_OF_RIGHTEOUSNESS_MSTR_EXCLUDED_ONCE_TARGET,
	ARMOUR_OF_RIGHTEOUSNESS_VALID_ENGAGED,
	ARMOUR_OF_RIGHTEOUSNESS_MSTR_VALID_ENGAGED,
	DENY_THE_HERETIC_EXCLUDED_ONCE,
	DENY_THE_HERETIC_MSTR_EXCLUDED_ONCE,
	DENY_THE_HERETIC_VALID_ENGAGED,
	DENY_THE_HERETIC_MSTR_VALID_ENGAGED,
	IMMACULATE_FLESH_VALID_MIN_ROLL,
	IMMACULATE_FLESH_MSTR_VALID_MIN_ROLL,
	IMMACULATE_FLESH_MSTR_EXCLUDED_WYRD,
	WORD_OF_DAMNATION_EXCLUDED_ONCE_TARGET,
	WORD_OF_DAMNATION_MSTR_EXCLUDED_ONCE_TARGET,
	WORD_OF_DAMNATION_VALID_ENGAGED,
	WORD_OF_DAMNATION_MSTR_VALID_ENGAGED,
	SOULFIRE_EXCLUDED_ONCE,
	SOULFIRE_MSTR_EXCLUDED_ONCE,
	SOULFIRE_VALID_MIN_ROLL,
	SOULFIRE_MSTR_VALID_MIN_ROLL,
	SOULFIRE_EXCLUDED_NOT_ENGAGED = 791,
	SOULFIRE_MSTR_EXCLUDED_NOT_ENGAGED,
	PRAYER_OF_ABSOLUTION_EXCLUDED_ONCE,
	PRAYER_OF_ABSOLUTION_MSTR_EXCLUDED_ONCE,
	PRAYER_OF_ABSOLUTION_EXCLUDED_NOT_ENGAGED,
	PRAYER_OF_ABSOLUTION_MSTR_EXCLUDED_NOT_ENGAGED,
	PRAYER_OF_ABSOLUTION_VALID_ENGAGED,
	PRAYER_OF_ABSOLUTION_MSTR_VALID_ENGAGED,
	HEALING_HAND_EXCLUDED_WOUNDS,
	HEALING_HAND_MSTR_EXCLUDED_WOUNDS,
	HEALING_HAND_VALID_ENGAGED,
	HEALING_HAND_MSTR_VALID_ENGAGED,
	BURN_THE_WITCH_MSTR_EXCLUDED_NO_SPELLS,
	BURN_THE_WITCH_EXCLUDED_NO_SPELLS,
	BURN_THE_WITCH_VALID_SPELLCASTER,
	BURN_THE_WITCH_MSTR_VALID_SPELLCASTER,
	TRIAL_BY_PAIN_EXCLUDED_ONCE_TARGET,
	TRIAL_BY_PAIN_MSTR_EXCLUDED_ONCE_TARGET,
	FANATICAL_ZEAL_EXCLUDED_ONCE,
	FANATICAL_ZEAL_MSTR_EXCLUDED_ONCE,
	TRIAL_BY_PAIN_VALID_MIN_ROLL,
	TRIAL_BY_PAIN_MSTR_VALID_MIN_ROLL,
	FANATICAL_ZEAL_EXCLUDED_NOT_ENGAGED,
	FANATICAL_ZEAL_MSTR_EXCLUDED_NOT_ENGAGED,
	FANATICAL_ZEAL_EXCLUDED_RANGED,
	FANATICAL_ZEAL_MSTR_EXCLUDED_RANGED,
	FUMES_OF_CONFUSION_EXCLUDED_ONCE,
	FUMES_OF_CONFUSION_EXCLUDED_NO_ENEMY,
	CHROMATIC_PROT_EXCLUDED_ONCE,
	IDOL_OF_BLOOD_EXCLUDED_NO_ENEMY,
	IDOL_OF_BLOOD_MSTR_EXCLUDED_NO_ENEMY,
	IDOL_OF_CHANGE_EXCLUDED_NO_ENEMY,
	IDOL_OF_CHANGE_MSTR_EXCLUDED_NO_ENEMY,
	IDOL_OF_LUST_EXCLUDED_NO_ENEMY,
	IDOL_OF_LUST_MSTR_EXCLUDED_NO_ENEMY,
	IDOL_OF_PESTILENCE_EXCLUDED_NO_ENEMY,
	IDOL_OF_PESTILENCE_MSTR_EXCLUDED_NO_ENEMY,
	IDOL_OF_BLOOD_VALID_ENGAGED,
	IDOL_OF_BLOOD_MSTR_VALID_ENGAGED,
	IDOL_OF_LUST_VALID_ENGAGED,
	IDOL_OF_LUST_MSTR_VALID_ENGAGED,
	PYRE_OF_THE_RIGHTEOUS_EXCLUDED_NO_ENEMY,
	PYRE_OF_THE_RIGHTEOUS_EXCLUDED_ONCE,
	ROTTEN_TOUCH_EXCLUDED_ONCE_TARGET,
	ROTTEN_TOUCH_MSTR_EXCLUDED_ONCE_TARGET,
	LIFESTEALER_VALID_WOUNDS,
	LIFESTEALER_MSTR_VALID_WOUNDS,
	CORPSE_FLESH_VALID_ENGAGED,
	CORPSE_FLESH_MSTR_VALID_ENGAGED,
	CORPSE_FLESH_EXCLUDED_NO_ENEMY,
	CORPSE_FLESH_MSTR_EXCLUDED_NO_ENEMY,
	IDOL_OF_DEATH_EXCLUDED_NO_ENEMY,
	IDOL_OF_DEATH_MSTR_EXCLUDED_NO_ENEMY,
	IDOL_OF_DEATH_EXCLUDED_ONCE,
	IDOL_OF_DEATH_MSTR_EXCLUDED_ONCE,
	CALL_OF_VANHEL_EXCLUDED_NO_ENEMY,
	CALL_OF_VANHEL_MSTR_EXCLUDED_NO_ENEMY,
	CALL_OF_VANHEL_EXCLUDED_ONCE_TARGET,
	CALL_OF_VANHEL_MSTR_EXCLUDED_ONCE_TARGET,
	HEART_FAILURE_VALID_ENGAGED,
	HEART_FAILURE_MSTR_VALID_ENGAGED,
	HEART_FAILURE_EXCLUDED_ONCE,
	HEART_FAILURE_MSTR_EXCLUDED_ONCE,
	HUMBLE_SERVANT_VALID_HEALTH,
	HUMBLE_SERVANT_EXCLUDED_HEALTH,
	VENT_EXCLUDED_ENGAGED,
	VENT_MSTR_EXCLUDED_ENGAGED,
	DEATH_STENCH_EXCLUDED_ONCE,
	DEATH_STENCH_MSTR_EXCLUDED_ONCE = 860,
	DEATH_STENCH_EXCLUDED_ENGAGED,
	DEATH_STENCH_MSTR_EXCLUDED_ENGAGED,
	IMMACULATE_FLESH_EXLUDED_POISON,
	IMMACULATE_FLESH_MSTR_EXLUDED_POISON,
	MAX_VALUE
}
