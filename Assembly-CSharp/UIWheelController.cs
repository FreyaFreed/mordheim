using System;
using System.Collections.Generic;
using UnityEngine;

public class UIWheelController : global::UIUnitControllerChanged
{
	private void Awake()
	{
		base.gameObject.SetActive(false);
		for (int i = 0; i < this.innerWheelIcons.Count; i++)
		{
			int catNo = i;
			this.innerWheelIcons[i].effects.onSelect.AddListener(delegate()
			{
				this.PreviewCategory(catNo);
			});
			this.innerWheelIcons[i].effects.onUnselect.AddListener(delegate()
			{
				this.UnHighlightCategory(catNo);
			});
			this.innerWheelIcons[i].effects.onAction.AddListener(delegate()
			{
				this.SelectCategory(catNo);
				this.OnInputAction(true);
			});
		}
		for (int j = 0; j < this.outerWheelIcons.Count; j++)
		{
			int actionId = j;
			this.outerWheelIcons[j].effects.onSelect.AddListener(delegate()
			{
				this.PreviewAction(actionId);
			});
			this.outerWheelIcons[j].effects.onUnselect.AddListener(delegate()
			{
				this.UnHighlightAction(actionId);
			});
			this.outerWheelIcons[j].effects.onAction.AddListener(delegate()
			{
				this.SetCurrentAction(actionId);
				this.OnInputAction(true);
			});
		}
	}

	public void Show()
	{
		if (!this.isShow)
		{
			this.activateAction = false;
			this.oldCurrentAction = base.CurrentUnitController.CurrentAction;
			if (base.CurrentUnitController.IsCurrentState(global::UnitController.State.MOVE))
			{
				this.oldActions = ((global::Moving)base.CurrentUnitController.StateMachine.GetActiveState()).actions;
			}
			else
			{
				this.oldActions = base.CurrentUnitController.availableActionStatus;
			}
			global::PandoraSingleton<global::PandoraInput>.Instance.SetCurrentState(global::PandoraInput.States.MISSION, true);
			global::PandoraSingleton<global::PandoraInput>.Instance.PushInputLayer(global::PandoraInput.InputLayer.WHEEL);
			if (this.categories == null)
			{
				this.categories = new global::System.Collections.Generic.List<global::System.Collections.Generic.List<global::WheelAction>>(6);
				for (int i = 0; i < 6; i++)
				{
					this.categories.Add(new global::System.Collections.Generic.List<global::WheelAction>());
				}
			}
			else
			{
				for (int j = 0; j < this.categories.Count; j++)
				{
					this.categories[j].Clear();
				}
			}
			this.FillCategories();
			this.selectingCategory = false;
			this.isShow = true;
			base.gameObject.SetActive(true);
			base.UpdateUnit = false;
			this.OpenCategory(base.CurrentUnitController.CurrentAction);
		}
	}

