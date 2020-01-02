using System;
using UnityEngine;
using UnityEngine.UI;

public class LobbyPlayersModule : global::UIModule
{
	public void RefreshPlayers()
	{
		bool flag = global::PandoraSingleton<global::Hermes>.Instance.IsHost();
		this.pressReadyMessage.SetActive(!flag);
		if (global::PandoraSingleton<global::MissionStartData>.Instance.FightingWarbands.Count > 1)
		{
			this.player1.gameObject.SetActive(true);
			this.player1.SetPlayerInfo((!flag) ? 1 : 0);
			this.player2.gameObject.SetActive(true);
			this.player2.SetPlayerInfo((!flag) ? 0 : 1);
			this.empty.gameObject.SetActive(false);
		}
		else if (global::PandoraSingleton<global::MissionStartData>.Instance.FightingWarbands.Count == 1)
		{
			this.player1.gameObject.SetActive(true);
			this.player1.SetPlayerInfo(0);
			this.player2.gameObject.SetActive(false);
			this.empty.gameObject.SetActive(true);
		}
		else
		{
			this.player1.gameObject.SetActive(false);
			this.player2.gameObject.SetActive(false);
			this.empty.gameObject.SetActive(false);
		}
	}

	public void SetErrorMessage(string text)
	{
		if (string.IsNullOrEmpty(text))
		{
			this.errorMessage.SetActive(false);
		}
		else
		{
			this.errorMessage.SetActive(true);
			this.errorMessageText.text = text;
		}
	}

	public global::SkirmishLobbyPlayer player1;

	public global::SkirmishLobbyPlayer player2;

	public global::SkirmishLobbyPlayer empty;

	public global::UnityEngine.GameObject pressReadyMessage;

	public global::UnityEngine.GameObject errorMessage;

	public global::UnityEngine.UI.Text errorMessageText;
}
