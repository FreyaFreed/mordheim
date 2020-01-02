using System;
using UnityEngine;

public class TNAutoCreate : global::UnityEngine.MonoBehaviour
{
	private void Start()
	{
		global::TNManager.Create(this.prefab, base.transform.position, base.transform.rotation, this.persistent);
		global::UnityEngine.Object.Destroy(base.gameObject);
	}

	public global::UnityEngine.GameObject prefab;

	public bool persistent;
}
