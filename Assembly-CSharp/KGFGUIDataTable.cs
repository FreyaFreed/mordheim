using System;
using System.Collections.Generic;
using UnityEngine;

public class KGFGUIDataTable : global::KGFIControl
{
	public KGFGUIDataTable(global::KGFDataTable theDataTable, params global::UnityEngine.GUILayoutOption[] theLayout)
	{
		this.itsDataTable = theDataTable;
		foreach (global::KGFDataColumn key in this.itsDataTable.Columns)
		{
			this.itsColumnWidth.Add(key, 0U);
			this.itsColumnVisible.Add(key, true);
		}
	}

	public event global::System.EventHandler PreRenderRow;

	public event global::System.EventHandler PostRenderRow;

	public event global::System.EventHandler PreRenderColumn;

	public event global::System.EventHandler PostRenderColumn;

	public event global::System.Func<global::KGFDataRow, global::KGFDataColumn, uint, bool> PreCellContentHandler;

	public event global::System.EventHandler OnClickRow;

	public event global::System.EventHandler EventSettingsChanged;

	private static void LoadTextures()
	{
		string str = "KGFCore/textures/";
		global::KGFGUIDataTable.itsTextureArrowUp = (global::UnityEngine.Texture2D)global::UnityEngine.Resources.Load(str + "arrow_up", typeof(global::UnityEngine.Texture2D));
		global::KGFGUIDataTable.itsTextureArrowDown = (global::UnityEngine.Texture2D)global::UnityEngine.Resources.Load(str + "arrow_down", typeof(global::UnityEngine.Texture2D));
	}

	public uint GetStartRow()
	{
		return this.itsStartRow;
	}

	public void SetStartRow(uint theStartRow)
	{
		this.itsStartRow = (uint)global::System.Math.Min((long)((ulong)theStartRow), (long)this.itsDataTable.Rows.Count);
	}

	public uint GetDisplayRowCount()
	{
		return this.itsDisplayRowCount;
	}

	public void SetDisplayRowCount(uint theDisplayRowCount)
	{
		this.itsDisplayRowCount = (uint)global::System.Math.Min((long)((ulong)theDisplayRowCount), (long)this.itsDataTable.Rows.Count - (long)((ulong)this.itsStartRow));
	}

	public void SetColumnVisible(int theColumIndex, bool theValue)
	{
		if (theColumIndex >= 0 && theColumIndex < this.itsDataTable.Columns.Count)
		{
			this.itsColumnVisible[this.itsDataTable.Columns[theColumIndex]] = theValue;
		}
	}

	public bool GetColumnVisible(int theColumIndex)
	{
		return theColumIndex >= 0 && theColumIndex < this.itsDataTable.Columns.Count && this.itsColumnVisible[this.itsDataTable.Columns[theColumIndex]];
	}

	public void SetColumnWidth(int theColumIndex, uint theValue)
	{
		if (theColumIndex >= 0 && theColumIndex < this.itsDataTable.Columns.Count)
		{
			this.itsColumnWidth[this.itsDataTable.Columns[theColumIndex]] = theValue;
		}
	}

	public uint GetColumnWidth(int theColumIndex)
	{
		if (theColumIndex >= 0 && theColumIndex < this.itsDataTable.Columns.Count)
		{
			return this.itsColumnWidth[this.itsDataTable.Columns[theColumIndex]];
		}
		return 0U;
	}

	public global::KGFDataRow GetCurrentSelected()
	{
		return this.itsCurrentSelected;
	}

	public void SetCurrentSelected(global::KGFDataRow theDataRow)
	{
		if (this.itsDataTable.Rows.Contains(theDataRow))
		{
			this.itsCurrentSelected = theDataRow;
		}
	}

