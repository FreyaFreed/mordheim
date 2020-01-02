using System;
using UnityEngine;

public class KGFGUIUtilityTutorial : global::UnityEngine.MonoBehaviour
{
	private void OnGUI()
	{
		int num = 300;
		int num2 = 250;
		global::UnityEngine.Rect screenRect = new global::UnityEngine.Rect((float)((global::UnityEngine.Screen.width - num) / 2), (float)((global::UnityEngine.Screen.height - num2) / 2), (float)num, (float)num2);
		global::UnityEngine.GUILayout.BeginArea(screenRect);
		global::KGFGUIUtility.BeginVerticalBox(global::KGFGUIUtility.eStyleBox.eBoxInvisible, new global::UnityEngine.GUILayoutOption[]
		{
			global::UnityEngine.GUILayout.ExpandHeight(true)
		});
		global::KGFGUIUtility.BeginHorizontalBox(global::KGFGUIUtility.eStyleBox.eBoxDarkTop, new global::UnityEngine.GUILayoutOption[0]);
		global::UnityEngine.GUILayout.FlexibleSpace();
		global::KGFGUIUtility.Label("KGFGUIUtility Tutorial", global::KGFGUIUtility.eStyleLabel.eLabel, new global::UnityEngine.GUILayoutOption[0]);
		global::UnityEngine.GUILayout.FlexibleSpace();
		global::KGFGUIUtility.EndHorizontalBox();
		global::KGFGUIUtility.BeginVerticalBox(global::KGFGUIUtility.eStyleBox.eBoxMiddleVertical, new global::UnityEngine.GUILayoutOption[]
		{
			global::UnityEngine.GUILayout.ExpandHeight(true)
		});
		global::UnityEngine.GUILayout.FlexibleSpace();
		global::KGFGUIUtility.BeginHorizontalPadding();
		global::KGFGUIUtility.Button("Top", global::KGFGUIUtility.eStyleButton.eButtonTop, new global::UnityEngine.GUILayoutOption[]
		{
			global::UnityEngine.GUILayout.ExpandWidth(true)
		});
		global::KGFGUIUtility.Button("Middle", global::KGFGUIUtility.eStyleButton.eButtonMiddle, new global::UnityEngine.GUILayoutOption[]
		{
			global::UnityEngine.GUILayout.ExpandWidth(true)
		});
		global::KGFGUIUtility.Button("Bottom", global::KGFGUIUtility.eStyleButton.eButtonBottom, new global::UnityEngine.GUILayoutOption[]
		{
			global::UnityEngine.GUILayout.ExpandWidth(true)
		});
		global::KGFGUIUtility.EndHorizontalPadding();
		global::UnityEngine.GUILayout.FlexibleSpace();
		global::KGFGUIUtility.EndVerticalBox();
		global::KGFGUIUtility.BeginHorizontalBox(global::KGFGUIUtility.eStyleBox.eBoxDarkBottom, new global::UnityEngine.GUILayoutOption[0]);
		global::KGFGUIUtility.BeginVerticalPadding();
		global::KGFGUIUtility.Button("Left", global::KGFGUIUtility.eStyleButton.eButtonLeft, new global::UnityEngine.GUILayoutOption[]
		{
			global::UnityEngine.GUILayout.ExpandWidth(true)
		});
		global::KGFGUIUtility.Button("Center", global::KGFGUIUtility.eStyleButton.eButtonMiddle, new global::UnityEngine.GUILayoutOption[]
		{
			global::UnityEngine.GUILayout.ExpandWidth(true)
		});
		global::KGFGUIUtility.Button("Right", global::KGFGUIUtility.eStyleButton.eButtonRight, new global::UnityEngine.GUILayoutOption[]
		{
			global::UnityEngine.GUILayout.ExpandWidth(true)
		});
		global::KGFGUIUtility.EndVerticalPadding();
		global::KGFGUIUtility.EndHorizontalBox();
		global::KGFGUIUtility.EndVerticalBox();
		global::UnityEngine.GUILayout.EndArea();
	}
}
