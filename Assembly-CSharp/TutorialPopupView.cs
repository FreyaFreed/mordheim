using System;
using UnityEngine;
using UnityEngine.UI;

public class TutorialPopupView : global::ConfirmationPopupView
{
	protected override void Awake()
	{
		base.Awake();
		base.gameObject.SetActive(false);
		global::PandoraSingleton<global::NoticeManager>.Instance.RegisterListener(global::Notices.GAME_TUTO_MESSAGE, new global::DelReceiveNotice(this.ShowMessage));
	}

	public void ShowMessage()
	{
		bool flag = (bool)global::PandoraSingleton<global::NoticeManager>.Instance.Parameters[0];
		string text = (string)global::PandoraSingleton<global::NoticeManager>.Instance.Parameters[1];
		if (flag)
		{
			base.gameObject.SetActive(true);
			this.Show(text.Replace("_console", string.Empty).Replace("message", "title"), text, new global::System.Action<bool>(this.OnTutorialConfirm), false, false);
			global::UnityEngine.Sprite sprite = global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadAsset<global::UnityEngine.Sprite>("Assets/gui/assets/tutorial/tutorial/pc/", global::AssetBundleId.LOADING, text.Replace("message", "image").Replace("_console", string.Empty) + ".png");
			if (sprite != null)
			{
				this.image.gameObject.SetActive(true);
				this.image.overrideSprite = sprite;
			}
			else
			{
				this.image.overrideSprite = null;
				this.image.gameObject.SetActive(false);
			}
		}
		else
		{
			this.Hide();
		}
	}

	private void OnTutorialConfirm(bool obj)
	{
	}

	public override void Show(string titleId, string textId, global::System.Action<bool> callback, bool hideButtons = false, bool hideCancel = false)
	{
		base.Show(callback, hideButtons, true);
		if (!string.IsNullOrEmpty(titleId))
		{
			this.title.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(titleId);
		}
		if (!string.IsNullOrEmpty(textId))
		{
			string stringById = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(textId);
			this.text.text = global::PandoraSingleton<global::LocalizationManager>.Instance.ReplaceAllActionsWithButtonName(stringById);
		}
		this.confirmButton.SetAction("action", "menu_confirm", 1, false, null, null);
		this.confirmButton.SetInteractable(false);
		this.confirmButton.OnAction(null, false, true);
	}

	public global::UnityEngine.UI.Image image;
}
