using System;
using UnityEngine;

public class CameraLayerCull : global::UnityEngine.MonoBehaviour
{
	private void Start()
	{
		base.gameObject.GetComponent<global::UnityEngine.Camera>().layerCullDistances = this.distances;
		this.update = false;
	}

	private void Update()
	{
		if (this.update)
		{
			this.update = false;
			base.gameObject.GetComponent<global::UnityEngine.Camera>().layerCullDistances = this.distances;
		}
	}

	public bool update;

	public float[] distances = new float[32];
}
