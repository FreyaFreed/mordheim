using System;
using System.Collections.Generic;
using UnityEngine;

public class SearchArea : global::UnityEngine.MonoBehaviour
{
	private void Start()
	{
		this.bounds = new global::System.Collections.Generic.List<global::UnityEngine.Bounds>();
		for (int i = 0; i < base.transform.childCount; i++)
		{
			global::UnityEngine.Transform child = base.transform.GetChild(i);
			if (child.name.Contains("bounds"))
			{
				global::UnityEngine.Bounds item = child.GetComponent<global::UnityEngine.Renderer>().bounds;
				this.bounds.Add(item);
			}
		}
	}

	public bool Contains(global::UnityEngine.Vector3 pos)
	{
		for (int i = 0; i < this.bounds.Count; i++)
		{
			if (this.bounds[i].Contains(pos))
			{
				return true;
			}
		}
		return false;
	}

	private global::System.Collections.Generic.List<global::UnityEngine.Bounds> bounds;
}
