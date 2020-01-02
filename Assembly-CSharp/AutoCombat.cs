using System;
using UnityEngine;

public class AutoCombat : global::UnityEngine.MonoBehaviour
{
	private void Start()
	{
		global::UnityEngine.Animator component = base.GetComponent<global::UnityEngine.Animator>();
		component.SetInteger("atk_count", 4);
		component.SetInteger("combat_style", 1);
		component.SetLayerWeight(1, 1f);
	}

	private void Update()
	{
	}
}
