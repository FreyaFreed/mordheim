using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HideoutTabManager : global::PandoraSingleton<global::HideoutTabManager>
{
	private void Awake()
	{
		this.audioSource = base.GetComponent<global::UnityEngine.AudioSource>();
		this.guiCanvas = base.GetComponent<global::CanvasGroupDisabler>();
	}

	public global::System.Collections.IEnumerator Load()
	{
		this.tabRightModules = new global::System.Collections.Generic.Dictionary<global::ModuleId, global::UIModule>();
		this.tabLoaderRight.Load(this.tabRightModules);
		yield return null;
		this.tabLeftModules = new global::System.Collections.Generic.Dictionary<global::ModuleId, global::UIModule>();
		this.tabLoaderLeft.Load(this.tabLeftModules);
		yield return null;
		this.tabCenterModules = new global::System.Collections.Generic.Dictionary<global::ModuleId, global::UIModule>();
		this.tabLoaderCenter.Load(this.tabCenterModules);
		yield return null;
		this.RegisterModules(this.popupAnchor, out this.popUpModules);
		this.RegisterBgs(this.popupAnchor, out this.popUpBgs);
		yield break;
	}

	public global::System.Collections.IEnumerator ParentModules()
	{
		this.tabLoaderRight.ParentModules();
		yield return null;
		this.tabLoaderLeft.ParentModules();
		yield return null;
		this.tabLoaderCenter.ParentModules();
		yield break;
	}

	private void Update()
	{
		if (global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyDown("hide_gui", -1))
		{
			this.guiCanvas.enabled = !this.guiCanvas.enabled;
		}
	}

	public void DeactivateAllButtons()
	{
		this.button1.gameObject.SetActive(false);
		this.button2.gameObject.SetActive(false);
		this.button3.gameObject.SetActive(false);
		this.button4.gameObject.SetActive(false);
		this.button5.gameObject.SetActive(false);
	}

	private void RegisterModules(global::UnityEngine.GameObject anchor, out global::System.Collections.Generic.Dictionary<global::ModuleId, global::UIModule> tabModules)
	{
		tabModules = new global::System.Collections.Generic.Dictionary<global::ModuleId, global::UIModule>();
		global::UIModule[] componentsInChildren = anchor.GetComponentsInChildren<global::UIModule>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			tabModules[componentsInChildren[i].moduleId] = componentsInChildren[i];
		}
	}

	private void RegisterModules(global::UnityEngine.GameObject anchor, out global::System.Collections.Generic.Dictionary<global::PopupModuleId, global::UIPopupModule> tabModules)
	{
		tabModules = new global::System.Collections.Generic.Dictionary<global::PopupModuleId, global::UIPopupModule>();
		global::UIPopupModule[] componentsInChildren = anchor.GetComponentsInChildren<global::UIPopupModule>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			tabModules[componentsInChildren[i].popupModuleId] = componentsInChildren[i];
		}
	}

	private void RegisterBgs(global::UnityEngine.GameObject anchor, out global::System.Collections.Generic.Dictionary<global::PopupBgSize, global::UnityEngine.GameObject> tabModules)
	{
		tabModules = new global::System.Collections.Generic.Dictionary<global::PopupBgSize, global::UnityEngine.GameObject>();
		global::UnityEngine.UI.Image[] componentsInChildren = anchor.GetComponentsInChildren<global::UnityEngine.UI.Image>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			for (int j = 0; j < 6; j++)
			{
				if (componentsInChildren[i].gameObject.name.Contains(((global::PopupBgSize)j).ToLowerString()))
				{
					tabModules[(global::PopupBgSize)j] = componentsInChildren[i].gameObject;
				}
			}
		}
	}

	public void ActivateLeftTabModule(bool activate, params global::ModuleId[] modules)
	{
		this.ActivateTabModule(this.tabLeftModules, activate, modules);
	}

	public void ActivateRightTabModule(bool activate, params global::ModuleId[] modules)
	{
		this.ActivateTabModule(this.tabRightModules, activate, modules);
	}

	public void ActivateCenterTabModule(bool activate, params global::ModuleId[] modules)
	{
		this.ActivateTabModule(this.tabCenterModules, activate, modules);
	}

	public void ActivateLeftTabModules(bool displayBg, params global::ModuleId[] modules)
	{
		this.ActivateTabModules<global::ModuleId, global::UIModule>(this.tabLeftModules, modules);
		this.tabLeftBg.SetActive(false);
	}

	public void ActivateRightTabModules(bool displayBg, params global::ModuleId[] modules)
	{
		this.ActivateTabModules<global::ModuleId, global::UIModule>(this.tabRightModules, modules);
		this.tabRightBg.SetActive(false);
	}

	public void ActivateCenterTabModules(params global::ModuleId[] modules)
	{
		bool flag = false;
		for (int i = 0; i < modules.Length; i++)
		{
			if (modules[i] == global::ModuleId.NOTIFICATION)
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			modules.AppendItem(global::ModuleId.NOTIFICATION);
		}
		this.ActivateTabModules<global::ModuleId, global::UIModule>(this.tabCenterModules, modules);
	}

	public void ActivatePopupModules(global::PopupBgSize popupSize, params global::PopupModuleId[] modules)
	{
		for (int i = 0; i < this.popUpBgs.Count; i++)
		{
			foreach (global::System.Collections.Generic.KeyValuePair<global::PopupBgSize, global::UnityEngine.GameObject> keyValuePair in this.popUpBgs)
			{
				if (keyValuePair.Key == popupSize)
				{
					keyValuePair.Value.SetActive(true);
				}
				else
				{
					keyValuePair.Value.SetActive(false);
				}
			}
		}
		this.ActivateTabModules<global::PopupModuleId, global::UIPopupModule>(this.popUpModules, modules);
	}

	private void ActivateTabModules<T, U>(global::System.Collections.Generic.Dictionary<T, U> tab, T[] modules) where U : global::UIPopupModule
	{
		foreach (global::System.Collections.Generic.KeyValuePair<T, U> keyValuePair in tab)
		{
			this.SetModule<U>(keyValuePair.Value, global::System.Array.IndexOf<T>(modules, keyValuePair.Key) >= 0);
		}
	}

	private void ActivateTabModule(global::System.Collections.Generic.Dictionary<global::ModuleId, global::UIModule> tab, bool activate, global::ModuleId[] modules)
	{
		for (int i = 0; i < modules.Length; i++)
		{
			this.SetModule<global::UIModule>(tab[modules[i]], activate);
		}
	}

	private void SetModule<T>(T module, bool activate) where T : global::UIPopupModule
	{
		if (module.gameObject.activeSelf != activate)
		{
			module.gameObject.SetActive(activate);
		}
		module.SetInteractable(activate);
		if (activate && !module.initialized)
		{
			module.Init();
		}
	}

	public global::System.Collections.Generic.List<T> GetModulesPopup<T>(params global::PopupModuleId[] modules) where T : global::UIPopupModule
	{
		global::System.Collections.Generic.List<T> list = new global::System.Collections.Generic.List<T>();
		for (int i = 0; i < modules.Length; i++)
		{
			list.Add((T)((object)this.popUpModules[modules[i]]));
		}
		return list;
	}

	public T GetModulePopup<T>(global::PopupModuleId moduleKey) where T : global::UIPopupModule
	{
		return (T)((object)this.popUpModules[moduleKey]);
	}

	public T GetModuleLeft<T>(global::ModuleId moduleKey) where T : global::UIModule
	{
		return (T)((object)this.tabLeftModules[moduleKey]);
	}

	public T GetModuleRight<T>(global::ModuleId moduleKey) where T : global::UIModule
	{
		return (T)((object)this.tabRightModules[moduleKey]);
	}

	public T GetModuleCenter<T>(global::ModuleId moduleKey) where T : global::UIModule
	{
		return (T)((object)this.tabCenterModules[moduleKey]);
	}

	public global::UITabLoader tabLoaderLeft;

	public global::UITabLoader tabLoaderRight;

	public global::UITabLoader tabLoaderCenter;

	public global::UnityEngine.GameObject tabLeftBg;

	public global::UnityEngine.GameObject tabRightBg;

	public global::UnityEngine.GameObject popupAnchor;

	public global::ButtonGroup button1;

	public global::ButtonGroup button2;

	public global::ButtonGroup button3;

	public global::ButtonGroup button4;

	public global::ButtonGroup button5;

	public global::UnityEngine.Sprite icnBack;

	public global::UnityEngine.Sprite icnOptions;

	public global::UnityEngine.AudioSource audioSource;

	private global::System.Collections.Generic.Dictionary<global::ModuleId, global::UIModule> tabLeftModules;

	private global::System.Collections.Generic.Dictionary<global::ModuleId, global::UIModule> tabRightModules;

	private global::System.Collections.Generic.Dictionary<global::ModuleId, global::UIModule> tabCenterModules;

	private global::System.Collections.Generic.Dictionary<global::PopupModuleId, global::UIPopupModule> popUpModules;

	private global::System.Collections.Generic.Dictionary<global::PopupBgSize, global::UnityEngine.GameObject> popUpBgs;

	private global::CanvasGroupDisabler guiCanvas;
}
