using System;
using System.Collections;
using UnityEngine.Events;

public class HideoutPlayerProgression : global::ICheapState
{
	public HideoutPlayerProgression(global::HideoutManager mng, global::HideoutCamAnchor anchor)
	{
		this.camAnchor = anchor;
	}

	public void Destroy()
	{
	}

	public void Enter(int iFrom)
	{
		this.warband = global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband;
		this.currentWheelIndex = 0;
		global::PandoraSingleton<global::HideoutManager>.Instance.CamManager.dummyCam.transform.position = this.camAnchor.transform.position;
		global::PandoraSingleton<global::HideoutManager>.Instance.CamManager.dummyCam.transform.rotation = this.camAnchor.transform.rotation;
		global::PandoraSingleton<global::HideoutManager>.Instance.CamManager.SetDOFTarget(this.camAnchor.dofTarget, 0f);
		global::UnitMenuController dramatis = global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.GetDramatis();
		global::PandoraSingleton<global::HideoutManager>.Instance.progressNode.SetContent(dramatis, null);
		global::PandoraSingleton<global::HideoutManager>.Instance.CamManager.SetDOFTarget(global::PandoraSingleton<global::HideoutManager>.Instance.progressNode.transform, 1.25f);
		dramatis.SwitchWeapons(global::UnitSlotId.SET1_MAINHAND);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.ActivateCenterTabModules(new global::ModuleId[]
		{
			global::ModuleId.WHEEL_WARBAND_SKILLS,
			global::ModuleId.WARBAND_TABS,
			global::ModuleId.TITLE,
			global::ModuleId.NOTIFICATION
		});
		global::PandoraSingleton<global::HideoutTabManager>.Instance.ActivateLeftTabModules(true, new global::ModuleId[]
		{
			global::ModuleId.PLAYER_SHEET,
			global::ModuleId.PLAYER_RANK_DESC
		});
		global::PandoraSingleton<global::HideoutTabManager>.Instance.ActivateRightTabModules(true, new global::ModuleId[]
		{
			global::ModuleId.TREASURY,
			global::ModuleId.TASK_LIST
		});
		this.warbandTabs = global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleCenter<global::WarbandTabsModule>(global::ModuleId.WARBAND_TABS);
		this.warbandTabs.Setup(global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleCenter<global::TitleModule>(global::ModuleId.TITLE));
		this.warbandTabs.SetCurrentTab(global::HideoutManager.State.PLAYER_PROGRESSION);
		this.warbandTabs.Refresh();
		global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleLeft<global::PlayerSheetModule>(global::ModuleId.PLAYER_SHEET).Refresh();
		global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleLeft<global::PlayerRankDescModule>(global::ModuleId.PLAYER_RANK_DESC).Refresh();
		this.taskDesc = global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleCenter<global::PlayerTaskDescModule>(global::ModuleId.TASK_DESC);
		this.skillWheel = global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleCenter<global::WarbandSkillsWheelModule>(global::ModuleId.WHEEL_WARBAND_SKILLS);
		this.skillDesc = global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleCenter<global::WarbandSkillDescModule>(global::ModuleId.WARBAND_SKILL_DESC);
		this.taskList = global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleRight<global::PlayerTasksSkillsModule>(global::ModuleId.TASK_LIST);
		this.taskList.Set(new global::UnityEngine.Events.UnityAction<global::PlayerProgressTab>(this.OnTabChanged), new global::UnityEngine.Events.UnityAction<global::AchievementCategory>(this.OnTaskSelected), new global::UnityEngine.Events.UnityAction<global::WarbandSkill>(this.OnSkillSelected), new global::UnityEngine.Events.UnityAction<global::WarbandSkill>(this.OnSkillConfirmed));
		this.taskList.tabs.tabs[0].SetOn();
		this.taskList.SetTab(global::PlayerProgressTab.TASKS, true);
		this.once = true;
	}

