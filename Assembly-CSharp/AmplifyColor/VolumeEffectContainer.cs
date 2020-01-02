using System;
using System.Collections.Generic;
using System.Linq;

namespace AmplifyColor
{
	[global::System.Serializable]
	public class VolumeEffectContainer
	{
		public VolumeEffectContainer()
		{
			this.volumes = new global::System.Collections.Generic.List<global::AmplifyColor.VolumeEffect>();
		}

		public void AddColorEffect(global::AmplifyColorBase colorEffect)
		{
			global::AmplifyColor.VolumeEffect volumeEffect;
			if ((volumeEffect = this.FindVolumeEffect(colorEffect)) != null)
			{
				volumeEffect.UpdateVolume();
			}
			else
			{
				volumeEffect = new global::AmplifyColor.VolumeEffect(colorEffect);
				this.volumes.Add(volumeEffect);
				volumeEffect.UpdateVolume();
			}
		}

		public global::AmplifyColor.VolumeEffect AddJustColorEffect(global::AmplifyColorBase colorEffect)
		{
			global::AmplifyColor.VolumeEffect volumeEffect = new global::AmplifyColor.VolumeEffect(colorEffect);
			this.volumes.Add(volumeEffect);
			return volumeEffect;
		}

		public global::AmplifyColor.VolumeEffect FindVolumeEffect(global::AmplifyColorBase colorEffect)
		{
			for (int i = 0; i < this.volumes.Count; i++)
			{
				if (this.volumes[i].gameObject == colorEffect)
				{
					return this.volumes[i];
				}
			}
			for (int j = 0; j < this.volumes.Count; j++)
			{
				if (this.volumes[j].gameObject != null && this.volumes[j].gameObject.SharedInstanceID == colorEffect.SharedInstanceID)
				{
					return this.volumes[j];
				}
			}
			return null;
		}

		public void RemoveVolumeEffect(global::AmplifyColor.VolumeEffect volume)
		{
			this.volumes.Remove(volume);
		}

		public global::AmplifyColorBase[] GetStoredEffects()
		{
			return (from r in this.volumes
			select r.gameObject).ToArray<global::AmplifyColorBase>();
		}

		public global::System.Collections.Generic.List<global::AmplifyColor.VolumeEffect> volumes;
	}
}
