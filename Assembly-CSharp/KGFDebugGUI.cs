using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using UnityEngine;

public class KGFDebugGUI : global::KGFModule, global::KGFIDebug
{
	public KGFDebugGUI() : base(new global::System.Version(1, 0, 0, 1), new global::System.Version(1, 1, 0, 0))
	{
	}

	private void OnEnable()
	{
		global::UnityEngine.Application.RegisterLogCallback(new global::UnityEngine.Application.LogCallback(this.HandleLog));
	}

	private void OnDisable()
	{
		global::UnityEngine.Application.RegisterLogCallback(null);
	}

	private void HandleLog(string theLogString, string theStackTrace, global::UnityEngine.LogType theLogType)
	{
		global::KGFeDebugLevel theLevel = global::KGFeDebugLevel.eInfo;
		switch (theLogType)
		{
		case global::UnityEngine.LogType.Error:
			theLevel = global::KGFeDebugLevel.eError;
			break;
		case global::UnityEngine.LogType.Assert:
			theLevel = global::KGFeDebugLevel.eFatal;
			break;
		case global::UnityEngine.LogType.Warning:
			theLevel = global::KGFeDebugLevel.eWarning;
			break;
		case global::UnityEngine.LogType.Log:
			theLevel = global::KGFeDebugLevel.eInfo;
			break;
		case global::UnityEngine.LogType.Exception:
			theLevel = global::KGFeDebugLevel.eError;
			break;
		}
		this.Log(theLevel, "CONSOLE", theLogString, theStackTrace);
	}

	protected override void KGFAwake()
	{
		if (global::KGFDebugGUI.itsInstance == null)
		{
			this.Init();
			global::KGFDebug.AddLogger(this);
		}
		else if (global::KGFDebugGUI.itsInstance != this)
		{
			global::UnityEngine.Object.Destroy(global::KGFDebugGUI.itsInstance.gameObject);
			global::UnityEngine.Debug.Log("multiple instances of KGFGUILogger are not allowed");
			return;
		}
	}

	public void Start()
	{
		this.itsTimeLeft = this.itsDataModuleGUILogger.itsFPSUpdateInterval;
	}

	public void Update()
	{
		this.itsTimeLeft -= global::UnityEngine.Time.deltaTime;
		this.itsAccumulatedFrames += global::UnityEngine.Time.timeScale / global::UnityEngine.Time.deltaTime;
		this.itsFramesInInterval++;
		if (global::UnityEngine.Input.GetKeyDown(global::UnityEngine.KeyCode.Escape))
		{
			this.itsLeaveFocus = true;
		}
		if (this.itsDataModuleGUILogger.itsHideKey != global::UnityEngine.KeyCode.None && !global::UnityEngine.Input.GetKey(this.itsDataModuleGUILogger.itsExpandKeyModifier) && ((global::UnityEngine.Input.GetKey(this.itsDataModuleGUILogger.itsHideKeyModifier) && global::UnityEngine.Input.GetKey(this.itsDataModuleGUILogger.itsHideKeyModifier) && global::UnityEngine.Input.GetKeyDown(this.itsDataModuleGUILogger.itsHideKey)) || (this.itsDataModuleGUILogger.itsHideKeyModifier == global::UnityEngine.KeyCode.None && global::UnityEngine.Input.GetKeyDown(this.itsDataModuleGUILogger.itsHideKey))))
		{
			this.itsDataModuleGUILogger.itsVisible = !this.itsDataModuleGUILogger.itsVisible;
		}
		if (this.itsDataModuleGUILogger.itsExpandKey != global::UnityEngine.KeyCode.None && !global::UnityEngine.Input.GetKey(this.itsDataModuleGUILogger.itsHideKeyModifier) && ((global::UnityEngine.Input.GetKey(this.itsDataModuleGUILogger.itsExpandKeyModifier) && global::UnityEngine.Input.GetKeyDown(this.itsDataModuleGUILogger.itsExpandKey)) || (this.itsDataModuleGUILogger.itsExpandKeyModifier == global::UnityEngine.KeyCode.None && global::UnityEngine.Input.GetKeyDown(this.itsDataModuleGUILogger.itsExpandKey))))
		{
			this.itsOpen = !this.itsOpen;
			if (this.itsOpen)
			{
				this.itsDataModuleGUILogger.itsVisible = true;
			}
		}
	}

	private static global::KGFDebugGUI GetInstance()
	{
		return global::KGFDebugGUI.itsInstance;
	}

	public static bool GetFocused()
	{
		return global::KGFDebugGUI.itsInstance != null && global::KGFDebugGUI.itsInstance.itsDataModuleGUILogger.itsVisible && (global::KGFDebugGUI.itsInstance.itsHover || global::KGFDebugGUI.itsInstance.itsFocus);
	}

	public static bool GetHover()
	{
		return global::KGFDebugGUI.itsInstance != null && global::KGFDebugGUI.itsInstance.itsDataModuleGUILogger.itsVisible && global::KGFDebugGUI.itsInstance.itsHover;
	}

