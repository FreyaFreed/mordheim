using System;
using AmplifyColor;
using UnityEngine;

[global::UnityEngine.AddComponentMenu("")]
public class AmplifyColorVolumeBase : global::UnityEngine.MonoBehaviour
{
	private void OnDrawGizmos()
	{
		if (this.ShowInSceneView)
		{
			global::UnityEngine.BoxCollider component = base.GetComponent<global::UnityEngine.BoxCollider>();
			global::UnityEngine.BoxCollider2D component2 = base.GetComponent<global::UnityEngine.BoxCollider2D>();
			if (component != null || component2 != null)
			{
				global::UnityEngine.Vector3 center;
				global::UnityEngine.Vector3 size;
				if (component != null)
				{
					center = component.center;
					size = component.size;
				}
				else
				{
					center = component2.offset;
					size = component2.size;
				}
				global::UnityEngine.Gizmos.color = global::UnityEngine.Color.green;
				global::UnityEngine.Gizmos.DrawIcon(base.transform.position, "lut-volume.png", true);
				global::UnityEngine.Gizmos.matrix = base.transform.localToWorldMatrix;
				global::UnityEngine.Gizmos.DrawWireCube(center, size);
			}
		}
	}

	private void OnDrawGizmosSelected()
	{
		global::UnityEngine.BoxCollider component = base.GetComponent<global::UnityEngine.BoxCollider>();
		global::UnityEngine.BoxCollider2D component2 = base.GetComponent<global::UnityEngine.BoxCollider2D>();
		if (component != null || component2 != null)
		{
			global::UnityEngine.Color green = global::UnityEngine.Color.green;
			green.a = 0.2f;
			global::UnityEngine.Gizmos.color = green;
			global::UnityEngine.Gizmos.matrix = base.transform.localToWorldMatrix;
			global::UnityEngine.Vector3 center;
			global::UnityEngine.Vector3 size;
			if (component != null)
			{
				center = component.center;
				size = component.size;
			}
			else
			{
				center = component2.offset;
				size = component2.size;
			}
			global::UnityEngine.Gizmos.DrawCube(center, size);
		}
	}

	public global::UnityEngine.Texture2D LutTexture;

	public float Exposure = 1f;

	public float EnterBlendTime = 1f;

	public int Priority;

	public bool ShowInSceneView = true;

	[global::UnityEngine.HideInInspector]
	public global::AmplifyColor.VolumeEffectContainer EffectContainer = new global::AmplifyColor.VolumeEffectContainer();
}
