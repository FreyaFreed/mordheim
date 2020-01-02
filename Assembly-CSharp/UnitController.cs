using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Prometheus;
using UnityEngine;
using UnityEngine.Events;

[global::UnityEngine.RequireComponent(typeof(global::UnityEngine.Animator))]
[global::UnityEngine.RequireComponent(typeof(global::UnityEngine.CapsuleCollider))]
[global::UnityEngine.RequireComponent(typeof(global::UnityEngine.Rigidbody))]
public class UnitController : global::UnitMenuController, global::IMyrtilus
{
	public global::CheapStateMachine StateMachine { get; private set; }

	public bool Initialized { get; private set; }

	public bool IsFixed { get; private set; }

	public float CapsuleHeight { get; private set; }

	public float CapsuleRadius { get; private set; }

	public global::ActionStatus LastActivatedAction { get; set; }

	public global::ActionStatus CurrentAction
	{
		get
		{
			return this.currentAction;
		}
		private set
		{
			this.currentAction = value;
			this.unit.SetActiveSkill((this.CurrentAction == null) ? null : this.CurrentAction.skillData);
		}
	}

	public bool IsInFriendlyZone
	{
		get
		{
			return this.friendlyEntered.Count > 0;
		}
	}

	public global::AIController AICtrlr { get; private set; }

	public global::UnitController defenderCtrlr
	{
		get
		{
			return (this.defenders == null || this.defenders.Count <= 0) ? null : this.defenders[0];
		}
		set
		{
			this.defenders.Clear();
			if (value != null)
			{
				this.defenders.Add(value);
			}
		}
	}

	public global::Destructible destructibleTarget
	{
		get
		{
			return (this.destructTargets.Count <= 0) ? null : this.destructTargets[0];
		}
		set
		{
			this.destructTargets.Clear();
			if (value != null)
			{
				this.destructTargets.Add(value);
			}
		}
	}

	public bool Sheated { get; private set; }

	public bool IsCharging
	{
		get
		{
			return this.CurrentAction != null && (this.CurrentAction.ActionId == global::UnitActionId.CHARGE || this.CurrentAction.ActionId == global::UnitActionId.AMBUSH);
		}
	}

	public global::System.Collections.Generic.List<global::UnitController> EngagedUnits { get; private set; }

	public bool Engaged
	{
		get
		{
			return this.EngagedUnits.Count > 0;
		}
	}

	public global::AttributeModList CurrentRollModifiers { get; private set; }

	public global::AttributeModList CurrentDamageModifiers { get; private set; }

	public global::Beacon CurrentBeacon { get; private set; }

	public bool Fleeing { get; set; }

	public bool TurnStarted { get; set; }

	public bool Resurected { get; private set; }

	public global::UnityEngine.Vector3 FleeTarget { get; private set; }

	public global::MapImprint Imprint { get; private set; }

	public bool HasBeenSpotted
	{
		get
		{
			if (!this.hasBeenSpotted)
			{
				this.hasBeenSpotted = this.IsImprintVisible();
			}
			return this.hasBeenSpotted;
		}
		set
		{
			this.hasBeenSpotted = value;
		}
	}

	protected override void Awake()
	{
		base.Awake();
		this.StateMachine = new global::CheapStateMachine(51);
		this.StateMachine.AddState(new global::Idle(this), 9);
		this.StateMachine.AddState(new global::TurnStart(this), 1);
		this.StateMachine.AddState(new global::TurnMessage(this), 2);
		this.StateMachine.AddState(new global::UpdateEffects(this), 3);
		this.StateMachine.AddState(new global::Recovery(this), 4);
		this.StateMachine.AddState(new global::Terror(this), 5);
		this.StateMachine.AddState(new global::PersonalRout(this), 6);
		this.StateMachine.AddState(new global::FearCheck(this), 7);
		this.StateMachine.AddState(new global::Stupidity(this), 8);
		this.StateMachine.AddState(new global::StartMove(this), 10);
		this.StateMachine.AddState(new global::Moving(this), 11);
		this.StateMachine.AddState(new global::Engaged(this), 12);
		this.StateMachine.AddState(new global::SwitchWeapon(this), 21);
		this.StateMachine.AddState(new global::Reload(this), 19);
		this.StateMachine.AddState(new global::Disengage(this), 20);
		this.StateMachine.AddState(new global::Delay(this), 22);
		this.StateMachine.AddState(new global::Perception(this), 13);
		this.StateMachine.AddState(new global::Search(this), 14);
		this.StateMachine.AddState(new global::Inventory(this), 15);
		this.StateMachine.AddState(new global::Activate(this), 16);
		this.StateMachine.AddState(new global::Trapped(this), 17);
		this.StateMachine.AddState(new global::Teleport(this), 18);
		this.StateMachine.AddState(new global::InteractivePointTarget(this), 23);
		this.StateMachine.AddState(new global::SingleTargeting(this), 24);
		this.StateMachine.AddState(new global::AOETargeting(this), 25);
		this.StateMachine.AddState(new global::ConeTargeting(this), 26);
		this.StateMachine.AddState(new global::LineTargeting(this), 27);
		this.StateMachine.AddState(new global::SpellCasting(this), 28);
		this.StateMachine.AddState(new global::SpellCurse(this), 29);
		this.StateMachine.AddState(new global::SkillUse(this), 30);
		this.StateMachine.AddState(new global::RangeCombatFire(this), 31);
		this.StateMachine.AddState(new global::CloseCombatAttack(this), 32);
		this.StateMachine.AddState(new global::ActionWait(this), 33);
		this.StateMachine.AddState(new global::CounterChoice(this), 34);
		this.StateMachine.AddState(new global::ActivateStance(this), 35);
		this.StateMachine.AddState(new global::Overwatch(this), 36);
		this.StateMachine.AddState(new global::Ambush(this), 37);
		this.StateMachine.AddState(new global::Charge(this), 38);
		this.StateMachine.AddState(new global::TurnFinished(this), 39);
		this.StateMachine.AddState(new global::Flee(this), 40);
		this.StateMachine.AddState(new global::FleeMove(this), 41);
		this.StateMachine.AddState(new global::AIControlled(this), 42);
		this.StateMachine.AddState(new global::NetControlled(this), 43);
		this.StateMachine.AddState(new global::Overview(this), 44);
		this.StateMachine.AddState(new global::AthleticCounter(this), 45);
		this.StateMachine.AddState(new global::Athletic(this), 47);
		this.StateMachine.AddState(new global::PrepareAthletic(this), 46);
		this.StateMachine.AddState(new global::Fly(this), 48);
		this.StateMachine.AddState(new global::PrepareFly(this), 49);
		this.StateMachine.AddState(new global::ArcTargeting(this), 50);
		this.EngagedUnits = new global::System.Collections.Generic.List<global::UnitController>();
		this.defenders = new global::System.Collections.Generic.List<global::UnitController>();
		this.chargeTargets = new global::System.Collections.Generic.List<global::UnitController>();
		this.chargePreviousTargets = new global::System.Collections.Generic.List<global::UnitController>();
		this.CurrentRollModifiers = new global::AttributeModList();
		this.CurrentDamageModifiers = new global::AttributeModList();
		this.detectedUnits = new global::System.Collections.Generic.List<global::UnitController>();
		this.detectedTriggers = new global::System.Collections.Generic.List<global::TriggerPoint>();
		this.detectedInteractivePoints = new global::System.Collections.Generic.List<global::InteractivePoint>();
		this.commands = new global::System.Collections.Generic.Queue<global::UnitController.Command>();
		this.oldItems = new global::System.Collections.Generic.List<global::Item>();
		this.isCaptainMorganing = false;
		this.Initialized = false;
		this.IsFixed = false;
		this.TurnStarted = false;
		this.lootBagPoint = null;
		this.hasBeenSpotted = false;
		this.rigbody = base.GetComponent<global::UnityEngine.Rigidbody>();
		global::UnityEngine.CapsuleCollider capsuleCollider = (global::UnityEngine.CapsuleCollider)base.GetComponent<global::UnityEngine.Collider>();
		this.CapsuleHeight = capsuleCollider.height;
		this.CapsuleRadius = capsuleCollider.radius;
	}

	private void Start()
	{
		base.Highlight.seeThrough = false;
	}

	public void FirstSyncInit(global::UnitSave save, uint guid, int warbandIdx, int playerIdx, global::PlayerTypeId playerTypeId, int idxInWarband, bool merge, bool loadBodyParts = true)
	{
		this.uid = guid;
		this.RegisterToHermes();
		this.owner = (uint)playerIdx;
		if (global::PandoraSingleton<global::MissionStartData>.Instance.isReload)
		{
			for (int i = 0; i < global::PandoraSingleton<global::MissionStartData>.Instance.units.Count; i++)
			{
				if (global::PandoraSingleton<global::MissionStartData>.Instance.units[i].myrtilusId == this.uid)
				{
					save.items = global::PandoraSingleton<global::MissionStartData>.Instance.units[i].unitSave.items;
					break;
				}
			}
		}
		this.unit = new global::Unit(save);
		this.unit.warbandIdx = warbandIdx;
		this.unit.warbandPos = idxInWarband;
		if (global::PandoraSingleton<global::MissionStartData>.Instance.isReload)
		{
			global::System.Collections.Generic.List<global::MissionEndUnitSave> units = global::PandoraSingleton<global::MissionStartData>.Instance.units;
			for (int j = 0; j < units.Count; j++)
			{
				if (this.uid == units[j].myrtilusId && units[j].status == global::UnitStateId.OUT_OF_ACTION)
				{
					this.unit.Status = global::UnitStateId.OUT_OF_ACTION;
					break;
				}
			}
		}
		base.InitializeBones();
		this.Imprint = base.GetComponent<global::MapImprint>();
		if (this.Imprint == null)
		{
			this.Imprint = base.gameObject.AddComponent<global::MapImprint>();
		}
		bool flag = this.IsPlayed();
		this.Imprint.Init("unit/" + this.unit.Id.ToLowerString(), "unit/" + global::UnitId.NONE.ToLowerString(), false, global::MapImprintType.UNIT, new global::UnityEngine.Events.UnityAction<bool, bool, global::UnityEngine.Events.UnityAction>(this.Hide), this, null, null, null);
	}

	public void InitMissionUnit(global::UnitSave save, uint uId, int warbandIdx, int playerIdx, global::PlayerTypeId playerTypeId, int idxInWarband, bool loadBodyParts)
	{
		if (playerTypeId == global::PlayerTypeId.AI || playerTypeId == global::PlayerTypeId.PASSIVE_AI)
		{
			this.AICtrlr = new global::AIController(this);
			this.unit.isAI = true;
		}
		base.InstantiateAllEquipment();
		if (loadBodyParts)
		{
			base.LaunchBodyPartsLoading(new global::System.Action(this.MissionBodyPartsLoaded), false);
		}
		else
		{
			this.MissionBodyPartsLoaded();
		}
	}

	public void MissionBodyPartsLoaded()
	{
		base.MergeNoAtlas();
		this.MissionInitPostProcess();
	}

	public void MissionInitPostProcess()
	{
		foreach (global::BodyPart bodyPart in this.unit.bodyParts.Values)
		{
			for (int i = 0; i < bodyPart.relatedGO.Count; i++)
			{
				bodyPart.relatedGO[i].SetActive(true);
			}
		}
		base.InitCloth();
		base.InitBodyTrails();
		this.Hide(false, true, null);
		this.startPosition = base.transform.position;
		this.InitActionStatus();
		if (this.combatCircle == null)
		{
			this.combatCircle = base.GetComponentInChildren<global::DynamicCombatCircle>();
		}
		if (this.combatCircle == null)
		{
			global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadAssetAsync<global::UnityEngine.GameObject>("Assets/prefabs/fx/", global::AssetBundleId.FX, "fx_combat_line.prefab", delegate(global::UnityEngine.Object go)
			{
				global::UnityEngine.GameObject gameObject = (global::UnityEngine.GameObject)global::UnityEngine.Object.Instantiate(go);
				gameObject.transform.SetParent(base.transform);
				gameObject.transform.localPosition = global::UnityEngine.Vector3.zero;
				gameObject.transform.localRotation = global::UnityEngine.Quaternion.identity;
				this.combatCircle = gameObject.GetComponent<global::DynamicCombatCircle>();
				this.combatCircle.gameObject.SetActive(true);
				this.combatCircle.Init();
			});
		}
		else
		{
			this.combatCircle.gameObject.SetActive(true);
			this.combatCircle.Init();
		}
		if (this.chargeCircle == null)
		{
			this.chargeCircle = base.GetComponentInChildren<global::DynamicChargeCircle>();
		}
		if (this.chargeCircle == null)
		{
			global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadAssetAsync<global::UnityEngine.GameObject>("Assets/prefabs/fx/", global::AssetBundleId.FX, "fx_charge_line.prefab", delegate(global::UnityEngine.Object go)
			{
				global::UnityEngine.GameObject gameObject = (global::UnityEngine.GameObject)global::UnityEngine.Object.Instantiate(go);
				gameObject.transform.SetParent(base.transform);
				gameObject.transform.localPosition = global::UnityEngine.Vector3.zero;
				gameObject.transform.localRotation = global::UnityEngine.Quaternion.identity;
				this.chargeCircle = gameObject.GetComponent<global::DynamicChargeCircle>();
				this.chargeCircle.gameObject.SetActive(false);
				this.chargeCircle.Init();
			});
		}
		else
		{
			this.chargeCircle.gameObject.SetActive(false);
			this.chargeCircle.Init();
		}
		this.fleeDistanceMultiplier = global::Constant.GetFloat(global::ConstantId.FLEE_MOVEMENT_MULTIPLIER);
		global::UnityEngine.AnimatorCullingMode cullingMode = this.animator.cullingMode;
		this.animator.cullingMode = global::UnityEngine.AnimatorCullingMode.AlwaysAnimate;
		this.animator.Play(global::AnimatorIds.idle, -1);
		this.animator.cullingMode = cullingMode;
		this.animator.updateMode = global::UnityEngine.AnimatorUpdateMode.AnimatePhysics;
		this.animator.applyRootMotion = true;
		this.InitBoneTargets();
		if (this.shaderSetter != null)
		{
			this.shaderSetter.ApplyShaderParams();
		}
		global::UnityEngine.Object.Destroy(this.shaderSetter);
		this.StateMachine.ChangeState(9);
		this.Initialized = true;
		if (this.finishedLoad != null)
		{
			this.finishedLoad();
		}
	}

	public void StartGameInitialization()
	{
		this.unit.ResetPoints();
		this.unit.SetAttribute(global::AttributeId.CURRENT_OFFENSE_POINTS, global::UnityEngine.Mathf.Min(this.unit.CurrentOffensePoints, (!this.GetWarband().IsAmbushed()) ? 2 : 1));
		if (!global::PandoraSingleton<global::MissionStartData>.Instance.isReload)
		{
			this.unit.AddEnchantment(global::EnchantmentId.BASE_SKILL_STANCE_DODGE, this.unit, false, false, global::AllegianceId.NONE);
			if (this.GetWarband().IsAmbusher())
			{
				this.unit.AddEnchantment(global::EnchantmentId.BALANCING_AMBUSHER_BUFF, this.unit, false, false, global::AllegianceId.NONE);
			}
			else if (this.GetWarband().IsAmbushed())
			{
				this.unit.AddEnchantment(global::EnchantmentId.BALANCING_AMBUSHEE_DEBUFF, this.unit, false, false, global::AllegianceId.NONE);
			}
			if (this.AICtrlr != null)
			{
				int num = 0;
				for (int i = 0; i < global::PandoraSingleton<global::MissionManager>.Instance.WarbandCtrlrs.Count; i++)
				{
					if (global::PandoraSingleton<global::MissionManager>.Instance.WarbandCtrlrs[i].playerTypeId == global::PlayerTypeId.PLAYER)
					{
						num++;
					}
				}
				if (!global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.isCampaign && num == 1)
				{
					int num2 = 970;
					num2 += this.unit.Rank;
					this.unit.AddEnchantment((global::EnchantmentId)num2, this.unit, false, false, global::AllegianceId.NONE);
				}
				if (this.GetWarband().IsRoaming())
				{
					this.unit.AddEnchantment(global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::ProcMissionRatingData>(1).EnchantmentId, this.unit, false, false, global::AllegianceId.NONE);
				}
				else
				{
					this.unit.AddEnchantment(global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::ProcMissionRatingData>(global::PandoraSingleton<global::MissionStartData>.Instance.CurrentMission.missionSave.ratingId).EnchantmentId, this.unit, false, false, global::AllegianceId.NONE);
				}
				if (this.unit.UnitSave.campaignId != 0)
				{
					this.unit.AddEnchantment(global::EnchantmentId.PERK_NO_SEARCH, this.unit, false, false, global::AllegianceId.NONE);
				}
			}
			this.unit.UpdateAttributes();
			this.unit.SetAttribute(global::AttributeId.CURRENT_WOUND, this.unit.Wound);
		}
		this.unit.UpdateEnchantmentsFx();
		this.unit.CacheBackpackSize();
		base.SwitchWeapons(this.unit.ActiveWeaponSlot);
		this.InitTargetsData();
	}

	public void Deployed(bool checkEngaged = true)
	{
		this.Ground();
		this.SetCombatCircle(this, true);
		this.animator.Play(global::AnimatorIds.idle, -1, (float)global::PandoraSingleton<global::MissionManager>.Instance.NetworkTyche.Rand(0.0, 1.0));
		if (base.Highlight != null)
		{
			base.Highlight.ReinitMaterials();
		}
		if (this.IsPlayed())
		{
			this.Imprint.alwaysVisible = true;
		}
		if (this.linkedSearchPoints != null && this.unit.Status != global::UnitStateId.OUT_OF_ACTION && this.unlockSearchPointOnDeath)
		{
			for (int i = 0; i < this.linkedSearchPoints.Count; i++)
			{
				this.linkedSearchPoints[i].gameObject.SetActive(false);
			}
		}
		if (global::PandoraSingleton<global::MissionStartData>.Instance.isReload)
		{
			global::System.Collections.Generic.List<global::MissionEndUnitSave> units = global::PandoraSingleton<global::MissionStartData>.Instance.units;
			for (int j = 0; j < units.Count; j++)
			{
				if (this.uid == units[j].myrtilusId)
				{
					base.transform.rotation = global::UnityEngine.Quaternion.Euler(units[j].rotation);
					this.SetFixed(units[j].position, true);
					if (units[j].status == global::UnitStateId.STUNNED)
					{
						this.animator.Play(global::AnimatorIds.kneeling_stunned);
						this.animator.SetInteger(global::AnimatorIds.unit_state, 2);
						this.unit.Status = global::UnitStateId.STUNNED;
					}
					else if (units[j].status == global::UnitStateId.OUT_OF_ACTION)
					{
						this.animator.Play(global::AnimatorIds.out_of_action);
						this.animator.SetInteger(global::AnimatorIds.unit_state, 3);
						this.unit.Status = global::UnitStateId.OUT_OF_ACTION;
						base.GetComponent<global::UnityEngine.Collider>().enabled = false;
						this.Imprint.alive = false;
						this.Imprint.needsRefresh = true;
					}
					global::System.Collections.Generic.List<global::UnitController> allUnits = global::PandoraSingleton<global::MissionManager>.Instance.GetAllUnits();
					for (int k = 0; k < units[j].enchantments.Count; k++)
					{
						global::EndUnitEnchantment endUnitEnchantment = units[j].enchantments[k];
						global::UnitController unitController = null;
						for (int l = 0; l < allUnits.Count; l++)
						{
							if (allUnits[l].uid == endUnitEnchantment.ownerMyrtilusId)
							{
								unitController = allUnits[l];
							}
						}
						if (unitController == null)
						{
							global::System.Collections.Generic.List<global::UnitController> excludedUnits = global::PandoraSingleton<global::MissionManager>.Instance.excludedUnits;
							for (int m = 0; m < excludedUnits.Count; m++)
							{
								if (excludedUnits[m].uid == endUnitEnchantment.ownerMyrtilusId)
								{
									unitController = excludedUnits[m];
								}
							}
						}
						global::Enchantment enchantment = this.unit.AddEnchantment(endUnitEnchantment.enchantId, unitController.unit, false, true, (global::AllegianceId)endUnitEnchantment.runeAllegianceId);
						if (enchantment != null)
						{
							enchantment.Duration = endUnitEnchantment.durationLeft;
						}
					}
					if (this.unit.Status != global::UnitStateId.OUT_OF_ACTION)
					{
						base.SwitchWeapons(units[j].weaponSet);
						this.unit.UpdateAttributes();
						this.unit.SetAttribute(global::AttributeId.CURRENT_WOUND, units[j].currentWounds);
						this.unit.SetAttribute(global::AttributeId.CURRENT_STRATEGY_POINTS, units[j].currentSP);
						this.unit.SetAttribute(global::AttributeId.CURRENT_OFFENSE_POINTS, units[j].currentOP);
						this.TurnStarted = units[j].turnStarted;
					}
					this.ladderVisible = units[j].isLadderVisible;
					this.unit.SetAttribute(global::AttributeId.CURRENT_MVU, units[j].currentMvu);
					this.MVUptsPerCategory = units[j].mvuPerCategories;
					break;
				}
			}
		}
		this.SetCombatCircle(this, true);
		if (checkEngaged)
		{
			this.CheckEngaged(true);
		}
		global::PandoraSingleton<global::MissionManager>.Instance.MissionEndData.UpdateUnit(this);
	}

	private void OnDestroy()
	{
		this.StateMachine.Destroy();
		this.RemoveFromHermes();
	}

	private void Update()
	{
		this.commandSent = false;
		if (this.commandsToSend.Count > 0)
		{
			global::UnitController.Command command = this.commandsToSend.Dequeue();
			this.Send(command.reliable, command.target, (uint)command.from, command.command, command.parms);
		}
		if (this.nextState != global::UnitController.State.NONE)
		{
			int stateIndex = (int)this.nextState;
			this.nextState = global::UnitController.State.NONE;
			this.StateMachine.ChangeState(stateIndex);
		}
		while (this.commands.Count > 0 && this.CanLaunchCommand())
		{
			global::UnitController.Command com = this.commands.Dequeue();
			this.RunCommand(com);
		}
		this.StateMachine.Update();
		float @float = this.animator.GetFloat(global::AnimatorIds.speed);
		if (this.Engaged && @float > -1f && this.currentAnimSpeed <= 0f)
		{
			this.currentAnimSpeed -= global::UnityEngine.Time.deltaTime * 2f;
			this.currentAnimSpeed = global::UnityEngine.Mathf.Max(this.currentAnimSpeed, -1f);
			this.animator.SetFloat(global::AnimatorIds.speed, this.currentAnimSpeed);
		}
		if (!this.Engaged && this.animator.GetFloat(global::AnimatorIds.speed) < 0f)
		{
			this.currentAnimSpeed += global::UnityEngine.Time.deltaTime * 2f;
			this.currentAnimSpeed = global::UnityEngine.Mathf.Min(this.currentAnimSpeed, 0f);
			base.SetAnimSpeed(this.currentAnimSpeed);
		}
	}

	private bool CanLaunchCommand()
	{
		global::PandoraDebug.LogInfo(string.Concat(new object[]
		{
			"Check CanLaunch Command! State = ",
			(global::UnitController.State)this.StateMachine.GetActiveStateId(),
			" sequence is playing : ",
			global::PandoraSingleton<global::SequenceManager>.Instance.isPlaying
		}), "UNIT", this);
		return !this.Fleeing && (!global::PandoraSingleton<global::SequenceManager>.Instance.isPlaying || this.StateMachine.GetActiveStateId() == 38) && (this.StateMachine.GetActiveStateId() == 9 || this.StateMachine.GetActiveStateId() == 11 || this.StateMachine.GetActiveStateId() == 12 || this.StateMachine.GetActiveStateId() == 14 || this.StateMachine.GetActiveStateId() == 15 || this.StateMachine.GetActiveStateId() == 16 || this.StateMachine.GetActiveStateId() == 17 || this.StateMachine.GetActiveStateId() == 18 || this.StateMachine.GetActiveStateId() == 20 || this.StateMachine.GetActiveStateId() == 23 || this.StateMachine.GetActiveStateId() == 46 || this.StateMachine.GetActiveStateId() == 24 || this.StateMachine.GetActiveStateId() == 25 || this.StateMachine.GetActiveStateId() == 26 || this.StateMachine.GetActiveStateId() == 27 || this.StateMachine.GetActiveStateId() == 33 || this.StateMachine.GetActiveStateId() == 34 || this.StateMachine.GetActiveStateId() == 36 || this.StateMachine.GetActiveStateId() == 37 || this.StateMachine.GetActiveStateId() == 38 || this.StateMachine.GetActiveStateId() == 39 || this.StateMachine.GetActiveStateId() == 40 || this.StateMachine.GetActiveStateId() == 41 || this.StateMachine.GetActiveStateId() == 42 || this.StateMachine.GetActiveStateId() == 43 || this.StateMachine.GetActiveStateId() == 44 || this.StateMachine.GetActiveStateId() == 49);
	}

	protected override void LateUpdate()
	{
		base.LateUpdate();
		if (this.IsFixed)
		{
			base.transform.position = this.fixPosition;
		}
		this.lastChargeMvt = -1;
		this.Resurected = false;
	}

	private void FixedUpdate()
	{
		this.StateMachine.FixedUpdate();
		if (this.newRotation != global::UnityEngine.Quaternion.identity)
		{
			base.transform.rotation = this.newRotation;
		}
	}

	public bool IsCurrentState(global::UnitController.State state)
	{
		return this.StateMachine.GetActiveStateId() == (int)state;
	}

	public void SetFixed(bool fix)
	{
		if (fix)
		{
			this.fixPosition = base.transform.position;
			this.rigbody.drag = 100f;
		}
		else
		{
			this.rigbody.drag = 0f;
		}
		this.IsFixed = fix;
	}

	public void SetFixed(global::UnityEngine.Vector3 position, bool fix)
	{
		base.transform.position = position;
		this.SetFixed(fix);
		for (int i = 0; i < this.cloths.Count; i++)
		{
			if (this.cloths[i] != null)
			{
				this.cloths[i].ClearTransformMotion();
			}
		}
	}

	public void SetKinemantic(bool kine)
	{
		this.rigbody.isKinematic = kine;
	}

	public global::WarbandController GetWarband()
	{
		if (this.unit.warbandIdx < global::PandoraSingleton<global::MissionManager>.Instance.WarbandCtrlrs.Count)
		{
			return global::PandoraSingleton<global::MissionManager>.Instance.WarbandCtrlrs[this.unit.warbandIdx];
		}
		return null;
	}

	public bool IsMine()
	{
		return (ulong)this.owner == (ulong)((long)global::PandoraSingleton<global::Hermes>.Instance.PlayerIndex);
	}

	public bool IsEnemy(global::UnitController otherUnit)
	{
		global::WarbandController warband = this.GetWarband();
		global::WarbandController warband2 = otherUnit.GetWarband();
		return warband.idx != warband2.idx && warband.teamIdx != warband2.teamIdx && warband2.BlackListed(warband.idx);
	}

	public string GetNameVisible()
	{
		return (!this.IsImprintVisible() && !this.IsPlayed()) ? global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("unknown_unit") : this.unit.Name;
	}

	public string GetLogName()
	{
		bool flag = this.IsPlayed();
		global::System.Text.StringBuilder stringBuilder = global::PandoraUtils.StringBuilder;
		stringBuilder.Append(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById((!flag) ? "color_red" : "color_blue"));
		stringBuilder.Append(this.GetNameVisible());
		stringBuilder.Append(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("color_end"));
		return stringBuilder.ToString();
	}

	public bool IsPlayed()
	{
		return this.GetWarband() != null && this.GetWarband().IsPlayed();
	}

	public bool IsImprintVisible()
	{
		bool flag = this.Imprint.State == global::MapImprintStateId.VISIBLE;
		this.hasBeenSpotted = (this.hasBeenSpotted || flag);
		return flag;
	}

	public int GetCurrentShots()
	{
		if (base.Equipments != null && base.Equipments[(int)this.unit.ActiveWeaponSlot] != null)
		{
			return base.Equipments[(int)this.unit.ActiveWeaponSlot].CurrentShots();
		}
		return 0;
	}

	public int GetMaxShots()
	{
		if (base.Equipments != null && base.Equipments[(int)this.unit.ActiveWeaponSlot] != null)
		{
			return base.Equipments[(int)this.unit.ActiveWeaponSlot].Item.Shots;
		}
		return 0;
	}

	public void FaceTarget(global::UnityEngine.Transform target, bool force = false)
	{
		if (target == base.transform)
		{
			return;
		}
		this.FaceTarget(target.position, force);
	}

