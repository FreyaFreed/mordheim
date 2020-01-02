using System;
using UnityEngine;

public class FogSetter : global::UnityEngine.MonoBehaviour
{
	private void Start()
	{
		global::UnityEngine.GameObject gameObject = global::UnityEngine.GameObject.Find("fx_fog_04");
		if (gameObject)
		{
			global::UnityEngine.Vector3 position = gameObject.transform.position;
			position.y = this.y;
			gameObject.transform.position = position;
		}
		global::UnityEngine.Object.Destroy(this);
	}

	public float y;
}