	private void FillCategories()
	{
		global::System.Collections.Generic.List<global::ActionStatus> list = new global::System.Collections.Generic.List<global::ActionStatus>();
		foreach (global::ActionStatus actionStatus in base.CurrentUnitController.actionStatus)
		{
			global::UIWheelController.Category category = global::UIWheelController.Category.NONE;
			switch (actionStatus.SkillId)
			{
			case global::SkillId.BASE_CLIMB:
			case global::SkillId.BASE_LEAP:
			case global::SkillId.BASE_JUMPDOWN:
				continue;
			case (global::SkillId)319:
			case (global::SkillId)320:
			case (global::SkillId)323:
			case (global::SkillId)324:
			case global::SkillId.BASE_SEARCH:
			case global::SkillId.BASE_END_TURN:
				goto IL_DD;
			case global::SkillId.BASE_PERCEPTION:
			case global::SkillId.BASE_SWITCH_WEAPONS:
			case global::SkillId.BASE_DISENGAGE:
			case global::SkillId.BASE_FLEE:
				category = global::UIWheelController.Category.BASE_ACTION;
				break;
			case global::SkillId.BASE_DELAY:
			case global::SkillId.BASE_STANCE_OVERWATCH:
			case global::SkillId.BASE_STANCE_AMBUSH:
			case global::SkillId.BASE_STANCE_PARRY:
			case global::SkillId.BASE_STANCE_DODGE:
				category = global::UIWheelController.Category.STANCES;
				break;
			case global::SkillId.BASE_ATTACK:
			case global::SkillId.BASE_CHARGE:
				if (base.CurrentUnitController.HasClose())
				{
					category = global::UIWheelController.Category.BASE_ACTION;
				}
				break;
			case global::SkillId.BASE_SHOOT:
			case global::SkillId.BASE_RELOAD:
			case global::SkillId.BASE_AIM:
				if (base.CurrentUnitController.HasRange())
				{
					category = global::UIWheelController.Category.BASE_ACTION;
				}
				break;
			default:
				goto IL_DD;
			}
			IL_104:
			if (category != global::UIWheelController.Category.NONE)
			{
				this.categories[(int)category].Add(new global::WheelAction(actionStatus, category));
				continue;
			}
			continue;
			IL_DD:
			global::UnitActionId actionId = actionStatus.ActionId;
			if (actionId == global::UnitActionId.CONSUMABLE)
			{
				list.Add(actionStatus);
			}
			goto IL_104;
		}
		for (int i = 0; i < base.CurrentUnitController.unit.PassiveSkills.Count; i++)
		{
			this.categories[2].Add(new global::WheelAction(new global::ActionStatus(base.CurrentUnitController.unit.PassiveSkills[i], base.CurrentUnitController), global::UIWheelController.Category.PASSIVE_SKILL));
		}
		for (int j = 0; j < base.CurrentUnitController.unit.ActiveSkills.Count; j++)
		{
			this.categories[1].Add(new global::WheelAction(base.CurrentUnitController.GetAction(base.CurrentUnitController.unit.ActiveSkills[j].Id), global::UIWheelController.Category.ACTIVE_SKILL));
		}
		for (int k = 0; k < base.CurrentUnitController.unit.Spells.Count; k++)
		{
			this.categories[3].Add(new global::WheelAction(base.CurrentUnitController.GetAction(base.CurrentUnitController.unit.Spells[k].Id), global::UIWheelController.Category.SPELLS));
		}
		int count = base.CurrentUnitController.unit.Items.Count;
		for (int l = 6; l < count; l++)
		{
			bool flag = false;
			for (int m = 0; m < list.Count; m++)
			{
				if (list[m].LinkedItem.Id == base.CurrentUnitController.unit.Items[l].Id && list[m].LinkedItem.QualityData.Id == base.CurrentUnitController.unit.Items[l].QualityData.Id)
				{
					flag = true;
					this.categories[5].Add(new global::WheelAction(list[m], base.CurrentUnitController.unit.Items[l], global::UIWheelController.Category.INVENTORY));
				}
			}
			if (!flag)
			{
				this.categories[5].Add(new global::WheelAction(base.CurrentUnitController.unit.Items[l]));
			}
		}
		for (int n = 0; n < this.categories.Count; n++)
		{
			this.innerWheelIcons[n].toggle.image.sprite = ((this.categories[n].Count == 0) ? this.notAvailableSprite : this.availableSprite);
		}
	}

	public override void OnDisable()
	{
		base.OnDisable();
		if (this.isShow)
		{
			global::PandoraSingleton<global::PandoraInput>.Instance.PopInputLayer(global::PandoraInput.InputLayer.WHEEL);
			global::PandoraSingleton<global::PandoraInput>.Instance.SetCurrentState(global::PandoraInput.States.MISSION, false);
			this.isShow = false;
			global::PandoraSingleton<global::PandoraInput>.Instance.SetActive(true);
			if (!this.activateAction)
			{
				base.CurrentUnitController.SetCurrentAction(this.oldCurrentAction.SkillId);
				global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<global::UnitController, global::ActionStatus, global::System.Collections.Generic.List<global::ActionStatus>>(global::Notices.CURRENT_UNIT_ACTION_CHANGED, base.CurrentUnitController, this.oldCurrentAction, this.oldActions);
			}
		}
	}

	private void OpenCategory(global::ActionStatus currentAction)
	{
		for (int i = 0; i < this.categories.Count; i++)
		{
			int num = this.categories[i].FindIndex((global::WheelAction x) => x.action == currentAction);
			if (num != -1)
			{
				this.SelectCategory(i);
				this.OpenCategory();
				this.SetCurrentAction(num);
				return;
			}
		}
		this.SelectCategory(0);
		this.OpenCategory();
		this.SetCurrentAction(0);
	}

	private void PreviewCategory(int categoryId)
	{
		this.innerWheelIcons[this.currentCategory].icon.color = global::UIWheelController.NotSelectedColor;
		this.innerWheelIcons[categoryId].icon.color = global::UIWheelController.SelectedColor;
		this.descriptionGroup.SetCurrentCategory((global::UIWheelController.Category)categoryId);
	}

	private void UnHighlightCategory(int categoryId)
	{
		this.innerWheelIcons[categoryId].icon.color = global::UIWheelController.NotSelectedColor;
	}

