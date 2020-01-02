using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using UnityEngine;

public class KGFDebug : global::KGFModule, global::KGFIValidator
{
	public KGFDebug() : base(new global::System.Version(1, 2, 0, 0), new global::System.Version(1, 2, 0, 0))
	{
	}

	protected override void KGFAwake()
	{
		base.KGFAwake();
		if (global::KGFDebug.itsInstance == null)
		{
			global::KGFDebug.itsInstance = this;
		}
		else if (global::KGFDebug.itsInstance != this)
		{
			global::UnityEngine.Debug.Log("there is more than one KFGDebug instance in this scene. please ensure there is always exactly one instance in this scene");
			global::UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	protected void Start()
	{
		if (global::KGFDebug.itsCachedLogs != null)
		{
			global::KGFDebug.itsCachedLogs.Clear();
			global::KGFDebug.itsCachedLogs = null;
		}
	}

	public static global::KGFDebug GetInstance()
	{
		return global::KGFDebug.itsInstance;
	}

	public override string GetName()
	{
		return "KGFDebug";
	}

	public override string GetDocumentationPath()
	{
		return "KGFDebug_Manual.html";
	}

	public override string GetForumPath()
	{
		return "index.php?qa=kgfdebug";
	}

	public override global::UnityEngine.Texture2D GetIcon()
	{
		global::KGFDebug.CheckInstance();
		if (global::KGFDebug.itsInstance != null)
		{
			return global::KGFDebug.itsInstance.itsDataModuleDebugger.itsIconModule;
		}
		return null;
	}

	public static global::KGFIDebug GetLogger(string theName)
	{
		foreach (global::KGFIDebug kgfidebug in global::KGFDebug.itsRegisteredLogger)
		{
			if (kgfidebug.GetName().Equals(theName))
			{
				return kgfidebug;
			}
		}
		return null;
	}

	public static void AddLogger(global::KGFIDebug theLogger)
	{
		global::KGFDebug.CheckInstance();
		if (!global::KGFDebug.itsRegisteredLogger.Contains(theLogger))
		{
			global::KGFDebug.itsRegisteredLogger.Add(theLogger);
			if (global::KGFDebug.itsCachedLogs != null)
			{
				foreach (global::KGFDebug.KGFDebugLog theLog in global::KGFDebug.itsCachedLogs)
				{
					theLogger.Log(theLog);
				}
			}
		}
		else
		{
			global::UnityEngine.Debug.LogError("this logger is already registered.");
		}
	}

	public static void RemoveLogger(global::KGFIDebug theLogger)
	{
		global::KGFDebug.CheckInstance();
		if (global::KGFDebug.itsRegisteredLogger.Contains(theLogger))
		{
			global::KGFDebug.itsRegisteredLogger.Remove(theLogger);
		}
		else
		{
			global::UnityEngine.Debug.LogError("the logger you tried to remove wasnt found.");
		}
	}

	private static void CheckInstance()
	{
		if (global::KGFDebug.itsInstance == null)
		{
			global::UnityEngine.Object @object = global::UnityEngine.Object.FindObjectOfType(typeof(global::KGFDebug));
			if (@object != null)
			{
				global::KGFDebug.itsInstance = (@object as global::KGFDebug);
			}
			else if (!global::KGFDebug.itsAlreadyChecked)
			{
				global::UnityEngine.Debug.LogError("Kolmich Debug Module is not running. Make sure that there is an instance of the KGFDebug prefab in the current scene. Adding loggers in Awake() can cause problems, prefer to add loggers in Start().");
				global::KGFDebug.itsAlreadyChecked = true;
			}
		}
	}

	public static void LogDebug(string theMessage)
	{
		global::KGFDebug.Log(global::KGFeDebugLevel.eDebug, "uncategorized", theMessage);
	}

	public static void LogDebug(string theMessage, string theCategory)
	{
		global::KGFDebug.Log(global::KGFeDebugLevel.eDebug, theCategory, theMessage);
	}

	public static void LogDebug(string theMessage, string theCategory, global::UnityEngine.MonoBehaviour theObject)
	{
		global::KGFDebug.Log(global::KGFeDebugLevel.eDebug, theCategory, theMessage, theObject);
	}

	public static void LogInfo(string theMessage)
	{
		global::KGFDebug.Log(global::KGFeDebugLevel.eInfo, "uncategorized", theMessage);
	}

	public static void LogInfo(string theMessage, string theCategory)
	{
		global::KGFDebug.Log(global::KGFeDebugLevel.eInfo, theCategory, theMessage);
	}

	public static void LogInfo(string theMessage, string theCategory, global::UnityEngine.MonoBehaviour theObject)
	{
		global::KGFDebug.Log(global::KGFeDebugLevel.eInfo, theCategory, theMessage, theObject);
	}

	public static void LogWarning(string theMessage)
	{
		global::KGFDebug.Log(global::KGFeDebugLevel.eWarning, "uncategorized", theMessage);
	}

	public static void LogWarning(string theMessage, string theCategory)
	{
		global::KGFDebug.Log(global::KGFeDebugLevel.eWarning, theCategory, theMessage);
	}

	public static void LogWarning(string theMessage, string theCategory, global::UnityEngine.MonoBehaviour theObject)
	{
		global::KGFDebug.Log(global::KGFeDebugLevel.eWarning, theCategory, theMessage, theObject);
	}

	public static void LogError(string theMessage)
	{
		global::KGFDebug.Log(global::KGFeDebugLevel.eError, "uncategorized", theMessage);
	}

	public static void LogError(string theMessage, string theCategory)
	{
		global::KGFDebug.Log(global::KGFeDebugLevel.eError, theCategory, theMessage);
	}

	public static void LogError(string theMessage, string theCategory, global::UnityEngine.MonoBehaviour theObject)
	{
		global::KGFDebug.Log(global::KGFeDebugLevel.eError, theCategory, theMessage, theObject);
	}

	public static void LogFatal(string theMessage)
	{
		global::KGFDebug.Log(global::KGFeDebugLevel.eFatal, "uncategorized", theMessage);
	}

	public static void LogFatal(string theMessage, string theCategory)
	{
		global::KGFDebug.Log(global::KGFeDebugLevel.eFatal, theCategory, theMessage);
	}

	public static void LogFatal(string theMessage, string theCategory, global::UnityEngine.MonoBehaviour theObject)
	{
		global::KGFDebug.Log(global::KGFeDebugLevel.eFatal, theCategory, theMessage, theObject);
	}

	private static void Log(global::KGFeDebugLevel theLevel, string theCategory, string theMessage)
	{
		global::KGFDebug.Log(theLevel, theCategory, theMessage, null);
	}

	private static void Log(global::KGFeDebugLevel theLevel, string theCategory, string theMessage, global::UnityEngine.MonoBehaviour theObject)
	{
		global::KGFDebug.CheckInstance();
		if (global::KGFDebug.itsInstance != null)
		{
			global::System.Text.StringBuilder stringBuilder = new global::System.Text.StringBuilder();
			if (theLevel >= global::KGFDebug.itsInstance.itsDataModuleDebugger.itsMinimumStackTraceLevel)
			{
				global::System.Diagnostics.StackTrace stackTrace = new global::System.Diagnostics.StackTrace(true);
				for (int i = 2; i < stackTrace.FrameCount; i++)
				{
					stringBuilder.Append(stackTrace.GetFrames()[i].ToString());
					stringBuilder.Append(global::System.Environment.NewLine);
				}
			}
			global::KGFDebug.KGFDebugLog kgfdebugLog = new global::KGFDebug.KGFDebugLog(theLevel, theCategory, theMessage, stringBuilder.ToString(), theObject);
			if (global::KGFDebug.itsCachedLogs != null)
			{
				global::KGFDebug.itsCachedLogs.Add(kgfdebugLog);
			}
			foreach (global::KGFIDebug kgfidebug in global::KGFDebug.itsRegisteredLogger)
			{
				if (kgfidebug.GetMinimumLogLevel() <= theLevel)
				{
					kgfidebug.Log(kgfdebugLog);
				}
			}
		}
	}

	public override global::KGFMessageList Validate()
	{
		global::KGFMessageList kgfmessageList = new global::KGFMessageList();
		if (this.itsDataModuleDebugger.itsIconModule == null)
		{
			kgfmessageList.AddWarning("the module icon is missing");
		}
		return kgfmessageList;
	}

	private static global::KGFDebug itsInstance = null;

	public global::KGFDebug.KGFDataDebug itsDataModuleDebugger = new global::KGFDebug.KGFDataDebug();

	private static global::System.Collections.Generic.List<global::KGFIDebug> itsRegisteredLogger = new global::System.Collections.Generic.List<global::KGFIDebug>();

	private static global::System.Collections.Generic.List<global::KGFDebug.KGFDebugLog> itsCachedLogs = new global::System.Collections.Generic.List<global::KGFDebug.KGFDebugLog>();

	private static bool itsAlreadyChecked = false;

	[global::System.Serializable]
	public class KGFDataDebug
	{
		public global::UnityEngine.Texture2D itsIconModule;

		public global::KGFeDebugLevel itsMinimumStackTraceLevel = global::KGFeDebugLevel.eFatal;
	}

	public class KGFDebugLog
	{
		public KGFDebugLog(global::KGFeDebugLevel theLevel, string theCategory, string theMessage, string theStackTrace)
		{
			this.itsLevel = theLevel;
			this.itsCategory = theCategory;
			this.itsMessage = theMessage;
			this.itsLogTime = global::System.DateTime.Now;
			this.itsStackTrace = theStackTrace;
			this.itsObject = null;
		}

		public KGFDebugLog(global::KGFeDebugLevel theLevel, string theCategory, string theMessage, string theStackTrace, object theObject)
		{
			this.itsLevel = theLevel;
			this.itsCategory = theCategory;
			this.itsMessage = theMessage;
			this.itsLogTime = global::System.DateTime.Now;
			this.itsStackTrace = theStackTrace;
			this.itsObject = theObject;
		}

		public global::KGFeDebugLevel GetLevel()
		{
			return this.itsLevel;
		}

		public string GetCategory()
		{
			return this.itsCategory;
		}

		public string GetMessage()
		{
			return this.itsMessage;
		}

		public global::System.DateTime GetLogTime()
		{
			return this.itsLogTime;
		}

		public string GetStackTrace()
		{
			return this.itsStackTrace;
		}

		public object GetObject()
		{
			return this.itsObject;
		}

		private global::KGFeDebugLevel itsLevel;

		private string itsCategory;

		private string itsMessage;

		private global::System.DateTime itsLogTime;

		private string itsStackTrace;

		private object itsObject;
	}
}
