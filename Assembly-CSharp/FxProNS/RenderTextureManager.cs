using System;
using System.Collections.Generic;
using UnityEngine;

namespace FxProNS
{
	public class RenderTextureManager : global::System.IDisposable
	{
		public static global::FxProNS.RenderTextureManager Instance
		{
			get
			{
				global::FxProNS.RenderTextureManager result;
				if ((result = global::FxProNS.RenderTextureManager.instance) == null)
				{
					result = (global::FxProNS.RenderTextureManager.instance = new global::FxProNS.RenderTextureManager());
				}
				return result;
			}
		}

		public global::UnityEngine.RenderTexture RequestRenderTexture(int _width, int _height, int _depth, global::UnityEngine.RenderTextureFormat _format)
		{
			if (this.allRenderTextures == null)
			{
				this.allRenderTextures = new global::System.Collections.Generic.List<global::UnityEngine.RenderTexture>();
			}
			if (this.availableRenderTextures == null)
			{
				this.availableRenderTextures = new global::System.Collections.Generic.List<global::UnityEngine.RenderTexture>();
			}
			global::UnityEngine.RenderTexture renderTexture = null;
			for (int i = 0; i < this.availableRenderTextures.Count; i++)
			{
				global::UnityEngine.RenderTexture renderTexture2 = this.availableRenderTextures[i];
				if (!(null == renderTexture2))
				{
					if (renderTexture2.width == _width && renderTexture2.height == _height && renderTexture2.depth == _depth && renderTexture2.format == _format)
					{
						renderTexture = renderTexture2;
					}
				}
			}
			if (null != renderTexture)
			{
				this.MakeRenderTextureNonAvailable(renderTexture);
				renderTexture.DiscardContents();
				return renderTexture;
			}
			renderTexture = this.CreateNewTexture(_width, _height, _depth, _format);
			this.MakeRenderTextureNonAvailable(renderTexture);
			return renderTexture;
		}

		public global::UnityEngine.RenderTexture ReleaseRenderTexture(global::UnityEngine.RenderTexture _tex)
		{
			if (null == _tex || this.availableRenderTextures == null)
			{
				return null;
			}
			if (this.availableRenderTextures.Contains(_tex))
			{
				return null;
			}
			this.availableRenderTextures.Add(_tex);
			return null;
		}

		public void SafeAssign(ref global::UnityEngine.RenderTexture a, global::UnityEngine.RenderTexture b)
		{
			if (a == b)
			{
				return;
			}
			this.ReleaseRenderTexture(a);
			a = b;
		}

		public void MakeRenderTextureNonAvailable(global::UnityEngine.RenderTexture _tex)
		{
			if (this.availableRenderTextures.Contains(_tex))
			{
				this.availableRenderTextures.Remove(_tex);
			}
		}

		private global::UnityEngine.RenderTexture CreateNewTexture(int _width, int _height, int _depth, global::UnityEngine.RenderTextureFormat _format)
		{
			global::UnityEngine.RenderTexture renderTexture = new global::UnityEngine.RenderTexture(_width, _height, _depth, _format);
			renderTexture.Create();
			this.allRenderTextures.Add(renderTexture);
			this.availableRenderTextures.Add(renderTexture);
			return renderTexture;
		}

		public void PrintRenderTextureStats()
		{
			string text = "<color=blue>availableRenderTextures: </color>" + this.availableRenderTextures.Count + "\n";
			foreach (global::UnityEngine.RenderTexture rt in this.availableRenderTextures)
			{
				text = text + "\t" + this.RenderTexToString(rt) + "\n";
			}
			global::UnityEngine.Debug.Log(text);
			text = "<color=green>allRenderTextures:</color>" + this.allRenderTextures.Count + "\n";
			foreach (global::UnityEngine.RenderTexture rt2 in this.allRenderTextures)
			{
				text = text + "\t" + this.RenderTexToString(rt2) + "\n";
			}
			global::UnityEngine.Debug.Log(text);
		}

		private string RenderTexToString(global::UnityEngine.RenderTexture _rt)
		{
			if (null == _rt)
			{
				return "null";
			}
			return string.Concat(new object[]
			{
				_rt.width,
				" x ",
				_rt.height,
				"\t",
				_rt.depth,
				"\t",
				_rt.format
			});
		}

		private void PrintRenderTexturesCount(string _prefix = "")
		{
			global::UnityEngine.Debug.Log(string.Concat(new object[]
			{
				_prefix,
				": ",
				this.allRenderTextures.Count - this.availableRenderTextures.Count,
				"/",
				this.allRenderTextures.Count
			}));
		}

		public void ReleaseAllRenderTextures()
		{
			if (this.allRenderTextures == null)
			{
				return;
			}
			foreach (global::UnityEngine.RenderTexture renderTexture in this.allRenderTextures)
			{
				if (!this.availableRenderTextures.Contains(renderTexture))
				{
					this.ReleaseRenderTexture(renderTexture);
				}
			}
		}

		public void PrintBalance()
		{
			global::UnityEngine.Debug.Log(string.Concat(new object[]
			{
				"RenderTextures balance: ",
				this.allRenderTextures.Count - this.availableRenderTextures.Count,
				"/",
				this.allRenderTextures.Count
			}));
		}

		public void Dispose()
		{
			if (this.allRenderTextures != null)
			{
				foreach (global::UnityEngine.RenderTexture renderTexture in this.allRenderTextures)
				{
					renderTexture.Release();
				}
				this.allRenderTextures.Clear();
			}
			if (this.availableRenderTextures != null)
			{
				this.availableRenderTextures.Clear();
			}
		}

		private static global::FxProNS.RenderTextureManager instance;

		private global::System.Collections.Generic.List<global::UnityEngine.RenderTexture> allRenderTextures;

		private global::System.Collections.Generic.List<global::UnityEngine.RenderTexture> availableRenderTextures;
	}
}
