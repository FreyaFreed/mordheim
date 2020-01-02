using System;
using System.Collections.Generic;

public class CampaignMissionSpawnIdComparer : global::System.Collections.Generic.IEqualityComparer<global::CampaignMissionSpawnId>
{
	public bool Equals(global::CampaignMissionSpawnId x, global::CampaignMissionSpawnId y)
	{
		return x == y;
	}

	public int GetHashCode(global::CampaignMissionSpawnId obj)
	{
		return (int)obj;
	}

	public static readonly global::CampaignMissionSpawnIdComparer Instance = new global::CampaignMissionSpawnIdComparer();
}
