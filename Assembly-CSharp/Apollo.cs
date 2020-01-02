using System;
using System.Collections.Generic;
using mset;
using UnityEngine;

public class Apollo : global::UnityEngine.MonoBehaviour
{
	private void Start()
	{
		global::UnityEngine.RenderSettings.ambientLight = global::UnityEngine.Color.black;
		this.greyTex = new global::UnityEngine.Texture2D(16, 16);
		global::UnityEngine.Color color = new global::UnityEngine.Color(0.73f, 0.73f, 0.73f, 1f);
		global::UnityEngine.Color[] pixels = this.greyTex.GetPixels();
		for (int i = 0; i < pixels.Length; i++)
		{
			pixels[i] = color;
		}
		this.greyTex.SetPixels(pixels);
		this.greyTex.Apply(true);
		this.blackSpecTex = new global::UnityEngine.Texture2D(16, 16);
		global::UnityEngine.Color color2 = new global::UnityEngine.Color(0f, 0f, 0f, 0f);
		pixels = this.blackSpecTex.GetPixels();
		for (int j = 0; j < pixels.Length; j++)
		{
			pixels[j] = color2;
		}
		this.blackSpecTex.SetPixels(pixels);
		this.blackSpecTex.Apply(true);
		this.blackTex = new global::UnityEngine.Texture2D(16, 16);
		color2 = new global::UnityEngine.Color(0f, 0f, 0f, 1f);
		pixels = this.blackTex.GetPixels();
		for (int k = 0; k < pixels.Length; k++)
		{
			pixels[k] = color2;
		}
		this.blackTex.SetPixels(pixels);
		this.blackTex.Apply(true);
		this.initialize = false;
		this.normal = false;
		this.spec = false;
		this.final = false;
		this.lightOn = true;
		global::UnityEngine.GameObject gameObject = global::UnityEngine.GameObject.Find("clouds");
		this.skies = new global::System.Collections.Generic.List<global::mset.Sky>();
		if (gameObject != null)
		{
			this.skies.AddRange(gameObject.GetComponentsInChildren<global::mset.Sky>(true));
			this.skies.Add(this.lightGreySky);
			this.skies.Add(this.darkGreySky);
		}
		else
		{
			this.skies.AddRange(global::UnityEngine.Object.FindObjectsOfType<global::mset.Sky>());
		}
		this.camPlacer = global::UnityEngine.Object.FindObjectOfType<global::ApolloCameraPlacer>();
	}

