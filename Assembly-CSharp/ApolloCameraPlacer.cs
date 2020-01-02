using System;
using System.Collections.Generic;
using UnityEngine;

public class ApolloCameraPlacer : global::UnityEngine.MonoBehaviour
{
	private void Start()
	{
		this.idx = -1;
		this.cameraPositions = new global::System.Collections.Generic.List<global::UnityEngine.Transform>();
		for (int i = 0; i < base.transform.childCount; i++)
		{
			this.cameraPositions.Add(base.transform.GetChild(i));
		}
		this.cam = global::UnityEngine.Camera.main.gameObject.GetComponent<global::CameraManager>();
	}

	private void Update()
	{
		if (this.next)
		{
			this.next = false;
			global::UnityEngine.Transform transform = this.cameraPositions[++this.idx % this.cameraPositions.Count];
			this.cam.dummyCam.transform.position = transform.position;
			this.cam.dummyCam.transform.rotation = transform.rotation;
			global::UnityEngine.GameObject gameObject = global::UnityEngine.GameObject.Find("movement_lines");
			if (gameObject)
			{
				gameObject.SetActive(false);
			}
		}
	}

	public bool next;

	private int idx;

	private global::System.Collections.Generic.List<global::UnityEngine.Transform> cameraPositions;

	private global::CameraManager cam;
}
