using System;
using UnityEngine;

public class SM_materialColorRandomizer : global::UnityEngine.MonoBehaviour
{
	private void Start()
	{
		if (!this.unifiedColor)
		{
			this.ColR = global::UnityEngine.Random.Range(this.color1.r, this.color2.r);
			this.ColG = global::UnityEngine.Random.Range(this.color1.g, this.color2.g);
			this.ColB = global::UnityEngine.Random.Range(this.color1.b, this.color2.b);
			this.ColA = global::UnityEngine.Random.Range(this.color1.a, this.color2.a);
		}
		else
		{
			float value = global::UnityEngine.Random.value;
			this.ColR = global::UnityEngine.Mathf.Min(this.color1.r, this.color2.r) + global::UnityEngine.Mathf.Abs(this.color1.r - this.color2.r) * value;
			this.ColG = global::UnityEngine.Mathf.Min(this.color1.g, this.color2.g) + global::UnityEngine.Mathf.Abs(this.color1.g - this.color2.g) * value;
			this.ColB = global::UnityEngine.Mathf.Min(this.color1.b, this.color2.b) + global::UnityEngine.Mathf.Abs(this.color1.b - this.color2.b) * value;
		}
		base.GetComponent<global::UnityEngine.Renderer>().material.color = new global::UnityEngine.Color(this.ColR, this.ColG, this.ColB, this.ColA);
	}

	public global::UnityEngine.Color color1 = new global::UnityEngine.Color(0.6f, 0.6f, 0.6f, 1f);

	public global::UnityEngine.Color color2 = new global::UnityEngine.Color(0.4f, 0.4f, 0.4f, 1f);

	public bool unifiedColor = true;

	private float ColR;

	private float ColG;

	private float ColB;

	private float ColA;
}
