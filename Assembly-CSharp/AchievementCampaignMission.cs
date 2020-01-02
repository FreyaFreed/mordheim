using System;

public class AchievementCampaignMission : global::Achievement
{
	public AchievementCampaignMission(global::AchievementData data) : base(data)
	{
	}

	public override bool CheckFinishMission(global::CampaignMissionId missionId)
	{
		return base.Data.CampaignMissionId == missionId;
	}
}
