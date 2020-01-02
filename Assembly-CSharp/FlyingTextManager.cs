using System;
using System.Collections.Generic;
using UnityEngine;

public class FlyingTextManager : global::PandoraSingleton<global::FlyingTextManager>
{
	public void Init()
	{
		this.canvas.worldCamera = global::UnityEngine.Camera.main;
		this.flyingTexts = new global::System.Collections.Generic.Dictionary<global::FlyingTextId, global::System.Collections.Generic.List<global::FlyingText>>();
		this.AddContainer("misc", ref this.miscContainer);
		this.AddContainer("deploy", ref this.deploymentContainter);
		this.AddContainer("unit", ref this.unitContainer);
		this.AddContainer("beacon", ref this.beaconContainer);
		this.ResetWorldCorners();
	}

	private void AddContainer(string name, ref global::UnityEngine.Transform trans)
	{
		global::UnityEngine.GameObject gameObject = new global::UnityEngine.GameObject();
		gameObject.name = name;
		gameObject.transform.SetParent(base.transform);
		gameObject.transform.localPosition = global::UnityEngine.Vector3.zero;
		gameObject.transform.localRotation = global::UnityEngine.Quaternion.identity;
		trans = gameObject.transform;
	}

	public void ResetWorldCorners()
	{
		(base.transform.parent as global::UnityEngine.RectTransform).GetWorldCorners(this.canvasCorners);
	}

	public void GetFlyingText(global::FlyingTextId id, global::System.Action<global::FlyingText> cb)
	{
		global::FlyingText flyingTxt = null;
		if (!this.flyingTexts.ContainsKey(id))
		{
			this.flyingTexts[id] = new global::System.Collections.Generic.List<global::FlyingText>();
		}
		for (int i = 0; i < this.flyingTexts[id].Count; i++)
		{
			if (this.flyingTexts[id][i].Done)
			{
				flyingTxt = this.flyingTexts[id][i];
				flyingTxt.gameObject.SetActive(true);
				cb(flyingTxt);
				return;
			}
		}
		global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadResourceAsync<global::UnityEngine.GameObject>("prefabs/flying_text/" + id.ToLowerString(), delegate(global::UnityEngine.Object flyPrefab)
		{
			flyingTxt = global::UnityEngine.Object.Instantiate<global::UnityEngine.GameObject>((global::UnityEngine.GameObject)flyPrefab).GetComponent<global::FlyingText>();
			flyingTxt.transform.SetParent(this.transform, false);
			this.flyingTexts[id].Add(flyingTxt);
			flyingTxt.gameObject.SetActive(true);
			cb(flyingTxt);
		}, true);
	}

	public global::UnityEngine.Canvas canvas;

	private global::System.Collections.Generic.Dictionary<global::FlyingTextId, global::System.Collections.Generic.List<global::FlyingText>> flyingTexts;

	public global::UnityEngine.Vector3[] canvasCorners = new global::UnityEngine.Vector3[4];

	public global::UnityEngine.Transform beaconContainer;

	public global::UnityEngine.Transform unitContainer;

	public global::UnityEngine.Transform deploymentContainter;

	public global::UnityEngine.Transform miscContainer;
}
