using System;
using Rewired;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ImageGroup : global::UnityEngine.MonoBehaviour
{
	protected virtual void Awake()
	{
		if (this.button == null)
		{
			global::UnityEngine.UI.Image[] componentsInChildren = base.GetComponentsInChildren<global::UnityEngine.UI.Image>(true);
			if (componentsInChildren.Length >= 1)
			{
				if (componentsInChildren.Length == 1)
				{
					this.button = componentsInChildren[0];
				}
				else
				{
					foreach (global::UnityEngine.UI.Image image in componentsInChildren)
					{
						global::UnityEngine.UI.LayoutElement component = image.GetComponent<global::UnityEngine.UI.LayoutElement>();
						if (component == null || !component.ignoreLayout)
						{
							this.button = image;
							break;
						}
					}
				}
			}
		}
	}

	protected virtual void Start()
	{
		global::PandoraSingleton<global::NoticeManager>.Instance.RegisterListener(global::Notices.INPUT_TYPE_CHANGED, new global::DelReceiveNotice(this.OnInputTypeChanged));
		global::PandoraSingleton<global::NoticeManager>.Instance.RegisterListener(global::Notices.OPTIONS_CLOSED, new global::DelReceiveNotice(this.RefreshImage));
	}

	protected virtual void OnDestroy()
	{
		if (global::PandoraSingleton<global::NoticeManager>.Instance != null)
		{
			global::PandoraSingleton<global::NoticeManager>.Instance.RemoveListener(global::Notices.INPUT_TYPE_CHANGED, new global::DelReceiveNotice(this.OnInputTypeChanged));
			global::PandoraSingleton<global::NoticeManager>.Instance.RemoveListener(global::Notices.OPTIONS_CLOSED, new global::DelReceiveNotice(this.RefreshImage));
		}
	}

	protected virtual void OnInputTypeChanged()
	{
		this.RefreshImage();
	}

	protected virtual void OnEnable()
	{
		this.RefreshImage();
	}

	public virtual void SetAction(string pandoraAction, string locTag, int inputLayer = 0, bool negative = false, global::UnityEngine.Sprite keyboardOverload = null, global::UnityEngine.Sprite consoleOverload = null)
	{
		this.negative = negative;
		this.inputLayer = inputLayer;
		this.keyboardImageOverload = keyboardOverload;
		this.consoleImageOverload = consoleOverload;
		base.gameObject.SetActive(true);
		if (!string.IsNullOrEmpty(pandoraAction))
		{
			this.action = pandoraAction;
			this.RefreshImage();
			this.button.gameObject.SetActive(true);
		}
		else
		{
			this.button.gameObject.SetActive(false);
			this.action = null;
		}
	}

	public void RefreshImage()
	{
		if (this.action != null && this.button != null)
		{
			if (this.gamepadOnly)
			{
				if (global::PandoraSingleton<global::PandoraInput>.Instance.lastInputMode != global::PandoraInput.InputMode.JOYSTICK)
				{
					global::UnityEngine.UI.MaskableGraphic[] componentsInChildren = base.GetComponentsInChildren<global::UnityEngine.UI.MaskableGraphic>(true);
					for (int i = 0; i < componentsInChildren.Length; i++)
					{
						componentsInChildren[i].enabled = false;
					}
				}
				else
				{
					global::UnityEngine.UI.MaskableGraphic[] componentsInChildren2 = base.GetComponentsInChildren<global::UnityEngine.UI.MaskableGraphic>(true);
					for (int j = 0; j < componentsInChildren2.Length; j++)
					{
						componentsInChildren2[j].enabled = true;
					}
				}
			}
			this.button.gameObject.SetActive(global::PandoraSingleton<global::PandoraInput>.Instance.lastInputMode == global::PandoraInput.InputMode.JOYSTICK || !this.hideIconInKeyboard);
			if (this.keyboardImageOverload != null)
			{
				this.button.sprite = this.keyboardImageOverload;
			}
			else
			{
				global::Rewired.Pole inputPole = (!this.negative) ? global::Rewired.Pole.Positive : global::Rewired.Pole.Negative;
				string text = string.Empty;
				switch (global::PandoraSingleton<global::PandoraInput>.Instance.lastInputMode)
				{
				case global::PandoraInput.InputMode.NONE:
				case global::PandoraInput.InputMode.KEYBOARD:
				case global::PandoraInput.InputMode.MOUSE:
				{
					global::Rewired.ActionElementMap inputForAction = global::PandoraSingleton<global::PandoraInput>.Instance.GetInputForAction(this.action, global::Rewired.ControllerType.Mouse, inputPole);
					if (inputForAction != null)
					{
						text = inputForAction.elementIdentifierName;
					}
					else
					{
						inputForAction = global::PandoraSingleton<global::PandoraInput>.Instance.GetInputForAction(this.action, global::Rewired.ControllerType.Keyboard, inputPole);
						if (inputForAction != null)
						{
							text = inputForAction.elementIdentifierName;
						}
					}
					break;
				}
				case global::PandoraInput.InputMode.JOYSTICK:
				{
					global::Rewired.ActionElementMap inputForAction = global::PandoraSingleton<global::PandoraInput>.Instance.GetInputForAction(this.action, global::Rewired.ControllerType.Joystick, inputPole);
					if (inputForAction != null)
					{
						string text2 = inputForAction.elementIdentifierName;
						if (text2.EndsWith("-") || text2.EndsWith("+"))
						{
							text2 = text2.Substring(0, text2.Length - 2);
						}
						if (inputForAction.elementType == global::Rewired.ControllerElementType.Axis && inputForAction.axisRange == global::Rewired.AxisRange.Full)
						{
							if (this.negative)
							{
								text2 += " -";
							}
							else
							{
								text2 += " +";
							}
						}
						text = "joy_" + text2;
					}
					else
					{
						inputForAction = global::PandoraSingleton<global::PandoraInput>.Instance.GetInputForAction(this.action, global::Rewired.ControllerType.Mouse, inputPole);
						if (inputForAction != null)
						{
							text = inputForAction.elementIdentifierName;
						}
						else
						{
							inputForAction = global::PandoraSingleton<global::PandoraInput>.Instance.GetInputForAction(this.action, global::Rewired.ControllerType.Keyboard, inputPole);
							if (inputForAction != null)
							{
								text = inputForAction.elementIdentifierName;
							}
						}
					}
					break;
				}
				}
				if (!string.IsNullOrEmpty(text))
				{
					if (!global::InputImageTable.butTable.ContainsKey(text))
					{
						text = "???";
					}
					string str = global::InputImageTable.butTable[text];
					this.button.sprite = global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadResource<global::UnityEngine.Sprite>("input/" + str, true);
				}
			}
		}
	}

	public global::UnityEngine.UI.Image button;

	public bool gamepadOnly;

	private global::UnityEngine.Sprite keyboardImageOverload;

	private global::UnityEngine.Sprite consoleImageOverload;

	public bool hideIconInKeyboard;

	protected string action;

	protected bool negative;

	protected int inputLayer;

	protected global::UnityEngine.Events.UnityAction callback;
}
