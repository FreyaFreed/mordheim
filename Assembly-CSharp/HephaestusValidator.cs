using System;
using UnityEngine;

public class HephaestusValidator : global::UnityEngine.MonoBehaviour
{
	private void OnGUI()
	{
		global::UnityEngine.GUILayout.BeginArea(new global::UnityEngine.Rect(25f, 25f, 300f, 700f));
		if (global::PandoraSingleton<global::Hephaestus>.Instance.Lobby != null)
		{
			global::UnityEngine.GUILayout.Label("LobbyID = " + global::PandoraSingleton<global::Hephaestus>.Instance.Lobby.id, new global::UnityEngine.GUILayoutOption[0]);
			if (global::UnityEngine.GUILayout.Button("Delete Lobby", new global::UnityEngine.GUILayoutOption[0]))
			{
				global::PandoraSingleton<global::Hephaestus>.Instance.LeaveLobby();
			}
			if (global::UnityEngine.GUILayout.Button("Invite Friends", new global::UnityEngine.GUILayoutOption[0]))
			{
				global::PandoraSingleton<global::Hephaestus>.Instance.InviteFriends();
			}
		}
		else
		{
			global::UnityEngine.GUILayout.Label("No Lobby Created", new global::UnityEngine.GUILayoutOption[0]);
			if (global::UnityEngine.GUILayout.Button("Create Lobby", new global::UnityEngine.GUILayoutOption[0]))
			{
				global::PandoraSingleton<global::Hephaestus>.Instance.CreateLobby("Testing12", global::Hephaestus.LobbyPrivacy.PUBLIC, new global::Hephaestus.OnLobbyCreatedCallback(this.OnLobbyEnteredCallback));
			}
			if (!this.searching && global::UnityEngine.GUILayout.Button("Find Lobby", new global::UnityEngine.GUILayoutOption[0]))
			{
				this.searching = true;
				global::PandoraSingleton<global::Hephaestus>.Instance.SearchLobbies(new global::Hephaestus.OnSearchLobbiesCallback(this.OnSearchLobbiesCallback));
			}
			if (!this.searching && global::PandoraSingleton<global::Hephaestus>.Instance.Lobbies.Count > 0)
			{
				for (int i = 0; i < global::PandoraSingleton<global::Hephaestus>.Instance.Lobbies.Count; i++)
				{
					if (global::UnityEngine.GUILayout.Button(global::PandoraSingleton<global::Hephaestus>.Instance.Lobbies[i].name, new global::UnityEngine.GUILayoutOption[0]))
					{
					}
				}
			}
		}
		global::UnityEngine.GUILayout.EndArea();
	}

	private void OnLobbyEnteredCallback(ulong lobbyId, bool success)
	{
	}

	private void OnSearchLobbiesCallback()
	{
		this.searching = false;
	}

	private void OnJoinLobbyCallback(bool success)
	{
	}

	private void OnLobbyUpdate()
	{
	}

	private ulong lobbyId;

	private bool searching;
}
