using System;
using System.Text;
using UnityEngine;
using WellFired;

[global::WellFired.USequencerEvent("Mordheim/PlayCamAnim")]
[global::WellFired.USequencerFriendlyName("PlayCamAnim")]
public class SeqPlayCamAnim : global::WellFired.USEventBase
{
	public override void FireEvent()
	{
		global::UnitController unitController = null;
		global::UnityEngine.Transform transform = null;
		global::UnityEngine.Transform transform2 = null;
		global::UnityEngine.Transform transform3 = null;
		switch (this.targetId)
		{
		case global::SequenceTargetId.FOCUSED_UNIT:
			unitController = global::PandoraSingleton<global::MissionManager>.Instance.focusedUnit;
			transform = unitController.transform;
			if (unitController.defenderCtrlr != null)
			{
				transform2 = unitController.defenderCtrlr.transform;
			}
			break;
		case global::SequenceTargetId.DEFENDER:
			unitController = global::PandoraSingleton<global::MissionManager>.Instance.focusedUnit.defenderCtrlr;
			transform = unitController.transform;
			break;
		case global::SequenceTargetId.ACTION_ZONE:
			unitController = global::PandoraSingleton<global::MissionManager>.Instance.focusedUnit;
			transform = unitController.interactivePoint.transform;
			transform3 = unitController.interactivePoint.cameraAnchor;
			break;
		case global::SequenceTargetId.ACTION_DEST:
			unitController = global::PandoraSingleton<global::MissionManager>.Instance.focusedUnit;
			transform = unitController.activeActionDest.destination.transform;
			break;
		}
		global::System.Text.StringBuilder stringBuilder = new global::System.Text.StringBuilder();
		if (this.raceBound)
		{
			string camBase = unitController.unit.BaseData.CamBase;
			stringBuilder.Append(camBase);
			stringBuilder.Append("_");
		}
		if (this.checkClaws && unitController.Equipments[(int)unitController.unit.ActiveWeaponSlot] != null && unitController.Equipments[(int)unitController.unit.ActiveWeaponSlot].Item.Id == global::ItemId.FIGHTING_CLAWS)
		{
			stringBuilder.Append("cl_");
		}
		if (this.unitSizeBound || (this.weaponSizeBound && unitController.unit.Data.UnitSizeId == global::UnitSizeId.LARGE))
		{
			global::UnitSizeData unitSizeData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::UnitSizeData>((int)unitController.unit.Data.UnitSizeId);
			string size = unitSizeData.Size;
			stringBuilder.Append(size);
			stringBuilder.Append("_");
		}
		else if (this.weaponSizeBound)
		{
			string size2 = unitController.Equipments[(int)unitController.unit.ActiveWeaponSlot].Item.StyleData.Size;
			stringBuilder.Append(size2);
			stringBuilder.Append("_");
		}
		stringBuilder.Append(this.clip);
		if (this.atkResultBound)
		{
			stringBuilder.Append((unitController.attackResultId != global::AttackResultId.PARRY) ? ((!unitController.criticalHit) ? "_success" : "_critical") : "_critical");
		}
		stringBuilder.Append("_cam");
		if (this.variations != 0)
		{
			int value = global::PandoraSingleton<global::GameManager>.Instance.LocalTyche.Rand(0, this.variations) + 1;
			stringBuilder.Append("_0");
			stringBuilder.Append(value);
		}
		else
		{
			stringBuilder.Append("_01");
		}
		string text = stringBuilder.ToString();
		global::UnitController unitController2 = global::PandoraSingleton<global::MissionManager>.Instance.OwnUnitInvolved(unitController, unitController.defenderCtrlr);
		if (unitController2 != null && unitController.defenderCtrlr == null && !text.Contains("search"))
		{
			if (transform != unitController2.transform)
			{
				global::PandoraSingleton<global::MissionManager>.Instance.CamManager.LookAtFocus(transform, false, true);
			}
			global::PandoraSingleton<global::MissionManager>.Instance.CamManager.SwitchToCam(global::CameraManager.CameraType.CHARACTER, unitController2.transform, true, true, transform == unitController2.transform, unitController2.unit.Data.UnitSizeId == global::UnitSizeId.LARGE);
		}
		else if (unitController2 != null && text.Contains("search"))
		{
			if (transform3 != null)
			{
				global::PandoraSingleton<global::MissionManager>.Instance.CamManager.LookAtFocus(transform, false, true);
				global::PandoraSingleton<global::MissionManager>.Instance.CamManager.SwitchToCam(global::CameraManager.CameraType.CONSTRAINED, unitController2.transform, true, true, false, unitController2.unit.Data.UnitSizeId == global::UnitSizeId.LARGE);
				global::ConstrainedCamera currentCam = global::PandoraSingleton<global::MissionManager>.Instance.CamManager.GetCurrentCam<global::ConstrainedCamera>();
				currentCam.SetOrigins(transform3);
			}
			else
			{
				global::PandoraSingleton<global::MissionManager>.Instance.CamManager.LookAtFocus(transform, false, true);
				global::PandoraSingleton<global::MissionManager>.Instance.CamManager.SwitchToCam(global::CameraManager.CameraType.SEMI_CONSTRAINED, unitController2.transform, true, true, false, unitController2.unit.Data.UnitSizeId == global::UnitSizeId.LARGE);
			}
		}
		else if (unitController2 != null)
		{
			global::PandoraSingleton<global::MissionManager>.Instance.CamManager.LookAtFocus((!(unitController2.transform == transform2)) ? transform2 : unitController.transform, false, true);
			global::PandoraSingleton<global::MissionManager>.Instance.CamManager.SwitchToCam(global::CameraManager.CameraType.SEMI_CONSTRAINED, unitController2.transform, true, true, false, unitController2.unit.Data.UnitSizeId == global::UnitSizeId.LARGE);
		}
		if (transform2 != null)
		{
			global::PandoraSingleton<global::MissionManager>.Instance.CamManager.AddLOSTarget(transform2);
		}
	}

	public override void ProcessEvent(float runningTime)
	{
	}

	public override void EndEvent()
	{
		base.EndEvent();
	}

	public string clip;

	public global::SequenceTargetId targetId;

	public global::CamAnimTypeId typeId;

	public bool raceBound;

	public bool weaponSizeBound;

	public bool unitSizeBound;

	public bool atkResultBound;

	public bool checkClaws;

	public int variations;
}