	private void SetButtonsForPlayerTask()
	{
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button1.gameObject.SetActive(true);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button1.SetAction("cancel", "menu_back", 0, false, global::PandoraSingleton<global::HideoutTabManager>.Instance.icnBack, null);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button1.OnAction(delegate
		{
			global::PandoraSingleton<global::HideoutManager>.Instance.StateMachine.ChangeState(0);
		}, false, true);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button2.gameObject.SetActive(false);
		this.SetLastButtons();
	}

	private void SetButtonsForWheelSelection()
	{
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button1.gameObject.SetActive(true);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button1.SetAction("cancel", "menu_back", 0, false, global::PandoraSingleton<global::HideoutTabManager>.Instance.icnBack, null);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button1.OnAction(delegate
		{
			global::PandoraSingleton<global::HideoutManager>.Instance.StateMachine.ChangeState(0);
		}, false, true);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button2.gameObject.SetActive(false);
		this.SetLastButtons();
	}

	private void SetButtonsForSkillSelection()
	{
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button1.SetAction("cancel", "menu_return_select_slot", 0, false, global::PandoraSingleton<global::HideoutTabManager>.Instance.icnBack, null);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button1.OnAction(new global::UnityEngine.Events.UnityAction(this.ReturnSelectSlot), false, true);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button2.gameObject.SetActive(false);
		this.SetLastButtons();
	}

	private void SetLastButtons()
	{
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button3.gameObject.SetActive(true);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button3.SetAction("show_chat", "menu_respec", 0, false, null, null);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button3.OnAction(new global::UnityEngine.Events.UnityAction(this.DisplayRespecPopup), false, true);
		this.RefreshRespecButton();
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button4.gameObject.SetActive(false);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button5.gameObject.SetActive(false);
	}

	public void ReturnSelectSlot()
	{
		this.taskList.list.ClearList();
		this.taskList.RefreshAvailablePoints();
		this.skillWheel.slots[this.currentWheelIndex].SetSelected(true);
	}

	private void RefreshRespecButton()
	{
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button3.label.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("menu_respec", new string[]
		{
			this.warband.GetWarbandSave().availaibleRespec.ToString()
		});
	}

	public void Exit(int iTo)
	{
	}

	public void Update()
	{
	}

	public void FixedUpdate()
	{
		if (this.once)
		{
			this.once = false;
			global::PandoraSingleton<global::HideoutManager>.Instance.ShowHideoutTuto(global::HideoutManager.HideoutTutoType.PROGRESSION);
		}
	}

	private void DisplayRespecPopup()
	{
		int availaibleRespec = this.warband.GetWarbandSave().availaibleRespec;
		int @int = global::Constant.GetInt(global::ConstantId.CAL_DAYS_PER_YEAR);
		int num = this.warband.GetWarbandSave().currentDate / @int;
		int num2 = @int - (this.warband.GetWarbandSave().currentDate - num * @int);
		if (this.warband.GetPlayerSkills().Count == 0)
		{
			global::PandoraSingleton<global::HideoutManager>.Instance.messagePopup.ShowLocalized(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("menu_respec_title"), global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("menu_respec_no_skills_desc", new string[]
			{
				num2.ToString()
			}), null, false, true);
		}
		else if (availaibleRespec == 0)
		{
			global::PandoraSingleton<global::HideoutManager>.Instance.messagePopup.ShowLocalized(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("menu_respec_none_title"), global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("menu_respec_none_desc", new string[]
			{
				num2.ToString()
			}), null, false, true);
		}
		else
		{
			global::PandoraSingleton<global::HideoutManager>.Instance.messagePopup.ShowLocalized(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("menu_respec_title"), global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("menu_respec_desc", new string[]
			{
				num2.ToString()
			}), new global::System.Action<bool>(this.Respec), false, false);
		}
	}

