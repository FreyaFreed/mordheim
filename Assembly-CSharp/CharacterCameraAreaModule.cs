using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[global::UnityEngine.RequireComponent(typeof(global::UnityEngine.UI.Image))]
public class CharacterCameraAreaModule : global::UIModule, global::UnityEngine.EventSystems.IEventSystemHandler, global::UnityEngine.EventSystems.IPointerEnterHandler, global::UnityEngine.EventSystems.IPointerExitHandler
{
	protected override void Awake()
	{
		base.Awake();
		this.defaultTarget = new global::UnityEngine.GameObject("CharacterCameraAreaModule_default_camera_target");
	}

	protected void Start()
	{
		this.btnRotateLeft.SetAction(string.Empty, string.Empty, 0, false, null, null);
		this.btnRotateLeft.OnAction(delegate
		{
			global::PandoraSingleton<global::HideoutManager>.Instance.currentUnit.transform.Rotate(0f, -45f, 0f);
		}, true, true);
		this.btnRotateRight.SetAction(string.Empty, string.Empty, 0, false, null, null);
		this.btnRotateRight.OnAction(delegate
		{
			global::PandoraSingleton<global::HideoutManager>.Instance.currentUnit.transform.Rotate(0f, 45f, 0f);
		}, true, true);
	}

	private void OnDestroy()
	{
		if (this.defaultTarget != null)
		{
			global::UnityEngine.Object.Destroy(this.defaultTarget);
		}
	}

	public void Init(global::UnityEngine.Vector3 cameraOrigin)
	{
		this.cameraOrigin = cameraOrigin;
		this.camManager = global::UnityEngine.Camera.main.GetComponent<global::CameraManager>();
	}

	private void Update()
	{
		if (global::PandoraSingleton<global::HideoutManager>.Instance.currentUnit)
		{
			float num = global::PandoraSingleton<global::PandoraInput>.Instance.GetAxis("cam_x", 0) * 5f;
			float num2 = global::PandoraSingleton<global::PandoraInput>.Instance.GetAxis("cam_y", 0) / 3f;
			if (this.pointerInRect)
			{
				num2 += global::PandoraSingleton<global::PandoraInput>.Instance.GetAxis("mouse_y", 0) / 3f;
				if (global::PandoraSingleton<global::PandoraInput>.Instance.GetKey("mouse_click", 0))
				{
					num += global::PandoraSingleton<global::PandoraInput>.Instance.GetAxis("mouse_x", 0) * 5f;
				}
			}
			global::PandoraSingleton<global::HideoutManager>.Instance.currentUnit.transform.Rotate(0f, -num, 0f);
			if (num2 != 0f)
			{
				float distanceToTarget = global::PandoraSingleton<global::HideoutManager>.Instance.CamManager.GetDistanceToTarget();
				if (distanceToTarget != 0f)
				{
					if (distanceToTarget - num2 < 2.5f)
					{
						num2 = distanceToTarget - 2.5f;
					}
					else if (distanceToTarget - num2 > 6f)
					{
						num2 = distanceToTarget - 6f;
					}
					global::PandoraSingleton<global::HideoutManager>.Instance.CamManager.Zoom2(num2);
				}
				else
				{
					this.SetCameraLookAtDefault(true);
				}
			}
		}
	}

	public void SetCameraLookAt(global::UnityEngine.Transform target, bool instantTransition)
	{
		float distanceToTarget = this.camManager.GetDistanceToTarget();
		if (distanceToTarget == 0f || distanceToTarget > 20f)
		{
			this.camManager.dummyCam.transform.position = this.cameraOrigin;
		}
		else
		{
			global::UnityEngine.Vector3 normalized = (this.cameraOrigin - target.transform.position).normalized;
			this.camManager.dummyCam.transform.position = target.transform.position + normalized * global::UnityEngine.Mathf.Max(2.5f, distanceToTarget);
		}
		float magnitude = (this.camManager.transform.position - target.transform.position).magnitude;
		bool flag = magnitude < 10f;
		this.camManager.LookAtFocus(target.transform, false, !instantTransition && flag);
	}

	public void SetCameraLookAtDefault(bool instantTransition)
	{
		global::UnityEngine.Vector3 position = global::PandoraSingleton<global::HideoutManager>.Instance.currentUnit.transform.position;
		global::Unit unit = global::PandoraSingleton<global::HideoutManager>.Instance.currentUnit.unit;
		if ((unit.WarbandId == global::WarbandId.SKAVENS || unit.Id == global::UnitId.GHOUL) && !unit.IsImpressive)
		{
			position.y += 0.7f;
		}
		else
		{
			position.y += 1.4f;
		}
		this.defaultTarget.transform.position = position;
		this.SetCameraLookAt(this.defaultTarget.transform, instantTransition);
	}

	public void OnPointerEnter(global::UnityEngine.EventSystems.PointerEventData eventData)
	{
		this.pointerInRect = true;
	}

	public void OnPointerExit(global::UnityEngine.EventSystems.PointerEventData eventData)
	{
		this.pointerInRect = false;
	}

	private const float MIN_ZOOM_DISTANCE = 2.5f;

	private const float MAX_ZOOM_DISTANCE = 6f;

	private bool pointerInRect;

	private global::UnityEngine.Vector3 cameraOrigin;

	private global::CameraManager camManager;

	private global::UnityEngine.GameObject defaultTarget;

	public global::ButtonGroup btnRotateLeft;

	public global::ButtonGroup btnRotateRight;
}
