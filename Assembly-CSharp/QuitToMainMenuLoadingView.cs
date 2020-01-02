using System;
using UnityEngine.UI;

public class QuitToMainMenuLoadingView : global::LoadingView
{
	private void Awake()
	{
		this.descriptionText.enabled = false;
	}

	public global::UnityEngine.UI.Text descriptionText;
}
