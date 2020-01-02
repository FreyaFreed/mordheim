using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using mset;
using Rewired;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameManager : global::PandoraSingleton<global::GameManager>
{
	public global::SaveManager Save { get; private set; }

	public global::OptionSave Options { get; private set; }

	public global::Profile Profile { get; private set; }

	public global::Tyche LocalTyche { get; private set; }

	public bool TacticalViewHelpersEnabled
	{
		get
		{
			return this.Options.tacticalViewHelpersEnabled;
		}
	}

	public bool WagonBeaconsEnabled
	{
		get
		{
			return this.Options.wagonBeaconsEnabled;
		}
	}

	public bool AutoExitTacticalEnabled
	{
		get
		{
			return this.Options.autoExitTacticalEnabled;
		}
	}

	public global::ConfirmationPopupView Popup
	{
		get
		{
			if (this.popup == null)
			{
				global::UnityEngine.GameObject gameObject = global::UnityEngine.Object.Instantiate<global::UnityEngine.GameObject>(this.popupPrefab);
				global::UnityEngine.Canvas component = gameObject.GetComponent<global::UnityEngine.Canvas>();
				component.sortingOrder = 999998;
				this.popup = gameObject.GetComponent<global::ConfirmationPopupView>();
				global::UnityEngine.Object.DontDestroyOnLoad(gameObject);
			}
			return this.popup;
		}
	}

	public bool IsFastForwarded
	{
		get
		{
			return this.Options.fastForwarded && !global::PandoraSingleton<global::Hermes>.Instance.IsConnected();
		}
	}

	private void Awake()
	{
		this.Init();
	}

	public void Init()
	{
		this.profileInitialized = false;
		this.graphicOptionsSet = false;
		this.Save = new global::SaveManager();
		this.LocalTyche = new global::Tyche((int)(global::UnityEngine.Random.value * 2.14748365E+09f), false);
		this.campaign = -1;
		global::PandoraDebug.LogDebug("INIT", "GAME MANAGER", this);
		using (global::System.Diagnostics.Process currentProcess = global::System.Diagnostics.Process.GetCurrentProcess())
		{
			currentProcess.PriorityClass = global::System.Diagnostics.ProcessPriorityClass.High;
		}
		string[] commandLineArgs = global::System.Environment.GetCommandLineArgs();
		this.Save.EraseOldSaveGame();
		this.EraseOldOptions(50);
		if (global::PandoraSingleton<global::Hephaestus>.Instance.FileExists("options.sg"))
		{
			global::PandoraSingleton<global::Hephaestus>.Instance.FileDelete("options.sg", new global::Hephaestus.OnFileDeleteCallback(this.OnOptionsDelete));
		}
		this.Options = new global::OptionSave();
		global::UnityEngine.Shader.WarmupAllShaders();
		global::PandoraSingleton<global::NoticeManager>.Instance.RegisterListener(global::Notices.CONTROLLER_CONNECTED, new global::DelReceiveNotice(this.OnControllerConnected));
		this.InitClient(null);
	}

	public void InitClient(global::UnityEngine.Events.UnityAction profileLoadedCb = null)
	{
		this.profileInitialized = false;
		this.graphicOptionsSet = false;
		base.StartCoroutine(this.InitializeHephaestusClient(profileLoadedCb));
	}

	private global::System.Collections.IEnumerator InitializeHephaestusClient(global::UnityEngine.Events.UnityAction profileLoadedCb)
	{
		yield return base.StartCoroutine(global::PandoraSingleton<global::Hephaestus>.Instance.InitializeClient());
		while (!global::PandoraSingleton<global::Hephaestus>.Instance.IsInitialized())
		{
			yield return null;
		}
		this.ReadOptions();
		while (!this.graphicOptionsSet)
		{
			yield return null;
		}
		if (profileLoadedCb != null)
		{
			while (!this.profileInitialized)
			{
				yield return null;
			}
			profileLoadedCb();
		}
		yield break;
	}

	public void ReadOptions()
	{
		if (global::System.IO.File.Exists(global::GameManager.OPTIONS_FILE))
		{
			global::Thoth.ReadFromFile(global::GameManager.OPTIONS_FILE, this.Options);
			this.ProcessOptions();
		}
		else
		{
			this.Options = new global::OptionSave();
			this.WriteOptions();
		}
	}

	public void WriteOptions()
	{
		global::Thoth.WriteToFile(global::GameManager.OPTIONS_FILE, this.Options);
		this.ProcessOptions();
	}

	private void ProcessOptions()
	{
		this.SetGraphicOptions();
		this.SetVolumeOptions();
		this.SetControlsOptions();
		this.SetMappingOptions();
		this.SetGameplayOptions();
		this.ReadProfile();
	}

	public void EraseOldOptions(int minVersion)
	{
		if (global::System.IO.File.Exists(global::GameManager.OPTIONS_FILE))
		{
			bool flag = false;
			byte[] array = global::System.IO.File.ReadAllBytes(global::GameManager.OPTIONS_FILE);
			if (array.Length < 4)
			{
				flag = true;
			}
			else
			{
				using (global::System.IO.MemoryStream memoryStream = new global::System.IO.MemoryStream(array))
				{
					using (global::System.IO.BinaryReader binaryReader = new global::System.IO.BinaryReader(memoryStream))
					{
						int num = binaryReader.ReadInt32();
						if (num < minVersion)
						{
							flag = true;
						}
					}
					memoryStream.Close();
				}
			}
			if (flag)
			{
				global::System.IO.File.Delete(global::GameManager.OPTIONS_FILE);
			}
		}
	}

	private void OnOptionsDelete(bool success)
	{
	}

	public void SetGraphicOptions()
	{
		base.StartCoroutine(this.SetGraphicOptionsCoroutine());
	}

	private global::System.Collections.IEnumerator SetGraphicOptionsCoroutine()
	{
		if (this.firstOptionsSetting)
		{
			switch (this.Options.shadowsQuality)
			{
			case 0:
				global::UnityEngine.QualitySettings.SetQualityLevel(0);
				break;
			case 1:
				global::UnityEngine.QualitySettings.SetQualityLevel(1);
				break;
			case 2:
				global::UnityEngine.QualitySettings.SetQualityLevel(2);
				break;
			case 3:
				global::UnityEngine.QualitySettings.SetQualityLevel(3);
				break;
			case 4:
				global::UnityEngine.QualitySettings.SetQualityLevel(4);
				break;
			default:
				global::UnityEngine.QualitySettings.SetQualityLevel(4);
				break;
			}
			switch (this.Options.textureQuality)
			{
			case 0:
				global::UnityEngine.QualitySettings.masterTextureLimit = 3;
				break;
			case 1:
				global::UnityEngine.QualitySettings.masterTextureLimit = 2;
				break;
			case 2:
				global::UnityEngine.QualitySettings.masterTextureLimit = 1;
				break;
			case 3:
				global::UnityEngine.QualitySettings.masterTextureLimit = 0;
				break;
			default:
				global::UnityEngine.QualitySettings.masterTextureLimit = 2;
				break;
			}
			switch (this.Options.shadowCascades)
			{
			case 0:
				global::UnityEngine.QualitySettings.shadowCascades = 0;
				break;
			case 1:
				global::UnityEngine.QualitySettings.shadowCascades = 2;
				break;
			case 2:
				global::UnityEngine.QualitySettings.shadowCascades = 4;
				break;
			default:
				global::UnityEngine.QualitySettings.shadowCascades = 0;
				break;
			}
			global::UnityEngine.QualitySettings.vSyncCount = ((!this.Options.vsync) ? 0 : 1);
		}
		this.graphicOptionsSet = true;
		while (global::UnityEngine.Camera.main == null)
		{
			yield return null;
		}
		global::CameraManager mngr = global::UnityEngine.Camera.main.GetComponent<global::CameraManager>();
		if (mngr)
		{
			global::mset.SkyManager.Get().GlobalSky.CamExposure = this.GetBrightnessExposureValue();
			mngr.SetDOFActive(this.Options.graphicsDof);
			mngr.SetSSAOActive(this.Options.graphicsSsao);
			mngr.SetSMAALevel(this.Options.graphicsSmaa);
		}
		if (!this.firstOptionsSetting)
		{
			bool resValid = false;
			for (int i = 0; i < global::UnityEngine.Screen.resolutions.Length; i++)
			{
				if (global::UnityEngine.Screen.resolutions[i].width == this.Options.resolution.width && global::UnityEngine.Screen.resolutions[i].height == this.Options.resolution.height)
				{
					resValid = true;
					break;
				}
			}
			if (!resValid)
			{
				this.Options.resolution = global::UnityEngine.Screen.resolutions[global::UnityEngine.Screen.resolutions.Length - 1];
			}
			global::UnityEngine.Screen.SetResolution(this.Options.resolution.width, this.Options.resolution.height, this.Options.fullScreen, 0);
			global::UnityEngine.Cursor.SetCursor(null, global::UnityEngine.Vector2.zero, global::UnityEngine.CursorMode.Auto);
		}
		this.firstOptionsSetting = false;
		yield break;
	}

	public void SetVolumeOptions()
	{
		global::PandoraSingleton<global::Pan>.Instance.SetVolume(global::Pan.Type.FX, this.Options.fxVolume);
		global::PandoraSingleton<global::Pan>.Instance.SetVolume(global::Pan.Type.MASTER, this.Options.masterVolume);
		global::PandoraSingleton<global::Pan>.Instance.SetVolume(global::Pan.Type.MUSIC, this.Options.musicVolume);
		global::PandoraSingleton<global::Pan>.Instance.SetVolume(global::Pan.Type.VOICE, this.Options.voiceVolume);
		global::PandoraSingleton<global::Pan>.Instance.SetVolume(global::Pan.Type.AMBIENT, this.Options.ambientVolume);
	}

	public void SetControlsOptions()
	{
		if (this.Options.gamepadEnabled)
		{
			global::PandoraSingleton<global::PandoraInput>.Instance.ActivateController(global::Rewired.ControllerType.Joystick);
		}
		else
		{
			global::PandoraSingleton<global::PandoraInput>.Instance.DeactivateController(global::Rewired.ControllerType.Joystick);
		}
		global::PandoraSingleton<global::PandoraInput>.Instance.SetActionInverted("mouse_x", this.Options.cameraXInverted);
		global::PandoraSingleton<global::PandoraInput>.Instance.SetActionInverted("cam_x", this.Options.cameraXInverted);
		global::PandoraSingleton<global::PandoraInput>.Instance.SetActionInverted("mouse_y", this.Options.cameraYInverted);
		global::PandoraSingleton<global::PandoraInput>.Instance.SetActionInverted("cam_y", this.Options.cameraYInverted);
		global::PandoraSingleton<global::PandoraInput>.Instance.SetLeftHandedMouse(this.Options.leftHandedMouse, true);
		global::PandoraSingleton<global::PandoraInput>.Instance.SetLeftHandedController(this.Options.leftHandedController, true);
		global::PandoraSingleton<global::PandoraInput>.Instance.SetMouseSensitivity(this.Options.mouseSensitivity);
		global::PandoraSingleton<global::PandoraInput>.Instance.SetJoystickSensitivity(this.Options.joystickSensitivity);
	}

	public void SetGameplayOptions()
	{
		global::PandoraSingleton<global::LocalizationManager>.Instance.SetLanguage((global::SupportedLanguage)this.Options.language, false);
		if (global::PandoraSingleton<global::MissionManager>.Exists())
		{
			for (int i = 0; i < global::PandoraSingleton<global::MissionManager>.Instance.WarbandCtrlrs.Count; i++)
			{
				if (global::PandoraSingleton<global::MissionManager>.Instance.WarbandCtrlrs[i].Beacon != null)
				{
					global::PandoraSingleton<global::MissionManager>.Instance.WarbandCtrlrs[i].Beacon.SetActive(this.Options.wagonBeaconsEnabled);
				}
			}
		}
	}

	public void SetMappingOptions()
	{
		global::PandoraSingleton<global::PandoraInput>.Instance.LoadMappingFromXml(global::Rewired.ControllerType.Keyboard, this.Options.keyboardMappingData);
		global::PandoraSingleton<global::PandoraInput>.Instance.LoadMappingFromXml(global::Rewired.ControllerType.Joystick, this.Options.joystickMappingData);
		global::PandoraSingleton<global::PandoraInput>.Instance.LoadMappingFromXml(global::Rewired.ControllerType.Mouse, this.Options.mouseMappingData);
	}

	public float GetBrightnessExposureValue()
	{
		if (this.Options.graphicsBrightness <= 0.5f)
		{
			return 0.1f + this.Options.graphicsBrightness / 0.5f * 0.9f;
		}
		return 1f + (this.Options.graphicsBrightness - 0.5f) / 0.5f * 9f;
	}

	private void OnControllerConnected()
	{
		global::PandoraSingleton<global::PandoraInput>.Instance.LoadMappingFromXml(global::Rewired.ControllerType.Joystick, this.Options.joystickMappingData);
	}

	public void SaveProfile()
	{
		byte[] data = global::Thoth.WriteToArray(this.Profile.ProfileSave);
		global::PandoraSingleton<global::Hephaestus>.Instance.FileWrite("profile.sg", data, new global::Hephaestus.OnFileWriteCallback(this.OnSaveProfile));
	}

	private void OnSaveProfile(bool success)
	{
		if (!success)
		{
		}
	}

	public void ReadProfile()
	{
		if (global::PandoraSingleton<global::Hephaestus>.Instance.FileExists("profile.sg"))
		{
			global::PandoraDebug.LogInfo("Profile Read START", "uncategorised", null);
			global::PandoraSingleton<global::Hephaestus>.Instance.FileRead("profile.sg", new global::Hephaestus.OnFileReadCallback(this.OnProfileRead));
		}
		else
		{
			this.GenerateProfile();
			this.profileInitialized = true;
		}
	}

	private void OnProfileRead(byte[] data, bool success)
	{
		if (success)
		{
			global::ProfileSave profileSave = new global::ProfileSave();
			global::Thoth.ReadFromArray(data, profileSave);
			this.Profile = new global::Profile(profileSave);
		}
		else
		{
			this.GenerateProfile();
		}
		this.profileInitialized = true;
	}

	private void GenerateProfile()
	{
		this.Profile = new global::Profile(new global::ProfileSave());
		this.SaveProfile();
	}

	private void OnDeleteProfile(bool success)
	{
	}

	public void EnableInput()
	{
		global::PandoraSingleton<global::PandoraInput>.Instance.SetActive(true);
		if (global::UnityEngine.EventSystems.EventSystem.current != null)
		{
			global::UnityEngine.EventSystems.EventSystem.current.sendNavigationEvents = true;
		}
		global::PandoraSingleton<global::PandoraInput>.Instance.PopInputLayer(global::PandoraInput.InputLayer.TRANSITION);
		global::PandoraSingleton<global::Pan>.Instance.Narrate("main_menu" + global::PandoraSingleton<global::GameManager>.Instance.LocalTyche.Rand(1, 6));
		global::PandoraSingleton<global::PandoraInput>.Instance.SetCurrentState(global::PandoraInput.States.MENU, true);
	}

	public void NoControllerConnected()
	{
		this.ShowSystemPopup(global::GameManager.SystemPopupId.CONTROLLER_DISCONNECTED, "no_controller_connected_title", "no_controller_connected_desc", delegate(bool confirm)
		{
		}, false);
	}

	public void UserDisconnected(string playerName)
	{
		if (string.IsNullOrEmpty(playerName))
		{
			return;
		}
		this.ShowSystemPopup(global::GameManager.SystemPopupId.USER_DISCONNECTED, "user_disconnected_desc", "user_disconnected_title", playerName, delegate(bool confirm)
		{
			global::PandoraSingleton<global::Hephaestus>.Instance.Reset();
			this.Save.Reset();
			global::PandoraSingleton<global::TransitionManager>.Instance.Clear(true);
			global::PandoraSingleton<global::TransitionManager>.Instance.DestroyLoading(0f);
			global::PandoraSingleton<global::PandoraInput>.Instance.ClearInputLayer();
			base.StartCoroutine(global::PandoraSingleton<global::AssetBundleLoader>.Instance.UnloadAll(false));
			global::UnityEngine.SceneManagement.SceneManager.LoadScene("copyright");
		}, false);
	}

	public void OnlineStatusChanged(string title, string desc)
	{
		this.ShowSystemPopup(global::GameManager.SystemPopupId.ONLINE_STATUS, title, desc, null, false);
	}

	public void ShowSystemPopup(global::GameManager.SystemPopupId popupId, string title, string desc, global::System.Action<bool> cb, bool showCancel = false)
	{
		this.ShowSystemPopup(popupId, title, desc, string.Empty, cb, showCancel);
	}

	public void ShowSystemPopup(global::GameManager.SystemPopupId popupId, string title, string desc, string param, global::System.Action<bool> cb, bool showCancel = false)
	{
		global::GameManager.SystemPopupData systemPopupData = null;
		if (popupId != global::GameManager.SystemPopupId.DLC)
		{
			for (int i = 0; i < this.systemPopups.Count; i++)
			{
				if (this.systemPopups[i].popupId == popupId)
				{
					systemPopupData = this.systemPopups[i];
					this.systemPopups.RemoveAt(i);
				}
			}
		}
		if (systemPopupData == null)
		{
			systemPopupData = new global::GameManager.SystemPopupData();
		}
		systemPopupData.popupId = popupId;
		systemPopupData.title = title;
		systemPopupData.desc = desc;
		systemPopupData.descParam = param;
		systemPopupData.cb = cb;
		systemPopupData.showCancel = showCancel;
		this.systemPopups.Add(systemPopupData);
		this.DisplaySystemPopup();
	}

	private void DisplaySystemPopup()
	{
		if (this.systemPopups.Count == 0)
		{
			return;
		}
		global::GameManager.SystemPopupData data = this.systemPopups[this.systemPopups.Count - 1];
		this.Popup.ShowLocalized(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(data.title), (!string.IsNullOrEmpty(data.descParam)) ? global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(data.desc, new string[]
		{
			data.descParam
		}) : global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(data.desc), delegate(bool confirm)
		{
			if (data.cb != null)
			{
				data.cb(confirm);
			}
			this.systemPopups.Remove(data);
			this.DisplaySystemPopup();
		}, false, false);
		if (!data.showCancel)
		{
			this.Popup.HideCancelButton();
		}
	}

	public bool IsPopupExist(global::GameManager.SystemPopupId popupId)
	{
		for (int i = 0; i < this.systemPopups.Count; i++)
		{
			if (this.systemPopups[i].popupId == popupId)
			{
				return true;
			}
		}
		return false;
	}

	public void ResetTimeScale()
	{
		global::UnityEngine.Time.timeScale = ((!this.IsFastForwarded || !global::PandoraSingleton<global::MissionManager>.Exists()) ? 1f : 1.15f);
	}

	public const float FF_SPEED_AI = 1.5f;

	public const float FF_SPEED_GENERAL = 1.15f;

	private const string PROFILE_FILE = "profile.sg";

	private const string OPTIONS_NAME = "options.sg";

	private const int DELETE_OPTIONS_UNDER = 50;

	public static readonly string SAVE_FOLDER = global::System.Environment.GetFolderPath(global::System.Environment.SpecialFolder.MyDocuments) + "/my games/Mordheim/";

	private static readonly string OPTIONS_FILE = global::GameManager.SAVE_FOLDER + "options.sg";

	private bool firstOptionsSetting = true;

	[global::UnityEngine.HideInInspector]
	public int campaign = -1;

	public global::WarbandSave currentSave;

	[global::UnityEngine.HideInInspector]
	public bool profileInitialized;

	public bool graphicOptionsSet;

	public global::UnityEngine.GameObject popupPrefab;

	private global::ConfirmationPopupView popup;

	[global::UnityEngine.HideInInspector]
	[global::System.NonSerialized]
	public bool skipLogos;

	[global::UnityEngine.HideInInspector]
	[global::System.NonSerialized]
	public bool inCopyright;

	[global::UnityEngine.HideInInspector]
	[global::System.NonSerialized]
	public bool inVideo;

	private global::System.Collections.Generic.List<global::GameManager.SystemPopupData> systemPopups = new global::System.Collections.Generic.List<global::GameManager.SystemPopupData>();

	private enum graphicsQualitySettings
	{
		NO_SHADOWS,
		LOW_SHADOWS,
		MEDIUM_SHADOWS,
		HIGH_SHADOWS,
		VERY_HIGH_SHADOWS
	}

	public enum SystemPopupId
	{
		USER_DISCONNECTED,
		ONLINE_STATUS,
		LOST_CONNECTION,
		CONTROLLER_DISCONNECTED,
		DLC,
		INVITE,
		CONNECTION_VALIDATION
	}

	private class SystemPopupData
	{
		public global::GameManager.SystemPopupId popupId;

		public string title;

		public string desc;

		public string descParam;

		public global::System.Action<bool> cb;

		public bool showCancel;
	}
}
