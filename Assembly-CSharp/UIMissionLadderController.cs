using System;
using UnityEngine;
using UnityEngine.UI;

public class UIMissionLadderController : global::ContentView<global::UIMissionLadderGroup, int>
{
	protected override void Awake()
	{
		base.Awake();
		this.arrow.enabled = false;
		global::PandoraSingleton<global::NoticeManager>.Instance.RegisterListener(global::Notices.MISSION_ROUND_START, new global::DelReceiveNotice(this.ShowLadder));
		global::PandoraSingleton<global::NoticeManager>.Instance.RegisterListener(global::Notices.LADDER_CHANGED, new global::DelReceiveNotice(this.OnLadderChanged));
		global::PandoraSingleton<global::NoticeManager>.Instance.RegisterListener(global::Notices.LADDER_UNIT_CHANGED, new global::DelReceiveNotice(this.OnLadderCurrentIndexChanged));
	}

	private void ShowLadder()
	{
		global::PandoraSingleton<global::NoticeManager>.Instance.RemoveListener(global::Notices.MISSION_ROUND_START, new global::DelReceiveNotice(this.ShowLadder));
		this.OnEnable();
		this.currentIdx = global::PandoraSingleton<global::MissionManager>.Instance.CurrentLadderIdx;
	}

	private void OnLadderCurrentIndexChanged()
	{
		if (base.Components.Count == 0)
		{
			this.OnLadderChanged();
		}
		global::UnitController item = (global::UnitController)global::PandoraSingleton<global::NoticeManager>.Instance.Parameters[0];
		bool force = global::PandoraSingleton<global::NoticeManager>.Instance.Parameters.Count > 1 && (bool)global::PandoraSingleton<global::NoticeManager>.Instance.Parameters[1];
		this.currentIdx = global::PandoraSingleton<global::MissionManager>.Instance.InitiativeLadder.IndexOf(item);
		if (base.Components.Count > 0)
		{
			if (this.currentIdx >= 0 && this.currentIdx < base.Components.Count)
			{
				if (this.currentIdx == 0)
				{
					this.Transform.anchoredPosition = new global::UnityEngine.Vector2(0f, this.Transform.anchoredPosition.y);
				}
				else
				{
					this.Transform.anchoredPosition = new global::UnityEngine.Vector2(-this.currentSize.x * (float)this.currentIdx, this.Transform.anchoredPosition.y);
				}
				if (!this.arrow.enabled)
				{
					this.arrow.enabled = true;
				}
				for (int i = 0; i < base.Components.Count; i++)
				{
					base.Components[i].SetCurrent(i <= this.currentIdx, force, i == global::PandoraSingleton<global::MissionManager>.Instance.CurrentLadderIdx);
					global::UnityEngine.UI.LayoutElement component = base.Components[i].gameObject.GetComponent<global::UnityEngine.UI.LayoutElement>();
					component.preferredWidth = ((i > this.currentIdx) ? this.normalSize.x : this.currentSize.x);
					component.preferredHeight = ((i > this.currentIdx) ? this.normalSize.y : this.currentSize.y);
				}
			}
			else
			{
				this.Transform.anchoredPosition = new global::UnityEngine.Vector2(0f, this.Transform.anchoredPosition.y);
				this.arrow.enabled = false;
			}
		}
	}

	private void OnLadderChanged()
	{
		for (int i = 0; i < global::PandoraSingleton<global::MissionManager>.Instance.InitiativeLadder.Count; i++)
		{
			base.Add(i);
		}
		base.OnAddEnd();
	}

	protected override void OnAdd(global::UIMissionLadderGroup component, int ladderIndex)
	{
		component.Set(global::PandoraSingleton<global::MissionManager>.Instance.InitiativeLadder[ladderIndex], ladderIndex == this.currentIdx);
		global::UnityEngine.UI.LayoutElement component2 = component.gameObject.GetComponent<global::UnityEngine.UI.LayoutElement>();
		component2.preferredWidth = ((ladderIndex > this.currentIdx) ? this.normalSize.x : this.currentSize.x);
		component2.preferredHeight = ((ladderIndex > this.currentIdx) ? this.normalSize.y : this.currentSize.y);
	}

	public global::UIMissionLadderGroup selectedUnit;

	public global::UnityEngine.RectTransform Transform;

	public global::UnityEngine.UI.Image arrow;

	public global::UnityEngine.Vector2 normalSize = new global::UnityEngine.Vector2(75f, 150f);

	public global::UnityEngine.Vector2 currentSize = new global::UnityEngine.Vector2(100f, 200f);

	private int currentIdx;
}
