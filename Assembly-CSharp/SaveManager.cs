using System;
using System.Collections.Generic;
using System.IO;

public class SaveManager
{
	public SaveManager()
	{
		this.Reset();
	}

	public int MaxSaveGames { get; private set; }

	public void Reset()
	{
		int @int = global::Constant.GetInt(global::ConstantId.SAVE_SLOTS_PER_WARBAND);
		global::System.Collections.Generic.List<global::WarbandData> list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::WarbandData>("Basic", "1");
		this.MaxSaveGames = list.Count * @int;
		this.timestamps = new global::System.DateTime[this.MaxSaveGames];
		this.saveIndices = new global::System.Collections.Generic.List<int>(this.MaxSaveGames);
		this.dataLoaded = false;
		this.firstEmptyIndex = -1;
		this.DeleteOldNameSaves();
	}

	private void DeleteOldNameSaves()
	{
		for (int i = 0; i < this.MaxSaveGames; i++)
		{
			string fileName = string.Format("savegame_{0}.sg", i);
			if (global::PandoraSingleton<global::Hephaestus>.Instance.FileExists(fileName))
			{
				global::PandoraSingleton<global::Hephaestus>.Instance.FileDelete(fileName, delegate(bool success)
				{
				});
			}
		}
	}

	public void EraseOldSaveGame()
	{
		this.CheckFileVersionTooOld(0);
	}

	private void CheckFileVersionTooOld(int startIdx)
	{
		for (int i = startIdx; i < this.MaxSaveGames; i++)
		{
			string saveFile = this.GetSaveFileName(i);
			if (global::PandoraSingleton<global::Hephaestus>.Instance.FileExists(saveFile))
			{
				int fileIdx = i;
				global::PandoraSingleton<global::Hephaestus>.Instance.FileRead(saveFile, delegate(byte[] data, bool success)
				{
					this.OnFileLoadedCheckOld(data, success, fileIdx, saveFile);
				});
				break;
			}
		}
	}

	private void OnFileLoadedCheckOld(byte[] data, bool success, int fileIdx, string saveFile)
	{
		if (success && data != null && data.Length > 4)
		{
			using (global::System.IO.MemoryStream memoryStream = new global::System.IO.MemoryStream(data))
			{
				using (global::System.IO.BinaryReader binaryReader = new global::System.IO.BinaryReader(memoryStream))
				{
					int num = binaryReader.ReadInt32();
					if (num < 24)
					{
						global::PandoraSingleton<global::Hephaestus>.Instance.FileDelete(saveFile, new global::Hephaestus.OnFileDeleteCallback(this.OnFileDelete));
						this.saveIndices.Remove(fileIdx);
						if (this.firstEmptyIndex > fileIdx)
						{
							this.firstEmptyIndex = fileIdx;
						}
					}
				}
				memoryStream.Close();
			}
		}
		if (fileIdx < this.MaxSaveGames - 1)
		{
			this.CheckFileVersionTooOld(fileIdx + 1);
		}
	}

	private void OnFileDelete(bool success)
	{
		if (!success)
		{
		}
	}

	private string GetSaveFileName(int campaign)
	{
		return string.Format(global::SaveManager.SAVE_FILE, campaign);
	}

	public bool CampaignExist(int campaign)
	{
		return global::PandoraSingleton<global::Hephaestus>.Instance.FileExists(this.GetSaveFileName(campaign));
	}

	public void LoadData()
	{
		this.saveIndices.Clear();
		this.firstEmptyIndex = -1;
		for (int i = 0; i < this.MaxSaveGames; i++)
		{
			if (this.CampaignExist(i))
			{
				this.timestamps[i] = this.GetSaveTimeStamp(i);
				if (this.saveIndices.Count == 0)
				{
					this.saveIndices.Add(i);
				}
				else
				{
					for (int j = 0; j < this.saveIndices.Count; j++)
					{
						if (this.timestamps[i] > this.timestamps[this.saveIndices[j]])
						{
							this.saveIndices.Insert(j, i);
							break;
						}
						if (j + 1 == this.saveIndices.Count)
						{
							this.saveIndices.Add(i);
							break;
						}
					}
				}
			}
			else if (this.firstEmptyIndex == -1)
			{
				this.firstEmptyIndex = i;
			}
		}
		this.dataLoaded = true;
	}

