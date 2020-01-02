using System;
using System.Collections;
using UnityEngine.Events;

public class HideoutEndGameWarband : global::ICheapState
{
	public HideoutEndGameWarband(global::HideoutManager mng, global::HideoutCamAnchor anchor)
	{
		this.camAnchor = anchor;
	}

	void global::ICheapState.Update()
	{
		if (this.goingToCamp)
		{
			return;
		}
		if (this.advance)
		{
			this.advance = false;
			if (this.xpModule != null && this.ooaModule == null && this.xpModule.EndShow())
			{
				if (global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.GetWarbandSave().endMission.wagonItems.GetItems().Count > 0 && global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.GetWarbandSave().endMission.won)
				{
					this.ShowItems();
				}
				else
				{
					this.GoToCamp();
				}
			}
			else if (this.ooaModule != null && this.ooaModule.EndShow())
			{
				this.GoToCamp();
			}
		}
	}

	public void Destroy()
	{
	}

	public void Enter(int iFrom)
	{
		this.advance = false;
		this.goingToCamp = false;
		global::PandoraSingleton<global::HideoutManager>.Instance.CamManager.dummyCam.transform.position = this.camAnchor.transform.position;
		global::PandoraSingleton<global::HideoutManager>.Instance.CamManager.dummyCam.transform.rotation = this.camAnchor.transform.rotation;
		global::PandoraSingleton<global::HideoutManager>.Instance.CamManager.SetDOFTarget(this.camAnchor.dofTarget, 0f);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.ActivateLeftTabModules(true, new global::ModuleId[]
		{
			global::ModuleId.WARBAND_SHEET,
			global::ModuleId.WARBAND_OVERVIEW_UNITS
		});
		global::PandoraSingleton<global::HideoutTabManager>.Instance.ActivateRightTabModules(false, new global::ModuleId[]
		{
			global::ModuleId.TREASURY
		});
		global::PandoraSingleton<global::HideoutTabManager>.Instance.ActivateCenterTabModules(new global::ModuleId[]
		{
			global::ModuleId.TITLE,
			global::ModuleId.ACTION_BUTTON
		});
		global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleCenter<global::TitleModule>(global::ModuleId.TITLE).Set("menu_mission_report", true);
		global::WarbandSheetModule moduleLeft = global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleLeft<global::WarbandSheetModule>(global::ModuleId.WARBAND_SHEET);
		moduleLeft.Set(global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband);
		global::WarbandOverviewUnitsModule moduleLeft2 = global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleLeft<global::WarbandOverviewUnitsModule>(global::ModuleId.WARBAND_OVERVIEW_UNITS);
		moduleLeft2.Set(global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband);
		global::ActionButtonModule moduleCenter = global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleCenter<global::ActionButtonModule>(global::ModuleId.ACTION_BUTTON);
		moduleCenter.Refresh(string.Empty, string.Empty, "menu_continue", new global::UnityEngine.Events.UnityAction(this.Advance));
		moduleCenter.SetFocus();
		global::PandoraSingleton<global::HideoutTabManager>.Instance.ActivateLeftTabModule(true, new global::ModuleId[]
		{
			global::ModuleId.SLIDE_XP
		});
		this.xpModule = global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleLeft<global::EndGameXPModule>(global::ModuleId.SLIDE_XP);
		this.xpModule.Set(global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband);
		for (int i = 0; i < global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.unitCtrlrs.Count; i++)
		{
			global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.unitCtrlrs[i].Hide(false, false, null);
		}
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button1.gameObject.SetActive(false);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button2.gameObject.SetActive(false);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button3.gameObject.SetActive(false);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button4.gameObject.SetActive(false);
	}

	public void Exit(int iTo)
	{
		for (int i = 0; i < global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.unitCtrlrs.Count; i++)
		{
			global::UnitMenuController unitMenuController = global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.unitCtrlrs[i];
			unitMenuController.Hide(false, false, null);
		}
		global::PandoraSingleton<global::HideoutManager>.Instance.warbandNodeGroup.Deactivate();
		global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.GetWarbandSave().inMission = false;
		global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.GetWarbandSave().endMission = null;
		global::PandoraSingleton<global::HideoutManager>.Instance.SaveChanges();
	}

	public void FixedUpdate()
	{
	}

	private void Advance()
	{
		this.advance = true;
	}

	private void ShowItems()
	{
		global::PandoraSingleton<global::HideoutTabManager>.Instance.ActivateLeftTabModule(false, new global::ModuleId[]
		{
			global::ModuleId.SLIDE_XP
		});
		global::PandoraSingleton<global::HideoutTabManager>.Instance.ActivateLeftTabModule(true, new global::ModuleId[]
		{
			global::ModuleId.SLIDE_OOA
		});
		this.ooaModule = global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleLeft<global::EndGameOoaModule>(global::ModuleId.SLIDE_OOA);
		this.ooaModule.Set(global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.GetWarbandSave().endMission.wagonItems);
	}

	private void GoToCamp()
	{
		this.goingToCamp = true;
		global::PandoraSingleton<global::PandoraInput>.Instance.SetActive(false);
		global::PandoraSingleton<global::GameManager>.Instance.StartCoroutine(this.GoingToCamp());
	}

	private global::System.Collections.IEnumerator GoingToCamp()
	{
		yield return null;
		yield return null;
		global::PandoraSingleton<global::HideoutManager>.Instance.StateMachine.ChangeState(0);
		global::PandoraSingleton<global::PandoraInput>.Instance.SetActive(true);
		yield break;
	}

	private global::HideoutCamAnchor camAnchor;

	private global::EndGameXPModule xpModule;

	private global::EndGameOoaModule ooaModule;

	private bool advance;

	private bool goingToCamp;
}
