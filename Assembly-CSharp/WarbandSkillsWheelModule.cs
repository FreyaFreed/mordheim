using System;
using System.Collections.Generic;
using UnityEngine.Events;

public class WarbandSkillsWheelModule : global::UIModule
{
	public void Set(global::UnityEngine.Events.UnityAction<int, global::WarbandSkill> skillSelected, global::UnityEngine.Events.UnityAction skillConfirmed)
	{
		this.onSkillSelected = skillSelected;
		this.onSkillConfirmed = skillConfirmed;
		base.gameObject.SetActive(true);
		this.rank = global::PandoraSingleton<global::GameManager>.Instance.Profile.Rank;
		this.skills = global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.GetPlayerSkills();
		for (int i = 0; i < this.slots.Count; i++)
		{
			this.slots[i].Set(i, (i >= this.skills.Count) ? null : this.skills[i], new global::System.Action<int, global::WarbandSkill>(this.OnSkillSelected), new global::System.Action<int, global::WarbandSkill>(this.OnSkillConfirmed), false);
		}
	}

	private void OnSkillSelected(int idx, global::WarbandSkill skill)
	{
		if (this.onSkillSelected != null)
		{
			this.onSkillSelected(idx, skill);
		}
	}

	private void OnSkillConfirmed(int idx, global::WarbandSkill skill)
	{
		if (this.onSkillConfirmed != null)
		{
			this.onSkillConfirmed();
		}
	}

	public global::System.Collections.Generic.List<global::WarbandSkillWheelSlot> slots;

	private int rank;

	private global::System.Collections.Generic.List<global::WarbandSkill> skills;

	private global::UnityEngine.Events.UnityAction<int, global::WarbandSkill> onSkillSelected;

	private global::UnityEngine.Events.UnityAction onSkillConfirmed;
}
