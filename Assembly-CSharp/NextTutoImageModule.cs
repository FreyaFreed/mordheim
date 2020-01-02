using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class NextTutoImageModule : global::UnityEngine.MonoBehaviour
{
	private void Awake()
	{
		for (int i = 0; i < this.tutoSprites.Count; i++)
		{
			this.tutoSprites[i].SetActive(false);
		}
	}

	public void Set(int index, global::System.Action backAction)
	{
		this.backCallback = backAction;
		this.SetSelected(true);
		this.currentTutorialIndex = global::UnityEngine.Mathf.Clamp(index, 0, this.tutoSprites.Count - 1);
		for (int i = 0; i < this.tutoSprites.Count; i++)
		{
			this.tutoSprites[i].SetActive(i == this.currentTutorialIndex);
		}
		string name = this.tutoSprites[this.currentTutorialIndex].gameObject.name;
		this.title.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("tutorial_" + name.Substring(name.Length - 2) + "_title");
		global::UnityEngine.Transform transform = this.tutoSprites[this.currentTutorialIndex].transform;
		for (int j = 0; j < transform.childCount; j++)
		{
			global::UnityEngine.UI.Image component = transform.GetChild(j).GetComponent<global::UnityEngine.UI.Image>();
			if (component != null && component.sprite == null)
			{
				component.sprite = global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadAsset<global::UnityEngine.Sprite>("Assets/gui/assets/tutorial/hideout/pc/", global::AssetBundleId.LOADING, string.Format("{0}_title_{1}.png", name.Replace("tutorial", "tuto"), (j + 1).ToString("00")));
			}
		}
		this.SetMessage(0);
	}

	private string GetPlatformFolder()
	{
		return string.Empty;
	}

	private void SetMessage(int index)
	{
		global::UnityEngine.Transform transform = this.tutoSprites[this.currentTutorialIndex].transform;
		int childCount = transform.childCount;
		global::UnityEngine.Debug.Log(this.currentTutorialIndex + " - " + childCount);
		if (index == childCount)
		{
			this.backCallback();
		}
		else
		{
			this.currentMessageIndex = global::UnityEngine.Mathf.Clamp(index, 0, childCount - 1);
			if (this.currentMessageIndex == childCount - 1)
			{
				global::PandoraSingleton<global::GameManager>.Instance.Profile.CompleteTutorial(this.currentTutorialIndex + global::Constant.GetInt(global::ConstantId.COMBAT_TUTORIALS_COUNT));
				global::PandoraSingleton<global::GameManager>.Instance.SaveProfile();
			}
			for (int i = 0; i < transform.childCount; i++)
			{
				transform.GetChild(i).gameObject.SetActive(i == this.currentMessageIndex);
			}
			this.counter.text = index + 1 + "/" + childCount;
			this.previous.gameObject.SetActive(this.currentMessageIndex != 0);
			this.next.gameObject.SetActive(true);
		}
	}

	public void Setup()
	{
		this.previous.SetAction("h", "controls_action_prev", 0, true, this.prevSprite, null);
		this.previous.OnAction(new global::UnityEngine.Events.UnityAction(this.PrevUnit), false, true);
		this.next.SetAction("h", "controls_action_next", 0, false, this.nextSprite, null);
		this.next.OnAction(new global::UnityEngine.Events.UnityAction(this.NextUnit), false, true);
	}

	private void PrevUnit()
	{
		this.SetMessage(this.currentMessageIndex - 1);
	}

	private void NextUnit()
	{
		this.SetMessage(this.currentMessageIndex + 1);
	}

	public global::System.Collections.Generic.List<global::UnityEngine.GameObject> tutoSprites;

	public global::UnityEngine.UI.Text title;

	public global::UnityEngine.UI.Text counter;

	public global::ButtonGroup previous;

	public global::ButtonGroup next;

	public global::UnityEngine.Sprite prevSprite;

	public global::UnityEngine.Sprite nextSprite;

	private int currentTutorialIndex;

	private int currentMessageIndex;

	private global::System.Action backCallback;
}
