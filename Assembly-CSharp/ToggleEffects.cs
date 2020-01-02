using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[global::UnityEngine.RequireComponent(typeof(global::UnityEngine.UI.Toggle))]
public class ToggleEffects : global::UnityEngine.MonoBehaviour, global::UnityEngine.EventSystems.ISubmitHandler, global::UnityEngine.EventSystems.IPointerClickHandler, global::UnityEngine.EventSystems.ISelectHandler, global::UnityEngine.EventSystems.IEventSystemHandler, global::UnityEngine.EventSystems.IPointerEnterHandler, global::UnityEngine.EventSystems.IPointerExitHandler, global::UnityEngine.EventSystems.IDeselectHandler
{
	public global::UnityEngine.UI.Toggle toggle
	{
		get
		{
			if (this._toggle == null)
			{
				this._toggle = base.GetComponent<global::UnityEngine.UI.Toggle>();
			}
			return this._toggle;
		}
	}

	private void Awake()
	{
		this.toggle.onValueChanged.AddListener(delegate(bool isOn)
		{
			this.OnValueChanged(isOn, false);
		});
	}

	private void OnEnable()
	{
		if (!this.actionDisabled)
		{
			this.Color(this.color.normalColor, false);
			this.Scale(this.scale.normalScale);
			this.OnValueChanged(this.toggle.isOn, true);
		}
	}

	private void OnDisable()
	{
		this.Color(this.color.disabledColor, false);
		this.Scale(this.scale.normalScale);
	}

	public void DisableAction()
	{
		this.actionDisabled = true;
		this.Color(this.color.disabledColor, false);
		this.Scale(this.scale.normalScale);
	}

	public void EnableAction()
	{
		this.actionDisabled = false;
		this.Color(this.color.normalColor, false);
		this.Scale(this.scale.normalScale);
	}

	private void OnValueChanged(bool isOn, bool force = false)
	{
		if (base.isActiveAndEnabled && (force || this.currentValue != isOn))
		{
			this.currentValue = isOn;
			if (!this.actionDisabled)
			{
				this.Color((!isOn) ? this.color.normalColor : this.color.highlightedColor, false);
				this.Scale((!isOn) ? this.scale.normalScale : this.scale.highlightedScale);
			}
			if (isOn)
			{
				if (this.toggleOnSelect && base.isActiveAndEnabled)
				{
					base.StartCoroutine(this.SelectOnNextFrame());
				}
				else
				{
					this.onSelect.Invoke();
				}
			}
			else
			{
				this.onUnselect.Invoke();
			}
		}
	}

	private void Color(global::UnityEngine.Color newColor, bool forceColor = false)
	{
		if (global::UnityEngine.Application.isPlaying)
		{
			for (int i = 0; i < this.toColor.Count; i++)
			{
				if (this.toColor[i] != null)
				{
					if (this.overrideColor)
					{
						this.toColor[i].DOColor(newColor, this.color.fadeDuration);
					}
					else
					{
						this.toColor[i].CrossFadeColor(newColor, this.color.fadeDuration, true, true);
					}
				}
			}
		}
	}

	private void Scale(global::UnityEngine.Vector2 newScale)
	{
		if (global::UnityEngine.Application.isPlaying)
		{
			global::UnityEngine.Vector3 endValue = new global::UnityEngine.Vector3(newScale.x, newScale.y, 1f);
			for (int i = 0; i < this.toScale.Count; i++)
			{
				if (this.toScale[i] != null)
				{
					this.toScale[i].DOScale(endValue, this.scale.duration);
				}
			}
		}
	}

	public void OnPointerEnter(global::UnityEngine.EventSystems.PointerEventData eventData)
	{
		if (base.isActiveAndEnabled && this.toggle.isActiveAndEnabled && (!this.checkInteractable || this.toggle.IsInteractable()))
		{
			if (!this.actionDisabled)
			{
				if (this.highlightOnOver)
				{
					this.Color((!this.currentValue) ? this.color.pressedColor : this.color.highlightedColor, false);
					this.Scale((!this.currentValue) ? this.scale.pressedScale : this.scale.highlightedScale);
				}
				if (this.toggleOnOver || this.selectOnOver)
				{
					if (this.toggleOnOver)
					{
						this.SetOn();
					}
					base.StartCoroutine(this.SelectOnNextFrame());
				}
			}
			this.onPointerEnter.Invoke();
		}
	}

	private global::System.Collections.IEnumerator SelectOnNextFrame()
	{
		global::UnityEngine.GameObject toSelect = null;
		if (this.toSelectOnOver != null)
		{
			toSelect = this.toSelectOnOver;
		}
		else
		{
			toSelect = base.gameObject;
		}
		if (global::UnityEngine.EventSystems.EventSystem.current != null && global::UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject != toSelect)
		{
			yield return null;
			toSelect.SetSelected(false);
		}
		else
		{
			this.onSelect.Invoke();
		}
		yield break;
	}