	private void Update()
	{
		if (this.once || this.initialize)
		{
			this.once = false;
			this.initialize = false;
			global::PandoraSingleton<global::PandoraInput>.Instance.SetCurrentState(global::PandoraInput.States.MENU, true);
			this.meshes = (global::UnityEngine.Object.FindObjectsOfType(typeof(global::UnityEngine.Renderer)) as global::UnityEngine.Renderer[]);
			if (this.meshes != null)
			{
				this.colors = new global::UnityEngine.Color[this.meshes.Length];
				this.colors2 = new global::UnityEngine.Color[this.meshes.Length];
				this.colors3 = new global::UnityEngine.Color[this.meshes.Length];
				this.specColors = new global::UnityEngine.Color[this.meshes.Length];
				this.specColors2 = new global::UnityEngine.Color[this.meshes.Length];
				this.specColors3 = new global::UnityEngine.Color[this.meshes.Length];
				this.intensity = new float[this.meshes.Length];
				this.intensity2 = new float[this.meshes.Length];
				this.intensity3 = new float[this.meshes.Length];
				this.sharpness = new float[this.meshes.Length];
				this.sharpness2 = new float[this.meshes.Length];
				this.sharpness3 = new float[this.meshes.Length];
				this.fresnel = new float[this.meshes.Length];
				this.fresnel2 = new float[this.meshes.Length];
				this.fresnel3 = new float[this.meshes.Length];
				this.diffTextures = new global::UnityEngine.Texture[this.meshes.Length];
				this.diffTextures2 = new global::UnityEngine.Texture[this.meshes.Length];
				this.diffTextures3 = new global::UnityEngine.Texture[this.meshes.Length];
				this.glowTextures = new global::UnityEngine.Texture[this.meshes.Length];
				this.normalTextures = new global::UnityEngine.Texture[this.meshes.Length];
				this.normalTextures2 = new global::UnityEngine.Texture[this.meshes.Length];
				this.normalTextures3 = new global::UnityEngine.Texture[this.meshes.Length];
				this.specTextures = new global::UnityEngine.Texture[this.meshes.Length];
				this.specTextures2 = new global::UnityEngine.Texture[this.meshes.Length];
				this.specTextures3 = new global::UnityEngine.Texture[this.meshes.Length];
				for (int i = 0; i < this.meshes.Length; i++)
				{
					if (this.meshes[i].material.HasProperty("_Color"))
					{
						this.colors[i] = this.meshes[i].material.GetColor("_Color");
					}
					if (this.meshes[i].material.HasProperty("_Color2"))
					{
						this.colors2[i] = this.meshes[i].material.GetColor("_Color2");
					}
					if (this.meshes[i].material.HasProperty("_Color3"))
					{
						this.colors3[i] = this.meshes[i].material.GetColor("_Color3");
					}
					if (this.meshes[i].material.HasProperty("_SpecColor"))
					{
						this.specColors[i] = this.meshes[i].material.GetColor("_SpecColor");
					}
					if (this.meshes[i].material.HasProperty("_SpecColor2"))
					{
						this.specColors2[i] = this.meshes[i].material.GetColor("_SpecColor2");
					}
					if (this.meshes[i].material.HasProperty("_SpecColor3"))
					{
						this.specColors3[i] = this.meshes[i].material.GetColor("_SpecColor3");
					}
					if (this.meshes[i].material.HasProperty("_SpecInt"))
					{
						this.intensity[i] = this.meshes[i].material.GetFloat("_SpecInt");
					}
					if (this.meshes[i].material.HasProperty("_SpecInt2"))
					{
						this.intensity2[i] = this.meshes[i].material.GetFloat("_SpecInt2");
					}
					if (this.meshes[i].material.HasProperty("_SpecInt3"))
					{
						this.intensity3[i] = this.meshes[i].material.GetFloat("_SpecInt3");
					}
					if (this.meshes[i].material.HasProperty("_Shininess"))
					{
						this.sharpness[i] = this.meshes[i].material.GetFloat("_Shininess");
					}
					if (this.meshes[i].material.HasProperty("_Shininess2"))
					{
						this.sharpness2[i] = this.meshes[i].material.GetFloat("_Shininess2");
					}
					if (this.meshes[i].material.HasProperty("_Shininess3"))
					{
						this.sharpness3[i] = this.meshes[i].material.GetFloat("_Shininess3");
					}
					if (this.meshes[i].material.HasProperty("_Fresnel"))
					{
						this.fresnel[i] = this.meshes[i].material.GetFloat("_Fresnel");
					}
					if (this.meshes[i].material.HasProperty("_Fresnel2"))
					{
						this.fresnel2[i] = this.meshes[i].material.GetFloat("_Fresnel2");
					}
					if (this.meshes[i].material.HasProperty("_Fresnel3"))
					{
						this.fresnel3[i] = this.meshes[i].material.GetFloat("_Fresnel3");
					}
					if (this.meshes[i].material.HasProperty("_MainTex"))
					{
						this.diffTextures[i] = this.meshes[i].material.GetTexture("_MainTex");
					}
					if (this.meshes[i].material.HasProperty("_MainTex2"))
					{
						this.diffTextures2[i] = this.meshes[i].material.GetTexture("_MainTex2");
					}
					if (this.meshes[i].material.HasProperty("_MainTex3"))
					{
						this.diffTextures3[i] = this.meshes[i].material.GetTexture("_MainTex3");
					}
					if (this.meshes[i].material.HasProperty("_BumpMap"))
					{
						this.normalTextures[i] = this.meshes[i].material.GetTexture("_BumpMap");
					}
					if (this.meshes[i].material.HasProperty("_BumpMap2"))
					{
						this.normalTextures2[i] = this.meshes[i].material.GetTexture("_BumpMap2");
					}
					if (this.meshes[i].material.HasProperty("_BumpMap3"))
					{
						this.normalTextures3[i] = this.meshes[i].material.GetTexture("_BumpMap3");
					}
					if (this.meshes[i].material.HasProperty("_SpecTex"))
					{
						this.specTextures[i] = this.meshes[i].material.GetTexture("_SpecTex");
					}
					if (this.meshes[i].material.HasProperty("_SpecTex2"))
					{
						this.specTextures2[i] = this.meshes[i].material.GetTexture("_SpecTex2");
					}
					if (this.meshes[i].material.HasProperty("_SpecTex3"))
					{
						this.specTextures3[i] = this.meshes[i].material.GetTexture("_SpecTex3");
					}
					if (this.meshes[i].material.HasProperty("_Illum"))
					{
						this.glowTextures[i] = this.meshes[i].material.GetTexture("_Illum");
					}
				}
			}
			this.normal = false;
			this.spec = false;
			this.final = false;
			this.lights = (global::UnityEngine.Object.FindObjectsOfType(typeof(global::UnityEngine.Light)) as global::UnityEngine.Light[]);
		}
		if (this.final)
		{
			this.final = false;
			this.SetFinal();
		}
		if (this.normal)
		{
			this.normal = false;
			this.SetGreyNormal();
		}
		if (this.spec)
		{
			this.spec = false;
			this.SetSpecNormal();
		}
	}