	public static void Render()
	{
		global::KGFGUIUtility.SetSkinIndex(0);
		if (global::KGFDebugGUI.itsInstance != null)
		{
			if (!global::KGFDebugGUI.itsInstance.itsDataModuleGUILogger.itsVisible)
			{
				return;
			}
			global::KGFModule.RenderHelpWindow();
			if (global::KGFDebugGUI.itsInstance.itsOpen)
			{
				global::KGFDebugGUI.itsInstance.itsOpenWindow.x = 0f;
				global::KGFDebugGUI.itsInstance.itsOpenWindow.y = 0f;
				global::KGFDebugGUI.itsInstance.itsOpenWindow.width = (float)global::UnityEngine.Screen.width;
				global::KGFDebugGUI.itsInstance.itsOpenWindow.height = global::KGFDebugGUI.itsInstance.itsCurrentHeight;
				if (global::KGFDebugGUI.itsInstance.itsCurrentHeight < global::KGFGUIUtility.GetSkinHeight() * 11f)
				{
					global::KGFDebugGUI.itsInstance.itsCurrentHeight = global::KGFGUIUtility.GetSkinHeight() * 11f;
				}
				else if (global::KGFDebugGUI.itsInstance.itsCurrentHeight > (float)global::UnityEngine.Screen.height / 3f * 2f)
				{
					global::KGFDebugGUI.itsInstance.itsCurrentHeight = (float)global::UnityEngine.Screen.height / 3f * 2f;
				}
				global::UnityEngine.GUILayout.BeginArea(global::KGFDebugGUI.itsInstance.itsOpenWindow);
				global::KGFGUIUtility.BeginVerticalBox(global::KGFGUIUtility.eStyleBox.eBoxDecorated, new global::UnityEngine.GUILayoutOption[0]);
				global::UnityEngine.Texture2D theIcon = null;
				if (global::KGFDebug.GetInstance() != null)
				{
					theIcon = global::KGFDebug.GetInstance().GetIcon();
				}
				global::KGFGUIUtility.BeginWindowHeader("KGFDebugger", theIcon);
				global::KGFDebugGUI.itsInstance.DrawSummary();
				global::UnityEngine.GUILayout.FlexibleSpace();
				float num = global::KGFDebugGUI.itsInstance.itsCurrentHeight;
				global::KGFDebugGUI.itsInstance.itsCurrentHeight = global::KGFGUIUtility.HorizontalSlider(global::KGFDebugGUI.itsInstance.itsCurrentHeight, (float)global::UnityEngine.Screen.height / 3f * 2f, global::KGFGUIUtility.GetSkinHeight() * 11f, new global::UnityEngine.GUILayoutOption[]
				{
					global::UnityEngine.GUILayout.Width(75f)
				});
				if (num != global::KGFDebugGUI.itsInstance.itsCurrentHeight)
				{
					global::KGFDebugGUI.itsInstance.SaveSizeToPlayerPrefs();
				}
				if (global::KGFGUIUtility.Button(global::KGFDebugGUI.itsInstance.itsDataModuleGUILogger.itsIconHelp, global::KGFGUIUtility.eStyleButton.eButton, new global::UnityEngine.GUILayoutOption[0]))
				{
					global::KGFModule.OpenHelpWindow(global::KGFDebugGUI.itsInstance);
				}
				global::KGFDebugGUI.itsInstance.itsOpen = global::KGFGUIUtility.Toggle(global::KGFDebugGUI.itsInstance.itsOpen, string.Empty, global::KGFGUIUtility.eStyleToggl.eTogglSwitch, new global::UnityEngine.GUILayoutOption[]
				{
					global::UnityEngine.GUILayout.Width(global::KGFGUIUtility.GetSkinHeight())
				});
				global::KGFGUIUtility.EndWindowHeader(false);
				global::UnityEngine.GUILayout.Space(5f);
				global::UnityEngine.GUILayout.BeginHorizontal(new global::UnityEngine.GUILayoutOption[0]);
				global::KGFGUIUtility.BeginVerticalBox(global::KGFGUIUtility.eStyleBox.eBoxDecorated, new global::UnityEngine.GUILayoutOption[]
				{
					global::UnityEngine.GUILayout.Width((float)global::UnityEngine.Screen.width * 0.2f)
				});
				global::KGFDebugGUI.itsInstance.DrawCategoryColumn();
				global::KGFGUIUtility.EndVerticalBox();
				global::KGFGUIUtility.BeginVerticalBox(global::KGFGUIUtility.eStyleBox.eBoxDecorated, new global::UnityEngine.GUILayoutOption[0]);
				global::KGFDebugGUI.itsInstance.DrawContentColumn();
				global::KGFGUIUtility.EndVerticalBox();
				global::UnityEngine.GUILayout.EndHorizontal();
				global::KGFGUIUtility.EndVerticalBox();
				global::UnityEngine.GUILayout.EndArea();
			}
			else
			{
				global::KGFDebugGUI.itsInstance.DrawMinimizedWindow();
			}
			global::UnityEngine.Vector3 mousePosition = global::UnityEngine.Input.mousePosition;
			mousePosition.y = (float)global::UnityEngine.Screen.height - mousePosition.y;
			if (global::KGFDebugGUI.itsInstance.itsOpen)
			{
				if (global::KGFDebugGUI.itsInstance.itsOpenWindow.Contains(mousePosition))
				{
					if (!global::KGFDebugGUI.itsInstance.itsHover)
					{
						global::KGFDebugGUI.itsInstance.itsHover = true;
					}
				}
				else if (global::KGFDebugGUI.itsInstance.itsHover && !global::KGFDebugGUI.itsInstance.itsOpenWindow.Contains(mousePosition))
				{
					global::KGFDebugGUI.itsInstance.itsHover = false;
				}
			}
			else if (!global::KGFDebugGUI.itsInstance.itsOpen)
			{
				if (global::KGFDebugGUI.itsInstance.itsMinimizedWindow.Contains(mousePosition))
				{
					if (!global::KGFDebugGUI.itsInstance.itsHover)
					{
						global::KGFDebugGUI.itsInstance.itsHover = true;
					}
				}
				else if (global::KGFDebugGUI.itsInstance.itsHover && !global::KGFDebugGUI.itsInstance.itsMinimizedWindow.Contains(mousePosition))
				{
					global::KGFDebugGUI.itsInstance.itsHover = false;
				}
			}
			global::UnityEngine.GUI.SetNextControlName(string.Empty);
			if (global::KGFDebugGUI.itsInstance.itsLeaveFocus && global::KGFDebugGUI.itsInstance.itsFocus)
			{
				global::UnityEngine.Debug.Log("unfocus KGFDebugGUI");
				global::UnityEngine.GUI.FocusControl(string.Empty);
				global::KGFDebugGUI.itsInstance.itsLeaveFocus = false;
			}
			else
			{
				global::KGFDebugGUI.itsInstance.itsLeaveFocus = false;
			}
		}
		global::KGFGUIUtility.SetSkinIndex(1);
	}

	public static void Expand()
	{
		if (global::KGFDebugGUI.itsInstance != null)
		{
			global::KGFDebugGUI.itsInstance.itsOpen = true;
		}
	}

	public static bool GetExpanded()
	{
		return global::KGFDebugGUI.itsInstance != null && global::KGFDebugGUI.itsInstance.itsOpen;
	}

	public static void Minimize()
	{
		if (global::KGFDebugGUI.itsInstance != null)
		{
			global::KGFDebugGUI.itsInstance.itsOpen = false;
		}
	}

	public void OnGUI()
	{
		global::KGFDebugGUI.Render();
	}

	private void Init()
	{
		if (global::KGFDebugGUI.itsInstance != null)
		{
			return;
		}
		global::KGFDebugGUI.itsInstance = this;
		global::KGFDebugGUI.itsInstance.itsLogTable.Columns.Add(new global::KGFDataColumn("Lvl", typeof(string)));
		global::KGFDebugGUI.itsInstance.itsLogTable.Columns.Add(new global::KGFDataColumn("Time", typeof(string)));
		global::KGFDebugGUI.itsInstance.itsLogTable.Columns.Add(new global::KGFDataColumn("Category", typeof(string)));
		global::KGFDebugGUI.itsInstance.itsLogTable.Columns.Add(new global::KGFDataColumn("Message", typeof(string)));
		global::KGFDebugGUI.itsInstance.itsLogTable.Columns.Add(new global::KGFDataColumn("StackTrace", typeof(string)));
		global::KGFDebugGUI.itsInstance.itsLogTable.Columns.Add(new global::KGFDataColumn("Object", typeof(object)));
		this.itsTableControl = new global::KGFGUIDataTable(this.itsLogTable, new global::UnityEngine.GUILayoutOption[]
		{
			global::UnityEngine.GUILayout.ExpandHeight(true)
		});
		this.itsTableControl.OnClickRow += this.OnLogTableRowIsClicked;
		this.itsTableControl.PostRenderRow += this.PostLogTableRowHook;
		this.itsTableControl.PreCellContentHandler += this.PreCellContentHook;
		this.itsTableControl.SetColumnWidth(0, 40U);
		this.itsTableControl.SetColumnWidth(1, 90U);
		this.itsTableControl.SetColumnWidth(2, 150U);
		this.itsTableControl.SetColumnWidth(3, 0U);
		this.itsTableControl.SetColumnVisible(4, false);
		this.itsTableControl.SetColumnVisible(5, false);
		this.itsLogLevelFilter.Clear();
		this.itsLogCategoryCount.Clear();
		foreach (object obj in global::System.Enum.GetValues(typeof(global::KGFeDebugLevel)))
		{
			global::KGFeDebugLevel key = (global::KGFeDebugLevel)((int)obj);
			this.itsLogLevelFilter.Add(key, true);
			this.itsLogCategoryCount.Add(key, 0U);
		}
		this.itsDataModuleGUILogger.itsLogsPerPage = (global::KGFeItemsPerPage)((int)global::System.Enum.Parse(typeof(global::KGFeItemsPerPage), global::UnityEngine.PlayerPrefs.GetInt("KGF.KGFModuleDebugger.itsLogsPerPage", 25).ToString()));
		this.LoadCategoryFilterFromPlayerPrefs();
		this.itsOpenWindow = new global::UnityEngine.Rect(0f, 0f, (float)global::UnityEngine.Screen.width, this.itsCurrentHeight);
		this.itsMinimizedWindow = new global::UnityEngine.Rect(0f, 0f, (float)global::UnityEngine.Screen.width, 100f);
		global::KGFDebugGUI.itsInstance.LoadSizeFromPlayerPrefs();
		if (this.itsCurrentHeight == 0f)
		{
			this.itsCurrentHeight = (float)global::UnityEngine.Screen.height * 0.5f;
		}
	}

