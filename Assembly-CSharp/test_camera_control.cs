using System;
using mset;
using UnityEngine;

public class test_camera_control : global::UnityEngine.MonoBehaviour
{
	public void Start()
	{
		global::UnityEngine.Vector3 eulerAngles = base.transform.eulerAngles;
		this.euler.x = eulerAngles.y;
		this.euler.y = eulerAngles.x;
		this.euler.y = global::UnityEngine.Mathf.Repeat(this.euler.y + 180f, 360f) - 180f;
		this.target = new global::UnityEngine.GameObject("_FreeCameraTarget")
		{
			hideFlags = (global::UnityEngine.HideFlags.HideInHierarchy | global::UnityEngine.HideFlags.HideInInspector | global::UnityEngine.HideFlags.DontSaveInEditor | global::UnityEngine.HideFlags.NotEditable | global::UnityEngine.HideFlags.DontSaveInBuild | global::UnityEngine.HideFlags.DontUnloadUnusedAsset)
		}.transform;
		if (this.usePivotPoint)
		{
			this.target.position = this.pivotPoint;
			this.targetDist = (base.transform.position - this.target.position).magnitude;
		}
		else
		{
			this.target.position = base.transform.position + base.transform.forward * this.distance;
			this.targetDist = this.distance;
		}
		this.targetRot = base.transform.rotation;
		this.targetLookAt = this.target.position;
	}

	public void Update()
	{
		this.inputBounds.x = (float)base.GetComponent<global::UnityEngine.Camera>().pixelWidth * this.paramInputBounds.x;
		this.inputBounds.y = (float)base.GetComponent<global::UnityEngine.Camera>().pixelHeight * this.paramInputBounds.y;
		this.inputBounds.width = (float)base.GetComponent<global::UnityEngine.Camera>().pixelWidth * this.paramInputBounds.width;
		this.inputBounds.height = (float)base.GetComponent<global::UnityEngine.Camera>().pixelHeight * this.paramInputBounds.height;
		if (this.target && this.inputBounds.Contains(global::UnityEngine.Input.mousePosition))
		{
			float num = global::UnityEngine.Input.GetAxis("Mouse X");
			float num2 = global::UnityEngine.Input.GetAxis("Mouse Y");
			bool flag = global::UnityEngine.Input.GetMouseButton(0) || global::UnityEngine.Input.touchCount == 1;
			bool flag2 = global::UnityEngine.Input.GetMouseButton(1) || global::UnityEngine.Input.touchCount == 2;
			bool flag3 = global::UnityEngine.Input.GetMouseButton(2) || global::UnityEngine.Input.touchCount == 3;
			bool flag4 = global::UnityEngine.Input.touchCount >= 4;
			bool flag5 = flag;
			bool flag6 = flag4 || (flag && (global::UnityEngine.Input.GetKey(global::UnityEngine.KeyCode.LeftShift) || global::UnityEngine.Input.GetKey(global::UnityEngine.KeyCode.RightShift)));
			bool flag7 = flag3 || (flag && (global::UnityEngine.Input.GetKey(global::UnityEngine.KeyCode.LeftControl) || global::UnityEngine.Input.GetKey(global::UnityEngine.KeyCode.RightControl)));
			bool flag8 = flag2;
			if (flag6)
			{
				num = num * this.thetaSpeed * 0.02f;
				global::mset.SkyManager.Get().GlobalSky.transform.Rotate(new global::UnityEngine.Vector3(0f, num, 0f));
			}
			else if (flag7)
			{
				num = num * this.moveSpeed * 0.005f * this.targetDist;
				num2 = num2 * this.moveSpeed * 0.005f * this.targetDist;
				this.targetLookAt -= base.transform.up * num2 + base.transform.right * num;
				if (this.useMoveBounds)
				{
					this.targetLookAt.x = global::UnityEngine.Mathf.Clamp(this.targetLookAt.x, -this.moveBounds, this.moveBounds);
					this.targetLookAt.y = global::UnityEngine.Mathf.Clamp(this.targetLookAt.y, -this.moveBounds, this.moveBounds);
					this.targetLookAt.z = global::UnityEngine.Mathf.Clamp(this.targetLookAt.z, -this.moveBounds, this.moveBounds);
				}
			}
			else if (flag8)
			{
				num2 = num2 * this.zoomSpeed * 0.005f * this.targetDist;
				this.targetDist += num2;
				this.targetDist = global::UnityEngine.Mathf.Max(0.1f, this.targetDist);
			}
			else if (flag5)
			{
				num = num * this.thetaSpeed * 0.02f;
				num2 = num2 * this.phiSpeed * 0.02f;
				this.euler.x = this.euler.x + num;
				this.euler.y = this.euler.y - num2;
				this.euler.y = global::test_camera_control.ClampAngle(this.euler.y, this.phiBoundMin, this.phiBoundMax);
				this.targetRot = global::UnityEngine.Quaternion.Euler(this.euler.y, this.euler.x, 0f);
			}
			this.targetDist -= global::UnityEngine.Input.GetAxis("Mouse ScrollWheel") * this.zoomSpeed * 0.5f;
			this.targetDist = global::UnityEngine.Mathf.Max(0.1f, this.targetDist);
		}
	}

	public void OnEnable()
	{
	}

	public void FixedUpdate()
	{
		this.distance = this.moveSmoothing * this.targetDist + (1f - this.moveSmoothing) * this.distance;
		base.transform.rotation = global::UnityEngine.Quaternion.Slerp(base.transform.rotation, this.targetRot, this.rotateSmoothing);
		this.target.position = global::UnityEngine.Vector3.Lerp(this.target.position, this.targetLookAt, this.moveSmoothing);
		this.distanceVec.z = this.distance;
		base.transform.position = this.target.position - base.transform.rotation * this.distanceVec;
	}

	private static float ClampAngle(float angle, float min, float max)
	{
		if (angle < -360f)
		{
			angle += 360f;
		}
		if (angle > 360f)
		{
			angle -= 360f;
		}
		return global::UnityEngine.Mathf.Clamp(angle, min, max);
	}

	public float thetaSpeed = 250f;

	public float phiSpeed = 120f;

	public float moveSpeed = 10f;

	public float zoomSpeed = 30f;

	public float phiBoundMin = -89f;

	public float phiBoundMax = 89f;

	public bool useMoveBounds = true;

	public float moveBounds = 100f;

	public float rotateSmoothing = 0.5f;

	public float moveSmoothing = 0.7f;

	public float distance = 2f;

	private global::UnityEngine.Vector2 euler;

	private global::UnityEngine.Quaternion targetRot;

	private global::UnityEngine.Vector3 targetLookAt;

	private float targetDist;

	private global::UnityEngine.Vector3 distanceVec = new global::UnityEngine.Vector3(0f, 0f, 0f);

	private global::UnityEngine.Transform target;

	private global::UnityEngine.Rect inputBounds;

	public global::UnityEngine.Rect paramInputBounds = new global::UnityEngine.Rect(0f, 0f, 1f, 1f);

	public bool usePivotPoint = true;

	public global::UnityEngine.Vector3 pivotPoint = new global::UnityEngine.Vector3(0f, 2f, 0f);
}
