using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarbandSwapModule : global::WarbandSlotPlacementModule
{
	public bool DeployRequested { get; private set; }

	public bool HasChanged { get; set; }

	public override void Init()
	{
		base.Init();
		global::UnityEngine.GameObject gameObject = global::UnityEngine.Object.Instantiate<global::UnityEngine.GameObject>(this.unitSheetPrefab);
		gameObject.transform.SetParent(this.firstUnitAnchor, false);
		this.firstUnitSheet = gameObject.GetComponent<global::UnitSheetModule>();
		gameObject = global::UnityEngine.Object.Instantiate<global::UnityEngine.GameObject>(this.unitQuickStatsPrefab);
		gameObject.transform.SetParent(this.firstUnitAnchor, false);
		this.firstUnitStats = gameObject.GetComponent<global::UnitQuickStatsModule>();
		gameObject = global::UnityEngine.Object.Instantiate<global::UnityEngine.GameObject>(this.unitSheetPrefab);
		gameObject.transform.SetParent(this.secondUnitAnchor, false);
		this.secondUnitSheet = gameObject.GetComponent<global::UnitSheetModule>();
		gameObject = global::UnityEngine.Object.Instantiate<global::UnityEngine.GameObject>(this.unitQuickStatsPrefab);
		gameObject.transform.SetParent(this.secondUnitAnchor, false);
		this.secondUnitStats = gameObject.GetComponent<global::UnitQuickStatsModule>();
		this.firstUnitSheet.gameObject.SetActive(false);
		this.firstUnitStats.gameObject.SetActive(false);
		this.secondUnitSheet.gameObject.SetActive(false);
		this.secondUnitStats.gameObject.SetActive(false);
	}

	public void Set(global::Warband warband, global::System.Action<bool> close, bool isMission, bool isCampaign, bool isContest, global::System.Collections.Generic.List<int> unitPosition, bool pushLayer, int ratingMin = 0, int ratingMax = 9999)
	{
		base.Set(warband, unitPosition, ratingMin, ratingMax);
		this.onClose = close;
		this.HasChanged = false;
		this.missionMode = isMission;
		this.warbandIcon.sprite = global::Warband.GetIcon(this.currentWarband.Id);
		this.warbandName.text = this.currentWarband.GetWarbandSave().Name;
		base.SetupAvailableSlots();
		this.firstUnit = null;
		this.firstUnitSheet.gameObject.SetActive(false);
		this.firstUnitStats.gameObject.SetActive(false);
		this.secondUnit = null;
		this.secondUnitSheet.gameObject.SetActive(false);
		this.secondUnitStats.gameObject.SetActive(false);
		this.maxSlotCount = 12 + global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.Warband.GetWarbandSlots().Reserve;
		this.leaderSlots[0].SetSelected(true);
		this.DeployRequested = false;
		this.layerPushed = pushLayer;
		if (pushLayer)
		{
			global::PandoraSingleton<global::PandoraInput>.Instance.PushInputLayer(global::PandoraInput.InputLayer.POP_UP);
		}
		global::PandoraSingleton<global::HideoutTabManager>.Instance.DeactivateAllButtons();
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button1.SetAction("cancel", "menu_back", global::PandoraSingleton<global::PandoraInput>.Instance.CurrentInputLayer, false, global::PandoraSingleton<global::HideoutTabManager>.Instance.icnBack, null);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button1.OnAction(delegate
		{
			this.Close(false);
		}, false, true);
		this.btnLaunch.SetAction(null, "menu_launch_mission", global::PandoraSingleton<global::PandoraInput>.Instance.CurrentInputLayer, false, null, null);
		this.btnLaunch.OnAction(delegate
		{
			this.Close(true);
		}, false, true);
		this.btnLaunch.gameObject.SetActive(this.missionMode);
		this.btnLaunchDeploy.SetAction(null, "menu_launch_mission_deploy", global::PandoraSingleton<global::PandoraInput>.Instance.CurrentInputLayer, false, null, null);
		this.btnLaunchDeploy.OnAction(delegate
		{
			this.DeployRequested = true;
			this.Close(true);
		}, false, true);
		this.btnLaunchDeploy.gameObject.SetActive(this.missionMode && !isCampaign);
		if (isMission || isContest)
		{
			this.CheckCanLaunchMission();
		}
		else
		{
			this.message.enabled = false;
		}
	}

	public void ForceClose()
	{
		if (this.layerPushed)
		{
			global::PandoraSingleton<global::PandoraInput>.Instance.PopInputLayer(global::PandoraInput.InputLayer.POP_UP);
		}
	}

	private void Close(bool valid)
	{
		this.ForceClose();
		if (this.onClose != null)
		{
			this.onClose(valid);
		}
	}

	public bool CanLaunchMission()
	{
		string text;
		return this.CanLaunchMission(out text);
	}

	public bool CanLaunchMission(out string reason)
	{
		global::Unit unitAtWarbandSlot = base.GetUnitAtWarbandSlot(2);
		if (unitAtWarbandSlot == null || (this.unitsPosition == null && unitAtWarbandSlot.GetActiveStatus() != global::UnitActiveStatusId.AVAILABLE))
		{
			reason = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById((this.unitsPosition != null) ? "na_hideout_leader" : "na_hideout_active_leader");
			return false;
		}
		if (base.GetActiveUnitsCount() < global::Constant.GetInt(global::ConstantId.MIN_MISSION_UNITS))
		{
			reason = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById((this.unitsPosition != null) ? "na_hideout_min_unit" : "na_hideout_min_active_unit");
			return false;
		}
		if (!global::PandoraUtils.IsBetween(global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.GetSkirmishRating(this.unitsPosition), this.warbandRatingMin, this.warbandRatingMax))
		{
			reason = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_rating_invalid", new string[]
			{
				this.warbandRatingMin.ToConstantString(),
				this.warbandRatingMax.ToConstantString()
			});
			return false;
		}
		bool flag = false;
		for (int i = 2; i < 12; i++)
		{
			global::Unit unitAtWarbandSlot2 = base.GetUnitAtWarbandSlot(i);
			if (unitAtWarbandSlot2 != null && unitAtWarbandSlot2.GetActiveStatus() != global::UnitActiveStatusId.AVAILABLE)
			{
				flag = true;
			}
		}
		reason = ((!flag) ? string.Empty : global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("na_hideout_warning_unavailable"));
		return true;
	}

	private void CheckCanLaunchMission()
	{
		string text;
		if (!this.CanLaunchMission(out text))
		{
			this.message.enabled = true;
			this.message.text = text;
			if (this.missionMode)
			{
				this.btnLaunch.SetDisabled(true);
				if (this.btnLaunchDeploy.gameObject.activeInHierarchy)
				{
					this.btnLaunchDeploy.SetDisabled(true);
				}
			}
		}
		else
		{
			this.message.enabled = true;
			this.message.text = ((!string.IsNullOrEmpty(text)) ? text : string.Empty);
			if (this.missionMode)
			{
				this.btnLaunch.SetDisabled(false);
				if (this.btnLaunchDeploy.gameObject.activeInHierarchy)
				{
					this.btnLaunchDeploy.SetDisabled(false);
				}
			}
		}
	}

	protected override void OnUnitSlotOver(int slotIndex, global::Unit unit, bool isImpressive)
	{
	}

	protected override void OnUnitSlotSelected(int slotIndex, global::Unit unit, bool isImpressive)
	{
		if (unit != null && this.firstUnit == null)
		{
			this.firstUnitSheet.gameObject.SetActive(true);
			this.firstUnitSheet.SetInteractable(false);
			this.firstUnitSheet.Refresh(null, unit, null, null, null);
			this.firstUnitStats.gameObject.SetActive(true);
			this.firstUnitStats.SetInteractable(false);
			this.firstUnitStats.RefreshStats(unit, null);
			this.secondUnitSheet.gameObject.SetActive(false);
			this.secondUnitStats.gameObject.SetActive(false);
		}
		else if (unit != this.firstUnit && this.IsSlotAvailableForSwap(slotIndex))
		{
			if (unit != null)
			{
				this.secondUnitSheet.gameObject.SetActive(true);
				this.secondUnitSheet.SetInteractable(false);
				this.secondUnitSheet.Refresh(null, unit, null, null, null);
				this.secondUnitStats.gameObject.SetActive(true);
				this.secondUnitStats.SetInteractable(false);
				this.secondUnitStats.RefreshStats(unit, this.firstUnit);
			}
			else
			{
				this.secondUnitSheet.gameObject.SetActive(false);
				this.secondUnitStats.gameObject.SetActive(false);
			}
			this.firstUnitStats.RefreshStats(this.firstUnit, unit);
		}
	}

	protected override void OnUnitSlotConfirmed(int slotIndex, global::Unit currentUnit, bool isImpressive)
	{
		if (currentUnit == null && isImpressive)
		{
			currentUnit = base.GetUnitAtWarbandSlot(slotIndex);
		}
		if (currentUnit != null || isImpressive)
		{
			if (this.firstUnit != null && currentUnit != this.firstUnit && this.IsSlotAvailableForSwap(slotIndex))
			{
				this.secondUnit = currentUnit;
				this.secondUnitSlotIndex = slotIndex;
				this.SwapUnits();
				global::PandoraSingleton<global::HideoutManager>.Instance.SaveChanges();
				this.HasChanged = true;
				this.FinishSwap();
				currentUnit = this.firstUnit;
				this.firstUnit = null;
				this.OnUnitSlotSelected(slotIndex, currentUnit, false);
			}
			else if (this.firstUnit == currentUnit)
			{
				this.allSlots[slotIndex].icon.color = global::UnityEngine.Color.white;
				this.firstUnit = null;
				this.FinishSwap();
			}
			else
			{
				this.firstUnit = currentUnit;
				this.firstUnitSlotIndex = slotIndex;
				this.FindSwappingNodes(this.firstUnit);
				this.StartSwap(this.firstUnitSlotIndex, this.cannotSwapNodes, isImpressive);
			}
		}
		else if (this.firstUnit != null && this.IsSlotAvailableForSwap(slotIndex))
		{
			base.SetUnitSlotIndex(this.firstUnit, slotIndex, false);
			this.firstUnit = null;
			global::PandoraSingleton<global::HideoutManager>.Instance.SaveChanges();
			this.HasChanged = true;
			this.FinishSwap();
			this.OnUnitSlotSelected(slotIndex, currentUnit, false);
		}
	}

	private bool IsSlotAvailableForSwap(int slotIndex)
	{
		for (int i = 0; i < this.cannotSwapNodes.Count; i++)
		{
			if (this.cannotSwapNodes[i] == slotIndex)
			{
				return false;
			}
		}
		return true;
	}

	private bool CanSwapImpressive(global::Unit srcImpressiveUnit, int srcSlotIndex, global::Unit dstUnit, int dstSlotIndex, global::Unit dstUnit2)
	{
		global::UnitId unitId = (dstUnit == null) ? global::UnitId.NONE : dstUnit.Id;
		global::UnitId unitId2 = (dstUnit2 == null) ? global::UnitId.NONE : dstUnit2.Id;
		return (!this.currentWarband.IsActiveWarbandSlot(srcSlotIndex) || unitId != unitId2 || unitId == global::UnitId.NONE || base.GetActiveUnitIdCount(unitId, null) + 2 <= dstUnit.Data.MaxCount) && (dstUnit == null || base.CanPlaceUnitAt(dstUnit, srcSlotIndex)) && (dstUnit2 == null || base.CanPlaceUnitAt(dstUnit2, srcSlotIndex + 1)) && (srcImpressiveUnit == null || base.CanPlaceUnitAt(srcImpressiveUnit, dstSlotIndex));
	}

	private bool FindSwappingNodes(global::Unit srcUnit)
	{
		this.cannotSwapNodes.Clear();
		bool result = false;
		int unitSlotIndex = base.GetUnitSlotIndex(srcUnit);
		bool isImpressive = srcUnit.IsImpressive;
		for (int i = 2; i < this.maxSlotCount; i++)
		{
			if (isImpressive && unitSlotIndex == i + 1)
			{
				this.cannotSwapNodes.Add(i + 1);
			}
			else
			{
				global::Unit unitAtWarbandSlot = base.GetUnitAtWarbandSlot(i);
				bool flag = unitAtWarbandSlot != null && unitAtWarbandSlot.IsImpressive;
				if (isImpressive && flag)
				{
					result = true;
				}
				else if (isImpressive)
				{
					if (this.CanSwapImpressive(srcUnit, unitSlotIndex, unitAtWarbandSlot, i, base.GetUnitAtWarbandSlot(i + 1)))
					{
						result = true;
					}
					else
					{
						this.cannotSwapNodes.Add(i);
					}
					i++;
				}
				else if (flag)
				{
					int linkedImpressiveSlotIndex = this.GetLinkedImpressiveSlotIndex(unitSlotIndex);
					if (linkedImpressiveSlotIndex + 1 < this.maxSlotCount)
					{
						global::Unit linkedImpressiveSlotUnit = this.GetLinkedImpressiveSlotUnit(unitSlotIndex);
						if (this.CanSwapImpressive(unitAtWarbandSlot, i, srcUnit, linkedImpressiveSlotIndex, linkedImpressiveSlotUnit))
						{
							result = true;
						}
						else
						{
							this.cannotSwapNodes.Add(i);
						}
					}
					else
					{
						this.cannotSwapNodes.Add(i);
					}
					i++;
				}
				else if (!base.CanPlaceUnitAt(srcUnit, i))
				{
					this.cannotSwapNodes.Add(i);
				}
				else if (unitAtWarbandSlot == null)
				{
					result = true;
				}
				else if (!base.CanPlaceUnitAt(unitAtWarbandSlot, unitSlotIndex))
				{
					this.cannotSwapNodes.Add(i);
				}
			}
		}
		return result;
	}

	private int GetLinkedImpressiveSlotIndex(int slotIndex)
	{
		if (slotIndex >= 12 + this.reserveSlots.Length)
		{
			return slotIndex;
		}
		if (slotIndex >= 12)
		{
			if ((slotIndex - 12) % 2 == 0)
			{
				return slotIndex;
			}
			return slotIndex - 1;
		}
		else
		{
			if ((slotIndex - 5) % 2 == 0)
			{
				return slotIndex;
			}
			return slotIndex - 1;
		}
	}

	private global::Unit GetLinkedImpressiveSlotUnit(int slotIndex)
	{
		if (slotIndex >= 12 + this.reserveSlots.Length)
		{
			return null;
		}
		if (slotIndex >= 12)
		{
			if ((slotIndex - 12) % 2 == 0)
			{
				return base.GetUnitAtWarbandSlot(slotIndex + 1);
			}
		}
		else if ((slotIndex - 5) % 2 == 0)
		{
			return base.GetUnitAtWarbandSlot(slotIndex + 1);
		}
		return base.GetUnitAtWarbandSlot(slotIndex - 1);
	}

	public void StartSwap(int fromSlotIndex, global::System.Collections.Generic.List<int> cannotSwapIndex, bool isImpressive)
	{
		global::Unit unitAtWarbandSlot = base.GetUnitAtWarbandSlot(fromSlotIndex);
		for (int i = 0; i < this.allSlots.Count; i++)
		{
			global::UIUnitSlot uiunitSlot = this.allSlots[i];
			if (!(uiunitSlot == null) && !uiunitSlot.isLocked)
			{
				if (cannotSwapIndex.Contains(i) || (isImpressive && i < 12 + this.reserveSlots.Length) || (fromSlotIndex >= 12 + this.reserveSlots.Length && unitAtWarbandSlot != null && unitAtWarbandSlot.IsImpressive))
				{					
					if (uiunitSlot.currentUnitAtSlot == null)
					{
						uiunitSlot.icon.color = global::UnityEngine.Color.white;
						uiunitSlot.icon.overrideSprite = this.noneIcon;
					}
					uiunitSlot.Deactivate();
				}
				else if (uiunitSlot.currentUnitAtSlot == null)
				{
					uiunitSlot.icon.color = global::UnityEngine.Color.white;
					uiunitSlot.icon.overrideSprite = this.swapIcon;
				}
			}			
		}
		if (isImpressive)
		{
			for (int j = 0; j < this.allImpressiveSlots.Count; j++)
			{
				global::UIUnitSlot uiunitSlot2 = this.allImpressiveSlots[j];
				if (!uiunitSlot2.isLocked)
				{
					if (uiunitSlot2.slotTypeIndex == fromSlotIndex)
					{
						uiunitSlot2.icon.color = global::Constant.GetColor(global::ConstantId.COLOR_CYAN);
					}
					else if (cannotSwapIndex.Contains(uiunitSlot2.slotTypeIndex))
					{
						this.allSlots[uiunitSlot2.slotTypeIndex].Deactivate();
						this.allSlots[uiunitSlot2.slotTypeIndex + 1].Deactivate();
						uiunitSlot2.Deactivate();
					}
					else
					{
						uiunitSlot2.Activate();
						if (uiunitSlot2.currentUnitAtSlot == null)
						{
							uiunitSlot2.icon.color = global::UnityEngine.Color.white;
							uiunitSlot2.icon.overrideSprite = this.swapIcon;
						}
					}
				}
			}
			return;
		}
		if (fromSlotIndex >= 12 + this.reserveSlots.Length && unitAtWarbandSlot != null && unitAtWarbandSlot.IsImpressive)
		{
			this.allSlots[fromSlotIndex].icon.color = global::Constant.GetColor(global::ConstantId.COLOR_CYAN);
			{
				for (int k = 0; k < this.allImpressiveSlots.Count; k++)
				{
					global::UIUnitSlot uiunitSlot3 = this.allImpressiveSlots[k];
					if (!uiunitSlot3.isLocked)
					{
						uiunitSlot3.Activate();
						if (uiunitSlot3.currentUnitAtSlot == null)
						{
							uiunitSlot3.icon.color = global::UnityEngine.Color.white;
							uiunitSlot3.icon.overrideSprite = this.swapIcon;
						}
					}
				}
			}
			return;
			{
				for (int l = 0; l < this.allImpressiveSlots.Count; l++)
				{
					global::UIUnitSlot uiunitSlot4 = this.allImpressiveSlots[l];
					if (!uiunitSlot4.isLocked)
					{
						if (cannotSwapIndex.Contains(uiunitSlot4.slotTypeIndex) || cannotSwapIndex.Contains(uiunitSlot4.slotTypeIndex + 1) || uiunitSlot4.currentUnitAtSlot == null)
						{
							uiunitSlot4.Deactivate();
						}
						else
						{
							uiunitSlot4.Activate();
						}
					}
				}
			}
		}
	}

	private void SwapUnits()
	{
		if (this.secondUnit != null && this.secondUnit.IsImpressive)
		{
			int linkedImpressiveSlotIndex = this.GetLinkedImpressiveSlotIndex(this.firstUnitSlotIndex);
			global::Unit linkedImpressiveSlotUnit = this.GetLinkedImpressiveSlotUnit(this.firstUnitSlotIndex);
			base.SetUnitSlotIndex(this.secondUnit, linkedImpressiveSlotIndex, false);
			if (linkedImpressiveSlotIndex - this.firstUnitSlotIndex == 0)
			{
				base.SetUnitSlotIndex(this.firstUnit, this.secondUnitSlotIndex, false);
				if (linkedImpressiveSlotUnit != null)
				{
					base.SetUnitSlotIndex(linkedImpressiveSlotUnit, this.secondUnitSlotIndex + 1, true);
				}
			}
			else
			{
				base.SetUnitSlotIndex(this.firstUnit, this.secondUnitSlotIndex + 1, true);
				if (linkedImpressiveSlotUnit != null)
				{
					base.SetUnitSlotIndex(linkedImpressiveSlotUnit, this.secondUnitSlotIndex, false);
				}
			}
		}
		else
		{
			if (this.secondUnit == null)
			{
				base.SetUnitSlotIndex(this.firstUnit, this.secondUnitSlotIndex, false);
			}
			else
			{
				base.SetUnitSlotIndex(this.firstUnit, this.secondUnitSlotIndex, false);
				base.SetUnitSlotIndex(this.secondUnit, this.firstUnitSlotIndex, false);
			}
			if (this.firstUnit.IsImpressive && (this.secondUnit == null || !this.secondUnit.IsImpressive))
			{
				global::Unit unitAtWarbandSlot = base.GetUnitAtWarbandSlot(this.secondUnitSlotIndex + 1);
				if (unitAtWarbandSlot != null)
				{
					base.SetUnitSlotIndex(unitAtWarbandSlot, this.firstUnitSlotIndex + 1, true);
				}
			}
		}
	}

	public void FinishSwap()
	{
		base.SetupAvailableSlots();
		this.firstUnitSheet.gameObject.SetActive(false);
		this.firstUnitStats.gameObject.SetActive(false);
		this.secondUnitSheet.gameObject.SetActive(false);
		this.secondUnitStats.gameObject.SetActive(false);
		this.CheckCanLaunchMission();
	}

	public new void Awake()
	{
		float num = 0.2f;
		if (this.reserveSlots != null && this.reserveSlots.Length == 8 && this.reserveImpressiveSlots != null && this.reserveImpressiveSlots.Length == 4)
		{
			global::UnityEngine.Transform parent = this.reserveImpressiveSlots[3].gameObject.transform.parent;
			if (parent)
			{
				parent = parent.parent;
				if (parent && parent.parent)
				{
					global::UnityEngine.Vector3 a = parent.localPosition - this.reserveImpressiveSlots[2].gameObject.transform.parent.parent.localPosition;
					this.reserveImpressiveSlots[1].gameObject.transform.parent.parent.localPosition -= a / 2f;
					this.reserveImpressiveSlots[0].gameObject.transform.parent.parent.localPosition -= a * num;
					this.reserveImpressiveSlots[2].gameObject.transform.parent.parent.localPosition += this.reserveImpressiveSlots[1].gameObject.transform.parent.parent.localPosition - this.reserveImpressiveSlots[2].gameObject.transform.parent.parent.localPosition + (this.reserveImpressiveSlots[1].gameObject.transform.parent.localPosition - this.reserveImpressiveSlots[2].gameObject.transform.parent.localPosition) + (this.reserveImpressiveSlots[1].gameObject.transform.localPosition - this.reserveImpressiveSlots[2].gameObject.transform.localPosition) + a * (num + 0.5f);
					parent.localPosition -= a * (num + 1f);
					base.CreateAdditionalReserveSlotGroup(parent, parent.localPosition + a * (num + 0.5f));
					base.CreateAdditionalReserveSlotGroup(parent, parent.localPosition + a * (2f * num + 1f));
				}
			}
		}
	}
	public global::ButtonGroup btnLaunch;

	public global::ButtonGroup btnLaunchDeploy;

	public global::UnityEngine.UI.Image warbandIcon;

	public global::UnityEngine.UI.Text warbandName;

	public global::UnityEngine.UI.Text message;

	public global::UnityEngine.GameObject unitSheetPrefab;

	public global::UnityEngine.GameObject unitQuickStatsPrefab;

	public global::Unit firstUnit;

	public global::UnityEngine.Transform firstUnitAnchor;

	private global::UnitSheetModule firstUnitSheet;

	private global::UnitQuickStatsModule firstUnitStats;

	public global::Unit secondUnit;

	public global::UnityEngine.Transform secondUnitAnchor;

	private global::UnitSheetModule secondUnitSheet;

	private global::UnitQuickStatsModule secondUnitStats;

	protected int firstUnitSlotIndex;

	protected int secondUnitSlotIndex;

	private global::System.Action<bool> onClose;

	private bool layerPushed;

	private bool missionMode;

	private global::System.Collections.Generic.List<int> cannotSwapNodes = new global::System.Collections.Generic.List<int>();

	private int maxSlotCount;
}
