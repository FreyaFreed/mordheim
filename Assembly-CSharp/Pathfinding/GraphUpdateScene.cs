using System;
using UnityEngine;

namespace Pathfinding
{
	[global::UnityEngine.HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_graph_update_scene.php")]
	[global::UnityEngine.AddComponentMenu("Pathfinding/GraphUpdateScene")]
	public class GraphUpdateScene : global::Pathfinding.GraphModifier
	{
		public void Start()
		{
			if (!this.firstApplied && this.applyOnStart)
			{
				this.Apply();
			}
		}

		public override void OnPostScan()
		{
			if (this.applyOnScan)
			{
				this.Apply();
			}
		}

		public virtual void InvertSettings()
		{
			this.setWalkability = !this.setWalkability;
			this.penaltyDelta = -this.penaltyDelta;
			if (this.setTagInvert == 0)
			{
				this.setTagInvert = this.setTag;
				this.setTag = 0;
			}
			else
			{
				this.setTag = this.setTagInvert;
				this.setTagInvert = 0;
			}
		}

		public void RecalcConvex()
		{
			this.convexPoints = ((!this.convex) ? null : global::Pathfinding.Polygon.ConvexHullXZ(this.points));
		}

		public void ToggleUseWorldSpace()
		{
			this.useWorldSpace = !this.useWorldSpace;
			if (this.points == null)
			{
				return;
			}
			this.convexPoints = null;
			global::UnityEngine.Matrix4x4 matrix4x = (!this.useWorldSpace) ? base.transform.worldToLocalMatrix : base.transform.localToWorldMatrix;
			for (int i = 0; i < this.points.Length; i++)
			{
				this.points[i] = matrix4x.MultiplyPoint3x4(this.points[i]);
			}
		}

		public void LockToY()
		{
			if (this.points == null)
			{
				return;
			}
			for (int i = 0; i < this.points.Length; i++)
			{
				this.points[i].y = this.lockToYValue;
			}
		}

		public void Apply(global::AstarPath active)
		{
			if (this.applyOnScan)
			{
				this.Apply();
			}
		}

		public global::UnityEngine.Bounds GetBounds()
		{
			global::UnityEngine.Bounds bounds;
			if (this.points == null || this.points.Length == 0)
			{
				global::UnityEngine.Collider component = base.GetComponent<global::UnityEngine.Collider>();
				global::UnityEngine.Renderer component2 = base.GetComponent<global::UnityEngine.Renderer>();
				if (component != null)
				{
					bounds = component.bounds;
				}
				else
				{
					if (!(component2 != null))
					{
						return new global::UnityEngine.Bounds(global::UnityEngine.Vector3.zero, global::UnityEngine.Vector3.zero);
					}
					bounds = component2.bounds;
				}
			}
			else
			{
				global::UnityEngine.Matrix4x4 matrix4x = global::UnityEngine.Matrix4x4.identity;
				if (!this.useWorldSpace)
				{
					matrix4x = base.transform.localToWorldMatrix;
				}
				global::UnityEngine.Vector3 vector = matrix4x.MultiplyPoint3x4(this.points[0]);
				global::UnityEngine.Vector3 vector2 = matrix4x.MultiplyPoint3x4(this.points[0]);
				for (int i = 0; i < this.points.Length; i++)
				{
					global::UnityEngine.Vector3 rhs = matrix4x.MultiplyPoint3x4(this.points[i]);
					vector = global::UnityEngine.Vector3.Min(vector, rhs);
					vector2 = global::UnityEngine.Vector3.Max(vector2, rhs);
				}
				bounds = new global::UnityEngine.Bounds((vector + vector2) * 0.5f, vector2 - vector);
			}
			if (bounds.size.y < this.minBoundsHeight)
			{
				bounds.size = new global::UnityEngine.Vector3(bounds.size.x, this.minBoundsHeight, bounds.size.z);
			}
			return bounds;
		}

		public void Apply()
		{
			if (global::AstarPath.active == null)
			{
				global::UnityEngine.Debug.LogError("There is no AstarPath object in the scene");
				return;
			}
			global::Pathfinding.GraphUpdateObject graphUpdateObject;
			if (this.points == null || this.points.Length == 0)
			{
				global::UnityEngine.Collider component = base.GetComponent<global::UnityEngine.Collider>();
				global::UnityEngine.Renderer component2 = base.GetComponent<global::UnityEngine.Renderer>();
				global::UnityEngine.Bounds bounds;
				if (component != null)
				{
					bounds = component.bounds;
				}
				else
				{
					if (!(component2 != null))
					{
						global::UnityEngine.Debug.LogWarning("Cannot apply GraphUpdateScene, no points defined and no renderer or collider attached");
						return;
					}
					bounds = component2.bounds;
				}
				if (bounds.size.y < this.minBoundsHeight)
				{
					bounds.size = new global::UnityEngine.Vector3(bounds.size.x, this.minBoundsHeight, bounds.size.z);
				}
				graphUpdateObject = new global::Pathfinding.GraphUpdateObject(bounds);
			}
			else
			{
				global::Pathfinding.GraphUpdateShape graphUpdateShape = new global::Pathfinding.GraphUpdateShape();
				graphUpdateShape.convex = this.convex;
				global::UnityEngine.Vector3[] array = this.points;
				if (!this.useWorldSpace)
				{
					array = new global::UnityEngine.Vector3[this.points.Length];
					global::UnityEngine.Matrix4x4 localToWorldMatrix = base.transform.localToWorldMatrix;
					for (int i = 0; i < array.Length; i++)
					{
						array[i] = localToWorldMatrix.MultiplyPoint3x4(this.points[i]);
					}
				}
				graphUpdateShape.points = array;
				global::UnityEngine.Bounds bounds2 = graphUpdateShape.GetBounds();
				if (bounds2.size.y < this.minBoundsHeight)
				{
					bounds2.size = new global::UnityEngine.Vector3(bounds2.size.x, this.minBoundsHeight, bounds2.size.z);
				}
				graphUpdateObject = new global::Pathfinding.GraphUpdateObject(bounds2);
				graphUpdateObject.shape = graphUpdateShape;
			}
			this.firstApplied = true;
			graphUpdateObject.modifyWalkability = this.modifyWalkability;
			graphUpdateObject.setWalkability = this.setWalkability;
			graphUpdateObject.addPenalty = this.penaltyDelta;
			graphUpdateObject.updatePhysics = this.updatePhysics;
			graphUpdateObject.updateErosion = this.updateErosion;
			graphUpdateObject.resetPenaltyOnPhysics = this.resetPenaltyOnPhysics;
			graphUpdateObject.modifyTag = this.modifyTag;
			graphUpdateObject.setTag = this.setTag;
			global::AstarPath.active.UpdateGraphs(graphUpdateObject);
		}

		public void OnDrawGizmos()
		{
			this.OnDrawGizmos(false);
		}

		public void OnDrawGizmosSelected()
		{
			this.OnDrawGizmos(true);
		}

		public void OnDrawGizmos(bool selected)
		{
			global::UnityEngine.Color color = (!selected) ? new global::UnityEngine.Color(0.8901961f, 0.239215687f, 0.08627451f, 0.9f) : new global::UnityEngine.Color(0.8901961f, 0.239215687f, 0.08627451f, 1f);
			if (selected)
			{
				global::UnityEngine.Gizmos.color = global::UnityEngine.Color.Lerp(color, new global::UnityEngine.Color(1f, 1f, 1f, 0.2f), 0.9f);
				global::UnityEngine.Bounds bounds = this.GetBounds();
				global::UnityEngine.Gizmos.DrawCube(bounds.center, bounds.size);
				global::UnityEngine.Gizmos.DrawWireCube(bounds.center, bounds.size);
			}
			if (this.points == null)
			{
				return;
			}
			if (this.convex)
			{
				color.a *= 0.5f;
			}
			global::UnityEngine.Gizmos.color = color;
			global::UnityEngine.Matrix4x4 matrix4x = (!this.useWorldSpace) ? base.transform.localToWorldMatrix : global::UnityEngine.Matrix4x4.identity;
			if (this.convex)
			{
				color.r -= 0.1f;
				color.g -= 0.2f;
				color.b -= 0.1f;
				global::UnityEngine.Gizmos.color = color;
			}
			if (selected || !this.convex)
			{
				for (int i = 0; i < this.points.Length; i++)
				{
					global::UnityEngine.Gizmos.DrawLine(matrix4x.MultiplyPoint3x4(this.points[i]), matrix4x.MultiplyPoint3x4(this.points[(i + 1) % this.points.Length]));
				}
			}
			if (this.convex)
			{
				if (this.convexPoints == null)
				{
					this.RecalcConvex();
				}
				global::UnityEngine.Gizmos.color = ((!selected) ? new global::UnityEngine.Color(0.8901961f, 0.239215687f, 0.08627451f, 0.9f) : new global::UnityEngine.Color(0.8901961f, 0.239215687f, 0.08627451f, 1f));
				for (int j = 0; j < this.convexPoints.Length; j++)
				{
					global::UnityEngine.Gizmos.DrawLine(matrix4x.MultiplyPoint3x4(this.convexPoints[j]), matrix4x.MultiplyPoint3x4(this.convexPoints[(j + 1) % this.convexPoints.Length]));
				}
			}
		}

		public global::UnityEngine.Vector3[] points;

		private global::UnityEngine.Vector3[] convexPoints;

		[global::UnityEngine.HideInInspector]
		public bool convex = true;

		[global::UnityEngine.HideInInspector]
		public float minBoundsHeight = 1f;

		[global::UnityEngine.HideInInspector]
		public int penaltyDelta;

		[global::UnityEngine.HideInInspector]
		public bool modifyWalkability;

		[global::UnityEngine.HideInInspector]
		public bool setWalkability;

		[global::UnityEngine.HideInInspector]
		public bool applyOnStart = true;

		[global::UnityEngine.HideInInspector]
		public bool applyOnScan = true;

		[global::UnityEngine.HideInInspector]
		public bool useWorldSpace;

		[global::UnityEngine.HideInInspector]
		public bool updatePhysics;

		[global::UnityEngine.HideInInspector]
		public bool resetPenaltyOnPhysics = true;

		[global::UnityEngine.HideInInspector]
		public bool updateErosion = true;

		[global::UnityEngine.HideInInspector]
		public bool lockToY;

		[global::UnityEngine.HideInInspector]
		public float lockToYValue;

		[global::UnityEngine.HideInInspector]
		public bool modifyTag;

		[global::UnityEngine.HideInInspector]
		public int setTag;

		private int setTagInvert;

		private bool firstApplied;
	}
}
