using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Dissolver : global::UnityEngine.MonoBehaviour
{
	public bool Ressolved
	{
		get
		{
			return this.dissolve == 0f;
		}
	}

	public bool Dissolved
	{
		get
		{
			return this.dissolve == 1f;
		}
	}

	public bool Dissolving
	{
		get
		{
			return this.dissolve != 1f && this.dissolve != 0f;
		}
	}

	public void Hide(bool hide, bool force = false, global::UnityEngine.Events.UnityAction onDissolved = null)
	{
		this.onDissolved = onDissolved;
		this.dissolveRenderers.Clear();
		base.GetComponentsInChildren<global::UnityEngine.Renderer>(true, this.dissolveRenderers);
		base.StopCoroutine("Dissolve");
		if (!hide)
		{
			for (int i = 0; i < this.dissolveRenderers.Count; i++)
			{
				global::UnityEngine.Renderer renderer = this.dissolveRenderers[i];
				renderer.enabled = true;
			}
		}
		if (force || this.dissolveSpeed == 0f || this.dissolveRenderers.Count == 0)
		{
			this.dissolve = (float)((!hide) ? 0 : 1);
			for (int j = 0; j < this.dissolveRenderers.Count; j++)
			{
				global::UnityEngine.Renderer renderer2 = this.dissolveRenderers[j];
				if (renderer2 != null)
				{
					global::UnityEngine.Material[] materials = renderer2.materials;
					for (int k = 0; k < materials.Length; k++)
					{
						materials[k].SetFloat("_Dissolve", this.dissolve);
					}
					renderer2.enabled = !hide;
				}
			}
			this.CallBack();
		}
		else if (base.gameObject.activeSelf)
		{
			base.StartCoroutine("Dissolve", hide);
		}
	}

	private global::System.Collections.IEnumerator Dissolve(bool hide)
	{
		float target = (float)((!hide) ? 0 : 1);
		while (this.dissolve != target)
		{
			this.dissolve += (float)((this.dissolve >= target) ? -1 : 1) * this.dissolveSpeed * global::UnityEngine.Time.deltaTime;
			this.dissolve = global::UnityEngine.Mathf.Clamp(this.dissolve, 0f, 1f);
			for (int i = 0; i < this.dissolveRenderers.Count; i++)
			{
				global::UnityEngine.Renderer rend = this.dissolveRenderers[i];
				if (rend != null)
				{
					global::UnityEngine.Material[] materials = rend.materials;
					for (int j = 0; j < materials.Length; j++)
					{
						materials[j].SetFloat("_Dissolve", this.dissolve);
					}
				}
			}
			yield return null;
		}
		if (hide && this.dissolve == target)
		{
			for (int k = 0; k < this.dissolveRenderers.Count; k++)
			{
				if (this.dissolveRenderers[k] != null)
				{
					this.dissolveRenderers[k].enabled = false;
				}
			}
		}
		yield return null;
		this.CallBack();
		yield break;
	}

	private void CallBack()
	{
		if (this.onDissolved != null)
		{
			this.onDissolved();
		}
	}

	public float dissolveSpeed = 2f;

	private readonly global::System.Collections.Generic.List<global::UnityEngine.Renderer> dissolveRenderers = new global::System.Collections.Generic.List<global::UnityEngine.Renderer>();

	private float dissolve;

	private global::UnityEngine.Events.UnityAction onDissolved;
}
