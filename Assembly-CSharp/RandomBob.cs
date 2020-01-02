using System;
using DG.Tweening;
using UnityEngine;

public class RandomBob : global::UnityEngine.MonoBehaviour
{
	public void Awake()
	{
		if (this.bobRange != 0f)
		{
			this.StartBob();
		}
		if (this.rotRange != 0f)
		{
			this.StartRot();
		}
	}

	private void StartBob()
	{
		global::DG.Tweening.Tweener tweener = base.transform.DOMove(new global::UnityEngine.Vector3(0f, (float)global::PandoraSingleton<global::GameManager>.Instance.LocalTyche.Rand((double)(-(double)this.bobRange), (double)this.bobRange), 0f), 3f, false);
		tweener.SetRelative<global::DG.Tweening.Tweener>();
		tweener.OnComplete(new global::DG.Tweening.TweenCallback(this.BobComplete));
		tweener.SetEase(global::DG.Tweening.Ease.InOutBack);
		tweener.SetLoops(2, global::DG.Tweening.LoopType.Yoyo);
		tweener.SetDelay((float)global::PandoraSingleton<global::GameManager>.Instance.LocalTyche.Rand(0.1, (double)this.randomRange));
	}

	private void StartRot()
	{
		float num = (float)global::PandoraSingleton<global::GameManager>.Instance.LocalTyche.Rand((double)(-(double)this.rotRange), (double)this.rotRange);
		float num2 = (float)global::PandoraSingleton<global::GameManager>.Instance.LocalTyche.Rand((double)(-(double)this.rotRange), (double)this.rotRange);
		if (num < 0f)
		{
			num += 360f;
		}
		if (num2 < 0f)
		{
			num2 += 360f;
		}
		global::UnityEngine.Quaternion lhs = global::UnityEngine.Quaternion.AngleAxis(num, global::UnityEngine.Vector3.forward);
		global::UnityEngine.Quaternion rhs = global::UnityEngine.Quaternion.AngleAxis(num2, base.transform.right);
		global::DG.Tweening.Tweener t = base.transform.DORotate((lhs * rhs).eulerAngles, 2f, global::DG.Tweening.RotateMode.Fast);
		t.SetEase(global::DG.Tweening.Ease.InOutSine);
		t.OnComplete(new global::DG.Tweening.TweenCallback(this.RotComplete));
	}

	public void BobComplete()
	{
		this.StartBob();
	}

	public void RotComplete()
	{
		this.StartRot();
	}

	public float randomRange = 3f;

	public float bobRange = 0.2f;

	public float rotRange = 5f;
}
