using System;
using UnityEngine;

namespace Pathfinding
{
	public struct GraphHitInfo
	{
		public GraphHitInfo(global::UnityEngine.Vector3 point)
		{
			this.tangentOrigin = global::UnityEngine.Vector3.zero;
			this.origin = global::UnityEngine.Vector3.zero;
			this.point = point;
			this.node = null;
			this.tangent = global::UnityEngine.Vector3.zero;
		}

		public float distance
		{
			get
			{
				return (this.point - this.origin).magnitude;
			}
		}

		public global::UnityEngine.Vector3 origin;

		public global::UnityEngine.Vector3 point;

		public global::Pathfinding.GraphNode node;

		public global::UnityEngine.Vector3 tangentOrigin;

		public global::UnityEngine.Vector3 tangent;
	}
}
