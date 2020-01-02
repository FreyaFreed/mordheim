using System;
using UnityEngine.UI;

public class NewCampaignLoadingView : global::LoadingView
{
	public override void Show()
	{
		base.Show();
		global::WarbandSave currentSave = global::PandoraSingleton<global::GameManager>.Instance.currentSave;
		string str = ((global::WarbandId)currentSave.id).ToString();
		this.titleText.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("com_wb_type_" + str);
		this.descriptionText.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("main_camp_intro_" + str);
		this.flag.sprite = global::Warband.GetIcon((global::WarbandId)currentSave.id);
		base.LoadBackground("bg_warband_" + str);
		base.LoadDialog("main_camp_intro_" + str);
	}

	public global::UnityEngine.UI.Text titleText;

	public global::UnityEngine.UI.Text descriptionText;

	public global::UnityEngine.UI.Image flag;
}
