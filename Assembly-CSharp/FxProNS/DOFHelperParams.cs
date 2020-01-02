using System;
using UnityEngine;

namespace FxProNS
{
	[global::System.Serializable]
	public class DOFHelperParams
	{
		public bool UseUnityDepthBuffer = true;

		public bool AutoFocus = true;

		public global::UnityEngine.LayerMask AutoFocusLayerMask = -1;

		[global::UnityEngine.Range(2f, 8f)]
		public float AutoFocusSpeed = 5f;

		[global::UnityEngine.Range(0.01f, 1f)]
		public float FocalLengthMultiplier = 0.33f;

		public float FocalDistMultiplier = 1f;

		[global::UnityEngine.Range(0.5f, 2f)]
		public float DOFBlurSize = 1f;

		public bool BokehEnabled;

		[global::UnityEngine.Range(2f, 8f)]
		public float DepthCompression = 4f;

		public global::UnityEngine.Camera EffectCamera;

		public global::UnityEngine.Transform Target;

		[global::UnityEngine.Range(0f, 1f)]
		public float BokehThreshold = 0.5f;

		[global::UnityEngine.Range(0.5f, 5f)]
		public float BokehGain = 2f;

		[global::UnityEngine.Range(0f, 1f)]
		public float BokehBias = 0.5f;

		public bool DoubleIntensityBlur;
	}
}
