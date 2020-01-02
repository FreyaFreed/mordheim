using System;
using UnityEngine;

public class DescriptionModule : global::UIModule
{
	public override void Init()
	{
		base.Init();
		global::PandoraDebug.LogDebug("Description Module Init", "uncategorised", null);
		global::UnityEngine.GameObject gameObject = global::UnityEngine.Object.Instantiate<global::UnityEngine.GameObject>(this.descPrefab);
		gameObject.transform.SetParent(base.transform, false);
		this.desc = gameObject.GetComponent<global::UIDescription>();
	}

	public void Show(bool visible)
	{
		this.desc.gameObject.SetActive(visible);
	}

	public global::UnityEngine.GameObject descPrefab;

	[global::UnityEngine.HideInInspector]
	public global::UIDescription desc;
}