	private void DrawMouseCursor()
	{
		if (global::KGFGUIUtility.GetStyleCursor() != null)
		{
			global::UnityEngine.Vector3 mousePosition = global::UnityEngine.Input.mousePosition;
			global::UnityEngine.Rect position = new global::UnityEngine.Rect(mousePosition.x, (float)global::UnityEngine.Screen.height - mousePosition.y, global::KGFGUIUtility.GetStyleCursor().fixedWidth, global::KGFGUIUtility.GetStyleCursor().fixedHeight);
			global::UnityEngine.GUI.Label(position, string.Empty, global::KGFGUIUtility.GetStyleCursor());
		}
	}

	private void SaveSizeToPlayerPrefs()
	{
		global::UnityEngine.PlayerPrefs.SetFloat("KGFDebugGUI.WindowSize", this.itsCurrentHeight);
	}

	private void LoadSizeFromPlayerPrefs()
	{
		this.itsCurrentHeight = global::UnityEngine.PlayerPrefs.GetFloat("KGFDebugGUI.WindowSize", 0f);
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
		this.Init();
		global::System.Collections.Generic.Dictionary<global::KGFeDebugLevel, uint> dictionary2;
		global::System.Collections.Generic.Dictionary<global::KGFeDebugLevel, uint> dictionary = dictionary2 = this.itsLogCategoryCount;
		uint num = dictionary2[theLevel];
		dictionary[theLevel] = num + 1U;
		global::KGFDebug.KGFDebugLog kgfdebugLog = new global::KGFDebug.KGFDebugLog(theLevel, theCategory, theMessage, theStackTrace, theObject);
		this.itsLogList.Add(kgfdebugLog);
		if (!this.itsLogCategories.ContainsKey(theCategory))
		{
			this.itsLogCategories[theCategory] = new global::KGFDebugGUI.KGFDebugCategory(theCategory);
			this.itsLogCategories[theCategory].itsSelectedState = true;
		}
		this.itsLogCategories[theCategory].IncreaseCount();
		if (this.FilterDebugLog(kgfdebugLog))
		{
			global::KGFDataRow kgfdataRow = this.itsLogTable.NewRow();
			kgfdataRow[0].Value = kgfdebugLog.GetLevel().ToString();
			kgfdataRow[1].Value = kgfdebugLog.GetLogTime().ToString("HH:mm:ss.fff");
			kgfdataRow[2].Value = kgfdebugLog.GetCategory();
			kgfdataRow[3].Value = kgfdebugLog.GetMessage();
			kgfdataRow[4].Value = kgfdebugLog.GetStackTrace();
			kgfdataRow[5].Value = kgfdebugLog.GetObject();
			this.itsLogTable.Rows.Add(kgfdataRow);
		}
		if (kgfdebugLog.GetLevel() >= this.itsDataModuleGUILogger.itsMinimumExpandLogLevel)
		{
			this.itsDataModuleGUILogger.itsVisible = true;
			this.itsOpen = true;
		}
	}

	public void SetMinimumLogLevel(global::KGFeDebugLevel theLevel)
	{
		this.itsDataModuleGUILogger.itsMinimumLogLevel = theLevel;
	}

	public global::KGFeDebugLevel GetMinimumLogLevel()
	{
		return this.itsDataModuleGUILogger.itsMinimumLogLevel;
	}

	private global::System.Collections.Generic.IEnumerable<global::KGFDebugGUI.KGFDebugCategory> GetAllCategories()
	{
		return this.itsLogCategories.Values;
	}

	private bool FilterDebugLog(global::KGFDebug.KGFDebugLog theLogItem)
	{
		if (!this.itsLogLevelFilter[theLogItem.GetLevel()])
		{
			return false;
		}
		if (!this.itsSearchFilterMessage.Equals("Search") && !this.itsSearchFilterMessage.Equals(string.Empty) && !theLogItem.GetMessage().Trim().ToLower().Contains(this.itsSearchFilterMessage.Trim().ToLower()))
		{
			return false;
		}
		foreach (global::KGFDebugGUI.KGFDebugCategory kgfdebugCategory in this.itsLogCategories.Values)
		{
			if (kgfdebugCategory.itsSelectedState && kgfdebugCategory.GetName().ToLower().Contains(theLogItem.GetCategory().ToLower()))
			{
				return true;
			}
		}
		return false;
	}

	private global::System.Collections.Generic.IEnumerable<global::KGFDebug.KGFDebugLog> GetFilteredLogList()
	{
		foreach (global::KGFDebug.KGFDebugLog aDebugLog in this.itsLogList)
		{
			if (this.FilterDebugLog(aDebugLog))
			{
				yield return aDebugLog;
			}
		}
		yield break;
	}

	private void UpdateLogList()
	{
		this.itsLogTable.Rows.Clear();
		foreach (global::KGFDebug.KGFDebugLog kgfdebugLog in this.GetFilteredLogList())
		{
			global::KGFDataRow kgfdataRow = this.itsLogTable.NewRow();
			kgfdataRow[0].Value = kgfdebugLog.GetLevel().ToString();
			kgfdataRow[1].Value = kgfdebugLog.GetLogTime().ToString("HH:mm:ss.fff");
			kgfdataRow[2].Value = kgfdebugLog.GetCategory();
			kgfdataRow[3].Value = kgfdebugLog.GetMessage();
			kgfdataRow[4].Value = kgfdebugLog.GetStackTrace();
			kgfdataRow[5].Value = kgfdebugLog.GetObject();
			this.itsLogTable.Rows.Add(kgfdataRow);
		}
		this.UpdateLogListPageNumber();
	}

	private void UpdateLogListPageNumber()
	{
		if (this.itsLogTable.Rows.Count <= (int)this.itsDataModuleGUILogger.itsLogsPerPage)
		{
			this.itsCurrentPage = 0U;
		}
		else
		{
			this.itsCurrentPage = (uint)(global::UnityEngine.Mathf.CeilToInt((float)this.itsLogTable.Rows.Count / (float)this.itsDataModuleGUILogger.itsLogsPerPage) - 1);
		}
	}

	private void DrawMinimizedWindow()
	{
		float height = global::KGFGUIUtility.GetSkinHeight() + (float)global::KGFGUIUtility.GetStyleButton(global::KGFGUIUtility.eStyleButton.eButton).margin.vertical + (float)global::KGFGUIUtility.GetStyleBox(global::KGFGUIUtility.eStyleBox.eBoxDecorated).padding.vertical;
		this.itsMinimizedWindow.x = 0f;
		this.itsMinimizedWindow.y = 0f;
		this.itsMinimizedWindow.width = (float)global::UnityEngine.Screen.width;
		this.itsMinimizedWindow.height = height;
		global::UnityEngine.GUILayout.BeginArea(this.itsMinimizedWindow);
		global::UnityEngine.GUILayout.BeginVertical(new global::UnityEngine.GUILayoutOption[0]);
		global::UnityEngine.GUILayout.BeginHorizontal(new global::UnityEngine.GUILayoutOption[0]);
		global::KGFGUIUtility.BeginVerticalBox(global::KGFGUIUtility.eStyleBox.eBoxDecorated, new global::UnityEngine.GUILayoutOption[0]);
		global::UnityEngine.Texture2D theIcon = null;
		if (global::KGFDebug.GetInstance() != null)
		{
			theIcon = global::KGFDebug.GetInstance().GetIcon();
		}
		global::KGFGUIUtility.BeginWindowHeader("KGFDebugger", theIcon);
		this.DrawSummary();
		global::UnityEngine.GUILayout.FlexibleSpace();
		if (global::KGFGUIUtility.Button(this.itsDataModuleGUILogger.itsIconHelp, global::KGFGUIUtility.eStyleButton.eButton, new global::UnityEngine.GUILayoutOption[0]))
		{
			global::KGFModule.OpenHelpWindow(global::KGFDebugGUI.itsInstance);
		}
		this.itsOpen = global::KGFGUIUtility.Toggle(this.itsOpen, string.Empty, global::KGFGUIUtility.eStyleToggl.eTogglSwitch, new global::UnityEngine.GUILayoutOption[]
		{
			global::UnityEngine.GUILayout.Width(global::KGFGUIUtility.GetSkinHeight())
		});
		global::KGFGUIUtility.EndWindowHeader(false);
		global::KGFGUIUtility.EndVerticalBox();
		global::UnityEngine.GUILayout.EndHorizontal();
		global::UnityEngine.GUILayout.FlexibleSpace();
		global::UnityEngine.GUILayout.EndVertical();
		global::UnityEngine.GUILayout.EndArea();
	}

