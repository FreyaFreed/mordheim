using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIModule : global::UIPopupModule
{
	protected override void Awake()
	{
		base.Awake();
		this.showQueue = new global::System.Collections.Generic.Queue<global::UnityEngine.GameObject>();
	}

	public void StartShow(float wait)
	{
		base.StartCoroutine("ShowNext", wait);
	}

	public bool EndShow()
	{
		if (this.showQueue.Count > 0)
		{
			base.StopCoroutine("ShowNext");
			while (this.showQueue.Count > 0)
			{
				this.showQueue.Dequeue().SetActive(true);
			}
			return false;
		}
		return true;
	}

	public global::System.Collections.IEnumerator ShowNext(float wait)
	{
		while (this.showQueue.Count > 0)
		{
			yield return new global::UnityEngine.WaitForSeconds(wait);
			this.showQueue.Dequeue().SetActive(true);
		}
		yield break;
	}

	public global::ModuleId moduleId;

	protected global::System.Collections.Generic.Queue<global::UnityEngine.GameObject> showQueue;
}
