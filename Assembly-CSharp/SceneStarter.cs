using System;
using UnityEngine;

public class SceneStarter : global::UnityEngine.MonoBehaviour
{
	private void Awake()
	{
		global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadAssetAsync<global::UnityEngine.GameObject>("Assets/prefabs/camera/", global::AssetBundleId.SCENE_PREFABS, "game_camera.prefab", delegate(global::UnityEngine.Object go)
		{
			global::UnityEngine.GameObject gameObject = (global::UnityEngine.GameObject)global::UnityEngine.Object.Instantiate(go);
			gameObject.name = "game_camera";
			global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadAssetAsync<global::UnityEngine.GameObject>("Assets/prefabs/systems/", global::AssetBundleId.SCENE_PREFABS, "game_systems.prefab", delegate(global::UnityEngine.Object go2)
			{
				global::UnityEngine.GameObject gameObject2 = (global::UnityEngine.GameObject)global::UnityEngine.Object.Instantiate(go2);
				gameObject2.name = "game_systems";
				global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadAssetAsync<global::UnityEngine.GameObject>("Assets/prefabs/gui/combat/", global::AssetBundleId.SCENE_PREFABS, "gui_mission.prefab", delegate(global::UnityEngine.Object go3)
				{
					global::UnityEngine.GameObject gameObject3 = (global::UnityEngine.GameObject)global::UnityEngine.Object.Instantiate(go3);
					gameObject3.name = "gui_mission";
				});
				global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadAssetAsync<global::UnityEngine.GameObject>("Assets/prefabs/gui/combat/", global::AssetBundleId.SCENE_PREFABS, "gui_player_retroaction.prefab", delegate(global::UnityEngine.Object go4)
				{
					global::UnityEngine.GameObject gameObject3 = (global::UnityEngine.GameObject)global::UnityEngine.Object.Instantiate(go4);
					gameObject3.name = "gui_retroaction";
				});
				global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadAssetAsync<global::UnityEngine.GameObject>("Assets/prefabs/gui/generic/", global::AssetBundleId.SCENE_PREFABS, "gui_tutorial_popup_large.prefab", delegate(global::UnityEngine.Object go5)
				{
					global::UnityEngine.GameObject gameObject3 = (global::UnityEngine.GameObject)global::UnityEngine.Object.Instantiate(go5);
					gameObject3.name = "gui_tutorial";
				});
			});
		});
		global::UnityEngine.Object.Destroy(base.gameObject);
	}

	private int loadedCount;
}
