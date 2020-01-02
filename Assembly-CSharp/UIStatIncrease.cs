using System;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

public class UIStatIncrease : global::UIStat
{
	protected override void Awake()
	{
		base.Awake();
		this.up.onAction.AddListener(new global::UnityEngine.Events.UnityAction(this.IncreateStat));
		this.down.onAction.AddListener(new global::UnityEngine.Events.UnityAction(this.DecreateStat));
	}

	public override void Refresh(global::Unit unit, bool showArrows, global::System.Action<global::AttributeId> statSelected, global::System.Action<global::AttributeId, bool> statChanged, global::System.Action<global::AttributeId> statUnselected)
	{
		this.isShowArrows = showArrows;
		base.Refresh(unit, showArrows, statSelected, statChanged, statUnselected);
		this.statChangedCallback = statChanged;
		this.RefreshArrows(unit);
	}

	public override void RefreshAttribute(global::Unit unit)
	{
		base.RefreshAttribute(unit);
		this.RefreshArrows(unit);
	}

	private void RefreshArrows(global::Unit unit)
	{
		if (this.isShowArrows && unit.CanRaiseAttribute(this.statId))
		{
			this.up.gameObject.SetActive(true);
		}
		else
		{
			this.up.gameObject.SetActive(false);
		}
		if (this.isShowArrows && unit.CanLowerAttribute(this.statId))
		{
			this.down.gameObject.SetActive(true);
		}
		else
		{
			this.down.gameObject.SetActive(false);
		}
	}

	private void IncreateStat()
	{
		this.statSelector.SetSelected(false);
		this.statChangedCallback(this.statId, true);
		this.statSelectedCallback(this.statId);
	}

	private void DecreateStat()
	{
		this.statSelector.SetSelected(false);
		this.statChangedCallback(this.statId, false);
		this.statSelectedCallback(this.statId);
	}

	protected override string GenerateStatsText(global::Unit unit, int stat, int? statMax)
	{
		int num = unit.GetBaseAttribute(this.statId) + unit.GetTempAttribute(this.statId);
		int attribute = unit.GetAttribute(this.statId);
		int baseAttribute = unit.GetBaseAttribute(unit.GetMaxAttribute(this.statId));
		int attribute2 = unit.GetAttribute(unit.GetMaxAttribute(this.statId));
		global::System.Text.StringBuilder stringBuilder = global::PandoraUtils.StringBuilder;
		bool flag = unit.GetTempAttribute(this.statId) > 0;
		if (flag)
		{
			stringBuilder.AppendFormat("{0}<b>{1}</b>{2}", global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("color_cyan"), attribute, global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("color_end"));
		}
		else
		{
			stringBuilder.Append(attribute);
		}
		stringBuilder.Append(' ');
		if (num != attribute)
		{
			if (flag)
			{
				stringBuilder.AppendFormat("({0}<b>{1}</b>{2}) ", global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("color_cyan"), num, global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("color_end"));
			}
			else
			{
				stringBuilder.AppendFormat("({0}) ", num);
			}
		}
		stringBuilder.Append("/ ");
		stringBuilder.Append(attribute2);
		stringBuilder.Append(' ');
		if (baseAttribute != attribute2)
		{
			stringBuilder.AppendFormat("({0})", baseAttribute);
		}
		return stringBuilder.ToString();
	}

	public int increase;

	public global::UnityEngine.GameObject arrows;

	public global::ToggleEffects up;

	public global::ToggleEffects down;

	public global::System.Action<global::AttributeId, bool> statChangedCallback;

	private bool isShowArrows;
}
