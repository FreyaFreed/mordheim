using System;
using UnityEngine;

namespace Pathfinding.Examples
{
	[global::UnityEngine.HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_examples_1_1_astar_smooth_follow2.php")]
	public class AstarSmoothFollow2 : global::UnityEngine.MonoBehaviour
	{
		private void LateUpdate()
		{
			global::UnityEngine.Vector3 b;
			if (this.staticOffset)
			{
				b = this.target.position + new global::UnityEngine.Vector3(0f, this.height, this.distance);
			}
			else if (this.followBehind)
			{
				b = this.target.TransformPoint(0f, this.height, -this.distance);
			}
			else
			{
				b = this.target.TransformPoint(0f, this.height, this.distance);
			}
			base.transform.position = global::UnityEngine.Vector3.Lerp(base.transform.position, b, global::UnityEngine.Time.deltaTime * this.damping);
			if (this.smoothRotation)
			{
				global::UnityEngine.Quaternion b2 = global::UnityEngine.Quaternion.LookRotation(this.target.position - base.transform.position, this.target.up);
				base.transform.rotation = global::UnityEngine.Quaternion.Slerp(base.transform.rotation, b2, global::UnityEngine.Time.deltaTime * this.rotationDamping);
			}
			else
			{
				base.transform.LookAt(this.target, this.target.up);
			}
		}

		public global::UnityEngine.Transform target;

		public float distance = 3f;

		public float height = 3f;

		public float damping = 5f;

		public bool smoothRotation = true;

		public bool followBehind = true;

		public float rotationDamping = 10f;

		public bool staticOffset;
	}
}
