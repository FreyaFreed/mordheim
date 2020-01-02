using System;
using UnityEngine.Events;
using UnityEngine.UI;

public class UnitBioModule : global::UIModule
{
	public override void Init()
	{
		base.Init();
		if (global::PandoraSingleton<global::Hephaestus>.Instance.IsPrivilegeRestricted(global::Hephaestus.RestrictionId.UGC))
		{
			this.editNameButton.gameObject.SetActive(false);
			this.editBioButton.gameObject.SetActive(false);
		}
		else
		{
			this.editNameButton.gameObject.SetActive(true);
			this.editBioButton.gameObject.SetActive(true);
			this.editNameButton.SetAction("rename_unit", "hideout_custom_edit_name", 0, false, null, null);
			this.editNameButton.OnAction(new global::UnityEngine.Events.UnityAction(this.OnEditNamePressed), false, true);
			this.editNameField.onEndEdit.AddListener(new global::UnityEngine.Events.UnityAction<string>(this.OnNameChanged));
			this.editNameField.interactable = false;
			this.editBioButton.SetAction("edit_bio", "hideout_custom_edit_bio", 0, false, null, null);
			this.editBioButton.OnAction(new global::UnityEngine.Events.UnityAction(this.OnEditBioPressed), false, true);
			this.editBioField.onEndEdit.AddListener(new global::UnityEngine.Events.UnityAction<string>(this.OnBioChanged));
			this.editBioField.interactable = false;
		}
	}

	public void Setup(global::UnityEngine.UI.ToggleGroup toggleGroup, global::System.Action<string> nameChangedCb, global::System.Action<string> bioChangedCb, global::System.Action navigateRight)
	{
		if (toggleGroup != null)
		{
			this.editNameButton.GetComponent<global::UnityEngine.UI.Toggle>().group = toggleGroup;
			this.editBioButton.GetComponent<global::UnityEngine.UI.Toggle>().group = toggleGroup;
		}
		this.onNameChanged = nameChangedCb;
		this.onBioChanged = bioChangedCb;
		this.onNavigateRight = navigateRight;
	}

	private void OnEnable()
	{
		if (this.toggleGroup != null)
		{
			this.toggleGroup.SetAllTogglesOff();
		}
	}

	private void Update()
	{
		if (global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp("h", 0) && this.onNavigateRight != null && (this.editBioButton.GetComponent<global::UnityEngine.UI.Toggle>().isOn || this.editNameButton.GetComponent<global::UnityEngine.UI.Toggle>().isOn))
		{
			this.onNavigateRight();
		}
		if (global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp("cancel", 2))
		{
			if (this.editNameField.interactable)
			{
				this.editNameField.DeactivateInputField();
				this.UnfocusField(this.editNameField);
				this.editNameButton.SetSelected(true);
			}
			else
			{
				this.editBioField.DeactivateInputField();
				this.UnfocusField(this.editBioField);
				this.editBioButton.SetSelected(true);
			}
		}
	}

	public void SetName(string name)
	{
		this.editNameField.text = name;
	}

	private void OnEditNamePressed()
	{
		if (global::PandoraSingleton<global::PandoraInput>.Instance.lastInputMode == global::PandoraInput.InputMode.JOYSTICK)
		{
			if (!global::PandoraSingleton<global::Hephaestus>.Instance.ShowVirtualKeyboard(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_name"), this.editNameField.text, 35U, false, new global::Hephaestus.OnVirtualKeyboardCallback(this.OnNameChangedVK), true))
			{
				this.FocusField(this.editNameField);
			}
		}
		else
		{
			this.FocusField(this.editNameField);
		}
	}

	private void OnNameChangedVK(bool success, string newName)
	{
		if (success)
		{
			this.OnNameChanged(newName);
		}
	}

	private void OnNameChanged(string newName)
	{
		this.UnfocusField(this.editNameField);
		if (this.onNameChanged != null)
		{
			this.onNameChanged(newName);
		}
	}

	public void SetBio(string bio)
	{
		this.editBioField.text = bio;
	}

	private void OnEditBioPressed()
	{
		if (global::PandoraSingleton<global::PandoraInput>.Instance.lastInputMode == global::PandoraInput.InputMode.JOYSTICK)
		{
			uint maxChar = 2500U;
			if (!global::PandoraSingleton<global::Hephaestus>.Instance.ShowVirtualKeyboard(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_biography"), this.editBioField.text, maxChar, false, new global::Hephaestus.OnVirtualKeyboardCallback(this.OnBioChangedVK), false))
			{
				this.FocusField(this.editBioField);
			}
		}
		else
		{
			this.FocusField(this.editBioField);
		}
	}

	private void OnBioChangedVK(bool success, string newBio)
	{
		if (success)
		{
			this.OnBioChanged(newBio);
		}
	}

	private void OnBioChanged(string newBio)
	{
		this.UnfocusField(this.editBioField);
		if (this.onBioChanged != null)
		{
			this.onBioChanged(newBio);
		}
	}

	private void FocusField(global::UnityEngine.UI.InputField field)
	{
		field.interactable = true;
		field.SetSelected(true);
		field.selectionAnchorPosition = 0;
		field.selectionFocusPosition = field.text.Length;
		global::PandoraSingleton<global::PandoraInput>.Instance.PushInputLayer(global::PandoraInput.InputLayer.CHAT);
	}

	private void UnfocusField(global::UnityEngine.UI.InputField field)
	{
		field.interactable = false;
		field.selectionAnchorPosition = 0;
		field.selectionFocusPosition = 0;
		global::PandoraSingleton<global::PandoraInput>.Instance.PopInputLayer(global::PandoraInput.InputLayer.CHAT);
	}

	public global::UnityEngine.UI.Selectable GetNavItem()
	{
		return this.editNameButton.GetComponent<global::UnityEngine.UI.Toggle>();
	}

	public global::HightlightAnimate highlight;

	public global::UnityEngine.UI.ToggleGroup toggleGroup;

	public global::ButtonGroup editNameButton;

	public global::ButtonGroup editBioButton;

	public global::UnityEngine.UI.InputField editNameField;

	public global::UnityEngine.UI.InputField editBioField;

	private global::System.Action<string> onNameChanged;

	private global::System.Action<string> onBioChanged;

	private global::System.Action onNavigateRight;
}
