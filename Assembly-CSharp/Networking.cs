using System;
using Steamworks;

public class Networking
{
	public Networking()
	{
		this.P2PSessionRequestCb = global::Steamworks.Callback<global::Steamworks.P2PSessionRequest_t>.Create(new global::Steamworks.Callback<global::Steamworks.P2PSessionRequest_t>.DispatchDelegate(this.OnP2PSessionRequest));
		this.P2PSessionConnectFailCb = global::Steamworks.Callback<global::Steamworks.P2PSessionConnectFail_t>.Create(new global::Steamworks.Callback<global::Steamworks.P2PSessionConnectFail_t>.DispatchDelegate(this.OnP2PSessionConnectFail));
	}

	public void SetDataReceivedCallback(global::Hephaestus.OnDataReceivedCallback cb)
	{
		this.dataReceivedCallback = cb;
	}

	public void CloseP2PSessionWithUser(ulong steamID)
	{
		global::Steamworks.SteamNetworking.CloseP2PSessionWithUser((global::Steamworks.CSteamID)steamID);
	}

	private void OnP2PSessionRequest(global::Steamworks.P2PSessionRequest_t callback)
	{
		global::PandoraDebug.LogDebug("OnP2PSessionRequest lobby is ?" + global::PandoraSingleton<global::Hephaestus>.Instance.Lobby != null, "HEPHAESTUS-STEAMWORKS", null);
		if (global::PandoraSingleton<global::Hephaestus>.Instance.Lobby != null)
		{
			int numLobbyMembers = global::Steamworks.SteamMatchmaking.GetNumLobbyMembers((global::Steamworks.CSteamID)global::PandoraSingleton<global::Hephaestus>.Instance.Lobby.id);
			if (numLobbyMembers != 2)
			{
				global::PandoraDebug.LogDebug("OnP2PSessionRequest need 2 members in lobby", "HEPHAESTUS-STEAMWORKS", null);
				return;
			}
			bool flag = false;
			for (int i = 0; i < numLobbyMembers; i++)
			{
				if (global::Steamworks.SteamMatchmaking.GetLobbyMemberByIndex((global::Steamworks.CSteamID)global::PandoraSingleton<global::Hephaestus>.Instance.Lobby.id, i) == callback.m_steamIDRemote)
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				global::PandoraDebug.LogDebug("OnP2PSessionRequest Accepting Session", "HEPHAESTUS-STEAMWORKS", null);
				global::Steamworks.SteamNetworking.AcceptP2PSessionWithUser(callback.m_steamIDRemote);
			}
			else
			{
				global::PandoraDebug.LogDebug("OnP2PSessionRequest occured outside of lobby/game", "HEPHAESTUS-STEAMWORKS", null);
			}
		}
	}

	private void OnP2PSessionConnectFail(global::Steamworks.P2PSessionConnectFail_t callback)
	{
		global::PandoraDebug.LogWarning("OnP2PSessionConnectFail Error = " + (global::Steamworks.EP2PSessionError)callback.m_eP2PSessionError, "uncategorised", null);
		global::PandoraSingleton<global::Hermes>.Instance.ConnectionError((ulong)callback.m_steamIDRemote, ((global::Steamworks.EP2PSessionError)callback.m_eP2PSessionError).ToLowerString());
	}

	public void GetConnectionState(ulong steamID)
	{
		global::Steamworks.P2PSessionState_t p2PSessionState_t = default(global::Steamworks.P2PSessionState_t);
		if (global::Steamworks.SteamNetworking.GetP2PSessionState((global::Steamworks.CSteamID)steamID, out p2PSessionState_t))
		{
			global::PandoraDebug.LogDebug(string.Concat(new object[]
			{
				"Current P2P State to ",
				steamID,
				" Is Connecting = ",
				p2PSessionState_t.m_bConnecting,
				" Is Connection Acvite = ",
				p2PSessionState_t.m_bConnectionActive,
				" Is Using Relay = ",
				p2PSessionState_t.m_bUsingRelay,
				" Is Remote IP = ",
				p2PSessionState_t.m_nRemoteIP,
				" Is Remote Port = ",
				p2PSessionState_t.m_nRemotePort
			}), "uncategorised", null);
		}
	}

	public void Send(bool reliable, global::Steamworks.CSteamID steamID, byte[] data)
	{
		if (reliable)
		{
			global::Steamworks.SteamNetworking.SendP2PPacket(steamID, data, (uint)data.Length, global::Steamworks.EP2PSend.k_EP2PSendReliable, 0);
		}
		else
		{
			global::Steamworks.SteamNetworking.SendP2PPacket(steamID, data, (uint)data.Length, global::Steamworks.EP2PSend.k_EP2PSendUnreliable, 0);
		}
	}

	public void ReadPackets()
	{
		uint num = 0U;
		while (global::Steamworks.SteamNetworking.IsP2PPacketAvailable(out num, 0))
		{
			byte[] array = new byte[num];
			uint num2 = 0U;
			global::Steamworks.CSteamID that;
			if (global::Steamworks.SteamNetworking.ReadP2PPacket(array, num, out num2, out that, 0))
			{
				this.dataReceivedCallback((ulong)that, array);
			}
		}
	}

	private global::Steamworks.Callback<global::Steamworks.P2PSessionRequest_t> P2PSessionRequestCb;

	private global::Steamworks.Callback<global::Steamworks.P2PSessionConnectFail_t> P2PSessionConnectFailCb;

	private global::Hephaestus.OnDataReceivedCallback dataReceivedCallback;
}
