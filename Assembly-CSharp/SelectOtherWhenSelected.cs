using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectOtherWhenSelected : global::UnityEngine.MonoBehaviour, global::UnityEngine.EventSystems.ISelectHandler, global::UnityEngine.EventSystems.IEventSystemHandler
{
	public void Select(global::UnityEngine.GameObject go)
	{
		global::UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(go ?? base.gameObject);
	}

	public void OnSelect(global::UnityEngine.EventSystems.BaseEventData eventData)
	{
		base.StartCoroutine(this.Select());
	}

	public global::System.Collections.IEnumerator Select()
	{
		yield return null;
		this.Select(this.target);
		yield break;
	}

	public global::UnityEngine.GameObject target;
}
