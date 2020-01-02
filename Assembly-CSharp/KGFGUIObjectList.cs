using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class KGFGUIObjectList
{
	public KGFGUIObjectList(global::System.Type theType)
	{
		this.itsListData = new global::System.Collections.Generic.List<global::KGFITaggable>();
		this.itsItemType = theType;
		this.itsData = new global::KGFDataTable();
		this.itsListFieldCache = new global::System.Collections.Generic.List<global::KGFGUIObjectList.KGFObjectListColumnItem>();
		this.CacheTypeMembers();
		this.itsGuiData = new global::KGFGUIDataTable(this.itsData, new global::UnityEngine.GUILayoutOption[0]);
		this.itsGuiData.OnClickRow += this.OnClickRow;
		this.itsGuiData.EventSettingsChanged += this.OnGuiDataSettingsChanged;
		this.itsGuiData.SetColumnVisible(0, false);
		for (int i = 0; i < this.itsListFieldCache.Count; i++)
		{
			this.itsGuiData.SetColumnVisible(i + 1, this.itsListFieldCache[i].itsDisplay);
		}
		this.itsListViewCategories = new global::KGFGUISelectionList();
		this.itsListViewCategories.EventItemChanged += this.OnCategoriesChanged;
	}

	public event global::System.EventHandler EventSelect;

	public event global::System.EventHandler EventSettingsChanged;

	public event global::System.EventHandler EventNew;

	public event global::System.EventHandler EventDelete;

	public event global::System.EventHandler PreRenderRow
	{
		add
		{
			this.itsGuiData.PreRenderRow += value;
		}
		remove
		{
			this.itsGuiData.PreRenderRow -= value;
		}
	}

	public event global::System.Func<global::KGFDataRow, global::KGFDataColumn, uint, bool> PreCellHandler
	{
		add
		{
			this.itsGuiData.PreCellContentHandler += value;
		}
		remove
		{
			this.itsGuiData.PreCellContentHandler -= value;
		}
	}

	public event global::System.EventHandler PostRenderRow
	{
		add
		{
			this.itsGuiData.PostRenderRow += value;
		}
		remove
		{
			this.itsGuiData.PostRenderRow -= value;
		}
	}

	private void OnGuiDataSettingsChanged(object theSender, global::System.EventArgs theArgs)
	{
		this.OnSettingsChanged();
	}

	public void SetFulltextFilter(string theFulltextSearch)
	{
		this.itsFulltextSearch = theFulltextSearch;
		this.UpdateList();
	}

	public void SetColumnWidthAll(uint theWidth)
	{
		for (int i = 1; i < this.itsListFieldCache.Count + 1; i++)
		{
			this.itsGuiData.SetColumnWidth(i, theWidth);
		}
	}

	public void SetColumnWidth(string theColumnHeader, uint theWidth)
	{
		for (int i = 0; i < this.itsListFieldCache.Count; i++)
		{
			if (this.itsListFieldCache[i].itsDisplay && this.itsListFieldCache[i].itsHeader == theColumnHeader)
			{
				this.itsGuiData.SetColumnWidth(i + 1, theWidth);
				break;
			}
		}
	}

	public void SetColumnVisible(string theColumnHeader, bool theVisible)
	{
		for (int i = 0; i < this.itsListFieldCache.Count; i++)
		{
			if (this.itsListFieldCache[i].itsDisplay && this.itsListFieldCache[i].itsHeader == theColumnHeader)
			{
				this.itsGuiData.SetColumnVisible(i + 1, theVisible);
				break;
			}
		}
	}

	public void SetList(global::System.Collections.IEnumerable theList)
	{
		global::System.Collections.Generic.List<global::KGFITaggable> list = new global::System.Collections.Generic.List<global::KGFITaggable>();
		foreach (object obj in theList)
		{
			if (obj is global::KGFITaggable)
			{
				list.Add((global::KGFITaggable)obj);
			}
		}
		this.SetList(list);
	}

	public void SetList(global::System.Collections.Generic.IEnumerable<global::KGFITaggable> theList)
	{
		this.itsListData = new global::System.Collections.Generic.List<global::KGFITaggable>(theList);
		this.itsListViewCategories.SetValues(this.GetAllTags().Distinct<string>());
		this.UpdateList();
	}

	public void AddMember(global::System.Reflection.MemberInfo theMemberInfo, string theHeader)
	{
		this.AddMember(theMemberInfo, theHeader, false);
	}

	public void AddMember(global::System.Reflection.MemberInfo theMemberInfo, string theHeader, bool theSearchable)
	{
		this.AddMember(theMemberInfo, theHeader, theSearchable, true);
	}

	public void AddMember(global::System.Reflection.MemberInfo theMemberInfo, string theHeader, bool theSearchable, bool theDisplay)
	{
		global::KGFGUIObjectList.KGFObjectListColumnItem kgfobjectListColumnItem = new global::KGFGUIObjectList.KGFObjectListColumnItem(theMemberInfo);
		kgfobjectListColumnItem.itsHeader = theHeader;
		kgfobjectListColumnItem.itsSearchable = theSearchable;
		kgfobjectListColumnItem.itsDisplay = theDisplay;
		this.itsListFieldCache.Add(kgfobjectListColumnItem);
		this.itsData.Columns.Add(new global::KGFDataColumn(theHeader, kgfobjectListColumnItem.GetReturnType()));
		if (kgfobjectListColumnItem.itsSearchable)
		{
			this.itsDisplayFullTextSearch = true;
		}
	}

	public object GetCurrentSelected()
	{
		return this.itsCurrentSelectedItem;
	}

	public void ClearSelected()
	{
		this.itsCurrentSelectedItem = null;
	}

	public void SetSelected(global::KGFITaggable theObject)
	{
		this.itsCurrentSelectedItem = theObject;
		int num = 0;
		foreach (global::KGFDataRow kgfdataRow in this.itsData.Rows)
		{
			if (kgfdataRow[0].Value == theObject)
			{
				this.itsGuiData.SetCurrentSelected(kgfdataRow);
				this.itsCurrentPage = num / (int)this.itsItemsPerPage;
				break;
			}
			num++;
		}
	}

	public global::UnityEngine.Rect GetLastRectScrollView()
	{
		return this.itsGuiData.GetLastRectScrollview();
	}

	public string SaveSettings()
	{
		global::System.Collections.Generic.List<string> list = new global::System.Collections.Generic.List<string>();
		foreach (global::KGFGUIObjectList.KGFObjectListColumnItem kgfobjectListColumnItem in this.itsListFieldCache)
		{
			if (kgfobjectListColumnItem.itsDropDown != null)
			{
				list.Add(kgfobjectListColumnItem.itsHeader + "=" + kgfobjectListColumnItem.itsDropDown.SelectedItem());
			}
			else
			{
				list.Add(kgfobjectListColumnItem.itsHeader + "=" + kgfobjectListColumnItem.itsFilterString);
			}
		}
		string arg = (this.itsGuiData.GetSortingColumn() == null) ? string.Empty : this.itsGuiData.GetSortingColumn().ColumnName;
		global::System.Collections.Generic.List<string> list2 = new global::System.Collections.Generic.List<string>();
		foreach (object arg2 in this.itsListViewCategories.GetSelected())
		{
			list2.Add(string.Empty + arg2);
		}
		string arg3 = list2.JoinToString(",");
		return string.Format("Filter:{0};SortBy:{1};Tags:{2}", list.JoinToString(","), arg, arg3);
	}

	public void LoadSettings(string theSettingsString)
	{
		this.itsLoadingActive = true;
		string[] array = theSettingsString.Split(new char[]
		{
			';'
		});
		foreach (string text in array)
		{
			string[] array3 = text.Split(new char[]
			{
				':'
			});
			if (array3.Length == 2)
			{
				if (array3[0] == "Filter")
				{
					foreach (string text2 in array3[1].Split(new char[]
					{
						','
					}))
					{
						string[] array5 = text2.Split(new char[]
						{
							'='
						});
						if (array5.Length == 2)
						{
							this.SetFilterInternal(array5[0], array5[1]);
						}
					}
				}
				if (array3[0] == "SortBy")
				{
					if (array3[1].Trim() == string.Empty)
					{
						this.itsGuiData.SetSortingColumn(null);
					}
					else
					{
						this.itsGuiData.SetSortingColumn(array3[1]);
					}
				}
				if (array3[0] == "Tags")
				{
					this.itsListViewCategories.SetSelectedAll(false);
					foreach (string theItem in array3[1].Split(new char[]
					{
						','
					}))
					{
						this.itsListViewCategories.SetSelected(theItem, true);
					}
				}
			}
		}
		this.itsRepaintWish = true;
		this.UpdateList();
		this.itsLoadingActive = false;
	}

	public void SetFilter(string theColumnName, string theFilter)
	{
		if (this.SetFilterInternal(theColumnName, theFilter))
		{
			this.OnSettingsChanged();
		}
	}

	public void ClearFilters()
	{
		foreach (global::KGFGUIObjectList.KGFObjectListColumnItem kgfobjectListColumnItem in this.itsListFieldCache)
		{
			kgfobjectListColumnItem.itsFilterString = string.Empty;
			if (kgfobjectListColumnItem.itsDropDown != null)
			{
				kgfobjectListColumnItem.itsDropDown.SetSelectedItem("<NONE>");
			}
		}
		this.itsRepaintWish = true;
		this.OnSettingsChanged();
	}

	private bool SetFilterInternal(string theColumnName, string theFilter)
	{
		foreach (global::KGFGUIObjectList.KGFObjectListColumnItem kgfobjectListColumnItem in this.itsListFieldCache)
		{
			if (theColumnName == kgfobjectListColumnItem.itsHeader)
			{
				kgfobjectListColumnItem.itsFilterString = theFilter;
				this.itsRepaintWish = true;
				return true;
			}
		}
		return false;
	}

	public void Render()
	{
		if (this.itsUpdateWish)
		{
			this.UpdateList();
		}
		int num = (int)global::System.Math.Ceiling((double)((float)this.itsData.Rows.Count / (float)this.itsItemsPerPage));
		if (this.itsCurrentPage >= num)
		{
			this.itsCurrentPage = 0;
		}
		this.itsRepaintWish = false;
		this.itsGuiData.SetDisplayRowCount((uint)this.itsItemsPerPage);
		global::KGFGUIUtility.BeginHorizontalBox(global::KGFGUIUtility.eStyleBox.eBoxDecorated, new global::UnityEngine.GUILayoutOption[0]);
		global::UnityEngine.GUILayout.BeginVertical(new global::UnityEngine.GUILayoutOption[]
		{
			global::UnityEngine.GUILayout.Width(180f)
		});
		this.itsListViewCategories.Render();
		global::UnityEngine.GUILayout.EndVertical();
		global::KGFGUIUtility.SpaceSmall();
		global::UnityEngine.GUILayout.BeginVertical(new global::UnityEngine.GUILayoutOption[0]);
		this.itsGuiData.SetStartRow((uint)((long)this.itsCurrentPage * (long)((ulong)this.itsItemsPerPage)));
		this.itsGuiData.Render();
		global::KGFGUIUtility.BeginHorizontalBox(global::KGFGUIUtility.eStyleBox.eBoxMiddleVerticalInteractive, new global::UnityEngine.GUILayoutOption[0]);
		int num2 = 0;
		foreach (global::KGFGUIObjectList.KGFObjectListColumnItem kgfobjectListColumnItem in this.itsListFieldCache)
		{
			num2++;
			if (kgfobjectListColumnItem.itsDisplay)
			{
				if (this.itsGuiData.GetColumnVisible(num2))
				{
					if (kgfobjectListColumnItem.itsSearchable && (kgfobjectListColumnItem.GetReturnType().IsEnum || kgfobjectListColumnItem.GetReturnType() == typeof(bool) || kgfobjectListColumnItem.GetReturnType() == typeof(string)))
					{
						global::UnityEngine.GUILayout.BeginHorizontal(new global::UnityEngine.GUILayoutOption[]
						{
							global::UnityEngine.GUILayout.Width(this.itsGuiData.GetColumnWidth(num2))
						});
						global::KGFGUIUtility.BeginVerticalBox(global::KGFGUIUtility.eStyleBox.eBoxInvisible, new global::UnityEngine.GUILayoutOption[0]);
						this.DrawFilterBox(kgfobjectListColumnItem, this.itsGuiData.GetColumnWidth(num2) - 4U);
						global::KGFGUIUtility.EndVerticalBox();
						global::UnityEngine.GUILayout.EndHorizontal();
						global::KGFGUIUtility.Separator(global::KGFGUIUtility.eStyleSeparator.eSeparatorVerticalFitInBox, new global::UnityEngine.GUILayoutOption[0]);
					}
					else
					{
						global::UnityEngine.GUILayout.BeginHorizontal(new global::UnityEngine.GUILayoutOption[]
						{
							global::UnityEngine.GUILayout.Width(this.itsGuiData.GetColumnWidth(num2))
						});
						global::UnityEngine.GUILayout.Label(" ", new global::UnityEngine.GUILayoutOption[0]);
						global::UnityEngine.GUILayout.EndHorizontal();
						global::KGFGUIUtility.Separator(global::KGFGUIUtility.eStyleSeparator.eSeparatorVerticalFitInBox, new global::UnityEngine.GUILayoutOption[0]);
					}
				}
			}
		}
		global::UnityEngine.GUILayout.FlexibleSpace();
		global::KGFGUIUtility.EndHorizontalBox();
		global::KGFGUIUtility.BeginHorizontalBox(global::KGFGUIUtility.eStyleBox.eBoxDarkMiddleVertical, new global::UnityEngine.GUILayoutOption[0]);
		global::UnityEngine.GUILayout.Label(string.Empty, new global::UnityEngine.GUILayoutOption[0]);
		global::UnityEngine.GUILayout.FlexibleSpace();
		global::KGFGUIUtility.EndHorizontalBox();
		global::KGFGUIUtility.BeginVerticalBox(global::KGFGUIUtility.eStyleBox.eBoxDarkBottom, new global::UnityEngine.GUILayoutOption[0]);
		global::UnityEngine.GUILayout.BeginHorizontal(new global::UnityEngine.GUILayoutOption[0]);
		if (!global::UnityEngine.Application.isPlaying)
		{
			if (this.EventNew != null && global::KGFGUIUtility.Button("New", global::KGFGUIUtility.eStyleButton.eButton, new global::UnityEngine.GUILayoutOption[]
			{
				global::UnityEngine.GUILayout.Width(75f)
			}))
			{
				this.EventNew(this, null);
			}
			if (this.EventDelete != null && global::KGFGUIUtility.Button("Delete", global::KGFGUIUtility.eStyleButton.eButton, new global::UnityEngine.GUILayoutOption[]
			{
				global::UnityEngine.GUILayout.Width(75f)
			}))
			{
				this.EventDelete(this, null);
			}
			global::UnityEngine.GUILayout.FlexibleSpace();
		}
		if (this.itsDisplayFullTextSearch)
		{
			global::UnityEngine.GUI.SetNextControlName("KGFGuiObjectList.FullTextSearch");
			string a = global::KGFGUIUtility.TextField(this.itsFulltextSearch, global::KGFGUIUtility.eStyleTextField.eTextField, new global::UnityEngine.GUILayoutOption[]
			{
				global::UnityEngine.GUILayout.Width(200f)
			});
			if (a != this.itsFulltextSearch)
			{
				this.itsFulltextSearch = a;
				this.UpdateList();
			}
		}
		global::KGFGUIUtility.Space();
		bool flag = global::KGFGUIUtility.Toggle(this.itsIncludeAll, "all Tags", global::KGFGUIUtility.eStyleToggl.eTogglSuperCompact, new global::UnityEngine.GUILayoutOption[]
		{
			global::UnityEngine.GUILayout.Width(70f)
		});
		if (flag != this.itsIncludeAll)
		{
			this.itsIncludeAll = flag;
			this.UpdateList();
		}
		if (global::KGFGUIUtility.Button("clear filters", global::KGFGUIUtility.eStyleButton.eButton, new global::UnityEngine.GUILayoutOption[]
		{
			global::UnityEngine.GUILayout.Width(100f)
		}))
		{
			this.itsFulltextSearch = string.Empty;
			this.ClearFilters();
			this.UpdateList();
		}
		global::UnityEngine.GUILayout.FlexibleSpace();
		global::KGFGUIUtility.BeginHorizontalBox(global::KGFGUIUtility.eStyleBox.eBoxInvisible, new global::UnityEngine.GUILayoutOption[0]);
		if (this.GetDisplayEntriesPerPage())
		{
			if (global::KGFGUIUtility.Button("<", global::KGFGUIUtility.eStyleButton.eButtonLeft, new global::UnityEngine.GUILayoutOption[]
			{
				global::UnityEngine.GUILayout.Width(25f)
			}))
			{
				global::KGFGUIObjectList.KGFeItemsPerPage kgfeItemsPerPage = this.itsItemsPerPage;
				if (kgfeItemsPerPage != global::KGFGUIObjectList.KGFeItemsPerPage.e25)
				{
					if (kgfeItemsPerPage != global::KGFGUIObjectList.KGFeItemsPerPage.e50)
					{
						if (kgfeItemsPerPage != global::KGFGUIObjectList.KGFeItemsPerPage.e100)
						{
							if (kgfeItemsPerPage != global::KGFGUIObjectList.KGFeItemsPerPage.e250)
							{
								if (kgfeItemsPerPage == global::KGFGUIObjectList.KGFeItemsPerPage.e500)
								{
									this.itsItemsPerPage = global::KGFGUIObjectList.KGFeItemsPerPage.e250;
								}
							}
							else
							{
								this.itsItemsPerPage = global::KGFGUIObjectList.KGFeItemsPerPage.e100;
							}
						}
						else
						{
							this.itsItemsPerPage = global::KGFGUIObjectList.KGFeItemsPerPage.e50;
						}
					}
					else
					{
						this.itsItemsPerPage = global::KGFGUIObjectList.KGFeItemsPerPage.e25;
					}
				}
				else
				{
					this.itsItemsPerPage = global::KGFGUIObjectList.KGFeItemsPerPage.e10;
				}
			}
			global::KGFGUIUtility.BeginVerticalBox(global::KGFGUIUtility.eStyleBox.eBoxMiddleHorizontal, new global::UnityEngine.GUILayoutOption[0]);
			string theText = this.itsItemsPerPage.ToString().Substring(1) + " entries per page";
			global::KGFGUIUtility.Label(theText, global::KGFGUIUtility.eStyleLabel.eLabelFitIntoBox, new global::UnityEngine.GUILayoutOption[0]);
			global::KGFGUIUtility.EndVerticalBox();
			if (global::KGFGUIUtility.Button(">", global::KGFGUIUtility.eStyleButton.eButtonRight, new global::UnityEngine.GUILayoutOption[]
			{
				global::UnityEngine.GUILayout.Width(25f)
			}))
			{
				global::KGFGUIObjectList.KGFeItemsPerPage kgfeItemsPerPage = this.itsItemsPerPage;
				if (kgfeItemsPerPage != global::KGFGUIObjectList.KGFeItemsPerPage.e10)
				{
					if (kgfeItemsPerPage != global::KGFGUIObjectList.KGFeItemsPerPage.e25)
					{
						if (kgfeItemsPerPage != global::KGFGUIObjectList.KGFeItemsPerPage.e50)
						{
							if (kgfeItemsPerPage != global::KGFGUIObjectList.KGFeItemsPerPage.e100)
							{
								if (kgfeItemsPerPage == global::KGFGUIObjectList.KGFeItemsPerPage.e250)
								{
									this.itsItemsPerPage = global::KGFGUIObjectList.KGFeItemsPerPage.e500;
								}
							}
							else
							{
								this.itsItemsPerPage = global::KGFGUIObjectList.KGFeItemsPerPage.e250;
							}
						}
						else
						{
							this.itsItemsPerPage = global::KGFGUIObjectList.KGFeItemsPerPage.e100;
						}
					}
					else
					{
						this.itsItemsPerPage = global::KGFGUIObjectList.KGFeItemsPerPage.e50;
					}
				}
				else
				{
					this.itsItemsPerPage = global::KGFGUIObjectList.KGFeItemsPerPage.e25;
				}
			}
		}
		global::UnityEngine.GUILayout.Space(10f);
		if (global::KGFGUIUtility.Button("<", global::KGFGUIUtility.eStyleButton.eButtonLeft, new global::UnityEngine.GUILayoutOption[]
		{
			global::UnityEngine.GUILayout.Width(25f)
		}) && this.itsCurrentPage > 0)
		{
			this.itsCurrentPage--;
		}
		global::KGFGUIUtility.BeginVerticalBox(global::KGFGUIUtility.eStyleBox.eBoxMiddleHorizontal, new global::UnityEngine.GUILayoutOption[0]);
		string theText2 = string.Format("page {0}/{1}", this.itsCurrentPage + 1, global::System.Math.Max(num, 1));
		global::KGFGUIUtility.Label(theText2, global::KGFGUIUtility.eStyleLabel.eLabelFitIntoBox, new global::UnityEngine.GUILayoutOption[0]);
		global::KGFGUIUtility.EndVerticalBox();
		if (global::KGFGUIUtility.Button(">", global::KGFGUIUtility.eStyleButton.eButtonRight, new global::UnityEngine.GUILayoutOption[]
		{
			global::UnityEngine.GUILayout.Width(25f)
		}) && this.itsData.Rows.Count > (int)((this.itsCurrentPage + (global::KGFGUIObjectList.KGFeItemsPerPage)1) * this.itsItemsPerPage))
		{
			this.itsCurrentPage++;
		}
		global::KGFGUIUtility.EndHorizontalBox();
		global::UnityEngine.GUILayout.EndHorizontal();
		global::KGFGUIUtility.EndVerticalBox();
		global::UnityEngine.GUILayout.EndVertical();
		global::KGFGUIUtility.EndHorizontalBox();
		if (global::UnityEngine.GUI.GetNameOfFocusedControl().Equals("KGFGuiObjectList.FullTextSearch") && this.itsFulltextSearch.Equals("Search"))
		{
			this.itsFulltextSearch = string.Empty;
		}
		if (!global::UnityEngine.GUI.GetNameOfFocusedControl().Equals("KGFGuiObjectList.FullTextSearch") && this.itsFulltextSearch.Equals(string.Empty))
		{
			this.itsFulltextSearch = "Search";
		}
	}

	public void SetDisplayEntriesPerPage(bool theDisplay)
	{
		this.itsDisplayEntriesPerPage = theDisplay;
	}

	public bool GetDisplayEntriesPerPage()
	{
		return this.itsDisplayEntriesPerPage;
	}

	public bool GetRepaint()
	{
		return this.itsGuiData.GetRepaintWish() || this.itsRepaintWish;
	}

	private void OnClickRow(object theSender, global::System.EventArgs theArgs)
	{
		global::KGFDataRow kgfdataRow = theSender as global::KGFDataRow;
		if (kgfdataRow != null)
		{
			this.itsCurrentSelectedItem = (global::KGFITaggable)kgfdataRow[0].Value;
			if (this.itsCurrentSelectedRow != kgfdataRow)
			{
				this.itsCurrentSelectedRow = kgfdataRow;
			}
			if (this.EventSelect != null)
			{
				this.EventSelect(this, new global::KGFGUIObjectList.KGFGUIObjectListSelectEventArgs(this.itsCurrentSelectedItem));
			}
		}
	}

	private void OnCategoriesChanged(object theSender, global::System.EventArgs theArgs)
	{
		this.UpdateList();
		this.OnSettingsChanged();
	}

	private void OnSettingsChanged()
	{
		if (!this.itsLoadingActive && this.EventSettingsChanged != null)
		{
			this.EventSettingsChanged(this, null);
		}
	}

	private global::System.Collections.Generic.IEnumerable<string> GetAllTags()
	{
		foreach (global::KGFITaggable anItem in this.itsListData)
		{
			if (anItem.GetTags().Length == 0)
			{
				yield return "<untagged>";
			}
			foreach (string aTag in anItem.GetTags())
			{
				yield return aTag;
			}
		}
		yield break;
	}

	private void CacheTypeMembers()
	{
		this.itsDisplayFullTextSearch = false;
		this.itsData.Rows.Clear();
		this.itsData.Columns.Clear();
		this.itsListFieldCache.Clear();
		this.itsData.Columns.Add(new global::KGFDataColumn("DATA", this.itsItemType));
		foreach (global::System.Reflection.FieldInfo theMemberInfo in this.itsItemType.GetFields(global::System.Reflection.BindingFlags.Instance | global::System.Reflection.BindingFlags.Public | global::System.Reflection.BindingFlags.NonPublic))
		{
			this.TryAddMember(theMemberInfo);
		}
		foreach (global::System.Reflection.PropertyInfo theMemberInfo2 in this.itsItemType.GetProperties(global::System.Reflection.BindingFlags.Instance | global::System.Reflection.BindingFlags.Public | global::System.Reflection.BindingFlags.NonPublic))
		{
			this.TryAddMember(theMemberInfo2);
		}
	}

	private void TryAddMember(global::System.Reflection.MemberInfo theMemberInfo)
	{
		global::KGFObjectListItemDisplayAttribute[] array = theMemberInfo.GetCustomAttributes(typeof(global::KGFObjectListItemDisplayAttribute), true) as global::KGFObjectListItemDisplayAttribute[];
		if (array.Length == 1)
		{
			this.AddMember(theMemberInfo, array[0].Header, array[0].Searchable, array[0].Display);
		}
	}

	private bool FullTextFilter(global::KGFITaggable theItem)
	{
		if (this.itsFulltextSearch.Trim() == "Search")
		{
			return false;
		}
		foreach (string text in this.itsFulltextSearch.Trim().ToLower().Split(new char[]
		{
			' '
		}))
		{
			bool flag = false;
			string value = text;
			string text2 = null;
			string[] array2 = text.Split(new char[]
			{
				'='
			});
			if (array2.Length == 2)
			{
				value = array2[1];
				text2 = array2[0];
			}
			foreach (global::KGFGUIObjectList.KGFObjectListColumnItem kgfobjectListColumnItem in this.itsListFieldCache)
			{
				if (text2 == null || !(kgfobjectListColumnItem.itsHeader.ToLower() != text2.ToLower()))
				{
					object returnValue = kgfobjectListColumnItem.GetReturnValue(theItem);
					if (kgfobjectListColumnItem.itsSearchable)
					{
						if (returnValue is global::System.Collections.IEnumerable && !(returnValue is string))
						{
							foreach (object obj in ((global::System.Collections.IEnumerable)returnValue))
							{
								if (obj != null)
								{
									if (obj.ToString().Trim().ToLower().Contains(value))
									{
										flag = true;
									}
								}
							}
						}
						else
						{
							string text3 = returnValue.ToString();
							if (text3.Trim().ToLower().Contains(value))
							{
								flag = true;
							}
						}
					}
				}
			}
			if (!flag)
			{
				return true;
			}
		}
		return false;
	}

	private bool PerItemFilter(global::KGFITaggable theItem)
	{
		foreach (global::KGFGUIObjectList.KGFObjectListColumnItem kgfobjectListColumnItem in this.itsListFieldCache)
		{
			object returnValue = kgfobjectListColumnItem.GetReturnValue(theItem);
			if (kgfobjectListColumnItem.GetIsFiltered(returnValue))
			{
				return true;
			}
		}
		return false;
	}

	private void UpdateList()
	{
		if (global::UnityEngine.Event.current != null && global::UnityEngine.Event.current.type != global::UnityEngine.EventType.Layout)
		{
			this.itsUpdateWish = true;
			return;
		}
		this.itsUpdateWish = false;
		this.itsData.Rows.Clear();
		foreach (global::KGFITaggable kgfitaggable in this.itsListData)
		{
			if (this.GetIsTagSelected(kgfitaggable.GetTags()))
			{
				if (string.IsNullOrEmpty(this.itsFulltextSearch) || !this.FullTextFilter(kgfitaggable))
				{
					if (!this.PerItemFilter(kgfitaggable))
					{
						global::KGFDataRow kgfdataRow = this.itsData.NewRow();
						kgfdataRow[0].Value = kgfitaggable;
						int num = 1;
						foreach (global::KGFGUIObjectList.KGFObjectListColumnItem kgfobjectListColumnItem in this.itsListFieldCache)
						{
							object returnValue = kgfobjectListColumnItem.GetReturnValue(kgfitaggable);
							kgfdataRow[num].Value = returnValue;
							num++;
						}
						this.itsData.Rows.Add(kgfdataRow);
					}
				}
			}
		}
	}

	private bool GetIsTagSelected(string[] theTags)
	{
		global::System.Collections.Generic.List<object> list = new global::System.Collections.Generic.List<object>(this.itsListViewCategories.GetSelected());
		int count = list.Count;
		int num = 0;
		foreach (object obj in this.itsListViewCategories.GetSelected())
		{
			string text = (string)obj;
			if (theTags.Length == 0 && text == "<untagged>")
			{
				if (!this.itsIncludeAll)
				{
					return true;
				}
				num++;
			}
			foreach (string a in theTags)
			{
				if (a == text)
				{
					if (!this.itsIncludeAll)
					{
						return true;
					}
					num++;
				}
			}
		}
		return num == count && this.itsIncludeAll;
	}

	private void OnDropDownValueChanged(object theSender, global::System.EventArgs theArgs)
	{
		this.UpdateList();
		this.OnSettingsChanged();
	}

	private void DrawFilterBox(global::KGFGUIObjectList.KGFObjectListColumnItem theItem, uint theWidth)
	{
		if (theItem.GetReturnType().IsEnum || theItem.GetReturnType() == typeof(bool))
		{
			if (theItem.itsDropDown == null)
			{
				if (theItem.GetReturnType() == typeof(bool))
				{
					theItem.itsDropDown = new global::KGFGUIDropDown(new global::System.Collections.Generic.List<string>(this.itsBoolValues).InsertItem("<NONE>", 0), theWidth, 5U, global::KGFGUIDropDown.eDropDirection.eUp, new global::UnityEngine.GUILayoutOption[0]);
				}
				else if (theItem.GetReturnType().IsEnum)
				{
					theItem.itsDropDown = new global::KGFGUIDropDown(global::System.Enum.GetNames(theItem.GetReturnType()).InsertItem("<NONE>", 0), theWidth, 5U, global::KGFGUIDropDown.eDropDirection.eUp, new global::UnityEngine.GUILayoutOption[0]);
				}
				theItem.itsDropDown.itsTitle = string.Empty;
				theItem.itsDropDown.SetSelectedItem(theItem.itsFilterString);
				theItem.itsDropDown.SelectedValueChanged += this.OnDropDownValueChanged;
			}
			theItem.itsDropDown.Render();
		}
		else if (theItem.GetReturnType() == typeof(string))
		{
			if (theItem.itsFilterString == null)
			{
				theItem.itsFilterString = string.Empty;
			}
			string text = global::KGFGUIUtility.TextField(theItem.itsFilterString, global::KGFGUIUtility.eStyleTextField.eTextField, new global::UnityEngine.GUILayoutOption[]
			{
				global::UnityEngine.GUILayout.Width(theWidth)
			});
			if (text != theItem.itsFilterString)
			{
				theItem.itsFilterString = text;
				this.UpdateList();
				this.OnSettingsChanged();
			}
		}
	}

	private const string NONE_STRING = "<NONE>";

	private const string itsControlSearchName = "KGFGuiObjectList.FullTextSearch";

	private const string itsTextSearch = "Search";

	private const string UnTagged = "<untagged>";

	private global::System.Collections.Generic.List<global::KGFITaggable> itsListData;

	private global::System.Type itsItemType;

	private global::KGFDataTable itsData;

	private global::KGFGUIDataTable itsGuiData;

	private global::System.Collections.Generic.List<global::KGFGUIObjectList.KGFObjectListColumnItem> itsListFieldCache;

	private bool itsDisplayFullTextSearch;

	private string itsFulltextSearch = string.Empty;

	private global::KGFGUISelectionList itsListViewCategories;

	private global::KGFDataRow itsCurrentSelectedRow;

	private global::KGFITaggable itsCurrentSelectedItem;

	private global::KGFGUIObjectList.KGFeItemsPerPage itsItemsPerPage = global::KGFGUIObjectList.KGFeItemsPerPage.e50;

	private bool itsIncludeAll = true;

	public int itsCurrentPage;

	private bool itsLoadingActive;

	private bool itsDisplayEntriesPerPage = true;

	private bool itsRepaintWish;

	private bool itsUpdateWish;

	private string[] itsBoolValues = new string[]
	{
		"True",
		"False"
	};

	public enum KGFeItemsPerPage
	{
		e10 = 10,
		e25 = 25,
		e50 = 50,
		e100 = 100,
		e250 = 250,
		e500 = 500
	}

	public class KGFGUIObjectListSelectEventArgs : global::System.EventArgs
	{
		public KGFGUIObjectListSelectEventArgs(global::KGFITaggable theItem)
		{
			this.itsItem = theItem;
		}

		public global::KGFITaggable GetItem()
		{
			return this.itsItem;
		}

		private global::KGFITaggable itsItem;
	}

	private class KGFObjectListColumnItem
	{
		public KGFObjectListColumnItem(global::System.Reflection.MemberInfo theMemberInfo)
		{
			this.itsMemberInfo = theMemberInfo;
		}

		public global::System.Type GetReturnType()
		{
			if (this.itsMemberInfo is global::System.Reflection.FieldInfo)
			{
				return ((global::System.Reflection.FieldInfo)this.itsMemberInfo).FieldType;
			}
			if (this.itsMemberInfo is global::System.Reflection.PropertyInfo)
			{
				return ((global::System.Reflection.PropertyInfo)this.itsMemberInfo).PropertyType;
			}
			return null;
		}

		public object GetReturnValue(object theInstance)
		{
			if (this.itsMemberInfo is global::System.Reflection.FieldInfo)
			{
				return ((global::System.Reflection.FieldInfo)this.itsMemberInfo).GetValue(theInstance);
			}
			if (this.itsMemberInfo is global::System.Reflection.PropertyInfo)
			{
				return ((global::System.Reflection.PropertyInfo)this.itsMemberInfo).GetValue(theInstance, null);
			}
			return null;
		}

		public bool GetIsFiltered(object theInstance)
		{
			if (this.GetReturnType() == typeof(bool) || this.GetReturnType().IsEnum)
			{
				if (this.itsDropDown != null)
				{
					if (this.itsDropDown.SelectedItem() == "<NONE>")
					{
						return false;
					}
					if (this.itsDropDown.SelectedItem() != theInstance.ToString())
					{
						return true;
					}
				}
				return false;
			}
			return !string.IsNullOrEmpty(this.itsFilterString) && !theInstance.ToString().ToLower().Contains(this.itsFilterString.ToLower());
		}

		public string itsHeader;

		public bool itsSearchable;

		public bool itsDisplay;

		public global::KGFGUIDropDown itsDropDown;

		public string itsFilterString = string.Empty;

		private global::System.Reflection.MemberInfo itsMemberInfo;
	}
}
