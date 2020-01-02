using System;
using System.IO;
using System.Net;
using System.Reflection;
using TNet;
using UnityEngine;

[global::UnityEngine.AddComponentMenu("TNet/Network Manager")]
public class TNManager : global::UnityEngine.MonoBehaviour
{
	public static global::TNet.GameClient client
	{
		get
		{
			return (!(global::TNManager.mInstance != null)) ? null : global::TNManager.mInstance.mClient;
		}
	}

	public static bool isConnected
	{
		get
		{
			return global::TNManager.mInstance != null && global::TNManager.mInstance.mClient.isConnected;
		}
	}

	public static bool isTryingToConnect
	{
		get
		{
			return global::TNManager.mInstance != null && global::TNManager.mInstance.mClient.isTryingToConnect;
		}
	}

	public static bool isHosting
	{
		get
		{
			return global::TNManager.mInstance == null || global::TNManager.mInstance.mClient.isHosting;
		}
	}

	public static bool isInChannel
	{
		get
		{
			return global::TNManager.mInstance != null && global::TNManager.mInstance.mClient.isConnected && global::TNManager.mInstance.mClient.isInChannel;
		}
	}

	public static bool isActive
	{
		get
		{
			return global::TNManager.mInstance != null && global::TNManager.mInstance.mClient.isActive;
		}
		set
		{
			if (global::TNManager.mInstance != null)
			{
				global::TNManager.mInstance.mClient.isActive = value;
			}
		}
	}

	public static bool noDelay
	{
		get
		{
			return global::TNManager.mInstance != null && global::TNManager.mInstance.mClient.noDelay;
		}
		set
		{
			if (global::TNManager.mInstance != null)
			{
				global::TNManager.mInstance.mClient.noDelay = value;
			}
		}
	}

	public static int ping
	{
		get
		{
			return (!(global::TNManager.mInstance != null)) ? 0 : global::TNManager.mInstance.mClient.ping;
		}
	}

	public static bool canUseUDP
	{
		get
		{
			return global::TNManager.mInstance != null && global::TNManager.mInstance.mClient.canUseUDP;
		}
	}

	public static int listeningPort
	{
		get
		{
			return (!(global::TNManager.mInstance != null)) ? 0 : global::TNManager.mInstance.mClient.listeningPort;
		}
	}

	public static int objectOwnerID
	{
		get
		{
			return global::TNManager.mObjectOwner;
		}
	}

	public static bool isThisMyObject
	{
		get
		{
			return global::TNManager.mObjectOwner == global::TNManager.playerID;
		}
	}

	public static global::System.Net.IPEndPoint packetSource
	{
		get
		{
			return (!(global::TNManager.mInstance != null)) ? null : global::TNManager.mInstance.mClient.packetSource;
		}
	}

	public static string channelData
	{
		get
		{
			return (!(global::TNManager.mInstance != null)) ? string.Empty : global::TNManager.mInstance.mClient.channelData;
		}
		set
		{
			if (global::TNManager.mInstance != null)
			{
				global::TNManager.mInstance.mClient.channelData = value;
			}
		}
	}

	public static int channelID
	{
		get
		{
			return (!global::TNManager.isConnected) ? 0 : global::TNManager.mInstance.mClient.channelID;
		}
	}

	public static int hostID
	{
		get
		{
			return (!global::TNManager.isConnected) ? global::TNManager.mPlayer.id : global::TNManager.mInstance.mClient.hostID;
		}
	}

	public static int playerID
	{
		get
		{
			return (!global::TNManager.isConnected) ? global::TNManager.mPlayer.id : global::TNManager.mInstance.mClient.playerID;
		}
	}

	public static string playerName
	{
		get
		{
			return (!global::TNManager.isConnected) ? global::TNManager.mPlayer.name : global::TNManager.mInstance.mClient.playerName;
		}
		set
		{
			if (global::TNManager.playerName != value)
			{
				global::TNManager.mPlayer.name = value;
				if (global::TNManager.isConnected)
				{
					global::TNManager.mInstance.mClient.playerName = value;
				}
			}
		}
	}

