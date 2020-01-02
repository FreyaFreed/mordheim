using System;
using HighlightingSystem;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuNode : global::UnityEngine.MonoBehaviour, global::UnityEngine.EventSystems.ISelectHandler, global::UnityEngine.EventSystems.IEventSystemHandler
{
	public global::HighlightingSystem.Highlighter highlightable { get; private set; }

	public bool KeepSelected { get; set; }

	public bool IsSelectable { get; set; }

	private void Awake()
	{
		this.IsSelectable = true;
		this.boxCollider = base.GetComponent<global::UnityEngine.BoxCollider>();
	}

	public bool IsOccupied()
	{
		return this.content != null;
	}

	public global::UnityEngine.GameObject GetContent()
	{
		return this.content;
	}

	public void RemoveContent()
	{
		if (this.content != null)
		{
			this.content.transform.SetParent(null);
			this.content.SetActive(false);
			this.content = null;
		}
		if (this.boxCollider != null)
		{
			this.boxCollider.enabled = true;
		}
		this.Unselect();
	}

	public void DestroyContent()
	{
		if (this.content != null)
		{
			global::UnityEngine.Object.Destroy(this.content);
			this.content = null;
		}
	}

	public void SetContent(global::UnityEngine.GameObject newContent)
	{
		this.RemoveContent();
		this.content = newContent;
		this.content.SetActive(true);
		this.content.transform.SetParent(base.transform);
		if (this.content != null)
		{
			this.content.transform.position = base.transform.position;
			this.content.transform.rotation = base.transform.rotation;
			this.Hide();
		}
		else
		{
			this.Unselect();
		}
	}

	public void SetContent(global::UnitMenuController unitCtrlr, global::MenuNode otherNode = null)
	{
		this.RemoveContent();
		this.content = unitCtrlr.gameObject;
		this.content.SetActive(true);
		this.content.transform.SetParent(base.transform);
		if (this.content != null)
		{
			global::UnityEngine.Vector3 vector = base.transform.position;
			global::UnityEngine.Quaternion quaternion = base.transform.rotation;
			if (otherNode != null)
			{
				vector = global::UnityEngine.Vector3.Lerp(vector, otherNode.transform.position, 0.5f);
				quaternion = global::UnityEngine.Quaternion.Lerp(quaternion, otherNode.transform.rotation, 0.5f);
			}
			unitCtrlr.InstantMove(vector, quaternion);
			this.Hide();
		}
		else
		{
			this.Unselect();
		}
	}

	public void Hide()
	{
	}

	public bool Visible()
	{
		return this.content;
	}

	public bool IsContent(global::UnityEngine.GameObject c)
	{
		if (this.content || !c)
		{
			return this.content == c;
		}
		return base.gameObject == c;
	}

	public bool IsContent(string gameObjectName)
	{
		return this.content && gameObjectName.Equals(this.content.name);
	}

	public void Select()
	{
		if (this.content)
		{
			this.Highlight(true, this.highLightColor);
		}
	}

	public void Unselect()
	{
		if (!this.KeepSelected && this.content)
		{
			this.Highlight(false, this.highLightColor);
		}
	}

	public void Lock()
	{
		if (this.content)
		{
			this.Highlight(true, global::UnityEngine.Color.red);
		}
	}

	public void SetHighlightColor(global::UnityEngine.Color newColor)
	{
		this.highLightColor = newColor;
	}

	private void Highlight(bool highlight, global::UnityEngine.Color color)
	{
		if (this.content)
		{
			this.highlightable = this.content.GetComponent<global::HighlightingSystem.Highlighter>();
			if (this.highlightable == null)
			{
				this.highlightable = this.content.AddComponent<global::HighlightingSystem.Highlighter>();
				this.highlightable.seeThrough = false;
			}
			if (this.highlightable)
			{
				if (highlight)
				{
					this.highlightable.ConstantOn(color, 0.25f);
				}
				else
				{
					this.highlightable.Off();
				}
			}
		}
	}

	private void Activate(bool activate, global::UnityEngine.GameObject go)
	{
		if (go != null && activate != go.activeSelf)
		{
			go.SetActive(activate);
		}
	}

	private void OnDrawGizmos()
	{
		global::PandoraUtils.DrawFacingGizmoCube(base.transform, 2f, 0.75f, 0.75f, 0f, 0f, true);
		global::UnityEngine.Gizmos.DrawIcon(base.transform.position + new global::UnityEngine.Vector3(0f, 2f, 0f), "start.png", true);
		global::UnityEngine.Gizmos.DrawSphere(base.transform.position, 0.03f);
	}

	public void OnSelect(global::UnityEngine.EventSystems.BaseEventData eventData)
	{
		this.Select();
	}

	public global::UnityEngine.GameObject content;

	private global::UnityEngine.Color highLightColor = new global::UnityEngine.Color(0.115686275f, 0.372549027f, 0.215686277f);

	private global::UnityEngine.BoxCollider boxCollider;
}
