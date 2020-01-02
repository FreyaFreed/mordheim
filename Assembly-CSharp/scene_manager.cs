using System;
using mset;
using Prometheus;
using UnityEngine;

public class scene_manager : global::UnityEngine.MonoBehaviour
{
	private void Start()
	{
		if (this.Imported == null)
		{
			this.Imported = global::UnityEngine.GameObject.Find("Imported").gameObject;
		}
		this.Imported.GetComponent<global::Prometheus.Rotate>().enabled = false;
		this.meshes = (global::UnityEngine.Object.FindObjectsOfType(typeof(global::UnityEngine.Renderer)) as global::UnityEngine.Renderer[]);
		this.lights = (global::UnityEngine.Object.FindObjectsOfType(typeof(global::UnityEngine.Light)) as global::UnityEngine.Light[]);
		this.LightIntensity = new float[this.lights.Length];
		for (int i = 0; i < this.lights.Length; i++)
		{
			this.LightIntensity[i] = this.lights[i].intensity;
		}
		global::UnityEngine.RenderSettings.ambientLight = global::UnityEngine.Color.black;
		this.shaderDiff = global::UnityEngine.Shader.Find("Diffuse");
		if (this.DaToneMapping == null && base.transform.GetComponent<global::Filmic>() != null)
		{
			this.DaToneMapping = base.transform.GetComponent<global::Filmic>();
		}
		if (this.DaColor == null && base.transform.GetComponent<global::AmplifyColorEffect>() != null)
		{
			this.DaColor = base.transform.GetComponent<global::AmplifyColorEffect>();
			this.DaColor.LutTexture = this.ColCorrection;
		}
		this.skies = (global::UnityEngine.Object.FindObjectsOfType(typeof(global::mset.Sky)) as global::mset.Sky[]);
		this.exposure = this.skies[0].MasterIntensity;
		this.Camexposure = this.skies[0].CamExposure;
		this.Diffexposure = this.skies[0].DiffIntensity;
		this.Specexposure = this.skies[0].SpecIntensity;
		if (this.fogs.Length > 0)
		{
			foreach (global::UnityEngine.GameObject gameObject in this.fogs)
			{
				gameObject.SetActive(false);
			}
		}
		this.currentSky = 0;
		for (int k = this.skies.Length - 1; k >= 0; k--)
		{
			this.setSky(k);
		}
		this.setBackground(this.background);
		this.greyTex = new global::UnityEngine.Texture2D(16, 16);
		global::UnityEngine.Color color = new global::UnityEngine.Color(0.73f, 0.73f, 0.73f, 1f);
		global::UnityEngine.Color[] pixels = this.greyTex.GetPixels();
		for (int l = 0; l < pixels.Length; l++)
		{
			pixels[l] = color;
		}
		this.greyTex.SetPixels(pixels);
		this.greyTex.Apply(true);
		this.blackTex = new global::UnityEngine.Texture2D(16, 16);
		pixels = this.blackTex.GetPixels();
		global::UnityEngine.Color color2 = new global::UnityEngine.Color(0.1f, 0.1f, 0.1f, 1f);
		for (int m = 0; m < pixels.Length; m++)
		{
			pixels[m] = color2;
		}
		this.blackTex.SetPixels(pixels);
		this.blackTex.Apply(true);
		if (this.meshes != null)
		{
			this.diffTextures = new global::UnityEngine.Texture[this.meshes.Length];
			this.specTextures = new global::UnityEngine.Texture[this.meshes.Length];
			this.glowTextures = new global::UnityEngine.Texture[this.meshes.Length];
			this.NormalTextures = new global::UnityEngine.Texture[this.meshes.Length];
			this.materials = new global::UnityEngine.Material[this.meshes.Length];
			this.RawDif = new global::UnityEngine.Material[this.meshes.Length];
			for (int n = 0; n < this.meshes.Length; n++)
			{
				if (this.meshes[n].material.HasProperty("_MainTex"))
				{
					this.diffTextures[n] = this.meshes[n].material.GetTexture("_MainTex");
				}
				if (this.meshes[n].material.HasProperty("_SpecTex"))
				{
					this.specTextures[n] = this.meshes[n].material.GetTexture("_SpecTex");
				}
				if (this.meshes[n].material.HasProperty("_Illum"))
				{
					this.glowTextures[n] = this.meshes[n].material.GetTexture("_Illum");
				}
				if (this.meshes[n].material.HasProperty("_BumpMap"))
				{
					this.NormalTextures[n] = this.meshes[n].material.GetTexture("_BumpMap");
				}
				this.materials[n] = this.meshes[n].material;
				this.RawDif[n] = new global::UnityEngine.Material(this.meshes[n].material);
				this.RawDif[n].shader = this.shaderDiff;
			}
		}
		this.setGrey(false);
		this.setBlack(false);
		this.setNormal(false);
	}

