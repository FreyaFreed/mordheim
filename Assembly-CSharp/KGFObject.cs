using System;
using UnityEngine;

public class KGFObject : global::UnityEngine.MonoBehaviour
{
	protected virtual void Awake()
	{
		global::KGFAccessor.AddKGFObject(this);
		this.EventOnAwake.Trigger(this);
		this.KGFAwake();
	}

	private void OnDestroy()
	{
		this.EventOnDestroy.Trigger(this);
		global::KGFAccessor.RemoveKGFObject(this);
		this.KGFDestroy();
	}

	protected virtual void KGFAwake()
	{
	}

	protected virtual void KGFDestroy()
	{
	}

	public global::KGFDelegate EventOnAwake = new global::KGFDelegate();

	public global::KGFDelegate EventOnDestroy = new global::KGFDelegate();
}
