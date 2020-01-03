using System;
using UnityEngine.Events;
using UnityEngine.UI;

public class HideoutSkills : global::BaseHideoutUnitState
{
	public HideoutSkills(global::HideoutManager mng, global::HideoutCamAnchor anchor, bool showSpell) : base(anchor, (!showSpell) ? global::HideoutManager.State.SKILLS : global::HideoutManager.State.SPELLS)
	{
		this.showSpell = showSpell;
	}

	public override void Enter(int iFrom)
	{
		this.currentSkillIndex = -1;
		this.warband = global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband;
		global::PandoraSingleton<global::HideoutTabManager>.Instance.ActivateRightTabModules(true, new global::ModuleId[]
		{
			global::ModuleId.TREASURY,
			global::ModuleId.SKILLS
		});
		if (global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.unitCtrlrs.Count > 1)
		{
			global::PandoraSingleton<global::HideoutTabManager>.Instance.ActivateCenterTabModules(new global::ModuleId[]
			{
				global::ModuleId.WHEEL_SKILLS,
				global::ModuleId.UNIT_TABS,
				global::ModuleId.TITLE,
				(!this.showSpell) ? global::ModuleId.SKILL_DESC : global::ModuleId.SPELL_DESC,
				global::ModuleId.DESC,
				global::ModuleId.NEXT_UNIT,
				global::ModuleId.CHARACTER_AREA,
				global::ModuleId.NOTIFICATION
			});
			global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleCenter<global::NextUnitModule>(global::ModuleId.NEXT_UNIT).Setup();
		}
		else
		{
			global::PandoraSingleton<global::HideoutTabManager>.Instance.ActivateCenterTabModules(new global::ModuleId[]
			{
				global::ModuleId.WHEEL_SKILLS,
				global::ModuleId.UNIT_TABS,
				global::ModuleId.TITLE,
				(!this.showSpell) ? global::ModuleId.SKILL_DESC : global::ModuleId.SPELL_DESC,
				global::ModuleId.DESC,
				global::ModuleId.CHARACTER_AREA,
				global::ModuleId.NOTIFICATION
			});
		}
		base.Enter(iFrom);
		this.descModule = global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleCenter<global::DescriptionModule>(global::ModuleId.DESC);
		this.descModule.gameObject.SetActive(false);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleRight<global::TreasuryModule>(global::ModuleId.TREASURY).Refresh(global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.GetWarbandSave());
		this.skillsModule = global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleRight<global::SkillsModule>(global::ModuleId.SKILLS);
		this.skillsModule.Refresh(this.showSpell);
		this.wheelModule = global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleCenter<global::SkillsWheelModule>(global::ModuleId.WHEEL_SKILLS);
		this.skillDescModule = global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleCenter<global::SkillDescModule>((!this.showSpell) ? global::ModuleId.SKILL_DESC : global::ModuleId.SPELL_DESC);
		this.characterCamModule = global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleCenter<global::CharacterCameraAreaModule>(global::ModuleId.CHARACTER_AREA);
		this.characterCamModule.Init(this.camAnchor.transform.position);
		this.SelectUnit(global::PandoraSingleton<global::HideoutManager>.Instance.currentUnit);
	}

	public override void Exit(int iTo)
	{
		base.Exit(iTo);
		this.wheelModule.Deactivate();
		this.skillsModule.ClearList();
		global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleCenter<global::WheelModule>(global::ModuleId.WHEEL).Deactivate();
	}

	public override void SelectUnit(global::UnitMenuController ctrlr)
	{
		base.SelectUnit(ctrlr);
		this.UpdateWheel();
		this.skillDescModule.gameObject.SetActive(false);
		this.skillsModule.Refresh(this.showSpell);
		this.skillsModule.ClearList();
		this.currentSkillIndex = -1;
		this.wheelModule.SelectSlot(0, true);
		this.SetButtonsWithoutLearnSkill(true);
	}

	public override global::UnityEngine.UI.Selectable ModuleLeftOnRight()
	{
		return this.wheelModule.activeSkills[0].toggle.toggle;
	}

	private void UpdateWheel()
	{
		if (this.showSpell)
		{
			this.wheelModule.ShowSpells(base.ModuleCentertOnLeft(), new global::System.Action<int, global::SkillData>(this.OnWheelSkillSelected), new global::System.Action<int, global::SkillData>(this.OnActiveSkill));
		}
		else
		{
			this.wheelModule.ShowSkills(base.ModuleCentertOnLeft(), new global::System.Action<int, global::SkillData>(this.OnWheelSkillSelected), new global::System.Action<int, global::SkillData>(this.OnPassiveSkill), new global::System.Action<int, global::SkillData>(this.OnActiveSkill));
		}
	}

