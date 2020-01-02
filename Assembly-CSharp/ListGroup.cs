using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListGroup : global::UnityEngine.MonoBehaviour
{
	private void Awake()
	{
		this.title = base.GetComponentInChildren<global::UnityEngine.UI.Text>();
	}

	public void Setup(string name, global::UnityEngine.GameObject item)
	{
		string name2 = string.Empty;
		if (this.title != null && !string.IsNullOrEmpty(name))
		{
			name2 = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(name);
		}
		this.SetupLocalized(name2, item);
	}

	public void SetupLocalized(string name, global::UnityEngine.GameObject item)
	{
		if (this.title != null && !string.IsNullOrEmpty(name))
		{
			this.title.text = name;
		}
		this.item = item;
		this.ClearList();
	}

	public void ClearList()
	{
		for (int i = 0; i < this.items.Count; i++)
		{
			this.items[i].transform.SetParent(null);
			global::UnityEngine.Object.Destroy(this.items[i]);
		}
		this.items.Clear();
	}

	public global::UnityEngine.GameObject AddToList()
	{
		global::UnityEngine.GameObject gameObject = global::UnityEngine.Object.Instantiate<global::UnityEngine.GameObject>(this.item);
		gameObject.transform.SetParent((this.maxCount == 0 || !(this.alternateAnchor != null) || this.items.Count < this.maxCount) ? this.anchor : this.alternateAnchor, false);
		this.items.Add(gameObject);
		return gameObject;
	}

	public global::UnityEngine.UI.Text title;

	public global::UnityEngine.RectTransform anchor;

	public global::UnityEngine.RectTransform alternateAnchor;

	private global::UnityEngine.GameObject item;

	public int maxCount;

	[global::UnityEngine.HideInInspector]
	public global::System.Collections.Generic.List<global::UnityEngine.GameObject> items = new global::System.Collections.Generic.List<global::UnityEngine.GameObject>();
}
