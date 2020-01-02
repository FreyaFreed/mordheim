using System;
using Pathfinding;
using UnityEngine;

public class test_update_graph : global::UnityEngine.MonoBehaviour
{
	private void Start()
	{
		this.oldWalkable = !this.walkable;
	}

	private void Update()
	{
		if (this.oldWalkable != this.walkable)
		{
			this.oldWalkable = this.walkable;
			global::Pathfinding.GraphUpdateObject graphUpdateObject = new global::Pathfinding.GraphUpdateObject(base.gameObject.GetComponent<global::UnityEngine.Renderer>().bounds);
			graphUpdateObject.modifyWalkability = true;
			graphUpdateObject.setWalkability = this.walkable;
			global::AstarPath.active.UpdateGraphs(graphUpdateObject);
		}
	}

	public bool walkable;

	private bool oldWalkable;
}
