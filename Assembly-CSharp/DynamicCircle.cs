using System;
using System.Collections.Generic;
using UnityEngine;

public class DynamicCircle : global::UnityEngine.MonoBehaviour
{
	[global::UnityEngine.HideInInspector]
	public global::System.Collections.Generic.List<global::Tuple<global::UnityEngine.Vector2, global::UnityEngine.Vector2>> Edges { get; private set; }

	public virtual void Init()
	{
		this.Edges = new global::System.Collections.Generic.List<global::Tuple<global::UnityEngine.Vector2, global::UnityEngine.Vector2>>();
	}

	private void FixedUpdate()
	{
		base.transform.rotation = global::UnityEngine.Quaternion.identity;
	}

	protected void DetectCollisions(float radiusA, float radiusB, global::UnityEngine.Quaternion rotation, bool flatEnv, float flatOffset, ref global::System.Collections.Generic.List<global::UnityEngine.Vector3> points)
	{
		this.circleCenter = base.transform.position;
		points.Clear();
		int i = 0;
		while (i < this.angleIteration)
		{
			float num = (float)i * (360f / (float)(this.angleIteration + 1));
			global::UnityEngine.Vector3 vector;
			float num3;
			if (radiusA != radiusB)
			{
				float f = num * 0.0174532924f * -1f;
				float num2 = radiusA * radiusB / global::UnityEngine.Mathf.Sqrt(global::UnityEngine.Mathf.Pow(radiusB, 2f) + global::UnityEngine.Mathf.Pow(radiusA, 2f) * global::UnityEngine.Mathf.Pow(global::UnityEngine.Mathf.Tan(f), 2f));
				num2 *= (((0f > num || num >= 90f) && (270f >= num || num > 360f)) ? 1f : -1f);
				float z = global::UnityEngine.Mathf.Tan(f) * num2;
				vector = new global::UnityEngine.Vector3(num2, 0f, z);
				vector = rotation * -vector;
				num3 = vector.magnitude;
				vector /= num3;
				num3 -= this.sphereRadius;
			}
			else
			{
				vector = global::UnityEngine.Vector3.forward;
				vector = global::UnityEngine.Quaternion.Euler(0f, (float)i * (360f / (float)this.angleIteration), 0f) * vector;
				vector.Normalize();
				num3 = radiusA - this.sphereRadius;
			}
			global::UnityEngine.Vector3 vector2 = this.GetPoint(this.circleCenter, vector, num3);
			if (points.Count <= 0)
			{
				goto IL_238;
			}
			global::UnityEngine.Vector2 vector3 = new global::UnityEngine.Vector2(points[points.Count - 1].x, points[points.Count - 1].z);
			global::UnityEngine.Vector2 vector4 = new global::UnityEngine.Vector2(vector2.x - this.circleCenter.x, vector2.z - this.circleCenter.z);
			if (this.IsClockwise(vector3, vector4))
			{
				if (global::UnityEngine.Vector2.SqrMagnitude(vector4 - vector3) >= 0.01f && global::UnityEngine.Vector2.SqrMagnitude(vector4 - new global::UnityEngine.Vector2(points[0].x, points[0].z)) >= 0.01f)
				{
					goto IL_238;
				}
			}
			IL_2F2:
			i++;
			continue;
			IL_238:
			if (flatEnv)
			{
				if (global::UnityEngine.Physics.Raycast(vector2 + global::UnityEngine.Vector3.up * this.envHeight + vector * flatOffset, global::UnityEngine.Vector3.down, out this.rayHitData, 2.8f, global::LayerMaskManager.decisionMask))
				{
					vector2.y = this.rayHitData.point.y;
				}
				else
				{
					vector2.y = base.transform.position.y;
				}
			}
			else
			{
				vector2.y = this.circleCenter.y;
			}
			vector2 -= this.circleCenter;
			points.Add(vector2);
			goto IL_2F2;
		}
	}

	public bool IsClockwise(global::UnityEngine.Vector3 a, global::UnityEngine.Vector3 b, global::UnityEngine.Vector3 c)
	{
		return (b.x - a.x) * (c.z - a.z) - (c.x - a.x) * (b.z - a.z) < 0f;
	}

	public bool IsClockwise(global::UnityEngine.Vector2 a, global::UnityEngine.Vector2 b)
	{
		return a.x * b.y - a.y * b.x < 0f;
	}

