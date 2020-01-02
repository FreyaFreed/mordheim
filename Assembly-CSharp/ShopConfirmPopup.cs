using System;
using UnityEngine;
using UnityEngine.UI;

public class ShopConfirmPopup : global::ConfirmationPopupView
{
	public void Show(string title, string desc, int availableQty, int unitValue, bool checkFunds, global::System.Action<bool, int> callback)
	{
		this.unitValue = unitValue;
		this.checkFunds = checkFunds;
		this.totalValue.color = global::UnityEngine.Color.white;
		this.confirmButton.SetDisabled(false);
		this.qtySelector.selections.Clear();
		for (int i = 1; i <= availableQty; i++)
		{
			this.qtySelector.selections.Add(i.ToString());
		}
		this.qtySelector.onValueChanged = delegate(int id, int idx)
		{
			this.UpdateTotal();
		};
		this.qtySelector.SetCurrentSel(0);
		this.UpdateTotal();
		base.ShowLocalized(title, desc, delegate(bool confirm)
		{
			callback(confirm, this.qtySelector.CurSel + 1);
		}, false, false);
	}

	private void UpdateTotal()
	{
		int num = this.qtySelector.CurSel + 1;
		int num2 = num * this.unitValue;
		this.totalValue.text = num2.ToString();
		if (this.checkFunds)
		{
			if (global::PandoraSingleton<global::HideoutManager>.Instance.WarbandChest.GetGold() >= num2)
			{
				this.totalValue.color = global::UnityEngine.Color.white;
				this.confirmButton.SetDisabled(false);
			}
			else
			{
				this.totalValue.color = global::UnityEngine.Color.red;
				this.confirmButton.SetDisabled(true);
			}
		}
	}

	public global::SelectorGroup qtySelector;

	public global::UnityEngine.UI.Text totalValue;

	private int unitValue;

	private bool checkFunds;
}
