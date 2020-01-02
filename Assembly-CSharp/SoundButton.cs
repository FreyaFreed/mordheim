using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[global::UnityEngine.RequireComponent(typeof(global::UnityEngine.UI.Button))]
public class SoundButton : global::UnityEngine.MonoBehaviour, global::UnityEngine.EventSystems.IPointerClickHandler, global::UnityEngine.EventSystems.ISelectHandler, global::UnityEngine.EventSystems.IEventSystemHandler
{
	public void OnPointerClick(global::UnityEngine.EventSystems.PointerEventData eventData)
	{
		global::UISound.Instance.OnClick();
	}

	public void OnSelect(global::UnityEngine.EventSystems.BaseEventData eventData)
	{
		global::UISound.Instance.OnSelect();
	}
}