	public void OnSubmit(global::UnityEngine.EventSystems.BaseEventData eventData)
	{
		if (this.toggle.IsInteractable())
		{
			global::UISound.Instance.OnClick();
			if (this.keepSelectedOnSubmit && !this.currentValue)
			{
				this.SetOn();
			}
			if (!global::UnityEngine.Input.GetMouseButtonUp(0) && this.enableKeySubmit)
			{
				this.onAction.Invoke();
			}
		}
	}

	public void OnPointerClick(global::UnityEngine.EventSystems.PointerEventData eventData)
	{
		if (base.isActiveAndEnabled && this.toggle.isActiveAndEnabled && (!this.checkInteractable || this.toggle.IsInteractable()) && eventData.button == global::UnityEngine.EventSystems.PointerEventData.InputButton.Left)
		{
			global::UISound.Instance.OnClick();
			if (!this.actionDisabled && this.keepSelectedOnSubmit && !this.currentValue)
			{
				this.SetOn();
			}
			if (eventData.clickCount == 2)
			{
				this.onDoubleClick.Invoke();
			}
			else if (!this.actionDisabled)
			{
				this.onAction.Invoke();
			}
		}
	}

	public void OnSelect(global::UnityEngine.EventSystems.BaseEventData eventData)
	{
		global::UISound.Instance.OnSelect();
		if (this.submitOnToggle && !this.actionDisabled)
		{
			this.onAction.Invoke();
		}
		if (this.highlightOnSelect && !this.actionDisabled)
		{
			this.Color((!this.currentValue) ? this.color.pressedColor : this.color.highlightedColor, false);
			this.Scale((!this.currentValue) ? this.scale.pressedScale : this.scale.highlightedScale);
		}
		if (this.toggleOnSelect && !(eventData is global::UnityEngine.EventSystems.PointerEventData))
		{
			this.SetOn();
		}
		if (base.isActiveAndEnabled)
		{
			base.StartCoroutine(this.SelectOnNextFrame());
		}
	}

	public void SetOn()
	{
		if (!this.currentValue && !this.toggle.isOn)
		{
			this.toggle.isOn = true;
		}
	}

	public void OnPointerExit(global::UnityEngine.EventSystems.PointerEventData eventData)
	{
		if (base.isActiveAndEnabled && this.toggle.isActiveAndEnabled && (!this.checkInteractable || this.toggle.IsInteractable()))
		{
			if (!this.actionDisabled)
			{
				if (this.unToggleOnExit)
				{
					this.toggle.isOn = false;
					this.Color(this.color.normalColor, false);
					this.Scale(this.scale.normalScale);
					if (global::UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject == base.gameObject)
					{
						global::UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
					}
				}
				else if (this.highlightOnOver)
				{
					this.Color((!this.currentValue) ? this.color.normalColor : this.color.highlightedColor, false);
					this.Scale((!this.currentValue) ? this.scale.normalScale : this.scale.highlightedScale);
				}
			}
			this.onPointerExit.Invoke();
		}
	}

	public void OnDeselect(global::UnityEngine.EventSystems.BaseEventData eventData)
	{
		if (!this.actionDisabled)
		{
			if (this.highlightOnSelect)
			{
				this.Color((!this.currentValue) ? this.color.normalColor : this.color.highlightedColor, false);
				this.Scale((!this.currentValue) ? this.scale.normalScale : this.scale.highlightedScale);
			}
			if (this.unToggleOnUnSelect)
			{
				this.toggle.isOn = false;
			}
		}
		this.onUnselect.Invoke();
	}

	public global::System.Collections.Generic.List<global::UnityEngine.UI.Graphic> toColor;

	public global::UnityEngine.UI.ColorBlock color = global::UnityEngine.UI.MordheimColorBlock.defaultColorBlock;

	public global::System.Collections.Generic.List<global::UnityEngine.RectTransform> toScale;

	public global::UnityEngine.UI.ScaleBlock scale = global::UnityEngine.UI.ScaleBlock.defaultScaleBlock;

	public global::UnityEngine.GameObject toSelectOnOver;

	public bool toggleOnOver = true;

	public bool selectOnOver;

	public bool highlightOnOver = true;

	public bool toggleOnSelect = true;

	public bool highlightOnSelect;

	public bool unToggleOnExit;

	public bool unToggleOnUnSelect;

	public bool submitOnToggle;

	public bool enableKeySubmit = true;

	public bool keepSelectedOnSubmit = true;

	public bool checkInteractable = true;

	public bool overrideColor;

	public global::UnityEngine.Events.UnityEvent onAction;

	public global::UnityEngine.Events.UnityEvent onPointerEnter;

	public global::UnityEngine.Events.UnityEvent onPointerExit;

	public global::UnityEngine.Events.UnityEvent onSelect;

	public global::UnityEngine.Events.UnityEvent onUnselect;

	public global::UnityEngine.Events.UnityEvent onDoubleClick;

	public bool actionDisabled;

	private global::UnityEngine.UI.Toggle _toggle;

	private bool currentValue;
}
