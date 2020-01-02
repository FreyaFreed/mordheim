using System;
using System.Collections.Generic;

public class MessageManager
{
	public void Init(global::CampaignMissionId missionId)
	{
		this.currentMissionId = missionId;
		this.messages = new global::System.Collections.Generic.Dictionary<int, global::System.Collections.Generic.List<string>>();
		if (missionId == global::CampaignMissionId.NONE)
		{
			return;
		}
		this.lastMessageTurn = 0;
		global::DataFactory instance = global::PandoraSingleton<global::DataFactory>.Instance;
		string field = "fk_campaign_mission_id";
		int num = (int)missionId;
		global::System.Collections.Generic.List<global::CampaignMissionMessageData> list = instance.InitData<global::CampaignMissionMessageData>(field, num.ToString());
		global::CampaignMissionMessageData mesData;
		foreach (global::CampaignMissionMessageData mesData2 in list)
		{
			mesData = mesData2;
			if (!this.messages.ContainsKey(mesData.UnitTurn))
			{
				global::System.Collections.Generic.List<global::CampaignMissionMessageData> list2 = list.FindAll((global::CampaignMissionMessageData x) => x.UnitTurn == mesData.UnitTurn);
				list2.Sort(new global::MessageSorter());
				this.messages[mesData.UnitTurn] = list2.ConvertAll<string>((global::CampaignMissionMessageData x) => x.Label);
				if (mesData.UnitTurn >= this.lastMessageTurn)
				{
					this.lastMessageTurn = mesData.UnitTurn;
				}
			}
		}
		this.currentTurn = 0;
	}

	public bool DisplayNewTurn(global::MessageDelegate displayedDel = null)
	{
		this.currentTurn++;
		this.currentPos = 0;
		return this.DisplayMessage(displayedDel);
	}

	public bool DisplayNextPos(global::MessageDelegate displayedDel = null)
	{
		this.currentPos++;
		return this.DisplayMessage(displayedDel);
	}

	private bool DisplayMessage(global::MessageDelegate displayedDel = null)
	{
		this.del = displayedDel;
		if (this.messages.ContainsKey(this.currentTurn) && this.currentPos < this.messages[this.currentTurn].Count)
		{
			bool v = this.currentTurn == this.lastMessageTurn && this.messages[this.currentTurn].Count - 1 == this.currentPos;
			string text = this.messages[this.currentTurn][this.currentPos];
			if (global::PandoraSingleton<global::PandoraInput>.Instance.lastInputMode == global::PandoraInput.InputMode.JOYSTICK)
			{
				string text2 = text;
				if (text2 != null)
				{
					if (global::MessageManager.<>f__switch$mapE == null)
					{
						global::MessageManager.<>f__switch$mapE = new global::System.Collections.Generic.Dictionary<string, int>(3)
						{
							{
								"tuto_01_message_02",
								0
							},
							{
								"tuto_03_message_07",
								0
							},
							{
								"tuto_03_message_11",
								0
							}
						};
					}
					int num;
					if (global::MessageManager.<>f__switch$mapE.TryGetValue(text2, out num))
					{
						if (num == 0)
						{
							text += "_console";
						}
					}
				}
			}
			global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<bool, string, bool>(global::Notices.GAME_TUTO_MESSAGE, true, text, v);
			global::PandoraDebug.LogInfo(this.messages[this.currentTurn][this.currentPos], "MESSAGE", null);
			if (this.del != null)
			{
				this.del();
				this.del = null;
			}
			return true;
		}
		return false;
	}

	private void OnMessageDisplayed()
	{
		if (this.del != null)
		{
			this.del();
			this.del = null;
		}
	}

	private global::System.Collections.Generic.Dictionary<int, global::System.Collections.Generic.List<string>> messages;

	private int lastMessageTurn;

	public int currentTurn;

	public int currentPos;

	private global::MessageDelegate del;

	private global::CampaignMissionId currentMissionId;
}
