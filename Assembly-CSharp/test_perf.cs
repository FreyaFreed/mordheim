using System;
using System.Collections.Generic;
using UnityEngine;

public class test_perf : global::UnityEngine.MonoBehaviour
{
	private void Start()
	{
		this.currentPrefabidx = 0;
		this.countLabelRect = new global::UnityEngine.Rect(0f, 0f, 200f, 50f);
		this.modelLabelRect = new global::UnityEngine.Rect(200f, 0f, 200f, 50f);
		this.layerMask = 512;
		this.generatedObj = new global::System.Collections.Generic.List<global::UnityEngine.GameObject>();
	}

	private void Update()
	{
		if (this.generatedObj.Count < this.limit && global::UnityEngine.Input.GetMouseButton(0) && this.timer >= 0f)
		{
			this.timer -= global::UnityEngine.Time.deltaTime;
			if (this.timer <= 0f)
			{
				global::UnityEngine.Ray ray = base.gameObject.GetComponent<global::UnityEngine.Camera>().ScreenPointToRay(global::UnityEngine.Input.mousePosition);
				if (global::UnityEngine.Physics.Raycast(ray, out this.hitInfo, 100000f, this.layerMask.value) && this.currentPrefabidx < this.prefabs.Count)
				{
					global::UnityEngine.GameObject gameObject = global::UnityEngine.Object.Instantiate(this.prefabs[this.currentPrefabidx], this.hitInfo.point, global::UnityEngine.Quaternion.identity) as global::UnityEngine.GameObject;
					gameObject.transform.parent = this.anchor.transform;
					gameObject.isStatic = this.markStatic;
					this.generatedObj.Add(gameObject);
					global::UnityEngine.Debug.DrawRay(ray.origin, this.hitInfo.point - ray.origin, global::UnityEngine.Color.red);
				}
				this.timer = this.lag;
			}
		}
		if (global::UnityEngine.Input.GetKeyDown(global::UnityEngine.KeyCode.PageDown))
		{
			this.currentPrefabidx = ((this.currentPrefabidx - 1 < 0) ? (this.prefabs.Count - 1) : (this.currentPrefabidx - 1));
		}
		if (global::UnityEngine.Input.GetKeyDown(global::UnityEngine.KeyCode.PageUp))
		{
			this.currentPrefabidx = ((this.currentPrefabidx + 1 >= this.prefabs.Count) ? 0 : (this.currentPrefabidx + 1));
		}
		if (this.autoGenerate != null)
		{
			if (this.generatedObj.Count < this.limit)
			{
				int num = (int)global::UnityEngine.Mathf.Ceil(global::UnityEngine.Mathf.Sqrt((float)this.limit));
				int num2 = this.generatedObj.Count % num;
				int num3 = this.generatedObj.Count / num;
				global::UnityEngine.GameObject gameObject2 = global::UnityEngine.Object.Instantiate(this.autoGenerate, new global::UnityEngine.Vector3((float)((num2 - num / 2) * 3), 0f, (float)((num3 - num / 2) * 3)), global::UnityEngine.Quaternion.identity) as global::UnityEngine.GameObject;
				gameObject2.transform.parent = this.anchor.transform;
				gameObject2.isStatic = this.markStatic;
				this.generatedObj.Add(gameObject2);
			}
			else
			{
				this.autoGenerate = null;
			}
		}
	}

	private void OnGUI()
	{
		if (this.prefabs.Count > 0)
		{
			global::UnityEngine.GUI.Label(this.countLabelRect, string.Concat(new object[]
			{
				"Prefab Count  : ",
				this.generatedObj.Count,
				" / ",
				this.limit
			}));
			global::UnityEngine.GUI.Label(this.modelLabelRect, "Current Model : " + this.prefabs[this.currentPrefabidx].name);
		}
		if (global::UnityEngine.GUI.Button(new global::UnityEngine.Rect(0f, 50f, 200f, 50f), "Combine"))
		{
			global::UnityEngine.StaticBatchingUtility.Combine(this.anchor);
		}
		if (global::UnityEngine.GUI.Button(new global::UnityEngine.Rect(0f, 100f, 200f, 50f), "AutoGenerate"))
		{
			this.Clear();
			this.autoGenerate = this.prefabs[this.currentPrefabidx];
		}
		if (global::UnityEngine.GUI.Button(new global::UnityEngine.Rect(0f, 150f, 200f, 50f), "Clear"))
		{
			this.Clear();
		}
	}

	private void Clear()
	{
		foreach (global::UnityEngine.GameObject obj in this.generatedObj)
		{
			global::UnityEngine.Object.Destroy(obj);
		}
		this.generatedObj.Clear();
	}

	public global::System.Collections.Generic.List<global::UnityEngine.GameObject> prefabs;

	public global::UnityEngine.GameObject anchor;

	public float lag = 0.05f;

	public int limit = 1000;

	public bool markStatic;

	private global::System.Collections.Generic.List<global::UnityEngine.GameObject> generatedObj;

	private float timer;

	private global::UnityEngine.Rect countLabelRect;

	private global::UnityEngine.Rect modelLabelRect;

	private int currentPrefabidx;

	private global::UnityEngine.RaycastHit hitInfo;

	private global::UnityEngine.LayerMask layerMask;

	private global::UnityEngine.GameObject autoGenerate;

	private int autoCount;
}
