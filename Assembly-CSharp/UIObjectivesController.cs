using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class UIObjectivesController : global::CanvasGroupDisabler
{
	private void Awake()
	{
		global::PandoraSingleton<global::NoticeManager>.Instance.RegisterListener(global::Notices.GAME_OBJECTIVE_UPDATE, new global::DelReceiveNotice(this.UpdateObjective));
		this.mainGroup = base.GetComponent<global::ListGroup>();
		this.mainGroup.Setup(null, this.ObjectiveCatPrefab);
	}

	private void UpdateObjective()
	{
		if (this.objectiveViews.Count == 0)
		{
			this.objectiveViews = new global::System.Collections.Generic.List<global::ObjectiveView>();
			this.mainGroup.ClearList();
			global::System.Collections.Generic.List<global::Objective> objectives = (global::System.Collections.Generic.List<global::Objective>)global::PandoraSingleton<global::NoticeManager>.Instance.Parameters[0];
			this.SetObjectives(objectives, false);
			if (!global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.isCampaign)
			{
				global::UnityEngine.GameObject cat = this.mainGroup.AddToList();
				global::ListGroup listGroup = this.SetCategory(cat, "mission_battleground_ressources", this.ObjectivePrefab, false);
				global::UnityEngine.Vector2 vector = (global::UnityEngine.Vector2)global::PandoraSingleton<global::NoticeManager>.Instance.Parameters[1];
				this.AddObjective(listGroup, null, -1, global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("mission_counter_search"), false, (int)vector.x, (int)vector.y);
				global::UnityEngine.Vector2 vector2 = (global::UnityEngine.Vector2)global::PandoraSingleton<global::NoticeManager>.Instance.Parameters[2];
				this.AddObjective(listGroup, null, -1, global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("mission_counter_wyrdstone_collected"), false, (int)vector2.x, (int)vector2.y);
			}
		}
		else if (!global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.isCampaign)
		{
			int i;
			for (i = 0; i < this.objectiveViews.Count - 2; i++)
			{
				this.objectiveViews[i].UpdateObjective(false, -1, -1);
			}
			global::UnityEngine.Vector2 vector3 = (global::UnityEngine.Vector2)global::PandoraSingleton<global::NoticeManager>.Instance.Parameters[1];
			this.objectiveViews[i++].UpdateObjective(false, (int)vector3.x, (int)vector3.y);
			global::UnityEngine.Vector2 vector4 = (global::UnityEngine.Vector2)global::PandoraSingleton<global::NoticeManager>.Instance.Parameters[2];
			this.objectiveViews[i].UpdateObjective(false, (int)vector4.x, (int)vector4.y);
		}
		else
		{
			for (int j = 0; j < this.objectiveViews.Count; j++)
			{
				this.objectiveViews[j].UpdateObjective(false, -1, -1);
			}
		}
	}

	private global::ListGroup SetCategory(global::UnityEngine.GameObject cat, string title, global::UnityEngine.GameObject prefab, bool isOptional)
	{
		global::ListGroup component = cat.GetComponent<global::ListGroup>();
		if (!string.IsNullOrEmpty(title))
		{
			title = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(title);
			if (isOptional)
			{
				title = title + " " + global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("mission_obj_optional");
			}
		}
		component.SetupLocalized(title, prefab);
		return component;
	}

	public void SetObjectives(global::System.Collections.Generic.List<global::Objective> objectives, bool loading = false)
	{
		if (loading)
		{
			global::PandoraSingleton<global::NoticeManager>.Instance.RemoveListener(global::Notices.GAME_OBJECTIVE_UPDATE, new global::DelReceiveNotice(this.UpdateObjective));
		}
		if (objectives == null)
		{
			return;
		}
		if (!global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.isCampaign || global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.isTuto)
		{
			global::UnityEngine.GameObject cat = this.mainGroup.AddToList();
			global::ListGroup listGroup = this.SetCategory(cat, "mission_battleground", this.ObjectivePrefab, false);
			this.AddObjective(listGroup, null, -1, global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("battleground_objective"), loading, -1, -1);
		}
		if (objectives.Count > 0 && objectives[0] != null)
		{
			global::UnityEngine.GameObject cat = this.mainGroup.AddToList();
			bool isOptional = global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.VictoryTypeId != 2;
			global::ListGroup listGroup;
			if (global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.isCampaign)
			{
				listGroup = this.SetCategory(cat, "mission_objectives", this.ObjectivePrefab, isOptional);
			}
			else
			{
				listGroup = this.SetCategory(cat, "mission_obj_" + global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.GetObjectiveTypeId(global::PandoraSingleton<global::Hermes>.Instance.PlayerIndex), this.ObjectivePrefab, isOptional);
			}
			for (int i = 0; i < objectives.Count; i++)
			{
				global::UnityEngine.GameObject cat2 = this.AddObjective(listGroup, objectives[i], -1, objectives[i].desc, loading, -1, -1);
				if (objectives[i].subDesc.Count > 0)
				{
					global::ListGroup listGroup2 = this.SetCategory(cat2, string.Empty, this.SubObjectivePrefab, false);
					for (int j = 0; j < objectives[i].subDesc.Count; j++)
					{
						this.AddObjective(listGroup2, objectives[i], j, objectives[i].subDesc[j], loading, -1, -1);
					}
				}
			}
		}
	}

	private global::UnityEngine.GameObject AddObjective(global::ListGroup listGroup, global::Objective objective, int subIndex, string desc, bool loading = false, int counter1 = -1, int counter2 = -1)
	{
		global::UnityEngine.GameObject gameObject = listGroup.AddToList();
		global::ObjectiveView objectiveView = gameObject.GetComponentsInChildren<global::ObjectiveView>(true)[0];
		objectiveView.mainObjective = objective;
		objectiveView.subIndex = subIndex;
		objectiveView.Set(objective, subIndex, desc, loading);
		objectiveView.UpdateObjective(loading, counter1, counter2);
		this.objectiveViews.Add(objectiveView);
		return gameObject;
	}

	private void Update()
	{
		if (this.objectiveViews.Count == 0)
		{
			return;
		}
		if (global::PandoraSingleton<global::SequenceManager>.Exists())
		{
			if (!this.sequencePlaying && global::PandoraSingleton<global::SequenceManager>.Instance.isPlaying)
			{
				this.sequencePlaying = true;
				for (int i = 0; i < this.objectiveViews.Count; i++)
				{
					global::DG.Tweening.DOTween.Pause(this.objectiveViews[i]);
				}
			}
			else if (this.sequencePlaying && !global::PandoraSingleton<global::SequenceManager>.Instance.isPlaying)
			{
				this.sequencePlaying = false;
				for (int j = 0; j < this.objectiveViews.Count; j++)
				{
					global::DG.Tweening.DOTween.Play(this.objectiveViews[j]);
				}
			}
		}
	}

	public const string RES_DUAL_COUNTER = "{0} / {1}";

	public global::UnityEngine.GameObject ObjectiveCatPrefab;

	public global::UnityEngine.GameObject ObjectivePrefab;

	public global::UnityEngine.GameObject SubObjectivePrefab;

	private global::ListGroup mainGroup;

	private global::System.Collections.Generic.List<global::ObjectiveView> objectiveViews = new global::System.Collections.Generic.List<global::ObjectiveView>();

	private bool sequencePlaying;
}
