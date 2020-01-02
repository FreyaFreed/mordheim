using System;
using TNet;
using UnityEngine;

[global::UnityEngine.RequireComponent(typeof(global::TNManager))]
public class TNAutoJoin : global::UnityEngine.MonoBehaviour
{
	private void Awake()
	{
		if (global::TNAutoJoin.instance == null)
		{
			global::TNAutoJoin.instance = this;
		}
	}

	private void Start()
	{
		if (this.connectOnStart)
		{
			this.Connect();
		}
	}

	public void Connect()
	{
		global::UnityEngine.Screen.sleepTimeout = -1;
		global::TNManager.Connect(this.serverAddress, this.serverPort);
	}

	private void OnNetworkConnect(bool result, string message)
	{
		if (result)
		{
			if (this.allowUDP)
			{
				global::TNManager.StartUDP(global::UnityEngine.Random.Range(10000, 50000));
			}
			global::TNManager.JoinChannel(this.channelID, this.firstLevel);
		}
		else if (!string.IsNullOrEmpty(this.failureFunctionName))
		{
			global::TNet.UnityTools.Broadcast(this.failureFunctionName, new object[]
			{
				message
			});
		}
		else
		{
			global::UnityEngine.Debug.LogError(message);
		}
	}

	private void OnNetworkDisconnect()
	{
		if (!string.IsNullOrEmpty(this.disconnectLevel) && global::UnityEngine.Application.loadedLevelName != this.disconnectLevel)
		{
			global::UnityEngine.Application.LoadLevel(this.disconnectLevel);
		}
	}

	private void OnNetworkJoinChannel(bool result, string message)
	{
		if (result)
		{
			if (!string.IsNullOrEmpty(this.successFunctionName))
			{
				global::TNet.UnityTools.Broadcast(this.successFunctionName, new object[0]);
			}
		}
		else
		{
			if (!string.IsNullOrEmpty(this.failureFunctionName))
			{
				global::TNet.UnityTools.Broadcast(this.failureFunctionName, new object[]
				{
					message
				});
			}
			else
			{
				global::UnityEngine.Debug.LogError(message);
			}
			global::TNManager.Disconnect();
		}
	}

	public static global::TNAutoJoin instance;

	public string serverAddress = "127.0.0.1";

	public int serverPort = 5127;

	public string firstLevel = "Example 1";

	public int channelID = 1;

	public string disconnectLevel;

	public bool allowUDP = true;

	public bool connectOnStart = true;

	public string successFunctionName;

	public string failureFunctionName;
}
