using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotificationModule : global::UIModule
{
	private new void Awake()
	{
		global::PandoraSingleton<global::NoticeManager>.Instance.RegisterListener(global::Notices.PROFILE_RANK_UP, new global::DelReceiveNotice(this.RankUp));
		global::PandoraSingleton<global::NoticeManager>.Instance.RegisterListener(global::Notices.TASK_COMPLETED, new global::DelReceiveNotice(this.TaskCompleted));
		this.notifications = new global::System.Collections.Generic.List<global::UINotification>();
	}

	private void Update()
	{
		this.notif.SetActive(this.notifications.Count > 0);
		if (this.notifications.Count > 0)
		{
			if (!this.notifications[0].activated)
			{
				this.title.text = this.notifications[0].title;
				this.desc.text = this.notifications[0].desc;
				global::NotificationType type = this.notifications[0].type;
				if (type != global::NotificationType.RANK)
				{
					if (type == global::NotificationType.TASK)
					{
						this.rank.SetActive(false);
						this.task.SetActive(true);
						this.task1.text = this.notifications[0].text1;
						this.task2.text = this.notifications[0].text2;
					}
				}
				else
				{
					this.rank.SetActive(true);
					this.rankPoints.text = this.notifications[0].text1;
					this.task.SetActive(false);
				}
			}
			this.timer += global::UnityEngine.Time.deltaTime;
			if (this.timer >= this.duration)
			{
				this.notifications.RemoveAt(0);
				this.timer = 0f;
			}
		}
	}

	private void RankUp()
	{
		global::PlayerRankId playerRankId = (global::PlayerRankId)((int)global::PandoraSingleton<global::NoticeManager>.Instance.Parameters[0]);
		int num = 0;
		global::PlayerRankData playerRankData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::PlayerRankData>((int)playerRankId);
		global::WarbandSkill warbandSkill = new global::WarbandSkill(playerRankData.WarbandSkillId);
		for (int i = 0; i < warbandSkill.Enchantments.Count; i++)
		{
			for (int j = 0; j < warbandSkill.Enchantments[i].Attributes.Count; j++)
			{
				if (warbandSkill.Enchantments[i].Attributes[j].WarbandAttributeId == global::WarbandAttributeId.PLAYER_SKILL_POINTS)
				{
					num += warbandSkill.Enchantments[i].Attributes[j].Modifier;
				}
			}
		}
		string stringById = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("player_rank_up_title");
		global::LocalizationManager instance = global::PandoraSingleton<global::LocalizationManager>.Instance;
		string key = "player_rank_up_desc";
		string[] array = new string[1];
		int num2 = 0;
		int num3 = (int)playerRankId;
		array[num2] = num3.ToString();
		this.AddNotification(stringById, instance.GetStringById(key, array), global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("player_rank_up_gained_points", new string[]
		{
			num.ToString()
		}), string.Empty);
	}

	private void TaskCompleted()
	{
		global::AchievementId id = (global::AchievementId)((int)global::PandoraSingleton<global::NoticeManager>.Instance.Parameters[0]);
		global::AchievementData achievementData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::AchievementData>((int)id);
		this.AddNotification(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("task_completed_title"), global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("task_completed_desc"), global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("task_title_" + achievementData.AchievementCategoryId.ToLowerString()), global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("task_xp", new string[]
		{
			achievementData.Xp.ToString()
		}));
	}

	private void AddNotification(string title, string desc, string text1 = "", string text2 = "")
	{
		global::UINotification uinotification = new global::UINotification();
		uinotification.title = title;
		uinotification.desc = desc;
		uinotification.text1 = text1;
		uinotification.text2 = text2;
		this.notifications.Add(uinotification);
	}

	public global::UnityEngine.GameObject notif;

	public global::UnityEngine.UI.Text title;

	public global::UnityEngine.UI.Text desc;

	public global::UnityEngine.GameObject rank;

	public global::UnityEngine.UI.Text rankPoints;

	public global::UnityEngine.GameObject task;

	public global::UnityEngine.UI.Text task1;

	public global::UnityEngine.UI.Text task2;

	public float duration = 5f;

	private global::System.Collections.Generic.List<global::UINotification> notifications;

	private float timer;
}