	private void DrawSummary()
	{
		float pixels = 10f;
		global::UnityEngine.GUILayout.Space(10f);
		global::KGFGUIUtility.BeginHorizontalBox(global::KGFGUIUtility.eStyleBox.eBoxInvisible, new global::UnityEngine.GUILayoutOption[0]);
		global::KGFGUIUtility.Label(string.Empty, this.itsDataModuleGUILogger.itsIconDebug, global::KGFGUIUtility.eStyleLabel.eLabel, new global::UnityEngine.GUILayoutOption[0]);
		global::KGFGUIUtility.Label(string.Format("{0}", this.itsLogCategoryCount[global::KGFeDebugLevel.eDebug]), global::KGFGUIUtility.eStyleLabel.eLabel, new global::UnityEngine.GUILayoutOption[0]);
		global::UnityEngine.GUILayout.Space(pixels);
		global::KGFGUIUtility.Label(string.Empty, this.itsDataModuleGUILogger.itsIconInfo, global::KGFGUIUtility.eStyleLabel.eLabel, new global::UnityEngine.GUILayoutOption[0]);
		global::KGFGUIUtility.Label(string.Format("{0}", this.itsLogCategoryCount[global::KGFeDebugLevel.eInfo]), global::KGFGUIUtility.eStyleLabel.eLabel, new global::UnityEngine.GUILayoutOption[0]);
		global::UnityEngine.GUILayout.Space(pixels);
		global::KGFGUIUtility.Label(string.Empty, this.itsDataModuleGUILogger.itsIconWarning, global::KGFGUIUtility.eStyleLabel.eLabel, new global::UnityEngine.GUILayoutOption[0]);
		global::KGFGUIUtility.Label(string.Format("{0}", this.itsLogCategoryCount[global::KGFeDebugLevel.eWarning]), global::KGFGUIUtility.eStyleLabel.eLabel, new global::UnityEngine.GUILayoutOption[0]);
		global::UnityEngine.GUILayout.Space(pixels);
		global::KGFGUIUtility.Label(string.Empty, this.itsDataModuleGUILogger.itsIconError, global::KGFGUIUtility.eStyleLabel.eLabel, new global::UnityEngine.GUILayoutOption[0]);
		global::KGFGUIUtility.Label(string.Format("{0}", this.itsLogCategoryCount[global::KGFeDebugLevel.eError]), global::KGFGUIUtility.eStyleLabel.eLabel, new global::UnityEngine.GUILayoutOption[0]);
		global::UnityEngine.GUILayout.Space(pixels);
		global::KGFGUIUtility.Label(string.Empty, this.itsDataModuleGUILogger.itsIconFatal, global::KGFGUIUtility.eStyleLabel.eLabel, new global::UnityEngine.GUILayoutOption[0]);
		global::KGFGUIUtility.Label(string.Format("{0}", this.itsLogCategoryCount[global::KGFeDebugLevel.eFatal]), global::KGFGUIUtility.eStyleLabel.eLabel, new global::UnityEngine.GUILayoutOption[0]);
		global::UnityEngine.GUILayout.Space(pixels);
		if (this.itsTimeLeft < 0f)
		{
			this.itsCurrentFPS = this.itsAccumulatedFrames / (float)this.itsFramesInInterval;
			this.itsTimeLeft = this.itsDataModuleGUILogger.itsFPSUpdateInterval;
			this.itsAccumulatedFrames = 0f;
			this.itsFramesInInterval = 0;
		}
		global::KGFGUIUtility.Label(string.Format("FPS: {0:F2}", this.itsCurrentFPS), global::KGFGUIUtility.eStyleLabel.eLabel, new global::UnityEngine.GUILayoutOption[0]);
		global::KGFGUIUtility.EndHorizontalBox();
	}

	private void DrawContentColumn()
	{
		global::KGFGUIUtility.BeginHorizontalBox(global::KGFGUIUtility.eStyleBox.eBoxDarkTop, new global::UnityEngine.GUILayoutOption[0]);
		global::KGFGUIUtility.BeginVerticalPadding();
		this.DrawFilterButtons();
		global::KGFGUIUtility.EndVerticalPadding();
		global::KGFGUIUtility.EndHorizontalBox();
		global::KGFGUIUtility.BeginHorizontalBox(global::KGFGUIUtility.eStyleBox.eBoxMiddleVertical, new global::UnityEngine.GUILayoutOption[0]);
		this.itsTableControl.SetStartRow((uint)((ulong)this.itsCurrentPage * (ulong)((long)this.itsDataModuleGUILogger.itsLogsPerPage)));
		this.itsTableControl.SetDisplayRowCount((uint)this.itsDataModuleGUILogger.itsLogsPerPage);
		global::KGFGUIUtility.BeginVerticalPadding();
		this.itsTableControl.Render();
		global::KGFGUIUtility.EndVerticalPadding();
		global::KGFGUIUtility.EndHorizontalBox();
		global::KGFGUIUtility.BeginHorizontalBox(global::KGFGUIUtility.eStyleBox.eBoxDarkBottom, new global::UnityEngine.GUILayoutOption[0]);
		this.DrawContentFilterBar();
		global::KGFGUIUtility.EndHorizontalBox();
	}

