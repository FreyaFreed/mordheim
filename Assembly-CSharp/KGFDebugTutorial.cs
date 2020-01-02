using System;
using UnityEngine;

public class KGFDebugTutorial : global::UnityEngine.MonoBehaviour
{
	private void Start()
	{
		global::KGFDebug.LogDebug("Start() method of enemy: BossLevelOne was called", "Load.methods");
		global::KGFDebug.LogInfo("Currently 25 enemies in the Scene", "Enemy.Count");
		global::KGFDebug.LogWarning("Gravitation inconsistency: Make sure gravitation is -9.81.", "Physics.forces");
		global::KGFDebug.LogError("Cannot open file: d:\\tmp\\mytestdata. Make sure such a file exist and check read write permissions of the directory.", "IO.filereads", this);
		global::KGFDebug.LogFatal("The referenced module is missing. Make sure to keep the module running if its in use.", "Module.Status");
		global::UnityEngine.Debug.LogError("This error demonstrates KGFDebugs ability to capture unity3d Debug.Log messages.");
	}
}
