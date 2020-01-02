using System;
using UnityEngine;

namespace Pathfinding
{
	public class GraphUpdateShape
	{
		public global::UnityEngine.Vector3[] points
		{
			get
			{
				return this._points;
			}
			set
			{
				this._points = value;
				if (this.convex)
				{
					this.CalculateConvexHull();
				}
			}
		}

		public bool convex
		{
			get
			{
				return this._convex;
			}
			set
			{
				if (this._convex != value && value)
				{
					this._convex = value;
					this.CalculateConvexHull();
				}
				else
				{
					this._convex = value;
				}
			}
		}

		private void CalculateConvexHull()
		{
			if (this.points == null)
			{
				this._convexPoints = null;
				return;
			}
			this._convexPoints = global::Pathfinding.Polygon.ConvexHullXZ(this.points);
			for (int i = 0; i < this._convexPoints.Length; i++)
			{
				global::UnityEngine.Debug.DrawLine(this._convexPoints[i], this._convexPoints[(i + 1) % this._convexPoints.Length], global::UnityEngine.Color.green);
			}
		}

		public global::UnityEngine.Bounds GetBounds()
		{
			if (this.points == null || this.points.Length == 0)
			{
				return default(global::UnityEngine.Bounds);
			}
			global::UnityEngine.Vector3 vector = this.points[0];
			global::UnityEngine.Vector3 vector2 = this.points[0];
			for (int i = 0; i < this.points.Length; i++)
			{
				vector = global::UnityEngine.Vector3.Min(vector, this.points[i]);
				vector2 = global::UnityEngine.Vector3.Max(vector2, this.points[i]);
			}
			return new global::UnityEngine.Bounds((vector + vector2) * 0.5f, vector2 - vector);
		}

		public bool Contains(global::Pathfinding.GraphNode node)
		{
			return this.Contains((global::UnityEngine.Vector3)node.position);
		}

		public bool Contains(global::UnityEngine.Vector3 point)
		{
			if (!this.convex)
			{
				return this._points != null && global::Pathfinding.Polygon.ContainsPointXZ(this._points, point);
			}
			if (this._convexPoints == null)
			{
				return false;
			}
			int i = 0;
			int num = this._convexPoints.Length - 1;
			while (i < this._convexPoints.Length)
			{
				if (global::Pathfinding.VectorMath.RightOrColinearXZ(this._convexPoints[i], this._convexPoints[num], point))
				{
					return false;
				}
				num = i;
				i++;
			}
			return true;
		}

		private global::UnityEngine.Vector3[] _points;

		private global::UnityEngine.Vector3[] _convexPoints;

		private bool _convex;
	}
}