	private void OnGUI()
	{
		global::UnityEngine.GUILayout.BeginVertical(new global::UnityEngine.GUILayoutOption[0]);
		if (global::UnityEngine.GUILayout.Button("Final", new global::UnityEngine.GUILayoutOption[0]))
		{
			this.SetFinal();
		}
		if (global::UnityEngine.GUILayout.Button("Grey Diff + Normal Map", new global::UnityEngine.GUILayoutOption[0]))
		{
			this.SetGreyNormal();
		}
		if (global::UnityEngine.GUILayout.Button("Spec + Normal Map", new global::UnityEngine.GUILayoutOption[0]))
		{
			this.SetSpecNormal();
		}
		string text = (!this.matColorsOn) ? "Material Colours Off" : "Material Colours On";
		if (global::UnityEngine.GUILayout.Button(text, new global::UnityEngine.GUILayoutOption[0]))
		{
			this.ToggleMatColours();
		}
		text = ((!this.specColorsOn) ? "Spec Colours Off" : "Spec Colours On");
		if (global::UnityEngine.GUILayout.Button(text, new global::UnityEngine.GUILayoutOption[0]))
		{
			this.ToggleSpecColours();
		}
		text = ((!this.specDataOn) ? "Spec Data Off" : "Spec Data On");
		if (global::UnityEngine.GUILayout.Button(text, new global::UnityEngine.GUILayoutOption[0]))
		{
			this.ToggleSpecData();
		}
		if (global::UnityEngine.GUILayout.Button(this.skies[this.selectedSky].gameObject.name, new global::UnityEngine.GUILayoutOption[0]))
		{
			this.SwitchSky();
		}
		string text2 = (!this.lightOn) ? "Lights Off" : "Lights On";
		if (global::UnityEngine.GUILayout.Button(text2, new global::UnityEngine.GUILayoutOption[0]))
		{
			this.ToggleLights();
		}
		if (this.camPlacer && global::UnityEngine.GUILayout.Button("Switch Cam", new global::UnityEngine.GUILayoutOption[0]))
		{
			this.camPlacer.next = true;
		}
		if (this.environments != null && this.environments.Length > 0 && global::UnityEngine.GUILayout.Button("Switch Layout", new global::UnityEngine.GUILayoutOption[0]))
		{
			this.environments[this.curEnv].gameObject.SetActive(false);
			this.curEnv = ++this.curEnv % this.environments.Length;
			this.environments[this.curEnv].gameObject.SetActive(true);
		}
		if (this.rotatingLight != null && global::UnityEngine.GUILayout.Button("Switch Rotating Light", new global::UnityEngine.GUILayoutOption[0]))
		{
			this.rotatingLight.SetActive(!this.rotatingLight.activeSelf);
		}
		global::UnityEngine.GUILayout.EndVertical();
	}

