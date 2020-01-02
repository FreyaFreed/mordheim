using System;
using System.Collections.Generic;

public class ListTabsModule : global::UIModule
{
	public void Setup(global::System.Action<int> tabChanged, int current)
	{
		this.tabChangedCallback = tabChanged;
		for (int i = 0; i < this.tabs.Count; i++)
		{
			int index = i;
			this.tabs[i].onAction.AddListener(delegate()
			{
				this.OnTabChanged(index);
			});
		}
		this.OnTabChanged(current);
	}

	private void OnTabChanged(int index)
	{
		this.currentTab = index;
		this.tabChangedCallback(index);
	}

	public void Next()
	{
		this.currentTab = ((this.currentTab + 1 < this.tabs.Count) ? (this.currentTab + 1) : 0);
		if (!this.tabs[this.currentTab].isActiveAndEnabled)
		{
			this.Next();
		}
		else
		{
			this.tabs[this.currentTab].SetOn();
			this.tabChangedCallback(this.currentTab);
		}
	}

	public void Prev()
	{
		this.currentTab = ((this.currentTab - 1 < 0) ? (this.tabs.Count - 1) : (this.currentTab - 1));
		if (!this.tabs[this.currentTab].isActiveAndEnabled)
		{
			this.Prev();
		}
		else
		{
			this.tabs[this.currentTab].SetOn();
			this.tabChangedCallback(this.currentTab);
		}
	}

	public global::System.Collections.Generic.List<global::ToggleEffects> tabs;

	private global::System.Action<int> tabChangedCallback;

	public int currentTab;
}
