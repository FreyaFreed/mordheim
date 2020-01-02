using System;
using System.Collections.Generic;
using UnityEngine;

public class KGFGUIDropDown : global::KGFIControl
{
	public KGFGUIDropDown(global::System.Collections.Generic.IEnumerable<string> theEntrys, uint theWidth, uint theMaxVisibleItems, global::KGFGUIDropDown.eDropDirection theDirection, params global::UnityEngine.GUILayoutOption[] theLayout)
	{
		if (theEntrys != null)
		{
			foreach (string item in theEntrys)
			{
				this.itsEntrys.Add(item);
			}
			this.itsWidth = theWidth;
			this.itsMaxVisibleItems = theMaxVisibleItems;
			this.itsDirection = theDirection;
			if (this.itsEntrys.Count > 0)
			{
				this.itsCurrentSelected = this.itsEntrys[0];
			}
		}
		else
		{
			global::UnityEngine.Debug.LogError("the list of entrys was null");
		}
	}

	public event global::System.EventHandler SelectedValueChanged;

	public void SetEntrys(global::System.Collections.Generic.IEnumerable<string> theEntrys)
	{
		this.itsEntrys.Clear();
		foreach (string item in theEntrys)
		{
			this.itsEntrys.Add(item);
		}
		if (this.itsEntrys.Count > 0)
		{
			this.itsCurrentSelected = this.itsEntrys[0];
		}
	}

	public global::System.Collections.Generic.IEnumerable<string> GetEntrys()
	{
		return this.itsEntrys;
	}

	public string SelectedItem()
	{
		return this.itsCurrentSelected;
	}

	public void SetSelectedItem(string theValue)
	{
		if (!this.itsEntrys.Contains(theValue))
		{
			return;
		}
		this.itsCurrentSelected = theValue;
		if (this.SelectedValueChanged != null)
		{
			this.SelectedValueChanged(theValue, global::System.EventArgs.Empty);
		}
	}

	public void Render()
	{
		if ((long)this.itsEntrys.Count <= (long)((ulong)this.itsMaxVisibleItems))
		{
			this.itsHeight = (uint)(this.itsEntrys.Count * (int)((uint)global::KGFGUIUtility.GetSkinHeight()));
		}
		else
		{
			this.itsHeight = this.itsMaxVisibleItems * (uint)global::KGFGUIUtility.GetSkinHeight();
		}
		if (this.itsVisible)
		{
			global::UnityEngine.GUILayout.BeginHorizontal(new global::UnityEngine.GUILayoutOption[]
			{
				global::UnityEngine.GUILayout.Width(this.itsWidth)
			});
			global::KGFGUIUtility.BeginHorizontalBox(global::KGFGUIUtility.eStyleBox.eBoxLeft, new global::UnityEngine.GUILayoutOption[0]);
			if (this.itsTitle != string.Empty)
			{
				global::KGFGUIUtility.Label(this.itsTitle, global::KGFGUIUtility.eStyleLabel.eLabelFitIntoBox, new global::UnityEngine.GUILayoutOption[]
				{
					global::UnityEngine.GUILayout.ExpandWidth(true)
				});
			}
			else
			{
				global::KGFGUIUtility.Label(this.itsCurrentSelected, global::KGFGUIUtility.eStyleLabel.eLabelFitIntoBox, new global::UnityEngine.GUILayoutOption[]
				{
					global::UnityEngine.GUILayout.ExpandWidth(true)
				});
			}
			global::KGFGUIUtility.EndHorizontalBox();
			if (this.itsIcon == null)
			{
				if (global::KGFGUIUtility.Button("v", global::KGFGUIUtility.eStyleButton.eButtonRight, new global::UnityEngine.GUILayoutOption[]
				{
					global::UnityEngine.GUILayout.ExpandWidth(false)
				}))
				{
					if (global::KGFGUIDropDown.itsOpenInstance != this)
					{
						global::KGFGUIDropDown.itsOpenInstance = this;
						global::KGFGUIDropDown.itsCorrectedOffset = false;
					}
					else
					{
						global::KGFGUIDropDown.itsOpenInstance = null;
						global::KGFGUIDropDown.itsCorrectedOffset = false;
					}
				}
			}
			else if (global::KGFGUIUtility.Button(this.itsIcon, global::KGFGUIUtility.eStyleButton.eButtonRight, new global::UnityEngine.GUILayoutOption[]
			{
				global::UnityEngine.GUILayout.ExpandWidth(false)
			}))
			{
				if (global::KGFGUIDropDown.itsOpenInstance != this)
				{
					global::KGFGUIDropDown.itsOpenInstance = this;
					global::KGFGUIDropDown.itsCorrectedOffset = false;
				}
				else
				{
					global::KGFGUIDropDown.itsOpenInstance = null;
					global::KGFGUIDropDown.itsCorrectedOffset = false;
				}
			}
			global::UnityEngine.GUILayout.EndHorizontal();
			if (global::UnityEngine.Event.current.type == global::UnityEngine.EventType.Repaint)
			{
				this.itsLastRect = global::UnityEngine.GUILayoutUtility.GetLastRect();
			}
			else
			{
				global::UnityEngine.Vector3 mousePosition = global::UnityEngine.Input.mousePosition;
				mousePosition.y = (float)global::UnityEngine.Screen.height - mousePosition.y;
				if (this.itsLastRect.Contains(mousePosition))
				{
					this.itsHover = true;
				}
				else if (global::KGFGUIDropDown.itsOpenInstance != this)
				{
					this.itsHover = false;
				}
			}
		}
	}

	public string GetName()
	{
		return "KGFGUIDropDown";
	}

	public bool IsVisible()
	{
		return this.itsVisible;
	}

	public bool GetHover()
	{
		return this.itsHover;
	}

	private global::System.Collections.Generic.List<string> itsEntrys = new global::System.Collections.Generic.List<string>();

	private global::UnityEngine.GUILayoutOption[] itsLayoutOptions;

	private string itsCurrentSelected = string.Empty;

	private bool itsVisible = true;

	public global::UnityEngine.Vector2 itsScrollPosition = global::UnityEngine.Vector2.zero;

	public global::UnityEngine.Rect itsLastRect;

	public static global::KGFGUIDropDown itsOpenInstance;

	public uint itsWidth;

	public uint itsHeight;

	private uint itsMaxVisibleItems = 1U;

	public global::KGFGUIDropDown.eDropDirection itsDirection;

	public string itsTitle = string.Empty;

	public global::UnityEngine.Texture2D itsIcon;

	public bool itsHover;

	public static bool itsCorrectedOffset;

	public enum eDropDirection
	{
		eAuto,
		eDown,
		eUp
	}
}
