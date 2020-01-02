using System;
using UnityEngine;

namespace Prometheus
{
	public class Translate : global::UnityEngine.MonoBehaviour
	{
		private void Update()
		{
			global::UnityEngine.Vector3 b = this.transPerSec * ((!this.smooth) ? global::UnityEngine.Time.deltaTime : global::UnityEngine.Time.smoothDeltaTime);
			base.transform.localPosition += b;
		}

		public global::UnityEngine.Vector3 transPerSec = new global::UnityEngine.Vector3(0f, 0.1f, 0f);

		public bool smooth = true;
	}
}