	public static object playerData
	{
		get
		{
			return (!global::TNManager.isConnected) ? global::TNManager.mPlayer.data : global::TNManager.mInstance.mClient.playerData;
		}
		set
		{
			global::TNManager.mPlayer.data = value;
			if (global::TNManager.isConnected)
			{
				global::TNManager.mInstance.mClient.playerData = value;
			}
		}
	}

	public static global::TNet.DataNode playerDataNode
	{
		get
		{
			global::TNet.DataNode dataNode = global::TNManager.playerData as global::TNet.DataNode;
			if (dataNode == null)
			{
				dataNode = new global::TNet.DataNode("Version", 11);
				global::TNManager.playerData = dataNode;
			}
			return dataNode;
		}
	}

	public static void SyncPlayerData()
	{
		if (global::TNManager.isConnected)
		{
			global::TNManager.mInstance.mClient.SyncPlayerData();
		}
	}

	public static global::TNet.List<global::TNet.Player> players
	{
		get
		{
			if (global::TNManager.isConnected)
			{
				return global::TNManager.mInstance.mClient.players;
			}
			if (global::TNManager.mPlayers == null)
			{
				global::TNManager.mPlayers = new global::TNet.List<global::TNet.Player>();
			}
			return global::TNManager.mPlayers;
		}
	}

	public static global::TNet.Player player
	{
		get
		{
			return (!global::TNManager.isConnected) ? global::TNManager.mPlayer : global::TNManager.mInstance.mClient.player;
		}
	}

	private static global::TNManager instance
	{
		get
		{
			if (global::TNManager.mInstance == null)
			{
				global::UnityEngine.GameObject gameObject = new global::UnityEngine.GameObject("Network Manager");
				global::TNManager.mInstance = gameObject.AddComponent<global::TNManager>();
			}
			return global::TNManager.mInstance;
		}
	}

	public static global::TNet.Player GetPlayer(int id)
	{
		if (global::TNManager.isConnected)
		{
			return global::TNManager.mInstance.mClient.GetPlayer(id);
		}
		if (id == global::TNManager.mPlayer.id)
		{
			return global::TNManager.mPlayer;
		}
		return null;
	}

	public static void SetPacketHandler(byte packetID, global::TNet.GameClient.OnPacket callback)
	{
		global::TNManager.instance.mClient.packetHandlers[packetID] = callback;
	}

	public static void SetPacketHandler(global::TNet.Packet packet, global::TNet.GameClient.OnPacket callback)
	{
		global::TNManager.instance.mClient.packetHandlers[(byte)packet] = callback;
	}

	public static bool StartUDP(int udpPort)
	{
		return global::TNManager.instance.mClient.StartUDP(udpPort);
	}

	public static void StopUDP()
	{
		if (global::TNManager.mInstance != null)
		{
			global::TNManager.mInstance.mClient.StopUDP();
		}
	}

	public static void Ping(global::System.Net.IPEndPoint udpEndPoint, global::TNet.GameClient.OnPing callback)
	{
		global::TNManager.instance.mClient.Ping(udpEndPoint, callback);
	}

	public static void Connect(global::System.Net.IPEndPoint externalIP, global::System.Net.IPEndPoint internalIP)
	{
		global::TNManager.instance.mClient.Disconnect();
		global::TNManager.instance.mClient.playerName = global::TNManager.mPlayer.name;
		global::TNManager.instance.mClient.playerData = global::TNManager.mPlayer.data;
		global::TNManager.instance.mClient.Connect(externalIP, internalIP);
	}

	public static void Connect(string address, int port)
	{
		global::System.Net.IPEndPoint ipendPoint = global::TNet.Tools.ResolveEndPoint(address, port);
		if (ipendPoint == null)
		{
			global::TNManager.instance.OnConnect(false, "Unable to resolve [" + address + "]");
		}
		else
		{
			global::TNManager.instance.mClient.playerName = global::TNManager.mPlayer.name;
			global::TNManager.instance.mClient.playerData = global::TNManager.mPlayer.data;
			global::TNManager.instance.mClient.Connect(ipendPoint, null);
		}
	}

