using System;
using UnityEngine;

public class UniStormThunder_C : global::UnityEngine.MonoBehaviour
{
	private void Start()
	{
		this.lightSource = base.GetComponent<global::UnityEngine.Light>();
	}

	private void Awake()
	{
		this.random = global::UnityEngine.Random.Range(0f, 65535f);
	}

	private void Update()
	{
		if (global::UnityEngine.Time.time - this.lastTime > this.flashLength)
		{
			if (global::UnityEngine.Random.value > this.lightningStrikeOdds)
			{
				global::UnityEngine.Object.Instantiate<global::UnityEngine.GameObject>(this.lightningBolt1);
				global::UnityEngine.Object.Instantiate<global::UnityEngine.GameObject>(this.lightningBolt2);
				float t = global::UnityEngine.Mathf.PerlinNoise(this.random, global::UnityEngine.Time.time);
				float yAngle = global::UnityEngine.Mathf.Lerp(this.minRotationAmount, this.maxRotationAmount, t);
				base.transform.Rotate(0f, yAngle, 0f);
				float t2 = global::UnityEngine.Mathf.PerlinNoise(this.random, global::UnityEngine.Time.time);
				float num = global::UnityEngine.Mathf.Lerp(this.minRotationAmountX, this.maxRotationAmountX, t2);
				base.transform.Rotate(0f, num, 0f);
				float t3 = global::UnityEngine.Mathf.PerlinNoise(this.random, global::UnityEngine.Time.time);
				float num2 = global::UnityEngine.Mathf.Lerp(this.minRotationAmountZ, this.maxRotationAmountZ, t3);
				base.transform.Rotate(0f, num2, 0f);
				base.transform.eulerAngles = new global::UnityEngine.Vector3(0f, num, 0f);
				base.transform.eulerAngles = new global::UnityEngine.Vector3(0f, 0f, num2);
				float t4 = global::UnityEngine.Mathf.PerlinNoise(this.random, global::UnityEngine.Time.time);
				base.GetComponent<global::UnityEngine.Light>().intensity = global::UnityEngine.Mathf.Lerp(this.minIntensity, this.maxIntensity, t4);
				base.GetComponent<global::UnityEngine.Light>().enabled = true;
				base.GetComponent<global::UnityEngine.AudioSource>().PlayOneShot(this.lightningSound[global::UnityEngine.Random.Range(0, this.lightningSound.Length)]);
			}
			else
			{
				base.GetComponent<global::UnityEngine.Light>().enabled = false;
			}
			this.lastTime = global::UnityEngine.Time.time;
		}
	}

	public float flashLength;

	public float lightningStrikeOdds;

	public float minIntensity;

	public float maxIntensity;

	public float minRotationAmount;

	public float maxRotationAmount;

	public float minRotationAmountX;

	public float maxRotationAmountX;

	public float minRotationAmountZ;

	public float maxRotationAmountZ;

	public global::UnityEngine.GameObject lightningBolt1;

	public global::UnityEngine.GameObject lightningBolt2;

	public global::UnityEngine.AudioClip[] lightningSound;

	public float lastTime;

	public global::UnityEngine.Light lightSource;

	private float random;
}
