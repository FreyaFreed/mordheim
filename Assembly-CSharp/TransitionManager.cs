using System;
using System.Collections;
using mset;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class TransitionManager : global::PandoraSingleton<global::TransitionManager>
{
	public global::SceneLoadingTypeId LoadingType
	{
		get
		{
			return this.loadingType;
		}
	}

	public bool GameLoadingDone { get; private set; }

	private void Awake()
	{
		this.Init();
	}

	public void Init()
	{
		this.SetState(global::TransitionManager.LoadingState.IDLE);
		this.GameLoadingDone = true;
	}

	private void LateUpdate()
	{
		if (this.transition != null && this.transitionTimer > 0f)
		{
			this.transitionTimer -= global::UnityEngine.Time.deltaTime;
			if (this.transitionTimer <= 0f)
			{
				this.transition.EndTransition();
				switch (this.currentState)
				{
				case global::TransitionManager.LoadingState.OUT_CURRENT_SCENE:
					base.StartCoroutine(this.ClearSystems());
					break;
				case global::TransitionManager.LoadingState.TO_NEXT_SCENE:
					if (!this.waitForPlayers || this.playersReady)
					{
						this.SetState(global::TransitionManager.LoadingState.IN_NEXT_SCENE);
					}
					break;
				case global::TransitionManager.LoadingState.IN_NEXT_SCENE:
					global::PandoraSingleton<global::PandoraInput>.Instance.SetActive(true);
					global::PandoraSingleton<global::PandoraInput>.Instance.PopInputLayer(global::PandoraInput.InputLayer.TRANSITION);
					if (global::UnityEngine.EventSystems.EventSystem.current != null)
					{
						global::UnityEngine.EventSystems.EventSystem.current.sendNavigationEvents = true;
					}
					global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice(global::Notices.TRANSITION_DONE);
					this.SetState(global::TransitionManager.LoadingState.IDLE);
					break;
				}
			}
			else
			{
				this.transition.ProcessTransition(1f - this.transitionTimer / this.transitionDuration);
			}
		}
		if (this.currentState == global::TransitionManager.LoadingState.IN_LOADING)
		{
			if (global::UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "loading" && !this.loadingStarted && this.transitionTimer <= 0f)
			{
				this.SetTransition(false);
				this.loadingStarted = true;
				this.loadingObject.OnTransitionDone();
				global::PandoraDebug.LogInfo("Loading Level:" + this.nextSceneName, "LOADING", this);
				global::UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(this.nextSceneName);
			}
			if (this.GameLoadingDone)
			{
				this.SetState((!this.waitAction) ? global::TransitionManager.LoadingState.TO_NEXT_SCENE : global::TransitionManager.LoadingState.WAIT_ACTION);
			}
		}
		else if (this.currentState == global::TransitionManager.LoadingState.TO_NEXT_SCENE && this.waitForPlayers && this.playersReady)
		{
			this.SetTransition(true);
			this.waitForPlayers = false;
		}
	}

	private void OnTransitionAction()
	{
		if (this.currentState == global::TransitionManager.LoadingState.WAIT_ACTION)
		{
			this.SetState(global::TransitionManager.LoadingState.TO_NEXT_SCENE);
		}
	}

	private void SetState(global::TransitionManager.LoadingState state)
	{
		this.currentState = state;
		switch (this.currentState)
		{
		case global::TransitionManager.LoadingState.OUT_CURRENT_SCENE:
			this.SetTransition(true);
			break;
		case global::TransitionManager.LoadingState.IN_LOADING:
			global::UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("loading");
			this.loadingStarted = false;
			break;
		case global::TransitionManager.LoadingState.WAIT_ACTION:
			global::PandoraSingleton<global::PandoraInput>.Instance.SetActive(true);
			global::PandoraSingleton<global::NoticeManager>.Instance.RegisterListener(global::Notices.TRANSITION_ACTION, new global::DelReceiveNotice(this.OnTransitionAction));
			global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice(global::Notices.TRANSITION_WAIT_FOR_ACTION);
			break;
		case global::TransitionManager.LoadingState.TO_NEXT_SCENE:
			if (this.waitForPlayers)
			{
				global::PandoraSingleton<global::MissionManager>.Instance.NetworkMngr.SendReadyToStart();
			}
			else
			{
				this.SetTransition(true);
			}
			break;
		case global::TransitionManager.LoadingState.IN_NEXT_SCENE:
			if (this.loadingType == global::SceneLoadingTypeId.NONE)
			{
				global::UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(this.nextSceneName);
			}
			else
			{
				global::UnityEngine.Object.Destroy(this.loadingObject.gameObject);
				this.loadingObject = null;
			}
			this.SetTransition(false);
			break;
		}
	}

	public void LoadNextScene(string nextScene, global::SceneLoadingTypeId loadingType, float transDuration, bool waitForAction = false, bool waitForPlayers = false, bool force = false)
	{
		if (this.currentState != global::TransitionManager.LoadingState.IDLE)
		{
			if (!force)
			{
				global::PandoraDebug.LogInfo("[TransitionManager] Trying to load a scene while not in Idle State. Use the force param to load it anyways.", "LOADING", null);
				return;
			}
			this.transition.EndTransition();
			this.DestroyLoading(transDuration);
		}
		this.transitionDuration = transDuration;
		this.transitionTimer = this.transitionDuration;
		this.nextSceneName = nextScene;
		this.loadingType = loadingType;
		this.waitAction = waitForAction;
		this.GameLoadingDone = false;
		this.playersReady = false;
		this.loadingObject = null;
		this.waitForPlayers = waitForPlayers;
		global::PandoraSingleton<global::PandoraInput>.Instance.ClearInputLayer();
		global::PandoraSingleton<global::PandoraInput>.Instance.SetActive(false);
		if (global::UnityEngine.EventSystems.EventSystem.current != null)
		{
			global::UnityEngine.EventSystems.EventSystem.current.sendNavigationEvents = false;
		}
		global::PandoraSingleton<global::PandoraInput>.instance.PushInputLayer(global::PandoraInput.InputLayer.TRANSITION);
		global::PandoraSingleton<global::Pan>.Instance.SoundsOff();
		this.SetState(global::TransitionManager.LoadingState.OUT_CURRENT_SCENE);
	}

	public void DestroyLoading(float transDuration = 0f)
	{
		if (this.loadingObject != null)
		{
			base.StartCoroutine(this.DestroyLoadingObjectDelayed(this.loadingObject.gameObject, transDuration));
		}
	}

	private global::System.Collections.IEnumerator DestroyLoadingObjectDelayed(global::UnityEngine.GameObject loadingObject, float delay)
	{
		yield return new global::UnityEngine.WaitForSeconds(delay);
		global::UnityEngine.Object.Destroy(loadingObject);
		yield break;
	}

	public void SetGameLoadingDone(bool overrideWaitPlayer = false)
	{
		if (overrideWaitPlayer)
		{
			this.waitForPlayers = false;
		}
		global::mset.SkyManager.Get().GlobalSky.CamExposure = global::PandoraSingleton<global::GameManager>.Instance.GetBrightnessExposureValue();
		if (this.currentState == global::TransitionManager.LoadingState.IN_LOADING)
		{
			this.GameLoadingDone = true;
		}
		if (this.noTransition)
		{
			this.noTransition = false;
			global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice(global::Notices.TRANSITION_DONE);
		}
	}

	private void SetTransition(bool show)
	{
		this.transitionTimer = this.transitionDuration;
		if (this.transition != null)
		{
			this.transition.Show(show, this.transitionDuration);
		}
	}

	public void RequestLoadingContent(global::LoadingViewManager loadingViewMan)
	{
		this.loadingObject = loadingViewMan;
		loadingViewMan.SetContent(this.loadingType, this.waitForPlayers);
	}

	private global::System.Collections.IEnumerator ClearSystems()
	{
		this.Clear(false);
		yield return base.StartCoroutine(global::PandoraSingleton<global::AssetBundleLoader>.Instance.UnloadAll(false));
		if (this.loadingType == global::SceneLoadingTypeId.NONE)
		{
			this.SetState(global::TransitionManager.LoadingState.IN_NEXT_SCENE);
		}
		else
		{
			this.SetState(global::TransitionManager.LoadingState.IN_LOADING);
		}
		yield break;
	}

	public void Clear(bool reset = false)
	{
		if (reset)
		{
			this.Init();
		}
		global::PandoraSingleton<global::NoticeManager>.Instance.RemoveListener(global::Notices.TRANSITION_ACTION, new global::DelReceiveNotice(this.OnTransitionAction));
		global::PandoraSingleton<global::NoticeManager>.Instance.Clear();
		global::PandoraSingleton<global::Hermes>.Instance.ResetGUID();
		global::PandoraSingleton<global::Pan>.Instance.Clear();
	}

	public void OnPlayersReady()
	{
		this.playersReady = true;
	}

	public bool IsDone()
	{
		return this.currentState == global::TransitionManager.LoadingState.IDLE;
	}

	public bool IsLoading()
	{
		return this.currentState == global::TransitionManager.LoadingState.OUT_CURRENT_SCENE || this.currentState == global::TransitionManager.LoadingState.IN_LOADING || this.currentState == global::TransitionManager.LoadingState.IN_NEXT_SCENE;
	}

	private const string LOADING_SCENE_NAME = "loading";

	[global::UnityEngine.SerializeField]
	private string nextSceneName;

	[global::UnityEngine.SerializeField]
	private global::SceneLoadingTypeId loadingType;

	private global::TransitionManager.LoadingState currentState;

	public global::TransitionBase transition;

	public bool noTransition;

	private float transitionDuration;

	private float transitionTimer;

	private bool loadingStarted;

	private global::LoadingViewManager loadingObject;

	private bool waitAction;

	private bool waitForPlayers;

	private bool playersReady;

	private enum LoadingState
	{
		IDLE,
		OUT_CURRENT_SCENE,
		IN_LOADING,
		WAIT_ACTION,
		TO_NEXT_SCENE,
		IN_NEXT_SCENE
	}
}
