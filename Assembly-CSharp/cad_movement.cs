using System;
using System.Collections;
using UnityEngine;

public class cad_movement : global::UnityEngine.MonoBehaviour
{
	private void Start()
	{
		this.FinishWait = true;
	}

	private global::System.Collections.IEnumerator RandomMov()
	{
		this.RandomWait = global::UnityEngine.Random.Range(1, 10);
		int x = global::UnityEngine.Random.Range(10, 50);
		int y = global::UnityEngine.Random.Range(10, 50);
		int z = global::UnityEngine.Random.Range(10, 50);
		global::UnityEngine.Vector3 force = new global::UnityEngine.Vector3((float)(x * this.multiplier), (float)(y * this.multiplier), (float)(z * this.multiplier));
		yield return new global::UnityEngine.WaitForSeconds((float)this.RandomWait);
		if (this.LastBone.transform.GetComponent<global::UnityEngine.Rigidbody>() != null)
		{
			this.LastBone.transform.GetComponent<global::UnityEngine.Rigidbody>().AddForce(force);
		}
		this.FinishWait = true;
		yield break;
	}

	private void Update()
	{
		if (this.FinishWait)
		{
			base.StartCoroutine(this.RandomMov());
			this.FinishWait = false;
		}
	}

	public global::UnityEngine.GameObject LastBone;

	public int multiplier = 1;

	public int RandomWait;

	private bool FinishWait;
}
