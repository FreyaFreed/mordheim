using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideoutManager : global::PandoraSingleton<global::HideoutManager>
{
	public global::CheapStateMachine StateMachine { get; private set; }

	public global::WarbandMenuController WarbandCtrlr { get; private set; }

	public global::WarbandChest WarbandChest { get; private set; }

	public global::Market Market { get; private set; }

	public global::Progressor Progressor { get; private set; }

	public global::Date Date { get; private set; }

	public global::CameraManager CamManager { get; private set; }

	public bool IsTrainingOutsider { get; set; }

	private void Awake()
	{
		global::PandoraSingleton<global::PandoraInput>.Instance.SetCurrentState(global::PandoraInput.States.MENU, true);
		global::HideoutCamAnchor[] componentsInChildren = base.GetComponentsInChildren<global::HideoutCamAnchor>();
		this.StateMachine = new global::CheapStateMachine(19);
		int i = 0;
		while (i < componentsInChildren.Length)
		{
			switch (componentsInChildren[i].state)
			{
			case global::HideoutManager.State.CAMP:
				this.StateMachine.AddState(new global::HideoutCamp(this, componentsInChildren[i]), 0);
				break;
			case global::HideoutManager.State.WARBAND:
				this.StateMachine.AddState(new global::HideoutWarband(this, componentsInChildren[i]), 1);
				break;
			case global::HideoutManager.State.INVENTORY:
				this.StateMachine.AddState(new global::HideoutInventory(this, componentsInChildren[i]), 2);
				break;
			case global::HideoutManager.State.SKIRMISH:
				this.StateMachine.AddState(new global::HideoutSkirmish(this, componentsInChildren[i]), 3);
				break;
			case global::HideoutManager.State.LOBBY:
				this.StateMachine.AddState(new global::HideoutLobby(this, componentsInChildren[i]), 4);
				break;
			case global::HideoutManager.State.END_GAME:
				this.StateMachine.AddState(new global::HideoutEndGame(this, componentsInChildren[i]), 5);
				break;
			case global::HideoutManager.State.END_GAME_WARBAND:
				this.StateMachine.AddState(new global::HideoutEndGameWarband(this, componentsInChildren[i]), 6);
				break;
			case global::HideoutManager.State.SKILLS:
				this.StateMachine.AddState(new global::HideoutSkills(this, componentsInChildren[i], false), 7);
				break;
			case global::HideoutManager.State.SPELLS:
				this.StateMachine.AddState(new global::HideoutSkills(this, componentsInChildren[i], true), 8);
				break;
			case global::HideoutManager.State.SHIPMENT:
				this.StateMachine.AddState(new global::HideoutSmuggler(this, componentsInChildren[i]), 9);
				break;
			case global::HideoutManager.State.SHOP:
				this.StateMachine.AddState(new global::HideoutShop(this, componentsInChildren[i]), 10);
				break;
			case global::HideoutManager.State.PLAYER_PROGRESSION:
				this.StateMachine.AddState(new global::HideoutPlayerProgression(this, componentsInChildren[i]), 11);
				break;
			case global::HideoutManager.State.MISSION:
				this.StateMachine.AddState(new global::HideoutMission(this, componentsInChildren[i]), 12);
				break;
			case global::HideoutManager.State.MISSION_PREPARATION:
				this.StateMachine.AddState(new global::HideoutMissionPrep(this, componentsInChildren[i]), 13);
				break;
			case global::HideoutManager.State.HIRE:
				this.StateMachine.AddState(new global::HideoutHire(this, componentsInChildren[i]), 14);
				break;
			case global::HideoutManager.State.UNIT_INFO:
				this.StateMachine.AddState(new global::HideoutUnitInfo(this, componentsInChildren[i]), 15);
				break;
			case global::HideoutManager.State.CUSTOMIZATION:
				this.StateMachine.AddState(new global::HideoutCustomization(this, componentsInChildren[i]), 16);
				break;
			case global::HideoutManager.State.WARBAND_STATS:
				this.StateMachine.AddState(new global::HideoutWarbandStats(this, componentsInChildren[i]), 18);
				break;
			}
			IL_26B:
			i++;
			continue;
			goto IL_26B;
		}
		this.StateMachine.AddState(new global::HideoutOptions(this), 17);
		if (!global::PandoraSingleton<global::MissionStartData>.Exists())
		{
			global::UnityEngine.GameObject gameObject = new global::UnityEngine.GameObject("mission_start_data");
			gameObject.AddComponent<global::MissionStartData>();
		}
		else
		{
			global::PandoraSingleton<global::MissionStartData>.Instance.Clear();
		}
		global::PandoraSingleton<global::NoticeManager>.Instance.RegisterListener(global::Notices.SKIRMISH_INVITE_ACCEPTED, new global::DelReceiveNotice(this.InviteReceived));
		base.StartCoroutine(this.Load());
	}

	private global::System.Collections.IEnumerator Load()
	{
		yield return null;
		yield return base.StartCoroutine(global::PandoraSingleton<global::HideoutTabManager>.Instance.Load());
		yield return null;
		this.Progressor = new global::Progressor();
		yield return null;
		global::WarbandSave warSave = global::PandoraSingleton<global::GameManager>.instance.currentSave;
		this.Date = new global::Date(warSave.currentDate);
		this.WarbandCtrlr = new global::WarbandMenuController(warSave);
		yield return null;
		this.WarbandChest = new global::WarbandChest(this.WarbandCtrlr.Warband);
		yield return null;
		this.Market = new global::Market(this.WarbandCtrlr.Warband);
		yield return null;
		this.GetShopNodeContent();
		yield return null;
		this.CamManager = global::UnityEngine.Camera.main.GetComponent<global::CameraManager>();
		global::CameraSetter cameraSetter = base.GetComponent<global::CameraSetter>();
		cameraSetter.SetCameraInfo(global::UnityEngine.Camera.main);
		global::UnityEngine.Camera.main.enabled = false;
		bool wasSkirmish = false;
		if (warSave.inMission)
		{
			wasSkirmish = warSave.endMission.isSkirmish;
		}
		if (!this.WarbandCtrlr.Warband.GetWarbandSave().inMission || wasSkirmish)
		{
			this.GenerateMissions(true);
		}
		else
		{
			this.missions = new global::System.Collections.Generic.List<global::Mission>();
			warSave.missions.Clear();
			warSave.scoutsSent = -1;
		}
		global::PandoraSingleton<global::HideoutTabManager>.Instance.DeactivateAllButtons();
		yield return null;
		global::PandoraSingleton<global::NoticeManager>.Instance.RegisterListener(global::Notices.TRANSITION_DONE, new global::DelReceiveNotice(this.OnTransitionDone));
		this.ValidateDLCs();
		this.ValidateDeadUnits();
		this.ValidateNewMutations();
		this.ValidateOutsiderRotationMissingEvent();
		this.startedLoading = true;
		yield return base.StartCoroutine(this.WarbandCtrlr.GenerateUnits());
		this.WarbandCtrlr.SetBannerWagon();
		yield return null;
		this.WarbandCtrlr.GenerateMap();
		this.startedLoading = false;
		yield return null;
		this.WarbandCtrlr.Warband.CheckRespecPoints();
		global::PandoraSingleton<global::GameManager>.Instance.Profile.CheckXp();
		global::PandoraSingleton<global::Hephaestus>.Instance.SetDLCBoughtCb(new global::Hephaestus.OnDLCBoughtCallback(this.ValidateDLCs));
		global::PandoraSingleton<global::Hephaestus>.Instance.LeaveLobby();
		global::PandoraSingleton<global::Hermes>.Instance.StopConnections(true);
		this.SaveChanges();
		yield return null;
		this.WarbandCtrlr.Warband.UpdateAttributes();
		string music = "hideout_" + this.WarbandCtrlr.Warband.WarbandData.Id.ToLowerString();
		global::PandoraSingleton<global::Pan>.Instance.PlayMusic(music, false);
		global::PandoraSingleton<global::Hephaestus>.Instance.SetRichPresence(global::Hephaestus.RichPresenceId.HIDEOUT, true);
		while (!this.finishedLoading)
		{
			yield return null;
		}
		this.CamManager.gameObject.GetComponent<global::UnityEngine.Camera>().enabled = true;
		while (this.tabsLoading > 0)
		{
			yield return null;
		}
		yield return base.StartCoroutine(global::PandoraSingleton<global::HideoutTabManager>.Instance.ParentModules());
		global::PandoraDebug.LogDebug("Loading Finished!", "uncategorised", null);
		global::PandoraSingleton<global::TransitionManager>.Instance.SetGameLoadingDone(false);
		if (this.WarbandCtrlr.Warband.GetWarbandSave().inMission)
		{
			this.WarbandCtrlr.Warband.GetWarbandSave().inMission = false;
			this.StateMachine.ChangeState(5);
		}
		else
		{
			if (global::PandoraSingleton<global::Hephaestus>.Instance.IsJoiningInvite())
			{
				this.checkInvite = true;
			}
			if (global::PandoraSingleton<global::Hephaestus>.Instance.IsPlayTogether())
			{
				this.checkPlayTogether = true;
			}
			this.StateMachine.ChangeState(0);
		}
		yield break;
	}

	private void ValidateDeadUnits()
	{
		for (int i = 0; i < this.WarbandCtrlr.Units.Count; i++)
		{
			global::Unit unit = this.WarbandCtrlr.Units[i];
			for (int j = 0; j < unit.Injuries.Count; j++)
			{
				if (unit.IsInjuryAttributeLimitExceeded(unit.Injuries[j].Data, true) || unit.IsInjuryRepeatLimitExceeded(unit.Injuries[j].Data, true))
				{
					this.WarbandCtrlr.Disband(unit, (unit.Injuries[j].Data.Id != global::InjuryId.DEAD) ? global::EventLogger.LogEvent.RETIREMENT : global::EventLogger.LogEvent.DEATH, (int)unit.Injuries[j].Data.Id);
					break;
				}
			}
		}
	}

	private void ValidateNewMutations()
	{
		for (int i = 0; i < this.WarbandCtrlr.Units.Count; i++)
		{
			this.CheckForMutation(this.WarbandCtrlr.Units[i], false);
		}
		for (int j = 0; j < this.WarbandCtrlr.Warband.Outsiders.Count; j++)
		{
			this.CheckForMutation(this.WarbandCtrlr.Warband.Outsiders[j], true);
		}
	}

	private void CheckForMutation(global::Unit unit, bool outsider)
	{
		if (!unit.UnitSave.mutationChecked)
		{
			if (unit.Rank == 4 || unit.Rank == 7 || unit.Rank == 8 || unit.Rank == 9)
			{
				global::System.Collections.Generic.List<global::UnitJoinUnitRankData> list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::UnitJoinUnitRankData>(new string[]
				{
					"fk_unit_id",
					"mutation"
				}, new string[]
				{
					unit.Id.ToIntString<global::UnitId>(),
					"1"
				});
				int num = 0;
				for (int i = 0; i < list.Count; i++)
				{
					int num2 = (int)((float)list[i].UnitRankId / 10f);
					if (unit.Rank >= num2)
					{
						num++;
					}
				}
				while (unit.Mutations.Count < num)
				{
					global::System.Collections.Generic.List<global::Item> list2 = new global::System.Collections.Generic.List<global::Item>();
					global::Mutation mutation = unit.AddRandomMutation(list2);
					unit.ResetBodyPart();
					if (!outsider)
					{
						this.WarbandChest.AddItems(list2);
					}
				}
			}
			unit.UnitSave.mutationChecked = true;
		}
	}

	private void ValidateOutsiderRotationMissingEvent()
	{
		if (this.WarbandCtrlr.Warband.GetAttribute(global::WarbandAttributeId.OUTSIDERS_COUNT) > 0 && this.WarbandCtrlr.Warband.Logger.FindEventsAfter(global::EventLogger.LogEvent.OUTSIDER_ROTATION, this.Date.CurrentDate + 1).Count < 2)
		{
			this.WarbandCtrlr.Warband.AddOutsiderRotationEvent();
		}
	}

	private void ValidateDLCs()
	{
		this.CheckUnitDLC(global::Hephaestus.DlcId.SMUGGLER, global::AllegianceId.ORDER, global::WarbandSkillId.DLC_SMUGGLER);
		this.CheckUnitDLC(global::Hephaestus.DlcId.PRIEST_OF_ULRIC, global::AllegianceId.ORDER, global::WarbandSkillId.DLC_PRIEST_OF_ULRIC);
		this.CheckUnitDLC(global::Hephaestus.DlcId.GLOBADIER, global::AllegianceId.DESTRUCTION, global::WarbandSkillId.DLC_GLOBADIER);
		this.CheckUnitDLC(global::Hephaestus.DlcId.DOOMWEAVER, global::AllegianceId.DESTRUCTION, global::WarbandSkillId.DLC_DOOMWEAVER);
	}

	private void CheckUnitDLC(global::Hephaestus.DlcId dlcId, global::AllegianceId requestedAllegiance, global::WarbandSkillId grantedSkillId)
	{
		if (global::PandoraSingleton<global::Hephaestus>.Instance.OwnsDLC(dlcId) && this.WarbandCtrlr.Warband.WarbandData.AllegianceId == requestedAllegiance && this.WarbandCtrlr.Warband.GetWarbandSave().skills.IndexOf(grantedSkillId, global::WarbandSkillIdComparer.Instance) == -1)
		{
			this.WarbandCtrlr.Warband.LearnSkill(new global::WarbandSkill(grantedSkillId));
		}
	}

	public bool IsPostMission()
	{
		return this.WarbandCtrlr.Warband.GetWarbandSave().scoutsSent < 0;
	}

	public global::UnitMenuController GetUnitMenuController(global::Unit unit)
	{
		for (int i = 0; i < this.WarbandCtrlr.unitCtrlrs.Count; i++)
		{
			if (this.WarbandCtrlr.unitCtrlrs[i].unit == unit)
			{
				return this.WarbandCtrlr.unitCtrlrs[i];
			}
		}
		return null;
	}

	public void GenerateMissions(bool newday)
	{
		this.missions = new global::System.Collections.Generic.List<global::Mission>();
		if (newday && this.WarbandCtrlr.Warband.GetWarbandSave().curCampaignIdx <= global::Constant.GetInt(global::ConstantId.CAMPAIGN_LAST_MISSION))
		{
			global::Tuple<int, global::EventLogger.LogEvent, int> tuple = this.WarbandCtrlr.Warband.Logger.FindLastEvent(global::EventLogger.LogEvent.NEW_MISSION);
			if (tuple.Item1 <= this.Date.CurrentDate && tuple.Item3 == this.WarbandCtrlr.Warband.GetWarbandSave().curCampaignIdx)
			{
				this.missions.Add(global::Mission.GenerateCampaignMission(this.WarbandCtrlr.Warband.Id, this.WarbandCtrlr.Warband.GetWarbandSave().curCampaignIdx));
			}
		}
		this.usedPositions = new global::System.Collections.Generic.List<global::System.Collections.Generic.KeyValuePair<int, int>>();
		if (this.WarbandCtrlr.Warband.GetWarbandSave().missions.Count == 0)
		{
			int attribute = this.WarbandCtrlr.Warband.GetAttribute(global::WarbandAttributeId.PROC_MISSIONS_AVAILABLE);
			for (int i = 0; i < attribute; i++)
			{
				this.AddProcMission(i != 0);
			}
			this.SaveChanges();
		}
		else
		{
			global::WarbandSave warbandSave = this.WarbandCtrlr.Warband.GetWarbandSave();
			for (int j = 0; j < warbandSave.missions.Count; j++)
			{
				global::Mission mission = new global::Mission(warbandSave.missions[j]);
				this.missions.Add(mission);
				this.usedPositions.Add(new global::System.Collections.Generic.KeyValuePair<int, int>((int)mission.GetDistrictId(), mission.missionSave.mapPosition));
			}
		}
	}

	public global::Mission AddProcMission(bool boost)
	{
		global::Mission mission = global::Mission.GenerateProcMission(ref this.usedPositions, this.WarbandCtrlr.Warband.WyrdstoneDensityModifiers, this.WarbandCtrlr.Warband.SearchDensityModifiers, this.WarbandCtrlr.Warband.MissionRatingModifiers);
		this.WarbandCtrlr.Warband.GetWarbandSave().missions.Add(mission.missionSave);
		this.missions.Add(mission);
		return mission;
	}

	private void OnTransitionDone()
	{
		global::PandoraDebug.LogInfo("OnTransitionDone", "FLOW", this);
		global::PandoraSingleton<global::NoticeManager>.Instance.RemoveListener(global::Notices.TRANSITION_DONE, new global::DelReceiveNotice(this.OnTransitionDone));
		this.transitionDone = true;
		if (this.nextTuto != global::HideoutManager.HideoutTutoType.NONE)
		{
			this.ShowHideoutTuto(this.nextTuto);
			this.nextTuto = global::HideoutManager.HideoutTutoType.NONE;
		}
	}

	private void OnDestroy()
	{
		this.StateMachine.Destroy();
		global::PandoraSingleton<global::Hephaestus>.Instance.SetDLCBoughtCb(null);
	}

	public void OnNextDay()
	{
		if (!this.finishedLoading)
		{
			return;
		}
		this.popupOrder = global::HideoutManager.NextDayOrder.NONE;
		this.currentMissionRewardsIndex = 0;
		this.ShowNextPopup();
	}

	private void ShowNextPopup()
	{
		this.popupOrder++;
		global::Tuple<int, global::EventLogger.LogEvent, int> tuple = null;
		switch (this.popupOrder)
		{
		case global::HideoutManager.NextDayOrder.NEXT_DAY:
			this.messagePopup.Show("hideout_day_skip", "hideout_day_skip_desc", new global::System.Action<bool>(this.OnNextDayPopup), false, false);
			break;
		case global::HideoutManager.NextDayOrder.NEW_MISSION:
			if (this.WarbandCtrlr.Warband.GetWarbandSave().curCampaignIdx <= global::Constant.GetInt(global::ConstantId.CAMPAIGN_LAST_MISSION))
			{
				tuple = this.WarbandCtrlr.Warband.Logger.FindLastEvent(global::EventLogger.LogEvent.NEW_MISSION);
			}
			if (tuple != null && tuple.Item1 == this.Date.CurrentDate && tuple.Item3 == this.WarbandCtrlr.Warband.GetWarbandSave().curCampaignIdx)
			{
				this.newMissionPopup.Setup(new global::System.Action<bool>(this.OnNewMissionConfirm), false);
				global::PandoraSingleton<global::Pan>.Instance.Narrate("new_campaign_mission" + global::PandoraSingleton<global::GameManager>.Instance.LocalTyche.Rand(1, 3));
			}
			else
			{
				this.ShowNextPopup();
			}
			break;
		case global::HideoutManager.NextDayOrder.SHIPMENT_LATE:
			tuple = this.WarbandCtrlr.Warband.Logger.FindLastEvent(global::EventLogger.LogEvent.SHIPMENT_LATE);
			if (tuple != null && tuple.Item1 + 1 == this.Date.CurrentDate)
			{
				global::WarbandSave warbandSave = this.WarbandCtrlr.Warband.GetWarbandSave();
				global::Unit unit = this.Progressor.CheckLateShipment(warbandSave, this.WarbandCtrlr, this.WarbandCtrlr.PrimaryFactionController);
				((global::HideoutCamp)this.StateMachine.GetActiveState()).RefreshButtons();
				global::PandoraSingleton<global::HideoutTabManager>.Instance.ActivatePopupModules(global::PopupBgSize.TALL, new global::PopupModuleId[]
				{
					global::PopupModuleId.POPUP_GENERIC_ANCHOR
				});
				global::System.Collections.Generic.List<global::UIPopupModule> modulesPopup = global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModulesPopup<global::UIPopupModule>(new global::PopupModuleId[]
				{
					global::PopupModuleId.POPUP_GENERIC_ANCHOR
				});
				global::ConfirmationPopupView confirmationPopupView = (global::ConfirmationPopupView)modulesPopup[0];
				if (unit != null)
				{
					confirmationPopupView.ShowLocalized(global::PandoraSingleton<global::LocalizationManager>.instance.GetStringById("popup_shipment_late_title"), global::PandoraSingleton<global::LocalizationManager>.instance.GetStringById(this.WarbandCtrlr.PrimaryFactionController.GetConsequenceLabel(), new string[]
					{
						unit.Name
					}), new global::System.Action<bool>(this.OnShipmentLatePopup), false, false);
					confirmationPopupView.HideCancelButton();
				}
				else
				{
					confirmationPopupView.ShowLocalized(global::PandoraSingleton<global::LocalizationManager>.instance.GetStringById("popup_shipment_late_title"), global::PandoraSingleton<global::LocalizationManager>.instance.GetStringById(this.WarbandCtrlr.PrimaryFactionController.GetConsequenceLabel() + "_no_units"), new global::System.Action<bool>(this.OnShipmentLatePopup), false, false);
					confirmationPopupView.HideCancelButton();
				}
				global::PandoraSingleton<global::Pan>.Instance.Narrate("shipment_failed" + global::PandoraSingleton<global::GameManager>.Instance.LocalTyche.Rand(1, 3));
				this.SaveChanges();
			}
			else
			{
				this.ShowNextPopup();
			}
			break;
		case global::HideoutManager.NextDayOrder.MISSION_REWARDS:
		{
			global::System.Collections.Generic.List<global::Tuple<int, global::EventLogger.LogEvent, int>> list = this.WarbandCtrlr.Warband.Logger.FindEventsAtDay(global::EventLogger.LogEvent.MISSION_REWARDS, this.Date.CurrentDate);
			if (list.Count > 0 && this.currentMissionRewardsIndex < list.Count)
			{
				tuple = list[this.currentMissionRewardsIndex];
				this.currentMissionRewardsIndex++;
				global::System.Collections.Generic.List<global::CampaignMissionData> list2 = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::CampaignMissionData>(new string[]
				{
					"fk_warband_id",
					"idx"
				}, new string[]
				{
					((int)this.WarbandCtrlr.Warband.WarbandData.Id).ToString(),
					tuple.Item3.ToString()
				});
				global::WarbandSkillId warbandSkillIdReward = list2[0].WarbandSkillIdReward;
				this.WarbandCtrlr.Warband.AddSkill(warbandSkillIdReward, true, true);
				this.SaveChanges();
				global::System.Collections.Generic.List<global::WarbandSkillItemData> itemRewards = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::WarbandSkillItemData>("fk_warband_skill_id", warbandSkillIdReward.ToIntString<global::WarbandSkillId>());
				global::System.Collections.Generic.List<global::WarbandSkillFreeOutsiderData> freeOutsiders = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::WarbandSkillFreeOutsiderData>("fk_warband_skill_id", warbandSkillIdReward.ToIntString<global::WarbandSkillId>());
				this.missionRewardPopup.Show(new global::System.Action<bool>(this.OnMissionRewardPopup), itemRewards, freeOutsiders);
				this.WarbandCtrlr.GenerateHireableUnits();
				this.RefreshTreasury();
				global::PandoraSingleton<global::Pan>.Instance.Narrate("shipment_completed");
			}
			else
			{
				this.ShowNextPopup();
			}
			break;
		}
		}
	}

	private void OnMissionRewardPopup(bool isConfirm)
	{
		this.popupOrder--;
		this.ShowNextPopup();
	}

	private void OnShipmentLatePopup(bool isConfirm)
	{
		this.ShowNextPopup();
	}

	private void OnNextDayPopup(bool isConfirm)
	{
		if (isConfirm)
		{
			this.Date.NextDay();
			this.WarbandCtrlr.Warband.GetWarbandSave().currentDate = this.Date.CurrentDate;
			global::PandoraDebug.LogInfo("New current date " + this.Date.CurrentDate, "MENUS", null);
			if (this.Date.CurrentDate == global::Constant.GetInt(global::ConstantId.CAL_DAY_START) + global::Constant.GetInt(global::ConstantId.CAL_DAYS_PER_YEAR))
			{
				global::PandoraSingleton<global::Hephaestus>.Instance.UnlockAchievement(global::Hephaestus.TrophyId.YEAR_1);
			}
			else if (this.Date.CurrentDate == global::Constant.GetInt(global::ConstantId.CAL_DAY_START) + global::Constant.GetInt(global::ConstantId.CAL_DAYS_PER_YEAR) * 5)
			{
				global::PandoraSingleton<global::Hephaestus>.Instance.UnlockAchievement(global::Hephaestus.TrophyId.YEAR_5);
			}
			global::System.Collections.Generic.List<global::UnitMenuController> list = new global::System.Collections.Generic.List<global::UnitMenuController>(this.WarbandCtrlr.unitCtrlrs);
			for (int i = 0; i < list.Count; i++)
			{
				this.Progressor.NextDayUnitProgress(list[i].unit);
			}
			this.Progressor.NextDayWarbandProgress();
			this.SaveChanges();
			this.nextDayPopup.SetUnitList(list);
			this.nextDayPopup.Show(new global::System.Action<bool>(this.OnNextDayFinished), false, false);
			this.RefreshTreasury();
			global::HideoutCamp hideoutCamp = (global::HideoutCamp)this.StateMachine.GetActiveState();
			hideoutCamp.RefreshButtons();
		}
	}

	private void RefreshTreasury()
	{
		global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleRight<global::TreasuryModule>(global::ModuleId.TREASURY).Refresh(this.WarbandCtrlr.Warband.GetWarbandSave());
	}

	private void OnNextDayFinished(bool isConfirm)
	{
		this.nextDayPopup.SetUnitList(null);
		global::PandoraDebug.LogDebug("OnNextDayFinished!", "uncategorised", null);
		this.ShowNextPopup();
	}

	private void OnNewMissionConfirm(bool isConfirm)
	{
		this.ShowNextPopup();
	}

	private void Update()
	{
		if (this.transitionDone)
		{
			if (this.checkInvite)
			{
				this.CheckInvite();
			}
			else if (this.checkPlayTogether)
			{
				this.CheckPlayTogether();
			}
		}
		this.StateMachine.Update();
		if (this.WarbandCtrlr != null)
		{
			this.WarbandCtrlr.generatingHireable = false;
		}
	}

	private void FixedUpdate()
	{
		this.StateMachine.FixedUpdate();
	}

	public void SaveChanges()
	{
		if (this.WarbandCtrlr != null)
		{
			global::PandoraSingleton<global::GameManager>.Instance.Save.SaveCampaign(this.WarbandCtrlr.Warband.GetWarbandSave(), global::PandoraSingleton<global::GameManager>.Instance.campaign);
		}
	}

	public global::UnityEngine.GameObject GetShipmentNodeContent()
	{
		if (this.shipmentNodeContent == null)
		{
			this.shipmentNodeContent = global::UnityEngine.Object.Instantiate<global::UnityEngine.GameObject>(this.shipmentNodePrefab);
		}
		return this.shipmentNodeContent;
	}

	public global::UnityEngine.GameObject GetNextDayNodeContent()
	{
		if (this.nextDayNodeContent == null)
		{
			this.nextDayNodeContent = global::UnityEngine.Object.Instantiate<global::UnityEngine.GameObject>(this.nextDayNodePrefab);
		}
		return this.nextDayNodeContent;
	}

	public global::UnityEngine.GameObject GetShopNodeContent()
	{
		if (this.shopNodeContent == null)
		{
			this.shopNodeContent = global::UnityEngine.Object.Instantiate<global::UnityEngine.GameObject>(this.shopNodePrefab);
			this.shopNodeContent.GetComponentInChildren<global::RackLoadoutNode>().SetLoadout(this.WarbandCtrlr.Warband.WarbandData.Wagon);
		}
		return this.shopNodeContent;
	}

	public void ShowHideoutTuto(global::HideoutManager.HideoutTutoType type)
	{
		if (global::PandoraSingleton<global::GameManager>.Instance.Options.skipTuto)
		{
			return;
		}
		if (!this.transitionDone)
		{
			this.nextTuto = type;
			return;
		}
		base.StartCoroutine(this.ShowTuto(type));
	}

	private global::System.Collections.IEnumerator ShowTuto(global::HideoutManager.HideoutTutoType type)
	{
		if (!this.WarbandCtrlr.Warband.HasShownHideoutTuto(type) && !global::PandoraSingleton<global::Hephaestus>.Instance.IsJoiningInvite())
		{
			this.showingTuto = true;
			yield return new global::UnityEngine.WaitForSeconds(0.2f);
			global::PandoraSingleton<global::HideoutTabManager>.Instance.ActivatePopupModules(global::PopupBgSize.TALL, new global::PopupModuleId[]
			{
				global::PopupModuleId.POPUP_GENERIC_ANCHOR
			});
			global::System.Collections.Generic.List<global::UIPopupModule> modules = global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModulesPopup<global::UIPopupModule>(new global::PopupModuleId[]
			{
				global::PopupModuleId.POPUP_GENERIC_ANCHOR
			});
			global::PandoraDebug.LogDebug("module length = " + modules.Count, "uncategorised", null);
			global::ConfirmationPopupView popup = modules[0].GetComponent<global::ConfirmationPopupView>();
			string typeTitle = type.ToString();
			string typeDesc = type.ToString();
			if (typeTitle[typeTitle.Length - 2] == '_')
			{
				typeTitle = typeTitle.Substring(0, typeTitle.Length - 2);
			}
			this.WarbandCtrlr.Warband.SetHideoutTutoShown(type);
			this.SaveChanges();
			popup.Show("hideout_" + typeTitle, "hideout_tuto_" + typeDesc, new global::System.Action<bool>(this.OnTutoMessageClose), false, false);
			popup.HideCancelButton();
		}
		yield break;
	}

	private void OnTutoMessageClose(bool confirm)
	{
		this.showingTuto = false;
	}

	private void InviteReceived()
	{
		this.checkInvite = true;
	}

	public bool IsCheckingInvite()
	{
		return this.checkInvite || this.checkPlayTogether;
	}

	private void CheckInvite()
	{
		this.checkInvite = false;
		if (!this.WarbandCtrlr.Warband.ValidateWarbandForInvite(false))
		{
			global::PandoraSingleton<global::GameManager>.Instance.ShowSystemPopup(global::GameManager.SystemPopupId.INVITE, "invite_hideout_quit_title", "invite_hideout_quit_desc", new global::System.Action<bool>(this.OnHideoutReceiveInviteShouldSaveAndQuit), true);
		}
		else if (this.StateMachine.GetActiveStateId() == 4)
		{
			global::PandoraSingleton<global::SkirmishManager>.Instance.LeaveLobby();
		}
		else if (this.StateMachine.GetActiveStateId() != 3 && this.StateMachine.GetActiveStateId() != 5 && this.StateMachine.GetActiveStateId() != 6 && this.StateMachine.GetActiveStateId() != 4)
		{
			this.StateMachine.ChangeState(3);
		}
		else
		{
			this.checkInvite = (this.StateMachine.GetActiveStateId() != 3);
		}
	}

	private void CheckPlayTogether()
	{
		this.checkPlayTogether = false;
		if (global::PandoraSingleton<global::Hephaestus>.Instance.IsPlayTogether())
		{
			string text;
			if (!this.WarbandCtrlr.Warband.IsSkirmishAvailable(out text) && !this.WarbandCtrlr.Warband.IsContestAvailable(out text))
			{
				global::PandoraSingleton<global::GameManager>.Instance.ShowSystemPopup(global::GameManager.SystemPopupId.INVITE, "play_together_hideout_quit_title", "play_together_hideout_quit_desc", new global::System.Action<bool>(this.OnHideoutReceiveInviteShouldSaveAndQuit), true);
			}
			else if (this.StateMachine.GetActiveStateId() == 4)
			{
				global::PandoraSingleton<global::SkirmishManager>.Instance.LeaveLobby();
			}
			else if (this.StateMachine.GetActiveStateId() != 3 && this.StateMachine.GetActiveStateId() != 5 && this.StateMachine.GetActiveStateId() != 6 && this.StateMachine.GetActiveStateId() != 4)
			{
				this.StateMachine.ChangeState(3);
			}
		}
	}

	private void OnHideoutReceiveInviteShouldSaveAndQuit(bool confirm)
	{
		if (confirm)
		{
			this.SaveChanges();
			global::SceneLauncher.Instance.LaunchScene(global::SceneLoadingId.OPTIONS_QUIT_GAME, false, false);
		}
		else
		{
			global::PandoraSingleton<global::Hephaestus>.Instance.ResetInvite();
			global::PandoraSingleton<global::Hephaestus>.Instance.ResetPlayTogether(true);
		}
	}

	public global::System.Collections.Generic.List<global::Mission> missions;

	private global::System.Collections.Generic.List<global::System.Collections.Generic.KeyValuePair<int, int>> usedPositions;

	public global::MenuNodeGroup campNodeGroup;

	public global::MenuNode warbandNodeWagon;

	public global::MenuNode warbandNodeFlag;

	public global::MenuNodeGroup warbandNodeGroup;

	public global::MenuNodeGroup skirmishNodeGroup;

	public global::MenuNode shopNode;

	public global::MenuNode progressNode;

	public global::MenuNode mapNode;

	public global::MenuNode unitNode;

	public global::UnityEngine.GameObject optionsPanel;

	public global::UnityEngine.GameObject shopNodePrefab;

	public global::UnityEngine.GameObject shipmentNodePrefab;

	public global::UnityEngine.GameObject nextDayNodePrefab;

	private global::UnityEngine.GameObject shipmentNodeContent;

	private global::UnityEngine.GameObject shopNodeContent;

	private global::UnityEngine.GameObject nextDayNodeContent;

	public global::ConfirmationPopupView messagePopup;

	public global::DailyReportPopup nextDayPopup;

	public global::NewMissionPopup newMissionPopup;

	public global::TextInputPopup textInputPopup;

	public global::MissionRewardPopup missionRewardPopup;

	public global::ShopConfirmPopup shopConfirmPopup;

	[global::UnityEngine.HideInInspector]
	public global::UnitMenuController currentUnit;

	[global::UnityEngine.HideInInspector]
	public int currentWarbandSlotIdx;

	[global::UnityEngine.HideInInspector]
	public bool currentWarbandSlotHireImpressive;

	private global::HideoutManager.NextDayOrder popupOrder;

	private int currentMissionRewardsIndex;

	public bool finishedLoading;

	public bool startedLoading;

	public bool transitionDone;

	private global::HideoutManager.HideoutTutoType nextTuto;

	[global::UnityEngine.HideInInspector]
	public bool showingTuto;

	private bool checkInvite;

	[global::UnityEngine.HideInInspector]
	public bool checkPlayTogether;

	public global::PlayTogetherPopupView playTogetherPopup;

	public int tabsLoading;

	public enum State
	{
		CAMP,
		WARBAND,
		INVENTORY,
		SKIRMISH,
		LOBBY,
		END_GAME,
		END_GAME_WARBAND,
		SKILLS,
		SPELLS,
		SHIPMENT,
		SHOP,
		PLAYER_PROGRESSION,
		MISSION,
		MISSION_PREPARATION,
		HIRE,
		UNIT_INFO,
		CUSTOMIZATION,
		OPTIONS,
		WARBAND_STATS,
		COUNT
	}

	public enum NextDayOrder
	{
		NONE,
		NEXT_DAY,
		NEW_MISSION,
		SHIPMENT_LATE,
		MISSION_REWARDS
	}

	public enum HideoutTutoType
	{
		NONE,
		CAMP,
		MANAGEMENT,
		SMUGGLER = 4,
		PROGRESSION = 8,
		SKIRMISH = 16,
		CAMPAIGN = 32,
		SHOP = 64,
		UNIT = 128,
		HIRE = 256,
		CAMP_2 = 512,
		CAMP_3 = 1024
	}
}
