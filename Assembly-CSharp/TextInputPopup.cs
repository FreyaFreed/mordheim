using System;
using UnityEngine;
using UnityEngine.UI;

public class TextInputPopup : global::ConfirmationPopupView
{
	public void ShowLocalized(string newTitle, string newText, global::System.Action<bool, string> callback, bool hideButtons = false, string initialContent = "", int maxLength = 0)
	{
		base.ShowLocalized(newTitle, newText, delegate(bool confirm)
		{
			callback(confirm, this.inputField.text);
		}, hideButtons, false);
		this.inputField.Select();
		this.inputField.characterLimit = maxLength;
		this.inputField.text = initialContent;
	}

	public void Show(string titleId, string textId, global::System.Action<bool, string> callback, bool hideButtons = false, string initialContent = "", int maxLength = 0)
	{
		base.Show(titleId, textId, delegate(bool confirm)
		{
			callback(confirm, this.inputField.text);
		}, hideButtons, false);
		this.inputField.Select();
		this.inputField.characterLimit = maxLength;
		this.inputField.text = initialContent;
	}

	[global::UnityEngine.SerializeField]
	private global::UnityEngine.UI.InputField inputField;
}