	public bool EmptyCampaignSaveExists()
	{
		if (this.firstEmptyIndex == -1 && !this.dataLoaded)
		{
			this.GetEmptyCampaignSlot();
		}
		return this.firstEmptyIndex != -1;
	}

	public int GetEmptyCampaignSlot()
	{
		if (!this.dataLoaded && this.firstEmptyIndex == -1)
		{
			for (int i = 0; i < this.MaxSaveGames; i++)
			{
				if (!this.CampaignExist(i))
				{
					this.firstEmptyIndex = i;
					break;
				}
			}
		}
		return this.firstEmptyIndex;
	}

	public bool HasCampaigns()
	{
		if (this.dataLoaded)
		{
			return this.saveIndices.Count > 0;
		}
		if (this.firstEmptyIndex > 0)
		{
			return true;
		}
		for (int i = 0; i < this.MaxSaveGames; i++)
		{
			if (this.CampaignExist(i))
			{
				return true;
			}
		}
		return false;
	}

	public global::System.Collections.Generic.List<int> GetCampaignSlots()
	{
		if (!this.dataLoaded)
		{
			this.LoadData();
		}
		return this.saveIndices;
	}

	public void SaveCampaign(global::WarbandSave warband, int campaign)
	{
		if (warband.inMission && warband.endMission.isSkirmish)
		{
			return;
		}
		global::PandoraSingleton<global::GameManager>.Instance.currentSave = warband;
		global::PandoraSingleton<global::GameManager>.Instance.Profile.LastPlayedCampaign = campaign;
		global::PandoraSingleton<global::GameManager>.Instance.Profile.UpdateHash(campaign, warband.GetCRC(false));
		global::PandoraSingleton<global::GameManager>.Instance.SaveProfile();
		byte[] data = global::Thoth.WriteToArray(warband);
		global::PandoraSingleton<global::Hephaestus>.Instance.FileWrite(this.GetSaveFileName(campaign), data, new global::Hephaestus.OnFileWriteCallback(this.OnSaveCampaign));
		global::PandoraDebug.LogDebug("SaveCampaign : campaign id = " + campaign, "SAVE MANAGER", null);
		if (this.dataLoaded)
		{
			this.saveIndices.Remove(campaign);
			this.saveIndices.Insert(0, campaign);
			this.timestamps[campaign] = global::System.DateTime.Now;
		}
	}

	private void OnSaveCampaign(bool success)
	{
		if (success)
		{
			global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice(global::Notices.GAME_SAVED);
		}
	}

	private global::System.DateTime GetSaveTimeStamp(int campaign)
	{
		return global::PandoraSingleton<global::Hephaestus>.Instance.GetFileTimeStamp(this.GetSaveFileName(campaign));
	}

	public global::System.DateTime GetCachedSaveTimeStamp(int campaign)
	{
		return this.timestamps[campaign];
	}

	public void DeleteCampaign(int campaign)
	{
		if (this.CampaignExist(campaign))
		{
			global::PandoraSingleton<global::Hephaestus>.Instance.FileDelete(this.GetSaveFileName(campaign), new global::Hephaestus.OnFileDeleteCallback(this.OnDeleteCampaign));
			if (this.dataLoaded)
			{
				this.saveIndices.Remove(campaign);
			}
			if (this.firstEmptyIndex > campaign || this.firstEmptyIndex == -1)
			{
				this.firstEmptyIndex = campaign;
			}
			global::PandoraSingleton<global::GameManager>.Instance.Profile.ClearHash(campaign);
			if (global::PandoraSingleton<global::GameManager>.Instance.Profile.LastPlayedCampaign == campaign)
			{
				global::PandoraSingleton<global::GameManager>.Instance.Profile.LastPlayedCampaign = -1;
			}
			global::PandoraSingleton<global::GameManager>.Instance.SaveProfile();
		}
	}

	private void OnDeleteCampaign(bool success)
	{
		if (!success)
		{
		}
	}

