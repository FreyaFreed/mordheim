using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SkillsModule : global::UIModule
{
	public override void Init()
	{
		base.Init();
		this.filterTitle.text = string.Empty;
		this.scrollGroup.Setup(this.skillPrefab, true);
		this.canLearnSkills = new global::System.Collections.Generic.List<global::SkillData>();
		this.cannotLearnSkills = new global::System.Collections.Generic.List<global::SkillData>();
		this.btnPreviousFilter.SetAction("subfilter", null, 0, false, null, null);
		this.btnPreviousFilter.OnAction(new global::UnityEngine.Events.UnityAction(this.Next), false, true);
	}

	public void Refresh(bool showOnlySpell)
	{
		this.showSpell = showOnlySpell;
		this.skillLinesGroup.gameObject.SetActive(!this.showSpell);
		this.title.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById((!this.showSpell) ? "hideout_menu_unit_skills" : "hideout_menu_unit_spells");
		this.skillLines = this.skillsShop.GetUnitSkillLines(global::PandoraSingleton<global::HideoutManager>.Instance.currentUnit.unit);
		this.RefreshUnspentPoints();
		this.warbandIcon.sprite = global::Warband.GetIcon(global::PandoraSingleton<global::HideoutManager>.Instance.currentUnit.unit.WarbandId);
		if (showOnlySpell)
		{
			this.emptyListMessage.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_select_spell_slot");
		}
		else
		{
			this.emptyListMessage.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_select_skill_slot");
		}
		this.emptyListMessage.gameObject.SetActive(true);
	}

	public void RefreshUnspentPoints()
	{
		int num = (!this.showSpell) ? global::PandoraSingleton<global::HideoutManager>.Instance.currentUnit.unit.UnspentSkill : global::PandoraSingleton<global::HideoutManager>.Instance.currentUnit.unit.UnspentSpell;
		if (num > 0)
		{
			this.unspentPoints.gameObject.SetActive(true);
			this.pointsText.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById((!this.showSpell) ? "hideout_unspent_skill_point" : "hideout_unspent_spell_point", new string[]
			{
				num.ToString()
			});
		}
		else
		{
			this.unspentPoints.gameObject.SetActive(false);
		}
	}

	public void ShowSkills(global::System.Action<global::SkillData> onSkillSelected, global::System.Action<int, bool, global::SkillData> onSkillConfirmed, global::SkillData currentSkill, bool active)
	{
		if (!this.showSpell)
		{
			this.title.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById((!active) ? "skill_category_title_passive_skill" : "skill_category_title_active_skill");
		}
		base.SetInteractable(true);
		this.isFocus = true;
		this.showActiveSkill = active;
		this.onSkillSelectedCallback = onSkillSelected;
		this.onSkillConfirmedCallback = onSkillConfirmed;
		this.scrollGroup.ClearList();
		if (currentSkill == null)
		{
			global::UnityEngine.CanvasGroup canvasGroup = this.skillLinesGroup;
			bool flag = true;
			this.skillLinesGroup.blocksRaycasts = flag;
			canvasGroup.interactable = flag;
			this.showMastery = false;
			this.currentSkillLineIndex = 0;
			for (int i = 0; i < this.skillLinesIcons.Count; i++)
			{
				this.skillLinesIcons[i].image.enabled = true;
				this.skillLinesIcons[i].available = this.skillLines.ContainsKey(this.skillLinesIcons[i].skillLine);
				this.skillLinesIcons[i].image.onAction.RemoveAllListeners();
				global::SkillLineId skillLine = this.skillLinesIcons[i].skillLine;
				this.skillLinesIcons[i].image.onAction.AddListener(delegate()
				{
					this.SelectSkillLine(skillLine);
				});
			}
			this.SelectSkillLine((!this.showSpell) ? this.skillLinesIcons[this.currentSkillLineIndex].skillLine : global::SkillLineId.SPELL);
		}
		else
		{
			global::UnityEngine.CanvasGroup canvasGroup2 = this.skillLinesGroup;
			bool flag = false;
			this.skillLinesGroup.blocksRaycasts = flag;
			canvasGroup2.interactable = flag;
			if (!global::SkillHelper.IsMastery(currentSkill))
			{
				global::SkillData skillMastery = global::SkillHelper.GetSkillMastery(currentSkill);
				if (skillMastery != null)
				{
					if (this.showSpell)
					{
						this.AddSkill(skillMastery, this.skillsShop.CanLearnSkill(skillMastery), true);
					}
					else
					{
						global::SkillLineId skillLineId = global::SkillHelper.GetSkillLineId(skillMastery.Id, global::PandoraSingleton<global::HideoutManager>.Instance.currentUnit.unit.Id);
						for (int j = 0; j < this.skillLinesIcons.Count; j++)
						{
							if (this.skillLines[this.skillLinesIcons[j].skillLine].Contains(skillLineId, global::SkillLineIdComparer.Instance))
							{
								this.showMastery = true;
								this.currentSkillLineIndex = j;
								this.skillLinesIcons[j].image.toggle.isOn = true;
								this.AddSkill(skillMastery, this.skillsShop.CanLearnSkill(skillMastery), true);
							}
						}
					}
				}
			}
		}
		this.emptyListMessage.gameObject.SetActive(false);
	}

	public void SelectSkillLine(global::SkillLineId skillLine)
	{
		if (!this.showMastery && this.isFocus)
		{
			this.filterTitle.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_skill_line_" + skillLine);
			for (int i = 0; i < this.skillLinesIcons.Count; i++)
			{
				if (this.skillLinesIcons[i].skillLine == skillLine)
				{
					this.currentSkillLineIndex = i;
					this.skillLinesIcons[i].image.toggle.isOn = true;
					break;
				}
			}
			this.scrollGroup.ClearList();
			this.skillsShop.GetSkills(global::PandoraSingleton<global::HideoutManager>.Instance.currentUnit.unit, this.skillLines[skillLine], this.showActiveSkill, ref this.canLearnSkills, ref this.cannotLearnSkills);
			for (int j = 0; j < this.canLearnSkills.Count; j++)
			{
				this.AddSkill(this.canLearnSkills[j], true, j == 0);
			}
			for (int k = 0; k < this.cannotLearnSkills.Count; k++)
			{
				this.AddSkill(this.cannotLearnSkills[k], false, this.canLearnSkills.Count == 0 && k == 0);
			}
		}
		else
		{
			this.filterTitle.text = string.Empty;
		}
	}

	private void AddSkill(global::SkillData skillData, bool canLearn, bool select)
	{
		global::UnityEngine.GameObject gameObject = this.scrollGroup.AddToList(null, null);
		global::UISkillItem component = gameObject.GetComponent<global::UISkillItem>();
		global::SkillData data = skillData;
		component.toggle.onAction.RemoveAllListeners();
		component.toggle.onSelect.RemoveAllListeners();
		if (canLearn)
		{
			int index = this.scrollGroup.items.Count - 1;
			bool isActive = this.showActiveSkill;
			component.toggle.onAction.AddListener(delegate()
			{
				this.onSkillConfirmedCallback(index, isActive, data);
			});
		}
		component.toggle.onSelect.AddListener(delegate()
		{
			this.onSkillSelectedCallback(data);
		});
		component.Set(skillData, canLearn);
		if (select)
		{
			gameObject.SetSelected(true);
		}
	}

	private void Next()
	{
		this.currentSkillLineIndex = ((this.currentSkillLineIndex + 1 >= this.skillLinesIcons.Count) ? 0 : (this.currentSkillLineIndex + 1));
		if (this.skillLines.ContainsKey(this.skillLinesIcons[this.currentSkillLineIndex].skillLine))
		{
			this.SelectSkillLine(this.skillLinesIcons[this.currentSkillLineIndex].skillLine);
		}
		else
		{
			this.Next();
		}
	}

	private void Prev()
	{
		this.currentSkillLineIndex = ((this.currentSkillLineIndex - 1 < 0) ? (this.skillLinesIcons.Count - 1) : (this.currentSkillLineIndex - 1));
		if (this.skillLines.ContainsKey(this.skillLinesIcons[this.currentSkillLineIndex].skillLine))
		{
			this.SelectSkillLine(this.skillLinesIcons[this.currentSkillLineIndex].skillLine);
		}
		else
		{
			this.Prev();
		}
	}

	public void Update()
	{
		if (this.isFocus && !this.showMastery && !this.showSpell)
		{
			if (global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp("h", 0))
			{
				this.Next();
			}
			else if (global::PandoraSingleton<global::PandoraInput>.Instance.GetNegKeyUp("h", 0))
			{
				this.Prev();
			}
		}
	}

	public void ClearList()
	{
		this.isFocus = false;
		this.scrollGroup.ClearList();
		this.filterTitle.text = string.Empty;
		this.emptyListMessage.gameObject.SetActive(true);
	}

	private readonly global::SkillsShop skillsShop = new global::SkillsShop();

	public global::UnityEngine.UI.Text title;

	public global::UnityEngine.GameObject skillPrefab;

	public global::ScrollGroup scrollGroup;

	public global::System.Collections.Generic.List<global::SkillLinesTab> skillLinesIcons;

	public global::UnityEngine.GameObject unspentPoints;

	public global::UnityEngine.UI.Text pointsText;

	public global::UnityEngine.UI.Text emptyListMessage;

	public global::UnityEngine.UI.Text filterTitle;

	public global::UnityEngine.UI.Image warbandIcon;

	private int currentSkillLineIndex;

	private global::System.Collections.Generic.Dictionary<global::SkillLineId, global::System.Collections.Generic.List<global::SkillLineId>> skillLines;

	private global::System.Collections.Generic.List<global::SkillData> canLearnSkills;

	private global::System.Collections.Generic.List<global::SkillData> cannotLearnSkills;

	private global::System.Action<global::SkillData> onSkillSelectedCallback;

	private global::System.Action<int, bool, global::SkillData> onSkillConfirmedCallback;

	private bool isFocus;

	private bool showActiveSkill;

	private bool showMastery;

	private bool showSpell;

	public global::UnityEngine.CanvasGroup skillLinesGroup;

	public global::ButtonGroup btnPreviousFilter;

	public global::ButtonGroup btnNextFilter;
}
