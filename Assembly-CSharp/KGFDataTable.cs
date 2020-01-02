using System;
using System.Collections.Generic;

public class KGFDataTable
{
	public global::System.Collections.Generic.List<global::KGFDataColumn> Columns
	{
		get
		{
			return this.itsColumns;
		}
	}

	public global::System.Collections.Generic.List<global::KGFDataRow> Rows
	{
		get
		{
			return this.itsRows;
		}
	}

	public global::KGFDataRow NewRow()
	{
		return new global::KGFDataRow(this);
	}

	private global::System.Collections.Generic.List<global::KGFDataColumn> itsColumns = new global::System.Collections.Generic.List<global::KGFDataColumn>();

	private global::System.Collections.Generic.List<global::KGFDataRow> itsRows = new global::System.Collections.Generic.List<global::KGFDataRow>();
}
