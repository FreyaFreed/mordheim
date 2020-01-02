using System;
using UnityEngine.UI;

public class SendShipmentPopup : global::UIPopupModule
{
	public void Set(global::FactionDelivery request)
	{
		this.nbFrag.text = request.FragmentCount.ToString();
		this.nbShard.text = request.ShardCount.ToString();
		this.nbCluster.text = request.ClusterCount.ToString();
		this.totalGold.text = request.totalGold.text;
		this.totalRep.text = request.totalRep.text;
	}

	public global::UnityEngine.UI.Text nbFrag;

	public global::UnityEngine.UI.Text nbShard;

	public global::UnityEngine.UI.Text nbCluster;

	public global::UnityEngine.UI.Text totalGold;

	public global::UnityEngine.UI.Text totalRep;
}
