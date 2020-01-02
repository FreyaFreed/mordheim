using System;
using Steamworks;
using UnityEngine;

internal class StatsAndAchievements
{
	private void OnEnable()
	{
		if (global::StatsAndAchievements.m_instance == null)
		{
			global::StatsAndAchievements.m_instance = this;
		}
		this.m_GameID = new global::Steamworks.CGameID(global::Steamworks.SteamUtils.GetAppID());
		new global::Steamworks.Callback<global::Steamworks.UserStatsReceived_t>(new global::Steamworks.Callback<global::Steamworks.UserStatsReceived_t>.DispatchDelegate(this.OnUserStatsReceived), false);
		new global::Steamworks.Callback<global::Steamworks.UserStatsStored_t>(new global::Steamworks.Callback<global::Steamworks.UserStatsStored_t>.DispatchDelegate(this.OnUserStatsStored), false);
		new global::Steamworks.Callback<global::Steamworks.UserAchievementStored_t>(new global::Steamworks.Callback<global::Steamworks.UserAchievementStored_t>.DispatchDelegate(this.OnAchievementStored), false);
	}

	private void FixedUpdate()
	{
		if (!this.m_bRequestedStats)
		{
			this.m_bRequestedStats = true;
			return;
		}
		if (!this.m_bStatsValid)
		{
			return;
		}
		foreach (global::StatsAndAchievements.Achievement_t achievement in this.m_Achievements)
		{
			this.EvaluateAchievement(achievement);
		}
		this.StoreStatsIfNecessary();
	}

	public void AddDistanceTraveled(float flDistance)
	{
		this.m_flGameFeetTraveled += flDistance / 72f / 12f;
	}

	public void OnGameStateChange(global::EClientGameState eNewState)
	{
		if (!this.m_bStatsValid)
		{
			return;
		}
		switch (eNewState)
		{
		default:
			return;
		case global::EClientGameState.k_EClientGameActive:
			this.m_flGameFeetTraveled = 0f;
			this.m_ulTickCountGameStart = global::UnityEngine.Time.time;
			return;
		case global::EClientGameState.k_EClientGameDraw:
			break;
		case global::EClientGameState.k_EClientGameWinner:
			this.m_nTotalNumWins++;
			this.m_nTotalNumLosses++;
			break;
		case global::EClientGameState.k_EClientFindInternetServers:
			return;
		}
		this.m_nTotalGamesPlayed++;
		this.m_flTotalFeetTraveled += this.m_flGameFeetTraveled;
		if (this.m_flGameFeetTraveled > this.m_flMaxFeetTraveled)
		{
			this.m_flMaxFeetTraveled = this.m_flGameFeetTraveled;
		}
		this.m_flGameDurationSeconds = (double)(global::UnityEngine.Time.time - this.m_ulTickCountGameStart);
		this.m_bStoreStats = true;
	}

	private void EvaluateAchievement(global::StatsAndAchievements.Achievement_t achievement)
	{
		if (achievement.m_bAchieved)
		{
			return;
		}
		switch (achievement.m_eAchievementID)
		{
		case global::StatsAndAchievements.Achievement.ACH_WIN_ONE_GAME:
			if (this.m_nTotalNumWins != 0)
			{
				this.UnlockAchievement(achievement);
			}
			break;
		case global::StatsAndAchievements.Achievement.ACH_WIN_100_GAMES:
			if (this.m_nTotalNumWins >= 100)
			{
				this.UnlockAchievement(achievement);
			}
			break;
		case global::StatsAndAchievements.Achievement.ACH_TRAVEL_FAR_ACCUM:
			if (this.m_flTotalFeetTraveled >= 5280f)
			{
				this.UnlockAchievement(achievement);
			}
			break;
		case global::StatsAndAchievements.Achievement.ACH_TRAVEL_FAR_SINGLE:
			if (this.m_flGameFeetTraveled > 500f)
			{
				this.UnlockAchievement(achievement);
			}
			break;
		}
	}

	private void UnlockAchievement(global::StatsAndAchievements.Achievement_t achievement)
	{
		achievement.m_bAchieved = true;
		achievement.m_iIconImage = 0;
		global::Steamworks.SteamUserStats.SetAchievement(achievement.m_eAchievementID.ToString());
		this.m_bStoreStats = true;
	}

