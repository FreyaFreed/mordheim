using System;
using UnityEngine;

namespace Pathfinding
{
	public class RecastBBTreeBox
	{
		public RecastBBTreeBox(global::Pathfinding.RecastMeshObj mesh)
		{
			this.mesh = mesh;
			global::UnityEngine.Vector3 min = mesh.bounds.min;
			global::UnityEngine.Vector3 max = mesh.bounds.max;
			this.rect = global::UnityEngine.Rect.MinMaxRect(min.x, min.z, max.x, max.z);
		}

		public bool Contains(global::UnityEngine.Vector3 p)
		{
			return this.rect.Contains(p);
		}

		public global::UnityEngine.Rect rect;

		public global::Pathfinding.RecastMeshObj mesh;

		public global::Pathfinding.RecastBBTreeBox c1;

		public global::Pathfinding.RecastBBTreeBox c2;
	}
}
