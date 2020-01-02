using System;
using UnityEngine;
using UnityEngine.UI;

public class WarbandSkillDescModule : global::UIModule
{
	public virtual void Set(int idx, global::WarbandSkill skill)
	{
		base.gameObject.SetActive(true);
		this.icon.sprite = global::WarbandSkill.GetIcon(skill);
		this.title.text = global::WarbandSkill.LocName(skill);
		this.desc.text = global::WarbandSkill.LocDesc(skill);
		if (skill != null)
		{
			this.masteryIcon.enabled = skill.IsMastery;
			global::WarbandSkillData skillMastery = skill.GetSkillMastery();
			if (skillMastery != null)
			{
				this.masterySection.SetActive(true);
				this.masteryDesc.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("warband_skill_desc_" + skill.Id.ToLowerString() + "_hideout");
			}
			else
			{
				this.masterySection.SetActive(false);
			}
		}
		else
		{
			this.masteryIcon.enabled = false;
			this.masterySection.SetActive(false);
		}
	}

	public global::UnityEngine.UI.Image icon;

	public global::UnityEngine.UI.Image masteryIcon;

	public global::UnityEngine.UI.Text title;

	public global::UnityEngine.UI.Text desc;

	public global::UnityEngine.GameObject masterySection;

	public global::UnityEngine.UI.Text masteryDesc;
}
