using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class HideoutHire : global::ICheapState
{
	public HideoutHire(global::HideoutManager mng, global::HideoutCamAnchor anchor)
	{
		this.camAnchor = anchor;
	}

	public void Destroy()
	{
	}

	public void Enter(int iFrom)
	{
		global::PandoraSingleton<global::HideoutManager>.Instance.CamManager.dummyCam.transform.position = this.camAnchor.transform.position;
		global::PandoraSingleton<global::HideoutManager>.Instance.CamManager.dummyCam.transform.rotation = this.camAnchor.transform.rotation;
		global::PandoraSingleton<global::HideoutManager>.Instance.CamManager.SetDOFTarget(this.camAnchor.dofTarget, 0f);
		this.firstSelectedUnit = true;
		this.warband = global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband;
		if (global::PandoraSingleton<global::HideoutManager>.Instance.currentWarbandSlotIdx != -1)
		{
			this.hireUnits = global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.GetHireableUnits(global::PandoraSingleton<global::HideoutManager>.Instance.currentWarbandSlotIdx, global::PandoraSingleton<global::HideoutManager>.Instance.currentWarbandSlotHireImpressive);
		}
		else
		{
			this.hireUnits = global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.GetHiredSwordUnits();
		}
		global::PandoraSingleton<global::HideoutTabManager>.Instance.ActivateLeftTabModules(true, new global::ModuleId[]
		{
			global::ModuleId.UNIT_SHEET,
			global::ModuleId.UNIT_STATS
		});
		global::PandoraSingleton<global::HideoutTabManager>.Instance.ActivateRightTabModules(true, new global::ModuleId[]
		{
			global::ModuleId.TREASURY,
			global::ModuleId.UNIT_DESC
		});
		if (this.hireUnits.Count > 1)
		{
			global::PandoraSingleton<global::HideoutTabManager>.Instance.ActivateCenterTabModules(new global::ModuleId[]
			{
				global::ModuleId.HIRE_UNIT_SELECTION,
				global::ModuleId.TITLE,
				global::ModuleId.CHARACTER_AREA,
				global::ModuleId.NEXT_UNIT
			});
			global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleCenter<global::NextUnitModule>(global::ModuleId.NEXT_UNIT).Setup();
		}
		else
		{
			global::PandoraSingleton<global::HideoutTabManager>.Instance.ActivateCenterTabModules(new global::ModuleId[]
			{
				global::ModuleId.HIRE_UNIT_SELECTION,
				global::ModuleId.TITLE,
				global::ModuleId.CHARACTER_AREA
			});
		}
		global::TitleModule moduleCenter = global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleCenter<global::TitleModule>(global::ModuleId.TITLE);
		moduleCenter.Set("hideout_hire", true);
		this.statsModule = global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleLeft<global::UnitStatsModule>(global::ModuleId.UNIT_STATS);
		this.statsModule.SetInteractable(false);
		this.sheetModule = global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleLeft<global::UnitSheetModule>(global::ModuleId.UNIT_SHEET);
		this.sheetModule.SetInteractable(false);
		this.unitDescModule = global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleRight<global::UnitDescriptionModule>(global::ModuleId.UNIT_DESC);
		this.characterCamModule = global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleCenter<global::CharacterCameraAreaModule>(global::ModuleId.CHARACTER_AREA);
		this.characterCamModule.Init(this.camAnchor.transform.position);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleRight<global::TreasuryModule>(global::ModuleId.TREASURY).Refresh(global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.GetWarbandSave());
		this.unitSelectionModule = global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleCenter<global::HireUnitSelectionModule>(global::ModuleId.HIRE_UNIT_SELECTION);
		if (global::PandoraSingleton<global::HideoutManager>.Instance.currentWarbandSlotIdx != -1)
		{
			this.unitSelectionModule.Set(this.hireUnits, new global::System.Action(this.Prev), new global::System.Action(this.Next), new global::System.Action<int>(this.UnitConfirmed), new global::System.Action<int>(this.OnDoubleClick));
		}
		else
		{
			this.unitSelectionModule.Set(this.hireUnits, new global::System.Action(this.Prev), new global::System.Action(this.Next), new global::System.Action<int>(this.UnitConfirmed), null);
		}
		global::PandoraSingleton<global::NoticeManager>.Instance.RegisterListener(global::Notices.NEXT_UNIT, new global::DelReceiveNotice(this.OnNextUnit));
		this.unitIndex = 0;
		this.SelectUnit(this.hireUnits[this.unitIndex]);
		this.once = true;
	}

	private void OnNextUnit()
	{
		bool flag = (bool)global::PandoraSingleton<global::NoticeManager>.Instance.Parameters[0];
		if (flag)
		{
			this.Prev();
		}
		else
		{
			this.Next();
		}
	}

	private void UnitConfirmed(int newIndex)
	{
		this.unitIndex = newIndex;
		if (!this.once && !global::PandoraSingleton<global::HideoutManager>.Instance.showingTuto)
		{
			this.SelectUnit(this.hireUnits[newIndex]);
		}
	}

	private void OnDoubleClick(int newIndex)
	{
		this.UnitConfirmed(newIndex);
		this.LaunchHirePopup();
	}

	public void Exit(int iTo)
	{
		global::PandoraSingleton<global::HideoutTabManager>.Instance.DeactivateAllButtons();
		global::PandoraSingleton<global::NoticeManager>.Instance.RemoveListener(global::Notices.NEXT_UNIT, new global::DelReceiveNotice(this.OnNextUnit));
		this.unitSelectionModule.Clear();
		global::PandoraSingleton<global::HideoutManager>.Instance.CamManager.CancelTransition();
	}

	public void Update()
	{
		if (global::PandoraSingleton<global::HideoutManager>.Instance.currentWarbandSlotIdx != -1 && global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp("action", 0))
		{
			this.LaunchHirePopup();
		}
	}

	private void Prev()
	{
		this.unitIndex = ((this.unitIndex - 1 < 0) ? (this.hireUnits.Count - 1) : (this.unitIndex - 1));
		this.SelectUnit(this.hireUnits[this.unitIndex]);
	}

	private void Next()
	{
		this.unitIndex = ((this.unitIndex + 1 >= this.hireUnits.Count) ? 0 : (this.unitIndex + 1));
		this.SelectUnit(this.hireUnits[this.unitIndex]);
	}

	public void FixedUpdate()
	{
		if (this.once)
		{
			this.once = false;
			global::PandoraSingleton<global::HideoutManager>.Instance.ShowHideoutTuto(global::HideoutManager.HideoutTutoType.HIRE);
		}
	}

	public void SelectUnit(global::UnitMenuController unitCtrlr)
	{
		if (unitCtrlr != null)
		{
			int index = this.hireUnits.IndexOf(unitCtrlr);
			this.unitSelectionModule.OnUnitSelected(index);
			global::PandoraSingleton<global::HideoutManager>.Instance.currentUnit = unitCtrlr;
			this.currentUnit = unitCtrlr;
			global::PandoraSingleton<global::HideoutManager>.Instance.unitNode.SetContent(this.currentUnit, null);
			global::PandoraSingleton<global::HideoutManager>.Instance.CamManager.SetDOFTarget(global::PandoraSingleton<global::HideoutManager>.Instance.unitNode.transform, 1.25f);
			this.currentUnit.SwitchWeapons(global::UnitSlotId.SET1_MAINHAND);
			this.statsModule.RefreshStats(unitCtrlr.unit, null, null);
			this.sheetModule.RefreshAttributes(unitCtrlr.unit);
			this.unitDescModule.Refresh(unitCtrlr.unit, true);
			if (global::PandoraSingleton<global::PandoraInput>.Instance.CurrentInputLayer == 0)
			{
				this.unitDescModule.tabs[0].SetSelected(true);
			}
			global::PandoraSingleton<global::HideoutTabManager>.Instance.button1.SetAction("cancel", "go_to_warband", 0, false, global::PandoraSingleton<global::HideoutTabManager>.Instance.icnBack, null);
			global::PandoraSingleton<global::HideoutTabManager>.Instance.button1.OnAction(new global::UnityEngine.Events.UnityAction(this.ReturnToWarband), false, true);
			global::PandoraSingleton<global::HideoutTabManager>.Instance.button2.gameObject.SetActive(false);
			global::PandoraSingleton<global::HideoutTabManager>.Instance.button3.gameObject.SetActive(false);
			global::PandoraSingleton<global::HideoutTabManager>.Instance.button4.gameObject.SetActive(false);
			this.characterCamModule.SetCameraLookAtDefault(this.firstSelectedUnit);
			this.firstSelectedUnit = false;
		}
	}

	private void LaunchHirePopup()
	{
		if (this.warband.GetUnitHireCost(this.currentUnit.unit) > global::PandoraSingleton<global::HideoutManager>.Instance.WarbandChest.GetGold())
		{
			global::PandoraSingleton<global::HideoutManager>.Instance.messagePopup.ShowLocalized(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_no_gold"), global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_no_gold_hire"), null, false, true);
		}
		else
		{
			global::PandoraSingleton<global::HideoutManager>.Instance.messagePopup.ShowLocalized(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById((!this.currentUnit.unit.UnitSave.isOutsider) ? "hideout_hire" : "popup_hire_outsider_title"), global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById((!this.currentUnit.unit.UnitSave.isOutsider) ? "popup_hire_unit_desc" : "popup_hire_outsider_desc", new string[]
			{
				this.currentUnit.unit.LocalizedName,
				this.warband.GetUnitHireCost(this.currentUnit.unit).ToString()
			}), new global::System.Action<bool>(this.OnHirePopup), false, false);
		}
	}

	private void OnHirePopup(bool confirm)
	{
		if (confirm)
		{
			global::PandoraSingleton<global::HideoutManager>.Instance.WarbandChest.RemoveGold(this.warband.GetUnitHireCost(this.currentUnit.unit));
			global::PandoraSingleton<global::Hephaestus>.Instance.IncrementStat(global::Hephaestus.StatId.HIRED_WARRIORS, 1);
			if (this.currentUnit.unit.UnitSave.isOutsider)
			{
				global::PandoraSingleton<global::HideoutManager>.Instance.currentUnit = this.currentUnit;
				this.currentUnit.unit.UnitSave.warbandSlotIndex = global::PandoraSingleton<global::HideoutManager>.Instance.currentWarbandSlotIdx;
				if (this.currentUnit.unit.Rank == 0)
				{
					global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.HireUnit(this.currentUnit);
					global::PandoraSingleton<global::HideoutManager>.Instance.SaveChanges();
					this.ReturnToWarband();
				}
				else
				{
					global::PandoraSingleton<global::HideoutManager>.Instance.IsTrainingOutsider = true;
					global::PandoraSingleton<global::HideoutManager>.Instance.StateMachine.ChangeState(7);
					global::PandoraSingleton<global::GameManager>.Instance.StartCoroutine(global::HideoutHire.ShowOutsiderPopupOnNextFrame());
				}
			}
			else
			{
				global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.HireUnit(this.currentUnit);
				this.currentUnit.unit.UnitSave.warbandSlotIndex = global::PandoraSingleton<global::HideoutManager>.Instance.currentWarbandSlotIdx;
				global::PandoraSingleton<global::HideoutManager>.Instance.SaveChanges();
				this.ReturnToWarband();
			}
		}
	}

	private static global::System.Collections.IEnumerator ShowOutsiderPopupOnNextFrame()
	{
		yield return null;
		yield return null;
		global::PandoraSingleton<global::HideoutManager>.Instance.messagePopup.Show("popup_new_outsider_title", "popup_new_outsider_desc", null, false, true);
		yield break;
	}

	private void ReturnToWarband()
	{
		global::PandoraSingleton<global::HideoutManager>.Instance.StateMachine.ChangeState(1);
	}

	public global::System.Collections.Generic.List<global::UnitMenuController> hireUnits;

	private global::HideoutCamAnchor camAnchor;

	private global::UnitDescriptionModule unitDescModule;

	private global::UnitStatsModule statsModule;

	private global::UnitSheetModule sheetModule;

	private global::HireUnitSelectionModule unitSelectionModule;

	private global::CharacterCameraAreaModule characterCamModule;

	private int unitIndex;

	private global::UnitMenuController currentUnit;

	private global::Warband warband;

	private bool firstSelectedUnit;

	private bool once = true;
}
