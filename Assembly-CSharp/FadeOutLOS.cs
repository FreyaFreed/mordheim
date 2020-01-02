using System;
using System.Collections.Generic;
using UnityEngine;

public class FadeOutLOS : global::UnityEngine.MonoBehaviour
{
	public void AddLayer(int layer)
	{
		this.layerMask.value = (this.layerMask.value | 1 << layer);
	}

	public void RemoveLayer(int layer)
	{
		this.layerMask.value = (this.layerMask.value & ~(1 << layer));
	}

	public void ClearTargets()
	{
		this.targets.Clear();
	}

	public void AddTarget(global::UnityEngine.Transform target)
	{
		if (!this.isInLayer(target))
		{
			return;
		}
		if (target != null && this.targets.IndexOf(target) == -1)
		{
			this.targets.Add(target);
		}
	}

	public void ReplaceTarget(global::UnityEngine.Transform target)
	{
		if (!this.isInLayer(target))
		{
			return;
		}
		if (target != null)
		{
			if (this.targets.Count == 0)
			{
				this.AddTarget(target);
			}
			else
			{
				this.targets[0] = target;
			}
		}
	}

	public bool isInLayer(global::UnityEngine.Transform target)
	{
		return target != null && (1 << target.gameObject.layer & this.layerMask.value) > 0;
	}

	private void LateUpdate()
	{
		if (this.targets.Count == 0 || this.targets[0] == null)
		{
			return;
		}
		for (int i = 0; i < this.fadedOutObjects.Count; i++)
		{
			this.fadedOutObjects[i].needFadeOut = false;
		}
		global::UnityEngine.Vector3 vector = base.transform.position - base.transform.forward;
		for (int j = 0; j < this.targets.Count; j++)
		{
			global::UnityEngine.Vector3 vector2 = this.targets[j].position + global::UnityEngine.Vector3.up * 1.3f;
			float num = global::UnityEngine.Vector3.Distance(vector, vector2);
			global::UnityEngine.RaycastHit[] array = global::UnityEngine.Physics.SphereCastAll(vector, this.radius, vector2 - vector, num - this.radius, this.layerMask.value);
			for (int k = 0; k < array.Length; k++)
			{
				bool flag = true;
				for (int l = 0; l < this.targets.Count; l++)
				{
					if (array[k].collider == this.targets[l].gameObject.GetComponent<global::UnityEngine.Collider>())
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					this.AddLOSInfo(array[k].collider.gameObject);
				}
			}
		}
		for (int m = this.fadedOutObjects.Count - 1; m >= 0; m--)
		{
			global::FadeOutLOS.FadeoutLOSInfo fadeoutLOSInfo = this.fadedOutObjects[m];
			fadeoutLOSInfo.gameOjbect.SendMessage("Fade", fadeoutLOSInfo.needFadeOut, global::UnityEngine.SendMessageOptions.DontRequireReceiver);
			if (!fadeoutLOSInfo.needFadeOut)
			{
				this.fadedOutObjects.RemoveAt(m);
				this.unUsedLOSInfo.Push(fadeoutLOSInfo);
			}
		}
	}

	private void AddLOSInfo(global::UnityEngine.GameObject go)
	{
		global::FadeOutLOS.FadeoutLOSInfo fadeoutLOSInfo = null;
		for (int i = 0; i < this.fadedOutObjects.Count; i++)
		{
			if (this.fadedOutObjects[i].gameOjbect == go)
			{
				fadeoutLOSInfo = this.fadedOutObjects[i];
			}
		}
		if (fadeoutLOSInfo == null && this.unUsedLOSInfo.Count > 0)
		{
			fadeoutLOSInfo = this.unUsedLOSInfo.Pop();
			fadeoutLOSInfo.gameOjbect = go;
			this.fadedOutObjects.Add(fadeoutLOSInfo);
		}
		if (fadeoutLOSInfo == null)
		{
			fadeoutLOSInfo = new global::FadeOutLOS.FadeoutLOSInfo(go);
			this.fadedOutObjects.Add(fadeoutLOSInfo);
		}
		fadeoutLOSInfo.needFadeOut = true;
	}

	public global::UnityEngine.LayerMask layerMask = 0;

	public float radius = 0.2f;

	private global::System.Collections.Generic.List<global::UnityEngine.Transform> targets = new global::System.Collections.Generic.List<global::UnityEngine.Transform>();

	private global::System.Collections.Generic.List<global::FadeOutLOS.FadeoutLOSInfo> fadedOutObjects = new global::System.Collections.Generic.List<global::FadeOutLOS.FadeoutLOSInfo>();

	private global::System.Collections.Generic.Stack<global::FadeOutLOS.FadeoutLOSInfo> unUsedLOSInfo = new global::System.Collections.Generic.Stack<global::FadeOutLOS.FadeoutLOSInfo>();

	private class FadeoutLOSInfo
	{
		public FadeoutLOSInfo(global::UnityEngine.GameObject go)
		{
			this.gameOjbect = go;
			this.needFadeOut = false;
		}

		public global::UnityEngine.GameObject gameOjbect;

		public bool needFadeOut;
	}
}
