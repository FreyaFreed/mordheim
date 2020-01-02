using System;
using UnityEngine;

namespace mset
{
	public class Logo : global::UnityEngine.MonoBehaviour
	{
		private void Reset()
		{
			this.logoTexture = (global::UnityEngine.Resources.Load("renderedLogo") as global::UnityEngine.Texture2D);
		}

		private void Start()
		{
		}

		private void updateTexRect()
		{
			if (this.logoTexture)
			{
				float num = (float)this.logoTexture.width;
				float num2 = (float)this.logoTexture.height;
				float num3 = 0f;
				float num4 = 0f;
				if (base.GetComponent<global::UnityEngine.Camera>())
				{
					num3 = (float)base.GetComponent<global::UnityEngine.Camera>().pixelWidth;
					num4 = (float)base.GetComponent<global::UnityEngine.Camera>().pixelHeight;
				}
				else if (global::UnityEngine.Camera.main)
				{
					num3 = (float)global::UnityEngine.Camera.main.pixelWidth;
					num4 = (float)global::UnityEngine.Camera.main.pixelHeight;
				}
				else if (global::UnityEngine.Camera.current)
				{
				}
				float num5 = this.logoPixelOffset.x + this.logoPercentOffset.x * num3 * 0.01f;
				float num6 = this.logoPixelOffset.y + this.logoPercentOffset.y * num4 * 0.01f;
				switch (this.placement)
				{
				case global::mset.Corner.TopLeft:
					this.texRect.x = num5;
					this.texRect.y = num6;
					break;
				case global::mset.Corner.TopRight:
					this.texRect.x = num3 - num5 - num;
					this.texRect.y = num6;
					break;
				case global::mset.Corner.BottomLeft:
					this.texRect.x = num5;
					this.texRect.y = num4 - num6 - num2;
					break;
				case global::mset.Corner.BottomRight:
					this.texRect.x = num3 - num5 - num;
					this.texRect.y = num4 - num6 - num2;
					break;
				}
				this.texRect.width = num;
				this.texRect.height = num2;
			}
		}

		private void OnGUI()
		{
			this.updateTexRect();
			if (this.logoTexture)
			{
				global::UnityEngine.GUI.color = this.color;
				global::UnityEngine.GUI.DrawTexture(this.texRect, this.logoTexture);
			}
		}

		public global::UnityEngine.Texture2D logoTexture;

		public global::UnityEngine.Color color = global::UnityEngine.Color.white;

		public global::UnityEngine.Vector2 logoPixelOffset = new global::UnityEngine.Vector2(0f, 0f);

		public global::UnityEngine.Vector2 logoPercentOffset = new global::UnityEngine.Vector2(0f, 0f);

		public global::mset.Corner placement = global::mset.Corner.BottomLeft;

		private global::UnityEngine.Rect texRect = new global::UnityEngine.Rect(0f, 0f, 0f, 0f);
	}
}
