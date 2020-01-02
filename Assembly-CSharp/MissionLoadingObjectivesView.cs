using System;
using UnityEngine;

public class MissionLoadingObjectivesView : global::UnityEngine.MonoBehaviour
{
	private void Awake()
	{
		this.mainObjectiveTemplate.SetActive(false);
		this.secondaryObjectiveTemplate.SetActive(false);
	}

	public void Add(bool done, string text, bool isPrimary)
	{
		global::UnityEngine.GameObject gameObject = global::UnityEngine.Object.Instantiate<global::UnityEngine.GameObject>((!isPrimary) ? this.secondaryObjectiveTemplate : this.mainObjectiveTemplate);
		gameObject.transform.SetParent(base.gameObject.transform, false);
		gameObject.SetActive(true);
		global::ObjectiveView component = gameObject.GetComponent<global::ObjectiveView>();
		component.toggleObjective.isOn = done;
		component.objectiveText.text = text;
	}

	public void Add(global::Objective objective)
	{
		this.Add(objective.done, global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(objective.desc), true);
		int num = 0;
		while ((float)num < objective.counter.y)
		{
			if (objective.TypeId == global::PrimaryObjectiveTypeId.BOUNTY)
			{
				this.Add(objective.dones[num], global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(objective.subDesc[2 * num]), false);
			}
			else
			{
				this.Add(objective.dones[num], global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(objective.subDesc[num]), false);
			}
			num++;
		}
	}

	public global::UnityEngine.GameObject mainObjectiveTemplate;

	public global::UnityEngine.GameObject secondaryObjectiveTemplate;
}
