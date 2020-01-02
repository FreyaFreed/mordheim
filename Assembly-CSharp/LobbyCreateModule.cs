using System;
using UnityEngine.Events;
using UnityEngine.UI;

public class LobbyCreateModule : global::UIModule
{
	public void Setup()
	{
		this.createExhibitionGame.SetAction(string.Empty, "menu_skirmish_create_game", 0, false, null, null);
		this.createExhibitionGame.OnAction(new global::UnityEngine.Events.UnityAction(this.CreateExhibitionGame), false, true);
		this.createContestGame.SetAction(string.Empty, "menu_skirmish_create_game", 0, false, null, null);
		this.createContestGame.OnAction(new global::UnityEngine.Events.UnityAction(this.CreateContestGame), false, true);
		this.contestUnavailableText.gameObject.SetActive(false);
		this.createContestGame.SetDisabled(true);
		this.createExhibitionGame.SetDisabled(true);
	}

	private void CreateExhibitionGame()
	{
		this.CreateExhibitionGamePopup(false);
	}

	public void CreateExhibitionGamePopup(bool silent = false)
	{
		if (global::PandoraSingleton<global::PandoraInput>.Instance.CurrentInputLayer != 1000)
		{
			global::PandoraSingleton<global::SkirmishManager>.Instance.OnCreateGame(true, new global::System.Action(this.OnCancelCreateExhibitionGame), silent);
		}
	}

	private void CreateContestGame()
	{
		this.CreateContestGamePopup(false);
	}

	public void CreateContestGamePopup(bool silent = false)
	{
		if (global::PandoraSingleton<global::PandoraInput>.Instance.CurrentInputLayer != 1000)
		{
			global::PandoraSingleton<global::SkirmishManager>.Instance.OnCreateGame(false, new global::System.Action(this.OnCancelCreateContestGame), silent);
		}
	}

	public void OnCancelCreateExhibitionGame()
	{
		this.createExhibitionGame.SetSelected(true);
	}

	public void OnCancelCreateContestGame()
	{
		this.createContestGame.SetSelected(true);
	}

	public void LockContest(string reasonTag)
	{
		this.contestUnavailableText.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(reasonTag);
		this.contestUnavailableText.gameObject.SetActive(true);
		this.createContestGame.SetDisabled(true);
	}

	public global::ButtonGroup createExhibitionGame;

	public global::ButtonGroup createContestGame;

	public global::UnityEngine.UI.Text contestUnavailableText;
}
