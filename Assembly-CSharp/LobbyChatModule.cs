using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LobbyChatModule : global::UIModule
{
	public void Setup()
	{
		this.messages.Setup(this.item, true);
		this.messages.ClearList();
		this.text.onEndEdit.RemoveAllListeners();
		this.text.onEndEdit.AddListener(new global::UnityEngine.Events.UnityAction<string>(this.SendChat));
		((global::KeyUpInputField)this.text).OnSelectCallBack = new global::UnityEngine.Events.UnityAction(this.Select);
		global::PandoraSingleton<global::NoticeManager>.Instance.RegisterListener(global::Notices.HERMES_CHAT, new global::DelReceiveNotice(this.GetChat));
		this.prevSelected = null;
	}

	public void Select()
	{
		this.prevSelected = global::UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
		this.text.ActivateInputField();
		global::PandoraSingleton<global::PandoraInput>.Instance.PushInputLayer(global::PandoraInput.InputLayer.CHAT);
	}

	private void GetChat()
	{
		ulong num = (ulong)global::PandoraSingleton<global::NoticeManager>.Instance.Parameters[0];
		string text = (string)global::PandoraSingleton<global::NoticeManager>.Instance.Parameters[1];
		global::UnityEngine.GameObject gameObject = this.messages.AddToList(null, null);
		global::UnityEngine.UI.Text component = gameObject.GetComponent<global::UnityEngine.UI.Text>();
		if (num != 0UL)
		{
			component.GetComponent<global::UnityEngine.UI.Text>().color = new global::UnityEngine.Color(127f, 127f, 0f);
		}
		component.text = text;
	}

	public void SendChat(string message)
	{
		if (!string.IsNullOrEmpty(message))
		{
			global::PandoraSingleton<global::Hermes>.Instance.SendChat(message);
			this.text.text = string.Empty;
		}
		this.text.DeactivateInputField();
		if (this.prevSelected != null)
		{
			this.prevSelected.SetSelected(false);
		}
		this.prevSelected = null;
		global::PandoraSingleton<global::PandoraInput>.Instance.PopInputLayer(global::PandoraInput.InputLayer.CHAT);
	}

	public global::ScrollGroup messages;

	public global::UnityEngine.GameObject item;

	public global::UnityEngine.UI.InputField text;

	private global::UnityEngine.GameObject prevSelected;
}
