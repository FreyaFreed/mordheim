using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DailyReportPopup : global::ConfirmationPopupView
{
	public void SetUnitList(global::System.Collections.Generic.List<global::UnitMenuController> units)
	{
		this.unitList = units;
	}

	public override void Show(global::System.Action<bool> callback, bool hideButtons = false, bool hideCancel = false)
	{
		base.Show(new global::System.Action<bool>(this.OnContinue), hideButtons, hideCancel);
		this.onDone = callback;
		this.displayedItems = 0;
		this.overflowUnits.Clear();
		this.overflowWarbands.Clear();
		this.warband.gameObject.SetActive(true);
		this.units.gameObject.SetActive(true);
		global::UnityEngine.Sprite sprite = global::Warband.GetIcon(global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.Id);
		this.icon.sprite = sprite;
		this.date.text = global::PandoraSingleton<global::HideoutManager>.Instance.Date.ToLocalizedString();
		this.holiday.text = global::PandoraSingleton<global::HideoutManager>.Instance.Date.ToLocalizedHoliday();
		int currentDate = global::PandoraSingleton<global::HideoutManager>.Instance.Date.CurrentDate;
		this.units.Setup(string.Empty, this.unitItem);
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		for (int i = 0; i < this.unitList.Count; i++)
		{
			switch (this.unitList[i].unit.GetActiveStatus())
			{
			case global::UnitActiveStatusId.INJURED:
				num++;
				break;
			case global::UnitActiveStatusId.UPKEEP_NOT_PAID:
				num3++;
				break;
			case global::UnitActiveStatusId.IN_TRAINING:
				num2++;
				break;
			case global::UnitActiveStatusId.TREATMENT_NOT_PAID:
				num4++;
				break;
			case global::UnitActiveStatusId.INJURED_AND_UPKEEP_NOT_PAID:
				num++;
				num3++;
				break;
			}
		}
		bool flag = false;
		for (int j = 0; j < this.unitList.Count; j++)
		{
			string name = this.unitList[j].unit.Name;
			global::UnityEngine.Sprite unitIcon = this.unitList[j].unit.GetIcon();
			global::EventLogger logger = this.unitList[j].unit.Logger;
			global::System.Collections.Generic.List<global::Tuple<int, global::EventLogger.LogEvent, int>> eventsFromDay = logger.GetEventsFromDay(currentDate - 1);
			for (int k = 0; k < eventsFromDay.Count; k++)
			{
				global::Tuple<int, global::EventLogger.LogEvent, int> tuple = eventsFromDay[k];
				switch (tuple.Item2)
				{
				case global::EventLogger.LogEvent.DEATH:
					if (tuple.Item1 == currentDate)
					{
						flag = true;
						this.AddUnitMessage(unitIcon, global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_complication_death", new string[]
						{
							name
						}));
					}
					break;
				case global::EventLogger.LogEvent.LEFT:
					if (tuple.Item1 == currentDate)
					{
						flag = true;
						this.AddUnitMessage(unitIcon, global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_warning_departed", new string[]
						{
							name
						}));
					}
					else if (tuple.Item1 == currentDate + 1)
					{
						flag = true;
						this.AddUnitMessage(unitIcon, global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_warning_upkeep", new string[]
						{
							name
						}));
					}
					else if (num3 == 1)
					{
						flag = true;
						this.AddUnitMessage(unitIcon, global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("daily_report_upkeep_single", new string[]
						{
							name,
							this.unitList[j].unit.GetUpkeepOwned().ToString()
						}));
					}
					break;
				case global::EventLogger.LogEvent.RECOVERY:
					if (tuple.Item1 == currentDate)
					{
						flag = true;
						this.AddUnitMessage(unitIcon, global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_recovered", new string[]
						{
							name,
							global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("injury_name_" + (global::InjuryId)tuple.Item3)
						}));
					}
					else if (num == 1)
					{
						flag = true;
						this.AddUnitMessage(unitIcon, global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("daily_report_in_treatment_single", new string[]
						{
							name,
							global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("injury_name_" + (global::InjuryId)tuple.Item3),
							(tuple.Item1 - currentDate).ToString()
						}));
					}
					break;
				case global::EventLogger.LogEvent.NO_TREATMENT:
					if (tuple.Item1 == currentDate + 1)
					{
						flag = true;
						this.AddUnitMessage(unitIcon, global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_complication", new string[]
						{
							name
						}));
					}
					else if (num4 == 1)
					{
						flag = true;
						this.AddUnitMessage(unitIcon, global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("daily_report_awaiting_treatment_single", new string[]
						{
							name,
							global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("injury_name_" + (global::InjuryId)tuple.Item3)
						}));
					}
					break;
				case global::EventLogger.LogEvent.SKILL:
					if (tuple.Item1 == currentDate)
					{
						flag = true;
						global::SkillData skillData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::SkillData>(tuple.Item3);
						string stringById;
						if (skillData.SkillTypeId == global::SkillTypeId.SPELL_ACTION)
						{
							stringById = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_learned_spell", new string[]
							{
								name,
								global::SkillHelper.GetLocalizedName(skillData)
							});
						}
						else
						{
							stringById = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_learned_skill", new string[]
							{
								name,
								global::SkillHelper.GetLocalizedName(skillData)
							});
						}
						this.AddUnitMessage(unitIcon, stringById);
					}
					else if (num2 == 1)
					{
						flag = true;
						this.AddUnitMessage(unitIcon, global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("daily_report_training_single", new string[]
						{
							name,
							global::SkillHelper.GetLocalizedName((global::SkillId)tuple.Item3),
							(tuple.Item1 - currentDate).ToString()
						}));
					}
					break;
				}
			}
		}
		if (num3 > 1)
		{
			flag = true;
			int totalUpkeepOwned = global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.GetTotalUpkeepOwned();
			this.AddUnitMessage(sprite, global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("daily_report_upkeep_multiple", new string[]
			{
				num3.ToString(),
				totalUpkeepOwned.ToString()
			}));
		}
		if (num4 > 1)
		{
			flag = true;
			this.AddUnitMessage(sprite, global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("daily_report_awaiting_treatment_multiple", new string[]
			{
				num4.ToString()
			}));
		}
		if (num > 1)
		{
			flag = true;
			this.AddUnitMessage(sprite, global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("daily_report_in_treatment_multiple", new string[]
			{
				num.ToString()
			}));
		}
		if (num2 > 1)
		{
			flag = true;
			this.AddUnitMessage(sprite, global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("daily_report_training_multiple", new string[]
			{
				num2.ToString()
			}));
		}
		if (!flag)
		{
			this.AddUnitMessage(global::Warband.GetIcon(global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.Id), global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("daily_report_injuries_none"));
		}
		this.warband.Setup(string.Empty, this.warbandItem);
		global::EventLogger logger2 = global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.Logger;
		global::System.Collections.Generic.List<global::Tuple<int, global::EventLogger.LogEvent, int>> eventsAtDay = logger2.GetEventsAtDay(currentDate);
		int l = 0;
		while (l < eventsAtDay.Count)
		{
			switch (eventsAtDay[l].Item2)
			{
			case global::EventLogger.LogEvent.FACTION0_DELIVERY:
			case global::EventLogger.LogEvent.FACTION1_DELIVERY:
			case global::EventLogger.LogEvent.FACTION2_DELIVERY:
				this.ShowDeliveryMessage(eventsAtDay[l]);
				break;
			case global::EventLogger.LogEvent.NEW_MISSION:
				if (global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.GetWarbandSave().curCampaignIdx <= global::Constant.GetInt(global::ConstantId.CAMPAIGN_LAST_MISSION))
				{
					string stringById2 = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.PrimaryFactionController.Faction.Data.Desc + "_name");
					this.AddWarbandMessage(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_mission_request", new string[]
					{
						stringById2
					}));
				}
				break;
			case global::EventLogger.LogEvent.CONTACT_ITEM:
			{
				global::WarbandContactId warbandContactId;
				global::ItemId itemId;
				int num5;
				global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.DecodeContactItemData(eventsAtDay[l].Item3, out warbandContactId, out itemId, out num5);
				if (itemId == global::ItemId.GOLD)
				{
					this.AddWarbandMessage(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("daily_report_contact_gold", new string[]
					{
						"#warband_skill_title_contact_" + warbandContactId,
						num5.ToString()
					}));
				}
				else
				{
					this.AddWarbandMessage(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("daily_report_contact", new string[]
					{
						"#warband_skill_title_contact_" + warbandContactId,
						"#item_quality_" + (global::ItemQualityId)num5,
						global::Item.GetLocalizedName(itemId, (global::ItemQualityId)num5)
					}));
				}
				break;
			}
			case global::EventLogger.LogEvent.MARKET_ROTATION:
				this.AddWarbandMessage(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_refresh_market"));
				global::PandoraSingleton<global::Pan>.Instance.Narrate("market_refresh");
				break;
			case global::EventLogger.LogEvent.OUTSIDER_ROTATION:
				this.AddWarbandMessage(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_refresh_outsider"));
				break;
			case global::EventLogger.LogEvent.RESPEC_POINT:
				this.AddWarbandMessage(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_new_respec_point"));
				break;
			}
			IL_915:
			l++;
			continue;
			goto IL_915;
		}
		this.ShowShipmentMessage(logger2);
		if (this.warband.items.Count == 0 && this.overflowWarbands.Count == 0)
		{
			this.AddWarbandMessage(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("daily_report_warband_none"));
		}
		this.warband.gameObject.SetActive(this.warband.items.Count > 0);
		this.units.gameObject.SetActive(this.units.items.Count > 0);
	}

	private void ShowShipmentMessage(global::EventLogger warLog)
	{
		int currentDate = global::PandoraSingleton<global::HideoutManager>.Instance.Date.CurrentDate;
		global::Tuple<int, global::EventLogger.LogEvent, int> tuple = warLog.FindLastEvent(global::EventLogger.LogEvent.SHIPMENT_LATE);
		global::Tuple<int, global::EventLogger.LogEvent, int> tuple2 = warLog.FindLastEvent(global::EventLogger.LogEvent.SHIPMENT_REQUEST);
		global::Tuple<int, global::EventLogger.LogEvent, int> tuple3 = warLog.FindLastEvent(global::EventLogger.LogEvent.SHIPMENT_DELIVERY);
		string stringById = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.PrimaryFactionController.Faction.Data.Desc + "_name");
		global::Tuple<int, global::EventLogger.LogEvent, int> tuple4 = warLog.FindEventBetween(global::EventLogger.LogEvent.SHIPMENT_LATE, currentDate, currentDate);
		if (tuple2 != null && tuple2.Item1 == currentDate)
		{
			this.AddWarbandMessage(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_shipment_request", new string[]
			{
				stringById
			}));
			global::PandoraSingleton<global::Pan>.Instance.Narrate("shipment_request" + global::PandoraSingleton<global::GameManager>.Instance.LocalTyche.Rand(1, 3));
		}
		else if (tuple4 != null && tuple4.Item1 == currentDate)
		{
			this.AddWarbandMessage(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_shipment_last_day"));
			global::PandoraSingleton<global::Pan>.Instance.Narrate("1_day_left");
		}
		else if (tuple != null && tuple.Item1 + 1 == currentDate)
		{
			this.AddWarbandMessage(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_shipment_late"));
		}
		else if (tuple3 != null && tuple3.Item1 == currentDate)
		{
			this.AddWarbandMessage(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_shipment_received", new string[]
			{
				stringById
			}));
		}
		else if (tuple != null && tuple2 != null && currentDate > tuple2.Item1 && tuple.Item1 > currentDate)
		{
			int num = tuple.Item1 - global::PandoraSingleton<global::HideoutManager>.Instance.Date.CurrentDate;
			this.AddWarbandMessage(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_next_shipment_long", new string[]
			{
				num.ToString()
			}));
			if (num == 3)
			{
				global::PandoraSingleton<global::Pan>.Instance.Narrate("3_days_left");
			}
		}
	}

	private void ShowDeliveryMessage(global::Tuple<int, global::EventLogger.LogEvent, int> factiondelivery)
	{
		string text = null;
		for (int i = 0; i < global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.factionCtrlrs.Count; i++)
		{
			global::FactionMenuController factionMenuController = global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.factionCtrlrs[i];
			if (factionMenuController.Faction.GetFactionDeliveryEvent() == factiondelivery.Item2)
			{
				global::ShipmentSave delivery = factionMenuController.Faction.GetDelivery(factiondelivery.Item3);
				text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(factionMenuController.Faction.Data.Desc + "_name");
				factionMenuController.Faction.ClearDelivery(factiondelivery.Item3);
			}
		}
		this.AddWarbandMessage(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_delivery_received", new string[]
		{
			text
		}));
	}

	private void AddWarbandMessage(string text)
	{
		if (this.displayedItems >= this.maxItemsPerPage)
		{
			this.overflowWarbands.Add(text);
		}
		else
		{
			global::UnityEngine.GameObject gameObject = this.warband.AddToList();
			global::UnityEngine.UI.Text text2 = gameObject.GetComponentsInChildren<global::UnityEngine.UI.Text>(true)[0];
			text2.text = text;
		}
		this.displayedItems++;
	}

	private void AddUnitMessage(global::UnityEngine.Sprite unitIcon, string text)
	{
		if (this.displayedItems >= this.maxItemsPerPage)
		{
			this.overflowUnits.Add(new global::Tuple<global::UnityEngine.Sprite, string>(unitIcon, text));
		}
		else
		{
			global::UnityEngine.GameObject gameObject = this.units.AddToList();
			global::UIIconDesc component = gameObject.GetComponent<global::UIIconDesc>();
			component.SetLocalized(unitIcon, text);
		}
		this.displayedItems++;
	}

	private void OnContinue(bool confirm)
	{
		if (this.overflowUnits.Count > 0 || this.overflowWarbands.Count > 0)
		{
			this.displayedItems = 0;
			global::System.Collections.Generic.List<global::Tuple<global::UnityEngine.Sprite, string>> list = new global::System.Collections.Generic.List<global::Tuple<global::UnityEngine.Sprite, string>>();
			list.AddRange(this.overflowUnits);
			global::System.Collections.Generic.List<string> list2 = new global::System.Collections.Generic.List<string>();
			list2.AddRange(this.overflowWarbands);
			this.overflowUnits.Clear();
			this.overflowWarbands.Clear();
			for (int i = 0; i < this.warband.items.Count; i++)
			{
				this.warband.items[i].gameObject.SetActive(false);
			}
			for (int j = 0; j < this.units.items.Count; j++)
			{
				this.units.items[j].gameObject.SetActive(false);
			}
			for (int k = 0; k < list.Count; k++)
			{
				this.AddUnitMessage(list[k].Item1, list[k].Item2);
			}
			for (int l = 0; l < list2.Count; l++)
			{
				this.AddWarbandMessage(list2[l]);
			}
			this.warband.gameObject.SetActive(list2.Count > 0 && this.overflowWarbands.Count != list2.Count);
			this.units.gameObject.SetActive(list.Count > 0 && this.overflowUnits.Count != list.Count);
			base.Show(new global::System.Action<bool>(this.OnContinue), false, false);
		}
		else
		{
			this.onDone(confirm);
		}
	}

	public global::UnityEngine.UI.Image icon;

	public global::UnityEngine.UI.Text date;

	public global::UnityEngine.UI.Text holiday;

	public global::ListGroup units;

	public global::ListGroup warband;

	public global::UnityEngine.GameObject unitItem;

	public global::UnityEngine.GameObject warbandItem;

	public int maxItemsPerPage = 5;

	private global::System.Collections.Generic.List<global::UnitMenuController> unitList;

	private global::System.Collections.Generic.List<global::Tuple<global::UnityEngine.Sprite, string>> overflowUnits = new global::System.Collections.Generic.List<global::Tuple<global::UnityEngine.Sprite, string>>();

	private global::System.Collections.Generic.List<string> overflowWarbands = new global::System.Collections.Generic.List<string>();

	private global::System.Action<bool> onDone;

	private int displayedItems;
}
