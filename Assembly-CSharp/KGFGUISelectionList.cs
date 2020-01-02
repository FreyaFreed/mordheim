using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KGFGUISelectionList
{
	public event global::System.EventHandler EventItemChanged;

	public void SetValues(global::System.Collections.IEnumerable theList)
	{
		this.itsListSource = theList;
		this.UpdateList();
		this.UpdateItemFilter();
	}

	public bool GetIsSelected(object theItem)
	{
		foreach (global::KGFGUISelectionList.ListItem listItem in this.itsData)
		{
			if (theItem == listItem.GetItem())
			{
				return listItem.itsSelected;
			}
		}
		return false;
	}

	public void SetDisplayMethod(global::System.Func<object, string> theDisplayMethod)
	{
		this.itsDisplayMethod = theDisplayMethod;
		this.UpdateItemFilter();
	}

	public void ClearDisplayMethod()
	{
		this.itsDisplayMethod = null;
		this.UpdateItemFilter();
	}

	private int ListItemComparer(global::KGFGUISelectionList.ListItem theListItem1, global::KGFGUISelectionList.ListItem theListItem2)
	{
		return theListItem1.GetString().CompareTo(theListItem2.GetString());
	}

	public void Render()
	{
		global::UnityEngine.GUILayout.BeginVertical(new global::UnityEngine.GUILayoutOption[0]);
		global::KGFGUIUtility.BeginVerticalBox(global::KGFGUIUtility.eStyleBox.eBoxDarkTop, new global::UnityEngine.GUILayoutOption[0]);
		this.DrawButtons();
		global::KGFGUIUtility.EndVerticalBox();
		global::KGFGUIUtility.BeginVerticalBox(global::KGFGUIUtility.eStyleBox.eBoxMiddleVertical, new global::UnityEngine.GUILayoutOption[0]);
		this.DrawList();
		global::KGFGUIUtility.EndVerticalBox();
		global::KGFGUIUtility.BeginHorizontalBox(global::KGFGUIUtility.eStyleBox.eBoxDarkMiddleVertical, new global::UnityEngine.GUILayoutOption[0]);
		global::KGFGUIUtility.Label(string.Empty, new global::UnityEngine.GUILayoutOption[]
		{
			global::UnityEngine.GUILayout.ExpandWidth(true)
		});
		global::KGFGUIUtility.EndHorizontalBox();
		global::KGFGUIUtility.BeginVerticalBox(global::KGFGUIUtility.eStyleBox.eBoxDarkBottom, new global::UnityEngine.GUILayoutOption[0]);
		this.DrawSearch();
		global::KGFGUIUtility.EndVerticalBox();
		global::UnityEngine.GUILayout.EndVertical();
		if (global::UnityEngine.GUI.GetNameOfFocusedControl().Equals("tagSearch") && this.itsSearch.Equals("Search"))
		{
			this.itsSearch = string.Empty;
		}
		if (!global::UnityEngine.GUI.GetNameOfFocusedControl().Equals("tagSearch") && this.itsSearch.Equals(string.Empty))
		{
			this.itsSearch = "Search";
		}
	}

	public global::System.Collections.Generic.IEnumerable<object> GetSelected()
	{
		foreach (global::KGFGUISelectionList.ListItem anItem in this.itsData)
		{
			if (anItem.itsSelected)
			{
				yield return anItem.GetItem();
			}
		}
		yield break;
	}

	public void SetSelected(global::System.Collections.Generic.IEnumerable<object> theList)
	{
		this.SetSelectedAll(false);
		foreach (object theItem in theList)
		{
			this.SetSelected(theItem, true);
		}
	}

	public void SetSelected(object theItem, bool theSelectionState)
	{
		foreach (global::KGFGUISelectionList.ListItem listItem in this.itsData)
		{
			if (theItem == listItem.GetItem())
			{
				listItem.itsSelected = theSelectionState;
				break;
			}
		}
	}

	public void SetSelected(string theItem, bool theSelectionState)
	{
		foreach (global::KGFGUISelectionList.ListItem listItem in this.itsData)
		{
			if (theItem == listItem.GetItem().ToString())
			{
				listItem.itsSelected = theSelectionState;
				break;
			}
		}
	}

	private void UpdateList()
	{
		global::System.Collections.Generic.List<object> selected = new global::System.Collections.Generic.List<object>(this.GetSelected());
		this.itsData.Clear();
		foreach (object arg in this.itsListSource)
		{
			this.itsData.Add(new global::KGFGUISelectionList.ListItem(string.Empty + arg));
		}
		this.itsData.Sort(new global::System.Comparison<global::KGFGUISelectionList.ListItem>(this.ListItemComparer));
		this.SetSelected(selected);
	}

	public void UpdateItemFilter()
	{
		if (this.itsSearch.Trim() == string.Empty || this.itsSearch.Trim() == "Search")
		{
			foreach (global::KGFGUISelectionList.ListItem listItem in this.itsData)
			{
				listItem.itsFiltered = false;
			}
		}
		else
		{
			foreach (global::KGFGUISelectionList.ListItem listItem2 in this.itsData)
			{
				listItem2.UpdateCache(this.itsDisplayMethod);
				listItem2.itsFiltered = !listItem2.GetString().Trim().ToLower().Contains(this.itsSearch.Trim().ToLower());
			}
		}
	}

	public void SetSelectedAll(bool theValue)
	{
		foreach (global::KGFGUISelectionList.ListItem listItem in this.itsData)
		{
			listItem.itsSelected = theValue;
		}
		if (this.EventItemChanged != null)
		{
			this.EventItemChanged(this, null);
		}
	}

	private void DrawButtons()
	{
		global::UnityEngine.GUILayout.BeginHorizontal(new global::UnityEngine.GUILayoutOption[0]);
		if (global::KGFGUIUtility.Button("All", global::KGFGUIUtility.eStyleButton.eButton, new global::UnityEngine.GUILayoutOption[0]))
		{
			this.SetSelectedAll(true);
		}
		if (global::KGFGUIUtility.Button("None", global::KGFGUIUtility.eStyleButton.eButton, new global::UnityEngine.GUILayoutOption[0]))
		{
			this.SetSelectedAll(false);
		}
		global::UnityEngine.GUILayout.EndHorizontal();
	}

	private void DrawList()
	{
		this.itsScrollPosition = global::UnityEngine.GUILayout.BeginScrollView(this.itsScrollPosition, new global::UnityEngine.GUILayoutOption[0]);
		global::KGFGUIUtility.BeginVerticalBox(global::KGFGUIUtility.eStyleBox.eBoxInvisible, new global::UnityEngine.GUILayoutOption[0]);
		foreach (global::KGFGUISelectionList.ListItem listItem in this.itsData)
		{
			if (!listItem.itsFiltered)
			{
				bool flag = global::KGFGUIUtility.Toggle(listItem.itsSelected, listItem.GetString(), global::KGFGUIUtility.eStyleToggl.eTogglSuperCompact, new global::UnityEngine.GUILayoutOption[0]);
				if (flag != listItem.itsSelected)
				{
					listItem.itsSelected = flag;
					if (this.EventItemChanged != null)
					{
						this.EventItemChanged(this, null);
					}
				}
			}
		}
		global::KGFGUIUtility.EndVerticalBox();
		global::UnityEngine.GUILayout.EndScrollView();
	}

	private void DrawSearch()
	{
		global::UnityEngine.GUI.SetNextControlName("tagSearch");
		string a = global::KGFGUIUtility.TextField(this.itsSearch, global::KGFGUIUtility.eStyleTextField.eTextField, new global::UnityEngine.GUILayoutOption[0]);
		if (a != this.itsSearch)
		{
			this.itsSearch = a;
			this.UpdateItemFilter();
		}
	}

	private const string itsControlSearchName = "tagSearch";

	private const string itsTextSearch = "Search";

	private global::System.Collections.Generic.List<global::KGFGUISelectionList.ListItem> itsData = new global::System.Collections.Generic.List<global::KGFGUISelectionList.ListItem>();

	private string itsSearch = string.Empty;

	private global::System.Collections.IEnumerable itsListSource;

	private global::UnityEngine.Vector2 itsScrollPosition = global::UnityEngine.Vector2.zero;

	private global::System.Func<object, string> itsDisplayMethod;

	private class ListItem
	{
		public ListItem(object theItem)
		{
			this.itsItem = theItem;
			this.itsSelected = false;
			this.itsFiltered = false;
			this.UpdateCache(null);
		}

		public void UpdateCache(global::System.Func<object, string> theDisplayMethod)
		{
			if (theDisplayMethod != null)
			{
				this.itsCachedString = theDisplayMethod(this.itsItem);
			}
			else
			{
				this.itsCachedString = this.itsItem.ToString();
			}
		}

		public string GetString()
		{
			return this.itsCachedString;
		}

		public object GetItem()
		{
			return this.itsItem;
		}

		private string itsCachedString;

		private object itsItem;

		public bool itsSelected;

		public bool itsFiltered;
	}
}
