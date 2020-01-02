using System;
using UnityEngine;

namespace Pathfinding
{
	public class LinkedLevelNode
	{
		public global::UnityEngine.Vector3 position;

		public bool walkable;

		public global::UnityEngine.RaycastHit hit;

		public float height;

		public global::Pathfinding.LinkedLevelNode next;
	}
}
