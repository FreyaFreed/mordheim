using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HideoutCustomization : global::BaseHideoutUnitState
{
	public HideoutCustomization(global::HideoutManager mng, global::HideoutCamAnchor anchor) : base(anchor, global::HideoutManager.State.CUSTOMIZATION)
	{
		this.availableModels = new global::System.Collections.Generic.List<string>();
	}

	public override void Enter(int iFrom)
	{
		global::PandoraSingleton<global::HideoutTabManager>.Instance.ActivateLeftTabModules(true, new global::ModuleId[]
		{
			global::ModuleId.UNIT_SHEET,
			global::ModuleId.UNIT_CUSTOMIZATION
		});
		global::PandoraSingleton<global::HideoutTabManager>.Instance.ActivateRightTabModules(true, new global::ModuleId[]
		{
			global::ModuleId.UNIT_CUSTOMIZATION,
			global::ModuleId.TREASURY
		});
		if (global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.unitCtrlrs.Count > 1)
		{
			global::PandoraSingleton<global::HideoutTabManager>.Instance.ActivateCenterTabModules(new global::ModuleId[]
			{
				global::ModuleId.NEXT_UNIT,
				global::ModuleId.UNIT_TABS,
				global::ModuleId.TITLE,
				global::ModuleId.CHARACTER_AREA,
				global::ModuleId.UNIT_CUSTOMIZATION,
				global::ModuleId.UNIT_CUSTOMIZATION_DESC,
				global::ModuleId.NOTIFICATION
			});
			global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleCenter<global::NextUnitModule>(global::ModuleId.NEXT_UNIT).Setup();
		}
		else
		{
			global::PandoraSingleton<global::HideoutTabManager>.Instance.ActivateCenterTabModules(new global::ModuleId[]
			{
				global::ModuleId.UNIT_TABS,
				global::ModuleId.TITLE,
				global::ModuleId.CHARACTER_AREA,
				global::ModuleId.UNIT_CUSTOMIZATION,
				global::ModuleId.UNIT_CUSTOMIZATION_DESC,
				global::ModuleId.NOTIFICATION
			});
		}
		base.Init();
		this.sheetModule = global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleLeft<global::UnitSheetModule>(global::ModuleId.UNIT_SHEET);
		this.bioModule = global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleLeft<global::UnitBioModule>(global::ModuleId.UNIT_CUSTOMIZATION);
		this.custoDescModule = global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleCenter<global::CustomizationDescModule>(global::ModuleId.UNIT_CUSTOMIZATION_DESC);
		this.characterCamModule = global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleCenter<global::CharacterCameraAreaModule>(global::ModuleId.CHARACTER_AREA);
		this.characterCamModule.Init(this.camAnchor.transform.position);
		this.wheelModule = global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleCenter<global::CustomizationWheelModule>(global::ModuleId.UNIT_CUSTOMIZATION);
		this.wheelModule.IsFocused = true;
		this.wheelModule.Activate(null, new global::UnityEngine.Events.UnityAction<global::BodyPartId, global::UnityEngine.Sprite>(this.OnBodyPartHighlighted), new global::UnityEngine.Events.UnityAction<global::BodyPartId>(this.OnBodyPartSelected), new global::UnityEngine.Events.UnityAction<global::UnityEngine.Sprite>(this.OnPresetHighlighted), new global::UnityEngine.Events.UnityAction(this.OnPresetSelected), new global::UnityEngine.Events.UnityAction<global::UnityEngine.Sprite>(this.OnSkinHighlighted), new global::UnityEngine.Events.UnityAction(this.OnSkinSelected));
		this.unitCustomizationModule = global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleRight<global::UnitCustomizationModule>(global::ModuleId.UNIT_CUSTOMIZATION);
		this.unitCustomizationModule.onTabSelected = new global::System.Action<int>(this.OnTabSelected);
		this.SelectUnit(global::PandoraSingleton<global::HideoutManager>.Instance.currentUnit);
		this.bioModule.Setup(this.wheelModule.GetComponent<global::UnityEngine.UI.ToggleGroup>(), new global::System.Action<string>(this.OnNameChanged), new global::System.Action<string>(this.OnBioChanged), delegate
		{
			this.wheelModule.SelectLastSelected();
		});
		this.SetupWheelSlotSelectionButtons();
	}

	public override void Exit(int iTo)
	{
		base.Exit(iTo);
		this.unitCustomizationModule.Clear();
		this.unitCustomizationModule.SetFocused(false);
	}

	public override global::UnityEngine.UI.Selectable ModuleLeftOnRight()
	{
		return null;
	}

	public override void SelectUnit(global::UnitMenuController ctrlr)
	{
		base.SelectUnit(ctrlr);
		this.bioModule.SetName(ctrlr.unit.UnitSave.stats.Name);
		this.bioModule.SetBio(ctrlr.unit.UnitSave.bio);
		this.presetsData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::ColorPresetData>("fk_warband_id", ((int)ctrlr.unit.WarbandId).ToString()).ToDynList<global::ColorPresetData>();
		for (int i = this.presetsData.Count - 1; i >= 0; i--)
		{
			global::System.Collections.Generic.List<global::BodyPartColorData> list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::BodyPartColorData>(new string[]
			{
				"fk_unit_id",
				"fk_color_preset_id"
			}, new string[]
			{
				((int)ctrlr.unit.Id).ToString(),
				this.presetsData[i].Id.ToIntString<global::ColorPresetId>()
			});
			if (list.Count == 0)
			{
				this.presetsData.RemoveAt(i);
			}
		}
		this.availableSkins = new global::System.Collections.Generic.List<string>();
		global::System.Collections.Generic.List<global::UnitJoinSkinColorData> list2 = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::UnitJoinSkinColorData>("fk_unit_id", ctrlr.unit.Data.Id.ToIntString<global::UnitId>());
		for (int j = 0; j < list2.Count; j++)
		{
			this.availableSkins.Add(list2[j].SkinColorId.ToLowerString());
		}
		this.wheelModule.RefreshSlots(ctrlr, this.presetsData.Count > 0);
		this.ReturnToWheelSlotSelection();
	}

	public override bool CanIncreaseAttributes()
	{
		return false;
	}

	private void OnPresetChanged(int index)
	{
		this.currentUnit.SetColorPreset(this.presetsData[index].Id);
		global::PandoraSingleton<global::HideoutManager>.Instance.SaveChanges();
	}

	private void OnSkinChanged(int index)
	{
		this.currentUnit.SetSkinColor(this.availableSkins[index]);
		global::PandoraSingleton<global::HideoutManager>.Instance.SaveChanges();
		this.wheelModule.RefreshSlots(this.currentUnit, this.presetsData.Count > 0);
	}

	private void OnModelChanged(int index)
	{
		if (this.selectedBodyPart == global::BodyPartId.LEGL)
		{
			if (this.currentUnit.unit.bodyParts.ContainsKey(global::BodyPartId.LEGR))
			{
				this.currentUnit.SetModelVariation(global::BodyPartId.LEGR, index, true);
			}
			if (this.currentUnit.unit.bodyParts.ContainsKey(global::BodyPartId.LLEGR))
			{
				this.currentUnit.SetModelVariation(global::BodyPartId.LLEGR, index, true);
			}
		}
		else if (this.selectedBodyPart == global::BodyPartId.FOOTL && this.currentUnit.unit.bodyParts.ContainsKey(global::BodyPartId.FOOTR) && !this.currentUnit.unit.HasInjury(global::InjuryId.SEVERED_LEG))
		{
			this.currentUnit.SetModelVariation(global::BodyPartId.FOOTR, index, true);
		}
		this.currentUnit.SetModelVariation(this.selectedBodyPart, index, true);
		this.availableColours = this.currentUnit.unit.bodyParts[this.selectedBodyPart].GetAvailableMaterials(false);
		this.unitCustomizationModule.SetTabsVisible(true);
		global::PandoraSingleton<global::HideoutManager>.Instance.SaveChanges();
	}

	private void OnColorChanged(int index)
	{
		if (this.selectedBodyPart == global::BodyPartId.LEGL)
		{
			if (this.currentUnit.unit.bodyParts.ContainsKey(global::BodyPartId.LEGR))
			{
				this.currentUnit.SetBodyPartColor(global::BodyPartId.LEGR, index);
			}
			if (this.currentUnit.unit.bodyParts.ContainsKey(global::BodyPartId.LLEGR))
			{
				this.currentUnit.SetBodyPartColor(global::BodyPartId.LLEGR, index);
			}
		}
		else if (this.selectedBodyPart == global::BodyPartId.FOOTL && this.currentUnit.unit.bodyParts.ContainsKey(global::BodyPartId.FOOTR))
		{
			this.currentUnit.SetBodyPartColor(global::BodyPartId.FOOTR, index);
		}
		this.currentUnit.SetBodyPartColor(this.selectedBodyPart, index);
		global::PandoraSingleton<global::HideoutManager>.Instance.SaveChanges();
	}

	private void OnTabSelected(int idx)
	{
		if (idx != -1)
		{
			this.unitCustomizationModule.SetSelected(false);
		}
		if (idx == 0)
		{
			global::System.Collections.Generic.List<string> list = new global::System.Collections.Generic.List<string>(this.availableColours.Count);
			for (int i = 1; i <= this.availableColours.Count; i++)
			{
				list.Add(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_custom_style_param", new string[]
				{
					i.ToString()
				}));
			}
			this.unitCustomizationModule.Refresh(list, new global::System.Action<int>(this.OnColorChanged), string.Empty);
			int colorIndex = this.currentUnit.unit.bodyParts[this.selectedBodyPart].GetColorIndex();
			if (colorIndex < 256 && colorIndex >= 0 && colorIndex < list.Count)
			{
				this.unitCustomizationModule.SetSelectedStyle(colorIndex);
			}
			else if (list.Count > 0)
			{
				this.unitCustomizationModule.SetSelectedStyle(0);
			}
		}
		else if (idx == 1)
		{
			global::System.Collections.Generic.List<string> list2 = new global::System.Collections.Generic.List<string>(this.availableModels.Count);
			for (int j = 1; j <= this.availableModels.Count; j++)
			{
				list2.Add(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_custom_style_param", new string[]
				{
					j.ToString()
				}));
			}
			this.unitCustomizationModule.Refresh(list2, new global::System.Action<int>(this.OnModelChanged), string.Empty);
			int variation = this.currentUnit.unit.bodyParts[this.selectedBodyPart].GetVariation();
			if (variation >= 0 && variation < list2.Count)
			{
				this.unitCustomizationModule.SetSelectedStyle(variation);
			}
			else if (list2.Count > 0)
			{
				this.unitCustomizationModule.SetSelectedStyle(0);
			}
		}
	}

	private void OnPresetHighlighted(global::UnityEngine.Sprite icon)
	{
		this.custoDescModule.Set(icon, "hideout_custom_preset", "hideout_custom_preset_desc");
	}

	private void OnPresetSelected()
	{
		this.SetupCustomizationButtons();
		this.unitCustomizationModule.SetFocused(true);
		this.unitCustomizationModule.SetTabsVisible(false);
		global::System.Collections.Generic.List<string> list = this.presetsData.ConvertAll<string>((global::ColorPresetData x) => global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_custom_color_preset_" + x.Name.ToLowerInvariant()));
		this.unitCustomizationModule.Refresh(list, new global::System.Action<int>(this.OnPresetChanged), global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_custom_preset"));
		if (list.Count > 0)
		{
			this.unitCustomizationModule.SetSelectedStyle(0);
		}
	}

	private void OnSkinHighlighted(global::UnityEngine.Sprite icon)
	{
		this.custoDescModule.Set(icon, "hideout_custom_skin_title", "hideout_custom_skin_desc");
	}

	private void OnSkinSelected()
	{
		this.SetupCustomizationButtons();
		this.unitCustomizationModule.SetFocused(true);
		this.unitCustomizationModule.SetTabsVisible(false);
		global::System.Collections.Generic.List<string> styles = this.availableSkins.ConvertAll<string>((string x) => global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_custom_color_skin_" + x.ToLowerInvariant()));
		this.unitCustomizationModule.Refresh(styles, new global::System.Action<int>(this.OnSkinChanged), global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("hideout_custom_skin_title"));
		string text = this.currentUnit.unit.UnitSave.skinColor;
		if (string.IsNullOrEmpty(text))
		{
			text = this.availableSkins[0];
		}
		for (int i = 0; i < this.availableSkins.Count; i++)
		{
			if (this.availableSkins[i] == text)
			{
				this.unitCustomizationModule.SetSelectedStyle(i);
				break;
			}
		}
	}

	private void OnBodyPartHighlighted(global::BodyPartId bodyPart, global::UnityEngine.Sprite icon)
	{
		if (this.currentUnit.unit.bodyParts.ContainsKey(bodyPart))
		{
			string str = this.currentUnit.unit.bodyParts[bodyPart].Name.ToLowerInvariant();
			this.custoDescModule.Set(icon, "hideout_custom_title_" + str, "hideout_custom_desc_" + str);
		}
	}

	private void OnBodyPartSelected(global::BodyPartId bodyPart)
	{
		this.selectedBodyPart = bodyPart;
		global::UnityEngine.Transform bodyPartLookAtTarget = this.GetBodyPartLookAtTarget(this.selectedBodyPart);
		if (bodyPartLookAtTarget == null)
		{
			this.characterCamModule.SetCameraLookAtDefault(false);
		}
		else
		{
			this.characterCamModule.SetCameraLookAt(bodyPartLookAtTarget, false);
		}
		this.availableModels = this.currentUnit.unit.bodyParts[this.selectedBodyPart].GetAvailableModels();
		this.availableColours = this.currentUnit.unit.bodyParts[this.selectedBodyPart].GetAvailableMaterials(false);
		this.SetupCustomizationButtons();
		this.unitCustomizationModule.SetTabsVisible(true);
		this.OnTabSelected(this.unitCustomizationModule.GetSelectedTabIndex());
		this.unitCustomizationModule.SetFocused(true);
	}

	private void ReturnToWheelSlotSelection()
	{
		this.SetupWheelSlotSelectionButtons();
		this.unitCustomizationModule.Clear();
		this.unitCustomizationModule.SetFocused(false);
		this.wheelModule.SelectLastSelected();
		this.wheelModule.IsFocused = true;
	}

	private void SetupCustomizationButtons()
	{
		if (global::UnityEngine.Input.mousePresent)
		{
			global::PandoraSingleton<global::HideoutTabManager>.Instance.button1.SetAction("cancel", "go_to_warband", 0, false, global::PandoraSingleton<global::HideoutTabManager>.Instance.icnBack, null);
			global::PandoraSingleton<global::HideoutTabManager>.Instance.button1.OnAction(new global::UnityEngine.Events.UnityAction(base.ReturnToWarband), true, true);
		}
		else
		{
			global::PandoraSingleton<global::HideoutTabManager>.Instance.button1.gameObject.SetActive(false);
		}
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button2.SetAction("cancel", "menu_return_select_slot", 0, false, null, null);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button2.OnAction(new global::UnityEngine.Events.UnityAction(this.ReturnToWheelSlotSelection), false, true);
	}

	private void SetupWheelSlotSelectionButtons()
	{
		global::PandoraSingleton<global::HideoutTabManager>.Instance.DeactivateAllButtons();
		if (global::UnityEngine.Input.mousePresent)
		{
			global::PandoraSingleton<global::HideoutTabManager>.Instance.button1.SetAction("cancel", "go_to_warband", 0, false, global::PandoraSingleton<global::HideoutTabManager>.Instance.icnBack, null);
		}
		else
		{
			global::PandoraSingleton<global::HideoutTabManager>.Instance.button1.SetAction("cancel", "go_to_warband", 0, false, null, null);
		}
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button1.OnAction(new global::UnityEngine.Events.UnityAction(base.ReturnToWarband), false, true);
	}

	private void OnNameChanged(string newName)
	{
		if (!string.IsNullOrEmpty(newName))
		{
			this.currentUnit.unit.UnitSave.stats.overrideName = newName;
			this.sheetModule.unitName.text = newName;
			this.bioModule.SetName(newName);
			global::PandoraSingleton<global::HideoutManager>.Instance.SaveChanges();
		}
		this.ReturnToWheelSlotSelection();
	}

	private void OnBioChanged(string newBio)
	{
		if (!string.IsNullOrEmpty(newBio))
		{
			this.currentUnit.unit.UnitSave.bio = newBio;
			this.bioModule.SetBio(newBio);
			global::PandoraSingleton<global::HideoutManager>.Instance.SaveChanges();
		}
		this.ReturnToWheelSlotSelection();
	}

	private global::UnityEngine.Transform GetBodyPartLookAtTarget(global::BodyPartId bodyPart)
	{
		switch (bodyPart)
		{
		case global::BodyPartId.HELMET:
		case global::BodyPartId.HEAD:
		case global::BodyPartId.GEAR_HELMET:
		case global::BodyPartId.GEAR_HEAD:
		case global::BodyPartId.GEAR_FACE:
			if (this.currentUnit.BonesTr.ContainsKey(global::BoneId.RIG_HEAD))
			{
				return this.currentUnit.BonesTr[global::BoneId.RIG_HEAD];
			}
			goto IL_264;
		case global::BodyPartId.BODY:
		case global::BodyPartId.GEAR_BACK:
		case global::BodyPartId.GEAR_BODY:
			if (this.currentUnit.BonesTr.ContainsKey(global::BoneId.RIG_SPINE3))
			{
				return this.currentUnit.BonesTr[global::BoneId.RIG_SPINE3];
			}
			goto IL_264;
		case global::BodyPartId.ARMR:
		case global::BodyPartId.HANDR:
			goto IL_C9;
		default:
			switch (bodyPart)
			{
			case global::BodyPartId.FOOTR:
				if (this.currentUnit.BonesTr.ContainsKey(global::BoneId.RIG_RLEGANKLE))
				{
					return this.currentUnit.BonesTr[global::BoneId.RIG_RLEGANKLE];
				}
				goto IL_264;
			case global::BodyPartId.SHOULDERR:
				goto IL_235;
			case global::BodyPartId.GEAR_LOIN:
				goto IL_1DB;
			case global::BodyPartId.GEAR_ARMR:
				goto IL_C9;
			case global::BodyPartId.GEAR_ARML:
				goto IL_9C;
			}
			return null;
		case global::BodyPartId.ARML:
		case global::BodyPartId.HANDL:
			break;
		case global::BodyPartId.LEGR:
		case global::BodyPartId.LLEGR:
			if (this.currentUnit.BonesTr.ContainsKey(global::BoneId.RIG_RLEG21))
			{
				return this.currentUnit.BonesTr[global::BoneId.RIG_RLEG21];
			}
			goto IL_264;
		case global::BodyPartId.LEGL:
			if (this.currentUnit.BonesTr.ContainsKey(global::BoneId.RIG_LLEG21))
			{
				return this.currentUnit.BonesTr[global::BoneId.RIG_LLEG21];
			}
			goto IL_264;
		case global::BodyPartId.FOOTL:
			if (this.currentUnit.BonesTr.ContainsKey(global::BoneId.RIG_LLEGANKLE))
			{
				return this.currentUnit.BonesTr[global::BoneId.RIG_LLEGANKLE];
			}
			goto IL_264;
		case global::BodyPartId.GEAR_NECK:
			goto IL_235;
		case global::BodyPartId.GEAR_BELT:
		case global::BodyPartId.GEAR_LEGS:
			goto IL_1DB;
		}
		IL_9C:
		if (this.currentUnit.BonesTr.ContainsKey(global::BoneId.RIG_LARMPALM))
		{
			return this.currentUnit.BonesTr[global::BoneId.RIG_LARMPALM];
		}
		goto IL_264;
		IL_C9:
		if (this.currentUnit.BonesTr.ContainsKey(global::BoneId.RIG_RARMPALM))
		{
			return this.currentUnit.BonesTr[global::BoneId.RIG_RARMPALM];
		}
		goto IL_264;
		IL_1DB:
		if (this.currentUnit.BonesTr.ContainsKey(global::BoneId.RIG_PELVIS))
		{
			return this.currentUnit.BonesTr[global::BoneId.RIG_PELVIS];
		}
		goto IL_264;
		IL_235:
		if (this.currentUnit.BonesTr.ContainsKey(global::BoneId.RIG_NECK1))
		{
			return this.currentUnit.BonesTr[global::BoneId.RIG_NECK1];
		}
		IL_264:
		return null;
	}

	private global::BodyPartId selectedBodyPart;

	private bool isActive;

	private global::UnitCustomizationModule unitCustomizationModule;

	private global::CustomizationWheelModule wheelModule;

	private global::CustomizationDescModule custoDescModule;

	private global::UnitBioModule bioModule;

	private global::System.Collections.Generic.List<global::ColorPresetData> presetsData;

	private global::System.Collections.Generic.List<string> availableSkins;

	private global::System.Collections.Generic.List<string> availableColours;

	private global::System.Collections.Generic.List<string> availableModels;
}
