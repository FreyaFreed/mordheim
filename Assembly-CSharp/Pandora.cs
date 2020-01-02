using System;
using System.Collections.Generic;
using UnityEngine;

public class Pandora : global::UnityEngine.MonoBehaviour
{
	private void Awake()
	{
		foreach (string text in global::System.Environment.GetCommandLineArgs())
		{
			if (text != null)
			{
				if (global::Pandora.<>f__switch$mapC == null)
				{
					global::Pandora.<>f__switch$mapC = new global::System.Collections.Generic.Dictionary<string, int>(2)
					{
						{
							"-lowSettings",
							0
						},
						{
							"-fullLog",
							1
						}
					};
				}
				int num;
				if (global::Pandora.<>f__switch$mapC.TryGetValue(text, out num))
				{
					if (num != 0)
					{
						if (num == 1)
						{
							global::Pandora.fullLog = true;
						}
					}
					else
					{
						global::Pandora.useLowSettings = true;
					}
				}
			}
		}
		global::UnityEngine.Profiler.maxNumberOfSamplesPerFrame = -1;
		if (global::Pandora.box == null)
		{
			global::Pandora.box = this;
			global::UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			global::Pandora.allSystemsInitialized = true;
		}
		else
		{
			global::UnityEngine.Object.DestroyImmediate(base.gameObject);
		}
	}

	private void Start()
	{
	}

	public uint GetNextGUID()
	{
		return global::Pandora.GUID++;
	}

	public static bool allSystemsInitialized;

	public static global::Pandora box;

	public global::UnityEngine.GameObject kgf_debug_prefab;

	public global::UnityEngine.GameObject kgf_console_prefab;

	public global::UnityEngine.GameObject kgf_debug_gui_prefab;

	private static uint GUID;

	public static bool useLowSettings;

	public static bool fullLog;
}