	public static void Connect(string address)
	{
		string[] array = address.Split(new char[]
		{
			':'
		});
		int port = 5127;
		if (array.Length > 1)
		{
			int.TryParse(array[1], out port);
		}
		global::TNManager.Connect(array[0], port);
	}

	public static void Disconnect()
	{
		if (global::TNManager.mInstance != null)
		{
			global::TNManager.mInstance.mClient.Disconnect();
		}
	}

	public static void JoinChannel(int channelID, string levelName)
	{
		if (global::TNManager.mInstance != null)
		{
			global::TNManager.mInstance.mClient.JoinChannel(channelID, levelName, false, 65535, null);
		}
		else
		{
			global::UnityEngine.Application.LoadLevel(levelName);
		}
	}

	public static void JoinChannel(int channelID, string levelName, bool persistent, int playerLimit, string password)
	{
		if (global::TNManager.mInstance != null)
		{
			global::TNManager.mInstance.mClient.JoinChannel(channelID, levelName, persistent, playerLimit, password);
		}
		else
		{
			global::UnityEngine.Application.LoadLevel(levelName);
		}
	}

	public static void JoinRandomChannel(string levelName, bool persistent, int playerLimit, string password)
	{
		if (global::TNManager.mInstance != null)
		{
			global::TNManager.mInstance.mClient.JoinChannel(-2, levelName, persistent, playerLimit, password);
		}
	}

	public static void CreateChannel(string levelName, bool persistent, int playerLimit, string password)
	{
		if (global::TNManager.mInstance != null)
		{
			global::TNManager.mInstance.mClient.JoinChannel(-1, levelName, persistent, playerLimit, password);
		}
		else
		{
			global::UnityEngine.Application.LoadLevel(levelName);
		}
	}

	public static void CloseChannel()
	{
		if (global::TNManager.mInstance != null)
		{
			global::TNManager.mInstance.mClient.CloseChannel();
		}
	}

	public static void LeaveChannel()
	{
		if (global::TNManager.mInstance != null)
		{
			global::TNManager.mInstance.mClient.LeaveChannel();
		}
	}

	public static void SetPlayerLimit(int max)
	{
		if (global::TNManager.mInstance != null)
		{
			global::TNManager.mInstance.mClient.SetPlayerLimit(max);
		}
	}

	public static void LoadLevel(string levelName)
	{
		if (global::TNManager.isConnected)
		{
			global::TNManager.mInstance.mClient.LoadLevel(levelName);
		}
		else
		{
			global::UnityEngine.Application.LoadLevel(levelName);
		}
	}

	public static void SaveFile(string filename, byte[] data)
	{
		if (global::TNManager.isConnected)
		{
			global::TNManager.mInstance.mClient.SaveFile(filename, data);
		}
		else
		{
			try
			{
				global::TNet.Tools.WriteFile(filename, data);
			}
			catch (global::System.Exception ex)
			{
				global::UnityEngine.Debug.LogError(ex.Message + " (" + filename + ")");
			}
		}
	}

	public static void LoadFile(string filename, global::TNet.GameClient.OnLoadFile callback)
	{
		if (callback != null)
		{
			if (global::TNManager.isConnected)
			{
				global::TNManager.mInstance.mClient.LoadFile(filename, callback);
			}
			else
			{
				callback(filename, global::TNet.Tools.ReadFile(filename));
			}
		}
	}

	public static void SetHost(global::TNet.Player player)
	{
		if (global::TNManager.mInstance != null)
		{
			global::TNManager.mInstance.mClient.SetHost(player);
		}
	}

	public static void SetTimeout(int seconds)
	{
		if (global::TNManager.mInstance != null)
		{
			global::TNManager.mInstance.mClient.SetTimeout(seconds);
		}
	}

