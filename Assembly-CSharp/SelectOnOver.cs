using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectOnOver : global::UnityEngine.MonoBehaviour, global::UnityEngine.EventSystems.IEventSystemHandler, global::UnityEngine.EventSystems.IPointerEnterHandler
{
	public void OnPointerEnter(global::UnityEngine.EventSystems.PointerEventData eventData)
	{
		global::UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(base.gameObject);
	}
}
