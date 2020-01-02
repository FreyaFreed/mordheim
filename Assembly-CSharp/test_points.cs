using System;
using UnityEngine;

public class test_points : global::UnityEngine.MonoBehaviour
{
	private void OnGUI()
	{
		global::UnitController currentUnit = global::PandoraSingleton<global::MissionManager>.Instance.GetCurrentUnit();
		if (currentUnit != null)
		{
			global::UnityEngine.GUI.Button(new global::UnityEngine.Rect(0f, 0f, 200f, 50f), "Strat Points = " + currentUnit.unit.CurrentStrategyPoints);
			global::UnityEngine.GUI.Button(new global::UnityEngine.Rect(0f, 50f, 200f, 50f), "Off Points = " + currentUnit.unit.CurrentOffensePoints);
			global::UnityEngine.GUI.Button(new global::UnityEngine.Rect(0f, 100f, 200f, 50f), "Temp Strat Points = " + currentUnit.unit.tempStrategyPoints);
			global::UnityEngine.GUI.Button(new global::UnityEngine.Rect(0f, 150f, 200f, 50f), "Temp Off Points = " + currentUnit.unit.tempOffensePoints);
		}
	}
}
