using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellsModule : global::UIModule
{
	public void Refresh()
	{
		this.spellSkillLine = this.skillsShop.GetUnitSpellsSkillLines(global::PandoraSingleton<global::HideoutManager>.Instance.currentUnit.unit);
		if (global::PandoraSingleton<global::HideoutManager>.Instance.currentUnit.unit.UnspentSpell > 0)
		{
			this.unspentSkills.gameObject.SetActive(true);
			this.unspentSkills.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("skill_unspent_point", new string[]
			{
				global::PandoraSingleton<global::HideoutManager>.Instance.currentUnit.unit.UnspentSpell.ToConstantString()
			});
		}
		else
		{
			this.unspentSkills.gameObject.SetActive(false);
		}
		this.canLearnSkills = new global::System.Collections.Generic.List<global::SkillData>();
		this.cannotLearnSkills = new global::System.Collections.Generic.List<global::SkillData>();
	}

	public void ShowSpells(global::System.Action<global::SkillData> onSpellSelected, global::System.Action<global::SkillData> onSpellConfirmed, global::SkillData currentSpell)
	{
		this.onSpellSelectedCallback = onSpellSelected;
		this.onSpellConfirmedCallback = onSpellConfirmed;
		this.scrollGroup.Setup(this.skillPrefab, true);
		this.scrollGroup.ClearList();
		if (currentSpell == null)
		{
			this.skillsShop.GetSkills(global::PandoraSingleton<global::HideoutManager>.Instance.currentUnit.unit, this.spellSkillLine, true, ref this.canLearnSkills, ref this.cannotLearnSkills);
			for (int i = 0; i < this.canLearnSkills.Count; i++)
			{
				global::SkillData skillData = this.canLearnSkills[i];
				this.AddSkill(skillData, true, i == 0);
			}
			for (int j = 0; j < this.cannotLearnSkills.Count; j++)
			{
				global::SkillData skillData2 = this.cannotLearnSkills[j];
				this.AddSkill(skillData2, false, this.canLearnSkills.Count == 0 && j == 0);
			}
		}
		else if (currentSpell.SkillQualityId == global::SkillQualityId.NORMAL_QUALITY)
		{
			global::SkillData skillMastery = global::SkillHelper.GetSkillMastery(currentSpell);
			if (skillMastery != null)
			{
				this.AddSkill(skillMastery, this.skillsShop.CanLearnSkill(skillMastery), true);
			}
		}
	}

	private void AddSkill(global::SkillData skillData, bool canLearn, bool select)
	{
		global::UnityEngine.GameObject gameObject = this.scrollGroup.AddToList(null, null);
		global::UISkillItem component = gameObject.GetComponent<global::UISkillItem>();
		global::SkillData data = skillData;
		component.toggle.onAction.RemoveAllListeners();
		component.toggle.onSelect.RemoveAllListeners();
		if (canLearn)
		{
			component.toggle.onAction.AddListener(delegate()
			{
				this.onSpellConfirmedCallback(data);
			});
		}
		component.toggle.onSelect.AddListener(delegate()
		{
			this.onSpellSelectedCallback(data);
		});
		component.Set(skillData, canLearn);
		if (select)
		{
			gameObject.SetSelected(true);
		}
	}

	public bool HasSpells()
	{
		return this.skillsShop.UnitHasSkillLine(global::PandoraSingleton<global::HideoutManager>.Instance.currentUnit.unit, global::SkillLineId.SPELL);
	}

	public void ClearList()
	{
		this.scrollGroup.ClearList();
	}

	private readonly global::SkillsShop skillsShop = new global::SkillsShop();

	public global::UnityEngine.GameObject skillPrefab;

	public global::ScrollGroup scrollGroup;

	public global::UnityEngine.UI.Text unspentSkills;

	private global::System.Collections.Generic.List<global::SkillLineId> spellSkillLine;

	private global::System.Action<global::SkillData> onSpellSelectedCallback;

	private global::System.Action<global::SkillData> onSpellConfirmedCallback;

	private global::System.Collections.Generic.List<global::SkillData> canLearnSkills;

	private global::System.Collections.Generic.List<global::SkillData> cannotLearnSkills;
}
