using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerTasksSkillsModule : global::UIModule
{
	private void Start()
	{
		global::System.Collections.Generic.List<global::WarbandSkillData> list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::WarbandSkillData>("fk_warband_skill_type_id", 3.ToString());
		this.warbandSkills = new global::System.Collections.Generic.List<global::WarbandSkill>();
		for (int i = 0; i < list.Count; i++)
		{
			this.warbandSkills.Add(new global::WarbandSkill(list[i].Id));
		}
		this.warbandSkills.Sort((global::WarbandSkill x, global::WarbandSkill y) => x.LocalizedName.CompareTo(y.LocalizedName));
	}

	public void Set(global::UnityEngine.Events.UnityAction<global::PlayerProgressTab> tabChanged, global::UnityEngine.Events.UnityAction<global::AchievementCategory> taskSelected, global::UnityEngine.Events.UnityAction<global::WarbandSkill> skillSelected, global::UnityEngine.Events.UnityAction<global::WarbandSkill> skillConfirmed)
	{
		this.onTabChanged = tabChanged;
		this.onTaskSelected = taskSelected;
		this.onSkillSelected = skillSelected;
		this.onSkillConfirmed = skillConfirmed;
		this.tabs.gameObject.SetActive(true);
		this.tabs.Setup(new global::System.Action<int>(this.OnTabChanged), (int)this.currentTab);
		int num = 0;
		int num2 = 0;
		global::AchievementCategory[] achievementsCategories = global::PandoraSingleton<global::GameManager>.Instance.Profile.AchievementsCategories;
		for (int i = 0; i < achievementsCategories.Length; i++)
		{
			if (achievementsCategories[i] != null)
			{
				num += achievementsCategories[i].Count;
				num2 += achievementsCategories[i].NbDone;
			}
		}
		this.tasksCompletion.text = num2.ToString() + "/" + num.ToString();
	}

	private void OnTabChanged(int tab)
	{
		this.SetTab((global::PlayerProgressTab)tab, true);
	}

	public void SetTab(global::PlayerProgressTab tab, bool callBack)
	{
		if (this.currentTab != tab || this.list.items.Count == 0)
		{
			this.currentTab = tab;
			global::PlayerProgressTab playerProgressTab = this.currentTab;
			if (playerProgressTab != global::PlayerProgressTab.TASKS)
			{
				if (playerProgressTab == global::PlayerProgressTab.SKILLS)
				{
					this.tabs.tabs[1].SetOn();
					this.SetSkillsList(true);
				}
			}
			else
			{
				this.tabs.tabs[0].SetOn();
				this.SetTasksList();
			}
		}
		if (callBack && this.onTabChanged != null)
		{
			this.onTabChanged(this.currentTab);
		}
	}

	public void SetCurrentSkill(global::WarbandSkill skill)
	{
		this.currentSkill = skill;
	}

	private void SetTasksList()
	{
		this.points.SetActive(false);
		this.list.ClearList();
		this.list.Setup(this.taskPrefab, true);
		global::System.Collections.Generic.List<global::AchievementCategory> list = global::PandoraSingleton<global::GameManager>.Instance.Profile.AchievementsCategories.Remove(new global::AchievementCategory[1]).Sorted((global::AchievementCategory x, global::AchievementCategory y) => x.LocName.CompareTo(y.LocName)).ToDynList<global::AchievementCategory>();
		for (int i = 0; i < list.Count; i++)
		{
			global::UnityEngine.GameObject gameObject = this.list.AddToList(null, null);
			global::UITaskListItem component = gameObject.GetComponent<global::UITaskListItem>();
			component.Set(list[i]);
			global::ToggleEffects component2 = gameObject.GetComponent<global::ToggleEffects>();
			component2.selectOnOver = true;
			global::AchievementCategory task = list[i];
			component2.onSelect.AddListener(delegate()
			{
				this.onTaskSelected(task);
			});
		}
	}

	public void SetSkillsList(bool clearOnly = false)
	{
		this.RefreshAvailablePoints();
		this.list.ClearList();
		if (clearOnly || (this.currentSkill != null && this.currentSkill.IsMastery))
		{
			return;
		}
		this.list.Setup(this.skillPrefab, true);
		for (int i = 0; i < this.warbandSkills.Count; i++)
		{
			if (this.currentSkill != null)
			{
				if (this.warbandSkills[i].Data.WarbandSkillIdPrerequisite == this.currentSkill.Id)
				{
					this.AddSkillToList(this.warbandSkills[i]);
					break;
				}
			}
			else if (this.warbandSkills[i].Data.SkillQualityId == global::SkillQualityId.NORMAL_QUALITY && !global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.HasSkill(this.warbandSkills[i].Data.Id, true))
			{
				this.AddSkillToList(this.warbandSkills[i]);
			}
		}
	}

	public void RefreshAvailablePoints()
	{
		int playerSkillsAvailablePoints = global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.GetPlayerSkillsAvailablePoints();
		if (playerSkillsAvailablePoints > 0)
		{
			this.points.SetActive(true);
			this.unspentPoints.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_unspent_skill_point", new string[]
			{
				playerSkillsAvailablePoints.ToString()
			});
		}
		else
		{
			this.points.SetActive(false);
		}
	}

	private void AddSkillToList(global::WarbandSkill data)
	{
		global::UnityEngine.GameObject gameObject = this.list.AddToList(null, null);
		global::UISkillItem component = gameObject.GetComponent<global::UISkillItem>();
		component.Set(data);
		global::ToggleEffects component2 = gameObject.GetComponent<global::ToggleEffects>();
		component2.onSelect.AddListener(delegate()
		{
			this.onSkillSelected(data);
		});
		component2.onAction.AddListener(delegate()
		{
			this.onSkillConfirmed(data);
		});
	}

	public void SetFocus(bool focus)
	{
		this.isFocused = focus;
		if (focus && global::PandoraSingleton<global::PandoraInput>.Instance.CurrentInputLayer == 0)
		{
			this.list.items[0].SetSelected(true);
		}
	}

	private void Update()
	{
		if (this.isFocused)
		{
			if (global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp("h", 0) && this.tabs.currentTab != 0)
			{
				this.tabs.Next();
			}
			else if (global::PandoraSingleton<global::PandoraInput>.Instance.GetNegKeyUp("h", 0))
			{
				this.tabs.Prev();
			}
			float axis = global::PandoraSingleton<global::PandoraInput>.Instance.GetAxis("cam_y", 0);
			if (axis != 0f)
			{
				this.list.ForceScroll(axis < 0f, false);
			}
		}
	}

	public global::ListTabsModule tabs;

	public global::UnityEngine.GameObject points;

	public global::UnityEngine.UI.Text unspentPoints;

	public global::ScrollGroup list;

	public global::UnityEngine.GameObject taskPrefab;

	public global::UnityEngine.GameObject skillPrefab;

	public global::UnityEngine.UI.Text tasksCompletion;

	private global::PlayerProgressTab currentTab;

	private bool isFocused;

	private global::UnityEngine.Events.UnityAction<global::PlayerProgressTab> onTabChanged;

	private global::UnityEngine.Events.UnityAction<global::AchievementCategory> onTaskSelected;

	private global::UnityEngine.Events.UnityAction<global::WarbandSkill> onSkillSelected;

	private global::UnityEngine.Events.UnityAction<global::WarbandSkill> onSkillConfirmed;

	private global::WarbandSkill currentSkill;

	private global::System.Collections.Generic.List<global::WarbandSkill> warbandSkills;
}
