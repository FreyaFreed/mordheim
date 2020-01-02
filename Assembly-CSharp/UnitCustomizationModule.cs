using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitCustomizationModule : global::UIModule
{
	public bool IsFocused { get; private set; }

	public override void Init()
	{
		base.Init();
		this.styleList.Setup(this.listItemTemplate, true);
		this.tabs[0].toggle.isOn = true;
		for (int i = 0; i < this.tabs.Length; i++)
		{
			int idx = i;
			this.tabs[i].onAction.AddListener(delegate()
			{
				this.TabSelected(idx);
			});
		}
		this.Clear();
	}

	public void Refresh(global::System.Collections.Generic.List<string> styles, global::System.Action<int> onStyleSelected, string title)
	{
		this.titleField.text = title;
		this.styleList.ClearList();
		if (styles != null)
		{
			for (int i = 0; i < styles.Count; i++)
			{
				global::UnityEngine.GameObject gameObject = this.styleList.AddToList(null, null);
				gameObject.SetActive(true);
				gameObject.GetComponentInChildren<global::UnityEngine.UI.Text>().text = styles[i];
				int idx = i;
				gameObject.GetComponent<global::ToggleEffects>().onAction.AddListener(delegate()
				{
					this.SetFocused(true);
					onStyleSelected(idx);
				});
			}
		}
		this.styleList.RealignList(true, 0, true);
	}

	public void SetSelectedStyle(int idx)
	{
		base.StartCoroutine(this.DelaySetSelected(idx));
	}

	private global::System.Collections.IEnumerator DelaySetSelected(int idx)
	{
		yield return 0;
		this.styleList.items[idx].SetSelected(true);
		this.styleList.RealignList(true, idx, true);
		yield break;
	}

	public void Clear()
	{
		this.styleList.ClearList();
		this.SetTabsVisible(false);
		this.titleField.gameObject.SetActive(false);
	}

	private void TabSelected(int tabIdx)
	{
		this.SetFocused(true);
		if (this.onTabSelected != null)
		{
			this.onTabSelected(tabIdx);
		}
	}

	public void SetTabsVisible(bool visible = true)
	{
		this.titleField.gameObject.SetActive(!visible);
		for (int i = 0; i < this.tabs.Length; i++)
		{
			this.tabs[i].gameObject.SetActive(visible);
		}
	}

	public int GetSelectedTabIndex()
	{
		for (int i = 0; i < this.tabs.Length; i++)
		{
			if (this.tabs[i].toggle.isOn)
			{
				return i;
			}
		}
		return -1;
	}

	public void SetFocused(bool focused)
	{
		this.IsFocused = focused;
	}

	private void Update()
	{
		if (this.IsFocused && this.tabs[0].isActiveAndEnabled && this.tabs[1].isActiveAndEnabled)
		{
			if (global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp("h", 0))
			{
				this.tabs[1].SetOn();
				this.TabSelected(1);
			}
			else if (global::PandoraSingleton<global::PandoraInput>.Instance.GetNegKeyUp("h", 0))
			{
				this.tabs[0].SetOn();
				this.TabSelected(0);
			}
		}
	}

	[global::UnityEngine.SerializeField]
	private global::ScrollGroup styleList;

	[global::UnityEngine.SerializeField]
	private global::UnityEngine.GameObject listItemTemplate;

	[global::UnityEngine.SerializeField]
	private global::ToggleEffects[] tabs;

	[global::UnityEngine.SerializeField]
	private global::UnityEngine.UI.Text titleField;

	public global::System.Action<int> onTabSelected;
}
