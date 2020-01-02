using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding.Serialization.JsonFx;
using UnityEngine;

public class AnimatorPlayer : global::PandoraSingleton<global::AnimatorPlayer>
{
	private void Awake()
	{
		global::PandoraSingleton<global::PandoraInput>.Instance.SetCurrentState(global::PandoraInput.States.MISSION, true);
	}

	private void Start()
	{
		this.init = false;
		this.sheated = false;
		this.knocked = false;
		this.currentUnit = string.Empty;
		this.weaponStyle = global::AnimStyleId.NONE;
		this.idleHash = global::UnityEngine.Animator.StringToHash("Base Layer.idle");
		this.sequences = new global::System.Collections.Generic.List<string>();
	}

	private void Update()
	{
		if (this.textAsset && !this.init)
		{
			string text = this.textAsset.text;
			this.animatorsData = new global::System.Collections.Generic.Dictionary<string, global::System.Collections.Generic.Dictionary<string, string[]>>();
			this.animatorsData = global::Pathfinding.Serialization.JsonFx.JsonReader.Deserialize<global::System.Collections.Generic.Dictionary<string, global::System.Collections.Generic.Dictionary<string, string[]>>>(text);
			global::UnityEngine.Debug.Log("json deserialized");
			this.units = new global::System.Collections.Generic.Dictionary<string, global::UnityEngine.GameObject>();
			foreach (string text2 in this.animatorsData.Keys)
			{
				this.unitObject = global::UnityEngine.GameObject.Find(text2 + "_menu");
				if (this.unitObject != null)
				{
					this.units[text2] = this.unitObject;
					this.unitObject.SetActive(false);
				}
				else
				{
					this.unitObject = global::UnityEngine.GameObject.Find(text2 + "_01_menu");
					if (this.unitObject != null)
					{
						this.units[text2] = this.unitObject;
						this.unitObject.SetActive(false);
					}
				}
			}
			this.unitObject = null;
			this.init = true;
		}
		if (this.unitObject)
		{
			this.unitObject.transform.position = global::UnityEngine.Vector3.zero;
			this.unitObject.transform.rotation = global::UnityEngine.Quaternion.identity;
		}
	}

	private void LateUpdate()
	{
		if (this.unitObject)
		{
			this.unitObject.transform.position = global::UnityEngine.Vector3.zero;
			this.unitObject.transform.rotation = global::UnityEngine.Quaternion.identity;
		}
	}

	private bool HasSpecialAnims()
	{
		return !this.ctrlr.unit.IsImpressive && !this.ctrlr.unit.IsMonster;
	}

	private void OnGUI()
	{
		if (global::UnityEngine.GUI.Button(new global::UnityEngine.Rect(0f, 0f, 50f, 30f), "Back"))
		{
			global::UnityEngine.Application.LoadLevel("main_menu");
		}
		if (this.init)
		{
			string text = this.currentUnit;
			int count = this.animatorsData.Count;
			int num = 0;
			foreach (string text2 in this.animatorsData.Keys)
			{
				int num2 = (num <= 5) ? 0 : 5;
				if (global::UnityEngine.GUI.Button(new global::UnityEngine.Rect((float)(global::UnityEngine.Screen.width - (num + 1 - num2) * 200), (float)((num <= 5) ? 0 : 35), 200f, 30f), text2))
				{
					text = text2;
				}
				num++;
			}
			global::UnityEngine.GUI.Label(new global::UnityEngine.Rect((float)(global::UnityEngine.Screen.width / 2 - 90), 75f, 90f, 30f), "Time Scale :");
			this.timeScale = global::UnityEngine.GUI.HorizontalSlider(new global::UnityEngine.Rect((float)(global::UnityEngine.Screen.width / 2), 80f, 100f, 30f), this.timeScale, 0f, 2f);
			global::UnityEngine.Time.timeScale = this.timeScale;
			if (global::UnityEngine.GUI.Button(new global::UnityEngine.Rect((float)(global::UnityEngine.Screen.width / 2 + 110), 75f, 22f, 22f), "R"))
			{
				this.timeScale = 1f;
			}
			if (text != string.Empty && text != this.currentUnit)
			{
				if (this.unitObject != null)
				{
					this.unitObject.SetActive(false);
					this.unitObject = null;
				}
				if (this.units.ContainsKey(text))
				{
					this.currentUnit = text;
					this.unitObject = this.units[text];
					this.unitObject.SetActive(true);
					this.ctrlr = this.unitObject.GetComponent<global::UnitMenuController>();
					this.animator = this.unitObject.GetComponent<global::UnityEngine.Animator>();
					this.unitObject.transform.position = global::UnityEngine.Vector3.zero;
					this.unitObject.transform.rotation = global::UnityEngine.Quaternion.identity;
					global::UnityEngine.Camera.main.GetComponent<global::CharacterFollowCamHack>().SetTarget(this.unitObject.transform);
					this.currentState = -1;
					this.sheated = false;
					this.knocked = false;
					this.weaponStyle = global::AnimStyleId.NONE;
					this.ctrlr.unit.currentAnimStyleId = this.weaponStyle;
					this.InitSequences();
					this.EquipWeapon();
				}
			}
			if (this.unitObject != null)
			{
				global::UnityEngine.GUI.Label(new global::UnityEngine.Rect((float)(global::UnityEngine.Screen.width / 2 - 100), (float)(global::UnityEngine.Screen.height - 30), 100f, 30f), "Running Speed");
				this.runningValue = global::UnityEngine.GUI.HorizontalSlider(new global::UnityEngine.Rect((float)(global::UnityEngine.Screen.width / 2), (float)(global::UnityEngine.Screen.height - 25), 100f, 30f), this.runningValue, 0f, 1f);
				this.animator.SetFloat(global::AnimatorIds.speed, this.runningValue);
				if (global::UnityEngine.GUI.Button(new global::UnityEngine.Rect((float)(global::UnityEngine.Screen.width / 2 - 100), (float)(global::UnityEngine.Screen.height - 75), 150f, 30f), "Combat Idle"))
				{
					this.runningValue = -1f;
					this.animator.SetFloat(global::AnimatorIds.speed, -1f);
				}
				if (global::UnityEngine.GUI.Button(new global::UnityEngine.Rect((float)(global::UnityEngine.Screen.width / 2 + 50), (float)(global::UnityEngine.Screen.height - 75), 150f, 30f), "Kneeling"))
				{
					this.knocked = !this.knocked;
					this.InitSequences();
				}
				this.SetWeaponStyleWeapons();
				global::UnityEngine.GUI.Label(new global::UnityEngine.Rect((float)(global::UnityEngine.Screen.width - 250), (float)(global::UnityEngine.Screen.height / 2 - 230), 250f, 30f), this.weaponStyle.ToString());
				this.currentState = -1;
				this.currentState = global::UnityEngine.GUI.SelectionGrid(new global::UnityEngine.Rect((float)(global::UnityEngine.Screen.width - 400), (float)global::UnityEngine.Screen.height / 2f - 200f, 400f, (float)global::UnityEngine.Screen.height / 2f + 200f), this.currentState, this.sequences.ToArray(), 2);
				if (this.currentState != -1)
				{
					this.PlaySequence(this.sequences[this.currentState]);
				}
			}
		}
	}

