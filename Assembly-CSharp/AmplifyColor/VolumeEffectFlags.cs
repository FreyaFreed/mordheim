using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AmplifyColor
{
	[global::System.Serializable]
	public class VolumeEffectFlags
	{
		public VolumeEffectFlags()
		{
			this.components = new global::System.Collections.Generic.List<global::AmplifyColor.VolumeEffectComponentFlags>();
		}

		public void AddComponent(global::UnityEngine.Component c)
		{
			global::AmplifyColor.VolumeEffectComponentFlags volumeEffectComponentFlags;
			if ((volumeEffectComponentFlags = this.components.Find((global::AmplifyColor.VolumeEffectComponentFlags s) => s.componentName == c.GetType() + string.Empty)) != null)
			{
				volumeEffectComponentFlags.UpdateComponentFlags(c);
			}
			else
			{
				this.components.Add(new global::AmplifyColor.VolumeEffectComponentFlags(c));
			}
		}

		public void UpdateFlags(global::AmplifyColor.VolumeEffect effectVol)
		{
			global::AmplifyColor.VolumeEffectComponent comp;
			foreach (global::AmplifyColor.VolumeEffectComponent comp2 in effectVol.components)
			{
				comp = comp2;
				global::AmplifyColor.VolumeEffectComponentFlags volumeEffectComponentFlags;
				if ((volumeEffectComponentFlags = this.components.Find((global::AmplifyColor.VolumeEffectComponentFlags s) => s.componentName == comp.componentName)) == null)
				{
					this.components.Add(new global::AmplifyColor.VolumeEffectComponentFlags(comp));
				}
				else
				{
					volumeEffectComponentFlags.UpdateComponentFlags(comp);
				}
			}
		}

		public static void UpdateCamFlags(global::AmplifyColorBase[] effects, global::AmplifyColorVolumeBase[] volumes)
		{
			foreach (global::AmplifyColorBase amplifyColorBase in effects)
			{
				amplifyColorBase.EffectFlags = new global::AmplifyColor.VolumeEffectFlags();
				foreach (global::AmplifyColorVolumeBase amplifyColorVolumeBase in volumes)
				{
					global::AmplifyColor.VolumeEffect volumeEffect = amplifyColorVolumeBase.EffectContainer.FindVolumeEffect(amplifyColorBase);
					if (volumeEffect != null)
					{
						amplifyColorBase.EffectFlags.UpdateFlags(volumeEffect);
					}
				}
			}
		}

		public global::AmplifyColor.VolumeEffect GenerateEffectData(global::AmplifyColorBase go)
		{
			global::AmplifyColor.VolumeEffect volumeEffect = new global::AmplifyColor.VolumeEffect(go);
			foreach (global::AmplifyColor.VolumeEffectComponentFlags volumeEffectComponentFlags in this.components)
			{
				if (volumeEffectComponentFlags.blendFlag)
				{
					global::UnityEngine.Component component = go.GetComponent(volumeEffectComponentFlags.componentName);
					if (component != null)
					{
						volumeEffect.AddComponent(component, volumeEffectComponentFlags);
					}
				}
			}
			return volumeEffect;
		}

		public global::AmplifyColor.VolumeEffectComponentFlags FindComponentFlags(string compName)
		{
			for (int i = 0; i < this.components.Count; i++)
			{
				if (this.components[i].componentName == compName)
				{
					return this.components[i];
				}
			}
			return null;
		}

		public string[] GetComponentNames()
		{
			return (from r in this.components
			where r.blendFlag
			select r.componentName).ToArray<string>();
		}

		public global::System.Collections.Generic.List<global::AmplifyColor.VolumeEffectComponentFlags> components;
	}
}
