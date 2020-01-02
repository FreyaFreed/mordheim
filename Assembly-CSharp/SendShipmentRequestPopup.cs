using System;
using UnityEngine.UI;

public class SendShipmentRequestPopup : global::UIPopupModule
{
	public void Set(global::FactionRequest request)
	{
		this.totalWeight.text = request.totalWeight.text;
		this.weightRequirement.text = request.weightRequirement.text;
		this.goldReward.text = request.goldReward.text;
		this.overweightGold.text = request.overweightGold.text;
		this.overweightReputation.text = request.overweightReputation.text;
	}

	public global::UnityEngine.UI.Text weightRequirement;

	public global::UnityEngine.UI.Text goldReward;

	public global::UnityEngine.UI.Text totalWeight;

	public global::UnityEngine.UI.Text overweightGold;

	public global::UnityEngine.UI.Text overweightReputation;
}