	private void setDiffuse(bool yes)
	{
		for (int i = 0; i < this.skies.Length; i++)
		{
			if (this.skies[i])
			{
				this.skies[i].DiffIntensity = ((!yes) ? 0f : 1f);
			}
		}
		if (global::mset.SkyManager.Get().GlobalSky)
		{
			global::mset.SkyManager.Get().GlobalSky.Apply();
		}
	}

	private void setSpecular(bool yes)
	{
		this.showSpecular = yes;
		for (int i = 0; i < this.skies.Length; i++)
		{
			if (this.skies[i])
			{
				this.skies[i].SpecIntensity = ((!yes) ? 0f : 1f);
			}
		}
		if (global::mset.SkyManager.Get().GlobalSky)
		{
			global::mset.SkyManager.Get().GlobalSky.Apply();
		}
	}

	private void setExposures(float val)
	{
		this.exposure = val;
		for (int i = 0; i < this.skies.Length; i++)
		{
			if (this.skies[i])
			{
				this.skies[i].MasterIntensity = val;
			}
		}
		global::mset.SkyManager.Get().GlobalSky.Apply();
	}

	private void setCamExposures(float val)
	{
		this.Camexposure = val;
		for (int i = 0; i < this.skies.Length; i++)
		{
			if (this.skies[i])
			{
				this.skies[i].CamExposure = val;
			}
		}
		global::mset.SkyManager.Get().GlobalSky.Apply();
	}

	private void setDiffExposures(float val)
	{
		this.Diffexposure = val;
		for (int i = 0; i < this.skies.Length; i++)
		{
			if (this.skies[i])
			{
				this.skies[i].DiffIntensity = val;
			}
		}
		global::mset.SkyManager.Get().GlobalSky.Apply();
	}

	private void setSpecExposures(float val)
	{
		this.Specexposure = val;
		for (int i = 0; i < this.skies.Length; i++)
		{
			if (this.skies[i])
			{
				this.skies[i].SpecIntensity = val;
			}
		}
		global::mset.SkyManager.Get().GlobalSky.Apply();
	}

	private void setSky(int index)
	{
		this.currentSky = index;
		this.skies[this.currentSky].Apply();
	}

	private void setBackground(bool yes)
	{
		this.background = yes;
		global::mset.SkyManager.Get().ShowSkybox = yes;
		global::mset.SkyManager.Get().GlobalSky.Apply();
	}

	private void setMaterials(bool yes)
	{
		if (this.meshes != null)
		{
			for (int i = 0; i < this.meshes.Length; i++)
			{
				if (yes)
				{
					this.meshes[i].material = this.RawDif[i];
				}
				else
				{
					this.meshes[i].material = this.materials[i];
				}
			}
		}
	}

	private void setGrey(bool yes)
	{
		if (this.meshes != null)
		{
			for (int i = 0; i < this.meshes.Length; i++)
			{
				if (yes)
				{
					if (this.diffTextures[i])
					{
						this.meshes[i].material.SetTexture("_MainTex", this.diffTextures[i]);
					}
				}
				else if (this.diffTextures[i])
				{
					this.meshes[i].material.SetTexture("_MainTex", this.greyTex);
				}
			}
		}
	}

