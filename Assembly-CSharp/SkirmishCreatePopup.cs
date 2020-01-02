using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkirmishCreatePopup : global::ConfirmationPopupView
{
	protected override void Awake()
	{
		base.Awake();
		this.lobbyName.transform.parent.GetComponentInChildren<global::ToggleEffects>(true).onSelect.AddListener(new global::UnityEngine.Events.UnityAction(this.OnLobbyNameSelected));
		this.lobbyName.transform.parent.GetComponentInChildren<global::ToggleEffects>(true).onAction.AddListener(new global::UnityEngine.Events.UnityAction(this.OnLobbyNameSelected));
	}

	private void OnLobbyNameSelected()
	{
		if (global::PandoraSingleton<global::PandoraInput>.Instance.lastInputMode != global::PandoraInput.InputMode.JOYSTICK || !global::PandoraSingleton<global::Hephaestus>.Instance.ShowVirtualKeyboard(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("menu_skirmish_create_game"), this.lobbyName.text, 40U, false, new global::Hephaestus.OnVirtualKeyboardCallback(this.OnVirtualKeyboard), this.lobbyPrivacy.selections.Count > 1))
		{
			this.lobbyName.SetSelected(true);
		}
	}

	private void OnVirtualKeyboard(bool success, string newstring)
	{
		if (success)
		{
			this.lobbyName.text = newstring;
		}
	}

	public void Show(string titleId, string textId, bool allowOffline, int rating, global::System.Action<bool> callback, bool hideButtons = false)
	{
		this.lobbyPrivacy.selections.Clear();
		if (global::PandoraSingleton<global::Hephaestus>.Instance.IsOnline())
		{
			for (int i = 0; i < 4; i++)
			{
				if (allowOffline || i != 3)
				{
					string key = "lobby_privacy_" + ((global::Hephaestus.LobbyPrivacy)i).ToLowerString();
					string stringById = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(key);
					this.lobbyPrivacy.SetButtonsVisible(true);
					this.lobbyPrivacy.selections.Add(stringById);
				}
			}
		}
		else
		{
			string key2 = "lobby_privacy_" + global::Hephaestus.LobbyPrivacy.OFFLINE.ToLowerString();
			string stringById2 = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(key2);
			this.lobbyPrivacy.selections.Add(stringById2);
			this.lobbyPrivacy.SetButtonsVisible(false);
		}
		int num = rating % 50;
		int num2 = rating - num;
		int num3 = rating + (50 - num);
		this.ratingMin.selections.Clear();
		this.ratingMax.selections.Clear();
		this.ratingMin.selections.Add("0");
		for (int j = 50; j < 5000; j += 50)
		{
			if (j <= num2)
			{
				this.ratingMin.selections.Add(j.ToConstantString());
			}
			else if (j >= num3)
			{
				this.ratingMax.selections.Add(j.ToConstantString());
			}
		}
		this.ratingMin.SetCurrentSel(global::UnityEngine.Mathf.Max(0, this.ratingMin.selections.Count - 2));
		this.ratingMax.SetCurrentSel(1);
		base.Show(titleId, textId, callback, hideButtons, false);
		this.lobbyPrivacy.SetCurrentSel(0);
		this.lobbyName.enabled = true;
		this.lobbyName.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("lobby_name_default", new string[]
		{
			global::PandoraSingleton<global::Hephaestus>.Instance.GetUserName()
		});
		this.lobbyName.transform.parent.gameObject.GetComponent<global::ToggleEffects>().SetSelected(true);
	}

	public string GetRatingMin()
	{
		return this.ratingMin.selections[this.ratingMin.CurSel];
	}

	public string GetRatingMax()
	{
		return this.ratingMax.selections[this.ratingMax.CurSel];
	}

	public override void Confirm()
	{
		if (this.lobbyName.transform.parent.gameObject != global::UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject && this.lobbyName.gameObject != global::UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject)
		{
			if (this.lobbyName.text == string.Empty)
			{
				this.lobbyName.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("lobby_name_default", new string[]
				{
					global::PandoraSingleton<global::Hephaestus>.Instance.GetUserName()
				});
			}
			base.Confirm();
		}
		else
		{
			this.lobbyName.DeactivateInputField();
		}
	}

	public global::SelectorGroup lobbyPrivacy;

	public global::SelectorGroup ratingMin;

	public global::SelectorGroup ratingMax;

	public global::UnityEngine.UI.InputField lobbyName;
}
