using System;
using System.Collections.Generic;
using UnityEngine;

public class test_unit_spawner : global::UnityEngine.MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
		if (global::UnityEngine.Input.GetKeyDown(global::UnityEngine.KeyCode.F12))
		{
			global::UnitController currentUnit = global::PandoraSingleton<global::MissionManager>.Instance.GetCurrentUnit();
			global::UnityEngine.Debug.Log(currentUnit.unit.GetRating());
			base.StartCoroutine(global::PandoraSingleton<global::UnitFactory>.Instance.CloneUnitCtrlr(currentUnit, currentUnit.unit.Rank, currentUnit.unit.GetRating(), global::UnityEngine.Vector3.zero, global::UnityEngine.Quaternion.identity));
			global::System.Collections.Generic.List<global::UnitController> allUnitsList = global::PandoraSingleton<global::MissionManager>.Instance.allUnitsList;
			for (int i = 0; i < allUnitsList.Count; i++)
			{
				allUnitsList[i].InitTargetsData();
			}
		}
	}
}
