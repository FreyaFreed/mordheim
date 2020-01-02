using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameInitialisation : global::UnityEngine.MonoBehaviour
{
	private void Awake()
	{
	}

	private void Start()
	{
		base.StartCoroutine(this.LoadPandora());
	}

	private global::System.Collections.IEnumerator LoadPandora()
	{
		global::UnityEngine.ResourceRequest req = global::UnityEngine.Resources.LoadAsync("prefabs/pandora");
		yield return req;
		global::UnityEngine.Object.Instantiate(req.asset);
		this.pandoraLoaded = true;
		yield break;
	}

	private void Update()
	{
		if (this.pandoraLoaded && global::PandoraSingleton<global::GameManager>.Instance.graphicOptionsSet && !this.launched)
		{
			this.launched = true;
			this.LaunchCopyrights();
		}
	}

	private void LaunchCopyrights()
	{
		global::PandoraDebug.LogDebug("Launching copyright scene", "uncategorised", null);
		global::UnityEngine.SceneManagement.SceneManager.LoadScene("copyright", global::UnityEngine.SceneManagement.LoadSceneMode.Single);
	}

	private bool launched;

	private bool pandoraLoaded;
}
