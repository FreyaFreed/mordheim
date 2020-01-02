using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemDescModule : global::UIModule
{
	public override void Init()
	{
		base.Init();
		this.uiItems = new global::System.Collections.Generic.List<global::UIInventoryItem>();
		this.uiItemDescs = new global::System.Collections.Generic.List<global::UIInventoryItemDescription>();
		for (int i = 0; i < 2; i++)
		{
			global::UnityEngine.GameObject gameObject = global::UnityEngine.Object.Instantiate<global::UnityEngine.GameObject>(this.itemDescriptionPrefab);
			gameObject.transform.SetParent(base.transform, false);
			this.uiItems.Add(gameObject.GetComponentInChildren<global::UIInventoryItem>());
			this.uiItemDescs.Add(gameObject.GetComponentInChildren<global::UIInventoryItemDescription>());
		}
		global::UnityEngine.GameObject gameObject2 = global::UnityEngine.Object.Instantiate<global::UnityEngine.GameObject>(this.runeDescriptionPrefab);
		gameObject2.transform.SetParent(base.transform, false);
		this.uiRune = gameObject2.GetComponentInChildren<global::UIInventoryRune>();
		global::UnityEngine.GameObject gameObject3 = global::UnityEngine.Object.Instantiate<global::UnityEngine.GameObject>(this.mutationDescriptionPrefab);
		gameObject3.transform.SetParent(base.transform, false);
		this.uiMutation = gameObject3.GetComponentInChildren<global::UIInventoryMutation>();
	}

	public void HideAll()
	{
		this.HideDesc(0);
		for (int i = 1; i < this.uiItems.Count; i++)
		{
			this.uiItems[i].gameObject.SetActive(false);
		}
	}

	public void HideDesc(int idx)
	{
		this.uiItems[idx].gameObject.SetActive(false);
		this.uiRune.gameObject.SetActive(false);
		this.uiMutation.gameObject.SetActive(false);
	}

	public void SetItem(global::Item item, global::UnitSlotId slotId, int idx)
	{
		this.HideDesc((idx != 1) ? 1 : idx);
		this.uiItems[idx].gameObject.SetActive(true);
		this.uiItems[idx].Set(item, false, false, global::ItemId.NONE, false);
		this.uiItemDescs[idx].Set(item, new global::UnitSlotId?(slotId));
	}

	public void SetRune(global::RuneMark runeMark)
	{
		this.HideDesc(1);
		this.uiRune.gameObject.SetActive(true);
		this.uiRune.Set(runeMark);
	}

	public void SetMutation(global::Mutation mutation)
	{
		this.HideDesc(0);
		this.HideDesc(1);
		this.uiMutation.gameObject.SetActive(true);
		this.uiMutation.Set(mutation);
	}

	public global::UnityEngine.GameObject itemDescriptionPrefab;

	public global::UnityEngine.GameObject runeDescriptionPrefab;

	public global::UnityEngine.GameObject mutationDescriptionPrefab;

	private global::System.Collections.Generic.List<global::UIInventoryItem> uiItems;

	private global::System.Collections.Generic.List<global::UIInventoryItemDescription> uiItemDescs;

	private global::UIInventoryRune uiRune;

	private global::UIInventoryMutation uiMutation;
}