	public void SetWeaponStyleWeapons()
	{
		string text = this.currentUnit;
		switch (text)
		{
		case "ska_base":
			if (global::UnityEngine.GUI.Button(new global::UnityEngine.Rect(0f, (float)(global::UnityEngine.Screen.height - 30), 250f, 30f), global::AnimStyleId.ONE_HAND_NO_SHIELD.ToString()))
			{
				this.weaponStyle = global::AnimStyleId.ONE_HAND_NO_SHIELD;
				this.EquipWeapon();
				this.InitSequences();
			}
			if (global::UnityEngine.GUI.Button(new global::UnityEngine.Rect(0f, (float)(global::UnityEngine.Screen.height - 60), 250f, 30f), global::AnimStyleId.ONE_HAND_SHIELD.ToString()))
			{
				this.weaponStyle = global::AnimStyleId.ONE_HAND_SHIELD;
				this.EquipWeapon();
				this.InitSequences();
			}
			if (global::UnityEngine.GUI.Button(new global::UnityEngine.Rect(0f, (float)(global::UnityEngine.Screen.height - 90), 250f, 30f), global::AnimStyleId.SPEAR_NO_SHIELD.ToString()))
			{
				this.weaponStyle = global::AnimStyleId.SPEAR_NO_SHIELD;
				this.EquipWeapon();
				this.InitSequences();
			}
			if (global::UnityEngine.GUI.Button(new global::UnityEngine.Rect(0f, (float)(global::UnityEngine.Screen.height - 120), 250f, 30f), global::AnimStyleId.SPEAR_SHIELD.ToString()))
			{
				this.weaponStyle = global::AnimStyleId.SPEAR_SHIELD;
				this.EquipWeapon();
				this.InitSequences();
			}
			if (global::UnityEngine.GUI.Button(new global::UnityEngine.Rect(0f, (float)(global::UnityEngine.Screen.height - 150), 250f, 30f), global::AnimStyleId.CLAW.ToString()))
			{
				this.weaponStyle = global::AnimStyleId.CLAW;
				this.EquipWeapon();
				this.InitSequences();
			}
			if (global::UnityEngine.GUI.Button(new global::UnityEngine.Rect(0f, (float)(global::UnityEngine.Screen.height - 180), 250f, 30f), global::AnimStyleId.DUAL_WIELD.ToString()))
			{
				this.weaponStyle = global::AnimStyleId.DUAL_WIELD;
				this.EquipWeapon();
				this.InitSequences();
			}
			if (global::UnityEngine.GUI.Button(new global::UnityEngine.Rect(0f, (float)(global::UnityEngine.Screen.height - 210), 250f, 30f), global::AnimStyleId.HALBERD.ToString()))
			{
				this.weaponStyle = global::AnimStyleId.HALBERD;
				this.EquipWeapon();
				this.InitSequences();
			}
			if (global::UnityEngine.GUI.Button(new global::UnityEngine.Rect(0f, (float)(global::UnityEngine.Screen.height - 240), 250f, 30f), global::AnimStyleId.DUAL_PISTOL.ToString()))
			{
				this.weaponStyle = global::AnimStyleId.DUAL_PISTOL;
				this.EquipWeapon();
				this.InitSequences();
			}
			break;
		case "ska_ogre_base":
		case "pos_ogre_base":
		case "mon_horror_base":
		case "mon_daemonette_base":
			if (global::UnityEngine.GUI.Button(new global::UnityEngine.Rect(0f, (float)(global::UnityEngine.Screen.height - 30), 250f, 30f), global::AnimStyleId.NONE.ToString()))
			{
				this.weaponStyle = global::AnimStyleId.NONE;
				this.EquipWeapon();
				this.InitSequences();
			}
			break;
		case "mer_base":
			if (global::UnityEngine.GUI.Button(new global::UnityEngine.Rect(0f, (float)(global::UnityEngine.Screen.height - 30), 250f, 30f), global::AnimStyleId.ONE_HAND_NO_SHIELD.ToString()))
			{
				this.weaponStyle = global::AnimStyleId.ONE_HAND_NO_SHIELD;
				this.EquipWeapon();
				this.InitSequences();
			}
			if (global::UnityEngine.GUI.Button(new global::UnityEngine.Rect(0f, (float)(global::UnityEngine.Screen.height - 60), 250f, 30f), global::AnimStyleId.ONE_HAND_SHIELD.ToString()))
			{
				this.weaponStyle = global::AnimStyleId.ONE_HAND_SHIELD;
				this.EquipWeapon();
				this.InitSequences();
			}
			if (global::UnityEngine.GUI.Button(new global::UnityEngine.Rect(0f, (float)(global::UnityEngine.Screen.height - 90), 250f, 30f), global::AnimStyleId.SPEAR_NO_SHIELD.ToString()))
			{
				this.weaponStyle = global::AnimStyleId.SPEAR_NO_SHIELD;
				this.EquipWeapon();
				this.InitSequences();
			}
			if (global::UnityEngine.GUI.Button(new global::UnityEngine.Rect(0f, (float)(global::UnityEngine.Screen.height - 120), 250f, 30f), global::AnimStyleId.SPEAR_SHIELD.ToString()))
			{
				this.weaponStyle = global::AnimStyleId.SPEAR_SHIELD;
				this.EquipWeapon();
				this.InitSequences();
			}
			if (global::UnityEngine.GUI.Button(new global::UnityEngine.Rect(0f, (float)(global::UnityEngine.Screen.height - 150), 250f, 30f), global::AnimStyleId.DUAL_WIELD.ToString()))
			{
				this.weaponStyle = global::AnimStyleId.DUAL_WIELD;
				this.EquipWeapon();
				this.InitSequences();
			}
			if (global::UnityEngine.GUI.Button(new global::UnityEngine.Rect(0f, (float)(global::UnityEngine.Screen.height - 180), 250f, 30f), global::AnimStyleId.TWO_HANDED.ToString()))
			{
				this.weaponStyle = global::AnimStyleId.TWO_HANDED;
				this.EquipWeapon();
				this.InitSequences();
			}
			if (global::UnityEngine.GUI.Button(new global::UnityEngine.Rect(0f, (float)(global::UnityEngine.Screen.height - 210), 250f, 30f), global::AnimStyleId.HALBERD.ToString()))
			{
				this.weaponStyle = global::AnimStyleId.HALBERD;
				this.EquipWeapon();
				this.InitSequences();
			}
			if (global::UnityEngine.GUI.Button(new global::UnityEngine.Rect(0f, (float)(global::UnityEngine.Screen.height - 240), 250f, 30f), global::AnimStyleId.WARHAMMER.ToString()))
			{
				this.weaponStyle = global::AnimStyleId.WARHAMMER;
				this.EquipWeapon();
				this.InitSequences();
			}
			if (global::UnityEngine.GUI.Button(new global::UnityEngine.Rect(0f, (float)(global::UnityEngine.Screen.height - 270), 250f, 30f), global::AnimStyleId.DUAL_PISTOL.ToString()))
			{
				this.weaponStyle = global::AnimStyleId.DUAL_PISTOL;
				this.EquipWeapon();
				this.InitSequences();
			}
			if (global::UnityEngine.GUI.Button(new global::UnityEngine.Rect(0f, (float)(global::UnityEngine.Screen.height - 300), 250f, 30f), global::AnimStyleId.BOW.ToString()))
			{
				this.weaponStyle = global::AnimStyleId.BOW;
				this.EquipWeapon();
				this.InitSequences();
			}
			if (global::UnityEngine.GUI.Button(new global::UnityEngine.Rect(0f, (float)(global::UnityEngine.Screen.height - 330), 250f, 30f), global::AnimStyleId.CROSSBOW.ToString()))
			{
				this.weaponStyle = global::AnimStyleId.CROSSBOW;
				this.EquipWeapon();
				this.InitSequences();
			}
			if (global::UnityEngine.GUI.Button(new global::UnityEngine.Rect(0f, (float)(global::UnityEngine.Screen.height - 360), 250f, 30f), global::AnimStyleId.RIFLE.ToString()))
			{
				this.weaponStyle = global::AnimStyleId.RIFLE;
				this.EquipWeapon();
				this.InitSequences();
			}
			break;
		case "mer_ogre_base":
			if (global::UnityEngine.GUI.Button(new global::UnityEngine.Rect(0f, (float)(global::UnityEngine.Screen.height - 30), 250f, 30f), global::AnimStyleId.ONE_HAND_NO_SHIELD.ToString()))
			{
				this.weaponStyle = global::AnimStyleId.ONE_HAND_NO_SHIELD;
				this.EquipWeapon();
				this.InitSequences();
			}
			if (global::UnityEngine.GUI.Button(new global::UnityEngine.Rect(0f, (float)(global::UnityEngine.Screen.height - 60), 250f, 30f), global::AnimStyleId.DUAL_WIELD.ToString()))
			{
				this.weaponStyle = global::AnimStyleId.DUAL_WIELD;
				this.EquipWeapon();
				this.InitSequences();
			}
			if (global::UnityEngine.GUI.Button(new global::UnityEngine.Rect(0f, (float)(global::UnityEngine.Screen.height - 90), 250f, 30f), global::AnimStyleId.WARHAMMER.ToString()))
			{
				this.weaponStyle = global::AnimStyleId.WARHAMMER;
				this.EquipWeapon();
				this.InitSequences();
			}
			break;
		case "sis_base":
			if (global::UnityEngine.GUI.Button(new global::UnityEngine.Rect(0f, (float)(global::UnityEngine.Screen.height - 30), 250f, 30f), global::AnimStyleId.ONE_HAND_NO_SHIELD.ToString()))
			{
				this.weaponStyle = global::AnimStyleId.ONE_HAND_NO_SHIELD;
				this.EquipWeapon();
				this.InitSequences();
			}
			if (global::UnityEngine.GUI.Button(new global::UnityEngine.Rect(0f, (float)(global::UnityEngine.Screen.height - 60), 250f, 30f), global::AnimStyleId.ONE_HAND_SHIELD.ToString()))
			{
				this.weaponStyle = global::AnimStyleId.ONE_HAND_SHIELD;
				this.EquipWeapon();
				this.InitSequences();
			}
			if (global::UnityEngine.GUI.Button(new global::UnityEngine.Rect(0f, (float)(global::UnityEngine.Screen.height - 90), 250f, 30f), global::AnimStyleId.DUAL_WIELD.ToString()))
			{
				this.weaponStyle = global::AnimStyleId.DUAL_WIELD;
				this.EquipWeapon();
				this.InitSequences();
			}
			if (global::UnityEngine.GUI.Button(new global::UnityEngine.Rect(0f, (float)(global::UnityEngine.Screen.height - 120), 250f, 30f), global::AnimStyleId.WARHAMMER.ToString()))
			{
				this.weaponStyle = global::AnimStyleId.WARHAMMER;
				this.EquipWeapon();
				this.InitSequences();
			}
			break;
		case "sis_ogre_base":
			if (global::UnityEngine.GUI.Button(new global::UnityEngine.Rect(0f, (float)(global::UnityEngine.Screen.height - 30), 250f, 30f), global::AnimStyleId.ONE_HAND_NO_SHIELD.ToString()))
			{
				this.weaponStyle = global::AnimStyleId.ONE_HAND_NO_SHIELD;
				this.EquipWeapon();
				this.InitSequences();
			}
			break;
		case "mon_plaguebearer_base":
		case "mon_bloodletter_base":
			if (global::UnityEngine.GUI.Button(new global::UnityEngine.Rect(0f, (float)(global::UnityEngine.Screen.height - 30), 250f, 30f), global::AnimStyleId.ONE_HAND_NO_SHIELD.ToString()))
			{
				this.weaponStyle = global::AnimStyleId.ONE_HAND_NO_SHIELD;
				this.EquipWeapon();
				this.InitSequences();
			}
			break;
		}
	}

