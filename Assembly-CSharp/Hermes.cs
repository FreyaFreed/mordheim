using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Hermes : global::PandoraSingleton<global::Hermes>, global::IMyrtilus
{
	public int PlayerIndex { get; private set; }

	public bool DoNotDisconnectMode
	{
		get
		{
			return this.doNotDisconnect;
		}
		set
		{
			if (!this.doNotDisconnect && value)
			{
				for (int i = 0; i < this.connections.Count; i++)
				{
					global::System.Collections.Generic.KeyValuePair<ulong, float> keyValuePair = this.connections[i];
					this.connections[i] = new global::System.Collections.Generic.KeyValuePair<ulong, float>(keyValuePair.Key, global::UnityEngine.Time.realtimeSinceStartup);
				}
			}
			this.doNotDisconnect = value;
		}
	}

	public void ResetGUID()
	{
		this.guid = 1U;
	}

	public uint GetNextGUID()
	{
		return this.guid++;
	}

	private void Awake()
	{
		this.Init();
	}

	public void Init()
	{
		global::PandoraSingleton<global::Hephaestus>.Instance.SetDataReceivedCallback(new global::Hephaestus.OnDataReceivedCallback(this.OnDataReceivedCallback));
		this.StopConnections(true);
		this.connections = new global::System.Collections.Generic.List<global::System.Collections.Generic.KeyValuePair<ulong, float>>();
		this.myrtilii = new global::System.Collections.Generic.List<global::IMyrtilus>();
		this.RegisterToHermes();
		this.uid = 4294967294U;
	}

	private void OnDestroy()
	{
		this.StopConnections(true);
	}

	private void LateUpdate()
	{
		this.timer += global::UnityEngine.Time.deltaTime;
		if (this.timer >= 1f)
		{
			this.timer = 0f;
			if (this.connections.Count > 0 && this.DoNotDisconnectMode)
			{
				this.Send(false, global::Hermes.SendTarget.OTHERS, this.uid, 4U, new object[0]);
			}
			else if (this.connections.Count > 0 && global::UnityEngine.Time.realtimeSinceStartup - this.lastSent > 5f)
			{
				if (!this.DoNotDisconnectMode)
				{
					for (int i = 0; i < this.connections.Count; i++)
					{
						float num = global::UnityEngine.Time.realtimeSinceStartup - this.connections[i].Value;
						if (num > 20f)
						{
							global::PandoraDebug.LogDebug("SendKeepAlive... Haven't recieved anything in a while", "HERMES", this);
							this.ConnectionError(this.connections[i].Key, string.Empty);
							return;
						}
					}
				}
				this.Send(false, global::Hermes.SendTarget.OTHERS, this.uid, 4U, new object[0]);
			}
		}
	}

	public void SetTimeout(float time)
	{
		this.timeout = time;
	}

	public void StartHosting()
	{
		this.PlayerIndex = 0;
	}

	public bool IsConnected()
	{
		return this.connections.Count > 0;
	}

	public bool IsHost()
	{
		return this.PlayerIndex == 0;
	}

	public void StopConnections(bool resetPlayerIndex = true)
	{
		for (int i = 0; i < this.connections.Count; i++)
		{
			global::PandoraSingleton<global::Hephaestus>.instance.DisconnectFromUser(this.connections[i].Key);
		}
		if (global::PandoraSingleton<global::MissionManager>.Exists())
		{
			global::PandoraSingleton<global::Hephaestus>.instance.ResetNetwork();
		}
		this.connections = new global::System.Collections.Generic.List<global::System.Collections.Generic.KeyValuePair<ulong, float>>();
		if (resetPlayerIndex)
		{
			this.PlayerIndex = 0;
		}
	}

	public void ResetPlayerIndex()
	{
		this.PlayerIndex = 0;
	}

	public void RegisterMyrtilus(global::IMyrtilus myrtilus, bool needUID = true)
	{
		if (needUID)
		{
			myrtilus.uid = this.GetNextGUID();
		}
		global::PandoraDebug.LogDebug("RegisterMyrtilus ID = " + myrtilus.uid, "HERMES", this);
		this.myrtilii.Add(myrtilus);
	}

	public void RemoveMyrtilus(global::IMyrtilus myrtilus)
	{
		this.myrtilii.Remove(myrtilus);
	}

	public void OnDataReceivedCallback(ulong from, byte[] data)
	{
		for (int i = 0; i < this.connections.Count; i++)
		{
			if (this.connections[i].Key == from)
			{
				this.connections[i] = new global::System.Collections.Generic.KeyValuePair<ulong, float>(from, global::UnityEngine.Time.realtimeSinceStartup);
			}
		}
		using (global::System.IO.MemoryStream memoryStream = new global::System.IO.MemoryStream(data))
		{
			using (global::System.IO.BinaryReader binaryReader = new global::System.IO.BinaryReader(memoryStream))
			{
				uint num = binaryReader.ReadUInt32();
				uint command = binaryReader.ReadUInt32();
				for (int j = 0; j < this.myrtilii.Count; j++)
				{
					if (this.myrtilii[j].uid == num)
					{
						this.myrtilii[j].Receive(from, command, binaryReader.ReadArray());
						break;
					}
				}
			}
		}
	}

	public void NewConnection(ulong userId)
	{
		global::PandoraDebug.LogDebug("NewConnection this user has connected to me = " + userId, "HERMES", this);
		this.connections.Add(new global::System.Collections.Generic.KeyValuePair<ulong, float>(userId, global::UnityEngine.Time.realtimeSinceStartup));
		global::PandoraDebug.LogInfo("Send PlayerIndex ID " + this.connections.Count, "HERMES", this);
		this.Send(true, global::Hermes.SendTarget.OTHERS, this.uid, 3U, new object[]
		{
			this.connections.Count
		});
	}

	public void RemoveConnection(ulong userId)
	{
		global::PandoraDebug.LogDebug("RemoveConnection user has left me = " + userId, "HERMES", this);
		int i;
		for (i = 0; i < this.connections.Count; i++)
		{
			if (this.connections[i].Key == userId)
			{
				break;
			}
		}
		if (i < this.connections.Count)
		{
			this.connections.RemoveAt(i);
		}
		global::PandoraSingleton<global::Hephaestus>.instance.DisconnectFromUser(userId);
	}

	public void ConnectionError(ulong id, string error)
	{
		global::PandoraDebug.LogDebug(string.Concat(new object[]
		{
			"ConnectionError ",
			id,
			" error: ",
			error
		}), "HERMES", this);
		this.StopConnections(false);
		global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice(global::Notices.HERMES_CONNECTION_LOST);
	}

	public void SendLoadLevel(string nextSceneName, global::SceneLoadingTypeId loadingType, float transitionDuration, bool waitForAction = false, bool waitForPlayers = false, bool force = false)
	{
		global::PandoraDebug.LogInfo(string.Concat(new object[]
		{
			"SendLoadLevel  ",
			nextSceneName,
			"duration = ",
			transitionDuration,
			" waitforAction = ",
			waitForAction,
			" waitForPlayers = ",
			waitForPlayers
		}), "HERMES", this);
		this.Send(true, global::Hermes.SendTarget.ALL, this.uid, 1U, new object[]
		{
			nextSceneName,
			(int)loadingType,
			transitionDuration,
			waitForAction,
			waitForPlayers,
			force
		});
	}

	private void LoadLevelRPC(string nextSceneName, int loadingType, float transitionDuration, bool waitForAction, bool waitForPlayers, bool force)
	{
		global::PandoraDebug.LogInfo(string.Concat(new object[]
		{
			"LoadLevelRPC  ",
			nextSceneName,
			"duration = ",
			transitionDuration,
			" waitforAction = ",
			waitForAction,
			" waitforPlayers = ",
			waitForPlayers
		}), "HERMES", this);
		global::PandoraSingleton<global::TransitionManager>.Instance.LoadNextScene(nextSceneName, (global::SceneLoadingTypeId)loadingType, transitionDuration, waitForAction, waitForPlayers, force);
	}

	public void SetPlayerIndex(ulong host, int index)
	{
		global::PandoraDebug.LogInfo("CommandList.SET_PLAYER_INDEX index = " + index, "HERMES", this);
		this.connections.Add(new global::System.Collections.Generic.KeyValuePair<ulong, float>(host, global::UnityEngine.Time.realtimeSinceStartup));
		this.PlayerIndex = index;
		if (this.connectedCallback != null)
		{
			this.connectedCallback();
			this.connectedCallback = null;
		}
	}

	private void KeepAliveRPC(ulong user)
	{
	}

	public void SendChat(string message)
	{
		if (global::PandoraSingleton<global::Hephaestus>.Instance.IsPrivilegeRestricted(global::Hephaestus.RestrictionId.CHAT))
		{
			global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<ulong, string, bool>(global::Notices.HERMES_CHAT, global::PandoraSingleton<global::Hephaestus>.Instance.GetUserId(), global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("chat_restricted"), true);
		}
		else
		{
			this.Send(true, global::Hermes.SendTarget.ALL, this.uid, 5U, new object[]
			{
				message
			});
		}
	}

	private void ChatRPC(ulong user, string message)
	{
		if (!global::PandoraSingleton<global::Hephaestus>.Instance.IsPrivilegeRestricted(global::Hephaestus.RestrictionId.CHAT))
		{
			if (user == 0UL)
			{
				global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<ulong, string, bool>(global::Notices.HERMES_CHAT, user, message, false);
			}
			else
			{
				global::PandoraSingleton<global::Hephaestus>.Instance.CanReceiveMessages(delegate(bool allowed)
				{
					if (allowed)
					{
						global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<ulong, string, bool>(global::Notices.HERMES_CHAT, user, message, false);
					}
				});
			}
		}
	}

	private void OnNetworkError(string message)
	{
		global::PandoraDebug.LogInfo("Network Error = " + message, "HERMES", this);
	}

	private void OnNetworkConnect(bool success, string message)
	{
		if (!success)
		{
			global::PandoraDebug.LogWarning(string.Concat(new object[]
			{
				"This CLIENT has connected to a server success = ",
				success,
				" Message = ",
				message
			}), "HERMES", this);
		}
		else
		{
			global::PandoraDebug.LogInfo(string.Concat(new object[]
			{
				"This CLIENT has connected to a server success = ",
				success,
				" Message = ",
				message
			}), "HERMES", this);
		}
	}

	private void OnNetworkDisconnect()
	{
		global::PandoraDebug.LogInfo("This CLIENT has Disconnected from the server.", "HERMES", this);
	}

	private void OnNetworkJoinChannel(bool success, string message)
	{
		global::PandoraDebug.LogInfo(string.Concat(new object[]
		{
			"This CLIENT has connected to a server channel success = ",
			success,
			" ms = ",
			message
		}), "HERMES", this);
		if (this.connectedCallback != null)
		{
			this.connectedCallback();
			this.connectedCallback = null;
		}
	}

	private void OnNetworkLeaveChannel()
	{
		global::PandoraDebug.LogInfo("This CLIENT has disconnected from the channel", "HERMES", this);
	}

	public uint uid { get; set; }

	public uint owner { get; set; }

	public void RegisterToHermes()
	{
		this.RegisterMyrtilus(this, true);
	}

	public void RemoveFromHermes()
	{
		this.RemoveMyrtilus(this);
	}

	public void Send(bool reliable, global::Hermes.SendTarget target, uint id, uint command, params object[] parms)
	{
		byte[] data = null;
		using (global::System.IO.MemoryStream memoryStream = new global::System.IO.MemoryStream())
		{
			using (global::System.IO.BinaryWriter binaryWriter = new global::System.IO.BinaryWriter(memoryStream))
			{
				binaryWriter.Write(id);
				binaryWriter.Write(command);
				binaryWriter.WriteArray(parms);
				data = memoryStream.ToArray();
			}
		}
		switch (target)
		{
		case global::Hermes.SendTarget.HOST:
			if (this.IsHost())
			{
				this.OnDataReceivedCallback(0UL, data);
			}
			else
			{
				global::PandoraSingleton<global::Hephaestus>.Instance.SendData(reliable, this.connections[0].Key, data);
			}
			break;
		case global::Hermes.SendTarget.ALL:
			this.lastSent = global::UnityEngine.Time.realtimeSinceStartup;
			for (int i = 0; i < this.connections.Count; i++)
			{
				global::PandoraSingleton<global::Hephaestus>.Instance.SendData(reliable, this.connections[i].Key, data);
			}
			this.OnDataReceivedCallback(0UL, data);
			break;
		case global::Hermes.SendTarget.OTHERS:
			this.lastSent = global::UnityEngine.Time.realtimeSinceStartup;
			for (int j = 0; j < this.connections.Count; j++)
			{
				global::PandoraSingleton<global::Hephaestus>.Instance.SendData(reliable, this.connections[j].Key, data);
			}
			break;
		}
	}

	public void Receive(ulong from, uint command, object[] parms)
	{
		switch (command)
		{
		case 1U:
		{
			string nextSceneName = (string)parms[0];
			int loadingType = (int)parms[1];
			float transitionDuration = (float)parms[2];
			bool waitForAction = (bool)parms[3];
			bool waitForPlayers = (bool)parms[4];
			bool force = (bool)parms[5];
			this.LoadLevelRPC(nextSceneName, loadingType, transitionDuration, waitForAction, waitForPlayers, force);
			break;
		}
		case 3U:
		{
			int index = (int)parms[0];
			this.SetPlayerIndex(from, index);
			break;
		}
		case 4U:
			this.KeepAliveRPC(from);
			break;
		case 5U:
		{
			string message = (string)parms[0];
			this.ChatRPC(from, message);
			break;
		}
		}
	}

	private const float DEFAULT_TIMEOUT = 20f;

	private const float SEND_WAIT = 5f;

	private int levelPrefix;

	public global::Hermes.OnConnectedCallback connectedCallback;

	private bool doNotDisconnect;

	private global::System.Collections.Generic.List<global::IMyrtilus> myrtilii = new global::System.Collections.Generic.List<global::IMyrtilus>();

	private global::System.Collections.Generic.List<global::System.Collections.Generic.KeyValuePair<ulong, float>> connections = new global::System.Collections.Generic.List<global::System.Collections.Generic.KeyValuePair<ulong, float>>();

	private uint guid = 1U;

	private float timeout = 20f;

	private float lastSent;

	private float timer;

	public enum SendTarget
	{
		NONE,
		HOST,
		ALL,
		OTHERS
	}

	private enum CommandList
	{
		ERROR,
		LOAD_LEVEL,
		CONNECTION,
		SET_PLAYER_INDEX,
		KEEP_ALIVE,
		CHAT,
		COUNT
	}

	public delegate void OnConnectedCallback();
}
