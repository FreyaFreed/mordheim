using System;

namespace UnityEngine.UI
{
	[global::System.Serializable]
	public struct ScaleBlock
	{
		public static global::UnityEngine.UI.ScaleBlock defaultScaleBlock
		{
			get
			{
				return new global::UnityEngine.UI.ScaleBlock
				{
					normalScale = new global::UnityEngine.Vector2(1f, 1f),
					highlightedScale = new global::UnityEngine.Vector2(1.1f, 1.1f),
					pressedScale = new global::UnityEngine.Vector2(1.1f, 1.1f),
					disabledScale = new global::UnityEngine.Vector2(0.9f, 0.9f),
					duration = 0.1f
				};
			}
		}

		public float duration;

		public global::UnityEngine.Vector2 disabledScale;

		public global::UnityEngine.Vector2 highlightedScale;

		public global::UnityEngine.Vector2 normalScale;

		public global::UnityEngine.Vector2 pressedScale;
	}
}
