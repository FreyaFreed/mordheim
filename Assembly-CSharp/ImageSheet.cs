using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[global::UnityEngine.ExecuteInEditMode]
public class ImageSheet : global::UnityEngine.MonoBehaviour
{
	private void Start()
	{
		this.img = base.GetComponent<global::UnityEngine.UI.Image>();
		this.timer = 0f;
		this.idx = 0;
		this.img.overrideSprite = this.sprites[this.idx];
	}

	private void Update()
	{
		this.timer += global::UnityEngine.Time.deltaTime;
		if (this.timer >= this.speed)
		{
			this.idx = (this.idx + 1) % this.sprites.Count;
			this.img.overrideSprite = this.sprites[this.idx];
			this.timer = 0f;
		}
	}

	private global::UnityEngine.UI.Image img;

	public global::System.Collections.Generic.List<global::UnityEngine.Sprite> sprites;

	public float speed;

	private float timer;

	private int idx;
}
