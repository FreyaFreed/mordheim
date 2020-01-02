using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;

public class FlyingLabel : global::FlyingText
{
	private void Awake()
	{
		this.label = base.GetComponent<global::UnityEngine.UI.Text>();
	}

	public override void Destroy()
	{
		base.Destroy();
		global::DG.Tweening.DOTween.Kill(this.label, false);
	}

	public void Play(global::UnityEngine.Vector3 position, bool loc, string text, params string[] parameters)
	{
		this.Play(position, null, loc, text, parameters);
	}

	public void Play(global::UnityEngine.Vector3 position, global::UnityEngine.Transform anchor, bool loc, string text, params string[] parameters)
	{
		base.Play(position, anchor);
		this.label.text = ((!loc) ? string.Format(text, parameters) : global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(text, parameters));
		this.label.color = this.startColor;
		if (this.duration > 0f)
		{
			global::DG.Tweening.DOTween.To(() => this.label.color, delegate(global::UnityEngine.Color c)
			{
				this.label.color = c;
			}, this.endColor, this.duration / 2f).SetDelay(this.duration / 2f);
		}
	}

	private global::UnityEngine.UI.Text label;
}
