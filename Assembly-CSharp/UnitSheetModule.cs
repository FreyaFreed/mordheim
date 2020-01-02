using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UnitSheetModule : global::UIModule
{
	public override void Init()
	{
		base.Init();
		for (int i = 0; i < this.advancements.Count; i++)
		{
			this.advancements[i].gameObject.SetActive(false);
		}
	}

	public void Refresh(global::UnityEngine.UI.Selectable right, global::Unit unit, global::System.Action<global::AttributeId> onAttributeSelected = null, global::System.Action<string, string> showDescription = null, global::System.Action<global::AttributeId> onAttributeUnselected = null)
	{
		this.unit = unit;
		global::UnityEngine.UI.Navigation navigation;
		for (int i = 0; i < this.stats.Count; i++)
		{
			if (this.stats[i].canGoRight)
			{
				navigation = this.stats[i].statSelector.toggle.navigation;
				navigation.selectOnRight = right;
				this.stats[i].statSelector.toggle.navigation = navigation;
			}
			this.stats[i].Refresh(unit, true, onAttributeSelected, null, onAttributeUnselected);
		}
		navigation = this.statusToggle.toggle.navigation;
		navigation.selectOnRight = right;
		this.statusToggle.toggle.navigation = navigation;
		if (showDescription != null)
		{
			this.statusToggle.onSelect.AddListener(delegate()
			{
				string str = unit.GetActiveStatus().ToLowerString();
				showDescription(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("unit_status_name_" + str), global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("unit_status_desc_simple_" + str));
			});
			this.ratingToggle.onSelect.AddListener(delegate()
			{
				string stringById = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("unit_type_" + unit.GetUnitTypeId().ToLowerString());
				int unitTypeRating = unit.GetUnitTypeRating();
				int rankRating = unit.GetRankRating();
				int equipmentRating = unit.GetEquipmentRating();
				int statsRating = unit.GetStatsRating();
				int skillsRating = unit.GetSkillsRating();
				int num = unit.GetInjuriesRating() + unit.GetMutationsRating();
				int num2 = unitTypeRating + rankRating + equipmentRating + statsRating + skillsRating + num;
				showDescription(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("unit_rating_title"), global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("unit_rating_desc", new string[]
				{
					stringById,
					unitTypeRating.ToString(),
					unit.Rank.ToString(),
					rankRating.ToString(),
					equipmentRating.ToString(),
					statsRating.ToString(),
					skillsRating.ToString(),
					num.ToString(),
					num2.ToString()
				}));
			});
		}
		this.RefreshAttributes(unit);
	}

	public void RefreshAttributes(global::Unit unit)
	{
		global::DG.Tweening.DOTween.Kill(this.xp, false);
		this.unit = unit;
		this.rank.text = unit.Rank.ToString();
		this.unitName.text = unit.Name;
		if (unit.Rank >= global::Constant.GetInt(global::ConstantId.MAX_UNIT_RANK))
		{
			this.xp.normalizedValue = 1f;
			for (int i = 0; i < this.advancements.Count; i++)
			{
				this.advancements[i].gameObject.SetActive(false);
			}
			this.xpValue.enabled = false;
		}
		else
		{
			global::UnitRankData unitRankData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::UnitRankData>((int)unit.UnitSave.rankId);
			int advancementsCountForRank = this.GetAdvancementsCountForRank(unit.Rank);
			global::System.Collections.Generic.List<global::UnitRankProgressionData> list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::UnitRankProgressionData>(new string[]
			{
				"fk_unit_type_id",
				"rank"
			}, new string[]
			{
				((int)unit.GetUnitTypeId()).ToString(),
				unit.Rank.ToString()
			});
			if (base.isActiveAndEnabled)
			{
				base.StartCoroutine(this.RefreshAdvancementBars(advancementsCountForRank));
			}
			this.xp.maxValue = (float)(advancementsCountForRank * list[0].Xp);
			this.xp.value = (float)(unitRankData.Advancement * list[0].Xp + unit.Xp);
			this.xp.fillRect.gameObject.SetActive(this.xp.value > 0f);
			this.xpValue.enabled = true;
			this.xpValue.text = this.xp.value + " / " + this.xp.maxValue;
		}
		this.icon.sprite = unit.GetIcon();
		switch (unit.GetUnitTypeId())
		{
		case global::UnitTypeId.LEADER:
			this.iconStar.enabled = true;
			this.iconStar.sprite = global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadResource<global::UnityEngine.Sprite>("icn_leader", true);
			goto IL_2AC;
		case global::UnitTypeId.IMPRESSIVE:
			this.iconStar.enabled = true;
			this.iconStar.sprite = global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadResource<global::UnityEngine.Sprite>("icn_impressive", true);
			goto IL_2AC;
		case global::UnitTypeId.HERO_1:
		case global::UnitTypeId.HERO_2:
		case global::UnitTypeId.HERO_3:
			this.iconStar.enabled = true;
			this.iconStar.sprite = global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadResource<global::UnityEngine.Sprite>("icn_heroes", true);
			goto IL_2AC;
		}
		this.iconStar.enabled = false;
		IL_2AC:
		this.type.text = unit.LocalizedType;
		this.rating.text = ((!unit.Active) ? ("0 (" + unit.GetRating() + ")") : unit.GetRating().ToString());
		this.status.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(string.Format("unit_status_name_{0}", unit.GetActiveStatus().ToLowerString()));
		for (int j = 0; j < this.stats.Count; j++)
		{
			this.stats[j].RefreshAttribute(unit);
		}
	}

	private global::System.Collections.IEnumerator RefreshAdvancementBars(int rankProgressionCount)
	{
		while (((global::UnityEngine.RectTransform)this.advancements[0].transform.parent).rect.width <= 1f)
		{
			yield return 0;
		}
		float offset = ((global::UnityEngine.RectTransform)this.advancements[0].transform.parent).rect.width / (float)rankProgressionCount;
		for (int i = 0; i < this.advancements.Count; i++)
		{
			if (i < rankProgressionCount - 1)
			{
				this.advancements[i].gameObject.SetActive(true);
				global::UnityEngine.Vector2 pos = this.advancements[i].anchoredPosition;
				pos.x = offset * (float)(i + 1);
				this.advancements[i].anchoredPosition = pos;
			}
			else
			{
				this.advancements[i].gameObject.SetActive(false);
			}
		}
		yield break;
	}

	private void OnEnable()
	{
		if (this.toggleGroup != null)
		{
			this.toggleGroup.SetAllTogglesOff();
		}
		this.highlight.Deactivate();
	}

	private int GetAdvancementsCountForRank(int rank)
	{
		global::System.Collections.Generic.List<global::UnitRankData> list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::UnitRankData>("rank", rank.ToString());
		int num = 0;
		for (int i = 0; i < list.Count; i++)
		{
			if (global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::UnitJoinUnitRankData>("fk_unit_id", this.unit.Id.ToIntString<global::UnitId>(), "fk_unit_rank_id", list[i].Id.ToIntString<global::UnitRankId>()).Count > 0)
			{
				num++;
			}
		}
		return num;
	}

	public void RemoveDisplayedXp(int xpRemoved)
	{
		this.xpValue.enabled = true;
		global::UnitRankData unitRankData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::UnitRankData>((int)this.unit.UnitSave.rankId);
		int advancementsCountForRank = this.GetAdvancementsCountForRank(this.unit.Rank);
		int num = (this.unit.Rank != global::Constant.GetInt(global::ConstantId.MAX_UNIT_RANK)) ? global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::UnitRankProgressionData>(new string[]
		{
			"fk_unit_type_id",
			"rank"
		}, new string[]
		{
			((int)this.unit.GetUnitTypeId()).ToString(),
			this.unit.Rank.ToString()
		})[0].Xp : 0;
		int num2 = unitRankData.Advancement * num + this.unit.Xp;
		int num3 = advancementsCountForRank * num;
		if (num2 < xpRemoved)
		{
			xpRemoved -= num2;
			int num4 = int.Parse(this.rank.text) - 1;
			this.rank.text = num4.ToString();
			advancementsCountForRank = this.GetAdvancementsCountForRank(num4);
			num = ((num4 != global::Constant.GetInt(global::ConstantId.MAX_UNIT_RANK)) ? global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::UnitRankProgressionData>(new string[]
			{
				"fk_unit_type_id",
				"rank"
			}, new string[]
			{
				((int)this.unit.GetUnitTypeId()).ToString(),
				num4.ToString()
			})[0].Xp : 0);
			num3 = advancementsCountForRank * num;
			this.xp.value = (float)(num3 - xpRemoved);
		}
		else
		{
			this.xp.value = this.xp.value - (float)xpRemoved;
		}
		this.xp.maxValue = (float)num3;
		this.xpValue.text = this.xp.value + " / " + this.xp.maxValue;
	}

	public void AddXp(int xpAdded)
	{
		global::DG.Tweening.DOTween.Kill(this.xp, false);
		if (xpAdded == 0)
		{
			return;
		}
		this.AddOneXp(xpAdded);
	}

	private void AddOneXp(int total)
	{
		int num = (int)this.xp.value + ((total <= 0) ? -1 : 1);
		total += ((total <= 0) ? 1 : -1);
		this.xp.DOValue((float)num, 0.1f, false).OnComplete(delegate
		{
			if (this.xp.value >= this.xp.maxValue)
			{
				this.DoLoopXp();
			}
			this.xpValue.text = this.xp.value + " / " + this.xp.maxValue;
			if (total != 0)
			{
				this.AddOneXp(total);
			}
		});
	}

	private void DoLoopXp()
	{
		global::PandoraSingleton<global::Pan>.Instance.Narrate("unit_progressed" + global::PandoraSingleton<global::GameManager>.Instance.LocalTyche.Rand(1, 6));
		int num = int.Parse(this.rank.text) + 1;
		this.rank.text = num.ToString();
		if (num < global::Constant.GetInt(global::ConstantId.MAX_UNIT_RANK))
		{
			this.xp.value = 0f;
			int advancementsCountForRank = this.GetAdvancementsCountForRank(num);
			int num2 = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::UnitRankProgressionData>(new string[]
			{
				"fk_unit_type_id",
				"rank"
			}, new string[]
			{
				((int)this.unit.GetUnitTypeId()).ToString(),
				this.unit.Rank.ToString()
			})[0].Xp;
			if (base.isActiveAndEnabled)
			{
				base.StartCoroutine(this.RefreshAdvancementBars(advancementsCountForRank));
			}
			this.xp.maxValue = (float)(advancementsCountForRank * num2);
		}
		else
		{
			for (int i = 0; i < this.advancements.Count; i++)
			{
				this.advancements[i].gameObject.SetActive(false);
			}
			this.xpValue.enabled = false;
			this.xp.maxValue = 1E+09f;
		}
	}

	private const string statusLoc = "unit_status_name_{0}";

	public global::HightlightAnimate highlight;

	public global::UnityEngine.UI.ToggleGroup toggleGroup;

	public global::UnityEngine.UI.Text rank;

	public global::UnityEngine.UI.Text unitName;

	public global::UnityEngine.UI.Text xpValue;

	public global::UnityEngine.UI.Slider xp;

	public global::UnityEngine.UI.Image icon;

	public global::UnityEngine.UI.Image iconStar;

	public global::UnityEngine.UI.Text type;

	public global::UnityEngine.UI.Text rating;

	public global::UnityEngine.UI.Text status;

	public global::ToggleEffects statusToggle;

	public global::ToggleEffects ratingToggle;

	public global::System.Collections.Generic.List<global::UIStat> stats;

	public global::System.Collections.Generic.List<global::UnityEngine.RectTransform> advancements;

	private global::Unit unit;
}
