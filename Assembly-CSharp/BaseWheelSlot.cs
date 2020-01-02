using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public abstract class BaseWheelSlot<T> : global::UnityEngine.MonoBehaviour
{
	public void Set(int index, T skill, global::System.Action<int, T> onSkillSelected, global::System.Action<int, T> activeSkillsConfirmed, bool isInTraining)
	{
		this.skillIndex = index;
		this.skillData = skill;
		this.activeSkillsConfirmedCallback = activeSkillsConfirmed;
		this.onSkillSelectedCallback = onSkillSelected;
		this.ResetListeners();
		this.toggle.onSelect.RemoveAllListeners();
		this.toggle.onSelect.AddListener(new global::UnityEngine.Events.UnityAction(this.SkillSelected));
		this.toggle.onPointerEnter.RemoveAllListeners();
		this.toggle.onPointerEnter.AddListener(new global::UnityEngine.Events.UnityAction(this.SkillSelected));
		this.toggle.onAction.RemoveAllListeners();
		this.toggle.onAction.AddListener(new global::UnityEngine.Events.UnityAction(this.SkillConfirmed));
		this.icon.overrideSprite = this.GetIcon();
		this.mastery.enabled = this.IsMastery();
		if (this.inTraining != null)
		{
			this.icon.enabled = !isInTraining;
			this.inTraining.enabled = isInTraining;
		}
	}

	public void SkillSelected()
	{
		this.onSkillSelectedCallback(this.skillIndex, this.skillData);
	}

	public void SkillConfirmed()
	{
		this.activeSkillsConfirmedCallback(this.skillIndex, this.skillData);
	}

	public void ResetListeners()
	{
		this.toggle.onSelect.RemoveAllListeners();
	}

	protected abstract global::UnityEngine.Sprite GetIcon();

	protected abstract bool IsMastery();

	public global::ToggleEffects toggle;

	public global::UnityEngine.UI.Image icon;

	public global::UnityEngine.UI.Image mastery;

	public global::UnityEngine.UI.Image inTraining;

	protected global::System.Action<int, T> onSkillSelectedCallback;

	protected global::System.Action<int, T> activeSkillsConfirmedCallback;

	protected T skillData;

	protected int skillIndex;
}
