using System;
using UnityEngine;

namespace Pathfinding
{
	[global::UnityEngine.HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_local_space_rich_a_i.php")]
	public class LocalSpaceRichAI : global::Pathfinding.RichAI
	{
		public override void UpdatePath()
		{
			this.canSearchPath = true;
			this.waitingForPathCalc = false;
			global::Pathfinding.Path currentPath = this.seeker.GetCurrentPath();
			if (currentPath != null && !this.seeker.IsDone())
			{
				currentPath.Error();
				currentPath.Claim(this);
				currentPath.Release(this, false);
			}
			this.waitingForPathCalc = true;
			this.lastRepath = global::UnityEngine.Time.time;
			global::UnityEngine.Matrix4x4 matrix = this.graph.GetMatrix();
			this.seeker.StartPath(matrix.MultiplyPoint3x4(this.tr.position), matrix.MultiplyPoint3x4(this.target.position));
		}

		protected override global::UnityEngine.Vector3 UpdateTarget(global::Pathfinding.RichFunnel fn)
		{
			global::UnityEngine.Matrix4x4 matrix = this.graph.GetMatrix();
			global::UnityEngine.Matrix4x4 inverse = matrix.inverse;
			global::UnityEngine.Debug.DrawRay(matrix.MultiplyPoint3x4(this.tr.position), global::UnityEngine.Vector3.up * 2f, global::UnityEngine.Color.red);
			global::UnityEngine.Debug.DrawRay(inverse.MultiplyPoint3x4(this.tr.position), global::UnityEngine.Vector3.up * 2f, global::UnityEngine.Color.green);
			this.nextCorners.Clear();
			global::UnityEngine.Vector3 vector = this.tr.position;
			global::UnityEngine.Vector3 vector2 = matrix.MultiplyPoint3x4(vector);
			bool flag;
			vector2 = fn.Update(vector2, this.nextCorners, 2, out this.lastCorner, out flag);
			vector = inverse.MultiplyPoint3x4(vector2);
			global::UnityEngine.Debug.DrawRay(vector, global::UnityEngine.Vector3.up * 3f, global::UnityEngine.Color.black);
			for (int i = 0; i < this.nextCorners.Count; i++)
			{
				this.nextCorners[i] = inverse.MultiplyPoint3x4(this.nextCorners[i]);
				global::UnityEngine.Debug.DrawRay(this.nextCorners[i], global::UnityEngine.Vector3.up * 3f, global::UnityEngine.Color.yellow);
			}
			if (flag && !this.waitingForPathCalc)
			{
				this.UpdatePath();
			}
			return vector;
		}

		public global::Pathfinding.LocalSpaceGraph graph;
	}
}
