using System;
using UnityEngine.UI;

public class SpellDescModule : global::SkillDescModule
{
	public override void Set(global::SkillData skillData, string reason = null)
	{
		base.Set(skillData, reason);
		global::Unit unit = (!global::PandoraSingleton<global::HideoutManager>.Exists()) ? null : global::PandoraSingleton<global::HideoutManager>.Instance.currentUnit.unit;
		bool flag = unit == null || unit.HasSkillOrSpell(skillData.Id);
		if (global::SkillHelper.IsMastery(skillData) && !flag)
		{
			global::SkillData skill = global::SkillHelper.GetSkill(skillData.SkillIdPrerequiste);
			this.skillType.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("skill_" + skill.SpellTypeId.ToLowerString());
			this.castingMod.text = global::SkillHelper.GetLocalizedCasting(skill);
			this.curseTitle.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById((skill.SpellTypeId != global::SpellTypeId.ARCANE) ? "skill_wrath_title" : "skill_curse_title");
			this.curseMod.text = global::SkillHelper.GetLocalizedCurse(skill);
		}
		else
		{
			this.skillType.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("skill_" + skillData.SpellTypeId.ToLowerString());
			this.castingMod.text = global::SkillHelper.GetLocalizedCasting(skillData);
			this.curseTitle.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById((skillData.SpellTypeId != global::SpellTypeId.ARCANE) ? "skill_wrath_title" : "skill_curse_title");
			this.curseMod.text = global::SkillHelper.GetLocalizedCurse(skillData);
		}
	}

	public global::UnityEngine.UI.Text castingMod;

	public global::UnityEngine.UI.Text curseMod;

	public global::UnityEngine.UI.Text curseTitle;
}
