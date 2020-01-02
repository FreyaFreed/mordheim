using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ButtonGroup : global::ImageGroup
{
	protected override void Awake()
	{
		base.Awake();
		if (this.effects == null)
		{
			this.effects = base.GetComponentInChildren<global::ToggleEffects>();
		}
		if (this.label == null)
		{
			this.label = base.GetComponentInChildren<global::UnityEngine.UI.Text>();
		}
		this.SetDisabled(false);
	}

	protected override void Start()
	{
		base.Start();
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	private void Localize()
	{
		if (this.label != null && this.locTag != null)
		{
			this.label.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(this.locTag);
		}
	}

	public override void SetAction(string pandoraAction, string locTag, int inputLayer = 0, bool negative = false, global::UnityEngine.Sprite keyboardOverload = null, global::UnityEngine.Sprite consoleOverload = null)
	{
		base.SetAction(pandoraAction, locTag, inputLayer, negative, keyboardOverload, consoleOverload);
		this.SetInteractable(true);
		this.locTag = locTag;
		this.Localize();
	}

	public bool IsInteractable()
	{
		return this.effects.toggle.interactable;
	}

	public void SetInteractable(bool inter)
	{
		this.effects.toggle.interactable = inter;
	}

	public void SetDisabled(bool disabled = true)
	{
		this.effects.overrideColor = true;
		this.effects.enabled = !disabled;
		this.effects.toggle.isOn = (this.effects.toggle.isOn && !disabled);
		this.SetInteractable(!disabled);
	}

	private new void OnEnable()
	{
		this.justEnabled = true;
	}

	private void Update()
	{
		if (this.justEnabled)
		{
			this.justEnabled = false;
			return;
		}
		if (this.action != null && this.callback != null && this.IsInteractable() && ((!this.negative && global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp(this.action, this.inputLayer)) || (this.negative && global::PandoraSingleton<global::PandoraInput>.Instance.GetNegKeyUp(this.action, this.inputLayer))))
		{
			global::PandoraDebug.LogDebug(string.Concat(new object[]
			{
				"ButtonGroup Action = ",
				this.action,
				"Layer ",
				this.inputLayer
			}), "uncategorised", null);
			this.callback();
		}
	}

	public virtual void OnAction(global::UnityEngine.Events.UnityAction func, bool mouseOnly, bool clear = true)
	{
		this.justEnabled = true;
		if (clear)
		{
			this.effects.onAction.RemoveAllListeners();
		}
		if (!mouseOnly)
		{
			this.callback = func;
		}
		else
		{
			this.callback = null;
		}
		if (func != null)
		{
			this.effects.onAction.AddListener(func);
		}
	}

	public global::ToggleEffects effects;

	public global::UnityEngine.UI.Text label;

	private string locTag;

	private bool justEnabled;
}
