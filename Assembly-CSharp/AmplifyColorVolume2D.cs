using System;
using UnityEngine;

[global::UnityEngine.RequireComponent(typeof(global::UnityEngine.BoxCollider2D))]
[global::UnityEngine.AddComponentMenu("Image Effects/Amplify Color Volume 2D")]
public class AmplifyColorVolume2D : global::AmplifyColorVolumeBase
{
	private void OnTriggerEnter2D(global::UnityEngine.Collider2D other)
	{
		global::AmplifyColorTriggerProxy2D component = other.GetComponent<global::AmplifyColorTriggerProxy2D>();
		if (component != null && component.OwnerEffect.UseVolumes && (component.OwnerEffect.VolumeCollisionMask & 1 << base.gameObject.layer) != 0)
		{
			component.OwnerEffect.EnterVolume(this);
		}
	}

	private void OnTriggerExit2D(global::UnityEngine.Collider2D other)
	{
		global::AmplifyColorTriggerProxy2D component = other.GetComponent<global::AmplifyColorTriggerProxy2D>();
		if (component != null && component.OwnerEffect.UseVolumes && (component.OwnerEffect.VolumeCollisionMask & 1 << base.gameObject.layer) != 0)
		{
			component.OwnerEffect.ExitVolume(this);
		}
	}
}
