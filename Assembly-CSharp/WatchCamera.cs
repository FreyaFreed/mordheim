using System;
using System.Collections.Generic;
using UnityEngine;

public class WatchCamera : global::ICheapState
{
	public WatchCamera(global::CameraManager camMngr)
	{
		this.mngr = camMngr;
		this.camTrans = this.mngr.transform;
		this.dummyCam = this.mngr.dummyCam.transform;
	}

	public void Destroy()
	{
	}

	public void Enter(int from)
	{
		this.watcher = null;
		this.previousEnemy = null;
		this.lastWatcher = null;
		this.prevDistance = 10000f;
		this.watcherDistance = 10000f;
	}

	public void Exit(int to)
	{
	}

	public void Update()
	{
		global::UnitController currentUnit = global::PandoraSingleton<global::MissionManager>.Instance.GetCurrentUnit();
		if (currentUnit == null || !currentUnit.IsImprintVisible())
		{
			this.visionTime = 5f;
			return;
		}
		if (currentUnit != this.previousEnemy)
		{
			this.visionTime = 5f;
		}
		this.watcher = null;
		this.watcherDistance = 10000f;
		bool flag = false;
		global::System.Collections.Generic.List<global::UnitController> viewers = currentUnit.Imprint.Viewers;
		for (int i = 0; i < viewers.Count; i++)
		{
			if (viewers[i].unit.Status != global::UnitStateId.OUT_OF_ACTION)
			{
				float num = global::UnityEngine.Vector3.SqrMagnitude(viewers[i].transform.position - currentUnit.transform.position);
				if (viewers[i] == this.lastWatcher)
				{
					flag = true;
					this.prevDistance = num;
				}
				if (num < this.watcherDistance)
				{
					this.watcher = viewers[i];
					this.watcherDistance = num;
				}
			}
		}
		global::UnitController unitController = null;
		this.visionTime += global::UnityEngine.Time.deltaTime;
		if (!flag && this.watcher != null && this.visionTime > 0.5f)
		{
			this.visionTime = 0f;
			unitController = this.watcher;
		}
		else if (flag)
		{
			if (this.lastWatcher != this.watcher && this.watcher != null)
			{
				if (this.watcherDistance > this.prevDistance / 2f)
				{
					this.visionTime = 0f;
				}
				if (this.watcher != null && this.visionTime > 0.5f)
				{
					unitController = this.watcher;
				}
				else
				{
					unitController = this.lastWatcher;
				}
			}
			else
			{
				this.visionTime = 0f;
				unitController = this.lastWatcher;
			}
		}
		if (unitController)
		{
			global::UnityEngine.Transform transform = unitController.transform;
			float num2 = 1.5f;
			if (currentUnit.unit.Data.UnitSizeId == global::UnitSizeId.LARGE)
			{
				num2 = 2f;
			}
			this.mngr.SetZoomDiff(unitController.unit.Data.UnitSizeId == global::UnitSizeId.LARGE);
			this.mngr.SetTarget(unitController.transform);
			this.mngr.LookAtFocus(currentUnit.transform, false, false);
			this.mngr.SetShoulderCam(unitController.unit.Data.UnitSizeId == global::UnitSizeId.LARGE, false);
			if (unitController != this.lastWatcher)
			{
				this.mngr.Transition(2f, false);
			}
			this.lastWatcher = unitController;
			this.dummyCam.LookAt(currentUnit.transform.position + global::UnityEngine.Vector3.up * num2);
			this.mngr.SetDOFTarget(currentUnit.transform, num2);
			if (this.previousEnemy != currentUnit)
			{
				global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<bool>(global::Notices.MISSION_SHOW_ENEMY, true);
				if (unitController && currentUnit)
				{
					global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<global::UnitController, int>(global::Notices.UPDATE_TARGET, currentUnit, currentUnit.unit.warbandIdx);
				}
				this.previousEnemy = currentUnit;
			}
		}
	}

	public void FixedUpdate()
	{
	}

	private const float TIME_TO_SWITCH = 0.5f;

	public global::UnityEngine.Vector3 targetPosition = global::UnityEngine.Vector3.zero;

	private global::UnitController watcher;

	private global::UnitController previousEnemy;

	public global::UnitController lastWatcher;

	private float prevDistance;

	private float watcherDistance;

	private float visionTime;

	private global::CameraManager mngr;

	private global::UnityEngine.Transform camTrans;

	private global::UnityEngine.Transform dummyCam;
}
