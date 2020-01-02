using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SmugglerFactionBonusModule : global::UIModule
{
	public void Setup(global::FactionMenuController faction)
	{
		this.factionType.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(faction.Faction.Data.Desc + "_name");
		for (int i = 0; i < this.bonus.Count; i++)
		{
			int num = i + 1;
			this.bonus[i].SetLocalized((num > faction.Faction.Save.rank) ? this.locked : this.unlocked, global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(string.Concat(new object[]
			{
				"faction_rank_",
				faction.Faction.Data.Id,
				"_",
				num
			})));
		}
		this.scrollGroup.scrollbar.value = 1f;
	}

	private void Update()
	{
		if (global::PandoraSingleton<global::PandoraInput>.Instance.CurrentInputLayer == 0)
		{
			float axis = global::PandoraSingleton<global::PandoraInput>.Instance.GetAxis("cam_y", 0);
			if (!global::UnityEngine.Mathf.Approximately(axis, 0f) && global::UnityEngine.Mathf.Abs(axis) > 0.8f)
			{
				this.scrollGroup.ForceScroll(axis < 0f, false);
			}
		}
	}

	public global::UnityEngine.Sprite locked;

	public global::UnityEngine.Sprite unlocked;

	public global::UnityEngine.UI.Text factionType;

	public global::System.Collections.Generic.List<global::UIIconDesc> bonus;

	public global::ScrollGroup scrollGroup;
}
