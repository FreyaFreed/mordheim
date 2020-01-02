using System;
using UnityEngine;
using UnityEngine.UI;

public class WarbandRankBonusPreviewModule : global::UIModule
{
	public void Set(global::Warband wb)
	{
		this.nextRankBonusField.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_warband_adv_next_rank_" + (wb.Rank + 1).ToString());
		this.currentRankBonusField.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_warband_adv_current_rank_" + wb.Rank.ToString());
	}

	[global::UnityEngine.SerializeField]
	private global::UnityEngine.UI.Text nextRankBonusField;

	[global::UnityEngine.SerializeField]
	private global::UnityEngine.UI.Text currentRankBonusField;
}
