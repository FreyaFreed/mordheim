using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIUnitCurrentActionController : global::UIUnitControllerChanged
{
	protected virtual void Awake()
	{
		global::PandoraSingleton<global::NoticeManager>.Instance.RegisterListener(global::Notices.CURRENT_UNIT_ACTION_CHANGED, new global::DelReceiveNotice(this.OnCurrentActionChanged));
		global::PandoraSingleton<global::NoticeManager>.Instance.RegisterListener(global::Notices.CURRENT_UNIT_TARGET_CHANGED, new global::DelReceiveNotice(this.OnCurrentTargetChanged));
		global::PandoraSingleton<global::NoticeManager>.Instance.RegisterListener(global::Notices.GAME_ACTION_CONFIRMATION_BOX, delegate
		{
			this.SetConfirmation(false);
		});
		global::PandoraSingleton<global::NoticeManager>.Instance.RegisterListener(global::Notices.GAME_ACTION_CANCEL, new global::DelReceiveNotice(this.ActionCanceled));
		global::PandoraSingleton<global::NoticeManager>.Instance.RegisterListener(global::Notices.GAME_MORE_INFO_UNIT_ACTION_TOGGLE, new global::DelReceiveNotice(this.UpdateMoreInfoVisibility));
	}

	private void OnCurrentTargetChanged()
	{
		global::UnitController target = global::PandoraSingleton<global::NoticeManager>.Instance.Parameters[0] as global::UnitController;
		this.OnCurrentTargetChanged(target);
	}

	private void OnCurrentTargetChanged(global::UnitController target)
	{
		if (this.currentAction != null)
		{
			this.SetWarningMessage();
			this.UpdateRollAndDamage();
		}
	}

	private void Start()
	{
		this.confirm.SetAction("action", string.Empty, 0, false, null, null);
		this.confirm.SetInteractable(false);
		this.cancel.SetInteractable(false);
		this.cancel.SetAction("cancel", "menu_cancel", 0, false, null, null);
		this.leftCycling.SetAction("cycling", string.Empty, 0, false, null, null);
		this.rightCycling.SetAction("cycling", string.Empty, 0, true, null, null);
	}

	private void OnCurrentActionChanged()
	{
		if (global::PandoraSingleton<global::NoticeManager>.Instance.Parameters != null)
		{
			global::UnitController unitController = null;
			if (global::PandoraSingleton<global::NoticeManager>.Instance.Parameters.Count >= 1)
			{
				unitController = (global::PandoraSingleton<global::NoticeManager>.Instance.Parameters[0] as global::UnitController);
			}
			if (unitController != null && unitController == base.CurrentUnitController && unitController.IsPlayed())
			{
				global::ActionStatus actionStatus;
				if (global::PandoraSingleton<global::NoticeManager>.Instance.Parameters.Count >= 2)
				{
					actionStatus = (global::PandoraSingleton<global::NoticeManager>.Instance.Parameters[1] as global::ActionStatus);
				}
				else
				{
					actionStatus = base.CurrentUnitController.CurrentAction;
				}
				global::System.Collections.Generic.List<global::ActionStatus> actionsStatus;
				if (global::PandoraSingleton<global::NoticeManager>.Instance.Parameters.Count >= 3)
				{
					actionsStatus = (global::PandoraSingleton<global::NoticeManager>.Instance.Parameters[2] as global::System.Collections.Generic.List<global::ActionStatus>);
				}
				else
				{
					actionsStatus = base.CurrentUnitController.availableActionStatus;
				}
				this.OnCurrentActionChanged(unitController, actionStatus, actionsStatus);
			}
		}
	}

	private void OnCurrentActionChanged(global::UnitController controller, global::ActionStatus actionStatus, global::System.Collections.Generic.List<global::ActionStatus> actionsStatus)
	{
		if (base.CurrentUnitController != null && base.CurrentUnitController == controller && base.CurrentUnitController.IsPlayed())
		{
			base.UpdateUnit = false;
			this.currentActionParameter = actionStatus;
			this.nextActionParameter = null;
			this.prevActionParameter = null;
			this.hasActionsParameter = false;
			this.isActionsCountParameter = false;
			this.isActionsCount1Parameter = false;
			this.hasActionsParameter = (actionsStatus != null);
			this.isActionsCountParameter = (actionsStatus != null && actionsStatus.Count > 0);
			this.isActionsCount1Parameter = (actionsStatus != null && actionsStatus.Count == 1);
			if (actionsStatus != null && this.isActionsCountParameter)
			{
				int num = actionsStatus.IndexOf(this.currentActionParameter);
				this.nextActionParameter = actionsStatus[(num + 1 >= actionsStatus.Count) ? 0 : (num + 1)];
				this.prevActionParameter = actionsStatus[(num - 1 < 0) ? (actionsStatus.Count - 1) : (num - 1)];
			}
			else
			{
				this.nextActionParameter = (this.prevActionParameter = null);
			}
			this.CurrentActionChanged();
		}
	}

	private void ActionCanceled()
	{
		if (base.CurrentUnitController != null)
		{
			this.actionName.text = base.CurrentUnitController.CurrentAction.LocalizedName;
			this.cancel.gameObject.SetActive(false);
		}
	}

	public void SetConfirmation(bool isCounter)
	{
		if (base.CurrentUnitController != null && base.CurrentUnitController.CurrentAction != null)
		{
			this.actionName.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("mission_confirm_action", new string[]
			{
				(!isCounter) ? base.CurrentUnitController.CurrentAction.LocalizedName : global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("reaction_melee_attack")
			});
			this.cancel.gameObject.SetActive(true);
		}
	}

	private void CurrentActionChanged()
	{
		if (base.CurrentUnitController != null)
		{
			bool flag = false;
			if (base.CurrentUnitController.IsPlayed() && base.TargetUnitController != null && !base.TargetUnitController.IsPlayed() && base.TargetUnitController.IsCurrentState(global::UnitController.State.COUNTER_CHOICE))
			{
				flag = true;
				this.currentAction = null;
			}
			else
			{
				this.currentAction = this.currentActionParameter;
			}
			if (this.currentAction != null)
			{
				this.action.overrideSprite = this.GetActionIcon();
				this.mastery.enabled = this.currentAction.IsMastery;
				this.UpdateRollAndDamage();
				this.SetWarningMessage();
				this.noAction.SetActive(false);
				this.confirm.gameObject.SetActive(true);
				this.cancel.gameObject.SetActive(this.currentAction.waitForConfirmation || base.CurrentUnitController.IsTargeting());
				if (this.currentAction.waitForConfirmation || base.CurrentUnitController.IsTargeting())
				{
					this.SetConfirmation(base.CurrentUnitController.IsCurrentState(global::UnitController.State.COUNTER_CHOICE));
				}
				else
				{
					this.actionName.text = this.currentAction.LocalizedName;
				}
				if (this.isActionsCountParameter)
				{
					this.navigation.SetActive(true);
					this.action.enabled = true;
					if (this.isActionsCount1Parameter || base.CurrentUnitController.IsCurrentState(global::UnitController.State.COUNTER_CHOICE))
					{
						this.navigation.SetActive(false);
						this.prevAction.enabled = false;
						this.prevMastery.enabled = false;
						this.nextAction.enabled = false;
						this.nextMastery.enabled = false;
					}
					else
					{
						this.navigation.SetActive(true);
						this.prevAction.enabled = true;
						this.nextAction.enabled = true;
						if (base.CurrentUnitController.IsCurrentState(global::UnitController.State.INTERACTIVE_TARGET))
						{
							this.prevAction.overrideSprite = base.CurrentUnitController.prevInteractiveTarget.action.GetIcon();
							this.prevMastery.enabled = false;
							this.nextAction.overrideSprite = base.CurrentUnitController.nextInteractiveTarget.action.GetIcon();
							this.nextMastery.enabled = false;
						}
						else
						{
							this.prevAction.overrideSprite = this.prevActionParameter.GetIcon();
							this.prevMastery.enabled = this.prevActionParameter.IsMastery;
							this.nextAction.overrideSprite = this.nextActionParameter.GetIcon();
							this.nextMastery.enabled = this.nextActionParameter.IsMastery;
						}
					}
				}
				else
				{
					if (base.CurrentUnitController.IsChoosingTarget())
					{
						this.navigation.SetActive(true);
					}
					else
					{
						this.navigation.SetActive(false);
					}
					this.prevAction.enabled = false;
					this.prevMastery.enabled = false;
					this.nextAction.enabled = false;
					this.nextMastery.enabled = false;
				}
				for (int i = 0; i < this.strategyPoints.Count; i++)
				{
					this.strategyPoints[i].SetActive(i < this.currentAction.StrategyPoints);
				}
				for (int j = 0; j < this.offensePoints.Count; j++)
				{
					this.offensePoints[j].SetActive(j < this.currentAction.OffensePoints);
				}
			}
			else
			{
				this.navigation.SetActive(false);
				this.confirm.gameObject.SetActive(false);
				this.cancel.gameObject.SetActive(false);
				this.damage.enabled = false;
				this.percent.enabled = false;
				if (flag)
				{
					this.noActionText.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("waiting_opponent");
				}
				else
				{
					this.noActionText.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("skill_name_none");
				}
				this.noAction.SetActive(true);
				this.action.overrideSprite = null;
				this.prevAction.enabled = false;
				this.prevMastery.enabled = false;
				this.nextAction.enabled = false;
				this.nextMastery.enabled = false;
				for (int k = 0; k < this.strategyPoints.Count; k++)
				{
					this.strategyPoints[k].SetActive(false);
				}
				for (int l = 0; l < this.offensePoints.Count; l++)
				{
					this.offensePoints[l].SetActive(false);
				}
				for (int m = 0; m < this.rollExtraInfo.Count; m++)
				{
					this.rollExtraInfo[m].gameObject.SetActive(false);
				}
				for (int n = 0; n < this.damageExtraInfo.Count; n++)
				{
					this.damageExtraInfo[n].gameObject.SetActive(false);
				}
			}
		}
	}

	private void SetWarningMessage()
	{
		global::UnitActionId actionId = this.currentAction.ActionId;
		if (actionId != global::UnitActionId.MELEE_ATTACK && actionId != global::UnitActionId.CHARGE)
		{
			if (actionId != global::UnitActionId.CLIMB && actionId != global::UnitActionId.JUMP && actionId != global::UnitActionId.LEAP)
			{
				if (actionId != global::UnitActionId.FLEE)
				{
					if (actionId != global::UnitActionId.SPELL)
					{
						global::PandoraSingleton<global::UIMissionManager>.Instance.rightSequenceMessage.HideWithTimer();
					}
					else if (base.CurrentUnitController.IsTargeting())
					{
						global::SpellTypeId spellTypeId = this.currentAction.skillData.SpellTypeId;
						if (spellTypeId != global::SpellTypeId.ARCANE)
						{
							if (spellTypeId == global::SpellTypeId.DIVINE)
							{
								global::PandoraSingleton<global::UIMissionManager>.Instance.rightSequenceMessage.WarningNoTimer("reaction_spell_divine", base.CurrentUnitController.unit.DivineWrathRoll, -1, -1, true);
							}
						}
						else
						{
							global::PandoraSingleton<global::UIMissionManager>.Instance.rightSequenceMessage.WarningNoTimer("reaction_spell_arcane", base.CurrentUnitController.unit.TzeentchsCurseRoll, -1, -1, true);
						}
					}
					else
					{
						global::PandoraSingleton<global::UIMissionManager>.Instance.rightSequenceMessage.HideWithTimer();
					}
				}
				else
				{
					global::PandoraSingleton<global::UIMissionManager>.Instance.rightSequenceMessage.WarningNoTimer("reaction_flee", -1, -1, -1, false);
				}
			}
			else if (base.CurrentUnitController.IsCurrentState(global::UnitController.State.INTERACTIVE_TARGET))
			{
				global::PandoraSingleton<global::UIMissionManager>.Instance.rightSequenceMessage.WarningNoTimer("reaction_fall", -1, this.currentAction.GetMinDamage(false), this.currentAction.GetMaxDamage(false), true);
			}
			else
			{
				global::PandoraSingleton<global::UIMissionManager>.Instance.rightSequenceMessage.HideWithTimer();
			}
		}
		else
		{
			global::UnitController target = this.currentAction.GetTarget();
			if (base.CurrentUnitController.IsTargeting() && target != null && target.CanCounterAttack() && !base.CurrentUnitController.IsCurrentState(global::UnitController.State.COUNTER_CHOICE))
			{
				global::PandoraSingleton<global::UIMissionManager>.Instance.rightSequenceMessage.WarningNoTimer("reaction_melee_attack", -1, -1, -1, true);
			}
			else
			{
				global::PandoraSingleton<global::UIMissionManager>.Instance.rightSequenceMessage.HideWithTimer();
			}
		}
	}

	private global::UnityEngine.Sprite GetActionIcon()
	{
		if (base.CurrentUnitController != null && this.currentAction.IsInteractive && base.CurrentUnitController.interactivePoint != null && (base.CurrentUnitController.interactivePoint.unitActionId == global::UnitActionId.SEARCH || base.CurrentUnitController.interactivePoint.unitActionId == global::UnitActionId.ACTIVATE))
		{
			return base.CurrentUnitController.interactivePoint.GetIconAction();
		}
		return this.currentAction.GetIcon();
	}

	protected override void OnUnitChanged()
	{
		if ((base.CurrentUnitController != null && base.CurrentUnitController.IsPlayed()) || !(base.TargetUnitController != null) || base.TargetUnitController.IsPlayed())
		{
		}
	}

	protected override void LateUpdate()
	{
		base.LateUpdate();
		if (base.IsVisible)
		{
			this.UpdateRollAndDamage();
		}
	}

	private void UpdateRollAndDamage()
	{
		if (this.currentAction != null && this.currentAction.Available)
		{
			int roll = this.currentAction.GetRoll(false);
			if (roll != -1)
			{
				this.percent.enabled = true;
				this.percent.text = roll.ToConstantPercString();
			}
			else
			{
				this.percent.enabled = false;
			}
			if (this.currentAction.ActionId != global::UnitActionId.CLIMB && this.currentAction.ActionId != global::UnitActionId.JUMP && this.currentAction.ActionId != global::UnitActionId.LEAP)
			{
				base.CurrentUnitController.RecalculateModifiers();
				if (!this.currentAction.HasDamage())
				{
					this.damage.enabled = false;
				}
				else
				{
					this.damage.enabled = true;
					this.damage.text = global::PandoraUtils.StringBuilder.Append(this.currentAction.GetMinDamage(false).ToConstantString()).Append('-').Append(this.currentAction.GetMaxDamage(false).ToConstantString()).ToString();
				}
			}
			else
			{
				this.damage.enabled = false;
			}
			this.UpdateMoreInfoVisibility();
			for (int i = 0; i < this.rollExtraInfo.Count; i++)
			{
				if (i < base.CurrentUnitController.CurrentRollModifiers.Count)
				{
					this.rollExtraInfo[i].gameObject.SetActive(true);
					this.rollExtraInfo[i].text.text = base.CurrentUnitController.CurrentRollModifiers[i].GetText(true);
				}
				else
				{
					this.rollExtraInfo[i].gameObject.SetActive(false);
				}
			}
			for (int j = 0; j < this.damageExtraInfo.Count; j++)
			{
				if (j < base.CurrentUnitController.CurrentDamageModifiers.Count)
				{
					this.damageExtraInfo[j].gameObject.SetActive(true);
					this.damageExtraInfo[j].text.text = base.CurrentUnitController.CurrentDamageModifiers[j].GetText(false);
				}
				else
				{
					this.damageExtraInfo[j].gameObject.SetActive(false);
				}
			}
		}
	}

	private void UpdateMoreInfoVisibility()
	{
		this.leftMoreInfoActionGo.gameObject.SetActive(global::PandoraSingleton<global::UIMissionManager>.Instance.ShowingMoreInfoUnitAction);
		this.rightMoreInfoActionGo.gameObject.SetActive(global::PandoraSingleton<global::UIMissionManager>.Instance.ShowingMoreInfoUnitAction);
	}

	public void WaitingOpponent()
	{
		this.currentAction = null;
		this.OnEnable();
		this.navigation.SetActive(false);
		this.confirm.gameObject.SetActive(false);
		this.cancel.gameObject.SetActive(false);
		this.damage.enabled = false;
		this.percent.enabled = false;
		this.noActionText.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("waiting_opponent");
		this.noAction.SetActive(true);
		this.action.overrideSprite = null;
		this.mastery.enabled = false;
		this.prevAction.enabled = false;
		this.prevMastery.enabled = false;
		this.nextAction.enabled = false;
		this.nextMastery.enabled = false;
		for (int i = 0; i < this.strategyPoints.Count; i++)
		{
			this.strategyPoints[i].SetActive(false);
		}
		for (int j = 0; j < this.offensePoints.Count; j++)
		{
			this.offensePoints[j].SetActive(false);
		}
		for (int k = 0; k < this.rollExtraInfo.Count; k++)
		{
			this.rollExtraInfo[k].gameObject.SetActive(false);
		}
		for (int l = 0; l < this.damageExtraInfo.Count; l++)
		{
			this.damageExtraInfo[l].gameObject.SetActive(false);
		}
	}

	public global::UnityEngine.UI.Text percent;

	public global::UnityEngine.UI.Text damage;

	public global::UnityEngine.UI.Image action;

	public global::UnityEngine.UI.Image mastery;

	public global::UnityEngine.UI.Image prevAction;

	public global::UnityEngine.UI.Image prevMastery;

	public global::UnityEngine.UI.Image nextAction;

	public global::UnityEngine.UI.Image nextMastery;

	public global::UnityEngine.UI.Text actionName;

	public global::UnityEngine.GameObject noAction;

	public global::UnityEngine.UI.Text noActionText;

	public global::System.Collections.Generic.List<global::UnityEngine.GameObject> offensePoints;

	public global::System.Collections.Generic.List<global::UnityEngine.GameObject> strategyPoints;

	private global::ActionStatus currentAction;

	public global::UnityEngine.GameObject navigation;

	public global::ImageGroup leftCycling;

	public global::ImageGroup rightCycling;

	public global::ButtonGroup confirm;

	public global::ButtonGroup cancel;

	public global::UnityEngine.GameObject leftMoreInfoActionGo;

	public global::UnityEngine.GameObject rightMoreInfoActionGo;

	public global::System.Collections.Generic.List<global::UIExtraActionInfo> rollExtraInfo;

	public global::System.Collections.Generic.List<global::UIExtraActionInfo> damageExtraInfo;

	private global::ActionStatus currentActionParameter;

	private global::ActionStatus prevActionParameter;

	private global::ActionStatus nextActionParameter;

	private bool hasActionsParameter;

	private bool isActionsCountParameter;

	private bool isActionsCount1Parameter;
}
