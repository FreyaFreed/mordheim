using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SliderGroup : global::UnityEngine.MonoBehaviour
{
	private void Awake()
	{
		this.slider = base.GetComponentInChildren<global::UnityEngine.UI.Slider>();
		this.slider.onValueChanged.AddListener(new global::UnityEngine.Events.UnityAction<float>(this.UpdateText));
	}

	private void UpdateText(float value)
	{
		int value2 = (int)(value * (float)this.valMult);
		this.field.text = value2.ToConstantString();
		if (this.onValueChanged != null)
		{
			this.onValueChanged(this.id, this.slider.value);
		}
	}

	private void UpdateText(string text)
	{
		this.slider.value = float.Parse(text, global::System.Globalization.NumberFormatInfo.InvariantInfo) / (float)this.valMult;
		if (this.onValueChanged != null)
		{
			this.onValueChanged(this.id, this.slider.value);
		}
	}

	public void SetValue(float v)
	{
		this.UpdateText(v);
		int value = (int)(v * (float)this.valMult);
		this.UpdateText(value.ToConstantString());
	}

	public int GetValue()
	{
		return (int)this.slider.value;
	}

	[global::UnityEngine.HideInInspector]
	public global::UnityEngine.UI.Slider slider;

	public global::UnityEngine.UI.Text field;

	public int valMult = 100;

	public global::SliderGroup.OnValueChanged onValueChanged;

	public int id;

	public delegate void OnValueChanged(int id, float val);
}
