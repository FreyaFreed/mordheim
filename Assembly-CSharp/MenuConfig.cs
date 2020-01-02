using System;
using UnityEngine;

public class MenuConfig
{
	public static global::UnityEngine.Color32 lightGray = new global::UnityEngine.Color32(168, 168, 168, byte.MaxValue);

	public static global::UnityEngine.Color32 darkGray = new global::UnityEngine.Color32(71, 71, 71, byte.MaxValue);

	public static global::UnityEngine.Color32 wyrdstoneGreen = new global::UnityEngine.Color32(74, 137, 96, byte.MaxValue);

	public static global::UnityEngine.Color32 goldenYellow = new global::UnityEngine.Color32(byte.MaxValue, 204, 94, byte.MaxValue);

	public static global::UnityEngine.Color32 disabledRed = new global::UnityEngine.Color32(byte.MaxValue, 48, 68, byte.MaxValue);

	public static global::UnityEngine.Color32 enchantRegular = new global::UnityEngine.Color32(19, 115, 240, byte.MaxValue);

	public static global::UnityEngine.Color32 enchantMaster = new global::UnityEngine.Color32(160, 84, 182, byte.MaxValue);

	public static global::UnityEngine.Color32 colorUnselected = new global::UnityEngine.Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 230);

	public static float alphaUnavailable = 0.2f;

	public static global::MenuConfig.OptionsGraphics[][] menuOptionsGfx = new global::MenuConfig.OptionsGraphics[][]
	{
		new global::MenuConfig.OptionsGraphics[]
		{
			global::MenuConfig.OptionsGraphics.GENERAL_QUALITY,
			global::MenuConfig.OptionsGraphics.RESOLUTION
		},
		new global::MenuConfig.OptionsGraphics[]
		{
			global::MenuConfig.OptionsGraphics.SHADOW_QUALITY,
			global::MenuConfig.OptionsGraphics.FULL_SCREEN
		},
		new global::MenuConfig.OptionsGraphics[]
		{
			global::MenuConfig.OptionsGraphics.TEXTURE_QUALITY,
			global::MenuConfig.OptionsGraphics.VERTICAL_SYNC
		},
		new global::MenuConfig.OptionsGraphics[]
		{
			global::MenuConfig.OptionsGraphics.ANISOTROPIC
		},
		new global::MenuConfig.OptionsGraphics[]
		{
			global::MenuConfig.OptionsGraphics.ANTI_ALIASING
		}
	};

	public enum ReasonType
	{
		AVAILABLE,
		NO_MONEY,
		NO_HERO_SLOT,
		NO_ABILITY,
		NO_SKILL_PTS,
		NO_LEADER,
		MAX_OUT,
		IN_TRAINING,
		INJURED,
		UPKEEP_NOT_PAID,
		PREREQUISITE,
		WRONG_ALLEGIANCE,
		EMPTY,
		EMPTY_LIST,
		CANNOT_BE_ENCHANTED,
		NOT_ENOUGH_UNITS,
		SOME_UPKEEP,
		CAMPAIGN
	}

	public enum OptionsGraphics
	{
		GENERAL_QUALITY,
		SHADOW_QUALITY,
		TEXTURE_QUALITY,
		ANISOTROPIC,
		ANTI_ALIASING,
		RESOLUTION,
		FULL_SCREEN,
		VERTICAL_SYNC,
		REVERT
	}
}
