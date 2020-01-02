using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class LoadLevelAction : global::UnityEngine.MonoBehaviour
{
	private void Awake()
	{
	}

	public void LoadLevel()
	{
		global::UnityEngine.Debug.Log(string.Concat(new object[]
		{
			"LoadLevel : async = ",
			this.isAsync,
			", additive = ",
			this.isAdditive,
			", levelname = ",
			this.levelName
		}));
		if (this.isAsync)
		{
			if (this.isAdditive)
			{
				base.StartCoroutine(this.WaitLoading(global::UnityEngine.Application.LoadLevelAdditiveAsync(this.levelName)));
			}
			else
			{
				base.StartCoroutine(this.WaitLoading(global::UnityEngine.Application.LoadLevelAsync(this.levelName)));
			}
		}
		else if (this.isAdditive)
		{
			global::UnityEngine.Application.LoadLevelAdditive(this.levelName);
			this.loaded.Invoke();
		}
		else
		{
			global::UnityEngine.Application.LoadLevel(this.levelName);
			this.loaded.Invoke();
		}
	}

	private global::System.Collections.IEnumerator WaitLoading(global::UnityEngine.AsyncOperation loading)
	{
		while (!loading.isDone)
		{
			yield return null;
		}
		this.loaded.Invoke();
		yield break;
	}

	public bool loadOnStart;

	public string levelName;

	public bool isAsync;

	public bool isAdditive;

	public global::UnityEngine.Events.UnityEvent loaded;
}
