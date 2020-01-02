using System;
using System.Collections.Generic;
using UnityEngine;

public class KGFCustomGUI : global::KGFModule
{
	public KGFCustomGUI() : base(new global::System.Version(1, 0, 0, 1), new global::System.Version(1, 0, 0, 0))
	{
	}

	public static global::UnityEngine.Rect GetItsWindowRectangle()
	{
		return global::KGFCustomGUI.itsWindowRectangle;
	}

	protected override void KGFAwake()
	{
		base.KGFAwake();
		if (global::KGFCustomGUI.itsInstance == null)
		{
			global::KGFCustomGUI.itsInstance = this;
		}
		else
		{
			global::UnityEngine.Object.Destroy(this);
		}
		global::KGFAccessor.RegisterAddEvent<global::KGFICustomGUI>(new global::System.Action<object, global::System.EventArgs>(this.OnCustomGuiChanged));
		global::KGFAccessor.RegisterRemoveEvent<global::KGFICustomGUI>(new global::System.Action<object, global::System.EventArgs>(this.OnCustomGuiChanged));
		this.UpdateInternalList();
	}

	private void OnCustomGuiChanged(object theSender, global::System.EventArgs theArgs)
	{
		this.UpdateInternalList();
	}

	private void UpdateInternalList()
	{
		global::KGFCustomGUI.itsCustomGuiList = global::KGFAccessor.GetObjects<global::KGFICustomGUI>();
	}

	protected void OnGUI()
	{
		global::KGFCustomGUI.Render();
	}

	protected void Update()
	{
		if ((global::UnityEngine.Input.GetKey(this.itsDataModuleCustomGUI.itsModifierKey) && global::UnityEngine.Input.GetKeyDown(this.itsDataModuleCustomGUI.itsSchortcutKey)) || (this.itsDataModuleCustomGUI.itsModifierKey == global::UnityEngine.KeyCode.None && global::UnityEngine.Input.GetKeyDown(this.itsDataModuleCustomGUI.itsSchortcutKey)))
		{
			this.itsDataModuleCustomGUI.itsBarVisible = !this.itsDataModuleCustomGUI.itsBarVisible;
		}
	}

	public static void Render()
	{
		global::KGFGUIUtility.SetSkinIndex(0);
		if (global::KGFCustomGUI.itsInstance != null && global::KGFCustomGUI.itsInstance.itsDataModuleCustomGUI.itsBarVisible)
		{
			global::UnityEngine.GUIStyle styleToggl = global::KGFGUIUtility.GetStyleToggl(global::KGFGUIUtility.eStyleToggl.eTogglRadioStreched);
			global::UnityEngine.GUIStyle styleBox = global::KGFGUIUtility.GetStyleBox(global::KGFGUIUtility.eStyleBox.eBoxDecorated);
			int num = (int)(styleToggl.contentOffset.x + (float)styleToggl.padding.horizontal + (global::KGFGUIUtility.GetSkinHeight() - (float)styleToggl.padding.vertical));
			int num2 = (int)((float)(styleBox.margin.top + styleBox.margin.bottom + styleBox.padding.top + styleBox.padding.bottom) + (styleToggl.fixedHeight + (float)styleToggl.margin.top) * (float)global::KGFCustomGUI.itsCustomGuiList.Count);
			global::UnityEngine.GUILayout.BeginArea(new global::UnityEngine.Rect((float)(global::UnityEngine.Screen.width - num), (float)((global::UnityEngine.Screen.height - num2) / 2), (float)num, (float)num2));
			global::KGFGUIUtility.BeginVerticalBox(global::KGFGUIUtility.eStyleBox.eBoxDecorated, new global::UnityEngine.GUILayoutOption[]
			{
				global::UnityEngine.GUILayout.ExpandWidth(true),
				global::UnityEngine.GUILayout.ExpandHeight(true)
			});
			global::UnityEngine.GUILayout.FlexibleSpace();
			foreach (global::KGFICustomGUI kgficustomGUI in global::KGFCustomGUI.itsCustomGuiList)
			{
				bool flag = global::KGFCustomGUI.itsCurrentSelectedGUI != null && global::KGFCustomGUI.itsCurrentSelectedGUI == kgficustomGUI;
				global::UnityEngine.Texture2D texture2D = kgficustomGUI.GetIcon();
				if (texture2D == null)
				{
					texture2D = global::KGFCustomGUI.itsInstance.itsDataModuleCustomGUI.itsUnknownIcon;
				}
				if (flag != global::KGFGUIUtility.Toggle(flag, texture2D, global::KGFGUIUtility.eStyleToggl.eTogglRadioStreched, new global::UnityEngine.GUILayoutOption[0]))
				{
					if (flag)
					{
						global::KGFCustomGUI.itsCurrentSelectedGUI = null;
					}
					else
					{
						global::KGFCustomGUI.itsCurrentSelectedGUI = kgficustomGUI;
					}
				}
			}
			global::UnityEngine.GUILayout.FlexibleSpace();
			global::KGFGUIUtility.EndVerticalBox();
			global::UnityEngine.GUILayout.EndArea();
			global::KGFCustomGUI.itsInstance.DrawCurrentCustomGUI((float)num);
		}
		global::KGFGUIUtility.SetSkinIndex(1);
	}

