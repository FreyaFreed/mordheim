using System;
using System.Net;
using TNet;
using UnityEngine;

[global::UnityEngine.AddComponentMenu("TNet/Network Server (internal)")]
public class TNServerInstance : global::UnityEngine.MonoBehaviour
{
	private static global::TNServerInstance instance
	{
		get
		{
			if (global::TNServerInstance.mInstance == null)
			{
				global::UnityEngine.GameObject gameObject = new global::UnityEngine.GameObject("_Server");
				global::TNServerInstance.mInstance = gameObject.AddComponent<global::TNServerInstance>();
				global::UnityEngine.Object.DontDestroyOnLoad(gameObject);
			}
			return global::TNServerInstance.mInstance;
		}
	}

	public static bool isActive
	{
		get
		{
			return global::TNServerInstance.mInstance != null && global::TNServerInstance.mInstance.mGame.isActive;
		}
	}

	public static bool isListening
	{
		get
		{
			return global::TNServerInstance.mInstance != null && global::TNServerInstance.mInstance.mGame.isListening;
		}
	}

	public static int listeningPort
	{
		get
		{
			return (!(global::TNServerInstance.mInstance != null)) ? 0 : global::TNServerInstance.mInstance.mGame.tcpPort;
		}
	}

	public static string serverName
	{
		get
		{
			return (!(global::TNServerInstance.mInstance != null)) ? null : global::TNServerInstance.mInstance.mGame.name;
		}
		set
		{
			if (global::TNServerInstance.instance != null)
			{
				global::TNServerInstance.mInstance.mGame.name = value;
			}
		}
	}

	public static int playerCount
	{
		get
		{
			return (!(global::TNServerInstance.mInstance != null)) ? 0 : global::TNServerInstance.mInstance.mGame.playerCount;
		}
	}

	public static global::TNet.GameServer game
	{
		get
		{
			return (!(global::TNServerInstance.mInstance != null)) ? null : global::TNServerInstance.mInstance.mGame;
		}
	}

	public static global::TNet.LobbyServer lobby
	{
		get
		{
			return (!(global::TNServerInstance.mInstance != null)) ? null : global::TNServerInstance.mInstance.mLobby;
		}
	}

	public static bool Start(int tcpPort)
	{
		return global::TNServerInstance.instance.StartLocal(tcpPort, 0, null, 0, global::TNServerInstance.Type.Udp);
	}

	public static bool Start(int tcpPort, int udpPort)
	{
		return global::TNServerInstance.instance.StartLocal(tcpPort, udpPort, null, 0, global::TNServerInstance.Type.Udp);
	}

	public static bool Start(int tcpPort, int udpPort, string fileName)
	{
		return global::TNServerInstance.instance.StartLocal(tcpPort, udpPort, fileName, 0, global::TNServerInstance.Type.Udp);
	}

	[global::System.Obsolete("Use TNServerInstance.Start(tcpPort, udpPort, lobbyPort, fileName) instead")]
	public static bool Start(int tcpPort, int udpPort, string fileName, int lanBroadcastPort)
	{
		return global::TNServerInstance.instance.StartLocal(tcpPort, udpPort, fileName, lanBroadcastPort, global::TNServerInstance.Type.Udp);
	}

	public static bool Start(int tcpPort, int udpPort, int lobbyPort, string fileName)
	{
		return global::TNServerInstance.instance.StartLocal(tcpPort, udpPort, fileName, lobbyPort, global::TNServerInstance.Type.Udp);
	}

	public static bool Start(int tcpPort, int udpPort, int lobbyPort, string fileName, global::TNServerInstance.Type type)
	{
		return global::TNServerInstance.instance.StartLocal(tcpPort, udpPort, fileName, lobbyPort, type);
	}

	public static bool Start(int tcpPort, int udpPort, string fileName, global::TNServerInstance.Type type, global::System.Net.IPEndPoint remoteLobby)
	{
		return global::TNServerInstance.instance.StartRemote(tcpPort, udpPort, fileName, remoteLobby, type);
	}

