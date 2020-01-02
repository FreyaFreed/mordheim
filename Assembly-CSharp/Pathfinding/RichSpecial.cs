using System;
using UnityEngine;

namespace Pathfinding
{
	public class RichSpecial : global::Pathfinding.RichPathPart
	{
		public override void OnEnterPool()
		{
			this.nodeLink = null;
		}

		public global::Pathfinding.RichSpecial Initialize(global::Pathfinding.NodeLink2 nodeLink, global::Pathfinding.GraphNode first)
		{
			this.nodeLink = nodeLink;
			if (first == nodeLink.startNode)
			{
				this.first = nodeLink.StartTransform;
				this.second = nodeLink.EndTransform;
				this.reverse = false;
			}
			else
			{
				this.first = nodeLink.EndTransform;
				this.second = nodeLink.StartTransform;
				this.reverse = true;
			}
			return this;
		}

		public global::Pathfinding.NodeLink2 nodeLink;

		public global::UnityEngine.Transform first;

		public global::UnityEngine.Transform second;

		public bool reverse;
	}
}
