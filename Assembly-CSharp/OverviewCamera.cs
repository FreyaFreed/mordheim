using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OverviewCamera : global::ICheapState
{
	public OverviewCamera(global::CameraManager camMngr)
	{
		global::OverviewCamera <>f__this = this;
		this.mngr = camMngr;
		this.cam = this.mngr.gameObject.GetComponent<global::UnityEngine.Camera>();
		this.dummyCam = camMngr.dummyCam.transform;
		global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadResourceAsync<global::UnityEngine.GameObject>("prefabs/camera/overview_target", delegate(global::UnityEngine.Object o)
		{
			global::UnityEngine.GameObject gameObject = (global::UnityEngine.GameObject)global::UnityEngine.Object.Instantiate(o);
			global::UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(gameObject, camMngr.gameObject.scene);
			gameObject.transform.SetParent(null);
			gameObject.transform.position = global::UnityEngine.Vector3.zero;
			gameObject.transform.rotation = global::UnityEngine.Quaternion.identity;
			<>f__this.lookAtTarget = gameObject.transform;
			<>f__this.lookAtTarget.gameObject.SetActive(false);
		}, false);
		this.zoomSpeed = 50f;
		this.zoomIdx = 2;
		this.currentZoom = (float)global::OverviewCamera.zoomLevels[this.zoomIdx];
		this.rotationAngle = 90f;
		this.rotationSpeed = 180f;
		this.rotationIdx = 0;
		this.SetTargetRotation();
		this.cyclingUnitIdx = 0;
		this.cyclingUnits = new global::System.Collections.Generic.List<global::UnitController>();
	}

	public void Destroy()
	{
	}

	public void FixedUpdate()
	{
	}

	public void Enter(int from)
	{
		this.cyclingUnits.Clear();
		this.active = true;
		global::PandoraSingleton<global::GameManager>.Instance.StartCoroutine(this.DisplayFlyingOverviews());
		if (global::PandoraSingleton<global::GameManager>.Instance.TacticalViewHelpersEnabled)
		{
			global::PandoraSingleton<global::MissionManager>.Instance.ActivateMapObjectiveZones(true);
		}
		this.lookAtTarget.gameObject.SetActive(true);
		this.moveSpeed = this.moveSpeedMin;
		if (global::PandoraSingleton<global::MissionManager>.Instance.GetCurrentUnit().IsPlayed())
		{
			this.SetTarget(global::PandoraSingleton<global::MissionManager>.Instance.GetCurrentUnit().transform, true);
		}
		else if (global::PandoraSingleton<global::MissionManager>.Instance.StateMachine.GetActiveStateId() == 1)
		{
			global::System.Collections.Generic.List<global::SpawnNode> list = global::PandoraSingleton<global::MissionStartData>.Instance.spawnNodes[global::PandoraSingleton<global::MissionManager>.Instance.GetMyWarbandCtrlr().idx];
			this.SetTarget(list[0].transform, true);
		}
		else
		{
			global::System.Collections.Generic.List<global::UnitController> allUnits = global::PandoraSingleton<global::MissionManager>.Instance.GetAllUnits();
			for (int i = 0; i < allUnits.Count; i++)
			{
				if (allUnits[i].IsPlayed() && allUnits[i].unit.Status != global::UnitStateId.OUT_OF_ACTION)
				{
					this.SetTarget(allUnits[i].transform, true);
					break;
				}
			}
		}
		this.SetTargetZoom();
		this.EnterOrthoCam();
		global::PandoraSingleton<global::UIMissionManager>.Instance.ShowingOverview = true;
		global::PandoraSingleton<global::UIMissionManager>.Instance.StateMachine.ChangeState(1);
		global::PandoraSingleton<global::UIMissionManager>.Instance.overview.OnEnable();
		global::PandoraSingleton<global::UIMissionManager>.Instance.HideUnitStats();
		global::PandoraSingleton<global::UIMissionManager>.Instance.HidePropsInfo();
		global::PandoraSingleton<global::UIMissionManager>.Instance.unitAction.OnDisable();
		global::PandoraSingleton<global::PandoraInput>.Instance.PushInputLayer(global::PandoraInput.InputLayer.MENU);
	}

	public void Exit(int to)
	{
		this.active = false;
		global::PandoraSingleton<global::GameManager>.Instance.StopCoroutine(this.DisplayFlyingOverviews());
		global::PandoraSingleton<global::PandoraInput>.Instance.PopInputLayer(global::PandoraInput.InputLayer.MENU);
		if (!global::PandoraSingleton<global::MissionManager>.Exists())
		{
			return;
		}
		global::PandoraSingleton<global::MissionManager>.Instance.CamManager.AddLOSLayer("characters");
		this.DeactivateFlyingOverviews();
		if (this.lookAtTarget != null && this.lookAtTarget.gameObject != null)
		{
			this.lookAtTarget.gameObject.SetActive(false);
		}
		this.ExitOrthoCam();
		if (global::PandoraSingleton<global::GameManager>.Instance.TacticalViewHelpersEnabled)
		{
			global::PandoraSingleton<global::MissionManager>.Instance.ActivateMapObjectiveZones(false);
		}
		global::PandoraSingleton<global::UIMissionManager>.Instance.ShowingOverview = false;
		global::PandoraSingleton<global::UIMissionManager>.Instance.HidePropsInfo();
		global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<global::UnitController>(global::Notices.CURRENT_UNIT_CHANGED, global::PandoraSingleton<global::MissionManager>.Instance.GetCurrentUnit());
		if (to == 7)
		{
			global::PandoraSingleton<global::UIMissionManager>.Instance.StateMachine.ChangeState(6);
		}
		else
		{
			global::PandoraSingleton<global::UIMissionManager>.Instance.StateMachine.ChangeState(0);
		}
		global::PandoraSingleton<global::UIMissionManager>.Instance.overview.OnDisable();
	}

	private void DeactivateFlyingOverviews()
	{
		for (int i = 0; i < this.flyingOverviews.Count; i++)
		{
			if (this.flyingOverviews[i] != null)
			{
				this.flyingOverviews[i].Deactivate();
			}
		}
		this.flyingOverviews.Clear();
	}

	private global::System.Collections.IEnumerator DisplayFlyingOverviews()
	{
		this.DeactivateFlyingOverviews();
		yield return new global::UnityEngine.WaitForSeconds(0.5f);
		global::PandoraSingleton<global::FlyingTextManager>.Instance.ResetWorldCorners();
		global::System.Collections.Generic.List<global::MapImprint> imprints = global::PandoraSingleton<global::MissionManager>.Instance.MapImprints;
		for (int i = 0; i < imprints.Count; i++)
		{
			if (i % 5 == 0)
			{
				yield return null;
			}
			this.flyingOverviews.Add(null);
			if (imprints[i].State == global::MapImprintStateId.VISIBLE || imprints[i].State == global::MapImprintStateId.LOST)
			{
				if (imprints[i].UnitCtrlr != null)
				{
					this.cyclingUnits.Add(imprints[i].UnitCtrlr);
				}
				this.DisplayFlyingText(i);
			}
		}
		this.cyclingUnits.Sort(new global::LadderSorter());
		yield break;
	}

	private void DisplayFlyingText(int idx)
	{
		global::PandoraSingleton<global::FlyingTextManager>.Instance.GetFlyingText(global::FlyingTextId.OVERVIEW_UNIT, delegate(global::FlyingText flyingText)
		{
			global::FlyingOverview flyingOverview = (global::FlyingOverview)flyingText;
			if (!this.active || idx >= this.flyingOverviews.Count)
			{
				flyingOverview.Deactivate();
				return;
			}
			global::MapImprint mapImprint = global::PandoraSingleton<global::MissionManager>.Instance.MapImprints[idx];
			bool clamp = false;
			global::MapBeacon mapBeacon = null;
			global::System.Collections.Generic.List<global::MapBeacon> mapBeacons = global::PandoraSingleton<global::MissionManager>.Instance.GetMapBeacons();
			for (int i = 0; i < mapBeacons.Count; i++)
			{
				if (mapBeacons[i].imprint == mapImprint)
				{
					mapBeacon = mapBeacons[i];
					clamp = true;
				}
			}
			global::System.Collections.Generic.List<global::WarbandController> warbandCtrlrs = global::PandoraSingleton<global::MissionManager>.Instance.WarbandCtrlrs;
			for (int j = 0; j < warbandCtrlrs.Count; j++)
			{
				if (warbandCtrlrs[j].wagon != null && warbandCtrlrs[j].wagon.mapImprint == mapImprint)
				{
					clamp = true;
				}
			}
			if (mapBeacon != null)
			{
				mapBeacon.flyingOverview = flyingOverview;
				mapBeacon.Refresh();
				flyingOverview.gameObject.SetActive(mapBeacon.isActiveAndEnabled);
			}
			flyingOverview.Set(mapImprint, clamp, false);
			if (this.flyingOverviews[idx] != null)
			{
				this.flyingOverviews[idx].Deactivate();
			}
			this.flyingOverviews[idx] = flyingOverview;
		});
	}

	public void Update()
	{
		if (global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp("restore_map", 6))
		{
			if (global::PandoraSingleton<global::MissionManager>.Instance.GetCurrentUnit().IsPlayed())
			{
				this.SetTarget(global::PandoraSingleton<global::MissionManager>.Instance.GetCurrentUnit().transform, true);
			}
			else
			{
				this.SetTarget(global::PandoraSingleton<global::MissionManager>.Instance.GetMyWarbandCtrlr().GetLeader().transform, true);
			}
			this.zoomIdx = global::OverviewCamera.zoomLevels.Length - 1;
			this.SetTargetZoom();
			this.rotationIdx = 0;
			this.SetTargetRotation();
		}
		if (global::PandoraSingleton<global::MissionManager>.Instance.StateMachine.GetActiveStateId() != 1)
		{
			if (global::PandoraSingleton<global::PandoraInput>.Instance.GetNegKeyUp("cycling", 6))
			{
				this.cyclingUnitIdx = ((this.cyclingUnitIdx - 1 < 0) ? (this.cyclingUnits.Count - 1) : (this.cyclingUnitIdx - 1));
				this.SetTarget(this.cyclingUnits[this.cyclingUnitIdx].Imprint.lastKnownPos, false);
			}
			else if (global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp("cycling", 6))
			{
				this.cyclingUnitIdx = ((this.cyclingUnitIdx + 1 >= this.cyclingUnits.Count) ? 0 : (this.cyclingUnitIdx + 1));
				this.SetTarget(this.cyclingUnits[this.cyclingUnitIdx].Imprint.lastKnownPos, false);
			}
		}
		global::UnityEngine.Vector3 vector = global::UnityEngine.Vector3.Lerp(this.lookAtTarget.position, this.lookAtTargetTargetPosition, 0.25f);
		if (this.currentSelectedIdx != -1)
		{
			this.moveSpeed = this.moveSpeedMin;
			this.increaseTime = 0f;
		}
		float num = global::PandoraSingleton<global::PandoraInput>.Instance.GetAxisRaw("h", 6);
		float num2 = global::PandoraSingleton<global::PandoraInput>.Instance.GetAxisRaw("v", 6);
		num += global::PandoraSingleton<global::PandoraInput>.Instance.GetAxis("scroll_map_x", 6);
		num2 += global::PandoraSingleton<global::PandoraInput>.Instance.GetAxis("scroll_map_y", 6);
		if (num != 0f || num2 != 0f)
		{
			global::UnityEngine.Vector3 forward = this.lookAtTarget.transform.forward;
			forward.y = 0f;
			global::UnityEngine.Vector3 a = num * this.lookAtTarget.transform.right + num2 * forward;
			this.moveSpeed = ((this.moveSpeed + this.moveSpeedIncrease * this.increaseTime >= this.moveSpeedMax) ? this.moveSpeedMax : (this.moveSpeed + this.moveSpeedIncrease * this.increaseTime));
			this.increaseTime += global::UnityEngine.Time.smoothDeltaTime;
			vector += a * this.moveSpeed * global::UnityEngine.Time.smoothDeltaTime;
			if (global::PandoraSingleton<global::MissionManager>.Instance.mapContour != null)
			{
				global::UnityEngine.Vector2 vector2 = new global::UnityEngine.Vector2(vector.x, vector.z);
				global::UnityEngine.Vector2 pointInsideMeshEdges = global::PandoraUtils.GetPointInsideMeshEdges(global::PandoraSingleton<global::MissionManager>.Instance.mapContour.FlatEdges, global::PandoraSingleton<global::MissionManager>.Instance.mapContour.center, vector2 * -1000f, vector2);
				vector.x = pointInsideMeshEdges.x;
				vector.z = pointInsideMeshEdges.y;
			}
			this.lookAtTargetTargetPosition = vector;
		}
		else
		{
			this.moveSpeed = this.moveSpeedMin;
			this.increaseTime = 0f;
		}
		float num3 = (float)(this.zoomIdx + 1);
		num3 *= 2.3f;
		this.lookAtTarget.transform.localScale = new global::UnityEngine.Vector3(num3, num3, num3);
		this.lookAtTarget.transform.position = vector;
		if (this.currentRotation != this.targetRotation)
		{
			this.currentRotation += this.rotationSpeed * global::UnityEngine.Time.smoothDeltaTime * (float)this.rotationDir;
			if ((this.rotationDir == -1 && this.currentRotation < this.targetRotation) || (this.rotationDir == 1 && this.currentRotation > this.targetRotation))
			{
				this.currentRotation = this.targetRotation;
			}
		}
		this.lookAtTarget.transform.rotation = global::UnityEngine.Quaternion.Euler(new global::UnityEngine.Vector3(0f, this.currentRotation, 0f));
		if (global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp("zoom", 6))
		{
			this.zoomIdx = ((this.zoomIdx - 1 < 0) ? (global::OverviewCamera.zoomLevels.Length - 1) : (this.zoomIdx - 1));
			this.SetTargetZoom();
		}
		else if (global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp("zoom_mouse", 6))
		{
			this.zoomIdx = ((this.zoomIdx - 1 < 0) ? this.zoomIdx : (this.zoomIdx - 1));
			this.SetTargetZoom();
		}
		else if (global::PandoraSingleton<global::PandoraInput>.Instance.GetNegKeyUp("zoom_mouse", 6))
		{
			this.zoomIdx = ((this.zoomIdx + 1 >= global::OverviewCamera.zoomLevels.Length) ? this.zoomIdx : (this.zoomIdx + 1));
			this.SetTargetZoom();
		}
		if (this.currentZoom != this.targetZoom)
		{
			this.currentZoom += this.zoomSpeed * global::UnityEngine.Time.smoothDeltaTime * (float)((this.currentZoom >= this.targetZoom) ? -1 : 1);
			if ((this.oldZoom >= this.targetZoom && this.currentZoom < this.targetZoom) || (this.oldZoom < this.targetZoom && this.currentZoom > this.targetZoom))
			{
				this.currentZoom = this.targetZoom;
			}
		}
		this.lookAtTarget.transform.position.y = 40f;
		global::UnityEngine.Camera main = global::UnityEngine.Camera.main;
		main.orthographicSize = this.currentZoom;
		global::UnityEngine.Vector3 vector3 = this.lookAtTarget.transform.position;
		main.transform.position = vector3;
		global::UnityEngine.Quaternion quaternion = global::UnityEngine.Quaternion.LookRotation(global::UnityEngine.Vector3.down);
		quaternion *= global::UnityEngine.Quaternion.Euler(0f, 0f, -this.currentRotation);
		global::UnityEngine.Vector3[] array = new global::UnityEngine.Vector3[]
		{
			main.ViewportToWorldPoint(new global::UnityEngine.Vector3(0.5f, 1f, main.nearClipPlane)),
			main.ViewportToWorldPoint(new global::UnityEngine.Vector3(1f, 0.5f, main.nearClipPlane)),
			main.ViewportToWorldPoint(new global::UnityEngine.Vector3(0.5f, 0f, main.nearClipPlane)),
			main.ViewportToWorldPoint(new global::UnityEngine.Vector3(0f, 0.5f, main.nearClipPlane))
		};
		global::UnityEngine.Vector3 vector4 = new global::UnityEngine.Vector3(0f, 0f, 0f);
		for (int i = 0; i < array.Length; i++)
		{
			vector4 += array[i] - this.mapBounds.ClosestPoint(array[i]);
		}
		vector3 -= vector4;
		float num4 = array[1].x - array[3].x;
		if (num4 > this.mapBounds.size.x)
		{
			vector3.x = this.mapBounds.center.x;
		}
		float num5 = array[0].z - array[2].z;
		if (num5 > this.mapBounds.size.z)
		{
			vector3.z = this.mapBounds.center.z;
		}
		vector3.y = 40f;
		this.dummyCam.position = vector3;
		this.dummyCam.rotation = quaternion;
		global::UnityEngine.Vector3 position = this.lookAtTarget.transform.position;
		position.y = 0f;
		this.currentSelectedIdx = -1;
		float num6 = float.MaxValue;
		for (int j = 0; j < this.flyingOverviews.Count; j++)
		{
			if (this.flyingOverviews[j] != null && this.flyingOverviews[j].imprint != null)
			{
				global::UnityEngine.Vector3 position2 = this.flyingOverviews[j].imprint.transform.position;
				position2.y = 0f;
				float num7 = global::UnityEngine.Vector3.SqrMagnitude(position - position2);
				if (num7 < 4f && num7 < num6)
				{
					num6 = num7;
					this.currentSelectedIdx = j;
				}
			}
		}
		bool flag = true;
		bool flag2 = true;
		if (this.currentSelectedIdx != -1)
		{
			flag2 = false;
			global::MapImprint imprint = this.flyingOverviews[this.currentSelectedIdx].imprint;
			switch (imprint.imprintType)
			{
			case global::MapImprintType.UNIT:
				this.SelectCurrentUnit(imprint.UnitCtrlr);
				flag = false;
				flag2 = true;
				goto IL_9C2;
			case global::MapImprintType.PLAYER_WAGON:
			case global::MapImprintType.ENEMY_WAGON:
				this.SelectCurrentWagon(imprint);
				goto IL_9C2;
			case global::MapImprintType.BEACON:
				goto IL_9C2;
			case global::MapImprintType.INTERACTIVE_POINT:
			case global::MapImprintType.WYRDSTONE:
				this.SelectCurrentInteractive(imprint.gameObject.GetComponent<global::InteractivePoint>());
				goto IL_9C2;
			case global::MapImprintType.TRAP:
				this.SelectCurrentTrap(imprint.Trap);
				goto IL_9C2;
			case global::MapImprintType.DESTRUCTIBLE:
				this.SelectCurrentDestructible(imprint.Destructible);
				goto IL_9C2;
			}
			this.currentSelectedIdx = -1;
			flag = true;
			flag2 = true;
		}
		IL_9C2:
		this.SetSelectedOverview(this.currentSelectedIdx);
		if (flag)
		{
			global::PandoraSingleton<global::UIMissionManager>.Instance.HideUnitStats();
		}
		if (flag2)
		{
			global::PandoraSingleton<global::UIMissionManager>.Instance.HidePropsInfo();
		}
		if (global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp("action", 6))
		{
			if (this.currentSelectedIdx != -1 && this.currentSelectedIdx < this.flyingOverviews.Count && this.flyingOverviews[this.currentSelectedIdx] != null && this.flyingOverviews[this.currentSelectedIdx].imprint != null && this.flyingOverviews[this.currentSelectedIdx].imprint.Beacon != null && this.flyingOverviews[this.currentSelectedIdx].imprint.Beacon.isActiveAndEnabled)
			{
				global::PandoraSingleton<global::MissionManager>.Instance.RemoveMapBecon(this.flyingOverviews[this.currentSelectedIdx].imprint.Beacon);
				this.RefreshUI();
			}
			else
			{
				global::PandoraSingleton<global::MissionManager>.Instance.SpawnMapBeacon(this.lookAtTarget.transform.position, new global::System.Action(this.RefreshUI));
			}
		}
	}

	private void RefreshUI()
	{
		global::PandoraSingleton<global::UIMissionManager>.Instance.overview.Refresh(this.zoomIdx);
	}

	private void SelectCurrentInteractive(global::InteractivePoint currentInteractive)
	{
		global::PandoraSingleton<global::UIMissionManager>.Instance.SetPropsInfo(currentInteractive.Imprint.visibleTexture, global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(currentInteractive.GetLocAction()));
	}

	private void SelectCurrentTrap(global::Trap currentTrap)
	{
		bool flag = global::PandoraSingleton<global::MissionManager>.Instance.GetMyWarbandCtrlr().teamIdx == currentTrap.TeamIdx;
		string key = "action_name_trap";
		if (!flag)
		{
			global::MapImprintType enemyImprintType = currentTrap.enemyImprintType;
			if (enemyImprintType != global::MapImprintType.INTERACTIVE_POINT)
			{
				if (enemyImprintType == global::MapImprintType.WYRDSTONE)
				{
					key = "action_name_gather_wyrdstone";
				}
			}
			else
			{
				key = "action_name_scavenge";
			}
		}
		global::PandoraSingleton<global::UIMissionManager>.Instance.SetPropsInfo(currentTrap.Imprint.visibleTexture, global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(key));
	}

	private void SelectCurrentWagon(global::MapImprint imprint)
	{
		global::PandoraSingleton<global::UIMissionManager>.Instance.SetPropsInfo(imprint.visibleTexture, imprint.Wagon.chest.warbandController.name);
	}

	private void SelectCurrentUnit(global::UnitController currentUnit)
	{
		global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<global::UnitController, bool>(global::Notices.LADDER_UNIT_CHANGED, currentUnit, true);
		if (currentUnit.IsPlayed())
		{
			global::PandoraSingleton<global::UIMissionManager>.Instance.CurrentUnitController = currentUnit;
			if (currentUnit.Engaged)
			{
				global::PandoraSingleton<global::UIMissionManager>.Instance.CurrentUnitTargetController = currentUnit.EngagedUnits[0];
				global::PandoraSingleton<global::UIMissionManager>.Instance.StateMachine.ChangeState(1);
			}
			else
			{
				global::PandoraSingleton<global::UIMissionManager>.Instance.StateMachine.ChangeState(0);
			}
		}
		else
		{
			global::PandoraSingleton<global::UIMissionManager>.Instance.CurrentUnitTargetController = currentUnit;
			if (currentUnit.Engaged)
			{
				global::PandoraSingleton<global::UIMissionManager>.Instance.CurrentUnitController = currentUnit.EngagedUnits[0];
				global::PandoraSingleton<global::UIMissionManager>.Instance.StateMachine.ChangeState(1);
			}
			else
			{
				global::PandoraSingleton<global::UIMissionManager>.Instance.unitCombatStats.OnDisable();
				global::PandoraSingleton<global::UIMissionManager>.Instance.StateMachine.ChangeState(1);
			}
		}
	}

	private void SelectCurrentDestructible(global::Destructible currentDestructible)
	{
		global::PandoraSingleton<global::UIMissionManager>.Instance.SetPropsInfo(currentDestructible.Imprint.visibleTexture, currentDestructible.LocalizedName);
	}

	private void EnterOrthoCam()
	{
		this.mngr.transitionCam = false;
		if (global::PandoraSingleton<global::MissionManager>.Instance.mapContour != null)
		{
			this.mapBounds = default(global::UnityEngine.Bounds);
			float num = 100f;
			for (int i = 0; i < global::PandoraSingleton<global::MissionManager>.Instance.mapContour.FlatEdges.Count; i++)
			{
				global::UnityEngine.Vector2 item = global::PandoraSingleton<global::MissionManager>.Instance.mapContour.FlatEdges[i].Item1;
				this.mapBounds.Encapsulate(new global::UnityEngine.Vector3(item.x, (float)(i % 2) * num, item.y));
			}
			this.mapBounds.SetMinMax(this.mapBounds.min, this.mapBounds.max + new global::UnityEngine.Vector3(0f, 0f, 15f));
		}
		this.mngr.SetDOFActive(false);
		this.cam.orthographic = true;
		this.cam.orthographicSize = (float)global::OverviewCamera.zoomLevels[this.zoomIdx];
		global::UnityEngine.Vector3 position = this.cam.transform.position + -this.cam.transform.forward * 40f;
		this.cam.transform.position = position;
		this.dummyCam.position = this.cam.transform.position;
	}

	private void ExitOrthoCam()
	{
		this.cam.orthographic = false;
		this.mngr.SetDOFActive(true);
		this.mngr.transitionCam = true;
	}

	public void SetTarget(global::UnityEngine.Transform target, bool immediate = false)
	{
		if (target)
		{
			this.lookAtTarget.rotation = target.rotation;
			this.SetTarget(target.position, false);
		}
	}

	public void SetTarget(global::UnityEngine.Vector3 targetPos, bool immediate = false)
	{
		global::PandoraSingleton<global::UIMissionManager>.Instance.HideUnitStats();
		this.lookAtTargetTargetPosition = targetPos;
		if (immediate)
		{
			this.lookAtTarget.transform.position = targetPos;
		}
		this.dummyCam.position = targetPos;
	}

	private void SetTargetZoom()
	{
		this.targetZoom = (float)global::OverviewCamera.zoomLevels[this.zoomIdx];
		this.oldZoom = this.currentZoom;
		this.RefreshUI();
	}

	private void SetTargetRotation()
	{
		this.targetRotation = (float)this.rotationIdx * this.rotationAngle;
		this.oldRotation = this.currentRotation;
	}

	public void GetNextPositionAngle(ref global::UnityEngine.Vector3 position, ref global::UnityEngine.Quaternion angle)
	{
		global::PandoraSingleton<global::MissionManager>.Instance.CamManager.RemoveLOSLayer("characters");
		global::UnityEngine.Vector3 a = this.OffsetPosition(this.lookAtTarget, new global::UnityEngine.Vector3(0f, 10f, -5f)) - this.OffsetPosition(this.lookAtTarget, new global::UnityEngine.Vector3(0f, 0f, -3f));
		a.Normalize();
		position = this.OffsetPosition(this.lookAtTarget, new global::UnityEngine.Vector3(0f, 0f, -3f)) + a * this.currentZoom;
		angle = global::UnityEngine.Quaternion.LookRotation(this.lookAtTarget.position + this.offsetVector - position);
	}

	private global::UnityEngine.Vector3 OffsetPosition(global::UnityEngine.Transform trans, global::UnityEngine.Vector3 offset)
	{
		global::UnityEngine.Vector3 a = trans.position;
		a += trans.forward * offset.z;
		a += trans.up * offset.y;
		return a + trans.right * offset.x;
	}

	public global::UnityEngine.Transform GetTarget()
	{
		return this.lookAtTarget;
	}

	public void SetSelectedOverview(int idx)
	{
		for (int i = 0; i < this.flyingOverviews.Count; i++)
		{
			if (this.flyingOverviews[i] != null && this.flyingOverviews[i].bgSelected.enabled != (i == idx))
			{
				this.flyingOverviews[i].bgSelected.enabled = (i == idx);
			}
		}
	}

	private const float fixedCameraHeight = 40f;

	[global::UnityEngine.HideInInspector]
	public global::UnityEngine.Transform lookAtTarget;

	private global::UnityEngine.Vector3 lookAtTargetTargetPosition;

	private static readonly int[] zoomLevels = new int[]
	{
		10,
		20,
		30,
		40
	};

	public int zoomIdx;

	public float zoomSpeed;

	private float currentZoom;

	private float targetZoom;

	private float oldZoom;

	public float rotationAngle;

	public int rotationIdx;

	public float rotationSpeed;

	private float currentRotation;

	private float targetRotation;

	private float oldRotation;

	private int rotationDir;

	private float moveSpeed = 20f;

	public float moveSpeedMin = 15f;

	public float moveSpeedMax = 40f;

	public float moveSpeedIncrease = 5f;

	private float increaseTime;

	private float fromHeight;

	private float toHeight;

	private global::UnityEngine.Bounds mapBounds;

	private int cyclingUnitIdx;

	private global::System.Collections.Generic.List<global::UnitController> cyclingUnits;

	private global::UnityEngine.Vector3 camDir = new global::UnityEngine.Vector3(0f, 1f, 0f);

	private global::UnityEngine.Vector3 offsetVector = new global::UnityEngine.Vector3(0f, 1.5f, 0f);

	private global::System.Collections.Generic.List<global::FlyingOverview> flyingOverviews = new global::System.Collections.Generic.List<global::FlyingOverview>();

	private int currentSelectedIdx = -1;

	private global::CameraManager mngr;

	private global::UnityEngine.Camera cam;

	private global::UnityEngine.Transform dummyCam;

	private global::System.Collections.Generic.List<global::UnityEngine.MonoBehaviour> tempGetComponentsList = new global::System.Collections.Generic.List<global::UnityEngine.MonoBehaviour>();

	private global::UnityEngine.RaycastHit[] raycastHits = new global::UnityEngine.RaycastHit[32];

	private bool active;
}
