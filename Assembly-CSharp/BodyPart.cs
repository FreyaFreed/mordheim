using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class BodyPart
{
	public BodyPart(global::BodyPartData partData, global::UnitId unitId, string warbandAsset, string unitAsset, string altUnitAsset, string unitSkinColor, int colorIndex, int var)
	{
		this.data = partData;
		this.relatedUnitId = unitId;
		this.warband = warbandAsset;
		this.unit = unitAsset;
		this.altUnit = altUnitAsset;
		this.variation = var;
		this.colorIdx = colorIndex;
		this.Empty = this.data.Empty;
		this.needAssetRefresh = true;
		this.AssetNeedReload = true;
		this.relatedGO = new global::System.Collections.Generic.List<global::UnityEngine.GameObject>();
		this.SetSkinColor(unitSkinColor);
		global::BodyPartId enumValue = (this.data.Id != global::BodyPartId.LLEGR) ? this.data.Id : global::BodyPartId.LEGR;
		this.bodyPartsValues = new string[]
		{
			unitId.ToIntString<global::UnitId>(),
			((global::ColorPresetId)(colorIndex >> 8)).ToIntString<global::ColorPresetId>(),
			enumValue.ToIntString<global::BodyPartId>(),
			global::ItemTypeId.NONE.ToIntString<global::ItemTypeId>()
		};
		this.bodyPartsValuesNoPreset = new string[]
		{
			unitId.ToIntString<global::UnitId>(),
			enumValue.ToIntString<global::BodyPartId>(),
			global::ItemTypeId.NONE.ToIntString<global::ItemTypeId>()
		};
		this.locked = false;
		string key = warbandAsset + unitAsset;
		this.models = global::BodyPartModelIdData.Data[key];
		this.materials = global::BodyPartMaterialIdData.Data[key];
	}

	public global::BodyPartId Id
	{
		get
		{
			return this.data.Id;
		}
	}

	public string Name
	{
		get
		{
			return this.data.Name;
		}
	}

	public bool Empty { get; private set; }

	public bool Customizable
	{
		get
		{
			return !this.locked && this.MutationId == global::MutationId.NONE && this.InjuryId == global::InjuryId.NONE;
		}
	}

	public bool AssetNeedReload { get; set; }

	public string AssetBundle { get; private set; }

	public global::MutationId MutationId { get; private set; }

	public global::InjuryId InjuryId { get; private set; }

	public string Color { get; private set; }

	public void DestroyRelatedGO()
	{
		if (this.relatedGO.Count > 0)
		{
			for (int i = 0; i < this.relatedGO.Count; i++)
			{
				global::UnityEngine.Object.DestroyImmediate(this.relatedGO[i]);
			}
			this.relatedGO.Clear();
		}
	}

	public void SetLocked(bool locked)
	{
		this.locked = locked;
		this.SetEmpty(locked);
	}

	public void SetEmpty(bool empty)
	{
		this.Empty = empty;
		this.AssetNeedReload = true;
	}

	public void SetInjury(global::InjuryId injId)
	{
		this.InjuryId = injId;
		this.Empty = false;
		this.needAssetRefresh = true;
		this.AssetNeedReload = true;
	}

	public void SetMutation(global::MutationId mutId)
	{
		this.MutationId = mutId;
		this.Empty = false;
		this.needAssetRefresh = true;
		this.AssetNeedReload = true;
	}

	public void SetVariation(int index)
	{
		if (index != this.variation)
		{
			this.variation = index;
			this.Empty = false;
			this.AssetNeedReload = true;
			this.needAssetRefresh = true;
		}
	}

	public int GetVariation()
	{
		return this.variation;
	}

	public void SetRelatedItem(global::Item item)
	{
		if (this.relatedItem == null || item.Id != this.relatedItem.Id)
		{
			this.relatedItem = item;
			this.Empty = (this.MutationId == global::MutationId.NONE && this.InjuryId == global::InjuryId.NONE && this.data.UnitSlotId != global::UnitSlotId.NONE && item.Id == global::ItemId.NONE);
			this.locked = this.Empty;
			this.bodyPartsValues[3] = this.relatedItem.TypeData.Id.ToIntString<global::ItemTypeId>();
			this.bodyPartsValuesNoPreset[2] = this.relatedItem.TypeData.Id.ToIntString<global::ItemTypeId>();
			this.needAssetRefresh = true;
			this.AssetNeedReload = true;
		}
	}

	public global::Item GetRelatedItem()
	{
		return this.relatedItem;
	}

	public string GetAsset(global::ItemTypeId preferredItemType)
	{
		if (this.Empty)
		{
			return string.Empty;
		}
		if (!this.needAssetRefresh)
		{
			return this.assetName;
		}
		global::BodyPart.LogBldr.Length = 0;
		this.Empty = false;
		this.assetName = string.Empty;
		this.AssetBundle = global::PandoraUtils.StringBuilder.AppendFormat("{0}_{1}", this.warband, this.unit).ToString();
		if (this.InjuryId != global::InjuryId.NONE)
		{
			string text = "01";
			string asset = global::PandoraUtils.StringBuilder.AppendFormat("{0}_{1}_{2}_injury_{3}", new object[]
			{
				this.warband,
				this.unit,
				this.data.Name,
				text
			}).ToString();
			this.assetName = this.FindBodyPart(asset);
			if (string.IsNullOrEmpty(this.assetName))
			{
				asset = global::PandoraUtils.StringBuilder.AppendFormat("{0}_{1}_{2}_injury_{3}", new object[]
				{
					this.warband,
					this.altUnit,
					this.data.Name,
					text
				}).ToString();
				this.assetName = this.FindBodyPart(asset);
			}
			if (string.IsNullOrEmpty(this.assetName))
			{
				this.Empty = true;
				return string.Empty;
			}
			if (this.assetName.Contains("00"))
			{
				this.Empty = true;
				return string.Empty;
			}
		}
		if (this.MutationId != global::MutationId.NONE)
		{
			string text2 = "01";
			string text3 = this.MutationId.ToLowerString();
			string asset2 = global::PandoraUtils.StringBuilder.AppendFormat("{0}_{1}_{2}_{3}_{4}", new object[]
			{
				this.warband,
				this.unit,
				this.data.Name,
				text3,
				text2
			}).ToString();
			this.assetName = this.FindBodyPart(asset2);
			if (string.IsNullOrEmpty(this.assetName))
			{
				text3 = text3.Substring(0, text3.LastIndexOf("_"));
				asset2 = global::PandoraUtils.StringBuilder.AppendFormat("{0}_{1}_{2}_{3}_{4}", new object[]
				{
					this.warband,
					this.unit,
					this.data.Name,
					text3,
					text2
				}).ToString();
				this.assetName = this.FindBodyPart(asset2);
			}
			if (string.IsNullOrEmpty(this.assetName))
			{
				this.Empty = true;
				return string.Empty;
			}
			if (this.assetName.Contains("00"))
			{
				this.Empty = true;
				return string.Empty;
			}
		}
		if (string.IsNullOrEmpty(this.assetName))
		{
			global::System.Collections.Generic.List<string> availableModels = this.GetAvailableModels();
			if (availableModels.Count > 0)
			{
				if (this.variation == -1 || this.variation >= availableModels.Count)
				{
					global::System.Collections.Generic.List<string> list = new global::System.Collections.Generic.List<string>();
					for (int i = 0; i < availableModels.Count; i++)
					{
						if (availableModels[i].Contains("01") && !availableModels[i].Contains("injury") && !availableModels[i].Contains("mutation"))
						{
							list.Add(availableModels[i]);
						}
					}
					if (list.Count > 0)
					{
						this.assetName = list[0];
						string value = string.Empty;
						switch (preferredItemType)
						{
						case global::ItemTypeId.CLOTH_ARMOR:
							value = "cloth";
							break;
						case global::ItemTypeId.LIGHT_ARMOR:
							value = "armorl";
							break;
						case global::ItemTypeId.HEAVY_ARMOR:
							value = "armorh";
							break;
						}
						if (!string.IsNullOrEmpty(value))
						{
							for (int j = 0; j < list.Count; j++)
							{
								if (list[j].Contains(value))
								{
									this.assetName = list[j];
									break;
								}
							}
						}
					}
					if (string.IsNullOrEmpty(this.assetName))
					{
						this.assetName = availableModels[0];
					}
				}
				else
				{
					this.assetName = availableModels[this.variation];
				}
			}
		}
		if (this.assetName.Contains(this.altUnit))
		{
			this.AssetBundle = global::PandoraUtils.StringBuilder.AppendFormat("{0}_{1}", this.warband, this.altUnit).ToString();
		}
		if (this.assetName.Contains("armorh"))
		{
			this.bodyPartsValues[3] = global::ItemTypeId.HEAVY_ARMOR.ToIntString<global::ItemTypeId>();
		}
		else if (this.assetName.Contains("armorl"))
		{
			this.bodyPartsValues[3] = global::ItemTypeId.LIGHT_ARMOR.ToIntString<global::ItemTypeId>();
		}
		else if (this.assetName.Contains("cloth"))
		{
			this.bodyPartsValues[3] = global::ItemTypeId.CLOTH_ARMOR.ToIntString<global::ItemTypeId>();
		}
		if (string.IsNullOrEmpty(this.assetName))
		{
			this.Empty = true;
			return string.Empty;
		}
		if (this.assetName.Contains("00"))
		{
			this.Empty = true;
			return string.Empty;
		}
		this.Empty = false;
		return this.GetMaterial();
	}

	public global::System.Collections.Generic.List<string> GetAvailableModels()
	{
		global::System.Collections.Generic.List<string> result = new global::System.Collections.Generic.List<string>();
		string name;
		if (this.relatedItem != null && this.relatedItem.Id != global::ItemId.NONE && (this.Id == global::BodyPartId.BODY || this.Id == global::BodyPartId.GEAR_BODY || (!this.relatedItem.Asset.Contains("armorh") && !this.relatedItem.Asset.Contains("armorl") && !this.relatedItem.Asset.Contains("cloth"))))
		{
			name = global::PandoraUtils.StringBuilder.AppendFormat("{0}_{1}_{2}_{3}", new object[]
			{
				this.warband,
				this.unit,
				this.data.Name,
				this.relatedItem.Asset
			}).ToString();
			this.InitDataLike(ref result, name, this.models, true, false);
			if (this.unit != this.altUnit)
			{
				name = global::PandoraUtils.StringBuilder.AppendFormat("{0}_{1}_{2}_{3}", new object[]
				{
					this.warband,
					this.altUnit,
					this.data.Name,
					this.relatedItem.Asset
				}).ToString();
				this.InitDataLike(ref result, name, this.models, true, false);
			}
		}
		bool includeAltArmor = this.Id != global::BodyPartId.BODY && this.Id != global::BodyPartId.GEAR_BODY;
		name = global::PandoraUtils.StringBuilder.AppendFormat("{0}_{1}_{2}", this.warband, this.unit, this.data.Name).ToString();
		this.InitDataLike(ref result, name, this.models, true, includeAltArmor);
		if (this.unit != this.altUnit)
		{
			name = global::PandoraUtils.StringBuilder.AppendFormat("{0}_{1}_{2}", this.warband, this.altUnit, this.data.Name).ToString();
			this.InitDataLike(ref result, name, this.models, true, includeAltArmor);
		}
		return result;
	}

	private global::System.Collections.Generic.List<string> InitDataLike(ref global::System.Collections.Generic.List<string> foundModels, string name, global::System.Collections.Generic.List<string> list, bool clean, bool includeAltArmor)
	{
		for (int i = 0; i < list.Count; i++)
		{
			string text = list[i];
			if (text.StartsWith(name, global::System.StringComparison.Ordinal))
			{
				if (clean && text[name.Length + 1] != '0')
				{
					string text2 = text.Substring(name.Length);
					if (includeAltArmor && (text2.Contains("armorh") || text2.Contains("armorl") || text2.Contains("cloth")))
					{
						foundModels.Add(text);
					}
				}
				else
				{
					foundModels.Add(text);
				}
			}
		}
		return foundModels;
	}

	private string FindBodyPart(string asset)
	{
		if (asset.EndsWith("00"))
		{
			return null;
		}
		for (int i = 0; i < this.models.Count; i++)
		{
			if (string.Compare(this.models[i], asset, global::System.StringComparison.OrdinalIgnoreCase) == 0)
			{
				return asset;
			}
		}
		return null;
	}

	public void SetSkinColor(string skinColor)
	{
		if (skinColor != this.skinColor)
		{
			this.skinColor = skinColor;
			int num = skinColor.IndexOf('_');
			if (num != -1)
			{
				this.altSkinColor = skinColor.Substring(0, num);
			}
			else
			{
				this.altSkinColor = string.Empty;
			}
			this.needAssetRefresh = true;
			this.AssetNeedReload = true;
		}
	}

	public void SetColorPreset(int offsetPreset)
	{
		if (offsetPreset != this.colorIdx)
		{
			this.colorIdx = offsetPreset;
			this.bodyPartsValues[1] = ((global::ColorPresetId)(this.colorIdx >> 8)).ToIntString<global::ColorPresetId>();
			this.needAssetRefresh = true;
			this.AssetNeedReload = true;
		}
	}

	public void SetColorOverride(int colorIndex)
	{
		if (this.colorIdx != colorIndex)
		{
			this.colorIdx = colorIndex;
			this.needAssetRefresh = true;
			this.AssetNeedReload = true;
		}
	}

	public int GetColorIndex()
	{
		return this.colorIdx;
	}

	private string GetMaterial()
	{
		if (this.Empty)
		{
			return string.Empty;
		}
		global::BodyPart.LogBldr.Length = 0;
		this.materialName = string.Empty;
		this.Color = string.Empty;
		if (this.colorIdx >= 256)
		{
			global::System.Collections.Generic.List<global::BodyPartColorData> list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::BodyPartColorData>(global::BodyPart.BODY_PART_ITEM_PRESET_COLOR_IDS, this.bodyPartsValues);
			if (list.Count > 0)
			{
				this.Color = list[0].Color;
			}
		}
		else
		{
			global::System.Collections.Generic.List<string> availableMaterials = this.GetAvailableMaterials(true);
			if (this.colorIdx >= availableMaterials.Count)
			{
				this.colorIdx = 0;
			}
			if (availableMaterials.Count > 0)
			{
				this.Color = availableMaterials[this.colorIdx];
				this.materialName = this.FindMaterial(this.Color);
			}
		}
		string text = this.assetName;
		if (this.InjuryId == global::InjuryId.NONE && this.MutationId == global::MutationId.NONE && this.data.Id == global::BodyPartId.LLEGR)
		{
			text = this.assetName.Replace("lleg", "leg");
		}
		if (string.IsNullOrEmpty(this.materialName) && this.data.Skinnable && !string.IsNullOrEmpty(this.skinColor) && !string.IsNullOrEmpty(this.Color))
		{
			string text2 = global::PandoraUtils.StringBuilder.AppendFormat("{0}_{1}_{2}", text, this.skinColor, this.Color).ToString();
			this.materialName = this.FindMaterial(text2);
			global::BodyPart.LogBldr.Append(text2);
		}
		if (string.IsNullOrEmpty(this.materialName) && this.data.Skinnable && !string.IsNullOrEmpty(this.altSkinColor) && !string.IsNullOrEmpty(this.Color))
		{
			string text3 = global::PandoraUtils.StringBuilder.AppendFormat("{0}_{1}_{2}", text, this.altSkinColor, this.Color).ToString();
			this.materialName = this.FindMaterial(text3);
			global::BodyPart.LogBldr.Append(" , ");
			global::BodyPart.LogBldr.Append(text3);
		}
		if (string.IsNullOrEmpty(this.materialName) && this.data.Skinnable && !string.IsNullOrEmpty(this.skinColor))
		{
			string text4 = global::PandoraUtils.StringBuilder.AppendFormat("{0}_{1}", text, this.skinColor).ToString();
			this.materialName = this.FindMaterial(text4);
			global::BodyPart.LogBldr.Append(" , ");
			global::BodyPart.LogBldr.Append(text4);
		}
		if (string.IsNullOrEmpty(this.materialName) && this.data.Skinnable && !string.IsNullOrEmpty(this.altSkinColor))
		{
			string text5 = global::PandoraUtils.StringBuilder.AppendFormat("{0}_{1}", text, this.altSkinColor).ToString();
			this.materialName = this.FindMaterial(text5);
			global::BodyPart.LogBldr.Append(" , ");
			global::BodyPart.LogBldr.Append(text5);
		}
		if (string.IsNullOrEmpty(this.materialName) && !string.IsNullOrEmpty(this.Color))
		{
			string text6 = global::PandoraUtils.StringBuilder.AppendFormat("{0}_{1}", text, this.Color).ToString();
			this.materialName = this.FindMaterial(text6);
			global::BodyPart.LogBldr.Append(" , ");
			global::BodyPart.LogBldr.Append(text6);
		}
		if (string.IsNullOrEmpty(this.materialName))
		{
			string text7 = text;
			this.materialName = this.FindMaterial(text7);
			global::BodyPart.LogBldr.Append(" , ");
			global::BodyPart.LogBldr.Append(text7);
		}
		if (!string.IsNullOrEmpty(this.materialName))
		{
			this.needAssetRefresh = false;
			if (this.InjuryId == global::InjuryId.NONE && this.MutationId == global::MutationId.NONE && this.data.Id == global::BodyPartId.LLEGR)
			{
				this.materialName = this.materialName.Replace("leg", "lleg");
			}
			return this.materialName;
		}
		if (this.colorIdx >= 256)
		{
			this.colorIdx = 0;
			return this.GetMaterial();
		}
		this.Empty = true;
		return string.Empty;
	}

	public global::System.Collections.Generic.List<string> GetAvailableMaterials(bool includeInjuries)
	{
		if (string.IsNullOrEmpty(this.assetName))
		{
			return new global::System.Collections.Generic.List<string>();
		}
		string name = this.assetName;
		if (this.InjuryId == global::InjuryId.NONE && this.MutationId == global::MutationId.NONE && this.data.Id == global::BodyPartId.LLEGR)
		{
			name = this.assetName.Replace("lleg", "leg");
		}
		global::System.Collections.Generic.List<string> list = new global::System.Collections.Generic.List<string>();
		this.InitDataLike(ref list, name, this.materials, false, false);
		if (!includeInjuries)
		{
			for (int i = list.Count - 1; i >= 0; i--)
			{
				if (list[i].Contains("injury"))
				{
					list.RemoveAt(i);
				}
			}
		}
		if (!string.IsNullOrEmpty(this.skinColor) && this.data.Skinnable)
		{
			global::System.Collections.Generic.List<string> list2 = new global::System.Collections.Generic.List<string>();
			for (int j = list.Count - 1; j >= 0; j--)
			{
				if (!list[j].Contains("skin") || (list[j].Contains(this.skinColor) && this.skinColor.Contains("pimple") == list[j].Contains("pimple")))
				{
					list2.Add(list[j]);
				}
			}
			if (list2.Count == 0 && !string.IsNullOrEmpty(this.altSkinColor))
			{
				for (int k = list.Count - 1; k >= 0; k--)
				{
					if (list[k].Contains(this.altSkinColor) && this.altSkinColor.Contains("pimple") == list[k].Contains("pimple"))
					{
						list2.Add(list[k]);
					}
				}
			}
			list = list2;
		}
		return list;
	}

	private string FindMaterial(string material)
	{
		for (int i = 0; i < this.materials.Count; i++)
		{
			if (string.Compare(this.materials[i], material, global::System.StringComparison.OrdinalIgnoreCase) == 0)
			{
				return material;
			}
		}
		return null;
	}

	public const int PRESET_OFFSET = 8;

	private const string ASSET_BUNDLE_NAME = "{0}_{1}";

	private const string INJURY_NAME = "{0}_{1}_{2}_injury_{3}";

	private const string MUTATION_NAME = "{0}_{1}_{2}_{3}_{4}";

	private const string BODY_PART_NAME = "{0}_{1}_{2}";

	private const string BODY_PART_ITEM_NAME = "{0}_{1}_{2}_{3}";

	private const string MAT_SKIN_COLOR = "{0}_{1}_{2}";

	private const string MAT_COLOR = "{0}_{1}";

	private const string SEPARATOR = " , ";

	private static global::System.Text.StringBuilder LogBldr = new global::System.Text.StringBuilder();

	private static string[] BODY_PART_ITEM_PRESET_COLOR_IDS = new string[]
	{
		"fk_unit_id",
		"fk_color_preset_id",
		"fk_body_part_id",
		"fk_item_type_id"
	};

	private global::System.Collections.Generic.List<string> models;

	private global::System.Collections.Generic.List<string> materials;

	public global::System.Collections.Generic.List<global::UnityEngine.GameObject> relatedGO;

	private global::BodyPartData data;

	private global::UnitId relatedUnitId;

	private global::Item relatedItem;

	private int colorIdx;

	private string warband = string.Empty;

	private string unit = string.Empty;

	private string altUnit = string.Empty;

	private int variation;

	private string skinColor = string.Empty;

	private string altSkinColor = string.Empty;

	private bool needAssetRefresh;

	private string assetName;

	private string materialName;

	private string[] bodyPartsValues;

	private string[] bodyPartsValuesNoPreset;

	private bool locked;
}
