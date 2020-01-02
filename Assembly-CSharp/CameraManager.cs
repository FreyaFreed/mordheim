using System;
using System.Collections;
using HighlightingSystem;
using Smaa;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.ImageEffects;

public class CameraManager : global::UnityEngine.MonoBehaviour
{
	public global::UnityEngine.Transform Target { get; private set; }

	public global::UnityEngine.Transform LookAtTarget { get; private set; }

	public float Zoom { get; private set; }

	public bool Locked { get; set; }

	private void Awake()
	{
		this.Locked = false;
		this.fadeOutLOS = base.GetComponent<global::FadeOutLOS>();
		this.smaa = base.GetComponent<global::Smaa.SMAA>();
		this.ssao = base.GetComponent<global::SSAOPro>();
		this.ssaoBasic = base.GetComponent<global::UnityStandardAssets.ImageEffects.ScreenSpaceAmbientObscurance>();
		if (this.ssaoBasic != null)
		{
			this.ssaoBasic.enabled = false;
			global::UnityEngine.Object.Destroy(this.ssaoBasic);
		}
		this.fxPro = base.GetComponent<global::FxPro>();
		this.dofTarget = new global::UnityEngine.GameObject("dof_target");
		global::UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(this.dofTarget, base.gameObject.scene);
		this.dofTarget.transform.SetParent(null);
		this.fxPro.DOFParams.Target = this.dofTarget.transform;
		this.savedFocalLengthMultiplier = this.fxPro.DOFParams.FocalLengthMultiplier;
		this.lut = base.GetComponent<global::AmplifyColorEffect>();
		this.zoomLevel = 1;
		this.sizeDiff = 0f;
		this.Zoom = this.ZOOM_LEVELS[this.zoomLevel];
		this.dummyCam = new global::UnityEngine.GameObject("dummy_camera");
		global::UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(this.dummyCam, base.gameObject.scene);
		this.dummyCam.transform.SetParent(null);
		this.dummyCam.transform.position = base.transform.position;
		this.dummyCam.transform.rotation = base.transform.rotation;
		this.overlay = base.GetComponent<global::Overlay>();
		this.ActivateOverlay(false, 0f);
		this.bloodSplatter = base.GetComponent<global::BloodSplatter>();
		this.bloodSplatter.Deactivate();
		global::HighlightingSystem.HighlightingRenderer component = base.GetComponent<global::HighlightingSystem.HighlightingRenderer>();
		component.offsetFactor = -0.25f;
		this.stateMachine = new global::CheapStateMachine(10);
		this.stateMachine.AddState(new global::CameraFixed(this), 0);
		this.stateMachine.AddState(new global::CharacterFollowCam(this), 1);
		this.stateMachine.AddState(new global::WatchCamera(this), 2);
		this.stateMachine.AddState(new global::CameraAnim(this), 3);
		this.stateMachine.AddState(new global::OverviewCamera(this), 4);
		this.stateMachine.AddState(new global::ConstrainedCamera(this), 5);
		this.stateMachine.AddState(new global::SemiConstrainedCamera(this), 6);
		this.stateMachine.AddState(new global::DeployCam(this), 7);
		this.stateMachine.AddState(new global::MeleeAttackCamera(this), 8);
		this.stateMachine.AddState(new global::RotateAroundCam(this), 9);
		this.SwitchToCam(global::CameraManager.CameraType.FIXED, null, false, true, true, false);
		global::UnityEngine.Shader.SetGlobalTexture("_DissolveTex", this.dissolveTex);
		global::UnityEngine.Shader.SetGlobalTexture("_LodFadeTex", this.lodFadeTex);
	}

	private void OnDestroy()
	{
		this.stateMachine.Destroy();
	}

	public void SetZoomDiff(bool isLarge)
	{
		float num = this.sizeDiff;
		this.sizeDiff = 0f;
		if (isLarge)
		{
			this.sizeDiff = 1f;
		}
		if (num != this.sizeDiff)
		{
			this.Zoom = this.Zoom - num + this.sizeDiff;
		}
	}

	public void SetZoomLevel(uint level)
	{
		if ((ulong)level > (ulong)((long)this.ZOOM_LEVELS.Length))
		{
			level = (uint)(this.ZOOM_LEVELS.Length - 1);
		}
		this.zoomLevel = (int)level;
		this.Zoom = this.ZOOM_LEVELS[this.zoomLevel] + this.sizeDiff;
	}

