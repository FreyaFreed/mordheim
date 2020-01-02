using System;
using UnityEngine;

public class SM_moveThis : global::UnityEngine.MonoBehaviour
{
	private void Update()
	{
		if (this.local)
		{
			base.transform.Translate(new global::UnityEngine.Vector3(this.translationSpeedX, this.translationSpeedY, this.translationSpeedZ) * global::UnityEngine.Time.deltaTime);
		}
		else
		{
			base.transform.Translate(new global::UnityEngine.Vector3(this.translationSpeedX, this.translationSpeedY, this.translationSpeedZ) * global::UnityEngine.Time.deltaTime, global::UnityEngine.Space.World);
		}
	}

	public float translationSpeedX;

	public float translationSpeedY = 1f;

	public float translationSpeedZ;

	public bool local = true;
}
