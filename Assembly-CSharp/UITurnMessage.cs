using System;
using System.Collections;
using System.Runtime.InteropServices;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UITurnMessage : global::CanvasGroupDisabler
{
	private void Awake()
	{
		global::PandoraSingleton<global::NoticeManager>.Instance.RegisterListener(global::Notices.MISSION_ROUND_START, new global::DelReceiveNotice(this.OnNewRound));
		global::PandoraSingleton<global::NoticeManager>.Instance.RegisterListener(global::Notices.MISSION_ROUT_TEST, new global::DelReceiveNotice(this.OnRoutTest));
		global::PandoraSingleton<global::NoticeManager>.Instance.RegisterListener(global::Notices.MISSION_PLAYER_CHANGE, new global::DelReceiveNotice(this.OnPlayerChange));
		global::PandoraSingleton<global::NoticeManager>.Instance.RegisterListener(global::Notices.MISSION_REINFORCEMENTS, new global::DelReceiveNotice(this.OnReinforcement));
		global::PandoraSingleton<global::NoticeManager>.Instance.RegisterListener(global::Notices.MISSION_DEAD_UNIT_FLEE, new global::DelReceiveNotice(this.OnDeadUnitLeave));
		global::PandoraSingleton<global::NoticeManager>.Instance.RegisterListener(global::Notices.MISSION_UNIT_SPAWN, new global::DelReceiveNotice(this.OnUnitSpawn));
		global::PandoraSingleton<global::NoticeManager>.Instance.RegisterListener(global::Notices.DEPLOY_UNIT, new global::DelReceiveNotice(this.OnDeployUnit));
		this.noneIcon = global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadResource<global::UnityEngine.Sprite>("unit/none", true);
		this.currentIndex = 0L;
		this.index = 0L;
	}

	private void Start()
	{
		this.ladderTemplate = global::PandoraSingleton<global::UIMissionManager>.Instance.ladder.template.GetComponent<global::UIMissionLadderGroup>();
	}

	private void OnRoutTest()
	{
		global::WarbandController warbandController = (global::WarbandController)global::PandoraSingleton<global::NoticeManager>.Instance.Parameters[0];
		bool flag = (bool)global::PandoraSingleton<global::NoticeManager>.Instance.Parameters[1];
		string msg = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById((!warbandController.IsPlayed()) ? "combat_enemy_rout" : "combat_self_rout") + "\n \n" + global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById((!flag) ? "com_failure" : "com_success");
		long coroutineIndex;
		this.index = (coroutineIndex = this.index) + 1L;
		base.StartCoroutine(this.Show(coroutineIndex, msg, (!warbandController.IsPlayed()) ? this.overlayEnemy : this.overlayPlayer, global::Warband.GetIcon(warbandController.WarData.Id), default(global::UnityEngine.Color)));
	}

	private void OnPlayerChange()
	{
		global::UnityEngine.Color iconColor = global::UnityEngine.Color.white;
		global::UnitController currentUnit = global::PandoraSingleton<global::MissionManager>.Instance.GetCurrentUnit();
		global::UnityEngine.Sprite iconSprite;
		string stringById;
		global::UnityEngine.Sprite overlaySprite;
		if (currentUnit.IsPlayed())
		{
			iconSprite = currentUnit.unit.GetIcon();
			stringById = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("combat_unit_turn", new string[]
			{
				currentUnit.unit.Name
			});
			overlaySprite = this.overlayPlayer;
			iconColor = this.ladderTemplate.allyColor;
		}
		else
		{
			if (currentUnit.unit.IsMonster)
			{
				iconColor = this.ladderTemplate.neutralColor;
				overlaySprite = this.overlayNeutral;
			}
			else
			{
				iconColor = this.ladderTemplate.enemyColor;
				overlaySprite = this.overlayEnemy;
			}
			if (currentUnit.IsImprintVisible())
			{
				stringById = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("combat_unit_turn", new string[]
				{
					currentUnit.unit.Name
				});
				iconSprite = currentUnit.unit.GetIcon();
			}
			else if (currentUnit.HasBeenSpotted)
			{
				stringById = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("combat_enemy_turn");
				iconSprite = currentUnit.unit.GetIcon();
			}
			else
			{
				stringById = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("combat_enemy_turn");
				iconSprite = this.noneIcon;
				iconColor = global::UnityEngine.Color.white;
			}
		}
		long coroutineIndex;
		this.index = (coroutineIndex = this.index) + 1L;
		base.StartCoroutine(this.Show(coroutineIndex, stringById, overlaySprite, iconSprite, iconColor));
	}

	private void OnDeployUnit()
	{
		global::UnitController currentUnit = global::PandoraSingleton<global::MissionManager>.Instance.GetCurrentUnit();
		global::UnityEngine.Sprite iconSprite;
		string stringById;
		global::UnityEngine.Sprite overlaySprite;
		global::UnityEngine.Color iconColor;
		if (currentUnit.IsPlayed())
		{
			iconSprite = currentUnit.unit.GetIcon();
			stringById = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("combat_unit_deploy", new string[]
			{
				currentUnit.unit.Name
			});
			overlaySprite = this.overlayPlayer;
			iconColor = this.ladderTemplate.allyColor;
		}
		else
		{
			iconSprite = this.noneIcon;
			stringById = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("combat_enemy_deploy");
			overlaySprite = this.overlayEnemy;
			iconColor = this.ladderTemplate.enemyColor;
		}
		this.currentTween.Complete();
		base.StopAllCoroutines();
		this.currentIndex = this.index;
		long coroutineIndex;
		this.index = (coroutineIndex = this.index) + 1L;
		base.StartCoroutine(this.Show(coroutineIndex, stringById, overlaySprite, iconSprite, iconColor));
	}

	private void OnNewRound()
	{
		global::PandoraSingleton<global::Pan>.Instance.Narrate("new_round");
		this.DisplayNewTurn(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("mission_round_no", new string[]
		{
			((int)global::PandoraSingleton<global::NoticeManager>.Instance.Parameters[0] + 1).ToConstantString()
		}));
	}

	private void OnReinforcement()
	{
		string text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("mission_round_no", new string[]
		{
			((int)global::PandoraSingleton<global::NoticeManager>.Instance.Parameters[0] + 1).ToConstantString()
		});
		text = text + "\n \n" + global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("mission_reinforcements");
		this.DisplayNewTurn(text);
	}

	private void OnDeadUnitLeave()
	{
		string text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("mission_round_no", new string[]
		{
			((int)global::PandoraSingleton<global::NoticeManager>.Instance.Parameters[0] + 1).ToConstantString()
		});
		text = text + "\n \n" + global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("mission_dead_unit_flee");
		this.DisplayNewTurn(text);
	}

	private void OnUnitSpawn()
	{
		this.DisplayNewTurn(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById((string)global::PandoraSingleton<global::NoticeManager>.Instance.Parameters[0], new string[]
		{
			(string)global::PandoraSingleton<global::NoticeManager>.Instance.Parameters[1]
		}));
	}

	private void DisplayNewTurn(string msg)
	{
		long coroutineIndex;
		this.index = (coroutineIndex = this.index) + 1L;
		base.StartCoroutine(this.Show(coroutineIndex, msg, null, null, default(global::UnityEngine.Color)));
	}

	private global::System.Collections.IEnumerator Show(long coroutineIndex, string msg, global::UnityEngine.Sprite overlaySprite = null, global::UnityEngine.Sprite iconSprite = null, [global::System.Runtime.InteropServices.Optional] global::UnityEngine.Color iconColor)
	{
		while (coroutineIndex != this.currentIndex)
		{
			yield return null;
		}
		this.OnEnable();
		this.overlay.enabled = (overlaySprite != null);
		this.overlay.overrideSprite = overlaySprite;
		this.icon.enabled = (iconSprite != null);
		this.icon.overrideSprite = iconSprite;
		this.icon.color = iconColor;
		this.text.text = msg;
		this.currentTween = base.CanvasGroup.DOFade(0f, 1f).SetDelay(2f).OnComplete(new global::DG.Tweening.TweenCallback(this.OnDisable));
		yield break;
	}

	public override void OnDisable()
	{
		bool interactable = base.CanvasGroup.interactable;
		base.OnDisable();
		if (interactable && this.index != 0L)
		{
			this.currentIndex += 1L;
		}
	}

	public global::UnityEngine.UI.Text text;

	public global::UnityEngine.UI.Image icon;

	public global::UnityEngine.UI.Image overlay;

	public global::UnityEngine.Sprite overlayEnemy;

	public global::UnityEngine.Sprite overlayPlayer;

	public global::UnityEngine.Sprite overlayNeutral;

	private global::DG.Tweening.Tweener currentTween;

	private global::UnityEngine.Sprite noneIcon;

	private global::UIMissionLadderGroup ladderTemplate;

	private long currentIndex;

	private long index;
}
