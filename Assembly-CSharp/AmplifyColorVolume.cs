using System;
using UnityEngine;

[global::UnityEngine.AddComponentMenu("Image Effects/Amplify Color Volume")]
[global::UnityEngine.RequireComponent(typeof(global::UnityEngine.BoxCollider))]
public class AmplifyColorVolume : global::AmplifyColorVolumeBase
{
	private void OnTriggerEnter(global::UnityEngine.Collider other)
	{
		global::AmplifyColorTriggerProxy component = other.GetComponent<global::AmplifyColorTriggerProxy>();
		if (component != null && component.OwnerEffect.UseVolumes && (component.OwnerEffect.VolumeCollisionMask & 1 << base.gameObject.layer) != 0)
		{
			component.OwnerEffect.EnterVolume(this);
		}
	}

	private void OnTriggerExit(global::UnityEngine.Collider other)
	{
		global::AmplifyColorTriggerProxy component = other.GetComponent<global::AmplifyColorTriggerProxy>();
		if (component != null && component.OwnerEffect.UseVolumes && (component.OwnerEffect.VolumeCollisionMask & 1 << base.gameObject.layer) != 0)
		{
			component.OwnerEffect.ExitVolume(this);
		}
	}
}
