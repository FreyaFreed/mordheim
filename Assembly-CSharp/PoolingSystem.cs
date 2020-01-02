using System;
using System.Collections.Generic;
using UnityEngine;

public class PoolingSystem<T> where T : class
{
	public PoolingSystem(global::UnityEngine.GameObject prefab, int initialSize)
	{
		this.original = prefab;
		if (typeof(T) == typeof(global::UnityEngine.GameObject))
		{
			this.isGameObject = true;
		}
		for (int i = 0; i < initialSize; i++)
		{
			global::UnityEngine.GameObject gameObject = global::UnityEngine.Object.Instantiate(this.original, global::PoolingSystem<T>.outOfTheWay, global::UnityEngine.Quaternion.identity) as global::UnityEngine.GameObject;
			gameObject.SetActive(false);
			this.availableList.Add(gameObject);
		}
	}

	public void CleanUp()
	{
		this.inUseList.TrimExcess();
		this.availableList.TrimExcess();
	}

	public int InUse
	{
		get
		{
			return this.inUseList.Count;
		}
	}

	public int Available
	{
		get
		{
			return this.availableList.Count;
		}
	}

	public void ReleaseElement(T element, bool SetOutOfTheWay)
	{
		global::UnityEngine.GameObject gameObject;
		if (this.isGameObject)
		{
			gameObject = (element as global::UnityEngine.GameObject);
		}
		else
		{
			global::UnityEngine.Component component = element as global::UnityEngine.Component;
			gameObject = component.gameObject;
		}
		if (SetOutOfTheWay)
		{
			gameObject.transform.position = global::PoolingSystem<T>.outOfTheWay;
		}
		gameObject.SetActive(false);
		this.inUseList.Remove(gameObject);
		this.availableList.Add(gameObject);
	}

	public T GetElement()
	{
		global::UnityEngine.GameObject gameObject;
		if (this.availableList.Count == 0)
		{
			gameObject = (global::UnityEngine.Object.Instantiate(this.original, global::PoolingSystem<T>.outOfTheWay, global::UnityEngine.Quaternion.identity) as global::UnityEngine.GameObject);
			gameObject.SetActive(false);
			this.inUseList.Add(gameObject);
		}
		else
		{
			gameObject = this.availableList[0];
			this.availableList.RemoveAt(0);
			this.inUseList.Add(gameObject);
		}
		gameObject.SetActive(true);
		if (this.isGameObject)
		{
			return gameObject as T;
		}
		return gameObject.GetComponent(typeof(T)) as T;
	}

	private static global::UnityEngine.Vector3 outOfTheWay = new global::UnityEngine.Vector3(20000f, 20000f, 20000f);

	private global::System.Collections.Generic.List<global::UnityEngine.GameObject> availableList = new global::System.Collections.Generic.List<global::UnityEngine.GameObject>();

	private global::System.Collections.Generic.List<global::UnityEngine.GameObject> inUseList = new global::System.Collections.Generic.List<global::UnityEngine.GameObject>();

	private global::UnityEngine.GameObject original;

	private bool isGameObject;
}
