using System;
using System.Collections;
using UnityEngine;

public class UISound : global::UnityEngine.MonoBehaviour
{
	public static global::UISound Instance
	{
		get
		{
			return global::UISound._instance;
		}
	}

	private void Awake()
	{
		if (global::UISound._instance != null)
		{
			global::UnityEngine.Object.Destroy(this);
			return;
		}
		global::UISound._instance = this;
		global::UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		this.source = base.GetComponent<global::UnityEngine.AudioSource>();
		this.LoadSounds();
	}

	private global::System.Collections.IEnumerator WaitForPan()
	{
		while (!global::PandoraSingleton<global::Pan>.Instance.Initialized)
		{
			yield return null;
		}
		this.LoadSounds();
		yield break;
	}

	private void LoadSounds()
	{
		global::PandoraSingleton<global::Pan>.Instance.GetSound("interface/", "click", true, delegate(global::UnityEngine.AudioClip clip)
		{
			this.click = clip;
		});
		global::PandoraSingleton<global::Pan>.Instance.GetSound("interface/", "mouseover", true, delegate(global::UnityEngine.AudioClip clip)
		{
			this.select = clip;
		});
	}

	public void OnSelect()
	{
		this.playSelect = true;
	}

	public void OnClick()
	{
		this.playClick = true;
	}

	private void OnDestroy()
	{
		global::UISound._instance = null;
	}

	private void Update()
	{
		this.time += global::UnityEngine.Time.deltaTime;
		if (this.time > 0.1f)
		{
			if (this.playClick)
			{
				this.playSelect = false;
				this.playClick = false;
				this.source.PlayOneShot(this.click);
				this.time = 0f;
			}
			else if (this.playSelect)
			{
				this.playSelect = false;
				this.source.PlayOneShot(this.select);
				this.time = 0f;
			}
		}
	}

	private static global::UISound _instance;

	private global::UnityEngine.AudioClip click;

	private global::UnityEngine.AudioClip select;

	private global::UnityEngine.AudioSource source;

	private bool playSelect;

	private bool playClick;

	private float time;
}
