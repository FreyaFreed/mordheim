using System;
using System.IO;
using System.Text;
using UnityEngine;

public class KGFDebugFile : global::KGFModule, global::KGFIDebug
{
	public KGFDebugFile() : base(new global::System.Version(1, 0, 0, 1), new global::System.Version(1, 1, 0, 0))
	{
		if (this.itsDataDebugFile.itsFilePath == string.Empty)
		{
			this.itsDataDebugFile.itsFilePath = global::System.IO.Path.Combine(global::System.Environment.GetFolderPath(global::System.Environment.SpecialFolder.Desktop), "KGFLog.txt");
		}
	}

	protected override void KGFAwake()
	{
		base.KGFAwake();
		if (this.itsDataDebugFile.itsFilePath == string.Empty)
		{
			this.itsDataDebugFile.itsFilePath = global::System.IO.Path.Combine(global::System.Environment.GetFolderPath(global::System.Environment.SpecialFolder.Desktop), "KGFLog.txt");
		}
		using (global::System.IO.StreamWriter streamWriter = new global::System.IO.StreamWriter(this.itsDataDebugFile.itsFilePath, false, global::System.Text.Encoding.ASCII))
		{
			streamWriter.WriteLine(string.Empty.PadLeft("FileLogger started: ".Length + global::System.DateTime.Now.ToString().Length, '='));
		}
		global::KGFDebug.AddLogger(this);
	}

	public override string GetName()
	{
		return "KGFDebugFile";
	}

	public override string GetDocumentationPath()
	{
		return "KGFDebugFile_Manual.html";
	}

	public override string GetForumPath()
	{
		return "index.php?qa=kgfdebug";
	}

	public override global::UnityEngine.Texture2D GetIcon()
	{
		return null;
	}

	public void SetLogFilePath(string thePath)
	{
		this.itsDataDebugFile.itsFilePath = thePath;
	}

	public void Log(global::KGFeDebugLevel theLevel, string theCategory, string theMessage)
	{
		this.Log(theLevel, theCategory, theMessage, string.Empty, null);
	}

	public void Log(global::KGFDebug.KGFDebugLog aLog)
	{
		this.Log(aLog.GetLevel(), aLog.GetCategory(), aLog.GetMessage(), aLog.GetStackTrace(), aLog.GetObject() as global::UnityEngine.MonoBehaviour);
	}

	public void Log(global::KGFeDebugLevel theLevel, string theCategory, string theMessage, string theStackTrace)
	{
		this.Log(theLevel, theCategory, theMessage, theStackTrace, null);
	}

	public void Log(global::KGFeDebugLevel theLevel, string theCategory, string theMessage, string theStackTrace, global::UnityEngine.MonoBehaviour theObject)
	{
		try
		{
			using (global::System.IO.StreamWriter streamWriter = new global::System.IO.StreamWriter(this.itsDataDebugFile.itsFilePath, true, global::System.Text.Encoding.ASCII))
			{
				if (theObject != null)
				{
					streamWriter.WriteLine("{0}{6}{1}{6}{2}{6}{3}{6}{4}{6}{5}", new object[]
					{
						global::System.DateTime.Now.ToString(),
						theLevel,
						theCategory,
						theMessage,
						theObject.name,
						theStackTrace,
						this.itsDataDebugFile.itsSeparator
					});
				}
				else
				{
					streamWriter.WriteLine("{0}{6}{1}{6}{2}{6}{3}{6}{4}{6}{5}", new object[]
					{
						global::System.DateTime.Now.ToString(),
						theLevel,
						theCategory,
						theMessage,
						string.Empty,
						theStackTrace,
						this.itsDataDebugFile.itsSeparator
					});
				}
			}
		}
		catch (global::System.Exception ex)
		{
			global::UnityEngine.Debug.LogError("couldn't write to file " + this.itsDataDebugFile.itsFilePath + ". " + ex.Message);
		}
	}

	public void SetMinimumLogLevel(global::KGFeDebugLevel theLevel)
	{
		this.itsDataDebugFile.itsMinimumLogLevel = theLevel;
	}

	public global::KGFeDebugLevel GetMinimumLogLevel()
	{
		return this.itsDataDebugFile.itsMinimumLogLevel;
	}

	public override global::KGFMessageList Validate()
	{
		global::KGFMessageList kgfmessageList = new global::KGFMessageList();
		if (this.itsDataDebugFile.itsSeparator.Length == 0)
		{
			kgfmessageList.AddInfo("no seperator is set");
		}
		if (this.itsDataDebugFile.itsFilePath == string.Empty)
		{
			kgfmessageList.AddInfo("no file path set. path will be set to desktop.");
		}
		else if (!global::System.IO.Directory.Exists(global::System.IO.Path.GetDirectoryName(this.itsDataDebugFile.itsFilePath)))
		{
			kgfmessageList.AddError("the current directory doesn`t exist");
		}
		return kgfmessageList;
	}

	public global::KGFDataDebugFile itsDataDebugFile = new global::KGFDataDebugFile();
}
