using System;
using UnityEngine;

namespace Pathfinding
{
	[global::UnityEngine.HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_relevant_graph_surface.php")]
	[global::UnityEngine.AddComponentMenu("Pathfinding/Navmesh/RelevantGraphSurface")]
	public class RelevantGraphSurface : global::UnityEngine.MonoBehaviour
	{
		public global::UnityEngine.Vector3 Position
		{
			get
			{
				return this.position;
			}
		}

		public global::Pathfinding.RelevantGraphSurface Next
		{
			get
			{
				return this.next;
			}
		}

		public global::Pathfinding.RelevantGraphSurface Prev
		{
			get
			{
				return this.prev;
			}
		}

		public static global::Pathfinding.RelevantGraphSurface Root
		{
			get
			{
				return global::Pathfinding.RelevantGraphSurface.root;
			}
		}

		public void UpdatePosition()
		{
			this.position = base.transform.position;
		}

		private void OnEnable()
		{
			this.UpdatePosition();
			if (global::Pathfinding.RelevantGraphSurface.root == null)
			{
				global::Pathfinding.RelevantGraphSurface.root = this;
			}
			else
			{
				this.next = global::Pathfinding.RelevantGraphSurface.root;
				global::Pathfinding.RelevantGraphSurface.root.prev = this;
				global::Pathfinding.RelevantGraphSurface.root = this;
			}
		}

		private void OnDisable()
		{
			if (global::Pathfinding.RelevantGraphSurface.root == this)
			{
				global::Pathfinding.RelevantGraphSurface.root = this.next;
				if (global::Pathfinding.RelevantGraphSurface.root != null)
				{
					global::Pathfinding.RelevantGraphSurface.root.prev = null;
				}
			}
			else
			{
				if (this.prev != null)
				{
					this.prev.next = this.next;
				}
				if (this.next != null)
				{
					this.next.prev = this.prev;
				}
			}
			this.prev = null;
			this.next = null;
		}

		public static void UpdateAllPositions()
		{
			global::Pathfinding.RelevantGraphSurface relevantGraphSurface = global::Pathfinding.RelevantGraphSurface.root;
			while (relevantGraphSurface != null)
			{
				relevantGraphSurface.UpdatePosition();
				relevantGraphSurface = relevantGraphSurface.Next;
			}
		}

		public static void FindAllGraphSurfaces()
		{
			global::Pathfinding.RelevantGraphSurface[] array = global::UnityEngine.Object.FindObjectsOfType(typeof(global::Pathfinding.RelevantGraphSurface)) as global::Pathfinding.RelevantGraphSurface[];
			for (int i = 0; i < array.Length; i++)
			{
				array[i].OnDisable();
				array[i].OnEnable();
			}
		}

		public void OnDrawGizmos()
		{
			global::UnityEngine.Gizmos.color = new global::UnityEngine.Color(0.223529413f, 0.827451f, 0.180392161f, 0.4f);
			global::UnityEngine.Gizmos.DrawLine(base.transform.position - global::UnityEngine.Vector3.up * this.maxRange, base.transform.position + global::UnityEngine.Vector3.up * this.maxRange);
		}

		public void OnDrawGizmosSelected()
		{
			global::UnityEngine.Gizmos.color = new global::UnityEngine.Color(0.223529413f, 0.827451f, 0.180392161f);
			global::UnityEngine.Gizmos.DrawLine(base.transform.position - global::UnityEngine.Vector3.up * this.maxRange, base.transform.position + global::UnityEngine.Vector3.up * this.maxRange);
		}

		private static global::Pathfinding.RelevantGraphSurface root;

		public float maxRange = 1f;

		private global::Pathfinding.RelevantGraphSurface prev;

		private global::Pathfinding.RelevantGraphSurface next;

		private global::UnityEngine.Vector3 position;
	}
}
