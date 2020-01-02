using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
	[global::UnityEngine.HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_raycast_modifier.php")]
	[global::UnityEngine.RequireComponent(typeof(global::Seeker))]
	[global::UnityEngine.AddComponentMenu("Pathfinding/Modifiers/Raycast Simplifier")]
	[global::System.Serializable]
	public class RaycastModifier : global::Pathfinding.MonoModifier
	{
		public override int Order
		{
			get
			{
				return 40;
			}
		}

		public override void Apply(global::Pathfinding.Path p)
		{
			if (this.iterations <= 0)
			{
				return;
			}
			if (!this.useRaycasting && !this.useGraphRaycasting)
			{
				global::UnityEngine.Debug.LogWarning("RaycastModifier is configured to not use either raycasting or graph raycasting. This would simplify the path to a straight line. The modifier will not be applied.");
				return;
			}
			global::System.Collections.Generic.List<global::UnityEngine.Vector3> vectorPath = p.vectorPath;
			for (int i = 0; i < this.iterations; i++)
			{
				if (this.subdivideEveryIter && i != 0)
				{
					global::Pathfinding.RaycastModifier.Subdivide(vectorPath);
				}
				int j = 0;
				while (j < vectorPath.Count - 2)
				{
					global::UnityEngine.Vector3 v = vectorPath[j];
					global::UnityEngine.Vector3 v2 = vectorPath[j + 2];
					if (this.ValidateLine(null, null, v, v2))
					{
						vectorPath.RemoveAt(j + 1);
					}
					else
					{
						j++;
					}
				}
			}
		}

		private static void Subdivide(global::System.Collections.Generic.List<global::UnityEngine.Vector3> points)
		{
			if (points.Capacity < points.Count * 3)
			{
				points.Capacity = points.Count * 3;
			}
			int count = points.Count;
			for (int i = 0; i < count - 1; i++)
			{
				points.Add(global::UnityEngine.Vector3.zero);
				points.Add(global::UnityEngine.Vector3.zero);
			}
			for (int j = count - 1; j > 0; j--)
			{
				global::UnityEngine.Vector3 a = points[j];
				global::UnityEngine.Vector3 b = points[j + 1];
				points[j * 3] = points[j];
				if (j != count - 1)
				{
					points[j * 3 + 1] = global::UnityEngine.Vector3.Lerp(a, b, 0.33f);
					points[j * 3 + 2] = global::UnityEngine.Vector3.Lerp(a, b, 0.66f);
				}
			}
		}

		public bool ValidateLine(global::Pathfinding.GraphNode n1, global::Pathfinding.GraphNode n2, global::UnityEngine.Vector3 v1, global::UnityEngine.Vector3 v2)
		{
			if (this.useRaycasting)
			{
				if (this.thickRaycast && this.thickRaycastRadius > 0f)
				{
					if (global::UnityEngine.Physics.SphereCast(new global::UnityEngine.Ray(v1 + this.raycastOffset, v2 - v1), this.thickRaycastRadius, (v2 - v1).magnitude, this.mask))
					{
						return false;
					}
				}
				else if (global::UnityEngine.Physics.Linecast(v1 + this.raycastOffset, v2 + this.raycastOffset, this.mask))
				{
					return false;
				}
			}
			if (this.useGraphRaycasting && n1 == null)
			{
				n1 = global::AstarPath.active.GetNearest(v1).node;
				n2 = global::AstarPath.active.GetNearest(v2).node;
			}
			if (this.useGraphRaycasting && n1 != null && n2 != null)
			{
				global::Pathfinding.NavGraph graph = global::Pathfinding.AstarData.GetGraph(n1);
				global::Pathfinding.NavGraph graph2 = global::Pathfinding.AstarData.GetGraph(n2);
				if (graph != graph2)
				{
					return false;
				}
				if (graph != null)
				{
					global::Pathfinding.IRaycastableGraph raycastableGraph = graph as global::Pathfinding.IRaycastableGraph;
					if (raycastableGraph != null)
					{
						return !raycastableGraph.Linecast(v1, v2, n1);
					}
				}
			}
			return true;
		}

		public bool useRaycasting = true;

		public global::UnityEngine.LayerMask mask = -1;

		[global::UnityEngine.Tooltip("Checks around the line between two points, not just the exact line.\nMake sure the ground is either too far below or is not inside the mask since otherwise the raycast might always hit the ground.")]
		public bool thickRaycast;

		[global::UnityEngine.Tooltip("Distance from the ray which will be checked for colliders")]
		public float thickRaycastRadius;

		[global::UnityEngine.Tooltip("Offset from the original positions to perform the raycast.\nCan be useful to avoid the raycast intersecting the ground or similar things you do not want to it intersect")]
		public global::UnityEngine.Vector3 raycastOffset = global::UnityEngine.Vector3.zero;

		[global::UnityEngine.Tooltip("Subdivides the path every iteration to be able to find shorter paths")]
		public bool subdivideEveryIter;

		[global::UnityEngine.Tooltip("How many iterations to try to simplify the path. If the path is changed in one iteration, the next iteration may find more simplification oppourtunities")]
		public int iterations = 2;

		[global::UnityEngine.Tooltip("Use raycasting on the graphs. Only currently works with GridGraph and NavmeshGraph and RecastGraph. This is a pro version feature.")]
		public bool useGraphRaycasting;
	}
}
