using System;

public class SceneLauncher
{
	public static global::SceneLauncher Instance
	{
		get
		{
			if (global::SceneLauncher.instance == null)
			{
				global::SceneLauncher.instance = new global::SceneLauncher();
			}
			return global::SceneLauncher.instance;
		}
	}

	public void LaunchScene(global::SceneLoadingId id, bool waitForPlayers = false, bool force = false)
	{
		global::SceneLoadingData sceneLoadingData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::SceneLoadingData>((int)id);
		global::PandoraSingleton<global::Hermes>.Instance.SendLoadLevel(sceneLoadingData.NextScene, sceneLoadingData.SceneLoadingTypeId, (float)sceneLoadingData.TransitionDuration, sceneLoadingData.WaitAction, waitForPlayers, force);
	}

	private static global::SceneLauncher instance;
}
