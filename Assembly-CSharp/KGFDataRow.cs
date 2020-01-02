using System;
using System.Collections.Generic;

public class KGFDataRow
{
	public KGFDataRow()
	{
		this.itsTable = null;
	}

	public KGFDataRow(global::KGFDataTable theTable)
	{
		this.itsTable = theTable;
		foreach (global::KGFDataColumn theColumn in theTable.Columns)
		{
			this.itsCells.Add(new global::KGFDataCell(theColumn, this));
		}
	}

	public global::KGFDataCell this[int theIndex]
	{
		get
		{
			if (theIndex >= 0 && theIndex < this.itsTable.Columns.Count)
			{
				return this.itsCells[theIndex];
			}
			throw new global::System.ArgumentOutOfRangeException();
		}
		set
		{
			if (theIndex >= 0 && theIndex < this.itsTable.Columns.Count)
			{
				this.itsCells[theIndex] = value;
				return;
			}
			throw new global::System.ArgumentOutOfRangeException();
		}
	}

	public global::KGFDataCell this[string theName]
	{
		get
		{
			foreach (global::KGFDataCell kgfdataCell in this.itsCells)
			{
				if (kgfdataCell.Column.ColumnName.Equals(theName))
				{
					return kgfdataCell;
				}
			}
			throw new global::System.ArgumentOutOfRangeException();
		}
		set
		{
			bool flag = false;
			for (int i = 0; i < this.itsCells.Count; i++)
			{
				if (this.itsCells[i].Column.ColumnName.Equals(theName))
				{
					this.itsCells[i] = value;
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				throw new global::System.ArgumentOutOfRangeException();
			}
		}
	}

	public global::KGFDataCell this[global::KGFDataColumn theColumn]
	{
		get
		{
			for (int i = 0; i < this.itsTable.Columns.Count; i++)
			{
				if (this.itsCells[i].Column.Equals(theColumn))
				{
					return this.itsCells[i];
				}
			}
			throw new global::System.ArgumentOutOfRangeException();
		}
		set
		{
			for (int i = 0; i < this.itsTable.Columns.Count; i++)
			{
				if (this.itsCells[i].Column.Equals(theColumn))
				{
					this.itsCells[i] = value;
				}
			}
			throw new global::System.ArgumentOutOfRangeException();
		}
	}

	public bool IsNull(global::KGFDataColumn theColumn)
	{
		return this.IsNull(theColumn.ColumnName);
	}

	public bool IsNull(string theColumn)
	{
		foreach (global::KGFDataCell kgfdataCell in this.itsCells)
		{
			if (kgfdataCell.Column.ColumnName.Equals(theColumn) && kgfdataCell.Value != null)
			{
				return false;
			}
		}
		return true;
	}

	private global::KGFDataTable itsTable;

	private global::System.Collections.Generic.List<global::KGFDataCell> itsCells = new global::System.Collections.Generic.List<global::KGFDataCell>();
}