	public void SetTarget(global::UnityEngine.Transform target)
	{
		this.Target = target;
		if (this.lut)
		{
			this.lut.TriggerVolumeProxy = this.Target;
		}
	}

	public T GetCurrentCam<T>()
	{
		return (T)((object)this.stateMachine.GetActiveState());
	}

	public T GetCamOfType<T>(global::CameraManager.CameraType type)
	{
		return (T)((object)this.stateMachine.GetState((int)type));
	}

	public global::CameraManager.CameraType GetCurrentCamType()
	{
		return (global::CameraManager.CameraType)this.stateMachine.GetActiveStateId();
	}

	private void FixedUpdate()
	{
		this.stateMachine.FixedUpdate();
	}

	public void LateUpdate()
	{
		if (global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp("zoom", 0))
		{
			this.Zoom = this.ZOOM_LEVELS[++this.zoomLevel % this.ZOOM_LEVELS.Length] + this.sizeDiff;
			this.Transition(0.5f, true);
		}
		float num = -global::PandoraSingleton<global::PandoraInput>.Instance.GetAxis("zoom_mouse", 0) / 4f;
		if (num != 0f)
		{
			this.Zoom = global::UnityEngine.Mathf.Clamp(this.Zoom + num, this.ZOOM_LEVELS[0] + this.sizeDiff, this.ZOOM_LEVELS[this.ZOOM_LEVELS.Length - 1] + this.sizeDiff);
			this.Transition(0.25f, true);
		}
		this.stateMachine.Update();
		if (this.transitionCam)
		{
			this.curTransTime += global::UnityEngine.Time.deltaTime;
			base.transform.position = global::UnityEngine.Vector3.Lerp(base.transform.position, this.dummyCam.transform.position, this.curTransTime / this.maxTransTime);
			base.transform.rotation = global::UnityEngine.Quaternion.Slerp(base.transform.rotation, this.dummyCam.transform.rotation, this.curTransTime / this.maxTransTime);
			if (this.curTransTime >= this.maxTransTime)
			{
				this.curTransTime = 0f;
				this.transitionCam = false;
			}
		}
		else
		{
			base.transform.position = this.dummyCam.transform.position;
			base.transform.rotation = this.dummyCam.transform.rotation;
			if (this.LookAtTarget != null)
			{
				this.dofTarget.transform.position = this.LookAtTarget.position;
			}
			else if (this.Target != null)
			{
				this.dofTarget.transform.position = this.Target.position;
			}
		}
	}

	public void SwitchToCam(global::CameraManager.CameraType camType, global::UnityEngine.Transform camTarget, bool transition = true, bool force = false, bool clearFocus = true, bool isLarge = false)
	{
		if (!this.Locked || force)
		{
			if (camTarget != null && camTarget != this.Target)
			{
				this.SetDOFTarget(camTarget, 0f);
				this.SetTarget(camTarget);
			}
			if (clearFocus)
			{
				this.focus = false;
				this.LookAtTarget = null;
			}
			if (transition)
			{
				this.Transition(2f, true);
			}
			else if (!transition)
			{
				this.CancelTransition();
			}
			this.stateMachine.ChangeState((int)camType);
			if (this.fadeOutLOS != null)
			{
				this.ResetLOSTarget(this.Target);
				this.AddLOSTarget(this.LookAtTarget);
			}
			this.SetZoomDiff(isLarge);
		}
	}

