using System;
using UnityEngine;
using UnityEngine.Events;

public class NextUnitModule : global::UIModule
{
	public void Setup()
	{
		this.previous.SetAction("switch_unit", "controls_action_prev_unit", 0, true, this.prevSprite, null);
		this.previous.OnAction(new global::UnityEngine.Events.UnityAction(this.NextUnit), false, true);
		this.next.SetAction("switch_unit", "controls_action_next_unit", 0, false, this.nextSprite, null);
		this.next.OnAction(new global::UnityEngine.Events.UnityAction(this.PrevUnit), false, true);
	}

	private void PrevUnit()
	{
		global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<bool>(global::Notices.NEXT_UNIT, false);
	}

	private void NextUnit()
	{
		global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<bool>(global::Notices.NEXT_UNIT, true);
	}

	public global::ButtonGroup previous;

	public global::ButtonGroup next;

	public global::UnityEngine.Sprite prevSprite;

	public global::UnityEngine.Sprite nextSprite;
}