	private void setBlack(bool yes)
	{
		if (this.meshes != null)
		{
			for (int i = 0; i < this.meshes.Length; i++)
			{
				if (yes)
				{
					if (this.specTextures[i])
					{
						this.meshes[i].material.SetTexture("_SpecTex", this.specTextures[i]);
					}
					if (this.glowTextures[i])
					{
						this.meshes[i].material.SetTexture("_Illum", this.glowTextures[i]);
					}
				}
				else
				{
					if (this.specTextures[i])
					{
						this.meshes[i].material.SetTexture("_SpecTex", this.blackTex);
					}
					if (this.glowTextures[i])
					{
						this.meshes[i].material.SetTexture("_Illum", this.blackTex);
					}
				}
			}
		}
	}

	private void setNormal(bool yes)
	{
		if (this.meshes != null)
		{
			for (int i = 0; i < this.meshes.Length; i++)
			{
				if (yes)
				{
					if (this.NormalTextures[i])
					{
						this.meshes[i].material.SetTexture("_BumpMap", this.NormalTextures[i]);
					}
				}
				else if (this.NormalTextures[i])
				{
					this.meshes[i].material.SetTexture("_BumpMap", this.normNeutro);
				}
			}
		}
	}

	private void SetDebugDiff(bool yes)
	{
		if (yes)
		{
			this.DaColor.enabled = this.showDebugDiffuse;
			this.DaColor.LutTexture = this.DiffCheck;
			global::UnityEngine.RenderSettings.ambientLight = global::UnityEngine.Color.white;
			this.untextured = true;
			global::mset.SkyManager.Get().ShowSkybox = false;
			for (int i = 0; i < this.lights.Length; i++)
			{
				if (this.lights[i])
				{
					this.lights[i].intensity = 0f;
				}
			}
			this.setMaterials(true);
		}
		else
		{
			this.DaColor.LutTexture = this.ColCorrection;
			this.DaColor.enabled = this.showColorGrading;
			global::UnityEngine.RenderSettings.ambientLight = global::UnityEngine.Color.black;
			this.setGrey(this.untextured);
			for (int j = 0; j < this.lights.Length; j++)
			{
				if (this.lights[j])
				{
					this.lights[j].intensity = this.LightIntensity[j];
				}
			}
			this.setMaterials(false);
		}
	}

	private void SetDebugSpec()
	{
		this.setNormal(true);
		this.setBlack(true);
		for (int i = 0; i < this.meshes.Length; i++)
		{
			if (this.diffTextures[i])
			{
				this.meshes[i].material.SetTexture("_MainTex", this.blackTex);
			}
		}
	}

	private void EnableRotation(bool yes)
	{
		if (yes)
		{
			this.Imported.GetComponent<global::Prometheus.Rotate>().enabled = true;
		}
		else
		{
			this.Imported.GetComponent<global::Prometheus.Rotate>().enabled = false;
		}
	}

