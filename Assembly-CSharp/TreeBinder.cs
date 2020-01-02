using System;
using System.Collections.Generic;
using RAIN.BehaviorTrees;
using UnityEngine;

[global::UnityEngine.ExecuteInEditMode]
public class TreeBinder : global::PandoraSingleton<global::TreeBinder>
{
	public global::System.Collections.Generic.List<global::RAIN.BehaviorTrees.BTAssetBinding> BtBindings { get; private set; }

	private void Awake()
	{
		global::RAIN.BehaviorTrees.BTAsset[] array = global::UnityEngine.Resources.LoadAll<global::RAIN.BehaviorTrees.BTAsset>("ai");
		this.BtBindings = new global::System.Collections.Generic.List<global::RAIN.BehaviorTrees.BTAssetBinding>();
		for (int i = 0; i < array.Length; i++)
		{
			global::RAIN.BehaviorTrees.BTAssetBinding btassetBinding = new global::RAIN.BehaviorTrees.BTAssetBinding();
			btassetBinding.binding = array[i].name;
			btassetBinding.behaviorTree = array[i];
			this.BtBindings.Add(btassetBinding);
		}
	}
}
