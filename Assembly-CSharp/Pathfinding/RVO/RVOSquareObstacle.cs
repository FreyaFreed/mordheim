using System;
using UnityEngine;

namespace Pathfinding.RVO
{
	[global::UnityEngine.HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_r_v_o_1_1_r_v_o_square_obstacle.php")]
	[global::UnityEngine.AddComponentMenu("Pathfinding/Local Avoidance/Square Obstacle")]
	public class RVOSquareObstacle : global::Pathfinding.RVO.RVOObstacle
	{
		protected override bool StaticObstacle
		{
			get
			{
				return false;
			}
		}

		protected override bool ExecuteInEditor
		{
			get
			{
				return true;
			}
		}

		protected override bool LocalCoordinates
		{
			get
			{
				return true;
			}
		}

		protected override float Height
		{
			get
			{
				return this.height;
			}
		}

		protected override bool AreGizmosDirty()
		{
			return false;
		}

		protected override void CreateObstacles()
		{
			this.size.x = global::UnityEngine.Mathf.Abs(this.size.x);
			this.size.y = global::UnityEngine.Mathf.Abs(this.size.y);
			this.height = global::UnityEngine.Mathf.Abs(this.height);
			global::UnityEngine.Vector3[] array = new global::UnityEngine.Vector3[]
			{
				new global::UnityEngine.Vector3(1f, 0f, -1f),
				new global::UnityEngine.Vector3(1f, 0f, 1f),
				new global::UnityEngine.Vector3(-1f, 0f, 1f),
				new global::UnityEngine.Vector3(-1f, 0f, -1f)
			};
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Scale(new global::UnityEngine.Vector3(this.size.x * 0.5f, 0f, this.size.y * 0.5f));
				array[i] += new global::UnityEngine.Vector3(this.center.x, 0f, this.center.y);
			}
			base.AddObstacle(array, this.height);
		}

		public float height = 1f;

		public global::UnityEngine.Vector2 size = global::UnityEngine.Vector3.one;

		public global::UnityEngine.Vector2 center = global::UnityEngine.Vector3.one;
	}
}