	private void ToggleMatColours()
	{
		this.matColorsOn = !this.matColorsOn;
		if (this.meshes != null)
		{
			for (int i = 0; i < this.meshes.Length; i++)
			{
				if (this.matColorsOn)
				{
					this.meshes[i].material.SetColor("_Color", this.colors[i]);
					this.meshes[i].material.SetColor("_Color2", this.colors2[i]);
					this.meshes[i].material.SetColor("_Color3", this.colors3[i]);
				}
				else
				{
					this.meshes[i].material.SetColor("_Color", global::UnityEngine.Color.white);
					this.meshes[i].material.SetColor("_Color2", global::UnityEngine.Color.white);
					this.meshes[i].material.SetColor("_Color3", global::UnityEngine.Color.white);
				}
			}
		}
	}

	private void ToggleSpecColours()
	{
		this.specColorsOn = !this.specColorsOn;
		if (this.meshes != null)
		{
			for (int i = 0; i < this.meshes.Length; i++)
			{
				if (this.specColorsOn)
				{
					this.meshes[i].material.SetColor("_SpecColor", this.specColors[i]);
					this.meshes[i].material.SetColor("_SpecColor2", this.specColors2[i]);
					this.meshes[i].material.SetColor("_SpecColor3", this.specColors3[i]);
				}
				else
				{
					this.meshes[i].material.SetColor("_SpecColor", global::UnityEngine.Color.white);
					this.meshes[i].material.SetColor("_SpecColor2", global::UnityEngine.Color.white);
					this.meshes[i].material.SetColor("_SpecColor3", global::UnityEngine.Color.white);
				}
			}
		}
	}

	private void ToggleSpecData()
	{
		this.specDataOn = !this.specDataOn;
		if (this.meshes != null)
		{
			for (int i = 0; i < this.meshes.Length; i++)
			{
				if (this.specDataOn)
				{
					if (this.meshes[i].material.HasProperty("_SpecInt"))
					{
						this.meshes[i].material.SetFloat("_SpecInt", this.intensity[i]);
					}
					if (this.meshes[i].material.HasProperty("_SpecInt2"))
					{
						this.meshes[i].material.SetFloat("_SpecInt2", this.intensity2[i]);
					}
					if (this.meshes[i].material.HasProperty("_SpecInt3"))
					{
						this.meshes[i].material.SetFloat("_SpecInt3", this.intensity3[i]);
					}
					if (this.meshes[i].material.HasProperty("_Shininess"))
					{
						this.meshes[i].material.SetFloat("_Shininess", this.sharpness[i]);
					}
					if (this.meshes[i].material.HasProperty("_Shininess2"))
					{
						this.meshes[i].material.SetFloat("_Shininess2", this.sharpness2[i]);
					}
					if (this.meshes[i].material.HasProperty("_Shininess3"))
					{
						this.meshes[i].material.SetFloat("_Shininess3", this.sharpness3[i]);
					}
					if (this.meshes[i].material.HasProperty("_Fresnel"))
					{
						this.meshes[i].material.SetFloat("_Fresnel", this.fresnel[i]);
					}
					if (this.meshes[i].material.HasProperty("_Fresnel2"))
					{
						this.meshes[i].material.SetFloat("_Fresnel2", this.fresnel2[i]);
					}
					if (this.meshes[i].material.HasProperty("_Fresnel3"))
					{
						this.meshes[i].material.SetFloat("_Fresnel3", this.fresnel3[i]);
					}
				}
				else
				{
					if (this.meshes[i].material.HasProperty("_SpecInt"))
					{
						this.meshes[i].material.SetFloat("_SpecInt", 1f);
					}
					if (this.meshes[i].material.HasProperty("_SpecInt2"))
					{
						this.meshes[i].material.SetFloat("_SpecInt2", 1f);
					}
					if (this.meshes[i].material.HasProperty("_SpecInt3"))
					{
						this.meshes[i].material.SetFloat("_SpecInt3", 1f);
					}
					if (this.meshes[i].material.HasProperty("_Shininess"))
					{
						this.meshes[i].material.SetFloat("_Shininess", 4f);
					}
					if (this.meshes[i].material.HasProperty("_Shininess2"))
					{
						this.meshes[i].material.SetFloat("_Shininess2", 4f);
					}
					if (this.meshes[i].material.HasProperty("_Shininess3"))
					{
						this.meshes[i].material.SetFloat("_Shininess3", 4f);
					}
					if (this.meshes[i].material.HasProperty("_Fresnel"))
					{
						this.meshes[i].material.SetFloat("_Fresnel", 0f);
					}
					if (this.meshes[i].material.HasProperty("_Fresnel2"))
					{
						this.meshes[i].material.SetFloat("_Fresnel2", 0f);
					}
					if (this.meshes[i].material.HasProperty("_Fresnel3"))
					{
						this.meshes[i].material.SetFloat("_Fresnel3", 0f);
					}
				}
			}
		}
	}