	private void RenderTableHeadings()
	{
		if (global::KGFGUIDataTable.itsTextureArrowDown == null)
		{
			global::KGFGUIDataTable.LoadTextures();
		}
		global::KGFGUIUtility.BeginHorizontalBox(global::KGFGUIUtility.eStyleBox.eBoxDarkTop, new global::UnityEngine.GUILayoutOption[]
		{
			global::UnityEngine.GUILayout.ExpandWidth(true),
			global::UnityEngine.GUILayout.ExpandHeight(false)
		});
		foreach (global::KGFDataColumn kgfdataColumn in this.itsDataTable.Columns)
		{
			if (this.itsColumnVisible[kgfdataColumn])
			{
				global::UnityEngine.GUILayoutOption[] options;
				if (this.itsColumnWidth[kgfdataColumn] != 0U)
				{
					options = new global::UnityEngine.GUILayoutOption[]
					{
						global::UnityEngine.GUILayout.Width(this.itsColumnWidth[kgfdataColumn])
					};
				}
				else
				{
					options = new global::UnityEngine.GUILayoutOption[]
					{
						global::UnityEngine.GUILayout.ExpandWidth(true)
					};
				}
				global::UnityEngine.GUILayout.BeginHorizontal(options);
				global::KGFGUIUtility.Label(kgfdataColumn.ColumnName, global::KGFGUIUtility.eStyleLabel.eLabelFitIntoBox, new global::UnityEngine.GUILayoutOption[0]);
				if (kgfdataColumn == this.itsSortColumn)
				{
					if (this.itsSortDirection)
					{
						global::KGFGUIUtility.Label(string.Empty, global::KGFGUIDataTable.itsTextureArrowDown, global::KGFGUIUtility.eStyleLabel.eLabelFitIntoBox, new global::UnityEngine.GUILayoutOption[]
						{
							global::UnityEngine.GUILayout.Width(14f)
						});
					}
					else
					{
						global::KGFGUIUtility.Label(string.Empty, global::KGFGUIDataTable.itsTextureArrowUp, global::KGFGUIUtility.eStyleLabel.eLabelFitIntoBox, new global::UnityEngine.GUILayoutOption[]
						{
							global::UnityEngine.GUILayout.Width(14f)
						});
					}
				}
				global::UnityEngine.GUILayout.EndHorizontal();
				if (global::UnityEngine.Event.current.type == global::UnityEngine.EventType.MouseUp && global::UnityEngine.GUILayoutUtility.GetLastRect().Contains(global::UnityEngine.Event.current.mousePosition))
				{
					this.SortColumn(kgfdataColumn);
				}
				global::KGFGUIUtility.Separator(global::KGFGUIUtility.eStyleSeparator.eSeparatorVerticalFitInBox, new global::UnityEngine.GUILayoutOption[0]);
			}
		}
		global::KGFGUIUtility.EndHorizontalBox();
	}

	private void SortColumn(global::KGFDataColumn theColumn)
	{
		if (this.itsSortColumn != theColumn)
		{
			this.SetSortingColumn(theColumn);
			this.itsSortDirection = false;
			this.itsDataTable.Rows.Sort(new global::System.Comparison<global::KGFDataRow>(this.RowComparison));
		}
		else
		{
			this.itsSortDirection = !this.itsSortDirection;
			this.itsDataTable.Rows.Reverse();
		}
		this.itsRepaint = true;
	}

	private int RowComparison(global::KGFDataRow theRow1, global::KGFDataRow theRow2)
	{
		if (this.itsSortColumn != null)
		{
			return theRow1[this.itsSortColumn].Value.ToString().CompareTo(theRow2[this.itsSortColumn].Value.ToString());
		}
		return 0;
	}

