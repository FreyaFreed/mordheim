using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SmugglerFactionOverviewModule : global::UIModule
{
	public void Setup(global::System.Action<global::FactionMenuController> onSelectCb, global::System.Action<global::FactionMenuController> onConfirmCb)
	{
		if (!this.init)
		{
			this.init = true;
			this.cards = new global::System.Collections.Generic.List<global::FactionOverviewCard>(global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.factionCtrlrs.Count);
			this.cardTweens = new global::System.Collections.Generic.List<global::DG.Tweening.Tweener>(global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.factionCtrlrs.Count);
			for (int i = 0; i < global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.factionCtrlrs.Count; i++)
			{
				global::FactionOverviewCard factionOverviewCard = global::UnityEngine.Object.Instantiate<global::FactionOverviewCard>(this.factionTemplate);
				factionOverviewCard.transform.SetParent(this.factionsContainer, false);
				factionOverviewCard.gameObject.SetActive(true);
				this.cards.Add(factionOverviewCard);
				global::ToggleEffects component = factionOverviewCard.GetComponent<global::ToggleEffects>();
				global::FactionMenuController faction = global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.factionCtrlrs[i];
				component.onSelect.AddListener(delegate()
				{
					this.FactionSelected(faction);
				});
				component.onAction.AddListener(delegate()
				{
					this.FactionConfirmed(faction);
				});
			}
		}
		this.onSelect = onSelectCb;
		this.onConfirm = onConfirmCb;
		this.Refresh();
		this.currentSelection = this.primaryCardIdx;
	}

	public void Refresh()
	{
		for (int i = 0; i < global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.factionCtrlrs.Count; i++)
		{
			if (global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.factionCtrlrs[i] != null)
			{
				global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.factionCtrlrs[i].Refresh();
				this.cards[i].SetFaction(global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.factionCtrlrs[i]);
				if (global::PandoraSingleton<global::HideoutManager>.Instance.WarbandCtrlr.factionCtrlrs[i].Faction.Primary)
				{
					this.cards[i].transform.SetAsFirstSibling();
					this.primaryCardIdx = i;
				}
			}
		}
	}

	private void OnDisable()
	{
		if (this.cards != null)
		{
			for (int i = 0; i < this.cards.Count; i++)
			{
				this.cards[i].GetComponent<global::ToggleEffects>().toggle.isOn = false;
				((global::UnityEngine.RectTransform)this.cards[i].transform.GetChild(0)).localPosition = global::UnityEngine.Vector3.zero;
			}
		}
	}

	public void SetFocus()
	{
		this.cards[this.currentSelection].SetSelected(false);
	}

	public void OnLostFocus()
	{
		this.HideHighlight();
	}

	private void FactionSelected(global::FactionMenuController faction)
	{
		this.ShowHighlight();
		for (int i = 0; i < this.cardTweens.Count; i++)
		{
			global::DG.Tweening.DOTween.Kill(this.cardTweens[i].id, false);
		}
		this.cardTweens.Clear();
		for (int j = 0; j < this.cards.Count; j++)
		{
			global::UnityEngine.RectTransform cardMovingPart = (global::UnityEngine.RectTransform)this.cards[j].transform.GetChild(0);
			if (this.cards[j].FactionCtrlr == faction)
			{
				this.cardTweens.Add(global::DG.Tweening.DOTween.To(() => cardMovingPart.localPosition, delegate(global::UnityEngine.Vector3 v)
				{
					cardMovingPart.localPosition = v;
				}, this.CARD_SLIDE_DESTINATION, 0.5f));
				this.currentSelection = j;
			}
			else if (cardMovingPart.localPosition.x != 0f)
			{
				this.cardTweens.Add(global::DG.Tweening.DOTween.To(() => cardMovingPart.localPosition, delegate(global::UnityEngine.Vector3 v)
				{
					cardMovingPart.localPosition = v;
				}, global::UnityEngine.Vector3.zero, 0.5f));
			}
		}
		if (this.onSelect != null)
		{
			this.onSelect(faction);
		}
	}

	private void FactionConfirmed(global::FactionMenuController faction)
	{
		if (this.onConfirm != null)
		{
			this.onConfirm(faction);
		}
	}

	private void ShowHighlight()
	{
		global::HighlightToggle component = this.cards[this.currentSelection].GetComponent<global::HighlightToggle>();
		component.hightlight.enabled = true;
		component.hightlight.gameObject.SetActive(true);
		component.hightlight.Highlight(component._targetTransform);
	}

	private void HideHighlight()
	{
		global::HighlightToggle component = this.cards[0].GetComponent<global::HighlightToggle>();
		component.hightlight.gameObject.SetActive(false);
		component.hightlight.enabled = false;
	}

	private const float CARD_SLIDE_TIME = 0.5f;

	private global::UnityEngine.Vector3 CARD_SLIDE_DESTINATION = new global::UnityEngine.Vector3(40f, 0f, 0f);

	private global::System.Collections.Generic.List<global::FactionOverviewCard> cards;

	private global::System.Collections.Generic.List<global::DG.Tweening.Tweener> cardTweens;

	public global::FactionOverviewCard factionTemplate;

	public global::UnityEngine.Transform factionsContainer;

	private global::System.Action<global::FactionMenuController> onSelect;

	private global::System.Action<global::FactionMenuController> onConfirm;

	private int primaryCardIdx;

	private int currentSelection;

	private bool init;
}