	private void SwitchSky()
	{
		if (this.skies[this.selectedSky].gameObject.transform.parent != null && this.skies[this.selectedSky].gameObject.transform.parent.gameObject.name.Contains("cloud"))
		{
			this.skies[this.selectedSky].gameObject.transform.parent.gameObject.SetActive(false);
		}
		if (++this.selectedSky >= this.skies.Count)
		{
			this.selectedSky = 0;
		}
		global::mset.SkyManager.Get().GlobalSky = this.skies[this.selectedSky];
		if (this.skies[this.selectedSky].gameObject.transform.parent != null && this.skies[this.selectedSky].gameObject.transform.parent.gameObject.name.Contains("cloud"))
		{
			this.skies[this.selectedSky].gameObject.transform.parent.gameObject.SetActive(true);
		}
	}

	private void ToggleLights()
	{
		this.lightOn = !this.lightOn;
		for (int i = 0; i < this.lights.Length; i++)
		{
			this.lights[i].enabled = this.lightOn;
		}
	}

	private void SetGreyNormal()
	{
		global::PandoraDebug.LogInfo("GreyNormal", "uncategorised", null);
		if (this.meshes != null)
		{
			for (int i = 0; i < this.meshes.Length; i++)
			{
				if (this.diffTextures[i])
				{
					this.meshes[i].material.SetTexture("_MainTex", this.greyTex);
				}
				if (this.diffTextures2[i])
				{
					this.meshes[i].material.SetTexture("_MainTex2", this.greyTex);
				}
				if (this.diffTextures3[i])
				{
					this.meshes[i].material.SetTexture("_MainTex3", this.greyTex);
				}
				if (this.specTextures[i])
				{
					this.meshes[i].material.SetTexture("_SpecTex", this.blackTex);
				}
				if (this.specTextures2[i])
				{
					this.meshes[i].material.SetTexture("_SpecTex2", this.blackTex);
				}
				if (this.specTextures3[i])
				{
					this.meshes[i].material.SetTexture("_SpecTex3", this.blackTex);
				}
				if (this.glowTextures[i])
				{
					this.meshes[i].material.SetTexture("_Illum", this.blackSpecTex);
				}
			}
		}
	}