	private void SetButtonsWithLearnSkill()
	{
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button1.SetAction("cancel", "menu_return_select_slot", 0, false, global::PandoraSingleton<global::HideoutTabManager>.Instance.icnBack, null);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button1.OnAction(new global::UnityEngine.Events.UnityAction(this.ReturnSelectSlot), false, true);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button2.SetAction("action", "menu_learn_skill", 0, false, null, null);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button2.OnAction(new global::UnityEngine.Events.UnityAction(this.SelectCurrentSkill), false, true);
		base.SetupApplyButton(global::PandoraSingleton<global::HideoutTabManager>.Instance.button3);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button4.gameObject.SetActive(false);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button5.gameObject.SetActive(false);
	}

	private void SetButtonsWithoutLearnSkill(bool inWheel)
	{
		if (inWheel)
		{
			global::PandoraSingleton<global::HideoutTabManager>.Instance.button1.SetAction("cancel", "go_to_warband", 0, false, global::PandoraSingleton<global::HideoutTabManager>.Instance.icnBack, null);
			global::PandoraSingleton<global::HideoutTabManager>.Instance.button1.OnAction(new global::UnityEngine.Events.UnityAction(base.ReturnToWarband), false, true);
			global::PandoraSingleton<global::HideoutTabManager>.Instance.button2.SetAction("action", "menu_select_slot", 0, false, null, null);
			global::PandoraSingleton<global::HideoutTabManager>.Instance.button2.OnAction(null, false, true);
			base.SetupApplyButton(global::PandoraSingleton<global::HideoutTabManager>.Instance.button3);
			global::PandoraSingleton<global::HideoutTabManager>.Instance.button4.SetAction("action", "menu_select_slot", 0, false, null, null);
			global::PandoraSingleton<global::HideoutTabManager>.Instance.button4.OnAction(new global::UnityEngine.Events.UnityAction(this.OnRespecButton), false, true);
			global::PandoraSingleton<global::HideoutTabManager>.Instance.button5.gameObject.SetActive(false);
			return;
		}
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button1.SetAction("cancel", "menu_return_select_slot", 0, false, global::PandoraSingleton<global::HideoutTabManager>.Instance.icnBack, null);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button1.OnAction(new global::UnityEngine.Events.UnityAction(this.ReturnSelectSlot), false, true);
		base.SetupApplyButton(global::PandoraSingleton<global::HideoutTabManager>.Instance.button2);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button3.gameObject.SetActive(false);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button4.gameObject.SetActive(false);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button5.gameObject.SetActive(false);
	}

	private void SetButtonsAttributeSelection()
	{
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button1.SetAction("cancel", "go_to_warband", 0, false, global::PandoraSingleton<global::HideoutTabManager>.Instance.icnBack, null);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button1.OnAction(new global::UnityEngine.Events.UnityAction(base.ReturnToWarband), false, true);
		base.SetupAttributeButtons(global::PandoraSingleton<global::HideoutTabManager>.Instance.button2, global::PandoraSingleton<global::HideoutTabManager>.Instance.button3, global::PandoraSingleton<global::HideoutTabManager>.Instance.button4);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button5.gameObject.SetActive(false);
	}

	private void OnSkillConfirmed(int index, bool active, global::SkillData skillData)
	{
		this.currentLearnSkill = skillData;
		if (this.skillsShop.CanLearnSkill(skillData))
		{
			global::PandoraSingleton<global::HideoutManager>.Instance.messagePopup.ShowLocalized(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("popup_learn_skill_title"), global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("popup_learn_skill_desc", new string[]
			{
				global::SkillHelper.GetLocalizedName(skillData)
			}), new global::System.Action<bool>(this.OnLearnSkillPopup), false, false);
		}
	}

	private void OnLearnSkillPopup(bool confirm)
	{
		if (confirm)
		{
			global::PandoraSingleton<global::HideoutManager>.Instance.WarbandChest.RemoveGold(this.warband.GetSkillLearnPrice(this.currentLearnSkill, global::PandoraSingleton<global::HideoutManager>.Instance.currentUnit.unit));
			global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleRight<global::TreasuryModule>(global::ModuleId.TREASURY).Refresh(global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.GetWarbandSave());
			this.currentUnit.unit.StartLearningSkill(this.currentLearnSkill, global::PandoraSingleton<global::HideoutManager>.Instance.Date.CurrentDate, !global::PandoraSingleton<global::HideoutManager>.Instance.IsTrainingOutsider);
			if (global::PandoraSingleton<global::HideoutManager>.Instance.IsTrainingOutsider)
			{
				this.currentUnit.unit.EndLearnSkill(true);
			}
			this.UpdateWheel();
			global::PandoraSingleton<global::HideoutManager>.Instance.SaveChanges();
			base.RefreshUnitAttributes();
			this.skillsModule.Refresh(this.showSpell);
			this.currentLearnSkill = null;
			this.ReturnSelectSlot();
		}
	}