	private void SelectCategory(int category)
	{
		this.UnHighlightCategory(this.currentCategory);
		this.innerWheelIcons[this.currentCategory].toggle.isOn = false;
		this.selectingCategory = true;
		if (category < 0)
		{
			category = this.categories.Count - 1;
		}
		else if (category >= this.categories.Count)
		{
			category = 0;
		}
		this.PreviewCategory(category);
		this.currentCategory = category;
		this.innerWheelIcons[this.currentCategory].toggle.isOn = true;
		if (this.categories[this.currentCategory].Count > 0)
		{
			for (int i = 0; i < this.outerWheelIcons.Count; i++)
			{
				if (i < this.categories[this.currentCategory].Count)
				{
					global::WheelAction wheelAction = this.categories[this.currentCategory][i];
					this.outerWheelIcons[i].gameObject.SetActive(true);
					this.outerWheelIcons[i].icon.sprite = wheelAction.GetIcon();
					this.outerWheelIcons[i].mastery.enabled = (wheelAction.action != null && wheelAction.action.IsMastery);
					this.SetActionIconColor(i, false);
				}
				else
				{
					this.outerWheelIcons[i].gameObject.SetActive(false);
				}
			}
		}
		else
		{
			for (int j = 0; j < this.outerWheelIcons.Count; j++)
			{
				this.outerWheelIcons[j].gameObject.SetActive(false);
			}
		}
		this.descriptionGroup.SetCurrentCategory((global::UIWheelController.Category)this.currentCategory);
	}

	private void OpenCategory()
	{
		if (base.CurrentUnitController != null && this.categories[this.currentCategory].Count > 0)
		{
			this.SetCurrentAction(0);
			this.outerWheelIcons[0].SetSelected(false);
		}
	}

	private void SetActionIconColor(int index, bool selected)
	{
		if (index >= 0 && index < this.categories[this.currentCategory].Count)
		{
			if (selected)
			{
				this.outerWheelIcons[index].toggle.image.enabled = !this.categories[this.currentCategory][index].Available;
				this.outerWheelIcons[index].icon.color = global::UIWheelController.SelectedColor;
			}
			else
			{
				this.outerWheelIcons[index].toggle.image.enabled = !this.categories[this.currentCategory][index].Available;
				this.outerWheelIcons[index].icon.color = global::UIWheelController.NotSelectedColor;
			}
		}
		else
		{
			this.outerWheelIcons[index].toggle.image.enabled = false;
			this.outerWheelIcons[index].icon.color = global::UIWheelController.NotSelectedColor;
		}
	}

	private void PreviewAction(int actionIndex)
	{
		if (this.categories[this.currentCategory].Count == 0)
		{
			return;
		}
		this.descriptionGroup.SetCurrentCategory((global::UIWheelController.Category)this.currentCategory);
		this.UnHighlightAction(this.currentAction);
		this.SetActionIconColor(actionIndex, true);
		if (this.currentCategory >= 0 && this.currentCategory < this.categories.Count && actionIndex >= 0 && actionIndex < this.categories[this.currentCategory].Count)
		{
			this.descriptionGroup.SetCurrentAction(this.categories[this.currentCategory][actionIndex]);
		}
	}

	private void UnHighlightAction(int actionIndex)
	{
		this.SetActionIconColor(actionIndex, false);
	}

	private void SetCurrentAction(int index)
	{
		if (this.categories[this.currentCategory].Count == 0)
		{
			return;
		}
		this.outerWheelIcons[this.currentAction].toggle.isOn = false;
		this.selectingCategory = false;
		this.UnHighlightAction(this.currentAction);
		if (index < 0)
		{
			index = this.categories[this.currentCategory].Count - 1;
		}
		else if (index >= this.categories[this.currentCategory].Count)
		{
			index = 0;
		}
		this.PreviewAction(index);
		this.outerWheelIcons[index].toggle.isOn = true;
		this.currentAction = index;
		if (this.categories[this.currentCategory][index].Available)
		{
			base.CurrentUnitController.SetCurrentAction(this.categories[this.currentCategory][index].action.SkillId);
			global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<global::UnitController, global::ActionStatus, global::System.Collections.Generic.List<global::ActionStatus>>(global::Notices.CURRENT_UNIT_ACTION_CHANGED, base.CurrentUnitController, this.categories[this.currentCategory][index].action, null);
		}
		else
		{
			base.CurrentUnitController.SetCurrentAction(global::SkillId.NONE);
			global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<global::UnitController, global::System.Collections.Generic.List<global::ActionStatus>>(global::Notices.CURRENT_UNIT_ACTION_CHANGED, base.CurrentUnitController, null);
		}
	}

	private void OnInputCancel(bool canQuit)
	{
		if (!this.selectingCategory)
		{
			for (int i = 0; i < this.outerWheelIcons.Count; i++)
			{
				this.outerWheelIcons[i].toggle.isOn = false;
			}
			this.selectingCategory = true;
			base.CurrentUnitController.SetCurrentAction(global::SkillId.NONE);
			global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<global::UnitController, global::System.Collections.Generic.List<global::ActionStatus>>(global::Notices.CURRENT_UNIT_ACTION_CHANGED, base.CurrentUnitController, null);
			this.SelectCategory(this.currentCategory);
		}
		else if (canQuit)
		{
			base.gameObject.SetActive(false);
		}
	}

