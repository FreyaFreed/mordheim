using System;
using UnityEngine;

public class UvAnimate : global::UnityEngine.MonoBehaviour
{
	private void Start()
	{
		this.m_Renderer = base.GetComponent<global::UnityEngine.Renderer>();
		if (this.m_Renderer == null || this.m_Renderer.sharedMaterial == null || this.m_Renderer.sharedMaterial.mainTexture == null)
		{
			base.enabled = false;
		}
		else
		{
			base.GetComponent<global::UnityEngine.Renderer>().material.mainTextureScale = new global::UnityEngine.Vector2(this.m_fTilingX, this.m_fTilingY);
			float num = this.m_fOffsetX + this.m_fTilingX;
			this.m_RepeatOffset.x = num - (float)((int)num);
			if (this.m_RepeatOffset.x < 0f)
			{
				this.m_RepeatOffset.x = this.m_RepeatOffset.x + 1f;
			}
			num = this.m_fOffsetY + this.m_fTilingY;
			this.m_RepeatOffset.y = num - (float)((int)num);
			if (this.m_RepeatOffset.y < 0f)
			{
				this.m_RepeatOffset.y = this.m_RepeatOffset.y + 1f;
			}
			this.m_EndOffset.x = 1f - (this.m_fTilingX - (float)((int)this.m_fTilingX) + (float)((this.m_fTilingX - (float)((int)this.m_fTilingX) >= 0f) ? 0 : 1));
			this.m_EndOffset.y = 1f - (this.m_fTilingY - (float)((int)this.m_fTilingY) + (float)((this.m_fTilingY - (float)((int)this.m_fTilingY) >= 0f) ? 0 : 1));
		}
	}

	private void OnWillRenderObject()
	{
		if (this.m_Renderer == null || this.m_Renderer.sharedMaterial == null)
		{
			return;
		}
		if (this.m_bFixedTileSize)
		{
			if (this.m_fScrollSpeedX != 0f && this.m_OriginalScale.x != 0f)
			{
				this.m_fTilingX = this.m_OriginalTiling.x * (base.transform.lossyScale.x / this.m_OriginalScale.x);
			}
			if (this.m_fScrollSpeedY != 0f && this.m_OriginalScale.y != 0f)
			{
				this.m_fTilingY = this.m_OriginalTiling.y * (base.transform.lossyScale.y / this.m_OriginalScale.y);
			}
			base.GetComponent<global::UnityEngine.Renderer>().material.mainTextureScale = new global::UnityEngine.Vector2(this.m_fTilingX, this.m_fTilingY);
		}
		this.m_fOffsetX += global::UnityEngine.Time.deltaTime * this.m_fScrollSpeedX;
		this.m_fOffsetY += global::UnityEngine.Time.deltaTime * this.m_fScrollSpeedY;
		bool flag = false;
		if (!this.m_bRepeat)
		{
			this.m_RepeatOffset.x = this.m_RepeatOffset.x + global::UnityEngine.Time.deltaTime * this.m_fScrollSpeedX;
			if (this.m_RepeatOffset.x < 0f || 1f < this.m_RepeatOffset.x)
			{
				this.m_fOffsetX = this.m_EndOffset.x;
				base.enabled = false;
				flag = true;
			}
			this.m_RepeatOffset.y = this.m_RepeatOffset.y + global::UnityEngine.Time.deltaTime * this.m_fScrollSpeedY;
			if (this.m_RepeatOffset.y < 0f || 1f < this.m_RepeatOffset.y)
			{
				this.m_fOffsetY = this.m_EndOffset.y;
				base.enabled = false;
				flag = true;
			}
		}
		global::UnityEngine.Vector2 vector = new global::UnityEngine.Vector2(this.m_fOffsetX, this.m_fOffsetY);
		this.m_Renderer.material.mainTextureOffset = vector;
		if (this.m_Renderer.material.HasProperty("_BumpMap"))
		{
			this.m_Renderer.material.SetTextureOffset("_BumpMap", vector);
		}
		if (this.m_Renderer.material.HasProperty("_SpecTex"))
		{
			this.m_Renderer.material.SetTextureOffset("_SpecTex", vector);
		}
		if (flag && this.m_bAutoDestruct)
		{
			global::UnityEngine.Object.DestroyObject(base.gameObject);
		}
	}

	public float m_fScrollSpeedX = 1f;

	public float m_fScrollSpeedY;

	public float m_fTilingX = 1f;

	public float m_fTilingY = 1f;

	public float m_fOffsetX;

	public float m_fOffsetY;

	public bool m_bFixedTileSize;

	public bool m_bRepeat = true;

	public bool m_bAutoDestruct;

	protected global::UnityEngine.Vector3 m_OriginalScale = default(global::UnityEngine.Vector3);

	protected global::UnityEngine.Vector2 m_OriginalTiling = default(global::UnityEngine.Vector2);

	protected global::UnityEngine.Vector2 m_EndOffset = default(global::UnityEngine.Vector2);

	protected global::UnityEngine.Vector2 m_RepeatOffset = default(global::UnityEngine.Vector2);

	protected global::UnityEngine.Renderer m_Renderer;
}