	private void DrawCategoryColumn()
	{
		bool flag = false;
		global::KGFGUIUtility.BeginHorizontalBox(global::KGFGUIUtility.eStyleBox.eBoxDarkTop, new global::UnityEngine.GUILayoutOption[0]);
		global::KGFGUIUtility.BeginVerticalPadding();
		if (global::KGFGUIUtility.Button("All", global::KGFGUIUtility.eStyleButton.eButtonLeft, new global::UnityEngine.GUILayoutOption[]
		{
			global::UnityEngine.GUILayout.ExpandWidth(true)
		}))
		{
			foreach (global::KGFDebugGUI.KGFDebugCategory kgfdebugCategory in this.GetAllCategories())
			{
				kgfdebugCategory.itsSelectedState = true;
				flag = true;
			}
		}
		if (global::KGFGUIUtility.Button("None", global::KGFGUIUtility.eStyleButton.eButtonRight, new global::UnityEngine.GUILayoutOption[]
		{
			global::UnityEngine.GUILayout.ExpandWidth(true)
		}))
		{
			foreach (global::KGFDebugGUI.KGFDebugCategory kgfdebugCategory2 in this.GetAllCategories())
			{
				kgfdebugCategory2.itsSelectedState = false;
				flag = true;
			}
		}
		global::KGFGUIUtility.EndVerticalPadding();
		global::KGFGUIUtility.EndHorizontalBox();
		global::KGFGUIUtility.BeginHorizontalBox(global::KGFGUIUtility.eStyleBox.eBoxMiddleVertical, new global::UnityEngine.GUILayoutOption[0]);
		this.itsCategoryScrollViewPosition = global::KGFGUIUtility.BeginScrollView(this.itsCategoryScrollViewPosition, false, true, new global::UnityEngine.GUILayoutOption[]
		{
			global::UnityEngine.GUILayout.ExpandWidth(true)
		});
		int num = 0;
		foreach (global::KGFDebugGUI.KGFDebugCategory kgfdebugCategory3 in this.GetAllCategories())
		{
			if (this.itsSearchFilterCategory.Trim().Equals("Search") || kgfdebugCategory3.GetName().ToLower().Contains(this.itsSearchFilterCategory.ToLower()))
			{
				num++;
				bool flag2 = global::KGFGUIUtility.Toggle(kgfdebugCategory3.itsSelectedState, string.Format("{0} ({1})", kgfdebugCategory3.GetName(), kgfdebugCategory3.GetCount().ToString()), global::KGFGUIUtility.eStyleToggl.eTogglSuperCompact, new global::UnityEngine.GUILayoutOption[0]);
				if (kgfdebugCategory3.itsSelectedState != flag2)
				{
					kgfdebugCategory3.itsSelectedState = flag2;
					flag = true;
				}
			}
		}
		if (num == 0)
		{
			global::KGFGUIUtility.Label("no items found", new global::UnityEngine.GUILayoutOption[0]);
		}
		global::UnityEngine.GUILayout.EndScrollView();
		global::KGFGUIUtility.EndHorizontalBox();
		string text = this.itsSearchFilterCategory;
		global::KGFGUIUtility.BeginHorizontalBox(global::KGFGUIUtility.eStyleBox.eBoxDarkBottom, new global::UnityEngine.GUILayoutOption[0]);
		global::UnityEngine.GUI.SetNextControlName("categorySearch");
		this.itsSearchFilterCategory = global::KGFGUIUtility.TextField(this.itsSearchFilterCategory, global::KGFGUIUtility.eStyleTextField.eTextField, new global::UnityEngine.GUILayoutOption[]
		{
			global::UnityEngine.GUILayout.ExpandWidth(true)
		});
		global::KGFGUIUtility.EndHorizontalBox();
		if (global::UnityEngine.GUI.GetNameOfFocusedControl().Equals("categorySearch"))
		{
			this.itsFocus = true;
			if (this.itsSearchFilterCategory.Equals("Search"))
			{
				this.itsSearchFilterCategory = string.Empty;
			}
		}
		if (!global::UnityEngine.GUI.GetNameOfFocusedControl().Equals("categorySearch"))
		{
			if (!global::UnityEngine.GUI.GetNameOfFocusedControl().Equals("messageSearch"))
			{
				this.itsFocus = false;
			}
			if (this.itsSearchFilterCategory.Equals(string.Empty))
			{
				this.itsSearchFilterCategory = "Search";
			}
		}
		if (!text.Equals(this.itsSearchFilterCategory))
		{
			flag = true;
		}
		if (flag)
		{
			this.UpdateLogList();
		}
	}

	private void DrawFilterButtons()
	{
		if (global::KGFGUIUtility.Button("All", global::KGFGUIUtility.eStyleButton.eButtonLeft, new global::UnityEngine.GUILayoutOption[]
		{
			global::UnityEngine.GUILayout.Width(70f)
		}))
		{
			foreach (object obj in global::System.Enum.GetValues(typeof(global::KGFeDebugLevel)))
			{
				global::KGFeDebugLevel key = (global::KGFeDebugLevel)((int)obj);
				this.itsLogLevelFilter[key] = true;
				this.UpdateLogList();
			}
			this.SaveCategoryFilterToPlayerPrefs();
		}
		if (global::KGFGUIUtility.Button("None", global::KGFGUIUtility.eStyleButton.eButtonRight, new global::UnityEngine.GUILayoutOption[]
		{
			global::UnityEngine.GUILayout.Width(70f)
		}))
		{
			foreach (object obj2 in global::System.Enum.GetValues(typeof(global::KGFeDebugLevel)))
			{
				global::KGFeDebugLevel key2 = (global::KGFeDebugLevel)((int)obj2);
				this.itsLogLevelFilter[key2] = false;
				this.UpdateLogList();
			}
			this.SaveCategoryFilterToPlayerPrefs();
		}
		foreach (object obj3 in global::System.Enum.GetValues(typeof(global::KGFeDebugLevel)))
		{
			global::KGFeDebugLevel kgfeDebugLevel = (global::KGFeDebugLevel)((int)obj3);
			if (kgfeDebugLevel != global::KGFeDebugLevel.eOff && kgfeDebugLevel != global::KGFeDebugLevel.eAll)
			{
				bool flag = global::KGFGUIUtility.Toggle(this.itsLogLevelFilter[kgfeDebugLevel], kgfeDebugLevel.ToString().Substring(1, kgfeDebugLevel.ToString().Length - 1), global::KGFGUIUtility.eStyleToggl.eTogglSuperCompact, new global::UnityEngine.GUILayoutOption[0]);
				global::UnityEngine.GUILayout.Space(10f);
				if (flag != this.itsLogLevelFilter[kgfeDebugLevel])
				{
					this.itsLogLevelFilter[kgfeDebugLevel] = flag;
					this.UpdateLogList();
					this.SaveCategoryFilterToPlayerPrefs();
				}
			}
		}
		global::UnityEngine.GUILayout.FlexibleSpace();
	}

