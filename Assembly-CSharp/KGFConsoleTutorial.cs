using System;
using UnityEngine;

public class KGFConsoleTutorial : global::UnityEngine.MonoBehaviour
{
	public void Awake()
	{
		global::KGFConsole.AddCommand("tutorial", this, "WriteMessage");
	}

	private void WriteMessage()
	{
		global::UnityEngine.Debug.Log("this is a message from WriteMessage()");
	}

	private void WriteMessage(int theNumber)
	{
		global::UnityEngine.Debug.Log("this is a message from WriteMessage(int) with the Parameter " + theNumber);
	}

	private void WriteMessage(string theText)
	{
		global::UnityEngine.Debug.Log("this is a message from WriteMessage(string) with the Parameter " + theText);
	}
}
