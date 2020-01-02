using System;
using System.Collections.Generic;
using UnityEngine;

public class UITabLoader : global::UnityEngine.MonoBehaviour
{
	public void Load(global::System.Collections.Generic.Dictionary<global::ModuleId, global::UIModule> tab)
	{
		for (int i = 0; i < this.modules.Count; i++)
		{
			this.modulesGO.Add(null);
			global::PandoraSingleton<global::HideoutManager>.Instance.tabsLoading++;
			int index = i;
			global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadAssetAsync<global::UnityEngine.GameObject>("Assets/prefabs/gui/hideout/", global::AssetBundleId.SCENE_PREFABS, this.modules[i] + ".prefab", delegate(global::UnityEngine.Object go)
			{
				global::PandoraSingleton<global::HideoutManager>.Instance.tabsLoading--;
				global::UnityEngine.GameObject gameObject = (global::UnityEngine.GameObject)go;
				gameObject = global::UnityEngine.Object.Instantiate<global::UnityEngine.GameObject>(gameObject);
				this.modulesGO[index] = gameObject;
				global::UIModule component = gameObject.GetComponent<global::UIModule>();
				tab.Add(component.moduleId, component);
			});
		}
	}

	public void ParentModules()
	{
		for (int i = 0; i < this.modulesGO.Count; i++)
		{
			this.modulesGO[i].transform.SetParent(this.anchor, false);
		}
		global::UnityEngine.Object.Destroy(this);
	}

	public global::UnityEngine.RectTransform anchor;

	public global::System.Collections.Generic.List<string> modules = new global::System.Collections.Generic.List<string>();

	private global::System.Collections.Generic.List<global::UnityEngine.GameObject> modulesGO = new global::System.Collections.Generic.List<global::UnityEngine.GameObject>();
}