	private void DrawContentFilterBar()
	{
		string a = this.itsSearchFilterMessage;
		bool flag = false;
		global::UnityEngine.GUI.SetNextControlName("messageSearch");
		this.itsSearchFilterMessage = global::KGFGUIUtility.TextField(this.itsSearchFilterMessage, global::KGFGUIUtility.eStyleTextField.eTextField, new global::UnityEngine.GUILayoutOption[]
		{
			global::UnityEngine.GUILayout.Width((float)global::UnityEngine.Screen.width * 0.2f)
		});
		global::UnityEngine.GUILayout.FlexibleSpace();
		if (global::UnityEngine.GUI.GetNameOfFocusedControl().Equals("messageSearch"))
		{
			this.itsFocus = true;
			if (this.itsSearchFilterMessage.Equals("Search"))
			{
				this.itsSearchFilterMessage = string.Empty;
				flag = true;
			}
		}
		if (!global::UnityEngine.GUI.GetNameOfFocusedControl().Equals("messageSearch"))
		{
			if (!global::UnityEngine.GUI.GetNameOfFocusedControl().Equals("categorySearch"))
			{
				this.itsFocus = false;
			}
			if (this.itsSearchFilterMessage.Equals(string.Empty))
			{
				this.itsSearchFilterMessage = "Search";
				flag = true;
			}
		}
		if (a != this.itsSearchFilterMessage && !flag)
		{
			this.itsLiveSearchChanged = true;
			this.itsLastChangeTime = global::UnityEngine.Time.time;
		}
		if (this.itsLiveSearchChanged && global::UnityEngine.Time.time - this.itsLastChangeTime > 1f && global::UnityEngine.Event.current.type != global::UnityEngine.EventType.Layout)
		{
			this.itsLiveSearchChanged = false;
			this.UpdateLogList();
		}
		global::UnityEngine.GUILayout.FlexibleSpace();
		global::UnityEngine.GUILayout.BeginVertical(new global::UnityEngine.GUILayoutOption[0]);
		global::KGFGUIUtility.BeginHorizontalBox(global::KGFGUIUtility.eStyleBox.eBoxInvisible, new global::UnityEngine.GUILayoutOption[0]);
		if (global::KGFGUIUtility.Button(this.itsDataModuleGUILogger.itsIconLeft, global::KGFGUIUtility.eStyleButton.eButtonLeft, new global::UnityEngine.GUILayoutOption[0]))
		{
			global::KGFeItemsPerPage itsLogsPerPage = this.itsDataModuleGUILogger.itsLogsPerPage;
			if (itsLogsPerPage != global::KGFeItemsPerPage.e25)
			{
				if (itsLogsPerPage != global::KGFeItemsPerPage.e50)
				{
					if (itsLogsPerPage != global::KGFeItemsPerPage.e100)
					{
						if (itsLogsPerPage != global::KGFeItemsPerPage.e250)
						{
							if (itsLogsPerPage == global::KGFeItemsPerPage.e500)
							{
								this.itsDataModuleGUILogger.itsLogsPerPage = global::KGFeItemsPerPage.e250;
							}
						}
						else
						{
							this.itsDataModuleGUILogger.itsLogsPerPage = global::KGFeItemsPerPage.e100;
						}
					}
					else
					{
						this.itsDataModuleGUILogger.itsLogsPerPage = global::KGFeItemsPerPage.e50;
					}
				}
				else
				{
					this.itsDataModuleGUILogger.itsLogsPerPage = global::KGFeItemsPerPage.e25;
				}
			}
			else
			{
				this.itsDataModuleGUILogger.itsLogsPerPage = global::KGFeItemsPerPage.e10;
			}
			global::UnityEngine.PlayerPrefs.SetInt("KGF.KGFModuleDebugger.itsLogsPerPage", (int)this.itsDataModuleGUILogger.itsLogsPerPage);
			this.UpdateLogListPageNumber();
		}
		global::KGFGUIUtility.BeginVerticalBox(global::KGFGUIUtility.eStyleBox.eBoxMiddleHorizontal, new global::UnityEngine.GUILayoutOption[0]);
		string theText = this.itsDataModuleGUILogger.itsLogsPerPage.ToString().Substring(1) + " entries per page";
		global::KGFGUIUtility.Label(theText, global::KGFGUIUtility.eStyleLabel.eLabelFitIntoBox, new global::UnityEngine.GUILayoutOption[0]);
		global::KGFGUIUtility.EndVerticalBox();
		if (global::KGFGUIUtility.Button(this.itsDataModuleGUILogger.itsIconRight, global::KGFGUIUtility.eStyleButton.eButtonRight, new global::UnityEngine.GUILayoutOption[0]))
		{
			global::KGFeItemsPerPage itsLogsPerPage = this.itsDataModuleGUILogger.itsLogsPerPage;
			if (itsLogsPerPage != global::KGFeItemsPerPage.e10)
			{
				if (itsLogsPerPage != global::KGFeItemsPerPage.e25)
				{
					if (itsLogsPerPage != global::KGFeItemsPerPage.e50)
					{
						if (itsLogsPerPage != global::KGFeItemsPerPage.e100)
						{
							if (itsLogsPerPage == global::KGFeItemsPerPage.e250)
							{
								this.itsDataModuleGUILogger.itsLogsPerPage = global::KGFeItemsPerPage.e500;
							}
						}
						else
						{
							this.itsDataModuleGUILogger.itsLogsPerPage = global::KGFeItemsPerPage.e250;
						}
					}
					else
					{
						this.itsDataModuleGUILogger.itsLogsPerPage = global::KGFeItemsPerPage.e100;
					}
				}
				else
				{
					this.itsDataModuleGUILogger.itsLogsPerPage = global::KGFeItemsPerPage.e50;
				}
			}
			else
			{
				this.itsDataModuleGUILogger.itsLogsPerPage = global::KGFeItemsPerPage.e25;
			}
			global::UnityEngine.PlayerPrefs.SetInt("KGF.KGFModuleDebugger.itsLogsPerPage", (int)this.itsDataModuleGUILogger.itsLogsPerPage);
			this.UpdateLogListPageNumber();
		}
		global::UnityEngine.GUILayout.Space(10f);
		if (global::KGFGUIUtility.Button(this.itsDataModuleGUILogger.itsIconLeft, global::KGFGUIUtility.eStyleButton.eButtonLeft, new global::UnityEngine.GUILayoutOption[0]) && this.itsCurrentPage > 0U)
		{
			this.itsCurrentPage -= 1U;
		}
		global::KGFGUIUtility.BeginVerticalBox(global::KGFGUIUtility.eStyleBox.eBoxMiddleHorizontal, new global::UnityEngine.GUILayoutOption[0]);
		int val = (int)global::System.Math.Ceiling((double)((float)this.itsLogTable.Rows.Count / (float)this.itsDataModuleGUILogger.itsLogsPerPage));
		string theText2 = string.Format("page {0}/{1}", this.itsCurrentPage + 1U, global::System.Math.Max(val, 1));
		global::KGFGUIUtility.Label(theText2, global::KGFGUIUtility.eStyleLabel.eLabelFitIntoBox, new global::UnityEngine.GUILayoutOption[0]);
		global::KGFGUIUtility.EndVerticalBox();
		if (global::KGFGUIUtility.Button(this.itsDataModuleGUILogger.itsIconRight, global::KGFGUIUtility.eStyleButton.eButtonRight, new global::UnityEngine.GUILayoutOption[0]) && (long)this.itsLogTable.Rows.Count > (long)((ulong)(this.itsCurrentPage + 1U) * (ulong)((long)this.itsDataModuleGUILogger.itsLogsPerPage)))
		{
			this.itsCurrentPage += 1U;
		}
		if (global::KGFGUIUtility.Button("clear", global::KGFGUIUtility.eStyleButton.eButton, new global::UnityEngine.GUILayoutOption[0]))
		{
			this.ClearCurrentLogs();
		}
		global::KGFGUIUtility.EndHorizontalBox();
		global::UnityEngine.GUILayout.EndVertical();
	}

	private void ClearCurrentLogs()
	{
		global::System.Collections.Generic.List<global::KGFDebug.KGFDebugLog> list = new global::System.Collections.Generic.List<global::KGFDebug.KGFDebugLog>();
		foreach (global::KGFDebug.KGFDebugLog kgfdebugLog in this.GetFilteredLogList())
		{
			list.Add(kgfdebugLog);
			if (this.itsLogCategories.ContainsKey(kgfdebugLog.GetCategory()))
			{
				this.itsLogCategories[kgfdebugLog.GetCategory()].DecreaseCount();
			}
			if (this.itsLogCategoryCount.ContainsKey(kgfdebugLog.GetLevel()))
			{
				global::System.Collections.Generic.Dictionary<global::KGFeDebugLevel, uint> dictionary2;
				global::System.Collections.Generic.Dictionary<global::KGFeDebugLevel, uint> dictionary = dictionary2 = this.itsLogCategoryCount;
				global::KGFeDebugLevel level;
				global::KGFeDebugLevel key = level = kgfdebugLog.GetLevel();
				uint num = dictionary2[level];
				dictionary[key] = num - 1U;
			}
		}
		foreach (global::KGFDebug.KGFDebugLog item in list)
		{
			this.itsLogList.Remove(item);
		}
		this.UpdateLogList();
	}

