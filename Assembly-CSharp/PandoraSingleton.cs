using System;
using UnityEngine;

public class PandoraSingleton<T> : global::UnityEngine.MonoBehaviour where T : global::UnityEngine.MonoBehaviour
{
	public static T Instance
	{
		get
		{
			if (global::PandoraSingleton<T>.instance == null)
			{
				global::PandoraSingleton<T>.instance = (T)((object)global::UnityEngine.Object.FindObjectOfType(typeof(T)));
				if (global::PandoraSingleton<T>.instance == null)
				{
					global::PandoraDebug.LogError("An instance of " + typeof(T) + " is needed in the scene, but there is none.", "uncategorised", null);
				}
			}
			return global::PandoraSingleton<T>.instance;
		}
	}

	private void Awake()
	{
		global::PandoraSingleton<T>.instance = base.GetComponent<T>();
	}

	private void OnDestroy()
	{
		global::PandoraSingleton<T>.instance = (T)((object)null);
	}

	public static bool Exists()
	{
		return global::PandoraSingleton<T>.instance != null;
	}

	protected static T instance;
}
