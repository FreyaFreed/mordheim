using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pan : global::PandoraSingleton<global::Pan>
{
	public bool Initialized { get; private set; }

	private void Awake()
	{
		this.Init();
	}

	public void Init()
	{
		this.nextMusic = null;
		this.currentMusic = null;
		this.audioSourcesFx = new global::System.Collections.Generic.List<global::UnityEngine.AudioSource>();
		this.audioSourcesVoice = new global::System.Collections.Generic.List<global::UnityEngine.AudioSource>();
		this.sounds = new global::System.Collections.Generic.Dictionary<int, global::UnityEngine.AudioClip>();
		this.narrations = new global::System.Collections.Generic.Queue<global::UnityEngine.AudioClip>();
		this.Initialized = true;
	}

	private void OnDestroy()
	{
		this.Clear();
	}

	public void Clear()
	{
		foreach (global::UnityEngine.AudioSource obj in this.audioSourcesFx)
		{
			global::UnityEngine.Object.Destroy(obj);
		}
		this.audioSourcesFx.Clear();
		foreach (global::UnityEngine.AudioSource obj2 in this.audioSourcesVoice)
		{
			global::UnityEngine.Object.Destroy(obj2);
		}
		this.audioSourcesVoice.Clear();
		this.sounds.Clear();
	}

	public void SetVolume(global::Pan.Type type, float volume)
	{
		switch (type)
		{
		case global::Pan.Type.MASTER:
			this.masterVolume = global::UnityEngine.Mathf.Clamp(volume, 0f, 1f);
			global::UnityEngine.AudioListener.volume = this.masterVolume;
			break;
		case global::Pan.Type.FX:
			this.fxVolume = global::UnityEngine.Mathf.Clamp(volume, 0f, 1f);
			foreach (global::UnityEngine.AudioSource audioSource in this.audioSourcesFx)
			{
				if (audioSource != null)
				{
					audioSource.volume = this.fxVolume;
				}
			}
			break;
		case global::Pan.Type.MUSIC:
			this.musicVolume = global::UnityEngine.Mathf.Clamp(volume, 0f, 1f);
			if (this.currentMusic != null)
			{
				this.currentMusic.volume = this.musicVolume;
			}
			break;
		case global::Pan.Type.AMBIENT:
			this.ambientVolume = global::UnityEngine.Mathf.Clamp(volume, 0f, 1f);
			if (this.ambientSource != null)
			{
				this.ambientSource.volume = this.ambientVolume;
			}
			break;
		case global::Pan.Type.VOICE:
			this.voiceVolume = global::UnityEngine.Mathf.Clamp(volume, 0f, 1f);
			foreach (global::UnityEngine.AudioSource audioSource2 in this.audioSourcesVoice)
			{
				if (audioSource2 != null)
				{
					audioSource2.volume = this.voiceVolume;
				}
			}
			if (this.audioSourceNarrator != null)
			{
				this.audioSourceNarrator.volume = this.voiceVolume;
			}
			break;
		case global::Pan.Type.NARRATOR:
			if (this.audioSourceNarrator != null)
			{
				this.audioSourceNarrator.volume = this.voiceVolume;
			}
			break;
		}
	}

	public void PlayMusic(string name, bool ambiance)
	{
		global::PandoraDebug.LogDebug("Play Music = " + name, "uncategorised", null);
		if (this.nextMusic == null)
		{
			global::UnityEngine.GameObject gameObject = new global::UnityEngine.GameObject();
			global::UnityEngine.AudioSource audioSource = gameObject.AddComponent<global::UnityEngine.AudioSource>();
			this.AddSource(audioSource, global::Pan.Type.MUSIC);
		}
		if (this.nextMusic != null)
		{
			global::PandoraDebug.LogDebug("Music LOAD = " + name, "uncategorised", null);
			this.GetSound(name, false, delegate(global::UnityEngine.AudioClip clip)
			{
				global::PandoraDebug.LogDebug("Music LOADED = " + name, "uncategorised", null);
				this.nextMusic.clip = clip;
				this.PlayMusic();
				this.SetAmbiance(ambiance);
			});
		}
	}

	public void PlayMusic()
	{
		if (this.currentMusic != null && this.currentMusic.isPlaying)
		{
			global::PandoraDebug.LogDebug("PlayMusic  = " + base.name, "uncategorised", null);
			base.StartCoroutine(this.Fade(this.currentMusic, this.musicVolume, 0f, true, null));
		}
		this.currentMusic = this.nextMusic;
		if (this.currentMusic != null && !this.currentMusic.isPlaying)
		{
			global::UnityEngine.Object.DontDestroyOnLoad(this.currentMusic.gameObject);
			base.StartCoroutine(this.Fade(this.currentMusic, 0f, this.musicVolume, false, null));
		}
		this.nextMusic = null;
	}

	public void PauseMusic(bool fade = true)
	{
		if (this.currentMusic && this.currentMusic.isPlaying)
		{
			if (fade)
			{
				base.StartCoroutine(this.Fade(this.currentMusic, this.musicVolume, 0f, false, delegate
				{
					if (this.currentMusic != null)
					{
						this.currentMusic.Pause();
					}
				}));
			}
			else
			{
				this.currentMusic.Pause();
			}
		}
	}

	public void UnPauseMusic(bool fade = true)
	{
		if (this.currentMusic)
		{
			if (fade)
			{
				base.StartCoroutine(this.Fade(this.currentMusic, 0f, this.musicVolume, false, delegate
				{
					if (this.currentMusic != null)
					{
						this.currentMusic.UnPause();
					}
				}));
			}
			else
			{
				this.currentMusic.UnPause();
			}
		}
	}

	public void SoundsOn()
	{
		base.StartCoroutine(this.FadeSoundsVolume(true));
	}

	public void SoundsOff()
	{
		base.StartCoroutine(this.FadeSoundsVolume(false));
	}

	private global::System.Collections.IEnumerator FadeSoundsVolume(bool on)
	{
		float time = 0f;
		float fxVolTo = 0f;
		float fxVolFr = this.fxVolume;
		float ambientVolTo = 0f;
		float ambientVolFr = this.ambientVolume;
		float narVolTo = 0f;
		float narVolFr = this.voiceVolume;
		if (on)
		{
			fxVolTo = global::PandoraSingleton<global::GameManager>.Instance.Options.fxVolume;
			fxVolFr = 0f;
			ambientVolTo = global::PandoraSingleton<global::GameManager>.Instance.Options.ambientVolume;
			ambientVolFr = 0f;
			narVolTo = global::PandoraSingleton<global::GameManager>.Instance.Options.voiceVolume;
			narVolFr = 0f;
		}
		while (time < 3f)
		{
			float newVolFx = global::UnityEngine.Mathf.Lerp(fxVolFr, fxVolTo, time / 3f);
			float newVolamb = global::UnityEngine.Mathf.Lerp(ambientVolFr, ambientVolTo, time / 3f);
			float newVolNar = global::UnityEngine.Mathf.Lerp(narVolFr, narVolTo, time / 3f);
			this.SetVolume(global::Pan.Type.FX, newVolFx);
			this.SetVolume(global::Pan.Type.NARRATOR, newVolNar);
			this.SetVolume(global::Pan.Type.AMBIENT, newVolNar);
			yield return 0;
			time += global::UnityEngine.Time.smoothDeltaTime;
		}
		yield break;
	}

	private global::System.Collections.IEnumerator Fade(global::UnityEngine.AudioSource sound, float fromV, float toV, bool kill, global::System.Action onFadeDone = null)
	{
		if (sound == null)
		{
			yield break;
		}
		float time = 0f;
		sound.volume = fromV;
		if (!sound.isPlaying)
		{
			sound.Play();
		}
		while (time < 3f && sound != null)
		{
			sound.volume = global::UnityEngine.Mathf.Lerp(fromV, toV, time / 3f);
			yield return 0;
			time += global::UnityEngine.Time.smoothDeltaTime;
		}
		if (sound != null)
		{
			sound.volume = toV;
		}
		if (onFadeDone != null)
		{
			onFadeDone();
		}
		if (kill)
		{
			global::UnityEngine.Object.Destroy(sound.gameObject);
		}
		yield break;
	}

	public void SetAmbiance(bool on)
	{
		if (on)
		{
			base.StartCoroutine(this.PlayAmbiance());
		}
		else
		{
			base.StopCoroutine(this.PlayAmbiance());
		}
	}

	private global::System.Collections.IEnumerator PlayAmbiance()
	{
		global::Tyche tyche = global::PandoraSingleton<global::GameManager>.Instance.LocalTyche;
		global::UnityEngine.Vector3 pos = default(global::UnityEngine.Vector3);
		for (;;)
		{
			yield return new global::UnityEngine.WaitForSeconds((float)tyche.Rand(50, 75));
			if (this.ambientSource != null)
			{
				pos.x = (float)tyche.Rand(-75, 75);
				pos.y = (float)tyche.Rand(-20, 20);
				pos.z = (float)tyche.Rand(-75, 75);
				this.ambientSource.transform.position = pos;
				this.GetSound(this.ambients[tyche.Rand(0, this.ambients.Length)], true, new global::System.Action<global::UnityEngine.AudioClip>(this.PlayAmbiantSound));
			}
		}
		yield break;
	}

	private void PlayAmbiantSound(global::UnityEngine.AudioClip clip)
	{
		if (this.ambientSource != null)
		{
			this.ambientSource.PlayOneShot(clip);
		}
	}

	public void Narrate(string narration)
	{
		this.GetSound("voices/narrator/", narration, false, delegate(global::UnityEngine.AudioClip clip)
		{
			if (this.audioSourceNarrator.isPlaying && clip != this.audioSourceNarrator.clip)
			{
				if (!this.narrations.Contains(clip))
				{
					this.narrations.Enqueue(clip);
					base.StopCoroutine(this.NarrateQueue());
					base.StartCoroutine(this.NarrateQueue());
				}
			}
			else
			{
				this.audioSourceNarrator.clip = clip;
				this.audioSourceNarrator.Play();
			}
		});
	}

	private global::System.Collections.IEnumerator NarrateQueue()
	{
		while (this.audioSourceNarrator.isPlaying)
		{
			yield return new global::UnityEngine.WaitForSeconds(1f);
			if (!this.audioSourceNarrator.isPlaying && this.narrations.Count > 0)
			{
				global::UnityEngine.AudioClip clip = this.narrations.Dequeue();
				this.audioSourceNarrator.clip = clip;
				this.audioSourceNarrator.Play();
			}
		}
		yield break;
	}

	public void AddSource(global::PanFlute audioEmitter)
	{
		this.AddSource(audioEmitter.GetComponent<global::UnityEngine.AudioSource>(), audioEmitter.fluteType);
	}

	public void AddSource(global::UnityEngine.AudioSource audioSource, global::Pan.Type fluteType)
	{
		if (audioSource == null)
		{
			return;
		}
		switch (fluteType)
		{
		case global::Pan.Type.FX:
			this.audioSourcesFx.Add(audioSource);
			audioSource.volume = this.fxVolume;
			break;
		case global::Pan.Type.MUSIC:
			if (this.nextMusic == null)
			{
				this.nextMusic = audioSource;
			}
			this.nextMusic.volume = this.musicVolume;
			this.nextMusic.loop = true;
			this.nextMusic.spatialBlend = 0f;
			break;
		case global::Pan.Type.AMBIENT:
			if (this.ambientSource == null)
			{
				this.ambientSource = audioSource;
				this.ambientSource.volume = this.ambientVolume;
			}
			break;
		case global::Pan.Type.VOICE:
			this.audioSourcesVoice.Add(audioSource);
			audioSource.volume = this.voiceVolume;
			audioSource.loop = false;
			audioSource.spatialBlend = 0f;
			break;
		case global::Pan.Type.NARRATOR:
			if (this.audioSourceNarrator == null)
			{
				this.audioSourceNarrator = audioSource;
				this.audioSourceNarrator.volume = this.voiceVolume;
				this.audioSourceNarrator.loop = false;
				this.audioSourceNarrator.spatialBlend = 0f;
			}
			break;
		}
	}

	public void RemoveSource(global::UnityEngine.AudioSource source, global::Pan.Type type)
	{
		switch (type)
		{
		case global::Pan.Type.FX:
			this.audioSourcesFx.Remove(source);
			break;
		case global::Pan.Type.MUSIC:
			if (this.currentMusic == source)
			{
				source.Stop();
				global::UnityEngine.Object.Destroy(source);
			}
			break;
		case global::Pan.Type.AMBIENT:
			this.ambientSource = null;
			break;
		case global::Pan.Type.VOICE:
			this.audioSourcesVoice.Remove(source);
			break;
		case global::Pan.Type.NARRATOR:
			this.audioSourceNarrator = null;
			break;
		}
	}

	public void GetSound(string soundName, bool cache, global::System.Action<global::UnityEngine.AudioClip> OnLoad)
	{
		this.GetSound(string.Empty, soundName, cache, OnLoad);
	}

	public void GetSound(string path, string soundName, bool cache, global::System.Action<global::UnityEngine.AudioClip> OnLoad)
	{
		int hash = 0;
		if (cache)
		{
			hash = soundName.GetHashCode();
			if (this.sounds.ContainsKey(hash))
			{
				OnLoad(this.sounds[hash]);
				return;
			}
		}
		global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadAssetAsync<global::UnityEngine.AudioClip>("Assets/sounds/runtime/" + path, global::AssetBundleId.SOUNDS, soundName + ".ogg", delegate(global::UnityEngine.Object sound)
		{
			global::UnityEngine.AudioClip audioClip = (global::UnityEngine.AudioClip)sound;
			if (audioClip == null)
			{
				global::PandoraDebug.LogWarning("Sound missing : " + soundName, "SOUND", null);
				OnLoad(null);
			}
			else
			{
				if (cache && hash != 0)
				{
					this.sounds[hash] = audioClip;
				}
				OnLoad(audioClip);
			}
		});
	}

	public const float DEFAULT_VOLUME = 1f;

	public const float DEFAULT_MUSIC_VOLUME = 0.45f;

	public const float DEFAULT_AMBIENT_VOLUME = 0.75f;

	private const float MUSIC_FADE_TIME = 3f;

	private readonly string[] ambients = new string[]
	{
		"bad_entity",
		"butcher_door",
		"corpse_decay",
		"corrupted_priests",
		"creature_chain",
		"death_breath",
		"deep_down",
		"demon",
		"exhale",
		"freak_lament",
		"ghost_whisper",
		"horror_boom",
		"laugh_madness",
		"old_tension",
		"possessed_baby",
		"pure_chaos",
		"secret_winch",
		"soul_eater",
		"spirit_leech",
		"torture_shouts",
		"underworld_portal"
	};

	public float masterVolume = 1f;

	private global::System.Collections.Generic.List<global::UnityEngine.AudioSource> audioSourcesFx = new global::System.Collections.Generic.List<global::UnityEngine.AudioSource>();

	public float fxVolume = 1f;

	[global::UnityEngine.SerializeField]
	private global::UnityEngine.AudioSource currentMusic;

	[global::UnityEngine.SerializeField]
	private global::UnityEngine.AudioSource nextMusic;

	public float musicVolume = 0.45f;

	private global::System.Collections.Generic.List<global::UnityEngine.AudioSource> audioSourcesVoice = new global::System.Collections.Generic.List<global::UnityEngine.AudioSource>();

	private global::UnityEngine.AudioSource audioSourceNarrator;

	private global::System.Collections.Generic.Queue<global::UnityEngine.AudioClip> narrations;

	public float voiceVolume = 1f;

	private global::System.Collections.Generic.List<global::UnityEngine.AudioClip> clipAmbient = new global::System.Collections.Generic.List<global::UnityEngine.AudioClip>();

	private global::UnityEngine.AudioSource ambientSource;

	public float ambientVolume = 0.75f;

	private global::System.Collections.Generic.Dictionary<int, global::UnityEngine.AudioClip> sounds;

	public enum Type
	{
		MASTER,
		FX,
		MUSIC,
		AMBIENT,
		VOICE,
		NARRATOR
	}
}
