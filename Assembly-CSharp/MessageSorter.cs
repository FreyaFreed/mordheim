using System;
using System.Collections.Generic;

public class MessageSorter : global::System.Collections.Generic.IComparer<global::CampaignMissionMessageData>
{
	int global::System.Collections.Generic.IComparer<global::CampaignMissionMessageData>.Compare(global::CampaignMissionMessageData x, global::CampaignMissionMessageData y)
	{
		if (x.Position > y.Position)
		{
			return 1;
		}
		if (x.Position < y.Position)
		{
			return -1;
		}
		return 0;
	}
}
