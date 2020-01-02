using System;
using System.Collections.Generic;
using UnityEngine;
using WellFired;

public class test_useq : global::UnityEngine.MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
		if (this.loaded && global::UnityEngine.Input.GetKeyDown(global::UnityEngine.KeyCode.Space) && this.sequence != null)
		{
			global::UnityEngine.Debug.Log("PLAY SEQUENCE");
			global::UnityEngine.Camera.main.transform.parent = this.anchors[this.anchorIdx];
			if (this.anchors[this.anchorIdx] != null)
			{
				global::UnityEngine.Camera.main.transform.localPosition = global::UnityEngine.Vector3.zero;
				global::UnityEngine.Camera.main.transform.localRotation = global::UnityEngine.Quaternion.identity;
			}
			this.anchorIdx = (this.anchorIdx + 1) % this.anchors.Count;
			this.sequence.Stop();
			this.sequence.Play();
		}
		if (!this.loaded && global::UnityEngine.Input.GetKeyDown(global::UnityEngine.KeyCode.Space))
		{
			global::UnityEngine.Debug.Log("LOADING SEQUENCE");
			global::UnityEngine.GameObject gameObject = (global::UnityEngine.GameObject)global::UnityEngine.Object.Instantiate(global::UnityEngine.Resources.Load("prefabs/sequences/" + this.sequenceName + "/" + this.sequenceName));
			this.sequence = gameObject.GetComponent<global::WellFired.USSequencer>();
			if (this.sequence == null)
			{
				global::UnityEngine.Debug.Log("SEQUENCE NULL!!!!");
			}
			else
			{
				this.loaded = true;
				global::UnityEngine.Debug.Log("SEQUENCE LOADED");
			}
		}
	}

	public global::System.Collections.Generic.List<global::UnityEngine.Transform> anchors;

	public string sequenceName;

	private global::WellFired.USSequencer sequence;

	private bool loaded;

	private int anchorIdx;
}
