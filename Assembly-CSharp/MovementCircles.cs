using System;
using UnityEngine;

public class MovementCircles : global::UnityEngine.MonoBehaviour
{
	private void Awake()
	{
		this.root = new global::UnityEngine.GameObject("movement_lines");
		this.root.transform.SetParent(null);
		global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadAssetAsync<global::UnityEngine.GameObject>("Assets/prefabs/fx/", global::AssetBundleId.FX, "fx_move_line.prefab", delegate(global::UnityEngine.Object linePrefab)
		{
			this.line = ((global::UnityEngine.GameObject)global::UnityEngine.Object.Instantiate(linePrefab)).GetComponent<global::UnityEngine.LineRenderer>();
			this.line.transform.SetParent(this.root.transform);
			this.line.transform.localPosition = global::UnityEngine.Vector3.zero;
			this.line.transform.localRotation = global::UnityEngine.Quaternion.identity;
			this.line.SetVertexCount(this.segments);
			this.line.useWorldSpace = false;
			this.line.enabled = false;
		});
	}

	public void Show(global::UnityEngine.Vector3 pos, float radius)
	{
		this.root.transform.position = pos;
		this.radius = radius;
		this.line.enabled = true;
		this.SetupCircle();
	}

	private void SetupCircle()
	{
		global::UnityEngine.Vector3 position = new global::UnityEngine.Vector3(0f, this.defaultHeight, 0f);
		float num = 360f / (float)(this.segments - 1);
		float num2 = 0f;
		for (int i = 0; i < this.segments; i++)
		{
			position.x = global::UnityEngine.Mathf.Sin(0.0174532924f * num2) * this.radius;
			position.z = global::UnityEngine.Mathf.Cos(0.0174532924f * num2) * this.radius;
			this.line.SetPosition(i, position);
			num2 += num;
		}
	}

	public void AdjustHeightAndRadius(float y, float radius)
	{
		global::UnityEngine.Vector3 position = this.root.transform.position;
		position.y = y;
		this.root.transform.position = position;
		if (this.radius != radius)
		{
			this.radius = radius;
			this.SetupCircle();
		}
	}

	public void Hide()
	{
		this.line.enabled = false;
	}

	public int segments = 50;

	public float defaultHeight = 0.1f;

	private global::UnityEngine.GameObject root;

	private global::UnityEngine.LineRenderer line;

	private float radius;
}
