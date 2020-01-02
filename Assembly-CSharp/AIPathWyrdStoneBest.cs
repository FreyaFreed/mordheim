using System;
using System.Collections.Generic;
using RAIN.Core;
using UnityEngine;

public class AIPathWyrdStoneBest : global::AIPathWyrdStoneClosest
{
	public override void Start(global::RAIN.Core.AI ai)
	{
		base.Start(ai);
		this.actionName = "PathWyrdStoneBest";
	}

	public override global::System.Collections.Generic.List<global::SearchPoint> GetTargets()
	{
		global::System.Collections.Generic.List<global::SearchPoint> targets = base.GetTargets();
		float num = (float)this.unitCtrlr.unit.Movement;
		num *= num;
		int num2 = 0;
		for (int i = targets.Count - 1; i >= 0; i--)
		{
			if (global::UnityEngine.Vector3.SqrMagnitude(targets[i].transform.position - this.unitCtrlr.transform.position) > num || !this.CanSeeSearchPoint(targets[i]))
			{
				targets.RemoveAt(i);
			}
			else if (targets[i].items[0].PriceSold > num2)
			{
				num2 = targets[i].items[0].PriceSold;
			}
		}
		for (int j = targets.Count - 1; j >= 0; j--)
		{
			if (targets[j].items[0].PriceSold < num2)
			{
				targets.RemoveAt(j);
			}
		}
		return targets;
	}

	private bool CanSeeSearchPoint(global::SearchPoint target)
	{
		global::UnityEngine.Vector3 position = this.unitCtrlr.transform.position;
		position.y += 1.5f;
		global::UnityEngine.Vector3 position2 = target.transform.position;
		position2.y += 1.25f;
		float num = global::UnityEngine.Vector3.SqrMagnitude(position - position2);
		global::UnityEngine.Physics.Raycast(position, position2 - position, out this.hitInfo, (float)this.unitCtrlr.unit.ViewDistance, global::LayerMaskManager.fowMask);
		return this.hitInfo.collider == null || this.hitInfo.distance * this.hitInfo.distance > num;
	}

	private global::UnityEngine.RaycastHit hitInfo;
}
