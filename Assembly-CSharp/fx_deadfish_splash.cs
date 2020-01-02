using System;
using UnityEngine;

[global::UnityEngine.ExecuteInEditMode]
public class fx_deadfish_splash : global::UnityEngine.MonoBehaviour
{
	private void OnParticleCollision(global::UnityEngine.GameObject other)
	{
		if (this.fishSplash == null)
		{
			return;
		}
		global::UnityEngine.GameObject gameObject = global::UnityEngine.Object.Instantiate<global::UnityEngine.GameObject>(this.fishSplash);
		gameObject.transform.SetParent(base.transform);
		gameObject.transform.localPosition = global::UnityEngine.Vector3.zero;
	}

	public global::UnityEngine.GameObject fishSplash;
}
