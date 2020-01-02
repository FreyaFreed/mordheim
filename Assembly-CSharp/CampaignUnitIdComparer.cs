using System;
using System.Collections.Generic;

public class CampaignUnitIdComparer : global::System.Collections.Generic.IEqualityComparer<global::CampaignUnitId>
{
	public bool Equals(global::CampaignUnitId x, global::CampaignUnitId y)
	{
		return x == y;
	}

	public int GetHashCode(global::CampaignUnitId obj)
	{
		return (int)obj;
	}

	public static readonly global::CampaignUnitIdComparer Instance = new global::CampaignUnitIdComparer();
}
