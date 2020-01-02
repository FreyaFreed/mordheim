using System;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class DynamicCombatCircle : global::DynamicCircle
{
	public global::UnityEngine.MeshCollider Collider { get; private set; }

	public bool NavCutterEnabled
	{
		get
		{
			return this.navCutter.enabled;
		}
	}

	public override void Init()
	{
		base.Init();
		this.capsuleMinHeight = 0.6f;
		this.sphereRadius = 0.2f;
		this.heightTreshold = 0.5f;
		this.angleIteration = 30;
		this.envHeight = 0.05f;
		this.pointsTreshold = 0.45f;
		this.collisionPointsDistMin = 0.0001f;
		this.displayMesh = new global::UnityEngine.Mesh();
		this.displayMesh.MarkDynamic();
		base.GetComponent<global::UnityEngine.MeshFilter>().mesh = this.displayMesh;
		this.meshRenderer = base.GetComponent<global::UnityEngine.MeshRenderer>();
		this.navCutter = base.GetComponent<global::Pathfinding.NavmeshCut>();
		this.navCutMesh = new global::UnityEngine.Mesh();
		this.navCutMesh.MarkDynamic();
		this.navCutter.mesh = this.navCutMesh;
		this.collisionMesh = new global::UnityEngine.Mesh();
		this.collisionMesh.MarkDynamic();
		this.Collider = base.GetComponentInChildren<global::UnityEngine.MeshCollider>();
		this.Collider.sharedMesh = this.collisionMesh;
	}

	public void Show(bool visible)
	{
		this.meshRenderer.enabled = visible;
	}

	public void SetNavCutterEnabled(bool enabled)
	{
		this.navCutter.enabled = enabled;
		if (enabled)
		{
			this.navCutter.ForceContourRefresh();
			this.navCutter.ForceUpdate();
		}
	}

	public void OverrideNavCutterRadius(float radius)
	{
		base.DetectCollisions(radius, radius, base.transform.parent.rotation, true, 0f, ref this.navCutPoints);
		base.CreateFlatMesh(this.navCutMesh, this.navCutPoints);
	}

	public void SetNavCutter()
	{
		base.CreateFlatMesh(this.navCutMesh, this.displayPoints);
	}

	public void Set(bool isEnemy, bool isEngaged, bool currentUnitIsPlayed, float sizeA, float sizeB, float currentUnitRadius, global::UnityEngine.Quaternion rotation)
	{
		this.Show(currentUnitIsPlayed);
		this.SetMaterial(isEnemy, isEngaged);
		sizeA += 0.02f;
		sizeB += 0.02f;
		base.DetectCollisions(sizeA, sizeB, rotation, true, -0.1f, ref this.displayPoints);
		base.CreateEdges(this.displayPoints, base.transform.position);
		base.CreateCylinderOutlineMesh(this.displayMesh, this.displayPoints, 0f, 0.2f);
		for (int i = 0; i < this.displayPoints.Count; i++)
		{
			global::UnityEngine.Vector3 value = this.displayPoints[i];
			value.y = 0f;
			this.displayPoints[i] = value;
		}
		this.SetNavCutter();
		sizeA -= currentUnitRadius;
		sizeB -= currentUnitRadius;
		base.DetectCollisions(sizeA, sizeB, rotation, false, 0f, ref this.collisionPoints);
		base.CreateCylinderMesh(this.collisionMesh, this.collisionPoints, -0.1f, 1.5f);
		this.Collider.enabled = false;
		this.Collider.enabled = true;
		this.SetNavCutterEnabled(true);
		base.transform.rotation = global::UnityEngine.Quaternion.identity;
	}

	public void SetMaterial(bool isEnemy, bool isEngaged)
	{
		global::UnityEngine.Material sharedMaterial;
		if (isEnemy)
		{
			sharedMaterial = ((!isEngaged) ? this.enemy : this.enemyEngaged);
		}
		else
		{
			sharedMaterial = ((!isEngaged) ? this.friendly : this.friendlyEngaged);
		}
		this.meshRenderer.sharedMaterial = sharedMaterial;
		this.currentColor = this.meshRenderer.sharedMaterial.GetColor("_Color");
	}

	public void SetAlpha(float a)
	{
		this.currentColor.a = a;
		this.meshRenderer.material.SetColor("_Color", this.currentColor);
	}

	public global::UnityEngine.Material friendly;

	public global::UnityEngine.Material friendlyEngaged;

	public global::UnityEngine.Material enemy;

	public global::UnityEngine.Material enemyEngaged;

	private global::System.Collections.Generic.List<global::UnityEngine.Vector3> displayPoints = new global::System.Collections.Generic.List<global::UnityEngine.Vector3>();

	private global::System.Collections.Generic.List<global::UnityEngine.Vector3> collisionPoints = new global::System.Collections.Generic.List<global::UnityEngine.Vector3>();

	private global::System.Collections.Generic.List<global::UnityEngine.Vector3> navCutPoints = new global::System.Collections.Generic.List<global::UnityEngine.Vector3>();

	private global::Pathfinding.NavmeshCut navCutter;

	private global::UnityEngine.MeshRenderer meshRenderer;

	private global::UnityEngine.Mesh displayMesh;

	private global::UnityEngine.Mesh navCutMesh;

	private global::UnityEngine.Mesh collisionMesh;

	private global::UnityEngine.Color currentColor;
}