	private bool StartLocal(int tcpPort, int udpPort, string fileName, int lobbyPort, global::TNServerInstance.Type type)
	{
		if (this.mGame.isActive)
		{
			this.Disconnect();
		}
		if (lobbyPort > 0)
		{
			if (type == global::TNServerInstance.Type.Tcp)
			{
				this.mLobby = new global::TNet.TcpLobbyServer();
			}
			else
			{
				this.mLobby = new global::TNet.UdpLobbyServer();
			}
			if (!this.mLobby.Start(lobbyPort))
			{
				this.mLobby = null;
				return false;
			}
			if (type == global::TNServerInstance.Type.Tcp)
			{
				this.mUp.OpenTCP(lobbyPort);
			}
			else
			{
				this.mUp.OpenUDP(lobbyPort);
			}
			this.mGame.lobbyLink = new global::TNet.LobbyServerLink(this.mLobby);
		}
		if (this.mGame.Start(tcpPort, udpPort))
		{
			this.mUp.OpenTCP(tcpPort);
			this.mUp.OpenUDP(udpPort);
			if (!string.IsNullOrEmpty(fileName))
			{
				this.mGame.LoadFrom(fileName);
			}
			return true;
		}
		this.Disconnect();
		return false;
	}

	private bool StartRemote(int tcpPort, int udpPort, string fileName, global::System.Net.IPEndPoint remoteLobby, global::TNServerInstance.Type type)
	{
		if (this.mGame.isActive)
		{
			this.Disconnect();
		}
		if (remoteLobby != null && remoteLobby.Port > 0)
		{
			if (type == global::TNServerInstance.Type.Tcp)
			{
				this.mLobby = new global::TNet.TcpLobbyServer();
				this.mGame.lobbyLink = new global::TNet.TcpLobbyServerLink(remoteLobby);
			}
			else if (type == global::TNServerInstance.Type.Udp)
			{
				this.mLobby = new global::TNet.UdpLobbyServer();
				this.mGame.lobbyLink = new global::TNet.UdpLobbyServerLink(remoteLobby);
			}
			else
			{
				global::UnityEngine.Debug.LogWarning("The remote lobby server type must be either UDP or TCP, not LAN");
			}
		}
		if (this.mGame.Start(tcpPort, udpPort))
		{
			this.mUp.OpenTCP(tcpPort);
			this.mUp.OpenUDP(udpPort);
			if (!string.IsNullOrEmpty(fileName))
			{
				this.mGame.LoadFrom(fileName);
			}
			return true;
		}
		this.Disconnect();
		return false;
	}

	public static void Stop()
	{
		if (global::TNServerInstance.mInstance != null)
		{
			global::TNServerInstance.mInstance.Disconnect();
		}
	}

	public static void Stop(string fileName)
	{
		if (global::TNServerInstance.mInstance != null && global::TNServerInstance.mInstance.mGame.isActive)
		{
			if (!string.IsNullOrEmpty(fileName))
			{
				global::TNServerInstance.mInstance.mGame.SaveTo(fileName);
			}
			global::TNServerInstance.Stop();
		}
	}

	public static void SaveTo(string fileName)
	{
		if (global::TNServerInstance.mInstance != null && global::TNServerInstance.mInstance.mGame.isActive && !string.IsNullOrEmpty(fileName))
		{
			global::TNServerInstance.mInstance.mGame.SaveTo(fileName);
		}
	}

	public static void MakePrivate()
	{
		if (global::TNServerInstance.mInstance != null)
		{
			global::TNServerInstance.mInstance.mGame.MakePrivate();
		}
	}

	private void Disconnect()
	{
		this.mGame.Stop();
		if (this.mLobby != null)
		{
			this.mLobby.Stop();
			this.mLobby = null;
		}
		this.mUp.Close();
	}

	private void OnDestroy()
	{
		this.Disconnect();
		this.mUp.WaitForThreads();
	}

	private static global::TNServerInstance mInstance;

	private global::TNet.GameServer mGame = new global::TNet.GameServer();

	private global::TNet.LobbyServer mLobby;

	private global::TNet.UPnP mUp = new global::TNet.UPnP();

	public enum Type
	{
		Lan,
		Udp,
		Tcp
	}

	public enum State
	{
		Inactive,
		Starting,
		Active
	}
}
