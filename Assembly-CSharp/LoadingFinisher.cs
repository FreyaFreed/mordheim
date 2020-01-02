using System;
using UnityEngine;

public class LoadingFinisher : global::UnityEngine.MonoBehaviour
{
	private void Start()
	{
		global::PandoraSingleton<global::TransitionManager>.Instance.SetGameLoadingDone(false);
	}
}
