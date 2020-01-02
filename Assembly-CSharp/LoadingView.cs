using System;
using UnityEngine;
using UnityEngine.UI;

public class LoadingView : global::UnityEngine.MonoBehaviour
{
	public virtual void Show()
	{
		base.gameObject.SetActive(true);
	}

	protected void LoadDialog(string soundName)
	{
		this.dialogName = soundName;
	}

	public void PlayDialog()
	{
		if (!string.IsNullOrEmpty(this.dialogName))
		{
			global::PandoraSingleton<global::Pan>.Instance.GetSound("voices/loading/english/", this.dialogName, false, delegate(global::UnityEngine.AudioClip clip)
			{
				this.audioSrc.clip = clip;
				this.audioSrc.Play();
			});
			this.dialogName = string.Empty;
		}
	}

	protected void LoadBackground(string name)
	{
		this.background.sprite = global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadAsset<global::UnityEngine.Sprite>("Assets/gui/assets/loading/bg/", global::AssetBundleId.LOADING, name + ".png");
	}

	public global::SceneLoadingTypeId id;

	public global::UnityEngine.UI.Image background;

	public global::UnityEngine.AudioSource audioSrc;

	private string dialogName;
}
