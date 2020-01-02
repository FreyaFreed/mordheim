using System;
using UnityEngine;

public class RackLoadoutNode : global::UnityEngine.MonoBehaviour
{
	public void SetLoadout(string warbandWagon)
	{
		if (this.loadout != null)
		{
			return;
		}
		string str = warbandWagon.Replace("wagon_dis_00", "props_dis_00_weapon_rack_loadout");
		global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadAssetAsync<global::UnityEngine.GameObject>("Assets/prefabs/environments/props/", global::AssetBundleId.PROPS, str + ".prefab", delegate(global::UnityEngine.Object p)
		{
			this.loadout = (global::UnityEngine.GameObject)global::UnityEngine.Object.Instantiate(p);
			this.loadout.transform.SetParent(base.transform);
			this.loadout.transform.localPosition = global::UnityEngine.Vector3.zero;
			this.loadout.transform.localRotation = global::UnityEngine.Quaternion.identity;
		});
	}

	private global::UnityEngine.GameObject loadout;
}
