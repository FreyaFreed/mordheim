using System;
using System.Globalization;

public static class EnumExtensions
{
	public static string ToLowerString(this object value)
	{
		return value.ToString().ToLowerInvariant();
	}

	public static string ToLowerString(this global::SkillId skill)
	{
		return skill.ToSkillIdString();
	}

	public static string ToLowerString(this global::AttributeId attribute)
	{
		return attribute.ToAttributeIdString();
	}

	public static string ToIntString<TEnum>(this TEnum enumValue) where TEnum : struct, global::System.IComparable, global::System.IFormattable, global::System.IConvertible
	{
		return global::Constant.ToString(enumValue.ToInt32(global::System.Globalization.CultureInfo.InvariantCulture));
	}

	public static string ToSkillIdString(this global::SkillId skill)
	{
		if (global::EnumExtensions.skillIdValues == null)
		{
			global::EnumExtensions.skillIdValues = new string[934];
			for (global::SkillId skillId = global::SkillId.NONE; skillId < global::SkillId.MAX_VALUE; skillId++)
			{
				global::EnumExtensions.skillIdValues[(int)skillId] = skillId.ToString().ToLowerInvariant();
			}
		}
		return global::EnumExtensions.skillIdValues[(int)skill];
	}

	public static string ToAttributeIdString(this global::AttributeId attribute)
	{
		if (global::EnumExtensions.attributeIdValues == null)
		{
			global::EnumExtensions.attributeIdValues = new string[152];
			for (global::AttributeId attributeId = global::AttributeId.NONE; attributeId < global::AttributeId.MAX_VALUE; attributeId++)
			{
				global::EnumExtensions.attributeIdValues[(int)attributeId] = attributeId.ToString().ToLowerInvariant();
			}
		}
		return global::EnumExtensions.attributeIdValues[(int)attribute];
	}

	private static string[] skillIdValues;

	private static string[] attributeIdValues;
}