	private void OnGUI()
	{
		global::UnityEngine.Rect pixelRect = base.GetComponent<global::UnityEngine.Camera>().pixelRect;
		pixelRect.y = base.GetComponent<global::UnityEngine.Camera>().pixelRect.height * 0.87f;
		pixelRect.height = base.GetComponent<global::UnityEngine.Camera>().pixelRect.height * 0.06f;
		global::UnityEngine.GUI.color = global::UnityEngine.Color.white;
		if (this.showGUI)
		{
			global::UnityEngine.GUILayout.BeginArea(new global::UnityEngine.Rect(0f, (float)(global::UnityEngine.Screen.height / 4), 250f, (float)global::UnityEngine.Screen.height));
			bool flag = this.showNormal;
			this.showNormal = global::UnityEngine.GUILayout.Toggle(this.showNormal, "Normal Map", new global::UnityEngine.GUILayoutOption[0]);
			if (flag != this.showNormal)
			{
				this.setNormal(this.showNormal);
			}
			if (!this.showDebugSpecular)
			{
				flag = this.untextured;
				this.untextured = global::UnityEngine.GUILayout.Toggle(this.untextured, "Diffuse/Grey", new global::UnityEngine.GUILayoutOption[0]);
				this.setGrey(this.untextured);
			}
			flag = this.showSpecular;
			this.showSpecular = global::UnityEngine.GUILayout.Toggle(this.showSpecular, "Specular", new global::UnityEngine.GUILayoutOption[0]);
			if (flag != this.showSpecular)
			{
				this.setBlack(this.showSpecular);
			}
			flag = this.showAmbienceObscurance;
			this.showAmbienceObscurance = global::UnityEngine.GUILayout.Toggle(this.showAmbienceObscurance, "Ambience Obscurance", new global::UnityEngine.GUILayoutOption[0]);
			flag = this.showColorGrading;
			this.showColorGrading = global::UnityEngine.GUILayout.Toggle(this.showColorGrading, "AmplifyColor", new global::UnityEngine.GUILayoutOption[0]);
			if (flag != this.showColorGrading)
			{
				this.DaColor.enabled = this.showColorGrading;
				this.DaColor.LutTexture = this.ColCorrection;
			}
			flag = this.showFilmToneMapping;
			this.showFilmToneMapping = global::UnityEngine.GUILayout.Toggle(this.showFilmToneMapping, "Film tonemapping", new global::UnityEngine.GUILayoutOption[0]);
			if (flag != this.showFilmToneMapping)
			{
				this.DaToneMapping.enabled = this.showFilmToneMapping;
			}
			bool flag2 = this.showDebugDiffuse;
			this.showDebugDiffuse = global::UnityEngine.GUILayout.Toggle(this.showDebugDiffuse, "Diffuse debugging", new global::UnityEngine.GUILayoutOption[0]);
			if (flag2 != this.showDebugDiffuse)
			{
				this.SetDebugDiff(this.showDebugDiffuse);
			}
			bool flag3 = this.showDebugSpecular;
			this.showDebugSpecular = global::UnityEngine.GUILayout.Toggle(this.showDebugSpecular, "Spec debugging", new global::UnityEngine.GUILayoutOption[0]);
			if (flag3 != this.showDebugSpecular)
			{
				this.SetDebugSpec();
			}
			flag = this.rotate;
			this.rotate = global::UnityEngine.GUILayout.Toggle(this.rotate, "Rotate model", new global::UnityEngine.GUILayoutOption[0]);
			if (flag != this.rotate)
			{
				this.EnableRotation(this.rotate);
			}
			global::UnityEngine.GUILayout.EndArea();
			global::UnityEngine.GUILayout.BeginArea(new global::UnityEngine.Rect((float)(global::UnityEngine.Screen.width - 100), (float)(global::UnityEngine.Screen.height / 4), 250f, (float)global::UnityEngine.Screen.height));
			flag = this.background;
			this.background = global::UnityEngine.GUILayout.Toggle(this.background, "Skybox", new global::UnityEngine.GUILayoutOption[0]);
			if (flag != this.background)
			{
				this.setBackground(this.background);
			}
			for (int i = 0; i < this.skies.Length; i++)
			{
				if (global::UnityEngine.GUILayout.Button(this.skies[i].name, new global::UnityEngine.GUILayoutOption[]
				{
					global::UnityEngine.GUILayout.Width(100f)
				}))
				{
					this.skies[i].Apply();
				}
			}
			foreach (global::UnityEngine.GameObject gameObject in this.fogs)
			{
				gameObject.SetActive(global::UnityEngine.GUILayout.Toggle(gameObject.activeSelf, gameObject.name, new global::UnityEngine.GUILayoutOption[0]));
			}
			global::UnityEngine.GUILayout.Label("Master: " + global::UnityEngine.Mathf.CeilToInt(this.exposure * 100f) + "%", new global::UnityEngine.GUILayoutOption[0]);
			float num = global::UnityEngine.Mathf.Sqrt(this.exposure);
			num = global::UnityEngine.GUILayout.HorizontalSlider(num, 0f, 2f, new global::UnityEngine.GUILayoutOption[]
			{
				global::UnityEngine.GUILayout.Width(100f)
			});
			this.exposure = num * num;
			this.setExposures(this.exposure);
			global::UnityEngine.GUILayout.Label("CamExp: " + global::UnityEngine.Mathf.CeilToInt(this.Camexposure * 100f) + "%", new global::UnityEngine.GUILayoutOption[0]);
			float num2 = global::UnityEngine.Mathf.Sqrt(this.Camexposure);
			num2 = global::UnityEngine.GUILayout.HorizontalSlider(num2, 0f, 2f, new global::UnityEngine.GUILayoutOption[]
			{
				global::UnityEngine.GUILayout.Width(100f)
			});
			this.Camexposure = num2 * num2;
			this.setCamExposures(this.Camexposure);
			global::UnityEngine.GUILayout.Label("Diff: " + global::UnityEngine.Mathf.CeilToInt(this.Diffexposure * 100f) + "%", new global::UnityEngine.GUILayoutOption[0]);
			float num3 = global::UnityEngine.Mathf.Sqrt(this.Diffexposure);
			num3 = global::UnityEngine.GUILayout.HorizontalSlider(num3, 0f, 2f, new global::UnityEngine.GUILayoutOption[]
			{
				global::UnityEngine.GUILayout.Width(100f)
			});
			this.Diffexposure = num3 * num3;
			this.setDiffExposures(this.Diffexposure);
			global::UnityEngine.GUILayout.Label("Spec: " + global::UnityEngine.Mathf.CeilToInt(this.Specexposure * 100f) + "%", new global::UnityEngine.GUILayoutOption[0]);
			float num4 = global::UnityEngine.Mathf.Sqrt(this.Specexposure);
			num4 = global::UnityEngine.GUILayout.HorizontalSlider(num4, 0f, 2f, new global::UnityEngine.GUILayoutOption[]
			{
				global::UnityEngine.GUILayout.Width(100f)
			});
			this.Specexposure = num4 * num4;
			this.setSpecExposures(this.Specexposure);
			global::UnityEngine.GUILayout.EndArea();
		}
	}

