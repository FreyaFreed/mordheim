using System;
using UnityEngine;

public interface KGFICustomGUI
{
	string GetName();

	string GetHeaderName();

	global::UnityEngine.Texture2D GetIcon();

	void Render();
}
