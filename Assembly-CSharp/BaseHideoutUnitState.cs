using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public abstract class BaseHideoutUnitState : global::ICheapState
{
	protected BaseHideoutUnitState(global::HideoutCamAnchor anchor, global::HideoutManager.State state)
	{
		this.camAnchor = anchor;
		this.statemachineState = state;
	}

	protected virtual void OnAttributeSelected(global::AttributeId attributeId)
	{
		this.attributeSelected = attributeId;
		if (attributeId != global::AttributeId.NONE)
		{
			if (attributeId == global::AttributeId.DAMAGE_MIN)
			{
				this.ShowDescription(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("attribute_name_weapon_damage"), global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("attribute_desc_weapon_damage"));
			}
			else
			{
				string str = attributeId.ToLowerString();
				this.ShowDescription(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("attribute_name_" + str), global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("attribute_desc_" + str));
			}
		}
	}

	protected virtual void OnAttributeUnselected(global::AttributeId attributeId)
	{
		if (this.descModule != null)
		{
			if (this.selectedStatHiding != null)
			{
				global::PandoraSingleton<global::HideoutManager>.Instance.StopCoroutine(this.selectedStatHiding);
			}
			this.selectedStatHiding = global::PandoraSingleton<global::HideoutManager>.Instance.StartCoroutine(this.HideStatDescDelayed());
		}
	}

	private global::System.Collections.IEnumerator HideStatDescDelayed()
	{
		yield return new global::UnityEngine.WaitForSeconds(1f);
		this.HideStatsDesc();
		yield break;
	}

	protected virtual void HideStatsDesc()
	{
		this.descModule.gameObject.SetActive(false);
	}

	protected virtual void ShowDescription(string title, string desc)
	{
		this.descModule.gameObject.SetActive(true);
		this.descModule.desc.SetLocalized(title, desc);
		if (this.selectedStatHiding != null)
		{
			global::PandoraSingleton<global::HideoutManager>.Instance.StopCoroutine(this.selectedStatHiding);
			this.selectedStatHiding = null;
		}
	}

	public void SetupAttributeButtons(global::ButtonGroup lowerAttributes, global::ButtonGroup raiseAttributes, global::ButtonGroup applyChanges)
	{
		if (this.attributeSelected != global::AttributeId.NONE && global::PandoraSingleton<global::HideoutManager>.Instance.currentUnit.unit.CanLowerAttribute(this.attributeSelected))
		{
			lowerAttributes.SetAction("raise_attribute", "menu_lower_attribute", 0, true, null, null);
			lowerAttributes.OnAction(new global::UnityEngine.Events.UnityAction(this.LowerAttribute), false, true);
		}
		else
		{
			lowerAttributes.gameObject.SetActive(false);
		}
		if (this.attributeSelected != global::AttributeId.NONE && global::PandoraSingleton<global::HideoutManager>.Instance.currentUnit.unit.CanRaiseAttribute(this.attributeSelected))
		{
			raiseAttributes.SetAction("raise_attribute", "menu_raise_attribute", 0, false, null, null);
			raiseAttributes.OnAction(new global::UnityEngine.Events.UnityAction(this.RaiseAttribute), false, true);
		}
		else
		{
			raiseAttributes.gameObject.SetActive(false);
		}
		if (global::PandoraSingleton<global::HideoutManager>.Instance.currentUnit.unit.HasPendingChanges())
		{
			if (!applyChanges.isActiveAndEnabled)
			{
				applyChanges.SetAction("action", "menu_apply", 0, false, null, null);
				applyChanges.OnAction(new global::UnityEngine.Events.UnityAction(this.ApplyChanges), false, true);
			}
		}
		else
		{
			applyChanges.gameObject.SetActive(false);
		}
	}

	private void LowerAttribute()
	{
		global::PandoraSingleton<global::HideoutManager>.Instance.currentUnit.unit.LowerAttribute(this.attributeSelected);
		this.RefreshUnitAttributes();
		this.statsModule.SelectStat(this.attributeSelected);
		this.OnAttributeSelected(this.attributeSelected);
	}

	private void RaiseAttribute()
	{
		global::PandoraSingleton<global::HideoutManager>.Instance.currentUnit.unit.RaiseAttribute(this.attributeSelected, true);
		this.RefreshUnitAttributes();
		this.statsModule.SelectStat(this.attributeSelected);
		this.OnAttributeSelected(this.attributeSelected);
	}

	public void SetupApplyButton(global::ButtonGroup applyChanges)
	{
		if (global::PandoraSingleton<global::HideoutManager>.Instance.currentUnit.unit.HasPendingChanges())
		{
			applyChanges.SetAction("action", "menu_apply_changes", 0, false, null, null);
			applyChanges.OnAction(new global::UnityEngine.Events.UnityAction(this.ApplyChangesAndResetDescription), false, true);
		}
		else
		{
			applyChanges.gameObject.SetActive(false);
		}
	}

	private void ApplyChangesAndResetDescription()
	{
		this.ApplyChanges();
		this.OnAttributeSelected(this.attributeSelected);
	}

	private global::System.Collections.IEnumerator ReloadStateOnNextFrame()
	{
		yield return 0;
		global::PandoraSingleton<global::HideoutManager>.Instance.StateMachine.ChangeState(global::PandoraSingleton<global::HideoutManager>.Instance.StateMachine.GetActiveStateId());
		yield break;
	}

	private void ApplyChanges()
	{
		this.CheckForChanges(new global::System.Action(this.OnApplyChanges), false);
	}

	public virtual void OnApplyChanges()
	{
		this.RefreshUnitAttributes();
		this.OnAttributeSelected(global::AttributeId.NONE);
	}

	public void ReturnToWarband()
	{
		this.CheckChangesAndReturnToWarband();
	}

	protected void CheckForChanges(global::System.Action callback, bool checkOutsider = false)
	{
		if (checkOutsider && global::PandoraSingleton<global::HideoutManager>.Instance.IsTrainingOutsider)
		{
			this.applyChangesCallback = callback;
			if (this.currentUnit.unit.UnspentMartial > 0 || this.currentUnit.unit.UnspentMental > 0 || this.currentUnit.unit.UnspentPhysical > 0 || this.currentUnit.unit.UnspentSkill > 0 || this.currentUnit.unit.UnspentSpell > 0)
			{
				global::PandoraSingleton<global::HideoutManager>.Instance.messagePopup.Show("popup_quit_outsider_title", "popup_quit_outsider_desc", new global::System.Action<bool>(this.OnOutsiderPopup), false, false);
			}
			else
			{
				global::PandoraSingleton<global::HideoutManager>.Instance.messagePopup.Show("popup_quit_outsider_no_point_title", "popup_quit_outsider_no_point_desc", new global::System.Action<bool>(this.OnOutsiderPopup), false, false);
			}
		}
		else if (this.currentUnit != null && this.currentUnit.unit.HasPendingChanges())
		{
			this.applyChangesCallback = callback;
			global::PandoraSingleton<global::HideoutManager>.Instance.messagePopup.Show("popup_apply_changes_title", "popup_apply_changes_desc", new global::System.Action<bool>(this.OnApplyPopup), false, false);
		}
		else if (callback != null)
		{
			callback();
		}
	}

	private void OnOutsiderPopup(bool confirm)
	{
		if (confirm)
		{
			this.currentUnit.unit.ApplyChanges(true);
			global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.HireUnit(this.currentUnit);
			global::PandoraSingleton<global::HideoutManager>.Instance.IsTrainingOutsider = false;
			global::PandoraSingleton<global::HideoutManager>.Instance.SaveChanges();
			if (this.applyChangesCallback != null)
			{
				this.applyChangesCallback();
				this.applyChangesCallback = null;
			}
		}
	}

	private void OnApplyPopup(bool confirm)
	{
		if (confirm)
		{
			this.currentUnit.unit.ApplyChanges(true);
			global::PandoraSingleton<global::HideoutManager>.Instance.SaveChanges();
		}
		else
		{
			this.currentUnit.unit.ResetChanges();
		}
		if (this.applyChangesCallback != null)
		{
			this.applyChangesCallback();
			this.applyChangesCallback = null;
		}
	}

	public void CheckChangesAndChangeState(global::HideoutManager.State newState)
	{
		int stateIndex = (int)newState;
		this.CheckForChanges(delegate
		{
			global::PandoraSingleton<global::HideoutManager>.Instance.StateMachine.ChangeState(stateIndex);
		}, false);
	}

	public void CheckChangesAndReturnToWarband()
	{
		this.CheckForChanges(new global::System.Action(this.ReturnToWarbandState), true);
	}

	private void ReturnToWarbandState()
	{
		global::PandoraSingleton<global::HideoutManager>.Instance.StateMachine.ChangeState(1);
	}

	public void ChangeCurrentUnit(global::UnitMenuController ctrlr, global::System.Action callback)
	{
		this.CheckForChanges(delegate
		{
			this.currentUnit = ctrlr;
			if (callback != null)
			{
				callback();
			}
		}, false);
	}

	public void Destroy()
	{
	}

	public virtual void Enter(int iFrom)
	{
		this.Init();
		global::PandoraSingleton<global::HideoutTabManager>.Instance.ActivateLeftTabModules(true, new global::ModuleId[]
		{
			global::ModuleId.UNIT_SHEET,
			global::ModuleId.UNIT_STATS
		});
		this.statsModule = global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleLeft<global::UnitStatsModule>(global::ModuleId.UNIT_STATS);
		this.statsModule.SetInteractable(this.CanIncreaseAttributes());
		this.sheetModule = global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleLeft<global::UnitSheetModule>(global::ModuleId.UNIT_SHEET);
		this.sheetModule.SetInteractable(this.CanIncreaseAttributes());
	}

	protected void Init()
	{
		global::PandoraSingleton<global::HideoutManager>.Instance.CamManager.dummyCam.transform.position = this.camAnchor.transform.position;
		global::PandoraSingleton<global::HideoutManager>.Instance.CamManager.dummyCam.transform.rotation = this.camAnchor.transform.rotation;
		global::PandoraSingleton<global::HideoutManager>.Instance.CamManager.SetDOFTarget(this.camAnchor.dofTarget, 0f);
		this.currentUnit = global::PandoraSingleton<global::HideoutManager>.Instance.currentUnit;
		this.firstSelectedUnit = true;
		this.tabMod = global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleCenter<global::UnitTabsModule>(global::ModuleId.UNIT_TABS);
		this.tabMod.Setup(global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleCenter<global::TitleModule>(global::ModuleId.TITLE));
		this.tabMod.SetCurrentTab(this.statemachineState);
		global::PandoraSingleton<global::NoticeManager>.Instance.RegisterListener(global::Notices.NEXT_UNIT, new global::DelReceiveNotice(this.OnNextUnit));
	}

	public virtual void Exit(int iTo)
	{
		this.currentUnit = null;
		if (this.statsModule != null)
		{
			this.statsModule.toggleGroup.SetAllTogglesOff();
		}
		global::PandoraSingleton<global::NoticeManager>.Instance.RemoveListener(global::Notices.NEXT_UNIT, new global::DelReceiveNotice(this.OnNextUnit));
		global::PandoraSingleton<global::HideoutTabManager>.Instance.DeactivateAllButtons();
		global::PandoraSingleton<global::HideoutManager>.Instance.CamManager.CancelTransition();
	}

	public virtual void Update()
	{
	}

	public virtual void FixedUpdate()
	{
	}

	private void OnNextUnit()
	{
		if (global::PandoraSingleton<global::HideoutManager>.Instance.IsTrainingOutsider)
		{
			return;
		}
		bool flag = (bool)global::PandoraSingleton<global::NoticeManager>.Instance.Parameters[0];
		int num = global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.unitCtrlrs.IndexOf(this.currentUnit);
		int num2 = num;
		if (flag)
		{
			num = ((num - 1 < 0) ? (global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.unitCtrlrs.Count - 1) : (num - 1));
		}
		else
		{
			num = ((num + 1 >= global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.unitCtrlrs.Count) ? 0 : (num + 1));
		}
		if (num != num2)
		{
			this.ChangeCurrentUnit(global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.unitCtrlrs[num], new global::System.Action(this.OnUnitChanged));
		}
	}

	private void OnUnitChanged()
	{
		this.SelectUnit(this.currentUnit);
	}

	public virtual void SelectUnit(global::UnitMenuController ctrlr)
	{
		global::PandoraSingleton<global::HideoutManager>.Instance.currentUnit = ctrlr;
		this.currentUnit = ctrlr;
		this.currentUnit.Hide(false, true, null);
		global::PandoraSingleton<global::HideoutManager>.Instance.unitNode.SetContent(this.currentUnit, null);
		global::PandoraSingleton<global::HideoutManager>.Instance.CamManager.SetDOFTarget(global::PandoraSingleton<global::HideoutManager>.Instance.unitNode.transform, 1.25f);
		this.currentUnit.SwitchWeapons(global::UnitSlotId.SET1_MAINHAND);
		if (this.statsModule != null)
		{
			if (this.CanIncreaseAttributes())
			{
				this.statsModule.RefreshStats(this.ModuleLeftOnRight(), this.currentUnit.unit, new global::System.Action<global::AttributeId>(this.OnAttributeSelected), new global::System.Action(this.OnAttributeChanged), new global::System.Action<global::AttributeId>(this.OnAttributeUnselected));
			}
			else
			{
				this.statsModule.RefreshStats(this.currentUnit.unit, null, null);
			}
		}
		if (this.sheetModule != null)
		{
			this.sheetModule.Refresh(this.ModuleLeftOnRight(), this.currentUnit.unit, new global::System.Action<global::AttributeId>(this.OnAttributeSelected), new global::System.Action<string, string>(this.ShowDescription), new global::System.Action<global::AttributeId>(this.OnAttributeUnselected));
		}
		this.RefreshUnitAttributes();
		this.tabMod.Refresh();
		if (this.characterCamModule != null)
		{
			this.characterCamModule.SetCameraLookAtDefault(this.firstSelectedUnit);
			this.firstSelectedUnit = false;
		}
		if (!this.tabMod.IsTabAvailable(this.statemachineState))
		{
			global::PandoraSingleton<global::HideoutManager>.Instance.StartCoroutine(this.ReturnToUnitInfo());
		}
	}

	public global::System.Collections.IEnumerator ReturnToUnitInfo()
	{
		yield return new global::UnityEngine.WaitForEndOfFrame();
		global::PandoraSingleton<global::HideoutManager>.Instance.StateMachine.ChangeState(15);
		yield break;
	}

	public abstract global::UnityEngine.UI.Selectable ModuleLeftOnRight();

	public global::ToggleEffects ModuleCentertOnLeft()
	{
		return this.statsModule.stats[0].statSelector;
	}

	public void RefreshUnitAttributes()
	{
		if (this.sheetModule != null)
		{
			this.sheetModule.RefreshAttributes(this.currentUnit.unit);
		}
		if (this.statsModule != null)
		{
			this.statsModule.RefreshAttributes(this.currentUnit.unit);
		}
	}

	protected virtual void OnAttributeChanged()
	{
		this.RefreshUnitAttributes();
	}

	public abstract bool CanIncreaseAttributes();

	protected global::HideoutCamAnchor camAnchor;

	private global::HideoutManager.State statemachineState;

	protected global::UnitTabsModule tabMod;

	protected global::CharacterCameraAreaModule characterCamModule;

	protected global::UnitSheetModule sheetModule;

	protected global::UnitStatsModule statsModule;

	protected global::DescriptionModule descModule;

	protected global::UnitMenuController currentUnit;

	private global::AttributeId attributeSelected;

	private global::System.Action applyChangesCallback;

	private bool firstSelectedUnit;

	private global::UnityEngine.Coroutine selectedStatHiding;
}
