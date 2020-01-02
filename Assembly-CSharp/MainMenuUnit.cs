using System;
using UnityEngine;

public class MainMenuUnit : global::UnityEngine.MonoBehaviour
{
	private void Awake()
	{
		this.animator = base.GetComponent<global::UnityEngine.Animator>();
		this.dissolver = base.GetComponent<global::Dissolver>();
		this.shaderSetter = base.GetComponent<global::ShaderSetter>();
	}

	private void Start()
	{
		this.animator.Rebind();
		this.shaderSetter.ApplyShaderParams();
	}

	public void LaunchAction(global::UnitActionId id, bool success, global::UnitStateId stateId, int variation = 0)
	{
		this.animator.SetTrigger("shout");
	}

	public void Hide(bool visible)
	{
		this.dissolver.Hide(visible, true, null);
	}

	public void EventSound(string soundName)
	{
	}

	public void EventShout(string soundName)
	{
		soundName = soundName.Replace("(unit)", this.unitId.ToLowerString());
		this.PlaySound(soundName);
	}

	private void PlaySound(string soundName)
	{
		global::PandoraSingleton<global::Pan>.Instance.GetSound(soundName, true, delegate(global::UnityEngine.AudioClip clip)
		{
			if (clip != null && this.audioSource != null)
			{
				this.audioSource.PlayOneShot(clip);
			}
		});
	}

	public global::UnitId unitId;

	public global::WarbandId warbandId;

	public global::UnityEngine.AudioSource audioSource;

	private global::UnityEngine.Animator animator;

	private global::Dissolver dissolver;

	private global::ShaderSetter shaderSetter;
}