	public void SetShoulderCam(bool isLarge, bool clear = false)
	{
		if (clear)
		{
			this.shoulderRight = 0;
		}
		float num = 1.75f;
		float num2 = 0.5f;
		if (isLarge)
		{
			num += this.sizeDiff;
			num2 = 1.25f;
		}
		this.dummyCam.transform.position = this.Target.position;
		this.dummyCam.transform.rotation = this.Target.rotation;
		this.dummyCam.transform.Translate(new global::UnityEngine.Vector3(num2, num, -2f));
		bool flag;
		if (this.LookAtTarget)
		{
			global::UnityEngine.RaycastHit raycastHit;
			flag = global::UnityEngine.Physics.Linecast(this.dummyCam.transform.position, this.LookAtTarget.transform.position + global::UnityEngine.Vector3.up * 2.5f, out raycastHit, global::LayerMaskManager.groundMask);
		}
		else
		{
			global::UnityEngine.RaycastHit raycastHit;
			flag = global::UnityEngine.Physics.Linecast(this.dummyCam.transform.position, this.dummyCam.transform.position + this.dummyCam.transform.forward * 2.5f, out raycastHit, global::LayerMaskManager.groundMask);
		}
		if (flag)
		{
			this.dummyCam.transform.Translate(new global::UnityEngine.Vector3(-num2 * 2f, 0f, 0f));
			if (this.LookAtTarget)
			{
				global::UnityEngine.RaycastHit raycastHit;
				flag = global::UnityEngine.Physics.Linecast(this.dummyCam.transform.position, this.LookAtTarget.transform.position + global::UnityEngine.Vector3.up * 1.5f, out raycastHit, global::LayerMaskManager.groundMask);
			}
			else
			{
				global::UnityEngine.RaycastHit raycastHit;
				flag = global::UnityEngine.Physics.Linecast(this.dummyCam.transform.position, this.dummyCam.transform.position + this.dummyCam.transform.forward * 1.5f, out raycastHit, global::LayerMaskManager.groundMask);
			}
			if (flag)
			{
				this.dummyCam.transform.Translate(new global::UnityEngine.Vector3(num2 * 2f, 0f, 1.5f));
				if (this.shoulderRight != 2)
				{
					this.shoulderRight = 2;
					this.Transition(2f, true);
				}
			}
			else if (this.shoulderRight != 1)
			{
				this.shoulderRight = 1;
				this.Transition(2f, true);
			}
		}
		else if (this.shoulderRight != 0)
		{
			this.shoulderRight = 0;
			this.Transition(2f, true);
		}
		this.dummyCam.transform.Translate(new global::UnityEngine.Vector3(0f, 0f, 0.25f));
		if (this.LookAtTarget != null)
		{
			this.dummyCam.transform.LookAt(this.LookAtTarget);
		}
	}

	public void SetSideCam(bool isLarge, bool clear = false)
	{
		if (clear)
		{
			this.shoulderRight = 0;
		}
		float num = 1.5f;
		float d = 1f;
		if (isLarge)
		{
			num = 2.5f;
			d = 1.25f;
		}
		global::UnityEngine.Quaternion rotation = default(global::UnityEngine.Quaternion);
		rotation.SetLookRotation(this.LookAtTarget.position - this.Target.position, global::UnityEngine.Vector3.up);
		global::UnityEngine.Vector3 eulerAngles = rotation.eulerAngles;
		float num2 = this.Target.position.y - this.LookAtTarget.position.y;
		float num3 = global::UnityEngine.Vector3.SqrMagnitude(new global::UnityEngine.Vector3(this.Target.position.x, 0f, this.Target.position.z) - new global::UnityEngine.Vector3(this.LookAtTarget.position.x, 0f, this.LookAtTarget.position.z));
		bool flag = true;
		if (num2 > 2.5f && num3 < 9f)
		{
			flag = false;
			num += 0.5f;
		}
		global::UnityEngine.Vector3 position = this.Target.position + new global::UnityEngine.Vector3(0f, num, 0f) + rotation * global::UnityEngine.Vector3.right * d - rotation * global::UnityEngine.Vector3.forward * 1.5f;
		this.dummyCam.transform.position = position;
		this.dummyCam.transform.LookAt(this.LookAtTarget);
		this.dummyCam.transform.Translate(-this.dummyCam.transform.forward, global::UnityEngine.Space.World);
		float magnitude = (this.LookAtTarget.transform.position - this.dummyCam.transform.position).magnitude;
		if (flag)
		{
			global::UnityEngine.Vector3 position2 = this.Target.position;
			position2.y = this.LookAtTarget.transform.position.y;
			global::UnityEngine.RaycastHit raycastHit;
			if (global::UnityEngine.Physics.SphereCast(position2, 0.1f, -this.dummyCam.transform.forward, out raycastHit, magnitude, global::LayerMaskManager.groundMask) && raycastHit.transform != this.Target)
			{
				global::UnityEngine.Vector3 position3 = this.dummyCam.transform.position;
				position = this.Target.position + new global::UnityEngine.Vector3(0f, num, 0f) - rotation * global::UnityEngine.Vector3.right * d - rotation * global::UnityEngine.Vector3.forward * 1.5f;
				this.dummyCam.transform.position = position;
				this.dummyCam.transform.LookAt(this.LookAtTarget);
				this.dummyCam.transform.Translate(-this.dummyCam.transform.forward, global::UnityEngine.Space.World);
				magnitude = (this.LookAtTarget.transform.position - this.dummyCam.transform.position).magnitude;
				global::UnityEngine.RaycastHit raycastHit2;
				if (global::UnityEngine.Physics.SphereCast(position2, 0.1f, -this.dummyCam.transform.forward, out raycastHit2, magnitude, global::LayerMaskManager.groundMask) && raycastHit.transform != this.Target)
				{
					this.dummyCam.transform.position = position3;
					this.dummyCam.transform.LookAt(this.LookAtTarget);
					this.dummyCam.transform.position = raycastHit.point + this.dummyCam.transform.forward * 0.2f;
				}
			}
		}
		this.dummyCam.transform.LookAt(this.LookAtTarget);
	}

