using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionActivator : global::UnityEngine.MonoBehaviour
{
	private void Awake()
	{
		global::UnityEngine.Renderer[] componentsInChildren = base.GetComponentsInChildren<global::UnityEngine.Renderer>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			this.mats.Add(componentsInChildren[i].material);
		}
		this.parts = base.GetComponentsInChildren<global::UnityEngine.ParticleSystem>();
		for (int j = 0; j < this.mats.Count; j++)
		{
			global::UnityEngine.Color color;
			if (this.mats[j].HasProperty("_TintColor"))
			{
				color = this.mats[j].GetColor("_TintColor");
			}
			else
			{
				color = this.mats[j].color;
			}
			color.a = this.a;
			this.mats[j].color = color;
		}
		for (int k = 0; k < this.parts.Length; k++)
		{
			global::UnityEngine.Color startColor = this.parts[k].startColor;
			startColor.a = this.a;
			this.parts[k].startColor = startColor;
		}
		for (int l = 0; l < this.gos.Length; l++)
		{
			this.gos[l].SetActive(false);
		}
	}

	private void OnTriggerEnter()
	{
		base.StopCoroutine("FadeOutFx");
		base.StartCoroutine("FadeInFx");
	}

	private void OnTriggerExit()
	{
		base.StopCoroutine("FadeInFx");
		base.StartCoroutine("FadeOutFx");
	}

	private global::System.Collections.IEnumerator FadeInFx()
	{
		for (int i = 0; i < this.gos.Length; i++)
		{
			this.gos[i].SetActive(true);
		}
		while (this.a < 1.1f)
		{
			for (int j = 0; j < this.mats.Count; j++)
			{
				global::UnityEngine.Color c;
				if (this.mats[j].HasProperty("_TintColor"))
				{
					c = this.mats[j].GetColor("_TintColor");
				}
				else
				{
					c = this.mats[j].color;
				}
				c.a = this.a;
				this.mats[j].color = c;
			}
			for (int k = 0; k < this.parts.Length; k++)
			{
				global::UnityEngine.Color c2 = this.parts[k].startColor;
				c2.a = this.a;
				this.parts[k].startColor = c2;
			}
			this.a += 0.01f;
			yield return new global::UnityEngine.WaitForFixedUpdate();
		}
		yield break;
	}

	private global::System.Collections.IEnumerator FadeOutFx()
	{
		while (this.a > -0.01f)
		{
			if (this.a < 0f)
			{
				this.a = 0f;
			}
			for (int i = 0; i < this.mats.Count; i++)
			{
				global::UnityEngine.Color c;
				if (this.mats[i].HasProperty("_TintColor"))
				{
					c = this.mats[i].GetColor("_TintColor");
				}
				else
				{
					c = this.mats[i].color;
				}
				c.a = this.a;
				this.mats[i].color = c;
			}
			for (int j = 0; j < this.parts.Length; j++)
			{
				global::UnityEngine.Color c2 = this.parts[j].startColor;
				c2.a = this.a;
				this.parts[j].startColor = c2;
			}
			this.a -= 0.01f;
			yield return new global::UnityEngine.WaitForFixedUpdate();
		}
		yield return new global::UnityEngine.WaitForSeconds(2f);
		for (int k = 0; k < this.gos.Length; k++)
		{
			this.gos[k].SetActive(false);
		}
		yield break;
	}

	public global::UnityEngine.GameObject[] gos;

	private global::System.Collections.Generic.List<global::UnityEngine.Material> mats = new global::System.Collections.Generic.List<global::UnityEngine.Material>();

	private global::UnityEngine.ParticleSystem[] parts;

	private float a;
}
