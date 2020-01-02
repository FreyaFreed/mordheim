using System;

public class EndGameInjuryModule : global::UIModule
{
	public void Setup(global::MissionEndUnitSave unit)
	{
		for (int i = 0; i < this.injury.Length; i++)
		{
			this.injury[i].gameObject.SetActive(false);
		}
		for (int j = 0; j < unit.injuries.Count; j++)
		{
			if (unit.injuries[j].Duration > 0)
			{
				this.injury[j].SetLocalized(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("injury_name_" + unit.injuries[j].Name), global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("injury_desc_" + unit.injuries[j].Name), global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("end_game_recovery_time_value", new string[]
				{
					unit.injuries[j].Duration.ToString()
				}));
			}
			else
			{
				this.injury[j].SetLocalized(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("injury_name_" + unit.injuries[j].Name), global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("injury_desc_" + unit.injuries[j].Name));
			}
			this.showQueue.Enqueue(this.injury[j].gameObject);
		}
		base.StartShow(0.5f);
	}

	public global::UIDescription[] injury;
}
