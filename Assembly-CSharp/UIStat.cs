using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIStat : global::UnityEngine.MonoBehaviour
{
	protected virtual void Awake()
	{
		if (this.statSelector != null)
		{
			this.statSelector.onSelect.AddListener(new global::UnityEngine.Events.UnityAction(this.OnStatSelected));
			this.statSelector.onUnselect.AddListener(new global::UnityEngine.Events.UnityAction(this.OnStatUnselected));
		}
	}

	public virtual void Refresh(global::Unit unit, bool showArrows, global::System.Action<global::AttributeId> statSelected, global::System.Action<global::AttributeId, bool> statChanged = null, global::System.Action<global::AttributeId> statUnselected = null)
	{
		this.statSelectedCallback = statSelected;
		this.statUnselectedCallback = statUnselected;
		this.RefreshAttribute(unit);
	}

	public virtual void RefreshAttribute(global::Unit unit)
	{
		global::AttributeId maxAttribute = unit.GetMaxAttribute(this.statId);
		if (maxAttribute == global::AttributeId.NONE)
		{
			this.value.text = this.GenerateStatsText(unit, unit.GetAttribute(this.statId), null);
		}
		else
		{
			this.value.text = this.GenerateStatsText(unit, unit.GetAttribute(this.statId), new int?(unit.GetAttribute(maxAttribute)));
		}
	}

	protected virtual string GenerateStatsText(global::Unit unit, int stat, int? statMax)
	{
		if (statMax != null)
		{
			return global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("menu_stat_max_value", new string[]
			{
				stat.ToConstantString(),
				statMax.ToString()
			});
		}
		global::AttributeData attributeData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::AttributeData>((int)this.statId);
		if (unit.HasModifierType(this.statId, global::AttributeMod.Type.TEMP))
		{
			return global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("menu_stat_increase_value", new string[]
			{
				stat + ((!attributeData.IsPercent) ? string.Empty : "%")
			});
		}
		return stat + ((!attributeData.IsPercent) ? string.Empty : "%");
	}

	private void OnStatSelected()
	{
		if (this.statSelectedCallback != null)
		{
			this.statSelectedCallback(this.statId);
		}
	}

	private void OnStatUnselected()
	{
		if (this.statUnselectedCallback != null)
		{
			this.statUnselectedCallback(this.statId);
		}
	}

	public virtual bool HasChanges()
	{
		return false;
	}

	public virtual void ApplyChanges(global::Unit unit)
	{
	}

	public virtual void RevertChanges(global::Unit unit)
	{
	}

	public global::AttributeId statId;

	public global::ToggleEffects statSelector;

	public global::UnityEngine.UI.Text value;

	protected global::System.Action<global::AttributeId> statSelectedCallback;

	protected global::System.Action<global::AttributeId> statUnselectedCallback;

	public bool canGoRight = true;
}
