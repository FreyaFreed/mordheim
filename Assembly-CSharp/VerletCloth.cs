using System;
using UnityEngine;

public class VerletCloth : global::UnityEngine.MonoBehaviour
{
	private void Start()
	{
		this.wasSequence = false;
		this.tMesh = new global::UnityEngine.Mesh();
		if (base.transform.parent != null && base.transform.parent.parent != null)
		{
			this.rootBone = base.transform.parent.parent;
			this.prevBonePos = this.rootBone.localPosition;
		}
		if (base.transform.parent != null && base.transform.parent.parent != null && base.transform.parent.parent.parent != null)
		{
			this.colliderRoot = base.transform.parent.parent.parent.gameObject;
			this.prevRootPos = this.colliderRoot.transform.position;
			this.bounds = default(global::UnityEngine.Bounds);
		}
		global::UnityEngine.MeshFilter meshFilter = base.gameObject.GetComponent<global::UnityEngine.MeshFilter>();
		if (meshFilter != null)
		{
			this.mesh = meshFilter.mesh;
		}
		else
		{
			global::UnityEngine.SkinnedMeshRenderer component = base.gameObject.GetComponent<global::UnityEngine.SkinnedMeshRenderer>();
			component.BakeMesh(this.tMesh);
			meshFilter = base.gameObject.AddComponent<global::UnityEngine.MeshFilter>();
			meshFilter.mesh = this.tMesh;
			this.mesh = this.tMesh;
			global::UnityEngine.MeshRenderer meshRenderer = base.gameObject.AddComponent<global::UnityEngine.MeshRenderer>();
			meshRenderer.sharedMaterial = component.sharedMaterial;
			global::UnityEngine.Object.Destroy(component);
			for (int i = 0; i < this.knots.Length; i++)
			{
				this.knots[i].curPos = this.tMesh.vertices[this.knots[i].meshVertIdx];
				this.knots[i].oldPos = this.knots[i].curPos;
				this.knots[i].orgPos = this.knots[i].curPos;
			}
			for (int j = 0; j < this.locks.Length; j++)
			{
				this.locks[j].orgPos = this.tMesh.vertices[this.knots[this.locks[j].knot].meshVertIdx];
			}
		}
		this.mesh.MarkDynamic();
		this.colliders = this.colliderRoot.GetComponentsInChildren<global::ArachneCollider>(true);
		this.prevCollidersPos = new global::UnityEngine.Vector3[this.colliders.Length];
		global::UnityEngine.Debug.Log("Colliders " + this.colliders.Length);
		global::UnityEngine.Renderer[] componentsInChildren = base.GetComponentsInChildren<global::UnityEngine.Renderer>();
		foreach (global::UnityEngine.Renderer renderer in componentsInChildren)
		{
			renderer.material.color = renderer.sharedMaterial.color;
		}
	}

	private void OnBecameVisible()
	{
		base.enabled = true;
	}

	private void OnBecameInvisible()
	{
		base.enabled = false;
	}

	private void Reset()
	{
		for (int i = 0; i < this.knots.Length; i++)
		{
			this.knots[i].curPos = this.knots[i].orgPos;
			this.knots[i].oldPos = this.knots[i].orgPos;
		}
	}