	private void Respec(bool confirm)
	{
		if (!confirm)
		{
			return;
		}
		this.warband.ResetPlayerSkills();
		global::PandoraSingleton<global::HideoutManager>.Instance.StartCoroutine(this.NextFrameReturn());
		this.RefreshRespecButton();
		global::PandoraSingleton<global::HideoutManager>.Instance.SaveChanges();
	}

	private global::System.Collections.IEnumerator NextFrameReturn()
	{
		yield return null;
		if (this.skillWheel.gameObject.activeInHierarchy)
		{
			this.skillWheel.SetSelected(true);
		}
		yield return null;
		this.OnTabChanged((!this.skillWheel.gameObject.activeInHierarchy) ? global::PlayerProgressTab.TASKS : global::PlayerProgressTab.SKILLS);
		if (this.skillWheel.gameObject.activeInHierarchy)
		{
			this.currentWheelIndex = 0;
			this.ReturnSelectSlot();
		}
		yield break;
	}

	private void OnTabChanged(global::PlayerProgressTab tab)
	{
		if (tab != global::PlayerProgressTab.TASKS)
		{
			if (tab == global::PlayerProgressTab.SKILLS)
			{
				this.taskDesc.gameObject.SetActive(false);
				this.skillWheel.Set(new global::UnityEngine.Events.UnityAction<int, global::WarbandSkill>(this.OnWheelSkillSelected), new global::UnityEngine.Events.UnityAction(this.OnWheelSkillConfirmed));
				this.skillWheel.slots[0].SetSelected(false);
			}
		}
		else
		{
			this.skillWheel.gameObject.SetActive(false);
			this.skillDesc.gameObject.SetActive(false);
			this.taskList.SetFocus(true);
			this.SetButtonsForPlayerTask();
		}
	}

	private void OnTaskSelected(global::AchievementCategory task)
	{
		this.taskDesc.Set(task);
	}

	private void OnWheelSkillSelected(int idx, global::WarbandSkill skill)
	{
		this.taskList.SetCurrentSkill(skill);
		this.taskList.SetFocus(false);
		this.skillDesc.Set(idx, skill);
		this.currentWheelIndex = idx;
		this.SetButtonsForWheelSelection();
	}

	private void OnWheelSkillConfirmed()
	{
		this.taskList.SetSkillsList(false);
		if (this.taskList.list.items.Count > 0)
		{
			this.taskList.SetFocus(true);
		}
	}

	private void OnSkillSelected(global::WarbandSkill skill)
	{
		this.currentSkill = skill;
		this.skillDesc.Set(0, skill);
		this.SetButtonsForSkillSelection();
	}

	private void OnSkillConfirmed(global::WarbandSkill skill)
	{
		if (skill.Data.Points <= this.warband.GetPlayerSkillsAvailablePoints())
		{
			this.currentSkill = skill;
			global::PandoraSingleton<global::HideoutManager>.Instance.messagePopup.ShowLocalized(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_warband_skill_confirm_title"), global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_warband_skill_confirm_desc", new string[]
			{
				skill.LocalizedName
			}), new global::System.Action<bool>(this.OnSkillPopupConfirmed), false, false);
		}
	}

	private void OnSkillPopupConfirmed(bool isConfirm)
	{
		if (isConfirm)
		{
			this.warband.LearnSkill(this.currentSkill);
			this.taskList.SetTab(global::PlayerProgressTab.SKILLS, true);
			this.ReturnSelectSlot();
			global::PandoraSingleton<global::HideoutManager>.Instance.SaveChanges();
		}
	}

	private global::HideoutCamAnchor camAnchor;

	private global::PlayerTasksSkillsModule taskList;

	private global::PlayerTaskDescModule taskDesc;

	private global::WarbandSkillsWheelModule skillWheel;

	private global::WarbandSkillDescModule skillDesc;

	private global::Warband warband;

	private global::WarbandSkill currentSkill;

	private int currentWheelIndex;

	private global::WarbandTabsModule warbandTabs;

	private bool once = true;
}
