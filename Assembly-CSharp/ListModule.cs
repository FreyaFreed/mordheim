using System;
using System.Collections.Generic;
using UnityEngine;

public class ListModule : global::UIModule
{
	public void SetTabs(global::ListTabsModule tabs)
	{
		this.listHeader = tabs;
	}

	public void Set<T>(global::System.Collections.Generic.IList<T> items, global::System.Action<int, T, global::UnityEngine.GameObject> itemAdded, global::System.Action<int, T> select, global::System.Action<int, T> confirm)
	{
		this.Clear();
		this.scrollGroup.Setup(this.prefab, true);
		this.isFocus = true;
		for (int i = 0; i < items.Count; i++)
		{
			T item = items[i];
			global::UnityEngine.GameObject gameObject = this.scrollGroup.AddToList(null, null);
			global::ToggleEffects component = gameObject.GetComponent<global::ToggleEffects>();
			int idx = items.Count;
			component.toggle.onValueChanged.AddListener(delegate(bool isOn)
			{
				if (isOn)
				{
					select(idx, item);
				}
			});
			component.onAction.AddListener(delegate()
			{
				confirm(idx, item);
			});
			itemAdded(idx, item, gameObject);
		}
		this.scrollGroup.RepositionScrollListOnNextFrame();
	}

	public void Clear()
	{
		this.scrollGroup.ClearList();
		this.isFocus = false;
	}

	private void OnDisable()
	{
		this.Clear();
	}

	private void Update()
	{
		if (this.isFocus)
		{
			if (global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp("h", 0))
			{
				this.listHeader.Next();
			}
			else if (global::PandoraSingleton<global::PandoraInput>.Instance.GetNegKeyUp("h", 0))
			{
				this.listHeader.Prev();
			}
		}
	}

	public void Reset()
	{
		this.Clear();
		this.listHeader = null;
	}

	public global::UnityEngine.GameObject prefab;

	public global::ScrollGroup scrollGroup;

	private bool isFocus;

	private global::ListTabsModule listHeader;
}
