using System;
using UnityEngine;

public class SM_UVScroller : global::UnityEngine.MonoBehaviour
{
	private void Awake()
	{
		this.r = base.GetComponent<global::UnityEngine.Renderer>();
	}

	private void Update()
	{
		this.timeWentY += global::UnityEngine.Time.deltaTime * this.speedY;
		this.timeWentX += global::UnityEngine.Time.deltaTime * this.speedX;
		this.r.materials[this.targetMaterialSlot].SetTextureOffset("_MainTex", new global::UnityEngine.Vector2(this.timeWentX, this.timeWentY));
	}

	public int targetMaterialSlot;

	public float speedY = 0.5f;

	public float speedX;

	private float timeWentX;

	private float timeWentY;

	private global::UnityEngine.Renderer r;
}
