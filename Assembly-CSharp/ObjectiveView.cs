using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveView : global::UnityEngine.MonoBehaviour
{
	public void Set(global::Objective obj, int objIndex, string desc, bool loading)
	{
		this.mainObjective = obj;
		this.subIndex = objIndex;
		this.objectiveText.text = desc;
	}

	public void UpdateObjective(bool loading, int counter1 = -1, int counter2 = -1)
	{
		if (this.mainObjective != null && this.mainObjective.Locked)
		{
			if (this.lastObjectiveInfo == null)
			{
				this.lastObjectiveInfo = new global::ObjectiveInfo();
				this.lastObjectiveInfo.locked = true;
			}
			base.gameObject.SetActive(false);
			return;
		}
		base.gameObject.SetActive(true);
		bool flag = false;
		if (this.mainObjective != null)
		{
			if (this.subIndex == -1)
			{
				counter1 = (int)this.mainObjective.counter.x;
				counter2 = (int)this.mainObjective.counter.y;
				flag = this.mainObjective.done;
			}
			else
			{
				flag = this.mainObjective.dones[this.subIndex];
			}
		}
		else if (counter1 != -1 && counter2 != -1)
		{
			flag = (counter1 == counter2);
		}
		if (this.lastObjectiveInfo == null)
		{
			this.lastObjectiveInfo = new global::ObjectiveInfo();
			this.lastObjectiveInfo.locked = false;
			this.lastObjectiveInfo.counter1 = counter1;
			this.lastObjectiveInfo.counter2 = counter2;
			this.lastObjectiveInfo.completed = flag;
		}
		this.toggleObjective.isOn = flag;
		this.counter.text = ((counter2 == -1) ? counter1.ToString() : string.Format("{0} / {1}", counter1, counter2));
		this.counter.gameObject.SetActive(!loading && (counter1 != -1 || counter2 != -1));
		this.complete.gameObject.SetActive(!loading);
		if (loading)
		{
			this.Hide();
		}
		else if (this.lastObjectiveInfo.locked)
		{
			this.OnNew();
		}
		else if (!this.lastObjectiveInfo.completed && flag)
		{
			this.OnComplete();
		}
		else if ((this.lastObjectiveInfo.completed && !flag) || (this.lastObjectiveInfo.counter1 != counter1 && counter2 != -1))
		{
			this.OnUpdate();
		}
		else
		{
			this.Hide();
		}
		this.lastObjectiveInfo.locked = false;
		this.lastObjectiveInfo.completed = flag;
		this.lastObjectiveInfo.counter1 = counter1;
		this.lastObjectiveInfo.counter2 = counter2;
	}

	public void OnNew()
	{
		this.completeCanvasGroup.alpha = 1f;
		this.complete.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("objective_new");
		this.Move();
		this.FadeOut();
		global::PandoraSingleton<global::Pan>.Instance.Narrate("new_objective");
	}

	public void OnUpdate()
	{
		global::DG.Tweening.DOTween.Kill(this, true);
		this.completeCanvasGroup.alpha = 1f;
		this.complete.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("objective_updated");
		this.Move();
		this.FadeOut();
		if (this.mainObjective != null)
		{
			global::PandoraSingleton<global::Pan>.Instance.Narrate("objective_updated");
		}
	}

	public void OnComplete()
	{
		global::DG.Tweening.DOTween.Kill(this, true);
		this.completeCanvasGroup.alpha = 1f;
		this.complete.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("objective_completed");
		this.Move();
		this.FadeOut();
		if (this.mainObjective != null)
		{
			global::PandoraSingleton<global::Pan>.Instance.Narrate("objective_completed");
		}
	}

	public void FadeOut()
	{
		global::DG.Tweening.DOTween.To(() => this.completeCanvasGroup.alpha, delegate(float alpha)
		{
			this.completeCanvasGroup.alpha = alpha;
		}, 0f, 1f).SetDelay(5f).SetTarget(this);
	}

	public void Move()
	{
		global::UnityEngine.RectTransform textTransform = (global::UnityEngine.RectTransform)this.completeCanvasGroup.transform;
		global::UnityEngine.Vector2 anchoredPosition = textTransform.anchoredPosition;
		anchoredPosition.x = this.startPositionX;
		textTransform.anchoredPosition = anchoredPosition;
		global::DG.Tweening.DOTween.To(() => textTransform.anchoredPosition.x, delegate(float x)
		{
			global::UnityEngine.Vector2 anchoredPosition2 = textTransform.anchoredPosition;
			anchoredPosition2.x = x;
			textTransform.anchoredPosition = anchoredPosition2;
		}, this.endPosition, 0.5f).SetTarget(this);
	}

	public void Hide()
	{
		if (!global::DG.Tweening.DOTween.IsTweening(this))
		{
			this.completeCanvasGroup.alpha = 0f;
		}
	}

	public global::UnityEngine.UI.Toggle toggleObjective;

	public global::UnityEngine.UI.Text counter;

	public global::UnityEngine.UI.Text objectiveText;

	public global::UnityEngine.UI.Text complete;

	public global::UnityEngine.CanvasGroup completeCanvasGroup;

	public float startPositionX;

	public float endPosition;

	public global::ObjectiveInfo lastObjectiveInfo;

	public global::Objective mainObjective;

	public int subIndex;

	private bool sequencePlaying;
}
