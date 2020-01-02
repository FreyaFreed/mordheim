using System;
using UnityEngine;

namespace Pathfinding
{
	[global::UnityEngine.RequireComponent(typeof(global::UnityEngine.Collider))]
	[global::UnityEngine.HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_dynamic_grid_obstacle.php")]
	public class DynamicGridObstacle : global::UnityEngine.MonoBehaviour
	{
		private void Start()
		{
			this.col = base.GetComponent<global::UnityEngine.Collider>();
			this.tr = base.transform;
			if (this.col == null)
			{
				throw new global::System.Exception("A collider must be attached to the GameObject for the DynamicGridObstacle to work");
			}
			this.prevBounds = this.col.bounds;
			this.prevEnabled = this.col.enabled;
			this.prevRotation = this.tr.rotation;
		}

		private void Update()
		{
			if (!this.col)
			{
				global::UnityEngine.Debug.LogError("Removed collider from DynamicGridObstacle", this);
				base.enabled = false;
				return;
			}
			while (global::AstarPath.active == null || global::AstarPath.active.isScanning)
			{
				this.lastCheckTime = global::UnityEngine.Time.realtimeSinceStartup;
			}
			if (global::UnityEngine.Time.realtimeSinceStartup - this.lastCheckTime < this.checkTime)
			{
				return;
			}
			if (this.col.enabled)
			{
				global::UnityEngine.Bounds bounds = this.col.bounds;
				global::UnityEngine.Quaternion rotation = this.tr.rotation;
				global::UnityEngine.Vector3 vector = this.prevBounds.min - bounds.min;
				global::UnityEngine.Vector3 vector2 = this.prevBounds.max - bounds.max;
				float magnitude = bounds.extents.magnitude;
				float num = magnitude * global::UnityEngine.Quaternion.Angle(this.prevRotation, rotation) * 0.0174532924f;
				if (vector.sqrMagnitude > this.updateError * this.updateError || vector2.sqrMagnitude > this.updateError * this.updateError || num > this.updateError || !this.prevEnabled)
				{
					this.DoUpdateGraphs();
				}
			}
			else if (this.prevEnabled)
			{
				this.DoUpdateGraphs();
			}
		}

		private void OnDestroy()
		{
			if (global::AstarPath.active != null)
			{
				global::Pathfinding.GraphUpdateObject ob = new global::Pathfinding.GraphUpdateObject(this.prevBounds);
				global::AstarPath.active.UpdateGraphs(ob);
			}
		}

		public void DoUpdateGraphs()
		{
			if (this.col == null)
			{
				return;
			}
			if (!this.col.enabled)
			{
				global::AstarPath.active.UpdateGraphs(this.prevBounds);
			}
			else
			{
				global::UnityEngine.Bounds bounds = this.col.bounds;
				global::UnityEngine.Bounds bounds2 = bounds;
				bounds2.Encapsulate(this.prevBounds);
				if (global::Pathfinding.DynamicGridObstacle.BoundsVolume(bounds2) < global::Pathfinding.DynamicGridObstacle.BoundsVolume(bounds) + global::Pathfinding.DynamicGridObstacle.BoundsVolume(this.prevBounds))
				{
					global::AstarPath.active.UpdateGraphs(bounds2);
				}
				else
				{
					global::AstarPath.active.UpdateGraphs(this.prevBounds);
					global::AstarPath.active.UpdateGraphs(bounds);
				}
				this.prevBounds = bounds;
			}
			this.prevEnabled = this.col.enabled;
			this.prevRotation = this.tr.rotation;
			this.lastCheckTime = global::UnityEngine.Time.realtimeSinceStartup;
		}

		private static float BoundsVolume(global::UnityEngine.Bounds b)
		{
			return global::System.Math.Abs(b.size.x * b.size.y * b.size.z);
		}

		private global::UnityEngine.Collider col;

		private global::UnityEngine.Transform tr;

		public float updateError = 1f;

		public float checkTime = 0.2f;

		private global::UnityEngine.Bounds prevBounds;

		private global::UnityEngine.Quaternion prevRotation;

		private bool prevEnabled;

		private float lastCheckTime = -9999f;
	}
}