	public static void Create(global::UnityEngine.GameObject go, bool persistent = true)
	{
		global::TNManager.CreateEx(0, persistent, go, new object[0]);
	}

	public static void Create(string path, bool persistent = true)
	{
		global::TNManager.CreateEx(0, persistent, path, new object[0]);
	}

	public static void Create(global::UnityEngine.GameObject go, global::UnityEngine.Vector3 pos, global::UnityEngine.Quaternion rot, bool persistent = true)
	{
		global::TNManager.CreateEx(1, persistent, go, new object[]
		{
			pos,
			rot
		});
	}

	public static void Create(string path, global::UnityEngine.Vector3 pos, global::UnityEngine.Quaternion rot, bool persistent = true)
	{
		global::TNManager.CreateEx(1, persistent, path, new object[]
		{
			pos,
			rot
		});
	}

	public static void Create(global::UnityEngine.GameObject go, global::UnityEngine.Vector3 pos, global::UnityEngine.Quaternion rot, global::UnityEngine.Vector3 vel, global::UnityEngine.Vector3 angVel, bool persistent = true)
	{
		global::TNManager.CreateEx(2, persistent, go, new object[]
		{
			pos,
			rot,
			vel,
			angVel
		});
	}

	public static void Create(string path, global::UnityEngine.Vector3 pos, global::UnityEngine.Quaternion rot, global::UnityEngine.Vector3 vel, global::UnityEngine.Vector3 angVel, bool persistent = true)
	{
		global::TNManager.CreateEx(2, persistent, path, new object[]
		{
			pos,
			rot,
			vel,
			angVel
		});
	}

	public static void CreateEx(int rccID, bool persistent, global::UnityEngine.GameObject go, params object[] objs)
	{
		if (go != null)
		{
			int num = global::TNManager.IndexOf(go);
			if (global::TNManager.isConnected)
			{
				if (num != -1)
				{
					global::System.IO.BinaryWriter binaryWriter = global::TNManager.mInstance.mClient.BeginSend(global::TNet.Packet.RequestCreate);
					binaryWriter.Write((ushort)num);
					binaryWriter.Write(global::TNManager.GetFlag(go, persistent));
					binaryWriter.Write((byte)rccID);
					binaryWriter.WriteArray(objs);
					global::TNManager.EndSend();
					return;
				}
				global::UnityEngine.Debug.LogError("\"" + go.name + "\" has not been added to TNManager's list of objects, so it cannot be instantiated.\nConsider placing it into the Resources folder and passing its name instead.", go);
			}
			objs = global::BinaryExtensions.CombineArrays(go, objs);
			global::TNet.UnityTools.ExecuteAll(global::TNManager.GetRCCs(), (byte)rccID, objs);
			global::TNet.UnityTools.Clear(objs);
		}
	}

	public static void CreateEx(int rccID, bool persistent, string path, params object[] objs)
	{
		global::UnityEngine.GameObject gameObject = global::TNManager.LoadGameObject(path);
		if (gameObject != null)
		{
			if (global::TNManager.isConnected)
			{
				global::System.IO.BinaryWriter binaryWriter = global::TNManager.mInstance.mClient.BeginSend(global::TNet.Packet.RequestCreate);
				byte flag = global::TNManager.GetFlag(gameObject, persistent);
				binaryWriter.Write(ushort.MaxValue);
				binaryWriter.Write(flag);
				binaryWriter.Write(path);
				binaryWriter.Write((byte)rccID);
				binaryWriter.WriteArray(objs);
				global::TNManager.EndSend();
				return;
			}
			objs = global::BinaryExtensions.CombineArrays(gameObject, objs);
			global::TNet.UnityTools.ExecuteAll(global::TNManager.GetRCCs(), (byte)rccID, objs);
			global::TNet.UnityTools.Clear(objs);
		}
		else
		{
			global::UnityEngine.Debug.LogError("Unable to load " + path);
		}
	}