	private void OnPassiveSkill(int index, global::SkillData skillData)
	{
		if (this.currentSkillIndex != index || this.currentSkillActive)
		{
			this.currentSkillIndex = index;
			this.currentSkillActive = false;
			if (skillData == null || global::SkillHelper.HasMastery(skillData))
			{
				base.CheckForChanges(delegate
				{
					this.OnApplyChanges();
					this.skillsModule.ShowSkills(new global::System.Action<global::SkillData>(this.OnSkillSelected), new global::System.Action<int, bool, global::SkillData>(this.OnSkillConfirmed), skillData, false);
				}, false);
			}
			else
			{
				this.skillsModule.ClearList();
				this.skillsModule.SetInteractable(false);
			}
		}
	}

	private void OnActiveSkill(int index, global::SkillData skillData)
	{
		if (this.currentSkillIndex != index || !this.currentSkillActive)
		{
			this.currentSkillIndex = index;
			this.currentSkillActive = true;
			if (skillData == null || global::SkillHelper.HasMastery(skillData))
			{
				base.CheckForChanges(delegate
				{
					this.OnApplyChanges();
					this.skillsModule.ShowSkills(new global::System.Action<global::SkillData>(this.OnSkillSelected), new global::System.Action<int, bool, global::SkillData>(this.OnSkillConfirmed), skillData, true);
				}, false);
			}
			else
			{
				this.skillsModule.ClearList();
				this.skillsModule.SetInteractable(false);
			}
		}
	}

	private void OnWheelSkillSelected(int idx, global::SkillData skillData)
	{
		if (skillData != null)
		{
			this.skillDescModule.gameObject.SetActive(true);
			this.descModule.gameObject.SetActive(false);
			this.skillDescModule.Set(skillData, null);
		}
		else
		{
			this.skillDescModule.gameObject.SetActive(false);
			this.descModule.gameObject.SetActive(false);
		}
		this.SetButtonsWithoutLearnSkill(true);
	}

	private void OnSkillSelected(global::SkillData skillData)
	{
		if (skillData != null)
		{
			this.selectedSkill = skillData;
			this.skillDescModule.gameObject.SetActive(true);
			this.descModule.gameObject.SetActive(false);
			string reason;
			if (this.skillsShop.CanLearnSkill(skillData, out reason))
			{
				this.SetButtonsWithLearnSkill();
			}
			else
			{
				this.SetButtonsWithoutLearnSkill(false);
			}
			this.skillDescModule.Set(skillData, reason);
		}
		else
		{
			this.skillDescModule.gameObject.SetActive(false);
			this.descModule.gameObject.SetActive(false);
			this.SetButtonsWithoutLearnSkill(false);
		}
	}

	protected override void ShowDescription(string title, string desc)
	{
		base.ShowDescription(title, desc);
		this.skillDescModule.gameObject.SetActive(false);
		this.SetButtonsAttributeSelection();
	}

	protected void ReturnSelectSlot()
	{
		int num = this.currentSkillIndex;
		this.currentSkillIndex = -1;
		this.wheelModule.SelectSlot(num, this.currentSkillActive);
		this.skillsModule.ClearList();
		this.skillsModule.RefreshUnspentPoints();
		this.SetButtonsWithoutLearnSkill(true);
	}

	protected override void OnAttributeChanged()
	{
		base.OnAttributeChanged();
		this.currentSkillIndex = -1;
		this.skillsModule.ClearList();
	}

	public override bool CanIncreaseAttributes()
	{
		return true;
	}

	private void SelectCurrentSkill()
	{
		if (this.selectedSkill != null)
		{
			this.OnSkillConfirmed(-1, false, this.selectedSkill);
		}
	}

	private void OnRespecButton()
	{
		global::PandoraSingleton<global::HideoutManager>.Instance.messagePopup.ShowLocalized(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("popup_learn_skill_title"), global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("popup_learn_skill_desc", new string[]
		{
			this.currentUnit.unit.Name
		}), new global::System.Action<bool>(this.OnRespecPopup), false, false);
	}

	private void OnRespecPopup(bool confirm)
	{
		if (confirm)
		{
			this.currentUnit.unit.ResetUnitSkills();
			this.UpdateWheel();
			global::PandoraSingleton<global::HideoutManager>.Instance.SaveChanges();
			base.RefreshUnitAttributes();
			this.skillsModule.Refresh(this.showSpell);
		}
	}

	private readonly global::SkillsShop skillsShop = new global::SkillsShop();

	private global::SkillsModule skillsModule;

	private global::SkillsWheelModule wheelModule;

	private global::SkillDescModule skillDescModule;

	private bool isActive;

	private global::SkillData currentLearnSkill;

	private bool currentSkillActive;

	private int currentSkillIndex;

	private global::SkillData selectedSkill;

	private bool showSpell;

	private global::Warband warband;
}
