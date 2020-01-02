using System;
using UnityEngine;

public class test_mergeweapon : global::UnityEngine.MonoBehaviour
{
	private void Start()
	{
	}

	public void AddEquipment()
	{
		this.isUsed = true;
		global::UnityEngine.SkinnedMeshRenderer[] componentsInChildren = this.DaWeapon.GetComponentsInChildren<global::UnityEngine.SkinnedMeshRenderer>();
		foreach (global::UnityEngine.SkinnedMeshRenderer thisRenderer in componentsInChildren)
		{
			this.ProcessBonedObject(thisRenderer);
		}
		this.DaWeapon.SetActive(false);
	}

	public void RemoveEquipment()
	{
		this.isUsed = false;
		global::UnityEngine.Object.Destroy(this.DaWeaponOnceUsed);
		this.DaWeapon.SetActive(true);
	}

	public void ProcessBonedObject(global::UnityEngine.SkinnedMeshRenderer ThisRenderer)
	{
		this.DaWeaponOnceUsed = new global::UnityEngine.GameObject(ThisRenderer.gameObject.name);
		this.DaWeaponOnceUsed.transform.parent = base.transform;
		global::UnityEngine.SkinnedMeshRenderer skinnedMeshRenderer = this.DaWeaponOnceUsed.AddComponent(typeof(global::UnityEngine.SkinnedMeshRenderer)) as global::UnityEngine.SkinnedMeshRenderer;
		global::UnityEngine.Transform[] array = new global::UnityEngine.Transform[ThisRenderer.bones.Length];
		for (int i = 0; i < ThisRenderer.bones.Length; i++)
		{
			array[i] = this.FindChildByName(ThisRenderer.bones[i].name, base.transform);
		}
		skinnedMeshRenderer.bones = array;
		skinnedMeshRenderer.sharedMesh = ThisRenderer.sharedMesh;
		skinnedMeshRenderer.materials = ThisRenderer.materials;
	}

	public global::UnityEngine.Transform FindChildByName(string ThisName, global::UnityEngine.Transform ThisGObj)
	{
		if (ThisGObj.name == ThisName)
		{
			return ThisGObj.transform;
		}
		foreach (object obj in ThisGObj)
		{
			global::UnityEngine.Transform thisGObj = (global::UnityEngine.Transform)obj;
			global::UnityEngine.Transform transform = this.FindChildByName(ThisName, thisGObj);
			if (transform != null)
			{
				return transform;
			}
		}
		return null;
	}

	public global::UnityEngine.GameObject DaWeapon;

	public bool isUsed;

	public global::UnityEngine.GameObject DaWeaponOnceUsed;
}
