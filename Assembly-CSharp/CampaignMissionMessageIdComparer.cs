using System;
using System.Collections.Generic;

public class CampaignMissionMessageIdComparer : global::System.Collections.Generic.IEqualityComparer<global::CampaignMissionMessageId>
{
	public bool Equals(global::CampaignMissionMessageId x, global::CampaignMissionMessageId y)
	{
		return x == y;
	}

	public int GetHashCode(global::CampaignMissionMessageId obj)
	{
		return (int)obj;
	}

	public static readonly global::CampaignMissionMessageIdComparer Instance = new global::CampaignMissionMessageIdComparer();
}
