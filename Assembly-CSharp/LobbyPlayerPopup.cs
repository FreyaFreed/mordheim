using System;
using UnityEngine.UI;

public class LobbyPlayerPopup : global::PandoraSingleton<global::LobbyPlayerPopup>
{
	private void Awake()
	{
		if (global::PandoraSingleton<global::LobbyPlayerPopup>.instance == null)
		{
			global::PandoraSingleton<global::LobbyPlayerPopup>.instance = this;
		}
	}

	private void Start()
	{
		if (!this.isShow)
		{
			base.gameObject.SetActive(false);
		}
	}

	public void Show(string player, string textId)
	{
		base.gameObject.SetActive(true);
		this.isShow = true;
		if (string.IsNullOrEmpty(player))
		{
			this.playerName.enabled = false;
		}
		else
		{
			this.playerName.enabled = true;
			this.playerName.text = player;
		}
		this.text.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(textId);
	}

	private void OnDisable()
	{
		if (this.isShow)
		{
			this.isShow = false;
		}
	}

	public global::UnityEngine.UI.Text playerName;

	public global::UnityEngine.UI.Text text;

	private bool isShow;
}
