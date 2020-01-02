using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIOverviewController : global::CanvasGroupDisabler
{
	public void Refresh(int idx)
	{
		this.cycleLeft.SetAction("cycling", "controls_action_prev_unit", 0, true, null, null);
		this.cycleRight.SetAction("cycling", "controls_action_next_unit", 0, false, null, null);
		for (int i = 0; i < this.zoomLevels.Count; i++)
		{
			this.zoomLevels[i].gameObject.SetActive(i == idx);
		}
		int availableMapBeacons = global::PandoraSingleton<global::MissionManager>.Instance.GetAvailableMapBeacons();
		if (this.oldBeaconCount != availableMapBeacons)
		{
			this.oldBeaconCount = availableMapBeacons;
			this.beaconCount.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("overview_beacon_count", new string[]
			{
				this.oldBeaconCount.ToConstantString(),
				5.ToConstantString()
			});
		}
	}

	public override void OnEnable()
	{
		base.OnEnable();
		this.Activate(true);
		if (global::PandoraSingleton<global::MissionManager>.Instance.StateMachine.GetActiveStateId() == 1)
		{
			this.cycleLeft.gameObject.SetActive(false);
			this.cycleRight.gameObject.SetActive(false);
		}
		else
		{
			this.cycleLeft.gameObject.SetActive(true);
			this.cycleRight.gameObject.SetActive(true);
		}
	}

	public override void OnDisable()
	{
		base.OnDisable();
		this.Activate(false);
	}

	private void Activate(bool activate)
	{
		this.beacons.SetActive(activate);
		this.zooms.SetActive(activate);
	}

	public global::ButtonGroup cycleLeft;

	public global::ButtonGroup cycleRight;

	public global::System.Collections.Generic.List<global::UnityEngine.UI.Image> zoomLevels;

	public global::UnityEngine.UI.Text beaconCount;

	public global::UnityEngine.GameObject beacons;

	public global::UnityEngine.GameObject zooms;

	private int oldBeaconCount = 99;
}