	public static global::TNet.List<global::TNet.CachedFunc> GetRCCs()
	{
		if (global::TNManager.rebuildMethodList)
		{
			global::TNManager.rebuildMethodList = false;
			if (global::TNManager.mInstance != null)
			{
				global::UnityEngine.MonoBehaviour[] componentsInChildren = global::TNManager.mInstance.GetComponentsInChildren<global::UnityEngine.MonoBehaviour>();
				int i = 0;
				int num = componentsInChildren.Length;
				while (i < num)
				{
					global::UnityEngine.MonoBehaviour monoBehaviour = componentsInChildren[i];
					global::TNManager.AddRCCs(monoBehaviour, monoBehaviour.GetType());
					i++;
				}
			}
			else
			{
				global::TNManager.AddRCCs<global::TNManager>();
			}
		}
		return global::TNManager.mRCCs;
	}

	public static void AddRCCs(global::UnityEngine.MonoBehaviour mb)
	{
		global::TNManager.AddRCCs(mb, mb.GetType());
	}

	public static void AddRCCs<T>()
	{
		global::TNManager.AddRCCs(null, typeof(T));
	}

	private static void AddRCCs(object obj, global::System.Type type)
	{
		global::System.Reflection.MethodInfo[] methods = type.GetMethods(global::System.Reflection.BindingFlags.Instance | global::System.Reflection.BindingFlags.Static | global::System.Reflection.BindingFlags.Public | global::System.Reflection.BindingFlags.NonPublic);
		for (int i = 0; i < methods.Length; i++)
		{
			if (methods[i].IsDefined(typeof(global::TNet.RCC), true))
			{
				global::TNet.RCC rcc = (global::TNet.RCC)methods[i].GetCustomAttributes(typeof(global::TNet.RCC), true)[0];
				for (int j = 0; j < global::TNManager.mRCCs.size; j++)
				{
					global::TNet.CachedFunc cachedFunc = global::TNManager.mRCCs[j];
					if (cachedFunc.id == rcc.id)
					{
						cachedFunc.obj = obj;
						cachedFunc.func = methods[i];
						return;
					}
				}
				global::TNet.CachedFunc item = default(global::TNet.CachedFunc);
				item.obj = obj;
				item.func = methods[i];
				item.id = rcc.id;
				global::TNManager.mRCCs.Add(item);
			}
		}
	}

	private static void RemoveRCC(int rccID)
	{
		for (int i = 0; i < global::TNManager.mRCCs.size; i++)
		{
			if ((int)global::TNManager.mRCCs[i].id == rccID)
			{
				global::TNManager.mRCCs.RemoveAt(i);
				return;
			}
		}
	}

	private static void RemoveRCCs<T>()
	{
		global::System.Reflection.MethodInfo[] methods = typeof(T).GetMethods(global::System.Reflection.BindingFlags.Instance | global::System.Reflection.BindingFlags.Static | global::System.Reflection.BindingFlags.Public | global::System.Reflection.BindingFlags.NonPublic);
		for (int i = 0; i < methods.Length; i++)
		{
			if (methods[i].IsDefined(typeof(global::TNet.RCC), true))
			{
				global::TNet.RCC rcc = (global::TNet.RCC)methods[i].GetCustomAttributes(typeof(global::TNet.RCC), true)[0];
				global::TNManager.RemoveRCC((int)rcc.id);
			}
		}
	}

	[global::TNet.RCC(0)]
	private static global::UnityEngine.GameObject OnCreate0(global::UnityEngine.GameObject go)
	{
		return global::UnityEngine.Object.Instantiate<global::UnityEngine.GameObject>(go);
	}

	[global::TNet.RCC(1)]
	private static global::UnityEngine.GameObject OnCreate1(global::UnityEngine.GameObject go, global::UnityEngine.Vector3 pos, global::UnityEngine.Quaternion rot)
	{
		return global::UnityEngine.Object.Instantiate(go, pos, rot) as global::UnityEngine.GameObject;
	}