	private void RenderTableRows()
	{
		this.itsDataTableScrollViewPosition = global::KGFGUIUtility.BeginScrollView(this.itsDataTableScrollViewPosition, false, true, new global::UnityEngine.GUILayoutOption[]
		{
			global::UnityEngine.GUILayout.ExpandHeight(true)
		});
		if (this.itsDataTable.Rows.Count > 0)
		{
			global::UnityEngine.GUILayout.BeginVertical(new global::UnityEngine.GUILayoutOption[0]);
			global::UnityEngine.Color color = global::UnityEngine.GUI.color;
			int num = (int)this.itsStartRow;
			while ((long)num < (long)((ulong)(this.itsStartRow + this.itsDisplayRowCount)) && num < this.itsDataTable.Rows.Count)
			{
				global::KGFDataRow kgfdataRow = this.itsDataTable.Rows[num];
				if (this.PreRenderRow != null)
				{
					this.PreRenderRow(kgfdataRow, global::System.EventArgs.Empty);
				}
				if (kgfdataRow == this.itsCurrentSelected)
				{
					global::KGFGUIUtility.BeginHorizontalBox(global::KGFGUIUtility.eStyleBox.eBoxDarkTopInteractive, new global::UnityEngine.GUILayoutOption[]
					{
						global::UnityEngine.GUILayout.ExpandWidth(true)
					});
				}
				else
				{
					global::KGFGUIUtility.BeginHorizontalBox(global::KGFGUIUtility.eStyleBox.eBoxMiddleVerticalInteractive, new global::UnityEngine.GUILayoutOption[]
					{
						global::UnityEngine.GUILayout.ExpandWidth(true)
					});
				}
				foreach (global::KGFDataColumn kgfdataColumn in this.itsDataTable.Columns)
				{
					if (this.itsColumnVisible[kgfdataColumn])
					{
						if (this.PreRenderColumn != null)
						{
							this.PreRenderColumn(kgfdataColumn, global::System.EventArgs.Empty);
						}
						bool flag = false;
						if (this.PreCellContentHandler != null)
						{
							flag = this.PreCellContentHandler(kgfdataRow, kgfdataColumn, this.itsColumnWidth[kgfdataColumn]);
						}
						if (!flag)
						{
							int num2 = 85;
							string text = kgfdataRow[kgfdataColumn].ToString().Substring(0, global::System.Math.Min(num2, kgfdataRow[kgfdataColumn].ToString().Length));
							if (text.Length == num2)
							{
								text += "...";
							}
							if (this.itsColumnWidth[kgfdataColumn] > 0U)
							{
								global::KGFGUIUtility.Label(text, global::KGFGUIUtility.eStyleLabel.eLabelFitIntoBox, new global::UnityEngine.GUILayoutOption[]
								{
									global::UnityEngine.GUILayout.Width(this.itsColumnWidth[kgfdataColumn])
								});
							}
							else
							{
								global::KGFGUIUtility.Label(text, global::KGFGUIUtility.eStyleLabel.eLabelFitIntoBox, new global::UnityEngine.GUILayoutOption[]
								{
									global::UnityEngine.GUILayout.ExpandWidth(true)
								});
							}
						}
						global::KGFGUIUtility.Separator(global::KGFGUIUtility.eStyleSeparator.eSeparatorVerticalFitInBox, new global::UnityEngine.GUILayoutOption[0]);
						if (this.PostRenderColumn != null)
						{
							this.PostRenderColumn(kgfdataColumn, global::System.EventArgs.Empty);
						}
					}
				}
				global::KGFGUIUtility.EndHorizontalBox();
				if (global::UnityEngine.GUILayoutUtility.GetLastRect().Contains(global::UnityEngine.Event.current.mousePosition) && global::UnityEngine.Event.current.type == global::UnityEngine.EventType.MouseDown && global::UnityEngine.Event.current.button == 0)
				{
					this.itsClickedRow = kgfdataRow;
					this.itsRepaint = true;
				}
				if (this.OnClickRow != null && this.itsClickedRow != null && global::UnityEngine.Event.current.type == global::UnityEngine.EventType.Layout)
				{
					if (this.itsCurrentSelected != this.itsClickedRow)
					{
						this.itsCurrentSelected = this.itsClickedRow;
					}
					else
					{
						this.itsCurrentSelected = null;
					}
					this.OnClickRow(this.itsClickedRow, global::System.EventArgs.Empty);
					this.itsClickedRow = null;
				}
				if (this.PostRenderRow != null)
				{
					this.PostRenderRow(kgfdataRow, global::System.EventArgs.Empty);
				}
				num++;
			}
			global::UnityEngine.GUI.color = color;
			global::UnityEngine.GUILayout.FlexibleSpace();
			global::UnityEngine.GUILayout.EndVertical();
		}
		else
		{
			global::UnityEngine.GUILayout.Label("no items found", new global::UnityEngine.GUILayoutOption[0]);
			global::UnityEngine.GUILayout.FlexibleSpace();
		}
		global::UnityEngine.GUILayout.EndScrollView();
		this.itsRectScrollView = global::UnityEngine.GUILayoutUtility.GetLastRect();
	}

