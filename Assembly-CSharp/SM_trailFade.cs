using System;
using UnityEngine;

public class SM_trailFade : global::UnityEngine.MonoBehaviour
{
	private void Start()
	{
		if (this.fadeInTime < 0.01f)
		{
			this.fadeInTime = 0.01f;
		}
		this.percent = this.timeElapsed / this.fadeInTime;
	}

	private void Update()
	{
		this.timeElapsed += global::UnityEngine.Time.deltaTime;
		global::UnityEngine.Color color = this.thisTrail.material.GetColor("_TintColor");
		if (this.timeElapsed <= this.fadeInTime)
		{
			this.percent = this.timeElapsed / this.fadeInTime;
			color.a = this.percent;
			this.thisTrail.material.SetColor("_TintColor", color);
		}
		if (this.timeElapsed > this.fadeInTime && this.timeElapsed < this.fadeInTime + this.stayTime)
		{
			color.a = 1f;
			this.thisTrail.material.SetColor("_TintColor", color);
		}
		if (this.timeElapsed >= this.fadeInTime + this.stayTime && this.timeElapsed < this.fadeInTime + this.stayTime + this.fadeOutTime)
		{
			this.timeElapsedLast += global::UnityEngine.Time.deltaTime;
			this.percent = 1f - this.timeElapsedLast / this.fadeOutTime;
			color.a = this.percent;
			this.thisTrail.material.SetColor("_TintColor", color);
		}
	}

	public float fadeInTime = 0.1f;

	public float stayTime = 1f;

	public float fadeOutTime = 0.7f;

	public global::UnityEngine.TrailRenderer thisTrail;

	private float timeElapsed;

	private float timeElapsedLast;

	private float percent;
}
