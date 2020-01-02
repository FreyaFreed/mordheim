using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HireUnitSelectionModule : global::UIModule
{
	public override void Init()
	{
		base.Init();
		this.scrollGroup.Setup(this.prefabItem, false);
	}

	public void Set(global::System.Collections.Generic.List<global::UnitMenuController> hireUnits, global::System.Action prev, global::System.Action next, global::System.Action<int> unitSelected, global::System.Action<int> doubleClick)
	{
		this.Clear();
		for (int i = 0; i < hireUnits.Count; i++)
		{
			global::UnityEngine.GameObject gameObject = this.scrollGroup.AddToList(null, null);
			global::HireUnitDescription component = gameObject.GetComponent<global::HireUnitDescription>();
			component.Set(hireUnits[i].unit);
			global::ToggleEffects component2 = gameObject.GetComponent<global::ToggleEffects>();
			this.items.Add(component2);
			component2.onAction.RemoveAllListeners();
			int index = i;
			component2.onSelect.AddListener(delegate()
			{
				unitSelected(index);
			});
			if (doubleClick != null)
			{
				component2.onDoubleClick.AddListener(delegate()
				{
					doubleClick(index);
				});
				component.btnBuy.onAction.AddListener(delegate()
				{
					doubleClick(index);
				});
			}
			else
			{
				component.btnBuy.transform.GetChild(0).gameObject.SetActive(false);
			}
		}
		this.SelectFirstUnit();
	}

	public void SelectFirstUnit()
	{
		if (base.gameObject.activeSelf)
		{
			base.StartCoroutine(this.RealignOnNextFrame());
		}
	}

	private global::System.Collections.IEnumerator RealignOnNextFrame()
	{
		yield return null;
		this.OnUnitSelected(0);
		yield break;
	}

	public void OnUnitSelected(int index)
	{
		this.items[index].SetOn();
		this.scrollGroup.RealignList(true, index, true);
	}

	public void Clear()
	{
		this.scrollGroup.ClearList();
		this.items.Clear();
	}

	internal void Set(global::System.Collections.Generic.List<global::UnitMenuController> hireUnits, global::System.Action Prev, global::System.Action Next, global::System.Action<int> UnitConfirmed, object p)
	{
		throw new global::System.NotImplementedException();
	}

	public global::ScrollGroup scrollGroup;

	public global::UnityEngine.GameObject prefabItem;

	public global::System.Collections.Generic.List<global::ToggleEffects> items;
}