	private void InitSequences()
	{
		this.sequences.Clear();
		if (!this.knocked)
		{
			switch (this.weaponStyle)
			{
			case global::AnimStyleId.NONE:
			case global::AnimStyleId.ONE_HAND_NO_SHIELD:
			case global::AnimStyleId.ONE_HAND_SHIELD:
			case global::AnimStyleId.SPEAR_NO_SHIELD:
			case global::AnimStyleId.SPEAR_SHIELD:
			case global::AnimStyleId.CLAW:
			case global::AnimStyleId.DUAL_WIELD:
			case global::AnimStyleId.TWO_HANDED:
			case global::AnimStyleId.HALBERD:
			case global::AnimStyleId.WARHAMMER:
				if (this.parry)
				{
					this.sequences.Add("parry");
				}
				this.sequences.Add("attack");
				this.sequences.Add("attack_fail");
				break;
			case global::AnimStyleId.DUAL_PISTOL:
			case global::AnimStyleId.BOW:
			case global::AnimStyleId.CROSSBOW:
			case global::AnimStyleId.RIFLE:
				this.sequences.Add("shoot");
				this.sequences.Add("reload");
				this.sequences.Add("aim");
				break;
			}
			if (this.HasSpecialAnims())
			{
				this.sequences.Add("disengage");
			}
			this.sequences.Add("hurt_back");
			this.sequences.Add("hurt_left");
			this.sequences.Add("hurt_right");
			this.sequences.Add("avoid_right");
			this.sequences.Add("avoid_high");
			this.sequences.Add("ooa_back");
			this.sequences.Add("ooa_front");
			this.sequences.Add("skill_01");
			if (this.currentUnit == "mon_horror_base")
			{
				this.sequences.Add("skill_02");
			}
			if (this.HasSpecialAnims())
			{
				this.sequences.Add("spell_area");
				this.sequences.Add("spell_point");
				this.sequences.Add("search");
				this.sequences.Add("interact");
			}
			if (this.currentUnit == "mer_ogre_base")
			{
				this.sequences.Add("cqc_special");
			}
			if (this.currentUnit == "mer_base")
			{
				this.sequences.Add("spell_special");
			}
			if (!this.ctrlr.unit.IsMonster)
			{
				this.sequences.Add("defeat");
				this.sequences.Add("perception");
				this.sequences.Add("stupidity");
				this.sequences.Add("cheer");
			}
			this.sequences.Add("climb3");
			if (this.HasSpecialAnims())
			{
				this.sequences.Add("climb6");
				this.sequences.Add("climb9");
				this.sequences.Add("climb3_fail");
				this.sequences.Add("climb6_fail");
				this.sequences.Add("climb9_fail");
			}
			this.sequences.Add("jump3");
			if (this.HasSpecialAnims())
			{
				this.sequences.Add("jump6");
				this.sequences.Add("jump9");
				this.sequences.Add("jump3_fail");
				this.sequences.Add("jump6_fail");
				this.sequences.Add("jump9_fail");
				this.sequences.Add("leap");
				this.sequences.Add("leap3_fail");
				this.sequences.Add("leap6_fail");
				this.sequences.Add("leap9_fail");
			}
		}
		else
		{
			this.sequences.Add("stunned");
			this.sequences.Add("stunned_hurt");
			this.sequences.Add("get_up");
			this.sequences.Add("ooa");
		}
	}

