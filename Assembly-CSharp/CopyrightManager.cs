using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CopyrightManager : global::UnityEngine.MonoBehaviour
{
	private void Awake()
	{
		global::PandoraSingleton<global::PandoraInput>.Instance.SetCurrentState(global::PandoraInput.States.MENU, false);
		global::PandoraSingleton<global::PandoraInput>.Instance.PushInputLayer(global::PandoraInput.InputLayer.TRANSITION);
		global::UnityEngine.Object.Destroy(this.cometLogo);
		global::PandoraSingleton<global::GameManager>.Instance.inCopyright = true;
		global::PandoraSingleton<global::GameManager>.Instance.inVideo = true;
	}

	private void Start()
	{
		this.skipButton.SetAction("action", "controls_action_skip_intro", -1, false, null, null);
		this.skipButton.gameObject.SetActive(false);
		this.autoSaveInfo.SetActive(false);
		this.playingMovie = false;
		this.transitionToMenu = false;
		this.transitionning = false;
		this.time = -1f;
		if (this.movieQueue.Length <= 0)
		{
			this.transitionToMenu = true;
		}
		if (global::PandoraSingleton<global::GameManager>.Instance.skipLogos)
		{
			this.movieQueueIdx = this.movieQueue.Length;
			this.copyrightInfo.SetActive(false);
		}
		this.FadeToMainMenu();
		base.StartCoroutine(this.LoadMainMenu());
	}

	private void Update()
	{
		if (this.time >= 0f)
		{
			this.time += global::UnityEngine.Time.deltaTime;
		}
		if (this.time >= 5f)
		{
			if (this.transitionToMenu)
			{
				this.transitionning = true;
				this.transitionToMenu = false;
				this.FadeToMainMenu();
			}
			else if (!this.transitionning && !this.playingMovie)
			{
				this.PlayMovie();
			}
		}
		if (this.playingMovie && this.skipButton.gameObject.activeSelf && !this.videoSkipped && global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp("action", -1))
		{
			this.videoSkipped = true;
			this.vidPlayer.Stop();
			this.OnMovieDone();
		}
	}

	private void FadeToMainMenu()
	{
		global::PandoraDebug.LogInfo("main_menu Start Fade", "FLOW", this);
		this.fade.destroy = true;
		this.fade.Fade(new global::FadeAction.OnFadeCallback(this.DestroyCopyright), new global::FadeAction.OnFadeCallback(global::PandoraSingleton<global::GameManager>.Instance.EnableInput));
		global::PandoraSingleton<global::Pan>.Instance.UnPauseMusic(true);
		global::PandoraSingleton<global::PandoraInput>.Instance.SetActive(false);
	}

	private void StartGame()
	{
		base.StartCoroutine(this.LoadMainMenu());
	}

	private global::System.Collections.IEnumerator LoadMainMenu()
	{
		while (!global::PandoraSingleton<global::GameManager>.Instance.profileInitialized)
		{
			yield return null;
		}
		global::UnityEngine.Application.backgroundLoadingPriority = global::UnityEngine.ThreadPriority.Low;
		global::UnityEngine.AsyncOperation mainMenuAsyncOperation = global::UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("main_menu", global::UnityEngine.SceneManagement.LoadSceneMode.Additive);
		yield return mainMenuAsyncOperation;
		global::UnityEngine.SceneManagement.SceneManager.SetActiveScene(global::UnityEngine.SceneManagement.SceneManager.GetSceneByName("main_menu"));
		yield return null;
		if (!global::PandoraSingleton<global::GameManager>.Instance.skipLogos)
		{
			this.time = 0f;
		}
		this.mainMenuCamera = global::UnityEngine.GameObject.Find("game_camera").GetComponent<global::UnityEngine.Camera>();
		this.mainMenuCamera.GetComponent<global::UnityEngine.Camera>().enabled = false;
		this.mainMenuCamera.GetComponent<global::UnityEngine.AudioListener>().enabled = false;
		this.mainMenuController = global::UnityEngine.GameObject.Find("gui/main_menu").GetComponent<global::MainMenuController>();
		this.mainMenuController.uiContainer.enabled = false;
		this.mainMenuController.environment.SetActive(false);
		global::PandoraSingleton<global::Pan>.Instance.PauseMusic(true);
		if (global::PandoraSingleton<global::GameManager>.Instance.skipLogos)
		{
			this.OnMovieDone();
		}
		else
		{
			global::PandoraSingleton<global::GameManager>.Instance.skipLogos = true;
		}
		yield break;
	}

	private void PlayMovie()
	{
		global::UnityEngine.QualitySettings.vSyncCount = 0;
		this.vidContainer.SetActive(true);
		base.StartCoroutine(this.vidPlayer.Play(this.movieQueue[this.movieQueueIdx], new global::System.Action(this.OnMovieDone)));
		this.copyrightPanel.SetActive(false);
		this.playingMovie = true;
		this.movieQueueIdx++;
		if (this.movieQueueIdx == this.movieQueue.Length)
		{
			this.skipButton.gameObject.SetActive(true);
			base.StopCoroutine("SubtitlePlayer");
			this.subtitleText.gameObject.SetActive(false);
			base.StartCoroutine("SubtitlePlayer");
		}
	}

	private void OnMovieDone()
	{
		this.playingMovie = false;
		if (this.movieQueueIdx >= this.movieQueue.Length)
		{
			this.transitionToMenu = true;
		}
	}

	private void DestroyCopyright()
	{
		this.mainMenuCamera.GetComponent<global::UnityEngine.Camera>().enabled = true;
		this.mainMenuCamera.GetComponent<global::UnityEngine.AudioListener>().enabled = true;
		this.mainMenuController.uiContainer.enabled = true;
		this.mainMenuController.environment.SetActive(true);
		global::UnityEngine.QualitySettings.vSyncCount = ((!global::PandoraSingleton<global::GameManager>.Instance.Options.vsync) ? 0 : 1);
		global::UnityEngine.Object.Destroy(base.gameObject);
		global::PandoraSingleton<global::GameManager>.Instance.inCopyright = false;
		global::UnityEngine.Application.backgroundLoadingPriority = global::UnityEngine.ThreadPriority.Normal;
	}

	private global::System.Collections.IEnumerator SubtitlePlayer()
	{
		int MAX_SUB = 36;
		int curSubIdx = 0;
		float[] subTimes = new float[]
		{
			13f,
			0.7f,
			0f,
			0.2f,
			0.2f,
			0.2f,
			4.5f,
			0.1f,
			0.5f,
			2.8f,
			0.1f,
			0f,
			0.1f,
			0.1f,
			0.5f,
			0.1f,
			0.1f,
			5.5f,
			0.1f,
			0.3f,
			0.1f,
			0.1f,
			0.1f,
			0.5f,
			0.1f,
			0.1f,
			0.1f,
			0.1f,
			0.1f,
			0.3f,
			0.1f,
			0.1f,
			0f,
			0.1f,
			0.1f,
			0f
		};
		float[] hideTimes = new float[]
		{
			5.2f,
			3.8f,
			3.1f,
			6.5f,
			3.8f,
			3.5f,
			3.2f,
			5f,
			6.4f,
			4.9f,
			5.6f,
			4.9f,
			5.3f,
			5.5f,
			3f,
			3.2f,
			3.7f,
			3.2f,
			3.2f,
			3.2f,
			6.5f,
			5.2f,
			3.7f,
			3.5f,
			5f,
			2.9f,
			4.2f,
			4.3f,
			7.2f,
			4.4f,
			2.1f,
			4f,
			4f,
			4.6f,
			3.2f,
			5.2f
		};
		while (curSubIdx < MAX_SUB)
		{
			yield return new global::UnityEngine.WaitForSeconds(subTimes[curSubIdx]);
			this.subtitleText.gameObject.SetActive(true);
			this.subtitleText.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("cut_scene_sub_" + (curSubIdx + 1));
			yield return new global::UnityEngine.WaitForSeconds(hideTimes[curSubIdx]);
			this.subtitleText.gameObject.SetActive(false);
			curSubIdx++;
		}
		yield return null;
		yield break;
	}

	private const float SHOW_TIME = 5f;

	private const string MAIN_MENU_SCENE = "main_menu";

	public global::UnityEngine.GameObject vidContainer;

	public global::VideoPlayer vidPlayer;

	public global::ImageGroup skipButton;

	public global::UnityEngine.GameObject copyrightPanel;

	public global::UnityEngine.GameObject copyrightInfo;

	public global::UnityEngine.GameObject autoSaveInfo;

	public global::ImageGroup saveButton;

	public global::UnityEngine.UI.Text subtitleText;

	public global::UnityEngine.GameObject cometLogo;

	public string[] movieQueue;

	private float time = -1f;

	private bool playingMovie;

	private bool transitionToMenu;

	private bool transitionning;

	private int movieQueueIdx;

	public global::FadeAction fade;

	private global::MainMenuController mainMenuController;

	private global::UnityEngine.Camera mainMenuCamera;

	private bool videoSkipped;
}