	public void Transition(float time = 2f, bool force = true)
	{
		if (!this.transitionCam || force)
		{
			this.transitionCam = true;
			this.curTransTime = 0f;
			this.maxTransTime = time;
		}
	}

	public void CancelTransition()
	{
		this.transitionCam = false;
		this.curTransTime = 0f;
	}

	public void ReplaceLOSTarget(global::UnityEngine.Transform target)
	{
		if (target != null)
		{
			this.fadeOutLOS.ReplaceTarget(target);
		}
	}

	public void ResetLOSTarget(global::UnityEngine.Transform target)
	{
		this.fadeOutLOS.ClearTargets();
		if (target != null)
		{
			this.fadeOutLOS.AddTarget(target);
		}
	}

	public void AddLOSTarget(global::UnityEngine.Transform target)
	{
		this.fadeOutLOS.AddTarget(target);
	}

	public void AddLOSLayer(string layer)
	{
		this.fadeOutLOS.AddLayer(global::UnityEngine.LayerMask.NameToLayer(layer));
	}

	public void RemoveLOSLayer(string layer)
	{
		this.fadeOutLOS.RemoveLayer(global::UnityEngine.LayerMask.NameToLayer(layer));
	}

	public void TurnOnDOF(global::UnityEngine.Transform target)
	{
		this.dofTarget.transform.position = target.position;
	}

	public void TurnOffDOF()
	{
	}

	public void SetDOFActive(bool active)
	{
		if (active)
		{
			this.fxPro.DOFParams.FocalLengthMultiplier = this.savedFocalLengthMultiplier;
		}
		else
		{
			this.fxPro.DOFParams.FocalLengthMultiplier = 0f;
		}
	}

	public void SetSSAOActive(bool enabled)
	{
		if (this.ssao != null && this.ssao.enabled != enabled)
		{
			this.ssao.enabled = enabled;
		}
		if (this.ssaoBasic != null && this.ssaoBasic.enabled != enabled)
		{
			this.ssaoBasic.enabled = enabled;
		}
	}

	public void SetSMAALevel(int level)
	{
		bool flag = level > 0;
		if (this.smaa.enabled != flag)
		{
			this.smaa.enabled = flag;
		}
		this.smaa.Quality = (global::Smaa.QualityPreset)(level - 1);
	}

	public void SetBloomActive(bool enabled)
	{
		this.fxPro.BloomEnabled = enabled;
	}

	public void LookAtFocus(global::UnityEngine.Transform target, bool overrideCurrentTarget, bool transition = true)
	{
		if (overrideCurrentTarget)
		{
			this.Target = target;
		}
		this.LookAtTarget = target;
		this.SetDOFTarget(target, 0f);
		if (transition)
		{
			this.Transition(2f, true);
		}
		this.dummyCam.transform.LookAt(this.LookAtTarget);
	}

	public void ClearLookAtFocus()
	{
		this.LookAtTarget = null;
		if (this.Target != null)
		{
			this.Transition(2f, true);
			this.SetDOFTarget(this.Target.transform, 0f);
		}
	}

