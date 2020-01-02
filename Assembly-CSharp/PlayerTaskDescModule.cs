using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTaskDescModule : global::UIModule
{
	public void Set(global::AchievementCategory cat)
	{
		base.gameObject.SetActive(true);
		this.taskName.text = cat.LocName;
		this.taskDesc.text = cat.LocDesc;
		this.taskListSubtitle.text = cat.LocDescShort;
		int count = cat.achievements.Count;
		int num = 0;
		for (int i = 0; i < this.subTasks.Count; i++)
		{
			if (i < cat.achievements.Count)
			{
				this.subTasks[i].gameObject.SetActive(true);
				num += ((!cat.achievements[i].Completed) ? 0 : 1);
				this.subTasks[i].Set(cat.achievements[i]);
			}
			else
			{
				this.subTasks[i].gameObject.SetActive(false);
			}
		}
		this.progress.text = string.Format("{0}/{1}", num, count);
	}

	private const string PROGRESS = "{0}/{1}";

	public global::UnityEngine.UI.Text taskName;

	public global::UnityEngine.UI.Text taskDesc;

	public global::UnityEngine.UI.Text progress;

	public global::UnityEngine.UI.Text taskListSubtitle;

	public global::ListGroup list;

	public global::UnityEngine.GameObject prefab;

	public global::System.Collections.Generic.List<global::UISubTaskListItem> subTasks;
}