	private void FixedUpdate()
	{
		if (global::PandoraSingleton<global::SequenceManager>.Exists())
		{
			if (this.wasSequence && !global::PandoraSingleton<global::SequenceManager>.Instance.isPlaying)
			{
				this.Reset();
				this.wasSequence = false;
			}
			else if (global::PandoraSingleton<global::SequenceManager>.Instance.isPlaying)
			{
				this.wasSequence = true;
			}
		}
		global::UnityEngine.Vector3 localPosition = this.rootBone.localPosition;
		global::UnityEngine.Vector3 position = this.colliderRoot.transform.position;
		global::UnityEngine.Vector3 b = position - this.prevRootPos;
		global::UnityEngine.Vector3 a = this.rootBone.InverseTransformDirection(b.normalized);
		this.prevRootPos = position;
		this.prevBonePos = localPosition;
		global::UnityEngine.Vector3 a2 = this.rootBone.InverseTransformDirection(global::UnityEngine.Physics.gravity);
		for (int i = 0; i < this.knots.Length; i++)
		{
			global::UnityEngine.Vector3 oldPos = this.knots[i].oldPos;
			global::UnityEngine.Vector3 vector = this.knots[i].curPos;
			if (this.knots[i].worldMove && !this.wasSequence)
			{
				vector = vector * 2f - oldPos - a / this.moveDiv + a2 / this.gravityDiv * global::UnityEngine.Time.deltaTime * global::UnityEngine.Time.deltaTime;
			}
			else
			{
				vector = vector * 2f - oldPos + a / (this.moveDiv * 2f) + a2 / this.gravityDiv * global::UnityEngine.Time.deltaTime * global::UnityEngine.Time.deltaTime;
			}
			this.knots[i].oldPos = this.knots[i].curPos;
			this.knots[i].curPos = vector;
		}
		for (int j = 0; j < this.locks.Length; j++)
		{
			this.knots[this.locks[j].knot].curPos = this.locks[j].orgPos;
		}
		for (int k = 0; k < 1; k++)
		{
			for (int l = 0; l < this.threads.Length; l++)
			{
				global::UnityEngine.Vector3 vector2 = this.knots[this.threads[l].knot1].curPos;
				global::UnityEngine.Vector3 vector3 = this.knots[this.threads[l].knot2].curPos;
				global::UnityEngine.Vector3 a3 = vector3 - vector2;
				float num = global::UnityEngine.Vector3.Distance(vector3, vector2);
				float d = (num - this.threads[l].restLength) / num;
				vector2 += a3 * 0.5f * d;
				vector3 -= a3 * 0.5f * d;
				this.knots[this.threads[l].knot1].curPos = vector2;
				this.knots[this.threads[l].knot2].curPos = vector3;
			}
			for (int m = 0; m < this.locks.Length; m++)
			{
				this.knots[this.locks[m].knot].curPos = this.locks[m].orgPos;
			}
		}
		for (int n = 0; n < this.colliders.Length; n++)
		{
			float radius = this.colliders[n].radius;
			float num2 = radius * radius;
			global::UnityEngine.Vector3 position2 = this.colliders[n].position;
			for (int num3 = 0; num3 < this.knots.Length; num3++)
			{
				global::UnityEngine.Vector3 a4 = base.transform.TransformPoint(this.knots[num3].curPos) - position2;
				float num4 = global::UnityEngine.Vector3.SqrMagnitude(a4);
				if (num2 > num4)
				{
					global::UnityEngine.Vector3 b2 = position2 - this.prevCollidersPos[n] - b;
					if (!this.wasSequence && !this.knots[num3].worldMove)
					{
						if (b2.y < 0f)
						{
							b2.y = 0f;
						}
						a4 += b2;
					}
					global::UnityEngine.Vector3 position3 = position2 + a4.normalized * radius;
					this.knots[num3].curPos = base.transform.InverseTransformPoint(position3);
					this.knots[num3].oldPos = this.knots[num3].curPos;
				}
			}
			this.prevCollidersPos[n] = position2;
		}
		for (int num5 = 0; num5 < this.locks.Length; num5++)
		{
			this.knots[this.locks[num5].knot].curPos = this.locks[num5].orgPos;
		}
		global::UnityEngine.Vector3[] vertices = this.mesh.vertices;
		for (int num6 = 0; num6 < this.knots.Length; num6++)
		{
			vertices[this.knots[num6].meshVertIdx] = this.knots[num6].curPos;
		}
		this.mesh.vertices = vertices;
		this.mesh.RecalculateNormals();
	}

	public global::ClothKnot[] knots;

	public global::ClothThread[] threads;

	public global::ClothLock[] locks;

	public global::UnityEngine.Mesh mesh;

	public global::UnityEngine.GameObject colliderRoot;

	public float gravityDiv = 10f;

	public float moveDiv = 100f;

	private global::ArachneCollider[] colliders;

	private global::UnityEngine.Vector3[] prevCollidersPos;

	private global::UnityEngine.Mesh tMesh;

	public global::UnityEngine.Transform rootBone;

	public global::UnityEngine.Vector3 prevBonePos;

	public global::UnityEngine.Vector3 prevRootPos;

	private global::UnityEngine.Bounds bounds;

	private bool wasSequence;
}
