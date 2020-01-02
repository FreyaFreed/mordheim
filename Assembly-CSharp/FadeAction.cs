using System;
using UnityEngine;

public class FadeAction : global::UnityEngine.MonoBehaviour
{
	public void Fade(global::FadeAction.OnFadeCallback callback, global::FadeAction.OnFadeCallback finishCallback = null)
	{
		this.onFade = callback;
		this.onFadeFinish = finishCallback;
		global::UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		this._isFadeIn = true;
		this._time = this.fadeDuration;
		this._rect = new global::UnityEngine.Rect((float)(-(float)this.bleeding), (float)(-(float)this.bleeding), (float)(global::UnityEngine.Screen.width + this.bleeding), (float)(global::UnityEngine.Screen.height + this.bleeding));
		this._texture = new global::UnityEngine.Texture2D(1, 1);
		this._texture.SetPixel(0, 0, this.Transparent);
		this._texture.Apply();
		this._started = true;
	}

	private void OnGUI()
	{
		if (this._started)
		{
			global::UnityEngine.GUI.depth = 0;
			global::UnityEngine.GUI.DrawTexture(this._rect, this._texture, global::UnityEngine.ScaleMode.StretchToFill);
		}
	}

	private void Update()
	{
		if (this._started)
		{
			this._time -= global::UnityEngine.Time.smoothDeltaTime;
			if (this._time < 0f)
			{
				if (this._isFadeIn)
				{
					this._time = this.fadeDuration;
					this._isFadeIn = false;
					if (this.onFade != null)
					{
						this.onFade();
						this.onFade = null;
					}
				}
				else
				{
					this._started = false;
					if (this.destroy)
					{
						if (this.onFadeFinish != null)
						{
							this.onFadeFinish();
							this.onFadeFinish = null;
						}
						global::UnityEngine.Object.Destroy(base.gameObject);
					}
				}
			}
			else
			{
				if (this._isFadeIn)
				{
					this._currentColor = global::UnityEngine.Color.Lerp(this.Black, this.Transparent, this._time / this.fadeDuration);
				}
				else
				{
					this._currentColor = global::UnityEngine.Color.Lerp(this.Transparent, this.Black, this._time / this.fadeDuration);
				}
				this._texture.SetPixel(0, 0, this._currentColor);
				this._texture.Apply();
			}
		}
	}

	private global::UnityEngine.Color Transparent = new global::UnityEngine.Color(0f, 0f, 0f, 0f);

	private global::UnityEngine.Color Black = new global::UnityEngine.Color(0f, 0f, 0f, 1f);

	public int bleeding = 10;

	public float fadeDuration = 2f;

	public bool destroy = true;

	private bool _started;

	private global::UnityEngine.Texture2D _texture;

	private global::UnityEngine.Color _currentColor;

	private global::UnityEngine.Rect _rect;

	private float _time;

	private bool _isFadeIn;

	private global::FadeAction.OnFadeCallback onFade;

	private global::FadeAction.OnFadeCallback onFadeFinish;

	public delegate void OnFadeCallback();
}
