using System;
using UnityEngine;
using UnityEngine.UI;

public class UILocalizer : global::UnityEngine.MonoBehaviour
{
	private void Start()
	{
		if (!this.waitForInit)
		{
			this.Localize();
		}
	}

	private void Update()
	{
		if (this.waitForInit && global::PandoraSingleton<global::Hephaestus>.Instance.IsInitialized())
		{
			this.Localize();
		}
	}

	private void Localize()
	{
		global::UnityEngine.UI.Text component = base.gameObject.GetComponent<global::UnityEngine.UI.Text>();
		if (component == null)
		{
			global::PandoraDebug.LogError("No component text found on object " + base.name, "GUI", this);
		}
		for (int i = 0; i < this.overrideLocale.Length; i++)
		{
			if (this.overrideLocale[i].platform == global::UnityEngine.Application.platform && !string.IsNullOrEmpty(this.overrideLocale[i].overrideString))
			{
				component.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(this.overrideLocale[i].overrideString);
				return;
			}
		}
		if (!string.IsNullOrEmpty(component.text))
		{
			component.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(component.text);
		}
		global::UnityEngine.Object.Destroy(this);
	}

	public bool waitForInit;

	public global::LocaleBuild[] overrideLocale;
}
