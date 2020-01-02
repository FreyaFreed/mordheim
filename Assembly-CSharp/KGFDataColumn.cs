using System;

public class KGFDataColumn
{
	public KGFDataColumn(string theName, global::System.Type theType)
	{
		this.itsName = theName;
		this.itsType = theType;
	}

	public void Add(string theName, global::System.Type theType)
	{
		this.itsName = theName;
		this.itsType = theType;
	}

	public string ColumnName
	{
		get
		{
			return this.itsName;
		}
		set
		{
			this.itsName = value;
		}
	}

	public global::System.Type ColumnType
	{
		get
		{
			return this.itsType;
		}
		set
		{
			this.itsType = value;
		}
	}

	private string itsName;

	private global::System.Type itsType;
}