	public void FaceTarget(global::UnityEngine.Vector3 position, bool force = false)
	{
		if (global::UnityEngine.Mathf.Approximately(position.x, base.transform.position.x) && global::UnityEngine.Mathf.Approximately(position.z, base.transform.position.z))
		{
			return;
		}
		if (this.unit.Status == global::UnitStateId.OUT_OF_ACTION || this.unit.Id == global::UnitId.MANTICORE)
		{
			return;
		}
		if (global::UnityEngine.Mathf.Abs(position.x - base.transform.position.x) < 0.01f && global::UnityEngine.Mathf.Abs(position.z - base.transform.position.z) < 0.01f)
		{
			return;
		}
		global::UnityEngine.Quaternion rotation = default(global::UnityEngine.Quaternion);
		rotation.SetLookRotation(position - base.transform.position, global::UnityEngine.Vector3.up);
		global::UnityEngine.Vector3 eulerAngles = rotation.eulerAngles;
		eulerAngles.x = 0f;
		eulerAngles.z = 0f;
		rotation = global::UnityEngine.Quaternion.Euler(eulerAngles);
		base.StopCoroutine("Face");
		if (force)
		{
			base.transform.rotation = rotation;
			return;
		}
		if (base.transform.rotation.eulerAngles.y != rotation.eulerAngles.y)
		{
			this.faceTargetRotation = rotation;
			base.StartCoroutine("Face");
		}
	}

	private global::System.Collections.IEnumerator Face()
	{
		for (;;)
		{
			base.transform.rotation = global::UnityEngine.Quaternion.Lerp(base.transform.rotation, this.faceTargetRotation, 10f * global::UnityEngine.Time.deltaTime);
			if (global::UnityEngine.Quaternion.Angle(base.transform.rotation, this.faceTargetRotation) < 0.1f)
			{
				break;
			}
			yield return 0;
		}
		yield break;
		yield break;
	}

	private void OnAnimatorIK()
	{
		if (this.isCaptainMorganing && this.animator.deltaPosition == global::UnityEngine.Vector3.zero && this.unit.Status == global::UnitStateId.NONE && this.animator.GetCurrentAnimatorStateInfo(0).fullPathHash == global::AnimatorIds.idle)
		{
			global::UnityEngine.LayerMask mask = 1 << global::UnityEngine.LayerMask.NameToLayer("environment") | 1 << global::UnityEngine.LayerMask.NameToLayer("props_big") | 1 << global::UnityEngine.LayerMask.NameToLayer("ground");
			float num = 0f;
			float num2 = 0f;
			global::UnityEngine.Vector3 ikposition = this.animator.GetIKPosition(global::UnityEngine.AvatarIKGoal.RightFoot);
			float num3 = global::UnityEngine.Mathf.Abs(ikposition.y - this.animator.bodyPosition.y);
			global::UnityEngine.Ray ray = new global::UnityEngine.Ray(ikposition + global::UnityEngine.Vector3.up * 0.4f, -global::UnityEngine.Vector3.up);
			global::UnityEngine.RaycastHit raycastHit;
			if (global::UnityEngine.Physics.Raycast(ray, out raycastHit, 2f, mask))
			{
				float num4 = global::UnityEngine.Mathf.Abs(raycastHit.point.y - this.animator.bodyPosition.y);
				global::UnityEngine.Vector3 point = raycastHit.point;
				point.y += this.animator.rightFeetBottomHeight;
				num2 = num3 - num4 + this.animator.rightFeetBottomHeight;
				if ((double)global::UnityEngine.Mathf.Abs(num2) < 0.4)
				{
					this.animator.SetIKPosition(global::UnityEngine.AvatarIKGoal.RightFoot, point);
					this.animator.SetIKPositionWeight(global::UnityEngine.AvatarIKGoal.RightFoot, 1f);
					global::UnityEngine.Quaternion quaternion = this.animator.GetIKRotation(global::UnityEngine.AvatarIKGoal.RightFoot);
					global::UnityEngine.Vector3 fromDirection = quaternion * global::UnityEngine.Vector3.up;
					quaternion = global::UnityEngine.Quaternion.FromToRotation(fromDirection, raycastHit.normal) * quaternion;
					this.animator.SetIKRotation(global::UnityEngine.AvatarIKGoal.RightFoot, quaternion);
					this.animator.SetIKRotationWeight(global::UnityEngine.AvatarIKGoal.RightFoot, 1f);
				}
			}
			global::UnityEngine.Vector3 ikposition2 = this.animator.GetIKPosition(global::UnityEngine.AvatarIKGoal.LeftFoot);
			float num5 = global::UnityEngine.Mathf.Abs(ikposition2.y - this.animator.bodyPosition.y);
			global::UnityEngine.Ray ray2 = new global::UnityEngine.Ray(ikposition2 + global::UnityEngine.Vector3.up * 0.4f, -global::UnityEngine.Vector3.up);
			global::UnityEngine.RaycastHit raycastHit2;
			if (global::UnityEngine.Physics.Raycast(ray2, out raycastHit2, 2f, mask))
			{
				float num6 = global::UnityEngine.Mathf.Abs(raycastHit2.point.y - this.animator.bodyPosition.y);
				global::UnityEngine.Vector3 point2 = raycastHit2.point;
				point2.y += this.animator.leftFeetBottomHeight;
				num = num5 - num6 + this.animator.leftFeetBottomHeight;
				if ((double)global::UnityEngine.Mathf.Abs(num) < 0.4)
				{
					this.animator.SetIKPosition(global::UnityEngine.AvatarIKGoal.LeftFoot, point2);
					this.animator.SetIKPositionWeight(global::UnityEngine.AvatarIKGoal.LeftFoot, 1f);
					global::UnityEngine.Quaternion quaternion2 = this.animator.GetIKRotation(global::UnityEngine.AvatarIKGoal.LeftFoot);
					global::UnityEngine.Vector3 fromDirection2 = quaternion2 * global::UnityEngine.Vector3.up;
					quaternion2 = global::UnityEngine.Quaternion.FromToRotation(fromDirection2, raycastHit2.normal) * quaternion2;
					this.animator.SetIKRotation(global::UnityEngine.AvatarIKGoal.LeftFoot, quaternion2);
					this.animator.SetIKRotationWeight(global::UnityEngine.AvatarIKGoal.LeftFoot, 1f);
				}
			}
			if ((double)global::UnityEngine.Mathf.Abs(num) < 0.4 && (double)global::UnityEngine.Mathf.Abs(num2) < 0.4 && ((double)num2 > 0.1 || (double)num2 < -0.1 || (double)num > 0.1 || (double)num < -0.1))
			{
				if ((double)num2 < -0.1 && (double)num < -0.1)
				{
					if (num2 > num)
					{
						this.animator.bodyPosition = new global::UnityEngine.Vector3(this.animator.bodyPosition.x, this.animator.bodyPosition.y + num, this.animator.bodyPosition.z);
					}
					else
					{
						this.animator.bodyPosition = new global::UnityEngine.Vector3(this.animator.bodyPosition.x, this.animator.bodyPosition.y + num2, this.animator.bodyPosition.z);
					}
				}
				else if ((double)num2 > 0.1 && (double)num < -0.1)
				{
					this.animator.bodyPosition = new global::UnityEngine.Vector3(this.animator.bodyPosition.x, this.animator.bodyPosition.y - num2, this.animator.bodyPosition.z);
				}
				else if ((double)num2 < -0.1 && (double)num > 0.1)
				{
					this.animator.bodyPosition = new global::UnityEngine.Vector3(this.animator.bodyPosition.x, this.animator.bodyPosition.y - num, this.animator.bodyPosition.z);
				}
				else if ((double)num2 > 0.1 && (double)num > 0.1)
				{
					if (num2 > num)
					{
						this.animator.bodyPosition = new global::UnityEngine.Vector3(this.animator.bodyPosition.x, this.animator.bodyPosition.y + num, this.animator.bodyPosition.z);
					}
					else
					{
						this.animator.bodyPosition = new global::UnityEngine.Vector3(this.animator.bodyPosition.x, this.animator.bodyPosition.y + num2, this.animator.bodyPosition.z);
					}
				}
				else if ((double)num2 < -0.1 || (double)num < -0.1)
				{
					if (num2 < num)
					{
						this.animator.bodyPosition = new global::UnityEngine.Vector3(this.animator.bodyPosition.x, this.animator.bodyPosition.y + num2, this.animator.bodyPosition.z);
					}
					else
					{
						this.animator.bodyPosition = new global::UnityEngine.Vector3(this.animator.bodyPosition.x, this.animator.bodyPosition.y + num, this.animator.bodyPosition.z);
					}
				}
			}
		}
	}

	public void ResetAtkUsed()
	{
		this.attackUsed = 0;
	}

	private void InitActionStatus()
	{
		this.actionStatus = new global::System.Collections.Generic.List<global::ActionStatus>();
		global::System.Collections.Generic.List<global::SkillData> list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::SkillData>("fk_skill_type_id", 1.ToConstantString());
		for (int i = 0; i < list.Count; i++)
		{
			this.actionStatus.Add(new global::ActionStatus(list[i], this));
		}
		for (int j = 0; j < this.unit.ActiveSkills.Count; j++)
		{
			this.actionStatus.Add(new global::ActionStatus(this.unit.ActiveSkills[j], this));
		}
		for (int k = 0; k < this.unit.Spells.Count; k++)
		{
			this.actionStatus.Add(new global::ActionStatus(this.unit.Spells[k], this));
		}
	}

	public bool UpdateActionStatus(bool notice, global::UnitActionRefreshId refreshType = global::UnitActionRefreshId.NONE)
	{
		if (global::PandoraSingleton<global::MissionManager>.Instance.GetCurrentUnit() == this)
		{
			this.UpdateTargetsData();
		}
		this.availableActionStatus.Clear();
		global::System.Collections.Generic.List<global::Item> list = new global::System.Collections.Generic.List<global::Item>();
		for (int i = this.actionStatus.Count - 1; i >= 0; i--)
		{
			if (this.actionStatus[i].LinkedItem != null)
			{
				if (!this.unit.HasItem(this.actionStatus[i].LinkedItem))
				{
					this.actionStatus.RemoveAt(i);
				}
				else
				{
					list.Add(this.actionStatus[i].LinkedItem);
				}
			}
		}
		for (int j = 6; j < this.unit.Items.Count; j++)
		{
			if (this.unit.Items[j].Id != global::ItemId.NONE && this.unit.Items[j].ConsumableData != null && !this.unit.Items[j].ConsumableData.OutOfCombat)
			{
				bool flag = false;
				for (int k = 0; k < list.Count; k++)
				{
					if (list[k].Id == this.unit.Items[j].Id && list[k].QualityData.Id == this.unit.Items[j].QualityData.Id)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					this.actionStatus.Add(new global::ActionStatus(this.unit.Items[j], this));
					list.Add(this.unit.Items[j]);
				}
			}
		}
		bool flag2 = refreshType != global::UnitActionRefreshId.ALWAYS;
		for (int l = 0; l < this.actionStatus.Count; l++)
		{
			global::ActionStatus actionStatus = this.actionStatus[l];
			if (refreshType == global::UnitActionRefreshId.NONE || refreshType == global::UnitActionRefreshId.ALWAYS || actionStatus.actionData.UnitActionRefreshId == refreshType)
			{
				bool available = actionStatus.Available;
				actionStatus.UpdateAvailable();
				flag2 |= (available != actionStatus.Available);
			}
			if (actionStatus.Available)
			{
				this.availableActionStatus.Add(actionStatus);
			}
		}
		this.availableActionStatus.Sort(new global::ActionStatusComparer());
		if (!notice || !flag2 || this.IsPlayed())
		{
		}
		return flag2;
	}

	public void SetCurrentAction(global::SkillId id)
	{
		this.CurrentAction = this.GetAction(id);
		this.RecalculateModifiers();
	}

	public void RecalculateModifiers()
	{
		this.CurrentRollModifiers.Clear();
		this.CurrentDamageModifiers.Clear();
		if (this.CurrentAction != null && this.IsPlayed())
		{
			this.CurrentAction.GetRoll(true);
			this.CurrentAction.GetMinDamage(true);
		}
	}

	public bool CanShowEnchantment(global::Enchantment x, global::EffectTypeId effecType)
	{
		return effecType == x.Data.EffectTypeId && !x.Data.NoDisplay && !this.unit.HasEnchantmentImmunity(x);
	}

	public int GetEffectTypeCount(global::EffectTypeId effectType)
	{
		int num = 0;
		num += this.GetEnchantmentsCount(this.unit.Enchantments, effectType);
		for (int i = 0; i < this.unit.ActiveItems.Count; i++)
		{
			num += this.GetEnchantmentsCount(this.unit.ActiveItems[i].Enchantments, effectType);
		}
		for (int j = 0; j < this.unit.Injuries.Count; j++)
		{
			num += this.GetEnchantmentsCount(this.unit.Injuries[j].Enchantments, effectType);
		}
		for (int k = 0; k < this.unit.Mutations.Count; k++)
		{
			num += this.GetEnchantmentsCount(this.unit.Mutations[k].Enchantments, effectType);
		}
		return num;
	}

	private int GetEnchantmentsCount(global::System.Collections.Generic.List<global::Enchantment> enchantments, global::EffectTypeId effectType)
	{
		int num = 0;
		for (int i = 0; i < enchantments.Count; i++)
		{
			if (this.CanShowEnchantment(enchantments[i], effectType))
			{
				num++;
			}
		}
		return num;
	}

	public global::ActionStatus GetAction(global::SkillId skillId)
	{
		for (int i = 0; i < this.actionStatus.Count; i++)
		{
			if (this.actionStatus[i].SkillId == skillId)
			{
				this.actionStatus[i].UpdateAvailable();
				return this.actionStatus[i];
			}
		}
		global::SkillData data = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::SkillData>((int)skillId);
		global::ActionStatus actionStatus = new global::ActionStatus(data, this);
		actionStatus.UpdateAvailable();
		return actionStatus;
	}

	public global::System.Collections.Generic.List<global::ActionStatus> GetActions(global::UnitActionId actionId)
	{
		global::System.Collections.Generic.List<global::ActionStatus> list = new global::System.Collections.Generic.List<global::ActionStatus>();
		for (int i = 0; i < this.actionStatus.Count; i++)
		{
			if (this.actionStatus[i].ActionId == actionId)
			{
				list.Add(this.actionStatus[i]);
			}
		}
		return list;
	}

	public void WaitForAction(global::UnitController.State nextState)
	{
		global::PandoraDebug.LogInfo(string.Concat(new object[]
		{
			"WaitForAction Unit ",
			base.name,
			" state ",
			nextState
		}), "WAIT FOR ACTION", null);
		global::PandoraSingleton<global::MissionManager>.Instance.delayedUnits.Push(this);
		this.actionDoneNextState = nextState;
		this.StateMachine.ChangeState(33);
	}

	public void LaunchMelee(global::UnitController.State nextState)
	{
		global::PandoraDebug.LogDebug("Launch Melee", "ACTION", this);
		if (this.defenderCtrlr == null)
		{
			this.defenderCtrlr = this.EngagedUnits[0];
		}
		this.hadTerror = this.unit.HasEnchantment(global::EnchantmentTypeId.TERROR);
		this.hadFear = this.unit.HasEnchantment(global::EnchantmentTypeId.FEAR);
		this.defHadTerror = this.defenderCtrlr.unit.HasEnchantment(global::EnchantmentTypeId.TERROR);
		this.defHadFear = this.defenderCtrlr.unit.HasEnchantment(global::EnchantmentTypeId.FEAR);
		this.WaitForAction(nextState);
		this.TriggerEnchantments(global::EnchantmentTriggerId.ON_ENGAGE, global::SkillId.NONE, global::UnitActionId.NONE);
		this.defenderCtrlr.defenderCtrlr = this;
		this.defenderCtrlr.TriggerEnchantments(global::EnchantmentTriggerId.ON_ENGAGE, global::SkillId.NONE, global::UnitActionId.NONE);
		this.CheckTerror();
	}

	public void CheckAllAlone()
	{
		if (!this.unit.HasEnchantment(global::EnchantmentTypeId.ALL_ALONE) && this.IsAllAlone())
		{
			this.unit.AddEnchantment(global::EnchantmentId.ALL_ALONE, this.unit, false, true, global::AllegianceId.NONE);
		}
		else if (this.unit.HasEnchantment(global::EnchantmentTypeId.ALL_ALONE) && !this.IsAllAlone())
		{
			this.unit.RemoveEnchantments(global::EnchantmentTypeId.ALL_ALONE);
		}
		if (this.unit.HasEnchantment(global::EnchantmentTypeId.ALL_ALONE))
		{
			this.nextState = global::UnitController.State.PERSONAL_ROUT;
		}
		else
		{
			this.CheckTerror();
		}
	}

	public bool IsAllAlone()
	{
		if (this.EngagedUnits.Count > 1)
		{
			int num = 0;
			for (int i = 0; i < this.EngagedUnits.Count; i++)
			{
				global::UnitController unitController = this.EngagedUnits[i];
				if (unitController.unit.Status == global::UnitStateId.NONE && unitController.HasClose())
				{
					bool flag = false;
					for (int j = 0; j < unitController.EngagedUnits.Count; j++)
					{
						if (unitController.EngagedUnits[j] != this && unitController.EngagedUnits[j].unit.Status == global::UnitStateId.NONE && unitController.EngagedUnits[j].HasClose())
						{
							flag = true;
						}
					}
					if (!flag)
					{
						num++;
					}
				}
			}
			if (num >= 2)
			{
				return true;
			}
		}
		return false;
	}

	public void ReapplyOnEngage()
	{
		for (int i = 0; i < this.EngagedUnits.Count; i++)
		{
			global::UnitController defenderCtrlr = this.EngagedUnits[i].defenderCtrlr;
			this.EngagedUnits[i].defenderCtrlr = this;
			this.EngagedUnits[i].TriggerEnchantments(global::EnchantmentTriggerId.ON_ENGAGE, global::SkillId.NONE, global::UnitActionId.NONE);
			this.EngagedUnits[i].defenderCtrlr = defenderCtrlr;
		}
	}

	public void CheckTerror()
	{
		if (this.unit.HasEnchantment(global::EnchantmentTypeId.TERROR) && !this.hadTerror)
		{
			this.nextState = global::UnitController.State.TERROR;
		}
		else if (this.defenderCtrlr != null && this.defenderCtrlr.unit.HasEnchantment(global::EnchantmentTypeId.TERROR) && !this.defHadTerror)
		{
			this.defenderCtrlr.StateMachine.ChangeState(5);
			this.StateMachine.ChangeState(33);
		}
		else
		{
			this.CheckFear();
		}
		this.hadTerror = false;
		this.defHadTerror = false;
	}

	public void CheckFear()
	{
		if (this.unit.HasEnchantment(global::EnchantmentTypeId.FEAR) && !this.hadFear)
		{
			this.nextState = global::UnitController.State.FEAR_CHECK;
		}
		else if (this.defenderCtrlr != null && this.defenderCtrlr.unit.HasEnchantment(global::EnchantmentTypeId.FEAR) && !this.defHadFear)
		{
			this.defenderCtrlr.StateMachine.ChangeState(7);
			this.StateMachine.ChangeState(33);
		}
		else
		{
			this.ActionDone();
		}
		this.hadFear = false;
		this.defHadFear = false;
	}

	public void ActionDone()
	{
		global::PandoraSingleton<global::MissionManager>.Instance.TurnTimer.Resume();
		this.StateMachine.ChangeState(9);
		global::PandoraDebug.LogDebug(string.Concat(new object[]
		{
			"Unit ",
			base.name,
			" from warband ",
			this.GetWarband().idx,
			" is calling ActionDone"
		}), "WAIT FOR ACTION", this);
		global::UnitController unitController = global::PandoraSingleton<global::MissionManager>.Instance.delayedUnits.Pop();
		global::PandoraDebug.LogDebug(string.Concat(new object[]
		{
			"Action Done: delayed Unit was ",
			unitController.name,
			" next state ",
			unitController.actionDoneNextState
		}), "WAIT FOR ACTION", this);
		unitController.StateMachine.ChangeState((int)unitController.actionDoneNextState);
	}

	public void SetChargeTargets(bool forceSendNotice = false)
	{
		if (this.unit.ChargeMovement == this.lastChargeMvt)
		{
			return;
		}
		this.lastChargeMvt = this.unit.ChargeMovement;
		global::System.Collections.Generic.List<global::UnitController> list = this.chargeTargets;
		this.chargeTargets = this.chargePreviousTargets;
		this.chargePreviousTargets = list;
		this.chargeTargets.Clear();
		global::System.Collections.Generic.List<global::UnitController> aliveEnemies = global::PandoraSingleton<global::MissionManager>.Instance.GetAliveEnemies(this.unit.warbandIdx);
		for (int i = 0; i < aliveEnemies.Count; i++)
		{
			if (this.CanChargeUnit(aliveEnemies[i], false))
			{
				this.chargeTargets.Add(aliveEnemies[i]);
			}
		}
		if (forceSendNotice || this.chargeTargets.Count != this.chargePreviousTargets.Count)
		{
			bool flag = true;
			int num = 0;
			while (num < this.chargeTargets.Count && flag)
			{
				flag = false;
				for (int j = 0; j < this.chargePreviousTargets.Count; j++)
				{
					if (this.chargePreviousTargets[j] == this.chargeTargets[num])
					{
						flag = true;
						break;
					}
				}
				num++;
			}
			if (!flag)
			{
				global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<int, global::System.Collections.Generic.List<global::UnitController>>(global::Notices.COMBAT_HIGHLIGHT_TARGET, -1, this.chargeTargets);
			}
		}
	}

	public bool CanChargeUnit(global::UnitController enemy, bool isAmbush = false)
	{
		if (enemy.IsInFriendlyZone)
		{
			return false;
		}
		bool flag = this.unit.Data.UnitSizeId == global::UnitSizeId.LARGE || enemy.unit.Data.UnitSizeId == global::UnitSizeId.LARGE;
		float @float = global::Constant.GetFloat((!flag) ? global::ConstantId.MELEE_RANGE_NORMAL : global::ConstantId.MELEE_RANGE_LARGE);
		float num = (float)((!isAmbush) ? this.unit.ChargeMovement : this.unit.AmbushMovement);
		num += @float;
		float num2 = global::UnityEngine.Vector3.Distance(base.transform.position, enemy.transform.position);
		bool flag2 = false;
		float num3 = (!isAmbush) ? global::Constant.GetFloat(global::ConstantId.CHARGE_MIN_DIST) : @float;
		float num4 = global::UnityEngine.Mathf.Abs(base.transform.position.y - enemy.transform.position.y);
		if (num2 >= num3 && num2 <= num && num4 < 2.5f)
		{
			float num5 = this.CapsuleRadius * 2f + 0.1f;
			float d = 0.4f + num5 / 2f;
			float height = this.CapsuleHeight + 0.01f - num5 / 2f;
			this.chargeValidColliders.Clear();
			this.chargeValidColliders.Add(base.GetComponent<global::UnityEngine.Collider>());
			this.chargeValidColliders.Add(this.combatCircle.Collider);
			this.chargeValidColliders.Add(enemy.GetComponent<global::UnityEngine.Collider>());
			this.chargeValidColliders.Add(enemy.combatCircle.Collider);
			global::UnityEngine.Vector3 vector = base.transform.position;
			while (!flag2)
			{
				num2 = global::UnityEngine.Vector3.Distance(vector, enemy.transform.position);
				global::UnityEngine.Vector3 vector2 = enemy.transform.position - vector;
				vector2.Normalize();
				global::UnityEngine.Vector3 vector3 = vector + global::UnityEngine.Vector3.up * d;
				flag2 = global::PandoraUtils.RectCast(vector3, vector2, num2, height, num5, global::LayerMaskManager.chargeMask, this.chargeValidColliders, out this.raycastHitInfo, true);
				if (!flag2)
				{
					float y;
					float y2;
					if (enemy.transform.position.y > base.transform.position.y)
					{
						y = enemy.transform.position.y;
						y2 = base.transform.position.y;
					}
					else
					{
						y = base.transform.position.y;
						y2 = enemy.transform.position.y;
					}
					if (num4 < 0.1f || global::UnityEngine.Vector2.SqrMagnitude(new global::UnityEngine.Vector2(this.raycastHitInfo.point.x, this.raycastHitInfo.point.z) - new global::UnityEngine.Vector2(vector.x, vector.z)) < 0.1f || !global::PandoraUtils.IsBetween(this.raycastHitInfo.point.y, y2, y) || this.raycastHitInfo.collider == null || this.raycastHitInfo.collider.gameObject.layer == global::LayerMaskManager.charactersLayer || this.raycastHitInfo.collider.gameObject.layer == global::LayerMaskManager.engage_circlesLayer)
					{
						break;
					}
					vector = vector3 + vector2 * this.raycastHitInfo.distance;
				}
			}
		}
		return flag2;
	}

	public void SetFleeTarget()
	{
		this.FleeTarget = global::UnityEngine.Vector3.zero;
		for (int i = 0; i < this.EngagedUnits.Count; i++)
		{
			this.FleeTarget += this.EngagedUnits[i].transform.position;
		}
		this.FleeTarget /= (float)this.EngagedUnits.Count;
	}

	public bool CanDisengage()
	{
		this.SetFleeTarget();
		float num = 0.2f;
		float num2 = this.CapsuleHeight - num + 0.01f;
		float width = this.CapsuleRadius * 2f + num + 0.01f;
		global::UnityEngine.Vector3 position = base.transform.position;
		position.y = 0f;
		global::UnityEngine.Vector3 fleeTarget = this.FleeTarget;
		fleeTarget.y = 0f;
		global::UnityEngine.Vector3 normalized = (position - fleeTarget).normalized;
		return global::PandoraUtils.RectCast(base.transform.position + normalized * this.CapsuleRadius * 0.8f + global::UnityEngine.Vector3.up * (num2 / 2f + num), normalized, 1f, num2, width, global::LayerMaskManager.chargeMask, null, out this.raycastHitInfo, true);
	}

	public global::BoneId GetMirrorBone(global::BoneId bone)
	{
		return global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::BoneData>((int)bone).BoneIdMirror;
	}

	public void InitBoneTargets()
	{
		this.boneTargets = new global::System.Collections.Generic.List<global::BoneTarget>();
		if (this.unit.Status != global::UnitStateId.OUT_OF_ACTION)
		{
			global::UnityEngine.Transform transform = base.transform;
			global::System.Collections.Generic.List<global::BoneData> list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::BoneData>();
			for (int i = 0; i < list.Count; i++)
			{
				global::BoneData boneData = list[i];
				if (boneData.IsRange && !this.unit.IsBoneBlocked(boneData.Id))
				{
					global::UnityEngine.Transform transform2 = base.BonesTr[boneData.Id];
					global::UnityEngine.Vector3 vector = transform.InverseTransformPoint(transform2.TransformPoint(global::UnityEngine.Vector3.zero));
					vector.x = global::UnityEngine.Mathf.Clamp(vector.x, -this.CapsuleRadius + 0.1f, this.CapsuleRadius - 0.1f);
					vector.y = global::UnityEngine.Mathf.Clamp(vector.y, 0.1f, this.CapsuleHeight - 0.1f);
					vector.z = global::UnityEngine.Mathf.Clamp(vector.z, -this.CapsuleRadius + 0.1f, this.CapsuleRadius - 0.1f);
					if (vector.y < this.CapsuleRadius)
					{
						global::UnityEngine.Vector3 vector2 = new global::UnityEngine.Vector3(0f, this.CapsuleRadius, 0f);
						vector = vector2 + (vector - vector2).normalized * (this.CapsuleRadius - 0.1f);
					}
					else if (vector.y > this.CapsuleHeight - this.CapsuleRadius)
					{
						global::UnityEngine.Vector3 vector3 = new global::UnityEngine.Vector3(0f, this.CapsuleHeight - this.CapsuleRadius, 0f);
						vector = vector3 + (vector - vector3).normalized * (this.CapsuleRadius - 0.1f);
					}
					this.boneTargets.Add(new global::BoneTarget
					{
						bone = boneData.Id,
						position = vector,
						transform = transform2
					});
				}
			}
		}
	}

	public void OnDrawGizmos()
	{
		if (this.boneTargets != null && this.unit.Status != global::UnitStateId.OUT_OF_ACTION)
		{
			for (int i = 0; i < this.boneTargets.Count; i++)
			{
				global::UnityEngine.Gizmos.DrawWireSphere(base.transform.TransformPoint(this.boneTargets[i].position), 0.1f);
			}
		}
	}

	public void InitTargetsData()
	{
		global::System.Collections.Generic.List<global::UnitController> allUnits = global::PandoraSingleton<global::MissionManager>.Instance.GetAllUnits();
		this.targetsData.Clear();
		for (int i = 0; i < allUnits.Count; i++)
		{
			if (allUnits[i] != this)
			{
				this.targetsData.Add(new global::TargetData(allUnits[i]));
			}
		}
	}

	public void UpdateTargetsData()
	{
		global::UnityEngine.Vector3 raySrc = base.transform.position + global::UnityEngine.Vector3.up * 1.4f;
		float maxSqrDist = (float)(this.unit.ViewDistance * this.unit.ViewDistance);
		for (int i = 0; i < this.targetsData.Count; i++)
		{
			if (this.targetsData[i].unitCtrlr.unit.Status != global::UnitStateId.OUT_OF_ACTION)
			{
				this.UpdateTargetData(raySrc, maxSqrDist, this.targetsData[i]);
			}
		}
	}

