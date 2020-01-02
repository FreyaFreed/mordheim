using System;
using HighlightingSystem;
using UnityEngine;

public class TriggerPoint : global::UnityEngine.MonoBehaviour
{
	public global::HighlightingSystem.Highlighter Highlight { get; private set; }

	public void Init()
	{
		this.anim = base.GetComponent<global::UnityEngine.Animation>();
		this.audioSource = base.GetComponent<global::UnityEngine.AudioSource>();
		global::UnityEngine.Renderer component = this.trigger.GetComponent<global::UnityEngine.Renderer>();
		if (component != null)
		{
			component.enabled = false;
		}
		this.Highlight = base.gameObject.GetComponent<global::HighlightingSystem.Highlighter>();
		if (this.Highlight == null)
		{
			this.Highlight = base.gameObject.AddComponent<global::HighlightingSystem.Highlighter>();
		}
		this.Highlight.seeThrough = false;
		if (this.soundName != string.Empty && this.audioSource != null)
		{
			global::PanFlute panFlute = base.gameObject.AddComponent<global::PanFlute>();
			panFlute.fluteType = global::Pan.Type.FX;
			this.audioSource.loop = false;
			this.audioSource.playOnAwake = false;
			this.audioSource.enabled = false;
		}
	}

	public virtual void Trigger(global::UnitController currentUnit)
	{
		if (this.anim != null)
		{
			this.anim.Play();
		}
		if (this.fx != null)
		{
			this.fx.transform.localPosition = global::UnityEngine.Vector3.zero;
			this.fx.transform.localRotation = global::UnityEngine.Quaternion.identity;
		}
		if (this.soundName != string.Empty && this.audioSource != null)
		{
			global::PandoraSingleton<global::Pan>.Instance.GetSound(this.soundName, true, delegate(global::UnityEngine.AudioClip clip)
			{
				this.audioSource.enabled = true;
				this.audioSource.PlayOneShot(clip);
			});
		}
	}

	public virtual void ActionOnUnit(global::UnitController currentUnit)
	{
	}

	public virtual bool IsActive()
	{
		return true;
	}

	public global::UnityEngine.GameObject trigger;

	public string soundName;

	protected global::UnityEngine.GameObject fx;

	private global::UnityEngine.Animation anim;

	private global::UnityEngine.AudioSource audioSource;

	[global::UnityEngine.HideInInspector]
	public uint guid;
}