	private global::UnityEngine.Vector3 GetPoint(global::UnityEngine.Vector3 startPoint, global::UnityEngine.Vector3 dir, float dist)
	{
		global::UnityEngine.Vector3 vector;
		if (global::PandoraUtils.SendCapsule(startPoint, dir, this.capsuleMinHeight, global::UnityEngine.Mathf.Max(1.5f - this.sphereRadius, this.capsuleMinHeight), dist, this.sphereRadius, out this.rayHitData))
		{
			vector = this.rayHitData.point;
			float num = global::UnityEngine.Vector2.SqrMagnitude(new global::UnityEngine.Vector2(vector.x, vector.z) - new global::UnityEngine.Vector2(this.circleCenter.x, this.circleCenter.z));
			float num2 = global::UnityEngine.Vector2.SqrMagnitude(new global::UnityEngine.Vector2(startPoint.x, startPoint.z) - new global::UnityEngine.Vector2(this.circleCenter.x, this.circleCenter.z));
			if (global::UnityEngine.Mathf.Abs(num - num2) < 0.1f || num < num2)
			{
				return startPoint;
			}
			if (global::UnityEngine.Mathf.Abs(vector.y - startPoint.y) <= this.heightTreshold)
			{
				float distance = this.rayHitData.distance;
				float num3 = dist - distance + ((distance <= this.sphereRadius) ? 0f : this.sphereRadius);
				global::UnityEngine.Vector3 startPoint2 = vector - dir * this.sphereRadius;
				startPoint2.y = vector.y;
				if (num3 > this.sphereRadius)
				{
					return this.GetPoint(startPoint2, dir, num3);
				}
			}
		}
		else
		{
			vector = startPoint + dir * (dist + this.sphereRadius) + global::UnityEngine.Vector3.up * (this.capsuleMinHeight - this.sphereRadius);
		}
		return vector;
	}

	protected void CreateEdges(global::System.Collections.Generic.List<global::UnityEngine.Vector3> points, global::UnityEngine.Vector3 circlePosition)
	{
		this.Edges.Clear();
		for (int i = 0; i < points.Count; i++)
		{
			int index = (i + 1 >= points.Count) ? 0 : (i + 1);
			this.Edges.Add(new global::Tuple<global::UnityEngine.Vector2, global::UnityEngine.Vector2>(new global::UnityEngine.Vector2(points[i].x + circlePosition.x, points[i].z + circlePosition.z), new global::UnityEngine.Vector2(points[index].x + circlePosition.x, points[index].z + circlePosition.z)));
		}
	}

	protected void CreateCylinderOutlineMesh(global::UnityEngine.Mesh mesh, global::System.Collections.Generic.List<global::UnityEngine.Vector3> points, float bottomOffset, float topOffset)
	{
		global::UnityEngine.Vector3[] array = new global::UnityEngine.Vector3[points.Count * 2];
		global::UnityEngine.Vector2[] array2 = new global::UnityEngine.Vector2[array.Length];
		int[] array3 = new int[points.Count * 6];
		int num = points.Count * 2;
		for (int i = 0; i < points.Count; i++)
		{
			array[i] = points[i] + global::UnityEngine.Vector3.up * bottomOffset;
			array2[i] = new global::UnityEngine.Vector2((float)((i % 2 != 0) ? 1 : 0), 0f);
			array[i + points.Count] = points[i] + global::UnityEngine.Vector3.up * topOffset;
			array2[i + points.Count] = new global::UnityEngine.Vector2((float)((i % 2 != 0) ? 1 : 0), 1f);
			array3[i * 3] = i;
			array3[i * 3 + 1] = (i + 1) % points.Count;
			array3[i * 3 + 2] = i + points.Count;
			if (i < points.Count - 1)
			{
				array3[(i + points.Count) * 3] = (i + points.Count + 1) % num;
				array3[(i + points.Count) * 3 + 1] = (i + points.Count) % num;
				array3[(i + points.Count) * 3 + 2] = (i + 1) % num;
			}
		}
		int num2 = points.Count - 1;
		array3[(num2 + points.Count) * 3] = (num2 + points.Count) % num;
		array3[(num2 + points.Count) * 3 + 1] = (num2 + points.Count + 1) % num;
		array3[(num2 + points.Count) * 3 + 2] = (num2 + 1) % num;
		this.SetMesh(mesh, array, array3);
		mesh.uv = array2;
	}

	protected void CreateCylinderOutlineFlatMesh(global::UnityEngine.Mesh mesh, global::System.Collections.Generic.List<global::UnityEngine.Vector3> points)
	{
		global::UnityEngine.Vector3[] array = new global::UnityEngine.Vector3[points.Count * 2];
		global::UnityEngine.Vector2[] array2 = new global::UnityEngine.Vector2[array.Length];
		int[] array3 = new int[points.Count * 6];
		int num = points.Count * 2;
		for (int i = 0; i < points.Count; i++)
		{
			array[i] = points[i];
			array2[i] = new global::UnityEngine.Vector2((float)((i % 2 != 0) ? 1 : 0), 0f);
			array[i + points.Count] = points[i] + points[i].normalized * -0.1f;
			array2[i + points.Count] = new global::UnityEngine.Vector2((float)((i % 2 != 0) ? 1 : 0), 1f);
			array3[i * 3] = i;
			array3[i * 3 + 1] = (i + 1) % points.Count;
			array3[i * 3 + 2] = i + points.Count;
			if (i < points.Count - 1)
			{
				array3[(i + points.Count) * 3] = (i + points.Count + 1) % num;
				array3[(i + points.Count) * 3 + 1] = (i + points.Count) % num;
				array3[(i + points.Count) * 3 + 2] = (i + 1) % num;
			}
		}
		int num2 = points.Count - 1;
		array3[(num2 + points.Count) * 3] = (num2 + points.Count) % num;
		array3[(num2 + points.Count) * 3 + 1] = (num2 + points.Count + 1) % num;
		array3[(num2 + points.Count) * 3 + 2] = (num2 + 1) % num;
		this.SetMesh(mesh, array, array3);
		mesh.uv = array2;
	}

