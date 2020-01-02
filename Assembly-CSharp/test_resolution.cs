using System;
using UnityEngine;

public class test_resolution : global::UnityEngine.MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
		if (global::UnityEngine.Input.GetKeyDown(global::UnityEngine.KeyCode.Return))
		{
			global::UnityEngine.Application.CaptureScreenshot("Screenshot.png");
		}
	}

	private void OnGUI()
	{
		global::UnityEngine.GUI.skin.box.normal.background = this.boxBg;
		global::UnityEngine.GUI.skin.box.border = new global::UnityEngine.RectOffset(2, 2, 2, 2);
		global::UnityEngine.GUI.skin.box.margin = new global::UnityEngine.RectOffset(0, 0, 0, 0);
		global::UnityEngine.GUI.skin.box.overflow = new global::UnityEngine.RectOffset(0, 0, 0, 0);
		global::UnityEngine.GUI.skin.box.padding = new global::UnityEngine.RectOffset(0, 0, 0, 0);
		global::UnityEngine.GUI.skin.box.stretchWidth = false;
		global::UnityEngine.GUI.skin.box.stretchHeight = false;
		int num = 0;
		foreach (global::UnityEngine.Resolution resolution in global::UnityEngine.Screen.resolutions)
		{
			global::UnityEngine.GUI.Box(new global::UnityEngine.Rect((float)(global::UnityEngine.Screen.width / 2 - resolution.width / 2), (float)(global::UnityEngine.Screen.height / 2 - resolution.height / 2), (float)resolution.width, (float)resolution.height), string.Empty);
			global::UnityEngine.GUI.Label(new global::UnityEngine.Rect((float)(global::UnityEngine.Screen.width / 2 - resolution.width / 2 + 2), (float)(global::UnityEngine.Screen.height / 2 - resolution.height / 2), (float)resolution.width, (float)resolution.height), resolution.width + " x " + resolution.height);
			if (global::UnityEngine.GUI.Button(new global::UnityEngine.Rect(0f, (float)(num * 25 + 50), 200f, 25f), resolution.width + " x " + resolution.height))
			{
				global::UnityEngine.Screen.SetResolution(resolution.width, resolution.height, global::UnityEngine.Screen.fullScreen);
			}
			num++;
		}
		if (global::UnityEngine.GUI.Button(new global::UnityEngine.Rect(0f, 0f, 200f, 50f), "Toggle Fullscreen"))
		{
			global::UnityEngine.Screen.fullScreen = !global::UnityEngine.Screen.fullScreen;
		}
		global::UnityEngine.GUI.Label(new global::UnityEngine.Rect(0f, (float)(num * 25 + 50), 200f, 50f), "Total resolutions = " + global::UnityEngine.Screen.resolutions.Length);
		global::UnityEngine.GUI.Label(new global::UnityEngine.Rect(200f, 0f, 200f, 50f), string.Concat(new object[]
		{
			"Current resolution = ",
			global::UnityEngine.Screen.width,
			" x ",
			global::UnityEngine.Screen.height
		}));
	}

	public global::UnityEngine.Texture2D boxBg;
}