	public void LoadCampaign(int campaign)
	{
		global::PandoraSingleton<global::Hephaestus>.Instance.FileRead(this.GetSaveFileName(campaign), new global::Hephaestus.OnFileReadCallback(this.OnLoadCampaign));
	}

	public void OnLoadCampaign(byte[] data, bool success)
	{
		if (success)
		{
			global::WarbandSave warbandSave = new global::WarbandSave();
			global::Thoth.ReadFromArray(data, warbandSave);
			if (warbandSave.lastVersion < 31 && warbandSave.inMission)
			{
				warbandSave.inMission = false;
				warbandSave.endMission = null;
			}
			global::PandoraSingleton<global::GameManager>.Instance.currentSave = warbandSave;
			global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice(global::Notices.GAME_LOADED);
		}
		else
		{
			global::PandoraDebug.LogWarning("Unable to load Campaign", "SAVE_MANAGER", null);
		}
	}

	public void GetCampaignInfo(int campaign, global::System.Action<global::WarbandSave> OnInfoLoaded)
	{
		global::PandoraSingleton<global::Hephaestus>.Instance.FileRead(this.GetSaveFileName(campaign), delegate(byte[] data, bool success)
		{
			this.OnGetCampaignInfo(data, success, OnInfoLoaded);
		});
	}

	public void OnGetCampaignInfo(byte[] data, bool success, global::System.Action<global::WarbandSave> OnInfoLoaded)
	{
		if (success)
		{
			global::WarbandSave warbandSave = new global::WarbandSave();
			global::Thoth.ReadFromArray(data, warbandSave);
			OnInfoLoaded(warbandSave);
		}
		else
		{
			global::PandoraDebug.LogWarning("Unable to load Campaign Info", "SAVE_MANAGER", null);
		}
	}

