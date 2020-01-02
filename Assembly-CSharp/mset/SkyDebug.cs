using System;
using UnityEngine;

namespace mset
{
	public class SkyDebug : global::UnityEngine.MonoBehaviour
	{
		private void Start()
		{
			this.debugID = global::mset.SkyDebug.debugCounter;
			global::mset.SkyDebug.debugCounter++;
		}

		private void LateUpdate()
		{
			bool flag = this.printOnce || this.printConstantly;
			if (base.GetComponent<global::UnityEngine.Renderer>() && flag)
			{
				this.printOnce = false;
				this.debugString = this.GetDebugString();
				if (this.printToConsole)
				{
					global::UnityEngine.Debug.Log(this.debugString);
				}
			}
		}

		public string GetDebugString()
		{
			string text = "<b>SkyDebug Info - " + base.name + "</b>\n";
			global::UnityEngine.Material material;
			if (global::UnityEngine.Application.isPlaying)
			{
				material = base.GetComponent<global::UnityEngine.Renderer>().material;
			}
			else
			{
				material = base.GetComponent<global::UnityEngine.Renderer>().sharedMaterial;
			}
			text = text + material.shader.name + "\n";
			string text2 = text;
			text = string.Concat(new object[]
			{
				text2,
				"is supported: ",
				material.shader.isSupported,
				"\n"
			});
			global::mset.ShaderIDs[] array = new global::mset.ShaderIDs[]
			{
				new global::mset.ShaderIDs(),
				new global::mset.ShaderIDs()
			};
			array[0].Link();
			array[1].Link("1");
			text += "\n<b>Anchor</b>\n";
			global::mset.SkyAnchor component = base.GetComponent<global::mset.SkyAnchor>();
			if (component != null)
			{
				text = text + "Curr. sky: " + component.CurrentSky.name + "\n";
				text = text + "Prev. sky: " + component.PreviousSky.name + "\n";
			}
			else
			{
				text += "none\n";
			}
			text += "\n<b>Property Block</b>\n";
			if (this.block == null)
			{
				this.block = new global::UnityEngine.MaterialPropertyBlock();
			}
			this.block.Clear();
			base.GetComponent<global::UnityEngine.Renderer>().GetPropertyBlock(this.block);
			for (int i = 0; i < 2; i++)
			{
				text = text + "Renderer Property block - blend ID " + i;
				if (this.printDetails)
				{
					text = text + "\nexposureIBL  " + this.block.GetVector(array[i].exposureIBL);
					text = text + "\nexposureLM   " + this.block.GetVector(array[i].exposureLM);
					text = text + "\nskyMin       " + this.block.GetVector(array[i].skyMin);
					text = text + "\nskyMax       " + this.block.GetVector(array[i].skyMax);
					text += "\ndiffuse SH\n";
					for (int j = 0; j < 4; j++)
					{
						text = text + this.block.GetVector(array[i].SH[j]) + "\n";
					}
					text += "...\n";
				}
				global::UnityEngine.Texture texture = this.block.GetTexture(array[i].specCubeIBL);
				global::UnityEngine.Texture texture2 = this.block.GetTexture(array[i].skyCubeIBL);
				text += "\nspecCubeIBL  ";
				if (texture)
				{
					text += texture.name;
				}
				else
				{
					text += "none";
				}
				text += "\nskyCubeIBL   ";
				if (texture2)
				{
					text += texture2.name;
				}
				else
				{
					text += "none";
				}
				if (this.printDetails)
				{
					text = text + "\nskyMatrix\n" + this.block.GetMatrix(array[i].skyMatrix);
					text = text + "\ninvSkyMatrix\n" + this.block.GetMatrix(array[i].invSkyMatrix);
				}
				if (i == 0)
				{
					text = text + "\nblendWeightIBL " + this.block.GetFloat(array[i].blendWeightIBL);
				}
				text += "\n\n";
			}
			return text;
		}

		private void OnDrawGizmosSelected()
		{
			bool flag = this.printOnce || this.printConstantly;
			if (base.GetComponent<global::UnityEngine.Renderer>() && this.printInEditor && this.printToConsole && flag)
			{
				this.printOnce = false;
				string message = this.GetDebugString();
				global::UnityEngine.Debug.Log(message);
			}
		}

		private void OnGUI()
		{
			if (this.printToGUI)
			{
				global::UnityEngine.Rect position = global::UnityEngine.Rect.MinMaxRect(3f, 3f, 360f, 1024f);
				if (global::UnityEngine.Camera.main)
				{
					position.yMax = (float)global::UnityEngine.Camera.main.pixelHeight;
				}
				position.xMin += (float)this.debugID * position.width;
				global::UnityEngine.GUI.color = global::UnityEngine.Color.white;
				if (this.debugStyle == null)
				{
					this.debugStyle = new global::UnityEngine.GUIStyle();
					this.debugStyle.richText = true;
				}
				string str = "<color=\"#000\">";
				string str2 = "</color>";
				global::UnityEngine.GUI.TextArea(position, str + this.debugString + str2, this.debugStyle);
				str = "<color=\"#FFF\">";
				position.xMin -= 1f;
				position.yMin -= 2f;
				global::UnityEngine.GUI.TextArea(position, str + this.debugString + str2, this.debugStyle);
			}
		}

		public bool printConstantly = true;

		public bool printOnce;

		public bool printToGUI = true;

		public bool printToConsole;

		public bool printInEditor = true;

		public bool printDetails;

		public string debugString = string.Empty;

		private global::UnityEngine.MaterialPropertyBlock block;

		private global::UnityEngine.GUIStyle debugStyle;

		private static int debugCounter;

		private int debugID;
	}
}