	private void PostLogTableRowHook(object theSender, global::System.EventArgs theArguments)
	{
		global::KGFDataRow kgfdataRow = theSender as global::KGFDataRow;
		global::UnityEngine.Color backgroundColor = global::UnityEngine.GUI.backgroundColor;
		if (kgfdataRow != null && !kgfdataRow.IsNull("Lvl"))
		{
			if (global::System.Enum.IsDefined(typeof(global::KGFeDebugLevel), kgfdataRow["Lvl"].ToString()))
			{
				global::UnityEngine.GUI.backgroundColor = this.GetColorForLevel((global::KGFeDebugLevel)((int)global::System.Enum.Parse(typeof(global::KGFeDebugLevel), kgfdataRow["Lvl"].ToString())));
			}
			else
			{
				global::UnityEngine.Debug.Log("the color is not defined");
			}
		}
		if (kgfdataRow != null)
		{
			global::UnityEngine.GUI.contentColor = global::UnityEngine.Color.white;
			if (kgfdataRow == this.itsCurrentSelectedRow)
			{
				global::KGFGUIUtility.BeginHorizontalBox(global::KGFGUIUtility.eStyleBox.eBoxDarkBottom, new global::UnityEngine.GUILayoutOption[]
				{
					global::UnityEngine.GUILayout.ExpandWidth(true)
				});
				global::UnityEngine.GUILayout.BeginVertical(new global::UnityEngine.GUILayoutOption[0]);
				global::UnityEngine.GUILayout.TextArea(string.Format("Object Path: {1}{0}{0}Time: {2}{0}{0}Category: {3}{0}{0}Message: {4}{0}{0}Stack Trace: {5}", new object[]
				{
					global::System.Environment.NewLine,
					global::KGFDebugGUI.GetObjectPath(kgfdataRow[5].Value as global::UnityEngine.MonoBehaviour),
					kgfdataRow[1].Value,
					kgfdataRow[2].Value,
					kgfdataRow[3].Value,
					kgfdataRow[4].Value
				}), new global::UnityEngine.GUILayoutOption[]
				{
					global::UnityEngine.GUILayout.ExpandWidth(true),
					global::UnityEngine.GUILayout.ExpandHeight(false)
				});
				if (global::KGFGUIUtility.Button("copy to file", global::KGFGUIUtility.eStyleButton.eButton, new global::UnityEngine.GUILayoutOption[]
				{
					global::UnityEngine.GUILayout.ExpandWidth(true)
				}))
				{
					string theFilePath = this.CreateTempFile(new global::System.Collections.Generic.Dictionary<string, string>
					{
						{
							"Message",
							kgfdataRow[3].Value.ToString()
						},
						{
							"Time",
							kgfdataRow[1].Value.ToString()
						},
						{
							"StackTrace",
							kgfdataRow[4].Value.ToString()
						}
					});
					this.OpenFile(theFilePath);
				}
				global::UnityEngine.GUILayout.EndVertical();
				global::KGFGUIUtility.EndHorizontalBox();
				global::UnityEngine.GUILayout.Space(5f);
			}
		}
		global::UnityEngine.GUI.backgroundColor = backgroundColor;
	}

	private bool PreCellContentHook(global::KGFDataRow theRow, global::KGFDataColumn theColumn, uint theWidth)
	{
		if (theColumn.ColumnName.Equals("Lvl"))
		{
			string text = theRow[theColumn.ColumnName].ToString();
			switch (text)
			{
			case "eDebug":
				global::KGFGUIUtility.Label(string.Empty, this.itsDataModuleGUILogger.itsIconDebug, global::KGFGUIUtility.eStyleLabel.eLabelFitIntoBox, new global::UnityEngine.GUILayoutOption[]
				{
					global::UnityEngine.GUILayout.Width(theWidth)
				});
				return true;
			case "eInfo":
				global::KGFGUIUtility.Label(string.Empty, this.itsDataModuleGUILogger.itsIconInfo, global::KGFGUIUtility.eStyleLabel.eLabelFitIntoBox, new global::UnityEngine.GUILayoutOption[]
				{
					global::UnityEngine.GUILayout.Width(theWidth)
				});
				return true;
			case "eWarning":
				global::KGFGUIUtility.Label(string.Empty, this.itsDataModuleGUILogger.itsIconWarning, global::KGFGUIUtility.eStyleLabel.eLabelFitIntoBox, new global::UnityEngine.GUILayoutOption[]
				{
					global::UnityEngine.GUILayout.Width(theWidth)
				});
				return true;
			case "eError":
				global::KGFGUIUtility.Label(string.Empty, this.itsDataModuleGUILogger.itsIconError, global::KGFGUIUtility.eStyleLabel.eLabelFitIntoBox, new global::UnityEngine.GUILayoutOption[]
				{
					global::UnityEngine.GUILayout.Width(theWidth)
				});
				return true;
			case "eFatal":
				global::KGFGUIUtility.Label(string.Empty, this.itsDataModuleGUILogger.itsIconFatal, global::KGFGUIUtility.eStyleLabel.eLabelFitIntoBox, new global::UnityEngine.GUILayoutOption[]
				{
					global::UnityEngine.GUILayout.Width(theWidth)
				});
				return true;
			}
			return false;
		}
		return false;
	}

	private void OnDebugGuiAdd(object theSender)
	{
		global::KGFICustomGUI kgficustomGUI = (global::KGFICustomGUI)theSender;
		if (!this.itsLogCategories.ContainsKey(kgficustomGUI.GetName()))
		{
			this.itsLogCategories[kgficustomGUI.GetName()] = new global::KGFDebugGUI.KGFDebugCategory(kgficustomGUI.GetName());
		}
	}

	private void OnLogTableRowIsClicked(object theSender, global::System.EventArgs theArguments)
	{
		global::KGFDataRow kgfdataRow = theSender as global::KGFDataRow;
		if (kgfdataRow != null)
		{
			if (kgfdataRow != this.itsCurrentSelectedRow)
			{
				this.itsCurrentSelectedRow = kgfdataRow;
			}
			else
			{
				this.itsCurrentSelectedRow = null;
			}
		}
	}

	private void SaveCategoryFilterToPlayerPrefs()
	{
		global::System.Text.StringBuilder stringBuilder = new global::System.Text.StringBuilder();
		foreach (object obj in global::System.Enum.GetValues(typeof(global::KGFeDebugLevel)))
		{
			global::KGFeDebugLevel kgfeDebugLevel = (global::KGFeDebugLevel)((int)obj);
			stringBuilder.Append(string.Format("{0}.{1}:", kgfeDebugLevel.ToString(), this.itsLogLevelFilter[kgfeDebugLevel].ToString()));
		}
		stringBuilder.Remove(stringBuilder.Length - 1, 1);
		global::UnityEngine.PlayerPrefs.SetString("KGF.KGFModuleDebug", stringBuilder.ToString());
	}

	private void LoadCategoryFilterFromPlayerPrefs()
	{
		string @string = global::UnityEngine.PlayerPrefs.GetString("KGF.KGFModuleDebug");
		string[] array = @string.Split(new char[]
		{
			':'
		});
		foreach (string text in array)
		{
			string[] array3 = text.Split(new char[]
			{
				'.'
			});
			if (array3.Length == 2)
			{
				if (global::System.Enum.IsDefined(typeof(global::KGFeDebugLevel), array3[0]))
				{
					bool value;
					if (bool.TryParse(array3[1], out value))
					{
						this.itsLogLevelFilter[(global::KGFeDebugLevel)((int)global::System.Enum.Parse(typeof(global::KGFeDebugLevel), array3[0]))] = value;
					}
				}
			}
		}
	}

	public global::UnityEngine.Color GetColorForLevel(global::KGFeDebugLevel theLevel)
	{
		switch (theLevel)
		{
		case global::KGFeDebugLevel.eDebug:
			return this.itsDataModuleGUILogger.itsColorDebug;
		case global::KGFeDebugLevel.eInfo:
			return this.itsDataModuleGUILogger.itsColorInfo;
		case global::KGFeDebugLevel.eWarning:
			return this.itsDataModuleGUILogger.itsColorWarning;
		case global::KGFeDebugLevel.eError:
			return this.itsDataModuleGUILogger.itsColorError;
		case global::KGFeDebugLevel.eFatal:
			return this.itsDataModuleGUILogger.itsColorFatal;
		default:
			return global::UnityEngine.Color.white;
		}
	}

