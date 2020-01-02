using System;
using UnityEngine;
using UnityEngine.UI;

public class UISubTaskListItem : global::UnityEngine.MonoBehaviour
{
	public void Set(global::Achievement achievement)
	{
		this.taskName.text = achievement.LocDesc;
		this.xpGained.text = achievement.Xp.ToConstantString();
		this.isDoneToggle.isOn = achievement.Completed;
	}

	public global::UnityEngine.UI.Text taskName;

	public global::UnityEngine.UI.Text xpGained;

	public global::UnityEngine.UI.Toggle isDoneToggle;
}