	public void UpdateTargetData(global::UnitController target)
	{
		global::UnityEngine.Vector3 raySrc = base.transform.position + global::UnityEngine.Vector3.up * 1.4f;
		float maxSqrDist = (float)(this.unit.ViewDistance * this.unit.ViewDistance);
		for (int i = 0; i < this.targetsData.Count; i++)
		{
			if (this.targetsData[i].unitCtrlr == target)
			{
				this.UpdateTargetData(raySrc, maxSqrDist, this.targetsData[i]);
				return;
			}
		}
	}

	private void UpdateTargetData(global::UnityEngine.Vector3 raySrc, float maxSqrDist, global::TargetData targetData)
	{
		for (int i = 0; i < targetData.boneTargetRange.Count; i++)
		{
			global::UnityEngine.Vector3 vector = targetData.unitCtrlr.transform.TransformPoint(targetData.unitCtrlr.boneTargets[i].position);
			float num = global::UnityEngine.Vector3.SqrMagnitude(raySrc - vector);
			if (num <= maxSqrDist)
			{
				float dist = global::UnityEngine.Mathf.Sqrt(num);
				global::BoneTargetRange boneTargetRange = targetData.boneTargetRange[i];
				this.SendTargetRay(raySrc, vector, dist, global::LayerMaskManager.rangeTargetMaskNoChar, targetData.unitCtrlr, boneTargetRange);
				targetData.boneTargetRangeBlockingUnit[i].hitBone = boneTargetRange.hitBone;
				targetData.boneTargetRangeBlockingUnit[i].hitPoint = boneTargetRange.hitPoint;
				targetData.boneTargetRangeBlockingUnit[i].distance = boneTargetRange.distance;
				if (boneTargetRange.hitBone)
				{
					this.SendTargetRay(raySrc, vector, dist, global::LayerMaskManager.rangeTargetMask, targetData.unitCtrlr, targetData.boneTargetRangeBlockingUnit[i]);
				}
			}
			else
			{
				targetData.boneTargetRange[i].hitBone = false;
				targetData.boneTargetRangeBlockingUnit[i].hitBone = false;
			}
		}
	}

	private void SendTargetRay(global::UnityEngine.Vector3 raySrc, global::UnityEngine.Vector3 rayDst, float dist, global::UnityEngine.LayerMask mask, global::UnitController checkedUnit, global::BoneTargetRange boneTarget)
	{
		int num = global::UnityEngine.Physics.RaycastNonAlloc(raySrc, rayDst - raySrc, global::PandoraUtils.hits, dist, mask);
		boneTarget.hitBone = (num == 0 || global::PandoraUtils.hits[0].transform.gameObject == checkedUnit.gameObject);
		boneTarget.hitPoint = ((num != 0) ? global::PandoraUtils.hits[0].point : rayDst);
		boneTarget.distance = dist;
	}

	public bool IsInRange(global::UnitController target, float minDistance, float maxDistance, float requiredPerc, bool unitBlocking, bool checkAllBones, global::BoneId requiredBoneId)
	{
		if (target == this)
		{
			return true;
		}
		for (int i = 0; i < this.targetsData.Count; i++)
		{
			if (this.targetsData[i].unitCtrlr == target)
			{
				return this.IsInRange(this.targetsData[i], minDistance, maxDistance, requiredPerc, unitBlocking, checkAllBones, requiredBoneId);
			}
		}
		return false;
	}

