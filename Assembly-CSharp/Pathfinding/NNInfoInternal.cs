using System;
using UnityEngine;

namespace Pathfinding
{
	public struct NNInfoInternal
	{
		public NNInfoInternal(global::Pathfinding.GraphNode node)
		{
			this.node = node;
			this.constrainedNode = null;
			this.clampedPosition = global::UnityEngine.Vector3.zero;
			this.constClampedPosition = global::UnityEngine.Vector3.zero;
			this.UpdateInfo();
		}

		public void UpdateInfo()
		{
			this.clampedPosition = ((this.node == null) ? global::UnityEngine.Vector3.zero : ((global::UnityEngine.Vector3)this.node.position));
			this.constClampedPosition = ((this.constrainedNode == null) ? global::UnityEngine.Vector3.zero : ((global::UnityEngine.Vector3)this.constrainedNode.position));
		}

		public global::Pathfinding.GraphNode node;

		public global::Pathfinding.GraphNode constrainedNode;

		public global::UnityEngine.Vector3 clampedPosition;

		public global::UnityEngine.Vector3 constClampedPosition;
	}
}
