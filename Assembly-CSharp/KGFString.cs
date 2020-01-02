using System;
using UnityEngine;

public class KGFString : global::UnityEngine.MonoBehaviour
{
	public string GetString()
	{
		return this.itsString;
	}

	public override string ToString()
	{
		return this.itsString;
	}

	public string itsString;
}
