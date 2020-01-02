using System;
using System.Collections.Generic;
using UnityEngine;

public class SearchZone : global::UnityEngine.MonoBehaviour
{
	private void Start()
	{
		this.bounds = new global::System.Collections.Generic.List<global::UnityEngine.Bounds>();
		for (int i = 0; i < base.transform.childCount; i++)
		{
			global::UnityEngine.Transform child = base.transform.GetChild(i);
			if (child.name == "bounds")
			{
				global::UnityEngine.Bounds item = child.GetComponent<global::UnityEngine.Renderer>().bounds;
				this.bounds.Add(item);
			}
		}
		global::SearchZoneId searchZoneId = this.type;
		if (searchZoneId == global::SearchZoneId.WYRDSTONE_CONCENTRATION || searchZoneId == global::SearchZoneId.WYRDSTONE_CLUSTER)
		{
			if (this.bounds.Count == 0)
			{
				global::PandoraDebug.LogError("SearchZone with no bounds", "DEPLOY", this);
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

	public global::SearchZoneId type;

	[global::UnityEngine.HideInInspector]
	public bool claimed;

	private global::System.Collections.Generic.List<global::UnityEngine.Bounds> bounds;
}
