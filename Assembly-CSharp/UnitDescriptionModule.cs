using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UnitDescriptionModule : global::UIModule
{
	public override void Init()
	{
		base.Init();
		this.listGroup.Setup(this.perkItem, true);
		this.selectable = base.GetComponent<global::UnityEngine.UI.Selectable>();
		for (int i = 0; i < this.tabs.Count; i++)
		{
			global::UnitDescriptionModuleTab tab = (global::UnitDescriptionModuleTab)i;
			this.tabs[i].onAction.AddListener(delegate()
			{
				this.SetTab(tab);
			});
			this.tabs[i].onSelect.AddListener(delegate()
			{
				this.SetTab(tab);
			});
		}
		this.tabChangedThisFrame = false;
	}

	public void SetTab(global::UnitDescriptionModuleTab tab)
	{
		if (this.currentTab == tab)
		{
			return;
		}
		this.currentTab = tab;
		switch (tab)
		{
		case global::UnitDescriptionModuleTab.DESCRIPTION:
			this.unitDescriptionGroup.SetActive(true);
			this.listTitleGroup.SetActive(true);
			this.perksSection.SetActive(true);
			this.listGroupInjuryHistory.gameObject.SetActive(false);
			this.description.text = this.currentUnit.LocalizedDescription;
			break;
		case global::UnitDescriptionModuleTab.INJURIES_AND_MUTATIONS:
			this.unitDescriptionGroup.SetActive(false);
			this.perksSection.SetActive(false);
			this.listTitleGroup.SetActive(false);
			this.listGroupInjuryHistory.gameObject.SetActive(true);
			this.listGroupInjuryHistory.Setup(this.mutationItem, true);
			this.listGroupInjuryHistory.ClearList();
			for (int i = 0; i < this.currentUnit.Mutations.Count; i++)
			{
				global::UnityEngine.GameObject gameObject = this.listGroupInjuryHistory.AddToList(null, null);
				global::InjuryMutationItem component = gameObject.GetComponent<global::InjuryMutationItem>();
				component.Set(this.currentUnit.Mutations[i]);
			}
			for (int j = 0; j < this.currentUnit.Injuries.Count; j++)
			{
				if (this.currentUnit.Injuries[j].Data.Id != global::InjuryId.LIGHT_WOUND)
				{
					global::UnityEngine.GameObject gameObject2 = this.listGroupInjuryHistory.AddToList(null, null);
					global::InjuryMutationItem component2 = gameObject2.GetComponent<global::InjuryMutationItem>();
					component2.Set(this.currentUnit.Injuries[j]);
				}
			}
			if (this.listGroupInjuryHistory.items.Count == 0)
			{
				global::UnityEngine.GameObject gameObject3 = this.listGroupInjuryHistory.AddToList(null, null);
				global::InjuryMutationItem component3 = gameObject3.GetComponent<global::InjuryMutationItem>();
				component3.Set(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("injury_name_no_injury"), global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("injury_desc_no_injury"), "0");
			}
			break;
		case global::UnitDescriptionModuleTab.HISTORY:
		{
			this.unitDescriptionGroup.SetActive(false);
			this.listTitleGroup.SetActive(false);
			this.perksSection.SetActive(false);
			this.listGroupInjuryHistory.gameObject.SetActive(true);
			this.listGroupInjuryHistory.ClearList();
			global::MonthId monthId = global::MonthId.MAX_VALUE;
			for (int k = this.currentUnit.UnitSave.stats.history.Count - 1; k >= 0; k--)
			{
				if (this.currentUnit.UnitSave.stats.history[k].Item1 <= global::PandoraSingleton<global::HideoutManager>.Instance.Date.CurrentDate)
				{
					global::Date date = new global::Date(this.currentUnit.UnitSave.stats.history[k].Item1);
					if (date.Month != monthId)
					{
						this.listGroupInjuryHistory.Setup(this.historyTitleItem, true);
						monthId = date.Month;
						global::UnityEngine.GameObject gameObject4 = this.listGroupInjuryHistory.AddToList(null, null);
						if (monthId == global::MonthId.NONE)
						{
							gameObject4.GetComponentsInChildren<global::UnityEngine.UI.Text>(true)[0].text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("calendar_holiday_" + date.Holiday.ToLowerString());
						}
						else
						{
							gameObject4.GetComponentsInChildren<global::UnityEngine.UI.Text>(true)[0].text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("calendar_month_" + monthId.ToLowerString());
						}
						this.listGroupInjuryHistory.Setup(this.historyItem, true);
					}
					string text = string.Empty;
					global::EventLogger.LogEvent item = this.currentUnit.UnitSave.stats.history[k].Item2;
					switch (item)
					{
					case global::EventLogger.LogEvent.INJURY:
					{
						global::InjuryId item2 = (global::InjuryId)this.currentUnit.UnitSave.stats.history[k].Item3;
						if (item2 != global::InjuryId.LIGHT_WOUND && item2 != global::InjuryId.FULL_RECOVERY && item2 != global::InjuryId.NEAR_DEATH && item2 != global::InjuryId.AMNESIA)
						{
							text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_unit_history_injury", new string[]
							{
								global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("injury_name_" + item2)
							});
						}
						break;
					}
					case global::EventLogger.LogEvent.MUTATION:
						text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_unit_history_mutation", new string[]
						{
							global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("mutation_name_" + (global::MutationId)this.currentUnit.UnitSave.stats.history[k].Item3)
						});
						break;
					default:
						switch (item)
						{
						case global::EventLogger.LogEvent.RANK_ACHIEVED:
							text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_unit_history_rank_up", new string[]
							{
								this.currentUnit.UnitSave.stats.history[k].Item3.ToString()
							});
							break;
						default:
							if (item == global::EventLogger.LogEvent.HIRE)
							{
								text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_unit_history_hire");
							}
							break;
						case global::EventLogger.LogEvent.MEMORABLE_KILL:
							text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_unit_history_kills", new string[]
							{
								this.currentUnit.UnitSave.stats.history[k].Item3.ToString()
							});
							break;
						}
						break;
					case global::EventLogger.LogEvent.SKILL:
					case global::EventLogger.LogEvent.SPELL:
						text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_unit_history_training", new string[]
						{
							global::SkillHelper.GetLocalizedName((global::SkillId)this.currentUnit.UnitSave.stats.history[k].Item3)
						});
						break;
					}
					if (!string.IsNullOrEmpty(text))
					{
						global::WarbandHistoryItem component4 = this.listGroupInjuryHistory.AddToList(null, null).GetComponent<global::WarbandHistoryItem>();
						if (date.Day == 0)
						{
							component4.date.text = string.Empty;
						}
						else
						{
							component4.date.text = date.Day.ToString();
						}
						component4.eventDesc.text = text;
					}
				}
			}
			break;
		}
		}
		this.listGroupInjuryHistory.RealignList(true, 0, true);
		this.tabChangedThisFrame = true;
	}

	public void Refresh(global::Unit unit, bool showCost)
	{
		this.currentUnit = unit;
		this.currentTab = global::UnitDescriptionModuleTab.MAX_VALUE;
		this.SetTab(global::UnitDescriptionModuleTab.DESCRIPTION);
		this.SetPerks();
		if (this.currentUnit.Mutations.Count > 0)
		{
			this.injuriesTabTitle.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_injuries_and_mutations");
		}
		else
		{
			this.injuriesTabTitle.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_injuries");
		}
	}

	private void SetPerks()
	{
		this.listGroup.ClearList();
		this.listGroup.Setup(this.perkItem, true);
		global::System.Collections.Generic.List<global::UnitJoinPerkData> list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::UnitJoinPerkData>("fk_unit_id", this.currentUnit.Data.Id.ToIntString<global::UnitId>());
		for (int i = 0; i < list.Count; i++)
		{
			string str = list[i].PerkId.ToLowerString();
			global::UnityEngine.GameObject gameObject = this.listGroup.AddToList(null, null);
			global::UIDescription component = gameObject.GetComponent<global::UIDescription>();
			component.Set("perk_name_" + str, "perk_desc_" + str);
		}
	}

	private void Update()
	{
		switch (this.currentTab)
		{
		case global::UnitDescriptionModuleTab.DESCRIPTION:
			this.CheckScroll(this.tabs[0], this.listGroup);
			break;
		case global::UnitDescriptionModuleTab.INJURIES_AND_MUTATIONS:
		case global::UnitDescriptionModuleTab.HISTORY:
			this.CheckScroll(this.tabs[(int)this.currentTab], this.listGroupInjuryHistory);
			break;
		}
		this.tabChangedThisFrame = false;
	}

	private void CheckScroll(global::ToggleEffects tab, global::ScrollGroup scrollGroup)
	{
		if (global::UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject == tab.gameObject && !this.tabChangedThisFrame && global::PandoraSingleton<global::PandoraInput>.Instance.CurrentInputLayer == 0)
		{
			float axis = global::PandoraSingleton<global::PandoraInput>.Instance.GetAxis("v", 0);
			if (!global::UnityEngine.Mathf.Approximately(axis, 0f) && global::UnityEngine.Mathf.Abs(axis) > 0.8f)
			{
				scrollGroup.ForceScroll(axis < 0f, false);
			}
		}
	}

	public void SetNav(global::UnityEngine.UI.Selectable left)
	{
		global::UnityEngine.UI.Navigation navigation = this.tabs[0].toggle.navigation;
		navigation.selectOnLeft = left;
		this.tabs[0].toggle.navigation = navigation;
	}

	public global::UnityEngine.UI.Selectable selectable;

	public global::UnityEngine.UI.Text description;

	public global::UnityEngine.UI.Text injuriesTabTitle;

	public global::UnityEngine.GameObject unitDescriptionGroup;

	public global::UnityEngine.GameObject listTitleGroup;

	public global::UnityEngine.GameObject perksSection;

	public global::UnityEngine.GameObject perkItem;

	public global::UnityEngine.GameObject mutationItem;

	public global::UnityEngine.GameObject historyTitleItem;

	public global::UnityEngine.GameObject historyItem;

	public global::ScrollGroup listGroup;

	public global::ScrollGroup listGroupInjuryHistory;

	public global::System.Collections.Generic.List<global::ToggleEffects> tabs;

	private global::Unit currentUnit;

	private global::UnitDescriptionModuleTab currentTab = global::UnitDescriptionModuleTab.MAX_VALUE;

	private bool tabChangedThisFrame;
}
