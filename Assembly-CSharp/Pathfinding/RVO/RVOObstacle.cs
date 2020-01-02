using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding.RVO
{
	public abstract class RVOObstacle : global::UnityEngine.MonoBehaviour
	{
		protected abstract void CreateObstacles();

		protected abstract bool ExecuteInEditor { get; }

		protected abstract bool LocalCoordinates { get; }

		protected abstract bool StaticObstacle { get; }

		protected abstract float Height { get; }

		protected abstract bool AreGizmosDirty();

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
			this.gizmoDrawing = true;
			global::UnityEngine.Gizmos.color = new global::UnityEngine.Color(0.615f, 1f, 0.06f, (!selected) ? 0.7f : 1f);
			if (this.gizmoVerts == null || this.AreGizmosDirty() || this._obstacleMode != this.obstacleMode)
			{
				this._obstacleMode = this.obstacleMode;
				if (this.gizmoVerts == null)
				{
					this.gizmoVerts = new global::System.Collections.Generic.List<global::UnityEngine.Vector3[]>();
				}
				else
				{
					this.gizmoVerts.Clear();
				}
				this.CreateObstacles();
			}
			global::UnityEngine.Matrix4x4 matrix = this.GetMatrix();
			for (int i = 0; i < this.gizmoVerts.Count; i++)
			{
				global::UnityEngine.Vector3[] array = this.gizmoVerts[i];
				int j = 0;
				int num = array.Length - 1;
				while (j < array.Length)
				{
					global::UnityEngine.Gizmos.DrawLine(matrix.MultiplyPoint3x4(array[j]), matrix.MultiplyPoint3x4(array[num]));
					num = j++;
				}
				if (selected)
				{
					int k = 0;
					int num2 = array.Length - 1;
					while (k < array.Length)
					{
						global::UnityEngine.Gizmos.DrawLine(matrix.MultiplyPoint3x4(array[k]) + global::UnityEngine.Vector3.up * this.Height, matrix.MultiplyPoint3x4(array[num2]) + global::UnityEngine.Vector3.up * this.Height);
						global::UnityEngine.Gizmos.DrawLine(matrix.MultiplyPoint3x4(array[k]), matrix.MultiplyPoint3x4(array[k]) + global::UnityEngine.Vector3.up * this.Height);
						num2 = k++;
					}
					int l = 0;
					int num3 = array.Length - 1;
					while (l < array.Length)
					{
						global::UnityEngine.Vector3 vector = matrix.MultiplyPoint3x4(array[num3]);
						global::UnityEngine.Vector3 vector2 = matrix.MultiplyPoint3x4(array[l]);
						global::UnityEngine.Vector3 vector3 = (vector + vector2) * 0.5f;
						global::UnityEngine.Vector3 normalized = (vector2 - vector).normalized;
						if (!(normalized == global::UnityEngine.Vector3.zero))
						{
							global::UnityEngine.Vector3 vector4 = global::UnityEngine.Vector3.Cross(global::UnityEngine.Vector3.up, normalized);
							global::UnityEngine.Gizmos.DrawLine(vector3, vector3 + vector4);
							global::UnityEngine.Gizmos.DrawLine(vector3 + vector4, vector3 + vector4 * 0.5f + normalized * 0.5f);
							global::UnityEngine.Gizmos.DrawLine(vector3 + vector4, vector3 + vector4 * 0.5f - normalized * 0.5f);
						}
						num3 = l++;
					}
				}
			}
			this.gizmoDrawing = false;
		}

		protected virtual global::UnityEngine.Matrix4x4 GetMatrix()
		{
			return (!this.LocalCoordinates) ? global::UnityEngine.Matrix4x4.identity : base.transform.localToWorldMatrix;
		}

		public void OnDisable()
		{
			if (this.addedObstacles != null)
			{
				if (this.sim == null)
				{
					throw new global::System.Exception("This should not happen! Make sure you are not overriding the OnEnable function");
				}
				for (int i = 0; i < this.addedObstacles.Count; i++)
				{
					this.sim.RemoveObstacle(this.addedObstacles[i]);
				}
			}
		}

		public void OnEnable()
		{
			if (this.addedObstacles != null)
			{
				if (this.sim == null)
				{
					throw new global::System.Exception("This should not happen! Make sure you are not overriding the OnDisable function");
				}
				for (int i = 0; i < this.addedObstacles.Count; i++)
				{
					global::Pathfinding.RVO.ObstacleVertex obstacleVertex = this.addedObstacles[i];
					global::Pathfinding.RVO.ObstacleVertex obstacleVertex2 = obstacleVertex;
					do
					{
						obstacleVertex.layer = this.layer;
						obstacleVertex = obstacleVertex.next;
					}
					while (obstacleVertex != obstacleVertex2);
					this.sim.AddObstacle(this.addedObstacles[i]);
				}
			}
		}

		public void Start()
		{
			this.addedObstacles = new global::System.Collections.Generic.List<global::Pathfinding.RVO.ObstacleVertex>();
			this.sourceObstacles = new global::System.Collections.Generic.List<global::UnityEngine.Vector3[]>();
			this.prevUpdateMatrix = this.GetMatrix();
			this.CreateObstacles();
		}

		public void Update()
		{
			global::UnityEngine.Matrix4x4 matrix = this.GetMatrix();
			if (matrix != this.prevUpdateMatrix)
			{
				for (int i = 0; i < this.addedObstacles.Count; i++)
				{
					this.sim.UpdateObstacle(this.addedObstacles[i], this.sourceObstacles[i], matrix);
				}
				this.prevUpdateMatrix = matrix;
			}
		}

		protected void FindSimulator()
		{
			global::Pathfinding.RVO.RVOSimulator rvosimulator = global::UnityEngine.Object.FindObjectOfType(typeof(global::Pathfinding.RVO.RVOSimulator)) as global::Pathfinding.RVO.RVOSimulator;
			if (rvosimulator == null)
			{
				throw new global::System.InvalidOperationException("No RVOSimulator could be found in the scene. Please add one to any GameObject");
			}
			this.sim = rvosimulator.GetSimulator();
		}

		protected void AddObstacle(global::UnityEngine.Vector3[] vertices, float height)
		{
			if (vertices == null)
			{
				throw new global::System.ArgumentNullException("Vertices Must Not Be Null");
			}
			if (height < 0f)
			{
				throw new global::System.ArgumentOutOfRangeException("Height must be non-negative");
			}
			if (vertices.Length < 2)
			{
				throw new global::System.ArgumentException("An obstacle must have at least two vertices");
			}
			if (this.gizmoDrawing)
			{
				global::UnityEngine.Vector3[] array = new global::UnityEngine.Vector3[vertices.Length];
				this.WindCorrectly(vertices);
				global::System.Array.Copy(vertices, array, vertices.Length);
				this.gizmoVerts.Add(array);
				return;
			}
			if (this.sim == null)
			{
				this.FindSimulator();
			}
			if (vertices.Length == 2)
			{
				this.AddObstacleInternal(vertices, height);
				return;
			}
			this.WindCorrectly(vertices);
			this.AddObstacleInternal(vertices, height);
		}

		private void AddObstacleInternal(global::UnityEngine.Vector3[] vertices, float height)
		{
			this.addedObstacles.Add(this.sim.AddObstacle(vertices, height, this.GetMatrix(), this.layer, true));
			this.sourceObstacles.Add(vertices);
		}

		private void WindCorrectly(global::UnityEngine.Vector3[] vertices)
		{
			int num = 0;
			float num2 = float.PositiveInfinity;
			for (int i = 0; i < vertices.Length; i++)
			{
				if (vertices[i].x < num2)
				{
					num = i;
					num2 = vertices[i].x;
				}
			}
			if (global::Pathfinding.VectorMath.IsClockwiseXZ(vertices[(num - 1 + vertices.Length) % vertices.Length], vertices[num], vertices[(num + 1) % vertices.Length]))
			{
				if (this.obstacleMode == global::Pathfinding.RVO.RVOObstacle.ObstacleVertexWinding.KeepOut)
				{
					global::System.Array.Reverse(vertices);
				}
			}
			else if (this.obstacleMode == global::Pathfinding.RVO.RVOObstacle.ObstacleVertexWinding.KeepIn)
			{
				global::System.Array.Reverse(vertices);
			}
		}

		public global::Pathfinding.RVO.RVOObstacle.ObstacleVertexWinding obstacleMode;

		public global::Pathfinding.RVO.RVOLayer layer = global::Pathfinding.RVO.RVOLayer.DefaultObstacle;

		protected global::Pathfinding.RVO.Simulator sim;

		private global::System.Collections.Generic.List<global::Pathfinding.RVO.ObstacleVertex> addedObstacles;

		private global::System.Collections.Generic.List<global::UnityEngine.Vector3[]> sourceObstacles;

		private bool gizmoDrawing;

		private global::System.Collections.Generic.List<global::UnityEngine.Vector3[]> gizmoVerts;

		private global::Pathfinding.RVO.RVOObstacle.ObstacleVertexWinding _obstacleMode;

		private global::UnityEngine.Matrix4x4 prevUpdateMatrix;

		public enum ObstacleVertexWinding
		{
			KeepOut,
			KeepIn
		}
	}
}
