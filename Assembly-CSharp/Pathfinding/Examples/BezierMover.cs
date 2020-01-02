using System;
using UnityEngine;

namespace Pathfinding.Examples
{
	[global::UnityEngine.HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_examples_1_1_bezier_mover.php")]
	public class BezierMover : global::UnityEngine.MonoBehaviour
	{
		private void Update()
		{
			this.Move(true);
		}

		private global::UnityEngine.Vector3 Plot(float t)
		{
			int num = this.points.Length;
			int num2 = global::UnityEngine.Mathf.FloorToInt(t);
			global::UnityEngine.Vector3 normalized = ((this.points[(num2 + 1) % num].position - this.points[num2 % num].position).normalized - (this.points[(num2 - 1 + num) % num].position - this.points[num2 % num].position).normalized).normalized;
			global::UnityEngine.Vector3 normalized2 = ((this.points[(num2 + 2) % num].position - this.points[(num2 + 1) % num].position).normalized - (this.points[(num2 + num) % num].position - this.points[(num2 + 1) % num].position).normalized).normalized;
			global::UnityEngine.Debug.DrawLine(this.points[num2 % num].position, this.points[num2 % num].position + normalized * this.tangentLengths, global::UnityEngine.Color.red);
			global::UnityEngine.Debug.DrawLine(this.points[(num2 + 1) % num].position - normalized2 * this.tangentLengths, this.points[(num2 + 1) % num].position, global::UnityEngine.Color.green);
			return global::Pathfinding.AstarSplines.CubicBezier(this.points[num2 % num].position, this.points[num2 % num].position + normalized * this.tangentLengths, this.points[(num2 + 1) % num].position - normalized2 * this.tangentLengths, this.points[(num2 + 1) % num].position, t - (float)num2);
		}

		private void Move(bool progress)
		{
			float num = this.time;
			float num2 = this.time + 1f;
			while (num2 - num > 0.0001f)
			{
				float num3 = (num + num2) / 2f;
				global::UnityEngine.Vector3 a = this.Plot(num3);
				if ((a - base.transform.position).sqrMagnitude > this.speed * global::UnityEngine.Time.deltaTime * (this.speed * global::UnityEngine.Time.deltaTime))
				{
					num2 = num3;
				}
				else
				{
					num = num3;
				}
			}
			this.time = (num + num2) / 2f;
			global::UnityEngine.Vector3 vector = this.Plot(this.time);
			global::UnityEngine.Vector3 a2 = this.Plot(this.time + 0.001f);
			base.transform.position = vector;
			base.transform.rotation = global::UnityEngine.Quaternion.LookRotation(a2 - vector);
		}

		public void OnDrawGizmos()
		{
			if (this.points.Length >= 3)
			{
				for (int i = 0; i < this.points.Length; i++)
				{
					if (this.points[i] == null)
					{
						return;
					}
				}
				for (int j = 0; j < this.points.Length; j++)
				{
					int num = this.points.Length;
					global::UnityEngine.Vector3 normalized = ((this.points[(j + 1) % num].position - this.points[j].position).normalized - (this.points[(j - 1 + num) % num].position - this.points[j].position).normalized).normalized;
					global::UnityEngine.Vector3 normalized2 = ((this.points[(j + 2) % num].position - this.points[(j + 1) % num].position).normalized - (this.points[(j + num) % num].position - this.points[(j + 1) % num].position).normalized).normalized;
					global::UnityEngine.Vector3 from = this.points[j].position;
					for (int k = 1; k <= 100; k++)
					{
						global::UnityEngine.Vector3 vector = global::Pathfinding.AstarSplines.CubicBezier(this.points[j].position, this.points[j].position + normalized * this.tangentLengths, this.points[(j + 1) % num].position - normalized2 * this.tangentLengths, this.points[(j + 1) % num].position, (float)k / 100f);
						global::UnityEngine.Gizmos.DrawLine(from, vector);
						from = vector;
					}
				}
			}
		}

		public global::UnityEngine.Transform[] points;

		public float tangentLengths = 5f;

		public float speed = 1f;

		private float time;
	}
}
