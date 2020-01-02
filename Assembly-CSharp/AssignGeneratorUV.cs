using System;
using UnityEngine;

[global::UnityEngine.ExecuteInEditMode]
public class AssignGeneratorUV : global::UnityEngine.MonoBehaviour
{
	private void Update()
	{
		for (int i = 0; i < this.renderers.Length; i++)
		{
			if (this.generator)
			{
				this.position = this.generator.Position;
				this.dimensions = this.generator.Dimensions;
			}
			global::UnityEngine.Vector4 zero = global::UnityEngine.Vector4.zero;
			if (this.dimensions.x < this.dimensions.y)
			{
				zero = new global::UnityEngine.Vector4(this.dimensions.x * (this.dimensions.y / this.dimensions.x), this.dimensions.y, this.position.x, this.position.z);
			}
			else
			{
				zero = new global::UnityEngine.Vector4(this.dimensions.x, this.dimensions.y * (this.dimensions.x / this.dimensions.y), this.position.x, this.position.z);
			}
			if (this.assignToSharedMaterial)
			{
				this.renderers[i].sharedMaterial.SetVector(this.uvVectorName, zero);
			}
			else
			{
				this.renderers[i].material.SetVector(this.uvVectorName, zero);
			}
		}
	}

	[global::UnityEngine.SerializeField]
	private global::FlowmapGenerator generator;

	[global::UnityEngine.SerializeField]
	private global::UnityEngine.Vector3 position;

	[global::UnityEngine.SerializeField]
	private global::UnityEngine.Vector2 dimensions;

	[global::UnityEngine.SerializeField]
	private global::UnityEngine.Renderer[] renderers;

	[global::UnityEngine.SerializeField]
	private bool assignToSharedMaterial = true;

	[global::UnityEngine.SerializeField]
	private string uvVectorName = "_FlowmapUV";
}
