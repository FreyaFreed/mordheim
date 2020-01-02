using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollGroup : global::UnityEngine.MonoBehaviour, global::UnityEngine.EventSystems.ISelectHandler, global::UnityEngine.EventSystems.IEventSystemHandler
{
	public int CurrentIndex { get; private set; }

	private void Awake()
	{
		this.CurrentIndex = -1;
		if (this.scrollRect == null)
		{
			this.scrollRect = base.GetComponentInChildren<global::UnityEngine.UI.ScrollRect>();
		}
		if (this.contentRoot == null)
		{
			this.contentRoot = (global::UnityEngine.RectTransform)this.scrollRect.transform.GetChild(0);
		}
		if (this.scrollbar == null)
		{
			this.scrollbar = base.GetComponentInChildren<global::UnityEngine.UI.Scrollbar>();
		}
		this.ensureVisible = this.scrollRect.GetComponent<global::ScrollRectEnsureVisible>();
		if (this.hideBarIfEmpty)
		{
			this.scrollbar.gameObject.SetActive(false);
		}
		if (this.fixedSizeHandle)
		{
			this.scrollbar.onValueChanged.AddListener(new global::UnityEngine.Events.UnityAction<float>(this.ResizeHandle));
			this.ResizeHandle(0f);
		}
	}

	private void ResizeHandle(float value)
	{
		this.scrollbar.size = 0f;
	}

	public void Setup(global::UnityEngine.GameObject item, bool hideBarIfEmpty)
	{
		this.item = item;
		this.hideBarIfEmpty = hideBarIfEmpty;
	}

	public void ClearList()
	{
		this.CurrentIndex = -1;
		for (int i = 0; i < this.items.Count; i++)
		{
			this.items[i].transform.SetParent(null);
			this.items[i].SetActive(false);
			this.itemsToBeDestroyed.Add(this.items[i]);
		}
		this.items.Clear();
		if (this.hideBarIfEmpty && this.scrollbar != null)
		{
			this.scrollbar.gameObject.SetActive(false);
		}
		this.scrollRect.normalizedPosition = global::UnityEngine.Vector2.zero;
		global::HightlightAnimate component = base.GetComponent<global::HightlightAnimate>();
		if (component != null)
		{
			component.Deactivate();
		}
	}

	public void RemoveItemAt(int index)
	{
		if (this.items.Count > 1)
		{
			global::UnityEngine.UI.Toggle component = this.items[index].GetComponent<global::UnityEngine.UI.Toggle>();
			global::UnityEngine.UI.Navigation navigation = component.navigation;
			bool flag = component.isOn;
			if (index < this.items.Count - 2)
			{
				global::UnityEngine.UI.Toggle component2 = this.items[index + 1].GetComponent<global::UnityEngine.UI.Toggle>();
				global::UnityEngine.UI.Navigation navigation2 = component2.navigation;
				if (this.scrollRect.horizontal)
				{
					navigation2.selectOnRight = navigation.selectOnRight;
				}
				else
				{
					navigation2.selectOnUp = navigation.selectOnUp;
				}
				component2.navigation = navigation2;
				if (flag)
				{
					base.StartCoroutine(this.SelectOnNextFrame(component2));
					flag = false;
				}
			}
			if (index > 0)
			{
				global::UnityEngine.UI.Toggle component3 = this.items[index - 1].GetComponent<global::UnityEngine.UI.Toggle>();
				global::UnityEngine.UI.Navigation navigation3 = component3.navigation;
				if (this.scrollRect.horizontal)
				{
					navigation3.selectOnLeft = navigation.selectOnLeft;
				}
				else
				{
					navigation3.selectOnDown = navigation.selectOnDown;
				}
				component3.navigation = navigation3;
				if (flag)
				{
					base.StartCoroutine(this.SelectOnNextFrame(component3));
				}
			}
		}
		this.itemsToBeDestroyed.Add(this.items[index]);
		this.items.RemoveAt(index);
	}

	public global::System.Collections.IEnumerator SelectOnNextFrame(global::UnityEngine.UI.Toggle toggle)
	{
		yield return false;
		toggle.isOn = true;
		toggle.SetSelected(true);
		yield break;
	}

	public void OnEnable()
	{
		if (this.CurrentIndex != -1)
		{
			this.RealignList(true, this.CurrentIndex, true);
		}
		if (this.hideBarIfUnnecessary)
		{
			this.CheckUnnecessaryBars();
		}
	}

	public global::UnityEngine.GameObject AddToList(global::UnityEngine.UI.Selectable up, global::UnityEngine.UI.Selectable down)
	{
		if (this.CurrentIndex == -1)
		{
			this.CurrentIndex = 0;
		}
		global::UnityEngine.GameObject go = global::UnityEngine.Object.Instantiate<global::UnityEngine.GameObject>(this.item);
		go.transform.SetParent(this.contentRoot, false);
		if (this.scrollbar != null && !this.hideScrollbar)
		{
			this.scrollbar.gameObject.SetActive(true);
		}
		if (this.maxItems != 0 && this.maxItems <= this.items.Count)
		{
			this.items[0].transform.SetParent(null);
			global::UnityEngine.Object.Destroy(this.items[0]);
			this.items.RemoveAt(0);
		}
		this.items.Add(go);
		int num = this.items.Count - 1;
		global::UnityEngine.UI.Toggle[] componentsInChildren = go.GetComponentsInChildren<global::UnityEngine.UI.Toggle>(true);
		if (this.hideBarIfUnnecessary && this.scrollbar != null)
		{
			this.scrollbar.gameObject.SetActive(false);
			if (base.gameObject.activeInHierarchy)
			{
				base.StartCoroutine(this.CheckUnnecessaryBars());
			}
		}
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].onValueChanged.AddListener(delegate(bool isOn)
			{
				this.RealignList(isOn, go, false);
			});
			global::UnityEngine.UI.Navigation navigation = componentsInChildren[i].navigation;
			if (this.useNavigation)
			{
				navigation.mode = global::UnityEngine.UI.Navigation.Mode.Explicit;
				navigation.selectOnUp = up;
				navigation.selectOnDown = down;
				if (this.items.Count > 1)
				{
					global::UnityEngine.UI.Selectable component = this.items[this.items.Count - 2].GetComponent<global::UnityEngine.UI.Selectable>();
					if (component != null)
					{
						if (this.scrollRect.horizontal)
						{
							navigation.selectOnLeft = component;
						}
						else
						{
							navigation.selectOnUp = component;
						}
						global::UnityEngine.UI.Navigation navigation2 = component.navigation;
						if (this.scrollRect.horizontal)
						{
							navigation2.selectOnRight = componentsInChildren[i];
						}
						else
						{
							navigation2.selectOnDown = componentsInChildren[i];
						}
						component.navigation = navigation2;
					}
				}
			}
			else
			{
				navigation.mode = global::UnityEngine.UI.Navigation.Mode.None;
			}
			componentsInChildren[i].navigation = navigation;
		}
		if (base.gameObject.activeInHierarchy)
		{
			if (this.realignListCoroutine != null)
			{
				base.StopCoroutine(this.realignListCoroutine);
			}
			this.realignListCoroutine = base.StartCoroutine(this.RepositionScrollListOnNextFrameCoroutine());
		}
		return go;
	}

	public void RealignList(bool isOn, int index, bool force = false)
	{
		if (isOn && base.isActiveAndEnabled && this.items.Count > index)
		{
			this.CurrentIndex = index;
			if (this.ensureVisible != null && (force || global::PandoraSingleton<global::PandoraInput>.Instance.lastInputMode != global::PandoraInput.InputMode.MOUSE))
			{
				this.ensureVisible.CenterOnItem((global::UnityEngine.RectTransform)this.items[index].transform);
			}
		}
	}

	public void RealignList(bool isOn, global::UnityEngine.GameObject go, bool force = false)
	{
		if (isOn && base.isActiveAndEnabled)
		{
			for (int i = 0; i < this.items.Count; i++)
			{
				if (go == this.items[i])
				{
					this.CurrentIndex = i;
					if (this.ensureVisible != null && (force || global::PandoraSingleton<global::PandoraInput>.Instance.lastInputMode != global::PandoraInput.InputMode.MOUSE))
					{
						this.ensureVisible.CenterOnItem((global::UnityEngine.RectTransform)this.items[i].transform);
					}
					return;
				}
			}
		}
	}

	public void ResetSelection()
	{
		if (this.items.Count > 0)
		{
			this.CurrentIndex = 0;
			if (this.scrollbar != null)
			{
				switch (this.scrollbar.direction)
				{
				case global::UnityEngine.UI.Scrollbar.Direction.LeftToRight:
					this.scrollRect.verticalNormalizedPosition = ((!this.preferLatestAdded) ? 0f : 1f);
					break;
				case global::UnityEngine.UI.Scrollbar.Direction.RightToLeft:
					this.scrollRect.verticalNormalizedPosition = ((!this.preferLatestAdded) ? 1f : 0f);
					break;
				case global::UnityEngine.UI.Scrollbar.Direction.BottomToTop:
					this.scrollRect.verticalNormalizedPosition = ((!this.preferLatestAdded) ? 1f : 0f);
					break;
				case global::UnityEngine.UI.Scrollbar.Direction.TopToBottom:
					this.scrollRect.verticalNormalizedPosition = ((!this.preferLatestAdded) ? 0f : 1f);
					break;
				}
			}
			else
			{
				this.RealignList(true, 0, true);
			}
		}
	}

	public void OnSelect(global::UnityEngine.EventSystems.BaseEventData eventData)
	{
		if (this.CurrentIndex == -1)
		{
			this.CurrentIndex = 0;
		}
		if (this.CurrentIndex >= 0 && this.items.Count > 0)
		{
			this.items[this.CurrentIndex].SetSelected(true);
		}
	}

	public void HideScrollbar()
	{
		this.hideScrollbar = true;
		if (this.scrollbar != null)
		{
			this.scrollbar.gameObject.SetActive(false);
		}
	}

	public void ShowScrollbar(bool forceShow = true)
	{
		this.hideScrollbar = false;
		if (this.scrollbar != null)
		{
			if (forceShow || !this.hideBarIfUnnecessary)
			{
				this.scrollbar.gameObject.SetActive(true);
			}
			else if (this.hideBarIfUnnecessary && base.gameObject.activeInHierarchy)
			{
				base.StartCoroutine(this.CheckUnnecessaryBars());
			}
		}
	}

	private global::System.Collections.IEnumerator CheckUnnecessaryBars()
	{
		yield return null;
		yield return null;
		yield return null;
		if (this.scrollbar != null)
		{
			this.scrollbar.gameObject.SetActive(this.scrollbar.size < 0.999f && !this.hideScrollbar && this.items.Count > 0);
		}
		yield break;
	}

	public void RepositionScrollListOnNextFrame()
	{
		base.StartCoroutine(this.RepositionScrollListOnNextFrameCoroutine());
	}

	private global::System.Collections.IEnumerator RepositionScrollListOnNextFrameCoroutine()
	{
		yield return null;
		this.ResetSelection();
		yield break;
	}

	public void ForceScroll(bool isNegative, bool setSelected = true)
	{
		if (this.scrollbar != null)
		{
			global::UnityEngine.EventSystems.PointerEventData pointerEventData = new global::UnityEngine.EventSystems.PointerEventData(global::UnityEngine.EventSystems.EventSystem.current);
			if (setSelected)
			{
				pointerEventData.selectedObject = this.scrollbar.gameObject;
			}
			pointerEventData.scrollDelta = ((!isNegative) ? global::UnityEngine.Vector2.one : (-global::UnityEngine.Vector2.one));
			this.scrollRect.OnScroll(pointerEventData);
		}
	}

	private void LateUpdate()
	{
		this.DestroyItems();
	}

	public void DestroyItems()
	{
		if (this.itemsToBeDestroyed.Count > 0)
		{
			for (int i = 0; i < this.itemsToBeDestroyed.Count; i++)
			{
				global::UnityEngine.Object.Destroy(this.itemsToBeDestroyed[i]);
			}
			this.itemsToBeDestroyed.Clear();
		}
	}

	public global::UnityEngine.RectTransform contentRoot;

	public global::UnityEngine.UI.Scrollbar scrollbar;

	public global::UnityEngine.UI.ScrollRect scrollRect;

	public global::ScrollRectEnsureVisible ensureVisible;

	[global::UnityEngine.Tooltip("Maximum number of items in the list. Adding more will remove the older ones. 0 is unlimited")]
	public int maxItems;

	private global::UnityEngine.GameObject item;

	public global::System.Collections.Generic.List<global::UnityEngine.GameObject> items = new global::System.Collections.Generic.List<global::UnityEngine.GameObject>();

	public global::System.Collections.Generic.List<global::UnityEngine.GameObject> itemsToBeDestroyed = new global::System.Collections.Generic.List<global::UnityEngine.GameObject>();

	public bool hideBarIfEmpty;

	[global::UnityEngine.Tooltip("Will not show the scrollbar if content fits in the content rect.")]
	public bool hideBarIfUnnecessary;

	public bool fixedSizeHandle;

	private float midPoint;

	private bool hideScrollbar;

	public bool useNavigation = true;

	[global::UnityEngine.Tooltip("Determines if the scroll group will select focus the latest added item rather than the first.")]
	public bool preferLatestAdded;

	private global::UnityEngine.Coroutine realignListCoroutine;
}
