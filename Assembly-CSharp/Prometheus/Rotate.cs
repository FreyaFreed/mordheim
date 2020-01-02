using System;
using UnityEngine;

namespace Prometheus
{
	[global::UnityEngine.ExecuteInEditMode]
	public class Rotate : global::UnityEngine.MonoBehaviour
	{
		private void Update()
		{
			global::UnityEngine.Vector3 eulerAngles = this.rotPerSec * global::UnityEngine.Time.deltaTime;
			base.transform.Rotate(eulerAngles, (!this.useWorldSpace) ? global::UnityEngine.Space.Self : global::UnityEngine.Space.World);
		}

		public bool useWorldSpace;

		public global::UnityEngine.Vector3 rotPerSec = new global::UnityEngine.Vector3(0f, 360f, 0f);
	}
}