	public global::UnityEngine.Rect GetLastRectScrollview()
	{
		return this.itsRectScrollView;
	}

	public bool GetRepaintWish()
	{
		bool result = this.itsRepaint;
		this.itsRepaint = false;
		return result;
	}

	public void SetSortingColumn(string theColumnName)
	{
		foreach (global::KGFDataColumn kgfdataColumn in this.itsDataTable.Columns)
		{
			if (kgfdataColumn.ColumnName == theColumnName)
			{
				this.itsSortColumn = kgfdataColumn;
				this.itsRepaint = true;
				break;
			}
		}
	}

	public void SetSortingColumn(global::KGFDataColumn theColumn)
	{
		this.itsSortColumn = theColumn;
		this.itsRepaint = true;
		if (this.EventSettingsChanged != null)
		{
			this.EventSettingsChanged(this, null);
		}
	}

	public global::KGFDataColumn GetSortingColumn()
	{
		return this.itsSortColumn;
	}

	public string SaveSettings()
	{
		return string.Format("SortBy:" + ((this.itsSortColumn == null) ? string.Empty : this.itsSortColumn.ColumnName), new object[0]);
	}

	public void LoadSettings(string theSettingsString)
	{
		string[] array = theSettingsString.Split(new char[]
		{
			':'
		});
		if (array.Length == 2 && array[0] == "SortBy")
		{
			if (array[1].Trim() == string.Empty)
			{
				this.SetSortingColumn(null);
			}
			else
			{
				this.SetSortingColumn(array[1]);
			}
		}
	}

	public void Render()
	{
		if (this.itsVisible)
		{
			global::UnityEngine.GUILayout.BeginVertical(new global::UnityEngine.GUILayoutOption[0]);
			this.RenderTableHeadings();
			this.RenderTableRows();
			global::UnityEngine.GUILayout.EndVertical();
		}
	}

	public string GetName()
	{
		return "KGFGUIDataTable";
	}

	public bool IsVisible()
	{
		return this.itsVisible;
	}

	private global::KGFDataTable itsDataTable;

	private global::UnityEngine.Vector2 itsDataTableScrollViewPosition;

	private uint itsStartRow;

	private uint itsDisplayRowCount = 100U;

	private global::System.Collections.Generic.Dictionary<global::KGFDataColumn, uint> itsColumnWidth = new global::System.Collections.Generic.Dictionary<global::KGFDataColumn, uint>();

	private global::System.Collections.Generic.Dictionary<global::KGFDataColumn, bool> itsColumnVisible = new global::System.Collections.Generic.Dictionary<global::KGFDataColumn, bool>();

	private global::KGFDataRow itsClickedRow;

	private global::KGFDataRow itsCurrentSelected;

	private bool itsVisible = true;

	private static global::UnityEngine.Texture2D itsTextureArrowUp;

	private static global::UnityEngine.Texture2D itsTextureArrowDown;

	private global::KGFDataColumn itsSortColumn;

	private bool itsSortDirection;

	private global::UnityEngine.Rect itsRectScrollView = default(global::UnityEngine.Rect);

	private bool itsRepaint;
}
