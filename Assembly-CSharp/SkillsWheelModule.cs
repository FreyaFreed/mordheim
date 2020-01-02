using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillsWheelModule : global::UIModule
{
	public void ShowSkills(global::ToggleEffects left, global::System.Action<int, global::SkillData> onSkillSelected, global::System.Action<int, global::SkillData> passiveSkillConfirmed, global::System.Action<int, global::SkillData> activeSkillsConfirmed)
	{
		this.onSkillSelectedCallback = onSkillSelected;
		this.passiveTitle.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("skill_category_title_passive_skill");
		this.passiveSkillsGo.SetActive(true);
		global::Unit unit = global::PandoraSingleton<global::HideoutManager>.Instance.currentUnit.unit;
		global::SkillData skillData = null;
		if (unit.UnitSave.skillInTrainingId != global::SkillId.NONE)
		{
			skillData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::SkillData>((int)unit.UnitSave.skillInTrainingId);
			if (skillData.SpellTypeId != global::SpellTypeId.NONE)
			{
				skillData = null;
			}
		}
		for (int i = 0; i < this.passiveSkills.Count; i++)
		{
			if (unit.PassiveSkills.Count == i && skillData != null && skillData.Passive)
			{
				this.passiveSkills[i].Set(i, skillData, this.onSkillSelectedCallback, passiveSkillConfirmed, true);
			}
			else
			{
				bool flag = i < unit.PassiveSkills.Count && skillData != null && skillData.SkillIdPrerequiste == unit.PassiveSkills[i].Id;
				if (flag)
				{
					this.passiveSkills[i].Set(i, skillData, this.onSkillSelectedCallback, passiveSkillConfirmed, true);
					skillData = null;
				}
				else
				{
					this.passiveSkills[i].Set(i, (i >= unit.PassiveSkills.Count) ? null : unit.PassiveSkills[i], this.onSkillSelectedCallback, passiveSkillConfirmed, false);
				}
			}
		}
		this.activeTitle.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("skill_category_title_active_skill");
		this.activeSkillsGo.SetActive(true);
		for (int j = 0; j < this.activeSkills.Count; j++)
		{
			if (unit.ActiveSkills.Count == j && skillData != null && !skillData.Passive)
			{
				this.activeSkills[j].Set(j, skillData, this.onSkillSelectedCallback, activeSkillsConfirmed, true);
			}
			else
			{
				bool flag2 = j < unit.ActiveSkills.Count && skillData != null && skillData.SkillIdPrerequiste == unit.ActiveSkills[j].Id;
				if (flag2)
				{
					this.activeSkills[j].Set(j, skillData, this.onSkillSelectedCallback, activeSkillsConfirmed, true);
					skillData = null;
				}
				else
				{
					this.activeSkills[j].Set(j, (j >= unit.ActiveSkills.Count) ? null : unit.ActiveSkills[j], this.onSkillSelectedCallback, activeSkillsConfirmed, false);
				}
			}
			global::UnityEngine.UI.Navigation navigation = this.activeSkills[j].toggle.toggle.navigation;
			navigation.selectOnLeft = left.toggle;
			this.activeSkills[j].toggle.toggle.navigation = navigation;
		}
	}

	public void ShowSpells(global::ToggleEffects left, global::System.Action<int, global::SkillData> onSkillSelected, global::System.Action<int, global::SkillData> spellConfirmed)
	{
		global::Unit unit = global::PandoraSingleton<global::HideoutManager>.Instance.currentUnit.unit;
		this.onSkillSelectedCallback = onSkillSelected;
		this.passiveSkillsGo.SetActive(false);
		this.activeTitle.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("skill_category_title_spells");
		this.activeSkillsGo.SetActive(true);
		global::SkillData skillData = null;
		if (unit.UnitSave.skillInTrainingId != global::SkillId.NONE)
		{
			skillData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::SkillData>((int)unit.UnitSave.skillInTrainingId);
			if (skillData.SpellTypeId == global::SpellTypeId.NONE)
			{
				skillData = null;
			}
		}
		for (int i = 0; i < this.activeSkills.Count; i++)
		{
			if (unit.Spells.Count == i && skillData != null && !skillData.Passive)
			{
				this.activeSkills[i].Set(i, skillData, this.onSkillSelectedCallback, spellConfirmed, true);
			}
			else
			{
				bool flag = i < unit.Spells.Count && skillData != null && skillData.SkillIdPrerequiste == unit.Spells[i].Id;
				if (flag)
				{
					this.activeSkills[i].Set(i, skillData, this.onSkillSelectedCallback, spellConfirmed, true);
					skillData = null;
				}
				else
				{
					this.activeSkills[i].Set(i, (i >= unit.Spells.Count) ? null : unit.Spells[i], this.onSkillSelectedCallback, spellConfirmed, false);
				}
			}
			global::UnityEngine.UI.Navigation navigation = this.activeSkills[i].toggle.toggle.navigation;
			navigation.selectOnLeft = left.toggle;
			this.activeSkills[i].toggle.toggle.navigation = navigation;
		}
	}

	public void Deactivate()
	{
		for (int i = 0; i < this.passiveSkills.Count; i++)
		{
			this.passiveSkills[i].ResetListeners();
		}
		for (int j = 0; j < this.activeSkills.Count; j++)
		{
			this.activeSkills[j].ResetListeners();
		}
	}

	public void SelectSlot(int currentSkillIndex, bool currentSkillActive)
	{
		if (currentSkillActive)
		{
			this.activeSkills[currentSkillIndex].toggle.SetSelected(false);
		}
		else
		{
			this.passiveSkills[currentSkillIndex].toggle.SetSelected(false);
		}
	}

	public global::UnityEngine.UI.Text passiveTitle;

	public global::UnityEngine.GameObject passiveSkillsGo;

	public global::System.Collections.Generic.List<global::SkillWheelSlot> passiveSkills;

	public global::UnityEngine.UI.Text activeTitle;

	public global::UnityEngine.GameObject activeSkillsGo;

	public global::System.Collections.Generic.List<global::SkillWheelSlot> activeSkills;

	private global::System.Action<int, global::SkillData> onSkillSelectedCallback;
}
