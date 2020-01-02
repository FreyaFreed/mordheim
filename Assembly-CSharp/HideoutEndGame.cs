using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HideoutEndGame : global::ICheapState
{
	public HideoutEndGame(global::HideoutManager mng, global::HideoutCamAnchor anchor)
	{
		this.camAnchor = anchor;
	}

	void global::ICheapState.Update()
	{
		global::MissionEndDataSave endMission = global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.GetWarbandSave().endMission;
		if (endMission.isSkirmish)
		{
			global::PandoraSingleton<global::HideoutManager>.Instance.StateMachine.ChangeState(0);
			return;
		}
		switch (this.state)
		{
		case global::HideoutEndGame.EndState.SELECT_UNIT:
		{
			global::PandoraDebug.LogDebug("SELECT_UNIT", "uncategorised", null);
			this.prevState = global::HideoutEndGame.EndState.SELECT_UNIT;
			global::UnitMenuController unitMenuController = global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.unitCtrlrs[this.unitList[this.currentUnit].Value];
			unitMenuController.Hide(false, false, null);
			if (endMission.units[this.unitList[this.currentUnit].Key].status == global::UnitStateId.OUT_OF_ACTION)
			{
				unitMenuController.animator.SetInteger(global::AnimatorIds.unit_state, 2);
				unitMenuController.animator.Play(global::AnimatorIds.kneeling_stunned, -1, (float)global::PandoraSingleton<global::GameManager>.Instance.LocalTyche.Rand(0.0, 1.0));
			}
			global::PandoraSingleton<global::HideoutManager>.Instance.CamManager.dummyCam.transform.position = unitMenuController.transform.position + unitMenuController.transform.forward * 4f;
			global::PandoraSingleton<global::HideoutManager>.Instance.CamManager.dummyCam.transform.Translate(0f, 1.5f, 0f);
			global::PandoraSingleton<global::HideoutManager>.Instance.CamManager.dummyCam.transform.LookAt(unitMenuController.transform.position + new global::UnityEngine.Vector3(0f, 1f, 0f));
			global::PandoraSingleton<global::HideoutTabManager>.Instance.ActivateLeftTabModules(true, new global::ModuleId[]
			{
				global::ModuleId.UNIT_SHEET,
				global::ModuleId.UNIT_STATS
			});
			int xpGained = this.GetXpGained(endMission, unitMenuController);
			global::UnitSheetModule moduleLeft = global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleLeft<global::UnitSheetModule>(global::ModuleId.UNIT_SHEET);
			moduleLeft.SetInteractable(false);
			moduleLeft.RefreshAttributes(unitMenuController.unit);
			moduleLeft.RemoveDisplayedXp(xpGained);
			global::UnitStatsModule moduleLeft2 = global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleLeft<global::UnitStatsModule>(global::ModuleId.UNIT_STATS);
			moduleLeft2.RefreshStats(unitMenuController.unit, null, null);
			moduleLeft2.SetInteractable(false);
			this.state = global::HideoutEndGame.EndState.OOA;
			break;
		}
		case global::HideoutEndGame.EndState.OOA:
		{
			global::PandoraDebug.LogDebug("OOA", "uncategorised", null);
			this.prevState = global::HideoutEndGame.EndState.OOA;
			this.state = global::HideoutEndGame.EndState.WAIT;
			global::UnitMenuController unitMenuController = global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.unitCtrlrs[this.unitList[this.currentUnit].Value];
			if (endMission.VictoryType == global::VictoryTypeId.LOSS && endMission.units[this.unitList[this.currentUnit].Key].status == global::UnitStateId.OUT_OF_ACTION)
			{
				global::PandoraSingleton<global::HideoutTabManager>.Instance.ActivateLeftTabModule(true, new global::ModuleId[]
				{
					global::ModuleId.SLIDE_OOA
				});
				this.currentMod = global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleLeft<global::EndGameOoaModule>(global::ModuleId.SLIDE_OOA);
				((global::EndGameOoaModule)this.currentMod).Set(endMission.units[this.unitList[this.currentUnit].Key], unitMenuController.unit);
				this.ShowContinueButton(this.currentUnit, this.unitList.Count);
			}
			else
			{
				this.currentMod = null;
				this.state = global::HideoutEndGame.EndState.INJURY;
			}
			break;
		}
		case global::HideoutEndGame.EndState.INJURY:
		{
			global::PandoraDebug.LogDebug("INJURY", "uncategorised", null);
			this.prevState = global::HideoutEndGame.EndState.INJURY;
			this.state = global::HideoutEndGame.EndState.WAIT;
			global::UnitMenuController unitMenuController = global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.unitCtrlrs[this.unitList[this.currentUnit].Value];
			if (endMission.units[this.unitList[this.currentUnit].Key].injuries.Count > 0)
			{
				global::PandoraSingleton<global::HideoutTabManager>.Instance.ActivateLeftTabModule(false, new global::ModuleId[]
				{
					global::ModuleId.SLIDE_OOA
				});
				global::PandoraSingleton<global::HideoutTabManager>.Instance.ActivateLeftTabModule(true, new global::ModuleId[]
				{
					global::ModuleId.SLIDE_INJURY
				});
				this.currentMod = global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleLeft<global::EndGameInjuryModule>(global::ModuleId.SLIDE_INJURY);
				((global::EndGameInjuryModule)this.currentMod).Setup(endMission.units[this.unitList[this.currentUnit].Key]);
				if (endMission.units[this.unitList[this.currentUnit].Key].dead)
				{
					unitMenuController.PlayDefState(global::AttackResultId.HIT, 0, global::UnitStateId.OUT_OF_ACTION);
					this.prevState = global::HideoutEndGame.EndState.XP;
					this.ShowContinueButton(this.currentUnit, this.unitList.Count);
				}
				else
				{
					unitMenuController.LaunchAction(global::UnitActionId.NONE, true, global::UnitStateId.NONE, 0);
					if (endMission.units[this.unitList[this.currentUnit].Key].isMaxRank)
					{
						this.ShowNextUnitButton(this.currentUnit, this.unitList.Count);
					}
					else
					{
						this.ShowContinueButton(this.currentUnit, this.unitList.Count);
					}
				}
			}
			else
			{
				this.currentMod = null;
				this.state = global::HideoutEndGame.EndState.XP;
			}
			break;
		}
		case global::HideoutEndGame.EndState.XP:
		{
			global::PandoraDebug.LogDebug("XP", "uncategorised", null);
			this.prevState = global::HideoutEndGame.EndState.DEAD;
			this.state = global::HideoutEndGame.EndState.WAIT;
			global::UnitMenuController unitMenuController = global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.unitCtrlrs[this.unitList[this.currentUnit].Value];
			if (!endMission.units[this.unitList[this.currentUnit].Key].dead && !endMission.units[this.unitList[this.currentUnit].Key].isMaxRank)
			{
				global::PandoraSingleton<global::HideoutTabManager>.Instance.ActivateLeftTabModule(false, new global::ModuleId[]
				{
					global::ModuleId.SLIDE_INJURY
				});
				global::PandoraSingleton<global::HideoutTabManager>.Instance.ActivateLeftTabModule(true, new global::ModuleId[]
				{
					global::ModuleId.SLIDE_XP
				});
				this.currentMod = global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleLeft<global::EndGameXPModule>(global::ModuleId.SLIDE_XP);
				((global::EndGameXPModule)this.currentMod).Set(endMission.units[this.unitList[this.currentUnit].Key], unitMenuController.unit);
				int xpGained = this.GetXpGained(endMission, unitMenuController);
				global::UnitSheetModule moduleLeft = global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleLeft<global::UnitSheetModule>(global::ModuleId.UNIT_SHEET);
				moduleLeft.AddXp(xpGained);
				this.ShowNextUnitButton(this.currentUnit, this.unitList.Count);
			}
			else
			{
				this.currentMod = null;
				this.state = global::HideoutEndGame.EndState.NEXT_UNIT;
			}
			break;
		}
		case global::HideoutEndGame.EndState.DEAD:
		{
			global::PandoraDebug.LogDebug("DEAD", "uncategorised", null);
			this.prevState = global::HideoutEndGame.EndState.DEAD;
			this.state = global::HideoutEndGame.EndState.WAIT;
			global::UnitMenuController unitMenuController = global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.unitCtrlrs[this.unitList[this.currentUnit].Value];
			global::PandoraSingleton<global::HideoutTabManager>.Instance.ActivateLeftTabModule(false, new global::ModuleId[]
			{
				global::ModuleId.SLIDE_INJURY
			});
			global::InjuryId injuryId = unitMenuController.unit.UnitSave.injuries[unitMenuController.unit.UnitSave.injuries.Count - 1];
			global::PandoraSingleton<global::HideoutManager>.Instance.messagePopup.Show("injury_name_" + injuryId, "injury_retirement_desc_" + injuryId, delegate(bool c)
			{
				this.canAdvance = true;
				this.Advance();
			}, false, false);
			global::PandoraSingleton<global::HideoutManager>.Instance.messagePopup.HideCancelButton();
			global::PandoraSingleton<global::HideoutManager>.Instance.SaveChanges();
			this.currentMod = null;
			this.ShowNextUnitButton(this.currentUnit, this.unitList.Count);
			global::PandoraSingleton<global::Pan>.Instance.Narrate("unit_death" + global::PandoraSingleton<global::GameManager>.Instance.LocalTyche.Rand(3, 5));
			break;
		}
		case global::HideoutEndGame.EndState.NEXT_UNIT:
			global::PandoraDebug.LogDebug("NEXT_UNIT", "uncategorised", null);
			if (this.currentUnit != -1)
			{
				global::UnitMenuController unitMenuController = global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.unitCtrlrs[this.unitList[this.currentUnit].Value];
				unitMenuController.Hide(true, false, null);
			}
			else if (!global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.GetWarbandSave().endMission.isSkirmish)
			{
				for (int i = 0; i < global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.unitCtrlrs.Count; i++)
				{
					global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.unitCtrlrs[i].Hide(true, false, null);
				}
			}
			this.currentUnit++;
			this.prevState = global::HideoutEndGame.EndState.NEXT_UNIT;
			global::PandoraSingleton<global::HideoutTabManager>.Instance.ActivateLeftTabModule(false, new global::ModuleId[]
			{
				global::ModuleId.SLIDE_OOA
			});
			global::PandoraSingleton<global::HideoutTabManager>.Instance.ActivateLeftTabModule(false, new global::ModuleId[]
			{
				global::ModuleId.SLIDE_INJURY
			});
			global::PandoraSingleton<global::HideoutTabManager>.Instance.ActivateLeftTabModule(false, new global::ModuleId[]
			{
				global::ModuleId.SLIDE_XP
			});
			this.ShowContinueButton(this.currentUnit, this.unitList.Count);
			if (this.currentUnit >= this.unitList.Count)
			{
				global::PandoraSingleton<global::HideoutManager>.Instance.StateMachine.ChangeState(6);
				this.state = global::HideoutEndGame.EndState.WAIT;
			}
			else
			{
				this.state = global::HideoutEndGame.EndState.SELECT_UNIT;
			}
			break;
		case global::HideoutEndGame.EndState.WAIT:
			this.canAdvance = true;
			break;
		}
	}

	public void Destroy()
	{
	}

	public void Enter(int iFrom)
	{
		global::PandoraSingleton<global::HideoutManager>.Instance.CamManager.ClearLookAtFocus();
		global::PandoraSingleton<global::HideoutManager>.Instance.CamManager.CancelTransition();
		global::PandoraSingleton<global::HideoutManager>.Instance.CamManager.dummyCam.transform.position = this.camAnchor.transform.position;
		global::PandoraSingleton<global::HideoutManager>.Instance.CamManager.dummyCam.transform.rotation = this.camAnchor.transform.rotation;
		global::PandoraSingleton<global::HideoutManager>.Instance.CamManager.SetDOFTarget(this.camAnchor.dofTarget, 0f);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.ActivateRightTabModules(false, new global::ModuleId[]
		{
			global::ModuleId.TREASURY
		});
		global::PandoraSingleton<global::HideoutTabManager>.Instance.ActivateCenterTabModules(new global::ModuleId[]
		{
			global::ModuleId.TITLE,
			global::ModuleId.ACTION_BUTTON
		});
		this.actionButtonModule = global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleCenter<global::ActionButtonModule>(global::ModuleId.ACTION_BUTTON);
		this.actionButtonModule.Refresh(string.Empty, string.Empty, "menu_continue", new global::UnityEngine.Events.UnityAction(this.Advance));
		global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleCenter<global::TitleModule>(global::ModuleId.TITLE).Set("menu_mission_report", true);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleRight<global::TreasuryModule>(global::ModuleId.TREASURY).Refresh(global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.GetWarbandSave());
		global::MenuNodeGroup warbandNodeGroup = global::PandoraSingleton<global::HideoutManager>.Instance.warbandNodeGroup;
		global::PandoraSingleton<global::HideoutManager>.Instance.warbandNodeWagon.SetContent(global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.wagon);
		this.unitList = new global::System.Collections.Generic.List<global::System.Collections.Generic.KeyValuePair<int, int>>();
		global::MissionEndDataSave endMission = global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.GetWarbandSave().endMission;
		this.CleanUpMissionWagon();
		for (int i = 0; i < global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.unitCtrlrs.Count; i++)
		{
			global::UnitMenuController unitMenuController = global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.unitCtrlrs[i];
			if (unitMenuController.unit.Active)
			{
				global::MissionEndUnitSave missionEndUnitSave = null;
				for (int j = 0; j < endMission.units.Count; j++)
				{
					if (endMission.units[j].isPlayed && unitMenuController.unit.UnitSave.warbandSlotIndex == endMission.units[j].unitSave.warbandSlotIndex)
					{
						missionEndUnitSave = endMission.units[j];
						global::PandoraDebug.LogDebug("Adding Unit from MissionEndData", "uncategorised", null);
						if (endMission.isSkirmish)
						{
							global::PandoraSingleton<global::HideoutManager>.Instance.Progressor.UpdateUnitStats(missionEndUnitSave, unitMenuController.unit);
						}
						else
						{
							warbandNodeGroup.nodes[unitMenuController.unit.UnitSave.warbandSlotIndex].SetContent(unitMenuController, (!unitMenuController.unit.IsImpressive) ? null : warbandNodeGroup.nodes[unitMenuController.unit.UnitSave.warbandSlotIndex + 1]);
							unitMenuController.SwitchWeapons(global::UnitSlotId.SET1_MAINHAND);
							this.unitList.Add(new global::System.Collections.Generic.KeyValuePair<int, int>(j, i));
							global::PandoraSingleton<global::HideoutManager>.Instance.Progressor.EndGameUnitProgress(missionEndUnitSave, unitMenuController.unit);
						}
						break;
					}
				}
				if (!endMission.isSkirmish)
				{
					if (missionEndUnitSave != null && !missionEndUnitSave.dead)
					{
						unitMenuController.RefreshBodyParts();
						unitMenuController.RefreshEquipments(null);
					}
					unitMenuController.Hide(true, false, null);
					unitMenuController.SwitchWeapons(global::UnitSlotId.SET1_MAINHAND);
					if (missionEndUnitSave != null && missionEndUnitSave.status == global::UnitStateId.OUT_OF_ACTION)
					{
						unitMenuController.animator.SetInteger(global::AnimatorIds.unit_state, 2);
						unitMenuController.animator.Play(global::AnimatorIds.kneeling_stunned, -1, (float)global::PandoraSingleton<global::GameManager>.Instance.LocalTyche.Rand(0.0, 1.0));
					}
					else
					{
						unitMenuController.animator.Play(global::AnimatorIds.idle, -1, (float)global::PandoraSingleton<global::GameManager>.Instance.LocalTyche.Rand(0.0, 1.0));
					}
				}
			}
		}
		if (endMission.isSkirmish)
		{
			global::PandoraSingleton<global::HideoutManager>.Instance.Progressor.UpdateWarbandStats(global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband);
		}
		else
		{
			if (endMission.wagonItems != null)
			{
				float @float = global::Constant.GetFloat(global::ConstantId.WYRDSTONE_WEIGHT);
				int priceSold = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::ItemData>(130).PriceSold;
				int priceSold2 = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::ItemData>(208).PriceSold;
				int priceSold3 = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::ItemData>(209).PriceSold;
				global::System.Collections.Generic.List<global::ItemSave> items = endMission.wagonItems.GetItems();
				for (int k = items.Count - 1; k >= 0; k--)
				{
					if (items[k].id == 554 || items[k].id == 555)
					{
						for (int l = 0; l < items[k].amount; l++)
						{
							global::ItemSave unclaimedRecipe = global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.GetUnclaimedRecipe((global::ItemId)items[k].id, true, global::ItemId.NONE);
							items.Add(unclaimedRecipe);
							global::PandoraSingleton<global::HideoutManager>.Instance.WarbandChest.AddItem(unclaimedRecipe, false);
						}
						items.RemoveAt(k);
					}
					else
					{
						global::PandoraSingleton<global::HideoutManager>.Instance.WarbandChest.AddItem(items[k], false);
						if (items[k].id == 130)
						{
							global::PandoraSingleton<global::Hephaestus>.Instance.IncrementStat(global::Hephaestus.StatId.WYRDSTONE_GATHER, (int)((float)(items[k].amount * priceSold) * @float));
						}
						else if (items[k].id == 208)
						{
							global::PandoraSingleton<global::Hephaestus>.Instance.IncrementStat(global::Hephaestus.StatId.WYRDSTONE_GATHER, (int)((float)(items[k].amount * priceSold2) * @float));
						}
						else if (items[k].id == 209)
						{
							global::PandoraSingleton<global::Hephaestus>.Instance.IncrementStat(global::Hephaestus.StatId.WYRDSTONE_GATHER, (int)((float)(items[k].amount * priceSold3) * @float));
						}
					}
				}
			}
			global::PandoraSingleton<global::HideoutManager>.Instance.Progressor.EndGameWarbandProgress(endMission, global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband);
			global::PandoraSingleton<global::NoticeManager>.Instance.RegisterListener(global::Notices.TRANSITION_DONE, new global::DelReceiveNotice(this.OnTransitionDone));
			global::PandoraSingleton<global::HideoutTabManager>.Instance.ActivateLeftTabModules(false, new global::ModuleId[0]);
		}
		global::PandoraSingleton<global::HideoutManager>.Instance.SaveChanges();
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button1.gameObject.SetActive(false);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button2.gameObject.SetActive(false);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button3.gameObject.SetActive(false);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button4.gameObject.SetActive(false);
		this.currentUnit = -1;
		this.state = global::HideoutEndGame.EndState.WAIT;
		this.prevState = global::HideoutEndGame.EndState.NEXT_UNIT;
	}

	public void Exit(int iTo)
	{
		global::MissionEndDataSave endMission = global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.GetWarbandSave().endMission;
		global::PandoraSingleton<global::HideoutManager>.Instance.warbandNodeGroup.Deactivate();
		global::System.Collections.Generic.List<global::UnitMenuController> list = new global::System.Collections.Generic.List<global::UnitMenuController>();
		global::System.Collections.Generic.List<global::UnitMenuController> list2 = new global::System.Collections.Generic.List<global::UnitMenuController>();
		if (!endMission.isSkirmish)
		{
			for (int i = 0; i < this.unitList.Count; i++)
			{
				global::UnitMenuController unitMenuController = global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.unitCtrlrs[this.unitList[i].Value];
				if (endMission.units[this.unitList[i].Key].dead)
				{
					global::InjuryId injuryId = unitMenuController.unit.UnitSave.injuries[unitMenuController.unit.UnitSave.injuries.Count - 1];
					if (injuryId == global::InjuryId.DEAD)
					{
						list2.Add(unitMenuController);
					}
					else
					{
						list.Add(unitMenuController);
					}
				}
			}
			for (int j = 0; j < list.Count; j++)
			{
				global::InjuryId data = list[j].unit.UnitSave.injuries[list[j].unit.UnitSave.injuries.Count - 1];
				global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Disband(list[j].unit, global::EventLogger.LogEvent.RETIREMENT, (int)data);
			}
			for (int k = 0; k < list2.Count; k++)
			{
				global::InjuryId data2 = list2[k].unit.UnitSave.injuries[list2[k].unit.UnitSave.injuries.Count - 1];
				global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Disband(list2[k].unit, global::EventLogger.LogEvent.DEATH, (int)data2);
			}
		}
	}

	private void CleanUpMissionWagon()
	{
		global::MissionEndDataSave endMission = global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.GetWarbandSave().endMission;
		global::System.Collections.Generic.List<global::ItemSave> items = endMission.wagonItems.GetItems();
		for (int i = items.Count - 1; i >= 0; i--)
		{
			global::ItemData itemData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::ItemData>(items[i].id);
			if (itemData.ItemTypeId == global::ItemTypeId.QUEST_ITEM)
			{
				items.RemoveAt(i);
			}
		}
		if (endMission.VictoryType == global::VictoryTypeId.LOSS && (endMission.crushed || endMission.missionSave.VictoryTypeId == 2))
		{
			endMission.wagonItems.Clear();
		}
	}

	public void FixedUpdate()
	{
	}

	private void ShowNextUnitButton(int unitNum, int unitTotal)
	{
		string desc = string.Concat(new string[]
		{
			global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("unit_name_warriors"),
			" ",
			(unitNum + 1).ToString(),
			"/",
			unitTotal.ToString()
		});
		if (this.HasNextUnit())
		{
			this.actionButtonModule.Refresh(string.Empty, desc, "end_game_next_unit", new global::UnityEngine.Events.UnityAction(this.Advance));
		}
		else
		{
			this.ShowContinueButton(desc);
		}
	}

	private void ShowContinueButton(int unitNum, int unitTotal)
	{
		string desc = string.Concat(new string[]
		{
			global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("unit_name_warriors"),
			" ",
			(unitNum + 1).ToString(),
			"/",
			unitTotal.ToString()
		});
		this.ShowContinueButton(desc);
	}

	private void ShowContinueButton(string desc = "")
	{
		this.actionButtonModule.Refresh(string.Empty, desc, "menu_continue", new global::UnityEngine.Events.UnityAction(this.Advance));
	}

	private bool HasNextUnit()
	{
		return this.currentUnit + 1 < this.unitList.Count;
	}

	private void Advance()
	{
		if (this.canAdvance)
		{
			this.canAdvance = false;
			if (this.currentMod != null && this.currentMod.EndShow())
			{
				this.state = this.prevState + 1;
			}
			else if (this.currentMod == null)
			{
				this.state = global::HideoutEndGame.EndState.NEXT_UNIT;
			}
		}
	}

	private void OnTransitionDone()
	{
		global::PandoraSingleton<global::NoticeManager>.Instance.RemoveListener(global::Notices.TRANSITION_DONE, new global::DelReceiveNotice(this.OnTransitionDone));
		this.canAdvance = true;
		this.Advance();
		this.actionButtonModule.Refresh(string.Empty, string.Empty, "end_game_next_unit", new global::UnityEngine.Events.UnityAction(this.Advance));
		this.actionButtonModule.SetFocus();
	}

	private int GetXpGained(global::MissionEndDataSave endMission, global::UnitMenuController ctrlr)
	{
		int num = 0;
		for (int i = 0; i < endMission.units[this.unitList[this.currentUnit].Key].XPs.Count; i++)
		{
			num += endMission.units[this.unitList[this.currentUnit].Key].XPs[i].Key;
		}
		return global::UnityEngine.Mathf.Max(-ctrlr.unit.Xp, num);
	}

	private global::HideoutCamAnchor camAnchor;

	private global::System.Collections.Generic.List<global::UnitMenuController> warbandUnits;

	private global::ActionButtonModule actionButtonModule;

	private global::HideoutEndGame.EndState state;

	private global::HideoutEndGame.EndState prevState;

	private global::UIModule currentMod;

	private int currentUnit = -1;

	private global::System.Collections.Generic.List<global::System.Collections.Generic.KeyValuePair<int, int>> unitList = new global::System.Collections.Generic.List<global::System.Collections.Generic.KeyValuePair<int, int>>();

	private bool canAdvance;

	private enum EndState
	{
		NONE,
		SELECT_UNIT,
		OOA,
		INJURY,
		XP,
		DEAD,
		NEXT_UNIT,
		WAIT
	}
}
