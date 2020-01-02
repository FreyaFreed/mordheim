using System;
using UnityEngine;

public class Beacon : global::UnityEngine.MonoBehaviour
{
	public void SetActive(bool active)
	{
		this.visual.SetActive(active);
	}

	public global::UnityEngine.GameObject visual;

	public int idx;
}
