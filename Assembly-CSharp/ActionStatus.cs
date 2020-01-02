using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class ActionStatus
{
	public ActionStatus(global::SkillData data, global::UnitController ctrlr)
	{
		this.Init(data, ctrlr);
	}

	public ActionStatus(global::Item item, global::UnitController ctrlr)
	{
		this.LinkedItem = item;
		global::SkillData data = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::SkillData>((int)item.ConsumableData.SkillId);
		this.Init(data, ctrlr);
	}

	public global::UnitActionId ActionId
	{
		get
		{
			return this.skillData.UnitActionId;
		}
	}

	public global::SkillId SkillId
	{
		get
		{
			return this.skillData.Id;
		}
	}

	public global::Item LinkedItem { get; private set; }

	public global::ZoneAoeId ZoneAoeId
	{
		get
		{
			return this.skillData.ZoneAoeId;
		}
	}

	public bool Available { get; private set; }

	public global::ActionStatus.AvailableReason NotAvailableReason { get; private set; }

	public global::EnchantmentId BlockedByEnchantment { get; private set; }

	public global::System.Collections.Generic.List<global::UnitController> Targets { get; private set; }

	public global::System.Collections.Generic.List<global::Destructible> Destructibles { get; private set; }

	public string Name
	{
		get
		{
			string text = null;
			if (this.IsInteractive && this.unitCtrlr.interactivePoint != null)
			{
				text = this.unitCtrlr.interactivePoint.GetLocAction();
			}
			return (!string.IsNullOrEmpty(text)) ? text : this.skillData.Name;
		}
	}

	public int RangeMin
	{
		get
		{
			if (this.IsShootAction())
			{
				global::Item item = this.unitCtrlr.Equipments[(int)this.unitCtrlr.unit.ActiveWeaponSlot].Item;
				if (item.TargetingId != global::TargetingId.CONE && item.TargetingId != global::TargetingId.LINE)
				{
					return item.RangeMin;
				}
			}
			return this.skillData.RangeMin;
		}
	}

	public int RangeMax
	{
		get
		{
			if (this.IsShootAction())
			{
				return this.unitCtrlr.Equipments[(int)this.unitCtrlr.unit.ActiveWeaponSlot].Item.RangeMax;
			}
			return this.skillData.Range + ((this.skillData.Range == 0 || this.skillData.SkillTypeId != global::SkillTypeId.SPELL_ACTION) ? 0 : this.unitCtrlr.unit.RangeBonusSpell);
		}
	}

	public global::TargetingId TargetingId
	{
		get
		{
			return (!this.IsShootAction()) ? this.skillData.TargetingId : this.unitCtrlr.Equipments[(int)this.unitCtrlr.unit.ActiveWeaponSlot].Item.TargetingId;
		}
	}

	public bool TargetAlly
	{
		get
		{
			return (!this.IsShootAction()) ? this.skillData.TargetAlly : (this.unitCtrlr.Equipments[(int)this.unitCtrlr.unit.ActiveWeaponSlot].Item.TargetAlly || this.skillData.TargetAlly);
		}
	}

	public int Radius
	{
		get
		{
			return (!this.IsShootAction()) ? this.skillData.Radius : this.unitCtrlr.Equipments[(int)this.unitCtrlr.unit.ActiveWeaponSlot].Item.Radius;
		}
	}

	public bool IsMastery
	{
		get
		{
			return this.skillData.SkillQualityId == global::SkillQualityId.MASTER_QUALITY;
		}
	}

	public int StrategyPoints
	{
		get
		{
			return global::UnityEngine.Mathf.Max(this.unitCtrlr.unit.GetStratCostModifier(this.SkillId, this.ActionId, this.skillData.SpellTypeId) + this.skillData.StrategyPoints + this.extraStrategyPts, 0);
		}
	}

	public int OffensePoints
	{
		get
		{
			return global::UnityEngine.Mathf.Max(this.unitCtrlr.unit.GetOffCostModifier(this.SkillId, this.ActionId, this.skillData.SpellTypeId) + this.skillData.OffensePoints + this.extraOffensePts, 0);
		}
	}

	public string LocalizedDescription
	{
		get
		{
			return global::SkillHelper.GetLocalizedDescription(this.skillData.Id);
		}
	}

	public bool IsInteractive
	{
		get
		{
			global::UnitActionId actionId = this.ActionId;
			switch (actionId)
			{
			case global::UnitActionId.ACTIVATE:
			case global::UnitActionId.CLIMB:
			case global::UnitActionId.JUMP:
				break;
			default:
				if (actionId != global::UnitActionId.LEAP && actionId != global::UnitActionId.SEARCH)
				{
					return false;
				}
				break;
			}
			if (this.unitCtrlr.interactivePoint != null)
			{
				return true;
			}
			return false;
		}
	}

	public string LocalizedName
	{
		get
		{
			string text = null;
			if (this.IsInteractive && this.unitCtrlr.interactivePoint != null)
			{
				text = this.unitCtrlr.interactivePoint.GetLocAction();
			}
			return (!string.IsNullOrEmpty(text)) ? global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(text) : global::SkillHelper.GetLocalizedName(this.skillData);
		}
	}

	public string LocalizedNotAvailableReason
	{
		get
		{
			if (this.NotAvailableReason == global::ActionStatus.AvailableReason.NONE)
			{
				return string.Empty;
			}
			if (this.NotAvailableReason != global::ActionStatus.AvailableReason.BLOCKED_BY_ENCHANTMENT)
			{
				return global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("na_action_" + this.NotAvailableReason.ToLowerString());
			}
			if (global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::EnchantmentData>((int)this.BlockedByEnchantment).NoDisplay)
			{
				return global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("enchant_desc_block_tutorial_actions");
			}
			return global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("na_action_" + this.NotAvailableReason.ToLowerString(), new string[]
			{
				"#enchant_title_" + this.BlockedByEnchantment.ToLowerString()
			});
		}
	}

	private void Init(global::SkillData data, global::UnitController ctrlr)
	{
		this.skillData = data;
		this.unitCtrlr = ctrlr;
		this.Available = false;
		this.actionData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::UnitActionData>((int)this.ActionId);
		this.requiredItemsData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::SkillItemData>("fk_skill_id", ((int)this.SkillId).ToConstantString());
		global::System.Collections.Generic.List<global::SkillFxData> list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::SkillFxData>("fk_skill_id", ((int)this.SkillId).ToConstantString());
		this.fxData = ((list == null || list.Count <= 0) ? null : list[0]);
		this.skillActionData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::SkillPerformSkillData>("fk_skill_id", ((int)this.SkillId).ToConstantString());
		this.Targets = new global::System.Collections.Generic.List<global::UnitController>();
		this.Destructibles = new global::System.Collections.Generic.List<global::Destructible>();
	}

	public void SetOwner(global::UnitController ctrlr)
	{
		this.unitCtrlr = ctrlr;
	}

	private int CountAvailableEngagedUnits()
	{
		int num = 0;
		for (int i = 0; i < this.unitCtrlr.EngagedUnits.Count; i++)
		{
			if (this.unitCtrlr.EngagedUnits[i].unit.IsAvailable())
			{
				num++;
			}
		}
		return num;
	}

	public void UpdateAvailable()
	{
		this.Available = true;
		this.BlockedByEnchantment = global::EnchantmentId.NONE;
		this.Targets.Clear();
		this.Destructibles.Clear();
		global::UnitActionId actionId = this.ActionId;
		if (actionId != global::UnitActionId.DISENGAGE)
		{
			if (actionId == global::UnitActionId.INTERACTION)
			{
				return;
			}
		}
		else if (this.CountAvailableEngagedUnits() == 0)
		{
			this.extraOffensePts = 0;
			this.extraStrategyPts = 0;
			this.extraOffensePts = -this.OffensePoints;
			this.extraStrategyPts = -this.StrategyPoints;
		}
		else
		{
			this.extraOffensePts = 0;
			this.extraStrategyPts = 0;
		}
		global::EnchantmentId blockedByEnchantment;
		if (this.unitCtrlr.AICtrlr != null && this.skillData.AiProof)
		{
			this.Available = false;
		}
		else if (global::PandoraSingleton<global::MissionManager>.Instance.interruptingUnit != null && global::PandoraSingleton<global::MissionManager>.Instance.interruptingUnit != this.unitCtrlr)
		{
			this.NotAvailableReason = global::ActionStatus.AvailableReason.NONE;
			this.Available = false;
		}
		else if (this.unitCtrlr.IsInFriendlyZone)
		{
			this.NotAvailableReason = global::ActionStatus.AvailableReason.FRIENDLY_ZONE;
			this.Available = false;
		}
		else if (!this.unitCtrlr.unit.HasEnoughPoints(this.StrategyPoints, this.OffensePoints))
		{
			this.NotAvailableReason = global::ActionStatus.AvailableReason.NOT_ENOUGH_POINTS;
			this.Available = false;
		}
		else if (this.unitCtrlr.unit.IsSkillBlocked(this.skillData, out blockedByEnchantment))
		{
			this.NotAvailableReason = global::ActionStatus.AvailableReason.BLOCKED_BY_ENCHANTMENT;
			this.BlockedByEnchantment = blockedByEnchantment;
			this.Available = false;
		}
		else if (this.skillData.NotEngaged && this.unitCtrlr.Engaged)
		{
			this.NotAvailableReason = global::ActionStatus.AvailableReason.ENGAGED;
			this.Available = false;
		}
		else if (this.skillData.Engaged && ((this.ActionId != global::UnitActionId.MELEE_ATTACK && !this.unitCtrlr.Engaged) || (this.ActionId == global::UnitActionId.MELEE_ATTACK && !this.unitCtrlr.Engaged && this.unitCtrlr.triggeredDestructibles.Count == 0)))
		{
			this.NotAvailableReason = global::ActionStatus.AvailableReason.NOT_ENGAGED;
			this.Available = false;
		}
		else if (this.skillData.NeedCloseSet && !this.unitCtrlr.HasClose())
		{
			this.NotAvailableReason = global::ActionStatus.AvailableReason.MELEE_WEAPON_NEEDED;
			this.Available = false;
		}
		else if (this.skillData.NeedRangeSet && !this.unitCtrlr.HasRange())
		{
			this.NotAvailableReason = global::ActionStatus.AvailableReason.RANGE_WEAPON_NEEDED;
			this.Available = false;
		}
		else if (this.skillData.WeaponLoaded && this.unitCtrlr.GetCurrentShots() <= 0)
		{
			this.NotAvailableReason = global::ActionStatus.AvailableReason.WEAPON_NOT_RELOADED;
			this.Available = false;
		}
		else if (this.unitCtrlr.unit.CurrentWound < this.skillData.WoundsCostMax)
		{
			this.NotAvailableReason = global::ActionStatus.AvailableReason.WOUND_COST;
			this.Available = false;
		}
		else if (this.skillData.TrapTypeId != global::TrapTypeId.NONE)
		{
			global::System.Collections.Generic.List<global::MapImprint> mapImprints = global::PandoraSingleton<global::MissionManager>.Instance.MapImprints;
			int teamIdx = this.unitCtrlr.GetWarband().teamIdx;
			for (int i = 0; i < mapImprints.Count; i++)
			{
				if (mapImprints[i].Trap != null && mapImprints[i].Trap.TeamIdx == teamIdx && global::UnityEngine.Vector3.SqrMagnitude(this.unitCtrlr.transform.position - mapImprints[i].Trap.transform.position) < 1f)
				{
					this.NotAvailableReason = global::ActionStatus.AvailableReason.TRAP_TOO_CLOSE;
					this.Available = false;
					break;
				}
			}
		}
		else if (this.SetTargets() && this.Targets.Count == 0 && this.Destructibles.Count == 0 && this.TargetingId == global::TargetingId.SINGLE_TARGET)
		{
			this.NotAvailableReason = global::ActionStatus.AvailableReason.TARGET_NOT_IN_RANGE;
			this.Available = false;
		}
		else if (this.requiredItemsData.Count != 0)
		{
			bool flag = false;
			int num = 0;
			while (num < this.requiredItemsData.Count && !flag)
			{
				global::SkillItemData skillItemData = this.requiredItemsData[num];
				flag = this.unitCtrlr.unit.HasItemActive(skillItemData.ItemId);
				if (flag && skillItemData.MutationId != global::MutationId.NONE)
				{
					flag = this.unitCtrlr.unit.HasMutation(skillItemData.MutationId);
				}
				num++;
			}
			if (!flag)
			{
				this.NotAvailableReason = global::ActionStatus.AvailableReason.NEED_WEAPON_TYPE;
				this.Available = false;
			}
		}
		if (this.ActionId == global::UnitActionId.END_TURN && !this.Available && this.unitCtrlr.unit.CurrentStrategyPoints == 0)
		{
			this.Available = true;
		}
		if (!this.Available)
		{
			return;
		}
		actionId = this.ActionId;
		switch (actionId)
		{
		case global::UnitActionId.SEARCH:
			break;
		case global::UnitActionId.SWITCH_WEAPONSET:
			if (!this.unitCtrlr.CanSwitchWeapon())
			{
				this.NotAvailableReason = global::ActionStatus.AvailableReason.SWITCH_WEAPON;
				this.Available = false;
			}
			return;
		case global::UnitActionId.DISENGAGE:
			if (!this.unitCtrlr.CanDisengage())
			{
				this.NotAvailableReason = global::ActionStatus.AvailableReason.DISENGAGE_NO_ROOM;
				this.Available = false;
			}
			return;
		case global::UnitActionId.SHOOT:
		case global::UnitActionId.AIM:
		case global::UnitActionId.CHARGE:
			return;
		case global::UnitActionId.RELOAD:
			if (this.unitCtrlr.GetCurrentShots() >= this.unitCtrlr.GetMaxShots())
			{
				this.NotAvailableReason = global::ActionStatus.AvailableReason.WEAPON_ALREADY_RELOADED;
				this.Available = false;
			}
			return;
		default:
			switch (actionId)
			{
			case global::UnitActionId.ACTIVATE:
			case global::UnitActionId.CLIMB:
			case global::UnitActionId.JUMP:
				break;
			default:
				if (actionId != global::UnitActionId.LEAP)
				{
					if (actionId != global::UnitActionId.CONSUMABLE)
					{
						return;
					}
					this.Available &= (this.unitCtrlr.unit.Data.UnitSizeId != global::UnitSizeId.LARGE && !this.unitCtrlr.unit.BothArmsMutated());
					return;
				}
				break;
			}
			break;
		case global::UnitActionId.FLEE:
			if (this.CountAvailableEngagedUnits() <= 0 || !this.unitCtrlr.CanDisengage())
			{
				this.NotAvailableReason = global::ActionStatus.AvailableReason.FLEE_NO_ROOM;
				this.Available = false;
			}
			return;
		case global::UnitActionId.DELAY:
			if (global::PandoraSingleton<global::MissionManager>.Instance.CurrentLadderIdx >= global::PandoraSingleton<global::MissionManager>.Instance.GetLadderLastValidPosition())
			{
				this.NotAvailableReason = global::ActionStatus.AvailableReason.DELAY;
				this.Available = false;
			}
			return;
		}
		bool flag2 = false;
		for (int j = 0; j < this.unitCtrlr.interactivePoints.Count; j++)
		{
			if (this.unitCtrlr.interactivePoints[j].GetUnitActionIds(this.unitCtrlr).IndexOf(this.ActionId, global::UnitActionIdComparer.Instance) != -1)
			{
				flag2 = true;
				break;
			}
		}
		this.Available = (this.Available && flag2);
	}

	public void Select()
	{
		if (this.actionData.Confirmation)
		{
			if (!this.waitForConfirmation)
			{
				global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice(global::Notices.GAME_ACTION_CONFIRMATION_BOX);
				this.unitCtrlr.SetAnimSpeed(0f);
				this.unitCtrlr.SetFixed(true);
				this.waitForConfirmation = true;
				this.OnActionSelected();
			}
			else
			{
				global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice(global::Notices.GAME_ACTION_CONFIRM);
				this.Confirm();
			}
		}
		else
		{
			this.Confirm();
		}
	}

	public void Cancel()
	{
		global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<bool>(global::Notices.GAME_ACTION_CANCEL, true);
		this.OnActionConfirmedCancelled(false);
		this.waitForConfirmation = false;
	}

	private void OnActionSelected()
	{
		global::UnitActionId actionId = this.ActionId;
		if (actionId == global::UnitActionId.STANCE)
		{
			if (this.SkillId == global::SkillId.BASE_STANCE_AMBUSH || this.SkillId == global::SkillId.PROWL || this.SkillId == global::SkillId.PROWL_MSTR || this.SkillId == global::SkillId.ONSLAUGHT || this.SkillId == global::SkillId.ONSLAUGHT_MSTR)
			{
				float @float = global::Constant.GetFloat((this.unitCtrlr.unit.Data.UnitSizeId != global::UnitSizeId.LARGE) ? global::ConstantId.MELEE_RANGE_NORMAL : global::ConstantId.MELEE_RANGE_LARGE);
				this.unitCtrlr.chargeCircle.gameObject.SetActive(true);
				this.unitCtrlr.chargeCircle.Set((float)this.unitCtrlr.unit.AmbushMovement + @float, this.unitCtrlr.CapsuleRadius);
			}
		}
	}

	private void OnActionConfirmedCancelled(bool confirmed)
	{
		global::UnitActionId actionId = this.ActionId;
		if (actionId == global::UnitActionId.STANCE)
		{
			if (this.SkillId == global::SkillId.BASE_STANCE_AMBUSH || this.SkillId == global::SkillId.PROWL || this.SkillId == global::SkillId.PROWL_MSTR || this.SkillId == global::SkillId.ONSLAUGHT || this.SkillId == global::SkillId.ONSLAUGHT_MSTR)
			{
				this.unitCtrlr.chargeCircle.gameObject.SetActive(false);
			}
		}
	}

	public void RemovePoints()
	{
		this.unitCtrlr.unit.RemovePoints(this.StrategyPoints, this.OffensePoints);
	}

	private void Confirm()
	{
		this.OnActionConfirmedCancelled(true);
		if (!this.Available)
		{
			return;
		}
		global::PandoraDebug.LogInfo("Confirming Skill " + this.SkillId, "ACTION", null);
		this.waitForConfirmation = false;
		this.unitCtrlr.SetCurrentAction(this.SkillId);
		this.SetTargets();
		global::UnitActionId actionId = this.ActionId;
		switch (actionId)
		{
		case global::UnitActionId.ACTIVATE:
		case global::UnitActionId.CLIMB:
		case global::UnitActionId.JUMP:
		case global::UnitActionId.INTERACTION:
			goto IL_DA;
		default:
			switch (actionId)
			{
			case global::UnitActionId.SEARCH:
				goto IL_DA;
			default:
				if (actionId == global::UnitActionId.LEAP)
				{
					goto IL_DA;
				}
				if (actionId != global::UnitActionId.SKILL && actionId != global::UnitActionId.SPELL)
				{
					this.unitCtrlr.SendSkill(this.SkillId);
					return;
				}
				break;
			case global::UnitActionId.SHOOT:
			case global::UnitActionId.AIM:
			case global::UnitActionId.MELEE_ATTACK:
			case global::UnitActionId.CHARGE:
				break;
			}
			break;
		case global::UnitActionId.CONSUMABLE:
			break;
		}
		this.GoToTargetingState();
		return;
		IL_DA:
		this.unitCtrlr.StateMachine.ChangeState(23);
	}

	public void Activate()
	{
		global::PandoraDebug.LogInfo("Activating Skill " + this.SkillId, "ACTION", null);
		this.RemovePoints();
		if (this.skillData.WoundsCostMin != 0)
		{
			int damage = global::PandoraSingleton<global::MissionManager>.Instance.NetworkTyche.Rand(this.skillData.WoundsCostMin, this.skillData.WoundsCostMax);
			this.unitCtrlr.ComputeDirectWound(damage, true, this.unitCtrlr, false);
		}
		this.unitCtrlr.LastActivatedAction = this;
		for (int i = 0; i < this.unitCtrlr.defenders.Count; i++)
		{
			this.unitCtrlr.defenders[i].ResetAttackResult();
			this.unitCtrlr.defenders[i].unit.ResetEnchantsChanged();
		}
		this.unitCtrlr.TriggerEnchantments(global::EnchantmentTriggerId.NONE, global::SkillId.NONE, global::UnitActionId.NONE);
		this.unitCtrlr.TriggerEnchantments(global::EnchantmentTriggerId.ON_ACTION, this.SkillId, this.ActionId);
		global::PandoraDebug.LogDebug("Activate Action" + this.ActionId, "ACTION", this.unitCtrlr);
		if (this.unitCtrlr.defenders.Count == 0)
		{
			if (this.ActionId != global::UnitActionId.END_TURN)
			{
				global::PandoraSingleton<global::MissionManager>.Instance.CombatLogger.AddLog(this.unitCtrlr, global::CombatLogger.LogMessage.PERFORM_SKILL, new string[]
				{
					this.unitCtrlr.GetLogName(),
					this.LocalizedName
				});
			}
		}
		else
		{
			this.logBldr.Length = 0;
			for (int j = 0; j < this.unitCtrlr.defenders.Count; j++)
			{
				if (this.unitCtrlr.defenderCtrlr.IsImprintVisible())
				{
					this.logBldr.AppendFormat(this.unitCtrlr.defenders[j].GetLogName(), new object[0]);
					if (j < this.unitCtrlr.defenders.Count - 1)
					{
						this.logBldr.Append(",");
					}
				}
			}
			string text = this.logBldr.ToString();
			if (string.IsNullOrEmpty(text))
			{
				text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("com_nobody");
			}
			global::PandoraSingleton<global::MissionManager>.Instance.CombatLogger.AddLog(this.unitCtrlr, global::CombatLogger.LogMessage.PERFORM_SKILL_TARGETS, new string[]
			{
				this.unitCtrlr.GetLogName(),
				this.LocalizedName,
				text
			});
		}
		global::UnitActionId actionId = this.ActionId;
		switch (actionId)
		{
		case global::UnitActionId.LEAP:
			goto IL_44C;
		default:
			switch (actionId)
			{
			case global::UnitActionId.ACTIVATE:
				this.unitCtrlr.StateMachine.ChangeState(16);
				return;
			default:
				if (actionId != global::UnitActionId.SKILL)
				{
					if (actionId != global::UnitActionId.SPELL)
					{
						return;
					}
					this.unitCtrlr.StateMachine.ChangeState(28);
					return;
				}
				break;
			case global::UnitActionId.STANCE:
				this.unitCtrlr.StateMachine.ChangeState(35);
				return;
			case global::UnitActionId.CLIMB:
			case global::UnitActionId.JUMP:
				goto IL_44C;
			case global::UnitActionId.AMBUSH:
				goto IL_382;
			case global::UnitActionId.OVERWATCH:
				goto IL_354;
			case global::UnitActionId.CONSUMABLE:
				break;
			case global::UnitActionId.FLY:
				this.unitCtrlr.StateMachine.ChangeState(49);
				return;
			}
			this.unitCtrlr.StateMachine.ChangeState(30);
			return;
		case global::UnitActionId.PERCEPTION:
			this.unitCtrlr.StateMachine.ChangeState(13);
			return;
		case global::UnitActionId.SEARCH:
			this.unitCtrlr.StateMachine.ChangeState(14);
			return;
		case global::UnitActionId.SWITCH_WEAPONSET:
			this.unitCtrlr.StateMachine.ChangeState(21);
			return;
		case global::UnitActionId.DISENGAGE:
			this.unitCtrlr.StateMachine.ChangeState(20);
			return;
		case global::UnitActionId.SHOOT:
		case global::UnitActionId.AIM:
			break;
		case global::UnitActionId.RELOAD:
			this.unitCtrlr.StateMachine.ChangeState(19);
			return;
		case global::UnitActionId.MELEE_ATTACK:
			this.unitCtrlr.StateMachine.ChangeState(32);
			return;
		case global::UnitActionId.CHARGE:
			goto IL_382;
		case global::UnitActionId.FLEE:
			this.unitCtrlr.StateMachine.ChangeState(40);
			return;
		case global::UnitActionId.END_TURN:
			this.unitCtrlr.StateMachine.ChangeState(39);
			return;
		case global::UnitActionId.DELAY:
			this.unitCtrlr.StateMachine.ChangeState(22);
			return;
		}
		IL_354:
		this.unitCtrlr.StateMachine.ChangeState(31);
		return;
		IL_382:
		this.unitCtrlr.LaunchMelee(global::UnitController.State.CHARGE);
		return;
		IL_44C:
		this.unitCtrlr.StateMachine.ChangeState(46);
	}

	public bool SetTargets()
	{
		this.Targets.Clear();
		this.Destructibles.Clear();
		if (this.skillData == null)
		{
			global::PandoraDebug.LogWarning("Getting targets for Skill " + this.SkillId + " but there is no skillData available", "ACTION", null);
			return false;
		}
		bool flag = this.skillActionData != null && this.skillActionData.Count > 0;
		int idx = this.unitCtrlr.GetWarband().idx;
		if (this.ActionId == global::UnitActionId.CHARGE)
		{
			this.unitCtrlr.SetChargeTargets(this.unitCtrlr == global::PandoraSingleton<global::MissionManager>.Instance.GetCurrentUnit() && this.unitCtrlr.IsPlayed());
			this.Targets.AddRange(this.unitCtrlr.chargeTargets);
		}
		else
		{
			global::System.Collections.Generic.List<global::UnitController> list = (!this.skillData.Engaged) ? global::PandoraSingleton<global::MissionManager>.Instance.GetAllUnits() : this.unitCtrlr.EngagedUnits;
			for (int i = 0; i < list.Count; i++)
			{
				global::UnitController unitController = list[i];
				if (((flag && unitController.unit.IsAvailable()) || (!flag && unitController.unit.Status != global::UnitStateId.OUT_OF_ACTION)) && (this.skillData.EnchantmentIdRequiredTarget == global::EnchantmentId.NONE || unitController.unit.HasEnchantment(this.skillData.EnchantmentIdRequiredTarget)))
				{
					if (this.skillData.TargetSelf && unitController == this.unitCtrlr)
					{
						this.Targets.Add(unitController);
					}
					if (this.TargetAlly && unitController != this.unitCtrlr && unitController.GetWarband().idx == idx)
					{
						this.Targets.Add(unitController);
					}
					if (this.skillData.TargetEnemy && unitController != this.unitCtrlr && unitController.GetWarband().idx != idx)
					{
						this.Targets.Add(unitController);
					}
				}
			}
			if (!this.skillData.Engaged)
			{
				if (this.TargetingId != global::TargetingId.AREA && this.TargetingId != global::TargetingId.AREA_GROUND)
				{
					bool flag2 = this.IsShootAction();
					float requiredPerc = (!flag2) ? global::Constant.GetFloat(global::ConstantId.RANGE_SPELL_REQUIRED_PERC) : global::Constant.GetFloat(global::ConstantId.RANGE_SHOOT_REQUIRED_PERC);
					for (int j = this.Targets.Count - 1; j >= 0; j--)
					{
						if (this.Targets[j] != this.unitCtrlr && !this.unitCtrlr.IsInRange(this.Targets[j], (float)this.RangeMin, (float)(this.RangeMax + this.Radius), requiredPerc, flag2, flag2, this.skillData.BoneIdTarget))
						{
							this.Targets.RemoveAt(j);
						}
					}
				}
				else
				{
					for (int k = this.Targets.Count - 1; k >= 0; k--)
					{
						if (this.Targets[k] != this.unitCtrlr && global::UnityEngine.Vector3.Magnitude(this.unitCtrlr.transform.position - this.Targets[k].transform.position) > (float)(this.RangeMax + this.Radius) + this.Targets[k].CapsuleRadius)
						{
							this.Targets.RemoveAt(k);
						}
					}
				}
			}
		}
		global::UnitActionId actionId = this.ActionId;
		switch (actionId)
		{
		case global::UnitActionId.SHOOT:
		case global::UnitActionId.AIM:
			break;
		default:
			if (actionId != global::UnitActionId.SPELL)
			{
				goto IL_4E0;
			}
			break;
		case global::UnitActionId.MELEE_ATTACK:
			this.Destructibles.AddRange(this.unitCtrlr.triggeredDestructibles);
			goto IL_4E0;
		case global::UnitActionId.CHARGE:
			goto IL_4E0;
		}
		if ((this.ActionId == global::UnitActionId.SPELL && this.skillData.WoundMax > 0 && this.skillData.EnchantmentIdRequiredTarget == global::EnchantmentId.NONE) || this.ActionId != global::UnitActionId.SPELL)
		{
			global::UnityEngine.Vector3 src = this.unitCtrlr.transform.position + global::UnityEngine.Vector3.up * 1.4f;
			for (int l = 0; l < global::PandoraSingleton<global::MissionManager>.Instance.triggerPoints.Count; l++)
			{
				if (global::PandoraSingleton<global::MissionManager>.Instance.triggerPoints[l] is global::Destructible)
				{
					global::Destructible destructible = (global::Destructible)global::PandoraSingleton<global::MissionManager>.Instance.triggerPoints[l];
					if (destructible.IsInRange(src, (float)(this.RangeMax + this.Radius)))
					{
						this.Destructibles.Add(destructible);
					}
				}
			}
		}
		IL_4E0:
		if (this.Targets.Count > 0 && flag)
		{
			global::SkillId skillIdPerformed = this.skillActionData[0].SkillIdPerformed;
			int m = this.Targets.Count - 1;
			while (m >= 0)
			{
				global::ActionStatus action = this.Targets[m].GetAction(skillIdPerformed);
				if (action == null)
				{
					goto IL_556;
				}
				action.UpdateAvailable();
				if (!action.Available)
				{
					goto IL_556;
				}
				IL_563:
				m--;
				continue;
				IL_556:
				this.Targets.RemoveAt(m);
				goto IL_563;
			}
		}
		return true;
	}

	public int GetRoll(bool updateModifiers = false)
	{
		bool flag = this.unitCtrlr.unit.GetUnitTypeId() == global::UnitTypeId.IMPRESSIVE;
		int num = -1;
		global::UnitActionId actionId = this.ActionId;
		switch (actionId)
		{
		case global::UnitActionId.PERCEPTION:
			num = this.unitCtrlr.unit.PerceptionRoll;
			if (updateModifiers)
			{
				this.unitCtrlr.CurrentRollModifiers.AddRange(this.unitCtrlr.unit.attributeModifiers.GetOrNull(global::AttributeId.PERCEPTION_ROLL), null, false, false, false);
			}
			goto IL_4C1;
		default:
			switch (actionId)
			{
			case global::UnitActionId.STANCE:
			{
				global::SkillId skillId = this.SkillId;
				if (skillId != global::SkillId.BASE_STANCE_PARRY)
				{
					if (skillId == global::SkillId.BASE_STANCE_DODGE)
					{
						num = this.unitCtrlr.unit.DodgeRoll;
						if (updateModifiers)
						{
							this.unitCtrlr.CurrentRollModifiers.AddRange(this.unitCtrlr.unit.attributeModifiers.GetOrNull(global::AttributeId.DODGE_ROLL), null, false, false, false);
						}
					}
				}
				else
				{
					num = this.unitCtrlr.unit.ParryingRoll;
					if (updateModifiers)
					{
						this.unitCtrlr.CurrentRollModifiers.AddRange(this.unitCtrlr.unit.attributeModifiers.GetOrNull(global::AttributeId.PARRYING_ROLL), null, false, false, false);
					}
				}
				goto IL_4C1;
			}
			case global::UnitActionId.CLIMB:
			{
				if (this.unitCtrlr.activeActionDest == null)
				{
					return -1;
				}
				if (flag)
				{
					return 100;
				}
				int fallHeight = this.unitCtrlr.GetFallHeight(this.unitCtrlr.activeActionDest.actionId);
				switch (fallHeight)
				{
				case 3:
					num = this.unitCtrlr.unit.ClimbRoll3;
					if (updateModifiers)
					{
						this.unitCtrlr.CurrentRollModifiers.AddRange(this.unitCtrlr.unit.attributeModifiers.GetOrNull(global::AttributeId.CLIMB_ROLL_3), null, false, false, false);
					}
					break;
				default:
					if (fallHeight == 9)
					{
						num = this.unitCtrlr.unit.ClimbRoll9;
						if (updateModifiers)
						{
							this.unitCtrlr.CurrentRollModifiers.AddRange(this.unitCtrlr.unit.attributeModifiers.GetOrNull(global::AttributeId.CLIMB_ROLL_9), null, false, false, false);
						}
					}
					break;
				case 6:
					num = this.unitCtrlr.unit.ClimbRoll6;
					if (updateModifiers)
					{
						this.unitCtrlr.CurrentRollModifiers.AddRange(this.unitCtrlr.unit.attributeModifiers.GetOrNull(global::AttributeId.CLIMB_ROLL_6), null, false, false, false);
					}
					break;
				}
				goto IL_4C1;
			}
			case global::UnitActionId.JUMP:
			{
				if (this.unitCtrlr.activeActionDest == null)
				{
					return -1;
				}
				if (flag)
				{
					return 100;
				}
				int fallHeight = this.unitCtrlr.GetFallHeight(this.unitCtrlr.activeActionDest.actionId);
				switch (fallHeight)
				{
				case 3:
					num = this.unitCtrlr.unit.JumpDownRoll3;
					if (updateModifiers)
					{
						this.unitCtrlr.CurrentRollModifiers.AddRange(this.unitCtrlr.unit.attributeModifiers.GetOrNull(global::AttributeId.JUMP_DOWN_ROLL_3), null, false, false, false);
					}
					break;
				default:
					if (fallHeight == 9)
					{
						num = this.unitCtrlr.unit.JumpDownRoll9;
						if (updateModifiers)
						{
							this.unitCtrlr.CurrentRollModifiers.AddRange(this.unitCtrlr.unit.attributeModifiers.GetOrNull(global::AttributeId.JUMP_DOWN_ROLL_9), null, false, false, false);
						}
					}
					break;
				case 6:
					num = this.unitCtrlr.unit.JumpDownRoll6;
					if (updateModifiers)
					{
						this.unitCtrlr.CurrentRollModifiers.AddRange(this.unitCtrlr.unit.attributeModifiers.GetOrNull(global::AttributeId.JUMP_DOWN_ROLL_6), null, false, false, false);
					}
					break;
				}
				goto IL_4C1;
			}
			case global::UnitActionId.AMBUSH:
				goto IL_AD;
			case global::UnitActionId.OVERWATCH:
				break;
			default:
				if (actionId != global::UnitActionId.LEAP)
				{
					if (actionId != global::UnitActionId.SPELL)
					{
						goto IL_4C1;
					}
					num = this.unitCtrlr.GetSpellCastingRoll(this.skillData.SpellTypeId, updateModifiers);
					goto IL_4C1;
				}
				else
				{
					if (flag)
					{
						return 100;
					}
					num = this.unitCtrlr.unit.LeapRoll;
					if (updateModifiers)
					{
						this.unitCtrlr.CurrentRollModifiers.AddRange(this.unitCtrlr.unit.attributeModifiers.GetOrNull(global::AttributeId.LEAP_ROLL), null, false, false, false);
					}
					goto IL_4C1;
				}
				break;
			}
			break;
		case global::UnitActionId.SHOOT:
		case global::UnitActionId.AIM:
			break;
		case global::UnitActionId.MELEE_ATTACK:
		case global::UnitActionId.CHARGE:
			goto IL_AD;
		}
		num = this.unitCtrlr.GetRangeHitRoll(updateModifiers);
		if (num == 100 && this.unitCtrlr.destructibleTarget != null)
		{
			return num;
		}
		goto IL_4C1;
		IL_AD:
		num = this.unitCtrlr.GetMeleeHitRoll(updateModifiers);
		if (num == 100 && this.unitCtrlr.destructibleTarget != null)
		{
			return num;
		}
		IL_4C1:
		if (num != -1)
		{
			num = global::UnityEngine.Mathf.Clamp(num, 0, global::Constant.GetInt(global::ConstantId.MAX_ROLL));
		}
		return num;
	}

	public global::UnitController GetTarget()
	{
		global::UnitActionId actionId = this.ActionId;
		switch (actionId)
		{
		case global::UnitActionId.SHOOT:
		case global::UnitActionId.AIM:
			if (this.unitCtrlr.defenderCtrlr != null)
			{
				return this.unitCtrlr.defenderCtrlr;
			}
			if (this.Targets.Count > 0)
			{
				return this.Targets[0];
			}
			return null;
		default:
			if (actionId == global::UnitActionId.AMBUSH || actionId == global::UnitActionId.OVERWATCH)
			{
				return this.unitCtrlr.defenderCtrlr;
			}
			if (actionId != global::UnitActionId.SPELL)
			{
				return null;
			}
			return this.unitCtrlr.defenderCtrlr;
		case global::UnitActionId.MELEE_ATTACK:
			if (this.unitCtrlr.defenderCtrlr != null)
			{
				return this.unitCtrlr.defenderCtrlr;
			}
			if (this.unitCtrlr.EngagedUnits.Count > 0)
			{
				return this.unitCtrlr.EngagedUnits[0];
			}
			return null;
		case global::UnitActionId.CHARGE:
			if (this.unitCtrlr.defenderCtrlr != null)
			{
				return this.unitCtrlr.defenderCtrlr;
			}
			if (this.unitCtrlr.chargeTargets.Count > 0)
			{
				return this.unitCtrlr.chargeTargets[0];
			}
			return null;
		}
	}

	public bool HasDamage()
	{
		global::UnitActionId actionId = this.ActionId;
		switch (actionId)
		{
		case global::UnitActionId.SHOOT:
		case global::UnitActionId.AIM:
		case global::UnitActionId.MELEE_ATTACK:
		case global::UnitActionId.CHARGE:
			break;
		default:
			switch (actionId)
			{
			case global::UnitActionId.CLIMB:
			case global::UnitActionId.JUMP:
				break;
			case global::UnitActionId.AMBUSH:
			case global::UnitActionId.OVERWATCH:
				return true;
			default:
				if (actionId != global::UnitActionId.LEAP)
				{
					if (actionId != global::UnitActionId.SPELL)
					{
						return this.skillData.WoundMax > 0;
					}
					return this.skillData.WoundMax > 0;
				}
				break;
			}
			return true;
		}
		return true;
	}

	public int GetMinDamage(bool updateModifiers = false)
	{
		global::UnitController target = this.GetTarget();
		global::Unit target2 = (!(target != null)) ? null : target.unit;
		int result = this.skillData.WoundMin;
		global::UnitActionId actionId = this.ActionId;
		switch (actionId)
		{
		case global::UnitActionId.SHOOT:
		case global::UnitActionId.AIM:
		case global::UnitActionId.MELEE_ATTACK:
		case global::UnitActionId.CHARGE:
			break;
		default:
			switch (actionId)
			{
			case global::UnitActionId.CLIMB:
			case global::UnitActionId.JUMP:
				break;
			case global::UnitActionId.AMBUSH:
			case global::UnitActionId.OVERWATCH:
				goto IL_7D;
			default:
				if (actionId != global::UnitActionId.LEAP)
				{
					if (actionId != global::UnitActionId.SPELL)
					{
						return result;
					}
					if (this.skillData.WoundMin > 0)
					{
						result = this.unitCtrlr.unit.ApplySpellDamageModifier(this.SkillId, this.skillData.WoundMin, target2, this.skillData.SpellTypeId, this.skillData.BypassArmor);
						if (updateModifiers)
						{
							this.unitCtrlr.CurrentDamageModifiers.Add(new global::AttributeMod(global::AttributeMod.Type.BASE, global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("attribute_base_spell_damage"), this.skillData.WoundMin, this.skillData.WoundMax), null, false, false, false);
							this.unitCtrlr.CurrentDamageModifiers.AddRange(this.unitCtrlr.unit.GetSpellDamageModifier(this.SkillId, target2, this.skillData.SpellTypeId, this.skillData.BypassArmor));
						}
					}
					return result;
				}
				break;
			}
			if (this.unitCtrlr.activeActionDest != null)
			{
				return this.unitCtrlr.GetFallHeight(this.unitCtrlr.activeActionDest.actionId);
			}
			return result;
		}
		IL_7D:
		result = this.unitCtrlr.unit.GetWeaponDamageMin(target2, false, this.skillData.BypassArmor, this.unitCtrlr.IsCharging);
		if (updateModifiers)
		{
			this.unitCtrlr.CurrentDamageModifiers.AddRange(this.unitCtrlr.unit.GetWeaponDamageModifier(target2, this.skillData.BypassArmor, this.unitCtrlr.IsCharging));
		}
		return result;
	}

	public int GetMaxDamage(bool critical = false)
	{
		global::UnitController target = this.GetTarget();
		global::Unit target2 = (!(target != null)) ? null : target.unit;
		int result = this.skillData.WoundMax;
		global::UnitActionId actionId = this.ActionId;
		switch (actionId)
		{
		case global::UnitActionId.SHOOT:
		case global::UnitActionId.AIM:
		case global::UnitActionId.MELEE_ATTACK:
		case global::UnitActionId.CHARGE:
			break;
		default:
			switch (actionId)
			{
			case global::UnitActionId.CLIMB:
			case global::UnitActionId.JUMP:
				break;
			case global::UnitActionId.AMBUSH:
			case global::UnitActionId.OVERWATCH:
				goto IL_7D;
			default:
				if (actionId != global::UnitActionId.LEAP)
				{
					if (actionId != global::UnitActionId.SPELL)
					{
						return result;
					}
					if (this.skillData.WoundMax > 0)
					{
						result = this.unitCtrlr.unit.ApplySpellDamageModifier(this.SkillId, this.skillData.WoundMax, target2, this.skillData.SpellTypeId, this.skillData.BypassArmor);
					}
					return result;
				}
				break;
			}
			if (this.unitCtrlr.activeActionDest != null)
			{
				result = this.unitCtrlr.GetFallHeight(this.unitCtrlr.activeActionDest.actionId) + 10;
			}
			return result;
		}
		IL_7D:
		return this.unitCtrlr.unit.GetWeaponDamageMax(target2, critical, this.skillData.BypassArmor, this.unitCtrlr.IsCharging);
	}

	public global::UnityEngine.Sprite GetIcon()
	{
		global::UnityEngine.Sprite sprite = null;
		if (this.LinkedItem != null)
		{
			sprite = this.LinkedItem.GetIcon();
		}
		if (sprite == null)
		{
			sprite = global::ActionStatus.GetIcon(this.skillData.Name);
		}
		if (sprite == null)
		{
			sprite = global::ActionStatus.GetIcon(this.ActionId.ToLowerString());
		}
		return sprite;
	}

	public static global::UnityEngine.Sprite GetIcon(string name)
	{
		global::UnityEngine.Sprite sprite = global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadResource<global::UnityEngine.Sprite>("action/" + name.ToLowerString(), true);
		if (sprite == null)
		{
			sprite = global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadResource<global::UnityEngine.Sprite>("action/" + name.ToLowerInvariant().Replace("base_", string.Empty).Replace("_mstr", string.Empty), true);
		}
		if (sprite == null)
		{
		}
		return sprite;
	}

	private void GoToTargetingState()
	{
		switch (this.TargetingId)
		{
		case global::TargetingId.SINGLE_TARGET:
			this.unitCtrlr.StateMachine.ChangeState(24);
			break;
		case global::TargetingId.LINE:
			this.unitCtrlr.StateMachine.ChangeState(27);
			break;
		case global::TargetingId.CONE:
			this.unitCtrlr.StateMachine.ChangeState(26);
			break;
		case global::TargetingId.AREA:
		case global::TargetingId.AREA_GROUND:
			this.unitCtrlr.StateMachine.ChangeState(25);
			break;
		case global::TargetingId.ARC:
			this.unitCtrlr.StateMachine.ChangeState(50);
			break;
		}
	}

	public bool IsShootAction()
	{
		return this.skillData.UnitActionId == global::UnitActionId.SHOOT || this.skillData.UnitActionId == global::UnitActionId.AIM || this.skillData.UnitActionId == global::UnitActionId.OVERWATCH;
	}

	public global::UnitActionData actionData;

	public global::SkillData skillData;

	public global::SkillFxData fxData;

	private global::System.Collections.Generic.List<global::SkillPerformSkillData> skillActionData;

	private global::UnitController unitCtrlr;

	public bool waitForConfirmation;

	private int extraOffensePts;

	private int extraStrategyPts;

	private global::System.Text.StringBuilder logBldr = new global::System.Text.StringBuilder();

	private global::System.Collections.Generic.List<global::SkillItemData> requiredItemsData;

	public enum AvailableReason
	{
		NONE,
		FRIENDLY_ZONE,
		NOT_ENOUGH_POINTS,
		NOT_ENGAGED,
		ENGAGED,
		MELEE_WEAPON_NEEDED,
		RANGE_WEAPON_NEEDED,
		WEAPON_NOT_RELOADED,
		WOUND_COST,
		WEAPON_ALREADY_RELOADED,
		INTERACTION_NOT_IN_RANGE,
		TARGET_NOT_IN_RANGE,
		NEED_WEAPON_TYPE,
		WEAPON_LOCKED,
		FLEE_NO_ROOM,
		DISENGAGE_NO_ROOM,
		DELAY,
		SWITCH_WEAPON,
		BLOCKED_BY_ENCHANTMENT,
		TRAP_TOO_CLOSE
	}
}