	public float GetDistanceToTarget()
	{
		if (this.LookAtTarget == null)
		{
			return 0f;
		}
		return (this.dummyCam.transform.position - this.LookAtTarget.position).magnitude;
	}

	public void Zoom2(float distance)
	{
		this.dummyCam.transform.Translate(this.dummyCam.transform.forward * distance, global::UnityEngine.Space.World);
	}

	public void SetDOFTarget(global::UnityEngine.Transform target, float yOffset)
	{
		if (target == null)
		{
			return;
		}
		global::UnityEngine.Vector3 position = target.position;
		position.y = target.position.y + yOffset;
		this.dofTarget.transform.position = position;
	}

	public global::UnityEngine.Vector3 OrientOffset(global::UnityEngine.Transform trans, global::UnityEngine.Vector3 offset)
	{
		if (trans == null)
		{
			return global::UnityEngine.Vector3.zero;
		}
		global::UnityEngine.Vector3 a = global::UnityEngine.Vector3.zero;
		a += trans.forward * offset.z;
		a += trans.up * offset.y;
		return a + trans.right * offset.x;
	}

	public void SetFOV(float newFOV, float time)
	{
	}

	private global::System.Collections.IEnumerator UpdateFOV()
	{
		bool inf = base.GetComponent<global::UnityEngine.Camera>().fieldOfView < this.targetFOV;
		while (base.GetComponent<global::UnityEngine.Camera>().fieldOfView != this.targetFOV)
		{
			base.GetComponent<global::UnityEngine.Camera>().fieldOfView += global::UnityEngine.Time.deltaTime * this.fovSpeed;
			if (inf)
			{
				base.GetComponent<global::UnityEngine.Camera>().fieldOfView = global::UnityEngine.Mathf.Min(base.GetComponent<global::UnityEngine.Camera>().fieldOfView, this.targetFOV);
			}
			else
			{
				base.GetComponent<global::UnityEngine.Camera>().fieldOfView = global::UnityEngine.Mathf.Max(base.GetComponent<global::UnityEngine.Camera>().fieldOfView, this.targetFOV);
			}
			yield return 0;
		}
		yield break;
	}

	public void ActivateOverlay(bool active, float time)
	{
		this.overlay.enabled = active;
	}

	public void ActivateBloodSplatter()
	{
		if (this.bloodSplatter != null)
		{
			this.bloodSplatter.Activate();
		}
	}

	public void DeactivateBloodSplatter()
	{
		if (this.bloodSplatter != null)
		{
			this.bloodSplatter.Deactivate();
		}
	}

	public const float SIZE_DIFF_LARGE = 1f;

	public const float SIZE_DIFF_NORMAL = 0f;

	public const float TRANSITION_TIME = 2f;

	private readonly float[] ZOOM_LEVELS = new float[]
	{
		2.2f,
		4f,
		6.2f
	};

	private global::CheapStateMachine stateMachine;

	public bool focus;

	public float sizeDiff;

	private int zoomLevel;

	public bool transitionCam = true;

	public float curTransTime;

	public float maxTransTime = 2f;

	public global::UnityEngine.GameObject dummyCam;

	private global::FadeOutLOS fadeOutLOS;

	public global::Overlay overlay;

	private global::BloodSplatter bloodSplatter;

	private global::SSAOPro ssao;

	private global::UnityStandardAssets.ImageEffects.ScreenSpaceAmbientObscurance ssaoBasic;

	private global::Smaa.SMAA smaa;

	private global::FxPro fxPro;

	public global::UnityEngine.GameObject dofTarget;

	private float savedFocalLengthMultiplier = 0.15f;

	public global::UnityEngine.Texture2D dissolveTex;

	public global::UnityEngine.Texture2D lodFadeTex;

	public int shoulderRight;

	private global::UnityEngine.Animator animator;

	private float targetFOV;

	private float fovSpeed = 1f;

	private global::AmplifyColorEffect lut;

	public enum CameraType
	{
		FIXED,
		CHARACTER,
		WATCH,
		ANIMATED,
		OVERVIEW,
		CONSTRAINED,
		SEMI_CONSTRAINED,
		DEPLOY,
		MELEE_ATTACK,
		ROTATE_AROUND,
		COUNT
	}
}
