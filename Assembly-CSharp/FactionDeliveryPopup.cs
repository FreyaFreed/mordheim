using System;
using UnityEngine.UI;

public class FactionDeliveryPopup : global::UIPopupModule
{
	public void Set(global::FactionDelivery delivery)
	{
		this.totalWeight.text = delivery.TotalWeight.ToString();
		this.totalGold.text = delivery.TotalGold.ToString();
		this.totalReputation.text = delivery.TotalWeight.ToString();
	}

	public global::UnityEngine.UI.Text totalWeight;

	public global::UnityEngine.UI.Text totalGold;

	public global::UnityEngine.UI.Text totalReputation;
}