	private bool IsInRange(global::TargetData targetData, float minDistance, float maxDistance, float requiredPerc, bool unitBlocking, bool checkAllBones, global::BoneId requiredBoneId)
	{
		bool flag = false;
		bool flag2 = true;
		global::System.Collections.Generic.List<global::BoneTargetRange> list = (!unitBlocking) ? targetData.boneTargetRange : targetData.boneTargetRangeBlockingUnit;
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i].hitBone)
			{
				if (list[i].distance >= minDistance && list[i].distance <= maxDistance)
				{
					if (targetData.unitCtrlr.boneTargets[i].bone == requiredBoneId)
					{
						flag = true;
					}
					else if (!checkAllBones)
					{
						return true;
					}
				}
				else
				{
					flag2 = false;
				}
			}
		}
		return (requiredBoneId == global::BoneId.NONE || flag) && flag2 && 1f - targetData.GetCover(unitBlocking) >= requiredPerc;
	}

	public global::TargetData GetTargetData(global::UnitController ctrlr)
	{
		for (int i = 0; i < this.targetsData.Count; i++)
		{
			if (this.targetsData[i].unitCtrlr == ctrlr)
			{
				return this.targetsData[i];
			}
		}
		return null;
	}

	public bool CanTargetFromPoint(global::TargetData targetData, float minDistance, float maxDistance, float requiredPerc, bool unitBlocking, bool checkAllBones, global::BoneId requiredBoneId = global::BoneId.NONE)
	{
		return this.CanTargetFromPoint(base.transform.position + global::UnityEngine.Vector3.up * 1.4f, targetData, minDistance, maxDistance, requiredPerc, unitBlocking, checkAllBones, requiredBoneId);
	}

	public bool CanTargetFromPoint(global::UnityEngine.Vector3 pos, global::TargetData targetData, float minDistance, float maxDistance, float requiredPerc, bool unitBlocking, bool checkAllBones, global::BoneId requiredBoneId = global::BoneId.NONE)
	{
		this.UpdateTargetData(pos, maxDistance * maxDistance, targetData);
		return this.IsInRange(targetData, minDistance, maxDistance, requiredPerc, unitBlocking, checkAllBones, requiredBoneId);
	}

	public bool HasEnemyInSight()
	{
		return this.HasEnemyInSight((float)this.unit.ViewDistance);
	}

	public bool HasEnemyInSight(float dist)
	{
		float @float = global::Constant.GetFloat(global::ConstantId.RANGE_SPELL_REQUIRED_PERC);
		global::System.Collections.Generic.List<global::UnitController> aliveEnemies = global::PandoraSingleton<global::MissionManager>.Instance.GetAliveEnemies(this.GetWarband().idx);
		for (int i = 0; i < aliveEnemies.Count; i++)
		{
			if (this.IsInRange(aliveEnemies[i], 0f, dist, @float, false, false, global::BoneId.NONE))
			{
				return true;
			}
		}
		return false;
	}

	public bool HasIdolInSight()
	{
		global::UnityEngine.Vector3 vector = base.transform.position + global::UnityEngine.Vector3.up * 1.4f;
		float num = (float)(this.unit.ViewDistance * this.unit.ViewDistance);
		int teamIdx = this.GetWarband().teamIdx;
		for (int i = 0; i < global::PandoraSingleton<global::MissionManager>.Instance.MapImprints.Count; i++)
		{
			global::MapImprint mapImprint = global::PandoraSingleton<global::MissionManager>.Instance.MapImprints[i];
			if (mapImprint.Destructible != null && mapImprint.Destructible.Owner != null && mapImprint.Destructible.Owner.GetWarband().teamIdx != teamIdx)
			{
				global::UnityEngine.Vector3 a = mapImprint.Destructible.transform.position + global::UnityEngine.Vector3.up;
				bool flag = global::UnityEngine.Physics.Raycast(vector, a - vector, out this.raycastHitInfo, (float)this.unit.ViewDistance, global::LayerMaskManager.groundMask);
				if (flag && this.raycastHitInfo.transform.parent != null && this.raycastHitInfo.transform.parent.gameObject == mapImprint.Destructible.gameObject)
				{
					return true;
				}
			}
		}
		return false;
	}

	public void InitDefense(global::UnitController atkr, bool face = true)
	{
		this.attackerCtrlr = atkr;
		this.unit.PreviousStatus = this.unit.Status;
		if (face && this.unit.IsAvailable())
		{
			this.FaceTarget(this.attackerCtrlr.transform, false);
		}
	}

	public void EndDefense()
	{
		this.TriggerEnchantments(global::EnchantmentTriggerId.ON_END_DEFENSE, global::SkillId.NONE, global::UnitActionId.NONE);
		this.unit.DestroyEnchantments(global::EnchantmentTriggerId.ON_END_DEFENSE, false);
		if (this.AICtrlr != null && this.attackerCtrlr != null)
		{
			this.GetWarband().BlackListWarband(this.attackerCtrlr.GetWarband().idx);
		}
		if (this.unit.Status == global::UnitStateId.STUNNED)
		{
			this.StateMachine.ChangeState(9);
		}
		if (this.unit.Status == global::UnitStateId.OUT_OF_ACTION)
		{
			global::PandoraDebug.LogDebug("EndDefense, This Unit is dead = " + this, "uncategorised", null);
			this.KillUnit();
		}
	}

	public void SetHitData(global::UnityEngine.Transform point, global::UnityEngine.Quaternion rot)
	{
		this.hitPoint = point;
	}

	public int GetFallHeight(global::UnitActionId unitActionId)
	{
		if (this.interactivePoint != null && unitActionId == global::UnitActionId.LEAP)
		{
			unitActionId = ((global::ActionZone)this.interactivePoint).GetJump().actionId;
		}
		int result = 0;
		switch (unitActionId)
		{
		case global::UnitActionId.CLIMB_3M:
		case global::UnitActionId.JUMP_3M:
			result = 3;
			break;
		case global::UnitActionId.CLIMB_6M:
		case global::UnitActionId.JUMP_6M:
			result = 6;
			break;
		case global::UnitActionId.CLIMB_9M:
		case global::UnitActionId.JUMP_9M:
			result = 9;
			break;
		}
		return result;
	}

	public void RemoveAthletics()
	{
		this.interactivePoint = null;
		this.activeActionDest = null;
		this.interactivePoints.Clear();
	}

	public void ResetAttackResult()
	{
		this.attackResultId = global::AttackResultId.NONE;
		this.lastActionWounds = 0;
		this.flyingLabel = string.Empty;
	}

	public int GetMeleeHitRoll(bool updateModifiers = false)
	{
		global::UnitController unitController = this.defenderCtrlr;
		if (unitController == null && this.EngagedUnits.Count > 0 && this.CurrentAction != null && this.CurrentAction.ActionId == global::UnitActionId.MELEE_ATTACK)
		{
			unitController = this.EngagedUnits[0];
		}
		else if (unitController == null && this.chargeTargets.Count > 0 && this.CurrentAction != null && this.CurrentAction.ActionId == global::UnitActionId.CHARGE)
		{
			unitController = this.chargeTargets[0];
		}
		else
		{
			if (unitController == null && this.destructibleTarget == null && this.CurrentAction != null && this.CurrentAction.Destructibles.Count > 0)
			{
				this.destructibleTarget = this.CurrentAction.Destructibles[0];
			}
			if (this.destructibleTarget != null)
			{
				return 100;
			}
		}
		if (unitController == null)
		{
			return 0;
		}
		if (updateModifiers)
		{
			this.CurrentRollModifiers.AddRange(this.unit.attributeModifiers.GetOrNull(global::AttributeId.COMBAT_MELEE_HIT_ROLL), null, false, false, false);
		}
		int num = this.unit.CombatMeleeHitRoll;
		if (unitController != null)
		{
			if (unitController.Fleeing)
			{
				num += global::Constant.GetInt(global::ConstantId.FLEE_MELEE_HIT_ROLL_MODIFIER);
			}
			else
			{
				num -= unitController.unit.MeleeResistance;
				if (updateModifiers)
				{
					this.CurrentRollModifiers.AddRange(unitController.unit.attributeModifiers.GetOrNull(global::AttributeId.MELEE_RESISTANCE), null, false, true, true);
				}
			}
		}
		return global::UnityEngine.Mathf.Clamp(num, 1, global::Constant.GetInt(global::ConstantId.MAX_ROLL));
	}

	public int GetRangeHitRoll(bool updateModifiers = false)
	{
		return this.GetRangeHitRoll(base.transform, updateModifiers);
	}

	public int GetRangeHitRoll(global::UnityEngine.Transform trans, bool updateModifiers = false)
	{
		if (this.Engaged)
		{
			return 0;
		}
		global::UnitController unitController = this.defenderCtrlr;
		if (unitController == null && this.CurrentAction != null && this.CurrentAction.Targets.Count > 0)
		{
			unitController = this.CurrentAction.Targets[0];
		}
		if (unitController == null && this.destructibleTarget == null && this.CurrentAction != null && this.CurrentAction.Destructibles.Count > 0)
		{
			this.destructibleTarget = this.CurrentAction.Destructibles[0];
		}
		if (this.destructibleTarget != null)
		{
			return 100;
		}
		if (unitController == null)
		{
			return 0;
		}
		if (updateModifiers)
		{
			this.CurrentRollModifiers.AddRange(this.unit.attributeModifiers.GetOrNull(global::AttributeId.COMBAT_RANGE_HIT_ROLL), null, false, false, false);
		}
		int num = this.unit.CombatRangeHitRoll;
		if (unitController != null)
		{
			if (unitController.unit.RangeResistance > 0)
			{
				num -= global::UnityEngine.Mathf.Max(unitController.unit.RangeResistance + this.unit.RangeResistanceDefenderModifier, 0);
				if (updateModifiers)
				{
					this.CurrentRollModifiers.AddRange(unitController.unit.attributeModifiers.GetOrNull(global::AttributeId.RANGE_RESISTANCE), null, false, true, true);
					this.CurrentRollModifiers.AddRange(this.unit.attributeModifiers.GetOrNull(global::AttributeId.RANGE_RESISTANCE_DEFENDER_MODIFIER), null, false, false, true);
				}
			}
			if (trans.position.y >= unitController.transform.position.y + 2.8f)
			{
				num += this.unit.GetAttribute(global::AttributeId.RANGE_HIT_ROLL_BONUS_HIGHER);
				if (updateModifiers)
				{
					this.CurrentRollModifiers.AddRange(this.unit.attributeModifiers.GetOrNull(global::AttributeId.RANGE_HIT_ROLL_BONUS_HIGHER), null, false, false, false);
				}
			}
			else if (trans.position.y <= unitController.transform.position.y - 2.8f)
			{
				num += this.unit.GetAttribute(global::AttributeId.RANGE_HIT_ROLL_BONUS_LOWER);
				if (updateModifiers)
				{
					this.CurrentRollModifiers.AddRange(this.unit.attributeModifiers.GetOrNull(global::AttributeId.RANGE_HIT_ROLL_BONUS_LOWER), null, false, false, false);
				}
			}
			if (unitController.unit.Status == global::UnitStateId.STUNNED)
			{
				num += this.unit.GetAttribute(global::AttributeId.RANGE_HIT_ROLL_BONUS_STUNNED);
				if (updateModifiers)
				{
					this.CurrentRollModifiers.AddRange(this.unit.attributeModifiers.GetOrNull(global::AttributeId.RANGE_HIT_ROLL_BONUS_STUNNED), null, false, false, false);
				}
			}
			if (unitController.Engaged)
			{
				num += this.unit.GetAttribute(global::AttributeId.RANGE_HIT_ROLL_BONUS_ENGAGED);
				if (updateModifiers)
				{
					this.CurrentRollModifiers.AddRange(this.unit.attributeModifiers.GetOrNull(global::AttributeId.RANGE_HIT_ROLL_BONUS_ENGAGED), null, false, false, false);
				}
			}
			global::TargetData targetData = this.GetTargetData(unitController);
			if (targetData != null)
			{
				float cover = targetData.GetCover(true);
				if ((double)cover > 0.5)
				{
					num += global::Constant.GetInt(global::ConstantId.RANGE_BONUS_COVER_HIGH);
					if (updateModifiers)
					{
						this.CurrentRollModifiers.Add(new global::AttributeMod(global::AttributeMod.Type.NONE, global::PandoraSingleton<global::LocalizationManager>.Instance.BuildStringAndLocalize("constant_", global::ConstantId.RANGE_BONUS_COVER_HIGH.ToString(), null, null), global::Constant.GetInt(global::ConstantId.RANGE_BONUS_COVER_HIGH), null, false, false), null, false, false, false);
					}
				}
				else if ((double)cover > 0.25)
				{
					num += global::Constant.GetInt(global::ConstantId.RANGE_BONUS_COVER_LOW);
					if (updateModifiers)
					{
						this.CurrentRollModifiers.Add(new global::AttributeMod(global::AttributeMod.Type.NONE, global::PandoraSingleton<global::LocalizationManager>.Instance.BuildStringAndLocalize("constant_", global::ConstantId.RANGE_BONUS_COVER_LOW.ToString(), null, null), global::Constant.GetInt(global::ConstantId.RANGE_BONUS_COVER_LOW), null, false, false), null, false, false, false);
					}
				}
			}
		}
		return global::UnityEngine.Mathf.Clamp(num, 1, global::Constant.GetInt(global::ConstantId.MAX_ROLL));
	}

	public int GetSpellCastingRoll(global::SpellTypeId typeId, bool updateModifiers = false)
	{
		int num = this.unit.SpellcastingRoll;
		if (updateModifiers)
		{
			this.CurrentRollModifiers.AddRange(this.unit.attributeModifiers.GetOrNull(global::AttributeId.SPELLCASTING_ROLL), null, false, false, false);
		}
		if (typeId != global::SpellTypeId.ARCANE)
		{
			if (typeId == global::SpellTypeId.DIVINE)
			{
				num += this.unit.DivineSpellcastingRoll;
				if (updateModifiers)
				{
					this.CurrentRollModifiers.AddRange(this.unit.attributeModifiers.GetOrNull(global::AttributeId.DIVINE_SPELLCASTING_ROLL), null, false, false, false);
				}
			}
		}
		else
		{
			num += this.unit.ArcaneSpellcastingRoll;
			if (updateModifiers)
			{
				this.CurrentRollModifiers.AddRange(this.unit.attributeModifiers.GetOrNull(global::AttributeId.ARCANE_SPELLCASTING_ROLL), null, false, false, false);
			}
		}
		return global::UnityEngine.Mathf.Clamp(num, 1, global::Constant.GetInt(global::ConstantId.MAX_ROLL));
	}

	public void ComputeWound()
	{
		this.defenderCtrlr.wasMaxWound = (this.defenderCtrlr.unit.CurrentWound == this.defenderCtrlr.unit.Wound);
		this.defenderCtrlr.unit.PreviousStatus = this.defenderCtrlr.unit.Status;
		int num = global::PandoraSingleton<global::MissionManager>.Instance.NetworkTyche.Rand(this.CurrentAction.GetMinDamage(false), this.CurrentAction.GetMaxDamage(false) + 1);
		int num2;
		global::AttributeId attributeId;
		if (base.HasClose())
		{
			num2 = this.unit.CriticalMeleeAttemptRoll;
			attributeId = global::AttributeId.CRITICAL_MELEE_ATTEMPT_ROLL;
		}
		else
		{
			num2 = this.unit.CriticalRangeAttemptRoll;
			attributeId = global::AttributeId.CRITICAL_RANGE_ATTEMPT_ROLL;
		}
		num2 += ((this.defenderCtrlr.unit.Status != global::UnitStateId.STUNNED) ? 0 : global::Constant.GetInt(global::ConstantId.CRIT_RANGE_BONUS_STUNNED));
		num2 -= this.defenderCtrlr.unit.CritResistance;
		this.criticalHit = this.unit.Roll(global::PandoraSingleton<global::MissionManager>.Instance.NetworkTyche, num2, attributeId, false, true, 0);
		this.defenderCtrlr.criticalHit = this.criticalHit;
		if (this.criticalHit)
		{
			global::PandoraDebug.LogDebug("Critical Hit!", "uncategorised", null);
			num = this.CurrentAction.GetMaxDamage(true);
			global::System.Collections.Generic.List<global::CriticalEffectData> datas = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::CriticalEffectData>();
			global::CriticalEffectData randomRatio = global::CriticalEffectData.GetRandomRatio(datas, global::PandoraSingleton<global::MissionManager>.Instance.NetworkTyche, null);
			if (this.defenderCtrlr.unit.Status < randomRatio.UnitStateId)
			{
				this.defenderCtrlr.unit.SetStatus(randomRatio.UnitStateId);
				if (this.IsPlayed() && !this.defenderCtrlr.IsPlayed() && this.defenderCtrlr.unit.Status == global::UnitStateId.STUNNED)
				{
					global::PandoraSingleton<global::Hephaestus>.Instance.IncrementStat(global::Hephaestus.StatId.STUN_ENEMIES, 1);
				}
			}
			if (randomRatio.ApplyDebuf)
			{
				this.defenderCtrlr.unit.AddEnchantment(global::EnchantmentId.OPEN_WOUND, this.unit, false, true, global::AllegianceId.NONE);
			}
			if (this.IsPlayed() && !this.defenderCtrlr.IsPlayed())
			{
				global::PandoraSingleton<global::Hephaestus>.Instance.IncrementStat(global::Hephaestus.StatId.CRITICALS, 1);
			}
		}
		global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<global::UnitController, int, bool>(global::Notices.RETROACTION_TARGET_DAMAGE, this.defenderCtrlr, num, this.criticalHit);
		if (num == 0)
		{
			this.attackResultId = global::AttackResultId.HIT_NO_WOUND;
			this.defenderCtrlr.attackResultId = global::AttackResultId.HIT_NO_WOUND;
			return;
		}
		this.attackResultId = global::AttackResultId.HIT;
		this.defenderCtrlr.attackResultId = global::AttackResultId.HIT;
		this.IncrementDamageDoneStats(this.defenderCtrlr.unit, num, this.criticalHit);
		global::PandoraSingleton<global::MissionManager>.Instance.CombatLogger.AddLog(this, (!this.criticalHit) ? global::CombatLogger.LogMessage.DAMAGE_INFLICT : global::CombatLogger.LogMessage.DAMAGE_CRIT_INFLICT, new string[]
		{
			this.GetLogName(),
			num.ToConstantString(),
			this.defenderCtrlr.GetLogName()
		});
		this.defenderCtrlr.unit.CurrentWound -= num;
		this.defenderCtrlr.lastActionWounds -= num;
		this.defenderCtrlr.flyingLabel = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("com_value", new string[]
		{
			this.defenderCtrlr.lastActionWounds.ToConstantString()
		});
		this.TriggerEnchantments(global::EnchantmentTriggerId.ON_DAMAGE, global::SkillId.NONE, global::UnitActionId.NONE);
		this.defenderCtrlr.TriggerEnchantments(global::EnchantmentTriggerId.ON_DAMAGE_RECEIVED, global::SkillId.NONE, global::UnitActionId.NONE);
		if (this.CurrentAction.ActionId == global::UnitActionId.MELEE_ATTACK || this.CurrentAction.ActionId == global::UnitActionId.CHARGE || this.CurrentAction.ActionId == global::UnitActionId.AMBUSH)
		{
			this.TriggerEnchantments(global::EnchantmentTriggerId.ON_MELEE_DAMAGE, global::SkillId.NONE, global::UnitActionId.NONE);
			this.TriggerEnchantments(global::EnchantmentTriggerId.ON_MELEE_DAMAGE_RANDOM, global::SkillId.NONE, global::UnitActionId.NONE);
			this.defenderCtrlr.TriggerEnchantments(global::EnchantmentTriggerId.ON_MELEE_DAMAGE_RECEIVED, global::SkillId.NONE, global::UnitActionId.NONE);
			if (this.CurrentAction.ActionId == global::UnitActionId.CHARGE || this.CurrentAction.ActionId == global::UnitActionId.AMBUSH)
			{
				this.TriggerEnchantments(global::EnchantmentTriggerId.ON_CHARGE_DAMAGE, global::SkillId.NONE, global::UnitActionId.NONE);
				this.defenderCtrlr.TriggerEnchantments(global::EnchantmentTriggerId.ON_CHARGE_DAMAGE_RECEIVED, global::SkillId.NONE, global::UnitActionId.NONE);
			}
		}
		if (this.CurrentAction.ActionId == global::UnitActionId.SHOOT || this.CurrentAction.ActionId == global::UnitActionId.AIM || this.CurrentAction.ActionId == global::UnitActionId.OVERWATCH)
		{
			this.TriggerEnchantments(global::EnchantmentTriggerId.ON_RANGE_DAMAGE, global::SkillId.NONE, global::UnitActionId.NONE);
			this.defenderCtrlr.TriggerEnchantments(global::EnchantmentTriggerId.ON_RANGE_DAMAGE_RECEIVED, global::SkillId.NONE, global::UnitActionId.NONE);
		}
		global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<global::Unit>(global::Notices.UNIT_ATTRIBUTES_CHANGED, this.defenderCtrlr.unit);
		this.defenderCtrlr.CheckOutOfAction(this);
		global::PandoraSingleton<global::MissionManager>.Instance.MissionEndData.UpdateUnit(this.defenderCtrlr);
		global::PandoraSingleton<global::MissionManager>.Instance.MissionEndData.UpdateUnit(this);
		global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<global::Unit>(global::Notices.UNIT_ATTRIBUTES_CHANGED, this.defenderCtrlr.unit);
	}

	public void ComputeDestructibleWound(global::Destructible dest)
	{
		int num = global::PandoraSingleton<global::MissionManager>.Instance.NetworkTyche.Rand(this.CurrentAction.GetMinDamage(false), this.CurrentAction.GetMaxDamage(false) + 1);
		global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<global::Destructible, int, bool>(global::Notices.RETROACTION_TARGET_DAMAGE, dest, num, false);
		this.attackResultId = global::AttackResultId.HIT;
		this.IncrementDamageDoneStats(num);
		global::PandoraSingleton<global::MissionManager>.Instance.CombatLogger.AddLog(this, global::CombatLogger.LogMessage.DAMAGE_INFLICT, new string[]
		{
			this.GetLogName(),
			num.ToConstantString(),
			global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(dest.Data.Name)
		});
		dest.ApplyDamage(num);
	}

	public void ComputeDirectWound(int damage, bool byPassArmor, global::UnitController damageDealer, bool fake = false)
	{
		this.wasMaxWound = (this.unit.CurrentWound == this.unit.Wound);
		this.criticalHit = false;
		if (this.defenderCtrlr != null)
		{
			this.defenderCtrlr.criticalHit = false;
		}
		this.unit.PreviousStatus = this.unit.Status;
		this.flyingLabel = string.Empty;
		if (!byPassArmor)
		{
			damage = global::UnityEngine.Mathf.Max(0, damage - this.unit.ArmorAbsorption);
		}
		global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<global::UnitController, int, bool>(global::Notices.RETROACTION_TARGET_DAMAGE, this, damage, this.criticalHit);
		if (damage == 0)
		{
			this.attackResultId = global::AttackResultId.HIT_NO_WOUND;
			return;
		}
		if (this.attackerCtrlr != null)
		{
			this.attackerCtrlr.IncrementDamageDoneStats(this.unit, damage, false);
			global::PandoraSingleton<global::MissionManager>.Instance.CombatLogger.AddLog(this, (damage <= 0) ? global::CombatLogger.LogMessage.HEALING_RECEIVED : global::CombatLogger.LogMessage.DAMAGE_INFLICT, new string[]
			{
				this.attackerCtrlr.GetLogName(),
				global::UnityEngine.Mathf.Abs(damage).ToConstantString(),
				this.GetLogName()
			});
		}
		else
		{
			global::PandoraSingleton<global::MissionManager>.Instance.CombatLogger.AddLog(this, (damage <= 0) ? global::CombatLogger.LogMessage.HEALING : global::CombatLogger.LogMessage.DAMAGE, new string[]
			{
				this.GetLogName(),
				global::UnityEngine.Mathf.Abs(damage).ToConstantString()
			});
		}
		if (!fake)
		{
			this.unit.CurrentWound -= damage;
		}
		this.lastActionWounds -= damage;
		this.attackResultId = global::AttackResultId.HIT;
		this.flyingLabel = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("com_value", new string[]
		{
			this.lastActionWounds.ToConstantString()
		});
		this.CheckOutOfAction(damageDealer);
		global::PandoraSingleton<global::MissionManager>.Instance.MissionEndData.UpdateUnit(damageDealer);
		global::PandoraSingleton<global::MissionManager>.Instance.MissionEndData.UpdateUnit(this);
		global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<global::Unit>(global::Notices.UNIT_ATTRIBUTES_CHANGED, this.unit);
	}

	public void CheckOutOfAction(global::UnitController damageDealer)
	{
		if (this.unit.CurrentWound <= 0)
		{
			global::PandoraDebug.LogDebug("Unit is Dead! current Wound < 0", "DEATH", this);
			if (damageDealer != null && damageDealer.IsPlayed() && !this.IsPlayed())
			{
				if (this.unit.Status == global::UnitStateId.STUNNED)
				{
					global::PandoraSingleton<global::Hephaestus>.Instance.IncrementStat(global::Hephaestus.StatId.STUNNED_OOAS, 1);
				}
				if (this.unit.GetUnitTypeId() == global::UnitTypeId.IMPRESSIVE)
				{
					global::PandoraSingleton<global::Hephaestus>.Instance.IncrementStat(global::Hephaestus.StatId.IMPRESSIVE_OOAS, 1);
				}
				global::PandoraSingleton<global::Hephaestus>.Instance.IncrementStat(global::Hephaestus.StatId.ENEMIES_OOA, 1);
			}
			if (this.IsPlayed())
			{
				global::PandoraSingleton<global::Hephaestus>.Instance.IncrementStat(global::Hephaestus.StatId.MY_TOTAL_OOA, 1);
				if (this.wasMaxWound)
				{
					global::PandoraSingleton<global::Hephaestus>.Instance.UnlockAchievement(global::Hephaestus.TrophyId.ONE_SHOT);
				}
			}
			this.unit.SetStatus(global::UnitStateId.OUT_OF_ACTION);
			this.unit.AddEnchantment(global::EnchantmentId.INJURY_ROLL, this.unit, false, true, global::AllegianceId.NONE);
		}
	}

	public void ResurectUnit()
	{
		this.Resurected = true;
		base.GetComponent<global::UnityEngine.Collider>().enabled = true;
		this.Imprint.alive = true;
		this.Imprint.alwaysHide = false;
		this.Imprint.alwaysVisible = false;
		this.killed = false;
		this.unit.Resurect();
		this.animator.Play(global::AnimatorIds.idle);
		this.StartGameInitialization();
		global::PandoraSingleton<global::MissionManager>.Instance.ForceUnitVisibilityCheck(this);
		this.combatCircle.gameObject.SetActive(true);
		this.SetCombatCircle(global::PandoraSingleton<global::MissionManager>.Instance.GetCurrentUnit(), true);
		this.StateMachine.ChangeState(9);
		global::PandoraSingleton<global::MissionManager>.Instance.resendLadder = true;
		global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<string, string>(global::Notices.MISSION_UNIT_SPAWN, "mission_unit_resurection", this.unit.Name);
	}

	public void KillUnit()
	{
		if (this.killed)
		{
			return;
		}
		this.killed = true;
		this.unit.CleanEnchantments();
		base.GetComponent<global::UnityEngine.Collider>().enabled = false;
		this.GetWarband().MoralValue -= this.unit.MoralImpact;
		if (this.attackerCtrlr != null)
		{
			global::PandoraDebug.LogDebug("Increment Kill Stat!", "DEATH", this);
			this.attackerCtrlr.IncrementKillStat(this.unit);
		}
		this.Imprint.alive = false;
		global::System.Collections.Generic.List<global::Item> list = new global::System.Collections.Generic.List<global::Item>();
		if (this.unit.RaceId != global::RaceId.DAEMON && !this.unit.NoLootBag)
		{
			if (this.unit.deathTrophy.Id != global::ItemId.NONE)
			{
				list.Add(this.unit.deathTrophy);
				this.unit.deathTrophy.Save.ownerMyrtilus = this.uid;
			}
			for (int i = this.unit.Items.Count - 1; i >= 0; i--)
			{
				global::UnitSlotId unitSlotId = (global::UnitSlotId)i;
				if (unitSlotId != global::UnitSlotId.ARMOR && unitSlotId != global::UnitSlotId.HELMET && ((unitSlotId == global::UnitSlotId.SET1_OFFHAND && !this.unit.Items[i - 1].IsPaired) || unitSlotId != global::UnitSlotId.SET1_OFFHAND) && ((unitSlotId == global::UnitSlotId.SET2_MAINHAND && !this.unit.Items[i - 2].IsLockSlot) || unitSlotId != global::UnitSlotId.SET2_MAINHAND) && ((unitSlotId == global::UnitSlotId.SET2_MAINHAND && this.unit.GetMutationId(unitSlotId) == global::MutationId.NONE) || unitSlotId != global::UnitSlotId.SET2_MAINHAND) && ((unitSlotId == global::UnitSlotId.SET2_OFFHAND && this.unit.GetMutationId(unitSlotId) == global::MutationId.NONE) || unitSlotId != global::UnitSlotId.SET2_OFFHAND) && ((unitSlotId == global::UnitSlotId.SET2_OFFHAND && !this.unit.Items[i - 2].IsLockSlot) || unitSlotId != global::UnitSlotId.SET2_OFFHAND) && ((unitSlotId == global::UnitSlotId.SET2_OFFHAND && !this.unit.Items[i - 1].IsPaired) || unitSlotId != global::UnitSlotId.SET2_OFFHAND))
				{
					global::System.Collections.Generic.List<global::Item> list2 = this.unit.EquipItem((global::UnitSlotId)i, new global::Item(global::ItemId.NONE, global::ItemQualityId.NORMAL), false);
					for (int j = 0; j < list2.Count; j++)
					{
						global::Item item = list2[j];
						for (int k = 0; k < global::PandoraSingleton<global::MissionManager>.Instance.WarbandCtrlrs.Count; k++)
						{
							if (global::PandoraSingleton<global::MissionManager>.Instance.WarbandCtrlrs[k].ItemIdol == item)
							{
								global::PandoraSingleton<global::MissionManager>.Instance.WarbandCtrlrs[k].RemoveMoralIdol();
							}
						}
						if (item.Id != global::ItemId.NONE)
						{
							if (!item.IsTrophy)
							{
								item.owner = this.unit;
							}
							item.Save.oldSlot = (int)unitSlotId;
							list.Add(item);
						}
					}
				}
			}
		}
		if (list.Count > 0)
		{
			this.lootBagPoint = global::PandoraSingleton<global::MissionManager>.Instance.SpawnLootBag(this, base.transform.position, list, false, false);
		}
		global::System.Collections.Generic.List<global::UnitController> list3 = new global::System.Collections.Generic.List<global::UnitController>();
		list3.AddRange(this.EngagedUnits);
		this.SetGraphWalkability(true);
		this.StateMachine.ChangeState(9);
		global::PandoraSingleton<global::MissionManager>.Instance.resendLadder = true;
		if (this.unit.Data.ZoneAoeIdDeathSpawn != global::ZoneAoeId.NONE)
		{
			global::ZoneAoe.Spawn(this.unit.Data.ZoneAoeIdDeathSpawn, 4f, base.transform.position, this, true, null);
		}
		if (this.unit.Data.UnitIdDeathSpawn != global::UnitId.NONE)
		{
			global::WarbandController warband = this.GetWarband();
			int l = 0;
			while (l < this.unit.Data.DeathSpawnCount)
			{
				global::UnitController unitController = global::PandoraSingleton<global::MissionManager>.Instance.excludedUnits.Find((global::UnitController x) => x.unit.Id == this.unit.Data.UnitIdDeathSpawn);
				if (unitController != null)
				{
					global::UnityEngine.Vector3 pos = base.transform.position;
					global::UnityEngine.Quaternion rotation = base.transform.rotation;
					bool flag = true;
					if (l > 0)
					{
						float num = global::Constant.GetFloat(global::ConstantId.MELEE_RANGE_LARGE) + 0.1f;
						float num2 = unitController.CapsuleRadius + 0.1f;
						float num3 = 6.28318548f * num;
						float num4 = num3 / num2;
						float num5 = 360f / num4;
						float num6 = 0f;
						bool flag2 = false;
						while (num6 < 360f && !flag2)
						{
							global::UnityEngine.Vector3 vector = global::UnityEngine.Vector3.forward;
							vector = global::UnityEngine.Quaternion.Euler(0f, num6, 0f) * vector;
							vector.Normalize();
							global::UnityEngine.Vector3 vector2 = base.transform.position + vector * num;
							global::UnityEngine.Vector2 vector3 = new global::UnityEngine.Vector2(vector2.x, vector2.z);
							if (!global::PandoraUtils.SendCapsule(base.transform.position, vector, num2 + 0.1f, 1.5f, num, num2))
							{
								bool flag3 = true;
								global::UnitController currentUnit = global::PandoraSingleton<global::MissionManager>.Instance.GetCurrentUnit();
								global::System.Collections.Generic.List<global::UnitController> allAliveUnits = global::PandoraSingleton<global::MissionManager>.Instance.GetAllAliveUnits();
								for (int m = 0; m < allAliveUnits.Count; m++)
								{
									global::UnityEngine.Vector2 a = new global::UnityEngine.Vector2(allAliveUnits[m].transform.position.x, allAliveUnits[m].transform.position.z);
									float d = 100f;
									global::UnityEngine.Vector2 checkDestPoint = vector3 + (a - vector3) * d;
									if ((allAliveUnits[m] != currentUnit && global::PandoraUtils.IsPointInsideEdges(allAliveUnits[m].combatCircle.Edges, vector3, checkDestPoint, -1f)) || (allAliveUnits[m] == currentUnit && global::UnityEngine.Vector3.SqrMagnitude(vector2 - currentUnit.transform.position) < num * num))
									{
										flag3 = false;
										break;
									}
								}
								if (flag3)
								{
									pos = vector2;
									flag2 = true;
									break;
								}
							}
							num6 += num5;
						}
						if (!flag2)
						{
							global::System.Collections.Generic.List<global::DecisionPoint> decisionPoints = global::PandoraSingleton<global::MissionManager>.Instance.GetDecisionPoints(this, global::DecisionPointId.SPAWN, 9999f, true);
							float num7 = float.MaxValue;
							global::DecisionPoint decisionPoint = null;
							for (int n = 0; n < decisionPoints.Count; n++)
							{
								float num8 = global::UnityEngine.Vector3.SqrMagnitude(base.transform.position - decisionPoints[n].transform.position);
								if (num8 < num7)
								{
									num7 = num8;
									decisionPoint = decisionPoints[n];
								}
							}
							flag = (decisionPoint != null);
							if (flag)
							{
								pos = decisionPoint.transform.position;
							}
						}
					}
					if (flag)
					{
						global::PandoraSingleton<global::MissionManager>.Instance.IncludeUnit(unitController, pos, rotation);
						global::PandoraSingleton<global::MissionManager>.Instance.ForceUnitVisibilityCheck(unitController);
					}
					l++;
				}
				else
				{
					l = this.unit.Data.DeathSpawnCount;
				}
			}
		}
		this.combatCircle.gameObject.SetActive(false);
		if (this.linkedSearchPoints != null && this.unlockSearchPointOnDeath)
		{
			this.unlockSearchPointOnDeath = false;
			for (int num9 = 0; num9 < this.linkedSearchPoints.Count; num9++)
			{
				this.linkedSearchPoints[num9].gameObject.SetActive(true);
			}
		}
		if (this.unit.CampaignData != null && this.unit.CampaignData.CampaignUnitIdSpawnOnDeath != global::CampaignUnitId.NONE && !this.spawnedOnDeath)
		{
			this.spawnedOnDeath = true;
			global::PandoraSingleton<global::MissionManager>.Instance.ActivateHiddenUnit(this.unit.CampaignData.CampaignUnitIdSpawnOnDeath, true, "mission_unit_resurection");
		}
		if (this.reviveUntilSearchEmpty)
		{
			bool flag4 = true;
			int num10 = 0;
			while (num10 < this.linkedSearchPoints.Count && flag4)
			{
				flag4 &= this.linkedSearchPoints[num10].IsEmpty();
				num10++;
			}
			if (!flag4)
			{
				global::System.Collections.Generic.List<global::DecisionPoint> availableSpawnPoints = global::PandoraSingleton<global::MissionManager>.Instance.GetAvailableSpawnPoints(false, true, null, this.forcedSpawnPoints);
				int index = global::PandoraSingleton<global::MissionManager>.Instance.NetworkTyche.Rand(0, availableSpawnPoints.Count);
				base.transform.position = availableSpawnPoints[index].transform.position;
				base.transform.rotation = availableSpawnPoints[index].transform.rotation;
				this.ResurectUnit();
				list3.Add(this);
			}
		}
		this.EngagedUnits.Clear();
		for (int num11 = 0; num11 < list3.Count; num11++)
		{
			global::PandoraDebug.LogDebug("Check Engage for " + list3[num11].gameObject.name, "DEATH", this);
			list3[num11].CheckEngaged(true);
		}
		global::PandoraSingleton<global::MissionManager>.Instance.CheckEndGame();
	}

	public void IncrementDamageDoneStats(global::Unit victim, int damage, bool critical)
	{
		damage = global::UnityEngine.Mathf.Min(damage, victim.CurrentWound);
		this.unit.AddToAttribute(global::AttributeId.TOTAL_DAMAGE, damage);
		if (this.unit.warbandIdx == victim.warbandIdx)
		{
			this.AddMvuPoint(global::ConstantId.MVU_DMG_FRIENDLY, global::MvuCategory.FRIENDLY_DAMAGE, damage);
		}
		else
		{
			this.AddMvuPoint(global::ConstantId.MVU_DMG_ENEMY, global::MvuCategory.ENEMY_DAMAGE, damage);
		}
	}

	public void IncrementDamageDoneStats(int damage)
	{
		this.AddMvuPoint(global::ConstantId.MVU_DMG_ENEMY, global::MvuCategory.ENEMY_DAMAGE, damage);
	}

	public void IncrementKillStat(global::Unit victim)
	{
		if (this.unit.GetUnitTypeId() == global::UnitTypeId.MONSTER || this.unit.GetUnitTypeId() == global::UnitTypeId.DRAMATIS)
		{
			return;
		}
		switch (victim.GetUnitTypeId())
		{
		case global::UnitTypeId.LEADER:
			if (this.unit.warbandIdx == victim.warbandIdx)
			{
				this.AddMvuPoint(global::ConstantId.MVU_KILL_FRIENDLY_LEADER, global::MvuCategory.FRIENDLY_OOA, 1);
			}
			else
			{
				this.AddMvuPoint(global::ConstantId.MVU_KILL_ENEMY_LEADER, global::MvuCategory.ENEMY_OOA, 1);
			}
			break;
		case global::UnitTypeId.HENCHMEN:
			if (this.unit.warbandIdx == victim.warbandIdx)
			{
				this.AddMvuPoint(global::ConstantId.MVU_KILL_FRIENDLY_HENCHMEN, global::MvuCategory.FRIENDLY_OOA, 1);
			}
			else
			{
				this.AddMvuPoint(global::ConstantId.MVU_KILL_ENEMY_HENCHMEN, global::MvuCategory.ENEMY_OOA, 1);
			}
			break;
		case global::UnitTypeId.IMPRESSIVE:
		case global::UnitTypeId.DRAMATIS:
			if (this.unit.warbandIdx == victim.warbandIdx)
			{
				this.AddMvuPoint(global::ConstantId.MVU_KILL_FRIENDLY_IMPRESSIVE, global::MvuCategory.FRIENDLY_OOA, 1);
			}
			if (victim.CampaignData == null && victim.Id == global::UnitId.CHAOS_OGRE)
			{
				this.unit.AddToAttribute(global::AttributeId.TOTAL_KILL_ROAMING, 1);
			}
			else
			{
				this.AddMvuPoint(global::ConstantId.MVU_KILL_ENEMY_IMPRESSIVE, global::MvuCategory.ENEMY_OOA, 1);
			}
			break;
		case global::UnitTypeId.MONSTER:
			this.AddMvuPoint(global::ConstantId.MVU_KILL_MONSTER, global::MvuCategory.ENEMY_OOA, 1);
			if (victim.CampaignData == null && victim.Id != global::UnitId.BLUE_HORROR)
			{
				this.unit.AddToAttribute(global::AttributeId.TOTAL_KILL_ROAMING, 1);
			}
			break;
		case global::UnitTypeId.HERO_1:
		case global::UnitTypeId.HERO_2:
		case global::UnitTypeId.HERO_3:
			if (this.unit.warbandIdx == victim.warbandIdx)
			{
				this.AddMvuPoint(global::ConstantId.MVU_KILL_FRIENDLY_HERO, global::MvuCategory.FRIENDLY_OOA, 1);
			}
			else
			{
				this.AddMvuPoint(global::ConstantId.MVU_KILL_ENEMY_HERO, global::MvuCategory.ENEMY_OOA, 1);
			}
			break;
		}
		this.unit.AddToAttribute(global::AttributeId.TOTAL_KILL, 1);
	}

	public void AddMvuPoint(global::ConstantId mvuConstant, global::MvuCategory category, int nb = 1)
	{
		int num = global::Constant.GetInt(mvuConstant) * nb;
		this.unit.AddToAttribute(global::AttributeId.CURRENT_MVU, num);
		this.unit.AddToAttribute(global::AttributeId.TOTAL_MVU, num);
		this.MVUptsPerCategory[(int)category] += num;
	}

	public bool IsTargeting()
	{
		return this.IsCurrentState(global::UnitController.State.SINGLE_TARGETING) || this.IsCurrentState(global::UnitController.State.AOE_TARGETING) || this.IsCurrentState(global::UnitController.State.CONE_TARGETING) || this.IsCurrentState(global::UnitController.State.LINE_TARGETING) || this.IsCurrentState(global::UnitController.State.COUNTER_CHOICE) || this.IsCurrentState(global::UnitController.State.INTERACTIVE_TARGET);
	}

	public bool IsChoosingTarget()
	{
		return this.IsCurrentState(global::UnitController.State.SINGLE_TARGETING) || this.IsCurrentState(global::UnitController.State.INTERACTIVE_TARGET);
	}

	public bool CanCounterAttack()
	{
		global::ActionStatus action = this.GetAction(global::SkillId.BASE_COUNTER_ATTACK);
		action.UpdateAvailable();
		return this.unit.IsAvailable() && !this.Fleeing && action.Available && this.unit.CounterDisabled == 0;
	}

	public bool IsBounty()
	{
		global::WarbandController warband = this.GetWarband();
		if (warband != null)
		{
			int idx = warband.idx;
			for (int i = 0; i < global::PandoraSingleton<global::MissionManager>.Instance.WarbandCtrlrs.Count; i++)
			{
				global::WarbandController warbandController = global::PandoraSingleton<global::MissionManager>.Instance.WarbandCtrlrs[i];
				if (warbandController.idx != idx)
				{
					for (int j = 0; j < warbandController.objectives.Count; j++)
					{
						if (warbandController.objectives[j].TypeId == global::PrimaryObjectiveTypeId.BOUNTY && ((global::ObjectiveBounty)warbandController.objectives[j]).IsUnitBounty(this))
						{
							return true;
						}
					}
				}
			}
		}
		return false;
	}

	public void RegisterItems()
	{
		this.oldItems.Clear();
		for (int i = 6; i < this.unit.Items.Count; i++)
		{
			this.oldItems.Add(this.unit.Items[i]);
		}
	}

	public void GetAlliesEnemies(out global::System.Collections.Generic.List<global::UnitController> allies, out global::System.Collections.Generic.List<global::UnitController> enemies)
	{
		this.SeparateAlliesEnemies(global::PandoraSingleton<global::MissionManager>.Instance.GetAllUnits(), out allies, out enemies);
	}

	public void GetDefendersAlliesEnemies(out global::System.Collections.Generic.List<global::UnitController> allies, out global::System.Collections.Generic.List<global::UnitController> enemies)
	{
		this.SeparateAlliesEnemies(this.defenders, out allies, out enemies);
	}

	private void SeparateAlliesEnemies(global::System.Collections.Generic.List<global::UnitController> refList, out global::System.Collections.Generic.List<global::UnitController> allies, out global::System.Collections.Generic.List<global::UnitController> enemies)
	{
		allies = new global::System.Collections.Generic.List<global::UnitController>();
		enemies = new global::System.Collections.Generic.List<global::UnitController>();
		int teamIdx = this.GetWarband().teamIdx;
		for (int i = 0; i < refList.Count; i++)
		{
			if (refList[i] != this)
			{
				if (refList[i].GetWarband().teamIdx == teamIdx)
				{
					allies.Add(refList[i]);
				}
				else
				{
					enemies.Add(refList[i]);
				}
			}
		}
	}

	public void TriggerEnchantments(global::EnchantmentTriggerId triggerId, global::SkillId skillId = global::SkillId.NONE, global::UnitActionId actionId = global::UnitActionId.NONE)
	{
		global::System.Collections.Generic.List<global::UnitController> list = new global::System.Collections.Generic.List<global::UnitController>();
		global::System.Collections.Generic.List<global::UnitController> list2;
		global::System.Collections.Generic.List<global::UnitController> list3;
		this.GetDefendersAlliesEnemies(out list2, out list3);
		string text = ((int)triggerId).ToConstantString();
		global::System.Collections.Generic.List<global::SkillEnchantmentData> list4 = new global::System.Collections.Generic.List<global::SkillEnchantmentData>();
		if (this.CurrentAction != null)
		{
			list4.AddRange(global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::SkillEnchantmentData>(new string[]
			{
				"fk_skill_id",
				"fk_enchantment_trigger_id"
			}, new string[]
			{
				((int)this.CurrentAction.SkillId).ToConstantString(),
				text
			}));
			if ((triggerId == global::EnchantmentTriggerId.ON_SPELL_IMPACT_RANDOM || triggerId == global::EnchantmentTriggerId.ON_SKILL_IMPACT_RANDOM || triggerId == global::EnchantmentTriggerId.ON_MELEE_DAMAGE_RANDOM) && list4.Count > 1)
			{
				int index = global::PandoraSingleton<global::MissionManager>.Instance.NetworkTyche.Rand(0, list4.Count);
				global::SkillEnchantmentData item = list4[index];
				list4.Clear();
				list4.Add(item);
			}
		}
		for (int i = 0; i < this.unit.PassiveSkills.Count; i++)
		{
			global::System.Collections.Generic.List<global::SkillEnchantmentData> list5 = new global::System.Collections.Generic.List<global::SkillEnchantmentData>(global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::SkillEnchantmentData>(new string[]
			{
				"fk_skill_id",
				"fk_enchantment_trigger_id"
			}, new string[]
			{
				((int)this.unit.PassiveSkills[i].Id).ToConstantString(),
				text
			}));
			if ((triggerId == global::EnchantmentTriggerId.ON_SPELL_IMPACT_RANDOM || triggerId == global::EnchantmentTriggerId.ON_SKILL_IMPACT_RANDOM || triggerId == global::EnchantmentTriggerId.ON_MELEE_DAMAGE_RANDOM) && list5.Count > 1)
			{
				int index2 = global::PandoraSingleton<global::MissionManager>.Instance.NetworkTyche.Rand(0, list5.Count);
				global::SkillEnchantmentData item2 = list5[index2];
				list5.Clear();
				list5.Add(item2);
			}
			list4.AddRange(list5);
		}
		for (int j = 0; j < list4.Count; j++)
		{
			global::SkillEnchantmentData skillEnchantmentData = list4[j];
			global::SkillData skillData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::SkillData>((int)skillEnchantmentData.SkillId);
			bool flag = true;
			if (skillEnchantmentData.Ratio != 0)
			{
				flag = (global::PandoraSingleton<global::MissionManager>.Instance.NetworkTyche.Rand(0, 100) < skillEnchantmentData.Ratio);
			}
			if (flag && skillEnchantmentData.EnchantmentTriggerId == triggerId && (skillEnchantmentData.EnchantmentTriggerId != global::EnchantmentTriggerId.NONE || !skillData.Passive) && ((skillEnchantmentData.EnchantmentTriggerId != global::EnchantmentTriggerId.ON_ACTION && skillEnchantmentData.EnchantmentTriggerId != global::EnchantmentTriggerId.ON_POST_ACTION && skillEnchantmentData.EnchantmentTriggerId != global::EnchantmentTriggerId.ON_ATHLETIC_SUCCESS_ENGAGED) || (skillEnchantmentData.UnitActionIdTrigger == global::UnitActionId.NONE && skillEnchantmentData.SkillIdTrigger == global::SkillId.NONE) || skillEnchantmentData.UnitActionIdTrigger == actionId || skillEnchantmentData.SkillIdTrigger == skillId))
			{
				if (skillEnchantmentData.Self)
				{
					this.AddTriggeredEnchantment(this, this.unit, skillEnchantmentData.EnchantmentId, ref list);
				}
				if (skillEnchantmentData.TargetSelf && this.defenders.IndexOf(this) != -1)
				{
					this.AddTriggeredEnchantment(this, this.unit, skillEnchantmentData.EnchantmentId, ref list);
				}
				if (skillEnchantmentData.TargetAlly && list2 != null)
				{
					for (int k = 0; k < list2.Count; k++)
					{
						this.AddTriggeredEnchantment(list2[k], this.unit, skillEnchantmentData.EnchantmentId, ref list);
					}
				}
				if (skillEnchantmentData.TargetEnemy && list3 != null)
				{
					for (int l = 0; l < list3.Count; l++)
					{
						this.AddTriggeredEnchantment(list3[l], this.unit, skillEnchantmentData.EnchantmentId, ref list);
					}
				}
			}
		}
		this.AddEnchantEffects(this.unit.Enchantments, this.defenders, list2, list3, triggerId, actionId, skillId, ref list);
		for (int m = 0; m < 6; m++)
		{
			if (m != (int)this.unit.InactiveWeaponSlot && m != (int)(this.unit.InactiveWeaponSlot + 1))
			{
				this.AddEnchantEffects(this.unit.Items[m].Enchantments, this.defenders, list2, list3, triggerId, actionId, skillId, ref list);
				if (this.unit.Items[m].RuneMark != null)
				{
					this.AddEnchantEffects(this.unit.Items[m].RuneMark.Enchantments, this.defenders, list2, list3, triggerId, actionId, skillId, ref list);
				}
			}
		}
		for (int n = 0; n < this.unit.Injuries.Count; n++)
		{
			this.AddEnchantEffects(this.unit.Injuries[n].Enchantments, this.defenders, list2, list3, triggerId, actionId, skillId, ref list);
		}
		for (int num = 0; num < this.unit.Mutations.Count; num++)
		{
			this.AddEnchantEffects(this.unit.Mutations[num].Enchantments, this.defenders, list2, list3, triggerId, actionId, skillId, ref list);
		}
		for (int num2 = 0; num2 < list.Count; num2++)
		{
			list[num2].unit.UpdateAttributes();
		}
	}

	private void AddEnchantEffects(global::System.Collections.Generic.List<global::Enchantment> enchants, global::System.Collections.Generic.List<global::UnitController> defenders, global::System.Collections.Generic.List<global::UnitController> allies, global::System.Collections.Generic.List<global::UnitController> enemies, global::EnchantmentTriggerId triggerId, global::UnitActionId actionId, global::SkillId skillId, ref global::System.Collections.Generic.List<global::UnitController> updatedUnits)
	{
		for (int i = enchants.Count - 1; i >= 0; i--)
		{
			for (int j = 0; j < enchants[i].Effects.Count; j++)
			{
				global::EnchantmentEffectEnchantmentData enchantmentEffectEnchantmentData = enchants[i].Effects[j];
				if (enchantmentEffectEnchantmentData.EnchantmentTriggerId == triggerId && ((enchantmentEffectEnchantmentData.EnchantmentTriggerId != global::EnchantmentTriggerId.ON_ACTION && enchantmentEffectEnchantmentData.EnchantmentTriggerId != global::EnchantmentTriggerId.ON_POST_ACTION && enchantmentEffectEnchantmentData.EnchantmentTriggerId != global::EnchantmentTriggerId.ON_ATHLETIC_SUCCESS_ENGAGED) || (enchantmentEffectEnchantmentData.UnitActionIdTrigger == global::UnitActionId.NONE && enchantmentEffectEnchantmentData.SkillIdTrigger == global::SkillId.NONE) || enchantmentEffectEnchantmentData.UnitActionIdTrigger == actionId || enchantmentEffectEnchantmentData.SkillIdTrigger == skillId))
				{
					bool flag = true;
					if (enchantmentEffectEnchantmentData.Ratio != 0)
					{
						flag = (global::PandoraSingleton<global::MissionManager>.Instance.NetworkTyche.Rand(0, 100) < enchantmentEffectEnchantmentData.Ratio);
					}
					if (flag)
					{
						global::AttributeId attributeModifierId = this.unit.GetAttributeModifierId(enchantmentEffectEnchantmentData.AttributeIdRoll);
						if (enchantmentEffectEnchantmentData.Self)
						{
							this.AddTriggeredEnchantment(this, this.unit, enchantmentEffectEnchantmentData.EnchantmentIdEffect, enchantmentEffectEnchantmentData.AttributeIdRoll, attributeModifierId, enchants[i].AllegianceId, ref updatedUnits);
						}
						if (enchantmentEffectEnchantmentData.TargetSelf && defenders.IndexOf(this) != -1)
						{
							this.AddTriggeredEnchantment(this, this.unit, enchantmentEffectEnchantmentData.EnchantmentIdEffect, enchantmentEffectEnchantmentData.AttributeIdRoll, attributeModifierId, enchants[i].AllegianceId, ref updatedUnits);
						}
						if (enchantmentEffectEnchantmentData.TargetAlly && allies != null)
						{
							for (int k = 0; k < allies.Count; k++)
							{
								this.AddTriggeredEnchantment(allies[k], this.unit, enchantmentEffectEnchantmentData.EnchantmentIdEffect, enchantmentEffectEnchantmentData.AttributeIdRoll, attributeModifierId, enchants[i].AllegianceId, ref updatedUnits);
							}
						}
						if (enchantmentEffectEnchantmentData.TargetEnemy && enemies != null)
						{
							for (int l = 0; l < enemies.Count; l++)
							{
								this.AddTriggeredEnchantment(enemies[l], this.unit, enchantmentEffectEnchantmentData.EnchantmentIdEffect, enchantmentEffectEnchantmentData.AttributeIdRoll, attributeModifierId, enchants[i].AllegianceId, ref updatedUnits);
							}
						}
					}
				}
			}
		}
	}

	public void AddTriggeredEnchantment(global::UnitController target, global::Unit provider, global::EnchantmentId effectId, ref global::System.Collections.Generic.List<global::UnitController> updatedUnits)
	{
		this.AddTriggeredEnchantment(target, provider, effectId, global::AttributeId.NONE, global::AttributeId.NONE, global::AllegianceId.NONE, ref updatedUnits);
	}

	public void AddTriggeredEnchantment(global::UnitController target, global::Unit provider, global::EnchantmentId effectId, global::AttributeId rollId, global::AttributeId rollModifierId, global::AllegianceId allegianceId, ref global::System.Collections.Generic.List<global::UnitController> updatedUnits)
	{
		bool flag = false;
		string v = string.Empty;
		if (rollId != global::AttributeId.NONE)
		{
			int num = target.unit.GetAttribute(rollId);
			num += provider.GetAttributeModifier(rollId);
			flag = target.unit.Roll(global::PandoraSingleton<global::MissionManager>.Instance.NetworkTyche, num, rollId, false, false, 0);
			if (flag)
			{
				v = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("retro_outcome_resist");
			}
		}
		global::Enchantment enchantment = null;
		if (!flag)
		{
			enchantment = target.unit.AddEnchantment(effectId, this.unit, false, false, allegianceId);
			if (enchantment == null)
			{
				v = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("retro_outcome_immune");
			}
			if (updatedUnits.IndexOf(target) == -1)
			{
				updatedUnits.Add(target);
			}
		}
		if (enchantment != null && !enchantment.Data.NoDisplay)
		{
			global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<global::UnitController, string, global::EffectTypeId, string>(global::Notices.RETROACTION_TARGET_ENCHANTMENT, target, enchantment.LocalizedName, enchantment.Data.EffectTypeId, v);
		}
	}

	public void Ground()
	{
		if (global::UnityEngine.Physics.Linecast(base.transform.position + global::UnityEngine.Vector3.up, base.transform.position + global::UnityEngine.Vector3.up * -3f, out this.raycastHitInfo, global::LayerMaskManager.groundMask))
		{
			this.SetFixed(this.raycastHitInfo.point, true);
		}
	}

	public void SpawnBeacon()
	{
		this.UpdateActionStatus(true, global::UnitActionRefreshId.ON_ACTION);
		this.SetCurrentBeacon(global::PandoraSingleton<global::MissionManager>.Instance.SpawnBeacon(base.transform.position));
		global::PandoraSingleton<global::MissionManager>.Instance.MoveCircle.Show(this.startPosition, (float)this.unit.Movement);
	}

	public void RevertBeacons(global::Beacon keavin)
	{
		if (keavin == this.CurrentBeacon)
		{
			return;
		}
		this.SetCurrentBeacon(keavin);
		global::PandoraSingleton<global::MissionManager>.Instance.RevertBeacons(keavin);
		global::PandoraSingleton<global::MissionManager>.Instance.MoveCircle.Show(this.startPosition, (float)this.unit.Movement);
	}

	private void SetCurrentBeacon(global::Beacon keavin)
	{
		this.CurrentBeacon = keavin;
		this.startPosition = keavin.transform.position;
	}

	public void ValidMove()
	{
		this.unit.RemoveTempPoints();
		global::PandoraSingleton<global::MissionManager>.Instance.ClearBeacons();
		this.startPosition = base.transform.position;
		this.startRotation = base.transform.rotation;
	}

	public override void SetAnimSpeed(float speed)
	{
		if (this.Engaged && speed == 0f)
		{
			if (this.currentAnimSpeed > 0f)
			{
				this.currentAnimSpeed = 0f;
			}
		}
		else
		{
			this.currentAnimSpeed = speed;
			base.SetAnimSpeed(speed);
		}
	}

	public void SetCombatCircle(global::UnitController currentUnit, bool forced = false)
	{
		if (!forced && (currentUnit == this || this.unit.Status == global::UnitStateId.OUT_OF_ACTION))
		{
			this.combatCircle.Edges.Clear();
			this.combatCircle.gameObject.SetActive(false);
		}
		else
		{
			this.combatCircle.gameObject.SetActive(true);
			global::UnityEngine.Quaternion quaternion = currentUnit.transform.rotation;
			float @float;
			float sizeB;
			if (this.unit.Id == global::UnitId.MANTICORE || currentUnit.unit.Id == global::UnitId.MANTICORE)
			{
				@float = global::Constant.GetFloat(global::ConstantId.MELEE_RANGE_VERY_LARGE);
				sizeB = global::Constant.GetFloat(global::ConstantId.MELEE_RANGE_VERY_VERY_LARGE);
				quaternion = ((this.unit.Id != global::UnitId.MANTICORE) ? quaternion : base.transform.rotation);
			}
			else
			{
				bool flag = this.unit.Data.UnitSizeId == global::UnitSizeId.LARGE || currentUnit.unit.Data.UnitSizeId == global::UnitSizeId.LARGE;
				@float = global::Constant.GetFloat((!flag) ? global::ConstantId.MELEE_RANGE_NORMAL : global::ConstantId.MELEE_RANGE_LARGE);
				sizeB = @float;
			}
			float currentUnitRadius = (currentUnit.unit.Id != global::UnitId.MANTICORE) ? currentUnit.CapsuleRadius : (currentUnit.CapsuleHeight / 2f);
			this.combatCircle.Set(this.IsEnemy(currentUnit), this.Engaged, currentUnit.IsPlayed(), @float, sizeB, currentUnitRadius, quaternion);
		}
		this.SetGraphWalkability(!this.combatCircle.gameObject.activeSelf);
		this.SetCombatCircleAlpha(currentUnit);
	}

	public void SetCombatCircleAlpha(global::UnitController currentUnit)
	{
		if (currentUnit != this && this.unit.Status != global::UnitStateId.OUT_OF_ACTION && this.Imprint.State == global::MapImprintStateId.VISIBLE)
		{
			float num = global::UnityEngine.Vector3.SqrMagnitude(currentUnit.transform.position - base.transform.position);
			this.combatCircle.SetAlpha(global::UnityEngine.Mathf.Clamp((80f - num) / 80f, 0f, 1f));
		}
		else
		{
			this.combatCircle.SetAlpha(0f);
		}
	}

	public void CheckEngaged(bool applyEnchants)
	{
		this.newEngagedUnits.Clear();
		if ((global::PandoraSingleton<global::MissionManager>.Instance.GetCurrentUnit() != null && global::PandoraSingleton<global::MissionManager>.Instance.IsCurrentPlayer()) || (global::PandoraSingleton<global::MissionManager>.Instance.GetCurrentUnit() == null && global::PandoraSingleton<global::Hermes>.Instance.IsHost()))
		{
			if (!this.IsInFriendlyZone && this.unit.Status != global::UnitStateId.OUT_OF_ACTION)
			{
				global::System.Collections.Generic.List<global::UnitController> allEnemies = global::PandoraSingleton<global::MissionManager>.Instance.GetAllEnemies(this.unit.warbandIdx);
				for (int i = 0; i < allEnemies.Count; i++)
				{
					global::UnitController unitController = allEnemies[i];
					if (unitController.unit.Status != global::UnitStateId.OUT_OF_ACTION)
					{
						bool flag = false;
						float num = global::UnityEngine.Mathf.Abs(base.transform.position.y - unitController.transform.position.y);
						if (num <= 1.9f && this.IsInRange(unitController, 0f, (float)this.unit.ViewDistance, global::Constant.GetFloat(global::ConstantId.RANGE_SPELL_REQUIRED_PERC), false, false, global::BoneId.NONE))
						{
							global::UnityEngine.Vector2 vector = new global::UnityEngine.Vector2(base.transform.position.x, base.transform.position.z);
							global::UnityEngine.Vector2 vector2 = new global::UnityEngine.Vector2(unitController.transform.position.x, unitController.transform.position.z);
							float minEdgeDistance = (!this.EngagedUnits.Contains(unitController)) ? 0f : 0.1f;
							float d = 100f;
							global::UnityEngine.Vector2 checkDestPoint = vector + (vector2 - vector) * d;
							flag = global::PandoraUtils.IsPointInsideEdges(unitController.combatCircle.Edges, vector, checkDestPoint, minEdgeDistance);
							if (!flag)
							{
								checkDestPoint = vector2 + (vector - vector2) * d;
								flag = global::PandoraUtils.IsPointInsideEdges(this.combatCircle.Edges, vector2, checkDestPoint, minEdgeDistance);
							}
						}
						if (flag)
						{
							this.newEngagedUnits.Add(unitController);
						}
					}
				}
			}
			bool flag2 = this.newEngagedUnits.Count != this.EngagedUnits.Count;
			if (!flag2)
			{
				for (int j = 0; j < this.newEngagedUnits.Count; j++)
				{
					if (this.EngagedUnits.IndexOf(this.newEngagedUnits[j]) == -1)
					{
						flag2 = true;
						break;
					}
				}
			}
			if (flag2)
			{
				this.SendNewEngagedUnits(applyEnchants);
				this.ProcessEngagedUnits(applyEnchants);
			}
		}
	}

	private void ProcessEngagedUnits(bool applyEnchants)
	{
		this.modifiedUnits.Clear();
		this.involvedUnits.Clear();
		this.modifiedUnits.Add(this);
		for (int i = 0; i < this.newEngagedUnits.Count; i++)
		{
			global::UnitController unitController = this.newEngagedUnits[i];
			if (!this.EngagedUnits.Contains(unitController))
			{
				global::PandoraUtils.InsertDistinct<global::UnitController>(ref this.modifiedUnits, unitController);
				this.EngagedUnits.Add(unitController);
				global::PandoraDebug.LogInfo("Adding engaged unit " + unitController.name + " to " + base.name, "uncategorised", null);
			}
			if (!unitController.EngagedUnits.Contains(this))
			{
				unitController.EngagedUnits.Add(this);
				global::PandoraDebug.LogInfo("Adding engaged unit " + base.name + " to " + unitController.name, "uncategorised", null);
			}
		}
		for (int j = this.EngagedUnits.Count - 1; j >= 0; j--)
		{
			global::UnitController unitController2 = this.EngagedUnits[j];
			if (!this.newEngagedUnits.Contains(unitController2))
			{
				this.EngagedUnits.RemoveAt(j);
				global::PandoraUtils.InsertDistinct<global::UnitController>(ref this.modifiedUnits, unitController2);
				global::PandoraDebug.LogInfo("Removing engaged unit " + unitController2.name + " from " + base.name, "uncategorised", null);
				if (unitController2.unit.Status != global::UnitStateId.OUT_OF_ACTION && unitController2.EngagedUnits.Remove(this))
				{
					global::PandoraDebug.LogInfo("Removing engaged unit " + base.name + " from " + unitController2.name, "uncategorised", null);
				}
			}
		}
		if (applyEnchants)
		{
			for (int k = 0; k < this.modifiedUnits.Count; k++)
			{
				this.modifiedUnits[k].TriggerEngagedEnchantments();
				global::PandoraUtils.InsertDistinct<global::UnitController>(ref this.involvedUnits, this.modifiedUnits[k]);
				for (int l = 0; l < this.modifiedUnits[k].EngagedUnits.Count; l++)
				{
					global::PandoraUtils.InsertDistinct<global::UnitController>(ref this.involvedUnits, this.modifiedUnits[k].EngagedUnits[l]);
				}
			}
			for (int m = 0; m < this.involvedUnits.Count; m++)
			{
				this.involvedUnits[m].TriggerAlliesEnchantments();
			}
		}
	}

	public void TriggerEngagedEnchantments()
	{
		for (int i = this.unit.Enchantments.Count - 1; i >= 0; i--)
		{
			if (this.unit.Enchantments[i].Data.EnchantmentTriggerIdDestroy == global::EnchantmentTriggerId.ON_ENGAGED_OWNER && !this.IsEngagedUnit(this.unit.Enchantments[i].Provider))
			{
				this.unit.RemoveEnchantment(i);
			}
		}
		if (this.EngagedUnits.Count == 0)
		{
			global::PandoraDebug.LogInfo(base.name + " not engaged anymore", "ENGAGE", this);
			this.unit.DestroyEnchantments(global::EnchantmentTriggerId.ON_ENGAGED_SINGLE, false);
			this.unit.DestroyEnchantments(global::EnchantmentTriggerId.ON_ENGAGED_MULTIPLE, false);
		}
		else if (this.EngagedUnits.Count == 1)
		{
			global::PandoraDebug.LogInfo(base.name + " now engaged with single unit", "ENGAGE", this);
			this.unit.DestroyEnchantments(global::EnchantmentTriggerId.ON_ENGAGED_SINGLE, false);
			this.TriggerEnchantments(global::EnchantmentTriggerId.ON_ENGAGED_SINGLE, global::SkillId.NONE, global::UnitActionId.NONE);
		}
		else if (this.EngagedUnits.Count > 1)
		{
			global::PandoraDebug.LogInfo(base.name + " now engaged with multiple units", "ENGAGE", this);
			this.unit.DestroyEnchantments(global::EnchantmentTriggerId.ON_ENGAGED_MULTIPLE, false);
			this.TriggerEnchantments(global::EnchantmentTriggerId.ON_ENGAGED_MULTIPLE, global::SkillId.NONE, global::UnitActionId.NONE);
		}
	}

	private bool IsEngagedUnit(global::Unit unit)
	{
		for (int i = 0; i < this.EngagedUnits.Count; i++)
		{
			if (this.EngagedUnits[i].unit == unit)
			{
				return true;
			}
		}
		return false;
	}

	public void TriggerAlliesEnchantments()
	{
		int num = 0;
		int teamIdx = this.GetWarband().teamIdx;
		int num2 = 0;
		while (num2 < this.EngagedUnits.Count && num == 0)
		{
			int num3 = 0;
			while (num3 < this.EngagedUnits[num2].EngagedUnits.Count && num == 0)
			{
				global::UnitController unitController = this.EngagedUnits[num2].EngagedUnits[num3];
				if (unitController != this && unitController.GetWarband().teamIdx == teamIdx)
				{
					num++;
				}
				num3++;
			}
			num2++;
		}
		global::PandoraDebug.LogInfo(string.Concat(new object[]
		{
			base.name,
			" has ",
			num,
			" allies"
		}), "ENGAGE", this);
		this.unit.DestroyEnchantments((num != 0) ? global::EnchantmentTriggerId.ON_ENGAGED_ALLY : global::EnchantmentTriggerId.ON_ENGAGED_NO_ALLY, false);
		this.TriggerEnchantments((num != 0) ? global::EnchantmentTriggerId.ON_ENGAGED_ALLY : global::EnchantmentTriggerId.ON_ENGAGED_NO_ALLY, global::SkillId.NONE, global::UnitActionId.NONE);
	}

	public bool HasSpells()
	{
		for (int i = 0; i < this.actionStatus.Count; i++)
		{
			if (this.actionStatus[i].ActionId == global::UnitActionId.SPELL)
			{
				return true;
			}
		}
		return false;
	}

	public global::System.Collections.Generic.List<global::UnityEngine.MonoBehaviour> GetCurrentActionTargets()
	{
		this.availableTargets.Clear();
		for (int i = 0; i < this.CurrentAction.Targets.Count; i++)
		{
			this.availableTargets.Add(this.CurrentAction.Targets[i]);
		}
		for (int j = 0; j < this.CurrentAction.Destructibles.Count; j++)
		{
			this.availableTargets.Add(this.CurrentAction.Destructibles[j]);
		}
		return this.availableTargets;
	}

	public void SetArcTargets(global::System.Collections.Generic.List<global::UnityEngine.MonoBehaviour> targets, global::UnityEngine.Vector3 dir, bool highlightTargets)
	{
		this.defenders.Clear();
		this.destructTargets.Clear();
		for (int i = 0; i < targets.Count; i++)
		{
			global::UnityEngine.Vector3 to = targets[i].transform.position - base.transform.position;
			if (global::UnityEngine.Vector3.Angle(dir, to) <= (float)this.currentAction.skillData.Angle / 2f)
			{
				if (targets[i] is global::UnitController)
				{
					this.defenders.Add((global::UnitController)targets[i]);
				}
				else if (targets[i] is global::Destructible)
				{
					this.destructTargets.Add((global::Destructible)targets[i]);
				}
			}
			if (highlightTargets)
			{
				this.HighlightTargets();
			}
		}
	}

	public void SetAoeTargets(global::System.Collections.Generic.List<global::UnityEngine.MonoBehaviour> targets, global::UnityEngine.Transform aoeSphere, bool highlightTargets)
	{
		float @float = global::Constant.GetFloat(global::ConstantId.RANGE_SPELL_REQUIRED_PERC);
		this.defenders.Clear();
		this.destructTargets.Clear();
		global::UnityEngine.Vector3 vector = base.transform.position + global::UnityEngine.Vector3.up * 1.25f;
		global::UnityEngine.Vector3 vector2 = vector + (aoeSphere.position - vector) * 0.99f;
		for (int i = 0; i < targets.Count; i++)
		{
			if (targets[i] is global::UnitController)
			{
				global::UnitController unitController = (global::UnitController)targets[i];
				global::TargetData targetData = new global::TargetData(unitController);
				if (this.CanTargetFromPoint(vector2, targetData, 0f, (float)this.CurrentAction.Radius, @float, false, false, global::BoneId.NONE))
				{
					this.defenders.Add(unitController);
				}
			}
			else if (targets[i] is global::Destructible)
			{
				global::Destructible destructible = (global::Destructible)targets[i];
				if (destructible.IsInRange(vector2, (float)this.CurrentAction.Radius))
				{
					this.destructTargets.Add(destructible);
				}
			}
		}
		if (highlightTargets)
		{
			this.HighlightTargets();
		}
	}

	public void SetConeTargets(global::System.Collections.Generic.List<global::UnityEngine.MonoBehaviour> targets, global::UnityEngine.Transform cone, bool highlighTargets)
	{
		this.SetGeometryTargets(targets, cone, new global::CheckGeometry(this.pointInsideCone), cone.forward, highlighTargets);
	}

	public void SetLineTargets(global::System.Collections.Generic.List<global::UnityEngine.MonoBehaviour> targets, global::UnityEngine.Transform line, bool highlighTargets)
	{
		this.SetGeometryTargets(targets, line, new global::CheckGeometry(this.pointsInsideCylinder), line.forward, highlighTargets);
	}

	public void SetCylinderTargets(global::System.Collections.Generic.List<global::UnityEngine.MonoBehaviour> targets, global::UnityEngine.Transform cylinder, bool highlighTargets)
	{
		this.SetGeometryTargets(targets, cylinder, new global::CheckGeometry(this.pointsInsideCylinder), cylinder.up, highlighTargets);
	}

	private void SetGeometryTargets(global::System.Collections.Generic.List<global::UnityEngine.MonoBehaviour> targets, global::UnityEngine.Transform geo, global::CheckGeometry geometryChecker, global::UnityEngine.Vector3 dir, bool highlightTargets)
	{
		bool flag = this.CurrentAction.IsShootAction();
		float requiredPerc = (!flag) ? global::Constant.GetFloat(global::ConstantId.RANGE_SPELL_REQUIRED_PERC) : global::Constant.GetFloat(global::ConstantId.RANGE_SHOOT_REQUIRED_PERC);
		this.defenders.Clear();
		this.destructTargets.Clear();
		for (int i = 0; i < targets.Count; i++)
		{
			if (targets[i] is global::UnitController)
			{
				global::UnitController unitController = (global::UnitController)targets[i];
				global::TargetData targetData = new global::TargetData(unitController);
				if (this.CanTargetFromPoint(geo.position, targetData, (float)this.CurrentAction.RangeMin, (float)this.CurrentAction.RangeMax, requiredPerc, flag, flag, global::BoneId.NONE))
				{
					bool flag2 = false;
					global::System.Collections.Generic.List<global::BoneTargetRange> list = (!flag) ? targetData.boneTargetRange : targetData.boneTargetRangeBlockingUnit;
					int num = 0;
					while (!flag2 && num < list.Count)
					{
						if (list[num].hitBone && geometryChecker(unitController.transform.TransformPoint(unitController.boneTargets[num].position), geo, (float)this.CurrentAction.Radius, (float)this.CurrentAction.RangeMax, dir))
						{
							flag2 = true;
							break;
						}
						num++;
					}
					if (flag2)
					{
						this.defenders.Add(unitController);
					}
				}
			}
			else if (targets[i] is global::Destructible && ((global::Destructible)targets[i]).IsInRange(geo.position, (float)this.CurrentAction.RangeMax) && geometryChecker(targets[i].transform.position + global::UnityEngine.Vector3.up, geo, (float)this.CurrentAction.Radius, (float)this.CurrentAction.RangeMax, dir))
			{
				this.destructTargets.Add((global::Destructible)targets[i]);
			}
		}
		if (highlightTargets)
		{
			this.HighlightTargets();
		}
	}

	private bool pointInsideCone(global::UnityEngine.Vector3 pos, global::UnityEngine.Transform cone, float coneRadius, float coneRange, global::UnityEngine.Vector3 dir)
	{
		float num = global::UnityEngine.Vector3.Dot(pos - cone.position, dir);
		if (num >= 0f && num <= coneRange)
		{
			float num2 = num / coneRange * coneRadius;
			float num3 = global::UnityEngine.Vector3.SqrMagnitude(pos - cone.position - dir * num);
			return num3 < num2 * num2;
		}
		return false;
	}

	private bool pointsInsideCylinder(global::UnityEngine.Vector3 pos, global::UnityEngine.Transform cylinder, float cylinderRadius, float cylinderRange, global::UnityEngine.Vector3 dir)
	{
		return this.pointsInsideCylinder(pos, cylinder.position, cylinderRadius, cylinderRange, dir);
	}

	private bool pointsInsideCylinder(global::UnityEngine.Vector3 pos, global::UnityEngine.Vector3 cylinderPos, float cylinderRadius, float cylinderRange, global::UnityEngine.Vector3 dir)
	{
		float num = global::UnityEngine.Vector3.Dot(pos - cylinderPos, dir);
		if (num >= 0f && num <= cylinderRange)
		{
			float num2 = global::UnityEngine.Vector3.SqrMagnitude(pos - cylinderPos - dir * num);
			return num2 < cylinderRadius * cylinderRadius;
		}
		return false;
	}

	public bool isInsideCylinder(global::UnityEngine.Vector3 cyclinderPos, float radius, float height, global::UnityEngine.Vector3 up)
	{
		for (int i = 0; i < this.boneTargets.Count; i++)
		{
			global::UnityEngine.Vector3 pos = base.transform.TransformPoint(this.boneTargets[i].position);
			if (this.pointsInsideCylinder(pos, cyclinderPos, radius, height, up))
			{
				return true;
			}
		}
		return false;
	}

	private void HighlightTargets()
	{
		this.ClearFlyingTexts();
		for (int i = 0; i < this.defenders.Count; i++)
		{
			if (this.defenders[i].Imprint.State == global::MapImprintStateId.VISIBLE)
			{
				int idx = i;
				global::PandoraSingleton<global::FlyingTextManager>.Instance.GetFlyingText(global::FlyingTextId.TARGET, delegate(global::FlyingText flyingText)
				{
					global::FlyingTarget flyingTarget = (global::FlyingTarget)flyingText;
					this.flyingOverviews.Add(flyingTarget);
					flyingTarget.Play(this, this.defenders[idx]);
					this.defenders[idx].Highlight.On(global::UnityEngine.Color.red);
				});
			}
		}
		for (int j = 0; j < this.destructTargets.Count; j++)
		{
			if (this.destructTargets[j].Imprint.State == global::MapImprintStateId.VISIBLE)
			{
				int idx = j;
				global::PandoraSingleton<global::FlyingTextManager>.Instance.GetFlyingText(global::FlyingTextId.TARGET, delegate(global::FlyingText flyingText)
				{
					global::FlyingTarget flyingTarget = (global::FlyingTarget)flyingText;
					this.flyingOverviews.Add(flyingTarget);
					flyingTarget.Play(this, this.destructTargets[idx]);
					this.destructTargets[idx].Highlight.On(global::UnityEngine.Color.red);
				});
			}
		}
	}

	public void ClearFlyingTexts()
	{
		for (int i = 0; i < this.flyingOverviews.Count; i++)
		{
			this.flyingOverviews[i].Deactivate();
		}
		this.flyingOverviews.Clear();
	}

	public void HitDefenders(global::UnityEngine.Vector3 projDir, bool hurt = true)
	{
		for (int i = 0; i < this.defenders.Count; i++)
		{
			global::UnitController unitController = this.defenders[i];
			unitController.unit.UpdateEnchantmentsFx();
			global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<global::UnitController>(global::Notices.RETROACTION_SHOW_OUTCOME, unitController);
			if (unitController.unit.PreviousStatus == global::UnitStateId.STUNNED && unitController.unit.Status == global::UnitStateId.NONE)
			{
				hurt = true;
				unitController.attackResultId = global::AttackResultId.NONE;
			}
			if (unitController.unit.Status == global::UnitStateId.STUNNED)
			{
				hurt = true;
				unitController.attackResultId = global::AttackResultId.HIT;
			}
			if (hurt)
			{
				if (unitController.lastActionWounds <= 0)
				{
					int num = 0;
					if (unitController.attackResultId == global::AttackResultId.HIT || unitController.attackResultId == global::AttackResultId.HIT_NO_WOUND)
					{
						global::UnityEngine.Vector3 forward = unitController.transform.forward;
						forward.y = 0f;
						num = ((unitController.unit.Status != global::UnitStateId.OUT_OF_ACTION) ? 1 : 0);
						num += ((global::UnityEngine.Vector3.Angle(projDir, forward) <= 90f) ? 1 : 0);
					}
					unitController.PlayDefState(unitController.attackResultId, num, unitController.unit.Status);
				}
				else if (unitController.lastActionWounds > 0)
				{
					unitController.PlayBuffDebuff(global::EffectTypeId.BUFF);
					unitController.EventDisplayDamage();
				}
			}
			else if ((this.currentAction.skillData.EffectTypeId == global::EffectTypeId.BUFF || this.currentAction.skillData.EffectTypeId == global::EffectTypeId.DEBUFF) && unitController.unit.HasEnchantmentsChanged)
			{
				unitController.PlayBuffDebuff(this.currentAction.skillData.EffectTypeId);
			}
			if (this.CurrentAction.fxData != null)
			{
				global::PandoraSingleton<global::Prometheus.Prometheus>.Instance.SpawnFx(this.CurrentAction.fxData.HitFx, unitController, null, null, default(global::UnityEngine.Vector3), default(global::UnityEngine.Vector3), null);
			}
		}
	}

	public void Hit()
	{
		if (this.attackResultId != global::AttackResultId.NONE || this.unit.Status != global::UnitStateId.NONE)
		{
			base.PlayDefState(this.attackResultId, (this.unit.Status != global::UnitStateId.OUT_OF_ACTION) ? 0 : 1, this.unit.Status);
		}
		else if (this.buffResultId != global::EffectTypeId.NONE)
		{
			base.PlayBuffDebuff(this.buffResultId);
			this.buffResultId = global::EffectTypeId.NONE;
		}
	}

	public void EnterZoneAoeAnim()
	{
		if (this.attackResultId != global::AttackResultId.NONE || this.unit.Status != global::UnitStateId.NONE)
		{
			base.PlayDefState(this.attackResultId, 0, this.unit.Status);
		}
		else
		{
			base.PlayBuffDebuff(this.currentZoneAoe.GetEnterEffectType());
		}
	}

	public void RefreshDetected()
	{
		base.Highlight.OccluderOn();
		float num = (float)this.unit.Movement * 2f;
		num *= num;
		for (int i = 0; i < this.detectedUnits.Count; i++)
		{
			global::UnitController unitController = this.detectedUnits[i];
			if (unitController != null)
			{
				if (global::UnityEngine.Vector3.SqrMagnitude(base.transform.position - unitController.transform.position) < num)
				{
					unitController.Highlight.On(this.DETECTED_COLOR);
					unitController.Highlight.seeThrough = true;
					if (!unitController.Imprint.alwaysVisible)
					{
						unitController.Imprint.alwaysVisible = true;
						unitController.Imprint.needsRefresh = true;
					}
				}
				else
				{
					unitController.Highlight.Off();
					if (unitController.GetWarband().idx != this.GetWarband().idx && unitController.Imprint.alwaysVisible)
					{
						unitController.Imprint.alwaysVisible = false;
						unitController.Imprint.needsRefresh = true;
					}
				}
			}
		}
		for (int j = 0; j < this.detectedTriggers.Count; j++)
		{
			if (this.detectedTriggers[j] != null)
			{
				if (global::UnityEngine.Vector3.SqrMagnitude(base.transform.position - this.detectedTriggers[j].transform.position) < num)
				{
					this.detectedTriggers[j].Highlight.On(this.DETECTED_COLOR);
					this.detectedTriggers[j].Highlight.seeThrough = true;
				}
				else
				{
					this.detectedTriggers[j].Highlight.Off();
				}
			}
		}
		for (int k = 0; k < this.detectedInteractivePoints.Count; k++)
		{
			if (this.detectedInteractivePoints[k] != null)
			{
				if (global::UnityEngine.Vector3.SqrMagnitude(base.transform.position - this.detectedInteractivePoints[k].transform.position) < num)
				{
					if (this.detectedInteractivePoints[k].Highlight != null)
					{
						this.detectedInteractivePoints[k].Highlight.On(this.DETECTED_COLOR);
						this.detectedInteractivePoints[k].Highlight.seeThrough = true;
					}
					if (this.detectedInteractivePoints[k].Imprint != null && !this.detectedInteractivePoints[k].Imprint.alwaysVisible)
					{
						this.detectedInteractivePoints[k].Imprint.alwaysVisible = true;
						this.detectedInteractivePoints[k].Imprint.needsRefresh = true;
					}
				}
				else if (this.detectedInteractivePoints[k].Imprint != null && this.detectedInteractivePoints[k].Imprint.alwaysVisible)
				{
					this.detectedInteractivePoints[k].Imprint.alwaysVisible = false;
					this.detectedInteractivePoints[k].Imprint.needsRefresh = true;
				}
			}
		}
	}

	public void HideDetected()
	{
		for (int i = 0; i < this.detectedUnits.Count; i++)
		{
			if (this.detectedUnits[i] != null)
			{
				this.detectedUnits[i].Highlight.Off();
				this.detectedUnits[i].Highlight.seeThrough = false;
				if (this.detectedUnits[i].GetWarband().idx != this.GetWarband().idx)
				{
					this.detectedUnits[i].Imprint.alwaysVisible = false;
				}
				this.detectedUnits[i].Imprint.needsRefresh = true;
			}
		}
		for (int j = 0; j < this.detectedTriggers.Count; j++)
		{
			if (this.detectedTriggers[j] != null)
			{
				this.detectedTriggers[j].Highlight.Off();
				this.detectedTriggers[j].Highlight.seeThrough = false;
			}
		}
		for (int k = 0; k < this.detectedInteractivePoints.Count; k++)
		{
			if (this.detectedInteractivePoints[k] != null)
			{
				if (this.detectedInteractivePoints[k].Highlight != null)
				{
					this.detectedInteractivePoints[k].Highlight.Off();
					this.detectedInteractivePoints[k].Highlight.seeThrough = false;
				}
				if (this.detectedInteractivePoints[k].Imprint != null)
				{
					this.detectedInteractivePoints[k].Imprint.alwaysVisible = false;
					this.detectedInteractivePoints[k].Imprint.needsRefresh = true;
				}
			}
		}
	}

	private void Fade(bool fade)
	{
		if (!fade)
		{
			if (this.Imprint != null && this.Imprint.State == global::MapImprintStateId.VISIBLE)
			{
				this.Hide(false, false, null);
			}
		}
		else
		{
			this.Hide(true, false, null);
		}
	}

	public void ReduceAlliesNavCutterSize(global::System.Action alliesNavReducedCb)
	{
		base.StartCoroutine(this.ReduceNavCutterSize(alliesNavReducedCb));
	}

	private global::System.Collections.IEnumerator ReduceNavCutterSize(global::System.Action alliesNavReducedCb)
	{
		global::PandoraSingleton<global::MissionManager>.Instance.lockNavRefresh = true;
		global::System.Collections.Generic.List<global::UnitController> allies = global::PandoraSingleton<global::MissionManager>.Instance.GetAliveAllies(this.unit.warbandIdx);
		for (int i = 0; i < allies.Count; i++)
		{
			if (allies[i] != this)
			{
				if (allies[i].AICtrlr != null && allies[i].Engaged)
				{
					allies[i].combatCircle.SetNavCutter();
				}
				else
				{
					allies[i].combatCircle.OverrideNavCutterRadius(this.CapsuleRadius + allies[i].CapsuleRadius + 0.1f);
				}
				allies[i].SetGraphWalkability(false);
				yield return null;
			}
		}
		global::PandoraSingleton<global::MissionManager>.Instance.lockNavRefresh = false;
		while (global::PandoraSingleton<global::MissionManager>.Instance.IsNavmeshUpdating)
		{
			yield return null;
		}
		if (alliesNavReducedCb != null)
		{
			alliesNavReducedCb();
		}
		yield break;
	}

	public void RestoreAlliesNavCutterSize()
	{
		global::System.Collections.Generic.List<global::UnitController> aliveAllies = global::PandoraSingleton<global::MissionManager>.Instance.GetAliveAllies(this.unit.warbandIdx);
		for (int i = 0; i < aliveAllies.Count; i++)
		{
			if (aliveAllies[i] != this)
			{
				aliveAllies[i].combatCircle.SetNavCutter();
				aliveAllies[i].SetGraphWalkability(false);
			}
		}
	}

	public bool isNavCutterActive()
	{
		return this.combatCircle.NavCutterEnabled;
	}

	public void SetGraphWalkability(bool walkable)
	{
		walkable |= (this.unit.Status == global::UnitStateId.OUT_OF_ACTION);
		if (!this.combatCircle.NavCutterEnabled && !walkable)
		{
			this.SetCombatCircle(global::PandoraSingleton<global::MissionManager>.Instance.GetCurrentUnit(), false);
		}
		this.combatCircle.SetNavCutterEnabled(!walkable);
		global::PandoraSingleton<global::MissionManager>.Instance.RefreshGraph();
	}

	public void ClampToNavMesh()
	{
		if (!this.IsFixed)
		{
			base.transform.position = global::PandoraSingleton<global::MissionManager>.Instance.ClampToNavMesh(base.transform.position);
		}
	}

	private void OnTriggerEnter(global::UnityEngine.Collider other)
	{
		if (other.transform.parent != null)
		{
			global::ZoneAoe component = other.transform.parent.gameObject.GetComponent<global::ZoneAoe>();
			if (component != null)
			{
				component.EnterZone(this);
			}
		}
		if (!this.IsFixed)
		{
			bool flag = false;
			bool flag2 = false;
			global::Beacon beacon = null;
			if (other.transform.parent != null)
			{
				beacon = other.transform.parent.gameObject.GetComponent<global::Beacon>();
			}
			if (beacon != null)
			{
				this.RevertBeacons(beacon);
				return;
			}
			if (!this.Engaged && other.transform.parent != null && other.transform.parent.parent != null)
			{
				global::UnitController component2 = other.transform.parent.parent.gameObject.GetComponent<global::UnitController>();
				if (component2 != null && component2.unit.Status != global::UnitStateId.OUT_OF_ACTION && component2.GetWarband().teamIdx == this.GetWarband().teamIdx && this.friendlyEntered.IndexOf(component2) == -1)
				{
					this.friendlyEntered.Add(component2);
					flag = true;
					flag2 = true;
					if (this.friendlyEntered.Count == 1)
					{
						float d = global::Constant.GetFloat((component2.unit.Data.UnitSizeId != global::UnitSizeId.LARGE) ? global::ConstantId.MELEE_RANGE_NORMAL : global::ConstantId.MELEE_RANGE_LARGE) + 0.1f;
						this.friendlyZoneEntryPoint = component2.transform.position + global::UnityEngine.Vector3.Normalize(base.transform.position - component2.transform.position) * d;
					}
				}
			}
			global::InteractivePoint[] components = other.GetComponents<global::InteractivePoint>();
			if (components.Length == 0 && other.transform.parent != null)
			{
				components = other.transform.parent.GetComponents<global::InteractivePoint>();
			}
			if (components.Length != 0)
			{
				foreach (global::InteractivePoint item in components)
				{
					if (this.interactivePoints.IndexOf(item) == -1)
					{
						this.interactivePoints.Add(item);
						flag = true;
					}
				}
				global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice(global::Notices.INTERACTION_POINTS_CHANGED);
			}
			if (other.transform.parent != null)
			{
				global::LocateZone component3 = other.transform.parent.gameObject.GetComponent<global::LocateZone>();
				if (component3 != null)
				{
					this.GetWarband().LocateZone(component3);
				}
			}
			if (flag && this.Initialized && !global::PandoraSingleton<global::SequenceManager>.Instance.isPlaying)
			{
				this.UpdateActionStatus(true, (!flag2) ? global::UnitActionRefreshId.ON_TRIGGER : global::UnitActionRefreshId.NONE);
			}
			if (this.StateMachine.GetActiveStateId() == 11)
			{
				((global::Moving)this.StateMachine.GetState(11)).OnTriggerEnter(other);
			}
			if (this.StateMachine.GetActiveStateId() != 17 && this.StateMachine.GetActiveStateId() != 18 && this.StateMachine.GetActiveStateId() != 47 && this.StateMachine.GetActiveStateId() != 48 && !this.IsInFriendlyZone && other.transform.parent != null)
			{
				global::TriggerPoint component4 = other.transform.parent.gameObject.GetComponent<global::TriggerPoint>();
				if (component4 != null)
				{
					if (component4 is global::Destructible)
					{
						this.triggeredDestructibles.Add((global::Destructible)component4);
						return;
					}
					if (component4 is global::Trap && ((global::Trap)component4).TeamIdx == this.GetWarband().teamIdx)
					{
						return;
					}
					this.currentTeleporter = (component4 as global::Teleporter);
					global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice(global::Notices.INTERACTION_POINTS_CHANGED);
					if (component4.IsActive())
					{
						if (global::PandoraSingleton<global::MissionManager>.Instance.IsCurrentPlayer())
						{
							this.SendTriggerActivated(base.transform.position, base.transform.rotation, component4);
						}
						else
						{
							this.SetFixed(true);
						}
					}
				}
			}
		}
	}

	private void OnTriggerExit(global::UnityEngine.Collider other)
	{
		if (other.transform.parent != null)
		{
			global::ZoneAoe component = other.transform.parent.gameObject.GetComponent<global::ZoneAoe>();
			if (component != null)
			{
				component.ExitZone(this);
			}
		}
		if (!this.IsFixed)
		{
			bool flag = false;
			bool flag2 = false;
			if (other.transform.parent != null && other.transform.parent.parent != null)
			{
				global::UnitController component2 = other.transform.parent.parent.gameObject.GetComponent<global::UnitController>();
				if (component2 != null && component2.GetWarband().teamIdx == this.GetWarband().teamIdx && this.friendlyEntered.IndexOf(component2) != -1)
				{
					this.friendlyEntered.Remove(component2);
					flag = true;
					flag2 = true;
					if (!this.IsInFriendlyZone)
					{
						this.friendlyZoneEntryPoint = global::UnityEngine.Vector3.zero;
					}
				}
			}
			global::InteractivePoint[] components = other.GetComponents<global::InteractivePoint>();
			if (components.Length == 0 && other.transform.parent != null)
			{
				components = other.transform.parent.GetComponents<global::InteractivePoint>();
			}
			if (components.Length != 0)
			{
				foreach (global::InteractivePoint item in components)
				{
					int num = this.interactivePoints.IndexOf(item);
					if (num != -1)
					{
						this.interactivePoints.RemoveAt(num);
						flag = true;
					}
				}
				global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice(global::Notices.INTERACTION_POINTS_CHANGED);
			}
			if (flag && !global::PandoraSingleton<global::SequenceManager>.Instance.isPlaying)
			{
				this.UpdateActionStatus(true, (!flag2) ? global::UnitActionRefreshId.ON_TRIGGER : global::UnitActionRefreshId.NONE);
			}
			if (this.StateMachine.GetActiveStateId() == 11)
			{
				((global::Moving)this.StateMachine.GetState(11)).OnTriggerExit(other);
			}
			if (other.transform.parent != null)
			{
				global::TriggerPoint component3 = other.transform.parent.gameObject.GetComponent<global::TriggerPoint>();
				if (component3 != null)
				{
					if (component3 is global::Destructible)
					{
						this.triggeredDestructibles.Remove((global::Destructible)component3);
						return;
					}
					this.currentTeleporter = null;
					global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice(global::Notices.INTERACTION_POINTS_CHANGED);
				}
			}
		}
	}

	public void StartSync()
	{
		global::PandoraDebug.LogDebug(string.Concat(new object[]
		{
			"start sync for unit ID ",
			this.uid,
			" Owner ID =",
			this.owner
		}), "HERMES", this);
		base.StartCoroutine("NetworkSync");
	}

	public void StopSync()
	{
		global::PandoraDebug.LogDebug(string.Concat(new object[]
		{
			"stop sync for unit ID ",
			this.uid,
			" Owner ID =",
			this.owner
		}), "HERMES", this);
		base.StopCoroutine("NetworkSync");
		this.Send(true, global::Hermes.SendTarget.OTHERS, this.uid, 1U, new object[]
		{
			0f,
			base.transform.rotation,
			base.transform.position
		});
	}

	private global::System.Collections.IEnumerator NetworkSync()
	{
		for (;;)
		{
			this.SendSpeedPosition();
			yield return new global::UnityEngine.WaitForSeconds(0.0166666675f);
		}
		yield break;
	}

	private void SendSpeedPosition()
	{
		this.Send(false, global::Hermes.SendTarget.OTHERS, this.uid, 1U, new object[]
		{
			this.animator.GetFloat(global::AnimatorIds.speed),
			base.transform.rotation,
			base.transform.position
		});
	}

	private void NetworkSyncRPC(float speed, global::UnityEngine.Quaternion rot, global::UnityEngine.Vector3 pos)
	{
		base.transform.rotation = rot;
		base.transform.position = pos;
		if (this.IsFixed)
		{
			this.SetFixed(true);
		}
		else
		{
			this.SetAnimSpeed((speed <= 0f) ? speed : (speed * 0.8f));
		}
	}

	public void SendMoveAndUpdateCircle(uint targetUID, global::UnityEngine.Vector3 pos, global::UnityEngine.Quaternion rot)
	{
		this.Send(true, global::Hermes.SendTarget.ALL, this.uid, 18U, new object[]
		{
			targetUID,
			pos,
			rot
		});
	}

	public void MoveAndUpdateCircleRPC(uint targetUID, global::UnityEngine.Vector3 pos, global::UnityEngine.Quaternion rot)
	{
		global::UnitController unitController = null;
		global::System.Collections.Generic.List<global::UnitController> allUnits = global::PandoraSingleton<global::MissionManager>.Instance.GetAllUnits();
		for (int i = 0; i < allUnits.Count; i++)
		{
			if (allUnits[i].uid == targetUID)
			{
				unitController = allUnits[i];
				break;
			}
		}
		unitController.transform.rotation = rot;
		unitController.transform.position = pos;
		if (unitController.IsFixed)
		{
			unitController.SetFixed(true);
		}
		unitController.SetCombatCircle(global::PandoraSingleton<global::MissionManager>.Instance.GetCurrentUnit(), false);
		unitController.SetGraphWalkability(false);
		unitController.RemoveAthletics();
		if (global::PandoraSingleton<global::MissionManager>.Instance.IsCurrentPlayer())
		{
			for (int j = 0; j < global::PandoraSingleton<global::MissionManager>.Instance.zoneAoes.Count; j++)
			{
				if (global::PandoraSingleton<global::MissionManager>.Instance.zoneAoes[j] != null)
				{
					global::PandoraSingleton<global::MissionManager>.Instance.zoneAoes[j].CheckEnterOrExitUnit(unitController, true);
				}
			}
		}
		global::PandoraSingleton<global::MissionManager>.Instance.MissionEndData.UpdateUnit(unitController);
	}

	public void SendFly()
	{
		this.Send(true, global::Hermes.SendTarget.ALL, this.uid, 21U, new object[0]);
		this.StateMachine.ChangeState(9);
	}

	public void FlyRPC()
	{
		this.StateMachine.ChangeState(48);
	}

	public void SendAthletic()
	{
		this.Send(true, global::Hermes.SendTarget.ALL, this.uid, 19U, new object[0]);
		this.StateMachine.ChangeState(9);
	}

	public void AthleticRPC()
	{
		this.StateMachine.ChangeState(47);
	}

	public void SendAthleticFinished(bool success, global::AthleticAction action)
	{
		this.Send(true, global::Hermes.SendTarget.ALL, this.uid, 20U, new object[]
		{
			success,
			(int)action
		});
		this.StateMachine.ChangeState(9);
	}

	public void AthleticFinishedRPC(bool success, int action)
	{
		if (success)
		{
			if (this.Engaged)
			{
				switch (action)
				{
				case 0:
					this.unit.AddEnchantment(global::EnchantmentId.ACTION_CLIMB_SUCCESS, this.unit, false, true, global::AllegianceId.NONE);
					break;
				case 1:
					this.unit.AddEnchantment(global::EnchantmentId.ACTION_JUMP_DOWN_SUCCESS, this.unit, false, true, global::AllegianceId.NONE);
					break;
				case 2:
					this.unit.AddEnchantment(global::EnchantmentId.ACTION_LEAP_SUCCESS, this.unit, false, true, global::AllegianceId.NONE);
					break;
				default:
					throw new global::System.ArgumentOutOfRangeException();
				}
				this.LaunchMelee(global::UnitController.State.START_MOVE);
			}
			else
			{
				this.StateMachine.ChangeState(10);
			}
		}
		else if (this.Engaged)
		{
			this.LaunchMelee(global::UnitController.State.ATHLETIC_COUNTER);
		}
		else
		{
			this.StateMachine.ChangeState(45);
		}
	}

	public void SendCurse()
	{
		this.Send(true, global::Hermes.SendTarget.ALL, this.uid, 16U, new object[0]);
		this.StateMachine.ChangeState(9);
	}

	public void CurseRPC()
	{
		if (this.unit.Status == global::UnitStateId.OUT_OF_ACTION)
		{
			if (this.currentSpellSuccess && this.currentSpellId != global::SkillId.NONE)
			{
				global::ZoneAoe.Spawn(this, this.GetAction(this.currentSpellId), null);
				this.currentSpellId = global::SkillId.NONE;
			}
			this.StateMachine.ChangeState(10);
		}
		else
		{
			this.StateMachine.ChangeState(29);
		}
	}

	public void SendSkill(global::SkillId skillId)
	{
		if (this.AICtrlr != null)
		{
			this.AICtrlr.UsedSkill(skillId, null);
		}
		this.Send(true, global::Hermes.SendTarget.ALL, this.uid, 2U, new object[]
		{
			(int)skillId
		});
		this.StateMachine.ChangeState(9);
	}

	public void SkillRPC(int skillId)
	{
		global::PandoraDebug.LogDebug("Skill " + (global::SkillId)skillId, "HERMES", this);
		this.ValidMove();
		this.SetCurrentAction((global::SkillId)skillId);
		this.currentSpellTargetPosition = base.transform.position;
		this.CurrentAction.Activate();
	}

	public void SendSkillSingleTarget(global::SkillId skillId, global::UnitController unitCtrlr)
	{
		if (this.AICtrlr != null)
		{
			this.AICtrlr.UsedSkill(skillId, unitCtrlr);
		}
		this.Send(true, global::Hermes.SendTarget.ALL, this.uid, 3U, new object[]
		{
			(int)skillId,
			unitCtrlr.uid
		});
		this.StateMachine.ChangeState(9);
	}

	public void SkillSingleTargetRPC(int skillId, uint targetUID)
	{
		global::PandoraDebug.LogDebug(string.Concat(new object[]
		{
			"SkillSingleTarget ",
			(global::SkillId)skillId,
			"TargetId = ",
			targetUID
		}), "HERMES", this);
		this.ValidMove();
		this.SetCurrentAction((global::SkillId)skillId);
		this.defenderCtrlr = null;
		this.destructibleTarget = null;
		global::System.Collections.Generic.List<global::UnitController> allUnits = global::PandoraSingleton<global::MissionManager>.Instance.GetAllUnits();
		for (int i = 0; i < allUnits.Count; i++)
		{
			if (allUnits[i].uid == targetUID)
			{
				this.defenderCtrlr = allUnits[i];
				break;
			}
		}
		if (this.defenderCtrlr != this)
		{
			this.FaceTarget(this.defenderCtrlr.transform, true);
		}
		this.currentSpellTargetPosition = this.defenderCtrlr.transform.position;
		this.CurrentAction.Activate();
	}

	public void SendSkillTargets(global::SkillId skillId, global::UnityEngine.Vector3 targetPos, global::UnityEngine.Vector3 targetDir)
	{
		if (this.AICtrlr != null)
		{
			this.AICtrlr.UsedSkill(skillId, null);
		}
		this.Send(true, global::Hermes.SendTarget.ALL, this.uid, 4U, new object[]
		{
			(int)skillId,
			targetPos,
			targetDir
		});
		this.StateMachine.ChangeState(9);
	}

	public void SkillTargetsRPC(int skillId, global::UnityEngine.Vector3 targetPos, global::UnityEngine.Vector3 targetDir)
	{
		global::PandoraDebug.LogDebug(string.Concat(new object[]
		{
			"SkillTargetsRPC ",
			(global::SkillId)skillId,
			" Targetpos = ",
			targetPos,
			" TargetDir = ",
			targetDir
		}), "HERMES", this);
		this.ValidMove();
		this.SetCurrentAction((global::SkillId)skillId);
		this.currentSpellTargetPosition = targetPos;
		global::UnityEngine.Quaternion rotation = global::UnityEngine.Quaternion.LookRotation(targetDir);
		if (this.unit.Id != global::UnitId.MANTICORE && !global::UnityEngine.Mathf.Approximately(targetDir.x, 0f) && !global::UnityEngine.Mathf.Approximately(targetDir.z, 0f))
		{
			targetDir.y = 0f;
			base.transform.rotation = global::UnityEngine.Quaternion.LookRotation(targetDir);
		}
		if (global::PandoraSingleton<global::MissionManager>.Instance.dummyTargeter == null)
		{
			global::PandoraSingleton<global::MissionManager>.Instance.dummyTargeter = new global::UnityEngine.GameObject("dummyTargeter");
		}
		global::PandoraSingleton<global::MissionManager>.Instance.dummyTargeter.transform.position = targetPos;
		global::PandoraSingleton<global::MissionManager>.Instance.dummyTargeter.transform.rotation = rotation;
		this.UpdateTargetsData();
		this.CurrentAction.SetTargets();
		switch (this.CurrentAction.TargetingId)
		{
		case global::TargetingId.LINE:
			this.currentSpellTargetPosition += targetDir * (float)this.CurrentAction.skillData.Range;
			this.SetLineTargets(this.GetCurrentActionTargets(), global::PandoraSingleton<global::MissionManager>.Instance.dummyTargeter.transform, false);
			break;
		case global::TargetingId.CONE:
			this.currentSpellTargetPosition += targetDir * (float)this.CurrentAction.skillData.Range;
			this.SetConeTargets(this.GetCurrentActionTargets(), global::PandoraSingleton<global::MissionManager>.Instance.dummyTargeter.transform, false);
			break;
		case global::TargetingId.AREA:
			this.SetAoeTargets(this.GetCurrentActionTargets(), global::PandoraSingleton<global::MissionManager>.Instance.dummyTargeter.transform, false);
			break;
		case global::TargetingId.AREA_GROUND:
			this.SetCylinderTargets(this.GetCurrentActionTargets(), global::PandoraSingleton<global::MissionManager>.Instance.dummyTargeter.transform, false);
			break;
		case global::TargetingId.ARC:
			this.SetArcTargets(this.GetCurrentActionTargets(), targetDir, false);
			break;
		}
		this.CurrentAction.Activate();
	}

	public void SendSkillSingleDestructible(global::SkillId skillId, global::Destructible destruct)
	{
		this.Send(true, global::Hermes.SendTarget.ALL, this.uid, 23U, new object[]
		{
			(int)skillId,
			destruct.guid
		});
		this.StateMachine.ChangeState(9);
	}

	public void SkillSingleDestructibleRPC(int skillId, uint destructUID)
	{
		global::PandoraDebug.LogDebug(string.Concat(new object[]
		{
			"SkillSingleTarget ",
			(global::SkillId)skillId,
			"TargetId = ",
			destructUID
		}), "HERMES", this);
		this.ValidMove();
		this.SetCurrentAction((global::SkillId)skillId);
		this.defenderCtrlr = null;
		this.destructibleTarget = null;
		global::System.Collections.Generic.List<global::TriggerPoint> triggerPoints = global::PandoraSingleton<global::MissionManager>.Instance.triggerPoints;
		for (int i = 0; i < triggerPoints.Count; i++)
		{
			if (triggerPoints[i].guid == destructUID)
			{
				this.destructibleTarget = (global::Destructible)triggerPoints[i];
				break;
			}
		}
		this.FaceTarget(this.destructibleTarget.transform, true);
		this.currentSpellTargetPosition = this.destructibleTarget.transform.position;
		this.CurrentAction.Activate();
	}

	public void SendStartMove(global::UnityEngine.Vector3 currentUnitPos, global::UnityEngine.Quaternion currentUnitRot)
	{
		this.Send(true, global::Hermes.SendTarget.ALL, this.uid, 12U, new object[]
		{
			currentUnitPos,
			currentUnitRot
		});
		this.StateMachine.ChangeState(9);
	}

	public void StartMoveRPC(global::UnityEngine.Vector3 currentUnitPos, global::UnityEngine.Quaternion currentUnitRot)
	{
		base.transform.position = currentUnitPos;
		base.transform.rotation = currentUnitRot;
		this.SetAnimSpeed(0f);
		this.SetFixed(true);
		this.StateMachine.ChangeState(10);
	}

	public void SendTriggerActivated(global::UnityEngine.Vector3 unitPos, global::UnityEngine.Quaternion unitRot, global::TriggerPoint trigger)
	{
		this.Send(true, global::Hermes.SendTarget.ALL, this.uid, 13U, new object[]
		{
			unitPos,
			unitRot,
			global::PandoraSingleton<global::MissionManager>.Instance.triggerPoints.IndexOf(trigger)
		});
	}

	public void TriggerActivatedRPC(global::UnityEngine.Vector3 unitPos, global::UnityEngine.Quaternion unitRot, int triggerIdx)
	{
		base.transform.position = unitPos;
		base.transform.rotation = unitRot;
		this.SetAnimSpeed(0f);
		this.SetFixed(true);
		this.activeTrigger = global::PandoraSingleton<global::MissionManager>.Instance.triggerPoints[triggerIdx];
		this.nextState = ((!(this.activeTrigger is global::Trap)) ? global::UnitController.State.TELEPORT : global::UnitController.State.TRAPPED);
	}

	public void SendZoneAoeCross(global::ZoneAoe zone, bool entering, bool network)
	{
		int num = global::PandoraSingleton<global::MissionManager>.Instance.ZoneAoeIdx(zone);
		global::PandoraDebug.LogDebug(string.Concat(new object[]
		{
			"SendZoneAoeCross Zone = ",
			num,
			" entering = ",
			entering,
			" network = ",
			network
		}), "HERMES", this);
		network &= global::PandoraSingleton<global::MissionManager>.Instance.transitionDone;
		if (network)
		{
			this.Send(true, global::Hermes.SendTarget.ALL, this.uid, 15U, new object[]
			{
				num,
				entering
			});
		}
		else
		{
			this.ZoneAoeCrossRPC(num, entering);
		}
	}

	public void ZoneAoeCrossRPC(int zoneAoeIdx, bool entering)
	{
		global::PandoraDebug.LogDebug(string.Concat(new object[]
		{
			"ZoneAoeCrossRPC Zone = ",
			zoneAoeIdx,
			" entering = ",
			entering
		}), "HERMES", this);
		this.currentZoneAoe = global::PandoraSingleton<global::MissionManager>.Instance.GetZoneAoe(zoneAoeIdx);
		global::UnitController currentUnit = global::PandoraSingleton<global::MissionManager>.Instance.GetCurrentUnit();
		if (entering)
		{
			this.currentZoneAoe.AddToAffected(this);
			this.currentZoneAoe.ApplyEnchantments(this, true);
			this.lastActionWounds = 0;
			this.flyingLabel = string.Empty;
			if (global::PandoraSingleton<global::MissionManager>.Instance.transitionDone)
			{
				int enchantmentDamage = this.unit.GetEnchantmentDamage(global::PandoraSingleton<global::MissionManager>.Instance.NetworkTyche, global::EnchantmentDmgTriggerId.ON_APPLY);
				if (!global::PandoraSingleton<global::SequenceManager>.Instance.isPlaying)
				{
					global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<string>(global::Notices.COMBAT_RIGHT_MESSAGE, "skill_name_" + this.currentZoneAoe.Name);
					global::PandoraSingleton<global::UIMissionManager>.Instance.rightSequenceMessage.HideWithTimer();
				}
				bool flag = this.StateMachine.GetActiveStateId() == 38;
				bool flag2 = this.StateMachine.GetActiveStateId() == 41;
				this.unit.UpdateAttributes();
				this.attackerCtrlr = this.currentZoneAoe.Owner;
				if (enchantmentDamage > 0)
				{
					this.ComputeDirectWound(enchantmentDamage, true, this.currentZoneAoe.Owner, false);
					if (this.unit.Status != global::UnitStateId.OUT_OF_ACTION)
					{
						if (this.IsFixed && global::PandoraSingleton<global::MissionManager>.Instance.GetCurrentUnit() != this)
						{
							base.PlayDefState(global::AttackResultId.HIT, 0, this.unit.Status);
						}
						else
						{
							this.EventDisplayDamage();
						}
					}
					else
					{
						this.KillUnit();
						base.PlayDefState(global::AttackResultId.HIT, 0, global::UnitStateId.OUT_OF_ACTION);
						if (!global::PandoraSingleton<global::MissionManager>.Instance.CheckEndGame())
						{
							if (global::PandoraSingleton<global::MissionManager>.Instance.IsCurrentPlayer() && (this == currentUnit || flag || flag2))
							{
								this.SendStartMove(base.transform.position, base.transform.rotation);
								return;
							}
							if (this != currentUnit)
							{
								this.StateMachine.ChangeState(9);
							}
						}
					}
				}
				else if (this.unit.Status == global::UnitStateId.STUNNED)
				{
					base.PlayDefState(global::AttackResultId.HIT, 0, this.unit.Status);
					if (global::PandoraSingleton<global::MissionManager>.Instance.IsCurrentPlayer() && (this == currentUnit || flag || flag2) && !global::PandoraSingleton<global::SequenceManager>.Instance.isPlaying)
					{
						this.SendStartMove(base.transform.position, base.transform.rotation);
						return;
					}
				}
			}
			this.unit.UpdateAttributes();
		}
		else
		{
			this.currentZoneAoe.RemoveUnit(this, false);
			this.currentZoneAoe.ApplyEnchantments(this, false);
			this.unit.UpdateAttributes();
			this.currentZoneAoe = null;
		}
	}

	private void SendNewEngagedUnits(bool applyEnchants)
	{
		uint[] array = new uint[this.newEngagedUnits.Count];
		for (int i = 0; i < this.newEngagedUnits.Count; i++)
		{
			array[i] = this.newEngagedUnits[i].uid;
		}
		this.Send(true, global::Hermes.SendTarget.OTHERS, this.uid, 22U, new object[]
		{
			array,
			applyEnchants,
			this.wasEngaged
		});
	}

	private void NewEngagedUnitsRPC(uint[] engagedUids, bool applyEnchants, bool previouslyEngaged)
	{
		this.wasEngaged = previouslyEngaged;
		this.newEngagedUnits.Clear();
		for (int i = 0; i < engagedUids.Length; i++)
		{
			this.newEngagedUnits.Add(global::PandoraSingleton<global::MissionManager>.Instance.GetUnitController(engagedUids[i]));
		}
		this.ProcessEngagedUnits(applyEnchants);
	}

	public void SendEngaged(global::UnityEngine.Vector3 currentUnitPos, global::UnityEngine.Quaternion currentUnitRot, bool charge = false)
	{
		global::PandoraDebug.LogDebug(string.Concat(new object[]
		{
			"Sending EngagedRPC  Charge = ",
			charge,
			" pos = ",
			currentUnitPos,
			" rot = ",
			currentUnitRot
		}), "HERMES", this);
		this.Send(true, global::Hermes.SendTarget.ALL, this.uid, 11U, new object[]
		{
			currentUnitPos,
			currentUnitRot,
			charge
		});
		this.StateMachine.ChangeState(9);
	}

	public void EngagedRPC(global::UnityEngine.Vector3 currentUnitPos, global::UnityEngine.Quaternion currentUnitRot, bool charge)
	{
		global::PandoraDebug.LogDebug(string.Concat(new object[]
		{
			"EngagedRPC 1 Charge = ",
			charge,
			" pos = ",
			currentUnitPos,
			" rot = ",
			currentUnitRot
		}), "HERMES", this);
		base.transform.position = currentUnitPos;
		base.transform.rotation = currentUnitRot;
		this.SetAnimSpeed(0f);
		this.SetFixed(true);
		if (!charge)
		{
			this.ValidMove();
		}
		this.friendlyEntered.Clear();
		global::PandoraDebug.LogDebug(string.Concat(new object[]
		{
			"EngagedRPC Engaged Units = ",
			this.EngagedUnits.Count,
			" pos = ",
			base.transform.position,
			" rot = ",
			base.transform.rotation
		}), "HERMES", this);
		if (charge)
		{
			this.defenderCtrlr = this.EngagedUnits[0];
			this.FaceTarget(this.defenderCtrlr.transform, true);
			this.StateMachine.ChangeState(32);
		}
		else if (this.IsMine())
		{
			if (!this.wasEngaged)
			{
				this.wasEngaged = true;
				this.LaunchMelee((this.AICtrlr == null) ? global::UnitController.State.ENGAGED : global::UnitController.State.AI_CONTROLLED);
			}
			else
			{
				this.StateMachine.ChangeState((this.AICtrlr == null) ? 12 : 42);
			}
		}
		else if (!this.wasEngaged)
		{
			this.wasEngaged = true;
			this.LaunchMelee(global::UnitController.State.NET_CONTROLLED);
		}
		else
		{
			this.StateMachine.ChangeState(43);
		}
	}

	public void SendAskInterruption(global::UnitActionId actionId, uint senderUID, global::UnityEngine.Vector3 currentUnitPos, global::UnityEngine.Quaternion currentUnitRot)
	{
		global::PandoraDebug.LogInfo(string.Concat(new object[]
		{
			"SendAskInterruption Action",
			actionId,
			" targetID = ",
			senderUID,
			" Position = ",
			currentUnitPos
		}), "HERMES", this);
		this.Send(true, global::Hermes.SendTarget.HOST, this.uid, 17U, new object[]
		{
			(int)actionId,
			senderUID,
			currentUnitPos,
			currentUnitRot
		});
	}

	private void AskInterruption(global::UnitActionId actionId, uint senderUID, global::UnityEngine.Vector3 currentUnitPos, global::UnityEngine.Quaternion currentUnitRot)
	{
		if (global::PandoraSingleton<global::MissionManager>.Instance.interruptingUnit == null)
		{
			global::PandoraDebug.LogInfo(string.Concat(new object[]
			{
				"Unit ",
				base.name,
				" is interruption Action",
				actionId,
				" targetID = ",
				senderUID,
				" Position = ",
				currentUnitPos
			}), "HERMES", this);
			global::PandoraSingleton<global::MissionManager>.Instance.interruptingUnit = this;
			if (actionId != global::UnitActionId.AMBUSH)
			{
				if (actionId != global::UnitActionId.OVERWATCH)
				{
					global::PandoraDebug.LogError("Action " + actionId + " not supported for interruption!", "HERMES", this);
				}
				else
				{
					this.SendOverwatch(senderUID, currentUnitPos, currentUnitRot);
				}
			}
			else
			{
				this.SendAmbush(senderUID, currentUnitPos, currentUnitRot);
			}
		}
		else
		{
			global::PandoraDebug.LogInfo(string.Concat(new string[]
			{
				"Unit ",
				base.name,
				" tried to interrupt the flow but ",
				global::PandoraSingleton<global::MissionManager>.Instance.interruptingUnit.name,
				" is already interrupting"
			}), "HERMES", this);
		}
	}

	private void SendOverwatch(uint senderUID, global::UnityEngine.Vector3 currentUnitPos, global::UnityEngine.Quaternion currentUnitRot)
	{
		global::PandoraDebug.LogInfo(string.Concat(new object[]
		{
			"SendOverwatch ID = ",
			senderUID,
			" Position = ",
			currentUnitPos
		}), "HERMES", this);
		this.Send(true, global::Hermes.SendTarget.ALL, this.uid, 5U, new object[]
		{
			senderUID,
			currentUnitPos,
			currentUnitRot
		});
		this.StateMachine.ChangeState(9);
	}

	public void OverwatchRPC(uint senderUID, global::UnityEngine.Vector3 currentUnitPos, global::UnityEngine.Quaternion currentUnitRot)
	{
		global::System.Collections.Generic.List<global::UnitController> allUnits = global::PandoraSingleton<global::MissionManager>.Instance.GetAllUnits();
		for (int i = 0; i < allUnits.Count; i++)
		{
			if (allUnits[i].uid == senderUID)
			{
				allUnits[i].OverwatchRPC(this, currentUnitPos, currentUnitRot);
				break;
			}
		}
	}

	public void OverwatchRPC(global::UnitController target, global::UnityEngine.Vector3 currentUnitPos, global::UnityEngine.Quaternion currentUnitRot)
	{
		global::PandoraDebug.LogInfo("OverwatchRPC", "HERMES", this);
		this.defenderCtrlr = target;
		this.destructibleTarget = null;
		this.defenderCtrlr.SetAnimSpeed(0f);
		this.defenderCtrlr.transform.position = currentUnitPos;
		this.defenderCtrlr.transform.rotation = currentUnitRot;
		this.defenderCtrlr.SetFixed(true);
		this.defenderCtrlr.ValidMove();
		this.defenderCtrlr.CurrentAction = null;
		this.unit.ConsumeEnchantments(global::EnchantmentConsumeId.OVERWATCH);
		this.unit.ConsumeEnchantments(global::EnchantmentConsumeId.AMBUSH);
		this.defenderCtrlr.WaitForAction(global::UnitController.State.START_MOVE);
		this.yieldedPos = currentUnitPos;
		this.yieldedRot = currentUnitRot;
		this.yieldedSkillId = global::SkillId.BASE_OVERWATCH_ATTACK;
		base.StartCoroutine(this.LaunchYieldedAction());
	}

	private void SendAmbush(uint senderUID, global::UnityEngine.Vector3 currentUnitPos, global::UnityEngine.Quaternion currentUnitRot)
	{
		global::PandoraDebug.LogInfo("Send AmbushRPC", "HERMES", this);
		this.Send(true, global::Hermes.SendTarget.ALL, this.uid, 6U, new object[]
		{
			senderUID,
			currentUnitPos,
			currentUnitRot
		});
		this.StateMachine.ChangeState(9);
	}

	public void AmbushRPC(uint targetUID, global::UnityEngine.Vector3 currentUnitPos, global::UnityEngine.Quaternion currentUnitRot)
	{
		global::System.Collections.Generic.List<global::UnitController> aliveEnemies = global::PandoraSingleton<global::MissionManager>.Instance.GetAliveEnemies(this.GetWarband().idx);
		for (int i = 0; i < aliveEnemies.Count; i++)
		{
			if (aliveEnemies[i].uid == targetUID)
			{
				aliveEnemies[i].AmbushRPC(this, currentUnitPos, currentUnitRot);
				break;
			}
		}
	}

	public void AmbushRPC(global::UnitController target, global::UnityEngine.Vector3 currentUnitPos, global::UnityEngine.Quaternion currentUnitRot)
	{
		global::PandoraDebug.LogInfo("AmbushRPC", "HERMES", this);
		this.defenderCtrlr = target;
		this.destructibleTarget = null;
		this.defenderCtrlr.TriggerEnchantments(global::EnchantmentTriggerId.ON_AMBUSHED, global::SkillId.NONE, global::UnitActionId.NONE);
		global::PandoraSingleton<global::MissionManager>.Instance.HideCombatCircles();
		this.defenderCtrlr.transform.position = currentUnitPos;
		this.defenderCtrlr.transform.rotation = currentUnitRot;
		this.defenderCtrlr.SetAnimSpeed(0f);
		this.defenderCtrlr.SetFixed(true);
		this.defenderCtrlr.ValidMove();
		this.defenderCtrlr.CurrentAction = null;
		this.unit.ConsumeEnchantments(global::EnchantmentConsumeId.AMBUSH);
		this.defenderCtrlr.WaitForAction(global::UnitController.State.START_MOVE);
		this.yieldedPos = currentUnitPos;
		this.yieldedRot = currentUnitRot;
		this.yieldedSkillId = global::SkillId.BASE_AMBUSH_ATTACK;
		base.StartCoroutine(this.LaunchYieldedAction());
	}

	private global::System.Collections.IEnumerator LaunchYieldedAction()
	{
		yield return new global::UnityEngine.WaitForFixedUpdate();
		this.defenderCtrlr.SetAnimSpeed(0f);
		this.defenderCtrlr.transform.position = this.yieldedPos;
		this.defenderCtrlr.transform.rotation = this.yieldedRot;
		this.defenderCtrlr.SetFixed(true);
		global::PandoraDebug.LogInfo(string.Concat(new object[]
		{
			"LaunchYieldedAction ",
			this.yieldedSkillId,
			" Position 3 = ",
			this.defenderCtrlr.transform.position
		}), "HERMES", this);
		global::PandoraDebug.LogInfo(string.Concat(new object[]
		{
			"LaunchYieldedAction ",
			this.yieldedSkillId,
			" Rotation 3 = ",
			this.defenderCtrlr.transform.rotation
		}), "HERMES", this);
		this.defenderCtrlr.SetCombatCircle(this, true);
		this.SetCombatCircle(this, false);
		while (global::PandoraSingleton<global::MissionManager>.Instance.IsNavmeshUpdating)
		{
			yield return null;
		}
		this.SetCurrentAction(this.yieldedSkillId);
		this.CurrentAction.Activate();
		yield break;
	}

	public void SendInventoryTakeAll()
	{
		this.Send(true, global::Hermes.SendTarget.ALL, this.uid, 14U, new object[0]);
	}

	public void InventoryTakeAllRPC()
	{
		global::PandoraDebug.LogInfo("InventoryChangRPC", "HERMES", this);
		global::SearchPoint searchPoint = (global::SearchPoint)this.interactivePoint;
		global::Item.SortEmptyItems(searchPoint.items, 0);
		global::Item switchItem = new global::Item(global::ItemId.NONE, global::ItemQualityId.NORMAL);
		while (searchPoint.items.Count > 0 && searchPoint.items[0].Id != global::ItemId.NONE && searchPoint.CanSwitchItem(0, switchItem))
		{
			global::UnitSlotId unitSlotId;
			if (!this.unit.GetEmptyItemSlot(out unitSlotId, searchPoint.items[0]))
			{
				break;
			}
			global::PandoraSingleton<global::MissionManager>.Instance.focusedUnit.InventoryChangeRPC(0, unitSlotId - global::UnitSlotId.ITEM_1);
		}
		this.InventoryDoneRPC();
	}

	public void SendInventoryChange(int itemIndex, int slotIndex)
	{
		this.Send(true, global::Hermes.SendTarget.ALL, this.uid, 7U, new object[]
		{
			itemIndex,
			slotIndex
		});
	}

	public void InventoryChangeRPC(int itemIndex, int slotIndex)
	{
		global::PandoraDebug.LogInfo(string.Concat(new object[]
		{
			"InventoryChangeRPC - ",
			itemIndex,
			" - ",
			slotIndex
		}), "HERMES", this);
		global::Inventory inventory = (global::Inventory)this.StateMachine.GetState(15);
		inventory.PickupItem(itemIndex, slotIndex);
	}

	public void SendInventoryDone()
	{
		this.Send(true, global::Hermes.SendTarget.ALL, this.uid, 8U, new object[0]);
	}

	public void InventoryDoneRPC()
	{
		global::PandoraDebug.LogInfo("InventoryDoneRPC", "HERMES", this);
		global::SearchPoint searchPoint = (global::SearchPoint)this.interactivePoint;
		searchPoint.Close(false);
		global::PandoraSingleton<global::MissionManager>.Instance.UpdateObjectivesUI(false);
		if (this.IsPlayed() && searchPoint.unitController != null && !searchPoint.unitController.IsPlayed() && global::PandoraSingleton<global::MissionManager>.Instance.lootedEnemies.IndexOf(searchPoint) == -1)
		{
			global::PandoraSingleton<global::Hephaestus>.Instance.IncrementStat(global::Hephaestus.StatId.LOOT_ENEMIES, 1);
			global::PandoraSingleton<global::MissionManager>.Instance.lootedEnemies.Add(searchPoint);
		}
		if (global::PandoraSingleton<global::MissionManager>.Instance.CheckEndGame())
		{
			return;
		}
		global::PandoraSingleton<global::MissionManager>.Instance.MissionEndData.UpdateUnit(this);
		if (!global::PandoraSingleton<global::MissionManager>.Instance.MissionEndData.missionSave.isTuto && global::PandoraSingleton<global::GameManager>.Instance.currentSave != null)
		{
			global::PandoraSingleton<global::MissionManager>.Instance.MissionEndData.wagonItems.Clear();
			global::PandoraSingleton<global::MissionManager>.Instance.MissionEndData.wagonItems.AddItems(global::PandoraSingleton<global::MissionManager>.Instance.GetMyWarbandCtrlr().wagon.chest.GetItems());
		}
		base.StartCoroutine(this.WaitForSearchIdle());
	}

	private global::System.Collections.IEnumerator WaitForSearchIdle()
	{
		while (this.animator.GetCurrentAnimatorStateInfo(0).fullPathHash != global::AnimatorIds.search_idle && this.animator.GetCurrentAnimatorStateInfo(0).fullPathHash != global::AnimatorIds.interact_idle)
		{
			yield return 0;
		}
		global::PandoraSingleton<global::MissionManager>.Instance.PlaySequence("search", this, new global::DelSequenceDone(this.OnInvSeqDone));
		yield break;
	}

	private void OnInvSeqDone()
	{
		this.wyrdstoneRollModifier = 0;
		for (int i = 6; i < this.unit.Items.Count; i++)
		{
			global::Item item = this.unit.Items[i];
			if (!this.oldItems.Contains(item))
			{
				global::ItemId id = item.Id;
				if (id == global::ItemId.WYRDSTONE_FRAGMENT || id == global::ItemId.WYRDSTONE_SHARD || id == global::ItemId.WYRDSTONE_CLUSTER)
				{
					this.wyrdstoneRollModifier += global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::ItemAttributeData>(new string[]
					{
						"fk_item_id",
						"fk_attribute_id"
					}, new string[]
					{
						((int)id).ToConstantString(),
						55.ToConstantString()
					})[0].Modifier;
				}
			}
		}
		global::UnitController unitController = ((global::SearchPoint)this.interactivePoint).SpawnCampaignUnit();
		((global::SearchPoint)this.interactivePoint).ActivateZoneAoe();
		if (unitController != null && unitController.unit.Id == global::UnitId.MANTICORE)
		{
			unitController.transform.position = global::UnityEngine.Vector3.one * 10000f;
			if (global::PandoraSingleton<global::MissionManager>.Instance.IsCurrentPlayer())
			{
				unitController.SendSkill(global::SkillId.BASE_FLY);
			}
			this.WaitForAction(global::UnitController.State.START_MOVE);
		}
		else if (this.wyrdstoneRollModifier != 0)
		{
			this.TriggerEnchantments(global::EnchantmentTriggerId.ON_WYRDSTONE_PICKUP, global::SkillId.NONE, global::UnitActionId.NONE);
			this.currentSpellTypeId = global::SpellTypeId.WYRDSTONE;
			if (global::PandoraSingleton<global::MissionManager>.Instance.IsCurrentPlayer())
			{
				this.SendCurse();
			}
			else
			{
				this.StateMachine.ChangeState(9);
			}
		}
		else if (((global::SearchPoint)this.interactivePoint).ShouldTriggerCurse())
		{
			this.currentSpellTypeId = global::SpellTypeId.MISSION;
			this.currentCurseSkillId = this.interactivePoint.curseId;
			if (global::PandoraSingleton<global::MissionManager>.Instance.IsCurrentPlayer())
			{
				this.SendCurse();
			}
			else
			{
				this.StateMachine.ChangeState(9);
			}
		}
		else if (global::PandoraSingleton<global::MissionManager>.Instance.IsCurrentPlayer())
		{
			this.SendStartMove(base.transform.position, base.transform.rotation);
		}
		else
		{
			this.StateMachine.ChangeState(9);
		}
	}

	public void SendInteractiveAction(global::SkillId skillId, global::InteractivePoint point)
	{
		this.Send(true, global::Hermes.SendTarget.ALL, this.uid, 9U, new object[]
		{
			skillId,
			global::PandoraSingleton<global::MissionManager>.Instance.GetInteractivePointIndex(point)
		});
		this.StateMachine.ChangeState(9);
	}

	public void InteractiveRPC(int skillId, int actionZoneIdx)
	{
		global::PandoraDebug.LogInfo(string.Concat(new object[]
		{
			"AthleticRPC skillId = ",
			(global::SkillId)skillId,
			"Action zone index ",
			actionZoneIdx
		}), "HERMES", this);
		this.interactivePoint = global::PandoraSingleton<global::MissionManager>.Instance.GetInteractivePoint(actionZoneIdx);
		this.SetCurrentAction((global::SkillId)skillId);
		this.ValidMove();
		this.activeActionDest = null;
		global::UnitActionId actionId = this.CurrentAction.ActionId;
		if (actionId != global::UnitActionId.CLIMB)
		{
			if (actionId != global::UnitActionId.JUMP)
			{
				if (actionId == global::UnitActionId.LEAP)
				{
					this.activeActionDest = ((global::ActionZone)this.interactivePoint).GetLeap();
				}
			}
			else
			{
				this.activeActionDest = ((global::ActionZone)this.interactivePoint).GetJump();
			}
		}
		else
		{
			this.activeActionDest = ((global::ActionZone)this.interactivePoint).GetClimb();
		}
		if (this.activeActionDest != null)
		{
			this.FaceTarget(this.activeActionDest.destination.transform.position, true);
		}
		this.CurrentAction.Activate();
	}

	public void SendActionDone()
	{
		this.Send(true, global::Hermes.SendTarget.ALL, this.uid, 10U, new object[0]);
		this.StateMachine.ChangeState(9);
	}

	private void ActionDoneRPC()
	{
		global::PandoraDebug.LogInfo("ActionDoneRPC", "HERMES", this);
		this.unit.UpdateValidNextActionEnchantments();
		this.ActionDone();
	}

	public void EventSheathe()
	{
		if (this.Sheated || this.currentActionId == global::UnitActionId.SWITCH_WEAPONSET)
		{
			base.SwitchWeapons((!this.Sheated) ? this.unit.InactiveWeaponSlot : this.unit.ActiveWeaponSlot);
			this.Sheated = false;
		}
		else
		{
			if (base.Equipments[(int)this.unit.ActiveWeaponSlot] != null)
			{
				base.Equipments[(int)this.unit.ActiveWeaponSlot].gameObject.SetActive(true);
				base.Equipments[(int)this.unit.ActiveWeaponSlot].Sheathe(base.BonesTr, false, this.unit.Id);
			}
			if (base.Equipments[(int)(this.unit.ActiveWeaponSlot + 1)] != null)
			{
				base.Equipments[(int)(this.unit.ActiveWeaponSlot + 1)].gameObject.SetActive(true);
				base.Equipments[(int)(this.unit.ActiveWeaponSlot + 1)].Sheathe(base.BonesTr, true, this.unit.Id);
			}
			this.unit.currentAnimStyleId = global::AnimStyleId.NONE;
			base.SetAnimStyle();
			this.Sheated = true;
		}
	}

	public void EventHurt(int variation)
	{
		for (int i = 0; i < this.defenders.Count; i++)
		{
			if (this.defenders[i].attackResultId == global::AttackResultId.HIT_NO_WOUND || this.defenders[i].attackResultId == global::AttackResultId.HIT)
			{
				this.defenders[i].PlayDefState(this.defenders[i].attackResultId, variation, this.defenders[i].unit.Status);
				if (this.CurrentAction.fxData != null)
				{
					global::PandoraSingleton<global::Prometheus.Prometheus>.Instance.SpawnFx(this.CurrentAction.fxData.ImpactFx, this.defenders[i], null, null, default(global::UnityEngine.Vector3), default(global::UnityEngine.Vector3), null);
				}
				if (this.criticalHit && (this.IsPlayed() || this.defenderCtrlr.IsPlayed()))
				{
					global::PandoraSingleton<global::MissionManager>.Instance.CamManager.ActivateBloodSplatter();
				}
			}
		}
		for (int j = 0; j < this.destructTargets.Count; j++)
		{
			this.destructTargets[j].Hit(this);
		}
	}

	public void EventAvoid(int variation)
	{
		if (this.defenderCtrlr == null)
		{
			return;
		}
		for (int i = 0; i < this.defenders.Count; i++)
		{
			if ((this.defenders[i].attackResultId == global::AttackResultId.MISS || this.defenders[i].attackResultId == global::AttackResultId.DODGE) && this.defenders[i].unit.Status == global::UnitStateId.NONE)
			{
				this.defenders[i].PlayDefState(this.defenders[i].attackResultId, variation, this.defenders[i].unit.Status);
			}
		}
	}

	public void EventParry()
	{
		if (this.defenderCtrlr == null)
		{
			return;
		}
		for (int i = 0; i < this.defenders.Count; i++)
		{
			if (this.defenders[i].attackResultId == global::AttackResultId.PARRY)
			{
				this.defenders[i].PlayDefState(this.defenders[i].attackResultId, 0, this.defenders[i].unit.Status);
			}
		}
	}

	public void EventFx(string fxName)
	{
		global::PandoraSingleton<global::Prometheus.Prometheus>.Instance.SpawnFx(fxName, this, null, null, default(global::UnityEngine.Vector3), default(global::UnityEngine.Vector3), null);
	}

	public void EventSoundFoot(string soundName)
	{
		this.GetRandomSound(ref soundName, ref this.lastFoot);
		this.PlaySound(soundName);
	}

	public void EventShout(string soundName)
	{
		this.GetParamSound(ref soundName);
		this.GetRandomSound(ref soundName, ref this.lastShout);
		this.PlaySound(soundName);
	}

	public void EventSound(string soundName)
	{
		this.GetParamSound(ref soundName);
		this.PlaySound(soundName);
	}

	private void GetParamSound(ref string soundName)
	{
		while (soundName.IndexOf("(") != -1)
		{
			int num = soundName.IndexOf("(");
			int num2 = soundName.IndexOf(")");
			string text = soundName.Substring(num + 1, num2 - num - 1);
			string text2 = string.Empty;
			string text3 = text;
			switch (text3)
			{
			case "armor":
				text2 = this.unit.Items[1].Sound;
				text2 = ((!string.IsNullOrEmpty(text2)) ? text2 : "cloth");
				break;
			case "weapon":
				text2 = this.unit.Items[(int)this.unit.ActiveWeaponSlot].Sound;
				break;
			case "off_weapon":
				text2 = this.unit.Items[(int)(this.unit.ActiveWeaponSlot + 1)].Sound;
				break;
			case "atk_weapon":
				if (this.attackerCtrlr != null && this.attackerCtrlr.HasClose())
				{
					text2 = this.attackerCtrlr.unit.Items[(int)this.attackerCtrlr.unit.ActiveWeaponSlot].Sound;
				}
				break;
			case "atk_weapon_cat":
				if (this.attackerCtrlr != null && this.attackerCtrlr.HasClose())
				{
					text2 = this.attackerCtrlr.unit.Items[(int)this.attackerCtrlr.unit.ActiveWeaponSlot].SoundCat;
				}
				break;
			case "atk_proj":
				if (this.attackerCtrlr != null && this.attackerCtrlr.HasRange())
				{
					text2 = this.attackerCtrlr.unit.Items[(int)this.attackerCtrlr.unit.ActiveWeaponSlot].ProjectileData.Sound;
				}
				break;
			case "unit":
				text2 = this.unit.Data.Name;
				break;
			case "spell":
				text2 = this.CurrentAction.skillData.Name;
				break;
			case "atk_spell":
				if (global::PandoraSingleton<global::MissionManager>.Instance.focusedUnit != null && global::PandoraSingleton<global::MissionManager>.Instance.focusedUnit.CurrentAction != null)
				{
					text2 = global::PandoraSingleton<global::MissionManager>.Instance.focusedUnit.CurrentAction.skillData.Name;
				}
				break;
			case "skill":
				if (this.CurrentAction != null && this.CurrentAction.SkillId != global::SkillId.NONE)
				{
					text2 = this.CurrentAction.skillData.Name;
					text2 = text2.Replace("_mstr", string.Empty);
				}
				break;
			}
			IL_357:
			if (text2 == string.Empty)
			{
				return;
			}
			global::System.Text.StringBuilder stringBuilder = global::PandoraUtils.StringBuilder;
			stringBuilder.Append(soundName.Substring(0, num));
			stringBuilder.Append(text2.ToLower());
			stringBuilder.Append(soundName.Substring(num2 + 1, soundName.Length - num2 - 1));
			soundName = stringBuilder.ToString();
			continue;
			goto IL_357;
		}
	}

	private void GetRandomSound(ref string soundName, ref string lastSoundPlayed)
	{
		int num = int.Parse(soundName.Substring(soundName.Length - 1));
		string arg = soundName.Substring(0, soundName.Length - 1);
		int num2 = global::PandoraSingleton<global::GameManager>.Instance.LocalTyche.Rand(1, num + 1);
		string a = arg + num2;
		if (a == lastSoundPlayed)
		{
			num2 = (num2 + 1) % (num + 1);
			if (num2 == 0)
			{
				num2++;
			}
		}
		lastSoundPlayed = arg + num2;
		soundName = lastSoundPlayed;
	}

	private void PlaySound(string soundName)
	{
		if (this.audioSource == null)
		{
			return;
		}
		global::PandoraSingleton<global::Pan>.Instance.GetSound(soundName, true, delegate(global::UnityEngine.AudioClip clip)
		{
			if (clip != null)
			{
				this.audioSource.PlayOneShot(clip);
			}
		});
	}

	public void EventDisplayDamage()
	{
		if (!string.IsNullOrEmpty(this.flyingLabel) && this.Imprint.State == global::MapImprintStateId.VISIBLE)
		{
			global::PandoraSingleton<global::FlyingTextManager>.Instance.GetFlyingText((!(this.attackerCtrlr != null) || !this.attackerCtrlr.criticalHit) ? global::FlyingTextId.DAMAGE : global::FlyingTextId.DAMAGE_CRIT, delegate(global::FlyingText fl)
			{
				((global::FlyingLabel)fl).Play(base.BonesTr[global::BoneId.RIG_HEAD].localPosition, base.transform, false, this.flyingLabel, new string[0]);
			});
			global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<global::UnitController>(global::Notices.RETROACTION_SHOW_OUTCOME, this);
		}
	}

	public void EventDisplayActionOutcome()
	{
		if (!string.IsNullOrEmpty(this.currentActionData.actionOutcome) && this.Imprint.State == global::MapImprintStateId.VISIBLE && global::PandoraSingleton<global::TransitionManager>.Instance.GameLoadingDone)
		{
			global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<global::UnitController>(global::Notices.RETROACTION_SHOW_OUTCOME, this);
		}
	}

	public void EventDisplayStatusOutcome()
	{
		if (this.unit.Status != global::UnitStateId.NONE && this.unit.Status != this.unit.PreviousStatus && this.Imprint.State == global::MapImprintStateId.VISIBLE)
		{
			global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<global::UnitController, string>(global::Notices.RETROACTION_TARGET_STATUS, this, global::PandoraSingleton<global::LocalizationManager>.Instance.BuildStringAndLocalize("unit_state_", this.unit.Status.ToString(), null, null));
		}
	}

	public void EventTrail(int active)
	{
		bool activate = active != 0;
		string newColor = (this.currentAction.fxData == null) ? string.Empty : this.currentAction.fxData.TrailColor;
		if (base.Equipments[(int)this.unit.ActiveWeaponSlot] != null)
		{
			this.ActivateTrails(base.Equipments[(int)this.unit.ActiveWeaponSlot].trails, activate, newColor);
		}
		if (base.Equipments[(int)(this.unit.ActiveWeaponSlot + 1)] != null)
		{
			this.ActivateTrails(base.Equipments[(int)(this.unit.ActiveWeaponSlot + 1)].trails, activate, newColor);
		}
		this.ActivateTrails(this.bodyPartTrails, activate, newColor);
	}

	private void ActivateTrails(global::System.Collections.Generic.List<global::WeaponTrail> trails, bool activate, string newColor)
	{
		if (trails != null && trails.Count > 0)
		{
			global::UnityEngine.Color color = this.TRAIL_BASE_COLOR;
			if (activate && newColor != null && !string.IsNullOrEmpty(newColor))
			{
				color = global::PandoraUtils.StringToColor(newColor);
			}
			for (int i = 0; i < trails.Count; i++)
			{
				global::WeaponTrail weaponTrail = trails[i];
				weaponTrail.Emit(activate);
				if (activate && weaponTrail.GetMaterial().HasProperty("_Color"))
				{
					weaponTrail.GetMaterial().SetColor("_Color", color);
				}
			}
		}
	}

	public void EventShoot(int idx)
	{
		((global::RangeCombatFire)this.StateMachine.GetState(31)).ShootProjectile(idx);
	}

	public void EventSpellStart()
	{
		if (this.currentAction != null && this.currentAction.ActionId != global::UnitActionId.CHARGE && this.currentAction.ActionId != global::UnitActionId.AMBUSH && this.currentAction.fxData != null)
		{
			global::PandoraSingleton<global::Prometheus.Prometheus>.Instance.SpawnFx(this.CurrentAction.fxData.RightHandFx, this, null, null, default(global::UnityEngine.Vector3), default(global::UnityEngine.Vector3), null);
			global::PandoraSingleton<global::Prometheus.Prometheus>.Instance.SpawnFx(this.CurrentAction.fxData.LeftHandFx, this, null, null, default(global::UnityEngine.Vector3), default(global::UnityEngine.Vector3), null);
			global::PandoraSingleton<global::Prometheus.Prometheus>.Instance.SpawnFx(this.CurrentAction.fxData.ChargeFx, (!this.CurrentAction.fxData.ChargeOnTarget || !(this.defenderCtrlr != null)) ? this : this.defenderCtrlr, null, null, default(global::UnityEngine.Vector3), default(global::UnityEngine.Vector3), null);
		}
	}

	public void EventSkillLaunch()
	{
		if (this.currentAction != null && this.currentAction.fxData != null)
		{
			global::PandoraSingleton<global::Prometheus.Prometheus>.Instance.SpawnFx(this.CurrentAction.fxData.LaunchFx, this, null, null, default(global::UnityEngine.Vector3), default(global::UnityEngine.Vector3), null);
		}
	}

	public void EventSpellShoot()
	{
		if (this.IsCurrentState(global::UnitController.State.SKILL_USE))
		{
			((global::SkillUse)this.StateMachine.GetState(30)).ShootProjectile();
		}
		else if (this.IsCurrentState(global::UnitController.State.SPELL_CASTING))
		{
			((global::SpellCasting)this.StateMachine.GetState(28)).ShootProjectile();
		}
		else if (this.IsCurrentState(global::UnitController.State.PERCEPTION))
		{
			((global::Perception)this.StateMachine.GetState(13)).LaunchFx();
		}
	}

	public void EventZoneAoe()
	{
		this.currentSpellTargetPosition = base.transform.position;
		global::ZoneAoe.Spawn(this, this.GetAction(global::SkillId.BASE_FLY), null);
	}

	public void EventAttachProj(int variation)
	{
		if (variation == 0)
		{
			if (base.Equipments[(int)this.unit.ActiveWeaponSlot] != null)
			{
				global::UnityEngine.GameObject projectile = base.Equipments[(int)this.unit.ActiveWeaponSlot].GetProjectile();
				global::UnityEngine.MeshFilter componentInChildren = projectile.GetComponentInChildren<global::UnityEngine.MeshFilter>();
				projectile.transform.SetParent(base.BonesTr[global::BoneId.RIG_WEAPONR]);
				projectile.transform.localPosition = new global::UnityEngine.Vector3(0f, 0f, global::UnityEngine.Mathf.Max(new float[]
				{
					componentInChildren.mesh.bounds.extents.x,
					componentInChildren.mesh.bounds.extents.y,
					componentInChildren.mesh.bounds.extents.z
				}) * 2f);
				projectile.transform.localRotation = global::UnityEngine.Quaternion.identity;
			}
		}
		else if (base.Equipments[(int)this.unit.ActiveWeaponSlot] != null)
		{
			base.Equipments[(int)this.unit.ActiveWeaponSlot].AttachProjectile();
		}
	}

	public void EventReloadWeapons(int slot)
	{
		global::Reload reload = (global::Reload)this.StateMachine.GetState(19);
		reload.ReloadWeapon(slot);
	}

	public void EventWeaponAim()
	{
		global::RangeCombatFire rangeCombatFire = (global::RangeCombatFire)this.StateMachine.GetState(31);
		rangeCombatFire.WeaponAim();
	}

	public void EventInteract()
	{
		if (this.IsCurrentState(global::UnitController.State.SKILL_USE))
		{
			((global::SkillUse)this.StateMachine.GetState(30)).OnInteract();
		}
		if (this.IsCurrentState(global::UnitController.State.ACTIVATE))
		{
			((global::Activate)this.StateMachine.GetState(16)).ActivatePoint();
		}
		else if ((this.IsCurrentState(global::UnitController.State.SEARCH) || this.IsCurrentState(global::UnitController.State.INVENTORY)) && this.interactivePoint != null && this.interactivePoint is global::SearchPoint)
		{
			((global::SearchPoint)this.interactivePoint).Open();
		}
	}

	public override void EventDissolve()
	{
		this.Imprint.alwaysHide = true;
		this.Hide(true, false, null);
	}

	public override void Hide(bool hide, bool force = false, global::UnityEngine.Events.UnityAction onDissolved = null)
	{
		base.Hide(hide || this.Imprint.alwaysHide, force, onDissolved);
	}

	public void EventFly()
	{
		if (this.IsCurrentState(global::UnitController.State.FLY))
		{
			((global::Fly)this.StateMachine.GetState(48)).FlyToPoint();
		}
	}

	public int GetCRC()
	{
		int num = 0;
		for (int i = 0; i < base.Equipments.Count; i++)
		{
			if (base.Equipments[i] != null && base.Equipments[i].Item != null && base.Equipments[i].Item.Save != null)
			{
				num += base.Equipments[i].Item.Save.GetCRC(false);
			}
		}
		return num + this.unit.GetCRC();
	}

	public uint uid { get; set; }

	public uint owner { get; set; }

	public void RegisterToHermes()
	{
		global::PandoraSingleton<global::Hermes>.Instance.RegisterMyrtilus(this, false);
	}

	public void RemoveFromHermes()
	{
		global::PandoraSingleton<global::Hermes>.Instance.RemoveMyrtilus(this);
	}

	public void Send(bool reliable, global::Hermes.SendTarget target, uint id, uint command, params object[] parms)
	{
		if (command != 1U)
		{
			this.SendSpeedPosition();
			if (this.commandSent)
			{
				global::UnitController.Command item = default(global::UnitController.Command);
				item.reliable = reliable;
				item.target = target;
				item.from = (ulong)this.uid;
				item.command = command;
				item.parms = parms;
				this.commandsToSend.Enqueue(item);
				return;
			}
			this.commandSent = true;
		}
		global::PandoraSingleton<global::Hermes>.Instance.Send(reliable, target, id, command, new object[]
		{
			this.GetCRC(),
			parms
		});
	}

	public void Receive(ulong from, uint command, object[] parms)
	{
		global::UnitController.Command command2 = default(global::UnitController.Command);
		command2.from = from;
		command2.command = command;
		if (parms != null)
		{
			command2.parms = (object[])parms.Clone();
		}
		if (command == 1U)
		{
			this.RunCommand(command2);
		}
		else if (command == 17U)
		{
			if (this.CanLaunchCommand() && this.StateMachine.GetActiveStateId() != 38)
			{
				this.RunCommand(command2);
			}
		}
		else
		{
			int crc = this.GetCRC();
			this.commands.Enqueue(command2);
			global::PandoraDebug.LogInfo(string.Concat(new object[]
			{
				"Queuing Command = ",
				(global::UnitController.CommandList)command,
				" State = ",
				(global::UnitController.State)this.StateMachine.GetActiveStateId()
			}), "MYRTILUS", this);
		}
	}

	private void RunCommand(global::UnitController.Command com)
	{
		uint command = com.command;
		object[] parms = com.parms;
		if (command != 1U)
		{
			global::PandoraDebug.LogInfo(string.Concat(new object[]
			{
				"Running Command ",
				(global::UnitController.CommandList)command,
				" at pos = ",
				base.transform.position,
				" at rot = ",
				base.transform.rotation,
				" at state = ",
				(global::UnitController.State)this.StateMachine.GetActiveStateId()
			}), "MYRTILUS", this);
		}
		object[] array = (object[])parms[1];
		switch (command)
		{
		case 1U:
		{
			float speed = (float)array[0];
			global::UnityEngine.Quaternion rot = (global::UnityEngine.Quaternion)array[1];
			global::UnityEngine.Vector3 pos = (global::UnityEngine.Vector3)array[2];
			this.NetworkSyncRPC(speed, rot, pos);
			break;
		}
		case 2U:
		{
			int skillId = (int)array[0];
			this.SkillRPC(skillId);
			break;
		}
		case 3U:
		{
			int skillId2 = (int)array[0];
			uint targetUID = (uint)array[1];
			this.SkillSingleTargetRPC(skillId2, targetUID);
			break;
		}
		case 4U:
		{
			int skillId3 = (int)array[0];
			global::UnityEngine.Vector3 targetPos = (global::UnityEngine.Vector3)array[1];
			global::UnityEngine.Vector3 targetDir = (global::UnityEngine.Vector3)array[2];
			this.SkillTargetsRPC(skillId3, targetPos, targetDir);
			break;
		}
		case 5U:
		{
			uint senderUID = (uint)array[0];
			global::UnityEngine.Vector3 currentUnitPos = (global::UnityEngine.Vector3)array[1];
			global::UnityEngine.Quaternion currentUnitRot = (global::UnityEngine.Quaternion)array[2];
			this.OverwatchRPC(senderUID, currentUnitPos, currentUnitRot);
			break;
		}
		case 6U:
		{
			uint targetUID2 = (uint)array[0];
			global::UnityEngine.Vector3 currentUnitPos2 = (global::UnityEngine.Vector3)array[1];
			global::UnityEngine.Quaternion currentUnitRot2 = (global::UnityEngine.Quaternion)array[2];
			this.AmbushRPC(targetUID2, currentUnitPos2, currentUnitRot2);
			break;
		}
		case 7U:
		{
			int itemIndex = (int)array[0];
			int slotIndex = (int)array[1];
			this.InventoryChangeRPC(itemIndex, slotIndex);
			break;
		}
		case 8U:
			this.InventoryDoneRPC();
			break;
		case 9U:
		{
			int skillId4 = (int)array[0];
			int actionZoneIdx = (int)array[1];
			this.InteractiveRPC(skillId4, actionZoneIdx);
			break;
		}
		case 10U:
			this.ActionDoneRPC();
			break;
		case 11U:
		{
			global::UnityEngine.Vector3 currentUnitPos3 = (global::UnityEngine.Vector3)array[0];
			global::UnityEngine.Quaternion currentUnitRot3 = (global::UnityEngine.Quaternion)array[1];
			bool charge = (bool)array[2];
			this.EngagedRPC(currentUnitPos3, currentUnitRot3, charge);
			break;
		}
		case 12U:
		{
			global::UnityEngine.Vector3 currentUnitPos4 = (global::UnityEngine.Vector3)array[0];
			global::UnityEngine.Quaternion currentUnitRot4 = (global::UnityEngine.Quaternion)array[1];
			this.StartMoveRPC(currentUnitPos4, currentUnitRot4);
			break;
		}
		case 13U:
		{
			global::UnityEngine.Vector3 unitPos = (global::UnityEngine.Vector3)array[0];
			global::UnityEngine.Quaternion unitRot = (global::UnityEngine.Quaternion)array[1];
			int triggerIdx = (int)array[2];
			this.TriggerActivatedRPC(unitPos, unitRot, triggerIdx);
			break;
		}
		case 14U:
			this.InventoryTakeAllRPC();
			break;
		case 15U:
		{
			int zoneAoeIdx = (int)array[0];
			bool entering = (bool)array[1];
			this.ZoneAoeCrossRPC(zoneAoeIdx, entering);
			break;
		}
		case 16U:
			this.CurseRPC();
			break;
		case 17U:
		{
			global::UnitActionId actionId = (global::UnitActionId)((int)array[0]);
			uint senderUID2 = (uint)array[1];
			global::UnityEngine.Vector3 currentUnitPos5 = (global::UnityEngine.Vector3)array[2];
			global::UnityEngine.Quaternion currentUnitRot5 = (global::UnityEngine.Quaternion)array[3];
			this.AskInterruption(actionId, senderUID2, currentUnitPos5, currentUnitRot5);
			break;
		}
		case 18U:
		{
			uint targetUID3 = (uint)array[0];
			global::UnityEngine.Vector3 pos2 = (global::UnityEngine.Vector3)array[1];
			global::UnityEngine.Quaternion rot2 = (global::UnityEngine.Quaternion)array[2];
			this.MoveAndUpdateCircleRPC(targetUID3, pos2, rot2);
			break;
		}
		case 19U:
			this.AthleticRPC();
			break;
		case 20U:
		{
			bool success = (bool)array[0];
			int action = (int)array[1];
			this.AthleticFinishedRPC(success, action);
			break;
		}
		case 21U:
			this.FlyRPC();
			break;
		case 22U:
		{
			uint[] engagedUids = (uint[])array[0];
			bool applyEnchants = (bool)array[1];
			bool previouslyEngaged = (bool)array[2];
			this.NewEngagedUnitsRPC(engagedUids, applyEnchants, previouslyEngaged);
			break;
		}
		case 23U:
		{
			int skillId5 = (int)array[0];
			uint destructUID = (uint)array[1];
			this.SkillSingleDestructibleRPC(skillId5, destructUID);
			break;
		}
		}
	}

	public const float FACE_TARGET_TIME = 10f;

	public const float FACE_TARGET_SPEED = 100f;

	public const float TARGET_HEIGHT = 1.5f;

	public const float TARGET_HEIGHT_TARGET = 1.25f;

	public const float MAX_ENGAGED_HEIGHT = 1.9f;

	private const float MAX_CHARGE_HEIGHT = 2.5f;

	private const float CHARGE_COLLISION_HEIGHT_THRESHOLD = 0.5f;

	private const float COMBAT_CIRCLE_FADE_DIST = 80f;

	private const float COMBAT_IDLE_LERP_SPEED = 2f;

	private global::UnityEngine.Color DETECTED_COLOR = new global::UnityEngine.Color(0.933333337f, 1f, 0.396078438f);

	private global::UnityEngine.Color TRAIL_BASE_COLOR = new global::UnityEngine.Color(255f, 255f, 255f, 128f);

	[global::UnityEngine.HideInInspector]
	public global::UnitController.State nextState;

	private global::UnitController.State actionDoneNextState;

	private global::UnityEngine.Vector3 fixPosition;

	private global::UnityEngine.Rigidbody rigbody;

	public bool isInLadder = true;

	private bool killed;

	public global::System.Collections.Generic.List<global::ActionStatus> actionStatus;

	public global::System.Collections.Generic.List<global::ActionStatus> availableActionStatus = new global::System.Collections.Generic.List<global::ActionStatus>();

	private global::ActionStatus currentAction;

	[global::UnityEngine.HideInInspector]
	public global::System.Collections.Generic.List<global::UnitController> friendlyEntered = new global::System.Collections.Generic.List<global::UnitController>();

	public global::UnityEngine.Vector3 friendlyZoneEntryPoint;

	[global::UnityEngine.HideInInspector]
	public int lastActionWounds;

	[global::UnityEngine.HideInInspector]
	public global::ActionData currentActionData = new global::ActionData();

	[global::UnityEngine.HideInInspector]
	public string currentActionLabel;

	[global::UnityEngine.HideInInspector]
	public global::AttributeId currentAttributeRoll;

	[global::UnityEngine.HideInInspector]
	public string flyingLabel;

	[global::UnityEngine.HideInInspector]
	public string actionOutcomeLabel;

	[global::UnityEngine.HideInInspector]
	public bool ladderVisible;

	private readonly global::System.Collections.Generic.List<global::FlyingTarget> flyingOverviews = new global::System.Collections.Generic.List<global::FlyingTarget>();

	[global::UnityEngine.HideInInspector]
	public global::System.Collections.Generic.List<global::InteractivePoint> interactivePoints = new global::System.Collections.Generic.List<global::InteractivePoint>();

	[global::UnityEngine.HideInInspector]
	public global::System.Collections.Generic.List<global::Destructible> triggeredDestructibles = new global::System.Collections.Generic.List<global::Destructible>();

	[global::UnityEngine.HideInInspector]
	public global::InteractivePoint interactivePoint;

	[global::UnityEngine.HideInInspector]
	public global::InteractiveTarget prevInteractiveTarget;

	[global::UnityEngine.HideInInspector]
	public global::InteractiveTarget nextInteractiveTarget;

	[global::UnityEngine.HideInInspector]
	public global::ActionDestination activeActionDest;

	[global::UnityEngine.HideInInspector]
	public global::UnityEngine.Transform failedTarget;

	[global::UnityEngine.HideInInspector]
	public global::TriggerPoint activeTrigger;

	[global::UnityEngine.HideInInspector]
	public global::Teleporter currentTeleporter;

	[global::UnityEngine.HideInInspector]
	public global::ZoneAoe currentZoneAoe;

	[global::UnityEngine.HideInInspector]
	public global::UnitController attackerCtrlr;

	[global::UnityEngine.HideInInspector]
	public global::System.Collections.Generic.List<global::UnitController> defenders;

	[global::UnityEngine.HideInInspector]
	public global::System.Collections.Generic.List<global::Destructible> destructTargets = new global::System.Collections.Generic.List<global::Destructible>();

	[global::UnityEngine.HideInInspector]
	public global::AttackResultId attackResultId;

	[global::UnityEngine.HideInInspector]
	public global::EffectTypeId buffResultId;

	[global::UnityEngine.HideInInspector]
	public bool criticalHit;

	[global::UnityEngine.HideInInspector]
	public int attackUsed;

	[global::UnityEngine.HideInInspector]
	public bool beenShot;

	private bool wasMaxWound;

	[global::UnityEngine.HideInInspector]
	public bool wasEngaged;

	[global::UnityEngine.HideInInspector]
	public global::DynamicCombatCircle combatCircle;

	[global::UnityEngine.HideInInspector]
	public global::DynamicChargeCircle chargeCircle;

	[global::UnityEngine.HideInInspector]
	public global::System.Collections.Generic.List<global::UnitController> chargeTargets;

	[global::UnityEngine.HideInInspector]
	public global::Prometheus.OlympusFire chargeFx;

	[global::UnityEngine.HideInInspector]
	public global::System.Collections.Generic.List<global::UnitController> chargePreviousTargets;

	private global::UnityEngine.RaycastHit raycastHitInfo;

	[global::UnityEngine.HideInInspector]
	public global::System.Collections.Generic.List<global::TargetData> targetsData = new global::System.Collections.Generic.List<global::TargetData>();

	public global::System.Collections.Generic.List<global::BoneTarget> boneTargets;

	[global::UnityEngine.HideInInspector]
	public global::UnityEngine.Transform hitPoint;

	[global::UnityEngine.HideInInspector]
	public global::UnityEngine.Vector3 startPosition;

	[global::UnityEngine.HideInInspector]
	public global::UnityEngine.Quaternion startRotation;

	[global::UnityEngine.HideInInspector]
	public int searchVariation;

	[global::UnityEngine.HideInInspector]
	public global::SearchPoint lootBagPoint;

	public global::System.Collections.Generic.List<global::Item> oldItems;

	[global::UnityEngine.HideInInspector]
	public global::UnityEngine.Vector3 currentSpellTargetPosition;

	[global::UnityEngine.HideInInspector]
	public global::SpellTypeId currentSpellTypeId;

	[global::UnityEngine.HideInInspector]
	public global::SkillId currentSpellId;

	[global::UnityEngine.HideInInspector]
	public bool currentSpellSuccess;

	[global::UnityEngine.HideInInspector]
	public global::SkillId currentCurseSkillId;

	[global::UnityEngine.HideInInspector]
	public int wyrdstoneRollModifier;

	[global::UnityEngine.HideInInspector]
	public float fleeDistanceMultiplier;

	[global::UnityEngine.HideInInspector]
	public float lastTimer;

	[global::UnityEngine.HideInInspector]
	public bool isCaptainMorganing;

	private global::System.Collections.Generic.Queue<global::UnitController.Command> commands;

	private bool commandSent;

	private global::System.Collections.Generic.Queue<global::UnitController.Command> commandsToSend = new global::System.Collections.Generic.Queue<global::UnitController.Command>();

	private bool hasBeenSpotted;

	[global::UnityEngine.HideInInspector]
	public global::System.Collections.Generic.List<global::UnitController> detectedUnits;

	[global::UnityEngine.HideInInspector]
	public global::System.Collections.Generic.List<global::TriggerPoint> detectedTriggers;

	[global::UnityEngine.HideInInspector]
	public global::System.Collections.Generic.List<global::InteractivePoint> detectedInteractivePoints;

	[global::UnityEngine.HideInInspector]
	public int recoveryTarget;

	[global::UnityEngine.HideInInspector]
	public int[] MVUptsPerCategory = new int[5];

	public global::UnityEngine.Quaternion newRotation = global::UnityEngine.Quaternion.identity;

	[global::UnityEngine.HideInInspector]
	public global::System.Collections.Generic.List<global::SearchPoint> linkedSearchPoints;

	[global::UnityEngine.HideInInspector]
	public bool unlockSearchPointOnDeath;

	[global::UnityEngine.HideInInspector]
	public bool reviveUntilSearchEmpty;

	[global::UnityEngine.HideInInspector]
	public global::System.Collections.Generic.List<global::DecisionPoint> forcedSpawnPoints;

	[global::UnityEngine.HideInInspector]
	public bool spawnedOnDeath;

	public int hitMod;

	private global::UnityEngine.Quaternion faceTargetRotation;

	private bool hadTerror;

	private bool hadFear;

	private bool defHadTerror;

	private bool defHadFear;

	private int lastChargeMvt = -1;

	private global::System.Collections.Generic.List<global::UnityEngine.Collider> chargeValidColliders = new global::System.Collections.Generic.List<global::UnityEngine.Collider>();

	private float currentAnimSpeed;

	private global::System.Collections.Generic.List<global::UnitController> newEngagedUnits = new global::System.Collections.Generic.List<global::UnitController>();

	private global::System.Collections.Generic.List<global::UnitController> modifiedUnits = new global::System.Collections.Generic.List<global::UnitController>();

	private global::System.Collections.Generic.List<global::UnitController> involvedUnits = new global::System.Collections.Generic.List<global::UnitController>();

	private global::System.Collections.Generic.List<global::UnityEngine.MonoBehaviour> availableTargets = new global::System.Collections.Generic.List<global::UnityEngine.MonoBehaviour>();

	private global::UnityEngine.Vector3 yieldedPos;

	private global::UnityEngine.Quaternion yieldedRot;

	private global::SkillId yieldedSkillId;

	public enum State
	{
		NONE,
		TURN_START,
		TURN_MESSAGE,
		UPDATE_EFFECTS,
		RECOVERY,
		TERROR,
		PERSONAL_ROUT,
		FEAR_CHECK,
		STUPIDITY,
		IDLE,
		START_MOVE,
		MOVE,
		ENGAGED,
		PERCEPTION,
		SEARCH,
		INVENTORY,
		ACTIVATE,
		TRAPPED,
		TELEPORT,
		RELOAD,
		DISENGAGE,
		SWITCH_WEAPON,
		DELAY,
		INTERACTIVE_TARGET,
		SINGLE_TARGETING,
		AOE_TARGETING,
		CONE_TARGETING,
		LINE_TARGETING,
		SPELL_CASTING,
		SPELL_CURSE,
		SKILL_USE,
		RANGE_COMBAT_FIRE,
		CLOSE_COMBAT_ATTACK,
		ACTION_WAIT,
		COUNTER_CHOICE,
		ACTIVATE_STANCE,
		OVERWATCH,
		AMBUSH,
		CHARGE,
		TURN_FINISHED,
		FLEE,
		FLEE_MOVE,
		AI_CONTROLLED,
		NET_CONTROLLED,
		OVERVIEW,
		ATHLETIC_COUNTER,
		PREPARE_ATHLETIC,
		ATHLETIC,
		FLY,
		PREPARE_FLY,
		ARC_TARGETING,
		NB_STATE
	}

	public enum CounterChoiceId
	{
		NONE,
		COUNTER,
		NO_COUNTER
	}

	[global::UnityEngine.HideInInspector]
	public enum CommandList
	{
		NONE,
		NETWORK_SYNC,
		SKILL,
		SKILL_SINGLE_TARGET,
		SKILL_MULTIPLE_TARGETS,
		OVERWATCH,
		AMBUSH,
		INVENTORY_CHANGE,
		INVENTORY_DONE,
		INTERACTIVE,
		ACTION_DONE,
		ENGAGED,
		START_MOVE,
		TRAPPED,
		INVENTORY_TAKE_ALL,
		ZONE_AOE,
		CURSE,
		ASK_INTERRUPTION,
		MOVE_AND_UPDATE_CIRCLE,
		ATHLETIC,
		ATHLETIC_FINISHED,
		FLY,
		ENGAGED_UNITS,
		SKILL_SINGLE_DESTRUCTIBLE,
		COUNT
	}

	private struct Command
	{
		public bool reliable;

		public global::Hermes.SendTarget target;

		public ulong from;

		public uint command;

		public object[] parms;
	}
}
