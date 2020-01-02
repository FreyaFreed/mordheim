using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AvailableUnitsModule : global::UIModule
{
	public override void Init()
	{
		base.Init();
		this.scrollGroup.Setup(this.itemPrefab, true);
	}

	public void Set(global::System.Collections.Generic.List<global::UnitMenuController> units, bool canHire)
	{
		if (units.Count > 0)
		{
			this.listDesc.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById((!canHire) ? "hideout_unit_unavailable_desc" : "hideout_unit_available_desc");
		}
		else
		{
			global::EventLogger logger = global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.Logger;
			int currentDate = global::PandoraSingleton<global::HideoutManager>.Instance.Date.CurrentDate;
			global::Tuple<int, global::EventLogger.LogEvent, int> tuple = logger.FindEventAfter(global::EventLogger.LogEvent.OUTSIDER_ROTATION, currentDate);
			if (tuple != null)
			{
				global::Date date = new global::Date(tuple.Item1);
				this.listDesc.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_outsider_unavailable_desc", new string[]
				{
					date.ToLocalizedString()
				});
			}
			else
			{
				this.listDesc.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_outsider_unavailable_later_desc");
			}
		}
		this.scrollGroup.ClearList();
		for (int i = 0; i < units.Count; i++)
		{
			global::UnityEngine.GameObject gameObject = this.scrollGroup.AddToList(null, null);
			global::HireUnitDescription component = gameObject.GetComponent<global::HireUnitDescription>();
			component.Set(units[i].unit);
		}
	}

	private void Update()
	{
		float axis = global::PandoraSingleton<global::PandoraInput>.Instance.GetAxis("cam_y", 0);
		if (axis != 0f)
		{
			this.scrollGroup.ForceScroll(axis < 0f, false);
		}
	}

	public global::UnityEngine.GameObject itemPrefab;

	public global::UnityEngine.UI.Text listDesc;

	public global::ScrollGroup scrollGroup;
}