	public void NewCampaign(int campaign, global::WarbandId warbandId, int forcedWarRank = 0)
	{
		int rank = 0;
		global::Warband warband = new global::Warband(new global::WarbandSave(warbandId));
		warband.Rank = forcedWarRank;
		global::Date date = new global::Date(global::Constant.GetInt(global::ConstantId.CAL_DAY_START));
		warband.GetWarbandSave().currentDate = date.CurrentDate;
		global::DataFactory instance = global::PandoraSingleton<global::DataFactory>.Instance;
		string field = "fk_warband_id";
		int num = (int)warbandId;
		global::System.Collections.Generic.List<global::WarbandNameData> list = instance.InitData<global::WarbandNameData>(field, num.ToString());
		warband.GetWarbandSave().name = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(list[global::PandoraSingleton<global::GameManager>.Instance.LocalTyche.Rand(0, list.Count)].Name);
		global::EventLogger eventLogger = new global::EventLogger(warband.GetWarbandSave().stats.history);
		eventLogger.AddHistory(date.CurrentDate, global::EventLogger.LogEvent.WARBAND_CREATED, 0);
		global::System.Collections.Generic.List<global::WarbandDefaultUnitData> list2 = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::WarbandDefaultUnitData>("fk_warband_id", ((int)warband.Id).ToString());
		global::System.Collections.Generic.Dictionary<global::WarbandSlotTypeId, int> dictionary = new global::System.Collections.Generic.Dictionary<global::WarbandSlotTypeId, int>();
		for (int i = 0; i < list2.Count; i++)
		{
			for (int j = 0; j < list2[i].Amount; j++)
			{
				global::Unit unit = global::Unit.GenerateUnit(list2[i].UnitId, rank);
				global::WarbandSlotTypeId warbandSlotTypeId = list2[i].WarbandSlotTypeId;
				unit.UnitSave.warbandSlotIndex = (int)(warbandSlotTypeId + ((!dictionary.ContainsKey(warbandSlotTypeId)) ? 0 : dictionary[warbandSlotTypeId]));
				if (dictionary.ContainsKey(warbandSlotTypeId))
				{
					global::System.Collections.Generic.Dictionary<global::WarbandSlotTypeId, int> dictionary3;
					global::System.Collections.Generic.Dictionary<global::WarbandSlotTypeId, int> dictionary2 = dictionary3 = dictionary;
					global::WarbandSlotTypeId key2;
					global::WarbandSlotTypeId key = key2 = warbandSlotTypeId;
					int num2 = dictionary3[key2];
					dictionary2[key] = num2 + 1;
				}
				else
				{
					dictionary[warbandSlotTypeId] = 1;
				}
				warband.HireUnit(unit);
			}
		}
		global::Market market = new global::Market(warband);
		market.RefreshMarket(global::MarketEventId.NO_EVENT, false);
		global::System.Collections.Generic.List<global::FactionData> list3 = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::FactionData>("fk_allegiance_id", ((int)warband.WarbandData.AllegianceId).ToString());
		int num3 = 0;
		for (int k = 0; k < list3.Count; k++)
		{
			if ((list3[k].Primary && list3[k].WarbandId == warbandId) || !list3[k].Primary)
			{
				global::FactionSave item = new global::FactionSave(list3[k].Id, num3);
				warband.GetWarbandSave().factions.Add(item);
				num3++;
			}
		}
		eventLogger.AddHistory(warband.GetWarbandSave().currentDate + 1, global::EventLogger.LogEvent.SHIPMENT_REQUEST, -1);
		global::DataFactory instance2 = global::PandoraSingleton<global::DataFactory>.Instance;
		string[] fields = new string[]
		{
			"fk_warband_id",
			"idx"
		};
		string[] array = new string[2];
		int num4 = 0;
		int num5 = (int)warbandId;
		array[num4] = num5.ToString();
		array[1] = warband.GetWarbandSave().curCampaignIdx.ToString();
		global::System.Collections.Generic.List<global::CampaignMissionData> list4 = instance2.InitData<global::CampaignMissionData>(fields, array);
		int date2 = warband.GetWarbandSave().currentDate + list4[0].Days;
		eventLogger.AddHistory(date2, global::EventLogger.LogEvent.NEW_MISSION, warband.GetWarbandSave().curCampaignIdx);
		global::WarbandChest warbandChest = new global::WarbandChest(warband);
		warbandChest.sendStats = false;
		warbandChest.AddItem(global::ItemId.GOLD, global::ItemQualityId.NORMAL, global::RuneMarkId.NONE, global::RuneMarkQualityId.NONE, global::AllegianceId.NONE, global::Constant.GetInt(global::ConstantId.CAMPAIGN_STARTING_MONEY) + global::PandoraSingleton<global::GameManager>.Instance.Profile.NewGameBonusGold, false);
		warbandChest.sendStats = true;
		global::System.Collections.Generic.List<global::WeekDayData> list5 = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::WeekDayData>();
		for (int l = 0; l < list5.Count; l++)
		{
			if (list5[l].RefreshMarket)
			{
				int nextDay = date.GetNextDay(list5[l].Id);
				warband.Logger.AddHistory(nextDay, global::EventLogger.LogEvent.MARKET_ROTATION, 0);
			}
			if (list5[l].RefreshOutsiders && warband.GetAttribute(global::WarbandAttributeId.OUTSIDERS_COUNT) > 0)
			{
				int nextDay2 = date.GetNextDay(list5[l].Id);
				warband.Logger.AddHistory(nextDay2, global::EventLogger.LogEvent.OUTSIDER_ROTATION, 0);
			}
		}
		this.SaveCampaign(warband.GetWarbandSave(), campaign);
		if (this.firstEmptyIndex == campaign)
		{
			this.firstEmptyIndex = -1;
			if (this.dataLoaded)
			{
				for (int m = 0; m < this.MaxSaveGames; m++)
				{
					if (!this.saveIndices.Contains(m))
					{
						this.firstEmptyIndex = m;
						break;
					}
				}
			}
		}
	}

	private const int DELETE_SAVE_UNDER = 24;

	private const int DELETE_MISSION_SAVE_UNDER = 31;

	public static readonly string SAVE_FILE = "savegame{0}.sg";

	private global::System.Collections.Generic.List<int> saveIndices;

	private global::System.DateTime[] timestamps;

	private int firstEmptyIndex = -1;

	private bool dataLoaded;
}
