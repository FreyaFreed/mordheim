using System;
using UnityEngine;

namespace WellFired
{
	[global::UnityEngine.ExecuteInEditMode]
	public class AmbientLightAdjuster : global::UnityEngine.MonoBehaviour
	{
		private void Update()
		{
			global::UnityEngine.RenderSettings.ambientLight = this.ambientLightColor;
		}

		public global::UnityEngine.Color ambientLightColor = global::UnityEngine.Color.red;
	}
}
