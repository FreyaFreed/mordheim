using System;
using UnityEngine;

public class test_launch_loading : global::UnityEngine.MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
		if (global::UnityEngine.Input.GetKeyDown(global::UnityEngine.KeyCode.Space))
		{
			global::UnityEngine.GameObject gameObject = (global::UnityEngine.GameObject)global::UnityEngine.Object.Instantiate(global::UnityEngine.Resources.Load("prefabs/transitions/transition_fade"));
			global::TransitionBase component = gameObject.GetComponent<global::TransitionBase>();
			global::PandoraSingleton<global::TransitionManager>.Instance.LoadNextScene("test_move_actions", global::SceneLoadingTypeId.MAIN_MENU, 1.5f, true, false, false);
		}
	}
}
