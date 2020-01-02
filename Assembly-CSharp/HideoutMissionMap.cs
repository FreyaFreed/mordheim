using System;
using System.Collections.Generic;

public class HideoutMissionMap : global::MenuNodeGroup
{
	protected override void Awake()
	{
		base.Awake();
		this.nodes.Sort((global::MenuNode a, global::MenuNode b) => b.transform.position.x.CompareTo(a.transform.position.x));
	}

	public void Activate(global::MenuNodeDelegateNode selectCampaign, global::MenuNodeDelegateNode selectDis01Node, global::MenuNodeDelegateNode selectDis02Node, global::MenuNodeDelegateNode unSelect, global::MenuNodeDelegateNode confirm, global::PandoraInput.InputLayer workingLayer, bool unselectOverOut = true)
	{
		this.onSelectCampaign = selectCampaign;
		this.onSelectDis01Node = selectDis01Node;
		this.onSelectDis02Node = selectDis02Node;
		base.Activate(new global::MenuNodeDelegateNode(this.OnSelectNode), unSelect, confirm, workingLayer, unselectOverOut);
	}

	private void OnSelectNode(global::MenuNode node, int idx)
	{
		node.SetSelected(true);
		idx = this.campaignNodes.IndexOf(node);
		if (idx != -1)
		{
			this.onSelectCampaign(node, idx);
			return;
		}
		idx = this.procNodesDis01.IndexOf(node);
		if (idx != -1)
		{
			this.onSelectDis01Node(node, idx);
			return;
		}
		idx = this.procNodesDis02.IndexOf(node);
		if (idx != -1)
		{
			this.onSelectDis02Node(node, idx);
			return;
		}
	}

	public global::System.Collections.Generic.List<global::MenuNode> campaignNodes;

	public global::System.Collections.Generic.List<global::MenuNode> procNodesDis01;

	public global::System.Collections.Generic.List<global::MenuNode> procNodesDis02;

	private global::MenuNodeDelegateNode onSelectCampaign;

	private global::MenuNodeDelegateNode onSelectDis01Node;

	private global::MenuNodeDelegateNode onSelectDis02Node;
}
