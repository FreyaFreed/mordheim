using System;
using UnityEngine.UI;

public class WarbandSheetModule : global::UIModule
{
	public void Set(global::Warband warband)
	{
		this.rank.text = warband.Rank.ToString();
		this.warbandType.text = global::Warband.GetLocalizedName(warband.Id);
		this.warbandName.text = warband.GetWarbandSave().Name;
		this.rating.text = warband.GetRating().ToString();
		this.upkeep.text = warband.GetTotalUpkeepOwned().ToString();
		if (warband.Rank < warband.GetMaxRank())
		{
			global::WarbandRankData nextWarbandRankData = warband.GetNextWarbandRankData();
			this.xpBar.fillRect.gameObject.SetActive(warband.Xp > 0);
			this.xpBar.value = (float)warband.Xp / (float)nextWarbandRankData.Exp;
			this.xpValue.enabled = true;
			this.xpValue.text = warband.Xp + " / " + nextWarbandRankData.Exp;
		}
		else
		{
			this.xpBar.fillRect.gameObject.SetActive(true);
			this.xpBar.value = 1f;
			this.xpValue.enabled = false;
		}
		this.units.text = string.Format("{0} / {1}", warband.GetNbActiveUnits(true), warband.GetNbMaxActiveSlots());
		this.reserve.text = string.Format("{0} / {1}", warband.GetNbReserveUnits(), warband.GetNbMaxReserveSlot());
		this.unavailable.text = warband.GetNbInactiveUnits(true).ToString();
		this.warbandIcon.sprite = global::Warband.GetIcon(warband.Id);
	}

	private const string UNITS = "{0} / {1}";

	public global::UnityEngine.UI.Text rank;

	public global::UnityEngine.UI.Text warbandName;

	public global::UnityEngine.UI.Text warbandType;

	public global::UnityEngine.UI.Text rating;

	public global::UnityEngine.UI.Text upkeep;

	public global::UnityEngine.UI.Slider xpBar;

	public global::UnityEngine.UI.Text xpValue;

	public global::UnityEngine.UI.Text units;

	public global::UnityEngine.UI.Text reserve;

	public global::UnityEngine.UI.Text unavailable;

	public global::UnityEngine.UI.Image warbandIcon;
}
