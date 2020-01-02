using System;
using UnityEngine;
using UnityEngine.UI;

public class TabIcon : global::UnityEngine.MonoBehaviour
{
	private void Awake()
	{
		this.Init();
	}

	public void Init()
	{
		if (this.initialized)
		{
			return;
		}
		this.initialized = true;
		this.toggle = base.GetComponent<global::ToggleEffects>();
		this.canvasGroup = base.GetComponent<global::UnityEngine.CanvasGroup>();
		this.textContent.gameObject.SetActive(false);
	}

	[global::UnityEngine.HideInInspector]
	public global::ToggleEffects toggle;

	[global::UnityEngine.HideInInspector]
	public global::UnityEngine.CanvasGroup canvasGroup;

	public global::UnityEngine.UI.Image icon;

	public global::TabsModule.IsAvailable IsAvailableDelegate;

	public global::UnityEngine.GameObject exclamationMark;

	public global::UnityEngine.GameObject impossibleMark;

	public string reason;

	public string titleText;

	public global::UnityEngine.GameObject textContent;

	public global::UnityEngine.UI.Text tabTitle;

	public global::UnityEngine.UI.Text tabReasonTitle;

	public bool available;

	[global::UnityEngine.HideInInspector]
	public global::HideoutManager.State state;

	[global::UnityEngine.HideInInspector]
	public global::HideoutCamp.NodeSlot nodeSlot;

	private bool initialized;
}
