using System;
using System.Collections.Generic;
using UnityEngine;

public class MenuNodeGroup : global::UnityEngine.MonoBehaviour
{
	protected virtual void Awake()
	{
		this.isOn = false;
	}

	public void Activate(global::MenuNodeDelegateNode select, global::MenuNodeDelegateNode unSelect, global::MenuNodeDelegateNode confirm, global::PandoraInput.InputLayer workingLayer, bool unselectOverOut = true)
	{
		this.selectDel = select;
		this.unSelectDel = unSelect;
		this.confirmDel = confirm;
		this.layer = workingLayer;
		this.unSelectOnOverOut = unselectOverOut;
		this.UnSelectCurrentNode();
		this.isOn = true;
		for (int i = 0; i < this.nodes.Count; i++)
		{
			this.nodes[i].Unselect();
		}
		this.mouseLastPosition = global::PandoraSingleton<global::PandoraInput>.Instance.GetMousePosition();
	}

	public void Deactivate()
	{
		this.selectDel = null;
		this.unSelectDel = null;
		this.confirmDel = null;
		this.isOn = false;
		this.UnSelectCurrentNode();
		for (int i = 0; i < this.nodes.Count; i++)
		{
			this.nodes[i].Unselect();
		}
	}

	public void Clear()
	{
		for (int i = 0; i < this.nodes.Count; i++)
		{
			this.nodes[i].RemoveContent();
		}
	}

	private void Update()
	{
		if (!this.isOn)
		{
			return;
		}
		global::UnityEngine.Vector3 mousePosition = global::PandoraSingleton<global::PandoraInput>.Instance.GetMousePosition();
		if (mousePosition != this.mouseLastPosition)
		{
			this.mouseLastPosition = mousePosition;
			global::MenuNode overNode = this.GetOverNode();
			if (overNode != null && this.nodes.IndexOf(overNode) != -1)
			{
				this.SelectNode(overNode);
			}
			else if (this.unSelectOnOverOut)
			{
				this.UnSelectCurrentNode();
			}
		}
		else if (global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyDown("h", 0) || global::PandoraSingleton<global::PandoraInput>.Instance.GetNegKeyDown("h", 0))
		{
			for (int i = 0; i < this.nodes.Count; i++)
			{
				this.nodeIdx += ((!global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyDown("h", 0)) ? -1 : 1);
				this.nodeIdx = ((this.nodeIdx < this.nodes.Count) ? this.nodeIdx : 0);
				this.nodeIdx = ((this.nodeIdx >= 0) ? this.nodeIdx : (this.nodes.Count - 1));
				if (this.nodes[this.nodeIdx].IsSelectable && this.nodes[this.nodeIdx].gameObject.activeSelf)
				{
					break;
				}
			}
			this.SelectNode(this.nodes[this.nodeIdx]);
		}
		else if (global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp("action", 0))
		{
			this.ConfirmNode();
		}
		else if (global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp("mouse_click", 0))
		{
			global::MenuNode overNode2 = this.GetOverNode();
			if (overNode2 != null && this.nodes.IndexOf(overNode2) != -1)
			{
				this.ConfirmNode();
			}
		}
	}

	public void SelectNode(global::MenuNode node)
	{
		if (this.currentNode != node)
		{
			this.UnSelectCurrentNode();
			this.currentNode = node;
			node.Select();
			this.nodeIdx = this.nodes.IndexOf(node);
			if (this.selectDel != null)
			{
				this.selectDel(this.currentNode, this.nodeIdx);
			}
			if (this.setDOF)
			{
				global::PandoraSingleton<global::HideoutManager>.Instance.CamManager.SetDOFTarget(this.currentNode.gameObject.transform, 0f);
			}
		}
	}

	public void UnSelectCurrentNode()
	{
		if (this.currentNode != null)
		{
			global::MenuNode node = this.currentNode;
			int idx = this.nodeIdx;
			this.currentNode.Unselect();
			this.currentNode = null;
			this.nodeIdx = 0;
			if (this.unSelectDel != null)
			{
				this.unSelectDel(node, idx);
			}
		}
	}

	private void ConfirmNode()
	{
		if (this.currentNode == null)
		{
			return;
		}
		global::MenuNode node = this.currentNode;
		int idx = this.nodeIdx;
		this.UnSelectCurrentNode();
		if (this.confirmDel != null)
		{
			this.confirmDel(node, idx);
		}
	}

	private global::MenuNode GetOverNode()
	{
		if (global::PandoraSingleton<global::PandoraInput>.Instance.CurrentInputLayer == (int)this.layer)
		{
			global::UnityEngine.Ray ray = global::UnityEngine.Camera.main.ScreenPointToRay(global::UnityEngine.Input.mousePosition);
			global::UnityEngine.RaycastHit raycastHit;
			if (global::UnityEngine.Physics.Raycast(ray, out raycastHit, float.PositiveInfinity, global::LayerMaskManager.menuNodeMask))
			{
				global::MenuNode componentInParent = raycastHit.transform.GetComponentInParent<global::MenuNode>();
				if (componentInParent != null && componentInParent.IsSelectable)
				{
					return componentInParent;
				}
			}
		}
		return null;
	}

	public bool setDOF = true;

	public global::System.Collections.Generic.List<global::MenuNode> nodes;

	private bool isOn;

	private global::UnityEngine.Vector3 mouseLastPosition;

	private global::MenuNode currentNode;

	private int nodeIdx;

	private global::MenuNodeDelegateNode selectDel;

	private global::MenuNodeDelegateNode unSelectDel;

	private global::MenuNodeDelegateNode confirmDel;

	private bool unSelectOnOverOut;

	private global::PandoraInput.InputLayer layer;
}
