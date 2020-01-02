using System;
using UnityEngine;

public class TestCampaigns : global::UnityEngine.MonoBehaviour
{
	private void OnGUI()
	{
		for (int i = 1; i <= this.maxMission; i++)
		{
			if (global::UnityEngine.GUI.Button(new global::UnityEngine.Rect((float)(global::UnityEngine.Screen.width - 230), (float)(global::UnityEngine.Screen.height - (this.maxMission + 1 - i) * 35), 220f, 22f), "Mission " + i))
			{
				this.LaunchMission(i);
			}
		}
	}

	private void LaunchMission(int idx)
	{
		if (!global::PandoraSingleton<global::MissionStartData>.Exists())
		{
			global::UnityEngine.GameObject gameObject = new global::UnityEngine.GameObject("mission_start_data");
			gameObject.AddComponent<global::MissionStartData>();
		}
		global::PandoraSingleton<global::MissionStartData>.Instance.ResetSeed();
		global::PandoraSingleton<global::PandoraInput>.Instance.SetActive(false);
		base.StartCoroutine(global::PandoraSingleton<global::MissionStartData>.Instance.SetMissionFull(global::Mission.GenerateCampaignMission(global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.Id, idx), global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr, delegate
		{
			global::SceneLauncher.Instance.LaunchScene(global::SceneLoadingId.LAUNCH_MISSION_CAMPAIGN, false, false);
		}));
	}

	private int maxMission = 8;
}
