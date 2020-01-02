using System;
using System.Collections.Generic;
using UnityEngine;

public class WeaponTrail : global::UnityEngine.MonoBehaviour
{
	public void Emit(bool activate)
	{
		this._emit = activate;
		if (activate)
		{
			base.enabled = activate;
		}
	}

	private void Start()
	{
		this._lastPosition = base.transform.position;
		this._o = new global::UnityEngine.GameObject(base.name + "_trail");
		this._o.transform.SetParent(null);
		this._o.transform.position = global::UnityEngine.Vector3.zero;
		this._o.transform.rotation = global::UnityEngine.Quaternion.identity;
		this._o.transform.localScale = global::UnityEngine.Vector3.one;
		this._o.AddComponent(typeof(global::UnityEngine.MeshFilter));
		this._o.AddComponent(typeof(global::UnityEngine.MeshRenderer));
		this._o.GetComponent<global::UnityEngine.Renderer>().material = this._material;
		this._trailMesh = new global::UnityEngine.Mesh();
		this._trailMesh.name = base.name + "TrailMesh";
		this._o.GetComponent<global::UnityEngine.MeshFilter>().mesh = this._trailMesh;
	}

	public global::UnityEngine.Material GetMaterial()
	{
		return this._o.GetComponent<global::UnityEngine.Renderer>().material;
	}

	private void OnDisable()
	{
		if (this._autoDestruct)
		{
			global::UnityEngine.Object.Destroy(this._o);
		}
	}

	private void OnDestroy()
	{
		if (this._o)
		{
			global::UnityEngine.Object.Destroy(this._o);
		}
	}

