using System;
using UnityEngine;
using UnityEngine.UI;

public class UISkillItem : global::UnityEngine.MonoBehaviour
{
	public void Set(global::SkillData skillData, bool canLearnSkill)
	{
		this.skillName.text = global::SkillHelper.GetLocalizedName(skillData);
		if (this.icon != null)
		{
			this.icon.sprite = global::SkillHelper.GetIcon(skillData);
		}
		if (this.pointCost != null)
		{
			this.pointCost.text = skillData.Points.ToConstantString();
			this.pointCost.color = ((!global::PandoraSingleton<global::HideoutManager>.Instance.currentUnit.unit.HasEnoughPointsForSkill(skillData)) ? global::UnityEngine.Color.red : global::UnityEngine.Color.white);
		}
		if (this.goldCost != null)
		{
			int skillLearnPrice = global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.GetSkillLearnPrice(skillData, global::PandoraSingleton<global::HideoutManager>.Instance.currentUnit.unit);
			this.goldCost.text = skillLearnPrice.ToConstantString();
			if (global::PandoraSingleton<global::HideoutManager>.Instance.WarbandChest.GetGold() < skillLearnPrice)
			{
				this.goldCost.color = global::UnityEngine.Color.red;
			}
		}
		if (this.ratingValue != null)
		{
			this.ratingValue.text = global::SkillHelper.GetRating(skillData).ToConstantString();
		}
		if (this.cannotLearnIcon != null)
		{
			this.cannotLearnIcon.enabled = !canLearnSkill;
		}
		this.mastery.enabled = (skillData.SkillQualityId == global::SkillQualityId.MASTER_QUALITY);
		if (this.redSplatter != null)
		{
			this.redSplatter.enabled = !canLearnSkill;
		}
	}

	public void Set(global::WarbandSkill skill)
	{
		this.skillName.text = skill.LocalizedName;
		if (this.icon != null)
		{
			this.icon.sprite = skill.GetIcon();
		}
		if (this.pointCost != null)
		{
			this.pointCost.text = skill.Data.Points.ToConstantString();
		}
		this.mastery.enabled = skill.IsMastery;
		if (this.goldCost != null)
		{
			this.goldCost.transform.parent.gameObject.SetActive(false);
		}
		if (this.cannotLearnIcon != null)
		{
			this.cannotLearnIcon.enabled = !skill.CanBuy;
		}
		if (this.redSplatter != null)
		{
			this.redSplatter.enabled = !skill.CanBuy;
		}
		if (this.ratingSection != null)
		{
			this.ratingSection.SetActive(false);
		}
	}

	public global::ToggleEffects toggle;

	public global::UnityEngine.UI.Text skillName;

	public global::UnityEngine.UI.Image icon;

	public global::UnityEngine.UI.Image cannotLearnIcon;

	public global::UnityEngine.UI.Text pointCost;

	public global::UnityEngine.UI.Text goldCost;

	public global::UnityEngine.UI.Text ratingValue;

	public global::UnityEngine.GameObject ratingSection;

	public global::UnityEngine.UI.Image mastery;

	public global::UnityEngine.UI.Image redSplatter;

	public global::UnityEngine.CanvasGroup canvasGroup;
}
