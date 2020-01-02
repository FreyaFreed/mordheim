using System;
using UnityEngine;

public class TransitionFade : global::TransitionBase
{
	private void Awake()
	{
		this.m_fadeTexture = new global::UnityEngine.Texture2D(1, 1);
		this.m_backgroundStyle.normal.background = this.m_fadeTexture;
		this.SetScreenOverlayColor(this.TRANSPARENT);
		base.enabled = false;
	}

	private void OnGUI()
	{
		global::UnityEngine.GUI.depth = 1000;
		global::UnityEngine.GUI.Label(new global::UnityEngine.Rect(-10f, -10f, (float)(global::UnityEngine.Screen.width + 10), (float)(global::UnityEngine.Screen.height + 10)), this.m_fadeTexture, this.m_backgroundStyle);
	}

	private void SetScreenOverlayColor(global::UnityEngine.Color newScreenOverlayColor)
	{
		this.m_currentOverlayColor = newScreenOverlayColor;
		this.m_fadeTexture.SetPixel(0, 0, this.m_currentOverlayColor);
		this.m_fadeTexture.Apply();
	}

	public override void Show(bool visible, float duration)
	{
		this.isVisible = visible;
		if (visible)
		{
			base.enabled = true;
		}
		this.SetScreenOverlayColor((!visible) ? this.BLACK : this.TRANSPARENT);
		this.m_targetOverlayColor = ((!visible) ? this.TRANSPARENT : this.BLACK);
		this.m_deltaColor = (this.m_targetOverlayColor - this.m_currentOverlayColor) / duration;
	}

	public override void ProcessTransition(float progress)
	{
		this.SetScreenOverlayColor(this.m_currentOverlayColor + this.m_deltaColor * global::UnityEngine.Time.deltaTime);
	}

	public override void EndTransition()
	{
		this.m_currentOverlayColor = this.m_targetOverlayColor;
		this.SetScreenOverlayColor(this.m_currentOverlayColor);
		this.m_deltaColor = new global::UnityEngine.Color(0f, 0f, 0f, 0f);
		if (!this.isVisible)
		{
			base.enabled = this.isVisible;
		}
	}

	public override void Reset()
	{
		this.SetScreenOverlayColor(this.TRANSPARENT);
	}

	private const int SCREEN_BLEEDING = 10;

	private const int FADE_GUI_DEPTH = 1000;

	private global::UnityEngine.Color TRANSPARENT = new global::UnityEngine.Color(0f, 0f, 0f, 0f);

	private global::UnityEngine.Color BLACK = new global::UnityEngine.Color(0f, 0f, 0f, 1f);

	private global::UnityEngine.GUIStyle m_backgroundStyle = new global::UnityEngine.GUIStyle();

	private global::UnityEngine.Texture2D m_fadeTexture;

	private global::UnityEngine.Color m_currentOverlayColor = new global::UnityEngine.Color(0f, 0f, 0f, 0f);

	private global::UnityEngine.Color m_targetOverlayColor = new global::UnityEngine.Color(0f, 0f, 0f, 0f);

	private global::UnityEngine.Color m_deltaColor = new global::UnityEngine.Color(0f, 0f, 0f, 0f);

	private bool isVisible;
}
