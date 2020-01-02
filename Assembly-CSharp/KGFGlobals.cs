using System;
using System.Collections.Generic;
using UnityEngine;

public static class KGFGlobals
{
	public static string GetObjectPath(this global::UnityEngine.GameObject theObject)
	{
		global::System.Collections.Generic.List<string> list = new global::System.Collections.Generic.List<string>();
		global::UnityEngine.Transform transform = theObject.transform;
		do
		{
			list.Add(transform.name);
			transform = transform.parent;
		}
		while (transform != null);
		list.Reverse();
		return string.Join("/", list.ToArray());
	}
}
