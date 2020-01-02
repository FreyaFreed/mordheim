using System;

[global::System.Serializable]
public class IDReference
{
	public string GetID()
	{
		return this.itsID;
	}

	public void SetID(string theID)
	{
		this.itsID = theID;
		this.itsEmpty = false;
	}

	public bool GetHasValue()
	{
		return !this.itsEmpty;
	}

	public void SetEmpty()
	{
		this.itsEmpty = true;
	}

	public override string ToString()
	{
		return this.GetID();
	}

	public bool GetCanBeDeleted()
	{
		return this.itsCanBeDeleted;
	}

	public void SetCanBeDeleted(bool theCanBeDeleted)
	{
		this.itsCanBeDeleted = theCanBeDeleted;
	}

	public string itsID = string.Empty;

	public bool itsEmpty = true;

	public bool itsCanBeDeleted;
}
