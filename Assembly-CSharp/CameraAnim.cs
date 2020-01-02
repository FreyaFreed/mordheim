using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraAnim : global::ICheapState
{
	public CameraAnim(global::CameraManager camMngr)
	{
		this.mngr = camMngr;
		this.dummyCam = camMngr.dummyCam.transform;
		this.camAnimRef = new global::UnityEngine.GameObject("camera_animator");
		this.camAnimRef.AddComponent<global::UnityEngine.Animation>();
		global::UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(this.camAnimRef.gameObject, camMngr.gameObject.scene);
		this.anchor = new global::UnityEngine.GameObject("camera_anchor");
		global::UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(this.anchor.gameObject, camMngr.gameObject.scene);
		this.camAnimRef.GetComponent<global::UnityEngine.Animation>().Stop();
	}

	public void Destroy()
	{
	}

	public void Enter(int from)
	{
	}

	public void Exit(int to)
	{
		this.AnimationDone();
	}

	public void Update()
	{
		if (!this.camAnimRef.GetComponent<global::UnityEngine.Animation>().isPlaying && (this.relative || this.onDone != null))
		{
			this.AnimationDone();
		}
	}

	public void FixedUpdate()
	{
	}

	public void AnimationDone()
	{
		if (this.camAnimRef)
		{
			this.camAnimRef.GetComponent<global::UnityEngine.Animation>().Stop();
			if (this.relative)
			{
				this.relative = false;
				this.dummyCam.SetParent(null);
			}
			if (this.onDone != null)
			{
				global::CamDelegate camDelegate = this.onDone;
				this.onDone = null;
				camDelegate();
			}
		}
	}

	private void Play(string clipName)
	{
		this.camAnimRef.GetComponent<global::UnityEngine.Animation>().Stop();
		if (this.camAnimRef.GetComponent<global::UnityEngine.Animation>()[clipName] == null)
		{
			global::UnityEngine.AnimationClip clip = global::UnityEngine.Resources.Load<global::UnityEngine.AnimationClip>("camera/clips/" + clipName);
			this.camAnimRef.GetComponent<global::UnityEngine.Animation>().AddClip(clip, clipName);
		}
		this.camAnimRef.GetComponent<global::UnityEngine.Animation>().Play(clipName);
	}

	public void PlayRelative(string clip, global::UnityEngine.Transform anchorRef, global::CamDelegate done = null)
	{
		this.PlayRelative(clip, anchorRef.position, anchorRef.rotation, done);
	}

	public void PlayRelative(string clip, global::UnityEngine.Vector3 position, global::UnityEngine.Quaternion rotation, global::CamDelegate done = null)
	{
		this.relative = true;
		this.onDone = done;
		this.anchor.transform.position = position;
		this.anchor.transform.rotation = rotation;
		this.camAnimRef.transform.SetParent(this.anchor.transform);
		this.dummyCam.SetParent(this.camAnimRef.transform);
		this.dummyCam.localPosition = global::UnityEngine.Vector3.zero;
		this.dummyCam.localRotation = global::UnityEngine.Quaternion.identity;
		this.Play(clip);
	}

	public void PlayAttached(string clip, global::UnityEngine.Transform anchorRef, global::CamDelegate done = null)
	{
		this.relative = true;
		this.onDone = done;
		this.camAnimRef.transform.SetParent(anchorRef.transform);
		this.dummyCam.SetParent(this.camAnimRef.transform);
		this.dummyCam.localPosition = global::UnityEngine.Vector3.zero;
		this.dummyCam.localRotation = global::UnityEngine.Quaternion.identity;
		this.Play(clip);
	}

	public void Stop()
	{
		this.camAnimRef.GetComponent<global::UnityEngine.Animation>().Stop();
	}

	private global::UnityEngine.GameObject camAnimRef;

	private global::UnityEngine.GameObject anchor;

	private bool relative;

	private global::CamDelegate onDone;

	private global::CameraManager mngr;

	private global::UnityEngine.Transform dummyCam;
}
