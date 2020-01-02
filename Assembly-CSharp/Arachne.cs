using System;
using System.Collections.Generic;
using UnityEngine;

public class Arachne : global::UnityEngine.MonoBehaviour
{
	public void Init()
	{
		global::System.Collections.Generic.List<global::UnityEngine.ClothSphereColliderPair> list = new global::System.Collections.Generic.List<global::UnityEngine.ClothSphereColliderPair>();
		global::System.Collections.Generic.List<global::UnityEngine.CapsuleCollider> list2 = new global::System.Collections.Generic.List<global::UnityEngine.CapsuleCollider>();
		global::UnityEngine.Cloth component = base.GetComponent<global::UnityEngine.Cloth>();
		global::UnitMenuController component2 = base.transform.parent.gameObject.GetComponent<global::UnitMenuController>();
		for (int i = 0; i < this.bones.Count; i++)
		{
			global::UnityEngine.Collider[] components = component2.BonesTr[this.bones[i]].GetComponents<global::UnityEngine.Collider>();
			component2.BonesTr[this.bones[i]].gameObject.layer = global::UnityEngine.LayerMask.NameToLayer("cloth");
			for (int j = 0; j < components.Length; j++)
			{
				if (components[j] is global::UnityEngine.CapsuleCollider)
				{
					list2.Add((global::UnityEngine.CapsuleCollider)components[j]);
				}
				else if (components[j] is global::UnityEngine.SphereCollider)
				{
					list.Add(new global::UnityEngine.ClothSphereColliderPair((global::UnityEngine.SphereCollider)components[j]));
				}
			}
		}
		component.sphereColliders = list.ToArray();
		component.capsuleColliders = list2.ToArray();
		global::UnityEngine.Object.Destroy(this);
	}

	public global::System.Collections.Generic.List<global::BoneId> bones;
}
