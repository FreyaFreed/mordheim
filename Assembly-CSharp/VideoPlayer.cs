using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[global::UnityEngine.RequireComponent(typeof(global::UnityEngine.UI.RawImage))]
public class VideoPlayer : global::UnityEngine.MonoBehaviour
{
	private void Awake()
	{
		this.rawImage = base.GetComponent<global::UnityEngine.UI.RawImage>();
		this.videoSound = base.GetComponent<global::UnityEngine.AudioSource>();
	}

	public void Stop()
	{
		if (this.currentVideo != null)
		{
			this.currentVideo.Stop();
		}
	}

	private void Update()
	{
		if (this.videoStarted && this.currentVideo != null && !this.currentVideo.isPlaying)
		{
			this.videoStarted = false;
			this.OnVideoEnd();
		}
	}

	public global::System.Collections.IEnumerator Play(string path, global::System.Action onVideoDone)
	{
		this.onVideoEnd = onVideoDone;
		global::UnityEngine.ResourceRequest resourceRequest = global::UnityEngine.Resources.LoadAsync<global::UnityEngine.MovieTexture>("video/" + path);
		yield return resourceRequest;
		this.currentVideo = (global::UnityEngine.MovieTexture)resourceRequest.asset;
		this.rawImage.texture = this.currentVideo;
		this.videoSound.clip = this.currentVideo.audioClip;
		this.rawImage.color = global::UnityEngine.Color.white;
		this.videoStarted = true;
		this.currentVideo.Play();
		this.videoSound.Play();
		yield break;
	}

	private void OnVideoEnd()
	{
		if (this.onVideoEnd != null)
		{
			this.onVideoEnd();
		}
	}

	private global::UnityEngine.UI.RawImage rawImage;

	private global::UnityEngine.AudioSource videoSound;

	private global::UnityEngine.MovieTexture currentVideo;

	private global::System.Action onVideoEnd;

	private bool videoStarted;

	private bool videoEnded;
}
