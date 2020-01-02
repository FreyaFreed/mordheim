using System;
using System.Collections.Generic;

public class AchievementCategory
{
	public AchievementCategory(global::AchievementCategoryId catId)
	{
		this.category = catId;
		this.achievements = new global::System.Collections.Generic.List<global::Achievement>();
	}

	public string LocName
	{
		get
		{
			if (this.locName == null)
			{
				this.locName = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("task_title_" + this.category.ToLowerString());
			}
			return this.locName;
		}
	}

	public string LocDesc
	{
		get
		{
			if (this.locDesc == null)
			{
				this.locDesc = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("task_desc_" + this.category.ToLowerString());
			}
			return this.locDesc;
		}
	}

	public string LocDescShort
	{
		get
		{
			if (this.locDescShort == null)
			{
				this.locDescShort = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("task_desc_" + this.category.ToLowerString() + "_short");
			}
			return this.locDescShort;
		}
	}

	public int NbDone
	{
		get
		{
			int num = 0;
			for (int i = 0; i < this.achievements.Count; i++)
			{
				if (this.achievements[i].Completed)
				{
					num++;
				}
			}
			return num;
		}
	}

	public int Count
	{
		get
		{
			return this.achievements.Count;
		}
	}

	public global::AchievementCategoryId category;

	public global::System.Collections.Generic.List<global::Achievement> achievements;

	private string locName;

	private string locDesc;

	private string locDescShort;
}
