using System;
using UnityEngine;

public class KGFCustomGUITutorial : global::KGFObject, global::KGFICustomGUI
{
	public string GetName()
	{
		return "KGFCustomGUITutorial";
	}

	public string GetHeaderName()
	{
		return "Custom GUI Tutorial";
	}

	public global::UnityEngine.Texture2D GetIcon()
	{
		return null;
	}

	public void Render()
	{
	}
}
