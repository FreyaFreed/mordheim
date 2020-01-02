using System;
using TNet;
using UnityEngine;

public abstract class TNLobbyClient : global::UnityEngine.MonoBehaviour
{
	protected virtual void OnDisable()
	{
		global::TNLobbyClient.errorString = string.Empty;
		global::TNLobbyClient.knownServers.Clear();
	}

	public static string errorString = string.Empty;

	public static global::TNet.ServerList knownServers = new global::TNet.ServerList();

	public static global::TNLobbyClient.OnListChange onChange;

	public static bool isActive = false;

	public string remoteAddress;

	public int remotePort = 5129;

	public delegate void OnListChange();
}