	[global::TNet.RCC(2)]
	private static global::UnityEngine.GameObject OnCreate2(global::UnityEngine.GameObject go, global::UnityEngine.Vector3 pos, global::UnityEngine.Quaternion rot, global::UnityEngine.Vector3 velocity, global::UnityEngine.Vector3 angularVelocity)
	{
		return global::TNet.UnityTools.Instantiate(go, pos, rot, velocity, angularVelocity);
	}

	public static void Destroy(global::UnityEngine.GameObject go)
	{
		if (global::TNManager.isConnected)
		{
			global::TNObject component = go.GetComponent<global::TNObject>();
			if (component != null)
			{
				global::System.IO.BinaryWriter binaryWriter = global::TNManager.mInstance.mClient.BeginSend(global::TNet.Packet.RequestDestroy);
				binaryWriter.Write(component.uid);
				global::TNManager.mInstance.mClient.EndSend();
				return;
			}
		}
		global::UnityEngine.Object.Destroy(go);
	}

	public static global::System.IO.BinaryWriter BeginSend(global::TNet.Packet type)
	{
		return global::TNManager.mInstance.mClient.BeginSend(type);
	}

	public static global::System.IO.BinaryWriter BeginSend(byte packetID)
	{
		return global::TNManager.mInstance.mClient.BeginSend(packetID);
	}

	public static void EndSend()
	{
		global::TNManager.mInstance.mClient.EndSend(true);
	}

	public static void EndSend(bool reliable)
	{
		global::TNManager.mInstance.mClient.EndSend(reliable);
	}

	public static void EndSend(int port)
	{
		global::TNManager.mInstance.mClient.EndSend(port);
	}

	public static void EndSend(global::System.Net.IPEndPoint target)
	{
		global::TNManager.mInstance.mClient.EndSend(target);
	}

	private void Awake()
	{
		if (global::TNManager.mInstance != null)
		{
			global::UnityEngine.Object.Destroy(base.gameObject);
		}
		else
		{
			global::TNManager.mInstance = this;
			global::TNManager.rebuildMethodList = true;
			global::UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			global::TNet.GameClient gameClient = this.mClient;
			gameClient.onError = (global::TNet.GameClient.OnError)global::System.Delegate.Combine(gameClient.onError, new global::TNet.GameClient.OnError(this.OnError));
			global::TNet.GameClient gameClient2 = this.mClient;
			gameClient2.onConnect = (global::TNet.GameClient.OnConnect)global::System.Delegate.Combine(gameClient2.onConnect, new global::TNet.GameClient.OnConnect(this.OnConnect));
			global::TNet.GameClient gameClient3 = this.mClient;
			gameClient3.onDisconnect = (global::TNet.GameClient.OnDisconnect)global::System.Delegate.Combine(gameClient3.onDisconnect, new global::TNet.GameClient.OnDisconnect(this.OnDisconnect));
			global::TNet.GameClient gameClient4 = this.mClient;
			gameClient4.onJoinChannel = (global::TNet.GameClient.OnJoinChannel)global::System.Delegate.Combine(gameClient4.onJoinChannel, new global::TNet.GameClient.OnJoinChannel(this.OnJoinChannel));
			global::TNet.GameClient gameClient5 = this.mClient;
			gameClient5.onLeftChannel = (global::TNet.GameClient.OnLeftChannel)global::System.Delegate.Combine(gameClient5.onLeftChannel, new global::TNet.GameClient.OnLeftChannel(this.OnLeftChannel));
			global::TNet.GameClient gameClient6 = this.mClient;
			gameClient6.onLoadLevel = (global::TNet.GameClient.OnLoadLevel)global::System.Delegate.Combine(gameClient6.onLoadLevel, new global::TNet.GameClient.OnLoadLevel(this.OnLoadLevel));
			global::TNet.GameClient gameClient7 = this.mClient;
			gameClient7.onPlayerJoined = (global::TNet.GameClient.OnPlayerJoined)global::System.Delegate.Combine(gameClient7.onPlayerJoined, new global::TNet.GameClient.OnPlayerJoined(this.OnPlayerJoined));
			global::TNet.GameClient gameClient8 = this.mClient;
			gameClient8.onPlayerLeft = (global::TNet.GameClient.OnPlayerLeft)global::System.Delegate.Combine(gameClient8.onPlayerLeft, new global::TNet.GameClient.OnPlayerLeft(this.OnPlayerLeft));
			global::TNet.GameClient gameClient9 = this.mClient;
			gameClient9.onRenamePlayer = (global::TNet.GameClient.OnRenamePlayer)global::System.Delegate.Combine(gameClient9.onRenamePlayer, new global::TNet.GameClient.OnRenamePlayer(this.OnRenamePlayer));
			global::TNet.GameClient gameClient10 = this.mClient;
			gameClient10.onCreate = (global::TNet.GameClient.OnCreate)global::System.Delegate.Combine(gameClient10.onCreate, new global::TNet.GameClient.OnCreate(this.OnCreateObject));
			global::TNet.GameClient gameClient11 = this.mClient;
			gameClient11.onDestroy = (global::TNet.GameClient.OnDestroy)global::System.Delegate.Combine(gameClient11.onDestroy, new global::TNet.GameClient.OnDestroy(this.OnDestroyObject));
			global::TNet.GameClient gameClient12 = this.mClient;
			gameClient12.onForwardedPacket = (global::TNet.GameClient.OnForwardedPacket)global::System.Delegate.Combine(gameClient12.onForwardedPacket, new global::TNet.GameClient.OnForwardedPacket(this.OnForwardedPacket));
		}
	}

