using System;
using System.Collections.Generic;
using UnityEngine;

public class LODMeshGroup
{
	public LODMeshGroup()
	{
		this.materialInstances = new global::System.Collections.Generic.Dictionary<global::UnityEngine.Material, global::System.Collections.Generic.List<global::UnityEngine.CombineInstance>>();
	}

	public global::System.Collections.Generic.Dictionary<global::UnityEngine.Material, global::System.Collections.Generic.List<global::UnityEngine.CombineInstance>> materialInstances;
}
