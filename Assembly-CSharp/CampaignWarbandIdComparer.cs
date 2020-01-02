using System;
using System.Collections.Generic;

public class CampaignWarbandIdComparer : global::System.Collections.Generic.IEqualityComparer<global::CampaignWarbandId>
{
	public bool Equals(global::CampaignWarbandId x, global::CampaignWarbandId y)
	{
		return x == y;
	}

	public int GetHashCode(global::CampaignWarbandId obj)
	{
		return (int)obj;
	}

	public static readonly global::CampaignWarbandIdComparer Instance = new global::CampaignWarbandIdComparer();
}
