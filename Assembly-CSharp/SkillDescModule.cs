using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillDescModule : global::UIModule
{
	public virtual void Set(global::SkillData skillData, string reason = null)
	{
		base.gameObject.SetActive(true);
		global::Unit unit = (!global::PandoraSingleton<global::HideoutManager>.Exists()) ? null : global::PandoraSingleton<global::HideoutManager>.Instance.currentUnit.unit;
		bool flag = unit == null || unit.HasSkillOrSpell(skillData.Id);
		this.skillName.text = global::SkillHelper.GetLocalizedName(skillData);
		this.icon.sprite = global::SkillHelper.GetIcon(skillData);
		global::SkillData skillData2 = null;
		if (!flag && global::SkillHelper.IsMastery(skillData))
		{
			skillData2 = global::SkillHelper.GetSkill(skillData.SkillIdPrerequiste);
		}
		this.duration.text = global::SkillHelper.GetLocalizedDuration(skillData2 ?? skillData);
		this.range.text = global::SkillHelper.GetLocalizedRange(skillData2 ?? skillData);
		if (this.ratingValue != null)
		{
			this.ratingValue.text = global::SkillHelper.GetRating(skillData2 ?? skillData).ToString();
		}
		this.effect.text = global::SkillHelper.GetLocalizedDescription((skillData2 ?? skillData).Id);
		if (this.learningTime != null)
		{
			if (flag)
			{
				this.learningTime.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("skill_training_learned");
			}
			else
			{
				this.learningTime.text = global::SkillHelper.GetLocalizedTrainingTime(skillData);
			}
		}
		if (this.requirement != null)
		{
			if (flag || (global::SkillHelper.IsMastery(skillData) && (unit.HasSkillOrSpell(skillData.SkillIdPrerequiste) || unit.UnitSave.skillInTrainingId == skillData.SkillIdPrerequiste)))
			{
				this.requirement.text = string.Empty;
			}
			else if (reason != null)
			{
				if (reason == "na_skill_attribute")
				{
					this.requirement.text = global::SkillHelper.GetLocalizedRequirement(skillData);
				}
				else
				{
					this.requirement.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(reason);
				}
			}
			else
			{
				this.requirement.text = string.Empty;
			}
			this.requirement.gameObject.SetActive(!string.IsNullOrEmpty(this.requirement.text));
		}
		if (this.splatter != null)
		{
			if (reason != null)
			{
				this.splatter.overrideSprite = this.splatterRed;
			}
			else
			{
				this.splatter.overrideSprite = null;
			}
		}
		for (int i = 0; i < this.offensePoints.Count; i++)
		{
			this.offensePoints[i].enabled = (i < skillData.OffensePoints);
		}
		for (int j = 0; j < this.strategyPoints.Count; j++)
		{
			this.strategyPoints[j].enabled = (j < skillData.StrategyPoints);
		}
		this.masteryIcon.enabled = global::SkillHelper.IsMastery(skillData);
		if (this.masteryGroup != null)
		{
			if (global::SkillHelper.IsMastery(skillData))
			{
				if (!flag)
				{
					this.masteryGroup.SetActive(true);
					this.masteryEffect.text = global::SkillHelper.GetLocalizedMasteryDescription(skillData.SkillIdPrerequiste);
					if (reason != null)
					{
						if (reason == "na_skill_attribute")
						{
							this.masteryRequirement.text = global::SkillHelper.GetLocalizedRequirement(skillData);
						}
						else
						{
							this.masteryRequirement.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(reason);
						}
					}
					else
					{
						this.masteryRequirement.text = string.Empty;
					}
				}
				else
				{
					this.masteryGroup.SetActive(false);
				}
			}
			else
			{
				global::SkillData skillMastery = global::SkillHelper.GetSkillMastery(skillData);
				if (skillMastery != null)
				{
					this.masteryGroup.SetActive(true);
					this.masteryEffect.text = global::SkillHelper.GetLocalizedMasteryDescription(skillData.Id);
					this.masteryRequirement.text = global::SkillHelper.GetLocalizedRequirement(skillMastery);
				}
				else
				{
					this.masteryGroup.SetActive(false);
				}
			}
			this.masteryRequirement.gameObject.SetActive(!string.IsNullOrEmpty(this.masteryRequirement.text));
		}
		if (this.activeStatGroup != null)
		{
			this.activeStatGroup.SetActive(!skillData.Passive);
		}
		if (this.skillType != null)
		{
			this.skillType.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById((!skillData.Passive) ? "skill_active" : "skill_passive");
		}
	}

	public global::UnityEngine.UI.Image icon;

	public global::UnityEngine.UI.Image splatter;

	public global::UnityEngine.Sprite splatterRed;

	public global::UnityEngine.UI.Image masteryIcon;

	public global::UnityEngine.UI.Text skillName;

	public global::UnityEngine.GameObject activeStatGroup;

	public global::UnityEngine.UI.Text range;

	public global::UnityEngine.UI.Text duration;

	public global::UnityEngine.UI.Text effect;

	public global::UnityEngine.UI.Text requirement;

	public global::UnityEngine.UI.Text masteryEffect;

	public global::UnityEngine.UI.Text masteryRequirement;

	public global::UnityEngine.UI.Text learningTime;

	public global::UnityEngine.UI.Text skillType;

	public global::UnityEngine.UI.Text ratingValue;

	public global::System.Collections.Generic.List<global::UnityEngine.UI.Image> offensePoints;

	public global::System.Collections.Generic.List<global::UnityEngine.UI.Image> strategyPoints;

	public global::UnityEngine.GameObject masteryGroup;
}
