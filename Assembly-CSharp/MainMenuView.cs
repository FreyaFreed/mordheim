using System;
using UnityEngine;

public class MainMenuView : global::UnityEngine.MonoBehaviour
{
	private void Awake()
	{
		this.mainView.gameObject.SetActive(false);
		this.gameModesView.gameObject.SetActive(false);
		this.creditsView.gameObject.SetActive(false);
		this.OnMain();
	}

	public void LoadCampaign()
	{
	}

	public void OnGameModes()
	{
		this.Show(this.gameModesView);
	}

	public void OnMain()
	{
		this.Show(this.mainView);
	}

	public void OnCredits()
	{
		this.Show(this.creditsView);
	}

	private void Show(global::UnityEngine.RectTransform toShow)
	{
		if (this._current != null)
		{
			this._current.gameObject.SetActive(false);
		}
		this._current = toShow;
		this._current.gameObject.SetActive(true);
		this._current.localPosition = global::UnityEngine.Vector3.zero;
	}

	public global::UnityEngine.RectTransform mainView;

	public global::UnityEngine.RectTransform gameModesView;

	public global::UnityEngine.RectTransform creditsView;

	private global::UnityEngine.RectTransform _current;
}