	protected void CreateCylinderMesh(global::UnityEngine.Mesh mesh, global::System.Collections.Generic.List<global::UnityEngine.Vector3> points, float bottomOffset, float topOffset)
	{
		global::UnityEngine.Vector3[] array = new global::UnityEngine.Vector3[points.Count * 2 + 2];
		int[] array2 = new int[points.Count * 12];
		int num = points.Count * 2;
		array[array.Length - 2] = global::UnityEngine.Vector3.up * bottomOffset;
		array[array.Length - 1] = global::UnityEngine.Vector3.up * topOffset;
		for (int i = 0; i < points.Count; i++)
		{
			array[i] = points[i] + global::UnityEngine.Vector3.up * bottomOffset;
			array[i + points.Count] = points[i] + global::UnityEngine.Vector3.up * topOffset;
			array2[i * 3] = i;
			array2[i * 3 + 1] = (i + 1) % points.Count;
			array2[i * 3 + 2] = i + points.Count;
			if (i < points.Count - 1)
			{
				array2[(i + points.Count) * 3] = (i + points.Count + 1) % num;
				array2[(i + points.Count) * 3 + 1] = (i + points.Count) % num;
				array2[(i + points.Count) * 3 + 2] = (i + 1) % num;
				array2[(i + points.Count * 2) * 3] = array.Length - 2;
				array2[(i + points.Count * 2) * 3 + 1] = (i + 1) % points.Count;
				array2[(i + points.Count * 2) * 3 + 2] = i;
				array2[(i + points.Count * 3) * 3] = array.Length - 1;
				array2[(i + points.Count * 3) * 3 + 1] = i + points.Count;
				array2[(i + points.Count * 3) * 3 + 2] = i + points.Count + 1;
			}
		}
		int num2 = points.Count - 1;
		array2[(num2 + points.Count) * 3] = (num2 + points.Count) % num;
		array2[(num2 + points.Count) * 3 + 1] = (num2 + points.Count + 1) % num;
		array2[(num2 + points.Count) * 3 + 2] = (num2 + 1) % num;
		array2[(num2 + points.Count * 2) * 3] = array.Length - 2;
		array2[(num2 + points.Count * 2) * 3 + 1] = (num2 + 1) % points.Count;
		array2[(num2 + points.Count * 2) * 3 + 2] = num2;
		array2[(num2 + points.Count * 3) * 3] = array.Length - 1;
		array2[(num2 + points.Count * 3) * 3 + 1] = num2 + points.Count;
		array2[(num2 + points.Count * 3) * 3 + 2] = points.Count;
		this.SetMesh(mesh, array, array2);
	}

	protected void CreateFlatMesh(global::UnityEngine.Mesh mesh, global::System.Collections.Generic.List<global::UnityEngine.Vector3> points)
	{
		global::UnityEngine.Vector3[] array = new global::UnityEngine.Vector3[points.Count + 1];
		array[0] = global::UnityEngine.Vector3.zero;
		int[] array2 = new int[array.Length * 3];
		for (int i = 0; i < points.Count; i++)
		{
			array[i + 1] = points[i];
			array2[i * 3] = 0;
			array2[i * 3 + 1] = i + 1;
			array2[i * 3 + 2] = (i + 2) % array.Length;
		}
		array2[array2.Length - 3] = 0;
		array2[array2.Length - 2] = array.Length - 1;
		array2[array2.Length - 1] = 1;
		this.SetMesh(mesh, array, array2);
	}

	private void SetMesh(global::UnityEngine.Mesh mesh, global::UnityEngine.Vector3[] vertices, int[] triangles)
	{
		mesh.Clear();
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.RecalculateBounds();
		mesh.RecalculateNormals();
	}

	protected float capsuleMinHeight = 0.6f;

	protected float sphereRadius = 0.2f;

	protected float heightTreshold = 0.5f;

	protected int angleIteration = 30;

	protected float envHeight = 1.5f;

	protected float pointsTreshold = 0.45f;

	protected float collisionPointsDistMin = 0.0001f;

	private global::UnityEngine.RaycastHit rayHitData;

	private global::UnityEngine.Vector3 circleCenter;
}