	private void OnDestroy()
	{
		if (global::TNManager.mInstance == this)
		{
			if (global::TNManager.isConnected)
			{
				this.mClient.Disconnect();
			}
			this.mClient.StopUDP();
			global::TNManager.mInstance = null;
		}
	}

	public static int IndexOf(global::UnityEngine.GameObject go)
	{
		if (go != null && global::TNManager.mInstance != null && global::TNManager.mInstance.objects != null)
		{
			int i = 0;
			int num = global::TNManager.mInstance.objects.Length;
			while (i < num)
			{
				if (global::TNManager.mInstance.objects[i] == go)
				{
					return i;
				}
				i++;
			}
			global::UnityEngine.Debug.LogError("The game object was not found in the TNManager's list of objects. Did you forget to add it?", go);
		}
		return -1;
	}

	private static global::UnityEngine.GameObject LoadGameObject(string path)
	{
		global::UnityEngine.GameObject gameObject = global::UnityEngine.Resources.Load(path, typeof(global::UnityEngine.GameObject)) as global::UnityEngine.GameObject;
		if (gameObject == null)
		{
			global::UnityEngine.Debug.LogError("Attempting to create a game object that can't be found in the Resources folder: " + path);
		}
		return gameObject;
	}

	private static byte GetFlag(global::UnityEngine.GameObject go, bool persistent)
	{
		global::TNObject component = go.GetComponent<global::TNObject>();
		if (component == null)
		{
			return 0;
		}
		return (!persistent) ? 2 : 1;
	}

	private void OnCreateObject(int creator, int index, uint objectID, global::System.IO.BinaryReader reader)
	{
		global::TNManager.mObjectOwner = creator;
		global::UnityEngine.GameObject gameObject;
		if (index == 65535)
		{
			gameObject = global::TNManager.LoadGameObject(reader.ReadString());
		}
		else
		{
			if (index < 0 || index >= this.objects.Length)
			{
				global::UnityEngine.Debug.LogError("Attempting to create an invalid object. Index: " + index);
				return;
			}
			gameObject = this.objects[index];
		}
		gameObject = global::TNManager.CreateGameObject(gameObject, reader);
		if (gameObject != null && objectID != 0U)
		{
			global::TNObject component = gameObject.GetComponent<global::TNObject>();
			if (component != null)
			{
				component.uid = objectID;
				component.Register();
			}
			else
			{
				global::UnityEngine.Debug.LogWarning("The instantiated object has no TNObject component. Don't request an ObjectID when creating it.", gameObject);
			}
		}
	}

