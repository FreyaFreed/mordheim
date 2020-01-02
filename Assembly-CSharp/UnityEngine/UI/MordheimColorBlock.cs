using System;

namespace UnityEngine.UI
{
	[global::System.Serializable]
	public struct MordheimColorBlock
	{
		public static global::UnityEngine.UI.ColorBlock defaultColorBlock
		{
			get
			{
				return new global::UnityEngine.UI.ColorBlock
				{
					normalColor = new global::UnityEngine.Color(0.6f, 0.6f, 0.6f, 1f),
					highlightedColor = new global::UnityEngine.Color(0.35f, 1f, 0.58f, 1f),
					pressedColor = new global::UnityEngine.Color(0.35f, 1f, 0.58f, 1f),
					disabledColor = new global::UnityEngine.Color(0.25f, 0.25f, 0.25f, 0.5f),
					fadeDuration = 0.1f
				};
			}
		}
	}
}