	private void PlaySequence(string seq)
	{
		this.ResetAll();
		float num = 0.1f;
		switch (seq)
		{
		case "climb3":
			this.animator.SetInteger(global::AnimatorIds.action, 2);
			this.animator.SetBool(global::AnimatorIds.actionSuccess, true);
			this.animator.SetInteger(global::AnimatorIds.unit_state, 0);
			this.animator.Play(this.idleHash);
			if (this.currentUnit != "mon_bloodletter_base" && this.currentUnit != "mon_plaguebearer_base" && this.currentUnit != "ska_ogre_base" && this.currentUnit != "pos_ogre_base" && this.currentUnit != "mon_horror_base")
			{
				num = 2.5f;
			}
			break;
		case "climb6":
			this.animator.SetInteger(global::AnimatorIds.action, 3);
			this.animator.SetBool(global::AnimatorIds.actionSuccess, true);
			this.animator.SetInteger(global::AnimatorIds.unit_state, 0);
			this.animator.Play(this.idleHash);
			num = 2.5f;
			break;
		case "climb9":
			this.animator.SetInteger(global::AnimatorIds.action, 4);
			this.animator.SetBool(global::AnimatorIds.actionSuccess, true);
			this.animator.SetInteger(global::AnimatorIds.unit_state, 0);
			this.animator.Play(this.idleHash);
			num = 2.5f;
			break;
		case "climb3_fail":
			this.animator.SetInteger(global::AnimatorIds.action, 2);
			this.animator.SetBool(global::AnimatorIds.actionSuccess, false);
			this.animator.SetInteger(global::AnimatorIds.unit_state, 0);
			this.animator.Play(this.idleHash);
			break;
		case "climb6_fail":
			this.animator.SetInteger(global::AnimatorIds.action, 3);
			this.animator.SetBool(global::AnimatorIds.actionSuccess, false);
			this.animator.SetInteger(global::AnimatorIds.unit_state, 0);
			this.animator.Play(this.idleHash);
			break;
		case "climb9_fail":
			this.animator.SetInteger(global::AnimatorIds.action, 4);
			this.animator.SetBool(global::AnimatorIds.actionSuccess, false);
			this.animator.SetInteger(global::AnimatorIds.unit_state, 0);
			this.animator.Play(this.idleHash);
			break;
		case "jump3":
			this.animator.SetInteger(global::AnimatorIds.action, 6);
			this.animator.SetBool(global::AnimatorIds.actionSuccess, true);
			this.animator.SetInteger(global::AnimatorIds.unit_state, 0);
			this.animator.Play(this.idleHash);
			if (this.currentUnit != "mon_bloodletter_base" && this.currentUnit != "mon_plaguebearer_base" && this.currentUnit != "ska_ogre_base" && this.currentUnit != "pos_ogre_base" && this.currentUnit != "mon_horror_base")
			{
				num = 2.5f;
			}
			break;
		case "jump6":
			this.animator.SetInteger(global::AnimatorIds.action, 7);
			this.animator.SetBool(global::AnimatorIds.actionSuccess, true);
			this.animator.SetInteger(global::AnimatorIds.unit_state, 0);
			this.animator.Play(this.idleHash);
			num = 2.5f;
			break;
		case "jump9":
			this.animator.SetInteger(global::AnimatorIds.action, 8);
			this.animator.SetBool(global::AnimatorIds.actionSuccess, true);
			this.animator.SetInteger(global::AnimatorIds.unit_state, 0);
			this.animator.Play(this.idleHash);
			num = 2.5f;
			break;
		case "jump3_fail":
			this.animator.SetInteger(global::AnimatorIds.action, 6);
			this.animator.SetBool(global::AnimatorIds.actionSuccess, false);
			this.animator.SetInteger(global::AnimatorIds.unit_state, 0);
			this.animator.Play(this.idleHash);
			break;
		case "jump6_fail":
			this.animator.SetInteger(global::AnimatorIds.action, 7);
			this.animator.SetBool(global::AnimatorIds.actionSuccess, false);
			this.animator.SetInteger(global::AnimatorIds.unit_state, 0);
			this.animator.Play(this.idleHash);
			break;
		case "jump9_fail":
			this.animator.SetInteger(global::AnimatorIds.action, 8);
			this.animator.SetBool(global::AnimatorIds.actionSuccess, false);
			this.animator.SetInteger(global::AnimatorIds.unit_state, 0);
			this.animator.Play(this.idleHash);
			break;
		case "leap":
			this.animator.SetInteger(global::AnimatorIds.action, 5);
			this.animator.SetBool(global::AnimatorIds.actionSuccess, true);
			this.animator.SetInteger(global::AnimatorIds.unit_state, 0);
			this.animator.Play(this.idleHash);
			num = 2.5f;
			break;
		case "leap3_fail":
			this.animator.SetInteger(global::AnimatorIds.action, 5);
			this.animator.SetBool(global::AnimatorIds.actionSuccess, false);
			this.animator.SetInteger(global::AnimatorIds.variation, 0);
			this.animator.SetInteger(global::AnimatorIds.unit_state, 0);
			this.animator.Play(this.idleHash);
			break;
		case "leap6_fail":
			this.animator.SetInteger(global::AnimatorIds.action, 5);
			this.animator.SetBool(global::AnimatorIds.actionSuccess, false);
			this.animator.SetInteger(global::AnimatorIds.variation, 1);
			this.animator.SetInteger(global::AnimatorIds.unit_state, 0);
			this.animator.Play(this.idleHash);
			break;
		case "leap9_fail":
			this.animator.SetInteger(global::AnimatorIds.action, 5);
			this.animator.SetBool(global::AnimatorIds.actionSuccess, false);
			this.animator.SetInteger(global::AnimatorIds.variation, 2);
			this.animator.SetInteger(global::AnimatorIds.unit_state, 0);
			this.animator.Play(this.idleHash);
			break;
		case "parry":
			this.animator.SetInteger(global::AnimatorIds.atkResult, 3);
			this.animator.SetInteger(global::AnimatorIds.unit_state, 0);
			this.animator.Play(this.idleHash);
			break;
		case "attack":
			this.animator.SetInteger(global::AnimatorIds.action, 19);
			this.animator.SetBool(global::AnimatorIds.actionSuccess, true);
			this.animator.SetInteger(global::AnimatorIds.unit_state, 0);
			this.animator.Play(this.idleHash);
			break;
		case "attack_fail":
			this.animator.SetInteger(global::AnimatorIds.action, 19);
			this.animator.SetBool(global::AnimatorIds.actionSuccess, false);
			this.animator.SetInteger(global::AnimatorIds.unit_state, 0);
			this.animator.Play(this.idleHash);
			break;
		case "shoot":
			this.animator.SetInteger(global::AnimatorIds.action, 16);
			this.animator.SetInteger(global::AnimatorIds.unit_state, 0);
			this.animator.Play(this.idleHash);
			break;
		case "aim":
			this.animator.SetInteger(global::AnimatorIds.action, 17);
			this.animator.SetInteger(global::AnimatorIds.unit_state, 0);
			this.animator.Play(this.idleHash);
			break;
		case "reload":
			this.animator.SetInteger(global::AnimatorIds.action, 18);
			this.animator.SetInteger(global::AnimatorIds.unit_state, 0);
			this.animator.Play(this.idleHash);
			break;
		case "disengage":
			this.animator.SetInteger(global::AnimatorIds.action, 15);
			this.animator.SetInteger(global::AnimatorIds.unit_state, 0);
			this.animator.Play(this.idleHash);
			break;
		case "spell_point":
			this.animator.SetInteger(global::AnimatorIds.action, 45);
			this.animator.SetInteger(global::AnimatorIds.variation, 1);
			this.animator.SetInteger(global::AnimatorIds.unit_state, 0);
			this.animator.Play(this.idleHash);
			num = 2.5f;
			break;
		case "spell_area":
			this.animator.SetInteger(global::AnimatorIds.action, 45);
			this.animator.SetInteger(global::AnimatorIds.variation, 0);
			this.animator.SetInteger(global::AnimatorIds.unit_state, 0);
			this.animator.Play(this.idleHash);
			break;
		case "defeat":
			this.animator.SetInteger(global::AnimatorIds.action, 50);
			this.animator.SetInteger(global::AnimatorIds.variation, 4);
			this.animator.SetInteger(global::AnimatorIds.unit_state, 0);
			this.animator.Play(this.idleHash);
			break;
		case "perception":
			this.animator.SetInteger(global::AnimatorIds.action, 12);
			this.animator.SetInteger(global::AnimatorIds.unit_state, 0);
			this.animator.Play(this.idleHash);
			break;
		case "stupidity":
			this.animator.SetInteger(global::AnimatorIds.action, 50);
			this.animator.SetInteger(global::AnimatorIds.variation, 0);
			this.animator.SetInteger(global::AnimatorIds.unit_state, 0);
			this.animator.Play(this.idleHash);
			break;
		case "cheer":
			this.animator.SetInteger(global::AnimatorIds.action, 50);
			this.animator.SetInteger(global::AnimatorIds.variation, 1);
			this.animator.SetInteger(global::AnimatorIds.unit_state, 0);
			this.animator.Play(this.idleHash);
			break;
		case "interact":
			this.animator.SetInteger(global::AnimatorIds.action, 13);
			this.animator.SetInteger(global::AnimatorIds.variation, 4);
			this.animator.SetInteger(global::AnimatorIds.unit_state, 0);
			this.animator.Play(this.idleHash);
			num = 3f;
			break;
		case "search":
			this.animator.SetInteger(global::AnimatorIds.action, 13);
			this.animator.SetInteger(global::AnimatorIds.variation, 3);
			this.animator.SetInteger(global::AnimatorIds.unit_state, 0);
			this.animator.Play(this.idleHash);
			num = 3f;
			break;
		case "hurt_back":
			this.animator.SetInteger(global::AnimatorIds.atkResult, 4);
			this.animator.SetInteger(global::AnimatorIds.variation, 2);
			this.animator.SetInteger(global::AnimatorIds.unit_state, 0);
			this.animator.Play(this.idleHash);
			break;
		case "hurt_left":
			this.animator.SetInteger(global::AnimatorIds.atkResult, 4);
			this.animator.SetInteger(global::AnimatorIds.variation, 1);
			this.animator.SetInteger(global::AnimatorIds.unit_state, 0);
			this.animator.Play(this.idleHash);
			break;
		case "hurt_right":
			this.animator.SetInteger(global::AnimatorIds.atkResult, 4);
			this.animator.SetInteger(global::AnimatorIds.variation, 0);
			this.animator.SetInteger(global::AnimatorIds.unit_state, 0);
			this.animator.Play(this.idleHash);
			break;
		case "avoid_right":
			this.animator.SetInteger(global::AnimatorIds.atkResult, 2);
			this.animator.SetInteger(global::AnimatorIds.variation, 0);
			this.animator.SetInteger(global::AnimatorIds.unit_state, 0);
			this.animator.Play(this.idleHash);
			break;
		case "avoid_high":
			this.animator.SetInteger(global::AnimatorIds.atkResult, 2);
			this.animator.SetInteger(global::AnimatorIds.variation, 1);
			this.animator.SetInteger(global::AnimatorIds.unit_state, 0);
			this.animator.Play(this.idleHash);
			break;
		case "ooa_back":
			this.animator.SetInteger(global::AnimatorIds.atkResult, 5);
			this.animator.SetInteger(global::AnimatorIds.variation, 0);
			this.animator.SetInteger(global::AnimatorIds.unit_state, 3);
			this.animator.Play(this.idleHash);
			break;
		case "ooa_front":
			this.animator.SetInteger(global::AnimatorIds.atkResult, 5);
			this.animator.SetInteger(global::AnimatorIds.variation, 1);
			this.animator.SetInteger(global::AnimatorIds.unit_state, 3);
			this.animator.Play(this.idleHash);
			break;
		case "skill_01":
			this.animator.SetInteger(global::AnimatorIds.action, 40);
			this.animator.SetInteger(global::AnimatorIds.variation, 0);
			this.animator.Play(this.idleHash);
			break;
		case "skill_02":
			this.animator.SetInteger(global::AnimatorIds.action, 40);
			this.animator.SetInteger(global::AnimatorIds.variation, 1);
			this.animator.Play(this.idleHash);
			break;
		case "stunned":
			this.animator.SetInteger(global::AnimatorIds.atkResult, 5);
			this.animator.SetInteger(global::AnimatorIds.unit_state, 2);
			this.animator.Play(this.idleHash);
			break;
		case "stunned_hurt":
			this.animator.SetInteger(global::AnimatorIds.atkResult, 5);
			this.animator.SetInteger(global::AnimatorIds.unit_state, 2);
			break;
		case "get_up":
			this.animator.SetInteger(global::AnimatorIds.unit_state, 0);
			break;
		case "ooa":
			this.animator.SetInteger(global::AnimatorIds.unit_state, 3);
			break;
		}
		base.StartCoroutine("ResetAction", num);
	}

