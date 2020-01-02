using System;
using UnityEngine;

public class KGFDebugLoggerTutorial : global::UnityEngine.MonoBehaviour, global::KGFIDebug
{
	public void Awake()
	{
		global::KGFDebug.AddLogger(this);
	}

	public string GetName()
	{
		return "KGFTutorialLogger";
	}

	public void Log(global::KGFDebug.KGFDebugLog aLog)
	{
		this.Log(aLog.GetLevel(), aLog.GetCategory(), aLog.GetMessage(), aLog.GetStackTrace(), aLog.GetObject() as global::UnityEngine.MonoBehaviour);
	}

	public void Log(global::KGFeDebugLevel theLevel, string theCategory, string theMessage)
	{
		this.Log(theLevel, theCategory, theMessage, string.Empty, null);
	}

	public void Log(global::KGFeDebugLevel theLevel, string theCategory, string theMessage, string theStackTrace)
	{
		this.Log(theLevel, theCategory, theMessage, theStackTrace, null);
	}

	public void Log(global::KGFeDebugLevel theLevel, string theCategory, string theMessage, string theStackTrace, global::UnityEngine.MonoBehaviour theObject)
	{
		if (theObject != null)
		{
			global::UnityEngine.Debug.Log(string.Format("{0} {1} {5} {2}{5}{3}{5}{4}", new object[]
			{
				theLevel,
				theCategory,
				theMessage,
				theObject.name,
				theStackTrace,
				global::System.Environment.NewLine
			}));
		}
		else
		{
			global::UnityEngine.Debug.Log(string.Format("{0} {1} {4}{2}{4}{3}", new object[]
			{
				theLevel,
				theCategory,
				theMessage,
				theStackTrace,
				global::System.Environment.NewLine
			}));
		}
	}

	public void SetMinimumLogLevel(global::KGFeDebugLevel theLevel)
	{
		this.itsMinimumLogLevel = theLevel;
	}

	public global::KGFeDebugLevel GetMinimumLogLevel()
	{
		return this.itsMinimumLogLevel;
	}

	private global::KGFeDebugLevel itsMinimumLogLevel;
}