	private static global::KGFCustomGUI GetInstance()
	{
		return global::KGFCustomGUI.itsInstance;
	}

	private void DrawCurrentCustomGUI(float aCustomGuiWidth)
	{
		if (global::KGFCustomGUI.itsCurrentSelectedGUI == null)
		{
			return;
		}
		float num = global::KGFGUIUtility.GetSkinHeight() + (float)global::KGFGUIUtility.GetStyleButton(global::KGFGUIUtility.eStyleButton.eButton).margin.vertical + (float)global::KGFGUIUtility.GetStyleBox(global::KGFGUIUtility.eStyleBox.eBoxDecorated).padding.vertical;
		global::UnityEngine.GUILayout.BeginArea(new global::UnityEngine.Rect(num, num, (float)global::UnityEngine.Screen.width - aCustomGuiWidth - num, (float)global::UnityEngine.Screen.height - num * 2f));
		global::KGFGUIUtility.BeginVerticalBox(global::KGFGUIUtility.eStyleBox.eBox, new global::UnityEngine.GUILayoutOption[0]);
		if (global::KGFCustomGUI.itsCurrentSelectedGUI.GetIcon() == null)
		{
			global::KGFGUIUtility.BeginWindowHeader(global::KGFCustomGUI.itsCurrentSelectedGUI.GetHeaderName(), this.itsDataModuleCustomGUI.itsUnknownIcon);
		}
		else
		{
			global::KGFGUIUtility.BeginWindowHeader(global::KGFCustomGUI.itsCurrentSelectedGUI.GetHeaderName(), global::KGFCustomGUI.itsCurrentSelectedGUI.GetIcon());
		}
		global::UnityEngine.GUILayout.FlexibleSpace();
		if (!global::KGFGUIUtility.EndWindowHeader(true))
		{
			global::UnityEngine.GUILayout.Space(0f);
			global::KGFCustomGUI.itsCurrentSelectedGUI.Render();
		}
		else
		{
			global::KGFCustomGUI.itsCurrentSelectedGUI = null;
		}
		global::KGFGUIUtility.EndVerticalBox();
		global::UnityEngine.GUILayout.EndArea();
	}

	public static global::UnityEngine.Texture2D GetDefaultIcon()
	{
		if (global::KGFCustomGUI.itsInstance != null)
		{
			return global::KGFCustomGUI.itsInstance.itsDataModuleCustomGUI.itsUnknownIcon;
		}
		return null;
	}

	public override global::UnityEngine.Texture2D GetIcon()
	{
		return null;
	}

	public override string GetName()
	{
		return "KGFCustomGUI";
	}

	public override string GetDocumentationPath()
	{
		return "KGFCustomGUIManual.html";
	}

	public override string GetForumPath()
	{
		return string.Empty;
	}

	public override global::KGFMessageList Validate()
	{
		global::KGFMessageList kgfmessageList = new global::KGFMessageList();
		if (this.itsDataModuleCustomGUI.itsUnknownIcon == null)
		{
			kgfmessageList.AddWarning("the unknown icon is missing");
		}
		if (this.itsDataModuleCustomGUI.itsModifierKey == this.itsDataModuleCustomGUI.itsSchortcutKey)
		{
			kgfmessageList.AddInfo("the modifier key is equal to the shortcut key");
		}
		return kgfmessageList;
	}

	private static global::KGFCustomGUI itsInstance = null;

	public global::KGFCustomGUI.KGFDataCustomGUI itsDataModuleCustomGUI = new global::KGFCustomGUI.KGFDataCustomGUI();

	private static global::System.Collections.Generic.List<global::KGFICustomGUI> itsCustomGuiList = null;

	private static global::KGFICustomGUI itsCurrentSelectedGUI = null;

	private static global::UnityEngine.Rect itsWindowRectangle = new global::UnityEngine.Rect(50f, 50f, 800f, 600f);

	[global::System.Serializable]
	public class KGFDataCustomGUI
	{
		public global::UnityEngine.Texture2D itsUnknownIcon;

		public global::UnityEngine.KeyCode itsModifierKey;

		public global::UnityEngine.KeyCode itsSchortcutKey = global::UnityEngine.KeyCode.F3;

		public bool itsBarVisible = true;
	}
}