	private string CreateTempFile(global::System.Collections.Generic.Dictionary<string, string> theContent)
	{
		string tempFileName = global::System.IO.Path.GetTempFileName();
		global::UnityEngine.Debug.Log("temp file path: " + tempFileName);
		string text = global::System.IO.Path.ChangeExtension(tempFileName, "txt");
		global::System.IO.File.Move(tempFileName, text);
		using (global::System.IO.StreamWriter streamWriter = new global::System.IO.StreamWriter(text, true, global::System.Text.Encoding.ASCII))
		{
			foreach (string text2 in theContent.Keys)
			{
				if (theContent[text2] != null)
				{
					streamWriter.WriteLine(text2);
					streamWriter.WriteLine(string.Empty.PadLeft(text2.Length, '='));
					foreach (string value in theContent[text2].Split(global::System.Environment.NewLine.ToCharArray()))
					{
						streamWriter.WriteLine(value);
					}
					streamWriter.WriteLine();
				}
			}
		}
		return text;
	}

	private void OpenFile(string theFilePath)
	{
		if (global::System.IO.File.Exists(theFilePath))
		{
			new global::System.Diagnostics.Process
			{
				StartInfo = 
				{
					FileName = theFilePath
				}
			}.Start();
		}
		else
		{
			global::UnityEngine.Debug.LogWarning("the file path was not found: " + theFilePath);
		}
	}

	private static string GetObjectPath(global::UnityEngine.MonoBehaviour theObject)
	{
		if (theObject != null)
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
		return string.Empty;
	}

	public override string GetName()
	{
		return "KGFDebugGUI";
	}

	public override string GetDocumentationPath()
	{
		return "KGFDebugGUI_Manual.html";
	}

	public override string GetForumPath()
	{
		return "index.php?qa=kgfdebug";
	}

	public override global::UnityEngine.Texture2D GetIcon()
	{
		return null;
	}

	public override global::KGFMessageList Validate()
	{
		global::KGFMessageList kgfmessageList = new global::KGFMessageList();
		if (this.itsDataModuleGUILogger.itsIconDebug == null)
		{
			kgfmessageList.AddWarning("the debug icon is missing");
		}
		if (this.itsDataModuleGUILogger.itsIconInfo == null)
		{
			kgfmessageList.AddWarning("the info icon is missing");
		}
		if (this.itsDataModuleGUILogger.itsIconWarning == null)
		{
			kgfmessageList.AddWarning("the warning icon is missing");
		}
		if (this.itsDataModuleGUILogger.itsIconError == null)
		{
			kgfmessageList.AddWarning("the error icon is missing");
		}
		if (this.itsDataModuleGUILogger.itsIconFatal == null)
		{
			kgfmessageList.AddWarning("the fatal error icon is missing");
		}
		if (this.itsDataModuleGUILogger.itsIconHelp == null)
		{
			kgfmessageList.AddWarning("the help icon is missing");
		}
		if (this.itsDataModuleGUILogger.itsIconLeft == null)
		{
			kgfmessageList.AddWarning("the left arrow icon is missing");
		}
		if (this.itsDataModuleGUILogger.itsIconRight == null)
		{
			kgfmessageList.AddWarning("the right arrow icon is missing");
		}
		if (this.itsDataModuleGUILogger.itsFPSUpdateInterval < 0f)
		{
			kgfmessageList.AddError("the FPS update interval�l must be greater than zero");
		}
		return kgfmessageList;
	}

	private static global::KGFDebugGUI itsInstance;

	private global::KGFDataTable itsLogTable = new global::KGFDataTable();

	private global::KGFGUIDataTable itsTableControl;

	private global::System.Collections.Generic.List<global::KGFDebug.KGFDebugLog> itsLogList = new global::System.Collections.Generic.List<global::KGFDebug.KGFDebugLog>();

	private global::System.Collections.Generic.Dictionary<string, global::KGFDebugGUI.KGFDebugCategory> itsLogCategories = new global::System.Collections.Generic.Dictionary<string, global::KGFDebugGUI.KGFDebugCategory>();

	private global::System.Collections.Generic.Dictionary<global::KGFeDebugLevel, bool> itsLogLevelFilter = new global::System.Collections.Generic.Dictionary<global::KGFeDebugLevel, bool>();

	private global::UnityEngine.Vector2 itsCategoryScrollViewPosition = global::UnityEngine.Vector2.zero;

	private string itsSearchFilterMessage = "Search";

	private string itsSearchFilterCategory = "Search";

	private float itsCurrentHeight = (float)global::UnityEngine.Screen.height;

	private bool itsOpen;

	private bool itsLiveSearchChanged;

	private float itsLastChangeTime;

	private uint itsCurrentPage;

	private global::KGFDataRow itsCurrentSelectedRow;

	private global::System.Collections.Generic.Dictionary<global::KGFeDebugLevel, uint> itsLogCategoryCount = new global::System.Collections.Generic.Dictionary<global::KGFeDebugLevel, uint>();

	public global::KGFDebugGUI.KGFDataModuleGUILogger itsDataModuleGUILogger = new global::KGFDebugGUI.KGFDataModuleGUILogger();

	private global::UnityEngine.Rect itsOpenWindow;

	private global::UnityEngine.Rect itsMinimizedWindow;

	private float itsAccumulatedFrames;

	private int itsFramesInInterval;

	private float itsTimeLeft;

	private float itsCurrentFPS;

	private bool itsHover;

	private bool itsFocus;

	private bool itsLeaveFocus;

	[global::System.Serializable]
	public class KGFDataModuleGUILogger
	{
		public global::KGFeDebugLevel itsMinimumLogLevel;

		public global::KGFeItemsPerPage itsLogsPerPage = global::KGFeItemsPerPage.e25;

		public global::KGFeDebugLevel itsMinimumExpandLogLevel = global::KGFeDebugLevel.eOff;

		public global::UnityEngine.Color itsColorDebug = global::UnityEngine.Color.white;

		public global::UnityEngine.Color itsColorInfo = global::UnityEngine.Color.grey;

		public global::UnityEngine.Color itsColorWarning = global::UnityEngine.Color.yellow;

		public global::UnityEngine.Color itsColorError = global::UnityEngine.Color.red;

		public global::UnityEngine.Color itsColorFatal = global::UnityEngine.Color.magenta;

		public global::UnityEngine.Texture2D itsIconDebug;

		public global::UnityEngine.Texture2D itsIconInfo;

		public global::UnityEngine.Texture2D itsIconWarning;

		public global::UnityEngine.Texture2D itsIconError;

		public global::UnityEngine.Texture2D itsIconFatal;

		public global::UnityEngine.Texture2D itsIconHelp;

		public global::UnityEngine.Texture2D itsIconLeft;

		public global::UnityEngine.Texture2D itsIconRight;

		public float itsFPSUpdateInterval = 0.5f;

		public global::UnityEngine.KeyCode itsHideKeyModifier;

		public global::UnityEngine.KeyCode itsHideKey = global::UnityEngine.KeyCode.F1;

		public global::UnityEngine.KeyCode itsExpandKeyModifier = global::UnityEngine.KeyCode.LeftAlt;

		public global::UnityEngine.KeyCode itsExpandKey = global::UnityEngine.KeyCode.F1;

		public bool itsVisible = true;
	}

	private class KGFDebugCategory
	{
		public KGFDebugCategory(string theName)
		{
			this.itsName = theName;
			this.itsCount = 0;
		}

		public string GetName()
		{
			return this.itsName;
		}

		public void IncreaseCount()
		{
			this.itsCount++;
		}

		public void DecreaseCount()
		{
			this.itsCount--;
		}

		public int GetCount()
		{
			return this.itsCount;
		}

		private int itsCount;

		private string itsName;

		public bool itsSelectedState;
	}
}
