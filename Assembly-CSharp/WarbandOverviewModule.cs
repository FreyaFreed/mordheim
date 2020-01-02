using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarbandOverviewModule : global::UIModule
{
	private new void Awake()
	{
		for (int i = 0; i < this.tabs.Length; i++)
		{
			int idx = i;
			this.tabs[i].onSelect.AddListener(delegate()
			{
				this.ShowPanel(idx);
			});
		}
	}

	public void ShowPanel(int panelIdx)
	{
		if (this.tabIdx != panelIdx)
		{
			this.tabIdx = panelIdx;
			for (int i = 0; i < this.panels.Length; i++)
			{
				if (i == panelIdx)
				{
					this.tabs[i].SetOn();
				}
				this.panels[i].SetActive(i == panelIdx);
			}
		}
	}

	public void Set(global::Warband wb)
	{
		this.descriptionText.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("com_wb_desc_" + wb.Id.ToLowerString());
		this.SetHistoryPanel(wb);
		this.SetStatsPanel(wb);
	}

	private void SetHistoryPanel(global::Warband wb)
	{
		this.historyScrollgroup.ClearList();
		global::System.Collections.Generic.List<global::Tuple<global::Tuple<int, global::EventLogger.LogEvent, int>, global::UnitStatSave>> list = new global::System.Collections.Generic.List<global::Tuple<global::Tuple<int, global::EventLogger.LogEvent, int>, global::UnitStatSave>>();
		for (int i = 0; i < wb.Logger.history.Count; i++)
		{
			list.Add(new global::Tuple<global::Tuple<int, global::EventLogger.LogEvent, int>, global::UnitStatSave>(wb.Logger.history[i], null));
		}
		for (int j = 0; j < wb.GetWarbandSave().oldUnits.Count; j++)
		{
			for (int k = 0; k < wb.GetWarbandSave().oldUnits[j].history.Count; k++)
			{
				list.Add(new global::Tuple<global::Tuple<int, global::EventLogger.LogEvent, int>, global::UnitStatSave>(wb.GetWarbandSave().oldUnits[j].history[k], wb.GetWarbandSave().oldUnits[j]));
			}
		}
		for (int l = 0; l < wb.Units.Count; l++)
		{
			for (int m = 0; m < wb.Units[l].UnitSave.stats.history.Count; m++)
			{
				list.Add(new global::Tuple<global::Tuple<int, global::EventLogger.LogEvent, int>, global::UnitStatSave>(wb.Units[l].UnitSave.stats.history[m], wb.Units[l].UnitSave.stats));
			}
		}
		list.Sort((global::Tuple<global::Tuple<int, global::EventLogger.LogEvent, int>, global::UnitStatSave> a, global::Tuple<global::Tuple<int, global::EventLogger.LogEvent, int>, global::UnitStatSave> b) => a.Item1.Item1.CompareTo(b.Item1.Item1));
		global::MonthId monthId = global::MonthId.MAX_VALUE;
		for (int n = list.Count - 1; n >= 0; n--)
		{
			if (list[n].Item1.Item1 <= global::PandoraSingleton<global::HideoutManager>.Instance.Date.CurrentDate)
			{
				global::Date date = new global::Date(list[n].Item1.Item1);
				if (date.Month != monthId)
				{
					this.historyScrollgroup.Setup(this.historyTitleTemplate, true);
					monthId = date.Month;
					global::UnityEngine.GameObject gameObject = this.historyScrollgroup.AddToList(null, null);
					if (monthId == global::MonthId.NONE)
					{
						gameObject.GetComponentsInChildren<global::UnityEngine.UI.Text>(true)[0].text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("calendar_holiday_" + date.Holiday.ToLowerString());
					}
					else
					{
						gameObject.GetComponentsInChildren<global::UnityEngine.UI.Text>(true)[0].text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("calendar_month_" + monthId.ToLowerString());
					}
					this.historyScrollgroup.Setup(this.historyEntryTemplate, true);
				}
				string text = string.Empty;
				global::EventLogger.LogEvent item = list[n].Item1.Item2;
				switch (item)
				{
				case global::EventLogger.LogEvent.HIRE:
				{
					global::UnitStatSave item2 = list[n].Item2;
					string stringById = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("unit_name_" + ((global::UnitId)item2.id).ToLowerString());
					text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_history_hire", new string[]
					{
						stringById,
						item2.Name
					});
					break;
				}
				case global::EventLogger.LogEvent.FIRE:
				{
					global::UnitStatSave item3 = list[n].Item2;
					string stringById2 = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("unit_name_" + ((global::UnitId)item3.id).ToLowerString());
					text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_history_fire", new string[]
					{
						stringById2,
						item3.Name
					});
					break;
				}
				case global::EventLogger.LogEvent.DEATH:
				{
					global::UnitStatSave item4 = list[n].Item2;
					text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_history_death", new string[]
					{
						item4.Name
					});
					break;
				}
				case global::EventLogger.LogEvent.RETIREMENT:
				{
					global::UnitStatSave item5 = list[n].Item2;
					text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_history_retire", new string[]
					{
						item5.Name
					});
					break;
				}
				case global::EventLogger.LogEvent.LEFT:
				{
					global::UnitStatSave item6 = list[n].Item2;
					text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_history_left", new string[]
					{
						item6.Name
					});
					break;
				}
				default:
					switch (item)
					{
					case global::EventLogger.LogEvent.WARBAND_CREATED:
						text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_history_warband_created");
						break;
					case global::EventLogger.LogEvent.RANK_ACHIEVED:
					{
						global::UnitStatSave item7 = list[n].Item2;
						if (item7 == null)
						{
							text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_history_warband_rank", new string[]
							{
								list[n].Item1.Item3.ToString()
							});
						}
						break;
					}
					case global::EventLogger.LogEvent.MEMORABLE_CAMPAIGN_VICTORY:
						text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_history_campaign_victory_total", new string[]
						{
							list[n].Item1.Item3.ToString()
						});
						break;
					case global::EventLogger.LogEvent.VICTORY_STREAK:
						text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_history_victory_streak", new string[]
						{
							list[n].Item1.Item3.ToString()
						});
						break;
					}
					break;
				}
				if (!string.IsNullOrEmpty(text))
				{
					global::WarbandHistoryItem component = this.historyScrollgroup.AddToList(null, null).GetComponent<global::WarbandHistoryItem>();
					if (date.Day == 0)
					{
						component.date.text = string.Empty;
					}
					else
					{
						component.date.text = date.Day.ToString();
					}
					component.eventDesc.text = text;
				}
			}
		}
	}

	private void SetStatsPanel(global::Warband wb)
	{
		global::Date date = new global::Date(global::Constant.GetInt(global::ConstantId.CAL_DAY_START));
		this.warbandCreateDate.text = date.ToLocalizedAbbrString();
		this.warbandDaysActive.text = (global::PandoraSingleton<global::HideoutManager>.Instance.Date.CurrentDate - global::Constant.GetInt(global::ConstantId.CAL_DAY_START)).ToString();
		int attribute = wb.GetAttribute(global::WarbandAttributeId.CAMPAIGN_MISSION_ATTEMPTED);
		this.missionsAttempted.text = attribute.ToString();
		int num = wb.GetAttribute(global::WarbandAttributeId.CAMPAIGN_MISSION_WIN);
		this.missionSuccessRate.text = ((float)num / (float)global::UnityEngine.Mathf.Max(1, attribute)).ToString("00%");
		this.missionCrushingVictories.text = wb.GetAttribute(global::WarbandAttributeId.CAMPAIGN_MISSION_CRUSHED_VICTORY).ToString();
		this.missionTotalVictories.text = wb.GetAttribute(global::WarbandAttributeId.CAMPAIGN_MISSION_TOTAL_VICTORY).ToString();
		attribute = wb.GetAttribute(global::WarbandAttributeId.SKIRMISH_ATTEMPTED);
		this.skirmishesAttempted.text = attribute.ToString();
		num = attribute - wb.GetAttribute(global::WarbandAttributeId.SKIRMISH_LOST);
		this.skirmishSuccessRate.text = ((float)num / (float)global::UnityEngine.Mathf.Max(1, attribute)).ToString("00%");
		this.skirmishDecisiveVictories.text = wb.GetAttribute(global::WarbandAttributeId.SKIRMISH_DECISIVE_VICTORY).ToString();
		this.skirmishObjectiveVictories.text = wb.GetAttribute(global::WarbandAttributeId.SKIRMISH_OBJECTIVE_VICTORY).ToString();
		this.skirmishBattlegroundVictories.text = wb.GetAttribute(global::WarbandAttributeId.SKIRMISH_BATTLEGROUND_VICTORY).ToString();
		int attribute2 = wb.GetAttribute(global::WarbandAttributeId.TOTAL_OOA);
		this.ooaAllies.text = attribute2.ToString();
		int attribute3 = wb.GetAttribute(global::WarbandAttributeId.TOTAL_KILL);
		this.ooaEnemies.text = attribute3.ToString();
		this.outOfActionRatio.text = ((float)attribute3 / (float)global::UnityEngine.Mathf.Max(1, attribute2)).ToString("00%");
		this.damageDealt.text = wb.GetAttribute(global::WarbandAttributeId.TOTAL_DAMAGE).ToString();
		this.allTimeGold.text = wb.GetAttribute(global::WarbandAttributeId.TOTAL_GOLD).ToString();
		this.allTimeWyrdFragments.text = wb.GetAttribute(global::WarbandAttributeId.FRAGMENTS_GATHERED).ToString();
		this.allTimeWyrdShards.text = wb.GetAttribute(global::WarbandAttributeId.SHARDS_GATHERED).ToString();
		this.allTimeWyrdClusters.text = wb.GetAttribute(global::WarbandAttributeId.CLUSTERS_GATHERED).ToString();
	}

	public void SetNav(global::UnityEngine.UI.Selectable left)
	{
		global::UnityEngine.UI.Navigation navigation = this.tabs[0].toggle.navigation;
		navigation.selectOnLeft = left;
		this.tabs[0].toggle.navigation = navigation;
	}

	public void Update()
	{
		if (this.tabIdx == 1)
		{
			float axis = global::PandoraSingleton<global::PandoraInput>.Instance.GetAxis("v", 0);
			if (!global::UnityEngine.Mathf.Approximately(axis, 0f))
			{
				this.historyScrollgroup.ForceScroll(axis < 0f, false);
			}
		}
	}

	[global::UnityEngine.Header("Panels")]
	public global::ToggleEffects[] tabs;

	public global::UnityEngine.GameObject[] panels;

	[global::UnityEngine.Header("Description")]
	public global::UnityEngine.UI.Text descriptionText;

	[global::UnityEngine.Header("History")]
	public global::UnityEngine.GameObject historyTitleTemplate;

	public global::UnityEngine.GameObject historyEntryTemplate;

	public global::ScrollGroup historyScrollgroup;

	[global::UnityEngine.Header("Stats")]
	public global::UnityEngine.UI.Text warbandCreateDate;

	public global::UnityEngine.UI.Text warbandDaysActive;

	public global::UnityEngine.UI.Text missionsAttempted;

	public global::UnityEngine.UI.Text missionSuccessRate;

	public global::UnityEngine.UI.Text missionCrushingVictories;

	public global::UnityEngine.UI.Text missionTotalVictories;

	public global::UnityEngine.UI.Text skirmishesAttempted;

	public global::UnityEngine.UI.Text skirmishSuccessRate;

	public global::UnityEngine.UI.Text skirmishDecisiveVictories;

	public global::UnityEngine.UI.Text skirmishObjectiveVictories;

	public global::UnityEngine.UI.Text skirmishBattlegroundVictories;

	public global::UnityEngine.UI.Text ooaAllies;

	public global::UnityEngine.UI.Text ooaEnemies;

	public global::UnityEngine.UI.Text outOfActionRatio;

	public global::UnityEngine.UI.Text damageDealt;

	public global::UnityEngine.UI.Text allTimeGold;

	public global::UnityEngine.UI.Text allTimeWyrdFragments;

	public global::UnityEngine.UI.Text allTimeWyrdShards;

	public global::UnityEngine.UI.Text allTimeWyrdClusters;

	private int tabIdx = -1;
}