	private global::System.Collections.IEnumerator ResetAction(float time)
	{
		yield return new global::UnityEngine.WaitForSeconds(time);
		this.animator.SetInteger(global::AnimatorIds.action, 0);
		this.animator.SetInteger(global::AnimatorIds.atkResult, 0);
		this.animator.SetBool(global::AnimatorIds.actionSuccess, false);
		this.animator.SetInteger(global::AnimatorIds.variation, 0);
		yield break;
	}

	private void ResetAll()
	{
		this.animator.SetInteger(global::AnimatorIds.action, 0);
		this.animator.SetInteger(global::AnimatorIds.atkResult, 0);
		this.animator.SetBool(global::AnimatorIds.actionSuccess, false);
		this.animator.SetInteger(global::AnimatorIds.variation, 0);
	}

	public void EquipWeapon()
	{
		this.parry = false;
		this.sheated = false;
		switch (this.weaponStyle)
		{
		case global::AnimStyleId.NONE:
			this.ctrlr.EquipItem(global::UnitSlotId.SET1_MAINHAND, global::ItemId.NONE);
			this.ctrlr.EquipItem(global::UnitSlotId.SET1_OFFHAND, global::ItemId.NONE);
			break;
		case global::AnimStyleId.ONE_HAND_NO_SHIELD:
			if (this.currentUnit == "ska_base" || this.currentUnit == "mer_base" || this.currentUnit == "mon_bloodletter_base" || this.currentUnit == "mon_plaguebearer_base" || this.currentUnit == "mer_ogre_base")
			{
				this.ctrlr.EquipItem(global::UnitSlotId.SET1_MAINHAND, global::ItemId.SWORD);
				this.parry = true;
			}
			if (this.currentUnit == "sis_base")
			{
				this.ctrlr.EquipItem(global::UnitSlotId.SET1_MAINHAND, global::ItemId.HAMMER);
			}
			if (this.currentUnit == "sis_ogre_base")
			{
				this.ctrlr.EquipItem(global::UnitSlotId.SET1_MAINHAND, global::ItemId.SIGMARITE_WARHAMMER);
			}
			this.ctrlr.EquipItem(global::UnitSlotId.SET1_OFFHAND, global::ItemId.NONE);
			break;
		case global::AnimStyleId.ONE_HAND_SHIELD:
			this.parry = true;
			this.ctrlr.EquipItem(global::UnitSlotId.SET1_MAINHAND, global::ItemId.DAGGER);
			this.ctrlr.EquipItem(global::UnitSlotId.SET1_OFFHAND, global::ItemId.SHIELD);
			break;
		case global::AnimStyleId.SPEAR_NO_SHIELD:
			this.ctrlr.EquipItem(global::UnitSlotId.SET1_MAINHAND, global::ItemId.SPEAR);
			this.ctrlr.EquipItem(global::UnitSlotId.SET1_OFFHAND, global::ItemId.NONE);
			break;
		case global::AnimStyleId.SPEAR_SHIELD:
			this.ctrlr.EquipItem(global::UnitSlotId.SET1_MAINHAND, global::ItemId.SPEAR);
			this.ctrlr.EquipItem(global::UnitSlotId.SET1_OFFHAND, global::ItemId.SHIELD);
			this.parry = true;
			break;
		case global::AnimStyleId.CLAW:
			this.ctrlr.EquipItem(global::UnitSlotId.SET1_MAINHAND, global::ItemId.FIGHTING_CLAWS);
			break;
		case global::AnimStyleId.DUAL_WIELD:
			if (this.currentUnit == "sis_base")
			{
				this.ctrlr.EquipItem(global::UnitSlotId.SET1_MAINHAND, global::ItemId.HAMMER);
				this.ctrlr.EquipItem(global::UnitSlotId.SET1_OFFHAND, global::ItemId.HAMMER);
			}
			else
			{
				this.ctrlr.EquipItem(global::UnitSlotId.SET1_MAINHAND, global::ItemId.SWORD);
				this.ctrlr.EquipItem(global::UnitSlotId.SET1_OFFHAND, global::ItemId.SWORD);
				this.parry = true;
			}
			break;
		case global::AnimStyleId.TWO_HANDED:
			this.ctrlr.EquipItem(global::UnitSlotId.SET1_MAINHAND, global::ItemId.TWO_HANDED_SWORD);
			this.ctrlr.EquipItem(global::UnitSlotId.SET1_OFFHAND, global::ItemId.NONE);
			break;
		case global::AnimStyleId.HALBERD:
			this.ctrlr.EquipItem(global::UnitSlotId.SET1_MAINHAND, global::ItemId.HALBERD);
			this.ctrlr.EquipItem(global::UnitSlotId.SET1_OFFHAND, global::ItemId.NONE);
			this.parry = true;
			break;
		case global::AnimStyleId.WARHAMMER:
			if (this.currentUnit == "sis_base")
			{
				this.ctrlr.EquipItem(global::UnitSlotId.SET1_MAINHAND, global::ItemId.TWO_HANDED_HAMMER);
				this.ctrlr.EquipItem(global::UnitSlotId.SET1_OFFHAND, global::ItemId.NONE);
			}
			else
			{
				this.ctrlr.EquipItem(global::UnitSlotId.SET1_MAINHAND, global::ItemId.TWO_HANDED_AXE);
				this.ctrlr.EquipItem(global::UnitSlotId.SET1_OFFHAND, global::ItemId.NONE);
			}
			break;
		case global::AnimStyleId.DUAL_PISTOL:
			if (this.currentUnit == "mer_base")
			{
				this.ctrlr.EquipItem(global::UnitSlotId.SET1_MAINHAND, global::ItemId.PISTOL);
			}
			if (this.currentUnit == "ska_base")
			{
				this.ctrlr.EquipItem(global::UnitSlotId.SET1_MAINHAND, global::ItemId.WARPLOCK_PISTOL);
			}
			break;
		case global::AnimStyleId.BOW:
			if (this.currentUnit == "mer_base")
			{
				this.ctrlr.EquipItem(global::UnitSlotId.SET1_MAINHAND, global::ItemId.BOW);
				this.ctrlr.EquipItem(global::UnitSlotId.SET1_OFFHAND, global::ItemId.NONE);
			}
			break;
		case global::AnimStyleId.CROSSBOW:
			if (this.currentUnit == "mer_base")
			{
				this.ctrlr.EquipItem(global::UnitSlotId.SET1_MAINHAND, global::ItemId.CROSSBOW);
				this.ctrlr.EquipItem(global::UnitSlotId.SET1_OFFHAND, global::ItemId.NONE);
			}
			break;
		case global::AnimStyleId.RIFLE:
			if (this.currentUnit == "mer_base")
			{
				this.ctrlr.EquipItem(global::UnitSlotId.SET1_MAINHAND, global::ItemId.HUNTING_RIFLE);
				this.ctrlr.EquipItem(global::UnitSlotId.SET1_OFFHAND, global::ItemId.NONE);
			}
			break;
		}
		this.ctrlr.unit.currentAnimStyleId = this.weaponStyle;
		this.ctrlr.SetAnimStyle();
	}

	public global::UnityEngine.TextAsset textAsset;

	private global::System.Collections.Generic.Dictionary<string, global::System.Collections.Generic.Dictionary<string, string[]>> animatorsData;

	private global::System.Collections.Generic.Dictionary<string, global::UnityEngine.GameObject> units;

	private bool init;

	private string currentUnit;

	public global::AnimStyleId weaponStyle;

	private int currentState;

	private global::UnityEngine.GameObject unitObject;

	private global::UnityEngine.Animator animator;

	private float runningValue;

	private float timeScale = 1f;

	private int idleHash;

	private global::System.Collections.Generic.List<string> sequences;

	private bool needsRestart;

	private global::UnitMenuController ctrlr;

	public bool sheated;

	private bool parry;

	private bool knocked;
}
