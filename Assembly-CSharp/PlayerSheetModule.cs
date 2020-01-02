using System;
using UnityEngine.UI;

public class PlayerSheetModule : global::UIModule
{
	public void Refresh()
	{
		this.warbandName.text = global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.GetWarbandSave().Name;
		this.rank.text = global::PandoraSingleton<global::GameManager>.Instance.Profile.Rank.ToString();
		if (global::PandoraSingleton<global::GameManager>.Instance.Profile.RankData.XpNeeded > 0 && global::PandoraSingleton<global::GameManager>.Instance.Profile.HasNextRank)
		{
			this.xp.fillRect.gameObject.SetActive((float)global::PandoraSingleton<global::GameManager>.Instance.Profile.CurrentXp > 0f);
			this.xp.normalizedValue = (float)global::PandoraSingleton<global::GameManager>.Instance.Profile.CurrentXp / (float)global::PandoraSingleton<global::GameManager>.Instance.Profile.RankData.XpNeeded;
			this.xpValue.text = global::PandoraSingleton<global::GameManager>.Instance.Profile.CurrentXp + " / " + global::PandoraSingleton<global::GameManager>.Instance.Profile.RankData.XpNeeded;
		}
		else
		{
			this.xp.fillRect.gameObject.SetActive(true);
			this.xp.normalizedValue = 1f;
			this.xpValue.text = string.Empty;
		}
	}

	public global::UnityEngine.UI.Text warbandName;

	public global::UnityEngine.UI.Text rank;

	public global::UnityEngine.UI.Slider xp;

	public global::UnityEngine.UI.Text xpValue;
}
