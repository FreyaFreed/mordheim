using System;
using System.Collections.Generic;

public class CampaignMissionIdComparer : global::System.Collections.Generic.IEqualityComparer<global::CampaignMissionId>
{
	public bool Equals(global::CampaignMissionId x, global::CampaignMissionId y)
	{
		return x == y;
	}

	public int GetHashCode(global::CampaignMissionId obj)
	{
		return (int)obj;
	}

	public static readonly global::CampaignMissionIdComparer Instance = new global::CampaignMissionIdComparer();
}
