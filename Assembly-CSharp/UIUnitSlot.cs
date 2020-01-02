using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIUnitSlot : global::UnityEngine.MonoBehaviour
{
	private void Awake()
	{
		this.slot = base.GetComponent<global::ToggleEffects>();
		this.canvasGroup = base.GetComponent<global::UnityEngine.CanvasGroup>();
		this.slot.onSelect.AddListener(new global::UnityEngine.Events.UnityAction(this.OnSelect));
		this.slot.onAction.AddListener(new global::UnityEngine.Events.UnityAction(this.OnConfirm));
		this.slot.onPointerEnter.AddListener(new global::UnityEngine.Events.UnityAction(this.OnOver));
	}

	private void OnOver()
	{
		if (this.overCallback != null)
		{
			this.overCallback(this.slotTypeIndex, this.currentUnitAtSlot, this.isImpressive);
		}
	}

	private void OnConfirm()
	{
		if (this.confirmedCallback != null)
		{
			this.confirmedCallback(this.slotTypeIndex, this.currentUnitAtSlot, this.isImpressive);
		}
	}

	private void OnSelect()
	{
		if (this.selectedCallback != null)
		{
			this.selectedCallback(this.slotTypeIndex, this.currentUnitAtSlot, this.isImpressive);
		}
	}

	public void Set(global::Unit unit, int index, global::System.Action<int, global::Unit, bool> over, global::System.Action<int, global::Unit, bool> selected, global::System.Action<int, global::Unit, bool> confirmed, bool showStatusIcon = true)
	{
		this.Set(index, over, selected, confirmed);
		this.currentUnitAtSlot = unit;
		if (unit != null)
		{
			this.icon.color = global::UnityEngine.Color.white;
			this.icon.overrideSprite = unit.GetIcon();
			if (this.unitTypeIcon != null)
			{
				switch (unit.GetUnitTypeId())
				{
				case global::UnitTypeId.LEADER:
					this.unitTypeIcon.enabled = true;
					this.unitTypeIcon.sprite = global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadResource<global::UnityEngine.Sprite>("icn_leader", true);
					goto IL_110;
				case global::UnitTypeId.IMPRESSIVE:
					this.unitTypeIcon.enabled = true;
					this.unitTypeIcon.sprite = global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadResource<global::UnityEngine.Sprite>("icn_impressive", true);
					goto IL_110;
				case global::UnitTypeId.HERO_1:
				case global::UnitTypeId.HERO_2:
				case global::UnitTypeId.HERO_3:
					this.unitTypeIcon.enabled = true;
					this.unitTypeIcon.sprite = global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadResource<global::UnityEngine.Sprite>("icn_heroes", true);
					goto IL_110;
				}
				this.unitTypeIcon.enabled = false;
			}
			IL_110:
			global::UnitActiveStatusId activeStatus = unit.GetActiveStatus();
			if (activeStatus != global::UnitActiveStatusId.AVAILABLE)
			{
				if (this.subIcon != null)
				{
					this.subIconObject.SetActive(showStatusIcon);
					this.subIcon.sprite = unit.GetActiveStatusIcon();
					this.subIcon.color = unit.GetActiveStatusIconColor();
					int activeStatusUnits = unit.GetActiveStatusUnits();
					this.subIconText.text = ((activeStatusUnits != 0) ? activeStatusUnits.ToConstantString() : string.Empty);
				}
				if (this.subIcon2 != null)
				{
					this.subIcon2.sprite = unit.GetSecondActiveStatusIcon();
					this.subIcon2.color = unit.GetSecondActiveStatusIconColor();
					this.subIconObject2.SetActive(showStatusIcon && this.subIcon2.sprite != null);
					int secondActiveStatusUnits = unit.GetSecondActiveStatusUnits();
					this.subIconText2.text = ((secondActiveStatusUnits != 0) ? secondActiveStatusUnits.ToConstantString() : string.Empty);
				}
			}
			else
			{
				if (this.subIconObject != null)
				{
					this.subIconObject.SetActive(false);
					this.subIconText.text = string.Empty;
				}
				if (this.subIconObject2 != null)
				{
					this.subIconObject2.SetActive(false);
					this.subIconText2.text = string.Empty;
				}
			}
			if (this.icnUnspentSkill != null)
			{
				this.icnUnspentSkill.gameObject.SetActive(unit.UnspentSkill > 0);
			}
		}
		else
		{
			this.icon.color = global::Constant.GetColor(global::ConstantId.COLOR_GOLD);
			this.icon.overrideSprite = null;
			if (this.unitTypeIcon != null)
			{
				this.unitTypeIcon.enabled = false;
			}
			if (this.subIcon != null)
			{
				this.subIconObject.SetActive(false);
				this.subIconText.text = string.Empty;
			}
			if (this.subIcon2 != null)
			{
				this.subIconObject2.SetActive(false);
				this.subIconText2.text = string.Empty;
			}
		}
	}

	public void Set(int index, global::System.Action<int, global::Unit, bool> over, global::System.Action<int, global::Unit, bool> selected, global::System.Action<int, global::Unit, bool> confirmed)
	{
		this.slotTypeIndex = index;
		this.currentUnitAtSlot = null;
		this.overCallback = over;
		this.selectedCallback = selected;
		this.confirmedCallback = confirmed;
		base.gameObject.SetActive(true);
		this.icon.overrideSprite = null;
		if (this.unitTypeIcon != null)
		{
			this.unitTypeIcon.enabled = false;
		}
		if (this.subIconObject != null)
		{
			this.subIconObject.SetActive(false);
		}
		if (this.subIconObject2 != null)
		{
			this.subIconObject2.SetActive(false);
		}
		if (this.icnUnspentSkill != null)
		{
			this.icnUnspentSkill.gameObject.SetActive(false);
		}
		this.isLocked = false;
	}

	public void Deactivate()
	{
		this.slot.toggle.interactable = false;
		if (this.icnUnspentSkill != null)
		{
			this.icnUnspentSkill.gameObject.SetActive(false);
		}
		this.canvasGroup.alpha = 0.33f;
	}

	public void Activate()
	{
		this.slot.toggle.interactable = true;
		this.canvasGroup.alpha = 1f;
	}

	public void Lock(global::UnityEngine.Sprite lockIcon)
	{
		this.icon.color = global::UnityEngine.Color.white;
		this.icon.overrideSprite = lockIcon;
		if (this.unitTypeIcon != null)
		{
			this.unitTypeIcon.enabled = false;
		}
		if (this.subIcon != null)
		{
			this.subIconObject.SetActive(false);
			this.subIconText.text = string.Empty;
		}
		if (this.subIcon2 != null)
		{
			this.subIconText2.text = string.Empty;
			this.subIconObject2.SetActive(false);
		}
		this.isLocked = true;
		this.Deactivate();
	}

	public void ShowImpressiveLinks(bool show)
	{
		if (this.impressiveLinks != null)
		{
			for (int i = 0; i < this.impressiveLinks.Count; i++)
			{
				this.impressiveLinks[i].SetActive(show);
			}
		}
	}

	[global::UnityEngine.HideInInspector]
	public global::ToggleEffects slot;

	public global::UnityEngine.UI.Image icon;

	public global::UnityEngine.UI.Image unitTypeIcon;

	public global::UnityEngine.GameObject subIconObject;

	public global::UnityEngine.UI.Image subIcon;

	public global::UnityEngine.UI.Text subIconText;

	public global::UnityEngine.GameObject subIconObject2;

	public global::UnityEngine.UI.Image subIcon2;

	public global::UnityEngine.UI.Text subIconText2;

	public global::UnityEngine.GameObject icnUnspentSkill;

	public bool isImpressive;

	public global::Unit currentUnitAtSlot;

	public int slotTypeIndex;

	[global::UnityEngine.HideInInspector]
	public global::UnityEngine.CanvasGroup canvasGroup;

	private global::System.Action<int, global::Unit, bool> overCallback;

	private global::System.Action<int, global::Unit, bool> selectedCallback;

	private global::System.Action<int, global::Unit, bool> confirmedCallback;

	public global::System.Collections.Generic.List<global::UnityEngine.GameObject> impressiveLinks;

	public bool isLocked;
}
