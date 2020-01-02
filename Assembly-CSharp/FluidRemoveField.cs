using System;
using Flowmap;
using UnityEngine;

[global::UnityEngine.AddComponentMenu("Flowmaps/Fields/Remove fluid")]
public class FluidRemoveField : global::FlowSimulationField
{
	public override global::Flowmap.FieldPass Pass
	{
		get
		{
			return global::Flowmap.FieldPass.RemoveFluid;
		}
	}

	protected override global::UnityEngine.Shader RenderShader
	{
		get
		{
			return global::UnityEngine.Shader.Find("Hidden/RemoveFluidPreview");
		}
	}
}
