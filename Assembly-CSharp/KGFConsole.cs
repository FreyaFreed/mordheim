using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;

public class KGFConsole : global::KGFModule
{
	public KGFConsole() : base(new global::System.Version(1, 2, 0, 0), new global::System.Version(1, 2, 0, 0))
	{
	}

	protected override void KGFAwake()
	{
		if (global::KGFConsole.itsInstance == null)
		{
			global::KGFConsole.itsInstance = this;
			global::KGFConsole.itsInstance.Init();
			return;
		}
		if (global::KGFConsole.itsInstance != this)
		{
			global::UnityEngine.Debug.Log("there is more than one KFGDebug instance in this scene. please ensure there is always exactly one instance in this scene");
			global::UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	protected void OnGUI()
	{
		global::KGFConsole.Render();
	}

	public static void Render()
	{
		global::KGFGUIUtility.SetSkinIndex(0);
		if (global::KGFConsole.itsInstance != null)
		{
			if (!global::KGFConsole.itsInstance.itsDataModuleConsole.itsVisible)
			{
				return;
			}
			if (global::KGFConsole.itsInstance.itsUnfocus)
			{
				global::UnityEngine.GUI.FocusControl(string.Empty);
				global::KGFConsole.itsInstance.itsUnfocus = false;
			}
			global::KGFModule.RenderHelpWindow();
			if (global::KGFConsole.itsInstance.itsOpen)
			{
				global::KGFConsole.itsInstance.itsOpenWindow.x = 0f;
				global::KGFConsole.itsInstance.itsOpenWindow.y = (float)global::UnityEngine.Screen.height - global::KGFConsole.itsInstance.itsCurrentHeight;
				global::KGFConsole.itsInstance.itsOpenWindow.width = (float)global::UnityEngine.Screen.width;
				global::KGFConsole.itsInstance.itsOpenWindow.height = global::KGFConsole.itsInstance.itsCurrentHeight;
				if (global::KGFConsole.itsInstance.itsCurrentHeight < global::KGFGUIUtility.GetSkinHeight() * 11f)
				{
					global::KGFConsole.itsInstance.itsCurrentHeight = global::KGFGUIUtility.GetSkinHeight() * 11f;
				}
				else if (global::KGFConsole.itsInstance.itsCurrentHeight > (float)global::UnityEngine.Screen.height / 3f * 2f)
				{
					global::KGFConsole.itsInstance.itsCurrentHeight = (float)global::UnityEngine.Screen.height / 3f * 2f;
				}
				global::UnityEngine.GUILayout.BeginArea(global::KGFConsole.itsInstance.itsOpenWindow);
				global::KGFGUIUtility.BeginVerticalBox(global::KGFGUIUtility.eStyleBox.eBoxDecorated, new global::UnityEngine.GUILayoutOption[]
				{
					global::UnityEngine.GUILayout.ExpandWidth(true)
				});
				global::UnityEngine.GUILayout.BeginHorizontal(new global::UnityEngine.GUILayoutOption[0]);
				global::KGFGUIUtility.BeginVerticalBox(global::KGFGUIUtility.eStyleBox.eBoxLeft, new global::UnityEngine.GUILayoutOption[]
				{
					global::UnityEngine.GUILayout.Width((float)global::UnityEngine.Screen.width * 0.2f)
				});
				global::KGFGUIUtility.BeginHorizontalPadding();
				global::KGFConsole.itsInstance.DrawCategoryColumn();
				global::KGFGUIUtility.EndHorizontalPadding();
				global::KGFGUIUtility.EndVerticalBox();
				global::KGFGUIUtility.BeginVerticalBox(global::KGFGUIUtility.eStyleBox.eBoxRight, new global::UnityEngine.GUILayoutOption[0]);
				global::KGFGUIUtility.BeginHorizontalPadding();
				global::KGFGUIUtility.BeginHorizontalBox(global::KGFGUIUtility.eStyleBox.eBoxDarkTop, new global::UnityEngine.GUILayoutOption[]
				{
					global::UnityEngine.GUILayout.ExpandWidth(true)
				});
				global::KGFGUIUtility.Label("click on entry to see details", new global::UnityEngine.GUILayoutOption[0]);
				global::KGFGUIUtility.EndHorizontalBox();
				global::KGFGUIUtility.BeginHorizontalBox(global::KGFGUIUtility.eStyleBox.eBoxMiddleVertical, new global::UnityEngine.GUILayoutOption[]
				{
					global::UnityEngine.GUILayout.ExpandWidth(true)
				});
				global::KGFGUIUtility.BeginVerticalPadding();
				global::KGFConsole.itsInstance.itsTableControl.Render();
				global::KGFGUIUtility.EndVerticalPadding();
				global::KGFGUIUtility.EndHorizontalBox();
				global::KGFGUIUtility.BeginHorizontalBox(global::KGFGUIUtility.eStyleBox.eBoxDarkBottom, new global::UnityEngine.GUILayoutOption[]
				{
					global::UnityEngine.GUILayout.ExpandWidth(true)
				});
				global::KGFConsole.itsInstance.DrawFilterSearchBar();
				global::KGFGUIUtility.EndHorizontalBox();
				global::KGFGUIUtility.EndHorizontalPadding();
				global::KGFGUIUtility.EndVerticalBox();
				global::UnityEngine.GUILayout.EndHorizontal();
				global::UnityEngine.GUILayout.Space(10f);
				global::KGFGUIUtility.BeginWindowHeader("KGFConsole", global::KGFConsole.itsInstance.GetIcon());
				global::KGFConsole.itsInstance.DrawFooterControls();
				global::UnityEngine.GUILayout.FlexibleSpace();
				float num = global::KGFConsole.itsInstance.itsCurrentHeight;
				if (global::KGFConsole.itsInstance.itsIsMobile)
				{
					global::KGFConsole.itsInstance.itsCurrentHeight = (float)global::UnityEngine.Screen.height * 0.9f;
				}
				else
				{
					global::KGFConsole.itsInstance.itsCurrentHeight = global::KGFGUIUtility.HorizontalSlider(global::KGFConsole.itsInstance.itsCurrentHeight, (float)global::UnityEngine.Screen.height / 3f * 2f, global::KGFGUIUtility.GetSkinHeight() * 11f, new global::UnityEngine.GUILayoutOption[]
					{
						global::UnityEngine.GUILayout.Width(75f)
					});
					if (num != global::KGFConsole.itsInstance.itsCurrentHeight)
					{
						global::KGFConsole.itsInstance.SaveSizeToPlayerPrefs();
					}
				}
				if (!global::KGFConsole.itsInstance.itsIsMobile && global::KGFGUIUtility.Button(global::KGFConsole.itsInstance.itsDataModuleConsole.itsIconHelp, global::KGFGUIUtility.eStyleButton.eButton, new global::UnityEngine.GUILayoutOption[0]))
				{
					global::KGFModule.OpenHelpWindow(global::KGFConsole.itsInstance);
				}
				global::KGFConsole.itsInstance.itsOpen = global::KGFGUIUtility.Toggle(global::KGFConsole.itsInstance.itsOpen, string.Empty, global::KGFGUIUtility.eStyleToggl.eTogglSwitch, new global::UnityEngine.GUILayoutOption[]
				{
					global::UnityEngine.GUILayout.Width(global::KGFGUIUtility.GetSkinHeight())
				});
				global::KGFGUIUtility.EndWindowHeader(false);
				global::KGFGUIUtility.EndVerticalBox();
				global::UnityEngine.GUILayout.EndArea();
			}
			else
			{
				global::KGFConsole.itsInstance.DrawMinimizedWindow();
			}
			global::KGFGUIUtility.RenderDropDownList();
			global::KGFConsole.itsInstance.itsSuggestions.Render();
			if (!global::KGFConsole.itsInstance.itsIsMobile)
			{
				global::UnityEngine.Vector3 mousePosition = global::UnityEngine.Input.mousePosition;
				mousePosition.y = (float)global::UnityEngine.Screen.height - mousePosition.y;
				if (global::KGFConsole.itsInstance.itsOpen)
				{
					if (global::KGFConsole.itsInstance.itsOpenWindow.Contains(mousePosition))
					{
						if (!global::KGFConsole.itsInstance.itsHover)
						{
							global::KGFConsole.itsInstance.itsHover = true;
							global::UnityEngine.Cursor.visible = false;
						}
					}
					else if (global::KGFConsole.itsInstance.itsHover && !global::KGFConsole.itsInstance.itsOpenWindow.Contains(mousePosition))
					{
						global::KGFConsole.itsInstance.itsHover = false;
					}
				}
				else if (!global::KGFConsole.itsInstance.itsOpen)
				{
					if (global::KGFConsole.itsInstance.itsMinimizedWindow.Contains(mousePosition))
					{
						if (!global::KGFConsole.itsInstance.itsHover)
						{
							global::KGFConsole.itsInstance.itsHover = true;
						}
					}
					else if (global::KGFConsole.itsInstance.itsHover && !global::KGFConsole.itsInstance.itsMinimizedWindow.Contains(mousePosition))
					{
						global::KGFConsole.itsInstance.itsHover = false;
					}
				}
			}
			if (global::UnityEngine.GUI.GetNameOfFocusedControl() == "commandCommand" || global::UnityEngine.GUI.GetNameOfFocusedControl().Equals("messageSearch") || global::UnityEngine.GUI.GetNameOfFocusedControl().Equals("categorySearch"))
			{
				global::KGFConsole.itsInstance.itsFocus = true;
			}
			else
			{
				global::KGFConsole.itsInstance.itsFocus = false;
			}
			global::UnityEngine.GUI.SetNextControlName(string.Empty);
			if ((global::KGFConsole.itsInstance.itsModifierKeyFocusPressed || global::KGFConsole.itsInstance.itsDataModuleConsole.itsModifierKeyFocus == global::UnityEngine.KeyCode.None) && global::KGFConsole.itsInstance.itsShortcutKeyFocusPressed)
			{
				global::KGFConsole.itsInstance.itsModifierKeyFocusPressed = false;
				global::KGFConsole.itsInstance.itsShortcutKeyFocusPressed = false;
				global::UnityEngine.GUI.FocusControl("commandCommand");
			}
		}
		global::KGFGUIUtility.SetSkinIndex(1);
	}

	public static void Expand()
	{
		if (global::KGFConsole.itsInstance != null)
		{
			global::KGFConsole.itsInstance.itsOpen = true;
		}
	}

	public static void Minimize()
	{
		if (global::KGFConsole.itsInstance != null)
		{
			global::KGFConsole.itsInstance.itsOpen = false;
		}
	}

	private void DrawMinimizedWindow()
	{
		float num = global::KGFGUIUtility.GetSkinHeight() + (float)global::KGFGUIUtility.GetStyleButton(global::KGFGUIUtility.eStyleButton.eButton).margin.vertical + (float)global::KGFGUIUtility.GetStyleBox(global::KGFGUIUtility.eStyleBox.eBoxDecorated).padding.vertical;
		this.itsMinimizedWindow.width = (float)global::UnityEngine.Screen.width;
		this.itsMinimizedWindow.y = (float)global::UnityEngine.Screen.height - num;
		this.itsMinimizedWindow.height = num;
		global::UnityEngine.GUILayout.BeginArea(this.itsMinimizedWindow);
		global::UnityEngine.GUILayout.BeginVertical(new global::UnityEngine.GUILayoutOption[0]);
		global::UnityEngine.GUILayout.BeginHorizontal(new global::UnityEngine.GUILayoutOption[0]);
		global::KGFGUIUtility.BeginVerticalBox(global::KGFGUIUtility.eStyleBox.eBoxDecorated, new global::UnityEngine.GUILayoutOption[0]);
		global::KGFGUIUtility.BeginWindowHeader("KGFConsole", this.GetIcon());
		this.DrawFooterControls();
		global::UnityEngine.GUILayout.FlexibleSpace();
		if (!global::KGFConsole.itsInstance.itsIsMobile && global::KGFGUIUtility.Button(this.itsDataModuleConsole.itsIconHelp, global::KGFGUIUtility.eStyleButton.eButton, new global::UnityEngine.GUILayoutOption[0]))
		{
			global::KGFModule.OpenHelpWindow(this);
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

	protected void Update()
	{
		if (this.itsDataModuleConsole.itsVisible)
		{
			global::UnityEngine.Cursor.visible = true;
		}
		if (this.itsDataModuleConsole.itsHideKey != global::UnityEngine.KeyCode.None && !global::UnityEngine.Input.GetKey(this.itsDataModuleConsole.itsExpandKeyModifier) && ((global::UnityEngine.Input.GetKey(this.itsDataModuleConsole.itsHideKeyModifier) && global::UnityEngine.Input.GetKey(this.itsDataModuleConsole.itsHideKeyModifier) && global::UnityEngine.Input.GetKeyDown(this.itsDataModuleConsole.itsHideKey)) || (this.itsDataModuleConsole.itsHideKeyModifier == global::UnityEngine.KeyCode.None && global::UnityEngine.Input.GetKeyDown(this.itsDataModuleConsole.itsHideKey))))
		{
			this.itsDataModuleConsole.itsVisible = !this.itsDataModuleConsole.itsVisible;
			if (!this.itsDataModuleConsole.itsVisible)
			{
				this.itsUnfocus = true;
			}
		}
		if (this.itsDataModuleConsole.itsExpandKey != global::UnityEngine.KeyCode.None && !global::UnityEngine.Input.GetKey(this.itsDataModuleConsole.itsHideKeyModifier) && ((global::UnityEngine.Input.GetKey(this.itsDataModuleConsole.itsExpandKeyModifier) && global::UnityEngine.Input.GetKeyDown(this.itsDataModuleConsole.itsExpandKey)) || (this.itsDataModuleConsole.itsExpandKeyModifier == global::UnityEngine.KeyCode.None && global::UnityEngine.Input.GetKeyDown(this.itsDataModuleConsole.itsExpandKey))))
		{
			this.itsOpen = !this.itsOpen;
			if (this.itsOpen)
			{
				this.itsDataModuleConsole.itsVisible = true;
			}
		}
		if (global::UnityEngine.Input.GetKeyUp(this.itsDataModuleConsole.itsModifierKeyFocus))
		{
			this.itsModifierKeyFocusPressed = true;
		}
		if (global::UnityEngine.Input.GetKeyUp(this.itsDataModuleConsole.itsSchortcutKeyFocus))
		{
			this.itsShortcutKeyFocusPressed = true;
		}
	}

	private void ReactoOnEnterEscape()
	{
		if (global::KGFConsole.itsInstance == null)
		{
			return;
		}
		if (global::UnityEngine.GUI.GetNameOfFocusedControl() == "commandCommand")
		{
			global::UnityEngine.Event current = global::UnityEngine.Event.current;
			if (current == null)
			{
				return;
			}
			this.itsShortcutKeyFocusPressed = false;
			this.itsModifierKeyFocusPressed = false;
			if (current.type == global::UnityEngine.EventType.KeyDown && current.keyCode == global::UnityEngine.KeyCode.Return)
			{
				if (global::KGFConsole.GetFocused() && global::KGFConsole.itsInstance.itsSuggestions.GetCount() > 0)
				{
					this.itsCommand = global::KGFConsole.itsInstance.itsSuggestions.GetSelected();
					this.ExecuteCommand();
				}
				else
				{
					this.ExecuteCommand();
				}
			}
			if (current.type == global::UnityEngine.EventType.KeyDown && current.keyCode == global::UnityEngine.KeyCode.Escape)
			{
				this.itsCommand = string.Empty;
				global::UnityEngine.GUI.FocusControl(string.Empty);
			}
			if (current.type == global::UnityEngine.EventType.KeyDown && current.keyCode == global::UnityEngine.KeyCode.UpArrow)
			{
				global::KGFConsole.itsInstance.itsSuggestions.SelectionUp();
			}
			if (current.type == global::UnityEngine.EventType.KeyDown && current.keyCode == global::UnityEngine.KeyCode.DownArrow)
			{
				global::KGFConsole.itsInstance.itsSuggestions.SelectionDown();
			}
		}
	}

	private void DrawFooterControls()
	{
		global::KGFGUIUtility.BeginHorizontalBox(global::KGFGUIUtility.eStyleBox.eBoxInvisible, new global::UnityEngine.GUILayoutOption[0]);
		global::UnityEngine.GUI.SetNextControlName("commandCommand");
		string b = this.itsCommand;
		this.ReactoOnEnterEscape();
		this.itsCommand = global::KGFGUIUtility.TextField(this.itsCommand, global::KGFGUIUtility.eStyleTextField.eTextFieldLeft, new global::UnityEngine.GUILayoutOption[]
		{
			global::UnityEngine.GUILayout.Width((float)global::UnityEngine.Screen.width * 0.125f)
		});
		if (this.itsCommand != b)
		{
			if (this.itsCommand != string.Empty)
			{
				int num = 0;
				global::System.Collections.Generic.List<string> list = new global::System.Collections.Generic.List<string>();
				foreach (string text in this.itsCommandDictionary.Keys)
				{
					if (text.ToLower().StartsWith(this.itsCommand.ToLower()))
					{
						list.Add(text);
						num++;
					}
					if ((long)num > (long)((ulong)global::KGFConsole.itsInstance.itsSuggestions.GetMaxCount()))
					{
						break;
					}
				}
				global::KGFConsole.itsInstance.itsSuggestions.SetItems(list);
			}
			else
			{
				global::KGFConsole.itsInstance.itsSuggestions.ClearItems();
			}
		}
		this.itsSuggestions.SetWidth((float)global::UnityEngine.Screen.width * 0.125f);
		if (global::UnityEngine.Event.current.type == global::UnityEngine.EventType.Repaint)
		{
			global::UnityEngine.Rect lastRect = global::UnityEngine.GUILayoutUtility.GetLastRect();
			if (!global::KGFConsole.itsInstance.itsOpen)
			{
				this.itsSuggestions.SetPosition(lastRect.x, (float)global::UnityEngine.Screen.height - lastRect.y);
			}
			else
			{
				this.itsSuggestions.SetPosition(lastRect.x, (float)global::UnityEngine.Screen.height - lastRect.y + global::KGFConsole.itsInstance.itsCurrentHeight - lastRect.height - global::KGFGUIUtility.GetStyleTextField(global::KGFGUIUtility.eStyleTextField.eTextFieldLeft).fixedHeight - (float)global::KGFGUIUtility.GetStyleTextField(global::KGFGUIUtility.eStyleTextField.eTextFieldLeft).padding.top);
			}
		}
		this.itsDropDownLastFive.itsWidth = (uint)((float)global::UnityEngine.Screen.width * 0.125f);
		this.itsDropDownFavouriteFive.itsWidth = (uint)((float)global::UnityEngine.Screen.width * 0.125f);
		if (global::KGFGUIUtility.Button(this.itsDataModuleConsole.itsIconExecute, "execute", global::KGFGUIUtility.eStyleButton.eButtonRight, new global::UnityEngine.GUILayoutOption[0]))
		{
			this.ExecuteCommand();
		}
		if (global::UnityEngine.Time.time > this.itsLastExecution + 0.5f)
		{
			this.itsLastExecution = 0f;
			this.itsCommandState = global::KGFConsole.eExecutionState.eNone;
		}
		switch (this.itsCommandState)
		{
		case global::KGFConsole.eExecutionState.eNone:
			global::UnityEngine.GUILayout.Space(8f);
			global::KGFGUIUtility.Label(string.Empty, this.itsDataModuleConsole.itsIconUnknown, global::KGFGUIUtility.eStyleLabel.eLabel, new global::UnityEngine.GUILayoutOption[0]);
			break;
		case global::KGFConsole.eExecutionState.eSuccessful:
			global::UnityEngine.GUILayout.Space(8f);
			global::KGFGUIUtility.Label(string.Empty, this.itsDataModuleConsole.itsIconSuccessful, global::KGFGUIUtility.eStyleLabel.eLabel, new global::UnityEngine.GUILayoutOption[0]);
			break;
		case global::KGFConsole.eExecutionState.eNotFound:
			global::UnityEngine.GUILayout.Space(8f);
			global::KGFGUIUtility.Label(string.Empty, this.itsDataModuleConsole.itsIconFatal, global::KGFGUIUtility.eStyleLabel.eLabel, new global::UnityEngine.GUILayoutOption[0]);
			break;
		case global::KGFConsole.eExecutionState.eError:
			global::UnityEngine.GUILayout.Space(8f);
			global::KGFGUIUtility.Label(string.Empty, this.itsDataModuleConsole.itsIconError, global::KGFGUIUtility.eStyleLabel.eLabel, new global::UnityEngine.GUILayoutOption[0]);
			break;
		}
		global::UnityEngine.GUILayout.Space(10f);
		global::KGFConsole.itsInstance.itsDropDownLastFive.Render();
		if (global::UnityEngine.Event.current.type == global::UnityEngine.EventType.Repaint && !this.itsOpen)
		{
			global::KGFGUIDropDown kgfguidropDown = global::KGFConsole.itsInstance.itsDropDownLastFive;
			kgfguidropDown.itsLastRect.x = kgfguidropDown.itsLastRect.x + this.itsMinimizedWindow.x;
			global::KGFGUIDropDown kgfguidropDown2 = global::KGFConsole.itsInstance.itsDropDownLastFive;
			kgfguidropDown2.itsLastRect.y = kgfguidropDown2.itsLastRect.y + this.itsMinimizedWindow.y;
		}
		else if (global::UnityEngine.Event.current.type == global::UnityEngine.EventType.Repaint && this.itsOpen)
		{
			global::KGFGUIDropDown kgfguidropDown3 = global::KGFConsole.itsInstance.itsDropDownLastFive;
			kgfguidropDown3.itsLastRect.x = kgfguidropDown3.itsLastRect.x + this.itsOpenWindow.x;
			global::KGFGUIDropDown kgfguidropDown4 = global::KGFConsole.itsInstance.itsDropDownLastFive;
			kgfguidropDown4.itsLastRect.y = kgfguidropDown4.itsLastRect.y + this.itsOpenWindow.y;
		}
		else if (global::UnityEngine.Event.current.type == global::UnityEngine.EventType.Layout && global::KGFGUIDropDown.itsOpenInstance != null)
		{
			global::KGFGUIDropDown.itsCorrectedOffset = true;
		}
		global::UnityEngine.GUILayout.Space(10f);
		global::KGFConsole.itsInstance.itsDropDownFavouriteFive.Render();
		if (global::UnityEngine.Event.current.type == global::UnityEngine.EventType.Repaint && !this.itsOpen)
		{
			global::KGFGUIDropDown kgfguidropDown5 = global::KGFConsole.itsInstance.itsDropDownFavouriteFive;
			kgfguidropDown5.itsLastRect.x = kgfguidropDown5.itsLastRect.x + this.itsMinimizedWindow.x;
			global::KGFGUIDropDown kgfguidropDown6 = global::KGFConsole.itsInstance.itsDropDownFavouriteFive;
			kgfguidropDown6.itsLastRect.y = kgfguidropDown6.itsLastRect.y + this.itsMinimizedWindow.y;
		}
		else if (global::UnityEngine.Event.current.type == global::UnityEngine.EventType.Repaint && this.itsOpen)
		{
			global::KGFGUIDropDown kgfguidropDown7 = global::KGFConsole.itsInstance.itsDropDownFavouriteFive;
			kgfguidropDown7.itsLastRect.x = kgfguidropDown7.itsLastRect.x + this.itsOpenWindow.x;
			global::KGFGUIDropDown kgfguidropDown8 = global::KGFConsole.itsInstance.itsDropDownFavouriteFive;
			kgfguidropDown8.itsLastRect.y = kgfguidropDown8.itsLastRect.y + this.itsOpenWindow.y;
		}
		else if (global::UnityEngine.Event.current.type == global::UnityEngine.EventType.Layout && global::KGFGUIDropDown.itsOpenInstance != null)
		{
			global::KGFGUIDropDown.itsCorrectedOffset = true;
		}
		if (!global::KGFConsole.itsInstance.itsIsMobile)
		{
			global::UnityEngine.GUILayout.Space(10f);
		}
		global::KGFGUIUtility.EndHorizontalBox();
		global::UnityEngine.GUILayout.FlexibleSpace();
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
			foreach (global::KGFConsole.KGFCcommandCategory kgfccommandCategory in this.GetAllCategories())
			{
				kgfccommandCategory.itsSelectedState = true;
				flag = true;
			}
		}
		if (global::KGFGUIUtility.Button("None", global::KGFGUIUtility.eStyleButton.eButtonRight, new global::UnityEngine.GUILayoutOption[]
		{
			global::UnityEngine.GUILayout.ExpandWidth(true)
		}))
		{
			foreach (global::KGFConsole.KGFCcommandCategory kgfccommandCategory2 in this.GetAllCategories())
			{
				kgfccommandCategory2.itsSelectedState = false;
				flag = true;
			}
		}
		global::KGFGUIUtility.EndVerticalPadding();
		global::KGFGUIUtility.EndHorizontalBox();
		global::KGFGUIUtility.BeginHorizontalBox(global::KGFGUIUtility.eStyleBox.eBoxMiddleVertical, new global::UnityEngine.GUILayoutOption[0]);
		this.itsCategoryScrollViewPosition = global::KGFGUIUtility.BeginScrollView(this.itsCategoryScrollViewPosition, false, false, new global::UnityEngine.GUILayoutOption[]
		{
			global::UnityEngine.GUILayout.ExpandWidth(true)
		});
		int num = 0;
		foreach (global::KGFConsole.KGFCcommandCategory kgfccommandCategory3 in this.GetAllCategories())
		{
			if (this.itsSearchFilterCategory.Trim().Equals("Search") || kgfccommandCategory3.GetName().ToLower().Contains(this.itsSearchFilterCategory.ToLower()))
			{
				num++;
				bool flag2 = global::KGFGUIUtility.Toggle(kgfccommandCategory3.itsSelectedState, string.Format("{0} ({1})", kgfccommandCategory3.GetName(), kgfccommandCategory3.GetCount().ToString()), global::KGFGUIUtility.eStyleToggl.eTogglSuperCompact, new global::UnityEngine.GUILayoutOption[0]);
				if (kgfccommandCategory3.itsSelectedState != flag2)
				{
					kgfccommandCategory3.itsSelectedState = flag2;
					flag = true;
				}
			}
		}
		if (num < 1)
		{
			global::KGFGUIUtility.Label("no items found", new global::UnityEngine.GUILayoutOption[0]);
		}
		global::KGFGUIUtility.EndScrollView();
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
			if (!global::UnityEngine.GUI.GetNameOfFocusedControl().Equals("categorySearch"))
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
			this.UpdateCommandListsResult();
		}
	}

	private void DrawFilterSearchBar()
	{
		string a = this.itsSearchFilterMessage;
		bool flag = false;
		global::UnityEngine.GUI.SetNextControlName("messageSearch");
		this.itsSearchFilterMessage = global::KGFGUIUtility.TextField(this.itsSearchFilterMessage, global::KGFGUIUtility.eStyleTextField.eTextField, new global::UnityEngine.GUILayoutOption[]
		{
			global::UnityEngine.GUILayout.Width(150f)
		});
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
			if (!global::UnityEngine.GUI.GetNameOfFocusedControl().Equals("messageSearch"))
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
			this.UpdateCommandListsResult();
		}
	}

	public override string GetName()
	{
		return "KGFConsole";
	}

	public override string GetDocumentationPath()
	{
		return "KGFConsole_Manual.html";
	}

	public override string GetForumPath()
	{
		return "index.php?qa=kgfconsole";
	}

	public override global::UnityEngine.Texture2D GetIcon()
	{
		global::KGFConsole.CheckInstance();
		if (global::KGFConsole.itsInstance != null && global::KGFConsole.itsInstance.itsDataModuleConsole != null)
		{
			return global::KGFConsole.itsInstance.itsDataModuleConsole.itsIconModule;
		}
		return null;
	}

	private static global::KGFConsole GetInstance()
	{
		return global::KGFConsole.itsInstance;
	}

	public static bool GetFocused()
	{
		return global::KGFConsole.itsInstance != null && global::KGFConsole.itsInstance.itsDataModuleConsole.itsVisible && (global::KGFConsole.itsInstance.itsHover || global::KGFConsole.itsInstance.itsFocus);
	}

	public static bool GetHover()
	{
		return global::KGFConsole.itsInstance != null && global::KGFConsole.itsInstance.itsDataModuleConsole.itsVisible && (global::KGFConsole.itsInstance.itsHover || global::KGFConsole.itsInstance.itsDropDownFavouriteFive.GetHover() || global::KGFConsole.itsInstance.itsDropDownLastFive.GetHover());
	}

	public static void AddCommand(string theCommandName, object theObject, string theMethodName)
	{
		global::KGFConsole.AddCommand(theCommandName, string.Empty, "uncategorized", theObject, theMethodName);
	}

	public static void AddCommand(string theCommandName, string theCategory, object theObject, string theMethodName)
	{
		global::KGFConsole.AddCommand(theCommandName, string.Empty, theCategory, theObject, theMethodName);
	}

	public static void AddCommand(string theCommandName, string theDescription, string theCategory, object theObject, string theMethodName)
	{
		global::KGFConsole.CheckInstance();
		if (global::KGFConsole.itsInstance == null)
		{
			return;
		}
		if (!global::KGFConsole.itsInstance.itsCommandDictionary.ContainsKey(theCommandName))
		{
			global::KGFConsole.KGFCommandCode value = new global::KGFConsole.KGFCommandCode(theCommandName, theDescription, theCategory, theObject, theMethodName);
			global::KGFConsole.itsInstance.itsCommandDictionary[theCommandName] = value;
			if (!global::KGFConsole.itsInstance.itsCommandCategories.ContainsKey(theCategory))
			{
				global::KGFConsole.itsInstance.itsCommandCategories.Add(theCategory, new global::KGFConsole.KGFCcommandCategory(theCategory));
				global::KGFConsole.itsInstance.itsCommandCategories[theCategory].IncreaseCount();
			}
			else
			{
				global::KGFConsole.itsInstance.itsCommandCategories[theCategory].IncreaseCount();
			}
			global::KGFConsole.itsInstance.LoadLastFiveFromPlayerPrefs();
			global::KGFConsole.itsInstance.LoadFavouriteFiveFromPlayerPrefs();
			global::KGFConsole.itsInstance.itsDropDownLastFive.SetEntrys(global::KGFConsole.itsInstance.itsLastFiveCommandCodes);
			global::KGFConsole.itsInstance.itsDropDownFavouriteFive.SetEntrys(global::KGFConsole.itsInstance.itsFavouriteCommandCodes.Keys);
			global::KGFConsole.itsInstance.UpdateCommandListsResult();
		}
		else
		{
			global::UnityEngine.Debug.LogError(string.Format("command '{0}' is already registered in the console module.", theCommandName));
		}
	}

	public static void RemoveCommand(string theCommandString)
	{
		global::KGFConsole.CheckInstance();
		if (global::KGFConsole.itsInstance == null)
		{
			return;
		}
		if (global::KGFConsole.itsInstance.itsCommandDictionary.ContainsKey(theCommandString))
		{
			global::KGFConsole.itsInstance.itsCommandDictionary.Remove(theCommandString);
			global::KGFConsole.itsInstance.UpdateCommandListsResult();
		}
	}

	private static void CheckInstance()
	{
		if (global::KGFConsole.itsInstance == null)
		{
			global::UnityEngine.Object @object = global::UnityEngine.Object.FindObjectOfType(typeof(global::KGFConsole));
			if (@object != null)
			{
				global::KGFConsole.itsInstance = (@object as global::KGFConsole);
				global::KGFConsole.itsInstance.Init();
			}
			else if (!global::KGFConsole.itsAlreadyChecked)
			{
				global::UnityEngine.Debug.LogError("KOLMICH Console Module is not running. Make sure that there is an instance of the KGFConsole prefab in the current scene.");
				global::KGFConsole.itsAlreadyChecked = true;
			}
		}
	}

	private void Init()
	{
		if (global::UnityEngine.Application.platform == global::UnityEngine.RuntimePlatform.Android || global::UnityEngine.Application.platform == global::UnityEngine.RuntimePlatform.IPhonePlayer)
		{
			global::KGFConsole.itsInstance.itsIsMobile = true;
			string skinPath = global::KGFGUIUtility.GetSkinPath();
			if (global::UnityEngine.Screen.width >= 800)
			{
				global::KGFGUIUtility.SetSkinPathEditor(skinPath.Replace("_16", "_32"));
			}
			else
			{
				global::KGFGUIUtility.SetSkinPathEditor(skinPath.Replace("_32", "_16"));
			}
		}
		else
		{
			global::KGFConsole.itsInstance.itsIsMobile = false;
		}
		global::System.Collections.Generic.List<string> theEntrys = new global::System.Collections.Generic.List<string>();
		global::KGFConsole.itsInstance.itsDropDownLastFive = new global::KGFGUIDropDown(theEntrys, 120U, 5U, global::KGFGUIDropDown.eDropDirection.eUp, new global::UnityEngine.GUILayoutOption[0]);
		global::KGFConsole.itsInstance.itsDropDownFavouriteFive = new global::KGFGUIDropDown(theEntrys, 120U, 5U, global::KGFGUIDropDown.eDropDirection.eUp, new global::UnityEngine.GUILayoutOption[0]);
		global::KGFConsole.itsInstance.itsDropDownLastFive.itsTitle = "last 5";
		global::KGFConsole.itsInstance.itsDropDownFavouriteFive.itsTitle = "favourite 5";
		global::KGFConsole.itsInstance.itsDropDownLastFive.itsIcon = this.itsDataModuleConsole.itsIconDropDown;
		global::KGFConsole.itsInstance.itsDropDownFavouriteFive.itsIcon = this.itsDataModuleConsole.itsIconDropDown;
		global::KGFConsole.itsInstance.itsDropDownLastFive.SelectedValueChanged += this.SelectedComboBoxValueChanged;
		global::KGFConsole.itsInstance.itsDropDownFavouriteFive.SelectedValueChanged += this.SelectedComboBoxValueChanged;
		global::KGFConsole.itsInstance.itsCommandTable = new global::KGFDataTable();
		if (global::KGFConsole.itsInstance.itsIsMobile)
		{
			global::KGFConsole.itsInstance.itsCommandTable.Columns.Add(new global::KGFDataColumn("Cmd", typeof(string)));
			global::KGFConsole.itsInstance.itsCommandTable.Columns.Add(new global::KGFDataColumn("Desc", typeof(string)));
			global::KGFConsole.itsInstance.itsCommandTable.Columns.Add(new global::KGFDataColumn("Cat", typeof(string)));
			global::KGFConsole.itsInstance.itsCommandTable.Columns.Add(new global::KGFDataColumn("Cnt", typeof(string)));
		}
		else
		{
			global::KGFConsole.itsInstance.itsCommandTable.Columns.Add(new global::KGFDataColumn("Command", typeof(string)));
			global::KGFConsole.itsInstance.itsCommandTable.Columns.Add(new global::KGFDataColumn("Description", typeof(string)));
			global::KGFConsole.itsInstance.itsCommandTable.Columns.Add(new global::KGFDataColumn("Category", typeof(string)));
			global::KGFConsole.itsInstance.itsCommandTable.Columns.Add(new global::KGFDataColumn("Count", typeof(string)));
		}
		this.itsTableControl = new global::KGFGUIDataTable(this.itsCommandTable, new global::UnityEngine.GUILayoutOption[]
		{
			global::UnityEngine.GUILayout.ExpandHeight(true)
		});
		this.itsTableControl.OnClickRow += this.OnCommandTableRowIsClicked;
		this.itsTableControl.PostRenderRow += this.PostCommandTableRow;
		this.itsTableControl.SetColumnWidth(0, (uint)((float)global::UnityEngine.Screen.width / 10f));
		this.itsTableControl.SetColumnWidth(1, (uint)((float)global::UnityEngine.Screen.width / 4f));
		this.itsTableControl.SetColumnWidth(2, (uint)((float)global::UnityEngine.Screen.width / 7f));
		this.itsTableControl.SetColumnWidth(3, 20U);
		this.itsTypeMapping = new global::System.Collections.Generic.Dictionary<global::System.Type, string>();
		this.itsTypeMapping.Add(typeof(short), "short");
		this.itsTypeMapping.Add(typeof(int), "int");
		this.itsTypeMapping.Add(typeof(long), "long");
		this.itsTypeMapping.Add(typeof(float), "float");
		this.itsTypeMapping.Add(typeof(double), "double");
		this.itsTypeMapping.Add(typeof(decimal), "decimal");
		this.itsTypeMapping.Add(typeof(string), "string");
		this.itsTypeMapping.Add(typeof(char), "char");
		this.itsTypeMapping.Add(typeof(bool), "bool");
		global::KGFConsoleCommands.AddCommands();
		global::KGFConsole.itsInstance.LoadSizeFromPlayerPrefs();
		if (this.itsCurrentHeight == 0f)
		{
			this.itsCurrentHeight = (float)global::UnityEngine.Screen.height * 0.5f;
		}
	}

	private global::System.Collections.Generic.IEnumerable<global::KGFConsole.KGFCcommandCategory> GetAllCategories()
	{
		return this.itsCommandCategories.Values;
	}

	private bool FilterCommandItem(global::KGFConsole.KGFCommandCode theCommandCode)
	{
		if (!this.itsSearchFilterMessage.Equals("Search") && !this.itsSearchFilterMessage.Equals(string.Empty))
		{
			string value = this.itsSearchFilterMessage.Trim().ToLower();
			if (!theCommandCode.itsDescription.Trim().ToLower().Contains(value) && !theCommandCode.itsCommandCode.Trim().ToLower().Contains(value) && !theCommandCode.itsCategory.Trim().ToLower().Contains(value))
			{
				return false;
			}
		}
		foreach (global::KGFConsole.KGFCcommandCategory kgfccommandCategory in this.itsCommandCategories.Values)
		{
			if (kgfccommandCategory.itsSelectedState && kgfccommandCategory.GetName().ToLower().Contains(theCommandCode.itsCategory.ToLower()))
			{
				return true;
			}
		}
		return false;
	}

	private global::System.Collections.Generic.IEnumerable<global::KGFConsole.KGFCommandCode> GetFilteredCommandListRealtime()
	{
		foreach (global::KGFConsole.KGFCommandCode aCommandCode in this.itsCommandDictionary.Values)
		{
			if (this.itsCommandCategories.ContainsKey(aCommandCode.itsCategory) && this.itsCommandCategories[aCommandCode.itsCategory].itsSelectedState && this.FilterCommandItem(aCommandCode))
			{
				yield return aCommandCode;
			}
		}
		yield break;
	}

	private void UpdateCommandListsResult()
	{
		this.itsCommandTable.Rows.Clear();
		foreach (global::KGFConsole.KGFCommandCode kgfcommandCode in this.GetFilteredCommandListRealtime())
		{
			global::KGFDataRow kgfdataRow = this.itsCommandTable.NewRow();
			kgfdataRow[0].Value = kgfcommandCode.itsCommandCode;
			kgfdataRow[1].Value = kgfcommandCode.itsDescription;
			kgfdataRow[2].Value = kgfcommandCode.itsCategory;
			kgfdataRow[3].Value = kgfcommandCode.GetExecutionCount().ToString();
			this.itsCommandTable.Rows.Add(kgfdataRow);
		}
	}

	private void ExecuteCommand()
	{
		this.itsCommand = this.CleanCommandString(this.itsCommand);
		string[] array = this.itsCommand.Split(new char[]
		{
			' '
		});
		string text = array[0];
		string text2 = string.Empty;
		if (this.itsCommand.Split(new char[]
		{
			' '
		}).Length > 1)
		{
			text2 = this.itsCommand.Substring(text.Length + 1, this.itsCommand.Length - (text.Length + 1));
		}
		else
		{
			text2 = null;
		}
		if (this.itsCommandDictionary.ContainsKey(text))
		{
			global::KGFConsole.KGFCommandCode kgfcommandCode = this.itsCommandDictionary[text];
			if (kgfcommandCode.Execute(text2))
			{
				global::UnityEngine.PlayerPrefs.SetInt("KGF.KGFModuleConsole.aCommand." + kgfcommandCode + text2, (int)kgfcommandCode.GetExecutionCount());
				this.itsCommand = this.itsCommand.Trim();
				if (!this.itsLastFiveCommandCodes.Contains(this.itsCommand))
				{
					if (this.itsLastFiveCommandCodes.Count < 5)
					{
						this.itsLastFiveCommandCodes.Add(this.itsCommand);
					}
					else
					{
						this.itsLastFiveCommandCodes = this.itsLastFiveCommandCodes.GetRange(1, 4);
						this.itsLastFiveCommandCodes.Add(this.itsCommand);
					}
					this.SaveLastFiveToPlayerPrefs();
				}
				if (this.itsFavouriteCommandCodes.ContainsKey(this.itsCommand))
				{
					global::System.Collections.Generic.Dictionary<string, global::KGFConsole.KGFFavouriteCommand> dictionary2;
					global::System.Collections.Generic.Dictionary<string, global::KGFConsole.KGFFavouriteCommand> dictionary = dictionary2 = this.itsFavouriteCommandCodes;
					string key2;
					string key = key2 = this.itsCommand;
					global::KGFConsole.KGFFavouriteCommand theItem = dictionary2[key2];
					dictionary[key] = ++theItem;
				}
				else
				{
					this.itsFavouriteCommandCodes.Add(this.itsCommand, new global::KGFConsole.KGFFavouriteCommand(kgfcommandCode, text2));
					this.itsFavourite.Add(this.itsFavouriteCommandCodes[this.itsCommand]);
				}
				this.itsFavourite.Sort();
				this.itsFavourite.Reverse();
				this.SaveFavouriteFiveToPlayerPrefs();
				this.itsCommandState = global::KGFConsole.eExecutionState.eSuccessful;
				this.itsCommand = string.Empty;
				global::KGFConsole.itsInstance.itsSuggestions.ClearItems();
				global::KGFConsole.itsInstance.itsDropDownLastFive.SetEntrys(this.itsLastFiveCommandCodes);
				global::System.Collections.Generic.List<string> list = new global::System.Collections.Generic.List<string>();
				foreach (global::KGFConsole.KGFFavouriteCommand kgffavouriteCommand in this.itsFavourite.GetRange(0, global::System.Math.Min(5, this.itsFavourite.Count)))
				{
					list.Add(kgffavouriteCommand.GetCommandCode() + " " + kgffavouriteCommand.GetParameters());
				}
				global::KGFConsole.itsInstance.itsDropDownFavouriteFive.SetEntrys(list);
			}
			else
			{
				this.itsCommandState = global::KGFConsole.eExecutionState.eError;
			}
		}
		else
		{
			this.itsCommandState = global::KGFConsole.eExecutionState.eNotFound;
		}
		this.itsLastExecution = global::UnityEngine.Time.time;
		this.UpdateCommandListsResult();
	}

	private void SaveLastFiveToPlayerPrefs()
	{
		global::System.Text.StringBuilder stringBuilder = new global::System.Text.StringBuilder();
		foreach (string str in this.itsLastFiveCommandCodes)
		{
			stringBuilder.Append(str + "|");
		}
		stringBuilder.Remove(stringBuilder.Length - 1, 1);
		global::UnityEngine.PlayerPrefs.SetString("KGF.KGFModuleConsole.itsLastFiveCommandCodes", stringBuilder.ToString());
	}

	private void SaveFavouriteFiveToPlayerPrefs()
	{
		global::System.Text.StringBuilder stringBuilder = new global::System.Text.StringBuilder();
		foreach (global::KGFConsole.KGFFavouriteCommand kgffavouriteCommand in this.itsFavourite)
		{
			stringBuilder.Append(string.Format("{0}:{1}:{2}|", kgffavouriteCommand.GetCommandCode().Trim(), kgffavouriteCommand.GetParameters(), kgffavouriteCommand.GetExecutionCount()));
		}
		stringBuilder.Remove(stringBuilder.Length - 1, 1);
		global::UnityEngine.PlayerPrefs.SetString("KGF.KGFModuleConsole.itsFavouriteCommandCodes", stringBuilder.ToString());
	}

	private void LoadLastFiveFromPlayerPrefs()
	{
		string @string = global::UnityEngine.PlayerPrefs.GetString("KGF.KGFModuleConsole.itsLastFiveCommandCodes", string.Empty);
		if (@string != string.Empty)
		{
			string[] array = @string.Split(new char[]
			{
				'|'
			});
			foreach (string text in array)
			{
				if (this.itsCommandDictionary.ContainsKey(text.Split(new char[]
				{
					' '
				})[0]) && !this.itsLastFiveCommandCodes.Contains(text))
				{
					this.itsLastFiveCommandCodes.Add(text.Trim());
				}
			}
		}
	}

	private void LoadFavouriteFiveFromPlayerPrefs()
	{
		string @string = global::UnityEngine.PlayerPrefs.GetString("KGF.KGFModuleConsole.itsFavouriteCommandCodes", string.Empty);
		if (@string != string.Empty)
		{
			string[] array = @string.Split(new char[]
			{
				'|'
			});
			foreach (string text in array)
			{
				string[] array3 = text.Split(new char[]
				{
					':'
				});
				for (int j = 0; j < array3.Length; j++)
				{
					array3[j] = array3[j].Trim();
				}
				if (this.itsCommandDictionary.ContainsKey(array3[0]))
				{
					if (array3[1].Length == 0)
					{
						if (!this.itsFavouriteCommandCodes.ContainsKey(array3[0]))
						{
							this.itsFavouriteCommandCodes.Add(array3[0], new global::KGFConsole.KGFFavouriteCommand(this.itsCommandDictionary[array3[0]], array3[1], uint.Parse(array3[2])));
							this.itsFavourite.Add(this.itsFavouriteCommandCodes[array3[0]]);
						}
					}
					else
					{
						string key = string.Format("{0} {1}", array3[0], array3[1]);
						if (!this.itsFavouriteCommandCodes.ContainsKey(key))
						{
							this.itsFavouriteCommandCodes.Add(key, new global::KGFConsole.KGFFavouriteCommand(this.itsCommandDictionary[array3[0]], array3[1], uint.Parse(array3[2])));
							this.itsFavourite.Add(this.itsFavouriteCommandCodes[key]);
						}
					}
				}
			}
			this.itsFavourite.Sort();
			this.itsFavourite = this.itsFavourite.GetRange(0, global::System.Math.Min(5, this.itsFavourite.Count));
		}
	}

	private void SaveSizeToPlayerPrefs()
	{
		global::UnityEngine.PlayerPrefs.SetFloat("KGFConsole.WindowSize", this.itsCurrentHeight);
	}

	private void LoadSizeFromPlayerPrefs()
	{
		this.itsCurrentHeight = global::UnityEngine.PlayerPrefs.GetFloat("KGFConsole.WindowSize", 0f);
	}

	private string CleanCommandString(string theCommand)
	{
		while (theCommand.Contains("  "))
		{
			theCommand = theCommand.Replace("  ", " ");
		}
		return theCommand.Trim();
	}

	private void OnCommandTableRowIsClicked(object theSender, global::System.EventArgs theArguments)
	{
		global::KGFDataRow kgfdataRow = theSender as global::KGFDataRow;
		if (kgfdataRow != null && global::KGFConsole.itsInstance.itsCommandDictionary.ContainsKey(kgfdataRow[0].ToString()))
		{
			if (this.itsCurrentSelectedRow != kgfdataRow)
			{
				this.itsCurrentSelectedRow = kgfdataRow;
			}
			else
			{
				this.itsCurrentSelectedRow = null;
			}
		}
	}

	private void PostCommandTableRow(object theSender, global::System.EventArgs theArguments)
	{
		global::KGFDataRow kgfdataRow = theSender as global::KGFDataRow;
		if (kgfdataRow != null)
		{
			global::UnityEngine.GUI.contentColor = global::UnityEngine.Color.white;
			if (kgfdataRow == this.itsCurrentSelectedRow && this.itsCommandDictionary.ContainsKey(kgfdataRow[0].ToString()))
			{
				global::KGFConsole.KGFCommandCode kgfcommandCode = this.itsCommandDictionary[kgfdataRow[0].ToString()];
				global::KGFGUIUtility.BeginVerticalBox(global::KGFGUIUtility.eStyleBox.eBoxDarkBottom, new global::UnityEngine.GUILayoutOption[]
				{
					global::UnityEngine.GUILayout.ExpandWidth(true)
				});
				global::UnityEngine.GUILayout.Space((float)global::KGFGUIUtility.GetStyleLabel(global::KGFGUIUtility.eStyleLabel.eLabel).fontSize);
				global::KGFGUIUtility.Label(string.Format("{0} {1}", "Description: ", kgfcommandCode.itsDescription), new global::UnityEngine.GUILayoutOption[]
				{
					global::UnityEngine.GUILayout.ExpandWidth(true)
				});
				global::UnityEngine.GUILayout.Space((float)global::KGFGUIUtility.GetStyleLabel(global::KGFGUIUtility.eStyleLabel.eLabel).fontSize);
				global::KGFGUIUtility.BeginHorizontalBox(global::KGFGUIUtility.eStyleBox.eBoxDarkBottom, new global::UnityEngine.GUILayoutOption[]
				{
					global::UnityEngine.GUILayout.ExpandWidth(true)
				});
				global::System.Text.StringBuilder stringBuilder = new global::System.Text.StringBuilder();
				foreach (global::KGFConsole.KGFMethodInfo kgfmethodInfo in kgfcommandCode.GetMehods())
				{
					stringBuilder.Append(string.Format("{0} {1}", kgfcommandCode.itsCommandCode, " "));
					uint num = 0U;
					foreach (global::System.Type key in kgfmethodInfo.GetParameterTypes())
					{
						num += 1U;
						stringBuilder.Append(string.Format("<{0}> ", this.itsTypeMapping[key]));
					}
					global::UnityEngine.Texture2D theImage = null;
					if (num == 0U)
					{
						theImage = this.itsDataModuleConsole.itsIconExecute;
					}
					if (global::KGFGUIUtility.Button(theImage, stringBuilder.ToString(), global::KGFGUIUtility.eStyleButton.eButton, new global::UnityEngine.GUILayoutOption[]
					{
						global::UnityEngine.GUILayout.ExpandWidth(true)
					}))
					{
						stringBuilder.Remove(0, stringBuilder.Length);
						stringBuilder.Append(string.Format("{0} ", kgfcommandCode.itsCommandCode));
						foreach (global::System.Type type in kgfmethodInfo.GetParameterTypes())
						{
							if (type == typeof(short))
							{
								stringBuilder.Append(string.Format("{0} ", 0.ToString()));
							}
							else if (type == typeof(int))
							{
								stringBuilder.Append(string.Format("{0} ", 0.ToString()));
							}
							else if (type == typeof(long))
							{
								stringBuilder.Append(string.Format("{0} ", 0L.ToString()));
							}
							else if (type == typeof(float))
							{
								stringBuilder.Append(string.Format("{0} ", 0f.ToString()));
							}
							else if (type == typeof(double))
							{
								stringBuilder.Append(string.Format("{0} ", 0.0.ToString()));
							}
							else if (type == typeof(decimal))
							{
								stringBuilder.Append(string.Format("{0} ", 0m.ToString()));
							}
							else if (type == typeof(string))
							{
								stringBuilder.Append("string ");
							}
							else if (type == typeof(char))
							{
								stringBuilder.Append(string.Format("{0} ", '\0'.ToString()));
							}
							else if (type == typeof(bool))
							{
								stringBuilder.Append(string.Format("{0} ", false.ToString()));
							}
						}
						this.itsCommand = this.CleanCommandString(stringBuilder.ToString());
						if (kgfmethodInfo.GetParameterCount() == 0U)
						{
							this.ExecuteCommand();
						}
						else
						{
							global::UnityEngine.GUI.FocusControl("commandCommand");
							this.itsFocus = true;
						}
					}
					stringBuilder.Remove(0, stringBuilder.Length);
				}
				global::KGFGUIUtility.EndHorizontalBox();
				global::KGFGUIUtility.EndVerticalBox();
			}
		}
	}

	private void SelectedComboBoxValueChanged(object theSender, global::System.EventArgs theArguments)
	{
		string text = theSender as string;
		if (text != null)
		{
			this.itsCommand = text;
			this.ExecuteCommand();
		}
	}

	public override global::KGFMessageList Validate()
	{
		global::KGFMessageList kgfmessageList = new global::KGFMessageList();
		if (this.itsDataModuleConsole.itsIconModule == null)
		{
			kgfmessageList.AddWarning("the module icon is missing");
		}
		if (this.itsDataModuleConsole.itsIconExecute == null)
		{
			kgfmessageList.AddWarning("the execution icon is missing");
		}
		if (this.itsDataModuleConsole.itsIconError == null)
		{
			kgfmessageList.AddWarning("the error icon is missing");
		}
		if (this.itsDataModuleConsole.itsIconFatal == null)
		{
			kgfmessageList.AddWarning("the fatal error icon is missing");
		}
		if (this.itsDataModuleConsole.itsIconUnknown == null)
		{
			kgfmessageList.AddWarning("the unknown icon is missing");
		}
		if (this.itsDataModuleConsole.itsIconSuccessful == null)
		{
			kgfmessageList.AddWarning("the successful icon is missing");
		}
		if (this.itsDataModuleConsole.itsIconHelp == null)
		{
			kgfmessageList.AddWarning("the help icon is missing");
		}
		if (this.itsDataModuleConsole.itsIconDropDown == null)
		{
			kgfmessageList.AddWarning("the drop down icon is missing");
		}
		if (this.itsDataModuleConsole.itsModifierKeyFocus == this.itsDataModuleConsole.itsSchortcutKeyFocus)
		{
			kgfmessageList.AddInfo("the modifier key is equal to the shortcut key");
		}
		return kgfmessageList;
	}

	private static global::KGFConsole itsInstance;

	private bool itsIsMobile;

	private bool itsOpen;

	private float itsCurrentHeight = (float)global::UnityEngine.Screen.height;

	private global::UnityEngine.Vector2 itsCategoryScrollViewPosition = global::UnityEngine.Vector2.zero;

	private string itsSearchFilterMessage = "Search";

	private string itsSearchFilterCategory = "Search";

	private bool itsLiveSearchChanged;

	private float itsLastChangeTime;

	private global::KGFDataRow itsCurrentSelectedRow;

	private global::System.Collections.Generic.Dictionary<string, global::KGFConsole.KGFCcommandCategory> itsCommandCategories = new global::System.Collections.Generic.Dictionary<string, global::KGFConsole.KGFCcommandCategory>();

	private global::System.Collections.Generic.Dictionary<string, global::KGFConsole.KGFCommandCode> itsCommandDictionary = new global::System.Collections.Generic.Dictionary<string, global::KGFConsole.KGFCommandCode>();

	private global::System.Collections.Generic.List<string> itsLastFiveCommandCodes = new global::System.Collections.Generic.List<string>();

	private global::System.Collections.Generic.Dictionary<string, global::KGFConsole.KGFFavouriteCommand> itsFavouriteCommandCodes = new global::System.Collections.Generic.Dictionary<string, global::KGFConsole.KGFFavouriteCommand>();

	private global::System.Collections.Generic.List<global::KGFConsole.KGFFavouriteCommand> itsFavourite = new global::System.Collections.Generic.List<global::KGFConsole.KGFFavouriteCommand>();

	private global::System.Collections.Generic.Dictionary<global::System.Type, string> itsTypeMapping;

	private global::KGFDataTable itsCommandTable;

	private string itsCommand = string.Empty;

	private global::KGFConsole.eExecutionState itsCommandState;

	private global::KGFGUIDataTable itsTableControl;

	private global::KGFGUIDropDown itsDropDownLastFive;

	private global::KGFGUIDropDown itsDropDownFavouriteFive;

	private bool itsHover;

	private bool itsFocus;

	private bool itsUnfocus;

	private float itsLastExecution;

	private static bool itsAlreadyChecked;

	public global::KGFConsole.KGFDataConsole itsDataModuleConsole = new global::KGFConsole.KGFDataConsole();

	private global::UnityEngine.Rect itsMinimizedWindow = new global::UnityEngine.Rect(0f, (float)(global::UnityEngine.Screen.height - 100), (float)global::UnityEngine.Screen.width, 100f);

	private global::UnityEngine.Rect itsOpenWindow;

	private global::KGFConsole.KGFSuggestionsControl itsSuggestions = new global::KGFConsole.KGFSuggestionsControl(5U);

	private bool itsModifierKeyFocusPressed;

	private bool itsShortcutKeyFocusPressed;

	[global::System.Serializable]
	public class KGFDataConsole
	{
		public global::UnityEngine.Texture2D itsIconModule;

		public global::UnityEngine.Texture2D itsIconExecute;

		public global::UnityEngine.Texture2D itsIconError;

		public global::UnityEngine.Texture2D itsIconFatal;

		public global::UnityEngine.Texture2D itsIconUnknown;

		public global::UnityEngine.Texture2D itsIconSuccessful;

		public global::UnityEngine.Texture2D itsIconHelp;

		public global::UnityEngine.Texture2D itsIconDropDown;

		public global::UnityEngine.KeyCode itsModifierKeyFocus = global::UnityEngine.KeyCode.LeftControl;

		public global::UnityEngine.KeyCode itsSchortcutKeyFocus = global::UnityEngine.KeyCode.LeftShift;

		public global::UnityEngine.KeyCode itsHideKeyModifier;

		public global::UnityEngine.KeyCode itsHideKey = global::UnityEngine.KeyCode.F2;

		public global::UnityEngine.KeyCode itsExpandKeyModifier = global::UnityEngine.KeyCode.LeftAlt;

		public global::UnityEngine.KeyCode itsExpandKey = global::UnityEngine.KeyCode.F2;

		public bool itsVisible = true;

		public uint itsVisibleSuggestions = 5U;
	}

	private class KGFMethodInfo
	{
		public KGFMethodInfo(global::System.Reflection.MethodInfo theMethodInfo, params global::System.Type[] theParameterTypes)
		{
			this.itsMethodInfo = theMethodInfo;
			foreach (global::System.Type item in theParameterTypes)
			{
				this.itsTypes.Add(item);
			}
		}

		public string GetName()
		{
			return this.itsMethodInfo.Name;
		}

		public uint GetParameterCount()
		{
			return (uint)this.itsTypes.Count;
		}

		public global::System.Collections.Generic.IEnumerable<global::System.Type> GetParameterTypes()
		{
			return this.itsTypes;
		}

		public global::System.Type GetParameterTypeAt(int thePosition)
		{
			if (thePosition >= 0 && thePosition < this.itsTypes.Count)
			{
				return this.itsTypes[thePosition];
			}
			global::UnityEngine.Debug.LogError("the index of the parameter was out of range");
			return null;
		}

		public global::System.Reflection.MethodInfo GetMethodInfo()
		{
			return this.itsMethodInfo;
		}

		private global::System.Reflection.MethodInfo itsMethodInfo;

		private global::System.Collections.Generic.List<global::System.Type> itsTypes = new global::System.Collections.Generic.List<global::System.Type>();
	}

	private class KGFCommandCode : global::System.IComparable<global::KGFConsole.KGFCommandCode>
	{
		public KGFCommandCode(string theCommandCode, string theDescription, string theCategory, object theObject, string theNameOfMethod)
		{
			this.itsCommandCode = theCommandCode;
			this.itsDescription = theDescription;
			this.itsCategory = theCategory;
			this.itsExecutionCount = 0U;
			this.itsObject = theObject;
			this.itsNameOfMethod = theNameOfMethod;
			this.GetMehods();
			if (this.itsMethodList.Count == 0)
			{
				global::UnityEngine.Debug.LogError("no method with the name " + theNameOfMethod + " was found for command " + theCommandCode);
			}
		}

		public KGFCommandCode(string theCommandCode, string theDescription, string theCategory, object theObject, string theNameOfMethod, uint theExecutionCount)
		{
			this.itsCommandCode = theCommandCode;
			this.itsDescription = theDescription;
			this.itsCategory = theCategory;
			this.itsExecutionCount = theExecutionCount;
			this.itsObject = theObject;
			this.itsNameOfMethod = theNameOfMethod;
			this.GetMehods();
			if (this.itsMethodList.Count == 0)
			{
				global::UnityEngine.Debug.LogError("no method with the name " + theNameOfMethod + " was found for command " + theCommandCode);
			}
		}

		public int CompareTo(global::KGFConsole.KGFCommandCode other)
		{
			return this.itsExecutionCount.CompareTo(other.itsExecutionCount);
		}

		public uint GetExecutionCount()
		{
			return this.itsExecutionCount;
		}

		public bool Execute(string theParameters)
		{
			string[] array;
			if (theParameters != null)
			{
				array = theParameters.Split(new char[]
				{
					' '
				});
			}
			else
			{
				array = new string[0];
			}
			object[] parameters;
			global::System.Reflection.MethodInfo methodInfo = this.FindMatchingMethod(array, out parameters);
			if (methodInfo != null)
			{
				if (array.Length == 0)
				{
					methodInfo.Invoke(this.itsObject, null);
				}
				else
				{
					methodInfo.Invoke(this.itsObject, parameters);
				}
				this.itsExecutionCount += 1U;
				return true;
			}
			return false;
		}

		public global::System.Collections.Generic.IEnumerable<global::KGFConsole.KGFMethodInfo> GetMehods()
		{
			if (this.itsMethodList == null)
			{
				this.itsMethodList = new global::System.Collections.Generic.List<global::KGFConsole.KGFMethodInfo>();
				foreach (global::System.Reflection.MethodInfo methodInfo in this.itsObject.GetType().GetMethods(global::System.Reflection.BindingFlags.Instance | global::System.Reflection.BindingFlags.Static | global::System.Reflection.BindingFlags.Public | global::System.Reflection.BindingFlags.NonPublic))
				{
					if (methodInfo.Name.Trim().Equals(this.itsNameOfMethod.Trim()))
					{
						global::System.Reflection.ParameterInfo[] parameters = methodInfo.GetParameters();
						if (methodInfo.GetParameters().Length > 0)
						{
							global::System.Type[] array = new global::System.Type[methodInfo.GetParameters().Length];
							foreach (global::System.Reflection.ParameterInfo parameterInfo in parameters)
							{
								array[parameterInfo.Position] = parameterInfo.ParameterType;
							}
							this.itsMethodList.Add(new global::KGFConsole.KGFMethodInfo(methodInfo, array));
						}
						else
						{
							this.itsMethodList.Add(new global::KGFConsole.KGFMethodInfo(methodInfo, new global::System.Type[0]));
						}
					}
				}
			}
			return this.itsMethodList;
		}

		private global::System.Reflection.MethodInfo FindMatchingMethod(string[] theParameters, out object[] theParametersObject)
		{
			foreach (global::KGFConsole.KGFMethodInfo kgfmethodInfo in this.itsMethodList)
			{
				if (kgfmethodInfo.GetParameterCount() == (uint)theParameters.Length)
				{
					if (theParameters.Length == 0)
					{
						theParametersObject = new object[0];
						return kgfmethodInfo.GetMethodInfo();
					}
					bool flag = true;
					theParametersObject = new object[kgfmethodInfo.GetParameterCount()];
					int num = 0;
					while ((long)num < (long)((ulong)kgfmethodInfo.GetParameterCount()))
					{
						global::System.Type parameterTypeAt = kgfmethodInfo.GetParameterTypeAt(num);
						if (parameterTypeAt.IsValueType)
						{
							if (parameterTypeAt == typeof(short))
							{
								short num2;
								if (!short.TryParse(theParameters[num], out num2))
								{
									flag = false;
									break;
								}
								theParametersObject[num] = num2;
							}
							else if (parameterTypeAt == typeof(int))
							{
								int num3;
								if (!int.TryParse(theParameters[num], out num3))
								{
									flag = false;
									break;
								}
								theParametersObject[num] = num3;
							}
							else if (parameterTypeAt == typeof(long))
							{
								long num4;
								if (!long.TryParse(theParameters[num], out num4))
								{
									flag = false;
									break;
								}
								theParametersObject[num] = num4;
							}
							else if (parameterTypeAt == typeof(float))
							{
								float num5;
								if (!float.TryParse(theParameters[num], out num5))
								{
									flag = false;
									break;
								}
								theParametersObject[num] = num5;
							}
							else if (parameterTypeAt == typeof(double))
							{
								double num6;
								if (!double.TryParse(theParameters[num], out num6))
								{
									flag = false;
									break;
								}
								theParametersObject[num] = num6;
							}
							else if (parameterTypeAt == typeof(decimal))
							{
								decimal num7;
								if (!decimal.TryParse(theParameters[num], out num7))
								{
									flag = false;
									break;
								}
								theParametersObject[num] = num7;
							}
							else if (parameterTypeAt == typeof(char))
							{
								char c;
								if (!char.TryParse(theParameters[num], out c))
								{
									flag = false;
									break;
								}
								theParametersObject[num] = c;
							}
							else
							{
								if (parameterTypeAt != typeof(bool))
								{
									flag = false;
									break;
								}
								bool flag2;
								if (!bool.TryParse(theParameters[num], out flag2))
								{
									flag = false;
									break;
								}
								theParametersObject[num] = flag2;
							}
						}
						else if (parameterTypeAt == typeof(string))
						{
							theParametersObject[num] = theParameters[num];
						}
						num++;
					}
					if (flag)
					{
						return kgfmethodInfo.GetMethodInfo();
					}
				}
			}
			theParametersObject = new object[0];
			return null;
		}

		public string itsCommandCode;

		public string itsDescription;

		public string itsCategory;

		private uint itsExecutionCount;

		private object itsObject;

		private string itsNameOfMethod;

		private global::System.Collections.Generic.List<global::KGFConsole.KGFMethodInfo> itsMethodList;
	}

	private class KGFCcommandCategory
	{
		public KGFCcommandCategory(string theName)
		{
			this.itsName = theName.Trim();
			this.itsCount = 0;
			this.itsSelectedState = true;
		}

		public string GetName()
		{
			return this.itsName;
		}

		public void IncreaseCount()
		{
			this.itsCount++;
		}

		public int GetCount()
		{
			return this.itsCount;
		}

		private int itsCount;

		private string itsName;

		public bool itsSelectedState;
	}

	private class KGFFavouriteCommand : global::System.IComparable<global::KGFConsole.KGFFavouriteCommand>
	{
		public KGFFavouriteCommand(global::KGFConsole.KGFCommandCode theCommandCode, string theParameter)
		{
			this.itsCommandCode = theCommandCode;
			this.itsParameters = theParameter;
			this.itsExecutionCount = 0U;
		}

		public KGFFavouriteCommand(global::KGFConsole.KGFCommandCode theCommandCode, string theParameter, uint theCount)
		{
			this.itsCommandCode = theCommandCode;
			this.itsParameters = theParameter.Trim();
			this.itsExecutionCount = theCount;
		}

		public uint GetExecutionCount()
		{
			return this.itsExecutionCount;
		}

		public string GetCommandCode()
		{
			return this.itsCommandCode.itsCommandCode;
		}

		public string GetParameters()
		{
			return this.itsParameters;
		}

		public int CompareTo(global::KGFConsole.KGFFavouriteCommand other)
		{
			return this.itsExecutionCount.CompareTo(other.itsExecutionCount);
		}

		public static global::KGFConsole.KGFFavouriteCommand operator ++(global::KGFConsole.KGFFavouriteCommand theItem)
		{
			theItem.itsExecutionCount += 1U;
			return theItem;
		}

		private global::KGFConsole.KGFCommandCode itsCommandCode;

		private string itsParameters;

		private uint itsExecutionCount;
	}

	private class KGFSuggestionsControl
	{
		public KGFSuggestionsControl(uint theMaxItems)
		{
			this.itsMaxItems = theMaxItems;
		}

		public void SetWidth(float theWidth)
		{
			this.itsRect.width = theWidth;
		}

		public void SetPosition(float theXValue, float theYValue)
		{
			this.itsRect.x = theXValue;
			this.itsRect.y = theYValue - (this.GetNumberOfEntriesToDisplay() + 1U) * global::KGFGUIUtility.GetSkinHeight();
		}

		private uint GetNumberOfEntriesToDisplay()
		{
			uint count = (uint)this.itsItems.Count;
			if (count > this.itsMaxItems)
			{
				count = this.itsMaxItems;
			}
			return count;
		}

		public void Render()
		{
			if (this.itsItems.Count > 0)
			{
				this.itsRect.height = this.GetNumberOfEntriesToDisplay() * global::KGFGUIUtility.GetSkinHeight();
				global::UnityEngine.GUILayout.BeginArea(this.itsRect);
				global::KGFGUIUtility.BeginVerticalBox(global::KGFGUIUtility.eStyleBox.eBox, new global::UnityEngine.GUILayoutOption[]
				{
					global::UnityEngine.GUILayout.ExpandWidth(true),
					global::UnityEngine.GUILayout.ExpandHeight(true)
				});
				global::UnityEngine.GUILayout.FlexibleSpace();
				global::UnityEngine.Color color = global::UnityEngine.GUI.color;
				for (int i = 0; i < this.itsItems.Count; i++)
				{
					if (this.itsCurrentSelected == i)
					{
						global::UnityEngine.GUI.color = color;
					}
					global::KGFGUIUtility.Label(this.itsItems[i], global::KGFGUIUtility.eStyleLabel.eLabelFitIntoBox, new global::UnityEngine.GUILayoutOption[0]);
					global::UnityEngine.GUI.color = global::UnityEngine.Color.gray;
				}
				global::UnityEngine.GUI.color = color;
				global::KGFGUIUtility.EndVerticalBox();
				global::UnityEngine.GUILayout.EndArea();
			}
		}

		public void SelectionUp()
		{
			if (this.itsItems.Count > 0)
			{
				this.itsCurrentSelected--;
				if (this.itsCurrentSelected < 0)
				{
					this.itsCurrentSelected += this.itsItems.Count;
				}
			}
		}

		public void SelectionDown()
		{
			this.itsCurrentSelected++;
			this.itsCurrentSelected %= this.itsItems.Count;
		}

		public int GetCount()
		{
			return this.itsItems.Count;
		}

		public uint GetMaxCount()
		{
			return this.itsMaxItems;
		}

		public string GetSelected()
		{
			if (this.itsCurrentSelected >= this.itsItems.Count)
			{
				return string.Empty;
			}
			return this.itsItems[this.itsCurrentSelected];
		}

		public void SetItems(global::System.Collections.Generic.IEnumerable<string> theItems)
		{
			this.itsItems.Clear();
			this.itsCurrentSelected = 0;
			this.itsItems.AddRange(theItems);
		}

		public void ClearItems()
		{
			this.itsItems.Clear();
			this.itsCurrentSelected = 0;
		}

		private global::UnityEngine.Rect itsRect;

		private uint itsMaxItems;

		private int itsCurrentSelected;

		private global::System.Collections.Generic.List<string> itsItems = new global::System.Collections.Generic.List<string>();
	}

	private enum eExecutionState
	{
		eNone,
		eSuccessful,
		eNotFound,
		eError
	}
}
