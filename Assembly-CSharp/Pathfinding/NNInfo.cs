using System;
using UnityEngine;

namespace Pathfinding
{
	public struct NNInfo
	{
		public NNInfo(global::Pathfinding.NNInfoInternal internalInfo)
		{
			this.node = internalInfo.node;
			this.position = internalInfo.clampedPosition;
		}

		[global::System.Obsolete("This field has been renamed to 'position'")]
		public global::UnityEngine.Vector3 clampedPosition
		{
			get
			{
				return this.position;
			}
		}

		public static explicit operator global::UnityEngine.Vector3(global::Pathfinding.NNInfo ob)
		{
			return ob.position;
		}

		public static explicit operator global::Pathfinding.GraphNode(global::Pathfinding.NNInfo ob)
		{
			return ob.node;
		}

		public readonly global::Pathfinding.GraphNode node;

		public readonly global::UnityEngine.Vector3 position;
	}
}
