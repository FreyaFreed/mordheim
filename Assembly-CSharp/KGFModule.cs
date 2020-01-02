using System;
using UnityEngine;

public abstract class KGFModule : global::KGFObject, global::KGFIValidator
{
	public KGFModule(global::System.Version theCurrentVersion, global::System.Version theMinimumCoreVersion)
	{
		this.itsCurrentVersion = theCurrentVersion;
		this.itsMinimumCoreVersion = theMinimumCoreVersion;
		if (global::KGFCoreVersion.GetCurrentVersion() < this.itsMinimumCoreVersion)
		{
			global::UnityEngine.Debug.LogError("the KGFCore verison installed in this scene is older than the required version. please update the KGFCore to the latest version");
		}
	}

	public global::System.Version GetCurrentVersion()
	{
		return this.itsCurrentVersion.Clone() as global::System.Version;
	}

	public global::System.Version GetRequiredCoreVersion()
	{
		return this.itsMinimumCoreVersion.Clone() as global::System.Version;
	}

	public abstract string GetName();

	public abstract global::UnityEngine.Texture2D GetIcon();

	public abstract string GetDocumentationPath();

	public abstract string GetForumPath();

	public abstract global::KGFMessageList Validate();

	public static void OpenHelpWindow(global::KGFModule theModule)
	{
		global::KGFModule.itsOpenModule = theModule;
	}

	public static void RenderHelpWindow()
	{
		if (global::KGFModule.itsOpenModule != null)
		{
			int num = 512 + (int)global::KGFGUIUtility.GetSkinHeight() * 2;
			int num2 = 256 + (int)global::KGFGUIUtility.GetSkinHeight() * 7;
			global::UnityEngine.Rect theRect = new global::UnityEngine.Rect((float)((global::UnityEngine.Screen.width - num) / 2), (float)((global::UnityEngine.Screen.height - num2) / 2), (float)num, (float)num2);
			global::KGFGUIUtility.Window(12345689, theRect, new global::UnityEngine.GUI.WindowFunction(global::KGFModule.RenderHelpWindowMethod), global::KGFModule.itsOpenModule.GetName() + " (part of KOLMICH Game Framework)", new global::UnityEngine.GUILayoutOption[0]);
			if (theRect.Contains(global::UnityEngine.Event.current.mousePosition) && global::UnityEngine.Event.current.type == global::UnityEngine.EventType.MouseDown && global::UnityEngine.Event.current.button == 0)
			{
				global::KGFModule.itsOpenModule = null;
			}
		}
		else
		{
			global::KGFModule.itsOpenModule = null;
		}
	}

