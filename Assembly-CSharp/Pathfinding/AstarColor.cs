using System;
using UnityEngine;

namespace Pathfinding
{
	[global::System.Serializable]
	public class AstarColor
	{
		public AstarColor()
		{
			this._NodeConnection = new global::UnityEngine.Color(1f, 1f, 1f, 0.9f);
			this._UnwalkableNode = new global::UnityEngine.Color(1f, 0f, 0f, 0.5f);
			this._BoundsHandles = new global::UnityEngine.Color(0.29f, 0.454f, 0.741f, 0.9f);
			this._ConnectionLowLerp = new global::UnityEngine.Color(0f, 1f, 0f, 0.5f);
			this._ConnectionHighLerp = new global::UnityEngine.Color(1f, 0f, 0f, 0.5f);
			this._MeshEdgeColor = new global::UnityEngine.Color(0f, 0f, 0f, 0.5f);
		}

		public static global::UnityEngine.Color GetAreaColor(uint area)
		{
			if (global::Pathfinding.AstarColor.AreaColors == null || (ulong)area >= (ulong)((long)global::Pathfinding.AstarColor.AreaColors.Length))
			{
				return global::Pathfinding.AstarMath.IntToColor((int)area, 1f);
			}
			return global::Pathfinding.AstarColor.AreaColors[(int)area];
		}

		public void OnEnable()
		{
			global::Pathfinding.AstarColor.NodeConnection = this._NodeConnection;
			global::Pathfinding.AstarColor.UnwalkableNode = this._UnwalkableNode;
			global::Pathfinding.AstarColor.BoundsHandles = this._BoundsHandles;
			global::Pathfinding.AstarColor.ConnectionLowLerp = this._ConnectionLowLerp;
			global::Pathfinding.AstarColor.ConnectionHighLerp = this._ConnectionHighLerp;
			global::Pathfinding.AstarColor.MeshEdgeColor = this._MeshEdgeColor;
			global::Pathfinding.AstarColor.AreaColors = this._AreaColors;
		}

		public global::UnityEngine.Color _NodeConnection;

		public global::UnityEngine.Color _UnwalkableNode;

		public global::UnityEngine.Color _BoundsHandles;

		public global::UnityEngine.Color _ConnectionLowLerp;

		public global::UnityEngine.Color _ConnectionHighLerp;

		public global::UnityEngine.Color _MeshEdgeColor;

		public global::UnityEngine.Color[] _AreaColors;

		public static global::UnityEngine.Color NodeConnection = new global::UnityEngine.Color(1f, 1f, 1f, 0.9f);

		public static global::UnityEngine.Color UnwalkableNode = new global::UnityEngine.Color(1f, 0f, 0f, 0.5f);

		public static global::UnityEngine.Color BoundsHandles = new global::UnityEngine.Color(0.29f, 0.454f, 0.741f, 0.9f);

		public static global::UnityEngine.Color ConnectionLowLerp = new global::UnityEngine.Color(0f, 1f, 0f, 0.5f);

		public static global::UnityEngine.Color ConnectionHighLerp = new global::UnityEngine.Color(1f, 0f, 0f, 0.5f);

		public static global::UnityEngine.Color MeshEdgeColor = new global::UnityEngine.Color(0f, 0f, 0f, 0.5f);

		private static global::UnityEngine.Color[] AreaColors;
	}
}