	private void Update()
	{
		if (this._emit && this._emitTime != 0f)
		{
			this._emitTime -= global::UnityEngine.Time.deltaTime;
			if (this._emitTime == 0f)
			{
				this._emitTime = -1f;
			}
			if (this._emitTime < 0f)
			{
				this._emit = false;
			}
		}
		if (this._emit)
		{
			float magnitude = (this._lastPosition - base.transform.position).magnitude;
			if (magnitude > this._minVertexDistance)
			{
				bool flag = false;
				if (this._points.Count < 3)
				{
					flag = true;
				}
				else
				{
					global::UnityEngine.Vector3 from = this._points[this._points.Count - 2].tipPosition - this._points[this._points.Count - 3].tipPosition;
					global::UnityEngine.Vector3 to = this._points[this._points.Count - 1].tipPosition - this._points[this._points.Count - 2].tipPosition;
					if (global::UnityEngine.Vector3.Angle(from, to) > this._maxAngle || magnitude > this._maxVertexDistance)
					{
						flag = true;
					}
				}
				if (flag)
				{
					global::WeaponTrail.Point point = new global::WeaponTrail.Point();
					point.basePosition = this._base.position;
					point.tipPosition = this._tip.position;
					point.timeCreated = global::UnityEngine.Time.time;
					this._points.Add(point);
					this._lastPosition = base.transform.position;
					if (this._points.Count == 1)
					{
						this._smoothedPoints.Add(point);
					}
					else if (this._points.Count > 1)
					{
						for (int i = 0; i < 1 + this.subdivisions; i++)
						{
							this._smoothedPoints.Add(point);
						}
					}
					if (this._points.Count >= 4)
					{
						global::System.Collections.Generic.IEnumerable<global::UnityEngine.Vector3> collection = global::Interpolate.NewCatmullRom(new global::UnityEngine.Vector3[]
						{
							this._points[this._points.Count - 4].tipPosition,
							this._points[this._points.Count - 3].tipPosition,
							this._points[this._points.Count - 2].tipPosition,
							this._points[this._points.Count - 1].tipPosition
						}, this.subdivisions, false);
						global::System.Collections.Generic.IEnumerable<global::UnityEngine.Vector3> collection2 = global::Interpolate.NewCatmullRom(new global::UnityEngine.Vector3[]
						{
							this._points[this._points.Count - 4].basePosition,
							this._points[this._points.Count - 3].basePosition,
							this._points[this._points.Count - 2].basePosition,
							this._points[this._points.Count - 1].basePosition
						}, this.subdivisions, false);
						global::System.Collections.Generic.List<global::UnityEngine.Vector3> list = new global::System.Collections.Generic.List<global::UnityEngine.Vector3>(collection);
						global::System.Collections.Generic.List<global::UnityEngine.Vector3> list2 = new global::System.Collections.Generic.List<global::UnityEngine.Vector3>(collection2);
						float timeCreated = this._points[this._points.Count - 4].timeCreated;
						float timeCreated2 = this._points[this._points.Count - 1].timeCreated;
						for (int j = 0; j < list.Count; j++)
						{
							int num = this._smoothedPoints.Count - (list.Count - j);
							if (num > -1 && num < this._smoothedPoints.Count)
							{
								global::WeaponTrail.Point point2 = new global::WeaponTrail.Point();
								point2.basePosition = list2[j];
								point2.tipPosition = list[j];
								point2.timeCreated = global::UnityEngine.Mathf.Lerp(timeCreated, timeCreated2, (float)j / (float)list.Count);
								this._smoothedPoints[num] = point2;
							}
						}
					}
				}
				else
				{
					this._points[this._points.Count - 1].basePosition = this._base.position;
					this._points[this._points.Count - 1].tipPosition = this._tip.position;
					this._smoothedPoints[this._smoothedPoints.Count - 1].basePosition = this._base.position;
					this._smoothedPoints[this._smoothedPoints.Count - 1].tipPosition = this._tip.position;
				}
			}
			else
			{
				if (this._points.Count > 0)
				{
					this._points[this._points.Count - 1].basePosition = this._base.position;
					this._points[this._points.Count - 1].tipPosition = this._tip.position;
				}
				if (this._smoothedPoints.Count > 0)
				{
					this._smoothedPoints[this._smoothedPoints.Count - 1].basePosition = this._base.position;
					this._smoothedPoints[this._smoothedPoints.Count - 1].tipPosition = this._tip.position;
				}
			}
		}
		if (!this._emit && this._lastFrameEmit && this._points.Count > 0)
		{
			this._points[this._points.Count - 1].lineBreak = true;
		}
		this._lastFrameEmit = this._emit;
		for (int k = this._points.Count - 1; k >= 0; k--)
		{
			if (global::UnityEngine.Time.time - this._points[k].timeCreated > this._lifeTime)
			{
				this._points.RemoveAt(k);
			}
		}
		for (int l = this._smoothedPoints.Count - 1; l >= 0; l--)
		{
			if (global::UnityEngine.Time.time - this._smoothedPoints[l].timeCreated > this._lifeTime)
			{
				this._smoothedPoints.RemoveAt(l);
			}
		}
		global::System.Collections.Generic.List<global::WeaponTrail.Point> smoothedPoints = this._smoothedPoints;
		if (smoothedPoints.Count > 1)
		{
			global::UnityEngine.Vector3[] array = new global::UnityEngine.Vector3[smoothedPoints.Count * 2];
			global::UnityEngine.Vector2[] array2 = new global::UnityEngine.Vector2[smoothedPoints.Count * 2];
			int[] array3 = new int[(smoothedPoints.Count - 1) * 6];
			global::UnityEngine.Color[] array4 = new global::UnityEngine.Color[smoothedPoints.Count * 2];
			for (int m = 0; m < smoothedPoints.Count; m++)
			{
				global::WeaponTrail.Point point3 = smoothedPoints[m];
				float num2 = (global::UnityEngine.Time.time - point3.timeCreated) / this._lifeTime;
				global::UnityEngine.Color color = global::UnityEngine.Color.Lerp(global::UnityEngine.Color.white, global::UnityEngine.Color.clear, num2);
				if (this._colors != null && this._colors.Length > 0)
				{
					float num3 = num2 * (float)(this._colors.Length - 1);
					float num4 = global::UnityEngine.Mathf.Floor(num3);
					float num5 = global::UnityEngine.Mathf.Clamp(global::UnityEngine.Mathf.Ceil(num3), 1f, (float)(this._colors.Length - 1));
					float t = global::UnityEngine.Mathf.InverseLerp(num4, num5, num3);
					if (num4 >= (float)this._colors.Length)
					{
						num4 = (float)(this._colors.Length - 1);
					}
					if (num4 < 0f)
					{
						num4 = 0f;
					}
					if (num5 >= (float)this._colors.Length)
					{
						num5 = (float)(this._colors.Length - 1);
					}
					if (num5 < 0f)
					{
						num5 = 0f;
					}
					color = global::UnityEngine.Color.Lerp(this._colors[(int)num4], this._colors[(int)num5], t);
				}
				float num6 = 0f;
				if (this._sizes != null && this._sizes.Length > 0)
				{
					float num7 = num2 * (float)(this._sizes.Length - 1);
					float num8 = global::UnityEngine.Mathf.Floor(num7);
					float num9 = global::UnityEngine.Mathf.Clamp(global::UnityEngine.Mathf.Ceil(num7), 1f, (float)(this._sizes.Length - 1));
					float t2 = global::UnityEngine.Mathf.InverseLerp(num8, num9, num7);
					if (num8 >= (float)this._sizes.Length)
					{
						num8 = (float)(this._sizes.Length - 1);
					}
					if (num8 < 0f)
					{
						num8 = 0f;
					}
					if (num9 >= (float)this._sizes.Length)
					{
						num9 = (float)(this._sizes.Length - 1);
					}
					if (num9 < 0f)
					{
						num9 = 0f;
					}
					num6 = global::UnityEngine.Mathf.Lerp(this._sizes[(int)num8], this._sizes[(int)num9], t2);
				}
				global::UnityEngine.Vector3 a = point3.tipPosition - point3.basePosition;
				array[m * 2] = point3.basePosition - a * (num6 * 0.5f);
				array[m * 2 + 1] = point3.tipPosition + a * (num6 * 0.5f);
				array4[m * 2] = (array4[m * 2 + 1] = color);
				float x = (float)m / (float)smoothedPoints.Count;
				array2[m * 2] = new global::UnityEngine.Vector2(x, 0f);
				array2[m * 2 + 1] = new global::UnityEngine.Vector2(x, 1f);
				if (m > 0)
				{
					array3[(m - 1) * 6] = m * 2 - 2;
					array3[(m - 1) * 6 + 1] = m * 2 - 1;
					array3[(m - 1) * 6 + 2] = m * 2;
					array3[(m - 1) * 6 + 3] = m * 2 + 1;
					array3[(m - 1) * 6 + 4] = m * 2;
					array3[(m - 1) * 6 + 5] = m * 2 - 1;
				}
			}
			this._trailMesh.Clear();
			this._trailMesh.vertices = array;
			this._trailMesh.colors = array4;
			this._trailMesh.uv = array2;
			this._trailMesh.triangles = array3;
			this._trailMesh.RecalculateNormals();
		}
		else if (!this._emit)
		{
			this._trailMesh.Clear();
			base.enabled = false;
			if (this._autoDestruct)
			{
				global::UnityEngine.Object.Destroy(this._o);
				global::UnityEngine.Object.Destroy(base.gameObject);
			}
		}
	}