	private void OnInputAction(bool canActivateAction)
	{
		if (this.selectingCategory)
		{
			if (this.categories[this.currentCategory].Count > 0)
			{
				this.OpenCategory();
			}
		}
		else if (canActivateAction && this.categories[this.currentCategory][this.currentAction].Available)
		{
			this.activateAction = true;
			global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<global::UnitController, global::ActionStatus, global::System.Collections.Generic.List<global::ActionStatus>>(global::Notices.CURRENT_UNIT_ACTION_CHANGED, base.CurrentUnitController, this.categories[this.currentCategory][this.currentAction].action, this.oldActions);
			this.categories[this.currentCategory][this.currentAction].action.Select();
			base.gameObject.SetActive(false);
		}
	}

	public void Update()
	{
		if (this.isShow)
		{
			if (global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp("menu", 5) || global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp("wheel", 5))
			{
				base.gameObject.SetActive(false);
			}
			else if (global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp("navig_confirm", 5))
			{
				this.OnInputAction(!this.selectingCategory);
			}
			else if (global::PandoraSingleton<global::PandoraInput>.Instance.GetNegKeyUp("h", 5) || global::PandoraSingleton<global::PandoraInput>.Instance.GetNegKeyUp("navig_x", 5))
			{
				this.OnInputAction(false);
			}
			else if (global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp("cancel", 5))
			{
				this.OnInputCancel(true);
			}
			else if (global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp("h", 5) || global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp("navig_x", 5))
			{
				this.OnInputCancel(false);
			}
			else if (global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp("v", 5) || global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp("navig_y", 5))
			{
				if (this.selectingCategory)
				{
					int num = this.currentCategory - 1;
					if (num < 0)
					{
						num = this.categories.Count - 1;
					}
					this.innerWheelIcons[num].SetSelected(false);
					this.SelectCategory(num);
				}
				else if (this.categories[this.currentCategory].Count > 0)
				{
					int num2 = this.currentAction - 1;
					if (num2 < 0)
					{
						num2 = this.categories[this.currentCategory].Count - 1;
					}
					this.outerWheelIcons[num2].SetSelected(false);
					this.SetCurrentAction(num2);
				}
			}
			else if (global::PandoraSingleton<global::PandoraInput>.Instance.GetNegKeyUp("v", 5) || global::PandoraSingleton<global::PandoraInput>.Instance.GetNegKeyUp("navig_y", 5))
			{
				if (this.selectingCategory)
				{
					int num3 = this.currentCategory + 1;
					if (num3 >= this.categories.Count)
					{
						num3 = 0;
					}
					this.innerWheelIcons[num3].SetSelected(false);
					this.SelectCategory(num3);
				}
				else if (this.categories[this.currentCategory].Count > 0)
				{
					int num4 = this.currentAction + 1;
					if (num4 >= this.categories[this.currentCategory].Count)
					{
						num4 = 0;
					}
					this.outerWheelIcons[num4].SetSelected(false);
					this.SetCurrentAction(num4);
				}
			}
		}
	}

	protected override void OnUnitChanged()
	{
		base.gameObject.SetActive(false);
	}

	private const global::PandoraInput.InputLayer layer = global::PandoraInput.InputLayer.WHEEL;

	private static readonly global::UnityEngine.Color NotSelectedColor = new global::UnityEngine.Color(0.5f, 0.5f, 0.5f);

	private static readonly global::UnityEngine.Color SelectedColor = new global::UnityEngine.Color(1f, 1f, 1f);

	public global::UnityEngine.GameObject outerWheelBackGround;

	public global::UnityEngine.GameObject outerWheel;

	public global::System.Collections.Generic.List<global::UIWheelIcon> innerWheelIcons;

	public global::System.Collections.Generic.List<global::UIWheelIcon> outerWheelIcons;

	private bool isShow;

	public global::UnityEngine.Sprite availableSprite;

	public global::UnityEngine.Sprite notAvailableSprite;

	private int currentCategory;

	private int currentAction;

	private bool selectingCategory = true;

	private global::System.Collections.Generic.List<global::System.Collections.Generic.List<global::WheelAction>> categories;

	public global::UIWheelDescription descriptionGroup;

	private global::ActionStatus oldCurrentAction;

	private bool activateAction;

	private global::System.Collections.Generic.List<global::ActionStatus> oldActions;

	public enum Category
	{
		NONE = -1,
		BASE_ACTION,
		ACTIVE_SKILL,
		PASSIVE_SKILL,
		SPELLS,
		STANCES,
		INVENTORY,
		COUNT
	}
}
