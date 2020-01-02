using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Item
{
	public Item(global::ItemSave iSave)
	{
		this.itemSave = iSave;
		this.Init((global::ItemId)this.itemSave.id, (global::ItemQualityId)this.itemSave.qualityId);
	}

	public Item(global::ItemId id, global::ItemQualityId qualityId = global::ItemQualityId.NORMAL)
	{
		this.itemSave = new global::ItemSave(id, qualityId, global::RuneMarkId.NONE, global::RuneMarkQualityId.NONE, global::AllegianceId.NONE, 1);
		this.Init(id, qualityId);
	}

	public global::AnimStyleData StyleData { get; private set; }

	public global::ItemTypeData TypeData { get; private set; }

	public global::ItemSave Save
	{
		get
		{
			return this.itemSave;
		}
	}

	public global::System.Collections.Generic.List<global::Enchantment> Enchantments { get; private set; }

	public global::System.Collections.Generic.List<global::ItemAttributeData> AttributeModifiers { get; private set; }

	public bool IsPaired
	{
		get
		{
			return this.itemData.Paired;
		}
	}

	public bool IsTwoHanded
	{
		get
		{
			return this.TypeData.IsTwoHanded;
		}
	}

	public bool IsLockSlot
	{
		get
		{
			return this.itemData.LockSlot;
		}
	}

	public bool IsStackable
	{
		get
		{
			return this.itemData.Stackable;
		}
	}

	public bool IsConsumable
	{
		get
		{
			return this.ConsumableData != null;
		}
	}

	public bool IsRecipe
	{
		get
		{
			return this.RecipeData != null;
		}
	}

	public bool IsUndroppable
	{
		get
		{
			return this.itemData.Undroppable;
		}
	}

	public global::TargetingId TargetingId
	{
		get
		{
			return this.itemData.TargetingId;
		}
	}

	public bool TargetAlly
	{
		get
		{
			return this.itemData.TargetAlly;
		}
	}

	public global::BoneId BoneId
	{
		get
		{
			return this.itemData.BoneId;
		}
	}

	public int Radius
	{
		get
		{
			return this.itemData.Radius;
		}
	}

	public global::ItemQualityData QualityData { get; private set; }

	public global::ItemQualityJoinItemTypeData QualityTypeData { get; private set; }

	public global::ItemSpeedData SpeedData { get; private set; }

	public global::ItemMutationData MutationData { get; private set; }

	public global::RuneMark RuneMark { get; private set; }

	public global::System.Collections.Generic.List<global::RuneMark> RecipeRuneMarks { get; private set; }

	public global::ItemConsumableData ConsumableData { get; private set; }

	public global::RuneMarkRecipeData RecipeData { get; private set; }

	public global::ItemId Id
	{
		get
		{
			return this.itemData.Id;
		}
	}

	public string Name
	{
		get
		{
			return this.itemData.Name;
		}
	}

	public string Asset
	{
		get
		{
			return this.itemData.Asset;
		}
	}

	public int DamageMin
	{
		get
		{
			return ((this.MutationData == null) ? this.itemData.DamageMin : this.MutationData.DamageMin) + ((this.QualityTypeData == null) ? 0 : this.QualityTypeData.DamageMinModifier);
		}
	}

	public int DamageMax
	{
		get
		{
			return ((this.MutationData == null) ? this.itemData.DamageMax : this.MutationData.DamageMax) + ((this.QualityTypeData == null) ? 0 : this.QualityTypeData.DamageMaxModifier);
		}
	}

	public int RangeMin
	{
		get
		{
			return this.rangeData.MinRange;
		}
	}

	public int RangeMax
	{
		get
		{
			return this.rangeData.Range + ((this.QualityTypeData == null) ? 0 : this.QualityTypeData.RangeModifier);
		}
	}

	public int ArmorAbsorption
	{
		get
		{
			return this.itemData.ArmorAbsorption + ((this.QualityTypeData == null) ? 0 : this.QualityTypeData.ArmorAbsorptionModifier);
		}
	}

	public int Rating
	{
		get
		{
			return this.itemData.Rating + ((this.QualityTypeData == null) ? 0 : this.QualityTypeData.RatingModifier);
		}
	}

	public global::ProjectileId ProjectileId
	{
		get
		{
			return this.itemData.ProjectileId;
		}
	}

	public global::ProjectileData ProjectileData { get; private set; }

	public int Shots
	{
		get
		{
			return this.itemData.Shots;
		}
	}

	public string Sound
	{
		get
		{
			return this.itemData.Sound;
		}
	}

	public string SoundCat
	{
		get
		{
			return this.itemData.SoundCat;
		}
	}

	public global::System.Collections.Generic.List<global::ItemJoinUnitSlotData> UnitSlots { get; private set; }

	public int Amount
	{
		get
		{
			return this.itemSave.amount;
		}
	}

	public bool Backup
	{
		get
		{
			return this.itemData.Backup;
		}
	}

	public bool IsWyrdStone
	{
		get
		{
			return this.Id == global::ItemId.WYRDSTONE_SHARD || this.Id == global::ItemId.WYRDSTONE_FRAGMENT || this.Id == global::ItemId.WYRDSTONE_CLUSTER;
		}
	}

	public bool IsIdol
	{
		get
		{
			return this.itemData.IsIdol || this.Id == global::ItemId.IDOL_MERCENARIES || this.Id == global::ItemId.IDOL_POSSESSED || this.Id == global::ItemId.IDOL_SISTERS || this.Id == global::ItemId.IDOL_SKAVEN;
		}
	}

	public int PriceBuy
	{
		get
		{
			return this.itemData.PriceBuy * this.QualityData.PriceBuyModifier + ((this.RuneMark == null) ? 0 : this.RuneMark.Cost);
		}
	}

	public int PriceSold
	{
		get
		{
			return this.itemData.PriceSold * this.QualityData.PriceSoldModifier + ((this.RuneMark == null) ? 0 : (this.RuneMark.Cost / 3));
		}
	}

	public string LocalizedName
	{
		get
		{
			if (this.Id == global::ItemId.GOLD)
			{
				return global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(this.LabelName, new string[]
				{
					this.itemSave.amount.ToConstantString()
				});
			}
			if (this.IsTrophy && this.owner != null)
			{
				return global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(this.LabelName, new string[]
				{
					this.owner.Name
				});
			}
			if (this.IsConsumable)
			{
				return global::SkillHelper.GetLocalizedName(this.ConsumableData.SkillId);
			}
			return global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(this.LabelName) + " " + ((this.RuneMark == null) ? string.Empty : this.RuneMark.SuffixLocName);
		}
	}

	public string LabelName
	{
		get
		{
			return this.labelName;
		}
	}

	public bool IsTrophy { get; private set; }

	private void Init(global::ItemId id, global::ItemQualityId qualityId)
	{
		this.AttributeModifiers = new global::System.Collections.Generic.List<global::ItemAttributeData>();
		this.Enchantments = new global::System.Collections.Generic.List<global::Enchantment>();
		qualityId = ((qualityId != global::ItemQualityId.NONE) ? qualityId : global::ItemQualityId.NORMAL);
		this.itemData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::ItemData>((int)id);
		this.labelName = "item_name_" + this.Name;
		this.TypeData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::ItemTypeData>((int)this.itemData.ItemTypeId);
		if (this.TypeData.Id == global::ItemTypeId.RECIPE_ENCHANTMENT_NORMAL || this.TypeData.Id == global::ItemTypeId.RECIPE_ENCHANTMENT_MASTERY)
		{
			qualityId = global::ItemQualityId.NORMAL;
		}
		this.QualityData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::ItemQualityData>((int)qualityId);
		if (this.QualityData.Id != global::ItemQualityId.NORMAL)
		{
			global::System.Collections.Generic.List<global::ItemQualityJoinItemTypeData> list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::ItemQualityJoinItemTypeData>(new string[]
			{
				"fk_item_quality_id",
				"fk_item_type_id"
			}, new string[]
			{
				((int)qualityId).ToConstantString(),
				((int)this.itemData.ItemTypeId).ToConstantString()
			});
			this.QualityTypeData = list[0];
		}
		this.StyleData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::AnimStyleData>((int)this.itemData.AnimStyleId);
		this.rangeData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::ItemRangeData>((int)this.itemData.ItemRangeId);
		this.SpeedData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::ItemSpeedData>((int)this.itemData.ItemSpeedId);
		this.UnitSlots = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::ItemJoinUnitSlotData>("fk_item_id", this.itemData.Id.ToIntString<global::ItemId>());
		global::System.Collections.Generic.List<global::ItemConsumableData> list2 = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::ItemConsumableData>(new string[]
		{
			"fk_item_id",
			"fk_item_quality_id"
		}, new string[]
		{
			((int)id).ToConstantString(),
			((int)qualityId).ToConstantString()
		});
		if (list2 != null && list2.Count > 0)
		{
			this.ConsumableData = list2[0];
		}
		if (this.itemData.ProjectileId != global::ProjectileId.NONE)
		{
			this.ProjectileData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::ProjectileData>((int)this.ProjectileId);
		}
		global::System.Collections.Generic.List<global::RuneMarkRecipeData> list3 = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::RuneMarkRecipeData>("fk_item_id", ((int)id).ToConstantString());
		if (list3 != null && list3.Count > 0)
		{
			this.RecipeData = list3[0];
			this.RecipeRuneMarks = new global::System.Collections.Generic.List<global::RuneMark>();
			for (int i = 0; i < list3.Count; i++)
			{
				this.RecipeRuneMarks.Add(new global::RuneMark(list3[i].RuneMarkId, list3[i].RuneMarkQualityId, global::AllegianceId.ORDER, global::ItemTypeId.NONE, null));
			}
		}
		this.SetModifiers(global::MutationId.NONE);
		if (this.itemSave.runeMarkId != 0)
		{
			this.RuneMark = new global::RuneMark((global::RuneMarkId)this.itemSave.runeMarkId, (global::RuneMarkQualityId)this.itemSave.runeMarkQualityId, (global::AllegianceId)this.itemSave.allegianceId, this.TypeData.Id, null);
		}
		if (this.Id == global::ItemId.BOUNTY_HUMAN_MERCS || this.Id == global::ItemId.BOUNTY_HUMAN_SISTERS || this.Id == global::ItemId.BOUNTY_OGRE_MERC || this.Id == global::ItemId.BOUNTY_POSSESSED || this.Id == global::ItemId.BOUNTY_SKAVEN || this.Id == global::ItemId.BOUNTY_HUMAN_WITCH_HUNTERS || this.Id == global::ItemId.BOUNTY_UNDEAD)
		{
			this.IsTrophy = true;
		}
	}

	public void SetModifiers(global::MutationId mutationId)
	{
		this.Enchantments.Clear();
		this.MutationData = null;
		global::System.Collections.Generic.List<global::ItemEnchantmentData> list = new global::System.Collections.Generic.List<global::ItemEnchantmentData>();
		if (this.itemData.MutationBased)
		{
			global::System.Collections.Generic.List<global::ItemMutationData> list2 = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::ItemMutationData>(new string[]
			{
				"fk_item_id",
				"fk_mutation_id"
			}, new string[]
			{
				((int)this.Id).ToConstantString(),
				((int)mutationId).ToConstantString()
			});
			if (list2.Count > 0)
			{
				this.MutationData = list2[0];
				this.SpeedData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::ItemSpeedData>((int)this.MutationData.ItemSpeedId);
			}
			this.AttributeModifiers = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::ItemAttributeData>(new string[]
			{
				"fk_item_id",
				"fk_item_quality_id",
				"fk_mutation_id"
			}, new string[]
			{
				((int)this.Id).ToConstantString(),
				((int)this.QualityData.Id).ToConstantString(),
				((int)mutationId).ToConstantString()
			});
			list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::ItemEnchantmentData>(new string[]
			{
				"fk_item_id",
				"fk_item_quality_id",
				"fk_mutation_id"
			}, new string[]
			{
				((int)this.Id).ToConstantString(),
				((int)this.QualityData.Id).ToConstantString(),
				((int)mutationId).ToConstantString()
			});
		}
		else
		{
			this.AttributeModifiers = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::ItemAttributeData>(new string[]
			{
				"fk_item_id",
				"fk_item_quality_id"
			}, new string[]
			{
				((int)this.Id).ToConstantString(),
				((int)this.QualityData.Id).ToConstantString()
			});
			list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::ItemEnchantmentData>(new string[]
			{
				"fk_item_id",
				"fk_item_quality_id"
			}, new string[]
			{
				((int)this.Id).ToConstantString(),
				((int)this.QualityData.Id).ToConstantString()
			});
		}
		for (int i = 0; i < list.Count; i++)
		{
			this.Enchantments.Add(new global::Enchantment(list[i].EnchantmentId, null, null, false, true, global::AllegianceId.NONE, true));
		}
	}

	public bool AddRuneMark(global::RuneMarkId runeMarkId, global::RuneMarkQualityId runeQualityId, global::AllegianceId allegianceId)
	{
		if (runeQualityId <= this.QualityData.RuneMarkQualityIdMax && (this.RuneMark == null || this.RuneMark.Data.Id == global::RuneMarkId.NONE))
		{
			this.itemSave.runeMarkId = (int)runeMarkId;
			this.itemSave.runeMarkQualityId = (int)runeQualityId;
			this.itemSave.allegianceId = (int)allegianceId;
			this.RuneMark = new global::RuneMark(runeMarkId, runeQualityId, allegianceId, this.TypeData.Id, null);
			return true;
		}
		return false;
	}

	public bool CanAddRuneMark()
	{
		return this.QualityData.Id > global::ItemQualityId.NORMAL && (this.RuneMark == null || this.RuneMark.Data.Id == global::RuneMarkId.NONE);
	}

	public bool HasEnchantment(global::EnchantmentId enchantmentId)
	{
		bool result = false;
		for (int i = 0; i < this.Enchantments.Count; i++)
		{
			if (this.Enchantments[i].Id == enchantmentId)
			{
				result = true;
				break;
			}
		}
		return result;
	}

	public bool UpdateEnchantmentsDuration(global::Unit currentUnit)
	{
		bool result = false;
		for (int i = this.Enchantments.Count - 1; i >= 0; i--)
		{
			if (this.Enchantments[i].UpdateDuration(currentUnit))
			{
				this.Enchantments.RemoveAt(i);
				result = true;
			}
		}
		return result;
	}

	public bool UpdateEnchantments(global::UnitStateId unitStateId)
	{
		bool result = false;
		for (int i = this.Enchantments.Count - 1; i >= 0; i--)
		{
			if (this.Enchantments[i].UpdateStatus(unitStateId))
			{
				this.Enchantments.RemoveAt(i);
				result = true;
			}
		}
		return result;
	}

	public int GetRating()
	{
		int num = 0;
		if (this.itemData != null)
		{
			num += this.Rating;
			if (this.RuneMark != null)
			{
				num += this.RuneMark.QualityItemTypeData.Rating;
			}
		}
		return num;
	}

	public global::UnityEngine.Sprite GetIcon()
	{
		return global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadResource<global::UnityEngine.Sprite>("item/" + this.itemData.Asset.ToLowerInvariant(), true);
	}

	public global::UnityEngine.Sprite GetRuneIcon()
	{
		if (this.RuneMark != null)
		{
			return this.RuneMark.GetIcon();
		}
		if (!this.IsConsumable && this.QualityData.RuneMarkQualityIdMax != global::RuneMarkQualityId.NONE)
		{
			return global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadResource<global::UnityEngine.Sprite>("runemark/" + this.QualityData.RuneMarkQualityIdMax.ToLowerString() + "_empty", true);
		}
		return null;
	}

	public global::ItemAssetData GetAssetData(global::RaceId raceId, global::WarbandId warbandId, global::UnitId unitId)
	{
		global::ItemAssetData result = null;
		global::System.Collections.Generic.List<global::ItemAssetData> list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::ItemAssetData>(new string[]
		{
			"fk_item_id",
			"fk_race_id",
			"fk_warband_id"
		}, new string[]
		{
			((int)this.Id).ToConstantString(),
			((int)raceId).ToConstantString(),
			((int)warbandId).ToConstantString()
		});
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i].UnitId == unitId)
			{
				return list[i];
			}
			if (list[i].UnitId == global::UnitId.NONE)
			{
				result = list[i];
			}
		}
		return result;
	}

	public string GetRuneNameDesc()
	{
		if (this.RuneMark != null)
		{
			return this.RuneMark.FullLocName + " :\n" + this.RuneMark.LocShort;
		}
		if (this.QualityData.RuneMarkQualityIdMax != global::RuneMarkQualityId.NONE)
		{
			global::AllegianceId allegianceId = (global::AllegianceId)this.Save.allegianceId;
			if (this.owner == null && global::PandoraSingleton<global::HideoutManager>.Exists())
			{
				global::UnitMenuController currentUnit = global::PandoraSingleton<global::HideoutManager>.Instance.currentUnit;
				if (currentUnit != null)
				{
					allegianceId = currentUnit.unit.AllegianceId;
				}
			}
			else if (this.owner == null && global::PandoraSingleton<global::MissionManager>.Exists())
			{
				global::UnitController currentUnit2 = global::PandoraSingleton<global::MissionManager>.Instance.GetCurrentUnit();
				if (currentUnit2 != null)
				{
					allegianceId = currentUnit2.unit.AllegianceId;
				}
			}
			return global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById((allegianceId != global::AllegianceId.ORDER) ? "item_enchant_type_mark_empty" : "item_enchant_type_rune_empty");
		}
		return string.Empty;
	}

	private bool IsDualWield()
	{
		for (int i = 0; i < this.Enchantments.Count; i++)
		{
			if (this.Enchantments[i].Id == global::EnchantmentId.ITEM_DUAL_WIELD)
			{
				return true;
			}
		}
		return false;
	}

	public string GetLocalizedDescription(global::UnitSlotId? equippedSlot)
	{
		if (this.IsConsumable)
		{
			return global::SkillHelper.GetLocalizedDescription(this.ConsumableData.SkillId);
		}
		if (this.IsRecipe)
		{
			return global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("item_desc_recipe_" + this.RecipeRuneMarks[0].Name + "_normal") + "\n" + this.RecipeRuneMarks[0].LocDesc;
		}
		string text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("item_desc_" + this.Name + "_" + this.QualityData.Name);
		if (this.IsDualWield())
		{
			text = text + "\n" + global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("item_dualwield_desc");
		}
		return text;
	}

	public static string GetLocalizedName(global::ItemId id)
	{
		return global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("item_name_" + id.ToLowerString());
	}

	public static string GetLocalizedName(global::ItemId id, global::ItemQualityId qualityId)
	{
		global::System.Collections.Generic.List<global::ItemConsumableData> list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::ItemConsumableData>(new string[]
		{
			"fk_item_id",
			"fk_item_quality_id"
		}, new string[]
		{
			((int)id).ToConstantString(),
			((int)qualityId).ToConstantString()
		});
		if (list != null && list.Count > 0)
		{
			return global::SkillHelper.GetLocalizedName(list[0].SkillId);
		}
		return global::Item.GetLocalizedName(id);
	}

	public static global::Item GetItemReward(global::System.Collections.Generic.List<global::SearchRewardItemData> rewardItems, global::Tyche tyche)
	{
		global::SearchRewardItemData randomRatio = global::SearchRewardItemData.GetRandomRatio(rewardItems, tyche, null);
		global::SearchRewardData searchRewardData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::SearchRewardData>((int)randomRatio.SearchRewardId);
		global::System.Collections.Generic.List<global::AllegianceId> list = new global::System.Collections.Generic.List<global::AllegianceId>();
		for (int i = 0; i < global::PandoraSingleton<global::MissionManager>.Instance.WarbandCtrlrs.Count; i++)
		{
			list.Add(global::PandoraSingleton<global::MissionManager>.Instance.WarbandCtrlrs[i].WarData.AllegianceId);
		}
		int index = tyche.Rand(0, list.Count);
		return global::Item.GetRandomLootableItem(tyche, randomRatio.ItemCategoryId, searchRewardData.ItemQualityId, searchRewardData.RuneMarkQualityId, list[index], null, null, null);
	}

	public static global::Item GetRandomLootableItem(global::Tyche tyche, global::ItemCategoryId categoryId, global::ItemQualityId qualityId, global::RuneMarkQualityId runeMarkQualityId, global::AllegianceId allegianceId, global::System.Collections.Generic.List<global::ItemId> allowedWeaponsArmors = null, global::System.Collections.Generic.List<global::ItemId> additionalLootableItemsId = null, global::System.Collections.Generic.List<global::ItemId> exludeItemIds = null)
	{
		global::System.Collections.Generic.List<global::ItemTypeData> list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::ItemTypeData>("fk_item_category_id", ((int)categoryId).ToConstantString());
		global::System.Collections.Generic.List<global::ItemTypeId> types = list.ConvertAll<global::ItemTypeId>((global::ItemTypeData x) => x.Id);
		return global::Item.GetRandomLootableItem(tyche, types, qualityId, runeMarkQualityId, allegianceId, allowedWeaponsArmors, additionalLootableItemsId, exludeItemIds);
	}

	public static global::Item GetRandomLootableItem(global::Tyche tyche, global::System.Collections.Generic.List<global::ItemTypeId> types, global::ItemQualityId qualityId, global::RuneMarkQualityId runeMarkQualityId, global::AllegianceId allegianceId, global::System.Collections.Generic.List<global::ItemId> allowedWeaponsArmors = null, global::System.Collections.Generic.List<global::ItemId> additionalLootableItemsId = null, global::System.Collections.Generic.List<global::ItemId> exludeItemIds = null)
	{
		if (types.Count == 1)
		{
			if (types[0] == global::ItemTypeId.RECIPE_ENCHANTMENT_NORMAL)
			{
				return new global::Item(global::ItemId.RECIPE_RANDOM_NORMAL, global::ItemQualityId.NORMAL);
			}
			if (types[0] == global::ItemTypeId.RECIPE_ENCHANTMENT_MASTERY)
			{
				return new global::Item(global::ItemId.RECIPE_RANDOM_MASTERY, global::ItemQualityId.NORMAL);
			}
		}
		global::System.Collections.Generic.List<global::ItemData> list = new global::System.Collections.Generic.List<global::ItemData>();
		for (int i = 0; i < types.Count; i++)
		{
			list.AddRange(global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::ItemData>(new string[]
			{
				"fk_item_type_id",
				"lootable"
			}, new string[]
			{
				((int)types[i]).ToConstantString(),
				"1"
			}));
		}
		if (additionalLootableItemsId != null)
		{
			for (int j = 0; j < additionalLootableItemsId.Count; j++)
			{
				global::ItemId itemId = additionalLootableItemsId[j];
				if (!list.Exists((global::ItemData x) => x.Id == itemId))
				{
					list.Add(global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::ItemData>((int)additionalLootableItemsId[j]));
				}
			}
		}
		if (allowedWeaponsArmors != null)
		{
			for (int k = list.Count - 1; k >= 0; k--)
			{
				global::ItemTypeData itemTypeData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::ItemTypeData>((int)list[k].ItemTypeId);
				global::ItemCategoryId itemCategoryId = itemTypeData.ItemCategoryId;
				if (itemCategoryId == global::ItemCategoryId.WEAPONS || itemCategoryId == global::ItemCategoryId.ARMOR)
				{
					if (allowedWeaponsArmors.IndexOf(list[k].Id, global::ItemIdComparer.Instance) == -1)
					{
						list.RemoveAt(k);
					}
				}
			}
		}
		int index = tyche.Rand(0, list.Count);
		if (list[index].ItemTypeId == global::ItemTypeId.RECIPE_ENCHANTMENT_NORMAL || list[index].ItemTypeId == global::ItemTypeId.RECIPE_ENCHANTMENT_MASTERY)
		{
			qualityId = global::ItemQualityId.NORMAL;
		}
		else
		{
			qualityId = global::Item.GetClosestQuality(list[index].Id, qualityId);
		}
		global::Item item = new global::Item(new global::ItemSave(list[index].Id, qualityId, global::RuneMarkId.NONE, global::RuneMarkQualityId.NONE, global::AllegianceId.NONE, 1));
		if (runeMarkQualityId > global::RuneMarkQualityId.NONE && item.CanAddRuneMark())
		{
			global::RuneMarkId randomRuneMark = global::Item.GetRandomRuneMark(tyche, item, allegianceId);
			if (randomRuneMark != global::RuneMarkId.NONE)
			{
				item.AddRuneMark(randomRuneMark, runeMarkQualityId, allegianceId);
			}
		}
		return item;
	}

	public static global::RuneMarkId GetRandomRuneMark(global::Tyche tyche, global::Item item, global::AllegianceId allegianceId)
	{
		global::System.Collections.Generic.List<global::RuneMarkJoinItemTypeData> list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::RuneMarkJoinItemTypeData>("fk_item_type_id", ((int)item.TypeData.Id).ToConstantString());
		global::System.Collections.Generic.List<global::RuneMarkId> list2 = new global::System.Collections.Generic.List<global::RuneMarkId>();
		for (int i = 0; i < list.Count; i++)
		{
			global::RuneMarkData runeMarkData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::RuneMarkData>((int)list[i].RuneMarkId);
			if ((allegianceId == global::AllegianceId.ORDER && runeMarkData.Rune) || (allegianceId == global::AllegianceId.DESTRUCTION && runeMarkData.Mark))
			{
				list2.Add(runeMarkData.Id);
			}
		}
		if (list2.Count > 0)
		{
			int index = tyche.Rand(0, list2.Count);
			return list2[index];
		}
		return global::RuneMarkId.NONE;
	}

	public static global::ItemQualityId GetClosestQuality(global::ItemId itemId, global::ItemQualityId desiredQualityId)
	{
		global::ItemData itemData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::ItemData>((int)itemId);
		return global::Item.GetClosestQuality(itemData.ItemTypeId, desiredQualityId);
	}

	public static global::ItemQualityId GetClosestQuality(global::ItemTypeId itemTypeId, global::ItemQualityId desiredQualityId)
	{
		global::System.Collections.Generic.List<global::ItemQualityId> list = new global::System.Collections.Generic.List<global::ItemQualityId>();
		list.Add(global::ItemQualityId.NORMAL);
		global::System.Collections.Generic.List<global::ItemQualityJoinItemTypeData> list2 = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::ItemQualityJoinItemTypeData>("fk_item_type_id", ((int)itemTypeId).ToConstantString());
		for (int i = 0; i < list2.Count; i++)
		{
			list.Add(list2[i].ItemQualityId);
		}
		for (int j = 0; j < list.Count; j++)
		{
			if (list[j] == desiredQualityId)
			{
				return desiredQualityId;
			}
		}
		global::ItemQualityId itemQualityId = global::ItemQualityId.NONE;
		for (int k = 0; k < list.Count; k++)
		{
			if (itemQualityId == global::ItemQualityId.NONE || global::UnityEngine.Mathf.Abs(list[k] - desiredQualityId) < global::UnityEngine.Mathf.Abs(itemQualityId - desiredQualityId))
			{
				itemQualityId = list[k];
			}
		}
		return itemQualityId;
	}

	public static void SortEmptyItems(global::System.Collections.Generic.List<global::Item> items, int startIdx)
	{
		for (int i = startIdx; i < items.Count - 1; i++)
		{
			if (items[i].Id == global::ItemId.NONE && items[i + 1].Id != global::ItemId.NONE)
			{
				global::Item value = items[i];
				items[i] = items[i + 1];
				items[i + 1] = value;
				i = startIdx - 1;
			}
		}
	}

	public bool IsSame(global::Item item)
	{
		return this.Id == item.Id && this.QualityData.Id == item.QualityData.Id && ((this.RuneMark == null && item.RuneMark == null) || (this.RuneMark != null && item.RuneMark != null && this.RuneMark.Data.Id == item.RuneMark.Data.Id && this.RuneMark.QualityData.Id == item.RuneMark.QualityData.Id));
	}

	public bool IsSold()
	{
		global::System.Collections.Generic.List<global::ItemSave> items = global::PandoraSingleton<global::HideoutManager>.Instance.Market.GetItems();
		for (int i = 0; i < items.Count; i++)
		{
			if (items[i].id == this.itemSave.id && items[i].qualityId == this.itemSave.qualityId && items[i].runeMarkId == this.itemSave.runeMarkId && items[i].runeMarkQualityId == this.itemSave.runeMarkQualityId)
			{
				return true;
			}
		}
		return false;
	}

	private static global::System.Text.StringBuilder descBuilder = new global::System.Text.StringBuilder();

	public bool linkedToObjective;

	public global::Unit owner;

	private global::ItemData itemData;

	private global::ItemSave itemSave;

	private global::ItemRangeData rangeData;

	private string labelName;
}
