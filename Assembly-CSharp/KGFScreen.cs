using System;
using System.Collections;
using System.IO;
using KGFUtils.Settings;
using UnityEngine;

public class KGFScreen : global::UnityEngine.MonoBehaviour
{
	protected void Awake()
	{
		if (global::KGFScreen.itsInstance == null)
		{
			global::KGFScreen.itsInstance = this;
			global::KGFScreen.itsInstance.Init();
			return;
		}
		if (global::KGFScreen.itsInstance != this)
		{
			global::UnityEngine.Debug.Log("there is more than one KFGDebug instance in this scene. please ensure there is always exactly one instance in this scene");
			global::UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	private global::KGFUtils.Settings.INIFile GetIniFile()
	{
		if (this.itsIniFile == null)
		{
			string text = global::KGFUtility.ConvertPathToPlatformSpecific(global::UnityEngine.Application.dataPath);
			text = global::System.IO.Path.Combine(text, "..");
			text = global::System.IO.Path.Combine(text, "settings.ini");
			this.itsIniFile = new global::KGFUtils.Settings.INIFile(text);
		}
		return this.itsIniFile;
	}

	public static global::KGFScreen GetInstance()
	{
		return global::KGFScreen.itsInstance;
	}

	private static void SetResolution3D(int theWidth, int theHeight)
	{
		global::KGFScreen.SetResolution3D(theWidth, theHeight, 60);
	}

	public static global::UnityEngine.Resolution GetResolution3D()
	{
		global::KGFScreen.CheckInstance();
		if (global::KGFScreen.itsInstance == null)
		{
			return default(global::UnityEngine.Resolution);
		}
		return global::KGFScreen.itsInstance.itsDataModuleScreen.itsResolution3D;
	}

	public static global::UnityEngine.Resolution GetResolution2D()
	{
		global::KGFScreen.CheckInstance();
		if (global::KGFScreen.itsInstance == null)
		{
			return default(global::UnityEngine.Resolution);
		}
		return global::KGFScreen.itsInstance.itsDataModuleScreen.itsResolution2D;
	}

	public static global::UnityEngine.Resolution GetResolutionDisplay()
	{
		global::KGFScreen.CheckInstance();
		if (global::KGFScreen.itsInstance == null)
		{
			return default(global::UnityEngine.Resolution);
		}
		return global::KGFScreen.itsInstance.itsDataModuleScreen.itsResolutionDisplay;
	}

	public static global::KGFScreen.eResolutionMode GetResolutionMode3D()
	{
		global::KGFScreen.CheckInstance();
		if (global::KGFScreen.itsInstance == null)
		{
			return global::KGFScreen.eResolutionMode.eNative;
		}
		return global::KGFScreen.itsInstance.itsDataModuleScreen.itsResolutionMode3D;
	}

	public static global::KGFScreen.eResolutionMode GetResolutionMode2D()
	{
		global::KGFScreen.CheckInstance();
		if (global::KGFScreen.itsInstance == null)
		{
			return global::KGFScreen.eResolutionMode.eNative;
		}
		return global::KGFScreen.itsInstance.itsDataModuleScreen.itsResolutionMode2D;
	}

	public static float GetAspect3D()
	{
		global::KGFScreen.CheckInstance();
		if (global::KGFScreen.itsInstance == null)
		{
			return 1f;
		}
		return global::KGFScreen.itsInstance.itsDataModuleScreen.itsAspect3D;
	}

	public static float GetAspect2D()
	{
		global::KGFScreen.CheckInstance();
		if (global::KGFScreen.itsInstance == null)
		{
			return 1f;
		}
		return global::KGFScreen.itsInstance.itsDataModuleScreen.itsAspect2D;
	}

	public static float GetScaleFactor3D()
	{
		global::KGFScreen.CheckInstance();
		if (global::KGFScreen.itsInstance == null)
		{
			return 1f;
		}
		return global::KGFScreen.itsInstance.itsDataModuleScreen.itsScaleFactor3D;
	}

	public static float GetScaleFactor2D()
	{
		global::KGFScreen.CheckInstance();
		if (global::KGFScreen.itsInstance == null)
		{
			return 1f;
		}
		return global::KGFScreen.itsInstance.itsDataModuleScreen.itsScaleFactor2D;
	}

	public static global::UnityEngine.Vector3 GetConvertedEventCurrentMousePosition(global::UnityEngine.Vector2 theEventCurrentMousePosition)
	{
		global::UnityEngine.Vector3 vector = global::UnityEngine.Input.mousePosition * global::KGFScreen.GetScaleFactor3D();
		global::UnityEngine.Vector3 mousePosition = global::UnityEngine.Input.mousePosition;
		float num = vector.x - mousePosition.x;
		float num2 = vector.y - mousePosition.y;
		num /= global::KGFScreen.GetScaleFactor3D();
		num2 /= global::KGFScreen.GetScaleFactor3D();
		global::UnityEngine.Vector2 v = new global::UnityEngine.Vector2(theEventCurrentMousePosition.x + num, theEventCurrentMousePosition.y - num2);
		return v;
	}

	public static global::UnityEngine.Vector3 GetMousePositionDisplay()
	{
		global::KGFScreen.CheckInstance();
		if (global::KGFScreen.itsInstance == null)
		{
			return global::UnityEngine.Vector3.zero;
		}
		float x = global::UnityEngine.Input.mousePosition.x * global::KGFScreen.GetScaleFactor3D();
		float y = (float)global::UnityEngine.Screen.height - global::UnityEngine.Input.mousePosition.y * global::KGFScreen.GetScaleFactor3D();
		global::UnityEngine.Vector3 result = new global::UnityEngine.Vector3(x, y, global::UnityEngine.Input.mousePosition.z);
		return result;
	}

	public static global::UnityEngine.Vector3 GetMousePosition2D()
	{
		global::KGFScreen.CheckInstance();
		if (global::KGFScreen.itsInstance == null)
		{
			return global::UnityEngine.Vector3.zero;
		}
		if (global::KGFScreen.GetResolutionMode3D() == global::KGFScreen.GetResolutionMode2D())
		{
			return global::UnityEngine.Input.mousePosition;
		}
		if (global::KGFScreen.GetResolutionMode2D() == global::KGFScreen.eResolutionMode.eNative && global::KGFScreen.GetResolutionMode3D() == global::KGFScreen.eResolutionMode.eAutoAdjust)
		{
			return global::UnityEngine.Input.mousePosition * global::KGFScreen.GetScaleFactor3D();
		}
		if (global::KGFScreen.GetResolutionMode2D() == global::KGFScreen.eResolutionMode.eAutoAdjust && global::KGFScreen.GetResolutionMode3D() == global::KGFScreen.eResolutionMode.eNative)
		{
			return global::UnityEngine.Input.mousePosition / global::KGFScreen.GetScaleFactor2D();
		}
		return global::UnityEngine.Input.mousePosition;
	}

	public static global::UnityEngine.Vector3 GetMousePositio3D()
	{
		global::KGFScreen.CheckInstance();
		if (global::KGFScreen.itsInstance == null)
		{
			return global::UnityEngine.Vector3.zero;
		}
		return global::UnityEngine.Input.mousePosition;
	}

	public static global::UnityEngine.Vector2 DisplayToScreen(global::UnityEngine.Vector2 theDisplayPosition)
	{
		return theDisplayPosition / global::KGFScreen.GetScaleFactor3D();
	}

	public static global::UnityEngine.Vector2 DisplayToScreen2D(global::UnityEngine.Vector2 theDisplayPosition)
	{
		return theDisplayPosition / global::KGFScreen.GetScaleFactor2D();
	}

	public static global::UnityEngine.Vector2 DisplayToScreenNormalized(global::UnityEngine.Vector2 theDisplayPosition)
	{
		global::UnityEngine.Vector2 result = new global::UnityEngine.Vector2(0f, 0f);
		global::UnityEngine.Vector2 vector = global::KGFScreen.DisplayToScreen(theDisplayPosition);
		result.x = vector.x / (float)global::KGFScreen.GetResolution3D().width;
		result.y = vector.y / (float)global::KGFScreen.GetResolution3D().height;
		return result;
	}

	public static global::UnityEngine.Rect DisplayToScreen(global::UnityEngine.Rect theDisplayRect)
	{
		return new global::UnityEngine.Rect(0f, 0f, 1f, 1f)
		{
			x = theDisplayRect.x / global::KGFScreen.GetScaleFactor3D(),
			y = theDisplayRect.y / global::KGFScreen.GetScaleFactor3D(),
			width = theDisplayRect.width / global::KGFScreen.GetScaleFactor3D(),
			height = theDisplayRect.height / global::KGFScreen.GetScaleFactor3D()
		};
	}

	public static global::UnityEngine.Rect DisplayToScreenNormalized(global::UnityEngine.Rect theDisplayRect)
	{
		global::UnityEngine.Rect result = new global::UnityEngine.Rect(0f, 0f, 1f, 1f);
		global::UnityEngine.Rect rect = global::KGFScreen.DisplayToScreen(theDisplayRect);
		result.x = rect.x / (float)global::KGFScreen.GetResolution3D().width;
		result.y = rect.y / (float)global::KGFScreen.GetResolution3D().height;
		result.width = rect.width / (float)global::KGFScreen.GetResolution3D().width;
		result.height = rect.height / (float)global::KGFScreen.GetResolution3D().height;
		return result;
	}

	public static global::UnityEngine.Rect NormalizedTo2DScreen(global::UnityEngine.Rect theDisplayRect)
	{
		return new global::UnityEngine.Rect(0f, 0f, 1f, 1f)
		{
			x = (float)global::KGFScreen.GetResolution2D().width * theDisplayRect.x,
			y = (float)global::KGFScreen.GetResolution2D().height * theDisplayRect.y,
			width = (float)global::KGFScreen.GetResolution2D().width * theDisplayRect.width,
			height = (float)global::KGFScreen.GetResolution2D().height * theDisplayRect.height
		};
	}

	public static global::UnityEngine.RenderTexture GetRenderTexture()
	{
		global::KGFScreen.CheckInstance();
		if (global::KGFScreen.itsInstance == null)
		{
			return null;
		}
		global::KGFScreen.itsInstance.CreateCamera();
		return global::KGFScreen.itsInstance.itsRenderTexture;
	}

	public static void BlitToScreen()
	{
		global::KGFScreen.CheckInstance();
		if (global::KGFScreen.itsInstance == null)
		{
			return;
		}
		if (global::KGFScreen.itsInstance.itsRenderTexture != null)
		{
			global::UnityEngine.Graphics.Blit(global::KGFScreen.itsInstance.itsRenderTexture, null);
		}
	}

	private void Update()
	{
	}

	private void CorrectMousePosition()
	{
		global::UnityEngine.Rect windowRect = global::KGFUtility.GetWindowRect();
		if (windowRect.width == 0f || windowRect.height == 0f)
		{
			return;
		}
		global::UnityEngine.Rect rect = new global::UnityEngine.Rect(windowRect.x, windowRect.y + windowRect.height - (float)global::KGFScreen.GetResolution2D().height, (float)global::KGFScreen.GetResolution2D().width, (float)global::KGFScreen.GetResolution2D().height);
		if (rect.width == 0f || rect.height == 0f)
		{
			return;
		}
		bool flag = false;
		global::KGFUtility.Point point;
		global::KGFUtility.GetCursorPos(out point);
		if ((float)point.X > rect.xMax)
		{
			point.X = (int)rect.xMax;
			flag = true;
		}
		if ((float)point.Y < rect.yMin)
		{
			point.Y = (int)rect.yMin;
			flag = true;
		}
		if ((float)point.Y > rect.yMax)
		{
			point.Y = (int)rect.yMax;
			flag = true;
		}
		if (flag)
		{
			global::KGFUtility.SetCursorPos(point.X, point.Y);
		}
	}

	private void CreateCamera()
	{
		if (this.itsCamera != null)
		{
			return;
		}
		base.gameObject.AddComponent<global::UnityEngine.Camera>();
		this.itsCamera = base.gameObject.GetComponent<global::UnityEngine.Camera>();
		this.itsCamera.clearFlags = global::UnityEngine.CameraClearFlags.Color;
		this.itsCamera.backgroundColor = global::UnityEngine.Color.black;
		this.itsCamera.cullingMask = 0;
		this.itsCamera.orthographic = true;
		this.itsCamera.orthographicSize = 1f;
		this.itsCamera.depth = 100f;
		this.itsCamera.farClipPlane = 1f;
		this.itsCamera.nearClipPlane = 0.5f;
	}

	private static void SetResolution3D(int theWidth, int theHeight, int theRefreshRate)
	{
		global::KGFScreen.CheckInstance();
		if (global::KGFScreen.itsInstance == null)
		{
			return;
		}
		global::KGFScreen.itsInstance.itsDataModuleScreen.itsResolution3D.width = theWidth;
		global::KGFScreen.itsInstance.itsDataModuleScreen.itsResolution3D.height = theHeight;
		global::KGFScreen.itsInstance.itsDataModuleScreen.itsResolution3D.refreshRate = theRefreshRate;
		global::KGFScreen.itsInstance.itsDataModuleScreen.itsAspect3D = global::KGFScreen.ReadAspect(theWidth, theHeight);
		global::KGFScreen.itsInstance.itsDataModuleScreen.itsScaleFactor3D = (float)global::KGFScreen.GetResolutionDisplay().width / (float)theWidth;
		global::UnityEngine.Debug.Log(string.Concat(new object[]
		{
			"KGFScreen: set resolution 3D to: ",
			theWidth,
			"/",
			theHeight,
			"/",
			theRefreshRate
		}));
		global::KGFScreen.itsInstance.CreateRenderTexture();
	}

	private static void SetResolution2D(int theWidth, int theHeight)
	{
		global::KGFScreen.CheckInstance();
		if (global::KGFScreen.itsInstance == null)
		{
			return;
		}
		global::KGFScreen.itsInstance.itsDataModuleScreen.itsResolution2D.width = theWidth;
		global::KGFScreen.itsInstance.itsDataModuleScreen.itsResolution2D.height = theHeight;
		global::KGFScreen.itsInstance.itsDataModuleScreen.itsResolution2D.refreshRate = 0;
		global::KGFScreen.itsInstance.itsDataModuleScreen.itsAspect2D = global::KGFScreen.ReadAspect(theWidth, theHeight);
		global::KGFScreen.itsInstance.itsDataModuleScreen.itsScaleFactor2D = ((float)global::KGFScreen.GetResolutionDisplay().width + 1f) / (float)theWidth;
		global::UnityEngine.Debug.Log(string.Concat(new object[]
		{
			"KGFScreen: set resolution 2D to: ",
			theWidth,
			"/",
			theHeight
		}));
	}

	private static void UpdateMouseRect()
	{
		global::UnityEngine.Rect windowRect = global::KGFUtility.GetWindowRect();
		global::KGFUtility.SetMouseRect(new global::UnityEngine.Rect(windowRect.x, windowRect.y + windowRect.height - (float)global::KGFScreen.GetResolution2D().height, (float)global::KGFScreen.GetResolution2D().width, (float)global::KGFScreen.GetResolution2D().height));
		global::UnityEngine.MonoBehaviour.print("new rect:" + windowRect);
	}

	private static void CheckInstance()
	{
		if (global::KGFScreen.itsInstance == null)
		{
			global::UnityEngine.Object @object = global::UnityEngine.Object.FindObjectOfType(typeof(global::KGFScreen));
			if (@object != null)
			{
				global::KGFScreen.itsInstance = (@object as global::KGFScreen);
				global::KGFScreen.itsInstance.Init();
			}
			else if (!global::KGFScreen.itsAlreadyChecked)
			{
				global::UnityEngine.Debug.LogError("KGFScreen is not running. Make sure that there is an instance of the KGFScreen prefab in the current scene.");
				global::KGFScreen.itsAlreadyChecked = true;
			}
		}
	}

	private void Init()
	{
		global::UnityEngine.Screen.SetResolution(global::UnityEngine.Screen.currentResolution.width, global::UnityEngine.Screen.currentResolution.height, false);
		base.StartCoroutine(this.SetResolutionDelayed());
	}

	private global::System.Collections.IEnumerator SetResolutionDelayed()
	{
		yield return new global::UnityEngine.WaitForSeconds(1f);
		this.ReadResolutionDisplay();
		global::UnityEngine.Debug.Log(string.Concat(new object[]
		{
			"display resolution set to: ",
			global::KGFScreen.GetResolutionDisplay().width,
			"/",
			global::KGFScreen.GetResolutionDisplay().height
		}));
		float anAspect = global::KGFScreen.ReadAspect(global::KGFScreen.GetResolutionDisplay().width, global::KGFScreen.GetResolutionDisplay().height);
		int aHeight = this.itsDataModuleScreen.itsMinHeight;
		int aWidth = (int)((float)aHeight * anAspect);
		if (aWidth < this.itsDataModuleScreen.itsMinWidth)
		{
			aWidth = this.itsDataModuleScreen.itsMinWidth;
			aHeight = (int)((float)this.itsDataModuleScreen.itsMinWidth / anAspect);
		}
		global::KGFScreen.eResolutionMode aResolutionMode3D = global::KGFScreen.GetResolutionMode3D();
		if (aResolutionMode3D == global::KGFScreen.eResolutionMode.eNative)
		{
			global::KGFScreen.SetResolution3D(global::KGFScreen.GetResolutionDisplay().width, global::KGFScreen.GetResolutionDisplay().height);
		}
		else if (aResolutionMode3D == global::KGFScreen.eResolutionMode.eAutoAdjust)
		{
			global::KGFScreen.SetResolution3D(aWidth, aHeight);
		}
		if (this.itsDataModuleScreen.itsResolutionMode2D == global::KGFScreen.eResolutionMode.eNative)
		{
			global::KGFScreen.SetResolution2D(global::KGFScreen.GetResolutionDisplay().width, global::KGFScreen.GetResolutionDisplay().height);
		}
		else if (this.itsDataModuleScreen.itsResolutionMode2D == global::KGFScreen.eResolutionMode.eAutoAdjust)
		{
			global::KGFScreen.SetResolution2D(aWidth, aHeight);
		}
		yield break;
	}

	private void ReadResolutionDisplay()
	{
		global::KGFScreen.itsInstance.itsDataModuleScreen.itsResolutionDisplay = default(global::UnityEngine.Resolution);
		global::KGFScreen.itsInstance.itsDataModuleScreen.itsResolutionDisplay.width = global::UnityEngine.Screen.width;
		global::KGFScreen.itsInstance.itsDataModuleScreen.itsResolutionDisplay.height = global::UnityEngine.Screen.height;
		global::KGFScreen.itsInstance.itsDataModuleScreen.itsResolutionDisplay.refreshRate = 60;
		global::KGFScreen.itsInstance.itsDataModuleScreen.itsAspectDisplay = global::KGFScreen.ReadAspect(global::UnityEngine.Screen.width, global::UnityEngine.Screen.height);
	}

	private static float ReadAspect(int theWidth, int theHeight)
	{
		return (float)theWidth / (float)theHeight;
	}

	private void CreateRenderTexture()
	{
		if (this.itsRenderTexture == null)
		{
			this.itsRenderTexture = new global::UnityEngine.RenderTexture(global::KGFScreen.GetResolution3D().width, global::KGFScreen.GetResolution3D().height, 16, global::UnityEngine.RenderTextureFormat.ARGB32);
		}
		else if (this.itsRenderTexture.width != global::KGFScreen.GetResolution3D().width)
		{
			this.itsRenderTexture.Release();
			this.itsRenderTexture = new global::UnityEngine.RenderTexture(global::KGFScreen.GetResolution3D().width, global::KGFScreen.GetResolution3D().height, 16, global::UnityEngine.RenderTextureFormat.ARGB32);
		}
		this.itsRenderTexture.isPowerOfTwo = true;
		this.itsRenderTexture.name = "KGFScreenRenderTexture";
		this.itsRenderTexture.Create();
	}

	private void OnPostRender()
	{
		global::KGFScreen.BlitToScreen();
	}

	private const string itsSettingsSection = "screen";

	private const string itsSettingsNameWidth = "resolution.width";

	private const string itsSettingsNameHeight = "resolution.height";

	private const string itsSettingsNameRefreshRate = "refreshrate";

	private const string itsSettingsNameIsFulscreen = "fullscreen";

	private static global::KGFScreen itsInstance;

	private static bool itsAlreadyChecked;

	private global::KGFUtils.Settings.INIFile itsIniFile;

	private global::UnityEngine.RenderTexture itsRenderTexture;

	private global::UnityEngine.Camera itsCamera;

	public global::KGFScreen.KGFDataScreen itsDataModuleScreen = new global::KGFScreen.KGFDataScreen();

	public enum eResolutionMode
	{
		eNative,
		eAutoAdjust
	}

	[global::System.Serializable]
	public class KGFDataScreen
	{
		public global::KGFScreen.eResolutionMode itsResolutionMode3D = global::KGFScreen.eResolutionMode.eAutoAdjust;

		public global::KGFScreen.eResolutionMode itsResolutionMode2D = global::KGFScreen.eResolutionMode.eAutoAdjust;

		[global::UnityEngine.HideInInspector]
		public global::UnityEngine.Resolution itsResolution3D;

		[global::UnityEngine.HideInInspector]
		public global::UnityEngine.Resolution itsResolution2D;

		[global::UnityEngine.HideInInspector]
		public global::UnityEngine.Resolution itsResolutionDisplay;

		[global::UnityEngine.HideInInspector]
		public float itsAspect3D = 1f;

		[global::UnityEngine.HideInInspector]
		public float itsAspect2D = 1f;

		[global::UnityEngine.HideInInspector]
		public float itsAspectDisplay = 1f;

		[global::UnityEngine.HideInInspector]
		public float itsScaleFactor3D = 1f;

		[global::UnityEngine.HideInInspector]
		public float itsScaleFactor2D = 1f;

		public int itsMinWidth = 480;

		public int itsMinHeight = 320;
	}
}
