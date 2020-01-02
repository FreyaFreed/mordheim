using System;
using System.IO;
using UnityEngine;

public class KGFDocumentation : global::UnityEngine.MonoBehaviour
{
	public void OpenDocumentation()
	{
		string text = global::UnityEngine.Application.dataPath;
		text = global::System.IO.Path.Combine(text, "kolmich");
		text = global::System.IO.Path.Combine(text, "documentation");
		text = global::System.IO.Path.Combine(text, "files");
		text += global::System.IO.Path.DirectorySeparatorChar;
		text += "documentation.html";
		text.Replace('/', global::System.IO.Path.DirectorySeparatorChar);
		global::UnityEngine.Application.OpenURL("file://" + text);
	}
}
