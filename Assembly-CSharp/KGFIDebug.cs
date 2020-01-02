using System;
using UnityEngine;

public interface KGFIDebug
{
	string GetName();

	void Log(global::KGFDebug.KGFDebugLog theLog);

	void Log(global::KGFeDebugLevel theLevel, string theCategory, string theMessage);

	void Log(global::KGFeDebugLevel theLevel, string theCategory, string theMessage, string theStackTrace);

	void Log(global::KGFeDebugLevel theLevel, string theCategory, string theMessage, string theStackTrace, global::UnityEngine.MonoBehaviour theObject);

	void SetMinimumLogLevel(global::KGFeDebugLevel theLevel);

	global::KGFeDebugLevel GetMinimumLogLevel();
}