	private static global::UnityEngine.GameObject CreateGameObject(global::UnityEngine.GameObject prefab, global::System.IO.BinaryReader reader)
	{
		if (!(prefab != null))
		{
			return null;
		}
		byte b = reader.ReadByte();
		if (b == 0)
		{
			return global::UnityEngine.Object.Instantiate<global::UnityEngine.GameObject>(prefab);
		}
		object[] array = reader.ReadArray(prefab);
		object obj;
		global::TNet.UnityTools.ExecuteFirst(global::TNManager.GetRCCs(), b, out obj, array);
		global::TNet.UnityTools.Clear(array);
		if (obj == null)
		{
			global::UnityEngine.Debug.LogError("Instantiating \"" + prefab.name + "\" returned null. Did you forget to return the game object from your RCC?");
		}
		return obj as global::UnityEngine.GameObject;
	}

	private void OnDestroyObject(uint objID)
	{
		global::TNObject tnobject = global::TNObject.Find(objID);
		if (tnobject)
		{
			global::UnityEngine.Object.Destroy(tnobject.gameObject);
		}
	}

	private void OnForwardedPacket(global::System.IO.BinaryReader reader)
	{
		uint objID;
		byte b;
		global::TNObject.DecodeUID(reader.ReadUInt32(), out objID, out b);
		if (b == 0)
		{
			global::TNObject.FindAndExecute(objID, reader.ReadString(), reader.ReadArray());
		}
		else
		{
			global::TNObject.FindAndExecute(objID, b, reader.ReadArray());
		}
	}

	private void Update()
	{
		this.mClient.ProcessPackets();
	}

	private void OnError(string err)
	{
		global::TNet.UnityTools.Broadcast("OnNetworkError", new object[]
		{
			err
		});
	}

	private void OnConnect(bool success, string message)
	{
		global::TNet.UnityTools.Broadcast("OnNetworkConnect", new object[]
		{
			success,
			message
		});
	}

	private void OnDisconnect()
	{
		global::TNet.UnityTools.Broadcast("OnNetworkDisconnect", new object[0]);
	}

	private void OnJoinChannel(bool success, string message)
	{
		global::TNet.UnityTools.Broadcast("OnNetworkJoinChannel", new object[]
		{
			success,
			message
		});
	}

	private void OnLeftChannel()
	{
		global::TNet.UnityTools.Broadcast("OnNetworkLeaveChannel", new object[0]);
	}

	private void OnLoadLevel(string levelName)
	{
		if (!string.IsNullOrEmpty(levelName))
		{
			global::UnityEngine.Application.LoadLevel(levelName);
		}
	}

	private void OnPlayerJoined(global::TNet.Player p)
	{
		global::TNet.UnityTools.Broadcast("OnNetworkPlayerJoin", new object[]
		{
			p
		});
	}

	private void OnPlayerLeft(global::TNet.Player p)
	{
		global::TNet.UnityTools.Broadcast("OnNetworkPlayerLeave", new object[]
		{
			p
		});
	}

	private void OnRenamePlayer(global::TNet.Player p, string previous)
	{
		global::TNManager.mPlayer.name = p.name;
		global::TNet.UnityTools.Broadcast("OnNetworkPlayerRenamed", new object[]
		{
			p,
			previous
		});
	}

	public static bool rebuildMethodList = true;

	private static global::TNet.List<global::TNet.CachedFunc> mRCCs = new global::TNet.List<global::TNet.CachedFunc>();

	private static global::TNet.Player mPlayer = new global::TNet.Player("Guest");

	private static global::TNet.List<global::TNet.Player> mPlayers;

	private static global::TNManager mInstance;

	private static int mObjectOwner = 1;

	public global::UnityEngine.GameObject[] objects;

	private global::TNet.GameClient mClient = new global::TNet.GameClient();
}
