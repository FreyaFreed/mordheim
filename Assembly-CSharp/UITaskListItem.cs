using System;
using UnityEngine;
using UnityEngine.UI;

public class UITaskListItem : global::UnityEngine.MonoBehaviour
{
	public void Set(global::AchievementCategory category)
	{
		this.groupName.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("task_title_" + category.category);
		int nbDone = category.NbDone;
		this.isDoneToggle.enabled = (nbDone == category.Count);
		this.strikeBar.enabled = this.isDoneToggle.enabled;
		this.doneText.text = string.Format("{0}/{1}", nbDone, category.Count);
	}

	public global::UnityEngine.UI.Text groupName;

	public global::UnityEngine.UI.Image isDoneToggle;

	public global::UnityEngine.UI.Text doneText;

	public global::UnityEngine.UI.Image strikeBar;
}
