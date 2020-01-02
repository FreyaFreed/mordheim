using System;

public class KGFDataCell
{
	public KGFDataCell(global::KGFDataColumn theColumn, global::KGFDataRow theRow)
	{
		this.itsColumn = theColumn;
		this.itsRow = theRow;
		this.itsValue = null;
	}

	public global::KGFDataColumn Column
	{
		get
		{
			return this.itsColumn;
		}
	}

	public global::KGFDataRow Row
	{
		get
		{
			return this.itsRow;
		}
	}

	public object Value
	{
		get
		{
			return this.itsValue;
		}
		set
		{
			this.itsValue = value;
		}
	}

	public override string ToString()
	{
		return this.itsValue.ToString();
	}

	private global::KGFDataColumn itsColumn;

	private global::KGFDataRow itsRow;

	private object itsValue;
}
