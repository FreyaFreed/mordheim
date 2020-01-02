using System;
using Flowmap;
using UnityEngine;

[global::UnityEngine.AddComponentMenu("Flowmaps/Fields/Heightmap")]
public class HeightmapField : global::FlowSimulationField
{
	public override global::Flowmap.FieldPass Pass
	{
		get
		{
			return global::Flowmap.FieldPass.Heightmap;
		}
	}

	protected override global::UnityEngine.Shader RenderShader
	{
		get
		{
			return global::UnityEngine.Shader.Find("Hidden/HeightmapFieldPreview");
		}
	}
}
