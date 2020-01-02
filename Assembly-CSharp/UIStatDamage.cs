using System;

public class UIStatDamage : global::UIStat
{
	protected override string GenerateStatsText(global::Unit unit, int stat, int? statMax)
	{
		bool flag = false;
		global::AttributeModList weaponDamageModifier = unit.GetWeaponDamageModifier(null, false, false);
		for (int i = 0; i < weaponDamageModifier.Count; i++)
		{
			if (weaponDamageModifier[i].IsTemp())
			{
				flag = true;
			}
		}
		string text = string.Format("{0}-{1}", unit.GetWeaponDamageMin(null, false, false, false), unit.GetWeaponDamageMax(null, false, false, false));
		return (!flag) ? text : global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("menu_stat_increase_value", new string[]
		{
			text
		});
	}
}
