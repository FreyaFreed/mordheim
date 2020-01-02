using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TabsModule : global::UIModule
{
	public override void Init()
	{
		base.Init();
	}

	public void Setup(global::TitleModule title)
	{
		this.titleModule = title;
		this.nextTab.SetAction("switch_tab", null, 0, false, null, null);
		this.nextTab.OnAction(new global::UnityEngine.Events.UnityAction(this.Next), false, true);
		this.prevTab.SetAction("switch_tab", null, 0, true, null, null);
		this.prevTab.OnAction(new global::UnityEngine.Events.UnityAction(this.Prev), false, true);
	}

	public virtual global::TabIcon AddTabIcon(global::HideoutManager.State state, int index, string spriteName = null, string loc = null, global::TabsModule.IsAvailable isAvailable = null)
	{
		if (string.IsNullOrEmpty(spriteName))
		{
			spriteName = "hideout_nav/" + state.ToLowerString();
		}
		if (isAvailable == null)
		{
			isAvailable = new global::TabsModule.IsAvailable(this.DefaultIsAvailable);
		}
		global::TabIcon tabIcon = this.icons[index];
		tabIcon.Init();
		global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadResourceAsync<global::UnityEngine.Sprite>(spriteName, delegate(global::UnityEngine.Object go)
		{
			if (tabIcon != null && tabIcon.icon != null)
			{
				tabIcon.icon.sprite = (global::UnityEngine.Sprite)go;
			}
		}, true);
		tabIcon.state = state;
		tabIcon.IsAvailableDelegate = isAvailable;
		tabIcon.titleText = (loc ?? ("hideout_" + state.ToLowerString()));
		tabIcon.exclamationMark.SetActive(false);
		tabIcon.impossibleMark.SetActive(false);
		tabIcon.toggle.toggle.interactable = false;
		tabIcon.toggle.onAction.AddListener(delegate()
		{
			this.SetCurrentTabs(index);
		});
		tabIcon.toggle.onPointerEnter.AddListener(delegate()
		{
			this.OnTabIconEnter(tabIcon);
		});
		tabIcon.toggle.onPointerExit.AddListener(delegate()
		{
			this.OnTabIconExit(tabIcon);
		});
		return tabIcon;
	}

	public void OnTabIconEnter(global::TabIcon tabIcon)
	{
		for (int i = 0; i < this.icons.Count; i++)
		{
			this.icons[i].textContent.gameObject.SetActive(false);
		}
		tabIcon.textContent.gameObject.SetActive(true);
		tabIcon.tabTitle.gameObject.SetActive(true);
		tabIcon.tabTitle.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(tabIcon.titleText);
		if (global::PandoraSingleton<global::HideoutManager>.Instance.StateMachine.GetActiveStateId() != 0 && !tabIcon.available)
		{
			if (!tabIcon.tabReasonTitle.gameObject.activeSelf)
			{
				tabIcon.tabReasonTitle.gameObject.SetActive(true);
			}
			string stringById = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(tabIcon.reason);
			if (tabIcon.tabReasonTitle.text != stringById)
			{
				tabIcon.tabReasonTitle.text = stringById;
			}
		}
		else if (tabIcon.tabReasonTitle.gameObject.activeSelf)
		{
			tabIcon.tabReasonTitle.gameObject.SetActive(false);
		}
		if (this.onIconEnter != null)
		{
			this.onIconEnter(tabIcon);
		}
	}

	public void OnTabIconExit(global::TabIcon tabIcon)
	{
		if (tabIcon.textContent.gameObject.activeSelf)
		{
			tabIcon.textContent.gameObject.SetActive(false);
		}
		if (this.onIconExit != null)
		{
			this.onIconExit(tabIcon);
		}
	}

	public bool DefaultIsAvailable(out string reason)
	{
		reason = string.Empty;
		return true;
	}

	public virtual void Refresh()
	{
		for (int i = 0; i < this.icons.Count; i++)
		{
			string reason;
			this.ActivateIcon(this.icons[i], this.icons[i].IsAvailableDelegate(out reason), reason);
		}
	}

	private void ActivateIcon(global::TabIcon tabIcon, bool active, string reason)
	{
		tabIcon.available = active;
		tabIcon.canvasGroup.alpha = ((!active) ? 0.75f : 1f);
		tabIcon.toggle.highlightOnOver = active;
		if (tabIcon.toggle.toggle.interactable != active)
		{
			tabIcon.toggle.toggle.interactable = active;
		}
		if (tabIcon.impossibleMark.activeSelf)
		{
			tabIcon.impossibleMark.SetActive(false);
		}
		if (active)
		{
			if (tabIcon.icon.overrideSprite != null)
			{
				tabIcon.icon.overrideSprite = null;
			}
			tabIcon.reason = string.Empty;
		}
		else
		{
			if (tabIcon.icon.overrideSprite != this.lockIcon)
			{
				tabIcon.icon.overrideSprite = this.lockIcon;
			}
			tabIcon.reason = reason;
		}
	}

	public void SetCurrentTab(global::HideoutManager.State state)
	{
		for (int i = 0; i < this.icons.Count; i++)
		{
			if (this.icons[i].state == state)
			{
				this.currentIdx = i;
				break;
			}
		}
		this.Refresh();
		if (!this.tabIcons.activeSelf)
		{
			this.tabIcons.SetActive(true);
		}
		if (this.titleModule != null)
		{
			this.titleModule.Set(this.icons[this.currentIdx].titleText, false);
		}
		this.icons[this.currentIdx].toggle.toggle.isOn = true;
	}

	public void SetExclamation(params int[] marks)
	{
		for (int i = 0; i < this.icons.Count; i++)
		{
			if (this.icons[i].exclamationMark.activeSelf)
			{
				this.icons[i].exclamationMark.SetActive(false);
			}
		}
		for (int j = 0; j < marks.Length; j++)
		{
			if (!this.icons[marks[j]].exclamationMark.activeSelf)
			{
				this.icons[marks[j]].exclamationMark.SetActive(true);
			}
		}
	}

	protected virtual void SetCurrentTabs(int index)
	{
		if (this.IsTabAvailable(index))
		{
			this.currentIdx = index;
			global::BaseHideoutUnitState baseHideoutUnitState = global::PandoraSingleton<global::HideoutManager>.Instance.StateMachine.GetActiveState() as global::BaseHideoutUnitState;
			if (baseHideoutUnitState != null)
			{
				baseHideoutUnitState.CheckChangesAndChangeState(this.icons[this.currentIdx].state);
			}
			else
			{
				int state = (int)this.icons[this.currentIdx].state;
				if (global::PandoraSingleton<global::HideoutManager>.Instance.StateMachine.GetActiveStateId() != state)
				{
					global::PandoraSingleton<global::HideoutManager>.Instance.StateMachine.ChangeState(state);
				}
			}
		}
		else
		{
			this.icons[this.currentIdx].toggle.toggle.isOn = true;
		}
	}

	protected bool IsTabAvailable(int index)
	{
		return this.icons[index].available;
	}

	public bool IsTabAvailable(global::HideoutManager.State iconState)
	{
		for (int i = 0; i < this.icons.Count; i++)
		{
			if (this.icons[i].state == iconState)
			{
				return this.IsTabAvailable(i);
			}
		}
		return false;
	}

	public void Prev()
	{
		if (this.tabIcons.activeSelf)
		{
			int num = this.currentIdx;
			do
			{
				num--;
				num = ((num < this.icons.Count) ? num : 0);
				num = ((num >= 0) ? num : (this.icons.Count - 1));
			}
			while (!this.IsTabAvailable(num));
			this.SetCurrentTabs(num);
		}
	}

	public void Next()
	{
		if (this.tabIcons.activeSelf)
		{
			int num = this.currentIdx;
			do
			{
				num++;
				num = ((num < this.icons.Count) ? num : 0);
				num = ((num >= 0) ? num : (this.icons.Count - 1));
			}
			while (!this.IsTabAvailable(num));
			this.SetCurrentTabs(num);
		}
	}

	public global::UnityEngine.RectTransform anchor;

	public global::UnityEngine.GameObject prefab;

	public global::UnityEngine.Sprite lockIcon;

	private global::TitleModule titleModule;

	public global::UnityEngine.GameObject tabIcons;

	public global::System.Collections.Generic.List<global::TabIcon> icons = new global::System.Collections.Generic.List<global::TabIcon>();

	public global::ButtonGroup nextTab;

	public global::ButtonGroup prevTab;

	private int currentIdx;

	public global::System.Action<global::TabIcon> onIconEnter;

	public global::System.Action<global::TabIcon> onIconExit;

	public delegate bool IsAvailable(out string reason);
}