	public global::mset.Sky[] skies;

	public global::UnityEngine.Light[] lights;

	public float[] LightIntensity;

	private bool background = true;

	private int currentSky;

	private bool showSpecular;

	private bool showNormal;

	private bool untextured;

	private bool showAmbienceObscurance;

	private bool showColorGrading;

	private bool showFilmToneMapping;

	private bool showDebugDiffuse;

	private bool showDebugSpecular;

	private float exposure;

	private float Camexposure;

	private float Diffexposure;

	private float Specexposure;

	private float glow;

	public global::UnityEngine.Renderer[] meshes;

	private global::UnityEngine.Texture[] diffTextures;

	private global::UnityEngine.Texture[] specTextures;

	private global::UnityEngine.Texture[] glowTextures;

	private global::UnityEngine.Texture[] NormalTextures;

	public global::UnityEngine.Texture2D greyTex;

	public global::UnityEngine.Texture2D blackTex;

	public global::UnityEngine.Texture2D normNeutro;

	private global::Filmic DaToneMapping;

	private global::AmplifyColorEffect DaColor;

	public bool showGUI = true;

	public global::UnityEngine.Texture2D DiffCheck;

	public global::UnityEngine.Texture2D ColCorrection;

	public global::UnityEngine.Material[] materials;

	public global::UnityEngine.Material[] RawDif;

	public global::UnityEngine.Shader shaderDiff;

	private global::UnityEngine.GameObject Imported;

	private bool rotate;

	public global::UnityEngine.GameObject[] fogs;
}