	private void SetSpecNormal()
	{
		global::PandoraDebug.LogInfo("SpecNormal", "uncategorised", null);
		if (this.meshes != null)
		{
			for (int i = 0; i < this.meshes.Length; i++)
			{
				if (this.diffTextures[i])
				{
					this.meshes[i].material.SetTexture("_MainTex", this.blackTex);
				}
				if (this.diffTextures2[i])
				{
					this.meshes[i].material.SetTexture("_MainTex2", this.blackTex);
				}
				if (this.diffTextures3[i])
				{
					this.meshes[i].material.SetTexture("_MainTex3", this.blackTex);
				}
				if (this.specTextures[i])
				{
					this.meshes[i].material.SetTexture("_SpecTex", this.specTextures[i]);
				}
				if (this.specTextures2[i])
				{
					this.meshes[i].material.SetTexture("_SpecTex2", this.specTextures2[i]);
				}
				if (this.specTextures3[i])
				{
					this.meshes[i].material.SetTexture("_SpecTex3", this.specTextures3[i]);
				}
				if (this.glowTextures[i])
				{
					this.meshes[i].material.SetTexture("_Illum", this.blackSpecTex);
				}
			}
		}
	}

	private void SetFinal()
	{
		global::PandoraDebug.LogInfo("Final", "uncategorised", null);
		if (this.meshes != null)
		{
			for (int i = 0; i < this.meshes.Length; i++)
			{
				if (this.diffTextures[i])
				{
					this.meshes[i].material.SetTexture("_MainTex", this.diffTextures[i]);
				}
				if (this.diffTextures2[i])
				{
					this.meshes[i].material.SetTexture("_MainTex2", this.diffTextures2[i]);
				}
				if (this.diffTextures3[i])
				{
					this.meshes[i].material.SetTexture("_MainTex3", this.diffTextures3[i]);
				}
				if (this.specTextures[i])
				{
					this.meshes[i].material.SetTexture("_SpecTex", this.specTextures[i]);
				}
				if (this.specTextures2[i])
				{
					this.meshes[i].material.SetTexture("_SpecTex2", this.specTextures2[i]);
				}
				if (this.specTextures3[i])
				{
					this.meshes[i].material.SetTexture("_SpecTex3", this.specTextures3[i]);
				}
				if (this.glowTextures[i])
				{
					this.meshes[i].material.SetTexture("_Illum", this.glowTextures[i]);
				}
			}
		}
	}

	public bool initialize;

	public bool normal;

	public bool spec;

	public bool final;

	public bool lightOn = true;

	public bool matColorsOn = true;

	public bool specColorsOn = true;

	public bool specDataOn = true;

	private int selectedSky;

	public global::mset.Sky lightGreySky;

	public global::mset.Sky darkGreySky;

	private global::UnityEngine.Renderer[] meshes;

	private global::UnityEngine.Color[] colors;

	private global::UnityEngine.Color[] colors2;

	private global::UnityEngine.Color[] colors3;

	private global::UnityEngine.Color[] specColors;

	private global::UnityEngine.Color[] specColors2;

	private global::UnityEngine.Color[] specColors3;

	private float[] intensity;

	private float[] intensity2;

	private float[] intensity3;

	private float[] sharpness;

	private float[] sharpness2;

	private float[] sharpness3;

	private float[] fresnel;

	private float[] fresnel2;

	private float[] fresnel3;

	private global::UnityEngine.Texture[] diffTextures;

	private global::UnityEngine.Texture[] diffTextures2;

	private global::UnityEngine.Texture[] diffTextures3;

	private global::UnityEngine.Texture[] specTextures;

	private global::UnityEngine.Texture[] specTextures2;

	private global::UnityEngine.Texture[] specTextures3;

	private global::UnityEngine.Texture[] glowTextures;

	private global::UnityEngine.Texture[] normalTextures;

	private global::UnityEngine.Texture[] normalTextures2;

	private global::UnityEngine.Texture[] normalTextures3;

	private global::UnityEngine.Texture2D greyTex;

	private global::UnityEngine.Texture2D blackSpecTex;

	private global::UnityEngine.Texture2D blackTex;

	private global::UnityEngine.Light[] lights;

	private global::System.Collections.Generic.List<global::mset.Sky> skies;

	private bool once = true;

	private global::ApolloCameraPlacer camPlacer;

	public global::UnityEngine.GameObject[] environments;

	private int curEnv;

	public global::UnityEngine.GameObject rotatingLight;
}
