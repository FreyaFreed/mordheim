using System;
using UnityEngine.UI;

public class PlayerRankDescModule : global::UIModule
{
	public void Refresh()
	{
		this.currentPerks.text = global::PandoraSingleton<global::GameManager>.Instance.Profile.GetCurrentRankDescription();
		if (global::PandoraSingleton<global::GameManager>.Instance.Profile.RankData.XpNeeded > 0)
		{
			this.nextRankBonus.text = global::PandoraSingleton<global::GameManager>.Instance.Profile.GetNextRankDescription();
		}
		else
		{
			this.nextRankBonus.text = string.Empty;
		}
	}

	public global::UnityEngine.UI.Text nextRankBonus;

	public global::UnityEngine.UI.Text currentPerks;
}
