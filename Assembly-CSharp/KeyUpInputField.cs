using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class KeyUpInputField : global::UnityEngine.UI.InputField
{
	public override void OnSelect(global::UnityEngine.EventSystems.BaseEventData eventData)
	{
		base.OnSelect(eventData);
		if (this.OnSelectCallBack != null)
		{
			this.OnSelectCallBack();
		}
	}

	public override void OnUpdateSelected(global::UnityEngine.EventSystems.BaseEventData eventData)
	{
		if (!base.isFocused)
		{
			return;
		}
		bool flag = false;
		while (global::UnityEngine.Event.PopEvent(this.processingEvent))
		{
			if (this.processingEvent.rawType == global::UnityEngine.EventType.KeyDown && this.processingEvent.character != '\n' && this.processingEvent.character != '\r' && this.processingEvent.keyCode != global::UnityEngine.KeyCode.Escape)
			{
				base.KeyPressed(this.processingEvent);
				flag = true;
			}
			else if (this.processingEvent.rawType == global::UnityEngine.EventType.KeyUp && (this.processingEvent.keyCode == global::UnityEngine.KeyCode.KeypadEnter || this.processingEvent.keyCode == global::UnityEngine.KeyCode.Return || this.processingEvent.keyCode == global::UnityEngine.KeyCode.Escape) && base.KeyPressed(this.processingEvent) == global::UnityEngine.UI.InputField.EditState.Finish)
			{
				flag = true;
				base.DeactivateInputField();
				break;
			}
		}
		if (flag)
		{
			base.UpdateLabel();
		}
		eventData.Use();
	}

	public override void OnDeselect(global::UnityEngine.EventSystems.BaseEventData eventData)
	{
		if (this.clearOnDeselect)
		{
			base.text = string.Empty;
		}
		base.OnDeselect(eventData);
	}

	private void Update()
	{
		if (base.isFocused && global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp("action", 1))
		{
			base.DeactivateInputField();
		}
	}

	[global::UnityEngine.SerializeField]
	public bool clearOnDeselect = true;

	private readonly global::UnityEngine.Event processingEvent = new global::UnityEngine.Event();

	public global::UnityEngine.Events.UnityAction OnSelectCallBack;
}
