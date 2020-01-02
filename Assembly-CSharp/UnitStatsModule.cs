using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class UnitStatsModule : global::UIModule
{
	public void RefreshStats(global::UnityEngine.UI.Selectable right, global::Unit unit, global::System.Action<global::AttributeId> onAttributeSelected = null, global::System.Action attributeChanged = null, global::System.Action<global::AttributeId> onAttributeUnselected = null)
	{
		this.onAttributeChanged = attributeChanged;
		this.currentUnit = unit;
		this.RefreshUnspentPoints();
		for (int i = 0; i < this.stats.Count; i++)
		{
			if (this.stats[i].canGoRight)
			{
				global::UnityEngine.UI.Navigation navigation = this.stats[i].statSelector.toggle.navigation;
				navigation.selectOnRight = right;
				this.stats[i].statSelector.toggle.navigation = navigation;
			}
			this.stats[i].Refresh(unit, true, onAttributeSelected, new global::System.Action<global::AttributeId, bool>(this.OnStatChanged), onAttributeUnselected);
		}
	}

	public void RefreshStats(global::Unit unit, global::System.Action<global::AttributeId> onAttributeSelected = null, global::System.Action<global::AttributeId> onAttributeUnselected = null)
	{
		this.currentUnit = unit;
		this.RefreshUnspentPoints();
		for (int i = 0; i < this.stats.Count; i++)
		{
			this.stats[i].Refresh(unit, false, onAttributeSelected, new global::System.Action<global::AttributeId, bool>(this.OnStatChanged), onAttributeUnselected);
		}
	}

	private void RefreshUnspentPoints()
	{
		if (this.currentUnit.UnspentMartial > 0)
		{
			this.martialStatPoints.gameObject.SetActive(true);
			this.martialStatPoints.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_msg_unspent_points", new string[]
			{
				this.currentUnit.UnspentMartial.ToString()
			});
		}
		else
		{
			this.martialStatPoints.gameObject.SetActive(false);
		}
		if (this.currentUnit.UnspentMental > 0)
		{
			this.mentalStatPoints.gameObject.SetActive(true);
			this.mentalStatPoints.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_msg_unspent_points", new string[]
			{
				this.currentUnit.UnspentMental.ToString()
			});
		}
		else
		{
			this.mentalStatPoints.gameObject.SetActive(false);
		}
		if (this.currentUnit.UnspentPhysical > 0)
		{
			this.physicalStatPoints.gameObject.SetActive(true);
			this.physicalStatPoints.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_msg_unspent_points", new string[]
			{
				this.currentUnit.UnspentPhysical.ToString()
			});
		}
		else
		{
			this.physicalStatPoints.gameObject.SetActive(false);
		}
	}

	public void RefreshAttributes(global::Unit unit)
	{
		this.currentUnit = unit;
		this.RefreshUnspentPoints();
		for (int i = 0; i < this.stats.Count; i++)
		{
			this.stats[i].RefreshAttribute(unit);
		}
	}

	private void OnStatChanged(global::AttributeId attributeId, bool increase)
	{
		if (increase)
		{
			this.currentUnit.RaiseAttribute(attributeId, true);
		}
		else
		{
			this.currentUnit.LowerAttribute(attributeId);
		}
		this.RefreshAttributes(this.currentUnit);
		if (this.onAttributeChanged != null)
		{
			this.onAttributeChanged();
		}
	}

	private void OnEnable()
	{
		if (this.toggleGroup != null)
		{
			this.toggleGroup.SetAllTogglesOff();
		}
		this.highlight.Deactivate();
	}

	public void SelectStat(global::AttributeId attributeId)
	{
		for (int i = 0; i < this.stats.Count; i++)
		{
			if (this.stats[i].statId == attributeId)
			{
				this.stats[i].statSelector.SetOn();
				break;
			}
		}
	}

	public void SetNav(global::UnityEngine.UI.Selectable right)
	{
		for (int i = 0; i < this.stats.Count; i++)
		{
			if (this.stats[i].canGoRight)
			{
				global::UnityEngine.UI.Navigation navigation = this.stats[i].statSelector.toggle.navigation;
				navigation.selectOnRight = right;
				this.stats[i].statSelector.toggle.navigation = navigation;
			}
		}
	}

	public global::HightlightAnimate highlight;

	public global::UnityEngine.UI.ToggleGroup toggleGroup;

	public global::UnityEngine.UI.Text martialStatPoints;

	public global::UnityEngine.UI.Text mentalStatPoints;

	public global::UnityEngine.UI.Text physicalStatPoints;

	public global::System.Collections.Generic.List<global::UIStat> stats;

	private global::Unit currentUnit;

	private global::System.Action onAttributeChanged;
}
