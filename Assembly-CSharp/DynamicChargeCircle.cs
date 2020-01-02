using System;
using System.Collections.Generic;
using UnityEngine;

public class DynamicChargeCircle : global::DynamicCircle
{
	public override void Init()
	{
		base.Init();
		this.capsuleMinHeight = 0.6f;
		this.heightTreshold = 0.5f;
		this.angleIteration = 60;
		this.envHeight = 0.5f;
		this.pointsTreshold = 0.45f;
		this.collisionPointsDistMin = 0.0001f;
		this.cylinderMesh = new global::UnityEngine.Mesh();
		base.GetComponent<global::UnityEngine.MeshFilter>().mesh = this.cylinderMesh;
	}

	public void Set(float chargeDist, float radius)
	{
		this.sphereRadius = radius;
		this.capsuleMinHeight = radius + 0.4f;
		base.DetectCollisions(chargeDist, chargeDist, base.transform.parent.transform.rotation, true, -0.2f, ref this.displayPoints);
		base.CreateCylinderOutlineMesh(this.cylinderMesh, this.displayPoints, -0.1f, 1.5f);
	}

	public bool fitToEnv = true;

	private global::System.Collections.Generic.List<global::UnityEngine.Vector3> displayPoints = new global::System.Collections.Generic.List<global::UnityEngine.Vector3>();

	private global::UnityEngine.Mesh cylinderMesh;
}
