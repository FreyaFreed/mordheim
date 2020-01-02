using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class ContentView<TComponent, T> : global::CanvasGroupDisabler where TComponent : global::UnityEngine.MonoBehaviour
{
	public global::System.Collections.Generic.List<TComponent> Components
	{
		get
		{
			return this.components;
		}
	}

	protected virtual void Awake()
	{
		this.template.gameObject.SetActive(false);
		this.templateTransform = (this.template.transform as global::UnityEngine.RectTransform);
		if (this.container == null)
		{
			this.container = (base.transform as global::UnityEngine.RectTransform);
		}
	}

	protected virtual void OnDestroy()
	{
		this.Components.Clear();
	}

	public void OnAddEnd()
	{
		for (int i = this.currentIndex; i < this.Components.Count; i++)
		{
			TComponent tcomponent = this.Components[i];
			tcomponent.gameObject.SetActive(false);
		}
		this.currentIndex = 0;
	}

	public TComponent Add(T toAdd)
	{
		TComponent tcomponent;
		if (this.currentIndex < this.Components.Count)
		{
			tcomponent = this.Components[this.currentIndex];
			tcomponent.gameObject.SetActive(true);
		}
		else
		{
			global::UnityEngine.GameObject gameObject = this.Create(this.template);
			tcomponent = gameObject.GetComponent<TComponent>();
			this.Components.Add(tcomponent);
		}
		this.currentIndex++;
		this.OnAdd(tcomponent, toAdd);
		return tcomponent;
	}

	protected global::UnityEngine.GameObject Create(global::UnityEngine.GameObject template)
	{
		global::UnityEngine.GameObject gameObject = global::UnityEngine.Object.Instantiate<global::UnityEngine.GameObject>(template);
		gameObject.transform.SetParent(this.container);
		gameObject.transform.localScale = template.transform.localScale;
		gameObject.SetActive(true);
		return gameObject;
	}

	protected abstract void OnAdd(TComponent component, T obj);

	public global::UnityEngine.GameObject template;

	public global::UnityEngine.RectTransform templateTransform;

	public global::UnityEngine.RectTransform container;

	private readonly global::System.Collections.Generic.List<TComponent> components = new global::System.Collections.Generic.List<TComponent>();

	private int currentIndex;
}
