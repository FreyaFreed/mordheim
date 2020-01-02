using System;
using UnityEngine;

[global::UnityEngine.RequireComponent(typeof(global::TNObject))]
public abstract class TNBehaviour : global::UnityEngine.MonoBehaviour
{
	public global::TNObject tno
	{
		get
		{
			if (this.mTNO == null)
			{
				this.mTNO = base.GetComponent<global::TNObject>();
			}
			return this.mTNO;
		}
	}

	protected virtual void OnEnable()
	{
		if (global::UnityEngine.Application.isPlaying)
		{
			this.tno.rebuildMethodList = true;
		}
	}

	public void DestroySelf()
	{
		global::TNManager.Destroy(base.gameObject);
	}

	private global::TNObject mTNO;
}
