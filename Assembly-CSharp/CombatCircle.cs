using System;
using UnityEngine;

public class CombatCircle : global::UnityEngine.MonoBehaviour
{
	public void Init(bool isEnemy, bool isEngaged, bool currentUnitIsPlayed, bool isLarge, float currentUnitRadius, float height)
	{
		global::UnityEngine.GameObject gameObject = (!isEnemy) ? ((!isEngaged) ? this.friendly : this.friendlyEngaged) : ((!isEngaged) ? this.enemy : this.enemyEngaged);
		this.enemy.SetActive(this.enemy == gameObject && currentUnitIsPlayed);
		this.enemyEngaged.SetActive(this.enemyEngaged == gameObject && currentUnitIsPlayed);
		this.friendly.SetActive(this.friendly == gameObject && currentUnitIsPlayed);
		this.friendlyEngaged.SetActive(this.friendlyEngaged == gameObject && currentUnitIsPlayed);
		float @float = global::Constant.GetFloat((!isLarge) ? global::ConstantId.MELEE_RANGE_NORMAL : global::ConstantId.MELEE_RANGE_LARGE);
		gameObject.transform.localScale = global::UnityEngine.Vector3.one * @float;
		this.fxRenderers = gameObject.GetComponentsInChildren<global::UnityEngine.Renderer>();
		float num = (@float - currentUnitRadius) * 2f;
		global::UnityEngine.Vector3 localScale = new global::UnityEngine.Vector3(num, height, num);
		this.meshCollider.transform.localScale = localScale;
		this.meshCollider.isTrigger = true;
		this.meshCollider.transform.localPosition = new global::UnityEngine.Vector3(0f, height, 0f);
	}

	public void SetAlpha(float a)
	{
		if (this.fxRenderers != null)
		{
			for (int i = 0; i < this.fxRenderers.Length; i++)
			{
				if (this.fxRenderers[i].material.HasProperty("_Color"))
				{
					global::UnityEngine.Color color = this.fxRenderers[i].material.GetColor("_Color");
					color.a = a;
					this.fxRenderers[i].material.SetColor("_Color", color);
				}
			}
		}
	}

	public global::UnityEngine.MeshCollider meshCollider;

	public global::UnityEngine.GameObject enemy;

	public global::UnityEngine.GameObject enemyEngaged;

	public global::UnityEngine.GameObject friendly;

	public global::UnityEngine.GameObject friendlyEngaged;

	private global::UnityEngine.Renderer[] fxRenderers;
}
