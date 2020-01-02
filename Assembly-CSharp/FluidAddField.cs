using System;
using Flowmap;
using UnityEngine;

[global::UnityEngine.AddComponentMenu("Flowmaps/Fields/Add fluid")]
public class FluidAddField : global::FlowSimulationField
{
	public override global::Flowmap.FieldPass Pass
	{
		get
		{
			return global::Flowmap.FieldPass.AddFluid;
		}
	}

	protected override global::UnityEngine.Shader RenderShader
	{
		get
		{
			return global::UnityEngine.Shader.Find("Hidden/AddFluidPreview");
		}
	}
}