	[global::UnityEngine.SerializeField]
	private bool _emit = true;

	[global::UnityEngine.SerializeField]
	private float _emitTime;

	[global::UnityEngine.SerializeField]
	private global::UnityEngine.Material _material;

	[global::UnityEngine.SerializeField]
	private float _lifeTime = 1f;

	[global::UnityEngine.SerializeField]
	private global::UnityEngine.Color[] _colors;

	[global::UnityEngine.SerializeField]
	private float[] _sizes;

	[global::UnityEngine.SerializeField]
	private float _minVertexDistance = 0.1f;

	[global::UnityEngine.SerializeField]
	private float _maxVertexDistance = 10f;

	[global::UnityEngine.SerializeField]
	private float _maxAngle = 3f;

	[global::UnityEngine.SerializeField]
	private bool _autoDestruct;

	[global::UnityEngine.SerializeField]
	private int subdivisions = 5;

	[global::UnityEngine.SerializeField]
	private global::UnityEngine.Transform _base;

	[global::UnityEngine.SerializeField]
	private global::UnityEngine.Transform _tip;

	private global::System.Collections.Generic.List<global::WeaponTrail.Point> _points = new global::System.Collections.Generic.List<global::WeaponTrail.Point>();

	private global::System.Collections.Generic.List<global::WeaponTrail.Point> _smoothedPoints = new global::System.Collections.Generic.List<global::WeaponTrail.Point>();

	private global::UnityEngine.GameObject _o;

	private global::UnityEngine.Mesh _trailMesh;

	private global::UnityEngine.Vector3 _lastPosition;

	private bool _lastFrameEmit = true;

	public class Point
	{
		public float timeCreated;

		public global::UnityEngine.Vector3 basePosition;

		public global::UnityEngine.Vector3 tipPosition;

		public bool lineBreak;
	}
}