	private void StoreStatsIfNecessary()
	{
		if (this.m_bStoreStats)
		{
			global::Steamworks.SteamUserStats.SetStat("NumGames", this.m_nTotalGamesPlayed);
			global::Steamworks.SteamUserStats.SetStat("NumWins", this.m_nTotalNumWins);
			global::Steamworks.SteamUserStats.SetStat("NumLosses", this.m_nTotalNumLosses);
			global::Steamworks.SteamUserStats.SetStat("FeetTraveled", this.m_flTotalFeetTraveled);
			global::Steamworks.SteamUserStats.SetStat("MaxFeetTraveled", this.m_flMaxFeetTraveled);
			global::Steamworks.SteamUserStats.UpdateAvgRateStat("AverageSpeed", this.m_flGameFeetTraveled, this.m_flGameDurationSeconds);
			global::Steamworks.SteamUserStats.GetStat("AverageSpeed", out this.m_flAverageSpeed);
			bool flag = global::Steamworks.SteamUserStats.StoreStats();
			this.m_bStoreStats = !flag;
		}
	}

	private void OnUserStatsReceived(global::Steamworks.UserStatsReceived_t pCallback)
	{
		if ((ulong)this.m_GameID == pCallback.m_nGameID)
		{
			if (pCallback.m_eResult == global::Steamworks.EResult.k_EResultOK)
			{
				global::UnityEngine.Debug.Log("Received stats and achievements from Steam\n");
				this.m_bStatsValid = true;
				foreach (global::StatsAndAchievements.Achievement_t achievement_t in this.m_Achievements)
				{
					global::Steamworks.SteamUserStats.GetAchievement(achievement_t.m_eAchievementID.ToString(), out achievement_t.m_bAchieved);
					achievement_t.m_rgchName = global::Steamworks.SteamUserStats.GetAchievementDisplayAttribute(achievement_t.m_eAchievementID.ToString(), "name");
					achievement_t.m_rgchDescription = global::Steamworks.SteamUserStats.GetAchievementDisplayAttribute(achievement_t.m_eAchievementID.ToString(), "desc");
				}
				global::Steamworks.SteamUserStats.GetStat("NumGames", out this.m_nTotalGamesPlayed);
				global::Steamworks.SteamUserStats.GetStat("NumWins", out this.m_nTotalNumWins);
				global::Steamworks.SteamUserStats.GetStat("NumLosses", out this.m_nTotalNumLosses);
				global::Steamworks.SteamUserStats.GetStat("FeetTraveled", out this.m_flTotalFeetTraveled);
				global::Steamworks.SteamUserStats.GetStat("MaxFeetTraveled", out this.m_flMaxFeetTraveled);
				global::Steamworks.SteamUserStats.GetStat("AverageSpeed", out this.m_flAverageSpeed);
			}
			else
			{
				global::UnityEngine.Debug.Log("RequestStats - failed, " + pCallback.m_eResult);
			}
		}
	}

	private void OnUserStatsStored(global::Steamworks.UserStatsStored_t pCallback)
	{
		if ((ulong)this.m_GameID == pCallback.m_nGameID)
		{
			if (pCallback.m_eResult == global::Steamworks.EResult.k_EResultOK)
			{
				global::UnityEngine.Debug.Log("StoreStats - success");
			}
			else if (pCallback.m_eResult == global::Steamworks.EResult.k_EResultInvalidParam)
			{
				global::UnityEngine.Debug.Log("StoreStats - some failed to validate");
				this.OnUserStatsReceived(new global::Steamworks.UserStatsReceived_t
				{
					m_eResult = global::Steamworks.EResult.k_EResultOK,
					m_nGameID = (ulong)this.m_GameID
				});
			}
			else
			{
				global::UnityEngine.Debug.Log("StoreStats - failed, " + pCallback.m_eResult);
			}
		}
	}

	private void OnAchievementStored(global::Steamworks.UserAchievementStored_t pCallback)
	{
		if ((ulong)this.m_GameID == pCallback.m_nGameID)
		{
			if (pCallback.m_nMaxProgress == 0U)
			{
				global::UnityEngine.Debug.Log("Achievement '" + pCallback.m_rgchAchievementName + "' unlocked!");
			}
			else
			{
				global::UnityEngine.Debug.Log(string.Concat(new object[]
				{
					"Achievement '",
					pCallback.m_rgchAchievementName,
					"' progress callback, (",
					pCallback.m_nCurProgress,
					",",
					pCallback.m_nMaxProgress,
					")"
				}));
			}
		}
	}

