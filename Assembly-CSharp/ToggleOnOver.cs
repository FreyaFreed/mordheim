using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[global::UnityEngine.RequireComponent(typeof(global::UnityEngine.UI.Toggle))]
public class ToggleOnOver : global::UnityEngine.MonoBehaviour, global::UnityEngine.EventSystems.IEventSystemHandler, global::UnityEngine.EventSystems.IPointerEnterHandler
{
	private void Awake()
	{
		this.toggle = base.GetComponent<global::UnityEngine.UI.Toggle>();
	}

	public void OnPointerEnter(global::UnityEngine.EventSystems.PointerEventData eventData)
	{
		this.toggle.isOn = true;
	}

	private global::UnityEngine.UI.Toggle toggle;
}
