using System;

public class KGFObjectListItemDisplayAttribute : global::System.Attribute
{
	public KGFObjectListItemDisplayAttribute(string theHeader)
	{
		this.itsHeader = theHeader;
		this.itsSearchable = false;
		this.itsDisplay = true;
	}

	public KGFObjectListItemDisplayAttribute(string theHeader, bool theSearchable)
	{
		this.itsHeader = theHeader;
		this.itsSearchable = theSearchable;
		this.itsDisplay = true;
	}

	public KGFObjectListItemDisplayAttribute(string theHeader, bool theSearchable, bool theDisplay)
	{
		this.itsHeader = theHeader;
		this.itsSearchable = theSearchable;
		this.itsDisplay = theDisplay;
	}

	public string Header
	{
		get
		{
			return this.itsHeader;
		}
	}

	public bool Searchable
	{
		get
		{
			return this.itsSearchable;
		}
	}

	public bool Display
	{
		get
		{
			return this.itsDisplay;
		}
	}

	private string itsHeader;

	private bool itsSearchable;

	private bool itsDisplay;
}
