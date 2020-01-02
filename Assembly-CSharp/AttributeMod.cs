using System;
using System.Text;
using UnityEngine;

public struct AttributeMod
{
	public AttributeMod(global::AttributeMod.Type type, string reason, int modifier, string effect = null, bool isPercent = false, bool isNegative = false)
	{
		this.reason = reason;
		this.effect = effect;
		this.modifier = ((!isNegative) ? modifier : (-modifier));
		this.isPercent = isPercent;
		this.maxModifier = null;
		this.isEnemy = false;
		this.showValue = true;
		this.type = type;
		this.count = 1;
		this.generatedText = null;
	}

	public AttributeMod(global::AttributeMod.Type type, string reason, int minModifier, int maxModifier)
	{
		this.reason = reason;
		this.effect = null;
		this.modifier = minModifier;
		this.maxModifier = new int?(maxModifier);
		this.isPercent = false;
		this.isEnemy = false;
		this.showValue = true;
		this.type = type;
		this.count = 1;
		this.generatedText = null;
	}

	public AttributeMod(string reason)
	{
		this = new global::AttributeMod(global::AttributeMod.Type.NONE, reason, 0, null, false, false);
		this.showValue = false;
	}

	public string GetText(bool forcePercent)
	{
		if (!this.showValue)
		{
			return this.reason;
		}
		if (!string.IsNullOrEmpty(this.generatedText))
		{
			return this.generatedText;
		}
		global::System.Text.StringBuilder stringBuilder = global::PandoraUtils.StringBuilder;
		if (this.isEnemy)
		{
			stringBuilder.Append("<color=#d75b5dff>");
		}
		if (this.maxModifier == null && this.reason != "attribute_base_roll")
		{
			if (this.modifier > 0)
			{
				stringBuilder.Append('+');
			}
			else if (this.modifier < 0)
			{
				stringBuilder.Append('-');
			}
		}
		stringBuilder.Append(global::UnityEngine.Mathf.Abs(this.modifier).ToConstantString());
		if (this.maxModifier != null)
		{
			stringBuilder.Append('-').Append(this.maxModifier.Value.ToConstantString());
		}
		if (forcePercent || this.isPercent || this.reason.EndsWith("perc", global::System.StringComparison.OrdinalIgnoreCase))
		{
			stringBuilder.Append('%');
		}
		if (this.effect != null)
		{
			stringBuilder.Append(' ').Append(this.effect);
			stringBuilder.Append(" (").Append(this.reason).Append(')');
		}
		else
		{
			stringBuilder.Append(' ').Append(this.reason);
		}
		if (this.count > 1)
		{
			stringBuilder.AppendFormat(" ({0})", this.count.ToConstantString());
		}
		if (this.isEnemy)
		{
			stringBuilder.Append("</color>");
		}
		this.generatedText = stringBuilder.ToString();
		return this.generatedText;
	}

	public global::AttributeMod Negate()
	{
		this.modifier = -this.modifier;
		return this;
	}

	public global::AttributeMod SetEffect(string effect)
	{
		this.effect = effect;
		return this;
	}

	public global::AttributeMod SetIsPercent(bool value)
	{
		this.isPercent = value;
		return this;
	}

	public global::AttributeMod SetEnemyMod(bool value)
	{
		this.isEnemy = value;
		return this;
	}

	public bool IsTemp()
	{
		return this.reason == "temp";
	}

	public bool IsSame(global::AttributeMod mod)
	{
		return this.effect == mod.effect && this.isPercent == mod.isPercent && this.isEnemy == mod.isEnemy && this.reason == mod.reason && this.type == mod.type;
	}

	public const string TEMP_ATTRIBUTE_MODIFIER = "temp";

	private const string COUNT_TXT = " ({0})";

	private string reason;

	private string effect;

	public int modifier;

	private int? maxModifier;

	private bool isPercent;

	private bool showValue;

	public bool isEnemy;

	public global::AttributeMod.Type type;

	public int count;

	private string generatedText;

	public enum Type
	{
		NONE,
		BASE,
		ATTRIBUTE,
		SKILL,
		ITEM,
		ENCHANTMENT,
		BUFF,
		DEBUFF,
		TEMP,
		INJURY,
		MUTATION
	}
}