	public void Render()
	{
		global::UnityEngine.GUILayout.Label("m_ulTickCountGameStart: " + this.m_ulTickCountGameStart, new global::UnityEngine.GUILayoutOption[0]);
		global::UnityEngine.GUILayout.Label("m_flGameDurationSeconds: " + this.m_flGameDurationSeconds, new global::UnityEngine.GUILayoutOption[0]);
		global::UnityEngine.GUILayout.Label("m_flGameFeetTraveled: " + this.m_flGameFeetTraveled, new global::UnityEngine.GUILayoutOption[0]);
		global::UnityEngine.GUILayout.Space(10f);
		global::UnityEngine.GUILayout.Label("NumGames: " + this.m_nTotalGamesPlayed, new global::UnityEngine.GUILayoutOption[0]);
		global::UnityEngine.GUILayout.Label("NumWins: " + this.m_nTotalNumWins, new global::UnityEngine.GUILayoutOption[0]);
		global::UnityEngine.GUILayout.Label("NumLosses: " + this.m_nTotalNumLosses, new global::UnityEngine.GUILayoutOption[0]);
		global::UnityEngine.GUILayout.Label("FeetTraveled: " + this.m_flTotalFeetTraveled, new global::UnityEngine.GUILayoutOption[0]);
		global::UnityEngine.GUILayout.Label("MaxFeetTraveled: " + this.m_flMaxFeetTraveled, new global::UnityEngine.GUILayoutOption[0]);
		global::UnityEngine.GUILayout.Label("AverageSpeed: " + this.m_flAverageSpeed, new global::UnityEngine.GUILayoutOption[0]);
		global::UnityEngine.GUILayout.BeginArea(new global::UnityEngine.Rect((float)(global::UnityEngine.Screen.width - 300), 0f, 300f, 800f));
		foreach (global::StatsAndAchievements.Achievement_t achievement_t in this.m_Achievements)
		{
			global::UnityEngine.GUILayout.Label(achievement_t.m_eAchievementID.ToString(), new global::UnityEngine.GUILayoutOption[0]);
			global::UnityEngine.GUILayout.Label(achievement_t.m_rgchName + " - " + achievement_t.m_rgchDescription, new global::UnityEngine.GUILayoutOption[0]);
			global::UnityEngine.GUILayout.Label("Achieved: " + achievement_t.m_bAchieved, new global::UnityEngine.GUILayoutOption[0]);
			global::UnityEngine.GUILayout.Space(20f);
		}
		if (global::UnityEngine.GUILayout.Button("RESET STATS AND ACHIEVEMENTS", new global::UnityEngine.GUILayoutOption[0]))
		{
			global::Steamworks.SteamUserStats.ResetAllStats(true);
			global::Steamworks.SteamUserStats.RequestCurrentStats();
			this.OnGameStateChange(global::EClientGameState.k_EClientGameMenu);
		}
		global::UnityEngine.GUILayout.EndArea();
	}

	private global::StatsAndAchievements.Achievement_t[] m_Achievements = new global::StatsAndAchievements.Achievement_t[]
	{
		new global::StatsAndAchievements.Achievement_t(global::StatsAndAchievements.Achievement.ACH_WIN_ONE_GAME, "Winner", string.Empty, false, 0),
		new global::StatsAndAchievements.Achievement_t(global::StatsAndAchievements.Achievement.ACH_WIN_100_GAMES, "Champion", string.Empty, false, 0),
		new global::StatsAndAchievements.Achievement_t(global::StatsAndAchievements.Achievement.ACH_TRAVEL_FAR_ACCUM, "Interstellar", string.Empty, false, 0),
		new global::StatsAndAchievements.Achievement_t(global::StatsAndAchievements.Achievement.ACH_TRAVEL_FAR_SINGLE, "Orbiter", string.Empty, false, 0)
	};

	private static global::StatsAndAchievements m_instance;

	private global::Steamworks.CGameID m_GameID;

	private bool m_bRequestedStats;

	private bool m_bStatsValid;

	private bool m_bStoreStats;

	private float m_flGameFeetTraveled;

	private float m_ulTickCountGameStart;

	private double m_flGameDurationSeconds;

	private int m_nTotalGamesPlayed;

	private int m_nTotalNumWins;

	private int m_nTotalNumLosses;

	private float m_flTotalFeetTraveled;

	private float m_flMaxFeetTraveled;

	private float m_flAverageSpeed;

	private enum Achievement
	{
		ACH_WIN_ONE_GAME,
		ACH_WIN_100_GAMES,
		ACH_HEAVY_FIRE,
		ACH_TRAVEL_FAR_ACCUM,
		ACH_TRAVEL_FAR_SINGLE
	}

	private class Achievement_t
	{
		public Achievement_t(global::StatsAndAchievements.Achievement achievement, string name, string desc, bool achieved, int icon)
		{
			this.m_eAchievementID = achievement;
			this.m_rgchName = name;
			this.m_rgchDescription = desc;
			this.m_bAchieved = achieved;
			this.m_iIconImage = icon;
		}

		public global::StatsAndAchievements.Achievement m_eAchievementID;

		public string m_rgchName;

		public string m_rgchDescription;

		public bool m_bAchieved;

		public int m_iIconImage;
	}
}
