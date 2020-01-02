using System;
using Pathfinding;
using RAIN.Action;
using RAIN.Core;

public class AIPatrolPoint : global::AIBase
{
	public override void Start(global::RAIN.Core.AI ai)
	{
		base.Start(ai);
		this.actionName = "PatrolPoint";
		this.searching = true;
		this.unitCtrlr.ReduceAlliesNavCutterSize(delegate
		{
			float num = (float)(this.unitCtrlr.unit.Movement * this.unitCtrlr.unit.CurrentStrategyPoints);
			int length = (int)global::PandoraSingleton<global::GameManager>.Instance.LocalTyche.Rand((double)this.unitCtrlr.unit.Movement, (double)num);
			global::Pathfinding.RandomPath p = global::Pathfinding.RandomPath.Construct(this.unitCtrlr.transform.position, length, new global::Pathfinding.OnPathDelegate(this.OnPathFinish));
			global::PandoraSingleton<global::MissionManager>.Instance.PathSeeker.StartPath(p, new global::Pathfinding.OnPathDelegate(this.OnPathFinish), 1);
		});
	}

	public override global::RAIN.Action.RAINAction.ActionResult Execute(global::RAIN.Core.AI ai)
	{
		if (this.searching)
		{
			return global::RAIN.Action.RAINAction.ActionResult.RUNNING;
		}
		if (this.success)
		{
			return global::RAIN.Action.RAINAction.ActionResult.SUCCESS;
		}
		return global::RAIN.Action.RAINAction.ActionResult.FAILURE;
	}

	public override void Stop(global::RAIN.Core.AI ai)
	{
		base.Stop(ai);
	}

	private void OnPathFinish(global::Pathfinding.Path p)
	{
		this.searching = false;
		this.success = (p != null && p.GetTotalLength() > 0f);
		this.path = p;
	}

	private global::Pathfinding.Path path;

	private bool searching;
}
