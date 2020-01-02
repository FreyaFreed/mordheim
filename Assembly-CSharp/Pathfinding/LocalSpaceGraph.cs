using System;
using UnityEngine;

namespace Pathfinding
{
	[global::UnityEngine.HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_local_space_graph.php")]
	public class LocalSpaceGraph : global::UnityEngine.MonoBehaviour
	{
		private void Start()
		{
			this.originalMatrix = base.transform.localToWorldMatrix;
		}

		public global::UnityEngine.Matrix4x4 GetMatrix()
		{
			return base.transform.worldToLocalMatrix * this.originalMatrix;
		}

		protected global::UnityEngine.Matrix4x4 originalMatrix;
	}
}
