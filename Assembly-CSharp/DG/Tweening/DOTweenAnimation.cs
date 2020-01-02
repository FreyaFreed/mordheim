using System;
using DG.Tweening.Core;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DG.Tweening
{
	[global::UnityEngine.AddComponentMenu("DOTween/DOTween Animation")]
	public class DOTweenAnimation : global::DG.Tweening.Core.ABSDOTweenAnimationComponent
	{
		private void Awake()
		{
			if (!this.isValid)
			{
				return;
			}
			switch (this.animationType)
			{
			case global::DG.Tweening.Core.DOTweenAnimationType.Move:
			{
				global::UnityEngine.Component component = base.GetComponent<global::UnityEngine.Rigidbody2D>();
				if (component != null)
				{
					this._tween = ((global::UnityEngine.Rigidbody2D)component).DOMove(this.endValueV3, this.duration, this.optionalBool0);
				}
				else
				{
					component = base.GetComponent<global::UnityEngine.Rigidbody>();
					if (component != null)
					{
						this._tween = ((global::UnityEngine.Rigidbody)component).DOMove(this.endValueV3, this.duration, this.optionalBool0);
					}
					else
					{
						this._tween = base.transform.DOMove(this.endValueV3, this.duration, this.optionalBool0);
					}
				}
				break;
			}
			case global::DG.Tweening.Core.DOTweenAnimationType.LocalMove:
				this._tween = base.transform.DOLocalMove(this.endValueV3, this.duration, this.optionalBool0);
				break;
			case global::DG.Tweening.Core.DOTweenAnimationType.Rotate:
			{
				global::UnityEngine.Component component = base.GetComponent<global::UnityEngine.Rigidbody2D>();
				if (component != null)
				{
					this._tween = ((global::UnityEngine.Rigidbody2D)component).DORotate(this.endValueFloat, this.duration);
				}
				else
				{
					component = base.GetComponent<global::UnityEngine.Rigidbody>();
					if (component != null)
					{
						this._tween = ((global::UnityEngine.Rigidbody)component).DORotate(this.endValueV3, this.duration, this.optionalRotationMode);
					}
					else
					{
						this._tween = base.transform.DORotate(this.endValueV3, this.duration, this.optionalRotationMode);
					}
				}
				break;
			}
			case global::DG.Tweening.Core.DOTweenAnimationType.LocalRotate:
				this._tween = base.transform.DOLocalRotate(this.endValueV3, this.duration, this.optionalRotationMode);
				break;
			case global::DG.Tweening.Core.DOTweenAnimationType.Scale:
				this._tween = base.transform.DOScale((!this.optionalBool0) ? this.endValueV3 : new global::UnityEngine.Vector3(this.endValueFloat, this.endValueFloat, this.endValueFloat), this.duration);
				break;
			case global::DG.Tweening.Core.DOTweenAnimationType.Color:
			{
				this.isRelative = false;
				global::UnityEngine.Component component = base.GetComponent<global::UnityEngine.SpriteRenderer>();
				if (component != null)
				{
					this._tween = ((global::UnityEngine.SpriteRenderer)component).DOColor(this.endValueColor, this.duration);
				}
				else
				{
					component = base.GetComponent<global::UnityEngine.Renderer>();
					if (component != null)
					{
						this._tween = ((global::UnityEngine.Renderer)component).material.DOColor(this.endValueColor, this.duration);
					}
					else
					{
						component = base.GetComponent<global::UnityEngine.UI.Image>();
						if (component != null)
						{
							this._tween = ((global::UnityEngine.UI.Image)component).DOColor(this.endValueColor, this.duration);
						}
						else
						{
							component = base.GetComponent<global::UnityEngine.UI.Text>();
							if (component != null)
							{
								this._tween = ((global::UnityEngine.UI.Text)component).DOColor(this.endValueColor, this.duration);
							}
						}
					}
				}
				break;
			}
			case global::DG.Tweening.Core.DOTweenAnimationType.Fade:
			{
				this.isRelative = false;
				global::UnityEngine.Component component = base.GetComponent<global::UnityEngine.SpriteRenderer>();
				if (component != null)
				{
					this._tween = ((global::UnityEngine.SpriteRenderer)component).DOFade(this.endValueFloat, this.duration);
				}
				else
				{
					component = base.GetComponent<global::UnityEngine.Renderer>();
					if (component != null)
					{
						this._tween = ((global::UnityEngine.Renderer)component).material.DOFade(this.endValueFloat, this.duration);
					}
					else
					{
						component = base.GetComponent<global::UnityEngine.UI.Image>();
						if (component != null)
						{
							this._tween = ((global::UnityEngine.UI.Image)component).DOFade(this.endValueFloat, this.duration);
						}
						else
						{
							component = base.GetComponent<global::UnityEngine.UI.Text>();
							if (component != null)
							{
								this._tween = ((global::UnityEngine.UI.Text)component).DOFade(this.endValueFloat, this.duration);
							}
						}
					}
				}
				break;
			}
			case global::DG.Tweening.Core.DOTweenAnimationType.Text:
			{
				global::UnityEngine.Component component = base.GetComponent<global::UnityEngine.UI.Text>();
				if (component != null)
				{
					this._tween = ((global::UnityEngine.UI.Text)component).DOText(this.endValueString, this.duration, this.optionalBool0, this.optionalScrambleMode, this.optionalString);
				}
				break;
			}
			case global::DG.Tweening.Core.DOTweenAnimationType.PunchPosition:
				this._tween = base.transform.DOPunchPosition(this.endValueV3, this.duration, this.optionalInt0, this.optionalFloat0, this.optionalBool0);
				break;
			case global::DG.Tweening.Core.DOTweenAnimationType.PunchRotation:
				this._tween = base.transform.DOPunchRotation(this.endValueV3, this.duration, this.optionalInt0, this.optionalFloat0);
				break;
			case global::DG.Tweening.Core.DOTweenAnimationType.PunchScale:
				this._tween = base.transform.DOPunchScale(this.endValueV3, this.duration, this.optionalInt0, this.optionalFloat0);
				break;
			case global::DG.Tweening.Core.DOTweenAnimationType.ShakePosition:
				this._tween = base.transform.DOShakePosition(this.duration, this.endValueV3, this.optionalInt0, this.optionalFloat0, this.optionalBool0);
				break;
			case global::DG.Tweening.Core.DOTweenAnimationType.ShakeRotation:
				this._tween = base.transform.DOShakeRotation(this.duration, this.endValueV3, this.optionalInt0, this.optionalFloat0);
				break;
			case global::DG.Tweening.Core.DOTweenAnimationType.ShakeScale:
				this._tween = base.transform.DOShakeScale(this.duration, this.endValueV3, this.optionalInt0, this.optionalFloat0);
				break;
			}
			if (this._tween == null)
			{
				return;
			}
			if (this.isFrom)
			{
				((global::DG.Tweening.Tweener)this._tween).From(this.isRelative);
			}
			else
			{
				this._tween.SetRelative(this.isRelative);
			}
			this._tween.SetTarget(base.gameObject).SetDelay(this.delay).SetLoops(this.loops, this.loopType).SetAutoKill(this.autoKill).OnKill(delegate
			{
				this._tween = null;
			});
			if (this.easeType == global::DG.Tweening.Ease.INTERNAL_Custom)
			{
				this._tween.SetEase(this.easeCurve);
			}
			else
			{
				this._tween.SetEase(this.easeType);
			}
			if (!string.IsNullOrEmpty(this.id))
			{
				this._tween.SetId(this.id);
			}
			if (this.hasOnStart)
			{
				if (this.onStart != null)
				{
					this._tween.OnStart(new global::DG.Tweening.TweenCallback(this.onStart.Invoke));
				}
			}
			else
			{
				this.onStart = null;
			}
			if (this.hasOnStepComplete)
			{
				if (this.onStepComplete != null)
				{
					this._tween.OnStepComplete(new global::DG.Tweening.TweenCallback(this.onStepComplete.Invoke));
				}
			}
			else
			{
				this.onStepComplete = null;
			}
			if (this.hasOnComplete)
			{
				if (this.onComplete != null)
				{
					this._tween.OnComplete(new global::DG.Tweening.TweenCallback(this.onComplete.Invoke));
				}
			}
			else
			{
				this.onComplete = null;
			}
			if (this.autoPlay)
			{
				this._tween.Play<global::DG.Tweening.Tween>();
			}
			else
			{
				this._tween.Pause<global::DG.Tweening.Tween>();
			}
		}

		private void OnDestroy()
		{
			if (this._tween != null && this._tween.IsActive())
			{
				this._tween.Kill(false);
			}
			this._tween = null;
		}

		public override void DOPlay()
		{
			global::DG.Tweening.DOTween.Play(base.gameObject);
		}

		public void DOPlayById(string id)
		{
			global::DG.Tweening.DOTween.Play(base.gameObject, id);
		}

		public void DOPlayAllById(string id)
		{
			global::DG.Tweening.DOTween.Play(id);
		}

		public void DOPlayNext()
		{
			global::DG.Tweening.DOTweenAnimation[] components = base.GetComponents<global::DG.Tweening.DOTweenAnimation>();
			while (this._playCount < components.Length - 1)
			{
				this._playCount++;
				global::DG.Tweening.DOTweenAnimation dotweenAnimation = components[this._playCount];
				if (dotweenAnimation != null && dotweenAnimation._tween != null && !dotweenAnimation._tween.IsPlaying() && !dotweenAnimation._tween.IsComplete())
				{
					dotweenAnimation._tween.Play<global::DG.Tweening.Tween>();
					break;
				}
			}
		}

		public override void DOPause()
		{
			global::DG.Tweening.DOTween.Pause(base.gameObject);
		}

		public override void DOTogglePause()
		{
			global::DG.Tweening.DOTween.TogglePause(base.gameObject);
		}

		public override void DORewind()
		{
			global::DG.Tweening.DOTween.Rewind(base.gameObject, true);
		}

		public override void DORestart(bool fromHere = false)
		{
			if (this._tween == null)
			{
				if (global::DG.Tweening.Core.Debugger.logPriority > 1)
				{
					global::DG.Tweening.Core.Debugger.LogNullTween(this._tween);
				}
				return;
			}
			if (fromHere && this.isRelative)
			{
				this.ReEvaluateRelativeTween();
			}
			this._tween.Restart(true);
		}

		public void DORestartById(string id)
		{
			global::DG.Tweening.DOTween.Restart(base.gameObject, id, true);
		}

		public void DORestartAllById(string id)
		{
			global::DG.Tweening.DOTween.Restart(id, true);
		}

		public override void DOComplete()
		{
			global::DG.Tweening.DOTween.Complete(base.gameObject);
		}

		public override void DOKill()
		{
			global::DG.Tweening.DOTween.Kill(base.gameObject, false);
			this._tween = null;
		}

		private void ReEvaluateRelativeTween()
		{
			if (this.animationType == global::DG.Tweening.Core.DOTweenAnimationType.Move)
			{
				((global::DG.Tweening.Tweener)this._tween).ChangeEndValue(base.transform.position + this.endValueV3, true);
			}
			else if (this.animationType == global::DG.Tweening.Core.DOTweenAnimationType.LocalMove)
			{
				((global::DG.Tweening.Tweener)this._tween).ChangeEndValue(base.transform.localPosition + this.endValueV3, true);
			}
		}

		public float delay;

		public float duration = 1f;

		public global::DG.Tweening.Ease easeType = global::DG.Tweening.Ease.OutQuad;

		public global::UnityEngine.AnimationCurve easeCurve = new global::UnityEngine.AnimationCurve(new global::UnityEngine.Keyframe[]
		{
			new global::UnityEngine.Keyframe(0f, 0f),
			new global::UnityEngine.Keyframe(1f, 1f)
		});

		public global::DG.Tweening.LoopType loopType;

		public int loops = 1;

		public string id = string.Empty;

		public bool isRelative;

		public bool isFrom;

		public bool autoKill = true;

		public bool isValid;

		public global::DG.Tweening.Core.DOTweenAnimationType animationType;

		public bool autoPlay = true;

		public float endValueFloat;

		public global::UnityEngine.Vector3 endValueV3;

		public global::UnityEngine.Color endValueColor = new global::UnityEngine.Color(1f, 1f, 1f, 1f);

		public string endValueString = string.Empty;

		public bool optionalBool0;

		public float optionalFloat0;

		public int optionalInt0;

		public global::DG.Tweening.RotateMode optionalRotationMode;

		public global::DG.Tweening.ScrambleMode optionalScrambleMode;

		public string optionalString;

		public bool hasOnStart;

		public bool hasOnStepComplete;

		public bool hasOnComplete;

		public global::UnityEngine.Events.UnityEvent onStart;

		public global::UnityEngine.Events.UnityEvent onStepComplete;

		public global::UnityEngine.Events.UnityEvent onComplete;

		private global::DG.Tweening.Tween _tween;

		private int _playCount = -1;
	}
}