	private static void RenderHelpWindowMethod(int theWindowID)
	{
		global::UnityEngine.GUILayout.BeginHorizontal(new global::UnityEngine.GUILayoutOption[0]);
		global::UnityEngine.GUILayout.FlexibleSpace();
		global::KGFGUIUtility.BeginVerticalBox(global::KGFGUIUtility.eStyleBox.eBoxInvisible, new global::UnityEngine.GUILayoutOption[]
		{
			global::UnityEngine.GUILayout.ExpandHeight(true)
		});
		global::KGFGUIUtility.BeginHorizontalPadding();
		global::KGFGUIUtility.BeginHorizontalBox(global::KGFGUIUtility.eStyleBox.eBoxDarkTop, new global::UnityEngine.GUILayoutOption[]
		{
			global::UnityEngine.GUILayout.ExpandWidth(true)
		});
		global::UnityEngine.GUILayout.FlexibleSpace();
		global::UnityEngine.GUILayout.Label(global::KGFGUIUtility.GetLogo(), new global::UnityEngine.GUILayoutOption[]
		{
			global::UnityEngine.GUILayout.Height(50f)
		});
		global::UnityEngine.GUILayout.FlexibleSpace();
		global::KGFGUIUtility.EndHorizontalBox();
		global::KGFGUIUtility.BeginHorizontalBox(global::KGFGUIUtility.eStyleBox.eBoxBottom, new global::UnityEngine.GUILayoutOption[]
		{
			global::UnityEngine.GUILayout.ExpandWidth(true),
			global::UnityEngine.GUILayout.ExpandHeight(true)
		});
		global::UnityEngine.GUILayout.Label("KOLMICH Creations e.U. is a small company based out of Vienna, Austria.\nWhile developing cool unity3d projects we put an immense amount of time \nto create professional tools and professional content. \n\n\nIf you have any ideas on improvements or you just want to give us some feedback use one of the links below.", new global::UnityEngine.GUILayoutOption[]
		{
			global::UnityEngine.GUILayout.ExpandWidth(true)
		});
		global::KGFGUIUtility.EndHorizontalBox();
		global::UnityEngine.GUILayout.Space(global::KGFGUIUtility.GetSkinHeight());
		global::KGFGUIUtility.BeginHorizontalBox(global::KGFGUIUtility.eStyleBox.eBoxDarkTop, new global::UnityEngine.GUILayoutOption[]
		{
			global::UnityEngine.GUILayout.ExpandWidth(true)
		});
		global::KGFGUIUtility.Label(global::KGFModule.itsOpenModule.GetName() + " version:", global::KGFGUIUtility.eStyleLabel.eLabelFitIntoBox, new global::UnityEngine.GUILayoutOption[0]);
		global::KGFGUIUtility.Label(global::KGFModule.itsOpenModule.GetCurrentVersion().ToString(), global::KGFGUIUtility.eStyleLabel.eLabelFitIntoBox, new global::UnityEngine.GUILayoutOption[0]);
		global::UnityEngine.GUILayout.FlexibleSpace();
		global::KGFGUIUtility.Label("req. KGFCore version:", global::KGFGUIUtility.eStyleLabel.eLabelFitIntoBox, new global::UnityEngine.GUILayoutOption[0]);
		global::KGFGUIUtility.Label(global::KGFModule.itsOpenModule.GetRequiredCoreVersion().ToString(), global::KGFGUIUtility.eStyleLabel.eLabelFitIntoBox, new global::UnityEngine.GUILayoutOption[0]);
		global::KGFGUIUtility.EndHorizontalBox();
		global::KGFGUIUtility.BeginHorizontalBox(global::KGFGUIUtility.eStyleBox.eBoxDarkBottom, new global::UnityEngine.GUILayoutOption[]
		{
			global::UnityEngine.GUILayout.ExpandWidth(true)
		});
		global::KGFGUIUtility.BeginVerticalPadding();
		if (global::KGFGUIUtility.Button(global::KGFGUIUtility.GetHelpIcon(), "documentation", global::KGFGUIUtility.eStyleButton.eButtonLeft, new global::UnityEngine.GUILayoutOption[]
		{
			global::UnityEngine.GUILayout.ExpandWidth(true)
		}))
		{
			global::UnityEngine.Application.OpenURL("http://www.kolmich.at/documentation/" + global::KGFModule.itsOpenModule.GetDocumentationPath());
			global::KGFModule.itsOpenModule = null;
		}
		if (global::KGFGUIUtility.Button(global::KGFGUIUtility.GetHelpIcon(), "forum", global::KGFGUIUtility.eStyleButton.eButtonMiddle, new global::UnityEngine.GUILayoutOption[]
		{
			global::UnityEngine.GUILayout.ExpandWidth(true)
		}))
		{
			global::UnityEngine.Application.OpenURL("http://www.kolmich.at/forum/" + global::KGFModule.itsOpenModule.GetForumPath());
			global::KGFModule.itsOpenModule = null;
		}
		if (global::KGFGUIUtility.Button(global::KGFGUIUtility.GetHelpIcon(), "homepage", global::KGFGUIUtility.eStyleButton.eButtonRight, new global::UnityEngine.GUILayoutOption[]
		{
			global::UnityEngine.GUILayout.ExpandWidth(true)
		}))
		{
			global::UnityEngine.Application.OpenURL("http://www.kolmich.at");
			global::KGFModule.itsOpenModule = null;
		}
		global::KGFGUIUtility.EndVerticalPadding();
		global::KGFGUIUtility.EndHorizontalBox();
		global::KGFGUIUtility.EndHorizontalPadding();
		global::KGFGUIUtility.EndVerticalBox();
		global::UnityEngine.GUILayout.FlexibleSpace();
		global::UnityEngine.GUILayout.EndHorizontal();
	}

	private const string itsCopyrightText = "KOLMICH Creations e.U. is a small company based out of Vienna, Austria.\nWhile developing cool unity3d projects we put an immense amount of time \nto create professional tools and professional content. \n\n\nIf you have any ideas on improvements or you just want to give us some feedback use one of the links below.";

	private global::System.Version itsCurrentVersion;

	private global::System.Version itsMinimumCoreVersion;

	private static global::KGFModule itsOpenModule;
}
