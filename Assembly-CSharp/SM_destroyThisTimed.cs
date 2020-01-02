using System;
using UnityEngine;

public class SM_destroyThisTimed : global::UnityEngine.MonoBehaviour
{
	private void Start()
	{
		global::UnityEngine.Object.Destroy(base.gameObject, this.destroyTime);
	}

	public float destroyTime = 5f;
}
